<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_as_distance.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_as_distance" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript %>
    <%=postAddDistance %>
    <%=postEditDistance %>
    <%=postSaveEdit %>
    <%=postRefresh %>

    <script type="text/javascript">

        function DwAddButtonClick(sender, rowNumber, buttonName) {
            if (buttonName == "b_add") {
                objDwAdd.AcceptText();
                postAddDistance();
            }
        }
        
        function DwEditButtonClick(sender, rowNumber, buttonName) {
            if (buttonName == "b_edit") {
                objDwEdit.AcceptText();
                postSaveEdit();
            }
        }

        function DwAddItemChange(sender, rowNumber, columnName, newValue) {
            objDwAdd.SetItem(rowNumber, columnName, newValue);
            objDwAdd.AcceptText();
            postRefresh();
        }

        function DwEditItemChange(sender, rowNumber, columnName, newValue) {
            objDwEdit.SetItem(rowNumber, columnName, newValue);
            objDwEdit.AcceptText();
            var first_province = objDwEdit.GetItem(1, "first_province");
            var second_province = objDwEdit.GetItem(1, "second_province");
            var p_seq = objDwEdit.GetItem(1, "to_where");
            if (first_province != "" && second_province != "" && p_seq != "" && columnName != "distance") {
                postEditDistance();
            }
        }
        
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwAdd" runat="server" DataWindowObject="d_exec_project_distance_add"
        LibraryList="~/DataWindow/app_assist/as_seminar.pbl" ClientScriptable="True"
        AutoRestoreContext="false" AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" ClientEventItemChanged="DwAddItemChange" ClientEventButtonClicked="DwAddButtonClick">
    </dw:WebDataWindowControl>
    <br />
    <dw:WebDataWindowControl ID="DwEdit" runat="server" DataWindowObject="d_exec_project_distance_edit"
        LibraryList="~/DataWindow/app_assist/as_seminar.pbl" ClientScriptable="True"
        AutoRestoreContext="false" AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" ClientEventItemChanged="DwEditItemChange" ClientEventButtonClicked="DwEditButtonClick">
    </dw:WebDataWindowControl>
    <br />
    <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" Width="735px" Height="395px">
        <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_exec_project_distance"
            LibraryList="~/DataWindow/app_assist/as_seminar.pbl" ClientEventButtonClicked="DwMainButtonClicked"
            ClientScriptable="True" AutoRestoreContext="false" AutoRestoreDataCache="true"
            ClientEventItemChanged="DwMainItemChanged" AutoSaveDataCacheAfterRetrieve="True"
            ClientFormatting="True">
        </dw:WebDataWindowControl>
    </asp:Panel>
</asp:Content>
