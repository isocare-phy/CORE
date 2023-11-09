<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsStatus.ascx.cs" Inherits="Saving.Applications.mbshr.ws_mbshr_member_detail_ctrl.DsStatus" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table class="DataSourceFormView" style="width: 700px;">
            <tr>
                <td width="18%">
                    <div>
                        <span>ประเภทการสมัคร:</span>
                    </div>
                </td>
                <td width="18%">
                    <asp:DropDownList ID="appltype_code" runat="server" Enabled="false">
                    </asp:DropDownList>
                </td>
                <td width="17%">
                    <div>
                        <span>สถานะออกใบเสร็จ:</span>
                    </div>
                </td>
                <td width="18%">
                    <asp:DropDownList ID="pausekeep_flag" runat="server" Enabled="false">
                        <asp:ListItem Value="0">ออกใบเสร็จตามปกติ</asp:ListItem>
                        <asp:ListItem Value="1">ขอหยุดออกใบเสร็จ</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td width="15%">
                    <div>
                        <span>ตั้งแต่วันที่:</span>
                    </div>
                </td>
                <td>
                    <asp:TextBox ID="pausekeep_date" runat="server" ReadOnly="true"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span>การคำนวณเงินปันผล:</span>
                    </div>
                </td>
                <td>
                    <asp:DropDownList ID="dividend_flag" runat="server" Enabled="false">
                        <asp:ListItem Value="-1">งดปันผล (ออก)</asp:ListItem>
                        <asp:ListItem Value="0">งดปันผล</asp:ListItem>
                        <asp:ListItem Value="1">ปันผลปกติ</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <div>
                        <span>เฉลี่ยคืน:</span>
                    </div>
                </td>
                <td>
                    <asp:DropDownList ID="average_flag" runat="server" Enabled="false">
                        <asp:ListItem Value="-1">งดเฉลี่ยคืน (ออก)</asp:ListItem>
                        <asp:ListItem Value="0">งดเฉลี่ยคืน</asp:ListItem>
                        <asp:ListItem Value="1">เฉลี่ยคืนปกติ</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <br>
        <table class="DataSourceFormView" style="width: 530px;" align="center">
            <tr>
                <td valign="top">
                    <asp:Panel ID="Panel1" runat="server" BorderWidth="1" Width="150" Height="65">
                        <table class="DataSourceFormView" style="width: 150px;">
                            <tr>
                                <td>
                                    สถานะการกู้/ค้ำ
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="lndroploanall_flag" runat="server" Text=" งดกู้ทุกประเภท" Enabled="false" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="lndropgrantee_flag" runat="server" Text=" งดค้ำทุกประเภท" Enabled="false" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
                <td valign="top">
                    <asp:Panel ID="Panel2" runat="server" BorderWidth="1" Width="155" Height="65">
                        <table class="DataSourceFormView" style="width: 155px;">
                            <tr>
                                <td>
                                    สถานะปันผล/เฉลี่ยคืน
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="sequest_divavg" runat="server" Text=" อายัด ปันผล/เฉลี่ยคืน" Enabled="false" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="divavgshow_flag" runat="server" Text=" ไม่แสดงปันผลในรายงาน" Enabled="false" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
                <td>
                    <asp:Panel ID="Panel3" runat="server" BorderWidth="1" Width="320" Height="65">
                        <table class="DataSourceFormView" style="width: 320px;">
                            <tr>
                                <td>
                                    สถานะประกอบการกู้
                                </td>
                            </tr>
                            <tr>
                                <td width="50%">
                                    <asp:CheckBox ID="klongtoon_flag" runat="server" Text=" กองทุนสำรองเลี้ยงซีพ" Enabled="false" />
                                </td>
                                <td width="50%">
                                    <asp:CheckBox ID="lntransright_flag" runat="server" Text=" หนังสือโอนสิทธิเรียกร้อง"
                                        Enabled="false" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="lnallowloan_flag" runat="server" Text=" ใบยินยอมคู่สมรส" Enabled="false" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="have_gain" runat="server" Text=" มีผู้รับโอนประโยชน์" Enabled="false" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>
