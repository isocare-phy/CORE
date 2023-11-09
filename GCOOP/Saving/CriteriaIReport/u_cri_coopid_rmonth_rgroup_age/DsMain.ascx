<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsMain.ascx.cs" Inherits="Saving.CriteriaIReport.u_cri_coopid_rmonth_rgroup_age.DsMain" %>
<link id="css1" runat="server" href="../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table class="iReportDataSourceFormView">
            <tr>
                <td>
                    <div>
                        <span>สาขา:</span>
                    </div>
                </td>
                <td colspan="2">
                   <asp:DropDownList ID="coop_id" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
             <tr>
                <td>
                    <div>
                        <span>ตั้งแต่เดือนที่:</span>
                    </div>
                </td>
               <td>
                    <asp:DropDownList ID="as_startmonth" runat="server">
                        <%--<asp:ListItem Value="00" Text=""></asp:ListItem>--%>
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
            </tr>
<%--            <tr>
                <td>
                    <div>
                        <span>ถึงเดือนที่:</span>
                    </div>
                </td>
                <td>
                    <asp:DropDownList ID="as_endmonth" runat="server">
                        <asp:ListItem Value="00" Text=""></asp:ListItem>
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
            </tr>--%>
            <tr>
                <td width="30%">
                    <div>
                        <span>ตามสังกัด:</span>
                    </div>
                </td>
                <td>
                    <asp:DropDownList ID="as_startgroup" runat="server">
                    </asp:DropDownList>
                    
                </td>
                <td width="35%">
                    <asp:DropDownList ID="as_endgroup" runat="server">
                    </asp:DropDownList>
                </td>
                </tr>
            <tr>
                <td>
                    <div>
                        <span>เป็นสมาชิกกี่ปีขึ้นไป:</span>
                    </div>
                </td>
                <td>
                    <asp:TextBox ID="age_mem" runat="server"></asp:TextBox>
                </td>
   
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>
