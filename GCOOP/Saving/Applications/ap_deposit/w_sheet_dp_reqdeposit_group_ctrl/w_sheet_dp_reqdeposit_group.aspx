<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_dp_reqdeposit_group.aspx.cs" Inherits="Saving.Applications.ap_deposit.w_sheet_dp_reqdeposit_group_ctrl.w_sheet_dp_reqdeposit_group" %>
<%@ Register Src="DsMain.ascx" TagName="DsMain" TagPrefix="uc1" %>
<%@ Register Src="DsList.ascx" TagName="DsList" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        //ประกาศตัวแปร ควรอยู่บริเวณบนสุดใน tag <script>
        var dsMain = new DataSourceTool();
        var dsList = new DataSourceTool();
        ////////////////////////

        //ประกาศฟังก์ชันสำหรับ event ItemChanged
        function OnDsMainItemChanged(s, r, c, v) {
            if (c == "all_check") {
                for (var ii = 0; ii < dsList.GetRowCount(); ii++) {
                    dsList.SetItem(ii, "choose_flag", v);
                }
            }
        }

        //ประกาศฟังก์ชันสำหรับ event Clicked
        function OnDsMainClicked(s, r, c) {

            if (c == "b_save") {
                var ck_value = false;
                for (var ii = 0; ii < dsList.GetRowCount(); ii++) {
                    if (dsList.GetItem(ii, "choose_flag") == "1") {
                        ck_value = true;
                    }
                }                
                if (ck_value == true) {
                    if (confirm("ยืนยันการเปิดบัญชี")) {
                        JsPosSave();
                    }
                } else {
                    alert("กรุณาเลือกข้อมูลการเปิดบัญชี!!");
                    return;
                }
            }      
        }
        
        function Validate() {
            
        }

        function MenubarOpen() {
        }

    </script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">    
   <uc1:DsMain ID="dsMain" runat="server" />
   <uc2:DsList ID="dsList" runat="server" />    
</asp:Content>




