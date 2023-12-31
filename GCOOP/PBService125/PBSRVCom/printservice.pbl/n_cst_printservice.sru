$PBExportHeader$n_cst_printservice.sru
$PBExportComments$PrintService เป็นที่รวมฟังชั่นเกี่ยวกับงานพิมพ์ไว้บริการให้ทั้งฝั่ง PBService และฝั่ง WebService
forward
global type n_cst_printservice from n_cst_base
end type
end forward

global type n_cst_printservice from n_cst_base
end type
global n_cst_printservice n_cst_printservice

type variables

Protected:

datastore ids_usermap		//print.usermap.xml		{จับคู่ User กับ IP}
datastore ids_formset			//print.formsets.xml		{จับคู่ IP กับ Formset}
datastore ids_formmap		//print.formmap.xml		{จับคู่ Formset กับ Form และกำหนด Printer}
datastore ids_formprop		//print.formprop.xml

n_cst_progresscontrol inv_progress

n_cst_dbconnectservice inv_connected

end variables

forward prototypes
public function integer of_reloadsetting ()
public function string of_getprintername_byuser (string as_userid, string as_formcode)
public function string of_getprintername_bycomip (string as_computerip, string as_formcode)
public function string of_getformsets ()
public function string of_getdefaultformset (string as_userid)
public function string of_getdataobject (string axml_data)
public function string of_getdefaultformset_bycomid (string as_computerid)
public function integer of_print_pdf (string axml_data, string as_pdffilename)
public subroutine of_setprogress (ref n_cst_progresscontrol anv_progress)
public function integer of_print (string axml_data, string as_form, string as_formset) throws Exception
public function string of_getprintername (string as_form, string as_formset)
public function integer of_print (ref datastore ads_data, string as_form, string as_formset) throws Exception
public function integer of_getsetting (ref string axml_users, ref string axml_formsets, ref string axml_forms, ref string axml_report)
public function str_progress of_getprogress ()
public function integer of_settrans (ref n_cst_dbconnectservice anv_connected) throws Exception
public function string of_getprintpdfpath ()
public function string of_getprintpdfurl ()
public function integer of_setformcodeproperties (string as_formcode, ref datastore ads_dwprint)
end prototypes

public function integer of_reloadsetting ();/***********************************************
<description>
สั่งให้โหลดค่า Setting เกี่ยวกับ PrintService จาก XML ใหม่ทั้ง 4 ไฟล์.
1. print.usermap.xml		{จับคู่ User กับ IP}
2. print.formsets.xml		{จับคู่ IP กับ Formset}
3. print.formmap.xml		{จับคู่ Formset กับ Form และกำหนด Printer}
</description>

<arguments>
</arguments>

<return>
String		คืนค่ากลับเป็น 1 ในกรณีปกติ, หากมีข้อผิดพลาดคืนค่ากลับเป็น -1
</return>

<usage>
lnv_printservice.of_reloadsetting()
</usage>
************************************************/
try
	if( isvalid( ids_usermap ) )then
		ids_usermap.reset()
	else
		ids_usermap = create datastore
	end if
	
	if( isvalid( ids_formmap ) )then
		ids_formmap.reset()
	else
		ids_formmap = create datastore
	end if
	
	if( isvalid( ids_formset ) )then
		ids_formset.reset()
	else
		ids_formset = create datastore
	end if
	
	if( isvalid( ids_formprop ) )then
		ids_formprop.reset()
	else
		ids_formprop = create datastore
	end if
	
	n_cst_xmlconfig lnv_config
	lnv_config = create n_cst_xmlconfig
	if( lnv_config.of_loadxml( "print.users" ) < 0 )then
		of_debuglog( "printservice.reloadsetting: loadxml 'print.users' failed !" )
		return -1
	end if
	ids_usermap = lnv_config.of_getdatastore( )
	
	if( lnv_config.of_loadxml( "print.formsets" ) < 0 )then
		of_debuglog( "printservice.reloadsetting: loadxml 'print.formsets' failed !" )
		return -1
	end if
	ids_formset = lnv_config.of_getdatastore( )
	
	if( lnv_config.of_loadxml( "print.forms" ) < 0 )then
		of_debuglog( "printservice.reloadsetting: loadxml 'print.forms' failed !" )
		return -1
	end if
	ids_formmap = lnv_config.of_getdatastore( )
	
	if( lnv_config.of_loadxml( "print.formcode" ) < 0 )then
		of_debuglog( "printservice.reloadsetting: loadxml 'print.formprop' failed !" )
		return -1
	end if
	ids_formprop = lnv_config.of_getdatastore( )
	
catch( Exception th )
	return -1
end try

return 1

end function

public function string of_getprintername_byuser (string as_userid, string as_formcode);/***********************************************
<description>
ขอชื่อเครื่องพิมพ์ด้วยรหัสชื่อผู้ใช้
</description>

<arguments>
as_userid			รหัสชื่อผู้ใช้ (เจ้าของเครื่องหลักของชุดเครื่องพิมพ์)
as_formcode		รหัสแบบฟอร์มการพิมพ์
</arguments>

<return>
String		ส่งค่ากลับเป็นชื่อเครื่องพิมพ์สำหรับสั่งพิมพ์, หากมีข้อผิดพลาดคืนค่าเป็น null
</return>

<usage>
ls_printername = lnv_printservice.of_getprintername_byuser( "ADMIN", "PRINTSLIP" )
</usage>
************************************************/
string ls_printername, ls_comip, ls_formsetcode
integer li_found

//หาเลข IP เครื่องด้วยรหัสชื่อผู้ใช้.
li_found = ids_usermap.find( "user_id = '"+as_userid+"'", 1, ids_usermap.rowcount() )
if( li_found > 0 )then
	ls_comip = trim(ids_formmap.getitemstring( li_found, "computer_ip" ))
else
	setnull( ls_comip )
	return ls_comip
end if

//หารหัสชุดเครื่องพิมพ์ด้วยเลข IP เครื่อง.
li_found = ids_formset.find( "computer_ip = '"+ls_comip+"'", 1, ids_formset.rowcount() )
if( li_found > 0 )then
	ls_formsetcode = trim(ids_formmap.getitemstring( li_found, "formset_code" ))
else
	setnull( ls_formsetcode )
	return ls_formsetcode
end if

//หาชื่อเครื่องพิมพ์ด้วย รหัสชุดเครื่องพิมพ์.
li_found = ids_formmap.find( "formset_code = '"+ls_formsetcode+"' and form_code = '"+as_formcode+"'", 1, ids_formmap.rowcount() )
if( li_found > 0 )then
	ls_printername = trim(ids_formmap.getitemstring( li_found, "printer_name" ))
else
	setnull( ls_printername )
end if
return ls_printername

end function

public function string of_getprintername_bycomip (string as_computerip, string as_formcode);/***********************************************
<description>
ขอชื่อเครื่องพิมพ์ด้วยหมายเลข IP เครื่อง
</description>

<arguments>
as_computerip		หมายเลข IP เครื่อง (เจ้าของชุดเครื่องพิมพ์)
as_formcode		รหัสแบบฟอร์มการพิมพ์
</arguments>

<return>
String		ส่งค่ากลับเป็นชื่อเครื่องพิมพ์สำหรับสั่งพิมพ์, หากมีข้อผิดพลาดคืนค่าเป็น null
</return>

<usage>
ls_printername = lnv_printservice.of_getprintername( "DEPOSITFRONT1", "PRINTSLIP" )
</usage>
************************************************/
string ls_printername, ls_formsetcode
integer li_found

//หารหัสชุดเครื่องพิมพ์ด้วยเลข IP เครื่อง.
li_found = ids_formset.find( "computer_ip = '"+as_computerip+"'", 1, ids_formset.rowcount() )
if( li_found > 0 )then
	ls_formsetcode = trim(ids_formmap.getitemstring( li_found, "formset_code" ))
else
	setnull( ls_formsetcode )
	return ls_formsetcode
end if

//หาชื่อเครื่องพิมพ์ด้วย รหัสชุดเครื่องพิมพ์.
li_found = ids_formmap.find( "formset_code = '"+ls_formsetcode+"' and form_code = '"+as_formcode+"'", 1, ids_formmap.rowcount() )
if( li_found > 0 )then
	ls_printername = trim(ids_formmap.getitemstring( li_found, "printer_name" ))
else
	setnull( ls_printername )
end if
return ls_printername

end function

public function string of_getformsets ();/***********************************************
<description>
ขอรายการชุดเครื่องพิมพ์ที่ลงทะเบียนไว้ระบบแล้ว (สำหรับ dropdowndatawindow, dropdownlist, list ต่างๆ).
</description>

<arguments>	
as_computerid		หมายเลข IP เครื่อง เช่น 192.168.0.192
</arguments>

<return>
String		รหัสชุดเครื่องพิมพ์ (formset_code) ในกรณีปกติ, ผิดพลาดคืนค่า null
</return>

<usage>
lnv_printservice.of_getformsets()

อ้างอิง:
xmlfile			print.formsets.xml
dwobject			dddw_print_formsets
</usage>
************************************************/
integer li_row,li_ins
string ls_xml
datastore lds_formsets
lds_formsets = create datastore
lds_formsets.dataobject = "dddw_print_formsets"
for li_row = 1 to ids_formset.rowcount( )
	li_ins = lds_formsets.insertrow(0)
	lds_formsets.setitem( li_ins, "formset_code", ids_formset.getitemstring( li_row, "formset_code" ) )
	lds_formsets.setitem( li_ins, "formset_desc", ids_formset.getitemstring( li_row, "formset_desc" ) )
next
ls_xml = lds_formsets.describe( "Datawindow.data.XML" )
return ls_xml

end function

public function string of_getdefaultformset (string as_userid);/***********************************************
<description>
ขอรหัสชุดเครื่องพิมพ์ด้วยรหัสชื่อผู้ใช้
</description>

<arguments>	
as_userid		รหัสชื่อผู้ใช้ (case sensitive)
</arguments>

<return>
String		รหัสชุดเครื่องพิมพ์ (formset_code) ในกรณีปกติ, ผิดพลาดคืนค่า null
</return>

<usage>
lnv_printservice.of_getdefaultformset( "ADMIN" )
</usage>
************************************************/
string ls_printername, ls_comip, ls_formsetcode
integer li_found

//หาเลข default IP เครื่องด้วยรหัสชื่อผู้ใช้.
li_found = ids_usermap.find( "user_id = '"+as_userid+"'", 1, ids_usermap.rowcount() )
if( li_found > 0 )then
	ls_comip = trim(ids_formmap.getitemstring( li_found, "computer_ip" ))
else
	setnull( ls_comip )
	return ls_comip
end if

//หารหัสชุดเครื่องพิมพ์ด้วยเลข IP เครื่อง.
li_found = ids_formset.find( "computer_ip = '"+ls_comip+"'", 1, ids_formset.rowcount() )
if( li_found > 0 )then
	ls_formsetcode = trim(ids_formmap.getitemstring( li_found, "formset_code" ))
else
	setnull( ls_formsetcode )
end if
return ls_formsetcode

end function

public function string of_getdataobject (string axml_data);/***********************************************
<description>
ขอชื่อ dataobject จาก DatawindowXML
</description>

<args>	
axml_data		ข้อมูล XML ของ Datawindow (หรือเรียกว่า DatawindowXML)
</args>

<return>
คืนชื่อ datawindow object, ถ้าไม่สำเร็จคืนค่า null
</return>

<usage>
ถูกใช้จากภายใน Object, ไม่แนะนำให้ใช้จากภายนอก เนื่องจากอนาคตอาจมีการเปลี่ยนแปลงสิทธิการเข้าถึงฟังชั่นนี้
</usage>
************************************************/
string ls_dataobject, ls_dw, ls_tmp
integer li_pos, li_end
try
	ls_dw = trim(axml_data)
	ls_tmp = left(right( ls_dw, 3 ),2)
	if( ls_tmp = "/>" )then
		li_pos = LastPos( ls_dw, "<" )
		if( li_pos > 0 )then
			ls_dataobject = right( ls_dw, len( ls_dw ) - li_pos )
			li_end = LastPos( ls_dataobject, "/>" )
			ls_dataobject  = left( ls_dataobject, li_end - 1 )
		end if
	else
		li_pos = LastPos( ls_dw, "/" )
		if( li_pos > 0 )then
			ls_dataobject = right( ls_dw, len( ls_dw ) - li_pos )
			li_end = LastPos( ls_dataobject, ">" )
			ls_dataobject  = left( ls_dataobject, li_end - 1 )
		end if	
	end if
catch( Exception th )
	//of_debuglog( th.getmessage() )
	setnull( ls_dataobject )
end try
return ls_dataobject

end function

public function string of_getdefaultformset_bycomid (string as_computerid);/***********************************************
<description>
ขอรหัสชุดเครื่องพิมพ์ด้วยหมายเลข IP เครื่อง
</description>

<arguments>	
as_computerid		หมายเลข IP เครื่อง เช่น 192.168.0.192
</arguments>

<return>
String		รหัสชุดเครื่องพิมพ์ (formset_code) ในกรณีปกติ, ผิดพลาดคืนค่า null
</return>

<usage>
lnv_printservice.of_getdefaultformset_bycomid( "192.168.0.192" )
</usage>
************************************************/
string ls_printername, ls_comip, ls_formsetcode
integer li_found

//หารหัสชุดเครื่องพิมพ์ด้วยเลข IP เครื่อง.
li_found = ids_formset.find( "computer_ip = '"+as_computerid+"'", 1, ids_formset.rowcount() )
if( li_found > 0 )then
	ls_formsetcode = trim(ids_formmap.getitemstring( li_found, "formset_code" ))
else
	setnull( ls_formsetcode )
end if
return ls_formsetcode

end function

public function integer of_print_pdf (string axml_data, string as_pdffilename);/***********************************************
<description>
สั่งพิมพ์ข้อมูลจาก XML ไปใส่ไฟล์ในรูปแบบ PDF.
ไฟล์ pdf ที่ส่งมาจะอ้างอิงวางไว้ภายในเครื่องที่รัน service นี้
</description>

<arguments>
axml_data			DatawindowXML ที่ต้องการพิมพ์
as_pdffilename		ชื่อไฟล์ PDF แบบไม่มี Path (path จะอยู่ใน XMLConfig ชื่อ printservice.pdfpath)
</arguments>

<return>
String		คืนค่ากลับเป็น 1 ในกรณีปกติ, หากมีข้อผิดพลาดคืนค่ากลับเป็น -1
</return>

<usage>
Integer li_return
String ls_xmldata
ls_xmldata = lds_data.describe( "Datawindow.data.XML" )
li_return = lnv_printservice.of_print_pdf( ls_xmldata, "dataprinted.pdf" )
</usage>
************************************************/
Integer li_return
datastore ldw_data
ldw_data = create datastore
ldw_data.dataobject = of_getdataobject( axml_data )
li_return = ldw_data.importstring( XML!, axml_data )
if( li_return < 0 )then
	of_debuglog( "printservice.of_print_pdf::importString error ("+string(li_return)+")." )
	return li_return
end if
ldw_data.groupcalc( )

//สั่งพิมพ์เป็น PDF ตามไฟล์ที่กำหนดมา
string ls_path
try
	ls_path = of_getprintpdfpath()
	ls_path = ls_path+as_pdffilename
	clipboard( ls_path )
	//messagebox( "Create Print PDF", "file = ["+ls_path+"]"+char(13)+char(10)+"คลิกปุ่ม OK เพื่อเริ่มสร้าง PDF." )
	li_return = ldw_data.saveas( ls_path, PDF!, false )
	//li_return = ldw_data.saveas( ls_path, Text!, false )
catch(Exception thw)
	//messagebox( "Create Print PDF", "printservice.of_print_pdf::create pdf error ("+th.getMessage()+")." )
	of_debuglog( "printservice.of_print_pdf::create pdf error ("+thw.getMessage()+")." )
	of_seterror( "printservice.of_print_pdf::create pdf error ("+thw.getMessage()+")." )
	return -1
end try
if( li_return < 0 )then
	//messagebox( "Create Print PDF", "printservice.of_print_pdf::create pdf failed ("+string(li_return)+")." )
	of_debuglog( "printservice.of_print_pdf::create pdf failed ("+string(li_return)+")." )
	of_seterror( "printservice.of_print_pdf::create pdf failed ("+string(li_return)+")." )
	return li_return
end if

return 1

end function

public subroutine of_setprogress (ref n_cst_progresscontrol anv_progress);/***********************************************
<description>
กำหนด ProgressControl สำหรับใช้เก็บข้อมูลความคืบหน้าการทำงานภายใน object นี้.
ฟังชั่นที่มีผล: of_report_print, of_report_print_pdf, of_report_print_xml
</description>

<arguments>	
anv_progress   instance ของ n_cst_progresscontrol
</arguments>

<return>
</return>

<usage>
lnv_config.of_setprogress( inv_progress )
</usage>
************************************************/
inv_progress = anv_progress

end subroutine

public function integer of_print (string axml_data, string as_form, string as_formset) throws Exception;/***********************************************
<description>
สั่งพิมพ์ข้อมูลจาก adw_print ไปที่เครื่องพิมพ์ที่ map ไว้ตาม as_formsetcode และ as_formcode.
</description>

<arguments>
axml_data		DatawindowXML ที่ต้องการพิมพ์
as_form			รหัสผูกรายงานกับเครื่องพิมพ์
as_formset		รหัสชุดเครื่องพิมพ์
</arguments>

<return>
String		คืนค่ากลับเป็น 1 ในกรณีปกติ, หากมีข้อผิดพลาดคืนค่ากลับเป็น -1
</return>

<usage>
Integer li_return
String ls_xmldata
ls_xmldata = lds_data.describe( "Datawindow.data.XML" )
li_return = lnv_printservice.of_print( ls_xmldata, "PRINTBOOK", "DEPOSITFRONT1" )
</usage>
************************************************/
Integer li_return
String ls_printername
ls_printername = of_getprintername( as_form, as_formset )
li_return = printSetPrinter( ls_printername )
if( li_return < 0 )then
	of_debuglog( "printservice.of_print::printSetPrinter error ("+string(li_return)+")::"+ls_printername+"." )
	return li_return
end if
datastore ldw_data
ldw_data = create datastore
ldw_data.dataobject = of_getdataobject( axml_data )
ldw_data.importstring( XML!, axml_data )
if( li_return < 0 )then
	of_debuglog( "printservice.of_print::importString error ("+string(li_return)+")." )
	return li_return
end if

this.of_setformcodeproperties( as_form, ldw_data )

ldw_data.groupcalc( )
ldw_data.print()
if( li_return < 0 )then
	of_debuglog( "printservice.of_print::print error ("+string(li_return)+")." )
	return li_return
end if
return 1

end function

public function string of_getprintername (string as_form, string as_formset);/***********************************************
<description>
ขอชื่อเครื่องพิมพ์ด้วยรหัสชุดเครื่องพิมพ์
</description>

<arguments>	
as_form				รหัสแบบฟอร์มการพิมพ์
as_formset			รหัสชุดเครื่องพิมพ์
</arguments>

<return>
String		ส่งค่ากลับเป็นชื่อเครื่องพิมพ์สำหรับสั่งพิมพ์, หากมีข้อผิดพลาดคืนค่าเป็น null
</return>

<usage>
ls_printername = lnv_printservice.of_getprintername( "PRINTSLIP", "DEPOSITFRONT1" )
</usage>
************************************************/
string ls_printername
integer li_found
if( isvalid( ids_formmap ) )then
	li_found = ids_formmap.find( "formset_code = '"+as_formset+"' and form_code = '"+as_form+"'", 1, ids_formmap.rowcount() )
	if( li_found > 0 )then
		ls_printername = trim(ids_formmap.getitemstring( li_found, "printer_name" ))
	else
		setnull( ls_printername )
		of_debuglog( "printservice.getprintername('"+as_form+"','"+as_formset+"'): no match printer, please map print first." )
	end if
else
	setnull(ls_printername)
	of_debuglog( "printservice.getprintername('"+as_form+"','"+as_formset+"'): invalid ids_formmap, please call reloadsetting first." )
end if

return ls_printername

end function

public function integer of_print (ref datastore ads_data, string as_form, string as_formset) throws Exception;/***********************************************
<description>
สั่งพิมพ์ข้อมูลจาก Datastore ไปที่เครื่องพิมพ์ที่ map ไว้.
</description>

<arguments>
adw_data		ข้อมูลที่ต้องการพิมพ์
as_form			รหัสผูกรายงานกับเครื่องพิมพ์
as_formset		รหัสผูกรายงานกับเครื่องพิมพ์
</arguments>

<return>
String		คืนค่ากลับเป็น 1 ในกรณีปกติ, หากมีข้อผิดพลาดคืนค่ากลับเป็น -1
</return>

<usage>
Integer li_return
li_return = lnv_printservice.of_print( lds_data, "PRINTBOOK", "DEPOSITFRONT1" )
</usage>
************************************************/
String ls_printername
ls_printername = of_getprintername( as_form, as_formset )
printSetPrinter( ls_printername )

this.of_setformcodeproperties( as_form, ads_data )

ads_data.print()
return 1

end function

public function integer of_getsetting (ref string axml_users, ref string axml_formsets, ref string axml_forms, ref string axml_report);/***********************************************
<description>
ขอ XMLConfig ที่เกี่ยวกับ PrintService
</description>

<arguments>
axml_users			XMLConfig สำหรับ UserMap, ส่งมาเพื่อรับค่ากลับไป.
axml_formsets		XMLConfig สำหรับ Formset, ส่งมาเพื่อรับค่ากลับไป.
axml_forms			XMLConfig สำหรับ FormMap, ส่งมาเพื่อรับค่ากลับไป.
</arguments>

<return>
String		คืนค่ากลับเป็น 1 ในกรณีปกติ, หากมีข้อผิดพลาดคืนค่าเป็น -1
</return>

<usage>
lnv_printservice.of_reloadsetting()

Integer li_return
String ls_xmlusers, ls_xmlformsets, ls_xmlforms
li_return = lnv_printservice.of_getsetting( ls_xmlusers, ls_xmlformsets, ls_xmlforms )
</usage>
************************************************/
try
	axml_users = ids_usermap.describe( "Datawindow.data.XML" )
	axml_formsets = ids_formset.describe( "Datawindow.data.XML" )
	axml_forms = ids_formmap.describe( "Datawindow.data.XML" )
catch( Exception th )
	of_debuglog( "printsrvice.getsetting: please call of_reloadsetting first, "+th.getMessage() )
	return -1
end try
return 1

end function

public function str_progress of_getprogress ();/***********************************************
<description>
ความคืบหน้าการประมวลผลภายใน printservice
</description>

<arguments>
</arguments>

<return>
String		ส่งค่ากลับเป็น Structure ข้อมูลความคืบหน้้า, หากมีข้อผิดพลาดคืนค่าเป็น null
</return>

<usage>
str_progress lstr_progress
lstr_progress = lnv_printservice.of_getprogress()
</usage>
************************************************/
return inv_progress.of_get_progress()

end function

public function integer of_settrans (ref n_cst_dbconnectservice anv_connected) throws Exception;/***********************************************
<description>
deprecated.กำหนด DBConnection สำหรับใช้เชื่อมต่อฐานข้อมูลสำหรับการทำงานภายใน PrintService นี้.
หาก Instance ของ DBConnection ที่ส่งมามี Debuglog กำหนดไว้แล้ว Debuglog นี้จะถูกกำหนดให้กับ PrintService นี้ทันที.

ฟังชั่นที่มีผล: of_report_print, of_report_print_pdf, of_report_print_xml
</description>

<arguments>	
anv_db   instance ของ n_cst_dbconnectservice
</arguments>

<return>
</return>

<usage>
lnv_config.of_settrans( inv_db.itr_dbconnect )
</usage>
************************************************/
Try
	inv_connected = anv_connected
	itr_transaction = anv_connected.itr_dbconnection
	if( not isvalid( itr_transaction ) )then
		Exception thw
		thw = create Exception
		thw.setmessage( "Transaction is not valid !" )
		throw thw
	end if
Catch( Exception exception )
	Throw exception
End Try
return 1

end function

public function string of_getprintpdfpath ();/***********************************************
<description>
ขอ Path ที่อยู่ไฟล์ PDF Print
</description>

<arguments>
</arguments>

<return>
String		ส่งค่ากลับเป็น Path สำหรับ PDF Print ในทางกลับกัน ส่ง null
</return>

<usage>
ls_path = lnv_printservice.of_getprintpdfpath( )	//path = C:\Gcoop\WebService\Print\PDF\
</usage>
***************************************************/
try
	n_cst_xmlconfig lnv_xml
	lnv_xml = create n_cst_xmlconfig
	return lnv_xml.of_getconstantvalue( "printservice.pdfpath" )
catch( Exception th )
	string ls_null
	setnull( ls_null )
	return ls_null
end try

end function

public function string of_getprintpdfurl ();/***********************************************
<description>
ขอ URL ที่อยู่ไฟล์ PDF Print
</description>

<arguments>
</arguments>

<return>
String		ส่งค่ากลับเป็น URL สำหรับ PDF Print ในทางกลับกัน ส่ง null
</return>

<usage>
ls_URL = lnv_printservice.of_getprintpdfurl( )	//URL= http://localhost/Gcoop/WebService/Print/PDF/
</usage>
***************************************************/
try
	n_cst_xmlconfig lnv_xml
	lnv_xml = create n_cst_xmlconfig
	lnv_xml.of_init(inv_connected)
	return lnv_xml.of_getconstantvalue( "printservice.pdfurl" )
catch( Exception th )
	string ls_null
	setnull( ls_null )
	return ls_null
end try

end function

public function integer of_setformcodeproperties (string as_formcode, ref datastore ads_dwprint);integer	li_found, li_papersize, li_paperheight, li_paperwidth
integer	li_marginflag, li_margintop, li_marginbottom, li_marginleft, li_marginright

if not( isvalid( ids_formprop ) ) then
	return 1
end if

li_found = ids_formprop.find( "form_code = '"+as_formcode+"'", 1, ids_formprop.rowcount() )
if li_found <= 0 then
	return 0
end if

li_papersize			= ids_formprop.getitemnumber( li_found, "paper_size" )
li_paperheight		= ids_formprop.getitemnumber( li_found, "paper_height" )
li_paperwidth		= ids_formprop.getitemnumber( li_found, "paper_width" )

li_marginflag		= ids_formprop.getitemnumber( li_found, "margin_flag" )
li_margintop			= ids_formprop.getitemnumber( li_found, "margin_top" )
li_marginbottom	= ids_formprop.getitemnumber( li_found, "margin_bottom" )
li_marginleft			= ids_formprop.getitemnumber( li_found, "margin_left" )
li_marginright		= ids_formprop.getitemnumber( li_found, "margin_right" )

if li_papersize > 0 then
	ads_dwprint.Modify("DataWindow.Print.Paper.Size="+string( li_papersize ) )
end if

if li_papersize = 255 or li_papersize = 256 then
	ads_dwprint.Modify("DataWindow.Print.CustomPage.Length="+string( li_paperheight) )
	ads_dwprint.Modify("DataWindow.Print.CustomPage.Width="+string( li_paperwidth) )
end if

if li_marginflag = 1 then
	ads_dwprint.Modify("DataWindow.Print.Margin.Top=" + string( li_margintop ) )
	ads_dwprint.Modify("DataWindow.Print.Margin.Bottom=" + string( li_marginbottom ) )
	ads_dwprint.Modify("DataWindow.Print.Margin.Left=" + string( li_marginleft ) )
	ads_dwprint.Modify("DataWindow.Print.Margin.Right=" + string( li_marginright ) )
end if

return 1
end function

on n_cst_printservice.create
call super::create
end on

on n_cst_printservice.destroy
call super::destroy
end on

event constructor;/***********************************************
<object>
PrintService เป็นที่รวมฟังชั่นเกี่ยวกับงานพิมพ์ไว้บริการให้ทั้งฝั่ง PBService และฝั่ง WebService
ซึ่งก่อนการใช้งานฟังชั่นใดๆใน service นี้ จำเป็นต้องเรียกฟังชั่น of_reloadsetting ก่อนอย่างน้อย 1 ครั้ง
เพื่อให้ service ทำการโหลดค่า configuration ที่จำเป็นมาพักรอไว้ให้ฟังชั่นต่างๆเรียกใช้งานได้ต่อไป
</object>

<also>
n_cst_xmlservice
</also>

<author>
Initial Version by Prazit (R) Jitmanozot
</author>

<usage>
ตัวอย่างการสั่งพิมพ์ข้อมูลผ่าน Network โดยใช้ประโยชน์จาก MapPrinter ใน service นี้

n_cst_printservice lnv_printservice
lnv_printservice = create n_cst_printservice
lnv_printservice.of_reloadsetting()
</usage>
<exclude>
************************************************/

end event
