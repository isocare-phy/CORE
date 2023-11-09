<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsList.ascx.cs" Inherits="Saving.Applications.mis.w_sheet_pm_estsumday_ctrl.DsList" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<link id="css2" runat="server" href="../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<table class="DataSourceRepeater">
    <tr>
        <th width="13%">
            เลขทะเบียนเงินลงทุน
        </th>
        <th >
            ประเภทเงินลงทุน
        </th>
        <th width="15%">
            รหัสเงินลงทุน
        </th>
        <th width="15%">
            จำนวนเงิน
        </th>
        <th width="12%">
            วันที่ทำรายการล่าสุด
        </th>
    </tr>
    <tbody>
        <asp:Repeater ID="Repeater1" runat="server">
            <ItemTemplate>
                <tr>
                    <td>
                        <asp:TextBox ID="account_no" runat="server" Style="text-align: center"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="investtype_desc" runat="server" Style="text-align: left;" ></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="symbol_code" runat="server" Style="text-align: center"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="prncbal" runat="server" Style="text-align: center" ToolTip="#,##0"></asp:TextBox>
                    </td>
                     <td>
                        <asp:TextBox ID="lastcalint_date" runat="server" Style="text-align: center" ></asp:TextBox>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </tbody>
</table>
