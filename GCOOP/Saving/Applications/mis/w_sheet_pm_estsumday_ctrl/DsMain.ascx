<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsMain.ascx.cs" Inherits="Saving.Applications.mis.w_sheet_pm_estsumday_ctrl.DsMain" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<link id="css2" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table class="DataSourceFormView">
            <tr>
                <td width="18%">
                    <div>
                        <span>ประเภทเงินลงทุน : </span>
                    </div>
                </td>
                <td width="30%" align="left">
                    <div>
                        <asp:DropDownList ID="dropdown" runat="server" Style="margin-right: 20px" BackColor="#FFFFCC">
                           
                        </asp:DropDownList>
                    </div>
                </td>
                <td></td>
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>
