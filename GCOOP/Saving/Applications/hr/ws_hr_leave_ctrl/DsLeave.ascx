<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsLeave.ascx.cs" Inherits="Saving.Applications.hr.ws_hr_leave_ctrl.DsLeave" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet" type="text/css" />
 
 <asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>  
        <table class ="DataSourceFormView">
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
                        <asp:TextBox ID="Operate_Date" runat="server"></asp:TextBox>
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
                        <asp:TextBox ID="Leave_From" runat="server"></asp:TextBox>
                    </div>
                </td>
                <td colspan="2">
                    <div>
                        <span>ถึงวันที่ :</span>
                    </div>
                </td>
                <td  colspan="3">
                    <div>
                        <asp:TextBox ID="Leave_To" runat="server"></asp:TextBox>
                    </div>
                </td>
            </tr>

            <tr>
                <td width="15%">
                    <div>
                        <span>จำนวนวันที่ลา :</span>
                    </div>
                </td>
                <td  width="10%">
                    <div>
                        <asp:TextBox ID="Totalday" runat="server"></asp:TextBox>
                    </div>
                </td>
                <td  width="10%">
                    <div>
                        <span>วัน</span>
                    </div>
                </td>
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
            </tr>--%>

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