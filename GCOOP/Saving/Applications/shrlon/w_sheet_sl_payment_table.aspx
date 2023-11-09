<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_sl_payment_table.aspx.cs" Inherits="Saving.Applications.shrlon.w_sheet_sl_payment_table" Title="Untitled Page" %>
<%@ Register assembly="WebDataWindow" namespace="Sybase.DataWindow.Web" tagprefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%=initJavaScript%>
<%=postRetrieve%>
<%=downloadPDF%>
<%=postInstallmentChanged%>
<%=postPeriodPaymentChanged%>
<%=postShow%>
<%=postNewClicked%>
   <%=popupReport%>
    <%=runProcess%>

<script type="text/javascript">
function MenubarNew(){
    postNewClicked();
}
function GetValueLoanRequest(docNo){
    objdw_criteria.SetItem(1,"loancontract_no", docNo);
    objdw_criteria.AcceptText();
}
function GetValueFromDlg(loancontractno){
    objdw_criteria.SetItem(1,"loancontract_no", loancontractno);
    objdw_criteria.AcceptText();
}
function onCriteriaButtonClicked(s, r, c){
    if( c == 'b_search' ){
        var t = objdw_criteria.GetItem(1,"paytab_type");
        if(t==1) Gcoop.OpenDlg('750', '550', 'w_dlg_sl_loancontract_search.aspx', '');
        else Gcoop.OpenDlg('750', '550', 'w_dlg_sl_loanrequest_search_all.aspx', '');
    }else if( c == 'b_retrieve' ){
        objdw_criteria.AcceptText();
        postRetrieve();
    }else if( c == 'b_calculate' ){
        objdw_criteria.AcceptText();
        postShow();
    }
}
function onCriteriaItemchanged(sender, rowNumber, columnName, newValue){
    /*0 – Accept data value
      1 – Reject data value and prevent focus change
      2 – Reject data value but allow focus change */
    if( columnName == 'loancontract_no' ){
        objdw_criteria.AcceptText();
        objdw_criteria.SetItem(1,"loancontract_no",newValue);
        postRetrieve();
    }else if( columnName == 'installment' ){
        objdw_criteria.AcceptText();
        objdw_criteria.SetItem(1,"installment",newValue);
        //var a = objdw_criteria.GetItem(1,"installment");
        //alert(a+" - newValue("+newValue+")");
        //postInstallmentChanged();
        postPeriodPaymentChanged();
    }else if( columnName == 'period_amt' ){
        objdw_criteria.AcceptText();
        objdw_criteria.SetItem(1,"period_amt",newValue);
        //var b = objdw_criteria.GetItem(1,"period_amt");
        //alert(b+" - newValue("+newValue+")");
        //postPeriodPaymentChanged();
        postInstallmentChanged();
    }else if( columnName == 'intrate_amt' ){
        objdw_criteria.AcceptText();
        objdw_criteria.SetItem(1,"intrate_amt",newValue);
        var c = objdw_criteria.GetItem(1,"intrate_amt");
        alert(c+" - newValue("+newValue+")");
        postInstallmentChanged();
    }
}



</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID = "LtServerMessage" runat = "server"></asp:Literal>
  
    <table style="width:100%;">
        <tr>
            <td>
                &nbsp;</td>
            <td>
                <table style="width:100%;">
                    <tr>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td align="right">
                            <input id="pdfbutton" type="button" value="ดาวน์โหลดเป็น PDF" onclick="downloadPDF()"/>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <dw:WebDataWindowControl ID="dw_criteria" runat="server" 
                    DataWindowObject="d_loansrv_lnpaytab_paycritiria" 
                    LibraryList="~/DataWindow/shrlon/sl_payment_table.pbl" 
                    ClientEventItemChanged="onCriteriaItemchanged" AutoRestoreContext="False" 
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" 
                    ClientFormatting="True" ClientScriptable="True"
                    ClientEventButtonClicked="onCriteriaButtonClicked">
                </dw:WebDataWindowControl>
            </td>
            <td rowspan="3" valign="top">
                <dw:WebDataWindowControl ID="dw_result" runat="server" 
                    DataWindowObject="d_loansrv_lnpaytab_paytable" 
                    LibraryList="~/DataWindow/shrlon/sl_payment_table.pbl" 
                    AutoRestoreContext="False" AutoRestoreDataCache="True" 
                    AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True" 
                    ClientScriptable="True" RowsPerPage="50">
                    <PageNavigationBarSettings NavigatorType="NumericWithQuickGo" Visible="True">
                    </PageNavigationBarSettings>
                </dw:WebDataWindowControl>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
    </table>
      <asp:HiddenField ID="HdOpenIFrame" runat="server" Value="False" />
    <asp:HiddenField ID="HdcheckPdf" runat="server" Value="False" />
    <asp:HiddenField ID="HdIsPostBack" runat="server" Value="false" />
    <asp:HiddenField ID="Hdcommitreport" runat="server" Value="false" />
</asp:Content>
