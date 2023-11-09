<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsMain.ascx.cs" Inherits="Saving.Applications.cmd.ws_cmd_dealer_ctrl.DsMain" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet" type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit" Height="97px" 
    Width="734px">
    <EditItemTemplate>
        <table class="DataSourceFormView">
            <tr>
                <td width="20%">
                    <div><span> เลขทะเบียนผู้จำหน่าย : </span></div>
                </td>
                <td>
                    <div><asp:TextBox ID="dealer_no" runat="server" Width="150px"></asp:TextBox></div>
                </td>
                <td width="20%">
                    <div> <span> ชื่อผู้จำหน่าย : </span></div>
                </td>
                <td>
                    <div><asp:TextBox ID="dealer_name" runat="server" Width="100%"></asp:TextBox></div>
                </td>
                <tr>
                 <td width="20%">
                    <div><span> ที่อยู่ : </span></div>
                </td>
                <td>
                    <div><asp:TextBox ID="dealer_addr" runat="server" Width="245%"></asp:TextBox></div>
                </td>
                </tr>
                <tr>
                 <td width="20%">
                    <div><span> เลขที่เสียภาษี : </span></div>
                </td>
                <td>
                    <div><asp:TextBox ID="dealer_taxid" runat="server" Width="245%"></asp:TextBox></div>
                </td>
                </tr>
                <tr>
                 <td width="20%">
                    <div><span> โทรศัพท์ : </span></div>
                </td>
                <td>
                    <div><asp:TextBox ID="dealer_phone" runat="server" Width="100%"></asp:TextBox></div>
                </td>
                </tr>
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>