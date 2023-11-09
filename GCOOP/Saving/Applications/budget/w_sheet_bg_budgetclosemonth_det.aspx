<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_bg_budgetclosemonth_det.aspx.cs"
    Inherits="Saving.Applications.budget.w_sheet_bg_budgetclosemonth_det" Title="Untitled Page" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=FilterBgGrp%>
    <%=InitBgMonthDet%>
    <%=InsertRow%>
    <%=DeleteRow%>
    <%=InitBgDetailAndBal%>
    <%=Refresh%>
    <script type="text/javascript">
    function OnClickInsertRow(){
        if(Gcoop.GetEl("HdRetrieve").value == "true"){
            InsertRow();
        }
    }

    function OnClickDeleteRow(){
        if(objDwDetail.RowCount() > 0){
            DeleteRow();
        }
    }
    
    function OnDwListClicked(sender, rowNumber, objectName){
        for (var i = 1; i <= sender.RowCount(); i++) {
            sender.SelectRow(i, false);
        }
        sender.SelectRow(rowNumber, true);
        Gcoop.GetEl("HdListRow").value = rowNumber + "";
        InitBgDetailAndBal();
    }
    
    function OnDwMainButtonClicked(sender, rowNumber, buttonName){
        if(buttonName == "cb_ok"){
            var year = 0;
            var month = 0;
            var seqNo = 0;
            try{
                year = objDwMain.GetItem(1, "budgetyear");
            }catch(err){year = 0};
            try{
                month = objDwMain.GetItem(1, "budgetmonth");
            }catch(err){month = 0};
            try{
                seqNo = objDwMain.GetItem(1, "seq_no");
            }catch(err){seqNo = 0};

            if(year != 0 && year != null && month != 0 && month != null && seqNo != 0 && seqNo != null){
                Gcoop.GetEl("HdRetrieve").value = "true";
                InitBgMonthDet();
            }
            else{
                alert("กรุณาเลือก ปี เดือน และหมวดงบประมาณก่อน");
            }
        }
    }

    function OnDwMainItemChanged(sender, rowNumber, columnName, newValue){
        if(columnName == "budgetyear"){
            objDwMain.SetItem(rowNumber, columnName, newValue);
            objDwMain.AcceptText();
            FilterBgGrp();
        }
    }
    
    function OnDwDetailClicked(sender, rowNumber, objectName){
         for (var i = 1; i <= sender.RowCount(); i++) {
            sender.SelectRow(i, false);
        }
        sender.SelectRow(rowNumber, true);
        sender.SetRow(rowNumber);
        Gcoop.GetEl("HdDetailRow").value = rowNumber + "";
    }
    
    function OnDwDetailItemChanged(sender, rowNumber, columnName, newValue){
        if(columnName == "month_amt"){
            objDwDetail.SetItem(rowNumber, columnName, newValue);
            objDwDetail.AcceptText();
            Refresh();
        }
    }
    
    function Validate(){
        return confirm("ต้องการบันทึกข้อมูล ใช่หรือไม่?");
    }
     
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <br />
    <asp:Label ID="Label1" runat="server" Text="รายละเอียดค่าใช้จ่ายประจำเดือน" Font-Bold="True"
        Font-Names="Tahoma" Font-Size="16px" Font-Underline="True" />
    <br />
    <br />
    <dw:WebDataWindowControl ID="DwMain" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" DataWindowObject="d_bg_budgetclosemont_det_main"
        LibraryList="~/DataWindow/budget/budget.pbl" ClientFormatting="True" ClientEventButtonClicked="OnDwMainButtonClicked"
        ClientEventItemChanged="OnDwMainItemChanged">
    </dw:WebDataWindowControl>
    <asp:Label ID="Label2" runat="server" Text="งบประมาณคงเหลือ" Font-Bold="True" Font-Names="Tahoma"
        Font-Size="14px" Font-Underline="True" /><br />
    <dw:WebDataWindowControl ID="DwBalance" runat="server" AutoRestoreContext="False"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
        DataWindowObject="d_bg_budgetclosemont_det_balance" LibraryList="~/DataWindow/budget/budget.pbl"
        ClientFormatting="True">
    </dw:WebDataWindowControl>
    <table>
        <tr>
            <td>
                <asp:Label ID="Label4" runat="server" Text="ประเภทรายการ" Font-Bold="True" Font-Names="Tahoma"
                    Font-Size="14px" Font-Underline="True" /><br />
                <asp:Panel ID="Panel3" runat="server" BorderStyle="Ridge" Width="260px" Height="350px"
                    ScrollBars="Auto">
                    <dw:WebDataWindowControl ID="DwList" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
                        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" DataWindowObject="d_bg_budgetclosemont_det_list"
                        LibraryList="~/DataWindow/budget/budget.pbl" ClientFormatting="True" Width="260px"
                        Height="350px" ClientEventClicked="OnDwListClicked">
                    </dw:WebDataWindowControl>
                </asp:Panel>
            </td>
            <td>
                <asp:Label ID="Label5" runat="server" Text="รายละเอียด" Font-Bold="True" Font-Names="Tahoma"
                    Font-Size="14px" Font-Underline="True" />
                <span onclick="OnClickInsertRow()" style="cursor: pointer;">
                    <asp:Label ID="LbInsert2" runat="server" Text="เพิ่มแถว" Font-Bold="False" Font-Names="Tahoma"
                        Font-Size="14px" Font-Underline="True" ForeColor="#006600" /></span> &nbsp;&nbsp;
                <span onclick="OnClickDeleteRow()" style="cursor: pointer;">
                    <asp:Label ID="LbDel2" runat="server" Text="ลบแถว" Font-Bold="False" Font-Names="Tahoma"
                        Font-Size="14px" Font-Underline="True" ForeColor="Red" /></span>
                <br />
                <asp:Panel ID="Panel4" runat="server" BorderStyle="Ridge" Width="470px" Height="350px"
                    ScrollBars="Auto">
                    <dw:WebDataWindowControl ID="DwDetail" runat="server" AutoRestoreContext="False"
                        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
                        DataWindowObject="d_bg_budgetclosemont_det" LibraryList="~/DataWindow/budget/budget.pbl"
                        ClientFormatting="True" ClientEventClicked="OnDwDetailClicked" Width="470px" Height="350px" ClientEventItemChanged="OnDwDetailItemChanged">
                    </dw:WebDataWindowControl>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="HdDetailRow" runat="server" />
    <asp:HiddenField ID="HdListRow" runat="server" />
    <asp:HiddenField ID="HdRetrieve" runat="server" Value="false" />
</asp:Content>
