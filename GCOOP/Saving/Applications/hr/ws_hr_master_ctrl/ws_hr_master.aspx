<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="ws_hr_master.aspx.cs" Inherits="Saving.Applications.hr.ws_hr_master_ctrl.ws_hr_master" %>

<%@ Register Src="DsMain.ascx" TagName="DsMain" TagPrefix="uc1" %>
<%@ Register Src="DsFamily.ascx" TagName="DsFamily" TagPrefix="uc2" %>
<%@ Register Src="DsEdu.ascx" TagName="DsEdu" TagPrefix="uc3" %>
<%@ Register Src="DsExperience.ascx" TagName="DsExperience" TagPrefix="uc4" %>
<%@ Register Src="DsTraining.ascx" TagName="DsTraining" TagPrefix="uc5" %>
<%@ Register Src="DsAssist.ascx" TagName="DsAssist" TagPrefix="uc6" %>
<%@ Register Src="DsDetail.ascx" TagName="DsDetail" TagPrefix="uc7" %>
<%@ Register Src="DsSalary.ascx" TagName="DsSalary" TagPrefix="uc8" %>
<%@ Register Src="DsGuarantee.ascx" TagName="DsGuarantee" TagPrefix="uc9" %>
<%@ Register Src="DsLeaves.ascx" TagName="DsLeaves" TagPrefix="uc10" %>
<%@ Register Src="DsMovework.ascx" TagName="DsMovework" TagPrefix="uc11" %>
<%@ Register Src="DsDisoffense.ascx" TagName="DsDisoffense" TagPrefix="uc12" %>
<%@ Register Src="DsLeaveout.ascx" TagName="DsLeaveout" TagPrefix="uc13" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        var dsMain = new DataSourceTool;

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

        function OnDsDetailItemChanged(s, r, c, v) {
            if (c == "adn_province") {
                PostAdnProvince();
            }
            else if (c == "adr_province") {
                PostAdrProvince();
            }
            else if (c == "adn_amphur") {
                PostAdnAmphur();
            }
            else if (c == "adr_amphur") {
                PostAdrAmphur();
            }
        }

        function OnDsSalaryItemChanged(s, r, c, v) {
            if (c == "salexp_bank") {
                PostExpBank();
            }
        }

        function OnDsFamilyClicked(s, r, c) {
            if (c == "b_del") {
                dsFamily.SetRowFocus(r);
                PostDeleteFamilyRow();
            }
        }

        /*function OnDsCureFamilyClicked(s, r, c) {
            if (c == "b_del") {
                dsCureFamily.SetRowFocus(r);
                PostDeleteCureFamilyRow();
            }
        }*/

        function OnDsFamily_HosClicked(s, r, c) {
            if (c == "b_del") {
                dsFamily_Hos.SetRowFocus(r);
                PostDeleteFamily_HosRow();
            }
        }

        function OnDsEduClicked(s, r, c) {
            if (c == "b_del") {
                dsEdu.SetRowFocus(r);
                PostDeleteEduRow();
            }
        }

        function OnDsExperienceClicked(s, r, c) {
            if (c == "b_del") {
                dsExperience.SetRowFocus(r);
                PostDeleteExperienceRow();
            }
        }

        function OnDsTrainingClicked(s, r, c) {
            if (c == "b_del") {
                dsTraining.SetRowFocus(r);
                PostDeleteTrainingRow();
            }
        }

        function OnDsAssistClicked(s, r, c) {
            if (c == "b_del") {
                dsAssist.SetRowFocus(r);
                PostDeleteAssistRow();
            }
        }

        function OnDsGuaranteeClicked(s, r, c) {
            if (c == "b_del") {
                dsGuarantee.SetRowFocus(r);
                PostDeleteGaranteeRow();
            }
        }

        function OnDsDisoffenseClicked(s, r, c) {
            if (c == "b_del") {
                dsDisoffense.SetRowFocus(r);
                PostDeleteDisoffenseRow();
            }
        }

        function OnDsDetailClicked(s, r, c) {
            if (c == "show") {
                displeyAddress();
            }
        }

        function SheetLoadComplete() {
            $(function () {
                var emp_no = dsMain.GetItem(0, "emp_no");
                var img_path = "../../../ImageEmployee/profile/profile_" + emp_no.trim() + ".jpg";
                $("#ctl00_ContentPlace_dsDetail_FormView1_Img_emp_profile").attr("src", img_path);
                // PostSheetLoadComplete();
            });
        }

    </script>
    <style type="text/css">
        .ui-tabs
        {
            font-family: Tahoma;
            font-size: 12px;
        }
        #tabs
        {
            width: 760px;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            var tabIndex = Gcoop.ParseInt($("#<%=hdTabIndex.ClientID%>").val());
            $("#tabs").tabs({
                active: tabIndex,
                activate: function (event, ui) {
                    $("#<%=hdTabIndex.ClientID%>").val(ui.newTab.index() + "");
                }
            });
        });</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <uc1:DsMain ID="dsMain" runat="server" />
    <br />
    <div id="tabs">
        <ul>
            <li><a href="#tabs-1">ข้อมูลเจ้าหน้าที่</a></li>
            <li><a href="#tabs-2">ข้อมูลเพิ่มเติม</a></li>
            <li><a href="#tabs-3">ประวัติครอบครัว</a></li>
            <li><a href="#tabs-4">ประวัติการศึกษา</a></li>
            <li><a href="#tabs-5">ประสบการณ์ทำงาน</a></li>
            <li><a href="#tabs-6">สัมนา/ดูงาน/ฝึกอบรม</a></li>
            <li><a href="#tabs-7">สวัสดิการ</a></li>
            <li><a href="#tabs-8">ประวัติการลา</a></li>
            <li><a href="#tabs-9">ลานอกสถานที่</a></li>
            <li><a href="#tabs-10">การค้ำประกัน</a></li>
            <li><a href="#tabs-11">ประวัติโยกย้าย</a></li>
            <li><a href="#tabs-12">ประวัติทางวินัย</a></li>
        </ul>
        <div id="tabs-1">
            <uc7:DsDetail ID="dsDetail" runat="server" />
        </div>
        <div id="tabs-2">
            <uc8:DsSalary ID="dsSalary" runat="server" />
        </div>
        <div id="tabs-3">
            <uc2:DsFamily ID="dsFamily" runat="server" />
        </div>
        <div id="tabs-4">
            <uc3:DsEdu ID="dsEdu" runat="server" />
        </div>
        <div id="tabs-5">
            <uc4:DsExperience ID="dsExperience" runat="server" />
        </div>
        <div id="tabs-6">
            <uc5:DsTraining ID="dsTraining" runat="server" />
        </div>
        <div id="tabs-7">
            <uc6:DsAssist ID="dsAssist" runat="server" />
        </div>
        <div id="tabs-8">
            <uc10:DsLeaves ID="dsLeaves" runat="server" />
        </div>
        <div id="tabs-9">
            <uc13:DsLeaveout ID="dsLeaveout" runat="server" />
        </div>
        <div id="tabs-10">
            <uc9:DsGuarantee ID="dsGuarantee" runat="server" />
        </div>
        <div id="tabs-11">
            <uc11:DsMovework ID="dsMovework" runat="server" />
        </div>
        <div id="tabs-12">
            <uc12:DsDisoffense ID="dsDisoffense" runat="server" />
        </div>
    </div>
    <asp:HiddenField ID="hdTabIndex" runat="server" />
    </div>
</asp:Content>
