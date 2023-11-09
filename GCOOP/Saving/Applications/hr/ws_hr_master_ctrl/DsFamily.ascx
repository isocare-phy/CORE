<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsFamily.ascx.cs" Inherits="Saving.Applications.hr.ws_hr_master_ctrl.DsFamily" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:Panel ID="Panel2" runat="server">
    <div align="right">
        <span id="Span4" class="NewRowLink" onclick="PostInsertRowFamily()" runat="server">เพิ่มแถว</span>
    </div>
    <asp:Panel ID="Panel1" runat="server" HorizontalAlign="Left">
    <table class="DataSourceRepeater">
                <tr>
                   <th width="5%">
                        ลำดับ
                    </th>
                    <th width="30%">
                        ชื่อ-นามสกุล:
                    </th>
                    <th width="20%">
                        ความสัมพันธ์:
                    </th>
                    <th width="20%">
                        วัน/เดือน/ปีเกิด:
                    </th>
                    <th width="20%">
                        ประกอบอาชีพ:
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
                    <td width="30%">
                        <asp:TextBox ID="f_name" runat="server"></asp:TextBox>
                    </td>
                    <td width="20%">
                        <asp:DropDownList ID="f_relation" runat="server">
                            <asp:ListItem Value = " "> </asp:ListItem>
                            <asp:ListItem Value = "บิดา">บิดา </asp:ListItem>
                            <asp:ListItem Value = "มารดา">มารดา </asp:ListItem>
                            <asp:ListItem Value = "คู่สมรส">คู่สมรส </asp:ListItem>
                            <asp:ListItem Value = "บุตร">บุตร</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td width="20%">
                        <asp:TextBox ID="birth_date" runat="server" Style="text-align: center;"></asp:TextBox>
                    </td>
                    <td width="20%">
                        <asp:TextBox ID="occupation" runat="server"></asp:TextBox>
                    </td>
                    <td width="5%">
                        <asp:Button ID="b_del" runat="server" Text="ลบ" Style="background-color: #DDDDDD;" />
                    </td>
                </tr>
            </table>
        </ItemTemplate>
    </asp:Repeater>
</asp:Panel>
