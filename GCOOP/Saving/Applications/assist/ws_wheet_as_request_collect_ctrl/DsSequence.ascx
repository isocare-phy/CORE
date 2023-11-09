<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsSequence.ascx.cs" Inherits="Saving.Applications.assist.ws_wheet_as_request_collect_ctrl.DsSequence" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table class="DataSourceFormView" style="width: 758px;">
            <tr>
                <td colspan="5">
                    <div>
                        <span style="width:99%;text-align:center">ค่าหุ้นสะสมรายเดือนเฉลี่ย</span>
                    </div>
                </td>
            </tr>
            <tr>                
                <td>
                    <div>
                        <span style="text-align:center">ขั้นที่ 1 (24-60) 5%</span>
                    </div>
                </td>
                <td>
                    <div>
                        <span style="text-align:center">ขั้นที่ 2 (61-120) 10%</span>
                    </div>
                </td>
                <td>
                    <div>
                        <span style="text-align:center">ขั้นที่ 3 (121-180) 15%</span>
                    </div>
                </td>
                <td>
                    <div>
                        <span style="text-align:center">ขั้นที่ 4 (181-240) 20%</span>
                    </div>
                </td>
                <td>
                    <div>
                        <span style="text-align:center">ขั้นที่ 5 (241 ขึ้นไป) 25%</span>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <asp:TextBox ID="sequence_one" runat="server" Style="text-align:right" ToolTip="#,##0.00"></asp:TextBox>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="sequence_two" runat="server" Style="text-align:right" ToolTip="#,##0.00"></asp:TextBox>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="sequence_three" runat="server" Style="text-align:right" ToolTip="#,##0.00"></asp:TextBox>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="sequence_four" runat="server" Style="text-align:right" ToolTip="#,##0.00"></asp:TextBox>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="sequence_five" runat="server" Style="text-align:right" ToolTip="#,##0.00"></asp:TextBox>
                    </div>
                </td>                            
            </tr> 
            <tr>
                <td colspan="2">
                    <div>
                        <span style="text-align:right;height:35px;font-size:18px;">รวมเงินสวัสดิการสะสม:</span>
                    </div>
                </td>
                <td colspan="3">
                    <div>
                        <asp:TextBox ID="assist_collect" runat="server" BackColor="Black" 
                            ForeColor="#CCFF33" 
                            Style="width:495px;height:35px;font-size:18px;text-align:right;" 
                            ToolTip="#,##0.00"></asp:TextBox>
                    </div>
                </td>     
            </tr>      
        </table>
        <table class="DataSourceFormView" style="width: 758px;">
           <tr>
                <td width="13%">
                    <div>
                        <span style="height:30px;">หนี้คงเหลือรวม:</span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="principal_balance" runat="server" ToolTip="#,##0.00" Style="height:30px;text-align:right"></asp:TextBox>
                    </div>
                </td>
                <td width="13%">
                    <div>
                        <span style="height:30px;">ดอกเบี้ยรวม:</span>
                    </div>
                </td>
                <td width="13%">
                    <div>
                        <asp:TextBox ID="int_amt" runat="server" ToolTip="#,##0.00" Style="height:30px;text-align:right"></asp:TextBox>
                    </div>
                </td>
                <td width="25%">
                    <div>
                        <span style="height:30px;">หลังจากหักหนี้แล้วคงเหลือรับได้:</span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="prnc_amt" runat="server" BackColor="Black" ForeColor="#CCFF33" 
                            ToolTip="#,##0.00" 
                            Style="font-size:16px;height:30px;text-align:right;"></asp:TextBox>
                    </div>
                </td>
            </tr>   
         </table> 
    </EditItemTemplate>
</asp:FormView>