<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_as_membercard.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_as_membercard" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript %>

    <script type="text/javascript">
    
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_sl_print_membercard_criteria"
        LibraryList="~/DataWindow/app_assist/as_seminar.pbl" ClientEventButtonClicked="DwMainButtonClick"
        ClientScriptable="True" AutoRestoreContext="false" AutoRestoreDataCache="true"
        AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True">
    </dw:WebDataWindowControl>
    <br />
    <dw:WebDataWindowControl ID="DwCard" runat="server" DataWindowObject="d_sl_print_membercard_nup"
        LibraryList="~/DataWindow/app_assist/as_seminar.pbl" ClientScriptable="True"
        AutoRestoreContext="false" AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True">
    </dw:WebDataWindowControl>
</asp:Content>
