<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsMain.ascx.cs" Inherits="Saving.Applications.hr.ws_hr_payroll_inc_exc_ctrl.DsMain" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table class="DataSourceFormView">
            <tr>
                <td>
                    <div>
                        <span>รหัสเจ้าหน้าที่:</span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="emp_no" runat="server" Style="text-align: center; background-color: #DDDDDD;"
                            ReadOnly="true"></asp:TextBox></div>
                </td>
                <td>
                    <div>
                        <span>ชื่อ:</span>
                    </div>
                </td>
                <td colspan="3">
                    <div>
                        <asp:TextBox ID="cp_name" runat="server" BackColor="#DDDDDD" ReadOnly="true"></asp:TextBox></div>
                </td>
            </tr>
            <tr>
                <td width="15%">
                    <div>
                        <span>เลขรับเงินเดือน:</span>
                    </div>
                </td>
                <td width="20%">
                    <div>
                        <asp:TextBox ID="salary_id" runat="server" Style="text-align: center;" BackColor="#DDDDDD"
                            ReadOnly="true"></asp:TextBox>
                    </div>
                </td>
                <td width="15%">
                    <div>
                        <span>ตำแหน่ง:</span>
                    </div>
                </td>
                <td width="50%">
                    <div>
                        <asp:TextBox ID="pos_desc" runat="server" BackColor="#DDDDDD" ReadOnly="true"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span>ประเภท:</span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="emptype_desc" runat="server" BackColor="#DDDDDD" ReadOnly="true"></asp:TextBox>
                    </div>
                </td>
                <td>
                    <div>
                        <span>กลุ่มงาน:</span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="deptgrp_desc" runat="server" BackColor="#DDDDDD" ReadOnly="true"></asp:TextBox>
                    </div>
                </td>
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>
