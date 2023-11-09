<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_kt_pensionpay.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_kt_pensionpay" %>


<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <%=postRetreiveDwMain%>
    <%=postPay%>

    <script type="text/javascript">

        function DwMainButtonClick(sender, rowNumber, buttonName) {

            if (buttonName == "b_1") {
                Gcoop.OpenDlg(610, 550, "w_dlg_kt_member_search_pension.aspx", "");
            }

        }
        function DwMain65ButtonClick(sender, rowNumber, buttonName) {

            if (buttonName == "b_pay") {
                postPay();
            }
        }

        function DwMainPItemChange(s, r, c, v) {
            objDwMainP.SetItem(r, c, v);
            objDwMainP.AcceptText();
            if (c == "member_no") {
                member_no = Gcoop.StringFormat(v, "00000000");
                objDwMainP.SetItem(r, c, member_no);
                Gcoop.GetEl("HdMemberNo").value = member_no;

                postRetreiveDwMain();

            }
        }

        function MenubarNew() {

            if (confirm("ยืนยันการล้างข้อมูลบนหน้าจอ")) {
                window.location = "";
            }
            return 0;
        }
        function MenubarOpen() {
            Gcoop.OpenDlg(610, 550, "w_dlg_kt_member_search_pension.aspx", "");
        }
        function GetValueFromDlg(memberNo) {
            objDwMainP.SetItem(1, "member_no", memberNo);
            objDwMainP.AcceptText();
            postRetreiveDwMain();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
<%--    <asp:Literal ID="LtAlert" runat="server"></asp:Literal>--%>

<br />
    <dw:WebDataWindowControl ID="DwMainP" runat="server" LibraryList="~/DataWindow/app_assist/kt_pension.pbl"
        DataWindowObject="d_kt_pay" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientEventButtonClicked="DwMainButtonClick" 
        ClientEventItemChanged="DwMainPItemChange" >
    </dw:WebDataWindowControl>
    <br />
        <dw:WebDataWindowControl ID="DwMain65" runat="server" LibraryList="~/DataWindow/app_assist/kt_65years.pbl"
        DataWindowObject="d_kt_pay" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientEventButtonClicked="DwMain65ButtonClick" 
        ClientEventItemChanged="DwMainItemChange" >
    </dw:WebDataWindowControl>
    
     <asp:HiddenField ID="HdMemberNo" runat="server" />
     <asp:HiddenField ID="HdDeadDate" runat="server" />
     <asp:HiddenField ID="HdMoneyType" runat="server" />
     <asp:HiddenField ID="HdBirthDate" runat="server" />
     <asp:HiddenField ID="HdDwRow" runat="server" />
     <asp:HiddenField ID="HdRowReceive" runat="server" />
     <asp:HiddenField ID="HdPayout" runat="server" />
</asp:Content>
