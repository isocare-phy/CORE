<%@ Page Title="Untitled Page" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_cm_constant_cause.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_cm_constant_cause" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <%=initJavaScript%>
 <%=jsinsertrow%>
 <script type="text/javascript">
     function Validate() {
         return confirm("ยืนยันการบันทึกข้อมูล?");
     }
     function Insertrow() {
         jsinsertrow();
     }

     function Insertrow2() {
         objdw_close.InsertRow(0);

     }
     function OnButtonClick(sender, row, name) {
         if (name == "b_delete") {
             var detail = "รหัสเหตุผล " + objdw_close.GetItem(row, "insresign_case");
             //                detail += " รหัสบัญชี " + objDwMain.GetItem(row, "account_id");
             if (confirm("คุณต้องการลบรายการ " + detail + " ใช่หรือไม่?")) {
                 objdw_close.DeleteRow(row);
             }
         }
         return 0;
     }
     

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
 <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
 <asp:HiddenField ID="Hrow" runat="server" Value="" />    
<table width="100%" border="0">
<tr>
            <td valign="top">
                <dw:WebDataWindowControl ID="dw_close" runat="server" AutoRestoreContext="False"
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
                    ClientScriptable="True" DataWindowObject="d_ins_ucf_resigncause" LibraryList="~/DataWindow/app_assist/as_insconstant.pbl"
                    ClientEventButtonClicked="OnButtonClick">
                </dw:WebDataWindowControl>
                <span class="linkSpan2" onclick="Insertrow()" style="font-size: small; color: Red;
                    float: left">เพิ่มแถว</span>
            </td>
        </tr>
</table>
</asp:Content>
