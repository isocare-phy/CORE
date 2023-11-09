<%@ Page Title="" Language="C#" MasterPageFile="~/Report.Master" AutoEventWireup="true" CodeBehind="u_cri_coopid_username_rdate.aspx.cs" Inherits="Saving.CriteriaIReport.u_cri_coopid_username_rdate.u_cri_coopid_username_rdate" %>
<%@ Register src="DsMain.ascx" tagname="DsMain" tagprefix="uc1" %>
<%@ Register src="DsList.ascx" tagname="DsList" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    var dsMain = new DataSourceTool();
    var dsList = new DataSourceTool();
    function OnDsMainItemChanged(s, r, c, v) {

    }

    function OnDsListClicked(s, r, c) {
        if (c == "user_name" || c == "full_name") {
            dsMain.SetItem(0, "user_name", dsList.GetItem(r, "user_name"));
        }
    }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <center>
        <asp:Label ID="ReportName" runat="server" Text="ชื่อรายงาน" Enabled="False" EnableTheming="False"
            Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Size="Large"
            Font-Underline="False"></asp:Label></center>
    <uc1:DsMain ID="dsMain" runat="server" />
    <uc2:DsList ID="dsList" runat="server" />
    </asp:Content>
