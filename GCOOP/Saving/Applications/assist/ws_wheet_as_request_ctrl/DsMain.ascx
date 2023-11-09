<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsMain.ascx.cs" Inherits="Saving.Applications.assist.ws_wheet_as_request_ctrl.DsMain" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table class="DataSourceFormView">            
            <tr>
                <td width="110px">
                    <div>
                        <span>เลขทะเบียน:</span>
                    </div>
                </td>
                <td width="104px">
                    <asp:TextBox ID="member_no" runat="server" Style="text-align:center;width:80px;" ></asp:TextBox>
                    <asp:Button ID="b_search" runat="server" Text="..." Style="width: 20px;" />                   
                </td>
                <td >
                    <div>
                        <span>ชื่อ-สกุล:</span>
                    </div>
                </td>
                <td>
                    <asp:TextBox ID="namesurname" runat="server" ReadOnly="true" BackColor="#DDDDDD" ></asp:TextBox>                   
                </td>
                <td style="width:100px">
                    <div >
                        <span>ประเภทสมาชิก:</span>
                    </div>
                </td>
                <td>
                    <asp:TextBox ID="membtype_desc" runat="server" ReadOnly="true" BackColor="#DDDDDD" Style="text-align:center;" ></asp:TextBox>                   
                </td>                              
            </tr>
            <tr>
                <td width="100px">
                    <div>
                        <span>วันที่ทำรายการ:</span>
                    </div>
                </td>
                <td >
                    <asp:TextBox ID="work_date" runat="server" ReadOnly="true" Style="text-align:center" BackColor="#DDDDDD" ></asp:TextBox>                   
                </td>
                <td>
                    <div>
                        <span>สังกัด:</span>
                    </div>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="membgroug" runat="server" ReadOnly="true" BackColor="#DDDDDD" Style="width:430px;" ></asp:TextBox>                   
                </td> 
                               
            </tr>
            <tr>
               
                <td>
                    <div>
                        <span>วันเกิด:</span>
                    </div>
                </td>
                <td>
                    <asp:TextBox ID="birth_date" runat="server" ReadOnly="true" Style="text-align:center;" BackColor="#DDDDDD" ></asp:TextBox>                   
                </td>                            
                <td>
                    <asp:TextBox ID="age" runat="server" ReadOnly="true" Style="width:70px;text-align:center;" BackColor="#DDDDDD" ></asp:TextBox>                   
                 </td>                            
                <td>
                    <div>
                        <span style="width:20px;text-align:center">ปี</span>                    
                        <span style="width:135px;">วันที่เป็นสมาชิก:</span>
                    </div>
                </td>                            
                <td>
                   <asp:TextBox ID="date_mamber" runat="server" ReadOnly="true" Style="width:110px;text-align:center;" BackColor="#DDDDDD" ></asp:TextBox>                   
                </td>                            
                <td>
                   <asp:TextBox ID="age_member" runat="server" ReadOnly="true" Style="width:110px;text-align:center;" BackColor="#DDDDDD" ></asp:TextBox>                   
                      <div>
                        <span style="width:33px;text-align:center">ปี</span>  
                      </div>
                </td>         
            </tr>
            <tr>
                <td>
                    <span>วันเกษียณ:</span>
                </td>
                <td>
                    <asp:TextBox ID="retry_date" runat="server" ReadOnly="true" Style="text-align:center;" BackColor="#DDDDDD"></asp:TextBox>        
                </td>  
                <td>
                    <span>เงินเดือน:</span>
                </td>
                <td>
                    <asp:TextBox ID="salary_amount" runat="server" ReadOnly="true" Style="text-align:center;" BackColor="#DDDDDD" ToolTip="#,##0.00"></asp:TextBox>        
                </td>  
                <td >
                    <div>
                        <span>บัตรประชาชน:</span>
                    </div>
                </td>
                <td >
                    <asp:TextBox ID="card_person" runat="server" ReadOnly="true" Style="text-align:center" BackColor="#DDDDDD" ></asp:TextBox>                   
                </td>                                                                                                 
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>