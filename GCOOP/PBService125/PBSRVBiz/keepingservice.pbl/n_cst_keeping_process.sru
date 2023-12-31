$PBExportHeader$n_cst_keeping_process.sru
forward
global type n_cst_keeping_process from NonVisualObject
end type
end forward

global type n_cst_keeping_process from NonVisualObject
end type
global n_cst_keeping_process n_cst_keeping_process

type variables
string		is_coopid , is_coopcontrol , is_recvperiod, is_arg[] , is_sqlextfn
string		is_prefixslip , is_kpslipno , is_kpfind
datetime	idtm_receipt, idtm_calintto , idtm_startcont
integer	ii_proctype , ii_msgerrflag
integer	ii_rdsattypeorg, ii_rdsatsumtypeorg
long		il_slipno , il_kpcount , il_kpfind

n_ds	ids_loandata, ids_contintspc, ids_inttable
n_ds 	ids_kptp , ids_msgerr , ids_kpconstant

n_cst_datawindowsservice	inv_dwsrv
n_cst_loansrv_interest		inv_intsrv
n_cst_stringservice			inv_stringsrv
n_cst_progresscontrol		inv_progress
n_cst_datetimeservice		inv_datetimesrv
n_cst_dbconnectservice		inv_transection
n_cst_roundmoneyservice	inv_rdmoneysrv
n_cst_procservice				inv_procsrv

transaction	itr_sqlca
Exception	ithw_exception
end variables

forward prototypes
public function integer of_initservice (n_cst_dbconnectservice atr_dbtrans) throws Exception
public function integer of_setprogress (ref n_cst_progresscontrol anv_progress) throws Exception
public function integer of_rcvprocess (str_keep_proc astr_keep_proc) throws Exception
protected function decimal of_calcontinterest (string as_contno, long al_row, datetime adtm_calintfrom, datetime adtm_calintto, ref decimal adc_intperiod, datetime adtm_lastposting) throws Exception
protected function integer of_deletepriormonth (integer ai_kpmthclearamt) throws Exception
protected function integer of_deletercpitem (string as_keepgroup) throws Exception
protected function integer of_deletereceipt () throws Exception
protected function integer of_deletereport () throws Exception
protected function integer of_extfn (ref str_extfn_keep astr_extfn_keep) throws Exception
protected function integer of_genkptemp () throws Exception
protected subroutine of_getkpslip () throws Exception
protected function integer of_hd_010001 () throws Exception
protected function integer of_process_genexpense () throws Exception
protected function integer of_processdeposit () throws Exception
protected function integer of_processffee () throws Exception
protected function integer of_processloan () throws Exception
protected function integer of_processmoneyreturn () throws Exception
protected function integer of_processother () throws Exception
protected function integer of_processrecpamt () throws Exception
protected function integer of_processshare () throws Exception
protected function integer of_set_msgerr (string as_msgerr)
protected function integer of_setsqlselect (ref n_ds ads_info) throws Exception
protected function integer of_setsqlselect_kptemp (ref n_ds ads_info) throws Exception
protected function integer of_setsrvdw (boolean ab_switch) throws Exception
protected function integer of_setsrvproc (boolean ab_switch)
protected function integer of_setsrvrdmoney (boolean ab_switch)
protected function integer of_updatemem () throws Exception
protected function string of_getsqlselect () throws Exception
protected function integer of_processrecpno (n_ds ads_procdata) throws Exception
protected function integer of_updateexp () throws Exception
protected function integer of_processsharefromlncont () throws Exception
protected function integer of_processfeehnd () throws Exception
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

if isnull( is_coopcontrol ) or is_coopcontrol = ""  then is_coopcontrol = '008001'
if isnull( is_coopid ) or is_coopid = "" then is_coopid = '008001'

if isnull( inv_transection ) or not isvalid( inv_transection ) then
	inv_transection = create n_cst_dbconnectservice
	inv_transection = atr_dbtrans
end if

ids_loandata	= create n_ds
ids_loandata.dataobject	= "d_kp_rcvproc_loan"
ids_loandata.settransobject( itr_sqlca )

// สำหรับดึงเงื่อนไขดอกเบี้ยอัตราพิเศษ
ids_contintspc	= create n_ds
ids_contintspc.dataobject	= "d_kp_rcvproc_loan_intspc"
ids_contintspc.settransobject( itr_sqlca )

// สำหรับเก็บเงื่อนไข error
ids_msgerr	= create n_ds
ids_msgerr.dataobject	= "d_kp_msg_error"
ids_msgerr.settransobject( itr_sqlca )

// ค่าคงที่ระบบจัดเก็บ
ids_kpconstant = create n_ds
ids_kpconstant.dataobject	= "d_kp_constant"
ids_kpconstant.settransobject( itr_sqlca )
ids_kpconstant.retrieve(is_coopcontrol)

// การคิดดอกเบี้ย
inv_intsrv = create n_cst_loansrv_interest
inv_intsrv.of_initservice( inv_transection )

// Service สำหรับสร้าง Sql Extend
inv_stringsrv	= create n_cst_stringservice

// สร้าง Progress สำหรับแสดงสถานะการประมวลผล
inv_progress	= create n_cst_progresscontrol

inv_datetimesrv	= create n_cst_datetimeservice

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

public function integer of_rcvprocess (str_keep_proc astr_keep_proc) throws Exception;/***********************************************************
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
string		ls_group, ls_memno , ls_xmloption , ls_proctext
integer	li_year, li_month, li_sharestatus, li_loanstatus, li_ffeestatus
integer	li_depositstatus, li_moneyretstatus, li_otherstatus, li_emertrnstatus , li_upmem , li_upexp
integer	li_recpstatus, li_insfirestatus, li_clrtype, li_chk, li_rowcount , li_kpmthclearamt
integer	li_maxproc, li_count, li_mainproc, li_supproc, li_delproc
long		ll_recpno
datetime	ldtm_receiptdate, ldtm_calintdate
boolean	lb_genamt , lb_err = false

str_extfn_keep lstr_extfn_keep

n_ds	lds_procdata 
n_cst_dwxmlieservice		lnv_dwxmliesrv

this.of_setsrvdw(true)

lnv_dwxmliesrv		= create n_cst_dwxmlieservice

// สร้าง n_ds ที่เกี่ยวข้อง
lds_procdata = create n_ds
lds_procdata.dataobject = "d_kp_option_proc"
lds_procdata.settransobject( itr_sqlca )

ls_xmloption		= astr_keep_proc.xml_option

// นำเข้า XML
if lnv_dwxmliesrv.of_xmlimport( lds_procdata, ls_xmloption	 ) < 1 then
	return -1
end if

li_year				= lds_procdata.getitemnumber( 1, "receive_year" )
li_month				= lds_procdata.getitemnumber( 1, "receive_month" )
li_sharestatus		= lds_procdata.getitemnumber( 1, "share_status" )
li_loanstatus			= lds_procdata.getitemnumber( 1, "loan_status" )
li_emertrnstatus	= lds_procdata.getitemnumber( 1, "emertrn_status" )
li_ffeestatus			= lds_procdata.getitemnumber( 1, "ffee_status" )
li_depositstatus		= lds_procdata.getitemnumber( 1, "deposit_status" )
li_moneyretstatus	= lds_procdata.getitemnumber( 1, "moneyret_status" )
li_otherstatus		= lds_procdata.getitemnumber( 1, "other_status" )
li_recpstatus		= lds_procdata.getitemnumber( 1, "recpno_status" )
li_insfirestatus		= lds_procdata.getitemnumber( 1, "insurefire_status" )
ll_recpno				= lds_procdata.getitemnumber( 1, "recpno_startnum" )
li_clrtype				= lds_procdata.getitemnumber( 1, "clear_type" )
li_kpmthclearamt	= lds_procdata.getitemnumber( 1, "kpmthclear_amt" )
li_upmem			= lds_procdata.getitemnumber( 1, "upmem_status" )
li_upexp				= lds_procdata.getitemnumber( 1, "updexpense_status" )

if isnull( li_upexp ) then li_upexp = 0

ls_group				= lds_procdata.getitemstring( 1, "group_text" )
ls_memno			= lds_procdata.getitemstring( 1, "mem_text" )

// กำหนดค่าให้ instance เพื่อใช้ในฟังก์ชันอื่น ๆ
//is_coopid				= lds_procdata.getitemstring( 1, "coop_id" )
ii_proctype			= lds_procdata.getitemnumber( 1, "proc_type" )
ii_msgerrflag		= lds_procdata.getitemnumber( 1, "msgerr_flag" )
idtm_receipt			= lds_procdata.getitemdatetime( 1, "receipt_date" )
idtm_calintto		= lds_procdata.getitemdatetime( 1, "calint_date" )
idtm_startcont		= lds_procdata.getitemdatetime( 1, "startcont_date" )

is_recvperiod		= string( li_year ) + string( li_month, "00" )
is_prefixslip			= right( is_recvperiod , 4 )

lb_genamt			= false

// รูปแบบการประมวลผล old
choose case ii_proctype
	case 2	// ตามกลุ่ม
		inv_stringsrv.of_analyzestring( ls_group, is_arg[] )
		ls_proctext = ls_group
	case 3 // ตามทะเบียน
		inv_stringsrv.of_analyzestring( ls_memno, is_arg[] )
		ls_proctext = ls_memno
end choose

//// new
//// กำหนดค่าที่เตรียมประมวล
//try
//	inv_procsrv.of_initservice()
//	inv_procsrv.of_set_proctype( li_proctype ) // กำหนดวิธีดึงข้อมูล 60 ดึงข้อมูลตามทะเบียนสมาชิก
//	inv_procsrv.of_set_txtproc( ls_proctext ) // ใส่ค่าที่ดึงข้อมูล
//	inv_procsrv.of_set_analyze() // gen ข้อมูลในการดึง
//	inv_procsrv.of_set_sqlselect( "mbmembmaster") // set ค่าที่ gen ลงในตารางที่เลือก
////	inv_procsrv.of_set_sqldw( lds_info_mem ) // ใส่ค่าที่กำหนดลง n_ds
//catch( Exception lthw_setdwsql )
//	ithw_exception.text	= "~r~nพบขอผิดพลาด (0.3)"
//	ithw_exception.text	+= lthw_setdwsql.text
//	ithw_exception.text 	+= "~r~nกรุณาตรวจสอบ"
//	lb_err = true
//end try
//if lb_err then Goto objdestroy

li_mainproc	= li_sharestatus + li_loanstatus + li_ffeestatus + li_depositstatus
li_supproc	= li_moneyretstatus + li_otherstatus + li_recpstatus + li_insfirestatus + li_upmem
li_maxproc 	= li_mainproc + li_supproc + 3 // บวก ลบข้อมูล, sumยอด, ทำรายงาน

inv_progress.istr_progress.progress_max = li_maxproc
inv_progress.istr_progress.status = 8

li_count		= 1

// อัพเดทข้อมูลสมาชิก
if ( li_upexp = 1 ) then
	li_count ++
	inv_progress.istr_progress.progress_index = li_count
	il_kpfind		= 0 // clear ค่าเริ่มต้นการหาหัวใบเสร็จ
	try
		li_chk	= this.of_updateexp( )
	catch( Exception lthw_procupexp )
		ithw_exception.text	= lthw_procupexp.text
		lb_err = true
	end try
	Goto objdestroy
end if

// ** รอแก้ไขให้ลบเฉพาะพวกที่ประมวลผล
// ลบข้อมูลเดือนที่แล้ว
if this.of_deletepriormonth(li_kpmthclearamt) <> 1 then
	rollback using itr_sqlca ;
	return -1
end if

// ตรวจสอบว่า clear ข้อมูลเก่าแบบไหน
if li_clrtype = 0 then
	if this.of_deletereceipt( ) <> 1 then
		rollback using itr_sqlca ;
		return -1
	end if
else
	try
		if li_sharestatus = 1 then this.of_deletercpitem( "SHR" )
		if li_ffeestatus = 1 then this.of_deletercpitem( "FFE" )
		if li_loanstatus = 1 then this.of_deletercpitem( "LON" )
		if li_depositstatus = 1 then this.of_deletercpitem( "DEP" )
		if li_otherstatus = 1 then this.of_deletercpitem( "OTH" )
		if li_moneyretstatus = 1 then this.of_deletercpitem( "MRT" )
		if li_insfirestatus = 1 then this.of_deletercpitem( "ISF" )
		if li_emertrnstatus = 1 then this.of_deletercpitem( "ETN" )
	catch( Exception lthw_delrcpitem )
		ithw_exception.text	= lthw_delrcpitem.text
		lb_err = true
	end try
	if lb_err then Goto objdestroy
end if

// สร้างข้อมูลหัวใบเสร็จ
try
	li_chk	= this.of_genkptemp( )
catch( Exception lthw_genkptp )
	ithw_exception.text	= lthw_genkptp.text
	lb_err = true
end try
if lb_err then Goto objdestroy

// อัพเดทข้อมูลสมาชิก
if ( li_upmem = 1 ) then
	li_count ++
	inv_progress.istr_progress.progress_index = li_count
	il_kpfind		= 0 // clear ค่าเริ่มต้นการหาหัวใบเสร็จ
	try
	
			li_chk	= this.of_updatemem( )
		
	catch( Exception lthw_procupmem )
		ithw_exception.text	= lthw_procupmem.text
		lb_err = true
	end try
	if lb_err then Goto objdestroy
end if

// เริ่มประมวลผล
// ค่าธรรมเนียมแรกเข้า
if ( li_ffeestatus = 1 ) then
	li_count ++
	inv_progress.istr_progress.progress_index = li_count
	il_kpfind		= 0 // clear ค่าเริ่มต้นการหาหัวใบเสร็จ
	try
		li_chk	= this.of_processffee( )
	catch( Exception lthw_procffee )
		ithw_exception.text	= lthw_procffee.text
		lb_err = true
	end try
	if lb_err then Goto objdestroy
	lb_genamt = true
end if

// หุ้นรายเดือน
if ( li_sharestatus = 1 ) then
	li_count ++
	inv_progress.istr_progress.progress_index = li_count
	il_kpfind		= 0
	try
		li_chk	= this.of_processshare( )
	catch( Exception lthw_procshr )
		ithw_exception.text	= lthw_procshr.text
		lb_err = true
	end try
	if lb_err then Goto objdestroy
	lb_genamt = true
end if

//wa honda
//หุ้นเพิ่มรายเดือนจากเงินกู้
if ( li_sharestatus = 1 and is_coopcontrol = '009001'  ) then
	li_count ++
	inv_progress.istr_progress.progress_index = li_count
	il_kpfind		= 0
	try
		li_chk	= this.of_processsharefromlncont( )
	catch( Exception lthw_procshr2 )
		ithw_exception.text	= lthw_procshr2.text
		lb_err = true
	end try
	if lb_err then Goto objdestroy
	lb_genamt = true
end if

// หนี้เงินกู้
if ( li_loanstatus = 1 ) then
	li_count ++
	inv_progress.istr_progress.progress_index = li_count	
	il_kpfind		= 0
	try
		li_chk	= this.of_processloan( )
	catch( Exception lthw_procloan )
		ithw_exception.text	= lthw_procloan.text
		lb_err = true
	end try
	if lb_err then Goto objdestroy
	lb_genamt = true
end if


// เงินฝากรายเดือน
if ( li_depositstatus = 1 ) then
	li_count ++
	inv_progress.istr_progress.progress_index = li_count
	il_kpfind		= 0
	try
		li_chk = this.of_processdeposit( )
	catch( Exception lthw_procdep )
		ithw_exception.text	= lthw_procdep.text
		lb_err = true
	end try
	if lb_err then Goto objdestroy
	lb_genamt = true
end if

// ชำระอื่น ๆ
if ( li_otherstatus = 1 ) then
	li_count ++
	inv_progress.istr_progress.progress_index = li_count
	il_kpfind		= 0
	try
		li_chk = this.of_processother( )
	catch( Exception lthw_procother )
		ithw_exception.text	= lthw_procother.text
		lb_err = true
	end try
	if lb_err then Goto objdestroy
	lb_genamt = true
end if

if ( li_moneyretstatus = 1 and is_coopcontrol = '009001'  ) then
	inv_progress.istr_progress.progress_index = li_count
	il_kpfind		= 0
	try
		li_chk	= this.of_processfeehnd( )
	catch( Exception lthw_procmrtreturn )
	ithw_exception.text	= lthw_procmrtreturn.text
	lb_err = true
end try
	if lb_err then Goto objdestroy
	lb_genamt = true
end if
			
//// คืนเงินส่วนที่เก็บเงินเกิน
//if li_moneyretstatus = 1 then
//	li_count ++
//	inv_progress.istr_progress.progress_index = li_count
//	li_chk = this.of_processmoneyreturn( )
//	if li_chk = 1 then
//		lb_genamt = true
//	else
//		rollback using itr_sqlca ;
//		throw ithw_exception
//	end if
//end if

// จำนวนเงินรวมเก็บของแต่ละคน
if lb_genamt then
	li_count ++
	inv_progress.istr_progress.progress_index = li_count	
	il_kpfind		= 0
	try
		li_chk	= this.of_processrecpamt( )
	catch( Exception lthw_procrecamt )
		ithw_exception.text	= lthw_procrecamt.text
		lb_err = true
	end try
	if lb_err then Goto objdestroy

	// Gen ประเภทการชำระของสมาชิกแต่ละคน
	li_count ++
	inv_progress.istr_progress.progress_index = li_count	
	il_kpfind		= 0
	try
		li_chk	= this.of_process_genexpense( )
	catch( Exception lthw_procgenexp )
		ithw_exception.text	= lthw_procgenexp.text
		lb_err = true
	end try
	if lb_err then Goto objdestroy
end if

if is_coopcontrol = '010001' then
	this.of_hd_010001()
end if

// ใส่เลขที่ใบเสร็จ
if li_recpstatus = 1 then
	li_count ++
	inv_progress.istr_progress.progress_index = li_count	
	il_kpfind		= 0
	try
		li_chk = this.of_processrecpno( lds_procdata )
	catch( Exception lthw_procrecno )
		ithw_exception.text	= lthw_procrecno.text
		lb_err = true
	end try
	if lb_err then Goto objdestroy
	lb_genamt = true
end if

objdestroy:

if isvalid(ids_loandata) then destroy ids_loandata
if isvalid(ids_contintspc) then destroy ids_contintspc
if isvalid(ids_msgerr) then destroy ids_msgerr
if isvalid(ids_kpconstant) then destroy ids_kpconstant
if isvalid(ids_kptp) then destroy ids_kptp

if lb_err then
	rollback using itr_sqlca ;
	throw ithw_exception
else
	commit using itr_sqlca ;
end if

inv_progress.istr_progress.progress_text		= "อัพเดทข้อมูลใบเสร็จ"
inv_progress.istr_progress.status = 1

this.of_setsrvdw(false)

return 1
end function

protected function decimal of_calcontinterest (string as_contno, long al_row, datetime adtm_calintfrom, datetime adtm_calintto, ref decimal adc_intperiod, datetime adtm_lastposting) throws Exception;/***********************************************************
<description>
	คำนวณดอกเบี้ยสัญญาเงินกู้
</description>

<arguments>  
	as_contno				String			เลขสัญญา
	al_row					Long			แถวที่
	adtm_calintfrom		Datetime		คำนวณดอกเบี้ยจากวันที่
	adtm_calintto			Datetime		คำนวณดอกเบี้ยถึงวันที่
	adc_intperiod (ref)		decimal		ดอกเบี้ยที่คำนวณได้
</arguments> 

<return> 
		1					Integer	ถ้าไม่มีข้อผิดพลาด
</return> 

<usage> 

	Revision History:
	Version 1.0 (Initial version) Modified Date 28/9/2010 by MiT
</usage> 
***********************************************************/

string		ls_continttabcode, ls_memno, ls_loantype
string		ls_expr , ls_contno //tukATM05102015
integer	li_calintmethod, li_continttype, li_spcinttype, li_intsteptype, li_lastperiod, li_monthdiff
integer	li_countpay , li_atmflag //tukATM05102015
long		ll_ispccount, ll_ispcindex, ll_find
dec{2} ldc_prnbal, ldc_apvamt, ldc_intamt, ldc_intarrear , ldc_prnbal22 //tukATM05102015
dec{4} ldc_contintfixrate , ldc_contintincrease
dec		ldc_int1, ldc_int2, ldc_intallamt
datetime	ldtm_calintfrom, ldtm_calintto, ldtm_startcont, ldtm_firstday
date	ld_calintfrom

if adtm_calintfrom > adtm_calintto then
	adc_intperiod	= 0
	return 0
end if

ls_memno				= ids_loandata.getitemstring( al_row, "member_no" )
ls_contno			    = ids_loandata.getitemstring( al_row, "loancontract_no" ) //tukATM11092015
li_atmflag      		= ids_loandata.getitemnumber( al_row, "atm_flag"  )		//tukATM11092015
ls_loantype				= ids_loandata.getitemstring( al_row, "loantype_code" )
li_lastperiod				= ids_loandata.getitemnumber( al_row, "last_periodpay" )
ldc_prnbal				= ids_loandata.getitemdecimal( al_row, "principal_balance" )
ldc_apvamt				= ids_loandata.getitemdecimal( al_row, "loanapprove_amt" )
li_calintmethod			= ids_loandata.getitemnumber( al_row, "interest_method" )
li_continttype			= ids_loandata.getitemnumber( al_row, "int_continttype" )
ldc_contintfixrate		= ids_loandata.getitemdecimal( al_row, "int_contintrate" )
ls_continttabcode		= ids_loandata.getitemstring( al_row, "int_continttabcode" )
ldc_contintincrease	= ids_loandata.getitemdecimal( al_row, "int_contintincrease" )
li_intsteptype			= ids_loandata.getitemnumber( al_row, "int_intsteptype" )
ldtm_startcont			= ids_loandata.getitemdatetime( al_row, "startcont_date" )
ldc_intarrear			= ids_loandata.getitemdecimal( al_row, "interest_arrear" )
li_countpay				= ids_loandata.getitemnumber( al_row, "countpay_flag" )

//tukATM05102015<<
 if li_atmflag = 1 then
	select a.PRINCIPAL_BALANCE
	into :ldc_prnbal22
	from LNCONTSTATEMENT a
    where a.LOANCONTRACT_NO = :ls_contno and a.LOANITEMTYPE_CODE = 'LPM'
    and a.SEQ_NO = (select max(b.seq_no) from LNCONTSTATEMENT b 
    where b.LOANCONTRACT_NO = :ls_contno and b.LOANITEMTYPE_CODE = 'LPM') using itr_sqlca ; 
	 
	if itr_sqlca.sqlcode = 100 then
		ldc_prnbal = 0
	else
		ldc_prnbal = ldc_prnbal22
	end if
	 
end if
//tukATM05102015>>

choose case li_continttype
	case 0	// ไม่คิดดอกเบี้ย
		adc_intperiod	= 0
		
	case 1	// อัตราคงที่
		// รูปแบบการคิด ด/บ
		choose case li_calintmethod
			case 1	// รายวัน
             		         if is_coopid = '007001' then
							ldc_intallamt	= inv_intsrv.of_calculateinterest( ldc_contintfixrate, adtm_calintfrom, adtm_calintto, ldc_prnbal )
							ldc_intallamt	= round( ldc_intallamt, 2 )
						else 
									ldc_intallamt	= inv_intsrv.of_calculateinterest( ldc_contintfixrate, adtm_calintfrom, adtm_calintto, ldc_prnbal )
									ldc_intallamt	= round( ldc_intallamt, 3 )
									ldc_intallamt	= round( ldc_intallamt, 2 )
						end if
				

				ldc_intallamt	= inv_intsrv.of_roundmoney( ldc_intallamt )

			case 2
				
				ldc_intallamt	= inv_intsrv.of_computeinterest( is_coopid, as_contno, adtm_calintto)
			
			case 3	// แบบผสม
				// ถ้าคิดต่อจากสิ้นเดือนที่แล้ว ไม่ต้องนับวันคิดเป็นเดือนได้เลย
				if ( li_countpay = 1 or ls_loantype = "11" ) and ( day( date( adtm_calintfrom ) ) = 1 ) then
					li_monthdiff		= inv_datetimesrv.of_monthsafter( date( adtm_calintfrom ), date( adtm_calintto ), false )
					ldc_intallamt	= ( ldc_prnbal * ldc_contintfixrate ) / 12
					
					// คูณก่อนแล้วค่อยปัดเศษ
					ldc_intallamt	= ldc_intallamt * li_monthdiff
					ldc_intallamt	= inv_intsrv.of_roundmoney( ldc_intallamt )
				else
					// ถ้ากู้หลังสิ้นเดือนที่แล้ว (ก็คือเดือนนี้) คิดเป็นวัน
					if ( adtm_calintfrom >= adtm_lastposting ) then
						ldc_intallamt	= inv_intsrv.of_calculateinterest( ldc_contintfixrate, adtm_calintfrom, adtm_calintto, ldc_prnbal )
					else
						// ต้องคิดดอกเบี้ยสองขยัก ( กู้หลังเรียกเก็บเดือนก่อน )
						// 1. รายวัน จาก adtm_calintfrom ถึง adtm_lastposting
						ldtm_firstday	= datetime( inv_datetimesrv.of_relativemonth( date( adtm_calintfrom ), 1 ) )
						ldtm_firstday	= datetime( inv_datetimesrv.of_firstdayofmonth( date( ldtm_firstday ) ) )
						
						ldc_int1 			= inv_intsrv.of_calculateinterest( ldc_contintfixrate, adtm_calintfrom, ldtm_firstday, ldc_prnbal )
						
						// 2. รายเดือน จาก ldtm_firstday ถึง adtm_calintto
						li_monthdiff		= inv_datetimesrv.of_monthsafter( date( ldtm_firstday ), date( adtm_calintto ), false )
						ldc_int2 			= truncate( ( ( ( ldc_prnbal * ldc_contintfixrate ) / 12 ) * li_monthdiff ), 2 )
						
						// ดอกเบี้ยรวม
						ldc_intallamt	= ldc_int1 + ldc_int2
					end if
					
					ldc_intallamt	= inv_intsrv.of_roundmoney( ldc_intallamt )
					
				end if
		end choose
		
		if ldc_intallamt > 0 then
			adc_intperiod	= ldc_intallamt
		else
			adc_intperiod	= 0
		end if
		
	case 2	// ตามตารางดอกเบี้ย
		// ดึงตาราง ด/บ
		inv_intsrv.of_getinteresttable( is_coopcontrol , ls_continttabcode, adtm_calintto, ids_inttable )
		
		// อัตราด/บเพิ่มลดพิเศษ
		inv_intsrv.of_setintincrease( ldc_contintincrease )
		
		// ดูประเภทการคิด ด/บ รายวัน, รายเดือน
		if li_calintmethod = 1 then
			
			// ตรวจว่าดูอัตราด/บจากยอดอนุมัติหรือคงเหลือ
			if li_intsteptype = 1 then
				adc_intperiod	= inv_intsrv.of_calculateinterest( ids_inttable, adtm_calintfrom, adtm_calintto, ldc_prnbal, ldc_apvamt )
			else
				adc_intperiod	= inv_intsrv.of_calculateinterest( ids_inttable, adtm_calintfrom, adtm_calintto, ldc_prnbal )
			end if

		else
			// เด๋วหา code รายเดือนมาใส่ ตอนนี้เว้นไว้ก่อน 555
			adc_intperiod	= inv_intsrv.of_computeinterest( is_coopid, as_contno, adtm_calintto)
		end if
		
	case 3	// อัตรา ด/บ เป็นช่วง
		ldtm_calintfrom	= adtm_calintfrom
		
		ll_ispccount	= ids_contintspc.retrieve( is_coopid , as_contno, adtm_calintto )

		if ll_ispccount = 0 then
			adc_intperiod	= 0
		else
			// หาวันที่คิดด/บล่าสุดว่าอยู่ช่วงไหน
			ls_expr		= "'"+string( ldtm_calintfrom, 'yyyy-mm-dd' ) + "' >= string( effective_date, 'yyyy-mm-dd' )"
			ll_find		= ids_contintspc.find( ls_expr, ll_ispccount, 1 )		//  ค้นจากข้างล่างขึ้นมา จะค้นจากข้างบนลงล่างไม่ได้
			
			// ลบอัตรา ด/บ ของช่วงวันที่ที่ผ่านมาแล้ว
			if ll_find > 1 then
				ids_contintspc.rowsdiscard( 1, ll_find - 1, primary! )
			end if
			
			// เริ่มหาอัตราพิเศษ
			for ll_ispcindex = 1 to ids_contintspc.rowcount()
				
				if ll_ispcindex = ids_contintspc.rowcount() then
					ldtm_calintto		= adtm_calintto
				else
					ldtm_calintto		= ids_contintspc.getitemdatetime( ll_ispcindex + 1, "effective_date" )
				end if
				
				li_spcinttype				= ids_contintspc.getitemnumber( ll_ispcindex, "int_continttype" )
				ls_continttabcode		= ids_contintspc.getitemstring( ll_ispcindex, "int_continttabcode" )
				ldc_contintfixrate		= ids_contintspc.getitemdecimal( ll_ispcindex, "int_contintrate" )
				ldc_contintincrease	= ids_contintspc.getitemdecimal( ll_ispcindex, "int_contintincrease" )
				
				// อัตราด/บเพิ่มลดพิเศษ
				inv_intsrv.of_setintincrease( ldc_contintincrease )
				
				choose case li_spcinttype
					case 0	//	ไม่คิดดอกเบี้ย
						
						ldc_intamt = 0
						
					case 1	//	ตามอัตราที่กำหนด
						
						ldc_intamt	= inv_intsrv.of_calculateinterest( ldc_contintfixrate, ldtm_calintfrom, ldtm_calintto, ldc_prnbal )
					case 2	//	ตามตารางดอกเบี้ย
						
						// ดึงตาราง ด/บ
						inv_intsrv.of_getinteresttable( is_coopcontrol , ls_continttabcode, ldtm_calintto, ids_inttable)
						
						// ตรวจว่าดูอัตราด/บจากยอดอนุมัติหรือคงเหลือ
						if li_intsteptype = 1 then
							ldc_intamt	= inv_intsrv.of_calculateinterest( ids_inttable, ldtm_calintfrom, ldtm_calintto, ldc_prnbal, ldc_apvamt )
						else
							ldc_intamt	= inv_intsrv.of_calculateinterest( ids_inttable, ldtm_calintfrom, ldtm_calintto, ldc_prnbal )
						end if
						
				end choose
				
				ldtm_calintfrom		= ldtm_calintto
				adc_intperiod		+= ldc_intamt
			next
		end if
end choose

if adc_intperiod < 0 then
	adc_intperiod = 0
end if
	
return 1
end function

protected function integer of_deletepriormonth (integer ai_kpmthclearamt) throws Exception;/***********************************************************
<description>
	ลบข้อมูลใบเสร็จงวดก่อนหน้าจาก kptempreceive + kptempreceivedet
</description>

<arguments>  
	
</arguments> 

<return> 
		1					Integer	ถ้าไม่มีข้อผิดพลาด
</return> 

<usage> 
	หมายเหตุ  kptempreceive & kptempreceivedet ต้องมีการเซต reference แบบ CASCADE ด้วย
	จึงจะสามารถลบข้อมูลได้
		
	Revision History:
	Version 1.0 (Initial version) Modified Date 28/9/2010 by MiT
</usage> 
***********************************************************/
string		ls_recvclear
string 	ls_sqlext , ls_sql
integer	li_chk , li_year , li_month
boolean	lb_err = false

inv_progress.istr_progress.progress_index		= 1 // การลบข้อมูล progress index นับเป็น 1 เสมอ
inv_progress.istr_progress.progress_text		= "กำลัง Clear ข้อมูลเรียกเก็บเดือนก่อน"
inv_progress.istr_progress.subprogress_text	= "กำลัง Clear ข้อมูลเรียกเก็บเดือนก่อน"

ls_recvclear				= is_recvperiod

yield()
if inv_progress.of_is_stop() then
	return 0
end if

choose case ii_proctype
	case 2
		inv_stringsrv.of_buildsqlext( is_arg[], "membgroup_code", ls_sqlext )
		ls_sqlext			= " and ( ( memcoop_id , member_no ) in ( select coop_id , member_no from mbmembmaster where coop_id = '"+is_coopid+"' and "+ ls_sqlext +" ) )"
		
	case 3
		inv_stringsrv.of_buildsqlext( is_arg[], "member_no", ls_sqlext )
		ls_sqlext			= " and "+ ls_sqlext
		
end choose

ls_sql		= " delete from kptempreceive "
ls_sql		+= " where	( recv_period	= '"+ ls_recvclear + "' ) "
ls_sql		+= " and		coop_id			= '"+ is_coopid + "' "
ls_sql		+= " and exists ( select 1 from mbmembmaster where mbmembmaster.coop_id = kptempreceive.memcoop_id "
ls_sql		+= " and mbmembmaster.member_no = kptempreceive.member_no " + ls_sqlext
ls_sql		+= " ) "

execute immediate :ls_sql using itr_sqlca;

if ( itr_sqlca.sqlcode <> 0 ) then
	ithw_exception.text += "<br />พบข้อผิดพลาด ขณะ Clear ข้อมูลเรียกเก็บเดือนก่อน"
	ithw_exception.text += "<br />~r~n" + string( itr_sqlca.sqlcode ) + "  " + itr_sqlca.sqlerrtext
	throw ithw_exception
end if

if is_coopcontrol = '010001' then
	
	if ii_proctype = 1 then
		delete from kptempreceivedet where recv_period = :is_recvperiod using itr_sqlca ;
		delete from kpreceiveexpense where recv_period = :is_recvperiod using itr_sqlca ;
		delete from kptempreceive where recv_period = :is_recvperiod using itr_sqlca ;
	else
	
		delete from kptempreceive 
		where coop_id = :is_coopcontrol and recv_period = :is_recvperiod
		and exists( select 1 from ( 	select k.coop_id , k.kpslip_no
											from kptempreceive k
											where k.coop_id = :is_coopcontrol
											and k.recv_period = :is_recvperiod
											minus
											select kd.coop_id , kd.kpslip_no
											from kptempreceivedet kd
											where kd.coop_id = :is_coopcontrol
											and kd.recv_period = :is_recvperiod
											group by kd.coop_id , kd.kpslip_no
											) kt
					where kt.coop_id = kptempreceive.coop_id
					and kt.kpslip_no = kptempreceive.kpslip_no
					)
		using itr_sqlca ;
		
		insert into kptempreceive
		( coop_id , kpslip_no , memcoop_id , member_no , recv_period , refmemcoop_id , ref_membno , membtype_code , department_code , membsection_code , 
		membgroup_code , process_date , receipt_no , receipt_date , operate_date , sharestkbf_value , sharestk_value , interest_accum , keeping_status , receive_amt , 
		money_text , grt_list , moneytype_code , tofrom_accid , misspay_status , bank_code , bank_branch , bank_accid , receipt_amt , receipt_amttext , expense_code , 
		expense_bank , expense_branch , expense_accid , last_seq_no , savedisk_type , trtype_code , keepsal_flag )
		select kp.coop_id , ( substr( kp.kpslip_no , 1 , 4 ) || '00' || substr( kp.kpslip_no , 7 , 6 ) )  as kpslip_no, kp.memcoop_id , kp.member_no , kp.recv_period , kp.refmemcoop_id , '' as ref_membno , kp.membtype_code , kp.department_code , kp.membsection_code , 
		kp.membgroup_code , kp.process_date , kp.receipt_no , kp.receipt_date , kp.operate_date , kp.sharestkbf_value , kp.sharestk_value , kp.interest_accum , kp.keeping_status , kp.receive_amt , 
		kp.money_text , kp.grt_list , kp.moneytype_code , kp.tofrom_accid , kp.misspay_status , kp.bank_code , kp.bank_branch , kp.bank_accid , kp.receipt_amt , kp.receipt_amttext , kp.expense_code , 
		kp.expense_bank , kp.expense_branch , kp.expense_accid , 0 as last_seq_no , kp.savedisk_type , 'KEEP1' as trtype_code , 0 as keepsal_flag
		from kptempreceive kp
		where 	kp.coop_id = :is_coopcontrol
		and 		kp.recv_period = :is_recvperiod
		and 		( kp.coop_id , kp.kpslip_no ) in ( select coop_id , kpslip_no from kptempreceive where coop_id = :is_coopcontrol and recv_period = :is_recvperiod and ( ref_membno = 'KEEPOUT' or trtype_code = 'COLL' ) )
		and ( kp.coop_id , kp.member_no ) in ( select k.coop_id , k.member_no from kptempreceive k where k.coop_id = :is_coopcontrol and k.recv_period = :is_recvperiod group by k.coop_id , k.member_no having count( k.member_no) = 1 )
		using itr_sqlca ;
		if ( itr_sqlca.sqlcode <> 0 ) then
			ithw_exception.text += "<br />พบข้อผิดพลาด ปรับปรุงใบผ่านรายการเรียกเก็บ(0.15)"
			ithw_exception.text += "<br />~r~n" + string( itr_sqlca.sqlcode ) + "  " + itr_sqlca.sqlerrtext
			lb_err = true ; Goto objdestroy
		end if
		/*
		ปรับกับเป็นปกติก่อนเดียวค่อยไปแยกทีหลัง
		*/
		update	kptempreceivedet ktd
		set 		ktd.kpslip_no = trim( substr( ktd.kpslip_no , 1 , 4 ) || '00' || substr( ktd.kpslip_no , 7 , 6 ) )
		where 	ktd.coop_id = :is_coopcontrol
		and 		ktd.recv_period = :is_recvperiod
		and 		( ktd.coop_id , ktd.kpslip_no ) in ( select coop_id , kpslip_no from kptempreceive where coop_id = :is_coopcontrol and recv_period = :is_recvperiod and ( ref_membno = 'KEEPOUT' or trtype_code = 'COLL' ) )
		and 		( ktd.coop_id , trim( substr( ktd.kpslip_no , 1 , 4 ) || '00' || substr( ktd.kpslip_no , 7 , 6 ) ) ) in ( select coop_id , kpslip_no from kptempreceive where coop_id = :is_coopcontrol and recv_period = :is_recvperiod )
		using itr_sqlca ;
		if ( itr_sqlca.sqlcode <> 0 ) then
			ithw_exception.text += "<br />พบข้อผิดพลาด ปรับปรุงใบผ่านรายการเรียกเก็บ(0.20)"
			ithw_exception.text += "<br />~r~n" + string( itr_sqlca.sqlcode ) + "  " + itr_sqlca.sqlerrtext
			lb_err = true ; Goto objdestroy
		end if
		
		delete from kptempreceive where coop_id = :is_coopcontrol and recv_period = :is_recvperiod and ( ref_membno = 'KEEPOUT' or trtype_code = 'COLL' ) 
		and ( coop_id , trim( substr( kpslip_no , 1 , 4 ) || '00' || substr( kpslip_no , 7 , 6 ) ) ) in ( select coop_id , kpslip_no from kptempreceive where coop_id = :is_coopcontrol and recv_period = :is_recvperiod )
		using itr_sqlca ;
		if ( itr_sqlca.sqlcode <> 0 ) then
			ithw_exception.text += "<br />พบข้อผิดพลาด ลบข้อมูลใบผ่านรายการเรียกเก็บ(0.30)"
			ithw_exception.text += "<br />~r~n" + string( itr_sqlca.sqlcode ) + "  " + itr_sqlca.sqlerrtext
			lb_err = true ; Goto objdestroy
		end if
		
	//	update kptempreceive k
	//	set k.last_seq_no = nvl(( 	select max( kd.seq_no ) 
	//								from kptempreceivedet kd
	//								where kd.coop_id = k.coop_id
	//								and kd.kpslip_no = k.kpslip_no
	//								and kd.coop_id = :is_coopcontrol
	//								and kd.recv_period = :is_recvperiod
	//								) , 0 )
	//	where k.coop_id = :is_coopcontrol
	//	and k.recv_period = :is_recvperiod
	//	using itr_sqlca ;
	
	end if
		
end if

update kptempreceive k
set k.last_seq_no = nvl(( 	select max( kd.seq_no ) 
							from kptempreceivedet kd
							where kd.coop_id = k.coop_id
							and kd.kpslip_no = k.kpslip_no
							and kd.coop_id = :is_coopcontrol
							and kd.recv_period = :is_recvperiod
							) , 0 )
where k.coop_id = :is_coopcontrol
and k.recv_period = :is_recvperiod
using itr_sqlca ;

objdestroy:
if lb_err then
	ithw_exception.text	= "n_cst_keeping_process.of_deletepriormonth() " + ithw_exception.text
	throw ithw_exception
end if

//li_month			= integer(right( is_recvperiod , 2 )) - ai_kpmthclearamt
//li_year			= mod( li_month , 12 )

////hardcode ลบสองเดือนย้อนหลังไปก่อน
//li_month			= integer(right( trim(is_recvperiod) , 2 )) - 2
//li_year			= integer(left( trim(is_recvperiod) , 2 ))
//if li_month < 1 then
//	
//end if

//ls_recvclear				= is_recvperiod
//
//yield()
//if inv_progress.of_is_stop() then
//	return 0
//end if

//// ลบข้อมูลเรียกเก็บเดือนที่แล้ว
//delete from kptempreceive
//where	( recv_period	< :ls_recvclear )
//and		coop_id			= :is_coopid
//using		itr_sqlca;
//
//if ( itr_sqlca.sqlcode <> 0 ) then
//	ithw_exception.text += "พบข้อผิดพลาด ขณะ Clear ข้อมูลเรียกเก็บเดือนก่อน~r~n"
//	ithw_exception.text += "~r~n" + string( itr_sqlca.sqlcode ) + "  " + itr_sqlca.sqlerrtext
//	throw ithw_exception
//end if

return 1
end function

protected function integer of_deletercpitem (string as_keepgroup) throws Exception;/***********************************************************
<description>
	ลบข้อมูลรายการใบเสร็จงวดนี้จาก kptempreceive + kptempreceivedet
	กรณีมีการประมวลผลซ้ำ
</description>

<arguments>  
	as_keepgroup		String		กลุ่มรหัสรายการที่ทำการลบ
</arguments> 

<return> 
		1					Integer	ถ้าไม่มีข้อผิดพลาด
</return> 

<usage> 
		
	Revision History:
	Version 1.0 (Initial version) Modified Date 28/9/2010 by MiT
</usage> 
***********************************************************/

string		ls_sqlext, ls_sql, ls_sqlup, ls_sqlupext, ls_temp, ls_sqlshrup, ls_sqlshrupext
string		ls_sqlexp
long		ll_index
n_ds	lds_dbdelete

inv_progress.istr_progress.progress_index		= 1 // การลบข้อมูล progress index นับเป็น 1 เสมอ
inv_progress.istr_progress.progress_text		= "กำลัง Clear ข้อมูลเก่า"
inv_progress.istr_progress.subprogress_text	= "กำลังลบข้อมูลรายการ "+as_keepgroup

yield()
if inv_progress.of_is_stop() then
	return 0
end if

ls_sqlup		= "update lncontmaster " + &
				   "set nkeep_principal = 0, nkeep_interest = 0, rkeep_principal = 0, rkeep_interest = 0, lastprocess_date = lastkeeping_date " + &
				   "where coop_id = '"+is_coopid+"' "

ls_sqlshrup	= "update shsharemaster set rkeep_sharevalue = 0, lastprocess_date = lastkeeping_date " + &
				   "where coop_id = '"+is_coopid+"' "

ls_sql			= "delete from kptempreceivedet " + &
				   "where	 ( recv_period = '"+is_recvperiod+"' ) " + &
				   "and coop_id = '"+is_coopid+"' "
					
ls_sqlexp		= "delete from kpreceiveexpense " + &
				   " where ( recv_period = '" + is_recvperiod + "' ) " + &
				   " and coop_id = '"+ is_coopid + "' "

choose case ii_proctype
	case 1
		ls_sqlupext		= "and ( contract_status > 0 ) "
		ls_sqlshrupext	= ""
		ls_sqlext			= ""
		
	case 2
		inv_stringsrv.of_buildsqlext( is_arg[], "membgroup_code", ls_sqlext )
		ls_sqlupext		= " and ( contract_status > 0 and member_no in ( select member_no from mbmembmaster where coop_id = '"+is_coopid+"' and "+ ls_sqlext +" ) )"
		ls_sqlshrupext	= " and ( member_no in ( select member_no from mbmembmaster where coop_id = '"+is_coopid+"' and "+ ls_sqlext +" ) )"
		ls_sqlext			= " and ( member_no in ( select member_no from mbmembmaster where coop_id = '"+is_coopid+"' and "+ ls_sqlext +" ) )"
		
	case 3
		inv_stringsrv.of_buildsqlext( is_arg[], "member_no", ls_sqlext )
		
		ls_sqlupext		= " and contract_status > 0 and "+ ls_sqlext 
		ls_sqlshrupext	= " and "+ ls_sqlext 
		ls_sqlext			= " and "+ ls_sqlext
		
end choose

choose case upper( trim( as_keepgroup ) )
	case "LON"    // หนี้
		ls_temp	= ls_sql + "and ( keepitemtype_code	in ('L01','L02','L03','L04','IAR','DPL') ) "
		ls_temp	+= ls_sqlext
		execute immediate :ls_temp using itr_sqlca;
		if itr_sqlca.sqlcode = -1 then return -1

		ls_temp	= ls_sqlup + ls_sqlupext
		execute immediate :ls_temp using itr_sqlca;
		if itr_sqlca.sqlcode = -1 then return -1

	case "SHR"  // หุ้น
		ls_temp	= ls_sql + "and ( keepitemtype_code	in ('S01','DPS') ) "
		ls_temp	+= ls_sqlext
		execute immediate :ls_temp using itr_sqlca;
		if itr_sqlca.sqlcode = -1 then return -1

		ls_temp	= ls_sqlshrup + ls_sqlshrupext
		execute immediate :ls_temp using itr_sqlca;
		if itr_sqlca.sqlcode = -1 then return -1
		
	case "FFE" // ค่าธรรมเนียม
		ls_temp	= ls_sql + "and ( keepitemtype_code	= 'FFE' ) "
		ls_temp	+= ls_sqlext
		execute immediate :ls_temp using itr_sqlca;
		if itr_sqlca.sqlcode = -1 then return -1

	case "DEP" // เงินฝาก
		ls_temp	= ls_sql + "and ( keepitemtype_code	in ('D00', 'D01', 'D02') ) "
		ls_temp	+= ls_sqlext
		execute immediate :ls_temp using itr_sqlca;
		if itr_sqlca.sqlcode = -1 then return -1
		
	case "OTH"
		ls_temp	= ls_sql + "and ( keepitemtype_code	in ('UIR', 'UIA', 'OTH' , 'ISF' ) ) "
		ls_temp	+= ls_sqlext
		execute immediate :ls_temp using itr_sqlca;
		if itr_sqlca.sqlcode = -1 then return -1
		
	case else
		ls_temp	= ls_sql + "and ( keepitemtype_code	= '"+as_keepgroup+"' ) "
		ls_temp	+= ls_sqlext
		execute immediate :ls_temp using itr_sqlca;
		if itr_sqlca.sqlcode = -1 then return -1
		
end choose

// ลบข้อมูลประเภทการชำระเงินทุกครั้ง
ls_temp = ls_sqlexp + ls_sqlext
execute immediate :ls_temp using itr_sqlca;
if itr_sqlca.sqlcode = -1 then return -1

//// ลบข้อมูลที่ไม่มีรายละเอียดแล้ว
//delete from kptempreceive
//where not exists 
//	(	select 	1 
//		from 		kptempreceivedet 
//		where	kptempreceive.kpslip_no = kptempreceivedet.kpslip_no 
//		and		kptempreceive.coop_id = kptempreceivedet.coop_id )
//and	kptempreceive.coop_id = :is_coopid
//using itr_sqlca ;

update kptempreceive
set last_seq_no = 0
where kptempreceive.coop_id = :is_coopid
and	kptempreceive.recv_period = :is_recvperiod
using itr_sqlca ;

update kptempreceive
set last_seq_no = nvl( 
							( select max( ktd.seq_no )
							from kptempreceivedet ktd
							where ktd.coop_id = kptempreceive.coop_id
							and ktd.kpslip_no = kptempreceive.kpslip_no )
							, 0 )
where exists
				(	select 1 
				from kptempreceivedet 
				where	kptempreceive.kpslip_no = kptempreceivedet.kpslip_no 
				and	kptempreceive.coop_id = kptempreceivedet.coop_id 
				and	kptempreceive.coop_id = :is_coopid
				and	kptempreceive.recv_period = :is_recvperiod)
and	kptempreceive.coop_id = :is_coopid
and	kptempreceive.recv_period = :is_recvperiod
using itr_sqlca ;

return 1
end function

protected function integer of_deletereceipt () throws Exception;/***********************************************************
<description>
	ลบข้อมูลใบเสร็จงวดนี้จาก kptempreceive + kptempreceivedet
	กรณีมีการประมวลผลซ้ำ
</description>

<arguments>  
	
</arguments> 

<return> 
		1					Integer	ถ้าไม่มีข้อผิดพลาด
</return> 

<usage> 
		
	Revision History:
	Version 1.0 (Initial version) Modified Date 28/9/2010 by MiT
</usage> 
***********************************************************/
string		ls_sqlext, ls_sql, ls_sqlup, ls_sqlupext, ls_temp, ls_sqldet, ls_sqlshrup, ls_sqlshrupext
string		ls_sqlexp
integer	li_chk
boolean	lb_err = true

inv_progress.istr_progress.progress_index		= 1 // การลบข้อมูล progress index นับเป็น 1 เสมอ
inv_progress.istr_progress.progress_text		= "กำลัง Clear ข้อมูลเก่า"
inv_progress.istr_progress.subprogress_text	= "กำลังลบข้อมูลเรียกเก็บทั้งหมด"

yield()
if inv_progress.of_is_stop() then
	ithw_exception.text += "~r~nยกเลิกโดยผู้ใช้ระบบ"
	lb_err = false ; Goto objdestroy
end if

// syntax สำหรับ clear ค่า สัญญา
ls_sqlup		= "update lncontmaster " + &
				   "set nkeep_principal = 0, nkeep_interest = 0, rkeep_principal = 0, rkeep_interest = 0, lastprocess_date = lastkeeping_date " + &
				   "where coop_id = '" +is_coopid+ "' "

// syntax สำหรับ clear ค่า หุ้น
ls_sqlshrup	= "update shsharemaster set rkeep_sharevalue = 0, lastprocess_date = lastkeeping_date " + &
				   "where coop_id = '" +is_coopid+ "' "

// syntax สำหรับ ลบ master ใบเสร็จ
ls_sql	= "delete from kptempreceive " + &
		   "where	( recv_period	= '"+is_recvperiod+"' ) " + &
		   "and coop_id = '" +is_coopid+ "' "

ls_sqlexp		= "delete from kpreceiveexpense " + &
				   " where ( recv_period = '" + is_recvperiod + "' ) " + &
				   " and coop_id = '"+ is_coopid + "' "

choose case ii_proctype
	case 1
		ls_sqlupext	= "and ( contract_status > 0 ) "
		ls_sqlext		= ""
		
      case 2
		inv_stringsrv.of_buildsqlext(is_arg[], "membgroup_code", ls_sqlext)

		ls_sqlupext		= " and ( contract_status > 0 and member_no in ( select member_no from mbmembmaster where "+ ls_sqlext + " and coop_id = '"+is_coopid+"' ) )"
		ls_sqlshrupext	= " and ( member_no in ( select member_no from mbmembmaster where "+ ls_sqlext + " and coop_id = '"+is_coopid+"' ) )"
		ls_sqlext			= " and ( member_no in ( select member_no from mbmembmaster where "+ ls_sqlext + " and coop_id = '"+is_coopid+"' ) )"
		
	case 3
		inv_stringsrv.of_buildsqlext( is_arg[], "member_no", ls_sqlext )
		
		ls_sqlupext		= " and ( contract_status > 0 ) and "+ ls_sqlext
		ls_sqlshrupext	= " and "+ ls_sqlext
		ls_sqlext			= " and "+ ls_sqlext
		
end choose

// เคลียร์ยอดรอเรียกเก็บหนี้
ls_temp	= ls_sqlup + ls_sqlupext
execute immediate :ls_temp using itr_sqlca;   
yield()
if inv_progress.of_is_stop() then
	ithw_exception.text += "~r~nยกเลิกโดยผู้ใช้ระบบ"
	lb_err = false ; Goto objdestroy
end if
if itr_sqlca.sqlcode = -1 then
	ithw_exception.text += "พบข้อผิดพลาด ขณะ Clear ยอดรอเรียกเก็บหนี้ คุณต้องการประมวลผลต่อหรือไม่"
	ithw_exception.text += "~r~n"+itr_sqlca.sqlerrtext
	lb_err = false ; Goto objdestroy
end if

// เคลียร์ยอดรอเรียกเก็บหุ้น
ls_temp	= ls_sqlshrup + ls_sqlshrupext
execute immediate :ls_temp using itr_sqlca;   
yield()
if inv_progress.of_is_stop() then
	ithw_exception.text += "~r~nยกเลิกโดยผู้ใช้ระบบ"
	lb_err = false ; Goto objdestroy
end if
if itr_sqlca.sqlcode = -1 then
	ithw_exception.text += "พบข้อผิดพลาด ขณะ Clear ยอดรอเรียกเก็บหุ้นรายเดือน คุณต้องการประมวลผลต่อหรือไม่"
	ithw_exception.text += "~r~n"+itr_sqlca.sqlerrtext
	lb_err = false ; Goto objdestroy
end if

// เคลียร์ใบเสร็จ ทั้งใบ ( เส้น relation ต้องเป็น cascade เท่านั้นไม่เช่นนั้นจะมีข้อผิดพลาดเพราะไม่ลบข้อมูลรายการด้วย )
ls_temp	= ls_sql + ls_sqlext
execute immediate :ls_temp using itr_sqlca;
yield()
if inv_progress.of_is_stop() then
	ithw_exception.text += "~r~nยกเลิกโดยผู้ใช้ระบบ"
	lb_err = false ; Goto objdestroy
end if
if itr_sqlca.sqlcode = -1 then
	ithw_exception.text += "พบข้อผิดพลาด ขณะ Clear ข้อมูลเดิม คุณต้องการประมวลผลต่อหรือไม่"
	ithw_exception.text += "~r~n"+itr_sqlca.sqlerrtext
	lb_err = false ; Goto objdestroy
end if

// clear ข้อมูลประเภทการชำระยอดเรียกเก็บ
ls_temp = ls_sqlexp + ls_sqlext
execute immediate :ls_temp using itr_sqlca;

yield()
if inv_progress.of_is_stop() then
	ithw_exception.text += "~r~nยกเลิกโดยผู้ใช้ระบบ"
	lb_err = false ; Goto objdestroy
end if
if itr_sqlca.sqlcode = -1 then 
	ithw_exception.text += "พบข้อผิดพลาด ขณะ Clear ข้อมูลประเภทการชำระยอดเรียกเก็บ คุณต้องการประมวลผลต่อหรือไม่"
	ithw_exception.text += "~r~n"+itr_sqlca.sqlerrtext
	lb_err = false ; Goto objdestroy
end if

objdestroy:
if lb_err then
	rollback using itr_sqlca;
	ithw_exception.text = "n_cst_keeping_process.of_deletereceipt()" + ithw_exception.text
	throw ithw_exception
end if

return 1
end function

protected function integer of_deletereport () throws Exception;/***********************************************************
<description>
	ลบข้อมูลใบเสร็จงวดนี้จาก kptempreceive + kptempreceivedet
	กรณีมีการประมวลผลซ้ำ
</description>

<arguments>  
	
</arguments> 

<return> 
		1					Integer	ถ้าไม่มีข้อผิดพลาด
</return> 

<usage> 
		
	Revision History:
	Version 1.0 (Initial version) Modified Date 28/9/2010 by MiT
</usage> 
***********************************************************/
string		ls_sqlext, ls_sql, ls_temp

inv_progress.istr_progress.progress_text		= "จัดทำรายงานงบหน้าเรียกเก็บ"
inv_progress.istr_progress.subprogress_text	= "กำลัง Clear ข้อมูลรายงาน"

yield()

// syntax สำหรับ ลบ รายงาน
ls_sql	= "delete from kprcvprocreport " + &
		   "where	( recv_period	= '"+is_recvperiod+"' ) "

choose case ii_proctype
	case 1
		ls_sqlext		= ""
      case 2
		inv_stringsrv.of_buildsqlext(is_arg[], "membgroup_code", ls_sqlext)

		ls_sqlext		= " and ( member_no in ( select member_no from mbmembmaster where "+ ls_sqlext + " ) )"
	case 3
		inv_stringsrv.of_buildsqlext( is_arg[], "member_no", ls_sqlext )
		
		ls_sqlext		= " and "+ ls_sqlext
end choose

// เคลียร์รายงาน
ls_temp	= ls_sql + ls_sqlext
execute immediate :ls_temp using itr_sqlca;
yield()
if itr_sqlca.sqlcode = -1 then
	ithw_exception.text += "พบข้อผิดพลาด ขณะ Clear ข้อมูลเดิม คุณต้องการประมวลผลต่อหรือไม่~r~n"+itr_sqlca.sqlerrtext
	return -1
end if

return 1
end function

protected function integer of_extfn (ref str_extfn_keep astr_extfn_keep) throws Exception;string		ls_function , ls_return

ls_function		= astr_extfn_keep.function_name

choose case lower(trim( ls_function) )
	case 'of_kp_proc_getsql_memkptemp'
		// ดึง sql คนที่เคยประมวลผลเรียกเก็บไปแล้ว
		// is_coopid
		
		choose case is_coopcontrol
			case "E017001"
				ls_return		= "select trim( kptempreceive.coop_id ) as coop_id , trim( kptempreceive.kpslip_no ) as kpslip_no, trim( kptempreceive.memcoop_id ) as memcoop_id , trim( kptempreceive.member_no ) as member_no , trim( kptempreceive.recv_period ) as recv_period, "
				ls_return		+= "trim( kptempreceive.refmemcoop_id ) as refmemcoop_id , trim( kptempreceive.ref_membno ) as ref_membno , trim( kptempreceive.membgroup_code ) as membgroup_code , nvl( kptempreceive.process_date , null ) as process_date , trim( kptempreceive.receipt_no ) as receipt_no , "
				ls_return		+= "nvl( kptempreceive.receipt_date , null ) as receipt_date, nvl( kptempreceive.operate_date , null ) as operate_date, "
				ls_return		+= "nvl( kptempreceive.sharestkbf_value , 0 ) as sharestkbf_value, nvl( kptempreceive.sharestk_value , 0 ) as sharestk_value, nvl( kptempreceive.interest_accum , 0 ) as interest_accum, "
				ls_return		+= "nvl( kptempreceive.keeping_status , 0 ) as keeping_status , nvl( kptempreceive.receive_amt , 0 ) as receive_amt, trim( kptempreceive.money_text ) as money_text , trim( kptempreceive.grt_list ) as grt_list, "
				ls_return		+= "nvl( kptempreceive.misspay_status , 0 ) as misspay_status , "
				ls_return		+= "nvl( kptempreceive.expense_code , '' ) as expense_code , nvl( kptempreceive.expense_bank , '' ) as expense_bank , nvl( kptempreceive.expense_branch , '' ) as expense_branch , nvl( kptempreceive.expense_accid , '' ) as expense_accid , "
				ls_return		+= "nvl( kptempreceive.trtype_code , '' ) as trtype_code , "
				ls_return		+= "nvl( kptempreceive.receipt_amt , 0 ) as receipt_amt , trim( kptempreceive.receipt_amttext ) as receipt_amttext , "
				ls_return		+= "nvl( kptempreceive.last_seq_no , 0 ) as last_seq_no , trim( kptempreceive.savedisk_type ) as savedisk_type , "
				ls_return		+= "nvl( kptempreceive.membtype_code , null ) as membtype_code , nvl( kptempreceive.department_code , null ) as department_code "
				ls_return		+= "from kptempreceive, mbmembmaster "
				ls_return		+= "where kptempreceive.coop_id = mbmembmaster.coop_id "
				ls_return		+= "and kptempreceive.member_no = mbmembmaster.member_no "
				ls_return		+= "and kptempreceive.coop_id = '"+ is_coopcontrol +"' "
			case else
				ls_return		= "SELECT trim( KPTEMPRECEIVE.COOP_ID ) as coop_id , trim( KPTEMPRECEIVE.KPSLIP_NO ) as kpslip_no, trim( KPTEMPRECEIVE.MEMCOOP_ID ) as memcoop_id , trim( KPTEMPRECEIVE.MEMBER_NO ) as member_no , trim( KPTEMPRECEIVE.RECV_PERIOD ) as recv_period, "
				ls_return		+= "trim( KPTEMPRECEIVE.REFMEMCOOP_ID ) as refmemcoop_id , trim( KPTEMPRECEIVE.REF_MEMBNO ) as ref_membno , trim( KPTEMPRECEIVE.MEMBGROUP_CODE ) as membgroup_code , nvl( KPTEMPRECEIVE.PROCESS_DATE , null ) as process_date , trim( KPTEMPRECEIVE.RECEIPT_NO ) as receipt_no ,   "
				ls_return		+= "nvl( KPTEMPRECEIVE.RECEIPT_DATE , null ) as receipt_date, nvl( KPTEMPRECEIVE.OPERATE_DATE , null ) as operate_date, nvl( KPTEMPRECEIVE.SHARESTKBF_VALUE , 0 ) as sharestkbf_value, nvl( KPTEMPRECEIVE.SHARESTK_VALUE , 0 ) as sharestk_value, nvl( KPTEMPRECEIVE.INTEREST_ACCUM , 0 ) as interest_accum,   "
		//		ls_return		+= "nvl( KPTEMPRECEIVE.KEEPING_STATUS , 0 ) as keeping_status , nvl( KPTEMPRECEIVE.RECEIVE_AMT , 0 ) as receive_amt, trim( KPTEMPRECEIVE.MONEY_TEXT ) as money_text , trim( KPTEMPRECEIVE.GRT_LIST ) as grt_list, trim( KPTEMPRECEIVE.MONEYTYPE_CODE ) as moneytype_code ,   "
				ls_return		+= "nvl( KPTEMPRECEIVE.KEEPING_STATUS , 0 ) as keeping_status , nvl( KPTEMPRECEIVE.RECEIVE_AMT , 0 ) as receive_amt, trim( KPTEMPRECEIVE.MONEY_TEXT ) as money_text , trim( KPTEMPRECEIVE.GRT_LIST ) as grt_list,  "
		//		ls_return		+= "trim( KPTEMPRECEIVE.TOFROM_ACCID ) as tofrom_accid , nvl( KPTEMPRECEIVE.MISSPAY_STATUS , 0 ) as misspay_status , trim( KPTEMPRECEIVE.BANK_CODE ) as bank_code , trim( KPTEMPRECEIVE.BANK_BRANCH ) as bank_branch,   "
				ls_return		+= "nvl( KPTEMPRECEIVE.MISSPAY_STATUS , 0 ) as misspay_status , "
		//		ls_return		+= "trim( KPTEMPRECEIVE.BANK_ACCID ) as bank_accid, nvl( KPTEMPRECEIVE.RECEIPT_AMT , 0 ) as receipt_amt, trim( KPTEMPRECEIVE.RECEIPT_AMTTEXT ) as receipt_amttext ,  trim( KPTEMPRECEIVE.EXPENSE_CODE ) as expense_code , trim( KPTEMPRECEIVE.EXPENSE_BANK ) as expense_bank ,   "
				ls_return		+= " nvl( kptempreceive.expense_code , '' ) as expense_code , nvl( kptempreceive.expense_bank , '' ) as expense_bank , nvl( kptempreceive.expense_branch , '' ) as expense_branch , nvl( kptempreceive.expense_accid , '' ) as expense_accid , "
				ls_return		+= " nvl( kptempreceive.trtype_code , '' ) as trtype_code , "
				ls_return		+= "nvl( KPTEMPRECEIVE.RECEIPT_AMT , 0 ) as receipt_amt , trim( KPTEMPRECEIVE.RECEIPT_AMTTEXT ) as receipt_amttext , "
		//		ls_return		+= "trim( KPTEMPRECEIVE.EXPENSE_BRANCH ) as expense_branch, trim( KPTEMPRECEIVE.EXPENSE_ACCID ) as expense_accid , nvl( KPTEMPRECEIVE.LAST_SEQ_NO , 0 ) as last_seq_no , trim( KPTEMPRECEIVE.SAVEDISK_TYPE ) as SAVEDISK_TYPE , "
				ls_return		+= "nvl( KPTEMPRECEIVE.LAST_SEQ_NO , 0 ) as last_seq_no , trim( KPTEMPRECEIVE.SAVEDISK_TYPE ) as SAVEDISK_TYPE , "
		//		ls_return		+= "nvl( kptempreceive.membtype_code , null ) as membtype_code , nvl( kptempreceive.department_code , null ) as department_code , trim( kptempreceive.membsection_code ) as membsection_code "
				ls_return		+= "nvl( kptempreceive.membtype_code , null ) as membtype_code , nvl( kptempreceive.department_code , null ) as department_code "
				ls_return		+= "FROM KPTEMPRECEIVE, MBMEMBMASTER  "
				ls_return		+= "WHERE ( KPTEMPRECEIVE.MEMBER_NO = MBMEMBMASTER.MEMBER_NO ) "
				ls_return		+= "and  ( KPTEMPRECEIVE.COOP_ID = MBMEMBMASTER.COOP_ID ) "
				ls_return		+= "and kptempreceive.coop_id = '"+ is_coopcontrol +"' "
		end choose

		astr_extfn_keep.function_return	= ls_return
	
	case 'of_kp_proc_setupdatesql_memkptemp_end'
		// set sql อัพเดทลง table kptempreceive
		
		astr_extfn_keep.column_key[1]				= 'COOP_ID'
		astr_extfn_keep.column_key[2]				= 'KPSLIP_NO'
		astr_extfn_keep.column_update[1]		= 'RECEIPT_NO'
		astr_extfn_keep.column_update[2]		= 'RECEIVE_AMT'
		astr_extfn_keep.column_update[3]		= 'MONEY_TEXT'
		astr_extfn_keep.column_update[4]		= 'LAST_SEQ_NO'
	
	case 'of_kp_proc_setupdatesql_memkptemp'
		// set sql อัพเดทลง table kptempreceive
		
		astr_extfn_keep.column_key[1]				= 'COOP_ID'
		astr_extfn_keep.column_key[2]				= 'KPSLIP_NO'
		astr_extfn_keep.column_update[1]		= 'COOP_ID'
		astr_extfn_keep.column_update[2]		= 'KPSLIP_NO'
		astr_extfn_keep.column_update[3]		= 'MEMCOOP_ID'
		astr_extfn_keep.column_update[4]		= 'MEMBER_NO'
		astr_extfn_keep.column_update[5]		= 'RECV_PERIOD'
		astr_extfn_keep.column_update[6]		= 'REFMEMCOOP_ID'
		astr_extfn_keep.column_update[7]		= 'REF_MEMBNO'
		astr_extfn_keep.column_update[8]		= 'MEMBGROUP_CODE'
		astr_extfn_keep.column_update[9]		= 'PROCESS_DATE'
		astr_extfn_keep.column_update[10]		= 'RECEIPT_NO'
		astr_extfn_keep.column_update[11]		= 'RECEIPT_DATE'
		astr_extfn_keep.column_update[12]		= 'OPERATE_DATE'
		astr_extfn_keep.column_update[13]		= 'SHARESTKBF_VALUE'
		astr_extfn_keep.column_update[14]		= 'SHARESTK_VALUE'
		astr_extfn_keep.column_update[15]		= 'INTEREST_ACCUM'
		astr_extfn_keep.column_update[16]		= 'KEEPING_STATUS'
		astr_extfn_keep.column_update[17]		= 'RECEIVE_AMT'
		astr_extfn_keep.column_update[18]		= 'MONEY_TEXT'
		astr_extfn_keep.column_update[19]		= 'GRT_LIST'
		astr_extfn_keep.column_update[20]		= 'MONEYTYPE_CODE'
		astr_extfn_keep.column_update[21]		= 'TOFROM_ACCID'
		astr_extfn_keep.column_update[22]		= 'MISSPAY_STATUS'
		astr_extfn_keep.column_update[23]		= 'BRANCH_ID'
		astr_extfn_keep.column_update[24]		= 'BANK_CODE'
		astr_extfn_keep.column_update[25]		= 'BANK_BRANCH'
		astr_extfn_keep.column_update[26]		= 'BANK_ACCID'
		astr_extfn_keep.column_update[27]		= 'RECEIPT_AMT'
		astr_extfn_keep.column_update[28]		= 'RECEIPT_AMTTEXT'
		astr_extfn_keep.column_update[29]		= 'EXPENSE_CODE'
		astr_extfn_keep.column_update[30]		= 'EXPENSE_BANK'
		astr_extfn_keep.column_update[31]		= 'EXPENSE_BRANCH'
		astr_extfn_keep.column_update[32]		= 'EXPENSE_ACCID'
		astr_extfn_keep.column_update[33]		= 'LAST_SEQ_NO'
		astr_extfn_keep.column_update[34]		= 'SAVEDISK_TYPE'
		astr_extfn_keep.column_update[35]		= 'MEMBTYPE_CODE'
		astr_extfn_keep.column_update[36]		= 'DEPARTMENT_CODE'
		astr_extfn_keep.column_update[37]		= 'TRTYPE_CODE'
		
	case 'of_kp_proc_getsql_memkpinfo'
		// ดึง sql คนที่ประมวลผล
		// is_coopid
		choose case is_coopcontrol
			case "E017001"
				ls_return		= "select ( shsharemaster.sharestk_amt * shsharetype.unitshare_value ) as sharestk_value , ( shsharemaster.sharebegin_amt * shsharetype.unitshare_value ) as sharestkbf_value , "
				ls_return		+= "nvl( mbmembmaster.accum_interest , 0 ) as accum_interest , "
				ls_return		+= "trim(mbmembmaster.member_no) as member_no , trim(mbmembmaster.membgroup_code) as membgroup_code , nvl( mbmembmaster.compoundkeep_status , 0 ) as compoundkeep_status , trim( mbmembmaster.compoundkeep_group ) as compoundkeep_group , "
				ls_return		+= "trim(mbucfmembgroup.savedisk_type) as savedisk_type , nvl( mbmembmaster.membtype_code , null ) as membtype_code , nvl( mbmembmaster.department_code , null ) as department_code , "
				ls_return		+= "trim(mbmembmaster.expense_code) as expense_code , trim(mbmembmaster.expense_bank) as expense_bank , trim(mbmembmaster.expense_branch) as expense_branch , trim(mbmembmaster.expense_accid) as expense_accid , "
				ls_return		+= " 'KEEP1' as trtype_code "
				ls_return		+= "from shsharemaster, shsharetype , mbmembmaster , mbucfmembgroup , ( "
				ls_return		+= "	select mbkp.coop_id , mbkp.member_no "
				ls_return		+= "	from( "
				ls_return		+= "		select mbmembmaster.coop_id , mbmembmaster.member_no "
				ls_return		+= "		from shsharemaster, shsharetype , mbmembmaster , mbucfmembgroup "
				ls_return		+= "		where	mbmembmaster.coop_id	= '" + is_coopcontrol + "' "
				ls_return		+= "		and mbmembmaster.coop_id = mbucfmembgroup.coop_id(+) "
				ls_return		+= "		and mbmembmaster.membgroup_code = mbucfmembgroup.membgroup_code(+) "
				ls_return		+= "		and shsharemaster.coop_id = mbmembmaster.coop_id "
				ls_return		+= "		and shsharetype.coop_id = shsharemaster.coop_id "
				ls_return		+= "		and ( shsharetype.sharetype_code = shsharemaster.sharetype_code ) "
				ls_return		+= "		and ( shsharemaster.member_no = mbmembmaster.member_no ) "
				ls_return		+= "		and mbmembmaster.member_status = 1 "
				ls_return		+= "		and mbmembmaster.pausekeep_flag = 0 "
				ls_return		+= "		and mbmembmaster.memref_flag = 0 "
				ls_return		+= "		union "
				ls_return		+= "		select mbmembmaster.coop_id , mbmembmaster.member_ref "
				ls_return		+= "		from shsharemaster, shsharetype , mbmembmaster , mbucfmembgroup "
				ls_return		+= "		where	mbmembmaster.coop_id	= '" + is_coopcontrol + "' "
				ls_return		+= "		and mbmembmaster.coop_id = mbucfmembgroup.coop_id(+) "
				ls_return		+= "		and mbmembmaster.membgroup_code = mbucfmembgroup.membgroup_code(+) "
				ls_return		+= "		and shsharemaster.coop_id = mbmembmaster.coop_id "
				ls_return		+= "		and shsharetype.coop_id = shsharemaster.coop_id "
				ls_return		+= "		and ( shsharetype.sharetype_code = shsharemaster.sharetype_code ) "
				ls_return		+= "		and ( shsharemaster.member_no = mbmembmaster.member_no ) "
				ls_return		+= "		and mbmembmaster.member_status = 1 "
				ls_return		+= "		and mbmembmaster.pausekeep_flag = 0 "
				ls_return		+= "		and mbmembmaster.memref_flag = 1 "
				ls_return		+= "	) mbkp "
				ls_return		+= "	group by mbkp.coop_id , mbkp.member_no "
				ls_return		+= ")mk "
				ls_return		+= "where	mbmembmaster.coop_id	= '" + is_coopcontrol + "' "
				ls_return		+= "and mbmembmaster.coop_id = mbucfmembgroup.coop_id(+) "
				ls_return		+= "and mbmembmaster.membgroup_code = mbucfmembgroup.membgroup_code(+) "
				ls_return		+= "and shsharemaster.coop_id = mbmembmaster.coop_id "
				ls_return		+= "and shsharetype.coop_id = shsharemaster.coop_id "
				ls_return		+= "and ( shsharetype.sharetype_code = shsharemaster.sharetype_code ) "
				ls_return		+= "and ( shsharemaster.member_no = mbmembmaster.member_no ) "
				ls_return		+= "and mbmembmaster.coop_id = mk.coop_id "
				ls_return		+= "and mbmembmaster.member_no = mk.member_no "
				ls_return		+= "and mbmembmaster.member_status = 1 "
				ls_return		+= "and mbmembmaster.pausekeep_flag = 0 "
				ls_return		+= "and mbmembmaster.memref_flag = 0 "
			case "008001"
				ls_return		= "select mb.sharestk_value , mb.sharestkbf_value , "
				ls_return		+= "mb.accum_interest , "
				ls_return		+= "( case mbmembmaster.memref_flag when 1 then mbmembmaster.coop_id else '' end ) as refmemcoop_id , "
				ls_return		+= "( case mbmembmaster.memref_flag when 1 then mbmembmaster.member_ref else '' end ) as ref_membno , "
				ls_return		+= "mb.member_no , mb.membgroup_code , mb.compoundkeep_status , mb.compoundkeep_group , "
				ls_return		+= "mb.savedisk_type , mb.membtype_code , mb.department_code , "
				ls_return		+= "mb.expense_code , mb.expense_bank , mb.expense_branch , mb.expense_accid , "
				ls_return		+= "mb.trtype_code "
				ls_return		+= "from mbmembmaster , ( "
				ls_return		+= "	select ( shsharemaster.sharestk_amt * shsharetype.unitshare_value ) as sharestk_value , ( shsharemaster.sharebegin_amt * shsharetype.unitshare_value ) as sharestkbf_value , "
				ls_return		+= "	nvl( mbmembmaster.accum_interest , 0 ) as accum_interest , "
				ls_return		+= "	trim(mbmembmaster.coop_id) as coop_id , trim(mbmembmaster.member_no) as member_no , trim(mbmembmaster.membgroup_code) as membgroup_code , nvl( mbmembmaster.compoundkeep_status , 0 ) as compoundkeep_status , trim( mbmembmaster.compoundkeep_group ) as compoundkeep_group , "
				ls_return		+= "	trim(mbucfmembgroup.savedisk_type) as savedisk_type , nvl( mbmembmaster.membtype_code , null ) as membtype_code , nvl( mbmembmaster.department_code , null ) as department_code , "
				ls_return		+= "	trim(mbmembmaster.expense_code) as expense_code , trim(mbmembmaster.expense_bank) as expense_bank , trim(mbmembmaster.expense_branch) as expense_branch , trim(mbmembmaster.expense_accid) as expense_accid , "
				ls_return		+= "	 'KEEP1' as trtype_code "
				ls_return		+= "	from shsharemaster, shsharetype , mbmembmaster , mbucfmembgroup "
				ls_return		+= "	where	mbmembmaster.coop_id	= '" + is_coopcontrol + "' "
				ls_return		+= "	and mbmembmaster.coop_id = mbucfmembgroup.coop_id(+) "
				ls_return		+= "	and mbmembmaster.membgroup_code = mbucfmembgroup.membgroup_code(+) "
				ls_return		+= "	and shsharemaster.coop_id = mbmembmaster.coop_id "
				ls_return		+= "	and shsharetype.coop_id = shsharemaster.coop_id "
				ls_return		+= "	and ( shsharetype.sharetype_code = shsharemaster.sharetype_code ) "
				ls_return		+= "	and ( shsharemaster.member_no = mbmembmaster.member_no ) "
				ls_return		+= "	and mbmembmaster.member_status = 1 "
				ls_return		+= "	and mbmembmaster.pausekeep_flag = 0 "
				ls_return		+= "	union "
				ls_return		+= "	select ( 0.00 ) as sharestk_value , ( 0.00 ) as sharestkbf_value ,  "
				ls_return		+= "	( 0.00 ) as accum_interest , "
				ls_return		+= "	trim(mbmembmaster.coop_id) as coop_id , trim(mbmembmaster.member_no) as member_no , trim(mbmembmaster.membgroup_code) as membgroup_code , nvl( mbmembmaster.compoundkeep_status , 0 ) as compoundkeep_status , trim( mbmembmaster.compoundkeep_group ) as compoundkeep_group , "
				ls_return		+= "	trim(mbucfmembgroup.savedisk_type) as savedisk_type , nvl( mbmembmaster.membtype_code , null ) as membtype_code , nvl( mbmembmaster.department_code , null ) as department_code , "
				ls_return		+= "	trim(mbmembmaster.expense_code) as expense_code , trim(mbmembmaster.expense_bank) as expense_bank , trim(mbmembmaster.expense_branch) as expense_branch , trim(mbmembmaster.expense_accid) as expense_accid , "
				ls_return		+= " 	'KEEP1' as trtype_code "
				ls_return		+= "	from mbmembmaster , mbucfmembgroup  "
				ls_return		+= "	where	mbmembmaster.coop_id	= '" + is_coopcontrol + "'  "
				ls_return		+= "	and mbmembmaster.coop_id = mbucfmembgroup.coop_id(+)  "
				ls_return		+= "	and mbmembmaster.membgroup_code = mbucfmembgroup.membgroup_code(+)  "
				ls_return		+= "	and mbmembmaster.member_status = 1  "
				ls_return		+= "	and mbmembmaster.pausekeep_flag = 0  "
				ls_return		+= "	and not exists ( select 1 from shsharemaster where shsharemaster.coop_id = mbmembmaster.coop_id and shsharemaster.member_no = mbmembmaster.member_no ) "
				ls_return		+= ")mb "
				ls_return		+= "where mb.coop_id = mbmembmaster.coop_id "
				ls_return		+= "and mb.member_no = mbmembmaster.member_no "
				ls_return		+= "and mbmembmaster.coop_id = '" + is_coopcontrol + "'  "
				
			case else

				ls_return		= "select ( shsharemaster.sharestk_amt * shsharetype.unitshare_value ) as sharestk_value , ( shsharemaster.sharebegin_amt * shsharetype.unitshare_value ) as sharestkbf_value , "
				ls_return		+= "nvl( mbmembmaster.accum_interest , 0 ) as accum_interest , "
				ls_return		+= "( case mbmembmaster.memref_flag when 1 then mbmembmaster.coop_id else '' end ) as refmemcoop_id , "
				ls_return		+= "( case mbmembmaster.memref_flag when 1 then mbmembmaster.member_ref else '' end ) as ref_membno , "
				ls_return		+= "trim(mbmembmaster.member_no) as member_no , trim(mbmembmaster.membgroup_code) as membgroup_code , nvl( mbmembmaster.compoundkeep_status , 0 ) as compoundkeep_status , trim( mbmembmaster.compoundkeep_group ) as compoundkeep_group , "
				ls_return		+= "trim(mbucfmembgroup.savedisk_type) as savedisk_type , nvl( mbmembmaster.membtype_code , null ) as membtype_code , nvl( mbmembmaster.department_code , null ) as department_code , "
				ls_return		+= "trim(mkp.expense_code) as expense_code , trim(mkp.expense_bank) as expense_bank , trim(mkp.expense_branch) as expense_branch , trim(mkp.expense_accid) as expense_accid , "
				ls_return		+= " 'KEEP1' as trtype_code "
				ls_return		+= "from shsharemaster, shsharetype , mbmembmaster , mbucfmembgroup , "
				/*ดึงวิธีชำระยอดส่งหักประจำเดือนจาก mbmembmaster รวมกับ mbmembmoneytr*/
				ls_return		+= "( "
				ls_return		+= "	select rank() over ( partition by member_no order by priority_amt desc ) as priority_no , coop_id , member_no , moneytype_code as expense_code , bank_code as expense_bank , bank_branch as expense_branch , bank_accid as expense_accid "
				ls_return		+= "	from( "
				ls_return		+= "		select 100 as priority_amt , mt.coop_id , mt.member_no , mt.moneytype_code , mt.bank_code , mt.bank_branch , mt.bank_accid "
				ls_return		+= "		from mbmembmoneytr mt "
				ls_return		+= "		where mt.coop_id =  '" + is_coopcontrol + "' "
				ls_return		+= "		and mt.trtype_code = 'KEEP1' "
				ls_return		+= "		group by mt.coop_id , mt.member_no , mt.moneytype_code , mt.bank_code , mt.bank_branch , mt.bank_accid "
				ls_return		+= "		union "
				ls_return		+= "		select 10 as priority_amt , m.coop_id , m.member_no , m.expense_code , m.expense_bank , m.expense_branch , m.expense_accid "
				ls_return		+= "		from mbmembmaster m "
				ls_return		+= "		where m.coop_id =  '" + is_coopcontrol + "' "
				ls_return		+= "		group by m.coop_id , m.member_no , m.expense_code , m.expense_bank , m.expense_branch , m.expense_accid "
				ls_return		+= "		) "
				ls_return		+= ")mkp , "
				/*สมาชิกส่งใบเสร็จรวมกับสมาชิกงดส่งใบเสร็จแต่ต้องการแสดงยอดคงเหลือหุ้นหนี้*/
				ls_return		+= "( "
				ls_return		+= "	select coop_id , member_no "
				ls_return		+= "	from( "
				ls_return		+= "		select coop_id , member_no "
				ls_return		+= "		from mbmembmaster "
				ls_return		+= "		where coop_id = '" + is_coopcontrol + "' "
				ls_return		+= "		and pausekeep_flag = 0 "
				ls_return		+= "		union "
				ls_return		+= "		select m.coop_id , m.member_no "
				ls_return		+= "		from mbmembmaster m , kpconstant kc "
				ls_return		+= "		where m.coop_id = kc.coop_id "
				ls_return		+= "		and m.coop_id = '" + is_coopcontrol + "' "
				ls_return		+= "		and m.pausekeep_flag = 1 "
				ls_return		+= "		and kc.showsl_flag = 1 "
				ls_return		+= "		)mkc "
				ls_return		+= ")mem "
				ls_return		+= "where mbmembmaster.coop_id	=  '" + is_coopcontrol + "' "
				ls_return		+= "and mbmembmaster.coop_id = mbucfmembgroup.coop_id(+) "
				ls_return		+= "and mbmembmaster.membgroup_code = mbucfmembgroup.membgroup_code(+) "
				ls_return		+= "and shsharemaster.coop_id = mbmembmaster.coop_id "
				ls_return		+= "and shsharetype.coop_id = shsharemaster.coop_id "
				ls_return		+= "and ( shsharetype.sharetype_code = shsharemaster.sharetype_code ) "
				ls_return		+= "and ( shsharemaster.member_no = mbmembmaster.member_no ) "
				ls_return		+= "and mbmembmaster.coop_id = mkp.coop_id "
				ls_return		+= "and mbmembmaster.member_no = mkp.member_no "
				ls_return		+= "and mbmembmaster.coop_id = mem.coop_id "
				ls_return		+= "and mbmembmaster.member_no = mem.member_no "
				ls_return		+= "and mbmembmaster.member_status = 1 "
//				ls_return		+= "and mbmembmaster.pausekeep_flag = 0 "
				ls_return		+= "and mkp.priority_no = 1 "
				
		end choose
		
		astr_extfn_keep.function_return	= ls_return
	case 'of_kp_proc_getsql_ffee'
		// ดึง sql ค่าธรรมเนียม
		// is_coopid
		choose case is_coopcontrol
			case "E017001"
				ls_return		= "select mf.coop_id , mf.member_no , mf.member_ref , mf.appltype_code , mf.first_fee , mf.membgroup_code , "
				ls_return		+= "mf.compoundkeep_group , mf.compoundkeep_status "
				ls_return		+= "from "
				ls_return		+= "( "
				ls_return		+= "	select trim( mbmembmaster.coop_id ) as coop_id , trim( mbmembmaster.member_no ) as member_no , '' as member_ref , "
				ls_return		+= "	trim( mbucfappltype.appltype_code ) as appltype_code , "
				ls_return		+= "	nvl( mbucfappltype.first_fee , 0 ) as first_fee, trim( mbmembmaster.membgroup_code ) as membgroup_code , "
				ls_return		+= "	trim( mbmembmaster.compoundkeep_group ) as compoundkeep_group , nvl( mbmembmaster.compoundkeep_status , 0 ) as compoundkeep_status "
				ls_return		+= "	from mbmembmaster, mbucfappltype "
				ls_return		+= "	where mbmembmaster.coop_id = '"+ is_coopcontrol +"' "
				ls_return		+= "	and mbmembmaster.coop_id = mbucfappltype.coop_id(+) "
				ls_return		+= "	and ( mbmembmaster.appltype_code = mbucfappltype.appltype_code (+)) "
				ls_return		+= "	and ( ( mbmembmaster.member_status = 1 ) "
				ls_return		+= "	and ( mbmembmaster.pausekeep_flag = 0 ) "
				ls_return		+= "	and ( mbmembmaster.firstfee_status = 0 ) "
				ls_return		+= "	and ( mbmembmaster.memref_flag = 0 ) ) "
				ls_return		+= "	union "
				ls_return		+= "	select trim( mbmembmaster.coop_id ) as coop_id , trim( mbmembmaster.member_ref ) as member_no , trim( mbmembmaster.member_no ) as member_ref , "
				ls_return		+= "	trim( mbucfappltype.appltype_code ) as appltype_code , "
				ls_return		+= "	nvl( mbucfappltype.first_fee , 0 ) as first_fee, trim( mbmembmaster.membgroup_code ) as membgroup_code , "
				ls_return		+= "	trim( mbmembmaster.compoundkeep_group ) as compoundkeep_group , nvl( mbmembmaster.compoundkeep_status , 0 ) as compoundkeep_status "
				ls_return		+= "	from mbmembmaster, mbucfappltype "
				ls_return		+= "	where mbmembmaster.coop_id = '"+ is_coopcontrol +"' "
				ls_return		+= "	and mbmembmaster.coop_id = mbucfappltype.coop_id(+) "
				ls_return		+= "	and ( mbmembmaster.appltype_code = mbucfappltype.appltype_code (+)) "
				ls_return		+= "	and ( ( mbmembmaster.member_status = 1 ) "
				ls_return		+= "	and ( mbmembmaster.pausekeep_flag = 0 ) "
				ls_return		+= "	and ( mbmembmaster.firstfee_status = 0 ) "
				ls_return		+= "	and ( mbmembmaster.memref_flag = 1 ) ) "
				ls_return		+= ") mf , mbmembmaster "
				ls_return		+= "where mf.coop_id = mbmembmaster.coop_id "
				ls_return		+= "and mf.member_no = mbmembmaster.member_no "
				ls_return		+= "and ( mbmembmaster.coop_id = '" + is_coopcontrol + "' ) "
				
			case else
				ls_return		= "SELECT trim( mbmembmaster.coop_id ) as coop_id , trim( MBMEMBMASTER.MEMBER_NO ) as member_no , "
				ls_return		+= "( case mbmembmaster.memref_flag when 1 then mbmembmaster.coop_id else '' end ) as refmemcoop_id , "
				ls_return		+= "( case mbmembmaster.memref_flag when 1 then mbmembmaster.member_ref else '' end ) as member_ref , "
				ls_return		+= "trim( MBUCFAPPLTYPE.APPLTYPE_CODE ) as appltype_code , "
				ls_return		+= "nvl( MBUCFAPPLTYPE.FIRST_FEE , 0 ) as first_fee, trim( MBMEMBMASTER.MEMBGROUP_CODE ) as membgroup_code , trim( MBMEMBMASTER.COMPOUNDKEEP_GROUP ) as compoundkeep_group , nvl( MBMEMBMASTER.COMPOUNDKEEP_STATUS , 0 ) as compoundkeep_status "
				ls_return		+= "FROM MBMEMBMASTER, MBUCFAPPLTYPE "
				ls_return		+= "where mbmembmaster.coop_id = '"+ is_coopcontrol +"' "
				ls_return		+= "and mbmembmaster.coop_id = mbucfappltype.coop_id(+) "
				ls_return		+= "and ( MBMEMBMASTER.APPLTYPE_CODE = MBUCFAPPLTYPE.APPLTYPE_CODE (+)) "
				ls_return		+= "and ( ( mbmembmaster.member_status = 1 ) "
				ls_return		+= "AND ( mbmembmaster.pausekeep_flag = 0 ) "
				ls_return		+= "AND ( mbmembmaster.firstfee_status = 0 ) ) "
		end choose
		astr_extfn_keep.function_return	= ls_return
		
	case 'of_kp_proc_getsql_share'
		// ดึง sql หุ้นฝากต่อเดือน
		
		choose case is_coopcontrol
			case "E017001"
				ls_return		= "select ms.coop_id , ms.member_no , ms.member_ref , ms.membtype_code , ms.sharetype_code , ms.sharestk_amt , "
				ls_return		+= "ms.last_period , ms.periodshare_amt , ms.sharemaster_status , ms.unitshare_value , "
				ls_return		+= "ms.sharebegin_amt , ms.rkeep_sharevalue , ms.compound_payment , ms.compound_paystatus , "
				ls_return		+= "ms.compound_status , ms.payment_status , ms.pausekeep_flag "
				ls_return		+= "from ( "
				ls_return		+= "	select trim( shsharemaster.coop_id ) as coop_id , trim( mbmembmaster.member_no ) as member_no, '' as member_ref , "
				ls_return		+= "	trim( mbmembmaster.membtype_code ) as membtype_code, trim( shsharemaster.sharetype_code ) as sharetype_code, nvl( shsharemaster.sharestk_amt , 0 ) as sharestk_amt , "
				ls_return		+= "	nvl( shsharemaster.last_period , 0 ) as last_period, nvl( shsharemaster.periodshare_amt , 0 ) as periodshare_amt , nvl( shsharemaster.sharemaster_status, 0 ) as sharemaster_status , nvl( shsharetype.unitshare_value , 0 ) as unitshare_value , "
				ls_return		+= "	nvl( shsharemaster.sharebegin_amt , 0 ) as sharebegin_amt , nvl( shsharemaster.rkeep_sharevalue , 0 ) as rkeep_sharevalue , nvl( shsharemaster.compound_payment , 0 ) as compound_payment , nvl( shsharemaster.compound_paystatus , 0 ) as compound_paystatus , "
				ls_return		+= "	nvl( shsharemaster.compound_status , 0 ) as compound_status , nvl( shsharemaster.payment_status , 0 ) as payment_status , nvl( mbmembmaster.pausekeep_flag , 0 ) as pausekeep_flag "
				ls_return		+= "	from mbmembmaster, shsharemaster, shsharetype "
				ls_return		+= "	where mbmembmaster.coop_id = shsharemaster.coop_id "
				ls_return		+= "	and mbmembmaster.member_no = shsharemaster.member_no "
				ls_return		+= "	and shsharemaster.coop_id = shsharetype.coop_id "
				ls_return		+= "	and shsharemaster.sharetype_code = shsharetype.sharetype_code "
				ls_return		+= "	and ( mbmembmaster.member_status = 1 "
				ls_return		+= "	and mbmembmaster.pausekeep_flag = 0  "
				ls_return		+= "	and mbmembmaster.memref_flag = 0 ) "
				ls_return		+= "	and shsharemaster.coop_id = '"+ is_coopcontrol +"'  "
				ls_return		+= "	union "
				ls_return		+= "	select trim( shsharemaster.coop_id ) as coop_id , trim( mbmembmaster.member_ref ) as member_no, trim( mbmembmaster.member_no ) as member_ref , "
				ls_return		+= "	trim( mbmembmaster.membtype_code ) as membtype_code, trim( shsharemaster.sharetype_code ) as sharetype_code, nvl( shsharemaster.sharestk_amt , 0 ) as sharestk_amt , "
				ls_return		+= "	nvl( shsharemaster.last_period , 0 ) as last_period, nvl( shsharemaster.periodshare_amt , 0 ) as periodshare_amt , nvl( shsharemaster.sharemaster_status, 0 ) as sharemaster_status , nvl( shsharetype.unitshare_value , 0 ) as unitshare_value , "
				ls_return		+= "	nvl( shsharemaster.sharebegin_amt , 0 ) as sharebegin_amt , nvl( shsharemaster.rkeep_sharevalue , 0 ) as rkeep_sharevalue , nvl( shsharemaster.compound_payment , 0 ) as compound_payment , nvl( shsharemaster.compound_paystatus , 0 ) as compound_paystatus , "
				ls_return		+= "	nvl( shsharemaster.compound_status , 0 ) as compound_status , nvl( shsharemaster.payment_status , 0 ) as payment_status , nvl( mbmembmaster.pausekeep_flag , 0 ) as pausekeep_flag "
				ls_return		+= "	from mbmembmaster, shsharemaster, shsharetype "
				ls_return		+= "	where mbmembmaster.coop_id = shsharemaster.coop_id "
				ls_return		+= "	and mbmembmaster.member_no = shsharemaster.member_no "
				ls_return		+= "	and shsharemaster.coop_id = shsharetype.coop_id "
				ls_return		+= "	and shsharemaster.sharetype_code = shsharetype.sharetype_code "
				ls_return		+= "	and ( mbmembmaster.member_status = 1 "
				ls_return		+= "	and mbmembmaster.pausekeep_flag = 0  "
				ls_return		+= "	and mbmembmaster.memref_flag = 1 ) "
				ls_return		+= "	and shsharemaster.coop_id = '"+ is_coopcontrol +"'  "
				ls_return		+= ") ms , mbmembmaster "
				ls_return		+= "where ms.coop_id = mbmembmaster.coop_id "
				ls_return		+= "and ms.member_no = mbmembmaster.member_no "
				ls_return		+= "and ( mbmembmaster.coop_id = '" + is_coopcontrol + "' ) "
			case else
		
				ls_return		= "SELECT trim( shsharemaster.coop_id ) as coop_id , trim( MBMEMBMASTER.MEMBER_NO ) as member_no, "
				ls_return		+= "( case mbmembmaster.memref_flag when 1 then mbmembmaster.coop_id else '' end ) as refmemcoop_id , "
				ls_return		+= "( case mbmembmaster.memref_flag when 1 then mbmembmaster.member_ref else '' end ) as member_ref , "
				ls_return		+= "trim( MBMEMBMASTER.MEMBTYPE_CODE ) as membtype_code, trim( SHSHAREMASTER.SHARETYPE_CODE ) as sharetype_code, nvl( SHSHAREMASTER.SHARESTK_AMT , 0 ) as sharestk_amt , "
				ls_return		+= "nvl( SHSHAREMASTER.LAST_PERIOD , 0 ) as last_period, nvl( SHSHAREMASTER.PERIODSHARE_AMT , 0 ) as periodshare_amt , nvl( SHSHAREMASTER.SHAREMASTER_STATUS, 0 ) as sharemaster_status , nvl( SHSHARETYPE.UNITSHARE_VALUE , 0 ) as unitshare_value ,   "
				ls_return		+= "nvl( SHSHAREMASTER.SHAREBEGIN_AMT , 0 ) as sharebegin_amt , nvl( SHSHAREMASTER.RKEEP_SHAREVALUE , 0 ) as rkeep_sharevalue , nvl( SHSHAREMASTER.COMPOUND_PAYMENT , 0 ) as compound_payment , nvl( SHSHAREMASTER.COMPOUND_PAYSTATUS , 0 ) as compound_paystatus ,   "
				ls_return		+= "nvl( SHSHAREMASTER.COMPOUND_STATUS , 0 ) as compound_status , nvl( SHSHAREMASTER.PAYMENT_STATUS , 0 ) as payment_status , nvl( MBMEMBMASTER.PAUSEKEEP_FLAG , 0 ) as pausekeep_flag "
				ls_return		+= "FROM MBMEMBMASTER, SHSHAREMASTER, SHSHARETYPE  "
				ls_return		+= "WHERE ( MBMEMBMASTER.MEMBER_NO = SHSHAREMASTER.MEMBER_NO )   "
				ls_return		+= "and ( SHSHAREMASTER.SHARETYPE_CODE = SHSHARETYPE.SHARETYPE_CODE )   "
				ls_return		+= "and shsharemaster.coop_id = mbmembmaster.coop_id "
				ls_return		+= "and shsharemaster.coop_id = shsharetype.coop_id "
				ls_return		+= "and ( mbmembmaster.member_status = 1 ) "
				ls_return		+= "and mbmembmaster.pausekeep_flag = 0    "
				ls_return		+= "and shsharemaster.coop_id = '"+ is_coopcontrol +"' "
		
		end choose
		
		if is_coopcontrol = '010001' then ls_return += " and shsharemaster.payment_status = 1 "
		
		astr_extfn_keep.function_return	= ls_return
	case 'of_kp_proc_getsql_sharefromcontmast'
		//ดึง sql หักหุ้นเพ่ิมจากการกู้ แบบรายเดือน
		ls_return		= "SELECT trim( shsharemaster.coop_id ) as coop_id , trim( MBMEMBMASTER.MEMBER_NO ) as member_no, trim(LNCONTMASTER.loancontract_no) as loancontract_no, nvl(LNCONTMASTER.KEEPSHAREPERIOD_AMT,0) as KEEPSHAREPERIOD_AMT,  "
		ls_return		+= "( case mbmembmaster.memref_flag when 1 then mbmembmaster.coop_id else '' end ) as refmemcoop_id , "
		ls_return		+= "( case mbmembmaster.memref_flag when 1 then mbmembmaster.member_ref else '' end ) as member_ref , "
		ls_return		+= "trim( MBMEMBMASTER.MEMBTYPE_CODE ) as membtype_code, trim( SHSHAREMASTER.SHARETYPE_CODE ) as sharetype_code, nvl( SHSHAREMASTER.SHARESTK_AMT , 0 ) as sharestk_amt , "
		ls_return		+= "nvl( SHSHAREMASTER.LAST_PERIOD , 0 ) as last_period, nvl( SHSHAREMASTER.PERIODSHARE_AMT , 0 ) as periodshare_amt , nvl( SHSHAREMASTER.SHAREMASTER_STATUS, 0 ) as sharemaster_status , nvl( SHSHARETYPE.UNITSHARE_VALUE , 0 ) as unitshare_value ,   "
		ls_return		+= "nvl( SHSHAREMASTER.SHAREBEGIN_AMT , 0 ) as sharebegin_amt , nvl( SHSHAREMASTER.RKEEP_SHAREVALUE , 0 ) as rkeep_sharevalue , nvl( SHSHAREMASTER.COMPOUND_PAYMENT , 0 ) as compound_payment , nvl( SHSHAREMASTER.COMPOUND_PAYSTATUS , 0 ) as compound_paystatus ,   "
		ls_return		+= "nvl( SHSHAREMASTER.COMPOUND_STATUS , 0 ) as compound_status , nvl( SHSHAREMASTER.PAYMENT_STATUS , 0 ) as payment_status , nvl( MBMEMBMASTER.PAUSEKEEP_FLAG , 0 ) as pausekeep_flag "
		ls_return		+= "FROM MBMEMBMASTER, SHSHAREMASTER, SHSHARETYPE, LNCONTMASTER  "
		ls_return		+= "WHERE ( MBMEMBMASTER.MEMBER_NO = SHSHAREMASTER.MEMBER_NO ) AND MBMEMBMASTER.MEMBER_NO = LNCONTMASTER.MEMBER_NO  "
		ls_return		+= "and ( SHSHAREMASTER.SHARETYPE_CODE = SHSHARETYPE.SHARETYPE_CODE )   "
		ls_return		+= "and shsharemaster.coop_id = mbmembmaster.coop_id "
		ls_return		+= "and shsharemaster.coop_id = shsharetype.coop_id "
		ls_return		+= "and ( mbmembmaster.member_status = 1  AND 	LNCONTMASTER.PRINCIPAL_BALANCE > 0 AND LNCONTMASTER.KEEPSHAREPERIOD_AMT > 0 ) "
		ls_return		+= "and mbmembmaster.pausekeep_flag = 0    "
		ls_return		+= "and mbmembmaster.coop_id = '"+ is_coopcontrol +"' "
		astr_extfn_keep.function_return	= ls_return
	case 'of_kp_proc_getsql_loan'
		// ดึง sql ชำระหนี้ต่อเดือน
		// is_coopid

		choose case is_coopcontrol
			case "E017001"
				ls_return		= "select ml.coop_id , ml.loancontract_no , ml.member_no , ml.member_ref , "
				ls_return		+= "ml.accum_interest , ml.loantype_code , ml.memcoop_id , ml.startkeep_period , ml.period_payamt , "
				ls_return		+= "ml.loangroup_code , ml.interest_method , ml.loanpayment_type , ml.period_payment , ml.period_payment_max , "
				ls_return		+= "ml.principal_balance , ml.last_periodpay , ml.lastcalint_date , ml.principal_arrear , "
				ls_return		+= "ml.interest_arrear , ml.contract_status , ml.startcont_date , ml.payment_status , "
				ls_return		+= "ml.trnfrom_memno , ml.lastprocess_date , ml.intmonth_arrear , ml.intyear_arrear , "
				ls_return		+= "ml.loanapprove_amt , ml.int_continttype , ml.int_contintrate , ml.int_continttabcode , "
				ls_return		+= "ml.int_contintincrease , ml.int_intsteptype , ml.compound_payment , ml.compound_paystatus , "
				ls_return		+= "ml.compound_paytype , ml.compound_status , ml.transfer_status , ml.contlaw_status , "
				ls_return		+= "ml.insurecoll_flag , ml.period_payintarr , ml.lastpayment_date , "
				ls_return		+= "ml.trnfrom_intbal , ml.kpreport_seqno , ml.countpay_flag , ml.contract_type "
				ls_return		+= "from ( "
				ls_return		+= "	select trim( lncontmaster.coop_id ) as coop_id , nvl( lncontmaster.loancontract_no , '' ) as loancontract_no , trim( mbmembmaster.member_no ) as member_no , '' as member_ref , "
				ls_return		+= "	nvl( mbmembmaster.accum_interest , null ) as accum_interest , trim( lnloantype.loantype_code ) as loantype_code , "
				ls_return		+= "	trim( lncontmaster.memcoop_id ) as memcoop_id , trim( lncontmaster.startkeep_period ) as startkeep_period , nvl( lncontmaster.period_payamt , 0 ) as period_payamt , "
				ls_return		+= "	trim( lnloantype.loangroup_code ) as loangroup_code , nvl( lnloantype.interest_method , null ) as interest_method , nvl( lncontmaster.loanpayment_type , null ) as loanpayment_type , nvl( lncontmaster.period_payment , null ) as period_payment , nvl( lncontmaster.period_payment_max , null ) as period_payment_max , "
				ls_return		+= "	nvl( lncontmaster.principal_balance , null ) as principal_balance , nvl( lncontmaster.last_periodpay , null ) as last_periodpay , nvl( lncontmaster.lastcalint_date , null ) as lastcalint_date , nvl( lncontmaster.principal_arrear , null ) as principal_arrear , "
				ls_return		+= "	nvl( lncontmaster.interest_arrear , null ) as interest_arrear , nvl( lncontmaster.contract_status , null ) as contract_status , nvl( lncontmaster.startcont_date , null ) as startcont_date , nvl( lncontmaster.payment_status , null ) as payment_status , "
				ls_return		+= "	trim( lncontmaster.trnfrom_memno ) as trnfrom_memno , nvl( lncontmaster.lastprocess_date , null ) as lastprocess_date , nvl( lncontmaster.intmonth_arrear , null ) as intmonth_arrear , nvl( lncontmaster.intyear_arrear , null ) as intyear_arrear , "
				ls_return		+= "	nvl( lncontmaster.loanapprove_amt , null ) as loanapprove_amt , nvl( lncontmaster.int_continttype , null ) as int_continttype , nvl( lncontmaster.int_contintrate , null ) as int_contintrate , trim( lncontmaster.int_continttabcode ) as int_continttabcode , "
				ls_return		+= "	nvl( lncontmaster.int_contintincrease , null ) as int_contintincrease , nvl( lncontmaster.int_intsteptype , null ) as int_intsteptype , nvl( lncontmaster.compound_payment , null ) as compound_payment , nvl( lncontmaster.compound_paystatus , null ) as compound_paystatus , "
				ls_return		+= "	nvl( lncontmaster.compound_paytype , null ) as compound_paytype , nvl( lncontmaster.compound_status , null ) as compound_status , nvl( lncontmaster.transfer_status , null ) as transfer_status , nvl( lncontmaster.contlaw_status , null ) as contlaw_status , "
				ls_return		+= "	nvl( lncontmaster.insurecoll_flag , null ) as insurecoll_flag , nvl( lncontmaster.period_payintarr , null ) as period_payintarr , nvl( lncontmaster.lastpayment_date , null ) as lastpayment_date , "
				ls_return		+= "	nvl( lncontmaster.trnfrom_intbal , null ) as trnfrom_intbal , nvl( lnloantype.kpreport_seqno , null ) as kpreport_seqno , nvl( lncontmaster.countpay_flag , null ) as countpay_flag , nvl( lncontmaster.contract_type , null ) as contract_type "
				ls_return		+= "	from lncontmaster , mbmembmaster , lnloantype "
				ls_return		+= "	where lncontmaster.memcoop_id = mbmembmaster.coop_id "
				ls_return		+= "	and lncontmaster.member_no = mbmembmaster.member_no "
				ls_return		+= "	and lncontmaster.coop_id = lnloantype.coop_id "
				ls_return		+= "	and lncontmaster.loantype_code = lnloantype.loantype_code "
				ls_return		+= "	and ( mbmembmaster.member_status = 1 "
				ls_return		+= "	and mbmembmaster.pausekeep_flag = 0 "
				ls_return		+= "	and lncontmaster.contract_status > 0 "
				ls_return		+= "	and ( lncontmaster.principal_balance > 0 or lncontmaster.interest_arrear > 0 ) "
				ls_return		+= "	and lncontmaster.memcoop_id = '"+ is_coopcontrol +"' "
				ls_return		+= "	and mbmembmaster.memref_flag = 0 ) "
				ls_return		+= "	union "
				ls_return		+= "	select trim( lncontmaster.coop_id ) as coop_id , nvl( lncontmaster.loancontract_no , '' ) as loancontract_no , trim( mbmembmaster.member_ref ) as member_no , trim( mbmembmaster.member_no ) as member_ref , "
				ls_return		+= "	nvl( mbmembmaster.accum_interest , null ) as accum_interest , trim( lnloantype.loantype_code ) as loantype_code , "
				ls_return		+= "	trim( lncontmaster.memcoop_id ) as memcoop_id , trim( lncontmaster.startkeep_period ) as startkeep_period , nvl( lncontmaster.period_payamt , 0 ) as period_payamt , "
				ls_return		+= "	trim( lnloantype.loangroup_code ) as loangroup_code , nvl( lnloantype.interest_method , null ) as interest_method , nvl( lncontmaster.loanpayment_type , null ) as loanpayment_type , nvl( lncontmaster.period_payment , null ) as period_payment , nvl( lncontmaster.period_payment_max , null ) as period_payment_max , "
				ls_return		+= "	nvl( lncontmaster.principal_balance , null ) as principal_balance , nvl( lncontmaster.last_periodpay , null ) as last_periodpay , nvl( lncontmaster.lastcalint_date , null ) as lastcalint_date , nvl( lncontmaster.principal_arrear , null ) as principal_arrear , "
				ls_return		+= "	nvl( lncontmaster.interest_arrear , null ) as interest_arrear , nvl( lncontmaster.contract_status , null ) as contract_status , nvl( lncontmaster.startcont_date , null ) as startcont_date , nvl( lncontmaster.payment_status , null ) as payment_status , "
				ls_return		+= "	trim( lncontmaster.trnfrom_memno ) as trnfrom_memno , nvl( lncontmaster.lastprocess_date , null ) as lastprocess_date , nvl( lncontmaster.intmonth_arrear , null ) as intmonth_arrear , nvl( lncontmaster.intyear_arrear , null ) as intyear_arrear , "
				ls_return		+= "	nvl( lncontmaster.loanapprove_amt , null ) as loanapprove_amt , nvl( lncontmaster.int_continttype , null ) as int_continttype , nvl( lncontmaster.int_contintrate , null ) as int_contintrate , trim( lncontmaster.int_continttabcode ) as int_continttabcode , "
				ls_return		+= "	nvl( lncontmaster.int_contintincrease , null ) as int_contintincrease , nvl( lncontmaster.int_intsteptype , null ) as int_intsteptype , nvl( lncontmaster.compound_payment , null ) as compound_payment , nvl( lncontmaster.compound_paystatus , null ) as compound_paystatus , "
				ls_return		+= "	nvl( lncontmaster.compound_paytype , null ) as compound_paytype , nvl( lncontmaster.compound_status , null ) as compound_status , nvl( lncontmaster.transfer_status , null ) as transfer_status , nvl( lncontmaster.contlaw_status , null ) as contlaw_status , "
				ls_return		+= "	nvl( lncontmaster.insurecoll_flag , null ) as insurecoll_flag , nvl( lncontmaster.period_payintarr , null ) as period_payintarr , nvl( lncontmaster.lastpayment_date , null ) as lastpayment_date , "
				ls_return		+= "	nvl( lncontmaster.trnfrom_intbal , null ) as trnfrom_intbal , nvl( lnloantype.kpreport_seqno , null ) as kpreport_seqno , nvl( lncontmaster.countpay_flag , null ) as countpay_flag , nvl( lncontmaster.contract_type , null ) as contract_type "
				ls_return		+= "	from lncontmaster , mbmembmaster , lnloantype "
				ls_return		+= "	where lncontmaster.memcoop_id = mbmembmaster.coop_id "
				ls_return		+= "	and lncontmaster.member_no = mbmembmaster.member_no "
				ls_return		+= "	and lncontmaster.coop_id = lnloantype.coop_id "
				ls_return		+= "	and lncontmaster.loantype_code = lnloantype.loantype_code "
				ls_return		+= "	and ( mbmembmaster.member_status = 1 "
				ls_return		+= "	and mbmembmaster.pausekeep_flag = 0 "
				ls_return		+= "	and lncontmaster.contract_status > 0 "
				ls_return		+= "	and ( lncontmaster.principal_balance > 0 or lncontmaster.interest_arrear > 0 ) "
				ls_return		+= "	and lncontmaster.memcoop_id = '"+ is_coopcontrol +"'  "
				ls_return		+= "	and mbmembmaster.memref_flag = 1 ) "
				ls_return		+= "	) ml , mbmembmaster "
				ls_return		+= "where ml.coop_id = mbmembmaster.coop_id "
				ls_return		+= "and ml.member_no = mbmembmaster.member_no "
				ls_return		+= "and ( mbmembmaster.coop_id = '" + is_coopcontrol + "' ) "
				
			case "010001"

				ls_return		= "SELECT trim( lncontmaster.coop_id ) as coop_id , nvl( LNCONTMASTER.LOANCONTRACT_NO , '' ) as loancontract_no , trim( MBMEMBMASTER.MEMBER_NO ) as member_no , nvl( MBMEMBMASTER.ACCUM_INTEREST , null ) as accum_interest , trim( LNLOANTYPE.LOANTYPE_CODE ) as loantype_code , "
				ls_return		+= " trim( lncontmaster.coop_id ) as refmemcoop_id , ( case when lncontmaster.transfer_status = 1 then lncontmaster.trnfrom_memno else lncontmaster.member_no end ) as member_ref , "
				ls_return		+= " trim( lncontmaster.memcoop_id ) as memcoop_id , trim( lncontmaster.startkeep_period ) as startkeep_period , nvl( lncontmaster.period_payamt , 0 ) as period_payamt , "
				ls_return		+= " trim( LNLOANTYPE.LOANGROUP_CODE ) as loangroup_code , nvl( LNLOANTYPE.INTEREST_METHOD , null ) as interest_method , nvl( LNCONTMASTER.LOANPAYMENT_TYPE , null ) as loanpayment_type , nvl( LNCONTMASTER.PERIOD_PAYMENT , null ) as period_payment , nvl( lncontmaster.period_payment_max , null ) as period_payment_max , "
				ls_return		+= " nvl( LNCONTMASTER.PRINCIPAL_BALANCE , null ) as principal_balance , nvl( LNCONTMASTER.LAST_PERIODPAY , null ) as last_periodpay , nvl( LNCONTMASTER.LASTCALINT_DATE , null ) as lastcalint_date , nvl( LNCONTMASTER.PRINCIPAL_ARREAR , null ) as principal_arrear , "
				ls_return		+= " nvl( LNCONTMASTER.INTEREST_ARREAR , null ) as interest_arrear , nvl( LNCONTMASTER.CONTRACT_STATUS , null ) as contract_status , nvl( LNCONTMASTER.STARTCONT_DATE , null ) as startcont_date , nvl( LNCONTMASTER.PAYMENT_STATUS , null ) as payment_status , "
				ls_return		+= " trim( LNCONTMASTER.TRNFROM_MEMNO ) as trnfrom_memno , nvl( LNCONTMASTER.LASTPROCESS_DATE , null ) as lastprocess_date , nvl( LNCONTMASTER.INTMONTH_ARREAR , null ) as intmonth_arrear , nvl( LNCONTMASTER.INTYEAR_ARREAR , null ) as intyear_arrear , "
				ls_return		+= " nvl( LNCONTMASTER.LOANAPPROVE_AMT , null ) as loanapprove_amt , nvl( LNCONTMASTER.INT_CONTINTTYPE , null ) as int_continttype , nvl( LNCONTMASTER.INT_CONTINTRATE , null ) as int_contintrate , trim( LNCONTMASTER.INT_CONTINTTABCODE ) as int_continttabcode , "
				ls_return		+= " nvl( LNCONTMASTER.INT_CONTINTINCREASE , null ) as int_contintincrease , nvl( LNCONTMASTER.INT_INTSTEPTYPE , null ) as int_intsteptype , nvl( LNCONTMASTER.COMPOUND_PAYMENT , null ) as compound_payment , nvl( LNCONTMASTER.COMPOUND_PAYSTATUS , null ) as compound_paystatus , "
				ls_return		+= " nvl( LNCONTMASTER.COMPOUND_PAYTYPE , null ) as compound_paytype , nvl( LNCONTMASTER.COMPOUND_STATUS , null ) as compound_status , nvl( LNCONTMASTER.TRANSFER_STATUS , null ) as transfer_status , nvl( LNCONTMASTER.CONTLAW_STATUS , null ) as contlaw_status , "
				ls_return		+= " nvl( LNCONTMASTER.INSURECOLL_FLAG , null ) as insurecoll_flag , nvl( LNCONTMASTER.PERIOD_PAYINTARR , null ) as period_payintarr , nvl( LNCONTMASTER.LASTPAYMENT_DATE , null ) as lastpayment_date , "
				ls_return		+= " nvl( LNCONTMASTER.TRNFROM_INTBAL , null ) as trnfrom_intbal , nvl( LNLOANTYPE.KPREPORT_SEQNO , null ) as kpreport_seqno , nvl( LNCONTMASTER.COUNTPAY_FLAG , null ) as countpay_flag , nvl( lncontmaster.contract_type , null ) as contract_type  "
				ls_return		+= " FROM LNCONTMASTER , MBMEMBMASTER , LNLOANTYPE "
				ls_return		+= " WHERE lncontmaster.memcoop_id = mbmembmaster.coop_id "
				ls_return		+= " and lncontmaster.member_no = mbmembmaster.member_no "
				ls_return		+= " and lncontmaster.coop_id = lnloantype.coop_id "
				ls_return		+= " and lncontmaster.loantype_code = lnloantype.loantype_code "
				ls_return		+= " and mbmembmaster.member_status = 1 "
				ls_return		+= " and mbmembmaster.pausekeep_flag = 0 "
				ls_return		+= " and lncontmaster.contract_status > 0 "
				ls_return		+= " and lncontmaster.payment_status <> -13 "
				ls_return		+= " and ( lncontmaster.principal_balance > 0 or lncontmaster.interest_arrear > 0 ) "
				ls_return		+= " and lncontmaster.memcoop_id = '"+ is_coopcontrol +"' "
			case else
				ls_return		= "SELECT trim( lncontmaster.coop_id ) as coop_id , nvl( LNCONTMASTER.LOANCONTRACT_NO , '' ) as loancontract_no , trim( MBMEMBMASTER.MEMBER_NO ) as member_no , nvl( MBMEMBMASTER.ACCUM_INTEREST , null ) as accum_interest , trim( LNLOANTYPE.LOANTYPE_CODE ) as loantype_code , "
				ls_return		+= " trim( lncontmaster.coop_id ) as refmemcoop_id , ( case when lncontmaster.transfer_status = 1 then lncontmaster.trnfrom_memno else lncontmaster.member_no end ) as member_ref , "
				ls_return		+= " trim( lncontmaster.memcoop_id ) as memcoop_id , trim( lncontmaster.startkeep_period ) as startkeep_period , nvl( lncontmaster.period_payamt , 0 ) as period_payamt , "
				ls_return		+= " trim( LNLOANTYPE.LOANGROUP_CODE ) as loangroup_code , nvl( LNLOANTYPE.INTEREST_METHOD , null ) as interest_method , nvl( LNCONTMASTER.LOANPAYMENT_TYPE , null ) as loanpayment_type , nvl( LNCONTMASTER.PERIOD_PAYMENT , null ) as period_payment , nvl( lncontmaster.period_payment_max , null ) as period_payment_max , "
				ls_return		+= " nvl( LNCONTMASTER.PRINCIPAL_BALANCE , null ) as principal_balance , nvl( LNCONTMASTER.LAST_PERIODPAY , null ) as last_periodpay , nvl( LNCONTMASTER.LASTCALINT_DATE , null ) as lastcalint_date , nvl( LNCONTMASTER.PRINCIPAL_ARREAR , null ) as principal_arrear , "
				ls_return		+= " nvl( LNCONTMASTER.INTEREST_ARREAR , null ) as interest_arrear , nvl( LNCONTMASTER.INTEREST_RETURN , null ) as interest_return, nvl( LNCONTMASTER.CONTRACT_STATUS , null ) as contract_status , nvl( LNCONTMASTER.STARTCONT_DATE , null ) as startcont_date , nvl( LNCONTMASTER.PAYMENT_STATUS , null ) as payment_status , "
				ls_return		+= " trim( LNCONTMASTER.TRNFROM_MEMNO ) as trnfrom_memno , nvl( LNCONTMASTER.LASTPROCESS_DATE , null ) as lastprocess_date , nvl( LNCONTMASTER.INTMONTH_ARREAR , null ) as intmonth_arrear , nvl( LNCONTMASTER.INTYEAR_ARREAR , null ) as intyear_arrear , "
				ls_return		+= " nvl( LNCONTMASTER.LOANAPPROVE_AMT , null ) as loanapprove_amt , nvl( LNCONTMASTER.INT_CONTINTTYPE , null ) as int_continttype , nvl( LNCONTMASTER.INT_CONTINTRATE , null ) as int_contintrate , trim( LNCONTMASTER.INT_CONTINTTABCODE ) as int_continttabcode , "
				ls_return		+= " nvl( LNCONTMASTER.INT_CONTINTINCREASE , null ) as int_contintincrease , nvl( LNCONTMASTER.INT_INTSTEPTYPE , null ) as int_intsteptype , nvl( LNCONTMASTER.COMPOUND_PAYMENT , null ) as compound_payment , nvl( LNCONTMASTER.COMPOUND_PAYSTATUS , null ) as compound_paystatus , "
				ls_return		+= " nvl( LNCONTMASTER.COMPOUND_PAYTYPE , null ) as compound_paytype , nvl( LNCONTMASTER.COMPOUND_STATUS , null ) as compound_status , nvl( LNCONTMASTER.TRANSFER_STATUS , null ) as transfer_status , nvl( LNCONTMASTER.CONTLAW_STATUS , null ) as contlaw_status , "
				ls_return		+= " nvl( LNCONTMASTER.INSURECOLL_FLAG , null ) as insurecoll_flag , nvl( LNCONTMASTER.PERIOD_PAYINTARR , null ) as period_payintarr , nvl( LNCONTMASTER.LASTPAYMENT_DATE , null ) as lastpayment_date , "
				ls_return		+= " nvl( LNCONTMASTER.TRNFROM_INTBAL , null ) as trnfrom_intbal , nvl( LNLOANTYPE.KPREPORT_SEQNO , null ) as kpreport_seqno , nvl( LNCONTMASTER.COUNTPAY_FLAG , null ) as countpay_flag , nvl( lncontmaster.contract_type , null ) as contract_type , "
				ls_return		+= " nvl( LNCONTMASTER.ATM_FLAG , null ) as atm_flag , nvl( LNCONTMASTER.PERIOD_INSTALLMENT , null ) as period_installment "
				ls_return		+= " FROM LNCONTMASTER , MBMEMBMASTER , LNLOANTYPE "
				ls_return		+= " WHERE lncontmaster.memcoop_id = mbmembmaster.coop_id "
				ls_return		+= " and lncontmaster.member_no = mbmembmaster.member_no "
				ls_return		+= " and lncontmaster.coop_id = lnloantype.coop_id "
				ls_return		+= " and lncontmaster.loantype_code = lnloantype.loantype_code "
				ls_return		+= " and mbmembmaster.member_status = 1 "
				ls_return		+= " and mbmembmaster.pausekeep_flag = 0 "
				ls_return		+= " and lncontmaster.contract_status > 0 "
				ls_return		+= " and ( lncontmaster.principal_balance > 0 or lncontmaster.interest_arrear > 0 ) "
				ls_return		+= " and mbmembmaster.coop_id = '"+ is_coopcontrol +"' "
		end choose
		
		astr_extfn_keep.function_return	= ls_return
		
	case 'of_kp_proc_getsql_dept'
		// ดึง sql เงินฝากต่อเดือน
		// is_coopid
		choose case is_coopcontrol
			case "E017001"
				
				ls_return		= "select md.coop_id , md.deptaccount_no , md.depttype_code , "
				ls_return		+= "md.memcoop_id , md.member_no , md.member_ref , "
				ls_return		+= "md.deptgroup_code , md.deptmonth_amt "
				ls_return		+= "from ( "
				ls_return		+= "	select trim( dpdeptmaster.coop_id ) as coop_id , trim( dpdeptmaster.deptaccount_no ) as deptaccount_no , trim( dpdeptmaster.depttype_code ) as depttype_code , "
				ls_return		+= "	trim( dpdeptmaster.memcoop_id ) as memcoop_id , trim( dpdeptmaster.member_no ) as member_no , '' as member_ref , "
				ls_return		+= "	trim( dpdepttype.deptgroup_code ) as deptgroup_code , nvl( dpdeptmaster.deptmonth_amt , 0 ) as deptmonth_amt "
				ls_return		+= "	from dpdeptmaster , dpdepttype , mbmembmaster "
				ls_return		+= "	where dpdeptmaster.memcoop_id = mbmembmaster.coop_id "
				ls_return		+= "	and dpdeptmaster.member_no = mbmembmaster.member_no "
				ls_return		+= "	and dpdeptmaster.coop_id = dpdepttype.coop_id "
				ls_return		+= "	and dpdeptmaster.depttype_code = dpdepttype.depttype_code "
				ls_return		+= "	and ( dpdeptmaster.deptmonth_status = 1 "
				ls_return		+= "	and dpdeptmaster.deptmonth_amt > 0 "
				ls_return		+= "	and dpdeptmaster.deptclose_status = 0 "
				ls_return		+= "	and mbmembmaster.memref_flag = 0 ) "
				ls_return		+= "	and mbmembmaster.coop_id = '"+ is_coopcontrol +"' "
				ls_return		+= "	union "
				ls_return		+= "	select trim( dpdeptmaster.coop_id ) as coop_id , trim( dpdeptmaster.deptaccount_no ) as deptaccount_no , trim( dpdeptmaster.depttype_code ) as depttype_code , "
				ls_return		+= "	trim( dpdeptmaster.memcoop_id ) as memcoop_id , trim( mbmembmaster.member_ref ) as member_no , trim( mbmembmaster.member_no ) as member_ref , "
				ls_return		+= "	trim( dpdepttype.deptgroup_code ) as deptgroup_code , nvl( dpdeptmaster.deptmonth_amt , 0 ) as deptmonth_amt "
				ls_return		+= "	from dpdeptmaster , dpdepttype , mbmembmaster "
				ls_return		+= "	where dpdeptmaster.memcoop_id = mbmembmaster.coop_id "
				ls_return		+= "	and dpdeptmaster.member_no = mbmembmaster.member_no "
				ls_return		+= "	and dpdeptmaster.coop_id = dpdepttype.coop_id "
				ls_return		+= "	and dpdeptmaster.depttype_code = dpdepttype.depttype_code "
				ls_return		+= "	and ( dpdeptmaster.deptmonth_status = 1 "
				ls_return		+= "	and dpdeptmaster.deptmonth_amt > 0 "
				ls_return		+= "	and dpdeptmaster.deptclose_status = 0 "
				ls_return		+= "	and mbmembmaster.memref_flag = 1 ) "
				ls_return		+= "	and mbmembmaster.coop_id = '"+ is_coopcontrol +"' "
				ls_return		+= ") md , mbmembmaster "
				ls_return		+= "where md.coop_id = mbmembmaster.coop_id "
				ls_return		+= "and md.member_no = mbmembmaster.member_no "
				ls_return		+= "and ( mbmembmaster.coop_id = '" + is_coopcontrol + "' ) "
				
			case "010001"
				ls_return		= "select trim( dpdeptmaster.coop_id ) as coop_id , trim( dpdeptmaster.deptaccount_no ) as deptaccount_no , trim( dpdeptmaster.depttype_code ) as depttype_code , "
				ls_return		+= " trim( dpdeptmaster.memcoop_id ) as memcoop_id , trim( dpdeptmaster.member_no ) as member_no , "
				ls_return		+= "( case mbmembmaster.memref_flag when 1 then mbmembmaster.coop_id else '' end ) as refmemcoop_id , "
				ls_return		+= "( case mbmembmaster.memref_flag when 1 then mbmembmaster.member_ref else '' end ) as member_ref , "
				ls_return		+= " trim( dpdepttype.deptgroup_code ) as deptgroup_code , nvl( dpdeptmaster.deptmonth_amt , 0 ) as deptmonth_amt "
				ls_return		+= " from dpdeptmaster , dpdepttype , mbmembmaster "
				ls_return		+= " where dpdeptmaster.memcoop_id = mbmembmaster.coop_id "
				ls_return		+= " and dpdeptmaster.member_no = mbmembmaster.member_no "
				ls_return		+= " and dpdeptmaster.coop_id = dpdepttype.coop_id "
				ls_return		+= " and dpdeptmaster.depttype_code = dpdepttype.depttype_code "
				ls_return		+= " and dpdeptmaster.deptmonth_status = 1 "
				ls_return		+= " and dpdeptmaster.deptmonth_amt > 0 "
				ls_return		+= " and dpdeptmaster.deptclose_status = 0 "
				ls_return		+= " and mbmembmaster.resign_status = 0 "
				ls_return		+= " and mbmembmaster.pausekeep_flag = 0 "
				ls_return		+= " and dpdeptmaster.memcoop_id = '"+ is_coopcontrol +"' "
				
			case else
				ls_return		= "select trim( dpdeptmaster.coop_id ) as coop_id , trim( dpdeptmaster.deptaccount_no ) as deptaccount_no , trim( dpdeptmaster.depttype_code ) as depttype_code , "
				ls_return		+= " trim( dpdeptmaster.memcoop_id ) as memcoop_id , trim( dpdeptmaster.member_no ) as member_no , "
//				ls_return		+= " '' as member_ref , "
				ls_return		+= "( case mbmembmaster.memref_flag when 1 then mbmembmaster.coop_id else '' end ) as refmemcoop_id , "
				ls_return		+= "( case mbmembmaster.memref_flag when 1 then mbmembmaster.member_ref else '' end ) as member_ref , "
				ls_return		+= " trim( dpdepttype.deptgroup_code ) as deptgroup_code , nvl( dpdeptmaster.deptmonth_amt , 0 ) as deptmonth_amt "
				ls_return		+= " from dpdeptmaster , dpdepttype , mbmembmaster "
				ls_return		+= " where dpdeptmaster.memcoop_id = mbmembmaster.coop_id "
				ls_return		+= " and dpdeptmaster.member_no = mbmembmaster.member_no "
				ls_return		+= " and dpdeptmaster.coop_id = dpdepttype.coop_id "
				ls_return		+= " and dpdeptmaster.depttype_code = dpdepttype.depttype_code "
				ls_return		+= " and dpdeptmaster.deptmonth_status = 1 "
				ls_return		+= " and dpdeptmaster.deptmonth_amt > 0 "
				ls_return		+= " and dpdeptmaster.deptclose_status = 0 "
				ls_return		+= " and mbmembmaster.pausekeep_flag = 0 "
				ls_return		+= " and dpdeptmaster.memcoop_id = '"+ is_coopcontrol +"' "
		end choose
		
		astr_extfn_keep.function_return	= ls_return
		
	case 'of_kp_proc_getsql_other'
		
//		SELECT nvl( kprcvkeepother.coop_id , null ) as coop_id , nvl( kprcvkeepother.memcoop_id , null ) as memcoop_id , nvl( MBMEMBMASTER.MEMBER_NO , null ) as member_no , 
//		nvl( kprcvkeepother.seq_no , 0 ) as seq_no , nvl( KPRCVKEEPOTHER.KEEPITEMTYPE_CODE , null ) as keepitemtype_code , nvl( KPRCVKEEPOTHER.DESCRIPTION , null ) as DESCRIPTION , 
//		nvl( KPRCVKEEPOTHER.ITEM_PAYMENT , 0 ) as item_payment , 10 as sort
//		FROM MBMEMBMASTER , KPRCVKEEPOTHER     
//		WHERE mbmembmaster.coop_id = kprcvkeepother.coop_id
//		and MBMEMBMASTER.MEMBER_NO = KPRCVKEEPOTHER.MEMBER_NO
//		and kprcvkeepother.coop_id = '001001'
		// ส่วนหัว
		ls_return		+= " select oth.coop_id , oth.memcoop_id , oth.member_no , "
		ls_return		+= " ( case mbmembmaster.memref_flag when 1 then mbmembmaster.coop_id else '' end ) as refmemcoop_id , "
		ls_return		+= " ( case mbmembmaster.memref_flag when 1 then mbmembmaster.member_ref else '' end ) as member_ref , "
		ls_return		+= " oth.seq_no , oth.loancontract_no , "
		ls_return		+= " oth.keepitemtype_code , oth.description , oth.keepothitemtype_code , "
		ls_return		+= " oth.item_payment , oth.sort "
		ls_return		+= " from ( "
		
		ls_return		+= " SELECT nvl( kprcvkeepother.coop_id , null ) as coop_id , nvl( kprcvkeepother.memcoop_id , null ) as memcoop_id , nvl( MBMEMBMASTER.MEMBER_NO , null ) as member_no , "
		ls_return		+= " nvl( kprcvkeepother.seq_no , 0 ) as seq_no , null as loancontract_no , nvl( KPRCVKEEPOTHER.KEEPITEMTYPE_CODE , null ) as keepitemtype_code , nvl( KPRCVKEEPOTHER.DESCRIPTION , null ) as DESCRIPTION , "
		ls_return		+= " nvl( KPRCVKEEPOTHER.KEEPOTHITEMTYPE_CODE , null ) as keepothitemtype_code , "
		ls_return		+= " nvl( KPRCVKEEPOTHER.ITEM_PAYMENT , 0 ) as item_payment , 10 as sort "
		ls_return		+= " FROM MBMEMBMASTER , KPRCVKEEPOTHER "
		ls_return		+= " WHERE mbmembmaster.coop_id = kprcvkeepother.coop_id "
		ls_return		+= " and MBMEMBMASTER.MEMBER_NO = KPRCVKEEPOTHER.MEMBER_NO "
		ls_return		+= " and kprcvkeepother.coop_id = '"+ is_coopid +"' "
		ls_return		+= " and ((kprcvkeepother.startkeep_period <= '"+is_recvperiod+"' AND kprcvkeepother.keepother_type = 2 ) "
		ls_return		+= " OR  (kprcvkeepother.startkeep_period = '"+is_recvperiod+"' AND kprcvkeepother.keepother_type >= 1 )) "
		// kprcvkeepother.startkeep_period 2 :> เก็บตลอดไป
		// 1 :> เก็บครั้งเดียว
		if is_coopcontrol = '001001' then
			// ฌาปนกิจรายเดือน เฉพาะ มหิดล
			ls_return		+= " union "
			ls_return		+= " select nvl( '00' || wcrecievemonth.branch_id , null ) as coop_id , nvl( '00' || wcrecievemonth.branch_id , null ) as memcoop_id , "
			ls_return		+= " nvl( wcrecievemonth.MEMBER_NO , null ) as member_no , "
			ls_return		+= " 100 as seq_no , wcrecievemonth.wfmember_no as loancontract_no , 'OTH' as keepitemtype_code , 'ค่าฌาปนกิจฯ' as DESCRIPTION , "
			ls_return		+= " 'WF' as keepotheritemtype_code , nvl( wcrecievemonth.carcass_amt , 0 ) as item_payment , 100 as sort "
			ls_return		+= " from wcrecievemonth "
			ls_return		+= " where '00' || wcrecievemonth.branch_id = '"+ is_coopid +"' "
			ls_return		+= " and wcrecievemonth.recv_period = '"+is_recvperiod+"' "
			ls_return		+= " and wcrecievemonth.carcass_amt > 0 "
			ls_return		+= " and wcrecievemonth.status_post = 8 "
			ls_return		+= " and wcrecievemonth.expense_code = 'TKP' "
			ls_return		+= " union "
			// ฌาปนกิจบำรุงรายปีสมาชิกหลัก
			ls_return		+= " select nvl( '00' || wcrecievemonth.branch_id , null ) as coop_id , nvl( '00' || wcrecievemonth.branch_id , null ) as memcoop_id , "
			ls_return		+= " nvl( wcrecievemonth.MEMBER_NO , null ) as member_no , "
			ls_return		+= " 110 as seq_no , wcrecievemonth.wfmember_no as loancontract_no , 'OTH' as keepitemtype_code , 'ค่าบำรุงฌาปนกิจฯรายปี' as DESCRIPTION , "
			ls_return		+= " 'W1' as keepotheritemtype_code , nvl( wcrecievemonth.fee_year , 0 ) as item_payment , 110 as sort "
			ls_return		+= " from wcrecievemonth,wcdeptmaster, wcdeptmaster wma "
			ls_return		+= " where '00' || wcrecievemonth.branch_id =  '"+ is_coopid +"' "
			ls_return		+= " and wcrecievemonth.recv_period = '"+is_recvperiod+"' "
			ls_return		+= " and wcdeptmaster.deptaccount_no = wcrecievemonth.wfmember_no "
			ls_return		+= " and wma.member_no = wcrecievemonth.member_no "
			ls_return		+= " and wma.wftype_code = '01' "
			ls_return		+= " and wma.deptclose_status = 0 "
			ls_return		+= " and wcdeptmaster.wftype_code = '01' "
			ls_return		+= " and wcrecievemonth.fee_year > 0 "
			ls_return		+= " and wcrecievemonth.status_post = 8 "
			ls_return		+= " and wma.expense_code = 'TKP' "
	//		// ฌาปนกิจบำรุงรายปีสมาชิกสมทบ
	//		ls_return		+= " union "
	//		ls_return		+= " select nvl( '00' || wcrecievemonth.branch_id , null ) as coop_id , nvl( '00' || wcrecievemonth.branch_id , null ) as memcoop_id , "
	//		ls_return		+= " nvl( wcrecievemonth.MEMBER_NO , null ) as member_no , "
	//		ls_return		+= " 120 as seq_no , wcrecievemonth.wfmember_no as loancontract_no , 'OTH' as keepitemtype_code , 'ค่าบำรุงฌาปนกิจฯรายปีสมทบ' as DESCRIPTION , "
	//		ls_return		+= " 'W2' as keepotheritemtype_code , nvl( wcrecievemonth.fee_year , 0 ) as item_payment , 120 as sort "
	//		ls_return		+= " from wcrecievemonth,wcdeptmaster, wcdeptmaster wma "
	//		ls_return		+= " where '00' || wcrecievemonth.branch_id =  '"+ is_coopid +"' "
	//		ls_return		+= " and wcrecievemonth.recv_period = '"+is_recvperiod+"' "
	//		ls_return		+= " and wcdeptmaster.deptaccount_no = wcrecievemonth.wfmember_no "
	//		ls_return		+= " and wma.member_no = wcrecievemonth.member_no "
	//		ls_return		+= " and wma.wftype_code = '01' "
	//		ls_return		+= " and wma.deptclose_status = 0 "
	//		ls_return		+= " and wcdeptmaster.wftype_code = '02' "
	//		ls_return		+= " and wcrecievemonth.fee_year > 0 "
	//		ls_return		+= " and wcrecievemonth.status_post = 8 "
	//		ls_return		+= " and wma.expense_code = 'TKP' "
	//		// ฌาปนกิจบำรุงรายปีสมาชิกบุตร
	//		ls_return		+= " union "
	//		ls_return		+= " select nvl( '00' || wcrecievemonth.branch_id , null ) as coop_id , nvl( '00' || wcrecievemonth.branch_id , null ) as memcoop_id , "
	//		ls_return		+= " nvl( wcrecievemonth.MEMBER_NO , null ) as member_no , "
	//		ls_return		+= " 130 as seq_no , wcrecievemonth.wfmember_no as loancontract_no , 'OTH' as keepitemtype_code , 'ค่าบำรุงฌาปนกิจฯรายปีบุตร' as DESCRIPTION , "
	//		ls_return		+= " 'W3' as keepotheritemtype_code , nvl( wcrecievemonth.fee_year , 0 ) as item_payment , 130 as sort "
	//		ls_return		+= " from wcrecievemonth,wcdeptmaster, wcdeptmaster wma "
	//		ls_return		+= " where '00' || wcrecievemonth.branch_id =  '"+ is_coopid +"' "
	//		ls_return		+= " and wcrecievemonth.recv_period = '"+is_recvperiod+"' "
	//		ls_return		+= " and wcdeptmaster.deptaccount_no = wcrecievemonth.wfmember_no "
	//		ls_return		+= " and wma.member_no = wcrecievemonth.member_no "
	//		ls_return		+= " and wma.wftype_code = '01' "
	//		ls_return		+= " and wma.deptclose_status = 0 "
	//		ls_return		+= " and wcdeptmaster.wftype_code = '03' "
	//		ls_return		+= " and wcrecievemonth.fee_year > 0 "
	//		ls_return		+= " and wcrecievemonth.status_post = 8 "
	//		ls_return		+= " and wma.expense_code = 'TKP' "
		end if
		
		if is_coopcontrol = '008001' then
			ls_return		+= " union "
			ls_return		+= " SELECT nvl( LNINSURANCEFIRE.COOP_ID , null ) as coop_id , nvl( MBMEMBMASTER.COOP_ID , null ) as memcoop_id , "
			ls_return		+= " nvl( LNINSURANCEFIRE.MEMBER_NO , null ) as member_no , "
			ls_return		+= " 100 as seq_no , nvl( LNINSURANCEFIRE.LOANCONTRACT_NO , null ) as loancontract_no , "
			ls_return		+= " nvl( LNINSURANCEFIRE.INSURETYPE_CODE , null ) as keepitemtype_code , nvl( LNINSURANCEFIRE.INSURANCE_NO , null ) as description , "
			ls_return		+= " 'I1' as keepotheritemtype_code, "
			ls_return		+= " nvl( LNINSURANCEFIRE.INSURENET_AMT , 0 ) as item_payment , 100 as sort "
			ls_return		+= " FROM LNINSURANCEFIRE,   "
			ls_return		+= " MBMEMBMASTER  "
			ls_return		+= " WHERE ( LNINSURANCEFIRE.MEMBER_NO = MBMEMBMASTER.MEMBER_NO ) and  "
			ls_return		+= " ( LNINSURANCEFIRE.COOP_ID = MBMEMBMASTER.COOP_ID ) and  "
			ls_return		+= " ( ( lninsurancefire.insurepay_status = 0 ) AND  "
			ls_return		+= " ( mbmembmaster.pausekeep_flag = 0 ) AND  "
			ls_return		+= " ( LNINSURANCEFIRE.INSURETYPE_CODE = '10' ) )    "
			ls_return		+= " union "
			ls_return		+= " SELECT nvl( LNINSURANCEFIRE.COOP_ID , null ) as coop_id , nvl( MBMEMBMASTER.COOP_ID , null ) as memcoop_id , "
			ls_return		+= " nvl( LNINSURANCEFIRE.MEMBER_NO , null ) as member_no , "
			ls_return		+= " 101 as seq_no , nvl( LNINSURANCEFIRE.LOANCONTRACT_NO , null ) as loancontract_no , "
			ls_return		+= " nvl( LNINSURANCEFIRE.INSURETYPE_CODE , null ) as keepitemtype_code , nvl( LNINSURANCEFIRE.INSURANCE_NO , null ) as description , "
			ls_return		+= " 'I2' as keepotheritemtype_code, "
			ls_return		+= " nvl( LNINSURANCEFIRE.INSURENET_AMT , 0 ) as item_payment , 101 as sort "
			ls_return		+= " FROM LNINSURANCEFIRE,   "
			ls_return		+= " MBMEMBMASTER  "
			ls_return		+= " WHERE ( LNINSURANCEFIRE.MEMBER_NO = MBMEMBMASTER.MEMBER_NO ) and  "
			ls_return		+= " ( LNINSURANCEFIRE.COOP_ID = MBMEMBMASTER.COOP_ID ) and  "
			ls_return		+= " ( ( lninsurancefire.insurepay_status = 0 ) AND  "
			ls_return		+= " ( mbmembmaster.pausekeep_flag = 0 ) AND  "
			ls_return		+= " ( LNINSURANCEFIRE.INSURETYPE_CODE = '20' ) ) "
		end if
		
		if is_coopcontrol = '010001' then
			ls_return		+= " union "
			ls_return		+= " SELECT nvl( LNINSURANCEFIRE.COOP_ID , null ) as coop_id , nvl( MBMEMBMASTER.COOP_ID , null ) as memcoop_id , "
			ls_return		+= " nvl( LNINSURANCEFIRE.MEMBER_NO , null ) as member_no , "
			ls_return		+= " 100 as seq_no , nvl( LNINSURANCEFIRE.INSURANCE_NO , null ) as loancontract_no , "
			ls_return		+= " 'ISF' as keepitemtype_code , 'เก็บเบี้ยประกันภัย' as description , "
			ls_return		+= " '00' as keepotheritemtype_code, "
			ls_return		+= " nvl( LNINSURANCEFIRE.INSURENET_AMT , 0 ) as item_payment , 100 as sort "
			ls_return		+= " FROM LNINSURANCEFIRE , "
			ls_return		+= " MBMEMBMASTER "
			ls_return		+= " WHERE ( LNINSURANCEFIRE.MEMBER_NO = MBMEMBMASTER.MEMBER_NO ) and "
			ls_return		+= " ( LNINSURANCEFIRE.COOP_ID = MBMEMBMASTER.COOP_ID ) and "
			ls_return		+= " ( ( lninsurancefire.mthkeep_status = 8 ) AND "
			ls_return		+= " ( mbmembmaster.resign_status = 0 ) and "
			ls_return		+= " ( mbmembmaster.pausekeep_flag = 0 ) ) and "
			ls_return		+= " ( lninsurancefire.coop_id = '" + is_coopid + "' ) "
		end if
		
		if is_coopcontrol = '003001' then //MHS_tuk15092015
			ls_return		+= " union "
			ls_return		+= " SELECT nvl( MBMEMBMASTER.COOP_ID , null ) as coop_id , nvl( MBMEMBMASTER.COOP_ID , null ) as memcoop_id , "
			ls_return		+= " nvl( INSGROUPMASTER.MEMBER_NO , null ) as member_no , "
			ls_return		+= " 100 as seq_no , nvl( INSGROUPMASTER.INSGROUPDOC_NO , null ) as loancontract_no , "
			ls_return		+= " 'E01' as keepitemtype_code , nvl( INSGROUPMASTER.INSGROUPDOC_NO , null )  as description , "
			ls_return		+= " '01' as keepotheritemtype_code, "
			ls_return		+= " nvl( INSGROUPMASTER.INSPAYMENT_AMT , 0 ) + nvl( INSGROUPMASTER.INSPAYMENT_ARREAR , 0 ) as item_payment , 100 as sort "
			ls_return		+= " FROM INSGROUPMASTER , "
			ls_return		+= " MBMEMBMASTER "
			ls_return		+= " WHERE ( INSGROUPMASTER.MEMBER_NO = MBMEMBMASTER.MEMBER_NO ) and "
			ls_return		+= " ( ( INSGROUPMASTER.INSPAYMENT_STATUS = 8 ) AND "
			ls_return		+= " ( mbmembmaster.resign_status = 0 ) and "
			ls_return		+= " ( mbmembmaster.pausekeep_flag = 0 ) ) and "
			ls_return		+= " ( MBMEMBMASTER.coop_id = '" + is_coopid + "' ) "
		end if
		
		//ส่วนท้าย
		ls_return		+= " ) oth , mbmembmaster "
		ls_return		+= " where mbmembmaster.coop_id = oth.memcoop_id "
		ls_return		+= " and mbmembmaster.member_no = oth.member_no "
		
		if  is_coopcontrol = '009001' then
			ls_return		=	" select oth.coop_id , oth.memcoop_id , trim( oth.member_no) as member_no , oth.startkeep_period, "
			ls_return		+=  " trim( oth.keepitemtype_code) as keepitemtype_code ,trim( oth.description) as description , trim( oth.keepothitemtype_code) as keepothitemtype_code , "
		 	ls_return		+= " nvl( oth.item_payment,0) as item_payment "
		 	ls_return		+= " FROM MBMEMBMASTER , KPRCVKEEPOTHER oth "
		 	ls_return		+= " WHERE mbmembmaster.coop_id = oth.coop_id " 
			ls_return		+= " and MBMEMBMASTER.MEMBER_NO = oth.MEMBER_NO  "
			ls_return		+= " and oth.coop_id = '009001' and oth.item_payment > 0  and oth.startkeep_period = '" + is_recvperiod + "'"
		  
	end if
		
		
		
		astr_extfn_keep.function_return	= ls_return
		
	case 'of_kp_proc_getsql_recpamt'
		// ดึง sql ยอดรวม
		// is_coopid , is_recvperiod
		
		ls_return		= "SELECT trim( KPTEMPRECEIVE.COOP_ID ) as coop_id , "
		ls_return		+= " trim( KPTEMPRECEIVE.KPSLIP_NO ) as kpslip_no , "
//		ls_return		+= " nvl( sum( kptempreceivedet.item_payment * kpucfkeepitemtype.sign_flag ), 0 ) as sumitem_payamt  "
		ls_return		+= " nvl( ( kptempreceivedet.item_payment * kpucfkeepitemtype.sign_flag ), 0 ) as sumitem_payamt  "
		ls_return		+= " FROM KPTEMPRECEIVE, "
		ls_return		+= " KPTEMPRECEIVEDET, "
		ls_return		+= " KPUCFKEEPITEMTYPE "
		ls_return		+= " WHERE KPTEMPRECEIVEDET.COOP_ID = KPTEMPRECEIVE.COOP_ID "
		ls_return		+= " and KPTEMPRECEIVEDET.KPSLIP_NO = KPTEMPRECEIVE.KPSLIP_NO "
		ls_return		+= " and KPTEMPRECEIVEDET.COOP_ID = KPUCFKEEPITEMTYPE.COOP_ID "
		ls_return		+= " and KPTEMPRECEIVEDET.KEEPITEMTYPE_CODE = KPUCFKEEPITEMTYPE.KEEPITEMTYPE_CODE "
		ls_return		+= " and kpucfkeepitemtype.system_flag = 1 "
		ls_return		+= " and kptempreceivedet.keepitemtype_code not in ( 'DPL','DPS' )  "
		ls_return		+= " and kptempreceive.coop_id = '"+ is_coopid +"' "
		ls_return		+= " and kptempreceive.recv_period = '"+ is_recvperiod +"' "
//		ls_return		+= "GROUP BY KPTEMPRECEIVE.COOP_ID, KPTEMPRECEIVE.KPSLIP_NO "
		
		astr_extfn_keep.function_return	= ls_return
		
	case "of_get_fee_hnd"
		 ls_return		= "  SELECT trim(a.MEMBER_NO) as member_no,            a.MEMB_NAME,            a.MEMB_SURNAME,            a.MEMBGROUP_CODE,   "
         ls_return		+= " a.MEMBTYPE_CODE,            b.PERIODSHARE_AMT,            b.PAYMENT_STATUS,            b.MISSPAY_AMT,            b.rcvshrarrear_amt  "
  		 ls_return		+= "FROM MBMEMBMASTER a,              SHSHAREMASTER b "
		ls_return		+= "   WHERE a.member_no =  b.member_no and  a.COOP_ID = '" + is_coopid + "'  and    "
		 ls_return		+= " a.MEMBER_STATUS = 1 AND  b.SHARESTK_AMT > 0   AND      b.rcvshrarrear_amt > 0 "
		astr_extfn_keep.function_return	= ls_return	
	case else
		ithw_exception.text = "ไม่พบฟังก์ชั่น พบข้อผิดพลาด~r~n"
		throw ithw_exception
end choose

return 1
end function

protected function integer of_genkptemp () throws Exception;string ls_memno , ls_memcoop , ls_refmem , ls_refmemcoop , ls_keepdisk
string ls_sqlkp , ls_sqlmem , ls_sqlupkp
string ls_membgrp , ls_comkpgrp
string ls_membtype , ls_department , ls_membsection , ls_refcoop , ls_refmemno
integer li_comkpst
long ll_index , ll_count , ll_addrow
dec{2} ldc_shrbfvalue , ldc_shrvalue , ldc_sumint
boolean lb_update = false

str_extfn_keep lstr_extfn_keep

n_ds lds_memkpinfo

ids_kptp		= create n_ds
ids_kptp.settransobject( itr_sqlca )

lds_memkpinfo	= create n_ds
lds_memkpinfo.settransobject( itr_sqlca )

inv_progress.istr_progress.progress_text		= "สร้างใบทำรายการเรียกเก็บ"
inv_progress.istr_progress.subprogress_text	= "กำลังดึงข้อมูล"
inv_progress.istr_progress.subprogress_index	= 0
inv_progress.istr_progress.subprogress_max	= 0

/////////////////////////////
// Start external function
lstr_extfn_keep.function_name		= 'of_kp_proc_getsql_memkptemp'
this.of_extfn( lstr_extfn_keep )
ls_sqlkp	= lstr_extfn_keep.function_return

lstr_extfn_keep.function_name		= 'of_kp_proc_getsql_memkpinfo'
this.of_extfn( lstr_extfn_keep )
ls_sqlmem	= lstr_extfn_keep.function_return
// End external function
/////////////////////////////

inv_dwsrv.of_create_dw( ids_kptp , ls_sqlkp , '' )
inv_dwsrv.of_create_dw( lds_memkpinfo , ls_sqlmem , '' )

this.of_setsqlselect_kptemp( ids_kptp )
this.of_setsqlselect( lds_memkpinfo )

il_kpcount	= ids_kptp.retrieve()
ll_count	= lds_memkpinfo.retrieve()

inv_progress.istr_progress.subprogress_max	= ll_count

if ll_count < 1 then
	destroy ids_kptp ; destroy lds_memkpinfo
	ithw_exception.text += "~nพบข้อผิดพลาดในการดึงข้อมูลใบเสร็จรับเงิน"
	rollback using itr_sqlca ;
	throw ithw_exception	
end if

// ดึงเลขที่ใบทำรายการล่าสุด
select max(kpslip_no) into :is_kpslipno from kptempreceive where coop_id = :is_coopid and recv_period = :is_recvperiod using itr_sqlca ;
if itr_sqlca.sqlcode <> 0 or isnull( is_kpslipno ) then is_kpslipno = '0'
il_slipno		= long( right( is_kpslipno , 8 ))

//เรียงลำดับเพื่อช่วยในการค้นหาข้อมูลให้เร็วขึ้น
//ids_kptp.setsort(' kptempreceive_coop_id , kptempreceive_recv_period , kptempreceive_member_no , kptempreceive_memcoop_id , kptempreceive_refmemcoop_id , kptempreceive_ref_membno ')
//ids_kptp.setsort(' coop_id , recv_period , member_no , memcoop_id , refmemcoop_id , ref_membno ')
//ids_kptp.setsort( " member_no " )
//ids_kptp.sort()
ids_kptp.setsort( " coop_id , member_no " )
ids_kptp.sort()

//lds_memkpinfo.setsort(' mbmembmaster_member_no ')
//lds_memkpinfo.setsort(' member_no ')
//lds_memkpinfo.sort()
lds_memkpinfo.setsort( " member_no " )
lds_memkpinfo.sort()

for ll_index = 1 to ll_count
	inv_progress.istr_progress.subprogress_index	= ll_index

	// ตรวจสอบการสั่งหยุดทำงาน
	yield()
	if inv_progress.of_is_stop() then
		destroy ids_kptp ; destroy lds_memkpinfo
		return 0
	end if
	
	ls_memno 			= trim( lds_memkpinfo.object.member_no[ll_index] )
//	ls_memno 			= trim( lds_memkpinfo.object.mbmembmaster_member_no[ll_index] )
	ls_memcoop 		= is_coopid
	ls_refmemcoop		= trim( lds_memkpinfo.object.refmemcoop_id[ll_index] )
	ls_refmem 			= trim( lds_memkpinfo.object.ref_membno[ll_index] )
	
	if il_kpcount > 0 then
		is_kpfind	= " coop_id = '"+is_coopid+"' and recv_period = '"+is_recvperiod+"' and member_no = '"+ls_memno+"' and memcoop_id = '"+ls_memcoop+"' "
//		is_kpfind	= " coop_id = '"+is_coopid+"' and recv_period = '"+is_recvperiod+"' and member_no = '"+ls_memno+"' and memcoop_id = '"+ls_memcoop+"' and refmemcoop_id = '"+ls_refmemcoop+"' and ref_membno = '"+ls_refmem+"' "
//		is_kpfind	= " kptempreceive_coop_id = '"+is_coopid+"' and kptempreceive_recv_period = '"+is_recvperiod+"' and kptempreceive_member_no = '"+ls_memno+"' and kptempreceive_memcoop_id = '"+ls_memcoop+"' and kptempreceive_refmemcoop_id = '"+ls_refmemcoop+"' and kptempreceive_ref_membno = '"+ls_refmem+"' "
		il_kpfind		= ids_kptp.find( is_kpfind , il_kpfind , il_kpcount )
	else
		il_kpfind = 0
	end if
	
	inv_progress.istr_progress.subprogress_text = string(ll_index) +". ทะเบียน >"+ls_memno
	
	choose case il_kpfind
		case is > 0
//			// พบข้อมูลไม่ต้องทำไร  รอแก้ไขให้สามารถอัพเดทพวกข้อมูลที่ต้องการได้
//			ls_membgrp 		= trim( lds_memkpinfo.object.membgroup_code[ll_index] )
//			li_comkpst 			= lds_memkpinfo.object.compoundkeep_status[ll_index]
//			if li_comkpst = 1 then
//				ls_comkpgrp	= trim( lds_memkpinfo.object.compoundkeep_group[ll_index] )
//				if isnull(ls_comkpgrp) then ls_comkpgrp = ls_membgrp
//				ls_membgrp		= ls_comkpgrp
//			end if
//			ids_kptp.object.membgroup_code[il_kpfind]	= ls_membgrp
			continue ;
		case 0
			
			lb_update		= true
			
			ls_membgrp 		= trim( lds_memkpinfo.object.membgroup_code[ll_index] )
			ls_keepdisk			= trim( lds_memkpinfo.object.savedisk_type[ll_index] )
			ls_membtype 		= trim( lds_memkpinfo.object.membtype_code[ll_index] )
			ls_department 		= trim( lds_memkpinfo.object.department_code[ll_index] )
//			ls_membsection	= trim( lds_memkpinfo.object.membsection_code[ll_index] )
			li_comkpst 			= lds_memkpinfo.object.compoundkeep_status[ll_index]
			ldc_sumint			= lds_memkpinfo.object.accum_interest[ll_index]
			ldc_shrvalue			= lds_memkpinfo.object.sharestk_value[ll_index]
			ldc_shrbfvalue		= lds_memkpinfo.object.sharestkbf_value[ll_index]

			if li_comkpst = 1 then
				ls_comkpgrp	= trim( lds_memkpinfo.object.compoundkeep_group[ll_index] )
				if isnull(ls_comkpgrp) then ls_comkpgrp = ls_membgrp
				ls_membgrp		= ls_comkpgrp
			end if

			// ไม่พบให้เอาเข้า
			ll_addrow	= ids_kptp.insertrow(0)
			
			// gen เลขที่ใบทำรายการ
			il_slipno++
			is_kpslipno		= is_prefixslip + right( '00000000' + string(il_slipno) , 8 )
			
			ids_kptp.object.coop_id[ll_addrow]				= is_coopid
			ids_kptp.object.kpslip_no[ll_addrow]				= is_kpslipno
			ids_kptp.object.memcoop_id[ll_addrow]			= ls_memcoop
			ids_kptp.object.member_no[ll_addrow]			= ls_memno
			ids_kptp.object.recv_period[ll_addrow]			= is_recvperiod
			ids_kptp.object.refmemcoop_id[ll_addrow]		= ls_refmemcoop
			ids_kptp.object.ref_membno[ll_addrow]			= ls_refmem
			ids_kptp.object.membtype_code[ll_addrow]		= ls_membtype
			ids_kptp.object.department_code[ll_addrow]	= ls_department
//			ids_kptp.object.membsection_code[ll_addrow]	= ls_membsection
			ids_kptp.object.membgroup_code[ll_addrow]	= ls_membgrp
//			ids_kptp.object.process_date[ll_addrow]			= idtm_receipt
//			ids_kptp.object.receipt_no[ll_addrow]			= ''
			ids_kptp.object.receipt_date	[ll_addrow]			= idtm_receipt
			ids_kptp.object.operate_date[ll_addrow]			= idtm_receipt
			ids_kptp.object.sharestkbf_value[ll_addrow]	= ldc_shrbfvalue
			ids_kptp.object.sharestk_value[ll_addrow]		= ldc_shrvalue
			ids_kptp.object.interest_accum[ll_addrow]		= ldc_sumint
			ids_kptp.object.keeping_status[ll_addrow]		= 1
			ids_kptp.object.last_seq_no[ll_addrow]			= 0
			ids_kptp.object.receive_amt[ll_addrow]			= 0
			ids_kptp.object.money_text[ll_addrow]			= ''
//			ids_kptp.object.grt_list[ll_addrow]					= 
//			ids_kptp.object.moneytype_code[ll_addrow]	= 
//			ids_kptp.object.tofrom_accid[ll_addrow]			= 
//			ids_kptp.object.misspay_status[ll_addrow]		= 
//			ids_kptp.object.branch_id[ll_addrow]				= 
//			ids_kptp.object.bank_code[ll_addrow]			= 
//			ids_kptp.object.bank_branch[ll_addrow]			= 
//			ids_kptp.object.bank_accid[ll_addrow]			= 
//			ids_kptp.object.receipt_amt[ll_addrow]			= 
//			ids_kptp.object.receipt_amttext[ll_addrow]		= 
			ids_kptp.object.trtype_code[ll_addrow]			= lds_memkpinfo.object.trtype_code[ll_index]
			ids_kptp.object.expense_code[ll_addrow]		= lds_memkpinfo.object.expense_code[ll_index]
			ids_kptp.object.expense_bank[ll_addrow]		= lds_memkpinfo.object.expense_bank[ll_index]
			ids_kptp.object.expense_branch[ll_addrow]		= lds_memkpinfo.object.expense_branch[ll_index]
			ids_kptp.object.expense_accid[ll_addrow]		= lds_memkpinfo.object.expense_accid[ll_index]
			ids_kptp.object.savedisk_type[ll_addrow]		= ls_keepdisk
			
//			ids_kptp.object.kptempreceive_coop_id[ll_addrow]				= is_coopid
//			ids_kptp.object.kptempreceive_kpslip_no[ll_addrow]				= is_kpslipno
//			ids_kptp.object.kptempreceive_memcoop_id[ll_addrow]			= ls_memcoop
//			ids_kptp.object.kptempreceive_member_no[ll_addrow]			= ls_memno
//			ids_kptp.object.kptempreceive_recv_period[ll_addrow]			= is_recvperiod
//			ids_kptp.object.kptempreceive_refmemcoop_id[ll_addrow]		= ls_refmemcoop
//			ids_kptp.object.kptempreceive_ref_membno[ll_addrow]			= ls_refmem
//			ids_kptp.object.kptempreceive_membgroup_code[ll_addrow]	= ls_membgrp
//			ids_kptp.object.kptempreceive_receipt_date	[ll_addrow]			= idtm_receipt
//			ids_kptp.object.kptempreceive_operate_date[ll_addrow]			= idtm_receipt
//			ids_kptp.object.kptempreceive_sharestkbf_value[ll_addrow]	= ldc_shrbfvalue
//			ids_kptp.object.kptempreceive_sharestk_value[ll_addrow]		= ldc_shrvalue
//			ids_kptp.object.kptempreceive_keeping_status[ll_addrow]		= 1
//			ids_kptp.object.kptempreceive_last_seq_no[ll_addrow]			= 0
			
		case else
			ithw_exception.text += "ไม่พบข้อมูลสมาชิกในการประมวล"
			ithw_exception.text += "~r~n" + ids_kptp.of_geterrormessage()
			destroy ids_kptp ; destroy lds_memkpinfo
			rollback using itr_sqlca ;
			throw ithw_exception	
	end choose

next

lstr_extfn_keep.function_name		= 'of_kp_proc_setupdatesql_memkptemp'
this.of_extfn( lstr_extfn_keep )
inv_dwsrv.of_update_dw( ids_kptp , 'KPTEMPRECEIVE' , lstr_extfn_keep.column_update , lstr_extfn_keep.column_key , '0' , true )

if lb_update then
	if ids_kptp.update() <> 1 then
		ithw_exception.text += "ไม่สามารถสร้างหัวใบเสร็จประจำเดือนได้"
		ithw_exception.text += "~r~n" + ids_kptp.of_geterrormessage()
		destroy ids_kptp ; destroy lds_memkpinfo
		rollback using itr_sqlca ;
		throw ithw_exception	
	end if
end if

il_kpcount	= ids_kptp.rowcount()

destroy lds_memkpinfo

return 1
end function

protected subroutine of_getkpslip () throws Exception;/***********************************************************
<description>
	ดึงค่าเลขที่ใบทำรายการประมวลล่าสุด
</description>

<arguments>  

</arguments> 

<return> 
	Integer	1 = success,  Exception = failure
</return> 

<usage> 
	Revision History:
	Version 1.0 (Initial version) Modified Date 31/01/2011 by MiT
</usage> 
***********************************************************/
il_slipno++
is_kpslipno	= is_prefixslip + right( '000000' + string( il_slipno ) , 6 )
end subroutine

protected function integer of_hd_010001 () throws Exception;string ls_coop , ls_kpslip , ls_memcoop , ls_memno , ls_recv , ls_refmemcoop , ls_refmem , ls_membtyp , ls_depart , ls_memsect 
string ls_memgrp , ls_receipt 
string ls_moneytxt , ls_grtlist , ls_moneytype , ls_tofrom , ls_bank , ls_bankbranch , ls_bankacc , ls_receipttxt , ls_expcode , ls_expbank , ls_expbranch , ls_expacc
string ls_savedisk , ls_trtype
integer li_keepsts , li_misspay , li_lastseq , li_keepsal
integer li_cnt
dec{2} ldc_shrstkbf , ldc_shrstk , ldc_intacc , ldc_receive , ldc_receipt
datetime ldtm_process , ldtm_receipt , ldtm_operate 
/*
update ref_membno
*/
update kptempreceive
set ref_membno = member_no
where coop_id = :is_coopcontrol
and recv_period = :is_recvperiod
and ( ref_membno is null or ref_membno = '' )
using itr_sqlca ;

update kptempreceivedet
set ref_membno = member_no
where coop_id = :is_coopcontrol
and recv_period = :is_recvperiod
and ( ref_membno is null or ref_membno = '' )
using itr_sqlca ;


/* ประเภทการหักชำระ KEEP2 จากประเภทเรียกเก็บ
595->581*/
insert into kptempreceive
( coop_id , kpslip_no , memcoop_id , member_no , recv_period , refmemcoop_id , ref_membno , membtype_code , department_code , membsection_code , 
membgroup_code , process_date , receipt_no , receipt_date , operate_date , sharestkbf_value , sharestk_value , interest_accum , keeping_status , receive_amt , 
money_text , grt_list , moneytype_code , tofrom_accid , misspay_status , bank_code , bank_branch , bank_accid , receipt_amt , receipt_amttext , expense_code , 
expense_bank , expense_branch , expense_accid , last_seq_no , savedisk_type , trtype_code , keepsal_flag )
select kp.coop_id , ( substr( kp.kpslip_no , 1 , 4 ) || 'T' || substr( kp.kpslip_no , 6 , 7 ) )  as kpslip_no, kp.memcoop_id , kp.member_no , kp.recv_period , kp.refmemcoop_id , 'KEEPOUT' as ref_membno , kp.membtype_code , kp.department_code , kp.membsection_code , 
kp.membgroup_code , kp.process_date , kp.receipt_no , kp.receipt_date , kp.operate_date , kp.sharestkbf_value , kp.sharestk_value , kp.interest_accum , kp.keeping_status , kp.receive_amt , 
kp.money_text , kp.grt_list , kp.moneytype_code , kp.tofrom_accid , kp.misspay_status , kp.bank_code , kp.bank_branch , kp.bank_accid , kp.receipt_amt , kp.receipt_amttext , tp.expense_code , 
tp.expense_bank , tp.expense_branch , tp.expense_accid , 0 as last_seq_no , kp.savedisk_type , 'KEEP2' as trtype_code , 0 as keepsal_flag
from kptempreceive kp , (
	select rank() over ( partition by kpslip_no order by priority_amt desc ) as priority_no , coop_id , kpslip_no , moneytype_code as expense_code , bank_code as expense_bank , bank_branch as expense_branch , bank_accid as expense_accid
	from(
		/*KEEP2 From Mbmembmoneytr*/
		select 100 as priority_amt , kd.coop_id , kd.kpslip_no , mt.moneytype_code , mt.bank_code , mt.bank_branch , mt.bank_accid
		from kptempreceivedet kd , kpucfkeepitemtype ki , mbmembmoneytr mt
		where kd.coop_id = ki.coop_id
		and kd.keepitemtype_code = ki.keepitemtype_code
		and kd.memcoop_id = mt.coop_id
		and kd.member_no = mt.member_no
		and kd.coop_id = :is_coopcontrol
		and kd.recv_period = :is_recvperiod
		and ki.trtype_code = 'KEEP2'
		and mt.trtype_code = 'KEEP2'
//		and mt.override_flag = 1
		group by kd.coop_id , kd.kpslip_no , mt.moneytype_code , mt.bank_code , mt.bank_branch , mt.bank_accid
		union
		/*KEEP2 From Mbmembmaster*/
		select 10 as priority_amt , kd.coop_id , kd.kpslip_no , m.expense_code , m.expense_bank , m.expense_branch , m.expense_accid
		from kptempreceivedet kd , kpucfkeepitemtype ki , mbmembmaster m
		where kd.coop_id = ki.coop_id
		and kd.keepitemtype_code = ki.keepitemtype_code
		and kd.memcoop_id = m.coop_id
		and kd.member_no = m.member_no
		and kd.coop_id = :is_coopcontrol
		and kd.recv_period = :is_recvperiod
		and ki.trtype_code = 'KEEP2'
		group by kd.coop_id , kd.kpslip_no , m.expense_code , m.expense_bank , m.expense_branch , m.expense_accid
	)
) tp , mbmembmaster m
where kp.coop_id = tp.coop_id
and kp.kpslip_no = tp.kpslip_no
and kp.coop_id = m.coop_id
and kp.member_no = m.member_no
and kp.coop_id = :is_coopcontrol
and kp.recv_period = :is_recvperiod
and m.retry_status = 0
and tp.priority_no = 1
//and kp.trtype_code = 'KEEP1'
and exists( select 1 from mbmembmoneytr mtr where mtr.coop_id = kp.coop_id and mtr.member_no = kp.member_no and mtr.trtype_code = 'KEEP1' and mtr.moneytype_code = 'SAL' )
using itr_sqlca ;

update 	kptempreceivedet ktd
set 		ktd.kpslip_no = ( substr( ktd.kpslip_no , 1 , 4 ) || 'T' || substr( ktd.kpslip_no , 6 , 7 ) )
where 	( ktd.coop_id , ktd.kpslip_no , seq_no ) in (		select kd.coop_id , kd.kpslip_no , seq_no
																		from kptempreceive k , kptempreceivedet kd , kpucfkeepitemtype ki , mbmembmaster m
																		where k.coop_id					= kd.coop_id
																		and k.kpslip_no						= kd.kpslip_no
																		and kd.coop_id 					= ki.coop_id
																		and kd.keepitemtype_code 		= ki.keepitemtype_code
																		and k.coop_id						= m.coop_id
																		and k.member_no					= m.member_no
																		and kd.coop_id 					= :is_coopcontrol
																		and kd.recv_period 				= :is_recvperiod
																		and ki.trtype_code 				= 'KEEP2'
																		and m.retry_status = 0	
																		and exists( select 1 from mbmembmoneytr mtr where mtr.coop_id = k.coop_id and mtr.member_no = k.member_no and mtr.trtype_code = 'KEEP1' and mtr.moneytype_code = 'SAL' )
																		)
using itr_sqlca ;

/* ประเภทการหักชำระ KEEP2 mbmembmoneytr
*/
insert into kptempreceive
( coop_id , kpslip_no , memcoop_id , member_no , recv_period , refmemcoop_id , ref_membno , membtype_code , department_code , membsection_code , 
membgroup_code , process_date , receipt_no , receipt_date , operate_date , sharestkbf_value , sharestk_value , interest_accum , keeping_status , receive_amt , 
money_text , grt_list , moneytype_code , tofrom_accid , misspay_status , bank_code , bank_branch , bank_accid , receipt_amt , receipt_amttext , expense_code , 
expense_bank , expense_branch , expense_accid , last_seq_no , savedisk_type , trtype_code , keepsal_flag )
select kp.coop_id , ( substr( kp.kpslip_no , 1 , 4 ) || 'T' || substr( kp.kpslip_no , 6 , 7 ) )  as kpslip_no, kp.memcoop_id , kp.member_no , kp.recv_period , kp.refmemcoop_id , 'KEEPOUT' as ref_membno , kp.membtype_code , kp.department_code , kp.membsection_code , 
kp.membgroup_code , kp.process_date , kp.receipt_no , kp.receipt_date , kp.operate_date , kp.sharestkbf_value , kp.sharestk_value , kp.interest_accum , kp.keeping_status , kp.receive_amt , 
kp.money_text , kp.grt_list , kp.moneytype_code , kp.tofrom_accid , kp.misspay_status , kp.bank_code , kp.bank_branch , kp.bank_accid , kp.receipt_amt , kp.receipt_amttext , tp.expense_code , 
tp.expense_bank , tp.expense_branch , tp.expense_accid , 0 as last_seq_no , kp.savedisk_type , 'KEEP2' as trtype_code , 0 as keepsal_flag
from kptempreceive kp , (
	select rank() over ( partition by kpslip_no order by priority_amt desc ) as priority_no , coop_id , kpslip_no , moneytype_code as expense_code , bank_code as expense_bank , bank_branch as expense_branch , bank_accid as expense_accid
	from(
		select 100 as priority_amt , kd.coop_id , kd.kpslip_no , mt.moneytype_code , mt.bank_code , mt.bank_branch , mt.bank_accid
		from kptempreceivedet kd , lncontmaster lm , lnucfloanpay lup , mbmembmoneytr mt
		where kd.memcoop_id = mt.coop_id
		and kd.member_no = mt.member_no
		and kd.bizzcoop_id = lm.coop_id
		and kd.loancontract_no = lm.loancontract_no
		and lm.loanpay_code = lup.loanpay_code
		and lup.keep_flag >= 2
		and kd.coop_id = :is_coopcontrol
		and kd.recv_period = :is_recvperiod
		and mt.trtype_code = 'KEEP2'
//		and mt.override_flag = 1
		group by kd.coop_id , kd.kpslip_no , mt.moneytype_code , mt.bank_code , mt.bank_branch , mt.bank_accid
		union
		select 10 as priority_amt , kd.coop_id , kd.kpslip_no , m.expense_code , m.expense_bank , m.expense_branch , m.expense_accid
		from kptempreceivedet kd , lncontmaster lm , lnucfloanpay lup , mbmembmaster m
		where kd.memcoop_id = m.coop_id
		and kd.member_no = m.member_no
		and kd.bizzcoop_id = lm.coop_id
		and kd.loancontract_no = lm.loancontract_no
		and lm.loanpay_code = lup.loanpay_code
		and lup.keep_flag >= 2
		and kd.memcoop_id = m.coop_id
		and kd.member_no = m.member_no
		and kd.coop_id = :is_coopcontrol
		and kd.recv_period = :is_recvperiod
		group by kd.coop_id , kd.kpslip_no , m.expense_code , m.expense_bank , m.expense_branch , m.expense_accid
	)
) tp , mbmembmaster m
where kp.coop_id = tp.coop_id
and kp.kpslip_no = tp.kpslip_no
and kp.coop_id = m.coop_id
and kp.member_no = m.member_no
and kp.coop_id = :is_coopcontrol
and kp.recv_period = :is_recvperiod
and ( kp.trtype_code <> 'KEEP2' or kp.trtype_code is null )
and not exists( select 1 from kptempreceive k where k.coop_id = :is_coopcontrol and k.recv_period = :is_recvperiod and k.coop_id = kp.coop_id and replace( k.kpslip_no , 'T' , '0' ) = kp.kpslip_no and k.trtype_code = 'KEEP2' )
//and m.retry_status = 0
and tp.priority_no = 1
//and exists( select 1 from mbmembmoneytr mtr where mtr.coop_id = kp.coop_id and mtr.member_no = kp.member_no and mtr.trtype_code = 'KEEP1' and mtr.moneytype_code = 'SAL' )
using itr_sqlca ;

update 	kptempreceivedet ktd
set 		ktd.kpslip_no = ( substr( ktd.kpslip_no , 1 , 4 ) || 'T' || substr( ktd.kpslip_no , 6 , 7 ) )
where 	( ktd.coop_id , ktd.kpslip_no , seq_no ) in (		select kd.coop_id , kd.kpslip_no , seq_no
																		from kptempreceive k , kptempreceivedet kd , lncontmaster lm , lnucfloanpay lup , mbmembmaster m
																		where k.coop_id = kd.coop_id
																		and k.kpslip_no = kd.kpslip_no
																		and kd.bizzcoop_id = lm.coop_id
																		and kd.loancontract_no = lm.loancontract_no
																		and lm.loanpay_code = lup.loanpay_code
																		and k.coop_id  = m.coop_id
																		and k.member_no = m.member_no
//																		and m.retry_status = 0
																		and lup.keep_flag >= 2	
																		and kd.coop_id = :is_coopcontrol
																		and kd.recv_period = :is_recvperiod
//																		and exists( select 1 from mbmembmoneytr mtr where mtr.coop_id = k.coop_id and mtr.member_no = k.member_no and mtr.trtype_code = 'KEEP1' and mtr.moneytype_code = 'SAL' )
																	)
using itr_sqlca ;


/*
แยกภาระค้ำอีกใบเสร็จ
*/
/*
insert into kptempreceive
( coop_id , kpslip_no , memcoop_id , member_no , recv_period , refmemcoop_id , ref_membno , membtype_code , department_code , membsection_code , 
membgroup_code , process_date , receipt_no , receipt_date , operate_date , sharestkbf_value , sharestk_value , interest_accum , keeping_status , receive_amt , 
money_text , grt_list , moneytype_code , tofrom_accid , misspay_status , bank_code , bank_branch , bank_accid , receipt_amt , receipt_amttext , expense_code , 
expense_bank , expense_branch , expense_accid , last_seq_no , savedisk_type , trtype_code , keepsal_flag )
select kp.coop_id , ( substr( kp.kpslip_no , 1 , 4 ) || 'K' || substr( kp.kpslip_no , 6 , 7 ) )  as kpslip_no, kp.memcoop_id , kp.member_no , kp.recv_period , kpd.refmemcoop_id , kpd.ref_membno , kp.membtype_code , kp.department_code , kp.membsection_code , 
kp.membgroup_code , kp.process_date , kp.receipt_no , kp.receipt_date , kp.operate_date , kp.sharestkbf_value , kp.sharestk_value , kp.interest_accum , kp.keeping_status , kp.receive_amt , 
kp.money_text , kp.grt_list , kp.moneytype_code , kp.tofrom_accid , kp.misspay_status , kp.bank_code , kp.bank_branch , kp.bank_accid , kp.receipt_amt , kp.receipt_amttext , kp.expense_code , 
kp.expense_bank , kp.expense_branch , kp.expense_accid , 0 as last_seq_no , kp.savedisk_type , 'COLL' as trtype_code , 0 as keepsal_flag
from kptempreceive kp , kptempreceivedet kpd , mbmembmaster m
where kp.coop_id = m.coop_id
and kp.member_no = m.member_no
and kp.coop_id = kpd.coop_id
and kp.kpslip_no = kpd.kpslip_no
and kp.coop_id = :is_coopcontrol
and kp.recv_period = :is_recvperiod
and kpd.member_no <> kpd.ref_membno
and kpd.ref_membno <> 'KEEPOUT'
and m.retry_status = 0
using itr_sqlca ;

update 	kptempreceivedet ktd
set 		ktd.kpslip_no = ( substr( ktd.kpslip_no , 1 , 4 ) || 'K' || substr( ktd.kpslip_no , 6 , 7 ) )
where 	( ktd.coop_id , ktd.kpslip_no , seq_no ) in (		select kd.coop_id , kd.kpslip_no , kd.seq_no
																		from kptempreceive k , kptempreceivedet kd , mbmembmaster m
																		where k.coop_id = m.coop_id
																		and k.member_no = m.member_no
																		and k.coop_id = kd.coop_id
																		and k.kpslip_no = kd.kpslip_no
																		and k.coop_id = :is_coopcontrol
																		and k.recv_period = :is_recvperiod
																		and kd.member_no <> kd.ref_membno
																		and kd.ref_membno <> 'KEEPOUT'
																		and m.retry_status = 0
																		)
using itr_sqlca ;

update kptempreceive k
set ( k.receive_amt , k.money_text ) = ( select sum( kd.item_payment ) , ftreadtbath( sum( kd.item_payment ) )
													from kptempreceivedet kd
													where kd.coop_id = k.coop_id
													and kd.kpslip_no = k.kpslip_no
													)
where k.coop_id = :is_coopcontrol
and k.recv_period = :is_recvperiod
using itr_sqlca ;
*/

declare data_cur cursor for
select kp.coop_id , ( substr( kp.kpslip_no , 1 , 4 ) || 'K' || substr( kp.kpslip_no , 6 , 7 ) )  as kpslip_no, kp.memcoop_id , kp.member_no , kp.recv_period , kpd.refmemcoop_id , kpd.ref_membno , kp.membtype_code , kp.department_code , kp.membsection_code , 
kp.membgroup_code , kp.process_date , kp.receipt_no , kp.receipt_date , kp.operate_date , kp.sharestkbf_value , kp.sharestk_value , kp.interest_accum , kp.keeping_status , kp.receive_amt , 
kp.money_text , kp.grt_list , kp.moneytype_code , kp.tofrom_accid , kp.misspay_status , kp.bank_code , kp.bank_branch , kp.bank_accid , kp.receipt_amt , kp.receipt_amttext , kp.expense_code , 
kp.expense_bank , kp.expense_branch , kp.expense_accid , 0 as last_seq_no , kp.savedisk_type , 'COLL' as trtype_code , 0 as keepsal_flag
from kptempreceive kp , kptempreceivedet kpd , mbmembmaster m
where kp.coop_id = m.coop_id
and kp.member_no = m.member_no
and kp.coop_id = kpd.coop_id
and kp.kpslip_no = kpd.kpslip_no
and kp.coop_id = :is_coopcontrol
and kp.recv_period = :is_recvperiod
and kpd.member_no <> kpd.ref_membno
and kpd.ref_membno <> 'KEEPOUT'
and m.retry_status = 0
order by kp.coop_id , kp.member_no , kp.ref_membno
using itr_sqlca ;
open data_cur ;
	fetch data_cur into 	:ls_coop , :ls_kpslip , :ls_memcoop , :ls_memno , :ls_recv , :ls_refmemcoop , :ls_refmem , :ls_membtyp , :ls_depart , :ls_memsect ,
								:ls_memgrp , :ldtm_process , :ls_receipt , :ldtm_receipt , :ldtm_operate , :ldc_shrstkbf , :ldc_shrstk , :ldc_intacc , :li_keepsts , :ldc_receive , 
								:ls_moneytxt , :ls_grtlist , :ls_moneytype , :ls_tofrom , :li_misspay , :ls_bank , :ls_bankbranch , :ls_bankacc , :ldc_receipt , :ls_receipttxt , :ls_expcode ,
								:ls_expbank , :ls_expbranch , :ls_expacc , :li_lastseq , :ls_savedisk , :ls_trtype , :li_keepsal ;
	do while itr_sqlca.sqlcode = 0
		
		li_cnt = 0
		select count( kpslip_no )
		into :li_cnt
		from kptempreceive
		where coop_id = :ls_coop
		and kpslip_no = :ls_kpslip
		using itr_sqlca;
		
		if li_cnt > 0 then
			ls_kpslip	= left( ls_kpslip , 5 ) + string( li_cnt ) + right( ls_kpslip , 6 )
		end if
		
		insert into kptempreceive
		( coop_id , kpslip_no , memcoop_id , member_no , recv_period , refmemcoop_id , ref_membno , membtype_code , department_code , membsection_code , 
		membgroup_code , process_date , receipt_no , receipt_date , operate_date , sharestkbf_value , sharestk_value , interest_accum , keeping_status , receive_amt , 
		money_text , grt_list , moneytype_code , tofrom_accid , misspay_status , bank_code , bank_branch , bank_accid , receipt_amt , receipt_amttext , expense_code , 
		expense_bank , expense_branch , expense_accid , last_seq_no , savedisk_type , trtype_code , keepsal_flag )
		values
		( 	:ls_coop , :ls_kpslip , :ls_memcoop , :ls_memno , :ls_recv , :ls_refmemcoop , :ls_refmem , :ls_membtyp , :ls_depart , :ls_memsect ,
			:ls_memgrp , :ldtm_process , :ls_receipt , :ldtm_receipt , :ldtm_operate , :ldc_shrstkbf , :ldc_shrstk , :ldc_intacc , :li_keepsts , :ldc_receive , 
			:ls_moneytxt , :ls_grtlist , :ls_moneytype , :ls_tofrom , :li_misspay , :ls_bank , :ls_bankbranch , :ls_bankacc , :ldc_receipt , :ls_receipttxt , :ls_expcode ,
			:ls_expbank , :ls_expbranch , :ls_expacc , :li_lastseq , :ls_savedisk , :ls_trtype , :li_keepsal)
		using itr_sqlca;

		update 	kptempreceivedet ktd
		set 		ktd.kpslip_no = :ls_kpslip
		where 	( ktd.coop_id , ktd.kpslip_no , ktd.seq_no , ktd.refmemcoop_id , ktd.ref_membno ) in 
																		(		select kd.coop_id , kd.kpslip_no , kd.seq_no , kd.refmemcoop_id , kd.ref_membno
																				from kptempreceive k , kptempreceivedet kd , mbmembmaster m
																				where k.coop_id = m.coop_id
																				and k.member_no = m.member_no
																				and k.coop_id = kd.coop_id
																				and k.kpslip_no = kd.kpslip_no
																				and k.coop_id = :is_coopcontrol
																				and k.recv_period = :is_recvperiod
																				and k.member_no = :ls_memno
																				and kd.ref_membno = :ls_refmem
																				and m.retry_status = 0
																				)
		using itr_sqlca ;

		fetch data_cur into 	:ls_coop , :ls_kpslip , :ls_memcoop , :ls_memno , :ls_recv , :ls_refmemcoop , :ls_refmem , :ls_membtyp , :ls_depart , :ls_memsect ,
									:ls_memgrp , :ldtm_process , :ls_receipt , :ldtm_receipt , :ldtm_operate , :ldc_shrstkbf , :ldc_shrstk , :ldc_intacc , :li_keepsts , :ldc_receive , 
									:ls_moneytxt , :ls_grtlist , :ls_moneytype , :ls_tofrom , :li_misspay , :ls_bank , :ls_bankbranch , :ls_bankacc , :ldc_receipt , :ls_receipttxt , :ls_expcode ,
									:ls_expbank , :ls_expbranch , :ls_expacc , :li_lastseq , :ls_savedisk , :ls_trtype , :li_keepsal ;
		
	loop
close data_cur ;

update kptempreceive k
set ( k.receive_amt , k.money_text ) = ( select sum( kd.item_payment ) , ftreadtbath( sum( kd.item_payment ) )
													from kptempreceivedet kd
													where kd.coop_id = k.coop_id
													and kd.kpslip_no = k.kpslip_no
													)
where k.coop_id = :is_coopcontrol
and k.recv_period = :is_recvperiod
using itr_sqlca ;

///*สุดท้ายอัพเดท ref_membno จาก kptempreceive -> kptempreceivedet */
//update kptempreceivedet kd
//set kd.ref_membno = ( select k.ref_membno from kptempreceive k where k.coop_id = kd.coop_id and k.kpslip_no = kd.kpslip_no )
//where kd.coop_id = :is_coopcontrol
//and kd.recv_period = :is_recvperiod
//and kd.trtype_code in ( '
//using itr_sqlca ;
/*
Gen expense
*/
delete from kpreceiveexpense where coop_id = :is_coopcontrol and recv_period = :is_recvperiod using itr_sqlca ;
insert into kpreceiveexpense
(
coop_id , recv_period , kpslip_no , seq_no , member_no , memcoop_id , moneytype_code , 
item_payment , item_status , expense_bank , expense_branch, expense_accid , 
tofrom_accid , kpseq_flag , kpseq_no , post_status ,
expense_code
)
select coop_id , recv_period , kpslip_no , 1 as seq_no , member_no , memcoop_id , 
( case when expense_code = 'TRN' then 'TDP' when expense_code is null then 'CSH' else expense_code end ) as moneytype_code ,
receive_amt , 1 as item_status , expense_bank , expense_branch , expense_accid ,
'' as tofrom_accid , 0 as kpseq_flag , 0 as kpseq_no , 0 as post_status ,
( case when expense_code = 'TRN' then 'TDP' when expense_code is null then 'CSH' when expense_code = 'TMT' then 'SAL' else expense_code end ) as expense_code
 from kptempreceive 
where coop_id = :is_coopcontrol
and recv_period = :is_recvperiod
using itr_sqlca ;

return 1
end function

protected function integer of_process_genexpense () throws Exception;/***********************************************************
<description>
	Gen ข้อมูล
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
string ls_sqlext , ls_temp , ls_bizzcoop
string ls_kpslipno , ls_tpkpslipno, ls_memcoop , ls_memno
string ls_moneytyp , ls_expbank , ls_expbranch , ls_expaccid
string ls_mthtype 
integer li_srtmthcut , li_seqno
long ll_index, ll_count
dec{2} ldc_mthvalue , ldc_recvamt , ldc_itempay
dec{6} ldc_mthpercent

n_ds	lds_expense

inv_progress.istr_progress.progress_text	= "ทำรายการประเภทการชำระยอดเรียกเก็บประจำเดือน"
inv_progress.istr_progress.subprogress_text	= "กำลังดึงข้อมูล"
inv_progress.istr_progress.subprogress_index	= 0
inv_progress.istr_progress.subprogress_max	= 0

lds_expense	= create n_ds
lds_expense.dataobject = "d_kp_rcvproc_expense"
lds_expense.settransobject( itr_sqlca )

ls_sqlext = "and ( kptempreceive.coop_id = '" + is_coopid + "' ) "

choose case ii_proctype
	case 1
		ls_sqlext	= "and ( kptempreceive.recv_period = '"+is_recvperiod+"' ) "
	case 2
		inv_stringsrv.of_buildsqlext( is_arg[], "kptempreceive.membgroup_code", ls_sqlext )
		ls_sqlext	= "and ( kptempreceive.recv_period = '"+is_recvperiod+"' ) and "+ls_sqlext
	case 3
		inv_stringsrv.of_buildsqlext(is_arg[], "kptempreceive.member_no", ls_sqlext)
		ls_sqlext	= "and ( kptempreceive.recv_period = '"+is_recvperiod+"' ) and "+ls_sqlext
end choose

ls_temp		= lds_expense.getsqlselect()
ls_temp		+= ls_sqlext

lds_expense.setsqlselect( ls_temp )

ll_count	= lds_expense.retrieve()
lds_expense.setsort( ' coop_id , member_no , sort_in_monthlycut ' )
lds_expense.sort()

inv_progress.istr_progress.subprogress_max	= ll_count

for ll_index = 1 to ll_count
	inv_progress.istr_progress.subprogress_index	= ll_index

	// ตรวจสอบการสั่งหยุดทำงาน
	yield()
	if inv_progress.of_is_stop() then
		destroy lds_expense
		return 0
	end if

	ls_kpslipno		= trim( lds_expense.object.kpslip_no[ll_index] )
	ls_bizzcoop		= trim( lds_expense.object.bizzcoop_id[ll_index] )
	ls_memcoop	= lds_expense.object.memcoop_id[ll_index]
	ls_memno		= trim( lds_expense.object.member_no[ll_index] )
	ls_moneytyp 	= lds_expense.object.moneytype_code[ll_index]
	ls_expbank 		= lds_expense.object.expense_bank[ll_index]
	ls_expbranch 	= lds_expense.object.expense_branch[ll_index]
	ls_expaccid		= lds_expense.object.expense_accid[ll_index]
	ls_mthtype		= lds_expense.object.monthlycut_type[ll_index] ; if isnull( ls_mthtype ) then ls_mthtype = 'A'
	li_seqno			= lds_expense.object.seq_no[ll_index]
	li_srtmthcut		= lds_expense.object.sort_in_monthlycut[ll_index]
	ldc_mthpercent	= lds_expense.object.monthlycut_percent[ll_index]
	ldc_mthvalue	= lds_expense.object.monthlycut_value[ll_index]

	if ls_tpkpslipno <> ls_kpslipno then 
		ldc_recvamt		= lds_expense.object.receive_amt[ll_index]
		ldc_itempay		= 0
	end if

	if isnull( ls_moneytyp ) then ls_moneytyp = ""
	if isnull( ls_moneytyp ) or len( ls_moneytyp ) = 0 then 
		this.of_set_msgerr( "ไม่พบข้อมูลประเภทการชำระยอดเรียกเก็บ ทะเบียน : " + ls_memno )
		continue ;
	end if

	choose case ls_mthtype
		case 'A'
			ldc_itempay		= ldc_recvamt
		case 'P'
			ldc_itempay		= round( truncate( ldc_mthpercent * ldc_recvamt , 3 ) , 2 )
		case 'V'
			ldc_itempay		= ldc_mthvalue
		case else
			continue ;
	end choose

	ldc_recvamt		-= ldc_itempay

	// insert ข้อมูลประเภทการชำระเงิน
	insert into kpreceiveexpense
	(	coop_id , 				recv_period , 			kpslip_no , 		seq_no , 				member_no , 			memcoop_id , 		bizzcoop_id ,
		moneytype_code , 	item_payment , 		item_status , 	expense_bank , 	expense_branch , 		expense_accid , 
		tofrom_accid , 			kpseq_flag , 			kpseq_no )
	values
	(	:is_coopid ,				:is_recvperiod ,			:ls_kpslipno ,	:li_seqno ,			:ls_memno ,			:ls_memcoop ,		:ls_bizzcoop ,
		:ls_moneytyp ,			:ldc_itempay ,			0 ,					:ls_expbank ,		:ls_expbranch ,			:ls_expaccid ,
		'' ,							0 ,							0)
	using itr_sqlca ;
			  
	if itr_sqlca.sqlcode = 0 then
		inv_progress.istr_progress.subprogress_text = string( ll_index ) +". ทะเบียน "+ls_memno+" > จำนวนเงิน "+string( ldc_itempay, "#,##0.00" ) + " > " + ls_moneytyp
	else
		if ii_msgerrflag = 1 then
			this.of_set_msgerr( "ประเภทชำระยอดเรียกเก็บ ทะเบียน : " + ls_memno + " > " + string( itr_sqlca.sqlcode ) + " < " + itr_sqlca.sqlerrtext )
		else
			ithw_exception.text = "ประเภทชำระยอดเรียกเก็บ พบข้อผิดพลาด"
			ithw_exception.text += "~r~nทะเบียน : " + ls_memno
			ithw_exception.text += "~r~n"+ string( itr_sqlca.sqlcode )
			ithw_exception.text += "~r~n"+ itr_sqlca.sqlerrtext
			destroy lds_expense 
			return -1
		end if
	end if

	ls_tpkpslipno	= ls_kpslipno

next

destroy lds_expense

return 1
end function

protected function integer of_processdeposit () throws Exception;/***********************************************************
<description>
	ประมวลเรียกเก็บเงินฝาก
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

string			ls_memno, ls_memprior, ls_membgroup, ls_sqlext, ls_temp, ls_subgrp
string			ls_keeptype, ls_depttype, ls_deptgroup, ls_deptaccno, ls_moneytype, ls_accid
string			ls_memcoop , ls_refmem , ls_refmemcoop
string			ls_sqldept , ls_deptcoopid
long			ll_index, ll_count
integer		li_seqno, li_chk, li_stepvalue, li_ret, li_compoundkeep
dec{2}		ldc_deptamt, ldc_shrstk, ldc_shrbegin
n_ds	lds_deposit
str_extfn_keep lstr_extfn_keep
boolean	lb_multi

inv_progress.istr_progress.progress_text		= "เงินฝากรายเดือน"
inv_progress.istr_progress.subprogress_text	= "กำลังดึงข้อมูล"
inv_progress.istr_progress.subprogress_index	= 0
inv_progress.istr_progress.subprogress_max	= 0

lds_deposit	= create n_ds
lds_deposit.settransobject( itr_sqlca )

lstr_extfn_keep.function_name	= 'of_kp_proc_getsql_dept'
this.of_extfn( lstr_extfn_keep )
ls_sqldept	= lstr_extfn_keep.function_return

inv_dwsrv.of_create_dw( lds_deposit , ls_sqldept , '' )

// สร้างชุด SQL สำหรับการดึงรายการ
li_ret = this.of_setsqlselect( lds_deposit )
if ( li_ret <> 1 ) then
	destroy lds_deposit ; destroy ids_kptp
	ithw_exception.text += "~nพบข้อผิดพลาดในการสร้างชุด SQL รายการเรียกเก็บเงินฝากรายเดือน"
	throw ithw_exception	
end if

ll_count = lds_deposit.retrieve()

lds_deposit.setsort( " memcoop_id , member_no , member_ref desc , coop_id , deptgroup_code , depttype_code , deptaccount_no " )
lds_deposit.sort()

inv_progress.istr_progress.subprogress_max = ll_count

for ll_index = 1 to ll_count
	
	inv_progress.istr_progress.subprogress_index = ll_index
	
	// ตรวจสอบการสั่งหยุดทำงาน
	yield()
	if inv_progress.of_is_stop() then
		destroy lds_deposit ; destroy ids_kptp
		return 0
	end if
	
	ls_memno		= trim( lds_deposit.getitemstring( ll_index, "member_no" ) )
	ls_memcoop		= trim( lds_deposit.getitemstring( ll_index, "memcoop_id" ) )
	ls_refmem		= lds_deposit.object.member_ref[ll_index]
	ls_refmemcoop	= is_coopcontrol
	ls_depttype		= trim( lds_deposit.getitemstring( ll_index, "depttype_code" ) )
	ls_deptgroup	= trim( lds_deposit.getitemstring( ll_index, "deptgroup_code" ) )
	ls_deptcoopid	= trim( lds_deposit.getitemstring( ll_index, "coop_id" ))
	ls_deptaccno	= lds_deposit.getitemstring( ll_index, "deptaccount_no" )
	ldc_deptamt		= lds_deposit.getitemdecimal( ll_index, "deptmonth_amt" )

	ls_keeptype		= "D"+ls_deptgroup
	
	// ปรับปรุงดึงข้อมูลที่ต้องการจาก buffer datastore
//	is_kpfind		= " coop_id = '"+is_coopid+"' and recv_period = '"+is_recvperiod+"' and member_no = '"+ls_memno+"' and memcoop_id = '"+ls_memcoop+"' and refmemcoop_id = '"+ls_refmemcoop+"' and ref_membno = '"+ls_refmem+"' "
	is_kpfind		= " coop_id = '"+is_coopid+"' and recv_period = '"+is_recvperiod+"' and member_no = '"+ls_memno+"' and memcoop_id = '"+ls_memcoop+"' "
	il_kpfind		= ids_kptp.find( is_kpfind , il_kpfind , il_kpcount )
	// wa เพิ่ม แกไข้ กรณีมี error 
	if il_kpfind <= 0 then 
		il_kpfind		= ids_kptp.find( is_kpfind , 1 , il_kpcount )
		
	end if
	try
		is_kpslipno	= ids_kptp.object.kpslip_no[il_kpfind]
		li_seqno		= ids_kptp.object.last_seq_no[il_kpfind] ; if isnull( li_seqno ) then li_seqno = 0
	catch( Exception lthw_error )
		destroy lds_deposit ; destroy ids_kptp
		
		lthw_error.text		= "ไม่สามารถดึงค่าใบรายการเรียกเก็บ (kpslip_no) -> of_processdeposit ได้ เลขสมาชิก "+ls_memno+" เลขสัญญา "+ ls_deptaccno
		
		throw lthw_error
	end try
	
	li_seqno ++
	ids_kptp.object.last_seq_no[il_kpfind]		= li_seqno
	//จบการปรับปรุง
	
	// insert ข้อมูลลง รายการเรียกเก็บ
	insert into kptempreceivedet
			( 	coop_id , 				memcoop_id,			refmemcoop_id,		kpslip_no,
				member_no,			recv_period,				ref_membno, 
			  	seq_no,					keepitemtype_code,	shrlontype_code, 
			  	description,				item_payment,			keepitem_status,		bfcontract_status ,
			  	posting_status ,		bfcontlaw_status)
	values( 	:is_coopid , 				:ls_memcoop,			:ls_refmemcoop,		:is_kpslipno,
				:ls_memno,				:is_recvperiod,			:ls_refmem,
			  	:li_seqno,				:ls_keeptype,			:ls_depttype, 
			  	:ls_deptaccno,			:ldc_deptamt,			1,							1 ,
			  	0 ,							1)
	using itr_sqlca ;

	if itr_sqlca.sqlcode = 0 then
		inv_progress.istr_progress.subprogress_text = string(ll_index) +". ทะเบียน "+ls_memno+" > "+ls_deptaccno+" > จำนวนเงิน "+string(ldc_deptamt,"#,##0.00")
	else
		if ii_msgerrflag = 1 then
			this.of_set_msgerr( 'ข้อมูลเงินฝาก( ไม่สามารถบันทึกรายการเงินฝากได้ ) ทะเบียนสมาชิก : ' + ls_memno + ' / เลขที่บัญชีสมาชิก : ' + ls_deptaccno + ' / สาขาที่เปิดบัญชี : ' + ls_deptcoopid )
		else
			ithw_exception.text = "ประมวลผลเงินฝาก พบข้อผิดพลาด"
			ithw_exception.text += "~r~n( ไม่สามารถบันทึกรายการเงินฝากได้ )"
			ithw_exception.text += "~r~nทะเบียน : " + ls_memno
			ithw_exception.text += "~r~nสาขาบัญชีเงินฝาก : " + ls_deptcoopid
			ithw_exception.text += "~r~nเลขที่บัญชีเงินฝาก : " + ls_deptaccno
			ithw_exception.text += "~r~n" + string( itr_sqlca.sqlcode ) 
			ithw_exception.text += "~r~n" + itr_sqlca.sqlerrtext 
			destroy lds_deposit ; destroy ids_kptp
			return -1
		end if
	end if

next

//// ตรวจว่ามีเงินฝากแบบ Multi หรือเปล่า
//select	count( member_no )
//into		:ll_count
//from		dpdeptmonthly
//where	( dept_amt > 0 ) using itr_sqlca;
//
//lb_multi		= false
//
//lds_deposit	= create n_ds
//lds_deposit.dataobject	= "d_kp_rcvproc_deposit"
//lds_deposit.settransobject( itr_sqlca )
//
//// สร้างชุด SQL สำหรับการดึงรายการ
//li_ret = this.of_setsqlselect( lds_deposit )
//if ( li_ret <> 1 ) then
//	destroy lds_deposit
//	ithw_exception.text += "~nพบข้อผิดพลาดในการสร้างชุด SQL รายการเงินฝาก"
//	throw ithw_exception	
//end if
//
//ll_count = lds_deposit.retrieve()
//inv_progress.istr_progress.subprogress_max = ll_count
//
//for ll_index = 1 to ll_count
//
//	inv_progress.istr_progress.subprogress_index = ll_index
//
//	// ตรวจสอบการสั่งหยุดทำงาน
//	yield()
//	if inv_progress.of_is_stop() then
//		destroy lds_deposit
//		return 0
//	end if
//
//	ls_memno		= trim( lds_deposit.getitemstring( ll_index, "member_no" ) )
//	ls_membgroup	= trim( lds_deposit.getitemstring( ll_index, "membgroup_code" ) )
//	ls_depttype		= trim( lds_deposit.getitemstring( ll_index, "depttype_code" ) )
//	ls_deptgroup	= trim( lds_deposit.getitemstring( ll_index, "deptgroup_code" ) )
//	ls_deptaccno	= lds_deposit.getitemstring( ll_index, "deptaccount_no" )
//	ldc_deptamt		= lds_deposit.getitemdecimal( ll_index, "deptmonth_amt" )
//	ls_subgrp		= lds_deposit.getitemstring( ll_index, "subgroup_code" )
//
//	// มีการผ่อนผันการชำระหรือไม่	
//	li_compoundkeep	= ids_loandata.getitemnumber( ll_index, "compoundkeep_status" )
//	if ( li_compoundkeep = 1 ) then		
//		ls_membgroup = trim( ids_loandata.getitemstring( ll_index, "compoundkeep_group" ) )
//	end if
//
//	if isnull( ldc_deptamt ) then ldc_deptamt = 0
//
//	// ตรวจสอบว่าเป็นการทำรายการของสมาชิกคนใหม่หรือไม่
//	if ls_memno <> ls_memprior then
//		// หาค่าลำดับที่ในรายการ
//		select	max( kptempreceivedet.seq_no )
//		into		:li_seqno
//		from	kptempreceive, kptempreceivedet
//		where( kptempreceive.member_no	= kptempreceivedet.member_no ) and
//				( kptempreceive.recv_period	= kptempreceivedet.recv_period ) and
//				( kptempreceive.ref_membno	= kptempreceivedet.ref_membno ) and
//				( kptempreceive.member_no	= :ls_memno ) and
//				( kptempreceive.recv_period	= :is_recvperiod ) and
//				( kptempreceive.ref_membno	= :ls_memno ) using itr_sqlca;
//
//		// ถ้าลำดับที่เป็นค่า null แสดงว่ายังไม่มี master
//		if isnull(li_seqno) or ( li_seqno = 0 ) then
//
//			ldc_shrbegin	= this.of_getsharebegin( ls_memno )
//			ldc_shrstk		= this.of_getsharestockkeep( ls_memno )
//			insert into kptempreceive
//					( member_no, recv_period, ref_membno, membgroup_code, receipt_date, sharestkbf_value, sharestk_value, keeping_status, moneytype_code, tofrom_accid, subgroup_code )
//			values	( :ls_memno, :is_recvperiod, :ls_memno, :ls_membgroup, :idtm_receipt, :ldc_shrbegin, :ldc_shrstk, 1, :ls_moneytype, :ls_accid, :ls_subgrp )  using itr_sqlca;
//
//			if itr_sqlca.sqlcode <> 0 then
//				ithw_exception.text = "ประมวลผลเงินฝาก พบข้อผิดพลาด~n" + string( itr_sqlca.sqlcode ) + "  " + itr_sqlca.sqlerrtext + "~n" + ithw_exception.text
//				destroy lds_deposit
//				throw ithw_exception
//			end if
//			li_seqno	= 0
//		end if
//		ls_memprior	= ls_memno
//	end if
//
//	// กำหนดค่าต่าง ๆ
//	if lb_multi then
//		ls_keeptype	= "D"+ls_depttype
//	else
//		ls_keeptype	= "D"+ls_deptgroup
//	end if
//
//	li_seqno ++
//
//	// insert ข้อมูลลง รายการเรียกเก็บ
//	insert into kptempreceivedet
//			( member_no,			recv_period,				ref_membno, 
//			  seq_no,					keepitemtype_code,	shrlontype_code, 
//			  description,			item_payment,			keepitem_status,
//			  posting_status )
//	values( :ls_memno,		:is_recvperiod,				:ls_memno,
//			  :li_seqno,				:ls_keeptype,			:ls_depttype, 
//			  :ls_deptaccno,		:ldc_deptamt,			1,
//			  0 )
//	using itr_sqlca ;
//
//	if itr_sqlca.sqlcode <> 0 then
//		
//		ithw_exception.text = "ประมวลผลเงินฝาก พบข้อผิดพลาด~n" + string( itr_sqlca.sqlcode ) + "  " + itr_sqlca.sqlerrtext + "~n" + ithw_exception.text
//		destroy lds_deposit
//		throw ithw_exception
//end if
//	inv_progress.istr_progress.subprogress_text = string(ll_index) +". ทะเบียน "+ls_memno+" > "+ls_deptaccno+" > จำนวนเงิน "+string(ldc_deptamt,"#,##0.00")
//next
//
destroy lds_deposit

return 1
end function

protected function integer of_processffee () throws Exception;/***********************************************************
<description>
	ประมวลค่าธรรมเนียมแรกเข้า
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
string		ls_memno, ls_membgroup, ls_desc, ls_keeptype, ls_temp, ls_sqlext
string		ls_memcoop , ls_refmem , ls_refmemcoop
string		ls_moneytype, ls_accid, ls_subgrp
string		ls_sqlffee
long		ll_index, ll_count
integer		li_seqno, li_chk, li_stepvalue,li_keepperiod, li_ret, li_compoundkeep
dec{2}		ldc_feeamt, ldc_shrstk, ldc_shrbegin

str_extfn_keep lstr_extfn_keep

n_ds	lds_ffee

inv_progress.istr_progress.progress_text		= "ค่าธรรมเนียมแรกเข้า"
inv_progress.istr_progress.subprogress_text	=  "กำลังดึงข้อมูล"
inv_progress.istr_progress.subprogress_index	= 0
inv_progress.istr_progress.subprogress_max	= 0

lds_ffee	= create n_ds
lds_ffee.settransobject( itr_sqlca )

lstr_extfn_keep.function_name	= "of_kp_proc_getsql_ffee"
this.of_extfn( lstr_extfn_keep )
ls_sqlffee	= lstr_extfn_keep.function_return

inv_dwsrv.of_create_dw( lds_ffee , ls_sqlffee , '' )

// สร้างชุด SQL สำหรับการดึงรายการ
li_ret = this.of_setsqlselect( lds_ffee )
if ( li_ret <> 1 ) then
	destroy lds_ffee 
	ithw_exception.text += "~nพบข้อผิดพลาดในการสร้างชุด SQL รายการค่าธรรมเนียมแรกเข้า"
	return -1
end if

ll_count	= lds_ffee.retrieve()
lds_ffee.setsort( " coop_id , member_no , member_ref desc " )
lds_ffee.sort()

inv_progress.istr_progress.subprogress_max	= ll_count

for ll_index = 1 to ll_count
	inv_progress.istr_progress.subprogress_index	= ll_index

	// ตรวจสอบการสั่งหยุดทำงาน
	yield()
	if inv_progress.of_is_stop() then
		destroy lds_ffee ; destroy ids_kptp
		return 0
	end if
	
	ls_memno		= trim( lds_ffee.getitemstring( ll_index, "member_no" ) )
	ls_memcoop 	= is_coopcontrol
	ls_refmem 		= lds_ffee.object.member_ref[1]
	ls_refmemcoop	= is_coopcontrol
	ldc_feeamt		= lds_ffee.getitemdecimal( ll_index, "first_fee" )
	
	if isnull( ldc_feeamt ) then ldc_feeamt = 0
	
	if ldc_feeamt = 0 then 
		this.of_set_msgerr( 'ไม่พบข้อมูลค่าธรรมเนียมแรกเข้า ทะเบียน : ' + ls_memno + ' ค่าธรรมเนียม 0.00 บาท ' )
		continue ;
	end if
	
	// กำหนดค่าต่าง ๆ
	ls_keeptype	= "FFE"
	ls_desc		= "ค่าธรรมเนียมแรกเข้า"
	
	// ปรับปรุงดึงข้อมูลที่ต้องการจาก buffer datastore
	is_kpfind		= " coop_id = '"+is_coopid+"' and recv_period = '"+is_recvperiod+"' and member_no = '"+ls_memno+"' and memcoop_id = '"+ls_memcoop+"' "
	il_kpfind		= ids_kptp.find( is_kpfind , il_kpfind , il_kpcount )

	try
		is_kpslipno	= ids_kptp.object.kpslip_no[il_kpfind]
		li_seqno		= ids_kptp.object.last_seq_no[il_kpfind] ; if isnull( li_seqno ) then li_seqno = 0
	catch( Exception lthw_error )
		destroy lds_ffee ; destroy ids_kptp
		
		lthw_error.text		= "ไม่สามารถดึงค่าใบรายการเรียกเก็บ (kpslip_no) -> of_processffee ได้ เลขสมาชิก "+ls_memno
		
		throw lthw_error
	end try
	
	li_seqno ++
	ids_kptp.object.last_seq_no[il_kpfind]		= li_seqno
	//จบการปรับปรุง

	inv_progress.istr_progress.subprogress_text	= string(ll_index) +". ทะเบียน "+ls_memno+" > ค่าธรรมเนียมแรกเข้า " + string( ldc_feeamt,"#,##0.00")
	
	// insert ข้อมูลลง รายการเรียกเก็บ
	insert into kptempreceivedet
			( 	coop_id , 					memcoop_id,			refmemcoop_id,		kpslip_no,				shrlontype_code ,
				member_no,				recv_period,				ref_membno,			seq_no, 
			 	keepitemtype_code,		description,				item_payment,			posting_status,			keepitem_status ,
				bfcontract_status ,		bfcontlaw_status)
	values( 	:is_coopid , 					:ls_memcoop,			:ls_refmemcoop,		:is_kpslipno,				'00' ,
				:ls_memno,					:is_recvperiod,			:ls_memno,				:li_seqno,
			 	:ls_keeptype,				:ls_desc,					:ldc_feeamt,			0,							1 ,
				1 ,								1 )
	using itr_sqlca;
	
	if itr_sqlca.sqlcode = 0 then
		inv_progress.istr_progress.subprogress_text = string(ll_index) +". ทะเบียน "+ls_memno+" > ค่าธรรมเนียมแรกเข้า "+string(ldc_feeamt,"#,##0.00")
	else
		if ii_msgerrflag = 1 then
			this.of_set_msgerr( "ค่าธรรมเนียมแรกเข้า ทะเบียน : " + ls_memno + " > " + string( itr_sqlca.sqlcode ) + " < " + itr_sqlca.sqlerrtext )
		else
			ithw_exception.text = "ประมวลผลค่าธรรมเนียมแรกเข้า พบข้อผิดพลาด"  
			ithw_exception.text += "~r~nทะเบียน : "+ ls_memno
			ithw_exception.text += "~r~n"+ string( itr_sqlca.sqlcode )
			ithw_exception.text += "~r~n"+ itr_sqlca.sqlerrtext
			destroy lds_ffee ; destroy ids_kptp
			return -1
		end if
	end if
next

destroy lds_ffee

return 1
end function

protected function integer of_processloan () throws Exception;/***********************************************************
<description>
	ประมวลเรียกเก็บสัญญาเงินกู้
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
//---------------------
// Rkeep	คือ จำนวนเงินที่เรียกเก็บจริงในงวดนั้น
// Nkeep	คืน จำนวนด/บหรือต้นที่คำนวณได้ประจำงวดนั้น (ไม่รวมพวกค้าง)
//---------------------
string ls_sqlext , ls_sqlloan , ls_temp
string ls_contno , ls_memno , ls_loantype , ls_loangroup , ls_keeptype
string	ls_memcoop , ls_refmem , ls_refmemcoop , ls_statusdesc , ls_loancoopid
string ls_startkeep , ls_desc , ls_loanitemcode
string ls_rdtyp , ls_satangtyp
integer li_splitrecp , li_fixpayfsttype , li_fixpayfstprntype , li_fixpaynxttype , li_fixpaynxtprntype
integer li_year , li_month , li_ret , li_seqno
integer li_loanpaytype , li_contlaw , li_insureflag , li_trnrcvflag , li_contstatus , li_paystatus , li_contracttype
integer li_lastperiod , li_bfperiod , li_pauseflag , li_atmflag //tuk11092015
integer li_fixpaytype , li_fixpayprntype
integer li_compstatus , li_comppaytype , li_comppaystatus
integer li_trnposamt , li_rdposamt , li_kpitemarrdouble_flag , li_installment , li_showflag
dec{2} ldc_periodamt , ldc_intperiodamt , ldc_intaccum , ldc_prinbalance , ldc_trnintarrbal , ldc_itembalance
dec{2} ldc_bfintarrear , ldc_bfintmtharr , ldc_bfintyeararr
dec{2} ldc_compamt , ldc_intperiod , ldc_intarrear , ldc_intarrpay , ldc_intmthperiod
dec{2} ldc_principal , ldc_itempay , ldc_totalintaccum
dec{2} ldc_rkeepint , ldc_rkeepprin
dec{2} ldc_fwarrear, ldc_bfintreturn , ldc_intreturn , ldc_fwreturn , ldc_intnet , ldc_intreturnpay , ldc_intperiodpay , ldc_principalpay //tuk1 ประกาศตัวแปรเพิ่ม
dec{4} ldc_interest , ldc_periodmax , ldc_tpperiodmax , ldc_perinstallment //tuk11092015
long ll_count , ll_index
datetime ldtm_procdate , ldtm_lastcalint , ldtm_startkeep , ldtm_lastpay , ldtm_lastproc , ldtm_priorproc , ldtm_slipdate //tukATM05102015
boolean lb_showonly

this.of_setsrvrdmoney( true )

str_extfn_keep lstr_extfn_keep

n_cst_calendarservice	lnv_calendarsrv

li_showflag		= ids_kpconstant.object.showsl_flag[1]

lnv_calendarsrv= create n_cst_calendarservice
lnv_calendarsrv.of_initservice( inv_transection )

inv_progress.istr_progress.progress_text		= "หนี้เงินกู้"
inv_progress.istr_progress.subprogress_text	= "กำลังดึงข้อมูล"
inv_progress.istr_progress.subprogress_index	= 0
inv_progress.istr_progress.subprogress_max	= 0

//set การปัดดอกเบี้ย
// set ค่าการปัดเศษ
inv_rdmoneysrv.of_set_constant( is_coopid , "KEP" , "procloan" )
ls_rdtyp			= inv_rdmoneysrv.of_get_roundtype(  )
ls_satangtyp		= inv_rdmoneysrv.of_get_satangtype(  )
li_trnposamt		= inv_rdmoneysrv.of_get_truncate_pos(  )
li_rdposamt		= inv_rdmoneysrv.of_get_round_pos(  )

//inv_intsrv.of_setrdsatangtype(4) // การปัดเศษสตางค์ : 1(ปัดขึ้นทีละสลึง) , 2(ปัดขึ้นทีละ 5 สตางค์) , 3(ปัดขึ้นทีละ 10 สตางค์) , 4(ปัดเต็มบาท) , 99( ดูจากตารางการปัด ) ปัดดอกเบี้ยครั้งสุดท้าย
//inv_intsrv.of_setrddecdigit(li_trnposamt) // ตำแหน่งการปัดทศนิยม
//inv_intsrv.of_setrddecformat(1) // การปัดทศนิยม : 1(ปัด5/4) , 2(ปัดทิ้ง) , 3(ปัดขึ้น) , 0(ไม่ทราบ)

// hardcode
integer li_rdsatang , li_rddecdigit , li_rddecfor
if is_coopid = "001001" then
	//Mahidon
	li_rdsatang 		= 4
	li_rddecdigit 	= 2
	li_rddecfor		= 1
end if
if is_coopid = "005001" then
	//Surin
	li_rdsatang 		= 1
	li_rddecdigit 	= 2
	li_rddecfor		= 1
end if

if is_coopid = "006001" then
	li_rdsatang 		= 0
	li_rddecdigit 	= 2
	li_rddecfor		= 5
end if

if is_coopid = "007001" then
	li_rdsatang 		= 0
	li_rddecdigit 	= 7
	li_rddecfor		= 1
end if

if is_coopid = "001001" or is_coopid = "005001" or is_coopid = "006001" or  is_coopid = "007001" then
	inv_intsrv.of_setrdsatangtype(li_rdsatang) // การปัดเศษสตางค์ : 1(ปัดขึ้นทีละสลึง) , 2(ปัดขึ้นทีละ 5 สตางค์) , 3(ปัดขึ้นทีละ 10 สตางค์) , 4(ปัดเต็มบาท) , 99( ดูจากตารางการปัด ) ปัดดอกเบี้ยครั้งสุดท้าย
	inv_intsrv.of_setrddecdigit(li_rddecdigit) // ตำแหน่งการปัดทศนิยม
	inv_intsrv.of_setrddecformat(li_rddecfor) // การปัดทศนิยม : 1(ปัด5/4) , 2(ปัดทิ้ง) , 3(ปัดขึ้น) , 0(ไม่ทราบ)
end if
// hardcode

// ตรวจสอบการกำหนดค่าระบบเงินกู้
select		receiptsplit_flag, 		fixpayintoverfst_type, 		fixpayintoverfstprn_type,
			fixpayintovernxt_type, fixpayintovernxtprn_type, kpitemarrdouble_flag
into		:li_splitrecp, 			:li_fixpayfsttype, 			:li_fixpayfstprntype,
			:li_fixpaynxttype, 		:li_fixpaynxtprntype, :li_kpitemarrdouble_flag
from		lnloanconstant
using 		itr_sqlca;

if isnull(li_splitrecp) then li_splitrecp = 0
if isnull(li_fixpayfsttype) then li_fixpayfsttype = 0
if isnull(li_fixpayfstprntype) then li_fixpayfstprntype = 0
if isnull(li_fixpaynxttype) then li_fixpaynxttype = 0
if isnull(li_fixpaynxtprntype) then li_fixpaynxtprntype = 0
if isnull(li_kpitemarrdouble_flag) then li_kpitemarrdouble_flag = 0
//0 ไม่รวมดอกเบี้ยค้างรับ
//1 รวมดอกเบี้ยค้างรับทบดอกเบี้ยส่งหัก
//2 รวมดอกเบี้ยค้างรับแยกยอด(แยกรายการเรียกเก็บ)

li_year	= integer( left( is_recvperiod, 4 ) )
li_month	= integer( mid( is_recvperiod, 5, 2 ) )

ldtm_procdate	= lnv_calendarsrv.of_getprocessdate( li_year, li_month )
ldtm_priorproc	= lnv_calendarsrv.of_getpostingdate( li_year, li_month )
//ldtm_priorproc = datetime( relativedate( date( ldtm_priorproc ), 1 ) )

if isnull( idtm_startcont ) then idtm_startcont = ldtm_procdate

ids_loandata	= create n_ds
ids_loandata.settransobject( itr_sqlca )

lstr_extfn_keep.function_name	= 'of_kp_proc_getsql_loan' //tuk2 อย่าลืมไป select interest_return เพิ่มที่ sql ด้วยของ Function Name : of_extfn( 'of_kp_proc_getsql_loan')
this.of_extfn( lstr_extfn_keep )
ls_sqlloan	= lstr_extfn_keep.function_return

inv_dwsrv.of_create_dw( ids_loandata , ls_sqlloan , '' )

// สร้างชุด SQL สำหรับการดึงรายการ
li_ret = this.of_setsqlselect( ids_loandata )
if ( li_ret <> 1 ) then
	destroy ids_loandata ; destroy ids_kptp ; destroy lnv_calendarsrv
	ithw_exception.text += "~nพบข้อผิดพลาดในการสร้างชุด SQL รายการหนี้เงินกู้"
	throw ithw_exception	
end if

ls_temp	= ids_loandata.getsqlselect()
//ls_sqlext = " and ( lncontmaster.startcont_date <= to_date( '" + string( ldtm_procdate, "yyyymmdd" ) + "', 'yyyymmdd' ) )"
ls_sqlext = " and ( lncontmaster.startcont_date <= to_date( '" + string( idtm_startcont, "yyyymmdd" ) + "', 'yyyymmdd' ) )"
ls_temp	= ls_temp + ls_sqlext

ids_loandata.setsqlselect( ls_temp )
ids_loandata.settransobject( itr_sqlca )

ll_count	= ids_loandata.retrieve()

ids_loandata.setsort( ' member_no , member_ref desc , loancontract_no ' )
ids_loandata.sort()

inv_progress.istr_progress.subprogress_max	= ll_count

for ll_index = 1 to ll_count
	inv_progress.istr_progress.subprogress_index	= ll_index
	
	// ตรวจสอบการสั่งหยุดทำงาน
	yield()
	if inv_progress.of_is_stop() then
		destroy ids_loandata ; destroy ids_kptp ; destroy lnv_calendarsrv
		return 0
	end if

// 	ls_contno ห้ามใส่ trim เพราะเด๋วจะ join ไม่เจอกัน
	ls_contno			= ids_loandata.getitemstring( ll_index, "loancontract_no" )
	ls_memno			= trim( ids_loandata.getitemstring( ll_index, "member_no" ) )
	ls_memcoop		= trim( ids_loandata.getitemstring( ll_index, "memcoop_id" ) )
	ls_refmem			= trim( ids_loandata.getitemstring( ll_index, "member_ref" ) )
	ls_refmemcoop		= is_coopcontrol
	ls_loancoopid		= trim( ids_loandata.getitemstring( ll_index, "coop_id" ))
	ls_loantype			= trim( ids_loandata.getitemstring( ll_index, "loantype_code" ) )
	ls_loangroup		= trim( ids_loandata.getitemstring( ll_index, "loangroup_code" ) )
	ls_startkeep			= trim( ids_loandata.getitemstring( ll_index, "startkeep_period" ) )
	li_loanpaytype		= ids_loandata.getitemnumber( ll_index, "loanpayment_type" )		
	li_contlaw			= ids_loandata.getitemnumber( ll_index, "contlaw_status" )
	li_insureflag			= ids_loandata.getitemnumber( ll_index, "insurecoll_flag" )
	li_trnrcvflag			= ids_loandata.getitemnumber( ll_index, "transfer_status" )
	li_contstatus			= ids_loandata.getitemnumber( ll_index, "contract_status" )
	li_paystatus			= ids_loandata.getitemnumber( ll_index, "payment_status" )
	li_lastperiod			= ids_loandata.getitemnumber( ll_index, "last_periodpay" )
	li_bfperiod			= ids_loandata.getitemnumber( ll_index, "last_periodpay" )
	li_comppaystatus	= ids_loandata.getitemnumber( ll_index, "compound_paystatus"  )		// สถานะการส่ง  งดต้น(-11), งดทั้งหมด(-1)
	li_comppaytype		= ids_loandata.getitemnumber( ll_index, "compound_paytype"  )			// การชำระ คงต้น คงยอด
	li_compstatus		= ids_loandata.getitemnumber( ll_index, "compound_status"  )				// 1 ผ่อนผัน 0 ไม่ผ่อน
	li_contracttype		= ids_loandata.getitemnumber( ll_index, "contract_type" )
	li_installment		= ids_loandata.getitemnumber( ll_index, "period_payamt" )
	li_atmflag          = ids_loandata.getitemnumber( ll_index, "atm_flag"  )		            //tuk11092015
    ldc_perinstallment	= ids_loandata.getitemnumber( ll_index, "period_installment" )          //tuk11092015
	ldc_compamt		    = ids_loandata.getitemdecimal( ll_index, "compound_payment"  )			// ชำระต่องวด
	ldc_periodamt		= ids_loandata.getitemdecimal( ll_index, "period_payment" )
	ldc_periodmax		= ids_loandata.getitemdecimal( ll_index, "period_payment_max" )
	ldc_intperiodamt	= ids_loandata.getitemdecimal( ll_index, "period_payintarr" )
	ldc_bfintarrear		= ids_loandata.getitemdecimal( ll_index, "interest_arrear" )
	ldc_bfintreturn		= ids_loandata.getitemdecimal( ll_index, "interest_return" ) //tuk3 มีตัวแปรรับค่า interest_return
	ldc_bfintmtharr		= ids_loandata.getitemdecimal( ll_index, "intmonth_arrear" )
	ldc_bfintyeararr	= ids_loandata.getitemdecimal( ll_index, "intyear_arrear" )
	ldc_intaccum		= ids_loandata.getitemdecimal( ll_index, "accum_interest" )
	ldc_prinbalance		= ids_loandata.getitemdecimal( ll_index, "principal_balance" )
	ldc_trnintarrbal		= ids_loandata.getitemdecimal( ll_index, "trnfrom_intbal" )
	ldtm_lastcalint		= ids_loandata.getitemdatetime( ll_index, "lastcalint_date" )
	ldtm_startkeep		= ids_loandata.getitemdatetime( ll_index, "startcont_date" )
	ldtm_lastpay		= ids_loandata.getitemdatetime( ll_index, "lastpayment_date" )
	ldtm_lastproc		= ids_loandata.getitemdatetime( ll_index, "lastprocess_date" )

	lb_showonly			= false
	
	if isnull( ldc_compamt ) then ldc_compamt = 0
	if isnull( li_compstatus ) then li_compstatus = 0
	if isnull( li_comppaytype ) then li_comppaytype = 0
	if isnull( li_comppaystatus ) then li_comppaystatus = 0

	// ผ่อนผันหรือไม่
	if ( li_compstatus = 1 ) then
		// งดหรือไม่
		if ( li_comppaystatus = -1 ) then
			if li_showflag = 1 then 
				lb_showonly	= true
			else
				continue ;
			end if
		else
			li_paystatus		= li_comppaystatus
			li_loanpaytype	= li_comppaytype
			ldc_periodamt	= ldc_compamt
		end if
	end if

	if isnull( li_lastperiod ) then li_lastperiod = 0
	if isnull( ldc_periodamt ) then ldc_periodamt = 0
	if isnull( ldc_periodmax ) then ldc_periodmax = 0
	if isnull( ldc_bfintarrear ) then ldc_bfintarrear = 0
	if isnull( ldc_bfintreturn ) then ldc_bfintreturn = 0 //tuk4 ให้ค่าตัวแปร ldc_bfintreturn (ดอกเบี้ยคืนยกมาเพื่อมาเก็บไว้ที่ kptempreceivedet)
	if isnull( ldc_bfintmtharr ) then ldc_bfintmtharr = 0
	if isnull( ldc_intaccum ) then ldc_intaccum = 0
	if isnull( ldc_prinbalance ) then ldc_prinbalance = 0
	
	//การคำนวณส่งต่องวดใหม่(period_payment) ของ ATM : MHS << //tuk11092015
	if is_coopcontrol = '003001'  and li_atmflag = 1 then
		
		//tuk05102015
		select LOANITEMTYPE_CODE
	    into :ls_loanitemcode
	    from LNCONTSTATEMENT
        where LOANCONTRACT_NO = :ls_contno
		and SEQ_NO = (select max(seq_no) from LNCONTSTATEMENT where LOANCONTRACT_NO = :ls_contno ) using itr_sqlca ;
		
		if trim(ls_loanitemcode) <> 'LPM' then
			ldc_periodamt = (ldc_prinbalance) / ldc_perinstallment
			
			if mod(ldc_periodamt,10) = 0 then
				ldc_periodamt = ldc_periodamt
			else
				ldc_periodamt = ldc_periodamt+( 10 - mod(ldc_periodamt,10))
		   end if
		
		   if ldc_periodamt < 500 then
			ldc_periodamt = 500
		   end if	
		
		   // update ข้อมูล period_payment ของ ATM : MHS
		   update	lncontmaster
		   set		PERIOD_PAYMENT	= :ldc_periodamt
		   where	coop_id				= :ls_loancoopid
		   and		( loancontract_no	= :ls_contno ) 
		   using itr_sqlca;
	
			if itr_sqlca.sqlcode <> 0 then
				if ii_msgerrflag = 1 then
					this.of_set_msgerr( 'ข้อมูลหนี้เงินกู้( ไม่สามารถบันทึกรายการส่งต่องวดได้ ) ทะเบียนสมาชิก : ' + ls_memno + ' / เลขที่สัญญา : ' + ls_contno + ' / ต้นเงิน : ' + string( ldc_rkeepprin , '#,##0.00') + ' / ดอกเบี้ย : ' + string( ldc_rkeepint , '#,##0.00') )
				else
					ithw_exception.text = "ประมวลผลเงินกู้ พบข้อผิดพลาด~n" + string( itr_sqlca.sqlcode ) + "  " + itr_sqlca.sqlerrtext + "~n" + ithw_exception.text
					destroy ids_loandata
					throw ithw_exception
				end if
			end if
	    end if
		
	end if	
    //จบการคำนวณส่งต่องวดใหม่(period_payment) ของ ATM : MHS >>//tuk11092015

	// ตรวจว่ามีการงดเก็บไว้หรือไม่
	if li_paystatus = -1 or li_paystatus = -13 then
		if li_showflag = 1 then 
			lb_showonly = true
		else
			continue ;
		end if
	end if

//	// ถ้าไม่ใช่สัญญาที่รับโอนก็คือสัญญาตัวเอง
//	if isnull( ls_refmemno ) or ls_refmemno = "" or ( li_splitrecp = 0 ) then
//		ls_refmemno = ls_memno
//	end if

//	// ตรวจสอบว่าสัญญาได้เวลาเก็บหรือยัง
//	if date( ldtm_startkeep ) > date( ldtm_procdate ) then
//		this.of_set_msgerr( 'ข้อมูลหนี้เงินกู้( ไม่พร้อมเก็บ ) ทะเบียนสมาชิก : ' + ls_memno + ' / เลขที่สัญญา : ' + ls_contno + ' < เริ่มเก็บวันที่ ' + string( ldtm_startkeep , 'dd/mm/' ) + string( year( date( ldtm_startkeep )) + 543 ) + ' > ' )
//		continue  ;//go to next
//	end if

	// ตรวจสอบว่าสัญญาได้เวลาเก็บหรือยัง โอ้ เอาออก เพราะคนส่วนใหญ่ไม่ตรวจปฏิทิน ทำให้ startkeep_period ผิด ถ้าเช็คจะไม่โดนเรียกเก็บ
//	if ls_startkeep > is_recvperiod then
//		this.of_set_msgerr( 'ข้อมูลหนี้เงินกู้( ไม่พร้อมเก็บ ) ทะเบียนสมาชิก : ' + ls_memno + ' / เลขที่สัญญา : ' + ls_contno + ' < เริ่มเก็บงวดที่ ' + ls_startkeep )
//		continue ;
//	end if

	if li_loanpaytype = 0 then
		this.of_set_msgerr( "ข้อมูลหนี้เงินกู้( ไม่เรียกเก็บ ) ทะเบียนสมาชิก : " + ls_memno + " / เลขที่สัญญา : " + ls_contno )
		continue ;
	end if

	/* รอแก้ไข ตรวจสอบหนี้ตามประเภท lnloantype.loanpay_type
	// ตรวจสอบงวดส่งสูงสุด *****
	if ldc_periodmax > 0 or ldc_periodamt = 0 then
		this.of_set_msgerr( 'ข้อมูลหนี้เงินกู้( ยอดส่งหักเป็นศูนย์ ) ทะเบียนสมาชิก : ' + ls_memno + ' / เลขที่สัญญา : ' + ls_contno )
		continue ;
	end if
	*/
	
	// ปรับปรุงดึงข้อมูลที่ต้องการจาก buffer datastore
//	is_kpfind		= " coop_id = '"+is_coopid+"' and recv_period = '"+is_recvperiod+"' and member_no = '"+ls_memno+"' and memcoop_id = '"+ls_memcoop+"' and refmemcoop_id = '"+ls_refmemcoop+"' and ref_membno = '"+ls_refmem+"' "
	is_kpfind		= " coop_id = '"+is_coopid+"' and recv_period = '"+is_recvperiod+"' and member_no = '"+ls_memno+"' and memcoop_id = '"+ls_memcoop+"' "
	il_kpfind		= ids_kptp.find( is_kpfind , il_kpfind , il_kpcount )
	
	// wa เพิ่ม แกไข้ กรณีมี error 
	if il_kpfind <= 0 then 
		il_kpfind		= ids_kptp.find( is_kpfind , 1 , il_kpcount )
		
	end if
	
	if il_kpfind <= 0 then
		il_kpfind = 0
	end if
	
	try
		is_kpslipno	= ids_kptp.object.kpslip_no[il_kpfind]
		li_seqno		= ids_kptp.object.last_seq_no[il_kpfind] ; if isnull( li_seqno ) then li_seqno = 0
	catch( Exception lthw_error )
		destroy ids_loandata
		destroy lnv_calendarsrv
		
		lthw_error.text		= "ไม่สามารถดึงค่าใบรายการเรียกเก็บ (kpslip_no) -> of_processloan ได้ เลขสมาชิก "+ls_memno+" เลขสัญญา "+ls_contno
		
		throw lthw_error
	end try
	
	//จบการปรับปรุง
	
	// ถ้าเป็นการงดให้แสดงแต่ยอดคงเหลืออย่างเดียว
	if lb_showonly and ( ldc_prinbalance > 0 ) then
		ls_desc		= "L" + ls_loangroup
		li_seqno ++
		// insert ข้อมูลลง รายการเรียกเก็บ
		insert into kptempreceivedet
				( 	coop_id , 				memcoop_id,			refmemcoop_id,		kpslip_no,
					member_no, 			recv_period, 			ref_membno, 			seq_no, 
					keepitemtype_code, 	shrlontype_code, 		bizzcoop_id,				loancontract_no, 		period, 
					principal_payment, 	interest_payment, 		intarrear_payment,	item_payment, 
					item_balance, 			keepitem_status, 		posting_status ,		description, 
					bfcontract_type)
		values( 	:is_coopid , 				:ls_memcoop,			:ls_refmemcoop,		:is_kpslipno,
					:ls_memno, 			:is_recvperiod, 			:ls_refmem, 			:li_seqno, 
					'DPL', 						:ls_loantype, 			:ls_loancoopid,			:ls_contno, 				:li_lastperiod, 
					0, 							0, 							0,				  			0, 
					:ldc_prinbalance, 		1, 							0 ,							:ls_desc,
					:li_contracttype)
		using	itr_sqlca;
				  
		if itr_sqlca.sqlcode = 0 then
			inv_progress.istr_progress.subprogress_text = string(ll_index) +". ทะเบียน >"+ls_memno+" สัญญา >"+string(ls_contno)
		else
			if ii_msgerrflag = 1 then
				this.of_set_msgerr( 'ข้อมูลหนี้เงินกู้( ไม่สามารถบันทึกรายการแสดงยอดคงเหลือได้ ) ทะเบียนสมาชิก : ' + ls_memno + ' / เลขที่สัญญา : ' + ls_contno )
			else
				ithw_exception.text = "ประมวลผลเงินกู้ พบข้อผิดพลาด"
				ithw_exception.text += "~r~n( ไม่สามารถบันทึกรายการแสดงยอดคงเหลือได้ )"
				ithw_exception.text += "~r~nทะเบียน : " + ls_memno
				ithw_exception.text += "~r~nสาขาสัญญาเงินกู้ : " + ls_loancoopid
				ithw_exception.text += "~r~nเลขที่สัญญาเงินกู้ : " + ls_contno
				ithw_exception.text += "~r~n" + string( itr_sqlca.sqlcode ) 
				ithw_exception.text += "~r~n" + itr_sqlca.sqlerrtext 
				destroy ids_loandata ; destroy ids_kptp ; destroy lnv_calendarsrv
				return -1
			end if
		end if
		
		ids_kptp.object.last_seq_no[il_kpfind]		= li_seqno
		continue ;
	end if	
	
//tukATM05102015<<
 if li_atmflag = 1 then 	
	select a.SLIP_DATE
	into :ldtm_slipdate
	from LNCONTSTATEMENT a
    where a.LOANCONTRACT_NO = :ls_contno and a.LOANITEMTYPE_CODE = 'LPM'
    and a.SEQ_NO = (select max(b.seq_no) from LNCONTSTATEMENT b 
    where b.LOANCONTRACT_NO = :ls_contno and b.LOANITEMTYPE_CODE = 'LPM') using itr_sqlca ; 

    if itr_sqlca.sqlcode = 100  then
    else
	     ldtm_lastcalint = ldtm_slipdate
    end if

end if
//tukATM05102015>>>	

//	 คำนวณดอกเบี้ย
	ldc_intperiod		= 0
	
	try
		this.of_calcontinterest( ls_contno, ll_index, ldtm_lastcalint, idtm_calintto, ldc_intperiod, ldtm_priorproc )
	catch( Exception lthw_calint )
		rollback using itr_sqlca;
		throw lthw_calint
	end try

	ldc_interest		= ldc_intperiod 
	ldc_intarrear 	= ldc_bfintarrear
	
	ldc_intreturn    = ldc_bfintreturn  //tuk5 ให้ค่าตัวแปรสำหรับดอกเบี้ยคืน
	ldc_intnet        = ldc_intarrear +  (ldc_interest - ldc_intreturn) //tuk6 สูตรการคิดดอกเบี้ยสุทธิ = ดอกเบี้ยที่คำนวณได้ในงวดนั้น + (ดอกเบี้ยค้าง-ดอกเบี้ยคืน)

    //	ldc_intarrear 	= ldc_bfintarrear - ldc_bfintmtharr
	
	/*ดอกเบี้ยรวมดอกเบี้ยค้าง ต้องมี flag แยกการรวมดอกเบี้ยค้าง	*/
	if li_kpitemarrdouble_flag = 2 then
		ldc_rkeepint	= ldc_interest 
	else
		ldc_rkeepint	= ldc_interest + ldc_intarrear
	end if
	
	// ดอกเบี้ยค้างแยกรายการออกมาต่างหาก ไม่รวมไปกับยอดประจำเดือน
	/* ดอกเบี้ยรวมดอกเบี้ยค้าง ต้องมี flag แยกการรวมดอกเบี้ยค้าง */
	if ( ldc_intarrear > 0 and li_kpitemarrdouble_flag = 2) then
		li_seqno ++
		
		// ถ้ามีการผ่อนผันดอกเบี้ยค้าง
		if ( ldc_intperiodamt > 0 ) then
			ldc_intarrear = ldc_intperiodamt
		end if
		
		// insert ข้อมูลลง รายการเรียกเก็บ
		insert into kptempreceivedet
				( 	coop_id , 				memcoop_id,			refmemcoop_id,			kpslip_no,		
					member_no, 			recv_period, 			ref_membno, 				seq_no, 					keepitemtype_code, 			shrlontype_code, 				bizzcoop_id ,
					loancontract_no, 		period, 					principal_payment, 		interest_payment, 		intarrear_payment,		  	item_payment, 
					item_balance, 			calintfrom_date, 		calintto_date, 				principal_period, 		interest_period, 				bfprinbalance_amt, 
					bflastcalint_date, 		bflastpay_date,			bfinterest_arrear, 			bfintmonth_arrear, 	bfintyear_arrear, 				bfperiod, 						bfcontract_type ,
					bfcontract_status, 		bfcontlaw_status, 		keepitem_status, 			posting_status )
				  				  
		values( 	:is_coopid , 				:ls_memcoop,			:ls_refmemcoop,			:is_kpslipno,
					:ls_memno, 			:is_recvperiod, 			:ls_refmem, 				:li_seqno, 				'IAR', 								:ls_loantype, 					:ls_loancoopid,
					:ls_contno, 				0, 							0, 								:ldc_intarrear, 			:ldc_intarrpay,			  		:ldc_intarrear, 					
					:ldc_itembalance, 		:ldtm_lastcalint, 		:idtm_calintto, 				0, 							:ldc_intarrear, 					:ldc_prinbalance, 
					:ldtm_lastcalint, 		:ldtm_lastpay,		  	:ldc_bfintarrear, 			:ldc_bfintmtharr, 		:ldc_bfintyeararr, 				:li_bfperiod, 					:li_contracttype,
					:li_contstatus, 			:li_contlaw, 				1, 								0 )
		using itr_sqlca;
				  
		if itr_sqlca.sqlcode <> 0 then
			if ii_msgerrflag = 1 then
				this.of_set_msgerr( 'ข้อมูลหนี้เงินกู้( ไม่สามารถบันทึกรายการดอกเบี้ยค้างได้ ) ทะเบียนสมาชิก : ' + ls_memno + ' / เลขที่สัญญา : ' + ls_contno + ' / ดอกเบี้ยค้าง : ' + string( ldc_intarrear , '#,##0.00') )
			else
				ithw_exception.text = "ประมวลผลเงินกู้ พบข้อผิดพลาด"
				ithw_exception.text += "~r~n( ไม่สามารถบันทึกรายการดอกเบี้ยค้างได้ )"
				ithw_exception.text += "~r~nทะเบียน : " + ls_memno
				ithw_exception.text += "~r~nสาขาสัญญาเงินกู้ : " + ls_loancoopid
				ithw_exception.text += "~r~nเลขที่สัญญาเงินกู้ : " + ls_contno
				ithw_exception.text += "~r~n" + string( itr_sqlca.sqlcode ) 
				ithw_exception.text += "~r~n" + itr_sqlca.sqlerrtext 
				destroy ids_loandata ; destroy ids_kptp ; destroy lnv_calendarsrv
				return -1
			end if
		end if		
	end if
	
	if ldc_prinbalance = 0 then
		this.of_set_msgerr( 'ข้อมูลหนี้เงินกู้( ไม่มีหนี้คงเหลือ ) ทะเบียนสมาชิก : ' + ls_memno + ' / เลขที่สัญญา : ' + ls_contno )
		ids_kptp.object.last_seq_no[il_kpfind]		= li_seqno
		continue ;
	end if	

	// ตรวจสอบประเภทการส่ง
	/*
	loanpayment_type
	0 ไม่มีการเรียกเก็บ
	1 ต้นเท่ากัน+ดอก
	2 ต้น+ดอกเท่ากัน
	3 คิดแต่ดอกเบี้ย
	*/
	choose case li_loanpaytype
		case 0
			
		case 1	// คงต้น
		//tuk12 เอาออกแก้กรณีคงต้น <<
		//			ldc_principal		= ldc_periodamt
		//			ldc_interest		= ldc_intperiod + ldc_bfintarrear /* ดอกเบี้ยรวมดอกเบี้ยค้าง ต้องมี flag แยกการรวมดอกเบี้ยค้าง */
		//tuk12>>

			//tuk13 แก้ไขกรณีคงต้น <<
		     if ldc_intnet <= 0 then
				ldc_intnet = 0
				//ldc_bfintreturn = ldc_bfintreturn - (ldc_bfintarrear + ldc_interest)
				ldc_intreturnpay = ldc_bfintarrear + ldc_interest
			else
				ldc_intreturnpay = ldc_bfintreturn
			end if		
		     ldc_rkeepint = ldc_intnet
			ldc_intarrpay = ldc_bfintarrear
			ldc_intperiodpay = ldc_interest
			ldc_principalpay = ldc_periodamt
			//tuk13>>
			
		case 2	// คงยอด
			
			if li_lastperiod = 0 then
				li_fixpaytype		= li_fixpayfsttype
				li_fixpayprntype	= li_fixpayfstprntype
			else
				li_fixpaytype		= li_fixpaynxttype
				li_fixpayprntype	= li_fixpaynxtprntype
			end if
			
			// ถ้าวันที่คิดดอกเบี้ยล่าสุดน้อยกว่าวันที่ประมวลผลครั้งที่แล้วและไม่ลดเงินต้นลง
			if ( isnull( ldtm_lastproc ) or ldtm_lastproc < ldtm_priorproc or ldtm_lastcalint < ldtm_priorproc ) and li_fixpaytype = 1 then
				try
					this.of_calcontinterest( ls_contno, ll_index, ldtm_priorproc, idtm_calintto, ldc_intmthperiod, ldtm_priorproc )
				catch( Exception lthw_calinterest )
					rollback using itr_sqlca;
					throw lthw_calinterest
				end try
			else
				ldc_intmthperiod	= ldc_intperiod + ldc_bfintmtharr
			end if
			
//			ldc_principal	= ldc_periodamt - ldc_intmthperiod //tukout
//			// hardcode ***
//			ldc_principal	= ldc_periodamt - ( ldc_intmthperiod + ldc_intarrear )
//			// end hardcode ***

             ldc_fwarrear = 0  //tuk
			ldc_fwreturn = 0  //tuk
			
			//tuk9<< เอาออก
			/*if ldc_principal < 0 then
				if li_fixpaytype = 2 and li_fixpayprntype = 2 then
					ldc_principal	= abs( ldc_principal )
				else
					ldc_principal	= 0
				end if
			end if*/
			//tuk9>>
			
			//tuk10<<	
		if ldc_intnet > ldc_periodamt then
				ldc_rkeepint = ldc_periodamt
				ldc_intreturnpay = ldc_bfintreturn
				if (ldc_bfintarrear - ldc_bfintreturn)  > ldc_rkeepint then
					ldc_intperiod = 0
					ldc_intarrpay = ldc_rkeepint - ldc_intperiodpay + ldc_intreturnpay
				else  // ldc_bfintarrear-ldc_bfintreturn <= ldc_rkeepint then
					ldc_intarrpay = ldc_bfintarrear
					ldc_intperiodpay =  ldc_rkeepint - ldc_intarrpay + ldc_intreturnpay
				end if			
		else
			if ldc_intnet <= 0 then
				ldc_intnet = 0
			end if	
			ldc_rkeepint = ldc_intnet
			ldc_intarrpay = ldc_bfintarrear
			ldc_intperiodpay = ldc_interest
			ldc_intreturnpay = (ldc_intarrpay + ldc_intperiodpay) - ldc_rkeepint
		end if
			ldc_principalpay = ldc_periodamt - ldc_rkeepint
		//tuk10>>	
			
		case 3
//			ldc_principal		= 0
//			ldc_rkeepprin	= 0
			// เก็บแต่ดอก
			if li_lastperiod + 1 >= li_installment then
				ldc_principalpay	= ldc_prinbalance
			else
				ldc_principalpay	= 0
			end if
	end choose	

	// check ว่าเกินยอดคงเหลือหรือเปล่า
	if ldc_principalpay > ldc_prinbalance then ldc_principalpay = ldc_prinbalance
	
	if is_coopcontrol = '008001' then
		/*หนี้งวดสุดท้ายเก็บหมดไหม ควรแยกเป็น Constant*/
		if li_lastperiod + 1 >= li_installment and ls_loantype <> '88' then
			ldc_principalpay	= ldc_prinbalance
		end if
		/*คือไรไม่รู้*/
		if li_paystatus = -12 then //ชำระแต่ต้น
		/*//อันเก่าเอาออก 11-08-2015 <<
		//ldc_principalpay		= ldc_periodamt 
		//ldc_rkeepint		= 0  
		//อันเก่าเอาออก 11-08-2015 >> */
			 if ldc_principalpay >= ldc_prinbalance then 
					ldc_principalpay = ldc_prinbalance
			else
			         ldc_principalpay		= ldc_periodamt
		     end if
			ldc_rkeepint		= 0
		end if
	end if
	
	// ตรวจสอบว่างดต้นมั้ย
	if li_paystatus = -11 then
		ldc_principalpay= 0
		ldc_rkeepprin	= 0
	else
		ldc_rkeepprin	= ldc_principalpay
	end if

	// hardcode ***
	ldc_tpperiodmax = ldc_rkeepprin + ldc_rkeepint /*+ ldc_intarrear */
	if li_kpitemarrdouble_flag = 2 then
		ldc_periodmax -= ldc_intarrear
	end if
	/*flag ตรวจสอบยอดชำระสูงสุด -> ลดต้น / ลดดอกเบี้ย */
	if ldc_periodmax > 0 and ldc_tpperiodmax > ldc_periodmax then
		// ตรวจว่า ด/บ มากกว่ายอดสูงสุดหรือเปล่า
		if ldc_rkeepint >= ldc_periodmax then
			// ถ้าไม่ Fix ต้นไว้ให้เป็น 0
			if li_loanpaytype <> 3 then
				ldc_rkeepprin = 0
			end if
			
			ldc_rkeepint = ldc_periodmax - ldc_rkeepprin
		else
			ldc_tpperiodmax = ldc_periodmax - ( ldc_rkeepint /* + ldc_intarrear */ )
			// ตรวจว่าเป็นแบบ Fix ต้นและยอดต้นถึงที่ Fix ไว้มั้ย
			if li_loanpaytype = 3 and ldc_tpperiodmax < ldc_principalpay then
				ldc_rkeepint = ldc_periodmax - ldc_principalpay
				ldc_rkeepprin = ldc_principalpay
			else
				ldc_rkeepprin = ldc_tpperiodmax
			end if
		end if

		if ldc_rkeepint < 0 then ldc_rkeepint = 0
		if ldc_rkeepprin < 0 then ldc_rkeepprin = 0
		
	end if

	// กำหนดค่าต่าง ๆ
	ls_keeptype			= "L"+ls_loangroup
	
	ldc_itempay			= ldc_rkeepprin + ldc_rkeepint
	ldc_itembalance	= ldc_prinbalance - ldc_rkeepprin
	ldc_totalintaccum	+= ldc_rkeepint
	//ldc_intarrpay		= 0  //tukเอาออกเนื่องจากได้แก้ไขการให้ค่าก่อนหน้านี้ตรงคงต้น หรือคงยอด
	
	li_lastperiod ++
	li_seqno ++

	// ถ้ามีด/บค้างของปี
	if ldc_bfintyeararr > 0 then
		if ldc_rkeepint > ldc_bfintyeararr then
			ldc_intarrpay	= ldc_bfintyeararr
		else
			ldc_intarrpay	= ldc_rkeepint
		end if
	end if
	
	inv_progress.istr_progress.subprogress_text = string(ll_index) +". ทะเบียน >"+ls_memno+" สัญญา >"+string(ls_contno)+" เงินต้น >"+string(ldc_principal,"#,##0.00")+" ด/บ >"+string(ldc_interest,"#,##0.00")
	
	// insert ข้อมูลลง รายการเรียกเก็บ
	// tuk11 insert เก็บค่า fwinterest_return และ bfinterest_return และ intperiod_payment และ intreturn_payment เพิ่ม
	insert into kptempreceivedet
			( 	coop_id , 				memcoop_id,			refmemcoop_id,			kpslip_no,		
				member_no, 			recv_period, 			ref_membno, 				seq_no, 						keepitemtype_code, 
				shrlontype_code, 		bizzcoop_id,				loancontract_no, 			period, 						principal_payment, 		interest_payment, 
				intarrear_payment,	item_payment, 			item_balance, 				calintfrom_date, 			calintto_date, 
				principal_period, 		interest_period, 		bfprinbalance_amt, 		bflastcalint_date, 			bflastpay_date,
			  	bfinterest_arrear, 		bfintmonth_arrear, 	bfintyear_arrear, 			bfperiod, 					bfcontract_status, 
				bfcontlaw_status, 		keepitem_status, 		posting_status 		 ,	 	fwinterest_arrear,               fwinterest_return,          bfinterest_return,
				intperiod_payment,    intreturn_payment )
	values( 	:is_coopid , 				:ls_memcoop,			:ls_refmemcoop,			:is_kpslipno,
				:ls_memno, 			:is_recvperiod, 			:ls_refmem, 				:li_seqno, 					:ls_keeptype, 
				:ls_loantype, 			:ls_loancoopid,			:ls_contno, 					:li_lastperiod, 				:ldc_rkeepprin, 			:ldc_rkeepint, 
				:ldc_intarrpay,			:ldc_itempay, 			:ldc_itembalance, 			:ldtm_lastcalint, 			:idtm_calintto, 
				:ldc_principal, 			:ldc_intperiod, 			:ldc_prinbalance, 			:ldtm_lastcalint, 			:ldtm_lastpay,
			  	:ldc_bfintarrear, 		:ldc_bfintmtharr, 		:ldc_bfintyeararr, 			:li_bfperiod, 				:li_contstatus, 
				:li_contlaw, 				1, 							0 						 ,                                :ldc_fwarrear,                :ldc_fwreturn,                :ldc_bfintreturn ,
				:ldc_intperiodpay ,     :ldc_intreturnpay )
	using	itr_sqlca;

	if itr_sqlca.sqlcode <> 0 then
		if ii_msgerrflag = 1 then
			this.of_set_msgerr( 'ข้อมูลหนี้เงินกู้( ไม่สามารถบันทึกรายการเงินกู้ ) ทะเบียนสมาชิก : ' + ls_memno + ' / เลขที่สัญญา : ' + ls_contno + ' / ต้นเงิน : ' + string( ldc_rkeepprin , '#,##0.00') + ' / ดอกเบี้ย : ' + string( ldc_rkeepint , '#,##0.00') )
		else
			ithw_exception.text = "ประมวลผลเงินกู้ พบข้อผิดพลาด"
			ithw_exception.text += "~r~n( ไม่สามารถบันทึกรายการเงินกู้ )"
			ithw_exception.text += "~r~nทะเบียน : " + ls_memno
			ithw_exception.text += "~r~nสาขาสัญญาเงินกู้ : " + ls_loancoopid
			ithw_exception.text += "~r~nเลขที่สัญญาเงินกู้ : " + ls_contno
			ithw_exception.text += "~r~nต้นเงิน : " + string( ldc_rkeepprin , "#,##0.00") + " / ดอกเบี้ย : " + string( ldc_rkeepint , '#,##0.00')
			ithw_exception.text += "~r~n" + string( itr_sqlca.sqlcode ) 
			ithw_exception.text += "~r~n" + itr_sqlca.sqlerrtext 
			destroy ids_loandata ; destroy ids_kptp ; destroy lnv_calendarsrv
			throw ithw_exception
		end if
	end if

	// update ข้อมูลเรียกเก็บในสัญญา
	update	lncontmaster
	set			lastprocess_date	= :idtm_calintto,	
				nkeep_principal		= :ldc_principalpay,
				rkeep_principal		= :ldc_rkeepprin,
				nkeep_interest		= :ldc_interest,
				rkeep_interest		= :ldc_rkeepint
	where	coop_id				= :ls_loancoopid
	and		( loancontract_no	= :ls_contno ) 
	using itr_sqlca;
	
	if itr_sqlca.sqlcode <> 0 then
		if ii_msgerrflag = 1 then
			this.of_set_msgerr( 'ข้อมูลหนี้เงินกู้( ไม่สามารถบันทึกรายการเงินกู้ ) ทะเบียนสมาชิก : ' + ls_memno + ' / เลขที่สัญญา : ' + ls_contno + ' / ต้นเงิน : ' + string( ldc_rkeepprin , '#,##0.00') + ' / ดอกเบี้ย : ' + string( ldc_rkeepint , '#,##0.00') )
		else
			ithw_exception.text = "ประมวลผลเงินกู้ พบข้อผิดพลาด~n" + string( itr_sqlca.sqlcode ) + "  " + itr_sqlca.sqlerrtext + "~n" + ithw_exception.text
			destroy ids_loandata
			throw ithw_exception
		end if
	end if
	
	//	if ll_index = ll_count then
//		update	kptempreceive
//		set			interest_accum		= :ldc_totalintaccum
//		where	( member_no		= :ls_memno ) and
//					( recv_period		= :is_recvperiod ) and
//					( ref_membno		= :ls_refmemno )
//		using itr_sqlca;
//	end if

	ids_kptp.object.last_seq_no[il_kpfind]		= li_seqno
next

this.of_setsrvrdmoney( false )

destroy ids_loandata
destroy lnv_calendarsrv

return 1
end function

protected function integer of_processmoneyreturn () throws Exception;/***********************************************************
<description>
ประมวลจ่ายคืนเงินกู้
</description>

<arguments>

</arguments>

<return>
Integer 1 = success, Exception = failure
</return>

<usage>

Revision History:
Version 1.0 (Initial version) Modified Date 28/9/2010 by MiT
</usage>
***********************************************************/

string ls_memno, ls_memprior, ls_contno,ls_memcoop
string ls_keeptype, ls_sqlext, ls_temp
long ll_index, ll_count
integer li_seqno, li_stepvalue, li_chk, li_procrettype, li_ret, li_compoundkeep
dec{2} ldc_intretamt, ldc_itemretamt, ldc_itempayall
n_ds lds_moneyret

inv_progress.istr_progress.progress_text = "หักคืนจำนวนเงินที่เรียกเก็บเกิน"
inv_progress.istr_progress.subprogress_text = "กำลังดึงข้อมูล"
inv_progress.istr_progress.subprogress_index = 0
inv_progress.istr_progress.subprogress_max = 0

lds_moneyret = create n_ds
lds_moneyret.dataobject = "d_kp_rcvproc_moneyreturn"
lds_moneyret.settransobject( itr_sqlca )

// สร้างชุด SQL สำหรับการดึงรายการ
li_ret = this.of_setsqlselect( lds_moneyret )
if ( li_ret <> 1 ) then
	destroy lds_moneyret
	ithw_exception.text += "~nพบข้อผิดพลาดในการสร้างชุด SQL รายการค่าธรรมเนียมแรกเข้า"
	throw ithw_exception
end if

ll_count = lds_moneyret.retrieve()
inv_progress.istr_progress.subprogress_max = ll_count

//select kpprocmrt_type
//into :li_procrettype
//from kpkeepconstant
//using itr_sqlca;


if isnull( li_procrettype ) then li_procrettype = 1

for ll_index = 1 to ll_count
	inv_progress.istr_progress.subprogress_index = ll_index
	
	// ตรวจสอบการสั่งหยุดทำงาน
	yield()
	if inv_progress.of_is_stop() then
		destroy lds_moneyret
		return 0
	end if
	
	ls_memno = trim( lds_moneyret.getitemstring( ll_index, "member_no" ) )
	ls_memcoop = trim( lds_moneyret.getitemstring( ll_index, "coop_id" ) )
	ls_contno = lds_moneyret.getitemstring( ll_index, "loancontract_no" )
	ldc_intretamt = lds_moneyret.getitemdecimal( ll_index, "interest_return" )
	
	if isnull( ldc_intretamt ) then ldc_intretamt = 0
	
	// ตรวจสอบว่าเป็นการทำรายการของสมาชิกคนใหม่หรือไม่
	if ls_memno <> ls_memprior then
		// หาลำดับที่รายการ
		select max( seq_no )
		into :li_seqno
		from kptempreceivedet
		where ( member_no = :ls_memno ) and
		( recv_period = :is_recvperiod ) and
		( ref_membno = :ls_memno ) using itr_sqlca;
		
		choose case li_procrettype
			case 1 // คืนทุกกรณี
			
			case 2 // คืนกรณียอดเรียกเก็บมากกว่า
				select sum( item_payment )
				into :ldc_itempayall
				from kptempreceivedet
				where ( member_no = :ls_memno ) and
				( recv_period = :is_recvperiod ) and
				( ref_membno = :ls_memno ) using itr_sqlca;
			
			case 3 // คืนกรณียอดเรียกเก็บหนี้มากกว่า
				select sum( item_payment )
				into :ldc_itempayall
				from kptempreceivedet
				where ( member_no = :ls_memno ) and
				( recv_period = :is_recvperiod ) and
				( ref_membno = :ls_memno ) and
				( keepitemtype_code in ( 'L01', 'L02', 'L03', 'L04' ) ) using itr_sqlca;
			
			case 4 // คืนกรณีที่ยอดรวมดอกเบี้ยเรียกเก็บมากกว่ายอดคืน
				select sum( interest_payment )
				into :ldc_itempayall
				from kptempreceivedet
				where ( member_no = :ls_memno ) and
				( recv_period = :is_recvperiod ) and
				( ref_membno = :ls_memno ) and
				( keepitemtype_code in ( 'L01', 'L02', 'L03', 'L04' ) ) using itr_sqlca;
		end choose
		
		if isnull( ldc_itempayall ) then ldc_itempayall = 0
		
		ls_memprior = ls_memno
	end if
	
	if isnull( li_seqno ) then li_seqno ++
	
	if li_procrettype > 1 then
		if ldc_intretamt > ldc_itempayall then continue
	end if
	// ปรับปรุงดึงข้อมูลที่ต้องการจาก buffer datastore
	// is_kpfind = " coop_id = '"+is_coopid+"' and recv_period = '"+is_recvperiod+"' and member_no = '"+ls_memno+"' and memcoop_id = '"+ls_memcoop+"' and refmemcoop_id = '"+ls_refmemcoop+"' and ref_membno = '"+ls_refmem+"' "
	// is_kpfind = " coop_id = '"+is_coopid+"' and recv_period = '"+is_recvperiod+"' and member_no = '"+ls_memno+"' and memcoop_id = '"+ls_memcoop+"' "
	// il_kpfind = ids_kptp.find( is_kpfind , il_kpfind , il_kpcount )
	
	select min( kpslip_no )
	into :is_kpslipno
	from kptempreceive
	where member_no = :ls_memno and recv_period = :is_recvperiod using itr_sqlca ;
	
	select max( seq_no)
	into :li_seqno
	from kptempreceivedet where member_no = :ls_memno and recv_period = :is_recvperiod and kpslip_no = :is_kpslipno using itr_sqlca ;
	if isnull( li_seqno ) then li_seqno = 0
	// is_kpslipno = ids_kptp.object.kpslip_no[il_kpfind]
	// li_seqno = ids_kptp.object.last_seq_no[il_kpfind] ; if isnull( li_seqno ) then li_seqno = 0
	li_seqno ++
	// ids_kptp.object.last_seq_no[il_kpfind] = li_seqno
	//จบการปร
	// กำหนดค่าต่าง ๆ
	ls_keeptype = "MRT"
	ldc_itemretamt = ldc_intretamt * (-1)
	ldc_itempayall = ldc_itempayall - ldc_itemretamt
	li_seqno ++
	
	inv_progress.istr_progress.subprogress_text = string( ll_index ) +". ทะเบียน "+ls_memno+" > สัญญา "+ls_contno+" > คืนด/บ "+string( ldc_intretamt,"#,##0.00" )
	
	// insert ข้อมูลลง รายการเรียกเก็บ
	insert into kptempreceivedet
	( member_no,coop_id, memcoop_id ,kpslip_no, recv_period, ref_membno, seq_no, keepitemtype_code, loancontract_no, description, item_payment, keepitem_status, posting_status )
	values ( :ls_memno, :ls_memcoop,:ls_memcoop,:is_kpslipno, :is_recvperiod, :ls_memno, :li_seqno, :ls_keeptype, :ls_contno, 'ดอกเบี้ยเก็บเกิน', :ldc_itemretamt, 1, 0 )
	using itr_sqlca;
	
	if itr_sqlca.sqlcode <> 0 then
		ithw_exception.text = "ประมวลผลจ่ายคืนเงิน ไม่สามารถบันทึกรายการคืนเงินเก็บเกินได้ ~r~n"+itr_sqlca.sqlerrtext
		destroy lds_moneyret
		throw ithw_exception
	end if
	
	update kptempreceive
	set interest_accum = interest_accum - :ldc_itemretamt
	where ( member_no = :ls_memno ) and
	( recv_period = :is_recvperiod ) and
	( ref_membno = :ls_memno ) and kpslip_no = :is_kpslipno
	using itr_sqlca ;
	
	if itr_sqlca.sqlcode <> 0 then
		ithw_exception.text = "ประมวลผลจ่ายคืนเงิน ไม่สามารถปรับปรุงยอด ด/บ สะสมในใบเสร็จรับเงินได้ ~r~n"+itr_sqlca.sqlerrtext
		destroy lds_moneyret
		throw ithw_exception
	end if
next

destroy lds_moneyret

return 1
end function

protected function integer of_processother () throws Exception;/***********************************************************
<description>
	ประมวลเรียกเก็บอื่น ๆ
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
string		ls_memno, ls_memprior, ls_membgroup, ls_desc, ls_sqlext, ls_sql, ls_moneytype, ls_accid
string		ls_memcoop , ls_refmem , ls_refmemcoop , ls_contno
string		ls_keeptype , ls_keepothtype , ls_prevkeepothtype
string		ls_sqloth
long		ll_index, ll_count
integer		li_seqno, li_chk, li_stepvalue, li_ret, li_compoundkeep , li_sort
dec{2}		ldc_itemamt, ldc_shrstk, ldc_shrbegin

str_extfn_keep lstr_extfn_keep

n_ds	lds_other

inv_progress.istr_progress.progress_text	= "รายการเรียกเก็บอื่นๆ"
inv_progress.istr_progress.subprogress_text	= "กำลังดึงข้อมูล"
inv_progress.istr_progress.subprogress_index	= 0
inv_progress.istr_progress.subprogress_max	= 0

lds_other	= create n_ds
lds_other.settransobject( itr_sqlca )

lstr_extfn_keep.function_name	= 'of_kp_proc_getsql_other'
this.of_extfn( lstr_extfn_keep )
ls_sqloth	= lstr_extfn_keep.function_return

inv_dwsrv.of_create_dw( lds_other , ls_sqloth , '' )

// สร้างชุด SQL สำหรับการดึงรายการ
li_ret = this.of_setsqlselect( lds_other )
if ( li_ret <> 1 ) then
	destroy lds_other ; destroy ids_kptp
	ithw_exception.text += "~nพบข้อผิดพลาดในการสร้างชุด SQL รายการอื่น ๆ"
	throw ithw_exception
end if

ll_count	= lds_other.retrieve()
lds_other.setsort( ' oth.member_no ' )
lds_other.sort()

inv_progress.istr_progress.subprogress_max	= ll_count

for ll_index = 1 to ll_count
	inv_progress.istr_progress.subprogress_index	= ll_index

	// ตรวจสอบการสั่งหยุดทำงาน
	yield()
	if inv_progress.of_is_stop() then
		destroy lds_other ; destroy ids_kptp
		return 0
	end if

	ls_memno		= trim( lds_other.getitemstring( ll_index, "member_no" ) )
	ls_memcoop 	= is_coopid
	ls_refmem 		= ''
	ls_refmemcoop	= ''
	ls_contno		=  ls_memno //left( trim( lds_other.getitemstring( ll_index, "loancontract_no" )) , 15 )
	ls_keeptype		= trim( lds_other.getitemstring( ll_index, "keepitemtype_code" ) )
	ls_keepothtype	= trim( lds_other.getitemstring( ll_index, "keepitemtype_code" ) )
	ls_desc			= trim( lds_other.getitemstring( ll_index, "description" ) )
	li_sort				= 100 // lds_other.getitemnumber( ll_index, "sort" )
	ldc_itemamt		= lds_other.getitemdecimal( ll_index, "item_payment" )

	if isnull( ldc_itemamt ) then ldc_itemamt = 0
	if ldc_itemamt = 0 then 
		this.of_set_msgerr( 'ไม่พบข้อมูลค่าเรียกเก็บอื่นๆ ทะเบียน : ' + ls_memno + ' ค่าเรียกเก็บอื่น ๆ 0.00 บาท ' )
		continue ;
	end if
	
	//กำหนดค่าต่างๆ //tuk05102015
	if ls_keeptype = 'E01' then
	else
		ls_keeptype = 'OTH'
    end if
	
	if ls_keepothtype <> ls_prevkeepothtype then 
		il_kpfind = 0
		ls_prevkeepothtype	= ls_keepothtype
	end if
	
	// ปรับปรุงดึงข้อมูลที่ต้องการจาก buffer datastore
	is_kpfind		= " coop_id = '"+is_coopid+"' and recv_period = '"+is_recvperiod+"' and member_no = '"+ls_memno+"' and memcoop_id = '"+ls_memcoop+"' "
	il_kpfind		= ids_kptp.find( is_kpfind , il_kpfind , il_kpcount )
	
	if il_kpfind <= 0 then
//		if ii_msgerrflag = 1 then
//			this.of_set_msgerr( "เรียกเก็บอื่น ๆ ( " + ls_desc + " ) ไม่พบข้อมูล ทะเบียน : " + ls_memno + " > " + string( itr_sqlca.sqlcode ) + " < " + itr_sqlca.sqlerrtext )
//		else
//			ithw_exception.text = "ประมวลผลอื่น ๆ  พบข้อผิดพลาด"
//			ithw_exception.text += "~r~n" + ls_desc
//			ithw_exception.text += "~r~nไม่พบข้อมูล ทะเบียน : "+ ls_memno
//			ithw_exception.text += "~r~n"+ string( itr_sqlca.sqlcode )
//			ithw_exception.text += "~r~n"+ itr_sqlca.sqlerrtext
//			destroy lds_other ; destroy ids_kptp
//			throw ithw_exception
//		end if
		il_kpfind = 1
		il_kpfind		= ids_kptp.find( is_kpfind , il_kpfind , il_kpcount )
		
//		if il_kpfind <= 0 then
//			continue;
//		end if
	end if
	
	try
		is_kpslipno	= ids_kptp.object.kpslip_no[il_kpfind]
		li_seqno		= ids_kptp.object.last_seq_no[il_kpfind] ; if isnull( li_seqno ) then li_seqno = 0
	catch( Exception lthw_error )
		destroy lds_other ; destroy ids_kptp
		
		lthw_error.text		= "ไม่สามารถดึงค่าใบรายการเรียกเก็บ (kpslip_no) -> of_processother ได้ เลขสมาชิก "+ls_memno
		
		throw lthw_error
	end try
	
	li_seqno ++
	ids_kptp.object.last_seq_no[il_kpfind]		= li_seqno
	//จบการปรับปรุง

	// insert ข้อมูลลง รายการเรียกเก็บ
	insert into kptempreceivedet
			( 	coop_id , 					memcoop_id,			refmemcoop_id,		kpslip_no,			shrlontype_code ,
				member_no, 				recv_period, 			ref_membno, 			seq_no, 				loancontract_no ,
				keepitemtype_code, 		description,			  	item_payment, 			posting_status, 	bfcontract_status ,
				keepitem_status ,			bfcontlaw_status)
	values	
			( 	:is_coopid , 					:ls_memcoop,			:is_coopid,		:is_kpslipno,	'' ,
				:ls_memno, 				:is_recvperiod, 			:ls_memno, 			:li_seqno, 			:ls_contno,
				:ls_keeptype, 						:ls_desc,			  		:ldc_itemamt, 			0, 						1 ,
				1 ,								1)
	using itr_sqlca;
			  
	if itr_sqlca.sqlcode = 0 then
		inv_progress.istr_progress.subprogress_text = string( ll_index ) +". ทะเบียน "+ls_memno+" > "+ls_desc+" > จำนวนเงิน "+string( ldc_itemamt, "#,##0.00" )
	else
		if ii_msgerrflag = 1 then
			this.of_set_msgerr( "เรียกเก็บอื่น ๆ ( " + ls_desc + " ) ทะเบียน : " + ls_memno + " > " + string( itr_sqlca.sqlcode ) + " < " + itr_sqlca.sqlerrtext )
		else
			ithw_exception.text = "ประมวลผลอื่น ๆ  พบข้อผิดพลาด"
			ithw_exception.text += "~r~n" + ls_desc
			ithw_exception.text += "~r~nทะเบียน : "+ ls_memno
			ithw_exception.text += "~r~n"+ string( itr_sqlca.sqlcode )
			ithw_exception.text += "~r~n"+ itr_sqlca.sqlerrtext
			destroy lds_other ; destroy ids_kptp
			throw ithw_exception
		end if
	end if
	
	choose case ls_keeptype
		case "UIA"
			update	kptempreceive
			set			interest_accum	= interest_accum + :ldc_itemamt
			where	( coop_id			= :is_coopid ) 
			and		( kpslip_no		= :is_kpslipno )
			using		itr_sqlca ;
			
			if itr_sqlca.sqlcode <> 0 then
				if ii_msgerrflag = 1 then
					this.of_set_msgerr( "ประมวลผลรายการอื่น ๆ ( ดอกเบี้ยค้างรับ ) ไม่สามารถปรับปรุงยอด ด/บ สะสมในใบเสร็จรับเงินได้ ทะเบียน : " + ls_memno + " > " + string( itr_sqlca.sqlcode ) + " < " + itr_sqlca.sqlerrtext )
				else
					ithw_exception.text = "ประมวลผลรายการอื่น ๆ ( ดอกเบี้ยค้างรับ ) ไม่สามารถปรับปรุงยอด ด/บ สะสมในใบเสร็จรับเงินได้ "  
					ithw_exception.text += "~r~nทะเบียน : "+ ls_memno
					ithw_exception.text += "~r~n"+ string( itr_sqlca.sqlcode )
					ithw_exception.text += "~r~n"+ itr_sqlca.sqlerrtext
					destroy lds_other ; destroy ids_kptp
					return -1
				end if
			end if
		case "UIR"
			update	kptempreceive
			set			interest_accum	= interest_accum - :ldc_itemamt
			where	( coop_id			= :is_coopid ) 
			and		( kpslip_no		= :is_kpslipno )
			using		itr_sqlca ;
			
			if itr_sqlca.sqlcode <> 0 then
				if ii_msgerrflag = 1 then
					this.of_set_msgerr( "ประมวลผลรายการอื่น ๆ ( ดอกเบี้ยจ่ายคืน ) ไม่สามารถปรับปรุงยอด ด/บ สะสมในใบเสร็จรับเงินได้ ทะเบียน : " + ls_memno + " > " + string( itr_sqlca.sqlcode ) + " < " + itr_sqlca.sqlerrtext )
				else
					ithw_exception.text = "ประมวลผลรายการอื่น ๆ ( ดอกเบี้ยจ่ายคืน ) ไม่สามารถปรับปรุงยอด ด/บ สะสมในใบเสร็จรับเงินได้ "  
					ithw_exception.text += "~r~nทะเบียน : "+ ls_memno
					ithw_exception.text += "~r~n"+ string( itr_sqlca.sqlcode )
					ithw_exception.text += "~r~n"+ itr_sqlca.sqlerrtext
					destroy lds_other ; destroy ids_kptp
					return -1
				end if
			end if
			
			//MHS_tuk15092015 <<
            case "E01"
			update	INSGROUPMASTER
			set		INSPAYMENT_STATUS = '1'
			where	( MEMBER_NO		= :ls_memno ) 
			and		( INSPAYMENT_STATUS = '8' )
			using	itr_sqlca ;
			
			if itr_sqlca.sqlcode <> 0 then
				if ii_msgerrflag = 1 then
					this.of_set_msgerr( "ประมวลผลรายการอื่น ๆ ( ประกันสินเชื่อ ) ไม่สามารถปรับปรุงรายการ ทะเบียน : " + ls_memno + " > " + string( itr_sqlca.sqlcode ) + " < " + itr_sqlca.sqlerrtext )
				else
					ithw_exception.text = "ประมวลผลรายการอื่น ๆ ( ประกันสินเชื่อ ) "  
					ithw_exception.text += "~r~nทะเบียน : "+ ls_memno
					ithw_exception.text += "~r~n"+ string( itr_sqlca.sqlcode )
					ithw_exception.text += "~r~n"+ itr_sqlca.sqlerrtext
					destroy lds_other ; destroy ids_kptp
					return -1
				end if
			end if
            //MHS_tuk15092015 >>>
			
	end choose
		
next

destroy lds_other ;

return 1
end function

protected function integer of_processrecpamt () throws Exception;/***********************************************************
<description>
	ประมวลสรุปยอดเรียก
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

string		ls_refmemno, ls_kpslipno , ls_kpslipprv
string		ls_memcoop , ls_refmem , ls_refmemcoop , ls_sqlrecp
string		ls_sqlext , ls_temp , ls_sqlselect , ls_sqlgroup , ls_receipt
string		ls_moneytext, ls_savekeep
long		ll_index, ll_count
integer	li_chk, li_pos, li_maxseqno
dec{2}	ldc_recpamt, ldc_sumemer, ldc_diskkeep , ldc_recpall
boolean	lb_err = false

n_ds		lds_recpdata
n_cst_thailibservice	lnv_thailibsrv
str_extfn_keep lstr_extfn_keep

inv_progress.istr_progress.progress_text	= "สรุปยอดเรียกเก็บทั้งหมด"
inv_progress.istr_progress.subprogress_text	= "กำลังดึงข้อมูล"
inv_progress.istr_progress.subprogress_index	= 0
inv_progress.istr_progress.subprogress_max	= 0

lnv_thailibsrv	= create n_cst_thailibservice

lds_recpdata	= create n_ds
lds_recpdata.dataobject = "d_kp_rcvproc_receiptamt"
lds_recpdata.settransobject( itr_sqlca )

ls_sqlext = "and ( kptempreceive.coop_id = '" + is_coopid + "' ) "

choose case ii_proctype
	case 1
		ls_sqlext	+= "and ( kptempreceive.recv_period = '"+is_recvperiod+"' ) "
	case 2
		inv_stringsrv.of_buildsqlext( is_arg[], "kptempreceive.membgroup_code", ls_sqlext )
		ls_sqlext = " and " + ls_sqlext
		ls_sqlext	+= "and ( kptempreceive.recv_period = '"+is_recvperiod+"' )  "+ls_sqlext
	case 3
		inv_stringsrv.of_buildsqlext(is_arg[], "kptempreceive.member_no", ls_sqlext)
		ls_sqlext = " and " + ls_sqlext
		ls_sqlext	+= "and ( kptempreceive.recv_period = '"+is_recvperiod+"' )  "+ls_sqlext
end choose

if ls_sqlext <> "" then
	ls_temp		= lds_recpdata.getsqlselect()
	li_pos			= pos( ls_temp, "GROUP BY", 1 )
	ls_sqlselect	= left( ls_temp, li_pos - 1 )
	ls_sqlgroup	= mid( ls_temp, li_pos - 1 )
	ls_temp		= ls_sqlselect + ls_sqlext + ls_sqlgroup
	
	lds_recpdata.setsqlselect( ls_temp )
	lds_recpdata.settransobject( itr_sqlca )
end if

ll_count		= lds_recpdata.retrieve( )

ids_kptp.setsort( ' coop_id , kpslip_no ' )
ids_kptp.sort()
lds_recpdata.setsort( ' coop_id , kpslip_no ' )
lds_recpdata.sort()

inv_progress.istr_progress.subprogress_max = ll_count

for ll_index = 1 to ll_count
	
	inv_progress.istr_progress.subprogress_index	= ll_index
	
	// ตรวจสอบการสั่งหยุดทำงาน
	yield()
	if inv_progress.of_is_stop() then
		ithw_exception.text += "~r~nยกเลิกโดยผู้ใช้ระบบ"
		lb_err = true ; Goto objdestroy
	end if
	
	ls_kpslipno		= lds_recpdata.getitemstring( ll_index, "kpslip_no" )
	ldc_recpamt		= lds_recpdata.getitemdecimal( ll_index, "sumitem_payamt" )
	li_maxseqno		= lds_recpdata.getitemnumber( ll_index, "max_seqno" )
	
	ls_moneytext	= lnv_thailibsrv.of_readthaibaht( ldc_recpamt )
	
	inv_progress.istr_progress.subprogress_text	= "เลขที่ kpslip "+ls_kpslipno+" > ยอดเรียกเก็บ "+string( ldc_recpamt, "#,##0.00" )
	
	update 	kptempreceive
	set 		receive_amt 	= :ldc_recpamt ,
				money_text		= :ls_moneytext ,
				last_seq_no		= :li_maxseqno 
	where	coop_id			= :is_coopcontrol
	and		kpslip_no			= :ls_kpslipno
	using itr_sqlca ;
	
	if itr_sqlca.sqlcode <> 0 then
		if ii_msgerrflag = 1 then
			this.of_set_msgerr( "บันทึกข้อมูลใบเสร็จประจำเดือน( สรุปยอดส่งหัก 0.01 ) >" + string( itr_sqlca.sqlcode ) + " < " + itr_sqlca.sqlerrtext )
		else
			ithw_exception.text = "บันทึกข้อมูลใบเสร็จประจำเดือน( สรุปยอดส่งหัก 0.01 ) พบข้อผิดพลาด"  
			ithw_exception.text += "~r~n"+ string( itr_sqlca.sqlcode )
			ithw_exception.text += "~r~n"+ itr_sqlca.sqlerrtext
			lb_err = true ; Goto objdestroy
		end if
	end if
	
next

//อัพเดทวันที่เรียกเก็บล่าสุด
update mbmembmaster m
set m.lastkeep_date = :idtm_receipt
where m.coop_id = :is_coopcontrol
and exists ( select 1 from kptempreceive kt
				where kt.coop_id = m.coop_id
				and kt.member_no = m.member_no
				and kt.coop_id = :is_coopcontrol
				and kt.recv_period = :is_recvperiod
				)
using itr_sqlca;
if itr_sqlca.sqlcode <> 0 then
	if ii_msgerrflag = 1 then
		this.of_set_msgerr( "บันทึกข้อมูลใบเสร็จประจำเดือน( อัพเดทวันที่เรียกเก็บล่าสุด 0.011 ) >" + string( itr_sqlca.sqlcode ) + " < " + itr_sqlca.sqlerrtext )
	else
		ithw_exception.text = "บันทึกข้อมูลใบเสร็จประจำเดือน( อัพเดทวันที่เรียกเก็บล่าสุด 0.011 ) พบข้อผิดพลาด"  
		ithw_exception.text += "~r~n"+ string( itr_sqlca.sqlcode )
		ithw_exception.text += "~r~n"+ itr_sqlca.sqlerrtext
		lb_err = true ; Goto objdestroy
	end if
end if

//default ข้อมูลที่ไม่ใส่วิธีคีย์
insert into kpmembexpense
( coop_id , member_no , seq_no , memcoop_id , moneytype_code , expense_bank , expense_branch , expense_accid , monthlycut_type , monthlycut_percent , monthlycut_value , sort_in_monthlycut )
select mm.coop_id , mm.member_no , 1 , mm.coop_id , 'TMT' , '' , '' , '' , 'A' , 0 , 0 , 1 
from mbmembmaster mm
where mm.coop_id = :is_coopid
and not exists ( 	select 1 
						from kpmembexpense ke 
						where ke.coop_id = :is_coopid
						and mm.coop_id = ke.coop_id 
						and mm.member_no = ke.member_no )
and mm.resign_status = 0
using itr_sqlca;
if itr_sqlca.sqlcode <> 0 then
	if ii_msgerrflag = 1 then
		this.of_set_msgerr( "บันทึกข้อมูลใบเสร็จประจำเดือน( นำเข้าวิธีคีย์สมาชิกใหม่ 0.02 ) >" + string( itr_sqlca.sqlcode ) + " < " + itr_sqlca.sqlerrtext )
	else
		ithw_exception.text = "บันทึกข้อมูลใบเสร็จประจำเดือน( นำเข้าวิธีคีย์สมาชิกใหม่ 0.02 ) พบข้อผิดพลาด"  
		ithw_exception.text += "~r~n"+ string( itr_sqlca.sqlcode )
		ithw_exception.text += "~r~n"+ itr_sqlca.sqlerrtext
		lb_err = true ; Goto objdestroy
	end if
end if

objdestroy:
if isvalid(lds_recpdata) then destroy lds_recpdata
if isvalid(lnv_thailibsrv) then destroy lnv_thailibsrv

if lb_err then
	rollback using itr_sqlca ;
	ithw_exception.text = "n_cst_keeping_process.of_processrecpamt()" + ithw_exception.text
	throw ithw_exception
end if

return 1
end function

protected function integer of_processshare () throws Exception;/***********************************************************
<description>
	ประมวลเรียกเก็บหุ้น
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
string		ls_memno, ls_memprior, ls_membgroup, ls_desc, ls_sqlext, ls_temp
string		ls_keeptype, ls_sharetype, ls_moneytype, ls_accid, ls_sqlshr
string		ls_memcoop , ls_refmem , ls_refmemcoop
long		ll_index, ll_count
integer	li_sharestatus, li_lastperiod, li_seqno, li_stepvalue, li_ret, li_compoundkeep
integer	li_pauseflag, li_paystatus , li_showflag
integer	li_compoundstatus, li_compoundpaystatus
dec{2}	ldc_periodamt, ldc_sharevalue, ldc_totalshrstk, ldc_shrbfvalue
dec{3}	ldc_shrbegin, ldc_shrstkamt
dec{2}	ldc_compoundamt, ldc_intaccum
boolean	lb_showonly

str_extfn_keep lstr_extfn_keep

n_ds	lds_share

li_showflag		= ids_kpconstant.object.showsl_flag[1]

inv_progress.istr_progress.progress_text		= "หุ้นรายเดือน"
inv_progress.istr_progress.subprogress_text	= "กำลังดึงข้อมูล"
inv_progress.istr_progress.subprogress_index	= 0
inv_progress.istr_progress.subprogress_max	= 0

lds_share	= create n_ds
lds_share.settransobject( itr_sqlca )

lstr_extfn_keep.function_name	= 'of_kp_proc_getsql_share'
this.of_extfn( lstr_extfn_keep )
ls_sqlshr	= lstr_extfn_keep.function_return

inv_dwsrv.of_create_dw( lds_share , ls_sqlshr , '' )

// สร้างชุด SQL สำหรับการดึงรายการ
li_ret = this.of_setsqlselect( lds_share )
if ( li_ret <> 1 ) then
	destroy lds_share
	ithw_exception.text += "~nพบข้อผิดพลาดในการสร้างชุด SQL รายการค่าหุ้น"
	throw ithw_exception	
end if

ll_count	= lds_share.retrieve()
//lds_share.setsort( ' shsharemaster_coop_id , mbmembmaster_member_no ' )
lds_share.setsort( " coop_id , member_no , member_ref desc " )
lds_share.sort()

inv_progress.istr_progress.subprogress_max	= ll_count

for ll_index = 1 to ll_count
	
	inv_progress.istr_progress.subprogress_index	= ll_index
	
	// ตรวจสอบการสั่งหยุดทำงาน
	yield()
	if inv_progress.of_is_stop() then
		destroy lds_share
		return 0
	end if
	
	ls_memno			= trim( lds_share.getitemstring( ll_index, "member_no" ) )
	ls_sharetype		= trim( lds_share.getitemstring( ll_index, "sharetype_code" ) )
	ls_memcoop 		= is_coopcontrol
	ls_refmem 			= lds_share.object.member_ref[ ll_index ]
	ls_refmemcoop		= is_coopcontrol
	li_sharestatus		= lds_share.getitemnumber( ll_index, "sharemaster_status" )
	li_lastperiod			= lds_share.getitemnumber( ll_index, "last_period" )
	li_pauseflag			= lds_share.getitemnumber( ll_index, "pausekeep_flag" )
	li_paystatus			= lds_share.getitemnumber( ll_index, "payment_status" )
	ldc_periodamt		= lds_share.getitemdecimal( ll_index, "periodshare_amt" )
	ldc_shrstkamt		= lds_share.getitemdecimal( ll_index, "sharestk_amt" )
	ldc_shrbegin		= lds_share.getitemdecimal( ll_index, "sharebegin_amt" )
	ldc_sharevalue		= lds_share.getitemdecimal( ll_index, "unitshare_value" )
	
	lb_showonly			= false
	
	// มีการผ่อนผันการชำระหรือไม่	
	li_compoundstatus			= lds_share.getitemdecimal( ll_index, "compound_status"  )			// ผ่อนผัน
	ldc_compoundamt			= lds_share.getitemdecimal( ll_index, "compound_payment"  )			// ชำระต่องวด
	li_compoundpaystatus	= lds_share.getitemnumber( ll_index, "compound_paystatus"  )		// งวดส่ง / ส่งต่อ
	
	if isnull( ldc_compoundamt ) then ldc_compoundamt = 0
	if isnull( li_compoundstatus ) then li_compoundstatus = 0
	if isnull( li_compoundpaystatus ) then li_compoundpaystatus = 0
	
	// ตรวจว่ามีการงดเก็บไว้หรือไม่
	if li_paystatus = -1 then
		if li_showflag = 1 then 
			lb_showonly = true
		else
			continue;
		end if
	end if
	
	// ขอผ่อนผันหรือไม่
	if ( li_compoundstatus = 1 ) then
		// ขอหยุดส่งหรือไม่
		if ( li_compoundpaystatus = -1 ) then
			if li_showflag = 1 then 
				lb_showonly = true
			else
				continue;
			end if
		else
			ldc_periodamt	= ldc_compoundamt
		end if
	end if

	if isnull( li_lastperiod ) then li_lastperiod = 0
	if isnull( ldc_shrstkamt ) then ldc_shrstkamt = 0

	// กำหนดค่าต่าง ๆ
	if lb_showonly then
		ls_keeptype		= "DPS"
		ls_desc			= "ค่าหุ้น"
		ldc_periodamt	= 0
	else
		ls_keeptype		= "S"+ls_sharetype
		ls_desc			= "ค่าหุ้น"
		ldc_periodamt	= ldc_periodamt * ldc_sharevalue
		
		li_lastperiod ++
	end if
	
	if ( isnull( ldc_periodamt ) or ldc_periodamt = 0 ) and not lb_showonly then
		this.of_set_msgerr( "ไม่พบข้อมูลเรียกเก็บค่าหุ้น ทะเบียน : " + ls_memno )
		continue ;
	end if
	
	ldc_shrstkamt	= ldc_shrstkamt * ldc_sharevalue
	ldc_totalshrstk	= ( ldc_shrstkamt + ldc_periodamt )
	
	// ปรับปรุงดึงข้อมูลที่ต้องการจาก buffer datastore
//	is_kpfind		= " coop_id = '"+is_coopid+"' and recv_period = '"+is_recvperiod+"' and member_no = '"+ls_memno+"' and memcoop_id = '"+ls_memcoop+"' and refmemcoop_id = '"+ls_refmemcoop+"' and ref_membno = '"+ls_refmem+"' "
	is_kpfind		= " coop_id = '"+is_coopid+"' and recv_period = '"+is_recvperiod+"' and member_no = '"+ls_memno+"' and memcoop_id = '"+ls_memcoop+"' "
	il_kpfind		= ids_kptp.find( is_kpfind , il_kpfind , il_kpcount )
	// wa เพิ่ม แกไข้ กรณีมี error 
	if il_kpfind <= 0 then 
		il_kpfind		= ids_kptp.find( is_kpfind , 1 , il_kpcount )
		
	end if
	try
		is_kpslipno	= ids_kptp.object.kpslip_no[il_kpfind]
		li_seqno		= ids_kptp.object.last_seq_no[il_kpfind] ; if isnull( li_seqno ) then li_seqno = 0
	catch( Exception lthw_errseq)
		ithw_exception.text		= "เลขสมาชิกไม่พบข้อมูลใบเสร็จประจำเดือน : " + ls_memno
		throw ithw_exception
	end try
	li_seqno ++
	ids_kptp.object.last_seq_no[il_kpfind]		= li_seqno
	ids_kptp.object.sharestk_value[il_kpfind]	= ldc_totalshrstk
	//จบการปรับปรุง

	inv_progress.istr_progress.subprogress_text	= string(ll_index) +". ทะเบียน "+ls_memno+" > หุ้นรายเดือน "+string(ldc_periodamt,"#,##0.00")+" > ทุนเรือนหุ้น "+string(ldc_shrstkamt,"#,##0.00")

	// insert ข้อมูลลง รายการเรียกเก็บ
	insert into kptempreceivedet
			( 	coop_id , 					memcoop_id,			refmemcoop_id,		kpslip_no,
				member_no, 				recv_period, 			ref_membno, 			seq_no, 
				keepitemtype_code, 		shrlontype_code, 		description,			  	period, 
				item_payment, 				item_balance, 			posting_status, 		keepitem_status,
				bizzcoop_id,					bfcontract_status ,	bfcontlaw_status )
	values( 	:is_coopid ,					:ls_memcoop ,			:ls_refmemcoop ,		:is_kpslipno,
				:ls_memno, 				:is_recvperiod, 			:ls_refmem, 			:li_seqno, 
				:ls_keeptype, 				:ls_sharetype, 			:ls_desc,			  		:li_lastperiod, 
				:ldc_periodamt, 			:ldc_totalshrstk, 		0, 							1,
				:ls_memcoop ,				:li_sharestatus ,		:li_sharestatus )
	using	itr_sqlca  ;

	if itr_sqlca.sqlcode <> 0 then
		if ii_msgerrflag = 1 then
			this.of_set_msgerr("ประมวลผลหุ้น ทะเบียน : " + ls_memno + " > " + string( itr_sqlca.sqlcode ) + " < " + itr_sqlca.sqlerrtext )
		else
			ithw_exception.text = "ประมวลผลหุ้น พบข้อผิดพลาด" 
			ithw_exception.text += "~r~nทะเบียน : "+ ls_memno
			ithw_exception.text += "~r~n"+ string( itr_sqlca.sqlcode ) 
			ithw_exception.text += "~r~n"+ itr_sqlca.sqlerrtext 
			destroy lds_share ; destroy ids_kptp
			return -1
		end if
	end if
	
	if not lb_showonly then
		//  บันทึกยอดเงินค่าหุ้นรอเรียกเก็บ
		update	shsharemaster
		set		rkeep_sharevalue	= :ldc_periodamt,
					lastprocess_date	= :idtm_receipt
		where	( sharetype_code	= :ls_sharetype ) 
		and		( coop_id 			= :is_coopid )
		and		( member_no 		= :ls_memno )
		using itr_sqlca;

		if itr_sqlca.sqlcode <> 0 then
			if ii_msgerrflag = 1 then
				this.of_set_msgerr("ไม่สามารถอัพเดทข้อมูลหุ้นรอเรียกเก็บได้ ทะเบียน : " + ls_memno + " > " + string( itr_sqlca.sqlcode ) + " < " + itr_sqlca.sqlerrtext )
			else
				ithw_exception.text = "ประมวลผลหุ้น พบข้อผิดพลาด" 
				ithw_exception.text += "~r~nไม่สามารถอัพเดทข้อมูลหุ้นรอเรียกเก็บได้"
				ithw_exception.text += "~r~nทะเบียน : " + ls_memno
				ithw_exception.text += "~r~n"+ string( itr_sqlca.sqlcode ) 
				ithw_exception.text += "~r~n"+ itr_sqlca.sqlerrtext 
				destroy lds_share ; destroy ids_kptp
				return -1
			end if
		end if
		
	end if
next

destroy lds_share

return 1
end function

protected function integer of_set_msgerr (string as_msgerr);/***********************************************************
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

ll_row		= ids_msgerr.insertrow(0)

ids_msgerr.object.message[ll_row]	= as_msgerr

return 1
end function

protected function integer of_setsqlselect (ref n_ds ads_info) throws Exception;/***********************************************************
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

string		ls_sqlext, ls_temp
integer	li_ret

choose case ii_proctype
	case 1
		ls_sqlext	= ""
	case 2
		inv_stringsrv.of_buildsqlext( is_arg[], "mbmembmaster.membgroup_code", ls_sqlext )
		ls_sqlext	= "and "+ls_sqlext
	case 3
		inv_stringsrv.of_buildsqlext( is_arg[], "mbmembmaster.member_no", ls_sqlext )
		ls_sqlext	= "and "+ls_sqlext
	case 4
		inv_stringsrv.of_buildsqlext( is_arg[], "mbmembmaster.emp_type", ls_sqlext )
		ls_sqlext	= "and "+ls_sqlext
end choose

ads_info.settransobject( itr_sqlca )

if ls_sqlext <> "" then
	ls_temp	= ads_info.getsqlselect()
	ls_temp	+= ls_sqlext
	li_ret = ads_info.setsqlselect( ls_temp )
	
	if ( li_ret <> 1 ) then
		return -1
	end if	
	ads_info.settransobject( itr_sqlca )
end if

return 1
end function

protected function integer of_setsqlselect_kptemp (ref n_ds ads_info) throws Exception;/***********************************************************
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

string		ls_sqlext, ls_temp
integer	li_ret

choose case ii_proctype
	case 1
		ls_sqlext	= ""
	case 2
		inv_stringsrv.of_buildsqlext( is_arg[], "kptempreceive.membgroup_code", ls_sqlext )
		ls_sqlext	= "and "+ls_sqlext
	case 3
		inv_stringsrv.of_buildsqlext( is_arg[], "kptempreceive.member_no", ls_sqlext )
		ls_sqlext	= "and "+ls_sqlext
	case 4
		inv_stringsrv.of_buildsqlext( is_arg[], "kptempreceive.emp_type", ls_sqlext )
		ls_sqlext	= "and "+ls_sqlext
end choose

ads_info.settransobject( itr_sqlca )

if ls_sqlext <> "" then
	ls_temp	= ads_info.getsqlselect()
	ls_temp	+= ls_sqlext
	li_ret = ads_info.setsqlselect( ls_temp )
	
	if ( li_ret <> 1 ) then
		return -1
	end if	
	ads_info.settransobject( itr_sqlca )
end if

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

protected function integer of_setsrvproc (boolean ab_switch);// Check argument
if isnull( ab_switch ) then
	return -1
end if

if ab_switch then
	if isnull( inv_procsrv ) or not isvalid( inv_procsrv ) then
		inv_procsrv	= create n_cst_procservice
		inv_procsrv.of_initservice()
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

protected function integer of_setsrvrdmoney (boolean ab_switch);// Check argument
if isnull( ab_switch ) then
	return -1
end if

if ab_switch then
	if isnull( inv_rdmoneysrv ) or not isvalid( inv_rdmoneysrv ) then
		inv_rdmoneysrv	= create n_cst_roundmoneyservice
		inv_rdmoneysrv.of_initservice( inv_transection )
		return 1
	end if
else
	if isvalid( inv_rdmoneysrv ) then
		destroy inv_rdmoneysrv
		return 1
	end if
end if

return 0
end function

protected function integer of_updatemem () throws Exception;/***********************************************************
<description>
	ประมวลค่าธรรมเนียมแรกเข้า
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
string		ls_memno, ls_membgroup, ls_desc, ls_keeptype, ls_temp, ls_sqlext
string		ls_memcoop , ls_refmem , ls_refmemcoop
string		ls_moneytype, ls_accid, ls_subgrp
string		ls_sqlupmem
long		ll_index, ll_count
integer		li_seqno, li_chk, li_stepvalue,li_keepperiod, li_ret, li_compoundkeep
dec{2}		ldc_feeamt, ldc_shrstk, ldc_shrbegin

str_extfn_keep lstr_extfn_keep

n_ds	lds_upmem

inv_progress.istr_progress.progress_text		= "ปรับปรุงข้อมูลสมาชิก"
inv_progress.istr_progress.subprogress_text	=  "กำลังดึงข้อมูล"
inv_progress.istr_progress.subprogress_index	= 0
inv_progress.istr_progress.subprogress_max	= 0

lds_upmem	= create n_ds
lds_upmem.settransobject( itr_sqlca )

lstr_extfn_keep.function_name	= 'of_kp_proc_getsql_memkpinfo'
this.of_extfn( lstr_extfn_keep )
ls_sqlupmem	= lstr_extfn_keep.function_return

inv_dwsrv.of_create_dw( lds_upmem , ls_sqlupmem , '' )

// สร้างชุด SQL สำหรับการดึงรายการ
li_ret = this.of_setsqlselect( lds_upmem )
if ( li_ret <> 1 ) then
	destroy lds_upmem ; destroy ids_kptp
	ithw_exception.text += "~nพบข้อผิดพลาดในการสร้างชุด SQL ปรับปรุงข้อมูลสมาชิก"
	throw ithw_exception
end if

ll_count	= lds_upmem.retrieve()
lds_upmem.setsort( ' coop_id , member_no ' )
lds_upmem.sort()

inv_progress.istr_progress.subprogress_max	= ll_count

for ll_index = 1 to ll_count
	inv_progress.istr_progress.subprogress_index	= ll_index

	// ตรวจสอบการสั่งหยุดทำงาน
	yield()
	if inv_progress.of_is_stop() then
		destroy lds_upmem ; destroy ids_kptp
		throw ithw_exception
	end if
	
	ls_memno		= trim( lds_upmem.getitemstring( ll_index, "member_no" ) )
	ls_memcoop 	= is_coopid
	ls_refmem 		= ''
	ls_refmemcoop	= ''

	inv_progress.istr_progress.subprogress_text	= string(ll_index) +". ทะเบียน "+ls_memno+" > ปรับปรุงข้อมูลสมาชิก " 
	
	// insert ข้อมูลลง รายการเรียกเก็บ
	update kptempreceive kt
	set ( kt.membtype_code , kt.department_code , kt.membgroup_code ) =
	( 	select m.membtype_code , m.department_code , m.membgroup_code 
		from mbmembmaster m
		where m.coop_id = kt.coop_id 
		and m.member_no = kt.member_no )
	where kt.coop_id = :is_coopcontrol
	and kt.member_no = :ls_memno
	using itr_sqlca;
	
	if itr_sqlca.sqlcode = 0 then
		inv_progress.istr_progress.subprogress_text = string(ll_index) +". ทะเบียน "+ls_memno+" > ปรับปรุงข้อมูลสมาชิก"
	else
		if ii_msgerrflag = 1 then
			this.of_set_msgerr( "ปรับปรุงข้อมูลสมาชิก ทะเบียน : " + ls_memno + " > " + string( itr_sqlca.sqlcode ) + " < " + itr_sqlca.sqlerrtext )
		else
			ithw_exception.text = "ปรับปรุงข้อมูลสมาชิก พบข้อผิดพลาด"  
			ithw_exception.text += "~r~nทะเบียน : "+ ls_memno
			ithw_exception.text += "~r~n"+ string( itr_sqlca.sqlcode )
			ithw_exception.text += "~r~n"+ itr_sqlca.sqlerrtext
			destroy lds_upmem ; destroy ids_kptp
			throw ithw_exception
		end if
	end if
next

destroy lds_upmem

return 1
end function

protected function string of_getsqlselect () throws Exception;/***********************************************************
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

string		ls_sqlext, ls_temp
integer	li_ret

choose case ii_proctype
	case 1
		ls_sqlext	= ""
	case 2
		inv_stringsrv.of_buildsqlext( is_arg[], "mbmembmaster.membgroup_code", ls_sqlext )
		ls_sqlext	= "and "+ls_sqlext
	case 3
		inv_stringsrv.of_buildsqlext( is_arg[], "mbmembmaster.member_no", ls_sqlext )
		ls_sqlext	= "and "+ls_sqlext
	case 4
		inv_stringsrv.of_buildsqlext( is_arg[], "mbmembmaster.emp_type", ls_sqlext )
		ls_sqlext	= "and "+ls_sqlext
end choose

return ls_sqlext
end function

protected function integer of_processrecpno (n_ds ads_procdata) throws Exception;/***********************************************************
<description>
	ประมวลสรุปยอดเรียก
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

string		ls_memno, ls_docno, ls_year, ls_temp, ls_sqlext, ls_sqltext
string		ls_refmemno, ls_prefix , ls_recpsort
string		ls_kpslipno
string		ls_sqlselect , ls_sql , ls_sqlrecp
long		ll_recpno
long		ll_index, ll_count
integer	li_seqno, li_chk, li_stepvalue, li_ret
boolean	lb_err = false
n_ds		lds_recpno

lds_recpno	= create n_ds
lds_recpno.settransobject( itr_sqlca )

ls_sqlselect		= this.of_getsqlselect( )

inv_progress.istr_progress.progress_text		= "จัดทำเลขที่ใบเสร็จ"
inv_progress.istr_progress.subprogress_text	= "กำลังดึงข้อมูล"
inv_progress.istr_progress.subprogress_index	= 0
inv_progress.istr_progress.subprogress_max	= 0

ls_recpsort			= ads_procdata.object.recpno_sort[1]
ll_recpno				= ads_procdata.getitemnumber( 1, "recpno_startnum" )

if isnull( ll_recpno ) then ll_recpno = 1

ls_prefix	= trim( mid( is_recvperiod, 3 ) )

if is_coopcontrol = '010001' then
	
	update kptempreceive kt
	set kt.receipt_no = ( select ktr.receipt_no
							from kptempreceiptno ktr
							where ktr.coop_id = kt.coop_id
							and ktr.kpslip_no = kt.kpslip_no
							and ktr.coop_id = :is_coopcontrol
							and ktr.recv_period = :is_recvperiod
							)
	where kt.coop_id = :is_coopcontrol
	and kt.recv_period = :is_recvperiod
	using itr_sqlca;

	ls_sql = " delete from kptempreceiptno "
	ls_sql += " where coop_id = '" + is_coopcontrol + "' "
	ls_sql += " and recv_period = '" + is_recvperiod + "' "
	ls_sql += " and exists( select 1 from mbmembmaster "
	ls_sql += " where kptempreceiptno.coop_id = mbmembmaster.coop_id "
	ls_sql += " and kptempreceiptno.member_no = mbmembmaster.member_no "
	ls_sql += " and mbmembmaster.coop_id = '" + is_coopcontrol + "' "
	ls_sql += ls_sqlselect + " ) "
	
	execute immediate :ls_sql using itr_sqlca;

end if

/*
clr ใบเสร็จที่ไม่มี detail
*/
delete from kptempreceive 
where coop_id = :is_coopcontrol and recv_period = :is_recvperiod
and exists( select 1 from ( 	select k.coop_id , k.kpslip_no
									from kptempreceive k
									where k.coop_id = :is_coopcontrol
									and k.recv_period = :is_recvperiod
									minus
									select kd.coop_id , kd.kpslip_no
									from kptempreceivedet kd
									where kd.coop_id = :is_coopcontrol
									and kd.recv_period = :is_recvperiod
									group by kd.coop_id , kd.kpslip_no
									) kt
			where kt.coop_id = kptempreceive.coop_id
			and kt.kpslip_no = kptempreceive.kpslip_no
			)
using itr_sqlca ;

ls_sqlrecp	= " select nvl( kptempreceive.coop_id , '' ) as coop_id , nvl( kptempreceive.kpslip_no , '' ) as kpslip_no , nvl( kptempreceive.receipt_no , '' ) as receipt_no , "
ls_sqlrecp	+= " nvl( mbmembmaster.member_no , '' ) as member_no , nvl( mbmembmaster.membgroup_code , '' ) as membgroup_code , nvl( mbmembmaster.membtype_code , '' ) as membtype_code , "
ls_sqlrecp	+= " nvl( mbmembmaster.member_type , '' ) as member_type "
ls_sqlrecp	+= " from kptempreceive , mbmembmaster "
ls_sqlrecp	+= " where kptempreceive.coop_id = mbmembmaster.coop_id "
ls_sqlrecp	+= " and kptempreceive.member_no = mbmembmaster.member_no "
ls_sqlrecp	+= " and kptempreceive.coop_id = '" + is_coopcontrol + "' "
ls_sqlrecp	+= " and kptempreceive.recv_period = '" + is_recvperiod + "' "
ls_sqlrecp	+= ls_sqlselect

inv_dwsrv.of_create_dw( lds_recpno , ls_sqlrecp , '' )
lds_recpno.settransobject( itr_sqlca )

ll_count		= lds_recpno.retrieve( )
inv_progress.istr_progress.subprogress_max	= ll_count

/*Sort Data*/
choose case ls_recpsort
	case "MEM"
		lds_recpno.setsort( " member_no , kpslip_no " )
	case "GRP"
		lds_recpno.setsort( " membgroup_code , kpslip_no " )
	case "TYP"
		lds_recpno.setsort( " membtype_code , kpslip_no " )
	case "TKS"
		lds_recpno.setsort( " membgroup_code , membtype_code , member_type , member_no , kpslip_no " )
	case else
		lds_recpno.setsort( " member_no , kpslip_no " )
end choose
lds_recpno.sort()

for ll_index = 1 to ll_count
	
	inv_progress.istr_progress.subprogress_index	= ll_index
	
	// ตรวจสอบการสั่งหยุดทำงาน
	yield()
	if inv_progress.of_is_stop() then
		lb_err = true ; Goto objdestroy
	end if
	
	ls_kpslipno	= lds_recpno.object.kpslip_no[ll_index]
	
	// เลขที่ใบเสร็จ
	ls_docno		= string( ll_recpno, "000000" )
	ls_temp		= ls_prefix + ls_docno
	
	inv_progress.istr_progress.subprogress_text	= "ทะเบียน : " + ls_memno + " > เลขที่ใบเสร็จ " + ls_temp
	
	update kptempreceive
	set receipt_no	= :ls_temp
	where coop_id = :is_coopcontrol
	and kpslip_no = :ls_kpslipno
	using itr_sqlca ;
	if itr_sqlca.sqlcode <> 0 then
		if ii_msgerrflag = 1 then
			this.of_set_msgerr( "บันทึกข้อมูลใบเสร็จเลขใบทำรายการ >" + ls_kpslipno + " " + string( itr_sqlca.sqlcode ) + " < " + itr_sqlca.sqlerrtext )
		else
			ithw_exception.text = "<bf />บันทึกข้อมูลใบเสร็จ(70.01) พบข้อผิดพลาด"  
			ithw_exception.text += "~r~n<bf />"+ string( itr_sqlca.sqlcode )
			ithw_exception.text += "~r~n<bf />"+ itr_sqlca.sqlerrtext
			lb_err = true ; Goto objdestroy	
		end if
	end if
	
	ll_recpno ++

next

if is_coopcontrol = '010001' then
	
	insert into kptempreceiptno
	( coop_id , kpslip_no , recv_period , member_no , receipt_no )
	select kt.coop_id , kt.kpslip_no , kt.recv_period , kt.member_no , kt.receipt_no 
	from kptempreceive kt
	where kt.coop_id = :is_coopcontrol
	and kt.recv_period = :is_recvperiod
	and not exists( select 1 from kptempreceiptno ktr where ktr.coop_id = kt.coop_id and ktr.kpslip_no = kt.kpslip_no and ktr.coop_id = :is_coopcontrol and ktr.recv_period = :is_recvperiod )
	using itr_sqlca ;
	
end if

// บันทีกเลขที่ใบเสร็จล่าสุด
if not isnull( ll_recpno ) then
	update	cmdocumentcontrol
	set			last_documentno	= :ll_recpno
	where	( document_code	= 'KPRECEIPTNO' ) 
	and		( coop_id				= :is_coopid )
	using 		itr_sqlca;
	if itr_sqlca.sqlcode <> 0 then
		if ii_msgerrflag = 1 then
			this.of_set_msgerr( "บันทึกข้อมูลใบเสร็จล่าสุด >" + string( itr_sqlca.sqlcode ) + " < " + itr_sqlca.sqlerrtext )
		else
			ithw_exception.text = "<bf />บันทึกข้อมูลใบเสร็จล่าสุด(70.02) พบข้อผิดพลาด"  
			ithw_exception.text += "~r~n<bf />"+ string( itr_sqlca.sqlcode )
			ithw_exception.text += "~r~n<bf />"+ itr_sqlca.sqlerrtext
			lb_err = true ; Goto objdestroy			
		end if
	end if
end if

objdestroy:
if lb_err then
	ithw_exception.text = "n_cst_keeping_process.of_processrecpno() " + ithw_exception.text
	rollback using itr_sqlca ;
	if isvalid( ids_kptp ) then destroy ids_kptp
	throw ithw_exception
end if
if isvalid( lds_recpno ) then destroy lds_recpno

return 1
end function

protected function integer of_updateexp () throws Exception;/***********************************************************
<description>
	ประมวลค่าธรรมเนียมแรกเข้า
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
string		ls_memno, ls_membgroup, ls_desc, ls_keeptype, ls_temp, ls_sqlext
string		ls_memcoop , ls_refmem , ls_refmemcoop
string		ls_moneytype, ls_accid, ls_subgrp
string		ls_sqlupmem
long		ll_index, ll_count
integer		li_seqno, li_chk, li_stepvalue,li_keepperiod, li_ret, li_compoundkeep
dec{2}		ldc_feeamt, ldc_shrstk, ldc_shrbegin

str_extfn_keep lstr_extfn_keep

n_ds	lds_upmem

inv_progress.istr_progress.progress_text		= "ปรับปรุงข้อมูลสมาชิก"
inv_progress.istr_progress.subprogress_text	=  "กำลังดึงข้อมูล"
inv_progress.istr_progress.subprogress_index	= 0
inv_progress.istr_progress.subprogress_max	= 0

lds_upmem	= create n_ds
lds_upmem.settransobject( itr_sqlca )

lstr_extfn_keep.function_name	= 'of_kp_proc_getsql_memkpinfo'
this.of_extfn( lstr_extfn_keep )
ls_sqlupmem	= lstr_extfn_keep.function_return

inv_dwsrv.of_create_dw( lds_upmem , ls_sqlupmem , '' )

// สร้างชุด SQL สำหรับการดึงรายการ
li_ret = this.of_setsqlselect( lds_upmem )
if ( li_ret <> 1 ) then
	destroy lds_upmem ; destroy ids_kptp
	ithw_exception.text += "~nพบข้อผิดพลาดในการสร้างชุด SQL ปรับปรุงข้อมูลสมาชิก"
	throw ithw_exception
end if

ll_count	= lds_upmem.retrieve()
lds_upmem.setsort( ' coop_id , member_no ' )
lds_upmem.sort()

inv_progress.istr_progress.subprogress_max	= ll_count

for ll_index = 1 to ll_count
	inv_progress.istr_progress.subprogress_index	= ll_index

	// ตรวจสอบการสั่งหยุดทำงาน
	yield()
	if inv_progress.of_is_stop() then
		destroy lds_upmem ; destroy ids_kptp
		throw ithw_exception
	end if
	
	ls_memno		= trim( lds_upmem.getitemstring( ll_index, "member_no" ) )
	ls_memcoop 	= is_coopid
	ls_refmem 		= ''
	ls_refmemcoop	= ''

	inv_progress.istr_progress.subprogress_text	= string(ll_index) +". ทะเบียน "+ls_memno+" > ปรับปรุงข้อมูลสมาชิก " 
	
	update kptempreceive kt
	set ( kt.expense_accid ) =
	( 	select trim(mkp.expense_accid) as expense_accid 
		from mbmembmaster , 
						/*ดึงวิธีชำระยอดส่งหักประจำเดือนจาก mbmembmaster รวมกับ mbmembmoneytr*/
		( 
			select rank() over ( partition by member_no order by priority_amt desc ) as priority_no , coop_id , member_no , moneytype_code as expense_code , bank_code as expense_bank , bank_branch as expense_branch , bank_accid as expense_accid 
			from( 
				select 100 as priority_amt , mt.coop_id , mt.member_no , mt.moneytype_code , mt.bank_code , mt.bank_branch , mt.bank_accid 
				from mbmembmoneytr mt 
				where mt.coop_id =  :is_coopid
				and mt.member_no = :ls_memno
				and mt.trtype_code = 'KEEP1' 
				and mt.moneytype_code = 'CBT'
				group by mt.coop_id , mt.member_no , mt.moneytype_code , mt.bank_code , mt.bank_branch , mt.bank_accid 
				union 
				select 10 as priority_amt , m.coop_id , m.member_no , m.expense_code , m.expense_bank , m.expense_branch , m.expense_accid 
				from mbmembmaster m 
				where m.coop_id =  :is_coopid
				and m.member_no = :ls_memno
				and m.expense_code = 'CBT'
				group by m.coop_id , m.member_no , m.expense_code , m.expense_bank , m.expense_branch , m.expense_accid 
				) 
		)mkp , 
						/*สมาชิกส่งใบเสร็จรวมกับสมาชิกงดส่งใบเสร็จแต่ต้องการแสดงยอดคงเหลือหุ้นหนี้*/
		( 
			select coop_id , member_no 
			from( 
				select coop_id , member_no 
				from mbmembmaster 
				where coop_id = :is_coopid
				and member_no = :ls_memno
				and pausekeep_flag = 0 
				union 
				select m.coop_id , m.member_no 
				from mbmembmaster m , kpconstant kc 
				where m.coop_id = kc.coop_id 
				and m.coop_id = :is_coopid
				and m.member_no = :ls_memno
				and m.pausekeep_flag = 1 
				and kc.showsl_flag = 1 
				)mkc 
		)mem 
		where mbmembmaster.coop_id = :is_coopid
		and mbmembmaster.coop_id = mkp.coop_id 
		and mbmembmaster.member_no = mkp.member_no 
		and mbmembmaster.coop_id = mem.coop_id 
		and mbmembmaster.member_no = mem.member_no 
		and mbmembmaster.coop_id = kt.coop_id
		and mbmembmaster.member_no = kt.member_no
		and mbmembmaster.member_status = 1 
		and mkp.expense_code in ( 'CBT' )
		and mkp.priority_no = 1  )
	where kt.coop_id = :is_coopcontrol
	and kt.member_no = :ls_memno
	and kt.recv_period = :is_recvperiod
	and kt.expense_code = 'CBT'
	using itr_sqlca;
	
	if itr_sqlca.sqlcode = 0 then
		inv_progress.istr_progress.subprogress_text = string(ll_index) +". ทะเบียน "+ls_memno+" > ปรับปรุงข้อมูลสมาชิก"
	else
		if ii_msgerrflag = 1 then
			this.of_set_msgerr( "ปรับปรุงข้อมูลสมาชิก ทะเบียน : " + ls_memno + " > " + string( itr_sqlca.sqlcode ) + " < " + itr_sqlca.sqlerrtext )
		else
			ithw_exception.text = "ปรับปรุงข้อมูลสมาชิก พบข้อผิดพลาด"  
			ithw_exception.text += "~r~nทะเบียน : "+ ls_memno
			ithw_exception.text += "~r~n"+ string( itr_sqlca.sqlcode )
			ithw_exception.text += "~r~n"+ itr_sqlca.sqlerrtext
			destroy lds_upmem ; destroy ids_kptp
			throw ithw_exception
		end if
	end if
	
	update kpreceiveexpense kt
	set ( kt.expense_accid ) =
	( 	select trim(mkp.expense_accid) as expense_accid 
		from mbmembmaster , 
						/*ดึงวิธีชำระยอดส่งหักประจำเดือนจาก mbmembmaster รวมกับ mbmembmoneytr*/
		( 
			select rank() over ( partition by member_no order by priority_amt desc ) as priority_no , coop_id , member_no , moneytype_code as expense_code , bank_code as expense_bank , bank_branch as expense_branch , bank_accid as expense_accid 
			from( 
				select 100 as priority_amt , mt.coop_id , mt.member_no , mt.moneytype_code , mt.bank_code , mt.bank_branch , mt.bank_accid 
				from mbmembmoneytr mt 
				where mt.coop_id =  :is_coopid
				and mt.member_no = :ls_memno
				and mt.trtype_code = 'KEEP2' 
				and mt.moneytype_code = 'CBT'
				group by mt.coop_id , mt.member_no , mt.moneytype_code , mt.bank_code , mt.bank_branch , mt.bank_accid 
				union
				select 50 as priority_amt , mt.coop_id , mt.member_no , mt.moneytype_code , mt.bank_code , mt.bank_branch , mt.bank_accid 
				from mbmembmoneytr mt 
				where mt.coop_id =  :is_coopid
				and mt.member_no = :ls_memno
				and mt.trtype_code = 'KEEP1' 
				and mt.moneytype_code = 'CBT'
				group by mt.coop_id , mt.member_no , mt.moneytype_code , mt.bank_code , mt.bank_branch , mt.bank_accid 
				union 
				select 10 as priority_amt , m.coop_id , m.member_no , m.expense_code , m.expense_bank , m.expense_branch , m.expense_accid 
				from mbmembmaster m 
				where m.coop_id =  :is_coopid
				and m.member_no = :ls_memno
				and m.expense_code = 'CBT'
				group by m.coop_id , m.member_no , m.expense_code , m.expense_bank , m.expense_branch , m.expense_accid 
				) 
		)mkp , 
						/*สมาชิกส่งใบเสร็จรวมกับสมาชิกงดส่งใบเสร็จแต่ต้องการแสดงยอดคงเหลือหุ้นหนี้*/
		( 
			select coop_id , member_no 
			from( 
				select coop_id , member_no 
				from mbmembmaster 
				where coop_id = :is_coopid
				and member_no = :ls_memno
				and pausekeep_flag = 0 
				union 
				select m.coop_id , m.member_no 
				from mbmembmaster m , kpconstant kc 
				where m.coop_id = kc.coop_id 
				and m.coop_id = :is_coopid
				and m.member_no = :ls_memno
				and m.pausekeep_flag = 1 
				and kc.showsl_flag = 1 
				)mkc 
		)mem 
		where mbmembmaster.coop_id = :is_coopid
		and mbmembmaster.coop_id = mkp.coop_id 
		and mbmembmaster.member_no = mkp.member_no 
		and mbmembmaster.coop_id = mem.coop_id 
		and mbmembmaster.member_no = mem.member_no 
		and mbmembmaster.coop_id = kt.coop_id
		and mbmembmaster.member_no = kt.member_no
		and mbmembmaster.member_status = 1 
		and mkp.expense_code in ( 'CBT' )
		and mkp.priority_no = 1  )
	where kt.coop_id = :is_coopcontrol
	and kt.member_no = :ls_memno
	and kt.recv_period = :is_recvperiod
	and kt.expense_code = 'CBT'
	using itr_sqlca;
	
	if itr_sqlca.sqlcode = 0 then
		inv_progress.istr_progress.subprogress_text = string(ll_index) +".(2) ทะเบียน "+ls_memno+" > ปรับปรุงข้อมูลสมาชิก"
	else
		if ii_msgerrflag = 1 then
			this.of_set_msgerr( "ปรับปรุงข้อมูลสมาชิก ทะเบียน(2) : " + ls_memno + " > " + string( itr_sqlca.sqlcode ) + " < " + itr_sqlca.sqlerrtext )
		else
			ithw_exception.text = "ปรับปรุงข้อมูลสมาชิก(2) พบข้อผิดพลาด"  
			ithw_exception.text += "~r~nทะเบียน : "+ ls_memno
			ithw_exception.text += "~r~n"+ string( itr_sqlca.sqlcode )
			ithw_exception.text += "~r~n"+ itr_sqlca.sqlerrtext
			destroy lds_upmem ; destroy ids_kptp
			throw ithw_exception
		end if
	end if
	
next

destroy lds_upmem

return 1
end function

protected function integer of_processsharefromlncont () throws Exception;/***********************************************************
<description>
	ประมวลเรียกเก็บหุ้น
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
string		ls_memno, ls_memprior, ls_membgroup, ls_desc, ls_sqlext, ls_temp,ls_contno,ls_sql
string		ls_keeptype, ls_sharetype, ls_moneytype, ls_accid, ls_sqlshr
string		ls_memcoop , ls_refmem , ls_refmemcoop
long		ll_index, ll_count
integer	li_sharestatus, li_lastperiod, li_seqno, li_stepvalue, li_ret, li_compoundkeep
integer	li_pauseflag, li_paystatus , li_showflag
integer	li_compoundstatus, li_compoundpaystatus
dec{2}	ldc_periodamt, ldc_sharevalue, ldc_totalshrstk, ldc_shrbfvalue
dec{3}	ldc_shrbegin, ldc_shrstkamt
dec{2}	ldc_compoundamt, ldc_intaccum
boolean	lb_showonly

str_extfn_keep lstr_extfn_keep

n_ds	lds_sharecont

li_showflag		= ids_kpconstant.object.showsl_flag[1]

inv_progress.istr_progress.progress_text		= "ส่งหุ้นเพิ่มรายเดือนจากเงินกู้"
inv_progress.istr_progress.subprogress_text	= "กำลังดึงข้อมูล"
inv_progress.istr_progress.subprogress_index	= 0
inv_progress.istr_progress.subprogress_max	= 0


lds_sharecont	= create n_ds
lds_sharecont.settransobject( itr_sqlca )

lstr_extfn_keep.function_name	= 'of_kp_proc_getsql_sharefromcontmast'
this.of_extfn( lstr_extfn_keep )
ls_sqlshr	= lstr_extfn_keep.function_return

inv_dwsrv.of_create_dw( lds_sharecont , ls_sqlshr , '' )

// สร้างชุด SQL สำหรับการดึงรายการ
li_ret = this.of_setsqlselect( lds_sharecont )
if ( li_ret <> 1 ) then
	ls_sql = lds_sharecont.getsqlselect()
	destroy lds_sharecont
	ithw_exception.text += "~nพบข้อผิดพลาดในการสร้างชุด SQL รายการค่าหุ้นเก็บเพิ่มจากสัญญาเงินกู้" + ls_sql
	throw ithw_exception	
end if

ll_count	= lds_sharecont.retrieve()
//lds_sharecontcont.setsort( ' shsharemaster_coop_id , mbmembmaster_member_no ' )
lds_sharecont.setsort( " coop_id , member_no , member_ref desc " )
lds_sharecont.sort()

inv_progress.istr_progress.subprogress_max	= ll_count

for ll_index = 1 to ll_count
	
	inv_progress.istr_progress.subprogress_index	= ll_index
	
	// ตรวจสอบการสั่งหยุดทำงาน
	yield()
	if inv_progress.of_is_stop() then
		destroy lds_sharecont
		return 0
	end if
	try
		ls_memno			= trim( lds_sharecont.getitemstring( ll_index, "member_no" ) )
		ls_sharetype		= '01' // trim( lds_sharecont.getitemstring( ll_index, "sharetype_code" ) )
		ls_memcoop 		= is_coopcontrol
		ls_refmem 			=  ls_memno //lds_sharecont.object.member_ref[ ll_index ]
		ls_refmemcoop		= is_coopcontrol
		li_sharestatus		= 1 //lds_sharecont.getitemnumber( ll_index, "sharemaster_status" )
		li_lastperiod			= 0 //lds_sharecont.getitemnumber( ll_index, "last_period" )
		li_pauseflag			= 0 // lds_sharecont.getitemnumber( ll_index, "pausekeep_flag" )
		li_paystatus			=  1 //lds_sharecont.getitemnumber( ll_index, "payment_status" )
		ldc_periodamt		= lds_sharecont.getitemdecimal( ll_index, "KEEPSHAREPERIOD_AMT" )
		ldc_shrstkamt		= lds_sharecont.getitemdecimal( ll_index, "sharestk_amt" )
		ldc_shrbegin		= 0// lds_sharecont.getitemdecimal( ll_index, "sharebegin_amt" )
		ldc_sharevalue		= 0 // lds_sharecont.getitemdecimal( ll_index, "unitshare_value" )
		ls_contno 			= lds_sharecont.object.loancontract_no[ ll_index ]
		lb_showonly			= false
	catch( Exception lthw_get)
	ls_sql = lds_sharecont.getsqlselect()
		ithw_exception.text		= "กำหนดการอ่านค่าแต่ฟิลด์ผิด : " + lthw_get.text + '  sqlll ' + ls_sql 
		throw ithw_exception
	end try
	
	if isnull( li_lastperiod ) then li_lastperiod = 0
	if isnull( ldc_shrstkamt ) then ldc_shrstkamt = 0

	// กำหนดค่าต่าง ๆ
	if lb_showonly then
		ls_keeptype		= "DPS"
		ls_desc			= "ค่าหุ้น"
		ldc_periodamt	= 0
	else
		ls_keeptype		= "S"+ls_sharetype
		ls_desc			= "ค่าหุ้นจาก" + ls_contno
		ldc_periodamt	= ldc_periodamt * ldc_sharevalue
		
		li_lastperiod ++
	end if
	
	if ( isnull( ldc_periodamt ) or ldc_periodamt = 0 ) and not lb_showonly then
		this.of_set_msgerr( "ไม่พบข้อมูลเรียกเก็บค่าหุ้น ทะเบียน : " + ls_memno )
		continue ;
	end if
	
	ldc_shrstkamt	= ldc_shrstkamt * ldc_sharevalue
	ldc_totalshrstk	= ( ldc_shrstkamt + ldc_periodamt )
	
	// ปรับปรุงดึงข้อมูลที่ต้องการจาก buffer datastore
//	is_kpfind		= " coop_id = '"+is_coopid+"' and recv_period = '"+is_recvperiod+"' and member_no = '"+ls_memno+"' and memcoop_id = '"+ls_memcoop+"' and refmemcoop_id = '"+ls_refmemcoop+"' and ref_membno = '"+ls_refmem+"' "
	is_kpfind		= " coop_id = '"+is_coopid+"' and recv_period = '"+is_recvperiod+"' and member_no = '"+ls_memno+"' and memcoop_id = '"+ls_memcoop+"' "
	il_kpfind		= ids_kptp.find( is_kpfind , il_kpfind , il_kpcount )
	if il_kpfind <= 0 then 
		il_kpfind		= ids_kptp.find( is_kpfind , 1 , il_kpcount )
		
	end if
	try
		is_kpslipno	= ids_kptp.object.kpslip_no[il_kpfind]
		li_seqno		= ids_kptp.object.last_seq_no[il_kpfind] ; if isnull( li_seqno ) then li_seqno = 0
	catch( Exception lthw_errseq)
		ithw_exception.text		= "เลขสมาชิกไม่พบข้อมูลใบเสร็จประจำเดือน : " + ls_memno
		throw ithw_exception
	end try
	li_seqno ++
	ids_kptp.object.last_seq_no[il_kpfind]		= li_seqno
	ids_kptp.object.sharestk_value[il_kpfind]	= ldc_totalshrstk
	//จบการปรับปรุง

	inv_progress.istr_progress.subprogress_text	= string(ll_index) +". ทะเบียน "+ls_memno+" > หุ้นรายเดือน "+string(ldc_periodamt,"#,##0.00")+" > ทุนเรือนหุ้น "+string(ldc_shrstkamt,"#,##0.00")

	// insert ข้อมูลลง รายการเรียกเก็บ
	insert into kptempreceivedet
			( 	coop_id , 					memcoop_id,			refmemcoop_id,		kpslip_no,
				member_no, 				recv_period, 			ref_membno, 			seq_no, 
				keepitemtype_code, 		shrlontype_code, 		description,			  	period, 
				item_payment, 				item_balance, 			posting_status, 		keepitem_status,
				bizzcoop_id,					bfcontract_status ,	bfcontlaw_status )
	values( 	:is_coopid ,					:ls_memcoop ,			:ls_refmemcoop ,		:is_kpslipno,
				:ls_memno, 				:is_recvperiod, 			:ls_refmem, 			:li_seqno, 
				:ls_keeptype, 				:ls_sharetype, 			:ls_desc,			  		:li_lastperiod, 
				:ldc_periodamt, 			:ldc_totalshrstk, 		0, 							1,
				:ls_memcoop ,				:li_sharestatus ,		:li_sharestatus )
	using	itr_sqlca  ;

	if itr_sqlca.sqlcode <> 0 then
		if ii_msgerrflag = 1 then
			this.of_set_msgerr("ประมวลผลหุ้น ทะเบียน : " + ls_memno + " > " + string( itr_sqlca.sqlcode ) + " < " + itr_sqlca.sqlerrtext )
		else
			ithw_exception.text = "ประมวลผลหุ้น พบข้อผิดพลาด" 
			ithw_exception.text += "~r~nทะเบียน : "+ ls_memno
			ithw_exception.text += "~r~n"+ string( itr_sqlca.sqlcode ) 
			ithw_exception.text += "~r~n"+ itr_sqlca.sqlerrtext 
			destroy lds_sharecont ; destroy ids_kptp
			return -1
		end if
	end if
	
	if not lb_showonly then
		//  บันทึกยอดเงินค่าหุ้นรอเรียกเก็บ
		update	shsharemaster
		set		rkeep_sharevalue	= rkeep_sharevalue + :ldc_periodamt,
					lastprocess_date	= :idtm_receipt
		where	( sharetype_code	= :ls_sharetype ) 
		and		( coop_id 			= :is_coopid )
		and		( member_no 		= :ls_memno )
		using itr_sqlca;

		if itr_sqlca.sqlcode <> 0 then
			if ii_msgerrflag = 1 then
				this.of_set_msgerr("ไม่สามารถอัพเดทข้อมูลหุ้นรอเรียกเก็บได้ ทะเบียน : " + ls_memno + " > " + string( itr_sqlca.sqlcode ) + " < " + itr_sqlca.sqlerrtext )
			else
				ithw_exception.text = "ประมวลผลหุ้น พบข้อผิดพลาด" 
				ithw_exception.text += "~r~nไม่สามารถอัพเดทข้อมูลหุ้นรอเรียกเก็บได้"
				ithw_exception.text += "~r~nทะเบียน : " + ls_memno
				ithw_exception.text += "~r~n"+ string( itr_sqlca.sqlcode ) 
				ithw_exception.text += "~r~n"+ itr_sqlca.sqlerrtext 
				destroy lds_sharecont ; destroy ids_kptp
				return -1
			end if
		end if
		
	end if
next

destroy lds_sharecont

return 1
end function

protected function integer of_processfeehnd () throws Exception;/***********************************************************
<description>
	ประมวลเรียกเก็บหุ้น
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
string		ls_memno, ls_memprior, ls_membgroup, ls_desc, ls_sqlext, ls_temp,ls_contno
string		ls_keeptype, ls_sharetype, ls_moneytype, ls_accid, ls_sqlshr
string		ls_memcoop , ls_refmem , ls_refmemcoop
long		ll_index, ll_count
integer	li_sharestatus, li_lastperiod, li_seqno, li_stepvalue, li_ret, li_compoundkeep
integer	li_pauseflag, li_paystatus , li_showflag
integer	li_compoundstatus, li_compoundpaystatus
dec{2}	ldc_periodamt, ldc_sharevalue, ldc_totalshrstk, ldc_shrbfvalue
dec{3}	ldc_shrbegin, ldc_shrstkamt
dec{2}	ldc_compoundamt, ldc_intaccum
boolean	lb_showonly

str_extfn_keep lstr_extfn_keep

n_ds	lds_feemisspay

li_showflag		= ids_kpconstant.object.showsl_flag[1]

inv_progress.istr_progress.progress_text		= "เรียกเก็บค่าปรับกรณีเก็บไม่ได้"
inv_progress.istr_progress.subprogress_text	= "กำลังดึงข้อมูล"
inv_progress.istr_progress.subprogress_index	= 0
inv_progress.istr_progress.subprogress_max	= 0

lds_feemisspay	= create n_ds
lds_feemisspay.settransobject( itr_sqlca )

lstr_extfn_keep.function_name	= 'of_get_fee_hnd'
this.of_extfn( lstr_extfn_keep )
ls_sqlshr	= lstr_extfn_keep.function_return

inv_dwsrv.of_create_dw( lds_feemisspay , ls_sqlshr , '' )

// สร้างชุด SQL สำหรับการดึงรายการ
li_ret = this.of_setsqlselect( lds_feemisspay )
if ( li_ret <> 1 ) then
	destroy lds_feemisspay
	ithw_exception.text += "~nพบข้อผิดพลาดในการสร้างชุด SQL รายการค่าหุ้น"
	throw ithw_exception	
end if

ll_count	= lds_feemisspay.retrieve()
//lds_feemisspay.setsort( ' shsharemaster_coop_id , mbmembmaster_member_no ' )
lds_feemisspay.setsort( " coop_id , member_no , member_ref desc " )
lds_feemisspay.sort()

inv_progress.istr_progress.subprogress_max	= ll_count

for ll_index = 1 to ll_count
	
	inv_progress.istr_progress.subprogress_index	= ll_index
	
	// ตรวจสอบการสั่งหยุดทำงาน
	yield()
	if inv_progress.of_is_stop() then
		destroy lds_feemisspay
		return 0
	end if
	
	ls_memno			= trim( lds_feemisspay.getitemstring( ll_index, "member_no" ) )
	ls_memcoop 		= is_coopcontrol
	lb_showonly			= false
	
	
	if isnull( li_lastperiod ) then li_lastperiod = 0
	if isnull( ldc_shrstkamt ) then ldc_shrstkamt = 0

	// กำหนดค่าต่าง ๆ
		ls_sharetype 	= "00"
		ls_keeptype		= "FMP"
		ls_desc			= "ค่าปรับ"
		ldc_periodamt	= 200.00
		
	
	// ปรับปรุงดึงข้อมูลที่ต้องการจาก buffer datastore
//	is_kpfind		= " coop_id = '"+is_coopid+"' and recv_period = '"+is_recvperiod+"' and member_no = '"+ls_memno+"' and memcoop_id = '"+ls_memcoop+"' and refmemcoop_id = '"+ls_refmemcoop+"' and ref_membno = '"+ls_refmem+"' "
	is_kpfind		= " coop_id = '"+is_coopid+"' and recv_period = '"+is_recvperiod+"' and member_no = '"+ls_memno+"' and memcoop_id = '"+ls_memcoop+"' "
	il_kpfind		= ids_kptp.find( is_kpfind , il_kpfind , il_kpcount )
	if il_kpfind <= 0 then 
		il_kpfind		= ids_kptp.find( is_kpfind , 1 , il_kpcount )
		
	end if
	try
		is_kpslipno	= ids_kptp.object.kpslip_no[il_kpfind]
		li_seqno		= ids_kptp.object.last_seq_no[il_kpfind] ; if isnull( li_seqno ) then li_seqno = 0
	catch( Exception lthw_errseq)
		ithw_exception.text		= "เลขสมาชิกไม่พบข้อมูลใบเสร็จประจำเดือน : " + ls_memno
		throw ithw_exception
	end try
	li_seqno ++
	ids_kptp.object.last_seq_no[il_kpfind]		= li_seqno
	//จบการปรับปรุง

	inv_progress.istr_progress.subprogress_text	= string(ll_index) +". ทะเบียน "+ls_memno+" > ค่าปรับรายเดือน "+string(ldc_periodamt,"#,##0.00")

	// insert ข้อมูลลง รายการเรียกเก็บ
	insert into kptempreceivedet
			( 	coop_id , 					memcoop_id,			refmemcoop_id,		kpslip_no,
				member_no, 				recv_period, 			ref_membno, 			seq_no, 
				keepitemtype_code, 		shrlontype_code, 		description,			  	period, 
				item_payment, 				item_balance, 			posting_status, 		keepitem_status,
				bizzcoop_id,					bfcontract_status ,	bfcontlaw_status )
	values( 	:is_coopid ,					:is_coopid ,			:is_coopid ,		:is_kpslipno,
				:ls_memno, 				:is_recvperiod, 			:ls_memno, 			:li_seqno, 
				:ls_keeptype, 				:ls_sharetype, 			:ls_desc,			  		:li_lastperiod, 
				:ldc_periodamt, 			:ldc_totalshrstk, 		0, 							1,
				:ls_memcoop ,				:li_sharestatus ,		:li_sharestatus )
	using	itr_sqlca  ;

	if itr_sqlca.sqlcode <> 0 then
		if ii_msgerrflag = 1 then
			this.of_set_msgerr("ประมวลผลหุ้น ทะเบียน : " + ls_memno + " > " + string( itr_sqlca.sqlcode ) + " < " + itr_sqlca.sqlerrtext )
		else
			ithw_exception.text = "ประมวลผลหุ้น พบข้อผิดพลาด" 
			ithw_exception.text += "~r~nทะเบียน : "+ ls_memno
			ithw_exception.text += "~r~n"+ string( itr_sqlca.sqlcode ) 
			ithw_exception.text += "~r~n"+ itr_sqlca.sqlerrtext 
			destroy lds_feemisspay ; destroy ids_kptp
			return -1
		end if
	end if
	
	if not lb_showonly then
		//  บันทึกยอดเงินค่าหุ้นรอเรียกเก็บ
		update	shsharemaster
		set		rkeep_sharevalue	= :ldc_periodamt,
					lastprocess_date	= :idtm_receipt
		where	( sharetype_code	= :ls_sharetype ) 
		and		( coop_id 			= :is_coopid )
		and		( member_no 		= :ls_memno )
		using itr_sqlca;

		if itr_sqlca.sqlcode <> 0 then
			if ii_msgerrflag = 1 then
				this.of_set_msgerr("ไม่สามารถอัพเดทข้อมูลหุ้นรอเรียกเก็บได้ ทะเบียน : " + ls_memno + " > " + string( itr_sqlca.sqlcode ) + " < " + itr_sqlca.sqlerrtext )
			else
				ithw_exception.text = "ประมวลผลหุ้น พบข้อผิดพลาด" 
				ithw_exception.text += "~r~nไม่สามารถอัพเดทข้อมูลหุ้นรอเรียกเก็บได้"
				ithw_exception.text += "~r~nทะเบียน : " + ls_memno
				ithw_exception.text += "~r~n"+ string( itr_sqlca.sqlcode ) 
				ithw_exception.text += "~r~n"+ itr_sqlca.sqlerrtext 
				destroy lds_feemisspay ; destroy ids_kptp
				return -1
			end if
		end if
		
	end if
next

destroy lds_feemisspay

return 1
end function

on n_cst_keeping_process.create
call super::create
TriggerEvent( this, "constructor" )
end on

on n_cst_keeping_process.destroy
TriggerEvent( this, "destructor" )
call super::destroy
end on

event constructor;ithw_exception = create Exception
end event

event destructor;if isvalid( ithw_exception ) then destroy ithw_exception

if isvalid( inv_stringsrv ) then destroy inv_stringsrv
if isvalid( inv_progress ) then destroy inv_progress
if isvalid( inv_intsrv ) then destroy inv_intsrv
if isvalid( inv_datetimesrv ) then destroy inv_datetimesrv
if isvalid( inv_dwsrv ) then destroy inv_dwsrv
if isvalid( inv_transection ) then destroy inv_transection

if isvalid( ids_contintspc ) then destroy ids_contintspc
if isvalid( ids_loandata ) then destroy ids_loandata
if isvalid( ids_inttable ) then destroy ids_inttable
if isvalid( ids_kptp ) then destroy ids_kptp
if isvalid( ids_msgerr ) then destroy ids_msgerr
end event
