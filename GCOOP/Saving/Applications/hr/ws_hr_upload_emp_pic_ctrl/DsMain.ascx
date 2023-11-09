<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsMain.ascx.cs" Inherits="Saving.Applications.hr.ws_hr_upload_emp_pic_ctrl.DsMain" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table class="DataSourceFormView" >
            <tr>
                <td width="20%">
                    <div>
                        <span>รหัสเจ้าหน้าที่ :</span>
                    </div>
                </td>
                <td width="80%">
                    <div>
                        <asp:TextBox ID="emp_no" runat="server" style="width:120px;"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span>ชื่อ-นามสกุล:</span>
                    </div>
                </td>
                <td >
                    <div>
                        <asp:TextBox ID="full_name" runat="server" style="width:150px;" ReadOnly></asp:TextBox>
                    </div>
                </td>
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>