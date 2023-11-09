<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsPayto.ascx.cs" Inherits="Saving.Applications.assist.dlg.ws_dlg_as_assistpay_v2_ctrl.DsPayto" %>
<link id="css1" runat="server" href="../../../../JsCss/DataSourceTool.css" rel="stylesheet"    type="text/css" />


<table class="DataSourceFormView">

<tr>  
        <td colspan="4">
            <strong style="font-size: 13px;">จ่ายโดย</strong>
        </td> 
    </tr>
</table>
<table class="DataSourceRepeater">
    
    <tr align="center">
        <th width="20%">
            <span>ประเภทเงิน</span>
        </th>   
        <th width="52%" colspan="2">
            <span>รายละเอียด</span>
        </th>
        <th width="25%">
            <span>จำนวนเงิน</span>
        </th>
        <th width="3%">
            
        </th>
    </tr>
   
    <asp:Repeater ID="Repeater1" runat="server">
        <ItemTemplate>    
            <tr>
                <td>
                    <asp:DropDownList ID="moneytype_code" runat="server" >
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:TextBox ID="payto_detail" runat="server" ReadOnly="true" Style="background-color: #CCCCCC;"></asp:TextBox>
                </td>
                <td width="10px">
                    <asp:Button ID="b_searchbank" runat="server" Text="..." />
                </td>
                <td>
                    <asp:TextBox ID="assist_amt" runat="server" Style="text-align: right; font-size: 11px;" ToolTip="#,##0.00" ></asp:TextBox>
                </td>
                <td align="center">
                    <asp:Button ID="b_delpayto" runat="server" Text="-" />
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>