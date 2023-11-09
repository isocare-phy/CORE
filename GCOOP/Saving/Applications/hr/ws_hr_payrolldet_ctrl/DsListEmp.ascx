<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsListEmp.ascx.cs" Inherits="Saving.Applications.hr.ws_hr_payrolldet_ctrl.DsListEmp" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<div align="center">
    <asp:Panel ID="Panel3" runat="server" HorizontalAlign="Left" Width="570px">
        <table class="DataSourceRepeater" style="width: 550px;">
            <tr>
                <th width="10%">
                    ลำดับ
                </th>
                <th width="15%">
                    ลำดับที่
                </th>
                <th width="15%">
                    เลขพนักงาน
                </th>
                <th>
                    ชื่อ-สกุล
                </th>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="Panel4" runat="server" Height="240px" Width="570px" ScrollBars="Auto"
        HorizontalAlign="Left">
        <table class="DataSourceRepeater" style="width: 550px;">
            <asp:Repeater ID="Repeater1" runat="server">
                <ItemTemplate>
                    <tr>
                        <td width="10%">
                            <asp:TextBox ID="running_number" runat="server" Style="text-align: center;"></asp:TextBox>
                        </td>
                        <td width="15%">
                            <asp:TextBox ID="emp_no" runat="server" Style="text-align: center;"></asp:TextBox>
                        </td>
                        <td width="15%">
                            <asp:TextBox ID="salary_id" runat="server" Style="text-align: center;"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="cp_name" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </asp:Panel>
</div>
