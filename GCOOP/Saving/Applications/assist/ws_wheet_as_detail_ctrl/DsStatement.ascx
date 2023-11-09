<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsStatement.ascx.cs" Inherits="Saving.Applications.assist.ws_wheet_as_detail_ctrl.DsStatement" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<table class="DataSourceRepeater">    
    <tr>  
        <th width="5%">
            ลำดับ
        </th>        
        <th  width="10%">
            วันที่จ่าย
        </th>
        <th  width="7%">
            ประเภทเงิน
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
                    <asp:TextBox ID="seq_no" runat="server" Style="text-align: center;"></asp:TextBox>       
                </td>
                <td>
                    <asp:TextBox ID="slip_date" runat="server" Style="text-align: center;"></asp:TextBox>                 
                </td> 
                <td>
                    <asp:TextBox ID="moneytype_desc" runat="server" Style="text-align: center;"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="pay_balance" runat="server" Style="text-align: right;" ToolTip="#,##0.00"></asp:TextBox>
                </td>   
                <td>
                    <asp:TextBox ID="entry_id" runat="server" Style="text-align: center;" ></asp:TextBox>
                </td>               
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
