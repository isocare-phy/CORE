<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_cmd_ptdurtrepair.aspx.cs" 
Inherits="Saving.Applications.cmd.w_sheet_cmd_ptdurtrepair" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%=jsPostFindShow%>
<%=jsPostDealer%>
<%=jsPostCheckrepairstatus%>
<%=jsPostReqrepairno%>
<%=jsReqrepairToContract%>
<%=jsPostPrint%>
    <script type="text/javascript">
        function MenubarOpen() {
            Gcoop.OpenDlg(790, 570, "w_dlg_st_product_search_repair.aspx", "");
        }

        function MenubarNew() {
            window.location = state.SsUrl + "Applications/cmd/w_sheet_cmd_ptdurtrepair.aspx";
        }

        function Validate() {
            var repair_detail = objDwMain.GetItem(1, "repair_detail");
            return confirm("ยืนยันการบันทึกข้อมูล");
        }

        function OnDwMainChange(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            switch (c) {
                case "durt_id":
                    jsPostFindShow();
                    break;
                case "dealer_no":
                    jsPostDealer();
                    break;
                case "repair_status":
                    jsPostCheckrepairstatus();
                    break;
            }
        }

        function OnDwMainClick(s, r, c) {
            if (c == "b_durtid") {
                Gcoop.GetEl("HdR").value = r;
                Gcoop.OpenDlg("750", "570", "w_dlg_cmd_ptdurtmaster.aspx", "");
            } else if (c == "b_dealer") {
                Gcoop.OpenDlg(600, 500, "w_dlg_cmd_dealermaster.aspx", "");
            } else if (c == "b_copy") {
                jsReqrepairToContract();
            }
        }

        function OnFindShow(durt_id, durt_name) {
            Gcoop.GetEl("HdDurtId").value = durt_id;
            objDwMain.SetItem(1, "durt_id", durt_id);
            objDwMain.AcceptText();
            jsPostFindShow();
        }

        function GetDlgDealer(dealerNo) {
            objDwMain.SetItem(1, "dealer_no", dealerNo);
            objDwMain.AcceptText();
            jsPostDealer();
        }

        function GetReqrepairNoFromDlg(reqrepair_no) {
            objDwMain.SetItem(1, "reqrepair_no", reqrepair_no);
            objDwMain.AcceptText();
            jsPostReqrepairno();
        }

        function GetDlgContractDealer(dealer_contractno, contract_phone, contract_email) {
            objDwMain.SetItem(1, "contract_name", dealer_contractno);
            objDwMain.SetItem(1, "contract_telmail", contract_phone);
            if (contract_email != null && contract_email != "") {
                objDwMain.SetItem(1, "repairman_email", contract_email);
            }
            objDwMain.AcceptText();
        }

        function SheetLoadComplete() {
            var StatusSave = Gcoop.GetEl("HdState").value;
            var Ckdeal = Gcoop.GetEl("HdCkdeal").value;
            if (StatusSave == "Update") {
                $("#bprint").show();
            } else {
                $("#bprint").hide();
            }

            if (Ckdeal == "true") {
                Gcoop.OpenDlg(700, 570, "w_dlg_cmd_contractdealer.aspx", "?dealer_no=" + Gcoop.GetEl("HdDealerNo").value);
            }
        }

        function OnClickPrint() {
            jsPostPrint();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
<asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <input type="button" id="bprint" value="พิมพ์" style="width: 80px; display:none;" onclick="OnClickPrint()"/>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_ptreqrepair_main" 
        LibraryList="~/DataWindow/Cmd/cmd_ptdurtrepair.pbl" Width="750px" ClientScriptable="True"
        AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" ClientEventButtonClicked="OnDwMainClick" ClientEventItemChanged="OnDwMainChange">
    </dw:WebDataWindowControl>
    <asp:HiddenField ID="HdR" runat="server" />
    <asp:HiddenField ID="HdDurtId" runat="server" />
    <asp:HiddenField ID="HdState" runat="server" />
    <asp:HiddenField ID="HdReqrepairNo" runat="server" />
    <asp:HiddenField ID="HdDealerNo" runat="server" />
    <asp:HiddenField ID="HdCkdeal" runat="server" Value="false" />
</asp:Content>
