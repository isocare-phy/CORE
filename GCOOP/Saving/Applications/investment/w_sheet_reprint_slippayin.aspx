<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_reprint_slippayin.aspx.cs" Inherits="Saving.Applications.investment.w_sheet_reprint_slippayin" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=jsFind%>
    <%=jsPrint%>
    <script type="text/javascript">

        function OnDwMainClicked(s, r, c) {
            if (c == "retrive") {
                jsFind();
            }
        }

        function OnDwDetailButtonClicked(s, r, c) {
            if (c == "print") {

                Gcoop.GetEl("HdRow").value = r;
                //        Gcoop.GetEl("HdSlipno").value = objDwMain.GetItem(r, "payinslip_no");
                jsPrint();

            }
        }

        function ItemChanged(sender, rowNumber, columnName, newValue) {
            if (columnName == "member_no") {
                sender.SetItem(rowNumber, columnName, Gcoop.StringFormat(newValue, "000000"));
                sender.AcceptText();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="dw_main" runat="server" DataWindowObject="d_cri_rdate"
        LibraryList="~/DataWindow/investment/loan_slippayin.pbl" AutoRestoreContext="False"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
        Width="720px" ClientEventItemChanged="ItemChanged" ClientEventButtonClicked="OnDwMainClicked"
        TabIndex="1" ClientFormatting="True">
    </dw:WebDataWindowControl>
    <br />
    <dw:WebDataWindowControl ID="dw_detail" runat="server" DataWindowObject="d_lc_slippayin_reprint"
        LibraryList="~/DataWindow/investment/loan_slippayin.pbl" AutoRestoreContext="False"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True "
        ClientEventButtonClicked="OnDwDetailButtonClicked" ClientFormatting="True">
    </dw:WebDataWindowControl>
    <asp:HiddenField ID="HdRow" runat="server" />
</asp:Content>
