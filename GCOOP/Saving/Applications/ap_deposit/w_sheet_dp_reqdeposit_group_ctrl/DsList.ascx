<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsList.ascx.cs" Inherits="Saving.Applications.ap_deposit.w_sheet_dp_reqdeposit_group_ctrl.DsList" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<table class="DataSourceRepeater">
    <tr align="center">  
        <th width="3%">
        </th>
        <th width="10%">
            <span>เลขสมาชิก</span>
        </th>                 
        <th width="20%">
            <span>ชื่อ สกุล</span>
        </th>         
        <th width="10%">
            <span>จำนวนเงิน</span>
        </th>
    </tr>
</table>
<asp:Panel ID="Panel1" runat="server" Height="550px" ScrollBars="Auto">
    <table class="DataSourceRepeater">      
        <asp:Repeater ID="Repeater1" runat="server">
            <ItemTemplate>    
                <tr>
                    <td width="3%" align="center">
                        <asp:CheckBox ID="choose_flag" runat="server" />
                    </td>                                   
                    <td width="10%">
                        <asp:TextBox ID="member_no" runat="server" Style="text-align:center;" ReadOnly="true"></asp:TextBox>
                    </td>
                    <td width="20%">
                        <asp:TextBox ID="deptaccount_name" runat="server" Style="text-align:left;" ReadOnly="true"></asp:TextBox>
                    </td>                   
                    <td width="10%">
                        <asp:TextBox ID="deptrequest_amt" runat="server" Style="text-align:right;" ToolTip="#,##0.00" ReadOnly="true"></asp:TextBox>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Panel>
