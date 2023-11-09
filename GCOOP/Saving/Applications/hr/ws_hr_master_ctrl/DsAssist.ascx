<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsAssist.ascx.cs" Inherits="Saving.Applications.hr.ws_hr_master_ctrl.DsAssist" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:Panel ID="Panel2" runat="server">
    <div align="right">
        <span id="Span3" class="NewRowLink" onclick="PostInsertRowAssist()" runat="server">
        เพิ่มแถว</span>
    </div>
    <asp:Panel ID="Panel1" runat="server" HorizontalAlign="Left">
    <table class="DataSourceRepeater">
                <tr>
                   <th width="5%">
                        ลำดับ
                    </th>
                    <th width="15%">
                        ประเภทสวัสดิการ
                    </th>
                    <th width="15%">
                        วันที่จ่าย
                    </th>
                    <th width="10%">
                        จำนวนเงิน
                    </th>
                    <th width="30%">
                        รายละเอียด
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
                        <asp:TextBox ID="running_number" runat="server" Style="text-align: center;" ></asp:TextBox>
                    </td>
                    <td width="15%">
                        <asp:DropDownList ID="assist_code" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td width="15%">
                        <asp:TextBox ID="assist_date" runat="server" Style="text-align: center;"></asp:TextBox>
                    </td>
                    <td width="10%">
                        <asp:TextBox ID="assist_amt" runat="server" Style="text-align: right;" ToolTip="#,##0.00"></asp:TextBox>
                    </td>
                    <td width="30%">
                        <asp:TextBox ID="assist_detail" runat="server"></asp:TextBox>
                    </td>
                    <td width="20%">
                        <asp:TextBox ID="assist_remark" runat="server" ></asp:TextBox>
                    </td>
                    <td width="5%">
                        <asp:Button ID="b_del" runat="server" Text="ลบ" Style="background-color: #DDDDDD;" />
                    </td>
                </tr>
            </table>
        </ItemTemplate>
    </asp:Repeater>
</asp:Panel>
