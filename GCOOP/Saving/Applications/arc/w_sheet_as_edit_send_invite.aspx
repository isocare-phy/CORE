<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_as_edit_send_invite.aspx.cs" Inherits="Saving.Applications.arc.w_sheet_as_edit_send_invite" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript %>
    <%=postSearchList %>
    <%=postChangeType %>
    <%=postInsertRow %>
    <%=postDeleteRow %>
    <%=postInvite %>
    <%=postRefresh %>
    <script type="text/javascript">

        function Validate() {
            objDwMain.AcceptText();
            return confirm("ยืนยันการบันทึกข้อมูล");
        }

        function DwProListClick(sender, rowNumber, objectName) {
            if (objectName != "datawindow" || rowNumber > 0) {
                for (var i = 1; i <= sender.RowCount(); i++) {
                    sender.SelectRow(i, false);
                }
                sender.SelectRow(rowNumber, true);
                sender.SetRow(rowNumber);
                
                Gcoop.GetEl("HdDetailRow").value = rowNumber;

                var project_id = objDwProList.GetItem(rowNumber, "project_id");
                var course_id = objDwProList.GetItem(rowNumber, "course_id");
                Gcoop.GetEl("HfProjectId").value = project_id;
                Gcoop.GetEl("HfCourseId").value = course_id;
                
            }
        }

        function DwMainButtonClick(sender, rowNumber, buttonName) {
            var project_id = Gcoop.GetEl("HfProjectId").value;
            var mem_type = objDwMain.GetItem(1, "mem_type");
            alert("Button Name = " + buttonName);//=============================================DwMain Click
            if (buttonName == "b_search") {
                if (project_id != "") {
                    if (mem_type != null) {
                        var mem_type = objDwMain.GetItem(1, "mem_type");
                        objDwMain.AcceptText();
                        //objDwList.AcceptText();
                        postSearchList();
                    }
                    else {
                        alert("กรุณาเลือกประเภทการเชิญก่อน");
                    }
                }
                else {
                    alert("กรุณาเลือกโครงการก่อน");
                }
            }
            else if (buttonName == "b_invite") {
                if (mem_type == "" || mem_type == null) {
                    alert("กรุณาเลือกประเภทการเชิญก่อน");
                }
                else if (mem_type != "" || mem_type != null) {
                    objDwMain.AcceptText();
                    //objDwList.AcceptText();
                    postInvite();
                }
            }
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
        //ฟัง
        function SheetLoadComplete() {
            for (var i = 1; i <= objDwProList.RowCount(); i++) {
                objDwProList.SelectRow(i, false);
            }
            var rowNumber = Gcoop.GetEl("HdDetailRow").value;
            objDwProList.SelectRow(rowNumber, true);
            objDwProList.SetRow(rowNumber);

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
    <asp:Panel ID="Panel1" runat="server" ScrollBars="Vertical" Height="100px" Width="620px">
        <dw:WebDataWindowControl ID="DwProList" runat="server" DataWindowObject="d_exec_project_invite_list"
            LibraryList="~/DataWindow/app_assist/as_seminar.pbl" ClientScriptable="True"
            AutoRestoreContext="false" AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True"
            ClientFormatting="True" ClientEventClicked="DwProListClick">
        </dw:WebDataWindowControl>
    </asp:Panel>
    <br />
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_sl_membsrch_criteria_edit"
        LibraryList="~/DataWindow/app_assist/as_seminar.pbl" ClientEventButtonClicked="DwMainButtonClick"
        ClientScriptable="True" AutoRestoreContext="False" AutoRestoreDataCache="true"
        AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True" ClientEventItemChanged="DwMainItemChange">
    </dw:WebDataWindowControl>
    &nbsp; <span onclick="OnClickInsertRow()" style="cursor: pointer;">
        <asp:Label ID="LbInsert2" runat="server" Text="เพิ่มแถว" Font-Bold="False" Font-Names="Tahoma"
            Font-Size="14px" Font-Underline="True" ForeColor="#006600" /></span> &nbsp;&nbsp;
    <span onclick="OnClickDeleteRow()" style="cursor: pointer;">
        <asp:Label ID="LbDel2" runat="server" Text="ลบแถว" Font-Bold="False" Font-Names="Tahoma"
            Font-Size="14px" Font-Underline="True" ForeColor="Red" /></span>
    <asp:Panel ID="Panel2" runat="server" ScrollBars="Auto" Height="500px" Width="100%">
        <dw:WebDataWindowControl ID="DwList" runat="server" DataWindowObject="d_exec_project_invite_edit_list"
            LibraryList="~/DataWindow/app_assist/as_seminar.pbl" ClientScriptable="True"
            AutoRestoreContext="False" AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True"
            ClientFormatting="True" ClientEventClicked="DwListClick" ClientEventItemChanged="DwListItemChange">
        </dw:WebDataWindowControl>
        <dw:WebDataWindowControl ID="DwListMem" runat="server" DataWindowObject="d_exec_project_invite_edit_list_bymem"
            LibraryList="~/DataWindow/app_assist/as_seminar.pbl" ClientScriptable="True"
            AutoRestoreContext="False" AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True"
            ClientFormatting="True" ClientEventClicked="DwListMemClick" Visible="False">
        </dw:WebDataWindowControl>
    </asp:Panel>
    <asp:HiddenField ID="HdDetailRow" runat="server" />
    <asp:HiddenField ID="HdListRow" runat="server" />
    <asp:HiddenField ID="HdMainRow" runat="server" />
    <asp:HiddenField ID="HfProjectId" runat="server" />
    <asp:HiddenField ID="HfCourseId" runat="server" />
</asp:Content>
