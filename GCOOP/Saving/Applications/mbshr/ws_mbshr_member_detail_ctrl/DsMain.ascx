<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsMain.ascx.cs" Inherits="Saving.Applications.mbshr.ws_mbshr_member_detail_ctrl.DsMain" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <span class="TitleSpan">ข้อมูลสมาชิก</span>
        <table class="DataSourceFormView">
            <tr>
                <td width="13%">
                    <div>
                        <span>เลขสมาชิก</span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="member_no" runat="server" Style="width: 99px;"></asp:TextBox>
                    </div>
                </td>
                <td width="13%">
                    <div>
                        <span>ชื่อ-สกุล:</span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="cp_name" runat="server" ReadOnly="true" Style="width: 179px;"></asp:TextBox>
                    </div>
                </td>
                <td width="13%">
                    <div>
                        <span>ปกติ/สมทบ:</span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:DropDownList ID="member_type" runat="server" Enabled="false">
                            <asp:ListItem Value="1">สมาชิกปกติ</asp:ListItem>
                            <asp:ListItem Value="2">สมาชิกสมทบ</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </td>
            </tr>
            <tr>
                <td width="13%">
                    <div>
                        <span>เพศ</span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:DropDownList ID="sex" runat="server" Enabled="false">
                            <asp:ListItem Value="M">ชาย</asp:ListItem>
                            <asp:ListItem Value="F">หญิง</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </td>
                <td width="13%">
                    <div>
                        <span>หน่วยสมาชิก:</span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="cp_membgroup" runat="server" ReadOnly="true" Style="width: 179px;"></asp:TextBox>
                    </div>
                </td>
                <td width="13%">
                    <div>
                        <span>ประเภทสมาชิก:</span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="cp_membtype" runat="server" Style="width: 145px;" ReadOnly="true"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="6">
                    <div>
                        <asp:TextBox ID="remark" runat="server" TextMode="MultiLine" ReadOnly="true" Style="border: 1px solid black;
                            height: 120px; border-bottom: 2px solid #000000; width: 725px;"></asp:TextBox>
                    </div>
                </td>
            </tr>
        </table>
        <br />
    </EditItemTemplate>
</asp:FormView>
