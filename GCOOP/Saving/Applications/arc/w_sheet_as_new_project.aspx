<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_as_new_project.aspx.cs" Inherits="Saving.Applications.arc.w_sheet_as_new_project" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript %>
    <%=postProjectId %>
    <%=postInsertRow %>
    <%=postDeleteRow %>
    <%=postFilterProjectId %>
    <script type="text/javascript">

        function Validate() {
            objDwMain.AcceptText();
            objDwDetail.AcceptText();
            var project_id = objDwMain.GetItem(1, "project_id");
            var project_name = objDwMain.GetItem(1, "project_name");
            //            if (project_id != null && project_name != null) {
            return confirm("ยืนยันการบันทึกข้อมูล");
            //            }
            //            else {
            //                alert("กรุณากรอกข้อมูลให้ครบ");
            //            }
        }

        function DwListItemChange(sender, rowNumber, columnName, newValue) {
            objDwList.SetItem(rowNumber, columnName, newValue);
            objDwList.AcceptText();
            if (columnName == "project_id") {
                Gcoop.GetEl("HdListRow").value = rowNumber;
                var project_id = objDwList.GetItem(rowNumber, "project_id");
                Gcoop.GetEl("HfProjectId").value = project_id;
                postProjectId();
            }
            else if (columnName == "project_year") {
                postFilterProjectId();
            }
        }

        function DwDetailClick(sender, rowNumber, objectName) {
            for (var i = 1; i <= sender.RowCount(); i++) {
                sender.SelectRow(i, false);
            }
            sender.SelectRow(rowNumber, true);
            sender.SetRow(rowNumber);
            Gcoop.GetEl("HdMainRow").value = rowNumber;
        }

        function DwMainItemChange(sender, rowNumber, columnName, newValue) {
            objDwMain.SetItem(rowNumber, columnName, newValue);
            objDwMain.AcceptText();
        }

        function DwDetailItemChange(sender, rowNumber, columnName, newValue) {
            objDwDetail.SetItem(rowNumber, columnName, newValue);
            objDwDetail.AcceptText();
        }

        function DwMainClick(sender, rowNumber, objectName) {
            Gcoop.CheckDw(sender, rowNumber, objectName, "pro_type", 1, 0);
            objDwMain.AcceptText();
            objDwDetail.AcceptText();
        }

        function OnClickInsertRow() {
            postInsertRow();
        }

        function OnClickDeleteRow() {
            if (objDwDetail.RowCount() > 0) {
                var currentRow = Gcoop.GetEl("HdMainRow").value;
                var confirmText = "ยืนยันการลบแถวที่ " + currentRow;
                if (confirm(confirmText)) {
                    postDeleteRow();
                }
            } else {
                alert("ยังไม่มีการเพิ่มแถวสำหรับรายการ");
            }
        }

        function SheetLoadComplete() {
            //            for (var i = 1; i <= objDwList.RowCount(); i++) {
            //                objDwList.SelectRow(i, false);
            //            }
            //            var rowNumber = Gcoop.GetEl("HdListRow").value;
            //            objDwList.SelectRow(rowNumber, true);
            //            objDwList.SetRow(rowNumber);

            for (var i = 1; i <= objDwDetail.RowCount(); i++) {
                objDwDetail.SelectRow(i, false);
            }
            var rowNumber = Gcoop.GetEl("HdMainRow").value;
            objDwDetail.SelectRow(rowNumber, true);
            objDwDetail.SetRow(rowNumber);
        }
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwList" runat="server" DataWindowObject="d_exec_project_list_dd"
        LibraryList="~/DataWindow/app_assist/as_seminar.pbl" ClientScriptable="True"
        AutoRestoreContext="false" AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" ClientEventClicked="DwListClick" ClientEventItemChanged="DwListItemChange">
    </dw:WebDataWindowControl>
    <table>
        <tr>
            <td>
                <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_exec_project_detail"
                    LibraryList="~/DataWindow/app_assist/as_seminar.pbl" ClientScriptable="True"
                    AutoRestoreContext="false" AutoRestoreDataCache="true" ClientEventItemChanged="DwMainItemChange"
                    AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True" ClientEventClicked="DwMainClick">
                </dw:WebDataWindowControl>
            </td>
        </tr>
        <tr>
            <td>
                <br />
                <asp:Label ID="Label1" runat="server" Text="รายละเอียดคอร์ส" Font-Bold="True" Font-Size="Medium"
                    ForeColor="#000066" Font-Underline="True"></asp:Label>
                <br />
                &nbsp; <span onclick="OnClickInsertRow()" style="cursor: pointer;">
                    <asp:Label ID="LbInsert2" runat="server" Text="เพิ่มแถว" Font-Bold="False" Font-Names="Tahoma"
                        Font-Size="14px" Font-Underline="True" ForeColor="#006600" /></span> &nbsp;&nbsp;
                <span onclick="OnClickDeleteRow()" style="cursor: pointer;">
                    <asp:Label ID="LbDel2" runat="server" Text="ลบแถว" Font-Bold="False" Font-Names="Tahoma"
                        Font-Size="14px" Font-Underline="True" ForeColor="Red" /></span>
                <dw:WebDataWindowControl ID="DwDetail" runat="server" DataWindowObject="d_exec_project_course_detail"
                    LibraryList="~/DataWindow/app_assist/as_seminar.pbl" ClientEventButtonClicked="DwMainButtonClicked"
                    ClientScriptable="True" AutoRestoreContext="false" AutoRestoreDataCache="true"
                    ClientEventItemChanged="DwDetailItemChange" AutoSaveDataCacheAfterRetrieve="True"
                    ClientFormatting="True" ClientEventClicked="DwDetailClick">
                </dw:WebDataWindowControl>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="HfProjectId" runat="server" />
    <asp:HiddenField ID="HdListRow" runat="server" />
    <asp:HiddenField ID="HdMainRow" runat="server" />
</asp:Content>
