<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsList.ascx.cs" Inherits="Saving.Applications.hr.ws_hr_edit_leave_ctrl.DsList" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:Panel ID="Panel1" runat="server" HorizontalAlign="Left">
    <table class="DataSourceRepeater">
        <tr>
            <th width="5%">
                ลำดับ
            </th>
            <th width="15%">
                ประเภทการลา
            </th>
            <th width="30%">
                ลาตั้งแต่วันที่ - ถึงวันที่
            </th>
            <th width="10%">
                จำนวนวันลา
            </th>
            <th width="10%">
                อนุมัติวันที่
            </th>
            <th width="10%">
               
            </th>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="Panel2" runat="server" Height="500px" HorizontalAlign="Left">
    <table class="DataSourceRepeater">
        <asp:Repeater ID="Repeater1" runat="server">
            <ItemTemplate>
                <tr>
                    <td width="5%">
                        <asp:TextBox ID="running_number" runat="server" Style="text-align: center; background-color: #DDDDDD;"
                            ReadOnly="True"></asp:TextBox>
                    </td>
                    <td width="15%">
                        <asp:TextBox ID="leave_desc" runat="server" Style="text-align: center; background-color: #DDDDDD;"
                            ReadOnly="True"></asp:TextBox>
                    </td>
                    <td width="15%">
                        <asp:TextBox ID="leave_from" runat="server" Style="text-align: center; background-color: #DDDDDD;"
                            ReadOnly="True"></asp:TextBox>
                    </td>
                    <td width="15%">
                        <asp:TextBox ID="leave_to" runat="server" Style="text-align: center; background-color: #DDDDDD;"
                            ReadOnly="True"></asp:TextBox>
                    </td>
                    <td width="10%">
                        <asp:TextBox ID="totalday" runat="server" Style="text-align: center; background-color: #DDDDDD;"
                            ReadOnly="True"></asp:TextBox>
                    </td>
                    <td width="10%">
                        <asp:TextBox ID="leave_date" runat="server" Style="text-align: center; background-color: #DDDDDD;"
                            ReadOnly="True"></asp:TextBox>
                    </td>
                    <td width="10%">
                        <asp:Button ID="b_detail" runat="server" Text="แก้ไข" Style="background-color: #DDDDDD;"></asp:Button>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Panel>
