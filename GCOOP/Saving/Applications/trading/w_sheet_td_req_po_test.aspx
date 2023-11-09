<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" 
CodeBehind="w_sheet_td_req_po_test.aspx.cs" Inherits="Saving.Applications.trading.w_sheet_td_req_po_test" %>

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
    <%=jsdeliverydateChange%>
    <%=jscredittermChange%>
    <%=jsTaxrat%>

    <script type="text/javascript">
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
                case "cred_no":
                    jsCredNo();
                    break;
                case "delivery_tdate":
                    jsdeliverydateChange();
                    break;
                case "credit_term":
                    jscredittermChange();
                    break;
            }
        }

        function OnDwDetailItemChanged(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            switch (c) {
                case "product_no":
                    jsProductNo();
                    break;
            }
        }

        function DwDetailInsertRow() {
            jsDwDetailInsertRow();
        }

        function MenubarOpen() {
            Gcoop.OpenIFrame("770", "600", "w_dlg_td_search_slip_cred.aspx", "?sliptype_code=PO");
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
            }
        }

        function GetDlgDebtNo(debt_no) {
            objDwMain.SetItem(1, "debt_no", debt_no);
            jsDebtNo();
        }

        function GetDlgCredNo(cred_no) {
            objDwMain.SetItem(1, "cred_no", cred_no);
            jsCredNo();
        }

        function OnDwDetailItemChanged(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            Gcoop.GetEl("HdRowDetail").value = r + "";
            switch (c) {
                case "disc_percent":
                    jsDiscPercentChange();
                    break;
                case "product_price":
                    jsDiscPercentChange();
                    break;
                case "item_qty":
                    jsItemQtyChange();
                    break;
                case "taxtype_code":
                    jsTaxCodeChange();
                    break;
                case "product_no":
                    jsProductNo();
                    break;
            }
        }

        function OnDDwDetailButtonClick(s, r, c) {
            switch (c) {
                case "b_1":
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
                    jsDiscountTailer();
                    break;
                case "disc_amt":
                    jsDiscountTailer();
                    break;
                case "taxopt":
                    jsTaxoptChange();
                    break;
                case "taxtype_code":
                    jsTaxCodeChangeTailer();
                    break;
                case "slip_amt":
                    jsSlipNetAmtTailer();
                    break;
                case "tax_rate": //
                    jsTaxrat();
                    break;

            }
        }
        function SheetLoadComplete() {
            if (Gcoop.GetEl("HdOpenIFrame").value == "True") {
                if (confirm("คุณต้องการพิมพ์ใบสั่งซื้อ หรือไม่ ?")) {
                    Gcoop.OpenIFrame("200", "200", "../../../Criteria/dlg/w_dlg_report_progress.aspx?&app=<%=app%>&gid=<%=gid%>&rid=<%=rid%>&pdf=<%=pdf%>", "");
                    Gcoop.GetEl("HdOpenReport").value = "false";
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
<asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_tradesrv_req_po_header"
        LibraryList="~/DataWindow/trading/sheet_td_req_po.pbl" ClientScriptable="True"
        ClientEventButtonClicked="OnDwMainButtonClick" ClientEvents="true" ClientEventItemChanged="OnDwMainItemChanged"
        cAutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" AutoRestoreContext="False" ClientEventClicked="OnDwMainClick" TabIndex="0">
    </dw:WebDataWindowControl>

        <asp:Label ID="Label3" runat="server" Text="เพิ่มแถว" onclick="DwDetailInsertRow();"
                        Font-Size="Medium" Font-Underline="True" ForeColor="#0000CC"></asp:Label>
    <dw:WebDataWindowControl ID="DwDetail" runat="server" DataWindowObject="d_tradesrv_req_po_detail"
        LibraryList="~/DataWindow/trading/sheet_td_req_po.pbl" ClientScriptable="True"
        ClientEventButtonClicked="OnDDwDetailButtonClick" ClientEvents="true" ClientEventItemChanged="OnDwDetailItemChanged"
        cAutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" AutoRestoreContext="False" ClientEventClicked="OnDwDetailClick"
        Width="760px" Height="150px" TabIndex="50">
    </dw:WebDataWindowControl>

    <dw:WebDataWindowControl ID="DwTailer" runat="server" DataWindowObject="d_tradesrv_req_po_tailer"
        LibraryList="~/DataWindow/trading/sheet_td_req_po.pbl" ClientScriptable="True"
        ClientEventButtonClicked="OnDwTailerButtonClick" ClientEvents="true" ClientEventItemChanged="OnDwTailerItemChanged"
        cAutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" AutoRestoreContext="False" ClientEventClicked="OnDwTailerClick" TabIndex="100">
    </dw:WebDataWindowControl>
    <asp:HiddenField ID="HdRowDetail" runat="server" Value="" />
    <asp:HiddenField ID="HdBranchId" runat="server" Value="" />
    <asp:HiddenField ID="HdOpenIFrame" runat="server" />
</asp:Content>

