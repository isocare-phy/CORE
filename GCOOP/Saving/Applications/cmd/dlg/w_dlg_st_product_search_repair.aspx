<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_st_product_search_repair.aspx.cs"
    Inherits="Saving.Applications.cmd.dlg.w_dlg_st_product_search_repair" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>ค้นหาข้อมูลพัสดุครุภัณฑ์</title>
    <%=jsPostSearch%>
    <script type="text/javascript">
        function OnDetailClick(s, r, c) {
            var reqrepair_no = objDwDetail.GetItem(r, "reqrepair_no");
            window.opener.GetReqrepairNoFromDlg(reqrepair_no);
            window.close();
        }

        function OnDwMainClick(s, r, c){
            if(c == "b_search") {
                jsPostSearch();
            }
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <asp:Panel ID="Panel1" runat="server" Width="780px" Font-Bold="True" Font-Names="tahoma"
        Font-Size="12px" ForeColor="#660033" GroupingText="ค้นหาข้อมูล รายการแจ้งซ่อมครุภัณฑ์">
        <table style="width: 540px;">
            <tr>
                <td>
                    <dw:WebDataWindowControl ID="DwMain" runat="server" ClientScriptable="True" 
                        DataWindowObject="d_main_search_ptdurtreqrepair"
                        AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
                        LibraryList="~/DataWindow/Cmd/dlg_search.pbl" Width="470px" 
                        ClientEventButtonClicked="OnDwMainClick">
                    </dw:WebDataWindowControl>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <div>
        <asp:Panel ID="Panel2" runat="server" Width="780px" Font-Bold="True" Font-Names="tahoma"
            Font-Size="12px" ForeColor="#660033" GroupingText="รายละเอียดการค้น">
            <dw:WebDataWindowControl ID="DwDetail" runat="server" DataWindowObject="d_list_search_ptdurtreqrepair"
                LibraryList="~/DataWindow/Cmd/dlg_search.pbl" Width="540px" Style="top: 0px;
                left: 0px" RowsPerPage="15" HorizontalScrollBar="NoneAndClip" VerticalScrollBar="NoneAndClip"
                ClientEventClicked="OnDetailClick" ClientScriptable="True">
                <PageNavigationBarSettings NavigatorType="NumericWithQuickGo" Visible="True">
                </PageNavigationBarSettings>
            </dw:WebDataWindowControl>
        </asp:Panel>
    </div>
    <asp:HiddenField ID="HSqlTemp" runat="server" Value="0" Visible="False" />
    </form>
</body>
</html>
