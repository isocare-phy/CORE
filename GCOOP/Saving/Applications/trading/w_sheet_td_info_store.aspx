<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" 
CodeBehind="w_sheet_td_info_store.aspx.cs" Inherits="Saving.Applications.trading.w_sheet_td_info_store" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%=initJavaScript%>
<%=postInsertRow%>
<%=postDeleteRow%>

    <script type="text/javascript">
        function OnClickInsertRow() {
            postInsertRow();
        }

        function Delete_ucf_store(sender, row, bName) { //function ลบแถวข้อมูล
            if (bName == "b_del") {
                var store_id = objDwMain.GetItem(row, "store_id");

                if (store_id == "" || store_id == null) {
                    Gcoop.GetEl("HdRow").value = row + "";
                    postDeleteRow();
                }
                else {
                    var isConfirm = confirm("ต้องการลบข้อมูลรหัสประเภทการชำระเงิน " + store_id + " ใช่หรือไม่ ?");
                    if (isConfirm) {
                        Gcoop.GetEl("HdRow").value = row + "";
                        postDeleteRow();
                    }
                }
            }
            return 0;
        }

        function Validate() { //function เช็คค่าข้อมูลก่อน save
            var isconfirm = confirm("ยืนยันการบันทึกข้อมูล ?");

            if (!isconfirm) {
                return false;
            }
            var RowDetail = objDwMain.RowCount();

            for (var i = 1; i <= RowDetail; i++) {
                var store_id = objDwMain.GetItem(i, "store_id");
                var store_desc = objDwMain.GetItem(i, "store_desc");

                if (store_id == "" || store_id == null || store_desc == "" || store_desc == null) {
                    alert("กรุณาระบุข้อมูลให้ครบถ้วน");
                    return false;
                }
            }
            return true;
        }

    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
<asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <span onclick="OnClickInsertRow()" style="cursor: pointer;">
        <asp:Label ID="LbInsert1" runat="server" Text="เพิ่มแถว" Font-Bold="False" Font-Names="Tahoma"
            Font-Size="14px" Font-Underline="True" ForeColor="Blue" /></span>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_tradesrv_info_store_master"
        LibraryList="~/DataWindow/trading/sheet_td_info_store.pbl" ClientScriptable="True"
        ClientEventButtonClicked="Delete_ucf_store" AutoRestoreContext="False" 
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" Width="760px" Height="510px">
    </dw:WebDataWindowControl>
    <asp:HiddenField ID="HdRow" runat="server" />
</asp:Content>