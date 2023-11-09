<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsMain.ascx.cs" Inherits="Saving.Applications.keeping.ws_kp_ccl_adjust_monthly_mbgrp_ctrl.DsMain" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet" type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table class="DataSourceFormView">
            <tr>
                <td width="12%">
                    <div>
                        <span>เลขที่ปรับปรุง :</span>
                    </div>
                </td>
                <td width="12%">
                    <div>
                        <asp:TextBox ID="adjslip_no" runat="server" Style="text-align: center"></asp:TextBox>
                    </div>
                </td>
                <td width="12%">
                    <div>
                        <span>วันที่ปรับปรุง :</span>
                    </div>
                </td>
                <td width="12%">
                    <div>
                        <asp:TextBox ID="adjslip_date" runat="server" ReadOnly="true"></asp:TextBox>
                    </div>
                </td>
                <td width="12%">
                    <div>
                        <span>วันที่ใบเสร็จ :</span>
                    </div>
                </td>
                <td width="12%">
                    <div>
                        <asp:TextBox ID="ref_slipdate" runat="server" ReadOnly="true"></asp:TextBox>
                    </div>
                </td>
                <td width="12%">
                    <div>
                        <span>ประเภท :</span>
                    </div>
                </td>
                <td width="12%">
                    <div>
                        <asp:TextBox ID="adjtype_code" runat="server" ReadOnly="true"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td width="12%">
                    <div>
                        <span>ทะเบียน :</span>
                    </div>
                </td>
                <td width="14%">
                    <div>
                        <asp:TextBox ID="member_no" runat="server" Style="text-align: center"></asp:TextBox>
                    </div>
                </td>
                <td width="12%">
                    <div>
                        <span>ชื่อ-ชื่อสกุล :</span>
                    </div>
                </td>
                <td colspan="3">
                    <div>
                        <asp:TextBox ID="member_name" runat="server" ReadOnly="true"></asp:TextBox>
                    </div>
                </td>
                <td width="12%">
                    <div>
                        <span>อ้างอิง :</span>
                    </div>
                </td>
                <td width="17%">
                    <div>
                        <asp:TextBox ID="ref_slipno" runat="server" ReadOnly="true"></asp:TextBox>
                    </div>
                </td>
            </tr>
             <tr>
                <td>
                    <div>
                        <span>เหตุผลที่ปรับปรุง :</span>
                    </div>
                </td>
                <td colspan="2">
                    <div>
                        <asp:TextBox ID="slipretcause_code" runat="server" Style="text-align: center"></asp:TextBox>
                    </div>
                </td>
                <td colspan="3">
                    <div>
                        <asp:TextBox ID="adjust_cause" runat="server" ReadOnly="true"></asp:TextBox>
                    </div>
                </td>
                <td width="12%">
                    <div>
                        <span>ยอดปรับปรุง :</span>
                    </div>
                </td>
                <td width="17%">
                    <div>
                        <asp:TextBox ID="slip_amt" runat="server" ReadOnly="true"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td width="12%">
                    <div>
                        <span>งวดจัดเก็บ :</span>
                    </div>
                </td>
                <td width="14%">
                    <div>
                        <asp:TextBox ID="ref_recvperiod" runat="server" Style="text-align: center"></asp:TextBox>
                    </div>
                </td>
                <td width="12%">
                    <div>
                        <span>สังกัด :</span>
                    </div>
                </td>
                <td colspan="3">
                    <div>
                        <asp:TextBox ID="membgroup_code" runat="server" ReadOnly="true"></asp:TextBox>
                    </div>
                </td>
                <td width="12%">
                    <div>
                        <span>บช :</span>
                    </div>
                </td>
                <td width="17%">
                    <div>
                        <asp:TextBox ID="tofrom_accid" runat="server" ReadOnly="true"></asp:TextBox>
                    </div>
                </td>
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>