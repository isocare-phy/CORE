<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_td_fin_apcredit.aspx.cs" Inherits="Saving.Applications.w_sheet_td_fin_apcredit" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=jsCredNo %>
    <%=jsPostSlip %>
    <%=jsDwDetailInsertRow %>
    <%=jsChickChkAll %>
    <script type="text/javascript">
        function Validate() { // บันทึก
            return confirm("ต้องการบันทึกหรือไม่");
        }

        function OnDwMainButtonClicked(s, r, c) {
            switch (c) {
                case "b_2":
                    Gcoop.OpenIFrame("740", "600", "w_dlg_td_search_cred.aspx", "");
                    break;
            }
        }

        function GetDlgCredNo(cred_no) {
            objDwMain.SetItem(1, "cred_no", cred_no);
            jsCredNo();
        }

        function MenubarOpen() {
            Gcoop.OpenIFrame("740", "600", "w_dlg_td_search_slip_apcredit.aspx", "?credittype_code=APC");
        }
        
        function GetDlgSlipNoApc(slip_no) {
            objDwMain.SetItem(1, "creditdoc_no", slip_no);
            jsPostSlip();
        }
        
        function DwDetailInsertRow() {
            jsDwDetailInsertRow();
        }

        function OnDwMainItemChanged(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            switch (c) {
                case "cred_no":
                    jsCredNo();
                    break;
            }
        }

        function ChickChkAll() {
            jsChickChkAll();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_tradesrv_fin_apcredit_header"
        LibraryList="~/DataWindow/trading/sheet_td_fin_apcredit.pbl" AutoRestoreContext="False"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
        ClientEventItemChanged="OnDwMainItemChanged" ClientFormatting="True" TabIndex="0"
        ClientEventButtonClicked="OnDwMainButtonClicked" ClientEventClicked="OnDwMainClicked">
    </dw:WebDataWindowControl>
    <asp:Label ID="Label3" runat="server" Text="เพิ่มแถว" onclick="DwDetailInsertRow();"
        Font-Size="Medium" Font-Underline="True" ForeColor="#0000CC"></asp:Label>
    <br />
    <input type="checkbox" id="chkAll" runat="server" onchange="ChickChkAll();" /> เลือกทั้งหมด
    <dw:WebDataWindowControl ID="DwDetail" runat="server" DataWindowObject="d_tradesrv_fin_apcredit_detail"
        LibraryList="~/DataWindow/trading/sheet_td_fin_apcredit.pbl" AutoRestoreContext="False"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
        ClientEventItemChanged="OnDwDetailItemChanged" ClientFormatting="True" TabIndex="50"
        ClientEventButtonClicked="OnDwDetailButtonClicked" ClientEventClicked="OnDwDetailClicked"
        Width="742px" Height="399px">
    </dw:WebDataWindowControl>
</asp:Content>
