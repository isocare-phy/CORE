<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsMain.ascx.cs" Inherits="Saving.Applications.hr.ws_hr_worktime_ctrl.DsMain" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table class="DataSourceFormView" style="width: 300px;">
            <tr>
                <td width="30%">
                    <div>
                        <span>ประจำวันที่:</span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="work_date" runat="server"></asp:TextBox>
                    </div>
                </td>
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>
