<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_as_save_payment_jobs.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_as_save_payment_jobs" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <%=initJavaScript %>
    <%=postRefresh %>
    <%=postRetrieveCost %>
    <%=postInsertRow %>
    <%=postDeleteRow %>

    <script type="text/javascript">

        function Validate() {
            objDwMain.AcceptText();
            objDwList.AcceptText();
            return confirm("ยืนยันการบันทึกข้อมูล");
        }

        function DwListClick(sender, rowNumber, objectName) {
            for (var i = 1; i <= sender.RowCount(); i++) {
                sender.SelectRow(i, false);
            }
            sender.SelectRow(rowNumber, true);
            sender.SetRow(rowNumber);
            Gcoop.GetEl("HdListRow").value = rowNumber;

            var project_id = objDwList.GetItem(rowNumber, "project_id");
            var course_id = objDwList.GetItem(rowNumber, "course_id");
            var costs_seq = objDwList.GetItem(rowNumber, "costs_seq");
            Gcoop.GetEl("HfProjectId").value = project_id;
            Gcoop.GetEl("HfCourseId").value = course_id;
            Gcoop.GetEl("HfCostSeq").value = costs_seq;
            Gcoop.GetEl("HdDetailRow").value = rowNumber;
            postRetrieveCost();
        }

        function DwMainClick(sender, rowNumber, objectName) {
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
            //postRefresh();
        }


        function OnClickInsertRow() {
            postInsertRow();
        }

        function OnClickDeleteRow() {
            if (objDwMain.RowCount() > 0) {
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
            for (var i = 1; i <= objDwList.RowCount(); i++) {
                objDwList.SelectRow(i, false);
            }
            var rowNumber = Gcoop.GetEl("HdListRow").value;
            objDwList.SelectRow(rowNumber, true);
            objDwList.SetRow(rowNumber);

            for (var i = 1; i <= objDwMain.RowCount(); i++) {
                objDwMain.SelectRow(i, false);
            }
            objDwMain.SelectRow(rowNumber, true);
            objDwMain.SetRow(rowNumber);
            Gcoop.GetEl("HdMainRow").value = rowNumber;
        }
        
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <table>
        <tr>
            <td>
                <asp:Panel ID="Panel2" runat="server" ScrollBars="Vertical" Height="100px" Width="370px">
                    <dw:WebDataWindowControl ID="DwList" runat="server" DataWindowObject="d_exec_project_course_list_jobs"
                        LibraryList="~/DataWindow/app_assist/as_seminar.pbl" ClientScriptable="True"
                        AutoRestoreContext="false" AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True"
                        ClientFormatting="True" ClientEventClicked="DwListClick">
                    </dw:WebDataWindowControl>
                </asp:Panel>
            </td>
        </tr>
    </table>
    &nbsp; <span onclick="OnClickInsertRow()" style="cursor: pointer;">
        <asp:Label ID="LbInsert2" runat="server" Text="เพิ่มแถว" Font-Bold="False" Font-Names="Tahoma"
            Font-Size="14px" Font-Underline="True" ForeColor="#006600" /></span> &nbsp;&nbsp;
    <span onclick="OnClickDeleteRow()" style="cursor: pointer;">
        <asp:Label ID="LbDel2" runat="server" Text="ลบแถว" Font-Bold="False" Font-Names="Tahoma"
            Font-Size="14px" Font-Underline="True" ForeColor="Red" /></span>
    <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" Width="720px">
        <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_exec_project_costs"
            LibraryList="~/DataWindow/app_assist/as_seminar.pbl" ClientScriptable="True"
            AutoRestoreContext="false" AutoRestoreDataCache="true" ClientEventItemChanged="DwMainItemChange"
            AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True" ClientEventClicked="DwMainClick">
        </dw:WebDataWindowControl>
    </asp:Panel>
    <asp:HiddenField ID="HfProjectId" runat="server" />
    <asp:HiddenField ID="HfCourseId" runat="server" />
    <asp:HiddenField ID="HfCostSeq" runat="server" />
    <asp:HiddenField ID="HdDetailRow" runat="server" />
    <asp:HiddenField ID="HdListRow" runat="server" />
    <asp:HiddenField ID="HdMainRow" runat="server" />
</asp:Content>
