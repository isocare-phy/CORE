<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_ln_loanapv.aspx.cs" Inherits="Saving.Applications.investment.w_sheet_ln_loanapv" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=memNoItemChange%>
    <%=appLoanMoney%>
    <%=StatusItemChange%>
    <script type="text/JavaScript">
        function Validate() {
            objDwMain.AcceptText();
 
            return confirm("ยืนยันการบันทึกข้อมูล");
        }
        function DwMainItemChange(sender, rowNumber, columnName, newValue) {

            if (columnName == "loanrequest_docno") {
                Gcoop.GetEl("Hfloanavp_amt").value = newValue;
                objDwMain.SetItem(1, "loanrequest_docno", newValue);
                objDwMain.AcceptText();
                memNoItemChange();
            }

            if (columnName == "loanrequest_status") {
                objDwMain.SetItem(1, "loanrequest_status", newValue);
                objDwMain.AcceptText();
                StatusItemChange();
            }
//            if (columnName == "loanapprove_amt") {
//                Gcoop.GetEl("Hfloanavp_amt").value = newValue;
//                objDwMain.SetItem(1, "loanapprove_amt", newValue);
//                objDwMain.AcceptText();
//                appLoanMoney();
//            }
//            if (columnName == "period_installment") {
//                Gcoop.GetEl("Hfperiod").value = newValue;
//                objDwMain.SetItem(1, "period_installment", newValue);
//                objDwMain.AcceptText();
//                appLoanMoney();
//            }
//            if (columnName == "period_payment") {
//                Gcoop.GetEl("Hfpayment").value = newValue;
//                objDwMain.SetItem(1, "period_installment", newValue);
//                objDwMain.AcceptText();
//                appLoanMoney();
//            }
        }
        function DwButtonClick(sender, rowNumber, buttonName) {
            if (buttonName == "b_search") {
                Gcoop.OpenIFrame("600", "590", "w_dlg_member_search_docno.aspx", "")
            }
            if (buttonName == "b_cal") {
                appLoanMoney();
            }
            return 0;
        }

        function PostMemberNo(member_no,loanrequest_docno) {
            objDwMain.AcceptText();
            Gcoop.GetEl("Hfdoc_no").value = loanrequest_docno;
            memNoItemChange();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
<%--    <dw:WebDataWindowControl ID="DwHead" runat="server" DataWindowObject="d_lcsrv_slippayin_memdet_fsct"
        LibraryList="~/DataWindow/loan/loan.pbl" ClientScriptable="True" ClientEvents="true"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        AutoRestoreContext="False" ClientEventItemChanged="DwItemChange">
    </dw:WebDataWindowControl>--%>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_lcsrv_loanapv"
        LibraryList="~/DataWindow/investment/loan.pbl" ClientScriptable="True" ClientEvents="true"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        AutoRestoreContext="False" ClientEventItemChanged="DwMainItemChange" ClientEventButtonClicked="DwButtonClick">
    </dw:WebDataWindowControl>
    <asp:HiddenField ID="Hfmember_no" runat="server" />
    <asp:HiddenField ID="Hfdoc_no" runat="server" />
    <asp:HiddenField ID="Hfloanavp_amt" runat="server" />
    <asp:HiddenField ID="Hfperiod" runat="server" />
    <asp:HiddenField ID="Hfpayment" runat="server" />
    <br />
</asp:Content>
