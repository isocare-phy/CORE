<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_member_search_docno.aspx.cs"
    Inherits="Saving.Applications.investment.dlg.w_dlg_member_search_docno" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<%=postCriteria%>
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript">
        function OnDwMainClick(s, r, c, v) {
            member_no = objDwMain.GetItem(r, "member_no");
            //            memb_name = objDwMain.GetItem(r, "mbucfprename_prename_desc") + objDwMain.GetItem(r, "memb_name") + objDwMain.GetItem(r, "mbucfprename_suffname_desc");
            loanrequest_docno = objDwMain.GetItem(r, "loanrequest_docno");
            parent.PostMemberNo(member_no, loanrequest_docno);
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
        <dw:WebDataWindowControl ID="DwCriteria" runat="server" AutoRestoreContext="False"
            AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
            ClientValidation="False" DataWindowObject="dlg_member_search" LibraryList="~/DataWindow/investment/loan.pbl"
            ClientEventClicked="OnDwCriClick">
        </dw:WebDataWindowControl>
        <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="dlg_member_list_docno"
            LibraryList="~/DataWindow/investment/loan.pbl" ClientScriptable="True" AutoRestoreContext="False"
            AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientEventClicked="OnDwMainClick"
            RowsPerPage="10">
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
