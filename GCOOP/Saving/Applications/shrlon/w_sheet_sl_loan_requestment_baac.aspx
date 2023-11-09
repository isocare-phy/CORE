<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_sl_loan_requestment_baac.aspx.cs" Inherits="Saving.Applications.shrlon.w_sheet_sl_loan_requestment_baac"
    ValidateRequest="false" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript %>
    <%=jsExpenseBank%>
    <%=jsExpenseCode%>
    <%=jsGetMemberInfo%>
    <%=jsGetMemberCollno%>
    <%=jsReNewPage%>
    <%=jsOpenOldDocNo%>
    <%=jsPostSetZero%>
    <%=jsSetpriod%>
    <%=jsCancelRequest %>
    <%=jsRefresh%>
    <%=jsCollInitP%>
    <%=jsCollCondition%>
    <%=jsSetDataList%>
    <%=jsPostColl %>
    <%=jsSumOthClr%>
    <%=jsContPeriod%>
    <%=jsChangeStartkeep%>
    <%=jsResumLoanClear%>
    <%=jsPermissSalary%>
    <%=jsRevert%>
    <%=jsPaycoopid%>
    <%=jsObjective %>
    <%=jspopupReportInvoice%>
    <%=jsRunProcessInvoice%>
    <%=jsSetloantype%>
    <%=jsPostreturn%>
    <%=jsmaxcreditperiod%>
    <%=jsChangeStartkeep%>
    <%=jspopupAgreeLoanReport%>
    <%=jspopupAgreeCollReport%>
    <%=jspopupCollReport%>
    <%=jspopupReportInvoice%>
    <%=jspopupLoanReport%>
    <%=jspopupDeptReport%>>
    <%=jsLoanpaymenttype%>
    <%=runProcess%>
    <%=popupReport%>
    <%=jssetcollrefcontno%>
    <%=jsSetloantypechg%>
    <%=jsrecalloanpermiss%>
    <%=jsReOtherclr%>
    <%=jsSetFixdate%>
    <%=jsExpensebankbrRetrieve %>
    <%=jsCheckCollmastrightBalance%>
    <%=resendStr%>
    <%=jsGetexpensememno%>
    <%=jsGetitemdescetc%>
    <%=jsCalpemisssalarybal %>
    <script type="text/javascript">

        function MenubarNew() {
            jsReNewPage();
        }

        function MenubarOpen() {
            Gcoop.OpenDlg('800', '600', 'w_dlg_sl_loanrequest_search.aspx', '');
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

            var period_payamt = objdw_main.GetItem(1, "period_payamt");
            var period_payment = objdw_main.GetItem(1, "period_payment");
            var loanrequest_amt = objdw_main.GetItem(1, "loanrequest_amt");
            var sum_use_amt = 0;

            for (var i = 1; i <= objdw_coll.RowCount(); i++) {
                var use_amt = objdw_coll.GetItem(i, "collactive_amt");
                sum_use_amt += use_amt;
            }

            objdw_main.AcceptText(); //main
            objdw_coll.AcceptText(); //หลักประกัน
            objdw_clear.AcceptText(); //หักกลบ     
            objdw_otherclr.AcceptText(); //หักอื่น

            if (sum_use_amt < loanrequest_amt || sum_use_amt == 0 + "") {
                alert("กรุณาใส่ข้อมูลหลักประกันให้ครบถ้วนด้วย");
            }
            else {
                return confirm("ยอดขอกู้ = " + loanrequest_amt + " |  ต้นเงิน = " + period_payment + " |  จน.งวด = " + period_payamt + "   ยืนยันการบันทึกข้อมูล");
            }
        }



        //**************** Start. 1.dw_main  ****************

        //Event----->ClientEventItemChanged="ItemDwMainChanged"
        function ItemDwMainChanged(sender, rowNumber, columnName, newValue) {
            objdw_main.SetItem(rowNumber, columnName, newValue);
            objdw_main.AcceptText();
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
                } catch (Error) {

                }

                // if (member_no != "null" && member_name != "null") {
                if (member_no != "null") {
                    var loanright_type = Gcoop.GetEl("Hdloanright_type").value;
                    if (loanright_type == 1) {
                        collmastclick();
                    } else {
                        jsGetMemberInfo();
                    }
                }

            }
            else if (columnName == "loanrcvfix_tdate") {
                objdw_main.SetItem(rowNumber, columnName, newValue);
                objdw_main.AcceptText();
                objdw_main.SetItem(1, "loanrcvfix_date", Gcoop.ToEngDate(newValue));
                objdw_main.AcceptText();
                jsChangeStartkeep();


            } else if (columnName == "period_payamt") {
                objdw_main.SetItem(rowNumber, columnName, newValue);
                objdw_main.AcceptText();
                jsContPeriod();

            } else if (columnName == "member_no") {
                objdw_main.SetItem(rowNumber, columnName, newValue);
                objdw_main.AcceptText();
                Gcoop.GetEl("HdMemberNo").value = newValue;
                var memcoop_id = objdw_main.GetItem(rowNumber, "memcoop_id");
                var loantype_code = objdw_main.GetItem(rowNumber, "loantype_code");
                var loanright_type = Gcoop.GetEl("Hdloanright_type").value;
                if (loanright_type == 1) {
                    collmastclick();
                }
                //                else if (loanright_type == 6) {

                //                    //เฉพาะหุ้นหลัก
                //                    // Gcoop.OpenIFrame(550, 450, "w_dlg_sl_loanrequest_monthpay.aspx", "?income=" + income + "&paymonth=" + paymonth + "&member_no=" + member_no + "&salary_amt=" + salary_amt + "&paymonth_coop_2=" + paymonth_coop_2 + "&principal_balance=" + principal_balance);
                //                    Gcoop.OpenDlg(720, 500, "w_dlg_sl_loanrequest_loanrightchoose_share.aspx", "?member_no=" + newValue + "&loantype_code=" + loantype_code);
                //                } 
                else if (loanright_type == 7) {
                    //เฉพาะเงินฝากหลัก
                    Gcoop.OpenDlg(720, 500, "w_dlg_sl_loanrequest_loanrightchoose_share.aspx", "?member_no=" + newValue + "&loantype_code=" + loantype_code);
                }
                else {
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

            } else if (columnName == "expense_code") {
                objdw_main.SetItem(rowNumber, columnName, newValue);
                objdw_main.AcceptText();
                jsExpenseCode();

            } else if (columnName == "expense_bank") {
                objdw_main.SetItem(rowNumber, columnName, newValue);
                objdw_main.AcceptText();
                jsExpenseBank();

            } else if (columnName == "expense_bank_1") {
                objdw_main.SetItem(rowNumber, columnName, newValue);
                objdw_main.AcceptText();
                jsExpenseBank();

            } else if (columnName == "loanrequest_amt") {
                var loanrequest_amt = newValue;
                var loancredit_amt = objdw_main.GetItem(1, "loancredit_amt");
                // ตรวจสอบถ้ายอดขอกู้มากกว่า ยอดตามสิทธิ์ให้กู้
                if (loanrequest_amt > loancredit_amt) {
                    //hardcode
                    // ช่วงคู่ขนานยังไม่lock
                    // alert("ยอดเงินขอกู้มากกว่ายอดเงินสิทธิ์ให้กู้ กรุณากรอกข้อมูลใหม่ สิทธิกู้สูงสุด #" + loancredit_amt.toString() + " คีย์ขอกู้ " + loanrequest_amt.toString());
                    if (confirm("ยอดเงินขอกู้มากกว่ายอดเงินสิทธิ์ให้กู้ กรุณากรอกข้อมูลใหม่ สิทธิกู้สูงสุด #" + loancredit_amt.toString() + " คีย์ขอกู้ " + loanrequest_amt.toString() + "   ยืนยันการขอกู้") == false) {
                        objdw_main.SetItem(1, "loanrequest_amt", 0);
                        // objdw_main.SetItem(rowNumber, columnName, newValue);
                        objdw_main.AcceptText();
                        jsPostSetZero();
                        //                    jsSetpriod();
                    } else {
                        objdw_main.SetItem(rowNumber, columnName, newValue);
                        //objdw_main.SetItem(rowNumber, "period_payamt", newValue);
                        objdw_main.AcceptText();
                        jsSetpriod();

                    }


                }
                else {
                    objdw_main.SetItem(rowNumber, columnName, newValue);
                    objdw_main.AcceptText();
                    jsSetpriod();
                }
            } else if (columnName == "loanreqregis_amt") {
                var loanrequest_amt = newValue;
                var loancredit_amt = objdw_main.GetItem(1, "loancredit_amt");
                var loanrequest_amtold = objdw_main.GetItem(1, "loanrequest_amt");
                // ตรวจสอบถ้ายอดขอกู้มากกว่า ยอดตามสิทธิ์ให้กู้
                if (loanrequest_amt > loancredit_amt) {
                    //hardcode
                    // ช่วงคู่ขนานยังไม่lock
                    if (confirm("ยอดเงินขอกู้มากกว่ายอดเงินสิทธิ์ให้กู้ กรุณากรอกข้อมูลใหม่ สิทธิกู้สูงสุด #" + loancredit_amt.toString() + " คีย์ขอกู้ " + loanrequest_amt.toString() + "   ยืนยันการขอกู้") == false) {
                        objdw_main.SetItem(1, "loanreqregis_amt", 0);
                        // objdw_main.SetItem(rowNumber, columnName, newValue);
                        objdw_main.AcceptText();
                        jsPostSetZero();
                        //                    jsSetpriod();
                    } else {
                        objdw_main.SetItem(1, "loanreqregis_amt", loanrequest_amt);
                        objdw_main.SetItem(rowNumber, columnName, newValue);
                        objdw_main.AcceptText();

                        if (loanrequest_amtold > loanrequest_amt) {
                            objdw_main.SetItem(1, "loanrequest_amt", loanrequest_amt);
                            objdw_main.AcceptText();
                            jsSetpriod();
                        }
                    }

                }

            } else if (columnName == "paytoorder_desc" || columnName == "paytoorder_desc_1") {
                var bfvalue = newValue;
                var afvalue = "00100" + bfvalue;
                objdw_main.SetItem(rowNumber, columnName, newValue);
                objdw_main.AcceptText();
                Gcoop.GetEl("Hdcoopid").value = newValue;
                jsPaycoopid();
            } else if (columnName == "loanpayment_type") {
                objdw_main.SetItem(1, columnName, newValue);
                objdw_main.AcceptText();

                jsContPeriod();
            } else if (columnName == "paymonth_other") {
                objdw_main.SetItem(rowNumber, columnName, newValue);
                objdw_main.AcceptText();
                var memno = objdw_main.GetItem(rowNumber, "member_no");
                var memcoop_id = objdw_main.GetItem(rowNumber, "memcoop_id");
                var loantype_code = objdw_main.GetItem(rowNumber, "loantype_code");
                if (memno != null) {
                    //alert("jssetcontno");

                    jsrecalloanpermiss();
                }
            } else if (columnName == "incomemonth_fixed") {
                objdw_main.SetItem(rowNumber, columnName, newValue);
                objdw_main.AcceptText();
                var memno = objdw_main.GetItem(rowNumber, "member_no");
                var memcoop_id = objdw_main.GetItem(rowNumber, "memcoop_id");
                var loantype_code = objdw_main.GetItem(rowNumber, "loantype_code");
                if (memno != null) {

                    jsrecalloanpermiss();
                }

            }
            else if (columnName == "loanrequest_type") {
                objdw_main.SetItem(rowNumber, columnName, newValue);
                objdw_main.AcceptText();
                var memno = objdw_main.GetItem(rowNumber, "member_no");
                var memcoop_id = objdw_main.GetItem(rowNumber, "memcoop_id");
                var loantype_code = objdw_main.GetItem(rowNumber, "loantype_code");

                if (newValue == 2 || memno != null) {
                    //alert("jssetcontno");
                    jssetcollrefcontno();
                } else {

                    if (memno != null || memcoop_id != null || memcoop_id != "" || loantype_code == "" || loantype_code != null) {
                        jsGetMemberInfo();
                    }
                }

            }
            else if (columnName == "period_payment") {
                var period_payment = newValue;
                var loanrequest_amt = objdw_main.GetItem(1, "loanrequest_amt");
                if (period_payment > loanrequest_amt) {

                    alert("ยอดชำระมากว่ายอดเงินขอกู้ กรุณากรอกข้อมูลใหม่");
                    objdw_main.SetItem(1, "period_payment", 0);

                    objdw_main.AcceptText();
                    jsPostSetZero();

                }
                else {
                    objdw_main.SetItem(rowNumber, "period_payment", newValue);
                    objdw_main.AcceptText();
                    jsRevert();
                }
            }
        }

        //Event----->ClientEventButtonClicked="OnDwMainClicked"
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

            } else if (objectName == "b_setkeep2") {

                var memberNoVal = objdw_main.GetItem(1, "member_no");
                var expense_code = objdw_main.GetItem(1, "loanpay_code");
                var expense_bank = objdw_main.GetItem(1, "loanpay_bank");
                var expense_branch = objdw_main.GetItem(1, "loanpay_branch");
                var expense_accid = objdw_main.GetItem(1, "loanpay_accid");

                Gcoop.OpenDlg(520, 250, "w_dlg_sl_lnreqloan_loanpay.aspx", "?member_no=" + memberNoVal + "&expense_code=" + expense_code + "&expense_bank=" + expense_bank + "&expense_branch=" + expense_branch + "&expense_accid=" + expense_accid + "&buttonc=kep");

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

            } else if (objectName == "b_search") {
                Gcoop.GetEl("HdColumnName").value = objectName;
                var coop_id = objdw_main.GetItem(rowNumber, "memcoop_id")
                Gcoop.OpenDlg('600', '600', 'w_dlg_sl_loanmember_search.aspx', "?coopId=" + coop_id);
            } else if (objectName == "b_mthpay") {
                var memberNo = objdw_main.GetItem(1, "member_no");
                if ((memberNo != "") && (memberNo != null) && (memberNo != "00000000")) {
                    var member_no = objdw_main.GetItem(1, "member_no");
                    var income = objdw_main.GetItem(1, "incomemonth_other");
                    var paymonth = objdw_main.GetItem(1, "paymonth_other");
                    var salary_amt = objdw_main.GetItem(1, "salary_amt");
                    var paymonth_exp = objdw_main.GetItem(1, "paymonth_exp");
                    var paymonth_coop_2 = objdw_main.GetItem(1, "paymonth_coop");
                    var principal_balance = Gcoop.GetEl("HdBalance").value;
                    var period_payment = objdw_main.GetItem(1, "period_payment");
                    var intest_amt = objdw_main.GetItem(1, "intestimate_amt");
                    var loanpayment_type = objdw_main.GetItem(1, "loanpayment_type");
                    var loanpayment_status = objdw_main.GetItem(1, "loanpayment_status");
                    if (loanpayment_status == -13 || loanpayment_status == -9) {
                        paymonth_coop_2 = paymonth_coop_2;
                    }
                    else {
                        if (loanpayment_type == 2) {
                            paymonth_coop_2 = paymonth_coop_2 + period_payment;
                        } else {
                            paymonth_coop_2 = paymonth_coop_2 + period_payment + intest_amt;
                        }
                    }
//                    alert(salary_amt + "," + paymonth + "," + paymonth_exp + "," + paymonth_coop_2 + "," + period_payment + "," + intest_amt);

                    // Gcoop.OpenIFrame(550, 450, "w_dlg_sl_loanrequest_monthpay.aspx", "?income=" + income + "&paymonth=" + paymonth + "&member_no=" + member_no);
                    Gcoop.OpenDlg(550, 550, "w_dlg_sl_loanrequest_monthpay.aspx", "?income=" + income + "&paymonth=" + paymonth + "&member_no=" + member_no + "&salary_amt=" + salary_amt + "&paymonth_coop_2=" + paymonth_coop_2 + "&principal_balance=" + principal_balance);
                    return 0;

                }
                else {
                    alert("ไม่สามารถแสดงรายการได้ กรุณากรอกเลขทะเบียนก่อน");
                }

            } else if ((objectName == "b_remark")) {

                objdw_main.AcceptText();
                var reamrk = objdw_main.GetItem(1, "remark")

                Gcoop.OpenDlg(620, 450, "w_dlg_loanrequest_remark.aspx", "?reamrk=" + reamrk);
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
            else if (objectName == "b_checksum") {
                jsPermissSalary();
            } else if (objectName == 'b_chgaccid') {
                var memberNoVal = objdw_main.GetItem(1, "member_no");
                var expense_code = objdw_main.GetItem(1, "expense_code");
                if ((memberNoVal != null) && (memberNoVal != "")) {
                    if (expense_code == "TRN") {
                        Gcoop.OpenDlg(620, 250, "w_dlg_show_accid.aspx", "?member=" + memberNoVal);
                    } else if (expense_code == "CBT") {

                        var memberNoVal = objdw_main.GetItem(1, "member_no");
                        var expense_code = objdw_main.GetItem(1, "loanpay_code");
                        var expense_bank = objdw_main.GetItem(1, "loanpay_bank");
                        var expense_branch = objdw_main.GetItem(1, "loanpay_branch");
                        var expense_accid = objdw_main.GetItem(1, "loanpay_accid");
                        Gcoop.OpenDlg(520, 250, "w_dlg_sl_lnreqloan_expense.aspx", "?member_no=" + memberNoVal + "&expense_code=" + expense_code + "&expense_bank=" + expense_bank + "&expense_branch=" + expense_branch + "&expense_accid=" + expense_accid + "&buttonc=expense");
                        // Gcoop.OpenDlg(620, 250, "w_dlg_sl_lnreqloan_expense.aspx", "?member=" + memberNoVal+"&button=expense");
                    }
                }
            } else if (objectName == "buyshare_flag") {
                Gcoop.CheckDw(sender, rowNumber, objectName, "buyshare_flag", 1, 0);
                sender.SetItem(1, "buyshare_amt", 0);
                jsRefresh();

            }
            else if (objectName == "b_retrive") {

                jsGetexpensememno();

            } else if (objectName == "b_recalloan") {
                jsCalpemisssalarybal();
            }
        }

        function GetValueReamrk(remark) {
            objdw_main.SetItem(1, "remark", remark);
            objdw_main.AcceptText();
            jsRefresh();


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
        function GetLoanpaymemno(expense_code, expense_bank, expense_branch, expense_accid, expense_bank_name, expense_branch_name, expense_desc) {

            objdw_main.SetItem(1, "loanpay_code", expense_code);
            objdw_main.SetItem(1, "loanpay_bank", expense_bank);
            objdw_main.SetItem(1, "loanpay_branch", expense_branch);
            objdw_main.SetItem(1, "loanpay_accid", expense_accid);
            objdw_main.SetItem(1, "paytoorder_desc", expense_desc);
            //            objdw_main.SetItem(1, "loanpay_branch", expense_branch_name);
            objdw_main.AcceptText();
            jsRefresh();


        }
        function GetLoanreceivememno(expense_code, expense_bank, expense_branch, expense_accid, expense_bank_name, expense_branch_name) {

            objdw_main.SetItem(1, "expense_code", expense_code);
            objdw_main.SetItem(1, "expense_bank", expense_bank);
            objdw_main.SetItem(1, "expense_branch", expense_branch);
            objdw_main.SetItem(1, "expense_accid", expense_accid);
            objdw_main.SetItem(1, "bank_name", expense_bank_name);
            objdw_main.SetItem(1, "branch_name", expense_branch_name);
            objdw_main.AcceptText();
            jsRefresh();


        }
        function GetAccIDDept(dept_no, deptaccount_name, prncbal) {
            var rowNumber = Gcoop.GetEl("HdRowNumber").value;
            objdw_coll.SetItem(rowNumber, "ref_collno", dept_no);
            objdw_coll.SetItem(rowNumber, "description", deptaccount_name);
            objdw_coll.SetItem(rowNumber, "coll_amt", prncbal);
            objdw_coll.SetItem(rowNumber, "coll_balance", prncbal);
            objdw_coll.SetItem(rowNumber, "use_amt", prncbal);
            Gcoop.GetEl("HUseamt").value = prncbal;
            objdw_coll.AcceptText();
            //jsPostreturn();
            jsCollCondition();


        }
        //**************** End. 1.dw_main  ****************


        //**************** Start. 2.dw_coll  ค้ำประกัน ****************

        //Event----->ClientEventItemChanged="ItemDwCollChanged"
        function ItemDwCollChanged(sender, rowNumber, columnName, newValue) {
            Gcoop.GetEl("HdRowNumber").value = rowNumber;

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
                else {
                    if (loancolltype_code == "01") {
                        //                        alert(0);
                        jsGetMemberCollno();
                    }
                    else if (loancolltype_code == "03") {
                        // alert(newValue);
                        //                        alert(1);
                        if ((newValue != null) && (newValue != "")) {
                            Gcoop.OpenDlg(620, 250, "w_dlg_show_accid_dept.aspx", "?dept=" + newValue);
                        }
                    }
                }

            } else if (columnName == "loancolltype_code") {
                objdw_coll.SetItem(rowNumber, columnName, newValue);
                objdw_coll.AcceptText();
                var loancolltype_code = objdw_coll.GetItem(rowNumber, "loancolltype_code");
                Gcoop.GetEl("HdRefcollrow").value = rowNumber + "";
                if (loancolltype_code == "02") {
                    //                    alert(2);
                    jsGetMemberCollno();
                }

            }
            else if (columnName == "collactive_amt") {
                var collbalance_amt = Number(objdw_coll.GetItem(rowNumber, "collbalance_amt"));
                if (newValue <= collbalance_amt) {
                    var loanrequest_amt = Number(objdw_main.GetItem(1, "loanrequest_amt"));
                    newValue = CheckpermissPerson(rowNumber, newValue);
                    objdw_coll.SetItem(rowNumber, columnName, newValue);
                    var percent = calpercent(newValue, loanrequest_amt);
                    objdw_coll.SetItem(rowNumber, "collactive_percent", percent);

                    objdw_coll.AcceptText();
                    var str = "objdw_coll.SetItem(" + rowNumber + ", '" + columnName + "', " + newValue + ")";
                    //                    settimeout(function () {
                    //                        eval(str);
                    //                        //                        eval("objdw_coll.AcceptText()");
                    ////                        objdw_coll.SetItem(rowNumber, columnName, newValue);
                    ////                        objdw_coll.AcceptText();
                    //                    }, 1000);
                }

            }
            else if (columnName == "collactive_percent") {
                if (newValue <= 100) {
                    objdw_coll.SetItem(rowNumber, columnName, newValue);
                    var loanrequest_amt = Number(objdw_main.GetItem(1, "loanrequest_amt"));
                    var collactive_amt = Numberfixed((newValue * loanrequest_amt) / 100, 2);
                    objdw_coll.SetItem(rowNumber, "collactive_amt", collactive_amt);
                    //CollCondition();
                    objdw_coll.AcceptText();
                }
                //jsRefresh();
            }
            else if (columnName == "calcollright_amt") {
                objdw_coll.SetItem(rowNumber, columnName, newValue);
                objdw_coll.AcceptText();
            }
        }

        function CheckpermissPerson(rowNumber, newValue) {
            var collbalance_amt = objdw_coll.GetItem(rowNumber, "collbalance_amt");
            var collmax_amt = objdw_coll.GetItem(rowNumber, "collmax_amt");
            if (newValue > collmax_amt && newValue < collbalance_amt) {
                if (confirm("ค้ำประกันเกินสิทธิการค้ำที่โดนจำกัดไว้(" + collmax_amt + "บาท)ต้องการทำรายการต่อหรือไม่")) {
                    return newValue;
                } else {
                    return collmax_amt;
                }
            } else if (newValue > collmax_amt && newValue > collbalance_amt) {
                if (confirm("ค้ำประกันเกินสิทธิค้ำคงเหลือต้องการทำรายการต่อหรือไม่")) {
                    return newValue;
                } else {
                    return collmax_amt;
                }
            }
            return newValue;

        }

        //Event----->ClientEventButtonClicked="OnDwCollClicked"
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
                var loantype_code = objdw_main.GetItem(1, "loantype_code");
                //  var requestDate = objdw_main.GetItem(1, "loanrequest_tdate");
                if ((collNo != "") && (collNo != null)) {

                    var loanmemno = objdw_main.GetItem(rowNumber, "member_no");
                    var refCollNo = objdw_coll.GetItem(rowNumber, "ref_collno");
                    var coop_id = objdw_coll.GetItem(rowNumber, "memcoop_id");
                    var collType = objdw_coll.GetItem(rowNumber, "loancolltype_code");
                    var coll_amt = objdw_coll.GetItem(rowNumber, "collbase_amt");
                    var coll_use = objdw_coll.GetItem(rowNumber, "coll_useamt");
                    var coll_blance = objdw_coll.GetItem(rowNumber, "collbalance_amt");
                    var base_percent = objdw_coll.GetItem(rowNumber, "collbase_percent");
                    var description = objdw_coll.GetItem(rowNumber, "description");
                    var loancolltype_code = objdw_coll.GetItem(rowNumber, "loancolltype_code");
                    //alert("collNo: " + collNo + "\n loantype_code: " + loantype_code + "\n loanmemno: " + loanmemno + "\n refCollNo: " + refCollNo + "\n coop_id: " + coop_id + "\n collType: " + collType + "\n coll_amt: " + coll_amt + "\n coll_use: " + coll_use + "\n coll_blance: " + coll_blance + "\n base_percent: " + base_percent + "\n description: " + description + "\n loancolltype_code: " + loancolltype_code);
                    Gcoop.OpenDlg('700', '450', 'w_dlg_sl_loanrequest_coll.aspx', "?refCollNo=" + refCollNo + "&coop_id=" + coop_id + "&coll_amt=" + coll_amt + "&coll_use=" + coll_use + "&coll_blance=" + coll_blance + "&collType=" + collType + "&description=" + description + "&loancolltype_code=" + loancolltype_code + "&base_percent= " + base_percent + "&row=" + rowNumber + "&loanmemno=" + loanmemno + "&loantype_code=" + loantype_code);
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
                Gcoop.OpenDlg('620', '450', 'w_dlg_sl_collmaster_search_req.aspx', "?member=" + memberNoVal + "&loantype_code=" + loantype_code);
            } else if ((buttonName == "b_searchc2") && (collTypeCode == '04')) {
                //ค้นหาหลักทรัพย์ค้ำประกัน
                var refCollNo2 = Gcoop.GetEl("HdMemberNo").value;
                var loantype_code = objdw_main.GetItem(1, "loantype_code");
                Gcoop.OpenDlg('640', '450', 'w_dlg_sl_collmaster_search_mb_.aspx', "?member=" + memberNoVal + "&loantype_code=" + loantype_code);
            }
            else if (buttonName == "b_addrow") {
                objdw_coll.InsertRow(objdw_coll.RowCount() + 1);
            }
            else if (buttonName == "b_recoll") {
                CollCondition();
            }
            else if (buttonName == "b_condition") {
                CollCondition();
            } else if (buttonName == "b_renew") {

            }

        }

        function recoll() {
            var sum = 0;
            var arr = new Array();
            for (var i = 1; i <= objdw_coll.RowCount(); i++) {
                if (objdw_coll.GetItem(i, "collactive_amt") != null) {
                    sum += Number(objdw_coll.GetItem(i, "collactive_amt"));
                    arr.push(i);
                }
            }
            var sum_percent = 0;
            for (var j = 0; j < (arr.length) - 1; j++) {
                var collactive_amt = Number(objdw_coll.GetItem(arr[j], "collactive_amt"));
                var percent = Numberfixed(collactive_amt / sum, 2);
                sum_percent += percent;
                objdw_coll.SetItem(arr[j], "collactive_percent", percent);
            }
            objdw_coll.SetItem(arr[j], "collactive_percent", 100 - Numberfixed(sum_percent, 2));
            objdw_coll.AcceptText();
        }

        function CollCondition() {
            var loanrequest_amt = objdw_main.GetItem(1, "loanrequest_amt");
            re_pole(loanrequest_amt);
        }

        function re_pole(loanrequest_amt) {
            var arr_seq = new Array();
            var tmp_loanrequest_amt = loanrequest_amt;
            var arr_priority = ["03", "02", "01", "04"];
            var arr_idx = new Array();
            for (var j = 0; j < arr_priority.length; j++) {
                var arr_sort_ingroup = new Array();
                var chk_loancolltype_code = arr_priority[j];
                for (var i = 1; i <= objdw_coll.RowCount(); i++) {
                    var loancolltype_code = objdw_coll.GetItem(i, "loancolltype_code");
                    if (loancolltype_code == chk_loancolltype_code) {
                        arr_sort_ingroup.push(i);
                    }
                }
                var ar_sort = SortArray_asc(arr_sort_ingroup);

                for (var i = 0; i < ar_sort.length; i++) {
                    arr_seq.push(ar_sort[i]);
                    if (chk_loancolltype_code == "01") {  // ประเภทคนค้ำ
                        arr_idx.push(ar_sort[i]);
                    } else {
                        var collactive_amt = Number(objdw_coll.GetItem(ar_sort[i], "collactive_amt"));
                        if (tmp_loanrequest_amt >= collactive_amt) {

                            tmp_loanrequest_amt = tmp_loanrequest_amt - collactive_amt;
                            objdw_coll.SetItem(ar_sort[i], "collactive_percent", calpercent(collactive_amt, loanrequest_amt));
                        } else {
                            if (tmp_loanrequest_amt > 0) {
                                objdw_coll.SetItem(ar_sort[i], "collactive_amt", tmp_loanrequest_amt);
                                objdw_coll.SetItem(ar_sort[i], "collactive_percent", calpercent(tmp_loanrequest_amt, loanrequest_amt));
                            } else {
                                objdw_coll.SetItem(ar_sort[i], "collactive_amt", 0);
                                objdw_coll.SetItem(ar_sort[i], "collactive_percent", 0);
                            }
                        }
                        objdw_coll.AcceptText();
                    }
                }
            }
            CalAmt_Person(arr_idx, tmp_loanrequest_amt, loanrequest_amt);
            ResultSum(arr_seq);
        }

        function ResultSum(arr_seq) {
            var sum_amt = 0;
            var sum_percent = 0;
            var i;
            var i_percent = -1;
            for (i = 0; i < (arr_seq.length) - 1; i++) {
                sum_amt = Numberfixed(sum_amt + Numberfixed(objdw_coll.GetItem(arr_seq[i], "collactive_amt"), 2), 2);
                sum_percent = Numberfixed(sum_percent + Numberfixed(objdw_coll.GetItem(arr_seq[i], "collactive_percent"), 2), 2);
            }
            var loanrequest_amt = Numberfixed(objdw_main.GetItem(1, "loanrequest_amt"), 2);
            var validate_sumamt = sum_amt + Numberfixed(objdw_coll.GetItem(arr_seq[i], "collactive_amt"), 2)
            var res_collactive_amt;
            if (validate_sumamt > loanrequest_amt) {
                var last_row = Numberfixed(loanrequest_amt - sum_amt, 2);
                objdw_coll.SetItem(arr_seq[i], "collactive_amt", Numberfixed(last_row, 2));
                res_collactive_amt = loanrequest_amt;
            } else if (validate_sumamt == (loanrequest_amt - 0.01)) {
                var flag = true;
                for (var r = (arr_seq.length) - 1; r >= 0; r--) {
                    var collbalance_amt = Number(objdw_coll.GetItem(arr_seq[r], "collbalance_amt"));
                    var collactive_amt = Number(objdw_coll.GetItem(arr_seq[r], "collactive_amt"));
                    if ((collactive_amt + 0.01) <= collbalance_amt && flag) {
                        objdw_coll.SetItem(arr_seq[r], "collactive_amt", collactive_amt + 0.01);
                        objdw_coll.AcceptText();
                        flag = false;
                        res_collactive_amt = loanrequest_amt;
                    }
                }
            } else if (validate_sumamt < 0) {
                res_collactive_amt = 0;
            } else {
                res_collactive_amt = Numberfixed(validate_sumamt, 2);
            }

            var validate_percent = Numberfixed(sum_percent + Numberfixed(objdw_coll.GetItem(arr_seq[i], "collactive_percent"), 2), 2);
            var res_percent;
            if (validate_percent > 100) {
                var last_row = 100 - sum_percent;
                objdw_coll.SetItem(arr_seq[i], "collactive_percent", Numberfixed(last_row, 2));
                res_percent = 100;

            } else if (validate_percent == 99.99) {
                i_percent = arr_seq.length - 1;
                var collactive_percent = Numberfixed(Number(objdw_coll.GetItem(arr_seq[i_percent], "collactive_percent")), 2);
                objdw_coll.SetItem(arr_seq[i_percent], "collactive_percent", Numberfixed(collactive_percent + 0.01, 2));
                objdw_coll.AcceptText();
                res_percent = 100;
            } else if (validate_percent < 0) {
                res_percent = 0;
            } else {
                res_percent = Numberfixed(validate_percent, 2);
            }
            // compute filed ไม่สามารถ setค่าได้จาก C# javascript ต้อง setด้วย jquery
            $("#objdw_coll_footer span:eq(1)").text(addCommas(res_collactive_amt, 2));
            $("#objdw_coll_footer span:eq(2)").text(addCommas(res_percent, 2));
            objdw_coll.AcceptText();
        }

        function CalAmt_Person(arr_idx, Sum_amt, loanrequest_amt) {
            if (arr_idx.length <= 0) {
                return;
            }
            var count = arr_idx.length;
            var avg = Sum_amt / count;
            if (checkAvg(arr_idx, avg)) {
                for (var i = 0; i < arr_idx.length; i++) {
                    objdw_coll.SetItem(arr_idx[i], "collactive_amt", Numberfixed(avg, 2));
                    objdw_coll.SetItem(arr_idx[i], "collactive_percent", calpercent(avg, loanrequest_amt));
                }
                objdw_coll.AcceptText();

            } else {
                var arr_idx2 = new Array();
                var tmp_sum = 0;
                for (var i = 0; i < arr_idx.length; i++) {
                    var collbalance_amt = Number(objdw_coll.GetItem(arr_idx[i], "collbalance_amt"));
                    var val_log = Number(objdw_coll.GetItem(arr_idx[i], "collmax_amt"));
                    var min = collbalance_amt;
                    if (min > val_log) {
                        min = val_log;
                    }
                    if (min <= avg) {
                        objdw_coll.SetItem(arr_idx[i], "collactive_amt", Numberfixed(min, 2));
                        objdw_coll.SetItem(arr_idx[i], "collactive_percent", calpercent(min, loanrequest_amt));
                        tmp_sum += min;
                    } else {
                        arr_idx2.push(arr_idx[i]);
                    }
                }
                objdw_coll.AcceptText();
                CalAmt_Person(arr_idx2, Sum_amt - tmp_sum, loanrequest_amt);
            }

        }

        function SortArray_asc(arr) {
            var arr_amt = new Array();
            for (var i = 0; i < arr.length; i++) {
                var tmp = Number(objdw_coll.GetItem(arr[i], "collbalance_amt"));
                arr_amt.push(tmp);
            }
            for (var i = 0; i < (arr_amt.length) - 1; i++) {
                for (var j = i + 1; j < arr_amt.length; j++) {
                    if (arr_amt[j] < arr_amt[i]) {
                        var tmp1, tmp2;
                        tmp1 = arr_amt[j];
                        arr_amt[j] = arr_amt[i];
                        arr_amt[i] = tmp;

                        tmp2 = arr[j];
                        arr[j] = arr[i];
                        arr[i] = tmp2;
                    }
                }
            }
            return arr;
        }

        function calpercent(val, total) {
            if (total == 0) {
                return 0;
            }
            var percent = Numberfixed((val / total) * 100, 2);
            return percent;
        }

        function Numberfixed(val, fixed) {
            val = Number(val);
            val = val.toFixed(fixed);
            return Number(val);
        }

        function addCommas(nStr, fixed) {
            nStr = parseFloat(nStr);
            nStr = nStr.toFixed(2);
            nStr = nStr.toString();
            nStr += '';
            x = nStr.split('.');
            x1 = x[0];
            if (x.length > 1) {
                //        var tmp = parseFloat('0.' + x[1]);
                //        var tmp2 = tmp.toFixed(fixed);
                //        var arr = tmp2.split('.');
                x2 = x[1];
            } else {
                var tmpdata = '';
                for (var i = 0; i < fixed; i++) {
                    tmpdata += '0';
                }
                x2 = tmpdata;
            }
            var rgx = /(\d+)(\d{3})/;
            while (rgx.test(x1)) {
                x1 = x1.replace(rgx, '$1' + ',' + '$2');
            }
            return x1 + "." + x2;
        }

        function checkAvg(arr, avg) {
            var flag = true;
            for (var i = 0; i < arr.length; i++) {
                var collbalance_amt = Number(objdw_coll.GetItem(arr[i], "collbalance_amt"));
                var val_log = Number(objdw_coll.GetItem(arr[i], "collmax_amt"));
                if (avg > collbalance_amt || avg > val_log)
                    flag = false;
            }
            return flag;
        }

        function ItemDwOtherclrChanged(sender, rowNumber, columnName, newValue) {
            objdw_otherclr.SetItem(rowNumber, columnName, newValue);
            objdw_otherclr.AcceptText();
            Gcoop.GetEl("Hdothercltrow").value = rowNumber + "";
            if (columnName == "clrother_amt") {

                jsSumOthClr();
            } else if (columnName == "clrothertype_code") {
                if (newValue == "FSV") {
                    objdw_otherclr.SetItem(rowNumber, "clrother_desc", "ค่าธรรมเนียมบริหารเงินกู้");
                } else if (newValue == "SHR") {
                    objdw_otherclr.SetItem(rowNumber, "clrother_desc", "หักซื้อหุ้นเพิ่ม");
                } else if (newValue == "INS") {
                    objdw_otherclr.SetItem(rowNumber, "clrother_desc", "ประกันชีวิต");
                } else if (newValue == "ETC") {
                    objdw_otherclr.SetItem(rowNumber, "clrother_desc", "อื่นๆ");
                } else {

                    jsGetitemdescetc();
                }


            }
        }


        //Event----->ClientEventButtonClicked="OnDwOtherclrClicked"
        function OnDwOtherclrClicked(sender, rowNumber, buttonName) {

            Gcoop.GetEl("Hdothercltrow").value = rowNumber + "";
            if (buttonName == "b_delete") {
                //                objdw_otherclr.DeleteRow(rowNumber);
                Gcoop.GetEl("HdRowNumber").value = rowNumber;
                jsReOtherclr();
            }
            else if (buttonName == "b_addrow") {
                objdw_otherclr.InsertRow(objdw_otherclr.RowCount() + 1);
            }
            else if (buttonName == "b_deptother") {
                Gcoop.GetEl("HdRowNumber").value = rowNumber + "";
                Gcoop.GetEl("HdColumnName").value = buttonName;
                // ค้นหาบัญชีเงินฝาก 
                var memberNoVal = objdw_main.GetItem(1, "member_no");
                if ((memberNoVal != null) && (memberNoVal != "")) {
                    //                    alert(3);
                    Gcoop.OpenDlg(620, 250, "w_dlg_show_accid.aspx", "?member=" + memberNoVal);
                }
            } else if (buttonName == "clrothertype_code") {
                alert(buttonName);
                if (newValue == "FSV") {
                    objdw_otherclr.SetItem(1, "clrother_desc", "ค่าธรรมเนียมบริหารเงินกู้");
                } else if (newValue == "SHR") {
                    objdw_otherclr.SetItem(1, "clrother_desc", "หักซื้อหุ้นเพิ่ม");
                } else if (newValue == "INS") {
                    objdw_otherclr.SetItem(1, "clrother_desc", "ประกันชีวิต");
                } else {

                    jsGetitemdescetc();
                }

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

        //Event----->ClientEventButtonClicked="OnDwClearClicked"
        function OnDwClearClicked(sender, rowNumber, objectName) {
            if (objectName == "clear_status") {

                Gcoop.CheckDw(sender, rowNumber, objectName, "clear_status", 1, 0);
                Gcoop.GetEl("HdClearrow").value = rowNumber;
                //jsSumOthClr();
                jsrecalloanpermiss();
            }
            else if (objectName == "b_detail") {
                var loanContractNo = objdw_clear.GetItem(rowNumber, "loancontract_no");
                if ((loanContractNo != null) && (loanContractNo != "")) {
                    jsSetDataList();
                    var contractNo = objdw_clear.GetItem(rowNumber, "loancontract_no");
                    Gcoop.OpenDlg(500, 400, 'w_dlg_sl_loanrequest_cleardet.aspx', '?contractNo=' + contractNo);

                }
                else {
                    alert("ไม่สามารถแสดงรายการได้ กรุณากรอกเลขทะเบียนก่อน");
                }

            }
            //            else if (objectName == "b_checksum") {
            //                jsPermissSalary();
            //            }
        }
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

        //w_dlg_dp_account_search เงินฝาก
        function NewAccountNo(dept_no, deptaccount_name, prncbal) {
            var rowNumber = Gcoop.GetEl("HdRowNumber").value;
            objdw_coll.SetItem(rowNumber, "ref_collno", dept_no);
            objdw_coll.SetItem(rowNumber, "description", deptaccount_name);
            objdw_coll.SetItem(rowNumber, "coll_amt", prncbal);
            objdw_coll.SetItem(rowNumber, "coll_balance", prncbal);
            objdw_coll.SetItem(rowNumber, "use_amt", prncbal);
            Gcoop.GetEl("HUseamt").value = prncbal;
            objdw_coll.AcceptText();
            //jsCollCondition();
            // jsGetMemberCollno();
            jsCheckCollmastrightBalance();
        }
        //w_dlg_sl_collmaster_search_req หลักทรัพย์
        function GetValueFromDlgCollmast(collRefNo, collmast_desc, mortgage_price, base_percent) {
            if (collmast_desc == null || collmast_desc == "") {
                collmast_desc = "";
            }
            var rowNumber = Gcoop.GetEl("HdRowNumber").value;
            var desc = collRefNo + ":" + collmast_desc;
            objdw_coll.SetItem(rowNumber, "ref_collno", collRefNo);
            objdw_coll.SetItem(rowNumber, "description", collmast_desc);
//            objdw_coll.SetItem(rowNumber, "coll_amt", mortgage_price);
//            objdw_coll.SetItem(rowNumber, "coll_balance", mortgage_price * base_percent);
//            objdw_coll.SetItem(rowNumber, "use_amt", mortgage_price * base_percent);
//            objdw_coll.SetItem(rowNumber, "base_percent", base_percent);
            Gcoop.GetEl("HUseamt").value = mortgage_price;
            Gcoop.GetEl("HdRefcollNO").value = collRefNo;
            objdw_coll.AcceptText();
            //jsPostreturn();
            //jsCollCondition();
            jsCheckCollmastrightBalance();
        }

        //ฟังก์ชั่นรายงาน**********************************************************************
        function OnClickLoan() {

            objDwMain.AcceptText();
            jspopupLoanReport();
        }

        function OnclickLinkNextinvoice() {
            objDwMain.AcceptText();
            jspopupReportInvoice();
        }
        function OnClickAgreeLoan() {

            objDwMain.AcceptText();
            jspopupAgreeLoanReport();
        }

        function OnclickColl() {
            objDwMain.AcceptText();
            jspopupCollReport();
        }
        function OnClickAgreeColl() {

            objDwMain.AcceptText();
            jspopupAgreeCollReport();
        }
        function OnClickdept() {

            objDwMain.AcceptText();
            jspopupDeptReport();
        }
        function OnClickPDF() {
            Gcoop.GetEl("HdSelectReport").value = "1";
            runProcess();

        }
        function OnClickPDF2() {
            Gcoop.GetEl("HdSelectReport").value = "2";
            runProcess();

        }
        function GetShowData() {

            jsReNewPage();
        }
        //*************************************************************************************
        function LoadDWColl(loanpermiss, xmlcoll, xmlclear) {
            //alert("okkk");
            //   alert(xmlcoll);
            Gcoop.GetEl("HdLoanrightpermiss").value = loanpermiss;
            // alert(loanpermiss);

            //       Gcoop.GetEl("LtXmcoll").value = xmlcoll;
            Gcoop.GetEl("Hdxmlcoll").value = xmlcoll.toString();
            //   alert(xmlclear);
            Gcoop.GetEl("Hdxmlclear").value = xmlclear.toString();

            resendStr();
        }
        function collmastclick() {
            Gcoop.GetEl("HdReturn").value = "";
            Gcoop.GetEl("HdColumnName").value = "";
            Gcoop.OpenDlg(720, 200, "w_dlg_sl_loanrequest_loanrightchoose.aspx", "");

        }
        function SheetLoadComplete() {
            if (Gcoop.GetEl("HdIsPostBack").value != "true") {
                Gcoop.SetLastFocus("member_no_0");
                Gcoop.Focus();
            }
            if (Gcoop.GetEl("HdCheckRemark").value == "true") {
                var member = Gcoop.GetEl("HdMemberNo").value;
                Gcoop.OpenDlg("460", "200", "w_dlg_ln_remarkstatus.aspx", "?MemberNo=" + member);
                Gcoop.GetEl("HdShowRemark").value = "true";

            }
            var returnVal = Gcoop.GetEl("HdReturn").value;
            var columnVal = Gcoop.GetEl("HdColumnName").value;
            var msgVal = Gcoop.GetEl("HdMsg").value;
            var memberNoVal = Gcoop.GetEl("HdMemberNo").value;
            var contno = Gcoop.GetEl("Hdcontno").value;
            var approve_flag = objdw_main.GetItem(1, "apvimmediate_flag");
            var openIframe = Gcoop.GetEl("HdCheck").value;
            if (openIframe == "true") {
                var openedF = Gcoop.GetEl("HdOpened").value;
                if (openedF != "1") {
                    Gcoop.GetEl("HdOpened").value = "1";
                    var member_no = objdw_main.GetItem(1, "member_no");
                    var punishgroup_code = "(1000,3000,5000)";
                    Gcoop.OpenIFrame(720, 400, "w_iframe_punish_detail.aspx", "?member_no=" + member_no + "&punishgroup_code=" + punishgroup_code);
                }
            }
            if (approve_flag == 1 && returnVal == 11) {
                Gcoop.GetEl("Hdcontno").value = "";
                alert(" เลขที่สัญญาเงินกู้ " + contno);
            }
            if (returnVal == 8 && columnVal == "genbaseloancredit") {
                Gcoop.GetEl("HdReturn").value = "";
                Gcoop.GetEl("HdColumnName").value = "";
                Gcoop.OpenDlg(720, 200, "w_dlg_sl_loanrequest_loanrightchoose.aspx", "");
                //                Gcoop.OpenIFrame(720,200, "w_dlg_sl_loanrequest_loanrightchoose.aspx","" );
            } else if (returnVal == 11 && approve_flag == 3) {
                //กรณีบันทึกข้อมูล
                Gcoop.GetEl("HdReturn").value = "";
                //เรียกหน้าจ่ายเงินกู้
                Gcoop.OpenDlg(760, 570, 'w_dlg_sl_popup_loanreceive.aspx', '');
            } else if (returnVal == 11 && approve_flag == 2) {
                var coop_id = objdw_main.GetItem(1, "coop_id");
                var lnrcvfrom_code = "CON";

                Gcoop.GetEl("HdReturn").value = "";
                Gcoop.GetEl("Hdcontno").value = "";
                var word = contno + "@" + lnrcvfrom_code + "@" + coop_id;
                alert(" เลขที่สัญญาเงินกู้ " + contno);
                Gcoop.OpenIFrame2("830", "650", "w_dlg_loan_receive_order.aspx", "?loans=" + word);
            }
            //--Open IFrame---
            var member_no = objdw_main.GetItem(1, "member_no");
            var loanright_type = Gcoop.GetEl("Hdloanright_type").value;
            var loantype_code = objdw_main.GetItem(1, "loantype_code");
            var OIF = Gcoop.GetEl("HdOIF").value;
            //            var text = "member_no :" + member_no + " loanright_type :" + loanright_type + " loantype_code :" + loantype_code + " OIF :" + OIF;
            //            alert(text.fontsize(10));
            if (loanright_type == 6 && loantype_code == 54 && OIF == "true" && (member_no != "" || member_no != null)) {
                //เฉพาะหุ้นหลัก
                Gcoop.GetEl("HdOIF").value = "false";
                Gcoop.OpenDlg(720, 500, "w_dlg_sl_loanrequest_loanrightchoose_share.aspx", "?member_no=" + member_no + "&loantype_code=" + loantype_code);
            }
            //---

            CollCondition();
            Disable_ALL();
        }

        function Disable_ALL() {
            DisabledTable(objdw_main, "loanrequest_status", "dw_main", null);
            DisabledTable(objdw_main, "loanrequest_status", "dw_coll", null);
            DisabledTable(objdw_main, "loanrequest_status", "dw_otherclr", null);
            DisabledTable(objdw_main, "loanrequest_status", "dw_clear", null);
            DisabledTable(objdw_main, "loanrequest_status", "dw_message", null);
        }

        function DisabledTable(s, col, namedw, findname) {
            var chk = s.GetItem(1, col);
            chk = chk.toString();

            if (findname == null || findname == '') {
                findname = '';
            } else {
                findname = ',' + findname;
            }
            var status;
            if (chk == '8') {
                status = false;
            } else {
                status = true;
            }
            $('#obj' + namedw + '_datawindow').find('input,select,button' + findname).attr('disabled', status)
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <asp:Literal ID="Ltdividen" runat="server"></asp:Literal>
    <asp:Literal ID="Ltjspopup" runat="server"></asp:Literal>
    <asp:Literal ID="Ltjspopupclr" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="dw_main" runat="server" DataWindowObject="d_sl_loanrequest_master"
        LibraryList="~/DataWindow/Shrlon/sl_loan_requestment_cen.pbl" AutoRestoreContext="False"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
        ClientEventItemChanged="ItemDwMainChanged" ClientEventClicked="OnDwMainClicked"
        ToolTip=" " ClientFormatting="True" TabIndex="1">
    </dw:WebDataWindowControl>
    <br />
    <asp:Label ID="Label3" runat="server" Text="หลักประกัน" Font-Bold="True">    </asp:Label>
    <%-- <asp:CheckBox ID="Checkcollloop" runat="server" AutoPostBack="True" OnCheckedChanged="GenpermissCollLoop"
        Text="สถานะแลกกันค้ำ" />--%>
    <asp:CheckBox ID="CbCheckcoop" runat="server" Visible="false" AutoPostBack="True"
        Text="ข้ามสาขา" />
    <asp:Literal ID="LtServerMessagecoll" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="dw_coll" runat="server" DataWindowObject="d_sl_loanrequest_collateral_2"
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
                    ClientScriptable="True" ClientEventItemChanged="ItemDwOtherclrChanged" ClientEventButtonClicked="OnDwOtherclrClicked"
                    ToolTip="ต้องการเพิ่มหักเงินฝากกด + แล้วเลือกเงินฝาก" BorderStyle="solid" BorderColor="white"
                    TabIndex="500">
                </dw:WebDataWindowControl>
            </td>
            <td valign="top">
                <asp:Label ID="Label2" runat="server" Text="รายการหักกลบ" Font-Bold="True"></asp:Label>
                <dw:WebDataWindowControl ID="dw_clear" runat="server" DataWindowObject="d_sl_loanrequest_clear"
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
    <asp:Literal ID="LtXmclear" runat="server" Visible="False"></asp:Literal>
    <asp:Literal ID="LtXmcoll" runat="server" Visible="False"></asp:Literal>
    <asp:TextBox ID="txt_reqNo" runat="server" Visible="false"></asp:TextBox>
    <asp:TextBox ID="txt_member_no" runat="server" Visible="false"></asp:TextBox>
    <asp:LinkButton ID="LinkButton3" runat="server" Visible="false" OnClick="LinkButton3_Click">พิมพ์สัญญาเงินกู้</asp:LinkButton>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:LinkButton ID="LinkButton2" runat="server" Visible="false" OnClick="LinkButton2_Click">พิมพ์ คำขอกู้เงินกู้สามัญ</asp:LinkButton>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:LinkButton ID="LinkButton1" runat="server" Visible="false" OnClick="LinkButton1_Click1">พิมพ์ หนังสือสัญญาค้ำประกัน</asp:LinkButton>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:LinkButton ID="LinkButton4" runat="server" Visible="false" OnClick="LinkButton4_Click">พิมพ์ ใบรับเงินพิ่ม</asp:LinkButton>
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
    <asp:HiddenField ID="HdShowRemark" runat="server" Value="true" />
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
    <asp:HiddenField ID="Hdothercltrow" runat="server" />
    <asp:HiddenField ID="Hdinttabfix_code" runat="server" />
    <asp:HiddenField ID="HdCollmaxval1" runat="server" />
    <asp:HiddenField ID="HdCollmaxval2" runat="server" />
    <asp:HiddenField ID="HdOIF" runat="server" />
    <asp:HiddenField ID="HdClearrow" runat="server" />
    <asp:HiddenField ID="HdFlag" runat="server" />
    <asp:HiddenField ID="HdDetailRow" runat="server" />
    <asp:HiddenField ID="HdCountPerson" runat="server" />
</asp:Content>
