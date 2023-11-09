<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" 
CodeBehind="w_sheet_as_cal_burary.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_as_cal_burary" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <%=initJavaScript%>
     <%=postCalContribute %>
     <%=postCalEncourage %>
     <%=postCalOutStanding%>
     <%=postUpdateVarOutStanding %>
     <%=postUpdateVarEncourage %>
     <%=postUpdateVarContribute%>
     <%=postTranOutStanding %>
     <%=postTranEncourage %>
     <%=postTranContribute %>
     <%=postCancelTranEncourage %>
     <%=postCancelTranContribute%>
    <script type="text/javascript">
        function MenubarNew() {
            window.location = "";
        }
        function UpdateVarOutStanding() {
            postUpdateVarOutStanding();
        }
        function UpdateVarEncourage() {
            postUpdateVarEncourage();
        }
        function UpdateVarContribute() {
            postUpdateVarContribute();
        }
        function OnDwOutStandingButtonClicked(sender, rowNumber, buttonName) {
            if (confirm("ยืนยันการทำรายการ")) {
                if (buttonName == "btn_calo") {
                    objdw_outstanding.AcceptText();
                    postCalOutStanding();
                }
                else if (buttonName == "btn_trno") {
                    objdw_outstanding.AcceptText();
                    postTranOutStanding();
                }
            }
        
        }
        function OnDwEncourageButtonClicked(sender, rowNumber, buttonName) {
            if (confirm("ยืนยันการทำรายการ")) {
                if (buttonName == "btn_cale") {
                    objdw_encourage.AcceptText();
                    postCalEncourage();
                }
                else if (buttonName == "btn_trne") {
                    objdw_encourage.AcceptText();
                    postTranEncourage();
                }
                else if (buttonName == "btn_cancel_trne") {
                    objdw_encourage.AcceptText();
                    postCancelTranEncourage();
                }
            }
        }
        function OnDwContributeButtonClicked(sender, rowNumber, buttonName) {
            if (confirm("ยืนยันการทำรายการ")) {
                if (buttonName == "btn_calc") {
                    objdw_contribute.AcceptText();
                    postCalContribute();
                }
                else if (buttonName == "btn_trnc") {
                    objdw_contribute.AcceptText();
                    postTranContribute();
                }
                else if (buttonName == "btn_cancel_tranc") {
                    objdw_contribute.AcceptText();
                    postCancelTranContribute();
                } 
            }
        }
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <asp:Literal ID="LtAlert" runat="server"></asp:Literal>
     <asp:Label ID="Label1" runat="server" Text="ทุนรางวัลการศึกษาดีเด่น"></asp:Label>
     
     <span onclick="UpdateVarOutStanding()" style="cursor: pointer; margin-left: 44%"">
     <asp:Label ID="Label4" runat="server"
     Text="บันทึกข้อมูล" Font-Underline="True" Font-Size="14 px"></asp:Label></span>

      <dw:WebDataWindowControl ID="dw_outstanding" runat="server" LibraryList="~/DataWindow/app_assist/as_public_funds.pbl"
            DataWindowObject="d_as_cal_outstanding_burary" ClientScriptable="True" AutoRestoreContext="false"
            AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientEventButtonClicked="OnDwOutStandingButtonClicked"
            ClientFormatting="True" ClientEventItemChanged="DwMainItemChange" TabIndex="250">
        </dw:WebDataWindowControl>

     <asp:Label ID="Label2" runat="server" Text="ทุนส่งเสริมการศึกษา"></asp:Label>

     <span onclick="UpdateVarEncourage()" style="cursor: pointer; margin-left: 47.5%";>
     <asp:Label ID="Label5" runat="server"
     Text="บันทึกข้อมูล" Font-Underline="True" Font-Size="14 px"></asp:Label></span>

      <dw:WebDataWindowControl ID="dw_encourage" runat="server" LibraryList="~/DataWindow/app_assist/as_public_funds.pbl"
            DataWindowObject="d_as_cal_encourage_burary" ClientScriptable="True" AutoRestoreContext="false"
            AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientEventButtonClicked="OnDwEncourageButtonClicked"
            ClientFormatting="True" ClientEventItemChanged="DwMainItemChange" TabIndex="250">
        </dw:WebDataWindowControl>
    <asp:Label ID="Label3" runat="server" Text="ทุนอุดหนุนการศึกษา" Visible="false"></asp:Label>

         <span onclick="UpdateVarContribute()" style="cursor: pointer; margin-left: 47.5%"">
     <asp:Label ID="Label6" runat="server"
     Text="บันทึกข้อมูล" Font-Underline="True" Font-Size="14 px" Visible="false"></asp:Label></span>

    <dw:WebDataWindowControl ID="dw_contribute" runat="server" LibraryList="~/DataWindow/app_assist/as_public_funds.pbl"
            DataWindowObject="d_as_cal_contribute_burary" ClientScriptable="True" AutoRestoreContext="false"
            AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientEventButtonClicked="OnDwContributeButtonClicked"
            ClientFormatting="True" ClientEventItemChanged="DwMainItemChange" TabIndex="250" Visible="false">
        </dw:WebDataWindowControl>
</asp:Content>
