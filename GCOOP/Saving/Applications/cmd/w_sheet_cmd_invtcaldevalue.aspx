<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_cmd_invtcaldevalue.aspx.cs" 
Inherits="Saving.Applications.cmd.w_sheet_cmd_invtcaldevalue" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%=jsProcess%>
<%=jsAccYear%>
    <script type="text/javascript">

        function OnDwMainClick(s, r, c) {
            if (c == "b_process") {
                jsProcess();
            }
        }

        function OnDwMainChange(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            if (c == "acc_year") {
                jsAccYear();
            }
            else if (c == "invt_month") {
                jsAccYear();
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_main_invtcaldevalue"
        LibraryList="~/DataWindow/Cmd/cmd_invtcaldevalue.pbl" Width="750px" ClientScriptable="True"
        AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" ClientEventItemChanged="OnDwMainChange" ClientEventButtonClicked="OnDwMainClick" >
    </dw:WebDataWindowControl>
    <dw:WebDataWindowControl ID="DwDetail" runat="server" DataWindowObject="d_detail_invtcaldevalue"
        LibraryList="~/DataWindow/Cmd/cmd_invtcaldevalue.pbl" Width="750px" ClientScriptable="True"
        AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" Visible="true">
    </dw:WebDataWindowControl>
</asp:Content>
