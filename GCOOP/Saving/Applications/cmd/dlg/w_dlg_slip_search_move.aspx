<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_slip_search_move.aspx.cs"
    Inherits="Saving.Applications.cmd.dlg.w_dlg_slip_search_move" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>ค้นหาใบรายการต่างๆ</title>
    <script type="text/javascript">

        function OnDetailClick(sender, rowNumber, objectName) {
            var slip_id = objdw_detail.GetItem(rowNumber, "slip_id");
            window.opener.GetValueFromDlg(slip_id);
            //alert(slip_id);
            window.close();
        }

        function OnDwMainChange(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            switch (c) {
                case "sliptype_id":
                    //alert(v);
                    //alert(objDw_main.GetItem(1, "sliptype_id"))
                    Gcoop.GetEl("Hdsliptype_id").value = v;
                    //alert(Gcoop.GetEl("Hdsliptype_id").value);
                    break;
                case "issue_state":
                    //alert(v);
                    Gcoop.GetEl("Hdref_no").value = v;
                    break;
            }
        }
      
    </script>
</head>
<body>
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <form id="form1" runat="server">
    <table style="width: 530px;">
        <tr>
            <td>
                <dw:WebDataWindowControl ID="Dw_main" runat="server" ClientScriptable="True" DataWindowObject="d_stk_slip_search_move"
                    LibraryList="~/DataWindow/Cmd/pointofsale_move.pbl" Width="500px" ClientEventItemChanged="OnDwMainChange">
                </dw:WebDataWindowControl>
            </td>
            <td>
                <asp:Button ID="b_search" runat="server" Text="ค้นหา" Height="50px" Width="55px"
                    OnClick="b_search_Click" />
            </td>
        </tr>
    </table>
    <div>
        <asp:Panel ID="Panel2" runat="server" Width="600px" Font-Bold="True" Font-Names="tahoma"
            Font-Size="12px" ForeColor="#660033" GroupingText="รายละเอียดการค้น">
            <dw:WebDataWindowControl ID="dw_detail" runat="server" DataWindowObject="d_stk_searchslipdea_move"
                LibraryList="~/DataWindow/Cmd/pointofsale_move.pbl" Width="530px" Style="top: 0px;
                left: 0px" RowsPerPage="10" HorizontalScrollBar="NoneAndClip" VerticalScrollBar="NoneAndClip"
                ClientEventClicked="OnDetailClick" ClientScriptable="True">
                <PageNavigationBarSettings NavigatorType="NumericWithQuickGo" Visible="True">
                </PageNavigationBarSettings>
            </dw:WebDataWindowControl>
        </asp:Panel>
    </div>
    <asp:HiddenField ID="HSqlTemp" runat="server" Value="0" />
    <asp:HiddenField ID="Hdsliptype_id" runat="server" Value="0" />
    <asp:HiddenField ID="Hdref_no" runat="server" Value="0" />
    </form>
</body>
</html>
