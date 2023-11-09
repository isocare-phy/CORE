<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsList.ascx.cs" Inherits="Saving.Applications.account.dlg.w_dlg_vc_trn_loan_ctrl.DsList" %>
<link id="css1" runat="server" href="../../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<div align="left">
    <asp:Panel ID="Panel1" runat="server" Width="670px" HorizontalAlign="Left">
        <table class="DataSourceRepeater" style="width: 650px;">
            <tr>
                <th width="4%">
                    <asp:CheckBox ID="chk_all" Checked="false" runat="server" onclick="Check_All()"/>  
                </th>
                <th width="15%">
                    เลขที่จ่าย
                </th>
                <th width="30%">
                    ชื่อ-ชื่อสกุล
                </th>
                <th width="8%">
                    ประเภท
                </th>
                <th width="13%">
                    เลขสัญญา
                </th>
                <th width="15%">
                    จำนวนเงินจ่าย
                </th>
                <th width="10%">
                    ผู้ทำรายการ
                </th>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="Panel2" runat="server" Height="390px" ScrollBars="Auto" Width="670px"
        HorizontalAlign="Left">
        <table class="DataSourceRepeater" style="width: 650px;">
            <asp:Repeater ID="Repeater1" runat="server">
                <ItemTemplate>
                    <tr>
                        <td width="4%">
                            <asp:CheckBox ID="operate_flag" runat="server" />
                        </td>
                        <td width="15%">
                            <asp:TextBox ID="payoutslip_no" runat="server" ReadOnly="true" Style="cursor: pointer;"></asp:TextBox>
                        </td>
                        <td width="30%">
                            <asp:TextBox ID="memb_name" runat="server" ReadOnly="true" Style="cursor: pointer;"> </asp:TextBox>
                        </td>
                        <td width="8%">
                            <asp:TextBox ID="shrlontype_code" runat="server" ReadOnly="true" Style="cursor: pointer;
                                text-align: center"></asp:TextBox>
                        </td>
                        <td width="13%">
                            <asp:TextBox ID="loancontract_no" runat="server" ReadOnly="true" Style="cursor: pointer;"></asp:TextBox>
                        </td>
                        <td width="15%">
                            <asp:TextBox ID="payout_amt" runat="server" ReadOnly="true" Style="cursor: pointer;
                                text-align: right" ToolTip="#,##0.00"></asp:TextBox>
                        </td>
                        <td width="10%">
                            <asp:TextBox ID="entry_id" runat="server" ReadOnly="true" Style="cursor: pointer;"></asp:TextBox>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </asp:Panel>
    <hr width="670px" align="left" />
</div>
