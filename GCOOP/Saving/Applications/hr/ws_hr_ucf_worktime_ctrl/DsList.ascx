<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsList.ascx.cs" Inherits="Saving.Applications.hr.ws_hr_ucf_worktime_ctrl.DsList" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<table class="DataSourceRepeater">
    <tr>
    <th width="4%"></th>
        <th width="8%">
            รหัส
        </th>
        <th width="15%">
            เวลาเข้างาน
        </th>     
        <th width="15%">
            เวลาออกงาน
        </th>
        <th width="10%">
            สายสูงสุด
        </th>    
        <th width="20%">
            รายละเอียด
        </th>       
        <th width="4%">
        </th>
    </tr>
    <asp:Repeater ID="Repeater1" runat="server">
        <ItemTemplate>
            <tr>
                <td>
                    <asp:TextBox ID="running_number" runat="server" style="text-align:center"></asp:TextBox>
                </td>   
                <td>
                    <asp:TextBox ID="worktime_code" runat="server" style="text-align:center"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="work_in" runat="server" style="text-align:center" MaxLength="5"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="work_out" runat="server" style="text-align:center" MaxLength="5"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="max_late" runat="server" style="text-align:center"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="worktime_desc" runat="server" style="text-align:left"></asp:TextBox>
                </td>      
                <td>
                    <asp:Button ID="b_del" runat="server" Text="ลบ" />
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>