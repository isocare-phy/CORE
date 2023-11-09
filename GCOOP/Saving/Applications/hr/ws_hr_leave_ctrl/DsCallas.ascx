<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsCallas.ascx.cs" 
Inherits="Saving.Applications.hr.ws_hr_leave_ctrl.DsCallas" %>

<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet" type="text/css" />
 
 <asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>  
        <table class ="DataSourceFormView">
            <tr>
                <td width="15%">
                    <div>
                        <span>ลาพักผ่อนยกมา :</span>
                    </div>
                </td>
                <td  width="10%">
                    <div>
                        <asp:TextBox ID="FROM_YEAR" runat="server"></asp:TextBox>
                    </div>
                </td>
                 <td width="15%">
                    <div>
                        <span>ลาพักผ่อนปีนี้ :</span>
                    </div>
                </td>
                <td width="10%">
                    <div>
                        <asp:TextBox ID="TO_YEAR" runat="server"></asp:TextBox>
                    </div>
                </td>
                <td width="10%">
                    <div>
                        <span>รวม :</span>
                    </div>
                </td>
                <td width="10%">
                    <div>
                        <asp:TextBox ID="total" runat="server"></asp:TextBox>
                    </div>
                </td>
                <td width="10%">
                    <div>
                        <span>คงเหลือ :</span>
                    </div>
                </td>
                <td width="10%">
                    <div>
                        <asp:TextBox ID="daytal" runat="server"></asp:TextBox>
                    </div>
                </td>
                <td width="10%">
                    <div>
                        <span>วัน</span>
                    </div>
                </td>
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>
<%--<center><div>(สะสมวันลาได้ 2 ปีรวมกันไม่เกิน 30 วัน)</div></center>--%>