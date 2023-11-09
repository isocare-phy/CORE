<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="ws_hr_worktime.aspx.cs" Inherits="Saving.Applications.hr.ws_hr_worktime_ctrl.ws_hr_worktime" %>

<%@ Register Src="DsList.ascx" TagName="DsList" TagPrefix="uc1" %>
<%@ Register Src="DsMain.ascx" TagName="DsMain" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var dsMain = new DataSourceTool();
        var dsList = new DataSourceTool();

        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล");
        }

        function GetEmpNoFromDlg(emp_no) {
            dsList.SetItem(Gcoop.GetEl("HdRows").value, "emp_no", emp_no);       
            PostEmpNo();
        }        

        function OnDsMainItemChanged(s, r, c, v) {
            if (c == "work_date") {                
                PostWorkDate();
            }
        }

        function OnDsMainClicked(s, r, c) {
        }

        function OnDsListItemChanged(s, r, c, v) {
            if (c == "emp_no") {
                dsList.SetRowFocus(r);
                PostEmpNo();
            } else if (c == "start_time") {
                var eleId = dsList.GetElement(r, c);
                alert(eleId);
            }
        }

        function OnClickInsertRow() {
            PostInsertRow();
        }

        function OnDsListClicked(s, r, c) {
            if (c == "b_search") {
                Gcoop.GetEl("HdRows").value = r;
                dsList.SetRowFocus(r);
                Gcoop.OpenIFrame2('685', '460', 'ws_dlg_hr_master_search.aspx', '');
            }
            else if (c == "b_del") {
                dsList.SetRowFocus(r);
                PostDeleteRow();
            }
        }

        function SheetLoadComplete() {
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <uc2:DsMain ID="dsMain" runat="server" />
    <br />
    <%--<span class="NewRowLink" onclick="OnClickInsertRow()">เพิ่มแถว</span>--%>
    <uc1:DsList ID="dsList" runat="server" />
    <asp:HiddenField ID="HdRows" runat="server" />
</asp:Content>
