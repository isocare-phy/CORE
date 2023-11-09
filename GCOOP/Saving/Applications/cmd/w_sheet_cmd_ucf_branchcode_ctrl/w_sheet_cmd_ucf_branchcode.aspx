<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_cmd_ucf_branchcode.aspx.cs" 
Inherits="Saving.Applications.cmd.w_sheet_cmd_ucf_branchcode_ctrl.w_sheet_cmd_ucf_branchcode" %>

<%@ Register Src="DsMain.ascx" TagName="DsMain" TagPrefix="uc1" %>
<%@ Register Src="DsList.ascx" TagName="DsList" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    var dsMain = new DataSourceTool();
    var dsList = new DataSourceTool();
//    function MenubarOpen() {
//        Gcoop.OpenIFrame2("600", "580", "w_dlg_dealer_search.aspx", "")
//    }

    function MenubarNew() {
        newclear();
    }

    function Validate() {
        try {
            var branch_desc = "";
            branch_desc = dsMain.GetItem(0, "branch_desc");

            if (branch_desc == null || branch_desc == " ") {
                alert("กรุณากรอกรายละเอียดสาขา");
                return;
            }
             else {
                return confirm("ยืนยันการบันทึกข้อมูล");
            }
        }
        catch (Error) { }

    }

    function SheetLoadComplete() {
    }

    function OnDsListClicked(s, r, c) {

        if (c == "b_del") {
            branch_desc = dsList.GetItem(r, "branch_desc");
            if (confirm("ต้องการลบ " + branch_desc + "  ใช่หรือไม่ ?")) {
                Postdel();
            }
        }
        if (c == "b_edit") {
            branch_desc = dsList.GetItem(r, "branch_desc");
            if (confirm("ต้องการแก้ไข " + branch_desc + "  ใช่หรือไม่ ?")) {
                Postedit();
            }
        }
    }

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <center>
    <uc1:DsMain ID="dsMain" runat="server" /></center>
    <br />
    <br />
    <center>
     <uc2:DsList ID="dsList" runat="server" /></center>
     <asp:HiddenField ID="Hdadd_status" runat="server" />
</asp:Content>
