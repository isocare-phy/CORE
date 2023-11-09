<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsDetailLoan.ascx.cs"
    Inherits="Saving.Applications.shrlon.ws_sl_slip_pay_mth_ctrl.DsDetailLoan" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<link id="css2" runat="server" href="../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<%--<table>
    <tr>
        <td colspan="2">
        <asp:CheckBox ID="chkdsDetailLoan" Checked="false" runat="server" onclick="Open_tabledsDetailLoan()" />
            <strong style="font-size: 12px;">รายการหนี้</strong>
            
         <span
                style="font-size: 12px">แก้ไขรายการหนี้</span>
        </td>     
    </tr>
</table>--%>
<%--<table class="DataSourceFormView" style="width: 770px;">
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
                  ยอดชำระ :
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
</table>--%>
<table class="DataSourceRepeater" style="width: 770px;">
    <tr>
        <th>
        </th>
        <th style="font-size: 12px;" >
            งวด
        </th>
        <th style="font-size: 12px;">
            ใบเสร็จ
        </th>
        <th style="font-size: 12px;" >
            ค้างชำระต้น
        </th>
        <th style="font-size: 12px;" >
            ค้างชำระด/บ
        </th>
        <th style="font-size: 12px;" >
            ชำระต้น
        </th>
        <th style="font-size: 12px;" >
            ชำระ ด/บ
        </th>
        <th style="font-size: 12px;" >
            รวมชำระ
        </th>
        <th width="4%" >
        </th>
    </tr>
    <asp:Repeater ID="Repeater2" runat="server">
        <ItemTemplate>
            <tr>
                <td align="center" width="5%">
                    <asp:CheckBox ID="operate_flag" runat="server" />
                </td>
                <td width="5%">
                    <asp:TextBox ID="recv_period" runat="server" Style="text-align: center; font-size: 11px;
                        background-color: #EEEEEE;"></asp:TextBox>
                </td>
                <td align="center" width="10%">
                   <asp:TextBox ID="receipt_no" runat="server" Style="text-align: center; font-size: 11px;
                        background-color: #EEEEEE;"></asp:TextBox>
                </td>
                <td width="10%">
                    <asp:TextBox ID="adjust_prnamt" runat="server" Style="text-align: right; font-size: 11px;
                        background-color: #EEEEEE;" ToolTip="#,##0.00" ReadOnly="true"></asp:TextBox>
                </td>
                 <td width="10%">
                    <asp:TextBox ID="adjust_intamt" runat="server" Style="text-align: right; font-size: 11px;
                        background-color: #EEEEEE;" ToolTip="#,##0.00" ReadOnly="true"></asp:TextBox>
                </td>
                <td width="10%">
                    <asp:TextBox ID="principal_payamt" runat="server" Style="text-align: right; font-size: 11px;
                        background-color: #CCFF66;" ToolTip="#,##0.00"></asp:TextBox>
                </td>
                <td width="10%">
                    <asp:TextBox ID="interest_payamt" runat="server" Style="text-align: right; font-size: 11px;
                        background-color: #CCFF66;" ToolTip="#,##0.00"></asp:TextBox>
                </td>
                <td width="10%">
                    <asp:TextBox ID="item_payamt" runat="server" Style="text-align: right; font-size: 11px;
                        background-color: #CCFF66;" ToolTip="#,##0.00"></asp:TextBox>
                </td>
                <td width="5%">
                        <asp:Button ID="bloan_detail" runat="server" Text=".."/>
                    </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
<table class="DataSourceFormView" style="width: 770px;">
   <tr>
                <td width="5%" style="border: 0px;">
                </td >
                <td width="5%" style="border: 0px;">
                </td >
                
                <th width="10%">
                 <span style="font-size: 12px;">ค้างรวม : </span>
                </th>
                <td width="10%">
                 <asp:TextBox ID="sum_prnamt" runat="server" Style="text-align: right; font-size: 11px;
                        background-color: Black;" ForeColor="GreenYellow" ToolTip="#,##0.00" ></asp:TextBox>
                </td>
                 <td width="10%">
                  <asp:TextBox ID="sum_intamt" runat="server" Style="text-align: right; font-size: 11px;
                        background-color: Black;" ForeColor="GreenYellow" ToolTip="#,##0.00"></asp:TextBox>
                </td>
                <td width="10%" style="border: 0px;">
                   
                </td>
                <td width="10%" style="border: 0px;">
                   
                </td>
                <td width="10%" style="border: 0px;">
      
                </td>
                <td width="5%" style="border: 0px;">
                </td>
</tr>
</table>
