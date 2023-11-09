<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_as_distance_province.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_as_distance_province" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript %>
    <%=postRetrieve %>
    <%=postRefresh %>
    <%=postInsertRow %>
    <%=postSave %>
    <%=postDeleteRow %>
    <script type="text/javascript">

        function DwHeadButtonClick(sender, rowNumber, buttonName) {
            objDwHead.AcceptText();
            postRetrieve();
        }

        function DwMainButtonClick(sender, rowNumber, buttonName) {
            objDwMain.AcceptText();
            postSave();
        }

        function DwHeadItemChange(sender, rowNumber, columnName, newValue) {
            objDwHead.SetItem(rowNumber, columnName, newValue);
            objDwHead.AcceptText();
            //postRefresh();
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
    <dw:WebDataWindowControl ID="DwHead" runat="server" DataWindowObject="d_exec_project_distance_post_retrieve"
        LibraryList="~/DataWindow/app_assist/as_seminar.pbl" ClientScriptable="True"
        AutoRestoreContext="false" AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" ClientEventItemChanged="DwHeadItemChange" ClientEventButtonClicked="DwHeadButtonClick">
    </dw:WebDataWindowControl>
    <br />
    <span onclick="OnClickInsertRow()" style="cursor: pointer;">
        <asp:Label ID="LbInsert2" runat="server" Text="เพิ่มแถว" Font-Bold="False" Font-Names="Tahoma"
            Font-Size="14px" Font-Underline="True" ForeColor="#006600" /></span> &nbsp;&nbsp;
    <span onclick="OnClickDeleteRow()" style="cursor: pointer;">
        <asp:Label ID="LbDel2" runat="server" Text="ลบแถว" Font-Bold="False" Font-Names="Tahoma"
            Font-Size="14px" Font-Underline="True" ForeColor="Red" /></span>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_exec_project_distance_post_test"
        LibraryList="~/DataWindow/app_assist/as_seminar.pbl" ClientScriptable="True"
        AutoRestoreContext="false" AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" ClientEventButtonClicked="DwMainButtonClick">
    </dw:WebDataWindowControl>
    <asp:HiddenField ID="HdMainRow" runat="server" />
</asp:Content>
