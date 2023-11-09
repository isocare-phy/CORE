<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_td_search_value_cust.aspx.cs"
    Inherits="Saving.Applications.trading.dlg.w_dlg_td_search_value_cust" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <%=jsSearch%>
    <script type="text/javascript">
        function OnDwCriButtonClicked(s, r, c) {
            if (c == "b_search") {
                jsSearch();
            }
        }

        function DwDetailClick(s, r, c) {
            if (r > 0) {
                var slip_no = objDwDetail.GetItem(r, "slip_no");
                parent.GetDlgBSlipNo(slip_no);
                parent.RemoveIFrame();
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="Label1" runat="server" Text="ค้นหา"></asp:Label>
        <dw:WebDataWindowControl ID="DwCri" runat="server" DataWindowObject="d_td_search_value_cri"
            LibraryList="~/DataWindow/trading/dlg_td_search_value.pbl" AutoRestoreContext="False"
            AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
            ClientEventButtonClicked="OnDwCriButtonClicked" ClientFormatting="True" TabIndex="1">
        </dw:WebDataWindowControl>
        <dw:WebDataWindowControl ID="DwDetail" runat="server" DataWindowObject="d_td_search_value_cust"
            LibraryList="~/DataWindow/trading/dlg_td_search_value.pbl" AutoRestoreContext="False"
            AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
            ClientFormatting="True" ClientEventClicked="DwDetailClick" RowsPerPage="15">
            <PageNavigationBarSettings NavigatorType="NumericWithQuickGo" Visible="True">
                <BarStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="12px" />
                <QuickGoNavigator GoToDescription="หน้า:" />
            </PageNavigationBarSettings>
        </dw:WebDataWindowControl>
    </div>
    <asp:HiddenField ID="HdSlipType" runat="server" />
    <asp:HiddenField ID="HdDebtNo" runat="server" />
    </form>
</body>
</html>
