<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsDetail.ascx.cs" Inherits="Saving.Applications.hr.ws_hr_leave_ctrl.DsDetail" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:Panel ID="Panel1" runat="server" HorizontalAlign="Left">
    <table class="DataSourceRepeater">
        <tr>
            <th width="5%">
                ลำดับ
            </th>
            <th width="15%">
                ประเภทการลา
            </th>
            <th width="10%">
                ลาตั้งแต่วันที่ 
            </th>
            <th width="10%">
                ถึงวันที่
            </th>
            <th width="10%">
                จำนวนวันลา
            </th>
            <th width="10%">
                อนุมัติวันที่
            </th>
            <%--<th width="30%">
                ผู้อนุมัติ
            </th>--%>
            <th width="5%">
                
            </th>
            <th width="5%">
                
            </th>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="Panel2" runat="server" Height="300px" HorizontalAlign="Left" ScrollBars="Auto">
    <table class="DataSourceRepeater">
        <asp:Repeater ID="Repeater1" runat="server">
            <ItemTemplate>
                <tr>
                    <td width="5%">
                        <asp:TextBox ID="running_number" runat="server" Style="text-align: center; background-color: #DDDDDD;"
                            ReadOnly="True"></asp:TextBox>
                    </td>
                    <td width="15%">
                        <asp:TextBox ID="leave_desc" runat="server" Style="text-align: center; background-color: #DDDDDD;"
                            ReadOnly="True"></asp:TextBox>
                    </td>
                    <td width="10%">
                        <asp:TextBox ID="leave_from" runat="server" Style="text-align: center; background-color: #DDDDDD;"
                            ReadOnly="True"></asp:TextBox>
                    </td>
                    <td width="10%">
                        <asp:TextBox ID="leave_to" runat="server" Style="text-align: center; background-color: #DDDDDD;"
                            ReadOnly="True"></asp:TextBox>
                    </td>
                    <td width="10%">
                        <asp:TextBox ID="totalday" runat="server" Style="text-align: center; background-color: #DDDDDD;"
                            ReadOnly="True"></asp:TextBox>
                    </td>
                    <td width="10%">
                        <asp:TextBox ID="apv_date" runat="server" Style="text-align: center; background-color: #DDDDDD;"
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
