<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" 
CodeBehind="w_sheet_kt_slip_65year.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_kt_slip_65year" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=postslipClick%>
    <%= postslipDetail%>
    <%=postRetreiveDwMem%>
    <script type="text/javascript">
 
        function Validate() {

            objDwMemb.AcceptText();
            //objDwDetailP.AcceptText();

            var member_no = objDwMemb.GetItem(1, "member_no");

            if (member_no != null) {
                return confirm("ยืนยันการบันทึกข้อมูล");
            }
            else {
                alert("กรุณากรอกข้อมูลให้ครบ");
            }
        }
        function DwMemButtonClick(sender, rowNumber, buttonName) {

            if (buttonName == "b_slip") {
                postslipClick();
            }
            if (buttonName == "b_detail") {
                postslipDetail();
            }
        }

        function DwMemPItemChange(sender, rowNumber, columnName, newValue) {
            objDwMemb.SetItem(rowNumber, columnName, newValue);
            objDwMemb.AcceptText();
            if (columnName == "member_no") {
                newValue = Gcoop.StringFormat(newValue, "00000000");
                Gcoop.GetEl("HdMemberNo").value = newValue;
                objDwMemb.SetItem(rowNumber, columnName, newValue);
                objDwMemb.AcceptText();
                postRetreiveDwMem();
            }
        }
        function Alert() {
            alert("ไม่มีเลขที่สมาชิกนี้");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <asp:Literal ID="LtAlert" runat="server"></asp:Literal>

    <br />

    <dw:WebDataWindowControl ID="DwMtran" runat="server" LibraryList="~/DataWindow/app_assist/Kt_65years.pbl"
        DataWindowObject="d_kt_slip_transaction" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientEventButtonClicked="DwMemButtonClick" >
    </dw:WebDataWindowControl>
    <br />
    <dw:WebDataWindowControl ID="DwMemb" runat="server" LibraryList="~/DataWindow/app_assist/Kt_65years.pbl"
        DataWindowObject="d_kt_slip_member" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientEventItemChanged="DwMemPItemChange" >
    </dw:WebDataWindowControl>

    <dw:WebDataWindowControl ID="DwMain" runat="server" LibraryList="~/DataWindow/app_assist/Kt_65years.pbl"
        DataWindowObject="d_kt_slip_65year" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientEventClicked="DwDetailButtonClick" 
        ClientEventButtonClicked="DwDetailButtonClick" >
    </dw:WebDataWindowControl>


     <asp:HiddenField ID="HdDwRow" runat="server" />  
      <asp:HiddenField ID="HdMemberNo" runat="server" />
      <asp:HiddenField ID="HdOpenIFrame" runat="server" /> 
</asp:Content>
