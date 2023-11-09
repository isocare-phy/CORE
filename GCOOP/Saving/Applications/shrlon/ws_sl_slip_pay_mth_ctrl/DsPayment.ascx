<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsPayment.ascx.cs" Inherits="Saving.Applications.shrlon.ws_sl_slip_pay_mth_ctrl.DsPayment" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<link id="css2" runat="server" href="../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit" Width="770px">
    <EditItemTemplate>
        <table class="DataSourceFormView" style="width: 770px;">
        <tr>
                <td width="5%" style="border: 0px;">
                </td >
                <td width="5%" style="border: 0px;">
                </td >
                
                <td width="10%" style="border: 0px;">
                </td>
                <td width="10%" style="border: 0px;">
                </td>
                 <th width="10%">
                 <span style="font-size: 12px;">ยอดชำระ : </span>
                </th>
                <td width="10%">
                    <asp:TextBox ID="sumprn_payment" runat="server" Style="text-align: right; font-size: 11px;
                        background-color: #CCFF66;" ToolTip="#,##0.00"></asp:TextBox>
                </td>
                <td width="10%">
                    <asp:TextBox ID="sumint_payment" runat="server" Style="text-align: right; font-size: 11px;
                        background-color: #CCFF66;" ToolTip="#,##0.00"></asp:TextBox>
                </td>
                <td width="10%">
                    <asp:TextBox ID="sumitem_payment" runat="server" Style="text-align: right; font-size: 11px;
                        background-color: #CCFF66;" ToolTip="#,##0.00"></asp:TextBox>
                </td>
                <td width="5%" style="border: 0px;">
                </td>
</tr>
 </table>
    </EditItemTemplate>
</asp:FormView>