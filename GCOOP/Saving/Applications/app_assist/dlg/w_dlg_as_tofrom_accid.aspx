<%@ Page Title="" Language="C#" MasterPageFile="~/FrameDialog.Master" AutoEventWireup="true"
    CodeBehind="w_dlg_as_tofrom_accid.aspx.cs" Inherits="Saving.Applications.app_assist.dlg.w_dlg_as_tofrom_accid" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function OnDwMainClicked(sender, rowNumber, objectName) {
            var account_id = sender.GetItem(rowNumber, "account_id");
            parent.GetToFromAccid(account_id);
            parent.RemoveIFrame();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_as_capital_tofrom_accid"
        LibraryList="~/DataWindow/app_assist/as_capital.pbl" AutoRestoreContext="False"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientScriptable="True" ClientEventClicked="OnDwMainClicked">
    </dw:WebDataWindowControl>
</asp:Content>
