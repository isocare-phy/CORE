<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsLeave.ascx.cs" Inherits="Saving.Applications.hr.ws_hr_leave_n_ctrl.DsLeave" %>
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
                        <asp:TextBox ID="LEAVE_DATE" runat="server" Style="text-align: center; background-color: #DDDDDD;"
                            ReadOnly="True"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span>ตั้งแต่วันที่ :</span>
                    </div>
                </td>
                <td colspan="3">
                    <div>
                        <asp:TextBox ID="Leave_From" runat="server" placeholder="วัน/เดือน/ปี"></asp:TextBox>
                    </div>
                </td>
                <td colspan="2">
                    <div>
                        <span>ถึงวันที่ :</span>
                    </div>
                </td>
                <td colspan="3">
                    <div>
                        <asp:TextBox ID="Leave_To" runat="server" placeholder="วัน/เดือน/ปี"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td width="15%">
                    <div>
                        <span>จำนวนวันที่ลา :</span>
                    </div>
                </td>
                <td width="10%">
                    <div>
                        <asp:TextBox ID="Totalday" runat="server"></asp:TextBox>
                    </div>
                </td>
                <td width="10%">
                    <div>
                        <span>วัน</span>
                    </div>
                </td>
                <%--<td  width="15%">
                    <div>
                        <span>จำนวนชั่วโมง :</span>
                    </div>
                </td>
                <td  width="15%">
                    <div>
                        <asp:TextBox ID="totaltime" runat="server"></asp:TextBox>
                    </div>
                </td>>
                <td  width="5%">
                    <div>
                        <span>ชม.</span>
                    </div>
                </td>
                <td  width="10%">
                    <div>
                        <span>โทร :</span>
                    </div>
                </td>
               <td  width="15%">
                    <div>
                        <asp:TextBox ID="Leave_Tel" runat="server"></asp:TextBox>
                    </div>
                </td>--%>
                <td>
                    <div>
                        <span>เหตุผลการลา :</span>
                    </div>
                </td>
                <td colspan="5">
                    <div>
                        <asp:TextBox ID="Leave_Cause" runat="server"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <%-- <tr>
                <td>
                    <div>
                        <span>สถานที่ติดต่อ :</span>
                    </div>
                </td>
                <td colspan="5">
                    <div>
                        <asp:TextBox ID="Leave_Place" runat="server"></asp:TextBox>
                    </div>
                </td>
                <td>
                    <div>
                        <span>เบอร์โทร :</span>
                    </div>
                </td>
                <td colspan="1">
                    <div>
                        <asp:TextBox ID="Leave_Tel" runat="server" style="width:80px"></asp:TextBox>
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

            <%--<tr>
                <td>
                    <div>
                        <span>เหตุผลการอนุมัติ :</span>
                    </div>
                </td>
                <td colspan="7">
                    <div>
                        <asp:TextBox ID="Apv_Remark" runat="server"></asp:TextBox>
                    </div>
                </td>
            </tr>--%>
        </table>
    </EditItemTemplate>
</asp:FormView>
