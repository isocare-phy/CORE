<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_cmd_ucf_unitcode.aspx.cs" 
Inherits="Saving.Applications.cmd.w_sheet_cmd_ucf_unitcode_ctrl.w_sheet_cmd_ucf_unitcode" %>

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

    //    function Validate() {
    //        return confirm("ยืนยันการบันทึกข้อมูล");
    //    }

    function Validate() {
        try {
            var unit_desc = "";
            unit_desc = dsMain.GetItem(0, "unit_desc");

            if (unit_desc == null || unit_desc == " ") {
                alert("กรุณากรอกชื่อหน่วยวัสดุ");
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

//        function OnDsMainClicked(s, r, c, v) {
//            if (c == "b_edit") {

//                if (confirm("ต้องการแก้ไขข้อมูลใช่หรือไม่")) {
//                    dsList.SetRowFocus(r);
//                    Postedit();
//                }
//            }
//        }

    function OnDsListClicked(s, r, c) {

        if (c == "b_edit") {
            unit_desc = dsList.GetItem(r, "unit_desc");
            if (confirm("ต้องการแก้ไขข้อมูล " + unit_desc + "  ใช่หรือไม่ ?")) {             
                Postedit();
            }
        }
        if (c == "b_del") {
            unit_desc = dsList.GetItem(r, "unit_desc");
            if (confirm("ต้องการลบ " + unit_desc + "  ใช่หรือไม่ ?")) {
                Postdel();
            }
        }
    }

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <center>
    <uc1:DsMain ID="dsMain" runat="server" /> </center>
    <br />
    <br />
    <center>
     <uc2:DsList ID="dsList" runat="server" /></center>
      <asp:HiddenField ID="Hdadd_status" runat="server" />
</asp:Content>
