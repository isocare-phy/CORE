<%@ Page Title="" Language="C#"  AutoEventWireup="true" 
CodeBehind="w_dlg_cmd_ptmetlmastlot_adj.aspx.cs" Inherits="Saving.Applications.cmd.dlg.w_dlg_cmd_ptmetlmastlot_adj" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server"> 
</head>

<script type ="text/javascript" >

    function OnDwListClick(s, r, c) {
        if (c == "lot_id" || c == "qty_bal") {
            var lot_id = objDw_detail.GetItem(r, "lot_id");
            var qty_bal = objDw_detail.GetItem(r, "qty_bal");
            window.opener.DlgFindShow(lot_id,qty_bal);
            window.close();
        }
        return 0;
    }

</script> 
<body>
    <form id="form1" runat="server">
    <div>    
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <asp:Panel ID="Panel3" runat="server">
        <dw:WebDataWindowControl ID="Dw_detail" runat="server" 
            AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True" 
            ClientScriptable="True" DataWindowObject="d_cmd_search_mastlot_adj" LibraryList="~/DataWindow/Cmd/cmd_ptinvtmast.pbl" 
            ClientEventClicked="OnDwListClick" PageNavigationBarSettings-NavigatorType="Numeric" RowsPerPage="10">
        </dw:WebDataWindowControl>
    </asp:Panel>
    <asp:HiddenField ID="HinvtId" runat="server" />   
    </div>
    </form>
</body>
</html>
