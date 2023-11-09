<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_td_info_produc_move.aspx.cs" Inherits="Saving.Applications.trading.w_sheet_td_info_produc_move" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%=jsPostProductNo%>
<%=jsstore%>

    <script type="text/javascript">
//        function Validate() { // บันทึก
//            return confirm("ต้องการบันทึกหรือไม่");
//        }

        function OnDwMainItemChanged(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            switch (c) {
                case "product_no":
                    jsPostProductNo();
                    break;
                case "store_id":
                    jsstore();
                    break;
            }
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

        function GetDlgProductNo(sheetrow, product_no, product_desc) {
            objDwMain.SetItem(sheetrow, "product_no", product_no);
            jsPostProductNo();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_tradesrv_info_product"
        LibraryList="~/DataWindow/trading/sheet_td_info_product_move.pbl" AutoRestoreContext="False"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
        ClientEventItemChanged="OnDwMainItemChanged" ClientFormatting="True" TabIndex="1"
        ClientEventButtonClicked="OnDwMainButtonClicked" ClientEventClicked="OnDwMainClicked">
    </dw:WebDataWindowControl>
    <table style="width: 100%; border: solid 1px; margin-top: 5px">
        <tr align="center" class="dwtab">
            <td style="background-color: rgb(211,213,255); cursor: pointer;" id="stab_1" width="50%"
                onclick="showTabPage(1);">
                รายการเคลื่อนไหวสินค้า
            </td>
            <td style="background-color: rgb(200,235,255); cursor: pointer;" id="stab_2" width="50%"
                onclick="showTabPage(2);">
                รายการเคลื่อนไหว Lot สินค้า
            </td>
        </tr>
    </table>
    <table style="width: 100%; border: solid 1px; margin-top: 2px" class="dwcontent">
        <tr>
            <td style="height: 400px;" valign="top">
                <div id="tab_1" style="visibility: visible; position: absolute;">

                    <dw:WebDataWindowControl ID="DwDetail" runat="server" DataWindowObject="d_tradesrv_info_productstockcard"
                        LibraryList="~/DataWindow/trading/sheet_td_info_product_move.pbl" AutoRestoreContext="False"
                        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
                        ClientEventItemChanged="OnDwStocktemChanged" ClientFormatting="True" TabIndex="1000"
                        ClientEventButtonClicked="OnDwStockButtonClicked" Width="742px" Height="399px" RowsPerPage="13">
                    
           <PageNavigationBarSettings NavigatorType="NumericWithQuickGo" Visible="True">
                <BarStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="12px" />
                <QuickGoNavigator GoToDescription="หน้า:" />
            </PageNavigationBarSettings>
            </dw:WebDataWindowControl>
                </div>
                <div id="tab_2" style="visibility: hidden; position: absolute;">
                    <dw:WebDataWindowControl ID="DwTailer" runat="server" DataWindowObject="d_tradesrv_info_productlot"
                        LibraryList="~/DataWindow/trading/sheet_td_info_product_move.pbl" AutoRestoreContext="False"
                        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
                        ClientEventItemChanged="OnDwLotItemChanged" ClientFormatting="True" TabIndex="1000"
                        ClientEventButtonClicked="OnDwLotButtonClicked" Width="742px" Height="399px" RowsPerPage="13">
                   
            <PageNavigationBarSettings NavigatorType="NumericWithQuickGo" Visible="True">
                <BarStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="12px" />
                <QuickGoNavigator GoToDescription="หน้า:" />
            </PageNavigationBarSettings>
        </dw:WebDataWindowControl>
                </div>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="HiddenFieldTab" runat="server" />
</asp:Content>
