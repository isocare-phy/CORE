<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="ws_hr_payroll_inc_exc.aspx.cs" Inherits="Saving.Applications.hr.ws_hr_payroll_inc_exc_ctrl.ws_hr_payroll_inc_exc" %>

<%@ Register Src="DsMain.ascx" TagName="DsMain" TagPrefix="uc1" %>
<%@ Register Src="DsListFixed.ascx" TagName="DsListFixed" TagPrefix="uc2" %>
<%@ Register Src="DsListOther.ascx" TagName="DsListOther" TagPrefix="uc3" %>
<%@ Register Src="DsList.ascx" TagName="DsList" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        var dsMain = new DataSourceTool;
        var dsListFixed = new DataSourceTool;
        //var dsListOther = new DataSourceTool;

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

        function OnDsListFixedItemChanged(s, r, c, v) {
            if (c == "item_amt") {
                var row = dsListFixed.GetRowCount();
                var sum_itemamt = 0;

                for (var i = 0; i < row; i++) {
                    sum_itemamt += Number(dsListFixed.GetItem(i, "item_amt"));
                }
                $("#ctl00_ContentPlace_dsListFixed_cpsum_fixed_amt").val(addCommas(sum_itemamt, 2));
            }
        }

        function OnDsListFixedClicked(s, r, c) {
            if (c == "b_delete") {
                dsListFixed.SetRowFocus(r);
                PostDeleteFixedRow();
            }
        }

//        function OnDsListOtherItemChanged(s, r, c, v) {
//            if (c == "item_amt") {
//                var row = dsListOther.GetRowCount();
//                var sum_itemamt = 0;

//                for (var i = 0; i < row; i++) {
//                    sum_itemamt += Number(dsListOther.GetItem(i, "item_amt"));
//                }
//                $("#ctl00_ContentPlace_dsListOther_cpsum_other_amt").val(addCommas(sum_itemamt, 2));
//            }
//        }

//        function OnDsListOtherClicked(s, r, c) {
//            if (c == "b_delete") {
//                dsListOther.SetRowFocus(r);
//                PostDeleteOtherRow();
//            }
//        }

        function SheetLoadComplete() {
//            var sum_itemamt = 0;

//            for (var i = 0; i < dsListOther.GetRowCount(); i++) {
//                sum_itemamt += Number(dsListOther.GetItem(i, "item_amt"));
//            }
//            $("#ctl00_ContentPlace_dsListOther_cpsum_other_amt").val(addCommas(sum_itemamt, 2));

//            sum_itemamt = 0;
//            for (var i = 0; i < dsListFixed.GetRowCount(); i++) {
//                sum_itemamt += Number(dsListFixed.GetItem(i, "item_amt"));
//            }
//            $("#ctl00_ContentPlace_dsListFixed_cpsum_fixed_amt").val(addCommas(sum_itemamt, 2));


        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <uc1:DsMain ID="dsMain" runat="server" />
    <br />
    <span style="font-size: 12px;"><font color="#cc0000"><u><strong>รายการเงินหัก-เงินเพิ่ม</strong></u></font></span>
    <uc2:DsListFixed ID="dsListFixed" runat="server" />
    <br />
    <uc4:DsList ID="dsList" runat="server" />
    <br />
    <%--<span style="font-size: 12px;"><font color="#cc0000"><u><strong>รายการเงินหัก-เงินเพิ่มประจำเดือน</strong></u></font></span>
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
                    <span>เดือน:</span></div>
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
    <uc3:DsListOther ID="dsListOther" runat="server" />--%>
    <br />
</asp:Content>
