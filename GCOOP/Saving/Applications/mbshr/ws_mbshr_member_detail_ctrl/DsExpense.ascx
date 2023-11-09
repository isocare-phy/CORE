<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsExpense.ascx.cs" Inherits="Saving.Applications.mbshr.ws_mbshr_member_detail_ctrl.DsExpense" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <span class="TitleSpan">บัญชีหลักสมาชิก</span>
        <table class="DataSourceFormView" style="width: 700px;">
            <tr>
                <td>
                    <div>
                        <span>ประเภทชำระ:</span>
                    </div>
                </td>
                <td>
                    <asp:DropDownList ID="expense_code" runat="server" Enabled="false">
                    </asp:DropDownList>
                </td>
                <td>
                    <div>
                        <span>เลขที่บัญชี:</span>
                    </div>
                </td>
                <td>
                    <asp:TextBox ID="expense_accid" runat="server" ReadOnly="true"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td width="13%">
                    <div>
                        <span>สาขา:</span>
                    </div>
                </td>
                <td width="35%">
                    <asp:TextBox ID="expense_bank" runat="server" Width="25%" ReadOnly="true"></asp:TextBox>
                    <asp:TextBox ID="bank_desc" runat="server" Width="70%" ReadOnly="true"></asp:TextBox>
                </td>
                <td width="13%">
                    <div>
                        <span>ธนาคาร:</span>
                    </div>
                </td>
                <td width="44%">
                    <asp:TextBox ID="expense_branch" runat="server" Width="25%" ReadOnly="true"></asp:TextBox>
                    <asp:TextBox ID="branch_name" runat="server" Width="70%" ReadOnly="true"></asp:TextBox>
                </td>
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>
