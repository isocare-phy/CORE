$PBExportHeader$n_cst_keeping_proc_service.sru
forward
global type n_cst_keeping_proc_service from nonvisualobject
end type
end forward

global type n_cst_keeping_proc_service from nonvisualobject
end type
global n_cst_keeping_proc_service n_cst_keeping_proc_service

type variables
Public:

transaction						itr_sqlca
Exception						ithw_exception

n_cst_dbconnectservice		inv_transection
n_cst_dwxmlieservice			inv_dwxmliesrv
n_cst_stringservice			inv_stringsrv

string		is_coopid , is_coopcontrol , is_recvperiod
string		is_arg[] , is_sqlext
string		is_proctxt
integer	ii_proctype
end variables

forward prototypes
public function integer of_set_txtproc (string as_proctxt)
public function integer of_get_argument (ref string as_arg[])
public function integer of_get_proctype (ref integer ai_proctype)
public function integer of_get_sqlselect (ref string as_sqlext)
public function integer of_initservice ()
public function integer of_set_analyze () throws Exception
public function integer of_set_argument (string as_arg[])
public function integer of_set_proctype (integer ai_proctype)
public function integer of_set_sqldw (ref n_ds ads_info) throws Exception
public function integer of_set_sqlselect (string as_tablename) throws Exception
public function integer of_set_sqldw_column (ref n_ds ads_info, string as_table_name, string as_coln_name, string as_operator, any aa_coln_value) throws Exception
public function integer of_set_sqldw_column (ref n_ds ads_info, string as_condition) throws Exception
protected function integer of_setsrvdwxmlie (boolean ab_switch)
end prototypes

public function integer of_set_txtproc (string as_proctxt);/***********************************************************
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
is_proctxt	= as_proctxt
return 1
end function

public function integer of_get_argument (ref string as_arg[]);as_arg[]		= is_arg[]

return 1
end function

public function integer of_get_proctype (ref integer ai_proctype);ai_proctype		= ii_proctype

return 1
end function

public function integer of_get_sqlselect (ref string as_sqlext);as_sqlext			= is_sqlext

return 1
end function

public function integer of_initservice ();/***********************************************************
<description>
	ใช้สำหรับ Intial service
</description>

<arguments>  
	
</arguments> 

<return> 
	1						Integer	ถ้าไม่มีข้อผิดพลาด
</return> 

<usage> 
	
	Revision History:
	Version 1.0 (Initial version) Modified Date 18/10/2012 by Godji
</usage> 
***********************************************************/
// Service สำหรับสร้าง Sql Extend
if isnull( inv_stringsrv ) or not isvalid( inv_stringsrv ) then
	inv_stringsrv = create n_cst_stringservice
end if

return 1
end function

public function integer of_set_analyze () throws Exception;/***********************************************************
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
if ii_proctype > 1 then inv_stringsrv.of_analyzestring( is_proctxt , is_arg[] )

return 1
end function

public function integer of_set_argument (string as_arg[]);/***********************************************************
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

public function integer of_set_proctype (integer ai_proctype);/***********************************************************
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

public function integer of_set_sqldw (ref n_ds ads_info) throws Exception;/***********************************************************
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

if is_sqlext <> "" then
	
	ls_temp	= ads_info.getsqlselect()
	li_pos		= pos( ls_temp , "GROUP BY" )
	if isnull( li_pos ) or li_pos = 0 then ls_temp	+= is_sqlext
	if li_pos > 0 then ls_temp = mid( ls_temp , 1 , li_pos - 1 ) + " " + is_sqlext + " " + mid( ls_temp , li_pos , len( ls_temp ))
	
	li_ret = ads_info.setsqlselect( ls_temp )
	
	if ( li_ret <> 1 ) then
		ithw_exception.text += "~r~n" + ads_info.of_geterrormessage()
		throw ithw_exception
	end if	
end if

return 1
end function

public function integer of_set_sqlselect (string as_tablename) throws Exception;/***********************************************************
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
		// ทั้งหมด
		is_sqlext	= ""
	case 20
		// ประเภทสมาชิก
		ls_column		+= ".membtype_code"
		inv_stringsrv.of_buildsqlext( is_arg[], ls_column, is_sqlext )
		is_sqlext	= " and " + is_sqlext			
	case 40
		// สังกัด
		ls_column		+= ".membgroup_code"
		inv_stringsrv.of_buildsqlext( is_arg[], ls_column , is_sqlext )
		is_sqlext	= " and " + is_sqlext		
	case 60
		// ทะเบียน
		ls_column		+= ".member_no"
		inv_stringsrv.of_buildsqlext( is_arg[], ls_column, is_sqlext )
		is_sqlext	= " and " + is_sqlext
end choose

return 1
end function

public function integer of_set_sqldw_column (ref n_ds ads_info, string as_table_name, string as_coln_name, string as_operator, any aa_coln_value) throws Exception;/***********************************************************
<description>
	นำชุด sql ที่สร้างมานำเข้า dw
</description>
	Set เงื่อนไขอื่นๆ ลงใน n_ds
<arguments>  
	ads_info			n_ds		datastore ข้อมูลรายการ
	as_table_name	String		ชื่อ Table ที่ต้องการ Set เงื่อนไข
	as_coln_name	String		ชื่อ Column ที่ต้องการ Set เงื่อนไข
	aa_coln_value	Any		ค่าที่ต้องการ Set เงื่อนไข
</arguments> 

<return> 
	integer		1 = success, -1 = failure
</return> 

<usage> 
	Revision History:
	Version 1.0 (Initial version) Modified Date 09/05/2012 by Godji
	Version 1.1 (Initial version) Modified Date 21/10/2012 by Godji
</usage> 
***********************************************************/
string ls_temp , ls_column
integer li_pos , li_ret
boolean lb_err

if isnull( as_table_name ) or len( trim( as_table_name ) ) = 0 then 
	ithw_exception.text += "~r~nไม่พบข้อมูล Table ที่ส่งมาทำรายการ"
	throw ithw_exception
end if

if isnull( as_coln_name ) or len( trim( as_coln_name ) ) = 0 then 
	ithw_exception.text += "~r~nไม่พบข้อมูล Column ที่ส่งมาทำรายการ"
	throw ithw_exception
end if

if isnull( as_operator ) or len( trim( as_operator ) ) = 0 then 
	ithw_exception.text += "~r~nไม่พบข้อมูล Operator ที่ส่งมาทำรายการ"
	throw ithw_exception
end if

if isnull( aa_coln_value ) then
	ithw_exception.text += "~r~nไม่พบข้อมูล Value ที่ส่งมาทำรายการ"
	throw ithw_exception
end if

as_table_name		= trim( as_table_name )
as_coln_name		= trim( as_coln_name )
aa_coln_value		= string( aa_coln_value )

ls_column	= " and " + as_table_name + "." + as_coln_name + " " + as_operator + " '" + aa_coln_value + "'"

ls_temp	= ads_info.getsqlselect()
li_pos		= pos( ls_temp , "GROUP BY" )
if isnull( li_pos ) or li_pos = 0 then ls_temp	+= ls_column
if li_pos > 0 then ls_temp = mid( ls_temp , 1 , li_pos - 1 ) + " " + ls_column + " " + mid( ls_temp , li_pos , len( ls_temp ))

li_ret = ads_info.setsqlselect( ls_temp )

if ( li_ret <> 1 ) then
	ithw_exception.text += "~r~n" + ads_info.of_geterrormessage()
	throw ithw_exception
end if	

return 1
end function

public function integer of_set_sqldw_column (ref n_ds ads_info, string as_condition) throws Exception;/***********************************************************
<description>
	นำชุด sql ที่สร้างมานำเข้า dw
</description>
	Set เงื่อนไขอื่นๆ ลงใน n_ds
<arguments>  
	ads_info			n_ds		datastore ข้อมูลรายการ
	as_table_name	String		ชื่อ Table ที่ต้องการ Set เงื่อนไข
	as_coln_name	String		ชื่อ Column ที่ต้องการ Set เงื่อนไข
	aa_coln_value	Any		ค่าที่ต้องการ Set เงื่อนไข
</arguments> 

<return> 
	integer		1 = success, -1 = failure
</return> 

<usage> 
	Revision History:
	Version 1.0 (Initial version) Modified Date 09/05/2012 by Godji
	Version 1.1 (Initial version) Modified Date 21/10/2012 by Godji
</usage> 
***********************************************************/
string ls_temp , ls_condition
integer li_pos , li_ret
boolean lb_err

if isnull( as_condition ) or len( trim( as_condition ) ) = 0 then 
	ithw_exception.text += "~r~nไม่พบข้อมูล เงื่อนไข ที่ส่งมาทำรายการ"
	throw ithw_exception
end if

as_condition		= trim( as_condition )

ls_condition	= " " + as_condition + " "

ls_temp	= ads_info.getsqlselect()
li_pos		= pos( ls_temp , "GROUP BY" )
if isnull( li_pos ) or li_pos = 0 then ls_temp	+= ls_condition
if li_pos > 0 then ls_temp = mid( ls_temp , 1 , li_pos - 1 ) + " " + ls_condition + " " + mid( ls_temp , li_pos , len( ls_temp ))

li_ret = ads_info.setsqlselect( ls_temp )

if ( li_ret <> 1 ) then
	ithw_exception.text += "~r~n" + ads_info.of_geterrormessage()
	throw ithw_exception
end if	

return 1
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

on n_cst_keeping_proc_service.create
call super::create
TriggerEvent( this, "constructor" )
end on

on n_cst_keeping_proc_service.destroy
TriggerEvent( this, "destructor" )
call super::destroy
end on

event constructor;ithw_exception = create Exception
end event

event destructor;this.of_setsrvdwxmlie( false )
if isvalid(inv_stringsrv) then destroy inv_stringsrv
end event

