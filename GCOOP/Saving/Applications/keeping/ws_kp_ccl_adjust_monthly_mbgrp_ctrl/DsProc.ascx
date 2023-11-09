<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsProc.ascx.cs" Inherits="Saving.Applications.keeping.ws_kp_ccl_adjust_monthly_mbgrp_ctrl.DsProc" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet" type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table class="DataSourceFormView">
            <tr>
                <td width="12%">
                    <div>
                        <span>ประจำปี :</span>
                    </div>
                </td>
                <td width="14%">
                    <div>
                        <asp:TextBox ID="receive_year" runat="server" Style="text-align: center"></asp:TextBox>
                    </div>
                </td>
                <td width="12%">
                    <div>
                        <span>ประจำเดือน :</span>
                    </div>
                </td>
                <td width="20%">
                    <div>
                        <asp:DropDownList ID="receive_month" runat="server">
                            <asp:ListItem Value="0" Text=""></asp:ListItem>
                            <asp:ListItem Value="1">มกราคม</asp:ListItem>
                            <asp:ListItem Value="2">กุมภาพันธ์</asp:ListItem>
                            <asp:ListItem Value="3">มีนาคม</asp:ListItem>
                            <asp:ListItem Value="4">เมษายน</asp:ListItem>
                            <asp:ListItem Value="5">พฤษภาคม</asp:ListItem>
                            <asp:ListItem Value="6">มิถุนายน</asp:ListItem>
                            <asp:ListItem Value="7">กรกฎาคม</asp:ListItem>
                            <asp:ListItem Value="8">สิงหาคม</asp:ListItem>
                            <asp:ListItem Value="9">กันยายน</asp:ListItem>
                            <asp:ListItem Value="10">ตุลาคม</asp:ListItem>
                            <asp:ListItem Value="11">พฤศจิกายน</asp:ListItem>
                            <asp:ListItem Value="12">ธันวาคม</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span>สังกัด :</span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="membgroup_code" runat="server" Style="text-align: center"></asp:TextBox>
                    </div>
                </td>
                <td colspan="2">
                    <div>
                        <asp:DropDownList ID="membgroup_desc" runat="server"></asp:DropDownList>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span>สาเหตุการยกเลิก :</span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="slipretcause_code" runat="server" Style="text-align: center"></asp:TextBox>
                    </div>
                </td>
                <td colspan="2">
                    <div>
                        <asp:DropDownList ID="slipretcause_desc" runat="server"></asp:DropDownList>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="4" align="center">
                </td>
            </tr>
            <tr>
                <td colspan="4" align="center">
                    <asp:Button ID="b_process" runat="server" Text="ประมวลผล" Height="35px" Width="350px" />
                </td>
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>