<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_moreaccount_activity.aspx.cs"
    Inherits="Saving.Applications.account.w_sheet_moreaccount_activity" Title="Untitled Page" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=postDetail%>
    <%=postCRDR%>
    <%=postAccountId%>
    <%=insertRowDetail%>
    <%=deleteRowDetail%>

    <script type="text/javascript">
        
        function OnDwMainButtonClicked(sender, rowNumber, buttonName){
            if(buttonName == "cb_ok"){
                var year = 0;
                var period = 0;
                try{
                    year = objDwMain.GetItem(1,"acc_year");
                }catch(err){ year = 0; }
                try{
                    period = objDwMain.GetItem(1,"acc_period");
                }catch(err){ period = 0; }
                if(year != 0 && period != 0 && year != null && period != null){
                    Gcoop.GetEl("HdCheckClick").value = "true";
                    postDetail();
                }
                else
                {
                    alert("กรุณากรอกข้อมูลให้เรียบร้อย");
                }
            }
        }
        
        function OnDwMainItemChanged(sender, rowNumber, colunmName, newValue){
            if(colunmName == "acc_year" || colunmName == "acc_period"){
                objDwMain.SetItem(rowNumber, colunmName, newValue);
                objDwMain.AcceptText();
            }
        }
        
        function OnDwDetailItemChanged(sender, rowNumber, colunmName, newValue){
            if(colunmName == "dr_amount" || colunmName == "cr_amount"){
                objDwDetail.SetItem(rowNumber,colunmName,newValue);
                objDwDetail.AcceptText();
                postCRDR();
            }
            else if(colunmName == "account_id"){
                objDwDetail.SetItem(rowNumber, colunmName, newValue);
                objDwDetail.AcceptText();
                postAccountId();
            } 
        }
        
        function OnClickInsertRow(){
            if(Gcoop.GetEl("HdCheckClick").value == "true"){
                insertRowDetail();
            }
        }
        
        function OnClickDeleteRow(){
            if(objDwDetail.RowCount() > 0){
                var currentRow = Gcoop.GetEl("HdDetailRow").value;
                var confirmText = "ยืนยันการลบแถวที่ " + currentRow;
                if (confirm(confirmText)) {
                    deleteRowDetail();
                }
            }
            else{
                alert("ยังไม่มีการเพิ่มแถวสำหรับรายการ");
            }
        }
        
        function OnDwDetailClicked(sender, rowNumber, objectName) {
            for (var i = 1; i <= sender.RowCount(); i++) {
                sender.SelectRow(i, false);
            }
            sender.SelectRow(rowNumber, true);
            sender.SetRow(rowNumber);
            Gcoop.GetEl("HdDetailRow").value = rowNumber + "";
        }
        
        function Validate(){ 
            return confirm("ต้องการบันทึกข้อมูล ใช่หรือไม?่");
        }
        function MenubarNew() {
            window.location = "";
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" ClientScriptable="True" DataWindowObject="d_acc_moreaccount_activity_main"
        LibraryList="~/DataWindow/account/acc_moreaccount_activity.pbl" AutoRestoreContext="False"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientEventButtonClicked="OnDwMainButtonClicked"
        ClientEventItemChanged="OnDwMainItemChanged" ClientFormatting="False">
    </dw:WebDataWindowControl>
    <table style="width: 100%;">
        <tr>
            <td class="style5">
                <asp:Label ID="Label7" runat="server" Text="รายการกิจกรรมเพิ่มเติม" Font-Bold="True" Font-Names="Tahoma"
                    Font-Size="14px" ForeColor="#0099CC" Font-Overline="False" Font-Underline="True" />
                &nbsp; <span onclick="OnClickInsertRow()" style="cursor: pointer;">
                    <asp:Label ID="LbInsert2" runat="server" Text="เพิ่มแถว" Font-Bold="False" Font-Names="Tahoma"
                        Font-Size="14px" Font-Underline="True" ForeColor="#006600" /></span> &nbsp;&nbsp;
                <span onclick="OnClickDeleteRow()" style="cursor: pointer;">
                    <asp:Label ID="LbDel2" runat="server" Text="ลบแถว" Font-Bold="False" Font-Names="Tahoma"
                        Font-Size="14px" Font-Underline="True" ForeColor="Red" /></span>
            </td>
        </tr>
        <tr>
            <td class="style6" colspan="3" valign="top">
                <asp:Panel ID="Panel1" runat="server" Height="450px" ScrollBars="Vertical" BorderStyle="Ridge">
                    <dw:WebDataWindowControl ID="DwDetail" runat="server" ClientScriptable="True" DataWindowObject="d_acc_moreaccount_activity_list"
                        LibraryList="~/DataWindow/account/acc_moreaccount_activity.pbl" AutoRestoreContext="False"
                        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientEventItemChanged="OnDwDetailItemChanged"
                        ClientFormatting="True" ClientEventClicked="OnDwDetailClicked">
                    </dw:WebDataWindowControl>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="HdDetailRow" runat="server" />
    <asp:HiddenField ID="HdCheckClick" runat="server" Value="false" />
</asp:Content>
