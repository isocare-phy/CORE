<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_bg_budgettype.aspx.cs"
    Inherits="Saving.Applications.budget.dlg.w_dlg_bg_budgettype" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ค้นหาประเภทงบประมาณ</title>

    <script type="text/javascript">
        function selectRow(sender, rowNumber, objectName) {
            try{
                var bgTypeCode = objDwList.GetItem(rowNumber, "budgettype_code");
                var bgGrpCode = objDwList.GetItem(rowNumber,"budgetgroup_code");
                var bgTypeDesc = objDwList.GetItem(rowNumber, "budgettype_desc");
                window.opener.GetBgTypeCodeFromDlg(bgGrpCode, bgTypeCode, bgTypeDesc);
                window.close(); 
            }
            catch(err){window.close();} 
            return;        
        }
        
        function OnDwMainItemChanged(sender, rowNumber, columnName, newValue){
            objDwMain.SetItem(rowNumber, columnName, newValue);
            objDwMain.AcceptText();
        }
        
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Panel ID="head" runat="server">
            <table>
                <tr>
                    <td>
                        <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_bg_budgettype_search_main"
                            LibraryList="~/DataWindow/budget/budget_dlg.pbl"
                            ClientScriptable="True" AutoRestoreContext="False" AutoRestoreDataCache="True"
                            AutoSaveDataCacheAfterRetrieve="True" ClientEventItemChanged="OnDwMainItemChanged">
                        </dw:WebDataWindowControl>
                    </td>
                    <td>
                        <asp:Button ID="B_Search" runat="server" Text="ค้นหา..." Height="50" Font-Bold="True"
                            Font-Size="Small" OnClick="B_Search_Click" />
                    </td>
                </tr>
            </table>
            <hr />
            <br />
            <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
        </asp:Panel>
            <dw:WebDataWindowControl ID="DwList" runat="server" DataWindowObject="d_bg_budgettype_search_list"
                LibraryList="~/DataWindow/budget/budget_dlg.pbl" ClientEventClicked="selectRow"
                ClientScriptable="True" RowsPerPage="15" HorizontalScrollBar="NoneAndClip"
                VerticalScrollBar="NoneAndClip" AutoRestoreContext="False" AutoRestoreDataCache="True"
                AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True">
                <PageNavigationBarSettings NavigatorType="NumericWithQuickGo" Visible="True">
                </PageNavigationBarSettings>
            </dw:WebDataWindowControl>
    </div>
    </form>
</body>
</html>
