<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_pm_clean_yield.aspx.cs" Inherits="Saving.Applications.pm.w_sheet_pm_clean_yield" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <br />
    <asp:Panel ID="Panel1" runat="server" ScrollBars="Horizontal" Width="700">
        <dw:WebDataWindowControl ID="DwMain" runat="server" LibraryList="~/DataWindow/pm/pm_investment.pbl"
            DataWindowObject="d_show_clean_yield_detail" ClientScriptable="True" AutoRestoreContext="false"
            AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
            ClientEventButtonClicked="OnDwButtomClicked" ClientEventItemChanged="DwMainItemChange"
            RowsPerPage="30">
            <PageNavigationBarSettings Position="Top" Visible="True" NavigatorType="Numeric">
                <BarStyle HorizontalAlign="Center" />
                <NumericNavigator FirstLastVisible="True" />
            </PageNavigationBarSettings>
        </dw:WebDataWindowControl>
    </asp:Panel>
</asp:Content>
