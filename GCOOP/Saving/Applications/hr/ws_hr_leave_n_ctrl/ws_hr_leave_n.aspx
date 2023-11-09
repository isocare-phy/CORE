<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="ws_hr_leave_n.aspx.cs" Inherits="Saving.Applications.hr.ws_hr_leave_n_ctrl.ws_hr_leave_n" %>

<%@ Register Src="DsMain.ascx" TagName="DsMain" TagPrefix="uc1" %>
<%@ Register Src="DsLeave.ascx" TagName="DsLeave" TagPrefix="uc2" %>
<%@ Register Src="DsList.ascx" TagName="DsList" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var dsMain = new DataSourceTool;
        var dsLeave = new DataSourceTool;
        var dsList = new DataSourceTool;

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
        /*function OnDsLeaveItemChanged(s, r, c) {
        var oneDay = 24 * 60 * 60 * 1000;
        var date_from = new Date(dsLeave.GetItem(0, "leave_from"));
        var date_to = new Date(dsLeave.GetItem(0, "leave_to"));
        var diffDays;
        if (c == "leave_from" != null && c == "leave_to") {
        diffDays = calculateDate(date_from, date_to);
        }
        dsLeave.SetItem(0, "totalday", diffDays);
        }
        function calculateDate(date_from, date_to) {
        //our custom function with two parameters, each for a selected date
        diffc = date_from.getTime() - date_to.getTime();
        //getTime() function used to convert a date into milliseconds. This is needed in order to perform calculations.

        days = Math.round(Math.abs(diffc / (1000 * 60 * 60 * 24)));
        //this is the actual equation that calculates the number of days.

        return days;
        }*/
        function SheetLoadComplete() {

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <br />
    <uc1:DsMain ID="dsMain" runat="server" />
    <uc2:DsLeave ID="dsLeave" runat="server" />
    <uc3:DsList ID="dsList" runat="server" />
</asp:Content>
