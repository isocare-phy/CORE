<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_cmd_local.aspx.cs"
    Inherits="Saving.Applications.cmd.w_sheet_cmd_local" Title="Untitled Page" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=newClear%>
    <%=Refresh %>
    <%=postRetrieveList %>
    <%=postRetrieveMain %>
    <%=initJavaScript %>
    <%=jsPostblank %>

    <script type="text/javascript">

        function DwLocalclick(sender, rowNumber, objectName) {
            var product_id = objDw_Local.GetItem(rowNumber, "product_id");
            var lot_id = objDw_Local.GetItem(rowNumber, "lot_id");

            Gcoop.GetEl("HLotid").value = lot_id;
            Gcoop.GetEl("HfproductId").value = product_id;

            postRetrieveMain();
        }

        function MenubarOpen() {
            Gcoop.OpenDlg(650, 550, "w_dlg_cmd_search_local.aspx", "");
        }

        function GetValueFromDlg(create_id) {
            //  alert(create_id);
            Gcoop.GetEl("Hfcreate_id").value = create_id;

            postRetrieveList();
        }

        function MenubarNew() {
            newClear();
        }

        function Validate() {
//            var alertstr = "";
//            var lot_id = objDw_main.GetItem(1, "lot_id");
//            var lotend = objDw_main.GetItem(1, "lotend");
//            var locate_stock = objDw_main.GetItem(1, "locate_stock");

//            if (lot_id == "" || lot_id == null) { alertstr = alertstr + "-กรุณาเลือก lot แรก\n"; }
//            if (lotend == "" || lotend == null) { alertstr = alertstr + "-กรุณาเลือก lot สุดท้าย\n"; }
//            if (locate_stock == "" || locate_stock == null) { alertstr = alertstr + "- กรุณากรอกสถานที่เก็บ\n"; }

//            if (alertstr == "") {
                return true;
//            } else {
//                alert(alertstr);
//                return false;
//            }
            //            objDw_main.AcceptText();
            //            objDw_Local.AcceptText();
            //            return confirm("ยืนยันการบันทึกข้อมูล");
        }




        //        function GetProductFromDlg(create_id, product_id) {


        //            Gcoop.GetEl("HfproductId").value = product_id;
        //            postRetrieveList();

        //        }
        function GetProductFromDlg(create_id, product_id, product_name, unit_name) {
            objDw_main.SetItem(1, "create_id", create_id);
            objDw_main.SetItem(1, "product_id", product_id);
            objDw_main.SetItem(1, "item_list", product_name);
            objDw_main.SetItem(1, "unit_name", unit_name);
            Gcoop.GetEl("HfproductId").value = product_id;
            Gcoop.GetEl("Hfcreate_id").value = create_id;
            // alert(product_id);
            postRetrieveList();
            //Refresh();
        }

        function b_search_click(sender, rowNumber, buttonName) {
            if (buttonName == "b_search") {
                var chb_1 = "01";
                Gcoop.OpenDlg(590, 570, "w_dlg_st_product_searchimp.aspx", "?ch_b=" + chb_1);
            }
            return 0;
        }

        function OnDwmainItemChanged(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            if (c == "move_tday" || c == "entry_tdate") {
                jsPostblank();
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <table style="width: 100%;">
        <tr>
            <td valign="top" align="left">
                <%--<dw:WebDataWindowControl ID="Dw_Local" runat="server" DataWindowObject="dw_local_dea"
                    LibraryList="~/DataWindow/Cmd/cmd_local.pbl" Width="250px" ClientScriptable="True"
                    ClientEventClicked="DwLocalclick" AutoRestoreContext="False" AutoRestoreDataCache="True"
                    AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True">
                </dw:WebDataWindowControl>--%>
                <asp:HiddenField ID="Hiddw_local" runat="server" />
            </td>
            <td valign="top" align="left">
                <dw:WebDataWindowControl ID="Dw_main" runat="server" DataWindowObject="dw_cmd_local_dea"
                    LibraryList="~/DataWindow/Cmd/cmd_local.pbl" Width="450px" ClientScriptable="True"
                    UseCurrentCulture="True" ClientEventButtonClicked="b_search_click" AutoRestoreContext="False"
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
                    ClientEventItemChanged="OnDwmainItemChanged">
                </dw:WebDataWindowControl>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="HfproductId" runat="server" />
    <asp:HiddenField ID="Hfcreate_id" runat="server" />
    <asp:HiddenField ID="HLotid" runat="server" />
</asp:Content>
