<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsGain.ascx.cs" Inherits="Saving.Applications.assist.dlg.ws_dlg_as_assistpay_ctrl.DsGain" %>
<link id="css1" runat="server" href="../../../../JsCss/DataSourceTool.css" rel="stylesheet"    type="text/css" />


<table class="DataSourceRepeater">
    <tr align="center">  
        <th width="50%">
            <span>ชื่อ -สกุล</span>
        </th>   
        <th width="45%">
            <span>จำนวนเงิน</span>
        </th>
        <th width="5%">
            
        </th>
    </tr>
   
    <asp:Repeater ID="Repeater1" runat="server">
        <ItemTemplate>    
            <tr>
                <td width="50%">
                    <asp:TextBox ID="gain_fullname" runat="server" ></asp:TextBox>
                </td>
                <td width="45%">
                    <asp:TextBox ID="assist_amt" runat="server" Style="text-align: right; font-size: 11px;" ToolTip="#,##0.00" ></asp:TextBox>
                </td>
                <td width="5%" align="center">
                    <asp:Button ID="b_del" runat="server" Text="-" />
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
