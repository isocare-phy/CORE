<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_cmd_ptdurtslipin_cancel.aspx.cs" 
Inherits="Saving.Applications.cmd.w_sheet_cmd_ptdurtslipin_cancel" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%=jsPostSearch%>
<%=jsPostSetProtect%>
<script type="text/javascript">

    function Validate() {
        for (i = 0; i <= objDwDetail.RowCount(); i++) {
            var chooseFlag = getObjFloat(objDwDetail, i, "choose_flag");
            var remark = getObjString(objDwDetail, i, "remark");
            if (chooseFlag == 1 && (remark == "" || remark == null)) {
                alert("กรุณากรอกเหตุผลในการยกเลิกรายการลงรับ");
                return;
            }
        }
        return confirm("ยืนยันการบันทึกข้อมูล");
    }

    function OnDwMainClick(s, r, c) {
        if (c == "b_search") {
            jsPostSearch(); 
        }
    }

    function OnDwDetailItemChange(s, r, c, v) {
        s.SetItem(r, c, v);
        s.AcceptText();
        if (c == "choose_flag") {
            Gcoop.GetEl("HdRow").value = r;
            jsPostSetProtect();
        }
    }

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_search_ptdurtslip"
        LibraryList="~/DataWindow/Cmd/cmd_ptdurtslipincancel.pbl" Width="750px" ClientScriptable="True"
        AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" ClientEventItemChanged="OnDwMainItemChange" ClientEventButtonClicked="OnDwMainClick">
    </dw:WebDataWindowControl>
    <dw:WebDataWindowControl ID="DwDetail" runat="server" DataWindowObject="d_detail_ptdurtslip"
        LibraryList="~/DataWindow/Cmd/cmd_ptdurtslipincancel.pbl" Width="750px" ClientScriptable="True"
        AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" ClientEventItemChanged="OnDwDetailItemChange" ClientEventButtonClicked="OnDwDetailClick">
    </dw:WebDataWindowControl>
    <asp:HiddenField ID="HdRow" runat="server" Value="0" />
</asp:Content>
