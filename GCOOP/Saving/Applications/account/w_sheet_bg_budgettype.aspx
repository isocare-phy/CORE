<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_bg_budgettype.aspx.cs"
    Inherits="Saving.Applications.account.w_sheet_bg_budgettype" Title="Untitled Page" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=InsertRow%>
    <%=DeleteRow%>
    <%=RetrieveList%>
    <%=SetFormat%>
    <script type="text/javascript">
        function OnClickInsertRow() {
            if (Gcoop.GetEl("HdInsert").value == "true") {
                InsertRow();
            }
        }

        function OnClickDeleteRow() {
            if (objDwList.RowCount() > 0) {
                if (confirm("ลบข้อมูล ใช่หรือไม่?")) {
                    DeleteRow();
                }
            }
        }

        function OnDwMainButtonClicked(sender, rowNumber, buttonName) {
            if (buttonName == "cb_ok") {
                var bgGrpCode = "";

                try {
                    bgGrpCode = objDwMain.GetItem(1, "bggrp_code");
                } catch (err) { bgGrpCode = ""; }

                if (bgGrpCode != null && bgGrpCode != "") {
                    Gcoop.GetEl("HdInsert").value = "true";
                    RetrieveList();
                }
                else {
                    alert("กรุณาเลือกหมวดงบประมาณก่อน");
                }
            }
        }

        function OnDwListClicked(sender, rowNumber, object) {
            for (var i = 1; i <= sender.RowCount(); i++) {
                sender.SelectRow(i, false);
            }
            sender.SelectRow(rowNumber, true);
            sender.SetRow(rowNumber);
            Gcoop.GetEl("HdListRow").value = rowNumber + "";
            Gcoop.GetEl("Hdgroup").value = sender.GetItem(rowNumber, "budgetgroup_code");
            Gcoop.GetEl("Hdtype").value = sender.GetItem(rowNumber, "budgettype_code");
        }

        function OnDwListItemChanged(sender, rowNumber, columnName, newValue) {
            if (columnName == "budgettype_code") {
                objDwList.SetItem(rowNumber, columnName, newValue);
                objDwList.AcceptText();
                if (Gcoop.IsNum(objDwList.GetItem(rowNumber, columnName))) {
                    SetFormat();
                }
                else {
                    alert("กรุณากรอกรหัสประเภทงบประมาณเป็นตัวเลข");
                }
            }
        }

        function Validate() {
            var check = false;
            var checkType = false;
            var bgGrpType = "";
            var bgGrpDesc = "";
            for (var i = 1; i <= objDwList.RowCount(); i++) {
                try {
                    bgGrpType = objDwList.GetItem(i, "budgettype_code");
                }
                catch (err) { bgGrpCode = ""; }
                try {
                    bgGrpDesc = objDwList.GetItem(i, "budgettype_desc");
                }
                catch (err) { bgGrpDesc = ""; }

                checkType = Gcoop.IsNum(bgGrpType);

                if (bgGrpType != "" && bgGrpType != null && bgGrpDesc != "" && bgGrpDesc != null && checkType != false) {
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
            else if (objDwList.RowCount() < 1) {
                alert("ไม่มีข้อมูล กรุณาทำการเพิ่มแถวก่อน");
            }
            else if (checkType == false) {
                alert("กรุณากรอกรหัสหมวดเป็นตัวเลข");
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
                <asp:Label ID="Label1" runat="server" Text="ประเภทงบประมาณ" Font-Bold="True" Font-Names="Tahoma"
                    Font-Size="16px" Font-Underline="True" />
            </td>
        </tr>
    </table>
    <br />
    <dw:WebDataWindowControl ID="DwMain" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" DataWindowObject="d_bg_budgettype_main"
        LibraryList="~/DataWindow/budget/budget.pbl" ClientFormatting="True" ClientEventButtonClicked="OnDwMainButtonClicked">
    </dw:WebDataWindowControl>
    <span onclick="OnClickInsertRow()" style="cursor: pointer;">
        <asp:Label ID="LbInsert2" runat="server" Text="เพิ่มแถว" Font-Bold="False" Font-Names="Tahoma"
            Font-Size="14px" Font-Underline="True" ForeColor="#006600" /></span> &nbsp;&nbsp;
    <span onclick="OnClickDeleteRow()" style="cursor: pointer;">
        <asp:Label ID="LbDel2" runat="server" Text="ลบแถว" Font-Bold="False" Font-Names="Tahoma"
            Font-Size="14px" Font-Underline="True" ForeColor="Red" /></span>
    <dw:WebDataWindowControl ID="DwList" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" DataWindowObject="d_bg_budgettype_list"
        LibraryList="~/DataWindow/budget/budget.pbl" ClientFormatting="True" RowsPerPage="15"
        ClientEventClicked="OnDwListClicked" ClientEventItemChanged="OnDwListItemChanged">
        <PageNavigationBarSettings NavigatorType="NumericWithQuickGo" Visible="True">
        </PageNavigationBarSettings>
    </dw:WebDataWindowControl>
    <asp:HiddenField ID="HdListRow" runat="server" />
    <asp:HiddenField ID="HdInsert" runat="server" Value="false" />
    <asp:HiddenField ID="Hdgroup" runat="server" Value="false" />
    <asp:HiddenField ID="Hdtype" runat="server" Value="false" />
</asp:Content>
