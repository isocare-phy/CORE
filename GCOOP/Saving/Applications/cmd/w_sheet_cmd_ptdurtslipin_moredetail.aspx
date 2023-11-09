<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_cmd_ptdurtslipin_moredetail.aspx.cs" 
Inherits="Saving.Applications.cmd.w_sheet_cmd_ptdurtslipin_moredetail" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <%=jsPostDurtgrpCode%>
     <%=jsPostDealer%>
     <%=jsPostCalPrice%>
     <%=jsPostSlipinNo%>
     <%=jsPostOnInsert%>
    <script type="text/javascript">

        function Validate() {
            try {
                var devaluelastcal_year = objDwMain.GetItem(1, "devaluelastcal_year");
                if (devaluelastcal_year == null || devaluelastcal_year == "") {
                    alert("กรุณากรอกค่าเสื่อมที่คิดปีล่าสุดให้ถูกต้อง");
                    return;
                }
                var devaluestart_tdate = objDwMain.GetItem(1, "devaluestart_date");
                if (devaluestart_tdate == null || devaluestart_tdate == "") {
                    alert("กรุณากรอกวันที่เริ่มคิดค่าเสื่อมให้ถูกต้อง");
                    return;
                }
                var buy_tdate = objDwMain.GetItem(1, "buy_date");
                if (buy_tdate == null || buy_tdate == "") {
                    alert("กรุณากรอกวันที่ซื้อให้ถูกต้อง");
                    return;
                }
                var durtrcv_code = objDwMain.GetItem(1, "durtrcv_code");
                if (durtrcv_code == null || durtrcv_code == "") {
                    alert("กรุณากรอกรหัสการได้มาให้ถูกต้อง");
                    return;
                }

                for (i = 1; i <= objDwDetail.RowCount(); i++) {
                    var devalue_percent = getObjFloat(objDwDetail, i, "devalue_percent");
                    if (devalue_percent <= 0) {
                        alert("กรุณากรอกค่าเสื่อม(%)ให้ถูกต้อง");
                        return;
                    }
                    var durtgrp_code = objDwDetail.GetItem(i, "durtgrp_code");
                    if (durtgrp_code == null || durtgrp_code == "") {
                        alert("กรุณากรอกหมวดครุภัณฑ์ให้ถูกต้อง");
                        return;
                    }
                    var unit_code = objDwDetail.GetItem(i, "unit_code");
                    if (unit_code == null || unit_code == "") {
                        alert("กรุณากรอกหน่วยนับให้ถูกต้อง");
                        return;
                    }
                    var durt_name = objDwDetail.GetItem(i, "durt_name");
                    if (durt_name == null || durt_name == "") {
                        alert("กรุณากรอกชื่อครุภัณฑ์ให้ถูกต้อง");
                        return;
                    }
                    var durt_qty = getObjFloat(objDwDetail, i, "durt_qty");
                    if (durt_qty <= 0) {
                        alert("กรุณากรอกจำนวนรับเข้าให้ถูกต้อง");
                        return;
                    }
                    var unit_price = getObjFloat(objDwDetail, i, "unit_price");
                    if (unit_price <= 0) {
                        alert("กรุณากรอกราคาต่อหน่วยให้ถูกต้อง");
                        return;
                    }
                    
                }
            }
            catch (Error) { }
            return confirm("ยืนยันการบันทึกข้อมูล");
        }

        function OnDwMainItemChange(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            switch (c) {
                case "durtgrp_code":
                    jsPostDurtgrpCode();
                    break;
                case "dealer_no":
                    jsPostDealer();
                    break;
                case "fee_amt":
                    CalPriceNetMain(s);
                    break;
            }
        }

        function OnDwDetailItemChange(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            switch (c) {
                case "durtgrp_code":
                    Gcoop.GetEl("HdRow").value = r;
                    jsPostDurtgrpCode();
                    break;
                case "unit_price":
                    CalPriceMain(s, r);
                    CalPriceAllDet();
                    break;
                case "durt_qty":
                    CalPriceMain(s, r);
                    CalPriceAllDet();
                    break;
            }
        }

        function CalPriceMain(s, r) {
            var sumamt = 0, price = 0, amount = 0, feeAmt = 0;
            try {
                price = getObjFloat(s, r, "unit_price");
                amount = getObjFloat(s, r, "durt_qty");
                sumamt = price * amount;
                s.SetItem(r, "unit_price", price);
                s.SetItem(r, "price_net", sumamt);
                s.AcceptText();
                }
            catch (Error)
            { }
        }

        function CalPriceNetMain(s) {
            var feeAmt = 0, price = 0, amount = 0;
            try {
                feeAmt = getObjFloat(s, 1, "fee_amt");
                if (feeAmt == 0) {
                    CalPriceAllDet();
                }
                else {
                    CalPriceAllDet();
                    /*price = getObjFloat(s, 1, "sumamt_net");
                    price = price + feeAmt;
                    price = price.toFixed(2);
                    s.SetItem(1, "sumamt_net", price);
                    s.AcceptText();*/
                }
            }
            catch (Error)
            { }
        }

        function CalPriceAllDet() {
            var sumprice = 0, sumamt = 0, feeAmt = 0;
            try {
                feeAmt = getObjFloat(objDwMain, 1, "fee_amt");
                var r = objDwDetail.RowCount();
                for (var i = 1; i <= r; i++) {
                    sumprice = getObjFloat(objDwDetail, i, "price_net");
                    sumamt = sumamt + sumprice;
                }
                sumamt = sumamt + feeAmt;
                objDwMain.SetItem(1, "sumamt_net", sumamt);
                objDwMain.AcceptText();
            }
            catch (Error)
            { }
        }

        function OnDwMainClick(s, r, c) {
            if (c == "b_dealer") {
                Gcoop.OpenDlg(600, 500, "w_dlg_cmd_dealermaster.aspx", "");
            }
        }

        function GetDlgDealer(dealerNo) {
            objDwMain.SetItem(1, "dealer_no", dealerNo);
            objDwMain.AcceptText();
            jsPostDealer();
        }

        function MenubarOpen() {
            Gcoop.OpenDlg(675, 400, "w_dlg_cmd_ptdurtslipin.aspx", "");
        }

        function GetValDlgDurtSlipin(SlipinNo) {
            objDwMain.SetItem(1, "slipin_no", SlipinNo);
            objDwMain.AcceptText();
            Gcoop.GetEl("HdSlipno").value = SlipinNo;
            jsPostSlipinNo();
        }

        function postOnInsert() {
            jsPostOnInsert();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
<asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_ptdurtslipin_main_new"
        LibraryList="~/DataWindow/Cmd/cmd_ptdurtslipin.pbl" Width="750px" ClientScriptable="True"
        AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" ClientEventItemChanged="OnDwMainItemChange" ClientEventButtonClicked="OnDwMainClick">
    </dw:WebDataWindowControl>
    <div style="height: 18px; vertical-align: bottom; padding-left:30px; padding-bottom:15px;">
        <span class="NewRowLink" onclick="postOnInsert()" style="font-size:medium; float: left">เพิ่มแถว</span>
    </div>
    <dw:WebDataWindowControl ID="DwDetail" runat="server" DataWindowObject="d_ptdurtslipin_detail"
        LibraryList="~/DataWindow/Cmd/cmd_ptdurtslipin.pbl" Width="750px" ClientScriptable="True"
        AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" ClientEventItemChanged="OnDwDetailItemChange" ClientEventButtonClicked="OnDwDetailClick">
    </dw:WebDataWindowControl>
    <asp:HiddenField ID="HdurtId" runat="server" />
    <asp:HiddenField ID="HdSlipno" runat="server" />
    <asp:HiddenField ID="HdRow" runat="server" />
</asp:Content>
