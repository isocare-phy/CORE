$PBExportHeader$n_cst_agentmemb_service.sru
$PBExportComments$ทำรายการเกี่ยวกับตัวแทน
forward
global type n_cst_agentmemb_service from nonvisualobject
end type
end forward

global type n_cst_agentmemb_service from nonvisualobject
end type
global n_cst_agentmemb_service n_cst_agentmemb_service

type variables
n_ds		ids_infosliptype, ids_initslipinfo
n_ds		ids_payoutslip, ids_payoutslipdet
n_ds		ids_payinslipshln , ids_payinslipshlndet
n_ds		ids_msgerror

transaction	itr_sqlca
Exception	ithw_exception

n_cst_dbconnectservice			inv_transection
n_cst_progresscontrol			inv_progress

constant int STATUS_PENDING = 0			// รอผ่านรายการ
constant int STATUS_NORMAL	= 1			// ปกติ , ผ่านรายการแล้ว
constant int STATUS_CLOSE	= -1			// ปิด , จบ
constant int STATUS_CANCEL	= -9			// ยกเลิก

constant int success				= 1			// ทำรายการสำเร็จ
constant int failure					= -1			// ทำรายการไม่สำเร็จ
end variables

forward prototypes
public function integer of_initservice (n_cst_dbconnectservice atr_dbtrans) throws Exception
public function integer of_savereqagmemb (str_agent astr_agent) throws Exception
public function integer of_chgreqagmemb (ref str_agent astr_agent) throws Exception
public function integer of_initreqagmemb (ref str_agent astr_agent) throws Exception
public function integer of_initmembdet (ref str_agent astr_agent) throws Exception
public function integer of_initmembdet_detail (ref str_agent astr_agent) throws Exception
public function integer of_searchagmemb (ref str_agent astr_agent) throws Exception
end prototypes

public function integer of_initservice (n_cst_dbconnectservice atr_dbtrans) throws Exception;/***********************************************************
<description>
	ไว้สำหรับเริ่มข้อมูลของ service ทำรายการเกี่ยวกับปันผล
</description>

<arguments>  
	atr_dbtrans					n_cst_dbconnectservice		user object สำหรับต่อฐานข้อมูล
</arguments> 

<return> 
	1								Integer		ถ้าไม่มีข้อมูลผิดพลาด
</return> 

<usage>
	สำหรับเริ่มข้อมูลของ service ทำรายการเกี่ยวกับปันผล
	ตัวอย่าง
	
	n_cst_dbconnectservice inv_db
	lnv_service = create n_cst_divavgoperate_service
	lnv_service.of_initservice( inv_db )
	
	----------------------------------------------------------------------
	Revision History:
	Version 1.0 (Initial version) Modified Date 16/11/2010 by Godji

</usage>

***********************************************************/
itr_sqlca = atr_dbtrans.itr_dbconnection

if isnull( inv_transection ) or not isvalid( inv_transection ) then
	inv_transection = create n_cst_dbconnectservice
	inv_transection = atr_dbtrans
end if

// สร้าง Progress สำหรับแสดงสถานะการประมวลผล
inv_progress = create n_cst_progresscontrol

return success
end function

public function integer of_savereqagmemb (str_agent astr_agent) throws Exception;/***********************************************************
<description>
	ไว้สำหรับบันทึกการเพิ่มข้อมูลตัวแทนแต่ละสังกัดลูกหนี้ตัวแทน
</description>

<arguments>  
		astr_agent.xml_head		String		xml สำหรับบันทึกข้อมูลตัวแทน
</arguments> 

<return> 
	Integer		1 = success, Exception = failure
</return> 

<usage>
	
	----------------------------------------------------------------------
	Revision History:
	Version 1.0 (Initial version) Modified Date 10/02/2011 by Godji

</usage>

***********************************************************/
string ls_agentreq , ls_membgroupcode
integer li_row , li_agentstatus , li_seqno

n_ds lds_xmlhead

lds_xmlhead = create n_ds
lds_xmlhead.dataobject = 'd_agentsrv_reqmemb'
lds_xmlhead.settransobject( itr_sqlca )
lds_xmlhead.importstring( XML!, astr_agent.xml_head )

li_row		= lds_xmlhead.rowcount()

if li_row < 1 then
	ithw_exception.text += "~nไม่พบข้อมูลตัวแทน กรุณาตรวจสอบ"
	throw ithw_exception
end if

ls_agentreq				= lds_xmlhead.object.agentrequest_no[ li_row ]
ls_membgroupcode	= lds_xmlhead.object.membgroup_code[ li_row ]
li_agentstatus 			= lds_xmlhead.object.agent_status[ li_row ]

// ตรวจสอบสถานะการลาออก
//if li_agentstatus = -1 then ; ithw_exception.text += "~nตัวแทนลาออกไปแล้วไม่สามารถเปลี่ยนแปลงรายการได้ กรุณาตรวจสอบ" ; throw ithw_exception ; end if

// gen ลำดับการคีย์
if lower(ls_agentreq) = 'auto' then
	setnull( ls_agentreq ) ; setnull(li_seqno)
	select max(agentrequest_no) into :ls_agentreq from agmembagent using itr_sqlca;
	select max(seq_no) into :li_seqno from agmembagent where membgroup_code = :ls_membgroupcode using itr_sqlca;
	if isnull( ls_agentreq ) then ls_agentreq = '00000'
	if isnull( li_seqno ) then li_seqno = 0
	ls_agentreq = '00000' + string( integer( ls_agentreq ) + 1 )
	ls_agentreq = right( ls_agentreq , 5 )
	li_seqno++
	lds_xmlhead.object.agentrequest_no[ li_row ]		= ls_agentreq
	lds_xmlhead.object.seq_no[ li_row ]					= li_seqno
	lds_xmlhead.object.agent_status[ li_row ]			= 1
else
	delete from agmembagent where agentrequest_no = :ls_agentreq using itr_sqlca;
	if itr_sqlca.sqlcode <> 0 then
		ithw_exception.text += "~nไม่สามารถแก้ไขรายการ agmembagent ได้~n" + lds_xmlhead.of_geterrormessage()
		rollback using itr_sqlca ;
		throw ithw_exception
	end if
end if

// ทำรายการบันทึกข้อมูล
if lds_xmlhead.update() <> 1 then
	ithw_exception.text += "~nไม่สามารถทำรายการ insert agmembagent ได้~n" + lds_xmlhead.of_geterrormessage()
	rollback using itr_sqlca ;
	throw ithw_exception
end if

commit using itr_sqlca ;

destroy(lds_xmlhead)

return 1
end function

public function integer of_chgreqagmemb (ref str_agent astr_agent) throws Exception;/***********************************************************
<description>
	ไว้สำหรับ itemchage เพิ่มข้อมูลตัวแทนแต่ละสังกัดลูกหนี้ตัวแทน
</description>

<arguments>  
		astr_agent.xml_head{ref}			String		xml สำหรับบันทึกข้อมูลตัวแทน
		astr_agent.column						String		ชื่อคอลัมที่ต้องการ change
		astr_agent.row							Integer	แถวที่ต้องการ change
</arguments> 

<return> 
	Integer		1 = success, Exception = failure
</return> 

<usage>
	
	----------------------------------------------------------------------
	Revision History:
	Version 1.0 (Initial version) Modified Date 11/02/2011 by Godji

</usage>

***********************************************************/
string ls_column , ls_prenamecode , ls_membname , ls_membsurname
string ls_picturename , ls_signaturename , ls_memtel , ls_memtelmobile , ls_membgroupcode
string ls_emailaddress , ls_memno
string ls_membaddr , ls_addrgroup , ls_soi , ls_mooban , ls_road , ls_tambol , ls_districtcode , ls_provincecode , ls_postcode
integer li_row

n_ds lds_xmlhead

lds_xmlhead = create n_ds
lds_xmlhead.dataobject = 'd_agentsrv_reqmemb'
lds_xmlhead.settransobject( itr_sqlca )
lds_xmlhead.importstring( XML!, astr_agent.xml_head )

ls_column	= astr_agent.column
li_row			= astr_agent.row

choose case lower(ls_column)
	case 'member_no'
		// นำข้อมูลจากทะเบียนเข้า
		ls_memno		= lds_xmlhead.object.member_no[ li_row ]
		
		select 	prename_code , 		memb_name , 			memb_surname , 		picture_name , 		signature_name , 
					mem_tel , 				mem_telmobile , 		email_address ,		membgroup_code ,	memb_addr , 
					addr_group , 			soi ,						mooban , 				road , 					tambol , 
					district_code , 			province_code , 		postcode
		into 		:ls_prenamecode , 	:ls_membname , 		:ls_membsurname , 	:ls_picturename , 		:ls_signaturename , 
					:ls_memtel , 			:ls_memtelmobile , 	:ls_emailaddress ,		:ls_membgroupcode ,	:ls_membaddr , 
					:ls_addrgroup , 		:ls_soi ,					:ls_mooban , 			:ls_road , 				:ls_tambol , 
					:ls_districtcode , 		:ls_provincecode , 	:ls_postcode
		from 		mbmembmaster
		where 	member_no = :ls_memno
		using 		itr_sqlca;
		
		if itr_sqlca.sqlcode = 0 then
			lds_xmlhead.object.membgroup_code[ li_row ]	= ls_membgroupcode
			lds_xmlhead.object.prename_code[ li_row ]		= ls_prenamecode
			lds_xmlhead.object.memb_name[ li_row ]			= ls_membname
			lds_xmlhead.object.memb_surname[ li_row ]		= ls_membsurname
			lds_xmlhead.object.picture_name[ li_row ]			= ls_picturename
			lds_xmlhead.object.signature_name[ li_row ]		= ls_signaturename
			lds_xmlhead.object.mem_tel[ li_row ]				= ls_memtel
			lds_xmlhead.object.mem_telmobile[ li_row ]		= ls_memtelmobile
			lds_xmlhead.object.email_address[ li_row ]			= ls_emailaddress
			lds_xmlhead.object.memb_addr[ li_row ]			= ls_membaddr
			lds_xmlhead.object.addr_group[ li_row ]				= ls_addrgroup
			lds_xmlhead.object.soi[ li_row ]						= ls_soi
			lds_xmlhead.object.mooban[ li_row ]					= ls_mooban
			lds_xmlhead.object.road[ li_row ]						= ls_road
			lds_xmlhead.object.tambol[ li_row ]					= ls_tambol
			lds_xmlhead.object.district_code[ li_row ]			= ls_districtcode
			lds_xmlhead.object.province_code[ li_row ]			= ls_provincecode
			lds_xmlhead.object.postcode[ li_row ]				= ls_postcode

		end if
	case 'b_resign'
		lds_xmlhead.object.agent_status[ li_row ]				= -1
	case 'b_ccresign'
		lds_xmlhead.object.agent_status[ li_row ]				= 1
	case else
		ithw_exception.text += "~nไม่พบรหัสการเปลี่ยนแปลงข้อมูล"
		throw ithw_exception
end choose

astr_agent.xml_head		= lds_xmlhead.describe( "Datawindow.data.XML" )

destroy(lds_xmlhead)

return 1
end function

public function integer of_initreqagmemb (ref str_agent astr_agent) throws Exception;/***********************************************************
<description>
	ไว้สำหรับเพิ่มข้อมูลตัวแทนแต่ละสังกัดลูกหนี้ตัวแทน
</description>

<arguments>  
		astr_agent.xml_head{ref}			String		xml สำหรับบันทึกข้อมูลตัวแทน
</arguments> 

<return> 
	Integer		1 = success, Exception = failure
</return> 

<usage>
	
	----------------------------------------------------------------------
	Revision History:
	Version 1.0 (Initial version) Modified Date 10/02/2011 by Godji

</usage>

***********************************************************/
string ls_agentreq

n_ds lds_xmlhead

if isnull( astr_agent.xml_head ) or len( trim( astr_agent.xml_head ) ) <= 0 then
	ithw_exception.text += "~nไม่สามารถดึงรายการตัวแทนได้"
	throw ithw_exception
end if

lds_xmlhead = create n_ds
lds_xmlhead.dataobject = 'd_agentsrv_reqmemb'
lds_xmlhead.settransobject( itr_sqlca )
lds_xmlhead.importstring( XML!, astr_agent.xml_head )

ls_agentreq			= lds_xmlhead.object.agentrequest_no[ 1 ] ; if isnull( ls_agentreq ) or lower(ls_agentreq) = "auto" then ls_agentreq = ""

if len( ls_agentreq ) < 1 then
	lds_xmlhead.object.agentrequest_no[ 1 ]	= "auto"
	lds_xmlhead.object.agent_status[ 1 ]			= 8
else
	lds_xmlhead.retrieve( ls_agentreq )
end if

astr_agent.xml_head		= lds_xmlhead.describe( "Datawindow.data.XML" )

destroy(lds_xmlhead)

return 1
end function

public function integer of_initmembdet (ref str_agent astr_agent) throws Exception;/***********************************************************
<description>
	ไว้สำหรับแสดงข้อมูลตัวแทนแต่ละสังกัดลูกหนี้ตัวแทน
</description>

<arguments>  
		astr_agent.xml_head					String		xml สำหรับบันทึกข้อมูลตัวแทน
		astr_agent.xml_list{ref}				String		xml สำหรับแสดงรายละเอียดตัวแทน
</arguments> 

<return> 
	Integer		1 = success, Exception = failure
</return> 

<usage>
	
	----------------------------------------------------------------------
	Revision History:
	Version 1.0 (Initial version) Modified Date 12/02/2011 by Godji

</usage>

***********************************************************/
string ls_dataobj
integer li_choice , li_row

n_ds lds_xmlhead , lds_xmllist

if isnull( astr_agent.xml_head ) or len( trim( astr_agent.xml_head ) ) <= 0 then
	ithw_exception.text += "~nไม่สามารถดึงรายการสังกัดตัวแทนได้"
	throw ithw_exception
end if

lds_xmlhead = create n_ds
lds_xmlhead.dataobject = 'd_agentsrv_membdet_main'
lds_xmlhead.settransobject( itr_sqlca )
lds_xmlhead.importstring( XML!, astr_agent.xml_head )

li_choice			= lds_xmlhead.object.choice[ 1 ]

choose case li_choice
	case 1  // สังกัดสมาชิก
		ls_dataobj = "d_agentsrv_membdetgrp_list"
	case 2  // สังกัดลูกหนี้ตัวแทน
		ls_dataobj = "d_agentsrv_membdetaggrp_list"
	case else
		ithw_exception.text += "~nกรุณาเลือกสังกัด"
		throw ithw_exception
end choose

lds_xmllist = create n_ds
lds_xmllist.dataobject = ls_dataobj
lds_xmllist.settransobject( itr_sqlca )
lds_xmllist.retrieve()

if li_choice = 1 then astr_agent.membgroup_code	= lds_xmllist.object.membgroup_code[ 1 ]
if li_choice = 2 then astr_agent.agentgrp_code = lds_xmllist.object.agentgrp_code[ 1 ]

this.of_initmembdet_detail( astr_agent )

astr_agent.xml_list		= lds_xmllist.describe( "Datawindow.data.XML" )

destroy(lds_xmlhead)
destroy(lds_xmllist)

return 1
end function

public function integer of_initmembdet_detail (ref str_agent astr_agent) throws Exception;/***********************************************************
<description>
	ไว้สำหรับแสดงรายละเอียดข้อมูลตัวแทนแต่ละสังกัดลูกหนี้ตัวแทน
</description>

<arguments>  
		astr_agent.xml_head				String		xml สำหรับบันทึกข้อมูลตัวแทน
		astr_agent.xml_detail{ref}		String		xml สำหรับแสดงรายละเอียดข้อมูลตัวแทน
		astr_agent.membgroup_code	String		สังกัด
		astr_agent.agentgrp_code		String		สังกัดลูกหนี้ตัวแทน
</arguments> 

<return> 
	Integer		1 = success, Exception = failure
</return> 

<usage>
	
	----------------------------------------------------------------------
	Revision History:
	Version 1.0 (Initial version) Modified Date 12/02/2011 by Godji

</usage>

***********************************************************/
string ls_group , ls_sql , ls_order
integer li_choice , li_ret

n_ds lds_xmlhead , lds_xmldetail

lds_xmlhead = create n_ds
lds_xmlhead.dataobject = 'd_agentsrv_membdet_main'
lds_xmlhead.settransobject( itr_sqlca )
lds_xmlhead.importstring( XML!, astr_agent.xml_head )

lds_xmldetail = create n_ds
lds_xmldetail.dataobject = 'd_agentsrv_membdet_detail'
lds_xmldetail.settransobject( itr_sqlca )
ls_sql			= lds_xmldetail.getsqlselect()

li_choice		= lds_xmlhead.object.choice[1]

choose case li_choice
	case 1
		ls_group		= " and membgroup_code = '" + astr_agent.membgroup_code + "' order by membgroup_code "
	case 2
		ls_group		= " and agentgrp_code = '" +  + astr_agent.agentgrp_code + "' order by agentgrp_code "
end choose

ls_sql			+= ls_group
li_ret 			= lds_xmldetail.setsqlselect( ls_sql )
if ( li_ret <> 1 ) then
	ithw_exception.text += "~nไม่สามารถดึงรายการรายละเอียดตัวแทนได้"
	throw ithw_exception
end if	

lds_xmldetail.retrieve()

astr_agent.xml_detail		= lds_xmldetail.describe( "Datawindow.data.XML" )

destroy(lds_xmlhead)
destroy(lds_xmldetail)

return 1
end function

public function integer of_searchagmemb (ref str_agent astr_agent) throws Exception;/***********************************************************
<description>
	ไว้สำหรับแสดงรายการที่ค้นหาตัวแทนตามข้อมูลที่กำหนด
</description>

<arguments>  
	astr_agent.xml_head			String			xml datawindows ส่วนข้อมูลที่ต้องการค้นหา
	astr_agent.xml_list{ref}		String			xml datawindows รายชื่อทะเบียนลูกหนี้ตัวแทนที่ค้นหา
</arguments> 

<return> 
	Integer		1 = success, Exception = failure
</return> 

<usage>

	----------------------------------------------------------------------
	Revision History:
	Version 1.0 (Initial version) Modified Date 12/02/2011 by Godji

</usage>

***********************************************************/
string ls_xmlhead
string ls_sql , ls_sqlext , ls_temp , ls_sort
string ls_memberno , ls_membname , ls_membsurname , ls_membgroupcode
string ls_cardperson , ls_agentgrpcode , ls_memtel , ls_memtelmobile
string ls_emailaddress , ls_remark , ls_agentrequestno

n_ds lds_xmlhead , lds_xmllist

ls_xmlhead		= astr_agent.xml_head

lds_xmlhead	 = create n_ds
lds_xmlhead.dataobject = "d_agentsrv_searchagmemb_main"
lds_xmlhead.settransobject( itr_sqlca )

lds_xmlhead.importstring( XML!, ls_xmlhead )

if lds_xmlhead.rowcount() < 1 then
	ithw_exception.text += "~nเนื่องจากไม่ได้รับรายละเอียดเงื่อนไขการดึงรายการ~nไม่สามารถดึงรายการค้นหาลูกหนี้ตัวแทนได้"
	throw ithw_exception
	return -1
end if

// รายการ search
lds_xmllist		= create n_ds
lds_xmllist.dataobject		= "d_agentsrv_searchagmemb_list"
lds_xmllist.settransobject( itr_sqlca )
ls_sql	= lds_xmllist.getsqlselect()

ls_memberno				= lds_xmlhead.object.member_no[ 1 ]					;	if isnull( ls_memberno ) then ls_memberno = ""
ls_membname				= lds_xmlhead.object.memb_name[ 1 ]					;	if isnull( ls_membname ) then ls_membname = ""
ls_membsurname			= lds_xmlhead.object.memb_surname[ 1 ]				;	if isnull( ls_membsurname ) then ls_membsurname = ""
ls_membgroupcode		= lds_xmlhead.object.membgroup_code[ 1 ]			;	if isnull( ls_membgroupcode ) then ls_membgroupcode = ""
ls_cardperson				= lds_xmlhead.object.card_person[ 1 ]					;	if isnull( ls_cardperson ) then ls_cardperson = ""
ls_agentgrpcode			= lds_xmlhead.object.agentgrp_code[ 1 ]				;	if isnull( ls_agentgrpcode ) then ls_agentgrpcode = ""
ls_memtel					= lds_xmlhead.object.mem_tel[ 1 ]						;	if isnull( ls_memtel ) then ls_memtel = ""
ls_memtelmobile			= lds_xmlhead.object.mem_telmobile[ 1 ]				;	if isnull( ls_memtelmobile ) then ls_memtelmobile = ""
ls_emailaddress			= lds_xmlhead.object.email_address[ 1 ]				;	if isnull( ls_emailaddress ) then ls_emailaddress = ""
ls_remark					= lds_xmlhead.object.remark[ 1 ]							;	if isnull( ls_remark ) then ls_remark = ""
ls_agentrequestno			= lds_xmlhead.object.agentrequest_no[ 1 ]				;	if isnull( ls_agentrequestno ) then ls_agentrequestno = ""
ls_sort						= lds_xmlhead.object.sort[ 1 ]								;	if isnull( ls_sort ) then ls_sort = ""

if len( ls_agentrequestno ) > 0 then
	ls_sqlext += " and ( agmembagent.agentrequest_no like '"+ls_agentrequestno+"%') "
end if

if len( ls_remark ) > 0 then
	ls_sqlext += " and ( agmembagent.remark like '"+ls_remark+"%') "
end if

if len( ls_emailaddress ) > 0 then
	ls_sqlext += " and ( agmembagent.email_address like '"+ls_emailaddress+"%') "
end if

if len( ls_memtelmobile ) > 0 then
	ls_sqlext += " and ( agmembagent.mem_telmobile like '"+ls_memtelmobile+"%') "
end if

if len( ls_memtel ) > 0 then
	ls_sqlext += " and ( agmembagent.mem_tel like '"+ls_memtel+"%') "
end if

if len( ls_memberno ) > 0 then
	ls_sqlext += " and ( agmembagent.member_no like '"+ls_memberno+"%') "
end if

if len( ls_membname ) > 0 then
	ls_sqlext += " and ( agmembagent.memb_name like '"+ls_membname+"%') "
end if

if len( ls_membsurname ) > 0 then
	ls_sqlext += " and ( agmembagent.memb_surname like '"+ls_membsurname+"%') "
end if

if len( ls_membgroupcode ) > 0 then
	ls_sqlext += " and ( agmembagent.membgroup_code like '"+ls_membgroupcode+"%') "
end if

if len( ls_cardperson ) > 0 then
	ls_sqlext += " and ( agmembagent.card_person like '"+ls_cardperson+"%') "
end if

if len( ls_agentgrpcode ) > 0 then
	ls_sqlext += " and ( agmembagent.agentgrp_code like '"+ls_agentgrpcode+"%') "
end if

ls_sort		= " order by " + trim( ls_sort )

ls_temp	= ls_sql + ls_sqlext + ls_sort

lds_xmllist.setsqlselect(ls_temp)

if lds_xmllist.retrieve() <= 0 then
	ithw_exception.text += "~nไม่พบข้อมูลที่ค้นหา~nกรุณาทำรายการค้นหาใหม่"
	throw ithw_exception
end if

// นำผลลัพธ์ที่ได้คืนค่ากลับ
astr_agent.xml_list	= lds_xmllist.describe( "Datawindow.data.XML" )

destroy(lds_xmlhead)
destroy(lds_xmllist)

return 1
end function

on n_cst_agentmemb_service.create
call super::create
TriggerEvent( this, "constructor" )
end on

on n_cst_agentmemb_service.destroy
TriggerEvent( this, "destructor" )
call super::destroy
end on

event constructor;ithw_exception = create Exception

end event

event destructor;if isvalid( ithw_exception ) then destroy ithw_exception
end event

