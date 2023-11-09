<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsSum.ascx.cs" Inherits="Saving.Applications.account.dlg.w_dlg_vc_trn_fin_ctrl.DsSum" %>
<link id="css1" runat="server" href="../../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:Panel ID="Panel1" runat="server" Width="670px" HorizontalAlign="Left">
    <asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
        <EditItemTemplate>
            <table class="DataSourceFormView" style="width: 650px;">
                <tr>
                    <td width="10%">
                        รวมจ่าย
                    </td>
                    <td width="20%">
                        <div>
                            <asp:TextBox ID="operate_flag_sum" runat="server" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                        </div>
                    </td>
                    <td width="10%">
                        สัญญา
                    </td>
                    <td>
                        <div>
                        </div>
                    </td>
                    <td>
                        <div>
                            <asp:TextBox ID="payment_sum" runat="server" ReadOnly="true" Style="text-align: right"
                                ToolTip="#,##0.00"></asp:TextBox>
                        </div>
                    </td>
                </tr>
            </table>
            <br />
            <table>
                <tr>
                    <td>
                        <div>
                            <asp:Button ID="btn_ok" runat="server" Text="ตกลง" Style="width: 50px" />
                        </div>
                    </td>
                    <td>
                        <div>
                            <asp:Button ID="btn_cancel" runat="server" Text="ยกเลิก" Style="width: 50px" />
                        </div>
                    </td>
                </tr>
            </table>
        </EditItemTemplate>
    </asp:FormView>
</asp:Panel>