<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_pm_financial_investment.aspx.cs" Inherits="Saving.Applications.pm.w_sheet_pm_financial_investment" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%=RunProcess%>
<%=GetValue%>
    <script type="text/javascript">
        function MenubarNew() {
            if (confirm("ยืนยันการล้างหน้าจอ")) {
                window.location = "";
            }
        }
        function OnDwButtomClicked(s, r, c, v) {
            switch (c) {
                case "b_1":
                    RunProcess();
                    break;
                case "b_2":
                    GetValue();
                    break;
            }
        }
        function DwMainItemChange(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            if (c == "setting_type") {
                SettingTypeChange();
            }
            if (c == "year_setting") {
                YearSettingChange();
            }
        }
       </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <br />
        <dw:WebDataWindowControl ID="DwMain" runat="server" LibraryList="~/DataWindow/pm/pm_investment.pbl"
        DataWindowObject="d_finance_cri" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientEventButtonClicked="OnDwButtomClicked" ClientEventItemChanged="DwMainItemChange">
    </dw:WebDataWindowControl>
<br />
<br />
    <asp:Panel ID="Panel1" runat="server" ScrollBars="Horizontal"  Width="700">
        <dw:WebDataWindowControl ID="DwDetail" runat="server" LibraryList="~/DataWindow/pm/pm_investment.pbl"
        DataWindowObject="d_pm_short" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True">
    </dw:WebDataWindowControl>
    </asp:Panel>
</asp:Content>
