<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_pm_receivable.aspx.cs" Inherits="Saving.Applications.pm.w_sheet_pm_receivable" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%=JsPostCalintDate%>
<%=RunProcess%>
    <script type="text/javascript">
        function MenubarNew() {
            if (confirm("ยืนยันการล้างหน้าจอ")) {
                window.location = "";
            }
        }
        function DwMainItemChange(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            if (c == "calintto_tdate") {
                Gcoop.GetEl("hdate").value = v;
//                JsPostCalintDate();
            }
        }
        function OnDwButtomClicked(s, r, c, v) {
            switch (c) {
                case "b_1":
                    RunProcess();
                    break;
            }

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <div align="center">
        <dw:WebDataWindowControl ID="DwMain" runat="server" LibraryList="~/DataWindow/pm/pm_investment.pbl"
        DataWindowObject="d_calinto_date" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientEventItemChanged="DwMainItemChange" ClientEventButtonClicked="OnDwButtomClicked">
    </dw:WebDataWindowControl>
    </div>

        <br />
       <br />
<%--    <asp:Panel ID="Panel1" runat="server" ScrollBars="Horizontal" Height="400" Width="700">
    <dw:WebDataWindowControl ID="DwDetail" runat="server" LibraryList="~/DataWindow/pm/pm_investment.pbl"
        DataWindowObject="d_account_due_date2" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        >
    </dw:WebDataWindowControl>
    </asp:Panel>--%>
        <asp:HiddenField ID="hdate" runat="server" />
</asp:Content>
