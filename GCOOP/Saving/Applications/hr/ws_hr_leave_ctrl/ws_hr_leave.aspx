<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="ws_hr_leave.aspx.cs" Inherits="Saving.Applications.hr.ws_hr_leave_ctrl.ws_hr_leave" %>

<%@ Register Src="DsMain.ascx" TagName="DsMain" TagPrefix="uc1" %>
<%@ Register Src="DsLeave.ascx" TagName="DsLeave" TagPrefix="uc2" %>
<%@ Register Src="DsDetail.ascx" TagName="DsDetail" TagPrefix="uc3" %>
<%@ Register Src="DsLasrights.ascx" TagName="DsLasrights" TagPrefix="uc4" %>
<%@ Register Src="DsCallas.ascx" TagName="DsCallas" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        var dsMain = new DataSourceTool();
        var dsLeave = new DataSourceTool();
        var dsDetail = new DataSourceTool();
        var dsLasrights = new DataSourceTool();

        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล");
        }

        function MenubarOpen() {
            Gcoop.OpenIFrame2('685', '460', 'ws_dlg_hr_master_search.aspx', '');
        }

        function GetEmpNoFromDlg(emp_no) {
            dsMain.SetItem(0, "emp_no", emp_no);
            PostEmpNo();
        }

        function OnDsMainItemChanged(s, r, c, v) {//sender,row,colum,value
            if (c == "emp_no") {
                PostEmpNo();
            }
        }
        function OnDsMainClicked(s, r, c) {

        }
        function OnDsLeaveItemChanged(s, r, c, v) {

        }
        function OnDsLeaveClicked(s, r, c) {

        }

        function OnDsDetailClicked(s, r, c) {
            if (c == "b_detail") {
                dsDetail.SetRowFocus(r);
                PostEmpLeave();
            }
        }

        function OnDsDetailClicked(s, r, c) {
            if (c == "b_delete") {
                dsDetail.SetRowFocus(r);
                PostEmpLeaveDelete();
            }
        }

        function OnDsLeaveItemChanged(s, r, c) {

            if (c == "Leave_To") {
                Postday();
            }
        }

        function SheetLoadComplete() {

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <uc1:DsMain ID="dsMain" runat="server" />
    <div>
        <uc4:DsLasrights ID="dsLasrights" runat="server" /></div>
        <uc5:DsCallas ID="dsCallas" runat="server" />
    <div><uc2:DsLeave ID="dsLeave" runat="server" /></div>
    <uc3:DsDetail ID="dsDetail" runat="server" />
</asp:Content>
