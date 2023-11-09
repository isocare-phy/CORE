<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsDisaster.ascx.cs" Inherits="Saving.Applications.assist.ws_wheet_as_request_ctrl.DsDisaster" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />

<script type="text/javascript">
    function chkNumber(ele) {
        var vchar = String.fromCharCode(event.keyCode);
        if ((vchar < '0' || vchar > '9') && (vchar != '.')) return false;
        ele.onKeyPress = vchar;
    }
</script>

<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table class="DataSourceFormView">
            <tr>
                <td  width="20%">
                    <div>
                        <span>วันที่ใบคำขอ:</span>
                    </div>
                </td>
                <td width="15%">
                    <asp:TextBox ID="req_date" runat="server" Style="text-align:center;" ></asp:TextBox>                            
                </td>
                <td width="20%">
                    <div>
                        <span>เลขที่ใบคำขอ:</span>
                    </div>
                </td>
                <td >
                    <asp:TextBox ID="assist_docno" runat="server" ReadOnly="true" BackColor="#DDDDDD" ></asp:TextBox>                   
                </td>
                <td width="10%">
                    <div>
                        <span>สถานะใบคำขอ:</span>
                    </div>
                </td>
                <td width="20%">
                    <asp:DropDownList ID="req_status" runat="server" >
                            <asp:ListItem Value="8" >รออนุมัติ</asp:ListItem>
                            <asp:ListItem Value="1">อนุมัติ</asp:ListItem>
                    </asp:DropDownList>               
                </td>                              
            </tr>
            <tr>
                <td>
                    <span>วันที่เริ่มประสบภัย</span>
                </td>
                <td>
                    <asp:TextBox ID="disaster_date" runat="server" Style="text-align:center;"></asp:TextBox>        
                </td>  
                <td>
                    <span>ประเภทภัพพิบัติ</span>
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="disaster_type" runat="server" >
                    </asp:DropDownList>     
                </td>                                                                                                           
            </tr>
        </table>
        <table>
            <tr>
                <td>
                    <table class="DataSourceFormView" style="width: 350px;">                        
                        <tr>
                            <td colspan="4">
                                <u><b>ที่อยู่ตามทะเบียนบ้าน:</b></u>
                            </td>
                        </tr>
                        <tr>
                            <td width="20%">
                                <div>
                                    <span style="font-size: 12px;">ที่อยู่:</span>
                                </div>
                            </td>
                            <td width="30%" colspan="3">
                                <asp:TextBox ID="H_MEMB_ADDR" runat="server" Style="font-size: 12px; text-align: center;"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td width="20%">
                                <div>
                                    <span style="font-size: 12px;">หมู่:</span>
                                </div>
                            </td>
                            <td width="30%">
                                <asp:TextBox ID="H_MOO" runat="server" Style="font-size: 12px; text-align: center;"></asp:TextBox>
                            </td>
                            <td>
                                <div>
                                    <span style="font-size: 12px;">ซอย:</span>
                                </div>
                            </td>
                            <td>
                                <asp:TextBox ID="H_SOI" runat="server" Style="font-size: 12px; text-align: left;"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div>
                                    <span style="font-size: 12px;">หมู่บ้าน:</span>
                                </div>
                            </td>
                            <td>
                                <asp:TextBox ID="H_village" runat="server" Style="font-size: 12px; text-align: left;"></asp:TextBox>
                            </td>
                            <td>
                                <div>
                                    <span style="font-size: 12px;">ถนน:</span>
                                </div>
                            </td>
                            <td>
                                <asp:TextBox ID="H_ROAD" runat="server" Style="font-size: 12px; text-align: left;"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div>
                                    <span style="font-size: 12px;">จังหวัด:</span>
                                </div>
                            </td>
                            <td>
                                <asp:DropDownList ID="H_PROVINCE_CODE" runat="server" Style="font-size: 12px;">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <div>
                                    <span style="font-size: 12px;">เขต/อำเภอ:</span>
                                </div>
                            </td>
                            <td>
                                <asp:DropDownList ID="H_DISTRICT_CODE" runat="server" Style="font-size: 12px;">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div>
                                    <span style="font-size: 12px;">แขวง/ตำบล:</span>
                                </div>
                            </td>
                            <td>
                                <asp:DropDownList ID="H_TAMBOL_CODE" runat="server" Style="font-size: 12px;">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <div>
                                    <span style="font-size: 12px;">รหัสไปรณีย์:</span>
                                </div>
                            </td>
                            <td>
                                <asp:TextBox ID="H_POSTCODE" runat="server" Style="font-size: 12px; text-align: center;"></asp:TextBox>
                            </td>                           
                        </tr>                                               
                 </table> 
              </td>
              <td>
                <table class="DataSourceFormView" style="width: 380px;">                                               
                        <tr>
                            <td colspan="4">
                                <u><b>ที่อยู่ที่ประสบภัย:</b></u>
                            </td>
                        </tr>
                        <tr>
                            <td width="20%">
                                <div>
                                    <span style="font-size: 12px;">ที่อยู่:</span>
                                </div>
                            </td>
                            <td width="30%" colspan="3">
                                <asp:TextBox ID="MEMB_ADDR" runat="server" Style="font-size: 12px; text-align: center;"></asp:TextBox>
                            </td>  
                        </tr>
                        <tr>                                                 
                            <td>
                                <div>
                                    <span style="font-size: 12px;">ซอย:</span>
                                </div>
                            </td>
                            <td>
                                <asp:TextBox ID="SOI" runat="server" Style="font-size: 12px; text-align: left;"></asp:TextBox>
                            </td>                            
                            <td>
                                <div>
                                    <span style="font-size: 12px;">ถนน:</span>
                                </div>
                            </td>
                            <td>
                                <asp:TextBox ID="ROAD" runat="server" Style="font-size: 12px; text-align: left;"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>                        
                            <td>
                                <div>
                                    <span style="font-size: 12px;">จังหวัด:</span>
                                </div>
                            </td>
                            <td>
                                <asp:DropDownList ID="PROVINCE_CODE" runat="server" Style="font-size: 12px;">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <div>
                                    <span style="font-size: 12px;">เขต/อำเภอ:</span>
                                </div>
                            </td>
                            <td>
                                <asp:DropDownList ID="DISTRICT_CODE" runat="server" Style="font-size: 12px;">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div>
                                    <span style="font-size: 12px;">แขวง/ตำบล:</span>
                                </div>
                            </td>
                            <td>
                                <asp:DropDownList ID="TAMBOL_CODE" runat="server" Style="font-size: 12px;">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <div>
                                    <span style="font-size: 12px;">รหัสไปรณีย์:</span>
                                </div>
                            </td>
                            <td>
                                <asp:TextBox ID="POSTCODE" runat="server" Style="font-size: 12px; text-align: center;"></asp:TextBox>
                            </td> 
                        </tr>
                        <tr>
                            <td>
                                <span style="font-size: 12px;">สถานที่ประสบภัย:</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="type_preple_add" runat="server" >
                                    <asp:ListItem Value="1" >บ้านตนเอง</asp:ListItem>
                                    <asp:ListItem Value="2">บ้านคู่สมรส</asp:ListItem>
                                    <asp:ListItem Value="3" >บ้านพักราชการ</asp:ListItem>
                                    <asp:ListItem Value="4">ผู้อยู่อาศัย</asp:ListItem>
                    </asp:DropDownList>
                            </td>
                            
                        </tr>
                        <tr>
                             <td colspan="4">
                                <asp:Button ID="b_linkaddress" Text="ที่อยู่ตามทะเบียนบ้าน -> ที่อยู่ที่ประสบภัย" runat="server"/>
                            </td>
                        </tr>                                             
                 </table>                    
              </td>
            </tr>
        </table>
        <table class="DataSourceFormView">
            <tr>
                <td width="20px">
                    <div>
                        <span style="font-size: 12px;">ประเมินค่าเสียหาย:</span>
                    </div>
                </td>
                <td width="50px">              
                    <asp:TextBox ID="damages_amt" runat="server" ToolTip="#,##0.00" Style="font-size:12px;text-align:right;height:20px;" OnKeyPress="return chkNumber(this)"></asp:TextBox>                   
                </td> 
                <td width="100px"></td>
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>
