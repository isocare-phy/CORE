<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wd_statement.ascx.cs"
    Inherits="Saving.Applications.shrlon.ws_sl_follow_edit_ctrl.wd_statement" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<link id="css2" runat="server" href="../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:Panel ID="Panel3" runat="server" Height="400px" Width="700px" ScrollBars="Auto">
<asp:Panel ID="Panel1" runat="server" HorizontalAlign="Center">
    <table class="DataSourceRepeater" style="width: 900px;">
        <tr>
            <th width="4%">
            </th>
            <th width="7%">
                งวดรายเดือน
            </th>
            <th width="7%">
                รหัสรายการ
            </th>
            <th width="9%">
                ต้นชำระ
            </th>
            <th width="9%">
                ดอกชำระ
            </th>
            <th width="10%">
                รวมชำระ
            </th>
            <th width="9%">
                ตั้งแต่วันที่
            </th>
            <th width="9%">
                ถึงวันที่
            </th>
             <th width="9%">
                ดอกเบี้ยงวด
            </th>
             <th width="9%">
                ต้นค้าง
            </th>
             <th width="9%">
                ดอกค้าง
            </th>
             <th width="5%">
                สถานะ
            </th>
            <th width="4%">
            </th>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="Panel2" runat="server" HorizontalAlign="Center">
    <table class="DataSourceRepeater" style="width: 900px;">
        <asp:Repeater ID="Repeater1" runat="server">
            <ItemTemplate>
                <tr>
                    <td width="4%">
                        <asp:TextBox ID="seq_no" runat="server" Style="text-align: center;"></asp:TextBox>
                    </td>
                    <td width="7%">
                        <asp:TextBox ID="recv_period" runat="server" Style="text-align: center;"></asp:TextBox>
                    </td>
                    <td width="7%">
                    <asp:DropDownList ID="sliptype_code" runat="server">
                        </asp:DropDownList>                        
                    </td>
                    <td width="9%">
                       <asp:TextBox ID="principal_payment" runat="server" Style="text-align: right;" ToolTip="#,##0.00"></asp:TextBox>
                    </td>
                    <td width="9%">
                        <asp:TextBox ID="interest_payment" runat="server" Style="text-align: right;" ToolTip="#,##0.00"></asp:TextBox>
                    </td>
                     <td width="10%">
                        <asp:TextBox ID="item_payment" runat="server" Style="text-align: right;" ToolTip="#,##0.00"></asp:TextBox>
                    </td>
                    <td width="9%">
                       <asp:TextBox ID="calint_from" runat="server" Style="text-align: center;"></asp:TextBox>
                    </td>
                    <td width="9%">
                       <asp:TextBox ID="calint_to" runat="server" Style="text-align: center;"></asp:TextBox>
                    </td>
                    <td width="9%">
                        <asp:TextBox ID="interest_period" runat="server" Style="text-align: right;" ToolTip="#,##0.00"></asp:TextBox>
                    </td>
                    <td width="9%">
                        <asp:TextBox ID="principal_arrear" runat="server" Style="text-align: right;" ToolTip="#,##0.00"></asp:TextBox>                                        
                    </td>
                     <td width="9%">
                        <asp:TextBox ID="interest_arrear" runat="server" Style="text-align: right;" ToolTip="#,##0.00"></asp:TextBox> 
                    </td>
                      <td width="5%">
                        <asp:TextBox ID="item_status" runat="server" Style="text-align: center;"></asp:TextBox>
                    </td>
                    <td width="4%">
                        <asp:Button ID="b_delete" runat="server" Text="-" />
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Panel>
</asp:Panel>
