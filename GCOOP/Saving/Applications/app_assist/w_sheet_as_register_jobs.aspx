<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_as_register_jobs.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_as_register_jobs" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript %>
    <%=postCourseId%>
    <%=postRetrieveDwMain %>
    <%=postProtect %>
    <%=postTraveling %>
    <%=postGetDistance %>
    <%=postGetMemberDetail %>

    <script type="text/javascript">

        function Validate() {
            objDwMain.AcceptText();
            return confirm("ยืนยันการบันทึกข้อมูล");
        }

        function MenubarOpen() {
            Gcoop.OpenDlg(600, 250, "w_dlg_as_enrollment_list.aspx", "");
        }

        function DwMainButtonClick(sender, rowNumber, buttonName) {
            if (buttonName == "b_search") {
                Gcoop.OpenDlg(610, 550, "w_dlg_sl_member_search.aspx", "");
            }
        }

        function DwMainClick(sender, rowNumber, objectName) {
            if (objectName == "follower_flag") {
                Gcoop.CheckDw(sender, rowNumber, objectName, "follower_flag", 1, 0);
                objDwMain.AcceptText();
            }
            if (objectName == "employee_flag") {
                Gcoop.CheckDw(sender, rowNumber, objectName, "employee_flag", 1, 0);
                objDwMain.AcceptText();
            }
            if (objectName == "lanunch_flag_1") {
                Gcoop.CheckDw(sender, rowNumber, objectName, "lanunch_flag_1", 1, 0);
                objDwMain.AcceptText();
            }
            if (objectName == "root_flag_1") {
                Gcoop.CheckDw(sender, rowNumber, objectName, "root_flag_1", 1, 0);
                objDwMain.AcceptText();
            }
            if (objectName == "dine_flag_1") {
                Gcoop.CheckDw(sender, rowNumber, objectName, "dine_flag_1", 1, 0);
                objDwMain.AcceptText();
            }
            if (objectName == "not_root_flag") {
                Gcoop.CheckDw(sender, rowNumber, objectName, "not_root_flag", 1, 0);
                objDwMain.AcceptText();
            }
            if (objectName == "not_lanunch_flag") {
                Gcoop.CheckDw(sender, rowNumber, objectName, "not_lanunch_flag", 1, 0);
                objDwMain.AcceptText();
            }
            if (objectName == "not_dine_flag") {
                Gcoop.CheckDw(sender, rowNumber, objectName, "not_dine_flag", 1, 0);
                objDwMain.AcceptText();
            }
            if (objectName == "magnitude1_flag") {
                Gcoop.CheckDw(sender, rowNumber, objectName, "magnitude1_flag", 1, 0);
                objDwMain.AcceptText();
            }
            if (objectName == "magnitude3_flag") {
                Gcoop.CheckDw(sender, rowNumber, objectName, "magnitude3_flag", 1, 0);
                objDwMain.AcceptText();
            }
            if (objectName == "magnitude2_flag") {
                Gcoop.CheckDw(sender, rowNumber, objectName, "magnitude2_flag", 1, 0);
                objDwMain.AcceptText();
            }
            if (objectName == "magnitude4_flag") {
                Gcoop.CheckDw(sender, rowNumber, objectName, "magnitude4_flag", 1, 0);
                objDwMain.AcceptText();
            }
        }

        function DwMainItemChange(sender, rowNumber, columnName, newValue) {
            objDwMain.SetItem(rowNumber, columnName, newValue);
            objDwMain.AcceptText();
            if (columnName == "project_id") {
                postCourseId();
            }
            else if (columnName == "traveling_flag") {
                postTraveling();
            }
            else if (columnName == "member_no") {
                Gcoop.GetEl("HfMemberNo").value = newValue;
                postGetMemberDetail();
            }
        }

        function GetValueFromDlg(memberno, pre_name, memb_name, memb_surname, membgroup_desc, membgroup_code) {
            objDwMain.SetItem(1, "member_no", memberno);
            objDwMain.SetItem(1, "thaifull_name", memb_name);
            objDwMain.SetItem(1, "thaisure_name", memb_surname);
            objDwMain.SetItem(1, "membgroup_desc", membgroup_desc);
            objDwMain.SetItem(1, "membgroup_code", membgroup_code);
            var project_id = objDwMain.GetItem(1, "project_id");
            var member_no = objDwMain.GetItem(1, "member_no");
            var course_id = objDwMain.GetItem(1, "course_id");
            if (project_id != null && member_no != null && course_id != null) {
                postGetDistance();
            }
        }

        function GetValueFromDlgList(project_id, member_no, course_id) {
            objDwMain.SetItem(1, "member_no", member_no);
            objDwMain.SetItem(1, "project_id", project_id);
            objDwMain.SetItem(1, "course_id", course_id);
            objDwMain.AcceptText();
            postRetrieveDwMain();
        }
        
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_exec_project_enrollment"
        LibraryList="~/DataWindow/app_assist/as_seminar.pbl" ClientEventButtonClicked="DwMainButtonClick"
        ClientScriptable="True" AutoRestoreContext="false" AutoRestoreDataCache="true"
        ClientEventItemChanged="DwMainItemChange" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" ClientEventClicked="DwMainClick">
    </dw:WebDataWindowControl>
    <asp:HiddenField ID="HfMemberNo" runat="server" />
</asp:Content>
