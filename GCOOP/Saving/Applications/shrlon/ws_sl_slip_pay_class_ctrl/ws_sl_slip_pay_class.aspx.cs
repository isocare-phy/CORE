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
//using CoreSavingLibrary.WcfReport;
using System.Globalization;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;



namespace Saving.Applications.shrlon.ws_sl_slip_pay_class_ctrl
{
    public partial class ws_sl_slip_pay_class : PageWebSheet, WebSheet
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
        public string PostOperateFlagA { get; set; }
        [JsPostBack]
        public string PostAccidFlag { get; set; }
        [JsPostBack]
        public string PostSlipItem { get; set; }
        [JsPostBack]
        public string PostOperateDate { get; set; }
        [JsPostBack]
        public string PostMoneytype { get; set; }
        [JsPostBack]
        public string PostSearchRetrieve { get; set; }
        [JsPostBack]
        public string PostDeleteRow { get; set; }
        [JsPostBack]
        public string PostSliptypecode { get; set; }
        [JsPostBack]
        public string PostPayspecMethod { get; set; }
        [JsPostBack]
        public string PostRePayspecMethod { get; set; }
        [JsPostBack]
        public string PostChkLoanPayment { get; set; }
        [JsPostBack]
        public string PostInsertRow { get; set; }


        DateTime idtm_lastDate;
        CultureInfo th = System.Globalization.CultureInfo.GetCultureInfo("th-TH");

        public void InitJsPostBack()
        {
            dsMain.InitDsMain(this);
            dsDetailShare.InitDsDetailShare(this);
            dsDetailLoan.InitDsDetailLoan(this);
            dsDetailEtc.InitDsDetailEtc(this);
            dsDetailAdjust.InitDsDetailAdjust(this);
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
                dsDetailEtc.DdLoanType();
                dsMain.DATA[0].SLIPTYPE_CODE = "PX";
                dsMain.DATA[0].ENTRY_ID = state.SsUsername;
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == PostMemberNo)
            {
                this.InitLnRcv();
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
            else if (eventArg == PostOperateFlag)
            {
                int row = dsDetailShare.GetRowFocus();
                decimal operate_flag = dsDetailShare.DATA[row].OPERATE_FLAG;
                decimal bfshrcont_balamt = dsDetailShare.DATA[row].BFSHRCONT_BALAMT;
                decimal item_balance = dsDetailShare.DATA[row].ITEM_BALANCE;
                decimal bfperiod = dsDetailShare.DATA[row].BFPERIOD;

                if (operate_flag == 1)
                {
                    dsDetailShare.DATA[row].BFSHRCONT_BALAMT = bfshrcont_balamt;
                    if (bfshrcont_balamt == 0 && bfperiod == 0)
                    {
                        dsDetailShare.DATA[row].PERIOD = 1;
                        dsDetailShare.DATA[row].PERIODCOUNT_FLAG = 1;
                        dsDetailShare.DATA[row].ITEM_PAYAMT = dsDetailShare.DATA[row].BFPERIOD_PAYMENT;
                    }
                    dsDetailShare.DATA[row].ITEM_BALANCE = item_balance;
                    calItemPay();
                }
                else if (operate_flag == 0)
                {
                    dsDetailShare.DATA[row].PERIOD = bfperiod;
                    dsDetailShare.DATA[row].PERIODCOUNT_FLAG = 0;
                    dsDetailShare.DATA[row].PRINCIPAL_PAYAMT = 0;
                    dsDetailShare.DATA[row].INTEREST_PAYAMT = 0;
                    dsDetailShare.DATA[row].ITEM_PAYAMT = 0;
                    calItemPay();
                }
            }
            else if (eventArg == PostOperateFlagL)
            {
                int rowl = dsDetailLoan.GetRowFocus();
                decimal operate_flag_l = dsDetailLoan.DATA[rowl].OPERATE_FLAG;
                decimal bfshrcont_balamt_l = dsDetailLoan.DATA[rowl].BFSHRCONT_BALAMT;
                decimal principal_payamt = dsDetailLoan.DATA[rowl].PRINCIPAL_PAYAMT;
                decimal rkeep_principal = dsDetailLoan.DATA[rowl].RKEEP_PRINCIPAL;


                if (operate_flag_l == 1)
                {
                    //เช็คว่ามีเรียกเก็บอยู่หรือไม่
                    if (dsDetailLoan.DATA[rowl].BFPXAFTERMTHKEEP_TYPE == 1 && dsDetailLoan.DATA[rowl].BFLASTPROC_DATE >= dsMain.DATA[0].OPERATE_DATE)
                    {
                        principal_payamt = bfshrcont_balamt_l - rkeep_principal;
                    }
                    else
                    {
                        principal_payamt = bfshrcont_balamt_l;
                    }

                    //by_max
                    decimal ldc_clearint = 0, ldc_prncalint = dsDetailLoan.DATA[rowl].ITEM_PAYAMT;
                    DateTime ldtm_calfrom = dsDetailLoan.DATA[rowl].BFLASTCALINT_DATE, ldtm_calto = state.SsWorkDate;

                    string ls_contno = dsDetailLoan.DATA[rowl].LOANCONTRACT_NO;
                    ldc_clearint = wcf.NShrlon.of_computeinterest_single(state.SsWsPass, state.SsCoopControl, ls_contno, ldc_prncalint, ldtm_calfrom, ldtm_calto);

                    dsDetailLoan.DATA[rowl].PRINCIPAL_PAYAMT = principal_payamt;
                    dsDetailLoan.DATA[rowl].INTEREST_PAYAMT = dsDetailLoan.DATA[rowl].CP_INTERESTPAY; //นำ comment ออกก่อน เนื่องจากกรณีที่ สัญญาที่ไม่เรียกเก็บกรณีมาชำระพิเศษแล้วดอกเบี้ยที่ต้องชำระไม่ set ค่าให้ by.cherry 
                    dsDetailLoan.DATA[rowl].ITEM_PAYAMT = dsDetailLoan.DATA[rowl].PRINCIPAL_PAYAMT + dsDetailLoan.DATA[rowl].INTEREST_PAYAMT;
                    dsDetailLoan.DATA[rowl].ITEM_BALANCE = bfshrcont_balamt_l - dsDetailLoan.DATA[rowl].PRINCIPAL_PAYAMT;

                }
                else
                {

                    dsDetailLoan.DATA[rowl].PRINCIPAL_PAYAMT = 0;
                    dsDetailLoan.DATA[rowl].INTEREST_PAYAMT = 0;
                    dsDetailLoan.DATA[rowl].ITEM_PAYAMT = 0;
                    dsDetailLoan.DATA[rowl].ITEM_BALANCE = bfshrcont_balamt_l;

                }

                if (dsDetailLoan.DATA[rowl].BFPAYSPEC_METHOD == 2)
                {
                    ReCalint();
                }
                calItemPay();
            }
            else if (eventArg == PostOperateFlagE)
            {
                int rowe = dsDetailEtc.GetRowFocus();
                decimal operate_flag_e = dsDetailEtc.DATA[rowe].OPERATE_FLAG;
                decimal ldc_bfshrcontbal = dsDetailEtc.DATA[rowe].BFSHRCONT_BALAMT;

                if (operate_flag_e == 1)
                {

                    dsDetailEtc.DATA[rowe].ITEM_PAYAMT = ldc_bfshrcontbal;

                    calItemPay();
                }
                else if (operate_flag_e == 0)
                {
                    dsDetailEtc.DATA[rowe].ITEM_PAYAMT = 0;
                    calItemPay();
                }
            }
            else if (eventArg == PostOperateFlagA)
            {
                int rowe = dsDetailAdjust.GetRowFocus();
                decimal operate_flag_a = dsDetailAdjust.DATA[rowe].OPERATE_FLAG;
                if (operate_flag_a == 1)
                {
                    //by_max
                    String mType = dsMain.DATA[0].MONEYTYPE_CODE;
                    dsDetailAdjust.DATA[rowe].SUM_SLIP_AMT = dsDetailAdjust.DATA[rowe].SLIP_AMT;
                    dsMain.DATA[0].SLIPTYPE_CODE = "PX";
                    string member_no = dsMain.DATA[0].MEMBER_NO;
                    string mem_no = WebUtil.MemberNoFormat(member_no);
                    HdCoopControl.Value = state.SsCoopControl;
                    str_slippayin slip = new str_slippayin();
                    slip.member_no = mem_no;
                    slip.slip_date = dsMain.DATA[0].SLIP_DATE;
                    slip.operate_date = dsMain.DATA[0].OPERATE_DATE;
                    slip.sliptype_code = dsMain.DATA[0].SLIPTYPE_CODE;
                    slip.memcoop_id = state.SsCoopControl;
                    wcf.NShrlon.of_initslippayin(state.SsWsPass, ref slip);
                    dsMain.ImportData(slip.xml_sliphead);
                    dsMain.DATA[0].ENTRY_ID = state.SsUsername;
                    if (mType == "")
                    {
                        if (state.SsUsername == "pyotcl05") {
                            dsMain.DATA[0].MONEYTYPE_CODE = "CBT";
                        }
                        else
                        {
                            dsMain.DATA[0].MONEYTYPE_CODE = "CSH";
                        }
                    }
                    else {
                        dsMain.DATA[0].MONEYTYPE_CODE = mType;
                    }
                    dsMain.DATA[0].SLIPTYPE_CODE = dsMain.DATA[0].SLIPTYPE_CODE.Trim();
                    string sliptype_code = dsMain.DATA[0].SLIPTYPE_CODE;
                    string moneytype_code = dsMain.DATA[0].MONEYTYPE_CODE;
                    dsMain.DdSliptype();
                    dsMain.DdFromAccId(sliptype_code, moneytype_code);
                    dsMain.DdMoneyType();
                    try
                    {
                        slip.member_no = mem_no;
                        slip.slip_date = dsMain.DATA[0].SLIP_DATE;
                        slip.operate_date = dsMain.DATA[0].OPERATE_DATE;
                        slip.sliptype_code = dsMain.DATA[0].SLIPTYPE_CODE;
                        slip.memcoop_id = state.SsCoopControl;
                        dsDetailShare.ImportData(slip.xml_slipshr);
                    }
                    catch { dsDetailShare.ResetRow(); }
                    try
                    {
                        slip.member_no = mem_no;
                        slip.slip_date = dsMain.DATA[0].SLIP_DATE;
                        slip.operate_date = dsMain.DATA[0].OPERATE_DATE;
                        slip.sliptype_code = dsMain.DATA[0].SLIPTYPE_CODE;
                        slip.memcoop_id = state.SsCoopControl;
                        dsDetailLoan.ImportData(slip.xml_sliplon);
                        ///เพิ่มส่วนการคำนวณดอกเบี้ยที่ต้องชำระ pxaftermthkeep
                        AbsIntpay();
                    }
                    catch { dsDetailLoan.ResetRow(); }
                    try
                    {
                        slip.member_no = mem_no;
                        slip.slip_date = dsMain.DATA[0].SLIP_DATE;
                        slip.operate_date = dsMain.DATA[0].OPERATE_DATE;
                        slip.sliptype_code = dsMain.DATA[0].SLIPTYPE_CODE;
                        slip.memcoop_id = state.SsCoopControl;
                        dsDetailEtc.ImportData(slip.xml_slipetc);
                        dsDetailEtc.DdLoanType();
                    }
                    catch { dsDetailEtc.ResetRow(); }
                    SetDefaultTofromaccid();

                    //int rowe = dsDetailAdjust.GetRowFocus();
                    decimal etc = 0;
                    dsMain.DATA[0].REF_SLIPNO = dsDetailAdjust.DATA[rowe].ADJSLIP_NO.Trim();
                    dsMain.DATA[0].REF_SYSTEM = "KEEP";
                    HdAdjust.Value = dsDetailAdjust.DATA[rowe].ADJSLIP_NO.Trim();
                    dsMain.DATA[0].REF_SLIPAMT = dsDetailAdjust.DATA[rowe].SUM_SLIP_AMT;

                    string sql = "select slipitemtype_code,shrlontype_code,loancontract_no,item_adjamt,principal_adjamt,interest_adjamt from slslipadjustclass where trim(adjslip_no) = '" + dsDetailAdjust.DATA[rowe].ADJSLIP_NO.Trim() + "' and item_adjamt > 0";
                    Sdt dt = WebUtil.QuerySdt(sql);
                    while (dt.Next())
                    {
                        if (dt.GetString("slipitemtype_code") == "SHR")
                        {
                            Page.ClientScript.RegisterStartupScript(GetType(), "shr", "$('#ctl00_ContentPlace_dsDetailShare_chkdsDetailShare').attr('checked', true);", true);
                            dsDetailShare.DATA[0].OPERATE_FLAG = 1;
                            dsDetailShare.DATA[0].PERIODCOUNT_FLAG = 1;
                            dsDetailShare.DATA[0].PERIOD = dsDetailShare.DATA[0].PERIOD + 1;
                            dsDetailShare.DATA[0].ITEM_PAYAMT = dt.GetDecimal("item_adjamt");
                            dsDetailShare.DATA[0].PRINCIPAL_ADJAMT = dt.GetDecimal("principal_adjamt");
                            dsDetailShare.DATA[0].INTEREST_ADJAMT = 0; 
                        }
                        else if (dt.GetString("slipitemtype_code") == "LON")
                        {
                            string seq_ln = "select *  from lncontmaster where loancontract_no  = '" + dt.GetString("loancontract_no") + "' and contract_status > 0 and member_no = '" + mem_no + "'";
                            Sdt dt_ = WebUtil.QuerySdt(seq_ln);
                            if (dt_.Rows.Count == 0)
                            {
                                LtServerMessage.Text = WebUtil.ErrorMessage(dt.GetString("loancontract_no") + " ไม่พบสัญญาดังกล่าว ");
                                dsDetailAdjust.ResetRow();
                                refresh();
                                return;
                            }
                            Page.ClientScript.RegisterStartupScript(GetType(), "lon", "$('#ctl00_ContentPlace_dsDetailLoan_chkdsDetailLoan').attr('checked', true);", true);
                            for (int i = 0; i < dsDetailLoan.RowCount; i++)
                            {
                                if (dsDetailLoan.DATA[i].LOANCONTRACT_NO == dt.GetString("loancontract_no"))
                                {
                                    dsDetailLoan.DATA[i].OPERATE_FLAG = 1;
                                    int rowl = i;
                                    decimal operate_flag_l = dsDetailLoan.DATA[rowl].OPERATE_FLAG;
                                    decimal bfshrcont_balamt_l = dsDetailLoan.DATA[rowl].BFSHRCONT_BALAMT;
                                    decimal principal_payamt = dsDetailLoan.DATA[rowl].PRINCIPAL_PAYAMT;
                                    decimal rkeep_principal = dsDetailLoan.DATA[rowl].RKEEP_PRINCIPAL;


                                    if (operate_flag_l == 1)
                                    {
                                        //เช็คว่ามีเรียกเก็บอยู่หรือไม่
                                        if (dsDetailLoan.DATA[rowl].BFPXAFTERMTHKEEP_TYPE == 1 && dsDetailLoan.DATA[rowl].BFLASTPROC_DATE >= dsMain.DATA[0].OPERATE_DATE)
                                        {
                                            principal_payamt = bfshrcont_balamt_l - rkeep_principal;
                                        }
                                        else
                                        {
                                            principal_payamt = bfshrcont_balamt_l;
                                        }

                                        //by_max
                                        decimal ldc_clearint = 0, ldc_prncalint = dsDetailLoan.DATA[rowl].ITEM_PAYAMT;
                                        DateTime ldtm_calfrom = dsDetailLoan.DATA[rowl].BFLASTCALINT_DATE, ldtm_calto = state.SsWorkDate;

                                        string ls_contno = dsDetailLoan.DATA[rowl].LOANCONTRACT_NO;
                                        ldc_clearint = wcf.NShrlon.of_computeinterest_single(state.SsWsPass, state.SsCoopControl, ls_contno, ldc_prncalint, ldtm_calfrom, ldtm_calto);

                                        dsDetailLoan.DATA[rowl].PRINCIPAL_PAYAMT = principal_payamt;
                                        dsDetailLoan.DATA[rowl].INTEREST_PAYAMT = dsDetailLoan.DATA[rowl].CP_INTERESTPAY; //นำ comment ออกก่อน เนื่องจากกรณีที่ สัญญาที่ไม่เรียกเก็บกรณีมาชำระพิเศษแล้วดอกเบี้ยที่ต้องชำระไม่ set ค่าให้ by.cherry 
                                        dsDetailLoan.DATA[rowl].ITEM_PAYAMT = dsDetailLoan.DATA[rowl].PRINCIPAL_PAYAMT + dsDetailLoan.DATA[rowl].INTEREST_PAYAMT;
                                        dsDetailLoan.DATA[rowl].ITEM_BALANCE = bfshrcont_balamt_l - dsDetailLoan.DATA[rowl].PRINCIPAL_PAYAMT;

                                    }
                                    else
                                    {

                                        dsDetailLoan.DATA[rowl].PRINCIPAL_PAYAMT = 0;
                                        dsDetailLoan.DATA[rowl].INTEREST_PAYAMT = 0;
                                        dsDetailLoan.DATA[rowl].ITEM_PAYAMT = 0;
                                        dsDetailLoan.DATA[rowl].ITEM_BALANCE = bfshrcont_balamt_l;

                                    }

                                    if (dsDetailLoan.DATA[rowl].BFPAYSPEC_METHOD == 2)
                                    {
                                        ReCalint();
                                    }

                                    dsDetailLoan.DATA[i].PERIODCOUNT_FLAG = 1;
                                    dsDetailLoan.DATA[i].PERIOD = dsDetailLoan.DATA[i].PERIOD + 1;
                                    dsDetailLoan.DATA[i].ITEM_PAYAMT = dt.GetDecimal("item_adjamt");
                                    dsDetailLoan.DATA[i].PRINCIPAL_ADJAMT = dt.GetDecimal("principal_adjamt");
                                    dsDetailLoan.DATA[i].INTEREST_ADJAMT = dt.GetDecimal("interest_adjamt");

                                    decimal bfpayspec_method = dsDetailLoan.DATA[i].BFPAYSPEC_METHOD;
                                    decimal rkeep_int = dsDetailLoan.DATA[i].RKEEP_INTEREST;
                                    decimal rkeep_prin = dsDetailLoan.DATA[i].RKEEP_PRINCIPAL;
                                    decimal aftermont = dsDetailLoan.DATA[i].BFPXAFTERMTHKEEP_TYPE;
                                    if (!((rkeep_int > 0 || rkeep_prin > 0) && aftermont == 1) && bfpayspec_method == 2)
                                    {
                                        //PostRePayspecMethod();
                                        int row = i;
                                        DateTime calintfrom = dsDetailLoan.DATA[row].BFLASTCALINT_DATE;
                                        string contno = dsDetailLoan.DATA[row].LOANCONTRACT_NO;
                                        decimal item_payamt = dsDetailLoan.DATA[row].ITEM_PAYAMT;
                                        DateTime calintto = dsMain.DATA[0].OPERATE_DATE;
                                        decimal prnbal_payamt = 0;
                                        decimal interest_payamt = 0;
                                        decimal intarrear = 0;
                                        try
                                        {
                                            Int16 xml_re = wcf.NShrlon.of_calrevertpaymeth2(state.SsWsPass, contno, calintfrom, calintto, item_payamt, ref prnbal_payamt, ref interest_payamt);
                                            if (xml_re == 1)
                                            {
                                                dsDetailLoan.DATA[row].ITEM_PAYAMT = item_payamt;
                                                intarrear = dsDetailLoan.DATA[row].BFINTARR_AMT;
                                                decimal sum = prnbal_payamt + interest_payamt;
                                                decimal total = sum - item_payamt;
                                                prnbal_payamt = prnbal_payamt - total;
                                                dsDetailLoan.DATA[row].PRINCIPAL_PAYAMT = prnbal_payamt - intarrear;
                                                dsDetailLoan.DATA[row].INTEREST_PAYAMT = interest_payamt + intarrear;
                                                dsDetailLoan.DATA[row].INTEREST_PERIOD = interest_payamt;
                                                dsDetailLoan.DATA[row].ITEM_BALANCE = dsDetailLoan.DATA[row].BFSHRCONT_BALAMT - dsDetailLoan.DATA[row].PRINCIPAL_PAYAMT;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                                        }
                                    }
                                    else if (bfpayspec_method == 2 && (rkeep_int == 0 || rkeep_prin == 0))
                                    {
                                        //PostRePayspecMethod();
                                        int row = i;
                                        DateTime calintfrom = dsDetailLoan.DATA[row].BFLASTCALINT_DATE;
                                        string contno = dsDetailLoan.DATA[row].LOANCONTRACT_NO;
                                        decimal item_payamt = dsDetailLoan.DATA[row].ITEM_PAYAMT;
                                        DateTime calintto = dsMain.DATA[0].OPERATE_DATE;
                                        decimal prnbal_payamt = 0;
                                        decimal interest_payamt = 0;
                                        decimal intarrear = 0;
                                        try
                                        {
                                            Int16 xml_re = wcf.NShrlon.of_calrevertpaymeth2(state.SsWsPass, contno, calintfrom, calintto, item_payamt, ref prnbal_payamt, ref interest_payamt);
                                            if (xml_re == 1)
                                            {
                                                dsDetailLoan.DATA[row].ITEM_PAYAMT = item_payamt;
                                                intarrear = dsDetailLoan.DATA[row].BFINTARR_AMT;
                                                decimal sum = prnbal_payamt + interest_payamt;
                                                decimal total = sum - item_payamt;
                                                prnbal_payamt = prnbal_payamt - total;
                                                dsDetailLoan.DATA[row].PRINCIPAL_PAYAMT = prnbal_payamt - intarrear;
                                                dsDetailLoan.DATA[row].INTEREST_PAYAMT = interest_payamt + intarrear;
                                                dsDetailLoan.DATA[row].INTEREST_PERIOD = interest_payamt;
                                                dsDetailLoan.DATA[row].ITEM_BALANCE = dsDetailLoan.DATA[row].BFSHRCONT_BALAMT - dsDetailLoan.DATA[row].PRINCIPAL_PAYAMT;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                                        }
                                    }
                                    else
                                    {
                                        decimal item_payamt = dsDetailLoan.DATA[i].ITEM_PAYAMT;
                                        decimal interest_payamt = dsDetailLoan.DATA[i].INTEREST_PAYAMT;
                                        decimal bfshrcont_balamt = dsDetailLoan.DATA[i].BFSHRCONT_BALAMT;
                                        decimal cp_interestpay = dsDetailLoan.DATA[i].CP_INTERESTPAY;
                                        decimal principalPayamt = item_payamt - interest_payamt;
                                        decimal total_itembal = 0;
                                        if (item_payamt <= interest_payamt)
                                        {
                                            dsDetailLoan.DATA[i].INTEREST_PAYAMT = item_payamt;
                                            dsDetailLoan.DATA[i].PRINCIPAL_PAYAMT = 0;
                                            total_itembal = bfshrcont_balamt;
                                        }
                                        else
                                        {
                                            if (principalPayamt > bfshrcont_balamt)
                                            {
                                                dsDetailLoan.DATA[i].PRINCIPAL_PAYAMT = principal_payamt;
                                                dsDetailLoan.DATA[i].INTEREST_PAYAMT = interest_payamt;
                                                dsDetailLoan.DATA[i].ITEM_PAYAMT = principal_payamt + interest_payamt;
                                            }
                                            else
                                            {
                                                dsDetailLoan.DATA[i].PRINCIPAL_PAYAMT = principalPayamt;
                                            }
                                            total_itembal = bfshrcont_balamt - principalPayamt;
                                        }
                                        dsDetailLoan.DATA[i].ITEM_BALANCE = total_itembal;
                                    }
                                    break;
                                }
                            }
                        }
                        else
                        {
                            etc = etc + dt.GetDecimal("item_adjamt");
                        }
                    }
                    sql  = "select sum(item_adjamt) as item_adjamt from slslipadjustdet where slipitemtype_code not in ('LON','SHR','DEP') and  trim(adjslip_no) = '" + dsDetailAdjust.DATA[rowe].ADJSLIP_NO.Trim() + "'";
                    dt = WebUtil.QuerySdt(sql);
                    while (dt.Next())
                    {
                        if (dt.GetDecimal("item_adjamt") > 0) {
                            Page.ClientScript.RegisterStartupScript(GetType(), "etc", "$('#ctl00_ContentPlace_dsDetailEtc_chkDetailEtc').attr('checked', true);", true);
                            dsDetailEtc.InsertLastRow();
                            int currow = dsDetailEtc.RowCount - 1;
                            dsDetailEtc.DdLoanType();
                            dsDetailEtc.DATA[currow].SLIPITEMTYPE_CODE = "LPR";
                            dsDetailEtc.DATA[currow].SLIPITEM_DESC = "เงินรอจ่ายคืน";
                            dsDetailEtc.DATA[currow].OPERATE_FLAG = 1;
                            dsDetailEtc.DATA[currow].ITEM_PAYAMT = dt.GetDecimal("item_adjamt");
                        }
                    }
                    calItemPay();
                } else {
                    this.InitLnRcv();
                }
            }
            else if (eventArg == PostSlipItem)
            {
                int row = dsDetailEtc.GetRowFocus();
                string slipitemtype_code = dsDetailEtc.DATA[row].SLIPITEMTYPE_CODE;
                //dsAdd.ItemType(slipitemtype_code);
                string sql = @" 
                 SELECT SLUCFSLIPITEMTYPE.SLIPITEMTYPE_CODE,   SLUCFSLIPITEMTYPE.SLIPITEMTYPE_DESC
                 FROM SLUCFSLIPITEMTYPE  
                 WHERE SLUCFSLIPITEMTYPE.SLIPITEMTYPE_CODE = {0}";
                sql = WebUtil.SQLFormat(sql, slipitemtype_code);
                Sdt dt = WebUtil.QuerySdt(sql);
                if (dt.Next())
                {
                    dsDetailEtc.DATA[row].SLIPITEM_DESC = dt.GetString("SLIPITEMTYPE_DESC");
                }
            }
            else if (eventArg == PostDeleteRow)
            {
                int row = dsDetailEtc.GetRowFocus();
                dsDetailEtc.DeleteRow(row);
                dsDetailEtc.DdLoanType();
            }
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
                dsDetailShare.RetrieveDetailLoan(payinslip_no);
                dsDetailEtc.RetrieveDetailEtc(payinslip_no);

                string moneytype_code = dsMain.DATA[0].MONEYTYPE_CODE;
                string sliptype_code = dsMain.DATA[0].SLIPTYPE_CODE;

                dsMain.DdFromAccId(sliptype_code, moneytype_code);
                dsMain.DdMoneyType();
                SetDefaultTofromaccid();
            }
            else if (eventArg == PostSliptypecode)
            {
                string member_no = dsMain.DATA[0].MEMBER_NO;
                if (member_no != "")
                {
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
                    Int16 xml_re = wcf.NShrlon.of_calrevertpaymeth2(state.SsWsPass, contno, calintfrom, calintto, item_payamt, ref prnbal_payamt, ref interest_payamt);
                    if (xml_re == 1)
                    {
                        dsDetailLoan.DATA[row].ITEM_PAYAMT = item_payamt;
                        intarrear = dsDetailLoan.DATA[row].BFINTARR_AMT;
                        decimal sum = prnbal_payamt + interest_payamt;
                        decimal total = sum - item_payamt;
                        prnbal_payamt = prnbal_payamt - total;
                        if (prnbal_payamt - intarrear > 0)
                        {
                            dsDetailLoan.DATA[row].PRINCIPAL_PAYAMT = prnbal_payamt - intarrear;
                            dsDetailLoan.DATA[row].INTEREST_PAYAMT = interest_payamt + intarrear;
                        }
                        else {
                            dsDetailLoan.DATA[row].PRINCIPAL_PAYAMT = 0;
                            dsDetailLoan.DATA[row].INTEREST_PAYAMT = item_payamt;
                        }
                        //mike 
                        dsDetailLoan.DATA[row].INTEREST_PERIOD = interest_payamt;
                        calItemPay();
                        dsDetailLoan.DATA[row].ITEM_BALANCE = dsDetailLoan.DATA[row].BFSHRCONT_BALAMT - dsDetailLoan.DATA[row].PRINCIPAL_PAYAMT;
                    }
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            }
            else if (eventArg == PostChkLoanPayment)
            {
                string sql = "select loanpayment_type from lncontmaster where coop_id = {0} and loancontract_no = {1}";
                sql = WebUtil.SQLFormat(sql, state.SsCoopId, HdLoancontract.Value);
                Sdt dt = WebUtil.QuerySdt(sql);
                dt.Next();
                int r = Convert.ToInt32(HdRow.Value);
                decimal bfshrcont_balamt = dsDetailLoan.DATA[r].BFSHRCONT_BALAMT;
                decimal bfperiod_payment = dsDetailLoan.DATA[r].BFPERIOD_PAYMENT;
                decimal cp_interestpay = dsDetailLoan.DATA[r].CP_INTERESTPAY;
                decimal dec_sum = 0;

                if (dt.GetString("loanpayment_type") == "2")
                {//คงยอด
                    dsDetailLoan.DATA[r].PRINCIPAL_PAYAMT = bfperiod_payment - cp_interestpay;
                    dsDetailLoan.DATA[r].INTEREST_PAYAMT = cp_interestpay;
                    dsDetailLoan.DATA[r].ITEM_PAYAMT = bfperiod_payment;
                    dsDetailLoan.DATA[r].ITEM_BALANCE = bfshrcont_balamt - (bfperiod_payment - cp_interestpay);
                }
                else
                {//คงต้น
                    dsDetailLoan.DATA[r].PRINCIPAL_PAYAMT = bfperiod_payment;
                    dsDetailLoan.DATA[r].INTEREST_PAYAMT = cp_interestpay;
                    dsDetailLoan.DATA[r].ITEM_PAYAMT = bfperiod_payment + cp_interestpay;
                    dsDetailLoan.DATA[r].ITEM_BALANCE = bfshrcont_balamt - bfperiod_payment;
                }

                if (dsDetailShare.DATA[0].PERIODCOUNT_FLAG == 1)
                {
                    dec_sum = dec_sum + dsDetailShare.DATA[0].ITEM_PAYAMT;
                }

                for (var ii = 0; ii < dsDetailLoan.RowCount; ii++)
                {
                    if (dsDetailLoan.DATA[ii].OPERATE_FLAG == 1)
                    { //periodcount_flag
                        dec_sum = dec_sum + dsDetailLoan.DATA[ii].ITEM_PAYAMT;
                    }
                }
                if (dec_sum > 0)
                {
                    dsMain.DATA[0].SLIP_AMT = dec_sum;
                }
            }
            else if (eventArg == PostInsertRow) {
                dsDetailEtc.InsertLastRow();
                int currow = dsDetailEtc.RowCount - 1;
                try
                {
                    dsDetailEtc.DATA[currow].SEQ_NO = dsDetailEtc.GetMaxValueDecimal("SEQ_NO") + 1;
                }
                catch
                {
                    if (dsDetailEtc.RowCount < 1)
                    {
                        dsDetailEtc.DATA[currow].SEQ_NO = 1;
                    }
                }
                dsDetailEtc.DdLoanType();
            }
        }

        public void ReCalint()
        {
            DateTime operate_date = dsMain.DATA[0].OPERATE_DATE;
            string xml_sliplon = dsDetailLoan.ExportXml();
            string sliptype_code = dsMain.DATA[0].SLIPTYPE_CODE;
            try
            {
                Int16 xml_re = wcf.NShrlon.of_initslippayin_calint(state.SsWsPass, ref xml_sliplon, sliptype_code, operate_date);
                if (xml_re == 1)
                {
                    dsDetailLoan.ResetRow();
                    dsDetailLoan.ImportData(xml_sliplon);
                }
                AbsIntpay();
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                str_slippayin strslip = new str_slippayin();
                strslip.coop_id = state.SsCoopControl;
                strslip.entry_id = state.SsUsername;
                strslip.xml_sliphead = dsMain.ExportXml();
                strslip.xml_slipshr = dsDetailShare.ExportXml();
                strslip.xml_sliplon = dsDetailLoan.ExportXml();
                strslip.xml_slipetc = dsDetailEtc.ExportXml();

                idtm_lastDate = dsMain.DATA[0].SLIP_DATE;

                wcf.NShrlon.of_saveslip_payin(state.SsWsPass, ref strslip);
                string PayinslipNo = strslip.payinslip_no.Trim();
                string slip_adjust = HdAdjust.Value;
                if (slip_adjust != "")
                {
                    Sdt dt;
                    dt = WebUtil.QuerySdt("select * from slslippayindet where payinslip_no = '" + PayinslipNo + "'");
                    while (dt.Next()) {
                        string sql;
                        if (dt.GetString("slipitemtype_code") == "LON")
                        {
                            sql = @"update  slslipadjustclass set item_adjamt = {2},principal_adjamt = {3},interest_adjamt = {4}
                                where slipitemtype_code = 'LON'
                                and adjslip_no = {0}
                                and loancontract_no = {1}";
                            if (dt.GetDecimal("item_payamt") >= (Math.Round(dt.GetDecimal("principal_adjamt"), 2) + Math.Round(dt.GetDecimal("interest_adjamt"), 2)))
                            {
                                sql = WebUtil.SQLFormat(sql, slip_adjust, dt.GetString("loancontract_no"), 0, 0, 0);
                                WebUtil.QuerySdt(sql);
                            }
                            else {
                                decimal item_adjamt = (dt.GetDecimal("interest_adjamt") + dt.GetDecimal("principal_adjamt")) - dt.GetDecimal("item_payamt");
                                if (item_adjamt <= 0) {
                                    item_adjamt = 0;
                                }
                                decimal interest_adjamt = 0;
                                decimal principal_adjamt = 0;


                                decimal sum_cal = 0;
                                if (dt.GetDecimal("principal_payamt") == 0)
                                {
                                    interest_adjamt = dt.GetDecimal("interest_adjamt") - dt.GetDecimal("item_payamt");
                                    principal_adjamt = dt.GetDecimal("item_payamt") - (dt.GetDecimal("interest_adjamt") - dt.GetDecimal("item_payamt"));
                                }
                                else
                                {
                                    sum_cal = dt.GetDecimal("item_payamt") - dt.GetDecimal("interest_payamt");
                                    interest_adjamt = sum_cal;
                                    if (item_adjamt - sum_cal >= 0)
                                    {
                                        principal_adjamt = item_adjamt - sum_cal;
                                    }
                                }

                                if (item_adjamt < (principal_adjamt + interest_adjamt)) {
                                    principal_adjamt = item_adjamt;
                                    interest_adjamt = 0;
                                }

                                sql = WebUtil.SQLFormat(sql, slip_adjust, dt.GetString("loancontract_no"), item_adjamt, principal_adjamt, interest_adjamt);
                                WebUtil.QuerySdt(sql);
                            }
                        }
                        else if (dt.GetString("slipitemtype_code") == "SHR")
                        {
                            sql = @"update  slslipadjustclass set item_adjamt = {1},principal_adjamt = {1}
                                where slipitemtype_code = 'SHR'
                                and adjslip_no = {0}";
                            if ((dt.GetDecimal("item_payamt")) >= (dt.GetDecimal("principal_adjamt")))
                            {
                                sql = WebUtil.SQLFormat(sql, slip_adjust, 0);
                                WebUtil.QuerySdt(sql);
                            }
                            else {
                                sql = WebUtil.SQLFormat(sql, slip_adjust, dt.GetDecimal("principal_adjamt") - dt.GetDecimal("item_payamt"));
                                WebUtil.QuerySdt(sql);
                            }
                        }
                    }
                }
                Printing.RePrintSlipSlInIreportExat(this, PayinslipNo, state.SsCoopControl, "r_sl_slip_dairly_phy_slip");
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลสำเร็จ ");
                dsMain.ResetRow();
                dsDetailShare.ResetRow();
                dsDetailLoan.ResetRow();
                dsDetailEtc.ResetRow();
                dsDetailAdjust.ResetRow();
                refresh();
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void WebSheetLoadEnd()
        {
            if (dsMain.DATA[0].ACCID_FLAG == 1)
            {
                dsMain.FindDropDownList(0, dsMain.DATA.TOFROM_ACCIDColumn).Enabled = true;
            }
            else
            {
                dsMain.FindDropDownList(0, dsMain.DATA.TOFROM_ACCIDColumn).Enabled = false;
            }

            if (dsMain.DATA[0].MONEYTYPE_CODE == "CSH")
            {
                dsMain.FindDropDownList(0, dsMain.DATA.TOFROM_ACCIDColumn).Enabled = false;
            }
            else
            {
                dsMain.FindDropDownList(0, dsMain.DATA.TOFROM_ACCIDColumn).Enabled = true;
            }
        }

        public void refresh()
        {
            dsMain.DATA[0].SLIP_DATE = state.SsWorkDate;
            dsMain.DATA[0].OPERATE_DATE = state.SsWorkDate;
            of_activeworkdate();
            dsMain.DdSliptype();
            dsMain.DdTofromAccBlank();
            dsMain.DdMoneyType();
            dsDetailEtc.DdLoanType();
            dsMain.DATA[0].SLIPTYPE_CODE = "PX";
            dsMain.DATA[0].ENTRY_ID = state.SsUsername;
        }

        private void InitLnRcv()
        {
            try
            {
                string MONEYTYPE_CODE = dsMain.DATA[0].MONEYTYPE_CODE;
                string member_no = dsMain.DATA[0].MEMBER_NO;
                string mem_no = WebUtil.MemberNoFormat(member_no);
                HdCoopControl.Value = state.SsCoopControl;
                str_slippayin slip = new str_slippayin();
                slip.member_no = mem_no;
                slip.slip_date = dsMain.DATA[0].SLIP_DATE;
                slip.operate_date = dsMain.DATA[0].OPERATE_DATE;
                slip.sliptype_code = dsMain.DATA[0].SLIPTYPE_CODE;
                slip.memcoop_id = state.SsCoopControl;
                wcf.NShrlon.of_initslippayin(state.SsWsPass, ref slip);
                dsMain.ImportData(slip.xml_sliphead);
                dsMain.DATA[0].ENTRY_ID = state.SsUsername;
                if (MONEYTYPE_CODE == "")
                {
                    if (state.SsUsername == "pyotcl05")
                    {
                        dsMain.DATA[0].MONEYTYPE_CODE = "CBT";
                    }
                    else
                    {
                        dsMain.DATA[0].MONEYTYPE_CODE = "CSH";
                    }
                }
                else {
                    dsMain.DATA[0].MONEYTYPE_CODE = MONEYTYPE_CODE;
                }
                dsMain.DATA[0].SLIPTYPE_CODE = dsMain.DATA[0].SLIPTYPE_CODE.Trim();
                string sliptype_code = dsMain.DATA[0].SLIPTYPE_CODE;
                string moneytype_code = dsMain.DATA[0].MONEYTYPE_CODE;

                dsMain.DdSliptype();
                dsMain.DdFromAccId(sliptype_code, moneytype_code);
                dsMain.DdMoneyType();

                try
                {
                    slip.member_no = mem_no;
                    slip.slip_date = dsMain.DATA[0].SLIP_DATE;
                    slip.operate_date = dsMain.DATA[0].OPERATE_DATE;
                    slip.sliptype_code = dsMain.DATA[0].SLIPTYPE_CODE;
                    slip.memcoop_id = state.SsCoopControl;
                    dsDetailShare.ImportData(slip.xml_slipshr);
                }
                catch { dsDetailShare.ResetRow(); }
                try
                {
                    slip.member_no = mem_no;
                    slip.slip_date = dsMain.DATA[0].SLIP_DATE;
                    slip.operate_date = dsMain.DATA[0].OPERATE_DATE;
                    slip.sliptype_code = dsMain.DATA[0].SLIPTYPE_CODE;
                    slip.memcoop_id = state.SsCoopControl;
                    dsDetailLoan.ImportData(slip.xml_sliplon);
                    AbsIntpay();
                }
                catch { dsDetailLoan.ResetRow(); }
                try
                {
                    slip.member_no = mem_no;
                    slip.slip_date = dsMain.DATA[0].SLIP_DATE;
                    slip.operate_date = dsMain.DATA[0].OPERATE_DATE;
                    slip.sliptype_code = dsMain.DATA[0].SLIPTYPE_CODE;
                    slip.memcoop_id = state.SsCoopControl;
                    dsDetailEtc.ImportData(slip.xml_slipetc);
                    dsDetailEtc.DdLoanType();
                }
                catch { dsDetailEtc.ResetRow(); }

                dsDetailAdjust.RetrieveDetailAdjust(mem_no);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
            SetDefaultTofromaccid();
        }

        public void calItemPay()
        {
            int row = dsDetailShare.RowCount;
            int rowl = dsDetailLoan.RowCount;
            int rowe = dsDetailEtc.RowCount;
            decimal slip_amt = 0;
            for (int i = 0; i < row; i++)
            {
                decimal item_payamt = dsDetailShare.DATA[i].ITEM_PAYAMT;
                slip_amt += item_payamt;
            }
            for (int k = 0; k < rowl; k++)
            {
                decimal item_payamt = dsDetailLoan.DATA[k].ITEM_PAYAMT;
                slip_amt += item_payamt;
            }
            for (int j = 0; j < rowe; j++)
            {
                decimal item_payamt = dsDetailEtc.DATA[j].ITEM_PAYAMT;
                slip_amt += item_payamt;
            }
            dsMain.DATA[0].SLIP_AMT = slip_amt;

            for (int i = 0; i < dsDetailAdjust.RowCount; i++) {
                if (dsDetailAdjust.DATA[i].OPERATE_FLAG == 1) {
                    dsDetailAdjust.DATA[i].SUM_SLIP_AMT = slip_amt;
                }
            }
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
                dsMain.DATA[0].TOFROM_ACCID = dt.GetString("account_id");
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


        public string Get_NumberDOC(string typecode)
        {
            string coop_id = state.SsCoopControl;
            Sta ta = new Sta(state.SsConnectionString);
            string postNumber = "";
            try
            {
                ta.AddInParameter("AVC_COOPID", coop_id, System.Data.OracleClient.OracleType.VarChar);
                ta.AddInParameter("AVC_DOCCODE", typecode, System.Data.OracleClient.OracleType.VarChar);
                ta.AddReturnParameter("return_value", System.Data.OracleClient.OracleType.VarChar);
                ta.ExePlSql("N_PK_DOCCONTROL.OF_GETNEWDOCNO");
                postNumber = ta.OutParameter("return_value").ToString();
                ta.Close();
            }
            catch
            {
                ta.Close();
            }
            return postNumber.ToString();
        }

        public void AbsIntpay()
        {
            decimal intmonthArrear = 0, rkeepprin = 0, rkeepint = 0, nkeepint = 0, interestPeriod = 0, bfintarrAmt = 0, intreturnAmt = 0;
            int loanrow = dsDetailLoan.RowCount;
            DateTime lastprocess_date, ldtm_opedate;
            for (int r = 0; r < loanrow; r++)
            {
                intmonthArrear = dsDetailLoan.DATA[r].BFINTARRMTH_AMT;
                bfintarrAmt = dsDetailLoan.DATA[r].BFINTARR_AMT;
                rkeepprin = dsDetailLoan.DATA[r].RKEEP_PRINCIPAL;
                rkeepint = dsDetailLoan.DATA[r].RKEEP_INTEREST;
                nkeepint = dsDetailLoan.DATA[r].NKEEP_INTEREST;
                interestPeriod = dsDetailLoan.DATA[r].INTEREST_PERIOD;
                intreturnAmt = dsDetailLoan.DATA[r].BFINTRETURN_AMT;
                lastprocess_date = dsDetailLoan.DATA[r].BFLASTPROC_DATE;
                ldtm_opedate = dsMain.DATA[0].OPERATE_DATE;
                if (dsDetailLoan.DATA[r].BFPXAFTERMTHKEEP_TYPE == 1 && lastprocess_date >= ldtm_opedate)
                {
                    dsDetailLoan.DATA[r].CP_INTERESTPAY = interestPeriod + bfintarrAmt + intmonthArrear;
                }
                else
                {
                    dsDetailLoan.DATA[r].CP_INTERESTPAY = interestPeriod + bfintarrAmt + intmonthArrear;
                }
            }
        }
    }
}