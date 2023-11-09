<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_divavg_memgroup.aspx.cs" Inherits="Saving.Applications.loantracking.dlg.w_dlg_divavg_memgroup" %>

<%@ Register assembly="WebDataWindow" namespace="Sybase.DataWindow.Web" tagprefix="dw" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <style type="text/css">
        .style1
        {
            font-size: small;
            font-weight: bold;
        }
    </style>
</head>
<%=postSearchMemGroup%>
<script type ="text/javascript" >

    function OnDwMainButtonClick(s,r,c)
    {
        if(c=="b_search")
        {
            var membgroup_code = objDw_main.GetItem(1,"membgroup_code");
            var membgroup_desc = objDw_main.GetItem(1,"membgroup_desc");
            if(membgroup_code == "" && membgroup_desc == null && membgroup_desc == "" && membgroup_desc == null)
            {
                alert("กรุณากรอกรายละเอียดสังกัด");
            }
            else 
            {
                postSearchMemGroup();
            }
        }
        return 0;
    }
    
    function OnDwDetailClick(s,r,c)
    {
        if(c == "membgroup_code" || c == "membgroup_desc")
        { 
            var membgroup_code = objDw_detail.GetItem(r,"membgroup_code");
            var B_name = Gcoop.GetEl("HdB_name").value;
            if(B_name == "start")
            {
                window.opener.SetStartMemGroup(membgroup_code);
                window.close();
            }
            else if(B_name == "end")
            {
                window.opener.SetEndMemGroup(membgroup_code);
                window.close();
            }
            
        }
    }
</script> 
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
        <br />
        <table style="width:100%;">
            <tr>
                <td class="style1">
                    รายละเอียดสังกัด</td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="Panel1" runat="server" BorderStyle="Ridge" Width="430px">
                        <dw:WebDataWindowControl ID="Dw_main" runat="server" AutoRestoreContext="False" 
                            AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" 
                            ClientFormatting="True" ClientScriptable="True" 
                            DataWindowObject="d_divavgsrv_search_ucfmembgroup_main" 
                            LibraryList="~/DataWindow/shrlon/div_avg.pbl" ClientEventButtonClicked="OnDwMainButtonClick">
                        </dw:WebDataWindowControl>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td class="style1">
                    แสดงรายละเอียดสังกัด</td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="Panel2" runat="server" BorderStyle="Ridge" Height="350px" 
                        Width="430px" ScrollBars="Vertical">
                        <dw:WebDataWindowControl ID="Dw_detail" runat="server" 
                            AutoRestoreContext="False" AutoRestoreDataCache="True" 
                            AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True" 
                            ClientScriptable="True" 
                            DataWindowObject="d_divavgsrv_search_ucfmembgroup_detail" 
                            LibraryList="~/DataWindow/shrlon/div_avg.pbl" ClientEventClicked="OnDwDetailClick">
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
                    <asp:HiddenField ID="HdB_name" runat="server" />
                    &nbsp;</td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
