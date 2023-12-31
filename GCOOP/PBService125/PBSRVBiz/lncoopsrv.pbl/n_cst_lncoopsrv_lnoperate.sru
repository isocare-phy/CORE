$PBExportHeader$n_cst_lncoopsrv_lnoperate.sru
$PBExportComments$lcsrv ส่วนบริการการทำรายการสินเชื่อ
forward
global type n_cst_lncoopsrv_lnoperate from nonvisualobject
end type
end forward

global type n_cst_lncoopsrv_lnoperate from nonvisualobject
end type
global n_cst_lncoopsrv_lnoperate n_cst_lncoopsrv_lnoperate

type variables
transaction	itr_sqlca
Exception	ithw_exception

n_cst_dbconnectservice		inv_transection
n_cst_lncoopsrv_interest		inv_intsrv
n_cst_doccontrolservice		inv_docsrv
n_cst_datetimeservice		inv_datetimesrv
n_cst_dwxmlieservice			inv_dwxmliesrv

string		is_coopcontrol

constant string	DWO_PAYOUTSLIP	= "d_lcsrv_slippayout"
constant string	DWO_PAYOUTEXP		= "d_lcsrv_slippayoutexp"
constant string	DWO_PAYINSLIP 		= "d_lcsrv_slippayin"
constant string	DWO_PAYINSLIPDET	= "d_lcsrv_slippayindet"
constant string	DWO_PAYINEXP		= "d_lcsrv_slippayinexp"
end variables

forward prototypes
public function integer of_initservice (n_cst_dbconnectservice anv_dbtrans) throws Exception
private function integer of_setsrvlcinterest (boolean ab_switch)
private function integer of_setsrvdwxmlie (boolean ab_switch)
private function integer of_setsrvdoccontrol (boolean ab_switch)
private function integer of_setsrvdatetime (boolean ab_switch)
private function integer of_initlnrcv_calint (ref n_ds ads_lnrcv) throws Exception
public function integer of_initpayin_calint (ref string as_xmlloan, datetime adtm_opedate) throws Exception
private function integer of_setdsmodify (ref n_ds ads_requester, long al_row, boolean ab_keymodify)
private function integer of_setabspayindet (ref n_ds ads_payindet) throws Exception
public function string of_getmembdetail (string as_coopid, string as_memno)
public function integer of_savepost_lnrcv (str_lcslippost astr_lcslippost) throws Exception
private function integer of_postslip_lnrcv (n_ds ads_payoutslip) throws Exception
private function integer of_setintspceffectdate (string as_coopid, string as_contno, datetime adtm_rcvdate) throws Exception
public function integer of_initpost_lnrcv (ref str_lcslippost astr_lcslippost) throws Exception
private function integer of_postslip_payinloan (n_ds ads_payinslip, n_ds ads_payinslipdet) throws Exception
public function integer of_initpost_payin (ref str_lcslippost astr_lcslippost) throws Exception
public function integer of_savepost_payin (str_lcslippost astr_lcslippost) throws Exception
public function integer of_saveccl_lnrcv (str_lcslipcancel astr_slipcancel) throws Exception
public function integer of_initlnrcv (ref str_lcslippayout astr_lcpayout) throws Exception
private function integer of_poststm_contract (str_lcpoststmloan astr_lnstatement) throws Exception
public function integer of_initpayin (ref str_lcslippayin astr_lcpayin) throws Exception
public function integer of_getloanpayment (ref n_ds ads_payloan, string as_memcoopid, string as_memno, string as_sliptype, datetime adtm_operate) throws Exception
public function integer of_calintlnpayment (ref n_ds ads_payinloan, datetime adtm_opedate) throws Exception
public function integer of_saveccl_payin (str_lcslipcancel astr_slipcancel) throws Exception
public function integer of_initccl_lnrcv (ref str_lcslipcancel astr_slipcancel) throws Exception
public function integer of_initccl_payinloan (ref str_lcslipcancel astr_slipcancel) throws Exception
private function integer of_initlnrcv_clrloan (n_ds ads_lnrcv, ref n_ds ads_payinloan) throws Exception
private function integer of_postccl_lnrcv (n_ds ads_payoutslip) throws Exception
private function integer of_postccl_payinloan (n_ds ads_payinslip, n_ds ads_payinslipdet) throws Exception
public function integer of_saveslip_lnrcv (ref str_lcslippayout astr_lcpayout) throws Exception
public function integer of_saveslip_payin (ref str_lcslippayin astr_lnslip) throws Exception
public function integer of_getmemblnrcvlist (ref str_lclnrcvlist astr_lclnrcvlist) throws Exception
private function integer of_initpayin_loan (ref n_ds ads_payloan, string as_memcoopid, string as_memno, string as_sliptype) throws Exception
public function integer of_getmembnoticemthlist (ref str_lcnoticemthlist astr_lcnoticemthlist) throws Exception
public function integer of_setpayfromnotice (ref n_ds ads_payinslip, ref n_ds ads_payinsliploan, string as_noticecoopid, string as_noticedocno)
public function integer of_initlnrcv_recalint (ref str_lcslippayout astr_slip) throws Exception
public function integer of_initintfixchg (string as_xmloption, ref string as_xmldata) throws Exception
public function integer of_saveintfixchg (ref string as_xmlintfixchg) throws Exception
end prototypes

public function integer of_initservice (n_cst_dbconnectservice anv_dbtrans) throws Exception;itr_sqlca 			= anv_dbtrans.itr_dbconnection
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

private function integer of_initlnrcv_calint (ref n_ds ads_lnrcv) throws Exception;string		ls_contno, ls_contcoopid, ls_rcvfromreqcont
integer	li_rcvperiod
dec{2}	ldc_prnbalance, ldc_loanrcvamt, ldc_prncalint, ldc_intperiod
datetime	ldtm_lastcalint, ldtm_calintfrom, ldtm_calintto
datetime	ldtm_rcvdate

// ตรวจสอบว่าต้องคำนวณ ด/บ ค้างหรือไม่
li_rcvperiod			= ads_lnrcv.getitemnumber( 1, "rcv_period" )
ldtm_rcvdate		= ads_lnrcv.getitemdatetime( 1, "slip_date" )

// ถ้ารับจากคำขอที่ได้รับอนุมัติหรืองวดที่รับเป็นงวดแรกไม่ต้องคิด ด/บ ค้าง
if li_rcvperiod <= 1 then
	return 0
end if

ls_contcoopid		= ads_lnrcv.getitemstring( 1, "concoop_id" )
ls_contno			= ads_lnrcv.getitemstring( 1, "loancontract_no" )
ldc_prnbalance		= ads_lnrcv.getitemdecimal( 1, "bfshrcont_balamt" )
ldc_loanrcvamt		= ads_lnrcv.getitemdecimal( 1, "payout_amt" )
ldtm_lastcalint		= ads_lnrcv.getitemdatetime( 1, "bflastcalint_date" )

ldc_prncalint		= ldc_prnbalance
ldtm_calintfrom		= ldtm_lastcalint
ldtm_calintto		= ldtm_rcvdate

// ประกาศ Service Interest
this.of_setsrvlcinterest( true )
ldc_intperiod	= inv_intsrv.of_computeinterest( ls_contcoopid, ls_contno, ldc_prncalint, ldtm_calintfrom, ldtm_calintto )	
this.of_setsrvlcinterest( false )


if ldc_intperiod <= 0 then
	ldc_intperiod = 0
end if

ads_lnrcv.setitem( 1, "interest_period", ldc_intperiod )
ads_lnrcv.setitem( 1, "prncalint_amt", ldc_prncalint )
ads_lnrcv.setitem( 1, "calint_from", ldtm_calintfrom )
ads_lnrcv.setitem( 1, "calint_to", ldtm_calintto )

return 1
end function

public function integer of_initpayin_calint (ref string as_xmlloan, datetime adtm_opedate) throws Exception;boolean	lb_error
n_ds		lds_payinloan

lds_payinloan		= create n_ds
lds_payinloan.dataobject		= DWO_PAYINSLIPDET

this.of_setsrvdwxmlie( true )

lb_error	= false

try
	inv_dwxmliesrv.of_xmlimport( lds_payinloan, as_xmlloan )
	
	this.of_calintlnpayment( lds_payinloan, adtm_opedate )
	
	as_xmlloan		= inv_dwxmliesrv.of_xmlexport( lds_payinloan )
	
catch( Exception lthw_err )
	ithw_exception.text	= lthw_err.text
	lb_error		= true
end try

this.of_setsrvdwxmlie( false )
if isvalid( lds_payinloan ) then destroy lds_payinloan

if lb_error then
	throw ithw_exception
end if

return 1


end function

private function integer of_setdsmodify (ref n_ds ads_requester, long al_row, boolean ab_keymodify);string		ls_iskey
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

private function integer of_setabspayindet (ref n_ds ads_payindet) throws Exception;string		ls_slipitemcode
long		ll_index, ll_count
dec{2}	ldc_bfshrcontbal, ldc_prnpay, ldc_intpay

ll_count		= ads_payindet.rowcount()

// ใส่ข้อมูลการชำระให้ครบถ้วน
for ll_index = 1 to ll_count
	ls_slipitemcode		= ads_payindet.getitemstring( ll_index, "slipitemtype_code" )
	ldc_bfshrcontbal	= ads_payindet.getitemdecimal( ll_index, "bfshrcont_balamt" )
	
	if isnull( ldc_bfshrcontbal ) then ldc_bfshrcontbal = 0
	
	choose case ls_slipitemcode
		case "LON", "INF"
			ldc_prnpay	= ads_payindet.getitemdecimal( ll_index, "principal_payamt" )
			ldc_intpay	= ads_payindet.getitemdecimal( ll_index, "interest_payamt" )
			
			if isnull( ldc_prnpay ) then ldc_prnpay	 = 0
			if isnull( ldc_intpay ) then ldc_intpay = 0
			
			if ldc_prnpay > ldc_bfshrcontbal then
				ithw_exception.text	= "ไม่อนุญาติให้ยอดชำระเงินต้นมากกว่ายอดเงินคงเหลือ กรุณาตรวจสอบ"
				throw ithw_exception
			end if
			
			ads_payindet.setitem( ll_index, "item_payamt", ldc_prnpay + ldc_intpay )
			ads_payindet.setitem( ll_index, "item_balance", ldc_bfshrcontbal - ldc_prnpay )
	end choose
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

public function integer of_savepost_lnrcv (str_lcslippost astr_lcslippost) throws Exception;string		ls_postcoopid, ls_postid, ls_slipcoopid, ls_slipno, ls_sliptype, ls_postdocno
string		ls_slipclrno, ls_xmlsliphead
datetime ldtm_postdate, ldtm_slipdate
integer	li_slipstatus
boolean	lb_error
long		ll_count
n_ds		lds_payoutslip, lds_payinslip, lds_payinslipdet

ls_slipcoopid		= astr_lcslippost.slipcoop_id
ls_slipno				= astr_lcslippost.slip_no

ls_postcoopid		= astr_lcslippost.post_coopid
ls_postid				= astr_lcslippost.post_id
ldtm_postdate		= astr_lcslippost.post_date

lb_error				= false

// สร้าง DataStore สำหรับใช้งาน
lds_payoutslip		= create n_ds
lds_payoutslip.dataobject	= DWO_PAYOUTSLIP
lds_payoutslip.settransobject( itr_sqlca )

ll_count	= lds_payoutslip.retrieve( ls_slipcoopid, ls_slipno )
if ll_count <= 0 then
	ithw_exception.text = "ไม่พบข้อมูลรายการจ่ายเงินกู้ที่ต้องการผ่านรายการ เลขที่ใบสั่งจ่าย "+ls_slipno
	lb_error					= true
	goto objdestroy
end if

try
	// ขอเลขที่ Slip Pay Out
	this.of_setsrvdoccontrol( true )
	ls_postdocno	= inv_docsrv.of_getnewdocno( is_coopcontrol, "LCSLIPPPOST" )
catch( Exception lthw_errdoc )
	ithw_exception.text	= lthw_errdoc.text
	lb_error					= true
end try

if lb_error then goto objdestroy

ls_sliptype		= trim( lds_payoutslip.getitemstring( 1, "sliptype_code" ) )
ls_slipclrno		= trim( lds_payoutslip.getitemstring( 1, "slipclear_no" ) )
ldtm_slipdate	= lds_payoutslip.getitemdatetime( 1, "slip_date" )

li_slipstatus		= 1

// ใส่ข้อมูลการผ่านรายการไปที่ Slip (เวลาส่งไป Post จะได้อ่านค่าจาก DS ได้เลย)
lds_payoutslip.setitem( 1, "postslipdoc_no", ls_postdocno )
lds_payoutslip.setitem( 1, "slip_status", li_slipstatus )

// --------------------------------------------------------
// ปรับปรุงสถานะ Slipแล้วไปทำการผ่านรายการ
// --------------------------------------------------------
update	slslippayout
set			slip_status		= :li_slipstatus,
			postslipdoc_no	= :ls_postdocno
where	( coop_id		= :ls_slipcoopid ) and
			( payoutslip_no	= :ls_slipno )
using		itr_sqlca ;

if itr_sqlca.sqlcode <> 0 then
	ithw_exception.text	= "ปรับปรุงสถานะของ Slip จ่ายเงินกู้ให้เป็นผ่านรายการแล้วไม่ได้ " + itr_sqlca.sqlerrtext
	lb_error					= true
	goto objdestroy
end if

// Export ข้อมูลอีกทีเพราะมีการใส่ข้อมูลการผ่านรายการเพิ่มเติม
ls_xmlsliphead		= string( lds_payoutslip.describe( "Datawindow.data.XML" ) )

try
	// ผ่านรายการการจ่ายเงินกู้
	this.of_postslip_lnrcv( lds_payoutslip )
	
	// ถ้ามีการหักกลบส่งไปผ่านรายการด้วย
	if ls_slipclrno <> "" and not isnull( ls_slipclrno ) then
		str_lcslippost	lstr_postpayin
		
		lstr_postpayin.slipcoop_id		= ls_slipcoopid
		lstr_postpayin.slip_no				= ls_slipclrno
		lstr_postpayin.post_id				= astr_lcslippost.post_id
		lstr_postpayin.post_date			= astr_lcslippost.post_date
		lstr_postpayin.trnsave_status	= 1
		
		this.of_savepost_payin( lstr_postpayin )
	end if
catch( Exception lthw_postlnrcv )
	rollback using itr_sqlca ;
	throw lthw_postlnrcv
end try

objdestroy:
destroy lds_payoutslip

if lb_error then
	rollback using itr_sqlca ;
	throw ithw_exception
end if

commit using itr_sqlca ;

return 1
end function

private function integer of_postslip_lnrcv (n_ds ads_payoutslip) throws Exception;string		ls_slipno, ls_docrcvno, ls_contno, ls_contcoopid, ls_moneytype, ls_lwditemtype
string		ls_loantype, ls_entryid, ls_coopid, ls_keepperiod
string		ls_bankcode, ls_bankbranch, ls_bankaccid
integer	li_periodrcv, li_continttype, li_year, li_month
long		ll_row
dec{2}	ldc_prinbalance, ldc_withdrawamt, ldc_lnrcvamt
dec{2}	ldc_bfintarrear, ldc_intperiod, ldc_intarrbal, ldc_prncalint
datetime	ldtm_entrydate, ldtm_procdate
datetime	ldtm_slipdate, ldtm_opedate, ldtm_calintfrom, ldtm_calintto, ldtm_startkeep
str_lcpoststmloan	lstr_lnstm

// ดึงข้อมูลสำหรับผ่านรายการจ่ายเงินกู้
ls_coopid				= trim( ads_payoutslip.getitemstring( 1, "coop_id" ) )
ls_slipno				= trim( ads_payoutslip.getitemstring( 1, "payoutslip_no" ) )
ls_docrcvno			= trim( ads_payoutslip.getitemstring( 1, "document_no" ) )
ls_contno			= trim( ads_payoutslip.getitemstring( 1, "loancontract_no" ) )
ls_contcoopid		= trim( ads_payoutslip.getitemstring( 1, "concoop_id" ) )
ls_loantype			= trim( ads_payoutslip.getitemstring( 1, "shrlontype_code" ) )
li_periodrcv			= ads_payoutslip.getitemnumber( 1, "rcv_period" )
ldc_lnrcvamt		= ads_payoutslip.getitemdecimal( 1, "payout_amt" )
ldc_prncalint		= ads_payoutslip.getitemdecimal( 1, "prncalint_amt" )
ldc_intperiod		= ads_payoutslip.getitemdecimal( 1, "interest_period" )
ldtm_slipdate		= ads_payoutslip.getitemdatetime( 1, "slip_date" )
ldtm_opedate		= ads_payoutslip.getitemdatetime( 1, "operate_date" )
ldtm_calintfrom		= ads_payoutslip.getitemdatetime( 1, "calint_from" )
ldtm_calintto		= ads_payoutslip.getitemdatetime( 1, "calint_to" )

ls_entryid			= ads_payoutslip.getitemstring( 1, "entry_id" )
ldtm_entrydate		= datetime( today(), now() )

// ดึงข้อมูลสัญญาเงินกู้
select		principal_balance, withdrawable_amt, interest_arrear, int_continttype
into		:ldc_prinbalance, :ldc_withdrawamt, :ldc_bfintarrear, :li_continttype
from		lccontmaster
where	( coop_id			= :ls_contcoopid ) and
			( loancontract_no	= :ls_contno )
using		itr_sqlca ;

if itr_sqlca.sqlcode <> 0 then
	ithw_exception.text	+= "ไม่พบข้อมูล เลขสัญญา #"+ls_contno
	throw ithw_exception
end if

// กำหนดค่าต่าง ๆ
ls_lwditemtype		= "LWD"
ldc_prinbalance		= ldc_prinbalance + ldc_lnrcvamt
ldc_withdrawamt	= ldc_withdrawamt - ldc_lnrcvamt
ldc_intarrbal			= ldc_bfintarrear + ldc_intperiod

// บันทึกข้อมูลโดยดูจากงวดที่รับเงิน
if li_periodrcv = 1 then
	
	// ถ้าสัญญานั้นอัตราดอกเบี้ยเป็นช่วงต้อง set ค่าวันที่ลงไป
	if li_continttype = 3 then
		try
			this.of_setintspceffectdate( ls_coopid, ls_contno, ldtm_slipdate )
		catch ( Exception lthw_intspc )
			throw lthw_intspc
		end try
	end if
	
	update	lccontmaster
	set			principal_balance	= :ldc_prinbalance,
				withdrawable_amt	= :ldc_withdrawamt,
				interest_arrear		= 0,
				last_periodrcv		= :li_periodrcv,
				startcont_date		= :ldtm_slipdate,
				lastreceive_date	= :ldtm_slipdate,
				lastcalint_date		= :ldtm_slipdate
	where	( loancontract_no	= :ls_contno ) and
				( coop_id				= :ls_contcoopid )
	using		itr_sqlca ;
else
	update	lccontmaster
	set			principal_balance	= :ldc_prinbalance,
				withdrawable_amt	= :ldc_withdrawamt,
				interest_arrear		= :ldc_intarrbal,
				last_periodrcv		= :li_periodrcv,
				lastreceive_date	= :ldtm_slipdate,
				lastcalint_date		= :ldtm_slipdate
	where	( loancontract_no	= :ls_contno ) and
				( coop_id			= :ls_contcoopid )
	using	itr_sqlca ;
end if

if itr_sqlca.sqlcode <> 0 then
	ithw_exception.text	+= "ไม่สามารถปรับปรุงยอดเบิกเงินกู้ได้ เลขสัญญา #"+ls_contno+"~r~n"+itr_sqlca.sqlerrtext
	throw ithw_exception
end if

// บันทึกลง Statement
lstr_lnstm.loancontract_no			= ls_contno
lstr_lnstm.concoop_id					= ls_contcoopid
lstr_lnstm.slip_date					= ldtm_slipdate
lstr_lnstm.operate_date				= ldtm_opedate
lstr_lnstm.account_date				= ldtm_slipdate
lstr_lnstm.intaccum_date				= ldtm_slipdate
lstr_lnstm.ref_slipno					= ls_slipno
lstr_lnstm.ref_docno					= ls_docrcvno
lstr_lnstm.loanitemtype_code		= ls_lwditemtype
lstr_lnstm.period						= li_periodrcv
lstr_lnstm.principal_payment		= ldc_lnrcvamt
lstr_lnstm.interest_payment			= 0
lstr_lnstm.principal_balance			= ldc_prinbalance
lstr_lnstm.prncalint_amt				= ldc_prncalint
lstr_lnstm.calint_from					= ldtm_calintfrom
lstr_lnstm.calint_to					= ldtm_calintto
lstr_lnstm.bfinterest_arrear			= ldc_bfintarrear
lstr_lnstm.interest_period			= ldc_intperiod
lstr_lnstm.interest_arrear			= ldc_intarrbal
lstr_lnstm.moneytype_code			= ls_moneytype
lstr_lnstm.item_status				= 1
lstr_lnstm.entry_id						= ls_entryid
lstr_lnstm.entry_bycoopid			= ls_coopid

try
	this.of_poststm_contract( lstr_lnstm )
catch ( Exception lthw_stm )
	throw lthw_stm
end try

return 1
end function

private function integer of_setintspceffectdate (string as_coopid, string as_contno, datetime adtm_rcvdate) throws Exception;integer	li_timetype, li_timeamt, li_seqno
datetime	ldtm_effdate, ldtm_expdate, ldtm_effnext

declare	data_cur cursor for
select		seq_no, int_timetype, int_timeamt
from		lccontintspc
where	( loancontract_no	= :as_contno ) and
			( coop_id			= :as_coopid )
order by seq_no asc
using		itr_sqlca ;

ldtm_effdate		= adtm_rcvdate

open data_cur ;
	fetch data_cur into :li_seqno, :li_timetype, :li_timeamt ;
	do while itr_sqlca.sqlcode = 0
		
		ldtm_effnext		= datetime( inv_datetimesrv.of_relativemonth( date( ldtm_effdate ), li_timeamt ) )
		
		// ถ้าเป็นชนเดือนให้เลื่อนวันไปต้นเดือนหน้า
		if li_timetype = 2 then
			ldtm_effnext		= datetime( inv_datetimesrv.of_lastdayofmonth( date( ldtm_effnext ) ) )
			ldtm_effnext		= datetime( relativedate( date( ldtm_effnext ), 1 ) )
		end if
		
		// ถ้าไม่ได้ระบุเวลาแสดงว่ามันเป็นอันสุดท้ายแล้ว
		if li_timeamt = 0 then
			ldtm_expdate	= datetime( date( 3000, 12, 31 ) )
		else
			ldtm_expdate	= datetime( relativedate( date( ldtm_effnext ), -1 ) )
		end if
		
		update	lccontintspc
		set			effective_date	= :ldtm_effdate,
					expire_date		= :ldtm_expdate
		where	( loancontract_no	= :as_contno ) and
					( seq_no				= :li_seqno )
		using		itr_sqlca ;
		
		if itr_sqlca.sqlcode <> 0 then
			ithw_exception.text	+= "(อัตราดอกเบี้ยเป็นช่วง ) ไม่สามารถปรับปรุงวันที่ของช่วงอัตราดอกเบี้ยได้ "+itr_sqlca.sqlerrtext
			throw ithw_exception
		end if
		
		ldtm_effdate	= ldtm_effnext
		
		fetch data_cur into :li_seqno, :li_timetype, :li_timeamt ;
	loop
close data_cur ;

return 1
end function

public function integer of_initpost_lnrcv (ref str_lcslippost astr_lcslippost) throws Exception;string		ls_coopid, ls_memno, ls_xmlmemdet
long		ll_count
datetime	ldtm_lnrcvdate
n_ds		lds_payoutlist

ls_coopid				= astr_lcslippost.slipcoop_id
ldtm_lnrcvdate		= astr_lcslippost.slip_date

astr_lcslippost.xml_sliplist		= ""

lds_payoutlist		= create n_ds
lds_payoutlist.dataobject	= "d_lcsrv_postlist_lnrcv"
lds_payoutlist.settransobject( itr_sqlca )
lds_payoutlist.retrieve( ls_coopid, ldtm_lnrcvdate )

ll_count		= lds_payoutlist.rowcount()

if ll_count < 1 then
	ithw_exception.text	= "ไม่พบข้อมูลใบสั่งจ่ายเงินกู้ประจำวันที่ "+string( ldtm_lnrcvdate, "dd/mm/" )+string( year( date( ldtm_lnrcvdate ) )+543 )+" (สาขา "+ls_coopid+")"
	destroy lds_payoutlist
	throw ithw_exception
end if

this.of_setsrvdwxmlie( true )
astr_lcslippost.xml_sliplist		= inv_dwxmliesrv.of_xmlexport( lds_payoutlist )
this.of_setsrvdwxmlie( false )

destroy lds_payoutlist

return 1
end function

private function integer of_postslip_payinloan (n_ds ads_payinslip, n_ds ads_payinslipdet) throws Exception;string		ls_contno, ls_contcoopid, ls_loantype, ls_itemcode, ls_slipno, ls_refdocno, ls_memno, ls_memcoopid, ls_refslipno, ls_sliptype
integer	li_installment, li_bflastperiodpay
integer	li_odflag, li_lawstatus, li_contstatus
integer	li_lastperiodpay
long		ll_count, ll_index
dec{2}	ldc_prnpay, ldc_intpay, ldc_intperiod, ldc_prncalint
dec{2}	ldc_bfprnbal, ldc_lnapvamt, ldc_bfintarr, ldc_bfintarrset, ldc_bfintpayment
dec{2}	ldc_wtdbal, ldc_prnbal, ldc_intarrbal, ldc_intarrsetbal, ldc_intpayment
datetime	ldtm_bflastpayment, ldtm_bflastcalint
datetime	ldtm_slipdate, ldtm_opedate, ldtm_calintfrom, ldtm_calintto
datetime	ldtm_lastpayment, ldtm_lastcalint

string		ls_entryid, ls_ecoopid
datetime	ldtm_entrydate
str_lcpoststmloan	lstr_lnstm

// กรองแต่รายการหนี้
ads_payinslipdet.setfilter( "slipitemtype_code = 'LON' and operate_flag = 1" )
ads_payinslipdet.filter()

ll_count	= ads_payinslipdet.rowcount()
if ll_count <= 0 then
	return 0
end if

ls_memno			= ads_payinslip.getitemstring( 1, "member_no" )
ls_memcoopid		= ads_payinslip.getitemstring( 1, "memcoop_id" )
ls_slipno				= ads_payinslip.getitemstring( 1, "payinslip_no" )
ls_refdocno			= ads_payinslip.getitemstring( 1, "document_no" )
ls_sliptype			= ads_payinslip.getitemstring( 1, "sliptype_code" )
ls_refslipno			= ads_payinslip.getitemstring( 1, "ref_slipno" )
ldtm_slipdate		= ads_payinslip.getitemdatetime( 1, "slip_date" )
ldtm_opedate		= ads_payinslip.getitemdatetime( 1, "operate_date" )

ls_entryid			= ads_payinslip.getitemstring( 1, "entry_id" )
ls_ecoopid			= ads_payinslip.getitemstring( 1, "entry_bycoopid" )
ldtm_entrydate		= datetime( today(), now() )

for ll_index = 1 to ll_count
	
	ls_contno			= ads_payinslipdet.getitemstring( ll_index, "loancontract_no" )
	ls_contcoopid		= ads_payinslipdet.getitemstring( ll_index, "concoop_id" )
	ls_loantype			= ads_payinslipdet.getitemstring( ll_index, "shrlontype_code" )
	ls_itemcode			= ads_payinslipdet.getitemstring( ll_index, "stm_itemtype" )
	li_installment		= ads_payinslipdet.getitemnumber( ll_index, "period" )
	ldc_prnpay			= ads_payinslipdet.getitemdecimal( ll_index, "principal_payamt" )
	ldc_intpay			= ads_payinslipdet.getitemdecimal( ll_index, "interest_payamt" )
	ldtm_calintfrom		= ads_payinslipdet.getitemdatetime( ll_index, "calint_from" )
	ldtm_calintto		= ads_payinslipdet.getitemdatetime( ll_index, "calint_to" )
	ldc_prncalint		= ads_payinslipdet.getitemdecimal( ll_index, "prncalint_amt" )
	ldc_intperiod		= ads_payinslipdet.getitemdecimal( ll_index, "interest_period" )
	
	// ตรวจสอบ ค่า Null
	if isnull( ldc_prnpay ) then ldc_prnpay = 0
	if isnull( ldc_intpay ) then ldc_intpay = 0
	
	// ดึงข้อมูลของสัญญา
	select	principal_balance, loanapprove_amt, withdrawable_amt,
			last_periodpay,	lastpayment_date,
			lastcalint_date, interest_arrear,
			intset_arrear, intpayment_amt, od_flag,
			contlaw_status, contract_status
	into	:ldc_bfprnbal, :ldc_lnapvamt, :ldc_wtdbal,
			:li_bflastperiodpay, :ldtm_bflastpayment,
			:ldtm_bflastcalint, :ldc_bfintarr,
			:ldc_bfintarrset, :ldc_bfintpayment, :li_odflag,
			:li_lawstatus, :li_contstatus
	from	lccontmaster
	where	( lccontmaster.loancontract_no	= :ls_contno ) and
				( lccontmaster.coop_id			= :ls_contcoopid )
	using	itr_sqlca ;
	
	if itr_sqlca.sqlcode <> 0 then
		ithw_exception.text	+= "การชำระ( Contract pay ) ไม่พบเลขที่สัญญาที่ระบุ "+ls_contno+" กรุณาตรวจสอบ"
		throw ithw_exception
	end if

	if isnull( ldc_bfprnbal ) then ldc_bfprnbal = 0
	if isnull( ldc_lnapvamt ) then ldc_lnapvamt = 0
	if isnull( ldc_wtdbal ) then ldc_wtdbal = 0
	if isnull( ldc_bfintarr ) then ldc_bfintarr = 0
	if isnull( ldc_bfintarrset ) then ldc_bfintarrset = 0
	if isnull( ldc_bfintpayment ) then ldc_bfintpayment = 0
	
	// ให้ค่าเริ่มต้น
	li_lastperiodpay		= li_bflastperiodpay
	ldc_intarrsetbal		= ldc_bfintarrset
	ldtm_lastpayment	= ldtm_bflastpayment
	
	// -------------- เริ่มกระบวนการ ตรวจเช็ค ตัดยอด ------------------------------
	// ถ้าเป็นสัญญา OD ต้องเพิ่มยอดรอเบิก
	if li_odflag = 1 then
		ldc_wtdbal		= ldc_wtdbal + ldc_prnpay
	end if
	
	// ถ้าชำระดอกเบี้ยไม่มากกว่าดอกเบี้ยค้าง ไม่ต้องขยับวันที่ดอกเบี้ยมา
	if ldc_intpay <= ldc_bfintarr then
		ldc_intarrbal			= ldc_bfintarr - ldc_intpay
		ldtm_lastcalint		= ldtm_bflastcalint
	else
		ldc_intarrbal			= ldc_bfintarr + ( ldc_intperiod - ldc_intpay )
		
		// วันที่คิดดอกเบี้ยล่าสุด (ไม่ใช้วันที่ ldtm_calintto เพราะว่า ldtm_calintto มีการขยับวันเพิ่มจากเงื่อนไข ถ้าใช้จะผิด)
		if ldtm_bflastcalint < ldtm_opedate then
			ldtm_lastcalint		= ldtm_opedate
		end if
		
		if ldc_intarrbal < 0 then ldc_intarrbal = 0
	end if
	
	// พวกที่ตัดยอดหรือเพิ่มยอดได้เลย
	ldc_prnbal		= ldc_bfprnbal - ldc_prnpay
	ldc_intpayment	= ldc_bfintpayment + ldc_intpay
	
	// ถ้ามีการขยับงวดชำระ
	if li_installment > li_bflastperiodpay then
		li_lastperiodpay	= li_installment
	end if
	
	// ถ้าหมดและไม่มียอดรอเบิกไม่มี ด/บ ค้าง ปิดสัญญาได้เลย
	if ldc_prnbal <= 0 and ldc_wtdbal = 0 and ldc_intarrbal = 0 and li_odflag <> 1 then
		li_contstatus	= -1
	end if
	
	// ถ้ามีการตั้งด/บ ค้างเอาไว้( ค้างตอนสิ้นปี/ตั้งค้างตอนดำเนินคดี)
	if ldc_bfintarrset > 0 then
		if ldc_intpay > ldc_bfintarrset then
			ldc_intarrsetbal	= 0
		else
			ldc_intarrsetbal	= ldc_bfintarrset - ldc_intpay
		end if
	end if
	
	// วันที่ชำระล่าสุด
	if ldtm_opedate > ldtm_lastpayment or isnull( ldtm_lastpayment ) or string( ldtm_lastpayment, "yyyymmdd" ) = "19000101" then
		ldtm_lastpayment	= ldtm_opedate
	end if
	
	// บันทึกลง Statement
	lstr_lnstm.loancontract_no			= ls_contno
	lstr_lnstm.concoop_id					= ls_contcoopid
	lstr_lnstm.slip_date					= ldtm_slipdate
	lstr_lnstm.operate_date				= ldtm_opedate
	lstr_lnstm.account_date				= ldtm_slipdate
	lstr_lnstm.intaccum_date				= ldtm_opedate
	lstr_lnstm.ref_slipno					= ls_slipno
	lstr_lnstm.ref_docno					= ls_refdocno
	lstr_lnstm.loanitemtype_code		= ls_itemcode
	lstr_lnstm.period						= li_lastperiodpay
	lstr_lnstm.principal_payment		= ldc_prnpay
	lstr_lnstm.interest_payment			= ldc_intpay
	lstr_lnstm.principal_balance			= ldc_prnbal
	lstr_lnstm.prncalint_amt				= ldc_prncalint
	lstr_lnstm.calint_from					= ldtm_calintfrom
	lstr_lnstm.calint_to					= ldtm_calintto
	lstr_lnstm.bfinterest_arrear			= ldc_bfintarr
	lstr_lnstm.interest_period			= ldc_intperiod
	lstr_lnstm.interest_arrear			= ldc_intarrbal
	lstr_lnstm.item_status				= 1
	lstr_lnstm.entry_id						= ls_entryid
	lstr_lnstm.entry_bycoopid			= ls_ecoopid

	try
		this.of_poststm_contract( lstr_lnstm )
	catch ( Exception lthw_stm )
		throw lthw_stm
	end try
	
	// Update ลงสัญญา
	update	lccontmaster
	set			withdrawable_amt	= :ldc_wtdbal,
				principal_balance	= :ldc_prnbal,
				last_periodpay		= :li_lastperiodpay,
				lastpayment_date	= :ldtm_lastpayment,
				prnpayment_amt	= prnpayment_amt + :ldc_prnpay,
				intpayment_amt	= :ldc_intpayment,
				lastcalint_date		= :ldtm_lastcalint,
				interest_arrear		= :ldc_intarrbal,
				intset_arrear		= :ldc_intarrsetbal,
				interest_accum		= interest_accum + :ldc_intpay,
				contract_status		= :li_contstatus
	where	( loancontract_no	= :ls_contno ) and
				( coop_id			= :ls_contcoopid )
	using	itr_sqlca ;
	
	if itr_sqlca.sqlcode <> 0 then
		ithw_exception.text	+= "ไม่สามารถปรับปรุงสัญญาสำหรับการชำระได้ เลขสัญญา #"+ls_contno+"~r~n"+itr_sqlca.sqlerrtext
		throw ithw_exception
	end if
	
next

if ls_sliptype = "LPM" then
	update	lcnoticemthrecv
	set			notice_status	= 1
	where	( coop_id		= :is_coopcontrol ) and
				( notice_docno	= :ls_refslipno )
	using		itr_sqlca ;
end if

return 1
end function

public function integer of_initpost_payin (ref str_lcslippost astr_lcslippost) throws Exception;string		ls_coopid, ls_memno, ls_xmlmemdet
long		ll_count
datetime	ldtm_slipdate
n_ds		lds_payinlist

ls_coopid				= astr_lcslippost.slipcoop_id
ldtm_slipdate		= astr_lcslippost.slip_date

astr_lcslippost.xml_sliplist		= ""

lds_payinlist		= create n_ds
lds_payinlist.dataobject	= "d_lcsrv_postlist_payin"
lds_payinlist.settransobject( itr_sqlca )
lds_payinlist.retrieve( ls_coopid, ldtm_slipdate )

ll_count		= lds_payinlist.rowcount()

if ll_count < 1 then
	ithw_exception.text	= "ไม่พบข้อมูลใบสั่งชำระเงินกู้ประจำวันที่ "+string( ldtm_slipdate, "dd/mm/" )+string( year( date( ldtm_slipdate ) )+543 )+" (สาขา "+ls_coopid+")"
	destroy lds_payinlist
	throw ithw_exception
end if

this.of_setsrvdwxmlie( true )
astr_lcslippost.xml_sliplist		= inv_dwxmliesrv.of_xmlexport( lds_payinlist )
this.of_setsrvdwxmlie( false )

destroy lds_payinlist

return 1
end function

public function integer of_savepost_payin (str_lcslippost astr_lcslippost) throws Exception;string		ls_postcoopid, ls_postid, ls_slipcoopid, ls_slipno, ls_sliptype, ls_postdocno
string		ls_slipclrno, ls_xmlsliphead
datetime ldtm_postdate, ldtm_slipdate
integer	li_slipstatus, li_trnsavestatus
boolean	lb_error
long		ll_count
n_ds		lds_payinslip, lds_payinslipdet

ls_slipcoopid		= astr_lcslippost.slipcoop_id
ls_slipno				= astr_lcslippost.slip_no

ls_postcoopid		= astr_lcslippost.post_coopid
ls_postid				= astr_lcslippost.post_id
ldtm_postdate		= astr_lcslippost.post_date

li_trnsavestatus		= astr_lcslippost.trnsave_status

lb_error				= false

// สร้าง DataStore สำหรับใช้งาน
lds_payinslip		= create n_ds
lds_payinslip.dataobject	= DWO_PAYINSLIP
lds_payinslip.settransobject( itr_sqlca )
lds_payinslip.retrieve( ls_slipcoopid, ls_slipno )

ll_count	= lds_payinslip.rowcount()
if ll_count <= 0 then
	ithw_exception.text = "ไม่พบข้อมูลรายการชำระเงินกู้ที่ต้องการผ่านรายการ เลขที่ใบสั่งชำระ "+ls_slipno
	lb_error					= true
	goto objdestroy
end if

lds_payinslipdet		= create n_ds
lds_payinslipdet.dataobject	= DWO_PAYINSLIPDET
lds_payinslipdet.settransobject( itr_sqlca )
lds_payinslipdet.retrieve( ls_slipcoopid, ls_slipno )

ll_count	= lds_payinslipdet.rowcount()
if ll_count <= 0 then
	ithw_exception.text = "ไม่พบข้อมูลรายละเอียดรายการชำระเงินกู้ที่ต้องการผ่านรายการ เลขที่ใบสั่งจ่าย "+ls_slipno
	lb_error					= true
	goto objdestroy
end if

try
	// ขอเลขที่ Slip Pay Out
	this.of_setsrvdoccontrol( true )
	ls_postdocno	= inv_docsrv.of_getnewdocno( is_coopcontrol, "LCSLIPPPOST" )
catch( Exception lthw_errdoc )
	ithw_exception.text	= lthw_errdoc.text
	lb_error					= true
end try

if lb_error then goto objdestroy

ls_sliptype		= trim( lds_payinslip.getitemstring( 1, "sliptype_code" ) )
ldtm_slipdate	= lds_payinslip.getitemdatetime( 1, "slip_date" )

li_slipstatus		= 1

// ใส่ข้อมูลการผ่านรายการไปที่ Slip (เวลาส่งไป Post จะได้อ่านค่าจาก DS ได้เลย)
lds_payinslip.setitem( 1, "postslipdoc_no", ls_postdocno )
lds_payinslip.setitem( 1, "slip_status", li_slipstatus )

// --------------------------------------------------------
// ปรับปรุงสถานะ Slipแล้วไปทำการผ่านรายการ
// --------------------------------------------------------
update	slslippayin
set			slip_status		= :li_slipstatus,
			postslipdoc_no	= :ls_postdocno
where	( coop_id		= :ls_slipcoopid ) and
			( payinslip_no	= :ls_slipno )
using		itr_sqlca ;

if itr_sqlca.sqlcode <> 0 then
	ithw_exception.text	= "ปรับปรุงสถานะของ Slip จ่ายเงินกู้ให้เป็นผ่านรายการแล้วไม่ได้ " + itr_sqlca.sqlerrtext
	lb_error					= true
	goto objdestroy
end if

// Export ข้อมูลอีกทีเพราะมีการใส่ข้อมูลการผ่านรายการเพิ่มเติม
ls_xmlsliphead		= string( lds_payinslip.describe( "Datawindow.data.XML" ) )

try
	// ผ่านรายการการถอนหุ้น
	this.of_postslip_payinloan( lds_payinslip, lds_payinslipdet )
	
catch( Exception lthw_cclpayin )
	ithw_exception.text	= lthw_cclpayin.text
	lb_error					= true
end try

objdestroy:
destroy lds_payinslip

if lb_error then
	if li_trnsavestatus <> 1 then
		rollback using itr_sqlca ;
	end if
	
	throw ithw_exception
end if

if li_trnsavestatus <> 1 then
	commit using itr_sqlca ;
end if

return 1
end function

public function integer of_saveccl_lnrcv (str_lcslipcancel astr_slipcancel) throws Exception;string		ls_scoopid, ls_slipno, ls_slipclrno, ls_cancelid
long		ll_count, ll_find
integer	li_slipstatus
datetime	ldtm_canceldate, ldtm_slipdate
boolean	lb_error
n_ds		lds_payoutslip, lds_payinslip, lds_payinslipdet, lds_sliplist

ls_cancelid			= astr_slipcancel.cancel_id
ldtm_canceldate	= astr_slipcancel.cancel_date
lb_error				= false

// สร้าง DataStore สำหรับใช้งาน
lds_sliplist		= create n_ds
lds_sliplist.dataobject		= "d_lcsrv_list_ccllnrcv"

lds_payoutslip	= create n_ds
lds_payoutslip.dataobject	= DWO_PAYOUTSLIP
lds_payoutslip.settransobject( itr_sqlca )

lds_payinslip	= create n_ds
lds_payinslip.dataobject	= DWO_PAYINSLIP
lds_payinslip.settransobject( itr_sqlca )

lds_payinslipdet	= create n_ds
lds_payinslipdet.dataobject	= DWO_PAYINSLIPDET
lds_payinslipdet.settransobject( itr_sqlca )

try
	this.of_setsrvdwxmlie( true )
	inv_dwxmliesrv.of_xmlimport( lds_sliplist, astr_slipcancel.xml_sliplist )
catch( Exception lthw_errimp )
	ithw_exception.text	= lthw_errimp.text
	lb_error					= true
end try

if lb_error then goto objdestroy

ll_find		= lds_sliplist.find( "operate_flag = 1", 1, lds_sliplist.rowcount() )

do while ll_find > 0
	ls_scoopid		= lds_sliplist.getitemstring( ll_find, "coop_id" )
	ls_slipno			= lds_sliplist.getitemstring( ll_find, "payoutslip_no" )

	ll_count			= lds_payoutslip.retrieve( ls_scoopid, ls_slipno )
	if ll_count <= 0 then
		ithw_exception.text	= "ไม่พบข้อมูลใบจ่ายเงินกู้ที่ระบุมา เลขที่ใบจ่าย ("+ls_slipno+")"
		lb_error					= true
		goto objdestroy
	end if
	
	ls_slipclrno		= trim( lds_payoutslip.getitemstring( 1, "slipclear_no" ) )
	ldtm_slipdate	= lds_payoutslip.getitemdatetime( 1, "slip_date" )
	
	if ldtm_canceldate = ldtm_slipdate then
		li_slipstatus		= -9
	else
		li_slipstatus		= -99
	end if
	
	// ใส่ข้อมูลการยกเลิกไปที่ Slip PayOut
	lds_payoutslip.setitem( 1, "slip_status", li_slipstatus )
	lds_payoutslip.setitem( 1, "cancel_id", ls_cancelid )
	lds_payoutslip.setitem( 1, "cancel_date", ldtm_canceldate )
	
	// ถ้ามีการหักกลบต้องยกเลิกการหักกลบก่อน
	if ls_slipclrno <> "" and not isnull( ls_slipclrno ) then
		lds_payinslip.retrieve( ls_scoopid, ls_slipclrno )
		lds_payinslipdet.retrieve( ls_scoopid, ls_slipclrno )
		
		if lds_payinslip.rowcount() <= 0 then
			ithw_exception.text	= "ไม่สามารถดึงข้อมูลใบทำรายการหักกลบได้ เลขที่ใบทำรายการ "+ls_slipclrno+" ("+ls_scoopid+")"
			lb_error					= true
			goto objdestroy
		end if
		
		// ใส่ข้อมูลการยกเลิกไปที่ Slip PayIn
		lds_payinslip.setitem( 1, "slip_status", li_slipstatus )
		lds_payinslip.setitem( 1, "cancel_id", ls_cancelid )
		lds_payinslip.setitem( 1, "cancel_date", ldtm_canceldate )
		
		// บันทึก Slip
		update	lcslippayin
		set			slip_status	= :li_slipstatus,
					cancel_id		= :ls_cancelid,
					cancel_date	= :ldtm_canceldate
		where	( coop_id		= :ls_scoopid ) and
					( payinslip_no	= :ls_slipclrno )
		using		itr_sqlca ;
		
		if itr_sqlca.sqlcode <> 0 then
			ithw_exception.text	= "ปรับปรุงสถานะ Slip หักกลบให้เป็นยกเลิกไม่ได้ "+itr_sqlca.sqlerrtext
			lb_error					= true
			goto objdestroy
		end if
		
		try
			this.of_postccl_payinloan( lds_payinslip, lds_payinslipdet )
		catch( Exception lthw_cclpayin )
			ithw_exception.text	= lthw_cclpayin.text
			lb_error					= true
		end try
		
		if lb_error then goto objdestroy
	end if
	
	// ยกเลิกการจ่ายเงินกู้
	update	lcslippayout
	set			slip_status	= :li_slipstatus,
				cancel_id		= :ls_cancelid,
				cancel_date	= :ldtm_canceldate
	where	( coop_id		= :ls_scoopid ) and
				( payoutslip_no	= :ls_slipno )
	using		itr_sqlca ;
	
	if itr_sqlca.sqlcode <> 0 then
		ithw_exception.text	= "ปรับปรุงสถานะ Slip การจ่ายเงินกู้ให้เป็นยกเลิกไม่ได้ "+itr_sqlca.sqlerrtext
		lb_error					= true
		goto objdestroy
	end if
	
	// ผ่านรายการยกเลิกการจ่ายเงินกู้
	try
		this.of_postccl_lnrcv( lds_payoutslip )
	catch( Exception lthw_cclpayout )
		ithw_exception.text	= lthw_cclpayout.text
		lb_error					= true
	end try
	
	ll_find ++
	if ll_find > lds_sliplist.rowcount() then exit
	ll_find		= lds_sliplist.find( "operate_flag = 1", ll_find, lds_sliplist.rowcount() )
loop

objdestroy:
destroy lds_payoutslip
destroy lds_payinslip
destroy lds_payinslipdet
destroy lds_sliplist
this.of_setsrvdwxmlie( false )

if lb_error then
	rollback using itr_sqlca ;
	throw ithw_exception
end if

commit using itr_sqlca ;

return 1
end function

public function integer of_initlnrcv (ref str_lcslippayout astr_lcpayout) throws Exception;string		ls_concoopid, ls_contno
integer	li_lastrcv
dec		ldc_payoutamt, ldc_payoutclr, ldc_payoutnet
datetime	ldtm_slipdate, ldtm_opedate

ls_concoopid	= astr_lcpayout.concoop_id
ls_contno		= astr_lcpayout.loancontract_no
ldtm_slipdate	= astr_lcpayout.slip_date
ldtm_opedate	= astr_lcpayout.operate_date

n_ds		lds_infocont, lds_lnrcv, lds_payloan

lds_infocont		= create n_ds
lds_infocont.dataobject	= "d_lcsrv_slippayoutcontdet"
lds_infocont.settransobject( itr_sqlca )
lds_infocont.retrieve( ls_concoopid, ls_contno )

if lds_infocont.rowcount() <= 0 then
	ithw_exception.text	= "ไม่พบข้อมูลสัญญาที่ต้องการจ่ายเงินกู้ เลขที่สัญญา "+ls_contno+" กรุณาตรวจสอบ"
	destroy lds_infocont
	throw ithw_exception
end if

lds_lnrcv			= create n_ds
lds_lnrcv.dataobject	= DWO_PAYOUTSLIP

li_lastrcv			= lds_infocont.getitemnumber( 1, "last_periodrcv" )
ldc_payoutamt	= lds_infocont.getitemdecimal( 1, "withdrawable_amt" )

if isnull( li_lastrcv ) then li_lastrcv = 0

li_lastrcv ++

// ใส่รายละเอียดข้อมูล
lds_lnrcv.insertrow( 0 )

lds_lnrcv.setitem( 1, "sliptype_code", "LWD" )
lds_lnrcv.setitem( 1, "concoop_id", ls_concoopid )
lds_lnrcv.setitem( 1, "loancontract_no", ls_contno )
lds_lnrcv.setitem( 1, "rcv_period", li_lastrcv )
lds_lnrcv.setitem( 1, "rcvperiod_flag", 0 )
lds_lnrcv.setitem( 1, "rcvfromreqcont_code", "CON" )
lds_lnrcv.setitem( 1, "slip_date", ldtm_slipdate )
lds_lnrcv.setitem( 1, "operate_date", ldtm_opedate )

// Clr ค่าตั้งต้น ด/บ ก่อนเสมอ
lds_lnrcv.setitem( 1, "prncalint_amt", 0 )
lds_lnrcv.setitem( 1, "interest_period", 0 )
lds_lnrcv.setitem( 1, "calint_from", ldtm_opedate )
lds_lnrcv.setitem( 1, "calint_from", ldtm_opedate )

lds_lnrcv.object.member_no[ 1 ]			= lds_infocont.object.member_no[ 1 ]
lds_lnrcv.object.memcoop_id[ 1 ]		= lds_infocont.object.memcoop_id[ 1 ]
lds_lnrcv.object.shrlontype_code[ 1 ]		= lds_infocont.object.loantype_code[ 1 ]
lds_lnrcv.object.loanrequest_docno[ 1 ]	= lds_infocont.object.loanrequest_docno[ 1 ]
lds_lnrcv.object.bfperiod[ 1 ]				= lds_infocont.object.last_periodrcv[ 1 ]
lds_lnrcv.object.bfloanapprove_amt[ 1 ]	= lds_infocont.object.loanapprove_amt[ 1 ]
lds_lnrcv.object.bfshrcont_balamt[ 1 ]	= lds_infocont.object.principal_balance[ 1 ]
lds_lnrcv.object.bfwithdraw_amt[ 1 ]		= lds_infocont.object.withdrawable_amt[ 1 ]
lds_lnrcv.object.bfinterest_arrear[ 1 ]		= lds_infocont.object.interest_arrear[ 1 ]
lds_lnrcv.object.bflastcalint_date[ 1 ]		= lds_infocont.object.lastcalint_date[ 1 ]
lds_lnrcv.object.bflastreceive_date[ 1 ]	= lds_infocont.object.lastreceive_date[ 1 ]
lds_lnrcv.object.bfcontlaw_status[ 1 ]		= lds_infocont.object.contlaw_status[ 1 ]
lds_lnrcv.object.payout_amt[ 1 ]			= lds_infocont.object.withdrawable_amt[ 1 ]

// ถ้าไม่ใช่งวดแรกส่งไปตั้ง ด/บ ค้าง
if li_lastrcv > 1 then
	this.of_initlnrcv_calint( lds_lnrcv )
end if

// รายการหักชำระ
lds_payloan		= create n_ds
lds_payloan.dataobject		= DWO_PAYINSLIPDET

this.of_initlnrcv_clrloan( lds_lnrcv, lds_payloan )

// หายอดหักชำระ
ldc_payoutclr	= dec( lds_payloan.describe( "evaluate( 'sum( item_payamt for all )', 1 )" ) )
ldc_payoutnet	= ldc_payoutamt - ldc_payoutclr

lds_lnrcv.setitem( 1, "payoutclr_amt", ldc_payoutclr )
lds_lnrcv.setitem( 1, "payoutnet_amt", ldc_payoutnet )

this.of_setsrvdwxmlie( true )
astr_lcpayout.xml_slipmemdet		= inv_dwxmliesrv.of_xmlexport( lds_infocont )
astr_lcpayout.xml_sliphead			= inv_dwxmliesrv.of_xmlexport( lds_lnrcv )
astr_lcpayout.xml_slipcutlon			= inv_dwxmliesrv.of_xmlexport( lds_payloan )
astr_lcpayout.xml_slipcutetc			= ""
astr_lcpayout.xml_expense			= ""
this.of_setsrvdwxmlie( false )

destroy lds_lnrcv
destroy lds_payloan
destroy lds_infocont

return 1
end function

private function integer of_poststm_contract (str_lcpoststmloan astr_lnstatement) throws Exception;string		ls_contno, ls_contcoopid, ls_itemcode, ls_refdocno, ls_refslipno
string		ls_entryid, ls_entrycoopid, ls_remark
integer	li_period, li_itemstatus, li_lastseqno
dec{2}	ldc_prnpay, ldc_intpay, ldc_prnbal, ldc_prncalint, ldc_intperiod
dec{2}	ldc_bfintarr, ldc_intarrbal
datetime	ldtm_slipdate, ldtm_opedate, ldtm_accdate, ldtm_intaccdate, ldtm_entrydate
datetime	ldtm_calintfrom, ldtm_calintto

ls_contno		= astr_lnstatement.loancontract_no
ls_contcoopid	= astr_lnstatement.concoop_id
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
ldc_intperiod	= astr_lnstatement.interest_period
ldc_intarrbal		= astr_lnstatement.interest_arrear
li_itemstatus	= astr_lnstatement.item_status
ls_entryid		= astr_lnstatement.entry_id
ls_entrycoopid	= astr_lnstatement.entry_bycoopid
ls_remark		= astr_lnstatement.remark

ldtm_entrydate	= datetime( today(), now() )

select		last_stm_no
into		:li_lastseqno
from		lccontmaster
where	( loancontract_no	= :ls_contno ) and
			( coop_id			= :ls_contcoopid ) 
using		itr_sqlca ;

if itr_sqlca.sqlcode <> 0 then
	ithw_exception.text	+= "of_poststm_contract ไม่พบเลขที่สัญญาที่ระบุ "+ls_contno+" กรุณาตรวจสอบ ~r~n"+itr_sqlca.sqlerrtext
	throw ithw_exception
end if

if isnull( li_lastseqno ) then li_lastseqno = 0

li_lastseqno ++

// เพิ่มรายการเคลื่อนไหวการชำระหนี้
insert into lccontstatement
			( loancontract_no,		coop_id,				seq_no,					slip_date,				operate_date,			account_date,			intaccum_date,
			  ref_slipno,				ref_docno,				loanitemtype_code,	period,		 			principal_payment,	interest_payment,		principal_balance,		
			  prncalint_amt,		calint_from,				calint_to,					bfintarrear_amt,		interest_period,		interest_arrear,	
			  item_status,			entry_id,					entry_date,				entry_bycoopid )
values	( :ls_contno,			:ls_contcoopid,			:li_lastseqno,			:ldtm_slipdate,			:ldtm_opedate,			:ldtm_accdate,			:ldtm_intaccdate,
			  :ls_refslipno,			:ls_refdocno,			:ls_itemcode,			:li_period,				:ldc_prnpay,				:ldc_intpay,				:ldc_prnbal,
			  :ldc_prncalint,			:ldtm_calintfrom,		:ldtm_calintto,			:ldc_bfintarr,			:ldc_intperiod,			:ldc_intarrbal,			
			  :li_itemstatus,			:ls_entryid,				:ldtm_entrydate,		:ls_entrycoopid )
using		itr_sqlca ;

if itr_sqlca.sqlcode <> 0 then
	ithw_exception.text	+= "of_poststm_contract  ไม่สามารถเพิ่มรายการ Statement หนี้ "+ls_contno+" ได้ กรุณาตรวจสอบ ~r~n"+itr_sqlca.sqlerrtext
	throw ithw_exception
end if

update	lccontmaster
set			last_stm_no		= :li_lastseqno
where	( loancontract_no	= :ls_contno )
using		itr_sqlca ;

if itr_sqlca.sqlcode <> 0 then
	ithw_exception.text	+= "of_poststm_contract  ไม่สามารถปรับปรุงลำดับที่ล่าสุดได้ เลขที่สัญญา "+ls_contno+" กรุณาตรวจสอบ ~r~n"+itr_sqlca.sqlerrtext
	throw ithw_exception
end if

// ตรวจสอบว่าถ้าเป็นรายการยกเลิกให้ไปปรับรายการคู่กันให้ยกเลิกด้วย
if li_itemstatus <> 1 then
	update	lccontstatement
	set			item_status	= :li_itemstatus
	where	( loancontract_no	= :ls_contno ) and
				( ref_slipno	= :ls_refslipno )
	using		itr_sqlca ;
	
	if itr_sqlca.sqlcode <> 0 then
		ithw_exception.text	+= "of_poststm_contract  ไม่สามารถปรับปรุงสถานะ Statement รายการคู่กันได้ เลขที่สัญญา "+ls_contno+" เลขที่ Slip "+ls_refslipno+" กรุณาตรวจสอบ ~r~n"+itr_sqlca.sqlerrtext
		throw ithw_exception
	end if
end if

return 1
end function

public function integer of_initpayin (ref str_lcslippayin astr_lcpayin) throws Exception;string		ls_memcoopid, ls_memno, ls_sliptype, ls_xmlmemdet, ls_refslipno, ls_finbankacc
datetime	ldtm_slipdate, ldtm_opedate
boolean	lb_error = false
n_ds		lds_payin, lds_payinloan, lds_payinexp

ls_memcoopid	= astr_lcpayin.memcoop_id
ls_memno		= astr_lcpayin.member_no
ls_sliptype		= astr_lcpayin.sliptype_code
ldtm_slipdate	= astr_lcpayin.slip_date
ldtm_opedate	= astr_lcpayin.operate_date
ls_refslipno		= astr_lcpayin.ref_slipno

if isnull( ls_memno ) then ls_memno = ""
if isnull( ls_memcoopid ) then ls_memcoopid = ""

ls_xmlmemdet		= this.of_getmembdetail( ls_memcoopid, ls_memno )

if ls_xmlmemdet = "" then
	ithw_exception.text = "ไม่พบข้อมูลของสมาชิกเลขที่ " + ls_memno +" สาขา("+ls_memcoopid+") กรุณาตรวจสอบ"
	throw ithw_exception
end if

if ls_sliptype = "LPM" then
	select		noticedue_date, finbankacc_no
	into		:ldtm_opedate, :ls_finbankacc
	from		lcnoticemthrecv
	where	( coop_id	= :ls_memcoopid ) and
				( notice_docno	= :ls_refslipno )
	using		itr_sqlca ;
	
	if itr_sqlca.sqlcode <> 0 then
		ldtm_opedate		= astr_lcpayin.operate_date
	end if
end if

// สร้าง DataStore สำหรับใช้งาน
lds_payin			= create n_ds
lds_payin.dataobject	= DWO_PAYINSLIP

lds_payinloan	= create n_ds
lds_payinloan.dataobject	= DWO_PAYINSLIPDET

lds_payinexp	= create n_ds
lds_payinexp.dataobject	= DWO_PAYINEXP

lds_payin.insertrow( 0 )

lds_payin.setitem( 1, "member_no", ls_memno )
lds_payin.setitem( 1, "memcoop_id", ls_memcoopid )

lds_payin.setitem( 1, "sliptype_code", ls_sliptype )
lds_payin.setitem( 1, "slip_date", ldtm_slipdate )
lds_payin.setitem( 1, "operate_date", ldtm_opedate )
lds_payin.setitem( 1, "ref_slipno", ls_refslipno )

try
	this.of_initpayin_loan( lds_payinloan, ls_memcoopid, ls_memno, ls_sliptype )
	this.of_calintlnpayment( lds_payinloan, ldtm_opedate )
	
	if ls_sliptype = "LPM" then
		this.of_setpayfromnotice( lds_payin, lds_payinloan, ls_memcoopid, ls_refslipno )
		
		lds_payinexp.insertrow( 0 )
		lds_payinexp.setitem( 1, "moneytype_code", 'CBT' )
		lds_payinexp.setitem( 1, "finbankacc_no", ls_finbankacc )
		lds_payinexp.setitem( 1, "tofrom_accid", '' )
		lds_payinexp.setitem( 1, "expense_amt", lds_payin.getitemdecimal( 1, "slip_amt" ) )
		
	end if
catch( Exception lthw_error )
	ithw_exception.text	= lthw_error.text
	lb_error					= true
end try

if lb_error then goto objdestroy

this.of_setsrvdwxmlie( true )
astr_lcpayin.xml_memdet	= ls_xmlmemdet
astr_lcpayin.xml_sliphead	= inv_dwxmliesrv.of_xmlexport( lds_payin )
astr_lcpayin.xml_sliplon		= inv_dwxmliesrv.of_xmlexport( lds_payinloan )
astr_lcpayin.xml_slipetc		= ""
astr_lcpayin.xml_expense	= inv_dwxmliesrv.of_xmlexport( lds_payinexp )
this.of_setsrvdwxmlie( false )

objdestroy:
destroy lds_payin
destroy lds_payinloan
destroy lds_payinexp

if lb_error then
	throw ithw_exception
end if

return 1
end function

public function integer of_getloanpayment (ref n_ds ads_payloan, string as_memcoopid, string as_memno, string as_sliptype, datetime adtm_operate) throws Exception;// สำหรับให้ระบบอื่นๆเรียกใช้ว่ามีสัญญาอะไรที่ต้องชำระบ้าง
integer		li_ret

li_ret	= this.of_initpayin_loan( ads_payloan, as_memcoopid, as_memno, as_sliptype )

li_ret	= this.of_calintlnpayment( ads_payloan, adtm_operate )

return li_ret
end function

public function integer of_calintlnpayment (ref n_ds ads_payinloan, datetime adtm_opedate) throws Exception;string		ls_contno, ls_concoopid, ls_itemtype
long		ll_count, ll_index
integer	li_operate
dec{2}	ldc_prnbal, ldc_prnpay, ldc_intamt, ldc_bfintarr, ldc_intpay
datetime	ldtm_bflastcalint, ldtm_calintto

// ประกาศ Service interest
this.of_setsrvlcinterest( true )

ll_count	= ads_payinloan.rowcount()

for ll_index = 1 to ll_count
	ls_itemtype			= ads_payinloan.getitemstring( ll_index, "slipitemtype_code" )
	
	if ls_itemtype <> "LON" then
		continue
	end if
	
	// ดึงข้อมูลสำหรับคำนวณ
	ls_concoopid		= ads_payinloan.getitemstring( ll_index, "concoop_id" )
	ls_contno			= ads_payinloan.getitemstring( ll_index, "loancontract_no" )
	ldc_prnbal			= ads_payinloan.getitemdecimal( ll_index, "bfshrcont_balamt" )
	ldtm_bflastcalint	= ads_payinloan.getitemdatetime( ll_index, "bflastcalint_date" )
	
	// Clear ข้อมูลดอกเบี้ยก่อนเสมอ
	ads_payinloan.setitem( ll_index, "prncalint_amt", 0 )
	ads_payinloan.setitem( ll_index, "interest_period", 0 )
	ads_payinloan.setitem( ll_index, "calint_to", ldtm_bflastcalint )
	
	ldtm_calintto		= adtm_opedate
	
	// คำนวณดอกเบี้ย
	ldc_intamt		= inv_intsrv.of_computeinterest( ls_concoopid, ls_contno, ldc_prnbal, ldtm_bflastcalint, ldtm_calintto )
	
	// ใส่ข้อมูล ด/บ
	ads_payinloan.setitem( ll_index, "prncalint_amt", ldc_prnbal )
	ads_payinloan.setitem( ll_index, "calint_from", ldtm_bflastcalint )
	ads_payinloan.setitem( ll_index, "calint_to", ldtm_calintto )
	ads_payinloan.setitem( ll_index, "interest_period", ldc_intamt )
	
	// ถ้ามีการ Check ว่าชำระ
	li_operate		= ads_payinloan.getitemnumber( ll_index, "operate_flag" )
	if li_operate = 1 then
		ldc_prnpay		= ads_payinloan.getitemdecimal( ll_index, "principal_payamt" )
		ldc_bfintarr		= ads_payinloan.getitemdecimal( ll_index, "bfintarr_amt" )
		
		ldc_intpay		= ldc_bfintarr + ldc_intamt
		
		ads_payinloan.setitem( ll_index, "interest_payamt", ldc_intpay )
		ads_payinloan.setitem( ll_index, "item_payamt", ldc_prnpay + ldc_intpay )
	end if
next

this.of_setsrvlcinterest( false )

return 1
end function

public function integer of_saveccl_payin (str_lcslipcancel astr_slipcancel) throws Exception;string		ls_scoopid, ls_slipno, ls_cancelid, ls_sliptype, ls_refmthnotice
long		ll_count, ll_find
integer	li_slipstatus
datetime	ldtm_canceldate, ldtm_slipdate
boolean	lb_error
n_ds		lds_payinslip, lds_payinslipdet, lds_sliplist

ls_cancelid			= astr_slipcancel.cancel_id
ldtm_canceldate	= astr_slipcancel.cancel_date
lb_error				= false

// สร้าง DataStore สำหรับใช้งาน
lds_sliplist		= create n_ds
lds_sliplist.dataobject		= "d_lcsrv_list_cclpayin"

lds_payinslip	= create n_ds
lds_payinslip.dataobject	= DWO_PAYINSLIP
lds_payinslip.settransobject( itr_sqlca )

lds_payinslipdet	= create n_ds
lds_payinslipdet.dataobject	= DWO_PAYINSLIPDET
lds_payinslipdet.settransobject( itr_sqlca )

try
	this.of_setsrvdwxmlie( true )
	inv_dwxmliesrv.of_xmlimport( lds_sliplist, astr_slipcancel.xml_sliplist )
catch( Exception lthw_errimp )
	ithw_exception.text	= lthw_errimp.text
	lb_error					= true
end try

if lb_error then goto objdestroy

ll_find		= lds_sliplist.find( "operate_flag = 1", 1, lds_sliplist.rowcount() )

do while ll_find > 0
	ls_scoopid		= lds_sliplist.getitemstring( ll_find, "coop_id" )
	ls_slipno			= lds_sliplist.getitemstring( ll_find, "payinslip_no" )

	lds_payinslip.retrieve( ls_scoopid, ls_slipno )
	lds_payinslipdet.retrieve( ls_scoopid, ls_slipno )
	
	ll_count		= lds_payinslip.rowcount()
	
	if ll_count <= 0 then
		ithw_exception.text	= "ไม่สามารถดึงข้อมูลใบทำรายการรับชำระได้ เลขที่ใบทำรายการ "+ls_slipno+" ("+ls_scoopid+")"
		lb_error					= true
		goto objdestroy
	end if
	
	ldtm_slipdate		= lds_payinslip.getitemdatetime( 1, "slip_date" )
	ls_sliptype			= lds_payinslip.getitemstring( 1, "sliptype_code" )
	ls_refmthnotice		= lds_payinslip.getitemstring( 1, "ref_slipno" )
	
	if ldtm_canceldate = ldtm_slipdate then
		li_slipstatus		= -9
	else
		li_slipstatus		= -99
	end if
	
	// ใส่ข้อมูลการยกเลิกไปที่ Slip PayIn
	lds_payinslip.setitem( 1, "slip_status", li_slipstatus )
	lds_payinslip.setitem( 1, "cancel_id", ls_cancelid )
	lds_payinslip.setitem( 1, "cancel_date", ldtm_canceldate )
	
	// บันทึก Slip
	update	lcslippayin
	set			slip_status	= :li_slipstatus,
				cancel_id		= :ls_cancelid,
				cancel_date	= :ldtm_canceldate
	where	( coop_id		= :ls_scoopid ) and
				( payinslip_no	= :ls_slipno )
	using		itr_sqlca ;
	
	if itr_sqlca.sqlcode <> 0 then
		ithw_exception.text	= "ปรับปรุงสถานะ Slip รับชำระให้เป็นยกเลิกไม่ได้ "+itr_sqlca.sqlerrtext
		lb_error					= true
		goto objdestroy
	end if
	
	try
		this.of_postccl_payinloan( lds_payinslip, lds_payinslipdet )
		
	catch( Exception lthw_cclpayin )
		ithw_exception.text	= lthw_cclpayin.text
		lb_error					= true
	end try

	if lb_error then goto objdestroy
	
	if ls_sliptype = "LPM" then
		update	lcnoticemthrecv
		set			notice_status	= 8
		where	( coop_id		= :ls_scoopid )
		and		( notice_docno	= :ls_refmthnotice )
		using		itr_sqlca ;
		
		if itr_sqlca.sqlcode <> 0 then
			ithw_exception.text	= "ปรับปรุงสถานะรายการแจ้งหนี้ไม่ได้ เลขที่ใบแจ้งหนี้ "+ls_refmthnotice+" "+itr_sqlca.sqlerrtext
			lb_error					= true
			goto objdestroy
		end if
	end if
	
	ll_find ++
	if ll_find > lds_sliplist.rowcount() then exit
	ll_find		= lds_sliplist.find( "operate_flag = 1", ll_find, lds_sliplist.rowcount() )

loop

objdestroy:
destroy lds_payinslip
destroy lds_payinslipdet
destroy lds_sliplist

if lb_error then
	rollback using itr_sqlca ;
	throw ithw_exception
end if

commit using itr_sqlca ;

return 1
end function

public function integer of_initccl_lnrcv (ref str_lcslipcancel astr_slipcancel) throws Exception;string		ls_mcoopid, ls_memno, ls_xmlmemdet
long		ll_count
datetime	ldtm_canceldate
n_ds		lds_temp

ls_mcoopid			= astr_slipcancel.memcoop_id
ls_memno			= astr_slipcancel.member_no
ldtm_canceldate	= astr_slipcancel.cancel_date

ls_xmlmemdet		= this.of_getmembdetail( ls_mcoopid, ls_memno )

if ls_xmlmemdet = "" then
	return 0
end if

this.of_setsrvdwxmlie( true )

lds_temp		= create n_ds
lds_temp.dataobject	= "d_lcsrv_list_ccllnrcv"
lds_temp.settransobject( itr_sqlca )

lds_temp.retrieve( ls_mcoopid, ls_memno, ldtm_canceldate )

astr_slipcancel.xml_memdet	= ls_xmlmemdet
astr_slipcancel.xml_sliplist		= inv_dwxmliesrv.of_xmlexport( lds_temp )

destroy( lds_temp )

this.of_setsrvdwxmlie( false )

return 1
end function

public function integer of_initccl_payinloan (ref str_lcslipcancel astr_slipcancel) throws Exception;string		ls_mcoopid, ls_memno, ls_xmlmemdet
long		ll_count
datetime	ldtm_canceldate
n_ds		lds_temp

ls_mcoopid			= astr_slipcancel.memcoop_id
ls_memno			= astr_slipcancel.member_no
ldtm_canceldate	= astr_slipcancel.cancel_date

ls_xmlmemdet		= this.of_getmembdetail( ls_mcoopid, ls_memno )

if ls_xmlmemdet = "" then
	return 0
end if

this.of_setsrvdwxmlie( true )

lds_temp		= create n_ds
lds_temp.dataobject	= "d_lcsrv_list_cclpayin"
lds_temp.settransobject( itr_sqlca )

lds_temp.retrieve( ls_mcoopid, ls_memno, ldtm_canceldate )

astr_slipcancel.xml_memdet	= ls_xmlmemdet
astr_slipcancel.xml_sliplist		= inv_dwxmliesrv.of_xmlexport( lds_temp )

destroy( lds_temp )

this.of_setsrvdwxmlie( false )

return 1
end function

private function integer of_initlnrcv_clrloan (n_ds ads_lnrcv, ref n_ds ads_payinloan) throws Exception;
string		ls_ccoopid, ls_lnreqno, ls_contrcv, ls_contclr, ls_memcoopid, ls_memno, ls_sliptype
long		ll_find
integer	li_rcvperiod
dec{2}	ldc_clramt, ldc_prnbal, ldc_intperiod, ldc_intarrear, ldc_prnpay, ldc_intpay
datetime	ldtm_slipdate

ls_ccoopid		= ads_lnrcv.getitemstring( 1, "concoop_id" )
ls_lnreqno		= ads_lnrcv.getitemstring( 1, "loanrequest_docno" )
ls_contrcv		= ads_lnrcv.getitemstring( 1, "loancontract_no" )
ls_memcoopid	= ads_lnrcv.getitemstring( 1, "memcoop_id" )
ls_memno		= ads_lnrcv.getitemstring( 1, "member_no" )
ls_sliptype		= ads_lnrcv.getitemstring( 1, "sliptype_code" )
li_rcvperiod		= ads_lnrcv.getitemnumber( 1, "rcv_period" )

// การจ่ายเงินกู้เอาวันที่ Slip เป็นหลัก
ldtm_slipdate	= ads_lnrcv.getitemdatetime( 1, "slip_date" )

this.of_initpayin_loan( ads_payinloan, ls_memcoopid, ls_memno, "CLC" )

// ถ้าเป็นการรับเงินกู้งวดแรก และรับจากสัญญา ต้องตัดสัญญาตัวเองออก
if li_rcvperiod = 1 and trim( ls_contrcv ) <> "" and not isnull( ls_contrcv ) then
	ll_find		= ads_payinloan.find( "concoop_id = '"+ls_ccoopid+"' and loancontract_no = '"+ls_contrcv+"'", 1, ads_payinloan.rowcount() )
	
	if ll_find > 0 then
		ads_payinloan.rowsdiscard( ll_find, ll_find, primary! )
	end if
end if

// คำนวณดอกเบี้ย
this.of_calintlnpayment( ads_payinloan, ldtm_slipdate )

if ls_lnreqno = "" then
	return 1
end if

// เอาใบคำขอไปดึงข้อมูลว่าตอนขอกู้เลือกหักชำระสัญญาไหนบ้าง
declare data_cur cursor for
select		concoop_id, loancontract_no, clear_amount
from		lcreqloanclr
where	( coop_id				= :ls_ccoopid ) and
			( loanrequest_docno	= :ls_lnreqno ) and
			( clear_status			= 1 )
using		itr_sqlca ;

open data_cur ;
	fetch data_cur into :ls_ccoopid, :ls_contclr, :ldc_clramt ;
	do while itr_sqlca.sqlcode = 0
		
		ll_find		= ads_payinloan.find( "loancontract_no = '"+ls_contclr+"' and concoop_id = '"+ls_ccoopid+"'", 1, ads_payinloan.rowcount() )
		
		if ll_find > 0 then
			
			ldc_prnbal		= ads_payinloan.getitemdecimal( ll_find, "bfshrcont_balamt" )
			ldc_intperiod	= ads_payinloan.getitemdecimal( ll_find, "interest_period" )
			ldc_intarrear	= ads_payinloan.getitemdecimal( ll_find, "bfintarr_amt" )
			
			if isnull( ldc_prnbal ) then ldc_prnbal = 0
			if isnull( ldc_intperiod ) then ldc_intperiod = 0
			if isnull( ldc_intarrear ) then ldc_intarrear = 0

			ldc_prnpay	= ldc_clramt
			ldc_intpay	= ldc_intperiod + ldc_intarrear
			
			ads_payinloan.setitem( ll_find, "operate_flag", 1 )
			ads_payinloan.setitem( ll_find, "principal_payamt", ldc_prnpay )
			ads_payinloan.setitem( ll_find, "interest_payamt", ldc_intpay )
			ads_payinloan.setitem( ll_find, "item_payamt", ldc_prnpay+ldc_intpay )
			ads_payinloan.setitem( ll_find, "item_balance", ldc_prnbal - ldc_prnpay )
			
		end if
		
		fetch data_cur into :ls_ccoopid, :ls_contclr, :ldc_clramt ;
	loop
close data_cur ;

return 1
end function

private function integer of_postccl_lnrcv (n_ds ads_payoutslip) throws Exception;string			ls_contno, ls_ccoopid, ls_sliptype, ls_cclloncode
string			ls_cancelid
integer		li_bfperiod
dec{2}		ldc_prnrcv, ldc_contprnbal, ldc_contintarr
dec{2}		ldc_prnbal, ldc_bfprnbal, ldc_contwtdbal, ldc_bfintarr
datetime		ldtm_slipdate, ldtm_opedate, ldtm_canceldate
datetime		ldtm_contlastcalint, ldtm_bflastrcv, ldtm_bflastcalint
str_lcpoststmloan	lstr_lnstm

ls_sliptype			= ads_payoutslip.getitemstring( 1, "sliptype_code" )
ls_ccoopid			= ads_payoutslip.getitemstring( 1, "concoop_id" )
ls_contno			= ads_payoutslip.getitemstring( 1, "loancontract_no" )
li_bfperiod			= ads_payoutslip.getitemnumber( 1, "bfperiod" )
ldc_prnrcv			= ads_payoutslip.getitemdecimal( 1, "payout_amt" )
ldc_bfprnbal			= ads_payoutslip.getitemdecimal( 1, "bfshrcont_balamt" )
ldc_bfintarr			= ads_payoutslip.getitemdecimal( 1, "bfinterest_arrear" )
ldtm_slipdate		= ads_payoutslip.getitemdatetime( 1, "slip_date" )
ldtm_opedate		= ads_payoutslip.getitemdatetime( 1, "operate_date" )
ldtm_bflastcalint	= ads_payoutslip.getitemdatetime( 1, "bflastcalint_date" )
ldtm_bflastrcv		= ads_payoutslip.getitemdatetime( 1, "bflastreceive_date" )

ls_cancelid			= ads_payoutslip.getitemstring( 1, "cancel_id" )
ldtm_canceldate	= ads_payoutslip.getitemdatetime( 1, "cancel_date" )

// รหัสการย้อนรายการใน STM
select		clnstm_itemtype
into		:ls_cclloncode
from		lcucfsliptype
where	( sliptype_code	= :ls_sliptype )
using		itr_sqlca ;

select		withdrawable_amt, principal_balance, interest_arrear,
			lastcalint_date
into		:ldc_contwtdbal, :ldc_contprnbal, :ldc_contintarr,
			:ldtm_contlastcalint
from		lccontmaster
where	( coop_id			= :ls_ccoopid ) and
			( loancontract_no	= :ls_contno )
using		itr_sqlca ;

if itr_sqlca.sqlcode <> 0 then
	ithw_exception.text		= "ไม่พบข้อมูลสัญญาที่ระบุมา "+ls_contno+" กรุณาตรวจสอบ"
	throw ithw_exception
end if

// ตรวจสอบว่ายอดต้องยังไม่วิ่งถ้าวิ่งไปแล้วยกเลิกไม่ได้
if ( ldc_bfprnbal + ldc_prnrcv ) <> ldc_contprnbal and ldtm_slipdate <> ldtm_contlastcalint then
	ithw_exception.text		= "สัญญาเงินกู้เลขที่ "+ls_contno+" มีการทำรายการอย่างอื่นไปแล้ว ไม่สามารถยกเลิกการจ่ายเงินกู้ได้ กรุณาตรวจสอบ"
	throw ithw_exception
end if

ldc_prnbal			= ldc_contprnbal - ldc_prnrcv
ldc_contwtdbal		= ldc_contwtdbal + ldc_prnrcv

// เอายอดยกเลิกไปลดใน Master
update	lccontmaster
set			principal_balance	= :ldc_bfprnbal,
			withdrawable_amt	= :ldc_contwtdbal,
			last_periodrcv		= :li_bfperiod,
			interest_arrear		= :ldc_bfintarr,
			lastcalint_date		= :ldtm_bflastcalint,
			lastreceive_date	= :ldtm_bflastrcv
where	( coop_id			= :ls_ccoopid ) and
			( loancontract_no	= :ls_contno )
using		itr_sqlca ;
if itr_sqlca.sqlcode <> 0 then
	ithw_exception.text	= "ไม่สามารถปรับปรุงสัญญาส่วนของการยกเลิกการจ่ายเงินกู้ได้~r~n"+itr_sqlca.sqlerrtext
	throw ithw_exception
end if

// บันทึกลง Statement
lstr_lnstm.concoop_id					= ls_ccoopid
lstr_lnstm.loancontract_no			= ls_contno
lstr_lnstm.slip_date					= ldtm_slipdate
lstr_lnstm.operate_date				= ldtm_canceldate
lstr_lnstm.account_date				= ldtm_canceldate
lstr_lnstm.intaccum_date				= ldtm_slipdate
lstr_lnstm.ref_slipno					= ads_payoutslip.getitemstring( 1, "payoutslip_no" )
lstr_lnstm.ref_docno					= ads_payoutslip.getitemstring( 1, "document_no" )
lstr_lnstm.loanitemtype_code		= ls_cclloncode
lstr_lnstm.period						= ads_payoutslip.getitemnumber( 1, "rcv_period" )
lstr_lnstm.principal_payment		= ldc_prnrcv
lstr_lnstm.interest_payment			= 0
lstr_lnstm.principal_balance			= ldc_prnbal
lstr_lnstm.prncalint_amt				= ads_payoutslip.getitemdecimal( 1, "prncalint_amt" )
lstr_lnstm.calint_from					= ads_payoutslip.getitemdatetime( 1, "calint_from" )
lstr_lnstm.calint_to					= ads_payoutslip.getitemdatetime( 1, "calint_to" )
lstr_lnstm.bfinterest_arrear			= ldc_contintarr
lstr_lnstm.interest_period			= ads_payoutslip.getitemdecimal( 1, "interest_period" )
lstr_lnstm.interest_arrear			= ldc_bfintarr
lstr_lnstm.item_status				= -9
lstr_lnstm.entry_id						= ls_cancelid

this.of_poststm_contract( lstr_lnstm )

return 1
end function

private function integer of_postccl_payinloan (n_ds ads_payinslip, n_ds ads_payinslipdet) throws Exception;string			ls_ccoopid, ls_contno, ls_sliptype, ls_cclloncode
long			ll_index, ll_count
integer		li_contstatus, li_odflag, li_stmstatus, li_bfperiod
dec{2}		ldc_prnpay, ldc_intpay, ldc_contprnbal, ldc_contintarr, ldc_contintacc
dec{2}		ldc_prnbal, ldc_wtdbal, ldc_bfintarramt, ldc_bfprnbalamt
datetime		ldtm_slipdate, ldtm_opedate, ldtm_contlastcalint, ldtm_contlastpay
datetime		ldtm_calintto, ldtm_bflastcalint, ldtm_bflastpay
string			ls_cancelid
datetime		ldtm_canceldate
str_lcpoststmloan	lstr_lnstm

ls_sliptype			= ads_payinslip.getitemstring( 1, "sliptype_code" )
ldtm_slipdate		= ads_payinslip.getitemdatetime( 1, "slip_date" )
ldtm_opedate		= ads_payinslip.getitemdatetime( 1, "operate_date" )

ls_cancelid			= ads_payinslip.getitemstring( 1, "cancel_id" )
ldtm_canceldate	= ads_payinslip.getitemdatetime( 1, "cancel_date" )

// ทำแต่รายการที่เป็นการชำระหนี้
ads_payinslipdet.setfilter( "slipitemtype_code = 'LON'" )
ads_payinslipdet.filter()

ll_count	= ads_payinslipdet.rowcount()
if ll_count <= 0 then
	return 0
end if

// รหัสการย้อนรายการใน STM
select		clnstm_itemtype
into		:ls_cclloncode
from		lcucfsliptype
where	( sliptype_code	= :ls_sliptype )
using		itr_sqlca ;

for ll_index = 1 to ll_count
	ls_ccoopid				= ads_payinslipdet.getitemstring( ll_index, "concoop_id" )
	ls_contno				= ads_payinslipdet.getitemstring( ll_index, "loancontract_no" )
	
	ldc_prnpay				= ads_payinslipdet.getitemdecimal( ll_index, "principal_payamt" )
	ldc_intpay				= ads_payinslipdet.getitemdecimal( ll_index, "interest_payamt" )
	li_bfperiod				= ads_payinslipdet.getitemnumber( ll_index, "bfperiod" )
	ldc_bfintarramt			= ads_payinslipdet.getitemdecimal( ll_index, "bfintarr_amt" )
	ldc_bfprnbalamt		= ads_payinslipdet.getitemdecimal( ll_index, "bfshrcont_balamt" )
	ldtm_bflastcalint		= ads_payinslipdet.getitemdatetime( ll_index, "bflastcalint_date" )
	ldtm_bflastpay			= ads_payinslipdet.getitemdatetime( ll_index, "bflastpay_date" )
	ldtm_calintto			= ads_payinslipdet.getitemdatetime( ll_index, "calint_to" )

	if isnull( ldc_prnpay ) then ldc_prnpay = 0
	if isnull( ldc_intpay ) then ldc_intpay = 0
	
	select		withdrawable_amt, principal_balance, interest_arrear, interest_accum,
				od_flag, lastcalint_date, lastpayment_date
	into		:ldc_wtdbal, :ldc_contprnbal, :ldc_contintarr, :ldc_contintacc,
				:li_odflag, :ldtm_contlastcalint, :ldtm_contlastpay
	from		lccontmaster
	where	( coop_id			= :ls_ccoopid ) and
				( loancontract_no	= :ls_contno )
	using		itr_sqlca ;
	
	if itr_sqlca.sqlcode <> 0 then
		ithw_exception.text		= "ไม่พบข้อมูลสัญญาที่ระบุมา "+ls_contno+" กรุณาตรวจสอบ"
		throw ithw_exception
	end if
	
	ldc_contintarr			= ldc_bfintarramt
	ldtm_contlastcalint		= ldtm_bflastcalint
	ldtm_contlastpay		= ldtm_bflastpay
	
	// ต้นเงินเพิ่ม ด/บ สะสมลด
	ldc_prnbal		= ldc_contprnbal + ldc_prnpay
	ldc_contintacc	= ldc_contintacc - ldc_intpay
	
	// ตรวจว่าเป็นสัญญา OD หรือเปล่า
	if li_odflag = 1 then
		ldc_wtdbal	= ldc_wtdbal + ldc_prnpay
	end if
	
	// สถานะสัญญาต้องเป็นปกติเสมอเพราะเป็นการยกเลิกการชำระ
	li_contstatus		= 1
	
	// เอายอดยกเลิกไปเพิ่มใส่ Master
	update	lccontmaster
	set			withdrawable_amt	= :ldc_wtdbal,
				principal_balance	= :ldc_prnbal,
				last_periodpay		= :li_bfperiod,	
				interest_arrear		= :ldc_bfintarramt,
				lastcalint_date		= :ldtm_bflastcalint,
				lastpayment_date	= :ldtm_bflastpay,
				interest_accum		= :ldc_contintacc,
				intpayment_amt	= intpayment_amt - :ldc_intpay,
				contract_status		= :li_contstatus
	where	( coop_id			= :ls_ccoopid ) and
				( loancontract_no	= :ls_contno )
	using		itr_sqlca ;
	if itr_sqlca.sqlcode <> 0 then
		ithw_exception.text	= "ไม่สามารถปรับปรุงสัญญาส่วนของการยกเลิกการชำระได้~r~n"+itr_sqlca.sqlerrtext
		throw ithw_exception
	end if
	
	if ldtm_slipdate = ldtm_canceldate then
		li_stmstatus		= -9
	else
		li_stmstatus		= -99
	end if
	
	// บันทึกลง Statement
	lstr_lnstm.concoop_id					= ls_ccoopid
	lstr_lnstm.loancontract_no			= ls_contno
	lstr_lnstm.slip_date					= ldtm_slipdate
	lstr_lnstm.operate_date				= ldtm_canceldate
	lstr_lnstm.account_date				= ldtm_canceldate
	lstr_lnstm.intaccum_date				= ldtm_opedate
	lstr_lnstm.ref_slipno					= ads_payinslip.getitemstring( 1, "payinslip_no" )
	lstr_lnstm.ref_docno					= ads_payinslip.getitemstring( 1, "document_no" )
	lstr_lnstm.loanitemtype_code		= ls_cclloncode
	lstr_lnstm.period						= ads_payinslipdet.getitemnumber( ll_index, "period" )
	lstr_lnstm.principal_payment		= ldc_prnpay
	lstr_lnstm.interest_payment			= ldc_intpay
	lstr_lnstm.principal_balance			= ldc_prnbal
	lstr_lnstm.prncalint_amt				= ads_payinslipdet.getitemdecimal( ll_index, "prncalint_amt" )
	lstr_lnstm.calint_from					= ads_payinslipdet.getitemdatetime( ll_index, "calint_from" )
	lstr_lnstm.calint_to					= ads_payinslipdet.getitemdatetime( ll_index, "calint_to" )
	lstr_lnstm.bfinterest_arrear			= ldc_contintarr
	lstr_lnstm.interest_period			= ads_payinslipdet.getitemdecimal( ll_index, "interest_period" )
	lstr_lnstm.interest_arrear			= ldc_contintarr
	lstr_lnstm.item_status				= li_stmstatus
	lstr_lnstm.entry_id						= ls_cancelid
	lstr_lnstm.entry_bycoopid			= ""

	this.of_poststm_contract( lstr_lnstm )
next

return 1
end function

public function integer of_saveslip_lnrcv (ref str_lcslippayout astr_lcpayout) throws Exception;string		ls_sliptype, ls_slipitemcode, ls_initfrom, ls_contrcvcoopid, ls_contrcvno, ls_contclrcoopid, ls_contclrno
string		ls_entryid, ls_ecoopid, ls_payoutslipno, ls_payoutdocno, ls_payindocno, ls_postdocno
string		ls_memcoopid, ls_memno
long		ll_index, ll_count, ll_row
integer	li_orderflag, li_slipstatus
dec{2}	ldc_payamt, ldc_clramt, ldc_netamt, ldc_prnpay, ldc_intpay
dec{2}	ldc_bfcontrcvbal, ldc_bfcontclrbal, ldc_bfcontrcvintarr, ldc_contrcvintperiod, ldc_bfcontclrintarr
boolean	lb_clearitem, lb_error
datetime	ldtm_entrydate, ldtm_orderdate
datetime	ldtm_slipdate

n_ds		lds_slippayout, lds_slippayoutexp, lds_slippayin, lds_slippayindet

li_orderflag		= astr_lcpayout.order_flag
ls_entryid		= astr_lcpayout.entry_id
ls_ecoopid		= astr_lcpayout.entry_bycoopid
ldtm_entrydate	= datetime( today(), now() )

lb_clearitem		= false
lb_error			= false
ldc_clramt		= 0

lds_slippayout	= create n_ds
lds_slippayout.dataobject	= DWO_PAYOUTSLIP
lds_slippayout.settransobject( itr_sqlca )

lds_slippayoutexp	= create n_ds
lds_slippayoutexp.dataobject	= DWO_PAYOUTEXP
lds_slippayoutexp.settransobject( itr_sqlca )

lds_slippayin	= create n_ds
lds_slippayin.dataobject		= DWO_PAYINSLIP
lds_slippayin.settransobject( itr_sqlca )

lds_slippayindet	= create n_ds
lds_slippayindet.dataobject	= DWO_PAYINSLIPDET
lds_slippayindet.settransobject( itr_sqlca )

// Import ข้อมูล
try
	this.of_setsrvdwxmlie( true )
	inv_dwxmliesrv.of_xmlimport( lds_slippayout, astr_lcpayout.xml_sliphead )

	// Import ประเภทเงินที่จะจ่าย slip payout
	inv_dwxmliesrv.of_xmlimport( lds_slippayoutexp, astr_lcpayout.xml_expense )
	
	// Import รายการ
	inv_dwxmliesrv.of_xmlimport( lds_slippayindet, astr_lcpayout.xml_slipcutlon )
	inv_dwxmliesrv.of_xmlimport( lds_slippayindet, astr_lcpayout.xml_slipcutetc )
catch( Exception lthw_errimp )
	ithw_exception.text	= lthw_errimp.text
	lb_error					= true
end try

if lb_error then goto objdestroy

// รายละเอียดสัญญาที่รับเงิน
ls_contrcvcoopid	= lds_slippayout.getitemstring( 1, "concoop_id" )
ls_contrcvno			= lds_slippayout.getitemstring( 1, "loancontract_no" )
ldc_payamt			= lds_slippayout.getitemdecimal( 1, "payout_amt" )

// กรองให้เหลือแต่พวกที่ทำรายการเท่านั้น
lds_slippayindet.setfilter( "operate_flag > 0" )
lds_slippayindet.filter()

// ลบพวกไม่ทำรายการทิ้งไป
lds_slippayindet.rowsdiscard( 1, lds_slippayindet.filteredcount(), filter! )

// ถ้ามีแถว Clear ต้องออก Slip Payin
if lds_slippayindet.rowcount() > 0 then
	lb_clearitem		= true
end if

try
	// ขอเลขที่ Slip Pay Out
	this.of_setsrvdoccontrol( true )
	ls_payoutslipno		= inv_docsrv.of_getnewdocno( is_coopcontrol, "LCSLIPPAYOUT" )
	
	if li_orderflag = 1 then
		ls_payoutdocno		= ""
		ls_postdocno		= ""
		li_slipstatus			= 8
	else
		ls_payoutdocno		= inv_docsrv.of_getnewdocno( is_coopcontrol, "LCSLIPPAYOUTDOC" )
		ls_postdocno		= inv_docsrv.of_getnewdocno( is_coopcontrol, "LCSLIPPPOST" )
		li_slipstatus			= 1
	end if
catch( Exception lthw_errdoc )
	ithw_exception.text	= lthw_errdoc.text
	lb_error					= true
end try

if lb_error then goto objdestroy

// คืนค่าใบจ่ายกลับไป
astr_lcpayout.payoutslip_no		= ls_payoutslipno

// ใส่เลขที่ Slip ใน Header PayOut
lds_slippayout.setitem( 1, "coop_id", is_coopcontrol )
lds_slippayout.setitem( 1, "payoutslip_no", ls_payoutslipno )
lds_slippayout.setitem( 1, "document_no", ls_payoutdocno )
lds_slippayout.setitem( 1, "postslipdoc_no", ls_postdocno )
lds_slippayout.setitem( 1, "slip_status", li_slipstatus )
lds_slippayout.setitem( 1, "finpost_status", 0 )
lds_slippayout.setitem( 1, "entry_id", ls_entryid )
lds_slippayout.setitem( 1, "entry_bycoopid", ls_ecoopid )
lds_slippayout.setitem( 1, "entry_date", ldtm_entrydate )

// ใส่เลขที่ Slip ใน Expense Payout
for ll_index = 1 to lds_slippayoutexp.rowcount()
	lds_slippayoutexp.setitem( ll_index, "coop_id", is_coopcontrol )
	lds_slippayoutexp.setitem( ll_index, "payoutslip_no", ls_payoutslipno )
	lds_slippayoutexp.setitem( ll_index, "seq_no", ll_index )
next

// ถ้ามีการหักชำระ
if lb_clearitem then
	try
		ls_memcoopid		= lds_slippayout.getitemstring( 1, "memcoop_id" )
		ls_memno			= lds_slippayout.getitemstring( 1, "member_no" )
		ldtm_slipdate		= lds_slippayout.getitemdatetime( 1, "slip_date" )
		
		if li_orderflag = 1 then
			ls_payindocno		= ""
			ls_postdocno		= ""
		else
			ls_payindocno		= inv_docsrv.of_getnewdocno( is_coopcontrol, "LCSLIPPAYINDOC" )
			ls_postdocno		= inv_docsrv.of_getnewdocno( is_coopcontrol, "LCSLIPPPOST" )
		end if
	catch( Exception lthw_errdocpay )
		ithw_exception.text	= lthw_errdocpay.text
		lb_error					= true
	end try
	
	if lb_error then goto objdestroy
		
	lds_slippayin.insertrow( 0 )
	
	// ใส่เลขที่ Slip ใน Header PayIn
	lds_slippayin.setitem( 1, "coop_id", is_coopcontrol )
	lds_slippayin.setitem( 1, "payinslip_no", ls_payoutslipno )
	lds_slippayin.setitem( 1, "document_no", ls_payindocno )
	lds_slippayin.setitem( 1, "postslipdoc_no", ls_postdocno )
	lds_slippayin.setitem( 1, "member_no", ls_memno )
	lds_slippayin.setitem( 1, "memcoop_id", ls_memcoopid )
	lds_slippayin.setitem( 1, "sliptype_code", "CLC" )
	lds_slippayin.setitem( 1, "slip_date", ldtm_slipdate )
	lds_slippayin.setitem( 1, "operate_date", ldtm_slipdate )
	
	lds_slippayin.setitem( 1, "slip_status", li_slipstatus )
	lds_slippayin.setitem( 1, "entry_id", ls_entryid )
	lds_slippayin.setitem( 1, "entry_bycoopid", ls_ecoopid )
	lds_slippayin.setitem( 1, "entry_date", ldtm_entrydate )
	
	// ใส่เลขที่ Slip ในรายละเอียดของการชำระ
	ll_count		= lds_slippayindet.rowcount()
	
	for ll_index = 1 to ll_count
		lds_slippayindet.setitem( ll_index, "coop_id", is_coopcontrol )
		lds_slippayindet.setitem( ll_index, "payinslip_no", ls_payoutslipno )
		lds_slippayindet.setitem( ll_index, "seq_no", ll_index )

		// พ่วงการตรวจสอบว่ามีการหักชำระสัญญาที่รับเงิน(สัญญาตัวเอง)มั้ย ถ้าหักต้อง clear ค่าที่เกี่ยวกับการคิดดอกเบี้ยทิ้ง
		ls_contclrcoopid	= lds_slippayindet.getitemstring( ll_index, "concoop_id" )
		ls_contclrno			= lds_slippayindet.getitemstring( ll_index, "loancontract_no" )
		
		if ls_contrcvcoopid = ls_contclrcoopid and ls_contrcvno = ls_contclrno then
			ldc_bfcontrcvbal		= lds_slippayout.getitemdecimal( 1, "bfshrcont_balamt" )
			ldc_bfcontrcvintarr		= lds_slippayout.getitemdecimal( 1, "bfinterest_arrear" )
			ldc_contrcvintperiod	= lds_slippayout.getitemdecimal( 1, "interest_period" )
			
			ldc_bfcontclrbal			= ldc_bfcontrcvbal + ldc_payamt
			ldc_bfcontclrintarr		= ldc_bfcontrcvintarr + ldc_contrcvintperiod
			
			lds_slippayindet.setitem( ll_index, "bfshrcont_balamt", ldc_bfcontclrbal )
			lds_slippayindet.setitem( ll_index, "bfintarr_amt", ldc_bfcontclrintarr )
			lds_slippayindet.setitem( ll_index, "bflastcalint_date", ldtm_slipdate )
			
//			lds_slippayindet.setitem( ll_index, "prncalint_amt", ldc_bfcontclrbal )
//			lds_slippayindet.setitem( ll_index, "calint_from", ldtm_slipdate )
//			lds_slippayindet.setitem( ll_index, "calint_to", ldtm_slipdate )
			lds_slippayindet.setitem( ll_index, "interest_period", 0 )
		end if
		
		// พ่วงการปรับยอดการชำระกันในกรณีหลุดจาก UI
		ls_slipitemcode		= lds_slippayindet.getitemstring( ll_index, "slipitemtype_code" )
		
		if ls_slipitemcode = "LON" or ls_slipitemcode = "INF" then
			ldc_bfcontclrbal	= lds_slippayindet.getitemdecimal( ll_index, "bfshrcont_balamt" )
			ldc_prnpay		= lds_slippayindet.getitemdecimal( ll_index, "principal_payamt" )
			ldc_intpay		= lds_slippayindet.getitemdecimal( ll_index, "interest_payamt" )
			
			if isnull( ldc_prnpay ) then ldc_prnpay	 = 0
			if isnull( ldc_intpay ) then ldc_intpay = 0
			
			lds_slippayindet.setitem( ll_index, "item_payamt", ldc_prnpay + ldc_intpay )
			lds_slippayindet.setitem( ll_index, "item_balance", ldc_bfcontclrbal - ldc_prnpay )
		end if
	next
	
	// ยอดชำระทั้งหมด
	if ll_count > 0 then
		ldc_clramt	= dec( lds_slippayindet.describe( "evaluate( 'sum( if( operate_flag = 1, item_payamt, 0 ) for all )', "+string( ll_count )+" )" ) )
	end if
	
	// ใส่เลขที่ใบชำระให้ใบจ่าย
	lds_slippayout.setitem( 1, "slipclear_no", ls_payoutslipno )
	
	// ใส่ยอดเงินในใบหักกลบ
	lds_slippayin.setitem( 1, "slip_amt", ldc_clramt )

	// คืนค่าใบหักชำระกลับไป
	astr_lcpayout.payinslip_no		= ls_payoutslipno

end if

ldc_netamt		= ldc_payamt - ldc_clramt

lds_slippayout.setitem( 1, "payoutclr_amt", ldc_clramt )
lds_slippayout.setitem( 1, "payoutnet_amt", ldc_netamt )

// --------------------------------------------------------
// เริ่มกระบวนการบันทึกรายการ
// --------------------------------------------------------

// บันทึก Slip Pay Out
if lds_slippayout.update() <> 1 then
	ithw_exception.text	= "บันทึกข้อมูล Slip LONRCV ไม่ได้"
	ithw_exception.text	+= lds_slippayout.of_geterrormessage()
	lb_error					= true
	goto objdestroy
end if

// บันทึก Slip Pay Out Expense
if lds_slippayoutexp.update() <> 1 then
	ithw_exception.text	= "บันทึกข้อมูล Slip LONRCV (ส่วน Expense) ไม่ได้"
	ithw_exception.text	+= lds_slippayoutexp.of_geterrormessage()
	lb_error					= true
	goto objdestroy
end if

// บันทึก Slip Pay In
if lds_slippayin.update() <> 1 then
	ithw_exception.text	= "บันทึกข้อมูล Slip PayIn ไม่ได้"
	ithw_exception.text	+= lds_slippayin.of_geterrormessage()
	lb_error					= true
	goto objdestroy
end if

// บันทึก Slip Pay Out Clear
if lds_slippayindet.update() <> 1 then
	ithw_exception.text	= "บันทึกข้อมูล Slip PayIn Detail ไม่ได้"
	ithw_exception.text	+= lds_slippayindet.of_geterrormessage()
	lb_error					= true
	goto objdestroy
end if

// ถ้าไม่ใช่ใบสั่งให้เริ่มกระบวนการผ่านรายการ
if li_orderflag <> 1 then
	try
		this.of_postslip_lnrcv( lds_slippayout )
		
		if lb_clearitem then
			this.of_postslip_payinloan( lds_slippayin, lds_slippayindet )	
		end if
	catch( Exception lthw_errpost )
		ithw_exception.text	= lthw_errpost.text
		lb_error					= true
	end try
end if

objdestroy:
destroy lds_slippayout
destroy lds_slippayoutexp
destroy lds_slippayin
destroy lds_slippayindet

this.of_setsrvdoccontrol( false )
this.of_setsrvdwxmlie( false )

if lb_error then
	rollback using itr_sqlca ;
	throw ithw_exception
end if

commit using itr_sqlca ;

return 1
end function

public function integer of_saveslip_payin (ref str_lcslippayin astr_lnslip) throws Exception;string		ls_payinslipno, ls_slipitemcode, ls_payindocno, ls_postdocno
string		ls_entryid, ls_ecoopid
long		ll_count, ll_index, ll_seqno
integer	li_orderflag, li_slipstatus
dec{2}	ldc_bfshrcontbal, ldc_prnpay, ldc_intpay, ldc_sumpayamt
datetime	ldtm_entrydate, ldtm_orderdate
boolean	lb_error
n_ds		lds_slippayin, lds_slippayindet, lds_slipexpense

li_orderflag		= astr_lnslip.order_flag
ls_entryid		= astr_lnslip.entry_id
ls_ecoopid		= astr_lnslip.entry_bycoopid
ldtm_entrydate	= datetime( today(), now() )

lb_error			= false

// Import หัว slip
lds_slippayin		= create n_ds
lds_slippayin.dataobject	= DWO_PAYINSLIP
lds_slippayin.settransobject( itr_sqlca )

lds_slippayindet	= create n_ds
lds_slippayindet.dataobject	= DWO_PAYINSLIPDET
lds_slippayindet.settransobject( itr_sqlca )

lds_slipexpense		= create n_ds
lds_slipexpense.dataobject	= DWO_PAYINEXP
lds_slipexpense.settransobject( itr_sqlca )

try
	this.of_setsrvdwxmlie( true )
	// Import หัว slip
	inv_dwxmliesrv.of_xmlimport( lds_slippayin, astr_lnslip.xml_sliphead )
	
	// Import รายการ
	inv_dwxmliesrv.of_xmlimport( lds_slippayindet, astr_lnslip.xml_sliplon )
	inv_dwxmliesrv.of_xmlimport( lds_slippayindet, astr_lnslip.xml_slipetc )

	// Import expense
	inv_dwxmliesrv.of_xmlimport( lds_slipexpense, astr_lnslip.xml_expense )
catch( Exception lthw_errimp )
	ithw_exception.text	= lthw_errimp.text
	lb_error					= true
end try

if lb_error then
	goto objdestroy
end if

// กรองให้เหลือแต่พวกที่ทำรายการเท่านั้น
lds_slippayindet.setfilter( "operate_flag > 0" )
lds_slippayindet.filter()

// ลบพวกไม่ทำรายการทิ้งไป
lds_slippayindet.rowsdiscard( 1, lds_slippayindet.filteredcount(), filter! )

// ตรวจเช็คจำนวนแถว
ll_count		= lds_slippayindet.rowcount()
if ll_count <= 0 then
	ithw_exception.text	= "ไม่พบข้อมูลสำหรับบันทึกรายการรับชำระหนี้ กรุณาตรวจสอบ"
	lb_error					= true
	goto objdestroy
end if

// ใส่ข้อมูลการชำระให้ครบถ้วน
for ll_index = 1 to ll_count
	ls_slipitemcode		= lds_slippayindet.getitemstring( ll_index, "slipitemtype_code" )
	
	if ls_slipitemcode = "LON" or ls_slipitemcode = "INF" then
		ldc_bfshrcontbal	= lds_slippayindet.getitemdecimal( ll_index, "bfshrcont_balamt" )
		ldc_prnpay			= lds_slippayindet.getitemdecimal( ll_index, "principal_payamt" )
		ldc_intpay			= lds_slippayindet.getitemdecimal( ll_index, "interest_payamt" )
		
		if ldc_prnpay > ldc_bfshrcontbal then
			ithw_exception.text	= "ไม่อนุญาติให้ยอดชำระเงินต้นมากกว่ายอดเงินคงเหลือ กรุณาตรวจสอบ"
			throw ithw_exception
		end if
		
		if isnull( ldc_prnpay ) then ldc_prnpay	 = 0
		if isnull( ldc_intpay ) then ldc_intpay = 0
		
		lds_slippayindet.setitem( ll_index, "item_payamt", ldc_prnpay + ldc_intpay )
		lds_slippayindet.setitem( ll_index, "item_balance", ldc_bfshrcontbal - ldc_prnpay )
	end if
next

// เรียงตามประเภทรายการ
lds_slippayindet.setsort( "slipitemtype_code asc" )
lds_slippayindet.sort()

// ยอดชำระทั้งหมด
ldc_sumpayamt		= dec( lds_slippayindet.describe( "evaluate( 'sum( if( operate_flag = 1, item_payamt, 0 ) for all )', "+string( ll_count )+" )" ) )

if ldc_sumpayamt <= 0 then
	ithw_exception.text	= "ไม่มียอดเงินรับชำระ กรุณาตรวจสอบ"
	lb_error					= true
	goto objdestroy
end if

try
	// ขอเลขที่ใบสั่งชำระ, เลขที่ใบ slip
	this.of_setsrvdoccontrol( true )
	ls_payinslipno	= inv_docsrv.of_getnewdocno( is_coopcontrol, "LCSLIPPAYIN" )
	
	if li_orderflag = 1 then
		ls_payindocno		= ""
		ls_postdocno		= ""
		li_slipstatus			= 8
	else
		ls_postdocno		= inv_docsrv.of_getnewdocno( ls_ecoopid, "LCSLIPPPOST" )
		ls_payindocno		= inv_docsrv.of_getnewdocno( ls_ecoopid, "LCSLIPPAYINDOC" )
		li_slipstatus			= 1
	end if
	
	this.of_setsrvdoccontrol( false )
catch( Exception lthw_errdoc )
	ithw_exception.text	= lthw_errdoc.text
	lb_error					= true
end try

if lb_error then goto objdestroy

// ใส่เลขที่ Slip ใน Header
lds_slippayin.setitem( 1, "coop_id", is_coopcontrol )
lds_slippayin.setitem( 1, "payinslip_no", ls_payinslipno )
lds_slippayin.setitem( 1, "postslipdoc_no", ls_postdocno )
lds_slippayin.setitem( 1, "document_no", ls_payindocno )

lds_slippayin.setitem( 1, "slip_amt", ldc_sumpayamt )
lds_slippayin.setitem( 1, "slip_status", li_slipstatus )
lds_slippayin.setitem( 1, "finpost_status", 0 )
lds_slippayin.setitem( 1, "entry_id", ls_entryid )
lds_slippayin.setitem( 1, "entry_bycoopid", ls_ecoopid )
lds_slippayin.setitem( 1, "entry_date", ldtm_entrydate )

// ใส่เลขที่ Slip และลำดับที่ใน detail
ll_count	= lds_slippayindet.rowcount()
for ll_index = 1 to ll_count
	ll_seqno ++
	
	lds_slippayindet.setitem( ll_index, "coop_id", is_coopcontrol )
	lds_slippayindet.setitem( ll_index, "payinslip_no", ls_payinslipno )
	lds_slippayindet.setitem( ll_index, "seq_no", ll_seqno )
next

// ใส่เลขที่ Slip และลำดับที่ใน Expense
ll_count	= lds_slipexpense.rowcount()
for ll_index = 1 to ll_count
	lds_slipexpense.setitem( ll_index, "coop_id", is_coopcontrol )
	lds_slipexpense.setitem( ll_index, "payinslip_no", ls_payinslipno )
	lds_slipexpense.setitem( ll_index, "seq_no", ll_index )
next

// ------------------------------------------
// เริ่มกระบวนการบันทึกรายการ
// ------------------------------------------
// บันทึก Slip
if lds_slippayin.update() <> 1 then
	ithw_exception.text	= "บันทึกข้อมูลใบชำระหนี้ Slip ไม่ได้"
	ithw_exception.text	+= lds_slippayin.of_geterrormessage()
	lb_error					= true
	goto objdestroy
end if

// บันทึก Slip Detail
if lds_slippayindet.update() <> 1 then
	ithw_exception.text	= "บันทึกข้อมูลใบชำระหนี้ัี Slip Detail ไม่ได้"
	ithw_exception.text	+= lds_slippayindet.of_geterrormessage()
	lb_error					= true
	goto objdestroy
end if

// บันทึก Slip Expense
if lds_slipexpense.update() <> 1 then
	ithw_exception.text	= "บันทึกข้อมูลใบชำระหนี้ Slip Expense ไม่ได้"
	ithw_exception.text	+= lds_slipexpense.of_geterrormessage()
	lb_error					= true
	goto objdestroy
end if

// ต้องไม่ใช่ใบสั่ง เริ่มกระบวนการผ่านรายการ
if li_orderflag <> 1 then
	try
		this.of_postslip_payinloan( lds_slippayin, lds_slippayindet )	
	catch( Exception lthw_errpost )
		ithw_exception.text	= lthw_errpost.text
		lb_error					= true
	end try
	
	if lb_error then goto objdestroy
end if

objdestroy:
if isvalid( lds_slippayin ) then destroy lds_slippayin
if isvalid( lds_slippayindet ) then destroy lds_slippayindet
if isvalid( lds_slipexpense ) then destroy lds_slipexpense

this.of_setsrvdwxmlie( false )

if lb_error then
	rollback using itr_sqlca ;
	throw ithw_exception
end if

// ถ้ามาถึงตรงนี้ได้แสดงว่าไม่เจออะไรผิดพลาด Commit ไปเลย
commit using itr_sqlca ;

// ส่งค่าเลขที่ใบสั่งกลับ
astr_lnslip.payinslip_no	= ls_payinslipno

return 1
end function

public function integer of_getmemblnrcvlist (ref str_lclnrcvlist astr_lclnrcvlist) throws Exception;string		ls_memno, ls_contno, ls_memcoopid, ls_concoopid
long		ll_count
n_ds		lds_lnrcv

ls_memcoopid		= astr_lclnrcvlist.memcoop_id
ls_memno			= astr_lclnrcvlist.member_no

astr_lclnrcvlist.concoop_id		= ""
astr_lclnrcvlist.loancontract_no	= ""
astr_lclnrcvlist.xml_list			= ""

lds_lnrcv		= create n_ds
lds_lnrcv.dataobject	= "d_lcsrv_list_memcont"
lds_lnrcv.settransobject( itr_sqlca )

ll_count		= lds_lnrcv.retrieve( ls_memcoopid, ls_memno )

if ll_count = 1 then
	astr_lclnrcvlist.concoop_id		= lds_lnrcv.getitemstring( 1, "coop_id" )
	astr_lclnrcvlist.loancontract_no	= lds_lnrcv.getitemstring( 1, "loancontract_no" )
elseif ll_count > 1 then
	this.of_setsrvdwxmlie( true )
	astr_lclnrcvlist.xml_list			= inv_dwxmliesrv.of_xmlexport( lds_lnrcv )
	this.of_setsrvdwxmlie( false )
end if

destroy lds_lnrcv

return ll_count
end function

private function integer of_initpayin_loan (ref n_ds ads_payloan, string as_memcoopid, string as_memno, string as_sliptype) throws Exception;string		ls_itemcode, ls_desc
long		ll_count, ll_index, ll_row
dec{2}	ldc_prnbal, ldc_intarrear
n_ds		lds_infocont

// สร้าง DataStore สำหรับใช้งาน
lds_infocont	= create n_ds
lds_infocont.dataobject	= "d_lcsrv_info_memcont"
lds_infocont.settransobject( itr_sqlca )

// กำหนดค่าต่าง ๆ
select		lnstm_itemtype, lnpay_desc
into		:ls_itemcode, :ls_desc
from		lcucfsliptype
where	( sliptype_code		= :as_sliptype )
using		itr_sqlca ;

// รายละเอียดสัญญาเงินกู้
ll_count		= lds_infocont.retrieve( as_memcoopid, as_memno )

for ll_index = 1 to ll_count
	ldc_prnbal			= lds_infocont.getitemdecimal( ll_index, "principal_balance" )
	ldc_intarrear		= lds_infocont.getitemdecimal( ll_index, "interest_arrear" )
	
	if isnull( ldc_prnbal ) then ldc_prnbal = 0
	if isnull( ldc_intarrear ) then ldc_intarrear = 0
	
	// ถ้าเป็นพวกสัญญารอเบิกให้ข้ามไป
	if ldc_prnbal <= 0 and ldc_intarrear <= 0 then continue
	
	ll_row	= ads_payloan.insertrow( 0 )
	
	ads_payloan.object.concoop_id[ ll_row ]			= lds_infocont.object.coop_id[ ll_index ]
	ads_payloan.object.loancontract_no[ ll_row ]		= lds_infocont.object.loancontract_no[ ll_index ]
	ads_payloan.object.shrlontype_code[ ll_row ]		= lds_infocont.object.loantype_code[ ll_index ]
	ads_payloan.object.period[ ll_row ]					= lds_infocont.object.last_periodpay[ ll_index ]
	ads_payloan.object.calint_from[ ll_row ]				= lds_infocont.object.lastcalint_date[ ll_index ]
	
	ads_payloan.object.bfperiod[ ll_row ]					= lds_infocont.object.last_periodpay[ ll_index ]
	ads_payloan.object.bfintarr_amt[ ll_row ]			= lds_infocont.object.interest_arrear[ ll_index ]
	ads_payloan.object.bfintarrset_amt[ ll_row ]		= lds_infocont.object.intset_arrear[ ll_index ]
	ads_payloan.object.bfwithdraw_amt[ ll_row ]		= lds_infocont.object.withdrawable_amt[ ll_index ]
	ads_payloan.object.bfperiod_payment[ ll_row ]	= lds_infocont.object.period_payment[ ll_index ]
	ads_payloan.object.bfshrcont_status[ ll_row ]		= lds_infocont.object.contract_status[ ll_index ]
	ads_payloan.object.bfcontlaw_status[ ll_row ]		= lds_infocont.object.contlaw_status[ ll_index ]
	ads_payloan.object.bflastcalint_date[ ll_row ]		= lds_infocont.object.lastcalint_date[ ll_index ]
	ads_payloan.object.bflastpay_date[ ll_row ]			= lds_infocont.object.lastpayment_date[ ll_index ]
	
	ads_payloan.setitem( ll_row, "operate_flag", 0 )
	ads_payloan.setitem( ll_row, "seq_no", ll_row )
	ads_payloan.setitem( ll_row, "slipitemtype_code", "LON" )
	ads_payloan.setitem( ll_row, "slipitem_desc", ls_desc )
	
	ads_payloan.setitem( ll_row, "principal_payamt", 0 )
	ads_payloan.setitem( ll_row, "interest_payamt", 0 )
	ads_payloan.setitem( ll_row, "item_payamt", 0 )
	ads_payloan.setitem( ll_row, "item_balance", ldc_prnbal )
	
	ads_payloan.setitem( ll_row, "bfshrcont_balamt", ldc_prnbal )
	ads_payloan.setitem( ll_row, "stm_itemtype", ls_itemcode )
next

destroy lds_infocont

return 1
end function

public function integer of_getmembnoticemthlist (ref str_lcnoticemthlist astr_lcnoticemthlist) throws Exception;string		ls_memno, ls_contno, ls_memcoopid, ls_concoopid
long		ll_count
n_ds		lds_noticelist

ls_memcoopid		= astr_lcnoticemthlist.memcoop_id
ls_memno			= astr_lcnoticemthlist.member_no

astr_lcnoticemthlist.noticecoop_id	= ""
astr_lcnoticemthlist.notice_docno	= ""
astr_lcnoticemthlist.xml_list			= ""

lds_noticelist		= create n_ds
lds_noticelist.dataobject	= "d_lcsrv_list_noticemth"
lds_noticelist.settransobject( itr_sqlca )

ll_count		= lds_noticelist.retrieve( ls_memcoopid, ls_memno )

if ll_count = 1 then
	astr_lcnoticemthlist.noticecoop_id		= lds_noticelist.getitemstring( 1, "coop_id" )
	astr_lcnoticemthlist.notice_docno		= lds_noticelist.getitemstring( 1, "notice_docno" )
elseif ll_count > 1 then
	this.of_setsrvdwxmlie( true )
	astr_lcnoticemthlist.xml_list				= inv_dwxmliesrv.of_xmlexport( lds_noticelist )
	this.of_setsrvdwxmlie( false )
end if

destroy lds_noticelist

return ll_count
end function

public function integer of_setpayfromnotice (ref n_ds ads_payinslip, ref n_ds ads_payinsliploan, string as_noticecoopid, string as_noticedocno);string		ls_ccoopid, ls_contno
long		ll_index, ll_count, ll_find
integer	li_period
dec		ldc_prnpay, ldc_intpay, ldc_itempay, ldc_itembal, ldc_noticeamt
dec		ldc_intperiod, ldc_intarrear, ldc_bfprnbal, ldc_slipamt
n_ds		lds_noticemthdet

lds_noticemthdet	= create n_ds
lds_noticemthdet.dataobject		= "d_lcsrv_noticemthrecvdet"
lds_noticemthdet.settransobject( itr_sqlca )

lds_noticemthdet.retrieve( as_noticecoopid, as_noticedocno )

ll_count		= lds_noticemthdet.rowcount()

if ll_count <= 0 then
	destroy lds_noticemthdet
	return 1
end if

for ll_index = 1 to ll_count
	ls_ccoopid		= lds_noticemthdet.getitemstring( ll_index, "concoop_id" )
	ls_contno		= lds_noticemthdet.getitemstring( ll_index, "loancontract_no" )
	li_period			= lds_noticemthdet.getitemnumber( ll_index, "period" )
	ldc_noticeamt	= lds_noticemthdet.getitemdecimal( ll_index, "item_payment" )
	
	ll_find		= ads_payinsliploan.find( "concoop_id = '"+ls_ccoopid+"' and loancontract_no = '"+ls_contno+"'", 1, ads_payinsliploan.rowcount() )
	
	if ll_find <= 0 then continue
	
	ldc_intperiod	= ads_payinsliploan.getitemdecimal( ll_find, "interest_period" )
	ldc_intarrear	= ads_payinsliploan.getitemdecimal( ll_find, "bfintarr_amt" )
	ldc_bfprnbal		= ads_payinsliploan.getitemdecimal( ll_find, "bfshrcont_balamt" )
	
	if ldc_noticeamt > ( ldc_intperiod + ldc_intarrear ) then
		ldc_intpay		= ldc_intperiod + ldc_intarrear
	else
		ldc_intpay		= ldc_noticeamt
	end if
	
	ldc_prnpay		= ldc_noticeamt - ldc_intpay
	ldc_itempay		= ldc_prnpay + ldc_intpay
	ldc_itembal		= ldc_bfprnbal - ldc_prnpay
	ldc_slipamt		= ldc_slipamt + ldc_itempay
	
	ads_payinsliploan.setitem( ll_find, "operate_flag", 1 )
	ads_payinsliploan.setitem( ll_find, "period", li_period )
	ads_payinsliploan.setitem( ll_find, "principal_payamt", ldc_prnpay )
	ads_payinsliploan.setitem( ll_find, "interest_payamt", ldc_intpay )
	ads_payinsliploan.setitem( ll_find, "item_payamt", ldc_itempay )
	ads_payinsliploan.setitem( ll_find, "item_balance", ldc_itembal )
	
next

ads_payinslip.setitem( 1, "slip_amt", ldc_slipamt )

destroy lds_noticemthdet

return 1
end function

public function integer of_initlnrcv_recalint (ref str_lcslippayout astr_slip) throws Exception;string		ls_xmlhead, ls_xmlloan, ls_xmlother
string		ls_rcvfromreqcont
long		ll_count
integer	li_rcvperiod, ll_find
dec{2}	ldc_payoutamt, ldc_payoutclr, ldc_payoutnet
boolean	lb_error
datetime	ldtm_rcvdate
n_ds		lds_payoutslip, lds_payinslipdet

// สร้าง DataStore สำหรับใช้งาน
lds_payoutslip	= create n_ds
lds_payoutslip.dataobject	= DWO_PAYOUTSLIP

lds_payinslipdet	= create n_ds
lds_payinslipdet.dataobject	= DWO_PAYINSLIPDET

ls_xmlhead		= astr_slip.xml_sliphead
ls_xmlloan		= astr_slip.xml_slipcutlon
ls_xmlother		= astr_slip.xml_slipcutetc

this.of_setsrvdwxmlie( true )

try
	// Import ข้อมูลจ่ายเงินกู้
	inv_dwxmliesrv.of_xmlimport( lds_payoutslip, ls_xmlhead )
	
	// Import ข้อมูลหักชำระ
	inv_dwxmliesrv.of_xmlimport( lds_payinslipdet, ls_xmlloan )
	inv_dwxmliesrv.of_xmlimport( lds_payinslipdet, ls_xmlother )
	
	li_rcvperiod		= lds_payoutslip.getitemnumber( 1, "rcv_period" )
	ldtm_rcvdate	= lds_payoutslip.getitemdatetime( 1, "slip_date" )
	
	// ถ้ารับเงินไม่ใช่งวดแรกต้องคิด ด/บ ค้าง
	if li_rcvperiod > 1 then
		this.of_initlnrcv_calint( lds_payoutslip )
	end if
	
	// ส่งรายการหักไปคำนวณดอกเบี้ยใหม่( ใน function filter รายการหนี้เอง)
	this.of_calintlnpayment( lds_payinslipdet, ldtm_rcvdate )
catch( Exception lthw_errimp )
	ithw_exception.text	= lthw_errimp.text
	lb_error					= true
end try

if lb_error then goto objdestroy

// คำนวณยอดหักชำระใหม่
ll_count	= lds_payinslipdet.rowcount()
if ll_count > 0 then
	ldc_payoutclr	= dec( lds_payinslipdet.describe( "evaluate( 'sum( if( operate_flag = 1, item_payamt, 0 ) for all )', "+string( ll_count )+" )" ) )
else
	ldc_payoutclr	= 0
end if

ldc_payoutamt		= lds_payoutslip.getitemdecimal( 1, "payout_amt" )
ldc_payoutnet		= ldc_payoutamt - ldc_payoutclr

lds_payoutslip.setitem( 1, "payoutclr_amt", ldc_payoutclr )
lds_payoutslip.setitem( 1, "payoutnet_amt", ldc_payoutnet )

// Export ข้อมูลอีกครั้งเพื่อให้ได้ค่าสุดท้าย
lds_payinslipdet.setfilter( "slipitemtype_code = 'LON'" )
lds_payinslipdet.filter()
ls_xmlloan		= inv_dwxmliesrv.of_xmlexport( lds_payinslipdet )

lds_payinslipdet.setfilter( "slipitemtype_code <> 'LON'" )
lds_payinslipdet.filter()
ls_xmlother		= inv_dwxmliesrv.of_xmlexport( lds_payinslipdet )

astr_slip.xml_sliphead	= inv_dwxmliesrv.of_xmlexport( lds_payoutslip )
astr_slip.xml_slipcutlon	= ls_xmlloan
astr_slip.xml_slipcutetc	= ls_xmlother

objdestroy:
destroy lds_payoutslip
destroy lds_payinslipdet

return 1

end function

public function integer of_initintfixchg (string as_xmloption, ref string as_xmldata) throws Exception;string		ls_ccoopid, ls_contno, ls_memname
string		ls_lntype, ls_lntypegrp
long		ll_index, ll_count, ll_row
integer	li_accyear, li_accmonth
dec{2}	ldc_prnbal, ldc_bfintarr, ldc_intamt
dec{4}	ldc_newintrate, ldc_intrate
n_ds		lds_option, lds_contdata, lds_intfixchgdata
datetime	ldtm_bflastcalint, ldtm_intchgdate, ldtm_contbefore

lds_option		= create n_ds
lds_option.dataobject		= "d_lcsrv_intfixchg_option"

try
	this.of_setsrvdwxmlie( true )
	inv_dwxmliesrv.of_xmlimport( lds_option, as_xmloption )
catch( Exception lthw_errimp )
	destroy lds_option
end try

ls_lntype				= lds_option.getitemstring( 1, "loantype_code" )
ldtm_contbefore	= lds_option.getitemdatetime( 1, "contbefore_date" )
ldtm_intchgdate	= lds_option.getitemdatetime( 1, "intfixchg_date" )
ldc_newintrate		= lds_option.getitemnumber( 1, "new_intrate" )

lds_contdata		= create n_ds
lds_contdata.dataobject	= "d_lcsrv_intfixchg_contdata"
lds_contdata.settransobject( itr_sqlca )

lds_intfixchgdata	= create n_ds
lds_intfixchgdata.dataobject	= "d_lcsrv_intfixchg_detail"

ll_count 			= lds_contdata.retrieve( is_coopcontrol, ls_lntype, ldtm_contbefore )

this.of_setsrvlcinterest( true )

for ll_index = 1 to ll_count
	ls_ccoopid			= lds_contdata.getitemstring( ll_index, "coop_id" )
	ls_contno			= lds_contdata.getitemstring( ll_index, "loancontract_no" )
	ls_memname		= lds_contdata.getitemstring( ll_index, "memb_name" )
	ls_lntypegrp			= lds_contdata.getitemstring( ll_index, "loangroup_code" )
	ldc_prnbal			= lds_contdata.getitemdecimal( ll_index, "principal_balance" )
	ldc_bfintarr			= lds_contdata.getitemdecimal( ll_index, "interest_arrear" )
	ldc_intrate			= lds_contdata.getitemdecimal( ll_index, "int_contintrate" )
	ldtm_bflastcalint	= lds_contdata.getitemdatetime( ll_index, "lastcalint_date" )
	
	if ldc_newintrate = ldc_intrate then
		ldc_intamt		= 0
	else
		// ตรวจว่าถ้าวันที่คิด ด/บ ถึงน้อยกว่าวันที่เปลี่ยนต้องตั้ง ด/บ ค้างเอาไว้
		if ldtm_bflastcalint < ldtm_intchgdate then
			ldc_intamt	= inv_intsrv.of_computeinterest( ls_ccoopid, ls_contno, ldc_prnbal, ldtm_bflastcalint, ldtm_intchgdate )
		else
			ldc_intamt	= 0
		end if
	end if
	
	choose case ls_lntypegrp
		case "01"
			ls_lntypegrp		= "สั้น"
		case "02"
			ls_lntypegrp		= "กลาง"
		case "03"
			ls_lntypegrp		= "ยาว"
	end choose
	
	ll_row				= lds_intfixchgdata.insertrow( 0 )
	
	lds_intfixchgdata.setitem( ll_row, "coop_id", ls_ccoopid )
	lds_intfixchgdata.setitem( ll_row, "loancontract_no", ls_contno )
	lds_intfixchgdata.setitem( ll_row, "memb_name", ls_memname )
	lds_intfixchgdata.setitem( ll_row, "loantype_desc", ls_lntypegrp )
	lds_intfixchgdata.setitem( ll_row, "intfixchgrate_date", ldtm_intchgdate )
	lds_intfixchgdata.setitem( ll_row, "old_intrate", ldc_intrate )
	lds_intfixchgdata.setitem( ll_row, "new_intrate", ldc_newintrate )
	lds_intfixchgdata.setitem( ll_row, "bfprnbal_amt", ldc_prnbal )
	lds_intfixchgdata.setitem( ll_row, "bflastcalint_date", ldtm_bflastcalint )
	lds_intfixchgdata.setitem( ll_row, "bfinterest_arrear", ldc_bfintarr )
	lds_intfixchgdata.setitem( ll_row, "interest_period", ldc_intamt )
	
next

as_xmldata		= inv_dwxmliesrv.of_xmlexport( lds_intfixchgdata )

if isvalid( lds_option ) then destroy lds_option
if isvalid( lds_contdata ) then destroy lds_contdata
if isvalid( lds_intfixchgdata ) then destroy lds_intfixchgdata

return 1
end function

public function integer of_saveintfixchg (ref string as_xmlintfixchg) throws Exception;string		ls_ccoopid, ls_contno
long		ll_count, ll_index
dec		ldc_newintrate, ldc_oldintrate, ldc_bfintarr, ldc_intamt, ldc_intarrbal
datetime	ldtm_intchgdate, ldtm_bflastcalint, ldtm_lastcalint
n_ds		lds_intfixchgdata

lds_intfixchgdata	= create n_ds
lds_intfixchgdata.dataobject	= "d_lcsrv_intfixchg_detail"
lds_intfixchgdata.settransobject( itr_sqlca )

try
	this.of_setsrvdwxmlie( true )
	inv_dwxmliesrv.of_xmlimport( lds_intfixchgdata, as_xmlintfixchg )
catch( Exception lthw_errimp )
	destroy lds_intfixchgdata
end try

ll_count		= lds_intfixchgdata.rowcount()

for ll_index = 1 to ll_count
	ls_ccoopid			= lds_intfixchgdata.getitemstring( ll_index, "coop_id" )
	ls_contno			= lds_intfixchgdata.getitemstring( ll_index, "loancontract_no" )
	ldtm_intchgdate	= lds_intfixchgdata.getitemdatetime( ll_index, "intfixchgrate_date" )
	ldc_oldintrate		= lds_intfixchgdata.getitemdecimal( ll_index, "old_intrate" )
	ldc_newintrate		= lds_intfixchgdata.getitemdecimal( ll_index, "new_intrate" )
	ldtm_bflastcalint	= lds_intfixchgdata.getitemdatetime( ll_index, "bflastcalint_date" )
	ldc_bfintarr			= lds_intfixchgdata.getitemdecimal( ll_index, "bfinterest_arrear" )
	ldc_intamt			= lds_intfixchgdata.getitemdecimal( ll_index, "interest_period" )
	
	if ldc_oldintrate = ldc_newintrate then
		continue
	end if
	
	if ldtm_bflastcalint < ldtm_intchgdate then
		ldtm_lastcalint	= ldtm_intchgdate
	else
		ldtm_lastcalint	= ldtm_bflastcalint
	end if
	
	ldc_intarrbal			= ldc_bfintarr + ldc_intamt
	
	update	lccontmaster
	set			lastcalint_date	= :ldtm_lastcalint,
				interest_arrear	= :ldc_intarrbal,
				int_contintrate	= :ldc_newintrate
	where	( coop_id				= :ls_ccoopid )
	and		( loancontract_no	= :ls_contno )
	using		itr_sqlca ;

	if itr_sqlca.sqlcode <> 0 then
		rollback using itr_sqlca ;
		destroy lds_intfixchgdata
		ithw_exception.text	= "ไม่สามารถเปลี่ยนแปลงอัตราดอกเบี้ยใหม่ของสัญญาได้ สัญญาเลขที่ "+ls_contno+" ~n"+itr_sqlca.sqlerrtext
		throw ithw_exception
	end if
next

// ลบพวกที่ไม่เปลี่ยนแปลงออก
lds_intfixchgdata.setfilter( "old_intrate <> new_intrate" )
lds_intfixchgdata.filter()

lds_intfixchgdata.rowsdiscard( 1, lds_intfixchgdata.filteredcount(), filter! )

if lds_intfixchgdata.update() <> 1 then
	rollback using itr_sqlca ;
	destroy lds_intfixchgdata
	ithw_exception.text	= "บันทึกข้อมูลประวัติการเปลี่ยนแปลงอัตราดอกเบี้ย ไม่ได้"
	ithw_exception.text	+= lds_intfixchgdata.of_geterrormessage()
end if

commit using itr_sqlca ;

return 1
end function

on n_cst_lncoopsrv_lnoperate.create
call super::create
TriggerEvent( this, "constructor" )
end on

on n_cst_lncoopsrv_lnoperate.destroy
TriggerEvent( this, "destructor" )
call super::destroy
end on

event constructor;ithw_exception = create Exception
end event

event destructor;if isvalid( ithw_exception ) then destroy ithw_exception
end event

