<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="ws_hr_payrolldet.aspx.cs" Inherits="Saving.Applications.hr.ws_hr_payrolldet_ctrl.ws_hr_payrolldet" %>

<%@ Register Src="DsMain.ascx" TagName="DsMain" TagPrefix="uc1" %>
<%@ Register Src="DsListInc.ascx" TagName="DsListInc" TagPrefix="uc2" %>
<%@ Register Src="DsListExc.ascx" TagName="DsListExc" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        var dsMain = new DataSourceTool;
        var dsListInc = new DataSourceTool;
        var dsListExc = new DataSourceTool;

        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล");
        }

        function MenubarOpen() {
            Gcoop.OpenIFrame2('685', '460', 'ws_dlg_hr_master_search.aspx', '');
        }

        function GetEmpNoFromDlg(emp_no) {
            dsMain.SetItem(0, "emp_no", emp_no);
            PostEmpNo();
        }

        function SheetLoadComplete() {
            var sum_inc = 0;

            for (var i = 0; i < dsListInc.GetRowCount(); i++) {
                sum_inc += Number(dsListInc.GetItem(i, "item_amt"));
            }
            $("#ctl00_ContentPlace_dsListInc_cpsum_Inc_amt").val(addCommas(sum_inc, 2));
            var sum_exc = 0;
            for (var i = 0; i < dsListExc.GetRowCount(); i++) {
                sum_exc += Number(dsListExc.GetItem(i, "item_amt"));
            }
            $("#ctl00_ContentPlace_dsListExc_cpsum_Exc_amt").val(addCommas(sum_exc, 2));

            var sum_netamt = 0;
            sum_netamt = sum_inc - sum_exc;
            $("#ctl00_ContentPlace_cpsum_net_amt").val(addCommas(sum_netamt, 2));
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <uc1:DsMain ID="dsMain" runat="server" />
    <br />
    <table class="DataSourceFormView" style="width: 550px">
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
                    <span>เดือน: </span>
                </div>
            </td>
            <td>
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
            <td width="15%">
                <input type="button" value="ดึงข้อมูล" style="width: 80px" onclick="PostRetrieve()" />
            </td>
        </tr>
    </table>
    <br />
    <table>
        <tr>
            <td>
                <uc2:DsListInc ID="dsListInc" runat="server" />
            </td>
            <td>
                <uc3:DsListExc ID="dsListExc" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <table class="DataSourceFormView" style="width: 340px;">
                    <tr>
                        <td style="text-align: right;">
                            <strong style="color: #CC0000">รวมรายได้สุทธิ:</strong>
                        </td>
                        <td width="30%">
                            <asp:TextBox ID="cpsum_net_amt" runat="server" Style="font-size: 13px; text-align: right;
                                font-weight: bold;" ToolTip="#,##0.00"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
