<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_bg_budgetsetamt.aspx.cs"
    Inherits="Saving.Applications.budget.w_sheet_bg_budgetsetamt" Title="Untitled Page" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=RetrieveList%>
    <%=Refresh%>

    <script type="text/javascript">
        
        function OnDwListItemChanged(sender, rowNumber, columnName, newValue){
            if (columnName == "setbudget_amt") {
                objDwList.SetItem(rowNumber, columnName, newValue);
                objDwList.AcceptText();
                var setAmt = objDwList.GetItem(rowNumber, columnName);
                if(setAmt <= 0){
                    alert("จำนวนเงินควรมีค่ามากกว่า 0");
                }
            }
        }
        
        function OnDwMainButtonClicked(sender, rowNumber, buttonName){
            if(buttonName == "cb_ok"){
                var bgYear = 0;
                try{
                    bgYear = objDwMain.GetItem(1, "budgetyear");
                }catch(err){ bgYear = 0; }
                if( bgYear == 0 || bgYear == null ){
                    alert("กรุณาเลือกปีงบประมาณก่อน");
                }
                else{
                    RetrieveList();
                }    
            }
        }
        
        function Validate(){
            var check = false;
            var bgSetAmt = 0;
            for(var i = 1; i <= objDwList.RowCount(); i++){
                try{
                    bgSetAmt = objDwList.GetItem(i, "setbudget_amt");  
                }
                catch(err){bgSetAmt = 0;}     
                
                if(bgSetAmt != null && bgSetAmt >= 0){
                    check = true;
                } 
                else{
                    check = false;
                    break;
                }
            }
            if(check == true){
                return confirm("บันทึกข้อมูล ใช่หรือไม่?");
            }
            else{
                alert("กรุณากรอกข้อมูลให้ครบถ้วน");
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <br />
    <table width="100%">
        <tr>
            <td align="left">
                <asp:Label ID="Label1" runat="server" Text="ตั้งค่างบประมาณ" Font-Bold="True" Font-Names="Tahoma"
                    Font-Size="16px" Font-Underline="True" />
            </td>
        </tr>
    </table>
    <br />
    <dw:WebDataWindowControl ID="DwMain" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" DataWindowObject="d_bg_budgetsetamt_main"
        LibraryList="~/DataWindow/budget/budget.pbl" ClientFormatting="True" ClientEventButtonClicked="OnDwMainButtonClicked">
    </dw:WebDataWindowControl>
    <asp:Label ID="Label3" runat="server" Text="รายละเอียดรายการ" Font-Bold="True" Font-Names="Tahoma"
        Font-Size="14px" Font-Underline="True" /><br />
    <br />
    <asp:Panel ID="Panel1" runat="server" BorderStyle="Ridge" ScrollBars="Auto" Height="420px">
        <dw:WebDataWindowControl ID="DwList" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
            AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" DataWindowObject="d_bg_budgetsetamt_list_new"
            LibraryList="~/DataWindow/budget/budget.pbl" ClientFormatting="True" ClientEventItemChanged="OnDwListItemChanged"
            Height="420px">
        </dw:WebDataWindowControl>
    </asp:Panel>
</asp:Content>
