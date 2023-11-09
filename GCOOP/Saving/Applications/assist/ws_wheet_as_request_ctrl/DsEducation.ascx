<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsEducation.ascx.cs" Inherits="Saving.Applications.assist.ws_wheet_as_request_ctrl.DsEducation" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />

<script language="JavaScript">
    function chkNumber(ele)
    {
        var vchar = String.fromCharCode(event.keyCode);
        if ((vchar < '0' || vchar > '9') && (vchar != '.')) return false;
        ele.onKeyPress=vchar;
    }
</script>


<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table class="DataSourceFormView">            
            <tr>
                <td>
                    <div>
                        <span>วันที่ใบคำขอ:</span>
                    </div>
                </td>
                <td>
                    <asp:TextBox ID="req_date" runat="server" Style="text-align:center;" ></asp:TextBox>                            
                </td>
                <td width="15%">
                    <div>
                        <span>เลขที่ใบคำขอ:</span>
                    </div>
                </td>
                <td>
                    <asp:TextBox ID="assist_docno" runat="server" ReadOnly="true" BackColor="#DDDDDD" ></asp:TextBox>                   
                </td>
                <td width="18%">
                    <div>
                        <span>สถานะใบคำขอ:</span>
                    </div>
                </td>
                <td>
                   <asp:DropDownList ID="req_status" runat="server" >
                            <asp:ListItem Value="8" >รออนุมัติ</asp:ListItem>
                            <asp:ListItem Value="1">อนุมัติ</asp:ListItem>
                   </asp:DropDownList>               
                </td>                              
            </tr>
            <tr>
                <td>
                    <span>คำนำหน้าบุตร:</span>
                </td>
                <td>
                    <asp:DropDownList ID="child_prename_code" runat="server" Style="text-align:center;">
                      
                    </asp:DropDownList>            
                </td>                 
                <td >
                    <div>
                        <span>ชื่อบุตร:</span>
                    </div>
                </td>
                <td >
                    <asp:TextBox ID="child_name" runat="server" ></asp:TextBox>        
                </td>
                <td >
                    <div>
                        <span>สกุลบุตร:</span>
                    </div>
                </td>
                <td >           
                    <asp:TextBox ID="child_surname" runat="server" ></asp:TextBox>  
                </td>                                 
            <tr>   
                <td >
                    <div>
                        <span>วันเกิดบุตร:</span>
                    </div>
                </td>
                <td>
                    <asp:TextBox ID="child_birth_date" runat="server"  Style="text-align:center;"></asp:TextBox>                     
                </td>              
                <td>
                    <div>
                        <span>อายุบุตร:</span>
                    </div>
                </td>
                <td>
                    <asp:TextBox ID="child_age" runat="server" Style="width:100px;text-align:center;" ReadOnly="true" BackColor="#DDDDDD"></asp:TextBox>
                     <div>
                        <span style="width:20px;text-align:center">ปี</span>  
                     </div>       
                </td>     
                 <td>
                    <div>
                        <span>เลขที่บัตรประชาชนบุตร:</span>
                    </div>
                </td>
                <td>
                    <div ><asp:TextBox ID="child_card_person" runat="server"></asp:TextBox></div>          
                </td>                                
            </tr>
             <tr>                                                  
                <td>
                    <div>
                        <span>บุตรกำลังศึกษา:</span>
                    </div>
                </td>
                <td>
                    <div ><asp:TextBox ID="child_no_study" runat="server" ></asp:TextBox></div>             
                </td>  
                <td>
                    <div>
                        <span>บุตรทำงานแล้ว:</span>
                    </div>
                </td>
                <td>
                    <div ><asp:TextBox ID="child_no_work" runat="server" ></asp:TextBox></div>      
                </td> 
                <td>
                    <div>
                        <span>จำนวนบุตรทั้งหมด:</span>
                    </div>
                </td>
                <td>
                    <div ><asp:TextBox ID="child_no" runat="server" ></asp:TextBox></div>          
                </td>                                
            </tr>
              <tr>                                                  
                <td>
                    <div>
                        <span>ชื่อสถานศึกษาบุตร:</span>
                    </div>
                </td>
                <td>
                    <div ><asp:TextBox ID="child_school" runat="server" ></asp:TextBox></div>             
                </td>  
                <td>
                    <div>
                        <span>ระดับชั้นบุตร:</span>
                    </div>
                </td>
                <td>
                    <asp:DropDownList ID="child_level" runat="server">
                    </asp:DropDownList>      
                </td> 
                <td>
                    <div>
                        <span>เกรดเฉลี่ยบุตร:</span>
                    </div>
                </td>
                <td>
                    <div ><asp:TextBox ID="child_gpa" runat="server" ToolTip = "#.00" OnKeyPress="return chkNumber(this)"></asp:TextBox></div>          
                </td>                                
            </tr>

        </table>
    </EditItemTemplate>
</asp:FormView>
