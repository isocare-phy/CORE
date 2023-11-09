<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_cmd_ucf_durtgrpcode.aspx.cs" 
Inherits="Saving.Applications.cmd.w_sheet_cmd_ucf_durtgrpcode_ctrl.w_sheet_cmd_ucf_durtgrpcode" %>

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
            var durtgrp_desc = "", devalue_percent = "", durtgrp_abb = "";
            durtgrp_desc = dsMain.GetItem(0, "durtgrp_desc");
            devalue_percent = dsMain.GetItem(0, "devalue_percent");
            durtgrp_abb = dsMain.GetItem(0, "durtgrp_abb");
            if (durtgrp_desc == null || durtgrp_desc == " ") {
                alert("กรุณากรอกหมวดครุภัณฑ์");
                return;
            }
            else if (durtgrp_abb == null || durtgrp_abb == "") {
                alert("กรุณากรอกตัวย่อครุภัณฑ์");
                return;
            }
            else if (devalue_percent <= 0) {
                alert("กรุณากรอกค่าเสื่อมให้ถูกต้อง");
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

//    function OnDsMainClicked(s, r, c, v) {
//        if (c == "b_edit") {
//            if (confirm("ต้องการลบข้อมูลใช่หรือไม่")) {
//                dsList.SetRowFocus(r);
//                Postedit();
//            }
//        }
//    }

    function OnDsListClicked(s, r, c) {
        if (c == "b_edit") {
            Postedit();
        }
        if (c == "b_del") {
            durtgrp_desc = dsList.GetItem(r, "durtgrp_desc");
            if (confirm("ต้องการลบ " + durtgrp_desc + "  ใช่หรือไม่ ?")) {
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
