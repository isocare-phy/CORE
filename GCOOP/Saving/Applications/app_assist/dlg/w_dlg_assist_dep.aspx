<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_assist_dep.aspx.cs" Inherits="Saving.Applications.app_assist.dlg.w_dlg_assist_dep" %>

<%@ Register assembly="WebDataWindow" namespace="Sybase.DataWindow.Web" tagprefix="dw" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Untitled Page</title>
</head>
<script type ="text/javascript" >
    function OnDwMainClick(s, r, c) {
        var bank_id = objDw_main.GetItem(r, "bank_id");
        var bank_name = objDw_main.GetItem(r, "sv_ac_name");
        var bank_balance = objDw_main.GetItem(r, "sv_ac_baln");
        var bank_aval = objDw_main.GetItem(r, "sv_ac_aval");
        var dept_desc = objDw_main.GetItem(r, "save_desc");
//        var cp_depttype = objDw_main.GetItem(r, "cp_depttype");
//        alert(dept_desc);
        var row = Gcoop.GetEl("HdRow").value;
        window.opener.SetExpense_Accid(bank_id, bank_name, bank_balance, bank_aval, dept_desc, row);
        window.close();
    }
</script> 
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
        <br />
        <table style="width:100%;">
            <tr>
                <td colspan="3">
                    <asp:Panel ID="Panel1" runat="server" BorderStyle="Ridge" Height="250px" 
                        ScrollBars="Auto" Width="670px">
                        <dw:WebDataWindowControl ID="Dw_main" runat="server" AutoRestoreContext="False" 
                            AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" 
                            ClientFormatting="True" ClientScriptable="True" 
                            DataWindowObject="d_view_bank_memberno" 
                            LibraryList="~/DataWindow/app_assist/kt_pension.pbl" 
                            ClientEventClicked="OnDwMainClick"></dw:WebDataWindowControl>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
        </table>
    
    </div>
    <asp:HiddenField ID="HdRow" runat="server" />
    </form>
</body>
</html>
