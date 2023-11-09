<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_st_product_searchimp.aspx.cs"
    Inherits="Saving.Applications.cmd.dlg.w_dlg_st_product_searchimp" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>ค้นหาข้อมูลพัสดุครุภัณฑ์</title>
    <script type="text/javascript">
        // function selectRow จากdatawindow  "objd_sl_product_search_list"
        // ส่งค่าตาม Request ของ main window ด้วยฟังค์ชั่น--->GetProductFromDlg(product_id, product_name);
        // จากนั้นก็จะปิดตัวเองลง
        function OnDetailClick(sender, rowNumber, objectName) {

            var create_id = objdw_detail.GetItem(rowNumber, "create_id");
            var product_id = objdw_detail.GetItem(rowNumber, "product_id");
            var unit_name = objdw_detail.GetItem(rowNumber, "unit_name");
            var product_name = objdw_detail.GetItem(rowNumber, "product_name");
            //            var isConfirm = confirm("ยืนยันต้องการเลือกข้อมูล " + product_id + ":" + product_name);
            //            if (isConfirm) {
            window.opener.GetProductFromDlg(create_id, product_id, product_name, unit_name);
            window.close();
            //            }
        }
       
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <%-- <input id="b_save" style="width: 250px; height: 35px; color: White; font-size: medium;
        font-weight: bold; background-color: Green;" type="button" OnClick="InsertCMD()"
        value="สร้างเลขพัสดุ/ครุภัณฑ์ใหม่" />--%>
    <asp:Panel ID="Panel1" runat="server" Width="540px" Font-Bold="True" Font-Names="tahoma"
        Font-Size="12px" ForeColor="#660033" GroupingText="ค้นหาข้อมูลครุภัณฑ์">
        <table style="width: 540px;">
            <tr>
                <td>
                    <dw:WebDataWindowControl ID="Dw_main" runat="server" ClientScriptable="True" DataWindowObject="d_sl_product_search_criteria_sale"
                        LibraryList="~/DataWindow/Cmd/pointofsale.pbl" Width="470px">
                    </dw:WebDataWindowControl>
                </td>
                <td>
                    <asp:Button ID="b_search" runat="server" Text="ค้นหา" Height="52px" Width="50px"
                        OnClick="b_search_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <div>
        <asp:Panel ID="Panel2" runat="server" Width="540px" Font-Bold="True" Font-Names="tahoma"
            Font-Size="12px" ForeColor="#660033" GroupingText="รายละเอียดการค้น">
            <dw:WebDataWindowControl ID="dw_detail" runat="server" DataWindowObject="d_sl_product_search_list_sale"
                LibraryList="~/DataWindow/Cmd/pointofsale.pbl" Width="530px" Height="400px" Style="top: 0px;
                left: 0px" ClientEventClicked="OnDetailClick" ClientScriptable="True">
            </dw:WebDataWindowControl>
        </asp:Panel>
    </div>
    <asp:HiddenField ID="HSqlTemp" runat="server" Value="0" />
    </form>
</body>
</html>