<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="ws_hr_workout.aspx.cs" Inherits="Saving.Applications.hr.ws_hr_workout_ctrl.ws_hr_workout" %>
<%@ Register src="DsMain.ascx" tagname="DsMain" tagprefix="uc1" %>
<%@ Register src="DsList.ascx" tagname="DsList" tagprefix="uc2" %>
<%@ Register src="DsLog.ascx" tagname="DsLog" tagprefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">

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

    function OnDsLogClicked(s, r, c) {
        if (c == "b_detail") {
            //alert('test');
            //dsList.SetRowStatus(r);
            PostEmpOT();
        }
    }


    function OnDsListItemChanged(s, r, c) {

        if (c == "work_out") {
            PostLast();
        } else if (c == "remark") {
            PostRemark();
        }
    }

    function SheetLoadComplete() {

    }
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
<div>
    <uc1:DsMain ID="dsMain" runat="server" />
    </div>
    <div>
        <uc2:DsList ID="dsList" runat="server" />
    </div>

    <div>
        <uc3:DsLog ID="dsLog" runat="server" />
    </div>
</asp:Content>
