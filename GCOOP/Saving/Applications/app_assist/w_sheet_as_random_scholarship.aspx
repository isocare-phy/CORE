<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_as_random_scholarship.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_as_random_scholarship" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript %>
    <%=postRandom %>
    <script type="text/javascript">

        function DwCriButtonClick(sender, rowNumber, buttonName) {
            if (buttonName == "b_random") {
                objDwCri.AcceptText();
                postRandom();
            }
        }

        function DwCriItemChange(sender, rowNumber, columnName, newValue) {
            objDwCri.SetItem(rowNumber, columnName, newValue);
            objDwCri.AcceptText();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwCri" runat="server" LibraryList="~/DataWindow/app_assist/as_capital.pbl"
        DataWindowObject="dw_cri_level_school_random" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientEventButtonClicked="DwCriButtonClick" ClientEventItemChanged="DwCriItemChange">
    </dw:WebDataWindowControl>
</asp:Content>
