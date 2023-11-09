<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsMain.ascx.cs" Inherits="Saving.Applications.mbshr.ws_mbshr_update_shr_ctrl.DsMain" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet" type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table class="DataSourceFormView">
            <tr>
                <td></td>
                <td width="20%"> 
                    <div> <span>สังกัด :</span></div>
                </td>
                <td width="">
                    <asp:DropDownList ID="smembgroup_code" runat="server">
                    </asp:DropDownList>
                </td>
                <td width="20%">
                    <div> <span>ถึงสังกัด :</span></div>
                </td>
                <td>
                    <asp:DropDownList ID="emembgroup_code" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>