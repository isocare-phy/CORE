<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsExperience.ascx.cs"
    Inherits="Saving.Applications.hr.ws_hr_master_ctrl.DsExperience" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:Panel ID="Panel3" runat="server">
    <div align="right">
        <span id="Span4" class="NewRowLink" onclick="PostInsertRowExperience()" runat="server">เพิ่มแถว</span>
    </div>
    <asp:Panel ID="Panel4" runat="server" HorizontalAlign="Left">
    <table class="DataSourceRepeater">
        <tr>
            <th width="5%">
                ลำดับ
            </th>
            <th width="25%">
                ชื่อบริษัท
            </th>
            <th width="25%">
                ตำแหน่ง
            </th>
            <th width="20%">
                วันที่เริ่มงาน
            </th>
            <th width="20%">
                หมายเหตุ
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
                        <asp:TextBox ID="running_number" runat="server" Style="text-align: center; "
                            ReadOnly="True"></asp:TextBox>
                    </td>
                    <td width="25%">
                        <asp:TextBox ID="corp_name" runat="server" ></asp:TextBox>
                    </td>
                    <td width="25%">
                        <asp:TextBox ID="pos_name" runat="server" ></asp:TextBox>
                    </td>
                    <td width="20%">
                        <asp:TextBox ID="yearstart" runat="server" ></asp:TextBox>
                    </td>
                    <td width="20%">
                        <asp:TextBox ID="remark" runat="server" ></asp:TextBox>
                    </td>
                    <td width="5%">
                        <asp:Button ID="b_del" runat="server" Text="ลบ" Style="background-color: #DDDDDD;" />
                    </td>
                </tr>
            </table>
        </ItemTemplate>
    </asp:Repeater>
</asp:Panel>