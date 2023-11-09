<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_cmd_ptdurtregno.aspx.cs" Inherits="Saving.Applications.cmd.dlg.w_dlg_cmd_ptdurtregno" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
        <script type="text/javascript">
        </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
        <dw:WebDataWindowControl ID="DwDetail" runat="server" DataWindowObject="d_detail_requisition_durt"
            LibraryList="~/DataWindow/Cmd/dlg_search.pbl" Width="450px" ClientScriptable="True"
            UseCurrentCulture="True" ClientEventClicked="DwDetailOnClick" AutoRestoreContext="False"
            AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
            ClientEventItemChanged="DwDetailOnItemChange"> 
        </dw:WebDataWindowControl>
    </form>
</body>
</html>
