<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_sl_loan_check_loanreq.aspx.cs" Inherits="Saving.Applications.shrlon.w_sheet_sl_loan_check_loanreq" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript %>
    <%=jsGetMemberInfo%>
    <%=jsReNewPage%>
    <%=jsOpenOldDocNo%>
    <%=jsSetpriod%>
    <%=jsRefresh%>
    <script type="text/javascript">

        function MenubarNew() {
            jsReNewPage();
        }

        function MenubarOpen() {
            Gcoop.OpenDlg('1000', '1000', 'w_dlg_sl_loanrequest_search.aspx', '');
        }

        //w_dlg_sl_loanrequest_search
        function GetValueLoanRequest(docNo, coop_id) {
            objdw_main.SetItem(1, "loanrequest_docno", docNo);
            objdw_main.SetItem(1, "coop_id", coop_id);
            //txt_reqNo.Text = docNo;
            objdw_main.AcceptText();
            jsOpenOldDocNo();
        }

        function Validate() {
            }

        //**************** Start. 1.dw_main  ****************

        //Event----->ClientEventItemChanged="ItemDwMainChanged"
        function ItemDwMainChanged(sender, rowNumber, columnName, newValue) {
            if ((columnName == "loantype_code") || (columnName == "loantype_code_1")) {
                objdw_main.SetItem(rowNumber, columnName, newValue);
                objdw_main.AcceptText();
                var member_no;
                var member_name;
                try {
                    member_no = objdw_main.GetItem(rowNumber, "member_no");
                    if (member_no == null) {
                        member_no = "null";
                        //jsSetloantype();
                        jsSetloantypechg();
                    }
                } catch (Error) { }

                try {
                    member_name = objdw_main.GetItem(rowNumber, "member_name");
                    if (member_name == null) {
                        member_name = "null";
                        //jsSetloantype();
                        jsSetloantypechg();
                    }
                } catch (Error) { }


                 if (member_no != "null" && member_name != "null") {

                    jsGetMemberInfo();
                }

            }  else if (columnName == "member_no") {
                objdw_main.SetItem(rowNumber, columnName, newValue);
                objdw_main.AcceptText();
                Gcoop.GetEl("HdMemberNo").value = newValue;
                var memcoop_id = objdw_main.GetItem(rowNumber, "memcoop_id");
                var loantype_code = objdw_main.GetItem(rowNumber, "loantype_code");
                if (memcoop_id != null || memcoop_id != "" || loantype_code == "" || loantype_code != null) {
                    jsGetMemberInfo();
                }
            } else if (columnName == "memcoop_id" || columnName == "memcoop_id_1") {
                objdw_main.SetItem(rowNumber, columnName, newValue);
                objdw_main.AcceptText();
                Gcoop.GetEl("HdMemcoopId").value = newValue;


            } else if (columnName == "membtype_code") {
                objdw_main.SetItem(rowNumber, columnName, newValue);
                objdw_main.AcceptText();
                Gcoop.GetEl("Hdmembtype_code").value = newValue;
                jsSetloantype();

            } 
        }

        function OnDwMainClicked(sender, rowNumber, objectName) {
            Gcoop.GetEl("HdReturn").value = rowNumber + "";
            if ((objectName == "loanrcvfix_flag") || (objectName == "clearloan_flag") || (objectName == "otherclr_flag") || (objectName == "custompayment_flag")) {
                if (objectName == "loanrcvfix_flag") {
                    Gcoop.CheckDw(sender, rowNumber, objectName, "loanrcvfix_flag", 1, 0); // Edit By Bank เรื่อง การ set วันที่ระบุวันจ่ายเงินกู้
                    jsSetFixdate();
                }
                if (objectName == "clearloan_flag") {
                    Gcoop.CheckDw(sender, rowNumber, objectName, "clearloan_flag", 1, 0);
                    var clearloan_flag = objdw_main.GetItem(1, "clearloan_flag");
                    if (clearloan_flag == 1) {
                        jsSumOthClr();
                    }
                    else if (clearloan_flag == 0) {
                        objdw_main.SetItem(1, "sum_clear", 0);
                        objdw_main.AcceptText();
                        jsRefresh();

                    }
                }
                if (objectName == "otherclr_flag") {
                    Gcoop.CheckDw(sender, rowNumber, objectName, "otherclr_flag", 1, 0);
                    var otherclr_flag = objdw_main.GetItem(1, "otherclr_flag");
                    if (otherclr_flag == 1) {
                        jsSumOthClr();
                    }
                    else if (otherclr_flag == 0) {
                        objdw_main.SetItem(1, "otherclr_amt", 0);
                        objdw_main.AcceptText();
                        jsRefresh();

                    }
                }

                if (objectName == "custompayment_flag") {
                    Gcoop.CheckDw(sender, rowNumber, objectName, "custompayment_flag", 1, 0);
                    jsRefresh();
                }

            } else if (objectName == "b_cancel") {
                var loanreqDocNo = objdw_main.GetItem(1, "loanrequest_docno");
                if ((loanreqDocNo != "") && (loanreqDocNo != null) && (loanreqDocNo != "Auto")) {
                    jsCancelRequest();
                }
                else {
                    alert("ไม่สามารถยกเลิกรายการได้ กรุณาตรวจสอบข้อมูลใหม่อีกครั้ง");
                }

            } else if (objectName == "b_inttab") {
                //ของ กสท. ไม่แสดงค่าตัวนี้
                Gcoop.OpenDlg('600', '400', 'w_dlg_sl_loanrequest_intratespc.aspx', '');
            } else if ((objectName == "b_expense_branch") || (objectName == "expense_branch_1" && (objdw_main.GetItem(1, "retrive_bk_branchflag") == 0))) {
                var expense_bank = objdw_main.GetItem(1, "expense_bank");

                if (expense_bank != "") {
                    // objdw_main.Setitem(1, "retrive_bk_branchflag", 1);
                    jsExpensebankbrRetrieve();
                }

            } else if (objectName == "b_permiss") {
                objdw_main.AcceptText();
                var memno = objdw_main.GetItem(1, "member_no")
                var ref_contmastno = objdw_main.GetItem(1, "ref_contmastno")
                var loangrpcredit_amt = objdw_main.GetItem(1, "loangrpcredit_amt")
                var loangrpuse_amt = objdw_main.GetItem(1, "loangrpuse_amt")
                var loancredit_amt = objdw_main.GetItem(1, "loancredit_amt")
                var loanmaxreq_amt = objdw_main.GetItem(1, "loanmaxreq_amt")
                var loanpermiss_amt = objdw_main.GetItem(1, "loanpermiss_amt")
                var maxperiod_payamt = objdw_main.GetItem(1, "maxsend_payamt")
                var maxperiod_payment = objdw_main.GetItem(1, "maxperiod_payamt")
                var loantype_code = objdw_main.GetItem(1, "loantype_code")
                Gcoop.OpenDlg(620, 450, "w_dlg_sl_loanrequest_permiss.aspx", "?member_no=" + memno + "&ref_contmastno=" + ref_contmastno + "&loangrpcredit_amt=" + loangrpcredit_amt + "&loangrpuse_amt=" + loangrpuse_amt + "&loancredit_amt=" + loancredit_amt + "&loanmaxreq_amt=" + loanmaxreq_amt + "&loanpermiss_amt=" + loanpermiss_amt + "&maxperiod_payamt=" + maxperiod_payamt + "&maxperiod_payment=" + maxperiod_payment + "&loantype_code=" + loantype_code);
                return
            }
            else if (objectName == "b_search") {
                Gcoop.GetEl("HdColumnName").value = objectName;
                var coop_id = objdw_main.GetItem(rowNumber, "memcoop_id")
                Gcoop.OpenDlg('600', '600', 'w_dlg_sl_loanmember_search.aspx', "?coopId=" + coop_id);
            } else if (objectName == "b_mthpay") {
                var memberNo = objdw_main.GetItem(1, "member_no");
                if ((memberNo != "") && (memberNo != null) && (memberNo != "00000000")) {
                    var member_no = objdw_main.GetItem(1, "member_no");
                    var income = objdw_main.GetItem(1, "incomemonth_fixed");
                    var income2 = objdw_main.GetItem(1, "incomemonth_other");
                    var paymonth = objdw_main.GetItem(1, "paymonth_other");
                    var salary_amt = objdw_main.GetItem(1, "salary_amt");
                    var paymonth_coop_2 = objdw_main.GetItem(1, "paymonth_coop_2");
                    var principal_balance = Gcoop.GetEl("HdBalance").value;
                    var period_payment = objdw_main.GetItem(1, "period_payment");
                    var intest_amt = objdw_main.GetItem(1, "intestimate_amt");
                    var loanpayment_type = objdw_main.GetItem(1, "loanpayment_type");
                    var minsalary_amt = objdw_main.GetItem(1, "minsalary_amt");
                    if (loanpayment_type == 2) {
                        paymonth_coop_2 = paymonth_coop_2 + period_payment;
                    } else {
                        paymonth_coop_2 = paymonth_coop_2 + period_payment + intest_amt;
                    }
                    income = income + income2;
                    // Gcoop.OpenIFrame(550, 450, "w_dlg_sl_loanrequest_monthpay.aspx", "?income=" + income + "&paymonth=" + paymonth + "&member_no=" + member_no);
                    Gcoop.OpenIFrame(550, 550, "w_dlg_sl_loanrequest_monthpay.aspx", "?income=" + income + "&paymonth=" + paymonth + "&member_no=" + member_no + "&salary_amt=" + salary_amt + "&paymonth_coop_2=" + paymonth_coop_2 + "&principal_balance=" + principal_balance + "&minsalary_amt=" + minsalary_amt);
                    return 0;

                }
                else {
                    alert("ไม่สามารถแสดงรายการได้ กรุณากรอกเลขทะเบียนก่อน");
                }

            } else if ((objectName == "b_showetcstatus")) {

                objdw_main.AcceptText();
                var memno = objdw_main.GetItem(1, "member_no")

                Gcoop.OpenDlg(620, 450, "w_dlg_sl_lnreqloan_show_etcstatus.aspx", "?member_no=" + memno);
            } else if (objectName == "b_permiss") {
                objdw_main.AcceptText();
                var memno = objdw_main.GetItem(1, "member_no")
                var ref_contmastno = objdw_main.GetItem(1, "ref_contmastno")
                var loangrpcredit_amt = objdw_main.GetItem(1, "loangrpcredit_amt")
                var loangrpuse_amt = objdw_main.GetItem(1, "loangrpuse_amt")
                var loancredit_amt = objdw_main.GetItem(1, "loancredit_amt")
                var loanmaxreq_amt = objdw_main.GetItem(1, "loanmaxreq_amt")
                var loanpermiss_amt = objdw_main.GetItem(1, "loanpermiss_amt")
                var maxperiod_payamt = objdw_main.GetItem(1, "maxsend_payamt")
                var maxperiod_payment = objdw_main.GetItem(1, "maxperiod_payamt")
                var loantype_code = objdw_main.GetItem(1, "loantype_code")
                Gcoop.OpenDlg(620, 450, "w_dlg_sl_loanrequest_permiss.aspx", "?member_no=" + memno + "&ref_contmastno=" + ref_contmastno + "&loangrpcredit_amt=" + loangrpcredit_amt + "&loangrpuse_amt=" + loangrpuse_amt + "&loancredit_amt=" + loancredit_amt + "&loanmaxreq_amt=" + loanmaxreq_amt + "&loanpermiss_amt=" + loanpermiss_amt + "&maxperiod_payamt=" + maxperiod_payamt + "&maxperiod_payment=" + maxperiod_payment + "&loantype_code=" + loantype_code);
                return
            }
            else if (objectName == "b_retrive") {

                jsGetexpensememno();

            } 
        }
        function GetValueAccID(dept_no, deptaccount_name, prncbal) {
            var colunmName = Gcoop.GetEl("HdColumnName").value;
            var rowNumber = Gcoop.GetEl("HdRowNumber").value;
            if (colunmName == "b_searchc") {

                // alert(prncbal);
                objdw_coll.SetItem(rowNumber, "ref_collno", dept_no);
                objdw_coll.SetItem(rowNumber, "description", deptaccount_name);
                objdw_coll.SetItem(rowNumber, "coll_amt", prncbal);
                objdw_coll.SetItem(rowNumber, "coll_balance", prncbal);
                objdw_coll.SetItem(rowNumber, "use_amt", prncbal);
                Gcoop.GetEl("HUseamt").value = prncbal;
                objdw_coll.AcceptText();
                jsPostreturn();
            }
            else if (colunmName == "b_chgaccid") {
                objdw_main.SetItem(1, "expense_accid", dept_no);

                objdw_main.AcceptText();
            } else if (colunmName == "b_deptother") {

                objdw_otherclr.SetItem(rowNumber, "clrother_desc", dept_no);

                objdw_otherclr.AcceptText();
            } else {
                objdw_main.SetItem(1, "expense_accid", dept_no);

                objdw_main.AcceptText();
            }

        }
        function GetAccIDDept(dept_no, deptaccount_name, prncbal) {
            var rowNumber = Gcoop.GetEl("HdRowNumber").value;
            objdw_coll.SetItem(rowNumber, "ref_collno", dept_no);
            objdw_coll.SetItem(rowNumber, "description", deptaccount_name);
            objdw_coll.SetItem(rowNumber, "coll_balance", prncbal);
            objdw_coll.SetItem(rowNumber, "use_amt", prncbal);
            Gcoop.GetEl("HUseamt").value = prncbal;
            objdw_coll.AcceptText();
            jsPostreturn();


        }
        //**************** End. 1.dw_main  ****************


        //**************** Start. 2.dw_coll  ค้ำประกัน ****************

        //Event----->ClientEventItemChanged="ItemDwCollChanged"
        function ItemDwCollChanged(sender, rowNumber, columnName, newValue) {
            Gcoop.GetEl("HdRowNumber").value = rowNumber + "";
            if (columnName == "ref_collno") {

                objdw_coll.SetItem(rowNumber, columnName, newValue);
                Gcoop.GetEl("HdRefcoll").value = newValue;
                Gcoop.GetEl("HdRefcollrow").value = rowNumber;

                objdw_coll.AcceptText();
                var loancolltype_code = objdw_coll.GetItem(rowNumber, "loancolltype_code");
                if (Gcoop.GetEl("HdRefcoll").value == Gcoop.GetEl("HdMemberNo").value && loancolltype_code == "01") {
                    //Gcoop.GetEl("HdRefcoll").value = "";
                    alert("เลขทะเบียนผู้กู้และผู้ค้ำประกันเป็นเลขเดียวกัน");
                }
                    if (loancolltype_code == "01") {

                        jsGetMemberCollno();
                    }
                    else if (loancolltype_code == "03") {
                        // alert(newValue);
                        if ((newValue != null) && (newValue != "")) {
                            Gcoop.OpenDlg(620, 250, "w_dlg_show_accid_dept.aspx", "?dept=" + newValue);
                        }

                    }
                //}

            } else if (columnName == "loancolltype_code") {
                objdw_coll.SetItem(rowNumber, columnName, newValue);
                objdw_coll.AcceptText();
                var loancolltype_code = objdw_coll.GetItem(rowNumber, "loancolltype_code");
                Gcoop.GetEl("HdRefcollrow").value = rowNumber + "";
                if (loancolltype_code == "02") {
                    jsGetMemberCollno();
                }else if (loancolltype_code == "04") {
                    objdw_coll.SetItem(rowNumber, "ref_collno", "Auto");
                    objdw_coll.AcceptText();
                }

            }
            else if (columnName == "use_amt") {
                objdw_coll.SetItem(rowNumber, columnName, newValue);
                objdw_coll.AcceptText();
            }
            else if (columnName == "coll_balance") {
                objdw_coll.SetItem(rowNumber, columnName, newValue);
                objdw_coll.AcceptText();
                jsRefresh();
            } else if (columnName == "coll_lockamt") {
                objdw_coll.SetItem(rowNumber, columnName, newValue);
                objdw_coll.AcceptText();
                jsRefresh();
            }
            else if (columnName == "coll_percent") {
                objdw_coll.SetItem(rowNumber, columnName, newValue);
                objdw_coll.AcceptText();
            }
            else if (columnName == "calcollright_amt") {
                objdw_coll.SetItem(rowNumber, columnName, newValue);
                objdw_coll.AcceptText();
            }
        }


        function OnDwCollClicked(sender, rowNumber, buttonName) {
            var collTypeCode = objdw_coll.GetItem(rowNumber, "loancolltype_code");
            Gcoop.GetEl("HdReturn").value = rowNumber + "";
            Gcoop.GetEl("HdRowNumber").value = rowNumber + "";
            var memberNoVal = objdw_main.GetItem(1, "member_no");
            if (buttonName == "b_delrow") {
                objdw_coll.DeleteRow(rowNumber);
            }
            else if (buttonName == "b_detail") {

                var collNo = objdw_coll.GetItem(rowNumber, "ref_collno");
                //  var requestDate = objdw_main.GetItem(1, "loanrequest_tdate");
                if ((collNo != "") && (collNo != null)) {
                    var loanmemno = objdw_main.GetItem(rowNumber, "member_no");
                    var refCollNo = objdw_coll.GetItem(rowNumber, "ref_collno");
                    var coop_id = objdw_coll.GetItem(rowNumber, "memcoop_id");
                    var collType = objdw_coll.GetItem(rowNumber, "loancolltype_code");
                    var coll_amt = objdw_coll.GetItem(rowNumber, "coll_amt");
                    var coll_use = objdw_coll.GetItem(rowNumber, "use_amt");
                    var coll_blance = objdw_coll.GetItem(rowNumber, "coll_balance");
                    var base_percent = objdw_coll.GetItem(rowNumber, "base_percent");
                    var description = objdw_coll.GetItem(rowNumber, "description");
                    var loancolltype_code = objdw_coll.GetItem(rowNumber, "loancolltype_code");
                    Gcoop.OpenDlg('700', '450', 'w_dlg_sl_loanrequest_coll.aspx', "?refCollNo=" + refCollNo + "&coop_id=" + coop_id + "&coll_amt=" + coll_amt + "&coll_use=" + coll_use + "&coll_blance=" + coll_blance + "&collType=" + collType + "&description=" + description + "&loancolltype_code=" + loancolltype_code + "&base_percent= " + base_percent + "&row=" + rowNumber + "&loanmemno=" + loanmemno);
                    return;
                }
                else {
                    alert("ไม่สามารถแสดงรายการได้ กรุณากรอกเลขที่อ้างอิงก่อน");
                }
            } else if ((buttonName == "b_searchc") && (collTypeCode == '01')) {

                Gcoop.GetEl("HdColumnName").value = buttonName;
                var coop_id = objdw_coll.GetItem(rowNumber, "coop_id");
                if (coop_id == null || coop_id == "") {
                    // ค้นหาคนค้ำประกัน
                    var memcoop_id = objdw_main.GetItem(1, "memcoop_id");
                    Gcoop.OpenDlg('600', '600', 'w_dlg_sl_loanmember_search.aspx', "?coopId=" + memcoop_id);
                }
                else {
                    Gcoop.OpenDlg('600', '600', 'w_dlg_sl_loanmember_search.aspx', "?coopId=" + coop_id);
                }

            }
            else if ((buttonName == "b_searchc") && (collTypeCode == '03')) {
                // ค้นหาบัญชีเงินฝาก 

                // Gcoop.OpenDlg('600', '450', 'w_dlg_dp_account_search.aspx', '');
                Gcoop.OpenDlg(620, 250, "w_dlg_dp_account_search.aspx", "?member=" + memberNoVal);
            }
            else if ((buttonName == "b_searchc") && (collTypeCode == '04')) {
                //ค้นหาหลักทรัพย์ค้ำประกัน
                var refCollNo2 = Gcoop.GetEl("HdMemberNo").value;
                var loantype_code = objdw_main.GetItem(1, "loantype_code");
                Gcoop.OpenDlg('600', '450', 'w_dlg_sl_collmaster_search_req.aspx', "?member=" + memberNoVal + "&loantype_code=" + loantype_code);
            } else if ((buttonName == "b_searchc2") && (collTypeCode == '04')) {
                //ค้นหาหลักทรัพย์ค้ำประกัน
                var refCollNo2 = Gcoop.GetEl("HdMemberNo").value;
                var loantype_code = objdw_main.GetItem(1, "loantype_code");
                Gcoop.OpenDlg('600', '450', 'w_dlg_sl_collmaster_search_mb_.aspx', "?member=" + memberNoVal + "&loantype_code=" + loantype_code);
            }
            else if (buttonName == "b_addrow") {
                objdw_coll.InsertRow(objdw_coll.RowCount() + 1);
            }
            else if (buttonName == "b_recoll") {
                jsCollInitP();
            }
            else if (buttonName == "b_condition") {
                jsCollCondition();
            } else if (buttonName == "b_renew") {

            }

        }

        //**************** End. 2.dw_coll  ****************



        //**************** Start. 3.dw_otherclr  ****************
        //Event----->ClientEventItemChanged="ItemDwOtherclrChanged"
        function ItemDwOtherclrChanged(sender, rowNumber, columnName, newValue) {
            if (columnName == "clrother_amt") {
                objdw_otherclr.SetItem(rowNumber, "clrother_amt", newValue);
                objdw_otherclr.AcceptText();
                jsSumOthClr();
            }
        }



        //End. 3.dw_otherclr ****************


        //**************** Start. 4.dw_clear ****************
        //Event----->ClientEventItemChanged="ItemDwClearChanged"
        function ItemDwClearChanged(sender, rowNumber, columnName, newValue) {
            if (columnName == "principal_balance") {
                objdw_clear.SetItem(rowNumber, "principal_balance", newValue);
                objdw_clear.AcceptText();
                var clear_status = objdw_clear.GetItem(rowNumber, "clear_status");
                Gcoop.GetEl("Hdprincipal").value = newValue;
                if (clear_status == 1) {
                    jsSumOthClr();
                }
            }
        }

        //****************End. 4.dw_clear ****************

        //### รับค่า return กลับมาของ Dlg ###############################

        //w_dlg_sl_loanrequest_cleardet.aspx
        function GetValueDlgClearLoan() {
            jsResumLoanClear();
        }

        //w_dlg_sl_loanmember_search
        function GetValueFromDlgloanMemberSearch(memberno) {
            //ส่งกลับจาก หน้าค้นหาสมาชิก
            var rowNumber = Gcoop.GetEl("HdRowNumber").value;
            var colunmName = Gcoop.GetEl("HdColumnName").value;

            if (colunmName == "b_searchc") {
                objdw_coll.SetItem(rowNumber, "ref_collno", memberno);
                objdw_coll.AcceptText();
                Gcoop.GetEl("HdRefcoll").value = memberno;
                Gcoop.GetEl("HdRefcollrow").value = rowNumber + "";
                jsGetMemberCollno();
            }
            else if (colunmName == "b_search") {
                objdw_main.SetItem(1, "member_no", memberno);
                Gcoop.GetEl("HdMemberNo").value = memberno;
                objdw_main.AcceptText();
                jsGetMemberInfo();
            }
        }

        //w_dlg_dp_account_search
        function NewAccountNo(dept_no, deptaccount_name, prncbal) {
            var rowNumber = Gcoop.GetEl("HdRowNumber").value;
            objdw_coll.SetItem(rowNumber, "ref_collno", dept_no);
            objdw_coll.SetItem(rowNumber, "description", deptaccount_name);
            objdw_coll.SetItem(rowNumber, "coll_balance", prncbal);
            objdw_coll.SetItem(rowNumber, "use_amt", prncbal);
            Gcoop.GetEl("HUseamt").value = prncbal;
            objdw_coll.AcceptText();
            jsPostreturn();

        }
        //w_dlg_sl_collmaster_search_req
        function GetValueFromDlgCollmast(collRefNo, collmast_desc, mortgage_price) {
            if (collmast_desc == null || collmast_desc == "") {
                collmast_desc = "";
            }
            var desc = collRefNo + ":" + collmast_desc;
            var rowNumber = Gcoop.GetEl("HdRowNumber").value;
            objdw_coll.SetItem(rowNumber, "ref_collno", Gcoop.GetEl("HdMemberNo").value);
            objdw_coll.SetItem(rowNumber, "description", collRefNo);
            objdw_coll.SetItem(rowNumber, "coll_balance", mortgage_price);
            objdw_coll.SetItem(rowNumber, "use_amt", (mortgage_price * 90) / 100);
            Gcoop.GetEl("HUseamt").value = (mortgage_price * 90) / 100;
            Gcoop.GetEl("HdRefcollNO").value = collRefNo;
            objdw_coll.AcceptText();
            jsPostreturn();
        }
     
         function OnClickPrintCollPerRow() {

            objdw_main.AcceptText();
           // alert("tes");
            //var loanreqDocNo = document.getElementById('txt_reqNo').value;
            var loanreqDocNo = Gcoop.GetEl("HdLnreqdoc_no").value; // = collRefNo;// objdw_main.GetItem(1, "loanrequest_docno"); //"RQ56000503";// 
            //alert("ttt");
            Gcoop.OpenDlg('550', '300', 'w_dlg_lnreqloan_printcoll.aspx', "?loanrequest_docno=" + loanreqDocNo);
                
        }
    

        //*************************************************************************************
        function showarrhist() {
            var member = Gcoop.GetEl("HdMemberNo").value;

            Gcoop.OpenIFrame("760", "300", "w_dlg_check_loan_arrear.aspx", "?MemberNo=" + member);
        
        }
        function collmastclick() {
            Gcoop.GetEl("HdReturn").value = "";
            Gcoop.GetEl("HdColumnName").value = "";
            Gcoop.OpenDlg(720, 200, "w_dlg_sl_loanrequest_loanrightchoose.aspx", "");

        }
        function ConfrimCollMemer(check_status) {
            
            if (check_status == -9) {
                var row = Gcoop.GetEl("HdRefcollrow").value;
                var rowNumber = Gcoop.GetEl("HdRowNumber").value;
                objdw_coll.SetItem(rowNumber, "ref_collno", "");
                objdw_coll.SetItem(rowNumber, "description", "");
                
            }
           // parent.RemoveIFrame();
        }
        function SheetLoadComplete() {
            if (Gcoop.GetEl("HdIsPostBack").value != "true") {

                Gcoop.SetLastFocus("member_no_0");
                Gcoop.Focus();
            }
            //&& (Gcoop.GetEl("HdShowRemark").value = "true") 
            var CheckRemark = Gcoop.GetEl("HdCheckRemark").value;
           
            if (CheckRemark  == "true") {
                var member = Gcoop.GetEl("HdMemberNo").value;
                Gcoop.GetEl("HdShowRemark").value = "false";
                Gcoop.GetEl("HdCheckRemark").value = "false"
                
                Gcoop.OpenIFrame("760", "300", "w_dlg_ln_remarkstatus.aspx", "?MemberNo=" + member);
                
            }

           

            var returnVal = Gcoop.GetEl("HdReturn").value;
            var columnVal = Gcoop.GetEl("HdColumnName").value;
            var msgVal = Gcoop.GetEl("HdMsg").value;
            var msgWarning = Gcoop.GetEl("HdMsgWarning").value;
            var memberNoVal = Gcoop.GetEl("HdMemberNo").value;
            var openIframe = Gcoop.GetEl("HdCheck").value;
            var confirmcoll = Gcoop.GetEl("Hdcollconfirm").value;

            if (openIframe == "true") {
                var openedF = Gcoop.GetEl("HdOpened").value;
                if (openedF != "1") {
                    Gcoop.GetEl("HdOpened").value = "1";
                    var member_no = objdw_main.GetItem(1, "member_no");
                    var punishgroup_code = "(1000,3000,5000)";
                    Gcoop.OpenIFrame(720, 400, "w_iframe_punish_detail.aspx", "?member_no=" + member_no + "&punishgroup_code=" + punishgroup_code);
                }
            }
           
            if (returnVal == 8 && columnVal == "genbaseloancredit") {
                Gcoop.GetEl("HdReturn").value = "";
                Gcoop.GetEl("HdColumnName").value = "";
                Gcoop.OpenDlg(720, 200, "w_dlg_sl_loanrequest_loanrightchoose.aspx", "");
                //                Gcoop.OpenIFrame(720,200, "w_dlg_sl_loanrequest_loanrightchoose.aspx","" );
            }else if (returnVal == "11") {
                //กรณีบันทึกข้อมูล
                Gcoop.GetEl("HdReturn").value = "";
                //เรียกหน้าจ่ายเงินกู้
                Gcoop.OpenDlg(760, 570, 'w_dlg_sl_popup_loanreceive.aspx', '');
            }
            if (confirmcoll == "8") {
                Gcoop.GetEl("Hdcollconfirm").value = "0";
                var strWindowFeatures = "menubar=no,location=yes,resizable=yes,scrollbars=yes,status=no";
                var rowNumber = Gcoop.GetEl("HdRowNumber").value ;
                var loanmemno = objdw_main.GetItem(rowNumber, "member_no");
                var refCollNo = objdw_coll.GetItem(rowNumber, "ref_collno");
                var coop_id = objdw_coll.GetItem(rowNumber, "memcoop_id");
                var collType = objdw_coll.GetItem(rowNumber, "loancolltype_code");
                var coll_amt = objdw_coll.GetItem(rowNumber, "coll_amt");
                var coll_use = objdw_coll.GetItem(rowNumber, "use_amt");
                var coll_blance = objdw_coll.GetItem(rowNumber, "coll_balance");
                var base_percent = objdw_coll.GetItem(rowNumber, "base_percent");
                var description = objdw_coll.GetItem(rowNumber, "description");
                var loancolltype_code = objdw_coll.GetItem(rowNumber, "loancolltype_code");
                Gcoop.OpenIFrame('700', '450', 'w_dlg_sl_loanrequest_coll_confirm.aspx',"?refCollNo=" + refCollNo + "&coop_id=" + coop_id + "&coll_amt=" + coll_amt + "&coll_use=" + coll_use + "&coll_blance=" + coll_blance + "&collType=" + collType + "&description=" + description + "&loancolltype_code=" + loancolltype_code + "&base_percent= " + base_percent + "&row=" + rowNumber + "&loanmemno=" + loanmemno);
              //   Gcoop.OpenDlg('700', '550', 'w_dlg_sl_loanrequest_coll_confirm.aspx', "?refCollNo=" + refCollNo + "&coop_id=" + coop_id + "&coll_amt=" + coll_amt + "&coll_use=" + coll_use + "&coll_blance=" + coll_blance + "&collType=" + collType + "&description=" + description + "&loancolltype_code=" + loancolltype_code + "&base_percent= " + base_percent + "&row=" + rowNumber + "&loanmemno=" + loanmemno);
                
                return;
            }

        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
     <asp:Literal ID="Ltdividen" runat="server"></asp:Literal>
    <asp:Literal ID="Ltjspopup" runat="server"></asp:Literal>
    <asp:Literal ID="Ltjspopupclr" runat="server"></asp:Literal>
    <asp:TextBox ID="txt_reqNo" runat="server" Visible="false"></asp:TextBox>
    <asp:TextBox ID="txt_member_no" runat="server" Visible="false"></asp:TextBox>
  
    <dw:WebDataWindowControl ID="dw_main" runat="server" DataWindowObject="d_sl_loanrequest_master_chktmp"
        LibraryList="~/DataWindow/Shrlon/sl_loan_requestment_surin.pbl" AutoRestoreContext="False"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
        ClientEventItemChanged="ItemDwMainChanged" ClientEventClicked="OnDwMainClicked" ToolTip="คำขอกู้ สิทธิกู้ คือ ตามระเบียบ ให้กู้ สูงสุด คือ สิทธิตาม เงินเดือนคงเหลือ"
        ClientFormatting="True" TabIndex="1">
    </dw:WebDataWindowControl>
    <br />
    <asp:Label ID="Label3" runat="server" Text="หลักประกัน" Font-Bold="True" >    </asp:Label> 
       
    <%-- <asp:CheckBox ID="Checkcollloop" runat="server" AutoPostBack="True" OnCheckedChanged="GenpermissCollLoop"
        Text="สถานะแลกกันค้ำ" />--%>
   <asp:CheckBox ID="CbCheckcoop" runat="server" Visible ="false" AutoPostBack="True" Text="ข้ามสาขา" />
     <asp:Literal ID="LtServerMessagecoll" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="dw_coll" runat="server" DataWindowObject="d_sl_loanrequest_collateral"
        LibraryList="~/DataWindow/Shrlon/sl_loan_requestment_cen.pbl" AutoRestoreContext="False"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
        ClientEventItemChanged="ItemDwCollChanged" ClientEventButtonClicked="OnDwCollClicked"
        ClientFormatting="True" TabIndex="250">
    </dw:WebDataWindowControl>
    <table style="width: 100%;" border="0">
        <tr>
            <td valign="top">
                <asp:Label ID="Label5" runat="server" Text="รายการหักอื่นๆ" Font-Bold="True"></asp:Label>
                <dw:WebDataWindowControl ID="dw_otherclr" runat="server" DataWindowObject="d_sl_loanrequest_otherclr"
                    LibraryList="~/DataWindow/Shrlon/sl_loan_requestment_cen.pbl" AutoRestoreContext="False"
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
                    ClientScriptable="True" ClientEventItemChanged="ItemDwOtherclrChanged" ClientEventButtonClicked="OnDwOtherclrClicked" ToolTip = "ต้องการเพิ่มหักเงินฝากกด + แล้วเลือกเงินฝาก"
                    BorderStyle="solid" BorderColor="white" TabIndex="500">
                </dw:WebDataWindowControl>
            </td>
            <td valign="top">
                <asp:Label ID="Label2" runat="server" Text="รายการหักกลบ" Font-Bold="True"></asp:Label>
                <dw:WebDataWindowControl ID="dw_clear" runat="server" DataWindowObject="d_sl_loanrequest_clear_chk"
                    LibraryList="~/DataWindow/Shrlon/sl_loan_requestment_cen.pbl" AutoRestoreContext="False"
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
                    ClientFormatting="True" ClientEventClicked="OnDwClearClicked" ClientEventItemChanged="ItemDwClearChanged"
                    TabIndex="600">
                </dw:WebDataWindowControl>
            </td>
        </tr>
    </table>
    <%-- <asp:Button ID="cb_checksum" runat="server" Text="ตรวจสอบยอด" Height="80px" Width="55px" OnClick="JsChecksumClick"
                    Style="width: 55px; height: 80px;" />--%>
    <dw:WebDataWindowControl ID="dw_message" runat="server" AutoRestoreContext="False"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
        DataWindowObject="d_ln_message" LibraryList="~/DataWindow/shrlon/sl_loan_requestment.pbl">
    </dw:WebDataWindowControl>
    <asp:Literal ID="LtXmlKeeping" runat="server" Visible="False"></asp:Literal>
    <asp:Literal ID="LtXmlReqloop" runat="server" Visible="False"></asp:Literal>
    <asp:Literal ID="LtXmlLoanDetail" runat="server" Visible="False"></asp:Literal>
    <asp:Literal ID="LtXmlOtherlr" runat="server" Visible="False"></asp:Literal>
    <asp:HiddenField ID="HdReturn" runat="server" />
    <asp:HiddenField ID="Hdmembtype_code" runat="server" />
    <asp:HiddenField ID="HdColumnName" runat="server" />
    <asp:HiddenField ID="HdMemberNo" runat="server" />
    <asp:HiddenField ID="HdRefcoll" runat="server" />
     <asp:HiddenField ID="HdRefcollrow" runat="server" />
    <asp:HiddenField ID="HdRefcollNO" runat="server" />
    <asp:HiddenField ID="HdMemcoopId" runat="server" />
    <asp:HiddenField ID="HdMsg" runat="server" />
    <asp:HiddenField ID="HdXml" runat="server" />
    <asp:HiddenField ID="HdMsgWarning" runat="server" />
    <asp:HiddenField ID="HdRowNumber" runat="server" />
    <asp:HiddenField ID="HdOpenIFrame" runat="server" Value="False" />
    <asp:HiddenField ID="HdcheckPdf" runat="server" Value="False" />
    <asp:HiddenField ID="HdIsPostBack" runat="server" Value="true" />
    <asp:HiddenField ID="Hdcommitreport" runat="server" Value="false" />
    <asp:HiddenField ID="Hdprincipal" runat="server" Value="false" />
    <asp:HiddenField ID="Hdobjective" runat="server" Value="false" />
    <asp:HiddenField ID="Hdcoopid" runat="server" Value="false" />
    <asp:HiddenField ID="HUseamt" runat="server" Value="false" />
    <asp:HiddenField ID="HdSelectReport" runat="server" Value="false" />
     <asp:HiddenField ID="HdPopupFlag" runat="server" Value="false" />
     <asp:HiddenField ID="HdCheckRemark" runat="server" Value="false" />
     <asp:HiddenField ID="HdShowRemark" runat="server" Value="false" />

     <asp:HiddenField ID="Hdreqround_factor" runat="server" Value="true" />
     <asp:HiddenField ID="Hdpayround_factor" runat="server" Value="true" />
     <asp:HiddenField ID="Hdlngrpcutright_flag" runat="server" Value="true" />
     <asp:HiddenField ID="Hdinttabrate_code" runat="server" Value="true" />
     <asp:HiddenField ID="Hdfixpaycal_type" runat="server" Value="true" />
     <asp:HiddenField ID="Hdrouninttype" runat="server" Value="true" />
     <asp:HiddenField ID="Hdcustomtime_type" runat="server" Value="true" />
     <asp:HiddenField ID="Hdloanright_type" runat="server" Value="true" />
     <asp:HiddenField ID="Hdloanrighttype_code" runat="server" Value="true" />
     <asp:HiddenField ID="Hdnotmoreshare_flag" runat="server" Value="true" />
     <asp:HiddenField ID="Hdmangrtpermgrp_code" runat="server" Value="true" />
     <asp:HiddenField ID="HdPaymonth" runat="server" />
     <asp:HiddenField ID="HdBalance" runat="server" />
     <asp:HiddenField ID="HdLoanrightpermiss" runat="server" />
     <asp:HiddenField ID="Hdxmlcoll" runat="server" />
     <asp:HiddenField ID="Hdxmlclear" runat="server" />
      <asp:HiddenField ID="Hdresign_timeadd" runat="server" />  
      <asp:HiddenField ID="Hdloangrpcredit_type" runat="server" />
    <asp:HiddenField ID="Hdloangrploantype_code" runat="server" />
    <asp:HiddenField ID="Hdcontno" runat="server" />
    <asp:HiddenField ID="HdCheck" runat="server" />
      <asp:HiddenField ID="Hdsalarybal_status" runat="server" />
      <asp:HiddenField ID="Hdminsalary_amt" runat="server" />
      <asp:HiddenField ID="Hdcollconfirm" runat="server" />
      <asp:HiddenField ID="HdLnreqdoc_no" runat="server" />
      <asp:HiddenField ID="Hdothercltrow" runat="server" />
      
</asp:Content>
