<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsListOther.ascx.cs"
    Inherits="Saving.Applications.hr.ws_hr_payroll_inc_exc_ctrl.DsListOther" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<div align="left">
    <div align="right" style="width: 550px;">
        <span id="Span2" class="NewRowLink" onclick="PostInsertRowOther()" runat="server">เพิ่มแถว</span>
    </div>
    <asp:Panel ID="Panel3" runat="server" HorizontalAlign="Left">
        <table class="DataSourceRepeater" style="width: 550px;">
            <tr>
                <th width="10%">
                    ลำดับ
                </th>
                <th width="15%">
                    งวด
                </th>
                <th width="15%">
                    รหัส
                </th>
                <th>
                    รายละเอียด
                </th>
                <th width="25%">
                    จำนวนเงิน
                </th>
                <th width="10%">
                </th>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="Panel4" runat="server" Height="130px" Width="570px" ScrollBars="Auto"
        HorizontalAlign="Left">
        <table class="DataSourceRepeater" style="width: 550px;">
            <asp:Repeater ID="Repeater1" runat="server">
                <ItemTemplate>
                    <tr>
                        <td width="10%">
                            <asp:TextBox ID="running_number" runat="server" Style="text-align: center;"></asp:TextBox>
                        </td>
                        <td width="15%">
                            <asp:TextBox ID="payroll_period" runat="server" Style="text-align: center;"></asp:TextBox>
                        </td>
                        <td width="15%">
                            <asp:DropDownList ID="salitem_code" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox ID="payother_desc" runat="server"></asp:TextBox>
                        </td>
                        <td width="25%">
                            <asp:TextBox ID="item_amt" runat="server" Style="text-align: right;" ToolTip="#,##0.00"></asp:TextBox>
                        </td>
                        <td width="10%">
                            <asp:Button ID="b_delete" runat="server" Text="ลบ" />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </asp:Panel>
    <table class="DataSourceFormView" style="width: 550px;">
        <tr>
            <td style="text-align: right;">
                <strong>รวม:</strong>
            </td>
            <td width="25%">
                <asp:TextBox ID="cpsum_other_amt" runat="server" Style="font-size: 11px; text-align: right;
                    font-weight: bold;" ToolTip="#,##0.00"></asp:TextBox>
            </td>
            <td width="10%">
            </td>
        </tr>
    </table>
</div>
