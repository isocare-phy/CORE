<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsLeaveout.ascx.cs"
    Inherits="Saving.Applications.hr.ws_hr_leaveout_ctrl.DsLeaveout" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table class="DataSourceFormView">
            <tr>
                <td>
                    <div>
                        <span>ประเภทการลา :</span>
                    </div>
                </td>
                <td colspan="3">
                    <div>
                        <asp:DropDownList ID="leave_code" runat="server">
                            <asp:ListItem Value="0" Text="ลาชั่วโมง"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </td>
                <td>
                    <div>
                        <span>วันที่ทำรายการ :</span>
                    </div>
                </td>
                <td colspan="3">
                    <div>
                        <asp:TextBox ID="Operate_Date" runat="server"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span>ช่วงเวลาที่เริ่มลา :</span>
                    </div>
                </td>
                <td colspan="3">
                    <div>
                        <asp:TextBox ID="start_time" runat="server" MaxLength="5" ></asp:TextBox>
                    </div>
                </td>
                <td colspan="2">
                    <div>
                        <span>ถึงเวลา :</span>
                    </div>
                </td>
                <td colspan="3">
                    <div>
                        <asp:TextBox ID="last_time" runat="server" MaxLength="5" ></asp:TextBox>
                    </div>
                </td>
                <td colspan="3" width="15%">
                    <div>
                        <asp:TextBox ID="totaltime" runat="server" MaxLength="5"></asp:TextBox>
                    </div>
                </td>
                <td colspan="2" width="10%">
                    <div>
                        <span>ชม.</span>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span>เหตุผลการลา :</span>
                    </div>
                </td>
                <td colspan="7">
                    <div>
                        <asp:TextBox ID="Leave_Cause" runat="server"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span>สถานะการอนุมัติ :</span>
                    </div>
                </td>
                <td colspan="3">
                    <div>
                        <asp:DropDownList ID="Apv_Status" runat="server">
                            <asp:ListItem Value="0" Text="อนุมัติ"></asp:ListItem>
                            <asp:ListItem Value="1" Text="ไม่อนุมัติ"></asp:ListItem>
                            <asp:ListItem Value="2" Text="ยกเลิก"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </td>
                <td colspan="2">
                    <div>
                        <span>วันที่อนุมัติ :</span>
                    </div>
                </td>
                <td colspan="2">
                    <div>
                        <asp:TextBox ID="Apv_Date" runat="server"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span>ผู้อนุมัติ :</span>
                    </div>
                </td>
                <td colspan="7">
                    <div>
                        <asp:DropDownList ID="Apv_Bycode" runat="server">
                        </asp:DropDownList>
                    </div>
                </td>
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>
