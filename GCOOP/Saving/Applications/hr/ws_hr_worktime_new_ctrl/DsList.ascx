<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsList.ascx.cs" Inherits="Saving.Applications.hr.ws_hr_worktime_new_ctrl.DsList" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
    <asp:Panel ID="Panel2" runat="server" Height="500px"  ScrollBars="Auto" HorizontalAlign="Center">
    <table class="DataSourceRepeater" align="center" style="width: 100%;">
         <tr>
        <th width="10%">
            <span>วันที่</span>
        </th>
        <th width="10%">
            <span>รหัสเจ้าหน้าที่</span>
        </th>
        <th width="22%">
            <span>ชื่อ - สกุล</span>
        </th>
        <th width="15%">
            <span>เวลาเข้างาน</span>
        </th>
        <th width="15%">
            <span>เวลาออกงาน</span>
        </th>
        <th width="15%">
            <span>ประเภทการเข้างาน</span>
        </th>
    </tr>
    </table>
    <table class="DataSourceRepeater" align="center" style="width: 100%;">
        <asp:Repeater ID="Repeater1" runat="server">
            <ItemTemplate>
                   <tr>
                   <td width="10%">
                        <div>
                            <asp:TextBox ID="work_date" runat="server" Style="text-align: center;" ReadOnly="true"></asp:TextBox>
                        </div>
                    </td>
                    <td width="10%">
                        <div>
                            <asp:TextBox ID="emp_no" runat="server" Style="text-align: center;" ReadOnly="true"></asp:TextBox>
                        </div>
                    </td>
                    <td width="22%">
                        <div>
                            <asp:TextBox ID="FULLNAME" runat="server" ReadOnly="true"></asp:TextBox>
                        </div>
                    </td>
                    <td width="15%">
                        <div>
                            <asp:TextBox ID="work_in" runat="server" Style="text-align: center;" MaxLength="5"></asp:TextBox>
                        </div>
                    </td>
                    <td width="15%">
                        <div>
                            <asp:TextBox ID="work_out" runat="server" Style="text-align: center;" MaxLength="5"></asp:TextBox>
                        </div>
                    </td>
                    <td width="15%">
                        <div>
                            <asp:TextBox ID="worktime_code" runat="server" Style="text-align: center; background-color: #DDDDDD;"
                                ReadOnly="true"></asp:TextBox>
                        </div>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Panel>