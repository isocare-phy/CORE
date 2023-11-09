<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" 
CodeBehind="w_sheet_cmducf_invtlitemcode.aspx.cs" Inherits="Saving.Applications.cmd.w_sheet_cmducf_invtlitemcode" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=jsPostDelete%>
    <script type="text/javascript">

        function Validate() {
            try {
                for (var i = 1; i <= objDwMain.RowCount(); i++) {
                    var item_des = objDwMain.GetItem(i, "item_des");
                    if (item_des == null || item_des == " ") {
                        alert("กรุณากรอกรายละเอียดการเคลื่อนไหวให้ถูกต้อง");
                        return;
                    }
                }
            }
            catch (Error) { }
            return confirm("ยืนยันการบันทึกข้อมูล");
        }


        function DwDetailOnClick(s, r, c) {
            if (c == "b_del") {
                var groupCode = objDwDetail.GetItem(r, "item_code");
                var isConfirm = confirm("ต้องการลบข้อมูล " + groupCode + "ใช่หรือไม่ ?");
                if (isConfirm) {
                    Gcoop.GetEl("HdR").value = r;
                    jsPostDelete();
                }
            }
            return false;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
<asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
            <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_main_ucfinvtlitemcode" 
            LibraryList="~/DataWindow/Cmd/cmd_ucfinvtlitemcode.pbl"  Width="750px" ClientScriptable="True"
            AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
            ClientFormatting="True" ClientEventButtonClicked="DwMainOnClick" ClientEventItemChanged="DwMainOnItemChange">
            </dw:WebDataWindowControl>
            <br />
             <dw:WebDataWindowControl ID="DwDetail" runat="server" DataWindowObject="d_detail_ucfinvtlitemcode" 
            LibraryList="~/DataWindow/Cmd/cmd_ucfinvtlitemcode.pbl"  Width="750px" ClientScriptable="True"
            AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
            ClientFormatting="True" ClientEventButtonClicked="DwDetailOnClick" ClientEventItemChanged="DwDetailOnItemChange">
            </dw:WebDataWindowControl>
    <asp:HiddenField ID="HdR" runat="server" />
            
    
</asp:Content>
