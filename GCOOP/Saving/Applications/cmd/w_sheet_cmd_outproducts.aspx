<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" 
CodeBehind="w_sheet_cmd_outproducts.aspx.cs" Inherits="Saving.Applications.cmd.w_sheet_cmd_outproducts" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
<asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
        <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_outproducts_detail"
            LibraryList="~/DataWindow/Cmd/cmd_outproducts.pbl" AutoRestoreContext="False" AutoRestoreDataCache="True"
            AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" 
            PageNavigationBarSettings-NavigatorType="Numeric" PageNavigationBarSettings-Visible="false">
        </dw:WebDataWindowControl>
</asp:Content>
