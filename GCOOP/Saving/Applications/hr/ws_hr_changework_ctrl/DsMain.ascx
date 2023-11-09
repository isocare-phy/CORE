<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsMain.ascx.cs" Inherits="Saving.Applications.hr.ws_hr_changework_ctrl.DsMain" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table class="DataSourceFormView">
            <tr>
                <td width="20%">
                    <div>
                        <span>รหัสพนักงาน :</span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="Emp_No" runat="server"></asp:TextBox>
                    </div>
                </td>
                <td width="20%">
                    <div>
                        <span>เลขที่รับเงินเดือน :</span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="Salary_Id" runat="server" Style="text-align: center; background-color: #DDDDDD;"
                            ReadOnly="True"></asp:TextBox>
                    </div>
                </td>
                <td width="15%">
                    <div>
                        <span>กลุ่มงาน :</span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="Deptgrp_Desc" runat="server" Style="text-align: center; background-color: #DDDDDD;"
                            ReadOnly="True"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span>ชื่อ - สกุล :</span>
                    </div>
                </td>
                <td colspan="3">
                    <div>
                        <asp:TextBox ID="Fullname" runat="server" Style="text-align: center; background-color: #DDDDDD;"
                            ReadOnly="True"></asp:TextBox>
                    </div>
                </td>
                <td>
                    <div>
                        <span>ตำแหน่ง :</span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="Pos_Desc" runat="server" Style="text-align: center; background-color: #DDDDDD;"
                            ReadOnly="True"></asp:TextBox>
                    </div>
                </td>
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>
