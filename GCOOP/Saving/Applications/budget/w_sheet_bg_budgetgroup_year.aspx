<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_bg_budgetgroup_year.aspx.cs"
    Inherits="Saving.Applications.budget.w_sheet_bg_budgetgroup_year" Title="Untitled Page" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=InsertRow%>
    <%=DeleteRow%>
    <%=InitDwList%>

    <script type="text/javascript">
    function OnClickInsertRow(){
        InsertRow();
    }
    
    function OnClickDeleteRow(){
        if(objDwList.RowCount() > 0){
            DeleteRow();
        }
    }
    
    function OnDwMainButtonClicked(sender, rowNumber, buttonName){
        if(buttonName == "cb_ok"){
            var year = 0;
            try{
                year = objDwMain.GetItem(1,"year");
            }catch(err){year = 0;}
            if(year != 0 && year != null){
                InitDwList();
            }
            else{
                alert("กรุณาเลือกปีงบประมาณก่อน");
            }
        }
    }
    
    function OnDwListClick(sender, rowNumber, objectName) {
        for (var i = 1; i <= sender.RowCount(); i++) {
            sender.SelectRow(i, false);
        }
        sender.SelectRow(rowNumber, true);
        sender.SetRow(rowNumber);
        Gcoop.GetEl("HdListRow").value = rowNumber + "";
    }
    
    function Validate(){
        return confirm("ต้องการบันทึกข้อมูล ใช่หรือไม่?");
    }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <br />
    <asp:Label ID="Label1" runat="server" Text="หมวดงบประมาณ" Font-Bold="True" Font-Names="Tahoma"
        Font-Size="16px" Font-Underline="True" />
    <br />
    <br />
    <dw:WebDataWindowControl ID="DwMain" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" DataWindowObject="d_bg_budgetgroup_year_head"
        LibraryList="~/DataWindow/budget/budget.pbl" ClientFormatting="True" ClientEventButtonClicking="OnDwMainButtonClicked">
    </dw:WebDataWindowControl>
    <br />
    <asp:Label ID="Label2" runat="server" Text="รายละเอียด" Font-Bold="True" Font-Names="Tahoma"
        Font-Size="14px" Font-Underline="True" />
    &nbsp;&nbsp;<span onclick="OnClickInsertRow()" style="cursor: pointer;">
        <asp:Label ID="LbInsert2" runat="server" Text="เพิ่มแถว" Font-Bold="False" Font-Names="Tahoma"
            Font-Size="14px" Font-Underline="True" ForeColor="#006600" /></span> &nbsp;&nbsp;
    <span onclick="OnClickDeleteRow()" style="cursor: pointer;">
        <asp:Label ID="LbDel2" runat="server" Text="ลบแถว" Font-Bold="False" Font-Names="Tahoma"
            Font-Size="14px" Font-Underline="True" ForeColor="Red" /></span>
    <br />
    <dw:WebDataWindowControl ID="DwList" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" DataWindowObject="d_bg_budgetgroup_year"
        LibraryList="~/DataWindow/budget/budget.pbl" ClientFormatting="True" RowsPerPage="17"
        ClientEventClicked="OnDwListClick">
        <PageNavigationBarSettings NavigatorType="NumericWithQuickGo" Visible="True">
        </PageNavigationBarSettings>
    </dw:WebDataWindowControl>
    <asp:HiddenField ID="HdListRow" runat="server" />
</asp:Content>
