<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_bg_budgetyear.aspx.cs"
    Inherits="Saving.Applications.account.w_sheet_bg_budgetyear" Title="Untitled Page" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=InsertRow%>
    <%=DeleteRow%>
    <%=SetStartEndDate%>
    <script type="text/javascript">
        function OnClickInsertRow() {
            InsertRow();
        }

        function OnClickDeleteRow() {
            if (objDwMain.RowCount() > 0) {
                if (confirm("ลบข้อมูล ใช่หรือไม่?")) {
                    DeleteRow();
                }
            }
        }

        function OnDwMainItemChanged(sender, rowNumber, columnName, newValue) {
            Gcoop.GetEl("HdcolumnName").value = columnName;
            if (columnName == "beginning_of_tbudget" || columnName == "ending_of_tbudget") {
                objDwMain.SetItem(rowNumber, columnName, newValue);
                objDwMain.AcceptText();
                return 0;
            }
            else if (columnName == "budgetyear") {
                objDwMain.SetItem(rowNumber, columnName, newValue);
                objDwMain.AcceptText();
                SetStartEndDate();
            }
            else if (columnName == "budget_status") {
                objDwMain.SetItem(rowNumber, columnName, newValue);
                objDwMain.AcceptText();
            }
        }

        function OnDwMainClicked(sender, rowNumber, objectName) {
            for (var i = 1; i <= sender.RowCount(); i++) {
                sender.SelectRow(i, false);
            }
            sender.SelectRow(rowNumber, true);
            sender.SetRow(rowNumber);
            Gcoop.GetEl("HdMainRow").value = rowNumber + "";
            Gcoop.GetEl("Hdyear").value = sender.GetItem(rowNumber, "budgetyear");
            Gcoop.GetEl("Hdstatus").value = sender.GetItem(rowNumber, "budget_status");
        }

        function Validate() {
            var check = false;
            var bgYear = "";
            var beginDate = "";
            var endDate = "";
            for (var i = 1; i <= objDwMain.RowCount(); i++) {

                try {
                    bgYear = objDwMain.GetItem(i, "budgetyear");
                }
                catch (err) { bgYear = ""; }
                try {
                    beginDate = objDwMain.GetItem(i, "beginning_of_tbudget");
                }
                catch (err) { beginDate = ""; }
                try {
                    endDate = objDwMain.GetItem(i, "ending_of_tbudget");
                }
                catch (err) { endDate = ""; }

                if (bgYear != "" && bgYear != null && beginDate != "" && beginDate != null && endDate != "" && endDate != null) {
                    check = true;
                }
                else {
                    check = false;
                    break;
                }
            }
            if (check == true) {
                return confirm("บันทึกข้อมูล ใช่หรือไม่?");
            }
            else if (objDwMain.RowCount() < 1) {
                alert("ไม่มีข้อมูล กรุณาทำการเพิ่มแถวก่อน");
            }
            else {
                alert("กรุณากรอกข้อมูลให้ครบถ้วน");
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <br />
    <table width="100%">
        <tr>
            <td align="left">
                <asp:Label ID="Label1" runat="server" Text="ปีงบประมาณ" Font-Bold="True" Font-Names="Tahoma"
                    Font-Size="16px" Font-Underline="True" />
            </td>
        </tr>
        <tr>
            <td>
                <br />
                <span onclick="OnClickInsertRow()" style="cursor: pointer;">
                    <asp:Label ID="LbInsert2" runat="server" Text="เพิ่มแถว" Font-Bold="False" Font-Names="Tahoma"
                        Font-Size="14px" Font-Underline="True" ForeColor="#006600" /></span> &nbsp;&nbsp;
                <span onclick="OnClickDeleteRow()" style="cursor: pointer;">
                    <asp:Label ID="LbDel2" runat="server" Text="ลบแถว" Font-Bold="False" Font-Names="Tahoma"
                        Font-Size="14px" Font-Underline="True" ForeColor="Red" /></span>
            </td>
        </tr>
    </table>
    <dw:WebDataWindowControl ID="DwMain" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" DataWindowObject="d_bg_budgetyear"
        LibraryList="~/DataWindow/budget/budget.pbl" ClientFormatting="True" ClientEventClicked="OnDwMainClicked"
        RowsPerPage="17" ClientEventItemChanged="OnDwMainItemChanged">
        <PageNavigationBarSettings NavigatorType="NumericWithQuickGo" Visible="True">
        </PageNavigationBarSettings>
    </dw:WebDataWindowControl>
    <asp:HiddenField ID="HdMainRow" runat="server" />
    <asp:HiddenField ID="HdcolumnName" runat="server" />
    <asp:HiddenField ID="Hdyear" runat="server" />
    <asp:HiddenField ID="Hdstatus" runat="server" />
</asp:Content>
