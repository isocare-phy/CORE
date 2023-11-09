<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsPatronize.ascx.cs"
    Inherits="Saving.Applications.assist.ws_as_request_ctrl.DsPatronize" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table class="DataSourceFormView">
            <tr>
                <td style="width: 15%">
                    <div>
                        <span>ประเภทการจ่าย:</span>
                    </div>
                </td>
                <td style="width: 85%">
                    <asp:DropDownList ID="assistpay_code" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>
