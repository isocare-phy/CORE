<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsLasrights.ascx.cs" 
Inherits="Saving.Applications.hr.ws_hr_leave_ctrl.DsLasrights" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet" type="text/css" />
 
 <asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>  
        <table class ="DataSourceFormView">
            <tr>
                <td width="13%">
                    <div>
                        <span>ครั้งสุดท้ายที่ลา :</span>
                    </div>
                </td>
                <td width="15%">
                    <div>
                        <asp:TextBox ID="leave_desc" runat="server"></asp:TextBox>
                    </div>
                </td>
                <td  width="11%">
                    <div>
                        <span>จำนวนวัน :</span>
                    </div>
                </td>
                <td width="10%">
                    <div>
                        <asp:TextBox ID="totalday" runat="server"></asp:TextBox>
                    </div>
                </td>
           
                <td width="10%">
                    <div>
                        <span>ตั้งแต่วันที่ :</span>
                    </div>
                </td>
                <td  width="13%">
                    <div>
                        <asp:TextBox ID="Leave_From" runat="server"></asp:TextBox>
                    </div>
                </td>
                <td  width="10%">
                    <div>
                        <span>ถึงวันที่ :</span>
                    </div>
                </td>
                <td   width="13%">
                    <div>
                        <asp:TextBox ID="Leave_To" runat="server"></asp:TextBox>
                    </div>
                </td>
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>