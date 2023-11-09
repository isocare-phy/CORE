<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsKeep.ascx.cs" Inherits="Saving.Applications.mbshr.ws_mbshr_adt_mbmoney_ctrl.DsKeep" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView3" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table class="DataSourceFormView" style="width:240px;">  
            <tr>
                <td colspan="4">
                    <u><b>บัญชีเรียกเก็บ:</b></u>
                </td>
            </tr>          
            <tr>                
                <td>
                    <div>
                        <span style="font-size: 12px;">รายการ:</span>
                    </div>
                </td>
                <td>
                    <asp:DropDownList ID="moneytype_code" runat="server" Style="width:140px;font-size: 12px;">
                        <asp:ListItem Value=""></asp:ListItem>
                        <asp:ListItem Value="CSH">CSH - เงิดสด</asp:ListItem>
                        <asp:ListItem Value="CBT">CBT - บัญชีธนาคาร</asp:ListItem>
                        <asp:ListItem Value="TRN">TRN - บัญชีเงินฝากสหกรณ์</asp:ListItem> 
                        <asp:ListItem Value="SAL">SAL - บัญชีเงินเดือน</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span style="font-size: 12px;">ธนาคาร:</span>
                    </div>
                </td>
                <td>
                    <asp:TextBox ID="bank_code" runat="server" Style="width: 30px; font-size: 12px;
                        text-align: center;"></asp:TextBox>
                    <asp:DropDownList ID="bank_name" runat="server" Style="width: 100px; font-size: 12px;">
                    </asp:DropDownList>
                    <asp:Button ID="b_bank" runat="server" Text="..." Style="width: 15px;" />
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span style="font-size: 12px;">สาขา:</span>
                    </div>
                </td>
                <td>
                    <asp:TextBox ID="bank_branch" runat="server" Style="width: 30px; font-size: 12px;
                        text-align: center;"></asp:TextBox>
                    <asp:DropDownList ID="branch_name" runat="server" Style="width: 100px; font-size: 12px;">
                    </asp:DropDownList>
                    <asp:Button ID="b_branch" runat="server" Text=".." Style="width: 15px;" />
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span style="font-size: 12px;">เลขที่บัญชี:</span>
                    </div>
                </td>
                <td>
                    <asp:TextBox ID="bank_accid" runat="server" Style="width:140px;font-size: 12px; text-align: center;"></asp:TextBox>
                </td>
            </tr>
        </table>   
    </EditItemTemplate>
</asp:FormView>
