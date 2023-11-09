<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsList.ascx.cs" Inherits="Saving.Applications.hr.ws_hr_worktime_ctrl.DsList" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<table class="DataSourceRepeater" style="width: 800px;">
    <tr>
        <th width="10%">
            <span>รหัสเจ้าหน้าที่</span>
        </th>
        <%--<th width="4%">
        </th>--%>
        <th width="22%">
            <span>ชื่อ - สกุล</span>
        </th>
        <th width="15%" colspan="2">
            <span>เวลาเข้างาน</span>
        </th>
        <th width="15%" colspan="2">
            <span>เวลาออกงาน</span>
        </th>
        <th width="15%">
            <span>ประเภทการเข้างาน</span>
        </th>
        <th width="4%">
        </th>
    </tr>
    <asp:Repeater ID="Repeater1" runat="server">
        <ItemTemplate>
            <tr>
                <td>
                    <div>
                        <asp:TextBox ID="emp_no" runat="server"></asp:TextBox>
                    </div>
                </td>
               <%-- <td>
                    <asp:Button ID="b_search" runat="server" Text="..." />
                </td>--%>
                <td>
                    <div>
                        <asp:TextBox ID="FULLNAME" runat="server"></asp:TextBox>
                    </div>
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
                    <div>
                        <asp:TextBox ID="end_hour" runat="server" Style="text-align: right;"></asp:TextBox>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="end_minute" runat="server" Style="text-align: right;"></asp:TextBox>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:DropDownList ID="worktime_code" runat="server">
                     <%--<asp:ListItem Value="0" Text="ทำงานปกติ"></asp:ListItem>
                         <asp:ListItem Value="1" Text="ทำงานนอกเวลา"></asp:ListItem>
                         <asp:ListItem Value="2" Text="ปฏิบัตินอกสถานที่"></asp:ListItem>
                         <asp:ListItem Value="3" Text="อบรม"></asp:ListItem>
                         <asp:ListItem Value="4" Text="ปฏิบัติงานต่างจังหวัด"></asp:ListItem>--%>
                        </asp:DropDownList>
                    </div>
                </td>
                <td>
                    <asp:Button ID="b_del" runat="server" Text="ลบ" />
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
