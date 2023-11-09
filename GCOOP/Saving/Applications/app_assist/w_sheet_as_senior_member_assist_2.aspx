<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_as_senior_member_assist_2.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_as_senior_member_assist_2" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=postDepptacount%>
    <%=postSearch %>
    <%=postToFromAccid%>
    <script type="text/javascript">
        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล?");
        }
        function OnBDwMainClicked(s, r, c, v) {
            if (c == "b_1") {
                var member_no = objDwMain.GetItem(r, "member_no");
                Gcoop.GetEl("HfRow").value = r;
                Gcoop.OpenIFrame(580, 500, "w_dlg_as_deptaccount_no.aspx", "?member_no=" + member_no);
            } else if (c == "b_2") {
                var moneytype_code = objDwMain.GetItem(r, "moneytype_code");
                Gcoop.GetEl("HfRow").value = r;
                Gcoop.OpenIFrame(305, 500, "w_dlg_as_tofrom_accid.aspx", "?moneytype_code=" + moneytype_code);
            }
        }
        function GetDeptAccountNo(deptaccount_no) {
            Gcoop.GetEl("Hfdeptaccount_no").value = deptaccount_no;
            postDepptacount();
        }
        function GetToFromAccid(account_id) {
            Gcoop.GetEl("Hfaccount_id").value = account_id;
            postToFromAccid();
        }
        function OnBDwHeadClicked(s, r, c, v) {
            if (c == "b_search") {
                postSearch();
            }
        }
        function OnDwHeadChanged(s, r, c, v) {
            objDwHead.SetItem(r, c, v);
            objDwHead.AcceptText();
            if (c == "capital_tdate") {
                Gcoop.GetEl("Hfcapital_tdate").value = v;
                //                postSearch();
            } else if (c == "capital_edate") {
                Gcoop.GetEl("Hfcapital_edate").value = v;
                //                postSearch();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <asp:Literal ID="LtAlert" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwHead" runat="server" LibraryList="~/DataWindow/app_assist/as_capital.pbl"
        DataWindowObject="d_as_approve_type_assist" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientEventButtonClicked="OnBDwHeadClicked" ClientEventItemChanged="OnDwHeadChanged">
    </dw:WebDataWindowControl>
    <asp:CheckBox ID="ChkAll" runat="server" AutoPostBack="True" Text=" อนุมัติใบคำขอทั้งหมด "
        OnCheckedChanged="CheckAllChanged" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Label ID="lblRowCount" runat="server" Text="" Font-Bold="true"></asp:Label>
    <dw:WebDataWindowControl ID="DwMain" runat="server" LibraryList="~/DataWindow/app_assist/as_capital.pbl"
        DataWindowObject="d_as_senior_member_assist_2" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        Width="750px" Height="430px" RowsPerPage="15" ClientEventButtonClicked="OnBDwMainClicked"
        ClientEventItemChanged="OnDwMainChanged">
        <PageNavigationBarSettings Position="Top" Visible="True" NavigatorType="Numeric">
            <BarStyle HorizontalAlign="Center" />
            <NumericNavigator FirstLastVisible="True" />
        </PageNavigationBarSettings>
    </dw:WebDataWindowControl>
    <asp:HiddenField ID="HfMemNo" runat="server" />
    <asp:HiddenField ID="HfSql" runat="server" />
    <asp:HiddenField ID="HfRow" runat="server" />
    <asp:HiddenField ID="Hfdeptaccount_no" runat="server" />
    <asp:HiddenField ID="Hfmoneytype_code" runat="server" />
    <asp:HiddenField ID="HfSqlFlag" runat="server" />
    <asp:HiddenField ID="Hfaccount_id" runat="server" />
    <asp:HiddenField ID="Hfcapital_tdate" runat="server" />
    <asp:HiddenField ID="Hfcapital_edate" runat="server" />
</asp:Content>
