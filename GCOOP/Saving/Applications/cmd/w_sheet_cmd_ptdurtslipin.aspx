<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_cmd_ptdurtslipin.aspx.cs" 
Inherits="Saving.Applications.cmd.w_sheet_cmd_ptdurtslipin" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <%=jsPostDurtgrpCode%>
     <%=jsPostDealer%>
     <%=jsPostCalPrice%>
     <%=jsPostSlipinNo%>
    <script type="text/javascript">

        function Validate() {
            try {
                for (i = 0; i <= objDwMain.RowCount(); i++) {
                    var devaluelastcal_year = objDwMain.GetItem(i, "devaluelastcal_year");
                    if (devaluelastcal_year == null || devaluelastcal_year == "") {
                        alert("กรุณากรอกค่าเสื่อมที่คิดปีล่าสุดให้ถูกต้อง");
                        return;
                    }
                    var devaluestart_tdate = objDwMain.GetItem(i, "devaluestart_date");
                    if (devaluestart_tdate == null || devaluestart_tdate == "") {
                        alert("กรุณากรอกวันที่เริ่มคิดค่าเสื่อมให้ถูกต้อง");
                        return;
                    }
                    var devalue_percent = getObjFloat(objDwMain, 1, "devalue_percent");
                    if (devalue_percent <= 0) {
                        alert("กรุณากรอกค่าเสื่อม(%)ให้ถูกต้อง");
                        return;
                    }
                    var buy_tdate = objDwMain.GetItem(i, "buy_date");
                    if (buy_tdate == null || buy_tdate == "") {
                        alert("กรุณากรอกวันที่ซื้อให้ถูกต้อง");
                        return;
                    }
                    var durtrcv_code = objDwMain.GetItem(i, "durtrcv_code");
                    if (durtrcv_code == null || durtrcv_code == "") {
                        alert("กรุณากรอกรหัสการได้มาให้ถูกต้อง");
                        return;
                    }
                    var durtgrp_code = objDwMain.GetItem(i, "durtgrp_code");
                    if (durtgrp_code == null || durtgrp_code == "") {
                        alert("กรุณากรอกหมวดครุภัณฑ์ให้ถูกต้อง");
                        return;
                    }
                    var unit_code = objDwMain.GetItem(i, "unit_code");
                    if (unit_code == null || unit_code == "") {
                        alert("กรุณากรอกหน่วยนับให้ถูกต้อง");
                        return;
                    }
                    var durt_name = objDwMain.GetItem(i, "durt_name");
                    if (durt_name == null || durt_name == "") {
                        alert("กรุณากรอกชื่อครุภัณฑ์ให้ถูกต้อง");
                        return;
                    }
                    var durt_qty = getObjFloat(objDwMain, 1, "durt_qty");
                    if (durt_qty <= 0) {
                        alert("กรุณากรอกจำนวนรับเข้าให้ถูกต้อง");
                        return;
                    }
                    var unit_price = getObjFloat(objDwMain, 1, "unit_price");
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
                case "unit_price":
                    CalPriceMain(s);
                    break;
                case "durt_qty":
                    CalPriceMain(s);
                    break;
                case "durtgrp_code":
                    jsPostDurtgrpCode();
                    break;
                case "dealer_no":
                    jsPostDealer();
                    break;
                case "price_net":
                    //jsPostCalPrice();
                    CalPriceNetMain(s);
                    break;
            }
        }

        function CalPriceMain(s) {
            var sumamt = 0, price = 0, amount = 0;
            try {
                price = getObjFloat(s, 1, "unit_price");
                amount = getObjFloat(s, 1, "durt_qty");
                sumamt = price * amount;
                s.SetItem(1, "price_net", sumamt);
                s.AcceptText();
                }
            catch (Error)
            { }
        }

        function CalPriceNetMain(s) {
            var sumamt = 0, price = 0, amount = 0;
            try {
                sumamt = getObjFloat(s, 1, "price_net");
                amount = getObjFloat(s, 1, "durt_qty");
                price = sumamt / amount;
                price = price.toFixed(2);
                s.SetItem(1, "unit_price", price);
                s.AcceptText();
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
//        function OnFindShow(durt_id, durt_name) {
//            Gcoop.GetEl("HdurtId").value = durt_id;
//            objDwMain.SetItem(1, "durt_id", durt_id);
//            objDwMain.AcceptText();
//            postFindShow();
//        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
<asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_ptdurtslipin_main"
        LibraryList="~/DataWindow/Cmd/cmd_ptdurtslipin.pbl" Width="750px" ClientScriptable="True"
        AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" ClientEventItemChanged="OnDwMainItemChange" ClientEventButtonClicked="OnDwMainClick">
    </dw:WebDataWindowControl>
    <asp:HiddenField ID="HdurtId" runat="server" />
    <asp:HiddenField ID="HdSlipno" runat="server" />
</asp:Content>
