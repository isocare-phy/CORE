<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_cmducf_invtadjtype.aspx.cs" Inherits="Saving.Applications.cmd.w_sheet_cmducf_invtadjtype" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=jsPostDelete%>
    <script type="text/javascript">

        function Validate() {
            try {
                for (var i = 1; i <= objDwMain.RowCount(); i++) {
                    var adjtype_desc = objDwMain.GetItem(i, "adjtype_desc");
                    if (adjtype_desc == null || adjtype_desc == " ") {
                        alert("กรุณากรอกรายละเอียดการปรับปรุงให้ถูกต้อง");
                        return;
                    }
                }
            }
            catch (Error) { }
        return confirm("ยืนยันการบันทึกข้อมูล");
        }

        function DwDetailOnClick(s, r, c) {
            if (c == "b_del") {
                var groupCode = objDwDetail.GetItem(r, "adjtype_code");
                var isConfirm = confirm("ต้องการลบข้อมูล " + groupCode + "ใช่หรือไม่ ?");
                if (isConfirm) {
                    Gcoop.GetEl("HdR").value = r;
                    jsPostDelete();
                }
            }
            return false;
        }

        function DwDetailoutOnClick(s, r, c) {
            if (c == "b_del") {
                var groupCode = objDwDetailout.GetItem(r, "adjtype_code");
                var isConfirm = confirm("ต้องการลบข้อมูล " + groupCode + "ใช่หรือไม่ ?");
                if (isConfirm) {
                    Gcoop.GetEl("HdR").value = r;
                    jsPostDelete();
                }
            }
            return false;
        }

        function postOnInsert() {
            jsPostOnInsert();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_main_ucfinvtadjtype"
        LibraryList="~/DataWindow/Cmd/cmd_ucfinvtadjtype.pbl" Width="750px" ClientScriptable="True"
        AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" ClientEventButtonClicked="DwMainOnClick" ClientEventItemChanged="DwMainOnItemChange">
    </dw:WebDataWindowControl>
    <br />
    <br />
    <dw:WebDataWindowControl ID="DwDetail" runat="server" DataWindowObject="d_detail_ucfinvtadjtype"
        LibraryList="~/DataWindow/Cmd/cmd_ucfinvtadjtype.pbl" Width="750px" ClientScriptable="True"
        AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" ClientEventButtonClicked="DwDetailOnClick" ClientEventItemChanged="DwDetailOnItemChange">
    </dw:WebDataWindowControl>
    <br />
    <br />
    <dw:WebDataWindowControl ID="DwDetailout" runat="server" DataWindowObject="d_detail_ucfinvtadjtype_1"
        LibraryList="~/DataWindow/Cmd/cmd_ucfinvtadjtype.pbl" Width="750px" ClientScriptable="True"
        AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" ClientEventButtonClicked="DwDetailoutOnClick" ClientEventItemChanged="DwDetailOnItemChange">
    </dw:WebDataWindowControl>
    <asp:HiddenField ID="HdR" runat="server" />
    
    
</asp:Content>