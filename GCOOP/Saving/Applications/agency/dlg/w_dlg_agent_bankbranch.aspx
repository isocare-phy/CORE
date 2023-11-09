<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_agent_bankbranch.aspx.cs" Inherits="Saving.Applications.agency.dlg.w_dlg_agent_bankbranch" %>

<%@ Register assembly="WebDataWindow" namespace="Sybase.DataWindow.Web" tagprefix="dw" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <style type="text/css">
        .style1
        {
            height: 23px;
        }
    </style>
</head>
<script type ="text/javascript" >
    function OnDwMainClick(s,r,c)
    {
        if(c == "branch_id" || c == "branch_name")
        {
            var BranchID = objDw_main.GetItem(r,"branch_id");
            var BranchName = objDw_main.GetItem(r,"branch_name");
            window.opener.SetBranch(BranchID);
            window.close();
        }
        return 0;
    }
</script> 

<body>
    <form id="form1" runat="server">
    <div style="font-size: medium">
    
        <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
        <table style="width:100%;">
            <tr>
                <td class="style1">
                    <asp:Panel ID="Panel1" runat="server">
                        <dw:WebDataWindowControl ID="Dw_main" runat="server" 
                            DataWindowObject="d_agent_bankbranch" 
                            LibraryList="~/DataWindow/agency/agent.pbl" 
                            ClientEventClicked="OnDwMainClick" AutoRestoreContext="False" 
                            AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" 
                            ClientFormatting="True" ClientScriptable="True" RowsPerPage="20"><PageNavigationBarSettings 
                            NavigatorType="NumericWithQuickGo" Visible="True">
                        </PageNavigationBarSettings>
                        </dw:WebDataWindowControl>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
