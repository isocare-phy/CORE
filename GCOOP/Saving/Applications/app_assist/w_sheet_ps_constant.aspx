<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_ps_constant.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_ps_constant" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=jsBtDel %>
    <%=jsInsertRow %>
    <script type="text/javascript">
        function Validate() {
            objDwMain.AcceptText();
            return confirm("ยืนยันการบันทึกข้อมูล");
        }

        function DwMemButtonClick(sender, rowNumber, objectName) {
            if (objectName == "b_del") {
                if (confirm("แน่ใจว่าต้องการลบแถว")) {
                    Gcoop.GetEl("HdMainRowDel").value = rowNumber;
                    jsBtDel();
                }
            }
        }

        function InsertRow() {
            objDwMain.InsertRow(0);
            //jsInsertRow();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <asp:Label ID="Label1" runat="server" Text="เพิ่มแถว" Font-Bold="True" Font-Size="Medium"
                    ForeColor="#000066" Font-Underline="True" onclick="InsertRow();"></asp:Label>

    <dw:WebDataWindowControl ID="DwMain" runat="server" LibraryList="~/DataWindow/app_assist/kt_pension.pbl"
        DataWindowObject="d_kt_constant" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientEventButtonClicked="DwMemButtonClick" ClientEventItemChanged="DwMemItemChange">
    </dw:WebDataWindowControl>
    <br />
    <br />
        <dw:WebDataWindowControl ID="DwPercent" runat="server" LibraryList="~/DataWindow/app_assist/kt_pension.pbl"
        DataWindowObject="d_ps_constant_percent" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True">
    </dw:WebDataWindowControl>
        <%--ClientEventButtonClicked="DwMemButtonClick" ClientEventItemChanged="DwMemItemChange"--%>

    <asp:HiddenField ID="HdMainRowAdd" runat="server" Value="" />
    <asp:HiddenField ID="HdMainRowDel" runat="server" Value="" />
    <asp:HiddenField ID="HdRowCount" runat="server" Value="" />
     <asp:HiddenField ID="HdRowCountPercent" runat="server" Value="" />
</asp:Content>
