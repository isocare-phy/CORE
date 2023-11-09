<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_pm_saveint.aspx.cs" Inherits="Saving.Applications.pm.w_sheet_pm_saveint" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=GetLoan%>
    <script type="text/JavaScript">
        function MenubarNew() {
            if (confirm("ยืนยันการล้างหน้าจอ")) {
                window.location = "";
            }
        }
        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล");
        }

        function OnClickCri(s, r, c) {
            if (c == 'b_1') {
                GetLoan();
            }
        }

        function OnChangeCri(s, r, c, v) {
            objDwMain.SetItem(r, c, v);
            objDwMain.AcceptText();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" DataWindowObject="d_cri_date_int"
        LibraryList="~/DataWindow/pm/pm_investment.pbl" ClientFormatting="True" ClientEventClicked="OnClickCri" ClientEventItemChanged="OnChangeCri">
    </dw:WebDataWindowControl>
    <br />
    <asp:Panel ID="Panel2" runat="server" ScrollBars="Auto" Width="750px">
        <dw:WebDataWindowControl ID="DwDetail" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
            AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" DataWindowObject="d_recperiod_int_list"
            LibraryList="~/DataWindow/pm/pm_investment.pbl" ClientFormatting="True" ClientEventClicked="OnDwListClicked">
        </dw:WebDataWindowControl>
    </asp:Panel>
</asp:Content>
