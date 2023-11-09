<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsDetailAdjust.ascx.cs" Inherits="Saving.Applications.shrlon.ws_sl_slip_pay_ctrl.DsDetailAdjust" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet" type="text/css" />
<link id="css2" runat="server" href="../../JsCss/DataSourceTool.css" rel="stylesheet" type="text/css" />
<table class="DataSourceRepeater" style="width: 650px;">
    <tr>
        <th width="5%">
        </th>
        <th width="18%">
            เลขที่ยกเลิก
        </th>        
        <th width="15%">
            เลขที่ใบเสร็จ
        </th>
        <th width="15%">
            วันที่ใบเสร็จ
        </th>
        <th width="15%">
            ยอดเงิน
        </th>
        <th width="15%">
            ผู้ทำรายการ
        </th>
        <th width="17%">
            วันที่วันที่ยกเลิก
        </th>
    </tr>
</table>
<asp:Panel ID="Panel2" runat="server" Height="320px" ScrollBars="Auto">
    <table class="DataSourceRepeater" style="width: 650px;">
        <asp:Repeater ID="Repeater1" runat="server">
            <ItemTemplate>
                <tr>
                    <td width="5%"  align="center">
                        <asp:CheckBox ID="operate_flag" runat="server" />
                    </td>
                    <td width="18%">
                        <asp:TextBox ID="adjslip_no" runat="server" Style="text-align: center"></asp:TextBox>
                    </td>                    
                    <td width="15%">
                        <asp:TextBox ID="receipt_no" runat="server" Style="text-align: center"></asp:TextBox>
                    </td>
                    <td width="15%">
                        <asp:TextBox ID="ref_slipdate" runat="server" Style="text-align: center"></asp:TextBox>
                    </td>
                    <td width="15%">
                        <asp:TextBox ID="slip_amt" runat="server" Style="text-align: right" ToolTip="#,##0.00"></asp:TextBox>
                    </td>
                    <td width="15%">
                        <asp:TextBox ID="entry_id" runat="server" Style="text-align: center"></asp:TextBox>
                    </td>
                    <td width="17%">
                        <asp:TextBox ID="adjslip_date" runat="server" Style="text-align: center"></asp:TextBox>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Panel>
