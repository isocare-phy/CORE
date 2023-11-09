<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_sl_approve_member_new.aspx.cs" Inherits="Saving.Applications.shrlon.w_sheet_sl_approve_member_new"
    Culture="th-TH" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=rowSelected%>
    <%=genNewDoc %>
    <%=initJavaScript %>

    <script type="text/javascript">
        function selectRow(sender, rowNumber, objectName) {
            if (objectName == "appl_status") {
                var appl_status = objdw_master.GetItem(rowNumber, "appl_status");
                if (appl_status == "8") {
                    objdw_master.SetItem(rowNumber, "member_no", "");
                }
            }
        }

        function b_buildrunno_onclick() {
            objdw_master.AcceptText();
            var count = objdw_master.RowCount();
            var selectedRow = "";
            for (var i = 0; i < count; i++) {
                var temp = objdw_master.GetItem(i + 1, "appl_status");
                if (temp == 1) {
                    genNewDoc();
                    return;
                }
            }
        }



        function b_wait_onclick() {
            var allrow = objdw_master.RowCount();
            for (var i = 1; i <= allrow; i++) {
                objdw_master.SetItem(i, "appl_status", "8");
            }
        }
        function b_approve_onclick() {
            var allrow = objdw_master.RowCount();
            for (var i = 1; i <= allrow; i++) {
                objdw_master.SetItem(i, "appl_status", "1");
            }
        }
        function b_reject_onclick() {
            var allrow = objdw_master.RowCount();
            for (var i = 1; i <= allrow; i++) {
                objdw_master.SetItem(i, "appl_status", "0");
            }
        }


        function newMemnoDlg() {
            opendlg("415", "170", "w_dlg_sl_edit_member_no.aspx", "");
        }

        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล");
        }
        function DwRowChanged(sender, rowNumber, columnName, newValue) {
            if (columnName == "appl_status") {

                objdw_master.SetItem(rowNumber, columnName, newValue);
                objdw_master.AcceptText();
                
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <%-- <table style="width: 100%;" border="1">
        <tr>
            <td style="width: 50%;" align="center">
                <input id="b_setrunno" type="button" value="ทะเบียนล่าสุด" style="width: 120px; height: 35px;"
                    onclick="newMemnoDlg();" />
            </td>
            <td style="width: 50%;" align="center">
                <input id="b_buildrunno" type="button" value="สร้างเลขทะเบียน" style="width: 120px;
                    height: 35px;" onclick="return b_buildrunno_onclick()" />
            </td>
        </tr>
    </table>--%>
    <table style="width: 100%;" border="1">
        <tr>
            <td id="table_list" style="height: 400px;" valign="top">
                <div style="margin-top: 2px; margin-left: 545px; position: absolute; z-index: 90;">
                    <input style="height: 30px; background-color: White; color: Black;" id="b_wait" type="button"
                        value="รอ" onclick="b_wait_onclick()" />
                    <input style="margin-left: -5px; height: 30px; background-color: White; color: Black;"
                        id="b_approve" type="button" value="อนุมัติ" onclick="b_approve_onclick()" />
                    <input style="margin-left: -5px; height: 30px; background-color: White; color: Black;"
                        id="b_reject" type="button" value="ไม่อนุมัติ" onclick="b_reject_onclick()" />
                </div>
                <dw:WebDataWindowControl ID="dw_master" runat="server" DataWindowObject="d_sl_approve_member_new_list"
                    LibraryList="~/DataWindow/shrlon/sl_approve_member_new.pbl" ClientScriptable="True"
                    RowsPerPage="10" AutoRestoreContext="False" AutoRestoreDataCache="True"
                    AutoSaveDataCacheAfterRetrieve="True" ClientEventItemChanged="DwRowChanged">
                    <PageNavigationBarSettings NavigatorType="Numeric" Visible="True">
                    </PageNavigationBarSettings>
                </dw:WebDataWindowControl>
            </td>
        </tr>
    </table>
</asp:Content>
