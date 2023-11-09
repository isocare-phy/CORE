<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsGuarantee.ascx.cs" 
Inherits="Saving.Applications.hr.ws_hr_master_ctrl.DsGuarantee" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:Panel ID="Panel2" runat="server">
    <div align="right">
        <span id="Span4" class="NewRowLink" onclick="PostInsertRowGuarantee()" runat="server">เพิ่มแถว</span>
    </div>
    <asp:Panel ID="Panel1" runat="server" HorizontalAlign="Left">
    <table class="DataSourceRepeater">
        <tr>
            <th width="5%">
                ลำดับ
            </th>
            <th width="25%">
                ชื่อ - นามสกุล
            </th>
            <th width="15%">
                วัน/เดือน/ปีเกิด
            </th>
            <th width="5%">
                อายุ
            </th>
            <th width="30%">
                สถานที่ทำงาน
            </th>
            <th width="15%">
                เบอร์โทร
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
                        <asp:TextBox ID="running_number" runat="server" Style="text-align: center; background-color: #DDDDDD;"
                            ReadOnly="True"></asp:TextBox>
                    </td>
                    <td width="25%">
                        <asp:TextBox ID="garan_name" runat="server" Style="text-align: center; "></asp:TextBox>
                    </td>
                    <td width="15%">
                        <asp:TextBox ID="garan_birth" runat="server" Style="text-align: center; "></asp:TextBox>
                    </td>
                    <td width="5%">
                        <asp:TextBox ID="garan_age" runat="server" Style="text-align: center; " 
                        ReadOnly="true"></asp:TextBox>
                    </td>
                    <td width="30%">
                        <asp:TextBox ID="garan_work" runat="server" Style="text-align: center;"></asp:TextBox>
                    </td>
                    <td width="15%">
                        <asp:TextBox ID="garan_tel" runat="server" Style="text-align: center; "></asp:TextBox>
                    </td>
                    <td width="5%">
                        <asp:Button ID="b_del" runat="server" Text="ลบ" Style="background-color: #DDDDDD; " />
                    </td>
                </tr>
            </table>
        </ItemTemplate>
    </asp:Repeater>
</asp:Panel>

