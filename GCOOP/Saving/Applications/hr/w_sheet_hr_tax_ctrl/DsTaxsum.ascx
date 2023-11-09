<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsTaxsum.ascx.cs" Inherits="Saving.Applications.hr.w_sheet_hr_tax_ctrl.DsTaxsum" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table class="DataSourceFormView" style=" width: 733px; " align = "center" >
            <tr>
            <td width="28%"></td>
             <td width="25%"></td>
              
                <td width="15%">
                    <div>
                        <span>รวมภาษีที่ต้องชำระ</span>
                    </div>
                </td>
                <td width="15%">
                    <div>
                        <asp:TextBox ID="taxsum" runat="server" Style="text-align: right; background-color:#000000;color:#00FF00;" ToolTip="#,##0.00"></asp:TextBox>
                    </div>
                </td>
                   <td width="5%">
                    <div>
                        <span style="text-align: center;">บาท</span>
                    </div>
                </td>
            </tr>
             <tr>
            <td width="28%"></td>
             <td width="25%"></td>
                <td width="15%">
                    <div>
                        <span>เดือนละ</span>
                    </div>
                </td>
                <td width="15%">
                    <div>
                        <asp:TextBox ID="TAXMONTH" runat="server" Style="text-align: right; background-color:#000000;color:#00FF00;" ToolTip="#,##0.00"></asp:TextBox>
                    </div>
                </td>
                <td width="5%">
                    <div>
                        <span style="text-align: center;">บาท</span>
                    </div>
                </td>
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>
