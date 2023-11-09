<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsList.ascx.cs" Inherits="Saving.Applications.hr.ws_hr_give_child_ctrl.DsList" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<table class="DataSourceRepeater">
    <tr>
        <th width="5%">
        ลำดับที่
        </th>
        <th width="35%">
            ชื่อบุตร
        </th>
        <th width="30%">
            เกิดวันที่
        </th>
        <th width="30%">
            จะมีอายุครบ 18 ปี วันที่
        </th>
        
    </tr>
</table>
<asp:Panel ID="Panel1" runat="server" Height="750px" ScrollBars="Auto">
    <table class="DataSourceRepeater">
        <asp:Repeater ID="Repeater1" runat="server">
            <ItemTemplate>
                <tr>
                    <td width="5%" align="center">
                        <asp:TextBox ID="running_number" Style="text-align: center" runat="server" ReadOnly></asp:TextBox>
                    </td>
                    <td width="35%">
                        <asp:TextBox ID="child_name" runat="server" ></asp:TextBox>
                    </td>
                    <td width="30%">
                        <asp:TextBox ID="birth_date" runat="server" Style="text-align: center" ></asp:TextBox>
                    </td>
                    <td width="30%">
                        <asp:TextBox ID="date2" runat="server" Style="text-align: center" ></asp:TextBox>
                    </td>
                  
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Panel>
