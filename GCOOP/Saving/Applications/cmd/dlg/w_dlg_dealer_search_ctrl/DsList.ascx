<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsList.ascx.cs" Inherits="Saving.Applications.cmd.dlg.w_dlg_dealer_search_ctrl.DsList" %>
<link id="css1" runat="server" href="../../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<div align="left">
    <asp:Panel ID="Panel1" runat="server" Width="580px" HorizontalAlign="Left">
        <table class="DataSourceRepeater" style="width: 560px;">
            <tr>
                <th width="30%">
                    เลขทะเบียนผู้จำหน่าย
                </th>
                <th width="50%">
                    ชื่อผู้จำหน่าย
                </th>
                <th width="12%">
                    โทรศัพท์
                </th>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="Panel2" runat="server" Height="390px" ScrollBars="Auto" Width="580px"
        HorizontalAlign="Left">
        <table class="DataSourceRepeater" style="width: 560px;">
            <asp:Repeater ID="Repeater1" runat="server">
                <ItemTemplate>
                    <tr>
                        <td width="30%">
                            <asp:TextBox ID="DEALER_NO" runat="server" ReadOnly="true" Style="cursor: pointer"></asp:TextBox>
                        </td>
                        <td width="50%">
                            <asp:TextBox ID="DEALER_NAME" runat="server" ReadOnly="true" Style="cursor: pointer"></asp:TextBox>
                        </td>
                        <td width="12%">
                            <asp:TextBox ID="DEALER_PHONE" runat="server" ReadOnly="true" Style="cursor: pointer"></asp:TextBox>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </asp:Panel>
    <hr width="580" align="left" />
</div>
