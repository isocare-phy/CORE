<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_bg_budgetclosemonth.aspx.cs"
    Inherits="Saving.Applications.budget.w_sheet_bg_budgetclosemonth" Title="Untitled Page" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=CloseMonthProcess%>
    <script type="text/javascript">
    function OnDwMainButtonClicked(sender, rowNmber, buttonName){
        if(buttonName == "cb_ok"){
            var year = 0;
            var month = 0;
            try{
                year = objDwMain.GetItem(1, "year");
            }catch(err){ year = 0};
            try{
                month = objDwMain.GetItem(1, "month");
            }
            catch(err){ month = 0 };
            if(year != 0 && year != null && month != 0 && month != null){
                CloseMonthProcess();
            }
            else{
                alert("กรุณาเลือก ปี และเดือนก่อนทำการปิดสิ้นเดือน");
            }
        }
    }
    function OnDwMainItemChanged(sender, rowNumber, columnName, newValue){
        objDwMain.SetItem(rowNumber, columnName, newValue);
        objDwMain.AcceptText();
    }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <br />
    <asp:Label ID="Label1" runat="server" Text="ปิดสิ้นเดือน" Font-Bold="True" Font-Names="Tahoma"
        Font-Size="16px" Font-Underline="True" />
    <br /><br />
    <dw:WebDataWindowControl ID="DwMain" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" DataWindowObject="d_bg_budgetclosemonth"
        LibraryList="~/DataWindow/budget/budget.pbl" ClientFormatting="True" ClientEventButtonClicked="OnDwMainButtonClicked" ClientEventItemChanged="OnDwMainItemChanged">
    </dw:WebDataWindowControl>
</asp:Content>
