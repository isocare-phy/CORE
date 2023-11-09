<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" 
CodeBehind="w_sheet_td_opr_rec.aspx.cs" Inherits="Saving.Applications.trading.w_sheet_td_opr_rec" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=jsPostSlipNo %>
    <%=jsBSlipNo %>
    <%=jsProductNo %>
    <%=jsDwDetailInsertRow %>
    <script type="text/javascript">
        function Validate() { // บันทึก
            return confirm("ต้องการบันทึกหรือไม่");
        }
        function MenubarOpen() {
            Gcoop.OpenIFrame("770", "600", "w_dlg_td_search_slip_store.aspx", "?sliptype_code=REC");
        }
        function GetDlgSlipStore(slip_no) { //รับค่าสลิป จาก ไดอะล็อค
            objDwMain.SetItem(1, "slip_no", slip_no);
            jsPostSlipNo();
        }
        function GetDlgBSlipNo(slip_no) {
            objDwMain.SetItem(1, "refdoc_no", slip_no);
            jsBSlipNo();
        }
        function OnDwMainButtonClick(s, r, c) {
            switch (c) {
                case "b_2":
                    Gcoop.OpenIFrame("900", "600", "w_dlg_td_search_value.aspx", "?sliptype_code=REC");
//                    var debt_no = ""
//                    try {
//                        debt_no = objDwMain.GetItem(1, "debt_no");
//                    } catch (err) { }
//                    Gcoop.OpenIFrame("900", "600", "w_dlg_td_search_value.aspx", "?sliptype_code=RC&debt_no=" + debt_no);
                    break;
            }
        }


        function DwDetailInsertRow() {
            jsDwDetailInsertRow();
        }

        function OnDwMainItemChanged(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            switch (c) {
                case "slip_no":
                    jsPostSlipNo();
                    break;
                case "refdoc_no":
                    jsBSlipNo();
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
        function OnDwDetailItemChanged(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            Gcoop.GetEl("HdRowDetail").value = r + "";
            switch (c) {
                case "product_no":
                    jsProductNo();
                    break;
            }
        }
        function OnDwDetailButtonClick(s, r, c) {
            switch (c) {
                case "b_2":
                    Gcoop.OpenIFrame("740", "600", "w_dlg_td_search_product.aspx", "?sheetrow=" + r);
                    break;
            }
        }
        function GetDlgProductNo(sheetrow, product_no, product_desc) {
            objDwDetail.SetItem(sheetrow, "product_no", product_no);
            Gcoop.GetEl("HdRowDetail").value = sheetrow + "";
            jsProductNo();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_tradesrv_opr_rec_header"
        LibraryList="~/DataWindow/trading/sheet_td_opr_rec.pbl" AutoRestoreContext="False"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
        ClientEventItemChanged="OnDwMainItemChanged" ClientFormatting="True" TabIndex="0"
        ClientEventButtonClicked="OnDwMainButtonClick" ClientEventClicked="OnDwMainClick">
    </dw:WebDataWindowControl>
    <asp:Label ID="Label3" runat="server" Text="เพิ่มแถว" onclick="DwDetailInsertRow();"
        Font-Size="Medium" Font-Underline="True" ForeColor="#0000CC"></asp:Label>
    <dw:WebDataWindowControl ID="DwDetail" runat="server" DataWindowObject="d_tradesrv_opr_rec_detail"
        LibraryList="~/DataWindow/trading/sheet_td_opr_rec.pbl" AutoRestoreContext="False"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
        ClientEventItemChanged="OnDwDetailItemChanged" ClientFormatting="True" TabIndex="50"
        ClientEventButtonClicked="OnDwDetailButtonClick" ClientEventClicked="OnDwDetailClick"
        Width="742px" Height="399px">
    </dw:WebDataWindowControl>
    <asp:HiddenField ID="HdRowDetail" runat="server" Value="" />
</asp:Content>