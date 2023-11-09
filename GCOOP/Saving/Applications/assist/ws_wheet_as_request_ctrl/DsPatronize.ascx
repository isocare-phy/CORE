<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsPatronize.ascx.cs" Inherits="Saving.Applications.assist.ws_wheet_as_request_ctrl.DsPatronize" %>
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
                <td width="13%">
                    <asp:TextBox ID="req_date" runat="server" Style="text-align:center;" ></asp:TextBox>                            
                </td>
                <td width="10%" colspan="2">
                    <div>
                        <span>เลขที่ใบคำขอ:</span>
                    </div>
                </td>
                <td width="15%" colspan="3">
                    <asp:TextBox ID="assist_docno" runat="server" ReadOnly="true" BackColor="#DDDDDD" ></asp:TextBox>                   
                </td>
                <td width="10%">
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
                    <div>
                        <span id="txt_date">วันที่รักษา:</span>
                    </div>
                </td>
                <td>
                    <asp:TextBox ID="start_treat" runat="server" Style="text-align:center;"></asp:TextBox>
                </td>
                <td width="1%">
                    <span id="txt_to" style="text-align:center; ">-</span>
                    <%--<asp:TextBox ID="TextBox1" runat="server" Style="text-align:center;" BackColor="#DDDDDD" ReadOnly="true">-</asp:TextBox>--%>
                </td>
                <td width="10%" colspan="2">
                    <asp:TextBox ID="end_treat" runat="server" Style="text-align:center; margin-left:3px; "></asp:TextBox>
                </td>
                <td width="4%">
                    <asp:TextBox ID="cal_date" runat="server" Style="text-align:center;" ReadOnly="true" BackColor="#DDDDDD"></asp:TextBox>
                </td>
                <td width="5%">
                    <div>
                        <span id="txt_day" style="text-align:center;">วัน</span>
                    </div>
                </td>
                <td >
                    <div>
                        <span>ประเภทการจ่าย:</span>
                    </div>
                </td>
                <td>
                   <asp:DropDownList ID="senile_type" runat="server" >
                   </asp:DropDownList>               
                </td> 
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>
