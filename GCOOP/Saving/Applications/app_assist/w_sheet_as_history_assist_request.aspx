<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_as_history_assist_request.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_as_history_assist_request" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript %>
    <%=postGetMemberDetail %>
    <script type="text/javascript">
        function DwMainItemChange(sender, rowNumber, columnName, newValue) {
            if (columnName == "member_no") {
                newValue = Gcoop.StringFormat(newValue, "00000000");
                Gcoop.GetEl("HdMemNo").value = newValue;
                postGetMemberDetail();
            }
            function DwMainButtonClick(sender, rowNumber, buttonName) {
                if (buttonName == "b_search") {
                    Gcoop.OpenDlg(610, 550, "w_dlg_sl_member_search.aspx", "");
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <asp:Literal ID="LtAlert" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_as_member_detail"
        LibraryList="~/DataWindow/app_assist/as_capital.pbl" ClientScriptable="True"
        AutoRestoreContext="false" AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" ClientEventItemChanged="DwMainItemChange" ClientEventButtonClicked="DwMainButtonClick">
    </dw:WebDataWindowControl>
    <dw:WebDataWindowControl ID="DwDetail" runat="server" DataWindowObject="d_as_history_assist_req"
        LibraryList="~/DataWindow/app_assist/as_capital.pbl" ClientScriptable="True"
        AutoRestoreContext="false" AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" ClientEventItemChanged="DwDetailItemChange" ClientEventButtonClicked="DwDetailButtonClick">
    </dw:WebDataWindowControl>
    <asp:HiddenField ID="HdMemNo" runat="server" />
</asp:Content>
