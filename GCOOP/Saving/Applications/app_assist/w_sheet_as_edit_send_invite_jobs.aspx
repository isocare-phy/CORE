<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_as_edit_send_invite_jobs.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_as_edit_send_invite_jobs" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript %>
    <%=postChangeType %>
    <%=postInsertRow %>
    <%=postDeleteRow %>

    <script type="text/javascript">

        function Validate() {
            objDwMain.AcceptText();
            return confirm("ยืนยันการบันทึกข้อมูล");
        }
        
        function DwMainItemChange(sender, rowNumber, columnName, newValue) {
            objDwMain.SetItem(rowNumber, columnName, newValue);
            objDwMain.AcceptText();
            //objDwList.AcceptText();
            if (columnName == "membgroup_no_begin" || columnName == "membgroup_no_end") {
                postRefresh();
            }
            else if (columnName == "mem_type") {
                postChangeType();
            }
        }

        function DwListClick(sender, rowNumber, objectName) {
            for (var i = 1; i <= sender.RowCount(); i++) {
                sender.SelectRow(i, false);
            }
            sender.SelectRow(rowNumber, true);
            sender.SetRow(rowNumber);
            Gcoop.GetEl("HdListRow").value = rowNumber;
        }

        function DwListMemClick(sender, rowNumber, objectName) {
            for (var i = 1; i <= sender.RowCount(); i++) {
                sender.SelectRow(i, false);
            }
            sender.SelectRow(rowNumber, true);
            sender.SetRow(rowNumber);
            Gcoop.GetEl("HdListRow").value = rowNumber;
        }

        function OnClickInsertRow() {
            postInsertRow();
        }

        function OnClickDeleteRow() {
            if (objDwMain.RowCount() > 0) {
                var currentRow = Gcoop.GetEl("HdListRow").value;
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

            for (var i = 1; i <= objDwListMem.RowCount(); i++) {
                objDwListMem.SelectRow(i, false);
            }
            objDwListMem.SelectRow(rowNumber, true);
            objDwListMem.SetRow(rowNumber);
            Gcoop.GetEl("HdMainRow").value = rowNumber;
        }
        
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_sl_membsrch_criteria_edit"
        LibraryList="~/DataWindow/app_assist/as_seminar.pbl" ClientScriptable="True"
        AutoRestoreContext="False" AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" ClientEventItemChanged="DwMainItemChange">
    </dw:WebDataWindowControl>
    &nbsp; <span onclick="OnClickInsertRow()" style="cursor: pointer;">
        <asp:Label ID="LbInsert2" runat="server" Text="เพิ่มแถว" Font-Bold="False" Font-Names="Tahoma"
            Font-Size="14px" Font-Underline="True" ForeColor="#006600" /></span> &nbsp;&nbsp;
    <span onclick="OnClickDeleteRow()" style="cursor: pointer;">
        <asp:Label ID="LbDel2" runat="server" Text="ลบแถว" Font-Bold="False" Font-Names="Tahoma"
            Font-Size="14px" Font-Underline="True" ForeColor="Red" /></span>
    <asp:Panel ID="Panel2" runat="server" ScrollBars="Auto" Height="500px" Width="100%">
        <dw:WebDataWindowControl ID="DwList" runat="server" DataWindowObject="d_exec_project_invite_edit_list_jobs"
            LibraryList="~/DataWindow/app_assist/as_seminar.pbl" ClientScriptable="True"
            AutoRestoreContext="False" AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True"
            ClientFormatting="True" ClientEventClicked="DwListClick" ClientEventItemChanged="DwListItemChange">
        </dw:WebDataWindowControl>
        <dw:WebDataWindowControl ID="DwListMem" runat="server" DataWindowObject="d_exec_project_invite_edit_list_bymem_jobs"
            LibraryList="~/DataWindow/app_assist/as_seminar.pbl" ClientScriptable="True"
            AutoRestoreContext="False" AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True"
            ClientFormatting="True" ClientEventClicked="DwListMemClick" Visible="False">
        </dw:WebDataWindowControl>
    </asp:Panel>
    <asp:HiddenField ID="HdDetailRow" runat="server" />
    <asp:HiddenField ID="HdListRow" runat="server" />
    <asp:HiddenField ID="HdMainRow" runat="server" />
</asp:Content>
