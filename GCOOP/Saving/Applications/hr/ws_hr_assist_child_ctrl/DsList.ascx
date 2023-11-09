<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsList.ascx.cs" Inherits="Saving.Applications.hr.ws_hr_assist_child_ctrl.DsList" %>
<link id="css1" runat="server" href="../../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:Panel ID="Panel1" runat="server" HorizontalAlign="Center">
    <table class="DataSourceRepeater" align="center" style="width: 700px;">
        <tr>
            <th width="5%">
            </th>
            <th width="15%">
                วันที่
            </th>
            <th width="15%">
                ชื่อ-สกุล
            </th>
            <th width="15%">
                ชั้นการศึกษา
            </th>
            <th width="15%">
                สถานศึกษา
            </th>
            <th width="15%">
                จำนวนเงิน
            </th>
            <th width="5%">
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
                    <td width="15%">
                        <asp:TextBox ID="assist_date" runat="server" Style="text-align: center;"></asp:TextBox>
                    </td>
                    <td width="15%">
                        <asp:TextBox ID="assist_detail" runat="server"></asp:TextBox>
                    </td>
                    <td width="15%">
                        <asp:TextBox ID="education_desc" runat="server" ></asp:TextBox>
                    </td>
                    <td width="15%">
                        <asp:TextBox ID="assist_posit" runat="server" Style="text-align: center" ></asp:TextBox>
                    </td>
                    <td width="15%">
                        <asp:TextBox ID="assist_amt" runat="server" Style="text-align: right" ToolTip="#,##0.00"></asp:TextBox>
                    </td>
                    <td width="4%">
                        <asp:Button ID="b_detail" runat="server" Text="แก้ไข" />
                    </td>
                    <td width="4%">
                        <asp:Button ID="b_delete" runat="server" Text="ลบ" />
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Panel>