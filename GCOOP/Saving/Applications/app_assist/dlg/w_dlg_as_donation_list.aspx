<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_as_donation_list.aspx.cs"
    Inherits="Saving.Applications.app_assist.dlg.w_dlg_as_donation_list" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">

        function DwMainClick(sender, rowNumber, objectName) {
            var assist_docno = objDwMain.GetItem(rowNumber, "assist_docno");
            var capital_year = objDwMain.GetItem(rowNumber, "capital_year");
            var member_no = objDwMain.GetItem(rowNumber, "member_no");
            window.opener.GetValueFromDlgList(assist_docno, capital_year, member_no);
            window.close();
        }
        
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" Height="400px">
            <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_as_reqdonate_list"
                LibraryList="~/DataWindow/app_assist/as_capital.pbl" ClientScriptable="True"
                AutoRestoreContext="false" AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True"
                ClientFormatting="True" ClientEventClicked="DwMainClick">
            </dw:WebDataWindowControl>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
