<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsMain.ascx.cs" Inherits="Saving.Applications.hr.ws_hr_payroll_process_ctrl.DsMain" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table class="DataSourceFormView" style="width: 500px">
            <tr>
                <td width="15%">
                    <div>
                        <span>วันที่โอน:</span></div>
                </td>
                <td width="20%">
                    <asp:TextBox ID="tran_date" runat="server" Style="text-align: center;"></asp:TextBox>
                </td>
                <td width="15%">
                    <div>
                        <span>ประเภท:</span></div>
                </td>
                <td width="30%">
                    <asp:DropDownList ID="emptype_code" runat="server">
                        </asp:DropDownList>
                </td>
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>
