<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_td_opr_iv_test2.aspx.cs" Inherits="Saving.Applications.trading.w_sheet_td_opr_iv_test2" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript %>
    <%=jsDebtNo%>
    <%=jsProductNo %>
    <%=jsPostSlipNo %>
    <%=jsDiscPercentChange %>
    <%=jsDiscAmtChange %>
    <%=jsDiscountTailer %>
    <%=jsRefresh %>
    <%=jsDwDetailInsertRow %>
    <%=jsItemQtyChange %>
    <%=jsTaxCodeChange %>
    <%=jsSlipNetAmtTailer%>
    <%=jsTaxoptChange %>
    <%=jsTaxCodeChangeTailer %>
    <%=jsinitValues %>
    <%=jsBSlipNo %>
    <%=jsBSlipDate%>
    <%=jsTransportfeeChange%>
    <%=jstaxratChange%>
    <%=jsprint%>
    <%=jsPostAccountNo%>
    <script type="text/javascript">
        var ajax = new AjaxCall();
        $(function () {
            $('input[name="slip_tdate_0"]').keyup(function () {
                ActiveChangeFormatDate('input[name="slip_tdate_0"]');
            });

            $('input[name="operate_tdate_0"]').keyup(function () {
                ActiveChangeFormatDate('input[name="operate_tdate_0"]');
            });

        });

        function Validate() { // บันทึก
            return confirm("ต้องการบันทึกหรือไม่");
        }

        function OnDwMainItemChanged(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            switch (c) {
                case "debt_no":
                    jsDebtNo();
                    break;
                case "refdoc_no":
                    jsBSlipNo();
                    break;
                case "slip_tdate":
                    //jsBSlipDate();
                    SlipDateChange();
                    break;
                case "credit_term":
                    //sBSlipDate();
                    SlipDateChange();
                    break;
                case "paymentby":
                    PayEnable();
                    jsDiscountTailer();
                    break;
                case "tdstockslip_account_no":
                    jsPostAccountNo();
                    break;
                case "tdstockslip_pay_amt":
                    pay_();
                    break;

            }
        }

        function validate_slipnet_amt() {
            var countDwDetail = objDwDetail.RowCount();
            if (countDwDetail == 0) {
                return;
            }
            var sumDwDetail = 0;
            for (var i = 1; i <= countDwDetail; i++) {
                sumDwDetail += getObjFloat(objDwDetail, i, "net_amt");
            }
            sumDwDetail = NumberRound(sumDwDetail, 2);
            var sumDwTailer = NumberRound(getObjFloat(objDwTailer, 1, "slipnet_amt"), 2);
            if (sumDwDetail > sumDwTailer) {  // บนมากว่า
                var tmp = NumberRound(sumDwDetail - sumDwTailer, 2);
                objDwTailer.SetItem(1, "slipnet_amt", sumDwDetail);
                var tax_amt = NumberRound(getObjFloat(objDwTailer, 1, "tax_amt") + tmp, 2);
                objDwTailer.SetItem(1, "tax_amt", tax_amt);
            } else if (sumDwTailer > sumDwDetail) {  // ล่างมากกว่า
                var tmp = NumberRound(sumDwTailer - sumDwDetail, 2);
                objDwTailer.SetItem(1, "slipnet_amt", sumDwDetail);
                var tax_amt = NumberRound(getObjFloat(objDwTailer, 1, "tax_amt") - tmp, 2);
                objDwTailer.SetItem(1, "tax_amt", tax_amt);
            }
            objDwTailer.AcceptText();
        }

        function PayEnable() {
            try {
                var paymentby = getObjString(objDwMain, 1, "paymentby");
                if (paymentby == "CST") {

                    $('select[name="tdstockslip_account_no_0"]').prop('disabled', false);
                }
                else {
                    $('select[name="tdstockslip_account_no_0"]').prop('disabled', true);
                    objDwMain.SetItem(1, "tdstockslip_account_no", "");
                    objDwMain.SetItem(1, "tdstockslip_bank_branch", "");
                    objDwMain.SetItem(1, "tdstockslip_bank_code", "");
                }
            }
            catch (Error) { }
            objDwMain.AcceptText();
        }

        function SlipDateChange() {
            var CreditTerm = getObjFloat(objDwMain, 1, "credit_term");
            var SlipDate = objDwMain.GetItem(1, "slip_tdate");
            var due_tdate = addDate(SlipDate, CreditTerm);
            objDwMain.SetItem(1, "due_tdate", due_tdate);
            objDwMain.AcceptText();
        }

        function addDate(date1, countday) {
            var tmp_date = parseInt(date1.substring(4, 8)) - 543 + "-" + date1.substring(2, 4) + "-" + date1.substring(0, 2);
            var d = new Date(tmp_date);
            d.setDate(d.getDate() + countday);
            var res = addZero2digit(d.getDate()) + addZero2digit(d.getMonth() + 1) + (d.getFullYear() + 543).toString();
            return res;
        }

        function addZero2digit(number) {
            number = parseInt(number);
            if (number > 0 && number < 10) {
                return "0" + number.toString();
            }
            return number.toString();
        }

        function DwDetailInsertRow() {
            jsDwDetailInsertRow();
        }

        function GetDlgValue(SOs, Products) {
            Gcoop.GetEl("HdQTsResult").value = SOs;
            Gcoop.GetEl("HdProducts").value = Products;
            jsinitValues();
        }

        function MenubarOpen() {
            Gcoop.OpenIFrame("770", "600", "w_dlg_td_search_slip.aspx", "?sliptype_code=IV");
        }

        function GetDlgSlipNo(slip_no) { //รับค่าสลิป จาก ไดอะล็อค
            objDwMain.SetItem(1, "slip_no", slip_no);
            jsPostSlipNo();
        }

        function OnDwMainButtonClick(s, r, c) {
            switch (c) {
                case "b_1":
                    Gcoop.OpenIFrame("740", "600", "w_dlg_td_search_cust.aspx", "");
                    break;
                case "b_2":
                    var debt_no = ""
                    try {
                        debt_no = objDwMain.GetItem(1, "debt_no");
                    } catch (err) { }
                    Gcoop.OpenIFrame("900", "600", "w_dlg_td_search_value_cust.aspx", "?sliptype_code=PO&debt_no=" + debt_no);
                    break;
                case "print_t":
                    jsprint();
                    break;
            }
        }

        function GetDlgDebtNo(debt_no) {
            objDwMain.SetItem(1, "debt_no", debt_no);
            jsDebtNo();
        }

        function GetDlgBSlipNo(slip_no) {
            objDwMain.SetItem(1, "refdoc_no", slip_no);
            jsBSlipNo();
        }

        function OnDwDetailItemChanged(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            Gcoop.GetEl("HdRowDetail").value = r + "";
            //            //เพิ่ม
            //            objDwMain.SetItem(1, "product_price", v);
            //            if (v > 0)
            //            { jsDiscPercentChange(); }

            switch (c) {
                case "disc_percent":
                    //jsDiscPercentChange();
                    DiscPercentChange(r);
                    TaxCodeChange(r);
                    TaxoptChange(1);
                    break;
                case "disc_amt":
                    //jsDiscAmtChange();
                    DiscAmtChange(r);
                    TaxCodeChange(r);
                    TaxoptChange(1);
                    break;
                case "product_price":
                    //jsDiscPercentChange();
                    DiscPercentChange(r);
                    TaxCodeChange(r);
                    TaxoptChange(1);
                    break;
                case "item_qty":
                    //jsItemQtyChange();
                    jsAjaxItemQty(s, r, c, v);
                    break;
                case "taxtype_code":
                    //jsTaxCodeChange();
                    TaxCodeChange(r);
                    TaxoptChange(1);
                    break;
                case "product_no":
                    jsAjaxProduct(s, r, c, v);
                    // jsProductNo();
                    break;
            }
        }

        function jsAjaxItemQty(s, r, c, v) {

            var debt_no = getObjString(objDwMain, 1, "debt_no");
            var price_level = getObjString(objDwMain, 1, "price_level");
            var slip_tdate = getObjString(objDwMain, 1, "slip_tdate");
            slip_tdate = slip_tdate.substring(0, 2) + "/" + slip_tdate.substring(2, 4) + "/" + slip_tdate.substring(4, 8);
            var product_no = getObjString(objDwDetail, r, "product_no");
            var item_qty = getObjFloat(objDwDetail, r, "item_qty");
            var arg = 'debt_no=' + debt_no + "&price_level=" + price_level + "&product_no=" + product_no + "&item_qty=" + item_qty + "&slip_tdate=" + slip_tdate;
            Gcoop.GetEl("HdRowDwDetail").value = r + "";
            ajax.Postback("ajaxinitItemQty", arg, initItemQty);
        }
        function initItemQty(text) {

            var row = Number(Gcoop.GetEl("HdRowDwDetail").value);
            var xml = new XmlDataSourceTool(text);
            var price = xml.GetItemNumber(1, "price");
            var disc_percent = xml.GetItemString(1, "disc_percent");
            var disc_amt = xml.GetItemNumber(1, "disc_amt");

            var item_qty = getObjFloat(objDwDetail, row, "item_qty");
            objDwDetail.SetItem(row, "product_price", price);
            if (disc_percent != "") {
                objDwDetail.SetItem(row, "disc_percent", disc_percent);
            }
            else {
                if (disc_amt != 0) {
                    objDwDetail.SetItem(row, "disc_amt", disc_amt * item_qty);
                }
            }
            objDwDetail.AcceptText();
            CalAmt2(item_qty, price, disc_percent, disc_amt, row);
            TaxCodeChange(row);
            TaxoptChange(1);
        }

        function jsAjaxProduct(s, r, c, v) {
            var product_no = s.GetItem(r, "product_no");
            var arg = 'product_no=' + product_no;
            Gcoop.GetEl("HdRowDwDetail").value = r + "";
            ajax.Postback("ajaxinitDetail", arg, initRowDwDetail);
        }

        function initRowDwDetail(text_xml) {
            var s = objDwDetail;
            var r = Number(Gcoop.GetEl("HdRowDwDetail").value);
            var xml = new XmlDataSourceTool(text_xml);
            s.SetItem(r, "coop_id", xml.GetItemString(1, "coop_id"));
            s.SetItem(r, "sliptype_code", xml.GetItemString(1, "sliptype_code"));
            s.SetItem(r, "slip_no", xml.GetItemString(1, "slip_no"));
            s.SetItem(r, "seq_no", xml.GetItemString(1, "seq_no"));
            s.SetItem(r, "refdoc_no", xml.GetItemString(1, "refdoc_no"));
            s.SetItem(r, "product_no", xml.GetItemString(1, "product_no"));
            s.SetItem(r, "item_qty", xml.GetItemString(1, "item_qty"));
            s.SetItem(r, "product_price", xml.GetItemString(1, "product_price"));
            s.SetItem(r, "unit_code", xml.GetItemString(1, "unit_code"));
            s.SetItem(r, "disc_percent", xml.GetItemString(1, "disc_percent"));
            s.SetItem(r, "disc_amt", xml.GetItemString(1, "disc_amt"));
            s.SetItem(r, "amtbefortax", xml.GetItemString(1, "amtbefortax"));
            s.SetItem(r, "taxtype_code", xml.GetItemString(1, "taxtype_code"));
            s.SetItem(r, "tax_rate", xml.GetItemString(1, "tax_rate"));
            s.SetItem(r, "net_amt", xml.GetItemString(1, "net_amt"));
            s.SetItem(r, "productbranch_id", xml.GetItemString(1, "productbranch_id"));
            s.SetItem(r, "product_desc", xml.GetItemString(1, ""));
            s.SetItem(r, "old_qty", xml.GetItemString(1, "old_qty"));
            s.SetItem(r, "balance_qty", xml.GetItemString(1, "balance_qty"));
            s.SetItem(r, "store_id", xml.GetItemString(1, "store_id"));
            s.AcceptText();
        }

        function DiscPercentChange(row) {
            try {
                //int row = Convert.ToInt32(HdRowDetail.Value);
                var item_qty = 0, product_price = 0, disc_amt = 0;
                var disc_percent = "";
                try {
                    item_qty = parseFloat(objDwDetail.GetItem(row, "item_qty"));
                    product_price = parseFloat(objDwDetail.GetItem(row, "product_price"));
                }
                catch (Error) {
                    alert("กรุณากรอกจำนวน และราคาต่อหน่วย");
                }
                try {
                    disc_percent = objDwDetail.GetItem(row, "disc_percent");
                }
                catch (Error) { }
                try {
                    disc_amt = parseFloat(objDwDetail.GetItem(row, "disc_amt"));
                }
                catch (Error) { }
                CalAmt2(item_qty, product_price, disc_percent, disc_amt, row);
            }
            catch (Error) { }
        }

        function TaxCodeChange(row) {
            try {
                var item_qty;
                try {
                    item_qty = parseFloat(objDwDetail.GetItem(row, "item_qty"));
                }
                catch (Error) {
                    alert("กรุณากรอก จำนวนสินค้า แถวที่ " + row);
                }
                var product_price = getObjFloat(objDwDetail, row, "product_price");
                var amount = (item_qty * product_price);
                var disc_amt = getObjFloat(objDwDetail, row, "disc_amt");
                var amtbefortax = amount - disc_amt;
                objDwDetail.SetItem(row, "amtbefortax", amtbefortax);
                if (row == 1) {
                    $("#objDwDetail_datawindow input[name='amtbefortax_0']").val(addCommas(amtbefortax, 2))
                }

                var taxtype_code = getObjString(objDwDetail, row, "taxtype_code");
                var tax_rate = getObjFloat(objDwDetail, row, "tax_rate");

                switch (taxtype_code) {
                    case "I":
                        var amount2 = Calamtbefortax_taxtype_code(row);
                        var amtbefortax2 = amount2 - disc_amt;

                        if (row == 1) {
                            $("#objDwDetail_datawindow input[name='amtbefortax_0']").val(addCommas(amtbefortax2, 2))
                        }
                        objDwDetail.SetItem(row, "amtbefortax", amtbefortax2);
                        objDwDetail.SetItem(row, "net_amt", amtbefortax);
                        break;
                    case "E":
                        objDwDetail.SetItem(row, "net_amt", amtbefortax + (amtbefortax * (tax_rate / 100)));
                        break;
                    case "X":
                        objDwDetail.SetItem(row, "net_amt", amtbefortax);
                        break;
                }


            }
            catch (Error) { }
            objDwDetail.AcceptText();
        }

        function Calamtbefortax_taxtype_code(row) {
            var item_qty = getObjFloat(objDwDetail, row, "item_qty");
            var product_price = getObjFloat(objDwDetail, row, "product_price");
            var amount = (item_qty * product_price);
            var taxtype_code = getObjString(objDwDetail, row, "taxtype_code");
            if (taxtype_code == "I") {
                var tax_rate = getObjFloat(objDwDetail, row, "tax_rate");
                product_price = (product_price - (product_price * tax_rate / (100 + tax_rate)))
                amount = NumberRound(item_qty * product_price, 2);
            }
            return amount;
        }

        function TaxoptChange(row) {
            try {
                var taxtype_code = "";
                var taxopt = "N";
                try {
                    taxopt = objDwTailer.GetItem(1, "taxopt");
                }
                catch (Error) { }

                var tax_amt = 0, amtbefortax = 0, slip_amt = 0, disc_amt = 0, amt = 0;

                var transportfee = getObjFloat(objDwTailer, 1, "transportfee");

                switch (taxopt) {
                    case "Y":
                        initcolor_from_taxopt(taxopt);
                        for (var i = 1; i <= objDwDetail.RowCount(); i++) {
                            try {
                                taxtype_code = objDwDetail.GetItem(i, "taxtype_code");
                            }
                            catch (Error) {
                                taxtype_code = "";
                            }
                            var tax_rate = getObjFloat(objDwTailer, 1, "tax_rate");
                            switch (taxtype_code) {
                                case "I":
                                    slip_amt += getObjFloat(objDwDetail, i, "net_amt");
                                    amt += getObjFloat(objDwDetail, i, "amtbefortax");
                                    //                                    tax_amt += (getObjFloat(objDwDetail, i, "net_amt")) - (getObjFloat(objDwDetail, i, "amtbefortax"));
                                    slip_amt -= (getObjFloat(objDwDetail, i, "net_amt")) - (getObjFloat(objDwDetail, i, "amtbefortax"));
                                    break;
                                case "E":
                                    slip_amt += getObjFloat(objDwDetail, i, "amtbefortax");
                                    amt += getObjFloat(objDwDetail, i, "amtbefortax");
                                    //tax_amt += (getObjFloat(objDwDetail, i, "net_amt")) - (getObjFloat(objDwDetail, i, "amtbefortax"));
                                    break;
                                case "X":
                                    slip_amt += getObjFloat(objDwDetail, i, "amtbefortax");
                                    amt += getObjFloat(objDwDetail, i, "amtbefortax");
                                    break;
                            }
                        }


                        disc_amt = getObjFloat(objDwTailer, 1, "disc_amt");
                        objDwTailer.SetItem(1, "slip_amt", amt);
                        objDwTailer.SetItem(1, "amtbefortax", slip_amt - disc_amt);
                        if (row == 1) {
                            $("#objDwTailer_datawindow input[name='amtbefortax_0']").val(addCommas(slip_amt - disc_amt, 2));
                        }

                        amtbefortax = slip_amt - disc_amt;

                        var tax_rate = getObjFloat(objDwTailer, 1, "tax_rate");
                        amtbefortax = getObjFloat(objDwTailer, 1, "amtbefortax");
                        tax_amt = ((amtbefortax * tax_rate) / 100);
                        break;


                    case "N":
                        initcolor_from_taxopt(taxopt);
                        for (var i = 1; i <= objDwDetail.RowCount(); i++) {
                            try { objDwDetail.SetItem(i, "taxtype_code", objDwTailer.GetItem(1, "taxtype_code")); }
                            catch (Error) { }
                            //HdRowDetail.Value = Convert.ToString(i);
                            if (i == 1) {
                                $("#objDwDetail_datawindow select[name='taxtype_code_0']").val(objDwTailer.GetItem(1, "taxtype_code"));
                            }
                            TaxCodeChange(i);
                        }

                        var tax_rate = getObjFloat(objDwTailer, 1, "tax_rate");
                        taxtype_code = getObjString(objDwTailer, 1, "taxtype_code");

                        switch (taxtype_code) {
                            case "I":
                                for (var i = 1; i <= objDwDetail.RowCount(); i++) {
                                    slip_amt += getObjFloat(objDwDetail, i, "amtbefortax");
                                }
                                objDwTailer.SetItem(1, "slip_amt", slip_amt);
                                disc_amt = getObjFloat(objDwTailer, 1, "disc_amt");
                                if (row == 1) {
                                    $("#objDwTailer_datawindow input[name='amtbefortax_0']").val(addCommas(slip_amt - disc_amt, 2));
                                }
                                objDwTailer.SetItem(1, "amtbefortax", slip_amt - disc_amt);
                                amtbefortax = slip_amt - disc_amt;
                                tax_amt = ((amtbefortax * tax_rate) / 100);
                                break;

                            case "E":
                                for (var i = 1; i <= objDwDetail.RowCount(); i++) {
                                    slip_amt += getObjFloat(objDwDetail, i, "amtbefortax");
                                }
                                objDwTailer.SetItem(1, "slip_amt", slip_amt);
                                disc_amt = getObjFloat(objDwTailer, 1, "disc_amt");
                                if (row == 1) {
                                    $("#objDwTailer_datawindow input[name='amtbefortax_0']").val(addCommas(slip_amt - disc_amt, 2));
                                }
                                objDwTailer.SetItem(1, "amtbefortax", slip_amt - disc_amt);
                                amtbefortax = slip_amt - disc_amt;
                                tax_amt = ((amtbefortax * tax_rate) / 100);
                                break;
                            case "X":
                                for (var i = 1; i <= objDwDetail.RowCount(); i++) {
                                    slip_amt += getObjFloat(objDwDetail, i, "amtbefortax");
                                }
                                objDwTailer.SetItem(1, "slip_amt", slip_amt);
                                disc_amt = getObjFloat(objDwTailer, 1, "disc_amt");
                                if (row == 1) {
                                    $("#objDwTailer_datawindow input[name='amtbefortax_0']").val(addCommas(slip_amt - disc_amt, 2));
                                }
                                objDwTailer.SetItem(1, "amtbefortax", slip_amt - disc_amt);
                                amtbefortax = slip_amt - disc_amt;
                                tax_amt = 0;
                                break;
                        }

                        break;
                }
                tax_amt = NumberRound(tax_amt, 2);
                objDwMain.SetItem(1, "tax_amt", tax_amt);
                objDwTailer.SetItem(1, "tax_amt", tax_amt);

                var sum_ = 0;
                sum_ = amtbefortax + tax_amt + transportfee;
                sum_ = NumberRound(sum_, 2);

                objDwTailer.SetItem(1, "slipnet_amt", sum_);
                //add
                try {
                    var slipnet_amt = getObjFloat(objDwTailer, 1, "slipnet_amt");
                    objDwMain.SetItem(1, "tdstockslip_pay_amt", slipnet_amt);
                }
                catch (Error) {
                }
                //end

            }
            catch (Error) {
                //LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
            objDwTailer.AcceptText();
            var disc_amt = getObjFloat(objDwTailer, 1, "disc_amt");
            if (disc_amt == 0) {
                validate_slipnet_amt();
            }
        }

        function DiscAmtChange(row) {
            try {
                var item_qty = 0, product_price = 0, disc_amt = 0;
                try {
                    item_qty = getObjFloat(objDwDetail, row, "item_qty");
                    product_price = getObjFloat(objDwDetail, row, "product_price");
                }
                catch (Error) {
                    alert("กรุณากรอกจำนวน และราคาต่อหน่วย");
                }
                disc_amt = getObjFloat(objDwDetail, row, "disc_amt");
                CalAmt(item_qty, product_price, disc_amt, row);
            }
            catch (Error) { }
        }

        function CalAmt(item_qty, product_price, disc_amt, row) {
            try {
                var amount = Calamtbefortax_taxtype_code(row);
                if (row == 1) {
                    $("#objDwDetail_datawindow input[name='amtbefortax_0']").val(addCommas(amount - disc_amt, 2));
                }
                objDwDetail.SetItem(row, "amtbefortax", (amount - disc_amt));
            }
            catch (Error) { }
            objDwDetail.AcceptText();
        }

        function CalAmt2(item_qty, product_price, disc_percent, disc_amt, row) {
            try {
                var amount = Calamtbefortax_taxtype_code(row);
                disc_amt = tradingService_Discount(disc_percent, amount);
                if (row == 1) {
                    $("#objDwDetail_datawindow input[name='disc_amt_0']").val(addCommas(disc_amt, 2));
                    $("#objDwDetail_datawindow input[name='amtbefortax_0']").val(addCommas(amount - disc_amt, 2));
                }
                objDwDetail.SetItem(row, "disc_amt", disc_amt);
                objDwDetail.SetItem(row, "amtbefortax", (amount - disc_amt));
            }
            catch (Error) { }
            objDwDetail.AcceptText();
        }

        function initcolor_from_taxopt(taxopt) {
            if (taxopt == "Y") {

                $("#objDwTailer_datawindow select[name='taxtype_code_0']").css('backgroundColor', '#E0E0E0');
                $("#objDwTailer_datawindow input[name='tax_rate_0']").css('backgroundColor', '#E0E0E0');
                for (var i = 0; i < objDwDetail.RowCount(); i++) {
                    $("#objDwDetail_datawindow select[name='taxtype_code_" + i + "']").css('backgroundColor', '#FFFFFF');
                    $("#objDwDetail_datawindow input[name='tax_rate_" + i + "']").css('backgroundColor', '#FFFFFF');
                    $("#objDwDetail_datawindow select[name='taxtype_code_" + i + "']").prop("disabled", false); // DwDetail.Modify("taxtype_code.Protect=0");
                    $("#objDwDetail_datawindow input[name='tax_rate_" + i + "']").prop("disabled", false); //  DwDetail.Modify("tax_rate.Protect=0");
                }
                $("#objDwTailer_datawindow select[name='taxtype_code_0']").prop("disabled", true); //DwTailer.Modify("taxtype_code.Protect=1");
                $("#objDwTailer_datawindow input[name='tax_rate_0']").prop("disabled", true); //  DwTailer.Modify("tax_rate.Protect=1");
            }
            else if (taxopt == "N") {

                $("#objDwTailer_datawindow select[name='taxtype_code_0']").css('backgroundColor', '#FFFFFF');
                $("#objDwTailer_datawindow input[name='tax_rate_0']").css('backgroundColor', '#FFFFFF');

                for (var i = 0; i < objDwDetail.RowCount(); i++) {
                    $("#objDwDetail_datawindow select[name='taxtype_code_" + i + "']").css('backgroundColor', '#E0E0E0');
                    $("#objDwDetail_datawindow input[name='tax_rate_" + i + "']").css('backgroundColor', '#E0E0E0');
                    $("#objDwDetail_datawindow select[name='taxtype_code_" + i + "']").prop("disabled", true); // DwDetail.Modify("taxtype_code.Protect=1");
                    $("#objDwDetail_datawindow input[name='tax_rate_" + i + "']").prop("disabled", true); //DwDetail.Modify("tax_rate.Protect=1");
                }
                $("#objDwTailer_datawindow select[name='taxtype_code_0']").prop("disabled", false); // DwTailer.Modify("taxtype_code.Protect=0");
                $("#objDwTailer_datawindow input[name='tax_rate_0']").prop("disabled", false);  // DwTailer.Modify("tax_rate.Protect=0");

            }
        }

        function OnDDwDetailButtonClick(s, r, c) {
            switch (c) {
                case "b_2":
                    Gcoop.OpenIFrame("740", "600", "w_dlg_td_search_product.aspx", "?sheetrow=" + r);
                    break;
                case "b_del":
                    objDwDetail.DeleteRow(r);
                    break;
            }
        }

        function GetDlgProductNo(sheetrow, product_no, product_desc) {
            objDwDetail.SetItem(sheetrow, "product_no", product_no);
            Gcoop.GetEl("HdRowDetail").value = sheetrow + "";
            jsProductNo();
        }

        function OnDwTailerItemChanged(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            switch (c) {
                case "disc_percent":
                    //jsDiscountTailer();
                    DiscountTailer();
                    TaxCodeChange(r);
                    TaxoptChange(1);
                    break;
                case "disc_amt":
                    //                    objDwTailer.SetItem(1, "disc_percent", "");
                    //                    $("#objDwTailer_datawindow input[name='disc_percent_0']").val("");
                    //jsDiscountTailer();
                    DiscountTailer();
                    TaxCodeChange(r);
                    TaxoptChange(1);
                    break;
                case "taxopt":
                    //jsTaxoptChange();
                    TaxoptChange(1);
                    break;
                case "taxtype_code":
                    //jsTaxCodeChangeTailer();
                    TaxoptChange(1);
                    calTAX();
                    break;
                case "slip_amt":
                    //jsSlipNetAmtTailer();
                    SlipNetAmtTailer();
                    break;
                case "transportfee":
                    //jsTransportfeeChange();
                    TransportfeeChange();
                    break;
                //          
                case "tax_rate":
                    //jstaxratChange();
                    TaxoptChange(1);
                    break;


            }
        }

        function DiscountTailer() {
            try {
                var row = 1;
                var slip_amt = 0;
                try {
                    slip_amt = objDwTailer.GetItem(row, "slip_amt");
                }
                catch (Error) {
                    objDwTailer.SetItem(row, "disc_percent", "");
                    alert("กรุณากรอกจำนวนงิน");
                }
                var disc_percent = getObjString(objDwTailer, row, "disc_percent");
                var amount = slip_amt;
                var disc_amt = 0;
                if (disc_percent != "") {
                    disc_amt = tradingService_Discount(disc_percent, amount);
                    if (row == 1) {
                        $("#objDwTailer_datawindow input[name='disc_amt_0']").val(addCommas(disc_amt, 2));
                    }
                    objDwTailer.SetItem(row, "disc_amt", disc_amt);
                }
                else {
                    disc_amt = getObjFloat(objDwTailer, row, "disc_amt");
                }
                objDwTailer.SetItem(row, "amtbefortax", (amount - disc_amt));
                if (row == 1) {
                    $("#objDwTailer_datawindow input[name='amtbefortax_0']").val(addCommas(amount - disc_amt, 2));
                }
            }
            catch (Error) { }
            calTAX();
            objDwTailer.AcceptText();
        }

        function SlipNetAmtTailer() {
            try {
                var slip_amt = getObjFloat(objDwTailer, 1, "slip_amt");
                var disc_amt = getObjFloat(objDwTailer, 1, "disc_amt");
                objDwTailer.SetItem(1, "amtbefortax", slip_amt - disc_amt);
                var tax_amt = getObjFloat(objDwTailer, 1, "tax_amt");
                var transportfee = getObjFloat(objDwTailer, 1, "transportfee");

                var sum_ = 0;
                sum_ = ((slip_amt - disc_amt) + tax_amt + transportfee);
                sum_ = NumberRound(sum_, 2);
                objDwTailer.SetItem(1, "slipnet_amt", sum_);
                if (row == 1) {
                    $("#objDwTailer_datawindow input[name='slipnet_amt_0']").val(addCommas(sum_, 2));
                }
            }
            catch (Error) {
                // LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
            objDwTailer.AcceptText();
        }

        function TransportfeeChange() {
            try {
                var slip_amt = getObjFloat(objDwTailer, 1, "slip_amt");
                var disc_amt = getObjFloat(objDwTailer, 1, "disc_amt");

                objDwTailer.SetItem(1, "amtbefortax", slip_amt - disc_amt);
                var tax_amt = getObjFloat(objDwTailer, 1, "tax_amt");
                var transportfee = getObjFloat(objDwTailer, 1, "transportfee");

                var sum = 0;
                sum = ((slip_amt - disc_amt) + tax_amt + transportfee);
                sum = NumberRound(sum, 2);
                objDwTailer.SetItem(1, "slipnet_amt", sum);
                if (row == 1) {
                    $("#objDwTailer_datawindow input[name='slipnet_amt_0']").val(addCommas(sum, 2));
                }
                //add
                try {
                    var slipnet_amt = getObjFloat(objDwTailer, 1, "slipnet_amt");
                    objDwMain.SetItem(1, "tdstockslip_pay_amt", slipnet_amt);
                }
                catch (Error) {
                }
                //end
            }
            catch (Error) {
            }
            objDwTailer.AcceptText();
        }

        function calTAX() {
            var transportfee = 0;
            var paymentby = "";
            var slip_amt = 0;
            var tax_amt = 0;
            var tax_rate = 0;
            var disc_amt = 0;
            var amtbefortax = 0;
            var taxtype_code = "";

            paymentby = getObjString(objDwMain, 1, "paymentby");
            taxtype_code = getObjString(objDwTailer, 1, "taxtype_code");
            slip_amt = getObjFloat(objDwTailer, 1, "slip_amt");
            tax_rate = getObjFloat(objDwTailer, 1, "tax_rate");
            disc_amt = getObjFloat(objDwTailer, 1, "disc_amt");
            amtbefortax = getObjFloat(objDwTailer, 1, "amtbefortax");
            transportfee = getObjFloat(objDwTailer, 1, "transportfee");

            switch (paymentby) {
                case "LON":
                    if (taxtype_code.trim() == "E") {
                        tax_amt = (amtbefortax * (tax_rate / 100));
                        objDwTailer.SetItem(1, "tax_amt", tax_amt);

                        var sum = 0;
                        sum = (amtbefortax + tax_amt + transportfee);
                        sum = NumberRound(sum, 2);
                        objDwTailer.SetItem(1, "slipnet_amt", sum);

                    }
                    //เพิ่ม
                    else if (taxtype_code.trim() == "I") {
                        tax_amt = (amtbefortax * (tax_rate / 100));
                        objDwTailer.SetItem(1, "tax_amt", tax_amt);
                        $("#objDwTailer_datawindow input[name='tax_amt_0']").val(addCommas(tax_amt, 2));

                        var sum = 0;
                        sum = (amtbefortax + tax_amt + transportfee);
                        sum = NumberRound(sum, 2);
                        objDwTailer.SetItem(1, "slipnet_amt", sum);
                        $("#objDwTailer_datawindow input[name='slipnet_amt_0']").val(addCommas(sum, 2));
                    } //
                    else {
                        tax_amt = (amtbefortax * (tax_rate / 100));
                        objDwTailer.SetItem(1, "tax_amt", tax_amt);

                        var sum = 0;
                        sum = (amtbefortax + transportfee);
                        sum = NumberRound(sum, 2);
                        objDwTailer.SetItem(1, "slipnet_amt", sum);

                    }
                    break;
                case "CSH":
                    if (taxtype_code.trim() == "E") {
                        tax_amt = (amtbefortax * (tax_rate / 100));
                        objDwTailer.SetItem(1, "tax_amt", tax_amt);

                        var sum = 0;
                        sum = (amtbefortax + tax_amt + transportfee);
                        sum = NumberRound(sum, 2);
                        objDwTailer.SetItem(1, "slipnet_amt", sum);

                    }
                    //เพิ่ม
                    else if (taxtype_code.trim() == "I") {
                        tax_amt = (amtbefortax * (tax_rate / 100));
                        objDwTailer.SetItem(1, "tax_amt", tax_amt);
                        $("#objDwTailer_datawindow input[name='tax_amt_0']").val(addCommas(tax_amt, 2));
                        var sum = 0;
                        sum = (amtbefortax + tax_amt + transportfee);
                        sum = NumberRound(sum, 2);
                        objDwTailer.SetItem(1, "slipnet_amt", sum);
                        $("#objDwTailer_datawindow input[name='slipnet_amt_0']").val(addCommas(sum, 2));
                    } //
                    else {
                        tax_amt = (amtbefortax * (tax_rate / 100));
                        objDwTailer.SetItem(1, "tax_amt", tax_amt);

                        var sum = 0;
                        sum = (amtbefortax + transportfee);
                        sum = NumberRound(sum, 2);
                        objDwTailer.SetItem(1, "slipnet_amt", (amtbefortax + transportfee));

                    }
                    break;
            }
            objDwTailer.AcceptText();
            var disc_amt = getObjFloat(objDwTailer, 1, "disc_amt");
            if (disc_amt == 0) {
                validate_slipnet_amt();
            }
            //add
            try {
                var slipnet_amt = getObjFloat(objDwTailer, 1, "slipnet_amt");
                objDwMain.SetItem(1, "tdstockslip_pay_amt", slipnet_amt);
            }
            catch (Error) {
            }
            //end
        }

        function tradingService_Discount(disc_percent, amount) {   // แปลงจาก services
            var first_amount = amount;
            if (disc_percent == "") {
                disc_percent = "0";
            }
            disc_percent = disc_percent.toString();
            amount = parseFloat(amount);
            var arr_disc_percent = disc_percent.split('/');
            var count = arr_disc_percent.length;
            for (var i = 0; i < count; i++) {
                var percent = parseFloat(arr_disc_percent[i]) / 100;
                amount = amount - (amount * percent);
            }

            return first_amount - NumberRound(amount, 2);
        }

        function pay_() {

            objDwMain.SetItem(1, "tdstockslip_less_pay", 0.00);
            objDwMain.SetItem(1, "tdstockslip_over_pay", 0.00);
            var tdstockslip_pay_amt = getObjFloat(objDwMain, 1, "tdstockslip_pay_amt");
            var slipnet_amt = getObjFloat(objDwTailer, 1, "slipnet_amt");

            var sum = NumberRound(slipnet_amt - tdstockslip_pay_amt, 2);
            if (sum < 0) {
                //var total = sum
                sum = sum * -1
                objDwMain.SetItem(1, "tdstockslip_over_pay", sum);
                objDwMain.SetItem(1, "tdstockslip_less_pay", 0.00);
            }
            else if (sum > 0) {
                objDwMain.SetItem(1, "tdstockslip_less_pay", sum);
                objDwMain.SetItem(1, "tdstockslip_over_pay", 0.00);
            }
            else {
                objDwMain.SetItem(1, "tdstockslip_less_pay", 0.00);
                objDwMain.SetItem(1, "tdstockslip_over_pay", 0.00);
            }
            objDwMain.AcceptText();
        }

        function SheetLoadComplete() {
            if (Gcoop.GetEl("HdOpenIFrame").value == "True") {
                if (confirm("คุณต้องการพิมพ์ใบกำกับภาษี หรือไม่ ?")) {
                    Gcoop.OpenIFrame("200", "200", "../../../Criteria/dlg/w_dlg_report_progress.aspx?&app=<%=app%>&gid=<%=gid%>&rid=<%=rid%>&pdf=<%=pdf%>", "");
                    Gcoop.GetEl("HdOpenReport").value = "false";
                }
            }
            var taxopt = "N";
            try {
                taxopt = objDwTailer.GetItem(1, "taxopt");
            }
            catch (Error) { }
            initcolor_from_taxopt(taxopt);
            PayEnable();
            var disc_amt = getObjFloat(objDwTailer, 1, "disc_amt");
            if (disc_amt == 0) {
                validate_slipnet_amt();
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_tradesrv_opr_iv_header"
        LibraryList="~/DataWindow/trading/sheet_td_opr_iv.pbl" ClientScriptable="True"
        ClientEventButtonClicked="OnDwMainButtonClick" ClientEvents="true" ClientEventItemChanged="OnDwMainItemChanged"
        cAutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" AutoRestoreContext="False" ClientEventClicked="OnDwMainClick"
        TabIndex="0">
    </dw:WebDataWindowControl>
    <asp:Label ID="Label3" runat="server" Text="เพิ่มแถว" onclick="DwDetailInsertRow();"
        Font-Size="Medium" Font-Underline="True" ForeColor="#0000CC"></asp:Label>
    <dw:WebDataWindowControl ID="DwDetail" runat="server" DataWindowObject="d_tradesrv_opr_iv_detail"
        LibraryList="~/DataWindow/trading/sheet_td_opr_iv.pbl" ClientScriptable="True"
        ClientEventButtonClicked="OnDDwDetailButtonClick" ClientEvents="true" ClientEventItemChanged="OnDwDetailItemChanged"
        cAutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" AutoRestoreContext="False" ClientEventClicked="OnDwDetailClick"
        Width="760px" Height="150px" TabIndex="50">
    </dw:WebDataWindowControl>
    <dw:WebDataWindowControl ID="DwTailer" runat="server" DataWindowObject="d_tradesrv_opr_iv_tailer"
        LibraryList="~/DataWindow/trading/sheet_td_opr_iv.pbl" ClientScriptable="True"
        ClientEventButtonClicked="OnDwTailerButtonClick" ClientEvents="true" ClientEventItemChanged="OnDwTailerItemChanged"
        cAutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" AutoRestoreContext="False" ClientEventClicked="OnDwTailerClick"
        TabIndex="100">
    </dw:WebDataWindowControl>
    <asp:HiddenField ID="HdRowDetail" runat="server" Value="" />
    <asp:HiddenField ID="HdResult" runat="server" Value="" />
    <asp:HiddenField ID="HdProducts" runat="server" Value="" />
    <asp:HiddenField ID="HdOpenIFrame" runat="server" />
    <asp:HiddenField ID="HdRowDwDetail" runat="server" />
</asp:Content>
