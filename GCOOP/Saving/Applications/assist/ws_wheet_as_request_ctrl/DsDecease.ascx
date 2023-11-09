<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsDecease.ascx.cs" Inherits="Saving.Applications.assist.ws_wheet_as_request_ctrl.DsDecease" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table class="DataSourceFormView">            
            <tr>
                <td width="15%">
                    <div>
                        <span>วันที่ใบคำขอ:</span>
                    </div>
                </td>
                <td width="15%">
                    <asp:TextBox ID="req_date" runat="server" Style="text-align:center;" ></asp:TextBox>                            
                </td>
                <td width="15%">
                    <div>
                        <span>เลขที่ใบคำขอ:</span>
                    </div>
                </td>
                <td width="15%">
                    <asp:TextBox ID="assist_docno" runat="server" ReadOnly="true" BackColor="#DDDDDD" ></asp:TextBox>                   
                </td>
                <td width="15%">
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
                    <span>วันที่ถึงแก่กรรม:</span>
                </td>
                <td>
                    <asp:TextBox ID="dead_date" runat="server" Style="text-align:center;"></asp:TextBox>                   
                </td>                                                     
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>
