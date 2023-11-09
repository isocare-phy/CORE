<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_member_search_new.aspx.cs"
    Inherits="Saving.Applications.investment.dlg.w_dlg_member_search_new" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<%=jsPostSearch%>
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript">
        function OnDwDetailClick(s, r, c) {
            member_no = objDwDetail.GetItem(r, "member_no");
            memb_name = objDwDetail.GetItem(r, "mbucfprename_prename_desc") + objDwDetail.GetItem(r, "memb_name") + objDwDetail.GetItem(r, "mbucfprename_suffname_desc");
            parent.ReceiveMemberNo(member_no, memb_name);
            parent.RemoveIFrame();
        }
        function OnDwMainClick(s, r, c) {
            if (c == "b_search") {
                jsPostSearch();
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div align="center">
        <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
        รายการค้นหา
        <table style="width: 500px;">
            <tr>
                <td>
                    <dw:WebDataWindowControl ID="DwMain" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
                        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" ClientValidation="False"
                        DataWindowObject="dlg_member_search" LibraryList="~/DataWindow/investment/loan.pbl"
                        ClientEventClicked="OnDwMainClick">
                    </dw:WebDataWindowControl>
                </td>
            </tr>
        </table>
        <hr />
        รายละเอียด
        <table style="width: 500px;">
            <tr>
                <td>
                    <dw:WebDataWindowControl ID="DwDetail" runat="server" DataWindowObject="dlg_member_list"
                        LibraryList="~/DataWindow/investment/loan.pbl" ClientScriptable="True" AutoRestoreContext="False"
                        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientEventClicked="OnDwDetailClick" RowsPerPage="10">
                        <PageNavigationBarSettings Position="Top" Visible="True" NavigatorType="Numeric">
                            <BarStyle HorizontalAlign="Center" />
                            <NumericNavigator FirstLastVisible="True" />
                        </PageNavigationBarSettings>
                    </dw:WebDataWindowControl>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
