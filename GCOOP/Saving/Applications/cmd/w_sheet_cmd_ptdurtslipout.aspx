<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" 
CodeBehind="w_sheet_cmd_ptdurtslipout.aspx.cs" Inherits="Saving.Applications.cmd.w_sheet_cmd_ptdurtslipout" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%--    <%=jsPostDel %>
    <%=jsPostFindShow %>--%>
    <script type="text/javascript"> function jsPostDel() { __doPostBack('__Page', 'jsPostDel') } </script>
    <script type="text/javascript"> function jsPostFindShow() { __doPostBack('__Page', 'jsPostFindShow') } </script>
    <script type="text/javascript"> function jsPostInsertRow() { __doPostBack('__Page', 'jsPostInsertRow') } </script>
    <script type="text/javascript">

        function Validate() {
            try {
                for (i = 0; i <= objDwMain.RowCount(); i++) {
                    var buy_tdate = objDwMain.GetItem(i, "sale_date");
                    if (buy_tdate == null || buy_tdate == "") {
                        alert("กรุณากรอกวันที่ตัดจำหน่ายให้ถูกต้อง");
                        return;
                    }
                    var durtrcv_code = objDwMain.GetItem(i, "cutreason_code");
                    if (durtrcv_code == null || durtrcv_code == "") {
                        alert("กรุณากรอกเหตุผลการตัดจำหน่ายให้ถูกต้อง");
                        return;
                    }                 
                }
            }
            catch (Error) { }
            return confirm("ยืนยันการบันทึกข้อมูล");
        }

        function jPostInsertRow() {
            jsPostInsertRow();
        }

        function OnDwDetailItemChange(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            switch (c) {
                case "durt_id":
                    Gcoop.GetEl("HdR").value = r;
                    jsPostFindShow();
                    break;
            }
        }

        function OnDwDetailClick(s, r, c) {
            if( c == "b_durtid") {
                Gcoop.GetEl("HdR").value = r;
                Gcoop.OpenDlg("750", "570", "w_dlg_cmd_ptdurtmaster.aspx", "");
            }
            else if ( c == "b_del") {
                Gcoop.GetEl("HdR").value = r;
                jsPostDel();
            }
        }

        function OnFindShow(durt_id, durt_name) {
            objDwDetail.AcceptText();
            Gcoop.GetEl("HdDurtId").value = durt_id;
            objDwDetail.SetItem(Gcoop.GetEl("HdR").value, "durt_id", durt_id);
            jsPostFindShow();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
<asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_ptdurtslipout_main"
        LibraryList="~/DataWindow/Cmd/cmd_ptdurtslipout.pbl" Width="750px" ClientScriptable="True"
        AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True">
    </dw:WebDataWindowControl>
    <div align="left" style="margin-right: 1px; width: 765px;">
        <span class="NewRowLink" onclick="jPostInsertRow();" runat="server">เพิ่มแถว</span>
    </div>
    <dw:WebDataWindowControl ID="DwDetail" runat="server" DataWindowObject="d_ptdurtslipout_detail"
        LibraryList="~/DataWindow/Cmd/cmd_ptdurtslipout.pbl" Width="750px" ClientScriptable="True"
        AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" ClientEventItemChanged="OnDwDetailItemChange" ClientEventButtonClicked="OnDwDetailClick">
    </dw:WebDataWindowControl>
    <asp:HiddenField ID="HdR" runat="server" />
    <asp:HiddenField ID="HdDurtId" runat="server" />
</asp:Content>
