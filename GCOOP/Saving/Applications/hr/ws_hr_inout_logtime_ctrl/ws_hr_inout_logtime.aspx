<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="ws_hr_inout_logtime.aspx.cs" Inherits="Saving.Applications.hr.ws_hr_inout_logtime_ctrl.ws_hr_inout_logtime" %>

<%@ Register Src="DsMain.ascx" TagName="DsMain" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var dsMain = new DataSourceTool;

        function date_time(id) {
            date = new Date;
            year = date.getFullYear() + 543;
            month = date.getMonth();
            months = new Array('มกราคม', 'กุมภาพันธ์', 'มีนาคม', 'เมษายน', 'พฤษภาคม', 'มิถุนายน', 'กรกฎาคม', 'สิงหาคม', 'กันยายน', 'ตุลาคม', 'พฤศจิกายน', 'ธันวาคม');
            d = date.getDate();
            day = date.getDay();
            days = new Array('อาทิตย์', 'จันทร์', 'อังคาร', 'พุธ', 'พฤหัสดี', 'ศุกร์', 'เสาร์');
            h = date.getHours();
            if (h < 10) {
                h = "0" + h;
            }
            m = date.getMinutes();
            if (m < 10) {
                m = "0" + m;
            }
            s = date.getSeconds();
            if (s < 10) {
                s = "0" + s;
            }
            result = 'วัน' + days[day] + 'ที่ ' + d + ' ' + months[month] + ' พ.ศ.' + year + ' เวลา ' + h + ':' + m + ':' + s;
            document.getElementById(id).innerHTML = result;
            setTimeout('date_time("' + id + '");', '1000');
            return true;
        }
        function OnDsMainClicked(s, r, c) {
            if (c == "process") {
                dsMain.Focus("emp_no");
                PostWorkDate();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <span id="date_time" style="color: #000000; font-size: x-large;"></span>
    <script type="text/javascript">        window.onload = date_time('date_time');</script>
    <br />
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <br />
    <uc1:DsMain ID="dsMain" runat="server" />
</asp:Content>
