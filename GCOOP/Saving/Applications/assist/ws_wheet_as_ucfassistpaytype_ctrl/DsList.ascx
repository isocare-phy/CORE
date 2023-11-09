<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsList.ascx.cs" Inherits="Saving.Applications.assist.ws_wheet_as_ucfassistpaytype_ctrl.DsList" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<table class="DataSourceRepeater">
    <tr>
        <th width="10%">
            กลุ่มสวัสดิการ
        </th>
        <th width="10%">
            รหัส
        </th>
        <th>
            คำอธิบาย
        </th>        
        <th width="5%">
            ลบ!
        </th>
    </tr>
    <asp:Repeater ID="Repeater1" runat="server">
        <ItemTemplate>
            <tr>
                <td>
                    <asp:DropDownList ID="ASSISTTYPE_GROUP" runat="server"></asp:DropDownList>
                </td>
                <td>
                    <asp:TextBox ID="ASSISTPAY_CODE" runat="server" Style="text-align: center;" MaxLength="2" ReadOnly="true"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="ASSISTPAY_DESC" runat="server"></asp:TextBox>
                </td>                
                <td>
                    <asp:Button ID="b_del" runat="server" Text="ลบ" />
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
