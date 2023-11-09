<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_td_search_slip_apdebt.aspx.cs"
    Inherits="Saving.Applications.trading.dlg.w_dlg_td_search_slip_apdebt" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <%=jsSearchData %>
    <script type="text/javascript">
        function onDwMainClick(s, r, c) {
            if (r > 0) {
                var slip_no = s.GetItem(r, "creditdoc_no");
                parent.GetDlgSlipNoApd(slip_no);
                parent.RemoveIFrame();
            }
        }
        function OnDwCriButtonClick(s, r, c) {
            switch (c) {
                case "b_search":
                    jsSearchData();
                    break;
            }
        }
        function OnDwCriItemChanged(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="Label1" runat="server" Text="ค้นหา"></asp:Label>
        <dw:WebDataWindowControl ID="DwCri" runat="server" DataWindowObject="d_td_search_slip_debt_cri"
            LibraryList="~/DataWindow/trading/dlg_td_search_slip_apdebt.pbl" AutoRestoreContext="False"
            AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
            ClientFormatting="True" TabIndex="1" ClientEventButtonClicked="OnDwCriButtonClick"
            ClientEventItemChanged="OnDwCriItemChanged">
        </dw:WebDataWindowControl>
        <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_tradesrv_fin_apdebt_listing"
            LibraryList="~/DataWindow/trading/dlg_td_search_slip_apdebt.pbl" AutoRestoreContext="False"
            AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
            ClientEventClicked="onDwMainClick" ClientFormatting="True" RowsPerPage="15">
            <PageNavigationBarSettings NavigatorType="NumericWithQuickGo" Visible="True">
                <BarStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="12px" />
                <QuickGoNavigator GoToDescription="หน้า:" />
            </PageNavigationBarSettings>
        </dw:WebDataWindowControl>
        <asp:HiddenField ID="HdDebtNo" runat="server" Value="" />
        <asp:HiddenField ID="HdCredittype_code" runat="server" Value="" />
    </div>
    </form>
</body>
</html>
