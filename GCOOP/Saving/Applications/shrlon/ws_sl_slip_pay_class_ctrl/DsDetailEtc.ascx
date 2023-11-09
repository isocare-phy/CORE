<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsDetailEtc.ascx.cs"
    Inherits="Saving.Applications.shrlon.ws_sl_slip_pay_class_ctrl.DsDetailEtc" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<link id="css2" runat="server" href="../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<table class="DataSourceRepeater" style="width: 770px;">
    <tr>
        <th width="5%">
        </th>
        <th width="15%" style="font-size: 12px;">
            รหัส
        </th>
        <th width="30%" style="font-size: 12px;">
            รายละเอียด
        </th>
        <th width="15%" style="font-size: 12px;">
            เงินชำระ
        </th>
        <th width="5%">
        </th>
    </tr>
    <asp:Repeater ID="Repeater3" runat="server">
        <ItemTemplate>
            <tr>
                <td align="center">
                    <asp:CheckBox ID="operate_flag" runat="server" />
                </td>
                <td>
                    <asp:DropDownList ID="slipitemtype_code" runat="server">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:TextBox ID="slipitem_desc" runat="server" Style="text-align: left; font-size: 11px;"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="item_payamt" runat="server" Style="text-align: right; font-size: 11px;
                        background-color: #CCFF66;" ToolTip="#,##0.00" onBlur="checkValue(this,this.defaultValue)"
                        onFocus="clearValue(this,this.defaultValue)"></asp:TextBox>
                </td>
                <td>
                    <asp:Button ID="b_del" runat="server" Text="-" />
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
