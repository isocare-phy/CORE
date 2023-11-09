<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_organize_search_loancont.aspx.cs"
    Inherits="Saving.Applications.investment.dlg.w_dlg_organize_search_loancont" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>ค้นหาสมาชิก</title>
    <script type="text/javascript">
        function selectRow(sender, rowNumber, objectName) {
            var loancont = objdw_detail.GetItem(rowNumber, "loancontract_no");
            window.opener.GetMemDetFromDlg(loancont);
            window.close();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    รายการค้นหา
    <table style="width: 530px;">
        <tr>
            <td>
                <dw:WebDataWindowControl ID="dw_data" runat="server" ClientScriptable="True" DataWindowObject="d_ol_search_member_criteria"
                    LibraryList="~/DataWindow/investment/int_editdetail.pbl">
                </dw:WebDataWindowControl>
            </td>
            <td>
                <asp:Button ID="cb_find" runat="server" Text="ค้นหา" Height="80px" Width="55px" OnClick="cb_find_Click" />
            </td>
        </tr>
    </table>
    รายละเอียด
    <asp:HiddenField ID="hidden_search" runat="server" />
    <dw:WebDataWindowControl ID="dw_detail" runat="server" ClientEventClicked="selectRow"
        DataWindowObject="d_ol_search_member_detailcont" LibraryList="~/DataWindow/investment/int_editdetail.pbl"
        RowsPerPage="17" ClientScriptable="True">
        <PageNavigationBarSettings Position="Top" Visible="True" NavigatorType="Numeric">
            <BarStyle HorizontalAlign="Center" />
            <NumericNavigator FirstLastVisible="True" />
        </PageNavigationBarSettings>
    </dw:WebDataWindowControl>
    </form>
</body>
</html>
