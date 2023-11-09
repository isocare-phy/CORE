<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_bg_movement.aspx.cs"
    Inherits="Saving.Applications.budget.w_sheet_bg_movement" Title="Untitled Page" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=initBgTypeList%>
    <%=FilterBgCode%>
    <%=initBalance%>

    <script type="text/javascript">
    function OnDwMainButtonClicked(sender, rowNumber, buttonName){
        if(buttonName == "cb_search"){
            var year = 0;
            var bgGrp = "";
            try{
                year = objDwMain.GetItem(rowNumber, "year");
            }catch(err){year = 0;}
            try{
                bgGrp = objDwMain.GetItem(rowNumber, "budgetgroup");
            }catch(err){bgGrp = "";}
            if(year != 0 && year !=null && bgGrp != "" && bgGrp != null){
                initBgTypeList();
            }
            else{
                alert("กรุณาเลือกปีงบประมาณ และหมวดงบประมาณก่อน");
            }
        }
    }
    
    function OnDwMainItemChanged(sender, rowNumber, columnName, newValue){
        if(columnName == "year"){
            objDwMain.SetItem(rowNumber, columnName, newValue);
            objDwMain.AcceptText();
            FilterBgCode();
        }
    }
    
    function OnDwListClicked(sender, rowNumber, object){
        for (var i = 1; i <= sender.RowCount(); i++) {
            sender.SelectRow(i, false);
        }
            sender.SelectRow(rowNumber, true);
            //sender.SetRow(rowNumber);
            //alert(object);
            Gcoop.GetEl("HdListRow").value = rowNumber + "";
            initBalance();
    }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <br />
    <asp:Label ID="Label1" runat="server" Text="รายการงบประมาณเคลื่อนไหว" Font-Bold="True"
        Font-Names="Tahoma" Font-Size="16px" Font-Underline="True" />
    <br />
    <br />
    <asp:Label ID="Label2" runat="server" Text="เงื่อนไข" Font-Bold="True" Font-Names="Tahoma"
        Font-Size="14px" Font-Underline="True" /><br />
    <dw:WebDataWindowControl ID="DwMain" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" DataWindowObject="d_bg_movement_head"
        LibraryList="~/DataWindow/budget/budget.pbl" ClientFormatting="True" ClientEventButtonClicked="OnDwMainButtonClicked"
        ClientEventItemChanged="OnDwMainItemChanged">
    </dw:WebDataWindowControl>
    <asp:Label ID="Label3" runat="server" Text="งบประมาณคงเหลือ" Font-Bold="True" Font-Names="Tahoma"
        Font-Size="14px" Font-Underline="True" /><br />
    <dw:WebDataWindowControl ID="DwBalance" runat="server" AutoRestoreContext="False"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
        DataWindowObject="d_bg_movement_balance" LibraryList="~/DataWindow/budget/budget.pbl"
        ClientFormatting="True">
    </dw:WebDataWindowControl>
    <table>
        <tr>
            <td>
                <asp:Label ID="Label4" runat="server" Text="ประเภทรายการ" Font-Bold="True" Font-Names="Tahoma"
                    Font-Size="14px" Font-Underline="True" /><br />
                <asp:Panel ID="Panel3" runat="server" BorderStyle="Ridge" Width="307px" Height="350px"
                    ScrollBars="Auto">
                    <dw:WebDataWindowControl ID="DwList" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
                        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" DataWindowObject="d_bg_movement_list"
                        LibraryList="~/DataWindow/budget/budget.pbl" ClientFormatting="True" Width="307px"
                        Height="350px" ClientEventClicked="OnDwListClicked">
                    </dw:WebDataWindowControl>
                </asp:Panel>
            </td>
            <td>
                <asp:Label ID="Label5" runat="server" Text="รายละเอียด" Font-Bold="True" Font-Names="Tahoma"
                    Font-Size="14px" Font-Underline="True" /><br />
                <asp:Panel ID="Panel4" runat="server" BorderStyle="Ridge" Width="430px" Height="350px"
                    ScrollBars="Auto">
                    <dw:WebDataWindowControl ID="DwDetail" runat="server" AutoRestoreContext="False"
                        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
                        DataWindowObject="d_bg_movement_detail" LibraryList="~/DataWindow/budget/budget.pbl"
                        ClientFormatting="True" Width="430px" Height="350px">
                    </dw:WebDataWindowControl>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="HdListRow" runat="server" />
</asp:Content>
