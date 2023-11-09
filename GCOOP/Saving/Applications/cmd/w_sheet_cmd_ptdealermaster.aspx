<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_cmd_ptdealermaster.aspx.cs" 
Inherits="Saving.Applications.cmd.w_sheet_cmd_ptdealermaster" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=jsPostDealer%>
    <script type="text/javascript">
        function Validate() {
            var dealer_name = objDwMain.GetItem(1, "dealer_name");
            var dealer_taxid = objDwMain.GetItem(1, "dealer_taxid");

            if (dealer_name == "" || dealer_name == null) {
                alert("กรุณาระบุชื่อคู่ค้า !!!");
                return;
            }
            if (dealer_taxid == "" || dealer_taxid == null) {
                alert("กรุณาระบุเลขที่เสียภาษีคู่ค้า !!!");
                return;
            }
            if (confirm("ยืนยันการบันทึกข้อมูล")) {
                return true;
            } else {
                return false;
            }
        }

        function MenubarOpen() {
            Gcoop.OpenDlg(600, 500, "w_dlg_cmd_dealermaster.aspx", "");
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
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_main_dealermaster"
        LibraryList="~/DataWindow/Cmd/cmd_dealermaster.pbl" Width="750px" ClientScriptable="True"
        AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" ClientEventButtonClicked="OnDwMainClicked" ClientEventItemChanged="OnDwMainItemChange">
    </dw:WebDataWindowControl>
</asp:Content>
