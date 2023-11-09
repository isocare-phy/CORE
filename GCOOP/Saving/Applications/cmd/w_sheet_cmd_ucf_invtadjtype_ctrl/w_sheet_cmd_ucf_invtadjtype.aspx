<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_cmd_ucf_invtadjtype.aspx.cs" 
Inherits="Saving.Applications.cmd.w_sheet_cmd_ucf_invtadjtype_ctrl.w_sheet_cmd_ucf_invtadjtype" %>

<%@ Register Src="DsMain.ascx" TagName="DsMain" TagPrefix="uc1" %>
<%@ Register Src="DsList.ascx" TagName="DsList" TagPrefix="uc2" %>
<%@ Register Src="DsList2.ascx" TagName="DsList2" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    var dsMain = new DataSourceTool();
    var dsList = new DataSourceTool();
    var dsList2 = new DataSourceTool();
//    function MenubarOpen() {
//        Gcoop.OpenIFrame2("600", "580", "w_dlg_dealer_search.aspx", "")
//    }

    function MenubarNew() {
        newclear();
    }

    function Validate() {
        try {
            var adjtype_desc = "";
            adjtype_desc = dsMain.GetItem(0, "adjtype_desc");

            if (adjtype_desc == null || adjtype_desc == " ") {
                alert("กรุณากรอกรายละเอียดการปรับปรุงให้ถูกต้อง");
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
            adjtype_desc = dsList.GetItem(r, "adjtype_desc");
            if (confirm("ต้องการลบ " + adjtype_desc + "  ใช่หรือไม่ ?")) {
                Postdel();
            }
        }
        if (c == "b_edit") {
            adjtype_desc = dsList.GetItem(r, "adjtype_desc");
            if (confirm("ต้องการแก้ไข " + adjtype_desc + "  ใช่หรือไม่ ?")) {
                Postedit();
            }
        }
    }

    function OnDsList2Clicked(s, r, c) {

        if (c == "b_del") {
            adjtype_desc = dsList2.GetItem(r, "adjtype_desc");
            if (confirm("ต้องการลบ " + adjtype_desc + "  ใช่หรือไม่ ?")) {
                Postdelete();
            }
        }
        if (c == "b_edit") {
            adjtype_desc = dsList2.GetItem(r, "adjtype_desc");
            if (confirm("ต้องการแก้ไข " + adjtype_desc + "  ใช่หรือไม่ ?")) {
                Posteditlist();
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
     <br />
    <br />
    <center>
     <uc2:DsList2 ID="dsList2" runat="server" /></center>
     <asp:HiddenField ID="Hdadd_status" runat="server" />
</asp:Content>
