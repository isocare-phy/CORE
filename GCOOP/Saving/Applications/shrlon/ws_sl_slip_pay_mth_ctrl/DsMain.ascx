<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsMain.ascx.cs" Inherits="Saving.Applications.shrlon.ws_sl_slip_pay_mth_ctrl.DsMain" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<link id="css2" runat="server" href="../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit" Width="406px">
    <EditItemTemplate>
        <table class="DataSourceFormView" style="width: 770px;">
            <tr>
                <td colspan="8">
                    <div align="right">
                        <asp:CheckBox ID="print" runat="server" />
                        บันทึก&พิมพ์ใบเสร็จ
                    </div>
                </td>
            </tr>
            <tr>
                <td width="12%">
                    <div>
                        <span style="font-size: 12px;">เลขที่ใบเสร็จ :</span>
                    </div>
                </td>
                <td width="12%">
                    <asp:TextBox ID="document_no" runat="server" Style="font-size: 12px; text-align: center;
                        background-color: #FFFF99;" ReadOnly="true"></asp:TextBox>
                </td>
                <td width="12%">
                    <div>
                        <span style="font-size: 12px;">วันที่ใบเสร็จ :</span>
                    </div>
                </td>
                <td width="12%">
                    <asp:TextBox ID="slip_date" runat="server" Style="font-size: 12px; text-align: center;
                        background-color: #FFFF99;" ReadOnly="true"></asp:TextBox>
                </td>
                <td width="12%">
                    <div>
                        <span style="font-size: 12px;">วันที่รายการ :</span>
                    </div>
                </td>
                <td width="12%">
                    <asp:TextBox ID="operate_date" runat="server" Style="font-size: 12px; text-align: center;"></asp:TextBox>
                </td>
                <td width="10%">
                    <div>
                        <span style="font-size: 12px;">รหัสรายการ :</span>
                    </div>
                </td>
                <td width="14%">
                    <asp:DropDownList ID="SLIPTYPE_CODE" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span style="font-size: 12px;">ทะเบียนสมาชิก :</span>
                    </div>
                </td>
                <td>
                    <asp:TextBox ID="member_no" runat="server" Style="width: 70px; text-align: center;"></asp:TextBox>
                    <asp:Button ID="b_memsearch" runat="server" Text="..." Style="width: 20px; margin-right: 0px;" />
                </td>
                <td>
                    <div>
                        <span style="font-size: 12px;">ชื่อ-สกุล :</span>
                    </div>
                </td>
                <td colspan="5">
                    <asp:TextBox ID="nameb" runat="server" Style="font-size: 12px; text-align: left;
                        background-color: #FFFF99;" Width="99%" ReadOnly="true"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span style="font-size: 12px;">หน่วย :</span>
                    </div>
                </td>
                <td colspan="5">
                    <asp:TextBox ID="membgroup" runat="server" Style="font-size: 12px; text-align: left;
                        background-color: #FFFF99;" Width="99%" ReadOnly="true"></asp:TextBox>
                </td>
                <td colspan="2">
                    <strong style="margin-left: 5px; font-size: 16px;">รวมยอดชำระ : </strong>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span style="font-size: 12px;">ทำรายการโดย :</span>
                    </div>
                </td>
                <td colspan="2">
                    <asp:DropDownList ID="moneytype_code" runat="server">
                    </asp:DropDownList>
                </td>
                <td>
                    <div>
                        <span style="font-size: 12px;">
                            <table width="100%">
                                <tr>
                                    <td align="left">
                                        <asp:CheckBox ID="accid_flag" runat="server" />
                                    </td>
                                    <td align="right">
                                        คู่บัญชี :
                                    </td>
                                </tr>
                            </table>
                        </span>
                    </div>
                </td>
                <td colspan="2">
                    <asp:DropDownList ID="tofrom_accid" runat="server" Style="width: 200px">
                    </asp:DropDownList>
                </td>
                <td colspan="2" rowspan="2">
                    <asp:TextBox ID="slip_amt" runat="server" Style="background-color: Black; margin-left: 5px;
                        text-align: right; font-size: 22px;" Width="200" Height="45" ToolTip="#,##0.00"
                        ForeColor="GreenYellow"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span>สถานะรายการ:</span>
                    </div>
                </td>
                <td colspan="2">
                    <asp:TextBox ID="compute1" runat="server" Style="text-align: center;" ></asp:TextBox>
                </td>
                <td>
                    <div>
                        <span>ผู้ทำรายการ :</span>
                    </div>
                </td>
                <td colspan="2">
                    <asp:TextBox ID="entry_id" runat="server" Style="width: 198px;"></asp:TextBox>
                </td>
            </tr>
            <tr >
                <td>
                    <div>
                        <span>อ้างอิงงวด:</span>
                    </div>
                </td>
                <td colspan="2">
                <asp:DropDownList ID="recv_period" runat="server">
                    </asp:DropDownList>
                </td>
                <td>
                    <div>
                        <span>ใบเสร็จ:</span>
                    </div>
                </td>
                <td colspan="2">
                    <asp:DropDownList ID="receipt_no" runat="server" Style="width: 178px">
                    </asp:DropDownList>
                    <asp:Button ID="b_ref" runat="server" Text="..." Style="margin-right: 2px;
                        font-size: 16px; width: 20px;" Font-Bold="true " />
                </td>
            </tr>
            <tr>
            <td>
                    <div>
                        <span>ต้นค้าง:</span>
                    </div>
                </td>
                <td>
                    <asp:TextBox ID="principal_arrear" runat="server" Style="text-align: right; width: 90px; background-color: Black;" ToolTip="#,##0.00" ForeColor="GreenYellow"></asp:TextBox>
                </td>
                 <td>
                    <div>
                        <span>ดอกเบี้ยค้าง:</span>
                    </div>
                </td>
                <td>
                    <asp:TextBox ID="intyear_arrear" runat="server" Style="text-align: right; width: 90px; background-color: Black;" ToolTip="#,##0.00" ForeColor="GreenYellow"></asp:TextBox>
                </td>
                <td>
                    <div>
                        <span>ด/บ ค้างรวม:</span>
                    </div>
                </td>
                <td colspan="2">
                    <asp:TextBox ID="receive_amt" runat="server" Style="text-align: right; width: 90px; background-color: Black;" ToolTip="#,##0.00" ForeColor="GreenYellow"></asp:TextBox>
                            
                </td>
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>
