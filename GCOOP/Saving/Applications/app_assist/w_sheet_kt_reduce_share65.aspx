<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" 
CodeBehind="w_sheet_kt_reduce_share65.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_kt_reduce_share65" %>


<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=postRetreiveDwMem %>
    <%=postPayClick%>
    <script type="text/javascript">
        function MenubarOpen() {
            Gcoop.OpenDlg(610, 550, "w_dlg_kt_member_search_pension.aspx", "");

        }
        function Validate() {

            objDwMainP.AcceptText();
            objDwDetailP.AcceptText();

            var member_no = objDwMemP.GetItem(1, "member_no");
            var assist_amt = objDwDetailP.GetItem(1, "assist_amt");

            if (member_no != null && assist_amt != null) {
                return confirm("ยืนยันการบันทึกข้อมูล");
            }
            else {
                alert("กรุณากรอกข้อมูลให้ครบ");
            }

        }
        function DwMemButtonClick(sender, rowNumber, buttonName) {

            if (buttonName == "b2_search") {
                Gcoop.OpenDlg(610, 550, "w_dlg_kt_member_search_pension.aspx", "");
            }
        }
        function DwDetailButtonClick(sender, rowNumber, buttonName) {

            if (buttonName == "b_pay") {
                postPayClick();
            }
        }

        function DwMemPItemChange(sender, rowNumber, columnName, newValue) {
            objDwMemP.SetItem(rowNumber, columnName, newValue);
            objDwMemP.AcceptText();

            if (columnName == "member_no") {
                newValue = Gcoop.StringFormat(newValue, "00000000");
                Gcoop.GetEl("HdMemberNo").value = newValue;

                postRetreiveDwMem();

            }
        }

        function Alert() {
            alert("ไม่มีเลขที่สมาชิกนี้");
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
        function GetValueFromDlg(memberno) {

            Gcoop.GetEl("HdMemberNo").value = memberno;
            //            Gcoop.GetEl("HdDeadDate").value = resign_date.toString();
            postRetreiveDwMem();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <asp:Literal ID="LtAlert" runat="server"></asp:Literal>

    <dw:WebDataWindowControl ID="DwMemP" runat="server" LibraryList="~/DataWindow/app_assist/kt_65years.pbl"
        DataWindowObject="d_kt_reqmember" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientEventButtonClicked="DwMemButtonClick" 
        ClientEventItemChanged="DwMemPItemChange" >
    </dw:WebDataWindowControl>

    <dw:WebDataWindowControl ID="DwMainP" runat="server" LibraryList="~/DataWindow/app_assist/kt_65years.pbl"
        DataWindowObject="d_kt_reqmaster_reduce" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        >
    </dw:WebDataWindowControl>

    <dw:WebDataWindowControl ID="DwDetailP" runat="server" LibraryList="~/DataWindow/app_assist/kt_65years.pbl"
        DataWindowObject="d_kt_reqmem_reduce" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientEventClicked="DwDetailButtonClick" 
        ClientEventButtonClicked="DwDetailButtonClick" >

    </dw:WebDataWindowControl>

     <asp:HiddenField ID="HdMemberNo" runat="server" />
     <asp:HiddenField ID="HdShareCk" runat="server" />
     <asp:HiddenField ID="HdMoneyType" runat="server" />
     <asp:HiddenField ID="HdBirthDate" runat="server" />
     <asp:HiddenField ID="HdDwRow" runat="server" />
     <asp:HiddenField ID="HdRowReceive" runat="server" />
     <asp:HiddenField ID="HdPayout" runat="server" />
</asp:Content>
