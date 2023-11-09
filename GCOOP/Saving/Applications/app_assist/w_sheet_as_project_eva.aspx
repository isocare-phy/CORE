<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
 CodeBehind="w_sheet_as_project_eva.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_as_project_eva" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

 <%=initJavaScript%>
 <%=postProjecteva%>   
 <%=postInsertRow %>
 <%=postDeleteRow%>
 <%=postSave%>

 <script type="text/javascript">

        function Validate() {
            objDwMain.AcceptText();
            return confirm("ยืนยันการบันทึกข้อมูล");
        }

        function DwMainButtonClick(sender, rowNumber, buttonName) {
            objDwMain.AcceptText();
            postSave();
        }

        function OnClickInsertRow() {
            postInsertRow();
        }

        function OnClickDeleteRow() {
            if (objDwMain.RowCount() > 0) {
                var currentRow = Gcoop.GetEl("HdMainRow").value;
                var confirmText = "ยืนยันการลบแถวที่ " + currentRow;
                if (confirm(confirmText)) {
                    postDeleteRow();
                }
            } else {
                alert("ยังไม่มีการเพิ่มแถวสำหรับรายการ");
            }
        }

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <asp:Literal ID="LtAlert" runat="server"></asp:Literal>

    <br />

    <span onclick="OnClickInsertRow()" style="cursor: pointer;">
    <asp:Label ID="LbInsert2" runat="server" Text="เพิ่มแถว" Font-Bold="False" Font-Names="Tahoma"
    Font-Size="14px" Font-Underline="True" ForeColor="#006600" /></span> &nbsp;&nbsp;
    <span onclick="OnClickDeleteRow()" style="cursor: pointer;">
    <asp:Label ID="LbDel2" runat="server" Text="ลบแถว" Font-Bold="False" Font-Names="Tahoma"
    Font-Size="14px" Font-Underline="True" ForeColor="Red" /></span>

    <dw:WebDataWindowControl ID="DwMain" runat="server" LibraryList="~/DataWindow/app_assist/as_seminar.pbl"
        DataWindowObject="d_exec_project_eva" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientEventButtonClicked="DwMainButtonClick" ClientEventItemChanged="DwMainItemChange">
    </dw:WebDataWindowControl>
    <br />
      <asp:HiddenField ID="HdMainRow" runat="server" />
</asp:Content>
