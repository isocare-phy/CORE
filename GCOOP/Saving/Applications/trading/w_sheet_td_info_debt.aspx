<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" 
CodeBehind="w_sheet_td_info_debt.aspx.cs" Inherits="Saving.Applications.trading.w_sheet_td_info_debt" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%=jsPostDebtNo %>
<%=jsPostMembNo%> 

<%=postGetDistrict%>   
    <script type="text/javascript">
        function Validate() { // บันทึก
            return confirm("ต้องการบันทึกหรือไม่");
        }

        function MenubarOpen() {
            Gcoop.OpenIFrame("740", "600", "w_dlg_td_search_debt.aspx", "");
        }

        function OnDwMainItemChanged(s, r, c, v) {
            switch (c) {
                case "debt_no":
                    s.SetItem(r, c, v);
                    s.AcceptText();
                    jsPostDebtNo();
                    break;
                case "debt_province":
                    objDwMain.SetItem(1, "debt_province", v);
                    //Gcoop.GetEl("HfProvince").value = v;
                    //objDwMain.AcceptText();
                    postGetDistrict();
                    break;
            }
        }

        function GetDlgDebtNo(debt_no) {
            objDwMain.SetItem(1, "debt_no", debt_no);
            jsPostDebtNo();
        }

        function GetDlgMemberNo(member_no) {
            objDwMain.SetItem(1, "member_no", member_no);
            jsPostMembNo();
        }
        function OnDwMainButtonClick(s, r, c) {
            switch (c) {
                case "b_member":
                    var member_no = ""
                    try {
                        member_no = objDwMain.GetItem(1, "member_no");
                    } catch (err) { }
                    Gcoop.OpenIFrame("900", "600", "w_dlg_td_search_memb.aspx", "?member_status=1");
                    break;
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_tradesrv_info_debt"
        LibraryList="~/DataWindow/trading/sheet_td_info_debt.pbl" AutoRestoreContext="False"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
        ClientEventItemChanged="OnDwMainItemChanged" ClientFormatting="True" TabIndex="1"
        ClientEventButtonClicked="OnDwMainButtonClick" ClientEventClicked="OnDwMainClick">
    </dw:WebDataWindowControl>

    <dw:WebDataWindowControl ID="DwStatement" runat="server" DataWindowObject="d_tradesrv_info_debtdec_stmt"
        LibraryList="~/DataWindow/trading/sheet_td_info_debt.pbl" AutoRestoreContext="False"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
        ClientEventItemChanged="OnDwStatementItemChanged" ClientFormatting="True" TabIndex="1"
        ClientEventButtonClicked="OnDwStatementButtonClicked" ClientEventClicked="OnDwStatementClicked"
        Width="742px" Height="399px" RowsPerPage="13">
        
           <PageNavigationBarSettings NavigatorType="NumericWithQuickGo" Visible="True">
                <BarStyle Font-Bold="True" Font-Names="Tahoma" Font-Size="12px" />
                <QuickGoNavigator GoToDescription="หน้า:" />
            </PageNavigationBarSettings>
    </dw:WebDataWindowControl>
</asp:Content>
