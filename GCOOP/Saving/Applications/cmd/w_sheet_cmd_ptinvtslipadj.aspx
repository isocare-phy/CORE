<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" 
CodeBehind="w_sheet_cmd_ptinvtslipadj.aspx.cs" Inherits="Saving.Applications.cmd.w_sheet_cmd_ptinvtslipadj" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%=jsPostFindShow%>
<%=jspostDlgShow%>
<%=jsCheckBal%>
<script type="text/javascript">

    function Validate() {
        try {
            for (var i = 1; i <= objDwMain.RowCount(); i++) {
                    var invt_id = objDwMain.GetItem(i, "invt_id");
                    if (invt_id == null || invt_id == "") {
                        alert("กรุณากรอกรหัสวัสดุให้ถูกต้อง");
                        return;
                    }
                    var adjtype_code = objDwMain.GetItem(i, "adjtype_code");
                    if (adjtype_code == null || adjtype_code == "") {
                        alert("กรุณากรอกรหัสการปรับปรุงให้ถูกต้อง");
                        return;
                    }
                    var invt_qty = getObjFloat(objDwMain, i, "invt_qty");
                    if (Gcoop.GetEl("HdState").value == "-9") {
                        alert("กรุณกรอกจำนวนปรับปรุงมากกว่า จำนวน คงเหลือใน LOT ");
                        return;
                    }
                    if (invt_qty <= 0) { alert("กรุณากรอกจำนวนวัสดุให้ถูกต้อง"); return; }
                    var reason_desc = objDwMain.GetItem(i, "reason_desc");
                    if (reason_desc == null || reason_desc == "") {
                        alert("กรุณากรอกเหตุผลในการปรับปรุงให้ถูกต้อง");
                        return;
                    }   
            }
        }
        catch (Error) { }
        return confirm("ยืนยันการบันทึกข้อมูล");
    }

    function OnDwDetailOnClick(s, r, c) {
        if (c == "b_invtidsearch") {
            Gcoop.OpenDlg("750", "570", "w_dlg_cmd_ptinvtmast.aspx", "")
        }
        else if (c == "b_lotidsearch") {
            var invt_id = objDwMain.GetItem(r, "invt_id")
            Gcoop.OpenDlg("370", "400", "w_dlg_cmd_ptmetlmastlot_adj.aspx", "?invt_id=" + invt_id);
        }
    }

    function OnDwMainItemChange(s, r, c, v) {
        s.SetItem(r, c, v);
        s.AcceptText();
        switch (c) {
            case "invt_id":
                Gcoop.GetEl("HinvtId").value = v;
                jsPostFindShow();
                break;
            case "lot_id":
                Gcoop.GetEl("HinvtId").value = v;
                jspostDlgShow();
                break;
            case "invt_qty":
                jsCheckBal();
                break;
            case "adjtype_code":
                jsCheckBal();
                break;
        }
    }

    function OnFindShow(invt_id) {
        Gcoop.GetEl("HinvtId").value = invt_id;
        jsPostFindShow();
    }
    function DlgFindShow(lot_id) {
        Gcoop.GetEl("HinvtId").value = lot_id;
        jspostDlgShow();

    }

</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_invslip_adj"
    LibraryList="~/DataWindow/Cmd/cmd_ptinvtslipadj.pbl" Width="750px" ClientScriptable="True"
    AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
    ClientFormatting="True" ClientEventButtonClicked="OnDwDetailOnClick" ClientEventItemChanged="OnDwMainItemChange">
    </dw:WebDataWindowControl>
    <asp:HiddenField ID="HinvtId" runat="server" />
    <asp:HiddenField ID="HdState" Value="1" runat="server" />
</asp:Content>
