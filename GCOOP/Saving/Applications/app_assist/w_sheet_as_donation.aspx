<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_as_donation.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_as_donation" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=postRefresh %>
    <%=postRetreiveDwMem %>
    <%=postRetrieveDwMain %>
    <%=postGetMemberDetail %>
    <%=postRetrieveBankBranch %>
    <script type="text/javascript">

        function Validate() {
            objDwMain.AcceptText();

            //            //var member_no = objDwMem.GetItem(1, "member_no");

            //            var capital_year = objDwMain.GetItem(1, "capital_year");
            //            //var salary_amt = objDwMain.GetItem(1, "salary_amt");
            //            var assist_amt = objDwDetail.GetItem(1, "assist_amt");
            //            var req_date = objDwDetail.GetItem(1, "req_date");

            //            var donate_to = objDwDetail.GetItem(1, "donate_to");
            //            var donate_date = objDwDetail.GetItem(1, "donate_date");

            //            if (member_no != null && capital_year != null && assist_amt != null && req_date != null && donate_to != null && donate_date != null) {
            return confirm("ยืนยันการบันทึกข้อมูล");
            //            }
            //            else {
            //                alert("กรุณากรอกข้อมูลให้ครบ");
            //            }
        }

        function MenubarOpen() {
            Gcoop.OpenDlg(570, 400, "w_dlg_as_donation_list.aspx", "");
        }

        function DwMemButtonClick(sender, rowNumber, buttonName) {
            if (buttonName == "b_search") {
                Gcoop.OpenDlg(610, 550, "w_dlg_sl_member_search.aspx", "");
            }
        }

        function GetValueFromDlg(memberno, pre_name, memb_name, memb_surname, membgroup_desc, membgroup_code) {
            objDwMem.SetItem(1, "member_no", memberno);
            Gcoop.GetEl("HfMemberNo").value = memberno;
            postRetreiveDwMem();
        }

        function GetValueFromDlgList(assist_docno, capital_year, member_no) {
            //objDwMem.SetItem(1, "member_no", memberno);
            Gcoop.GetEl("HfMemNo").value = member_no;
            Gcoop.GetEl("HfAssistDocNo").value = assist_docno;
            Gcoop.GetEl("HfCapitalYear").value = capital_year;
            postRetrieveDwMain();
        }

        function DwMainItemChange(sender, rowNumber, columnName, newValue) {
            objDwMain.SetItem(rowNumber, columnName, newValue);
            objDwMain.AcceptText();
            if (columnName == "assist_amt") {
                //objDwSchool.SetItem(1, "assist_amt", newValue);
                postRefresh();
            }
            else if (columnName == "expense_bank") {
                objDwMain.AcceptText();
                postRetrieveBankBranch();
            }
        }

        function DwMemItemChange(sender, rowNumber, columnName, newValue) {
            objDwMem.SetItem(rowNumber, columnName, newValue);
            objDwMem.AcceptText();
            if (columnName == "member_no") {
                newValue = Gcoop.StringFormat(newValue, "000000");
                Gcoop.GetEl("HfMemberNo").value = newValue;
                postGetMemberDetail();
            }
        }

        function DwDonateItemChange(sender, rowNumber, columnName, newValue) {
            objDwDetail.SetItem(rowNumber, columnName, newValue);
            objDwDetail.AcceptText();
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
    <dw:WebDataWindowControl ID="DwMain" runat="server" LibraryList="~/DataWindow/app_assist/as_capital.pbl"
        DataWindowObject="d_as_reqmaster_donation" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientEventItemChanged="DwMainItemChange">
    </dw:WebDataWindowControl>
    <br />
    <dw:WebDataWindowControl ID="DwDetail" runat="server" LibraryList="~/DataWindow/app_assist/as_capital.pbl"
        DataWindowObject="d_as_reqdonate" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientEventItemChanged="DwDonateItemChange">
    </dw:WebDataWindowControl>
    <asp:HiddenField ID="HfMemNo" runat="server" />
    <asp:HiddenField ID="HfAssistDocNo" runat="server" />
    <asp:HiddenField ID="HfCapitalYear" runat="server" />
    <asp:HiddenField ID="HfMemberNo" runat="server" />
    <asp:HiddenField ID="HfReqSts" runat="server" />
</asp:Content>
