<%@ Page Title="" Language="C#" MasterPageFile="~/FrameDialog.Master" AutoEventWireup="true"
    CodeBehind="ws_dlg_hr_master_search.aspx.cs" Inherits="Saving.Applications.hr.dlg.ws_dlg_hr_master_search_ctrl.ws_dlg_hr_master_search" %>

<%@ Register Src="DsCriteria.ascx" TagName="DsCriteria" TagPrefix="uc1" %>
<%@ Register Src="DsList.ascx" TagName="DsList" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function OnDsListClicked(s, r, c) {
            var emp_no = dsList.GetItem(r, "emp_no");
            try {
                window.opener.GetEmpNoFromDlg(emp_no);
                window.close();
            } catch (err) {
                parent.GetEmpNoFromDlg(emp_no);
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
                <uc1:DsCriteria ID="dsCriteria" runat="server" />
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
