<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsDetail.ascx.cs" Inherits="Saving.Applications.keeping.ws_kp_ccl_adjust_monthly_mbgrp_ctrl.DsDetail" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet" type="text/css" />
<span style="font-size: 13px;"><font color="#cc0000"><u><strong>รายการใบเสร็จ</strong></u></font></span>
<table class="TbStyle" style="width: 710px;">
    <tr>
        <th width="5%">
            No.
        </th>
        <th width="25%">
            รายละเอียด
        </th>
        <th width="12%">
            ต้นเงิน
        </th>
        <th width="12%">
            ดอกเบี้ย
        </th>
        <th width="12%">
            รวมปรับปรุง
        </th>
        <th width="12%">
            คงเหลือ
        </th>
    </tr>
</table>
<asp:Panel ID="Panel2" runat="server" HorizontalAlign="Center">
    <table class="TbStyle" style="width: 710px;">
        <asp:Repeater ID="Repeater1" runat="server">
            <ItemTemplate>
                <tr>
                    <td width="5%">
                        <asp:CheckBox ID="operate_flag" runat="server" Style="text-align: center" />
                    </td>
                    <td width="25%">
                        <asp:TextBox ID="slipitemtype_code" runat="server" Width="20px" ReadOnly="true"></asp:TextBox>
                        <asp:TextBox ID="slipitem_desc" runat="server" Width="145px" ReadOnly="true"></asp:TextBox>
                    </td>
                    <td width="12%">
                        <asp:TextBox ID="principal_adjamt" runat="server" Style="text-align: right;" ToolTip="#,##0.00"
                            ReadOnly="true"></asp:TextBox>
                    </td>
                    <td width="12%">
                        <asp:TextBox ID="interest_adjamt" runat="server" Style="text-align: right;" ToolTip="#,##0.00"
                            ReadOnly="true"></asp:TextBox>
                    </td>
                    <td width="12%">
                        <asp:TextBox ID="item_adjamt" runat="server" Style="text-align: right;" ToolTip="#,##0.00"
                            ReadOnly="true"></asp:TextBox>
                    </td>
                    <td width="12%">
                        <asp:TextBox ID="bfshrcont_balamt" runat="server" Style="text-align: right;" ToolTip="#,##0.00"
                            ReadOnly="true"></asp:TextBox>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Panel>