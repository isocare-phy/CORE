<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_camels_var.aspx.cs"
    Inherits="Saving.Applications.mis.dlg.w_dlg_camels_var" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
    <script type="text/javascript">       
        function RowClick(sender, rowNumber, objectName) {
            var str_temp = window.location.toString();
            var str_arr = str_temp.split("?", 2);
            var moneysheet_code = objdw_sheethead.GetItem(rowNumber, "moneysheet_code");
            window.location = str_arr[0] + "?cmd=" + moneysheet_code;
        }
        function RowClickDetail(sender, rowNumber, objectName) {
            var moneysheet_code = objdw_sheetdet.GetItem(rowNumber, "moneysheet_code").replace(/\s*$/, "");
            var moneysheet_seq = objdw_sheetdet.GetItem(rowNumber, "moneysheet_seq");
            var description = objdw_sheetdet.GetItem(rowNumber, "description");

            window.opener.GetMoneysheet(moneysheet_code, moneysheet_seq, rowNumber, description);
            window.close();           
        }
    </script>

    <title>กำหนดตัวแปร</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="100%">
            <tr>
                <td width="30%" valign="top">
                    <dw:WebDataWindowControl ID="dw_sheethead" runat="server" DataWindowObject="d_mis_ratio_accsheetmoneyhead"
                        LibraryList="~/DataWindow/mis.pbl;~/DataWindow/mis/camels_set.pbl" 
                        ClientEventClicked="RowClick" ClientScriptable="True">
                    </dw:WebDataWindowControl>
                </td>
                <td width="70%" valign="top">
                    <dw:WebDataWindowControl ID="dw_sheetdet" runat="server" DataWindowObject="d_mis_ratio_accsheetmoneydet"
                        LibraryList="~/DataWindow/mis.pbl;~/DataWindow/mis/camels_set.pbl" ClientEventClicked="RowClickDetail"
                        Height="570px" AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" 
                        ClientFormatting="True" ClientScriptable="True" Width="100%" VerticalScrollBar="Auto" BorderWidth="0">
                    </dw:WebDataWindowControl>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
