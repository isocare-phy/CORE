<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    ValidateRequest="false" CodeBehind="w_sheet_hr_tax.aspx.cs" Inherits="Saving.Applications.hr.w_sheet_hr_tax" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript %>
    <%=getNameId%>
    <%=getProcess%>
    <%=getProcess91%>
    <%=checkFlag %>
    <%=getCheckprocess %>
    <%=getCheckprocess91%>
    <script type="text/javascript">
        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล?");
        }
        function MenubarOpen() {
            Gcoop.OpenDlg('590', '600', 'w_dlg_hr_master_search.aspx', '');
        }

        function ChangedMain(s, r, c, v) {
            if (c == "empid") {
                objDwMain.SetItem(1, c, v);
                objDwMain.AcceptText();
                getNameId();
            }
        }

        function Click_Process(s, r, c) {
            if (c == "b_process") {
                var year = objdw_process.GetItem(1, "year_pay");
                var month = objdw_process.GetItem(1, "month_pay");
                var check_year = objdw_process.GetItem(1, "check_year");
                if (check_year == 0) {
                    alert("คำนวณภาษีประจำปี " + year + " ระบบจะแสดงภาษีประจำปีผ่านทางรายงาน");
                    getCheckprocess91();
                }
                else {
                    alert("คำนวณภาษีประจำเดือน    " + month + "   ปี    " + year);
                    getCheckprocess();
                }
            }
        }

        function OnDwCriteriaItemsClicked(sender, rowNumber, objectName) {
            Gcoop.CheckDw(sender, rowNumber, objectName, "check_year", 1, 0);
            if (objectName == "check_year") {
                //objdw_criteria.AcceptText();
                checkFlag();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="dw_process" runat="server" AutoRestoreContext="False"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="true"
        DataWindowObject="dw_hr_tax_main" LibraryList="~/DataWindow/hr/hr_payroll.pbl"
        Width="750px" Height="150px" ClientEventButtonClicked="Click_Process" ClientEventClicked="OnDwCriteriaItemsClicked">
    </dw:WebDataWindowControl>
    <dw:WebDataWindowControl ID="DwMain" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="true" DataWindowObject="dw_hr_main"
        LibraryList="~/DataWindow/hr/hr_payroll.pbl" Width="750px" Height="40px" ClientEventItemChanged="ChangedMain"
        ClientEventClicked="OnDwCriteriaItemsClicked">
    </dw:WebDataWindowControl>
    <br />
    <dw:WebDataWindowControl ID="DwDetail" runat="server" DataWindowObject="dw_hr_taxrate_a"
        LibraryList="~/DataWindow/hr/hr_payroll.pbl" AutoRestoreContext="False" AutoRestoreDataCache="True"
        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True">
    </dw:WebDataWindowControl>
    <asp:Literal ID="Literal1" runat="server"></asp:Literal>
</asp:Content>
