<%@ Page Title="" Language="C#" MasterPageFile="~/FrameDialog.Master" AutoEventWireup="true"
    CodeBehind="w_dlg_dealer_search.aspx.cs" Inherits="Saving.Applications.cmd.dlg.w_dlg_dealer_search_ctrl.w_dlg_dealer_search" %>

<%@ Register Src="DsCriteria.ascx" TagName="DsCriteria" TagPrefix="uc1" %>
<%@ Register Src="DsList.ascx" TagName="DsList" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var dsList = new DataSourceTool();
        var DsCriteria = new DataSourceTool();

        function OnDsCriteriaClicked(s, r, c) {
        }

        function OnDsListItemChanged(s, r, c, v) {
        }

        function OnDsCriteriaClicked(s, r, c) {
        }

        function OnDsListItemChanged(s, r, c, v) {
        }

        function OnDsListClicked(s, r, c) {
            var dealerno = dsList.GetItem(r, "dealer_no");
            try {
                window.opener.GetValueFromDlg(dealerno);
                window.close();
            } catch (err) {
                parent.GetValueFromDlg(dealerno);
                parent.RemoveIFrame();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <table>
        <tr>
            <td>
                <uc1:DsCriteria ID="DsCriteria" runat="server" />
            </td>
            <td>
                <asp:Button ID="BtSearch" runat="server" Text="ค้น" Width="60px" Height="60px" OnClick="BtSearch_Click" />
            </td>
        </tr>
    </table>
    <br />
    <uc2:DsList ID="dsList" runat="server" />
    <asp:Label ID="LbCount" runat="server" ForeColor="Green" Font-Bold="True" Font-Names="Tahoma"
        Font-Size="14px"></asp:Label>
    <br />
</asp:Content>
