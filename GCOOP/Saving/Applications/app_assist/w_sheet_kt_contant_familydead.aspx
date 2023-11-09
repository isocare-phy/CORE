<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" 
CodeBehind="w_sheet_kt_contant_familydead.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_kt_contant_familydead" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%=initJavaScript%>
    <%=jsBtDel %>
    <%=jsInsertRow %>
    <%=jsTypeBtDel%>
    <script type="text/javascript">
        function Validate() {
            objDwMain.AcceptText();
            return confirm("ยืนยันการบันทึกข้อมูล");
        }

        function DwMainButtonClick(sender, rowNumber, objectName) {
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
        function DwTypeButtonClick(sender, rowNumber, objectName) {
            if (objectName == "b_del_type") {
                if (confirm("แน่ใจว่าต้องการลบแถว")) {
                    Gcoop.GetEl("HdTypeRowDel").value = rowNumber;
                    jsTypeBtDel();
                }
            }
        }

        function TypeInsertRow() {
            objDwType.InsertRow(0);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
<asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <asp:Label ID="Label1" runat="server" Text="เพิ่มแถว" Font-Bold="True" Font-Size="Medium"
                    ForeColor="#000066" Font-Underline="True" onclick="InsertRow();"></asp:Label>

    <dw:WebDataWindowControl ID="DwMain" runat="server" LibraryList="~/DataWindow/app_assist/kt_paydead.pbl"
        DataWindowObject="d_kt_contant_familydead" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientEventButtonClicked="DwMainButtonClick">
    </dw:WebDataWindowControl>

    <asp:Label ID="Label2" runat="server" Text="เพิ่มแถว" Font-Bold="True" Font-Size="Medium"
                    ForeColor="#000066" Font-Underline="True" onclick="TypeInsertRow();"></asp:Label>

    <dw:WebDataWindowControl ID="DwType" runat="server" LibraryList="~/DataWindow/app_assist/kt_paydead.pbl"
        DataWindowObject="d_kt_contant_familytype" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientEventButtonClicked="DwTypeButtonClick" >
    </dw:WebDataWindowControl>

    <asp:HiddenField ID="HdMainRowAdd" runat="server" Value="" />
    <asp:HiddenField ID="HdMainRowDel" runat="server" Value="" />
    <asp:HiddenField ID="HdMainRowCount" runat="server" Value="" />

    <asp:HiddenField ID="HdTypeRowAdd" runat="server" Value="" />
    <asp:HiddenField ID="HdTypeRowDel" runat="server" Value="" />
    <asp:HiddenField ID="HdTypeRowCount" runat="server" Value="" />
</asp:Content>
