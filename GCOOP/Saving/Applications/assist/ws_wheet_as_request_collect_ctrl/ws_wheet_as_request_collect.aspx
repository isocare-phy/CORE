<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="ws_wheet_as_request_collect.aspx.cs" Inherits="Saving.Applications.assist.ws_wheet_as_request_collect_ctrl.ws_wheet_as_request_collect" %>
<%@ Register Src="DsMain.ascx" TagName="DsMain" TagPrefix="uc1" %>
<%@ Register Src="DsDetail.ascx" TagName="DsDetail" TagPrefix="uc2" %>
<%@ Register Src="DsSequence.ascx" TagName="DsSequence" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <script type="text/javascript">
     var dsMain = new DataSourceTool;
     var dsDetail = new DataSourceTool;
     var dsSequence = new DataSourceTool;
     function Validate() {
         return confirm("ยืนยันการบันทึกข้อมูล");
     }
     function OnDsMainItemChanged(s, r, c, v) {
         if (c == "member_no") {
             PostRetrivememno();
         }
     }
     function OnDsMainClicked(s, r, c) {
         if (c == "b_search") {
             Gcoop.OpenIFrame2(650, 600, 'w_dlg_sl_member_search_tks.aspx', '')
         }
     }

     function GetMembNoFromDlg(memberno) {
         dsMain.SetItem(0, "member_no", memberno.trim());
         PostRetrivememno();
     }
     function OnDsSequenceItemChanged(s, r, c) {

     }
     function OnDsSequenceClicked(s, r, c) {

     }
     function OnDsDetailItemChanged(s, r, c) {
        if (c == "resign_date") {

            PostCalAgain();

         }
         else if (c == "monthcollect") {

         PostCalAgain2();

         }

     }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <uc1:DsMain ID="dsMain" runat="server" />
    <uc2:DsDetail ID="dsDetail" runat="server" />
<uc3:DsSequence ID="dsSequence" runat="server" />
</asp:Content>

