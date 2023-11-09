<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsList.ascx.cs" 
Inherits="Saving.Applications.hr.ws_hr_constant_ucflasrights_ctrl.DsList" %>
<link id="css1" runat="server" href="../../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:Panel ID="Panel1" runat="server" HorizontalAlign="Center">
    <table class="DataSourceRepeater" align="center" style="width: 700px;">
        <tr>
            <th width="5%">
            </th>
            <th width="10%">
                รหัส
            </th>
            <th width="20%">
                ประเภทการลา
            </th>
            <th width="20%">
                อายุงานตั้งแต่
            </th>
            <th width="20%">
                ถึงอายุงาน
            </th>
            <th width="20%">
                จำนวนวันที่ลาได้
            </th>
            <th width="5%">
            </th>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="Panel2" runat="server" Height="500px" ScrollBars="Auto" HorizontalAlign="Center">
    <table class="DataSourceRepeater" align="center" style="width: 700px;">
        <asp:Repeater ID="Repeater1" runat="server">
            <ItemTemplate>
                <tr>
                    <td width="5%">
                        <asp:TextBox ID="running_number" runat="server" Style="text-align: center;"></asp:TextBox>
                    </td>
                    <td width="10%">
                        <asp:TextBox ID="las_code" runat="server" Style="text-align: center;"></asp:TextBox>
                    </td>
                    <td width="20%">
                        <asp:DropDownList ID="leave_code" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td width="20%">
                        <asp:TextBox ID="las_smwork" runat="server"></asp:TextBox>
                    </td>
                    <td width="20%">
                        <asp:TextBox ID="las_emwork" runat="server"></asp:TextBox>
                    </td>
                    <td width="20%">
                        <asp:TextBox ID="las_dateleave" runat="server"></asp:TextBox>
                    </td>
                    <td width="5%">
                        <asp:Button ID="b_delete" runat="server" Text="ลบ" />
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Panel>
