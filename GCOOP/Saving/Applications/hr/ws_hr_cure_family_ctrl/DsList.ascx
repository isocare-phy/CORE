<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsList.ascx.cs" 
Inherits="Saving.Applications.hr.ws_hr_cure_family_ctrl.DsList" %>

<link id="css1" runat="server" href="../../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:Panel ID="Panel1" runat="server" HorizontalAlign="Center">
    <table class="DataSourceRepeater" align="center" style="width: 700px;">
        <tr>
            <th width="5%">
            </th>
            <th width="20%">
                ชื่อผู้ป่วย
            </th>
            <th width="35%">
                อาการ
            </th>
            <th width="20%">
                วันที่รับการรักษา
            </th>
            <th width="15%">
                จำนวนเงิน
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
                    <td width="22%">
                        <asp:TextBox ID="assist_name" runat="server" Style="text-align: center;"></asp:TextBox>
                    </td>
                    <td width="37%">
                        <asp:TextBox ID="assist_detail" runat="server"></asp:TextBox>
                    </td>
                    <td width="22%">
                        <asp:TextBox ID="assist_sdate" runat="server" ></asp:TextBox>
                    </td>
                    <td width="17%">
                        <asp:TextBox ID="assist_minamt" runat="server" Style="text-align: right" ToolTip="#,##0.00"></asp:TextBox>
                    </td>
                    <td width="7%">
                        <asp:Button ID="b_detail" runat="server" Text="แก้ไข" />
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Panel>
