<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsDetailAdjust.ascx.cs"
    Inherits="Saving.Applications.shrlon.ws_sl_slip_pay_class_ctrl.DsDetailAdjust" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<link id="css2" runat="server" href="../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<table style="width: 726px">
    <tr>
        <td class="style3">
            <strong style="font-size: 14px;"><u>รายละเอียดใบเสร็จที่ค้างชำระจากเรียกเก็บ </u>
            </strong>
        </td>
    </tr>
</table>
<table class="DataSourceRepeater" style="width: 650px;">
    <tr>
        <th width="5%">
        </th>
        <th width="12%">
            เลขที่ยกเลิก
        </th>
        <th width="13%">
            เลขที่ใบเสร็จ
        </th>
        <th width="13%">
            วันที่ใบเสร็จ
        </th>
        <th width="14%">
            ยอดเงิน
        </th>
        <th width="14%">
            ยอดชำระ
        </th>
        <th width="14%">
            ผู้ทำรายการ
        </th>
        <th width="17%">
            วันที่วันที่ยกเลิก
        </th>
    </tr>
    <asp:Repeater ID="Repeater1" runat="server">
        <ItemTemplate>
            <tr>
                <td align="center">
                    <asp:CheckBox ID="operate_flag" runat="server" />
                </td>
                <td >
                    <asp:TextBox ID="adjslip_no" runat="server" Style="text-align: center" ReadOnly=true></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="receipt_no" runat="server" Style="text-align: center" ReadOnly=true></asp:TextBox>
                </td>
                <td >
                    <asp:TextBox ID="ref_slipdate" runat="server" Style="text-align: center" ReadOnly=true></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="slip_amt" runat="server" Style="text-align: right" ToolTip="#,##0.00" ReadOnly=true></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="sum_slip_amt" runat="server" Style="text-align: right;ont-size: 11px;background-color: #CCFF66" ToolTip="#,##0.00" ReadOnly=true></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="entry_id" runat="server" Style="text-align: center" ReadOnly=true></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="adjslip_date" runat="server" Style="text-align: center" ReadOnly=true></asp:TextBox>
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
