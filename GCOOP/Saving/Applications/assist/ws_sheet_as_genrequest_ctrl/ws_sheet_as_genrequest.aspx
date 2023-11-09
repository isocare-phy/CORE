<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="ws_sheet_as_genrequest.aspx.cs" Inherits="Saving.Applications.assist.ws_sheet_as_genrequest_ctrl.ws_sheet_as_genrequest" %>

<%@ Register Src="DsMain.ascx" TagName="DsMain" TagPrefix="uc1" %>
<%@ Register Src="DsList.ascx" TagName="DsList" TagPrefix="uc2" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var dsMain = new DataSourceTool;
        var dsList = new DataSourceTool;
        
        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล");
        }

        function OnDsMainClicked(s, r, c, v) {
            if (c == "b_process") {
                if (dsMain.GetItem(0, "process_month") == "1") { dsMain.SetItem(0, "process_month", "01"); }
                var chk_assistcode = dsMain.GetItem(0, "assisttype_code");
                if (chk_assistcode == "00" || chk_assistcode == null) {
                    alert("กรุณาเลือก ประเภทสวัสดิการ!!!"); return;
                } else {
                    PostProcess(); 
                }
            } else if (c == "b_save") {                
                if (confirm("ยืนยันการบันทึก")) {
                    PostSave();
                }
            }
        }
        function OnDsMainItemChanged(s, r, c, v) {
            if (c == "all_check") {
                for (var ii = 0; ii < dsList.GetRowCount(); ii++) {
                    dsList.SetItem(ii, "choose_flag", v);
                }
            } else if (c == "moneytype_code") {
                if (v == "CSH") {
                    dsMain.SetItem(0, "expense_bank", '');
                    dsMain.GetElement(0, "expense_bank").style.background = "#CCCCCC";
                    dsMain.GetElement(0, "expense_bank").readOnly = true;
                    dsMain.SetItem(0, "expense_branch", '');
                    dsMain.GetElement(0, "expense_branch").style.background = "#CCCCCC";
                    dsMain.GetElement(0, "expense_branch").readOnly = true;
                    PostDefaultAcc();
                } else {
                    dsMain.GetElement(0, "expense_bank").style.background = "#FFFFFF";
                    dsMain.GetElement(0, "expense_bank").readOnly = false;
                    dsMain.SetItem(0, "expense_branch", '');
                    dsMain.GetElement(0, "expense_branch").style.background = "#FFFFFF";
                    dsMain.GetElement(0, "expense_branch").readOnly = false;
                    PostBank();
                }
            } else if (c == "expense_bank") {
                dsMain.SetItem(0, "expense_branch", '');
                PostBranch();
            }
        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
   
    <uc1:DsMain ID="dsMain" runat="server" />
    <uc2:DsList ID="dsList" runat="server" />
    
</asp:Content>
