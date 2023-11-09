<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsPayout.ascx.cs" Inherits="Saving.Applications.assist.ws_wheet_as_detail_ctrl.DsPayout" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<table class="DataSourceRepeater">    
    <tr>  
        <th width="5%">
            วันที่จ่าย
        </th>        
        <th  width="10%">
            เลขที่ทำรายการ
        </th>
        <th  width="7%">
            จำนวนเงิน
        </th> 
        <th width="5%">
            ผู้ทำรายการ
        </th>  
    </tr>
    <asp:Repeater ID="Repeater1" runat="server">
        <ItemTemplate>
            <tr>
                <td>
                    <asp:TextBox ID="slip_date" runat="server" Style="text-align: center;"></asp:TextBox>       
                </td>
                <td>
                    <asp:TextBox ID="ref_reqdocno" runat="server" Style="text-align: center;"></asp:TextBox>                 
                </td> 
                <td>
                    <asp:TextBox ID="payout_amt" runat="server" Style="text-align: right;" ToolTip="#,##0.00"></asp:TextBox>
                </td>   
                <td>
                    <asp:TextBox ID="entry_id" runat="server" Style="text-align: center;" ></asp:TextBox>
                </td>               
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
