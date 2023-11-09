<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_cmd_ucf_invtlitemcode.aspx.cs" 
Inherits="Saving.Applications.cmd.w_sheet_cmd_ucf_invtlitemcode_ctrl.w_sheet_cmd_ucf_invtlitemcode" %>

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
            var item_des = "";
            item_des = dsMain.GetItem(0, "item_des");

            if (item_des == null || item_des == " ") {
                alert("กรุณากรอกรายละเอียดการเคลื่อนไหวให้ถูกต้อง");
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
            item_des = dsList.GetItem(r, "item_des");
            if (confirm("ต้องการลบ " + item_des + "  ใช่หรือไม่ ?")) {
                Postdel();
            }
        }
        if (c == "b_edit") {
            item_des = dsList.GetItem(r, "item_des");
            if (confirm("ต้องการแก้ไข " + item_des + "  ใช่หรือไม่ ?")) {
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
