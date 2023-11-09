<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsPauseloan.ascx.cs"
    Inherits="Saving.Applications.mbshr.ws_mbshr_member_detail_ctrl.DsPauseloan" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<table class="DataSourceRepeater" style="width: 700px;">
    <tr>
        <th width="15%">
            รหัส
        </th>
        <th width="20%">
            ประเภทที่งดกู้
        </th>
        <th>
            หมายเหตุ
        </th>
    </tr>
    <asp:Repeater ID="Repeater1" runat="server">
        <ItemTemplate>
            <tr>
                <td>
                    <asp:TextBox ID="loantype_code" runat="server" ReadOnly="true"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="loantype_desc" runat="server" ReadOnly="true"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="pauseloan_cause" runat="server" ReadOnly="true"></asp:TextBox>
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
