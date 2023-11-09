<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_slip_search_repair.aspx.cs"
    Inherits="Saving.Applications.cmd.dlg.w_dlg_slip_search_repair" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>ค้นหาใบรายการต่างๆ</title>
    <script type="text/javascript">
        function OnDetailClick(sender, rowNumber, objectName) {
            var slip_id = objdw_detail.GetItem(rowNumber, "slip_id");
            var product_id = objdw_detail.GetItem(rowNumber, "product_id");
            window.opener.GetValueFromDlg(slip_id, product_id);
            window.close();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table style="width: 530px;">
            <tr>
                <td>
                    <dw:WebDataWindowControl ID="Dw_main" runat="server" ClientScriptable="True" DataWindowObject="d_stk_slip_search_repair"
                        LibraryList="~/DataWindow/Cmd/pointofsale_repair.pbl" Width="500px">
                    </dw:WebDataWindowControl>
                </td>
                <td>
                    <asp:Button ID="b_search" runat="server" Text="ค้นหา" Height="52px" Width="55px"
                        OnClick="b_search_Click" />
                </td>
            </tr>
        </table>
        <asp:Panel ID="Panel2" runat="server" Width="600px" Font-Bold="True" Font-Names="tahoma"
            Font-Size="12px" ForeColor="#660033" GroupingText="รายละเอียดการค้น">
            <dw:WebDataWindowControl ID="dw_detail" runat="server" DataWindowObject="d_stk_searchslipdea_repair"
                LibraryList="~/DataWindow/Cmd/pointofsale_repair.pbl" RowsPerPage="17" ClientEventClicked="OnDetailClick"
                ClientScriptable="True" ClientFormatting="True">
                <PageNavigationBarSettings Position="Top" Visible="True" NavigatorType="Numeric">
                    <BarStyle HorizontalAlign="Center" />
                    <NumericNavigator FirstLastVisible="True" />
                </PageNavigationBarSettings>
            </dw:WebDataWindowControl>
        </asp:Panel>
    </div>
    <asp:HiddenField ID="HSqlTemp" runat="server" Value="0" Visible="False" />
    </form>
</body>
</html>
