<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="ws_wheet_as_request.aspx.cs" Inherits="Saving.Applications.assist.ws_wheet_as_request_ctrl.ws_wheet_as_request" %>
<%@ Register Src="DsMain.ascx" TagName="DsMain" TagPrefix="uc1" %>
<%@ Register Src="DsAssisttype.ascx" TagName="DsAssisttype" TagPrefix="uc2" %>
<%@ Register Src="DsDecease.ascx" TagName="DsDecease" TagPrefix="uc3" %>
<%@ Register Src="DsFamilydecease.ascx" TagName="DsFamilydecease" TagPrefix="uc8" %>
<%@ Register Src="DsEducation.ascx" TagName="DsEducation" TagPrefix="uc4" %>
<%@ Register Src="DsDisaster.ascx" TagName="DsDisaster" TagPrefix="uc5" %>
<%@ Register Src="DsPatronize.ascx" TagName="DsPatronize" TagPrefix="uc6" %>
<%@ Register Src="DsGain.ascx" TagName="DsGain" TagPrefix="uc10" %>
<%@ Register Src="DsAmount.ascx" TagName="DsAmount" TagPrefix="uc7" %>
<%@ Register Src="DsList.ascx" TagName="DsList" TagPrefix="uc9" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        //ประกาศตัวแปร ควรอยู่บริเวณบนสุดใน tag <script>
        var dsMain = new DataSourceTool();
        var dsAssisttype = new DataSourceTool();
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
            for (i = 0, sum = 0; i < 12; i++)
                sum += parseFloat(id.charAt(i)) * (13 - i); if ((11 - sum % 11) % 10 != parseFloat(id.charAt(12)))
                return false; return true;
        }
        function checkForm_child() {
            if (!checkID(dsEducation.GetItem(0, "child_card_person"))) {
                alert("รหัสประชาชนไม่ถูกต้อง");
                dsEducation.SetItem(0, "child_card_person", "");
                return;
            }
        }
        function checkForm() {
            if (!checkID(dsFamilydecease.GetItem(0, "family_card_person"))) {
                alert("รหัสประชาชนไม่ถูกต้อง");
                dsFamilydecease.SetItem(0, "family_card_person", "");
                return;
            }
        }
        ////////////////////////////////

        //ประกาศฟังก์ชันสำหรับ event ItemChanged
        function OnDsMainItemChanged(s, r, c, v) {
            if (c == "member_no") {
                var ls_memberno = dsMain.GetItem(0, "member_no");
                dsMain.SetItem(0, "member_no", ls_memberno);
                Gcoop.GetEl("hdActMemno").value = "chk1";
                Gcoop.GetEl("hdCheckDsAmount").value = "chk1";
                PostRetriveMain();
            }
        }

        //ประกาศฟังก์ชันสำหรับ event Clicked
        function OnDsMainClicked(s, r, c) {
            if (c == "b_search") {
                Gcoop.OpenIFrame2(650, 600, 'w_dlg_sl_member_search_tks.aspx', '')
            }
        }
        function OnDsAmountItemChanged(s, r, c, v) {
            if (c == "expense_bank") {
                var ls_expensebank = dsAmount.GetItem(0, "expense_bank");
                dsAmount.SetItem(0, "expense_bank", ls_expensebank);
                dsAmount.SetItem(0, "expense_branch", "");
                PostRetriveBranch();
            } else if (c == "moneytype_code") {
                var ls_montype = dsAmount.GetItem(0, "moneytype_code");
                if (ls_montype == "CSH") {
                    dsAmount.SetItem(0, "expense_bank", "");
                    dsAmount.SetItem(0, "expense_accid", "");
                    dsAmount.SetItem(0, "expense_branch", "");
                    dsAmount.SetItem(0, "send_system", "กรุณาเลือกระบบ");
                    dsAmount.SetItem(0, "deptaccount_no", "กรุณาเลือกบัญชี");
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
                    dsAmount.SetItem(0, "send_system", "กรุณาเลือกระบบ");
                    dsAmount.SetItem(0, "deptaccount_no", "กรุณาเลือกบัญชี");
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
                    dsAmount.SetItem(0, "send_system", "กรุณาเลือกระบบ");
                    dsAmount.SetItem(0, "deptaccount_no", "กรุณาเลือกบัญชี");
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
                    dsAmount.SetItem(0, "deptaccount_no", "กรุณาเลือกบัญชี");
                    dsAmount.GetElement(0, "deptaccount_no").disabled = false;
                    dsAmount.GetElement(0, "deptaccount_no").style.background = "#FFFFFF";
                } else {
                    dsAmount.SetItem(0, "deptaccount_no", "กรุณาเลือกบัญชี");
                    dsAmount.GetElement(0, "deptaccount_no").disabled = true;
                    dsAmount.GetElement(0, "deptaccount_no").style.background = "#CCCCCC";
                }
            } else if (c == "approve_amt") {
                PostChkMoney();
            }

        }
        function OnDsDisasterClicked(s, r, c) {
            if (c == "b_linkaddress") {
                PostLinkAddress();
            }
        }
        function OnDsDisasterItemChanged(s, r, c) {
            if (c == "h_province_code") {
                PostGetCurrDistrict();
                dsDisaster.Focus(0, c);
            } else if (c == "province_code") {
                PostGetDistrict();
                dsDisaster.Focus(0, c);
            } else if (c == "h_district_code") {
                PostGetCurrPostcode();
            } else if (c == "district_code") {
                PostGetPostcode();
            } else if (c == "disaster_type") {
                PostGenBaht2();
            } 
        }       
        function GetMembNoFromDlg(memberno) {
            dsMain.SetItem(0, "member_no", memberno.trim());
            PostRetriveMain();
        }
        function Validate() {
            if (dsAssisttype.GetItem(0, "assisttype_code") == "10") {
                //alert(Gcoop.GetEl("hdSaveChk_GPA").value)
            
                return confirm("ยืนยันการบันทึกข้อมูล");
            } else if (dsAssisttype.GetItem(0, "assisttype_code") == "20") {
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
            }
        }
        function OnDsAssisttypeItemChanged(s, r, c, v) {
            if (c == "assisttype_code") {
                PostTap();
            } else if (c == "ass_year") {
                PostLastYear();
            }
        }
        function OnDsFamilyItemChanged(s, r, c, v) {
            if (c == "family_card_person") {
                checkForm();
                PostCardPerson();
            } else if (c == "assistpay_code") {
                PostChageFamily();
            }
        }
        function OnDsEducationItemChanged(s, r, c, v) {
            if (c == "child_level") {
                PostChageLevel();
            } else if (c == "child_gpa") {
                if (v > 4) { alert("ตรวจสอบเกรดเฉลี่ย"); dsEducation.SetItem(0, "child_gpa", 0); false; }
                PostChkGpa();
            } else if (c == "child_birth_date") {
                PostGetAgeChild();
            } else if (c == "child_card_person") {
                checkForm_child();
                PostCardPersonChild();
            } else if (c == "child_no_work") {
                PostCalChildNo();
            }
            else if (c == "child_no_study") {
                PostCalChildNo();
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
            PostInsertRow();
        }

        function OnDsPatronizeItemChanged(s, r, c, v) {
            if (c == "end_treat") {
                var ls_streat = dsPatronize.GetItem(0, "start_treat");
                var ls_end = dsPatronize.GetItem(0, "end_treat");
                if (ls_streat > ls_end) {
                    dsPatronize.SetItem(0, "end_treat", ls_streat);
                    alert("ตรวจสอบวันที่รักษา");
                }
                PostCalDateTreat();
            } else if (c == "start_treat") {
                var ls_req = dsPatronize.GetItem(0, "req_date"); 
                var ls_streat = dsPatronize.GetItem(0, "start_treat");
                var ls_end = dsPatronize.GetItem(0, "end_treat");
                if (ls_streat > ls_end) {
                    dsPatronize.SetItem(0, "start_treat", ls_end);
                    alert("ตรวจสอบวันที่รักษา");
                }
                PostCalDateTreat();
            } else if (c == "senile_type") {
                PostGenBaht();
            }
        }
        function OnDsDeceaseItemChanged(s, r, c, v) {
            if (c == "dead_date") {
                PostGenBaht2();
            }
        }

        function MenubarOpen() {
            //var memno = dsMain.GetItem(0, "member_no");
            //var assisttype_code = dsAssisttype.GetItem(0, "assisttype_code");
            //Gcoop.OpenIFrame2("800", "550", "ws_dlg_ass_edit.aspx", "?assisttype_code=" + assisttype_code+ "|" + memno);
            Gcoop.OpenIFrame2("800", "550", "ws_dlg_ass_edit.aspx", "" );
        }


        function GetValueFromDlg(assistdoc_no) {
            dsMain.SetItem(0, "assist_docno", assistdoc_no);
            //dsEducation.SetItem(0, "assist_docno", assistdoc_no);
            Gcoop.GetEl("hdCheckDsAmount").value = "chk2";
            PostEdit();
            
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
   <uc2:DsAssisttype ID="dsAssisttype" runat="server" />
    <br />

    <div id="tabs">
        <ul style="font-size:12px;">
    
            <li><a href="#tabs-1">ส่งเสริมการศึกษาบุตร</a></li>
            <li><a href="#tabs-2">สมาชิกถึงแก่กรรม</a></li>
            <li><a href="#tabs-3">สวัสดิการครอบครัว</a></li>
            <li><a href="#tabs-4">สมาชิกประสบภัยพิบัติ</a></li>           
            <li><a href="#tabs-5">สวัสดิการเกื้อกูลสมาชิก</a></li>  
            
        </ul>
        <div id="tabs-1">
            <uc4:DsEducation ID="dsEducation" runat="server" />          
        </div>
        <div id="tabs-2">
            <uc3:DsDecease ID="dsDecease" runat="server" />            
        </div>
        <div id="tabs-3">
            <uc8:DsFamilydecease ID="dsFamilydecease" runat="server" />            
        </div>
        <div id="tabs-4">
            <uc5:DsDisaster ID="dsDisaster" runat="server" />            
        </div>
        <div id="tabs-5">
            <uc6:DsPatronize ID="dsPatronize" runat="server" />            
        </div> 
    </div>  
    <br />
    <div align="right" style="margin-right:60px;" ><span id="insertRow" style="display:none" class="NewRowLink" onclick="PostInsertRow()">เพิ่มแถว</span></div>
    <uc10:DsGain ID="dsGain" runat="server" />
    <uc7:DsAmount ID="dsAmount" runat="server" />
    <uc9:DsList ID="dsList" runat="server" />
    <asp:HiddenField ID="hdTabIndex" Value="0" runat="server" />
    <asp:HiddenField ID="hdSaveChk_GPA" Value="0"  runat="server" />
    <asp:HiddenField ID="hdInertRow" runat="server" />
    <asp:HiddenField ID="hdCalDay" runat="server" />
    <asp:HiddenField ID="hdActMemno" runat="server" />
    <asp:HiddenField ID="hdCheckDsAmount" runat="server" />
</asp:Content>



