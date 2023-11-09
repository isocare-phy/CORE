<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsMain.ascx.cs" Inherits="Saving.Applications.hr.ws_hr_cure_family_ctrl.DsMain" %>
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
                        <span>เงินเดือน(ข้าพเจ้า):</span>
                    </div>
                </td>
                <td>
                    <div>
                      <asp:TextBox ID="salary_amt" runat="server" Style="text-align: right" ToolTip="#,##0.00"></asp:TextBox>
                    </div>
                </td>
                <td>
                    <div>
                        <span>เงินเดือน(คู่สมรส):</span>
                    </div>
                </td>
                <td>
                    <div>
                       <asp:TextBox ID="f_salary" runat="server" Style="text-align: right" ToolTip="#,##0.00"></asp:TextBox>
                    </div>
                </td>
                <td>
                    <div>
                       
                    </div>
                </td>
                <td>
                    <div>
                      
                    </div>
                </td>
            </tr>
           
        </table>
    </EditItemTemplate>
</asp:FormView>
