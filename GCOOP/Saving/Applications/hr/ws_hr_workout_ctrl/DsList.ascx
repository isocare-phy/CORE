<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsList.ascx.cs" Inherits="Saving.Applications.hr.ws_hr_workout_ctrl.DsList" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table class="DataSourceFormView">
            <tr>
                <td>
                    <div>
                        <span>วันที่ร้องขอโอที :</span>
                    </div>
                </td>
                <td colspan="3">
                    <div>
                        <asp:TextBox ID="date_work" runat="server"></asp:TextBox>
                    </div>
                </td>
                 <td>
                    <div>
                        <span>ประเภทโอที :</span>
                    </div>
                </td>
                 <td colspan="6">
                    <div>
                        <asp:DropDownList ID="remark" runat="server">
                        </asp:DropDownList>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span>ช่วงเวลาที่เริ่มทำ OT :</span>
                    </div>
                </td>
                <td colspan="3">
                    <div>
                        <asp:TextBox ID="work_in" runat="server" MaxLength="5" ></asp:TextBox>
                    </div>
                </td>
                <td colspan="2">
                    <div>
                        <span>ถึงเวลา :</span>
                    </div>
                </td>
                <td colspan="3">
                    <div>
                        <asp:TextBox ID="work_out" runat="server" MaxLength="5" ></asp:TextBox>
                    </div>
                </td>
                <td colspan="3" width="15%">
                    <div>
                        <asp:TextBox ID="ot_p" runat="server" MaxLength="5"></asp:TextBox>
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
                        <span>รายละเอียด :</span>
                    </div>
                </td>
                 <td colspan="6">
                    <div>
                        <asp:TextBox ID="description" runat="server"></asp:TextBox>
                    </div>
                </td>
            </tr>
            
          <!--  <tr>
                <td>
                    <div>
                        <span>สถานะการอนุมัติ :</span>
                    </div>
                </td>
                <td colspan="4">
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

                <td>
                    <div>
                        <span>ผู้อนุมัติ :</span>
                    </div>
                </td>
                <td colspan="6">
                    <div>
                        <asp:DropDownList ID="Apv_Bycode" runat="server">
                        </asp:DropDownList>
                    </div>
                </td>

            </tr> -->
            <tr>
                
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>
