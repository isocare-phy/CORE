<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_pension_sharewithdraw.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_pension_sharewithdraw" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript %>
    <%=jsPostSerch %>
    <script type="text/javascript">
        function Validate() {
            return confirm("ต้องการบันทึกข้อมูล?");
        }

        function DwCriButtonClick(sender, rowNumber, buttonName) {
            if (buttonName == "b_search") {
                jsPostSerch();
            }
        }

        function DwCriItemChange(sender, rowNumber, columnName, newValue) {
            if (columnName == "start_tdate") {
                sender.SetItem(rowNumber, columnName, newValue);
                sender.SetItem(rowNumber, "start_date", Gcoop.ToEngDate(newValue));
                sender.AcceptText();
            }
            else if (columnName == "end_tdate") {
                sender.SetItem(rowNumber, columnName, newValue);
                sender.SetItem(rowNumber, "end_date", Gcoop.ToEngDate(newValue));
                sender.AcceptText();
            }
        }

        function MenubarNew() {

            if (confirm("ยืนยันการล้างข้อมูลบนหน้าจอ")) {
                window.location = "";
            }
            return 0;
        }
        function MenubarOpen() {
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <br />
    <dw:WebDataWindowControl ID="DwCri" runat="server" LibraryList="~/DataWindow/app_assist/kt_pension.pbl"
        DataWindowObject="d_cri_rdate" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientEventButtonClicked="DwCriButtonClick" ClientEventItemChanged="DwCriItemChange">
    </dw:WebDataWindowControl>
    <br />
    <dw:WebDataWindowControl ID="DwMain" runat="server" LibraryList="~/DataWindow/app_assist/kt_pension.pbl"
        DataWindowObject="d_asn_pen_shrwth" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True">
    </dw:WebDataWindowControl>
</asp:Content>
