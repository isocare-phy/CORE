<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_ln_ucf_loantype.aspx.cs" Inherits="Saving.Applications.investment.w_sheet_ln_ucf_loantype" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=jsPostDel%>
    <%=jsPostInsert%>
    <%=jsPostLoanType%>
    <%=jsPostContintType%>
    <script type="text/javascript">
        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล");
        }
        function OnDwMainBClick(s, r, c) {
            if (c == "b_del") {
                Gcoop.GetEl("HdRow").value = r;
                jsPostDel();
            }
            if (c == "b_1") {
                Gcoop.GetEl("HdRow").value = r;
                jsPostLoanType();
            }
        }
        function OnClickInsertRow() {
            jsPostInsert();
        }
        function OnDwDetailChange(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            if (c == "contint_type") {
                jsPostContintType();
            }
        }
        //        function OnDwMainClick(s, r, c) {
        //            Gcoop.GetEl("HdRow").value = r;
        //            jsPostLoanType();
        //        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <span onclick="OnClickInsertRow()" style="cursor: pointer;">
        <asp:Label ID="LbInsert1" runat="server" Text="เพิ่มแถว" Font-Bold="False" Font-Names="Tahoma"
            Font-Size="14px" Font-Underline="True" ForeColor="Blue" /></span>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="dw_lc_lccfloantype"
        LibraryList="~/DataWindow/investment/ln_ucf_all.pbl" ClientScriptable="True" AutoRestoreContext="False"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" RowsPerPage="10"
        ClientEventClicked="OnDwMainClick" ClientEventButtonClicked="OnDwMainBClick">
        <PageNavigationBarSettings Position="Top" Visible="True" NavigatorType="Numeric">
            <BarStyle HorizontalAlign="Center" />
            <NumericNavigator FirstLastVisible="True" />
        </PageNavigationBarSettings>
    </dw:WebDataWindowControl>
    <asp:Label ID="Label2" runat="server" Text="หมายเหตุ : ห้ามแก้ไขรหัสประเภทเงินกู้"
        Font-Bold="False" Font-Names="Tahoma" Font-Size="Small" Font-Underline="True"
        ForeColor="#000000" />
    <asp:HiddenField ID="HiddenField1" runat="server" />
    <dw:WebDataWindowControl ID="DwDetail" runat="server" DataWindowObject="dw_lc_lccfloantype1"
        LibraryList="~/DataWindow/investment/ln_ucf_all.pbl" ClientScriptable="True" AutoRestoreContext="False"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientEventItemChanged="OnDwDetailChange">
    </dw:WebDataWindowControl>
    <asp:HiddenField ID="HdRow" runat="server" />
</asp:Content>
