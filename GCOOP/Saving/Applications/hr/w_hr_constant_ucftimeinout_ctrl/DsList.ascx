<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsList.ascx.cs" Inherits="Saving.Applications.hr.w_hr_constant_ucftimeinout_ctrl.DsList" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<table class="DataSourceRepeater">
    <tr>
        <th width="30px">
            <center>ประเภท</center>
        </th>
        <th width="100px">
          <center> เวลา </center> 
        </th>
        <th width="20px">
        </th>
    </tr>
    <asp:Repeater ID="Repeater1" runat="server">
        <ItemTemplate>
            <tr>
                <td>
                    <asp:TextBox ID="time_code" runat="server" style="text-align:center"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="times" runat="server" style="text-align:center"></asp:TextBox>
                </td>
                <td>
                    <asp:Button ID="b_del" runat="server" Text="ลบ" />
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
