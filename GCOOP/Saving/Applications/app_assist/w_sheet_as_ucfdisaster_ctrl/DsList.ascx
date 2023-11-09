<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsList.ascx.cs" Inherits="Saving.Applications.app_assist.w_sheet_as_ucfdisaster_ctrl.DsList" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />

<table class="DataSourceRepeater">
    <tr>
        <th width="45%">
            มูลค่าทรัพย์สินที่เสียหาย (บาท)
        </th>
        <th width="10%">
            จำนวนเงินสวัสดิการ
        </th>
        <th width="10%">
            อายุสมาชิก (เดือน)
        </th>
          <th width="10%">
            ถึงอายุสมาชิก (เดือน)
        </th>
          <th width="10%">
            รหัสทำรายการ
        </th>
         <th width="10%">
            คู่บัญชี
        </th>
         <th width="5%">
            ลบ
        </th>
    </tr>
    <tbody>
        <asp:Repeater ID="Repeater1" runat="server">
            <ItemTemplate>
                <tr>
                    <td width="45%">
                        <asp:TextBox ID="envdesc" runat="server"></asp:TextBox>
                    </td>
                    <td width="10%">
                        <asp:TextBox ID="envvalue" runat="server" Style="text-align: right;" ToolTip="#,##0.00"></asp:TextBox>
                    </td>
                     <td width="10%">
                        <asp:TextBox ID="start_age" runat="server" Style="text-align: right;" ToolTip="#,##0"></asp:TextBox>
                    </td>
                    <td width="10%">
                        <asp:TextBox ID="end_age" runat="server" Style="text-align: right;" ToolTip="#,##0"></asp:TextBox>
                    </td>
                    <td width="10%">
                        <asp:TextBox ID="system_code" runat="server" Style="text-align: center;"></asp:TextBox>
                    </td>
                    <td width="10%">
                       <asp:TextBox ID="tofrom_accid" runat="server" Style="text-align: center;"></asp:TextBox>
                    </td>
                   
                    <td width="5%">
                       <asp:Button ID="b_del" runat="server" Text="ลบ" />
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </tbody>
</table>
