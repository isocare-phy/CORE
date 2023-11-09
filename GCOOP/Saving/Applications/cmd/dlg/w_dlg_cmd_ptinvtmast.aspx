<%@ Page Title="" Language="C#"  AutoEventWireup="true" 
CodeBehind="w_dlg_cmd_ptinvtmast.aspx.cs" Inherits="Saving.Applications.cmd.dlg.w_dlg_cmd_ptinvtmast" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">   
    <%=postFindinvt%>
    <%=jsPostInvtsubgrp%>
<script type ="text/javascript" >
    function DwFindOnChange(s, r, c, v) {
        s.SetItem(r, c, v);
        s.AcceptText();
        if (c == "invtgrp_code") {
            jsPostInvtsubgrp();
        }
    }

    function OnDwFindClickButton(s, r, b) {
        if (b == "b_search") {
            postFindinvt();
        }
        return 0;
    }
    function OnDwListClick(s, r, c) {
        if (c == "invt_id" || c == "invt_name" || c == "invtgrp_desc") {
            var invt_id = objDw_detail.GetItem(r, "invt_id");
            var invt_name = objDw_detail.GetItem(r, "invt_name");
            var invtgrp_desc = objDw_detail.GetItem(r, "invtgrp_desc");
            window.opener.OnFindShow(invt_id, invt_name, invtgrp_desc);
            window.close();
        }
        return 0;
    }
</script> 
</head>
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
                    <asp:Panel ID="Panel1" runat="server" Height="100px" BorderStyle="Ridge" Width="550px">
                        <dw:WebDataWindowControl ID="Dw_find" runat="server" 
                            AutoRestoreContext="False" AutoRestoreDataCache="True" 
                            AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True" 
                            ClientScriptable="True" DataWindowObject="d_cmd_search_ptinvtmast" 
                            LibraryList="~/DataWindow/Cmd/cmd_ptinvtmast.pbl" 
                            style="top: 0px; left: 0px" ClientEventButtonClicked="OnDwFindClickButton" ClientEventItemChanged="DwFindOnChange">
                        </dw:WebDataWindowControl>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td> รายละเอียด </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="Panel2" runat="server" Height="334px" BorderStyle="Ridge" 
                        ScrollBars="Vertical" Width="550px">
                        <asp:Panel ID="Panel3" runat="server" Height="101px">
                            <dw:WebDataWindowControl ID="Dw_detail" runat="server" 
                                AutoRestoreContext="False" AutoRestoreDataCache="True" 
                                AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True" 
                                ClientScriptable="True" DataWindowObject="d_se_detail_ptinvtmast" 
                                LibraryList="~/DataWindow/Cmd/cmd_ptinvtmast.pbl" 
                                ClientEventClicked="OnDwListClick">
                            </dw:WebDataWindowControl>
                        </asp:Panel>
                    </asp:Panel>
                </td>
            </tr>
                    
            </table>
            <asp:HiddenField ID="HSqlTemp" runat="server" />
    </div>
    </form>
</body>
</html>