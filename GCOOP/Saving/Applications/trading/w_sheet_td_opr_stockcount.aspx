<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_td_opr_stockcount.aspx.cs" Inherits="Saving.Applications.trading.w_sheet_td_opr_stockcount" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=jsPostSlipNo %>
    <%=jsProductNo %>
    <%=jsDwDetailInsertRow %>
    <%=jsStoreChange%>
    
    <script type="text/javascript">
        function Validate() { // บันทึก
            return confirm("ต้องการบันทึกหรือไม่");
        }

        function OnDwMainItemChanged(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            Gcoop.GetEl("HdRowDetail").value = r + "";
            switch (c) {
                case "product_no":
                    jsProductNo();
                    break;
                case "store_id":
                    jsStoreChange();
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
            //Gcoop.OpenIFrame("770", "600", "w_dlg_td_search_stockcount.aspx", "");
            Gcoop.OpenIFrame("770", "600", "w_dlg_td_search_stockcount.aspx", "?sliptype_code=VCS");
        }
        

//        function GetDlgSlipNo(slip_no) { //รับค่าสลิป จาก ไดอะล็อค
//            objDwMain.SetItem(1, "slip_no", slip_no);
//            jsPostSlipNo();
//        }
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

        function GetStockCount(slip_no) {
            objDwMain.SetItem(1, "slip_no", slip_no);
            jsPostSlipNo();
        }



    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_tradesrv_opr_stockcount_header"
        LibraryList="~/DataWindow/trading/sheet_td_opr_stockcount.pbl" AutoRestoreContext="False"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
        ClientEventItemChanged="OnDwMainItemChanged" ClientFormatting="True" TabIndex="0"
        ClientEventButtonClicked="OnDwMainButtonClick" ClientEventClicked="OnDwMainClick">
    </dw:WebDataWindowControl>
    <asp:Label ID="Label3" runat="server" Text="เพิ่มแถว" onclick="DwDetailInsertRow();"
        Font-Size="Medium" Font-Underline="True" ForeColor="#0000CC"></asp:Label>
    <dw:WebDataWindowControl ID="DwDetail" runat="server" DataWindowObject="d_tradesrv_opr_stockcount_detail"
        LibraryList="~/DataWindow/trading/sheet_td_opr_stockcount.pbl" AutoRestoreContext="False"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
        ClientEventItemChanged="OnDwDetailItemChanged" ClientFormatting="True" TabIndex="50"
        ClientEventButtonClicked="OnDwDetailButtonClick" ClientEventClicked="OnDwDetailClick"
        Width="742px" Height="399px">
    </dw:WebDataWindowControl>
    <asp:HiddenField ID="HdRowDetail" runat="server" Value="" />
    <asp:HiddenField ID="HdSlip_no" runat="server" Value="" />
</asp:Content>
