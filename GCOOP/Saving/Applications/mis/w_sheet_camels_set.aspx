<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_camels_set.aspx.cs" Inherits="Saving.Applications.mis.w_sheet_camels_set" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=postRatioId %>
    <%=postGroupId%>
    <%=postGroupBy%>
    <%=postGroupOut%>
    <%=saveData %>
    <%=popupReport%>
    <script type="text/javascript">
        var rowNumSend = 0;
        function showTabPage(tab) {
            var i = 1;
            var tabamount = 3;
            for (i = 1; i <= tabamount; i++) {
                document.getElementById("tab_" + i).style.visibility = "hidden";
                document.getElementById("stab_" + i).style.backgroundColor = "rgb(200,235,255)";
                if (i == tab) {
                    document.getElementById("tab_" + i).style.visibility = "visible";
                    document.getElementById("stab_" + i).style.backgroundColor = "rgb(211,213,255)";
                    document.getElementById("ctl00_ContentPlace_HdCurrentTap").value = i + "";
                }
            }
        }

        function MenubarSave() {
            if (confirm("ยืนยันการบันทึกข้อมูล")) {
                saveData();
            }
        }

        function OnDeleteRowTap1(sender, rowNumber, buttonName) {
            if (confirm("คุณต้องการลบ ใช่หรือไม่??")) {
                objdw_tap1.DeleteRow(rowNumber);
            }
        }

        function OnInsertRowTap1() {
            objdw_tap1.InsertRow(objdw_tap1.RowCount() + 1);
        }

        function OnInsertRowTap21() {
            objdw_tap21.InsertRow(objdw_tap21.RowCount() + 1);
        }

        function OnInsertRowTap3() {
            objdw_tap3.InsertRow(objdw_tap3.RowCount() + 1);
        }

        function OnOpenDlgVar(sender, rowNumber, buttonName) {
            rowNumSend = rowNumber;
            //objdw_tap1.InsertRow(0);
            //opendlg("725", "600", "w_dlg_camels_var.aspx", "?rowNumber=" + rowNumber);
            Gcoop.OpenDlg("725", "600", "w_dlg_camels_var.aspx", "?rowNumber=" + rowNumber);
        }
        function GetMoneysheet(moneysheet_code, moneysheet_seq, rowNumber) {
            objdw_tap1.SetItem(rowNumSend, "moneysheet_code", moneysheet_code);
            objdw_tap1.SetItem(rowNumSend, "moneysheet_seq", moneysheet_seq);
            objdw_tap1.SetItem(rowNumSend, "description", description);
        }

        function OnOpenDlgOperand(sender, rowNumber, buttonName) {
            if (objdw_tap22.RowCount() >= 2) {
                alert("เพิ่มตัวแปรครบแล้ว");
                return 1;
            }
            if ((objdw_tap22.RowCount() >= 1) && (objdw_tap22.GetItem(1, "ratio_id") != null)) {
                objdw_tap22.InsertRow(0);
            }
            //opendlg("750", "600", "w_dlg_camels_operand.aspx", "");
            Gcoop.OpenDlg("750", "600", "w_dlg_camels_operand.aspx", "");
        }

        function OnOpenDlgGroups(sender, rowNumber, buttonName) {
            if ((objdw_tap31.RowCount() >= 1) && (objdw_tap31.GetItem(1, "ratio_id") != null)) {
                objdw_tap31.InsertRow(0);
            }
            var GroupBy = document.getElementById("ctl00_ContentPlace_HdGroupBy").value;
            //opendlg("260", "250", "w_dlg_camels_groups.aspx", "?GroupBy=" + GroupBy);
            Gcoop.OpenDlg("260", "250", "w_dlg_camels_groups.aspx", "?GroupBy=" + GroupBy);
        }

        function OnRatioIdClick(sender, rowNumber, objectName) {
            var str_temp = window.location.toString();
            var str_arr = str_temp.split("?", 2);
            var RatioId = objdw_tap21.GetItem(rowNumber, "ratio_id");
            document.getElementById("ctl00_ContentPlace_HdRatioId").value = RatioId;
            postRatioId();
        }

        function OnGroupIdClick(sender, rowNumber, objectName) {
            var str_temp = window.location.toString();
            var str_arr = str_temp.split("?", 2);
            var GroupId = objdw_tap3.GetItem(rowNumber, "ratgroup_id");
            document.getElementById("ctl00_ContentPlace_HdGroupBy").value = GroupId;
            document.getElementById("ctl00_ContentPlace_HdGroupId").value = GroupId;
            postGroupId();
        }

        function GetValueFromDlgVar(moneysheet_code, moneysheet_seq, description, rownum) {
            var str_temp = window.location.toString();
            var str_arr = str_temp.split("?", 2);
            objdw_tap1.SetItem(rownum, "moneysheet_code", Trim(moneysheet_code));
            objdw_tap1.SetItem(rownum, "moneysheet_seq", moneysheet_seq);
            objdw_tap1.SetItem(rownum, "description", Trim(description));
        }

        function GetValueFromDlgOperand(VarId, VarName, rownum) {
            var str_temp = window.location.toString();
            var str_arr = str_temp.split("?", 2);
            var RatioId = document.getElementById("ctl00_ContentPlace_HdRatioId").value;
            if (RatioId == "") {
                RatioId = 1;
            }
            objdw_tap22.SetItem(objdw_tap22.RowCount(), "ratio_id", RatioId);
            objdw_tap22.SetItem(objdw_tap22.RowCount(), "ratvar_id", VarId);
            objdw_tap22.SetItem(objdw_tap22.RowCount(), "ratvar_alias", VarName);
        }

        function GetValueFromDlgGroups(ratio_id, ratio_name, rownum) {
            var str_temp = window.location.toString();
            var str_arr = str_temp.split("?", 2);
            var GroupBy = document.getElementById("ctl00_ContentPlace_HdGroupBy").value;
            if (GroupBy == "") {
                document.getElementById("ctl00_ContentPlace_HdGroupBy").value = 1;
            }
            document.getElementById("ctl00_ContentPlace_HdGroupRatioId").value = ratio_id;
            objdw_tap31.SetItem(objdw_tap31.RowCount(), "ratio_id", ratio_id);
            objdw_tap31.SetItem(objdw_tap31.RowCount(), "ratio_name", ratio_name);
            postGroupBy();
        }

        function OnGroupOut(sender, rowNumber, objectName) {
            var RatioId = objdw_tap31.GetItem(rowNumber, "ratio_id");
            document.getElementById("ctl00_ContentPlace_HdGroupRatioId").value = RatioId;
            document.getElementById("ctl00_ContentPlace_HdGroupBy").value = 0;
            postGroupOut();
        }
        //ฟังก์ชั่นรายงาน**********************************************************************
        function OnClickLinkNext() {

            objdw_tap1.AcceptText();
//          alert("aaaa");  //DEBUG
            popupReport();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <span style="cursor: pointer" onclick="OnClickLinkNext();">-พิมพ์รายงานกำหนดตัวแปรสำหรับอัตราส่วน</span>
    <br />
    <table style="width: 100%; border: solid 1px; margin-top: 5px">
        <tr align="center" class="dwtab">
            <td style="background-color: rgb(211,213,255); cursor: pointer;" id="stab_1" width="33%"
                onclick="showTabPage(1);" width="20%">
                กำหนดตัวแปรสำหรับอัตราส่วน
            </td>
            <td style="background-color: rgb(200,235,255); cursor: pointer;" id="stab_2" width="33%"
                onclick="showTabPage(2);" width="20%">
                กำหนดอัตราส่วน
            </td>
            <td style="background-color: rgb(200,235,255); cursor: pointer;" id="stab_3" width="33%"
                onclick="showTabPage(3);" width="20%">
                จัดกลุ่มอัตราส่วน
            </td>
        </tr>
    </table>
    <table style="width: 100%; height: 600px; border: solid 1px; margin-top: 2px" class="dwcontent">
        <tr>
            <td style="height: 200px;" valign="top">
                <div id="tab_1" style="visibility: visible; position: absolute;">
                    <span class="linkSpan" onclick="OnInsertRowTap1()">เพิ่มตัวแปร</span>    <br />
                    <dw:WebDataWindowControl ID="dw_tap1" runat="server" DataWindowObject="d_mis_ratio_variables"
                        LibraryList="~/DataWindow/mis/camels_set.pbl" ClientScriptable="True" ClientEventButtonClicked="OnOpenDlgVar"
                        AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
                        Width="740" VerticalScrollBar="Auto" BorderWidth="0" Height="550">
                    </dw:WebDataWindowControl>
                    <span class="linkSpan" onclick="OnOpenDlg()">เพิ่มตัวแปร</span>
                </div>
                <div id="tab_2" style="visibility: hidden; position: absolute;">
                    <table width="600px">
                        <tr>
                            <td valign="top" width="50%">
                                <span class="linkSpan" onclick="OnInsertRowTap21()">เพิ่มอัตราส่วน</span> <br />
                                <dw:WebDataWindowControl ID="dw_tap21" runat="server" DataWindowObject="d_mis_ratio_ratios"
                                    LibraryList="~/DataWindow/mis/camels_set.pbl" ClientScriptable="True" OnAfterPerformAction="dw_tap21_AfterPerformAction"
                                    ClientEventButtonClicked="OnRatioIdClick" AutoRestoreContext="False" AutoRestoreDataCache="True"
                                    AutoSaveDataCacheAfterRetrieve="True" ClientEventClicked="OnGetRow">
                                </dw:WebDataWindowControl>
                            </td>
                            <td valign="top" width="50%" height="100%">
                                <table>
                                    <tr>
                                        <td>
                                            <span class="linkSpan" onclick="OnOpenDlgOperand()">เพิ่มตัวแปร</span>   <br />
                                            <dw:WebDataWindowControl ID="dw_tap22" runat="server" DataWindowObject="d_mis_ratio_operand"
                                                LibraryList="~/DataWindow/mis/camels_set.pbl" ClientScriptable="True" ClientEventButtonClicked="OnOpenDlgOperand"
                                                OnAfterPerformAction="dw_tap22_AfterPerformAction" AutoRestoreContext="False"
                                                AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True">
                                            </dw:WebDataWindowControl>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <br />
                                            <dw:WebDataWindowControl ID="dw_tap23" runat="server" DataWindowObject="d_mis_ratio_function"
                                                LibraryList="~/DataWindow/mis/camels_set.pbl" ClientScriptable="True">
                                            </dw:WebDataWindowControl>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="tab_3" style="visibility: hidden; position: absolute;">
                    <table>
                        <td valign="top">
                            <span class="linkSpan" onclick="OnInsertRowTap3()">เพิ่มกลุ่ม</span>   <br />
                            <dw:WebDataWindowControl ID="dw_tap3" runat="server" DataWindowObject="d_mis_ratio_group"
                                LibraryList="~/DataWindow/mis/camels_set.pbl" ClientScriptable="True" ClientEventButtonClicked="OnGroupIdClick"
                                AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
                                OnAfterPerformAction="dw_tap3_AfterPerformAction">
                            </dw:WebDataWindowControl>
                        </td>
                        <td valign="top">
                            <span class="linkSpan" onclick="OnOpenDlgGroups()">เพิ่มอัตราส่วน</span>   <br />
                            <dw:WebDataWindowControl ID="dw_tap31" runat="server" DataWindowObject="d_mis_ratio_ratios_bygroup"
                                LibraryList="~/DataWindow/mis/camels_set.pbl" ClientEventButtonClicked="OnGroupOut"
                                ClientScriptable="True" OnAfterPerformAction="dw_tap31_AfterPerformAction" AutoRestoreContext="False"
                                AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True">
                            </dw:WebDataWindowControl>
                        </td>
                    </table>
                </div>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="HdRow" runat="server" />
    <asp:HiddenField ID="HdGroupRatioId" runat="server" />
    <asp:HiddenField ID="HdGroupBy" runat="server" />
    <asp:HiddenField ID="HdCurrentTap" runat="server" />
    <asp:HiddenField ID="HdRatioId" runat="server" />
    <asp:HiddenField ID="HdGroupId" runat="server" />
    <asp:HiddenField ID="HdOpenIFrame" runat="server" Value="False" />
    <asp:HiddenField ID="HdcheckPdf" runat="server" Value="False" />
    <asp:HiddenField ID="HdIsPostBack" runat="server" Value="false" />
    <asp:HiddenField ID="Hdcommitreport" runat="server" Value="false" />
    <asp:Literal ID="LtChangeTap" runat="server"></asp:Literal>
</asp:Content>
