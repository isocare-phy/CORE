<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsDetail.ascx.cs" Inherits="Saving.Applications.hr.ws_hr_cure_newfamily_ctrl.DsDetail" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:Panel ID="Panel1" runat="server" HorizontalAlign="Left">
    <table class="DataSourceRepeater">
        <tr>
            <th width="5%">
                ลำดับ
            </th>
            <th width="15%">
                ชื่อผู้ป่วย
            </th>
            <th width="10%">
                วันที่ตรวจรักษา
            </th>
            <th width="10%">
                จำนวนเงินที่จ่ายจริง
            </th>
            <th width="10%">
                จำนวนเงินที่เบิกได้
            </th>
            <th width="5%">
                
            </th>
            <th width="5%">
                
            </th>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="Panel2" runat="server" Height="500px" HorizontalAlign="Left">
    <table class="DataSourceRepeater">
        <asp:Repeater ID="Repeater1" runat="server">
            <ItemTemplate>
                <tr>
                    <td width="5%">
                        <asp:TextBox ID="running_number" runat="server" Style="text-align: center; background-color: #DDDDDD;"
                            ReadOnly="True"></asp:TextBox>
                    </td>
                    <td width="15%">
                        <asp:TextBox ID="assist_name" runat="server" Style="text-align: center; background-color: #DDDDDD;"
                            ReadOnly="True"></asp:TextBox>
                    </td>
                    <td width="10%">
                        <asp:TextBox ID="assist_sdate" runat="server" Style="text-align: center; background-color: #DDDDDD;"
                            ReadOnly="True"></asp:TextBox>
                    </td>
                    <td width="10%">
                        <asp:TextBox ID="assist_amt" runat="server" Style="text-align: right; background-color: #DDDDDD;" ToolTip="#,##0.00"
                            ReadOnly="True"></asp:TextBox>
                    </td>
                    <td width="10%">
                        <asp:TextBox ID="assist_minamt" runat="server" Style="text-align: right; background-color: #DDDDDD;" ToolTip="#,##0.00"
                            ReadOnly="True"></asp:TextBox>
                    </td>
                    <td width="5%">
                        <asp:Button ID="b_detail" runat="server" Text="แก้ไข" Style="background-color: #DDDDDD;"></asp:Button>
                    </td>
                    <td width="5%">
                        <asp:Button ID="b_delete" runat="server" Text="ลบ" Style="background-color: #DDDDDD;"></asp:Button>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Panel>
