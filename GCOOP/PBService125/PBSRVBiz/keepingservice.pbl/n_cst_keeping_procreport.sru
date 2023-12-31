$PBExportHeader$n_cst_keeping_procreport.sru
$PBExportComments$service การประมวลออกรายงาน

forward
global type n_cst_keeping_procreport from NonVisualObject
end type
end forward

/// <summary> service การประมวลออกรายงาน
/// 
/// </summary>
global type n_cst_keeping_procreport from NonVisualObject
end type
global n_cst_keeping_procreport n_cst_keeping_procreport

type variables
string		is_coopid , is_coopcontrol , is_recvperiod, is_arg[] , is_sqlextfn, is_reporttyp
integer	ii_proctype 

n_ds 	ids_msgerr , ids_report

n_cst_keeping_service		inv_kpsrv
n_cst_datawindowsservice	inv_dwsrv
n_cst_stringservice			inv_stringsrv
n_cst_progresscontrol		inv_progress
n_cst_dbconnectservice		inv_transection

transaction	itr_sqlca
Exception	ithw_exception
end variables

forward prototypes
public function integer of_initservice (n_cst_dbconnectservice atr_dbtrans) throws Exception
public function integer of_setprogress (ref n_cst_progresscontrol anv_progress) throws Exception
public function integer of_rcvproc_report (str_keep_proc astr_keep_proc) throws Exception
protected function integer of_deletereport () throws Exception
protected function integer of_extfn (ref str_extfn_keep astr_extfn_keep) throws Exception
protected function integer of_processreport () throws Exception
protected function integer of_setreport_dep (long al_row, integer ai_seqno, string as_deptkeep, string as_depttype, string as_deptno, decimal adc_deptamt) throws Exception
protected function integer of_setreport_ffe (long al_row, integer ai_seqno, decimal adc_item) throws Exception
protected function integer of_setreport_lon (long al_row, long al_seqno, string as_keeptype, string as_loantype, string as_contract, integer ai_period, decimal adc_prin, decimal adc_int, decimal adc_bal) throws Exception
protected function integer of_setreport_mem (long al_row, string as_coopid, string as_kpslipno, integer ai_seqno, string as_memcoopid, string as_memno, string as_refmemcoopid, string as_refmem, string as_membtyp, string as_department, string as_memgrp, string as_memsec, string as_membtyp_desc, string as_department_desc, string as_memgrp_desc, string as_memsec_desc, string as_receiptno) throws Exception
protected function integer of_setreport_oth (long al_row, long al_seqno, decimal adc_item) throws Exception
protected function integer of_setreport_shr (long al_row, integer ai_seqno, integer ai_period, decimal adc_shrvalue, decimal adc_shrstkvalue) throws Exception
protected function integer of_setsrvdw (boolean ab_switch) throws Exception
protected function integer of_processreporthnd () throws Exception
end prototypes

public function integer of_initservice (n_cst_dbconnectservice atr_dbtrans) throws Exception;/***********************************************************
<description>
	ใช้สำหรับ Intial service
</description>

<arguments>  
	atr_dbtrans			n_cst_dbconnectservice	รายละเอียดการเชื่อมต่อฐานข้อมูล
</arguments> 

<return> 
	Integer				1 = success
</return> 

<usage> 
	เรียกใช้ครั้งเดียว และต้องเรียกใช้เป็นฟังก์ชั่นแรกหลังจาก create instance
	ก่อนที่จะเรียกใช้ฟังก์ชั่นอื่น ๆ
	
	Revision History:
	Version 1.0 (Initial version) Modified Date 28/9/2010 by MiT
</usage> 
***********************************************************/
itr_sqlca = atr_dbtrans.itr_dbconnection
is_coopcontrol	= atr_dbtrans.is_coopcontrol
is_coopid = atr_dbtrans.is_coopid

if isnull( inv_transection ) or not isvalid( inv_transection ) then
	inv_transection = create n_cst_dbconnectservice
	inv_transection = atr_dbtrans
end if

// สำหรับเก็บเงื่อนไข error
ids_msgerr = create n_ds
ids_msgerr.dataobject = "d_kp_msg_error"
ids_msgerr.settransobject( itr_sqlca )

// รายงาน
ids_report = create n_ds
ids_report.dataobject = "r_kp_proc_report_cen"
ids_report.settransobject( itr_sqlca )

// Service สำหรับสร้าง Sql Extend
inv_stringsrv	= create n_cst_stringservice

// สร้าง Progress สำหรับแสดงสถานะการประมวลผล
inv_progress	= create n_cst_progresscontrol

// Service สำหรับการประมวลผล
inv_kpsrv			= create n_cst_keeping_service
inv_kpsrv.of_initservice( atr_dbtrans )

return 1
end function

public function integer of_setprogress (ref n_cst_progresscontrol anv_progress) throws Exception;/***********************************************************
<description>
	กำหนด progress bar ที่ใช้แสดงผล
</description>

<arguments>

</arguments> 

<return> 
	Integer	1 = success
</return> 

<usage> 
	
	Revision History:
	Version 1.0 (Initial version) Modified Date 28/9/2010 by MiT
</usage> 
***********************************************************/

anv_progress = inv_progress

return 1
end function

public function integer of_rcvproc_report (str_keep_proc astr_keep_proc) throws Exception;/***********************************************************
<description>
	ประมวลผลเรียกเก็บ
</description>

<arguments>

</arguments> 

<return> 
	Integer	1 = success,  Exception = failure
</return> 

<usage> 
	
	Revision History:
	Version 1.0 (Initial version) Modified Date 28/9/2010 by MiT
</usage> 
***********************************************************/

string		ls_memtyp , ls_group, ls_memno , ls_xmloption
integer	li_year , li_month , li_mainproc , li_count
integer	li_chk
long		ll_recpno
datetime	ldtm_receiptdate, ldtm_calintdate

str_extfn_keep lstr_extfn_keep

n_ds	lds_procdata
n_cst_dwxmlieservice		lnv_dwxmliesrv

this.of_setsrvdw(true)

lnv_dwxmliesrv		= create n_cst_dwxmlieservice

lds_procdata = create n_ds
lds_procdata.dataobject = "d_kp_option_proc_report"
lds_procdata.settransobject( itr_sqlca )

ls_xmloption		= astr_keep_proc.xml_option

// นำเข้า XML
if lnv_dwxmliesrv.of_xmlimport( lds_procdata, ls_xmloption	 ) < 1 then
	return -1
end if

li_year				= lds_procdata.getitemnumber( 1, "receive_year" )
li_month				= lds_procdata.getitemnumber( 1, "receive_month" )
ls_memtyp			= lds_procdata.getitemstring( 1, "memb_text" )
ls_group				= lds_procdata.getitemstring( 1, "group_text" )
ls_memno			= lds_procdata.getitemstring( 1, "mem_text" )

// กำหนดค่าให้ instance เพื่อใช้ในฟังก์ชันอื่น ๆ
is_reporttyp			= lower(trim(lds_procdata.getitemstring( 1, "report_type" )))
ii_proctype			= lds_procdata.getitemnumber( 1, "proc_type" )

is_recvperiod		= string( li_year ) + string( li_month, "00" )

//// รูปแบบการประมวลผล
//choose case ii_proctype
//	case 2	// ตามกลุ่ม
//		inv_stringsrv.of_analyzestring( ls_group, is_arg[] )
//	case 3 	// ตามทะเบียน
//		inv_stringsrv.of_analyzestring( ls_memno, is_arg[] )
//	case 4	// ตามประเภทสมาชิก
//		inv_stringsrv.of_analyzestring( ls_memtyp, is_arg[] )
//end choose
//
//// set ค่าต่างๆ
//inv_kpsrv.of_setargument( is_arg )
//inv_kpsrv.of_setproctype( ii_proctype )

inv_kpsrv.of_setproctype( ii_proctype )
inv_kpsrv.of_set_txtmemno( ls_memno )
inv_kpsrv.of_set_txtmemgrp( ls_group )
inv_kpsrv.of_set_txtmemtyp( ls_memtyp )
inv_kpsrv.of_setanalyze()

li_mainproc	= 1

inv_progress.istr_progress.progress_max = 1

li_count		= 1

inv_progress.istr_progress.progress_index = li_count
li_chk = this.of_processreport( )
if li_chk <> 1 then
	rollback using itr_sqlca ;
	throw ithw_exception
end if

inv_progress.istr_progress.progress_text		= "ประมวลงบหน้าจัดเก็บเสร็จเรียบร้อย"

commit using itr_sqlca ;

inv_progress.istr_progress.status = 1

this.of_setsrvdw(false)

return 1
end function

protected function integer of_deletereport () throws Exception;/***********************************************************
<description>
	ลบข้อมูลสมาชิกที่ประมวลรายงานใหม่
</description>

<arguments>

</arguments> 

<return> 
	Integer	1 = success,  Exception = failure
</return> 

<usage> 
	Revision History:
	Version 1.0 (Initial version) Modified Date 23/04/2012 by Godji
</usage> 
***********************************************************/
string		ls_delsql , ls_sqlext

yield()
if inv_progress.of_is_stop() then
	return 0
end if

inv_kpsrv.of_setsqlselect( "kprcvprocreport" )
inv_kpsrv.of_getsqlselect( ls_sqlext )

ls_delsql		= "delete from kprcvprocreport where coop_id = '"+is_coopid+"' and recv_period = '"+is_recvperiod+"' "
ls_delsql		+= ls_sqlext

execute immediate :ls_delsql using itr_sqlca;
if itr_sqlca.sqlcode = -1 then return -1

return 1
end function

protected function integer of_extfn (ref str_extfn_keep astr_extfn_keep) throws Exception;string		ls_function , ls_return

ls_function		= astr_extfn_keep.function_name

choose case lower( ls_function )
	case 'of_kp_procreport_getsql_kptemp'
		// ดึง sql งบหน้าเรียกเก็บดึงข้อมูลจาก kptempreceive
		// is_coopid
		
//		SELECT kptempreceive.coop_id , kptempreceive.memcoop_id , kptempreceive.refmemcoop_id , 
//		kptempreceive.ref_membno , kptempreceive.membgroup_code , kptempreceive.membtype_code , 
//		kptempreceive.department_code , kptempreceive.membsection_code , 
//		KPTEMPRECEIVE.MEMBER_NO , KPTEMPRECEIVE.RECV_PERIOD , 
//		KPTEMPRECEIVE.RECEIPT_NO , KPTEMPRECEIVEDET.KEEPITEMTYPE_CODE , KPTEMPRECEIVEDET.SHRLONTYPE_CODE , 
//		KPTEMPRECEIVEDET.DESCRIPTION , KPTEMPRECEIVEDET.PERIOD , KPTEMPRECEIVEDET.PRINCIPAL_PAYMENT , 
//		KPTEMPRECEIVEDET.INTEREST_PAYMENT , KPTEMPRECEIVEDET.ITEM_PAYMENT , KPTEMPRECEIVEDET.ITEM_BALANCE , 
//		KPTEMPRECEIVEDET.LOANCONTRACT_NO , KPTEMPRECEIVEDET.SEQ_NO , KPUCFKEEPITEMTYPE.SIGN_FLAG 
//		FROM KPTEMPRECEIVE , MBMEMBMASTER , KPTEMPRECEIVEDET , KPUCFKEEPITEMTYPE 
//		where kptempreceive.coop_id = kptempreceivedet.coop_id 
//		and kptempreceive.kpslip_no = kptempreceivedet.kpslip_no 
//		and kptempreceive.memcoop_id = mbmembmaster.coop_id 
//		and kptempreceive.member_no = mbmembmaster.member_no 
//		and kptempreceivedet.coop_id = kpucfkeepitemtype.coop_id 
//		and kptempreceivedet.keepitemtype_code = kpucfkeepitemtype.keepitemtype_code 
//		and ( ( kpucfkeepitemtype.system_flag = 1 ) 
//		and ( kptempreceivedet.keepitem_status = 1 ) ) 
//		and kptempreceive.coop_id = '001001' 

		ls_return		= "SELECT nvl( kptempreceive.coop_id , null ) as coop_id , nvl( kptempreceive.kpslip_no , null ) as kpslip_no , nvl( kptempreceive.memcoop_id , null ) as memcoop_id , nvl( kptempreceive.refmemcoop_id , null ) as refmemcoop_id , "
		ls_return		+= " nvl( kptempreceive.ref_membno , null ) as ref_membno , nvl( kptempreceive.membgroup_code , null ) as membgroup_code , nvl( kptempreceive.membtype_code , null ) as membtype_code , "
		ls_return		+= " nvl( kptempreceive.department_code , null ) as department_code , nvl( kptempreceive.membsection_code , null ) as membsection_code , "
		ls_return		+= " nvl( KPTEMPRECEIVE.MEMBER_NO , null ) as member_no , nvl( KPTEMPRECEIVE.RECV_PERIOD , null ) as recv_period , "
		ls_return		+= " nvl( kptempreceive.sharestk_value , null ) as sharestk_value , "
		ls_return		+= " nvl( KPTEMPRECEIVE.RECEIPT_NO , null ) as receipt_no , nvl( KPTEMPRECEIVEDET.KEEPITEMTYPE_CODE , null ) as keepitemtype_code , nvl( KPTEMPRECEIVEDET.SHRLONTYPE_CODE , null ) as shrlontype_code , "
		ls_return		+= " nvl( KPTEMPRECEIVEDET.DESCRIPTION , null ) as description , nvl( KPTEMPRECEIVEDET.PERIOD , null ) as period , nvl( KPTEMPRECEIVEDET.PRINCIPAL_PAYMENT , null ) as principal_payment , "
		ls_return		+= " nvl( KPTEMPRECEIVEDET.INTEREST_PAYMENT , null ) as interest_payment , nvl( KPTEMPRECEIVEDET.ITEM_PAYMENT , null ) as item_payment , nvl( KPTEMPRECEIVEDET.ITEM_BALANCE , null ) as item_balance , "
		ls_return		+= " nvl( KPTEMPRECEIVEDET.LOANCONTRACT_NO , null ) as loancontract_no , nvl( KPTEMPRECEIVEDET.SEQ_NO , null ) as seq_no , nvl( KPUCFKEEPITEMTYPE.SIGN_FLAG , null ) as sign_flag , "
		ls_return		+= " nvl( kpucfkeepitemtype.keepitemtype_grp , null ) as keepitemtype_grp , "
		ls_return		+= " nvl( mb.membgroup_desc , null ) as membgroup_desc , "
		ls_return		+= " nvl( mbucfmembtype.membtype_desc , null ) as membtype_desc , nvl( mbucfdepartment.department_desc , null ) as department_desc "
		ls_return		+= " FROM KPTEMPRECEIVE , MBMEMBMASTER , KPTEMPRECEIVEDET , KPUCFKEEPITEMTYPE , MBUCFMEMBGROUP MB , MBUCFMEMBTYPE , MBUCFDEPARTMENT"
		ls_return		+= " where kptempreceive.coop_id = kptempreceivedet.coop_id "
		ls_return		+= " and kptempreceive.kpslip_no = kptempreceivedet.kpslip_no "
		ls_return		+= " and kptempreceive.memcoop_id = mbmembmaster.coop_id "
		ls_return		+= " and kptempreceive.member_no = mbmembmaster.member_no "
		ls_return		+= " and kptempreceive.memcoop_id = mb.coop_id (+) "
		ls_return		+= " and kptempreceive.membgroup_code = mb.membgroup_code (+) "
//		ls_return		+= " and kptempreceive.memcoop_id = ms.coop_id (+) "
//		ls_return		+= " and kptempreceive.membsection_code = ms.membgroup_code (+) "
		ls_return		+= " and kptempreceive.memcoop_id = mbucfmembtype.coop_id (+) "
		ls_return		+= " and kptempreceive.membtype_code = mbucfmembtype.membtype_code (+) "
		ls_return		+= " and kptempreceive.memcoop_id = mbucfdepartment.coop_id (+) "
		ls_return		+= " and kptempreceive.department_code = mbucfdepartment.department_code (+) "
		ls_return		+= " and kptempreceivedet.coop_id = kpucfkeepitemtype.coop_id "
		ls_return		+= " and kptempreceivedet.keepitemtype_code = kpucfkeepitemtype.keepitemtype_code "
		ls_return		+= " and ( ( kpucfkeepitemtype.system_flag = 1 ) "
		ls_return		+= " and ( kptempreceivedet.keepitem_status = 1 ) ) "
		ls_return		+= " and kptempreceive.coop_id = '"+is_coopid+"' "
		ls_return		+= " and kptempreceive.recv_period = '"+is_recvperiod+"' "

		astr_extfn_keep.function_return	= ls_return

	case 'of_kp_procreport_getsql_kpmast'
		// ดึง sql งบหน้าเรียกเก็บดึงข้อมูลจาก kpmastreceive
		// is_coopid
		
		ls_return		= "SELECT nvl( kpmastreceive.coop_id , null ) as coop_id , nvl( kpmastreceive.kpslip_no , null ) as kpslip_no , nvl( kpmastreceive.memcoop_id , null ) as memcoop_id , nvl( kpmastreceive.refmemcoop_id , null ) as refmemcoop_id , "
		ls_return		+= " nvl( kpmastreceive.ref_membno , null ) as ref_membno , nvl( kpmastreceive.membgroup_code , null ) as membgroup_code , nvl( kpmastreceive.membtype_code , null ) as membtype_code , "
		ls_return		+= " nvl( kpmastreceive.department_code , null ) as department_code , nvl( kpmastreceive.membsection_code , null ) as membsection_code , "
		ls_return		+= " nvl( kpmastreceive.MEMBER_NO , null ) as member_no , nvl( kpmastreceive.RECV_PERIOD , null ) as recv_period , "
		ls_return		+= " nvl( kpmastreceive.sharestk_value , null ) as sharestk_value , "
		ls_return		+= " nvl( kpmastreceive.RECEIPT_NO , null ) as receipt_no , nvl( kpmastreceivedet.KEEPITEMTYPE_CODE , null ) as keepitemtype_code , nvl( kpmastreceivedet.SHRLONTYPE_CODE , null ) as shrlontype_code , "
		ls_return		+= " nvl( kpmastreceivedet.DESCRIPTION , null ) as description , nvl( kpmastreceivedet.PERIOD , null ) as period , nvl( kpmastreceivedet.PRINCIPAL_PAYMENT - kpmastreceivedet.adjust_prnamt , null ) as principal_payment , "
		ls_return		+= " nvl( kpmastreceivedet.INTEREST_PAYMENT - kpmastreceivedet.adjust_intamt , null ) as interest_payment , nvl( kpmastreceivedet.ITEM_PAYMENT - kpmastreceivedet.adjust_itemamt , null ) as item_payment , nvl( kpmastreceivedet.ITEM_BALANCE , null ) as item_balance , "
		ls_return		+= " nvl( kpmastreceivedet.LOANCONTRACT_NO , null ) as loancontract_no , nvl( kpmastreceivedet.SEQ_NO , null ) as seq_no , nvl( KPUCFKEEPITEMTYPE.SIGN_FLAG , null ) as sign_flag , "
		ls_return		+= " nvl( kpucfkeepitemtype.keepitemtype_grp , null ) as keepitemtype_grp , "
		ls_return		+= " nvl( mb.membgroup_desc , null ) as membgroup_desc , "
		ls_return		+= " nvl( mbucfmembtype.membtype_desc , null ) as membtype_desc , nvl( mbucfdepartment.department_desc , null ) as department_desc "
		ls_return		+= " FROM kpmastreceive , MBMEMBMASTER , kpmastreceivedet , KPUCFKEEPITEMTYPE , MBUCFMEMBGROUP MB , MBUCFMEMBTYPE , MBUCFDEPARTMENT "
		ls_return		+= " where kpmastreceive.coop_id = kpmastreceivedet.coop_id "
		ls_return		+= " and kpmastreceive.kpslip_no = kpmastreceivedet.kpslip_no " 
		ls_return		+= " and kpmastreceive.memcoop_id = mbmembmaster.coop_id "
		ls_return		+= " and kpmastreceive.member_no = mbmembmaster.member_no "
		ls_return		+= " and kpmastreceive.memcoop_id = mb.coop_id (+) "
		ls_return		+= " and kpmastreceive.membgroup_code = mb.membgroup_code (+) "
//		ls_return		+= " and kpmastreceive.memcoop_id = ms.coop_id (+) "
//		ls_return		+= " and kpmastreceive.membsection_code = ms.membgroup_code (+) "
		ls_return		+= " and kpmastreceive.memcoop_id = mbucfmembtype.coop_id (+) "
		ls_return		+= " and kpmastreceive.membtype_code = mbucfmembtype.membtype_code (+) "
		ls_return		+= " and kpmastreceive.memcoop_id = mbucfdepartment.coop_id (+) "
		ls_return		+= " and kpmastreceive.department_code = mbucfdepartment.department_code (+) "
		ls_return		+= " and kpmastreceivedet.coop_id = kpucfkeepitemtype.coop_id "
		ls_return		+= " and kpmastreceivedet.keepitemtype_code = kpucfkeepitemtype.keepitemtype_code "
		ls_return		+= " and ( ( kpucfkeepitemtype.system_flag = 1 ) "
		ls_return		+= " and ( kpmastreceivedet.keepitem_status = 1 ) ) "
		ls_return		+= " and kpmastreceive.coop_id = '"+is_coopid+"' "
		ls_return		+= " and kpmastreceive.recv_period = '"+is_recvperiod+"' "
		
		astr_extfn_keep.function_return	= ls_return
		
	case 'of_kp_procreport_getsql_kparre'
		// ดึง sql ค้างงบหน้าเรียกเก็บดึงข้อมูลจาก kpmastreceive
		// is_coopid
		
		astr_extfn_keep.function_return	= ls_return		
		
	case else
		ithw_exception.text = "ไม่พบฟังก์ชั่น พบข้อผิดพลาด~r~n"
		throw ithw_exception
end choose

return 1
end function

protected function integer of_processreport () throws Exception;/***********************************************************
<description>
	ประมวลผลรายงาน
</description>

<arguments>

</arguments> 

<return> 
	Integer	1 = success,  Exception = failure
</return> 

<usage> 
	Revision History:
	Version 1.0 (Initial version) Modified Date 17/02/2012 by Godji
</usage> 
***********************************************************/
string ls_progtxt , ls_fnext , ls_sqlkprpt
string ls_coopid , ls_memcoopid , ls_refmemcoopid
string ls_kpslipno , ls_memno , ls_refmem , ls_memprior
string ls_membtyp , ls_department , ls_memgrp , ls_memsec
string ls_membtyp_desc , ls_department_desc , ls_memgrp_desc , ls_memsec_desc
string ls_keepgrp , ls_keeptype , ls_itemtype , ls_contno , ls_desc
string ls_keepgrpprv , ls_keeptypeprv
string ls_receiptno
integer li_seqno
integer li_period , li_signflag
integer li_rowffee , li_rowother , li_rowshare
integer li_rowdepsav , li_rowdepfix , li_rowdepspc
integer li_rowemer , li_rownorm , li_rowspec
integer li_rowmrt , li_rowtemp
integer li_ret
long ll_rowffee , ll_rowother , ll_rowshare
long ll_rowdepsav , ll_rowdepfix , ll_rowdepspc
long ll_rowemer , ll_rownorm , ll_rowspec
long ll_rowmrt , ll_rowtemp
long ll_index , ll_count , ll_rptrow
dec{2} ldc_prinamt , ldc_intamt , ldc_itemamt , ldc_balance , lds_shrstk

str_extfn_keep lstr_extfn_keep

n_ds lds_keep

choose case is_reporttyp
	case "kptemp"	// งบหน้าจัดเก็บประจำเดือน( ปัจจุบัน )
		ls_progtxt	= "จัดทำรายงานงบหน้าจัดเก็บประจำเดือน( ปัจจุบัน )"
		ls_fnext		= "of_kp_procreport_getsql_kptemp"
	case "kpmast"	// งบหน้าจัดเก็บประจำเดือน( ย้อนหลัง )
		ls_progtxt	= "จัดทำรายงานงบหน้าจัดเก็บประจำเดือน( ย้อนหลัง )"
		ls_fnext		= "of_kp_procreport_getsql_kpmast"
	case "kparre"	// งบหน้าค้างชำระ
		ls_progtxt	= "จัดทำรายงานงบหน้าค้างชำระ"
		ls_fnext		= "of_kp_procreport_getsql_kparre"
	case else
		ithw_exception.text = "พบข้อผิดพลาดในการสร้างงบหน้าเรียกเก็บ"
		ithw_exception.text += "~r~nไม่พบข้อมูลการประมวล"
		return -1
end choose

inv_progress.istr_progress.progress_text		= ls_progtxt
inv_progress.istr_progress.subprogress_text	= "กำลังลบข้อมูลเก่า"
inv_progress.istr_progress.subprogress_index	= 0
inv_progress.istr_progress.subprogress_max	= 0

if this.of_deletereport() <> 1 then
	destroy lds_keep
	ithw_exception.text += "~nพบข้อผิดพลาดในการลบข้อมูล"
	throw ithw_exception	
end if

inv_progress.istr_progress.subprogress_text	= "กำลังดึงข้อมูล"

lds_keep	= create n_ds
lds_keep.settransobject( itr_sqlca )

// get function นอก
lstr_extfn_keep.function_name	= ls_fnext
this.of_extfn( lstr_extfn_keep )
ls_sqlkprpt	= lstr_extfn_keep.function_return

inv_dwsrv.of_create_dw( lds_keep , ls_sqlkprpt , '' )
// จบ get function นอก

inv_kpsrv.of_setsqlselect( "mbmembmaster" )
if inv_kpsrv.of_setsqldw( lds_keep ) <> 1 then
	destroy lds_keep
	ithw_exception.text += "~nพบข้อผิดพลาดในการสร้างชุด SQL สร้างรายงานงบหน้าประจำเดือน"
	return -1
end if

ll_count		= lds_keep.retrieve()

lds_keep.setsort( " coop_id , kpslip_no , seq_no " )
lds_keep.sort()

inv_progress.istr_progress.subprogress_max	= ll_count

for ll_index = 1 to ll_count
	
	inv_progress.istr_progress.subprogress_index	= ll_index
	
	// ตรวจสอบการสั่งหยุดทำงาน
	yield()
	if inv_progress.of_is_stop() then
		destroy lds_keep
		return 0	
	end if
		
	ls_coopid					= trim( lds_keep.getitemstring( ll_index, "coop_id" ) )
	ls_kpslipno				= trim( lds_keep.getitemstring( ll_index, "kpslip_no" ) )
	ls_memcoopid			= trim( lds_keep.getitemstring( ll_index, "memcoop_id" ) )
	ls_memno				= trim( lds_keep.getitemstring( ll_index, "member_no" ) )
	ls_refmemcoopid		= trim( lds_keep.getitemstring( ll_index, "refmemcoop_id" ) )
	ls_refmem				= trim( lds_keep.getitemstring( ll_index, "ref_membno" ) )
	ls_membtyp				= trim( lds_keep.getitemstring( ll_index, "membtype_code" ) )
	ls_department			= trim( lds_keep.getitemstring( ll_index, "department_code" ) )
	ls_memgrp				= trim( lds_keep.getitemstring( ll_index, "membgroup_code" ) )
	ls_memsec				= trim( lds_keep.getitemstring( ll_index, "membsection_code" ) )
	ls_membtyp_desc		= trim( lds_keep.getitemstring( ll_index, "membtype_desc" ) )
	ls_department_desc	= trim( lds_keep.getitemstring( ll_index, "department_desc" ) )
	ls_memgrp_desc		= trim( lds_keep.getitemstring( ll_index, "membgroup_desc" ) )
	ls_memsec_desc		= trim( lds_keep.getitemstring( ll_index, "membsection_desc" ) )
	ls_receiptno				= trim( lds_keep.getitemstring( ll_index, "receipt_no" ) )
	ls_keeptype				= trim( lds_keep.getitemstring( ll_index, "keepitemtype_code" ) )
	ls_itemtype				= trim( lds_keep.getitemstring( ll_index, "shrlontype_code" ) )
	ls_contno				= trim( lds_keep.getitemstring( ll_index, "loancontract_no" ) )
	ls_desc					= trim( lds_keep.getitemstring( ll_index, "description" ) )
	li_period					= lds_keep.getitemnumber( ll_index, "period" )
	li_signflag				= lds_keep.getitemnumber( ll_index, "sign_flag" )
	ldc_prinamt				= lds_keep.getitemdecimal( ll_index, "principal_payment" )
	ldc_intamt				= lds_keep.getitemdecimal( ll_index, "interest_payment" )
	ldc_itemamt				= lds_keep.getitemdecimal( ll_index, "item_payment" )
	ldc_balance				= lds_keep.getitemdecimal( ll_index, "item_balance" )
	lds_shrstk				= lds_keep.getitemdecimal( ll_index, "sharestk_value" )
	
	// ตรวจสอบว่ายังเป็นคนเดิมหรือไม่
	if ls_memno <> ls_memprior then
		ls_memprior	= ls_memno
		
		// ค่าแถวใหม่
		ll_rowffee		= ll_rptrow ; ll_rowother		= ll_rptrow ; ll_rowshare	= ll_rptrow
		ll_rowdepsav	= ll_rptrow ; ll_rowdepfix	= ll_rptrow ; ll_rowdepspc	= ll_rptrow
		ll_rowemer		= ll_rptrow ; ll_rownorm		= ll_rptrow ; ll_rowspec		= ll_rptrow
		ll_rowmrt		= ll_rptrow
		
		// ล้างค่าแถวของแต่ละคน
		li_rowffee		= 0 ; li_rowother		= 0 ; li_rowshare		= 0
		li_rowdepsav	= 0 ; li_rowdepfix		= 0 ; li_rowdepspc	= 0
		li_rowemer		= 0 ; li_rownorm		= 0 ; li_rowspec		= 0
		li_rowmrt		= 0
	end if

	choose case upper(ls_keeptype)
		case "FFE"
			ll_rowffee ++
			li_rowffee ++
			if ll_rowffee > ll_rptrow then
				ll_rptrow		= ids_report.insertrow(0)
				this.of_setreport_mem( ll_rptrow , ls_coopid , ls_kpslipno , li_rowffee , ls_memcoopid , ls_memno , ls_refmemcoopid , ls_refmem , ls_membtyp , ls_department , ls_memgrp , ls_memsec , ls_membtyp_desc , ls_department_desc , ls_memgrp_desc , ls_memsec_desc , ls_receiptno )
				this.of_setreport_ffe( ll_rptrow , li_rowffee , ldc_itemamt )
			else
				this.of_setreport_ffe( ll_rowffee , li_rowffee , ldc_itemamt )
			end if
			
		case "S01" , "DPS"
			ll_rowshare ++
			li_rowshare ++
			if ll_rowshare > ll_rptrow then
				ll_rptrow		= ids_report.insertrow(0)
				this.of_setreport_mem( ll_rptrow , ls_coopid , ls_kpslipno , li_rowshare , ls_memcoopid , ls_memno , ls_refmemcoopid , ls_refmem , ls_membtyp , ls_department , ls_memgrp , ls_memsec , ls_membtyp_desc , ls_department_desc , ls_memgrp_desc , ls_memsec_desc , ls_receiptno )
				this.of_setreport_shr( ll_rptrow , li_rowshare , li_period , ldc_itemamt , lds_shrstk )
			else
				this.of_setreport_shr( ll_rowshare , li_rowshare , li_period , ldc_itemamt , lds_shrstk )
			end if
			
		case "L01", "L02", "L03" , "DPL"
			
			if ls_keeptype = "DPL" then ls_keeptype = ls_desc
			
			choose case ls_keeptype
				case "L01"
					li_rowemer	++
					ll_rowemer ++
					li_rowtemp		= li_rowemer
					ll_rowtemp		= ll_rowemer
				case "L02"
					li_rownorm	++
					ll_rownorm ++
					li_rowtemp		= li_rownorm
					ll_rowtemp		= ll_rownorm
				case "L03"
					li_rowspec ++
					ll_rowspec ++
					li_rowtemp		= li_rowspec
					ll_rowtemp		= ll_rowspec
			end choose
			
			if ll_rowtemp > ll_rptrow then
				ll_rptrow		= ids_report.insertrow(0)
				this.of_setreport_mem( ll_rptrow , ls_coopid , ls_kpslipno , li_rowtemp , ls_memcoopid , ls_memno , ls_refmemcoopid , ls_refmem , ls_membtyp , ls_department , ls_memgrp , ls_memsec , ls_membtyp_desc , ls_department_desc , ls_memgrp_desc , ls_memsec_desc , ls_receiptno )
				this.of_setreport_lon( ll_rptrow , li_rowtemp , ls_keeptype , ls_itemtype , ls_contno , li_period , ldc_prinamt , ldc_intamt , ldc_balance )
			else
				this.of_setreport_lon( ll_rowtemp , li_rowtemp , ls_keeptype , ls_itemtype , ls_contno , li_period , ldc_prinamt , ldc_intamt , ldc_balance )
			end if
		case "D00", "D01", "D02"
			choose case ls_keeptype
				case "D00"
					li_rowdepsav	++
					ll_rowdepsav	++
					li_rowtemp		= li_rowdepsav
					ll_rowtemp		= ll_rowdepsav
				case "D01"
					li_rowdepspc	++
					ll_rowdepspc	++
					li_rowtemp		= li_rowdepspc
					ll_rowtemp		= ll_rowdepspc
				case "D02"
					li_rowdepfix	++
					ll_rowdepfix	++
					li_rowtemp		= li_rowdepfix
					ll_rowtemp		= ll_rowdepfix
			end choose
			
			if ll_rowtemp > ll_rptrow then
				ll_rptrow		= ids_report.insertrow(0)
				this.of_setreport_mem( ll_rptrow , ls_coopid , ls_kpslipno , li_rowtemp , ls_memcoopid , ls_memno , ls_refmemcoopid , ls_refmem , ls_membtyp , ls_department , ls_memgrp , ls_memsec , ls_membtyp_desc , ls_department_desc , ls_memgrp_desc , ls_memsec_desc , ls_receiptno )
				this.of_setreport_dep( ll_rptrow , li_rowtemp , ls_keeptype , ls_itemtype , ls_desc , ldc_itemamt )				
			else
				this.of_setreport_dep( ll_rowtemp , li_rowtemp , ls_keeptype , ls_itemtype , ls_desc , ldc_itemamt )
			end if

		case "OTH", "IAR", "UIA"	// หมวดอื่นๆ ดอกเบี้ยค้างก็ถือเป็นอื่นๆ
			ll_rowother ++
			li_rowother ++
			if ll_rowother > ll_rptrow then
				ll_rptrow		= ids_report.insertrow(0)
				this.of_setreport_mem( ll_rptrow , ls_coopid , ls_kpslipno , li_rowother , ls_memcoopid , ls_memno , ls_refmemcoopid , ls_refmem , ls_membtyp , ls_department , ls_memgrp , ls_memsec , ls_membtyp_desc , ls_department_desc , ls_memgrp_desc , ls_memsec_desc , ls_receiptno )
				this.of_setreport_oth( ll_rptrow , li_rowother , ldc_itemamt )
			else
				this.of_setreport_oth( ll_rowother , li_rowother , ldc_itemamt )
			end if

		case "MRT", "UIR"
//			ldc_itemamt		= ldc_itemamt * li_signflag
//			
//			if li_rowmem = 0 then
//				li_rowmem ++
//				ls_sql	= ls_insert + " , intret_amt ) "+ls_values+" ,"+string(li_rowmem)+", "+string(ldc_itemamt)+" )"
//			else
//				// ไม่มีการเพิ่มแถว เงินคืนมีแถวเดียวเสมอ
//				ls_sql	= ls_update + " intret_amt = intret_amt + "+string(ldc_itemamt)+ls_where+string( 1 )+" )"
//			end if

	end choose
	
next

li_ret	= ids_report.update()

if li_ret <> 1 then
	ithw_exception.text += "ไม่สามารถสร้างข้อมูลรายงานได้"
	ithw_exception.text += "~r~n" + ids_report.of_geterrormessage()
	ithw_exception.text += "กรุณาตรวจสอบ"
	destroy lds_keep
	return -1
end if

//ls_sqlexttemp	= "and ( kptempreceive.recv_period = '"+is_recvperiod+"' ) "
//ls_insert			= " insert into kprcvprocreport ( member_no, recv_period, receipt_no, seq_no"
//ls_update		= " update kprcvprocreport set "
//
//choose case ii_proctype
//	case 1
//		ls_sqlext	= ls_sqlexttemp
//	case 2
//		inv_stringsrv.of_buildsqlext( is_arg[], "membgroup_code", ls_sqlext )
//		ls_sqlext	= " and ( member_no in ( select member_no from mbmembmaster where "+ ls_sqlext + " ) )"
//		ls_sqlext	= ls_sqlexttemp+' and '+ls_sqlext
//	case 3
//		inv_stringsrv.of_buildsqlext( is_arg[], "kptempreceive.member_no", ls_sqlext )
//		ls_sqlext	= ls_sqlexttemp+' and '+ls_sqlext
//end choose
//
//lds_report	= create datastore
//lds_report.dataobject	= "d_kp_rcvproc_report"
//lds_report.settransobject( itr_sqlca )
//
//if ls_sqlext <> "" then
//	ls_temp	= lds_report.getsqlselect()
//	ls_temp	+= ls_sqlext
//	lds_report.setsqlselect( ls_temp )
//	lds_report.settransobject( itr_sqlca )
//end if
//
//ll_count		= lds_report.retrieve()
//inv_progress.istr_progress.subprogress_max	= ll_count
//
//for ll_index = 1 to ll_count
//	
//	inv_progress.istr_progress.subprogress_index	= ll_index
//	
//	// ตรวจสอบการสั่งหยุดทำงาน
//	yield()
//	if inv_progress.of_is_stop() then
//		destroy lds_report
//		return 0
//	end if
//
//	ls_memno		= trim( lds_report.getitemstring( ll_index, "member_no" ) )
//	ls_desc			= trim( lds_report.getitemstring( ll_index, "description" ) )
//	ls_receiptno		= trim( lds_report.getitemstring( ll_index, "receipt_no" ) )
//	ls_keeptype		= trim( lds_report.getitemstring( ll_index, "keepitemtype_code" ) )
//	ls_itemtype		= trim( lds_report.getitemstring( ll_index, "shrlontype_code" ) )
//	ls_contno		= trim( lds_report.getitemstring( ll_index, "loancontract_no" ) )
//	ls_statusdesc	= trim( lds_report.getitemstring( ll_index, "status_desc" ) )
//	li_period			= lds_report.getitemnumber( ll_index, "period" )
//	li_signflag		= lds_report.getitemnumber( ll_index, "sign_flag" )
//	ldc_prinamt		= lds_report.getitemdecimal( ll_index, "principal_payment" )
//	ldc_intamt		= lds_report.getitemdecimal( ll_index, "interest_payment" )
//	ldc_itemamt		= lds_report.getitemdecimal( ll_index, "item_payment" )
//	ldc_balance		= lds_report.getitemdecimal( ll_index, "item_balance" )
//	
//	if isnull( ls_desc ) then ls_desc = ""
//	if isnull( ls_receiptno ) then ls_receiptno = ""
//	if isnull( ls_itemtype ) then ls_itemtype = ""
//	if isnull( ls_contno ) then ls_contno = ""
//	if isnull( ls_statusdesc ) then ls_statusdesc = ""
//	if isnull( li_period ) then li_period = 0
//	if isnull( ldc_prinamt ) then ldc_prinamt = 0.0
//	if isnull( ldc_intamt ) then ldc_intamt = 0.0
//	if isnull( ldc_itemamt ) then ldc_itemamt = 0.0
//	if isnull( ldc_balance ) then ldc_balance = 0.0
//
//	ls_values		= " values ( '"+ls_memno+"',  '"+is_recvperiod+"', '"+ls_receiptno+"'"
//	ls_where		= " where ( member_no = '"+ls_memno+"' ) and ( recv_period = '"+is_recvperiod+"' ) and ( seq_no = "
//
//	// ตรวจสอบว่ายังเป็นคนเดิมหรือไม่
//	if ls_memno <> ls_memprior then
//		ls_memprior	= ls_memno
//		
//		// ล้างค่าแถว
//		li_rowffee		= 0 ; li_rowother	= 0 ; li_rowshare		= 0
//		li_rowdepsav	= 0 ; li_rowdepfix	= 0 ; li_rowdepspc	= 0
//		li_rowemer		= 0 ; li_rownorm	= 0 ; li_rowspec		= 0
//		li_rowmem		= 0 ; li_rowmrt		= 0 ; li_rowtrnemer	= 0
//	end if
//	
//	choose case upper(ls_keeptype)
//		case "FFE"
//			li_rowffee ++
//			if li_rowffee > li_rowmem then
//				li_rowmem ++
//				ls_sql	= ls_insert + " , ffee_amt ) "+ls_values+" ,"+string(li_rowmem)+", "+string(ldc_itemamt)+" )"
//			else
//				ls_sql	= ls_update + " ffee_amt = "+string(ldc_itemamt)+ls_where+string(li_rowffee)+" )"
//			end if
//		case "S01"
//			li_rowshare ++
//			if li_rowshare > li_rowmem then
//				li_rowmem ++
//				ls_sql	= ls_insert + " , share_period, share_value ) "+ls_values+" ,"+string(li_rowmem)+", "+string(li_period)+", "+string(ldc_itemamt)+" )"
//			else
//				ls_sql	= ls_update + " share_period = "+string(li_period)+", share_value = "+string(ldc_itemamt)+ls_where+string(li_rowshare)+" )"
//			end if
//		case "L01", "L02", "L03"
//			string	ls_loantemp
//			choose case ls_keeptype
//				case "L01"
//					ls_loantemp	= "emer"
//					li_rowemer	++
//					li_rowtemp		= li_rowemer
//				case "L02"
//					ls_loantemp	= "norm"
//					li_rownorm	++
//					li_rowtemp		= li_rownorm
//				case "L03"
//					ls_loantemp	= "spec"
//					li_rowspec ++
//					li_rowtemp		= li_rowspec
//			end choose
//			
//			if li_rowtemp > li_rowmem then
//				li_rowmem ++
//				ls_sql	= ls_insert + ", "+ls_loantemp+"_loantype, "+ls_loantemp+"_contract, "+ls_loantemp+"_period, "+ls_loantemp+"_principal, "+ls_loantemp+"_interest, "+ls_loantemp+"_balance, "+ls_loantemp+"_status ) "+ &
//						   ls_values+" ,"+string(li_rowmem)+", '"+ls_itemtype+"', '"+ls_contno+"', "+string(li_period)+", "+string(ldc_prinamt)+", "+string(ldc_intamt)+","+string(ldc_balance)+",'"+ls_statusdesc+"' )"
//			else
//				ls_sql	= ls_update + ls_loantemp+"_loantype = '"+ls_itemtype+"', "+ls_loantemp+"_contract = '"+ls_contno+"', "+ls_loantemp+"_period = "+string(li_period)+", "+ls_loantemp+"_principal = "+string(ldc_prinamt) + &
//						   ", "+ls_loantemp+"_interest = "+string(ldc_intamt)+", "+ls_loantemp+"_balance = "+string(ldc_balance)+", "+ls_loantemp+"_status = '"+ls_statusdesc+"' "+ls_where+string(li_rowtemp)+" )"
//			end if
//		case "D00", "D01", "D02"
//			string	ls_deptemp
//			choose case ls_keeptype
//				case "D00"
//					ls_deptemp	= "depsav"
//					li_rowdepsav	++
//					li_rowtemp		= li_rowdepsav
//				case "D01"
//					ls_deptemp	= "depspc"
//					li_rowdepspc	++
//					li_rowtemp		= li_rowdepspc
//				case "D02"
//					ls_deptemp	= "depfix"
//					li_rowdepfix	++
//					li_rowtemp		= li_rowdepfix
//			end choose
//			
//			if li_rowtemp > li_rowmem then
//				li_rowmem ++
//				ls_sql	= ls_insert + ", "+ls_deptemp+"_deptype, "+ls_deptemp+"_no, "+ls_deptemp+"_amt ) "+ &
//						   ls_values+" ,"+string(li_rowmem)+", '"+ls_itemtype+"', '"+ls_desc+"', "+string(ldc_itemamt)+" )"
//			else
//				ls_sql	= ls_update + ls_deptemp+"_deptype = '"+ls_itemtype+"', "+ls_deptemp+"_no = '"+ls_desc+"', "+ls_deptemp+"_amt = "+string(ldc_itemamt)+ls_where+string(li_rowtemp)+" )"
//			end if
//		case "OTH", "IAR", "UIA"	// หมวดอื่นๆ ดอกเบี้ยค้างก็ถือเป็นอื่นๆ
//			if li_rowmem = 0 then
//				li_rowmem ++
//				ls_sql	= ls_insert + " , other_amt ) "+ls_values+" ,"+string(li_rowmem)+", "+string(ldc_itemamt)+" )"
//			else
//				// ไม่มีการเพิ่มแถว อื่นๆมีแถวเดียวเสมอ
//				ls_sql	= ls_update + " other_amt = other_amt + "+string(ldc_itemamt)+ls_where+string( 1 )+" )"
//			end if
//		case "MRT", "UIR"
//			ldc_itemamt		= ldc_itemamt * li_signflag
//			
//			if li_rowmem = 0 then
//				li_rowmem ++
//				ls_sql	= ls_insert + " , intret_amt ) "+ls_values+" ,"+string(li_rowmem)+", "+string(ldc_itemamt)+" )"
//			else
//				// ไม่มีการเพิ่มแถว เงินคืนมีแถวเดียวเสมอ
//				ls_sql	= ls_update + " intret_amt = intret_amt + "+string(ldc_itemamt)+ls_where+string( 1 )+" )"
//			end if
//		case "ETN"
//			li_rowtrnemer ++
//			if li_rowtrnemer > li_rowmem then
//				li_rowmem ++
//				ls_sql	= ls_insert + " , trnemer_contno, trnemer_amt ) "+ls_values+" ,"+string(li_rowmem)+", '"+ls_contno+"', "+string(ldc_itemamt)+" )"
//			else
//				ls_sql	= ls_update + " trnemer_contno = '"+ls_contno+"', trnemer_amt = "+string(ldc_itemamt)+ls_where+string(li_rowtrnemer)+" )"
//			end if
//	end choose
//	
//	inv_progress.istr_progress.subprogress_text = string(ll_index) +". ทะเบียน >"+ls_memno+" รายการ >"+string(ls_keeptype)+" > "+ls_desc
//
//	execute immediate :ls_sql using itr_sqlca ;
//		
//	if itr_sqlca.sqlcode <> 0 then
//		ithw_exception.text	= "ประมวลผลรายงาน(Long) พบข้อผิดพลาด ~r~n"+itr_sqlca.sqlerrtext
//		if li_chk = 2 then
//			destroy lds_report
//			return -1
//		end if
//	end if
//next

destroy lds_keep

return 1
end function

protected function integer of_setreport_dep (long al_row, integer ai_seqno, string as_deptkeep, string as_depttype, string as_deptno, decimal adc_deptamt) throws Exception;/***********************************************************
<description>
	set ค่าลง datawindows รายงานของระบบเงินฝาก
</description>

<arguments>
	al_row				long			ลำดับในรายการ dw
	ai_seqno				Integer		ลำดับรายการทะเบียนสมาชิก
	as_deptkeep		String			กลุ่มประเภทเงินฝาก
	as_depttype			string			ประเภทเงินฝาก
	as_deptno			string			เลขที่บัญชีเงินฝาก
	adc_deptamt		Decimal		ยอดฝาก
</arguments> 

<return> 
	Integer	1 = success,  Exception = failure
</return> 

<usage> 
	
	Revision History:
	Version 1.0 (Initial version) Modified Date 22/02/2012 by Godji
</usage> 
***********************************************************/
string		ls_cldepttype , ls_cldeptno , ls_cldeptamt

choose case upper( as_deptkeep )
	case "D00"
		ls_cldepttype	= "depsav_deptype"
		ls_cldeptno		= "depsav_no"
		ls_cldeptamt	= "depsav_amt"
	case "D01"
		ls_cldepttype	= "depspc_deptype"
		ls_cldeptno		= "depspc_no"
		ls_cldeptamt	= "depspc_amt"
	case "D02"
		ls_cldepttype	= "depfix_deptype"
		ls_cldeptno		= "depfix_no"
		ls_cldeptamt	= "depfix_amt"		
end choose

ids_report.setitem( al_row , "seq_no" , ai_seqno )
ids_report.setitem( al_row , ls_cldepttype , as_depttype )
ids_report.setitem( al_row , ls_cldeptno , as_deptno )
ids_report.setitem( al_row , ls_cldeptamt , adc_deptamt )

return 1
end function

protected function integer of_setreport_ffe (long al_row, integer ai_seqno, decimal adc_item) throws Exception;/***********************************************************
<description>
	set ค่าลง datawindows รายงาน
</description>

<arguments>
	al_row				long			ลำดับของ dw รายงาน
	ai_seqno				Integer		ลำดับในรายการ
	adc_item				Decimal		ยอดรวมชำระค่าธรรมเนียมแรกเข้า
</arguments> 

<return> 
	Integer	1 = success,  Exception = failure
</return> 

<usage> 
	Revision History:
	Version 1.0 (Initial version) Modified Date 22/02/2012 by Godji
</usage> 
***********************************************************/

ids_report.setitem( al_row , "seq_no" , ai_seqno )
ids_report.setitem( al_row , "ffe_amt" , adc_item )

return 1
end function

protected function integer of_setreport_lon (long al_row, long al_seqno, string as_keeptype, string as_loantype, string as_contract, integer ai_period, decimal adc_prin, decimal adc_int, decimal adc_bal) throws Exception;/***********************************************************
<description>
	set ค่าลง datawindows รายงาน ระบบเงินกู้
</description>

<arguments>
	al_row				String			ลำดับใน dw
	ai_seqno				String			ลำดับ
	as_keeptype			String			กลุ่มประเภทเงินกู้ / ประเงินฝาก
	as_loantype			String			ประเภทเงินกู้ / บัญชีเงินฝาก
	as_contract			String			เลขที่สัญญา
	ai_period				Integer		งวด
	adc_prin				Decimal		ต้นเงิน
	adc_int				Decimal		ดอกเบี้ย
	adc_bal				Decimal		ยอดรวมชำระ
</arguments> 

<return> 
	Integer	1 = success,  Exception = failure
</return> 

<usage> 
	
	Revision History:
	Version 1.0 (Initial version) Modified Date 22/02/2012 by Godji
</usage> 
***********************************************************/
string ls_clloantyp , ls_clcont , ls_clperiod , ls_clprin , ls_clint , ls_clbal

choose case upper( as_keeptype )
	case "L01"
		ls_clloantyp 		= "emer_loantype"
		ls_clcont 			= "emer_contract"
		ls_clperiod 		= "emer_period"
		ls_clprin 			= "emer_prin"
		ls_clint 			= "emer_int" 
		ls_clbal			= "emer_bal"
	case "L02"
		ls_clloantyp 		= "norm_loantype"
		ls_clcont 			= "norm_contract"
		ls_clperiod 		= "norm_period"
		ls_clprin 			= "norm_prin"
		ls_clint 			= "norm_int" 
		ls_clbal			= "norm_bal"
	case "L03"
		ls_clloantyp 		= "spec_loantype"
		ls_clcont 			= "spec_contract"
		ls_clperiod 		= "spec_period"
		ls_clprin 			= "spec_prin"
		ls_clint 			= "spec_int" 
		ls_clbal			= "spec_bal"
end choose

ids_report.setitem( al_row , "seq_no" , al_seqno )
ids_report.setitem( al_row , ls_clloantyp , as_loantype )
ids_report.setitem( al_row , ls_clcont , as_contract )
ids_report.setitem( al_row , ls_clperiod , ai_period )
ids_report.setitem( al_row , ls_clprin , adc_prin )
ids_report.setitem( al_row , ls_clint , adc_int )
ids_report.setitem( al_row , ls_clbal , adc_bal )

return 1
end function

protected function integer of_setreport_mem (long al_row, string as_coopid, string as_kpslipno, integer ai_seqno, string as_memcoopid, string as_memno, string as_refmemcoopid, string as_refmem, string as_membtyp, string as_department, string as_memgrp, string as_memsec, string as_membtyp_desc, string as_department_desc, string as_memgrp_desc, string as_memsec_desc, string as_receiptno) throws Exception;/***********************************************************
<description>
	set ค่าลง datawindows รายงาน
</description>

<arguments>
	al_row						Long			ลำดับ DW
	as_coopid					String			รหัสสาขาสหกรณ์
	as_kpslipno					String			เลขที่ใบทำรายการจัดเก็บ
	as_memcoopid				String			รหัสสาขาทะเบียนสมาชิก
	as_memno					String			ทะเบียนสมาชิก
	as_refmemcoopid			String			รหัสสาขาสหกรณ์อ้างอิงทะเบียนสมาชิก
	as_refmem					String			อ้างอิงทะเบียนสมาชิก
	as_membtyp				String			ประเภทสมาชิก
	as_department				String			ประเภทพนักงาน
	as_memgrp					String			รหัสสังกัด
	as_memsec					String			รหัสสังกัดย่อย
	as_membtyp_desc			String			รายละเอียดประเภทสมาชิก
	as_department_desc		String			รายละเอียดพนักงาน
	as_memgrp_desc			String			รายละเอียดสังกัด
	as_memsec_desc			String			รายละเอียดสังกัดย่อย
	as_receiptno				String			เลขที่ใบเสร็จ
</arguments> 

<return> 
	Integer	1 = success,  Exception = failure
</return> 

<usage> 
	
	Revision History:
	Version 1.0 (Initial version) Modified Date 22/02/2012 by Godji
</usage> 
***********************************************************/
ids_report.setitem( al_row , "coop_id" , as_coopid )
ids_report.setitem( al_row , "recv_period" , is_recvperiod )
ids_report.setitem( al_row , "kpslip_no" , as_kpslipno )
ids_report.setitem( al_row , "seq_no" , ai_seqno )
ids_report.setitem( al_row , "memcoop_id" , as_memcoopid )
ids_report.setitem( al_row , "member_no" , as_memno )
ids_report.setitem( al_row , "refmemcoop_id" , as_refmemcoopid )
ids_report.setitem( al_row , "ref_memno" , as_refmem )
ids_report.setitem( al_row , "membtype_code" , as_membtyp )
ids_report.setitem( al_row , "department_code" , as_department )
ids_report.setitem( al_row , "membgroup_code" , as_memgrp )
ids_report.setitem( al_row , "membsection_code" , as_memsec )
ids_report.setitem( al_row , "membtype_desc" , as_membtyp_desc )
ids_report.setitem( al_row , "department_desc" , as_department_desc )
ids_report.setitem( al_row , "membgroup_desc" , as_memgrp_desc )
ids_report.setitem( al_row , "membsection_desc" , as_memsec_desc )
ids_report.setitem( al_row , "receipt_no" , as_receiptno )

return 1
end function

protected function integer of_setreport_oth (long al_row, long al_seqno, decimal adc_item) throws Exception;/***********************************************************
<description>
	set ค่าลง datawindows รายงาน ยอดอื่นๆ
</description>

<arguments>
	al_row				Long			ลำดับใน dw
	ai_seqno				Integer		ลำดับในรายการ
	adc_item				Decimal		ยอดชำระ
</arguments> 

<return> 
	Integer	1 = success,  Exception = failure
</return> 

<usage> 
	
	Revision History:
	Version 1.0 (Initial version) Modified Date 22/02/2012 by Godji
</usage> 
***********************************************************/
//dec{2} ldc_item

//ldc_item		= ids_report.object.other_amt[al_row]
//if isnull( ldc_item ) then ldc_item = 0
//
//ldc_item += adc_item

ids_report.setitem( al_row , "seq_no" , al_seqno )
ids_report.setitem( al_row , "other_amt" , adc_item )

return 1
end function

protected function integer of_setreport_shr (long al_row, integer ai_seqno, integer ai_period, decimal adc_shrvalue, decimal adc_shrstkvalue) throws Exception;/***********************************************************
<description>
	set ค่าลง datawindows รายงาน
</description>

<arguments>
	al_row				long			ลำดับใน dw รายงาน
	ai_seqno				Integer		ลำดับในรายการ
	ai_period				Integer		งวดหุ้น
	adc_shrvalue		Decimal		มูลค่าชำระหุ้นต่อเดือน
	adc_shrstkvalue	Decimal		มูลค่าหุ้นรวม
</arguments> 

<return> 
	Integer	1 = success,  Exception = failure
</return> 

<usage> 
	
	Revision History:
	Version 1.0 (Initial version) Modified Date 22/02/2012 by Godji
</usage> 
***********************************************************/

ids_report.setitem( al_row , "seq_no" , ai_seqno )
ids_report.setitem( al_row , "share_period" , ai_period )
ids_report.setitem( al_row , "share_value" , adc_shrvalue )
ids_report.setitem( al_row , "sharestk_value" , adc_shrstkvalue )

return 1
end function

protected function integer of_setsrvdw (boolean ab_switch) throws Exception;// Check argument
if isnull( ab_switch ) then
	return -1
end if

if ab_switch then
	if isnull( inv_dwsrv ) or not isvalid( inv_dwsrv ) then
		inv_dwsrv	= create n_cst_datawindowsservice
		inv_dwsrv.of_initservice( inv_transection )
		return 1
	end if
else
	if isvalid( inv_dwsrv ) then
		destroy inv_dwsrv
		return 1
	end if
end if

return 0
end function

protected function integer of_processreporthnd () throws Exception;/***********************************************************
<description>
	ประมวลผลรายงาน
</description>

<arguments>

</arguments> 

<return> 
	Integer	1 = success,  Exception = failure
</return> 

<usage> 
	Revision History:
	Version 1.0 (Initial version) Modified Date 17/02/2012 by Godji
</usage> 
***********************************************************/
string ls_progtxt , ls_fnext , ls_sqlkprpt
string ls_coopid , ls_memcoopid , ls_refmemcoopid
string ls_kpslipno , ls_memno , ls_refmem , ls_memprior
string ls_membtyp , ls_department , ls_memgrp , ls_memsec
string ls_membtyp_desc , ls_department_desc , ls_memgrp_desc , ls_memsec_desc
string ls_keepgrp , ls_keeptype , ls_itemtype , ls_contno , ls_desc
string ls_keepgrpprv , ls_keeptypeprv
string ls_receiptno
integer li_seqno
integer li_period , li_signflag
integer li_rowffee , li_rowother , li_rowshare
integer li_rowdepsav , li_rowdepfix , li_rowdepspc
integer li_rowemer , li_rownorm , li_rowspec
integer li_rowmrt , li_rowtemp
integer li_ret
long ll_rowffee , ll_rowother , ll_rowshare
long ll_rowdepsav , ll_rowdepfix , ll_rowdepspc
long ll_rowemer , ll_rownorm , ll_rowspec
long ll_rowmrt , ll_rowtemp
long ll_index , ll_count , ll_rptrow
dec{2} ldc_prinamt , ldc_intamt , ldc_itemamt , ldc_balance , lds_shrstk

str_extfn_keep lstr_extfn_keep

n_ds lds_keep

choose case is_reporttyp
	case "kptemp"	// งบหน้าจัดเก็บประจำเดือน( ปัจจุบัน )
		ls_progtxt	= "จัดทำรายงานงบหน้าจัดเก็บประจำเดือน( ปัจจุบัน )"
		ls_fnext		= "of_kp_procreport_getsql_kptemp"
	case "kpmast"	// งบหน้าจัดเก็บประจำเดือน( ย้อนหลัง )
		ls_progtxt	= "จัดทำรายงานงบหน้าจัดเก็บประจำเดือน( ย้อนหลัง )"
		ls_fnext		= "of_kp_procreport_getsql_kpmast"
	case "kparre"	// งบหน้าค้างชำระ
		ls_progtxt	= "จัดทำรายงานงบหน้าค้างชำระ"
		ls_fnext		= "of_kp_procreport_getsql_kparre"
	case else
		ithw_exception.text = "พบข้อผิดพลาดในการสร้างงบหน้าเรียกเก็บ"
		ithw_exception.text += "~r~nไม่พบข้อมูลการประมวล"
		return -1
end choose

inv_progress.istr_progress.progress_text		= ls_progtxt
inv_progress.istr_progress.subprogress_text	= "กำลังลบข้อมูลเก่า"
inv_progress.istr_progress.subprogress_index	= 0
inv_progress.istr_progress.subprogress_max	= 0

if this.of_deletereport() <> 1 then
	destroy lds_keep
	ithw_exception.text += "~nพบข้อผิดพลาดในการลบข้อมูล"
	throw ithw_exception	
end if

inv_progress.istr_progress.subprogress_text	= "กำลังดึงข้อมูล"

lds_keep	= create n_ds
lds_keep.settransobject( itr_sqlca )

// get function นอก
lstr_extfn_keep.function_name	= ls_fnext
this.of_extfn( lstr_extfn_keep )
ls_sqlkprpt	= lstr_extfn_keep.function_return

inv_dwsrv.of_create_dw( lds_keep , ls_sqlkprpt , '' )
// จบ get function นอก

inv_kpsrv.of_setsqlselect( "mbmembmaster" )
if inv_kpsrv.of_setsqldw( lds_keep ) <> 1 then
	destroy lds_keep
	ithw_exception.text += "~nพบข้อผิดพลาดในการสร้างชุด SQL สร้างรายงานงบหน้าประจำเดือน"
	return -1
end if

ll_count		= lds_keep.retrieve()

lds_keep.setsort( " coop_id , kpslip_no , seq_no " )
lds_keep.sort()

inv_progress.istr_progress.subprogress_max	= ll_count

for ll_index = 1 to ll_count
	
	inv_progress.istr_progress.subprogress_index	= ll_index
	
	// ตรวจสอบการสั่งหยุดทำงาน
	yield()
	if inv_progress.of_is_stop() then
		destroy lds_keep
		return 0	
	end if
		
	ls_coopid					= trim( lds_keep.getitemstring( ll_index, "coop_id" ) )
	ls_kpslipno				= trim( lds_keep.getitemstring( ll_index, "kpslip_no" ) )
	ls_memcoopid			= trim( lds_keep.getitemstring( ll_index, "memcoop_id" ) )
	ls_memno				= trim( lds_keep.getitemstring( ll_index, "member_no" ) )
	ls_refmemcoopid		= trim( lds_keep.getitemstring( ll_index, "refmemcoop_id" ) )
	ls_refmem				= trim( lds_keep.getitemstring( ll_index, "ref_membno" ) )
	ls_membtyp				= trim( lds_keep.getitemstring( ll_index, "membtype_code" ) )
	ls_department			= trim( lds_keep.getitemstring( ll_index, "department_code" ) )
	ls_memgrp				= trim( lds_keep.getitemstring( ll_index, "membgroup_code" ) )
	ls_memsec				= trim( lds_keep.getitemstring( ll_index, "membsection_code" ) )
	ls_membtyp_desc		= trim( lds_keep.getitemstring( ll_index, "membtype_desc" ) )
	ls_department_desc	= trim( lds_keep.getitemstring( ll_index, "department_desc" ) )
	ls_memgrp_desc		= trim( lds_keep.getitemstring( ll_index, "membgroup_desc" ) )
	ls_memsec_desc		= trim( lds_keep.getitemstring( ll_index, "membsection_desc" ) )
	ls_receiptno				= trim( lds_keep.getitemstring( ll_index, "receipt_no" ) )
	ls_keeptype				= trim( lds_keep.getitemstring( ll_index, "keepitemtype_code" ) )
	ls_itemtype				= trim( lds_keep.getitemstring( ll_index, "shrlontype_code" ) )
	ls_contno				= trim( lds_keep.getitemstring( ll_index, "loancontract_no" ) )
	ls_desc					= trim( lds_keep.getitemstring( ll_index, "description" ) )
	li_period					= lds_keep.getitemnumber( ll_index, "period" )
	li_signflag				= lds_keep.getitemnumber( ll_index, "sign_flag" )
	ldc_prinamt				= lds_keep.getitemdecimal( ll_index, "principal_payment" )
	ldc_intamt				= lds_keep.getitemdecimal( ll_index, "interest_payment" )
	ldc_itemamt				= lds_keep.getitemdecimal( ll_index, "item_payment" )
	ldc_balance				= lds_keep.getitemdecimal( ll_index, "item_balance" )
	lds_shrstk				= lds_keep.getitemdecimal( ll_index, "sharestk_value" )
	
	// ตรวจสอบว่ายังเป็นคนเดิมหรือไม่
	if ls_memno <> ls_memprior then
		ls_memprior	= ls_memno
		
		// ค่าแถวใหม่
		ll_rowffee		= ll_rptrow ; ll_rowother		= ll_rptrow ; ll_rowshare	= ll_rptrow
		ll_rowdepsav	= ll_rptrow ; ll_rowdepfix	= ll_rptrow ; ll_rowdepspc	= ll_rptrow
		ll_rowemer		= ll_rptrow ; ll_rownorm		= ll_rptrow ; ll_rowspec		= ll_rptrow
		ll_rowmrt		= ll_rptrow
		
		// ล้างค่าแถวของแต่ละคน
		li_rowffee		= 0 ; li_rowother		= 0 ; li_rowshare		= 0
		li_rowdepsav	= 0 ; li_rowdepfix		= 0 ; li_rowdepspc	= 0
		li_rowemer		= 0 ; li_rownorm		= 0 ; li_rowspec		= 0
		li_rowmrt		= 0
	end if

	choose case upper(ls_keeptype)
		case "FFE"
			ll_rowffee ++
			li_rowffee ++
			if ll_rowffee > ll_rptrow then
				ll_rptrow		= ids_report.insertrow(0)
				this.of_setreport_mem( ll_rptrow , ls_coopid , ls_kpslipno , li_rowffee , ls_memcoopid , ls_memno , ls_refmemcoopid , ls_refmem , ls_membtyp , ls_department , ls_memgrp , ls_memsec , ls_membtyp_desc , ls_department_desc , ls_memgrp_desc , ls_memsec_desc , ls_receiptno )
				this.of_setreport_ffe( ll_rptrow , li_rowffee , ldc_itemamt )
			else
				this.of_setreport_ffe( ll_rowffee , li_rowffee , ldc_itemamt )
			end if
			
		case "S01" , "DPS"
			ll_rowshare ++
			li_rowshare ++
			if ll_rowshare > ll_rptrow then
				ll_rptrow		= ids_report.insertrow(0)
				this.of_setreport_mem( ll_rptrow , ls_coopid , ls_kpslipno , li_rowshare , ls_memcoopid , ls_memno , ls_refmemcoopid , ls_refmem , ls_membtyp , ls_department , ls_memgrp , ls_memsec , ls_membtyp_desc , ls_department_desc , ls_memgrp_desc , ls_memsec_desc , ls_receiptno )
				this.of_setreport_shr( ll_rptrow , li_rowshare , li_period , ldc_itemamt , lds_shrstk )
			else
				this.of_setreport_shr( ll_rowshare , li_rowshare , li_period , ldc_itemamt , lds_shrstk )
			end if
			
		case "L01", "L02", "L03", "L04" , "DPL"
			
			if ls_keeptype = "DPL" then ls_keeptype = ls_desc
			
			choose case ls_keeptype
				case "L01"
					li_rowemer	++
					ll_rowemer ++
					li_rowtemp		= li_rowemer
					ll_rowtemp		= ll_rowemer
				case "L02"
					li_rownorm	++
					ll_rownorm ++
					li_rowtemp		= li_rownorm
					ll_rowtemp		= ll_rownorm
				case "L03"
					li_rownorm	++
					ll_rownorm ++
					li_rowtemp		= li_rownorm
					ll_rowtemp		= ll_rownorm
				case "L04"
					li_rowspec ++
					ll_rowspec ++
					li_rowtemp		= li_rowspec
					ll_rowtemp		= ll_rowspec
			end choose
			
			if ll_rowtemp > ll_rptrow then
				ll_rptrow		= ids_report.insertrow(0)
				this.of_setreport_mem( ll_rptrow , ls_coopid , ls_kpslipno , li_rowtemp , ls_memcoopid , ls_memno , ls_refmemcoopid , ls_refmem , ls_membtyp , ls_department , ls_memgrp , ls_memsec , ls_membtyp_desc , ls_department_desc , ls_memgrp_desc , ls_memsec_desc , ls_receiptno )
				this.of_setreport_lon( ll_rptrow , li_rowtemp , ls_keeptype , ls_itemtype , ls_contno , li_period , ldc_prinamt , ldc_intamt , ldc_balance )
			else
				this.of_setreport_lon( ll_rowtemp , li_rowtemp , ls_keeptype , ls_itemtype , ls_contno , li_period , ldc_prinamt , ldc_intamt , ldc_balance )
			end if
		case "D00", "D01", "D02"
			choose case ls_keeptype
				case "D00"
					li_rowdepsav	++
					ll_rowdepsav	++
					li_rowtemp		= li_rowdepsav
					ll_rowtemp		= ll_rowdepsav
				case "D01"
					li_rowdepspc	++
					ll_rowdepspc	++
					li_rowtemp		= li_rowdepspc
					ll_rowtemp		= ll_rowdepspc
				case "D02"
					li_rowdepfix	++
					ll_rowdepfix	++
					li_rowtemp		= li_rowdepfix
					ll_rowtemp		= ll_rowdepfix
			end choose
			
			if ll_rowtemp > ll_rptrow then
				ll_rptrow		= ids_report.insertrow(0)
				this.of_setreport_mem( ll_rptrow , ls_coopid , ls_kpslipno , li_rowtemp , ls_memcoopid , ls_memno , ls_refmemcoopid , ls_refmem , ls_membtyp , ls_department , ls_memgrp , ls_memsec , ls_membtyp_desc , ls_department_desc , ls_memgrp_desc , ls_memsec_desc , ls_receiptno )
				this.of_setreport_dep( ll_rptrow , li_rowtemp , ls_keeptype , ls_itemtype , ls_desc , ldc_itemamt )				
			else
				this.of_setreport_dep( ll_rowtemp , li_rowtemp , ls_keeptype , ls_itemtype , ls_desc , ldc_itemamt )
			end if

		case "OTH", "IAR", "UIA"	// หมวดอื่นๆ ดอกเบี้ยค้างก็ถือเป็นอื่นๆ
			ll_rowother ++
			li_rowother ++
			if ll_rowother > ll_rptrow then
				ll_rptrow		= ids_report.insertrow(0)
				this.of_setreport_mem( ll_rptrow , ls_coopid , ls_kpslipno , li_rowother , ls_memcoopid , ls_memno , ls_refmemcoopid , ls_refmem , ls_membtyp , ls_department , ls_memgrp , ls_memsec , ls_membtyp_desc , ls_department_desc , ls_memgrp_desc , ls_memsec_desc , ls_receiptno )
				this.of_setreport_oth( ll_rptrow , li_rowother , ldc_itemamt )
			else
				this.of_setreport_oth( ll_rowother , li_rowother , ldc_itemamt )
			end if

		case "MRT", "UIR"
//			ldc_itemamt		= ldc_itemamt * li_signflag
//			
//			if li_rowmem = 0 then
//				li_rowmem ++
//				ls_sql	= ls_insert + " , intret_amt ) "+ls_values+" ,"+string(li_rowmem)+", "+string(ldc_itemamt)+" )"
//			else
//				// ไม่มีการเพิ่มแถว เงินคืนมีแถวเดียวเสมอ
//				ls_sql	= ls_update + " intret_amt = intret_amt + "+string(ldc_itemamt)+ls_where+string( 1 )+" )"
//			end if

	end choose
	
next

li_ret	= ids_report.update()

if li_ret <> 1 then
	ithw_exception.text += "ไม่สามารถสร้างข้อมูลรายงานได้"
	ithw_exception.text += "~r~n" + ids_report.of_geterrormessage()
	ithw_exception.text += "กรุณาตรวจสอบ"
	destroy lds_keep
	return -1
end if

//ls_sqlexttemp	= "and ( kptempreceive.recv_period = '"+is_recvperiod+"' ) "
//ls_insert			= " insert into kprcvprocreport ( member_no, recv_period, receipt_no, seq_no"
//ls_update		= " update kprcvprocreport set "
//
//choose case ii_proctype
//	case 1
//		ls_sqlext	= ls_sqlexttemp
//	case 2
//		inv_stringsrv.of_buildsqlext( is_arg[], "membgroup_code", ls_sqlext )
//		ls_sqlext	= " and ( member_no in ( select member_no from mbmembmaster where "+ ls_sqlext + " ) )"
//		ls_sqlext	= ls_sqlexttemp+' and '+ls_sqlext
//	case 3
//		inv_stringsrv.of_buildsqlext( is_arg[], "kptempreceive.member_no", ls_sqlext )
//		ls_sqlext	= ls_sqlexttemp+' and '+ls_sqlext
//end choose
//
//lds_report	= create datastore
//lds_report.dataobject	= "d_kp_rcvproc_report"
//lds_report.settransobject( itr_sqlca )
//
//if ls_sqlext <> "" then
//	ls_temp	= lds_report.getsqlselect()
//	ls_temp	+= ls_sqlext
//	lds_report.setsqlselect( ls_temp )
//	lds_report.settransobject( itr_sqlca )
//end if
//
//ll_count		= lds_report.retrieve()
//inv_progress.istr_progress.subprogress_max	= ll_count
//
//for ll_index = 1 to ll_count
//	
//	inv_progress.istr_progress.subprogress_index	= ll_index
//	
//	// ตรวจสอบการสั่งหยุดทำงาน
//	yield()
//	if inv_progress.of_is_stop() then
//		destroy lds_report
//		return 0
//	end if
//
//	ls_memno		= trim( lds_report.getitemstring( ll_index, "member_no" ) )
//	ls_desc			= trim( lds_report.getitemstring( ll_index, "description" ) )
//	ls_receiptno		= trim( lds_report.getitemstring( ll_index, "receipt_no" ) )
//	ls_keeptype		= trim( lds_report.getitemstring( ll_index, "keepitemtype_code" ) )
//	ls_itemtype		= trim( lds_report.getitemstring( ll_index, "shrlontype_code" ) )
//	ls_contno		= trim( lds_report.getitemstring( ll_index, "loancontract_no" ) )
//	ls_statusdesc	= trim( lds_report.getitemstring( ll_index, "status_desc" ) )
//	li_period			= lds_report.getitemnumber( ll_index, "period" )
//	li_signflag		= lds_report.getitemnumber( ll_index, "sign_flag" )
//	ldc_prinamt		= lds_report.getitemdecimal( ll_index, "principal_payment" )
//	ldc_intamt		= lds_report.getitemdecimal( ll_index, "interest_payment" )
//	ldc_itemamt		= lds_report.getitemdecimal( ll_index, "item_payment" )
//	ldc_balance		= lds_report.getitemdecimal( ll_index, "item_balance" )
//	
//	if isnull( ls_desc ) then ls_desc = ""
//	if isnull( ls_receiptno ) then ls_receiptno = ""
//	if isnull( ls_itemtype ) then ls_itemtype = ""
//	if isnull( ls_contno ) then ls_contno = ""
//	if isnull( ls_statusdesc ) then ls_statusdesc = ""
//	if isnull( li_period ) then li_period = 0
//	if isnull( ldc_prinamt ) then ldc_prinamt = 0.0
//	if isnull( ldc_intamt ) then ldc_intamt = 0.0
//	if isnull( ldc_itemamt ) then ldc_itemamt = 0.0
//	if isnull( ldc_balance ) then ldc_balance = 0.0
//
//	ls_values		= " values ( '"+ls_memno+"',  '"+is_recvperiod+"', '"+ls_receiptno+"'"
//	ls_where		= " where ( member_no = '"+ls_memno+"' ) and ( recv_period = '"+is_recvperiod+"' ) and ( seq_no = "
//
//	// ตรวจสอบว่ายังเป็นคนเดิมหรือไม่
//	if ls_memno <> ls_memprior then
//		ls_memprior	= ls_memno
//		
//		// ล้างค่าแถว
//		li_rowffee		= 0 ; li_rowother	= 0 ; li_rowshare		= 0
//		li_rowdepsav	= 0 ; li_rowdepfix	= 0 ; li_rowdepspc	= 0
//		li_rowemer		= 0 ; li_rownorm	= 0 ; li_rowspec		= 0
//		li_rowmem		= 0 ; li_rowmrt		= 0 ; li_rowtrnemer	= 0
//	end if
//	
//	choose case upper(ls_keeptype)
//		case "FFE"
//			li_rowffee ++
//			if li_rowffee > li_rowmem then
//				li_rowmem ++
//				ls_sql	= ls_insert + " , ffee_amt ) "+ls_values+" ,"+string(li_rowmem)+", "+string(ldc_itemamt)+" )"
//			else
//				ls_sql	= ls_update + " ffee_amt = "+string(ldc_itemamt)+ls_where+string(li_rowffee)+" )"
//			end if
//		case "S01"
//			li_rowshare ++
//			if li_rowshare > li_rowmem then
//				li_rowmem ++
//				ls_sql	= ls_insert + " , share_period, share_value ) "+ls_values+" ,"+string(li_rowmem)+", "+string(li_period)+", "+string(ldc_itemamt)+" )"
//			else
//				ls_sql	= ls_update + " share_period = "+string(li_period)+", share_value = "+string(ldc_itemamt)+ls_where+string(li_rowshare)+" )"
//			end if
//		case "L01", "L02", "L03"
//			string	ls_loantemp
//			choose case ls_keeptype
//				case "L01"
//					ls_loantemp	= "emer"
//					li_rowemer	++
//					li_rowtemp		= li_rowemer
//				case "L02"
//					ls_loantemp	= "norm"
//					li_rownorm	++
//					li_rowtemp		= li_rownorm
//				case "L03"
//					ls_loantemp	= "spec"
//					li_rowspec ++
//					li_rowtemp		= li_rowspec
//			end choose
//			
//			if li_rowtemp > li_rowmem then
//				li_rowmem ++
//				ls_sql	= ls_insert + ", "+ls_loantemp+"_loantype, "+ls_loantemp+"_contract, "+ls_loantemp+"_period, "+ls_loantemp+"_principal, "+ls_loantemp+"_interest, "+ls_loantemp+"_balance, "+ls_loantemp+"_status ) "+ &
//						   ls_values+" ,"+string(li_rowmem)+", '"+ls_itemtype+"', '"+ls_contno+"', "+string(li_period)+", "+string(ldc_prinamt)+", "+string(ldc_intamt)+","+string(ldc_balance)+",'"+ls_statusdesc+"' )"
//			else
//				ls_sql	= ls_update + ls_loantemp+"_loantype = '"+ls_itemtype+"', "+ls_loantemp+"_contract = '"+ls_contno+"', "+ls_loantemp+"_period = "+string(li_period)+", "+ls_loantemp+"_principal = "+string(ldc_prinamt) + &
//						   ", "+ls_loantemp+"_interest = "+string(ldc_intamt)+", "+ls_loantemp+"_balance = "+string(ldc_balance)+", "+ls_loantemp+"_status = '"+ls_statusdesc+"' "+ls_where+string(li_rowtemp)+" )"
//			end if
//		case "D00", "D01", "D02"
//			string	ls_deptemp
//			choose case ls_keeptype
//				case "D00"
//					ls_deptemp	= "depsav"
//					li_rowdepsav	++
//					li_rowtemp		= li_rowdepsav
//				case "D01"
//					ls_deptemp	= "depspc"
//					li_rowdepspc	++
//					li_rowtemp		= li_rowdepspc
//				case "D02"
//					ls_deptemp	= "depfix"
//					li_rowdepfix	++
//					li_rowtemp		= li_rowdepfix
//			end choose
//			
//			if li_rowtemp > li_rowmem then
//				li_rowmem ++
//				ls_sql	= ls_insert + ", "+ls_deptemp+"_deptype, "+ls_deptemp+"_no, "+ls_deptemp+"_amt ) "+ &
//						   ls_values+" ,"+string(li_rowmem)+", '"+ls_itemtype+"', '"+ls_desc+"', "+string(ldc_itemamt)+" )"
//			else
//				ls_sql	= ls_update + ls_deptemp+"_deptype = '"+ls_itemtype+"', "+ls_deptemp+"_no = '"+ls_desc+"', "+ls_deptemp+"_amt = "+string(ldc_itemamt)+ls_where+string(li_rowtemp)+" )"
//			end if
//		case "OTH", "IAR", "UIA"	// หมวดอื่นๆ ดอกเบี้ยค้างก็ถือเป็นอื่นๆ
//			if li_rowmem = 0 then
//				li_rowmem ++
//				ls_sql	= ls_insert + " , other_amt ) "+ls_values+" ,"+string(li_rowmem)+", "+string(ldc_itemamt)+" )"
//			else
//				// ไม่มีการเพิ่มแถว อื่นๆมีแถวเดียวเสมอ
//				ls_sql	= ls_update + " other_amt = other_amt + "+string(ldc_itemamt)+ls_where+string( 1 )+" )"
//			end if
//		case "MRT", "UIR"
//			ldc_itemamt		= ldc_itemamt * li_signflag
//			
//			if li_rowmem = 0 then
//				li_rowmem ++
//				ls_sql	= ls_insert + " , intret_amt ) "+ls_values+" ,"+string(li_rowmem)+", "+string(ldc_itemamt)+" )"
//			else
//				// ไม่มีการเพิ่มแถว เงินคืนมีแถวเดียวเสมอ
//				ls_sql	= ls_update + " intret_amt = intret_amt + "+string(ldc_itemamt)+ls_where+string( 1 )+" )"
//			end if
//		case "ETN"
//			li_rowtrnemer ++
//			if li_rowtrnemer > li_rowmem then
//				li_rowmem ++
//				ls_sql	= ls_insert + " , trnemer_contno, trnemer_amt ) "+ls_values+" ,"+string(li_rowmem)+", '"+ls_contno+"', "+string(ldc_itemamt)+" )"
//			else
//				ls_sql	= ls_update + " trnemer_contno = '"+ls_contno+"', trnemer_amt = "+string(ldc_itemamt)+ls_where+string(li_rowtrnemer)+" )"
//			end if
//	end choose
//	
//	inv_progress.istr_progress.subprogress_text = string(ll_index) +". ทะเบียน >"+ls_memno+" รายการ >"+string(ls_keeptype)+" > "+ls_desc
//
//	execute immediate :ls_sql using itr_sqlca ;
//		
//	if itr_sqlca.sqlcode <> 0 then
//		ithw_exception.text	= "ประมวลผลรายงาน(Long) พบข้อผิดพลาด ~r~n"+itr_sqlca.sqlerrtext
//		if li_chk = 2 then
//			destroy lds_report
//			return -1
//		end if
//	end if
//next

destroy lds_keep

return 1
end function

on n_cst_keeping_procreport.create
call super::create
TriggerEvent( this, "constructor" )
end on

on n_cst_keeping_procreport.destroy
TriggerEvent( this, "destructor" )
call super::destroy
end on

event constructor;ithw_exception = create Exception
end event

event destructor;if isvalid( ithw_exception ) then destroy ithw_exception
if isvalid( inv_stringsrv ) then destroy inv_stringsrv
if isvalid( inv_progress ) then destroy inv_progress
if isvalid( inv_transection ) then destroy inv_transection
if isvalid( inv_kpsrv ) then destroy inv_kpsrv

if isvalid( ids_msgerr ) then destroy ids_msgerr
if isvalid( ids_report ) then destroy ids_report
end event
