<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_bg_cancelslip.aspx.cs"
    Inherits="Saving.Applications.budget.w_sheet_bg_cancelslip" Title="Untitled Page" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=RetrieveList%>
    <%=RetrieveDetail%>

    <script type="text/javascript">
        function CheckAll(obj) {
            var theForm = obj.form;
            if (obj.checked){
                for(var i = 1; i <= objDwList.RowCount() ; i++){
                    objDwList.SetItem(i, "payment_status", -9);
                }
            }
            else{
                for(var i = 1; i <= objDwList.RowCount() ; i++){
                    objDwList.SetItem(i, "payment_status", 8);
                }
            }
        }
    
        function OnDwListClicked(sender, rowNumber, objectName){          
            var row = 0;
            Gcoop.CheckDw(sender, rowNumber, objectName, "payment_status", -9, 8);
            for (var i = 1; i <= sender.RowCount(); i++) {
                sender.SelectRow(i, false);
            }
            if(objectName == "slip_no"){
                //objectName == "payment_status"
                sender.SelectRow(rowNumber, true);
                sender.SetRow(rowNumber);
                var slipNo = objDwList.GetItem(rowNumber, "slip_no");
                Gcoop.GetEl("HdSlipNo").value = slipNo;
                RetrieveDetail();
                row = rowNumber;
            }
        } 
        
        function OnDwMainButtonClicked(sender, rowNumber, buttonName){
            if(buttonName == "cb_ok"){
                RetrieveList();
            }
        }
        
        function OnDwMainItemChanged(sender, rowNumber, columnName, newValue){
            objDwMain.SetItem(rowNumber, columnName, newValue);
            objDwMain.AcceptText();
            return 0;
        }
        
        function Validate(){
            return confirm("ต้องการบันทึกข้อมูล ใช่หรือไม่?");        
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <br />
    <asp:Label ID="Label1" runat="server" Text="ยกเลิกใบสำคัญจ่าย" Font-Bold="True" Font-Names="Tahoma"
        Font-Size="16px" Font-Underline="True" />
    <br />
    <br />
    <dw:WebDataWindowControl ID="DwMain" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" DataWindowObject="d_bg_budgetcancelslip_main"
        LibraryList="~/DataWindow/budget/budget.pbl" ClientFormatting="True" ClientEventButtonClicked="OnDwMainButtonClicked"
        ClientEventItemChanged="OnDwMainItemChanged">
    </dw:WebDataWindowControl>
    <table>
        <tr>
            <td>
                <asp:Label ID="Label2" runat="server" Text="รายการ" Font-Bold="True" Font-Names="Tahoma"
                    Font-Size="14px" Font-Underline="True" /><br />
                &nbsp;<input type="checkbox" name="checkall" onclick="CheckAll(this)" />
                <asp:Label ID="Label4" runat="server" Text="เลือกทั้งหมด"></asp:Label>
                <asp:Panel ID="Panel1" runat="server" Width="310px" ScrollBars="Auto" Height="400px" BorderStyle="Ridge">
                    <dw:WebDataWindowControl ID="DwList" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
                        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" DataWindowObject="d_bg_budgetcancelslip_list"
                        LibraryList="~/DataWindow/budget/budget.pbl" ClientFormatting="True" ClientEventClicked="OnDwListClicked"
                        Width="310px" Height="400px">
                    </dw:WebDataWindowControl>
                </asp:Panel>
            </td>
            <td>
                <asp:Label ID="Label3" runat="server" Text="รายละเอียดรายการ" Font-Bold="True" Font-Names="Tahoma"
                    Font-Size="14px" Font-Underline="True" /><br />
                <br />
                <asp:Panel ID="Panel2" runat="server" Width="440px" ScrollBars="Auto" Height="400px" BorderStyle="Ridge">
                    <dw:WebDataWindowControl ID="DwDetail" runat="server" AutoRestoreContext="False"
                        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
                        DataWindowObject="d_bg_budgetcancelslip_detail" LibraryList="~/DataWindow/budget/budget.pbl"
                        ClientFormatting="True" Width="440px"
                        Height="400px ">
                    </dw:WebDataWindowControl>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="HdSlipNo" runat="server" />
</asp:Content>
