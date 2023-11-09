<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_as_new_project_jobs.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_as_new_project_jobs" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript %>
    <%=postProjectId %>
    
    <script type="text/javascript">

        function Validate() {
            objDwMain.AcceptText();
            objDwDetail.AcceptText();
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
            Gcoop.GetEl("HfProjectId").value = project_id;
            postProjectId();
        }

        function DwMainItemChange(sender, rowNumber, columnName, newValue) {
            objDwMain.SetItem(rowNumber, columnName, newValue);
            objDwMain.AcceptText();
        }

        function DwMainClick(sender, rowNumber, objectName) {
            Gcoop.CheckDw(sender, rowNumber, objectName, "follower_flag", 1, 0);
            objDwMain.AcceptText();
            objDwDetail.AcceptText();
        }

        function SheetLoadComplete() {
            for (var i = 1; i <= objDwList.RowCount(); i++) {
                objDwList.SelectRow(i, false);
            }
            var rowNumber = Gcoop.GetEl("HdListRow").value;
            objDwList.SelectRow(rowNumber, true);
            objDwList.SetRow(rowNumber);
        }
        
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <table>
        <tr>
            <td valign="top">
                <dw:WebDataWindowControl ID="DwList" runat="server" DataWindowObject="d_exec_project_list_jobs"
                    LibraryList="~/DataWindow/app_assist/as_seminar.pbl" ClientScriptable="True"
                    AutoRestoreContext="false" AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True"
                    ClientFormatting="True" ClientEventClicked="DwListClick">
                </dw:WebDataWindowControl>
            </td>
            <td>
                <table>
                    <tr>
                        <td>
                            <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_exec_project_detail"
                                LibraryList="~/DataWindow/app_assist/as_seminar.pbl" ClientScriptable="True"
                                AutoRestoreContext="false" AutoRestoreDataCache="true" ClientEventItemChanged="DwMainItemChange"
                                AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True">
                            </dw:WebDataWindowControl>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                            <dw:WebDataWindowControl ID="DwDetail" runat="server" DataWindowObject="d_exec_project_course_detail"
                                LibraryList="~/DataWindow/app_assist/as_seminar.pbl" ClientEventButtonClicked="DwMainButtonClicked"
                                ClientScriptable="True" AutoRestoreContext="false" AutoRestoreDataCache="true"
                                ClientEventItemChanged="DwMainItemChanged" AutoSaveDataCacheAfterRetrieve="True"
                                ClientFormatting="True">
                            </dw:WebDataWindowControl>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="HfProjectId" runat="server" />
    <asp:HiddenField ID="HdListRow" runat="server" />
</asp:Content>