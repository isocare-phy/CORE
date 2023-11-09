<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsMain.ascx.cs" Inherits="Saving.Applications.hr.ws_hr_inout_logtime_ctrl.DsMain" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table class="DataSourceFormView" style="width: 300px;">
            <tr>
                <td width="30%">
                    <div>
                        <span>รหัสพนักงาน :</span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="emp_no" runat="server"></asp:TextBox>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:Button ID="process" runat="server" Text="เข้างาน/ออกงาน"></asp:Button>
                    </div>
                </td>
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>
