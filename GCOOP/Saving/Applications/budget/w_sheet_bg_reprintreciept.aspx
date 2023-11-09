<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_bg_reprintreciept.aspx.cs"
    Inherits="Saving.Applications.budget.w_sheet_bg_reprintreciept" Title="Untitled Page" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=InitList%>
    <%=PrintReceipt%>
    <script type="text/javascript">
        function OnDwMainItemChanged(sender, rowNumber, colunmName, newValue){
            if(colunmName == "operate_date"){
                objDwMain.SetItem(rowNumber, colunmName, newValue);
                objDwMain.AcceptText();
                return 0;
            }
        }
        
        function OnDwMainButtonClicked(sender, rowNumber, buttonName){
            if(buttonName == "cb_search"){
                InitList();
            }
        }
        
        function OnDwListButtonClicked(sender, rowNumber, buttonName){
            if(buttonName == "cb_print"){
                PrintReceipt();
            }
        }
        
        function OnDwListClicked(sender, rowNumber, objectName){
            if(objectName == "ai_select"){
                Goop.CheckDw(sender, rowNumber, objectName, "ai_select", 1, 0);
            }
        }
        
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <br />
    <asp:Label ID="Label1" runat="server" Text="พิมพ์ใบสำคัญจ่าย" Font-Bold="True" Font-Names="Tahoma"
        Font-Size="16px" Font-Underline="True" />
    <br />
    <br />
    <asp:Label ID="Label2" runat="server" Text="เงื่อนไข" Font-Bold="True" Font-Names="Tahoma"
        Font-Size="14px" Font-Underline="True" /><br />
    <dw:WebDataWindowControl ID="DwMain" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" DataWindowObject="d_bg_reprintreceipt_main"
        LibraryList="~/DataWindow/budget/budget.pbl" ClientFormatting="True" ClientEventButtonClicked="OnDwMainButtonClicked"
        ClientEventItemChanged="OnDwMainItemChanged">
    </dw:WebDataWindowControl>
    <asp:Label ID="Label3" runat="server" Text="รายการ" Font-Bold="True" Font-Names="Tahoma"
        Font-Size="14px" Font-Underline="True" /><br />
    <dw:WebDataWindowControl ID="DwList" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" DataWindowObject="d_bg_reprintreceipt_list"
        LibraryList="~/DataWindow/budget/budget.pbl" ClientFormatting="True" ClientEventButtonClicked="OnDwListButtonClicked"
        ClientEventClicked="OnDwListClicked">
    </dw:WebDataWindowControl>
</asp:Content>
