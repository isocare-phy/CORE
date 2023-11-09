<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsList.ascx.cs" Inherits="Saving.Applications.ap_deposit.w_sheet_dp_approve_chgdept_ctrl.DsList" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<table class="DataSourceRepeater" style="width: 770px;">
    <tr>
        <th width="5%">
        </th>
        <th  width="20%">
            ใบคำร้อง
        </th>
        <th  width="20%">
            เลขที่บัญชี
        </th>
        <th  width="20%">
            ยอดฝากรายเดือนเดิม
        </th>
        <th  width="20%">
            ยอดฝากรายเดือนใหม่       
        </th>
        <th  width="15%">
            สถานะ
        </th>
    </tr>
    <asp:Repeater ID="Repeater1" runat="server">
        <ItemTemplate>
            <tr>
                <td style="text-align:center;">
                    <asp:CheckBox ID="choose_flag" runat="server"  />
                </td>
                <td>
                    <asp:TextBox ID="DPREQCHG_DOC" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="DEPTACCOUNT_NO" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="DEPTMONTH_OLDAMT" runat="server" ReadOnly="true" ToolTip="#,##0.00" style="text-align:right;"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="DEPTMONTH_NEWAMT" runat="server" ReadOnly="true" ToolTip="#,##0.00" style="text-align:right;"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="CHANGE_DESC" runat="server" style="text-align:center;"></asp:TextBox>
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
