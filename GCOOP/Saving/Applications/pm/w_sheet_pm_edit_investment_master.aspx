<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_pm_edit_investment_master.aspx.cs" Inherits="Saving.Applications.pm.w_sheet_pm_edit_investment_master" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<%=postAccountNo%>
<%=postBankCode%>
<%=postNodueFlag%>
<%=postBankAccountNo %>
<%=postNull %>
    <script type="text/javascript">
        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล");
        }
        function MenubarOpen() {
            Gcoop.OpenIFrame("630", "350", "w_dlg_account_list.aspx", "");
        }
        function MenubarNew() {
            if (confirm("ยืนยันการล้างข้อมูลบนหน้าจอ")) {
                window.location = "";
            }
        }
        function OnDwButtomClicked(s, r, c, v) {
            switch (c) {
                case "b_1":
                    RunProcess();
                    break;
            }
        }
        function ChooseAcc(account_no) {
            objDwMain.SetItem(1, "account_no", account_no);
            objDwMain.AcceptText();
            postAccountNo();
        }
        function DwMainItemChange(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            if (c == "account_no") {
                postAccountNo();
            }
            else if (c == "bank_code") {
                postBankCode();
            }
            else if (c == "tran_bankacc_no") {
                postBankAccountNo();
            }
            else if (c == "close_status") {
                postNull();
            }
            if (c == "nodue_flag") {
                postNodueFlag();
            }
        }
       </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <br />
        <dw:WebDataWindowControl ID="DwMain" runat="server" LibraryList="~/DataWindow/pm/pm_investment.pbl"
        DataWindowObject="d_invest_master1" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientEventButtonClicked="OnDwButtomClicked" ClientEventItemChanged="DwMainItemChange">
    </dw:WebDataWindowControl>
<br />
   <asp:HiddenField ID="HdDueDate" runat="server" Value="" />
</asp:Content>
