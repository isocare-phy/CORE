<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" 
CodeBehind="w_sheet_td_info_cred.aspx.cs" Inherits="Saving.Applications.trading.w_sheet_td_info_cred" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=jsPostCredNo %>
    <%=jsPostMembNo%>    
    <%=jsDwDetailInsertRow %>
    <%=postGetDistrict%>

    <script type="text/javascript">
        function Validate() { // บันทึก
            return confirm("ต้องการบันทึกหรือไม่");
        }

        function MenubarOpen() {
            Gcoop.OpenIFrame("740", "600", "w_dlg_td_search_cred.aspx", "");
        }

        function OnDwMainItemChanged(s, r, c, v) {
            switch (c) {
                case "cred_no":
                    s.SetItem(r, c, v);
                    s.AcceptText();
                    jsPostCredNo();
                    break;
                case "cred_province":
                    objDwMain.SetItem(1, "cred_province", v);
                    Gcoop.GetEl("HfProvince").value = v;
                    objDwMain.AcceptText();
                    postGetDistrict();
                    break;
            }
        }

        function GetDlgCredNo(cred_no) {
            objDwMain.SetItem(1, "cred_no", cred_no);
            jsPostCredNo();
        }
        
        function GetDlgMemberNo(member_no) {
            objDwMain.SetItem(1, "member_no", member_no);
            jsPostMembNo();
        }

        function OnDwMainButtonClick(s, r, c) {
            switch (c) {
                case "b_sch_creb":
                    Gcoop.OpenIFrame("740", "600", "w_dlg_td_search_cred.aspx", "");
                    break;
                case "b_member":
                    var member_no = ""
                    try {
                        member_no = objDwMain.GetItem(1, "member_no");
                    } catch (err) { }
                    Gcoop.OpenIFrame("900", "600", "w_dlg_td_search_memb.aspx", "?member_status=1");
                    break;                    
            }
        }
        function DwDetailInsertRow() {
            jsDwDetailInsertRow();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
<asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_tradesrv_info_cred"
        LibraryList="~/DataWindow/trading/sheet_td_info_cred.pbl" AutoRestoreContext="False"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
        ClientEventItemChanged="OnDwMainItemChanged" ClientFormatting="True" TabIndex="1"
        ClientEventButtonClicked="OnDwMainButtonClick" ClientEventClicked="OnDwMainClick">
    </dw:WebDataWindowControl>
<%--    <asp:Label ID="Label3" runat="server" Text="เพิ่มแถว" onclick="DwDetailInsertRow();"
        Font-Size="Medium" Font-Underline="True" ForeColor="#0000CC"></asp:Label>--%>
    <dw:WebDataWindowControl ID="DwStatement" runat="server" DataWindowObject="d_tradesrv_info_creddec_stmt"
        LibraryList="~/DataWindow/trading/sheet_td_info_cred.pbl" AutoRestoreContext="False"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
        ClientEventItemChanged="OnDwStatementItemChanged" ClientFormatting="True" TabIndex="1"
        ClientEventButtonClicked="OnDwStatementButtonClicked" ClientEventClicked="OnDwStatementClicked"
        Width="742px" Height="399px">
    </dw:WebDataWindowControl>
    <asp:HiddenField ID="HfProvince" runat="server" />
</asp:Content>
