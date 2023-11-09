<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsMain.ascx.cs" Inherits="Saving.Applications.assist.dlg.ws_dlg_as_assistpay_ctrl.DsMain" %>
<link id="css1" runat="server" href="../../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView2" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table border="0">
            <br />
            <%--<tr>
                <td>
                    <div align="left">
                        <asp:CheckBox ID="print" runat="server" />
                        บันทึก&พิมพ์ใบเสร็จ
                    </div>
                </td>
            </tr>--%>
        </table>
        <table class="DataSourceFormView">
            <tr>
                <td width="13%">
                    <div>
                        <span style="font-size: 11px;">ทะเบียน:</span>
                    </div>
                </td>
                <td width="17%">
                    <asp:TextBox ID="member_no" runat="server" Style="text-align: center; background-color: #CCCCCC;"></asp:TextBox>
                </td>
                <td width="13%">
                    <div>
                        <span style="font-size: 11px;">ชื่อ-ชื่อสกุล:</span>
                    </div>
                </td>
                <td width="27%">
                    <asp:TextBox ID="full_name" runat="server" ReadOnly="true" Style="background-color: #CCCCCC;"></asp:TextBox>
                </td>
                <td width="13%">
                    <div>
                        <span style="font-size: 11px;">วันที่จ่าย:</span></div>
                </td>
                <td width="17%">
                    <asp:TextBox ID="REQ_DATE" runat="server" Style="text-align: center;background-color: #CCCCCC;"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span style="font-size: 11px;">สังกัด :</span>
                    </div>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="membgroup_desc" runat="server" Style="width:407px;background-color: #CCCCCC;"></asp:TextBox>
                </td>
                <td width="13%">
                    <div>
                        <span style="font-size: 11px;">ประเภทสมาชิก :</span>
                    </div>
                </td>
                <td width="17%">
                    <asp:TextBox ID="member_type" runat="server" Style="text-align: center; background-color: #CCCCCC"></asp:TextBox>
                </td>
            </tr>
        </table>
        <table class="DataSourceFormView" border="0">
           <tr>
                <td colspan="4">
                    <strong style="font-size: 12px;">รายละเอียดสัญญา</strong>
                </td>
            </tr>
            <tr>
                <td width="18%">
                    <div>
                        <span style="font-size: 11px;">เลขที่ใบคำขอ:</span>
                    </div>
                </td>
                <td width="25%">
                    <asp:TextBox ID="ASSIST_DOCNO" runat="server" Style="text-align: center; background-color:Lime;"></asp:TextBox>
                </td>
                <td width="15%">
                    <div>
                        <span style="font-size: 11px;">ประเภทสวัสดิการ :</span>
                    </div>
                </td>
                <td width="20%">
                     <%-- <asp:TextBox ID="ASSISTTYPE_CODE" runat="server" Style="text-align: center; background-color: #FFFFCC;"></asp:TextBox>--%>
                  <asp:DropDownList ID="assisttype_desc" runat="server" Style="margin-left: 1px; width: 130px;" 
                        BackColor="#CCCCCC">
                    </asp:DropDownList>
                </td>       
                <td width="15%">
                    <div>
                        <span style="font-size: 11px;">ประเภทการจ่ายสวัสดิการ :</span>
                    </div>
                </td>
                <td width="20%">
                     <asp:DropDownList ID="assistpay_desc" runat="server" Style="margin-left: 1px; width: 115px;"
                        BackColor="#CCCCCC">
                    </asp:DropDownList>
                </td>  
             </tr>            
        </table>
        <%--/////////////////--%>
        <table>
            <tr>
                <td>
                    <table class="DataSourceFormView" style="width: 350px;">                        
                        <tr>
                            <td colspan="4">
                                <u><b>จ่ายสวัสดิการโดย:</b></u>
                            </td>
                        </tr>
                        <tr>
                            <td width="20%">
                                <div>
                                    <span style="font-size: 12px;">ประเภทเงิน:</span>
                                </div>
                            </td>
                            <td width="30%" colspan="3">
                                <asp:DropDownList ID="moneytype_code" runat="server" Style="font-size: 12px;text-align:left;">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                             <td width="20%">
                                <div>
                                    <span style="font-size: 12px;">ระบบ:</span>
                                </div>
                            </td>
                            <td width="30%">
                                <asp:DropDownList ID="send_system" runat="server" Style="font-size: 12px; text-align: center;">
                                    <asp:ListItem Value="DEP">เงินฝาก</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>                           
                            <td>
                                <div>
                                    <span style="font-size: 12px;">รหัสบัญชี:</span>
                                </div>
                            </td>
                            <td colspan="3">
                                <asp:DropDownList ID="tofrom_accid" runat="server" Style="font-size: 12px; text-align: left;">
                                 </asp:DropDownList>
                            </td>
                        </tr>     
                        <tr>                            
                            <td width="50%">  
                                <div>
                                    <span style="font-size:12px;font-weight:bold;">ยอดอนุมัติ :</span>
                                </div>
                            </td>  
                            <td>  
                                <asp:TextBox ID="APPROVE_AMT" runat="server" Style="background-color: Black;text-align:right;font-weight:bold;" 
                                        ToolTip="#,##0.00" ForeColor="#CCFF33" ReadOnly="true"></asp:TextBox>
                            </td>          
                        </tr>                                                         
                 </table> 
              </td>
              <td>
                <table class="DataSourceFormView" style="width: 380px;">                                               
                        <tr>
                            <td colspan="4">
                                <u><b>บัญชีสมาชิก:</b></u>
                            </td>
                        </tr>
                        <tr>
                            <td width="30%">
                                <div>
                                    <span style="font-size: 12px;">ธนาคาร :</span>
                                </div>
                            </td>
                            <td width="30%">
                                <asp:DropDownList ID="expense_bank" runat="server" Style="font-size: 12px; text-align:left;">
                                 </asp:DropDownList>
                            </td>  
                        
                            <td>
                                <div>
                                    <span style="font-size: 11px;">สาขา :</span>
                                </div>
                            </td>
                            <td >
                                <asp:DropDownList ID="expense_branch" runat="server"  BackColor="#FFFFCC">
                                </asp:DropDownList>
                            </td> 
                        </tr>
                        <tr>                                                 
                            <td>
                                <div>
                                    <span style="font-size: 12px;">เลขที่บัญชี:</span>
                                </div>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="DEPTACCOUNT_NO" runat="server" Style="font-size: 12px; text-align: left;"></asp:TextBox>
                                                                 
                            </td>     
                            <td>
                                <asp:Button ID="b_searchdeptno" runat="server" Text="..ค้นหา.."  Style="width:100px" />    
                            </td>                                               
                        </tr>         
                        <tr>
                            <td>
                                <div>
                                    <span style="font-size: 11px;">ค่าธรรมเนียม :</span>
                                </div>
                            </td>
                            <td  colspan="3">
                                <asp:TextBox ID="fee" runat="server" Style="background-color: #CCCCCC;text-align:right;" ToolTip="#,##0.00"></asp:TextBox>                  
                            </td> 
                        </tr>  
                        <tr>                                               
                           <td>                    
                                <div>
                                    <span style="font-size:12px;font-weight:bold;">ยอดจ่าย :</span>
                                </div>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="ASSIST_AMT" runat="server" Style="background-color: Black;text-align:right;font-weight:bold;" ToolTip="#,##0.00" ForeColor="#CCFF33"   ></asp:TextBox>
                            </td>   
                        </tr>                                                   
                 </table>                    
              </td>
            </tr>
        </table>
        <br />
        <table class="DataSourceFormView">
            <tr>
                <td >
                <asp:CheckBox ID="chkloan" Checked="false" runat="server"/>
                    <strong style="font-size: 12px;">รายการหักชำระหนี้</strong>
                </td>
            </tr>
            <tr> 
                <td>
                    <div>
                        <span style="float:right;text-align:center;font-size: 11px;">รายละเอียด</span>
                    </div>
                </td>   
                <td>
                    <div>
                        <span style="float:left;text-align:center;">จำนวนเงิน</span>
                    </div>
                </td> 
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="TextBox1" runat="server" Style="float:right;text-align: left; font-size: 11px;
                        background-color: #EEEEEE;" ReadOnly="true">-ชำระหนี้</asp:TextBox>                       
                </td>      
                <td>
                    <asp:TextBox ID="lon_amt" runat="server" Style="text-align: right; font-size: 11px;" ToolTip="#,##0.00" ></asp:TextBox>
                </td>    
            </tr>
            <tr style="height:30px;"></tr>
            <tr>
                <td >
                    <div id="show_box"><asp:CheckBox ID="chkgain" runat="server"/>
                        <strong style="font-size: 12px;">ผู้รับผลประโยชน์</strong>
                    </div>
                </td>
            </tr>
        </table>        
    </EditItemTemplate>
</asp:FormView>
