<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsList.ascx.cs" Inherits="Saving.Applications.hr.w_sheet_hr_salamas_ctrl.DsList" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<table class="DataSourceRepeater" style="width: 400px;">
    <tr>
        <th width="20%">
            รายการ
        </th>
        <th width="20%">
            จำนวนเงิน
        </th>
    </tr>
    <tbody>
        <asp:Repeater ID="Repeater1" runat="server">
            <ItemTemplate>
                <tr>
                    <td>
                        <asp:TextBox ID="PAYLIST" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="PAYROLL_AMT" runat="server" Style="text-align: right;" ToolTip="#,##0.00"></asp:TextBox>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </tbody>
</table>
