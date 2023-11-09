<%@ Page Title="" Language="C#" MasterPageFile="~/FrameDialog.Master" AutoEventWireup="true" CodeBehind="w_dlg_as_senior_assist_paylist.aspx.cs" Inherits="Saving.Applications.app_assist.dlg.w_dlg_as_senior_assist_paylist" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
<asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_as_senio_assist_paylist"
        LibraryList="~/DataWindow/app_assist/as_capital.pbl" AutoRestoreContext="False"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientScriptable="True" Height="275" Width="485">
    </dw:WebDataWindowControl>
</asp:Content>
