<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsTran.ascx.cs" Inherits="Saving.Applications.hr.w_sheet_hr_salamas_ctrl.DsTran" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table class="DataSourceFormView" style="width: 400px;">
     
            <tr>
                <td width="100">
                    <div>
                        <span>บัญชีเงินฝาก:</span>
                    </div>
                </td>
                <td width="150">
                    <div>
                        <asp:TextBox ID="tran_acc" runat="server" style="text-align:center;" Readonly ="true"></asp:TextBox>
                    </div>
                </td>
                <td width="100">
                    <div>
                        <span>วันที่จ่าย:</span>
                    </div>
                </td>
                <td width="150">
                    <div>
                        <asp:TextBox ID="tran_date" runat="server" style="text-align:center;" Readonly ="true"></asp:TextBox>
                    </div>
                </td>
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>