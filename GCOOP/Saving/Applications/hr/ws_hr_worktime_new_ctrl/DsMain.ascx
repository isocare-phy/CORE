<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsMain.ascx.cs" Inherits="Saving.Applications.hr.ws_hr_worktime_new_ctrl.DsMain" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit" Width = "100%" >
    <EditItemTemplate>
        <table class="DataSourceFormView">
            <tr>
                <td width="10%">
                    <div>
                        <span>ปี:</span>
                    </div>
                </td>
                <td width="10%">
                    <div>
                        <asp:TextBox ID="year" runat="server" Style="text-align: center;"></asp:TextBox>
                    </div>
                </td>
                <td width="10%">
                    <div>
                        <span>เดือน:</span>
                    </div>
                </td>
                <td width="15%">
                    <asp:DropDownList ID="month" runat="server">
                        <asp:ListItem Value="" >กรุณาเลือก</asp:ListItem>
                        <asp:ListItem Value="01">มกราคม</asp:ListItem>
                        <asp:ListItem Value="02">กุมภาพันธ์</asp:ListItem>
                        <asp:ListItem Value="03">มีนาคม</asp:ListItem>
                        <asp:ListItem Value="04">เมษายน</asp:ListItem>
                        <asp:ListItem Value="05">พฤษภาคม</asp:ListItem>
                        <asp:ListItem Value="06">มิถุนายน</asp:ListItem>
                        <asp:ListItem Value="07">กรกฎาคม</asp:ListItem>
                        <asp:ListItem Value="08">สิงหาคม</asp:ListItem>
                        <asp:ListItem Value="09">กันยายน</asp:ListItem>
                        <asp:ListItem Value="10">ตุลาคม</asp:ListItem>
                        <asp:ListItem Value="11">พฤศจิกายน</asp:ListItem>
                        <asp:ListItem Value="12">ธันวาคม</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td width="10%">
                    <div>
                        <span>เลขพนักงาน:</span>
                    </div>
                </td>
                <td width="20%">
                    <asp:DropDownList ID="emp_no" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>
