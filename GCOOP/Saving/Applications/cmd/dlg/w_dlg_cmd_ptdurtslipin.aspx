<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_cmd_ptdurtslipin.aspx.cs" Inherits="Saving.Applications.cmd.dlg.w_dlg_cmd_ptdurtslipin" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">  
    <title> รายครุภัณฑ์รอเบิก </title> 
    <script type ="text/javascript" >

        function OnDwDetailClick(s, r, c) {
            var slipinNo = "";
            objDwDetail.AcceptText();
            slipinNo = objDwDetail.GetItem(r, "slipin_no");
            window.opener.GetValDlgDurtSlipin(slipinNo);
            window.close();
        }

    </script> 
</head>
<body>
    <form id="form1" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <table style="width: 100%; font-size: small;">
        <tr>
            <td>
                <dw:WebDataWindowControl ID="DwDetail" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True" 
                    AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True" ClientScriptable="True" 
                    DataWindowObject="d_detail_durtslipin" LibraryList="~/DataWindow/Cmd/cmd_dlgsearchdurt.pbl" 
                    ClientEventClicked="OnDwDetailClick">
                </dw:WebDataWindowControl>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>