<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsMain.ascx.cs" Inherits="Saving.Applications.hr.ws_hr_worktime_imp_ctrl.DsMain" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormViewMain" runat="server" DefaultMode="Edit" Width="100%">
    <EditItemTemplate>
        <table class="DataSourceFormView">
        <tr>
            <td width="20%">
                <span>วันที่ : </span>
            </td>
            <td>
                <asp:TextBox ID="entry_date" runat="server" Width="90%" Style="text-align: center; " ReadOnly="true"></asp:TextBox>
            </td>
            
            <td>
                <asp:Button ID="b_process" runat="server" Text="Import ข้อมูล"/>
            </td>
            <td width="30%">
                
            </td>
        </tr>
    </table>
    </EditItemTemplate>
</asp:FormView>