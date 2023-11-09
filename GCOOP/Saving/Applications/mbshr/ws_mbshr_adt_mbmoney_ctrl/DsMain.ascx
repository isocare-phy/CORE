<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsMain.ascx.cs" Inherits="Saving.Applications.mbshr.ws_mbshr_adt_mbmoney_ctrl.DsMain" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>               
        <table class="DataSourceFormView">
            <tr>
                <td>
                    <div>
                        <span style="width:100px;font-size: 12px;">เลขสมาชิก:</span>
                    </div>
                </td>
                <td>
                    <asp:TextBox ID="member_no" runat="server" Style="width:100px;text-align: center;font-size: 12px;"></asp:TextBox>                    
                </td> 
                <td>
                    <asp:Button ID="b_search" runat="server" Text="..." Style="width:50px;margin-right: 0px;font-size: 12px;"/>          
                </td>
                <td>
                    <div>
                        <span style="width:120px;font-size: 12px;">ชื่อ - สกุล:</span>
                    </div>
                </td>
                    <td>
                    <asp:TextBox ID="fullname" runat="server" ReadOnly="true" Style="width:320px;font-size: 12px;" BackColor="DarkGray"></asp:TextBox>
                </td>
            </tr>
        </table>              
    </EditItemTemplate>
</asp:FormView>
