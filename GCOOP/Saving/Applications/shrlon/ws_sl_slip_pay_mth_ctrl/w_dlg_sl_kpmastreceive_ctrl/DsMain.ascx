<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsMain.ascx.cs" Inherits="Saving.Applications.shrlon.ws_sl_slip_pay_mth_ctrl.w_dlg_sl_kpmastreceive_ctrl.DsMain" %>
<link id="css1" runat="server" href="../../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table class="DataSourceFormView" style="width: 560px;">
            <tr>
                <td width="100px">
                    <div>
                        <span>ทะเบียนสมาชิก:</span>
                    </div>
                </td>
                <td width="150px">
                    <div>
                        <asp:TextBox ID="MEMBER_NO" runat="server"></asp:TextBox>
                    </div>
                </td>
                <td width="100px">
                    <div>
                        <span>งวด :</span>
                    </div>
                </td>
                <td width="150px">
                    <div>
                        <asp:DropDownList ID="RECV_PERIOD" runat="server">
                        </asp:DropDownList>
                    </div>
                </td>
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>
