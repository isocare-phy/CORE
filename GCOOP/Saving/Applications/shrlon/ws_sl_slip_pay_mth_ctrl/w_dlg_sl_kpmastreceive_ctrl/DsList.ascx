<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsList.ascx.cs" Inherits="Saving.Applications.shrlon.ws_sl_slip_pay_mth_ctrl.w_dlg_sl_kpmastreceive_ctrl.DsList" %>
<link id="css1" runat="server" href="../../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<div align="left">
    <asp:Panel ID="Panel1" runat="server" Width="640px" HorizontalAlign="Left">
        <table class="DataSourceRepeater" style="width: 620px;">
            <tr>
                <th width="15%">
                    งวดส่ง
                </th>
                <th width="25%">
                    เลขที่ใบเสร็จ
                </th>
                <th width="20%">
                    เลขที่สมาชิก
                </th>
                <th width="20%">
                    วันที่ใบเสร็จ
                </th>
                <th width="20%">
                    ยอดชำระ
                </th>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="Panel2" runat="server" Height="390px" ScrollBars="Auto" Width="640px"
        HorizontalAlign="Left">
        <table class="DataSourceRepeater" style="width: 620px;">
            <asp:Repeater ID="Repeater1" runat="server">
                <ItemTemplate>
                    <tr>
                        <td width="15%">
                            <asp:TextBox ID="RECV_PERIOD" runat="server" ReadOnly="true" Style="cursor: pointer;
                                text-align: center;"></asp:TextBox>
                        </td>
                        <td width="25%">
                            <asp:TextBox ID="RECEIPT_NO" runat="server" ReadOnly="true" Style="cursor: pointer"></asp:TextBox>
                        </td>
                        <td width="20%">
                            <asp:TextBox ID="MEMBER_NO" runat="server" ReadOnly="true" Style="cursor: pointer;
                                text-align: center;"></asp:TextBox>
                        </td>
                        <td width="20%">
                            <asp:TextBox ID="OPERATE_DATE" runat="server" ReadOnly="true" Style="cursor: pointer; text-align: center;"></asp:TextBox>
                        </td>
                        <td width="20%">
                            <asp:TextBox ID="RECEIVE_AMT" runat="server" ReadOnly="true" Style="cursor: pointer;
                                text-align: right;" ToolTip="#,##0.00"></asp:TextBox>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </asp:Panel>
    <hr width="580" align="left" />
</div>
