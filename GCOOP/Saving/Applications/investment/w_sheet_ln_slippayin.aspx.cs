using System;
using CoreSavingLibrary;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Sybase.DataWindow;
using DataLibrary;
using CoreSavingLibrary.WcfInvestment;

namespace Saving.Applications.investment
{
    public partial class w_sheet_ln_slippayin : PageWebSheet, WebSheet
    {
        #region Variable
        protected String jsPostMemberNo;
        protected String jsPostMoneytype;
        protected String jsPostFlag;
        protected String jsPostPayAmt;
        protected String jsPostEtcDel;
        protected String jsPostEtcChange;
        protected String jsChangeOperatedate;
        protected String jsPostInsertPayInExp;
        protected String jspostSetOperateDate;
        protected String jsPostLPMValue;
        private DwThDate tOperateDate;
        private DwThDate tChequeDate;
        private InvestmentClient InvestmentService;
        private String pbl = "loan_slippayin.pbl";
        #endregion

        #region Websheet Members
        public void InitJsPostBack()
        {
            tOperateDate = new DwThDate(DwPayIn, this);
            tOperateDate.Add("operate_date", "operate_tdate");
            tOperateDate.Add("slip_date", "slip_tdate");


            tChequeDate = new DwThDate(DwPayInExp, this);
            tChequeDate.Add("cheque_date", "cheque_tdate");
            jsPostMemberNo = WebUtil.JsPostBack(this, "jsPostMemberNo");
            jsPostMoneytype = WebUtil.JsPostBack(this, "jsPostMoneytype");
            jsPostFlag = WebUtil.JsPostBack(this, "jsPostFlag");
            jsPostPayAmt = WebUtil.JsPostBack(this, "jsPostPayAmt");
            jsPostEtcDel = WebUtil.JsPostBack(this, "jsPostEtcDel");
            jsPostEtcChange = WebUtil.JsPostBack(this, "jsPostEtcChange");
            jsChangeOperatedate = WebUtil.JsPostBack(this, "jsChangeOperatedate");
            jsPostInsertPayInExp = WebUtil.JsPostBack(this, "jsPostInsertPayInExp");
            jspostSetOperateDate = WebUtil.JsPostBack(this, "jspostSetOperateDate");
            jsPostLPMValue = WebUtil.JsPostBack(this, "jsPostLPMValue");
        }

        public void WebSheetLoadBegin()
        {
            InvestmentService = wcf.Investment;
            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                DwPayIn.InsertRow(0);
                DwPayInExp.InsertRow(0);
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwPayIn);
                this.RestoreContextDw(DwPayInExp, tChequeDate);
                this.RestoreContextDw(DwPayInLon);
                this.RestoreContextDw(DwPayInEtc);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "jsPostMemberNo":
                    checkSlipno();
                    break;
                case "jsPostMoneytype":
                    ResetSlExp();
                    break;
                case "jsPostFlag":
                    ChangeLoanFlag();
                    break;
                case "jsPostPayAmt":
                    JsPostPayAmt();
                    break;
                case "jsPostEtcDel":
                    EtcDelete();
                    break;
                case "jsPostEtcChange":
                    EtcChange();
                    break;
                case "jsChangeOperatedate":
                    ChangeOperatedate();
                    break;
                case "jsPostInsertPayInExp":
                    DwPayInExp.InsertRow(0);
                    break;
                case "jspostSetOperateDate":
                    JspostSetOperateDate();
                    break;
                case "jsPostLPMValue":
                    InitData(HiddenLPMValue.Value);
                    break;
            }
        }

        public void SaveWebSheet()
        {
            if (DwPayIn.GetItemDecimal(1, "slip_amt") >= 0)
            {
                str_lcslippayin slippayin = new str_lcslippayin();
                try
                {
                    slippayin.xml_sliphead = DwPayIn.Describe("Datawindow.Data.XML");
                    slippayin.xml_expense = DwPayInExp.Describe("Datawindow.Data.XML");
                    slippayin.xml_sliplon = DwPayInLon.Describe("Datawindow.Data.XML");
                    slippayin.xml_slipetc = DwPayInEtc.Describe("Datawindow.Data.XML");
                    slippayin.entry_id = state.SsUsername;
                    slippayin.entry_bycoopid = state.SsCoopId;
                }
                catch { }

                try
                {
                    short result = InvestmentService.of_saveord_payin(state.SsWsPass, ref slippayin);
                    string reqdoc_no = slippayin.payinslip_no;
                    if (result == 1)
                    {
                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลสำเร็จ");

                        Printing.LnCoopSlip(this, state.SsCoopControl, reqdoc_no, xmlconfig.LnReceivePrintMode);
                    }
                    else
                    {
                        
                    }
                }
                catch (Exception exerr) 
                { LtServerMessage.Text = WebUtil.ErrorMessage(exerr); 
                }
            }
            else
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ยอดทำรายการติดลบ กรุณาตรวจสอบ");
            }
        }

        public void WebSheetLoadEnd()
        {
            try
            {
                DwUtil.RetrieveDDDW(DwPayInExp, "moneytype_code", pbl, null);
            }
            catch { }
            try
            {
                DwUtil.RetrieveDDDW(DwPayInExp, "finbankacc_no", pbl, null);
            }
            catch { }

            tOperateDate.Eng2ThaiAllRow();
            tChequeDate.Eng2ThaiAllRow();
            DwMain.SaveDataCache();
            DwPayIn.SaveDataCache();
            DwPayInExp.SaveDataCache();
            DwPayInLon.SaveDataCache();
            DwPayInEtc.SaveDataCache();
        }
        #endregion

        #region Function
        private void checkSlipno()
        {
            String ls_sliptype = "";
            String ls_memno = "";
            
            ls_sliptype = DwPayIn.GetItemString(1, "sliptype_code");
            ls_memno = DwMain.GetItemString(1, "member_no");
            ls_memno = ls_memno.PadLeft(6,'0');

            DwMain.SetItemString(1, "member_no", ls_memno);

            Hdmemno.Value = ls_memno;

            if (ls_sliptype == "LPX")
            {
                InitData("");
            }
            else if (ls_sliptype == "LPM")
            {
                try
                {
                    str_lcnoticemthlist lclnrcvlist = new str_lcnoticemthlist();
                    lclnrcvlist.memcoop_id = state.SsCoopId;
                    lclnrcvlist.member_no = ls_memno;

                    short result = InvestmentService.of_getmembnoticemthlist(state.SsWsPass, ref lclnrcvlist);
                    if (result == 1)
                    {
                        InitData(lclnrcvlist.notice_docno);
                    }
                    else if (result > 1)
                    {
                        HdOpenIFrame.Value = "True";
                    }
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            }
        }

        private void InitData(string ref_slipno)
        {
            str_lcslippayin lstr_lcslippayin = new str_lcslippayin();

            String ls_memno = "";
            String ls_sliptype = "";

            try{ls_memno = DwMain.GetItemString(1, "member_no");}catch{ls_memno = "";}
            try{ls_sliptype = DwPayIn.GetItemString(1, "sliptype_code");}catch{ls_sliptype = "";}

            // ถ้าไม่ครบถ้วนยังไม่ต้อง Init
            if ( ls_memno == "" || ls_sliptype == "") {
                return;
            }

            try
            {
                lstr_lcslippayin.memcoop_id = state.SsCoopId;
                lstr_lcslippayin.member_no = ls_memno;
                lstr_lcslippayin.sliptype_code = ls_sliptype;
                lstr_lcslippayin.slip_date = state.SsWorkDate;
                lstr_lcslippayin.operate_date = state.SsWorkDate;
                lstr_lcslippayin.ref_slipno = ref_slipno;

                short result = InvestmentService.of_initpayin(state.SsWsPass, ref lstr_lcslippayin);
                DwMain.Reset();
                DwPayIn.Reset();
                DwPayInExp.Reset();
                DwPayInLon.Reset();
                DwPayInEtc.Reset();

                if (lstr_lcslippayin.xml_memdet != "" && lstr_lcslippayin.xml_memdet != null) {
                    DwMain.ImportString(lstr_lcslippayin.xml_memdet, Sybase.DataWindow.FileSaveAsType.Xml);
                }
                if (lstr_lcslippayin.xml_sliphead != "" && lstr_lcslippayin.xml_sliphead != null)
                {
                    DwPayIn.ImportString(lstr_lcslippayin.xml_sliphead, Sybase.DataWindow.FileSaveAsType.Xml);
                }
                if (lstr_lcslippayin.xml_sliplon != "" && lstr_lcslippayin.xml_sliplon != null)
                {
                    DwPayInLon.ImportString(lstr_lcslippayin.xml_sliplon, Sybase.DataWindow.FileSaveAsType.Xml);
                }
                if (lstr_lcslippayin.xml_slipetc != "" && lstr_lcslippayin.xml_slipetc != null)
                {
                    DwPayInEtc.ImportString(lstr_lcslippayin.xml_slipetc, Sybase.DataWindow.FileSaveAsType.Xml);
                }
                if (lstr_lcslippayin.xml_expense != "" && lstr_lcslippayin.xml_expense != null)
                {
                    DwPayInExp.ImportString(lstr_lcslippayin.xml_expense, Sybase.DataWindow.FileSaveAsType.Xml);
                }
                else
                {
                    DwPayInExp.InsertRow(0);
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        private void ResetSlExp()
        {
            DwPayInExp.SetItemNull(Convert.ToInt16(Hd_row.Value), "finbankacc_no");
            DwPayInExp.SetItemNull(Convert.ToInt16(Hd_row.Value), "cheque_bank");
            DwPayInExp.SetItemNull(Convert.ToInt16(Hd_row.Value), "cheque_branch");
            DwPayInExp.SetItemNull(Convert.ToInt16(Hd_row.Value), "bank_desc");
            DwPayInExp.SetItemNull(Convert.ToInt16(Hd_row.Value), "cheque_no");
            DwPayInExp.SetItemNull(Convert.ToInt16(Hd_row.Value), "cheque_date");
            DwPayInExp.SetItemNull(Convert.ToInt16(Hd_row.Value), "cheque_branch");
        }

        private void ChangeLoanFlag()
        {
            decimal flag = 0;
            Decimal slip_amt = 0, item_balance = 0, item_payamt = 0, bfshrcont_balamt = 0;
            Decimal interest_period = 0, bfintarr_amt = 0, interest_payamt = 0, bfperiod = 0;
            Decimal ldc_periodpay = 0, ldc_prnpay = 0, ldc_period = 0;
            String ls_sliptype;
            int r = Convert.ToInt32(Hd_row.Value);
            try
            {
                flag = DwPayInLon.GetItemDecimal(r, "operate_flag");
                slip_amt = DwPayIn.GetItemDecimal(1, "slip_amt");
                ls_sliptype = DwPayIn.GetItemString(1, "sliptype_code");
                item_balance = DwPayInLon.GetItemDecimal(r, "item_balance");
                item_payamt = DwPayInLon.GetItemDecimal(r, "item_payamt");
                bfshrcont_balamt = DwPayInLon.GetItemDecimal(r, "bfshrcont_balamt");
                interest_period = DwPayInLon.GetItemDecimal(r, "interest_period");
                bfintarr_amt = DwPayInLon.GetItemDecimal(r, "bfintarr_amt");
                bfperiod = DwPayInLon.GetItemDecimal(r, "bfperiod");
                ldc_periodpay = DwPayInLon.GetItemDecimal(r, "bfperiod_payment");
                interest_payamt = interest_period + bfintarr_amt;

                if (flag == 1)
                {
                    if (ls_sliptype == "LPX")
                    {
                        ldc_prnpay = bfshrcont_balamt;
                        ldc_period = bfperiod;

                    }
                    else
                    {
                        ldc_prnpay = ldc_periodpay;
                        ldc_period = bfperiod + 1;

                        if ( ldc_prnpay > bfshrcont_balamt )
                        {
                            ldc_prnpay = bfshrcont_balamt;
                        }
                    }

                    item_balance = bfshrcont_balamt - ldc_prnpay;

                    slip_amt = slip_amt + ldc_prnpay + interest_payamt;
                    DwPayInLon.SetItemDecimal(r, "item_balance", item_balance);
                    DwPayInLon.SetItemDecimal(r, "principal_payamt", ldc_prnpay);
                    DwPayInLon.SetItemDecimal(r, "interest_payamt", interest_payamt);
                    DwPayInLon.SetItemDecimal(r, "item_payamt", ldc_prnpay + interest_payamt);
                    DwPayInLon.SetItemDecimal(r, "period", ldc_period);
                }
                else
                {
                    slip_amt = slip_amt - item_payamt;
                    DwPayInLon.SetItemDecimal(r, "item_balance", bfshrcont_balamt);
                    DwPayInLon.SetItemDecimal(r, "item_payamt", 0);
                    DwPayInLon.SetItemDecimal(r, "principal_payamt", 0);
                    DwPayInLon.SetItemDecimal(r, "interest_payamt", 0);
                    DwPayInLon.SetItemDecimal(r, "period", bfperiod);
                }
                DwPayIn.SetItemDecimal(1, "slip_amt", slip_amt);

                if (DwPayInExp.RowCount == 1)
                {
                    DwPayInExp.SetItemDecimal(1, "expense_amt", DwPayIn.GetItemDecimal(1, "slip_amt"));
                }

            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
        }

        private void JsPostPayAmt()
        {
            Decimal slip_amt = 0, principal_payamt = 0, interest_payamt = 0, item_payamt = 0, item_payamt_ = 0, item_balance = 0, bfshrcont_balamt = 0;
            Decimal interest_period = 0, bfintarr_amt = 0, interest_payamt_all = 0;
            int r = Convert.ToInt32(Hd_row.Value);
            String ls_col = HdLnCol.Value;
            try
            {
                slip_amt = DwPayIn.GetItemDecimal(1, "slip_amt");
                principal_payamt = DwPayInLon.GetItemDecimal(r, "principal_payamt");// ต้นเงินแถวนั้นๆ
                interest_payamt = DwPayInLon.GetItemDecimal(r, "interest_payamt"); //ดอกเบี้ยแถวนั้นๆ
                item_payamt = DwPayInLon.GetItemDecimal(r, "item_payamt"); //ยอดชำระแถวนั้นๆ
                item_payamt_ = item_payamt;
                item_balance = DwPayInLon.GetItemDecimal(r, "item_balance");//คงเหลือของแถวนั้นๆ
                bfshrcont_balamt = DwPayInLon.GetItemDecimal(r, "bfshrcont_balamt");

                interest_period = DwPayInLon.GetItemDecimal(r, "interest_period");
                bfintarr_amt = DwPayInLon.GetItemDecimal(r, "bfintarr_amt");
                interest_payamt_all = interest_period + bfintarr_amt;
                if (ls_col == "item_payamt")
                {
                    if (item_payamt > interest_payamt_all && item_payamt <= (principal_payamt + interest_payamt_all))
                    {
                        interest_payamt = interest_payamt_all;
                        principal_payamt = item_payamt - interest_payamt_all;
                    }
                    else if (item_payamt <= interest_payamt_all)
                    {
                        interest_payamt = item_payamt;
                        principal_payamt = 0;
                    }
                    else if (item_payamt > interest_payamt_all && item_payamt > (principal_payamt + interest_payamt_all))
                    {
                        interest_payamt = item_payamt - principal_payamt;
                    }
                    DwPayInLon.SetItemDecimal(r, "interest_payamt", interest_payamt );
                    DwPayInLon.SetItemDecimal(r, "principal_payamt", principal_payamt );
                }

                item_payamt = principal_payamt + interest_payamt;
                item_balance = bfshrcont_balamt - principal_payamt;
                if (item_balance >= 0)
                {
                    DwPayInLon.SetItemDecimal(r, "item_balance", item_balance);
                    DwPayInLon.SetItemDecimal(r, "item_payamt", item_payamt);

                    slip_amt = slip_amt + (item_payamt - item_payamt_);
                    DwPayIn.SetItemDecimal(1, "slip_amt", slip_amt);

                    if (DwPayInExp.RowCount == 1)
                    {
                        DwPayInExp.SetItemDecimal(1, "expense_amt", DwPayIn.GetItemDecimal(1, "slip_amt"));
                    }
                }
                else
                {
                    DwPayInLon.SetItemDecimal(r, "principal_payamt", principal_payamt - (item_payamt - item_payamt_));
                }
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
        }

        private void EtcDelete()
        {
            int row = Convert.ToInt32(HdEtcRow.Value);
            Decimal item_payamt = 0, slip_amt = 0;
            try
            {
                slip_amt = DwPayIn.GetItemDecimal(1, "slip_amt");
                item_payamt = DwPayInEtc.GetItemDecimal(row, "item_payamt");

                slip_amt = slip_amt - item_payamt;
                DwPayIn.SetItemDecimal(1, "slip_amt", slip_amt);
                DwPayInEtc.DeleteRow(row);

                if (DwPayInExp.RowCount == 1)
                {
                    DwPayInExp.SetItemDecimal(1, "expense_amt", DwPayIn.GetItemDecimal(1, "slip_amt"));
                }
            }
            catch
            {
                DwPayInEtc.DeleteRow(row);
            }
        }

        private void EtcChange()
        {
            Decimal item_payamt = 0, slip_amt = 0;
            int row = Convert.ToInt32(HdEtcRow.Value);
            try
            {
                slip_amt = DwPayIn.GetItemDecimal(1, "slip_amt");
                item_payamt = DwPayInEtc.GetItemDecimal(row, "item_payamt");

                slip_amt = slip_amt + item_payamt;
                DwPayIn.SetItemDecimal(1, "slip_amt", slip_amt);

                if (DwPayInExp.RowCount == 1)
                {
                    DwPayInExp.SetItemDecimal(1, "expense_amt", DwPayIn.GetItemDecimal(1, "slip_amt"));
                }
            }
            catch
            {

            }
        }

        private void ChangeOperatedate()
        {
            String tDate2 = DwPayIn.GetItemString(1, "operate_tdate");
            DateTime dt2 = DateTime.ParseExact(tDate2, "ddMMyyyy", WebUtil.TH);
            DwPayIn.SetItemDateTime(1, "operate_date", dt2);
            DateTime slip_date = DwPayIn.GetItemDateTime(1, "operate_date");
            String xmlloan = DwPayInLon.Describe("Datawindow.Data.XML");

            try
            {
                InvestmentService.of_initpayin_calint(state.SsWsPass, ref xmlloan, slip_date);

                DwPayInLon.Reset();
                if (xmlloan != "" && xmlloan != null)
                {
                    DwPayInLon.ImportString(xmlloan, Sybase.DataWindow.FileSaveAsType.Xml);
                }

                Decimal slip_amt = 0;
                for (int i = 1; i <= DwPayInLon.RowCount; i++)
                {
                    if (DwPayInLon.GetItemDecimal(i, "operate_flag") == 1)
                    {
                        slip_amt += DwPayInLon.GetItemDecimal(i, "item_payamt");
                    }
                }

                for (int i = 1; i <= DwPayInEtc.RowCount; i++)
                {
                    slip_amt += DwPayInEtc.GetItemDecimal(i, "item_payamt");
                }

                DwPayIn.SetItemDecimal(1, "slip_amt", slip_amt);
            }
            catch( Exception ex_calint)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex_calint);
            }
        }

        private void JspostSetOperateDate()
        {
            try 
            {
                String tSlip_date = DwPayIn.GetItemString(1, "slip_tdate");
                DateTime operate_date = DateTime.ParseExact(tSlip_date, "ddMMyyyy", WebUtil.TH);
                DwPayIn.SetItemDateTime(1, "operate_date", operate_date);
                DwPayIn.SetItemDateTime(1, "slip_date", operate_date);
                tOperateDate.Eng2ThaiAllRow();

                ChangeOperatedate();

            }

            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message); }
        }
        #endregion
    }
}