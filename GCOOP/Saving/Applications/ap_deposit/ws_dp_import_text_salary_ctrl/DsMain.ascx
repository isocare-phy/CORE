<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsMain.ascx.cs" Inherits="Saving.Applications.ap_deposit.ws_dp_import_text_salary_ctrl.DsMain" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet" type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table class="DataSourceFormView">
            <tr>
                <td width="20%">
                    <div>
                        <span>วันที่โอนเงิน :</span>
                    </div>
                </td>
                <td width="25%">
                    <div>
                        <asp:TextBox ID="tran_date" runat="server" Style="width: 170px; text-align:center;"></asp:TextBox>
                    </div>
                </td>
                <td width="20%">
                    <div>
                        <span>ประเภทรายการ :</span>
                    </div>
                </td>
                <td width="30%">
                    <asp:DropDownList ID="system_code" runat="server">
                        <asp:ListItem Text="โอนเงินเดือน" Value="DTS"></asp:ListItem>
                        <asp:ListItem Text="โอนเงินจากกองคลัง" Value="TMP"></asp:ListItem>
                        <asp:ListItem Text="ฝากโอนจากเงินฝาก" Value="DTD"></asp:ListItem>
                        <asp:ListItem Text="ถอนเพื่อการโอนภายใน" Value="WTI"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    
                </td>
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>