<%@ Page Title="" Language="C#" AutoEventWireup="true" 
CodeBehind="w_dlg_cmd_ptdurtmaster.aspx.cs" Inherits="Saving.Applications.cmd.dlg.w_dlg_cmd_ptdurtmaster" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">   
</head>
<%=postFindinvt%>
<%=jsPostRetrieveSubgrp%>
<script type ="text/javascript" >
    function OnDwFindClickButton(s, r, b) {
        if (b == "b_search") {
            postFindinvt();
        }
        return 0;
    }

    function OnDwFindChange(s, r, c, v) {
        s.SetItem(r, c, v);
        s.AcceptText();
        if (c == "durtgrp_code") {
            jsPostRetrieveSubgrp();
        }
    }

    function OnDwListClick(s, r, c) {
        if (c == "durt_id" || c == "durt_name" || c == "durt_regno") {
            var durt_id = objDw_detail.GetItem(r, "durt_id");
            var durt_name = objDw_detail.GetItem(r, "durt_name");
            window.opener.OnFindShow(durt_id, durt_name);
            window.close();
        }
        return 0;
    }
</script> 
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
        <table style="width: 100%; font-size: small;">
            <tr>
                <td> ค้นหาข้อมูล </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="Panel1" runat="server" Height="120px" BorderStyle="Ridge" Width="620px">
                    <dw:WebDataWindowControl ID="Dw_find" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True" 
                    AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True" ClientScriptable="True" 
                    DataWindowObject="d_cmd_search_ptdurtmaster" LibraryList="~/DataWindow/Cmd/cmd_ptdurtmaster.pbl" 
                    style="top: 0px; left: 0px" ClientEventButtonClicked="OnDwFindClickButton"
                    ClientEventItemChanged="OnDwFindChange"></dw:WebDataWindowControl>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td> รายละเอียด </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="Panel2" runat="server" Height="334px" BorderStyle="Ridge" 
                        ScrollBars="Vertical" Width="620px">
                        <asp:Panel ID="Panel3" runat="server" Height="101px">
                            <dw:WebDataWindowControl ID="Dw_detail" runat="server" 
                                AutoRestoreContext="False" AutoRestoreDataCache="True" 
                                AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True" 
                                ClientScriptable="True" DataWindowObject="d_se_detail_ptdurtmaster" 
                                LibraryList="~/DataWindow/Cmd/cmd_ptdurtmaster.pbl" 
                                ClientEventClicked="OnDwListClick">
                            </dw:WebDataWindowControl>
                        </asp:Panel>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:HiddenField ID="HSqlTemp" runat="server" />
                </td>
            </tr>
            </table>
    
    </div>
    </form>
</body>
</html>