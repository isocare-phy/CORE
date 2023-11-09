<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_bg_slip.aspx.cs"
    Inherits="Saving.Applications.budget.w_sheet_bg_slip" Title="Untitled Page" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=InsertRow%>
    <%=DeleteRow%>
    <%=RetrieveBgType%>
    <%=SetAmt%>
    <%=RetrieveAccId%>
    <%=MemberName%>
    <script type="text/javascript">
        function GetMemberNoFromDlg(memberNo) {
            objDwMain.SetItem(1, "member_no", memberNo);
            objDwMain.AcceptText();
            MemberName();
        }

        function OnClickInsertRow() {
            InsertRow();
        }

        function OnClickDeleteRow() {
            if (objDwList.RowCount() > 0) {
                DeleteRow();
            }
        }

        function OnDwListClicked(sender, rowNumber, object) {
            for (var i = 1; i <= sender.RowCount(); i++) {
                sender.SelectRow(i, false);
            }
            sender.SelectRow(rowNumber, true);
            sender.SetRow(rowNumber);
            Gcoop.GetEl("HdListRow").value = rowNumber + "";
        }

        function OnDwListItemChanged(sender, rowNumber, columnName, newValue) {
            objDwList.SetItem(rowNumber, columnName, newValue);
            objDwList.AcceptText();
            if (columnName == "budgetgroup_code") {
                objDwList.SetItem(rowNumber, columnName, newValue);
                objDwList.AcceptText();
                RetrieveBgType();
            }
            else if (columnName == "itempay_amt") {
//                if (objDwList.GetItem(rowNumber, columnName) > 0) {
                    SetAmt();
//                }
//                else {
//                    alert("จำนวนเงินควรมีค่ามากกว่า 0");
//                }
            }
            else if (columnName == "budgettype_code") {
                RetrieveAccId();
            }
        }

        function OnDwMainButtonClicked(sender, rowNumber, buttonName) {
            if (buttonName == "cb_search") {
                Gcoop.OpenIFrame(700, 550, "w_dlg_bg_member_search.aspx", "");
            }
        }

        function OnDwMainItemChanged(sender, rowNumber, columnName, newValue) {
            if (columnName == "member_no") {
                objDwMain.SetItem(rowNumber, columnName, newValue);
                objDwMain.AcceptText();
                MemberName();
            }
        }

        function Validate() {
            var checkMain = false;
            var checkList = false;
            var operateDate = "";
            var membNo = "";
            var cashType = "";
            var accId = "";
            var paymentDesc = "";

            try {
                operateDate = objDwMain.GetItem(1, "operate_tdate");
            }
            catch (err) { operateDate = ""; }

            if (operateDate != "" && operateDate != null) {
                checkMain = true;
            }
            else {
                checkMain = false;
            }
            var rowList = objDwList.RowCount();

            if (rowList > 0) {
                var bgGrp = "";
                var bgType = "";
                var slipDesc = "";
                var itemPay = 0;
                for (var i = 1; i <= rowList; i++) {
                    try {
                        bgGrp = objDwList.GetItem(i, "budgetgroup_code");
                    } catch (err) { bgGrp = ""; }
                    try {
                        bgType = objDwList.GetItem(i, "budgettype_code");
                    } catch (err) { bgType = ""; }
                    try {
                        slipDesc = objDwList.GetItem(i, "slipitem_desc");
                    } catch (err) { slipDesc = ""; }
                    try {
                        itemPay = objDwList.GetItem(i, "itempay_amt");
                    } catch (err) { itemPay = ""; }
                    if (bgGrp != "" && bgGrp != null && bgType != "" && bgType != null && slipDesc != "" && slipDesc != null && itemPay != null) {
                        checkList = true;
                    }
                    else {
                        checkList = false;
                    }
                }
            }

            if (checkMain == true && checkList == true) {
                return confirm("บันทึกข้อมูล ใช่หรือไม่?");
            }
            else if (rowList < 1 && checkMain == true) {
                alert("กรุณาเพิ่มรายละเอียดเบิก - จ่ายงบประมาณก่อน");
            }
//            else if (rowList > 0 && checkMain == true) {
//                alert("กรุณากรอกข้อมูลรายละเอียดให้ครบถ้วน และจำนวนเงินควรมีค่ามากกว่า 0");
//            }
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
                <asp:Label ID="Label1" runat="server" Text="เบิก - จ่ายงบประมาณ" Font-Bold="True"
                    Font-Names="Tahoma" Font-Size="16px" Font-Underline="True" />
            </td>
        </tr>
    </table>
    <br />
    <dw:WebDataWindowControl ID="DwMain" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" DataWindowObject="d_bg_budgetslip"
        LibraryList="~/DataWindow/budget/budget.pbl" ClientFormatting="True" ClientEventButtonClicked="OnDwMainButtonClicked"
        ClientEventItemChanged="OnDwMainItemChanged">
    </dw:WebDataWindowControl>
    <span onclick="OnClickInsertRow()" style="cursor: pointer;">
        <asp:Label ID="LbInsert2" runat="server" Text="เพิ่มแถว" Font-Bold="False" Font-Names="Tahoma"
            Font-Size="14px" Font-Underline="True" ForeColor="#006600" /></span> &nbsp;&nbsp;
    <span onclick="OnClickDeleteRow()" style="cursor: pointer;">
        <asp:Label ID="LbDel2" runat="server" Text="ลบแถว" Font-Bold="False" Font-Names="Tahoma"
            Font-Size="14px" Font-Underline="True" ForeColor="Red" /></span>
    <dw:WebDataWindowControl ID="DwList" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" DataWindowObject="d_bg_budgetslip_det"
        LibraryList="~/DataWindow/budget/budget.pbl" ClientFormatting="True" RowsPerPage="15"
        ClientEventClicked="OnDwListClicked" ClientEventItemChanged="OnDwListItemChanged">
        <PageNavigationBarSettings NavigatorType="NumericWithQuickGo" Visible="True">
        </PageNavigationBarSettings>
    </dw:WebDataWindowControl>
    <asp:HiddenField ID="HdListRow" runat="server" />
    <asp:HiddenField ID="HdInsert" runat="server" Value="false" />
</asp:Content>
