<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_lt_withdraw_estimate.aspx.cs" 
Inherits="Saving.Applications.loantracking.w_sheet_lt_withdraw_estimate" Title="Untitled Page" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript %>
    <%=loanCalInt %>
    <%=calculateAmt%>
    <%=saveWithdraw%>
    <%=searchMemberNo%>
    <%=setTrnColl  %>
    <title>Share Withdraw Page</title>
<script type="text/javascript">
    function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล");
    }
        function OnDwMainItemChanged(sender, rowNumber, columnName, newValue){
        //เมื่อมีการเปลี่ยนวันที่
       // alert ("1");
        if(columnName=="operate_tdate"){
            alert (newValue);
            if(LoanChecked()){
                objDwMain.SetItem(1,"operate_tdate",newValue);
                objDwMain.SetItem( 1, "operate_date", Gcoop.ToEngDate(newValue));
                objDwMain.AcceptText();
                objDwOperateLoan.AcceptText();
                loanCalInt();
            }
        }else if (columnName == "member_no"){
           // alert (newValue//);
            Gcoop.GetEl("HdMemberNo").value = newValue ;
            objDwMain.SetItem( 1, "member_no", newValue );
            objDwMain.AcceptText();
            searchMemberNo();
        }
    }
    function OnDwLoanItemClicked(sender, rowNumber, objectName){
        if(objectName=="operate_flag"){
            //กำหนดค่า ให้ Check Box
            Gcoop.CheckDw(sender, rowNumber, objectName, "operate_flag", 1, 0);
            var flag = objDwOperateLoan.GetItem(rowNumber,"operate_flag");
            if(flag==1){
                var bfshrcontbalamt = objDwMain.GetItem(1,"bfshrcont_balamt");
                var payoutclramt = objDwMain.GetItem(1,"payoutclr_amt");
                var amttotal = bfshrcontbalamt-payoutclramt;
                if(amttotal<=0){
                    alert("ไม่สามารถทำรายการได้ เนื่องจาก หุ้นคงเหลือ น้อยกว่า หนี้คงเหลือ");
                    objDwOperateLoan.SetItem(rowNumber,"operate_flag",0);
                    return false;
                }else{
                    objDwOperateLoan.SetItem(rowNumber,"principal_payamt",objDwOperateLoan.GetItem(rowNumber,"item_balance"));
                    objDwMain.AcceptText();
                    objDwOperateLoan.AcceptText();
                    loanCalInt();
                }
            }else{
                objDwOperateLoan.SetItem(rowNumber,"principal_payamt",0);
                objDwOperateLoan.SetItem(rowNumber,"interest_payamt",0);
                objDwOperateLoan.SetItem(rowNumber,"item_payamt",0);
                objDwOperateLoan.SetItem(rowNumber,"item_balance",objDwOperateLoan.GetItem(rowNumber,"bfshrcont_balamt"));
                objDwMain.AcceptText();
                objDwOperateLoan.AcceptText();
                calculateAmt();
            }
        }
    }
    
    function OnDwLoanItemChanged(sender, rowNumber, columnName, newValue){
        if (columnName == "principal_payamt" || columnName == "interest_payamt") {
            var flag = objDwOperateLoan.GetItem(rowNumber,"operate_flag");
            if(flag==1){
                objDwOperateLoan.SetItem(rowNumber, columnName, newValue);
                objDwMain.AcceptText();
                objDwOperateLoan.AcceptText();
                calculateAmt();
            }
        }
    }
    
    function LoanChecked(){
        //มีการเช็คถูกที่ Loan หรือไม่?
        var row = objDwOperateLoan.RowCount();
        var i=1;
        for(i;i<=row;i++){
            var flagValue = objDwOperateLoan.GetItem(i,"operate_flag");
            if(flagValue==1){
                return true;
                break;
            }
        }
        return false;
    }    
    
    function SheetLoadComplete(){

        var indexVal = Gcoop.GetEl("HfIndex").value;
        var allIndexVal = Gcoop.ParseInt(Gcoop.GetEl("HfAllIndex").value);
        if(indexVal==allIndexVal){
            window.opener.RefreshByDlg();
            window.close();
        }
        var bfshrcontbalamt = objDwMain.GetItem(1,"bfshrcont_balamt");
        var payoutclramt = objDwMain.GetItem(1,"payoutclr_amt");
        var amttotal = bfshrcontbalamt-payoutclramt;
        if(amttotal<0){
            alert("ค่าหักชำระหุ้น ติดลบ กรุณา ระบุจำนวนเงินใหม่");
            return false;
        }
    }
    function SaveWithdraw(){
        var payoutnetamt = objDwMain.GetItem(1,"payoutnet_amt");
        var bfshrcontbalamt = objDwMain.GetItem(1,"bfshrcont_balamt");
        var payoutclramt = objDwMain.GetItem(1,"payoutclr_amt");
        var totalamt = bfshrcontbalamt - payoutclramt;
        if(totalamt>=0){
            objDwMain.AcceptText();
            objDwOperateLoan.AcceptText();
            saveWithdraw();
        }else{
            alert("ไม่สามารถ บันทึกได้ เนื่องจาก ยอดชำระค่าหุ้น ติดลบ");
            return false;
        }
    }
    function SetTrnColl()
    {
        setTrnColl ();
    }
    function OnItemdetailChanged(s, row, c, v) {
        if (c == "unit_price" || c == "quantiy") {
            var Total = 0;
            var total_amt = 0;
            var quantiy = 0;
            objdw_detail.SetItem(row, c, v);
            objdw_detail.AcceptText();
            for (i = 0; i < objdw_detail.RowCount(); i++) {
                Total = Gcoop.ParseFloat(objdw_detail.GetItem(i + 1, "unit_price"));
                quantiy = Gcoop.ParseFloat(objdw_detail.GetItem(i + 1, "quantiy"));
                total_amt += (Total * quantiy);
            }
            objDw_main.SetItem(1, "total_amt", total_amt); //total_amt
            objDw_main.AcceptText();
        } else if (c == "product_id") {
            objdw_detail.SetItem(row, c, v);
            objdw_detail.AcceptText();
            getProduct();
        }

        return 0;
    }
    
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
 <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
   <asp:HiddenField ID="HfIndex" runat="server" />
    <asp:HiddenField ID="HfAllIndex" runat="server" />
    <asp:HiddenField ID="HfFormType" runat="server" />
     <asp:HiddenField ID="HdMemberNo" runat="server" />
    <asp:Label ID="LbSaveStatus" runat="server" Text=""></asp:Label>
 <table border="0">
 <tr>
     <td>
                <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_sl_share_swd"
                    LibraryList="~/DataWindow/keeping/sl_share_withdraw.pbl" AutoRestoreContext="False"
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
                    ClientEventItemChanged="OnDwMainItemChanged" ClientFormatting="True">
                </dw:WebDataWindowControl>
     </td>
 </tr>
 <tr>
      <td>
       <dw:WebDataWindowControl ID="DwOperateLoan" runat="server" DataWindowObject="d_sl_slip_operate_loan"
                    LibraryList="~/DataWindow/keeping/sl_share_withdraw.pbl" AutoRestoreContext="False"
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" Height="100px"
                    ClientScriptable="True" ClientEventClicked="OnDwLoanItemClicked" ClientFormatting="True"
                    ClientEventItemChanged="OnDwLoanItemChanged">
                </dw:WebDataWindowControl>
                <br />
      </td>
 </tr>
 <tr>
     <td>
        
          <dw:WebDataWindowControl ID="DwOperateEtc" runat="server" DataWindowObject="d_sl_slip_operate_etc"
                    LibraryList="~/DataWindow/keeping/sl_share_withdraw.pbl" AutoRestoreContext="False"
                    Height="80px" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
                    ClientScriptable="True">
                </dw:WebDataWindowControl>
     </td>
 </tr>
 <tr>
    <td>
        <input id="btnWithdraw" type="button" value="ประมาณการโอนหนี้ผู้ค้ำ" onclick="SetTrnColl()" />
        <br />
         <asp:Label ID="Label1" runat="server" Text="สัญญาที่โอนหนี้ให้ผู้ค้ำ"  Font-Bold = "true"></asp:Label>
        <dw:WebDataWindowControl ID="DwTrnColl" runat="server" DataWindowObject="d_loansrv_est_trncoll"
                    LibraryList="~/DataWindow/keeping/sl_share_withdraw.pbl" AutoRestoreContext="False"
                    Height="80px" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
                    ClientScriptable="True">
                </dw:WebDataWindowControl>
                <br />
    </td>
 </tr>
  <tr>
    <td>
     <asp:Label ID="Label2" runat="server" Text="ผู้ค้ำประกันร่วม" Font-Bold = "true" ></asp:Label>
        <dw:WebDataWindowControl ID="DwTrnCollCo" runat="server" DataWindowObject="d_loansrv_est_trncoll"
                    LibraryList="~/DataWindow/keeping/sl_share_withdraw.pbl" AutoRestoreContext="False"
                    Height="80px" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
                    ClientScriptable="True">
                </dw:WebDataWindowControl>
    </td>
 </tr>
 </table>
</asp:Content>
