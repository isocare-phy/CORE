<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsTraining.ascx.cs"
    Inherits="Saving.Applications.hr.ws_hr_master_ctrl.DsTraining" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:Panel ID="Panel2" runat="server">
    <div align="right">
        <span id="Span2" class="NewRowLink" onclick="PostInsertRowTraining()" runat="server">
            เพิ่มแถว</span>
    </div>
    <asp:Panel ID="Panel1" runat="server" HorizontalAlign="Left">
    <table class="DataSourceRepeater">
                <tr>
                   <th width="5%">
                        ลำดับ
                    </th>
                    <th width="30%">
                        ชื่อโครงการ
                    </th>
                    <th width="24%">
                        สถานที่
                    </th>
                    <th width="10%">
                        วันที่เริ่ม
                    </th>
                    <th width="10%">
                        วันสุดท้าย
                    </th>
                    <th width="10%">
                        ค่าใช้จ่าย
                    </th>
                    <th width="6%">
                    </th>
                </tr>
    </table>
</asp:Panel>
    <asp:Repeater ID="Repeater1" runat="server">
        <ItemTemplate>
            <table class="DataSourceRepeater">
                    <td width="5%">
                        <asp:TextBox ID="running_number" runat="server" Style="text-align: center;" ></asp:TextBox>
                    </td>
                    <td width="30%">
                        <asp:TextBox ID="tr_subject" runat="server"></asp:TextBox>
                    </td>
                    <td width="24%">
                        <asp:TextBox ID="tr_location" runat="server"></asp:TextBox>
                    </td>
                    <td width="10%">
                        <asp:TextBox ID="tr_fromdate" runat="server" Style="text-align: center;"></asp:TextBox>
                    </td>
                    <td width="10%">
                        <asp:TextBox ID="tr_todate" runat="server" Style="text-align: center;"></asp:TextBox>
                    </td>
                    <td width="10%">
                        <asp:TextBox ID="tr_expamt" runat="server" Style="text-align: right;" ToolTip="#,##0.00"></asp:TextBox>
                    </td>
                    <td width="6%">
                        <asp:Button ID="b_del" runat="server" Text="ลบ" Style="background-color: #DDDDDD; " />
                    </td>
            </table>
        </ItemTemplate>
    </asp:Repeater>
</asp:Panel>
