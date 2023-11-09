<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsList.ascx.cs" Inherits="Saving.Applications.hr.ws_hr_constant_ucfworkintime_ctrl.DsList" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<table class="DataSourceRepeater">
    <tr>
        <th width="30px">
            รหัส
        </th>
        <th width="100px">
            เวลา
        </th>
        <th width="20px">
        </th>
    </tr>
    <asp:Repeater ID="Repeater1" runat="server">
        <ItemTemplate>
            <tr>
                <td>
                    <asp:TextBox ID="worktime_code" runat="server" Style="text-align: center"></asp:TextBox>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="start_hour" runat="server" Style="text-align: right;"></asp:TextBox>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="start_minute" runat="server" Style="text-align: right;"></asp:TextBox>
                    </div>
                </td>
                <td>
                    <asp:Button ID="b_del" runat="server" Text="ลบ" />
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
