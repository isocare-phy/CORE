<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsBank.ascx.cs" Inherits="Saving.Applications.assist.dlg.ws_dlg_as_assistpay_v2_ctrl.DsBank" %>
<link id="css1" runat="server" href="../../../../JsCss/DataSourceTool.css" rel="stylesheet"    type="text/css" />
<asp:FormView ID="FormView2" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table class="DataSourceFormView">
            <tr >
                <td width="13%">
                    <div>
                        <span style="font-size: 11px;">เชื่อมโยงผ่านบัญชี:</span>
                    </div>
                </td>

                <td>

                        <asp:DropDownList ID="accaccount" runat="server"></asp:DropDownList>

                </td>
           </table>
    </EditItemTemplate>
</asp:FormView>