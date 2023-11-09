<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_cmd_search_local.aspx.cs"
    Inherits="Saving.Applications.cmd.w_dlg_cmd_search_local" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <%=postSearchList %>
    <title>ค้นหาหาสถานที่เก็บสินค้า</title>
    <script type="text/javascript">

        function DwMainButtonClick(sender, rowNumber, buttonName) {
            if (buttonName == "b_search") {
                objDw_main.AcceptText();
                postSearchList();
            }
        }

        function OnDetailClick(sender, rowNumber, objectName) {
           // var product_id = objdw_detail.GetItem(rowNumber, "product_id");
           var create_id = objdw_detail.GetItem(rowNumber, "create_id");
           // alert(product_id);
           window.opener.GetValueFromDlg(create_id);
            window.close();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <table style="width: 530px;">
        <tr>
            <td>
                <dw:WebDataWindowControl ID="Dw_main" runat="server" ClientScriptable="True" DataWindowObject="dw_cmd_search_localmas"
                    LibraryList="~/DataWindow/Cmd/cmd_local.pbl" Width="480px" AutoRestoreContext="False"
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientEventButtonClicked="DwMainButtonClick">
                </dw:WebDataWindowControl>
            </td>
            <td>
            </td>
        </tr>
    </table>
    <div>
        <asp:Panel ID="Panel2" runat="server" Width="530px" Font-Bold="True" Font-Names="tahoma"
            Font-Size="12px" ForeColor="#660033" GroupingText="รายละเอียดการค้น">
            <dw:WebDataWindowControl ID="dw_detail" runat="server" DataWindowObject="dw_cmd_search_localdea"
                LibraryList="~/DataWindow/Cmd/cmd_local.pbl" Width="530px" Height="350px" Style="top: 0px;
                left: 0px" ClientEventClicked="OnDetailClick" ClientScriptable="True" AutoRestoreContext="False"
                AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True">
            </dw:WebDataWindowControl>
        </asp:Panel>
    </div>
    <asp:HiddenField ID="HSqlTemp" runat="server" Value="0" Visible="False" />
    </form>
</body>
</html>