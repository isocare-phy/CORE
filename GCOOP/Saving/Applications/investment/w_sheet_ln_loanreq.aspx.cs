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
using CoreSavingLibrary.WcfInvestment;
using Sybase.DataWindow;
using DataLibrary;

namespace Saving.Applications.investment
{
    public partial class w_sheet_ln_loanreq : PageWebSheet, WebSheet
    {
        private DwThDate tDwDetail;
        protected String jsPostMemberNo;
        protected String jsPostLoantypeCode;
        protected String jsPostDel;
        protected String jsPostInsert;
        protected String jsPostContIntType;
        protected String jsPostClearStatus;
        protected String jsPostRefColl;
        protected String jsPostCollAmt;
        protected String jsPostCalPayMent;
        protected String jsPostBack;
        protected String jsPostLoanRequest;
        protected String jsPostperiodinstallment;
        protected String jsPostPeriodPayment;
        protected String jsPostOpenlnreq;
        public String pbl = "ln_req_loan.pbl";

        public void InitJsPostBack()
        {
            jsPostMemberNo = WebUtil.JsPostBack(this, "jsPostMemberNo");
            jsPostLoantypeCode = WebUtil.JsPostBack(this, "jsPostLoantypeCode");
            //
            jsPostDel = WebUtil.JsPostBack(this, "jsPostDel");
            jsPostContIntType = WebUtil.JsPostBack(this, "jsPostContIntType");
            jsPostInsert = WebUtil.JsPostBack(this, "jsPostInsert");
            //DwClr
            jsPostClearStatus = WebUtil.JsPostBack(this, "jsPostClearStatus");
            jsPostRefColl = WebUtil.JsPostBack(this, "jsPostRefColl");
            jsPostCollAmt = WebUtil.JsPostBack(this, "jsPostCollAmt");
            jsPostCalPayMent = WebUtil.JsPostBack(this, "jsPostCalPayMent");

            jsPostBack = WebUtil.JsPostBack(this, "jsPostBack");

            jsPostLoanRequest = WebUtil.JsPostBack(this, "jsPostLoanRequest");
            jsPostperiodinstallment = WebUtil.JsPostBack(this, "jsPostperiodinstallment");
            jsPostPeriodPayment = WebUtil.JsPostBack(this, "jsPostPeriodPayment");
            jsPostOpenlnreq = WebUtil.JsPostBack(this, "jsPostOpenlnreq");

            tDwDetail = new DwThDate(DwDetail, this);
            tDwDetail.Add("loanrequest_date", "loanrequest_tdate");
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                DwDetail.InsertRow(0);

                DwMain.SetItemString(1, "loanobjective_code", "001");
                DwMain.SetItemDateTime(1, "loanrequest_date", state.SsWorkDate);

                DwIntSpc.Visible = false;
                Label7.Visible = false;
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwDetail);
                this.RestoreContextDw(DwColl);
                this.RestoreContextDw(DwClr);
                this.RestoreContextDw(DwClroth);
                if (HdType.Value == "3")
                {
                    this.RestoreContextDw(DwIntSpc);
                }
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "jsPostMemberNo":
                    JsPostMemberNo();
                    break;
                case "jsPostLoantypeCode":
                    DwDetail.SetItemString(1, "loantype_code", DwDetail.GetItemString(1, "loantype_code_1"));
                    JsPostLoantypeCode();
                    break;
                case "jsPostCollAmt":
                    JsPostCollAmt();
                    break;
                case "jsPostCalPayMent":
                    JsPostCalPayMent();
                    break;
                case "jsPostOpenlnreq":
                    OpenLoanReq();
                    break;
                case "jsPostRefColl":
                    JsPostRefCollNo();
                    break;
                case "jsPostDel":
                    JsPostDel();
                    break;
                case "jsPostContIntType":
                    JsPostContIntType();
                    break;
                case "jsPostClearStatus":
                    JsPostClearStatus();
                    break;
                case "jsPostInsert":
                    try
                    {
                        DwIntSpc.InsertRow(0);
                        int seq_no = GetMaxSeqNo();
                        int r = DwIntSpc.RowCount;
                        DwIntSpc.SetItemDecimal(r, "seq_no", Convert.ToDecimal(seq_no));
                    }
                    catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
                    break;
                case "jsPostLoanRequest":
                    JsPostLoanRequest();
                    break;
                case "jsPostperiodinstallment":
                    JsCalPeriodPayment();
                    break;
                case "jsPostPeriodPayment":
                    JsCalPeriodInstallment();
                    break;
            }
        }

        public void SaveWebSheet()
        {
            int statussave = 0;
            statussave = CheckSaveInfo();
            if (statussave == 1)
            {
                InvestmentClient InvestmentService = wcf.Investment;
                try
                {
                    str_lcreqloan str_lcreqloan = new str_lcreqloan();
                    str_lcreqloan.coop_id = state.SsCoopId;
                    str_lcreqloan.entry_id = state.SsUsername;
                    str_lcreqloan.loanrequest_date = state.SsWorkDate;
                    str_lcreqloan.loantype_code = DwDetail.GetItemString(1, "loantype_code_1");
                    str_lcreqloan.member_no = DwMain.GetItemString(1, "member_no");
                    str_lcreqloan.xml_lccontclr = DwClr.Describe("DataWindow.Data.XML");
                    str_lcreqloan.xml_lccontclroth = DwClroth.Describe("DataWindow.Data.XML");
                    str_lcreqloan.xml_lccontcoll = DwColl.Describe("DataWindow.Data.XML");
                    str_lcreqloan.xml_lcreqloan = DwDetail.Describe("DataWindow.Data.XML");

                    if (HdType.Value == "3")
                    {
                        str_lcreqloan.xml_lcintspc = DwIntSpc.Describe("DataWindow.Data.XML");
                    }
                    short result = InvestmentService.of_save_lcreqloan(state.SsWsPass, ref str_lcreqloan);
                    if (result == 1)
                    {
                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                    }
                    else
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("บันทึกไม่สำเร็จ");
                    }
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("บันทึกไม่สำเร็จเนื่องจาก :" + ex);
                }
            }
        }

        public void WebSheetLoadEnd()
        {
            try
            {
                DwUtil.RetrieveDDDW(DwDetail, "loantype_code_1", pbl, null);
            }
            catch { }
            try
            {
                DwUtil.RetrieveDDDW(DwDetail, "loanobjective_code", pbl, null);
            }
            catch { }
            try
            {
                DwUtil.RetrieveDDDW(DwColl, "loancolltype_code", pbl, null);
            }
            catch { }
            tDwDetail.Eng2ThaiAllRow();
            DwMain.SaveDataCache();
            DwDetail.SaveDataCache();
            DwColl.SaveDataCache();
            DwClr.SaveDataCache();
            DwClroth.SaveDataCache();
            if (HdType.Value == "3")
            {
                DwIntSpc.SaveDataCache();
            }

        }

        public void JsPostMemberNo()
        {
            try
            {
                string member_no = DwMain.GetItemString(1, "member_no");
                member_no = member_no.PadLeft(6,'0');
                DwMain.SetItemString(1, "member_no", member_no);

                DwUtil.RetrieveDataWindow(DwMain, pbl, null, state.SsCoopId, member_no);
                member_no = DwMain.GetItemString(1, "member_no");
                DwDetail.Modify("loantype_code_1.Protect=0");
                JsPostLoantypeCode();
            }
            catch
            {
                DwMain.InsertRow(0);
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูลรหัสสหกรณ์");
            }
        }

        public void JsPostLoantypeCode()
        {
            InvestmentClient InvestmentService = wcf.Investment;
            try
            {
                str_lcreqloan str_lcreqloan = new str_lcreqloan();

                str_lcreqloan.coop_id = state.SsCoopId;
                str_lcreqloan.memcoop_id = state.SsCoopId;
                str_lcreqloan.entry_id = state.SsUsername;
                str_lcreqloan.loanrequest_date = state.SsWorkDate;
                str_lcreqloan.loantype_code = DwDetail.GetItemString(1, "loantype_code_1");
                str_lcreqloan.member_no = DwMain.GetItemString(1, "member_no");

                short result = InvestmentService.of_init_lcreqloan(state.SsWsPass, ref str_lcreqloan);

                if (result == 1 && str_lcreqloan.xml_lccontclr != null && str_lcreqloan.xml_lccontclr != "")
                {
                    DwClr.Reset();
                    DwClr.ImportString(str_lcreqloan.xml_lccontclr, FileSaveAsType.Xml);
                }
                if (result == 1 && str_lcreqloan.xml_lccontclroth != null && str_lcreqloan.xml_lccontclroth != "")
                {
                    DwClroth.Reset();
                    DwClroth.ImportString(str_lcreqloan.xml_lccontclroth, FileSaveAsType.Xml);
                }
                if (result == 1 && str_lcreqloan.xml_lccontcoll != null && str_lcreqloan.xml_lccontcoll != "")
                {
                    DwColl.Reset();
                    DwColl.ImportString(str_lcreqloan.xml_lccontcoll, FileSaveAsType.Xml);
                }
                if (result == 1 && str_lcreqloan.xml_lcreqloan != null && str_lcreqloan.xml_lcreqloan != "")
                {
                    DwDetail.Reset();
                    DwDetail.ImportString(str_lcreqloan.xml_lcreqloan, FileSaveAsType.Xml);
                    try
                    {
                        Decimal int_continttype = DwDetail.GetItemDecimal(1, "int_continttype");
                        if (int_continttype == 3)
                        {
                            HdType.Value = "3";
                            Label7.Visible = true;
                            DwIntSpc.Visible = true;
                        }
                    }catch { }
                }
                if (result == 1 && str_lcreqloan.xml_lcintspc != null && str_lcreqloan.xml_lcintspc != "")
                {
                    HdType.Value = "3";
                    Label7.Visible = true;
                    DwIntSpc.Visible = true;
                    DwIntSpc.Reset();
                    DwIntSpc.ImportString(str_lcreqloan.xml_lcintspc, FileSaveAsType.Xml);
                }

            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
        }

        public int CheckSaveInfo()
        {
            int status = 1;
            String loanobjective_code = "", loancolltype_code = "", ref_collno = "";
            Decimal period_lastpayment = 0, period_payment = 0, period_installment = 0, loanrequest_amt = 0;
            try
            {
                try { loanrequest_amt = DwDetail.GetItemDecimal(1, "loanrequest_amt"); }
                catch { loanrequest_amt = 0; }
                try { period_installment = DwDetail.GetItemDecimal(1, "period_installment"); }
                catch { period_installment = 0; }
                try { period_payment = DwDetail.GetItemDecimal(1, "period_payment"); }
                catch { period_payment = 0; }
                try { period_lastpayment = DwDetail.GetItemDecimal(1, "period_lastpayment"); }
                catch { period_lastpayment = 0; }
                try { loanobjective_code = DwDetail.GetItemString(1, "loanobjective_code"); }
                catch { loanobjective_code = ""; }
                if (loanrequest_amt != ((period_installment-1) * period_payment)+period_lastpayment)
                {
                    status = 0;
                    LtServerMessage.Text = WebUtil.ErrorMessage("งวดที่ส่ง หรือ  ชำระ/งวด ไม่ถูกต้อง");
                }
                //else if (period_lastpayment != 0)
                //{
                //    status = 0;
                //    LtServerMessage.Text = WebUtil.ErrorMessage("งวดสุดท้ายชำระ ไม่ถูกต้อง");
                //}
                else if (loanobjective_code.Trim() == null || loanobjective_code.Trim() == "")
                {
                    status = 0;
                    LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาระบุ วัตถุประสงค์การกู้");
                }
                else if (DwColl.RowCount == 0)
                {
                    status = 0;
                    LtServerMessage.Text = WebUtil.ErrorMessage("ตรวจสอบหลักค้ำประกัน");
                }
                else
                {
                    try
                    {
                        for (int i = 1; i < DwColl.RowCount + 1; i++)
                        {
                            loancolltype_code = DwColl.GetItemString(i, "loancolltype_code");
                            ref_collno = DwColl.GetItemString(i, "ref_collno");
                        }
                    }
                    catch
                    {
                        status = 0;
                        LtServerMessage.Text = WebUtil.ErrorMessage("ตรวจสอบประเภทหลักประกัน หรือ เลขที่บัตรประชาชน/หลักทรัพย์");
                    }
                }
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); status = 0; }
            return status;
        }

        public void JsPostClearStatus()
        {
            int r = 0;
            decimal flag = 0;
            try
            {
                r = Convert.ToInt32(HdClrRow.Value);
                flag = DwClr.GetItemDecimal(r, "clear_status");
                if (flag == 1)
                {
                    DwClr.SetItemDecimal(r, "clear_amount", DwClr.GetItemDecimal(r, "principal_balance"));
                }
                else
                {
                    DwClr.SetItemDecimal(r, "clear_amount", 0);
                }
            }
            catch
            {

            }
        }

        public void JsPostCollAmt()
        {
            try
            {
                int r = Convert.ToInt32(HdCollRow.Value);
                Decimal Amt = Convert.ToDecimal(HdCollAmt.Value);
                String Desc = DwColl.GetItemString(r, "description");
                Desc = Desc + Amt.ToString("#,###") + " บาท";
                //if (DwColl.GetItemString(r, "loancolltype_code") == "04")
                //{
                //    Desc = Desc + " หุ้น";
                //}
                //else
                //{
                //    Desc = Desc + " บาท";
                //}
                DwColl.SetItemString(r, "description", Desc);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }

        }

        public void JsPostCalPayMent()
        {
            Decimal loanrequest_amt = 0, period_installment = 0, period_payment = 0, period_lastpayment = 0, sum1 = 0, sum2 = 0, sum3 = 0;
            try
            {
                loanrequest_amt = DwDetail.GetItemDecimal(1, "loanrequest_amt");
                period_installment = DwDetail.GetItemDecimal(1, "period_installment");
                period_payment = DwDetail.GetItemDecimal(1, "period_payment");

                sum1 = loanrequest_amt % period_installment;
                if (sum1 == 0 && period_payment == 0)
                {
                    sum1 = loanrequest_amt / period_installment;
                    DwDetail.SetItemDecimal(1, "period_payment", sum1);
                    DwDetail.SetItemDecimal(1, "period_lastpayment", sum1);
                }
                else if (period_payment == 0)
                {
                    sum1 = loanrequest_amt - (loanrequest_amt % period_installment);//ทั้งหมด ลบ เศษออก
                    sum1 = sum1 / period_installment;//แต่ละงวด
                    sum2 = sum1 + (loanrequest_amt % period_installment);//งวดสุดท้าย
                    DwDetail.SetItemDecimal(1, "period_payment", sum1);
                    DwDetail.SetItemDecimal(1, "period_lastpayment", sum2);
                }
                else if (period_payment != 0)
                {
                    sum3 = loanrequest_amt - ((period_installment - 1) * period_payment);
                    DwDetail.SetItemDecimal(1, "period_lastpayment", sum3);
                }

            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }
        
        public int GetMaxSeqNo()
        {
            int seq_no = 0;
            try
            {
                string loanrequest_docno = DwDetail.GetItemString(1, "loanrequest_docno");
                string sqlgetseqno = "select max(seq_no) as mseqno from LCREQLOANINTSPC where branch_id='" + state.SsCoopId + "' and LOANREQUEST_DOCNO='" + loanrequest_docno.Trim() + "'";
                Sdt seqNo = WebUtil.QuerySdt(sqlgetseqno);
                if (seqNo.Next())
                {
                    seq_no = Convert.ToInt32(seqNo.GetDecimal("mseqno"));
                }
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
            seq_no++;
            return seq_no;
        }

        public void JsPostDel()
        {
            try
            {
                int i = Convert.ToInt32(HdRow.Value);
                String loanrequest_docno = "";
                Decimal seq_no = 0;
                seq_no = DwIntSpc.GetItemDecimal(i, "seq_no");
                loanrequest_docno = DwDetail.GetItemString(i, "loanrequest_docno");

                string sqldel = "delete from LCREQLOANINTSPC where branch_id='" + state.SsCoopId + "' and loanrequest_docno='" + loanrequest_docno + "' and seq_no=" + seq_no + "";
                Sdt del = WebUtil.QuerySdt(sqldel);
                DwIntSpc.DeleteRow(i);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void JsPostContIntType()
        {
            decimal int_continttype = 0;
            try
            {
                int_continttype = DwDetail.GetItemDecimal(1, "int_continttype");
                if (int_continttype == 3)
                {
                    HdType.Value = "3";
                    Label7.Visible = true;
                    DwIntSpc.Visible = true;
                    string loanrequest_docno = DwDetail.GetItemString(1, "loanrequest_docno").Trim();
                    if (loanrequest_docno != "" && loanrequest_docno != null && loanrequest_docno != "AUTO")
                    {
                        DwUtil.RetrieveDataWindow(DwIntSpc, pbl, null, state.SsCoopId, loanrequest_docno);
                    }
                }
                else
                {
                    HdType.Value = "";
                    Label7.Visible = false;
                    DwIntSpc.Visible = false;
                }
            }
            catch { }
        }

        public void JsPostLoanRequest()
        {
            str_lccalperiod lstr_lccalperiod = new str_lccalperiod();

            InvestmentClient InvestmentService = wcf.Investment;

            try
            {
                lstr_lccalperiod.loantype_code = DwDetail.GetItemString(1,"loantype_code");
                lstr_lccalperiod.loanpayment_type = Convert.ToInt16(DwDetail.GetItemDecimal(1, "loanpayment_type"));
                lstr_lccalperiod.loanpayment_percent = DwDetail.GetItemDecimal(1, "loanpayment_percent");
                lstr_lccalperiod.loanpayment_fixamt = DwDetail.GetItemDecimal(1, "loanpayment_fixamt");
                lstr_lccalperiod.calperiod_maxinstallment = Convert.ToInt16(DwDetail.GetItemDecimal(1, "maxperiod_installment"));
                lstr_lccalperiod.calperiod_prnamt = DwDetail.GetItemDecimal(1, "loanrequest_amt");
                lstr_lccalperiod.calperiod_intrate = DwDetail.GetItemDecimal(1, "contsign_intrate");

                InvestmentService.of_initinstallment(state.SsWsPass, ref lstr_lccalperiod);

                DwDetail.SetItemDecimal(1,"period_installment",lstr_lccalperiod.period_installment);
                DwDetail.SetItemDecimal(1,"period_payment",lstr_lccalperiod.period_payment);
                DwDetail.SetItemDecimal(1,"period_lastpayment",lstr_lccalperiod.period_lastpayment);
            }
            catch(Exception e)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(e.Message);
            }
        }

        public void JsCalPeriodInstallment()
        {
            str_lccalperiod lstr_lccalperiod = new str_lccalperiod();

            InvestmentClient InvestmentService = wcf.Investment;

            try
            {
                lstr_lccalperiod.loantype_code = DwDetail.GetItemString(1, "loantype_code");
                lstr_lccalperiod.loanpayment_type = Convert.ToInt16(DwDetail.GetItemDecimal(1, "loanpayment_type"));
                lstr_lccalperiod.loanpayment_percent = DwDetail.GetItemDecimal(1, "loanpayment_percent");
                lstr_lccalperiod.loanpayment_fixamt = DwDetail.GetItemDecimal(1, "loanpayment_fixamt");
                lstr_lccalperiod.calperiod_maxinstallment = Convert.ToInt16(DwDetail.GetItemDecimal(1, "maxperiod_installment"));
                lstr_lccalperiod.calperiod_prnamt = DwDetail.GetItemDecimal(1, "loanrequest_amt");
                lstr_lccalperiod.calperiod_intrate = DwDetail.GetItemDecimal(1, "contsign_intrate");
                lstr_lccalperiod.period_payment = DwDetail.GetItemDecimal(1, "period_payment");

                InvestmentService.of_calinstallment(state.SsWsPass, ref lstr_lccalperiod);

                DwDetail.SetItemDecimal(1, "period_installment", lstr_lccalperiod.period_installment);
                DwDetail.SetItemDecimal(1, "period_payment", lstr_lccalperiod.period_payment);
                DwDetail.SetItemDecimal(1, "period_lastpayment", lstr_lccalperiod.period_lastpayment);
            }
            catch (Exception e)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(e.Message);
            }
        }

        public void JsCalPeriodPayment()
        {
            str_lccalperiod lstr_lccalperiod = new str_lccalperiod();

            InvestmentClient InvestmentService = wcf.Investment;

            try
            {
                lstr_lccalperiod.loantype_code = DwDetail.GetItemString(1, "loantype_code");
                lstr_lccalperiod.loanpayment_type = Convert.ToInt16(DwDetail.GetItemDecimal(1, "loanpayment_type"));
                lstr_lccalperiod.loanpayment_percent = DwDetail.GetItemDecimal(1, "loanpayment_percent");
                lstr_lccalperiod.loanpayment_fixamt = DwDetail.GetItemDecimal(1, "loanpayment_fixamt");
                lstr_lccalperiod.calperiod_maxinstallment = Convert.ToInt16(DwDetail.GetItemDecimal(1, "maxperiod_installment"));
                lstr_lccalperiod.calperiod_prnamt = DwDetail.GetItemDecimal(1, "loanrequest_amt");
                lstr_lccalperiod.calperiod_intrate = DwDetail.GetItemDecimal(1, "contsign_intrate");
                lstr_lccalperiod.period_installment = Convert.ToInt16(DwDetail.GetItemDecimal(1, "period_installment"));

                InvestmentService.of_calperiodpay(state.SsWsPass, ref lstr_lccalperiod);

                DwDetail.SetItemDecimal(1, "period_installment", lstr_lccalperiod.period_installment);
                DwDetail.SetItemDecimal(1, "period_payment", lstr_lccalperiod.period_payment);
                DwDetail.SetItemDecimal(1, "period_lastpayment", lstr_lccalperiod.period_lastpayment);
            }
            catch (Exception e)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(e.Message);
            }
        }

        public void OpenLoanReq()
        {
            InvestmentClient InvestmentService = wcf.Investment;
            String lnreq_docno = Hdlnreq_Docno.Value;

            str_lcreqloan astr_lcreqloan = new str_lcreqloan();
            astr_lcreqloan.coop_id = state.SsCoopId;
            astr_lcreqloan.loanrequest_docno = lnreq_docno;

            try
            {
                short result = InvestmentService.of_open_reqloan(state.SsWsPass, ref astr_lcreqloan);
                if (result == 1)
                {
                    DwDetail.Reset();
                    DwClr.Reset();
                    DwClroth.Reset();
                    DwColl.Reset();
                    DwIntSpc.Reset();
                    JsPostMemberNo();
                    if (astr_lcreqloan.xml_lcreqloan != "" && astr_lcreqloan.xml_lcreqloan != null)
                    {
                        DwDetail.ImportString(astr_lcreqloan.xml_lcreqloan, Sybase.DataWindow.FileSaveAsType.Xml);
                    }
                    if (astr_lcreqloan.xml_lccontclr != "" && astr_lcreqloan.xml_lccontclr != null)
                    {
                        DwClr.ImportString(astr_lcreqloan.xml_lccontclr, Sybase.DataWindow.FileSaveAsType.Xml);
                    }
                    if (astr_lcreqloan.xml_lccontclroth != "" && astr_lcreqloan.xml_lccontclroth != null)
                    {
                        DwClroth.ImportString(astr_lcreqloan.xml_lccontclroth, Sybase.DataWindow.FileSaveAsType.Xml);
                    }
                    if (astr_lcreqloan.xml_lccontcoll != "" && astr_lcreqloan.xml_lccontcoll != null)
                    {
                        DwColl.ImportString(astr_lcreqloan.xml_lccontcoll, Sybase.DataWindow.FileSaveAsType.Xml);
                    }
                    if (astr_lcreqloan.xml_lcintspc != "" && astr_lcreqloan.xml_lcintspc != null)
                    {
                        DwIntSpc.ImportString(astr_lcreqloan.xml_lcintspc, Sybase.DataWindow.FileSaveAsType.Xml);
                    }
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void JsPostRefCollNo()
        {
            int li_row = Convert.ToInt32(HdCollRow.Value);
            String ls_colltype = "";
            String ls_refcollno = "";

            try { ls_colltype = DwColl.GetItemString(li_row, "loancolltype_code"); } catch { ls_colltype = ""; };
            try { ls_refcollno = DwColl.GetItemString(li_row, "ref_collno"); } catch { ls_refcollno = ""; };

            if ( ls_colltype == "01" && ls_refcollno != "") {
                GetIDCard();
            }
        }


        public void GetIDCard() //ฟังก์ชันดึงข้อมูลรายละเอียดของเลขบัตรประชาชน กรณีที่ระบุเลขบัตรเอง
        {
            int row = Convert.ToInt32(HdCollRow.Value); //แถวที่ระบุเลขบัตรประชาชนเอง
            String IDCard = DwColl.GetItemString(row, "ref_collno"); //เลขบัตรประชาชนที่ระบุเอง

            if (!FindRowIDCard(row, IDCard)) //กรณีที่เลขบัตรประชาชนที่ระบุมา ไม่ซ้ำกับแถวอื่น
            {
                String sqlselectIDCard = "select person_name from lcmembpersonmaster where person_id = '" + IDCard + "'";
                Sdt PersonID = WebUtil.QuerySdt(sqlselectIDCard);
                if (PersonID.Next()) //กรณีที่พบรายละเอียดของเลขบัตรประชาชน ให้ดึงมาใส่ในช่องรายละเอียด
                {
                    DwColl.SetItemString(row, "description", PersonID.GetString("person_name"));
                }
                else
                {
                    DwColl.SetItemNull(row, "description");
                }
            }
            else //กรณีที่เลขบัตรประชาชนซ้ำ ให้ clear ข้อมูลในช่อง และแจ้งเตือน
            {
                DwColl.SetItemNull(row, "description");
                DwColl.SetItemNull(row, "ref_collno");
                LtServerMessage.Text = WebUtil.ErrorMessage("เลขบัตรประชาชน " + IDCard + " ถูกใช้แล้ว กรุณาตรวจสอบ");
            }
        }

        public bool FindRowIDCard(int row, String IDCard) //ฟังก์ชันป้องกันการซ้ำกันของเลขบัตรประชาชน
        {
            /*###### loancolltype_code = '01' หมายถึงบุคคลค้ำประกัน ######*/
            int find_row = 0;
            if (row != 1) //กรณีที่แถวที่ต้องการนำเลขบัตรประชาชนมาตรวจสอบไม่ใช่แถวแรก ให้ค้นหาช่วงหน้า
                find_row = DwColl.FindRow("ref_collno = '" + IDCard + "' and loancolltype_code = '01'", 1, row - 1);

            if (DwColl.RowCount != row && find_row == 0) //กรณีที่แถวที่ต้องการนำเลขบัตรประชาชนมาตรวจสอบไม่ใช่แถวสุดท้าย และยังไม่เจอเลขซ้ำ ให้ค้นหาช่วงหลัง
                find_row = DwColl.FindRow("ref_collno = '" + IDCard + "' and loancolltype_code = '01'", row + 1, DwColl.RowCount);

            if (find_row > 0) //กรณีที่พบเลขซ้ำจะได้ค่า find_row = แถวที่ตรวจพบเลขซ้ำ
                return true;
            else
                return false;
        }


    }
}