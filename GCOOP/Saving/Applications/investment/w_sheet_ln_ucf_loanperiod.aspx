<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_ln_ucf_loanperiod.aspx.cs" Inherits="Saving.Applications.investment.w_sheet_ln_ucf_loanperiod" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%=jsPostDel%>
<%=jsPostInsert%>
    <script type="text/javascript">
        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล");
        }
        function OnDwMainBClick(s, r, c) {
            if (c == "b_del") {
                Gcoop.GetEl("HdRow").value = r;
                jsPostDel();
            }
        }
        function OnClickInsertRow() {
            jsPostInsert();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <span onclick="OnClickInsertRow()" style="cursor: pointer;">
        <asp:Label ID="LbInsert1" runat="server" Text="เพิ่มแถว" Font-Bold="False" Font-Names="Tahoma"
            Font-Size="14px" Font-Underline="True" ForeColor="Blue" /></span>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="dw_lc_lccfloantypeperiod"
        LibraryList="~/DataWindow/investment/ln_ucf_all.pbl" ClientScriptable="True" ClientEventButtonClicked="OnDwMainBClick"
        AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" RowsPerPage="20">
        <PageNavigationBarSettings Position="Top" Visible="True" NavigatorType="Numeric">
            <BarStyle HorizontalAlign="Center" />
            <NumericNavigator FirstLastVisible="True" />
        </PageNavigationBarSettings>
    </dw:WebDataWindowControl>
    <asp:Label ID="Label2" runat="server" Text="หมายเหตุ : ห้ามแก้ไขรหัสประเภทเงินกู้" Font-Bold="False" Font-Names="Tahoma"
            Font-Size="Small" Font-Underline="True" ForeColor="#000000" />
    <asp:HiddenField ID="HdRow" runat="server" />
</asp:Content>
