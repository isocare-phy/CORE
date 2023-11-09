<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_bg_budgettype_noaccid.aspx.cs"
    Inherits="Saving.Applications.budget.dlg.w_dlg_bg_budgettype_noaccid" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>

    <script type="text/javascript">
    function B_Close_Click(){
        var check = "false";
        if(confirm("ต้องการทำรายการตัดจ่าย ใช่หรือไม่?")){
            check = "true";
            parent.GetValueYesORNo(check);
            parent.RemoveIFrame();
        }
        else{
            check = "false";
            parent.GetValueYesORNo(check);
            parent.RemoveIFrame();
        }
    }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="Label1" runat="server" Text="ประเภทงบประมาณที่ไม่มีรหัสคู่บัญชี" Font-Bold="True"
            Font-Names="Tahoma" Font-Size="16px" Font-Underline="True" />
        <br />
        <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" Height="350px">
            <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_bg_budgettype_noaccid"
                LibraryList="~/DataWindow/budget/budget_dlg.pbl" ClientScriptable="True" AutoRestoreContext="False"
                AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientEventItemChanged="OnDwMainItemChanged"
                Height="350px">
            </dw:WebDataWindowControl>
        </asp:Panel>
    </div>
    <table>
        <tr>
            <td align="center">
                <asp:Panel ID="Panel2" runat="server" Width="350px">
                    <asp:Button ID="B_Search" runat="server" Text="ปิดหน้าจอ" Height="40" Font-Bold="True"
                        Font-Size="Small" OnClientClick="B_Close_Click()" />
                </asp:Panel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
