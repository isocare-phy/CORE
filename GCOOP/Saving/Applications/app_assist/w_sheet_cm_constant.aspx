<%@ Page Title="Untitled Page" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_cm_constant.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_cm_constant" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=jsinsertrow%>
   
    <script type="text/javascript">
        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล?");
        }
        function InsertRow() {
            jsinsertrow();          
        }        
        
        function OnButtonClick(sender, row, name) {
            if (name == "b_delete") {
                var detail = "รหัสสถานะภาพ " + objdw_status.GetItem(row, "insmemb_type");
                //                detail += " รหัสบัญชี " + objDwMain.GetItem(row, "account_id");
                if (confirm("คุณต้องการลบรายการ " + detail + " ใช่หรือไม่?")) {
                    objdw_status.DeleteRow(row);
                }
            }
            return 0;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <asp:HiddenField ID="Hrow" runat="server" Value="" />
    
    <asp:HiddenField ID="HdStatusRow" runat="server" Value="" />
    <table width="100%" border="0">
        <tr>
            <td valign="top">
                <dw:WebDataWindowControl ID="dw_status" runat="server" AutoRestoreContext="False"
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
                    ClientScriptable="True" DataWindowObject="d_ins_ucf_mbtyp" LibraryList="~/DataWindow/app_assist/as_insconstant.pbl"
                    ClientEventButtonClicked="OnButtonClick" >
                </dw:WebDataWindowControl>
                <span class="linkSpan" onclick="InsertRow()" style="font-size: small; color: Red;
                    float: left">เพิ่มแถว</span>
            </td>
        </tr>
       
        
    </table>
</asp:Content>
