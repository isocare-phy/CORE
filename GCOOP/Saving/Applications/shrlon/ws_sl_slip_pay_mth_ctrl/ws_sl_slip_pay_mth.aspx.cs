using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary.WcfNShrlon;
using CoreSavingLibrary.WcfNCommon;
using DataLibrary;
using System.Globalization;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Saving.Applications.shrlon.ws_sl_slip_pay_mth_ctrl
{
    public partial class ws_sl_slip_pay_mth : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public string PostMemberNo { get; set; }
        [JsPostBack]
        public string PostOperateFlag { get; set; }
        [JsPostBack]
        public string PostOperateFlagL { get; set; }
        [JsPostBack]
        public string PostOperateFlagE { get; set; }
        [JsPostBack]
        public string PostAccidFlag { get; set; }
        [JsPostBack]
        public string PostSlipItem { get; set; }
        [JsPostBack]
        public string PostInsertRow { get; set; }
        [JsPostBack]
        public string PostOperateDate { get; set; }
        [JsPostBack]
        public string PostMoneytype { get; set; }
        [JsPostBack]
        public string PostSearchRetrieve { get; set; }
        [JsPostBack]
        public string PostPrint { get; set; }
        [JsPostBack]
        public string PostDeleteRow { get; set; }
        [JsPostBack]
        public string PostSliptypecode { get; set; }
        [JsPostBack]
        public string PostPayspecMethod { get; set; }
        [JsPostBack]
        public string PostRePayspecMethod { get; set; }
        [JsPostBack]
        public string PostRecvPeriod { get; set; }
        [JsPostBack]
        public string PostReceiptNo { get; set; }
        [JsPostBack]
        public string PostAdjust_payment { get; set; }
        


        DateTime idtm_lastDate;
        DateTime idtm_activedate = new DateTime();
        CultureInfo th = System.Globalization.CultureInfo.GetCultureInfo("th-TH");

        public void InitJsPostBack()
        {
            dsMain.InitDsMain(this);
            //dsDetailShare.InitDsDetailShare(this);
            dsPayment.InitDsPayment(this);
            dsDetailLoan.InitDsDetailLoan(this);
            //dsDetailEtc.InitDsDetailEtc(this);
        }

        public void WebSheetLoadBegin()
        {

            if (!IsPostBack)
            {
                dsMain.DATA[0].SLIP_DATE = state.SsWorkDate;
                dsMain.DATA[0].OPERATE_DATE = state.SsWorkDate;
                of_activeworkdate();
                dsMain.DdSliptype();
                dsMain.DdTofromAccBlank();//ทดสอบ Dd ช่องว่าง
                dsMain.DdMoneyType();
                //dsDetailEtc.DdLoanType();
                dsMain.DATA[0].SLIPTYPE_CODE = "PMX"; //fix ชำระรายเดือน ให้เปลี่ยนทุกไซต์เป็น PMX 
                dsMain.DATA[0].ENTRY_ID = state.SsUsername;
                //add_row.Visible = false;
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == PostMemberNo)
            {
                //add_row.Visible = true;

                this.InitLnRcv();

            }
                //tomy ทำเพิ่มเลือกงวด
            else if (eventArg == PostRecvPeriod)
            {
                string member_no = dsMain.DATA[0].MEMBER_NO;
                string mem_no = WebUtil.MemberNoFormat(member_no);
                string recv_period = dsMain.DATA[0].RECV_PERIOD;
                dsMain.RetrieveReceipt(mem_no, recv_period);

            }
             //   tomy init หนี้ท่อนนี้ 21/11/2017  เพิ่มเติม พี่โอ๋ให้ init ตามงวด ให้ init ทั้งหมดตอนโหลดเลย
            else if (eventArg == PostReceiptNo)
            {
                string member_no = dsMain.DATA[0].MEMBER_NO;
                string recv_period = dsMain.DATA[0].RECV_PERIOD;
                string receipt_no = dsMain.DATA[0].RECEIPT_NO;
                dsDetailLoan.RetrievePrinInt(member_no, recv_period, receipt_no);
            }
                
            else if (eventArg == PostMoneytype)
            {
                string sliptype_code = dsMain.DATA[0].SLIPTYPE_CODE;
                string moneytype_code = dsMain.DATA[0].MONEYTYPE_CODE;
                dsMain.DATA[0].TOFROM_ACCID = "";
                dsMain.DdFromAccId(sliptype_code, moneytype_code);
                SetDefaultTofromaccid();
                if (moneytype_code != "TRN")
                {
                    dsMain.DATA[0].REF_SLIPAMT = 0;
                    dsMain.DATA[0].REF_SLIPNO = "";
                    dsMain.DATA[0].REF_SYSTEM = "";
                    dsMain.DATA[0].EXPENSE_ACCID = "";
                }
            }
                //tomy หุ้น ยังไม่ใช้
            //else if (eventArg == PostOperateFlag)
            //{
            //    int row = dsDetailShare.GetRowFocus();
            //    decimal operate_flag = dsDetailShare.DATA[row].OPERATE_FLAG;
            //    decimal bfshrcont_balamt = dsDetailShare.DATA[row].BFSHRCONT_BALAMT;
            //    decimal item_balance = dsDetailShare.DATA[row].ITEM_BALANCE;
            //    decimal bfperiod = dsDetailShare.DATA[row].BFPERIOD;

            //    if (operate_flag == 1)
            //    {
            //        dsDetailShare.DATA[row].BFSHRCONT_BALAMT = bfshrcont_balamt;
            //        if (bfshrcont_balamt == 0 && bfperiod == 0)
            //        {
            //            dsDetailShare.DATA[row].PERIOD = 1;
            //            dsDetailShare.DATA[row].PERIODCOUNT_FLAG = 1;
            //            dsDetailShare.DATA[row].ITEM_PAYAMT = dsDetailShare.DATA[row].BFPERIOD_PAYMENT;
            //        }
            //        dsDetailShare.DATA[row].ITEM_BALANCE = item_balance;
            //        //dsDetailShare.DATA[row].PERIOD
            //        calItemPay();
            //    }
            //    else if (operate_flag == 0)
            //    {
            //        dsDetailShare.DATA[row].PERIOD = bfperiod;
            //        dsDetailShare.DATA[row].PERIODCOUNT_FLAG = 0;
            //        dsDetailShare.DATA[row].PRINCIPAL_PAYAMT = 0;
            //        dsDetailShare.DATA[row].INTEREST_PAYAMT = 0;
            //        dsDetailShare.DATA[row].ITEM_PAYAMT = 0;
            //        calItemPay();
            //    }
            //}
            else if (eventArg == PostOperateFlagL)
            {
                int rowl = dsDetailLoan.GetRowFocus();
                decimal operate_flag_l = dsDetailLoan.DATA[rowl].OPERATE_FLAG;
                decimal prn_payamt = dsDetailLoan.DATA[rowl].PRINCIPAL_PAYAMT;
                decimal int_payamt = dsDetailLoan.DATA[rowl].INTEREST_PAYAMT;
                decimal item_payamt = dsDetailLoan.DATA[rowl].ITEM_PAYAMT;

                if (operate_flag_l == 1)
                {

                    dsDetailLoan.DATA[rowl].PRINCIPAL_PAYAMT = prn_payamt;
                    dsDetailLoan.DATA[rowl].INTEREST_PAYAMT = int_payamt; //นำ comment ออกก่อน เนื่องจากกรณีที่ สัญญาที่ไม่เรียกเก็บกรณีมาชำระพิเศษแล้วดอกเบี้ยที่ต้องชำระไม่ set ค่าให้ by.cherry 
                    dsDetailLoan.DATA[rowl].ITEM_PAYAMT = item_payamt;       
                }
                else
                {
                    dsDetailLoan.DATA[rowl].PRINCIPAL_PAYAMT = 0;
                    dsDetailLoan.DATA[rowl].INTEREST_PAYAMT = 0;
                    dsDetailLoan.DATA[rowl].ITEM_PAYAMT = 0;                    
                }
                calItemPay();
            }
               // tomy อื่นๆ ยังไม่ใช้
            //else if (eventArg == PostOperateFlagE)
            //{
            //    int rowe = dsDetailEtc.GetRowFocus();
            //    decimal operate_flag_e = dsDetailEtc.DATA[rowe].OPERATE_FLAG;
            //    decimal ldc_bfshrcontbal = dsDetailEtc.DATA[rowe].BFSHRCONT_BALAMT;

            //    if (operate_flag_e == 1)
            //    {

            //        dsDetailEtc.DATA[rowe].ITEM_PAYAMT = ldc_bfshrcontbal;

            //        calItemPay();
            //    }
            //    else if (operate_flag_e == 0)
            //    {
            //        dsDetailEtc.DATA[rowe].ITEM_PAYAMT = 0;
            //        calItemPay();
            //    }
            //}
//            else if (eventArg == PostSlipItem)
//            {
//                int row = dsDetailEtc.GetRowFocus();
//                string slipitemtype_code = dsDetailEtc.DATA[row].SLIPITEMTYPE_CODE;
//                //dsAdd.ItemType(slipitemtype_code);
//                string sql = @" 
//                 SELECT SLUCFSLIPITEMTYPE.SLIPITEMTYPE_CODE,   SLUCFSLIPITEMTYPE.SLIPITEMTYPE_DESC
//                 FROM SLUCFSLIPITEMTYPE  
//                 WHERE SLUCFSLIPITEMTYPE.SLIPITEMTYPE_CODE = {0}";
//                sql = WebUtil.SQLFormat(sql, slipitemtype_code);
//                Sdt dt = WebUtil.QuerySdt(sql);
//                if (dt.Next())
//                {
//                    dsDetailEtc.DATA[row].SLIPITEM_DESC = dt.GetString("SLIPITEMTYPE_DESC");
//                }
//            }
            //else if (eventArg == PostInsertRow)
            //{
            //    dsDetailEtc.InsertLastRow();
            //    int currow = dsDetailEtc.RowCount - 1;
            //    try
            //    {
            //        dsDetailEtc.DATA[currow].SEQ_NO = dsDetailEtc.GetMaxValueDecimal("SEQ_NO") + 1;
            //    }
            //    catch
            //    {
            //        if (dsDetailEtc.RowCount < 1)
            //        {
            //            dsDetailEtc.DATA[currow].SEQ_NO = 1;
            //        }
            //    }
            //    dsDetailEtc.DdLoanType();
            //}
            //else if (eventArg == PostDeleteRow)
            //{
            //    int row = dsDetailEtc.GetRowFocus();
            //    dsDetailEtc.DeleteRow(row);
            //}
            else if (eventArg == PostOperateDate)
            {
                ReCalint();
            }
            else if (eventArg == PostSearchRetrieve)
            {
                string payinslip_no = HdPayNo.Value;

                HdPayNo.Value = "";

                dsMain.RetrieveMain(payinslip_no);
                dsDetailLoan.RetrieveDetailLoan(payinslip_no);
                //dsDetailShare.RetrieveDetailLoan(payinslip_no);
                //dsDetailEtc.RetrieveDetailEtc(payinslip_no);

                string moneytype_code = dsMain.DATA[0].MONEYTYPE_CODE;
                string sliptype_code = dsMain.DATA[0].SLIPTYPE_CODE;

                dsMain.DdFromAccId(sliptype_code, moneytype_code);
                dsMain.DdMoneyType();
                SetDefaultTofromaccid();
            }
            else if (eventArg == PostPrint)
            {
                string rslip = "";

                rslip = dsMain.DATA[0].PAYINSLIP_NO;
                string sqlprint = "select printslip_type, ireport_obj from lnloanconstant ";
                Sdt dtp = WebUtil.QuerySdt(sqlprint);
                string printtype = "PB", ireportobj = "r_sl_slip_in_exat_a4_table";
                //alter table lnloanconstant add printslip_type varchar2(2);
                //alter table lnloanconstant add ireport_obj varchar2(50);
                if (dtp.Next())
                {
                    printtype = dtp.GetString("printslip_type");
                    ireportobj = dtp.GetString("ireport_obj");

                }
                else
                {
                    printtype = "PB";
                    ireportobj = "r_sl_slip_in_exat_a4_table";
                }
                if (rslip != "")
                {
                    if (printtype == "IR")
                    {
                        Printing.PrintSlipSlInOutIreportExat(this, rslip, state.SsCoopControl, ireportobj);
                    }
                    else if (printtype == "JS")
                    {
                        Printing.ShrlonPrintSlipPayIn(this, state.SsCoopControl, rslip, 2);
                    }
                    else {
                        Printing.PrintSlipSlpayin(this, rslip, state.SsCoopControl);
                    }
                    
                }
            }
            else if (eventArg == PostSliptypecode)
            {
                string member_no = dsMain.DATA[0].MEMBER_NO;
                if (member_no != "")
                {
                    //add_row.Visible = true;
                    this.InitLnRcv();
                }
            }
            else if (eventArg == PostPayspecMethod)
            {
                ReCalint();
                int r = dsDetailLoan.GetRowFocus();
                SetOnLoadedScript("ReCalculate(" + r + ")");
            }
            else if (eventArg == PostRePayspecMethod)
            {
                int row = dsDetailLoan.GetRowFocus();
                DateTime calintfrom = dsDetailLoan.DATA[row].BFLASTCALINT_DATE;
                string contno = dsDetailLoan.DATA[row].LOANCONTRACT_NO;
                decimal item_payamt = dsDetailLoan.DATA[row].ITEM_PAYAMT;
                DateTime calintto = dsMain.DATA[0].OPERATE_DATE;
                decimal prnbal_payamt = 0;
                decimal interest_payamt = 0;
                decimal intarrear = 0;
                try
                {
                    //tomy ชำระรายเดือนยังไม่ต้องคิดคำนวน
                    //Int16 xml_re = wcf.NShrlon.of_calrevertpaymeth2(state.SsWsPass, contno, calintfrom, calintto, item_payamt, ref prnbal_payamt, ref interest_payamt);
                    //if (xml_re == 1)
                    //{

                        dsDetailLoan.DATA[row].ITEM_PAYAMT = item_payamt;
                        intarrear = dsDetailLoan.DATA[row].BFINTARR_AMT;
                        decimal sum = prnbal_payamt + interest_payamt;
                        decimal total = sum - item_payamt;
                        prnbal_payamt = prnbal_payamt - total;
                        dsDetailLoan.DATA[row].PRINCIPAL_PAYAMT = prnbal_payamt - intarrear;
                        dsDetailLoan.DATA[row].INTEREST_PAYAMT = interest_payamt + intarrear;

                        //mike 
                        dsDetailLoan.DATA[row].INTEREST_PERIOD = interest_payamt;
                        calItemPay();
                        dsDetailLoan.DATA[row].ITEM_BALANCE = dsDetailLoan.DATA[row].BFSHRCONT_BALAMT - dsDetailLoan.DATA[row].PRINCIPAL_PAYAMT;
                    //}
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            }
               //หมี
            else if (eventArg == PostAdjust_payment)
            {
                decimal start_pay = dsPayment.DATA[0].SUMITEM_PAYMENT;  // 7000
                decimal sum_int = 0; // ดอกรวมจ่าย
                decimal start_principal = 0;
                decimal sum_principal = 0;
                decimal last_pay = 0;
                decimal check_pay = dsPayment.DATA[0].SUMITEM_PAYMENT;
                Clear_Payment();
                // start interest
                for (int i = 0; i < dsDetailLoan.RowCount; i++)
                {
                    if (dsDetailLoan.DATA[i].ADJUST_INTAMT != 0)
                    {
                        check_pay = check_pay - dsDetailLoan.DATA[i].ADJUST_INTAMT; //
                        if (check_pay > 0)
                        {
                            dsDetailLoan.DATA[i].INTEREST_PAYAMT = dsDetailLoan.DATA[i].ADJUST_INTAMT;
                            sum_int += dsDetailLoan.DATA[i].ADJUST_INTAMT;
                        }
                        else
                        {
                            last_pay = check_pay + dsDetailLoan.DATA[i].ADJUST_INTAMT;
                            dsDetailLoan.DATA[i].INTEREST_PAYAMT = last_pay;
                            sum_int += last_pay;
                        }
                    }
                    if (sum_int == start_pay)
                    {
                        break;
                    }
                }

                dsPayment.DATA[0].SUMINT_PAYMENT = sum_int;
                // end interest
                //start principal
                start_principal = start_pay - sum_int;
                check_pay = start_principal;
                last_pay = 0;
                if (start_principal > 0)
                {
                    for (int i = 0; i < dsDetailLoan.RowCount; i++)
                    {
                        if (dsDetailLoan.DATA[i].ADJUST_PRNAMT != 0)
                        {
                            check_pay = check_pay - dsDetailLoan.DATA[i].ADJUST_PRNAMT; //
                            if (check_pay > 0)
                            {
                                dsDetailLoan.DATA[i].PRINCIPAL_PAYAMT = dsDetailLoan.DATA[i].ADJUST_PRNAMT;
                                sum_principal += dsDetailLoan.DATA[i].ADJUST_PRNAMT;
                            }
                            else
                            {
                                last_pay = check_pay + dsDetailLoan.DATA[i].ADJUST_PRNAMT;
                                dsDetailLoan.DATA[i].PRINCIPAL_PAYAMT = last_pay;
                                sum_principal += last_pay;
                            }
                        }
                        if (sum_principal == start_principal)
                        {
                            break;
                        }
                    }
                }
                dsPayment.DATA[0].SUMPRN_PAYMENT = sum_principal;

                // end principal
                // update item_payment
                dsPayment.DATA[0].SUMITEM_PAYMENT = sum_principal + sum_int;
                decimal total_prin = 0, total_int = 0, pay_prin = 0, pay_int = 0;
                for (int i = 0; i < dsDetailLoan.RowCount; i++)
                {
                    dsDetailLoan.DATA[i].ITEM_PAYAMT = dsDetailLoan.DATA[i].INTEREST_PAYAMT + dsDetailLoan.DATA[i].PRINCIPAL_PAYAMT;
                    pay_prin += dsDetailLoan.DATA[i].PRINCIPAL_PAYAMT;
                    pay_int += dsDetailLoan.DATA[i].INTEREST_PAYAMT;
                    total_prin += dsDetailLoan.DATA[i].ADJUST_PRNAMT;
                    total_int += dsDetailLoan.DATA[i].ADJUST_INTAMT;
                }

                dsDetailLoan.DATA[0].SUM_INTAMT = total_prin - pay_prin;
                dsDetailLoan.DATA[0].SUM_PRNAMT = total_int - pay_int;
                // end update

            }
        }

        public void ReCalint()
        {
            //tomy ชำระรายเดือนตอนนี้ยังไม่ต้องทำ 
            //DateTime operate_date = dsMain.DATA[0].OPERATE_DATE;
            //string xml_sliplon = dsDetailLoan.ExportXml();
            //string sliptype_code = dsMain.DATA[0].SLIPTYPE_CODE;
            //try
            //{
            //    Int16 xml_re = wcf.NShrlon.of_initslippayin_calint(state.SsWsPass, ref xml_sliplon, sliptype_code, operate_date);
            //    if (xml_re == 1)
            //    {
            //        dsDetailLoan.ResetRow();
            //        dsDetailLoan.ImportData(xml_sliplon);
            //    }
            //    AbsIntpay();
            //}
            //catch (Exception ex)
            //{
            //    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            //}
        }

        public void SaveWebSheet()
        {
            try
            {
                str_slippayin strslip = new str_slippayin();
                //strslip.coop_id = state.SsCoopControl;
                //strslip.entry_id = state.SsUsername;
                //strslip.xml_sliphead = dsMain.ExportXml();
                //strslip.xml_slipshr = dsDetailShare.ExportXml();
                //strslip.xml_sliplon = dsDetailLoan.ExportXml();
                //strslip.xml_slipetc = dsDetailEtc.ExportXml();

                idtm_lastDate = dsMain.DATA[0].SLIP_DATE;
                // strslip.xml_sliphead = dsMain.ExportXmlPBFormat("d_sl_payinslip");

                //wcf.NShrlon.of_saveslip_payin(state.SsWsPass, ref strslip);

                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลสำเร็จ ");

                decimal print = dsMain.DATA[0].PRINT;

                string sqlprint = "select printslip_type, ireport_obj from lnloanconstant ";
                Sdt dtp = WebUtil.QuerySdt(sqlprint);
                string printtype = "PB", ireportobj = "r_sl_slip_in_exat_a4_table";
                //alter table lnloanconstant add printslip_type varchar2(2);
                //alter table lnloanconstant add ireport_obj varchar2(50);
                if (dtp.Next())
                {
                    printtype = dtp.GetString("printslip_type");
                    ireportobj = dtp.GetString("ireport_obj");

                }
                else
                {
                    printtype = "PB";
                    ireportobj = "r_sl_slip_in_exat_a4_table";
                }
                if (print == 1)
                {

                    if (printtype == "IR")
                    {
                        //พิมพ์แบบireport

                        String PayinslipNo = strslip.payinslip_no.Trim();
                        if (state.SsCoopControl == "020001")
                        {

                            Printing.PrintIRSlippayInPBN(this, PayinslipNo, ireportobj);
                        }
                        else if (state.SsCoopControl == "022001")
                        {
                            Printing.RePrintSlipSlInIrExat(this, PayinslipNo, state.SsCoopControl);
                        }
                        Printing.RePrintSlipSlInIreportExat(this, PayinslipNo, state.SsCoopControl, ireportobj);

                    }
                    else if (printtype == "JS")
                    {
                        //พิมพ์่jsslip
                        if (state.SsCoopControl == "027001")
                        {
                            //ใช้ดป็น PBSLIP แต่ base เป็น JS
                            string payinslip_no = strslip.payinslip_no;
                            Printing.PrintSlipSlpayin(this, payinslip_no, state.SsCoopControl);
                        }
                        else
                        {
                            String PayinslipNo = strslip.payinslip_no.Trim();
                            Printing.ShrlonPrintSlipPayIn(this, state.SsCoopControl, PayinslipNo, 2);
                        }
                    }
                    else
                    {
                        //PBSLIP
                        string payinslip_no = strslip.payinslip_no;
                        Printing.PrintSlipSlpayin(this, payinslip_no, state.SsCoopControl);
                    }

                }
                Session["sliptype"] = dsMain.DATA[0].SLIPTYPE_CODE;
                dsMain.ResetRow();
                //dsDetailShare.ResetRow();
                dsDetailLoan.ResetRow();
                //dsDetailEtc.ResetRow();
                refresh();
                if (idtm_lastDate != state.SsWorkDate)
                {
                    dsMain.DATA[0].SLIP_DATE = idtm_lastDate;
                    dsMain.DATA[0].OPERATE_DATE = idtm_lastDate;
                }
                else
                {
                    dsMain.DATA[0].SLIP_DATE = state.SsWorkDate;
                    dsMain.DATA[0].OPERATE_DATE = state.SsWorkDate;
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void WebSheetLoadEnd()
        {
            //for (int i = 0; i < dsDetailShare.RowCount; i++)
            //{
            //    if (dsDetailShare.DATA[i].OPERATE_FLAG == 1)
            //    {
            //        dsDetailShare.FindTextBox(i, dsDetailShare.DATA.SLIPITEMColumn).ReadOnly = true;
            //        dsDetailShare.FindTextBox(i, dsDetailShare.DATA.PERIODColumn).Enabled = true;
            //        dsDetailShare.FindCheckBox(i, dsDetailShare.DATA.PERIODCOUNT_FLAGColumn).Enabled = true;
            //        dsDetailShare.FindTextBox(i, dsDetailShare.DATA.BFSHRCONT_BALAMTColumn).ReadOnly = true;
            //        dsDetailShare.FindTextBox(i, dsDetailShare.DATA.ITEM_PAYAMTColumn).ReadOnly = false;
            //        dsDetailShare.FindTextBox(i, dsDetailShare.DATA.ITEM_BALANCEColumn).ReadOnly = true;
            //    }
            //    else
            //    {
            //        dsDetailShare.FindTextBox(i, dsDetailShare.DATA.SLIPITEMColumn).ReadOnly = true;
            //        dsDetailShare.FindTextBox(i, dsDetailShare.DATA.PERIODColumn).ReadOnly = true;
            //        dsDetailShare.FindTextBox(i, dsDetailShare.DATA.BFSHRCONT_BALAMTColumn).ReadOnly = true;
            //        dsDetailShare.FindTextBox(i, dsDetailShare.DATA.ITEM_PAYAMTColumn).ReadOnly = true;
            //        dsDetailShare.FindTextBox(i, dsDetailShare.DATA.ITEM_BALANCEColumn).ReadOnly = true;
            //        dsDetailShare.FindCheckBox(i, dsDetailShare.DATA.PERIODCOUNT_FLAGColumn).Enabled = false;
            //    }
            //}
            for (int k = 0; k < dsDetailLoan.RowCount; k++)
            {
                if (dsDetailLoan.DATA[k].OPERATE_FLAG == 1)
                {
                    dsDetailLoan.FindTextBox(k, dsDetailLoan.DATA.RECV_PERIODColumn).ReadOnly = true;
                    dsDetailLoan.FindTextBox(k, dsDetailLoan.DATA.RECEIPT_NOColumn).ReadOnly = true;
                    dsDetailLoan.FindTextBox(k, dsDetailLoan.DATA.ADJUST_PRNAMTColumn).ReadOnly = true;
                    dsDetailLoan.FindTextBox(k, dsDetailLoan.DATA.ADJUST_INTAMTColumn).ReadOnly = true;
                    dsDetailLoan.FindTextBox(k, dsDetailLoan.DATA.PRINCIPAL_PAYAMTColumn).ReadOnly = false;
                    dsDetailLoan.FindTextBox(k, dsDetailLoan.DATA.INTEREST_PAYAMTColumn).ReadOnly = false;
                    dsDetailLoan.FindTextBox(k, dsDetailLoan.DATA.ITEM_PAYAMTColumn).ReadOnly = false;
                }
                else
                {
                    dsDetailLoan.FindTextBox(k, dsDetailLoan.DATA.RECV_PERIODColumn).ReadOnly = false;
                    dsDetailLoan.FindTextBox(k, dsDetailLoan.DATA.RECEIPT_NOColumn).ReadOnly = true;
                    dsDetailLoan.FindTextBox(k, dsDetailLoan.DATA.ADJUST_PRNAMTColumn).ReadOnly = true;
                    dsDetailLoan.FindTextBox(k, dsDetailLoan.DATA.ADJUST_INTAMTColumn).ReadOnly = true;
                    dsDetailLoan.FindTextBox(k, dsDetailLoan.DATA.PRINCIPAL_PAYAMTColumn).ReadOnly = true;
                    dsDetailLoan.FindTextBox(k, dsDetailLoan.DATA.INTEREST_PAYAMTColumn).ReadOnly = true;
                    dsDetailLoan.FindTextBox(k, dsDetailLoan.DATA.ITEM_PAYAMTColumn).ReadOnly = true;
                }
            }
            //for (int k = 0; k < dsDetailEtc.RowCount; k++)
            //{
            //    if (dsDetailEtc.DATA[k].OPERATE_FLAG == 1)
            //    {
            //        dsDetailEtc.FindDropDownList(k, dsDetailEtc.DATA.SLIPITEMTYPE_CODEColumn).Enabled = true;
            //        dsDetailEtc.FindTextBox(k, dsDetailLoan.DATA.SLIPITEM_DESCColumn).ReadOnly = true;
            //        dsDetailEtc.FindTextBox(k, dsDetailLoan.DATA.ITEM_PAYAMTColumn).ReadOnly = false;
            //        //dsDetailEtc.FindTextBox(k, dsDetailLoan.DATA.PRNCALINT_AMTColumn).ReadOnly = true;
            //    }
            //    else
            //    {
            //        dsDetailEtc.FindDropDownList(k, dsDetailEtc.DATA.SLIPITEMTYPE_CODEColumn).Enabled = false;
            //        dsDetailEtc.FindTextBox(k, dsDetailLoan.DATA.SLIPITEM_DESCColumn).ReadOnly = true;
            //        dsDetailEtc.FindTextBox(k, dsDetailLoan.DATA.ITEM_PAYAMTColumn).ReadOnly = true;
            //        //dsDetailEtc.FindTextBox(k, dsDetailLoan.DATA.PRNCALINT_AMTColumn).ReadOnly = true;
            //    }
            //}
            if (dsMain.DATA[0].ACCID_FLAG == 1)
            {
                dsMain.FindDropDownList(0, dsMain.DATA.TOFROM_ACCIDColumn).Enabled = true;
            }
            else
            {
                dsMain.FindDropDownList(0, dsMain.DATA.TOFROM_ACCIDColumn).Enabled = false;
            }

            if (HdSlipStatus.Value == "1")
            {
                //main 
                dsMain.FindTextBox(0, dsMain.DATA.OPERATE_DATEColumn).ReadOnly = true;
                dsMain.FindDropDownList(0, "SLIPTYPE_CODE").Enabled = false;
                dsMain.FindDropDownList(0, "moneytype_code").Enabled = false;
                dsMain.FindCheckBox(0, "accid_flag").Enabled = false;
                dsMain.FindDropDownList(0, "tofrom_accid").Enabled = false;
                dsMain.FindTextBox(0, dsMain.DATA.COMPUTE1Column).ReadOnly = true;
                dsMain.FindTextBox(0, dsMain.DATA.ENTRY_IDColumn).ReadOnly = true;
                dsMain.FindTextBox(0, dsMain.DATA.SLIP_AMTColumn).ReadOnly = true;
                dsMain.FindTextBox(0, dsMain.DATA.EXPENSE_ACCIDColumn).ReadOnly = true;
                dsMain.FindTextBox(0, dsMain.DATA.REF_SLIPAMTColumn).ReadOnly = true;

                //loan
                for (int i = 0; i < dsDetailLoan.RowCount; i++)
                {
                    dsDetailLoan.FindCheckBox(i, "operate_flag").Enabled = false;
                    dsDetailLoan.FindTextBox(i, "recv_period").ReadOnly = true;
                    dsDetailLoan.FindTextBox(i, "receipt_no").ReadOnly = true;
                    dsDetailLoan.FindTextBox(i, "adjust_prnamt").ReadOnly = true;
                    dsDetailLoan.FindTextBox(i, "adjust_intamt").ReadOnly = true;
                    dsDetailLoan.FindTextBox(i, "principal_payamt").ReadOnly = true;
                    dsDetailLoan.FindTextBox(i, "interest_payamt").ReadOnly = true;
                    dsDetailLoan.FindTextBox(i, "item_payamt").ReadOnly = true;
                }
                //share
                //for (int i = 0; i < dsDetailShare.RowCount; i++)
                //{
                //    dsDetailShare.FindCheckBox(i, "operate_flag").Enabled = false;
                //    dsDetailShare.FindTextBox(i, "slipitem").ReadOnly = true;
                //    dsDetailShare.FindCheckBox(i, "periodcount_flag").Enabled = false;
                //    dsDetailShare.FindTextBox(i, "period").ReadOnly = true;
                //    dsDetailShare.FindTextBox(i, "bfshrcont_balamt").ReadOnly = true;
                //    dsDetailShare.FindTextBox(i, "item_payamt").ReadOnly = true;
                //    dsDetailShare.FindTextBox(i, "item_balance").ReadOnly = true;
                //}
                // etc
                //for (int i = 0; i < dsDetailEtc.RowCount; i++)
                //{
                //    dsDetailEtc.FindCheckBox(i, "operate_flag").Enabled = false;
                //    dsDetailEtc.FindDropDownList(i, "slipitemtype_code").Enabled = false;
                //    dsDetailEtc.FindTextBox(i, "slipitem_desc").ReadOnly = true;
                //    dsDetailEtc.FindTextBox(i, "item_payamt").ReadOnly = true;
                //    dsDetailEtc.FindTextBox(i, "prncalint_amt").ReadOnly = true;
                //}
            }
        }
        public void refresh()
        {
            dsMain.DATA[0].SLIP_DATE = state.SsWorkDate;
            dsMain.DATA[0].OPERATE_DATE = state.SsWorkDate;
            of_activeworkdate();
            dsMain.DdSliptype();
            dsMain.DdTofromAccBlank();//ทดสอบ Dd ช่องว่าง
            dsMain.DdMoneyType();
            //dsDetailEtc.DdLoanType();
            dsMain.DATA[0].SLIPTYPE_CODE = Session["sliptype"].ToString();
            dsMain.DATA[0].ENTRY_ID = state.SsUsername;
            //add_row.Visible = false;
        }

        private void InitLnRcv()
        {
            try
            {
                //HfisCalInt.Value = "false";

                string member_no = dsMain.DATA[0].MEMBER_NO;
                string mem_no = WebUtil.MemberNoFormat(member_no);
                HdCoopControl.Value = state.SsCoopControl;
                
                //tomy เริ่ม set ค่า
                dsMain.RetrieveMember(mem_no);
                
                dsMain.DATA[0].SLIP_STATUS = 8;
                dsMain.DATA[0].SLIP_DATE = state.SsWorkDate;
                dsMain.DATA[0].OPERATE_DATE = state.SsWorkDate;
                dsMain.DATA[0].SLIPTYPE_CODE = "PMX"; //fix ชำระรายเดือน ให้เปลี่ยนทุกไซต์เป็น PMX 

                dsMain.DATA[0].ENTRY_ID = state.SsUsername;
                String mType = dsMain.DATA[0].MONEYTYPE_CODE;
                if (mType == "")
                {
                    dsMain.DATA[0].MONEYTYPE_CODE = "CSH";
                }
                string sliptype_code = dsMain.DATA[0].SLIPTYPE_CODE;
                string moneytype_code = dsMain.DATA[0].MONEYTYPE_CODE;

                dsMain.DdSliptype();
                dsMain.DdFromAccId(sliptype_code, moneytype_code);
                dsMain.DdMoneyType();

                //NEW--------------
                dsMain.RetrievePeriod(mem_no);
                string recv_period = dsMain.DATA[0].RECV_PERIOD;
                dsMain.RetrieveReceipt(mem_no,  recv_period);

                string receipt_no = dsMain.DATA[0].RECEIPT_NO;
                dsDetailLoan.RetrievePrinInt(mem_no, recv_period, receipt_no);
                //end set ค่า

            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }

            SetDefaultTofromaccid();
        }
        public void calItemPay()
        {
            //int row = dsDetailShare.RowCount;
            int rowl = dsDetailLoan.RowCount;
            //int rowe = dsDetailEtc.RowCount;

            decimal slip_amt = 0;
            //for (int i = 0; i < row; i++)
            //{
            //    decimal item_payamt = dsDetailShare.DATA[i].ITEM_PAYAMT;
            //    slip_amt += item_payamt;
            //}
            for (int k = 0; k < rowl; k++)
            {
                decimal item_payamt = dsDetailLoan.DATA[k].ITEM_PAYAMT;
                slip_amt += item_payamt;

            }
            //for (int j = 0; j < rowe; j++)
            //{
            //    decimal item_payamt = dsDetailEtc.DATA[j].ITEM_PAYAMT;
            //    slip_amt += item_payamt;

            //}

            dsMain.DATA[0].SLIP_AMT = slip_amt;

        }

        /// <summary>
        /// set คู่บัญชี
        /// </summary>
        private void SetDefaultTofromaccid()
        {
            string sql = @"select account_id
                from cmucftofromaccid 
                where coop_id = {0} 
	            and moneytype_code = {1}
	            and sliptype_code = {2}
	            and default_flag = 1";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, dsMain.DATA[0].MONEYTYPE_CODE, dsMain.DATA[0].SLIPTYPE_CODE);
            Sdt dt = WebUtil.QuerySdt(sql);
            if (dt.Next())
            {
                string accid = dt.GetString("account_id");
                dsMain.DATA[0].TOFROM_ACCID = accid;
            }
        }
        //หมี
        public void Clear_Payment()
        {
            for (int i = 0; i < dsDetailLoan.RowCount; i++)
            {
                dsDetailLoan.DATA[i].INTEREST_PAYAMT = 0;
                dsDetailLoan.DATA[i].PRINCIPAL_PAYAMT = 0;
                dsDetailLoan.DATA[i].ITEM_PAYAMT = 0;
            }
        }

        /// <summary>
        /// get วันทำการ
        /// </summary>       
        public void of_activeworkdate()
        {
            try
            {
                string sqlStr;
                int li_clsdaystatus = 0;
                DateTime ldtm_workdate;
                Sdt dt;
                sqlStr = @" select workdate, closeday_status
                    from amappstatus 
                    where coop_id = '" + state.SsCoopControl + @"'
                    and application = 'shrlon'";
                dt = WebUtil.QuerySdt(sqlStr);
                if (dt.Next())
                {
                    ldtm_workdate = dt.GetDate("workdate");
                    li_clsdaystatus = dt.GetInt32("closeday_status");
                    if (li_clsdaystatus == 1)
                    {
                        int result = wcf.NCommon.of_getnextworkday(state.SsWsPass, state.SsWorkDate, ref idtm_lastDate);
                    }
                    else
                    {
                        idtm_lastDate = state.SsWorkDate;
                    }
                }
                if (state.SsWorkDate != idtm_lastDate)
                {
                    dsMain.DATA[0].OPERATE_DATE = idtm_lastDate;
                    dsMain.DATA[0].SLIP_DATE = idtm_lastDate;
                    LtServerMessage.Text = WebUtil.WarningMessage("ระบบได้ทำการปิดวันไปแล้ว เปลี่ยนวันทำการเป็น " + idtm_lastDate.ToString("dd/MM/yyyy", th));
                    this.SetOnLoadedScript("alert('ระบบได้ทำการปิดวันไปแล้ว เปลี่ยนวันทำการเป็น " + idtm_lastDate.ToString("dd/MM/yyyy", th) + " ')");
                }
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
        } 
    }
}