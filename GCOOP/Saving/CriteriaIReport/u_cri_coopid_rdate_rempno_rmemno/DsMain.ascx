<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsMain.ascx.cs" Inherits="Saving.CriteriaIReport.u_cri_coopid_rdate_rempno_rmemno.DsMain" %>
<link id="css1" runat="server" href="../../JsCss/DataSourceTool.css" rel="stylesheet" type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table class="iReportDataSourceFormView">
            <tr>
                <td width="30%">
                    <div>
                        <span>สาขา:</span>
                    </div>
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="coop_id" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span>ตั้งแต่วันที่:</span>
                    </div>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="post_date" runat="server"> </asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span>ถึงวันที่:</span>
                    </div>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="post_date2" runat="server"> </asp:TextBox>
                </td>
                <!--<td colspan="3">
                    <asp:DropDownList ID="uinf_acction" runat="server">
                        <asp:ListItem Value="HIR" Selected="True">HIR-พนักงานใหม่</asp:ListItem>
                        <asp:ListItem Value="DTA">DTA-เปลี่ยนแปลงข้อมูล</asp:ListItem>
                        <asp:ListItem Value="TER">TER-ลาออก/ เปลี่ยนวันที่ลาออก</asp:ListItem>
                        <asp:ListItem Value="CTR">CTR-ยกเลิกลาออก</asp:ListItem>
                        <asp:ListItem Value="PRO">PRO-ปรับ G1 เป็น STAFF</asp:ListItem>
                        <asp:ListItem Value="REH">REH-ปรับจ้างสัญญาจ้างเป็นพนักงานประจำ</asp:ListItem>
                        <asp:ListItem Value="XFR">XFR-โอนย้ายข้ามบริษัท</asp:ListItem>
                    </asp:DropDownList>-->
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span>เลขสมาชิก:</span>
                    </div>
                </td>
                <td>
                    <asp:TextBox ID="smemb_no" runat="server"> </asp:TextBox>
                </td>
                <td>
                    -
                </td>
                <td>
                    <asp:TextBox ID="ememb_no" runat="server"> </asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span>เลขพนักงาน:</span>
                    </div>
                </td>
                <td>
                    <asp:TextBox ID="semp_no" runat="server"> </asp:TextBox>
                </td>
                <td>
                    -
                </td>
                <td>
                    <asp:TextBox ID="eemp_no" runat="server"> </asp:TextBox>
                </td>
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>