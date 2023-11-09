<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsList.ascx.cs" Inherits="Saving.Applications.assist.ws_wheet_as_ucfassisttypedet_ctrl.DsList" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />

<table class="DataSourceRepeater">    
    <tr>  
        <th width="10%">
            ประเภทสมาชิก
        </th>        
        <th  width="10%">
            ประเภทการจ่าย
        </th>
        <th  width="7%">
            เงื่อนไขต่ำสุด
        </th>
        <th  width="7%">
            เงื่อนไขสูงสุด
        </th> 
        <th width="5%">
            จำนวนครั้ง
        </th>  
        <th  width="10%">
            ยอดเงินที่จ่าย
        </th>                 
        <th width="8%">
            จ่ายครั้งแรก
        </th>
        <th  width="6%">
            หน่วย
        </th>
        <th width="7%">
           จ่ายครั้งแรกสูงสุด
        </th>
        <th width="8%">
           การจ่ายครั้งต่อไป
        </th>
        <th  width="6%">
            หน่วย
        </th> 
        <th width="7%">
           จ่ายครั้งต่อไปสูงสุด
        </th>
        <th width="5%">
            ลบ!
        </th>
    </tr>
    <asp:Repeater ID="Repeater1" runat="server">
        <ItemTemplate>
            <tr>
                <td>
                    <asp:DropDownList ID="membtype_code" runat="server" >
                       <%-- <asp:ListItem Value="1" >สามัญ</asp:ListItem>
                        <asp:ListItem Value="2">สมทบ</asp:ListItem>--%>
                    </asp:DropDownList>        
                </td>
                <td>
                    <asp:DropDownList ID="assisttype_pay" runat="server">
                    </asp:DropDownList>                   
                </td> 
                <td>
                    <asp:TextBox ID="MIN_CHECK" runat="server" Style="text-align: right;" OnKeyPess="return chkNumber(this)"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="MAX_CHECK" runat="server" Style="text-align: right;" OnKeyPess="return chkNumber(this)"></asp:TextBox>
                </td>   
                <td>
                    <asp:TextBox ID="NUM_PAY" runat="server" Style="text-align: right;" OnKeyPess="return chkNumber(this)"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="MAX_PAYAMT" runat="server" Style="text-align: right;" ToolTip="#,##0.00" OnKeyPess="return chkNumber(this)"></asp:TextBox>
                </td> 
                <td>
                    <asp:TextBox ID="first_payamt" runat="server" Style="text-align: right;" ToolTip="#,##0.00" OnKeyPess="return chkNumber(this)"></asp:TextBox>
                </td> 
                <td>
                    <asp:DropDownList ID="first_typepay" runat="server" >
                        <asp:ListItem Value="1">บาท</asp:ListItem>
                        <asp:ListItem Value="2">เปอร์เซ็นต์</asp:ListItem>
                    </asp:DropDownList>        
                </td> 
                <td>
                    <asp:TextBox ID="max_firstpayamt" runat="server" Style="text-align: right;" ToolTip="#,##0.00"></asp:TextBox>
                </td>                
                <td>
                    <asp:TextBox ID="NEXT_PAYAMT" runat="server" Style="text-align: right;" ToolTip="#,##0.00" OnKeyPess="return chkNumber(this)"></asp:TextBox>
                </td>    
                <td>
                    <asp:DropDownList ID="next_typepay" runat="server" >
                        <asp:ListItem Value="1">บาท</asp:ListItem>
                        <asp:ListItem Value="2">เปอร์เซ็นต์</asp:ListItem>
                    </asp:DropDownList>        
                </td>  
                 <td>
                    <asp:TextBox ID="max_nextpayamt" runat="server" Style="text-align: right;" ToolTip="#,##0.00"></asp:TextBox>
                </td>       
                <td>
                    <asp:Button ID="b_del" runat="server" Text="ลบ" />
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
