<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_hr_salamas.aspx.cs" Inherits="Saving.Applications.hr.w_sheet_hr_salamas_ctrl.w_sheet_hr_salamas" %>

<%@ Register Src="DsList.ascx" TagName="DsList" TagPrefix="uc1" %>
<%@ Register Src="DsMain.ascx" TagName="DsMain" TagPrefix="uc1" %>
<%@ Register Src="DsTran.ascx" TagName="DsTran" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style3
        {
            float: left;
            width: 96%;
            height: 22px;
            margin: 2px 2px 0px 0px;
            line-height: 22px;
            text-align: right;
            vertical-align: middle;
            border: 1px solid #000000;
            background-color: rgb(211, 231, 255);
            font-family: Tahoma;
            font-size: 13px;
        }
        .style2
        {
            width: 96%;
            height: 22px;
            float: left;
            font-family: Tahoma;
            font-size: 13px;
            border: 1px solid #000000;
            margin: 2px 2px 0px 0px;
            vertical-align: middle;
        }
    </style>
    <script type="text/javascript">

        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล");
        }
        function SheetLoadComplete() {

        }
        function OnDsListClicked(s, r, c) {

        }
        function OnDsListItemChanged(s, r, c, v) {
            
        }
        //ประกาศฟังก์ชันสำหรับ event ItemChanged
        function OnDsMainItemChanged(s, r, c, v) {
            if (c == "emplcode") {
                PostRetreive();
            }
        }

        //ประกาศฟังก์ชันสำหรับ event Clicked
        function OnDsMainClicked(s, r, c) {
            if (c == "taxcal") {
                PostTaxcal();
            }
            else if (c == "tran_dp") {
                PostTrandp();
            }
        }

        //ประกาศฟังก์ชันสำหรับ event ItemChanged
        function OnDsDetailItemChanged(s, r, c, v) {

        }

        //ประกาศฟังก์ชันสำหรับ event Clicked
        function OnDsDetailClicked(s, r, c) {
            if (c == "taxcal") {
                dsDetail.SetRowFocus(r);
                PostDeleteRow();
            }
        }

        function OnClickInsertRow() {
            PostInsertRow();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <table>
        <tr>
            <td width="100">
                <div>
                    <span class="style3">ปี:</span>
                </div>
            </td>
            <td>
                <div>
                    <asp:TextBox ID="year" runat="server" class="style2"></asp:TextBox>
                </div>
            </td>
            <td width="100">
                <div>
                    <span class="style3">เดือน:</span>
                </div>
            </td>
            <td>
                <div>
                    <asp:DropDownList ID="month" runat="server" class="style2" Width="100">
                        <asp:ListItem>มกราคม</asp:ListItem>
                        <asp:ListItem>กุมภาพันธ์</asp:ListItem>
                        <asp:ListItem>มีนาคม</asp:ListItem>
                        <asp:ListItem>เมษายน</asp:ListItem>
                        <asp:ListItem>พฤษภาคม</asp:ListItem>
                        <asp:ListItem>มิถุนายน</asp:ListItem>
                        <asp:ListItem>กรกฏาคม</asp:ListItem>
                        <asp:ListItem>สิงหาคม</asp:ListItem>
                        <asp:ListItem>กันยายน</asp:ListItem>
                        <asp:ListItem>ตุลาคม</asp:ListItem>
                        <asp:ListItem>พฤศจิกายน</asp:ListItem>
                        <asp:ListItem>ธันวาคม</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </td>
        </tr>
        <tr>
            <td width="100">
                <div>
                    <span class="style3">รูปแบบการจ่าย:</span>
                </div>
            </td>
            <td>
                <div>
                    <asp:DropDownList ID="pay_type" runat="server" class="style2" Width="150">
                        <asp:ListItem>โอนเงินเข้าบัญชีก่อนวันทำการ 2 วัน</asp:ListItem>
                        <asp:ListItem>โอนเงินเข้าบัญชีสิ้นเดือน</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </td>
            <td width="100">
                <div>
                    <span class="style3">วันที่จ่าย:</span>
                </div>
            </td>
            <td>
                <div>
                    <asp:TextBox ID="pay_date" runat="server" class="style2"></asp:TextBox>
                </div>
            </td>
        </tr>
    </table>
    <uc1:DsMain ID="dsMain" runat="server" />
    <br />
    <u>รายละเอียด</u>
    <uc1:DsTran ID="dsTran" runat="server" />
    <uc1:DsList ID="dsList" runat="server" />
</asp:Content>
