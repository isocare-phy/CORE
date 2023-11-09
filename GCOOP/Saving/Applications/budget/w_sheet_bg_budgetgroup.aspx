<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_bg_budgetgroup.aspx.cs"
    Inherits="Saving.Applications.budget.w_sheet_bg_budgetgroup" Title="Untitled Page" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=InsertRow%>
    <%=DeleteRow%>
    <%=SetFormat%>
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

        function OnDwMainClicked(sender, rowNumber, objectName) {
            for (var i = 1; i <= sender.RowCount(); i++) {
                sender.SelectRow(i, false);
            }
            sender.SelectRow(rowNumber, true);
            sender.SetRow(rowNumber);
            Gcoop.GetEl("HdMainRow").value = rowNumber + "";
            Gcoop.GetEl("Hdcode").value = sender.GetItem(rowNumber, "budgetgroup_code");
        }

        function OnDwMainItemChanged(sender, rowNumber, columnName, newValue) {
            if (columnName == "budgetgroup_code") {
                objDwMain.SetItem(rowNumber, columnName, newValue);
                objDwMain.AcceptText();
                if (Gcoop.IsNum(objDwMain.GetItem(rowNumber, columnName))) {
                    SetFormat();
                }
                else {
                    alert("กรุณากรอกรหัสหมวดเป็นตัวเลข");
                }
            }
        }

        function Validate() {
            var check = false;
            var checkCode = false;
            var bgGrpCode = "";
            var bgGrpDesc = "";
            for (var i = 1; i <= objDwMain.RowCount(); i++) {

                try {
                    bgGrpCode = objDwMain.GetItem(i, "budgetgroup_code");
                }
                catch (err) { bgGrpCode = ""; }
                try {
                    bgGrpDesc = objDwMain.GetItem(i, "budgetgroup_desc");
                }
                catch (err) { bgGrpDesc = ""; }

                checkCode = Gcoop.IsNum(bgGrpCode);

                if (bgGrpCode != "" && bgGrpCode != null && bgGrpDesc != "" && bgGrpDesc != null && checkCode != false) {
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
            else if (checkCode == false) {
                alert("กรุณากรอกรหัสหมวดเป็นตัวเลข");
            }
            else {
                alert("กรุณากรอกข้อมูลให้ครบถ้วน");
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:HiddenField ID="Hdcode" runat="server" />
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <br />
    <table width="100%">
        <tr>
            <td align="left">
                <asp:Label ID="Label1" runat="server" Text="หมวดงบประมาณ" Font-Bold="True" Font-Names="Tahoma"
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
        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" DataWindowObject="d_bg_budgetgroup"
        LibraryList="~/DataWindow/budget/budget.pbl" ClientFormatting="True" ClientEventClicked="OnDwMainClicked"
        RowsPerPage="17" ClientEventItemChanged="OnDwMainItemChanged">
        <PageNavigationBarSettings NavigatorType="NumericWithQuickGo" Visible="True">
        </PageNavigationBarSettings>
    </dw:WebDataWindowControl>
    <asp:HiddenField ID="HdMainRow" runat="server" />
</asp:Content>
