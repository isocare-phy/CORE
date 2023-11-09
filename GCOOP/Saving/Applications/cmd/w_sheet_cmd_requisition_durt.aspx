<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_cmd_requisition_durt.aspx.cs" 
Inherits="Saving.Applications.cmd.w_sheet_cmd_requisition_durt" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=jsPostSlipinNo%>
    <%=jsPostFormat%>
    <script type="text/javascript">

        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล");
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
                    //jsPostFormat();
                    break;
            }
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
        ClientFormatting="True" ClientEventItemChanged="DwDetailOnItemChange">
        </dw:WebDataWindowControl>
</asp:Content>
