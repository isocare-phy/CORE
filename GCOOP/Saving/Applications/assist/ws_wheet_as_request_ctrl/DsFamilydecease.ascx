<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsFamilydecease.ascx.cs" Inherits="Saving.Applications.assist.ws_wheet_as_request_ctrl.DsFamilydecease" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table class="DataSourceFormView">            
            <tr>
                <td width="20%">
                    <div>
                        <span>วันที่ใบคำขอ:</span>
                    </div>
                </td>
                <td width="17%">
                    <asp:TextBox ID="req_date" runat="server" Style="text-align:center;" ></asp:TextBox>                            
                </td>
                <td width="17%">
                    <div>
                        <span>เลขที่ใบคำขอ:</span>
                    </div>
                </td>
                <td width="20%">
                    <asp:TextBox ID="assist_docno" runat="server" ReadOnly="true" BackColor="#DDDDDD" ></asp:TextBox>                   
                </td>
                <td width="14%">
                    <div>
                        <span>สถานะใบคำขอ:</span>
                    </div>
                </td>
                <td width="15%">
                   <asp:DropDownList ID="req_status" runat="server" >
                            <asp:ListItem Value="8" >รออนุมัติ</asp:ListItem>
                            <asp:ListItem Value="1">อนุมัติ</asp:ListItem>
                   </asp:DropDownList>               
                </td>                              
            </tr>
            <tr>
                <td>
                    <span>วันที่ตามเอกสารราชการ:</span>
                </td>
                <td>
                    <asp:TextBox ID="dead_date" runat="server" Style="text-align:center;"></asp:TextBox>                   
                </td>                 
                <td >
                    <div>
                        <span>ชื่อสมาชิกครอบครัว:</span>
                    </div>
                </td>
                <td >
                    <asp:TextBox ID="familydead_name" runat="server" ></asp:TextBox>                   
                </td>                
                <td >
                    <div>
                        <span>เกี่ยวข้องเป็น:</span>
                    </div>
                </td>
                <td>
                    <asp:DropDownList ID="assistpay_code" runat="server" >
                    </asp:DropDownList>
                </td>                                           
            </tr>
            <tr>
                <td>
                    <span>บัตรประชาชน:</span>
                </td>
                <td >
                    <asp:TextBox ID="family_card_person" runat="server" ></asp:TextBox>                   
                </td>
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>
