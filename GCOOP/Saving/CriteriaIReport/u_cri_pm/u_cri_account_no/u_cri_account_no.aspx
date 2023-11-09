<%@ Page Title="" Language="C#" MasterPageFile="~/Report.Master" AutoEventWireup="true" CodeBehind="u_cri_account_no.aspx.cs" Inherits="Saving.CriteriaIReport.u_cri_pm.u_cri_account_no.u_cri_account_no" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
        type="text/css" />
        <script type="text/javascript">
            function open_search_dlg() {
            }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <center>
        <asp:Label ID="ReportName" runat="server" Text="ชื่อรายงาน" Enabled="False" EnableTheming="False"
            Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Size="Large"
            Font-Underline="False"></asp:Label></center>
    <br />
    <table class="iReportDataSourceFormView">
        <tr>
        <td style="width: 25%">
                
            </td>
            <td style="width: 20%">
                <div>
                    <span>เลขบัญชีลงทุน :</span></div>
            </td>
            <td>
                <asp:TextBox ID="account_no" runat="server" Width="120px" MaxLength="10" ></asp:TextBox>
            </td>
        </tr>
    </table>
</asp:Content>

