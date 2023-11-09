<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" 
CodeBehind="w_sheet_kt_viewlog_payout_50bath.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_kt_viewlog_payout_50bath" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript %>
    <%=jsPostMembNo %>
    <script type="text/javascript">
        function OnDwMainItemChanged(sender, rowNumber, columnName, newValue) {
            switch(columnName){
                case "member_no":
                    sender.SetItem(rowNumber, columnName, newValue);
                    sender.AcceptText();
                    jsPostMembNo();
                    break;
            }
        }

        function Validate() {
            return confirm("ต้องเปลี่ยนแปลงสถานะการรอบัญชีใช่หรือไม่");
        }

        function MenubarOpen() {
            Gcoop.OpenDlg(610, 550, "w_dlg_dp_account_search.aspx", "");
        }

        function NewAccountNo(member_no) {
            objDwMain.SetItem(1, "member_no", Gcoop.Trim(member_no));
            objDwMain.AcceptText();
            jsPostMembNo();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>

    <dw:WebDataWindowControl ID="DwMain" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" DataWindowObject="d_kt_viewlog_paymain"
        LibraryList="~/DataWindow/app_assist/kt_50bath.pbl" ClientEventItemChanged="OnDwMainItemChanged"
        ClientFormatting="True">
    </dw:WebDataWindowControl>

    <dw:WebDataWindowControl ID="DwSlip" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" DataWindowObject="d_kt_viewlog_paydetail"
        LibraryList="~/DataWindow/app_assist/kt_50bath.pbl" ClientEventItemChanged="OnDwSlipItemChanged"
        ClientFormatting="True">
    </dw:WebDataWindowControl>
</asp:Content>
