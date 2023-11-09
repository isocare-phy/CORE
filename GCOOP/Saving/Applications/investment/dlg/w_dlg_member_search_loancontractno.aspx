<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_member_search_loancontractno.aspx.cs" Inherits="Saving.Applications.investment.dlg.w_dlg_member_search_loancontractno" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<%=postCriteria%>
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript">
        function OnDwMainClick(s, r, c, v) {
            member_no = objDwMain.GetItem(r, "member_no");
            loancontract_no = objDwMain.GetItem(r, "loancontract_no");
            parent.PostLoancontractNo(member_no, loancontract_no);
            parent.RemoveIFrame();
        }
        function OnDwCriClick(s, r, c, v) {
            if (c == "b_search") {
                postCriteria();
            }
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div align="center">
        <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
        <dw:WebDataWindowControl ID="DwCriteria" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
            AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" ClientValidation="False"
            DataWindowObject="dlg_member_search" LibraryList="~/DataWindow/investment/loan.pbl"
            ClientEventClicked="OnDwCriClick">
        </dw:WebDataWindowControl>

                         <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="dlg_loancontract_no_list"
                        LibraryList="~/DataWindow/investment/loan.pbl" ClientScriptable="True" AutoRestoreContext="False"
                        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientEventClicked="OnDwMainClick" RowsPerPage="10">
                        <PageNavigationBarSettings Position="Top" Visible="True" NavigatorType="Numeric">
                            <BarStyle HorizontalAlign="Center" />
                            <NumericNavigator FirstLastVisible="True" />
                        </PageNavigationBarSettings>
                    </dw:WebDataWindowControl>
        <%--<input type="button" value="ปิดหน้าจอ" onclick="parent.RemoveIFrame();" />--%>
    </div>
    </form>
</body>
</html>

