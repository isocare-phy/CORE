<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsList.ascx.cs" Inherits="Saving.Applications.mis.w_sheet_miscapitaltype_ctrl.DsList" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<table class="DataSourceRepeater">
    <tr>
        <th width="20%">
            หมวด
        </th>
        <th width="25%">
            เงินทุน
        </th>
        <th width="35%">
            รายละเอียด
        </th>
         <th width="10%">
            เรียงลำดับ
        </th>
        <th width="5%">
            ลบ
        </th>
    </tr>
    <tbody>
        <asp:Repeater ID="Repeater1" runat="server">
            <ItemTemplate>
                <tr>
                    <td >
                    <asp:DropDownList ID="CAPTYPE_CODE" runat="server" Style="background-color:#e7e7e7;"></asp:DropDownList>
                        <%--<asp:TextBox ID="CAPTYPE_CODE_DESC" runat="server" Style="text-align: center;" ></asp:TextBox>--%>
                    </td>
                    <td >
                        <asp:TextBox ID="system_code" runat="server" Style="text-align: center;background-color:#e7e7e7;" ></asp:TextBox>
                    </td>
                    <td ">
                        <asp:TextBox ID="description" runat="server" Style="text-align: center;"></asp:TextBox>
                    </td>
                    <td >
                        <asp:TextBox ID="seq_no" runat="server" Style="text-align: center;"></asp:TextBox>
                    </td>
                     <td align="center">
                        <asp:Button ID="b_del" runat="server" Text="ลบ" />
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </tbody>
</table>
