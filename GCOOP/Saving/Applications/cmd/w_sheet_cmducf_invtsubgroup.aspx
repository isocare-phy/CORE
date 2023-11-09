<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" 
CodeBehind="w_sheet_cmducf_invtsubgroup.aspx.cs" Inherits="Saving.Applications.cmd.w_sheet_cmducf_invtsubgroup" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%=jsPostRetriveInvtgrpsub%>
<%=jsPostSetEdit%>
    <script type="text/javascript">
        function Validate() {
            try {
                var unit_desc = objDwMain.GetItem(i, "unit_desc");
                if (unit_desc == null || unit_desc == "") {
                    alert("กรุณากรอกชื่อหน่วยวัสดุให้ถูกต้อง");
                    return;
                }
            }
            catch (Error) { }
            return confirm("ยืนยันการบันทึกข้อมูล");
        }

        function OnDwMainItemChange(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            if (c == "invtgrp_code") {
                jsPostRetriveInvtgrpsub();
            }
        }

        function DwDetailOnItemChange(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
        }

        function DwDetailOnClick(s, r, c) {
            if (c == "b_edit") {
                Gcoop.GetEl("HdR").value = r;
                jsPostSetEdit();
            }
            if (c == "b_del") {
                Gcoop.GetEl("HdR").value = r;
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_main_ucfinvtsubgrpcode"
        LibraryList="~/DataWindow/Cmd/cmd_ucfinvtsubgroupcode.pbl" Width="750px" ClientScriptable="True"
        AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" ClientEventItemChanged="OnDwMainItemChange">
    </dw:WebDataWindowControl>
    <br />
    <dw:WebDataWindowControl ID="DwDetail" runat="server" DataWindowObject="d_detail_ucfinvtsubgrpcode"
        LibraryList="~/DataWindow/Cmd/cmd_ucfinvtsubgroupcode.pbl" Width="750px" ClientScriptable="True"
        AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" ClientEventButtonClicked="DwDetailOnClick" ClientEventItemChanged="DwDetailOnItemChange">
    </dw:WebDataWindowControl>
    <asp:HiddenField ID="HdSaveMode" runat="server" />
    <asp:HiddenField ID="HdR" runat="server" />
</asp:Content>
