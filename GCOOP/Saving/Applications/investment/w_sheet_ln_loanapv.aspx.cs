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
    public partial class w_sheet_ln_loanapv : PageWebSheet, WebSheet
    {
        protected String memNoItemChange;
        protected String appLoanMoney;
        protected String StatusItemChange;
        protected Sdt dt;
        DwThDate tDwMain;
        private InvestmentClient InvestmentService;
        public void InitJsPostBack()
        {
            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("loanapprove_date", "loanapprove_tdate");
            memNoItemChange = WebUtil.JsPostBack(this, "memNoItemChange");
            appLoanMoney = WebUtil.JsPostBack(this, "appLoanMoney");
            StatusItemChange = WebUtil.JsPostBack(this, "StatusItemChange");
        }
        public void WebSheetLoadBegin()
        {
            InvestmentService = wcf.Investment;
            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
            }
            else
            {
                this.RestoreContextDw(DwMain);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "memNoItemChange")
            {
                GetMemberDetail();
            }
            if (eventArg == "appLoanMoney")
            {
                CalDayLoan();
            
            }
            if (eventArg == "StatusItemChange")
            {
                SetDefaultApvAMT();
            }
        }

        public void SaveWebSheet()
        {
            String loancontract_no = " ";
            DwMain.SetItemString(1, "loancontract_no", loancontract_no);
            str_lcreqloan lcreqloan = new str_lcreqloan();

            lcreqloan.xml_lcreqloan = DwMain.Describe("DataWindow.Data.XML");
             lcreqloan.approve_id = state.SsUsername;
             lcreqloan.approve_bycoopid=state.SsCoopId;
             lcreqloan.approve_date = state.SsWorkDate;

            try
            {
                short result = InvestmentService.of_save_lcapvloan(state.SsWsPass, ref lcreqloan);


                if (result == 1)
                {
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกทำรายการสำเร็จ");
                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("บึนทึกทำรายการไม่สำเร็จ");
                }
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(" SaveWebSheet()>>>LoanService.of_save_lcapvloan  >>พบปัญหา :" + ex); }
        }

        public void WebSheetLoadEnd()
        {
            try
            {
                DwUtil.RetrieveDDDW(DwMain, "loanobjective_code", "loan.pbl", null);
            }
            catch { }
            try
            {
                DwUtil.RetrieveDDDW(DwMain, "loantype_code_1", "loan.pbl", null);
            }
            catch { }

            tDwMain.Eng2ThaiAllRow();
            DwMain.SaveDataCache();
        }

        private void GetMemberDetail()
        {
            str_lcreqloan lstr_lcreqloan = new str_lcreqloan();
            String ls_reqno = "";

            ls_reqno = Hfdoc_no.Value;

            try
            {
                lstr_lcreqloan.coop_id = state.SsCoopId;
                lstr_lcreqloan.loanrequest_docno = ls_reqno;
                short result = InvestmentService.of_init_lcapvloan(state.SsWsPass, ref lstr_lcreqloan);

                if (lstr_lcreqloan.xml_lcreqloan != null && lstr_lcreqloan.xml_lcreqloan != "")
                {
                    DwMain.Reset();
                    DwMain.ImportString(lstr_lcreqloan.xml_lcreqloan, Sybase.DataWindow.FileSaveAsType.Xml);
                }
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(" init พบปัญหา :" + ex); }
        }

        private void CalDayLoan()
        {
            Decimal loanapprove_amt = 0;
            Decimal loanrequest_amt = 0;
            Decimal period_installment = 0;
            Decimal period_payment = 0;
            String  LOANREQUEST_DOCNO = Hfdoc_no.Value;

            if (LOANREQUEST_DOCNO != "" && LOANREQUEST_DOCNO != null)
            {
                loanapprove_amt = DwMain.GetItemDecimal(1, "loanapprove_amt");// จำนวนเงินที่อนุมัติ
                loanrequest_amt = DwMain.GetItemDecimal(1, "loanrequest_amt");// จำนวนเงินที่ขอกู้ 
                period_installment = DwMain.GetItemDecimal(1, "period_installment");  // จำนวนงวด
                period_payment = DwMain.GetItemDecimal(1, "period_payment");  // จ่ายต่องวด
                try
                {
                    Decimal sumpay = (loanapprove_amt - (period_payment * (period_installment - 1)));
                    DwMain.SetItemDecimal(1, "period_lastpayment", sumpay);
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(" Method CalDayLoan(คำนวณชำระต่องวด) พบปัญหา :" + ex);
                }
            }
            else {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถคำนวณการชำระได้ เนื่องจากยังไม่เลือกเลขใบคำขอ");
                 }
        }

        private void SetDefaultApvAMT()
        {
            Decimal loanrequest_amt = 0;
            Int16 lnapvstatus = 0;

            lnapvstatus = Convert.ToInt16( DwMain.GetItemDecimal(1, "loanrequest_status"));
            
            if (lnapvstatus == 1)
            {
                loanrequest_amt = DwMain.GetItemDecimal(1, "loanrequest_amt");// จำนวนเงินที่ขอกู้ 
                
                try
                {
                    DwMain.SetItemDecimal(1, "loanapprove_amt", loanrequest_amt);
                    DwMain.SetItemDateTime(1, "loanapprove_date", state.SsWorkDate);
                    tDwMain.Eng2ThaiAllRow();

                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(" กำหนดค่ายอดเงินอนุมัติให้ไม่ได้ " + ex);
                }
            }
            
        }
    }
}