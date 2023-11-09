<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsList.ascx.cs" 
Inherits="Saving.Applications.hr.ws_hr_constant_ucfeduamt_ctrl.DsList" %>
<link id="css1" runat="server" href="../../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:Panel ID="Panel1" runat="server" HorizontalAlign="Center">
    <table class="DataSourceRepeater" align="center" style="width: 700px;">
        <tr>
            <th width="5%">
            </th>
            <th width="10%">
                รหัส
            </th>
            <th width="25%">
                ระดับการศึกษา
            </th>
            <th width="25%">
                จำนวนเงินที่เบิกได้
            </th>
            <th width="5%">
            </th>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="Panel2" runat="server" Height="500px" ScrollBars="Auto" HorizontalAlign="Center">
    <table class="DataSourceRepeater" align="center" style="width: 700px;">
        <asp:Repeater ID="Repeater1" runat="server">
            <ItemTemplate>
                <tr>
                    <td width="5%">
                        <asp:TextBox ID="running_number" runat="server" Style="text-align: center;"></asp:TextBox>
                    </td>
                    <td width="10%">
                        <asp:TextBox ID="edu_code" runat="server" Style="text-align: center;"></asp:TextBox>
                    </td>
                    <td width="25%">
                        <asp:DropDownList ID="education_code" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td width="25%">
                        <asp:TextBox ID="edu_amt" runat="server" Style="text-align: right" ToolTip="#,##0.00"></asp:TextBox>
                    </td>
                    <td width="5%">
                        <asp:Button ID="b_delete" runat="server" Text="ลบ" />
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Panel>
