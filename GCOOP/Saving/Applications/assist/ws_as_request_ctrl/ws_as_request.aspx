<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="ws_as_request.aspx.cs" Inherits="Saving.Applications.assist.ws_as_request_ctrl.ws_as_request" %>

<%@ Register Src="DsMain.ascx" TagName="DsMain" TagPrefix="uc1" %>
<%@ Register Src="DsEducation.ascx" TagName="DsEducation" TagPrefix="uc2" %>
<%@ Register Src="DsDecease.ascx" TagName="DsDecease" TagPrefix="uc3" %>
<%@ Register Src="DsFamilydecease.ascx" TagName="DsFamilydecease" TagPrefix="uc4" %>
<%@ Register Src="DsDisaster.ascx" TagName="DsDisaster" TagPrefix="uc5" %>
<%@ Register Src="DsPatronize.ascx" TagName="DsPatronize" TagPrefix="uc6" %>
<%@ Register Src="DsMedical.ascx" TagName="DsMedical" TagPrefix="uc7" %>
<%@ Register Src="DsGain.ascx" TagName="DsGain" TagPrefix="uc8" %>
<%@ Register Src="DsAmount.ascx" TagName="DsAmount" TagPrefix="uc9" %>
<%@ Register Src="DsList.ascx" TagName="DsList" TagPrefix="uc10" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        //ประกาศตัวแปร ควรอยู่บริเวณบนสุดใน tag <script>
        var dsMain = new DataSourceTool();
        var dsDecease = new DataSourceTool();
        var dsFamilydecease = new DataSourceTool();
        var dsEducation = new DataSourceTool();
        var dsDisaster = new DataSourceTool();
        var dsPatronize = new DataSourceTool();
        var dsGain = new DataSourceTool();
        var dsAmount = new DataSourceTool();
        var dsList = new DataSourceTool();

        //เช็คบัตรประชาชน//////////////////
        function checkID(id) {
            if (id.length != 13) return false;
//            for (i = 0, sum = 0; i < 12; i++) {
//                sum += parseFloat(id.charAt(i)) * (13 - i); if ((11 - sum % 11) % 10 != parseFloat(id.charAt(12)))
//                return false; return true;

//                    
//            } 
            return true;
        }

        function checkForm() {
            if (!checkID(dsFamilydecease.GetItem(0, "card_person"))) {
                alert("รหัสประชาชนไม่ถูกต้อง");
                dsFamilydecease.SetItem(0, "card_person", "");
                return;
            }
        }
        ////////////////////////////////

        //ประกาศฟังก์ชันสำหรับ event ItemChanged
        function OnDsMainItemChanged(s, r, c, v) {
            if (c == "member_no") {
                var ls_memberno = dsMain.GetItem(0, "member_no");

                dsMain.SetItem(0, "member_no", ls_memberno);
                PostMemberNo();
            }
            else if (c == "assisttype_code") {
                PostAssistType();       
            } else if (c == "assist_year") {
                PostAssistYear();
            } else if (c == "calage_date") {
                PostCalage();
            }
        }

        function OnDsBonus_insteadItemChanged(s, r, c, v) {
            if (c == "member_no_ref") {
                Gcoop.GetEl("Hd_row").value = r;
                Postmembname_ref();
            }
        }

        //ประกาศฟังก์ชันสำหรับ event Clicked
        function OnDsMainClicked(s, r, c) {
            if (c == "b_search") {
                Gcoop.OpenIFrame2(650, 600, 'wd_as_member_search.aspx', '')
            }
            else if (c == "b_add") {
                Gcoop.OpenIFrameUploadImg(Gcoop.GetEl("HdTokenIMG").value, "000", "", "");
            }
        }
        function OnDsBonus_insteadClicked(s, r, c) {
            if (c == "b_search") {
                Gcoop.GetEl("Hd_membno").value = 1;
                Gcoop.GetEl("Hd_row").value = r;
                Gcoop.OpenIFrame2(650, 600, 'wd_as_member_search.aspx', '')
            }
            if (c == "b_del") {
                Jspostdel();
            }
        }

        function OnDsListClicked(s, r, c) {
            var ls_assreqno = dsList.GetItem(r, "assistreq_docno");
            Gcoop.GetEl("hdAssreqno").value = ls_assreqno;
            InitHistory();
        }
        

        function OnDsAmountItemChanged(s, r, c, v) {
            if (c == "expense_bank") {
                var ls_expensebank = dsAmount.GetItem(0, "expense_bank");
                dsAmount.SetItem(0, "expense_bank", ls_expensebank);
                dsAmount.SetItem(0, "expense_branch", "");
                PostRetriveBankBranch();
            } else if (c == "moneytype_code") {
                var ls_montype = dsAmount.GetItem(0, "moneytype_code");
                if (ls_montype == "CSH") {
                    dsAmount.SetItem(0, "expense_bank", "");
                    dsAmount.SetItem(0, "expense_accid", "");
                    dsAmount.SetItem(0, "expense_branch", "");
                    dsAmount.SetItem(0, "send_system", "");
                    dsAmount.SetItem(0, "deptaccount_no", "");
                    dsAmount.GetElement(0, "expense_bank").disabled = true;
                    dsAmount.GetElement(0, "expense_accid").disabled = true;
                    dsAmount.GetElement(0, "send_system").disabled = true;
                    dsAmount.GetElement(0, "expense_branch").disabled = true;
                    dsAmount.GetElement(0, "deptaccount_no").disabled = true;
                    dsAmount.GetElement(0, "expense_bank").style.background = "#CCCCCC";
                    dsAmount.GetElement(0, "expense_accid").style.background = "#CCCCCC";
                    dsAmount.GetElement(0, "expense_branch").style.background = "#CCCCCC";
                    dsAmount.GetElement(0, "send_system").style.background = "#CCCCCC";
                    dsAmount.GetElement(0, "deptaccount_no").style.background = "#CCCCCC";
                } else if (ls_montype == "TRN") {
                    dsAmount.SetItem(0, "expense_bank", "");
                    dsAmount.SetItem(0, "expense_accid", "");
                    dsAmount.SetItem(0, "expense_branch", "");
                    dsAmount.SetItem(0, "send_system", "");
                    dsAmount.SetItem(0, "deptaccount_no", "");
                    dsAmount.GetElement(0, "expense_bank").disabled = true;
                    dsAmount.GetElement(0, "expense_accid").disabled = true;
                    dsAmount.GetElement(0, "expense_branch").disabled = true;
                    dsAmount.GetElement(0, "send_system").disabled = false;
                    dsAmount.GetElement(0, "deptaccount_no").disabled = true;
                    dsAmount.GetElement(0, "expense_bank").style.background = "#CCCCCC";
                    dsAmount.GetElement(0, "expense_accid").style.background = "#CCCCCC";
                    dsAmount.GetElement(0, "expense_branch").style.background = "#CCCCCC";
                    dsAmount.GetElement(0, "send_system").style.background = "#FFFFFF";
                    dsAmount.GetElement(0, "deptaccount_no").style.background = "#CCCCCC";
                } else {
                    dsAmount.SetItem(0, "send_system", "");
                    dsAmount.SetItem(0, "deptaccount_no", "");
                    dsAmount.GetElement(0, "expense_bank").disabled = false;
                    dsAmount.GetElement(0, "expense_accid").disabled = false;
                    dsAmount.GetElement(0, "expense_branch").disabled = false;
                    dsAmount.GetElement(0, "send_system").disabled = true;
                    dsAmount.GetElement(0, "deptaccount_no").disabled = true;
                    dsAmount.GetElement(0, "expense_bank").style.background = "#FFFFFF";
                    dsAmount.GetElement(0, "expense_accid").style.background = "#FFFFFF";
                    dsAmount.GetElement(0, "expense_branch").style.background = "#FFFFFF";
                    dsAmount.GetElement(0, "send_system").style.background = "#CCCCCC";
                    dsAmount.GetElement(0, "deptaccount_no").style.background = "#CCCCCC";
                }
            } else if (c == "send_system") {
                var ls_sendsys = dsAmount.GetItem(0, "send_system");
                if (ls_sendsys == "DEP") {
                    dsAmount.SetItem(0, "deptaccount_no", "");
                    dsAmount.GetElement(0, "deptaccount_no").disabled = false;
                    dsAmount.GetElement(0, "deptaccount_no").style.background = "#FFFFFF";
                } else if (ls_sendsys == "LON") {
                    dsAmount.SetItem(0, "deptaccount_no", "");
                    dsAmount.GetElement(0, "deptaccount_no").disabled = true;
                    dsAmount.GetElement(0, "deptaccount_no").style.background = "#CCCCCC";
                }
                else {
                    dsAmount.SetItem(0, "deptaccount_no", "");
                    dsAmount.GetElement(0, "deptaccount_no").disabled = true;
                    dsAmount.GetElement(0, "deptaccount_no").style.background = "#CCCCCC";
                }

            }
        }

        function GetMembNoFromDlg(memberno) {
            var memb_no = Gcoop.GetEl("Hd_membno").value;
            var r = Gcoop.GetEl("Hd_row").value;
            if (memb_no == "1") {
//                dsBonus_instead.SetItem(r, "member_no_ref", memberno.trim());
//                Postmembname_ref();
            }
            else {
                dsMain.SetItem(0, "member_no", memberno.trim());
                PostMemberNo();
            }
            
        }

        function Validate() {
            /*
            if (dsMain.GetItem(0, "assisttype_code") == "10") {
            if (Gcoop.GetEl("hdSaveChk_GPA").value > dsEducation.GetItem(0, "child_level"));
            {
            return confirm("เกรดเฉลี่ย ต่ำกว่าเกณฑ์ที่กำหนด ต้องการจะดำเนินการต่อหรือไม่");
            }
            } else if (dsMain.GetItem(0, "assisttype_code") == "20") {
            if (dsGain.GetRowCount() == 0) {
            alert("กดเพิ่มแถวและ กรอกชื่อผู้รับผลประโยชน์");
            return;
            }
            for (var i = 1; i <= dsGain.GetRowCount(); i++) {
            if (dsGain.GetItem(i - 1, "gain_name") == null || dsGain.GetItem(i - 1, "gain_surname") == null) {
            alert("ตรวจสอบชื่อและนามสกุลผู้รับผลประโยชน์");
            return;
            }
            var ls_seqno = dsGain.GetItem(i - 1, "seq_no");
            if (ls_seqno == null) {
            alert("ตรวจสอบลำดับผู้รับผลประโยชน์");
            return;
            }
            var ii = i;
            for (ii; ii <= dsGain.GetRowCount() - i; ii++) {
            if (ls_seqno == dsGain.GetItem(ii, "seq_no") || dsGain.GetItem(ii, "seq_no") == null) {
            alert("ตรวจสอบลำดับผู้รับผลประโยชน์");
            return;
            }
            }
            }
            return confirm("ยืนยันการบันทึกข้อมูล");
            } else {
            return confirm("ยืนยันการบันทึกข้อมูล");
            }*/
            return confirm("ยืนยันการบันทึกข้อมูล");
        }

        function OnDsEducationItemChanged(s, r, c, v) {
            if (c == "assistpay_code") {
                PostAssistPay();
            } else if (c == "edu_gpa") {
                if (v > 4) {
                    alert("ตรวจสอบเกรดเฉลี่ย");
                    dsEducation.SetItem(0, "edu_gpa", 0);
                    return;
                }

                if (Gcoop.GetEl("hdasscondition").value == "1") {
                    PostInitPermiss();
                }
            } else if (c == "edu_childbirthdate") {
                PostGetChildAge();
            } else if (c == "ass_rcvcardid") {
                if (!checkID(v)) {
                    alert("รหัสประชาชนไม่ถูกต้อง");
                    dsEducation.SetItem(0, "edu_childcardid", "");
                    return;
                }
                PostCardPerson();
            } else if (c == "ass_prcardid") {
                PostCardPersonParent();
            }
        }

        function OnDsDeceaseItemChanged(s, r, c, v) {
            if (c == "assistpay_code") {
                PostAssistPay();
            }
        }

        function OnDsFamilyItemChanged(s, r, c, v) {
            if (c == "ass_rcvcardid") {
                if (!checkID(v)) {
                    alert("รหัสประชาชนไม่ถูกต้อง");
                    dsEducation.SetItem(0, "fam_cardid", "");
                    return;
                }
                PostCardPerson();
            } else if (c == "assistpay_code") {
                PostAssistPay();
            }
        }

        function OnDsDisasterClicked(s, r, c) {
            if (c == "b_linkaddress") {
                PostLinkAddress();
            }
        }

        function OnDsDisasterItemChanged(s, r, c) {

            if (c == "dis_disamt") {
                if (Gcoop.GetEl("hdasscondition").value == "5") {
                    PostInitPermiss();
                }
            } else if (c == "assistpay_code") {
                PostAssistPay();
            }
        }

        function OnDsPatronizeItemChanged(s, r, c, v) {
            if (c == "assistpay_code") {
                PostAssistPay();
            }
        }

        function OnDsMedicalItemChanged(s, r, c, v) {
            if (c == "assistpay_code") {
                PostAssistPay();
            } else if (c == "med_startdate" || c == "med_enddate") {
                PostCalMedDay();
            }
        }

        function OnDsGainClicked(s, r, c, v) {
            if (c == "b_del") {
                dsGain.SetRowFocus(r);
                if (confirm("ลบข้อมูลผู้รับผลประโยชน์แถวที่ " + (r + 1) + " ?") == true) {
                    PostDelRow();
                }
            }
        }

        function PostInsertRow(s, r, c, v) {
            Gcoop.GetEl("Hd_row").value = r;
            PostInsertRow();
        }

        function SheetLoadComplete() {
            var stmflag = dsAmount.GetItem(0, "stm_flag");

            if (stmflag == 1) {
                dsAmount.GetElement(0, "")
            }
            for (var i = 0; i < dsList.GetRowCount(); i++) {
                if (i == r) {
                    dsList.GetElement(i, "assisttype_code").style.background = "#FFFF99";
                    dsList.GetElement(i, "asscontract_no").style.background = "#FFFF99";
                    dsList.GetElement(i, "approve_date").style.background = "#FFFF99";
                }
                else {
                    dsList.GetElement(i, "assisttype_code").style.background = "#FFFFFF";
                    dsList.GetElement(i, "asscontract_no").style.background = "#FFFFFF";
                    dsList.GetElement(i, "approve_date").style.background = "#FFFFFF";
                }
            }

            var ls_contsts = dsCont.GetItem(0, "asscont_status");
            if (ls_contsts == -9) {
                dsCont.GetElement(0, "statusdesc").style.background = "#FF0000";
                dsCont.GetElement(0, "statusdesc").style.color = "#FFFFFF";
            }
        }

//        function OnDsBonusItemChanged(s, r, c, v) {
//            if (c == "bonus_methpay") {
//            var bonus = dsBonus.GetItem(0,"bonus_methpay");
//            if (bonus == 3) {
//                    $('#ctl00_ContentPlace_dsBonus_instead_Panel1').show();
//                }
//                else {
//                    $('#ctl00_ContentPlace_dsBonus_instead_Panel1').hide();
//                }
//            }
//        }

        function PostInsertRowss() {
            JsInsertrow(); 
            
        }

    </script>
    <script type="text/javascript">
        Number.prototype.round = function (p) {
            p = p || 10;
            return parseFloat(this.toFixed(p));
        };

        $(function () {
            //alert($("#tabs").tabs()); //ชื่อฟิวส์

            var tabIndex = Gcoop.GetEl("hdTabIndex").value; // Gcoop.ParseInt($("#<%=hdTabIndex.ClientID%>").val());
            $("#tabs").tabs({
                active: tabIndex,
                activate: function (event, ui) {
                    $("#<%=hdTabIndex.ClientID%>").val(ui.newTab.index() + "");
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <uc1:DsMain ID="dsMain" runat="server" />
    <br />
    <div id="tabs">
        <ul style="font-size: 12px;">
            <li><a href="#tabs-1">ทุนการศึกษาบุตร</a></li>
            <li><a href="#tabs-2">สมาชิกถึงแก่กรรม</a></li>
            <li><a href="#tabs-3">ครอบครัวสมาชิก</a></li>
            <li><a href="#tabs-4">ประสบภัยพิบัติ</a></li>
            <li><a href="#tabs-5">เกื้อกูลสมาชิก</a></li>
            <li><a href="#tabs-6">รักษาพยาบาล</a></li>
        </ul>
        <div id="tabs-1">
            <uc2:DsEducation ID="dsEducation" runat="server" />
        </div>
        <div id="tabs-2">
            <uc3:DsDecease ID="dsDecease" runat="server" />
        </div>
        <div id="tabs-3">
            <uc4:DsFamilydecease ID="dsFamilydecease" runat="server" />
        </div>
        <div id="tabs-4">
            <uc5:DsDisaster ID="dsDisaster" runat="server" />
        </div>
        <div id="tabs-5">
            <uc6:DsPatronize ID="dsPatronize" runat="server" />
        </div>
        <div id="tabs-6">
            <uc7:DsMedical ID="dsMedical" runat="server" />
        </div>
    </div>
    <br />
    <div align="right" style="margin-right: 20px;">
        <span id="insertRow" style="display: none" class="NewRowLink" onclick="PostInsertRow()">
            เพิ่มแถว</span></div>
    <uc8:DsGain ID="dsGain" runat="server" />
    <uc9:DsAmount ID="dsAmount" runat="server" />
    <uc10:DsList ID="dsList" runat="server" />
    <asp:HiddenField ID="hdTabIndex" Value="0" runat="server" />
    <asp:HiddenField ID="hdSaveChk_GPA" runat="server" />
    <asp:HiddenField ID="hdInertRow" runat="server" />
    <asp:HiddenField ID="hdCalDay" runat="server" />
    <asp:HiddenField ID="hdActMemno" runat="server" />
    <asp:HiddenField ID="hdassgrp" runat="server" />
    <asp:HiddenField ID="hdasscondition" runat="server" />
    <asp:HiddenField ID="HdTokenIMG" runat="server" />
    <asp:HiddenField ID="Hd_membno" runat="server" />
    <asp:HiddenField ID="Hd_row" runat="server" />
    <asp:HiddenField ID="hdAssreqno" runat="server" />
</asp:Content>
