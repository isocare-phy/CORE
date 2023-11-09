<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="ws_hr_payroll_depttran.aspx.cs" Inherits="Saving.Applications.hr.ws_hr_payroll_depttran_ctrl.ws_hr_payroll_depttran" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function PostCheck() {
            PostCheck();
        }

        function Post() {
            Post();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <table class="DataSourceFormView" style="width: 500px">
        <tr>
            <td colspan="2">
                <div>
                    <span>ประเภทพนักงาน:</span></div>
            </td>
            <td colspan="2">
                <asp:DropDownList ID="emptype_code" runat="server">
                </asp:DropDownList>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div>
                    <span>ยอดผ่านรายการ:</span></div>
            </td>
            <td colspan="2">
                <asp:TextBox ID="tran_amt" runat="server" Style="text-align: right;" ToolTip="#,##0.00"></asp:TextBox>
            </td>
            <td>
                <input type="button" value="ตรวจสอบ" style="width: 80px" onclick="PostCheck()" />
            </td>
        </tr>
        <tr>
            <td width="15%">
                <div>
                    <span>ปี:</span></div>
            </td>
            <td width="20%">
                <asp:TextBox ID="year" runat="server" Style="text-align: center;"></asp:TextBox>
            </td>
            <td width="15%">
                <div>
                    <span>เดือน:</span></div>
            </td>
            <td width="30%">
                <asp:DropDownList ID="month" runat="server">
                    <asp:ListItem Value="0" Text=""></asp:ListItem>
                    <asp:ListItem Value="1">มกราคม</asp:ListItem>
                    <asp:ListItem Value="2">กุมภาพันธ์</asp:ListItem>
                    <asp:ListItem Value="3">มีนาคม</asp:ListItem>
                    <asp:ListItem Value="4">เมษายน</asp:ListItem>
                    <asp:ListItem Value="5">พฤษภาคม</asp:ListItem>
                    <asp:ListItem Value="6">มิถุนายน</asp:ListItem>
                    <asp:ListItem Value="7">กรกฎาคม</asp:ListItem>
                    <asp:ListItem Value="8">สิงหาคม</asp:ListItem>
                    <asp:ListItem Value="9">กันยายน</asp:ListItem>
                    <asp:ListItem Value="10">ตุลาคม</asp:ListItem>
                    <asp:ListItem Value="11">พฤศจิกายน</asp:ListItem>
                    <asp:ListItem Value="12">ธันวาคม</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td width="20%">
                <input type="button" value="ผ่านรายการ" style="width: 80px" onclick="Post()" />
            </td>
        </tr>
    </table>
</asp:Content>
