<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsMain.ascx.cs" Inherits="Saving.Applications.hr.ws_hr_assist_child_ctrl.DsMain" %>
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
                        <asp:TextBox ID="emp_no" runat="server" Style="text-align: center;" ></asp:TextBox></div>
                </td>
                <td width="15%">
                    <div>
                        <span>ชื่อ-สกุล:</span>
                    </div>
                </td>
                <td width="20%">
                    <div>
                        <asp:TextBox ID="fullname" runat="server" Style="text-align: center;" ReadOnly="true"></asp:TextBox></div>
                </td>
                <td width="15%">
                    <div>
                        <span>ตำแหน่ง:</span>
                    </div>
                </td>
                <td width="20%">
                    <div>
                       <asp:TextBox ID="position_emp" runat="server" Style="text-align: center;" ReadOnly="true"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span>คู่สมรสชื่อ:</span>
                    </div>
                </td>
                <td>
                    <div>
                      <asp:TextBox ID="f_name" runat="server" Style="text-align: center;" ReadOnly="true"></asp:TextBox>
                    </div>
                </td>
                <td>
                    <div>
                        <span>อาชีพ:</span>
                    </div>
                </td>
                <td>
                    <div>
                       <asp:TextBox ID="occupation" runat="server" Style="text-align: center;"></asp:TextBox>
                    </div>
                </td>
                <%--<td>
                    <div>
                        <span>ตำแหน่ง:</span>
                    </div>
                </td>
                <td>
                    <div>
                       <asp:TextBox ID="position_f" runat="server" Style="text-align: center;"></asp:TextBox>
                    </div>
                </td>--%>
            </tr>
            <%--<tr>
                <td>
                    <div>
                        <span>สำนักงาน:</span>
                    </div>
                </td>
                <td colspan="2">
                    <div>
                       <asp:TextBox ID="empgroup" runat="server" Style="text-align: center;"></asp:TextBox>
                    </div>
                </td>
                <td>
                    <div>
                        
                </td>
                <td>
                    <div>
                        
                    </div>
                </td>
                <td>
                    <div>
                        
                </td>
            </tr>--%>
        </table>
    </EditItemTemplate>
</asp:FormView>
