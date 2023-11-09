<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" 
CodeBehind="w_sheet_cmd_ptinvtmast.aspx.cs" Inherits="Saving.Applications.cmd.w_sheet_cmd_ptinvtmast" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=jsPostOnInsert%>
    <%=postFindShow%>
    <%=jsPostInvtGrp%>
    <%=jsPostDealer%>
    <script type="text/javascript">

        function Validate() {
            try {
                for (i = 0; i <= objDwMain.RowCount(); i++) {
                    var invt_name = objDwMain.GetItem(i, "invt_name");
                    if (invt_name == null || invt_name == "" ) {
                        alert("กรุณากรอกชื่อวัสดุให้ถูกต้อง");
                        return;
                    }
                    var qty_max = objDwMain.GetItem(i, "qty_max");
                    var qty_reorder = objDwMain.GetItem(i, "qty_reorder");
                    if (qty_max == null || qty_max == "" && qty_reorder == null || qty_reorder == "") {
                    }
                    var invtgrp_code = objDwMain.GetItem(i, "invtgrp_code");
                    if (invtgrp_code == null || invtgrp_code == "") {
                        alert("กรุณากรอกหมวดวัสดุให้ถูกต้อง");
                        return;
                    }
                }
            }
            catch (Error) { }
            return confirm("ยืนยันการบันทึกข้อมูล");
        }

        function postOnInsert() {
            jsPostOnInsert();
        }

        function MenubarOpen() {
            Gcoop.OpenDlg(580, 570, "w_dlg_cmd_ptinvtmast.aspx", "");
        }

        function OnDwMainItemChange(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            switch (c) {
                case "invtgrp_code":
                    jsPostInvtGrp();
                    break;
                case "dealer_no":
                    jsPostDealer();
                    break;
            }
        }

        function OnDwMainClicked(s, r, c) {
            if (c == "b_seardeal") {
                Gcoop.OpenDlg(600, 500, "w_dlg_cmd_dealermaster.aspx", "");
            }
        }

        function OnFindShow(invt_id, invt_name) {
            Gcoop.GetEl("HinvtId").value = invt_id;
            Gcoop.GetEl("HinvtName").value = invt_name;
            objDwMain.SetItem(1, "invt_id", invt_id);
            objDwMain.SetItem(1, "invt_name", invt_name);
            objDwMain.AcceptText();
            postFindShow();
        }

        function GetDlgDealer(dealerNo) {
            objDwMain.SetItem(1, "dealer_no", dealerNo);
            objDwMain.AcceptText();
            jsPostDealer();
        }

        function MenubarNew() {
            window.location = state.SsUrl + "Applications/cmd/w_sheet_cmd_ptinvtmast.aspx";
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_detail_ptinvtmast"
        LibraryList="~/DataWindow/Cmd/cmd_ptinvtmast.pbl" Width="750px" ClientScriptable="True"
        AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" ClientEventButtonClicked="OnDwMainClicked" ClientEventItemChanged="OnDwMainItemChange">
    </dw:WebDataWindowControl>
    <br />
    <dw:WebDataWindowControl ID="DwDetail" runat="server" DataWindowObject="d_cmd_statement"
        LibraryList="~/DataWindow/Cmd/cmd_ptinvtmast.pbl" Width="750px" ClientScriptable="True"
        AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" >
    </dw:WebDataWindowControl>
    <div style="height: 18px; vertical-align: bottom; padding-left:65px;">
        <%--<span class="linkSpan" onclick="postOnInsert()" style="font-size: small; color:Red;float: left">เพิ่มข้อมูล</span>--%>
    </div>
    <asp:HiddenField ID="HinvtId" runat="server" />
    <asp:HiddenField ID="HinvtName" runat="server" />
</asp:Content>
