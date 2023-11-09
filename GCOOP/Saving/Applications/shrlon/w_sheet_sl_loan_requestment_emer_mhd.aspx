<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_sl_loan_requestment_emer_mhd.aspx.cs" Inherits="Saving.Applications.shrlon.w_sheet_sl_loan_requestment_emer_mhd" %>

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
    <%=jspopupReport%>
    <%=jsrunProcess%>
    <%=jspopupReportInvoice%>
    <%=jsRunProcessInvoice%>
    <%=jsSetloantype%>
    <%=jsPostreturn %>
    <%=jsmaxcreditperiod%>
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
            objdw_main.AcceptText();
            jsOpenOldDocNo();
        }

        function Validate() {
            var period_payamt = objdw_main.GetItem(1, "period_payamt");
            var period_payment = objdw_main.GetItem(1, "period_payment");
            var loanrequest_amt = objdw_main.GetItem(1, "loanrequest_amt");

            objdw_main.AcceptText(); //main
            objdw_coll.AcceptText(); //หลักประกัน
            objdw_clear.AcceptText(); //หักกลบ     
            objdw_otherclr.AcceptText(); //หักอื่น
            return confirm("ยอดขอกู้ = " + loanrequest_amt + " | ต้นเงิน = " + period_payment + " |  จน.งวด = " + period_payamt + "   ยืนยันการบันทึกข้อมูล");
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

                    }
                } catch (Error) { }

                try {
                    member_name = objdw_main.GetItem(rowNumber, "member_name");
                    if (member_name == null) {
                        member_name = "null";

                    }
                } catch (Error) { }


                if (member_name != "null") {
                    jsmaxcreditperiod();
                }
                else if (member_no != "null") {

                    jsGetMemberInfo();
                }


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
                    //ช่วงคู่ขนานยังไม่lock
                    alert("ยอดเงินขอกู้มากกว่ายอดเงินสิทธิ์ให้กู้ กรุณากรอกข้อมูลใหม่");
                    objdw_main.SetItem(1, "loanrequest_amt", 0);
                    //objdw_main.SetItem(rowNumber, columnName, newValue);
                    objdw_main.AcceptText();
                    jsPostSetZero();
                    //  jsSetpriod();
                }
                else {
                    objdw_main.SetItem(rowNumber, columnName, newValue);
                    objdw_main.AcceptText();
                    jsSetpriod();
                }
            }
            else if (columnName == "loanrcvfix_tdate") {
                objdw_main.SetItem(rowNumber, "loanrcvfix_tdate", newValue);
                objdw_main.AcceptText();

                // แก้ไขกรณี 003001 ให้สามารถกรอกวันที่จ่ายเงินกู้ได้
                var coop_id = objdw_main.GetItem(1, "coop_id");
                if (coop_id != "003001") {
                    jsChangeStartkeep();
                }
            }
            else if (columnName == "paytoorder_desc" || columnName == "paytoorder_desc_1") {
                var bfvalue = newValue;
                var afvalue = "00100" + bfvalue;
                objdw_main.SetItem(rowNumber, columnName, newValue);
                objdw_main.AcceptText();

                Gcoop.GetEl("Hdcoopid").value = newValue;
                jsPaycoopid();
            }
            else if (columnName == "loanobjective_code" || columnName == "loanobjective_code_1") {


                objdw_main.SetItem(rowNumber, columnName, newValue);
                objdw_main.AcceptText();
                Gcoop.GetEl("Hdobjective").value = newValue;
                jsObjective();
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
                    Gcoop.CheckDw(sender, rowNumber, objectName, "loanrcvfix_flag", 1, 0);
                    jsRefresh();
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

            }
            else if (objectName == "b_search") {
                Gcoop.GetEl("HdColumnName").value = objectName;
                var coop_id = objdw_main.GetItem(rowNumber, "memcoop_id")
                Gcoop.OpenDlg('600', '600', 'w_dlg_sl_loanmember_search.aspx', "?coopId=" + coop_id);
            } else if (objectName == "b_mthpay") {
                var memberNo = objdw_main.GetItem(1, "member_no");
                if ((memberNo != "") && (memberNo != null) && (memberNo != "00000000")) {
                    var member_no = objdw_main.GetItem(1, "member_no");
                    var income = objdw_main.GetItem(1, "incomemonth_other");
                    var paymonth = objdw_main.GetItem(1, "paymonth_other");
                    Gcoop.OpenIFrame(550, 450, "w_dlg_sl_loanrequest_monthpay.aspx", "?income=" + income + "&paymonth=" + paymonth + "&member_no=" + member_no);
                    return 0;
                }
                else {
                    alert("ไม่สามารถแสดงรายการได้ กรุณากรอกเลขทะเบียนก่อน");
                }

            } else if (objectName == "b_checksum") {
                jsPermissSalary();
            } else if (objectName == 'b_chgaccid') {
                //เปลี่ยนแปลงเลขที่บัญชี
                //                    alert(1);
                Gcoop.GetEl("HdColumnName").value = objectName;
                var memberNoVal = objdw_main.GetItem(1, "member_no");
                //                    alert(memberNoVal);
                if ((memberNoVal != null) && (memberNoVal != "")) {
                    //                    alert(3);
                    Gcoop.OpenDlg(620, 250, "w_dlg_show_accid.aspx", "?member=" + memberNoVal);
                }
            }
        }
        function GetValueAccID(dept_no, deptaccount_name, prncbal) {
            var colunmName = Gcoop.GetEl("HdColumnName").value;
            if (colunmName == "b_searchc") {
                var rowNumber = Gcoop.GetEl("HdRowNumber").value;
                // alert(prncbal);
                objdw_coll.SetItem(rowNumber, "ref_collno", dept_no);
                objdw_coll.SetItem(rowNumber, "description", deptaccount_name);
                objdw_coll.SetItem(rowNumber, "coll_balance", prncbal);
                objdw_coll.SetItem(rowNumber, "use_amt", prncbal);
                Gcoop.GetEl("HUseamt").value = prncbal;
                objdw_coll.AcceptText();
                jsPostreturn();
            }
            else if (colunmName == "b_chgaccid") {
                objdw_main.SetItem(1, "expense_accid", dept_no);
                objdw_main.AcceptText();
            }

        }
        //**************** End. 1.dw_main  ****************


        //**************** Start. 2.dw_coll  ค้ำประกัน ****************

        //Event----->ClientEventItemChanged="ItemDwCollChanged"
        function ItemDwCollChanged(sender, rowNumber, columnName, newValue) {
            Gcoop.GetEl("HdRowNumber").value = rowNumber + "";
            if (columnName == "ref_collno") {
                objdw_coll.SetItem(rowNumber, columnName, newValue);
                objdw_coll.AcceptText();
                Gcoop.GetEl("HdRefcoll").value = newValue;
                var loancolltype_code = objdw_coll.GetItem(rowNumber, "loancolltype_code");
                if (loancolltype_code == "01") {
                    jsGetMemberCollno();
                }

            } else if (columnName == "loancolltype_code") {
                objdw_coll.SetItem(rowNumber, columnName, newValue);
                objdw_coll.AcceptText();
                var loancolltype_code = objdw_coll.GetItem(rowNumber, "loancolltype_code");
                if (loancolltype_code == "02") {
                    jsGetMemberCollno();
                }

            }
            else if (columnName == "use_amt") {
                objdw_coll.SetItem(rowNumber, columnName, newValue);
                objdw_coll.AcceptText();
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

        //Event----->ClientEventButtonClicked="OnDwCollClicked"
        function OnDwCollClicked(sender, rowNumber, buttonName) {
            var collTypeCode = objdw_coll.GetItem(rowNumber, "loancolltype_code");
            Gcoop.GetEl("HdReturn").value = rowNumber + "";
            Gcoop.GetEl("HdRowNumber").value = rowNumber + "";
            if (buttonName == "b_delrow") {
                objdw_coll.DeleteRow(rowNumber);
            }
            else if (buttonName == "b_detail") {

                var collNo = objdw_coll.GetItem(1, "ref_collno");

                if ((collNo != "") && (collNo != null)) {
                    jsSetDataList();
                    var refCollNo = objdw_coll.GetItem(rowNumber, "ref_collno");
                    var coop_id = objdw_coll.GetItem(rowNumber, "coop_id");
                    var collType = objdw_coll.GetItem(rowNumber, "loancolltype_code");
                    var requestDate = objdw_main.GetItem(rowNumber, "loanrequest_tdate");
                    Gcoop.OpenDlg('600', '450', 'w_dlg_sl_loanrequest_coll.aspx', "?refCollNo=" + refCollNo + "&coop_id=" + coop_id + "&collType=" + collType + "&date=" + requestDate);
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
                Gcoop.GetEl("HdColumnName").value = "b_searchc";
                // ค้นหาบัญชีเงินฝาก 
                var memberNoVal = objdw_main.GetItem(1, "member_no");

                if ((memberNoVal != null) && (memberNoVal != "")) {
                    Gcoop.OpenDlg(620, 250, "w_dlg_show_accid.aspx", "?member=" + memberNoVal);
                }

            }
            else if ((buttonName == "b_searchc") && (collTypeCode == '04')) {
                //ค้นหาหลักทรัพย์ค้ำประกัน 
                Gcoop.OpenDlg('600', '450', 'w_dlg_sl_collmaster_search_req.aspx', '');
            }
            else if (buttonName == "b_addrow") {
                objdw_coll.InsertRow(objdw_coll.RowCount() + 1);
            }
            else if (buttonName == "b_recoll") {
                jsCollInitP();
            }
            else if (buttonName == "b_condition") {
                jsCollCondition();
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


        //Event----->ClientEventButtonClicked="OnDwOtherclrClicked"
        function OnDwOtherclrClicked(sender, rowNumber, buttonName) {
            if (buttonName == "b_delete") {
                objdw_otherclr.DeleteRow(rowNumber);
            }
            else if (buttonName == "b_addrow") {
                objdw_otherclr.InsertRow(objdw_otherclr.RowCount() + 1);
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
                jsSumOthClr();
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

            } else if (objectName == "b_checksum") {
                jsPermissSalary();
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
            objdw_coll.SetItem(rowNumber, "ref_collno", collRefNo);
            objdw_coll.SetItem(rowNumber, "description", desc);
            objdw_coll.SetItem(rowNumber, "coll_balance", mortgage_price);
            objdw_coll.SetItem(rowNumber, "use_amt", (mortgage_price * 90) / 100);
            Gcoop.GetEl("HUseamt").value = (mortgage_price * 90) / 100;
            Gcoop.GetEl("HdRefcollNO").value = collRefNo;
            objdw_coll.AcceptText();
            jsPostreturn();
        }

        //ฟังก์ชั่นรายงาน**********************************************************************
        function OnClickLinkNext() {

            objdw_main.AcceptText();
            jspopupReport();
        }

        function OnclickLinkNextinvoice() {
            objdw_main.AcceptText();
            jspopupReportInvoice();
        }
        //*************************************************************************************
        function SheetLoadComplete() {
            if (Gcoop.GetEl("HdIsPostBack").value != "true") {

                Gcoop.SetLastFocus("member_no_0");
                Gcoop.Focus();
            }
        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
     <asp:TextBox ID="txt_reqNo" runat="server" Visible="false"></asp:TextBox>
    <asp:TextBox ID="txt_member_no" runat="server" Visible="false"></asp:TextBox>
    <asp:LinkButton ID="LinkButton2" runat="server" Visible="false" OnClick="LinkButton2_Click">พิมพ์สัญญาเงินกู้</asp:LinkButton>
    <br />
    <br />
    <dw:WebDataWindowControl ID="dw_main" runat="server" DataWindowObject="d_sl_loanrequest_master"
        LibraryList="~/DataWindow/Shrlon/sl_loan_requestment_cen.pbl" AutoRestoreContext="False"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
        ClientEventItemChanged="ItemDwMainChanged" ClientEventClicked="OnDwMainClicked"
        ClientFormatting="True" TabIndex="1">
    </dw:WebDataWindowControl>
    <br />
    <asp:Label ID="Label3" runat="server" Text="หลักประกัน" Font-Bold="True"></asp:Label>
    <%--   <asp:CheckBox ID="Checkcollloop" runat="server" AutoPostBack="True" OnCheckedChanged="CheckColl"
        Text="สถานะค้ำวน" />--%>
    <asp:CheckBox ID="CbCheckcoop" runat="server" AutoPostBack="True" Text="ข้ามสาขา" />
    <dw:WebDataWindowControl ID="dw_coll" runat="server" DataWindowObject="d_sl_loanrequest_collateral"
        LibraryList="~/DataWindow/Shrlon/sl_loan_requestment_cen.pbl" AutoRestoreContext="False"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
        ClientEventItemChanged="ItemDwCollChanged" ClientEventButtonClicked="OnDwCollClicked"
        ClientFormatting="True" TabIndex="250">
    </dw:WebDataWindowControl>
    <table style="width: 100%;" border="0">
        <tr>
            <td>
                <asp:Label ID="Label5" runat="server" Text="รายการหักอื่นๆ" Font-Bold="True"></asp:Label>
                <dw:WebDataWindowControl ID="dw_otherclr" runat="server" DataWindowObject="d_sl_loanrequest_otherclr"
                    LibraryList="~/DataWindow/Shrlon/sl_loan_requestment_cen.pbl" AutoRestoreContext="False"
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
                    ClientScriptable="True" ClientEventItemChanged="ItemDwOtherclrChanged" ClientEventButtonClicked="OnDwOtherclrClicked"
                    BorderStyle="solid" BorderColor="white" TabIndex="500">
                </dw:WebDataWindowControl>
            </td>
            <td>
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
    <asp:HiddenField ID="HdReturn" runat="server" />
    <asp:HiddenField ID="Hdmembtype_code" runat="server" />
    <asp:HiddenField ID="HdColumnName" runat="server" />
    <asp:HiddenField ID="HdMemberNo" runat="server" />
    <asp:HiddenField ID="HdRefcoll" runat="server" />
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
</asp:Content>
