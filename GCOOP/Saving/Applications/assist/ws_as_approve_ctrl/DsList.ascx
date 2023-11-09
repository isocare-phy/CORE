<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsList.ascx.cs" Inherits="Saving.Applications.assist.ws_as_approve_ctrl.DsList" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<table class="DataSourceRepeater">
    <tr>
        <th width="4%">
        </th>
        <th width="30%">
            ชื่อ - สกุล
        </th>
        <th width="32%">
            ประเภท
        </th>
        <th width="10%">
            วันที่ขอ
        </th>
        <th width="12%">
            เงินสวัสดิการ
        </th>
        <th width="12%">
            สถานะ
        </th>
    </tr>
</table>
<asp:Panel ID="Panel1" runat="server" Height="750px" ScrollBars="Auto">
    <table class="DataSourceRepeater">
        <asp:Repeater ID="Repeater1" runat="server">
            <ItemTemplate>
                <tr>
                    <td width="4%" align="center">
                        <asp:CheckBox ID="choose_flag" runat="server" />
                    </td>
                    <td width="30%">
                        <asp:TextBox ID="cp_name" runat="server" ReadOnly="true"></asp:TextBox>
                    </td>
                    <td width="32%">
                        <asp:TextBox ID="assistpay_desc" runat="server" Style="text-align: center" ReadOnly="true"></asp:TextBox>
                    </td>
                    <td width="10%">
                        <asp:TextBox ID="req_date" runat="server" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                    </td>
                    <td width="12%">
                        <asp:TextBox ID="assistnet_amt" runat="server" Style="text-align: right" ToolTip="#,##0.00"
                            ReadOnly="true"></asp:TextBox>
                    </td>
                    <td width="12%">
                        <asp:DropDownList ID="req_status" runat="server">
                            <asp:ListItem Value="8" Text="รออนุมัติ"></asp:ListItem>
                            <asp:ListItem Value="1" Text="อนุมัติ"></asp:ListItem>
                            <asp:ListItem Value="0" Text="ไม่อนุมัติ"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td colspan="5">
                        <asp:TextBox ID="ass_rcvname" runat="server" ReadOnly="true"></asp:TextBox>
                    </td>
                </tr>

            </ItemTemplate>
        </asp:Repeater>
        </table>

   <table class="DataSourceRepeater">
        <tr>
        <td width="4%">

        </td>
        <td  width="30%" style="text-align: right">
            รวมจำนวนคำขอ :
        </td>
        <td width="32%" style="text-align: center">
            <label id="sum_req"  runat="server" ></label>
        </td>
         <td width="10%" style="text-align: right">
            รวมเงิน :
        </td>
        <td width="12%" style="text-align: right">
            <label id="sum_balance"  runat="server" ></label>
        </td>
        <td width="12%">

        </td>
    </tr>
    </table>
  </asp:Panel>

