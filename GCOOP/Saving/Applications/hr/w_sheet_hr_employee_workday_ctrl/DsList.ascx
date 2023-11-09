<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsList.ascx.cs" Inherits="Saving.Applications.hr.w_sheet_hr_employee_workday_ctrl.DsList" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />

<table class="DataSourceRepeater">
    <tr>
        <th width="5%">
            ลำดับ
        </th>
        <th width="7%">
            รหัสเจ้าหน้าที่
        </th>
        <th width="40%">
            ชื่อ - สกุล
        </th>
          <th width="8%">
           เข้างานประจำวัน
        </th>
         
    </tr>
    <tbody>
        <asp:Repeater ID="Repeater1" runat="server">
            <ItemTemplate>
                <tr>
                    <td >
                        <asp:TextBox ID="running_number" runat="server" Style="text-align: center;" ></asp:TextBox>
                    </td>
                    <td >
                        <asp:TextBox ID="emplcode" runat="server" Style="text-align: center;"></asp:TextBox>
                    </td>
                     <td >
                        <asp:TextBox ID="COMPUTE_1" runat="server"  ></asp:TextBox>
                    </td>
                    <td >
                        <asp:CheckBox ID="check_in" runat="server" Style="text-align: center;" ></asp:CheckBox>
                    </td>
                   
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </tbody>
</table>