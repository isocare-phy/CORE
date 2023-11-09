<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsListExc.ascx.cs" Inherits="Saving.Applications.hr.ws_hr_payrolldet_ctrl.DsListExc" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<div align="left">
    <asp:Panel ID="Panel3" runat="server" HorizontalAlign="Left" Width="360px">
        <table class="DataSourceRepeater" style="width: 340px;">
            <tr>
                <th width="15%">
                    ลำดับ
                </th>
                <th>
                    รายการหัก
                </th>
                <th width="30%">
                    จำนวน
                </th>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="Panel4" runat="server" Height="200px" Width="360px" ScrollBars="Auto"
        HorizontalAlign="Left">
        <table class="DataSourceRepeater" style="width: 340px;">
            <asp:Repeater ID="Repeater1" runat="server">
                <ItemTemplate>
                    <tr>
                        <td width="15%">
                            <asp:TextBox ID="running_number" runat="server" Style="text-align: center;"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="salitem_desc" runat="server"></asp:TextBox>
                        </td>
                        <td width="30%">
                            <asp:TextBox ID="item_amt" runat="server" Style="text-align: right;" ToolTip="#,##0.00"></asp:TextBox>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </asp:Panel>
    <table class="DataSourceFormView" style="width: 340px;">
        <tr>
            <td>
            </td>
            <td style="text-align: right;">
                <strong>รวมรายการหัก:</strong>
            </td>
            <td width="30%">
                <asp:TextBox ID="cpsum_Exc_amt" runat="server" Style="font-size: 12px; text-align: right;
                    font-weight: bold;" ToolTip="#,##0.00"></asp:TextBox>
            </td>
        </tr>
    </table>
</div>