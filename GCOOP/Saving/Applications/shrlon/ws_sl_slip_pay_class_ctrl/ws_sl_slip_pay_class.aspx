<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="ws_sl_slip_pay_class.aspx.cs" Inherits="Saving.Applications.shrlon.ws_sl_slip_pay_class_ctrl.ws_sl_slip_pay_class" %>

<%@ Register Src="DsMain.ascx" TagName="DsMain" TagPrefix="uc1" %>
<%@ Register Src="DsDetailShare.ascx" TagName="DsDetailShare" TagPrefix="uc2" %>
<%@ Register Src="DsDetailLoan.ascx" TagName="DsDetailLoan" TagPrefix="uc3" %>
<%@ Register Src="DsDetailEtc.ascx" TagName="DsDetailEtc" TagPrefix="uc4" %>
<%@ Register Src="DsDetailAdjust.ascx" TagName="DsDetailAdjust" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="<%=state.SsUrl %>JsCss/AjaxCall.js" type="text/javascript"></script>
    <script type="text/javascript">
        var dsMain = new DataSourceTool;
        var dsDetailShare = new DataSourceTool;
        var dsDetailLoan = new DataSourceTool;
        var dsDetailEtc = new DataSourceTool;
        var dsDetailAdjust = new DataSourceTool;
        var ajax = new AjaxCall();
        function clearValue(obj, text) {
            if (obj.value == text) obj.value = '';
        }
        function checkValue(obj, text) {
            if (obj.value == '') obj.value = text;
        }
        function Validate() {
            var moneytype_code = dsMain.GetItem(0, "moneytype_code");
            var check = true;
            if (moneytype_code == "TRN") {
                var slip_amt = dsMain.GetItem(0, "slip_amt");
                var ref_slipamt = dsMain.GetItem(0, "ref_slipamt");
                slip_amt = parseFloat((slip_amt).toFixed(2));
                ref_slipamt = parseFloat((ref_slipamt).toFixed(2));
                if (slip_amt > ref_slipamt) {
                    check = false;
                    alert("ยอดชำระมากกว่ายอดจ่าย กรุณาตรวจสอบ");
                }
            }

            var dsDetailAdjustRow = 0;
            for (var i = 0; i < dsDetailAdjust.GetRowCount(); i++) {
                if (dsDetailAdjust.GetItem(i, "operate_flag") == 1) {
                    dsDetailAdjustRow = i;
                    break;
                }
            }
            var sum_etc = 0;
            for (var i = 0; i < dsDetailEtc.GetRowCount(); i++) {
                if (dsDetailEtc.GetItem(i, "operate_flag") == 1) {
                    sum_etc = sum_etc + dsDetailEtc.GetItem(i, "item_payamt");
                }
            }
            var sum_slip_amt = dsMain.GetItem(0, "slip_amt") - sum_etc;
            if (sum_slip_amt > dsDetailAdjust.GetItem(dsDetailAdjustRow, "slip_amt")) {
                check = false;
            }
            if (check == true) {
                return confirm("ยืนยันการบันทึกข้อมูล");
            }
        }

        function OnDsMainItemChanged(s, r, c, v) {
            if (c == "member_no") {
                PostMemberNo();
            } else if (c == "accid_flag") {
                PostAccidFlag();
            } else if (c == "operate_date") {
                PostOperateDate();
            } else if (c == "moneytype_code") {
                PostMoneytype();
            } else if (c == "ajaxpostmember") {

            } else if (c == "sliptype_code") {
                PostSliptypecode();
            }
        }
        function OnDsDetailLoanClicked(s, r, c) {
            if (c == "bloan_detail") {
                var ls_lcontno = dsDetailLoan.GetItem(r, "loancontract_no");
                Gcoop.OpenIFrame3("900", "600", "w_dlg_sl_detail_contract.aspx", "?lcontno=" + ls_lcontno);
            }
        }
        function OnDsDetailShareClicked(s, r, c) {
            if (c == "bshr_detail") {
                var ls_memno = dsMain.GetItem(0, "member_no");
                var ls_shr_typecode = "01";
                Gcoop.OpenIFrame3("800", "710", "w_dlg_sl_detail_share.aspx", "?memno=" + ls_memno + "&shrtype=" + ls_shr_typecode);
            }
        }
        function Postmember(text) {
            alert(text);
            var xml = new XmlDataSourceTool(text);
            alert(xml.GetItemString(0, "closeday_status"));
        }

        function MenubarOpen() {
            Gcoop.GetEl("HdStatusOpen").value = "open";
            Gcoop.OpenIFrame3("830", "650", "w_dlg_sl_search_slip.aspx", "");
        }

        function GetItemLoan(payinslip_no) {
            Gcoop.RemoveIFrame();
            Gcoop.GetEl("HdPayNo").value = payinslip_no;
            PostSearchRetrieve();
        }

        function GetValueFromDlg(member_no) {
            Gcoop.GetEl("HdStatusOpen").value = "new";
            dsMain.SetItem(0, "member_no", member_no);
            PostMemberNo();
        }

        function OnDsMainClicked(s, r, c) {

            if (c == "b_memsearch") {
                Gcoop.OpenIFrame("630", "450", "w_dlg_sl_member_search_lite.aspx", "");

            }
            else if (c == "operate_date") {
                datePicker.PickDs(dsMain, r, c, PostOperateDate);
            }
            else if (c == "b_ref") {
                var member_no = dsMain.GetItem(r, "member_no");
                var moneytype_code = dsMain.GetItem(r, "moneytype_code");
                if (moneytype_code == "TRN") {
                    Gcoop.OpenIFrame2("700", "450", "w_dlg_sl_receive_ref_slip.aspx", "?member_no=" + member_no);
                } else {
                    alert("กรุณาเลือกโอนภายในระบบ");
                }
            }
        }

        function GetRefSlipFromDialog(expense_accid, ref_system, ref_slipno, ref_slipamt) {
            dsMain.SetItem(0, "expense_accid", expense_accid);
            dsMain.SetItem(0, "ref_system", ref_system);
            dsMain.SetItem(0, "ref_slipno", ref_slipno);
            dsMain.SetItem(0, "ref_slipamt", ref_slipamt);
            dsMain.SetItem(0, "cp_refslip", ref_slipno + " ระบบ " + ref_system);
        }
        function OnDsDetailAdjustItemChanged(s, r, c, v) {
            if (c == "operate_flag") {
//                if (r == (dsDetailAdjust.GetRowCount() - 1)) {
                    dsDetailAdjust.SetRowFocus(r);
                    PostOperateFlagA();
//                } else {
//                    dsDetailAdjust.SetItem(r, "operate_flag", 0);
//                    alert("กรุณาเลือก เดือนที่ค้าง");
//                }
            }
        }

        function OnDsDetailShareItemChanged(s, r, c, v) {
            if (c == "operate_flag") {
                dsDetailShare.SetRowFocus(r);
                PostOperateFlag();

            } else if (c == "periodcount_flag") {
                dsDetailShare.SetRowFocus(r);
                var operate_flag = dsDetailShare.GetItem(r, "operate_flag");
                var periodcount_flag = dsDetailShare.GetItem(r, "periodcount_flag");
                var period = dsDetailShare.GetItem(r, "period");
                var bfperiod_payment = dsDetailShare.GetItem(r, "bfperiod_payment");
                var sum = 0;

                if (periodcount_flag == 1) {
                    sum = period + 1;
                    dsDetailShare.SetItem(r, "period", sum);
                    dsDetailShare.GetElement(r, "period").disabled = false;
                } else if (periodcount_flag == 0) {
                    dsDetailShare.SetItem(r, "period", period - 1);
                }
            } else if (c == "item_payamt") {
                dsDetailShare.SetRowFocus(r);
                var bfshrcont_balamt = dsDetailShare.GetItem(r, "bfshrcont_balamt");
                var item_payamt = dsDetailShare.GetItem(r, "item_payamt");
                var total = bfshrcont_balamt + item_payamt;
                var period = dsDetailShare.GetItem(r, "period");

                dsDetailShare.SetItem(r, "item_balance", total);
                cal();
                if (dsDetailShare.GetItem(r, "item_payamt") < dsDetailShare.GetItem(r, "principal_adjamt")) {
                    dsDetailShare.SetItem(r, "periodcount_flag", 0);
                } else {
                    dsDetailShare.SetItem(r, "periodcount_flag", 1);
                }
                var periodcount_flag = dsDetailShare.GetItem(r, "periodcount_flag");
                var period = dsDetailShare.GetItem(r, "period");
                var sum = 0;
                if (periodcount_flag == 1) {
                    sum = period + 1;
                    dsDetailShare.SetItem(r, "period", sum);
                    dsDetailShare.GetElement(r, "period").disabled = false;
                } else if (periodcount_flag == 0) {
                    dsDetailShare.SetItem(r, "period", period - 1);
                }
            }

            else if (c == "item_balance") {
                dsDetailShare.SetRowFocus(r);
                var item_balance = dsDetailShare.GetItem(r, "item_balance");
                var bfshrcont_balamt = dsDetailShare.GetItem(r, "bfshrcont_balamt");
                var total = item_balance - bfshrcont_balamt;
                dsDetailShare.SetItem(r, "item_payamt", total);
                cal();
            }
        }

        function OnDsDetailLoanItemChanged(s, r, c, v) {
            if (c == "operate_flag") {
                dsDetailLoan.SetRowFocus(r);
                PostOperateFlagL();
            } else if (c == "periodcount_flag") {
                dsDetailLoan.SetRowFocus(r);
                var operate_flag = dsDetailLoan.GetItem(r, "operate_flag");
                var periodcount_flag = dsDetailLoan.GetItem(r, "periodcount_flag");
                var period = dsDetailLoan.GetItem(r, "period");
                var bfshrcont_balamt = dsDetailLoan.GetItem(r, "bfshrcont_balamt");
                var bfperiod_payment = dsDetailLoan.GetItem(r, "bfperiod_payment");
                var cp_interestpay = dsDetailLoan.GetItem(r, "cp_interestpay");
                var sum = 0;
                if (periodcount_flag == 1) {
                    sum = period + 1;
                    dsDetailLoan.SetItem(r, "period", sum);

                } else if (periodcount_flag == 0) {
                    dsDetailLoan.SetItem(r, "period", period - 1);
                }
            }
            else if (c == "principal_payamt") {
                dsDetailLoan.SetRowFocus(r);
                var bfpayspec_method = dsDetailLoan.GetItem(r, "bfpayspec_method");
                if (bfpayspec_method == 2) {
                    PostPayspecMethod();
                } else {
                    ReCalculate(r);
                }
            } else if (c == "interest_payamt") {
                dsDetailLoan.SetRowFocus(r);
                ReCalculate(r);
            }

            else if (c == "item_payamt") {
                var bfpayspec_method = dsDetailLoan.GetItem(r, "bfpayspec_method");
                var rkeep_int = dsDetailLoan.GetItem(r, "rkeep_interest");
                var rkeep_prin = dsDetailLoan.GetItem(r, "rkeep_principal");
                var aftermont = dsDetailLoan.GetItem(r, "bfpxaftermthkeep_type");
                var item_payamt = dsDetailLoan.GetItem(r, "item_payamt");
                var principal_adjamt = dsDetailLoan.GetItem(r, "principal_adjamt");
                var interest_adjamt = dsDetailLoan.GetItem(r, "interest_adjamt");
                if (item_payamt > (principal_adjamt + interest_adjamt)) {
                    alert("ยอดชำระเกินว่างที่ค้างชำระไว้");
                    dsDetailLoan.SetItem(r, "item_payamt", (principal_adjamt + interest_adjamt));
                }
                if (!((rkeep_int > 0 || rkeep_prin > 0) && aftermont == 1) && bfpayspec_method == 2) {
                    PostRePayspecMethod();
                }
                else if (bfpayspec_method == 2 && (rkeep_int == 0 && rkeep_prin == 0)) {
                    PostRePayspecMethod();
                }
                else {
                    dsDetailLoan.SetRowFocus(r);
                    var item_payamt = dsDetailLoan.GetItem(r, "item_payamt");
                    var principal_payamt = dsDetailLoan.GetItem(r, "principal_payamt");
                    var interest_payamt = dsDetailLoan.GetItem(r, "interest_payamt");
                    var bfshrcont_balamt = dsDetailLoan.GetItem(r, "bfshrcont_balamt");
                    var cp_interestpay = dsDetailLoan.GetItem(r, "cp_interestpay");
                    var principalPayamt = item_payamt - interest_payamt;
                    var total_itembal = 0;
                    if (item_payamt <= interest_payamt) {
                        dsDetailLoan.SetItem(r, "interest_payamt", item_payamt);
                        dsDetailLoan.SetItem(r, "principal_payamt", 0);
                        total_itembal = bfshrcont_balamt;
                    } else {
                        if (principalPayamt > bfshrcont_balamt) {
                            var over = item_payamt - (principal_payamt + cp_interestpay);
                            over = numberWithCommas(over, 2);
                            var text_alert = "ชำระเงิน " + numberWithCommas(item_payamt, 2) + " บาท\nเกินไป " + over + " บาท";
                            alert(text_alert);

                            dsDetailLoan.SetItem(r, "principal_payamt", principal_payamt);
                            dsDetailLoan.SetItem(r, "interest_payamt", interest_payamt);
                            dsDetailLoan.SetItem(r, "item_payamt", principal_payamt + interest_payamt);
                        } else {
                            dsDetailLoan.SetItem(r, "principal_payamt", principalPayamt);
                        }
                        total_itembal = bfshrcont_balamt - principalPayamt;
                    }
                    //var total_itembal = bfshrcont_balamt - principal_payamt;

                    dsDetailLoan.SetItem(r, "item_balance", total_itembal);
                    cal();
                    if (dsDetailLoan.GetItem(r, "item_payamt") < (dsDetailLoan.GetItem(r, "principal_adjamt") + dsDetailLoan.GetItem(r, "interest_adjamt"))) {
                        dsDetailLoan.SetItem(r, "periodcount_flag", 0);
                    } else {
                        dsDetailLoan.SetItem(r, "periodcount_flag", 1);
                    }
                    var periodcount_flag = dsDetailLoan.GetItem(r, "periodcount_flag");
                    var period = dsDetailLoan.GetItem(r, "period");
                    var sum = 0;
                    if (periodcount_flag == 1) {
                        sum = period + 1;
                        dsDetailLoan.SetItem(r, "period", sum);
                        dsDetailLoan.GetElement(r, "period").disabled = false;
                    } else if (periodcount_flag == 0) {
                        dsDetailLoan.SetItem(r, "period", period - 1);
                    }
                }
            }
        }
        function ReCalculate(r) {
            dsDetailLoan.SetRowFocus(r);
            var principal_payamt = dsDetailLoan.GetItem(r, "principal_payamt");
            var interest_payamt = dsDetailLoan.GetItem(r, "interest_payamt");
            var bfshrcont_balamt = dsDetailLoan.GetItem(r, "bfshrcont_balamt");

            if (principal_payamt > bfshrcont_balamt) {
                dsDetailLoan.SetItem(r, "principal_payamt", bfshrcont_balamt);
                principal_payamt = bfshrcont_balamt;
                alert("ชำระต้นเงินเกินยอดคงเหลือ");
            }

            var total = principal_payamt + interest_payamt;
            var total_itembal = bfshrcont_balamt - principal_payamt;
            dsDetailLoan.SetItem(r, "item_payamt", total);
            dsDetailLoan.SetItem(r, "item_balance", total_itembal);

            cal();
        }

        Number.prototype.format = function (n, x, s, c) {
            var re = '\\d(?=(\\d{' + (x || 3) + '})+' + (n > 0 ? '\\D' : '$') + ')',
        num = this.toFixed(Math.max(0, ~ ~n));

            return (c ? num.replace('.', c) : num).replace(new RegExp(re, 'g'), '$&' + (s || ','));
        };

        function OnDsDetailEtcItemChanged(s, r, c, v) {
            if (c == "operate_flag") {
                dsDetailEtc.SetRowFocus(r);
                PostOperateFlagE();

            } else if (c == "slipitemtype_code") {
                dsDetailEtc.SetRowFocus(r);
                PostSlipItem();
            }

            else if (c == "item_payamt") {
                dsDetailEtc.SetRowFocus(r);
                var item = dsDetailEtc.GetItem(r, "item_payamt");
                var bfshrcont_balamt = dsDetailEtc.GetItem(r, "bfshrcont_balamt");
                dsDetailEtc.SetItem(r, "item_payamt", v);
                var bal = bfshrcont_balamt - item;
                if (bal < 0) {
                    item = bfshrcont_balamt;
                    bal = bfshrcont_balamt - item;
                }

                if (dsDetailEtc.GetItem(r, "item_payamt") > 0) {
                    dsDetailEtc.SetItem(r, "operate_flag", 1);
                }
                dsDetailEtc.SetItem(r, "cp_balance", bal);
                cal();
            }
        }

        function OnDsDetailEtcClicked(s, r, c) {
            if (c == "b_del") {
                dsDetailEtc.SetRowFocus(r);
                PostDeleteRow();
            }
        }

        function cal() {
            sum_total = 0;
            for (var i = 0; i < dsDetailShare.GetRowCount(); i++) {
                sum_total = sum_total + dsDetailShare.GetItem(i, "item_payamt");
            }
            for (var i = 0; i < dsDetailLoan.GetRowCount(); i++) {
                sum_total = sum_total + dsDetailLoan.GetItem(i, "item_payamt");
            }
            for (var i = 0; i < dsDetailEtc.GetRowCount(); i++) {
                sum_total = sum_total + dsDetailEtc.GetItem(i, "item_payamt");
            }
            dsMain.SetItem(0, "slip_amt", sum_total);
            var dsDetailAdjustRow = 0;
            for (var ii = 0; ii < dsDetailAdjust.GetRowCount(); ii++) {
                if (dsDetailAdjust.GetItem(ii, "operate_flag") == 1) {
                    dsDetailAdjustRow = ii;
                    break;
                }
            }
            dsDetailAdjust.SetItem(dsDetailAdjustRow, "sum_slip_amt", sum_total);
        }
        function numberWithCommas(x, fixed) {
            x = x.toFixed(fixed);
            x = x.toString();
            var pattern = /(-?\d+)(\d{3})/;
            while (pattern.test(x))
                x = x.replace(pattern, "$1,$2");
            return x;
        }

        function SheetLoadComplete() {

            var StatusOpen = Gcoop.GetEl("HdStatusOpen").value;
            if (StatusOpen == "open") {
                Open_All();
            } else if (StatusOpen == "new") {
                disabletable_all();
            }
            $("ctl00_ContentPlace_dsMain_FormView1 input[name='ctl00$ContentPlace$dsMain$FormView1$slip_amt']").attr('disabled', true);
            var coop_id = Gcoop.GetEl("HdCoopControl").value;
            //alert(coop_id);
            //PostFlagLon();
            /////////////////////////////////////////////////////////////////////////////////////////////////
            var Adjust_count = 0;
            var Share_count = 0;
            var Loan_count = 0;
            var Etc_count = 0;
            if (dsDetailAdjust.GetRowCount() != 0) {
                for (var i = 0; i < dsDetailAdjust.GetRowCount(); i++) {
                    Adjust_count = Adjust_count + dsDetailAdjust.GetItem(i, "operate_flag");
                }
                for (var i = 0; i < dsDetailShare.GetRowCount(); i++) {
                    Share_count = Share_count + dsDetailShare.GetItem(i, "operate_flag");
                }
                for (var i = 0; i < dsDetailLoan.GetRowCount(); i++) {
                    Loan_count = Loan_count + dsDetailLoan.GetItem(i, "operate_flag");
                }
                for (var i = 0; i < dsDetailEtc.GetRowCount(); i++) {
                    Etc_count = Etc_count + dsDetailEtc.GetItem(i, "operate_flag");
                }
                if (Share_count != 0 || Loan_count != 0 || Etc_count != 0) {

                } else if (Adjust_count == 0) {

                }
            }
            /////////////////////////////////////////////////////////////////////////////////////////////////
        }

        function Open_All() {
            //FormView1
            $('#ctl00_ContentPlace_dsMain_FormView1').find('input,select,button').attr('disabled', true);
            // dsDetailShare
            $('#ctl00_ContentPlace_dsDetailShare_chkdsDetailShare').attr('disabled', true);
            $('.DataSourceRepeater').eq(0).find('input,select,button').attr('disabled', true);
            //dsDetailLoan
            $('#ctl00_ContentPlace_dsDetailLoan_chkdsDetailLoan').attr('disabled', true);
            $('.DataSourceRepeater').eq(1).find('input,select,button').attr('disabled', true);
            //dsDetailEtc
            $('#ctl00_ContentPlace_dsDetailEtc_chkDetailEtc').attr('disabled', true);
            $('.DataSourceRepeater').eq(2).find('input,select,button').attr('disabled', true);
        }

        function disabletable_all() {
            //Open_tabledsDetailShare();
            //Open_tabledsDetailLoan();
            //Open_tabledsDetailEtc();
        }
        function OnClickAddRow() {
            PostInsertRow();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <uc1:DsMain ID="dsMain" runat="server" />
    <br />
    <center>
        <uc5:DsDetailAdjust ID="dsDetailAdjust" runat="server" />
    </center>
    <br />
    <hr />
    <div id="p1">
        <uc2:DsDetailShare ID="dsDetailShare" runat="server" />
        <br />
        <uc3:DsDetailLoan ID="dsDetailLoan" runat="server" />
        <br />
        <div align="right" style="margin-right: 1px; width: 765px;">
            <span class="NewRowLink" onclick="OnClickAddRow()" id="add_row" runat="server">เพิ่มแถว</span></div>
        <uc4:DsDetailEtc ID="dsDetailEtc" runat="server" />
        <br />
        <asp:HiddenField ID="HdOpenReport" runat="server" />
        <asp:HiddenField ID="HdPayNo" runat="server" />
        <asp:HiddenField ID="HdStatusOpen" runat="server" Value="new" />
        <asp:HiddenField ID="HdCoopControl" runat="server" />
        <asp:HiddenField ID="HdMaxshareHold" runat="server" />
        <asp:HiddenField ID="HdRow" runat="server" />
        <asp:HiddenField ID="HdAdjust" runat="server" />
        <asp:HiddenField ID="HdLoancontract" runat="server" />
    </div>
</asp:Content>
