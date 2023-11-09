<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_ln_cancel_payspecial.aspx.cs" Inherits="Saving.Applications.loan.w_sheet_ln_cancel_payspecial" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=jsPostMemberNo%>
    <script type="text/JavaScript">
        function Validate() {
            objDwMain.AcceptText();
            objDwDetail.AcceptText();
            return confirm("ยืนยันการบันทึกข้อมูล");
        }
        function MenubarOpen() {
            Gcoop.OpenIFrame("630", "350", "w_dlg_member_search_new.aspx", "");
        }
        function ReceiveMemberNo(member_no, memb_name) {
            objDwMain.SetItem(1, "member_no", member_no);
            objDwMain.AcceptText();
            jsPostMemberNo();
        }
        function OnDwMainItemChange(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            if (c == "member_no") {
                jsPostMemberNo();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_lcwin_memdet"
        LibraryList="~/DataWindow/investment/loan_slippayin.pbl" ClientScriptable="True" ClientEvents="true"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        AutoRestoreContext="False" ClientEventItemChanged="OnDwMainItemChange" ClientEventButtonClicked="MenubarOpen">
    </dw:WebDataWindowControl>
    <hr />
        <dw:WebDataWindowControl ID="DwDetail" runat="server" DataWindowObject="d_lcsrv_list_cclpayin"
        LibraryList="~/DataWindow/investment/loan.pbl" ClientScriptable="True" ClientEvents="true"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        AutoRestoreContext="False">
    </dw:WebDataWindowControl>
    <br />
</asp:Content>
