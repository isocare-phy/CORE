<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_ln_ucf_loanstatement.aspx.cs" Inherits="Saving.Applications.investment.w_sheet_ln_ucf_loanstatement" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <span onclick="OnClickInsertRow()" style="cursor: pointer;">
        <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="dw_lc_lcucfloanitemtype"
            LibraryList="~/DataWindow/investment/ln_ucf_all.pbl" ClientScriptable="True" ClientEventButtonClicked="OnDwMainBClick"
            AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
            RowsPerPage="20">
            <PageNavigationBarSettings Position="Top" Visible="True" NavigatorType="Numeric">
                <BarStyle HorizontalAlign="Center" />
                <NumericNavigator FirstLastVisible="True" />
            </PageNavigationBarSettings>
        </dw:WebDataWindowControl>
        <asp:Label ID="Label2" runat="server" Text="หมายเหตุ : หน้าจอนี้เป็น System Config ไม่สามารถ ลบ/เพิ่ม ได้"
            Font-Bold="False" Font-Names="Tahoma" Font-Size="Small" Font-Underline="True"
            ForeColor="#000000" />
        <asp:HiddenField ID="HdRow" runat="server" />
</asp:Content>
