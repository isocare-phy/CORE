<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsList.ascx.cs" Inherits="Saving.Applications.hr.dlg.ws_dlg_hr_master_search_ctrl.DsList" %>
<link id="css1" runat="server" href="../../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<div align="left">
    <asp:Panel ID="Panel1" runat="server" Width="680px" HorizontalAlign="Left">
        <table class="DataSourceRepeater" style="width: 660px;">
            <tr>
                <th width="10%">
                    รหัสเจ้าหน้าที่
                </th>
                <th width="14%">
                    เลขรับเงินเดือน
                </th>
                <th width="25%">
                    ชื่อ-สกุล
                </th>
                <th width="15%">
                    ตำแหน่ง
                </th>
                <th width="18%">
                    งาน/แผนก
                </th>
                <th width="18%">
                    ฝ่ายสังกัด
                </th>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="Panel2" runat="server" Height="300px" ScrollBars="Auto" Width="680px"
        HorizontalAlign="Left">
        <table class="DataSourceRepeater" style="width: 660px;">
            <asp:Repeater ID="Repeater1" runat="server">
                <ItemTemplate>
                    <tr>
                        <td width="10%">
                            <asp:TextBox ID="emp_no" runat="server" ReadOnly="true" Style="cursor: pointer"></asp:TextBox>
                        </td>
                        <td width="14%">
                            <asp:TextBox ID="salary_id" runat="server" ReadOnly="true" Style="cursor: pointer"></asp:TextBox>
                        </td>
                        <td width="25%">
                            <asp:TextBox ID="emp_fullname" runat="server" ReadOnly="true" Style="cursor: pointer"></asp:TextBox>
                        </td>
                        <td width="15%">
                            <asp:TextBox ID="pos_desc" runat="server" ReadOnly="true" Style="cursor: pointer"></asp:TextBox>
                        </td>
                        <td width="18%">
                            <asp:TextBox ID="deptgrp_desc" runat="server" ReadOnly="true" Style="cursor: pointer"></asp:TextBox>
                        </td>
                        <td width="18%">
                            <asp:TextBox ID="deptline_desc" runat="server" ReadOnly="true" Style="cursor: pointer"></asp:TextBox>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </asp:Panel>
    <hr width="650" align="left" />
</div>