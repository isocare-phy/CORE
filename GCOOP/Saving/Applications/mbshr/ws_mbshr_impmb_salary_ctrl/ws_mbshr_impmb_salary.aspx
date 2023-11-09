<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" 
CodeBehind="ws_mbshr_impmb_salary.aspx.cs" Inherits="Saving.Applications.mbshr.ws_mbshr_impmb_salary_ctrl.ws_mbshr_impmb_salary" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        $(function () {
            //uplond text file
            if ($('.FileName').val()) {
                $('.textFileName').text($('.FileName').val());
            }
        });

        function UpdateMembGroup() {
            var re = confirm("ต้องการปรับปรุงสังกัดสมาชิก (พนักงาน) ?");
            if (re == true) {
                PostUpdateMembGroup();
            }
        }

        function UpdateMembGroupParttime() {
            var re = confirm("ต้องการปรับปรุงสังกัดสมาชิก (ลูกจ้าง) ?");
            if (re == true) {
                PostUpdateMembGroupParttime();
            }
        }

        function UpdateSal() {
            var re = confirm("ต้องการปรับปรุงเงินเดือน (พนักงาน) ?");
            if (re == true) {
                PostUpdateSal();
            }
        }

        function UpdateSalParttime() {
            var re = confirm("ต้องการปรับปรุงเงินเดือน (ลูกจ้าง) ?");
            if (re == true) {
                PostUpdateSalParttime();
            }
        }

        function UpdateMembName() {
            var re = confirm("ต้องการปรับปรุงข้อมูลชื่อสมาชิก ?");
            if (re == true) {
                PostMembName();
            }
        }

        function UpdateMembSurname() {
            var re = confirm("ต้องการปรับปรุงข้อมูลชื่อสกุลสมาชิก ?");
            if (re == true) {
                PostMembSurname();
            }
        }

        function UpdateMinShare() {
            var re = confirm("ต้องการปรับปรุงฐานหุ้นขั้นต่ำตามเงินเดือนสมาชิก ?");
            if (re == true) {
                PostUpdateMinShare();
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <div> 
        <span class="txtFileName"></span>
        <asp:TextBox ID="FileName" class="FileName" runat="server" Visible="false"></asp:TextBox>
    </div>
    <table class="DataSourceFormView" width="100%">
        <tr>
            <td width='5%'>
                <div>
                    <span>File Path : </span>
                </div>
            </td>
            <td width='30%'>
                <asp:FileUpload ID="txtInput" class="Filetxt" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <input type="button" value="Import ข้อมูล" style="width: 342px;" onclick="PostImport()" />
            </td>
        </tr>
    </table>
    <br />
    <table class="DataSourceFormView" width="100%">
        <tr>
            <td colspan="2" style="font-family:Cordia New; font-size:25pt; font-weight:bold;">
                อัพเดทข้อมูล
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <input type="button" value="ปรับปรุงข้อมูลเงินเดือนพนักงาน" style="width:250px;" onclick="UpdateSal()" />
            </td>
            <td colspan="2">
                <input type="button" value="ปรับปรุงข้อมูลเงินเดือนลูกจ้าง" style="width:250px;" onclick="UpdateSalParttime()" />
            </td>
        </tr>
        <tr style="height:25px;">
        </tr>
        <tr>
            <td colspan="2">
                <input type="button" value="ปรับปรุงข้อมูลสังกัดพนักงาน" style="width:250px;" onclick="UpdateMembGroup()" />
            </td>
            <td colspan="2">
                <input type="button" value="ปรับปรุงข้อมูลสังกัดลูกจ้าง"  style="width:250px;" onclick="UpdateMembGroupParttime()" />
            </td>
        </tr>
        <tr style="height:25px;">
        </tr>
        <tr>
            <td colspan="2">
                <input type="button" value="ปรับปรุงข้อมูลชื่อสมาชิก" style="width:250px;" onclick="UpdateMembName()" />
            </td>
            <td colspan="2">
                <input type="button" value="ปรับปรุงข้อมูลชื่อสกุลสมาชิก"  style="width:250px;" onclick="UpdateMembSurname()" />
            </td>
        </tr>
        <tr style="height:25px;">
        </tr>
        <tr>
            <td colspan="2">
                <input type="button" value="ปรับปรุงข้อมูลหุ้นฐานประจำปี" style="width:250px;" onclick="PostUpdateShare()" />
            </td>
            <td colspan="2">
                <input type="button" value="ปรับปรุงข้อมูลหุ้นฐานขั้นต่ำ" style="width:250px;" onclick="UpdateMinShare()" />
            </td>
        </tr>
    </table>
</asp:Content>
