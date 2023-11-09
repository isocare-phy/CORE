<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsTax.ascx.cs" Inherits="Saving.Applications.hr.w_sheet_hr_tax_ctrl.DsTax" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<table class="DataSourceRepeater">
    <tr>
        <th>
            เงินได้สุทธิ
        </th>
        <th>
            เงินได้สุทธิจริง
        </th>
        <th>
            อัตราภาษี
        </th>
        <th>
            ภาษีที่ต้องชำระ
        </th>
    </tr>
    <tbody>
        <asp:Repeater ID="Repeater1" runat="server">
            <ItemTemplate>
                <tr>
                    <td>
                        <asp:TextBox ID="cp_minmaxrate" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="GOSS_RATE" runat="server" Style="text-align: right;" ToolTip="#,##0.00"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="cp_taxrate" runat="server" Style="text-align: right;" ToolTip="#,##0.00"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="Taxcall" runat="server" Style="text-align: right;" ToolTip="#,##0.00"></asp:TextBox>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </tbody>
</table>
