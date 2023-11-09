<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" 
CodeBehind="w_sheet_ent_coop_save.aspx.cs" Inherits="Saving.Applications.investment.w_sheet_ent_coop_save" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%=initJavaScript%>
<%=jsInsertRow %>
<%=jsInitRow %>
<%=jsOpenRow %>
<%=jsDwDetailInsertRow%>
<script type="text/JavaScript">
    function Validate() {
        objDwMain.AcceptText();
        objDwDetail.AcceptText();
        return confirm("ยืนยันการบันทึกข้อมูล");
    }
    function OnDwMainItemChanged(sender, row, col, Value) {
        switch (col) {
            case "biz_year":
                sender.SetItem(row, col, Value);
                sender.AcceptText();
                jsInitRow();
                break;
        }
    }
    function MenubarOpen() {
        Gcoop.OpenIFrame("600", "600", "w_dlg_search_coop.aspx", "");
    }
    function ReceivePreNameCode(member_no, coop_id) {
        objDwMain.SetItem(1, "member_no", member_no);
        objDwMain.SetItem(1, "coop_id", coop_id);
       jsInitRow();
    }
    function OnDwMainClick(s, r, c) {
        if (c == "btn_search") {
            Gcoop.OpenIFrame("600", "600", "w_dlg_search_coop.aspx", "");
        }
    }
   </script>
  </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
     <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
                <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_lc_member_yearbiz"
                LibraryList="~/DataWindow/investment/int_lcmember.pbl" ClientScriptable="True"
                AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
                ClientFormatting="True"  ClientEventItemChanged="OnDwMainItemChanged" ClientEventClicked="OnDwMainClick">
                </dw:WebDataWindowControl>
                <asp:Label ID="Label1" runat="server" Text="เพิ่มแถว" onclick="jsDwDetailInsertRow();"
                        Font-Size="Small" Font-Underline="True" ForeColor="#0000CC"></asp:Label>
                <dw:WebDataWindowControl ID="DwDetail" runat="server" DataWindowObject="d_lc_member_yearboard"
                LibraryList="~/DataWindow/investment/int_lcmember.pbl" ClientScriptable="True"
                AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
                ClientFormatting="True" Height="500" width="760" RowsPerPage="5">
                    <PageNavigationBarSettings Position="Bottom" Visible="True" NavigatorType="Numeric">
                    <BarStyle HorizontalAlign="Center" />
                    <NumericNavigator FirstLastVisible="True" />
                    </PageNavigationBarSettings>
                </dw:WebDataWindowControl>
</asp:Content>
