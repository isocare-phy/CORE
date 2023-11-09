<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsList.ascx.cs" Inherits="Saving.Applications.mis.w_sheet_mssysbal_msv_ctrl.DsList" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />


<table class="DataSourceRepeater">
    <tr>
    <th width="10%">
            หมวด
        </th>
        <th width=15%">
            เงินทุน
        </th>
        <th width="25%">
           ประเภท
        </th>
        <th width="20%">
            จำนวนเงิน
        </th>
          <th width="10%">
            อัตรา(%)
        </th>
    </tr>
    <tbody>
        <asp:Repeater ID="Repeater1" runat="server">
            <ItemTemplate>
                <tr>
                    <td width="10%">
                        <asp:TextBox ID="BIZZ_SYSTEM_DET" runat="server"  Style="text-align: center;" Enabled="False"></asp:TextBox>
                    </td>
                     <td width="15%">
                        <asp:TextBox ID="BIZZ_SYSTEM_DESC" runat="server"  Style="text-align: center;" Enabled="False"></asp:TextBox>
                    </td>
                      <td width="25%">
                        <asp:TextBox ID="BIZZTYPE_CODE_DESC" runat="server" Style="text-align: left;" Enabled="False"></asp:TextBox>
                    </td>
                     <td width="20%">
                        <asp:TextBox ID="BALANCE_VALUE" runat="server" Style="text-align: right;" ToolTip="#,##0.00" Enabled="False"></asp:TextBox>
                    </td>
                    <td width="10%">
                        <asp:TextBox ID="BIZZTYPE_RATE" runat="server" Style="text-align: center;" ToolTip="#,##0.00"></asp:TextBox>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </tbody>
</table>
