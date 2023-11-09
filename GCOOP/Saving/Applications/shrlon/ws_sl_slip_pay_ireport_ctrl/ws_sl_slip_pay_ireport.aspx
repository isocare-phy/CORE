<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="ws_sl_slip_pay_ireport.aspx.cs" Inherits="Saving.Applications.shrlon.ws_sl_slip_pay_ireport_ctrl.ws_sl_slip_pay_ireport" %>

<%@ Register Src="DsMain.ascx" TagName="DsMain" TagPrefix="uc1" %>
<%@ Register Src="DsDetailLoan.ascx" TagName="DsDetailLoan" TagPrefix="uc3" %>
<%@ Register Src="DsDetailEtc.ascx" TagName="DsDetailEtc" TagPrefix="uc4" %>
<%@ Register src="DsDetailShare.ascx" tagname="DsDetailShare" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="<%=state.SsUrl %>JsCss/AjaxCall.js" type="text/javascript"></script>
    <script type="text/javascript">
        var dsMain = new DataSourceTool;
        var dsDetailShare = new DataSourceTool;
        var dsDetailLoan = new DataSourceTool;
        var dsDetailEtc = new DataSourceTool;
        var ajax = new AjaxCall();

        function Validate() {
            var slipStatus = Gcoop.GetEl("HdSlipStatus").value;
            if (slipStatus == 1) {
                alert("รายการที่มีอยู่แล้ว ไม่สามารถบันทึกได้");
            }
            else {
                return confirm("ยืนยันการบันทึกข้อมูล");
            }
        }

        function OnDsMainItemChanged(s, r, c, v) {
            if (c == "member_no") {
                Gcoop.GetEl("HdSlipStatus").value = 0;
                //                var arg = 'memno=' + v + '&status=insert';
                //                ajax.Postback("jsajaxpostmember", arg , Postmember);
                PostMemberNo();

            } else if (c == "accid_flag") {
                PostAccidFlag();
            } else if (c == "operate_date") {
                PostOperateDate();
            } else if (c == "moneytype_code") {
                PostMoneytype();
            } else if (c == "ajaxpostmember") {

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
            Gcoop.GetEl("HdSlipStatus").value = 1;
            PostSearchRetrieve();
        }

        function GetValueFromDlg(member_no) {
            Gcoop.GetEl("HdStatusOpen").value = "new";
            dsMain.SetItem(0, "member_no", member_no);
            Gcoop.GetEl("HdSlipStatus").value = 0;
            PostMemberNo();
        }

        function OnDsMainClicked(s, r, c) {

            if (c == "b_memsearch") {
                Gcoop.OpenIFrame("630", "450", "w_dlg_sl_member_search_lite.aspx", "");

            }
            else if (c == "operate_date") {
                var slipStatus = Gcoop.GetEl("HdSlipStatus").value;
                if (slipStatus != 1) {
                    datePicker.PickDs(dsMain, r, c, PostOperateDate);
                }
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

        }

        function OnDsDetailShareItemChanged(s, r, c, v) {
            if (c == "operate_flag") {
                dsDetailShare.SetRowFocus(r);
                PostOperateFlag();

            } else if (c == "periodcount_flag") {
                dsDetailShare.SetRowFocus(r);
                var periodcount_flag = dsDetailShare.GetItem(r, "periodcount_flag");
                var period = dsDetailShare.GetItem(r, "period");
                var sum = 0;
                if (periodcount_flag == 1) {
                    sum = period + 1;
                    dsDetailShare.SetItem(r, "period", sum);

                } else if (periodcount_flag == 0) {
                    dsDetailShare.SetItem(r, "period", period - 1);
                }
            } else if (c == "item_payamt") {
                dsDetailShare.SetRowFocus(r);
                var bfshrcont_balamt = dsDetailShare.GetItem(r, "bfshrcont_balamt");
                var item_payamt = dsDetailShare.GetItem(r, "item_payamt");
                var total = bfshrcont_balamt + item_payamt;
                dsDetailShare.SetItem(r, "item_balance", total);

                cal();
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
                var periodcount_flag = dsDetailLoan.GetItem(r, "periodcount_flag");
                var period = dsDetailLoan.GetItem(r, "period");
                var sum = 0;
                if (periodcount_flag == 1) {
                    sum = period + 1;
                    dsDetailLoan.SetItem(r, "period", sum);

                } else if (periodcount_flag == 0) {
                    dsDetailLoan.SetItem(r, "period", period - 1);
                }
            }
            else if (c == "principal_payamt" || c == "interest_payamt") {
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

            else if (c == "item_payamt") {
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
            }
        }

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
                    alert("ยอดชำระเกินยอดคงเหลือ");
                    dsDetailEtc.SetItem(r, "item_payamt", bfshrcont_balamt);
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
        }
        function OnClickPrint() {
            PostPrint();
        }

        function OnClickAddRow() {
            var slipStatus = Gcoop.GetEl("HdSlipStatus").value;
            if (slipStatus != 1) {
                PostInsertRow();
            }
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
            Open_tabledsDetailShare();
            Open_tabledsDetailLoan();
            Open_tabledsDetailEtc();
        }

        //        function DisabledTableFormView1(namecheckbox, nameDw, findname) {
        //            var chk = $('#ctl00_ContentPlace_' + namecheckbox).is(':checked');
        //            if (findname == null || findname == '') {
        //                findname = '';
        //            } else {
        //                findname = ',' + findname;
        //            }
        //            var status;
        //            if (chk) {
        //                status = false;
        //            } else {
        //                status = true;
        //            }
        //            $('#ctl00_ContentPlace_' + nameDw + '_FormView1').find('input,select,button' + findname).attr('disabled', status)
        //        }

        function Open_tabledsDetailShare() {
            var chk = $('#ctl00_ContentPlace_dsDetailShare_chkdsDetailShare').is(':checked');
            var status;
            if (chk) {
                status = false;
            } else {
                status = true;
            }

            $('.DataSourceRepeater').eq(0).find('input,select,button').attr('disabled', status);

            if (status) {
                var count = dsDetailShare.GetRowCount();
                for (var i = 0; i < count; i++) {
                    dsDetailShare.SetItem(i, "operate_flag", 0);
                    dsDetailShare.SetItem(i, "item_payamt", 0);
                }
            }

        }

        function Open_tabledsDetailLoan() {

            var chk = $('#ctl00_ContentPlace_dsDetailLoan_chkdsDetailLoan').is(':checked');
            var status;
            if (chk) {
                status = false;
            } else {
                status = true;
            }
            $('.DataSourceRepeater').eq(1).find('input,select,button').attr('disabled', status);
            if (status) {
                var count = dsDetailLoan.GetRowCount();
                for (var i = 0; i < count; i++) {
                    dsDetailLoan.SetItem(i, "operate_flag", 0);
                    dsDetailLoan.SetItem(i, "principal_payamt", 0);
                    dsDetailLoan.SetItem(i, "interest_payamt", 0);
                    dsDetailLoan.SetItem(i, "item_payamt", 0);
                }
            }
        }

        function Open_tabledsDetailEtc() {
            var chk = $('#ctl00_ContentPlace_dsDetailEtc_chkDetailEtc').is(':checked');
            var status;
            if (chk) {
                status = false;
            } else {
                status = true;
            }
            $('.DataSourceRepeater').eq(2).find('input,select,button').attr('disabled', status);
            if (status) {
                var count = dsDetailEtc.GetRowCount();
                for (var i = 0; i < count; i++) {
                    dsDetailEtc.SetItem(i, "operate_flag", 0);
                    dsDetailEtc.SetItem(i, "slipitemtype_code", "");
                    dsDetailEtc.SetItem(i, "item_payamt", 0);
                    dsDetailEtc.SetItem(i, "cp_balance", 0);
                }
            }
        }           

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <div align="left">
        <input type="button" value="พิมพ์ใบเสร็จ" style="width: 80px" onclick="OnClickPrint()" />
    </div>
    <uc1:DsMain ID="dsMain" runat="server" />
    <br />
    <hr />

    <uc2:DsDetailShare ID="dsDetailShare" runat="server" />

    <br />
    <uc3:DsDetailLoan ID="dsDetailLoan" runat="server" />
    <br />
    <div align="right" style="margin-right: 1px; width: 765px;">
        <span class="NewRowLink" onclick="OnClickAddRow()" id="add_row" runat="server">เพิ่มแถว</span></div>
    <uc4:DsDetailEtc ID="dsDetailEtc" runat="server" />
    <asp:HiddenField ID="HdOpenReport" runat="server" />
    <asp:HiddenField ID="HdPayNo" runat="server" />
    <asp:HiddenField ID="HdSlipStatus" runat="server" />
    <asp:HiddenField ID="HdStatusOpen" runat="server" Value="new" />
</asp:Content>
