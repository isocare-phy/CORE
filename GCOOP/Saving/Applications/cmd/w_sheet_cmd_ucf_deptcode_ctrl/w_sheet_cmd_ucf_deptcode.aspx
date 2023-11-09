<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_cmd_ucf_deptcode.aspx.cs" 
Inherits="Saving.Applications.cmd.w_sheet_cmd_ucf_deptcode_ctrl.w_sheet_cmd_ucf_deptcode" %>

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
            var dept_desc = "", dept_abb = "";
            dept_desc = dsMain.GetItem(0, "dept_desc");
            dept_abb = dsMain.GetItem(0, "dept_abb");
            if (dept_desc == null || dept_desc == " ") {
                alert("กรุณากรอกชื่อแผนก");
                return;
            }
            else if (dept_abb == null || dept_abb == "") {
                alert("กรุณากรอกตัวย่อแผนก");
                return;
            } else {
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
            dept_desc = dsList.GetItem(r, "dept_desc");
            if (confirm("ต้องการลบ " + dept_desc + "  ใช่หรือไม่ ?")) {
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
