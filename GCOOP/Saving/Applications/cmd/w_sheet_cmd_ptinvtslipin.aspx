<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" 
CodeBehind="w_sheet_cmd_ptinvtslipin.aspx.cs" Inherits="Saving.Applications.cmd.w_sheet_cmd_ptinvtslipin" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=jsPostInvtId%>
    <%=jsPostOnInsert%>
    <%=jsPostDel%>
    <%=postFindShow%>
    <%=jsPostDealer%>
    <script type="text/javascript">

        function Validate() {
            try {
                for (var i = 1; i <= objDwDetail.RowCount(); i++) {
                    var invt_id = getObjString(objDwDetail, i, "invt_id");
                    var invt_qty = getObjFloat(objDwDetail, i, "invt_qty");
                    var unit_price = getObjFloat(objDwDetail, i, "unit_price");
                    if (invt_id == null || invt_id == "") {
                        alert("กรุณากรอกรหัสวัสดุให้ถูกต้อง");
                        return;
                    }
                    if (invt_qty <= 0) {
                        alert("กรุณากรอกจำนวนวัสดุให้ถูกต้อง");
                        return;
                    }
                    if (unit_price <= 0) {
                        alert("กรุณากรอกราคาต่อหน่วยให้ถูกต้อง");
                        return;
                    }
                }
            }
            catch (Error) { }

            return confirm("ยืนยันการบันทึกข้อมูล");
        }

        function OnFindShow(invt_id) {
            Gcoop.GetEl("HinvtId").value = invt_id;
            postFindShow();
        }

        function OnDwMainClike(s, r, c) {
            if (c == "b_dealer") {
                Gcoop.OpenDlg(600, 500, "w_dlg_cmd_dealermaster.aspx", "");
            }
        }

        function OnDwMainItemChange(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            switch (c) {
                case "dealer_no":
                    jsPostDealer();
                    break;
            }
        }

        function OnDwDetailOnClick(s, r, c) {
            if (c == "b_invtidsearch") {
                Gcoop.GetEl("HdR").value = r;
                Gcoop.OpenDlg("750", "570", "w_dlg_cmd_ptinvtmast.aspx", "")
            }
            else if (c == "b_del") {
                Gcoop.GetEl("HdR").value = r;
                jsPostDel();
                CalPriceMain();
            }
        }

        function OnDwDetailItemChange(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            switch (c) {
                case "invt_id":
                    Gcoop.GetEl("HdR").value = r;
                    jsPostInvtId();
                    break;
                case "invt_qty":
                    CalPriceDetail(r);
                    break;
                case "unit_price":
                    CalPriceDetail(r);
                    break;
            }
        }

        function postOnInsert() {
            objDwDetail.InsertRow(0);
        }

        function CalPriceDetail(row) {
            var invt_qty = 0, unit_price = 0, sumamt = 0;
            try {
                invt_qty = getObjFloat(objDwDetail, row, "invt_qty");
                unit_price = getObjFloat(objDwDetail, row, "unit_price");
                sumamt = invt_qty * unit_price;
                objDwDetail.SetItem(row, "sumprice", sumamt);
                CalPriceMain();
            }
            catch (Error) { }
        }

        function CalPriceMain() {
            var sumprice = 0, sumamt = 0;
            try {
                var r = objDwDetail.RowCount();
                for (var i = 1; i <= r; i++) {
                    sumprice = getObjFloat(objDwDetail, i, "sumprice");
                    sumamt = sumamt + sumprice;
                }
                objDwMain.SetItem(1, "sumamt", sumamt);
                objDwMain.AcceptText();
            }
            catch (Error)
            { }
        }

        function GetDlgDealer(dealerNo) {
            objDwMain.SetItem(1, "dealer_no", dealerNo);
            objDwMain.AcceptText();
            jsPostDealer();
        }
            
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_slipin_main"
        LibraryList="~/DataWindow/Cmd/cmd_ptinvtslipin.pbl" Width="750px" ClientScriptable="True"
        AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" ClientEventButtonClicked="OnDwMainClike" ClientEventItemChanged="OnDwMainItemChange">
    </dw:WebDataWindowControl>
    <div style="height: 18px; vertical-align: bottom; padding-left:30px;">
        <span class="NewRowLink" onclick="postOnInsert()" style="font-size:medium; float: left">เพิ่มแถว</span>
    </div>
    <br />
    <dw:WebDataWindowControl ID="DwDetail" runat="server" DataWindowObject="d_slipin_detail"
        LibraryList="~/DataWindow/Cmd/cmd_ptinvtslipin.pbl" Width="750px" ClientScriptable="True"
        AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" ClientEventButtonClicked="OnDwDetailOnClick" ClientEventItemChanged="OnDwDetailItemChange">
    </dw:WebDataWindowControl>
    <asp:HiddenField ID="HdR" runat="server" />
    <asp:HiddenField ID="HinvtId" runat="server" />
    <asp:HiddenField ID="HinvtName" runat="server" />
    
</asp:Content>
