<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_bg_budgetadjust.aspx.cs" Inherits="Saving.Applications.budget.w_sheet_bg_budgetadjust" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript %>
    <%=FilterBgCode %>
    <%=FilterSortSeq %>
    <%=FilterToSortSeq %>

    <script type="text/javascript">

        function Validate() {
            objDwMain.AcceptText();
            return confirm("ยืนยันการบันทึกข้อมูล");
        }

        function DwMainItemChange(sender, rowNumber, columnName, newValue) {
            if (columnName == "budgetyear") {
                objDwMain.SetItem(rowNumber, columnName, newValue);
                objDwMain.AcceptText();
                FilterBgCode();
            }
            else if (columnName == "seq_no") {
                objDwMain.SetItem(rowNumber, columnName, newValue);
                objDwMain.AcceptText();
                FilterSortSeq();
            }
            else if (columnName == "to_seq_no") {
                objDwMain.SetItem(rowNumber, columnName, newValue);
                objDwMain.AcceptText();
                FilterToSortSeq();
            }
        }
        
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" DataWindowObject="d_bg_budgetadjust"
        LibraryList="~/DataWindow/budget/budget.pbl" ClientFormatting="True" ClientEventItemChanged="DwMainItemChange">
    </dw:WebDataWindowControl>
</asp:Content>
