<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsMain.ascx.cs" Inherits="Saving.Applications.hr.ws_hr_master_ctrl.DsMain" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table class="DataSourceFormView">
            <tr>
                <td width="15%">
                    <div>
                        <span>รหัสเจ้าหน้าที่:</span>
                    </div>
                </td>
                <td width="15%">
                    <div>
                        <asp:TextBox ID="emp_no" runat="server" Style="text-align: center;" ReadOnly="true"></asp:TextBox></div>
                </td>
                <td width="15%">
                    <div>
                        <span>เลขรับเงินเดือน:</span>
                    </div>
                </td>
                <td width="20%">
                    <div>
                        <asp:TextBox ID="salary_id" runat="server" Style="text-align: center;"></asp:TextBox></div>
                </td>
                <td width="15%">
                    <div>
                        <span>ประเภท:</span>
                    </div>
                </td>
                <td width="20%">
                    <div>
                        <asp:DropDownList ID="emptype_code" runat="server">
                        </asp:DropDownList>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span>เพศ:</span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:DropDownList ID="sex" runat="server">
                            <asp:ListItem Value=" "></asp:ListItem>
                            <asp:ListItem Value="M">ชาย</asp:ListItem>
                            <asp:ListItem Value="F">หญิง</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </td>
                <td>
                    <div>
                        <span>ตำแหน่ง:</span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:DropDownList ID="pos_code" runat="server">
                        </asp:DropDownList>
                    </div>
                </td>
                <td>
                    <div>
                        <span>กลุ่มงาน:</span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:DropDownList ID="deptgrp_code" runat="server">
                        </asp:DropDownList>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span>คำนำหน้า:</span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:DropDownList ID="prename_code" runat="server">
                        </asp:DropDownList>
                    </div>
                </td>
                <td>
                    <div>
                        <span>ชื่อ:</span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="emp_name" runat="server"></asp:TextBox></div>
                </td>
                <td>
                    <div>
                        <span>นามสกุล:</span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="emp_surname" runat="server"></asp:TextBox></div>
                </td>
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>
