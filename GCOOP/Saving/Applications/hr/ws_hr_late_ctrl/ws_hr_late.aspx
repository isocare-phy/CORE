<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="ws_hr_late.aspx.cs" Inherits="Saving.Applications.hr.ws_hr_late_ctrl.ws_hr_late" %>
<%@ Register src="DsLate.ascx" tagname="DsLate" tagprefix="uc1" %>
<%@ Register src="DsMain.ascx" tagname="DsMain" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        var dsMain = new DataSourceTool();
        var dsLate = new DataSourceTool();

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
            function OnDsLateItemChanged(s, r, c, v) {
                
            }
            function OnDsLateClicked(s, r, c) {

            }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <uc2:DsMain ID="dsMain" runat="server" />
    <uc1:DsLate ID="dsLate" runat="server" />
</asp:Content>
