<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_hr_master.aspx.cs"
    Inherits="Saving.Applications.hr.w_sheet_hr_master" Title="ข้อมูลสมาชิก" ValidateRequest="false" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=postNewClear%>
    <%=postGetMember%>
    <%=postDeleteRow%>
    <%=postSearchGetMember%>
    <%=postUploadpic%>
    <%=postFilterAmpher%>
    <%=postRefresh%>
    <%=postFilterStep%>
    <%=postStepSalary%>
    <%=postSetPostcode%>
    <%=postShowDetailFami%>
    <%=postShowDetailEdu%>
    <%=postShowDetailExp%>
    <%=postShowDetailSemi%>
    <%=postShowDetailWel%>
    
    <%-- <%=postFilterDeptid %>--%>
    <script type="text/javascript">
        function Validate() {
            objDwMain.AcceptText();
            return confirm("ยืนยันการบันทึกข้อมูล?");
        }

        function MenubarOpen() {
            Gcoop.OpenDlg('780', '600', 'w_dlg_hr_master_search.aspx', '');
        }

        function GetValueEmplid(emplid) {
            var str_temp = window.location.toString();
            var str_arr = str_temp.split("?", 2);
            objDwMain.SetItem(1, "emplid", Gcoop.Trim(emplid));
            objDwMain.AcceptText();
            postGetMember();
        }



        function OnButtonClicked(s, row, oName) {
            if (oName == "b_search") {
                MenubarOpen();
            }
            else if (oName == "b_del") {
                var emplid = "";
                emplid = objDwMain.GetItem(1, "emplid");
                objDwMain.SetItem(1, "emplid", emplid);

                objDwMain.AcceptText();
                Gcoop.GetEl("HdfEmplid").value = emplid;
                if (emplid == "" || emplid == null || emplid == "Auto") {
                    alert("ยังไม่มีข้อมูลไม่สามารถลบรายการได้");
                }
                else {
                    if (confirm("คุณต้องการลบรายการ " + emplid + " ใช่หรือไม่?")) {
                        postDeleteRow();
                    }
                }
            }
            return 0;
        }

        function MenubarNew() {
            if (confirm("ยืนยันการล้างข้อมูลบนหน้าจอ")) {
                postNewClear();
            }
        }


        function OnItemChangeMain(s, r, c, v) {
            objDwMain.SetItem(r, c, v);
            objDwMain.AcceptText();
            if (c == "emplcode") {
                objDwMain.SetItem(1, "emplcode", Gcoop.StringFormat(v, "00000"));
                objDwMain.AcceptText();
            }
            else if (c == "emplamph") {
                objDwMain.SetItem(1, "emplamph", v);
                objDwMain.AcceptText();
                postSetPostcode();
            }
            else if (c == "empldist") {
                objDwMain.SetItem(1, "empldist", v);
                objDwMain.AcceptText();
            }
            else if (c == "empladdrpostcode") {
                objDwMain.SetItem(1, "empladdrpostcode", v);
                objDwMain.AcceptText();
            }
            else if (c == "emplciticardaddr1") {
                objDwMain.SetItem(1, "emplciticardaddr1", v);
                objDwMain.AcceptText();
            }
            else if (c == "email") {
                objDwMain.SetItem(1, "email", v);
                objDwMain.AcceptText();
            }
            else if (c == "remkgetout") {
                objDwMain.SetItem(1, "remkgetout", v);
                objDwMain.AcceptText();
            }
            else if (c == "emplbirtcntry") {
                objDwMain.SetItem(1, "emplbirtcntry", v);
                objDwMain.AcceptText();
            }
            else if (c == "birth_tdate") {
                objDwMain.SetItem(1, "birth_tdate", v);
                objDwMain.AcceptText();
                objDwMain.SetItem(1, "emplbirtdate", Gcoop.ToEngDate(v));
                objDwMain.AcceptText();
            }
            else if (c == "citycard_tdate") {
                objDwMain.SetItem(1, "citycard_tdate", v);
                objDwMain.AcceptText();
                objDwMain.SetItem(1, "emplciticarddate", Gcoop.ToEngDate(v));
                objDwMain.AcceptText();
            }
            else if (c == "cityexp_tdate") {
                objDwMain.SetItem(1, "cityexp_tdate", v);
                objDwMain.AcceptText();
                objDwMain.SetItem(1, "emplciticardexpidate", Gcoop.ToEngDate(v));
                objDwMain.AcceptText();
            }
            else if (c == "emplstat") {
                objDwMain.SetItem(1, "emplstat", v);
                objDwMain.AcceptText();
                postRefresh();
            }
            else if (c == "begin_tdate") {
                objDwMain.SetItem(1, "begin_tdate", v);
                objDwMain.AcceptText();
                objDwMain.SetItem(1, "emplbegndate", Gcoop.ToEngDate(v));
                objDwMain.AcceptText();
            }
            else if (c == "prob_tdate") {
                objDwMain.SetItem(1, "prob_tdate", v);
                objDwMain.AcceptText();
                objDwMain.SetItem(1, "emplprobdate", Gcoop.ToEngDate(v));
                objDwMain.AcceptText();
            }
            else if (c == "getout_tdate") {
                objDwMain.SetItem(1, "getout_tdate", v);
                objDwMain.AcceptText();
                objDwMain.SetItem(1, "getout_date", Gcoop.ToEngDate(v));
                objDwMain.AcceptText();
            }
            else if (c == "last_tdate") {
                objDwMain.SetItem(1, "last_tdate", v);
                objDwMain.AcceptText();
                objDwMain.SetItem(1, "last_date", Gcoop.ToEngDate(v));
                objDwMain.AcceptText();

            }
            else if (c == "emplprvn") {
                objDwMain.SetItem(1, "emplprvn", v);
                objDwMain.AcceptText();
                postFilterAmpher();
            }
            else if (c == "empltele") {
                objDwMain.SetItem(1, "empltele", v);
                objDwMain.AcceptText();
                if (v != null) {
                    if (v.length != 10) {
                        alert("กรุณากรอกข้อมูลเบอร์โทรศัพท์ 10 หลัก ในรูปแบบ 089#######");
                    }
                }
            }
            else if (c == "emplcitiid") {
                objDwMain.SetItem(1, "emplcitiid", v);
                objDwMain.AcceptText();
                if (v != null) {
                    if (v.length != 13) {
                        alert("กรุณากรอกข้อมูลเลขที่บัตรประชาชน 13 หลัก ในรูปแบบ #############");
                    }
                }
            }
            else if (c == "postid") {
                objDwMain.SetItem(1, "postid", v);
                objDwMain.AcceptText();
            }
            else if (c == "hrlevel") {
                objDwMain.SetItem(1, "hrlevel", v);
                objDwMain.AcceptText();
                postStepSalary();
            }
            else if (c == "account") {
                objDwMain.SetItem(1, "account", v);
                objDwMain.AcceptText();
                postStepSalary();
            }
            else if (c == "step_position") {
                objDwMain.SetItem(1, "step_position", v);
                objDwMain.AcceptText();
                postStepSalary();
            }
            objDwMain.AcceptText();
            //return 0;
        }

        function OnDwMainClick(s, r, c) {
            if (c == "emplamph" && objDwMain.GetItem(1, "emplprvn") == null) {
                alert("กรุณาเลือก จังหวัด ก่อน");
            }
            return 0;
        }

        function OnClickUploadPic() {
            postUploadpic();
        }

        //        function OnClickTab(tab){
        //           var i = 1;
        //            for (i = 1; i < 2; i++) {
        //                document.getElementById("tab" + i).style.visibility = "hidden";
        //                document.getElementById("mtab" + i).style.backgroundColor = "rgb(200,235,255)";
        //                if (i == tab) {
        //                    document.getElementById("tab" + i).style.visibility = "visible";
        //                    document.getElementById("mtab" + i).style.backgroundColor = "rgb(211,213,255)";
        //                }
        //            }
        //        }  
        function showTabPage(tab) {
            Gcoop.GetEl("Hdftab").value = tab;
            var tabnum = Gcoop.GetEl("Hdftab").value;
            var i = 1;
            var tabamount = 5;
            for (i = 1; i <= tabamount; i++) {
           
                document.getElementById("tab" + i).style.visibility = "hidden";
                document.getElementById("stab" + i).style.backgroundColor = "rgb(200,235,255)";
                if (i == tabnum) {
                   
                    document.getElementById("tab" + i).style.visibility = "visible";
                    document.getElementById("stab" + i).style.backgroundColor = "rgb(211,213,255)";
                }
            }
        }
        function OnFamilist(sender, rowNumber, objectName) {

            var seq_no = objdw_famlist.GetItem(rowNumber, "seq_no");
           
            if (seq_no != "Auto") {
                objdw_family.SetItem(1, "seq_no", Gcoop.Trim(seq_no));
               
                objdw_family.AcceptText();
                postShowDetailFami();
            }
            return 0;
        }
        function OnEdulist(sender, rowNumber, objectName) {

            var seq_no = objdw_edulist.GetItem(rowNumber, "seq_no");

            if (seq_no != "Auto") {
                objdw_edudetail.SetItem(1, "seq_no", Gcoop.Trim(seq_no));

                objdw_edudetail.AcceptText();
                postShowDetailEdu();
            }
            return 0;
        }
        function OnExplist(sender, rowNumber, objectName) {

            var seq_no = objdw_explist.GetItem(rowNumber, "seq_no");

            if (seq_no != "Auto") {
                objdw_expdetail.SetItem(1, "seq_no", Gcoop.Trim(seq_no));

                objdw_expdetail.AcceptText();
                postShowDetailExp();
            }
            return 0;
        }
        function OnSemilist(sender, rowNumber, objectName) {

            var seq_no = objdw_siminarlist.GetItem(rowNumber, "seq_no");

            if (seq_no != "Auto") {
                objdw_siminardetail.SetItem(1, "seq_no", Gcoop.Trim(seq_no));

                objdw_siminardetail.AcceptText();
                postShowDetailSemi();
            }
            return 0;
        }
        function OnWellist(sender, rowNumber, objectName) {

            var seq_no = objdw_wellist.GetItem(rowNumber, "emp_seq");

            if (seq_no != "Auto") {
                objdw_weldetail.SetItem(1, "emp_seq", Gcoop.Trim(seq_no));

                objdw_weldetail.AcceptText();
                postShowDetailWel();
            }
            return 0;
        }
        function SheetLoadComplete() {
            var tabnum = Gcoop.GetEl("Hdftab").value;
            var i = 1;
            var tabamount = 5;
            for (i = 1; i <= tabamount; i++) {

                document.getElementById("tab" + i).style.visibility = "hidden";
                document.getElementById("stab" + i).style.backgroundColor = "rgb(200,235,255)";
                if (i == tabnum) {

                    document.getElementById("tab" + i).style.visibility = "visible";
                    document.getElementById("stab" + i).style.backgroundColor = "rgb(211,213,255)";
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <table>
        <tr>
            <td align="center">
                <asp:Image ID="Image1" runat="server" ImageAlign="Middle" ImageUrl="~/Applications/hr/image/icon_guest.jpg"
                    Width="99px" />
            </td>
        </tr>
        <tr>
            <td align="center">
                <br />
                <asp:FileUpload ID="FileUpload" runat="server" EnableTheming="True" Width="202px" />
                <asp:Button ID="Button1" runat="server" Text="เพิ่มรูป" OnClick="Button1_Click" UseSubmitBehavior="False" /><br />
                <asp:Label ID="Label1" runat="server" Text="Label" Visible="false"></asp:Label>&nbsp;
            </td>
        </tr>
        <tr>
            <td align="center">
                <div id="Div1">
                    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="dw_hr_emplfilemas"
                        LibraryList="~/DataWindow/hr/hr_master.pbl" ClientScriptable="True" Height="520px"
                        Width="770px" ClientEventButtonClicked="OnButtonClicked" AutoSaveDataCacheAfterRetrieve="True"
                        AutoRestoreDataCache="True" AutoRestoreContext="False" ClientEventItemChanged="OnItemChangeMain"
                        ClientEventClicked="OnDwMainClick" ClientFormatting="True">
                    </dw:WebDataWindowControl>
                </div>
            </td>
        </tr>
    </table>
    <table style="width: 100%;">
        <tr>
            <td align="left">
                <span style="cursor: pointer; font-size: 15px" onclick="OnClickLinkNext();">ข้อมูลเพิ่มเติม</span>
            </td>
            <td align="right">
            </td>
        </tr>
    </table>
    <table width="100%" border="0" style="height: 500px;" >
        <tr>
            <td width="15%" valign="top">
                <table width="100%" style="border: solid 1px;" class="dwtab">
                    <tr>
                        <td align="center" style="font-weight: bold; text-decoration: underline; border-bottom: solid 2px;
                            padding-bottom: 3px; font-size: 15px;">
                            เมนู
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="background-color: rgb(211,213,255); cursor: pointer; padding: 10px 0 10px 0;
                            font-size: 14px;" id="stab1" onclick="showTabPage(1);">
                            ประวัติครอบครัว
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="background-color: rgb(211,213,255); cursor: pointer; padding: 10px 0 10px 0;
                            font-size: 14px;" id="stab2" onclick="showTabPage(2);">
                            ประวัติการศึกษา
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="background-color: rgb(211,213,255); cursor: pointer; padding: 10px 0 10px 0;
                            font-size: 14px;" id="stab3" onclick="showTabPage(3);">
                            ประสบการณ์ทำงาน
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="background-color: rgb(211,213,255); cursor: pointer; padding: 10px 0 10px 0;
                            font-size: 14px;" id="stab4" onclick="showTabPage(4);">
                            สัมนา/ดูงาน/ฝึกอบรม
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="background-color: rgb(211,213,255); cursor: pointer; padding: 10px 0 10px 0;
                            font-size: 14px;" id="stab5" onclick="showTabPage(5);">
                            สวัสดิการ
                        </td>
                    </tr>
                </table>
            </td>
            <td style="border: solid 1px;" valign="top" align="center" class="style5">
                <div id="tab1" style="visibility: visible; position: absolute;">
                    <table>
                        <tr>
                            <td valign="top">
                                <dw:WebDataWindowControl ID="dw_famlist" runat="server" DataWindowObject="dw_hr_employee_family_list_filemas"
                                    LibraryList="~/DataWindow/hr/hr_master.pbl" ClientScriptable="True" AutoRestoreContext="False"
                                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True" ClientEventClicked ="OnFamilist">
                                </dw:WebDataWindowControl>
                            </td>
                            <td valign="top">
                                <dw:WebDataWindowControl ID="dw_family" runat="server" DataWindowObject="dw_hr_employee_family_detail_filemas"
                                    LibraryList="~/DataWindow/hr/hr_master.pbl" ClientScriptable="True" AutoRestoreContext="False"
                                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True">
                                </dw:WebDataWindowControl>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="tab2" style="visibility: hidden; position: absolute;">
                    <table>
                        <tr>
                            <td valign="top">
                                <dw:WebDataWindowControl ID="dw_edulist" runat="server" DataWindowObject="dw_hr_employee_edu_list_filemas"
                                    LibraryList="~/DataWindow/hr/hr_master.pbl" ClientScriptable="True" AutoRestoreContext="False"
                                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True" ClientEventClicked ="OnEdulist" >
                                </dw:WebDataWindowControl>
                            </td>
                            <td valign="top">
                                <dw:WebDataWindowControl ID="dw_edudetail" runat="server" DataWindowObject="dw_hr_employee_edu_detail_filemas"
                                    LibraryList="~/DataWindow/hr/hr_master.pbl" ClientScriptable="True" AutoRestoreContext="False"
                                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True">
                                </dw:WebDataWindowControl>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="tab3" style="visibility: hidden; position: absolute;">
                    <table>
                        <tr>
                            <td valign="top">
                                <dw:WebDataWindowControl ID="dw_explist" runat="server" DataWindowObject="dw_hr_employee_expr_list_filemas"
                                    LibraryList="~/DataWindow/hr/hr_master.pbl" ClientScriptable="True" AutoRestoreContext="False"
                                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True" ClientEventClicked ="OnExplist" >
                                </dw:WebDataWindowControl>
                            </td>
                            <td valign="top">
                                <dw:WebDataWindowControl ID="dw_expdetail" runat="server" DataWindowObject="dw_hr_employee_expr_detail_filemas"
                                    LibraryList="~/DataWindow/hr/hr_master.pbl" ClientScriptable="True" AutoRestoreContext="False"
                                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True">
                                </dw:WebDataWindowControl>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="tab4" style="visibility: hidden; position: absolute;">
                    <table>
                        <tr>
                            <td valign="top">
                                <dw:WebDataWindowControl ID="dw_siminarlist" runat="server" DataWindowObject="dw_hr_seminar_list_filemas"
                                    LibraryList="~/DataWindow/hr/hr_master.pbl" ClientScriptable="True" AutoRestoreContext="False"
                                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True" ClientEventClicked ="OnSemilist">
                                </dw:WebDataWindowControl>
                            </td>
                            <td valign="top">
                                <dw:WebDataWindowControl ID="dw_siminardetail" runat="server" DataWindowObject="dw_hr_seminar_detail_filemas"
                                    LibraryList="~/DataWindow/hr/hr_master.pbl" ClientScriptable="True" AutoRestoreContext="False"
                                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True">
                                </dw:WebDataWindowControl>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="tab5" style="visibility: hidden; position: absolute;">
                    <table>
                        <tr>
                            <td valign="top">
                                <dw:WebDataWindowControl ID="dw_wellist" runat="server" DataWindowObject="dw_hr_welfare_employee_list_filemas"
                                    LibraryList="~/DataWindow/hr/hr_master.pbl" ClientScriptable="True" AutoRestoreContext="False"
                                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True" ClientEventClicked ="OnWellist">
                                </dw:WebDataWindowControl>
                            </td>
                            <td valign="top">
                                <dw:WebDataWindowControl ID="dw_weldetail" runat="server" DataWindowObject="dw_hr_welfare_employee_detail_filemas"
                                    LibraryList="~/DataWindow/hr/hr_master.pbl" ClientScriptable="True" AutoRestoreContext="False"
                                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True">
                                </dw:WebDataWindowControl>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="HdfEmplid" Value="" runat="server" />
    <asp:HiddenField ID="Hdpath" Value="" runat="server" />
     <asp:HiddenField ID="Hdftab" Value="" runat="server" />
</asp:Content>
