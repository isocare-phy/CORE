<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_list_noticemth.aspx.cs" Inherits="Saving.Applications.loan.dlg.w_dlg_list_noticemth" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function OnDwMainClick(sender, row, col) {
            var notice_docno = objDwMain.GetItem(row, "notice_docno");
            parent.ReciveValue(notice_docno);
            parent.RemoveIFrame();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div align="center">
        <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
        รายการสัญญา
        <br />
        <dw:WebDataWindowControl ID="DwMain" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
            AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" ClientValidation="False"
            DataWindowObject="d_lcsrv_list_noticemth" LibraryList="~/DataWindow/investment/loan_slippayin.pbl"
            ClientEventClicked="OnDwMainClick" RowsPerPage="10">
            <PageNavigationBarSettings Position="Top" Visible="True" NavigatorType="Numeric">
                <BarStyle HorizontalAlign="Center" />
                <NumericNavigator FirstLastVisible="True" />
            </PageNavigationBarSettings>
        </dw:WebDataWindowControl>
    </div>
    </form>
</body>
</html>
