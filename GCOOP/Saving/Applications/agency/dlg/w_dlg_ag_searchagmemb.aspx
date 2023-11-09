<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_ag_searchagmemb.aspx.cs" Inherits="Saving.Applications.agency.dlg.w_dlg_ag_searchagmemb" %>

<%@ Register assembly="WebDataWindow" namespace="Sybase.DataWindow.Web" tagprefix="dw" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <style type="text/css">
        .style1
        {
            font-size: small;
            font-family: Tahoma;
            font-weight: bold;
        }
        .style2
        {
            font-size: small;
            font-weight: bold;
        }
    </style>
</head>
<%=postSearch%>

<script type="text/javascript">
    function OnDwMainItemChange(s,r,c,v)
    {
        if(c == "member_no")
        {
            objDw_main.SetItem(1, "member_no", Gcoop.StringFormat(v,"00000000"));
            objDw_main.AcceptText();
        }
        return 0;
    }
    
    function OnDwDetailClick(s, r, c) 
    {
        var agentrequest_no = objDw_detail.GetItem(r, "agentrequest_no");
        window.opener.GetMemberFromAgent(agentrequest_no);
        window.close();
    }


    function OnDwMainButtonClick(s, r, b) {
        if (b == "b_search") {
            objDw_main.AcceptText();
            postSearch();
        }
        return 0;
    }

</script>

<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
        <table style="width:100%;">
            <tr>
                <td class="style2">
                    เงื่อนไขการค้นหาทะเบียนตัวแทน</td>
            </tr>
            <tr>
                <td>
                    <dw:WebDataWindowControl ID="Dw_main" runat="server" AutoRestoreContext="False" 
                        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" 
                        ClientScriptable="True" ClientValidation="False" 
                        DataWindowObject="d_agentsrv_searchagmemb_main" 
                        LibraryList="~/DataWindow/agency/egat_ag_searchagmemb.pbl" 
                        ClientEventButtonClicked="OnDwMainButtonClick" 
                        ClientEventItemChanged="OnDwMainItemChange" style="top: 0px; left: 0px">
                    </dw:WebDataWindowControl>
                </td>
            </tr>
            <tr>
                <td class="style1">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="style1">
                    รายการที่ค้นหา</td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="Panel1" runat="server" Height="300px" ScrollBars="Auto" 
                        Width="550px">
                        <dw:WebDataWindowControl ID="Dw_detail" runat="server" 
                        AutoRestoreContext="False" AutoRestoreDataCache="True" 
                        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" 
                        ClientValidation="False" DataWindowObject="d_agentsrv_searchagmemb_list" 
                        LibraryList="~/DataWindow/agency/egat_ag_searchagmemb.pbl" 
                        ClientEventClicked="OnDwDetailClick">
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
