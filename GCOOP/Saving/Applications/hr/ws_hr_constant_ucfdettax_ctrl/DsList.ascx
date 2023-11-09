<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsList.ascx.cs" Inherits="Saving.Applications.hr.ws_hr_constant_ucfdettax_ctrl.DsList" %>
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
            <th width="15%">
                ช่วงเงินได้ขั้นต่ำสุด
            </th>
            <th width="15%">
                ช่วงเงินได้ขั้นสูงสุด
            </th>
            <th width="10%">
                อัตราภาษีร้อยละ
            </th>
            <th width="20%">
                ภาษีแต่ละขั้นเงินได้สุทธิ
            </th>
            <th width="20%">
                ภาษีสะสมสูงสุดของขั้น
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
                        <asp:TextBox ID="taxid" runat="server" Style="text-align: center;"></asp:TextBox>
                    </td>
                    <td width="15%">
                        <asp:TextBox ID="minrate" runat="server" Style="text-align: right;"></asp:TextBox>
                    </td>
                    <td width="15%">
                        <asp:TextBox ID="maxrate" runat="server" Style="text-align: right;"></asp:TextBox>
                    </td>
                    <td width="10%">
                        <asp:TextBox ID="pertaxrate" runat="server" Style="text-align: center;"></asp:TextBox>
                    </td>
                    <td width="20%">
                        <asp:TextBox ID="paytax" runat="server" Style="text-align: right;"></asp:TextBox>
                    </td>
                    <td width="20%">
                        <asp:TextBox ID="maxpaytax" runat="server" Style="text-align: right;"></asp:TextBox>
                    </td>
                    <td width="5%">
                        <asp:Button ID="b_delete" runat="server" Text="ลบ" />
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Panel>
