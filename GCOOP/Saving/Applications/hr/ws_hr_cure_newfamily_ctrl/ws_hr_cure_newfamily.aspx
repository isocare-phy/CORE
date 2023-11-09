<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="ws_hr_cure_newfamily.aspx.cs" Inherits="Saving.Applications.hr.ws_hr_cure_newfamily_ctrl.ws_hr_cure_newfamily" %>
<%@ Register src="DsMain.ascx" tagname="DsMain" tagprefix="uc1" %>
<%@ Register src="DsCure.ascx" tagname="DsCure" tagprefix="uc2" %>
<%@ Register src="DsDetail.ascx" tagname="DsDetail" tagprefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">

    var dsMain = new DataSourceTool();
    var dsLeaveout = new DataSourceTool;
    var dsDetail = new DataSourceTool();

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

    function OnDsLeaveItemChanged(s, r, c) {
        if (c == "start_time") {
            Post();
        }
        if (c == "last_time") {
            PostLast();
        }
    }

    function OnDsDetailClicked(s, r, c) {
        if (c == "b_detail") {
            dsDetail.SetRowFocus(r);

            //alert(r);

            PostEmpLeave();
        } else if (c == "b_delete") {

            dsDetail.SetRowFocus(r);

            //alert(r);

            PostDelete();
        }
    }

    function OnDsLeaveClicked(s, r, c) {

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
    <uc2:DsCure ID="dsCure" runat="server" />
    </div>
<div> 
    <uc3:DsDetail ID="dsDetail" runat="server" />
    </div>

</asp:Content>
