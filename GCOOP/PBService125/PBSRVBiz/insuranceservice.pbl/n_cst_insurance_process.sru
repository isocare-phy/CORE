$PBExportHeader$n_cst_insurance_process.sru
forward
global type n_cst_insurance_process from nonvisualobject
end type
end forward

global type n_cst_insurance_process from nonvisualobject
end type
global n_cst_insurance_process n_cst_insurance_process

type variables

n_ds		ids_loandata, ids_contintspc, ids_inttable
n_ds		ids_constant
n_cst_dbconnectservice		inv_transection
n_cst_loansrv_interest		inv_intsrv
n_cst_progresscontrol		inv_progress
n_cst_dwxmlieservice				inv_dwxmliesrv
transaction	itr_sqlca
Exception	ithw_exception
end variables

forward prototypes
public function string of_init_force (string as_loantype, boolean ab_new, datetime adtm_str, datetime adtm_end)
public function integer of_initservice (n_cst_dbconnectservice atr_dbtrans)
public function integer of_xmlimport (n_ds ads_info, string as_dwobjname, string as_xmlinfo)
public function string of_xmlexport (n_ds ads_info)
public function string of_get_insreceive (string as_period)
public function integer of_init_ins_loan (ref string as_xmldatacri, ref string as_xmdataloan)
public function integer of_init_insloandetail (ref string as_xmlinsmain, ref string as_xmlinslist, ref string as_xmldata, ref string as_xmlloan, ref string as_xmlinsstmt, long al_insgroupid, string as_memno)
public function integer of_rowchang_insloandetail (ref str_insppndetail astr_insloandetail)
public function integer of_ins_processinstallfromloan (string as_xmldatacri, string as_xmlloandata) throws Exception
public function integer of_setsrvdwxmlie (boolean ab_switch)
public function integer of_ins_processenlarge (string as_xmlcri, datetime adtm_operate) throws Exception
public function integer of_setprogress (ref n_cst_progresscontrol anv_progress) throws Exception
public function integer of_ins_processtoinsyearbf (string as_xmlcri) throws Exception
public function integer of_ins_postdivavgtoinsmast (string as_xmlcri) throws Exception
public function integer of_init_insloandetail (ref str_insppndetail astr_insloandetail) throws Exception
public function integer of_ins_processcancleinstallfromloan (string as_xmldatacri, string as_xmlloandata) throws Exception
public function integer of_ins_cancleprocessenlarge (string as_xmlcri, datetime adtm_operate) throws Exception
public function integer of_ins_updateinsloandetail (ref str_insppndetail astr_insloandetail) throws Exception
public function integer of_postupdate_insloandetail (ref string as_xmlinsmain, ref string as_xmlinslist, ref string as_xmldata, ref string as_xmlloan, ref string as_xmlinsstmt, long al_insgroupid, string as_memno) throws Exception
public function integer of_ins_procdivavgtoinsmast (string as_xmlcri) throws Exception
end prototypes

public function string of_init_force (string as_loantype, boolean ab_new, datetime adtm_str, datetime adtm_end);
string		ls_return
long		ll_currentrow , ll_rowcount , ll_insid
string		ls_membno , ls_prename , ls_name , ls_sname , ls_insname , ls_level
dec{2}	ldc_loan , ldc_extra , ldc_insblanc , ldc_insamt , ldc_ins
datetime	ldtm_approve

n_ds		lds_insforce
lds_insforce	= create n_ds
lds_insforce.dataobject	= "d_ins_forcenew_datastore"
//lds_insforce.settransobject( itr_sqlca )

n_ds		lds_loandata
lds_loandata	= create n_ds
lds_loandata.dataobject	= "d_ins_force_loan"
lds_loandata.settransobject( itr_sqlca )

ll_rowcount	= lds_loandata.retrieve( adtm_str , adtm_end , as_loantype )

if ll_rowcount > 0 then
	SELECT	MAX( INSGROUP_ID )
	INTO		:ll_insid
	FROM		INSGROUPMASTER
	;
end if

for ll_currentrow = 1 to ll_rowcount
	
	ls_membno		= trim( lds_loandata.getitemstring( ll_currentrow , "member_no" ))
	ls_prename		= trim( lds_loandata.getitemstring( ll_currentrow , "prename_desc" ))
	ls_name			= trim( lds_loandata.getitemstring( ll_currentrow , "memb_name" ))
	ls_sname		= trim( lds_loandata.getitemstring( ll_currentrow , "memb_surname" ))
	ldc_loan			= lds_loandata.getitemdecimal( ll_currentrow , "loanapprove_amt" )
	ldc_extra			= lds_loandata.getitemdecimal( ll_currentrow , "extraloan_amt" )
	ldtm_approve	= lds_loandata.getitemdatetime( ll_currentrow , "startcont_date" )
	
	if isNull(ldc_extra) then ldc_extra = 0
	
	if ldc_ins < ( ldc_loan + ldc_extra ) then
		// PK
		ll_insid			= ll_insid + 1
		
		ls_insname		= ls_prename + ls_name + "   " + ls_sname
			
		SELECT	MIN( MAXINSCOST_AMT )
		INTO		:ldc_insblanc
		FROM		INSLEVELCOST
		WHERE	MAXINSCOST_AMT >= :ldc_loan
			AND	INSTYPE_CODE = '01'
		;
		
		SELECT	LEVEL_CODE , INSPERIOD_PAYMENT
		INTO		:ls_level , :ldc_insamt
		FROM		INSLEVELCOST
		WHERE	MAXINSCOST_AMT = :ldc_insblanc
			AND	INSTYPE_CODE = '01'
		;
		
		INSERT INTO INSGROUPMASTER(
			INSGROUP_ID,				MEMBER_NO,					INSGROUPDOC_NO,				INSTYPE_CODE,
			LEVEL_CODE,				MARRIGE_NAME,				INSCOST_BLANCE,				INSPAYMENT_AMT,
			INSGROUP_DATE,			INSREQ_DATE,					INSMEMB_STATUS,				INSMEMB_TYPE,
			STARTSAFE_DATE,		INSPEROD_PAYMENT,			APPLTYPE_CODE )
		VALUES (
			:ll_insid ,						:ls_membno ,					'-',										'01',
			:ls_level ,					:ls_insname,					:ldc_insblanc,						:ldc_insamt,
			:ldtm_approve,				:ldtm_approve,					'1',										'1',
			:ldtm_approve,				:ldc_insamt,						'03' )
		;
	end if
	
next

commit;

return		ls_return
end function

public function integer of_initservice (n_cst_dbconnectservice atr_dbtrans);/***********************************************************
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

if isnull( inv_transection ) or not isvalid( inv_transection ) then
	inv_transection = create n_cst_dbconnectservice
	inv_transection = atr_dbtrans
end if
itr_sqlca = atr_dbtrans.itr_dbconnection

// สร้าง Progress สำหรับแสดงสถานะการประมวลผล
inv_progress = create n_cst_progresscontrol

return 1
end function

public function integer of_xmlimport (n_ds ads_info, string as_dwobjname, string as_xmlinfo);/***********************************************************
<description>
	Import Xml
</description>

<arguments>  
	ads_info				n_ds		datastore ที่จะ import ข้อมูลเข้า
	as_dwobjname		String		ชื่อ dw object
	as_xmlinfo			String		ข้อมูลในรูปแบบ xml ที่ต้องการ import
</arguments> 

<return> 
	Integer	 	จำนวนแถวที่ import ได้
					-1   No rows or startrow value supplied is greater than the number of rows in the string
					-3   Invalid argument
					-4   Invalid input
					-11   XML Parsing Error; XML parser libraries not found or XML not well formed
					-12   XML Template does not exist or does not match the DataWindow
					-13    Unsupported DataWindow style for import
					-14    Error resolving DataWindow nesting
</return> 

<usage> 
	Revision History:
	Version 1.0 (Initial version) Modified Date 13/10/2010 by MiT
</usage> 
***********************************************************/
integer	li_row

if not isvalid( ads_info ) or isnull( ads_info ) then
	ads_info = create n_ds
end if

ads_info.dataobject = as_dwobjname
ads_info.settransobject( itr_sqlca )

if ( as_xmlinfo = "" ) then return 0

li_row = ads_info.importstring( XML!, as_xmlinfo )

return li_row
end function

public function string of_xmlexport (n_ds ads_info);/***********************************************************
<description>
	Export xml เข้าสู่ datastore
</description>

<arguments>  
	ads_info				n_ds		datastore ที่จะ export ข้อมูล
</arguments> 

<return> 
	String		 	xml ที่ export ได้
</return> 

<usage> 
	Revision History:
	Version 1.0 (Initial version) Modified Date 13/10/2010 by MiT
</usage> 
***********************************************************/
string		ls_xml

if ( ads_info.rowcount() > 0 ) then
	ls_xml = string( ads_info.describe( "Datawindow.data.XML" ) )
else
	ls_xml = ""
end if

return ls_xml
end function

public function string of_get_insreceive (string as_period);
string		ls_return

n_ds		lds_recv
lds_recv	= create n_ds
lds_recv.dataobject	= "d_ins_receive_master"
lds_recv.settransobject( itr_sqlca )

long		ll_currentrow , ll_rowcount

ll_rowcount		= lds_recv.retrieve( as_period )

ls_return			= this.of_xmlexport( lds_recv )

return ls_return
end function

public function integer of_init_ins_loan (ref string as_xmldatacri, ref string as_xmdataloan);//ตั้งค่า init หน้าจอ ติดตั้งประกันชีวิต ปปน พคช
string ls_loantypecode ,ls_instypecode, ls_startgroup, ls_endgroup,ls_instype
datetime ldtm_start, ldtm_end
long ll_row, ll_count,li_rowcount
n_ds lds_cri, lds_loandata
lds_cri = create n_ds
lds_cri.dataobject = "d_pk_process_loan_cri"
lds_cri.settransobject( itr_sqlca )
li_rowcount = lds_cri.importstring( XML!, as_xmldatacri )

lds_loandata = create n_ds
lds_loandata.dataobject = "d_pk_ppn_egat_loan_list_process"
lds_loandata.settransobject( itr_sqlca )

ls_instype = lds_cri.getitemstring( 1,"instype_code")
ls_loantypecode = lds_cri.getitemstring( 1,"loantype_code")
ldtm_start = lds_cri.getitemdatetime(1,"start_date")
ldtm_end = lds_cri.getitemdatetime(1,"end_date")

ll_count = lds_loandata.retrieve(ls_loantypecode,ldtm_start, ldtm_end)
as_xmdataloan = this.of_xmlexport( lds_loandata )
return ll_count
end function

public function integer of_init_insloandetail (ref string as_xmlinsmain, ref string as_xmlinslist, ref string as_xmldata, ref string as_xmlloan, ref string as_xmlinsstmt, long al_insgroupid, string as_memno);string ls_memno, ls_instype
long ll_row, ll_count, ll_insgroupid,li_rowcount
n_ds lds_main, lds_inslist, lds_insdata, lds_insloan, lds_insstmt

lds_main = create n_ds
lds_main.dataobject = "d_sk_member_detail"
lds_main.settransobject( itr_sqlca )
lds_main.importstring( XML!, as_xmlinsmain )
li_rowcount =  lds_main.retrieve( as_memno )
as_xmlinsmain = this.of_xmlexport( lds_main )
lds_inslist = create n_ds
lds_inslist.dataobject = "d_pk_ppn_egat_list"
lds_inslist.settransobject( itr_sqlca )
lds_inslist.importstring( XML!, as_xmlinslist )
li_rowcount = lds_inslist.retrieve( as_memno )
as_xmlinslist = this.of_xmlexport( lds_inslist )
lds_insdata = create n_ds
lds_insdata.dataobject = "d_pk_ppn_egat_main"
lds_insdata.settransobject( itr_sqlca )
lds_insdata.importstring( XML!, as_xmldata )
li_rowcount =  lds_insdata.retrieve( al_insgroupid )
as_xmldata = this.of_xmlexport( lds_insdata )
lds_insloan = create n_ds
lds_insloan.dataobject = "d_ins_prakun_loan_detail"
lds_insloan.settransobject( itr_sqlca )
lds_insloan.importstring( XML!, as_xmlloan )
li_rowcount = lds_insloan.retrieve( as_memno )
as_xmlloan = this.of_xmlexport( lds_insloan )
lds_insstmt = create n_ds
lds_insstmt.dataobject = "d_pk_ppn_egat_statement"
lds_insstmt.settransobject( itr_sqlca )
lds_insstmt.importstring( XML!, as_xmlinsstmt )
li_rowcount = lds_insstmt.retrieve( as_memno )
as_xmlinsstmt = this.of_xmlexport( lds_insstmt )
return 1
end function

public function integer of_rowchang_insloandetail (ref str_insppndetail astr_insloandetail);/*
หน้าจอรายละเอียดประกัน
of_rowchang_insloandetail แสดงข้อมูลกรณีมีการเลือกแถวรายการประกันใหม่ 
*/
string ls_memno, ls_instype,ls_insgroupid,ls_xmlcri, ls_xmlmain,ls_xmldata, ls_xmlloan, ls_xmlstmt,ls_xmllist
long ll_row, ll_count, ll_insgroupid,li_rowcount, al_insgroupid
n_ds lds_main, lds_inslist, lds_insdata, lds_insloan, lds_insstmt
str_insppndetail  lstr_inspandet

ls_xmlmain = astr_insloandetail.as_xmlinsmain
ls_xmllist = astr_insloandetail.as_xmlloanlist
ls_xmldata = astr_insloandetail.as_xmldetail
ls_xmlloan = astr_insloandetail.as_xmlloan
ls_xmlstmt = astr_insloandetail.as_xmlstatement

lds_main = create n_ds
lds_main.dataobject = "d_sk_member_detail"
lds_main.settransobject( itr_sqlca )
lds_main.importstring( XML!, ls_xmlmain )

ls_memno = lds_main.getitemstring(1,"member_no")
li_rowcount =  lds_main.retrieve( ls_memno )
ls_xmlmain = this.of_xmlexport( lds_main )

lds_inslist = create n_ds
lds_inslist.dataobject = "d_pk_ppn_egat_list"
lds_inslist.settransobject( itr_sqlca )

lds_inslist.importstring( XML!, ls_xmllist )
li_rowcount = lds_inslist.rowcount(  )
ls_xmllist = this.of_xmlexport( lds_inslist )
ll_row = lds_inslist.SelectRow(1,true)
if ll_row > 0 then
	ll_insgroupid = lds_inslist.getitemnumber( ll_row,"insgroup_id")
end if
if ll_row > 0 then 
	lds_insdata = create n_ds
	lds_insdata.dataobject = "d_pk_ppn_egat_main"
	lds_insdata.settransobject( itr_sqlca )
	lds_insdata.importstring( XML!, ls_xmldata )
	li_rowcount =  lds_insdata.retrieve( al_insgroupid )
	ls_xmldata = this.of_xmlexport( lds_insdata )
	
	lds_insstmt = create n_ds
	lds_insstmt.dataobject = "d_pk_ppn_egat_statement"
	lds_insstmt.settransobject( itr_sqlca )
	lds_insstmt.importstring( XML!, ls_xmlstmt )
	li_rowcount = lds_insstmt.retrieve( al_insgroupid )
	ls_xmllist = this.of_xmlexport( lds_insstmt )
end if
astr_insloandetail.as_xmlinsmain = ls_xmlmain
astr_insloandetail.as_xmlloanlist = ls_xmllist
astr_insloandetail.as_xmldetail = ls_xmldata
astr_insloandetail.as_xmlloan = ls_xmlloan
astr_insloandetail.as_xmlstatement = ls_xmlstmt

return 1
end function

public function integer of_ins_processinstallfromloan (string as_xmldatacri, string as_xmlloandata) throws Exception;/*
of_ins_processinstallfromloan  ติดตั้งประกันชีวิตจากเงินกู้
as_xmldatacri  = เงือนไขในการติดตั้ง
s_xmlloandata = ข้อมูลเงินกู้
*/
string ls_memno, ls_contno, ls_insgroupdocno, ls_instypecode,ls_loantypecode, ls_insdocno,ls_cardperson, ls_userid
dec{2} ldc_loanapp, ldc_inspayment, ldc_periodpay
dec{4} ldc_percent = 0.075
long ll_row,ll_index, ll_rowcount, ll_insgroupid
datetime ldtm_startcont, ldtm_endsafe
integer li_membtype, li_period, li_chkins

string ls_loantype,   ls_receiptno
long  ll_count
integer  li_rowcount
n_ds lds_loandata
n_ds lds_cri


lds_cri = create n_ds
lds_cri.dataobject = "d_pk_process_loan_cri"
lds_cri.settransobject( itr_sqlca )
this.of_setsrvdwxmlie(true)
inv_dwxmliesrv.of_xmlimport( lds_cri , as_xmldatacri )


li_rowcount = lds_cri.rowcount()

ls_instypecode = lds_cri.getitemstring(1,"instype_code")
ls_loantypecode = lds_cri.getitemstring(1,"loantype_code")

//of_xmlimport( lds_prokeeping_ins, "d_ins_process_keeping", as_xmldata )
lds_loandata = create n_ds
lds_loandata.dataobject = "d_pk_ppn_egat_loan_list_process" //""
lds_loandata.settransobject( itr_sqlca )
inv_progress.istr_progress.progress_text	= "ติดตั้งประกันชีวิต"
inv_progress.istr_progress.subprogress_text	= "กำลังดึงข้อมูล"
inv_progress.istr_progress.subprogress_index	= 0
inv_progress.istr_progress.subprogress_max	= 0


this.of_setsrvdwxmlie(true)
inv_dwxmliesrv.of_xmlimport( lds_loandata , as_xmlloandata )
ll_count = lds_loandata.rowcount( )
inv_progress.istr_progress.subprogress_max	= ll_count

if  ll_count <= 0 then
	ithw_exception.text = "ประมวลประกันชีวิต พบข้อผิดพลาด~n ไม่พบข้อมูลที่ส่งมา กรุณาตรวจสอบ"  + ithw_exception.text
	destroy lds_loandata
	throw ithw_exception
end if
for  ll_row = 1 to  ll_count
	
	yield()
	if inv_progress.of_is_stop() then
		destroy lds_loandata
		return 1
	end if
	
	ls_memno 	= lds_loandata.getitemstring( ll_row,"member_no")
	ls_contno	=  lds_loandata.getitemstring( ll_row,"loancontract_no")
	ls_cardperson = lds_loandata.getitemstring( ll_row,"card_person")
	ldc_loanapp = lds_loandata.getitemdecimal(ll_row,"principal_balance")
	ldc_inspayment = lds_loandata.getitemdecimal(ll_row,"inspayment_amt")
	ldtm_startcont = lds_loandata.getitemdatetime( ll_row,"startcont_date")
	
	inv_progress.istr_progress.subprogress_index	= ll_row
	inv_progress.istr_progress.subprogress_text	= "เลขที่# " + ls_memno 
	
	li_membtype = 1
	if isnull(ldc_inspayment) then ldc_inspayment = 0
	if ldc_inspayment = 0 then
		ldc_inspayment = ldc_loanapp * 0.0075
	end if
	li_chkins = 0
	select count( insgroupdoc_no )
	into :li_chkins
	from insgroupmaster where insgroupdoc_no = :ls_contno and instype_code = :ls_instypecode  using itr_sqlca ;
	if isnull(li_chkins) then li_chkins = 0
	if li_chkins > 0 then
		//messagebox('ติดตั้งประกัน','เลขที่สมาชิก #' + ls_memno + '  เลขที่สัญญา ' + ls_contno + '  ได้ติดตั้งแล้ว')
		continue;
	end if
	select max( insgroup_id )
	into :ll_insgroupid
	from insgroupmaster using itr_sqlca  ;
	
	if isnull( ll_insgroupid ) then ll_insgroupid = 0
	ll_insgroupid ++
	ldtm_endsafe =   datetime( date(string(year(date( ldtm_startcont)) + 1 ) + string(ldtm_startcont,'-mm-dd')  ))
//	ldtm_endsafe = datetime(relativedate( date( ldtm_startcont ),366)) //datetime( string(ldtm_startcont,'dd/mm/') + string(year(date( ldtm_startcont)) + 1 ) ) //
	if ls_instypecode = '05' then
		li_period = 3
		
		ldc_periodpay =  ldc_inspayment / li_period
		ldc_periodpay =  truncate(ldc_periodpay,0) + 1 //ldc_periodpay + ( 10 - mod( ldc_periodpay ,10))
	else
		li_period = 1
		ldc_periodpay = ldc_inspayment
	end if
	insert into  insgroupmaster 
	(	member_no,		instype_code,		level_code,				insgroupdoc_no,			insgroup_date,	insgroup_id, period,
		insreq_date,		inscost_blance,		insperod_payment,		last_period,				insmemb_status,	
		last_stm_no,		inspayment_status,	loan_amt,					share_amt,				remark,
		process_date,		misspay_amt,		insreqdoc_no,			loanreq_amt,				startsafe_date	,	endsafe_date,
		marrige_name	,	insgroupno_ref,	insmemb_type	,		person_card	,			inspayment_arrear,	inspayment_amt,	expense_code	) 
	values 
	(	:ls_memno ,		:ls_instypecode ,		1,						:ls_contno ,					:ldtm_startcont,	:ll_insgroupid,	:li_period ,
		:ldtm_startcont,	:ldc_loanapp,			:ldc_periodpay, 	0 , 							1,
		1,						1,							:ldc_loanapp,			0,			'' ,
		:ldtm_startcont,	0,							:ls_contno,				:ldc_loanapp,		:ldtm_startcont	, :ldtm_endsafe,
		' ',					:ls_contno,					:li_membtype,			:ls_cardperson,			0, :ldc_inspayment,	'TKP'	) using itr_sqlca ;
	if( itr_sqlca.sqlcode <> 0 ) then

		ithw_exception.text = "ประมวลประกันชีวิต1 พบข้อผิดพลาด~n" + string( itr_sqlca.sqlcode ) + "  " + itr_sqlca.sqlerrtext + "~n" + ithw_exception.text
		destroy lds_loandata
		throw ithw_exception
		return -1
	end if
	
	INSERT INTO INSGROUPSTATEMENT  
	( 	MEMBER_NO,  	 	instype_code,   			SEQ_NO,						INSITEMTYPE_CODE,   insgroup_id,
		INSPERIOD_AMT,  INSPERIOD_PAYMENT,  	insprince_balance, 
		OPERATE_DATE,  	INSGROUPSLIP_DATE,    	ENTRY_DATE,  	 			insgroupdoc_no ,
		ENTRY_ID,			refdoc_no )  
	VALUES 
	( 	:ls_memno,   		:ls_instypecode,   			1,   	'IPX',   :ll_insgroupid,
		1,						:ldc_inspayment,   		:ldc_inspayment,			
		:ldtm_startcont,  	:ldtm_startcont,   			:ldtm_startcont,	:ls_contno ,		
		:ls_userid,			'ติดตั้งจากเงินกู้' 		)  using itr_sqlca  ;
			  
	if itr_sqlca.sqlcode <> 0 then
		ithw_exception.text = "ประมวลประกันชีวิ2ต พบข้อผิดพลาด~n" + string( itr_sqlca.sqlcode ) + "  " + itr_sqlca.sqlerrtext + "~n" + ithw_exception.text
		destroy lds_loandata
		throw ithw_exception
		return -1
	end if

next

commit using itr_sqlca ;
destroy lds_loandata
inv_progress.istr_progress.status = 1

this.of_setsrvdwxmlie(false)
	
destroy lds_loandata
//messagebox('ติดตั้งประกัน ปปน พคช','ติดตั้งประกันชีวิตเรียบร้อยแล้ว')
return 1
end function

public function integer of_setsrvdwxmlie (boolean ab_switch);// Check argument
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

public function integer of_ins_processenlarge (string as_xmlcri, datetime adtm_operate) throws Exception;/*
หน้าจอต่ออายุประกัน
of_ins_processenlarge 

*/
string		ls_memno, ls_instypecode, ls_insitemtype, ls_startinstype, ls_endinstype, ls_startgroup, ls_endgroup,ls_userid
string ls_insgroupno, ls_itemtypecode,ls_desc, ls_filter
datetime ldtm_start, ldtm_end , ldtm_operate, ldtm_member, ldtm_birthdate,ldtm_endsafe
dec{2}	ldc_periodpayall, ldc_inspayarr, ldc_periodpay,ldc_inspaymentarr,ldc_inspaymentall, ldc_instotal, ldc_inspaymentarrnew, li_period, ldc_loanbalance
dec{4}	ldc_percent
long  ll_row, ll_count, ll_insgroupid
integer li_laststmseq, li_insproctype,li_membage, li_filterdate, li_countdatetype

string  ls_instype,ls_insgroupid,ls_xmlcri
n_ds lds_main, lds_insdata,lds_levelcost

lds_main = create n_ds
lds_main.dataobject = "d_ins_cri_connect_nextyear"
lds_main.settransobject( itr_sqlca )


this.of_setsrvdwxmlie(true)
inv_dwxmliesrv.of_xmlimport( lds_main , as_xmlcri )


lds_levelcost = create n_ds
lds_levelcost.dataobject = "d_ins_uncf_levelcost_enlarge"
lds_levelcost.settransobject( itr_sqlca )
lds_levelcost.retrieve()


lds_main.accepttext()
ls_startinstype = lds_main.getitemstring(1,"startinstype_code")
ls_endinstype = lds_main.getitemstring(1,"endintype_code")
ldtm_start  = lds_main.getitemdatetime(1,"start_date")
ldtm_end  = lds_main.getitemdatetime(1,"end_date")
ls_startgroup = lds_main.getitemstring(1,"start_group")
ls_endgroup = lds_main.getitemstring(1,"end_group")
ldc_percent	= lds_main.getitemdecimal(1,"percent_ins")
li_countdatetype = lds_main.getitemnumber(1,"countdate_type")
li_insproctype = lds_main.getitemnumber(1,"process_type")
ls_userid	 =  lds_main.getitemstring(1,"user_id")
ldtm_operate = lds_main.getitemdatetime(1,"operate_date")
li_filterdate   = lds_main.getitemnumber(1,"dateoperate_flag")


lds_insdata = create n_ds

if li_filterdate = 1 or  ls_startinstype = '04' or ls_startinstype = '05' then 
	lds_insdata.dataobject = "d_ins_info_mast_connect_enlarge_date"
	lds_insdata.settransobject( itr_sqlca )
	ll_count = lds_insdata.retrieve( ls_startinstype,  ls_endinstype, ls_startgroup, ls_endgroup,ldtm_start,ldtm_end )
else
	lds_insdata.dataobject = "d_ins_info_mast_connect_enlarge_date_all"
	lds_insdata.settransobject( itr_sqlca )
	ll_count = lds_insdata.retrieve( ls_startinstype,  ls_endinstype, ls_startgroup, ls_endgroup )
end if


inv_progress.istr_progress.progress_text	= "ประกันชีวิต"
inv_progress.istr_progress.subprogress_text	= "กำลังดึงข้อมูล"
inv_progress.istr_progress.subprogress_index	= 0
inv_progress.istr_progress.subprogress_max	= 0

inv_progress.istr_progress.subprogress_max	= ll_count
//if li_filterdate = 1 then
//	ls_filter = " string(endsafe_date,'yyyymmdd') >= '" + string( ldtm_start,'yyyymmdd') + "' and  '" + string( ldtm_end,'yyyymmdd') + "' <=  string( endsafe_date,'yyyymmdd') "  
//	lds_insdata.setfilter( ls_filter )
//	lds_insdata.filter()
//end if
ll_count =  lds_insdata.rowcount()
if ll_count = 0 then
	messagebox('ต่ออายุประกัน','ไม่พบข้อมูลที่จะต่ออายุประกัน')
	ithw_exception.text = "ประมวลประกันชีวิต ไม่พบข้อมูลที่จะต่อประกัน  " + ithw_exception.text
	throw ithw_exception
	return 1
end if
inv_progress.istr_progress.subprogress_max	= ll_count
for ll_row = 1  to ll_count
	
	yield()
	if inv_progress.of_is_stop() then
		destroy lds_insdata
		return 0
	end if
	
	ls_memno		 = trim(lds_insdata.getitemstring(ll_row,"member_no"))
	ls_insgroupno	 = trim(lds_insdata.getitemstring(ll_row,"insgroupdoc_no"))
	ls_instypecode 	= trim(lds_insdata.getitemstring(ll_row,"instype_code"))
	ldc_inspaymentarr = lds_insdata.getitemdecimal(ll_row,"inspayment_arrear")
	ldc_inspaymentall = lds_insdata.getitemdecimal(ll_row,"inspayment_amt")
	li_period 		=  lds_insdata.getitemnumber(ll_row,"period")
	ldc_instotal = lds_insdata.getitemdecimal(ll_row,"paymentall_amt")
	li_laststmseq = lds_insdata.getitemnumber(ll_row,"last_stm_no")
	ll_insgroupid  = lds_insdata.getitemnumber(ll_row,"insgroup_id")
	
	ldtm_member   = lds_insdata.getitemdatetime(ll_row,"member_date")
	ldtm_birthdate   = lds_insdata.getitemdatetime(ll_row,"birth_date")
	ldtm_endsafe	= lds_insdata.getitemdatetime(ll_row,"endsafe_date")
	if isnull( li_laststmseq ) then li_laststmseq = 0
	if isnull( ldc_inspaymentarr ) then ldc_inspaymentarr = 0
	if isnull( ldc_inspaymentall ) then ldc_inspaymentall = 0
	if isnull( ldc_instotal ) then ldc_instotal = 0
	
//	if li_countdatetype = 1 then
//		ldtm_start = datetime(relativedate(date(ldtm_endsafe),1))
//		//ldtm_end =  datetime( string(ldtm_endsafe,'dd/mm/') + string(year(date( ldtm_endsafe)) + 1 ) ) 
//		ldtm_end = datetime(relativedate(date(ldtm_endsafe),365))
//	end if
	
	inv_progress.istr_progress.subprogress_index	= ll_row
	inv_progress.istr_progress.subprogress_text	= "เลขที่# " + ls_memno 
	choose case ls_instypecode
		case '03' //ประกันสุขภาพ
			li_membage = year(date( ldtm_operate ))  - year(date( ldtm_birthdate )) 
			lds_levelcost.setfilter("instype_code = '" + ls_instypecode + "' and max_age >= " + string( li_membage ) + " and  min_age <= " + string(li_membage) )
			lds_levelcost.filter()
			if lds_levelcost.rowcount() > 0 then
				ldc_inspaymentall = lds_levelcost.getitemdecimal( 1,"insperiod_payment")
			else
				ithw_exception.text = "ประมวลประกันชีวิต พบข้อผิดพลาด~n" + string( itr_sqlca.sqlcode ) + "  " + itr_sqlca.sqlerrtext + "~n" + ithw_exception.text
				
				throw ithw_exception
			end if
		case '04' //ประกัน ปปน
			select sum( principal_balance )
			into :ldc_loanbalance
			from lncontmaster where member_no = :ls_memno and loantype_code = '21' using itr_sqlca  ;
			ldc_inspaymentall = ldc_loanbalance *ldc_percent
			ldc_periodpay = ldc_inspaymentall 
			
			//ldtm_end = datetime( string(ldtm_endsafe,'dd/mm/') + string(year(date( ldtm_endsafe )) + 1 ) )
			ldtm_end =   datetime( date(string(year(date( ldtm_endsafe)) + 1 ) + string(ldtm_endsafe,'-mm-dd')  )) //ldtm_end = datetime(relativedate(date(ldtm_endsafe),366))
		case '05' //ประกันพคช
			select sum( principal_balance )
			into :ldc_loanbalance
			from lncontmaster where member_no = :ls_memno and loantype_code = '22' using itr_sqlca  ;
			ldc_inspaymentall = ldc_loanbalance * ldc_percent
			ldc_periodpay = ldc_inspaymentall / li_period
//			ldc_periodpay =  truncate(ldc_periodpay,0 ) + 1
			//ldtm_end = datetime( string(ldtm_endsafe,'dd/mm/') + string(year(date( ldtm_endsafe )) + 1 ) )
			//ldtm_end = datetime(relativedate(date(ldtm_endsafe),366 ))
			ldtm_end =   datetime( date(string(year(date( ldtm_endsafe)) + 1 ) + string(ldtm_endsafe,'-mm-dd')  ))
		case else
			li_membage = year(date( ldtm_operate ))  - year(date( ldtm_birthdate )) 
			lds_levelcost.setfilter("instype_code = '" + ls_instypecode + "' and max_age >= " + string( li_membage ) + " and  min_age <= " + string(li_membage))
			lds_levelcost.filter()
			if lds_levelcost.rowcount() > 0 then
				ldc_inspaymentall = lds_levelcost.getitemdecimal( 1,"insperiod_payment")
			else
				ithw_exception.text = "ประมวลประกันชีวิต พบข้อผิดพลาด~n" + string( itr_sqlca.sqlcode ) + "  " + itr_sqlca.sqlerrtext + "~n" + ithw_exception.text
				
				throw ithw_exception
			end if
	end choose
	
	ldc_inspaymentarrnew = ldc_inspaymentarr + ldc_inspaymentall
	select max( seq_no )
	into :li_laststmseq
	from INSGROUPSTATEMENT where insgroup_id = :ll_insgroupid using itr_sqlca  ;
	if isnull( li_laststmseq ) then li_laststmseq = 0
	li_laststmseq ++
	if li_insproctype = 1 then
		ls_itemtypecode = 'CLS'
		ls_desc = "ปิดประกัน"
		ldc_inspaymentall = 0
		
	else
		ls_itemtypecode = 'AIS'
		ls_desc = "ติดตั้งประกัน"
	end if
	INSERT INTO INSGROUPSTATEMENT  
	( 	MEMBER_NO,  	 	instype_code,   			SEQ_NO,						INSITEMTYPE_CODE,   
		INSPERIOD_AMT,  INSPERIOD_PAYMENT,  	insprince_balance, 		inspayment_arrear,
		OPERATE_DATE,  	INSGROUPSLIP_DATE,    	ENTRY_DATE,  	 			insgroupdoc_no , insgroup_id, 
		ENTRY_ID,			refdoc_no )  
	VALUES 
	( 	:ls_memno,   		:ls_instypecode,   			:li_laststmseq,   			:ls_itemtypecode ,   
		0,						:ldc_inspaymentall,   		:ldc_inspaymentall ,		:ldc_inspaymentall	,	
		:ldtm_operate,  	:ldtm_operate,   			:ldtm_operate,				:ls_insgroupno ,	:ll_insgroupid,	
		:ls_userid,			:ls_desc 		)  using itr_sqlca  ;
			  
	if itr_sqlca.sqlcode <> 0 then
			ithw_exception.text = "ประมวลประกันชีวิต INSGROUPSTATEMENT พบข้อผิดพลาด~n" + string( ll_insgroupid ) + '#' + ls_memno + " " +  string( itr_sqlca.sqlcode ) + "  " + itr_sqlca.sqlerrtext + "~n" + ithw_exception.text
			destroy lds_insdata
			throw ithw_exception
			return -1
	end if
	if li_insproctype = 1 then
		//กรณีปิดประกัน
		ldc_inspaymentall = 0
		update insgroupmaster
		set 	inspayment_arrear = :ldc_inspaymentall, last_stm_no = :li_laststmseq, inspayment_amt = :ldc_inspaymentall,
				insmemb_status = -1
		where member_no = :ls_memno and instype_code = :ls_instypecode and insgroup_id = :ll_insgroupid  using itr_sqlca ;
			
	else
	
		update insgroupmaster
		set 	inspayment_arrear = :ldc_inspaymentall, last_stm_no = :li_laststmseq, inspayment_amt = :ldc_inspaymentall, expense_code = 'TKP',
				startsafe_date = :ldtm_start, endsafe_date = :ldtm_end , insperod_payment = :ldc_periodpay
		where member_no = :ls_memno and instype_code = :ls_instypecode and insgroup_id = :ll_insgroupid  using itr_sqlca ;
	end if
	if itr_sqlca.sqlcode <> 0 then
			ithw_exception.text = "ประมวลประกันชีวิต insgroupmaster พบข้อผิดพลาด~n" + string( itr_sqlca.sqlcode ) + "  " + itr_sqlca.sqlerrtext + "~n" + ithw_exception.text
			destroy lds_insdata
			throw ithw_exception
			return -1
	end if	
	
next 
inv_progress.istr_progress.status = 1
if itr_sqlca.sqlcode = 0 then
	commit  using itr_sqlca;
	//ithw_exception.text = "ประมวลจัดทำต่ออายุ หรอ ปิดประกันเรียบร้อย"
end if
destroy lds_levelcost
destroy lds_insdata
destroy lds_main
this.of_setsrvdwxmlie(false)


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

public function integer of_ins_processtoinsyearbf (string as_xmlcri) throws Exception;/*
	of_ins_processtoinsyearbf  ประกันชีวิต  ประมวลข้อมูลประกันหักจากปันผล
*/
string ls_memno, ls_instypecode, ls_expensecode, ls_grouptxt,ls_userid,ls_primemno
integer li_row, li_count, li_seqno, li_proctype, li_sort, li_divavgyear
long ll_insgroupid, ll_divavgyear
datetime ldtm_operate, ldtm_entry
dec{2} ldc_inspaymentarr

n_ds lds_maincri, lds_insdata


lds_maincri = create n_ds
lds_maincri.dataobject = "d_insmain_cri_divavg"
lds_maincri.settransobject( itr_sqlca )

this.of_setsrvdwxmlie(true)
inv_dwxmliesrv.of_xmlimport( lds_maincri , as_xmlcri )

//lds_maincri.importstring( XML!, as_xmlcri )


lds_insdata = create n_ds
lds_insdata.dataobject = "d_ins_process_to_divavg"
lds_insdata.settransobject( itr_sqlca )

ll_divavgyear = lds_maincri.getitemnumber( 1,"divavg_year")
li_proctype  = lds_maincri.getitemnumber( 1,"process_type")
ls_expensecode = lds_maincri.getitemstring(1,"expense_code")
ldtm_operate = lds_maincri.getitemdatetime( 1,"operate_date")
ldtm_entry = lds_maincri.getitemdatetime( 1,"entry_date")
ls_userid		= lds_maincri.getitemstring(1,"user_id")

inv_progress.istr_progress.progress_text	= "ประกันชีวิต"
inv_progress.istr_progress.subprogress_text	= "กำลังดึงข้อมูล"
inv_progress.istr_progress.subprogress_index	= 0
inv_progress.istr_progress.subprogress_max	= 0

li_count = lds_insdata.retrieve(ls_expensecode)
inv_progress.istr_progress.subprogress_max	= li_count
for li_row = 1 to li_count
	
	yield()
	if inv_progress.of_is_stop() then
		destroy lds_insdata
		return 0
	end if
	
	inv_progress.istr_progress.subprogress_index	= li_row
	inv_progress.istr_progress.subprogress_text	= "เลขที่# " + ls_memno 
	
	ls_memno = lds_insdata.getitemstring( li_row,"member_no")
	ls_instypecode = lds_insdata.getitemstring( li_row,"instype_code")
	ldc_inspaymentarr = lds_insdata.getitemdecimal( li_row,"inspayment_arrear")
	if ls_memno = ls_primemno then 
		li_sort ++
	else 
		li_sort = 1
		
	end if
	insert into insreceiveyear
	( 	 div_year,	member_no,		ins_amt, 		divpost_status ,		sort_no 		, instype_code	)
	values
	(	:li_divavgyear, :ls_memno,   :ldc_inspaymentarr  , 0,  :li_sort ,	:ls_instypecode ) using itr_sqlca ;
	
	if itr_sqlca.sqlcode <> 0 then
			ithw_exception.text = "ประมวลประกันชีวิต insgroupmaster พบข้อผิดพลาด~n" + string( itr_sqlca.sqlcode ) + "  " + itr_sqlca.sqlerrtext + "~n" + ithw_exception.text
			destroy lds_insdata
			throw ithw_exception
			return -1
	end if	
	
	ls_primemno = ls_memno
next

inv_progress.istr_progress.status = 1
if itr_sqlca.sqlcode = 0 then
	commit  using itr_sqlca;
	ithw_exception.text = "ประมวลประกันเรียบร้อย"
end if

destroy lds_insdata
destroy lds_maincri
this.of_setsrvdwxmlie(false)

return 1
end function

public function integer of_ins_postdivavgtoinsmast (string as_xmlcri) throws Exception;/*
	of_ins_postdivavgtoinsmast  ประกันชีวิต  ประมวลผ่านรายการป้ันผลหักชำระประกัน
*/
string ls_memno, ls_instypecode, ls_expensecode, ls_grouptxt,ls_userid,ls_primemno
integer li_row, li_count, li_seqno, li_proctype, li_sort, li_divavgyear,li_kcount,li_k
long ll_insgroupid, ll_divavgyear
datetime ldtm_operate, ldtm_entry
dec{2} ldc_inspaymentarr, ldc_inspaymentall, ldc_inspayment, ldc_total, ldc_receive, ldc_insarr

n_ds lds_maincri, lds_insdata, lds_insdet


lds_maincri = create n_ds
lds_maincri.dataobject = "d_insmain_cri_divavg"
lds_maincri.settransobject( itr_sqlca )

this.of_setsrvdwxmlie(true)
inv_dwxmliesrv.of_xmlimport( lds_maincri , as_xmlcri )


lds_insdata = create n_ds
lds_insdata.dataobject = "d_ins_post_from_divavg_cutins"
lds_insdata.settransobject( itr_sqlca )

lds_insdet = create n_ds
lds_insdet.dataobject = "d_ins_pro_div_cut_ins_det"
lds_insdet.settransobject( itr_sqlca )

ll_divavgyear = lds_maincri.getitemnumber( 1,"divavg_year")
li_proctype  = lds_maincri.getitemnumber( 1,"process_type")
ls_expensecode = lds_maincri.getitemstring(1,"expense_code")
ldtm_operate = lds_maincri.getitemdatetime( 1,"operate_date")
ldtm_entry = lds_maincri.getitemdatetime( 1,"entry_date")
ls_userid		= lds_maincri.getitemstring(1,"user_id")

inv_progress.istr_progress.progress_text	= "ประกันชีวิต"
inv_progress.istr_progress.subprogress_text	= "กำลังดึงข้อมูล"
inv_progress.istr_progress.subprogress_index	= 0
inv_progress.istr_progress.subprogress_max	= 0

li_count = lds_insdata.retrieve(ll_divavgyear)
inv_progress.istr_progress.subprogress_max	= li_count
for li_row = 1 to li_count
	
	yield()
	if inv_progress.of_is_stop() then
		destroy lds_insdata
		return 0
	end if
	

	ls_memno 		 = lds_insdata.getitemstring( li_row,"member_no")
	ldc_inspayment = lds_insdata.getitemdecimal( li_row,"ins_amt")
	ldc_receive		 = lds_insdata.getitemdecimal( li_row,"ins_receive")
	
	inv_progress.istr_progress.subprogress_index	= li_row
	inv_progress.istr_progress.subprogress_text	= "เลขที่# " + ls_memno 
	
	li_kcount  =  lds_insdet.retrieve( ll_divavgyear, ls_memno )
	
	if  ldc_receive >=  ldc_inspayment then
		update insgroupyear
		set  cash_item = :ldc_insarr
		where account_year = :ll_divavgyear and  member_no = :ls_memno using itr_sqlca ;
	else
		for li_k = 1 to li_kcount
			ll_insgroupid  =  lds_insdet.getitemnumber( li_k,"cash_item")
			ldc_insarr =  lds_insdet.getitemnumber( li_k, "ins_amt" )
			if ldc_receive > ldc_insarr then
				update insgroupyear
				set  cash_item = :ldc_insarr
				where account_year = :ll_divavgyear and insgroup_id = :ll_insgroupid  ;
			end if
		next
	end if
	if itr_sqlca.sqlcode <> 0 then
			ithw_exception.text = "ประมวลประกันชีวิต insgroupmaster พบข้อผิดพลาด~n" + string( itr_sqlca.sqlcode ) + "  " + itr_sqlca.sqlerrtext + "~n" + ithw_exception.text
			destroy lds_insdata
			throw ithw_exception
			return -1
	end if	
	if ls_primemno = ls_memno then
		ldc_total +=  ldc_inspaymentall
	else
		insert into  insreceiveyear 
		( div_year,		member_no,		ins_amt,			ins_receive, 	divpost_status , 	sort_no )
		values
		(	:ll_divavgyear, :ls_memno,			:ldc_total,		0,					0,			1	) using itr_sqlca ;
		
		ldc_total = ldc_inspaymentall
	end if
	ls_primemno = ls_memno
next

inv_progress.istr_progress.status = 1
if itr_sqlca.sqlcode = 0 then
	commit  using itr_sqlca;
	//ithw_exception.text = "ประมวลตัดผ่านรายการประกันเรียบร้อย"
end if

destroy lds_insdata
destroy lds_maincri
this.of_setsrvdwxmlie(false)

return 1
end function

public function integer of_init_insloandetail (ref str_insppndetail astr_insloandetail) throws Exception;/*
of_ins_updateinsloandetail update ข้อมูลจากหน้าจอรายละเอียดประกัน
as_xmlinsmain ข้อมูลรายละเอียดสมาชิก
as_xmlloanlist รายการประกันทีมี
as_xmldetail รายละเอียดประกันหลัก
as_xmlloan ข้อมูลเงินกู้
as_xmlstatement รายละเอียดการเคลือนไหว
*/

string ls_memno, ls_instype,ls_insgroupid,ls_xmlcri, ls_xmlmain,ls_xmldata, ls_xmlloan, ls_xmlstmt,ls_xmllist
long ll_row, ll_count, ll_insgroupid,li_rowcount, al_insgroupid
n_ds lds_main, lds_inslist, lds_insdata, lds_insloan, lds_insstmt
str_insppndetail  lstr_inspandet

ls_xmlmain = astr_insloandetail.as_xmlinsmain
ls_xmllist = astr_insloandetail.as_xmlloanlist
ls_xmldata = astr_insloandetail.as_xmldetail
ls_xmlloan = astr_insloandetail.as_xmlloan
ls_xmlstmt = astr_insloandetail.as_xmlstatement

lds_main = create n_ds
lds_main.dataobject = "d_sk_member_detail"
lds_main.settransobject( itr_sqlca )

this.of_setsrvdwxmlie(true)
inv_dwxmliesrv.of_xmlimport( lds_main , ls_xmlmain )

ls_memno = lds_main.getitemstring(1,"member_no")
li_rowcount =  lds_main.retrieve( ls_memno )
ls_xmlmain = this.of_xmlexport( lds_main )

lds_inslist = create n_ds
lds_inslist.dataobject = "d_pk_ppn_egat_list"
lds_inslist.settransobject( itr_sqlca )

this.of_setsrvdwxmlie(true)
inv_dwxmliesrv.of_xmlimport( lds_inslist , ls_xmllist )

li_rowcount = lds_inslist.rowcount(  )
ls_xmllist = this.of_xmlexport( lds_inslist )
ll_row = lds_inslist.SelectRow(1,true)
if ll_row > 0 then
	ll_insgroupid = lds_inslist.getitemnumber( ll_row,"insgroup_id")
end if
if ll_row > 0 then 
	lds_insdata = create n_ds
	lds_insdata.dataobject = "d_pk_ppn_egat_main"
	lds_insdata.settransobject( itr_sqlca )
	this.of_setsrvdwxmlie(true)
	inv_dwxmliesrv.of_xmlimport( lds_insdata , ls_xmldata )
	
	ll_insgroupid = lds_inslist.getitemnumber( 1,"insgroup_id")
	if ll_insgroupid <= 0 or isnull( ll_insgroupid )  then
		select max( insgroup_id )
		into :ll_insgroupid
		from insgroupmaster using itr_sqlca  ;
		if isnull( ll_insgroupid ) then ll_insgroupid  = 0
		
		if lds_insdata.rowcount(  ) > 0 then
			lds_insdata.update() 
		end if
	else
		//ldc_inscost = lds_insdata.getitemdecimal( 
	end if
	

	lds_insstmt = create n_ds
	lds_insstmt.dataobject = "d_pk_ppn_egat_statement"
	lds_insstmt.settransobject( itr_sqlca )
	this.of_setsrvdwxmlie(true)
	inv_dwxmliesrv.of_xmlimport( lds_insstmt , ls_xmlstmt )
	
	if  lds_insstmt.rowcount(  ) > 0 then
	
		lds_insstmt.update() 
	end if
end if
astr_insloandetail.as_xmlinsmain = ls_xmlmain
astr_insloandetail.as_xmlloanlist = ls_xmllist
astr_insloandetail.as_xmldetail = ls_xmldata
astr_insloandetail.as_xmlloan = ls_xmlloan
astr_insloandetail.as_xmlstatement = ls_xmlstmt
if itr_sqlca.sqlcode = 0 then
	commit using itr_sqlca ;
else
//	rollback using itr_sqlca ;
//	return -1
//	throw itr_sqlca.
end if
return 1
/*
string ls_memno, ls_instype,ls_insgroupid,ls_xmlcri, ls_xmlmain,ls_xmldata, ls_xmlloan, ls_xmlstmt,ls_xmllist
long ll_row, ll_count, ll_insgroupid,li_rowcount, al_insgroupid
n_ds lds_main, lds_inslist, lds_insdata, lds_insloan, lds_insstmt
str_insppndetail  lstr_inspandet

ls_xmlmain = astr_insloandetail.as_xmlinsmain
ls_xmllist = astr_insloandetail.as_xmlloanlist
ls_xmldata = astr_insloandetail.as_xmldetail
ls_xmlloan = astr_insloandetail.as_xmlloan
ls_xmlstmt = astr_insloandetail.as_xmlstatement

lds_main = create n_ds
lds_main.dataobject = "d_sk_member_detail"
lds_main.settransobject( itr_sqlca )
lds_main.importstring( XML!, ls_xmlmain )

ls_memno = lds_main.getitemstring(1,"member_no")
li_rowcount =  lds_main.retrieve( ls_memno )
ls_xmlmain = this.of_xmlexport( lds_main )

lds_inslist = create n_ds
lds_inslist.dataobject = "d_pk_ppn_egat_list"
lds_inslist.settransobject( itr_sqlca )

lds_inslist.importstring( XML!, ls_xmllist )
li_rowcount = lds_inslist.retrieve( ls_memno )
ls_xmllist = this.of_xmlexport( lds_inslist )
if li_rowcount > 0 then 
	lds_insdata = create n_ds
	lds_insdata.dataobject = "d_pk_ppn_egat_main"
	lds_insdata.settransobject( itr_sqlca )
	lds_insdata.importstring( XML!, ls_xmldata )
	li_rowcount =  lds_insdata.retrieve( al_insgroupid )
	ls_xmldata = this.of_xmlexport( lds_insdata )
	
	lds_insloan = create n_ds
	lds_insloan.dataobject = "d_ins_prakun_loan_detail"
	lds_insloan.settransobject( itr_sqlca )
	lds_insloan.importstring( XML!, ls_xmlloan )
	li_rowcount = lds_insloan.retrieve( ls_memno )
	ls_xmlloan = this.of_xmlexport( lds_insloan )
	
	lds_insstmt = create n_ds
	lds_insstmt.dataobject = "d_pk_ppn_egat_statement"
	lds_insstmt.settransobject( itr_sqlca )
	lds_insstmt.importstring( XML!, ls_xmlstmt )
	li_rowcount = lds_insstmt.retrieve( ls_memno )
	ls_xmllist = this.of_xmlexport( lds_insstmt )
end if
 lds_insdata.update( ) 
  lds_insstmt.update( ) 
astr_insloandetail.as_xmlinsmain = ls_xmlmain
astr_insloandetail.as_xmlloanlist = ls_xmllist
astr_insloandetail.as_xmldetail = ls_xmldata
astr_insloandetail.as_xmlloan = ls_xmlloan
astr_insloandetail.as_xmlstatement = ls_xmlstmt
*/
return 1
end function

public function integer of_ins_processcancleinstallfromloan (string as_xmldatacri, string as_xmlloandata) throws Exception;/*
of_ins_processinstallfromloan  ยกเลิกติดตั้งประกันชีวิตจากเงินกู้
as_xmldatacri  = เงือนไขในการติดตั้ง
s_xmlloandata = ข้อมูลเงินกู้
*/
string ls_memno, ls_contno, ls_insgroupdocno, ls_instypecode,ls_loantypecode, ls_insdocno,ls_cardperson, ls_userid
dec{2} ldc_loanapp, ldc_inspayment, ldc_periodpay
dec{4} ldc_percent = 0.075
long ll_row,ll_index, ll_rowcount, ll_insgroupid
datetime ldtm_startcont, ldtm_endsafe
integer li_membtype, li_period, li_chkins

string ls_loantype,   ls_receiptno
long  ll_count
integer  li_rowcount
n_ds lds_loandata
n_ds lds_cri

inv_progress.istr_progress.progress_text	= "ติดตั้งประกันชีวิต"
inv_progress.istr_progress.subprogress_text	= "กำลังดึงข้อมูล"
inv_progress.istr_progress.subprogress_index	= 0
inv_progress.istr_progress.subprogress_max	= 0


lds_cri = create n_ds
lds_cri.dataobject = "d_pk_process_loan_cri"
lds_cri.settransobject( itr_sqlca )
li_rowcount = lds_cri.importstring( XML!, as_xmldatacri )

ls_instypecode = lds_cri.getitemstring(1,"instype_code")
ls_loantypecode = lds_cri.getitemstring(1,"loantype_code")

//of_xmlimport( lds_prokeeping_ins, "d_ins_process_keeping", as_xmldata )
lds_loandata = create n_ds
lds_loandata.dataobject = "d_pk_ppn_egat_loan_list_process" //""
lds_loandata.settransobject( itr_sqlca )

this.of_setsrvdwxmlie(true)
inv_dwxmliesrv.of_xmlimport( lds_loandata , as_xmlloandata )
ll_count = lds_loandata.rowcount( )
inv_progress.istr_progress.subprogress_max	= ll_count

if  ll_count <= 0 then
	ithw_exception.text = "ประมวลประกันชีวิต พบข้อผิดพลาด~n ไม่พบข้อมูลที่ส่งมา กรุณาตรวจสอบ"  + ithw_exception.text
	destroy lds_loandata
	throw ithw_exception
end if
for  ll_row = 1 to  ll_count
	
	yield()
	if inv_progress.of_is_stop() then
		destroy lds_loandata
		return 1
	end if
	
	ls_memno 	= lds_loandata.getitemstring( ll_row,"member_no")
	ls_contno	=  lds_loandata.getitemstring( ll_row,"loancontract_no")
	ls_cardperson = lds_loandata.getitemstring( ll_row,"card_person")
	ldc_loanapp = lds_loandata.getitemdecimal(ll_row,"principal_balance")
	ldc_inspayment = lds_loandata.getitemdecimal(ll_row,"inspayment_amt")
	ldtm_startcont = lds_loandata.getitemdatetime( ll_row,"startcont_date")
	li_membtype = 1
	
	inv_progress.istr_progress.subprogress_index	= ll_row
	inv_progress.istr_progress.subprogress_text	= "เลขที่# " + ls_memno 
	
	if isnull(ldc_inspayment) then ldc_inspayment = 0
	
	delete from INSGROUPSTATEMENT where instype_code = :ls_instypecode and member_no = :ls_memno and insgroupdoc_no = :ls_contno 
	using itr_sqlca ;
	delete from insgroupmaster where  instype_code = :ls_instypecode and member_no = :ls_memno and insgroupdoc_no = :ls_contno 
	using itr_sqlca ;
	if( itr_sqlca.sqlcode <> 0 ) then

		ithw_exception.text = "ประมวลประกันชีวิต พบข้อผิดพลาด~n" + string( itr_sqlca.sqlcode ) + "  " + itr_sqlca.sqlerrtext + "~n" + ithw_exception.text
		destroy lds_loandata
		throw ithw_exception
		return -1
	end if


next
commit using itr_sqlca;
destroy lds_loandata
inv_progress.istr_progress.status = 1

this.of_setsrvdwxmlie(false)
	
destroy lds_loandata
//messagebox('ติดตั้งประกัน ปปน พคช','ติดตั้งประกันชีวิตเรียบร้อยแล้ว')
return 1
end function

public function integer of_ins_cancleprocessenlarge (string as_xmlcri, datetime adtm_operate) throws Exception;/*
หน้าจอต่ออายุประกัน
of_ins_processenlarge 

*/
string		ls_memno, ls_instypecode, ls_insitemtype, ls_startinstype, ls_endinstype, ls_startgroup, ls_endgroup,ls_userid
string ls_insgroupno, ls_itemtypecode,ls_desc, ls_filter
datetime ldtm_start, ldtm_end , ldtm_operate, ldtm_member, ldtm_birthdate,ldtm_endsafe
dec{2}	ldc_periodpayall, ldc_inspayarr, ldc_periodpay,ldc_inspaymentarr,ldc_inspaymentall, ldc_instotal, ldc_inspaymentarrnew, li_period, ldc_loanbalance
dec{4}	ldc_percent
long  ll_row, ll_count, ll_insgroupid
integer li_laststmseq, li_insproctype,li_membage, li_filterdate, li_countdatetype

string  ls_instype,ls_insgroupid,ls_xmlcri
n_ds lds_main, lds_insdata,lds_levelcost

lds_main = create n_ds
lds_main.dataobject = "d_ins_cri_connect_nextyear"
lds_main.settransobject( itr_sqlca )


this.of_setsrvdwxmlie(true)
inv_dwxmliesrv.of_xmlimport( lds_main , as_xmlcri )


lds_levelcost = create n_ds
lds_levelcost.dataobject = "d_ins_uncf_levelcost_enlarge"
lds_levelcost.settransobject( itr_sqlca )
lds_levelcost.retrieve()


lds_main.accepttext()
ls_startinstype = lds_main.getitemstring(1,"startinstype_code")
ls_endinstype = lds_main.getitemstring(1,"endintype_code")
ldtm_start  = lds_main.getitemdatetime(1,"start_date")
ldtm_end  = lds_main.getitemdatetime(1,"end_date")
ls_startgroup = lds_main.getitemstring(1,"start_group")
ls_endgroup = lds_main.getitemstring(1,"end_group")
ldc_percent	= lds_main.getitemdecimal(1,"percent_ins")
li_countdatetype = lds_main.getitemnumber(1,"countdate_type")
li_insproctype = lds_main.getitemnumber(1,"process_type")
ls_userid	 =  lds_main.getitemstring(1,"user_id")
ldtm_operate = lds_main.getitemdatetime(1,"operate_date")
li_filterdate   = lds_main.getitemnumber(1,"dateoperate_flag")


lds_insdata = create n_ds

if li_filterdate = 1 or  ls_startinstype = '04' or ls_startinstype = '05' then 
	lds_insdata.dataobject = "d_ins_info_mast_connect_enlarge_date"
	lds_insdata.settransobject( itr_sqlca )
	ll_count = lds_insdata.retrieve( ls_startinstype,  ls_endinstype, ls_startgroup, ls_endgroup,ldtm_start,ldtm_end )
else
	lds_insdata.dataobject = "d_ins_info_mast_connect_enlarge_date_all"
	ll_count = lds_insdata.retrieve( ls_startinstype,  ls_endinstype, ls_startgroup, ls_endgroup )
	lds_insdata.settransobject( itr_sqlca )
end if


inv_progress.istr_progress.progress_text	= "ประกันชีวิต"
inv_progress.istr_progress.subprogress_text	= "กำลังดึงข้อมูล"
inv_progress.istr_progress.subprogress_index	= 0
inv_progress.istr_progress.subprogress_max	= 0

inv_progress.istr_progress.subprogress_max	= ll_count
//if li_filterdate = 1 then
//	ls_filter = " string(endsafe_date,'yyyymmdd') >= '" + string( ldtm_start,'yyyymmdd') + "' and  '" + string( ldtm_end,'yyyymmdd') + "' <=  string( endsafe_date,'yyyymmdd') "  
//	lds_insdata.setfilter( ls_filter )
//	lds_insdata.filter()
//end if
ll_count =  lds_insdata.rowcount()
if ll_count = 0 then
	messagebox('ต่ออายุประกัน','ไม่พบข้อมูลที่จะต่ออายุประกัน')
	ithw_exception.text = "ประมวลประกันชีวิต ไม่พบข้อมูลที่จะต่อประกัน  " + ithw_exception.text
	throw ithw_exception
	return 1
end if
inv_progress.istr_progress.subprogress_max	= ll_count
for ll_row = 1  to ll_count
	
	yield()
	if inv_progress.of_is_stop() then
		destroy lds_insdata
		return 0
	end if
	
	ls_memno		 = trim(lds_insdata.getitemstring(ll_row,"member_no"))
	ls_insgroupno	 = trim(lds_insdata.getitemstring(ll_row,"insgroupdoc_no"))
	ls_instypecode 	= trim(lds_insdata.getitemstring(ll_row,"instype_code"))
	ldc_inspaymentarr = lds_insdata.getitemdecimal(ll_row,"inspayment_arrear")
	ldc_inspaymentall = lds_insdata.getitemdecimal(ll_row,"inspayment_amt")
	li_period 		=  lds_insdata.getitemnumber(ll_row,"period")
	ldc_instotal = lds_insdata.getitemdecimal(ll_row,"paymentall_amt")
	li_laststmseq = lds_insdata.getitemnumber(ll_row,"last_stm_no")
	ll_insgroupid  = lds_insdata.getitemnumber(ll_row,"insgroup_id")
	
	ldtm_member   = lds_insdata.getitemdatetime(ll_row,"member_date")
	ldtm_birthdate   = lds_insdata.getitemdatetime(ll_row,"birth_date")
	ldtm_endsafe	= lds_insdata.getitemdatetime(ll_row,"endsafe_date")
	if isnull( li_laststmseq ) then li_laststmseq = 0
	if isnull( ldc_inspaymentarr ) then ldc_inspaymentarr = 0
	if isnull( ldc_inspaymentall ) then ldc_inspaymentall = 0
	if isnull( ldc_instotal ) then ldc_instotal = 0
	
//	if li_countdatetype = 1 then
//		ldtm_start = datetime(relativedate(date(ldtm_endsafe),1))
//		//ldtm_end =  datetime( string(ldtm_endsafe,'dd/mm/') + string(year(date( ldtm_endsafe)) + 1 ) ) 
//		ldtm_end = datetime(relativedate(date(ldtm_endsafe),365))
//	end if
	
	inv_progress.istr_progress.subprogress_index	= ll_row
	inv_progress.istr_progress.subprogress_text	= "เลขที่# " + ls_memno 
	choose case ls_instypecode
		case '03' //ประกันสุขภาพ
			li_membage = year(date( ldtm_operate ))  - year(date( ldtm_birthdate )) 
			lds_levelcost.setfilter("instype_code = '" + ls_instypecode + "' and max_age >= " + string( li_membage ) + " and  min_age <= " + string(li_membage) )
			lds_levelcost.filter()
			if lds_levelcost.rowcount() > 0 then
				ldc_inspaymentall = lds_levelcost.getitemdecimal( 1,"insperiod_payment")
			else
				ithw_exception.text = "ประมวลประกันชีวิต พบข้อผิดพลาด~n" + string( itr_sqlca.sqlcode ) + "  " + itr_sqlca.sqlerrtext + "~n" + ithw_exception.text
				
				throw ithw_exception
			end if
		case '04' //ประกัน ปปน
			select sum( principal_balance )
			into :ldc_loanbalance
			from lncontmaster where member_no = :ls_memno and loantype_code = '21' using itr_sqlca  ;
			ldc_inspaymentall = ldc_loanbalance *ldc_percent
			ldc_periodpay = ldc_inspaymentall 
			
			//ldtm_end = datetime( string(ldtm_endsafe,'dd/mm/') + string(year(date( ldtm_endsafe )) + 1 ) )
			ldtm_end =   datetime( date(string(year(date( ldtm_endsafe)) - 1 ) + string(ldtm_endsafe,'-mm-dd')  )) //ldtm_end = datetime(relativedate(date(ldtm_endsafe),366))
		case '05' //ประกันพคช
			select sum( principal_balance )
			into :ldc_loanbalance
			from lncontmaster where member_no = :ls_memno and loantype_code = '22' using itr_sqlca  ;
			ldc_inspaymentall = ldc_loanbalance * ldc_percent
			ldc_periodpay = ldc_inspaymentall / li_period
			ldc_periodpay =  truncate(ldc_periodpay,0 ) + 1
			//ldtm_end = datetime( string(ldtm_endsafe,'dd/mm/') + string(year(date( ldtm_endsafe )) + 1 ) )
			//ldtm_end = datetime(relativedate(date(ldtm_endsafe),366 ))
			ldtm_end =   datetime( date(string(year(date( ldtm_endsafe)) - 1 ) + string(ldtm_endsafe,'-mm-dd')  ))
		case else
			li_membage = year(date( ldtm_operate ))  - year(date( ldtm_birthdate )) 
			lds_levelcost.setfilter("instype_code = '" + ls_instypecode + "' and max_age >= " + string( li_membage ) + " and  min_age <= " + string(li_membage))
			lds_levelcost.filter()
			if lds_levelcost.rowcount() > 0 then
				ldc_inspaymentall = lds_levelcost.getitemdecimal( 1,"insperiod_payment")
			else
				ithw_exception.text = "ประมวลประกันชีวิต พบข้อผิดพลาด~n" + string( itr_sqlca.sqlcode ) + "  " + itr_sqlca.sqlerrtext + "~n" + ithw_exception.text
				
				throw ithw_exception
			end if
	end choose
	
	ldc_inspaymentarrnew = ldc_inspaymentarr - ldc_inspaymentall
	select max( seq_no )
	into :li_laststmseq
	from INSGROUPSTATEMENT where insgroup_id = :ll_insgroupid using itr_sqlca  ;
	if isnull( li_laststmseq ) then li_laststmseq = 0
	li_laststmseq ++
	if li_insproctype = 1 then
		ls_itemtypecode = 'RLS'
		ls_desc = "ยกเลิกปิดประกัน"
		ldc_inspaymentall = 0
		
	else
		ls_itemtypecode = 'RAS'
		ls_desc = "ยกเลิกการต่ออายุประกัน"
	end if
	
	INSERT INTO INSGROUPSTATEMENT  
	( 	MEMBER_NO,  	 	instype_code,   			SEQ_NO,						INSITEMTYPE_CODE,   
		INSPERIOD_AMT,  INSPERIOD_PAYMENT,  	insprince_balance, 		inspayment_arrear,
		OPERATE_DATE,  	INSGROUPSLIP_DATE,    	ENTRY_DATE,  	 			insgroupdoc_no , insgroup_id, 
		ENTRY_ID,			refdoc_no )  
	VALUES 
	( 	:ls_memno,   		:ls_instypecode,   			:li_laststmseq,   			:ls_itemtypecode ,   
		0,						:ldc_inspaymentall,   		:ldc_inspaymentarrnew ,		:ldc_inspaymentarrnew	,	
		:ldtm_operate,  	:ldtm_operate,   			:ldtm_operate,				:ls_insgroupno ,	:ll_insgroupid,	
		:ls_userid,			:ls_desc 		)  using itr_sqlca  ;
			  
	if itr_sqlca.sqlcode <> 0 then
			ithw_exception.text = "ประมวลประกันชีวิต INSGROUPSTATEMENT พบข้อผิดพลาด~n" + string( ll_insgroupid ) + '#' + ls_memno + " " +  string( itr_sqlca.sqlcode ) + "  " + itr_sqlca.sqlerrtext + "~n" + ithw_exception.text
			destroy lds_insdata
			throw ithw_exception
			return -1
	end if
	if li_insproctype = 1 then
		//กรณีปิดประกัน
		ldc_inspaymentall = 0
		update insgroupmaster
		set 	inspayment_arrear = :ldc_inspaymentall, last_stm_no = :li_laststmseq, inspayment_amt = :ldc_inspaymentall,
				insmemb_status = -1
		where member_no = :ls_memno and instype_code = :ls_instypecode and insgroup_id = :ll_insgroupid  using itr_sqlca ;
			
	else
	
		update insgroupmaster
		set 	inspayment_arrear = :ldc_inspaymentarrnew, last_stm_no = :li_laststmseq, inspayment_amt = :ldc_inspaymentall, expense_code = 'TKP',
				startsafe_date = :ldtm_start, endsafe_date = :ldtm_end , insperod_payment = :ldc_periodpay
		where member_no = :ls_memno and instype_code = :ls_instypecode and insgroup_id = :ll_insgroupid  using itr_sqlca ;
	end if
	if itr_sqlca.sqlcode <> 0 then
			ithw_exception.text = "ประมวลประกันชีวิต ยกเลิกการต่ออายุ insgroupmaster พบข้อผิดพลาด~n" + string( itr_sqlca.sqlcode ) + "  " + itr_sqlca.sqlerrtext + "~n" + ithw_exception.text
			destroy lds_insdata
			throw ithw_exception
			return -1
	end if	
	
next 
inv_progress.istr_progress.status = 1
if itr_sqlca.sqlcode = 0 then
	commit  using itr_sqlca;
	//ithw_exception.text = "ประมวลจัดทำต่ออายุ หรอ ปิดประกันเรียบร้อย"
end if
destroy lds_levelcost
destroy lds_insdata
destroy lds_main
this.of_setsrvdwxmlie(false)


return 1
end function

public function integer of_ins_updateinsloandetail (ref str_insppndetail astr_insloandetail) throws Exception;/*
of_ins_updateinsloandetail update ข้อมูลจากหน้าจอรายละเอียดประกัน
as_xmlinsmain ข้อมูลรายละเอียดสมาชิก
as_xmlloanlist รายการประกันทีมี
as_xmldetail รายละเอียดประกันหลัก
as_xmlloan ข้อมูลเงินกู้
as_xmlstatement รายละเอียดการเคลือนไหว
*/

string ls_memno, ls_instype,ls_insgroupid,ls_xmlcri, ls_xmlmain,ls_xmldata, ls_xmlloan, ls_xmlstmt,ls_xmllist
long ll_row, ll_count, ll_insgroupid,li_rowcount, al_insgroupid
n_ds lds_main, lds_inslist, lds_insdata, lds_insloan, lds_insstmt
str_insppndetail  lstr_inspandet

ls_xmlmain = astr_insloandetail.as_xmlinsmain
ls_xmllist = astr_insloandetail.as_xmlloanlist
ls_xmldata = astr_insloandetail.as_xmldetail
ls_xmlloan = astr_insloandetail.as_xmlloan
ls_xmlstmt = astr_insloandetail.as_xmlstatement

lds_main = create n_ds
lds_main.dataobject = "d_sk_member_detail"
lds_main.settransobject( itr_sqlca )

this.of_setsrvdwxmlie(true)
inv_dwxmliesrv.of_xmlimport( lds_main , ls_xmlmain )

ls_memno = lds_main.getitemstring(1,"member_no")
li_rowcount =  lds_main.retrieve( ls_memno )
ls_xmlmain = this.of_xmlexport( lds_main )

lds_inslist = create n_ds
lds_inslist.dataobject = "d_pk_ppn_egat_list"
lds_inslist.settransobject( itr_sqlca )

this.of_setsrvdwxmlie(true)
inv_dwxmliesrv.of_xmlimport( lds_inslist , ls_xmllist )

li_rowcount = lds_inslist.rowcount(  )
ls_xmllist = this.of_xmlexport( lds_inslist )
ll_row = lds_inslist.SelectRow(1,true)
if ll_row > 0 then
	ll_insgroupid = lds_inslist.getitemnumber( ll_row,"insgroup_id")
end if
if ll_row > 0 then 
	lds_insdata = create n_ds
	lds_insdata.dataobject = "d_pk_ppn_egat_main"
	lds_insdata.settransobject( itr_sqlca )
	this.of_setsrvdwxmlie(true)
	inv_dwxmliesrv.of_xmlimport( lds_insdata , ls_xmldata )
	
	ll_insgroupid = lds_inslist.getitemnumber( 1,"insgroup_id")
	if ll_insgroupid <= 0 or isnull( ll_insgroupid )  then
		select max( insgroup_id )
		into :ll_insgroupid
		from insgroupmaster using itr_sqlca  ;
		if isnull( ll_insgroupid ) then ll_insgroupid  = 0
		
		if lds_insdata.rowcount(  ) > 0 then
			lds_insdata.update() 
		end if
	else
		//ldc_inscost = lds_insdata.getitemdecimal( 
	end if
	

	lds_insstmt = create n_ds
	lds_insstmt.dataobject = "d_pk_ppn_egat_statement"
	lds_insstmt.settransobject( itr_sqlca )
	this.of_setsrvdwxmlie(true)
	inv_dwxmliesrv.of_xmlimport( lds_insstmt , ls_xmlstmt )
	
	if  lds_insstmt.rowcount(  ) > 0 then
	
		lds_insstmt.update() 
	end if
end if
astr_insloandetail.as_xmlinsmain = ls_xmlmain
astr_insloandetail.as_xmlloanlist = ls_xmllist
astr_insloandetail.as_xmldetail = ls_xmldata
astr_insloandetail.as_xmlloan = ls_xmlloan
astr_insloandetail.as_xmlstatement = ls_xmlstmt
if itr_sqlca.sqlcode = 0 then
	commit using itr_sqlca ;
else
//	rollback using itr_sqlca ;
//	return -1
//	throw itr_sqlca.
end if
return 1
end function

public function integer of_postupdate_insloandetail (ref string as_xmlinsmain, ref string as_xmlinslist, ref string as_xmldata, ref string as_xmlloan, ref string as_xmlinsstmt, long al_insgroupid, string as_memno) throws Exception;string ls_memno, ls_instype
long ll_row, ll_count, ll_insgroupid,li_rowcount
n_ds lds_main, lds_inslist, lds_insdata, lds_insloan, lds_insstmt

lds_main = create n_ds
lds_main.dataobject = "d_sk_member_detail"
lds_main.settransobject( itr_sqlca )
lds_main.importstring( XML!, as_xmlinsmain )
li_rowcount =  lds_main.retrieve( as_memno )
as_xmlinsmain = this.of_xmlexport( lds_main )
lds_inslist = create n_ds
lds_inslist.dataobject = "d_pk_ppn_egat_list"
lds_inslist.settransobject( itr_sqlca )
lds_inslist.importstring( XML!, as_xmlinslist )
li_rowcount = lds_inslist.retrieve( as_memno )
as_xmlinslist = this.of_xmlexport( lds_inslist )
lds_insdata = create n_ds
lds_insdata.dataobject = "d_pk_ppn_egat_main"
lds_insdata.settransobject( itr_sqlca )

this.of_setsrvdwxmlie(true)
inv_dwxmliesrv.of_xmlimport( lds_insdata , as_xmldata )

if  lds_insdata.update( ) = 1 then
//	messagebox('บันทึกประกัน','บันทึกข้อมูลประกันเรียบร้อยแล้ว')
//else
//	messagebox('บันทึกประกัน','ไม่สามารถบันทึกข้อมูลประกันได้')
end if
as_xmldata = this.of_xmlexport( lds_insdata )
lds_insloan = create n_ds
lds_insloan.dataobject = "d_pk_process_loan_cri"
lds_insloan.settransobject( itr_sqlca )

this.of_setsrvdwxmlie(true)
inv_dwxmliesrv.of_xmlimport( lds_insloan , as_xmlloan )
as_xmlloan = this.of_xmlexport( lds_insloan )

lds_insstmt = create n_ds
lds_insstmt.dataobject = "d_pk_ppn_egat_statement"
lds_insstmt.settransobject( itr_sqlca )
this.of_setsrvdwxmlie(true)
inv_dwxmliesrv.of_xmlimport( lds_insstmt , as_xmlinsstmt )
if  lds_insstmt.update( ) = 1 then
//	messagebox('บันทึกประกัน','บันทึกข้อมูลประกันเรียบร้อยแล้ว')
//else
//	messagebox('บันทึกประกัน','ไม่สามารถบันทึกข้อมูลประกันได้')
end if


as_xmlinsstmt = this.of_xmlexport( lds_insstmt )
return 1
end function

public function integer of_ins_procdivavgtoinsmast (string as_xmlcri) throws Exception;/*
	of_ins_procdivavgtoinsmast  ประกันชีวิต  ประมวลตั้งยอดเพื่อให้ปันผลหักประกัน
*/
string ls_memno, ls_instypecode, ls_expensecode, ls_grouptxt,ls_userid,ls_primemno
integer li_row, li_count, li_seqno, li_proctype, li_sort, li_divavgyear
long ll_insgroupid, ll_divavgyear
datetime ldtm_operate, ldtm_entry
dec{2} ldc_inspaymentarr, ldc_inspaymentall, ldc_inspayment, ldc_total

n_ds lds_maincri, lds_insdata


lds_maincri = create n_ds
lds_maincri.dataobject = "d_insmain_cri_divavg"
lds_maincri.settransobject( itr_sqlca )

this.of_setsrvdwxmlie(true)
inv_dwxmliesrv.of_xmlimport( lds_maincri , as_xmlcri )

//lds_maincri.importstring( XML!, as_xmlcri )


lds_insdata = create n_ds
lds_insdata.dataobject = "d_ins_posttoinsfrom_divavg"
lds_insdata.settransobject( itr_sqlca )

ll_divavgyear = lds_maincri.getitemnumber( 1,"divavg_year")
li_proctype  = lds_maincri.getitemnumber( 1,"process_type")
ls_expensecode = lds_maincri.getitemstring(1,"expense_code")
ldtm_operate = lds_maincri.getitemdatetime( 1,"operate_date")
ldtm_entry = lds_maincri.getitemdatetime( 1,"entry_date")
ls_userid		= lds_maincri.getitemstring(1,"user_id")

inv_progress.istr_progress.progress_text	= "ประกันชีวิต"
inv_progress.istr_progress.subprogress_text	= "กำลังดึงข้อมูล"
inv_progress.istr_progress.subprogress_index	= 0
inv_progress.istr_progress.subprogress_max	= 0

li_count = lds_insdata.retrieve(ls_expensecode)
inv_progress.istr_progress.subprogress_max	= li_count
for li_row = 1 to li_count
	
	yield()
	if inv_progress.of_is_stop() then
		destroy lds_insdata
		return 0
	end if
	

	ls_memno = lds_insdata.getitemstring( li_row,"member_no")
	ls_instypecode = lds_insdata.getitemstring( li_row,"instype_code")
	ldc_inspaymentarr = lds_insdata.getitemdecimal( li_row,"inspayment_arrear")
	ldc_inspayment = lds_insdata.getitemdecimal( li_row,"inspayment_amt")
	ll_insgroupid   =   lds_insdata.getitemnumber( li_row,"insgroup_id")
	if ls_instypecode = '01' or ls_instypecode = '02' or ls_instypecode = '03'  or ls_instypecode = '06'  then
		ldc_inspaymentall = ldc_inspayment * 12
	else
		ldc_inspaymentall = ldc_inspaymentarr
		
	end if
	
	inv_progress.istr_progress.subprogress_index	= li_row
	inv_progress.istr_progress.subprogress_text	= "เลขที่# " + ls_memno 
	
	insert into insgroupyear
	( 	account_year,		member_no,		instype_code,		inscost_blance,		month_1,	month_2,
		month_3,			month_4,			month_5,			month_6,			month_7,	month_8,
		month_9,			month_10,			month_11,			month_12,			post_status,ins_amt,		insgroup_id  )
	values
	(  :ll_divavgyear,		:ls_memno,			:ls_instypecode,	0,		0,				0,
		0,						0,						0,						0,						0,				0,
		0,						0,						0,						0,						0,				:ldc_inspaymentall ,				:ll_insgroupid   )using itr_sqlca ;
	
	if itr_sqlca.sqlcode <> 0 then
			ithw_exception.text = "ประมวลประกันชีวิต insgroupmaster พบข้อผิดพลาด~n" + string( itr_sqlca.sqlcode ) + "  " + itr_sqlca.sqlerrtext + "~n" + ithw_exception.text
			destroy lds_insdata
			throw ithw_exception
			return -1
	end if	
	if ls_primemno = ls_memno then
		ldc_total +=  ldc_inspaymentall
	else
		insert into  insreceiveyear 
		( div_year,		member_no,		ins_amt,			ins_receive, 	divpost_status , 	sort_no )
		values
		(	:ll_divavgyear, :ls_memno,			:ldc_total,		0,					0,			1	) using itr_sqlca ;
		ldc_total = ldc_inspaymentall
	end if
	ls_primemno = ls_memno
next

inv_progress.istr_progress.status = 1
if itr_sqlca.sqlcode = 0 then
	commit  using itr_sqlca;
	//ithw_exception.text = "ประมวลตัดผ่านรายการประกันเรียบร้อย"
end if

destroy lds_insdata
destroy lds_maincri
this.of_setsrvdwxmlie(false)

return 1
end function

on n_cst_insurance_process.create
call super::create
TriggerEvent( this, "constructor" )
end on

on n_cst_insurance_process.destroy
TriggerEvent( this, "destructor" )
call super::destroy
end on

event constructor;ithw_exception = create Exception
end event

event destructor;if isvalid( ithw_exception ) then destroy ithw_exception
if isvalid( inv_progress ) then destroy inv_progress
end event

