<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsContmaster.ascx.cs" Inherits="Saving.Applications.assist.ws_wheet_as_detail_ctrl.DsContmaster" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView2" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table class="DataSourceFormView" style="width: 770px;">
            <tr>
                <td width="13%">
                    <div>
                        <span>เลขสวัสดิการ:</span>
                    </div>
                </td>
                <td width="18%">                    
                    <asp:DropDownList ID="asscontract_no" runat="server">                      
                    </asp:DropDownList>                
                </td>
                <td width="13%">
                    <div>
                        <span>ยอดอนุมัติ:</span>
                    </div>
                </td>
                <td colspan="3">
                    <div>
                        <asp:TextBox ID="approve_amt" runat="server" ReadOnly="true" BackColor="#DCDCDC" ToolTip="#,##0.00" Style="text-align:right"></asp:TextBox>
                    </div>
                </td>
            </tr>
           <tr>                               
                <td>
                    <div>
                        <span>ยอดคงเหลือ:</span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="withdrawable_amt" runat="server" ReadOnly="true" BackColor="#DCDCDC" ToolTip="#,##0.00" Style="text-align:right"></asp:TextBox>
                    </div>
                </td>
                <td>
                    <div>
                        <span>ยอดจ่าย:</span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="pay_balance" runat="server" ReadOnly="true" BackColor="#DCDCDC" ToolTip="#,##0.00" Style="text-align:right"></asp:TextBox>
                    </div>
                </td>
                <td width="13%">
                    <div>
                        <span>สถานะการรับทุน:</span>
                    </div>
                </td>
                <td width="18%">                    
                    <asp:DropDownList ID="payment_status" runat="server">
                        <asp:ListItem Value="1">ปกติ</asp:ListItem>
                        <asp:ListItem Value="-9">ยกเลิก</asp:ListItem>
                        <asp:ListItem Value="8">ของดรับทุน</asp:ListItem>
                    </asp:DropDownList>                
                </td> 
            </tr>                    
        </table>
    </EditItemTemplate>
</asp:FormView>
