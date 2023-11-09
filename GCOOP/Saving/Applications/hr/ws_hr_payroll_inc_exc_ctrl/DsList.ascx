<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsList.ascx.cs" 
Inherits="Saving.Applications.hr.ws_hr_payroll_inc_exc_ctrl.DsList" %>

<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
    <center>
        <table class="FormStyle" style="width: 550px;">
         <tr>
            <td style="text-align: right;">
                <strong>รวมรายการยอดรับ:</strong>
            </td>
            <td width="25%">
                <asp:TextBox ID="roll" runat="server" Style="font-size: 11px; text-align: right;
                    font-weight: bold;" ToolTip="#,##0.00"></asp:TextBox>
            </td>
            <td width="10%">
            </td>
        </tr>
        <tr>
            <td style="text-align: right;">
                <strong>รวมรายการยอดหัก:</strong>
            </td>
            <td width="25%">
                <asp:TextBox ID="pay" runat="server" Style="font-size: 11px; text-align: right;
                    font-weight: bold;" ToolTip="#,##0.00"></asp:TextBox>
                </td>
            <td width="10%">
            </td>
        </tr>
        </table>
        </center>
    </EditItemTemplate>
</asp:FormView>
