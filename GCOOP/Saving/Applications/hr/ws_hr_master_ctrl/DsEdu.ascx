<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsEdu.ascx.cs" Inherits="Saving.Applications.hr.ws_hr_master_ctrl.DsEdu" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:Panel ID="Panel2" runat="server">
    <div align="right">
        <span class="NewRowLink" onclick="PostInsertRowEdu()" id="add_row" runat="server">เพิ่มแถว</span>
    </div>
    <asp:Panel ID="Panel1" runat="server" HorizontalAlign="Left">
    <table class="DataSourceRepeater">
                <tr>
                   <th width="5%">
                        ลำดับ
                    </th>
                    <th width="12%">
                        ระดับการศึกษา:
                    </th>
                    <th width="17%">
                        วุฒิการศึกษา:
                    </th>
                    <th width="15%">
                        ชื่อสถาบัน:
                    </th>
                    <th width="15%">
                        สาขาวิชา:
                    </th>
                    <th width="12%">
                        ปีที่จบ:
                    </th>
                    <th width="8%">
                        เกรดเฉลี่ย:
                    </th>
                    <th width="5%">
                    </th>
                </tr>
    </table>
</asp:Panel>
    <asp:Repeater ID="Repeater1" runat="server">
        <ItemTemplate>
            <table class="DataSourceRepeater">
                <tr>
                    <td width="5%">
                            <asp:TextBox ID="running_number" runat="server" Style="text-align: center;"></asp:TextBox>
                    </td>
                    <td width="12%">
                        <asp:DropDownList ID="education_code" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td width="17%">
                        <asp:TextBox ID="edu_degree" runat="server"></asp:TextBox>
                    </td>
                    <td width="15%">
                        <asp:TextBox ID="edu_inst" runat="server"></asp:TextBox>
                    </td>
                    <td width="15%">
                        <asp:TextBox ID="edu_major" runat="server"></asp:TextBox>
                    </td>
                    <td width="12%">
                        <asp:TextBox ID="edu_succyear" runat="server" Style="text-align: center;"></asp:TextBox>
                    </td>
                    <td width="8%">
                        <asp:TextBox ID="edu_gpa" runat="server" Style="text-align: right;"></asp:TextBox>
                    </td>
                    <td width="5%">
                            <asp:Button ID="b_del" runat="server" Text="ลบ" Style="background-color: #DDDDDD;" />
                    </td>
                </tr>
            </table>
        </ItemTemplate>
    </asp:Repeater>
</asp:Panel>
