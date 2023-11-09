<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_ln_process_close_month.aspx.cs" Inherits="Saving.Applications.investment.w_sheet_ln_process_close_month" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/JavaScript">
        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล ?");
        }

        function OnDwMainItemChanged(s, r, c, v) {
            return 0;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_lcsrv_clsmonth_loanoption"
        LibraryList="~/DataWindow/investment/ln_process_close_month.pbl" ClientScriptable="True"
        ClientEvents="true" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" AutoRestoreContext="False" ClientEventItemChanged="OnDwMainItemChanged">
    </dw:WebDataWindowControl>
</asp:Content>
