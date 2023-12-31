$PBExportHeader$n_cst_money_return_opr.sru
$PBExportComments$service ทำรายการคืนเงินสมาชิก

forward
global type n_cst_money_return_opr from NonVisualObject
end type
type stri_lnstatement from Structure within n_cst_money_return_opr
end type
end forward

type stri_lnstatement from Structure
	string loancontract_no
	datetime slip_date
	datetime operate_date
	datetime account_date
	string ref_slipno
	string ref_docno
	string loanitemtype_code
	integer period
	decimal principal_payment
	decimal interest_payment
	decimal principal_balance
	decimal prncalint_amt
	datetime calint_from
	datetime calint_to
	decimal bfinterest_arrear
	decimal bfinterest_return
	decimal interest_period
	decimal interest_arrear
	decimal interest_return
	string moneytype_code
	string remark
	integer item_status
	string entry_id
end type

/// <summary> service ทำรายการคืนเงินสมาชิก
/// 
/// </summary>
global type n_cst_money_return_opr from NonVisualObject
end type
global n_cst_money_return_opr n_cst_money_return_opr

type variables
n_cst_progresscontrol					inv_progress
n_cst_dbconnectservice					inv_transection
n_cst_dwxmlieservice						inv_dwxmliesrv
n_cst_money_return_proc_service		inv_mrprocsrv
n_cst_procservice							inv_procsrv
n_cst_doccontrolservice					inv_docsrv
n_cst_loansrv_interest					inv_intsrv

transaction	itr_sqlca
Exception	ithw_exception

constant integer	STATUS_CLOSE = -1
end variables

forward prototypes
public function integer of_initservice (n_cst_dbconnectservice atr_dbtrans) throws Exception
public function integer of_setprogress (ref n_cst_progresscontrol anv_progress) throws Exception
public function integer of_init_mr_opr_proc (ref str_money_return_xml astr_mr_xml) throws Exception
public function integer of_save_mr_opr_proc (ref str_money_return_xml astr_mr_xml) throws Exception
protected function integer of_post_lncont_stm (str_money_return_lncontstm astr_lnstatement) throws Exception
protected function integer of_post_mr_opr_proc_crt (n_ds ads_option) throws Exception
protected function integer of_post_mr_opr_proc_lon (n_ds ads_option) throws Exception
protected function integer of_post_mr_payintarr (ref n_ds ads_slippayout, ref n_ds ads_slippayoutdet, ref n_ds ads_slippayin, ref n_ds ads_slippayindet) throws Exception
protected function integer of_post_shshare_stm (str_money_return_shsharestm astr_shstatement) throws Exception
protected function integer of_post_slslippayin (n_ds ads_slippayin, n_ds ads_slippayindet) throws Exception
protected function integer of_post_slslippayout (n_ds ads_slippayout, n_ds ads_slippayoutdet) throws Exception
protected function integer of_setsrvdoccontrol (boolean ab_switch)
protected function integer of_setsrvdwxmlie (boolean ab_switch)
protected function integer of_setsrvmrproc (boolean ab_switch)
protected function integer of_setsrvproc (boolean ab_switch)
public function integer of_post_mr_opr_proc_crt_msk (n_ds ads_option) throws Exception
public function integer of_test_mr_payint_surin (n_ds ads_option, datetime adtm_calintto) throws Exception
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
// Service Transection
if isnull( inv_transection ) or not isvalid( inv_transection ) then
	inv_transection	= create n_cst_dbconnectservice
end if

inv_transection	= atr_dbtrans
itr_sqlca 			= inv_transection.itr_dbconnection

//itr_sqlca = atr_dbtrans.itr_dbconnection

// สร้าง Progress สำหรับแสดงสถานะการประมวลผล
inv_progress = create n_cst_progresscontrol

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

public function integer of_init_mr_opr_proc (ref str_money_return_xml astr_mr_xml) throws Exception;/***********************************************************
<description>
	ดึงข้อมูลเงินคืนตามเงื่อนไขที่ระบุ
</description>

<arguments>  
		str_mr_xml.xml_option			String		dwxml เงื่อนไขการดึงข้อมูล
		str_mr_xml.xml_detail{Ref}		String		dwxml ข้อมูลการต้นคืนดอกเบี้ยคืนตามเงื่อนไขที่ทำรายการ
</arguments> 

<return> 
	Integer	1 = success,  Exception = failure
</return> 

<usage> 	
	Revision History:
	Version 1.0 (Initial version) Modified Date 25/07/2012 by Godji
</usage> 
***********************************************************/
string ls_coopid , ls_proctext
integer li_proctype
datetime ldtm_operate

n_ds lds_option  , lds_rptsum

lds_option = create n_ds
lds_option.dataobject = "d_mr_opr_proc_option"
lds_option.settransobject( itr_sqlca )

lds_rptsum = create n_ds
lds_rptsum.dataobject = "d_mr_opr_proc_rpt_sum"
lds_rptsum.settransobject( itr_sqlca )

this.of_setsrvmrproc(true)
this.of_setsrvdwxmlie(true)

if inv_dwxmliesrv.of_xmlimport( lds_option , astr_mr_xml.xml_option ) < 1 then 
	destroy lds_option   ; destroy lds_rptsum
	ithw_exception.text += "~nไม่พบข้อมูลเงื่อนไขการดึงข้อมูลเงินคืน"
	throw ithw_exception	
end if

ls_coopid					= lds_option.object.coop_id[1]
ls_proctext				= lds_option.object.proc_text[1]		; if isnull( ls_proctext ) then ls_proctext = ""
li_proctype				= lds_option.object.proc_type[1]
ldtm_operate			= lds_option.object.operate_date[1]

inv_mrprocsrv.of_setproctype( li_proctype )
inv_mrprocsrv.of_set_coopid( ls_coopid )
inv_mrprocsrv.of_set_proctext( ls_proctext )
inv_mrprocsrv.of_setanalyze()

inv_mrprocsrv.of_setsqlselect( "mbmembmaster" )
if inv_mrprocsrv.of_setsqldw( lds_rptsum ) <> 1 then
	destroy lds_option  ; destroy lds_rptsum
	ithw_exception.text += "~nพบข้อผิดพลาดในการสร้างชุด SQL รายงานสรุปเงินคืน(1)"
	return -1
end if

if inv_mrprocsrv.of_setsqldw( lds_rptsum , "mbmoneyreturn" ) <> 1 then
	destroy lds_option  ; destroy lds_rptsum
	ithw_exception.text += "~nพบข้อผิดพลาดในการสร้างชุด SQL รายงานสรุปเงินคืน(2)"
	return -1
end if

if lds_rptsum.retrieve( ldtm_operate ) < 1 then
	destroy lds_option  ; destroy lds_rptsum
	ithw_exception.text += "~nไม่พบข้อมูลต้นคืนดอกเบี้ยคืน"
	throw ithw_exception	
end if

//lds_rptsum.object.t_coopname

astr_mr_xml.xml_report_summary = inv_dwxmliesrv.of_xmlexport( lds_rptsum )

this.of_setsrvdwxmlie(false)
this.of_setsrvmrproc(false)

destroy lds_option  ; destroy lds_rptsum

return 1
end function

public function integer of_save_mr_opr_proc (ref str_money_return_xml astr_mr_xml) throws Exception;/***********************************************************
<description>
	บันทึกข้อมูลต้นคืนดอกเบี้ยคืน
</description>

<arguments>  
		str_keep.xml_main			String		dwxml ข้อมูลการต้นคืนดอกเบี้ยคืนตามเงื่อนไขที่ทำรายการ
</arguments> 

<return> 
	Integer	1 = success,  Exception = failure
</return> 

<usage> 	
	Revision History:
	Version 1.0 (Initial version) Modified Date 13/06/2011 by Godji
	Version 1.1 (Initial version) Modified Date 13/06/2011 by Godji
</usage> 
***********************************************************/
string ls_coopid , ls_proctext
string ls_memno
integer li_pos
integer li_mrcreate , li_mrpost , li_max
integer li_chk , li_proctype
long ll_row , ll_rowcount , ll_nwrow
dec{2} ldc_money
dec{2} ldc_prinret , ldc_intret , ldc_itemret
datetime ldtm_entry , ldtm_operate , ldtm_slip
boolean lb_err = false

n_ds lds_option

lds_option = create n_ds
lds_option.dataobject = "d_mr_opr_proc_option"
lds_option.settransobject( itr_sqlca )

this.of_setsrvmrproc(true)
this.of_setsrvproc(true)
this.of_setsrvdwxmlie(true)
this.of_setsrvdoccontrol( true )

li_chk = inv_dwxmliesrv.of_xmlimport( lds_option , astr_mr_xml.xml_option )
if li_chk < 1 then 
	ithw_exception.text += "~nไม่พบข้อมูลเงื่อนไขการดึงข้อมูลเงินคืน"
	lb_err = true ; if lb_err then Goto objdestroy
end if

ls_coopid					= lds_option.object.coop_id[1]
ls_proctext				= lds_option.object.proc_text[1]		; if isnull( ls_proctext ) then ls_proctext = ""
li_proctype				= lds_option.object.proc_type[1]
li_mrcreate				= lds_option.object.mrcreate_status[1]
li_mrpost					= lds_option.object.mrpost_status[1]
ldtm_operate			= lds_option.object.operate_date[1]

try
	inv_mrprocsrv.of_setproctype( li_proctype )
	inv_mrprocsrv.of_set_coopid( ls_coopid )
	inv_mrprocsrv.of_set_proctext( ls_proctext )
	inv_mrprocsrv.of_setanalyze()
	
	inv_mrprocsrv.of_setsqlselect( "mbmembmaster" )

	inv_procsrv.of_set_proctype( li_proctype ) // กำหนดวิธีดึงข้อมูล 60 ดึงข้อมูลตามทะเบียนสมาชิก
	inv_procsrv.of_set_txtproc( ls_proctext ) // ใส่ค่าที่ดึงข้อมูล
	inv_procsrv.of_set_analyze() // gen ข้อมูลในการดึง
	inv_procsrv.of_set_sqlselect( "mbmembmaster") // set ค่าที่ gen ลงในตารางที่เลือก
catch( Exception lthw_setproc )
	ithw_exception.text	+= lthw_setproc.text
	lb_err = true
end try
if lb_err then Goto objdestroy

li_max	= li_mrcreate + li_mrpost

inv_progress.istr_progress.status = 8
inv_progress.istr_progress.progress_text = "ประมวลจ่ายเงินคืนสมาชิก"
inv_progress.istr_progress.progress_max = li_max
inv_progress.istr_progress.progress_index = 0
inv_progress.istr_progress.subprogress_text = "กำลังทำรายการ กรุณารอสักครู่"
inv_progress.istr_progress.subprogress_max = 1
inv_progress.istr_progress.subprogress_index	= 0

try
	if li_mrcreate = 1 then
		// ผ่านรายการคืนต้นกับดอกเบี้ย ระบบเงินกู้
		inv_progress.istr_progress.progress_index	= li_mrcreate
		this.of_post_mr_opr_proc_crt(lds_option)
	end if
	if li_mrpost = 1 then
		inv_progress.istr_progress.progress_index	= li_mrcreate + li_mrpost
		this.of_post_mr_opr_proc_lon(lds_option)
	end if
catch( Exception lthw_mrproc )
	ithw_exception.text	+= lthw_mrproc.text
	throw ithw_exception
end try

objdestroy:
this.of_setsrvdoccontrol(false)
this.of_setsrvdwxmlie(false)
this.of_setsrvproc(false)
this.of_setsrvmrproc(false)

if isvalid(lds_option) then destroy lds_option

if lb_err then
	ithw_exception.text = "n_cst_money_return_opr.of_save_mr_opr_proc()" + ithw_exception.text
	rollback using itr_sqlca ;
	throw ithw_exception
else
	commit using itr_sqlca ;
end if

inv_progress.istr_progress.status = 1

return 1
end function

protected function integer of_post_lncont_stm (str_money_return_lncontstm astr_lnstatement) throws Exception;string		ls_contno, ls_contcoopid, ls_itemcode, ls_refdocno, ls_refslipno, ls_statusdesc
string		ls_moneytype, ls_entryid, ls_entrycoopid, ls_remark
integer	li_seqno, li_period, li_itemstatus, li_lastseqno
dec{2}	ldc_prnpay, ldc_intpay, ldc_prnbal, ldc_prncalint, ldc_intperiod
dec{2}	ldc_bfintarr, ldc_bfintret, ldc_intarrbal, ldc_intretbal
datetime	ldtm_slipdate, ldtm_opedate, ldtm_accdate, ldtm_intaccdate, ldtm_entrydate
datetime	ldtm_calintfrom, ldtm_calintto

ls_contno		= astr_lnstatement.loancontract_no
ls_contcoopid	= astr_lnstatement.contcoop_id
ldtm_slipdate	= astr_lnstatement.slip_date
ldtm_opedate	= astr_lnstatement.operate_date
ldtm_accdate	= astr_lnstatement.account_date
ldtm_intaccdate= astr_lnstatement.intaccum_date
ls_refslipno		= astr_lnstatement.ref_slipno
ls_refdocno		= astr_lnstatement.ref_docno
ls_itemcode		= astr_lnstatement.loanitemtype_code
li_period			= astr_lnstatement.period
ldc_prnpay		= astr_lnstatement.principal_payment
ldc_intpay		= astr_lnstatement.interest_payment
ldc_prnbal		= astr_lnstatement.principal_balance
ldc_prncalint	= astr_lnstatement.prncalint_amt
ldtm_calintfrom	= astr_lnstatement.calint_from
ldtm_calintto	= astr_lnstatement.calint_to
ldc_bfintarr		= astr_lnstatement.bfinterest_arrear
ldc_bfintret		= astr_lnstatement.bfinterest_return
ldc_intperiod	= astr_lnstatement.interest_period
ldc_intarrbal		= astr_lnstatement.interest_arrear
ldc_intretbal		= astr_lnstatement.interest_return
ls_moneytype	= astr_lnstatement.moneytype_code
li_itemstatus	= astr_lnstatement.item_status
ls_entryid		= astr_lnstatement.entry_id
ls_entrycoopid	= astr_lnstatement.entry_bycoopid
ls_remark		= astr_lnstatement.remark

ldtm_entrydate	= datetime( today(), now() )

select		last_stm_no, status_desc
into		:li_lastseqno, :ls_statusdesc
from		lncontmaster
where	( loancontract_no	= :ls_contno ) and
			( coop_id				= :ls_contcoopid ) 
using		itr_sqlca ;

if itr_sqlca.sqlcode <> 0 then
	ithw_exception.text	+= "of_poststm_contract ไม่พบเลขที่สัญญาที่ระบุ "+ls_contno+" กรุณาตรวจสอบ ~r~n"+itr_sqlca.sqlerrtext
	throw ithw_exception
end if

if isnull( li_lastseqno ) then li_lastseqno = 0

li_lastseqno ++

// เพิ่มรายการเคลื่อนไหวการชำระหนี้
insert into lncontstatement
			( loancontract_no,		coop_id,					seq_no,					slip_date,				operate_date,			account_date,			intaccum_date,
			  ref_slipno,				ref_docno,				loanitemtype_code,	
			  period,		 			principal_payment,	interest_payment,		principal_balance,		
			  prncalint_amt,		calint_from,				calint_to,					bfintarrear_amt,		bfintreturn_amt,		interest_period,
			  interest_arrear,		interest_return,			moneytype_code,		item_status,				entry_id,					entry_date,				entry_bycoopid,
			  remark,					bfcontstatus_desc )
values	( :ls_contno,			:ls_contcoopid,			:li_lastseqno,			:ldtm_slipdate,			:ldtm_opedate,			:ldtm_accdate,			:ldtm_intaccdate,
			  :ls_refslipno,			:ls_refdocno,			:ls_itemcode,			
			  :li_period,				:ldc_prnpay,				:ldc_intpay,				:ldc_prnbal,
			  :ldc_prncalint,			:ldtm_calintfrom,		:ldtm_calintto,			:ldc_bfintarr,			:ldc_bfintret,			:ldc_intperiod,
			  :ldc_intarrbal,			:ldc_intretbal,			:ls_moneytype,			:li_itemstatus,			:ls_entryid,				:ldtm_entrydate,		:ls_entrycoopid,
			  :ls_remark,			:ls_statusdesc )
using		itr_sqlca ;

if itr_sqlca.sqlcode <> 0 then
	ithw_exception.text	+= "of_poststm_contract  ไม่สามารถเพิ่มรายการ Statement หนี้ "+ls_contno+" ได้ กรุณาตรวจสอบ ~r~n"+itr_sqlca.sqlerrtext
	throw ithw_exception
end if

update	lncontmaster
set			last_stm_no		= :li_lastseqno
where	( loancontract_no	= :ls_contno )
and		( coop_id 			= :ls_contcoopid ) 
using		itr_sqlca ;

if itr_sqlca.sqlcode <> 0 then
	ithw_exception.text	+= "of_poststm_contract  ไม่สามารถปรับปรุงลำดับที่ล่าสุดได้ เลขที่สัญญา "+ls_contno+" กรุณาตรวจสอบ ~r~n"+itr_sqlca.sqlerrtext
	throw ithw_exception
end if

// ตรวจสอบว่าถ้าเป็นรายการยกเลิกให้ไปปรับรายการคู่กันให้ยกเลิกด้วย
if li_itemstatus <> 1 then
	update	lncontstatement
	set			item_status	= :li_itemstatus
	where	( loancontract_no	= :ls_contno ) and
				( ref_slipno	= :ls_refslipno )		and
				(coop_id		= :ls_contcoopid )
	using		itr_sqlca ;
	
	if itr_sqlca.sqlcode <> 0 then
		ithw_exception.text	+= "of_poststm_contract  ไม่สามารถปรับปรุงสถานะ Statement รายการคู่กันได้ เลขที่สัญญา "+ls_contno+" เลขที่ Slip "+ls_refslipno+" กรุณาตรวจสอบ ~r~n"+itr_sqlca.sqlerrtext
		throw ithw_exception
	end if
end if

return 1
end function

protected function integer of_post_mr_opr_proc_crt (n_ds ads_option) throws Exception;string ls_sqlselect , ls_sql
string ls_coop , ls_recv
string ls_entry
string ls_mrcrtsdtm , ls_mrcrtedtm
datetime ldtm_mrcrts , ldtm_mrcrte
boolean lb_err

//by mikekong 
//inv_progress.istr_progress.subprogress_text = "ลบข้อมูลสร้างทะเบียนคืน รอสักครู่..."
inv_progress.istr_progress.subprogress_index = 0
inv_progress.istr_progress.subprogress_max = 3

ls_coop			= ads_option.object.coop_id[1]
ls_recv			= ads_option.object.recv_period[1]
ls_entry			= ads_option.object.entry_id[1]
ldtm_mrcrts		= ads_option.object.mrcreate_sdate[1]
ldtm_mrcrte		= ads_option.object.mrcreate_edate[1]

ls_mrcrtsdtm 	= string( ldtm_mrcrts , 'dd/mm/yyyy' )
ls_mrcrtedtm	= string( ldtm_mrcrte , 'dd/mm/yyyy' )

try
	inv_procsrv.of_set_sqlselect( "mbmembmaster")
	inv_procsrv.of_get_sqlselect( ls_sqlselect )
catch( Exception lthw_sqlselect )
	ithw_exception.text	+= lthw_sqlselect.text
	ithw_exception.text 	+= "~r~nกรุณาตรวจสอบ"
	lb_err = true 
end try
if lb_err then Goto objdestroy

// not use edited by mikekong
// ลบข้อมูล
//ls_sql		= " delete from mbmoneyreturn "
//ls_sql		+= " where mbmoneyreturn.return_status = 8 "
//ls_sql		+= " and exists ( select 1 from mbmembmaster where mbmembmaster.coop_id = mbmoneyreturn.coop_id and mbmembmaster.member_no = mbmoneyreturn.member_no "
//ls_sql		+= " and mbmembmaster.coop_id = '"+ls_coop+"' "
//ls_sql		+= ls_sqlselect
//ls_sql		+= " ) "
//execute immediate :ls_sql using itr_sqlca;

//if itr_sqlca.sqlcode <> 0 then
//	ithw_exception.text 	= "พบข้อผิดพลาด of_post_mr_opr_proc_crt (70.1) "
//	ithw_exception.text 	+= "~r~nไม่สามารถเคลียร์ต้นคืน ได้"
//	ithw_exception.text 	+= "~r~n"+ string( itr_sqlca.sqlcode ) + "  " + itr_sqlca.sqlerrtext
//	ithw_exception.text 	+= "~r~nกรุณาตรวจสอบ"
//	lb_err = true ; if lb_err then Goto objdestroy
//end if

// gen ต้นเงิน
ls_sql		= " insert into mbmoneyreturn "
ls_sql		+= " ( coop_id , member_no , "
ls_sql		+= " seq_no , "
ls_sql		+= " system_from , description , shrlontype_code , bizzcoop_id , loancontract_no , returnitemtype_code , return_amount , return_status , entry_id ) "
ls_sql		+= " select coop_id , member_no , "
ls_sql		+= " nvl( ( select max( mr.seq_no ) from mbmoneyreturn mr where mr.coop_id = lncontmaster.memcoop_id and mr.member_no = mr.member_no ) , 0 ) + rank() over ( partition by member_no order by loancontract_no ) as f_row , "
ls_sql		+= " 'LON' , 'PRIN' , loantype_code , coop_id , loancontract_no , 'PRN' , abs( principal_balance ) , 8 , '" + ls_entry + "' "
ls_sql		+= " from lncontmaster where principal_balance < 0 "
ls_sql		+= " and exists ( select 1 from mbmembmaster where mbmembmaster.coop_id = lncontmaster.memcoop_id and mbmembmaster.member_no = lncontmaster.member_no "
ls_sql		+= " and mbmembmaster.coop_id = '"+ls_coop+"' "
ls_sql		+= ls_sqlselect
ls_sql		+= " ) "
ls_sql		+="and exists ( select 1 from kpmastreceivedet kd where kd.coop_id = '"+ls_coop+"' and kd.recv_period = '"+ls_recv+"'  and kd.keepitem_status = 1 and kd.money_return_status = 8 and kd.loancontract_no = lncontmaster.loancontract_no and kd.coop_id=lncontmaster.coop_id) "
execute immediate :ls_sql using itr_sqlca;

if itr_sqlca.sqlcode <> 0 then
	ithw_exception.text 	= "พบข้อผิดพลาด of_post_mr_opr_proc_crt (70.2) "
	ithw_exception.text 	+= "~r~nไม่สามารถทำรายการต้นคืน ได้"
	ithw_exception.text 	+= "~r~n"+ string( itr_sqlca.sqlcode ) + "  " + itr_sqlca.sqlerrtext
	ithw_exception.text 	+= "~r~nกรุณาตรวจสอบ"
	lb_err = true ; if lb_err then Goto objdestroy
end if


////gen ดอกเบี้ย จาก slip
//ls_sql		= " insert into mbmoneyreturn "
//ls_sql		+= " ( coop_id , member_no , seq_no , system_from , description , shrlontype_code , bizzcoop_id , loancontract_no , returnitemtype_code , return_amount , return_status , entry_id ) "
// ls_sql		+= " select tmp.coop_id , tmp.member_no , "
//ls_sql		+= " nvl( ( select max( mr.seq_no ) from mbmoneyreturn mr where mr.coop_id = tmp.coop_id and mr.member_no = tmp.member_no ) , 0 ) + ( rank() over ( partition by member_no order by loancontract_no ) ) as s_no,"
//ls_sql		+= "  'LON' , 'GEN INT' , tmp.shrlontype_code , tmp.coop_id , tmp.loancontract_no , 'INT' , tmp.intret , 8 , '"+ls_entry+"'  from"
//ls_sql		+= " ("
//ls_sql		+= "  select si.coop_id,si.member_no,sd.shrlontype_code  , sd.loancontract_no , sum( sd.interest_return )as intret"
//ls_sql		+= " 	 from slslippayin si , slslippayindet sd , lnloantype lt "
//ls_sql		+= " 	 where si.slip_date between to_date( '"+ls_mrcrtsdtm+"' , 'dd/mm/yyyy' ) and to_date( '"+ls_mrcrtedtm+"' , 'dd/mm/yyyy' ) "
//ls_sql		+= " 	 and si.slip_status = 1 and si.coop_id = sd.coop_id "
//ls_sql		+= " 	 and si.payinslip_no = sd.payinslip_no "
//ls_sql		+= " 	 and sd.slipitemtype_code = 'LON' "
//ls_sql		+= " 	 and sd.interest_return > 0 "
//ls_sql		+= " 	 and sd.shrlontype_code = lt.loantype_code "
//ls_sql		+= " 	 and exists ( select 1 from kpmastreceivedet kd where kd.coop_id = '"+ls_coop+"' and kd.recv_period = '"+ls_recv+"' and kd.keepitem_status = 1 and kd.money_return_status = 8 and kd.loancontract_no = sd.loancontract_no and kd.coop_id = sd.coop_id) "
//ls_sql		+= " 	group by si.coop_id,si.member_no,sd.shrlontype_code  , sd.loancontract_no"
//ls_sql		+= " ) tmp"
//execute immediate :ls_sql using itr_sqlca;

// gen ดอกเบี้ย จาก Interset_return table  lncontmaster
ls_sql		= " insert into mbmoneyreturn "
ls_sql		+= " ( coop_id , member_no , "
ls_sql		+= " seq_no , "
ls_sql		+= " system_from , description , shrlontype_code , bizzcoop_id , loancontract_no , returnitemtype_code , return_amount , return_status , entry_id ) "
ls_sql		+= " select coop_id , member_no , "
ls_sql		+= " nvl( ( select max( mr.seq_no ) from mbmoneyreturn mr where mr.coop_id = lncontmaster.memcoop_id and mr.member_no = mr.member_no ) , 0 ) + rank() over ( partition by member_no order by loancontract_no ) as f_row , "
ls_sql		+= " 'LON' , 'interest' , loantype_code , coop_id , loancontract_no , 'INT' , abs( interest_return ) , 8 , '" + ls_entry + "' "
ls_sql		+= " from lncontmaster where interest_return > 0 "
ls_sql		+= " and exists ( select 1 from mbmembmaster where mbmembmaster.coop_id = lncontmaster.memcoop_id and mbmembmaster.member_no = lncontmaster.member_no "
ls_sql		+= " and mbmembmaster.coop_id = '"+ls_coop+"' "
ls_sql		+= ls_sqlselect
ls_sql		+= " ) "
ls_sql		+="and exists ( select 1 from kpmastreceivedet kd where kd.coop_id = '"+ls_coop+"' and kd.recv_period = '"+ls_recv+"'  and kd.keepitem_status = 1 and kd.money_return_status = 8 and kd.loancontract_no = lncontmaster.loancontract_no and kd.coop_id=lncontmaster.coop_id) "
execute immediate :ls_sql using itr_sqlca;

if itr_sqlca.sqlcode <> 0 then
	ithw_exception.text 	= "พบข้อผิดพลาด of_post_mr_opr_proc_crt (70.3) "
	ithw_exception.text 	+= "~r~nไม่สามารถทำรายการดอกเบี้ยคืน ได้"
	ithw_exception.text 	+= "~r~n"+ string( itr_sqlca.sqlcode ) + "  " + itr_sqlca.sqlerrtext
	ithw_exception.text 	+= "~r~nกรุณาตรวจสอบ"
	lb_err = true ; if lb_err then Goto objdestroy
end if

//ปรับค่ากลับสถานะที่ kpmastreceivedet
ls_sql		= " update kpmastreceivedet "
ls_sql		+= " set money_return_status = 1 "
ls_sql		+= " where coop_id = '"+ls_coop+"' "
ls_sql		+= " and recv_period = '"+ls_recv+"' "
ls_sql		+= " and exists "
ls_sql		+= " ( select 1 "
ls_sql		+= " from mbmoneyreturn "
ls_sql		+= " where mbmoneyreturn.bizzcoop_id = kpmastreceivedet.bizzcoop_id "
ls_sql		+= " and mbmoneyreturn.loancontract_no = kpmastreceivedet.loancontract_no "
ls_sql		+= " and mbmoneyreturn.return_status = 8 "
ls_sql		+= " ) "
execute immediate :ls_sql using itr_sqlca;

if itr_sqlca.sqlcode <> 0 then
	ithw_exception.text 	= "พบข้อผิดพลาด of_post_mr_opr_proc_crt (70.3) "
	ithw_exception.text 	+= "~r~nไม่สามารถทำรายการดอกเบี้ยคืน ได้"
	ithw_exception.text 	+= "~r~n"+ string( itr_sqlca.sqlcode ) + "  " + itr_sqlca.sqlerrtext
	ithw_exception.text 	+= "~r~nกรุณาตรวจสอบ"
	lb_err = true ; if lb_err then Goto objdestroy
end if

// not use edited by mikekong
////หุ้นรอจ่ายคืน
//ls_sql		= " insert into mbmoneyreturn "
//ls_sql		+= " ( coop_id , member_no , "
//ls_sql		+= " seq_no , "
//ls_sql		+= " system_from , description , shrlontype_code , bizzcoop_id , loancontract_no , returnitemtype_code , return_amount , return_status , entry_id ) "
//ls_sql		+= " select coop_id , member_no , "
//ls_sql		+= " nvl( ( select max( mr.seq_no ) from mbmoneyreturn mr where mr.coop_id = shsharemaster.coop_id and mr.member_no = shsharemaster.member_no ) , 0 ) + rank() over ( partition by member_no order by member_no ) as f_row , "
//ls_sql		+= " 'LON' , 'SHR' , sharetype_code , coop_id , member_no , 'SHR' , sharestk_amt , 8 , '" + ls_entry + "' "
//ls_sql		+= " from shsharemaster where sharestk_amt > 0 and sharemaster_status = -1 "
//ls_sql		+= " and exists ( select 1 from mbmembmaster where mbmembmaster.coop_id = shsharemaster.coop_id and mbmembmaster.member_no = shsharemaster.member_no "
//ls_sql		+= " and mbmembmaster.coop_id = '"+ls_coop+"' "
//ls_sql		+= ls_sqlselect
//ls_sql		+= " ) "
//execute immediate :ls_sql using itr_sqlca;
//
//if itr_sqlca.sqlcode <> 0 then
//	ithw_exception.text 	= "พบข้อผิดพลาด of_post_mr_opr_proc_crt (70.4) "
//	ithw_exception.text 	+= "~r~nไม่สามารถทำรายการหุ้นรอจ่ายคืน ได้"
//	ithw_exception.text 	+= "~r~n"+ string( itr_sqlca.sqlcode ) + "  " + itr_sqlca.sqlerrtext
//	ithw_exception.text 	+= "~r~nกรุณาตรวจสอบ"
//	lb_err = true ; if lb_err then Goto objdestroy
//end if

//ปรับค่ากลับสถานะที่ kpmastreceivedet
ls_sql		= " update kpmastreceivedet "
ls_sql		+= " set money_return_status = 1 "
ls_sql		+= " where coop_id = '"+ls_coop+"' "
ls_sql		+= " and recv_period = '"+ls_recv+"' "
ls_sql		+= " and keepitemtype_code = 'S01' "
ls_sql		+= " and exists "
ls_sql		+= " ( select 1 "
ls_sql		+= " from mbmoneyreturn "
ls_sql		+= " where mbmoneyreturn.coop_id = kpmastreceivedet.memcoop_id "
ls_sql		+= " and mbmoneyreturn.member_no = kpmastreceivedet.member_no "
ls_sql		+= " and mbmoneyreturn.return_status = 8 "
ls_sql		+= " ) "
execute immediate :ls_sql using itr_sqlca;

if itr_sqlca.sqlcode <> 0 then
	ithw_exception.text 	= "พบข้อผิดพลาด of_post_mr_opr_proc_crt (70.3) "
	ithw_exception.text 	+= "~r~nไม่สามารถทำรายการดอกเบี้ยคืน ได้"
	ithw_exception.text 	+= "~r~n"+ string( itr_sqlca.sqlcode ) + "  " + itr_sqlca.sqlerrtext
	ithw_exception.text 	+= "~r~nกรุณาตรวจสอบ"
	lb_err = true ; if lb_err then Goto objdestroy
end if

objdestroy:

if lb_err then
	ithw_exception.text = "~r~nn_cst_divsrv_process.of_post_mr_opr_proc_crt()" + ithw_exception.text
	throw ithw_exception
end if

return 1
end function

protected function integer of_post_mr_opr_proc_lon (n_ds ads_option) throws Exception;string ls_coopid , ls_bizzcoop , ls_memcoop
string ls_loantype , ls_contno , ls_memno
string ls_slippayout , ls_document , ls_membgrp
string ls_moneytype , ls_bank , ls_branch , ls_accid
string ls_entryid
integer li_nwrow , li_nwrowdet
integer li_mrconttodp , li_mrpayintarr
long ll_index , ll_count
dec{2} ldc_prnamt , ldc_intamt , ldc_itemamt
datetime ldtm_operate , ldtm_slip , ldtm_entry
boolean lb_err = false
Exception lthw_mrprclon

n_ds lds_moneyret , lds_slippayout , lds_slippayoutdet
n_ds lds_slippayin , lds_slippayindet , lds_constant

lds_constant = create n_ds
lds_constant.dataobject = "d_kp_constant"
lds_constant.settransobject( itr_sqlca )

lds_moneyret = create n_ds
lds_moneyret.dataobject = "d_mr_opr_proc_info_ret_lon"
lds_moneyret.settransobject( itr_sqlca )

lds_slippayout = create n_ds
lds_slippayout.dataobject = "d_mr_info_slslippayout"
lds_slippayout.settransobject( itr_sqlca )

lds_slippayoutdet = create n_ds
lds_slippayoutdet.dataobject = "d_mr_info_slslippayoutdet"
lds_slippayoutdet.settransobject( itr_sqlca )

lds_slippayin = create n_ds
lds_slippayin.dataobject = "d_mr_info_slslippayin"
lds_slippayin.settransobject( itr_sqlca )

lds_slippayindet = create n_ds
lds_slippayindet.dataobject = "d_mr_info_slslippayindet"
lds_slippayindet.settransobject( itr_sqlca )

if inv_mrprocsrv.of_setsqldw( lds_moneyret ) <> 1 then
	lthw_mrprclon.text = "พบข้อผิดพลาดในการสร้างชุด SQL รายงานสรุปเงินคืน(1)"
	lb_err = true ; if lb_err then Goto objdestroy
end if

if inv_mrprocsrv.of_setsqldw( lds_moneyret , "mbmoneyreturn" ) <> 1 then
	lthw_mrprclon.text = "พบข้อผิดพลาดในการสร้างชุด SQL รายงานสรุปเงินคืน(2)"
	lb_err = true ; if lb_err then Goto objdestroy
end if

ll_count = lds_moneyret.retrieve()

if lds_moneyret.retrieve(  ) < 1 then
	// ไม่เจอแสดงว่าไม่มีเงินคืน
	return 1
end if

inv_progress.istr_progress.subprogress_text = "ดึงข้อมูล ต้นคืนดอกเบี้ยคืนระบบเงินกู้ กรุณารอสักครู่"
inv_progress.istr_progress.subprogress_max = ll_count
inv_progress.istr_progress.subprogress_index	= 0

ls_coopid			= ads_option.object.coop_id[1]
ls_entryid		= ads_option.object.entry_id[1]
ldtm_slip			= ads_option.object.slip_date[1]
ldtm_operate	= ads_option.object.operate_date[1]

ldtm_entry		= datetime( today() , now() )

// ดึงข้อมูลค่าคงที่
if lds_constant.retrieve( inv_transection.is_coopcontrol ) > 0 then
	li_mrconttodp 	= lds_constant.object.mr_conttodp_flag[1]		
	li_mrpayintarr	= lds_constant.object.mr_payintarr_flag[1]
end if
if isnull( li_mrconttodp ) then li_mrconttodp = 0
if isnull( li_mrpayintarr ) then li_mrpayintarr = 0

for ll_index = 1 to ll_count
	
	inv_progress.istr_progress.subprogress_index = ll_index
	
	// ตรวจสอบการสั่งหยุดทำงาน
	yield()
	if inv_progress.of_is_stop() then
		lthw_mrprclon.text = "หยุดการทำงานโดนพนักงาน"
		lb_err = true ; if lb_err then Goto objdestroy
	end if
	
	ls_memcoop	= lds_moneyret.object.coop_id[ll_index]
	ls_memno		= lds_moneyret.object.member_no[ll_index]
	ls_membgrp		= lds_moneyret.object.membgroup_code[ll_index]
	ls_moneytype	= lds_moneyret.object.moneytype_code[ll_index]
	ls_bank			= lds_moneyret.object.bank_code[ll_index]
	ls_branch		= lds_moneyret.object.bank_branch[ll_index]
	ls_accid			= lds_moneyret.object.bank_accid[ll_index]
	ldc_itemamt		= lds_moneyret.object.return_amount[ll_index]		; if isnull( ldc_itemamt ) then ldc_itemamt = 0
	
	ls_slippayout 	= inv_docsrv.of_getnewdocno( ls_coopid , "SLSLIPPAYOUT" )
//	ls_document	= inv_docsrv.of_getnewdocno( ls_coopid , "CMSLIPPAYNO" )
	
	lds_slippayout.reset()
	lds_slippayoutdet.reset()
	
	li_nwrow		= lds_slippayout.insertrow(0)
	
	// ใส่เลขที่ Slip ใน Header
	lds_slippayout.setitem( li_nwrow, "coop_id" , ls_coopid )
	lds_slippayout.setitem( li_nwrow, "payoutslip_no", ls_slippayout )
	lds_slippayout.setitem( li_nwrow, "memcoop_id" , ls_memcoop )
	lds_slippayout.setitem( li_nwrow, "member_no", ls_memno )
	lds_slippayout.setitem( li_nwrow, "document_no", "KPSLIPNO" )
	lds_slippayout.setitem( li_nwrow, "sliptype_code", "LRT" )
	lds_slippayout.setitem( li_nwrow, "slip_date", ldtm_slip )
	lds_slippayout.setitem( li_nwrow, "operate_date", ldtm_operate )
	lds_slippayout.setitem( li_nwrow, "entry_id", ls_entryid )
	lds_slippayout.setitem( li_nwrow, "entry_date", ldtm_entry )
	lds_slippayout.setitem( li_nwrow, "slip_status", 1 )

	lds_slippayout.setitem( li_nwrow, "moneytype_code", ls_moneytype )
	lds_slippayout.setitem( li_nwrow, "expense_bank", ls_bank ) 
	lds_slippayout.setitem( li_nwrow, "expense_branch", ls_branch )
	lds_slippayout.setitem( li_nwrow, "expense_accid", ls_accid )
	lds_slippayout.setitem( li_nwrow, "membgroup_code", ls_membgrp )
	lds_slippayout.setitem( li_nwrow, "bankfee_amt", 0 )
	lds_slippayout.setitem( li_nwrow, "banksrv_amt", 0 )
	lds_slippayout.setitem( li_nwrow, "payout_amt", ldc_itemamt )
	lds_slippayout.setitem( li_nwrow, "payoutnet_amt", ldc_itemamt )
	
	declare data_cur cursor for
	SELECT mr.shrlontype_code,mr.loancontract_no,sum( case when mr.returnitemtype_code = 'PRN' then mr.return_amount else 0 end ) as prnreturn_amount,sum( case when mr.returnitemtype_code = 'INT' then mr.return_amount else 0 end ) as intreturn_amount,mr.bizzcoop_id
	FROM mbmoneyreturn mr,mbmembmaster mm
	WHERE ( mr.coop_id = mm.coop_id ) 
	and ( mr.member_no = mm.member_no ) 
	and ( mr.return_status = 8 )
	and ( mr.returnitemtype_code in ( 'PRN' , 'INT' ) )
	and mr.member_no = :ls_memno
	and mr.coop_id = :ls_coopid
	GROUP BY mr.shrlontype_code,mr.loancontract_no,mr.bizzcoop_id
	using itr_sqlca ;
	
	open data_cur;
		fetch data_cur into :ls_loantype , :ls_contno , :ldc_prnamt , :ldc_intamt , :ls_bizzcoop;
		do while itr_sqlca.sqlcode = 0 ;
			
			if isnull( ldc_prnamt ) then ldc_prnamt = 0
			if isnull( ldc_intamt ) then ldc_intamt = 0
			ldc_itemamt		= ldc_prnamt + ldc_intamt

			li_nwrowdet		= lds_slippayoutdet.insertrow(0)

			lds_slippayoutdet.object.coop_id[li_nwrowdet]					= ls_coopid
			lds_slippayoutdet.object.payoutslip_no[li_nwrowdet]			= ls_slippayout
			lds_slippayoutdet.object.slipitemtype_code[li_nwrowdet] 	= "MRL"
			lds_slippayoutdet.object.seq_no[li_nwrowdet] 					= li_nwrowdet
			lds_slippayoutdet.object.shrlontype_code[li_nwrowdet] 		= ls_loantype
			lds_slippayoutdet.object.concoop_id[li_nwrowdet]				= ls_bizzcoop
			lds_slippayoutdet.object.loancontract_no[li_nwrowdet] 		= ls_contno
			lds_slippayoutdet.object.slipitem_desc[li_nwrowdet] 			= "ต้นคืนดอกเบี้ยคืน"
			lds_slippayoutdet.object.principal_payamt[li_nwrowdet] 		= ldc_prnamt
			lds_slippayoutdet.object.interest_payamt[li_nwrowdet] 		= ldc_intamt
			lds_slippayoutdet.object.item_payamt[li_nwrowdet]			= ldc_itemamt
			
			fetch data_cur into :ls_loantype , :ls_contno , :ldc_prnamt , :ldc_intamt , :ls_bizzcoop;
		loop
	close data_cur;
	
//not use edited by mikekong	
//	declare data_shr cursor for
//	SELECT mr.shrlontype_code,mr.loancontract_no, 0 as prnreturn_amount, 0 as intreturn_amount , sum( mr.return_amount ) as itemreturn_amount ,mr.bizzcoop_id
//	FROM mbmoneyreturn mr,mbmembmaster mm
//	WHERE ( mr.coop_id = mm.coop_id ) 
//	and ( mr.member_no = mm.member_no ) 
//	and ( mr.return_status = 8 )
//	and ( mr.returnitemtype_code in ( 'SHR' ) )
//	and mr.member_no = :ls_memno
//	and mr.coop_id = :ls_coopid
//	GROUP BY mr.shrlontype_code,mr.loancontract_no,mr.bizzcoop_id
//	using itr_sqlca ;
	
//not use edited by mikekong	
//	open data_shr;
//		fetch data_shr into :ls_loantype , :ls_contno , :ldc_prnamt , :ldc_intamt , :ldc_itemamt , :ls_bizzcoop;
//		do while itr_sqlca.sqlcode = 0 ;
//			
//			if isnull( ldc_prnamt ) then ldc_prnamt = 0
//			if isnull( ldc_intamt ) then ldc_intamt = 0
//			if isnull( ldc_itemamt	 ) then ldc_itemamt	 = 0
//
//			li_nwrowdet		= lds_slippayoutdet.insertrow(0)
//
//			lds_slippayoutdet.object.coop_id[li_nwrowdet]					= ls_coopid
//			lds_slippayoutdet.object.payoutslip_no[li_nwrowdet]			= ls_slippayout
//			lds_slippayoutdet.object.slipitemtype_code[li_nwrowdet] 	= "MRS"
//			lds_slippayoutdet.object.seq_no[li_nwrowdet] 					= li_nwrowdet
//			lds_slippayoutdet.object.shrlontype_code[li_nwrowdet] 		= ls_loantype
//			lds_slippayoutdet.object.concoop_id[li_nwrowdet]				= ls_bizzcoop
//			lds_slippayoutdet.object.loancontract_no[li_nwrowdet] 		= ls_contno
//			lds_slippayoutdet.object.slipitem_desc[li_nwrowdet] 			= "หุ้นรอคืน"
//			lds_slippayoutdet.object.principal_payamt[li_nwrowdet] 		= ldc_prnamt
//			lds_slippayoutdet.object.interest_payamt[li_nwrowdet] 		= ldc_intamt
//			lds_slippayoutdet.object.item_payamt[li_nwrowdet]			= ldc_itemamt
//			
//			fetch data_shr into :ls_loantype , :ls_contno , :ldc_prnamt , :ldc_intamt , :ldc_itemamt , :ls_bizzcoop;
//		loop
//	close data_shr;
	
//not use edited by mikekong
//	try
//		// นำเงินจ่ายคืนชำระดอกเบี้ยค้างจ่าย
//		if li_mrpayintarr = 1 then 
//			this.of_post_mr_payintarr( lds_slippayout , lds_slippayoutdet , lds_slippayin , lds_slippayindet )
//		end if
//	catch( Exception lthw_mrpayin )
//		lthw_mrprclon.text		+= lthw_mrpayin.text
//		lb_err = true
//	end try	
//	if lb_err then Goto objdestroy	
	
	// บันทึก Slip Pay Out
	if lds_slippayout.update() <> 1 then
		lthw_mrprclon.text	= "บันทึกข้อมูล Slip Pay Out ไม่ได้"
		lthw_mrprclon.text	+= lds_slippayout.of_geterrormessage()
		lb_err = true ; if lb_err then Goto objdestroy
	end if
	
	// บันทึก Slip Detail Pay Out Det
	if lds_slippayoutdet.update() <> 1 then
		lthw_mrprclon.text	= "บันทึกข้อมูล Slip Pay Out Det ไม่ได้"
		lthw_mrprclon.text	+= lds_slippayoutdet.of_geterrormessage()
		lb_err = true ; if lb_err then Goto objdestroy
	end if
	
	try
		this.of_post_slslippayout( lds_slippayout , lds_slippayoutdet )
		//if li_mrpayintarr = 1 then this.of_post_slslippayin( lds_slippayin , lds_slippayindet )
	catch( Exception lthw_setproc )
		lthw_mrprclon.text		+= lthw_setproc.text
		lb_err = true
	end try	
	if lb_err then Goto objdestroy	
		
	update	mbmoneyreturn
	set			return_status			= 1,
				return_slippayoutno 	= :ls_slippayout ,
				return_id 				= :ls_entryid ,
				return_date 				= :ldtm_entry
	where	return_status			= 8
	and		coop_id					= :ls_coopid
	and		member_no				= :ls_memno
	and		returnitemtype_code	in ( 'PRN' , 'INT' , 'SHR' )
	using itr_sqlca ;
	if itr_sqlca.sqlcode <> 0 then
		lthw_mrprclon.text = "ไม่สามารถอัพเดทเงินคืนทะเบียนสมาชิก : "+ls_memno
		lthw_mrprclon.text += "~r~n" + string(itr_sqlca.sqlcode)
		lthw_mrprclon.text += "~r~n" + itr_sqlca.sqlerrtext
		lthw_mrprclon.text += "~r~nกรุณาตรวจสอบ "
		lb_err = true ; if lb_err then Goto objdestroy
	end if
	
	inv_progress.istr_progress.subprogress_text = string(ll_index,'#,##0') + '/' + string(ll_count,'#,##0') +". ทะเบียน "+ls_memno+" > ต้นคืนดอกเบี้ยคืน " + string( ldc_itemamt,"#,##0.00")
	
next

objdestroy:
if isvalid(lds_moneyret) then destroy lds_moneyret
if isvalid(lds_slippayout) then destroy lds_slippayout
if isvalid(lds_slippayoutdet) then destroy lds_slippayoutdet
if isvalid(lds_slippayin) then destroy lds_slippayin
if isvalid(lds_slippayindet) then destroy lds_slippayindet

if lb_err then
	lthw_mrprclon.text = "n_cst_money_return_opr.of_post_mr_opr_proc_lon()" + lthw_mrprclon.text
	throw lthw_mrprclon
end if

return 1
end function

protected function integer of_post_mr_payintarr (ref n_ds ads_slippayout, ref n_ds ads_slippayoutdet, ref n_ds ads_slippayin, ref n_ds ads_slippayindet) throws Exception;string	ls_coopid , ls_payinslip , ls_document
string ls_contcoop , ls_contno , ls_loantype
string ls_memcoop , ls_memno
integer li_row , li_subrow , li_chkcnt
integer li_lastperiod , li_contract
dec{2} ldc_prinbal , ldc_periodpay , ldc_intarr , ldc_nkpint , ldc_rkpprin , ldc_rkpint
dec{2} ldc_tppayoutnet , ldc_payoutnet , ldc_payintarr , ldc_slipclr
datetime ldtm_lastcal , ldtm_lastproc
boolean lb_err
Exception lthw_mrpayint

ads_slippayin.reset()
ads_slippayindet.reset()

li_row		= ads_slippayin.insertrow(0)

ls_coopid			= ads_slippayout.object.coop_id[1]
ls_memcoop 	= ads_slippayout.object.memcoop_id[1]
ls_memno		= ads_slippayout.object.member_no[1]
ldc_payoutnet	= ads_slippayout.object.payoutnet_amt[1]

select count( loancontract_no )
into :li_chkcnt
from lncontmaster
where interest_arrear > 0
and memcoop_id = :ls_memcoop
and member_no = :ls_memno
using itr_sqlca ;

// ไม่มีค้าง
if li_chkcnt < 1 then return 1

ldc_tppayoutnet	= ldc_payoutnet

ls_payinslip	 	= inv_docsrv.of_getnewdocno( ls_coopid , "SLSLIPPAYIN" )
ls_document	= inv_docsrv.of_getnewdocno( ls_coopid , "SLRECEIPTNO" )

ads_slippayin.object.coop_id[li_row]					= ls_coopid
ads_slippayin.object.payinslip_no[li_row]			= ls_payinslip
ads_slippayin.object.memcoop_id[li_row]			= ls_memcoop
ads_slippayin.object.member_no[li_row]				= ls_memno
ads_slippayin.object.document_no[li_row]			= ls_document
ads_slippayin.object.sliptype_code[li_row]			= "PX"
ads_slippayin.object.slip_date[li_row]					= ads_slippayout.object.slip_date[1]
ads_slippayin.object.operate_date[li_row]			= ads_slippayout.object.operate_date[1]
//ads_slippayin.object.sharestkbf_value[li_row]		= 0
//ads_slippayin.object.sharestk_value[li_row]			= 0
//ads_slippayin.object.intaccum_amt[li_row]			= 0
ads_slippayin.object.moneytype_code[li_row]		= "TRN"
//ads_slippayin.object.slip_amt[li_row]					= 0
ads_slippayin.object.slip_status[li_row]				= 1
//ads_slippayin.object.membgroup_code[li_row]		= ads_slippayout.object.[1]
//ads_slippayin.object.subgroup_code[li_row]			= ads_slippayout.object.[1]
ads_slippayin.object.entry_id[li_row]					= ads_slippayout.object.entry_id[1]
ads_slippayin.object.entry_date[li_row]				= ads_slippayout.object.entry_date[1]

declare data_cur cursor for
select coop_id , loancontract_no , loantype_code , principal_balance , 
period_payment , last_periodpay , lastcalint_date , lastprocess_date, interest_arrear , 
nkeep_interest , rkeep_principal , rkeep_interest , contract_status 
from lncontmaster
where interest_arrear > 0
and memcoop_id = :ls_memcoop
and member_no = :ls_memno
order by interest_arrear desc
using itr_sqlca ;

open data_cur;
	fetch data_cur into :ls_contcoop , :ls_contno , :ls_loantype , :ldc_prinbal , : ldc_periodpay , :li_lastperiod , :ldtm_lastcal , :ldtm_lastproc , :ldc_intarr , :ldc_nkpint , :ldc_rkpprin , :ldc_rkpint , :li_contract ;
	do while itr_sqlca.sqlcode = 0 and ldc_payoutnet > 0;
		
		if ldc_payoutnet - ldc_intarr > 0 then
			ldc_payintarr	= ldc_intarr
			ldc_payoutnet	= ldc_payoutnet - ldc_intarr
		else
			ldc_payintarr	= ldc_payoutnet
			ldc_payoutnet	= 0
		end if
		
		li_subrow 	= ads_slippayindet.insertrow(0)
		
		ads_slippayindet.object.coop_id[li_subrow]					= ls_coopid
		ads_slippayindet.object.payinslip_no[li_subrow]			= ls_payinslip
		ads_slippayindet.object.slipitemtype_code[li_subrow]		= "LON"
		ads_slippayindet.object.seq_no[li_subrow]					= li_subrow
		ads_slippayindet.object.operate_flag[li_subrow]			= 1
		ads_slippayindet.object.shrlontype_code[li_subrow]		= ls_loantype
		ads_slippayindet.object.concoop_id[li_subrow]				= ls_contcoop
		ads_slippayindet.object.loancontract_no[li_subrow]		= ls_contno
		ads_slippayindet.object.slipitem_desc[li_subrow]			= "เงินคืนชำระดอกเบี้ยค้างจ่าย"
		ads_slippayindet.object.periodcount_flag[li_subrow]		= 0
		ads_slippayindet.object.period[li_subrow]					= li_lastperiod
		ads_slippayindet.object.principal_payamt[li_subrow]		= 0
		ads_slippayindet.object.interest_payamt[li_subrow]		= 0
		ads_slippayindet.object.intarrear_payamt[li_subrow]		= ldc_payintarr
		ads_slippayindet.object.item_payamt[li_subrow]			= ldc_payintarr
		ads_slippayindet.object.item_balance[li_subrow]			= ldc_prinbal
		ads_slippayindet.object.prncalint_amt[li_subrow]			= ldc_prinbal
		ads_slippayindet.object.calint_from[li_subrow]				= ldtm_lastcal
		ads_slippayindet.object.calint_to[li_subrow]					= ldtm_lastcal
		ads_slippayindet.object.interest_period[li_subrow]		= 0
		ads_slippayindet.object.interest_return[li_subrow]			= 0
		ads_slippayindet.object.stm_itemtype[li_subrow]			= "LPX"
		ads_slippayindet.object.bfperiod[li_subrow]					= li_lastperiod
		ads_slippayindet.object.bfintarr_amt[li_subrow]			= ldc_intarr
		ads_slippayindet.object.bfintarrset_amt[li_subrow]		= 0
		ads_slippayindet.object.bflastcalint_date[li_subrow]		= ldtm_lastcal
		ads_slippayindet.object.bflastproc_date[li_subrow]		= ldtm_lastproc
		ads_slippayindet.object.bfperiod_payment[li_subrow]		= ldc_periodpay
		ads_slippayindet.object.bfshrcont_balamt[li_subrow]		= ldc_prinbal
		ads_slippayindet.object.bfshrcont_status[li_subrow]		= li_contract
		ads_slippayindet.object.rkeep_principal[li_subrow]		= ldc_rkpprin
		ads_slippayindet.object.rkeep_interest[li_subrow]			= ldc_rkpint
		ads_slippayindet.object.nkeep_interest[li_subrow]			= ldc_nkpint
		ads_slippayindet.object.bfintreturn_flag[li_subrow]		= 0
//		ads_slippayindet.object.bfintreturn_amt[li_subrow]		= 0
		
	fetch data_cur into :ls_contcoop , :ls_contno , :ls_loantype , :ldc_prinbal , : ldc_periodpay , :li_lastperiod , :ldtm_lastcal , :ldtm_lastproc , :ldc_intarr , :ldc_nkpint , :ldc_rkpprin , :ldc_rkpint , :li_contract ;
	loop
close data_cur;

ldc_slipclr	= dec( ads_slippayindet.describe( "evaluate( 'sum( item_payamt for all )', "+string( ads_slippayindet.rowcount() )+" )" ) )

ldc_tppayoutnet	= ldc_tppayoutnet - ldc_slipclr

ads_slippayin.object.slip_amt[li_row]					= ldc_slipclr

ads_slippayout.object.slipclear_no[1]					= ls_payinslip
ads_slippayout.object.payoutclr_amt[1]				= ldc_slipclr
ads_slippayout.object.payoutnet_amt[1]				= ldc_tppayoutnet

// บันทึก Slip Pay In
if ads_slippayin.update() <> 1 then
	lthw_mrpayint.text	= "บันทึกข้อมูล Slip Pay Out ไม่ได้"
	lthw_mrpayint.text	+= ads_slippayin.of_geterrormessage()
	lb_err = true ; if lb_err then Goto objdestroy
end if

// บันทึก Slip Detail Pay In Det
if ads_slippayindet.update() <> 1 then
	lthw_mrpayint.text	= "บันทึกข้อมูล Slip Pay Out Det ไม่ได้"
	lthw_mrpayint.text	+= ads_slippayindet.of_geterrormessage()
	lb_err = true ; if lb_err then Goto objdestroy
end if

objdestroy:

if lb_err then
	lthw_mrpayint.text = "n_cst_money_return_opr.of_post_mr_payintarr()" + lthw_mrpayint.text
	throw lthw_mrpayint
end if

return 1
end function

protected function integer of_post_shshare_stm (str_money_return_shsharestm astr_shstatement) throws Exception;/***********************************************************
<description>
	ผ่านรายการ statement หุ้น
</description>

<arguments>  
	astr_shstatement		str_postshrstm		โครงสร้าง structure statement หุ้น
</arguments> 

<return> 
	ลำดับที่			Integer	ลำดับที่ล่าสุดของ statement หุ้น
</return> 

<usage>

	Revision History:
	Version 1.0 (Initial version) Modified Date 28/9/2010 by MiT
</usage> 
***********************************************************/
integer	li_stmno, li_period, li_itemstatus
string		ls_memno, ls_memcoopid, ls_xmlshare, ls_entryid, ls_entrycoopid
string		ls_docno, ls_sharetype, ls_itemtype, ls_moneytype, ls_refslipno
datetime	ldtm_opedate, ldtm_slipdate, ldtm_entrydate, ldtm_accdate, ldtm_shrtime
decimal	ldc_shrpayamt, ldc_shrstkamt, ldc_bfshrarramt, ldc_shrarramt

ldtm_entrydate		= datetime( today(), now() )

ls_memno 			= astr_shstatement.member_no
ls_memcoopid		= astr_shstatement.memcoop_id
ls_sharetype		= astr_shstatement.sharetype_code
ldtm_slipdate		= astr_shstatement.slip_date
ldtm_opedate		= astr_shstatement.operate_date
ldtm_accdate		= astr_shstatement.account_date
ldtm_shrtime		= astr_shstatement.sharetime_date
ls_docno				= astr_shstatement.document_no
ls_itemtype			= astr_shstatement.stmitem_code
li_period				= astr_shstatement.period
ldc_shrpayamt		= astr_shstatement.sharepay_amt
ldc_shrstkamt		= astr_shstatement.sharebal_amt
ls_entryid			= astr_shstatement.entry_id
ls_entrycoopid		= astr_shstatement.entry_bycoopid
ls_moneytype		= astr_shstatement.moneytype
ldc_bfshrarramt	= astr_shstatement.bfsharearr_amt
ldc_shrarramt		= astr_shstatement.sharearr_amt
li_itemstatus		= astr_shstatement.item_status
ls_refslipno			= astr_shstatement.ref_slipno

select	max( seq_no )
into	:li_stmno
from	shsharestatement
where( member_no		= :ls_memno ) and
		( coop_id				= :ls_memcoopid ) and
		( sharetype_code	= :ls_sharetype )
using	itr_sqlca ;

if isnull( li_stmno ) then li_stmno = 0

li_stmno	++
	
// บันทึกรายการเคลื่อนไหว
insert into shsharestatement
		( member_no,			coop_id,					sharetype_code,		seq_no,					ref_slipno,			ref_docno,
		  slip_date,				operate_date,			account_date,			share_date,
		  shritemtype_code,	period,					share_amount,			sharestk_amt,
		  moneytype_code,	shrarrearbf_amt,		shrarrear_amt,			item_status,
		  entry_id,				entry_date,				entry_bycoopid )
		  
values( :ls_memno,			:ls_memcoopid,		:ls_sharetype,			:li_stmno,				:ls_refslipno,		:ls_docno,
		  :ldtm_slipdate,		:ldtm_opedate,			:ldtm_accdate,			:ldtm_shrtime,
		  :ls_itemtype,			:li_period,				:ldc_shrpayamt,		:ldc_shrstkamt,
		  :ls_moneytype,		:ldc_bfshrarramt,		:ldc_shrarramt,		  	:li_itemstatus,
		  :ls_entryid,			:ldtm_entrydate,		:ls_entrycoopid )
using itr_sqlca;

if ( itr_sqlca.sqlcode <> 0 ) then
	ithw_exception.text += "~nไม่สามารถเพิ่ม statement หุ้นได้ ~n" + string( itr_sqlca.sqlcode ) + "  " + itr_sqlca.sqlerrtext 
	throw ithw_exception
end if

update	shsharemaster
set			last_stm_no = :li_stmno
where ( member_no = :ls_memno ) and 	
		 ( sharetype_code = :ls_sharetype )and
		 (coop_id 			=:ls_memcoopid)
using	itr_sqlca ;

if ( itr_sqlca.sqlcode <> 0 ) then
	ithw_exception.text += "~nไม่สามารถปรับปรุงลำดับที่ล่าสุดหุ้นได้ ~n" + string( itr_sqlca.sqlcode ) + "  " + itr_sqlca.sqlerrtext 	
	throw ithw_exception
end if

// ตรวจสอบว่าเป็นรายการยกเลิกให้ไปปรับรายการคู่กันให้ยกเลิกด้วย
if ( li_itemstatus <> 1 ) then
	update	shsharestatement
	set			item_status = :li_itemstatus
	where ( member_no = :ls_memno ) and 	
			 ( sharetype_code = :ls_sharetype ) and
			 ( ref_slipno = :ls_refslipno )and
		 (coop_id 			=:ls_memcoopid)
	using	itr_sqlca ;
	
	if ( itr_sqlca.sqlcode <> 0 ) then
		ithw_exception.text += "~nไม่สามารถปรับปรุงสถานะ Statement หุ้นได้ ~n" + string( itr_sqlca.sqlcode ) + "  " + itr_sqlca.sqlerrtext 
		throw ithw_exception
	end if	
end if

return 1
end function

protected function integer of_post_slslippayin (n_ds ads_slippayin, n_ds ads_slippayindet) throws Exception;string		ls_slipno, ls_contno, ls_moneytype, ls_lwditemtype
string		ls_entryid,ls_docno,ls_accid,ls_branch,ls_bank , ls_branchoper
string		ls_coopid , ls_contcoopid , ls_memcoopid , ls_memno
long		ll_row, ll_index, ll_count
integer	li_tranyear , li_seqno
dec{2}	ldc_prnretamt, ldc_intretamt, ldc_intretbal, ldc_prnbal , ldc_intarr , ldc_intarrbal
dec{2}	ldc_bfintretbal, ldc_bfprnbal, ldc_bfintarrear , ldc_itemamt
datetime	ldtm_entrydate
datetime	ldtm_slipdate, ldtm_opedate, ldtm_null
datetime ldtm_calintfrom , ldtm_calintto

str_money_return_lncontstm	lstr_lnstm

// ไม่พบข้อมูลไม่ต้องทำ
if ads_slippayin.rowcount() < 1 then return 1

setnull( ldtm_null )

ls_coopid			= ads_slippayin.object.coop_id[1]
ls_memcoopid	= ads_slippayin.object.memcoop_id[1]
ls_memno		= ads_slippayin.object.member_no[1]
ls_slipno			= trim( ads_slippayin.getitemstring( 1, "payinslip_no" ) )
ls_docno			= trim( ads_slippayin.getitemstring( 1, "document_no" ) )
ls_entryid		= ads_slippayin.getitemstring( 1, "entry_id" )
ls_moneytype 	= ads_slippayin.getitemstring( 1, "moneytype_code" )
//ls_bank 			= ads_slippayin.getitemstring( 1, "expense_bank" )
//ls_branch 		= ads_slippayin.getitemstring( 1, "expense_branch" )
//ls_accid 			= ads_slippayin.getitemstring( 1, "expense_accid" )
ldc_itemamt		= ads_slippayin.object.slip_amt[1]
ldtm_slipdate	= ads_slippayin.getitemdatetime( 1, "slip_date" )
ldtm_opedate	= ads_slippayin.getitemdatetime( 1, "operate_date" )
ldtm_entrydate	= datetime( today(), now() )

ll_count		= ads_slippayindet.rowcount()

if ll_count <= 0 then
//	destroy ads_slippayindet
	ithw_exception.text	= "ไม่พบข้อมูลทำรายการ"
	throw ithw_exception
end if

for ll_index = 1 to ll_count
	
	ls_contno			= trim( ads_slippayindet.getitemstring( ll_index, "loancontract_no" ) )
	ls_contcoopid		= ads_slippayindet.object.concoop_id[ll_index]
	ldc_prnretamt		= ads_slippayindet.getitemdecimal( ll_index, "principal_payamt" )
	ldc_intretamt		= ads_slippayindet.getitemdecimal( ll_index, "interest_payamt" )
	ldc_bfprnbal			= ads_slippayindet.object.item_balance[ ll_index ]
	ldc_intarr			= ads_slippayindet.object.intarrear_payamt[ ll_index ]
	ldc_bfintarrear		= ads_slippayindet.object.bfintarr_amt[ ll_index ]
	ldtm_calintfrom 	= ads_slippayindet.object.calint_from[ ll_index ]
	ldtm_calintto		= ads_slippayindet.object.calint_to[ ll_index ]
	
	// กำหนดค่าต่าง ๆ
	ls_lwditemtype		= ads_slippayindet.object.stm_itemtype[ll_index]

	ldc_intarrbal			= ldc_bfintarrear - ldc_intarr

	// บันทึกข้อมูลโดยดูจากงวดที่รับเงิน
	update	lncontmaster
	set			interest_arrear		= :ldc_intarrbal
	where	coop_id				= :ls_contcoopid
	and		( loancontract_no	= :ls_contno )
	using		itr_sqlca ;
	
	if itr_sqlca.sqlcode <> 0 then
		ithw_exception.text = "ไม่สามารถปรับปรุงยอดต้นคืนดอกเบี้ยคืนได้ เลขสัญญา : "+ls_contno
		ithw_exception.text += "~r~n"+itr_sqlca.sqlerrtext
		throw ithw_exception
	end if
	
	// บันทึกลง Statement
	lstr_lnstm.loancontract_no			= ls_contno
	lstr_lnstm.contcoop_id				= ls_contcoopid
	lstr_lnstm.slip_date					= ldtm_slipdate
	lstr_lnstm.operate_date				= ldtm_opedate
	lstr_lnstm.account_date				= ldtm_slipdate
	lstr_lnstm.intaccum_date				= ldtm_opedate
	lstr_lnstm.ref_slipno					= ls_slipno
	lstr_lnstm.ref_docno					= ls_docno
	lstr_lnstm.loanitemtype_code		= ls_lwditemtype
	lstr_lnstm.period						= 0
	lstr_lnstm.principal_payment		= 0
	lstr_lnstm.interest_payment			= ldc_intarrbal
	lstr_lnstm.principal_balance			= ldc_prnbal
	lstr_lnstm.prncalint_amt				= 0
	lstr_lnstm.calint_from					= ldtm_calintfrom
	lstr_lnstm.calint_to					= ldtm_calintto
	lstr_lnstm.bfinterest_arrear			= ldc_bfintarrear
	lstr_lnstm.interest_period			= 0
	lstr_lnstm.interest_arrear			= ldc_bfintarrear
	lstr_lnstm.moneytype_code			= ls_moneytype
	lstr_lnstm.item_status				= 1
	lstr_lnstm.entry_id						= ls_entryid
	lstr_lnstm.entry_bycoopid			= ls_coopid
	
	this.of_post_lncont_stm( lstr_lnstm )
	
next

return 1
end function

protected function integer of_post_slslippayout (n_ds ads_slippayout, n_ds ads_slippayoutdet) throws Exception;string		ls_slipno, ls_contno, ls_moneytype, ls_lwditemtype
string		ls_entryid,ls_docno,ls_accid,ls_branch,ls_bank , ls_branchoper
string		ls_slipitemtype , ls_shrtype
string		ls_coopid , ls_contcoopid , ls_memcoopid , ls_memno
long		ll_row, ll_index, ll_count
integer	li_tranyear , li_seqno
dec{2}	ldc_prnretamt, ldc_intretamt, ldc_intretbal, ldc_prnbal,ldc_prnretbal,	ldc_bfprnretbal
dec{2}	ldc_bfintretbal, ldc_bfprnbal, ldc_bfintarrear , ldc_itemamt
dec{2}	ldc_itemdetamt , ldc_shrbal
datetime	ldtm_entrydate
datetime	ldtm_slipdate, ldtm_opedate, ldtm_null

str_money_return_lncontstm	lstr_lnstm
str_money_return_shsharestm	lstr_shstm

setnull( ldtm_null )

ls_coopid			= ads_slippayout.object.coop_id[1]
ls_memcoopid	= ads_slippayout.object.memcoop_id[1]
ls_memno		= ads_slippayout.object.member_no[1]
ls_slipno			= trim( ads_slippayout.getitemstring( 1, "payoutslip_no" ) )
ls_docno			= trim( ads_slippayout.getitemstring( 1, "document_no" ) )
ls_entryid		= ads_slippayout.getitemstring( 1, "entry_id" )
ls_moneytype 	= ads_slippayout.getitemstring( 1, "moneytype_code" )
ls_bank 			= ads_slippayout.getitemstring( 1, "expense_bank" )
ls_branch 		= ads_slippayout.getitemstring( 1, "expense_branch" )
ls_accid 			= ads_slippayout.getitemstring( 1, "expense_accid" )
ldc_itemamt		= ads_slippayout.object.payoutnet_amt[1]
ldtm_slipdate	= ads_slippayout.getitemdatetime( 1, "slip_date" )
ldtm_opedate	= ads_slippayout.getitemdatetime( 1, "operate_date" )
ldtm_entrydate	= datetime( today(), now() )

ll_count		= ads_slippayoutdet.rowcount()

if ll_count <= 0 then
//	destroy ads_slippayoutdet
	ithw_exception.text	= "ไม่พบข้อมูลทำรายการ"
	throw ithw_exception
end if

for ll_index = 1 to ll_count
	ls_contno			= trim( ads_slippayoutdet.getitemstring( ll_index, "loancontract_no" ) )
	ls_contcoopid		= ads_slippayoutdet.object.concoop_id[ll_index]
	ls_shrtype			= ads_slippayoutdet.object.shrlontype_code[ll_index]
	ls_slipitemtype		= ads_slippayoutdet.object.slipitemtype_code[ll_index]
	ldc_prnretamt		= ads_slippayoutdet.getitemdecimal( ll_index, "principal_payamt" )
	ldc_intretamt		= ads_slippayoutdet.getitemdecimal( ll_index, "interest_payamt" )
	ldc_itemdetamt		= ads_slippayoutdet.getitemdecimal( ll_index, "item_payamt" )

	choose case ls_slipitemtype
		case "MRL"
			select principal_balance , interest_arrear , interest_return , principal_return
			into :ldc_bfprnbal , :ldc_bfintarrear , :ldc_bfintretbal , :ldc_bfprnretbal
			from lncontmaster
			where coop_id = :ls_contcoopid
			and loancontract_no = :ls_contno
			using itr_sqlca ;
		
			if itr_sqlca.sqlcode <> 0 then
				ithw_exception.text	= "ไม่พบข้อมูล เลขสัญญา : "+ls_contno
				throw ithw_exception
			end if
		
			// กำหนดค่าต่าง ๆ
			ls_lwditemtype		= 'LRT'
			ldc_prnbal			= ldc_bfprnbal + ldc_prnretamt	
			ldc_intretbal			= ldc_bfintretbal - ldc_intretamt	
			ldc_prnretbal		= ldc_bfprnretbal - ldc_prnretamt	
			
			// บันทึกข้อมูลโดยดูจากงวดที่รับเงิน
			update	lncontmaster
			set			principal_balance	= :ldc_prnbal,
						interest_return		= :ldc_intretbal,
						principal_return	= :ldc_prnretbal
			where	coop_id				= :ls_contcoopid
			and		( loancontract_no	= :ls_contno )
			using		itr_sqlca ;
			
			if itr_sqlca.sqlcode <> 0 then
				ithw_exception.text = "ไม่สามารถปรับปรุงยอดต้นคืนดอกเบี้ยคืนได้ เลขสัญญา : "+ls_contno
				ithw_exception.text += "~r~n"+itr_sqlca.sqlerrtext
				throw ithw_exception
			end if
			
			// บันทึกลง Statement
			lstr_lnstm.loancontract_no			= ls_contno
			lstr_lnstm.contcoop_id				= ls_contcoopid
			lstr_lnstm.slip_date					= ldtm_slipdate
			lstr_lnstm.operate_date				= ldtm_opedate
			lstr_lnstm.account_date				= ldtm_slipdate
			lstr_lnstm.intaccum_date				= ldtm_opedate
			lstr_lnstm.ref_slipno					= ls_slipno
			lstr_lnstm.ref_docno					= ls_docno
			lstr_lnstm.loanitemtype_code		= ls_lwditemtype
			lstr_lnstm.period						= 0
			lstr_lnstm.principal_payment		= ldc_prnretamt
			lstr_lnstm.interest_payment			= ldc_intretamt
			lstr_lnstm.principal_balance			= ldc_prnbal
			lstr_lnstm.prncalint_amt				= 0
			lstr_lnstm.calint_from					= ldtm_null
			lstr_lnstm.calint_to					= ldtm_null
			lstr_lnstm.bfinterest_arrear			= ldc_bfintarrear
			lstr_lnstm.interest_period			= 0
			lstr_lnstm.interest_arrear			= ldc_bfintarrear
			lstr_lnstm.moneytype_code			= ls_moneytype
			lstr_lnstm.item_status				= 1
			lstr_lnstm.entry_id						= ls_entryid
			lstr_lnstm.entry_bycoopid			= ls_coopid
			
			try
				this.of_post_lncont_stm( lstr_lnstm )
			catch( Exception lthw_pslon )
				ithw_exception.text += lthw_pslon.text
				throw ithw_exception
			end try
			
		case "MRS"
			
			select sharestk_amt
			into :ldc_shrbal
			from shsharemaster
			where coop_id = :ls_memcoopid
			and member_no = :ls_memno
			using itr_sqlca ;
			
			if itr_sqlca.sqlcode <> 0 then
				ithw_exception.text = "ไม่สามารถปรับปรุงยอดหุ้นคืนได้ เลขประจำตัว : "+ls_contno
				ithw_exception.text += "~r~n"+itr_sqlca.sqlerrtext
				throw ithw_exception
			end if
			
			ldc_shrbal	-= ldc_itemdetamt
			
			// บันทึกข้อมูลโดยดูจากงวดที่รับเงิน
			update	shsharemaster
			set			sharestk_amt		= :ldc_shrbal
			where	coop_id				= :ls_memcoopid
			and		( member_no		= :ls_memno )
			using		itr_sqlca ;
			
			if itr_sqlca.sqlcode <> 0 then
				ithw_exception.text = "ไม่สามารถปรับปรุงยอดต้นคืนดอกเบี้ยคืนได้ เลขสัญญา : "+ls_memno
				ithw_exception.text += "~r~n"+itr_sqlca.sqlerrtext
				throw ithw_exception
			end if
			
			lstr_shstm.member_no				= ls_memno
			lstr_shstm.memcoop_id				= ls_memcoopid
			lstr_shstm.sharetype_code			= ls_shrtype
			lstr_shstm.slip_date					= ldtm_slipdate
			lstr_shstm.operate_date				= ldtm_opedate
			lstr_shstm.account_date				= ldtm_slipdate
			lstr_shstm.sharetime_date			= ldtm_opedate
			lstr_shstm.document_no				= ls_docno
			lstr_shstm.stmitem_code				= "SRT"
			lstr_shstm.period						= 0
			lstr_shstm.sharepay_amt			= ldc_itemdetamt
			lstr_shstm.sharebal_amt				= ldc_shrbal
			lstr_shstm.entry_id					= ls_entryid
			lstr_shstm.entry_bycoopid			= ls_coopid
			lstr_shstm.moneytype					= "TRN"
			lstr_shstm.bfsharearr_amt			= 0
			lstr_shstm.sharearr_amt				= 0
			lstr_shstm.item_status				= 1
			lstr_shstm.ref_slipno					= ls_slipno
			
			try
				this.of_post_shshare_stm( lstr_shstm )
			catch( Exception lthw_psshr )
				ithw_exception.text += lthw_psshr.text
				throw ithw_exception
			end try
			
	end choose
	
next

if ls_moneytype = "TRN" and ldc_itemamt > 0 then
	
	li_tranyear		= integer(string( ldtm_opedate , 'yyyy' ) )+543
	ls_branchoper	= right( ls_coopid , 3 )
	select		max( seq_no )
	into		:li_seqno
	from		dpdepttran
	where	coop_id				= :ls_coopid
	and		deptaccount_no	= :ls_accid
	and		tran_year			= :li_tranyear
	and		tran_date			= :ldtm_opedate
	using itr_sqlca;
	if isnull( li_seqno ) or itr_sqlca.sqlcode <> 0 then li_seqno = 0
	li_seqno ++
	
	insert into dpdepttran
	( coop_id , deptaccount_no , memcoop_id , member_no , system_code , tran_year , tran_date , seq_no , deptitem_amt , tran_status , branch_operate )
	values
	( :ls_coopid , :ls_accid , :ls_memcoopid , :ls_memno , 'TRL' , :li_tranyear , :ldtm_opedate , :li_seqno , :ldc_itemamt , 0 , :ls_branchoper)
	using itr_sqlca;
	
	if itr_sqlca.sqlcode <> 0 then
		ithw_exception.text = "ไม่สามารถผ่านรายการไประบบเงินฝากได้ ทะเบียน : "+ls_memno
		ithw_exception.text += "~r~n"+itr_sqlca.sqlerrtext
		throw ithw_exception
	end if
	
end if

return 1
end function

protected function integer of_setsrvdoccontrol (boolean ab_switch);// Check argument
if isnull( ab_switch ) then
	return -1
end if

if ab_switch then
	if isnull( inv_docsrv ) or not isvalid( inv_docsrv ) then
		inv_docsrv	= create n_cst_doccontrolservice
		inv_docsrv.of_settrans( inv_transection )
		return 1
	end if
else
	if isvalid( inv_docsrv ) then
		destroy inv_docsrv
		return 1
	end if
end if

return 0
end function

protected function integer of_setsrvdwxmlie (boolean ab_switch);// Check argument
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

protected function integer of_setsrvmrproc (boolean ab_switch);// Check argument
if isnull( ab_switch ) then
	return -1
end if

if ab_switch then
	if isnull( inv_mrprocsrv ) or not isvalid( inv_mrprocsrv ) then
		inv_mrprocsrv	= create n_cst_money_return_proc_service
		inv_mrprocsrv.of_initservice( inv_transection )
		return 1
	end if
else
	if isvalid( inv_mrprocsrv ) then
		destroy inv_mrprocsrv
		return 1
	end if
end if

return 0
end function

protected function integer of_setsrvproc (boolean ab_switch);// Check argument
if isnull( ab_switch ) then
	return -1
end if

if ab_switch then
	if isnull( inv_procsrv ) or not isvalid( inv_procsrv ) then
		inv_procsrv	= create n_cst_procservice
		inv_procsrv.of_initservice(  )
		return 1
	end if
else
	if isvalid( inv_procsrv ) then
		destroy inv_procsrv
		return 1
	end if
end if

return 0
end function

public function integer of_post_mr_opr_proc_crt_msk (n_ds ads_option) throws Exception;string ls_sqlselect , ls_sql
string ls_coop , ls_recv
string ls_entry, ls_operate
string ls_mrcrtsdtm , ls_mrcrtedtm
datetime ldtm_mrcrts , ldtm_mrcrte,ldtm_operate
boolean lb_err

inv_progress.istr_progress.subprogress_text = "ลบข้อมูลสร้างทะเบียนคืน รอสักครู่..."
inv_progress.istr_progress.subprogress_index = 0
inv_progress.istr_progress.subprogress_max = 3

ls_coop			= ads_option.object.coop_id[1]
ls_recv			= ads_option.object.recv_period[1]
ls_entry			= ads_option.object.entry_id[1]
ldtm_mrcrts		= ads_option.object.mrcreate_sdate[1]
ldtm_mrcrte		= ads_option.object.mrcreate_edate[1]
ldtm_operate	= ads_option.object.operate_date[1] 

ls_mrcrtsdtm 	= string( ldtm_mrcrts , 'dd/mm/yyyy' )
ls_mrcrtedtm	= string( ldtm_mrcrte , 'dd/mm/yyyy' )
ls_operate	= string( ldtm_operate , 'dd/mm/yyyy' )

try
	inv_procsrv.of_set_sqlselect( "mbmembmaster")
	inv_procsrv.of_get_sqlselect( ls_sqlselect )
catch( throwable lthw_sqlselect )
	ithw_exception.text	+= lthw_sqlselect.text
	ithw_exception.text 	+= "~r~nกรุณาตรวจสอบ"
	lb_err = true 
end try
if lb_err then Goto objdestroy

// ลบข้อมูล
ls_sql		= " delete from mbmoneyreturn "
ls_sql		+= " where mbmoneyreturn.return_status = 8 "
ls_sql		+= " and exists ( select 1 from mbmembmaster where mbmembmaster.coop_id = mbmoneyreturn.coop_id and mbmembmaster.member_no = mbmoneyreturn.member_no "
ls_sql		+= " and mbmembmaster.coop_id = '"+ls_coop+"' "
ls_sql		+= ls_sqlselect
ls_sql		+= " ) "
execute immediate :ls_sql using itr_sqlca;

if itr_sqlca.sqlcode <> 0 then
	ithw_exception.text 	= "พบข้อผิดพลาด of_post_mr_opr_proc_crt (70.1) "
	ithw_exception.text 	+= "~r~nไม่สามารถเคลียร์ต้นคืน ได้"
	ithw_exception.text 	+= "~r~n"+ string( itr_sqlca.sqlcode ) + "  " + itr_sqlca.sqlerrtext
	ithw_exception.text 	+= "~r~nกรุณาตรวจสอบ"
	lb_err = true ; if lb_err then Goto objdestroy
end if

// gen ต้นเงิน  จากยอดคงเหลือที่ติดลบ
ls_sql		= " insert into mbmoneyreturn "
ls_sql		+= " ( coop_id , member_no , "
ls_sql		+= " seq_no , "
ls_sql		+= " system_from , description , shrlontype_code , bizzcoop_id , loancontract_no , returnitemtype_code , return_amount , return_status , entry_id, return_date ) "
ls_sql		+= " select coop_id , member_no , "
ls_sql		+= " nvl( ( select max( mr.seq_no ) from mbmoneyreturn mr where mr.coop_id = lncontmaster.memcoop_id and mr.member_no = mr.member_no ) , 0 ) + rank() over ( partition by member_no order by loancontract_no ) as f_row , "
ls_sql		+= " 'LON' , 'PRIN' , loantype_code , coop_id , loancontract_no , 'PRN' , case when principal_balance < 0 then   abs( principal_balance ) else nkeep_principal end , 8 , '" + ls_entry + "', to_date('" +ls_operate + "','dd/mm/yyyy') "
ls_sql		+= " from lncontmaster where ( principal_balance < 0 ) or ( principal_balance = 0 and nkeep_principal > 0 ) "
ls_sql		+= " and exists ( select 1 from mbmembmaster where mbmembmaster.coop_id = lncontmaster.memcoop_id and mbmembmaster.member_no = lncontmaster.member_no "
ls_sql		+= " and mbmembmaster.coop_id = '"+ls_coop+"' "
ls_sql		+= ls_sqlselect 
ls_sql		+= " ) "
execute immediate :ls_sql using itr_sqlca;

if itr_sqlca.sqlcode <> 0 then
	ithw_exception.text 	= "พบข้อผิดพลาด of_post_mr_opr_proc_crt (70.2) "
	ithw_exception.text 	+= "~r~nไม่สามารถทำรายการต้นคืน ได้"
	ithw_exception.text 	+= "~r~n"+ string( itr_sqlca.sqlcode ) + "  " + itr_sqlca.sqlerrtext
	ithw_exception.text 	+= "~r~nกรุณาตรวจสอบ"
	lb_err = true ; if lb_err then Goto objdestroy
end if

// gen ดอกเบี้ย จากยอด ดอกเบียคืนใน master
ls_sql		= " insert into mbmoneyreturn "
ls_sql		+= " ( coop_id , member_no , "
ls_sql		+= " seq_no , "
ls_sql		+= " system_from , description , shrlontype_code , bizzcoop_id , loancontract_no , returnitemtype_code , return_amount , return_status , entry_id , return_date) "
ls_sql		+= " select coop_id , member_no , "
ls_sql		+= " nvl( ( select max( mr.seq_no ) from mbmoneyreturn mr where mr.coop_id = lncontmaster.memcoop_id and mr.member_no = mr.member_no ) , 0 ) + rank() over ( partition by member_no order by loancontract_no ) as f_row , "
ls_sql		+= " 'LON' , 'GEN INT' , loantype_code , coop_id , loancontract_no , 'INT' , case when principal_balance < 0 then     interest_return when principal_balance = 0 and interest_return > 0  then interest_return   else interest_return  end   , 8 , '" + ls_entry + "', to_date('" +ls_operate + "','dd/mm/yyyy')"
ls_sql		+= " from lncontmaster where ( interest_return > 0 or ( principal_balance = 0 and nkeep_interest > 0 )) "
ls_sql		+= " and exists ( select 1 from mbmembmaster where mbmembmaster.coop_id = lncontmaster.memcoop_id and mbmembmaster.member_no = lncontmaster.member_no "
ls_sql		+= " and mbmembmaster.coop_id = '"+ls_coop+"' "
ls_sql		+= ls_sqlselect
ls_sql		+= " ) "
execute immediate :ls_sql using itr_sqlca;

if itr_sqlca.sqlcode <> 0 then
	ithw_exception.text 	= "พบข้อผิดพลาด of_post_mr_opr_proc_crt (70.2) "
	ithw_exception.text 	+= "~r~nไม่สามารถทำรายการดอกเบี้ยคืน ได้"
	ithw_exception.text 	+= "~r~n"+ string( itr_sqlca.sqlcode ) + "  " + itr_sqlca.sqlerrtext
	ithw_exception.text 	+= "~r~nกรุณาตรวจสอบ"
	lb_err = true ; if lb_err then Goto objdestroy
end if
//-----------------------  005001  เพิ่มที่สุรินทร์ -------------------------------


if itr_sqlca.sqlcode <> 0 then
	ithw_exception.text 	= "พบข้อผิดพลาด of_post_mr_opr_proc_crt (70.3) "
	ithw_exception.text 	+= "~r~nไม่สามารถทำรายการดอกเบี้ยคืน ได้"
	ithw_exception.text 	+= "~r~n"+ string( itr_sqlca.sqlcode ) + "  " + itr_sqlca.sqlerrtext
	ithw_exception.text 	+= "~r~nกรุณาตรวจสอบ"
	lb_err = true ; if lb_err then Goto objdestroy
end if


//หุ้นรอจ่ายคืน
ls_sql		= " insert into mbmoneyreturn "
ls_sql		+= " ( coop_id , member_no , "
ls_sql		+= " seq_no , "
ls_sql		+= " system_from , description , shrlontype_code , bizzcoop_id , loancontract_no , returnitemtype_code , return_amount , return_status , entry_id , return_date) "
ls_sql		+= " select coop_id , member_no , "
ls_sql		+= " nvl( ( select max( mr.seq_no ) from mbmoneyreturn mr where mr.coop_id = shsharemaster.coop_id and mr.member_no = shsharemaster.member_no ) , 0 ) + rank() over ( partition by member_no order by member_no ) as f_row , "
ls_sql		+= " 'LON' , 'SHR' , sharetype_code , coop_id , member_no , 'SHR' ,  case when sharestk_amt = 0 then rkeep_sharevalue else  abs( sharestk_amt * 10 ) end , 8 , '" + ls_entry + "', to_date('" +ls_operate + "','dd/mm/yyyy') "
ls_sql		+= " from shsharemaster where  ( (sharestk_amt < 0)  or ( sharestk_amt >  0 and  sharemaster_status <> 1 )    or (sharestk_amt = 0 and rkeep_sharevalue > 0 and last_period > 0 ) )   "
ls_sql		+= " and exists ( select 1 from mbmembmaster where mbmembmaster.coop_id = shsharemaster.coop_id and mbmembmaster.member_no = shsharemaster.member_no "
ls_sql		+= " and mbmembmaster.coop_id = '"+ls_coop+"' "
ls_sql		+= ls_sqlselect
ls_sql		+= " ) "
execute immediate :ls_sql using itr_sqlca;

if itr_sqlca.sqlcode <> 0 then
	ithw_exception.text 	= "พบข้อผิดพลาด of_post_mr_opr_proc_crt (70.4) "
	ithw_exception.text 	+= "~r~nไม่สามารถทำรายการหุ้นรอจ่ายคืน ได้"
	ithw_exception.text 	+= "~r~n"+ string( itr_sqlca.sqlcode ) + "  " + itr_sqlca.sqlerrtext
	ithw_exception.text 	+= "~r~nกรุณาตรวจสอบ"
	lb_err = true ; if lb_err then Goto objdestroy
end if


objdestroy:

if lb_err then
	ithw_exception.text = "~r~nn_cst_divsrv_process.of_post_mr_opr_proc_crt()" + ithw_exception.text
	throw ithw_exception
end if

return 1
end function

public function integer of_test_mr_payint_surin (n_ds ads_option, datetime adtm_calintto) throws Exception;string	ls_coopid , ls_payinslip , ls_document
string ls_contcoop , ls_contno , ls_loantype,ls_recv,ls_entry
string ls_memcoop , ls_memno, ls_coop, ls_clrcontno
integer li_row , li_subrow , li_chkcnt, li_day,li_rowcount
integer li_lastperiod , li_contract, li_rowww
dec{2} ldc_prinbal , ldc_periodpay , ldc_intarr , ldc_nkpint , ldc_rkpprin , ldc_rkpint,ldc_intarrbf
dec{2} ldc_tppayoutnet , ldc_payoutnet , ldc_payintarr , ldc_slipclr, ldc_return = 0
dec{4} ldc_intrate
datetime ldtm_lastcal , ldtm_lastproc,ldtm_mrcrts,ldtm_mrcrte, ldtm_refslip
boolean lb_err
throwable lthw_mrpayint	
datastore lds_loanlwd  , lds_mbdet
lds_loanlwd = create datastore
lds_loanlwd.dataobject = "d_kp_gen_list_cal_int_arrear"
lds_loanlwd.settransobject( itr_sqlca)

lds_mbdet = create datastore
lds_mbdet.dataobject = "d_kp_loan_moneyreturn_int_returnpx"
lds_mbdet.settransobject( itr_sqlca)


ls_coop			= ads_option.object.coop_id[1]
ls_recv			= ads_option.object.recv_period[1]
ls_entry			= ads_option.object.entry_id[1]
ldtm_mrcrts		= ads_option.object.mrcreate_sdate[1]
ldtm_mrcrte		= ads_option.object.mrcreate_edate[1]
//inv_intsrv.set
//ls_mrcrtsdtm 	= string( ldtm_mrcrts , 'dd/mm/yyyy' )
//ls_mrcrtedtm	= string( ldtm_mrcrte , 'dd/mm/yyyy' )

li_rowcount = lds_loanlwd.retrieve()


for li_row = 1 to li_rowcount
	ls_memno  = lds_loanlwd.getitemstring( li_row,"member_no")
	ls_contno  = lds_loanlwd.getitemstring( li_row,"loancontract_no")
	ls_loantype =  lds_loanlwd.getitemstring( li_row,"loantype_code")
	ldtm_lastcal = lds_loanlwd.getitemdatetime(li_row,"lastcalint_date")
	ldc_prinbal   =  lds_loanlwd.getitemdecimal( li_row,"principal_balance")
	ldc_intarrbf   =  lds_loanlwd.getitemdecimal( li_row,"interest_arrear")
	ls_contcoop  =  lds_loanlwd.getitemstring( li_row,"coop_id")
	
	if isnull( ldc_intarrbf ) then ldc_intarrbf = 0
	//wa
			// คำนวณดอกเบี้ย
			ldc_intarr		=  inv_intsrv.of_computeinterest(ls_contcoop, ls_contno, ldc_prinbal, ldtm_lastcal, ldtm_mrcrte )

		ldc_intarr =  ldc_intarr +  ldc_intarrbf
		li_rowww = lds_mbdet.retrieve( ls_memno, ls_contno )
		if li_rowww > 0 then
			ls_clrcontno = lds_mbdet.getitemstring( 1,"loancontract_no")
			ldc_return = lds_mbdet.getitemdecimal(1,"sum_ret")
			ldtm_refslip = ldtm_lastcal
		else
			continue;
		end if
		
		if isnull( ldc_return ) then ldc_return = 0
		
		if ldc_return < ldc_intarr then
			continue;
			ls_contno = ""
			ldc_prinbal = 0
			ldc_intarr = 0
		end if
		
		
		update mbmoneyreturn 
		set contno_new = :ls_contno, ref_slipdate = :ldtm_refslip, interest_payamt = :ldc_intarr,loanbalance_new = :ldc_prinbal
		where member_no = :ls_memno and  loancontract_no = :ls_clrcontno      using itr_sqlca;
	next
	
	update mbmoneyreturn a
	set a.loanapprove_new = ( select b.loanapprove_amt from lncontmaster b where a.contno_new = b.loancontract_no )
	where  a.return_status = 8 using itr_sqlca ;

destroy lds_mbdet
destroy lds_loanlwd
return 1
end function

on n_cst_money_return_opr.create
call super::create
TriggerEvent( this, "constructor" )
end on

on n_cst_money_return_opr.destroy
TriggerEvent( this, "destructor" )
call super::destroy
end on

event constructor;ithw_exception = create Exception
end event

event destructor;if isvalid( ithw_exception ) then destroy ithw_exception
if isvalid( inv_progress ) then destroy inv_progress
if isvalid( inv_dwxmliesrv ) then destroy inv_dwxmliesrv
if isvalid( inv_mrprocsrv ) then destroy inv_mrprocsrv
if isvalid( inv_docsrv ) then destroy inv_docsrv
end event
