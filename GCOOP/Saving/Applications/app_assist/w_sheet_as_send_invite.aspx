<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_as_send_invite.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_as_send_invite" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript %>
    <%=postSearchList %>
    <%=postRefresh %>
    <%=postChangeType %>
    <%=postInvite %>
    <%=postcheckAll %>
    <%=postcheckAllpostcode %>
    <%=postFraction%>
    <script type="text/javascript">

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
            if (buttonName == "b_search") {
                if (project_id != "") {
                    if (mem_type != null) {
                        //var mem_type = objDwMain.GetItem(1, "mem_type");
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

        function DwListButtonClick(sender, rowNumber, buttonName) {
            if (buttonName == "b_fraction") {
                var fraction = objDwList.GetItem(rowNumber, "fraction");
                var fraction2 = objDwList.GetItem(rowNumber, "fraction2");
                Gcoop.GetEl("HfRow").value = rowNumber;
                postFraction();
            }
        }

        function DwListClick(sender, rowNumber, objectName) {
            if (objectName == "select_flag") {
                Gcoop.CheckDw(sender, rowNumber, objectName, "select_flag", 1, 0);
            }
            else if (objectName == "select_all") {
                Gcoop.CheckDw(sender, rowNumber, objectName, "select_all", 1, 0);
                Gcoop.GetEl("HfRow").value = rowNumber;
                postcheckAllpostcode();
            }
        }

        function DwMainItemChange(sender, rowNumber, columnName, newValue) {
            objDwMain.SetItem(rowNumber, columnName, newValue);
            objDwMain.AcceptText();
            //objDwList.AcceptText();
            if (columnName == "membgroup_no_begin" || columnName == "membgroup_no_end") {
                //postRefresh();
            }
            else if (columnName == "mem_type") {
                postChangeType();
            }
        }

        function DwListItemChange(sender, rowNumber, columnName, newValue) {
            objDwList.SetItem(rowNumber, columnName, newValue);
            objDwList.AcceptText();
            var countmem = objDwList.GetItem(rowNumber, "countmem");
            if (columnName == "inv_num") {
                if (newValue > countmem) {
                    alert("เกินจำนวนคนทั้งหมด");
                    objDwList.SetItem(rowNumber, "inv_num", 0);
                }
            }
        }

        function ClickCheckAll() {
            if (objDwList.RowCount() > 1 || objDwListMem.RowCount() > 1) {
                postcheckAll();
            }
        }

        function SheetLoadComplete() {
            for (var i = 1; i <= objDwProList.RowCount(); i++) {
                objDwProList.SelectRow(i, false);
            }
            var rowNumber = Gcoop.GetEl("HdDetailRow").value;
            objDwProList.SelectRow(rowNumber, true);
            objDwProList.SetRow(rowNumber);
        }
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <asp:Panel ID="Panel1" runat="server" ScrollBars="Vertical" Height="100px" Width="620px">
        <dw:WebDataWindowControl ID="DwProList" runat="server" DataWindowObject="d_exec_project_invite_list"
            LibraryList="~/DataWindow/app_assist/as_seminar.pbl" ClientScriptable="True"
            AutoRestoreContext="false" AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True"
            ClientFormatting="True" ClientEventClicked="DwProListClick" ClientEventButtonClicked="DwListButtonClick">
        </dw:WebDataWindowControl>
    </asp:Panel>
    <br />
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_sl_membsrch_criteria"
        LibraryList="~/DataWindow/app_assist/as_seminar.pbl" ClientScriptable="True"
        AutoRestoreContext="false" AutoRestoreDataCache="true" ClientEventItemChanged="DwMainItemChange"
        AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True" ClientEventButtonClicked="DwMainButtonClick">
    </dw:WebDataWindowControl>
    <br />
    <asp:CheckBox ID="CheckAll" runat="server" Text="เลือกทั้งหมด" onclick="ClickCheckAll()" />
    <asp:Panel ID="Panel2" runat="server" ScrollBars="Auto" Height="500px" Width="100%">
        <dw:WebDataWindowControl ID="DwList" runat="server" DataWindowObject="d_sl_membsrch_list_memno_bygroup_test"
            LibraryList="~/DataWindow/app_assist/as_seminar.pbl" ClientScriptable="True"
            AutoRestoreContext="False" AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True"
            ClientFormatting="True" ClientEventClicked="DwListClick" ClientEventItemChanged="DwListItemChange"
            ClientEventButtonClicked="DwListButtonClick">
            <%--<PageNavigationBarSettings Position="Top" Visible="True" NavigatorType="Numeric">
                <BarStyle HorizontalAlign="Center" />
                <NumericNavigator FirstLastVisible="True" />
            </PageNavigationBarSettings>--%>
        </dw:WebDataWindowControl>
        <dw:WebDataWindowControl ID="DwListMem" runat="server" DataWindowObject="d_sl_membsrch_list_memno_bymem_test3"
            LibraryList="~/DataWindow/app_assist/as_seminar.pbl" ClientScriptable="True"
            AutoRestoreContext="False" AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True"
            ClientFormatting="True" ClientEventClicked="DwListClick" Visible="False" RowsPerPage="50">
            <PageNavigationBarSettings Position="Top" Visible="True" NavigatorType="Numeric">
                <BarStyle HorizontalAlign="Center" />
                <NumericNavigator FirstLastVisible="True" />
            </PageNavigationBarSettings>
        </dw:WebDataWindowControl>
    </asp:Panel>
    <asp:HiddenField ID="HfProjectId" runat="server" />
    <asp:HiddenField ID="HfCourseId" runat="server" />
    <asp:HiddenField ID="HdDetailRow" runat="server" />
    <asp:HiddenField ID="HfColumn" runat="server" />
    <asp:HiddenField ID="HfRow" runat="server" />
</asp:Content>
