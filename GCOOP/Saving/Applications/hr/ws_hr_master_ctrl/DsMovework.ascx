<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsMovework.ascx.cs"
    Inherits="Saving.Applications.hr.ws_hr_master_ctrl.DsMovework" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:Panel ID="Panel1" runat="server" HorizontalAlign="Left">
    <table class="DataSourceRepeater">
        <tr>
            <th width="5%">
                ลำดับ
            </th>
            <th width="10%">
                เลขที่คำสั่ง
            </th>
            <th width="15%">
                วันที่คำสั่ง
            </th>
            <th width="25%">
                ตำแหน่งงานเดิม
            </th>
            <th width="10%">
                เงินเดือนเดิม
            </th>
            <th width="25%">
                ตำแหน่งงานใหม่
            </th>
            <th width="10%">
                เงินเดือนใหม่
            </th>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="Panel2" runat="server" Height="500px" HorizontalAlign="Left" ScrollBars="Auto" >
    <table class="DataSourceRepeater">
        <asp:Repeater ID="Repeater1" runat="server">
            <ItemTemplate>
                <tr>
                    <td width="5%">
                        <asp:TextBox ID="running_number" runat="server" Style="text-align: center; background-color: #DDDDDD;"
                            ReadOnly="True"></asp:TextBox>
                    </td>
                    <td width="10%">
                        <asp:TextBox ID="order_docno" runat="server" Style="text-align: center; background-color: #DDDDDD;"
                            ReadOnly="True"></asp:TextBox>
                    </td>
                    <td width="15%">
                        <asp:TextBox ID="order_date" runat="server" Style="text-align: center; background-color: #DDDDDD;"
                            ReadOnly="True"></asp:TextBox>
                    </td>
                    <td width="25%">
                        <asp:TextBox ID="pos_desc" runat="server" Style="text-align: center; background-color: #DDDDDD;"
                            ReadOnly="True"></asp:TextBox>
                    </td>
                    <td width="10%">
                        <asp:TextBox ID="old_salary_amt" runat="server" Style="text-align: center; background-color: #DDDDDD;"
                            ReadOnly="True" ToolTip="#,##0.00"></asp:TextBox>
                    </td>
                    <td width="25%">
                        <asp:TextBox ID="deptgrp_desc" runat="server" Style="text-align: center; background-color: #DDDDDD;"
                            ReadOnly="True"></asp:TextBox>
                    </td>
                    <td width="10%">
                        <asp:TextBox ID="salary_amt" runat="server" Style="text-align: center; background-color: #DDDDDD;"
                            ReadOnly="True" ToolTip="#,##0.00"></asp:TextBox>
                    </td>
                    <%--<td width="25%">
                        <asp:TextBox ID="old_salary_amt" Style="text-align: center; background-color: #DDDDDD;"
                            runat="server" ToolTip="#,##0.00"></asp:TextBox>
                    </td>--%>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Panel>
