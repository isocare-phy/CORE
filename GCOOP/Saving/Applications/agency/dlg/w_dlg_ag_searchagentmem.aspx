<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_ag_searchagentmem.aspx.cs"
    Inherits="Saving.Applications.agency.dlg.w_dlg_ag_searchagentmem" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>ค้นหาทะเบียนลูกหนี้ตัวแทน</title>
    <style type="text/css">

.objDw-main2BF{;background-color:transparent;OVERFLOW:hidden}
.objDw-main2C0{;background-color:#ffffff;OVERFLOW:hidden;COLOR:#000000;FONT:10pt "Tahoma", sans-serif;FONT-STYLE:normal;FONT-WEIGHT:normal;TEXT-DECORATION:none;TEXT-ALIGN:right;WORD-BREAK:break-all;BORDER-STYLE:none}
.objDw-main2C1{;COLOR:#000000;FONT:10pt "Tahoma", sans-serif;FONT-STYLE:normal;FONT-WEIGHT:normal;TEXT-DECORATION:none;TEXT-ALIGN:center}
.objDw-main2C2{;background-color:#c0dcc0;OVERFLOW:hidden;COLOR:#000000;FONT:10pt "Tahoma", sans-serif;FONT-STYLE:normal;FONT-WEIGHT:bold;TEXT-DECORATION:none;TEXT-ALIGN:center;WORD-BREAK:break-all;BORDER-STYLE:solid;BORDER-WIDTH:1px;BORDER-COLOR:#000000}
.objDw-main2C3{;background-color:#ffffff;COLOR:#000000;FONT:10pt "Tahoma", sans-serif;FONT-STYLE:normal;FONT-WEIGHT:normal;TEXT-DECORATION:none;TEXT-ALIGN:left;BORDER-STYLE:solid;BORDER-WIDTH:1px;BORDER-COLOR:#000000}
.objDw-main2C5{;background-color:#ffffff;COLOR:#000000;FONT:10pt "Tahoma", sans-serif;FONT-STYLE:normal;FONT-WEIGHT:normal;TEXT-DECORATION:none;TEXT-ALIGN:center;BORDER-STYLE:solid;BORDER-WIDTH:1px;BORDER-COLOR:#000000}
.objDw-main2CA{;background-color:#d3e7ff;OVERFLOW:hidden;COLOR:#000000;FONT:10pt "Tahoma", sans-serif;FONT-STYLE:normal;FONT-WEIGHT:normal;TEXT-DECORATION:none;TEXT-ALIGN:right;WORD-BREAK:break-all;BORDER-STYLE:solid;BORDER-WIDTH:1px;BORDER-COLOR:#000000}
    </style>
</head>
<%=postSearch%>
<%=postFormatMember%>

<script type="text/javascript">
    function OnDwMainItemChange(s,r,c,v)
    {
        if(c == "member_no")
        {
            objDw_main.SetItem(1, "member_no", Gcoop.StringFormat(v,"000000"));
            objDw_main.AcceptText();
            postFormatMember();
        }
        return 0;
    }
    
    function OnDwDetailClick(s, r, c) 
    {
        var member_no = objDw_detail.GetItem(r, "member_no");
        window.opener.SearchMember(member_no);
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

<body style="font-size: small; font-family: Tahoma">
    <form id="form2" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <br />
    <table style="width:100%;">
        <tr>
            <td>
                <b>เงื่อนไขการค้นหาทะเบียนสมาชิก</b></td>
        </tr>
        <tr>
            <td>
                <dw:WebDataWindowControl ID="Dw_main" runat="server" DataWindowObject="d_agentsrv_searchagentmem_main"
                    LibraryList="~/DataWindow/agency/agent.pbl" AutoRestoreContext="False" AutoRestoreDataCache="True"
                    AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True" ClientScriptable="True"
                    ClientEventButtonClicked="OnDwMainButtonClick" 
                    ClientEventClicked="OnDwMainClick" style="top: 0px; left: -36px" 
                    ClientEventItemChanged="OnDwMainItemChange">
                </dw:WebDataWindowControl>
            </td>
        </tr>
        <tr>
            <td>
                <b>รายละเอียด</b></td>
        </tr>
        <tr>
            <td>
    <dw:WebDataWindowControl ID="Dw_detail" runat="server" AutoRestoreContext="False"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientScriptable="True" DataWindowObject="d_agentsrv_searchagentmem_list" LibraryList="~/DataWindow/agency/agent.pbl"
        ClientEventItemFocusChanged="OnDwDetailItemFocusChanged" RowsPerPage="15" ClientEventClicked="OnDwDetailClick"
        Style="top: 0px; left: 0px">
        <PageNavigationBarSettings NavigatorType="NumericWithQuickGo" Visible="True">
        </PageNavigationBarSettings>
    </dw:WebDataWindowControl>
            </td>
        </tr>
        <tr>
            <td>
    <asp:HiddenField ID="HdCurrentrow" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
    <asp:HiddenField ID="HdRowClick" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
    </table>
    </form>
</body>
</html>
