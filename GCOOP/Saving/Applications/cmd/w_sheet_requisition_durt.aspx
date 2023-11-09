<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" 
CodeBehind="w_sheet_requisition_durt.aspx.cs" Inherits="Saving.Applications.cmd.w_sheet_requisition_durt" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript"> function jsPostSlipinNo() { __doPostBack('__Page', 'jsPostSlipinNo') } </script>
<script type="text/javascript"> function jsPostSetFormat() { __doPostBack('__Page', 'jsPostSetFormat') } </script>

    <script type="text/javascript">

        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล");
        }

        function MenubarOpen() {
            Gcoop.OpenDlg(675, 400, "w_dlg_cmd_ptdurtslipin.aspx","");
        }

        function SheetLoadComplete() {
            var Cksave = Gcoop.GetEl("HdCksave").value;
            if (Cksave == "true") {
                Gcoop.OpenDlg(700, 570, "w_dlg_cmd_ptdurtregno.aspx", "?slipin_no=" + Gcoop.GetEl("HdSlipno").value);
            }
        }

        function DwMainOnItemChange(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            switch (c) {
                case "slipin_no":
                    jsPostSlipinNo();
                    break;
            }
        }

        function DwDetailOnItemChange(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            switch (c) {
                case "dept_code":
                    objDwDetail.SetItem(r, "dept_code", v);
                    //jsPostSetFormat();
                    break;
            }
        }

        function DwDetailOnClick(s, r, c) {
            switch (c) {
                case "b_gen":
                    Gcoop.GetEl("HdRow").value = r;
                    jsPostSetFormat();
                    break;
            }
        }

        function GetValDlgDurtSlipin(SlipinNo) {
            objDwMain.SetItem(1, "slipin_no", SlipinNo);
            objDwMain.AcceptText();
            Gcoop.GetEl("HdSlipno").value = SlipinNo;
            jsPostSlipinNo();
        }
    
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_main_requisition_durt"
    LibraryList="~/DataWindow/Cmd/cmd_requisition_durt.pbl" Width="750px" ClientScriptable="True"
    AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
    ClientFormatting="True" ClientEventItemChanged="DwMainOnItemChange">
    </dw:WebDataWindowControl>
    <dw:WebDataWindowControl ID="DwDetail" runat="server" DataWindowObject="d_detail_requisition_durt"
    LibraryList="~/DataWindow/Cmd/cmd_requisition_durt.pbl" Width="750px" ClientScriptable="True"
    AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
    ClientFormatting="True" ClientEventItemChanged="DwDetailOnItemChange" ClientEventButtonClicked="DwDetailOnClick">
    </dw:WebDataWindowControl>
    <asp:HiddenField ID="HdRow" runat="server" />
    <asp:HiddenField ID="HdDurtreg" runat="server" />
    <asp:HiddenField ID="HdSlipno" runat="server" />
    <asp:HiddenField ID="HdCksave" runat="server" Value="false" />
</asp:Content>
