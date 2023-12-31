$PBExportHeader$n_cst_lnsrv_slslipadj.sru
forward
global type n_cst_lnsrv_slslipadj from NonVisualObject
end type
end forward

global type n_cst_lnsrv_slslipadj from NonVisualObject
end type
global n_cst_lnsrv_slslipadj n_cst_lnsrv_slslipadj

type variables
transaction		itr_sqlca
exception		iex_exception

n_cst_dbconnectservice		inv_transection
n_cst_doccontrolservice		inv_docsrv
n_cst_datetimeservice		inv_datetimesrv
n_cst_dwxmlieservice			inv_dwxmliesrv
 
string		is_coopcontrol, is_coopid, is_userid
integer	ii_adjmthlonflag
datetime	idtm_opdate

constant string	DWO_ADJSLIP		= "d_kpsrv_slipadj"
constant string	DWO_ADJSLIPDET	= "d_kpsrv_slipadjdet"
end variables

forward prototypes
private function integer of_poststm_contract (str_poststmloan astr_lnstatement) throws Exception
public function integer of_initservice (n_cst_dbconnectservice anv_dbtrans) throws Exception
private function integer of_setsrvdoccontrol (boolean ab_switch)
private function integer of_setsrvdatetime (boolean ab_switch)
private function integer of_setsrvdwxmlie (boolean ab_switch)
private function integer of_poststm_share (str_poststmshare astr_infoshrstm) throws Exception
private function integer of_init_adjmth_lon (ref n_ds ads_adjslipdet, n_ds ads_kpmastdet) throws Exception
private function integer of_init_adjmth_shr (ref n_ds ads_adjslipdet, n_ds ads_kpmastdet) throws Exception
private function integer of_init_adjmth_etc (ref n_ds ads_adjslipdet, n_ds ads_kpmastdet) throws Exception
public function integer of_setabsadjust (n_ds ads_slipadjdet)
public function integer of_check_overadj (n_ds ads_slipadjdet) throws Exception
private function integer of_post_adjmth_lon (n_ds ads_slipadj, n_ds ads_slipadjdet) throws Exception
private function integer of_post_adjmth_shr (n_ds ads_slipadj, n_ds ads_slipadjdet) throws Exception
private function integer of_post_adjmth_etc (n_ds ads_slipadj, n_ds ads_slipadjdet) throws Exception
private function integer of_post_ccladj_shr (n_ds ads_slipadj, n_ds ads_slipadjdet) throws Exception
private function integer of_post_ccladj_lon (n_ds ads_slipadj, n_ds ads_slipadjdet) throws Exception
public function integer of_save_ccladj (ref str_slipadjust astr_slipadjust) throws Exception
public function integer of_save_adjmth (ref str_slipadjust astr_kpadj) throws Exception
public function integer of_init_adjmth (ref str_slipadjust astr_kpadj) throws Exception
private function integer of_post_ccladj_etc (n_ds ads_slipadj, n_ds ads_slipadjdet) throws Exception
end prototypes

private function integer of_poststm_contract (str_poststmloan astr_lnstatement) throws Exception;string		ls_contno, ls_ccoopid, ls_itemcode, ls_refdocno, ls_refslipno
string		ls_moneytype, ls_entryid, ls_entrycoopid, ls_remark
integer	li_period, li_itemstatus, li_lastseqno
dec{2}	ldc_prnpay, ldc_intpay, ldc_prnbal, ldc_prncalint, ldc_intperiod
dec{2}	ldc_bfintarr, ldc_bfintret, ldc_intarrbal, ldc_intretbal
datetime	ldtm_slipdate, ldtm_opedate, ldtm_accdate, ldtm_intaccdate, ldtm_entrydate
datetime	ldtm_calintfrom, ldtm_calintto

ls_ccoopid		= astr_lnstatement.contcoop_id
ls_contno		= astr_lnstatement.loancontract_no
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

select		last_stm_no
into		:li_lastseqno
from		lncontmaster
where	( coop_id				= :ls_ccoopid )
and		( loancontract_no	= :ls_contno )
using		itr_sqlca ;

if itr_sqlca.sqlcode <> 0 then
	iex_exception.text	+= "of_poststm_contract ไม่พบเลขที่สัญญาที่ระบุ "+ls_contno+" กรุณาตรวจสอบ ~r~n"+itr_sqlca.sqlerrtext
	throw iex_exception
end if

if isnull( li_lastseqno ) then li_lastseqno = 0

li_lastseqno ++

// เพิ่มรายการเคลื่อนไหวการชำระหนี้
insert into lncontstatement
			( coop_id,				loancontract_no,		seq_no,					slip_date,				operate_date,			account_date,			intaccum_date,
			  ref_slipno,				ref_docno,				loanitemtype_code,	
			  period,		 			principal_payment,	interest_payment,		principal_balance,		prncalint_amt,			calint_from,				calint_to,
			  bfintarrear_amt,		bfintreturn_amt,		interest_period,		interest_arrear,		interest_return,			moneytype_code,		item_status,
			  entry_id,				entry_date,				entry_bycoopid,		remark )
values	( :ls_ccoopid,			:ls_contno,				:li_lastseqno,			:ldtm_slipdate,			:ldtm_opedate,			:ldtm_accdate,			:ldtm_intaccdate,
			  :ls_refslipno,			:ls_refdocno,			:ls_itemcode,			
			  :li_period,				:ldc_prnpay,				:ldc_intpay,				:ldc_prnbal,				:ldc_prncalint,			:ldtm_calintfrom,		:ldtm_calintto,
			  :ldc_bfintarr,			:ldc_bfintret,			:ldc_intperiod,			:ldc_intarrbal,			:ldc_intretbal,			:ls_moneytype,			:li_itemstatus,
			  :ls_entryid,			:ldtm_entrydate,		:ls_entrycoopid,		:ls_remark )
using		itr_sqlca ;

if itr_sqlca.sqlcode <> 0 then
	iex_exception.text	+= "of_poststm_contract  ไม่สามารถเพิ่มรายการ Statement หนี้ "+ls_contno+" ได้ กรุณาตรวจสอบ "+itr_sqlca.sqlerrtext
	throw iex_exception
end if

update	lncontmaster
set			last_stm_no		= :li_lastseqno
where	( coop_id 			= :ls_ccoopid )
and		( loancontract_no	= :ls_contno )
using		itr_sqlca ;

if itr_sqlca.sqlcode <> 0 then
	iex_exception.text	+= "of_poststm_contract  ไม่สามารถปรับปรุงลำดับที่ล่าสุดได้ เลขที่สัญญา "+ls_contno+" กรุณาตรวจสอบ ~r~n"+itr_sqlca.sqlerrtext
	throw iex_exception
end if

return 1
end function

public function integer of_initservice (n_cst_dbconnectservice anv_dbtrans) throws Exception;// Service Transection
if isnull( inv_transection ) or not isvalid( inv_transection ) then
	inv_transection	= create n_cst_dbconnectservice
end if

inv_transection	= anv_dbtrans
itr_sqlca 			= inv_transection.itr_dbconnection
is_coopcontrol	= anv_dbtrans.is_coopcontrol
is_coopid			= anv_dbtrans.is_coopid

return 1
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

private function integer of_poststm_share (str_poststmshare astr_infoshrstm) throws Exception;string		ls_memno, ls_mcoopid, ls_entryid, ls_entrycoopid
string		ls_docno, ls_sharetype, ls_itemtype, ls_moneytype, ls_refslipno
integer	li_stmno, li_period, li_itemstatus
decimal	ldc_shrpayamt, ldc_shrstkamt, ldc_bfshrarramt, ldc_shrarramt
datetime	ldtm_opedate, ldtm_slipdate, ldtm_entrydate, ldtm_accdate, ldtm_shrtime

ls_mcoopid			= astr_infoshrstm.memcoop_id
ls_memno 			= astr_infoshrstm.member_no
ls_sharetype		= astr_infoshrstm.sharetype_code
ldtm_slipdate		= astr_infoshrstm.slip_date
ldtm_opedate		= astr_infoshrstm.operate_date
ldtm_accdate		= astr_infoshrstm.account_date
ldtm_shrtime		= astr_infoshrstm.sharetime_date
ls_docno				= astr_infoshrstm.document_no
ls_itemtype			= astr_infoshrstm.stmitem_code
li_period				= astr_infoshrstm.period
ldc_shrpayamt		= astr_infoshrstm.sharepay_amt
ldc_shrstkamt		= astr_infoshrstm.sharebal_amt
ls_entryid			= astr_infoshrstm.entry_id
ls_entrycoopid		= astr_infoshrstm.entry_bycoopid
ls_moneytype		= astr_infoshrstm.moneytype
ldc_bfshrarramt	= astr_infoshrstm.bfsharearr_amt
ldc_shrarramt		= astr_infoshrstm.sharearr_amt
li_itemstatus		= astr_infoshrstm.item_status
ls_refslipno			= astr_infoshrstm.ref_slipno

ldtm_entrydate		= datetime( today(), now() )

select		last_stm_no
into		:li_stmno
from		shsharemaster
where	coop_id = :ls_mcoopid
and		member_no = :ls_memno
using	itr_sqlca ;

if isnull( li_stmno ) then li_stmno = 0

li_stmno	++
	
// บันทึกรายการเคลื่อนไหว
insert into shsharestatement
		( coop_id,				member_no,			sharetype_code,		seq_no,					ref_slipno,			ref_docno,
		  slip_date,				operate_date,			account_date,			share_date,
		  shritemtype_code,	period,					share_amount,			sharestk_amt,
		  moneytype_code,	shrarrearbf_amt,		shrarrear_amt,			item_status,
		  entry_id,				entry_date,				entry_bycoopid )
		  
values( :ls_mcoopid,			:ls_memno,				:ls_sharetype,			:li_stmno,				:ls_refslipno,		:ls_docno,
		  :ldtm_slipdate,		:ldtm_opedate,			:ldtm_accdate,			:ldtm_shrtime,
		  :ls_itemtype,			:li_period,				:ldc_shrpayamt,		:ldc_shrstkamt,
		  :ls_moneytype,		:ldc_bfshrarramt,		:ldc_shrarramt,		  	:li_itemstatus,
		  :ls_entryid,			:ldtm_entrydate,		:ls_entrycoopid )
using itr_sqlca;

if ( itr_sqlca.sqlcode <> 0 ) then
	iex_exception.text += "of_poststm_share ไม่สามารถเพิ่ม statement หุ้นได้ ทะเบียน "+ls_memno+" กรุณาตรวจสอบ " + itr_sqlca.sqlerrtext 
	throw iex_exception
end if

update	shsharemaster
set			last_stm_no = :li_stmno
where	( coop_id 			= :ls_mcoopid )
and		( member_no 		= :ls_memno )
and		( sharetype_code	= :ls_sharetype )
using	itr_sqlca ;

if ( itr_sqlca.sqlcode <> 0 ) then
	iex_exception.text += "of_poststm_share ไม่สามารถปรับปรุงลำดับที่ล่าสุดหุ้นได้ ทะเบียน "+ls_memno+" กรุณาตรวจสอบ " + itr_sqlca.sqlerrtext 
	throw iex_exception
end if

return 1
end function

private function integer of_init_adjmth_lon (ref n_ds ads_adjslipdet, n_ds ads_kpmastdet) throws Exception;string 	ls_contno, ls_keeptype, ls_slipitemtype
integer	li_period, li_bfccontsts, li_bfccontlaw
long		ll_index, ll_count, ll_insrow
dec		ldc_prnpay, ldc_intpay, ldc_itempay, ldc_bfprnbalkep, ldc_bfprnbalcont
dec		ldc_prnadj, ldc_intadj, ldc_itemadj
dec		ldc_bfcprnbal, ldc_bfcintarr, ldc_bfcintset, ldc_adjbalprn, ldc_adjbalint, ldc_adjbalitem
datetime ldtm_bfclastcalint, ldtm_bfclastproc, ldtm_bfclastpay

ads_kpmastdet.setfilter( "keepitemtype_code in ('L01','L02','L03','PAR','IAR')" )
ads_kpmastdet.filter()

ll_count	= ads_kpmastdet.rowcount()

for ll_index = 1 to ll_count
	ls_contno		= ads_kpmastdet.getitemstring( ll_index, "loancontract_no" )
	ls_keeptype		= ads_kpmastdet.getitemstring( ll_index, "keepitemtype_code" )
	
	li_period			= ads_kpmastdet.getitemnumber( ll_index, "period" )
	
	ldc_prnpay		= ads_kpmastdet.getitemdecimal( ll_index, "principal_payment" )
	ldc_intpay		= ads_kpmastdet.getitemdecimal( ll_index, "interest_payment" )
	ldc_itempay		= ads_kpmastdet.getitemdecimal( ll_index, "item_payment" )
	
	ldc_prnadj		= ads_kpmastdet.getitemdecimal( ll_index, "adjust_prnamt" )
	ldc_intadj		= ads_kpmastdet.getitemdecimal( ll_index, "adjust_intamt" )
	ldc_itemadj		= ads_kpmastdet.getitemdecimal( ll_index, "adjust_itemamt" )
	
	if isnull( ldc_prnpay ) then ldc_prnpay = 0
	if isnull( ldc_intpay ) then ldc_intpay = 0
	if isnull( ldc_itempay ) then ldc_itempay = 0
	
	if isnull( ldc_prnadj ) then ldc_prnadj = 0
	if isnull( ldc_intadj ) then ldc_intadj = 0
	if isnull( ldc_itemadj ) then ldc_itemadj = 0
	
	// ดึงยอดปัจจุบัน
	select	principal_balance, 	interest_arrear, intset_arrear, 
			lastcalint_date, lastprocess_date, lastpayment_date, contract_status, contlaw_status
	into	:ldc_bfcprnbal, :ldc_bfcintarr, :ldc_bfcintset, 
			:ldtm_bfclastcalint, :ldtm_bfclastproc, :ldtm_bfclastpay, :li_bfccontsts, :li_bfccontlaw
	from	lncontmaster
	where coop_id = :is_coopcontrol
	and loancontract_no = :ls_contno
	using itr_sqlca ;
	
	// ตรวจสอบยอดคงเหลือก่อนทำรายการ
	ldc_bfprnbalkep	= ads_kpmastdet.getitemdecimal( ll_index, "bfprinbalance_amt" )
	
	// ไม่ต้องตรวจสอบแล้วเพราะว่าใช้เป็นการตั้งค้างแทน ** oh 21/9/18
//	if ( ldc_bfprnbalkep - ldc_prnpay ) <> ldc_bfcprnbal then
//		iex_exception.text	= "สัญญาเลขที่ : "+ls_contno+" มีการเปลี่ยนแปลงข้อมูลไปแล้ว กรุณาตรวจสอบ"
//		throw iex_exception
//	end if
	
	if ls_keeptype = "L01" or ls_keeptype = "L02" or ls_keeptype = "L03" then
		ls_slipitemtype		= "LON"
	else
		ls_slipitemtype		= ls_keeptype
	end if
	
	ll_insrow		= ads_adjslipdet.insertrow( 0 )
	
	ads_adjslipdet.object.shrlontype_code[ ll_insrow ]			= ads_kpmastdet.object.shrlontype_code[ ll_index ]
	ads_adjslipdet.object.loancontract_no[ ll_insrow ]			= ads_kpmastdet.object.loancontract_no[ ll_index ]
	ads_adjslipdet.object.slipitem_desc[ ll_insrow ]			= ads_kpmastdet.object.description[ ll_index ]
	ads_adjslipdet.object.bfmthpay_refmembno[ ll_insrow ]	= ads_kpmastdet.object.ref_membno[ ll_index ]
	ads_adjslipdet.object.bfmthpay_seqno[ ll_insrow ]		= ads_kpmastdet.object.seq_no[ ll_index ]
	ads_adjslipdet.object.bfshrcont_status[ ll_insrow ]		= ads_kpmastdet.object.bfcontract_status[ ll_index ]
	
	ads_adjslipdet.object.itemsign_flag[ ll_insrow ]			= ads_kpmastdet.object.sign_flag[ ll_index ]
	
	ldc_adjbalprn	= ldc_prnpay - ldc_prnadj
	ldc_adjbalint	= ldc_intpay - ldc_intadj
	ldc_adjbalitem	= ldc_itempay - ldc_itemadj
	
	ads_adjslipdet.setitem( ll_insrow, "seq_no", ll_insrow )
	ads_adjslipdet.setitem( ll_insrow, "concoop_id", is_coopcontrol )
	ads_adjslipdet.setitem( ll_insrow, "principal_adjamt", ldc_adjbalprn )
	ads_adjslipdet.setitem( ll_insrow, "interest_adjamt", ldc_adjbalint )
	ads_adjslipdet.setitem( ll_insrow, "item_adjamt", ldc_adjbalitem )
	ads_adjslipdet.setitem( ll_insrow, "item_balance", ldc_bfcprnbal )
	ads_adjslipdet.setitem( ll_insrow, "bfintarr_amt", ldc_bfcintarr )
	ads_adjslipdet.setitem( ll_insrow, "bfintarrset_amt", ldc_bfcintset )
	ads_adjslipdet.setitem( ll_insrow, "bflastcalint_date", ldtm_bfclastcalint )
	ads_adjslipdet.setitem( ll_insrow, "bflastproc_date", ldtm_bfclastproc )
	ads_adjslipdet.setitem( ll_insrow, "bflastpay_date", ldtm_bfclastpay )
	ads_adjslipdet.setitem( ll_insrow, "bfshrcont_balamt", ldc_bfcprnbal )
	ads_adjslipdet.setitem( ll_insrow, "bfshrcont_status", li_bfccontsts )
	ads_adjslipdet.setitem( ll_insrow, "bfcontlaw_status", li_bfccontlaw )
	
	ads_adjslipdet.setitem( ll_insrow, "operate_flag", 0 )
	ads_adjslipdet.setitem( ll_insrow, "slipitemtype_code", ls_slipitemtype )
	ads_adjslipdet.setitem( ll_insrow, "periodadj_amt", li_period )
	ads_adjslipdet.setitem( ll_insrow, "bfmthpay_kpitemtyp", ls_keeptype )
	ads_adjslipdet.setitem( ll_insrow, "bfmthpay_prnamt", ldc_prnpay )
	ads_adjslipdet.setitem( ll_insrow, "bfmthpay_intamt", ldc_intpay )
	ads_adjslipdet.setitem( ll_insrow, "bfmthpay_intarramt", 0 )
	ads_adjslipdet.setitem( ll_insrow, "bfmthpay_itemamt", ldc_itempay )
	ads_adjslipdet.setitem( ll_insrow, "bfmthadj_prnamt", ldc_prnadj )
	ads_adjslipdet.setitem( ll_insrow, "bfmthadj_intamt", ldc_intadj )
	ads_adjslipdet.setitem( ll_insrow, "bfmthadj_intarramt", 0 )
	ads_adjslipdet.setitem( ll_insrow, "bfmthadj_itemamt", ldc_itemadj )
	
	ads_adjslipdet.setitem( ll_insrow, "sign_flag", 1 )
next

return 1
end function

private function integer of_init_adjmth_shr (ref n_ds ads_adjslipdet, n_ds ads_kpmastdet) throws Exception;string 	ls_memno, ls_keeptype
integer	li_period
long		ll_index, ll_count, ll_insrow
dec		ldc_itempay, ldc_itemadj

ads_kpmastdet.setfilter( "keepitemtype_code in ('S01','SAR')" )
ads_kpmastdet.filter()

ll_count	= ads_kpmastdet.rowcount()

for ll_index = 1 to ll_count
	string		ls_slipitem
	integer	li_shrstatus
	dec		ldc_shrstkbal
	
	ls_memno		= ads_kpmastdet.getitemstring( ll_index, "member_no" )
	ls_keeptype		= ads_kpmastdet.getitemstring( ll_index, "keepitemtype_code" )
	
	li_period			= ads_kpmastdet.getitemnumber( ll_index, "period" )
	
	ldc_itempay		= ads_kpmastdet.getitemdecimal( ll_index, "item_payment" )
	ldc_itemadj		= ads_kpmastdet.getitemdecimal( ll_index, "adjust_itemamt" )
	
	if isnull( ldc_itempay ) then ldc_itempay = 0
	if isnull( ldc_itemadj ) then ldc_itemadj = 0
	
	// ต้องดึงค่าใหม่เพราะอาจมีการเปลี่ยนแปลง
	select		sh.sharestk_amt * st.unitshare_value,
				sh.sharemaster_status
	into		:ldc_shrstkbal, :li_shrstatus
	from		shsharemaster sh join shsharetype st on sh.coop_id = st.coop_id and sh.sharetype_code = st.sharetype_code
	where	( sh.coop_id				= :is_coopcontrol )
	and		( sh.member_no		= :ls_memno )
	using		itr_sqlca ;
	
	if itr_sqlca.sqlcode <> 0 then
		iex_exception.text	+= "ไม่พบข้อมูลหุ้นทะเบียน : "+ ls_memno +"กรุณาตรวจสอบ"
		throw iex_exception
	end if
	
	if ls_keeptype = "S01" then
		ls_slipitem	= "SHR"
	else
		ls_slipitem	= ls_keeptype
	end if
	
	ll_insrow		= ads_adjslipdet.insertrow( 0 )
	
	ads_adjslipdet.object.shrlontype_code[ ll_insrow ]			= ads_kpmastdet.object.shrlontype_code[ ll_index ]
	ads_adjslipdet.object.slipitem_desc[ ll_insrow ]			= ads_kpmastdet.object.description[ ll_index ]
	ads_adjslipdet.object.bfmthpay_refmembno[ ll_insrow ]	= ads_kpmastdet.object.ref_membno[ ll_index ]
	ads_adjslipdet.object.bfmthpay_seqno[ ll_insrow ]		= ads_kpmastdet.object.seq_no[ ll_index ]
	ads_adjslipdet.object.bfshrcont_status[ ll_insrow ]		= ads_kpmastdet.object.bfcontract_status[ ll_index ]
	ads_adjslipdet.object.itemsign_flag[ ll_insrow ]			= ads_kpmastdet.object.sign_flag[ ll_index ]
	
	ads_adjslipdet.object.principal_adjamt[ ll_insrow ]		= ldc_itempay - ldc_itemadj
	ads_adjslipdet.object.item_adjamt[ ll_insrow ]				= ldc_itempay - ldc_itemadj
	
	ads_adjslipdet.setitem( ll_insrow, "operate_flag", 0 )
	ads_adjslipdet.setitem( ll_insrow, "slipitemtype_code", ls_slipitem )
	ads_adjslipdet.setitem( ll_insrow, "bfmthpay_kpitemtyp", ls_keeptype )
	ads_adjslipdet.setitem( ll_insrow, "periodadj_amt", li_period )
	ads_adjslipdet.setitem( ll_insrow, "item_balance", ldc_shrstkbal )
	ads_adjslipdet.setitem( ll_insrow, "bfshrcont_balamt", ldc_shrstkbal )
	ads_adjslipdet.setitem( ll_insrow, "bfshrcont_status", li_shrstatus )
	ads_adjslipdet.setitem( ll_insrow, "bfmthpay_itemamt", ldc_itempay )
	ads_adjslipdet.setitem( ll_insrow, "bfmthadj_itemamt", ldc_itemadj )
	ads_adjslipdet.setitem( ll_insrow, "sign_flag", -1 )
next

return 1
end function

private function integer of_init_adjmth_etc (ref n_ds ads_adjslipdet, n_ds ads_kpmastdet) throws Exception;string 	ls_memno, ls_keeptype
integer	li_period
long		ll_index, ll_count, ll_insrow
dec		ldc_itempay, ldc_itemadj

ads_kpmastdet.setfilter( "keepitemtype_code not in ('S01', 'L01', 'L02', 'L03', 'PAR', 'IAR', 'SAR')" )
ads_kpmastdet.filter()

ll_count	= ads_kpmastdet.rowcount()

for ll_index = 1 to ll_count
	
	ls_memno		= ads_kpmastdet.getitemstring( ll_index, "member_no" )
	ls_keeptype		= ads_kpmastdet.getitemstring( ll_index, "keepitemtype_code" )
	
	li_period			= ads_kpmastdet.getitemnumber( ll_index, "period" )
	
	ldc_itempay		= ads_kpmastdet.getitemdecimal( ll_index, "item_payment" )
	ldc_itemadj		= ads_kpmastdet.getitemdecimal( ll_index, "adjust_itemamt" )
	
	if isnull( ldc_itempay ) then ldc_itempay = 0
	if isnull( ldc_itemadj ) then ldc_itemadj = 0
	
	ll_insrow		= ads_adjslipdet.insertrow( 0 )
	
	ads_adjslipdet.object.shrlontype_code[ ll_insrow ]			= ads_kpmastdet.object.shrlontype_code[ ll_index ]
	ads_adjslipdet.object.slipitem_desc[ ll_insrow ]			= ads_kpmastdet.object.description[ ll_index ]
	ads_adjslipdet.object.bfmthpay_refmembno[ ll_insrow ]	= ads_kpmastdet.object.ref_membno[ ll_index ]
	ads_adjslipdet.object.bfmthpay_seqno[ ll_insrow ]		= ads_kpmastdet.object.seq_no[ ll_index ]
	ads_adjslipdet.object.bfshrcont_status[ ll_insrow ]		= ads_kpmastdet.object.bfcontract_status[ ll_index ]
	
	ads_adjslipdet.object.itemsign_flag[ ll_insrow ]			= ads_kpmastdet.object.sign_flag[ ll_index ]	
	
	ads_adjslipdet.object.item_adjamt[ ll_insrow ]				= ldc_itempay - ldc_itemadj
	
	ads_adjslipdet.setitem( ll_insrow, "operate_flag", 0 )
	ads_adjslipdet.setitem( ll_insrow, "slipitemtype_code", ls_keeptype )
	ads_adjslipdet.setitem( ll_insrow, "bfmthpay_kpitemtyp", ls_keeptype )
	ads_adjslipdet.setitem( ll_insrow, "bfmthpay_itemamt", ldc_itempay )
	ads_adjslipdet.setitem( ll_insrow, "bfmthadj_itemamt", ldc_itemadj )
	ads_adjslipdet.setitem( ll_insrow, "sign_flag", 1 )
	
	
next

return 1
end function

public function integer of_setabsadjust (n_ds ads_slipadjdet);string		ls_slipitemcode
integer	li_signflag
long		ll_index, ll_count
dec		ldc_prnpay, ldc_intpay, ldc_bfprnbal

ll_count	= ads_slipadjdet.rowcount()

for ll_index = 1 to ll_count
	ls_slipitemcode		= ads_slipadjdet.getitemstring( ll_index, "slipitemtype_code" )
	li_signflag			= ads_slipadjdet.getitemnumber( ll_index, "sign_flag" )
	
	choose case ls_slipitemcode
		case "LON"
			ldc_prnpay	= ads_slipadjdet.getitemdecimal( ll_index, "principal_adjamt" )
			ldc_intpay	= ads_slipadjdet.getitemdecimal( ll_index, "interest_adjamt" )
			ldc_bfprnbal	= ads_slipadjdet.getitemdecimal( ll_index, "bfshrcont_balamt" )
			
			if isnull( ldc_prnpay ) then ldc_prnpay	 = 0
			if isnull( ldc_intpay ) then ldc_intpay = 0
			
			ads_slipadjdet.setitem( ll_index, "item_adjamt", ldc_prnpay + ldc_intpay )
			ads_slipadjdet.setitem( ll_index, "item_balance", ldc_bfprnbal + ( ldc_prnpay * li_signflag ) )
			
		case "SHR"
			ldc_prnpay	= ads_slipadjdet.getitemdecimal( ll_index, "principal_adjamt" )
			ldc_bfprnbal	= ads_slipadjdet.getitemdecimal( ll_index, "bfshrcont_balamt" )

			ads_slipadjdet.setitem( ll_index, "item_adjamt", ldc_prnpay )
			ads_slipadjdet.setitem( ll_index, "item_balance", ldc_bfprnbal + ( ldc_prnpay * li_signflag ) )
	end choose
next

return 1
end function

public function integer of_check_overadj (n_ds ads_slipadjdet) throws Exception;string		ls_adjtype, ls_validtext
string		ls_contno, ls_slipitemtype, ls_slipitemdesc
long		ll_find, ll_count
dec		ldc_maxprn, ldc_maxint, ldc_adjprn, ldc_adjint
dec		ldc_refslipamt, ldc_sumpayamt

// ค้นหาการปรับยอดเกินยอดของหนี้
ls_validtext	= ""
ls_validtext	+= " ( ( principal_adjamt > ( bfmthpay_prnamt - bfmthadj_prnamt ) ) or ( interest_adjamt > ( bfmthpay_intamt - bfmthadj_intamt ) ) ) "
ls_validtext	+= " and ( slipitemtype_code = 'LON' ) "

ll_count	= ads_slipadjdet.rowcount()
ll_find		= ads_slipadjdet.find( ls_validtext, 1, ll_count )

if ll_find > 0 then
	ls_contno	= ads_slipadjdet.getitemstring( ll_find, "loancontract_no" )
	ldc_maxprn	= ads_slipadjdet.getitemdecimal( ll_find, "bfmthpay_prnamt" )
	ldc_maxint	= ads_slipadjdet.getitemdecimal( ll_find, "bfmthpay_intamt" )
	ldc_adjprn	= ads_slipadjdet.getitemdecimal( ll_find, "bfmthadj_prnamt" )
	ldc_adjint	= ads_slipadjdet.getitemdecimal( ll_find, "bfmthadj_intamt" )
	
	ldc_maxprn	= ldc_maxprn - ldc_adjprn
	ldc_maxint	= ldc_maxint - ldc_adjint
	
	if ldc_maxprn < 0 then ldc_maxprn = 0
	if ldc_maxint < 0 then ldc_maxint = 0
	
	iex_exception.text	= "มีการปรับปรุงเกินยอดที่จะปรับปรุงได้ สัญญา "+ls_contno
	iex_exception.text	+= " ต้นเงินที่ปรับปรุงได้ = "+string( ldc_maxprn, "#, ##0.00" )
	iex_exception.text	+= " ดอกเบี้ยที่ปรับปรุงได้ = "+string( ldc_maxint, "#, ##0.00" )
	iex_exception.text	+= " กรุณาตรวจสอบ"
	
	throw iex_exception
end if

// ค้นหาการปรับยอดเกินยอดของรายการทั่วๆไป
ls_validtext	= "( item_adjamt > ( bfmthpay_itemamt - bfmthadj_itemamt ) ) and ( slipitemtype_code <> 'LON' )"

ll_find		= ads_slipadjdet.find( ls_validtext, 1, ll_count )
if ll_find > 0 then
	ls_slipitemtype	= ads_slipadjdet.getitemstring( ll_find, "slipitemtype_code" )
	ls_slipitemdesc	= ads_slipadjdet.getitemstring( ll_find, "slipitem_desc" )
	ldc_maxprn		= ads_slipadjdet.getitemdecimal( ll_find, "bfmthpay_itemamt" )
	ldc_adjprn		= ads_slipadjdet.getitemdecimal( ll_find, "bfmthadj_itemamt" )
	
	ldc_maxprn	= ldc_maxprn - ldc_adjprn
	if ldc_maxprn < 0 then ldc_maxprn = 0
	
	iex_exception.text	= "มีการปรับปรุงเกินยอดที่จะปรับปรุงได้ รหัสรายการ "+ls_slipitemtype
	iex_exception.text	+= " ยอดเงินที่ปรับปรุงได้ = "+string( ldc_maxprn, "#, ##0.00" )
	iex_exception.text	+= " กรุณาตรวจสอบ"
	throw iex_exception
end if

return 1
end function

private function integer of_post_adjmth_lon (n_ds ads_slipadj, n_ds ads_slipadjdet) throws Exception;string		ls_contno, ls_memno, ls_adjslipno, ls_refdocno, ls_rcvperiod, ls_adjcause, ls_slitmtyp, ls_entryid
integer	li_contstatus, li_odflag, li_periodadj, li_kpseq, li_bflastperiod
long		ll_index, ll_count
dec		ldc_prnbal, ldc_prnpay, ldc_intpay, ldc_itempay, ldc_intarrbal, ldc_intperiod
dec		ldc_bfprnbal, ldc_wtdbal, ldc_bfintreturn, ldc_bfmthprn, ldc_bfmthint
dec		ldc_bfintarr, ldc_bfintmth, ldc_bfintyr, ldc_intarrmthbal, ldc_intarryrbal, ldc_bfprnarr
dec		ldc_kpprnpay, ldc_kpprnbal
datetime ldtm_bflastcalint, ldtm_lastcalint, ldtm_refsldate, ldtm_adjdate, ldtm_lastpay
datetime ldtm_kpcalto
str_poststmloan	lstr_lnstm

ls_memno		= trim( ads_slipadj.getitemstring( 1, "member_no" ) )
ls_adjslipno		= trim( ads_slipadj.getitemstring( 1, "adjslip_no" ) )
ls_refdocno		= trim( ads_slipadj.getitemstring( 1, "ref_slipno" ) )
ls_rcvperiod		= trim( ads_slipadj.getitemstring( 1, "ref_recvperiod" ) )
ls_adjcause		= trim( ads_slipadj.getitemstring( 1, "adjust_cause" ) )
ls_entryid		= trim( ads_slipadj.getitemstring( 1, "entry_id" ) )
ldtm_refsldate	= ads_slipadj.getitemdatetime( 1, "ref_slipdate" )
ldtm_adjdate	= ads_slipadj.getitemdatetime( 1, "adjslip_date" )

ads_slipadjdet.setfilter( "slipitemtype_code in ( 'LON', 'PAR', 'IAR' )" )
ads_slipadjdet.filter()

ll_count		= ads_slipadjdet.rowcount()

for ll_index = 1 to ll_count
	ls_contno		= trim( ads_slipadjdet.getitemstring( ll_index, "loancontract_no" ) )
	ls_slitmtyp		= trim( ads_slipadjdet.getitemstring( ll_index, "slipitemtype_code" ) )
	li_kpseq			= ads_slipadjdet.getitemnumber( ll_index, "bfmthpay_seqno" )
	li_periodadj		= ads_slipadjdet.getitemnumber( ll_index,"periodadj_amt" )
	ldc_prnpay		= ads_slipadjdet.getitemdecimal( ll_index, "principal_adjamt" )
	ldc_intpay		= ads_slipadjdet.getitemdecimal( ll_index, "interest_adjamt" )
	ldc_bfmthprn	= ads_slipadjdet.getitemdecimal( ll_index, "bfmthpay_prnamt" )
	ldc_bfmthint		= ads_slipadjdet.getitemdecimal( ll_index, "bfmthpay_intamt" )
	
	li_contstatus			= 1
	
	// ดึงค่าใบเสร็จตอนยกเลิก
	select		principal_payment, bfprinbalance_amt, calintto_date,
				bflastcalint_date, bfinterest_arrear, bfintmonth_arrear, bfintyear_arrear, bflastpay_date
	into		:ldc_kpprnpay, :ldc_kpprnbal, :ldtm_kpcalto,
				:ldtm_bflastcalint, :ldc_bfintarr, :ldc_bfintmth, :ldc_bfintyr, :ldtm_lastpay
	from		kpmastreceivedet
	where	coop_id = :is_coopcontrol
	and		kpslip_no = :ls_refdocno
	and		seq_no = :li_kpseq
	using		itr_sqlca ;
	
	// ดึงค่าสัญญาล่าสุด
	select		principal_balance, interest_arrear, intmonth_arrear, intset_arrear, 
				withdrawable_amt, interest_return, od_flag, lastcalint_date,
				principal_arrear
	into		:ldc_bfprnbal, :ldc_bfintarr, :ldc_bfintmth, :ldc_bfintyr, 
				:ldc_wtdbal, :ldc_bfintreturn, :li_odflag, :ldtm_lastcalint,
				:ldc_bfprnarr
	from		lncontmaster
	where	loancontract_no	= :ls_contno
	and		coop_id				= :is_coopcontrol
	using itr_sqlca;
	
	if itr_sqlca.sqlcode <> 0 then
		iex_exception.text	= "ไม่พบข้อมูลสัญญาเงินกู้เลขที่ : " + ls_contno
		throw iex_exception
	end if
	
	// ตรวจสอบว่ายอดมีการขยับไปหรือยัง *** ไม่ต้องตรวจแล้วเพราะใช้การตั้งค้างแทน โอ้ 21/9/61
//	if ldc_bfprnbal <> (ldc_kpprnbal - ldc_kpprnpay) or ldtm_lastcalint <> ldtm_kpcalto then
//		iex_exception.text		= "สัญญา "+ls_contno+" มีการเปลี่ยนแปลงข้อมูลไปแล้ว ไม่สามารถทำรายการได้ จะยกเลิกได้ต้องมีคงเหลือเป็น "+string(ldc_kpprnbal - ldc_kpprnpay) +" วันที่คิด ด/บ "+string(ldtm_kpcalto,"dd/mm/yyyy")
//		throw iex_exception
//	end if
	
	ldc_prnbal			= ldc_bfprnbal + ldc_prnpay

	ldc_intarrbal			= ldc_bfintarr + ldc_intpay
	ldc_intarrmthbal	= ldc_bfintmth
	ldc_intarryrbal		= ldc_bfintyr
	
	ldc_intperiod		= 0
	
	ldc_itempay			= ldc_prnpay + ldc_intpay
	
	if li_odflag = 1 then
		ldc_wtdbal	= ldc_wtdbal - ldc_prnpay
	else
		// ถ้าหนี้คงเหลือเป็น 0 
		if ldc_prnbal = 0 then li_contstatus = -1
	end if
	
	// ใช้ตั้งค้างแทน
//	choose case ii_adjmthlonflag
//		case 2 // แบบถอยวันที่คิดดอกเบี้ยไม่ตั้งค้าง จาก kpmastreceivedet
//			
//			ldc_intarrbal			= ldc_bfintarr
//			ldc_intarrmthbal	= ldc_bfintmth
//			ldc_intarryrbal		= ldc_bfintyr
//			
//			ldtm_lastcalint		= ldtm_bflastcalint
//			
//		case else
//			// ให้คิดแบบตั้งค้างดอกเบี้ยเรียกเก็บ
//	end choose
	
	// บันทึกลง Statement
	lstr_lnstm.contcoop_id				= is_coopcontrol
	lstr_lnstm.loancontract_no			= ls_contno
	lstr_lnstm.slip_date					= ldtm_refsldate
	lstr_lnstm.operate_date				= ldtm_adjdate
	lstr_lnstm.account_date				= ldtm_adjdate
	lstr_lnstm.intaccum_date				= ldtm_refsldate
	lstr_lnstm.ref_slipno					= ls_adjslipno
	lstr_lnstm.ref_docno					= ls_refdocno
	lstr_lnstm.loanitemtype_code		= "RPM"
	lstr_lnstm.period						= li_periodadj
	lstr_lnstm.principal_payment		= ldc_prnpay
	lstr_lnstm.interest_payment			= ldc_intpay
	lstr_lnstm.principal_balance			= ldc_prnbal
	lstr_lnstm.prncalint_amt				= ldc_prnbal
	lstr_lnstm.calint_from					= ldtm_adjdate
	lstr_lnstm.calint_to					= ldtm_adjdate
	lstr_lnstm.bfinterest_arrear			= ldc_bfintarr
	lstr_lnstm.bfinterest_return			= ldc_bfintreturn
	lstr_lnstm.interest_period			= ldc_intperiod
	lstr_lnstm.interest_arrear			= ldc_intarrbal
	lstr_lnstm.interest_return			= ldc_bfintreturn
	lstr_lnstm.moneytype_code			= "TRN"
	lstr_lnstm.remark						= ls_adjcause
	lstr_lnstm.item_status				= 1
	lstr_lnstm.entry_id						= ls_entryid
	lstr_lnstm.entry_bycoopid			= is_coopid
	
	this.of_poststm_contract( lstr_lnstm )
	
	update	lncontmaster
	set			withdrawable_amt	= :ldc_wtdbal,
				principal_balance	= :ldc_prnbal,
				interest_arrear		= :ldc_intarrbal,
				intmonth_arrear 	= :ldc_intarrmthbal, 
				prnpayment_amt	= prnpayment_amt - :ldc_prnpay,
				intpayment_amt	= intpayment_amt - :ldc_intpay,
				interest_accum		= interest_accum - :ldc_intpay,
				contract_status		= :li_contstatus,
				lastcalint_date		= :ldtm_lastcalint,
				interest_return		= 0,
				lastpayment_date	= :ldtm_lastpay
	where	coop_id 				= :is_coopcontrol
	and		loancontract_no	= :ls_contno
	using		itr_sqlca ;
	
	if itr_sqlca.sqlcode <> 0 then
		iex_exception.text	+= "ไม่สามารถปรับปรุงสัญญาสำหรับการปรับปรุงรายการรายเดือนได้ เลขสัญญา #"+ls_contno+"<br>~r~n"+itr_sqlca.sqlerrtext
		throw iex_exception
	end if

	// ถ้าเป็นรายการชำระประจำเดือน ต้องไปตั้งค้าง
	if ls_slitmtyp = "LON" then
		insert into mbmembmtharrear
			( coop_id, member_no, recv_period, seq_no, mtharritem_code, loancontract_no, period, prnarr_amt, prnarr_bal, ref_mthslipno )
		values
			( :is_coopcontrol, :ls_memno, :ls_rcvperiod, :li_kpseq, 'LON', :ls_contno, :li_periodadj, :ldc_prnpay, :ldc_prnpay, :ls_refdocno )
		using itr_sqlca ;
		
		if itr_sqlca.sqlcode <> 0 then
			iex_exception.Text = "ไม่สามารถเพิ่มรายการค้างชำระได้ เลขที่สัญญา "+ls_contno+" "+itr_sqlca.sqlerrtext
			throw iex_exception
		end if
	elseif ls_slitmtyp = "PAR" then
		update	mbmembmtharrear
		set 		prnarr_bal = nvl( prnarr_bal, 0 ) + :ldc_prnpay
		where	coop_id = :is_coopcontrol
		and		member_no = :ls_memno
		and		loancontract_no = :ls_contno
		and		mtharritem_code = 'LON'
		and		period = :li_periodadj 
		using		itr_sqlca ;

		if itr_sqlca.sqlcode <> 0 then
			iex_exception.Text = "ไม่สามารถปรับปรุงรายการค้างชำระสัญญา "+ls_contno+" งวดที่ "+string( li_periodadj )+" ได้ " + itr_sqlca.sqlerrtext
			throw iex_exception
		end if
	end if
	
	// Update กลับไปที่ตัว slip รายเดือน
	update	kpmastreceivedet
	set			adjust_prnamt				= adjust_prnamt + :ldc_prnpay,
				adjust_intamt				= adjust_intamt + :ldc_intpay,
				adjust_itemamt				= adjust_itemamt + :ldc_itempay,
				cancel_id						= :ls_entryid,
				cancel_date					= :idtm_opdate,
				keepitem_status			= -99,
				money_return_status 		= -9
	where	( coop_id				= :is_coopcontrol ) and
				( kpslip_no			= :ls_refdocno ) and
				( seq_no				= :li_kpseq )
	using		itr_sqlca ;
	
	if itr_sqlca.sqlcode <> 0 then
		iex_exception.text	+= "ไม่สามารถปรับปรุง Slip รายการรายเดือนได้~r~n"+itr_sqlca.sqlerrtext
		throw iex_exception
	end if
	
next

return 1
end function

private function integer of_post_adjmth_shr (n_ds ads_slipadj, n_ds ads_slipadjdet) throws Exception;string		ls_memno, ls_refdocno, ls_rcvperiod, ls_adjslipno, ls_adjcause, ls_slitmtyp, ls_entryid
long		ll_index, ll_count
integer	li_period, li_kpseq
dec		ldc_unitshare, ldc_itempay, ldc_bfmthitem
dec		ldc_sharestk, ldc_shareamt
datetime ldtm_refsldate, ldtm_adjdate
str_poststmshare lstr_shstm

ls_memno		= ads_slipadj.getitemstring( 1, "member_no" )
ls_adjslipno		= ads_slipadj.getitemstring( 1, "adjslip_no" )
ls_adjcause		= ads_slipadj.getitemstring( 1, "adjust_cause" )
ls_refdocno		= ads_slipadj.getitemstring( 1, "ref_slipno" )
ls_rcvperiod		= ads_slipadj.getitemstring( 1, "ref_recvperiod" )
ls_entryid		= ads_slipadj.getitemstring( 1, "entry_id" )
ldtm_refsldate	= ads_slipadj.getitemdatetime( 1, "ref_slipdate" )
ldtm_adjdate	= ads_slipadj.getitemdatetime( 1, "adjslip_date" )

ads_slipadjdet.setfilter( "slipitemtype_code in ( 'SHR', 'SAR' )" )
ads_slipadjdet.filter()

ll_count		= ads_slipadjdet.rowcount()

for ll_index = 1 to ll_count
	ls_slitmtyp		= ads_slipadjdet.getitemstring( ll_index, "slipitemtype_code" )
	li_period			= ads_slipadjdet.getitemnumber( ll_index, "periodadj_amt" )
	li_kpseq			= ads_slipadjdet.getitemnumber( ll_index, "bfmthpay_seqno" )
	ldc_itempay		= ads_slipadjdet.getitemdecimal( ll_index, "item_adjamt" )
	ldc_bfmthitem	= ads_slipadjdet.getitemdecimal( ll_index, "bfmthpay_itemamt" )
	
	select		sh.sharestk_amt, st.unitshare_value
	into		:ldc_sharestk, :ldc_unitshare
	from		shsharemaster sh join shsharetype st on sh.coop_id = st.coop_id and sh.sharetype_code = st.sharetype_code
	where	sh.coop_id = :is_coopcontrol
	and		sh.member_no = :ls_memno
	using itr_sqlca;
	
	if isnull( ldc_sharestk ) then ldc_sharestk = 0
	
	ldc_sharestk		= ldc_sharestk * ldc_unitshare
	ldc_sharestk		= ldc_sharestk - ldc_itempay
	
	// ปรับเป็นจำนวนหุ้น
	ldc_sharestk		= ldc_sharestk / ldc_unitshare
	ldc_shareamt	= ldc_itempay / ldc_unitshare
	
	lstr_shstm.memcoop_id		= is_coopcontrol
	lstr_shstm.member_no		= ls_memno
	lstr_shstm.sharetype_code	= "01"
	lstr_shstm.slip_date			= ldtm_refsldate
	lstr_shstm.operate_date		= ldtm_adjdate
	lstr_shstm.account_date		= ldtm_adjdate
	lstr_shstm.sharetime_date	= ldtm_refsldate
	lstr_shstm.document_no		= ls_refdocno
	lstr_shstm.stmitem_code		= "RPM"
	lstr_shstm.period				= li_period
	lstr_shstm.sharepay_amt	= ldc_shareamt
	lstr_shstm.sharebal_amt		= ldc_sharestk
	lstr_shstm.entry_id			= ls_entryid
	lstr_shstm.entry_bycoopid	= is_coopid
	lstr_shstm.moneytype			= "TRN"
	lstr_shstm.bfsharearr_amt	= 0
	lstr_shstm.sharearr_amt		= 0
	lstr_shstm.item_status		= 1
	lstr_shstm.ref_slipno			= ls_adjslipno
	lstr_shstm.remark				= ls_adjcause
	
	// เพิ่ม statement
	this.of_poststm_share( lstr_shstm )
	
	// ลดยอดหุ้น
	update	shsharemaster
	set			sharestk_amt = :ldc_sharestk
	where	coop_id = :is_coopcontrol
	and		member_no = :ls_memno
	and		sharetype_code = '01'
	using itr_sqlca;
	
	if itr_sqlca.sqlcode <> 0 then
		iex_exception.text = "ไม่สามารถปรับปรุงทะเบียนหุ้นได้ " + itr_sqlca.sqlerrtext
		throw iex_exception
	end if
	
	// ถ้าเป็นรายการชำระประจำเดือน ต้องไปตั้งค้าง
	if ls_slitmtyp = "SHR" then
		insert into mbmembmtharrear
			( coop_id, member_no, recv_period, seq_no, mtharritem_code, period, prnarr_amt, prnarr_bal, ref_mthslipno )
		values
			( :is_coopcontrol, :ls_memno, :ls_rcvperiod, :li_kpseq, 'SHR', :li_period, :ldc_bfmthitem, :ldc_itempay, :ls_refdocno )
		using itr_sqlca;
	
		if itr_sqlca.sqlcode <> 0 then
			iex_exception.Text = "ไม่สามารถเพิ่มรายการค้างชำระหุ้นได้ " + itr_sqlca.sqlerrtext
			throw iex_exception
		end if
	else
		update	mbmembmtharrear
		set 		prnarr_bal = nvl( prnarr_bal, 0 ) + :ldc_itempay
		where	coop_id = :is_coopcontrol
		and		member_no = :ls_memno
		and		mtharritem_code = 'SHR'
		and		period = :li_period 
		using		itr_sqlca ;

		if itr_sqlca.sqlcode <> 0 then
			iex_exception.Text = "ไม่สามารถปรับปรุงรายการค้างชำระหุ้นงวดที่ "+string( li_period )+" ได้ " + itr_sqlca.sqlerrtext
			throw iex_exception
		end if
	end if
	
	// Update กลับไปที่ตัว slip รายเดือน
	update	kpmastreceivedet
	set			adjust_itemamt				= adjust_itemamt + :ldc_itempay,
				cancel_id						= :ls_entryid,
				cancel_date					= :idtm_opdate,
				keepitem_status			= -99,
				money_return_status 		= -9
	where	( coop_id				= :is_coopcontrol ) and
				( kpslip_no			= :ls_refdocno ) and
				( seq_no				= :li_kpseq )
	using		itr_sqlca ;
	
	if itr_sqlca.sqlcode <> 0 then
		iex_exception.text	+= "ไม่สามารถปรับปรุง Slip รายการรายเดือนได้~r~n"+itr_sqlca.sqlerrtext
		throw iex_exception
	end if
	
next
			
return 1
end function

private function integer of_post_adjmth_etc (n_ds ads_slipadj, n_ds ads_slipadjdet) throws Exception;string		ls_memno, ls_refdocno, ls_rcvperiod, ls_adjslipno, ls_adjcause, ls_entryid, ls_itemtype, ls_insureno
long		ll_index, ll_count
integer	li_period, li_kpseq
dec		ldc_unitshare, ldc_itempay, ldc_bfmthitem
dec		ldc_sharestk, ldc_shareamt
datetime ldtm_refsldate, ldtm_adjdate
str_poststmshare lstr_shstm

ls_memno		= ads_slipadj.getitemstring( 1, "member_no" )
ls_adjslipno		= ads_slipadj.getitemstring( 1, "adjslip_no" )
ls_adjcause		= ads_slipadj.getitemstring( 1, "adjust_cause" )
ls_refdocno		= ads_slipadj.getitemstring( 1, "ref_slipno" )
ls_rcvperiod		= ads_slipadj.getitemstring( 1, "ref_recvperiod" )
ls_entryid		= ads_slipadj.getitemstring( 1, "entry_id" )
ldtm_refsldate	= ads_slipadj.getitemdatetime( 1, "ref_slipdate" )
ldtm_adjdate	= ads_slipadj.getitemdatetime( 1, "adjslip_date" )

ads_slipadjdet.setfilter( "slipitemtype_code not in ( 'SHR', 'SAR', 'LON', 'PAR', 'IAR' )" )
ads_slipadjdet.filter()

ll_count		= ads_slipadjdet.rowcount()

for ll_index = 1 to ll_count
	ls_itemtype		= trim( ads_slipadjdet.getitemstring( ll_index, "slipitemtype_code" ) )
	
	li_period			= ads_slipadjdet.getitemnumber( ll_index, "periodadj_amt" )
	li_kpseq			= ads_slipadjdet.getitemnumber( ll_index, "bfmthpay_seqno" )
	ldc_itempay		= ads_slipadjdet.getitemdecimal( ll_index, "item_adjamt" )
	ldc_bfmthitem	= ads_slipadjdet.getitemdecimal( ll_index, "bfmthpay_itemamt" )
	
	choose case ls_itemtype
		case "ISF"
			ls_insureno		= ads_slipadjdet.getitemstring( ll_index, "description" )
			
			update	lninsurancefire
			set			insurepay_status	= 0,
						mthkeep_status = -9
			where	( coop_id			= :is_coopcontrol )
			and		( insurance_no	= :ls_insureno )
			using		itr_sqlca ;
			
			if itr_sqlca.sqlcode <> 0 then
				iex_exception.text = "ไม่สามารถปรับปรุงสถานะการเรียกเก็บค่าเบี้ยประกันได้ #"+ls_insureno+"~r~n"+itr_sqlca.sqlerrtext
				throw iex_exception
			end if
		case "FFE"
			// ปรับสถานะค่าธรรมเนียมแรกเข้า
			update	mbmembmaster
			set			firstfee_status	= 0
			where	( coop_id			= :is_coopcontrol )
			and		( member_no	= :ls_memno )
			using		itr_sqlca ;
			
			if itr_sqlca.sqlcode <> 0 then
				iex_exception.text	+= "ไม่สามารถปรับปรุงสถานะเรียกเก็บค่าธรรมเนียมรายเดือนได้ "+ls_memno+"<br>"+itr_sqlca.sqlerrtext
				throw iex_exception
			end if
	end choose
	
	// Update กลับไปที่ตัว slip รายเดือน
	update	kpmastreceivedet
	set			adjust_itemamt				= adjust_itemamt + :ldc_itempay,
				cancel_id						= :ls_entryid,
				cancel_date					= :idtm_opdate,
				keepitem_status			= -99,
				money_return_status 		= -9
	where	( coop_id				= :is_coopcontrol ) and
				( kpslip_no			= :ls_refdocno ) and
				( seq_no				= :li_kpseq )
	using		itr_sqlca ;
	
	if itr_sqlca.sqlcode <> 0 then
		iex_exception.text	+= "ไม่สามารถปรับปรุง Slip รายการรายเดือนได้~r~n"+itr_sqlca.sqlerrtext
		throw iex_exception
	end if
	
next
			
return 1
end function

private function integer of_post_ccladj_shr (n_ds ads_slipadj, n_ds ads_slipadjdet) throws Exception;string		ls_memno , ls_refdocno, ls_rcvperiod, ls_slitmtyp
string		ls_adjslipno , ls_adjcause
integer	li_bflastperiod , li_period, li_kpseq
integer	li_itemsts
long		ll_index, ll_count
dec{2}	ldc_unitshare
dec{2}	ldc_itempay , ldc_bfmthitem
dec{3}	ldc_sharestk , ldc_shareamt
datetime	ldtm_refsldate , ldtm_adjdate
str_poststmshare	lstr_shstm

ls_adjslipno		= ads_slipadj.getitemstring( 1, "adjslip_no" )
ls_memno		= ads_slipadj.getitemstring( 1, "member_no" )
ls_refdocno		= ads_slipadj.getitemstring( 1, "ref_slipno" )
ls_rcvperiod		= ads_slipadj.getitemstring( 1, "ref_recvperiod" )
ls_adjcause		= ads_slipadj.getitemstring( 1, "adjust_cause" )
ldtm_refsldate	= ads_slipadj.getitemdatetime( 1, "ref_slipdate" )
ldtm_adjdate	= ads_slipadj.getitemdatetime( 1, "adjslip_date" )

ads_slipadjdet.setfilter( "slipitemtype_code in ( 'SHR', 'SAR' )" )
ads_slipadjdet.filter()

ll_count		= ads_slipadjdet.rowcount()

for ll_index = 1 to ll_count
	ls_slitmtyp		= trim( ads_slipadjdet.getitemstring( ll_index, "slipitemtype_code" ) )
	ldc_itempay		= ads_slipadjdet.getitemdecimal( ll_index, "item_adjamt" )
	ldc_bfmthitem	= ads_slipadjdet.getitemdecimal( ll_index , "bfmthpay_itemamt" )
	
	li_period			= ads_slipadjdet.getitemnumber( ll_index, "periodadj_amt" )
	li_kpseq			= ads_slipadjdet.getitemnumber( ll_index, "bfmthpay_seqno" )

	// ดึงข้อมูลหุ้นล่าสุด
	select sh.sharestk_amt, st.unitshare_value
	into	:ldc_sharestk, :ldc_unitshare
	from	shsharemaster sh 
			join shsharetype st on sh.coop_id = st.coop_id and sh.sharetype_code = st.sharetype_code
	where sh.coop_id = :is_coopcontrol
	and  sh.member_no = :ls_memno
	using itr_sqlca ;
	
	if isnull( ldc_sharestk ) then ldc_sharestk = 0
	
	ldc_sharestk		= ldc_sharestk * ldc_unitshare
	
	ldc_sharestk		= ldc_sharestk + ldc_itempay
	ldc_sharestk		= ldc_sharestk / ldc_unitshare
	ldc_shareamt	= ldc_itempay / ldc_unitshare
	
	lstr_shstm.memcoop_id		= is_coopcontrol
	lstr_shstm.member_no		= ls_memno
	lstr_shstm.sharetype_code	= "01"
	lstr_shstm.slip_date			= ldtm_refsldate
	lstr_shstm.operate_date		= idtm_opdate
	lstr_shstm.account_date		= idtm_opdate
	lstr_shstm.sharetime_date	= ldtm_refsldate
	lstr_shstm.document_no		= ls_refdocno
	lstr_shstm.stmitem_code		= "SPM"
	lstr_shstm.period				= li_period
	lstr_shstm.sharepay_amt	= ldc_shareamt
	lstr_shstm.sharebal_amt		= ldc_sharestk
	lstr_shstm.entry_id			= is_userid
	lstr_shstm.entry_bycoopid	= is_coopid
	lstr_shstm.moneytype			= "TRN"
	lstr_shstm.bfsharearr_amt	= 0
	lstr_shstm.sharearr_amt		= 0
	lstr_shstm.item_status		= 1
	lstr_shstm.ref_slipno			= ls_adjslipno
	lstr_shstm.remark				= ls_adjcause
	
	// เพิ่ม statement
	this.of_poststm_share( lstr_shstm )
	
	// ลดยอดหุ้น
	update 	shsharemaster
	set			sharestk_amt	= :ldc_sharestk
	where 	( coop_id = :is_coopcontrol )
	and		( member_no = :ls_memno )
	using itr_sqlca;
	
	if ( itr_sqlca.sqlcode <> 0 ) then
		iex_exception.text = "ไม่สามารถปรับปรุงทะเบียนหุ้นได้ " + itr_sqlca.sqlerrtext
		throw iex_exception
	end if
	
	if ls_slitmtyp = "SHR" then
		// ลบรายการค้างชำระออกจาก list
		delete	 from mbmembmtharrear
		where	coop_id = :is_coopcontrol
		and		member_no = :ls_memno
		and		mtharritem_code = 'SHR'
		and		recv_period = :ls_rcvperiod 
		using itr_sqlca ;
		
		if itr_sqlca.sqlcode <> 0 then
			iex_exception.Text = "ไม่สามารถลบข้อมูลค้างชำระของหุ้น "+ls_memno+" ประจำงวด "+ls_rcvperiod+" ได้"
			throw iex_exception
		end if
	elseif ls_slitmtyp = "SAR" then
		update	mbmembmtharrear
		set			prnarr_bal = nvl( prnarr_bal, 0 ) - :ldc_itempay
		where	coop_id = :is_coopcontrol
		and		member_no = :ls_memno
		and		mtharritem_code = 'SHR'
		and		period = :li_period 
		using		itr_sqlca ;

		if itr_sqlca.sqlcode <> 0 then
			iex_exception.Text = "ไม่สามารถปรับปรุงรายการค้างชำระค่าหุ้น "+ls_memno+" งวดที่ "+string( li_period )+" ได้ " + itr_sqlca.sqlerrtext
			throw iex_exception
		end if
	end if
	
	// ปรับสถานะกลับไปเป็นปกติ
	update	kpmastreceivedet
	set			adjust_itemamt				= adjust_itemamt - :ldc_itempay,
				cancel_id						= null,
				cancel_date					= null,
				keepitem_status			= 1
	where	( coop_id				= :is_coopcontrol )
	and		( kpslip_no			= :ls_refdocno )
	and		( seq_no				= :li_kpseq )
	using		itr_sqlca ;
	
	if itr_sqlca.sqlcode <> 0 then
		iex_exception.text	+= "ไม่สามารถปรับปรุง Slip รายการรายเดือนได้~r~n"+itr_sqlca.sqlerrtext
		throw iex_exception
	end if
	
next

return 1
end function

private function integer of_post_ccladj_lon (n_ds ads_slipadj, n_ds ads_slipadjdet) throws Exception;string		ls_contno, ls_memno, ls_adjslipno, ls_refdocno, ls_adjcause, ls_rcvperiod, ls_slitmtyp
integer	li_contstatus, li_odflag, li_period
integer	li_bflastperiod, li_kpseq
long		ll_index, ll_count
dec		ldc_prnbal, ldc_prnpay, ldc_intpay, ldc_intperiod, ldc_intarrpay, ldc_kpintpay, ldc_itempay
dec		ldc_bfprnbal, ldc_wtdbal, ldc_bfintreturn, ldc_bfmthprn, ldc_bfmthint, ldc_bfintarrset
dec		ldc_bfintarr, ldc_bfintmth, ldc_bfintyr, ldc_intarrbal, ldc_intarrmthbal, ldc_intarryrbal
datetime	ldtm_bflastcalint, ldtm_lastcalint, ldtm_refsldate, ldtm_adjdate, ldtm_lastproc, ldtm_lastpay
str_poststmloan lstr_lnstm

ls_adjslipno		= ads_slipadj.getitemstring( 1, "adjslip_no" )
ls_memno		= ads_slipadj.getitemstring( 1, "member_no" )
ls_refdocno		= ads_slipadj.getitemstring( 1, "ref_slipno" )
ls_rcvperiod		= ads_slipadj.getitemstring( 1, "ref_recvperiod" )
ls_adjcause		= ads_slipadj.getitemstring( 1, "adjust_cause" )
ldtm_refsldate	= ads_slipadj.getitemdatetime( 1, "ref_slipdate" )
ldtm_adjdate	= ads_slipadj.getitemdatetime( 1, "adjslip_date" )

ads_slipadjdet.setfilter("slipitemtype_code in ( 'LON', 'PAR', 'IAR' ) ")
ads_slipadjdet.filter()

ll_count		= ads_slipadjdet.rowcount()

for ll_index = 1 to ll_count
	ls_contno		= ads_slipadjdet.getitemstring( ll_index, "loancontract_no" )
	ls_slitmtyp		= ads_slipadjdet.getitemstring( ll_index, "slipitemtype_code" )
	ldc_prnpay		= ads_slipadjdet.getitemdecimal( ll_index, "principal_adjamt" )
	ldc_intpay		= ads_slipadjdet.getitemdecimal( ll_index, "interest_adjamt" )
	ldc_bfmthprn	= ads_slipadjdet.getitemdecimal( ll_index, "bfmthpay_prnamt" )
	ldc_bfmthint		= ads_slipadjdet.getitemdecimal( ll_index, "bfmthpay_intamt" )
	
	li_period			= ads_slipadjdet.getitemnumber( ll_index, "periodadj_amt" )
	li_kpseq			= ads_slipadjdet.getitemnumber( ll_index, "bfmthpay_seqno" )
	
	ldc_itempay		= ldc_prnpay + ldc_intpay
	
	// ดึงค่าสัญญาล่าสุด
	select		principal_balance, withdrawable_amt, lastcalint_date, interest_arrear,
				interest_return, od_flag, contract_status
	into		:ldc_bfprnbal, :ldc_wtdbal, :ldtm_bflastcalint, :ldc_bfintarr,
				:ldc_bfintreturn, :li_odflag, :li_contstatus
	from		lncontmaster
	where	coop_id = :is_coopcontrol
	and		loancontract_no = :ls_contno
	using itr_sqlca;
	
	if itr_sqlca.sqlcode <> 0 then
		iex_exception.text = "ไม่พบข้อมูลสัญญาเงินกู้เลขที่ : " + ls_contno
		throw iex_exception
	end if

	ldc_prnbal		= ldc_bfprnbal - ldc_prnpay
	ldc_intarrbal		= ldc_intarrbal - ldc_intpay
	
	if li_odflag = 1 then
		ldc_wtdbal	= ldc_wtdbal + ldc_prnpay
	else
		// ถ้าหนี้คงเหลือเป็น 0
		if ldc_prnbal = 0 then li_contstatus = -1
	end if
	
	// บันทึกลง Statement
	lstr_lnstm.contcoop_id			= is_coopcontrol
	lstr_lnstm.loancontract_no		= ls_contno
	lstr_lnstm.slip_date				= ldtm_refsldate
	lstr_lnstm.operate_date			= idtm_opdate
	lstr_lnstm.account_date			= idtm_opdate
	lstr_lnstm.intaccum_date			= ldtm_refsldate
	lstr_lnstm.ref_slipno				= ls_adjslipno
	lstr_lnstm.ref_docno				= ls_refdocno
	lstr_lnstm.loanitemtype_code	= "LPM"
	lstr_lnstm.period					= li_period
	lstr_lnstm.principal_payment	= ldc_prnpay
	lstr_lnstm.interest_payment		= ldc_intpay
	lstr_lnstm.principal_balance		= ldc_prnbal
	lstr_lnstm.prncalint_amt			= ldc_bfprnbal
	lstr_lnstm.calint_from				= ldtm_bflastcalint
	lstr_lnstm.calint_to				= ldtm_bflastcalint
	lstr_lnstm.bfinterest_arrear		= ldc_bfintarr
	lstr_lnstm.bfinterest_return		= ldc_bfintreturn
	lstr_lnstm.interest_period		= 0
	lstr_lnstm.interest_arrear		= ldc_intarrbal
	lstr_lnstm.interest_return		= ldc_bfintreturn
	lstr_lnstm.moneytype_code		= "TRN"
	lstr_lnstm.remark					= ls_adjcause
	lstr_lnstm.item_status			= 1
	lstr_lnstm.entry_id					= is_userid
	lstr_lnstm.entry_bycoopid		= is_coopid
	
	this.of_poststm_contract(lstr_lnstm)
	
	update lncontmaster
	set	withdrawable_amt	= :ldc_wtdbal,
		principal_balance	= :ldc_prnbal,
		interest_arrear		= :ldc_intarrbal,
		intmonth_arrear	= 0,
		prnpayment_amt	= prnpayment_amt + :ldc_prnpay,
		intpayment_amt	= intpayment_amt + :ldc_intpay,
		interest_accum		= interest_accum + :ldc_intpay,
		contract_status		= :li_contstatus,
		interest_return		= 0
	where	coop_id = :is_coopcontrol
	and 		loancontract_no = :ls_contno
	using itr_sqlca;
	
	if itr_sqlca.sqlcode <> 0 then
		iex_exception.text += "ไม่สามารถปรับปรุงสัญญาสำหรับการปรับปรุงรายการรายเดือนได้ เลขสัญญา #" + ls_contno + "<br>~r~n" + itr_sqlca.sqlerrtext
		throw iex_exception
	end if
	
	if ls_slitmtyp = "LON" then
		// ลบรายการค้างชำระออกจาก list
		delete	 from mbmembmtharrear
		where coop_id = :is_coopcontrol
		and loancontract_no = :ls_contno
		and recv_period = :ls_rcvperiod
		using itr_sqlca ;
		
		if itr_sqlca.sqlcode <> 0 then
			iex_exception.Text = "ไม่สามารถลบข้อมูลค้างชำระของสัญญา "+ls_contno+" ประจำงวด "+ls_rcvperiod+" ได้"
			throw iex_exception
		end if
	elseif ls_slitmtyp = "PAR" then
		update	mbmembmtharrear
		set			prnarr_bal = nvl( prnarr_bal, 0 ) - :ldc_prnpay
		where	coop_id = :is_coopcontrol
		and		member_no = :ls_memno
		and		loancontract_no = :ls_contno
		and		mtharritem_code = 'LON'
		and		period = :li_period 
		using		itr_sqlca ;

		if itr_sqlca.sqlcode <> 0 then
			iex_exception.Text = "ไม่สามารถปรับปรุงรายการค้างชำระสัญญา "+ls_contno+" งวดที่ "+string( li_period )+" ได้ " + itr_sqlca.sqlerrtext
			throw iex_exception
		end if
	end if

	// Update กลับไปที่ตัว slip รายเดือน
	update	kpmastreceivedet
	set			adjust_prnamt				= adjust_prnamt - :ldc_prnpay,
				adjust_intamt				= adjust_intamt - :ldc_intpay,
				adjust_itemamt				= adjust_itemamt - :ldc_itempay,
				cancel_id						= null,
				cancel_date					= null,
				keepitem_status			= 1
	where	( coop_id				= :is_coopcontrol )
	and		( kpslip_no			= :ls_refdocno )
	and		( seq_no				= :li_kpseq )
	using		itr_sqlca ;
	
	if itr_sqlca.sqlcode <> 0 then
		iex_exception.text	+= "ไม่สามารถปรับปรุง Slip รายการรายเดือนได้~r~n"+itr_sqlca.sqlerrtext
		throw iex_exception
	end if
	
next

return 1
end function

public function integer of_save_ccladj (ref str_slipadjust astr_slipadjust) throws Exception;string		ls_adjslipno, ls_refdocno
long		ll_index , ll_count
boolean	lb_error = false
n_ds		lds_slipadj, lds_slipadjdet

ls_adjslipno		= astr_slipadjust.slipadj_no
is_userid			= astr_slipadjust.entry_id
idtm_opdate		= astr_slipadjust.operate_date

lds_slipadj		= create n_ds
lds_slipadj.dataobject	= DWO_ADJSLIP
lds_slipadj.settransobject( itr_sqlca )

lds_slipadjdet		= create n_ds
lds_slipadjdet.dataobject		= DWO_ADJSLIPDET
lds_slipadjdet.settransobject( itr_sqlca )

lds_slipadj.retrieve( is_coopcontrol, ls_adjslipno )
if lds_slipadj.rowcount() <= 0 then
	iex_exception.text	= "ไม่พบข้อมุลใบปรับปรุงเลขที่ #"+ls_adjslipno+" กรุณาตรวจสอบ"
	lb_error		= true
	goto objdestroy
end if

lds_slipadjdet.retrieve( is_coopcontrol, ls_adjslipno )
if lds_slipadjdet.rowcount() <= 0 then
	iex_exception.text	= "ไม่พบข้อมุลการทำรายการ กรุณาตรวจสอบ"
	lb_error		= true
	goto objdestroy
end if

ls_refdocno		= trim( lds_slipadj.getitemstring( 1, "ref_slipno" ) )

// ปรับสถานะใบเสร็จเดือนให้เป็นปกติ
update	kpmastreceive
set			keeping_status		= 1
where	( coop_id				= :is_coopcontrol )
and		( kpslip_no			= :ls_refdocno )
using		itr_sqlca ;

if itr_sqlca.sqlcode <> 0 then
	iex_exception.text	+= "ไม่สามารถปรับปรุง Slip รายการรายเดือนได้~r~n"+itr_sqlca.sqlerrtext
	lb_error		= true
	goto objdestroy
end if

// ปรับสถานะใบปรับปรุง
update	slslipadjust
set			slip_status	= -9 ,
			cancel_id		= :is_userid ,
			cancel_date	= :idtm_opdate
where	coop_id		= :is_coopcontrol
and		adjslip_no	= :ls_adjslipno
using		itr_sqlca ;

if itr_sqlca.sqlcode <> 0 then
	iex_exception.text	+= "ไม่สามารถปรับปรุงสถานะ Slip ยกเลิกได้ เลขใบทำรายการ "+ls_adjslipno+" "+itr_sqlca.sqlerrtext
	lb_error		= true
	goto objdestroy
end if

// ทำการยกเลิก
try
	// รายการหุ้น
	this.of_post_ccladj_shr( lds_slipadj, lds_slipadjdet )
	
	// รายการหนี้
	this.of_post_ccladj_lon( lds_slipadj, lds_slipadjdet )

	// รายการอื่นๆ
	this.of_post_ccladj_etc( lds_slipadj, lds_slipadjdet )
catch( exception lex_ccladj )
	iex_exception.text	= lex_ccladj.text
	lb_error		= true
end try

objdestroy:
if isvalid( lds_slipadj ) then destroy lds_slipadj
if isvalid( lds_slipadjdet ) then destroy lds_slipadjdet

if lb_error then
	rollback using itr_sqlca ;
	throw iex_exception
end if

commit using itr_sqlca ;

return 1
end function

public function integer of_save_adjmth (ref str_slipadjust astr_kpadj) throws Exception;string		ls_xmlmain, ls_xmldet
string		ls_adjslipno, ls_slipitemtype, ls_slipitemprior, ls_refdocno
long		ll_index, ll_count, ll_seqno
dec		ldc_sumpayamt
datetime ldtm_entry
boolean	lb_error = false

n_ds	lds_slipadj, lds_slipadjdet

ls_xmlmain		= astr_kpadj.xml_sliphead
ls_xmldet		= astr_kpadj.xml_slipdet

idtm_opdate		= astr_kpadj.operate_date

ldtm_entry		= datetime( today(), now() )

this.of_setsrvdwxmlie( true )

// ดึงข้อมูลค่าคงที่ระบบจัดเก็บ
select		adj_mth_lon_flag
into		:ii_adjmthlonflag
from		kpconstant
where	coop_id		= :is_coopcontrol
using itr_sqlca;

if itr_sqlca.sqlcode <> 0 or isnull( ii_adjmthlonflag ) then
	ii_adjmthlonflag	= 1	// adj_mth_flag สถานะการคืนเงินกู้ 1 แบบตั้งค้างดอกเบี้ยเรียกเก็บ 2 แบบถอยวันที่คิดดอกเบี้ยไม่ตั้งค้าง
end if

lds_slipadj = create n_ds
lds_slipadj.dataobject = DWO_ADJSLIP
lds_slipadj.settransobject( itr_sqlca )

lds_slipadjdet = create n_ds
lds_slipadjdet.dataobject = DWO_ADJSLIPDET
lds_slipadjdet.settransobject( itr_sqlca )

try
	inv_dwxmliesrv.of_xmlimport( lds_slipadj, ls_xmlmain, false, false )
	
	inv_dwxmliesrv.of_xmlimport( lds_slipadjdet, ls_xmldet, false, false )
catch( exception lex_impdw )
	iex_exception.text		= "พบขอผิดพลาดในการนำเข้าข้อมูลปรับปรุงใบเสร็จประจำเดือน "+lex_impdw.text
	lb_error		= true
end try

if lb_error then
	goto objdestroy
end if

// กรองให้เหลือแต่พวกที่ทำรายการเท่านั้น
lds_slipadjdet.setfilter( "operate_flag > 0" )
lds_slipadjdet.filter()

// ลบพวกไม่ทำรายการทิ้งไป
lds_slipadjdet.rowsdiscard( 1, lds_slipadjdet.filteredcount(), filter! )

// ตรวจเช็คจำนวนแถวอีกที
ll_count	= lds_slipadjdet.rowcount()
if ll_count <= 0 then
	iex_exception.text	= "ไม่พบข้อมุลการทำรายการ กรุณาตรวจสอบ"
	lb_error		= true
	goto objdestroy
end if

ls_refdocno		= trim( lds_slipadj.getitemstring( 1, "ref_slipno" ) )

// ใส่ข้อมูลการปรับปรุงให้ครบถ้วน
this.of_setabsadjust( lds_slipadjdet )

// ยอดปรับปรุงทั้งหมด
ldc_sumpayamt		= dec( lds_slipadjdet.describe( "evaluate( 'sum( if( operate_flag = 1, item_adjamt * itemsign_flag, 0 ) for all )', "+string( ll_count )+" )" ) )

// ตรวจสอบการปรับปรุงยอดเกิน
this.of_check_overadj( lds_slipadjdet )

// เรียงตามประเภทรายการ
lds_slipadjdet.setsort( "slipitemtype_code asc" )
lds_slipadjdet.sort()

// ขอเลขที่ Slip
this.of_setsrvdoccontrol( true )
ls_adjslipno	= inv_docsrv.of_getnewdocno( is_coopid, "SLSLIPADJUST" )
this.of_setsrvdoccontrol( false )

// ใส่เลขที่ Slip ใน Header
lds_slipadj.setitem( 1, "coop_id", is_coopcontrol )
lds_slipadj.setitem( 1, "adjslip_no", ls_adjslipno )
lds_slipadj.setitem( 1, "slip_amt", ldc_sumpayamt )
lds_slipadj.setitem( 1, "entry_date", ldtm_entry )

// ใส่เลขที่ Slip และลำดับที่ใน detail
for ll_index = 1 to ll_count
	ls_slipitemtype		= lds_slipadjdet.getitemstring( ll_index, "slipitemtype_code" )
	
	if ls_slipitemtype <> ls_slipitemprior then
		ll_seqno			= 0
		ls_slipitemprior	= ls_slipitemtype
	end if

	ll_seqno ++

	lds_slipadjdet.setitem( ll_index, "coop_id", is_coopcontrol )
	lds_slipadjdet.setitem( ll_index, "adjslip_no", ls_adjslipno )
	lds_slipadjdet.setitem( ll_index, "seq_no", ll_seqno )
next

// ------------------------------------------
// เริ่มกระบวนการบันทึกและผ่านรายการ
// ------------------------------------------
// บันทึก Slip
if lds_slipadj.update() <> 1 then
	iex_exception.text	= "บันทึกข้อมูล Slip Adjust ไม่ได้"
	iex_exception.text	+= lds_slipadj.of_geterrormessage()
	lb_error		= true
	goto objdestroy
end if

// บันทึก Slip Detail
if lds_slipadjdet.update() <> 1 then
	iex_exception.text	= "บันทึกข้อมูล Slip Adjust Detail ไม่ได้"
	iex_exception.text	+= lds_slipadjdet.of_geterrormessage()
	lb_error		= true
	goto objdestroy
end if

// ปรับสถานะใบเสร็จเดือนให้เป็นยกเลิก
update	kpmastreceive
set			keeping_status	= -99
where	( coop_id				= :is_coopcontrol )
and		( kpslip_no			= :ls_refdocno ) 
using		itr_sqlca ;

if itr_sqlca.sqlcode <> 0 then
	iex_exception.text	+= "ไม่สามารถปรับปรุง Slip รายการรายเดือนได้~r~n"+itr_sqlca.sqlerrtext
	lb_error		= true
	goto objdestroy
end if

// ทำการผ่านรายการ
try
	// คืนรายการหุ้น
	this.of_post_adjmth_shr( lds_slipadj, lds_slipadjdet )
	
	// คืนรายการหนี้
	this.of_post_adjmth_lon( lds_slipadj, lds_slipadjdet )
	
	// คืนรายการอื่นๆ
	this.of_post_adjmth_etc( lds_slipadj, lds_slipadjdet )
catch ( exception lthw_posterr )
	iex_exception.text	= lthw_posterr.text
	lb_error		= true
end try

objdestroy:
if isvalid( lds_slipadj ) then destroy lds_slipadj
if isvalid( lds_slipadjdet ) then destroy lds_slipadjdet

if lb_error then
	rollback using itr_sqlca ;
	throw iex_exception
end if

commit using itr_sqlca ;

return 1
end function

public function integer of_init_adjmth (ref str_slipadjust astr_kpadj) throws Exception;string		ls_xmlmain
string		ls_memno, ls_sliptype, ls_kpslipno, ls_recvperiod, ls_tofromaccid
string		ls_mbname, ls_mbsurname, ls_prename, ls_mbgrp, ls_mbgrpdesc
long		ll_chkrow, ll_count
dec		ldc_receive, ldc_sumpay
datetime	ldtm_receipt
boolean	lb_error = false

n_ds	lds_slipadj, lds_slipadjdet
n_ds	lds_kpmastdet

this.of_setsrvdwxmlie( true )

lds_slipadj = create n_ds
lds_slipadj.dataobject = DWO_ADJSLIP
lds_slipadj.settransobject( itr_sqlca )

lds_slipadjdet = create n_ds
lds_slipadjdet.dataobject = DWO_ADJSLIPDET
lds_slipadjdet.settransobject( itr_sqlca )

lds_kpmastdet	= create n_ds
lds_kpmastdet.dataobject = "d_kpsrv_info_kpmastdet"
lds_kpmastdet.settransobject( itr_sqlca )

ls_xmlmain		= astr_kpadj.xml_sliphead

if inv_dwxmliesrv.of_xmlimport( lds_slipadj, ls_xmlmain ) < 1 then
	iex_exception.text += "~r~nพบขอผิดพลาดในการนำเข้าข้อมูลปรับปรุงใบเสร็จประจำเดือน"
	iex_exception.text += "~r~nกรุณาตรวจสอบ"
	lb_error = true ; Goto objdestroy
end if

ls_memno		= lds_slipadj.getitemstring( 1, "member_no" )
ls_kpslipno		= lds_slipadj.getitemstring( 1, "ref_slipno" )
ls_sliptype		= lds_slipadj.getitemstring( 1, "adjtype_code" )

ll_chkrow	= lds_kpmastdet.retrieve( is_coopcontrol, ls_kpslipno )
if ll_chkrow < 1 then
	iex_exception.text = "ไม่พบข้อมูลใบเสร็จประจำเดือน( Detail ) : " + ls_kpslipno+ " ทะเบียน : " + ls_memno+ " กรุณาตรวจสอบ"
	lb_error = true
	goto objdestroy
end if

// กำหนดหัว slip
select		mb.memb_name, mb.memb_surname, mpre.prename_desc, mb.membgroup_code, mgrp.membgroup_desc,
			km.recv_period, km.receive_amt, km.receipt_date
into		:ls_mbname, :ls_mbsurname, :ls_prename, :ls_mbgrp, :ls_mbgrpdesc,
			:ls_recvperiod, :ldc_receive, :ldtm_receipt
from		kpmastreceive km
			join mbmembmaster mb on km.coop_id = mb.coop_id and km.member_no = mb.member_no
			join mbucfprename mpre on mb.prename_code = mpre.prename_code
			join mbucfmembgroup mgrp on mb.membgroup_code = mgrp.membgroup_code
where	km.coop_id = :is_coopcontrol
and 		km.kpslip_no = :ls_kpslipno
using itr_sqlca ;

if itr_sqlca.sqlcode <> 0 then
	iex_exception.text = "ไม่สามารถดึงข้อมูลสมาชิก : " + ls_memno+ " กรุณาตรวจสอบ"
	lb_error = true
	goto objdestroy
end if

// คู่บัญชี
select		account_id
into		:ls_tofromaccid
from		cmucftofromaccid
where	coop_id = :is_coopcontrol
and		applgroup_code = 'KEP'
and		sliptype_code = 'ADJ'
and		default_flag = 1
using itr_sqlca ;

if isnull( ls_tofromaccid ) then ls_tofromaccid = ""

lds_slipadj.setitem( 1, "memb_name", ls_mbname )
lds_slipadj.setitem( 1, "memb_surname", ls_mbsurname )
lds_slipadj.setitem( 1, "prename_desc", ls_prename )
lds_slipadj.setitem( 1, "membgroup_code", ls_mbgrp )
lds_slipadj.setitem( 1, "tofrom_accid", ls_tofromaccid )

lds_slipadj.setitem( 1, "slipretall_flag", 1 )
lds_slipadj.setitem( 1, "ref_recvperiod", ls_recvperiod )
lds_slipadj.setitem( 1, "ref_slipno", ls_kpslipno )
lds_slipadj.setitem( 1, "ref_slipamt", ldc_receive )
lds_slipadj.setitem( 1, "ref_slipdate", ldtm_receipt )		

//init detail
try
	// ยกเลิกหุ้น
	this.of_init_adjmth_shr( lds_slipadjdet, lds_kpmastdet )
	
	// ยกเลิกหนี้
	this.of_init_adjmth_lon( lds_slipadjdet, lds_kpmastdet )
	
	// ยกเลิกอื่นๆ
	this.of_init_adjmth_etc( lds_slipadjdet, lds_kpmastdet )
catch( exception lex_errdet )
	iex_exception.text	= lex_errdet.text
	lb_error		= true
end try

if lb_error then
	goto objdestroy
end if

ll_count			= lds_slipadjdet.rowcount()

// ยอดปรับปรุงทั้งหมด
ldc_sumpay		= dec( lds_slipadjdet.describe( "evaluate( 'sum( if( operate_flag = 1, item_adjamt * itemsign_flag, 0 ) for all )', "+string( ll_count )+" )" ) )
lds_slipadj.setitem( 1, "slip_amt", ldc_sumpay )

astr_kpadj.xml_sliphead	= inv_dwxmliesrv.of_xmlexport( lds_slipadj )
astr_kpadj.xml_slipdet	= inv_dwxmliesrv.of_xmlexport( lds_slipadjdet )

objdestroy:
this.of_setsrvdwxmlie( false )

if isvalid(lds_slipadj) then destroy lds_slipadj
if isvalid(lds_slipadjdet) then destroy lds_slipadjdet
if isvalid(lds_kpmastdet) then destroy lds_kpmastdet

if lb_error then
	rollback using itr_sqlca ;
	iex_exception.text = "of_init_adjmth() " + iex_exception.text
	throw iex_exception
end if

return 1
end function

private function integer of_post_ccladj_etc (n_ds ads_slipadj, n_ds ads_slipadjdet) throws Exception;string		ls_memno, ls_refdocno, ls_slitmtyp
string		ls_adjslipno, ls_insureno
integer	li_bflastperiod, li_kpseq
long		ll_index, ll_count
dec{2}	ldc_itempay
str_poststmshare	lstr_shstm

ls_adjslipno		= ads_slipadj.getitemstring( 1, "adjslip_no" )
ls_memno		= ads_slipadj.getitemstring( 1, "member_no" )
ls_refdocno		= ads_slipadj.getitemstring( 1, "ref_slipno" )

ads_slipadjdet.setfilter( "slipitemtype_code not in ( 'SHR', 'SAR', 'LON', 'PAR', 'IAR' )" )
ads_slipadjdet.filter()

ll_count		= ads_slipadjdet.rowcount()

for ll_index = 1 to ll_count
	ls_slitmtyp		= trim( ads_slipadjdet.getitemstring( ll_index, "slipitemtype_code" ) )
	ldc_itempay		= ads_slipadjdet.getitemdecimal( ll_index, "item_adjamt" )
	
	li_kpseq			= ads_slipadjdet.getitemnumber( ll_index, "bfmthpay_seqno" )

	choose case ls_slitmtyp
		case "ISF"
			ls_insureno		= ads_slipadjdet.getitemstring( ll_index, "description" )
			
			update	lninsurancefire
			set			insurepay_status	= 1,
						mthkeep_status = 1
			where	( coop_id			= :is_coopcontrol )
			and		( insurance_no	= :ls_insureno )
			using		itr_sqlca ;
			
			if itr_sqlca.sqlcode <> 0 then
				iex_exception.text = "ไม่สามารถปรับปรุงสถานะการเรียกเก็บค่าเบี้ยประกันได้ #"+ls_insureno+"~r~n"+itr_sqlca.sqlerrtext
				throw iex_exception
			end if
		case "FFE"
			// ปรับสถานะค่าธรรมเนียมแรกเข้า
			update	mbmembmaster
			set			firstfee_status	= 1
			where	( coop_id			= :is_coopcontrol )
			and		( member_no	= :ls_memno )
			using		itr_sqlca ;
			
			if itr_sqlca.sqlcode <> 0 then
				iex_exception.text	+= "ไม่สามารถปรับปรุงสถานะเรียกเก็บค่าธรรมเนียมรายเดือนได้ "+ls_memno+"<br>"+itr_sqlca.sqlerrtext
				throw iex_exception
			end if
	end choose
	
	// ปรับสถานะกลับไปเป็นปกติ
	update	kpmastreceivedet
	set			adjust_itemamt				= adjust_itemamt - :ldc_itempay,
				cancel_id						= null,
				cancel_date					= null,
				keepitem_status			= 1
	where	( coop_id				= :is_coopcontrol )
	and		( kpslip_no			= :ls_refdocno )
	and		( seq_no				= :li_kpseq )
	using		itr_sqlca ;
	
	if itr_sqlca.sqlcode <> 0 then
		iex_exception.text	+= "ไม่สามารถปรับปรุง Slip รายการรายเดือนได้~r~n"+itr_sqlca.sqlerrtext
		throw iex_exception
	end if
	
next

return 1
end function

on n_cst_lnsrv_slslipadj.create
call super::create
TriggerEvent( this, "constructor" )
end on

on n_cst_lnsrv_slslipadj.destroy
TriggerEvent( this, "destructor" )
call super::destroy
end on

event destructor;this.of_setsrvdwxmlie( false )
end event

event constructor;// ประกาศ Throw สำหรับ Err
iex_exception	= create exception
end event
