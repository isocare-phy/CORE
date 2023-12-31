$PBExportHeader$n_cst_lncoopsrv_allrequest.sru
$PBExportComments$lcsrv ส่วนบริการใบคำขอต่างๆ
forward
global type n_cst_lncoopsrv_allrequest from nonvisualobject
end type
end forward

global type n_cst_lncoopsrv_allrequest from nonvisualobject
end type
global n_cst_lncoopsrv_allrequest n_cst_lncoopsrv_allrequest

type variables
transaction	itr_sqlca
Exception	ithw_exception

string			is_coopcontrol

n_cst_dbconnectservice		inv_transection
n_cst_lncoopsrv_interest		inv_intsrv
n_cst_doccontrolservice		inv_docsrv
n_cst_datetimeservice		inv_datetimesrv
n_cst_dwxmlieservice			inv_dwxmliesrv

string		DWCONTADJ			= "d_lcsrv_req_contadj"
string		DWCONTADJPAY		= "d_lcsrv_req_contadj_payment"
string		DWCONTADJINT		= "d_lcsrv_req_contadj_contint"
string		DWCONTADJINTSPC	= "d_lcsrv_req_contadj_contintspc"
string		DWCONTADJCOLL		= "d_lcsrv_req_contadj_collateral"
end variables

forward prototypes
public function integer of_initservice (n_cst_dbconnectservice anv_dbtrans) throws Exception
private function integer of_setsrvlcinterest (boolean ab_switch)
private function integer of_setsrvdwxmlie (boolean ab_switch)
private function integer of_setsrvdoccontrol (boolean ab_switch)
private function integer of_setsrvdatetime (boolean ab_switch)
public function integer of_setdsmodify (ref n_ds ads_requester, long al_row, boolean ab_keymodify)
public function integer of_init_lcreqloan (ref str_lcreqloan astr_lcreqloan) throws Exception
public function integer of_init_lcapvloan (ref str_lcreqloan astr_lcreqloan) throws Exception
public function string of_gennewcontractno (string as_coopid, string as_loantype) throws Exception
private function integer of_buildcontno_reqloan (string as_coopid, string as_lnreqno, ref string as_lncontno) throws Exception
public function boolean of_checkcontadjustdet (n_ds ads_data, string as_oldcol[], string as_newcol[])
public function integer of_init_reqcontadjust (ref str_lccontaj astr_lccontaj) throws Exception
public function boolean of_checkcontadjustdetrow (n_ds ads_old, n_ds ads_new, string as_col[])
private function integer of_setlncontadjdata (str_lccontaj astr_lccontaj, ref n_ds ads_adjpay, ref n_ds ads_adjint, ref n_ds ads_adjintspc, ref n_ds ads_adjcoll, ref n_ds ads_adjintspcold, ref n_ds ads_adjcollold) throws Exception
private function integer of_setlncontadjdocno (string as_ecoopid, string as_entryid, ref n_ds ads_adjmast, ref n_ds ads_adjpay, ref n_ds ads_adjint, ref n_ds ads_adjintspc, ref n_ds ads_adjcoll, ref n_ds ads_adjintspcold, ref n_ds ads_adjcollold) throws Exception
private function integer of_post_reqcontadj_pay (n_ds ads_adjmast, n_ds ads_adjpay) throws Exception
private function integer of_post_reqcontadj_int (n_ds ads_adjmast, n_ds ads_adjint, n_ds ads_adjintspc) throws Exception
private function integer of_post_reqcontadj_coll (n_ds ads_adjmast, n_ds ads_adjcoll) throws Exception
public function string of_getmembdetail (string as_coopid, string as_memno)
public function integer of_open_reqloan (ref str_lcreqloan astr_lcreqloan) throws Exception
public function integer of_init_reqcontsign (ref str_lccontsign astr_lccontsign) throws Exception
public function integer of_save_reqcontsign (ref str_lccontsign astr_lccontsign) throws Exception
public function integer of_init_reqcontplanrcv (ref str_lccontplanrcv astr_lccontplanrcv) throws Exception
public function integer of_save_reqcontplan (ref str_lccontplanrcv astr_lccontplanrcv) throws Exception
public function integer of_save_lcapvloan (ref str_lcreqloan astr_lcapvloan) throws Exception
public function integer of_save_reqchgcontplace (ref str_lccontplace astr_lccontplace) throws Exception
public function integer of_save_lcreqloan (ref str_lcreqloan astr_lcreqloan) throws Exception
public function integer of_save_reqcontadjust (ref str_lccontaj astr_lccontaj) throws Exception
public function integer of_init_printcontract (ref str_lcprintcont astr_lcprintcont) throws Exception
public function integer of_save_printcontract (ref str_lcprintcont astr_lcprintcont) throws Exception
private function integer of_post_printpersdet (n_ds ads_contcoll) throws Exception
private function integer of_post_printcontcoll (n_ds ads_contcoll, string as_concoopid, string as_contno) throws Exception
end prototypes

public function integer of_initservice (n_cst_dbconnectservice anv_dbtrans) throws Exception;/***********************************************************
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
itr_sqlca 			= anv_dbtrans.itr_dbconnection
is_coopcontrol	= anv_dbtrans.is_coopcontrol

if isnull( inv_transection ) or not isvalid( inv_transection ) then
	inv_transection = create n_cst_dbconnectservice
	inv_transection = anv_dbtrans
end if

return 1
end function

private function integer of_setsrvlcinterest (boolean ab_switch);// Check argument
if isnull( ab_switch ) then
	return -1
end if

if ab_switch then
	if isnull( inv_intsrv ) or not isvalid( inv_intsrv ) then
		inv_intsrv	= create n_cst_lncoopsrv_interest
		inv_intsrv.of_initservice( inv_transection )
		return 1
	end if
else
	if isvalid( inv_intsrv ) then
		destroy inv_intsrv
		return 1
	end if
end if

return 0
end function

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

private function integer of_setsrvdoccontrol (boolean ab_switch);// Check argument
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

private function integer of_setsrvdatetime (boolean ab_switch);// Check argument
if isnull( ab_switch ) then
	return -1
end if

if ab_switch then
	if isnull( inv_datetimesrv ) or not isvalid( inv_datetimesrv ) then
		inv_datetimesrv	= create n_cst_datetimeservice
		return 1
	end if
else
	if isvalid( inv_datetimesrv ) then
		destroy inv_datetimesrv
		return 1
	end if
end if

return 0
end function

public function integer of_setdsmodify (ref n_ds ads_requester, long al_row, boolean ab_keymodify);string		ls_iskey
long		ll_index, ll_count

ads_requester.setitemstatus( al_row, 0, primary!, datamodified! )

// ปรับ Attrib ทุก Column ให้เป็น Update ซะ
ll_count	= long( ads_requester.describe( "DataWindow.Column.Count" ) )
for ll_index = 1 to ll_count
	
	ls_iskey	= ads_requester.describe("#"+string( ll_index )+".Key")
	
	// ถ้าเป็น PK และเงื่อนไขคือไม่ปรับ Key ไม่ต้องปรับสถานะ
	if upper( ls_iskey ) = "YES" and not( ab_keymodify ) then
		continue
	end if
	
	ads_requester.setitemstatus( 1, ll_index, primary!, datamodified! )
next

return 1
end function

public function integer of_init_lcreqloan (ref str_lcreqloan astr_lcreqloan) throws Exception;string		ls_coopid, ls_mcoopid, ls_memno, ls_loantype
string		ls_continttab, ls_contintfix, ls_permgrpcode, ls_objcode
integer	li_lnpaytype, li_odflag, li_continttype, li_intsteptype, li_maxperiod, li_memtime
long		ll_row
dec{2}	ldc_lnpayfix, ldc_maxloan
dec		ldc_lnpaypercent, ldc_contintrate, ldc_contintinc, ldc_lnintrate
boolean	lb_error
datetime	ldtm_reqdate, ldtm_memdate, ldtm_mindate
n_ds		lds_reqmain, lds_reqclr, lds_reqclroth, lds_reqintspc

ls_mcoopid		= astr_lcreqloan.memcoop_id
ls_memno		= astr_lcreqloan.member_no
ls_loantype		= astr_lcreqloan.loantype_code
ldtm_reqdate	= astr_lcreqloan.loanrequest_date

ls_coopid			= is_coopcontrol

select		maxloan_amt, loanpermgrp_code,
			loanpayment_type, loanpayment_percent, loanpayment_fixamt,
			od_flag, defaultobj_code,
			contint_type, inttabfix_code, inttabrate_code, 
			intrate_increase, intstep_type
into		:ldc_maxloan, :ls_permgrpcode,
			:li_lnpaytype, :ldc_lnpaypercent, :ldc_lnpayfix,
			:li_odflag, :ls_objcode,
			:li_continttype, :ls_contintfix, :ls_continttab,
			:ldc_contintinc, :li_intsteptype
from		lccfloantype
where	( coop_id			= :ls_coopid ) and
			( loantype_code	= :ls_loantype )
using		itr_sqlca ;

// งวดชำระ
select		max( max_period )
into		:li_maxperiod
from		lccfloantypeperiod
where	( coop_id		= :ls_coopid ) and
			( loantype_code	= :ls_loantype )
using		itr_sqlca ;

// หาอัตราดอกเบี้ยก่อน
try
	this.of_setsrvlcinterest( true )
	ldc_lnintrate		= inv_intsrv.of_getloantypeintrate( ls_loantype, ldtm_reqdate, 1, 1 )
	this.of_setsrvlcinterest( false )
catch( Exception lthw_interr )
	ithw_exception.text	= lthw_interr.text
	lb_error					= false
end try

if lb_error then throw ithw_exception

// สร้าง DS ไว้สำหรับใส่ข้อมูล
lds_reqmain		= create n_ds
lds_reqmain.dataobject	= "d_lcsrv_loanreq"

lds_reqclr		= create n_ds
lds_reqclr.dataobject	= "d_lcsrv_loanreq_clr"

lds_reqclroth	= create n_ds
lds_reqclroth.dataobject	= "d_lcsrv_loanreq_clroth"

lds_reqintspc	= create n_ds
lds_reqintspc.dataobject	= "d_lcsrv_loanreq_intspc"

// ให้ค่าอัตราดอกเบี้ยคงที่
if li_continttype = 1 then
	ldc_contintrate	= ldc_lnintrate
else
	ldc_contintrate	= 0
end if

// ส่วน Main
ll_row		= lds_reqmain.insertrow( 0 )

lds_reqmain.setitem( ll_row, "memcoop_id", ls_mcoopid )
lds_reqmain.setitem( ll_row, "member_no", ls_memno )
lds_reqmain.setitem( ll_row, "loantype_code", ls_loantype )
lds_reqmain.setitem( ll_row, "loanrequest_date", ldtm_reqdate )

lds_reqmain.setitem( ll_row, "loanpayment_type", li_lnpaytype )
lds_reqmain.setitem( ll_row, "loanpayment_percent", ldc_lnpaypercent )
lds_reqmain.setitem( ll_row, "loanpayment_fixamt", ldc_lnpayfix )
lds_reqmain.setitem( ll_row, "maxperiod_installment", li_maxperiod )
lds_reqmain.setitem( ll_row, "period_installment", li_maxperiod )

lds_reqmain.setitem( ll_row, "int_continttype", li_continttype )
lds_reqmain.setitem( ll_row, "int_contintrate", ldc_contintrate )
lds_reqmain.setitem( ll_row, "int_continttabcode", ls_continttab )
lds_reqmain.setitem( ll_row, "int_contintincrease", ldc_contintinc )
lds_reqmain.setitem( ll_row, "int_intsteptype", 1 )
lds_reqmain.setitem( ll_row, "contsign_intrate", ldc_lnintrate )
lds_reqmain.setitem( ll_row, "od_flag", li_odflag )
lds_reqmain.setitem( ll_row, "loanobjective_code", ls_objcode )

// ส่วนการหักกลบสัญญาเก่า
string		ls_clrcoopid, ls_clrcontno, ls_clrloantype
integer	li_clrlastperiod, li_clrcontstatus
dec		ldc_clrapvamt, ldc_clrprnbal, ldc_clrpayment
datetime	ldtm_clrlastcalint

declare lnold_cur cursor for
select		coop_id, loancontract_no, loantype_code, 
			loanapprove_amt, principal_balance,
			period_payment, last_periodpay,
			lastcalint_date, contract_status
from		lccontmaster
where	( memcoop_id	= :ls_mcoopid ) and
			( member_no	= :ls_memno ) and
			( contract_status > 0 ) and
			( principal_balance > 0 or interest_arrear > 0 )
using		itr_sqlca ;

open lnold_cur ;
	fetch lnold_cur into :ls_clrcoopid, :ls_clrcontno, :ls_clrloantype, :ldc_clrapvamt, :ldc_clrprnbal, :ldc_clrpayment, :li_clrlastperiod, :ldtm_clrlastcalint, :li_clrcontstatus ;
	
	do while itr_sqlca.sqlcode = 0
		ll_row		= lds_reqclr.insertrow( 0 )
		
		lds_reqclr.setitem( ll_row, "seq_no", ll_row )
		lds_reqclr.setitem( ll_row, "concoop_id", ls_clrcoopid )
		lds_reqclr.setitem( ll_row, "loancontract_no", ls_clrcontno )
		lds_reqclr.setitem( ll_row, "loantype_code", ls_clrloantype )
		lds_reqclr.setitem( ll_row, "loanapprove_amt", ldc_clrapvamt )
		lds_reqclr.setitem( ll_row, "principal_balance", ldc_clrprnbal )
		lds_reqclr.setitem( ll_row, "period_payment", ldc_clrpayment )
		lds_reqclr.setitem( ll_row, "last_periodpay", li_clrlastperiod )
		lds_reqclr.setitem( ll_row, "lastcalint_date", ldtm_clrlastcalint )
		lds_reqclr.setitem( ll_row, "contract_status", li_clrcontstatus )
		
		fetch lnold_cur into :ls_clrcoopid, :ls_clrcontno, :ls_clrloantype, :ldc_clrapvamt, :ldc_clrprnbal, :ldc_clrpayment, :li_clrlastperiod, :ldtm_clrlastcalint, :li_clrcontstatus ;
	loop
close lnold_cur ;

// ถ้าเป็นดอกเบี้ยอัตราพิเศษ
if li_continttype = 3 then
	string		ls_ispctabcode
	integer	li_ispcratetype, li_timetype, li_timeamt
	dec		ldc_ispcfixrate, ldc_ispcinc
	
	declare intspc_cur cursor for
	select		intrate_type, intratefix_rate, intratetab_code,
				intrate_increase, inttime_type, inttime_amt
	from		lccfloantypeintspc
	where	( coop_id			= :ls_coopid ) and
				( loantype_code	= :ls_loantype )
	using		itr_sqlca ;
	
	open intspc_cur ;
		fetch intspc_cur into :li_ispcratetype, :ldc_ispcfixrate, :ls_ispctabcode, :ldc_ispcinc, :li_timetype, :li_timeamt ;
		
		do while itr_sqlca.sqlcode = 0
			ll_row		= lds_reqintspc.insertrow( 0 )
			
			lds_reqintspc.setitem( ll_row, "seq_no", ll_row )
			lds_reqintspc.setitem( ll_row, "int_continttype", li_ispcratetype )
			lds_reqintspc.setitem( ll_row, "int_contintrate", ldc_ispcfixrate )
			lds_reqintspc.setitem( ll_row, "int_continttabcode", ls_ispctabcode )
			lds_reqintspc.setitem( ll_row, "int_contintincrease", ldc_ispcinc )
			lds_reqintspc.setitem( ll_row, "int_timetype", li_timetype )
			lds_reqintspc.setitem( ll_row, "int_timeamt", li_timeamt )
			
			fetch intspc_cur into :li_ispcratetype, :ldc_ispcfixrate, :ls_ispctabcode, :ldc_ispcinc, :li_timetype, :li_timeamt ;
		loop
		
	close intspc_cur ;
end if


try
	this.of_setsrvdwxmlie( true )
	astr_lcreqloan.xml_lcreqloan		= inv_dwxmliesrv.of_xmlexport( lds_reqmain )
	astr_lcreqloan.xml_lccontcoll		= ""
	astr_lcreqloan.xml_lccontclr			= inv_dwxmliesrv.of_xmlexport( lds_reqclr )
	astr_lcreqloan.xml_lccontclroth		= inv_dwxmliesrv.of_xmlexport( lds_reqclroth )
	astr_lcreqloan.xml_lcintspc			= inv_dwxmliesrv.of_xmlexport( lds_reqintspc )
	this.of_setsrvdwxmlie( false )
catch( Exception lthw_errexp )
	ithw_exception.text	= lthw_errexp.text
	lb_error					= true
end try

if isvalid( lds_reqmain ) then destroy lds_reqmain
if isvalid( lds_reqclr ) then destroy lds_reqclr
if isvalid( lds_reqclroth ) then destroy lds_reqclroth
if isvalid( lds_reqintspc ) then destroy lds_reqintspc

if lb_error then
	throw ithw_exception
end if

return 1
end function

public function integer of_init_lcapvloan (ref str_lcreqloan astr_lcreqloan) throws Exception;string		ls_coopid, ls_lnreqno
integer	li_apvstatus
boolean	lb_error = false
n_ds		lds_apvloan, lds_reqintspc

ls_coopid			= astr_lcreqloan.coop_id
ls_lnreqno		= astr_lcreqloan.loanrequest_docno

lds_apvloan		= create n_ds
lds_apvloan.dataobject	= "d_lcsrv_loanapv"
lds_apvloan.settransobject( itr_sqlca )

lds_apvloan.retrieve( ls_coopid, ls_lnreqno )

if lds_apvloan.rowcount() <= 0 then
	ithw_exception.text	= "ไม่พบข้อมูลใบคำขอกู้เงินเลขที่ "+ls_lnreqno+" กรุณาตรวจสอบ"
	destroy lds_apvloan
	throw ithw_exception
end if

li_apvstatus		= lds_apvloan.getitemnumber( 1, "loanrequest_status" )

choose case li_apvstatus
	case 1
		ithw_exception.text	= "ใบคำขอกู้เงินเลขที่ "+ls_lnreqno+" มีสถานะเป็นอนุมัติแล้ว ไม่สามารถทำรายการได้อีก กรุณาตรวจสอบ"
		destroy lds_apvloan
		throw ithw_exception
	case 0
		ithw_exception.text	= "ใบคำขอกู้เงินเลขที่ "+ls_lnreqno+" มีสถานะเป็นไม่อนุมัติแล้ว ไม่สามารถทำรายการได้อีก กรุณาตรวจสอบ"
		destroy lds_apvloan
		throw ithw_exception
end choose

lds_reqintspc	= create n_ds
lds_reqintspc.dataobject	= "d_lcsrv_loanreq_intspc"
lds_reqintspc.settransobject( itr_sqlca )

try
	this.of_setsrvdwxmlie( true )
	astr_lcreqloan.xml_lcreqloan		= inv_dwxmliesrv.of_xmlexport( lds_apvloan )
	astr_lcreqloan.xml_lcintspc			= inv_dwxmliesrv.of_xmlexport( lds_reqintspc )
	this.of_setsrvdwxmlie( false )
catch( Exception lthw_errexp )
	ithw_exception.text	= "ใบคำขอกู้เงินเลขที่ "+ls_lnreqno+" ไม่สามารถส่งข้อมูลมาแสดงที่หน้าจอได้ กรุณาตรวจสอบ"
	lb_error					= true
end try

destroy lds_apvloan
destroy lds_reqintspc

if lb_error then
	throw ithw_exception
end if

return 1
end function

public function string of_gennewcontractno (string as_coopid, string as_loantype) throws Exception;/***********************************************************
<description>
	สำหรับขอเลขที่สัญญาใหม่ของเงินกู้แต่ละประเภทที่ส่งเข้ามา
</description>

<arguments>  
	as_loantype		String		ประเภทเงินกู้ที่ขอเลขที่สัญญาใหม่
</arguments> 

<return> 
	String			เลขที่สัญญาใหม่ที่ระบบสร้างให้
	Exception	ถ้ามีข้อผิดพลาด
</return> 

<usage> 
	เรียกใช้โดยส่งประเภทเงินกู้ที่จะออกเลขสัญญาใหม่เข้ามา
	ระบบจะทำการสร้างเลขสัญญาให้แล้ว Return ค่ากลับไป
	
	string		ls_loantype, ls_newcontno
	
	ls_loantype		= dw_example.getitemstring( 1, "loantype_code" )
	
	ls_newcontno	= lnv_lnoperate.of_gennewcontractno( ls_loantype )	
	
	----------------------------------------------------------------------
	Revision History:
	Version 1.0 (Initial version) Modified Date 20/9/2010 by OhhO

</usage> 
***********************************************************/
string		ls_contno, ls_prefix, ls_doccode

// ดึงค่าเลขเอกสารจาก config
select		prefix, document_code
into		:ls_prefix, :ls_doccode
from		lccfloantype
where	( loantype_code	= :as_loantype ) and
			( coop_id				= :as_coopid )
using		itr_sqlca ;

if ls_doccode = "" or isnull( ls_doccode ) then
	ithw_exception.text	= "เงินกู้ประเภทนี้ไม่มีเลขที่เอกสารสำหรับออกเลขสัญญา กรุณาไปกำหนดด้วย"
	throw ithw_exception
end if

this.of_setsrvdoccontrol( true )

ls_contno	= inv_docsrv.of_getnewdocno( as_coopid, ls_doccode, ls_prefix )

this.of_setsrvdoccontrol( false )

return ls_contno
end function

private function integer of_buildcontno_reqloan (string as_coopid, string as_lnreqno, ref string as_lncontno) throws Exception;string			ls_loantype

// argument check
if isnull( as_lnreqno ) then as_lnreqno = ""
if isnull( as_lncontno ) then as_lncontno = ""

// ตรวจสอบใบคำขอ
if as_lnreqno = "" or as_lnreqno = "CNV" then
	ithw_exception.text	= "เลขที่ใบคำขอไม่มีส่งมา หรือ เลขที่ส่งมาไม่ถูกต้อง "+as_lnreqno
	throw ithw_exception
end if

// ถ้าไม่มีเลขที่สัญญาส่งมาสร้างเอาใหม่
if trim( as_lncontno ) = "" then
	// หาประเภทเงินกู้
	select		loantype_code
	into		:ls_loantype
	from		lcreqloan
	where	( loanrequest_docno	= :as_lnreqno ) and
				( coop_id					= :as_coopid )
	using		itr_sqlca ;
	
	as_lncontno	= this.of_gennewcontractno( as_coopid, ls_loantype )

	// ใส่ค่าเลขที่สัญญากลับไปที่ใบคำขอ
	update	lcreqloan
	set			concoop_id			= :as_coopid,	
				loancontract_no	= :as_lncontno
	where	( loanrequest_docno	= :as_lnreqno ) and
				( coop_id					= :as_coopid )
	using		itr_sqlca ;

	if itr_sqlca.sqlcode <> 0 then
		ithw_exception.text	= "ไม่สามารถใส่เลขที่สัญญาลงไปทีใบคำขอได้ "+itr_sqlca.sqlerrtext
		throw ithw_exception
	end if
end if

insert into lccontmaster
		( loancontract_no,			coop_id,						member_no,				memcoop_id,				loantype_code,			loanobjective_code,
		  loanrequest_docno,		loanrequest_amt,			loanapprove_date,			loanapprove_amt,			withdrawable_amt,	principal_balance,
		  loanpayment_type,		loanpayment_percent,	loanpayment_fixamt,		period_installment,		period_payment,		period_lastpayment,		payment_status,
		  od_flag,					contlaw_status,				contsign_status,			contplace_status,			contract_status,		remark,
		  int_continttype,			int_contintrate,				int_continttabcode,		int_contintincrease,		int_intsteptype,
		  mthdue_type,				mthdue_fixdate,			mthdue_holidaytype )		  
select	  :as_lncontno,				coop_id,						member_no,				memcoop_id,				loantype_code,			loanobjective_code,
		  loanrequest_docno,		loanrequest_amt,			loanapprove_date,			loanapprove_amt,			loanapprove_amt,		0,
		  loanpayment_type,		loanpayment_percent,	loanpayment_fixamt,		period_installment,		period_payment,		period_lastpayment,		1,
		  od_flag,					1,								0,								0,								1,							remark,						
		  int_continttype,			int_contintrate,				int_continttabcode,		int_contintincrease,		int_intsteptype,
		  mthdue_type,				mthdue_fixdate,			mthdue_holidaytype
from		lcreqloan
where	( loanrequest_docno	= :as_lnreqno ) and
			( coop_id					= :as_coopid )
using		itr_sqlca ;

if itr_sqlca.sqlcode <> 0 then
	ithw_exception.text	= "ไม่สามารถผ่านรายการใบคำขอไปสร้างเป็นสัญญาได้ "+itr_sqlca.sqlerrtext
	throw ithw_exception
end if

// ถ้าเป็นสัญญาที่คิดดอกเบี้ยเป็นช่วง
insert into lccontintspc
			( loancontract_no, 	coop_id,		seq_no, 		int_continttype, 		int_contintrate, 		int_continttabcode,		 int_contintincrease	, int_timetype,		 int_timeamt )
select		:as_lncontno, 			coop_id,		seq_no, 		int_continttype, 		int_contintrate, 		int_continttabcode, 		 int_contintincrease	, int_timetype,		 int_timeamt
from		lcreqloanintspc
where	( loanrequest_docno	= :as_lnreqno )and
			( coop_id					= :as_coopid )
using		itr_sqlca ;

if itr_sqlca.sqlcode <> 0 then
	ithw_exception.text	= "ไม่สามารถผ่านรายการใบคำขอไปสร้างเป็นสัญญาได้ (ส่วนของอัตราดอกเบี้ยพิเศษเป็นช่วง) "+itr_sqlca.sqlerrtext
	throw ithw_exception
end if

// รายการค้ำประกัน
insert into lccontcoll
			( loancontract_no,		coop_id,				seq_no,			loancolltype_code,		ref_collno,		description,
			coll_amt,					coll_percent,		coll_status,		base_percent )
select 	:as_lncontno,			coop_id,				seq_no,			loancolltype_code,		ref_collno,		description,
			coll_amt,					coll_percent,		1,					base_percent
from		lcreqloancoll
where	( lcreqloancoll.loanrequest_docno	= :as_lnreqno )and
			( lcreqloancoll.coop_id				= :as_coopid )
using		itr_sqlca ;

if itr_sqlca.sqlcode <> 0 then
	ithw_exception.text	= "ไม่สามารถผ่านรายการหลักประกันในใบคำขอไปสร้างเป็นหลักประกันของสัญญาได้ "+itr_sqlca.sqlerrtext
	throw ithw_exception
end if

return 1
end function

public function boolean of_checkcontadjustdet (n_ds ads_data, string as_oldcol[], string as_newcol[]);string		ls_coltype
any		la_bfdata, la_atdata
integer	li_colcount, li_colno
boolean	lb_adjust
datetime	ldtm_bfdate, ldtm_atdate

lb_adjust			= false
li_colcount		= upperbound( as_newcol )

for li_colno = 1 to li_colcount
	ls_coltype	= ads_data.describe( as_oldcol[ li_colno ]+".coltype" )
	
	if ls_coltype = "date" or ls_coltype = "datetime" then
		ldtm_bfdate	= ads_data.getitemdatetime( 1, as_oldcol[ li_colno ] )
		ldtm_atdate	= ads_data.getitemdatetime( 1, as_newcol[ li_colno ] )
		
		if isnull( ldtm_bfdate ) then ldtm_bfdate = datetime( date( 1900, 1, 1 ) )
		if isnull( ldtm_atdate ) then ldtm_atdate = datetime( date( 1900, 1, 1 ) )
		
		if ldtm_bfdate <> ldtm_atdate then
			lb_adjust	= true
			exit
		end if
	else
		la_bfdata		= ads_data.of_getitemany( 1, as_oldcol[ li_colno ] )
		la_atdata		= ads_data.of_getitemany( 1, as_newcol[ li_colno ] )
		
		if la_bfdata <> la_atdata then
			lb_adjust	= true
			exit
		end if
	end if
	
next

return lb_adjust
end function

public function integer of_init_reqcontadjust (ref str_lccontaj astr_lccontaj) throws Exception;string		ls_concoopid, ls_contno, ls_memno, ls_mcoopid
string		ls_prename, ls_mname, ls_suffname
long		ll_count, ll_row, ll_index
integer	li_continttype
datetime	ldtm_contaj
n_ds		lds_infocont, lds_temp, lds_colllist

ls_concoopid	= astr_lccontaj.contcoop_id
ls_contno		= astr_lccontaj.loancontract_no
ldtm_contaj		= astr_lccontaj.contaj_date

astr_lccontaj.xml_adjmast			= ""
astr_lccontaj.xml_adjpayment		= ""
astr_lccontaj.xml_adjcoll				= ""
astr_lccontaj.xml_adjint				= ""
astr_lccontaj.xml_adjintspc			= ""

// รายละเอียดสัญญาที่จะทำการเปลี่ยนแปลงแก้ไข
lds_infocont		= create n_ds
lds_infocont.dataobject	= "d_lcsrv_info_contract"
lds_infocont.settransobject( itr_sqlca )

ll_count			= lds_infocont.retrieve( ls_concoopid, ls_contno )
if ll_count <= 0 then
	ithw_exception.text	= "ไม่พบข้อมูลเสัญญาที่ส่งเข้ามา สัญญาเลขที่ '"+ls_contno + "' กรุณาตรวจสอบ"
	destroy lds_infocont
	throw ithw_exception
end if

// ชื่อสมาชิก
ls_mcoopid		= lds_infocont.getitemstring( ll_count, "memcoop_id" )
ls_memno		= lds_infocont.getitemstring( ll_count, "member_no" )

select mbucfprename.prename_desc,
		mbmembmaster.memb_name,
		mbucfprename.suffname_desc
into	:ls_prename, :ls_mname, :ls_suffname
from	mbmembmaster, mbucfprename
where	( mbmembmaster.prename_code = mbucfprename.prename_code )
and		( mbmembmaster.coop_id		= :ls_mcoopid )
and		( mbmembmaster.member_no		= :ls_memno )
using		itr_sqlca ;

this.of_setsrvdwxmlie( true )

lds_temp		= create n_ds

// รายละเอียดสัญญาที่ทำการแก้ไข
lds_temp.dataobject	= DWCONTADJ
ll_row			= lds_temp.insertrow( 0 )

lds_temp.setitem( ll_row, "concoop_id", ls_concoopid )
lds_temp.setitem( ll_row, "loancontract_no", ls_contno )

lds_temp.setitem( ll_row, "contadjust_date", ldtm_contaj )
lds_temp.setitem( ll_row, "contadjust_status", 1 )

lds_temp.setitem( ll_row, "prename_desc", ls_prename )
lds_temp.setitem( ll_row, "memb_name", ls_mname )
lds_temp.setitem( ll_row, "suffname_desc", ls_suffname )

lds_temp.object.memcoop_id[ ll_row ]			= lds_infocont.object.memcoop_id[ 1 ]
lds_temp.object.member_no[ ll_row ]			= lds_infocont.object.member_no[ 1 ]
lds_temp.object.loantype_code[ ll_row ]			= lds_infocont.object.loantype_code[ 1 ]
lds_temp.object.loantype_desc[ ll_row ]			= lds_infocont.object.loantype_desc[ 1 ]
lds_temp.object.loantype_prefix[ ll_row ]		= lds_infocont.object.loantype_prefix[ 1 ]
lds_temp.object.loanapprove_amt[ ll_row ]		= lds_infocont.object.loanapprove_amt[ 1 ]
lds_temp.object.contsign_amt[ ll_row ]			= lds_infocont.object.contsign_amt[ 1 ]
lds_temp.object.principal_balance[ ll_row ]		= lds_infocont.object.principal_balance[ 1 ]
lds_temp.object.all_periodpay[ ll_row ]			= lds_infocont.object.period_installment[ 1 ]
lds_temp.object.last_periodpay[ ll_row ]			= lds_infocont.object.last_periodpay[ 1 ]

astr_lccontaj.xml_adjmast		= inv_dwxmliesrv.of_xmlexport( lds_temp )

// เงื่อนไขการชำระประจำเดือน
lds_temp.dataobject	= DWCONTADJPAY
ll_row			= lds_temp.insertrow( 0 )

lds_temp.object.contadjust_code[ ll_row ]			= "PAY"
lds_temp.object.loanpayment_type[ ll_row ]		= lds_infocont.object.loanpayment_type[ 1 ]
lds_temp.object.period_installment[ ll_row ]		= lds_infocont.object.period_installment[ 1 ]
lds_temp.object.period_payment[ ll_row ]			= lds_infocont.object.period_payment[ 1 ]
lds_temp.object.period_paypercent[ ll_row ]		= lds_infocont.object.period_paypercent[ 1 ]
lds_temp.object.payment_status[ ll_row ]			= lds_infocont.object.payment_status[ 1 ]
lds_temp.object.oldpayment_type[ ll_row ]			= lds_infocont.object.loanpayment_type[ 1 ]
lds_temp.object.oldperiod_installment[ ll_row ]	= lds_infocont.object.period_installment[ 1 ]
lds_temp.object.oldperiod_payment[ ll_row ]		= lds_infocont.object.period_payment[ 1 ]
lds_temp.object.oldperiod_paypercent[ ll_row ]	= lds_infocont.object.period_paypercent[ 1 ]
lds_temp.object.oldpayment_status[ ll_row ]		= lds_infocont.object.payment_status[ 1 ]

astr_lccontaj.xml_adjpayment		= inv_dwxmliesrv.of_xmlexport( lds_temp )

// เงื่อนไขการคิด ด/บ
lds_temp.dataobject	= DWCONTADJINT
ll_row			= lds_temp.insertrow( 0 )

lds_temp.object.contadjust_code[ ll_row ]			= "INT"
lds_temp.object.int_continttype[ ll_row ]				= lds_infocont.object.int_continttype[ 1 ]
lds_temp.object.int_contintrate[ ll_row ]				= lds_infocont.object.int_contintrate[ 1 ]
lds_temp.object.int_continttabcode[ ll_row ]		= lds_infocont.object.int_continttabcode[ 1 ]
lds_temp.object.int_contintincrease[ ll_row ]		= lds_infocont.object.int_contintincrease[ 1 ]
lds_temp.object.int_intsteptype[ ll_row ]				= lds_infocont.object.int_intsteptype[ 1 ]

lds_temp.object.oldint_continttype[ ll_row ]			= lds_infocont.object.int_continttype[ 1 ]
lds_temp.object.oldint_contintrate[ ll_row ]			= lds_infocont.object.int_contintrate[ 1 ]
lds_temp.object.oldint_continttabcode[ ll_row ]		= lds_infocont.object.int_continttabcode[ 1 ]
lds_temp.object.oldint_contintincrease[ ll_row ]	= lds_infocont.object.int_contintincrease[ 1 ]
lds_temp.object.oldint_intsteptype[ ll_row ]			= lds_infocont.object.int_intsteptype[ 1 ]

astr_lccontaj.xml_adjint		= inv_dwxmliesrv.of_xmlexport( lds_temp )

// ถ้าเป็นดอกเบี้ยอัตราพิเศษเป็นช่วง
li_continttype	= lds_infocont.getitemnumber( 1, "int_continttype" )
if li_continttype = 3 then
	datastore	lds_contintspc
	
	lds_contintspc	= create datastore
	lds_contintspc.dataobject	= "d_lcsrv_info_contintspcall"
	lds_contintspc.settransobject( itr_sqlca )
	lds_contintspc.retrieve( ls_concoopid, ls_contno )
	
	lds_temp.dataobject	= DWCONTADJINTSPC

	for ll_index = 1 to lds_contintspc.rowcount()
		ll_row			= lds_temp.insertrow( 0 )
		
		lds_temp.object.contadjust_type[ ll_row ]		= "NEW"
		lds_temp.object.seq_no[ ll_row ]					= ll_index
		lds_temp.object.int_continttype[ ll_row ]			= lds_contintspc.object.int_continttype[ ll_index ]
		lds_temp.object.int_contintrate[ ll_row ]			= lds_contintspc.object.int_contintrate[ ll_index ]
		lds_temp.object.int_continttabcode[ ll_row ]	= lds_contintspc.object.int_continttabcode[ ll_index ]
		lds_temp.object.int_contintincrease[ ll_row ]	= lds_contintspc.object.int_contintincrease[ ll_index ]
		lds_temp.object.effective_date[ ll_row ]			= lds_contintspc.object.effective_date[ ll_index ]
		lds_temp.object.expire_date[ ll_row ]				= lds_contintspc.object.expire_date[ ll_index ]
	next
	
	astr_lccontaj.xml_adjintspc	= inv_dwxmliesrv.of_xmlexport( lds_temp )
	
	destroy lds_contintspc

end if

// รายการค้ำประกัน
lds_temp.dataobject	= DWCONTADJCOLL

lds_colllist	= create n_ds
lds_colllist.dataobject	= "d_lcsrv_info_listcoll"
lds_colllist.settransobject( itr_sqlca )

ll_count		= lds_colllist.retrieve( ls_concoopid, ls_contno )

for ll_index = 1 to ll_count
	ll_row			= lds_temp.insertrow( 0 )
	
	lds_temp.object.contadjust_type[ ll_row ]	= "NEW"
	lds_temp.object.seq_no[ ll_row ]				= ll_index
	lds_temp.object.loancolltype_code[ ll_row ]	= lds_colllist.object.loancolltype_code[ ll_index ]
	lds_temp.object.ref_collno[ ll_row ]			= lds_colllist.object.ref_collno[ ll_index ]
	lds_temp.object.description[ ll_row ]			= lds_colllist.object.description[ ll_index ]
	lds_temp.object.coll_amt[ ll_row ]				= lds_colllist.object.coll_amt[ ll_index ]
	lds_temp.object.coll_percent[ ll_row ]		= lds_colllist.object.coll_percent[ ll_index ]
	lds_temp.object.base_percent[ ll_row ]		= lds_colllist.object.base_percent[ ll_index ]
	
next

astr_lccontaj.xml_adjcoll	= inv_dwxmliesrv.of_xmlexport( lds_temp )

if isvalid( lds_infocont ) then destroy lds_infocont
if isvalid( lds_temp ) then destroy lds_temp
if isvalid( lds_colllist ) then destroy lds_colllist
this.of_setsrvdwxmlie( false )

return 1
end function

public function boolean of_checkcontadjustdetrow (n_ds ads_old, n_ds ads_new, string as_col[]);string		ls_coltype
any		la_bfdata, la_atdata
integer	li_colcount, li_colno
long		ll_index, ll_count
boolean	lb_adjust
datetime	ldtm_bfdate, ldtm_atdate

if ads_old.rowcount() <> ads_new.rowcount() then
	return true
end if

lb_adjust			= false
li_colcount		= upperbound( as_col )
ll_count			= ads_old.rowcount()

// ตรวจทีละแถว
for ll_index = 1 to ll_count
	
	// ตรวจสอบทีละ column ที่ระบุมา
	for li_colno = 1 to li_colcount
		ls_coltype	= ads_old.describe( as_col[ li_colno ]+".coltype" )
		
		if ls_coltype = "date" or ls_coltype = "datetime" then
			ldtm_bfdate	= ads_old.getitemdatetime( ll_index, as_col[ li_colno ] )
			ldtm_atdate	= ads_new.getitemdatetime( ll_index, as_col[ li_colno ] )
			
			if isnull( ldtm_bfdate ) then ldtm_bfdate = datetime( date( 1900, 1, 1 ) )
			if isnull( ldtm_atdate ) then ldtm_atdate = datetime( date( 1900, 1, 1 ) )
			
			if ldtm_bfdate <> ldtm_atdate then
				lb_adjust	= true
				exit
			end if
		else
			la_bfdata		= ads_old.of_getitemany( ll_index, as_col[ li_colno ] )
			la_atdata		= ads_new.of_getitemany( ll_index, as_col[ li_colno ] )
			
			if la_bfdata <> la_atdata then
				lb_adjust	= true
				exit
			end if
		end if
	next
next


return lb_adjust
end function

private function integer of_setlncontadjdata (str_lccontaj astr_lccontaj, ref n_ds ads_adjpay, ref n_ds ads_adjint, ref n_ds ads_adjintspc, ref n_ds ads_adjcoll, ref n_ds ads_adjintspcold, ref n_ds ads_adjcollold) throws Exception;long		ll_index

// รายการชำระรายเดือน
ads_adjpay		= create n_ds
ads_adjpay.dataobject	= DWCONTADJPAY
ads_adjpay.settransobject( itr_sqlca )

inv_dwxmliesrv.of_xmlimport( ads_adjpay, astr_lccontaj.xml_adjpayment )

// รายการวิธีคิดดอกเบี้ย
ads_adjint		= create n_ds
ads_adjint.dataobject	= DWCONTADJINT
ads_adjint.settransobject( itr_sqlca )

inv_dwxmliesrv.of_xmlimport( ads_adjint, astr_lccontaj.xml_adjint )

// รายการวิธีคิดดอกเบี้ยอัตราพิเศษ
ads_adjintspc		= create n_ds
ads_adjintspc.dataobject	= DWCONTADJINTSPC
ads_adjintspc.settransobject( itr_sqlca )

inv_dwxmliesrv.of_xmlimport( ads_adjintspc, astr_lccontaj.xml_adjintspc )
ads_adjintspc.setsort( "effective_date asc" )
ads_adjintspc.sort()

// รายการวิธีคิดดอกเบี้ยอัตราพิเศษ<ก่อนเปลี่ยนแปลง>
ads_adjintspcold		= create n_ds
ads_adjintspcold.dataobject	= DWCONTADJINTSPC
ads_adjintspcold.settransobject( itr_sqlca )

inv_dwxmliesrv.of_xmlimport( ads_adjintspcold, astr_lccontaj.xml_oldadjintspc )
ads_adjintspcold.setsort( "effective_date asc" )
ads_adjintspcold.sort()
for ll_index = 1 to ads_adjintspcold.rowcount()
	ads_adjintspcold.setitem( ll_index, "contadjust_type", "OLD" )
next

// รายการการค้ำประกัน
ads_adjcoll		= create n_ds
ads_adjcoll.dataobject	= DWCONTADJCOLL
ads_adjcoll.settransobject( itr_sqlca )

inv_dwxmliesrv.of_xmlimport( ads_adjcoll, astr_lccontaj.xml_adjcoll )
ads_adjcoll.setsort( "loancolltype_code asc, ref_collno asc" )
ads_adjcoll.sort()

// รายการการค้ำประกัน<ก่อนเปลี่ยนแปลง>
ads_adjcollold		= create n_ds
ads_adjcollold.dataobject	= DWCONTADJCOLL
ads_adjcollold.settransobject( itr_sqlca )

inv_dwxmliesrv.of_xmlimport( ads_adjcollold, astr_lccontaj.xml_oldadjcoll )
ads_adjcollold.setsort( "loancolltype_code asc, ref_collno asc" )
ads_adjcollold.sort()
for ll_index = 1 to ads_adjcollold.rowcount()
	ads_adjcollold.setitem( ll_index, "contadjust_type", "OLD" )
next

return 1
end function

private function integer of_setlncontadjdocno (string as_ecoopid, string as_entryid, ref n_ds ads_adjmast, ref n_ds ads_adjpay, ref n_ds ads_adjint, ref n_ds ads_adjintspc, ref n_ds ads_adjcoll, ref n_ds ads_adjintspcold, ref n_ds ads_adjcollold) throws Exception;string		ls_contajdocno
long		ll_index
datetime	ldtm_entrydate

ldtm_entrydate	= datetime( today(), now() )

// ส่วน Master ของการเปลี่ยนแปลงรายละเอียดสัญญา
this.of_setsrvdoccontrol( true )
ls_contajdocno		= inv_docsrv.of_getnewdocno( as_ecoopid, "LCCONTADJDOCNO" )
this.of_setsrvdoccontrol( false )

ads_adjmast.setitem( 1, "coop_id", is_coopcontrol )
ads_adjmast.setitem( 1, "contadjust_docno", ls_contajdocno )
ads_adjmast.setitem( 1, "entry_id", as_entryid )
ads_adjmast.setitem( 1, "entry_date", ldtm_entrydate )
ads_adjmast.setitem( 1, "entry_bycoopid", as_ecoopid )

// การเปลี่ยนแปลงการชำระประจำเดือน
ads_adjpay.setitem( 1, "coop_id", is_coopcontrol )
ads_adjpay.setitem( 1, "contadjust_docno", ls_contajdocno )

// การเปลี่ยนแปลงอัตราด/บของสัญญา
ads_adjint.setitem( 1, "coop_id", is_coopcontrol )
ads_adjint.setitem( 1, "contadjust_docno", ls_contajdocno )

// การเปลี่ยนแปลงอัตราด/บของสัญญาพิเศษ
for ll_index = 1 to ads_adjintspc.rowcount()
	ads_adjintspc.setitem( ll_index, "coop_id", is_coopcontrol )
	ads_adjintspc.setitem( ll_index, "contadjust_docno", ls_contajdocno )
	ads_adjintspc.setitem( ll_index, "seq_no", ll_index )
next

// การเปลี่ยนแปลงอัตราด/บของสัญญาพิเศษ<ก่อนเปลี่ยนแปลง>
for ll_index = 1 to ads_adjintspcold.rowcount()
	ads_adjintspcold.setitem( ll_index, "coop_id", is_coopcontrol )
	ads_adjintspcold.setitem( ll_index, "contadjust_docno", ls_contajdocno )
	ads_adjintspcold.setitem( ll_index, "seq_no", ll_index )
next

// การเปลี่ยนแปลงการค้ำประกัน
for ll_index = 1 to ads_adjcoll.rowcount()
	ads_adjcoll.setitem( ll_index, "coop_id", is_coopcontrol )
	ads_adjcoll.setitem( ll_index, "contadjust_docno", ls_contajdocno )
	ads_adjcoll.setitem( ll_index, "seq_no", ll_index )
next

// การเปลี่ยนแปลงการค้ำประกัน<ก่อนเปลี่ยนแปลง>
for ll_index = 1 to ads_adjcollold.rowcount()
	ads_adjcollold.setitem( ll_index, "coop_id", is_coopcontrol )
	ads_adjcollold.setitem( ll_index, "contadjust_docno", ls_contajdocno )
	ads_adjcollold.setitem( ll_index, "seq_no", ll_index )
next

return 1
end function

private function integer of_post_reqcontadj_pay (n_ds ads_adjmast, n_ds ads_adjpay) throws Exception;string		ls_contno
integer	li_paytype, li_periodall, li_paystatus
dec{2}	ldc_payment
dec{5}	ldc_paypercent

ls_contno		= ads_adjmast.getitemstring( 1, "loancontract_no" )

li_paytype		= ads_adjpay.getitemnumber( 1, "loanpayment_type" )
li_periodall		= ads_adjpay.getitemnumber( 1, "period_installment" )
li_paystatus		= ads_adjpay.getitemnumber( 1, "payment_status" )
ldc_payment	= ads_adjpay.getitemdecimal( 1, "period_payment" )
ldc_paypercent	= ads_adjpay.getitemdecimal( 1, "period_paypercent" )

update	lccontmaster
set			loanpayment_type		= :li_paytype,
			period_installment		= :li_periodall,
			period_payment		= :ldc_payment,
			period_paypercent		= :ldc_paypercent,
			payment_status		= :li_paystatus
where	( loancontract_no		= :ls_contno )
using		itr_sqlca ;

if itr_sqlca.sqlcode <> 0 then
	ithw_exception.text	= "ไม่สามารถปรับปรุงรายละเอียดการชำระรายเดือนได้ สัญญา #"+ls_contno+"~n~r"+itr_sqlca.sqlerrtext
	throw ithw_exception
end if
	
return 1
end function

private function integer of_post_reqcontadj_int (n_ds ads_adjmast, n_ds ads_adjint, n_ds ads_adjintspc) throws Exception;string		ls_ccoopid, ls_contno, ls_continttab
integer	li_continttype, li_intsteptype
long		ll_index
dec{4}	ldc_contintrate, ldc_contintinc
datetime	ldtm_effdate, ldtm_expdate

ls_ccoopid		= ads_adjmast.getitemstring( 1, "concoop_id" )
ls_contno		= ads_adjmast.getitemstring( 1, "loancontract_no" )

li_continttype		= ads_adjint.getitemnumber( 1, "int_continttype" )
ldc_contintrate		= ads_adjint.getitemdecimal( 1, "int_contintrate" )
ls_continttab		= ads_adjint.getitemstring( 1, "int_continttabcode" )
ldc_contintinc		= ads_adjint.getitemdecimal( 1, "int_contintincrease" )
li_intsteptype		= ads_adjint.getitemnumber( 1, "int_intsteptype" )

update	lccontmaster
set			int_continttype			= :li_continttype,
			int_contintrate			= :ldc_contintrate,
			int_continttabcode		= :ls_continttab,
			int_contintincrease		= :ldc_contintinc,
			int_intsteptype			= :li_intsteptype
where	( coop_id				= :ls_ccoopid ) and
			( loancontract_no		= :ls_contno )
using		itr_sqlca ;

if itr_sqlca.sqlcode <> 0 then
	ithw_exception.text	= "ไม่สามารถปรับปรุงรายละเอียดอัตรา ด/บ ได้ สัญญา #"+ls_contno+"~n~r"+itr_sqlca.sqlerrtext
	throw ithw_exception
end if

// ส่วนของ ด/บ พิเศษเป็นช่วง
if ads_adjintspc.rowcount() > 0 then
	// ข้อมูลเก่าลบทิ้งไป
	delete from lccontintspc
	where	( coop_id				= :ls_ccoopid ) and
				( loancontract_no		= :ls_contno )
	using		itr_sqlca ;
	
	if itr_sqlca.sqlcode <> 0 then
		ithw_exception.text	= "ไม่สามารถลบข้อมูลอัตรา ด/บ พิเศษเป็นช่วงได้ สัญญา #"+ls_contno+"~n~r"+itr_sqlca.sqlerrtext
		throw ithw_exception
	end if
	
	for ll_index = 1 to ads_adjintspc.rowcount()
		li_continttype		= ads_adjintspc.getitemnumber( 1, "int_continttype" )
		ldc_contintrate		= ads_adjintspc.getitemdecimal( 1, "int_contintrate" )
		ls_continttab		= ads_adjintspc.getitemstring( 1, "int_continttabcode" )
		ldc_contintinc		= ads_adjintspc.getitemdecimal( 1, "int_contintincrease" )
		ldtm_effdate		= ads_adjintspc.getitemdatetime( 1, "effective_date" )
		ldtm_expdate		= ads_adjintspc.getitemdatetime( 1, "expire_date" )
		
		insert into lccontintspc
					( coop_id, loancontract_no, seq_no, effective_date, expire_date, int_continttype, int_contintrate, int_continttabcode, int_contintincrease )
		values	( :ls_ccoopid, :ls_contno, :ll_index, :ldtm_effdate, :ldtm_expdate, :li_continttype, :ldc_contintrate, :ls_continttab, :ldc_contintinc )
		using		itr_sqlca ;
		
		if itr_sqlca.sqlcode <> 0 then
			ithw_exception.text	= "ไม่สามารถเพิ่มข้อมูลใหม่อัตรา ด/บ พิเศษเป็นช่วงได้ สัญญา #"+ls_contno+"~n~r"+itr_sqlca.sqlerrtext
			throw ithw_exception
		end if
	next
end if

return 1
end function

private function integer of_post_reqcontadj_coll (n_ds ads_adjmast, n_ds ads_adjcoll) throws Exception;string		ls_ccoopid, ls_contno
string		ls_colltype, ls_refcollno, ls_colldesc
long		ll_index
dec{2}	ldc_collamt, ldc_collpercent, ldc_basepercent

ls_ccoopid		= ads_adjmast.getitemstring( 1, "concoop_id" )
ls_contno		= ads_adjmast.getitemstring( 1, "loancontract_no" )

// ลบข้อมูลเก่าทิ้งก่อน
delete from lccontcoll
where	( coop_id			= :ls_ccoopid ) and
			( loancontract_no	= :ls_contno )
using		itr_sqlca ;
if itr_sqlca.sqlcode <> 0 then
	ithw_exception.text	= "ไม่สามารถลบข้อมูลหลักประกันเก่าได้ สัญญา #"+ls_contno+"~n~r"+itr_sqlca.sqlerrtext
	throw ithw_exception
end if

for ll_index = 1 to ads_adjcoll.rowcount()

	ls_colltype			= ads_adjcoll.object.loancolltype_code[ ll_index ]
	ls_refcollno			= ads_adjcoll.object.ref_collno[ ll_index ]
	ls_colldesc			= ads_adjcoll.object.description[ ll_index ]
	ldc_collamt			= ads_adjcoll.object.coll_amt[ ll_index ]
	ldc_collpercent		= ads_adjcoll.object.coll_percent[ ll_index ]
	ldc_basepercent	= ads_adjcoll.object.base_percent[ ll_index ]
	
	insert into lccontcoll
				( coop_id, loancontract_no, seq_no, loancolltype_code, ref_collno, description, coll_amt, coll_percent, base_percent, coll_status )
	values	( :ls_ccoopid, :ls_contno, :ll_index, :ls_colltype, :ls_refcollno, :ls_colldesc, :ldc_collamt, :ldc_collpercent, :ldc_basepercent, 1 )
	using		itr_sqlca ;
	if itr_sqlca.sqlcode <> 0 then
		ithw_exception.text	= "ไม่สามารถเพิ่มข้อมูลใหม่หลักประกันเงินกู้ได้ สัญญา #"+ls_contno+"~n~r"+itr_sqlca.sqlerrtext
		throw ithw_exception
	end if
next

return 1
end function

public function string of_getmembdetail (string as_coopid, string as_memno);string		ls_xmlmemdet
long		ll_count
n_ds		lds_infomemdet

// initial info memdetail
lds_infomemdet = create n_ds
lds_infomemdet.dataobject = "d_lcsrv_memdet"
lds_infomemdet.settransobject( itr_sqlca )

ls_xmlmemdet		= ""

ll_count	= lds_infomemdet.retrieve( as_coopid, as_memno )

if ll_count <= 0 then
	destroy lds_infomemdet
	return ls_xmlmemdet
end if

ls_xmlmemdet		= string( lds_infomemdet.describe( "Datawindow.data.XML" ) )

destroy lds_infomemdet

return ls_xmlmemdet
end function

public function integer of_open_reqloan (ref str_lcreqloan astr_lcreqloan) throws Exception;string		ls_coopid, ls_reqdocno
boolean	lb_error = false
n_ds		lds_reqmain, lds_reqcoll, lds_reqclr, lds_reqclroth, lds_reqintspc

ls_coopid			= astr_lcreqloan.coop_id
ls_reqdocno		= astr_lcreqloan.loanrequest_docno

lds_reqmain		= create n_ds
lds_reqmain.dataobject	= "d_lcsrv_loanreq"
lds_reqmain.settransobject( itr_sqlca )
lds_reqmain.retrieve( ls_coopid, ls_reqdocno )

if lds_reqmain.rowcount() <= 0 then
	ithw_exception.text	= "ไม่สามารถดึงข้อมูลใบคำขอเลขที่ "+ls_reqdocno+" ("+ls_coopid+") กรุณาตรวจสอบ"
	destroy lds_reqmain
	throw ithw_exception
end if

lds_reqcoll		= create n_ds
lds_reqcoll.dataobject	= "d_lcsrv_loanreq_coll"
lds_reqcoll.settransobject( itr_sqlca )
lds_reqcoll.retrieve( ls_coopid, ls_reqdocno )

lds_reqclr		= create n_ds
lds_reqclr.dataobject	= "d_lcsrv_loanreq_clr"
lds_reqclr.settransobject( itr_sqlca )
lds_reqclr.retrieve( ls_coopid, ls_reqdocno )

lds_reqclroth	= create n_ds
lds_reqclroth.dataobject	= "d_lcsrv_loanreq_clroth"
lds_reqclroth.settransobject( itr_sqlca )
lds_reqclroth.retrieve( ls_coopid, ls_reqdocno )

lds_reqintspc	= create n_ds
lds_reqintspc.dataobject	= "d_lcsrv_loanreq_intspc"
lds_reqintspc.settransobject( itr_sqlca )
lds_reqintspc.retrieve( ls_coopid, ls_reqdocno )

try
	this.of_setsrvdwxmlie( true )
	astr_lcreqloan.xml_lcreqloan		= inv_dwxmliesrv.of_xmlexport( lds_reqmain )
	astr_lcreqloan.xml_lccontcoll		= inv_dwxmliesrv.of_xmlexport( lds_reqcoll )
	astr_lcreqloan.xml_lccontclr			= inv_dwxmliesrv.of_xmlexport( lds_reqclr )
	astr_lcreqloan.xml_lccontclroth		= inv_dwxmliesrv.of_xmlexport( lds_reqclroth )
	astr_lcreqloan.xml_lcintspc			= inv_dwxmliesrv.of_xmlexport( lds_reqintspc )
catch( Exception lthw_errexp )
	ithw_exception.text	= lthw_errexp.text
	lb_error					= true
end try

if isvalid( lds_reqmain ) then destroy lds_reqmain
if isvalid( lds_reqcoll ) then destroy lds_reqcoll
if isvalid( lds_reqclr ) then destroy lds_reqclr
if isvalid( lds_reqclroth ) then destroy lds_reqclroth
if isvalid( lds_reqintspc ) then destroy lds_reqintspc
this.of_setsrvdwxmlie( false )

if lb_error then
	throw ithw_exception
end if

return 1
end function

public function integer of_init_reqcontsign (ref str_lccontsign astr_lccontsign) throws Exception;string		ls_ccoopid, ls_contno
string		ls_memno, ls_memname, ls_lntypecode, ls_lntypedesc, ls_lntypeprefix
integer	li_contsignstatus
long		ll_count, ll_row
dec{2}	ldc_lnreqamt, ldc_lnapvamt
datetime	ldtm_lnapvdate
n_ds		lds_contsign

ls_ccoopid		= astr_lccontsign.concoop_id
ls_contno		= astr_lccontsign.loancontract_no

select		lccontmaster.member_no,
			mbucfprename.prename_desc||mbmembmaster.memb_name||'  '||mbucfprename.suffname_desc,
			lccontmaster.loantype_code,
			lccfloantype.loantype_desc,
			lccfloantype.prefix,
			lccontmaster.loanrequest_amt,
			lccontmaster.loanapprove_date,
			lccontmaster.loanapprove_amt,
			lccontmaster.contsign_status
into		:ls_memno, :ls_memname, :ls_lntypecode, :ls_lntypedesc, :ls_lntypeprefix, :ldc_lnreqamt, :ldtm_lnapvdate, :ldc_lnapvamt, :li_contsignstatus
from		lccontmaster, mbmembmaster, mbucfprename, lccfloantype
where	( lccontmaster.coop_id = :ls_ccoopid ) and  
			( lccontmaster.loancontract_no = :ls_contno ) and
			( mbmembmaster.prename_code = mbucfprename.prename_code ) and  
			( lccontmaster.memcoop_id = mbmembmaster.coop_id ) and  
			( lccontmaster.member_no = mbmembmaster.member_no ) and  
			( lccontmaster.coop_id = lccfloantype.coop_id ) and  
			( lccontmaster.loantype_code = lccfloantype.loantype_code )
using		itr_sqlca ;

if itr_sqlca.sqlcode <> 0 then
	ithw_exception.text	= "ไม่พบข้อมูลสัญญาเลขที่ "+ls_contno+" ("+ls_ccoopid+") กรุณาตรวจสอบ"
	throw ithw_exception
end if

if li_contsignstatus = 1 then
	ithw_exception.text	= "สัญญาเลขที่ "+ls_contno+" ("+ls_ccoopid+") ได้มีการเซ็นต์สัญญาไปแล้ว ไม่สามารถบันทึกการเซ็นต์สัญญาได้อีก กรุณาตรวจสอบ"
	throw ithw_exception
end if

lds_contsign		= create n_ds
lds_contsign.dataobject		= "d_lcsrv_contsign"

ll_row			= lds_contsign.insertrow( 0 )

lds_contsign.setitem( ll_row, "member_no", ls_memno )
lds_contsign.setitem( ll_row, "memb_name", ls_memname )
lds_contsign.setitem( ll_row, "loantype_code", ls_lntypecode )
lds_contsign.setitem( ll_row, "loantype_desc", ls_lntypedesc )
lds_contsign.setitem( ll_row, "prefix", ls_lntypeprefix )
lds_contsign.setitem( ll_row, "loanrequest_amt", ldc_lnreqamt )
lds_contsign.setitem( ll_row, "loanapprove_amt", ldc_lnapvamt )
lds_contsign.setitem( ll_row, "loanapprove_date", ldtm_lnapvdate )

lds_contsign.setitem( ll_row, "concoop_id", ls_ccoopid )
lds_contsign.setitem( ll_row, "loancontract_no", ls_contno )

try
	this.of_setsrvdwxmlie( true )
	astr_lccontsign.xml_contract	= inv_dwxmliesrv.of_xmlexport( lds_contsign )
	this.of_setsrvdwxmlie( false )
catch( Exception lthw_errexp )
	destroy	lds_contsign
	throw lthw_errexp
end try

destroy lds_contsign

return 1
end function

public function integer of_save_reqcontsign (ref str_lccontsign astr_lccontsign) throws Exception;string		ls_ccoopid, ls_contno, ls_contsigndocno
long		ll_count
dec{2}	ldc_contsignamt
datetime	ldtm_contsign
boolean	lb_error = false
n_ds		lds_contsign

string		ls_entryid, ls_ecoopid
datetime	ldtm_entrydate

ls_entryid		= astr_lccontsign.entry_id
ls_ecoopid		= astr_lccontsign.entry_bycoopid
ldtm_entrydate	= datetime( today(), now() )

lds_contsign		= create n_ds
lds_contsign.dataobject		= "d_lcsrv_contsign"

// Import ข้อมูล
try
	this.of_setsrvdwxmlie( true )
	inv_dwxmliesrv.of_xmlimport( lds_contsign, astr_lccontsign.xml_contract )

catch( Exception lthw_errimp )
	ithw_exception.text	= lthw_errimp.text
	lb_error					= true
end try

if lb_error then goto objdestroy

ll_count	= lds_contsign.rowcount()
if ll_count <= 0 then
	ithw_exception.text	= "ไม่มีข้อมูลการเซ็นต์สัญญาเงินกู้สำหรับการบันทึก กรุณาตรวจสอบ"
	lb_error					= true
	goto objdestroy
end if

// ขอเลขที่เอกสาร
this.of_setsrvdoccontrol( true )

try
	ls_contsigndocno		= inv_docsrv.of_getnewdocno( is_coopcontrol, "LCREGCONTSIGN" )
catch( Exception lthw_errdoc )
	ithw_exception.text	= lthw_errdoc.text
	lb_error					= true
end try

if lb_error then goto objdestroy

// ให้ค่าเลขที่เอกสาร
lds_contsign.setitem( 1, "coop_id", is_coopcontrol )
lds_contsign.setitem( 1, "contsign_docno", ls_contsigndocno )
lds_contsign.setitem( 1, "entry_id", ls_entryid )
lds_contsign.setitem( 1, "entry_bycoopid", ls_ecoopid )
lds_contsign.setitem( 1, "entry_date", ldtm_entrydate )

// -----------------------
// เริ่มกระบวนการบันทึก
// -----------------------
if lds_contsign.update() <> 1 then
	ithw_exception.text	= "บันทึกข้อมูลการเซ็นต์สัญญา (ContSignature) ไม่ได้  "
	ithw_exception.text	+= ls_contsigndocno +"  "+ lds_contsign.of_geterrormessage()
	lb_error					= false
	goto objdestroy
end if

// ผ่านรายการไปปรับปรุงสัญญา
ls_ccoopid			= lds_contsign.getitemstring( 1, "coop_id" )
ls_contno			= lds_contsign.getitemstring( 1, "loancontract_no" )
ldc_contsignamt	= lds_contsign.getitemdecimal( 1, "contsign_amt" )
ldtm_contsign		= lds_contsign.getitemdatetime( 1, "contsign_date" )

update	lccontmaster
set			withdrawable_amt	= :ldc_contsignamt,	
			contsign_amt		= :ldc_contsignamt,
			contsign_date		= :ldtm_contsign,
			contsign_status		= 1
where	( coop_id			= :ls_ccoopid ) and
			( loancontract_no	= :ls_contno )
using		itr_sqlca ;

if itr_sqlca.sqlcode <> 0 then
	ithw_exception.text	= "ไม่สามารถปรับปรุงรายละเอียดบันทึกการเซ็นต์สัญญาได้ เลขที่สัญญา "+ls_contno+" ("+ls_ccoopid+") "+itr_sqlca.sqlerrtext
	lb_error				= false
	goto objdestroy
end if

objdestroy:
this.of_setsrvdwxmlie( false )
destroy lds_contsign

if lb_error then
	rollback using itr_sqlca ;
	throw ithw_exception
end if

commit using itr_sqlca ;

return 1
end function

public function integer of_init_reqcontplanrcv (ref str_lccontplanrcv astr_lccontplanrcv) throws Exception;string		ls_ccoopid, ls_contno
long		ll_row
boolean	lb_error = false
n_ds		lds_continfo, lds_contplanrcv

ls_ccoopid		= astr_lccontplanrcv.concoop_id
ls_contno		= astr_lccontplanrcv.loancontract_no

lds_continfo		= create n_ds
lds_continfo.dataobject	= "d_lcsrv_detail_contract"
lds_continfo.settransobject( itr_sqlca )

lds_contplanrcv	= create n_ds
lds_contplanrcv.dataobject	= "d_lcsrv_contplanrcv"
lds_contplanrcv.settransobject( itr_sqlca )

ll_row		= lds_continfo.retrieve( ls_ccoopid, ls_contno )

if ll_row <= 0 then
	ithw_exception.text	= "ไม่พบข้อมูลสัญญาเงินกู้เลขที่ "+ls_contno+" ("+ls_ccoopid+") กรุณาตรวจสอบ"
	lb_error					= true
	goto objdestroy
end if

lds_contplanrcv.retrieve( ls_ccoopid, ls_contno )

try
	this.of_setsrvdwxmlie( true )
	astr_lccontplanrcv.xml_continfo		= inv_dwxmliesrv.of_xmlexport( lds_continfo )
	astr_lccontplanrcv.xml_planlist		= inv_dwxmliesrv.of_xmlexport( lds_contplanrcv )
catch( Exception lthw_errexp )
	ithw_exception.text	= lthw_errexp.text
	lb_error					= true
end try

if lb_error then goto objdestroy

objdestroy:
destroy lds_continfo
destroy lds_contplanrcv
this.of_setsrvdwxmlie( false )

if lb_error then
	throw ithw_exception
end if


return 1
end function

public function integer of_save_reqcontplan (ref str_lccontplanrcv astr_lccontplanrcv) throws Exception;string		ls_ccoopid, ls_contno
string		ls_entryid, ls_ecoopid
long		ll_row
boolean	lb_error = false
datetime	ldtm_entrydate
n_ds		lds_contplanrcv

ls_ccoopid		= astr_lccontplanrcv.concoop_id
ls_contno		= astr_lccontplanrcv.loancontract_no

ls_entryid		= astr_lccontplanrcv.entry_id
ls_ecoopid		= astr_lccontplanrcv.entry_bycoopid
ldtm_entrydate	= datetime( today(), now() )

lds_contplanrcv	= create n_ds
lds_contplanrcv.dataobject	= "d_lcsrv_contplanrcv"
lds_contplanrcv.settransobject( itr_sqlca )

// Import ข้อมูล
try
	this.of_setsrvdwxmlie( true )
	inv_dwxmliesrv.of_xmlimport( lds_contplanrcv, astr_lccontplanrcv.xml_planlist )

catch( Exception lthw_errimp )
	ithw_exception.text	= lthw_errimp.text
	lb_error					= true
end try

if lb_error then goto objdestroy

for ll_row = 1 to lds_contplanrcv.rowcount()
	lds_contplanrcv.setitem( ll_row, "coop_id", ls_ccoopid )
	lds_contplanrcv.setitem( ll_row, "loancontract_no", ls_contno )
	lds_contplanrcv.setitem( ll_row, "seq_no", ll_row )
next

// ลบข้อมูลเก่าทิ้งเสมอ
delete from lccontplanrecv
where	( coop_id			= :ls_ccoopid ) and
			( loancontract_no	= :ls_contno )
using		itr_sqlca ;

// -----------------------
// เริ่มกระบวนการบันทึก
// -----------------------
if lds_contplanrcv.update() <> 1 then
	ithw_exception.text	= "บันทึกข้อมูลแผนการรับเงินกู้ (lccontplanrecv) ไม่ได้  "
	ithw_exception.text	+= lds_contplanrcv.of_geterrormessage()
	lb_error					= false
	goto objdestroy
end if

objdestroy:
destroy lds_contplanrcv
this.of_setsrvdwxmlie( false )

if lb_error then
	rollback using itr_sqlca ;
	throw ithw_exception
end if

commit using itr_sqlca ;

return 1
end function

public function integer of_save_lcapvloan (ref str_lcreqloan astr_lcapvloan) throws Exception;string			ls_coopid, ls_lnreqno, ls_lncontno, ls_apvid, ls_apvbycoopid
string			ls_loantype, ls_statusdesc
integer		li_continttype, li_apvstatus
long			ll_index
dec{2}		ldc_lnapvamt
datetime		ldtm_apvdate
boolean		lb_error
n_ds			lds_apvdata, lds_reqintspc

ls_apvid			= astr_lcapvloan.approve_id
ls_apvbycoopid	= astr_lcapvloan.approve_bycoopid
ldtm_apvdate	= astr_lcapvloan.approve_date

lds_apvdata		= create n_ds
lds_apvdata.dataobject		= "d_lcsrv_loanapv"
lds_apvdata.settransobject( itr_sqlca )

lds_reqintspc	= create n_ds
lds_reqintspc.dataobject	= "d_lcsrv_loanreq_intspc"
lds_reqintspc.settransobject( itr_sqlca )

lb_error			= false

this.of_setsrvdwxmlie( true )
try
	inv_dwxmliesrv.of_xmlimport( lds_apvdata, astr_lcapvloan.xml_lcreqloan )
	inv_dwxmliesrv.of_xmlimport( lds_reqintspc, astr_lcapvloan.xml_lcintspc )
catch( Exception lthw_imperr )
	ithw_exception.text	= lthw_imperr.text
	lb_error					= true
end try

if lb_error then goto objdestroy

// ตรวจสอบใบคำขอ
if lds_apvdata.rowcount() <= 0 then
	ithw_exception.text	= "ไม่มีข้อมูลใบคำขอกู้ที่จะทำการอนุมัติ"
	lb_error					= true
	goto objdestroy
end if

ls_coopid			= lds_apvdata.getitemstring( 1, "coop_id" )
ls_lnreqno		= lds_apvdata.getitemstring( 1, "loanrequest_docno" )
ls_loantype		= lds_apvdata.getitemstring( 1, "loantype_code" )
ls_lncontno		= lds_apvdata.getitemstring( 1, "loancontract_no" )
li_apvstatus		= lds_apvdata.getitemnumber( 1, "loanrequest_status" )
li_continttype	= lds_apvdata.getitemnumber( 1, "int_continttype" )
ldc_lnapvamt	= lds_apvdata.getitemdecimal( 1, "loanapprove_amt" )

choose case li_apvstatus
	case 0
		ls_lncontno		= ""
		ldc_lnapvamt	= 0
		
	case 1
		if ldc_lnapvamt <= 0 then
			ithw_exception.text	= "ใบคำขอกู้เงิน เลขที่ "+ls_lnreqno+" กรุณาระบุยอดเงินอนุมัติด้วย "
			lb_error					= true
			goto objdestroy
		end if
		
		// ถ้าไม่มีเลขสัญญาต้องสร้างให้
		if ls_lncontno = "" or isnull( ls_lncontno ) then
			ls_lncontno	= this.of_gennewcontractno( ls_coopid, ls_loantype )
		end if
		
	case else
		ithw_exception.text	= "ใบคำขอกู้เงิน เลขที่ "+ls_lnreqno+" กรุณาระบุสถานะการอนุมัติด้วย "
		lb_error					= true
		goto objdestroy
		
end choose

// ปรับสถานะให้เป็น Update ทั้งแถว
this.of_setdsmodify( lds_apvdata, 1, false )

// บันทึกกลับไปที่ใบคำขอกู้ก่อน เพราะเวลาสร้างสัญญาจะต้องไปดึงข้อมูลจากใบคำขอมาสร้าง
lds_apvdata.setitem( 1, "concoop_id", ls_coopid )
lds_apvdata.setitem( 1, "loancontract_no", ls_lncontno )
lds_apvdata.setitem( 1, "approve_id", ls_apvid )
lds_apvdata.setitem( 1, "approve_bycoopid", ls_apvbycoopid )
lds_apvdata.setitem( 1, "approve_date", ldtm_apvdate )

if lds_apvdata.update() <> 1 then
	ithw_exception.text	+= "ใบคำขอกู้เงิน เลขที่ "+ls_lnreqno+" ไม่สามารถบันทึกการอนุมัติได้ "+lds_apvdata.of_geterrormessage()
	lb_error					= true
	goto objdestroy
end if

// บันทึกในส่วนดอกเบี้ยพิเศษเป็นช่วง
delete from lcreqloanintspc where ( coop_id = :ls_coopid and loanrequest_docno = :ls_lnreqno )
using		itr_sqlca ;

// ใส่เลขเอกสารให้ table ดอกเบี้ยพิเศษ
for ll_index = 1 to lds_reqintspc.rowcount()
	lds_reqintspc.setitem( ll_index, "coop_id", ls_coopid )
	lds_reqintspc.setitem( ll_index, "loanrequest_docno", ls_lnreqno )
	lds_reqintspc.setitem( ll_index, "seq_no", ll_index )
next

if lds_reqintspc.update() <> 1 then
	ithw_exception.text	+= "อนุมัติคำขอกู้เงินเลขที่ "+ls_lnreqno+" ไม่สามารถบันทึกรายการดอกเบี้ยอัตราพิเศษได้ "+lds_reqintspc.of_geterrormessage()
	lb_error					= true
	goto objdestroy
end if

// ถ้าอนุมัติสร้างเลขสัญญาเลย
if li_apvstatus = 1 then
	// สร้างสัญญาเงินกู้
	try
		this.of_buildcontno_reqloan( ls_coopid, ls_lnreqno, ls_lncontno )
	catch ( Exception lthw_buildcontno )
		ithw_exception.text	= lthw_buildcontno.text
		lb_error					= true
	end try
	
	astr_lcapvloan.loancontract_no		= ls_lncontno
end if

objdestroy:
destroy lds_apvdata
destroy lds_reqintspc

if lb_error then
	rollback using itr_sqlca ;
	throw ithw_exception
end if

commit using itr_sqlca ;

return 1
end function

public function integer of_save_reqchgcontplace (ref str_lccontplace astr_lccontplace) throws Exception;string		ls_coopid, ls_contplcdocno, ls_ccoopid, ls_contno
string		ls_entryid, ls_ecoopid
long		ll_index, ll_count
integer	li_place
boolean	lb_error
datetime	ldtm_entrydate
n_ds		lds_master, lds_detail

ls_coopid			= is_coopcontrol
ls_ecoopid		= astr_lccontplace.entry_bycoopid
ls_entryid		= astr_lccontplace.entry_id
ldtm_entrydate	= datetime( today(), now() )

lds_master	= create n_ds
lds_master.dataobject	= "d_lcsrv_req_chgcontplace"
lds_master.settransobject( itr_sqlca )

lds_detail	= create n_ds
lds_detail.dataobject		= "d_lcsrv_req_chgcontplacedet"
lds_detail.settransobject( itr_sqlca )

lb_error			= false

this.of_setsrvdwxmlie( true )
try
	inv_dwxmliesrv.of_xmlimport( lds_master, astr_lccontplace.xml_contplace )
	inv_dwxmliesrv.of_xmlimport( lds_detail, astr_lccontplace.xml_contplacedet )
catch( Exception lthw_imperr )
	ithw_exception.text	= lthw_imperr.text
	lb_error					= true
end try

if lb_error then goto objdestroy

// ตรวจว่ามีการทำรายการหรือเปล่า
ll_count	= lds_detail.rowcount()
if ll_count <= 0 then
	ithw_exception.text	= "ไม่มีรายการสัญญาที่จะทำรายการ กรุณาตรวจสอบ"
	lb_error					= true
	goto objdestroy
end if

// ขอเลขที่เอกสาร
this.of_setsrvdoccontrol( true )

try
	ls_contplcdocno		= inv_docsrv.of_getnewdocno( ls_coopid, "LCCHGCONTPLACE" )
catch( Exception lthw_errdoc )
	ithw_exception.text	= lthw_errdoc.text
	lb_error					= true
end try

if lb_error then goto objdestroy

// ให้ค่าเลขที่เอกสาร
lds_master.setitem( 1, "coop_id", ls_coopid )
lds_master.setitem( 1, "chgcontplace_docno", ls_contplcdocno )
lds_master.setitem( 1, "entry_id", ls_entryid )
lds_master.setitem( 1, "entry_bycoopid", ls_ecoopid )
lds_master.setitem( 1, "entry_date", ldtm_entrydate )

// ใส่เลขที่เอกสารและลำดับที่ใน detail
for ll_index = 1 to ll_count
	lds_detail.setitem( ll_index, "coop_id", ls_coopid )
	lds_detail.setitem( ll_index, "chgcontplace_docno", ls_contplcdocno )
	lds_detail.setitem( ll_index, "concoop_id", ls_coopid )
next

// -----------------------
// เริ่มกระบวนการบันทึก
// -----------------------
// บันทึก Master
if lds_master.update() <> 1 then
	ithw_exception.text	= "บันทึกข้อมูลใบขอนำสัญญาเข้า/ออก (ChgContplace Master) ไม่ได้  "
	ithw_exception.text	+= ls_contplcdocno +"  "+ lds_master.of_geterrormessage()
	lb_error					= true
	goto objdestroy
end if

// บันทึก Detail
if lds_detail.update() <> 1 then
	ithw_exception.text	= "บันทึกข้อมูลรายละเอียดสัญญาที่จะนำเข้า/ออก (ChgContplace Detail) ไม่ได้  "
	ithw_exception.text	+= ls_contplcdocno +"  "+ lds_detail.of_geterrormessage()
	lb_error					= true
	goto objdestroy
end if

li_place		= lds_master.getitemnumber( 1, "chgcontplace_type" )

// ปรับปรุงสถานะของที่อยู่ของสัญญา
for ll_index = 1 to ll_count
	ls_ccoopid	= lds_detail.getitemstring( ll_index, "concoop_id" )
	ls_contno	= lds_detail.getitemstring( ll_index, "loancontract_no" )
	
	update	lccontmaster
	set			contplace_status	= :li_place
	where	( coop_id			= :ls_ccoopid ) and
				( loancontract_no	= :ls_contno )
	using		itr_sqlca ;
	
	if itr_sqlca.sqlcode <> 0 then
		ithw_exception.text	= "ผิดพลาด!!! ไม่สามารถปรับปรุงสถานะที่อยู่ของสัญญา "+ls_contno+" ได้ "+itr_sqlca.sqlerrtext
		lb_error					= true
		goto objdestroy
	end if
next

objdestroy:
destroy lds_master
destroy lds_detail
this.of_setsrvdwxmlie( true )
this.of_setsrvdoccontrol( false )

if lb_error then
	rollback using itr_sqlca ;
	throw ithw_exception
end if

commit using itr_sqlca ;

// ส่งกลับสำหรับนำไปพิมพ์
astr_lccontplace.chgcontplace_docno		= ls_contplcdocno

return 1
end function

public function integer of_save_lcreqloan (ref str_lcreqloan astr_lcreqloan) throws Exception;string		ls_rqlndocno, ls_coopid, ls_entryid, ls_entrycoopid
long		ll_index
boolean	lb_error
datetime	ldtm_entrydate
n_ds		lds_reqmain, lds_reqcoll, lds_reqclr, lds_reqclroth, lds_reqintspc

lds_reqmain		= create n_ds
lds_reqmain.dataobject	= "d_lcsrv_loanreq"
lds_reqmain.settransobject( itr_sqlca )

lds_reqcoll		= create n_ds
lds_reqcoll.dataobject	= "d_lcsrv_loanreq_coll"
lds_reqcoll.settransobject( itr_sqlca )

lds_reqclr		= create n_ds
lds_reqclr.dataobject	= "d_lcsrv_loanreq_clr"
lds_reqclr.settransobject( itr_sqlca )

lds_reqclroth	= create n_ds
lds_reqclroth.dataobject	= "d_lcsrv_loanreq_clroth"
lds_reqclroth.settransobject( itr_sqlca )

lds_reqintspc	= create n_ds
lds_reqintspc.dataobject	= "d_lcsrv_loanreq_intspc"
lds_reqintspc.settransobject( itr_sqlca )

lb_error			= false

this.of_setsrvdwxmlie( true )
try
	inv_dwxmliesrv.of_xmlimport( lds_reqmain, astr_lcreqloan.xml_lcreqloan )
	inv_dwxmliesrv.of_xmlimport( lds_reqcoll, astr_lcreqloan.xml_lccontcoll )
	inv_dwxmliesrv.of_xmlimport( lds_reqclr, astr_lcreqloan.xml_lccontclr )
	inv_dwxmliesrv.of_xmlimport( lds_reqclroth, astr_lcreqloan.xml_lccontclroth )
	inv_dwxmliesrv.of_xmlimport( lds_reqintspc, astr_lcreqloan.xml_lcintspc )
catch( Exception lthw_imperr )
	ithw_exception.text	= lthw_imperr.text
	lb_error					= true
end try

if lb_error then goto objdestroy

// สาขาที่จะใส่ใน PK ใช้สาขาคุม สาขาที่บันทึกใช้ที่ UI ส่งเข้ามา
ls_coopid			= is_coopcontrol
ls_entrycoopid	= astr_lcreqloan.entry_bycoopid

ls_entryid		= astr_lcreqloan.entry_id
ldtm_entrydate	= datetime( today(), now() )

// ตรวจดูจากเลขที่เอกสารว่าเป็นการบันทึกใหม่หรือแก้ไข
ls_rqlndocno	= trim( lds_reqmain.getitemstring( 1, "loanrequest_docno" ) )

if ls_rqlndocno = "" or isnull( ls_rqlndocno ) or upper( ls_rqlndocno ) = "AUTO" then
	// ขอเลขที่เอกสาร
	this.of_setsrvdoccontrol( true )
	
	try
		ls_rqlndocno		= inv_docsrv.of_getnewdocno( ls_coopid, "LCREQLOAN" )
	catch( Exception lthw_errdoc )
		ithw_exception.text	= lthw_errdoc.text
		lb_error					= true
	end try
	
	if lb_error then goto objdestroy
	
	lds_reqmain.setitem( 1, "coop_id", ls_coopid )
	lds_reqmain.setitem( 1, "loanrequest_docno", ls_rqlndocno )
	lds_reqmain.setitem( 1, "loanrequest_status", 8 )
	lds_reqmain.setitem( 1, "entry_id", ls_entryid )
	lds_reqmain.setitem( 1, "entry_bycoopid", ls_entrycoopid )
	lds_reqmain.setitem( 1, "entry_date", ldtm_entrydate )
	
else
	// เป็นการแก้ไขใบคำขอ
	this.of_setdsmodify( lds_reqmain, 1, false )

	// ลบข้อมูลรายละเอียดทั้งหมดเพื่อรอบันทึกใหม่
	delete from lcreqloanclr where ( coop_id = :ls_coopid and loanrequest_docno = :ls_rqlndocno )
	using		itr_sqlca ;
	
	delete from lcreqloanclrother where ( coop_id = :ls_coopid and loanrequest_docno = :ls_rqlndocno )
	using		itr_sqlca ;

	delete from lcreqloancoll where ( coop_id = :ls_coopid and loanrequest_docno = :ls_rqlndocno )
	using		itr_sqlca ;
	
	delete from lcreqloanintspc where ( coop_id = :ls_coopid and loanrequest_docno = :ls_rqlndocno )
	using		itr_sqlca ;

	delete from lcreqloancontsignlist where ( coop_id = :ls_coopid and loanrequest_docno = :ls_rqlndocno )
	using		itr_sqlca ;
end if

// ใส่เลขเอกสารให้ table คนค้ำ
for ll_index = 1 to lds_reqcoll.rowcount()
	lds_reqcoll.setitem( ll_index, "coop_id", ls_coopid )
	lds_reqcoll.setitem( ll_index, "loanrequest_docno", ls_rqlndocno )
	lds_reqcoll.setitem( ll_index, "seq_no", ll_index )
next

// ใส่เลขเอกสารให้ table สัญญาที่ต้อง Clear
for ll_index = 1 to lds_reqclr.rowcount()
	lds_reqclr.setitem( ll_index, "coop_id", ls_coopid )
	lds_reqclr.setitem( ll_index, "loanrequest_docno", ls_rqlndocno )
	lds_reqclr.setitem( ll_index, "seq_no", ll_index )
next

// ใส่เลขเอกสารให้ table การหักอื่นๆ
for ll_index = 1 to lds_reqclroth.rowcount()
	lds_reqclroth.setitem( ll_index, "coop_id", ls_coopid )
	lds_reqclroth.setitem( ll_index, "loanrequest_docno", ls_rqlndocno )
	lds_reqclroth.setitem( ll_index, "seq_no", ll_index )
next

// ใส่เลขเอกสารให้ table ดอกเบี้ยพิเศษ
for ll_index = 1 to lds_reqintspc.rowcount()
	lds_reqintspc.setitem( ll_index, "coop_id", ls_coopid )
	lds_reqintspc.setitem( ll_index, "loanrequest_docno", ls_rqlndocno )
	lds_reqintspc.setitem( ll_index, "seq_no", ll_index )
next

// บันทึกข้อมูลลง DB
if lds_reqmain.update() <> 1 then
	ithw_exception.text	= "ไม่สามารถบันทึกข้อมูลคำขอกู้ได้ (Main) "+lds_reqmain.of_geterrormessage()
	lb_error					= false
	goto objdestroy
end if

if lds_reqcoll.update() <> 1 then
	ithw_exception.text	= "ไม่สามารถบันทึกข้อมูลคำขอกู้ได้ (หลักประกัน) "+lds_reqcoll.of_geterrormessage()
	lb_error					= false
	goto objdestroy
end if

if lds_reqclr.update() <> 1 then
	ithw_exception.text	= "ไม่สามารถบันทึกข้อมูลคำขอกู้ได้ (หนี้เก่า) "+lds_reqclr.of_geterrormessage()
	lb_error					= false
	goto objdestroy
end if

if lds_reqclroth.update() <> 1 then
	ithw_exception.text	= "ไม่สามารถบันทึกข้อมูลคำขอกู้ได้ (รายการหักอื่นๆ) "+lds_reqclroth.of_geterrormessage()
	lb_error					= false
	goto objdestroy
end if

if lds_reqintspc.update() <> 1 then
	ithw_exception.text	= "ไม่สามารถบันทึกข้อมูลคำขอกู้ได้ (ดอกเบี้ยอัตราพิเศษ) "+lds_reqintspc.of_geterrormessage()
	lb_error					= false
	goto objdestroy
end if

objdestroy:
this.of_setsrvdwxmlie( false )
this.of_setsrvdoccontrol( false )
if isvalid( lds_reqmain ) then destroy lds_reqmain
if isvalid( lds_reqcoll ) then destroy lds_reqcoll
if isvalid( lds_reqclr ) then destroy lds_reqclr
if isvalid( lds_reqclroth ) then destroy lds_reqclroth
if isvalid( lds_reqintspc ) then destroy lds_reqintspc

if lb_error then
	rollback using itr_sqlca ;
	throw ithw_exception
end if

commit using itr_sqlca ;

// ส่งเลขที่ใบคำขอกลับ เผื่อนำไปทำรายการ
astr_lcreqloan.loanrequest_docno		= ls_rqlndocno

return 1
end function

public function integer of_save_reqcontadjust (ref str_lccontaj astr_lccontaj) throws Exception;string		ls_contajdocno, ls_oldcol[], ls_newcol[]
string		ls_entryid, ls_ecoopid
boolean	lb_chgpay, lb_chgint, lb_chgcoll, lb_error
n_ds		lds_adjmast, lds_adjpay, lds_adjint, lds_adjintspc, lds_adjcoll, lds_adjintspcold, lds_adjcollold

ls_entryid		= astr_lccontaj.entry_id
ls_ecoopid		= astr_lccontaj.entry_bycoopid

lb_chgpay	= false
lb_chgint		= false
lb_chgcoll	= false
lb_error		= false

this.of_setsrvdwxmlie( true )

// สร้าง DS ใบคำขอ
lds_adjmast		= create n_ds
lds_adjmast.dataobject	= DWCONTADJ
lds_adjmast.settransobject( itr_sqlca )

inv_dwxmliesrv.of_xmlimport( lds_adjmast, astr_lccontaj.xml_adjmast )

// สร้าง DS ชุดการเปลี่ยนแปลง
this.of_setlncontadjdata( astr_lccontaj, lds_adjpay, lds_adjint, lds_adjintspc, lds_adjcoll, lds_adjintspcold, lds_adjcollold )

// ตรวจสอบการเปลี่ยนแปลงการชำระ
ls_newcol	= { "loanpayment_type", "period_installment", "period_payment", "payment_status" }
ls_oldcol		= { "oldpayment_type", "oldperiod_installment", "oldperiod_payment", "oldpayment_status" }
lb_chgpay	= this.of_checkcontadjustdet( lds_adjpay, ls_newcol, ls_oldcol )

// ตรวจสอบการเปลี่ยนแปลงอัตรา ด/บ
ls_newcol	= { "int_continttype", "int_contintrate", "int_continttabcode", "int_contintincrease", "int_intsteptype" }
ls_oldcol		= { "oldint_continttype", "oldint_contintrate", "oldint_continttabcode", "oldint_contintincrease", "oldint_intsteptype" }
lb_chgint		= this.of_checkcontadjustdet( lds_adjint, ls_newcol, ls_oldcol )

// อัตรา ดอกเบี้ยพิเศษ
if lb_chgint = false then
	ls_newcol	= { "effective_date", "expire_date", "int_continttype", "int_continttabcode", "int_contintincrease", "int_contintrate" }
	lb_chgint		= this.of_checkcontadjustdetrow( lds_adjintspcold, lds_adjintspc, ls_newcol )
end if

// ตรวจสอบการเปลี่ยนแปลงหลักประกัน
ls_newcol	= { "loancolltype_code", "ref_collno", "description", "coll_percent" }
lb_chgcoll	= this.of_checkcontadjustdetrow( lds_adjcoll, lds_adjcollold, ls_newcol )

// ถ้าไม่มีการเปลี่ยนแปลงอะไรเลย
if lb_chgpay = false and lb_chgint = false and lb_chgcoll = false then
	ithw_exception.text	= "ไม่พบว่ามีการการเปลี่ยนแปลงข้อมูลสัญญา กรุณาตรวจสอบ"
	lb_error					= true
	goto objdestroy
end if

// ส่งไปสร้างเลขที่เอกสาร
this.of_setlncontadjdocno( ls_ecoopid, ls_entryid, lds_adjmast, lds_adjpay, lds_adjint, lds_adjintspc, lds_adjcoll, lds_adjintspcold, lds_adjcollold )

ls_contajdocno		= lds_adjmast.getitemstring( 1, "contadjust_docno" )

if lds_adjmast.update() <> 1 then
	ithw_exception.text	= "บันทึกข้อมูลการเปลี่ยนแปลงรายละเอียดสัญญา (ContractAdjust) ไม่ได้  "
	ithw_exception.text	+= ls_contajdocno +"  "+ lds_adjmast.of_geterrormessage()
	lb_error					= true
	goto objdestroy
end if

// การเปลี่ยนแปลงการชำระประจำเดือน
if lb_chgpay then
	if lds_adjpay.update() <> 1 then
		ithw_exception.text	= "บันทึกข้อมูลการเปลี่ยนแปลงรายละเอียดสัญญาในส่วนการชำระประจำเดือน (Contract Payment) ไม่ได้  "
		ithw_exception.text	+= ls_contajdocno +"  "+ lds_adjpay.of_geterrormessage()
		lb_error					= true
		goto objdestroy
	end if
end if

// การเปลี่ยนแปลงอัตราด/บของสัญญา
if lb_chgint then
	if lds_adjint.update() <> 1 then
		ithw_exception.text	= "บันทึกข้อมูลการเปลี่ยนแปลงรายละเอียดสัญญาในส่วนการคิดอัตรา ด/บ (Contract Interest) ไม่ได้  "
		ithw_exception.text	+= ls_contajdocno +"  "+ lds_adjint.of_geterrormessage()
		lb_error					= true
		goto objdestroy
	end if
	
	// อัตรา ด/บ พิเศษเป็นช่วง
	if lds_adjintspc.rowcount() > 0 then
		if lds_adjintspc.update() <> 1 then
			ithw_exception.text	= "บันทึกข้อมูลการเปลี่ยนแปลงรายละเอียดสัญญาในส่วนอัตรา ด/บ พิเศษเป็นช่วง (Contract IntSpecial) ไม่ได้  "
			ithw_exception.text	+= ls_contajdocno +"  "+ lds_adjintspc.of_geterrormessage()
			lb_error					= true
			goto objdestroy
		end if
	end if
	
	// อัตรา ด/บ พิเศษเป็นช่วง<ก่อนเปลี่ยนแปลง>
	if lds_adjintspcold.rowcount() > 0 then
		if lds_adjintspc.update() <> 1 then
			ithw_exception.text	= "บันทึกข้อมูลการเปลี่ยนแปลงรายละเอียดสัญญาในส่วนอัตรา ด/บ พิเศษเป็นช่วง (Contract IntSpecial Old) ไม่ได้  "
			ithw_exception.text	+= ls_contajdocno +"  "+ lds_adjintspcold.of_geterrormessage()
			lb_error					= true
			goto objdestroy
		end if
	end if
end if

// การเปลี่ยนแปลงรายละเอียดการค้ำประกัน
if lb_chgcoll then
	if lds_adjcoll.rowcount() > 0 then
		if lds_adjcoll.update() <> 1 then
			ithw_exception.text	= "บันทึกข้อมูลการเปลี่ยนแปลงรายละเอียดสัญญาในส่วนการค้ำประกันไม่ได้(Contract Coll) ไม่ได้  "
			ithw_exception.text	+= ls_contajdocno +"  "+ lds_adjcoll.of_geterrormessage()
			lb_error					= true
			goto objdestroy
		end if
	end if
	
	if lds_adjcollold.rowcount() > 0 then
		if lds_adjcollold.update() <> 1 then
			ithw_exception.text	= "บันทึกข้อมูลการเปลี่ยนแปลงรายละเอียดสัญญาในส่วนการค้ำประกันไม่ได้(Contract Coll Old) ไม่ได้  "
			ithw_exception.text	+= ls_contajdocno +"  "+ lds_adjcollold.of_geterrormessage()
			lb_error					= true
			goto objdestroy
		end if
	end if
end if

// ผ่านรายการแก้ไขไปปรับปรุงในสัญญา
try
	if lb_chgpay	then
		this.of_post_reqcontadj_pay( lds_adjmast, lds_adjpay )
	end if
	
	if lb_chgint	then
		this.of_post_reqcontadj_int( lds_adjmast, lds_adjint, lds_adjintspc )
	end if

	if lb_chgcoll	then
		this.of_post_reqcontadj_coll( lds_adjmast, lds_adjcoll )
	end if
catch( Exception lthw_postadj )
	ithw_exception.text	= lthw_postadj.text
	lb_error					= true
end try

objdestroy:
destroy lds_adjmast
destroy lds_adjpay
destroy lds_adjint
destroy lds_adjintspc
destroy lds_adjcoll
destroy lds_adjintspcold
destroy lds_adjcollold
this.of_setsrvdwxmlie( false )

if lb_error then
	rollback using itr_sqlca ;
	throw ithw_exception
end if

commit using itr_sqlca ;

// ส่งเลขที่ใบคำขอกลับ เผื่อนำไปทำรายการได้
astr_lccontaj.contaj_docno		= ls_contajdocno

return 1
end function

public function integer of_init_printcontract (ref str_lcprintcont astr_lcprintcont) throws Exception;string		ls_reqno, ls_coopid
dec		ldc_apvamt, ldc_signamt
long		ll_row
datetime	ldtm_apvdate, ldtm_signdate
boolean	lb_error = false
n_ds		lds_continfo, lds_signlist, lds_contcoll

ls_coopid			= astr_lcprintcont.coop_id
ls_reqno			= astr_lcprintcont.loanrequest_docno

lds_continfo		= create n_ds
lds_continfo.dataobject	= "d_lcsrv_print_loancontrct"
lds_continfo.settransobject( itr_sqlca )

lds_signlist		= create n_ds
lds_signlist.dataobject	= "d_lcsrv_print_loancontrctsignlist"
lds_signlist.settransobject( itr_sqlca )

lds_contcoll		= create n_ds
lds_contcoll.dataobject	= "d_lcsrv_print_loancontrctcoll"
lds_contcoll.settransobject( itr_sqlca )

lds_continfo.retrieve( ls_coopid, ls_reqno )
ll_row		= lds_continfo.rowcount()
if ll_row <= 0 then
	ithw_exception.text	= "ไม่พบข้อมูลคำขอกู้เลขที่ "+ls_reqno+" ("+ls_coopid+") กรุณาตรวจสอบ"
	lb_error					= true
	goto objdestroy
end if

lds_signlist.retrieve( ls_coopid, ls_reqno )
lds_contcoll.retrieve( ls_coopid, ls_reqno )

ldc_apvamt		= lds_continfo.getitemdecimal( 1, "loanapprove_amt" )
ldtm_apvdate	= lds_continfo.getitemdatetime( 1, "loanapprove_date" )

ldc_signamt		= lds_continfo.getitemdecimal( 1, "contsign_amt" )
ldtm_signdate	= lds_continfo.getitemdatetime( 1, "contsign_date" )

if ldc_signamt = 0 then
	lds_continfo.setitem( 1, "contsign_amt", ldc_apvamt )
	lds_continfo.setitem( 1, "contsign_date", ldtm_apvdate )
end if

this.of_setsrvdwxmlie( true )
astr_lcprintcont.xml_contdetail		= inv_dwxmliesrv.of_xmlexport( lds_continfo )
astr_lcprintcont.xml_contcoll		= inv_dwxmliesrv.of_xmlexport( lds_contcoll )

// ฝั่งผู้ให้กู้
lds_signlist.setfilter( "contsign_type = 1" )
lds_signlist.filter()
astr_lcprintcont.xml_signlist1		= inv_dwxmliesrv.of_xmlexport( lds_signlist )

// ฝั่งผู้กู้
lds_signlist.setfilter( "contsign_type = 2" )
lds_signlist.filter()
astr_lcprintcont.xml_signlist2		= inv_dwxmliesrv.of_xmlexport( lds_signlist )

this.of_setsrvdwxmlie( false )

objdestroy:
destroy lds_continfo
destroy lds_signlist
destroy lds_contcoll

if lb_error then
	throw ithw_exception
end if

return 1
end function

public function integer of_save_printcontract (ref str_lcprintcont astr_lcprintcont) throws Exception;string		ls_coopid, ls_reqno, ls_ccoopid, ls_contno, ls_refcollno
long		ll_index
integer	li_updcoll
boolean	lb_error = false
dec		ldc_signamt, ldc_signint
datetime	ldtm_signdate, ldtm_startpay
n_ds		lds_continfo, lds_signlist, lds_contcoll

lds_continfo		= create n_ds
lds_continfo.dataobject	= "d_lcsrv_print_loancontrct"

lds_signlist		= create n_ds
lds_signlist.dataobject	= "d_lcsrv_print_loancontrctsignlist"
lds_signlist.settransobject( itr_sqlca )

lds_contcoll		= create n_ds
lds_contcoll.dataobject	= "d_lcsrv_print_loancontrctcoll"
lds_contcoll.settransobject( itr_sqlca )

this.of_setsrvdwxmlie( true )
try
	inv_dwxmliesrv.of_xmlimport( lds_continfo, astr_lcprintcont.xml_contdetail )
	inv_dwxmliesrv.of_xmlimport( lds_signlist, astr_lcprintcont.xml_signlist1 )
	inv_dwxmliesrv.of_xmlimport( lds_signlist, astr_lcprintcont.xml_signlist2 )
	inv_dwxmliesrv.of_xmlimport( lds_contcoll, astr_lcprintcont.xml_contcoll )
catch( Exception lthw_imperr )
	ithw_exception.text	= lthw_imperr.text
	lb_error					= true
end try

if lb_error then goto objdestroy

ls_coopid			= lds_continfo.getitemstring( 1, "coop_id" )
ls_reqno			= lds_continfo.getitemstring( 1, "loanrequest_docno" )
ls_ccoopid		= lds_continfo.getitemstring( 1, "concoop_id" )
ls_contno		= lds_continfo.getitemstring( 1, "loancontract_no" )
ldtm_signdate	= lds_continfo.getitemdatetime( 1, "contsign_date" )
ldc_signamt		= lds_continfo.getitemdecimal( 1, "contsign_amt" )
ldc_signint		= lds_continfo.getitemdecimal( 1, "contsign_intrate" )
ldtm_startpay	= lds_continfo.getitemdatetime( 1, "startpay_date" )

// ตรวจสอบ
if ldc_signamt = 0 or isnull( ldc_signamt ) then
	ithw_exception.text	= "กรุณาระบุยอดที่จะเซ็นต์สัญญาด้วย"
	lb_error					= true
	goto objdestroy
end if

if ldc_signint = 0 or isnull( ldc_signint ) then
	ithw_exception.text	= "กรุณาระบุอัตราดอกเบี้ยที่จะระบุในสัญญาด้วย"
	lb_error					= true
	goto objdestroy
end if

for ll_index = 1 to lds_contcoll.rowcount()
	ls_refcollno		= trim( lds_contcoll.getitemstring( ll_index, "ref_collno" ) )
	
	if ls_refcollno = "" or isnull( ls_refcollno ) then
		ithw_exception.text	= "กรุณาใส่ข้อมูลเลขที่บัตรประชาชนในรายการคนค้ำประกันให้ครบถ้วนด้วย"
		lb_error					= true
		goto objdestroy
	end if
next

update	lcreqloan
set			contsign_date		= :ldtm_signdate,
			contsign_amt		= :ldc_signamt,
			contsign_intrate	= :ldc_signint,
			startpay_date		= :ldtm_startpay
where	( coop_id					= :ls_coopid ) and
			( loanrequest_docno	= :ls_reqno )
using		itr_sqlca ;

// ลบทิ้งรายการค้ำประกันที่เป็นคน
delete from lcreqloancoll
where	( coop_id		= :ls_coopid ) and
			( loanrequest_docno	= :ls_reqno ) and
			( loancolltype_code	= '01' )
using		itr_sqlca ;

// ลบทิ้งรายการคนเซ็นสัญญา
delete from lcreqloancontsignlist
where	( coop_id		= :ls_coopid ) and
			( loanrequest_docno	= :ls_reqno )
using		itr_sqlca ;

// ใส่เลขเอกสารให้คนเซ็นสัญญา
for ll_index = 1 to lds_signlist.rowcount()
	lds_signlist.setitem( ll_index, "coop_id", ls_coopid )
	lds_signlist.setitem( ll_index, "loanrequest_docno", ls_reqno )
	lds_signlist.setitem( ll_index, "seq_no", ll_index )
next

// ใส่เลขเอกสารให้ table คนค้ำ
for ll_index = 1 to lds_contcoll.rowcount()
	lds_contcoll.setitem( ll_index, "coop_id", ls_coopid )
	lds_contcoll.setitem( ll_index, "loanrequest_docno", ls_reqno )
	lds_contcoll.setitem( ll_index, "seq_no", ll_index )
	lds_contcoll.setitem( ll_index, "loancolltype_code", '01' )
next

if lds_signlist.update() <> 1 then
	ithw_exception.text	= "ไม่สามารถบันทึกรายชื่อผู้เซ็นต์สัญญาได้ "+lds_signlist.of_geterrormessage()
	lb_error					= true
	goto objdestroy
end if

if lds_contcoll.update() <> 1 then
	ithw_exception.text	= "ไม่สามารถบันทึกข้อมูลรายชื่อผู้ค้ำประกันได้ "+lds_contcoll.of_geterrormessage()
	lb_error					= true
	goto objdestroy
end if

try
	// ถ้ามีสัญญาแล้วเอาคนค้ำไปบันทึกใส่ในสัญญา
	if ls_contno <> "" and not isnull( ls_contno ) then
		this.of_post_printcontcoll( lds_contcoll, ls_ccoopid, ls_contno )
	end if
	
	// ปรับปรุงรายละเอียดคนค้ำไปใส่ table person master
	this.of_post_printpersdet( lds_contcoll )
	
catch( Exception lthw_errpost )
	ithw_exception.text	= lthw_errpost.text
	lb_error					= true
end try

if lb_error then goto objdestroy

objdestroy:
this.of_setsrvdwxmlie( false )
destroy lds_continfo
destroy lds_signlist
destroy lds_contcoll

if lb_error then
	rollback using itr_sqlca ;
	throw ithw_exception
end if

commit using itr_sqlca ;

return 1
end function

private function integer of_post_printpersdet (n_ds ads_contcoll) throws Exception;string		ls_adno, ls_admoo, ls_adroad, ls_adsoi, ls_adtambol, ls_addistrict, ls_adprovince
string		ls_refcollno, ls_desc, ls_personid
long		ll_index
datetime	ldtm_birthdate
dec		ldc_collamt, ldc_collperc, ldc_baseperc

for ll_index = 1 to ads_contcoll.rowcount()
	ls_refcollno		= ads_contcoll.getitemstring( ll_index, "ref_collno" )
	ls_desc			= ads_contcoll.getitemstring( ll_index, "description" )
	ls_adno			= ads_contcoll.getitemstring( ll_index, "addr_no" )
	ls_admoo		= ads_contcoll.getitemstring( ll_index, "addr_moo" )
	ls_adroad		= ads_contcoll.getitemstring( ll_index, "addr_road" )
	ls_adsoi			= ads_contcoll.getitemstring( ll_index, "addr_soi" )
	ls_adtambol		= ads_contcoll.getitemstring( ll_index, "addr_tambol" )
	ls_addistrict		= ads_contcoll.getitemstring( ll_index, "addr_district" )
	ls_adprovince	= ads_contcoll.getitemstring( ll_index, "addr_province" )
	ldtm_birthdate	= ads_contcoll.getitemdatetime( ll_index, "birth_date" )
	
	ls_refcollno		= trim( ls_refcollno )
	ls_desc			= trim( ls_desc )

	select		person_id
	into		:ls_personid
	from		lcmembpersonmaster
	where	( person_id		= :ls_refcollno )
	using		itr_sqlca ;
	
	if itr_sqlca.sqlcode = 0 then
		update	lcmembpersonmaster
		set			birth_date		= :ldtm_birthdate,
					addr_no			= :ls_adno,
					addr_moo		= :ls_admoo,
					addr_road		= :ls_adroad,
					addr_soi			= :ls_adsoi,
					addr_tambol	= :ls_adtambol,
					addr_district		= :ls_addistrict,
					addr_province	= :ls_adprovince
		where	( person_id		= :ls_refcollno )
		using		itr_sqlca ;
	else
		insert into lcmembpersonmaster
					( person_id, person_name, birth_date, addr_no, addr_moo, addr_road, addr_soi, addr_tambol, addr_district, addr_province )
		values	( :ls_refcollno, :ls_desc, :ldtm_birthdate, :ls_adno, :ls_admoo, :ls_adroad, :ls_adsoi, :ls_adtambol, :ls_addistrict, :ls_adprovince )
		using		itr_sqlca ;
	end if
	
	if itr_sqlca.sqlcode <> 0 then
		ithw_exception.text	= "ไม่สามารถจัดเก็บรายละเอียดคนค้ำประกันได้ (PersonMaster) "+ls_refcollno+") ได้ "+itr_sqlca.sqlerrtext
		throw ithw_exception
	end if
	
next

return 1
end function

private function integer of_post_printcontcoll (n_ds ads_contcoll, string as_concoopid, string as_contno) throws Exception;string		ls_refcollno, ls_desc
long		ll_index
dec		ldc_collamt, ldc_collperc, ldc_baseperc

delete from	lccontcoll
where	( coop_id					= :as_concoopid ) and
			( loancontract_no		= :as_contno ) and
			( loancolltype_code	= '01' )
using		itr_sqlca ;

for ll_index = 1 to ads_contcoll.rowcount()
	ls_refcollno		= ads_contcoll.getitemstring( ll_index, "ref_collno" )
	ls_desc			= ads_contcoll.getitemstring( ll_index, "description" )
	ldc_collamt		= ads_contcoll.getitemdecimal( ll_index, "coll_amt" )
	ldc_collperc		= ads_contcoll.getitemdecimal( ll_index, "coll_percent" )
	ldc_baseperc	= ads_contcoll.getitemdecimal( ll_index, "base_percent" )
	
	insert into lccontcoll
				( coop_id, loancontract_no, seq_no, loancolltype_code, ref_collno, description, coll_amt, coll_percent, base_percent )
	values	( :as_concoopid, :as_contno, :ll_index, '01', :ls_refcollno, :ls_desc, :ldc_collamt, :ldc_collperc, :ldc_baseperc )
	using		itr_sqlca ;
	
	if itr_sqlca.sqlcode <> 0 then
		ithw_exception.text	= "ไม่สามารถบันทึกผู้ค้ำประกันให้สัญญาเลขที่ "+as_contno+" ("+as_concoopid+") ได้ "+itr_sqlca.sqlerrtext
		throw ithw_exception
	end if
next

return 1
end function

on n_cst_lncoopsrv_allrequest.create
call super::create
TriggerEvent( this, "constructor" )
end on

on n_cst_lncoopsrv_allrequest.destroy
TriggerEvent( this, "destructor" )
call super::destroy
end on

event constructor;ithw_exception = create Exception

end event

event destructor;if isvalid( ithw_exception ) then destroy ithw_exception
end event

