$PBExportHeader$n_cst_moneyorder_service.sru
forward
global type n_cst_moneyorder_service from nonvisualobject
end type
end forward

global type n_cst_moneyorder_service from nonvisualobject
end type
global n_cst_moneyorder_service n_cst_moneyorder_service

type variables

Exception		ithw_exception
transaction		itr_sqlca

integer	success = 1
integer	failure = -1

n_cst_doccontrolservice	inv_docservice
n_cst_dbconnectservice	inv_db

end variables

forward prototypes
public function integer of_save_moneyorder (string as_moneyorder_master_xml, string as_moneyorder_detail_xml, string as_entryid, datetime adtm_entry) throws Exception
public function string of_err_generate_import_xml (integer ai_error)
public function string of_init_moneyorder (string as_moneyorder_master_xml, string as_entryid, datetime adtm_entry) throws Exception
public function integer of_approve_moneyorder (string as_moneyorder_list_xml, string as_entryid, datetime adtm_entry) throws Exception
public function integer of_init (n_cst_dbconnectservice anv_db)
private function integer of_chkbf_moneyorder (ref n_ds ads_detail, string as_ptbdocno, decimal adc_ptb)
public function integer of_operate_moneyorder (string as_ptbdocno, string as_ptbtypecode, long al_seqno, string as_ptodocno, integer ai_ptoflag, string as_entryid, datetime adtm_entry) throws Exception
public function integer of_cancelappr_moneyorder (string as_moneyorder_list_xml, string as_entryid, datetime adtm_entry) throws Exception
public function integer of_cancel_moneyorder (string as_moneyorder_list_xml, string as_entryid, datetime adtm_entry) throws Exception
public function string of_getlist_moneyorder (datetime adtm_trn) throws Exception
public function integer of_getdata_moneyorder (string as_docno, ref string as_moneyorder_master_xml, ref string as_moneyorder_detail_xml) throws Exception
public function string of_getlistcancel_moneyorder (datetime adtm_trn) throws Exception
public function string of_getlistappr_moneyorder (datetime adtm_trn) throws Exception
public function string of_getlistreappr_moneyorder (datetime adtm_trn) throws Exception
public function integer of_postusemoneyorder (string as_docno, integer ai_seqno, string as_ref_slipno, integer ai_ref_seqno, integer ai_itemstatus, string as_entry_id, datetime adtm_wdate) throws Exception
end prototypes

public function integer of_save_moneyorder (string as_moneyorder_master_xml, string as_moneyorder_detail_xml, string as_entryid, datetime adtm_entry) throws Exception;/***********************************************
<description>
เพื่อทำการสร้างและบันทึกสำหรับใบธนาณัติ ก่อนที่จะนำไปขึ้นเงินต่อไป

</description>

<arguments>
as_moneyorder_master_xml	รับข้อมูลที่เป็น XML เข้ามา เป็นตัว Master
as_moneyorder_detail_xml		รับข้อมูลที่เป็น XML เข้ามา เป็นตัว Detail
as_entryid							ข้อมูลรหัสผู้ใช้
adtm_entry							วันที่ตอนเข้าระบบ
</arguments>

<return>
integer			ส่งข้อมูลการทำงานกลับไป 1-สำเร็จ , 0-มีข้อผิดพลาด
</return>

<usage>
ยังไม่มีตัวอย่างการใช้งาน
</usage>
************************************************/

///////////////////////////////////////////
// Master
///////////////////////////////////////////

integer		li_ptbstatus , li_postflag , li_count , li_rc , li_row , li_found
dec{2}		ldc_ptb , ldc_pmfee , ldc_sumitem_amt
string			ls_ptbdocno , ls_ptbtypecode , ls_memberno , ls_mbgroupcode , ls_pmdocno , ls_pmbranch , ls_err
datetime		ldtm_trn , ldtm_doc , ldtm_post
n_ds			lds_master , lds_detail

lds_master		= create n_ds
lds_master.dataobject = "d_fn_paytrnbank_trn_master"
lds_master.settransobject( itr_sqlca )
if not  isnull( as_moneyorder_master_xml ) and trim( as_moneyorder_master_xml ) <> ""  then
	li_rc	= lds_master.importstring( XML!, as_moneyorder_master_xml )
	if ( li_rc < 0 ) then
		ithw_exception.text		+= this.of_err_generate_import_xml( li_rc )
		throw ithw_exception
	end if
end if

lds_detail		= create n_ds
lds_detail.dataobject = "d_fn_paytrnbank_trn_detail"
lds_detail.settransobject( itr_sqlca )
if not  isnull( as_moneyorder_detail_xml ) and trim( as_moneyorder_detail_xml ) <> ""  then
	li_rc	= lds_detail.importstring( XML!, as_moneyorder_detail_xml )
	if ( li_rc < 0 ) then
		ithw_exception.text		+= this.of_err_generate_import_xml( li_rc )
		throw ithw_exception
	end if
end if

/////   สร้างเลขที่เอกสาร
//a_edit   ls_ptbdocno		= inv_docservice.of_getnewdocno( "FNMONEYORDER" )
lds_master.setitem( 1 , "paytrnbank_docno" , ls_ptbdocno )

ls_ptbdocno			= trim( lds_master.getitemstring( 1 , "paytrnbank_docno" ))
li_postflag			= lds_master.getitemnumber( 1 , "post_flag" )

lds_master.setitem( 1 , "entry_id" , as_entryid )
lds_master.setitem( 1 , "entry_date" , adtm_entry )

if li_postflag = 1 then 
	ldtm_post		= datetime( date( adtm_entry ))
	lds_master.setitem( 1 , "post_id" , as_entryid )
	lds_master.setitem( 1 , "post_date" , ldtm_post )
end if

//ls_ptbdocno				= trim( lds_master.getitemstring( 1 , "paytrnbank_docno" ))
ls_memberno			= trim( lds_master.getitemstring( 1 , "member_no" ))
ls_mbgroupcode		= trim( lds_master.getitemstring( 1 , "membgroup_code" ))
ls_pmdocno				= trim( lds_master.getitemstring( 1 , "paymentdoc_no" ))
ls_pmbranch			= trim( lds_master.getitemstring( 1 , "payment_branch" ))
ldc_ptb					= lds_master.getitemdecimal( 1 , "paytrnbank_amt" )
ldc_pmfee				= lds_master.getitemdecimal( 1 , "payment_fee" )
ldtm_trn					= lds_master.getitemdatetime( 1 , "trn_date" )
ldtm_doc					= lds_master.getitemdatetime( 1 , "doc_date" )

if isnull( ls_pmdocno ) then ls_pmdocno = ""
if isnull( ls_memberno ) then ls_memberno = ""
if isnull( ls_pmbranch ) then ls_pmbranch = ""
if isnull( ldc_ptb ) then ldc_ptb = 0.00
if isnull( ldc_pmfee ) then ldc_pmfee = 0.00

//if  trim( ls_ptbdocno ) = "" then
//	li_count ++
//	ls_err	+= string( li_count ) + "." + "เลขที่เอกสาร~r~n"
//end if

if  trim( ls_memberno ) = "" then
	li_count ++
	ls_err	+= string( li_count ) + "." + "เลขที่สมาชิก~r~n"
end if

if  trim( ls_mbgroupcode ) = "" then
	li_count ++
	ls_err	+= string( li_count ) + "." + "สังกัด~r~n"
end if

if  trim( ls_pmdocno ) = "" then
	li_count ++
	ls_err	+= string( li_count ) + "." + "เลขที่ธนาณัติ~r~n"
end if

if  trim( ls_pmbranch ) = "" then
	li_count ++
	ls_err	+= string( li_count ) + "." + "รหัสไปรษณีย์ (ธนาณัติ)~r~n"
end if

if  ldc_ptb = 0 then
	li_count ++
	ls_err	+= string( li_count ) + "." + "จำนวนเงิน~r~n"
end if

//if  ldc_pmfee = 0 then
//	li_count ++
//	ls_err	+= string( li_count ) + "." + "ค่าธรรมเนียม~r~n"
//end if

if  isNull( ldtm_trn ) then
	li_count ++
	ls_err	+= string( li_count ) + "." + "วันที่โอน~r~n"
end if

if  isNull( ldtm_doc ) then
	li_count ++
	ls_err	+= string( li_count ) + "." + "วันที่หน้าธนาณัติ~r~n"
end if

if li_count > 0 then
	ithw_exception.text	+= "กรุณาระบุค่าต่างๆ ดังนี้ ก่อนครับ~r~n" + ls_err
	throw ithw_exception
end if

select count(*)
into :li_found
from finpaytrnbank_mor
where paymentdoc_no = :ls_pmdocno
and 	paytrnbank_status = 1
and	post_flag	= 1
using itr_sqlca ;

if isnull( li_found ) or itr_sqlca.sqlcode <> 0 then li_found = 0 

if li_found > 0 then
	ithw_exception.text	= "ไม่สามารถบันทึกเลขที่ธนาณัติซ้ำได้ เลขที่ : " + ls_pmdocno
	throw ithw_exception
end if

ldc_sumitem_amt = dec( lds_detail.describe( "evaluate( 'sum( moneytrn_amt for all ) ',0)" ) )

if (ldc_ptb + ldc_pmfee ) <> ldc_sumitem_amt then
	ithw_exception.text	+= "จำนวนเงินรวมในรายละเอียดไม่เท่ากับจำนวนเงินรวมครับ"
	throw ithw_exception
end if	

// กำหนดค่าให้กับรายละเอียด
li_count  = lds_detail.rowcount( )
for li_row = 1 to li_count 
	lds_detail.setitem( li_row , "paytrnbank_docno" , ls_ptbdocno )
	lds_detail.setitem( li_row , "seq_no" , li_row )
next

lds_master.accepttext( )
lds_detail.accepttext( )

// กำหนดสถานะ datawindow Update ได้
lds_master.setitemstatus( 1 , 0 , Primary! , NewModified!)
lds_detail.setitemstatus( 1 , 0 , Primary! , NewModified!)

if lds_master.update() <> 1 then
	ithw_exception.text	+= "1.ไม่สามารถเพิ่มรายการธนาณัติได้"
	ithw_exception.text	+= lds_master.of_geterrormessage( )
	rollback using itr_sqlca ;
	throw ithw_exception
end if

if lds_detail.update() <> 1 then
	ithw_exception.text	+= "2.ไม่สามารถเพิ่มรายการธนาณัติได้"
	ithw_exception.text	+= lds_detail.of_geterrormessage( )
	rollback using itr_sqlca ;
	throw ithw_exception
end if

commit using itr_sqlca ;

return SUCCESS
end function

public function string of_err_generate_import_xml (integer ai_error);choose case ai_error
	case	-1   
		return "No rows or startrow value supplied is greater than the number of rows in the string"
	case	-3
		return "Invalid argument"
	case	-4
		return "Invalid input"
	case	-11
		return "XML Parsing Error; XML parser libraries not found or XML not well formed"
	case	-12
		return "XML Template does not exist or does not match the DataWindow"
	case	-13
		return "Unsupported DataWindow style for import"
	case	-14
		return "Error resolving DataWindow nesting"
	case else
		return "No Error Message"
end choose
end function

public function string of_init_moneyorder (string as_moneyorder_master_xml, string as_entryid, datetime adtm_entry) throws Exception;/***********************************************
<description>
เพื่อทำการจัดการกับค่าเริ่มต้นสำหรับใบธนาณัติ ก่อนที่จะทำรายการในลำดับต่อๆ ไป.
ว่าต้องมีค่าอะไรบ้าง แต่ละค่าควรจะเป็นอะไร เป้นต้น.

</description>

<arguments>
as_moneyorder_master_xml	รับข้อมูลที่เป็น XML เข้ามา
as_entryid							ข้อมูลรหัสผู้ใช้
adtm_entry							วันที่ตอนเข้าระบบ
</arguments>

<return>
string			ส่งข้อมูลกลับเป็น Datawindow ที่มีข้อมูลแล้วในรูปแบบ XML
</return>

<usage>
ยังไม่มีตัวอย่างการใช้งาน
</usage>
************************************************/

integer		li_rc ,li_membflag ,li_membstatus
string			ls_ftbinfo_xml , ls_memberno , ls_mbgroupcode , ls_mbprecode , ls_null
string			ls_mbprename , ls_mbname , ls_mbsurname , ls_mbgroupdesc     ,ls_membfullname ,  ls_persontype
datetime		ldtm_operate , ldtm_null
n_ds			lds_master

lds_master		= create n_ds
lds_master.dataobject = "d_fn_paytrnbank_trn_master"

if not  isnull( as_moneyorder_master_xml ) and trim( as_moneyorder_master_xml ) <> ""  then
	li_rc	= lds_master.importstring( XML!, as_moneyorder_master_xml )
	if ( li_rc < 0 ) then
		ithw_exception.text		+= this.of_err_generate_import_xml( li_rc )
		throw ithw_exception
	end if
end if

setNull( ls_null )
setNull( ldtm_null )

ls_memberno		= trim( lds_master.getitemstring( 1 , "member_no" ))
li_membflag			= lds_master.object.member_flag[1]

if li_membflag = 1 then 

	select prename_desc || ' ' || memb_name || ' ' || memb_surname  , membgroup_code , member_status
	into 	:ls_membfullname , :ls_mbgroupcode , :li_membstatus
	from mbmembmaster a , mbucfprename b
	where a.prename_code = b.prename_code
	and	member_no = :ls_memberno
	using itr_sqlca ;
	
	if itr_sqlca.sqlcode <> 0 then 
		ithw_exception.text	= "ไม่พบข้อมูลสมาชิก"
		throw ithw_exception
	end if
	
elseif li_membflag = 0 then
	
	select prename_desc , first_name || ' ' || last_name , persontype_code
	into 	:ls_mbprename , :ls_membfullname , :ls_persontype
	from fincontackmaster a , mbucfprename b
	where a.prename_code = b.prename_code	
	and contack_no = :ls_memberno
	using itr_sqlca ;
	
	if itr_sqlca.sqlcode <> 0 then 
		ithw_exception.text	= "ไม่พบข้อมูลบุคคลภายนอก"
		throw ithw_exception
	end if
	
	// หากเ็ป็นประเภทบุคคลธรรมดา
	if ls_persontype = "01" then
		ls_membfullname = ls_mbprename + ' ' + ls_membfullname
	end if
	
else
	ls_membfullname  	= ""
	ls_mbgroupcode 			= "" 
end if

if li_membstatus < 0 then
	ithw_exception.text	= "สมาชิกเลขที่ : " + ls_memberno +  " มีสถานะลาออก" 
	throw ithw_exception
end if

ldtm_operate		= datetime( date( adtm_entry ))

lds_master.setitem( 1 , "membgroup_code" , ls_mbgroupcode )
lds_master.setitem( 1 , "trn_date" , ldtm_operate )
lds_master.setitem( 1 , "doc_date" , ldtm_operate )
lds_master.setitem( 1 , "paytrnbank_desc" , ls_membfullname )
//lds_master.setitem( 1 , "mbmemb_name" , ls_mbname )
//lds_master.setitem( 1 , "mbmemb_surname" , ls_mbsurname )
lds_master.setitem( 1 , "mbmembgroup_desc" , ls_mbgroupdesc )
lds_master.setitem( 1 , "entry_id" , as_entryid )
lds_master.setitem( 1 , "entry_date" , adtm_entry )
lds_master.setitem( 1 , "post_id" , ls_null )
lds_master.setitem( 1 , "post_date" , ldtm_null )

ls_ftbinfo_xml		= lds_master.describe( "Datawindow.data.XML" )

return ls_ftbinfo_xml
end function

public function integer of_approve_moneyorder (string as_moneyorder_list_xml, string as_entryid, datetime adtm_entry) throws Exception;/***********************************************
<description>
เพื่อทำการขึ้นเงินสำหรับใบธนาณัติ 

</description>

<arguments>
as_moneyorder_list_xml			รับข้อมูลที่เป็น XML เข้ามา
as_entryid							ข้อมูลรหัสผู้ใช้
adtm_entry							วันที่ตอนเข้าระบบ
</arguments>

<return>
integer			ส่งข้อมูลการทำงานกลับไป 1-สำเร็จ , 0-มีข้อผิดพลาด
</return>

<usage>
ยังไม่มีตัวอย่างการใช้งาน
</usage>
************************************************/

long			ll_rowcount , ll_currentrow
integer		li_postflag , li_rc
string			ls_ptbdocno
n_ds			lds_listptb

lds_listptb		= create n_ds
lds_listptb.dataobject = "d_fn_paytrnbank_cash_list"
lds_listptb.settransobject( itr_sqlca )
if not  isnull( as_moneyorder_list_xml ) and trim( as_moneyorder_list_xml ) <> ""  then
	li_rc	= lds_listptb.importstring( XML!, as_moneyorder_list_xml )
	if ( li_rc < 0 ) then
		ithw_exception.text		+= this.of_err_generate_import_xml( li_rc )
		throw ithw_exception
	end if
end if

ll_rowcount		= lds_listptb.rowcount()
for ll_currentrow = 1 to ll_rowcount
	
	li_postflag		= lds_listptb.getitemnumber( ll_currentrow , "post_flag" )
	ls_ptbdocno		= trim( lds_listptb.getitemstring( ll_currentrow , "paytrnbank_docno" ))
	
	if li_postflag = 1 then
		//lds_listptb.setitem( ll_currentrow , "post_id" , as_entryid )
		//lds_listptb.setitem( ll_currentrow , "post_date" , adtm_entry )
		
		UPDATE	FINPAYTRNBANK_MOR
		SET		POST_FLAG = 1 ,
					POST_ID = :as_entryid ,
					POST_DATE = :adtm_entry
		WHERE	PAYTRNBANK_DOCNO = :ls_ptbdocno
		USING	itr_sqlca ;
		
		if itr_sqlca.sqlcode <> 0 then
			ithw_exception.text	+= "ไม่สามารถทำรายการธนาณัติได้"
			rollback using itr_sqlca ;
			throw ithw_exception
		end if
		
	end if
	
next

commit using itr_sqlca ;

return SUCCESS
end function

public function integer of_init (n_cst_dbconnectservice anv_db);

inv_db		= anv_db
itr_sqlca	= inv_db.itr_dbconnection

inv_docservice.of_settrans(anv_db)

return 1
end function

private function integer of_chkbf_moneyorder (ref n_ds ads_detail, string as_ptbdocno, decimal adc_ptb);/***********************************************
<description>
เพื่อตรวจสอบ Datawindow ตัว Detail ของธนาณัติ
เป็น Function สำหรับ of_save_moneyorder

</description>

************************************************/

long		ll_rowcount , ll_currentrow
dec{2}	ldc_monamt

ldc_monamt		= 0

ll_rowcount	= ads_detail.rowcount()
for ll_currentrow = 1 to ll_rowcount
	
	ads_detail.setitem( ll_currentrow , "paytrnbank_docno" , as_ptbdocno )
	ads_detail.setitem( ll_currentrow , "seq_no" , ll_currentrow )
	
//	ldc_monamt		= ldc_monamt + ads_detail.getitemdecimal( ll_currentrow , "moneytrn_amt" )
	
next

//if adc_ptb <> ldc_monamt then
//	return -1
//end if

return SUCCESS
end function

public function integer of_operate_moneyorder (string as_ptbdocno, string as_ptbtypecode, long al_seqno, string as_ptodocno, integer ai_ptoflag, string as_entryid, datetime adtm_entry) throws Exception;/***********************************************
<description>
เพื่อทำการอัพเดทสถานะการขึ้นเงินของใบธนาณัติ 
ว่ามีการนำไปใช้โดยระบบใดๆ หรือไม่

</description>

<arguments>
as_ptbdocno						เลขที่เอกสารธนาณัติ
as_ptbtypecode						ประเภทรายการเอกสาร
al_seqno								ลำดับที่ของรายการ
as_ptodocno						เลขที่เอกสารที่นำไปใช้ ( อ้างอิง )
ai_ptoflag							สถานะการนำไปใช้
as_entryid							ข้อมูลรหัสผู้ใช้
adtm_entry							วันที่ตอนเข้าระบบ
</arguments>

<return>
integer			ส่งข้อมูลการทำงานกลับไป 1-สำเร็จ , 0-มีข้อผิดพลาด
</return>

<usage>
ยังไม่มีตัวอย่างการใช้งาน
</usage>
************************************************/

long			ll_currentrow , ll_rowcount
integer		li_rc , li_count
string			ls_ptodocno , ls_err
datetime		ldtm_operate

if  trim( as_ptbdocno ) = "" then
	li_count ++
	ls_err	+= string( li_count ) + "." + "เลขที่เอกสาร~r~n"
end if

if  trim( as_ptbtypecode ) = "" then
	li_count ++
	ls_err	+= string( li_count ) + "." + "ประเภทรายการเอกสาร~r~n"
end if

if  isNull( al_seqno ) then
	li_count ++
	ls_err	+= string( li_count ) + "." + "ลำดับของรายการ~r~n"
end if

if  trim( as_ptodocno ) = "" then
	li_count ++
	ls_err	+= string( li_count ) + "." + "เลขที่เอกสารที่นำไปใช้~r~n"
end if

if  isNull( ai_ptoflag ) then
	li_count ++
	ls_err	+= string( li_count ) + "." + "สถานะของรายการ~r~n"
end if

if  trim( as_ptodocno ) = "" then
	li_count ++
	ls_err	+= string( li_count ) + "." + "ชื่อผู้ใช้~r~n"
end if

if  isNull( adtm_entry ) then
	li_count ++
	ls_err	+= string( li_count ) + "." + "วันที่เข้าระบบ~r~n"
end if

if li_count > 0 then 
	ithw_exception.text	+= "กรุณาระบุค่าต่างๆ ดังนี้ ก่อนครับ~r~n" + ls_err
	throw ithw_exception
end if

UPDATE	FINPAYTRNBANKDET
SET		paytrnoperate_flag = :ai_ptoflag ,
			paytrnoperate_id = :as_entryid ,
			paytrnoperate_date = :adtm_entry ,
			paytrnoperate_docno = :as_ptodocno
WHERE	Paytrnbank_docno = :as_ptbdocno
	AND	paytrnitemtype_code = :as_ptbtypecode
	AND	seq_no = :al_seqno
USING	itr_sqlca ;

if itr_sqlca.sqlcode <> 0 then
	ithw_exception.text	+= "ไม่สามารถทำรายการธนาณัติได้"
	rollback using itr_sqlca ;
	throw ithw_exception
end if

commit using itr_sqlca ;

return SUCCESS
end function

public function integer of_cancelappr_moneyorder (string as_moneyorder_list_xml, string as_entryid, datetime adtm_entry) throws Exception;/***********************************************
<description>
เพื่อทำการยกเลิกการขึ้นเงินสำหรับใบธนาณัติ 

</description>

<arguments>
as_moneyorder_list_xml			รับข้อมูลที่เป็น XML เข้ามา
as_entryid							ข้อมูลรหัสผู้ใช้
adtm_entry							วันที่ตอนเข้าระบบ
</arguments>

<return>
integer			ส่งข้อมูลการทำงานกลับไป 1-สำเร็จ , 0-มีข้อผิดพลาด
</return>

<usage>
ยังไม่มีตัวอย่างการใช้งาน
</usage>
************************************************/

long			ll_rowcount , ll_currentrow
integer		li_postflag , li_rc
string			ls_ptbdocno
n_ds			lds_listptb

lds_listptb		= create n_ds
lds_listptb.dataobject = "d_fn_paytrnbank_cash_list"
lds_listptb.settransobject( itr_sqlca )
if not  isnull( as_moneyorder_list_xml ) and trim( as_moneyorder_list_xml ) <> ""  then
	li_rc	= lds_listptb.importstring( XML!, as_moneyorder_list_xml )
	if ( li_rc < 0 ) then
		ithw_exception.text		+= this.of_err_generate_import_xml( li_rc )
		throw ithw_exception
	end if
end if

ll_rowcount		= lds_listptb.rowcount()
for ll_currentrow = 1 to ll_rowcount
	
	li_postflag		= lds_listptb.getitemnumber( ll_currentrow , "post_flag" )
	ls_ptbdocno		= trim( lds_listptb.getitemstring( ll_currentrow , "paytrnbank_docno" ))
	
	if li_postflag = 0 then
		//lds_listptb.setitem( ll_currentrow , "post_id" , as_entryid )
		//lds_listptb.setitem( ll_currentrow , "post_date" , adtm_entry )
		
		UPDATE	FINPAYTRNBANK_MOR
		SET		POST_FLAG = 0
		WHERE	PAYTRNBANK_DOCNO = :ls_ptbdocno
		USING	itr_sqlca ;
		
		if itr_sqlca.sqlcode <> 0 then
			ithw_exception.text	+= "ไม่สามารถทำรายการธนาณัติได้"
			rollback using itr_sqlca ;
			throw ithw_exception
		end if
		
	end if
	
next

commit using itr_sqlca ;

return SUCCESS
end function

public function integer of_cancel_moneyorder (string as_moneyorder_list_xml, string as_entryid, datetime adtm_entry) throws Exception;/***********************************************
<description>
เพื่อทำการยกเลิกสำหรับใบธนาณัติ 

</description>

<arguments>
as_moneyorder_list_xml			รับข้อมูลที่เป็น XML เข้ามา
as_entryid							ข้อมูลรหัสผู้ใช้
adtm_entry							วันที่ตอนเข้าระบบ
</arguments>

<return>
integer			ส่งข้อมูลการทำงานกลับไป 1-สำเร็จ , 0-มีข้อผิดพลาด
</return>

<usage>
ยังไม่มีตัวอย่างการใช้งาน
</usage>
************************************************/

long			ll_rowcount , ll_currentrow
integer		li_ptbstatus , li_rc
string			ls_ptbdocno
n_ds			lds_listptb

lds_listptb		= create n_ds
lds_listptb.dataobject = "d_fn_paytrnbank_cancelapv_list"
lds_listptb.settransobject( itr_sqlca )
if not  isnull( as_moneyorder_list_xml ) and trim( as_moneyorder_list_xml ) <> ""  then
	li_rc	= lds_listptb.importstring( XML!, as_moneyorder_list_xml )
	if ( li_rc < 0 ) then
		ithw_exception.text		+= this.of_err_generate_import_xml( li_rc )
		throw ithw_exception
	end if
end if

ll_rowcount		= lds_listptb.rowcount()
for ll_currentrow = 1 to ll_rowcount
	
	li_ptbstatus		= lds_listptb.getitemnumber( ll_currentrow , "paytrnbank_status" )
	ls_ptbdocno		= trim( lds_listptb.getitemstring( ll_currentrow , "paytrnbank_docno" ))
	
	if li_ptbstatus = -9 then
		//lds_listptb.setitem( ll_currentrow , "cancel_id" , as_entryid )
		//lds_listptb.setitem( ll_currentrow , "cancel_date" , adtm_entry )
		
		UPDATE	FINPAYTRNBANK_MOR
		SET		PAYTRNBANK_STATUS = -9 , 
					CANCEL_ID = :as_entryid ,
					CANCEL_DATE = :adtm_entry
		WHERE	PAYTRNBANK_DOCNO = :ls_ptbdocno
		USING	itr_sqlca ;
		
		if itr_sqlca.sqlcode <> 0 then
			ithw_exception.text	+= "ไม่สามารถทำรายการธนาณัติได้"
			rollback using itr_sqlca ;
			throw ithw_exception
		end if
		
	end if
	
next

commit using itr_sqlca ;

return SUCCESS
end function

public function string of_getlist_moneyorder (datetime adtm_trn) throws Exception;/***********************************************
<description>
เพื่อทำการดึงข้อมูลให้กับ d_fn_paytrnbank_operate_list

</description>

<arguments>
adtm_trn							วันที่โอน
</arguments>

<return>
string			ส่งข้อมูลกลับเป็น Datawindow ที่มีข้อมูลแล้วในรูปแบบ XML
</return>

<usage>
ยังไม่มีตัวอย่างการใช้งาน
</usage>
************************************************/

if isNull( adtm_trn ) then
	ithw_exception.text	+= "กรุณาระบุค่า วันที่โอน ด้วย"
	throw ithw_exception
end if

string			ls_listinfo_xml
n_ds			lds_list

lds_list		= create n_ds
lds_list.dataobject = "d_fn_paytrnbank_operate_list"
lds_list.settransobject( itr_sqlca )

lds_list.retrieve( adtm_trn )

ls_listinfo_xml		=  ""
if lds_list.rowcount( ) > 0 then
	ls_listinfo_xml		= lds_list.describe( "Datawindow.data.XML" )
end if

return ls_listinfo_xml
end function

public function integer of_getdata_moneyorder (string as_docno, ref string as_moneyorder_master_xml, ref string as_moneyorder_detail_xml) throws Exception;/***********************************************
<description>
เพื่อทำการดึงข้อมูลให้กับ d_fn_paytrnbank_operate_main 
กับ d_fn_paytrnbank_operate_detail

</description>

<arguments>
as_docno							เลขที่เอกสาร
</arguments>

<return>
integer			ส่งข้อมูลการทำงานกลับไป 1-สำเร็จ , 0-มีข้อผิดพลาด
</return>

<usage>
ยังไม่มีตัวอย่างการใช้งาน
</usage>
************************************************/

if isNull( as_docno ) then
	ithw_exception.text	+= "กรุณาระบุค่า เลขที่เอกสาร ด้วย"
	throw ithw_exception
end if

string			ls_master_xml , ls_detail_xml
n_ds			lds_master , lds_detail

lds_master		= create n_ds
lds_master.dataobject = "d_fn_paytrnbank_operate_main"
lds_master.settransobject( itr_sqlca )

lds_master.retrieve( as_docno )

lds_detail		= create n_ds
lds_detail.dataobject = "d_fn_paytrnbank_operate_detail"
lds_detail.settransobject( itr_sqlca )

lds_detail.retrieve( as_docno )

as_moneyorder_master_xml	= ""
as_moneyorder_detail_xml		= ""

if ( lds_master.rowcount() > 0 ) then
	as_moneyorder_master_xml	= lds_master.Describe( "Datawindow.Data.XML" )
end if

if ( lds_detail.rowcount() > 0 ) then
	as_moneyorder_detail_xml	= lds_detail.Describe( "Datawindow.Data.XML" )
end if


return SUCCESS
end function

public function string of_getlistcancel_moneyorder (datetime adtm_trn) throws Exception;/***********************************************
<description>
เพื่อทำการดึงข้อมูลให้กับ d_fn_paytrnbank_cancelapv_list

</description>

<arguments>
adtm_trn							วันที่โอน
</arguments>

<return>
string			ส่งข้อมูลกลับเป็น Datawindow ที่มีข้อมูลแล้วในรูปแบบ XML
</return>

<usage>
ยังไม่มีตัวอย่างการใช้งาน
</usage>
************************************************/

if isNull( adtm_trn ) then
	ithw_exception.text	+= "กรุณาระบุค่า วันที่โอน ด้วย"
	throw ithw_exception
end if

string			ls_listinfo_xml
n_ds			lds_list

lds_list		= create n_ds
lds_list.dataobject = "d_fn_paytrnbank_cancelapv_list"
lds_list.settransobject( itr_sqlca )

lds_list.retrieve( adtm_trn )

ls_listinfo_xml		= lds_list.describe( "Datawindow.data.XML" )

return ls_listinfo_xml

end function

public function string of_getlistappr_moneyorder (datetime adtm_trn) throws Exception;/***********************************************
<description>
เพื่อทำการดึงข้อมูลให้กับ d_fn_paytrnbank_cash_list

</description>

<arguments>
adtm_trn							วันที่โอน
</arguments>

<return>
string			ส่งข้อมูลกลับเป็น Datawindow ที่มีข้อมูลแล้วในรูปแบบ XML
</return>

<usage>
ยังไม่มีตัวอย่างการใช้งาน
</usage>
************************************************/

if isNull( adtm_trn ) then
	ithw_exception.text	+= "กรุณาระบุค่า วันที่โอน ด้วย"
	throw ithw_exception
end if

string			ls_listinfo_xml
n_ds			lds_list

lds_list		= create n_ds
lds_list.dataobject = "d_fn_paytrnbank_cash_list"
lds_list.settransobject( itr_sqlca )

lds_list.retrieve( adtm_trn )

ls_listinfo_xml		= lds_list.describe( "Datawindow.data.XML" )

return ls_listinfo_xml

end function

public function string of_getlistreappr_moneyorder (datetime adtm_trn) throws Exception;/***********************************************
<description>
เพื่อทำการดึงข้อมูลให้กับ d_fn_paytrnbank_cancelcash_list

</description>

<arguments>
adtm_trn							วันที่โอน
</arguments>

<return>
string			ส่งข้อมูลกลับเป็น Datawindow ที่มีข้อมูลแล้วในรูปแบบ XML
</return>

<usage>
ยังไม่มีตัวอย่างการใช้งาน
</usage>
************************************************/

if isNull( adtm_trn ) then
	ithw_exception.text	+= "กรุณาระบุค่า วันที่โอน ด้วย"
	throw ithw_exception
end if

string			ls_listinfo_xml
n_ds			lds_list

lds_list		= create n_ds
lds_list.dataobject = "d_fn_paytrnbank_cancelcash_list"
lds_list.settransobject( itr_sqlca )

lds_list.retrieve( adtm_trn )

ls_listinfo_xml		= lds_list.describe( "Datawindow.data.XML" )

return ls_listinfo_xml

end function

public function integer of_postusemoneyorder (string as_docno, integer ai_seqno, string as_ref_slipno, integer ai_ref_seqno, integer ai_itemstatus, string as_entry_id, datetime adtm_wdate) throws Exception;
if ai_itemstatus > 0 then
	
		update finpaytrnbankdet
		set	paytrnoperate_flag 		= 1 , 
				paytrnoperate_id	  		= :as_entry_id ,
				paytrnoperate_date		= :adtm_wdate ,
				paytrnoperate_docno	= :as_ref_slipno ,
				document_type			= :ai_ref_seqno
		where 	paytrnbank_docno	= :as_docno	and
				seq_no	= :ai_seqno 
		using		itr_sqlca ;
		
		if itr_sqlca.sqlcode <> 0 then
//			ithw_exception.text	+= "ไม่สามารถทำรายการธนาณัติได้"
//			rollback using itr_sqlca ;
//			throw ithw_exception
			return failure
		end if

else
		// หากยกเลิกรายการจะต้อง update สถานะกลับมาเป็น ยังไม่ใช้งาน
		update finpaytrnbankdet
		set	paytrnoperate_flag 		= 0 , 
				paytrnoperate_id	  		= :as_entry_id ,
				paytrnoperate_date		= :adtm_wdate ,
				paytrnoperate_docno	= :as_ref_slipno ,
				document_type			= :ai_ref_seqno
		where 	paytrnbank_docno	= :as_docno	and
				seq_no	= :ai_seqno 
		using		itr_sqlca ;
		
		if itr_sqlca.sqlcode <> 0 then
//			ithw_exception.text	+= "ไม่สามารถทำรายการธนาณัติได้"
//			rollback using itr_sqlca ;
//			throw ithw_exception
			return failure
		end if
		
end if

return success
end function

on n_cst_moneyorder_service.create
call super::create
TriggerEvent( this, "constructor" )
end on

on n_cst_moneyorder_service.destroy
TriggerEvent( this, "destructor" )
call super::destroy
end on

event constructor;itr_sqlca			= create transaction
ithw_exception	= create Exception
inv_docservice	= create n_cst_doccontrolservice	
end event

