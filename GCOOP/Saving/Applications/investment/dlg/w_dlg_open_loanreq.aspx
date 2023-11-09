<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_open_loanreq.aspx.cs"
    Inherits="Saving.Applications.investment.dlg.w_dlg_open_loanreq" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <%=jsPostSearch%>
    <%=jsPostMemberno%>
    <%=jsPostBlank%>
    <script type="text/javascript">
        function OnDwFilterButtonClicked(sender, row, bName) {
            jsPostSearch();
        }

        function OnDwMainClick(sender, row, col) {
            var loanrequest_docno = objDwMain.GetItem(row, "loanrequest_docno");
            var member_no = objDwMain.GetItem(row, "member_no");
            var loanrequest_status = objDwMain.GetItem(row, "loanrequest_status");
            var loantype_code = objDwMain.GetItem(row, "loantype_code");
            parent.GetLoanReq(loanrequest_docno, member_no, loanrequest_status, loantype_code);
            parent.RemoveIFrame();
        }

        function OnDwFilterItemChanged(sender, row, col, value) {
            sender.SetItem(row, col, value);
            sender.AcceptText();
            if (col == "member_no") {
                jsPostMemberno();
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div align="center">
        <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
        รายการใบคำขอกู้
        <br />
        <dw:WebDataWindowControl ID="DwFilter" runat="server" AutoRestoreContext="False"
            AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
            ClientValidation="False" DataWindowObject="d_lc_reqloansearch_criteria" LibraryList="~/DataWindow/investment/ln_req_loan.pbl"
            ClientEventButtonClicked="OnDwFilterButtonClicked" ClientEventItemChanged="OnDwFilterItemChanged">
        </dw:WebDataWindowControl>
        <dw:WebDataWindowControl ID="DwMain" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
            AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" ClientValidation="False"
            DataWindowObject="d_lc_reqloanlist" LibraryList="~/DataWindow/investment/ln_req_loan.pbl"
            ClientEventClicked="OnDwMainClick" RowsPerPage="5">
            <PageNavigationBarSettings Position="Top" Visible="True" NavigatorType="Numeric">
                <BarStyle HorizontalAlign="Center" />
                <NumericNavigator FirstLastVisible="True" />
            </PageNavigationBarSettings>
        </dw:WebDataWindowControl>
    </div>
    </form>
</body>
</html>
