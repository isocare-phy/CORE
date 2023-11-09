<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsAmount.ascx.cs" Inherits="Saving.Applications.assist.ws_wheet_as_request_ctrl.DsAmount" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />

<script type="text/javascript">
    function chkNumber(ele) {
        var vchar = String.fromCharCode(event.keyCode);
        if ((vchar < '0' || vchar > '9') && (vchar != '.')) return false;
        ele.onKeyPress = vchar;
    }

</script>

<br />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table>
            
            <tr>
                <td>
                    <table class="DataSourceFormView"  style="width:450px;">  
                        <tr>
                            <td >
                                <div>
                                    <span>หนี้คงเหลือ:</span>
                                </div>
                            </td>
                            <td colspan="3">                       
                               <asp:TextBox ID="principal_balance" runat="server" ToolTip="#,##0.00" Style="text-align:right"></asp:TextBox>           
                            </td>   
                                                                                                                                                               
                        </tr>
                        <tr>
                            <td width="20%">
                                <div>
                                    <span>การจ่ายเงิน:</span>
                                </div>
                            </td>
                            <td width="25%">              
                                 <asp:DropDownList ID="moneytype_code" runat="server" >
                               </asp:DropDownList>                    
                            </td>   
                            <td width="20%">
                                <div>
                                    <span>ธนาคาร:</span>
                                </div>
                            </td>
                            <td width="25%">
                                <asp:DropDownList ID="expense_bank" runat="server" ></asp:DropDownList>                   
                            </td>                                                                                                                                        
                        </tr>
                        <tr>                    
                            <td>
                                <div>
                                    <span>เลขธนาคาร:</span>
                                </div>
                            </td>
                            <td>
                                <asp:TextBox ID="expense_accid" runat="server" OnKeyPress="return chkNumber(this)"></asp:TextBox>
                            </td>    
                            <td>
                                <div>
                                    <span>สาขา:</span>
                                </div>
                            </td>
                            <td >
                                <asp:DropDownList ID="expense_branch" runat="server" >
                                </asp:DropDownList>                   
                            </td>                                     
                       </tr>
                       <tr>                       
                            <td>
                                <div>
                                    <span>โอนไประบบ:</span>
                                </div>
                            </td>
                            <td >
                                <asp:DropDownList ID="send_system" runat="server" >
                                    <asp:ListItem Value="">กรุณาเลือกระบบ</asp:ListItem>
                                    <asp:ListItem Value="DEP">เงินฝาก</asp:ListItem>
                                    <asp:ListItem Value="LON">สินเชื่อ</asp:ListItem>
                                </asp:DropDownList>                   
                            </td>  
                            <td>
                                <div>
                                    <span>เลขที่บัญชี:</span>
                                </div>
                            </td>
                            <td width="15%">    
                                <asp:DropDownList ID="deptaccount_no" runat="server" >          
                                </asp:DropDownList>                     
                            </td>                                    
                       </tr>      
                       <tr>                                                                                                                                  
                            <td>
                                <div>
                                    <span>หมายเหตุ:</span>
                                </div>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="remark" runat="server" ></asp:TextBox>                   
                            </td>                                
                        </tr>
                    </table>          
                </td>
                <td>
                    <table class="DataSourceFormView"  style="width:300px;">                  
                        <tr>         
                            <td>
                                <div>
                                    <span style="height:40px;">ยอดเงินตามสิทธิ์:</span>
                                </div>
                            </td>                        
                            <td width="150px">              
                                <asp:TextBox ID="system_amt" runat="server"  BackColor="#00000" ForeColor="#CCFF33" ReadOnly="true" ToolTip="#,##0.00" Style="font-size:20px;text-align:right;height:40px;" OnKeyPress="return chkNumber(this)"></asp:TextBox>                   
                            </td>  
                       </tr>  
                       <tr>         
                            <td>
                                <div>
                                    <span style="height:40px;">ยอดเงินที่เคยได้:</span>
                                </div>
                            </td>                        
                            <td width="150px">              
                                <asp:TextBox ID="ever_amt" runat="server"  BackColor="#00000" ForeColor="#CCFF33" ReadOnly="true"  ToolTip="#,##0.00" Style="font-size:20px;text-align:right;height:40px;" OnKeyPress="return chkNumber(this)"></asp:TextBox>                   
                            </td>  
                       </tr>   
                       <tr>         
                            <td>
                                <div>
                                    <span style="height:40px;">ยอดเงินสุทธิ:</span>
                                </div>
                            </td>                        
                            <td width="150px">              
                                <asp:TextBox ID="approve_amt" runat="server"  BackColor="#00000" ForeColor="#CCFF33" ToolTip="#,##0.00" Style="font-size:20px;text-align:right;height:40px;" OnKeyPress="return chkNumber(this)"></asp:TextBox>                   
                            </td>  
                       </tr>             
                    </table>
                </td>
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>