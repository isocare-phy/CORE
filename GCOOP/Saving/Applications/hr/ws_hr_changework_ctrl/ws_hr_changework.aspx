<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="ws_hr_changework.aspx.cs" Inherits="Saving.Applications.hr.ws_hr_changework_ctrl.ws_hr_changework" %>

<%@ Register Src="DsDetail.ascx" TagName="DsDetail" TagPrefix="uc1" %>
<%@ Register Src="DsMain.ascx" TagName="DsMain" TagPrefix="uc2" %>
<%@ Register Src="DsList.ascx" TagName="DsList" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        var dsMain = new DataSourceTool();
        var dsDetail = new DataSourceTool();
        var dsList = new DataSourceTool();
        function MenubarOpen() {
            Gcoop.OpenIFrame2('685', '460', 'ws_dlg_hr_master_search.aspx', '');
        }
        function GetEmpNoFromDlg(emp_no) {
            dsMain.SetItem(0, "emp_no", emp_no);
            PostEmpNo();
        }
        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล");
        }
        function SheetLoadComplete() {

        }
        function OnDsMainItemChanged(s, r, c, v) {
            if (c == "emp_no") {
                PostEmpNo();
            }
        }
        function OnDsMainClicked(s, r, c) {

        }
        function OnDsDetailItemChanged(s, r, c, v) {

        }
        function OnDsDetailClicked(s, r, c) {

        }
        function OnDsListItemChanged(s, r, c, v) {
            if (r >= 0) {
                Gcoop.GetEl("HdCheckRow").value = r;
                PostList();
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <uc2:DsMain ID="dsMain" runat="server" />
    <uc1:DsDetail ID="dsDetail" runat="server" />
    <uc3:DsList ID="dsList" runat="server" />
    <asp:HiddenField ID="HdCheckRow" runat="server" />
</asp:Content>
