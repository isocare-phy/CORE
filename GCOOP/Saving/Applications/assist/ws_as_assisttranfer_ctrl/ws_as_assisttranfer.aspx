<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="ws_as_assisttranfer.aspx.cs" Inherits="Saving.Applications.assist.ws_as_assisttranfer_ctrl.ws_as_assisttranfer" %>
<%@ Register Src="DsList.ascx" TagName="DsList" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">  
    <script type="text/javascript">


        var dsList = new DataSourceTool();

        function Validate() {
            var isconfirm = confirm("ยืนยันการบันทึกข้อมูล?");

            if (!isconfirm) {
                return false;
            }
            var alert = "";
            var accbudgetgroup_typ_to = dsList.GetItem(0, "assisttype_to");
            var accbudgetgroup_typ_from = dsList.GetItem(0, "assisttype_from");
            var transfer_amt = dsList.GetItem(0, "transfer_amt");
            var approve_date = dsList.GetItem(0, "entry_date");
   
            if (accbudgetgroup_typ_to == "" || accbudgetgroup_typ_to == null) {
                alert += "กรุณาระบุสวัดิการที่รับโอน\n";
            }
            if (accbudgetgroup_typ_from == "" || accbudgetgroup_typ_from == null) {
                alert += "กรุณาระบุสวัสดิการที่ต้องการโอนย้าย\n";
            }
            if (transfer_amt == "" || transfer_amt == null) {
                alert += "กรุณาระบุยอดที่ต้องการย้าย\n";
            }
            if (approve_date == "" || approve_date == null) {
                alert += "กรุณาระบุวันที่ทำรายการ\n";
            }

            if (alert != "") {
                confirm(alert);
                return false;
            }
            else {
                return true;
            }
        }

        function SheetLoadComplete() {
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <uc1:DsList ID="dsList" runat="server" />
    <asp:HiddenField ID="Hd_Year" runat="server" />
</asp:Content>


