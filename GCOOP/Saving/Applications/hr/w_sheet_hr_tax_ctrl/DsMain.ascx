<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsMain.ascx.cs" Inherits="Saving.Applications.hr.w_sheet_hr_tax_ctrl.DsMain" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table class="DataSourceFormView" style="width: 400px;">
     
            <tr>
                <td width="100">
                    <div>
                        <span>รหัสพนักงาน:</span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:DropDownList ID="EMPLCODE" runat="server">
                        </asp:DropDownList>
                    </div>
                </td>
                <td rowspan ="2" colspan ="2">
                <div>
                 <asp:Button ID="taxcal" runat="server" Text="คำนวณภาษี" Width="100px" Height="25px" />
                </div>
                </td>
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>