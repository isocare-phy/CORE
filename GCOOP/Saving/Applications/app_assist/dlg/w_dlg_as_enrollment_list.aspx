<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_as_enrollment_list.aspx.cs"
    Inherits="Saving.Applications.app_assist.dlg.w_dlg_as_enrollment_list" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <%=postGetCourseId %>
    <%=postGetList %>
    <%=postSaveList %>
    <%=postDelete %>
    <%=postFilterProjectId %>
    <title></title>
    <script type="text/javascript">

        function DwHeadItemChange(sender, rowNumber, columnName, newValue) {
           
            if (columnName == "project_id") {
                var project_id = objDwHead.GetItem(1, "project_id");
                var course_id = objDwHead.GetItem(1, "course_id");
                objDwHead.SetItem(rowNumber, columnName, newValue);
                objDwHead.AcceptText();
                postGetCourseId();
            }
        }

        function DwHeadButtonClick(sender, rowNumber, buttonName) {
            if (buttonName == "b_search") {
                postGetList();
            }
        }

        function DwMainHeadButtonClick(sender, rowNumber, buttonName) {
            if (buttonName == "b_save") {
                postSaveList();
            }
        }

        function DwMainClick(sender, rowNumber, objectName) {
            Gcoop.GetEl("HdMainRow").value = rowNumber;
            if (objectName == "member_no") {
                var project_id = objDwMain.GetItem(rowNumber, "project_id");
                var member_no = objDwMain.GetItem(rowNumber, "member_no");
                var course_id = objDwMain.GetItem(rowNumber, "course_id");
                window.opener.GetValueFromDlgList(project_id, member_no, course_id);
                window.close();
            }
        }

        function DwMainButtonClick(sender, rowNumber, buttonName) {
            Gcoop.GetEl("HdMainRow").value = rowNumber;
            if (buttonName == "b_delete") {
                var member_no = objDwMain.GetItem(rowNumber, "member_no");
                if (confirm("ยืนยันการลบข้อมูลของเลขสมาชิกที่ :" + member_no)) {
                    postDelete();
                }
            }
        }

        function SheetLoadComplete() {
            alert("1");
            for (var i = 1; i <= objDwMain.RowCount(); i++) {
                objDwMain.SelectRow(i, false);
            }
            var rowNumber = Gcoop.GetEl("HdMainRow").value;
            objDwMain.SelectRow(rowNumber, true);
            objDwMain.SetRow(rowNumber);
            alert("2");
        }
        
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <div>
        <dw:WebDataWindowControl ID="DwHead" runat="server" DataWindowObject="d_exec_project_course_list_dd_button"
            LibraryList="~/DataWindow/app_assist/as_seminar.pbl" ClientScriptable="True"
            AutoRestoreContext="false" AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True"
            ClientFormatting="True" ClientEventButtonClicked="DwHeadButtonClick" ClientEventItemChanged="DwHeadItemChange">
        </dw:WebDataWindowControl>
        <dw:WebDataWindowControl ID="DwMainHead" runat="server" DataWindowObject="d_exec_project_enrollment_test_head"
            LibraryList="~/DataWindow/app_assist/as_seminar.pbl" ClientScriptable="True"
            AutoRestoreContext="false" AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True"
            ClientFormatting="True" ClientEventButtonClicked="DwMainHeadButtonClick">
        </dw:WebDataWindowControl>
        <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" Height="500px">
            <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_exec_project_enrollment_test"
                LibraryList="~/DataWindow/app_assist/as_seminar.pbl" ClientScriptable="True"
                AutoRestoreContext="false" AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True"
                ClientFormatting="True" ClientEventClicked="DwMainClick" ClientEventButtonClicked="DwMainButtonClick">
            </dw:WebDataWindowControl>
        </asp:Panel>
    </div>
    <asp:HiddenField ID="HdMainRow" runat="server" />
    </form>
</body>
</html>
