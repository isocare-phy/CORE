﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsList.ascx.cs" Inherits="Saving.Applications.assist.dlg.wd_as_expense_detail_ctrl.DsList" %>
<link id="css1" runat="server" href="../../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table class="DataSourceFormView" style="width: 460px;">
            <tr>
                <td width="12%">
                    <div>
                        <span>ธนาคาร:</span>
                    </div>
                </td>
                <td width="18%">
                    <div>
                        <asp:DropDownList ID="bank_code" runat="server">
                        </asp:DropDownList>
                    </div>
                </td>
            </tr>
            <tr>
                <td width="12%">
                    <div>
                        <span>สาขา:</span>
                    </div>
                </td>
                <td width="18%">
                    <div>
                        <asp:DropDownList ID="branch_code" runat="server">
                        </asp:DropDownList>
                    </div>
                </td>
            </tr>
            <tr>
                <td width="12%">
                    <div>
                        <span>เลขธนาคาร/เลขบัญชีสหกรณ์:</span>
                    </div>
                </td>
                <td width="18%">
                    <div>
                        <asp:TextBox ID="expense_accid" runat="server"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td width="12%">
                    <div>
                        <span>ประเภท Clearing:</span>
                    </div>
                </td>
                <td width="18%">
                    <div>
                        <asp:DropDownList ID="expense_clearing" runat="server">
                            <asp:ListItem Value="DC" Text="Direct Credit"></asp:ListItem>
                            <asp:ListItem Value="MCL" Text="Media Clearing"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </td>
            </tr>
            <tr>
                <td width="12%">
                    <div>
                        <span>สั่งจ่ายในนาม:</span>
                    </div>
                </td>
                <td width="18%">
                    <div>
                        <asp:TextBox ID="pay_name" runat="server"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td width="12%">
                    <div>
                        <span>รหัสบัญชี:</span>
                    </div>
                </td>
                <td width="18%">
                    <div>
                        <asp:DropDownList ID="tofrom_accid" runat="server">
                        </asp:DropDownList>
                    </div>
                </td>
            </tr>
            <tr style="display: none;">
                <td width="12%">
                    <div>
                        <span></span>
                    </div>
                </td>
                <td width="18%">
                    <div>
                        <asp:DropDownList ID="cashtype" runat="server">
                        </asp:DropDownList>
                    </div>
                </td>
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>
