<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsDetail.ascx.cs" Inherits="Saving.Applications.mbshr.ws_mbshr_member_detail_ctrl.DsDetail" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <span class="TitleSpan">ที่อยู่</span>
        <table class="DataSourceFormView" style="width: 700px;">
            <tr>
                <td>
                    <div>
                        <span>วันเกิด:</span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="birth_date" runat="server" Width="70%" Style="text-align: center;"
                            ReadOnly="true"></asp:TextBox>
                        <asp:TextBox ID="age" runat="server" Width="25%" ReadOnly="true"></asp:TextBox>
                    </div>
                </td>
                <td>
                    <div>
                        <span>บัตรประชาชน:</span>
                    </div>
                </td>
                <td colspan="3">
                    <div>
                        <asp:TextBox ID="card_person" runat="server" ReadOnly="true"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span>ที่อยู่ตามทะเบียนบ้าน:</span>
                    </div>
                </td>
                <td colspan="6">
                    <div>
                        <asp:TextBox ID="cp_addr" runat="server" Width="99.5%" ReadOnly="true"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span>ที่อยู่ปัจจุบัน:</span>
                    </div>
                </td>
                <td colspan="6">
                    <div>
                        <asp:TextBox ID="cp_curraddr" runat="server" Width="99.5%" ReadOnly="true"></asp:TextBox>
                    </div>
                </td>
            </tr>
        </table>
        <span class="TitleSpan">ข้อมูลการสมรส</span>
        <table class="DataSourceFormView" style="width: 700px;">
            <tr>
                <td width="19%">
                    <div>
                        <span>สถานภาพสมรส:</span>
                    </div>
                </td>
                <td width="18%">
                    <div>
                        <asp:DropDownList ID="mariage_status" runat="server" Enabled="false">
                            <asp:ListItem Value="1">โสด</asp:ListItem>
                            <asp:ListItem Value="2">สมรส</asp:ListItem>
                            <asp:ListItem Value="3">หย่าร้าง</asp:ListItem>
                            <asp:ListItem Value="4">หม้าย</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </td>
                <td width="15%">
                    <div>
                        <span>ชื่อคู่สมรส:</span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="mate_name" runat="server" ReadOnly="true"></asp:TextBox>
                    </div>
                </td>
                <td width="15%">
                    <div>
                        <span>เลขพนักงาน:</span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="mate_salaryid" runat="server" ReadOnly="true"></asp:TextBox>
                    </div>
                </td>
            </tr>
        </table>
        <br />
        <span class="TitleSpan">ข้อมูลการทำงาน</span>
        <table class="DataSourceFormView" style="width: 700px;">
            <tr>
                <td width="13%">
                    <div>
                        <span>วันบรรจุ:</span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="work_date" runat="server" Width="65%" Style="text-align: center;"
                            ReadOnly="true"></asp:TextBox>
                        <asp:TextBox ID="work_age" runat="server" Width="30%" ReadOnly="true"></asp:TextBox>
                    </div>
                </td>
                <td width="13%">
                    <div>
                        <span>ตำแหน่ง:</span>
                    </div>
                </td>
                <td width="25%">
                    <div>
                        <asp:TextBox ID="position_desc" runat="server" ReadOnly="true"></asp:TextBox>
                    </div>
                </td>
                <td width="13%">
                    <div>
                        <span>เลขที่พนักงาน:</span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="salary_id" runat="server" ReadOnly="true"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span>วันเกษียณ:</span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="retry_date" runat="server" Width="65%" Style="text-align: center;"
                            ReadOnly="true"></asp:TextBox>
                        <asp:TextBox ID="retry_age" runat="server" Width="30%" ReadOnly="true"></asp:TextBox>
                    </div>
                </td>
                <td>
                    <div>
                        <span>เกษียณแบบ:</span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:DropDownList ID="retry_status" runat="server" Enabled="false">
                            <asp:ListItem Value="0">พนักงานปกติ</asp:ListItem>
                            <asp:ListItem Value="1">เกษียณ</asp:ListItem>
                            <asp:ListItem Value="2">เกษียณก่อนเกณฑ์</asp:ListItem>
                            <asp:ListItem Value="3">ลาออกจากองค์กร</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </td>
                <td>
                    <div>
                        <span>เงินเดือน:</span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="salary_amount" runat="server" ReadOnly="true"></asp:TextBox>
                    </div>
                </td>
            </tr>
        </table>
        <br />
        <span class="TitleSpan">ข้อมูลสมาชิกสหกรณ์</span>
        <table class="DataSourceFormView" style="width: 700px;">
            <tr>
                <td width="13%">
                    <div>
                        <span>วันเป็นสมาชิก:</span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="member_date" runat="server" Width="65%" Style="text-align: center;"
                            ReadOnly="true"></asp:TextBox>
                        <asp:TextBox ID="memb_age" runat="server" Width="30%" ReadOnly="true"></asp:TextBox>
                    </div>
                    <td width="13%">
                        <div>
                            <span>วันที่ลาออก:</span>
                        </div>
                    </td>
                    <td>
                        <div>
                            <asp:TextBox ID="resign_date" runat="server" Style="text-align: center;" ReadOnly="true"></asp:TextBox>
                        </div>
                    </td>
                    <td width="13%">
                        <div>
                            <span>วันที่ปิดบัญชี:</span>
                        </div>
                    </td>
                    <td>
                        <div>
                            <asp:TextBox ID="close_date" runat="server" Style="text-align: center;" ReadOnly="true"></asp:TextBox>
                        </div>
                    </td>
            </tr>
            <tr>
                <td width="13%">
                    <div>
                        <span>อ้างอิงสมาชิก:</span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="member_ref" runat="server" ReadOnly="true"></asp:TextBox>
                    </div>
                </td>
                <td width="5%">
                    <div>
                        <span>สาเหตุลาออก:</span>
                    </div>
                </td>
                <td colspan="3">
                    <div>
                        <asp:TextBox ID="resigncause_code" runat="server" ReadOnly="true"></asp:TextBox>
                    </div>
                </td>
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>
