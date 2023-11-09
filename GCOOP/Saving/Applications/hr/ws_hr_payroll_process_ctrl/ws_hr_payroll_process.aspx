<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="ws_hr_payroll_process.aspx.cs" Inherits="Saving.Applications.hr.ws_hr_payroll_process_ctrl.ws_hr_payroll_process" %>

<%@ Register Src="DsMain.ascx" TagName="DsMain" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var dsMain = new DataSourceTool;

        function Post() {
            Post();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
        type="text/css" />
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <uc1:DsMain ID="dsMain" runat="server" />
    <br />
    <table class="DataSourceFormView" style="width: 500px">
        <tr>
            <td width="15%">
                <div>
                    <span>ปี :</span></div>
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
                <input type="button" value="ประมวลผล" style="width: 80px" onclick="Post()" />
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <br />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div>
                    <span>rate ประกันสังคม (%):</span></div>
            </td>
            <td colspan="2">
                <asp:TextBox ID="ss_emprate" runat="server" Style="text-align: center;" ToolTip="#,##0.00"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div>
                    <span>จำนวนวัน(คำนวณเงินเดือน):</span></div>
            </td>
            <td colspan="2">
                <asp:TextBox ID="salary_day" runat="server" Style="text-align: center;" ToolTip="#,##0"></asp:TextBox>
            </td>
            <td style="color: #FF0000">
                *ใส่ 0 ตามวันจริง
            </td>
        </tr>
    </table>
</asp:Content>
