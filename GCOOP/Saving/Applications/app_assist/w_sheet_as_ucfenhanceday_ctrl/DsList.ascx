<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsList.ascx.cs" Inherits="Saving.Applications.app_assist.w_sheet_as_ucfenhanceday_ctrl.DsList" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />

<table class="DataSourceRepeater">
    <tr>
        <th width="20%">
            ประเภท
        </th>
        <th width="40%">
           รายละเอียด
        </th>
        <th width="10%">
           รหัส
        </th>
        <th width="10%">
            อายุการเป็นสมาชิก
        </th>
          <th width="10%">
            ระยะเวลาคำขอ
        </th>
        <%--  <th width="5%">
            ลบ
        </th>--%>
    </tr>
    <tbody>
        <asp:Repeater ID="Repeater1" runat="server">
            <ItemTemplate>
                <tr>
                    <td width="20%">
                        <asp:TextBox ID="ass_envgroup" runat="server"></asp:TextBox>
                    </td>
                    <td width="40%">
                        <asp:TextBox ID="ass_desc" runat="server" ></asp:TextBox>
                    </td>
                      <td width="10%">
                        <asp:TextBox ID="ass_code" runat="server" Style="text-align: center;"></asp:TextBox>
                    </td>
                     <td width="10%">
                        <asp:TextBox ID="ass_memb_req" runat="server" Style="text-align: center;" ToolTip="#,##0" ></asp:TextBox>
                    </td>
                    <td width="10%">
                        <asp:TextBox ID="ass_max_req" runat="server" Style="text-align: center;" ToolTip="#,##0"></asp:TextBox>
                    </td>
                   <%-- <td width="5%">
                        <asp:Button ID="b_del" runat="server" Text="ลบ" />
                    </td>--%>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </tbody>
</table>
