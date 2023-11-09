<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_sl_approve_member_resign.aspx.cs"
    Inherits="Saving.Applications.shrlon.w_sheet_sl_approve_member_resign" Title="Untitled Page" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <%=initJavaScript%>

    <script type="text/javascript">
        function Validate() {
           
            objdw_master.AcceptText();

            return confirm("ยืนยันการบันทึกข้อมูล");
            //return false;
        }
        function b_wait_onclick() {
            var allrow = objdw_master.RowCount();
            for (var i = 1; i <= allrow; i++) {
                objdw_master.SetItem(i, "resignreq_status", "8");

            }
        }
        function b_approve_onclick() {
            var allrow = objdw_master.RowCount();
            for (var i = 1; i <= allrow; i++) {
                objdw_master.SetItem(i, "resignreq_status", "1");

            }
        }
        function b_reject_onclick() {
            var allrow = objdw_master.RowCount();
            for (var i = 1; i <= allrow; i++) {
                objdw_master.SetItem(i, "resignreq_status", "0");

            }
        }
        function DwRowChanged(sender, rowNumber, columnName, newValue) {

            if (columnName == "resignreq_status") {
                objdw_master.SetItem(rowNumber, columnName, newValue);
             //   alert(objdw_master.GetItem(rowNumber, "resignreq_status"));
                objdw_master.AcceptText();



            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <table style="width: 100%;" border="1">
        <tr>
            <td style="height: 400px;" valign="top">
                <div style="margin-top: 0px; margin-left: 565px; position: absolute; z-index: 90;">
                    <input style="height: 26px; width: 26px" id="b_wait" type="button" value="รอ" onclick="b_wait_onclick()" />
                    <input style="margin-left: -4px; height: 26px; width: 48px;" id="b_approve" type="button"
                        value="อนุมัติ" onclick="b_approve_onclick()" />
                    <input style="margin-left: -3px; height: 26px;" id="b_reject" type="button" value="ไม่อนุมัติ"
                        onclick="b_reject_onclick()" />
                </div>
                <dw:WebDataWindowControl ID="dw_master" runat="server" DataWindowObject="d_sl_resign_apvlist"
                    LibraryList="~/DataWindow/shrlon/sl_approve_member_resign.pbl" ClientScriptable="True"
                    RowsPerPage="10" ClientEventItemChanged="DwRowChanged" AutoRestoreContext="false"
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True">
                    <PageNavigationBarSettings NavigatorType="Numeric" Visible="True">
                    </PageNavigationBarSettings>
                </dw:WebDataWindowControl>
            </td>
        </tr>
    </table>
</asp:Content>
