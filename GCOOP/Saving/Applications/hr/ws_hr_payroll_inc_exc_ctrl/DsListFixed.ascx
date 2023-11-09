<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsListFixed.ascx.cs"
    Inherits="Saving.Applications.hr.ws_hr_payroll_inc_exc_ctrl.DsListFixed" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<div align="left">
    <div align="right" style="width: 550px;">
        <span id="Span1" class="NewRowLink" onclick="PostInsertRowFixed()" runat="server">เพิ่มแถว</span>
    </div>
    <asp:Panel ID="Panel1" runat="server" HorizontalAlign="Left">
        <table class="DataSourceRepeater" style="width: 550px;">
            <tr>
                <th width="10%">
                    ลำดับ
                </th>
                <th>
                    รายการ
                </th>
                <th width="25%">
                    จำนวนเงิน
                </th>
                <th width="10%">
                </th>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="Panel2" runat="server" Height="130px" Width="570px" ScrollBars="Auto"
        HorizontalAlign="Left">
        <table class="DataSourceRepeater" style="width: 550px;">
            <asp:Repeater ID="Repeater1" runat="server">
                <ItemTemplate>
                    <tr>
                        <td width="10%">
                            <asp:TextBox ID="running_number" runat="server" Style="text-align: center;"></asp:TextBox>
                        </td>
                        <td>
                            <asp:DropDownList ID="salitem_code" runat="server">
                            </asp:DropDownList>
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
</div>
