<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_as_clearloan.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_as_clearloan_benefit" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function DwMainEventClick(sender, rowNumber, column) {
            if (column == "assist_docno" || column == "membgroup_code" || column == "member_no" || column == "withdrawable_amt" || column == "clearloan_amt" || column == "clearloan_int") {
                Gcoop.GetEl("HdSelectedRow").value = rowNumber;
                JsPostAssDocno();
            }
        }

        function DwMainBtnClick(sender, rowNumber, buttonName) {

        }

        function DwDetailBtnClick(sender, rowNumber, buttonName) {
            if (buttonName == "b_clearloan") {
                var assist_docno = objDw_Detail.GetItem(1, "assist_docno");
                if (assist_docno == "" || assist_docno == null) {
                    alert("กรุณาเลือกรายการ");
                } else {
                    if (confirm("ยืนยันการทำรายการ")) {
                        objDw_Detail.AcceptText();
                        JsPostTranLoan();
                    }
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <table style="width: 100%;">
        <tr>
            <td valign="top">
                <asp:Panel ID="Panel1" runat="server">
                    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_as_clearloan_list"
                        LibraryList="~/DataWindow/app_assist/as_clearloan.pbl" AutoRestoreContext="False"
                        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
                        ClientScriptable="True" ClientEventButtonClicked="DwMainBtnClick" ClientEventClicked="DwMainEventClick"
                        ClientEventItemChanged="DwMainItemChange">
                    </dw:WebDataWindowControl>
                </asp:Panel>
            </td>
            <td>
                <asp:Panel ID="Panel2" runat="server">
                    <dw:WebDataWindowControl ID="Dw_Detail" runat="server" DataWindowObject="d_as_payassist_detail"
                        LibraryList="~/DataWindow/app_assist/as_payout.pbl" AutoRestoreContext="False"
                        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
                        ClientScriptable="True" ClientEventButtonClicked="DwDetailBtnClick">
                    </dw:WebDataWindowControl>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="HdSelectedRow" runat="server" />
</asp:Content>
