<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="ws_sheet_as_assistpay.aspx.cs" Inherits="Saving.Applications.assist.ws_sheet_as_assistpay_ctrl.ws_sheet_as_assistpay" %>
<%@ Register Src="DsMain.ascx" TagName="DsMain" TagPrefix="uc1" %>
<%@ Register Src="DsList.ascx" TagName="DsList" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var dsMain = new DataSourceTool;
        var dsList = new DataSourceTool;

        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล");
        }

        function OnDsMainItemChanged(s, r, c, v) {
            if (c == "assisttype_code") {
                dsMain.SetItem(0, "assist_code", v);
            } else if (c == "assist_code") {
                dsMain.SetItem(0, "assisttype_code", v);
            } else if (c == "select_check") {
                if (v == 0) {
                    for (var i = 0; i < dsList.GetRowCount(); i++) {
                        dsList.SetItem(i, "choose_flag", v);
                        dsList.GetElement(i, "assist_docno").style.background = "#FFFFFF";
                        dsList.GetElement(i, "member_no").style.background = "#FFFFFF";
                        dsList.GetElement(i, "assistpay_code").style.background = "#FFFFFF";
                        dsList.GetElement(i, "assisttype_desc").style.background = "#FFFFFF";
                        dsList.GetElement(i, "full_name").style.background = "#FFFFFF";
                        dsList.GetElement(i, "approve_amt").style.background = "#FFFFFF";
                    }
                } else {
                    for (var i = 0; i < dsList.GetRowCount(); i++) {
                        dsList.SetItem(i, "choose_flag", v);
                        dsList.GetElement(i, "assist_docno").style.background = "#FFFF99";
                        dsList.GetElement(i, "member_no").style.background = "#FFFF99";
                        dsList.GetElement(i, "assistpay_code").style.background = "#FFFF99";
                        dsList.GetElement(i, "assisttype_desc").style.background = "#FFFF99";
                        dsList.GetElement(i, "full_name").style.background = "#FFFF99";
                        dsList.GetElement(i, "approve_amt").style.background = "#FFFF99";
                    }
                }
            } else if (c == "req_status") {
                PostChangeStatus();
            }
        }

        function OnDsMainClicked(s, r, c) {
            if (c == "b_clear") {
                dsMain.SetItem(0, "member_no", "");
                dsMain.SetItem(0, "assist_code", "");
                dsMain.SetItem(0, "assisttype_code", "");
                PostSearch();
            } else if (c == "b_search") {
                PostSearch();
            } else if (c == "b_searchmem") {
                Gcoop.OpenIFrame2(650, 600, 'w_dlg_sl_member_search_tks.aspx', '')
            }else  if (c == "b_pay") {
                var assist = "";
                var assist_code = "";
                var word = "";
                var assist_pay="";

                for (var i = 0; i < dsList.GetRowCount(); i++) {
                var row =dsList.GetRowCount();
               // alert(row);
                    if (dsList.GetItem(i, "choose_flag") == 1) {
                        assist = dsList.GetItem(i, "assist_docno");
                        word += ","+assist;                        
                    }
                }               
                if (word != "") {
                    word = word.substring(1, word.length);
                    //Gcoop.OpenIFrame2("800", "550", "ws_dlg_as_assistpay.aspx", "?assists=" + word);
                    Gcoop.OpenIFrame2("800", "550", "ws_dlg_as_assistpay_v2.aspx", "?assists=" + word);

                } else { alert('กรุณาเลือกใบคำร้อง!!!'); }
            }
        }

        function GetMembNoFromDlg(memberno) {
            dsMain.SetItem(0, "member_no", memberno.trim());
        }
        function GetRetrivedata() {
            PostSearch();
        }
        function OnDsDsListItemChanged(s, r, c, v) {
            if (c == "choose_flag") {
                if (v == 0) {
                    dsMain.SetItem(0, "select_check", 0);
                    dsList.SetItem(r, "choose_flag", v);
                    dsList.GetElement(r, "assist_docno").style.background = "#FFFFFF";
                    dsList.GetElement(r, "member_no").style.background = "#FFFFFF";
                    dsList.GetElement(r, "assistpay_code").style.background = "#FFFFFF";
                    dsList.GetElement(r, "assisttype_desc").style.background = "#FFFFFF";
                    dsList.GetElement(r, "full_name").style.background = "#FFFFFF";
                    dsList.GetElement(r, "approve_amt").style.background = "#FFFFFF";
                } else {
                    dsList.SetItem(r, "choose_flag", v);
                    dsList.GetElement(r, "assist_docno").style.background = "#FFFF99";
                    dsList.GetElement(r, "member_no").style.background = "#FFFF99";
                    dsList.GetElement(r, "assistpay_code").style.background = "#FFFF99";
                    dsList.GetElement(r, "assisttype_desc").style.background = "#FFFF99";
                    dsList.GetElement(r, "full_name").style.background = "#FFFF99";
                    dsList.GetElement(r, "approve_amt").style.background = "#FFFF99";
                }
            } else if (c == "req_status") {
                if (v != 8) {
                    dsList.SetItem(r, "choose_flag", 1);
                    dsList.GetElement(r, "assist_docno").style.background = "#FFFF99";
                    dsList.GetElement(r, "member_no").style.background = "#FFFF99";
                    dsList.GetElement(r, "assistpay_code").style.background = "#FFFF99";
                    dsList.GetElement(r, "assisttype_desc").style.background = "#FFFF99";
                    dsList.GetElement(r, "full_name").style.background = "#FFFF99";
                    dsList.GetElement(r, "approve_amt").style.background = "#FFFF99";
                } else {
                    dsList.SetItem(r, "choose_flag", 0);
                    dsList.GetElement(r, "assist_docno").style.background = "#FFFFFF";
                    dsList.GetElement(r, "member_no").style.background = "#FFFFFF";
                    dsList.GetElement(r, "assistpay_code").style.background = "#FFFFFF";
                    dsList.GetElement(r, "assisttype_desc").style.background = "#FFFFFF";
                    dsList.GetElement(r, "full_name").style.background = "#FFFFFF";
                    dsList.GetElement(r, "approve_amt").style.background = "#FFFFFF";
                }
            }

        }

        function OnDsListClicked(s, r, c) {
        }

        function SheetLoadComplete() {
            for (var i = 0; i < dsList.GetRowCount(); i++) {
                if (dsList.GetItem(i, "choose_flag") == 1) {
                    dsList.GetElement(i, "assist_docno").style.background = "#FFFF99";
                    dsList.GetElement(i, "member_no").style.background = "#FFFF99";
                    dsList.GetElement(i, "assistpay_code").style.background = "#FFFF99";
                    dsList.GetElement(i, "assisttype_desc").style.background = "#FFFF99";
                    dsList.GetElement(i, "full_name").style.background = "#FFFF99";
                    dsList.GetElement(i, "approve_amt").style.background = "#FFFF99";
                    dsList.GetElement(i, "req_status").style.background = "#FFFF99";
                }
            }
        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
   
    <uc1:DsMain ID="dsMain" runat="server" />
    <uc2:DsList ID="dsList" runat="server" />
</asp:Content>


