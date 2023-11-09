<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsTaxcal.ascx.cs" Inherits="Saving.Applications.hr.w_sheet_hr_tax_ctrl.DsSalary" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table class="DataSourceFormView" style="width: 400px;">
            <tr>
                <td width="100">
                    <div>
                        <span>เงินเดือน</span>
                    </div>
                </td>
                <td colspan="2" width="100">
                    <div>
                       <asp:TextBox ID="salary" runat="server" style="text-align:right; background-color:#E7E7E7;" ToolTip="#,##0.00"></asp:TextBox>
                       
                    </div>
                </td>
            </tr>
            <tr>
                <td width="100">
                    <div>
                        <span>ตกเบิก</span>
                    </div>
                </td>
                <td colspan="2" width="100">
                    <div>
                       <asp:TextBox ID="A02" runat="server" style="text-align:right; " ToolTip="#,##0.00"></asp:TextBox>
                       
                    </div>
                </td>
            </tr>
            <tr>
                <td width="100">
                    <div>
                        <span>โบนัส</span>
                    </div>
                </td>
                <td colspan="2" width="100">
                    <div >
                       <asp:TextBox ID="bonus" runat="server" style="text-align:right; background-color:#E7E7E7;" ToolTip="#,##0.00"></asp:TextBox>
                       
                    </div>
                </td>
            </tr>
            <tr>
            <td></td>
                <td width="100">
                    <div>
                        <span>รวมเงินได้พึงประเมิน</span>
                    </div>
                </td>
                <td width="80">
                    <div>
                       <asp:TextBox ID="COMPUTE_1" runat="server" style="text-align:right; background-color:#000000;color:#00FF00; " ToolTip="#,##0.00"></asp:TextBox>
                       
                    </div>
                </td>
            </tr>
           
            
        </table>
    </EditItemTemplate>
</asp:FormView>