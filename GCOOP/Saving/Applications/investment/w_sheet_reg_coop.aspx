<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" 
CodeBehind="w_sheet_reg_coop.aspx.cs" Inherits="Saving.Applications.investment.w_sheet_reg_coop" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%=initJavaScript%>
<%=jsInsertRow %>
<%=jsOpenRow %>
<%=jsPostDistrict %>
<%=jsPostProvince %>
  <script type="text/JavaScript">
      function Validate() {
          objDwMain.AcceptText();
          return confirm("ยืนยันการบันทึกข้อมูล");
      }
      function OnDwMainItemChanged(sender, row, col, Value) {
          switch (col) {
              case "district_code":
                  sender.SetItem(row, col, Value);
                  sender.AcceptText();
                  jsPostDistrict();
                  break;
              case "province_code":
                  sender.SetItem(row, col, Value);
                  sender.AcceptText();
                  jsPostProvince();
                  break;
          }
      }
      function MenubarOpen() {
          Gcoop.OpenIFrame("600", "600", "w_dlg_member_search_new.aspx", "");
      }
      function ReceiveMemberNo(member_no, memb_name) {
          objDwMain.SetItem(1, "member_no", member_no);
          objDwMain.AcceptText();
          jsOpenRow();
      }
   </script>
  </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
     <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_lcwin_member"
        LibraryList="~/DataWindow/investment/int_lcmember.pbl" ClientScriptable="True"
        AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True"  ClientEventItemChanged="OnDwMainItemChanged">
    </dw:WebDataWindowControl>
</asp:Content>
