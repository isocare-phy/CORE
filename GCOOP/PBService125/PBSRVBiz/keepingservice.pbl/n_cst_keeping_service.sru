$PBExportHeader$n_cst_keeping_service.sru
forward
global type n_cst_keeping_service from NonVisualObject
end type
end forward

global type n_cst_keeping_service from NonVisualObject
end type
global n_cst_keeping_service n_cst_keeping_service

type variables
Public:

transaction						itr_sqlca
Exception						ithw_exception

n_cst_dbconnectservice		inv_transection
n_cst_dwxmlieservice			inv_dwxmliesrv
n_cst_stringservice			inv_stringsrv

string		is_coopid , is_coopcontrol , is_recvperiod
string		is_arg[] , is_sqlext
string		is_txtmemno , is_txtmemgrp , is_txtmemtyp
integer	ii_proctype
end variables

forward prototypes
private function integer of_setsrvdwxmlie (boolean ab_switch)
public function integer of_setargument (string as_arg[])
public function integer of_setproctype (integer ai_proctype)
public function integer of_set_msgerr (ref n_ds ads_msgerr, string as_msgerr)
public function integer of_getargument (ref string as_arg[])
public function integer of_getproctype (ref integer ai_proctype)
public function integer of_setsqldw (ref n_ds ads_info) throws Exception
public function integer of_setsqlselect (string as_tablename) throws Exception
public function integer of_getsqlselect (ref string as_sqlext)
public function integer of_setanalyze () throws Exception
public function integer of_set_txtmemno (string as_txtmemno)
public function integer of_set_txtmemgrp (string as_txtmemgrp)
public function integer of_set_txtmemtyp (string as_txtmemtyp)
public function integer of_setsqldw_coop (ref n_ds ads_info, string as_tablename) throws Exception
public function integer of_set_recvperiod (string as_recvperiod)
public function integer of_setsqldw_coop_recvperiod (ref n_ds ads_info, string as_tablename) throws Exception
public function integer of_initservice (n_cst_dbconnectservice anv_dbtrans) throws Exception
public function integer of_setsqldw_recvperiod (ref n_ds ads_info, string as_tablename) throws Exception
public function integer of_imp_orther_lap (string as_path_file, string as_period) throws Exception
end prototypes

private function integer of_setsrvdwxmlie (boolean ab_switch);// Check argument
if isnull( ab_switch ) then
	return -1
end if

if ab_switch then
	if isnull( inv_dwxmliesrv ) or not isvalid( inv_dwxmliesrv ) then
		inv_dwxmliesrv	= create n_cst_dwxmlieservice
		return 1
	end if
else
	if isvalid( inv_dwxmliesrv ) then
		destroy inv_dwxmliesrv
		return 1
	end if
end if

return 0
end function

public function integer of_setargument (string as_arg[]);/***********************************************************
<description>
	set ค่า argument ที่ต้องการประมวล
</description>

<arguments>

</arguments> 

<return> 
	Integer	1 = success,  Exception = failure
</return> 

<usage> 
	
	Revision History:
	Version 1.0 (Initial version) Modified Date 09/05/2012 by Godji
</usage> 
***********************************************************/
is_arg[]		= as_arg[]

return 1
end function

public function integer of_setproctype (integer ai_proctype);/***********************************************************
<description>
	set ค่าประเภทการคำนวณ
</description>

<arguments>

</arguments> 

<return> 
	Integer	1 = success,  Exception = failure
</return> 

<usage> 
	
	Revision History:
	Version 1.0 (Initial version) Modified Date 09/05/2012 by Godji
</usage> 
***********************************************************/
ii_proctype		= ai_proctype

return 1
end function

public function integer of_set_msgerr (ref n_ds ads_msgerr, string as_msgerr);/***********************************************************
<description>
	Message Error :-> รายการ
</description>

<arguments>  

</arguments> 

<return> 
	Integer	1 = success,  Exception = failure
</return> 

<usage> 
	
	Revision History:
	Version 1.0 (Initial version) Modified Date 08/02/2012 by Godji
</usage> 
***********************************************************/
long ll_row

if isnull( as_msgerr ) or as_msgerr = "" then return -1
as_msgerr	= left( trim( as_msgerr ) , 150 )

ll_row		= ads_msgerr.insertrow(0)

ads_msgerr.object.message[ll_row]	= as_msgerr

return 1
end function

public function integer of_getargument (ref string as_arg[]);as_arg[]		= is_arg[]

return 1
end function

public function integer of_getproctype (ref integer ai_proctype);ai_proctype		= ii_proctype

return 1
end function

public function integer of_setsqldw (ref n_ds ads_info) throws Exception;/***********************************************************
<description>
	นำชุด sql ที่สร้างมานำเข้า dw
</description>

<arguments>  
	ads_info			n_ds		datastore ข้อมูลรายการที่จะทำการจัดเก็บ
</arguments> 

<return> 
	integer		1 = success, -1 = failure
</return> 

<usage> 
		
	Revision History:
	Version 1.0 (Initial version) Modified Date 23/04/2012 by Godji
</usage> 
***********************************************************/
string		ls_temp
integer	li_pos
integer	li_ret

ads_info.settransobject( itr_sqlca )

if is_sqlext <> "" then
//	แบบเดิม	
//	ls_temp	= ads_info.getsqlselect()
//	ls_temp	+= is_sqlext
	
	ls_temp	= ads_info.getsqlselect()
	li_pos		= pos( ls_temp , 'GROUP BY' )
	if isnull( li_pos ) or li_pos = 0 then ls_temp	+= is_sqlext
	if li_pos > 0 then ls_temp = mid( ls_temp , 1 , li_pos - 1 ) + ' ' + is_sqlext + ' ' + mid( ls_temp , li_pos , len( ls_temp ))
	
	li_ret = ads_info.setsqlselect( ls_temp )
	
	if ( li_ret <> 1 ) then
		ithw_exception.text += "~r~n" + ads_info.of_geterrormessage()
		return -1
	end if	
end if

return 1
end function

public function integer of_setsqlselect (string as_tablename) throws Exception;/***********************************************************
<description>
	สร้างชุด sql ในส่วน Where เพิ่มเติมสำหรับการประมวลผล 
	จากการเลือกช่วงสังกัด หรือ ช่วงเลขสมาชิก
</description>

<arguments>  
	ads_info			n_ds		datastore ข้อมูลรายการที่จะทำการจัดเก็บ
</arguments> 

<return> 
	integer		1 = success, -1 = failure
</return> 

<usage> 
		
	Revision History:
	Version 1.0 (Initial version) Modified Date 28/9/2010 by MiT
</usage> 
***********************************************************/
string		ls_tablename , ls_column
string		ls_temp

is_sqlext				= ""
ls_tablename		= as_tablename

if len( ls_tablename ) = 0 or ls_tablename = "" or isnull( ls_tablename ) then
	ls_column		= "mbmembmaster"
else
	ls_column		= trim( ls_tablename )
end if

choose case ii_proctype
	case 1
		is_sqlext	= ""
	case 2
		ls_column		+= ".membgroup_code"
		inv_stringsrv.of_buildsqlext( is_arg[], ls_column, is_sqlext )
		is_sqlext	= " and " + is_sqlext
	case 3
		ls_column		+= ".member_no"
		inv_stringsrv.of_buildsqlext( is_arg[], ls_column , is_sqlext )
		is_sqlext	= " and " + is_sqlext
	case 4
		ls_column		+= ".membtype_code"
		inv_stringsrv.of_buildsqlext( is_arg[], ls_column, is_sqlext )
		is_sqlext	= " and " + is_sqlext	
end choose

return 1
end function

public function integer of_getsqlselect (ref string as_sqlext);as_sqlext			= is_sqlext

return 1
end function

public function integer of_setanalyze () throws Exception;/***********************************************************
<description>
	set ค่าตามประเภทที่ต้องการคำนวณ
</description>

<arguments>

</arguments> 

<return> 
	Integer	1 = success,  Exception = failure
</return> 

<usage> 
	
	Revision History:
	Version 1.0 (Initial version) Modified Date 09/05/2012 by Godji
</usage> 
***********************************************************/
// รูปแบบการประมวลผล
choose case ii_proctype
	case 2	// ตามกลุ่ม
		inv_stringsrv.of_analyzestring( is_txtmemgrp, is_arg[] )
	case 3 	// ตามทะเบียน
		inv_stringsrv.of_analyzestring( is_txtmemno, is_arg[] )
	case 4	// ตามประเภทสมาชิก
		inv_stringsrv.of_analyzestring( is_txtmemtyp, is_arg[] )
end choose

return 1
end function

public function integer of_set_txtmemno (string as_txtmemno);/***********************************************************
<description>
	set ค่าทะเบียนสมาชิกที่ต้องการประมวล
</description>

<arguments>

</arguments> 

<return> 
	Integer	1 = success,  Exception = failure
</return> 

<usage> 
	
	Revision History:
	Version 1.0 (Initial version) Modified Date 09/05/2012 by Godji
</usage> 
***********************************************************/
is_txtmemno	= as_txtmemno
return 1
end function

public function integer of_set_txtmemgrp (string as_txtmemgrp);/***********************************************************
<description>
	set ค่าสังกัดสมาชิกที่ต้องการประมวล
</description>

<arguments>

</arguments> 

<return> 
	Integer	1 = success,  Exception = failure
</return> 

<usage> 
	
	Revision History:
	Version 1.0 (Initial version) Modified Date 09/05/2012 by Godji
</usage> 
***********************************************************/
is_txtmemgrp	= as_txtmemgrp
return 1
end function

public function integer of_set_txtmemtyp (string as_txtmemtyp);/***********************************************************
<description>
	set ค่าประเภทสมาชิกที่ต้องการประมวล
</description>

<arguments>

</arguments> 

<return> 
	Integer	1 = success,  Exception = failure
</return> 

<usage> 
	
	Revision History:
	Version 1.0 (Initial version) Modified Date 09/05/2012 by Godji
</usage> 
***********************************************************/
is_txtmemtyp	= as_txtmemtyp
return 1
end function

public function integer of_setsqldw_coop (ref n_ds ads_info, string as_tablename) throws Exception;/***********************************************************
<description>
	นำชุด sql ที่สร้างมานำเข้า dw
</description>

<arguments>  
	ads_info			n_ds		datastore ข้อมูลรายการที่จะทำการจัดเก็บ
	as_tablename	String		table ที่ต้องการเชื่อม coopid
</arguments> 

<return> 
	integer		1 = success, -1 = failure
</return> 

<usage> 
		
	Revision History:
	Version 1.0 (Initial version) Modified Date 09/05/2012 by Godji
</usage> 
***********************************************************/
string		ls_temp , ls_column
integer	li_pos , li_ret

if isnull( as_tablename ) or len( trim( as_tablename )) = 0 then as_tablename = "mbmembmaster"

ls_column	= " and " + as_tablename + ".coop_id = '" + is_coopid + "'"

ads_info.settransobject( itr_sqlca )

//if is_sqlext <> "" then
//	ls_temp	= ads_info.getsqlselect()
//	ls_temp	+= ls_column
	
	ls_temp	= ads_info.getsqlselect()
	li_pos		= pos( ls_temp , 'GROUP BY' )
	if isnull( li_pos ) or li_pos = 0 then ls_temp	+= ls_column
	if li_pos > 0 then ls_temp = mid( ls_temp , 1 , li_pos - 1 ) + ' ' + ls_column + ' ' + mid( ls_temp , li_pos , len( ls_temp ))
	
	li_ret = ads_info.setsqlselect( ls_temp )
	
	if ( li_ret <> 1 ) then
		ithw_exception.text += "~r~n" + ads_info.of_geterrormessage()
		return -1
	end if	
//end if

return 1
end function

public function integer of_set_recvperiod (string as_recvperiod);/***********************************************************
<description>
	set ค่า งวดเรียกเก็บประจำเดือน
</description>

<arguments>

</arguments> 

<return> 
	Integer	1 = success,  Exception = failure
</return> 

<usage> 
	
	Revision History:
	Version 1.0 (Initial version) Modified Date 24/06/2012 by Godji
</usage> 
***********************************************************/
is_recvperiod		= as_recvperiod

return 1
end function

public function integer of_setsqldw_coop_recvperiod (ref n_ds ads_info, string as_tablename) throws Exception;/***********************************************************
<description>
	นำชุด sql ที่สร้างมานำเข้า dw
</description>

<arguments>  
	ads_info			n_ds		datastore ข้อมูลรายการที่จะทำการจัดเก็บ
	as_tablename	String		table ที่ต้องการเชื่อม coopid
</arguments> 

<return> 
	integer		1 = success, -1 = failure
</return> 

<usage> 
		
	Revision History:
	Version 1.0 (Initial version) Modified Date 09/05/2012 by Godji
</usage> 
***********************************************************/
string		ls_temp , ls_column
integer	li_pos , li_ret

if isnull( as_tablename ) or len( trim( as_tablename )) = 0 then as_tablename = "mbmembmaster"

ls_column	= " and " + as_tablename + ".coop_id = '" + is_coopid + "'"
ls_column	+= " and " + as_tablename + ".recv_period = '" + is_recvperiod + "'"

ads_info.settransobject( itr_sqlca )

//if is_sqlext <> "" then
//	ls_temp	= ads_info.getsqlselect()
//	ls_temp	+= ls_column
	
	ls_temp	= ads_info.getsqlselect()
	li_pos		= pos( ls_temp , 'GROUP BY' )
	if isnull( li_pos ) or li_pos = 0 then ls_temp	+= ls_column
	if li_pos > 0 then ls_temp = mid( ls_temp , 1 , li_pos - 1 ) + ' ' + ls_column + ' ' + mid( ls_temp , li_pos , len( ls_temp ))
	
	li_ret = ads_info.setsqlselect( ls_temp )
	
	if ( li_ret <> 1 ) then
		ithw_exception.text += "~r~n" + ads_info.of_geterrormessage()
		return -1
	end if	
//end if

return 1
end function

public function integer of_initservice (n_cst_dbconnectservice anv_dbtrans) throws Exception;/***********************************************************
<description>
	ใช้สำหรับ Intial service
</description>

<arguments>  
	atr_dbtrans			n_cst_dbconnectservice	รายละเอียดการเชื่อมต่อฐานข้อมูล
</arguments> 

<return> 
	1						Integer	ถ้าไม่มีข้อผิดพลาด
</return> 

<usage> 
	เรียกใช้ครั้งเดียว และต้องเรียกใช้เป็นฟังก์ชั่นแรกหลังจาก create instance
	ก่อนที่จะเรียกใช้ฟังก์ชั่นอื่น ๆ
	
	Revision History:
	Version 1.0 (Initial version) Modified Date 28/9/2010 by MiT
</usage> 
***********************************************************/
// Service Transection
if isnull( inv_transection ) or not isvalid( inv_transection ) then
	inv_transection	= create n_cst_dbconnectservice
end if

inv_transection	= anv_dbtrans
itr_sqlca 			= inv_transection.itr_dbconnection

is_coopid			= anv_dbtrans.is_coopid
is_coopcontrol	= anv_dbtrans.is_coopcontrol

//this.of_setsrvdwxmlie( true )

// Service สำหรับสร้าง Sql Extend
inv_stringsrv	= create n_cst_stringservice

return 1
end function

public function integer of_setsqldw_recvperiod (ref n_ds ads_info, string as_tablename) throws Exception;/***********************************************************
<description>
	นำชุด sql ที่สร้างมานำเข้า dw
</description>

<arguments>  
	ads_info			n_ds		datastore ข้อมูลรายการที่จะทำการจัดเก็บ
	as_tablename	String		table ที่ต้องการเชื่อม coopid
</arguments> 

<return> 
	integer		1 = success, -1 = failure
</return> 

<usage> 
		
	Revision History:
	Version 1.0 (Initial version) Modified Date 09/05/2012 by Godji
</usage> 
***********************************************************/
string		ls_temp , ls_column
integer	li_pos , li_ret

if isnull( as_tablename ) or len( trim( as_tablename )) = 0 then as_tablename = "mbmembmaster"

ls_column	+= " and " + as_tablename + ".recv_period = '" + is_recvperiod + "'"

ads_info.settransobject( itr_sqlca )

//if is_sqlext <> "" then
//	ls_temp	= ads_info.getsqlselect()
//	ls_temp	+= ls_column
	
	ls_temp	= ads_info.getsqlselect()
	li_pos		= pos( ls_temp , 'GROUP BY' )
	if isnull( li_pos ) or li_pos = 0 then ls_temp	+= ls_column
	if li_pos > 0 then ls_temp = mid( ls_temp , 1 , li_pos - 1 ) + ' ' + ls_column + ' ' + mid( ls_temp , li_pos , len( ls_temp ))
	
	li_ret = ads_info.setsqlselect( ls_temp )
	
	if ( li_ret <> 1 ) then
		ithw_exception.text += "~r~n" + ads_info.of_geterrormessage()
		return -1
	end if	
//end if

return 1
end function

public function integer of_imp_orther_lap (string as_path_file, string as_period) throws Exception;

long  row, i
integer li_seqno, li_chk, chk_member
decimal ld_money
string  ls_part, ls_desc, ls_member, ls_recvperiod, ls_ym, ls_kpslip, keep_member

ls_part = as_path_file

datastore rs_orther
rs_orther = create datastore
rs_orther.dataobject = "d_kp_import_other"
rs_orther.settransobject( itr_sqlca)	
rs_orther.importfile(ls_part, 1)
row = rs_orther.rowcount()

ls_recvperiod = trim(as_period)

for i = 1 to row
	chk_member = 0
	ls_desc		=	rs_orther.getitemstring(i, "col_a")
	ld_money	=	rs_orther.getitemdecimal(i, "col_b")
	//ls_member	=	'00' + rs_orther.getitemstring(i, "col_c") // test ข้อมูลระบบเก่า 
	ls_member	=	rs_orther.getitemstring(i, "col_c") // ใช้จริง
	
	if i = 1 then
		chk_member = 0
		keep_member = ls_member
	elseif keep_member = ls_member then
		chk_member = 1
	else
		chk_member = 0
		keep_member = ls_member
	end if
	
	select max( seq_no )
	into :li_seqno
	from kptempreceivedet where recv_period = :ls_recvperiod and member_no =  :ls_member
	using itr_sqlca;
	
	if isnull(li_seqno) then li_seqno = 0
	li_seqno = li_seqno + 1
	
	if li_seqno = 1 then
		if chk_member = 0 then
			select last_documentno + 1 R, substr(document_year, 3, 2) || case when LENGTH(to_char(sysdate,'mm')) = 1 then '0' || to_char(sysdate,'mm') else to_char(sysdate,'mm') end ym 
			into :ls_kpslip, :ls_ym from cmdocumentcontrol where document_code = 'KPSLIPNO'
			using itr_sqlca;
	
			if len(ls_kpslip) = 4 then
				ls_kpslip = ls_ym + '0000' + ls_kpslip
			elseif len(ls_kpslip) = 5 then
				ls_kpslip = ls_ym + '000' + ls_kpslip
			elseif len(ls_kpslip) = 6 then
				ls_kpslip = ls_ym + '000' + ls_kpslip
			elseif len(ls_kpslip) = 7 then
				ls_kpslip = ls_ym + '0' + ls_kpslip
			elseif len(ls_kpslip) = 8 then
				ls_kpslip = ls_kpslip
			end if
		end if
	else
		select distinct kpslip_no 
		into :ls_kpslip
		from kptempreceivedet where recv_period = :ls_recvperiod and member_no =  :ls_member
		using itr_sqlca;
	end if

	// insert ข้อมูลลง รายการเรียกเก็บ I61
	insert into kptempreceivedet
	(coop_id, kpslip_no, seq_no, memcoop_id, member_no, recv_period, refmemcoop_id, ref_membno, keepitemtype_code, shrlontype_code, description, period, item_payment, bfperiod, bfprinbalance_amt, posting_status, keepitem_status)
	values
	('027001', :ls_kpslip, :li_seqno, '027001', :ls_member, :ls_recvperiod, '027001', :ls_member, 'OTH', 'I6', :ls_desc, 0, :ld_money, 0, 0 ,0 ,1)
	using itr_sqlca;
	
	if itr_sqlca.sqlcode <> 0 then
		rollback using itr_sqlca ;
		return 0
	end if
	
	//update master
	update kptempreceive
	set receive_amt =
	(
		select sum(item_payment) item_payment from kptempreceivedet 
		where recv_period = :ls_recvperiod and member_no = :ls_member
	)
	, money_text = 
	(
		select ftreadtbaht(sum(item_payment)) from kptempreceivedet 
		where recv_period = :ls_recvperiod and member_no = :ls_member
	)
	, last_seq_no = :li_seqno
	where recv_period = :ls_recvperiod and member_no = :ls_member
	using itr_sqlca;
	
	if itr_sqlca.sqlcode <> 0 then
		rollback using itr_sqlca ;
		return 0
	end if
	
	if li_seqno = 1 then
		if chk_member = 0 then
			update cmdocumentcontrol set last_documentno = last_documentno + 1 where document_code = 'KPSLIPNO'
			using itr_sqlca;
		end if
	end if

next

commit;
return 1
end function

on n_cst_keeping_service.create
call super::create
TriggerEvent( this, "constructor" )
end on

on n_cst_keeping_service.destroy
TriggerEvent( this, "destructor" )
call super::destroy
end on

event constructor;ithw_exception = create Exception
end event

event destructor;this.of_setsrvdwxmlie( false )
if isvalid(inv_stringsrv) then destroy inv_stringsrv
end event
