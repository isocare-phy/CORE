<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_slip_search.aspx.cs"
    Inherits="Saving.Applications.cmd.w_dlg_slip_search" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>ค้นหาใบรายการต่างๆ</title>

    <script type="text/javascript">

        function OnDetailClick(sender, rowNumber, objectName) {
            var slip_id = objd_stk_searchslipdea.GetItem(rowNumber, "slip_id");
            window.opener.GetValueFromDlg(slip_id);
            window.close();
        }

        //            // function การรับ Request จาก dlg ที่่ ส่งมาด้วย prm 1 ตัว
        //        function GetValueFromDlg(slip_id) {
        //                var str_temp = window.location.toString();
        //                var str_arr = str_temp.split("?", 2);
        //                window.location = str_arr[0] + "?slip_id=" + slip_id;


        //            }
    </script>

    <% Page_LoadComplete(); %>
</head>
<body>
    <form id="form1" runat="server">
    <asp:Panel ID="Panel1" runat="server" Width="530px" Font-Bold="True" Font-Names="tahoma"
        Font-Size="12px" ForeColor="#660033" GroupingText="ค้นหาข้อมูลใบรายการพัสดุ-ครุภัณฑ์">
    <div>
        <table style="width: 530px;">
            <tr>
                <td>
                    <dw:WebDataWindowControl ID="Dw_main" runat="server" ClientScriptable="True"
                        DataWindowObject="d_stk_slip_search" LibraryList="~/DataWindow/Cmd/impproduct.pbl"
                        Width="500px">
                    </dw:WebDataWindowControl>
                </td>
                <td>
                    <asp:Button ID="b_search" runat="server" Text="ค้นหา" Height="52px" Width="55px"
                        OnClick="b_search_Click" />
                </td>
            </tr>
        </table>
     </div>
    </asp:Panel>
        <asp:Panel ID="Panel2" runat="server" Width="530px" Font-Bold="True" Font-Names="tahoma"
            Font-Size="12px" ForeColor="#660033" GroupingText="รายละเอียดการค้น">
            <dw:WebDataWindowControl ID="dw_detail" runat="server" 
                DataWindowObject="d_stk_searchslipdea_pt" LibraryList="~/DataWindow/Cmd/impproduct.pbl"
                Width="530px" Style="top: 0px;
                left: 0px" RowsPerPage="10" HorizontalScrollBar="NoneAndClip" VerticalScrollBar="NoneAndClip"
                ClientEventClicked="OnDetailClick" ClientScriptable="True">
                <PageNavigationBarSettings NavigatorType="NumericWithQuickGo" Visible="True">
                </PageNavigationBarSettings>
            </dw:WebDataWindowControl>
        </asp:Panel>
    
    <asp:HiddenField ID="HSqlTemp" runat="server" Value="0" Visible="False" />
    </form>
</body>
</html>
