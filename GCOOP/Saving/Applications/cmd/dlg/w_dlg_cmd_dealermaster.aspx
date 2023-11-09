<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_cmd_dealermaster.aspx.cs" Inherits="Saving.Applications.cmd.dlg.w_dlg_cmd_dealermaster" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ค้นหาข้อมูลคู่ค้า</title>
    <%=jsPostSearch%>
    <script type="text/javascript">
        function DwMainOnClick(s, r, c) {
            if (c == "b_search") {
                jsPostSearch();
            }
        }

        function DwDetailOnClick(s, r, c) {
            var dealer_no = objDwDetail.GetItem(r, "dealer_no");
            window.opener.GetDlgDealer(dealer_no);
            window.close();
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_main_search_dealermaster"
        LibraryList="~/DataWindow/Cmd/dlg_search.pbl" Width="450px" ClientScriptable="True"
        UseCurrentCulture="True" ClientEventButtonClicked="DwMainOnClick" AutoRestoreContext="False"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientEventItemChanged="DwMainOnItemChange"> 
    </dw:WebDataWindowControl>
    <dw:WebDataWindowControl ID="DwDetail" runat="server" DataWindowObject="d_list_search_dealermaster"
        LibraryList="~/DataWindow/Cmd/dlg_search.pbl" Width="450px" ClientScriptable="True"
        UseCurrentCulture="True" ClientEventClicked="DwDetailOnClick" AutoRestoreContext="False"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientEventItemChanged="DwDetailOnItemChange"> 
    </dw:WebDataWindowControl>
    </form>
</body>
</html>
