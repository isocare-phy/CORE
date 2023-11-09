<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_td_opr_rc.aspx.cs" Inherits="Saving.Applications.trading.w_sheet_td_opr_rc" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript %>
    <%=jsCredNo %>
    <%=jsDebtNo%>
    <%=jsProductNo %>
    <%=jsPostSlipNo %>
    <%=jsDiscPercentChange %>
    <%=jsDiscountTailer %>
    <%=jsRefresh %>
    <%=jsDwDetailInsertRow %>
    <%=jsItemQtyChange %>
    <%=jsTaxCodeChange %>
    <%=jsSlipNetAmtTailer%>
    <%=jsTaxoptChange %>
    <%=jsTaxCodeChangeTailer %>
    <%=jsBSlipNo%>
    <%=jstaxrat%>
    <script type="text/javascript">
        var ajax = new AjaxCall();
        function Validate() { // บันทึก
            return confirm("ต้องการบันทึกหรือไม่");
        }

        function OnDwMainItemChanged(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            switch (c) {
                case "debt_no":
                    jsAjaxDebt_no(s, r, c, v);
                    //jsDebtNo();
                    break;
                case "cred_no":
                    jsAjaxCredNo(s, r, c, v);
                    //jsCredNo();
                    break;
                case "refdoc_no":
                    jsBSlipNo();
                    break;
            }
        }

        function jsAjaxCredNo(s, r, c, v) {
            var coop_id = getObjString(s, r, "coop_id");
            var cred_no = getObjString(s, r, "cred_no");
            var arg = 'coop_id=' + coop_id + "&cred_no=" + cred_no;
            ajax.PostbackPage("w_sheet_td_opr_rc_ajax.aspx", "ajaxinitCred_no", arg, initCredno);
        }
        function initCredno(text_xml) {
            if (text_xml != "") {
                var s = objDwMain;
                var r = 1;
                var xml = new XmlDataSourceTool(text_xml);
                s.SetItem(r, "crednm", xml.GetItemString(0, "crednm"));
                s.AcceptText();
            }
        }

        function jsAjaxDebt_no(s, r, c, v) {
            var coop_id = getObjString(s, r, "coop_id");
            var debt_no = getObjString(s, r, "debt_no");
            var arg = 'coop_id=' + coop_id + "&debt_no=" + debt_no;
            ajax.PostbackPage("w_sheet_td_opr_rc_ajax.aspx", "ajaxinitDebt_no", arg, initDebt_no);
        }
        function initDebt_no(text_xml) {
            if (text_xml != "") {
                var s = objDwMain;
                var r = 1;
                var xml = new XmlDataSourceTool(text_xml);
                s.SetItem(r, "debtnm", xml.GetItemString(0, "debtnm"));
                s.SetItem(r, "paymentby", xml.GetItemString(0, "paymentby"));
                s.SetItem(r, "price_level", xml.GetItemString(0, "price_level"));
                s.SetItem(r, "debt_addr", xml.GetItemString(0, "debt_addr"));
                s.SetItem(r, "debt_type", xml.GetItemString(0, "debt_type"));
                s.AcceptText();
            }
        }

        function DwDetailInsertRow() {
            jsDwDetailInsertRow();
        }

        function MenubarOpen() {
            Gcoop.OpenIFrame("770", "600", "w_dlg_td_search_slip_cred.aspx", "?sliptype_code=RC");
        }

        function GetDlgSlipNoCred(slip_no) { //รับค่าสลิป จาก ไดอะล็อค
            objDwMain.SetItem(1, "slip_no", slip_no);
            jsPostSlipNo();
        }

        function OnDwMainButtonClick(s, r, c) {
            switch (c) {
                case "b_1":
                    Gcoop.OpenIFrame("740", "600", "w_dlg_td_search_sale.aspx", "");
                    break;
                case "b_2":
                    Gcoop.OpenIFrame("740", "600", "w_dlg_td_search_cust.aspx", "");
                    break;
                case "b_3":
                    var cred_no = ""
                    try {
                        cred_no = objDwMain.GetItem(1, "cred_no");
                    } catch (err) { }
                    Gcoop.OpenIFrame("900", "600", "w_dlg_td_search_value_cust.aspx", "?sliptype_code=PO&debt_no=" + cred_no);
                    break;
            }
        }

        function GetDlgDebtNo(debt_no) {
            objDwMain.SetItem(1, "debt_no", debt_no);
            //jsDebtNo();
            jsAjaxDebt_no(objDwMain, 1, "", "");
        }

        function GetDlgCredNo(cred_no) {
            objDwMain.SetItem(1, "cred_no", cred_no);
            jsAjaxCredNo(objDwMain, 1, "", "");
            //jsCredNo();
        }

        function GetDlgBSlipNo(slip_no) {
            objDwMain.SetItem(1, "refdoc_no", slip_no);
            jsBSlipNo();
        }

        function OnDwDetailItemChanged(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            Gcoop.GetEl("HdRowDetail").value = r + "";
            switch (c) {
                case "disc_percent":
                    //jsDiscPercentChange();
                    DiscPercentChange(r);
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
                    //jsAjaxItemQty(s, r, c, v);
                    break;
                case "taxtype_code":
                    //jsTaxCodeChange();
                    TaxCodeChange(r);
                    TaxoptChange(1);
                    break;
                case "product_no":
                    //jsProductNo();
                    jsAjaxProduct(s, r, c, v);
                    break;
            }
        }
        function jsAjaxProduct(s, r, c, v) {
            var product_no = s.GetItem(r, "product_no");
            var store_id = s.GetItem(r, "store_id");
            var arg = 'product_no=' + product_no + "&store_id=" + store_id;
            Gcoop.GetEl("HdRowDwDetail").value = r + "";
            ajax.PostbackPage("w_sheet_td_opr_rc_ajax.aspx", "ajaxinitDetail", arg, initRowDwDetail);
        }
        function initRowDwDetail(text_xml) {
            if (text_xml != "") {
                var s = objDwDetail;
                var r = Number(Gcoop.GetEl("HdRowDwDetail").value);
                var xml = new XmlDataSourceTool(text_xml);
                if (r == 1) {
                    $("#objDwDetail_datawindow input[name='tax_rate_0']").val(addCommas(xml.GetItemString(0, "tax_rate"), 2));
                    $("#objDwTailer_datawindow input[name='tax_rate_0']").val(addCommas(xml.GetItemString(0, "tax_rate"), 2));
                    $("#objDwDetail_datawindow select[name='taxtype_code_0']").val(xml.GetItemString(0, "taxtype_code"));
                    $("#objDwTailer_datawindow select[name='taxtype_code_0']").val(xml.GetItemString(0, "taxtype_code"));
                }
                s.SetItem(r, "product_desc", xml.GetItemString(0, "product_desc"));
                s.SetItem(r, "item_qty", 0);
                s.SetItem(r, "unit_code", xml.GetItemString(0, "unit_code"));
                s.SetItem(r, "taxtype_code", xml.GetItemString(0, "taxtype_code"));
                s.SetItem(r, "tax_rate", xml.GetItemString(0, "tax_rate"));
                s.SetItem(r, "balance_qty", xml.GetItemString(0, "balance_qty"));
                objDwTailer.SetItem(1, "tax_rate", xml.GetItemString(0, "tax_rate"));
                objDwTailer.SetItem(1, "taxtype_code", xml.GetItemString(0, "taxtype_code"));
                objDwTailer.AcceptText();
                s.AcceptText();
                //jsAjaxItemQty(objDwDetail, r, "", 0);
                Gcoop.GetEl("HdFocus").value = r + "";
                SetFocusDwDetail();
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
            ajax.PostbackPage("w_sheet_td_opr_rc_ajax.aspx", "ajaxinitItemQty", arg, initItemQty);
        }
        function initItemQty(text) {
            if (text != "") {
                var row = Number(Gcoop.GetEl("HdRowDwDetail").value);
                var xml = new XmlDataSourceTool(text);
                var price = Number(xml.GetItemNumber(0, "price"));
                var disc_percent = xml.GetItemString(0, "disc_percent");
                var disc_amt = xml.GetItemNumber(0, "disc_amt");

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
                    item_qty = getObjFloat(objDwDetail, row, "item_qty");
                }
                catch (Error) {
                    alert("กรุณากรอก จำนวนสินค้า แถวที่ " + row);
                }
                var product_price = getObjFloat(objDwDetail, row, "product_price");
                var amount = (item_qty * product_price);
                var disc_amt = getObjFloat(objDwDetail, row, "disc_amt");
                var amtbefortax = Number(amount - disc_amt);
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
                //   
                case "tax_rate":
                    //jstaxrat();
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
                //                if (row == 1) {
                $("#objDwTailer_datawindow input[name='slipnet_amt_0']").val(addCommas(sum, 2));
                //                }
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
            
           
            SetStoreID();
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
            var transportfee = getObjFloat(objDwTailer, 1, "transportfee");
            sumDwDetail = NumberRound(sumDwDetail, 2);
            var sumDwTailer = NumberRound(getObjFloat(objDwTailer, 1, "slipnet_amt") - transportfee, 2);
            if (sumDwDetail > sumDwTailer) {  // บนมากว่า
                var tmp = NumberRound(sumDwDetail - sumDwTailer, 2);
                //  objDwTailer.SetItem(1, "slipnet_amt", sumDwDetail);
                var tax_amt = NumberRound(getObjFloat(objDwTailer, 1, "tax_amt") + tmp, 2);
                objDwTailer.SetItem(1, "tax_amt", tax_amt);
            } else if (sumDwTailer > sumDwDetail) {  // ล่างมากกว่า
                var tmp = NumberRound(sumDwTailer - sumDwDetail, 2);
                //    objDwTailer.SetItem(1, "slipnet_amt", sumDwDetail);
                var tax_amt = NumberRound(getObjFloat(objDwTailer, 1, "tax_amt") - tmp, 2);
                objDwTailer.SetItem(1, "tax_amt", tax_amt);
            }
            sumDwTailer = sumDwTailer + transportfee;
            objDwTailer.SetItem(1, "slipnet_amt", sumDwTailer);
            objDwTailer.AcceptText();
        }

        function SetStoreID() {
            var store_id = getObjString(objDwMain, 1, "store_id");
            var count = objDwDetail.RowCount();
            for (var i = 1; i <= count; i++) {
                objDwDetail.SetItem(i, "store_id", store_id);
            }
            objDwDetail.AcceptText();
        }

        function SetFocusDwDetail() {
            var r = Gcoop.GetEl("HdFocus").value;
            if (r != "") {
                r = Number(r);
                $("#objDwDetail_datawindow input[name='product_no_" + (r) + "']").focus();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_tradesrv_opr_rc_header"
        LibraryList="~/DataWindow/trading/sheet_td_opr_rc.pbl" ClientScriptable="True"
        ClientEventButtonClicked="OnDwMainButtonClick" ClientEvents="true" ClientEventItemChanged="OnDwMainItemChanged"
        cAutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" AutoRestoreContext="False" ClientEventClicked="OnDwMainClick"
        TabIndex="0">
    </dw:WebDataWindowControl>
    <asp:Label ID="Label3" runat="server" Text="เพิ่มแถว" onclick="DwDetailInsertRow();"
        Font-Size="Medium" Font-Underline="True" ForeColor="#0000CC"></asp:Label>
    <dw:WebDataWindowControl ID="DwDetail" runat="server" DataWindowObject="d_tradesrv_opr_rc_detail"
        LibraryList="~/DataWindow/trading/sheet_td_opr_rc.pbl" ClientScriptable="True"
        ClientEventButtonClicked="OnDDwDetailButtonClick" ClientEvents="true" ClientEventItemChanged="OnDwDetailItemChanged"
        cAutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" AutoRestoreContext="False" ClientEventClicked="OnDwDetailClick"
        Width="760px" Height="180px" TabIndex="50">
    </dw:WebDataWindowControl>
    <dw:WebDataWindowControl ID="DwTailer" runat="server" DataWindowObject="d_tradesrv_opr_rc_tailer"
        LibraryList="~/DataWindow/trading/sheet_td_opr_rc.pbl" ClientScriptable="True"
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
    <asp:HiddenField ID="HdFocus" runat="server" Value="" />
</asp:Content>
