<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="ws_hr_worktime_new.aspx.cs" Inherits="Saving.Applications.hr.ws_hr_worktime_new_ctrl.ws_hr_worktime_new" %>

<%@ Register Src="DsList.ascx" TagName="DsList" TagPrefix="uc1" %>
<%@ Register Src="DsMain.ascx" TagName="DsMain" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var dsMain = new DataSourceTool();
        var dsList = new DataSourceTool();

        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล");
        }


        function OnDsMainItemChanged(s, r, c, v) {
            if (c == "emp_no") {
                PostEmpno();
            }
        }
        function OnDsListItemChanged(s, r, c, v) {
            if (c == "start_time") {
                var eleId = dsList.GetElement(r, c);
                alert(eleId);
            } else if (c == "work_in") {
                var new_v = dsList.GetItem(r, "work_in");
                var worktime = dsList.GetItem(r, "worktime");
                var work_in = dsList.GetItem(r, "max_late");
                if (worktime != "01") { worktime = "01" }
                if (worktime == "01") { //////
                 
                    if (new_v.match(/([0-9]{2})([.]{1})([0-9]{2})/g) === null) {
                        alert('Fomat เวลาไม่ถูกต้องกรุณากรอกใหม่  00.00 ');
                        dsList.SetItem(r, "work_in", work_in);
                    } else {
                        var new_arr = new_v.split('.')
                        if (new_arr[0] > 23) {
                            alert('Fomat เวลาไม่ถูกต้องกรุณากรอกใหม่  00.00 ')
                            dsList.SetItem(r, "work_in", work_in);
                        } else {
                            if (new_arr[1] > 59) {
                                alert('Fomat เวลาไม่ถูกต้องกรุณากรอกใหม่  00.00 ')
                                dsList.SetItem(r, "work_in", work_in);
                            } else {

                                var new_v = new_v.match(/([0-9]{2})([.]{1})([0-9]{2})/g);

                                dsList.SetItem(r, "work_in", new_v);
                            }
                        }
                    }
                } ///////
            } else if (c == "work_out") {
                var new_v = dsList.GetItem(r, "work_out");
                var worktime = dsList.GetItem(r, "worktime");
                var hwwork_out = dsList.GetItem(r, "hwwork_out");

                if (worktime != "01") { worktime = "01" }
                if (worktime == "01") { ////////
                    if (new_v.match(/([0-9]{2})([.]{1})([0-9]{2})/g) === null) {
                        alert('Fomat เวลาไม่ถูกต้องกรุณากรอกใหม่  00.00 ');
                        dsList.SetItem(r, "work_out", hwwork_out);
                    } else {
                        var new_arr = new_v.split('.')
                        if (new_arr[0] > 23) {
                            alert('Fomat เวลาไม่ถูกต้องกรุณากรอกใหม่  00.00 ')
                            dsList.SetItem(r, "work_out", hwwork_out);
                        } else {
                            if (new_arr[1] > 59) {
                                alert('Fomat เวลาไม่ถูกต้องกรุณากรอกใหม่  00.00 ')
                                dsList.SetItem(r, "work_out", hwwork_out);
                            } else {

                                var new_v = new_v.match(/([0-9]{2})([.]{1})([0-9]{2})/g);

                                dsList.SetItem(r, "work_out", new_v);
                            }
                        }
                    }
                } ////////
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
    <uc1:DsList ID="dsList" runat="server" />
    <asp:HiddenField ID="HdRows" runat="server" />
</asp:Content>
