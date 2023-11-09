<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_td_info_produc.aspx.cs" Inherits="Saving.Applications.trading.w_sheet_td_info_produc" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%=jsPostProductNo%>
<%=jsDwTailerInsertRow%>
<%=jsDwTailerInsertRow1%>
<%=jspostProductChange%>
    <script type="text/javascript">
        function Validate() { // บันทึก
            return confirm("ต้องการบันทึกหรือไม่");
        }

        function OnDwMainItemChanged(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            switch (c) {
                case "product_no":
                    jsPostProductNo();
                    break;
            }
                }
        function SheetLoadComplete() {
            var tab = Gcoop.ParseInt(Gcoop.GetEl("HiddenFieldTab").value);
            showTabPage(tab);
        }

        function showTabPage(tab) {
            var i = 1;
            var tabamount = 2;
            for (i = 1; i <= tabamount; i++) {
                document.getElementById("tab_" + i).style.visibility = "hidden";
                document.getElementById("stab_" + i).style.backgroundColor = "rgb(200,235,255)";
                if (i == tab) {
                    document.getElementById("tab_" + i).style.visibility = "visible";
                    document.getElementById("stab_" + i).style.backgroundColor = "rgb(211,213,255)";
                    Gcoop.GetEl("HiddenFieldTab").value = i + "";
                }
            }
        }

        function MenubarOpen() {
            Gcoop.OpenIFrame("740", "600", "w_dlg_td_search_product.aspx", "?sheetrow=1");
        }

        function OnDwMainButtonClick(s, r, c) {
            if (c == "b_1") {
                var product_no = "";
                try {
                    product_no = objDwMain.GetItem(1, "product_no");
                } catch (err) { }
                Gcoop.OpenIFrame("900", "600", "w_dlg_td_product_change.aspx", "?product_no_old=" + product_no);       
            }
        }

        function GetDlgProductChange(product_no, product_desc) {
            objDwMain.SetItem(1, "product_no", product_no);
            jspostProductChange();
        }


        function GetDlgProductNo(sheetrow, product_no, product_desc) {
            objDwMain.SetItem(sheetrow, "product_no", product_no);
            jsPostProductNo();
        }
        function DwTailerInsertRow() {
            jsDwTailerInsertRow();
        }
        function DwTailerInsertRow1() {
            jsDwTailerInsertRow1();
        }

        
         function OnDwStockButtonClicked(s, r, c) {
            switch (c) {
                case "b_del":
                    objDwDetail.DeleteRow(r);
                    break;
            }
        }

        function OnDwTailerButtonClicked(s, r, c) {
            switch (c) {
                case "b_del":
                    objDwTailer.DeleteRow(r);
                    break;
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_tradesrv_info_product_edit"
        LibraryList="~/DataWindow/trading/sheet_td_info_product.pbl" AutoRestoreContext="False"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
        ClientEventItemChanged="OnDwMainItemChanged" ClientFormatting="True" TabIndex="1"
        ClientEventButtonClicked="OnDwMainButtonClick" ClientEventClicked="OnDwMainClick">
    </dw:WebDataWindowControl>
    <table style="width: 100%; border: solid 1px; margin-top: 5px">
        <tr align="center" class="dwtab">
            <td style="background-color: rgb(211,213,255); cursor: pointer;" id="stab_1" width="50%"
                onclick="showTabPage(1);">
                รายละเอียด
            </td>
            <td style="background-color: rgb(200,235,255); cursor: pointer;" id="stab_2" width="50%"
                onclick="showTabPage(2);">
                ราคา
            </td>
        </tr>
    </table>
    <table style="width: 100%; border: solid 1px; margin-top: 2px" class="dwcontent">
        <tr>
            <td style="height: 400px;" valign="top">

                <div id="tab_1" style="visibility: visible; position: absolute;">
                    <asp:Label ID="Label2" runat="server" Text="เพิ่มแถว" onclick="DwTailerInsertRow();"
                        Font-Size="Medium" Font-Underline="True" ForeColor="#0000CC" onclicked="showTabPage(1);"></asp:Label>
                    <dw:WebDataWindowControl ID="DwDetail" runat="server" DataWindowObject="d_tradesrv_info_product_detail"
                        LibraryList="~/DataWindow/trading/sheet_td_info_product.pbl" AutoRestoreContext="False"
                        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
                        ClientEventItemChanged="OnDwStocktemChanged" ClientFormatting="True" TabIndex="100"
                        ClientEventButtonClicked="OnDwStockButtonClicked" Width="742px" Height="399px">
                    </dw:WebDataWindowControl>
                </div>

                <div id="tab_2" style="visibility: hidden; position: absolute;">
                    <asp:Label ID="Label1" runat="server" Text="เพิ่มแถว" onclick="DwTailerInsertRow1();"
                        Font-Size="Medium" Font-Underline="True" ForeColor="#0000CC" onclicked="showTabPage(2);"></asp:Label>
                    <dw:WebDataWindowControl ID="DwTailer" runat="server" DataWindowObject="d_tradesrv_info_productprice"
                        LibraryList="~/DataWindow/trading/sheet_td_info_product.pbl" AutoRestoreContext="False"
                        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
                        ClientEventItemChanged="OnDwTailerItemChanged" ClientFormatting="True" TabIndex="200"
                        ClientEventButtonClicked="OnDwTailerButtonClicked" Width="742px" Height="399px">
                    </dw:WebDataWindowControl>
                </div>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="HiddenFieldTab" runat="server" />
</asp:Content>
