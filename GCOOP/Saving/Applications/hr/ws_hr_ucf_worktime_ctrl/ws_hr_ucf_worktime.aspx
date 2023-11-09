<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="ws_hr_ucf_worktime.aspx.cs" Inherits="Saving.Applications.hr.ws_hr_ucf_worktime_ctrl.ws_hr_ucf_worktime" %>

<%@ Register Src="DsList.ascx" TagName="DsList" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

    var dsList = new DataSourceTool();

    function Validate() {
        return confirm("ยืนยันการบันทึกข้อมูล");
    }

    function OnDsDeListItemChanged(s, r, c, v) {
          if (c == "work_in") {
            var new_v = dsList.GetItem(r, "work_in");
            var worktime_code = dsList.GetItem(r, "worktime_code");
            var workin = dsList.GetItem(r, "workin");
            if (worktime_code == "01") { //////
                if (new_v.match(/([0-9]{2})([.]{1})([0-9]{2})/g) === null) {
                    alert('Fomat เวลาไม่ถูกต้องกรุณากรอกใหม่  00.00 ');
                    dsList.SetItem(r, "work_in", workin);
                } else {
                    var new_arr = new_v.split('.')
                    if (new_arr[0] > 23) {
                        alert('Fomat เวลาไม่ถูกต้องกรุณากรอกใหม่  00.00 ')
                        dsList.SetItem(r, "work_in", workin);
                    } else {
                        if (new_arr[1] > 59) {
                            alert('Fomat เวลาไม่ถูกต้องกรุณากรอกใหม่  00.00 ')
                            dsList.SetItem(r, "work_in", workin);
                        } else {

                            var new_v = new_v.match(/([0-9]{2})([.]{1})([0-9]{2})/g);

                            dsList.SetItem(r, "work_in", new_v);
                        }
                    }
                }
            } ///////
            else if (worktime_code == "02") {
                if (new_v.match(/([0-9]{2})([.]{1})([0-9]{2})/g) === null) {
                    alert('Fomat เวลาไม่ถูกต้องกรุณากรอกใหม่  00.00 ');
                    dsList.SetItem(r, "work_in", workin);
                } else {
                    var new_arr = new_v.split('.')
                    if (new_arr[0] > 23) {
                        alert('Fomat เวลาไม่ถูกต้องกรุณากรอกใหม่  00.00 ')
                        dsList.SetItem(r, "work_in", workin);
                    } else {
                        if (new_arr[1] > 59) {
                            alert('Fomat เวลาไม่ถูกต้องกรุณากรอกใหม่  00.00 ')
                            dsList.SetItem(r, "work_in", workin);
                        } else {

                            var new_v = new_v.match(/([0-9]{2})([.]{1})([0-9]{2})/g);

                            dsList.SetItem(r, "work_in", new_v);
                        }
                    }
                }
            } ///////
            else if (worktime_code == "03" || worktime_code == "04") {
                if (new_v.match(/([0-9]{2})([.]{1})([0-9]{2})/g) === null) {
                    alert('Fomat เวลาไม่ถูกต้องกรุณากรอกใหม่  00.00 ');
                    dsList.SetItem(r, "work_in", workin);
                } else {
                    var new_arr = new_v.split('.')
                    if (new_arr[0] > 23) {
                        alert('Fomat เวลาไม่ถูกต้องกรุณากรอกใหม่  00.00 ')
                        dsList.SetItem(r, "work_in", workin);
                    } else {
                        if (new_arr[1] > 59) {
                            alert('Fomat เวลาไม่ถูกต้องกรุณากรอกใหม่  00.00 ')
                            dsList.SetItem(r, "work_in", workin);
                        } else {

                            var new_v = new_v.match(/([0-9]{2})([.]{1})([0-9]{2})/g);

                            dsList.SetItem(r, "work_in", new_v);
                        }
                    }
                }
            } ///////
        } else if (c == "work_out") {
            var new_v = dsList.GetItem(r, "work_out");
            var worktime_code = dsList.GetItem(r, "worktime_code");
            var workout = dsList.GetItem(r, "workout");
            if (worktime_code == "01") { ////////
                if (new_v.match(/([0-9]{2})([.]{1})([0-9]{2})/g) === null) {
                    alert('Fomat เวลาไม่ถูกต้องกรุณากรอกใหม่  00.00 ');
                    dsList.SetItem(r, "work_out", workout);
                } else {
                    var new_arr = new_v.split('.')
                    if (new_arr[0] > 23) {
                        alert('Fomat เวลาไม่ถูกต้องกรุณากรอกใหม่  00.00 ')
                        dsList.SetItem(r, "work_out", workout);
                    } else {
                        if (new_arr[1] > 59) {
                            alert('Fomat เวลาไม่ถูกต้องกรุณากรอกใหม่  00.00 ')
                            dsList.SetItem(r, "work_out", workout);
                        } else {

                            var new_v = new_v.match(/([0-9]{2})([.]{1})([0-9]{2})/g);

                            dsList.SetItem(r, "work_out", new_v);
                        }
                    }
                }
            } ////////
            else if (worktime_code == "02") { ////////
                if (new_v.match(/([0-9]{2})([.]{1})([0-9]{2})/g) === null) {
                    alert('Fomat เวลาไม่ถูกต้องกรุณากรอกใหม่  00.00 ');
                    dsList.SetItem(r, "work_out", workout);
                } else {
                    var new_arr = new_v.split('.')
                    if (new_arr[0] > 23) {
                        alert('Fomat เวลาไม่ถูกต้องกรุณากรอกใหม่  00.00 ')
                        dsList.SetItem(r, "work_out", workout);
                    } else {
                        if (new_arr[1] > 59) {
                            alert('Fomat เวลาไม่ถูกต้องกรุณากรอกใหม่  00.00 ')
                            dsList.SetItem(r, "work_out", workout);
                        } else {

                            var new_v = new_v.match(/([0-9]{2})([.]{1})([0-9]{2})/g);

                            dsList.SetItem(r, "work_out", new_v);
                        }
                    }
                }
            } ////////
            else if (worktime_code == "03" || worktime_code == "04") { ////////
                if (new_v.match(/([0-9]{2})([.]{1})([0-9]{2})/g) === null) {
                    alert('Fomat เวลาไม่ถูกต้องกรุณากรอกใหม่  00.00 ');
                    dsList.SetItem(r, "work_out", workout);
                } else {
                    var new_arr = new_v.split('.')
                    if (new_arr[0] > 23) {
                        alert('Fomat เวลาไม่ถูกต้องกรุณากรอกใหม่  00.00 ')
                        dsList.SetItem(r, "work_out", workout);
                    } else {
                        if (new_arr[1] > 59) {
                            alert('Fomat เวลาไม่ถูกต้องกรุณากรอกใหม่  00.00 ')
                            dsList.SetItem(r, "work_out", workout);
                        } else {

                            var new_v = new_v.match(/([0-9]{2})([.]{1})([0-9]{2})/g);

                            dsList.SetItem(r, "work_out", new_v);
                        }
                    }
                }
            } ////////

        }
    }

    function SheetLoadComplete() {
    }

    function OnDsListClicked(s, r, c) {
        if (c == "b_del") {
            dsList.SetRowFocus(r);
            PostDelRow();
        }
    }

    function OnClickNewRow() {
        PostNewRow();
    }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <div align="right" style="margin-right: 1px; width: 100%;">
        <span class="NewRowLink" onclick="OnClickNewRow()">เพิ่มแถว</span>
    </div>
    <div>
        <uc1:DsList ID="dsList" runat="server" />
    </div>
</asp:Content>
