<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsList.ascx.cs" Inherits="Saving.Applications.assist.ws_sheet_as_genrequest_ctrl.DsList" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<table class="DataSourceRepeater">
    <tr align="center">  
        <th width="3%">
        </th>
        <th width="10%">
            <span>วันที่จ่ายล่าสุด</span>
        </th> 
         
        <th width="10%">
            <span>ทะเบียน</span>
        </th>
        <th width="20%">
            <span>ชื่อ สกุล</span>
        </th>
         
        <th width="8%">
            <span>จำนวนครั้งที่จ่าย</span>
        </th>
        <th width="5%">
            <span>วิธีจ่ายล่าสุด</span>
        </th>
        <th width="15%">
            <span>จำนวนเงินตามสิทธิ์</span>
        </th>
        <th width="10%">
            <span>จำนวนเงิน</span>
        </th>
         <th width="10%">
            <span>คงเหลือ</span>
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
                        <asp:TextBox ID="slip_date" runat="server" Style="text-align:center;" ReadOnly="true"></asp:TextBox>
                    </td>                   
                    <td width="10%">
                        <asp:TextBox ID="member_no" runat="server" Style="text-align:center;" ReadOnly="true"></asp:TextBox>
                    </td>
                    <td width="20%">
                        <asp:TextBox ID="full_name" runat="server" Style="text-align:left;" ReadOnly="true"></asp:TextBox>
                    </td>
                   
                    <td width="8%">
                        <asp:TextBox ID="last_periodpay" runat="server" Style="text-align:center;" ReadOnly="true"></asp:TextBox>
                    </td>
                    <td width="5%">
                        <asp:TextBox ID="moneytype_code" runat="server" Style="text-align:center;" ReadOnly="true"></asp:TextBox>
                    </td>
                    <td width="15%">
                        <asp:TextBox ID="max_payamt" runat="server" Style="text-align:right;" ToolTip="#,##0.00" ReadOnly="true"></asp:TextBox>
                    </td>
                    <td width="10%">
                        <asp:TextBox ID="approve_amt" runat="server" Style="text-align:right;" ToolTip="#,##0.00" ReadOnly="true"></asp:TextBox>
                    </td>
                    <td width="10%">
                        <asp:TextBox ID="prncbal" runat="server" Style="text-align:right;" ToolTip="#,##0.00" ReadOnly="true"></asp:TextBox>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Panel>
