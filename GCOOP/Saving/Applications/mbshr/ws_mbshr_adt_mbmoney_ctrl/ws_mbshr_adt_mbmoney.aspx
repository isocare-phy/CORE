<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="ws_mbshr_adt_mbmoney.aspx.cs" Inherits="Saving.Applications.mbshr.ws_mbshr_adt_mbmoney_ctrl.ws_mbshr_adt_mbmoney" %>
<%@ Register Src="DsMain.ascx" TagName="DsMain" TagPrefix="uc1" %>
<%@ Register Src="DsMaster.ascx" TagName="DsMaster" TagPrefix="uc2" %>
<%@ Register Src="DsKeep.ascx" TagName="DsKeep" TagPrefix="uc3" %>
<%@ Register Src="DsDiv.ascx" TagName="DsDiv" TagPrefix="uc4" %>
<%@ Register Src="DsList.ascx" TagName="DsList" TagPrefix="uc5" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var dsMain = new DataSourceTool;
        var dsMaster = new DataSourceTool;
        var dsKeep = new DataSourceTool;
        var dsDiv = new DataSourceTool;
        var dsList = new DataSourceTool;

        function OnDsMainItemChanged(s, r, c, v) {
            if (c == "member_no") {
                PostMember();
            }
        }
            
       function OnDsMasterItemChanged(s, r, c, v) {
           if (c == "expense_bank") {
               PostExpenseBank();
            } else if (c == "expense_branch") {
                PostExpenseBranch();
            } else if (c == "bank_name") {
                PostBank();
            } else if (c == "branch_name") {
                var branch_name = dsMaster.GetItem(0, "branch_name");
                dsMaster.SetItem(0, "expense_branch", branch_name);
            } else if (c == "expense_code") {
                var expense_code = dsMaster.GetItem(0, "expense_code");
                if (expense_code == "CSH") {
                    dsMaster.GetElement(0, "expense_bank").readOnly = true;
                    dsMaster.GetElement(0, "bank_name").disabled = true;
                    dsMaster.GetElement(0, "b_bank").disabled = true;
                    dsMaster.GetElement(0, "expense_branch").readOnly = true;
                    dsMaster.GetElement(0, "branch_name").disabled = true;
                    dsMaster.GetElement(0, "b_branch").disabled = true;
                    dsMaster.GetElement(0, "bank_name").disabled = true;
                    dsMaster.GetElement(0, "expense_accid").disabled = true;
                    dsMaster.SetItem(0, "expense_bank", "");
                    dsMaster.SetItem(0, "bank_name", "");
                    dsMaster.SetItem(0, "expense_branch", "");
                    dsMaster.SetItem(0, "branch_name", "");
                    dsMaster.SetItem(0, "expense_accid", "");
                    dsMaster.GetElement(0, "expense_bank").style.background = "#CCCCCC";
                    dsMaster.GetElement(0, "bank_name").style.background = "#CCCCCC";
                    dsMaster.GetElement(0, "b_bank").style.background = "#CCCCCC";
                    dsMaster.GetElement(0, "expense_branch").style.background = "#CCCCCC";
                    dsMaster.GetElement(0, "branch_name").style.background = "#CCCCCC";
                    dsMaster.GetElement(0, "b_branch").style.background = "#CCCCCC";
                    dsMaster.GetElement(0, "expense_accid").style.background = "#CCCCCC";
                } else if (expense_code == "TRN") {
                    dsMaster.GetElement(0, "expense_bank").readOnly = true;
                    dsMaster.GetElement(0, "bank_name").disabled = true;
                    dsMaster.GetElement(0, "b_bank").disabled = true;
                    dsMaster.GetElement(0, "expense_branch").readOnly = true;
                    dsMaster.GetElement(0, "branch_name").disabled = true;
                    dsMaster.GetElement(0, "b_branch").disabled = true;
                    dsMaster.GetElement(0, "bank_name").disabled = true;
                    dsMaster.GetElement(0, "expense_accid").disabled = false;
                    dsMaster.SetItem(0, "expense_bank", "");
                    dsMaster.SetItem(0, "bank_name", "");
                    dsMaster.SetItem(0, "expense_branch", "");
                    dsMaster.SetItem(0, "branch_name", "");
                    dsMaster.GetElement(0, "expense_bank").style.background = "#CCCCCC";
                    dsMaster.GetElement(0, "bank_name").style.background = "#CCCCCC";
                    dsMaster.GetElement(0, "b_bank").style.background = "#CCCCCC";
                    dsMaster.GetElement(0, "expense_branch").style.background = "#CCCCCC";
                    dsMaster.GetElement(0, "branch_name").style.background = "#CCCCCC";
                    dsMaster.GetElement(0, "b_branch").style.background = "#CCCCCC";
                    dsMaster.GetElement(0, "expense_accid").style.background = "#FFFFFF";
                } else {
                    dsMaster.GetElement(0, "expense_bank").readOnly = false;
                    dsMaster.GetElement(0, "bank_name").disabled = false;
                    dsMaster.GetElement(0, "b_bank").disabled = false;
                    dsMaster.GetElement(0, "expense_branch").readOnly = false;
                    dsMaster.GetElement(0, "branch_name").disabled = false;
                    dsMaster.GetElement(0, "b_branch").disabled = false;
                    dsMaster.GetElement(0, "bank_name").disabled = false;
                    dsMaster.GetElement(0, "expense_accid").disabled = false;
                    dsMaster.GetElement(0, "expense_bank").style.background = "#FFFFFF";
                    dsMaster.GetElement(0, "bank_name").style.background = "#FFFFFF";
                    dsMaster.GetElement(0, "b_bank").style.background = "#FFFFFF";
                    dsMaster.GetElement(0, "expense_branch").style.background = "#FFFFFF";
                    dsMaster.GetElement(0, "branch_name").style.background = "#FFFFFF";
                    dsMaster.GetElement(0, "b_branch").style.background = "#FFFFFF";
                    dsMaster.GetElement(0, "expense_accid").style.background = "#FFFFFF";
                }
            }     
        }
        function OnDsKeepItemChanged(s, r, c, v) {
            if (c == "bank_code") {
                PostExpenseBankKeep();
            } else if (c == "bank_branch") {
                PostExpenseBranchKeep();
            } else if (c == "bank_name") {
                PostBankKeep();
            } else if (c == "branch_name") {
                var branch_name = dsKeep.GetItem(0, "branch_name");
                dsKeep.SetItem(0, "bank_branch", branch_name);
            } else if (c == "moneytype_code") {
                var expense_code = dsKeep.GetItem(0, "moneytype_code");
                if (expense_code == "CSH") {
                    dsKeep.GetElement(0, "bank_code").readOnly = true;
                    dsKeep.GetElement(0, "bank_name").disabled = true;
                    dsKeep.GetElement(0, "b_bank").disabled = true;
                    dsKeep.GetElement(0, "bank_branch").readOnly = true;
                    dsKeep.GetElement(0, "branch_name").disabled = true;
                    dsKeep.GetElement(0, "b_branch").disabled = true;
                    dsKeep.GetElement(0, "bank_name").disabled = true;
                    dsKeep.GetElement(0, "bank_accid").disabled = true;
                    dsKeep.SetItem(0, "bank_code", "");
                    dsKeep.SetItem(0, "bank_name", "");
                    dsKeep.SetItem(0, "bank_branch", "");
                    dsKeep.SetItem(0, "branch_name", "");
                    dsKeep.SetItem(0, "bank_accid", "");
                    dsKeep.GetElement(0, "bank_code").style.background = "#CCCCCC";
                    dsKeep.GetElement(0, "bank_name").style.background = "#CCCCCC";
                    dsKeep.GetElement(0, "b_bank").style.background = "#CCCCCC";
                    dsKeep.GetElement(0, "bank_branch").style.background = "#CCCCCC";
                    dsKeep.GetElement(0, "branch_name").style.background = "#CCCCCC";
                    dsKeep.GetElement(0, "b_branch").style.background = "#CCCCCC";
                    dsKeep.GetElement(0, "bank_accid").style.background = "#CCCCCC";
                } else if (expense_code == "TRN") {
                    dsKeep.GetElement(0, "bank_code").readOnly = true;
                    dsKeep.GetElement(0, "bank_name").disabled = true;
                    dsKeep.GetElement(0, "b_bank").disabled = true;
                    dsKeep.GetElement(0, "bank_branch").readOnly = true;
                    dsKeep.GetElement(0, "branch_name").disabled = true;
                    dsKeep.GetElement(0, "b_branch").disabled = true;
                    dsKeep.GetElement(0, "bank_name").disabled = true;
                    dsKeep.GetElement(0, "bank_accid").disabled = false;
                    dsKeep.SetItem(0, "bank_code", "");
                    dsKeep.SetItem(0, "bank_name", "");
                    dsKeep.SetItem(0, "bank_branch", "");
                    dsKeep.SetItem(0, "branch_name", "");
                    dsKeep.GetElement(0, "bank_code").style.background = "#CCCCCC";
                    dsKeep.GetElement(0, "bank_name").style.background = "#CCCCCC";
                    dsKeep.GetElement(0, "b_bank").style.background = "#CCCCCC";
                    dsKeep.GetElement(0, "bank_branch").style.background = "#CCCCCC";
                    dsKeep.GetElement(0, "branch_name").style.background = "#CCCCCC";
                    dsKeep.GetElement(0, "b_branch").style.background = "#CCCCCC";
                    dsKeep.GetElement(0, "bank_accid").style.background = "#FFFFFF";
                } else if (expense_code == "SAL") {
                    dsKeep.GetElement(0, "bank_code").readOnly = true;
                    dsKeep.GetElement(0, "bank_name").disabled = true;
                    dsKeep.GetElement(0, "b_bank").disabled = true;
                    dsKeep.GetElement(0, "bank_branch").readOnly = true;
                    dsKeep.GetElement(0, "branch_name").disabled = true;
                    dsKeep.GetElement(0, "b_branch").disabled = true;
                    dsKeep.GetElement(0, "bank_name").disabled = true;
                    dsKeep.GetElement(0, "bank_accid").disabled = true;
                    dsKeep.SetItem(0, "bank_code", "");
                    dsKeep.SetItem(0, "bank_name", "");
                    dsKeep.SetItem(0, "bank_branch", "");
                    dsKeep.SetItem(0, "branch_name", "");
                    dsKeep.SetItem(0, "bank_accid", "");
                    dsKeep.GetElement(0, "bank_code").style.background = "#CCCCCC";
                    dsKeep.GetElement(0, "bank_name").style.background = "#CCCCCC";
                    dsKeep.GetElement(0, "b_bank").style.background = "#CCCCCC";
                    dsKeep.GetElement(0, "bank_branch").style.background = "#CCCCCC";
                    dsKeep.GetElement(0, "branch_name").style.background = "#CCCCCC";
                    dsKeep.GetElement(0, "b_branch").style.background = "#CCCCCC";
                    dsKeep.GetElement(0, "bank_accid").style.background = "#CCCCCC";
                }                
                else {
                    dsKeep.GetElement(0, "bank_code").readOnly = false;
                    dsKeep.GetElement(0, "bank_name").disabled = false;
                    dsKeep.GetElement(0, "b_bank").disabled = false;
                    dsKeep.GetElement(0, "bank_branch").readOnly = false;
                    dsKeep.GetElement(0, "branch_name").disabled = false;
                    dsKeep.GetElement(0, "b_branch").disabled = false;
                    dsKeep.GetElement(0, "bank_name").disabled = false;
                    dsKeep.GetElement(0, "bank_accid").disabled = false;
                    dsKeep.GetElement(0, "bank_code").style.background = "#FFFFFF";
                    dsKeep.GetElement(0, "bank_name").style.background = "#FFFFFF";
                    dsKeep.GetElement(0, "b_bank").style.background = "#FFFFFF";
                    dsKeep.GetElement(0, "bank_branch").style.background = "#FFFFFF";
                    dsKeep.GetElement(0, "branch_name").style.background = "#FFFFFF";
                    dsKeep.GetElement(0, "b_branch").style.background = "#FFFFFF";
                    dsKeep.GetElement(0, "bank_accid").style.background = "#FFFFFF";
                }
            }
        }
        function OnDsDivItemChanged(s, r, c, v) {
            if (c == "bank_code") {
                PostExpenseBankDiv();
            } else if (c == "bank_branch") {
                PostExpenseBranchDiv();
            } else if (c == "bank_name") {
                PostBankDiv();
            } else if (c == "branch_name") {
                var branch_name = dsDiv.GetItem(0, "branch_name");
                dsDiv.SetItem(0, "bank_branch", branch_name);
            } else if (c == "moneytype_code") {
                var expense_code = dsDiv.GetItem(0, "moneytype_code");
                if ((expense_code == "CSH") || (expense_code =="SHR") || (expense_code =="SQA") || (expense_code =="SQC")|| (expense_code == "SQL")) {
                    dsDiv.GetElement(0, "bank_code").readOnly = true;
                    dsDiv.GetElement(0, "bank_name").disabled = true;
                    dsDiv.GetElement(0, "b_bank").disabled = true;
                    dsDiv.GetElement(0, "bank_branch").readOnly = true;
                    dsDiv.GetElement(0, "branch_name").disabled = true;
                    dsDiv.GetElement(0, "b_branch").disabled = true;
                    dsDiv.GetElement(0, "bank_name").disabled = true;
                    dsDiv.GetElement(0, "bank_accid").disabled = true;
                    dsDiv.SetItem(0, "bank_code", "");
                    dsDiv.SetItem(0, "bank_name", "");
                    dsDiv.SetItem(0, "bank_branch", "");
                    dsDiv.SetItem(0, "branch_name", "");
                    dsDiv.SetItem(0, "bank_accid", "");
                    dsDiv.GetElement(0, "bank_code").style.background = "#CCCCCC";
                    dsDiv.GetElement(0, "bank_name").style.background = "#CCCCCC";
                    dsDiv.GetElement(0, "b_bank").style.background = "#CCCCCC";
                    dsDiv.GetElement(0, "bank_branch").style.background = "#CCCCCC";
                    dsDiv.GetElement(0, "branch_name").style.background = "#CCCCCC";
                    dsDiv.GetElement(0, "b_branch").style.background = "#CCCCCC";
                    dsDiv.GetElement(0, "bank_accid").style.background = "#CCCCCC";
                } else if (expense_code == "DEP") {
                    dsDiv.GetElement(0, "bank_code").readOnly = true;
                    dsDiv.GetElement(0, "bank_name").disabled = true;
                    dsDiv.GetElement(0, "b_bank").disabled = true;
                    dsDiv.GetElement(0, "bank_branch").readOnly = true;
                    dsDiv.GetElement(0, "branch_name").disabled = true;
                    dsDiv.GetElement(0, "b_branch").disabled = true;
                    dsDiv.GetElement(0, "bank_name").disabled = true;
                    dsDiv.GetElement(0, "bank_accid").disabled = false;
                    dsDiv.SetItem(0, "bank_code", "");
                    dsDiv.SetItem(0, "bank_name", "");
                    dsDiv.SetItem(0, "bank_branch", "");
                    dsDiv.SetItem(0, "branch_name", "");
                    dsDiv.GetElement(0, "bank_code").style.background = "#CCCCCC";
                    dsDiv.GetElement(0, "bank_name").style.background = "#CCCCCC";
                    dsDiv.GetElement(0, "b_bank").style.background = "#CCCCCC";
                    dsDiv.GetElement(0, "bank_branch").style.background = "#CCCCCC";
                    dsDiv.GetElement(0, "branch_name").style.background = "#CCCCCC";
                    dsDiv.GetElement(0, "b_branch").style.background = "#CCCCCC";
                    dsDiv.GetElement(0, "bank_accid").style.background = "#FFFFFF";
                } else {
                    dsDiv.GetElement(0, "bank_code").readOnly = false;
                    dsDiv.GetElement(0, "bank_name").disabled = false;
                    dsDiv.GetElement(0, "b_bank").disabled = false;
                    dsDiv.GetElement(0, "bank_branch").readOnly = false;
                    dsDiv.GetElement(0, "branch_name").disabled = false;
                    dsDiv.GetElement(0, "b_branch").disabled = false;
                    dsDiv.GetElement(0, "bank_name").disabled = false;
                    dsDiv.GetElement(0, "bank_accid").disabled = false;
                    dsDiv.SetItem(0, "bank_code", "");
                    dsDiv.SetItem(0, "bank_name", "");
                    dsDiv.SetItem(0, "bank_branch", "");
                    dsDiv.SetItem(0, "branch_name", "");
                    dsDiv.SetItem(0, "bank_accid", "");
                    dsDiv.GetElement(0, "bank_code").style.background = "#FFFFFF";
                    dsDiv.GetElement(0, "bank_name").style.background = "#FFFFFF";
                    dsDiv.GetElement(0, "b_bank").style.background = "#FFFFFF";
                    dsDiv.GetElement(0, "bank_branch").style.background = "#FFFFFF";
                    dsDiv.GetElement(0, "branch_name").style.background = "#FFFFFF";
                    dsDiv.GetElement(0, "b_branch").style.background = "#FFFFFF";
                    dsDiv.GetElement(0, "bank_accid").style.background = "#FFFFFF";
                }
            } else if (c == "acc_flag") {
                //PostSetAccDiv();
                //bee
                if (dsDiv.GetItem(0, "acc_flag") == 1) {
                    dsDiv.GetElement(0, "moneytype_code").disabled = true;
                    dsDiv.GetElement(0, "bank_code").readOnly = true;
                    dsDiv.GetElement(0, "bank_name").disabled = true;
                    dsDiv.GetElement(0, "b_bank").disabled = true;
                    dsDiv.GetElement(0, "bank_branch").readOnly = true;
                    dsDiv.GetElement(0, "branch_name").disabled = true;
                    dsDiv.GetElement(0, "b_branch").disabled = true;
                    dsDiv.GetElement(0, "bank_name").disabled = true;
                    dsDiv.GetElement(0, "bank_accid").disabled = true;
                    dsDiv.SetItem(0, "bank_code", "");
                    dsDiv.SetItem(0, "bank_name", "");
                    dsDiv.SetItem(0, "bank_branch", "");
                    dsDiv.SetItem(0, "branch_name", "");
                    dsDiv.SetItem(0, "bank_accid", "");
                    dsDiv.SetItem(0, "moneytype_code", "");
                    dsDiv.GetElement(0, "bank_code").style.background = "#CCCCCC";
                    dsDiv.GetElement(0, "bank_name").style.background = "#CCCCCC";
                    dsDiv.GetElement(0, "b_bank").style.background = "#CCCCCC";
                    dsDiv.GetElement(0, "bank_branch").style.background = "#CCCCCC";
                    dsDiv.GetElement(0, "branch_name").style.background = "#CCCCCC";
                    dsDiv.GetElement(0, "b_branch").style.background = "#CCCCCC";
                    dsDiv.GetElement(0, "bank_accid").style.background = "#CCCCCC";
                } else {
                    dsDiv.GetElement(0, "bank_code").readOnly = false;
                    dsDiv.GetElement(0, "bank_name").disabled = false;
                    dsDiv.GetElement(0, "b_bank").disabled = false;
                    dsDiv.GetElement(0, "bank_branch").readOnly = false;
                    dsDiv.GetElement(0, "branch_name").disabled = false;
                    dsDiv.GetElement(0, "b_branch").disabled = false;
                    dsDiv.GetElement(0, "bank_name").disabled = false;
                    dsDiv.GetElement(0, "bank_accid").disabled = false;
                    dsDiv.GetElement(0, "moneytype_code").disabled = false;
                    dsDiv.SetItem(0, "bank_code", "");
                    dsDiv.SetItem(0, "bank_name", "");
                    dsDiv.SetItem(0, "bank_branch", "");
                    dsDiv.SetItem(0, "branch_name", "");
                    dsDiv.GetElement(0, "bank_code").style.background = "#FFFFFF";
                    dsDiv.GetElement(0, "bank_name").style.background = "#FFFFFF";
                    dsDiv.GetElement(0, "b_bank").style.background = "#FFFFFF";
                    dsDiv.GetElement(0, "bank_branch").style.background = "#FFFFFF";
                    dsDiv.GetElement(0, "branch_name").style.background = "#FFFFFF";
                    dsDiv.GetElement(0, "b_branch").style.background = "#FFFFFF";
                    dsDiv.GetElement(0, "bank_accid").style.background = "#FFFFFF";            
                }
            }
        }
        function OnDsListItemChanged(s, r, c, v) {
            if (dsList.GetItem(r, "moneytype_code") == "TRN" || dsList.GetItem(r, "moneytype_code") == "TBK" || dsList.GetItem(r, "moneytype_code") == "CSH") {
                dsList.SetItem(r, "bank_code", "00");
                dsList.SetItem(r, "bank_branch", "");
                dsList.SetItem(r, "branch_name", "");
                dsList.GetElement(r, "bank_code").disabled = true;
                dsList.GetElement(r, "bank_branch").disabled = true;                             
                dsList.GetElement(r, "branch_name").disabled = true;
                dsList.GetElement(r, "bank_code").style.background = "#CCCCCC";
                dsList.GetElement(r, "bank_branch").style.background = "#CCCCCC";
                dsList.GetElement(r, "branch_name").style.background = "#CCCCCC"; 
                if (dsList.GetItem(r, "moneytype_code") == "CSH") {
                    dsList.SetItem(r, "bank_accid", "");
                    dsList.GetElement(r, "bank_accid").disabled = true;
                    dsList.GetElement(r, "bank_accid").style.background = "#CCCCCC";
                }
            } else {
                dsList.GetElement(r, "bank_code").disabled = false;
                dsList.GetElement(r, "bank_branch").disabled = false;
                dsList.GetElement(r, "branch_name").disabled = false;
                dsList.GetElement(r, "bank_code").style.background = "#FFFFFF";
                dsList.GetElement(r, "bank_branch").style.background = "#FFFFFF";
                dsList.GetElement(r, "branch_name").style.background = "#FFFFFF";
                dsList.GetElement(r, "bank_accid").disabled = false;
                dsList.GetElement(r, "bank_accid").style.background = "#FFFFFF";
                
            }
        
        }
        function OnDsMainClicked(s, r, c, v) {
            if (c == "b_search") {
                Gcoop.OpenIFrame("610", "590", "w_dlg_sl_member_search.aspx", "");
            } else if (c == "b_bank") {
                Gcoop.OpenIFrame("580", "590", "w_dlg_kp_bank_search.aspx", "");
            }
        }
        function OnDsMasterClicked(s, r, c, v) {
            Gcoop.GetEl("hdSearch").value = "main";
            if (c == "b_bank") {
                Gcoop.OpenIFrame("580", "590", "w_dlg_kp_bank_search.aspx", "");
            } else if (c == "b_branch") {
                var bank_code = dsMaster.GetItem(0, "expense_bank");
                Gcoop.OpenIFrame("580", "590", "w_dlg_kp_bankbranch_search.aspx", "?bank_code=" + bank_code);
            }
        }
        function OnDsKeepClicked(s, r, c, v) {
            Gcoop.GetEl("hdSearch").value = "keep";
            if (c == "b_bank") {
                Gcoop.OpenIFrame("580", "590", "w_dlg_kp_bank_search.aspx", "");
            } else if (c == "b_branch") {
                var bank_code = dsKeep.GetItem(0, "bank_code");
                Gcoop.OpenIFrame("580", "590", "w_dlg_kp_bankbranch_search.aspx", "?bank_code=" + bank_code);
            }
        }
        function OnDsDivClicked(s, r, c, v) {
            Gcoop.GetEl("hdSearch").value = "div";
            if (c == "b_bank") {
                Gcoop.OpenIFrame("580", "590", "w_dlg_kp_bank_search.aspx", "");
            } else if (c == "b_branch") {
                var bank_code = dsDiv.GetItem(0, "bank_code");
                Gcoop.OpenIFrame("580", "590", "w_dlg_kp_bankbranch_search.aspx", "?bank_code=" + bank_code);
            }
        }
        function OnDsListClicked(s, r, c, v) {
            if (c == "b_del") {
                dsList.SetRowFocus(r);
                PostDeleteRow();
            }
        }
        function GetValueFromDlg(memberno) {
            dsMain.SetItem(0, "member_no", memberno);
            PostMember();
        }
        function GetBankFromDlg(bank_code) {
            if (Gcoop.GetEl("hdSearch").value == "main") {
                dsMaster.SetItem(0, "expense_bank", bank_code);
                dsMaster.SetItem(0, "bank_name", bank_code);
                PostExpenseBank();
            } else if (Gcoop.GetEl("hdSearch").value == "keep") {
                dsKeep.SetItem(0, "bank_code", bank_code);
                dsKeep.SetItem(0, "bank_name", bank_code);
                PostExpenseBankKeep();
            } else if (Gcoop.GetEl("hdSearch").value == "div") {
                dsDiv.SetItem(0, "bank_code", bank_code);
                dsDiv.SetItem(0, "bank_name", bank_code);
                PostExpenseBankDiv();
            }            
        }
        function GetBankBranchFromDlg(branch_id) {              
            if (Gcoop.GetEl("hdSearch").value == "main") {
                dsMaster.SetItem(0, "expense_branch", branch_id);
                dsMaster.SetItem(0, "branch_name", branch_id);
            } else if (Gcoop.GetEl("hdSearch").value == "keep") {
                dsKeep.SetItem(0, "bank_branch", branch_id);
                dsKeep.SetItem(0, "branch_name", branch_id);               
            } else if (Gcoop.GetEl("hdSearch").value == "div") {
                dsDiv.SetItem(0, "bank_branch", branch_id);
                dsDiv.SetItem(0, "branch_name", branch_id);
            }
        }
        function Validate() {
                return confirm("ยืนยันการบันทึกข้อมูล");
        }

        function SheetLoadComplete() {
            var expense_code = dsMaster.GetItem(0, "expense_code");
            if (expense_code == "CSH") {
                dsMaster.GetElement(0, "expense_bank").readOnly = true;
                dsMaster.GetElement(0, "bank_name").disabled = true;
                dsMaster.GetElement(0, "b_bank").disabled = true;
                dsMaster.GetElement(0, "expense_branch").readOnly = true;
                dsMaster.GetElement(0, "branch_name").disabled = true;
                dsMaster.GetElement(0, "b_branch").disabled = true;
                dsMaster.GetElement(0, "bank_name").disabled = true;
                dsMaster.GetElement(0, "expense_accid").disabled = true;
                dsMaster.SetItem(0, "expense_bank", "");
                dsMaster.SetItem(0, "bank_name", "");
                dsMaster.SetItem(0, "expense_branch", "");
                dsMaster.SetItem(0, "branch_name", "");
                dsMaster.SetItem(0, "expense_accid", "");
                dsMaster.GetElement(0, "expense_bank").style.background = "#CCCCCC";
                dsMaster.GetElement(0, "bank_name").style.background = "#CCCCCC";
                dsMaster.GetElement(0, "b_bank").style.background = "#CCCCCC";
                dsMaster.GetElement(0, "expense_branch").style.background = "#CCCCCC";
                dsMaster.GetElement(0, "branch_name").style.background = "#CCCCCC";
                dsMaster.GetElement(0, "b_branch").style.background = "#CCCCCC";
                dsMaster.GetElement(0, "expense_accid").style.background = "#CCCCCC";
            } else if (expense_code == "TRN") {
                dsMaster.GetElement(0, "expense_bank").readOnly = true;
                dsMaster.GetElement(0, "bank_name").disabled = true;
                dsMaster.GetElement(0, "b_bank").disabled = true;
                dsMaster.GetElement(0, "expense_branch").readOnly = true;
                dsMaster.GetElement(0, "branch_name").disabled = true;
                dsMaster.GetElement(0, "b_branch").disabled = true;
                dsMaster.GetElement(0, "bank_name").disabled = true;
                dsMaster.GetElement(0, "expense_accid").disabled = false;
                dsMaster.SetItem(0, "expense_bank", "");
                dsMaster.SetItem(0, "bank_name", "");
                dsMaster.SetItem(0, "expense_branch", "");
                dsMaster.SetItem(0, "branch_name", "");
                dsMaster.GetElement(0, "expense_bank").style.background = "#CCCCCC";
                dsMaster.GetElement(0, "bank_name").style.background = "#CCCCCC";
                dsMaster.GetElement(0, "b_bank").style.background = "#CCCCCC";
                dsMaster.GetElement(0, "expense_branch").style.background = "#CCCCCC";
                dsMaster.GetElement(0, "branch_name").style.background = "#CCCCCC";
                dsMaster.GetElement(0, "b_branch").style.background = "#CCCCCC";
                dsMaster.GetElement(0, "expense_accid").style.background = "#FFFFFF";
            } else {
                dsMaster.GetElement(0, "expense_bank").readOnly = false;
                dsMaster.GetElement(0, "bank_name").disabled = false;
                dsMaster.GetElement(0, "b_bank").disabled = false;
                dsMaster.GetElement(0, "expense_branch").readOnly = false;
                dsMaster.GetElement(0, "branch_name").disabled = false;
                dsMaster.GetElement(0, "b_branch").disabled = false;
                dsMaster.GetElement(0, "bank_name").disabled = false;
                dsMaster.GetElement(0, "expense_accid").disabled = false;
                dsMaster.GetElement(0, "expense_bank").style.background = "#FFFFFF";
                dsMaster.GetElement(0, "bank_name").style.background = "#FFFFFF";
                dsMaster.GetElement(0, "b_bank").style.background = "#FFFFFF";
                dsMaster.GetElement(0, "expense_branch").style.background = "#FFFFFF";
                dsMaster.GetElement(0, "branch_name").style.background = "#FFFFFF";
                dsMaster.GetElement(0, "b_branch").style.background = "#FFFFFF";
                dsMaster.GetElement(0, "expense_accid").style.background = "#FFFFFF";
            }
            var expense_codediv = dsDiv.GetItem(0, "moneytype_code");
            if ((expense_codediv == "CSH") || (expense_codediv == "SHR") || (expense_codediv == "SQA") || (expense_codediv == "SQC") || (expense_codediv == "SQL")) {
                dsDiv.GetElement(0, "bank_code").readOnly = true;
                dsDiv.GetElement(0, "bank_name").disabled = true;
                dsDiv.GetElement(0, "b_bank").disabled = true;
                dsDiv.GetElement(0, "bank_branch").readOnly = true;
                dsDiv.GetElement(0, "branch_name").disabled = true;
                dsDiv.GetElement(0, "b_branch").disabled = true;
                dsDiv.GetElement(0, "bank_name").disabled = true;
                dsDiv.GetElement(0, "bank_accid").disabled = true;
                dsDiv.SetItem(0, "bank_code", "");
                dsDiv.SetItem(0, "bank_name", "");
                dsDiv.SetItem(0, "bank_branch", "");
                dsDiv.SetItem(0, "branch_name", "");
                dsDiv.SetItem(0, "bank_accid", "");
                dsDiv.GetElement(0, "bank_code").style.background = "#CCCCCC";
                dsDiv.GetElement(0, "bank_name").style.background = "#CCCCCC";
                dsDiv.GetElement(0, "b_bank").style.background = "#CCCCCC";
                dsDiv.GetElement(0, "bank_branch").style.background = "#CCCCCC";
                dsDiv.GetElement(0, "branch_name").style.background = "#CCCCCC";
                dsDiv.GetElement(0, "b_branch").style.background = "#CCCCCC";
                dsDiv.GetElement(0, "bank_accid").style.background = "#CCCCCC";
            } else if (expense_codediv == "DEP") {
                dsDiv.GetElement(0, "bank_code").readOnly = true; 
                dsDiv.GetElement(0, "bank_name").disabled = true;
                dsDiv.GetElement(0, "b_bank").disabled = true;
                dsDiv.GetElement(0, "bank_branch").readOnly = true;
                dsDiv.GetElement(0, "branch_name").disabled = true;
                dsDiv.GetElement(0, "b_branch").disabled = true;
                dsDiv.GetElement(0, "bank_name").disabled = true;
                dsDiv.GetElement(0, "bank_accid").disabled = false;
                dsDiv.SetItem(0, "bank_code", "");
                dsDiv.SetItem(0, "bank_name", "");
                dsDiv.SetItem(0, "bank_branch", "");
                dsDiv.SetItem(0, "branch_name", "");
                dsDiv.GetElement(0, "bank_code").style.background = "#CCCCCC";
                dsDiv.GetElement(0, "bank_name").style.background = "#CCCCCC";
                dsDiv.GetElement(0, "b_bank").style.background = "#CCCCCC";
                dsDiv.GetElement(0, "bank_branch").style.background = "#CCCCCC";
                dsDiv.GetElement(0, "branch_name").style.background = "#CCCCCC";
                dsDiv.GetElement(0, "b_branch").style.background = "#CCCCCC";
                dsDiv.GetElement(0, "bank_accid").style.background = "#FFFFFF";
            } else {
                dsDiv.GetElement(0, "bank_code").readOnly = false;
                dsDiv.GetElement(0, "bank_name").disabled = false;
                dsDiv.GetElement(0, "b_bank").disabled = false;
                dsDiv.GetElement(0, "bank_branch").readOnly = false;
                dsDiv.GetElement(0, "branch_name").disabled = false;
                dsDiv.GetElement(0, "b_branch").disabled = false;
                dsDiv.GetElement(0, "bank_name").disabled = false;
                dsDiv.GetElement(0, "bank_accid").disabled = false;
                dsDiv.GetElement(0, "bank_code").style.background = "#FFFFFF";
                dsDiv.GetElement(0, "bank_name").style.background = "#FFFFFF";
                dsDiv.GetElement(0, "b_bank").style.background = "#FFFFFF";
                dsDiv.GetElement(0, "bank_branch").style.background = "#FFFFFF";
                dsDiv.GetElement(0, "branch_name").style.background = "#FFFFFF";
                dsDiv.GetElement(0, "b_branch").style.background = "#FFFFFF";
                dsDiv.GetElement(0, "bank_accid").style.background = "#FFFFFF";
            }

            if (dsDiv.GetItem(0, "acc_flag") == 1) {
                dsDiv.GetElement(0, "moneytype_code").disabled = true;
                dsDiv.GetElement(0, "bank_code").readOnly = true;
                dsDiv.GetElement(0, "bank_name").disabled = true;
                dsDiv.GetElement(0, "b_bank").disabled = true;
                dsDiv.GetElement(0, "bank_branch").readOnly = true;
                dsDiv.GetElement(0, "branch_name").disabled = true;
                dsDiv.GetElement(0, "b_branch").disabled = true;
                dsDiv.GetElement(0, "bank_name").disabled = true;
                dsDiv.GetElement(0, "bank_accid").disabled = true;
                dsDiv.SetItem(0, "bank_code", "");
                dsDiv.SetItem(0, "bank_name", "");
                dsDiv.SetItem(0, "bank_branch", "");
                dsDiv.SetItem(0, "branch_name", "");
                dsDiv.SetItem(0, "bank_accid", "");
                dsDiv.SetItem(0, "moneytype_code", "");
                dsDiv.GetElement(0, "bank_code").style.background = "#CCCCCC";
                dsDiv.GetElement(0, "bank_name").style.background = "#CCCCCC";
                dsDiv.GetElement(0, "b_bank").style.background = "#CCCCCC";
                dsDiv.GetElement(0, "bank_branch").style.background = "#CCCCCC";
                dsDiv.GetElement(0, "branch_name").style.background = "#CCCCCC";
                dsDiv.GetElement(0, "b_branch").style.background = "#CCCCCC";
                dsDiv.GetElement(0, "bank_accid").style.background = "#CCCCCC";
            }

            var expense_codekeep = dsKeep.GetItem(0, "moneytype_code");
            if (expense_codekeep == "CSH") {
                dsKeep.GetElement(0, "bank_code").readOnly = true;
                dsKeep.GetElement(0, "bank_name").disabled = true;
                dsKeep.GetElement(0, "b_bank").disabled = true;
                dsKeep.GetElement(0, "bank_branch").readOnly = true;
                dsKeep.GetElement(0, "branch_name").disabled = true;
                dsKeep.GetElement(0, "b_branch").disabled = true;
                dsKeep.GetElement(0, "bank_name").disabled = true;
                dsKeep.GetElement(0, "bank_accid").disabled = true;
                dsKeep.SetItem(0, "bank_code", "");
                dsKeep.SetItem(0, "bank_name", "");
                dsKeep.SetItem(0, "bank_branch", "");
                dsKeep.SetItem(0, "branch_name", "");
                dsKeep.SetItem(0, "bank_accid", "");
                dsKeep.GetElement(0, "bank_code").style.background = "#CCCCCC";
                dsKeep.GetElement(0, "bank_name").style.background = "#CCCCCC";
                dsKeep.GetElement(0, "b_bank").style.background = "#CCCCCC";
                dsKeep.GetElement(0, "bank_branch").style.background = "#CCCCCC";
                dsKeep.GetElement(0, "branch_name").style.background = "#CCCCCC";
                dsKeep.GetElement(0, "b_branch").style.background = "#CCCCCC";
                dsKeep.GetElement(0, "bank_accid").style.background = "#CCCCCC";
            } else if (expense_codekeep == "TRN") {
                dsKeep.GetElement(0, "bank_code").readOnly = true;
                dsKeep.GetElement(0, "bank_name").disabled = true;
                dsKeep.GetElement(0, "b_bank").disabled = true;
                dsKeep.GetElement(0, "bank_branch").readOnly = true;
                dsKeep.GetElement(0, "branch_name").disabled = true;
                dsKeep.GetElement(0, "b_branch").disabled = true;
                dsKeep.GetElement(0, "bank_name").disabled = true;
                dsKeep.GetElement(0, "bank_accid").disabled = false;
                dsKeep.SetItem(0, "bank_code", "");
                dsKeep.SetItem(0, "bank_name", "");
                dsKeep.SetItem(0, "bank_branch", "");
                dsKeep.SetItem(0, "branch_name", "");
                dsKeep.GetElement(0, "bank_code").style.background = "#CCCCCC";
                dsKeep.GetElement(0, "bank_name").style.background = "#CCCCCC";
                dsKeep.GetElement(0, "b_bank").style.background = "#CCCCCC";
                dsKeep.GetElement(0, "bank_branch").style.background = "#CCCCCC";
                dsKeep.GetElement(0, "branch_name").style.background = "#CCCCCC";
                dsKeep.GetElement(0, "b_branch").style.background = "#CCCCCC";
                dsKeep.GetElement(0, "bank_accid").style.background = "#FFFFFF";
            } else if (expense_codekeep == "SAL") {
                dsKeep.GetElement(0, "bank_code").readOnly = true;
                dsKeep.GetElement(0, "bank_name").disabled = true;
                dsKeep.GetElement(0, "b_bank").disabled = true;
                dsKeep.GetElement(0, "bank_branch").readOnly = true;
                dsKeep.GetElement(0, "branch_name").disabled = true;
                dsKeep.GetElement(0, "b_branch").disabled = true;
                dsKeep.GetElement(0, "bank_name").disabled = true;
                dsKeep.GetElement(0, "bank_accid").disabled = true;
                dsKeep.SetItem(0, "bank_code", "");
                dsKeep.SetItem(0, "bank_name", "");
                dsKeep.SetItem(0, "bank_branch", "");
                dsKeep.SetItem(0, "branch_name", "");
                dsKeep.SetItem(0, "bank_accid", "");
                dsKeep.GetElement(0, "bank_code").style.background = "#CCCCCC";
                dsKeep.GetElement(0, "bank_name").style.background = "#CCCCCC";
                dsKeep.GetElement(0, "b_bank").style.background = "#CCCCCC";
                dsKeep.GetElement(0, "bank_branch").style.background = "#CCCCCC";
                dsKeep.GetElement(0, "branch_name").style.background = "#CCCCCC";
                dsKeep.GetElement(0, "b_branch").style.background = "#CCCCCC";
                dsKeep.GetElement(0, "bank_accid").style.background = "#CCCCCC";
            } else {
                dsKeep.GetElement(0, "bank_code").readOnly = false;
                dsKeep.GetElement(0, "bank_name").disabled = false;
                dsKeep.GetElement(0, "b_bank").disabled = false;
                dsKeep.GetElement(0, "bank_branch").readOnly = false;
                dsKeep.GetElement(0, "branch_name").disabled = false;
                dsKeep.GetElement(0, "b_branch").disabled = false;
                dsKeep.GetElement(0, "bank_name").disabled = false;
                dsKeep.GetElement(0, "bank_accid").disabled = false;
                dsKeep.GetElement(0, "bank_code").style.background = "#FFFFFF";
                dsKeep.GetElement(0, "bank_name").style.background = "#FFFFFF";
                dsKeep.GetElement(0, "b_bank").style.background = "#FFFFFF";
                dsKeep.GetElement(0, "bank_branch").style.background = "#FFFFFF";
                dsKeep.GetElement(0, "branch_name").style.background = "#FFFFFF";
                dsKeep.GetElement(0, "b_branch").style.background = "#FFFFFF";
                dsKeep.GetElement(0, "bank_accid").style.background = "#FFFFFF";
            }
            
            
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>   
    <table class="DataSourceFormView">
        <tr>
            <td colspan="3">
                <uc1:DsMain ID="dsMain" runat="server" />
            </td>
        </tr>   
       <tr>
            <td style="width:240px;">
                <uc2:DsMaster ID="dsMaster" runat="server"/>
            </td>
            <td style="width:240px;">
                <uc3:DsKeep ID="dsKeep" runat="server" />
            </td>
            <td style="width:240px;">
                <uc4:DsDiv ID="dsDiv" runat="server" />
            </td>
        </tr>  
    </table>
    <table>
        <tr>
            <td>
                <div align="right">
                    <span class="NewRowLink" onclick="PostInsertRow()">เพิ่มแถว</span>
                </div>
                <uc5:DsList ID="dsList" runat="server" />
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hdSearch" Value="0" runat="server" />
</asp:Content>
