<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsLoan.ascx.cs" Inherits="Saving.Applications.shrlon.ws_lon_prc_preparepay_ctrl.DsLoan" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:Panel ID="Panel2" runat="server" Height="400px" Width="700px" ScrollBars="Auto">
    <table class="DataSourceRepeater">    
        <tr>
            <th>
            <input type="button" value="All" onclick="PostBtall()" />
               <%-- <asp:CheckBox ID="checkall_flag" runat="server" />--%>
            </th>
            <th width="100%" colspan="2">
                ประเภทหนี้
            </th>
            <%--<th width="20%" colspan="2">
                ประเภท
            </th>
            <th width="50%">  
                รายละเอียด
            </th>--%>
        </tr>
        <asp:Repeater ID="Repeater1" runat="server">
            <ItemTemplate>
                <tr>
                    <td align="center" width="5%">
                        <asp:CheckBox ID="OPERATE_FLAG" runat="server" />
                    </td>
                    <td width="10%">
                        <asp:TextBox ID="LOANTYPE_CODE" runat="server" ReadOnly="true" Style="text-align: center;
                            background-color: #EEEEEE;"></asp:TextBox>
                    </td>
                    <td width="85%">
                        <asp:TextBox ID="LOANTYPE_DESC" runat="server" ReadOnly="true" Style="text-align: left;
                            background-color: #EEEEEE;"></asp:TextBox>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Panel>
