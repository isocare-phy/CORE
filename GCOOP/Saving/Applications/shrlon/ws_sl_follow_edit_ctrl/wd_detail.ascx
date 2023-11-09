<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wd_detail.ascx.cs" Inherits="Saving.Applications.shrlon.ws_sl_follow_edit_ctrl.wd_detail" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<link id="css2" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table class="DataSourceFormView" style="width: 700px;">
            <tr>
                <td>
                    <div>
                        <span>เลขสมาชิก:</span>
                    </div>
                </td>
                <td>
                    <asp:TextBox ID="member_no" runat="server" ReadOnly="true" BackColor="#DDDDDD"
                        Style="text-align: center;"></asp:TextBox>
                </td>
                <td>
                    <div>
                        <span>เลขสัญญา:</span>
                    </div>
                </td>
                <td>
                    <asp:TextBox ID="loancontract_no" runat="server" Style="text-align: center" ReadOnly="true"
                        BackColor="#DDDDDD" ToolTip="#,##0.00"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <strong><u>รายละเอียด</u></strong>
                </td>
            </tr>
            <tr>
                <td width="25%">
                    <div>
                        <span>ประเภทสัญญา:</span>
                    </div>
                </td>
                <td width="25%">
                    <asp:TextBox ID="loantype_code" runat="server" Style="text-align: center;" ReadOnly="true" BackColor="#DDDDDD"></asp:TextBox>
                </td>
                <td width="25%">
                    <div>
                        <span style="font-size: 12px;">งวดส่ง:</span>
                    </div>
                </td>
                <td width="25%">
                    <asp:TextBox ID="period_payamt" runat="server" Style="text-align: right;" ToolTip="#,##0.00"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span>วันที่อนุมัติสัญญา:</span>
                    </div>
                </td>
                <td>
                    <asp:TextBox ID="loanapprove_date" runat="server" Style="text-align: center;"></asp:TextBox>
                </td>
                <td>
                    <div>
                        <span>ส่งต่องวด:</span>
                    </div>
                </td>
                <td>
                    <asp:TextBox ID="period_payment" runat="server" Style="text-align: right;" ToolTip="#,##0.00"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span>คิดดอกเบี้ยล่าสุด:</span>
                    </div>
                </td>
                <td>
                    <asp:TextBox ID="lastcalint_date" runat="server" Style="text-align: center;"></asp:TextBox>
                </td>
                <td>
                    <div>
                        <span style="font-size: 12px;">รวมต้นค้าง:</span>
                    </div>
                </td>
                <td>
                    <asp:TextBox ID="principal_arrear" runat="server" Style="text-align: right;" ToolTip="#,##0.00" ReadOnly="true" BackColor="#DDDDDD"></asp:TextBox>
                </td>
                
            </tr>
            <tr>
                <td>
                    <div>
                        <span>คงเหลือ:</span>
                    </div>
                </td>
                <td>
                    <asp:TextBox ID="principal_balance" runat="server" Style="text-align: right;" ToolTip="#,##0.00"></asp:TextBox>
                </td>
                <td>
                    <div>
                        <span>รวมดอกเบี้ยค้าง:</span>
                    </div>
                </td>
                <td>
                    <asp:TextBox ID="interest_arrear" runat="server" Style="text-align: right;" ToolTip="#,##0.00" ReadOnly="true" BackColor="#DDDDDD"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <strong><u>สถานะ</u></strong>
                </td>
                <td colspan="2">
                    <strong><u>ลำดับล่าสุด</u></strong>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span>สถานะ:</span>
                    </div>
                </td>
                <td>
                    <asp:TextBox ID="contract_status" runat="server" Style="text-align: center;"></asp:TextBox>
                </td>
                 <td>
                    <div>
                        <span>Statement ล่าสุด:</span>
                    </div>
                </td>
                <td>
                    <asp:TextBox ID="last_stm_no" runat="server" Style="text-align: center;"></asp:TextBox>
                </td>
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>
