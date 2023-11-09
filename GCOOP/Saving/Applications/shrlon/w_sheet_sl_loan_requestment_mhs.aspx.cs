using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Saving.WcfCommon;
using Saving.WcfShrlon;
using Saving.WcfBusscom;
using Sybase.DataWindow;
using System.Web.Services.Protocols;
using DataLibrary;
using System.Data;

namespace Saving.Applications.shrlon
{
    public partial class w_sheet_sl_loan_requestment_mhs : PageWebSheet, WebSheet
    {
        public String lc_loangroup = "02";
        public String lc_loantype = null;
        private DwThDate tDwMain;
        //*******ประกาศตัวเกี่ยวกับ  Javascript********//
        protected String jsExpenseBank;
        protected String jsExpenseCode;
        protected String jsGetMemberInfo;
        protected String jsGetMemberCollno;
        protected String jsReNewPage;
        protected String jsOpenOldDocNo;
        protected String jsPostSetZero;
        protected String jsSetpriod;
        protected String jsCancelRequest;
        protected String jsRefresh;
        protected String jsCollInitP;
        protected String jsCollCondition;
        protected String jsSetDataList;
        protected String jsPostColl;
        protected String jsSumOthClr;
        protected String jsContPeriod;
        protected String jsChangeStartkeep;
        protected String jsResumLoanClear;
        protected String jsPermissSalary;
        protected String jsRevert;
        protected String jsPaycoopid;
        protected String jsObjective;
        protected String jsSetloantype;
        protected String jsPostreturn;
        protected String jsmaxcreditperiod;
        protected String jsLoanpaymenttype;
        //*******ประกาศตัวเกี่ยวกับ  Reprot********//
        string reqdoc_no = "";
        String member_no = "";
        string fromset = "";
        int x = 1;
        protected String app;
        protected String gid;
        protected String rid;
        protected String pdf;

        protected String jsrunProcessLoan;
        protected String jsRunProcessInvoice;
        protected String jsrunProcessAgreeLoan;
        protected String jsrunProcessAgreeColl;
        protected String jsrunProcessCollReport;
        protected String jsrunProcessDeptReport;

        protected String jspopupAgreeLoanReport;
        protected String jspopupAgreeCollReport;
        protected String jspopupCollReport;
        protected String jspopupReportInvoice;
        protected String jspopupLoanReport;
        protected String jspopupDeptReport;


        //*******end Reprot********//

        private ShrlonClient shrlonService;
        private BusscomClient BusscomService;
        private CommonClient commonService;
        String ls_membtype = "";
        String loantype = "";
        int li_cramationstatus = 0;//สถานะฌาปนกิจ
        String pbl = "sl_loan_requestment_cen.pbl";

        /// <summary>
        /// Check Init Javascript
        /// </summary>
        public void InitJsPostBack()
        {
            jsExpenseCode = WebUtil.JsPostBack(this, "jsExpenseCode");
            jsExpenseBank = WebUtil.JsPostBack(this, "jsExpenseBank");
            jsGetMemberInfo = WebUtil.JsPostBack(this, "jsGetMemberInfo");
            jsGetMemberCollno = WebUtil.JsPostBack(this, "jsGetMemberCollno");
            jsReNewPage = WebUtil.JsPostBack(this, "jsReNewPage");
            jsOpenOldDocNo = WebUtil.JsPostBack(this, "jsOpenOldDocNo");
            jsPostSetZero = WebUtil.JsPostBack(this, "jsPostSetZero");
            jsSetpriod = WebUtil.JsPostBack(this, "jsSetpriod");
            jsCancelRequest = WebUtil.JsPostBack(this, "jsCancelRequest");
            jsRefresh = WebUtil.JsPostBack(this, "jsRefresh");
            jsCollInitP = WebUtil.JsPostBack(this, "jsCollInitP");
            jsCollCondition = WebUtil.JsPostBack(this, "jsCollCondition");
            jsSetDataList = WebUtil.JsPostBack(this, "jsSetDataList");
            jsPostColl = WebUtil.JsPostBack(this, "jsPostColl");
            jsSumOthClr = WebUtil.JsPostBack(this, "jsSumOthClr");
            jsChangeStartkeep = WebUtil.JsPostBack(this, "jsChangeStartkeep");
            jsContPeriod = WebUtil.JsPostBack(this, "jsContPeriod");
            jsResumLoanClear = WebUtil.JsPostBack(this, "jsResumLoanClear");
            jsPermissSalary = WebUtil.JsPostBack(this, "jsPermissSalary");
            jsRevert = WebUtil.JsPostBack(this, "jsRevert");
            jsPaycoopid = WebUtil.JsPostBack(this, "jsPaycoopid");
            jsObjective = WebUtil.JsPostBack(this, "jsObjective");
            jsSetloantype = WebUtil.JsPostBack(this, "jsSetloantype");
            jsPostreturn = WebUtil.JsPostBack(this, "jsPostreturn");
            jsmaxcreditperiod = WebUtil.JsPostBack(this, "jsmaxcreditperiod");


            jsrunProcessLoan = WebUtil.JsPostBack(this, "jsrunProcessLoan");
            jsRunProcessInvoice = WebUtil.JsPostBack(this, "jsRunProcessInvoice");
            jsrunProcessAgreeColl = WebUtil.JsPostBack(this, "jsrunProcessAgreeColl");
            jsrunProcessCollReport = WebUtil.JsPostBack(this, "jsrunProcessCollReport");
            jsrunProcessAgreeLoan = WebUtil.JsPostBack(this, "jsrunProcessAgreeLoan");
            jsrunProcessDeptReport = WebUtil.JsPostBack(this, "jsrunProcessDeptReport");
            jspopupAgreeLoanReport = WebUtil.JsPostBack(this, "jspopupAgreeLoanReport");
            jspopupAgreeCollReport = WebUtil.JsPostBack(this, "jspopupAgreeCollReport");
            jspopupCollReport = WebUtil.JsPostBack(this, "jspopupCollReport");
            jspopupReportInvoice = WebUtil.JsPostBack(this, "jspopupReportInvoice");
            jspopupLoanReport = WebUtil.JsPostBack(this, "jspopupLoanReport");
            jspopupDeptReport = WebUtil.JsPostBack(this, "jspopupDeptReport");

            tDwMain = new DwThDate(dw_main, this);
            tDwMain.Add("loanrequest_date", "loanrequest_tdate");
            tDwMain.Add("loanrcvfix_date", "loanrcvfix_tdate");
            tDwMain.Add("startkeep_date", "startkeep_tdate");

        }

        public void WebSheetLoadBegin()
        {

            try
            {
                shrlonService = wcf.Shrlon;
                commonService = wcf.Common;
                BusscomService = wcf.Busscom;

            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ติดต่อ Web Service ไม่ได้");
                return;
            }

            if (IsPostBack)
            {
                this.RestoreContextDw(dw_main);
                this.RestoreContextDw(dw_coll);
                this.RestoreContextDw(dw_clear);
                this.RestoreContextDw(dw_otherclr);
                String setting = dw_coll.Describe("coop_id.Protect");
                bool isChecked = CbCheckcoop.Checked;
                if (isChecked)
                {
                    dw_coll.Modify("coop_id.Protect=0");
                    RetreiveDDDW();
                    JsExpenseCode();
                }
                else
                {

                    dw_coll.SetItemString(1, "coop_id", state.SsCoopControl);
                    dw_coll.Modify("coop_id.Protect=1");
                }
            }
            if (dw_main.RowCount < 1)
            {
                dw_main.DisplayOnly = false;
                dw_clear.DisplayOnly = false;
                dw_coll.DisplayOnly = false;
                dw_otherclr.DisplayOnly = false;
                Session["strItemchange"] = "";
                dw_main.InsertRow(0);
                dw_main.SetItemString(1, "memcoop_id", state.SsCoopControl);
                RetreiveDDDW();
                JsChangeStartkeep();
                tDwMain.Eng2ThaiAllRow();

                CbCheckcoop.Checked = false;
                HdIsPostBack.Value = "false";
            }
            if (dw_main.RowCount > 1)
            {
                dw_main.DeleteRow(dw_main.RowCount);
            }
            dw_message.Reset();
            dw_message.InsertRow(0);
            dw_message.DisplayOnly = false;
            dw_message.Visible = false;
        }

        /// <summary>
        /// Check  PostBack Javascript
        /// </summary>
        /// <param name="eventArg"></param>
        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "jsGetMemberInfo")
            {
                JsGetMemberInfo();
            }
            else if (eventArg == "jsExpenseBank")
            {
                JsExpenseBank();
            }
            else if (eventArg == "jsExpenseCode")
            {
                JsExpenseCode();
            }
            else if (eventArg == "jsGetMemberCollno")
            {
                JsGetMemberCollno();
            }
            else if (eventArg == "jsReNewPage")
            {
                JsReNewPage();
            }
            else if (eventArg == "jsOpenOldDocNo")
            {
                JsOpenOldDocNo();
            }
            else if (eventArg == "jsPostSetZero")
            {
                //  LtServerMessage.Text = "ยอดเงินขอกู้มากกว่ายอดเงินสิทธิ์ให้กู้ ";
                //hardcode 
                //dw_main.SetItemDecimal(1, "loanrequest_amt", 0);
            }
            else if (eventArg == "jsSetpriod")
            {
                JsSetpriod();

            }
            else if (eventArg == "jsCancelRequest")
            {

                JsCancelRequest();
            }
            else if (eventArg == "jsRefresh")
            {
            }
            else if (eventArg == "jsCollInitP")
            {
                JsCollInitP();
            }
            else if (eventArg == "jsCollCondition")
            {
                JsCollCondition();
            }
            else if (eventArg == "jsSetDataList")
            {
                JsSetDataList();
            }
            else if (eventArg == "jsPostColl")
            {
                JsPostColl();
            }
            else if (eventArg == "jsChangeStartkeep")
            {
                JsChangeStartkeep();
            }
            else if (eventArg == "jsSumOthClr")
            {
                JsSumOthClr();
            }
            else if (eventArg == "jsContPeriod")
            {
                JsContPeriod();

            }
            else if (eventArg == "jsResumLoanClear")
            {

                JsResumLoanClear();

            }
            else if (eventArg == "jsPermissSalary")
            {
                JsPermissSalary();
            }
            else if (eventArg == "jsRevert")
            {
                JsRevert();
            }
            else if (eventArg == "jsObjective")
            {
                JsObjective();
            }
            else if (eventArg == "jsPaycoopid")
            {
                JsPaycoopid();
            }


            else if (eventArg == "jsrunProcessLoan") { JsrunProcessLoan(); }
            else if (eventArg == "jsRunProcessInvoice") { JsRunProcessInvoice(); }
            else if (eventArg == "jsrunProcessAgreeColl") { JsrunProcessAgreeColl(); }
            else if (eventArg == "jsrunProcessCollReport") { JsrunProcessCollReport(); }
            else if (eventArg == "jsrunProcessAgreeLoan") { JsrunProcessAgreeLoan(); }

            else if (eventArg == "jspopupAgreeLoanReport") { JspopupAgreeLoanReport(); }
            else if (eventArg == "jspopupAgreeCollReport") { JspopupAgreeCollReport(); }
            else if (eventArg == "jspopupCollReport") { JspopupCollReport(); }
            else if (eventArg == "jspopupReportInvoice") { JspopupReportInvoice(); }
            else if (eventArg == "jspopupLoanReport") { JspopupLoanReport(); }
            else if (eventArg == "jspopupDeptReport") { JspopupDeptReport(); }

            else if (eventArg == "jsSetloantype")
            {
                JsSetloantype();
            }
            else if (eventArg == "jsPostreturn")
            {
                JsPostreturn();
            }
            else if (eventArg == "jsmaxcreditperiod")
            {
                Jsmaxcreditperiod();
            }

            else if (eventArg == "jsLoanpaymenttype") { JsLoanpaymenttype(); }

        }

        private void JspopupLoanReport()
        {
            //เด้ง Popup ออกรายงานเป็น PDF.
            //String pop = "Gcoop.OpenPopup('http://localhost/GCOOP/WebService/Report/PDF/20110223093839_shrlonchk_SHRLONCHK001.pdf')";

            //String pop = "Gcoop.OpenPopup('" + Session["Loan"].ToString() + "')";
            //Ltjspopup.Text = "<script>" + pop + "</script>";
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "DsReport", pop, true);
            reqdoc_no = txt_reqNo.Text;
            member_no = txt_member_no.Text;
            string as_xml = "";
            try
            {
                try
                {
                    fromset = state.SsPrinterSet;

                }
                catch (Exception ex)
                {
                    fromset = "216";

                }
                string re = wcf.Shrlon.of_printloan(state.SsWsPass, reqdoc_no, fromset, state.SsCoopControl, member_no, ref as_xml);
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage("ตรวจสอบเครื่องพิมพ์" + ex); }


        }

        private void JspopupLoanReport(bool isJsPrint, int printMode)
        {
            reqdoc_no = txt_reqNo.Text;
            try
            {
                Printing.ShrlonPrintLoanReport(this, state.SsCoopId, reqdoc_no, printMode);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ตรวจสอบเครื่องพิมพ์: " + ex.Message);
            }
        }

        private void JspopupReportInvoice()
        {
            //เด้ง Popup ออกรายงานเป็น PDF.
            //String pop = "Gcoop.OpenPopup('http://localhost/GCOOP/WebService/Report/PDF/20110223093839_shrlonchk_SHRLONCHK001.pdf')";

            //String pop = "Gcoop.OpenPopup('" + Session["pdfinvoice"].ToString() + "')";
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "DsReport", pop, true);
        }
        private void JsRunProcessInvoice()
        {

            // --- Page Arguments
            try
            {
                app = Request["app"].ToString();
            }
            catch { }
            if (app == null || app == "")
            {
                app = state.SsApplication;
            }
            try
            {
                //gid = Request["gid"].ToString();
                gid = "LNNORM_DAILY";
            }
            catch { }
            try
            {
                //rid = Request["rid"].ToString();
                rid = "LNNORM_DAILY13";
            }
            catch { }

            String doc_no = "";// DwMain.GetItemString(1, "loanrequest_docno");

            String coop_id;
            try { coop_id = dw_main.GetItemString(1, "coop_id"); }
            catch { coop_id = state.SsCoopId; }
            if (x == 2)
            {
                doc_no = reqdoc_no;
            }

            if (doc_no == null || doc_no == "")
            {
                return;
            }


            //แปลง Criteria ให้อยู่ในรูปแบบมาตรฐาน.
            ReportHelper lnv_helper = new ReportHelper();
            lnv_helper.AddArgument(coop_id, ArgumentType.String);
            lnv_helper.AddArgument(doc_no, ArgumentType.String);

            //ชื่อไฟล์ PDF = YYYYMMDDHHMMSS_<GID>_<RID>.PDF

            String pdfFileName = DateTime.Now.ToString("yyyyMMddHHmmss", WebUtil.EN);
            pdfFileName += "_" + gid + "_" + rid + ".pdf";
            pdfFileName = pdfFileName.Trim();

            //ส่งให้ ReportService สร้าง PDF ให้ {โดยปกติจะอยู่ใน C:\GCOOP\Saving\PDF\}.
            try
            {
                Saving.WcfReport.ReportClient lws_report = wcf.Report;
                String criteriaXML = lnv_helper.PopArgumentsXML();
                this.pdf = lws_report.GetPDFURL(state.SsWsPass) + pdfFileName;
                String li_return = lws_report.Run(state.SsWsPass, app, gid, rid, criteriaXML, pdfFileName);
                if (li_return == "true")
                {
                    HdOpenIFrame.Value = "True";
                    HdcheckPdf.Value = "True";

                }
                else if (li_return != "true")
                {
                    HdcheckPdf.Value = "False";
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                return;
            }
            Session["pdfinvoice"] = pdf;
            //PopupReport();


        }
        private void JspopupCollReport()
        {
            //เด้ง Popup ออกรายงานเป็น PDF.
            //String pop = "Gcoop.OpenPopup('http://localhost/GCOOP/WebService/Report/PDF/20110223093839_shrlonchk_SHRLONCHK001.pdf')";

            //String pop = "Gcoop.OpenPopup('" + Session["Coll"].ToString() + "')";
            //Ltjspopup.Text = "<script>" + pop + "</script>";
            ////ClientScript.RegisterClientScriptBlock(this.GetType(), "DsReport", pop, true);

            reqdoc_no = txt_reqNo.Text;
            member_no = txt_member_no.Text;
            string as_xml = "";
            try
            {
                try
                {
                    fromset = state.SsPrinterSet;

                }
                catch (Exception ex)
                {
                    fromset = "216";

                }
                string re = wcf.Shrlon.of_printloancoll(state.SsWsPass, reqdoc_no, fromset, state.SsCoopId, member_no, ref as_xml);
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage("ตรวจสอบเครื่องพิมพ์" + ex); }
        }

        private void JspopupAgreeCollReport()
        {
            //เด้ง Popup ออกรายงานเป็น PDF.
            //String pop = "Gcoop.OpenPopup('http://localhost/GCOOP/WebService/Report/PDF/20110223093839_shrlonchk_SHRLONCHK001.pdf')";

            //String pop = "Gcoop.OpenPopup('" + Session["AgreeColl"].ToString() + "')";
            ////   ClientScript.RegisterClientScriptBlock(this.GetType(), "DsReport", pop, true);
            //Ltjspopup.Text = "<script>" + pop + "</script>";

            reqdoc_no = txt_reqNo.Text;
            member_no = txt_member_no.Text;
            string as_xml = "";
            try
            {
                try
                {
                    fromset = state.SsPrinterSet;

                }
                catch (Exception ex)
                {
                    fromset = "216";

                }
                string re = wcf.Shrlon.of_printloancollagree(state.SsWsPass, reqdoc_no, fromset, state.SsCoopId, member_no, ref as_xml);
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage("ตรวจสอบเครื่องพิมพ์" + ex); }
        }

        private void JspopupAgreeLoanReport()
        {
            //เด้ง Popup ออกรายงานเป็น PDF.
            //String pop = "Gcoop.OpenPopup('http://localhost/GCOOP/WebService/Report/PDF/20110223093839_shrlonchk_SHRLONCHK001.pdf')";

            String pop = "Gcoop.OpenPopup('" + Session["AgreeLoan"].ToString() + "')";
            Ltjspopup.Text = "<script>" + pop + "</script>";
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "DsReport", pop, true);
            //reqdoc_no = DwMain.GetItemString(1, "loanrequest_docno");
            //member_no = DwMain.GetItemString(1, "member_no");
            //try
            //{
            //    try
            //    {
            //        fromset = state.SsPrinterSet;

            //    }
            //    catch (Exception ex)
            //    {
            //        fromset = "216";

            //    }
            //    string re = wcf.Shrlon.of_printloanagree(state.SsWsPass, reqdoc_no, fromset, state.SsCoopId, member_no);
            //}
            //catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage("ตรวจสอบเครื่องพิมพ์" + ex); }

        }

        private void JsrunProcessAgreeLoan()
        {

            // --- Page Arguments
            try
            {
                app = Request["app"].ToString();
            }
            catch { }
            if (app == null || app == "")
            {
                app = state.SsApplication;
            }
            try
            {
                //gid = Request["gid"].ToString();
                gid = "LNNORM_DAILY";
            }
            catch { }
            try
            {
                //rid = Request["rid"].ToString();
                rid = "LNNORM_DAILY18";
            }
            catch { }

            String doc_no = "";
            //= dw_main.GetItemString(1, "loanrequest_docno");

            String coop_id;
            try { coop_id = dw_main.GetItemString(1, "coop_id"); }
            catch { coop_id = state.SsCoopId; }
            if (x == 2)
            {
                doc_no = reqdoc_no;
            }
            if (doc_no == null || doc_no == "")
            {
                return;
            }

            //String ls_xmlmain = dw_main.Describe("DataWindow.Data.XML");
            //String ls_format = "CAT";
            //short li_membtime = 0; Decimal ldc_right25 = 0; Decimal ldc_right33 = 0; Decimal ldc_right35 = 0; Decimal ldc_right26 = 0; Decimal ldc_right40 = 0;
            //int re = shrlonService.of_print_callpermiss(state.SsWsPass, ls_xmlmain, ls_format, ref li_membtime, ref ldc_right25, ref ldc_right33, ref ldc_right35, ref ldc_right26, ref ldc_right40);
            //Decimal li_membtime_ = li_membtime;
            //Decimal ldc_right25_ = ldc_right25;
            //Decimal ldc_right33_ = ldc_right33;
            //Decimal ldc_right35_ = ldc_right35;
            //Decimal ldc_right26_ = ldc_right26;
            //Decimal ldc_right40_ = ldc_right40;
            //string loan26 = dw_main.GetItemString(1, "loantype_code");
            //if (loan26 == "26")
            //{
            //    decimal right26 = dw_main.GetItemDecimal(1, "loancredit_amt");
            //    ldc_right26_ = right26;
            //    ldc_right33_ = 0;
            //    ldc_right35_ = 0;
            //    ldc_right25_ = 0;
            //}


            //แปลง Criteria ให้อยู่ในรูปแบบมาตรฐาน.
            ReportHelper lnv_helper = new ReportHelper();
            lnv_helper.AddArgument(coop_id, ArgumentType.String);
            lnv_helper.AddArgument(doc_no, ArgumentType.String);
            //lnv_helper.AddArgument(li_membtime_.ToString(), ArgumentType.Number);
            //lnv_helper.AddArgument(ldc_right25_.ToString(), ArgumentType.Number);
            //lnv_helper.AddArgument(ldc_right33_.ToString(), ArgumentType.Number);
            //lnv_helper.AddArgument(ldc_right35_.ToString(), ArgumentType.Number);
            //lnv_helper.AddArgument(ldc_right26_.ToString(), ArgumentType.Number);
            //lnv_helper.AddArgument(ldc_right40_.ToString(), ArgumentType.Number);

            //ชื่อไฟล์ PDF = YYYYMMDDHHMMSS_<GID>_<RID>.PDF

            String pdfFileName = DateTime.Now.ToString("yyyyMMddHHmmss", WebUtil.EN);
            pdfFileName += "_" + gid + "_" + rid + ".pdf";
            pdfFileName = pdfFileName.Trim();

            //ส่งให้ ReportService สร้าง PDF ให้ {โดยปกติจะอยู่ใน C:\GCOOP\Saving\PDF\}.
            try
            {
                Saving.WcfReport.ReportClient lws_report = wcf.Report;
                String criteriaXML = lnv_helper.PopArgumentsXML();
                this.pdf = lws_report.GetPDFURL(state.SsWsPass) + pdfFileName;
                String li_return = lws_report.Run(state.SsWsPass, app, gid, rid, criteriaXML, pdfFileName);
                if (li_return == "true")
                {
                    HdOpenIFrame.Value = "True";
                    HdcheckPdf.Value = "True";

                }
                else if (li_return != "true")
                {
                    HdcheckPdf.Value = "False";
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                return;
            }

            //Session["AgreeLoan"] = pdf;
            // PopupReport();
            Ltjspopup.Text += "<a href = '" + pdf + "' target='_blank'>ยินยอมผู้กู้</a><br/>";

        }

        private void JsrunProcessCollReport()
        {

            // --- Page Arguments
            try
            {
                app = Request["app"].ToString();
            }
            catch { }
            if (app == null || app == "")
            {
                app = state.SsApplication;
            }
            try
            {
                //gid = Request["gid"].ToString();
                gid = "LNNORM_DAILY";
            }
            catch { }
            try
            {
                //rid = Request["rid"].ToString();
                rid = "LNNORM_DAILY17";
            }
            catch { }

            String doc_no = "";// DwMain.GetItemString(1, "loanrequest_docno");

            String coop_id;
            try { coop_id = dw_main.GetItemString(1, "coop_id"); }
            catch { coop_id = state.SsCoopId; }
            if (x == 2)
            {
                doc_no = reqdoc_no;
            }

            if (doc_no == null || doc_no == "")
            {
                return;
            }

            //String ls_xmlmain = dw_main.Describe("DataWindow.Data.XML");
            //String ls_format = "CAT";
            //short li_membtime = 0; Decimal ldc_right25 = 0; Decimal ldc_right33 = 0; Decimal ldc_right35 = 0; Decimal ldc_right26 = 0; Decimal ldc_right40 = 0;
            //int re = shrlonService.of_print_callpermiss(state.SsWsPass, ls_xmlmain, ls_format, ref li_membtime, ref ldc_right25, ref ldc_right33, ref ldc_right35, ref ldc_right26, ref ldc_right40);
            //Decimal li_membtime_ = li_membtime;
            //Decimal ldc_right25_ = ldc_right25;
            //Decimal ldc_right33_ = ldc_right33;
            //Decimal ldc_right35_ = ldc_right35;
            //Decimal ldc_right26_ = ldc_right26;
            //Decimal ldc_right40_ = ldc_right40;
            //string loan26 = dw_main.GetItemString(1, "loantype_code");
            //if (loan26 == "26")
            //{
            //    decimal right26 = dw_main.GetItemDecimal(1, "loancredit_amt");
            //    ldc_right26_ = right26;
            //    ldc_right33_ = 0;
            //    ldc_right35_ = 0;
            //    ldc_right25_ = 0;
            //}


            //แปลง Criteria ให้อยู่ในรูปแบบมาตรฐาน.
            ReportHelper lnv_helper = new ReportHelper();
            lnv_helper.AddArgument(coop_id, ArgumentType.String);
            lnv_helper.AddArgument(doc_no, ArgumentType.String);
            //lnv_helper.AddArgument(li_membtime_.ToString(), ArgumentType.Number);
            //lnv_helper.AddArgument(ldc_right25_.ToString(), ArgumentType.Number);
            //lnv_helper.AddArgument(ldc_right33_.ToString(), ArgumentType.Number);
            //lnv_helper.AddArgument(ldc_right35_.ToString(), ArgumentType.Number);
            //lnv_helper.AddArgument(ldc_right26_.ToString(), ArgumentType.Number);
            //lnv_helper.AddArgument(ldc_right40_.ToString(), ArgumentType.Number);

            //ชื่อไฟล์ PDF = YYYYMMDDHHMMSS_<GID>_<RID>.PDF

            String pdfFileName = DateTime.Now.ToString("yyyyMMddHHmmss", WebUtil.EN);
            pdfFileName += "_" + gid + "_" + rid + ".pdf";
            pdfFileName = pdfFileName.Trim();

            //ส่งให้ ReportService สร้าง PDF ให้ {โดยปกติจะอยู่ใน C:\GCOOP\Saving\PDF\}.
            try
            {
                Saving.WcfReport.ReportClient lws_report = wcf.Report;
                String criteriaXML = lnv_helper.PopArgumentsXML();
                this.pdf = lws_report.GetPDFURL(state.SsWsPass) + pdfFileName;
                String li_return = lws_report.Run(state.SsWsPass, app, gid, rid, criteriaXML, pdfFileName);
                if (li_return == "true")
                {
                    HdOpenIFrame.Value = "True";
                    HdcheckPdf.Value = "True";

                }
                else if (li_return != "true")
                {
                    HdcheckPdf.Value = "False";
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                return;
            }
            // Session["Coll"] = pdf;
            Ltjspopup.Text += "<a href = '" + pdf + "' target='_blank' >สัญญาผู้ค้ำ</a><br/>";
            // PopupReport();

        }

        private void JsrunProcessAgreeColl()
        {

            // --- Page Arguments
            try
            {
                app = Request["app"].ToString();
            }
            catch { }
            if (app == null || app == "")
            {
                app = state.SsApplication;
            }
            try
            {
                //gid = Request["gid"].ToString();
                gid = "LNNORM_DAILY";
            }
            catch { }
            try
            {
                //rid = Request["rid"].ToString();
                rid = "LNNORM_DAILY16";
            }
            catch { }

            String doc_no = "";// DwMain.GetItemString(1, "loanrequest_docno");

            String coop_id;
            try { coop_id = dw_main.GetItemString(1, "coop_id"); }
            catch { coop_id = state.SsCoopId; }
            if (x == 2)
            {
                doc_no = reqdoc_no;
            }

            if (doc_no == null || doc_no == "")
            {
                return;
            }

            //String ls_xmlmain = dw_main.Describe("DataWindow.Data.XML");
            //String ls_format = "CAT";
            //short li_membtime = 0; Decimal ldc_right25 = 0; Decimal ldc_right33 = 0; Decimal ldc_right35 = 0; Decimal ldc_right26 = 0; Decimal ldc_right40 = 0;
            //int re = shrlonService.of_print_callpermiss(state.SsWsPass, ls_xmlmain, ls_format, ref li_membtime, ref ldc_right25, ref ldc_right33, ref ldc_right35, ref ldc_right26, ref ldc_right40);
            //Decimal li_membtime_ = li_membtime;
            //Decimal ldc_right25_ = ldc_right25;
            //Decimal ldc_right33_ = ldc_right33;
            //Decimal ldc_right35_ = ldc_right35;
            //Decimal ldc_right26_ = ldc_right26;
            //Decimal ldc_right40_ = ldc_right40;
            //string loan26 = dw_main.GetItemString(1, "loantype_code");
            //if (loan26 == "26")
            //{
            //    decimal right26 = dw_main.GetItemDecimal(1, "loancredit_amt");
            //    ldc_right26_ = right26;
            //    ldc_right33_ = 0;
            //    ldc_right35_ = 0;
            //    ldc_right25_ = 0;
            //}


            //แปลง Criteria ให้อยู่ในรูปแบบมาตรฐาน.
            ReportHelper lnv_helper = new ReportHelper();
            lnv_helper.AddArgument(coop_id, ArgumentType.String);
            lnv_helper.AddArgument(doc_no, ArgumentType.String);
            //lnv_helper.AddArgument(li_membtime_.ToString(), ArgumentType.Number);
            //lnv_helper.AddArgument(ldc_right25_.ToString(), ArgumentType.Number);
            //lnv_helper.AddArgument(ldc_right33_.ToString(), ArgumentType.Number);
            //lnv_helper.AddArgument(ldc_right35_.ToString(), ArgumentType.Number);
            //lnv_helper.AddArgument(ldc_right26_.ToString(), ArgumentType.Number);
            //lnv_helper.AddArgument(ldc_right40_.ToString(), ArgumentType.Number);

            //ชื่อไฟล์ PDF = YYYYMMDDHHMMSS_<GID>_<RID>.PDF

            String pdfFileName = DateTime.Now.ToString("yyyyMMddHHmmss", WebUtil.EN);
            pdfFileName += "_" + gid + "_" + rid + ".pdf";
            pdfFileName = pdfFileName.Trim();

            //ส่งให้ ReportService สร้าง PDF ให้ {โดยปกติจะอยู่ใน C:\GCOOP\Saving\PDF\}.
            try
            {
                Saving.WcfReport.ReportClient lws_report = wcf.Report;
                String criteriaXML = lnv_helper.PopArgumentsXML();
                this.pdf = lws_report.GetPDFURL(state.SsWsPass) + pdfFileName;
                String li_return = lws_report.Run(state.SsWsPass, app, gid, rid, criteriaXML, pdfFileName);
                if (li_return == "true")
                {
                    HdOpenIFrame.Value = "True";
                    HdcheckPdf.Value = "True";

                }
                else if (li_return != "true")
                {
                    HdcheckPdf.Value = "False";
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                return;
            }
            //  Session["AgreeColl"] = pdf;
            Ltjspopup.Text += "<a href = '" + pdf + "' target='_blank'>ยินยอมผู้ค้ำ</a><br/>";
            // PopupReport();

        }

        private void JsrunProcessLoan()
        {


            // --- Page Arguments
            try
            {
                app = Request["app"].ToString();
            }
            catch { }
            if (app == null || app == "")
            {
                app = state.SsApplication;
            }
            try
            {
                //gid = Request["gid"].ToString();
                gid = "LNNORM_DAILY";
            }
            catch { }
            try
            {
                //rid = Request["rid"].ToString();
                rid = "LNNORM_DAILY15";
            }
            catch { }

            String doc_no = "";// DwMain.GetItemString(1, "loanrequest_docno");

            String coop_id;
            try { coop_id = dw_main.GetItemString(1, "coop_id"); }
            catch { coop_id = state.SsCoopId; }
            if (x == 2)
            {
                doc_no = reqdoc_no;
            }

            if (doc_no == null || doc_no == "")
            {
                return;
            }

            //String ls_xmlmain = dw_main.Describe("DataWindow.Data.XML");
            //String ls_format = "CAT";
            //short li_membtime = 0; Decimal ldc_right25 = 0; Decimal ldc_right33 = 0; Decimal ldc_right35 = 0; Decimal ldc_right26 = 0; Decimal ldc_right40 = 0;
            //int re = shrlonService.of_print_callpermiss(state.SsWsPass, ls_xmlmain, ls_format, ref li_membtime, ref ldc_right25, ref ldc_right33, ref ldc_right35, ref ldc_right26, ref ldc_right40);
            //Decimal li_membtime_ = li_membtime;
            //Decimal ldc_right25_ = ldc_right25;
            //Decimal ldc_right33_ = ldc_right33;
            //Decimal ldc_right35_ = ldc_right35;
            //Decimal ldc_right26_ = ldc_right26;
            //Decimal ldc_right40_ = ldc_right40;
            //string loan26 = dw_main.GetItemString(1, "loantype_code");
            //if (loan26 == "26")
            //{
            //    decimal right26 = dw_main.GetItemDecimal(1, "loancredit_amt");
            //    ldc_right26_ = right26;
            //    ldc_right33_ = 0;
            //    ldc_right35_ = 0;
            //    ldc_right25_ = 0;
            //}


            //แปลง Criteria ให้อยู่ในรูปแบบมาตรฐาน.
            ReportHelper lnv_helper = new ReportHelper();
            lnv_helper.AddArgument(coop_id, ArgumentType.String);
            lnv_helper.AddArgument(doc_no, ArgumentType.String);
            //lnv_helper.AddArgument(li_membtime_.ToString(), ArgumentType.Number);
            //lnv_helper.AddArgument(ldc_right25_.ToString(), ArgumentType.Number);
            //lnv_helper.AddArgument(ldc_right33_.ToString(), ArgumentType.Number);
            //lnv_helper.AddArgument(ldc_right35_.ToString(), ArgumentType.Number);
            //lnv_helper.AddArgument(ldc_right26_.ToString(), ArgumentType.Number);
            //lnv_helper.AddArgument(ldc_right40_.ToString(), ArgumentType.Number);

            //ชื่อไฟล์ PDF = YYYYMMDDHHMMSS_<GID>_<RID>.PDF

            String pdfFileName = DateTime.Now.ToString("yyyyMMddHHmmss", WebUtil.EN);
            pdfFileName += "_" + gid + "_" + rid + ".pdf";
            pdfFileName = pdfFileName.Trim();

            //ส่งให้ ReportService สร้าง PDF ให้ {โดยปกติจะอยู่ใน C:\GCOOP\Saving\PDF\}.
            try
            {
                Saving.WcfReport.ReportClient lws_report = wcf.Report;
                String criteriaXML = lnv_helper.PopArgumentsXML();
                this.pdf = lws_report.GetPDFURL(state.SsWsPass) + pdfFileName;
                String li_return = lws_report.Run(state.SsWsPass, app, gid, rid, criteriaXML, pdfFileName);
                if (li_return == "true")
                {
                    HdOpenIFrame.Value = "True";
                    HdcheckPdf.Value = "True";

                }
                else if (li_return != "true")
                {
                    HdcheckPdf.Value = "False";
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                return;
            }
            //  Session["Loan"] = pdf;
            Ltjspopup.Text += "<a href = '" + pdf + "' target='_blank'>สัญญาผู้กู้</a><br/>";
            // PopupReport();

        }

        private void JspopupDeptReport()
        {
            //เด้ง Popup ออกรายงานเป็น PDF.
            //String pop = "Gcoop.OpenPopup('http://localhost/GCOOP/WebService/Report/PDF/20110223093839_shrlonchk_SHRLONCHK001.pdf')";

            //String pop = "Gcoop.OpenPopup('" + Session["dept"].ToString() + "')";
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "DsReport", pop, true);
            reqdoc_no = txt_reqNo.Text;
            member_no = txt_member_no.Text;
            string as_xml = "";
            try
            {
                try
                {
                    fromset = state.SsPrinterSet;

                }
                catch (Exception ex)
                {
                    fromset = "216";

                }
                string re = wcf.Shrlon.of_printloandept(state.SsWsPass, reqdoc_no, fromset, state.SsCoopId, member_no, ref as_xml);
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage("ตรวจสอบเครื่องพิมพ์" + ex); }
        }
        private void JspopupDeptReport(bool isJsPrint, int printMode)
        {
            try
            {
                reqdoc_no = txt_reqNo.Text;
                Printing.ShrlonPrintDeptReport(this, state.SsCoopId, reqdoc_no, printMode);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ตรวจสอบเครื่องพิมพ์: " + ex.Message);
            }
        }

        private void jJsrunProcessDeptReport()
        {

            // --- Page Arguments
            try
            {
                app = Request["app"].ToString();
            }
            catch { }
            if (app == null || app == "")
            {
                app = state.SsApplication;
            }
            try
            {
                //gid = Request["gid"].ToString();
                gid = "LNNORM_DAILY";
            }
            catch { }
            try
            {
                //rid = Request["rid"].ToString();
                rid = "LNNORM_DAILY19";
            }
            catch { }

            String doc_no = "";// DwMain.GetItemString(1, "loanrequest_docno");

            String coop_id;
            try { coop_id = dw_main.GetItemString(1, "coop_id"); }
            catch { coop_id = state.SsCoopId; }
            if (x == 2)
            {
                doc_no = reqdoc_no;
            }

            if (doc_no == null || doc_no == "")
            {
                return;
            }

            //String ls_xmlmain = dw_main.Describe("DataWindow.Data.XML");
            //String ls_format = "CAT";
            //short li_membtime = 0; Decimal ldc_right25 = 0; Decimal ldc_right33 = 0; Decimal ldc_right35 = 0; Decimal ldc_right26 = 0; Decimal ldc_right40 = 0;
            //int re = shrlonService.of_print_callpermiss(state.SsWsPass, ls_xmlmain, ls_format, ref li_membtime, ref ldc_right25, ref ldc_right33, ref ldc_right35, ref ldc_right26, ref ldc_right40);
            //Decimal li_membtime_ = li_membtime;
            //Decimal ldc_right25_ = ldc_right25;
            //Decimal ldc_right33_ = ldc_right33;
            //Decimal ldc_right35_ = ldc_right35;
            //Decimal ldc_right26_ = ldc_right26;
            //Decimal ldc_right40_ = ldc_right40;
            //string loan26 = dw_main.GetItemString(1, "loantype_code");
            //if (loan26 == "26")
            //{
            //    decimal right26 = dw_main.GetItemDecimal(1, "loancredit_amt");
            //    ldc_right26_ = right26;
            //    ldc_right33_ = 0;
            //    ldc_right35_ = 0;
            //    ldc_right25_ = 0;
            //}


            //แปลง Criteria ให้อยู่ในรูปแบบมาตรฐาน.
            ReportHelper lnv_helper = new ReportHelper();
            lnv_helper.AddArgument(coop_id, ArgumentType.String);
            lnv_helper.AddArgument(doc_no, ArgumentType.String);
            //lnv_helper.AddArgument(li_membtime_.ToString(), ArgumentType.Number);
            //lnv_helper.AddArgument(ldc_right25_.ToString(), ArgumentType.Number);
            //lnv_helper.AddArgument(ldc_right33_.ToString(), ArgumentType.Number);
            //lnv_helper.AddArgument(ldc_right35_.ToString(), ArgumentType.Number);
            //lnv_helper.AddArgument(ldc_right26_.ToString(), ArgumentType.Number);
            //lnv_helper.AddArgument(ldc_right40_.ToString(), ArgumentType.Number);

            //ชื่อไฟล์ PDF = YYYYMMDDHHMMSS_<GID>_<RID>.PDF

            String pdfFileName = DateTime.Now.ToString("yyyyMMddHHmmss", WebUtil.EN);
            pdfFileName += "_" + gid + "_" + rid + ".pdf";
            pdfFileName = pdfFileName.Trim();

            //ส่งให้ ReportService สร้าง PDF ให้ {โดยปกติจะอยู่ใน C:\GCOOP\Saving\PDF\}.
            try
            {
                Saving.WcfReport.ReportClient lws_report = wcf.Report;
                String criteriaXML = lnv_helper.PopArgumentsXML();
                this.pdf = lws_report.GetPDFURL(state.SsWsPass) + pdfFileName;
                String li_return = lws_report.Run(state.SsWsPass, app, gid, rid, criteriaXML, pdfFileName);
                if (li_return == "true")
                {
                    HdOpenIFrame.Value = "True";
                    HdcheckPdf.Value = "True";

                }
                else if (li_return != "true")
                {
                    HdcheckPdf.Value = "False";
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                return;
            }
            Session["dept"] = pdf;
            // PopupReport();

        }
        private void JspopupReportslipfin()
        {
            //เด้ง Popup ออกรายงานเป็น PDF.
            //String pop = "Gcoop.OpenPopup('http://localhost/GCOOP/WebService/Report/PDF/20110223093839_shrlonchk_SHRLONCHK001.pdf')";

            String pop = "Gcoop.OpenPopup('" + Session["pdfslipfin"].ToString() + "')";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "DsReport", pop, true);
        }
        private void JsRunProcessslipfin()
        {

            // --- Page Arguments
            try
            {
                app = Request["app"].ToString();
            }
            catch { }
            if (app == null || app == "")
            {
                app = state.SsApplication;
            }
            try
            {
                //gid = Request["gid"].ToString();
                gid = "LNNORM_DAILY";
            }
            catch { }
            try
            {
                //rid = Request["rid"].ToString();
                rid = "LNNORM_DAILY20";
            }
            catch { }

            String doc_no = "";// DwMain.GetItemString(1, "loanrequest_docno");

            String coop_id;
            try { coop_id = dw_main.GetItemString(1, "coop_id"); }
            catch { coop_id = state.SsCoopId; }
            if (x == 2)
            {
                doc_no = reqdoc_no;
            }

            if (doc_no == null || doc_no == "")
            {
                return;
            }


            //แปลง Criteria ให้อยู่ในรูปแบบมาตรฐาน.
            ReportHelper lnv_helper = new ReportHelper();
            lnv_helper.AddArgument(coop_id, ArgumentType.String);
            lnv_helper.AddArgument(doc_no, ArgumentType.String);

            //ชื่อไฟล์ PDF = YYYYMMDDHHMMSS_<GID>_<RID>.PDF

            String pdfFileName = DateTime.Now.ToString("yyyyMMddHHmmss", WebUtil.EN);
            pdfFileName += "_" + gid + "_" + rid + ".pdf";
            pdfFileName = pdfFileName.Trim();

            //ส่งให้ ReportService สร้าง PDF ให้ {โดยปกติจะอยู่ใน C:\GCOOP\Saving\PDF\}.
            try
            {
                Saving.WcfReport.ReportClient lws_report = wcf.Report;
                String criteriaXML = lnv_helper.PopArgumentsXML();
                this.pdf = lws_report.GetPDFURL(state.SsWsPass) + pdfFileName;
                String li_return = lws_report.Run(state.SsWsPass, app, gid, rid, criteriaXML, pdfFileName);
                if (li_return == "true")
                {
                    HdOpenIFrame.Value = "True";
                    HdcheckPdf.Value = "True";

                }
                else if (li_return != "true")
                {
                    HdcheckPdf.Value = "False";
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                return;
            }
            Session["pdfslipfin"] = pdf;
            //PopupReport();


        }

        /// <summary>
        ///บันทึกใบคำขอ
        /// </summary>
        public void SaveWebSheet()
        {

            try
            {
                dw_main.SetItemString(1, "coop_id", state.SsCoopId);
                String memcoop_id = dw_main.GetItemString(1, "memcoop_id");
                member_no = dw_main.GetItemString(1, "member_no");
                String dwMain_XML = dw_main.Describe("DataWindow.Data.XML");
                String dwColl_XML = "";
                String dwClear_XML = "";
                String dwOtherClr_XML = "";
                DateTime ldtm_loanrequest = dw_main.GetItemDateTime(1, "loanrequest_date");
                string ls_message = "";
                Decimal use_amt = 0;
                String as_deptaccount = "";
                try
                {
                    for (int i = 1; i <= dw_coll.RowCount; i++)
                    {


                        string ref_collno = "";
                        ref_collno = dw_coll.GetItemString(i, "ref_collno");

                        if (ref_collno != null || ref_collno != "")
                        {
                            dw_coll.SetItemString(i, "coop_id", state.SsCoopId);
                            dw_coll.SetItemString(i, "refcoop_id", state.SsCoopControl);
                        }
                    }

                    //mai กรณี ไม่มีการค้ำประกันให้สามารถบันทึกได้ 
                    if (dw_coll.RowCount > 0)
                    {

                        //dw_coll.SetFilter("ref_collno <>'' ");
                        //dw_coll.Filter();
                        dwColl_XML = dw_coll.Describe("DataWindow.Data.XML");
                    }
                }
                catch { dwColl_XML = ""; }
                try
                {
                    for (int i = 1; i <= dw_clear.RowCount; i++)
                    {
                        dw_clear.SetItemString(i, "coop_id", state.SsCoopId);
                        dw_clear.SetItemString(i, "concoop_id", state.SsCoopControl);
                    }

                    if (dw_clear.RowCount > 0)
                    {
                        dwClear_XML = dw_clear.Describe("DataWindow.Data.XML");
                    }

                }
                catch { dwClear_XML = ""; }


                try
                {
                    for (int i = 1; i <= dw_otherclr.RowCount; i++)
                    {
                        dw_otherclr.SetItemString(i, "coop_id", state.SsCoopId);

                    }
                    if (dw_otherclr.RowCount > 0)
                    {
                        dwOtherClr_XML = dw_otherclr.Describe("DataWindow.Data.XML");
                    }

                }
                catch { dwOtherClr_XML = ""; }
                str_itemchange strList = new str_itemchange();
                str_savereqloan strSave = new str_savereqloan();
                strList = WebUtil.str_itemchange_session(this);


                strSave.xml_main = dwMain_XML;
                strSave.xml_clear = dwClear_XML;
                strSave.xml_guarantee = dwColl_XML;
                strSave.xml_otherclr = dwOtherClr_XML;
                strSave.contcoopid = state.SsCoopControl;
                strSave.format_type = "CAT";
                strSave.entry_id = state.SsUsername;
                strSave.coop_id = state.SsCoopId;
                String period_payamt = dw_main.GetItemDecimal(1, "period_payamt").ToString("0.00");
                bool is_point1 = period_payamt.IndexOf(".00") < 0;

                String period_payment = dw_main.GetItemDecimal(1, "period_payment").ToString("0.00");
                bool is_point2 = period_payment.IndexOf(".00") < 0;

                if (is_point1 == true || is_point2 == true)
                {
                    if (is_point1 == true) { LtServerMessage.Text = WebUtil.ErrorMessage("จำนวนงวดเป็นทศนิยม =" + period_payamt); }
                    else if (is_point2 == true) { LtServerMessage.Text = WebUtil.ErrorMessage("ต้นชำระเป็นทศนิยม =" + period_payment); }
                    else { LtServerMessage.Text = WebUtil.ErrorMessage("ยอดค้ำน้อยกว่ายอดขอกู้ กรุณาตรวจสอบ หรือกด คำนวณ % ใหม่อีกครั้ง "); }
                }
                else
                {
                    String runningNo = shrlonService.LoanRightSaveReqloan(state.SsWsPass, ref strSave);
                    reqdoc_no = strSave.request_no;
                    txt_reqNo.Text = reqdoc_no;
                    txt_member_no.Text = member_no;
                    x = 2;
                    //เช็คอายัดเงินฝากค้ำประกัน
                    for (int i = 1; i <= dw_coll.RowCount; i++)
                    {
                        string loancolltype_code = dw_coll.GetItemString(i, "loancolltype_code");
                        if (loancolltype_code == "03")
                        {
                            use_amt = dw_coll.GetItemDecimal(i, "use_amt");
                            as_deptaccount = dw_coll.GetItemString(i, "ref_collno");
                            int reslut = shrlonService.of_autosequest(state.SsWsPass, as_deptaccount, state.SsCoopControl, use_amt, ldtm_loanrequest, state.SsClientIp, ref ls_message);
                        }

                    }
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเรียบร้อยแล้ว");
                    String ls_loantype = dw_main.GetItemString(1, "loantype_code");
                    Session["loantypeCode"] = ls_loantype;
                    JsReNewPage();
                    LinkButton1.Visible = true;
                    LinkButton2.Visible = true;
                }
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
        }


        public void WebSheetLoadEnd()
        {
            JsExpenseCode();
            try
            {
                //DataWindowChild dwloantype_code = dw_main.GetChild("loantype_code_1");
                //dwloantype_code.SetFilter("LNLOANTYPE.LOANGROUP_CODE   ='" + lc_loangroup + "'");
                //dwloantype_code.Filter();

                //  JsSumOthClr();
                str_itemchange strList = new str_itemchange();
                strList.xml_main = dw_main.Describe("DataWindow.Data.XML");

                if (dw_clear.RowCount == 0)
                { strList.xml_clear = null; }
                else { strList.xml_clear = dw_clear.Describe("DataWindow.Data.XML"); }
                if (dw_coll.RowCount == 0) { strList.xml_guarantee = null; }
                else { strList.xml_guarantee = dw_coll.Describe("DataWindow.Data.XML"); }
                Session["strItemchange"] = strList;

                //หาจำนวนงวดที่ชำระ
                string member_no = dw_main.GetItemString(1, "member_no");
                if (member_no != null || member_no != "")
                {
                    DateTime startkeep_date = dw_main.GetItemDate(1, "startkeep_date");
                    DateTime retry_date = dw_main.GetItemDateTime(1, "retry_date");
                    Int32 month_ = 12 - (startkeep_date.Month - 1);
                    Int32 retryage = (retry_date.Year - startkeep_date.Year - 1) * 12 + 9 + month_;
                    dw_main.SetItemDecimal(1, "retry_age", retryage);
                }
            }
            catch { }

            DwUtil.RetrieveDDDW(dw_coll, "loancolltype_code", pbl, null);
            dw_main.SaveDataCache();//main
            dw_coll.SaveDataCache();//หลักประกัน
            dw_clear.SaveDataCache();//หักกลบ     
            dw_otherclr.SaveDataCache();//หักอื่น
        }

        /// <summary>
        ///  reset หน้าใหม่
        /// </summary>
        private void JsReNewPage()
        {
            dw_main.Reset();
            dw_main.InsertRow(0);
            dw_coll.Reset();
            dw_clear.Reset();
            dw_otherclr.Reset();
            dw_main.SetItemString(1, "memcoop_id", state.SsCoopControl);
            RetreiveDDDW();
            JsChangeStartkeep();
            tDwMain.Eng2ThaiAllRow();
            HdIsPostBack.Value = "false";

            //DataWindowChild dwloantype_code = dw_main.GetChild("loantype_code_1");
            //dwloantype_code.SetFilter("LNLOANTYPE.LOANTYPE_CODE in ('20','21','22') ");
            //dwloantype_code.Filter();
        }

        /// <summary>
        ///  retreive datawindows dropdown
        /// </summary>
        public void RetreiveDDDW()
        {

            try
            {

                loantype = Session["loantypeCode"].ToString();

                Session.Remove("loantype");

            }
            catch
            {
                String sql = "";


                sql = "select min(loantype_code) from lnloantype where  coop_id = '" + state.SsCoopControl + "'";

                DataTable dt = WebUtil.Query(sql);
                if (dt.Rows.Count > 0)
                {
                    lc_loantype = dt.Rows[0][0].ToString().Trim();
                    loantype = lc_loantype;
                    Session.Remove("loantype");
                }
                else
                {
                    throw new Exception("ไม่พบประเภทเงินกู้");
                }
            }
            try
            {
                DwUtil.RetrieveDDDW(dw_main, "loantype_code_1", pbl, null);
                if (loantype == "")
                {
                    ///<กำหนดค่าเริ่มต้น เป็นสามัญ>

                    dw_main.SetItemString(1, "loantype_code", lc_loantype);

                }
                else
                {
                    //mai แก้ไข retrieve Dropdown
                    DataWindowChild dwloantype_code = dw_main.GetChild("loantype_code_1");
                    //dwloantype_code.SetFilter("LNLOANTYPE.LOANGROUP_CODE   ='" + lc_loangroup + "'");
                    //dwloantype_code.Filter();
                    dw_main.SetItemString(1, "loantype_code", loantype);
                }
                DwUtil.RetrieveDDDW(dw_main, "expense_code", pbl, null);
                DwUtil.RetrieveDDDW(dw_main, "loanobjective_code_1", pbl, null);
                DwUtil.RetrieveDDDW(dw_main, "membtype_code", pbl, null);
                DwUtil.RetrieveDDDW(dw_main, "coop_id_1", pbl, null);
                DwUtil.RetrieveDDDW(dw_main, "memcoop_id", pbl, null);
                DwUtil.RetrieveDDDW(dw_main, "paytoorder_desc_1", pbl, null);
                //mai แก้ไขเพิ่ม coop_id
                DataWindowChild paytoorder = dw_main.GetChild("paytoorder_desc_1");
                //if (state.SsUsername == "Loan01")
                //{

                //    dw_main.SetItemString(1, "paytoorder_desc", "001001");
                //}
                //else if (state.SsUsername == "Loan02")
                //{

                //    dw_main.SetItemString(1, "paytoorder_desc", "001002");
                //}
                //else if (state.SsUsername == "Loan03")
                //{

                //    dw_main.SetItemString(1, "paytoorder_desc", "001003");
                //}
                //else if (state.SsUsername == "Loan04")
                //{

                //    dw_main.SetItemString(1, "paytoorder_desc", "001004");
                //}

                // else {
                dw_main.SetItemString(1, "paytoorder_desc", state.SsCoopId);
                //}
                //mai เพิ่ม coop_id
                dw_main.SetItemString(1, "coop_id", state.SsCoopId);

                DwUtil.RetrieveDDDW(dw_coll, "loancolltype_code", pbl, null);
                DwUtil.RetrieveDDDW(dw_coll, "coop_id", pbl, null);
                DwUtil.RetrieveDDDW(dw_otherclr, "clrothertype_code", pbl, null);


            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
        }

        /// <summary>
        /// กำหนดวันที่จ่ายเงินกู้และวันที่เรียกเก็บ
        /// </summary>
        private void JsChangeStartkeep()
        {
            try
            {

                DateTime postingdate = new DateTime();
                DateTime processdate = new DateTime();
                DateTime ldtm_loanreceive = new DateTime();
                dw_main.SetItemDateTime(1, "loanrequest_date", state.SsWorkDate);
                DateTime ldtm_loanrequest = dw_main.GetItemDateTime(1, "loanrequest_date");
                String loantype = dw_main.GetItemString(1, "loantype_code");
                short ai_increase = 0;

                //ประเภคบุคคลค้ำ จ่ายถัดวันขอกู้อีก 3 วันทำการ
                //a hardcode
                if (loantype == "23" || loantype == "26")
                {
                    ai_increase = 3;
                    ldtm_loanreceive = wcf.Busscom.of_relativeworkdate(state.SsWsPass, ldtm_loanrequest, ai_increase);
                }
                else
                {
                    ldtm_loanreceive = ldtm_loanrequest;
                }
                int year = Convert.ToInt16(ldtm_loanreceive.Year + 543);
                short month = Convert.ToInt16(ldtm_loanreceive.Month);
                //a hardcode
                String sqlpro = " SELECT MAX(LASTPROCESS_DATE)as LASTPROCESS_DATE FROM  LNCONTMASTER ";
                Sdt dtpro = WebUtil.QuerySdt(sqlpro);
                if (dtpro.Next())
                {

                    //processdate = dtpro.GetDate("LASTPROCESS_DATE");

                    //กรณี วันที่จ่ายเงิน=วันที่ขอกู้
                    processdate = wcf.Busscom.of_getpostingdate(state.SsWsPass, ldtm_loanreceive);
                }

                //จ่ายเงินกู้หลังเรียกเก็บหรือไม่
                if (ldtm_loanreceive < processdate)
                {
                    month = Convert.ToInt16(ldtm_loanreceive.Month + 1);
                    if (month > 12)
                    {
                        month = 1;
                        year = year + 1;

                    }
                    short month_old = Convert.ToInt16(ldtm_loanreceive.Month);
                    DateTime postingdate_old = wcf.Busscom.of_getpostingdate2(state.SsWsPass, Convert.ToInt16(year), month_old);

                    int day = ldtm_loanreceive.DayOfYear - postingdate_old.DayOfYear;
                    if (day == 1)
                    {
                        month = Convert.ToInt16(ldtm_loanreceive.Month + 1);
                        postingdate = wcf.Busscom.of_getpostingdate2(state.SsWsPass, Convert.ToInt16(year), month);
                    }
                    else
                    {
                        month = Convert.ToInt16(processdate.Month + 1);
                        postingdate = wcf.Busscom.of_getpostingdate2(state.SsWsPass, Convert.ToInt16(year), month);
                    }


                }
                else if (ldtm_loanreceive > processdate)
                {

                    ldtm_loanreceive = wcf.Busscom.of_relativeworkdate(state.SsWsPass, ldtm_loanreceive, ai_increase);
                    month = Convert.ToInt16(ldtm_loanreceive.Month + 2);
                    postingdate = wcf.Busscom.of_getpostingdate2(state.SsWsPass, Convert.ToInt16(year), month);

                }
                else
                {
                    month = Convert.ToInt16(ldtm_loanreceive.Month + 1);
                    postingdate = wcf.Busscom.of_getpostingdate2(state.SsWsPass, Convert.ToInt16(year), month);
                }

                dw_main.SetItemDate(1, "loanrcvfix_date", ldtm_loanrequest);
                postingdate = wcf.InterPreter.GetStartkeep(state.SsConnectionIndex, state.SsCoopControl, ldtm_loanrequest);
                dw_main.SetItemDate(1, "startkeep_date", postingdate);
                tDwMain.Eng2ThaiAllRow();

            }
            catch (Exception ex)
            {
                //LtServerMessage.Text = WebUtil.ErrorMessage("JsChangeStartkeep===>" + ex);
            }
        }


        private void JsSetloantype()
        {
            string membtypecode = Hdmembtype_code.Value;
            dw_main.SetItemString(1, "loantype_code", "");
            DwUtil.RetrieveDDDW(dw_main, "loantype_code_1", pbl, null);
            dw_main.SetItemString(1, "membtype_code", membtypecode);
            if (membtypecode == "05")
            {
                DataWindowChild dwloantype_code = dw_main.GetChild("loantype_code_1");
                dwloantype_code.SetFilter("LNLOANTYPE.LOANGROUP_CODE   ='02' and LNLOANMBTYPE.MEMBTYPE_CODE ='" + membtypecode + "' ");
                dwloantype_code.Filter();
                dw_main.SetItemString(1, "loantype_code", loantype);
            }
            else if (membtypecode == "12")
            {
                DataWindowChild dwloantype_code = dw_main.GetChild("loantype_code_1");
                dwloantype_code.SetFilter("LNLOANTYPE.LOANGROUP_CODE   ='02' and LNLOANMBTYPE.MEMBTYPE_CODE ='" + membtypecode + "' ");
                dwloantype_code.Filter();
                dw_main.SetItemString(1, "loantype_code", loantype);
            }
            else if (membtypecode == "13")
            {
                DataWindowChild dwloantype_code = dw_main.GetChild("loantype_code_1");
                dwloantype_code.SetFilter("LNLOANTYPE.LOANGROUP_CODE   ='02' and LNLOANMBTYPE.MEMBTYPE_CODE ='" + membtypecode + "' ");
                dwloantype_code.Filter();
                dw_main.SetItemString(1, "loantype_code", loantype);
            }
            else
            {
                DataWindowChild dwloantype_code = dw_main.GetChild("loantype_code_1");
                dwloantype_code.SetFilter("LNLOANTYPE.LOANGROUP_CODE   ='" + lc_loantype + "' and LNLOANMBTYPE.MEMBTYPE_CODE ='" + membtypecode + "' ");
                dwloantype_code.Filter();
                dw_main.SetItemString(1, "loantype_code", loantype);
            }


        }

        /// <summary>
        /// fillter ประเภทการจ่ายเงิน
        /// </summary>
        private void JsExpenseCode()
        {
            str_itemchange strList = new str_itemchange();
            strList = WebUtil.str_itemchange_session(this);
            string expendCode = "";
            try
            {
                expendCode = dw_main.GetItemString(1, "expense_code");
            }
            catch { }
            if (expendCode == null || expendCode == "") return;

            if ((expendCode == "CHQ") || (expendCode == "TRN") || (expendCode == "CBT") || (expendCode == "DRF") || (expendCode == "TBK"))
            {
                //dw_main.Modify("t_32.visible =0");
                //dw_main.Modify("postagefee_amt.visible =0");
                //dw_main.Modify("t_50.visible =0");
                //dw_main.Modify("tax_amt.visible =0");

                //dw_main.Modify("t_31.visible =0");
                //dw_main.Modify("emsfee_amt.visible =0");
                //dw_main.Modify("t_52.visible =0");
                //dw_main.Modify("onlinefee_amt.visible =0");

                //dw_main.Modify("expense_accid_t.visible =0");
                //dw_main.Modify("compute_12.visible =0");

                //dw_main.Modify("t_44.visible =0");
                //dw_main.Modify("compute_11.visible =0");
                //dw_main.Modify("t_47.visible =0");
                //dw_main.Modify("postagesrv_amt.visible =0");

                //dw_main.Modify("t_53.visible =0");
                //dw_main.Modify("paytoorder_desc.visible =0");
                //dw_main.Modify("paytoorder_desc_1.visible =0");

                //ฝั่งธนาคาร
                dw_main.Modify("t_20.visible =1");
                dw_main.Modify("expense_bank.visible =1");
                dw_main.Modify("t_30.visible =1");
                dw_main.Modify("expense_bank_1.visible =1");

                dw_main.Modify("t_39.visible =1");
                dw_main.Modify("expense_branch.visible =1");
                dw_main.Modify("t_27.visible =1");
                dw_main.Modify("expense_branch_1.visible =1");

                //dw_main.Modify("t_38.visible =1");
                //dw_main.Modify("expense_accid.visible =1");

                //dw_main.Modify("t_40.visible =1");
                //dw_main.Modify("bankfee_amt.visible =1");
                //dw_main.Modify("t_45.visible =1");
                //dw_main.Modify("banksrv_amt.visible =1");

                //dw_main.Modify("t_41.visible =1");
                //dw_main.Modify("compute_10.visible =1");
                try
                {

                    DwUtil.RetrieveDDDW(dw_main, "expense_bank_1", "sl_loan_requestment.pbl", null);

                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            }
            else if ((expendCode == "CSH") || (expendCode == "BEX") || (expendCode == "MON") || (expendCode == "MOS") || (expendCode == "MOO"))
            {
                //dw_main.Modify("t_32.visible =1");
                //dw_main.Modify("postagefee_amt.visible =1");
                //dw_main.Modify("t_50.visible =1");
                //dw_main.Modify("tax_amt.visible =1");

                //dw_main.Modify("t_31.visible =1");
                //dw_main.Modify("emsfee_amt.visible =1");
                //dw_main.Modify("t_52.visible =1");
                //dw_main.Modify("onlinefee_amt.visible =1");

                //dw_main.Modify("expense_accid_t.visible =1");
                //dw_main.Modify("compute_12.visible =1");

                //dw_main.Modify("t_44.visible =1");
                //dw_main.Modify("compute_11.visible =1");
                //dw_main.Modify("t_47.visible =1");
                //dw_main.Modify("postagesrv_amt.visible =1");

                //if (expendCode == "CSH")
                //{
                //    dw_main.Modify("t_53.visible =0");
                //    dw_main.Modify("paytoorder_desc.visible =0");
                //    dw_main.Modify("paytoorder_desc_1.visible =0");
                //}
                //else
                //{
                //    dw_main.Modify("t_53.visible =1");
                //    dw_main.Modify("paytoorder_desc.visible =1");
                //    dw_main.Modify("paytoorder_desc_1.visible =1");
                //}
                //ฝั่งธนาคาร
                dw_main.Modify("t_20.visible =0");
                dw_main.Modify("expense_bank.visible =0");
                dw_main.Modify("t_30.visible =0");
                dw_main.Modify("expense_bank_1.visible =0");

                dw_main.Modify("t_39.visible =0");
                dw_main.Modify("expense_branch.visible =0");
                dw_main.Modify("t_27.visible =0");
                dw_main.Modify("expense_branch_1.visible =0");

                //dw_main.Modify("t_38.visible =0");
                //dw_main.Modify("expense_accid.visible =0");

                //dw_main.Modify("t_40.visible =0");
                //dw_main.Modify("bankfee_amt.visible =0");
                //dw_main.Modify("t_45.visible =0");
                //dw_main.Modify("banksrv_amt.visible =0");

                //dw_main.Modify("t_41.visible =0");
                //dw_main.Modify("compute_10.visible =0");

            }
            if ((expendCode == "MON") || (expendCode == "MOS") || (expendCode == "MOO"))
            {
                if ((strList.xml_main == null) || (strList.xml_main == ""))
                {
                    strList.xml_main = dw_main.Describe("DataWindow.Data.XML");
                    strList.xml_main = shrlonService.ReCalFee(state.SsWsPass, strList.xml_main);
                    //นำเข้าข้อมูลหลัก
                    dw_main.Reset();
                    dw_main.ImportString(strList.xml_main, FileSaveAsType.Xml);
                    //DwUtil.ImportData(strList.xml_main, dw_main, tDwMain, FileSaveAsType.Xml);
                    if (dw_main.RowCount > 1) dw_main.DeleteRow(dw_main.RowCount);
                }
            }
        }


        /// <summary>
        /// fillter สาขาธนาคาร
        /// </summary>
        private void JsExpenseBank()
        {
            try
            {

                DwUtil.RetrieveDDDW(dw_main, "expense_branch_1", "sl_loan_requestment.pbl", null);
                String bankCode;
                try { bankCode = dw_main.GetItemString(1, "expense_bank").Trim(); }
                catch { bankCode = ""; }
                DataWindowChild dwExpenseBranch = dw_main.GetChild("expense_branch_1");

                dwExpenseBranch.SetFilter("CMUCFBANKBRANCH.BANK_CODE='" + bankCode + "'");
                dwExpenseBranch.Filter();
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        private void JsPaycoopid()
        {
            if (Hdcoopid.Value.Length > 1)
            {
                dw_main.SetItemString(1, "paytoorder_desc", Hdcoopid.Value);
                dw_main.SetItemString(1, "paytoorder_desc_1", Hdcoopid.Value);
            }
            else
            {
                int paycoop_id = Convert.ToInt16(Hdcoopid.Value);
                dw_main.SetItemString(1, "paytoorder_desc", "001" + paycoop_id.ToString("000"));
                dw_main.SetItemString(1, "paytoorder_desc_1", "001" + paycoop_id.ToString("000"));
            }
        }

        private void JsObjective()
        {
            int objective_code = Convert.ToInt16(Hdobjective.Value);
            dw_main.SetItemString(1, "loanobjective_code", objective_code.ToString("000"));
            dw_main.SetItemString(1, "loanobjective_code_1", objective_code.ToString("000"));
        }

        /// <summary>
        ///  init ข้อมูลสมาชิก
        /// </summary>
        private void JsGetMemberInfo()
        {
            try
            {
                JsReNewPage();
                LinkButton1.Visible = false;
                LinkButton2.Visible = false;
                CbCheckcoop.Checked = false;
                //  Checkcollloop.Checked = false;
                Decimal ldc_sharestk, ldc_periodshramt, ldc_periodshrvalue, ldc_shrvalue, ldc_loanrequeststatus = 0;
                Decimal ldc_salary = 0, ldc_incomemth = 0, ldc_paymonth;
                Decimal ldc_shrstkvalue = 0;
                int li_shrpaystatus, li_lastperiod = 0, li_membertype;

                DateTime ldtm_birth = new DateTime(), ldtm_member = new DateTime(), ldtm_work = new DateTime(), ldtm_retry = new DateTime();
                string ls_position, ls_remark, ls_membname, ls_membgroup, ls_groupname;
                string ls_membtypedesc = "", ls_controlname = "", ls_membcontrol = "", ls_appltype, ls_memno, ls_CoopControl;
                Decimal lndroploanall_flag = 0;
                String member_no = WebUtil.MemberNoFormat(HdMemberNo.Value);
                dw_main.SetItemString(1, "member_no", member_no);

                ls_CoopControl = state.SsCoopControl; //สาขาที่ทำรายการ

                string as_xmlmessage = "";

                String ls_loantype = "";

                try
                {
                    ls_loantype = dw_main.GetItemString(1, "loantype_code");
                    string lnrequest_date = dw_main.GetItemString(1, "loanrequest_tdate");

                    String entry_year = WebUtil.Right(lnrequest_date, 4);
                    int yyyy = Convert.ToInt32(entry_year) - 543;
                    String entry_day = WebUtil.Left(lnrequest_date, 4);

                    String dd = WebUtil.Left(entry_day, 2);
                    String mm = WebUtil.Right(entry_day, 2);
                    String entry_tt = dd + "/" + mm + "/" + yyyy.ToString();

                    // เช็คว่ามีใบคำขอกู้รออนุมัติอยู่หรือไม่
                    string Message = wcf.InterPreter.Getloanrequest(state.SsConnectionIndex, ls_CoopControl, member_no, entry_tt, ls_loantype);

                    if (Message != "")
                    {
                        if (Message == "1")
                        {
                            LtServerMessage.Text = WebUtil.WarningMessage("มีใบคำขอกู้ของ ท." + member_no + " ได้ผ่านการอนุมัติแล้ว");
                        }
                        else
                        {
                            str_itemchange strList = new str_itemchange();
                            str_requestopen strRequestOpen = new str_requestopen();
                            strList = WebUtil.str_itemchange_session(this);

                            strRequestOpen.memcoop_id = state.SsCoopControl;
                            strRequestOpen.request_no = Message;
                            strRequestOpen.coop_id = state.SsCoopId;
                            strRequestOpen.format_type = "CAT";
                            strRequestOpen.xml_main = dw_main.Describe("DataWindow.Data.XML");
                            strRequestOpen.xml_clear = "";
                            strRequestOpen.xml_guarantee = "";
                            strRequestOpen.xml_insurance = "";
                            strRequestOpen.xml_intspc = "";
                            strRequestOpen.xml_otherclr = "";

                            try
                            {
                                strRequestOpen = shrlonService.of_loanrequestopen(state.SsWsPass, strRequestOpen);

                                //นำข้อมูลเก็บไว้ใน DataWindow
                                dw_main.Reset();
                                dw_main.ImportString(strRequestOpen.xml_main, FileSaveAsType.Xml);

                                if (dw_main.RowCount > 1) dw_main.DeleteRow(dw_main.RowCount);

                                string expendBank = "";
                                try
                                { expendBank = dw_main.GetItemString(1, "expense_bank"); }
                                catch { expendBank = ""; }
                                //ตรวจสอบ ธนาคารว่ามีหรือไม่ 
                                if (expendBank != "")
                                {
                                    // เรียก Method  ShowBranch() สำหรับแสดงสาขาธนาคาร
                                    JsExpenseBank();
                                }
                                tDwMain.Eng2ThaiAllRow();


                                try
                                {
                                    dw_coll.Reset();
                                    dw_coll.ImportString(strRequestOpen.xml_guarantee, FileSaveAsType.Xml);
                                }
                                catch
                                {
                                    dw_coll.Reset();
                                    DwUtil.ImportData(strRequestOpen.xml_guarantee, dw_coll, null, FileSaveAsType.Xml);

                                }
                                try
                                {
                                    dw_clear.Reset();
                                    dw_clear.ImportString(strRequestOpen.xml_clear, FileSaveAsType.Xml);
                                }
                                catch
                                {
                                    dw_clear.Reset();
                                }
                                try
                                {
                                    dw_otherclr.Reset();
                                    dw_otherclr.ImportString(LtXmlOtherlr.Text, FileSaveAsType.Xml);
                                }
                                catch
                                {
                                    dw_otherclr.Reset();
                                }

                                strList.xml_main = strRequestOpen.xml_main;
                                strList.xml_guarantee = strRequestOpen.xml_guarantee;
                                strList.xml_clear = strRequestOpen.xml_clear;
                                strList.xml_otherclr = strRequestOpen.xml_otherclr;

                                Session["strItemchange"] = strList;
                                LtXmlOtherlr.Text = strRequestOpen.xml_otherclr;

                                Decimal loanrequestStatus = dw_main.GetItemDecimal(1, "loanrequest_status");

                            }
                            catch
                            {

                            }

                            LtServerMessage.Text = WebUtil.WarningMessage("มีใบคำขอกู้สำหรับวันที่" + entry_tt + "แล้ว ระบบจะดึงข้อมูลใบคำขอให้อัตโนมัติ");
                        }
                    }
                    else
                    {
                        //เช็คสิทธิ์ ก่อนขอกู้ว่ามีสัญญาเก่าค้างหรือไม่
                        int checkoldloanpayment = wcf.Shrlon.of_checkoldloanpayment(state.SsWsPass, state.SsCoopControl, member_no, ls_loantype, ref as_xmlmessage);

                        if (checkoldloanpayment != 1)   //มีสัญญาเก่าค้าง
                        {
                            if ((as_xmlmessage != "") && (as_xmlmessage != null))
                            {
                                LtServerMessage.Text = WebUtil.WarningMessage(as_xmlmessage);
                            }

                        }
                        else    //ไม่มีสัญญาเก่าค้าง
                        {
                            String ls_memcoopid;
                            if (HdMemcoopId.Value == "")
                            {
                                ls_memcoopid = state.SsCoopControl;
                            }
                            else
                            {
                                ls_memcoopid = HdMemcoopId.Value;
                            }

                            String sqlstr = @"   SELECT a.membgroup_control,
                                                b.membgroup_desc as control_desc,
                                                a.membgroup_code,
                                                a.membgroup_desc , 
                                             MBMEMBMASTER.BIRTH_DATE,   
                                             MBMEMBMASTER.MEMBER_DATE,   
                                             MBMEMBMASTER.WORK_DATE,   
                                             MBMEMBMASTER.RETRY_DATE,   
                                             MBMEMBMASTER.SALARY_AMOUNT,   
                                             MBMEMBMASTER.MEMBTYPE_CODE,   
                                             MBUCFMEMBTYPE.MEMBTYPE_DESC,   
                                             SHSHAREMASTER.LAST_PERIOD,   
                                             SHSHAREMASTER.PERIODSHARE_AMT,   
                                             mbucfprename.prename_desc||mbmembmaster.memb_name||'   '||mbmembmaster.memb_surname as member_name,   
                                             SHSHAREMASTER.PAYMENT_STATUS,   
                                             MBMEMBMASTER.POSITION_DESC,   
                                             MBMEMBMASTER.POSITION_CODE,   
                                             MBMEMBMASTER.REMARK,   
                                             MBMEMBMASTER.MEMBER_STATUS,   
                                             MBMEMBMASTER.RESIGN_STATUS,   
                                             SHSHAREMASTER.SHARESTK_AMT,   
                                             SHSHARETYPE.UNITSHARE_VALUE,   
                                             MBMEMBMASTER.MEMBER_TYPE,   
                                             MBMEMBMASTER.APPLTYPE_CODE,   MBMEMBMASTER.LNDROPGRANTEE_FLAG,
                                             MBMEMBMASTER.RETRY_STATUS,      MBMEMBMASTER.LNDROPLOANALL_FLAG, 
                                             MBMEMBMASTER.MEMBER_NO,   
                                             MBMEMBMASTER.PAUSEKEEP_FLAG,   
                                             MBMEMBMASTER.PAUSEKEEP_DATE,       
                                             MBMEMBMASTER.COOP_ID  ,MBMEMBMASTER.CREMATION_STATUS
                               FROM MBMEMBMASTER,   
                                     MBUCFMEMBGROUP a ,   MBUCFMEMBGROUP b,
                                     MBUCFPRENAME,
                                     MBUCFMEMBTYPE,
                                     SHSHAREMASTER,   
                                     SHSHARETYPE  
                               WHERE ( a.MEMBGROUP_CODE = MBMEMBMASTER.MEMBGROUP_CODE ) and  a.membgroup_control = b.membgroup_code(+) and
                                        a.coop_id = b.coop_id(+) and 
                                    ( SHSHAREMASTER.MEMBER_NO = MBMEMBMASTER.MEMBER_NO ) and    
                                     ( MBMEMBMASTER.PRENAME_CODE = MBUCFPRENAME.PRENAME_CODE ) and
                                     ( MBMEMBMASTER.MEMBTYPE_CODE = MBUCFMEMBTYPE.MEMBTYPE_CODE ) and  
                                     ( SHSHAREMASTER.SHARETYPE_CODE = SHSHARETYPE.SHARETYPE_CODE ) and  
                                     ( MBMEMBMASTER.COOP_ID = a.COOP_ID ) and     ( MBMEMBMASTER.MEMBER_STATUS=1) AND
                                     ( MBMEMBMASTER.COOP_ID = SHSHAREMASTER.COOP_ID ) and  
                                     ( SHSHAREMASTER.COOP_ID = SHSHARETYPE.COOP_ID ) and  
                                     ( ( mbmembmaster.member_no = '" + member_no + @"' ) AND  
                                     ( shsharemaster.sharetype_code = '01' ) )    and  
                                     MBMEMBMASTER.COOP_ID   ='" + ls_memcoopid + @"' ";
                            Sdt dt = WebUtil.QuerySdt(sqlstr);//เป็น service

                            while (dt.Next())
                            {
                                try
                                {
                                    li_cramationstatus = dt.GetInt32("cremation_status");
                                }
                                catch { li_cramationstatus = 0; }

                                try
                                {
                                    lndroploanall_flag = dt.GetDecimal("LNDROPLOANALL_FLAG");
                                }
                                catch { lndroploanall_flag = 0; }
                                ls_membname = dt.GetString("member_name");
                                ls_membcontrol = dt.GetString("membgroup_control");
                                ls_controlname = dt.GetString("control_desc");
                                ls_membgroup = dt.GetString("membgroup_code");
                                ls_groupname = dt.GetString("membgroup_desc");
                                ldc_salary = dt.GetDecimal("salary_amount");
                                li_lastperiod = dt.GetInt32("last_period");

                                try
                                {
                                    ldtm_birth = dt.GetDate("birth_date");
                                }
                                catch { }
                                try
                                {
                                    ///<หาวันที่เกษียณ>
                                    ldtm_retry = wcf.InterPreter.CalReTryDate(state.SsConnectionIndex, state.SsCoopControl, ldtm_birth);
                                }
                                catch { }
                                try
                                {
                                    ldtm_member = dt.GetDate("member_date");
                                }
                                catch { }
                                try
                                {
                                    ldtm_work = dt.GetDate("work_date");
                                }
                                catch { }

                                ldc_incomemth = 0;
                                ldc_paymonth = 0;
                                ls_position = dt.GetString("position_desc");
                                ls_remark = dt.GetString("remark");
                                ldc_shrvalue = dt.GetDecimal("unitshare_value");
                                ldc_sharestk = dt.GetDecimal("sharestk_amt");
                                ldc_periodshramt = dt.GetDecimal("periodshare_amt");
                                li_shrpaystatus = dt.GetInt32("payment_status");
                                li_membertype = dt.GetInt32("member_type");
                                ls_appltype = dt.GetString("appltype_code");
                                ls_memno = dt.GetString("member_no");
                                ls_memcoopid = dt.GetString("coop_id");

                                ls_membtype = dt.GetString("membtype_code");
                                ls_membtypedesc = dt.GetString("membtype_desc");
                                ldc_shrstkvalue = Convert.ToDecimal((Convert.ToInt32(ldc_shrvalue) * Convert.ToInt32(ldc_sharestk)));
                                ldc_periodshrvalue = Convert.ToDecimal((Convert.ToInt32(ldc_periodshramt) * Convert.ToInt32(ldc_shrvalue)));



                                dw_main.SetItemString(1, "memcoop_id", ls_memcoopid);
                                dw_main.SetItemString(1, "coop_id", state.SsCoopId);
                                dw_main.SetItemString(1, "member_name", ls_membname);
                                dw_main.SetItemString(1, "mbucfmembgroup_membgroup_code", ls_membgroup);
                                dw_main.SetItemString(1, "mbucfmembgroup_membgroup_desc", ls_groupname);
                                dw_main.SetItemString(1, "membgroup_desc", ls_controlname);
                                dw_main.SetItemDecimal(1, "salary_amt", ldc_salary);
                                dw_main.SetItemDecimal(1, "share_lastperiod", li_lastperiod);
                                dw_main.SetItemDateTime(1, "birth_date", ldtm_birth);
                                dw_main.SetItemDateTime(1, "member_date", ldtm_member);
                                dw_main.SetItemString(1, "membtype_code", ls_membtype);
                                //dw_main.SetItemString(1, "membtype_desc", ls_membtypedesc);
                                try
                                {
                                    dw_main.SetItemDateTime(1, "retry_date", ldtm_retry);
                                }
                                catch { dw_main.SetItemDateTime(1, "retry_date", DateTime.Now); }
                                dw_main.SetItemDecimal(1, "incomemonth_other", ldc_incomemth);
                                dw_main.SetItemDecimal(1, "paymonth_other", ldc_paymonth);
                                dw_main.SetItemString(1, "position_desc", ls_position);
                                dw_main.SetItemString(1, "remark", "");
                                dw_main.SetItemString(1, "member_remark", ls_remark);

                                dw_main.SetItemDecimal(1, "sharestk_value", ldc_shrstkvalue);
                                dw_main.SetItemDecimal(1, "periodshare_value", ldc_periodshrvalue);
                                dw_main.SetItemDecimal(1, "sharepay_status", li_shrpaystatus);
                                dw_main.SetItemDecimal(1, "intestimate_amt", 0);
                                dw_main.SetItemDecimal(1, "member_type", li_membertype);
                                dw_main.SetItemString(1, "appltype_code", ls_appltype);

                                try
                                {
                                    ///<หาอายุสมาชิก>
                                    Decimal birth_age = BusscomService.of_cal_yearmonth(state.SsWsPass, ldtm_birth, DateTime.Now);
                                    dw_main.SetItemDecimal(1, "birth_age", birth_age);
                                }
                                catch
                                {

                                    Decimal birth_age = BusscomService.of_cal_yearmonth(state.SsWsPass, DateTime.Now, DateTime.Now);
                                    dw_main.SetItemDecimal(1, "birth_age", birth_age);
                                }
                                try
                                {
                                    ///<หาเกษียณอายุ>
                                    Decimal retry_age = BusscomService.of_cal_yearmonth(state.SsWsPass, DateTime.Now, ldtm_retry);
                                    String retry_agel = WebUtil.Left(retry_age.ToString("000.00"), 3);
                                    String retry_ager = WebUtil.Right(retry_age.ToString(""), 2);
                                    int retryagel = Convert.ToInt32(retry_agel) * 12;
                                    Decimal retryager = (Math.Round(Convert.ToDecimal(retry_ager)) / 10) * 10;
                                    dw_main.SetItemDecimal(1, "retry_age", retryagel + retryager);
                                }
                                catch
                                {
                                    Decimal retry_age = BusscomService.of_cal_yearmonth(state.SsWsPass, DateTime.Now, DateTime.Now);
                                    String retry_agel = WebUtil.Left(retry_age.ToString("000.00"), 3);
                                    String retry_ager = WebUtil.Right(retry_age.ToString(""), 2);
                                    int retryagel = Convert.ToInt32(retry_agel) * 12;
                                    Decimal retryager = (Math.Round(Convert.ToDecimal(retry_ager)) / 10) * 10;
                                    dw_main.SetItemDecimal(1, "retry_age", retryagel + retryager);
                                }
                                try
                                {
                                    ///<หาอายุการเป็นสมาชิก>
                                    Decimal member_age = BusscomService.of_cal_yearmonth(state.SsWsPass, ldtm_member, DateTime.Now);
                                    dw_main.SetItemDecimal(1, "member_age", member_age);
                                }
                                catch
                                {
                                    Decimal member_age = BusscomService.of_cal_yearmonth(state.SsWsPass, DateTime.Now, DateTime.Now);
                                    dw_main.SetItemDecimal(1, "member_age", member_age);
                                }
                            }


                            //DateTime adtm_reqdate = dw_main.GetItemDateTime(1, "loanrequest_date");
                            //object[] Setloantypeinfo = shrlonService.Setloantypeinfo(state.SsWsPass, ls_memcoopid, ls_loantype, adtm_reqdate);
                            //  dw_main.SetItemDecimal(1, "loanpayment_type", 1);
                            //dw_main.SetItemDecimal(1, "minsalary_perc", Convert.ToDecimal(Setloantypeinfo[1]));
                            //dw_main.SetItemDecimal(1, "minsalary_amt", Convert.ToDecimal(Setloantypeinfo[2]));
                            //dw_main.SetItemDecimal(1, "minsalary_inc", Convert.ToDecimal(Setloantypeinfo[3]));
                            //   dw_main.SetItemDecimal(1, "int_intsteptype", 1);
                            //dw_main.SetItemDecimal(1, "contract_time", Convert.ToDecimal(Setloantypeinfo[5]));
                            //dw_main.SetItemDecimal(1, "od_flag", Convert.ToDecimal(Setloantypeinfo[6]));
                            //dw_main.SetItemString(1, "loanobjective_code", "007");//Setloantypeinfo[7].ToString()
                            //dw_main.SetItemDecimal(1, "loanrcvfix_flag", Convert.ToDecimal(Setloantypeinfo[8]));
                            ////dw_main.SetItemDate(1, "loanrcvfix_date", Convert.ToDateTime(Setloantypeinfo[9]));
                            ////dw_main.SetItemDate(1, "startkeep_date", Convert.ToDateTime(Setloantypeinfo[10]));
                            //dw_main.SetItemDecimal(1, "loanrcvperiod_month", Convert.ToDecimal(Setloantypeinfo[11]));
                            //dw_main.SetItemDecimal(1, "loanrcvperiod_year", Convert.ToDecimal(Setloantypeinfo[12]));
                            //dw_main.SetItemDecimal(1, "apvimmediate_flag", Convert.ToDecimal(Setloantypeinfo[13]));
                            //dw_main.SetItemDecimal(1, "loanrequest_status", Convert.ToDecimal(Setloantypeinfo[14]));
                            //dw_main.SetItemDecimal(1, "clearinsure_flag", Convert.ToDecimal(Setloantypeinfo[15]));
                            //dw_main.SetItemDecimal(1, "int_continttype", 2);
                            //dw_main.SetItemDecimal(1, "int_contintrate", new Decimal(0.055));
                            //dw_main.SetItemString(1, "int_continttabcode", "INT01");
                            //dw_main.SetItemDecimal(1, "int_contintincrease", 0);

                            string sqllntype = @"select  loanpermgrp_code,  contint_type,  contractint_rate, inttabrate_code,salperct_balance,salamt_balance,loanpayment_type
                                        from lnloantype  where loantype_code ='" + loantype + @"' ";
                            Sdt dtlntype = WebUtil.QuerySdt(sqllntype);
                            string intcontinttabcode = "INT01";
                            double intcontintrate = 0.055;
                            decimal intcontinttype = 2;
                            decimal ldc_minpercsal = 0;
                            string loanpermissgroup = "01";
                            decimal ldc_minsalaamt = 0, ldc_paymenttype = 1;
                            if (dtlntype.Next())
                            {

                                intcontinttabcode = dtlntype.GetString("inttabrate_code");
                                intcontinttype = dtlntype.GetDecimal("contint_type");
                                ldc_minpercsal = dtlntype.GetDecimal("salperct_balance");
                                ldc_minsalaamt = dtlntype.GetDecimal("salamt_balance");
                                ldc_paymenttype = dtlntype.GetDecimal("loanpayment_type");
                                loanpermissgroup = dtlntype.GetString("loanpermgrp_code");
                            }
                            string sqlint = @"select interest_rate from lncfloanintratedet where loanintrate_code =(select inttabrate_code   from lnloantype  where loantype_code = '" + loantype + @"') ";
                            Sdt dtint = WebUtil.QuerySdt(sqlint);
                            if (dtint.Next())
                            {

                                intcontintrate = dtint.GetDouble("interest_rate");
                            }
                            dw_main.SetItemDecimal(1, "int_continttype", intcontinttype);
                            dw_main.SetItemDouble(1, "int_contintrate", intcontintrate);
                            dw_main.SetItemString(1, "int_continttabcode", intcontinttabcode);
                            dw_main.SetItemDecimal(1, "minsalary_perc", ldc_minpercsal);
                            dw_main.SetItemDecimal(1, "minsalary_amt", ldc_minsalaamt);
                            dw_main.SetItemDecimal(1, "loanpayment_type", ldc_paymenttype);
                            dw_main.SetItemString(1, "instype_code", loanpermissgroup);



                            SetAcci_dept();//เซ็ค เลขที่บัญชี


                            if (lndroploanall_flag != 0)
                            {

                                LtServerMessage.Text = WebUtil.ErrorMessage("สมาชิกท่านนี้ งดกู้");
                            }
                            else
                            {
                                ///<สิทธิ์กู้สูดสุง>
                                ///
                                string re_lastperiod = wcf.InterPreter.GetlastperiodShare(li_lastperiod);
                                if (re_lastperiod == "1")
                                {
                                    Jsmaxcreditperiod();
                                }
                                else { LtServerMessage.Text = WebUtil.ErrorMessage("งวดการส่งค่าหุ้นของสมาชิกท่านนี้ยังไม่ถึง 4 งวด"); }
                                ///<ตรวจหาสัญญาที่จะหักกลบ ถ้ามีก็เอามาแสดง ใน dw_clear>
                                Genbaseloanclear();
                                JsSetMonthpayCoop();
                            }


                        } //end เช็คสิทธิ์ ก่อนขอกู้ว่ามีสัญญาเก่าค้างหรือไม่

                        DwUtil.RetrieveDDDW(dw_main, "loantype_code_1", pbl, null);
                        DataWindowChild dwloantype_code = dw_main.GetChild("loantype_code_1");
                        //dwloantype_code.SetFilter("MEMBTYPE_CODE ='" + loanmbgroup_code + "' and LNLOANTYPE.LOANGROUP_CODE   ='02'");
                        //dwloantype_code.Filter();

                        dw_main.SetItemString(1, "loantype_code", ls_loantype);
                        dw_main.SetItemString(1, "loantype_code_1", ls_loantype);
                    }


                }

                catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาเลือกประเภทเงินกู้ที่จะทำรายการ" + ex); }



            }
            catch (Exception ex)
            {
                // LtServerMessage.Text = WebUtil.ErrorMessage("JsGetMemberInfo===>" + ex);

            }

        }

        /// <summary>
        /// ดึงข้อมูลสัญญาหักกลบ
        /// </summary>
        private void Genbaseloanclear()
        {

            String member_no = dw_main.GetItemString(1, "member_no");
            DateTime loanrcvfix_date = dw_main.GetItemDate(1, "loanrcvfix_date");
            String ls_memcoopid;
            try { ls_memcoopid = dw_main.GetItemString(1, "memcoop_id"); }
            catch { ls_memcoopid = state.SsCoopControl; }

            string ls_contno, ls_conttype, ls_prefix, ls_permgrp, ls_loantype = "", ls_intcontintcode, ls_coopid = "";
            Decimal li_count, li_index, li_row, li_countold, li_minperiod = 0, li_period, li_continttype, li_transfersts = 0;
            Decimal li_paytype, li_status = 1, li_clearflag, li_contstatus, li_intcontinttype, li_intsteptype;
            Decimal li_checkcontclr = 0, li_periodamt, li_contlaw, li_paystatus, li_clearinsure, li_countpayflag = 0;
            Decimal ldc_appvamt, ldc_balance = 0, ldc_withdrawable, ldc_rkeepprin, ldc_rkeepint, ldc_transbal;
            Decimal ldc_intarrear, ldc_payment, ldc_intestim;
            Decimal ldc_minpay = 0, ldc_intrate, ldc_intcontintrate, ldc_intincrease;
            DateTime ldtm_lastcalint, ldtm_lastproc, ldtm_today, ldtm_approve, ldtm_startcont, ldtm_calintfrom;

            String sqlStr1 = @"SELECT LNLOANTYPECLR.LOANTYPE_CODE,   
                                 LNLOANTYPECLR.LOANTYPE_CLEAR,   
                                 LNLOANTYPECLR.MINPERIOD_PAY,   
                                 LNLOANTYPECLR.MINPERCENT_PAY,   
                                 LNLOANTYPECLR.CHKCONTCREDIT_FLAG,   
                                 LNLOANTYPE.LOANRIGHT_TYPE,   
                                 LNLOANTYPECLR.CONTRACT_STATUS  
                            FROM LNLOANTYPECLR,   
                                 LNLOANTYPE  
                           WHERE ( LNLOANTYPECLR.LOANTYPE_CLEAR = LNLOANTYPE.LOANTYPE_CODE ) and  
                                 ( LNLOANTYPECLR.COOP_ID = LNLOANTYPE.COOP_ID ) and  
                                 ( ( LNLOANTYPECLR.COOP_ID = '" + ls_memcoopid + "' ) )    ";

            Sdt dt1 = WebUtil.QuerySdt(sqlStr1);
            if (dt1.Next())
            {
                for (int i = 0; i < dt1.GetRowCount(); i++)
                {
                    li_minperiod = Convert.ToDecimal(dt1.Rows[i]["minperiod_pay"]);
                    ldc_minpay = Convert.ToDecimal(dt1.Rows[i]["minpercent_pay"]);
                    li_checkcontclr = Convert.ToDecimal(dt1.Rows[i]["contract_status"]);
                }
            }

            String sqlStr = @"   SELECT LNCONTMASTER.LOANCONTRACT_NO,   
                                             LNCONTMASTER.MEMBER_NO,   
                                             LNCONTMASTER.LOANTYPE_CODE,   
                                             LNCONTMASTER.LOANAPPROVE_AMT,   
                                             LNCONTMASTER.WITHDRAWABLE_AMT,   
                                             LNCONTMASTER.PRINCIPAL_BALANCE,   
                                             LNCONTMASTER.LAST_PERIODPAY,   
                                             LNCONTMASTER.LASTCALINT_DATE,   
                                             LNCONTMASTER.LASTPROCESS_DATE,   
                                             LNCONTMASTER.INTEREST_ARREAR,   
                                             LNCONTMASTER.RKEEP_PRINCIPAL,   
                                             LNCONTMASTER.RKEEP_INTEREST,   
                                             LNLOANTYPE.PREFIX,   
                                             LNCONTMASTER.LOANPAYMENT_TYPE,   
                                             LNCONTMASTER.PERIOD_PAYMENT,   
                                             LNLOANTYPE.LOANPERMGRP_CODE,   
                                             LNCONTMASTER.CONTRACT_STATUS,   
                                             LNCONTMASTER.CONTRACTINT_TYPE,   
                                             LNCONTMASTER.CONTRACT_INTEREST,   
                                             LNCONTMASTER.LOANAPPROVE_DATE,   
                                             LNCONTMASTER.STARTCONT_DATE,   
                                             LNCONTMASTER.INT_CONTINTTYPE,   
                                             LNCONTMASTER.INT_CONTINTRATE,   
                                             LNCONTMASTER.INT_CONTINTTABCODE,   
                                             LNCONTMASTER.INT_CONTINTINCREASE,   
                                             LNCONTMASTER.INT_INTSTEPTYPE,   
                                             LNCONTMASTER.PERIOD_PAYAMT,   
                                             LNCONTMASTER.CONTLAW_STATUS,   
                                             LNCONTMASTER.PRINCIPAL_TRANSBAL,   
                                             LNCONTMASTER.PAYMENT_STATUS,   
                                             LNLOANTYPE.CLEARINSURE_FLAG,   
                                             LNCONTMASTER.INSURECOLL_FLAG  
                                        FROM LNCONTMASTER,   
                                             LNLOANTYPE  
                                       WHERE ( LNCONTMASTER.LOANTYPE_CODE = LNLOANTYPE.LOANTYPE_CODE ) and  
                                             ( LNCONTMASTER.COOP_ID = LNLOANTYPE.COOP_ID ) and  
                                             ( ( lncontmaster.member_no = '" + member_no + @"' ) AND  
                                             ( lncontmaster.principal_balance + lncontmaster.withdrawable_amt > 0 ) AND  
                                             ( lncontmaster.contract_status > 0 ) AND  
                                             ( LNCONTMASTER.MEMCOOP_ID = '" + ls_memcoopid + @"' ) )   
                                    ORDER BY LNCONTMASTER.LOANCONTRACT_NO ASC  ";
            Sdt dt = WebUtil.QuerySdt(sqlStr);
            if (dt.Next())
            {
                dw_clear.Reset();

                int rowCount = dt.GetRowCount();
                for (int i = 0; i < rowCount; i++)
                {
                    try { ls_contno = dt.Rows[i]["loancontract_no"].ToString(); }
                    catch { ls_contno = ""; }
                    try
                    {
                        ls_conttype = dt.Rows[i]["loantype_code"].ToString();
                    }
                    catch { ls_conttype = ""; }
                    try
                    {
                        ls_prefix = dt.Rows[i]["prefix"].ToString();
                    }
                    catch { ls_prefix = ""; }
                    try
                    {// คำนำหน้าประเภทสัญญา
                        ls_permgrp = dt.Rows[i]["loanpermgrp_code"].ToString();
                    }
                    catch { ls_permgrp = ""; }
                    try
                    {// กลุ่มวงเงินกู้
                        li_paytype = Convert.ToDecimal(dt.Rows[i]["loanpayment_type"]);
                    }
                    catch { li_paytype = 0; }
                    try
                    {
                        li_period = Convert.ToDecimal(dt.Rows[i]["last_periodpay"]);
                    }
                    catch { li_period = 0; }
                    try
                    {
                        li_contstatus = Convert.ToDecimal(dt.Rows[i]["contract_status"]);
                    }
                    catch { li_contstatus = 0; }
                    try
                    {
                        li_continttype = Convert.ToDecimal(dt.Rows[i]["contractint_type"]);

                    }
                    catch { li_continttype = 0; }
                    try
                    {
                        ldc_intrate = Convert.ToDecimal(dt.Rows[i]["contract_interest"]);
                    }
                    catch { ldc_intrate = 0; }
                    try
                    {
                        ldc_payment = Convert.ToDecimal(dt.Rows[i]["period_payment"]);
                    }
                    catch { ldc_payment = 0; }
                    try
                    {
                        ldc_appvamt = Convert.ToDecimal(dt.Rows[i]["loanapprove_amt"]);
                    }
                    catch { ldc_appvamt = 0; }
                    try
                    {
                        ldc_withdrawable = Convert.ToDecimal(dt.Rows[i]["withdrawable_amt"]);
                    }
                    catch { ldc_withdrawable = 0; }
                    try
                    {
                        ldc_balance = Convert.ToDecimal(dt.Rows[i]["principal_balance"]);
                    }
                    catch { ldc_balance = 0; }
                    try
                    {
                        ldc_intarrear = Convert.ToDecimal(dt.Rows[i]["interest_arrear"]);
                    }
                    catch { ldc_intarrear = 0; }
                    try
                    {
                        ldc_rkeepprin = Convert.ToDecimal(dt.Rows[i]["rkeep_principal"]);
                    }
                    catch { ldc_rkeepprin = 0; }
                    try
                    {
                        ldc_rkeepint = Convert.ToDecimal(dt.Rows[i]["rkeep_interest"]);
                    }
                    catch { ldc_rkeepint = 0; }
                    try
                    {
                        ldtm_lastcalint = Convert.ToDateTime(dt.Rows[i]["lastcalint_date"]);
                    }
                    catch { ldtm_lastcalint = DateTime.Now; }
                    try
                    {
                        ldtm_lastproc = Convert.ToDateTime(dt.Rows[i]["lastprocess_date"]);
                    }
                    catch { ldtm_lastproc = DateTime.Now; }
                    try
                    {
                        ldtm_approve = Convert.ToDateTime(dt.Rows[i]["loanapprove_date"]);
                    }
                    catch { ldtm_approve = DateTime.Now; }
                    try
                    {
                        ldtm_startcont = Convert.ToDateTime(dt.Rows[i]["startcont_date"]);
                    }
                    catch { ldtm_startcont = DateTime.Now; }
                    try
                    {
                        li_intcontinttype = Convert.ToDecimal(dt.Rows[i]["int_continttype"]);
                    }
                    catch { li_intcontinttype = 0; }
                    try
                    {
                        ldc_intcontintrate = Convert.ToDecimal(dt.Rows[i]["int_contintrate"]);
                    }
                    catch { ldc_intcontintrate = 0; }
                    try
                    {
                        ls_intcontintcode = dt.Rows[i]["int_continttabcode"].ToString();
                    }
                    catch { ls_intcontintcode = ""; }
                    try
                    {
                        ldc_intincrease = Convert.ToDecimal(dt.Rows[i]["int_contintincrease"]);
                    }
                    catch { ldc_intincrease = 0; }
                    try
                    {
                        li_intsteptype = Convert.ToDecimal(dt.Rows[i]["int_intsteptype"]);
                    }
                    catch { li_intsteptype = 0; }
                    try
                    {
                        li_periodamt = Convert.ToDecimal(dt.Rows[i]["period_payamt"]);
                    }
                    catch { li_periodamt = 0; }
                    try
                    {
                        li_transfersts = Convert.ToDecimal(dt.Rows[i]["transfer_status"]);
                    }
                    catch { li_transfersts = 0; }
                    try
                    {
                        ls_coopid = dt.Rows[i]["coop_id"].ToString();
                    }
                    catch { ls_coopid = ls_memcoopid; }
                    try
                    {

                        li_contlaw = Convert.ToDecimal(dt.Rows[i]["contlaw_status"]);
                    }
                    catch { li_contlaw = 0; }
                    try
                    {
                        ldc_transbal = Convert.ToDecimal(dt.Rows[i]["principal_transbal"]);
                    }
                    catch { ldc_transbal = 0; }
                    try
                    {
                        li_paystatus = Convert.ToDecimal(dt.Rows[i]["payment_status"]);
                    }
                    catch { li_paystatus = 0; }
                    try
                    {
                        li_clearinsure = Convert.ToDecimal(dt.Rows[i]["insurecoll_flag"]);
                    }
                    catch { li_clearinsure = 0; }
                    try
                    {
                        ldc_intcontintrate = dw_main.GetItemDecimal(1, "int_contintrate");
                        ls_loantype = dw_main.GetItemString(1, "loantype_code");
                        DateTime processdate = new DateTime(); int day_amount = 0;
                        //a hardcode
                        String sqlpro = " SELECT MAX(LASTPROCESS_DATE)as LASTPROCESS_DATE FROM  LNCONTMASTER ";
                        Sdt dtpro = WebUtil.QuerySdt(sqlpro);
                        if (dtpro.Next()) { processdate = dtpro.GetDate("LASTPROCESS_DATE"); }
                        short year = Convert.ToInt16(loanrcvfix_date.Year + 543);
                        short month_bf = Convert.ToInt16(loanrcvfix_date.Month - 1);
                        short month_af = Convert.ToInt16(loanrcvfix_date.Month);
                        DateTime postingdate_bf = wcf.Busscom.of_getpostingdate2(state.SsWsPass, year, month_bf);
                        DateTime postingdate_af = wcf.Busscom.of_getpostingdate2(state.SsWsPass, year, month_af);
                        if (postingdate_bf < postingdate_af)
                        {

                            day_amount = postingdate_af.DayOfYear - postingdate_bf.DayOfYear;

                        }
                        if (ls_conttype != ls_loantype)
                        {
                            ldc_intestim = wcf.Shrlon.of_roundmoney(state.SsWsPass, ldc_balance * (ldc_intcontintrate) * day_amount / 365);

                        }
                        else
                        {

                            ldc_intestim = wcf.Shrlon.of_roundmoney(state.SsWsPass, (wcf.Shrlon.of_computeinterest(state.SsWsPass, ls_memcoopid, ls_contno, loanrcvfix_date)));
                        }
                    }
                    catch { ldc_intestim = 0; }
                    //if (ls_conttype == dw_main.GetItemString(1, "loantype_code") || ls_conttype == "20" || ls_conttype == "21" || ls_conttype == "10" || ls_conttype == "11" || ls_conttype == "12")
                    //{

                    //    li_status = 1;


                    //}
                    //else { li_status = 0; }

                    try
                    {
                        dw_clear.InsertRow(i + 1);
                        dw_clear.SetItemString(i + 1, "loancontract_no", ls_contno);
                        dw_clear.SetItemString(i + 1, "coop_id", state.SsCoopId);
                        dw_clear.SetItemString(i + 1, "concoop_id", state.SsCoopControl);
                        dw_clear.SetItemString(i + 1, "loantype_code", ls_conttype);
                        dw_clear.SetItemString(i + 1, "prefix", ls_prefix);
                        dw_clear.SetItemDecimal(i + 1, "loanpayment_type", li_paytype);
                        dw_clear.SetItemDecimal(i + 1, "period_payment", ldc_payment);
                        dw_clear.SetItemDecimal(i + 1, "loanapprove_amt", ldc_appvamt);
                        dw_clear.SetItemDecimal(i + 1, "withdrawable_amt", ldc_withdrawable);
                        dw_clear.SetItemDecimal(i + 1, "principal_balance", ldc_balance);
                        dw_clear.SetItemDecimal(i + 1, "last_periodpay", li_period);
                        dw_clear.SetItemDecimal(i + 1, "minperiod_pay", li_minperiod);
                        dw_clear.SetItemDecimal(i + 1, "minpercent_pay", ldc_minpay);
                        dw_clear.SetItemDateTime(i + 1, "lastcalint_date", ldtm_lastcalint);
                        dw_clear.SetItemDecimal(i + 1, "contract_status", li_contstatus);
                        dw_clear.SetItemString(i + 1, "permissgroup_code", ls_permgrp);
                        // dw_clear.SetItemDecimal(i + 1, "clear_status", li_status);
                        dw_clear.SetItemDateTime(i + 1, "lastprocess_date", ldtm_lastproc);
                        dw_clear.SetItemDecimal(i + 1, "contractint_type", li_continttype);
                        dw_clear.SetItemDecimal(i + 1, "contract_interest", ldc_intrate);
                        dw_clear.SetItemDecimal(i + 1, "rkeep_principal", ldc_rkeepprin);
                        dw_clear.SetItemDecimal(i + 1, "rkeep_interest", ldc_rkeepint);
                        dw_clear.SetItemDecimal(i + 1, "interest_arrear", ldc_intarrear);//ldc_intarrear
                        if (li_paytype == 1)
                        {
                            dw_clear.SetItemDecimal(i + 1, "intestimate_amt", Convert.ToDecimal(ldc_intestim));
                        }
                        else
                        {
                            dw_clear.SetItemDecimal(i + 1, "intestimate_amt", 0);
                        }
                        dw_clear.SetItemDateTime(i + 1, "loanapprove_date", ldtm_approve);
                        dw_clear.SetItemDateTime(i + 1, "startcont_date", ldtm_startcont);
                        dw_clear.SetItemDecimal(i + 1, "int_continttype", li_intcontinttype);
                        dw_clear.SetItemDecimal(i + 1, "int_contintrate", ldc_intcontintrate);
                        dw_clear.SetItemString(i + 1, "int_continttabcode", ls_intcontintcode);
                        dw_clear.SetItemDecimal(i + 1, "int_contintincrease", ldc_intincrease);
                        dw_clear.SetItemDecimal(i + 1, "int_intsteptype", li_intsteptype);
                        dw_clear.SetItemDecimal(i + 1, "period_payamt", li_periodamt);
                        dw_clear.SetItemDecimal(i + 1, "contlaw_status", li_contlaw);
                        dw_clear.SetItemDecimal(i + 1, "payment_status", li_paystatus);
                        dw_clear.SetItemDecimal(i + 1, "principal_transbal", ldc_transbal);
                        dw_clear.SetItemDecimal(i + 1, "insurecoll_flag", li_clearinsure);
                        dw_clear.SetItemDecimal(i + 1, "countpay_flag", li_countpayflag);
                        //  dw_clear.SetItemDecimal(i+1, "transfer_status", li_transfersts);
                        //  dw_clear.SetItemDecimal(i+1, "prnclear_amt", ldc_balance);



                        //                        Decimal minperiod_pay = 0;
                        //                        Decimal period_payamt = dw_clear.GetItemDecimal(i + 1, "last_periodpay");
                        //                        string sqlcheckperiod = @"select minperiod_pay,minpercent_pay from lnloantypecheckperiod
                        //                                           where loantype_code ='" + ls_loantype + @"' and loantype_clear ='" + ls_conttype + "'";
                        //                        Sdt dtcheckperiod = WebUtil.QuerySdt(sqlcheckperiod);
                        //                        if (dtcheckperiod.Next()) { minperiod_pay = dtcheckperiod.GetDecimal("minperiod_pay"); }
                        //                        if (period_payamt < minperiod_pay)
                        //                        {
                        //                            dw_main.SetItemDecimal(1, "loancredit_amt", 0);
                        //                            LtServerMessage.Text = WebUtil.ErrorMessage("ท." + member_no + " ยังไม่ทำการชำระหนี้งวดแรกไม่สามารถขอกู้ใหม่ได้");

                        //                        }

                        Decimal ldc_calintflag = 0;
                        string sqlclr = @"select minperiod_pay,minpercent_pay ,loantype_clear,calint_flag from lnloantypeclr
                                           where loantype_code ='" + ls_loantype + @"' and loantype_clear ='" + ls_conttype + "'";
                        Sdt dtclr = WebUtil.QuerySdt(sqlclr);
                        if (dtclr.Next())
                        {
                            ldc_calintflag = dtclr.GetDecimal("calint_flag");
                            Decimal intestimate_amt = 0;//= wcf.Shrlon.of_roundmoney(state.SsWsPass, (wcf.Shrlon.of_computeinterest(state.SsWsPass, ls_memcoopid, ls_contno, loanrcvfix_date)));

                            if (li_paytype == 1)
                            {
                                intestimate_amt = wcf.Shrlon.of_roundmoney(state.SsWsPass, (wcf.Shrlon.of_computeinterest(state.SsWsPass, ls_memcoopid, ls_contno, loanrcvfix_date)));
                            }
                            else { intestimate_amt = 0; }
                            if (ldc_calintflag == 1)
                            {
                                dw_clear.SetItemDecimal(i + 1, "intestimate_amt", 0);
                            }
                            else
                            {
                                dw_clear.SetItemDecimal(i + 1, "intestimate_amt", intestimate_amt);
                            }
                            dw_clear.SetItemDecimal(i + 1, "clear_status", 1);
                            JsSumOthClr();
                        }
                        else { li_status = 0; }

                    }
                    catch (Exception ex)
                    {
                        //LtServerMessage.Text = WebUtil.ErrorMessage("Genbaseloanclear===>" + ex); 
                    }
                }
            }
        }

        /// <summary>
        /// init ข้อมูลคนค้ำ
        /// </summary>
        private void JsGetMemberCollno()
        {
            try
            {
                int row = Convert.ToInt16(HdRefcollrow.Value);
                String ls_memcoopid;
                String ref_collno = WebUtil.MemberNoFormat(HdRefcoll.Value);
                Decimal period_payamt = dw_main.GetItemDecimal(1, "period_payamt");
                String description = "";
                String membtype_code = "";
                Decimal ldc_salary = 0, retry_age = 0;
                DateTime ldtm_member = DateTime.Now;
                DateTime ldtm_birth = DateTime.Now;
                DateTime ldtm_retry = DateTime.Now;
                Decimal lndropgrantee_flag = 0;
                String remark = "";
                string loancolltype_code = dw_coll.GetItemString(row, "loancolltype_code");

                if (loancolltype_code == "01")
                {
                    dw_coll.SetItemString(row, "ref_collno", ref_collno);

                    if (HdMemcoopId.Value == "")
                    {
                        ls_memcoopid = dw_main.GetItemString(1, "memcoop_id");
                    }
                    else
                    {
                        ls_memcoopid = HdMemcoopId.Value;
                    }
                    dw_coll.SetItemString(row, "coop_id", state.SsCoopControl);

                    String sqlstr = @"  SELECT       MBMEMBMASTER.BIRTH_DATE,
                                             MBMEMBMASTER.LNDROPGRANTEE_FLAG,
                                             MBMEMBMASTER.REMARK,
                                             MBMEMBMASTER.MEMBER_DATE,   
                                             MBMEMBMASTER.WORK_DATE,   
                                             MBMEMBMASTER.RETRY_DATE,   
                                             MBMEMBMASTER.SALARY_AMOUNT,   
                                             MBMEMBMASTER.MEMBTYPE_CODE,   
                                             MBUCFMEMBTYPE.MEMBTYPE_DESC,                                             
                                             mbucfprename.prename_desc||mbmembmaster.memb_name||'   '||mbmembmaster.memb_surname as member_name
                               FROM MBMEMBMASTER,   
                                     MBUCFMEMBGROUP,   
                                     MBUCFPRENAME,
                                     MBUCFMEMBTYPE
                                  
                               WHERE ( MBUCFMEMBGROUP.MEMBGROUP_CODE = MBMEMBMASTER.MEMBGROUP_CODE ) and                                     
                                     ( MBMEMBMASTER.PRENAME_CODE = MBUCFPRENAME.PRENAME_CODE ) and
                                     ( MBMEMBMASTER.MEMBTYPE_CODE = MBUCFMEMBTYPE.MEMBTYPE_CODE ) and 
                                     ( MBMEMBMASTER.COOP_ID = MBUCFMEMBGROUP.COOP_ID ) and  
                                      ( mbmembmaster.member_no = '" + ref_collno + @"' ) AND                                 
                                     MBMEMBMASTER.COOP_ID   ='" + state.SsCoopControl + @"'    ";
                    Sdt dt = WebUtil.QuerySdt(sqlstr);
                    while (dt.Next())
                    {
                        lndropgrantee_flag = dt.GetDecimal("lndropgrantee_flag");
                        description = dt.GetString("member_name");
                        try
                        {
                            remark = dt.GetString("remark");
                        }
                        catch { remark = ""; }
                        membtype_code = dt.GetString("membtype_code");
                        ldc_salary = dt.GetDecimal("salary_amount");
                        try
                        {
                            ldtm_birth = dt.GetDate("BIRTH_DATE");
                        }
                        catch { }

                        try
                        {
                            ///<หาวันที่เกษียณ>
                            ldtm_retry = wcf.InterPreter.CalReTryDate(state.SsConnectionIndex, state.SsCoopControl, ldtm_birth);
                        }
                        catch { }


                    }

                    String[] mem_coll = shrlonService.GetMembercoll(state.SsWsPass, ls_memcoopid, ref_collno, state.SsWorkDate);

                    retry_age = Math.Round(wcf.Busscom.of_cal_yearmonth(state.SsWsPass, state.SsWorkDate, ldtm_retry) * 12);

                    if (retry_age < period_payamt)
                    {

                        LtServerMessage.Text = WebUtil.WarningMessage("งวดเกษียณของผู้ค้ำ ท. " + ref_collno + "  น้อยกว่างวดการส่งชำระ  " + retry_age + "  <  " + period_payamt);
                        dw_coll.SetItemString(row, "ref_collno", ref_collno);
                        dw_coll.SetItemString(row, "description", description);
                        dw_coll.SetItemDecimal(row, "coll_balance", Convert.ToDecimal(mem_coll[2]));
                    }
                    else
                    {

                        if (mem_coll[0] != "")
                        {
                            dw_coll.SetItemString(row, "ref_collno", ref_collno);
                            dw_coll.SetItemString(row, "description", description);
                            dw_coll.SetItemDecimal(row, "coll_balance", Convert.ToDecimal(mem_coll[2]));
                            HUseamt.Value = mem_coll[2];
                            //  JsPostreturn();
                        }
                        else
                        {
                            dw_coll.SetItemString(row, "ref_collno", ref_collno);
                            dw_coll.SetItemString(row, "description", description);
                            dw_coll.SetItemDecimal(row, "coll_balance", Convert.ToDecimal(mem_coll[2]));
                            HUseamt.Value = mem_coll[2];
                        }
                    }
                }
                else if (loancolltype_code == "02")
                {

                    dw_coll.SetItemString(row, "ref_collno", dw_main.GetItemString(1, "member_no"));
                    dw_coll.SetItemString(row, "description", dw_main.GetItemString(1, "member_name"));
                    dw_coll.SetItemDecimal(row, "coll_balance", dw_main.GetItemDecimal(1, "sharestk_value"));
                    dw_coll.SetItemDecimal(row, "use_amt", dw_main.GetItemDecimal(1, "sharestk_value"));
                    HUseamt.Value = dw_main.GetItemDecimal(1, "sharestk_value").ToString();
                    //  JsPostreturn();
                }
            }
            catch (Exception ex)
            {
                //LtServerMessage.Text = WebUtil.ErrorMessage("JsGetMemberCollno===>" + ex); 
            }
        }

        /// <summary>
        ///  เปิดใบคำขอเก่าขึ้นมาแก้ไข
        /// </summary>
        private void JsOpenOldDocNo()
        {

            str_itemchange strList = new str_itemchange();
            str_requestopen strRequestOpen = new str_requestopen();
            strList = WebUtil.str_itemchange_session(this);
            string docno = dw_main.GetItemString(1, "loanrequest_docno");
            string coop_id = dw_main.GetItemString(1, "coop_id");
            strRequestOpen.memcoop_id = state.SsCoopControl;
            strRequestOpen.request_no = docno;
            strRequestOpen.coop_id = coop_id;
            strRequestOpen.format_type = "CAT";
            strRequestOpen.xml_main = dw_main.Describe("DataWindow.Data.XML");
            strRequestOpen.xml_clear = "";
            strRequestOpen.xml_guarantee = "";
            strRequestOpen.xml_insurance = "";
            strRequestOpen.xml_intspc = "";
            strRequestOpen.xml_otherclr = "";

            try
            {
                strRequestOpen = shrlonService.of_loanrequestopen(state.SsWsPass, strRequestOpen);
                DateTime ldtm_birth = new DateTime();
                DateTime ldtm_retry = new DateTime();

                //นำข้อมูลเก็บไว้ใน DataWindow
                dw_main.Reset();
                dw_main.ImportString(strRequestOpen.xml_main, FileSaveAsType.Xml);
                DwUtil.RetrieveDDDW(dw_main, "loantype_code_1", pbl, null);
                if (dw_main.RowCount > 1) dw_main.DeleteRow(dw_main.RowCount);
                try
                {
                    ldtm_birth = dw_main.GetItemDate(1, "birth_date");
                }
                catch { }
                try
                {
                    ///<หาวันที่เกษียณ>

                    ldtm_retry = wcf.InterPreter.CalReTryDate(state.SsConnectionIndex, state.SsCoopControl, ldtm_birth);

                }
                catch { }
                try
                {
                    dw_main.SetItemDateTime(1, "retry_date", ldtm_retry);
                }
                catch { dw_main.SetItemDateTime(1, "retry_date", DateTime.Now); }

                try
                {
                    ///<หาเกษียณอายุ>
                    Decimal retry_age = BusscomService.of_cal_yearmonth(state.SsWsPass, DateTime.Now, ldtm_retry);
                    String retry_agel = WebUtil.Left(retry_age.ToString("00.00"), 2);
                    String retry_ager = WebUtil.Right(retry_age.ToString(""), 2);
                    int retryagel = Convert.ToInt32(retry_agel) * 12;
                    Decimal retryager = (Math.Round(Convert.ToDecimal(retry_ager)) / 10) * 10;
                    dw_main.SetItemDecimal(1, "retry_age", retryagel + retryager);
                }
                catch
                {
                    Decimal retry_age = BusscomService.of_cal_yearmonth(state.SsWsPass, DateTime.Now, DateTime.Now);
                    String retry_agel = WebUtil.Left(retry_age.ToString("00.00"), 2);
                    String retry_ager = WebUtil.Right(retry_age.ToString(""), 2);
                    int retryagel = Convert.ToInt32(retry_agel) * 12;
                    Decimal retryager = (Math.Round(Convert.ToDecimal(retry_ager)) / 10) * 10;
                    dw_main.SetItemDecimal(1, "retry_age", retryagel + retryager);
                }

                string expendBank = "";
                try
                { expendBank = dw_main.GetItemString(1, "expense_bank"); }
                catch { expendBank = ""; }
                //ตรวจสอบ ธนาคารว่ามีหรือไม่ 
                if (expendBank != "")
                {
                    // เรียก Method  ShowBranch() สำหรับแสดงสาขาธนาคาร
                    JsExpenseBank();
                }
                tDwMain.Eng2ThaiAllRow();


                try
                {
                    dw_coll.Reset();
                    dw_coll.ImportString(strRequestOpen.xml_guarantee, FileSaveAsType.Xml);
                }
                catch
                {
                    dw_coll.Reset();
                    DwUtil.ImportData(strRequestOpen.xml_guarantee, dw_coll, null, FileSaveAsType.Xml);

                }
                try
                {
                    dw_clear.Reset();
                    dw_clear.ImportString(strRequestOpen.xml_clear, FileSaveAsType.Xml);
                }
                catch
                {
                    dw_clear.Reset();
                }
                try
                {
                    dw_otherclr.Reset();
                    dw_otherclr.ImportString(LtXmlOtherlr.Text, FileSaveAsType.Xml);
                }
                catch
                {
                    dw_otherclr.Reset();
                }

                strList.xml_main = strRequestOpen.xml_main;
                strList.xml_guarantee = strRequestOpen.xml_guarantee;
                strList.xml_clear = strRequestOpen.xml_clear;
                strList.xml_otherclr = strRequestOpen.xml_otherclr;

                Session["strItemchange"] = strList;
                LtXmlOtherlr.Text = strRequestOpen.xml_otherclr;

                Decimal loanrequestStatus = dw_main.GetItemDecimal(1, "loanrequest_status");
                //เปิดให้แก้ไขได้หลังจาก open 
                //if ((loanrequestStatus != 8) && (loanrequestStatus != 81) && (loanrequestStatus != 11))
                //{
                //    dw_main.DisplayOnly = true;
                //    dw_clear.DisplayOnly = true;
                //    dw_coll.DisplayOnly = true;
                //    dw_otherclr.DisplayOnly = true;
                //}
                //else
                //{
                //    dw_main.DisplayOnly = false;
                //    dw_clear.DisplayOnly = false;
                //    dw_coll.DisplayOnly = false;
                //    dw_otherclr.DisplayOnly = false;
                //}
            }
            catch (Exception ex)
            {
                //LtServerMessage.Text = WebUtil.ErrorMessage("JsOpenOldDocNo====>" + ex); 
            }

        }
        /// <summary>
        /// ยกเลิก ใบคำขอกู้
        /// </summary>    
        private void JsCancelRequest()
        {
            String dwXmlMain = dw_main.Describe("DataWindow.Data.XML");
            String dwXmlMessage = "";
            String cancelID = state.SsUsername;
            String coop_id = state.SsCoopId;
            try
            {
                int result = shrlonService.CancelRequest(state.SsWsPass, ref dwXmlMain, ref dwXmlMessage, cancelID, coop_id);
                if (result == 1)
                {
                    //นำเข้าข้อมูลหลัก
                    dw_main.Reset();
                    dw_main.ImportString(dwXmlMain, FileSaveAsType.Xml);
                    if (dw_main.RowCount > 1) dw_main.DeleteRow(dw_main.RowCount);

                    SaveWebSheet();
                    LtServerMessage.Text = WebUtil.WarningMessage("ได้ทำการยกเลิกใบคำขอกู้เงินเรียบร้อยแล้ว");
                }
                else
                {
                    if ((dwXmlMessage != "") && (dwXmlMessage != null))
                    {
                        dw_message.Reset();
                        dw_message.ImportString(dwXmlMessage, FileSaveAsType.Xml);
                        string msgtext = dw_message.GetItemString(1, "msgtext");

                        LtServerMessage.Text = WebUtil.WarningMessage(msgtext);
                    }
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("JsCancelRequest===>" + ex);
            }

        }

        /// <summary>
        /// set วันที่เริ่มเรียกเก็บ
        /// </summary>



        /// <summary>
        ///ยอดขอกู้==> หายอดชำระ
        /// </summary>
        private void JsSetpriod()
        {
            string ls_loantype = dw_main.GetItemString(1, "loantype_code");
            Decimal loanrequest_amt = dw_main.GetItemDecimal(1, "loanrequest_amt");
            Decimal period_payamt = dw_main.GetItemDecimal(1, "period_payamt");
            Decimal period_payment = 0;

            try
            {
                // ปัดยอดขอกู้
                String ll_roundloan = wcf.Busscom.of_getattribloantype(state.SsWsPass, ls_loantype, "reqround_factor");
                int roundloan = Convert.ToInt16(ll_roundloan);
                if (roundloan > 0)
                {
                    if (loanrequest_amt > 0)
                    {
                        loanrequest_amt = Math.Round((loanrequest_amt) / roundloan) * roundloan;
                    }
                    else
                    {
                        loanrequest_amt = 0;

                    }
                }
                dw_main.SetItemDecimal(1, "loanrequest_amt", loanrequest_amt);

                // ปัดยอดชำระ
                String ll_roundpay = wcf.Busscom.of_getattribloantype(state.SsWsPass, ls_loantype, "payround_factor");
                int roundpay = Convert.ToInt16(ll_roundpay);
                if (roundpay > 0)
                {
                    if (period_payamt > 0)
                    {
                        period_payment = loanrequest_amt / period_payamt;// Math.Round((loanrequest_amt / period_payamt) / roundpay) * roundpay;
                        if ((loanrequest_amt / period_payamt) > period_payment) { period_payment = period_payment + roundpay; }
                    }
                    else
                    {
                        period_payment = 0;
                    }
                }
                period_payment = Math.Round(period_payment / roundpay) * roundpay;
                dw_main.SetItemDecimal(1, "period_payment", period_payment);

                Decimal intestimate_amt = of_calintestimatemain();
                dw_main.SetItemDecimal(1, "intestimate_amt", intestimate_amt);
                JsLoanpaymenttype();
                JsGenBuyshare();
                JsInsertRowcoll();
            }
            catch (Exception ex)
            {
                //LtServerMessage.Text = WebUtil.ErrorMessage("JsSetpriod===>" + ex); 
            }
        }

        /// <summary>
        /// ยอดชำระ==> หาจน.งวด 
        /// </summary>
        private void JsRevert()
        {
            string ls_loantype = dw_main.GetItemString(1, "loantype_code");
            Decimal period_payamt = new Decimal(0.00);
            period_payamt = dw_main.GetItemDecimal(1, "period_payamt");
            Decimal loanrequest_amt = dw_main.GetItemDecimal(1, "loanrequest_amt");
            Decimal period_payment = dw_main.GetItemDecimal(1, "period_payment");
            Decimal maxsend_payamt = dw_main.GetItemDecimal(1, "maxsend_payamt");
            // ปัดยอดชำระ
            String ll_roundpay = wcf.Busscom.of_getattribloantype(state.SsWsPass, ls_loantype, "payround_factor");
            int roundpay = Convert.ToInt16(ll_roundpay);
            //if (roundpay > 0)
            //{
            //    if (period_payment > 0)
            //    {
            //        period_payment = Math.Round(period_payment / roundpay) * roundpay;

            //    }
            //    else
            //    {
            //        period_payment = 0;

            //    }
            //}

            if (loanrequest_amt == 0 && period_payamt > 0)
            {
                loanrequest_amt = period_payamt * period_payment;
                // ปัดยอดขอกู้
                String ll_roundloan = wcf.Busscom.of_getattribloantype(state.SsWsPass, ls_loantype, "reqround_factor");
                int roundloan = Convert.ToInt16(ll_roundloan);
                if (roundloan > 0)
                {
                    if (loanrequest_amt > 0)
                    {
                        loanrequest_amt = Math.Round((loanrequest_amt) / roundloan) * roundloan;
                    }
                    else
                    {
                        loanrequest_amt = 0;

                    }
                }
                dw_main.SetItemDecimal(1, "loanrequest_amt", loanrequest_amt);

            }
            else
            {
                period_payamt = loanrequest_amt / period_payment;
                if (period_payamt > maxsend_payamt)
                {
                    period_payamt = maxsend_payamt;
                    period_payment = loanrequest_amt / period_payamt;
                    //period_payment = Math.Round(period_payment / roundpay) * roundpay;
                }
                period_payment = Math.Round(period_payment / roundpay) * roundpay;
                dw_main.SetItemDecimal(1, "period_payment", period_payment);

                dw_main.SetItemDouble(1, "period_payamt", Math.Round(Convert.ToDouble(period_payamt)));
            }
            HdIsPostBack.Value = "false";
        }

        /// <summary>
        /// จน.งวด==> หายอดชำระ
        ///</summary>         
        private void JsContPeriod()
        {


            string ls_loantype = dw_main.GetItemString(1, "loantype_code");
            Decimal period_payamt = dw_main.GetItemDecimal(1, "period_payamt");
            Decimal loanrequest_amt = dw_main.GetItemDecimal(1, "loanrequest_amt");
            decimal loanpayment_type = dw_main.GetItemDecimal(1, "loanpayment_type");
            decimal ldc_intrate = dw_main.GetItemDecimal(1, "int_contintrate");
            Decimal period_payment = 0;

            // ปัดยอดชำระ
            String ll_roundpay = wcf.Busscom.of_getattribloantype(state.SsWsPass, ls_loantype, "payround_factor");
            int roundpay = Convert.ToInt16(ll_roundpay);
            double ldc_fr = 1, ldc_temp = 1, loapayment_amt = 0;
            if (loanpayment_type == 1)
            {
                //คงต้น
                period_payment = loanrequest_amt / period_payamt;
            }
            else
            {
                int li_fixcaltype = 1;
                //คงยอด


                if (li_fixcaltype == 1)
                {
                    // ด/บ ทั้งปี / 12
                    //ldc_fr			= exp( - ai_period * log( ( 1 + adc_intrate / 12 ) ) )
                    //ldc_payamt	= adc_principal * ( adc_intrate / 12 ) / ( 1 - ldc_fr )

                    ldc_temp = Math.Log(1 + (Convert.ToDouble(ldc_intrate / 12)));
                    ldc_fr = Math.Exp(-Convert.ToDouble(period_payamt) * ldc_temp);
                    loapayment_amt = (Convert.ToDouble(loanrequest_amt) * (Convert.ToDouble(ldc_intrate) / 12)) / ((1 - ldc_fr));


                }
                else
                {
                    //แบบสุโขทัย
                    //ldc_fr			= ( adc_intrate * ( 1+ adc_intrate) ^ ai_period )/ (((1 + adc_intrate ) ^ ai_period ) - 1  )
                    //ldc_payamt = adc_principal / ldc_fr
                }

                period_payment = Math.Ceiling(Convert.ToDecimal(loapayment_amt));
            }
            if (roundpay > 0)
            {

                period_payamt = Math.Round(period_payamt / 10) * 10;

            }
            period_payment = Math.Round(period_payment / roundpay) * roundpay;
            dw_main.SetItemDecimal(1, "period_payment", period_payment);

            HdIsPostBack.Value = "false";
        }

        private void JsLoanpaymenttype()
        {

            string ls_loantype = dw_main.GetItemString(1, "loantype_code");
            Decimal period_payamt = dw_main.GetItemDecimal(1, "period_payamt");
            Decimal loanrequest_amt = dw_main.GetItemDecimal(1, "loanrequest_amt");
            decimal loanpayment_type = dw_main.GetItemDecimal(1, "loanpayment_type");
            decimal ldc_intrate = dw_main.GetItemDecimal(1, "int_contintrate");
            Decimal period_payment = 0;

            // ปัดยอดชำระ
            String ll_roundpay = wcf.Busscom.of_getattribloantype(state.SsWsPass, ls_loantype, "payround_factor");
            int roundpay = Convert.ToInt16(ll_roundpay);
            double ldc_fr = 1, ldc_temp = 1, loapayment_amt = 0;
            if (loanpayment_type == 1)
            {
                //คงต้น
                period_payment = loanrequest_amt / period_payamt;
            }
            else
            {
                int li_fixcaltype = 1;
                //คงยอด


                if (li_fixcaltype == 1)
                {
                    // ด/บ ทั้งปี / 12
                    //ldc_fr			= exp( - ai_period * log( ( 1 + adc_intrate / 12 ) ) )
                    //ldc_payamt	= adc_principal * ( adc_intrate / 12 ) / ( 1 - ldc_fr )

                    ldc_temp = Math.Log(1 + (Convert.ToDouble(ldc_intrate / 12)));
                    ldc_fr = Math.Exp(-Convert.ToDouble(period_payamt) * ldc_temp);
                    loapayment_amt = (Convert.ToDouble(loanrequest_amt) * (Convert.ToDouble(ldc_intrate) / 12)) / ((1 - ldc_fr));


                }
                else
                {
                    //แบบสุโขทัย
                    //ldc_fr			= ( adc_intrate * ( 1+ adc_intrate) ^ ai_period )/ (((1 + adc_intrate ) ^ ai_period ) - 1  )
                    //ldc_payamt = adc_principal / ldc_fr
                }

                period_payment = Math.Ceiling(Convert.ToDecimal(loapayment_amt));
            }
            if (roundpay > 0)
            {

                period_payamt = Math.Round(period_payamt / 10) * 10;

            }
            dw_main.SetItemDecimal(1, "period_payment", period_payment);
            HdIsPostBack.Value = "false";
        }


        /// <summary>
        /// ค้ำประกัน
        /// </summary>
        private void JsCollCondition()
        {
            String dwXmlMain = dw_main.Describe("DataWindow.Data.XML");
            String dwXmlColl = dw_coll.Describe("DataWindow.Data.XML");
            String dwXmlMessage = "";
            try
            {
                dwXmlColl = shrlonService.CollPercCondition(state.SsWsPass, dwXmlMain, dwXmlColl, ref dwXmlMessage);

                try
                {
                    if ((dwXmlColl != "") && (dwXmlColl != null))
                    {
                        dw_coll.Reset();
                        dw_coll.ImportString(dwXmlColl, FileSaveAsType.Xml);
                    }

                }
                catch
                {
                    dw_coll.Reset();

                }


                if ((dwXmlMessage != "") && (dwXmlMessage != null))
                {
                    dw_message.Reset();
                    dw_message.ImportString(dwXmlMessage, FileSaveAsType.Xml);
                    string msgtext = dw_message.GetItemString(1, "msgtext");
                    LtServerMessage.Text = WebUtil.WarningMessage(msgtext);
                }
            }
            catch (Exception ex)
            {
                //LtServerMessage.Text = WebUtil.ErrorMessage("JsCollCondition===>" + ex); 
            }

        }

        private void JsCollInitP()
        {


            String dwXmlMain = dw_main.Describe("DataWindow.Data.XML");
            String dwXmlColl = dw_coll.Describe("DataWindow.Data.XML");

            dwXmlColl = shrlonService.CollInitPrecent(state.SsWsPass, dwXmlMain, dwXmlColl);

            try
            {
                if ((dwXmlColl != "") && (dwXmlColl != null))
                {
                    dw_coll.Reset();
                    dw_coll.ImportString(dwXmlColl, FileSaveAsType.Xml);
                }

            }
            catch
            {
                dw_coll.Reset();
                //dw_coll.InsertRow(0); 
            }

            string ls_loantype = dw_main.GetItemString(1, "loantype_code");
            if (ls_loantype != "23")
            {
                Decimal coll_balance = 0;
                string loancolltype_code = "";
                Decimal total = 0;
                Decimal balance = 0;
                Decimal per90 = new Decimal(0.9);
                Decimal sharestk_value = dw_main.GetItemDecimal(1, "sharestk_value");
                if (ls_loantype == "21") { sharestk_value = sharestk_value * per90; }
                Decimal loanrequest_amt, loanrequest;
                loanrequest = dw_main.GetItemDecimal(1, "loanrequest_amt");
                loanrequest_amt = loanrequest - sharestk_value;
                int row = dw_coll.RowCount;
                int i = 0;
                Decimal sum_balance = 0, coll_percen = 0, total_balance = 0;


                for (i = 0; i < row; i++)
                {

                    loancolltype_code = dw_coll.GetItemString(i + 1, "loancolltype_code");

                    if (loancolltype_code == "03")
                    {
                        coll_balance = dw_coll.GetItemDecimal(i + 1, "coll_balance");

                    }


                    total = total + coll_balance;

                }

                while (row > 0)
                {
                    if (row > 1)
                    {

                        String ref_collno = dw_coll.GetItemString(row, "ref_collno");
                        string sql = @"  SELECT LNCONTCOLL.LOANCONTRACT_NO,   
                                 LNCONTMASTER.MEMBER_NO,   
                                 
                                 MBMEMBMASTER.MEMB_NAME,   
                                 MBMEMBMASTER.MEMB_SURNAME,   
                                 lncontmaster.principal_balance + lncontmaster.withdrawable_amt as sum_balance,   
                                 LNCONTCOLL.COLL_PERCENT,   
                                 'CONT' as itemtype_code  
                            FROM LNCONTCOLL,   
                                 LNCONTMASTER,   
                                 LNLOANTYPE,   
                                 MBMEMBMASTER 
                           WHERE ( LNCONTCOLL.LOANCONTRACT_NO = LNCONTMASTER.LOANCONTRACT_NO ) and  
                                 ( LNCONTMASTER.LOANTYPE_CODE = LNLOANTYPE.LOANTYPE_CODE ) and  
                                 ( LNCONTMASTER.MEMBER_NO = MBMEMBMASTER.MEMBER_NO ) and  
                                 ( LNCONTCOLL.COOP_ID = LNCONTMASTER.COOP_ID ) and  
                                 ( LNCONTMASTER.COOP_ID = LNLOANTYPE.COOP_ID ) and  
                                 ( LNCONTMASTER.MEMCOOP_ID = MBMEMBMASTER.COOP_ID ) and  
                                 ( lncontcoll.ref_collno = '" + ref_collno + @"' ) AND  
                                 ( lncontcoll.loancolltype_code = '01' ) AND  
                                 ( lncontcoll.coll_status = 1 ) AND  
                                 ( lncontmaster.contract_status > 0 ) AND         
                                 LNCONTCOLL.COOP_ID = '" + state.SsCoopId + @"'   ";

                        Sdt dt = WebUtil.QuerySdt(sql);
                        if (dt.Next())
                        {
                            int rowCount = dt.GetRowCount();
                            for (int x = 0; x < rowCount; x++)
                            {
                                sum_balance = Convert.ToDecimal(dt.Rows[x]["sum_balance"]);
                                coll_percen = Convert.ToDecimal(dt.Rows[x]["COLL_PERCENT"]);

                            }
                            total_balance = sum_balance * coll_percen;
                        }


                        Decimal collbalance = dw_coll.GetItemDecimal(row, "coll_balance");
                        balance = loanrequest_amt * ((collbalance - total_balance) / total);
                        //check มีรายการอายัดอยู่หรือไม่
                        if (loancolltype_code == "03")
                        {
                            String as_deptaccount = ref_collno;
                            Decimal adc_chack_amt = balance;
                            short ai_pass = 0;
                            string ls_message = "";
                            int result = wcf.Shrlon.of_chack_prncbal(state.SsWsPass, as_deptaccount, adc_chack_amt, ref  ai_pass, ref  ls_message);
                            if (ai_pass == 0) { LtServerMessage.Text = WebUtil.ErrorMessage("บัญชี  " + as_deptaccount + " " + ls_message); }
                        }

                        // balance = loanrequest_amt * (collbalance / total);
                        dw_coll.SetItemDecimal(row, "use_amt", balance);
                        dw_coll.SetItemDecimal(row, "coll_percent", (balance / loanrequest));
                    }
                    else
                    {

                        dw_coll.SetItemDecimal(row, "use_amt", sharestk_value);
                        dw_coll.SetItemDecimal(row, "coll_percent", (sharestk_value / loanrequest));
                    }

                    row--;
                }

                Decimal bal = balance + balance;

                //if (loanrequest != bal) { LtServerMessage.Text = WebUtil.WarningMessage("ยอดค้ำประกันไม่เท่ากับยอดขอกู้"); }
            }

        }

        private void JsSetDataList()
        {
            str_itemchange strList = new str_itemchange();
            strList = WebUtil.str_itemchange_session(this);
            if (dw_clear.RowCount == 0)
            { strList.xml_clear = null; }
            else { strList.xml_clear = dw_clear.Describe("DataWindow.Data.XML"); }
            if (dw_coll.RowCount == 0) { strList.xml_guarantee = null; }
            else { strList.xml_guarantee = dw_coll.Describe("DataWindow.Data.XML"); }

            Session["strItemchange"] = strList;
        }

        private void JsPostColl()
        {
            try
            {

                String columnName = "ref_collno";
                if (HdColumnName.Value == "" || HdColumnName.Value == "setcolldetail") { columnName = "setcolldetail"; }
                String dwMainXML = dw_main.Describe("DataWindow.Data.XML");
                String dwCollXML = dw_coll.Describe("DataWindow.Data.XML");
                String dwClearXML = dw_clear.Describe("DataWindow.Data.XML");
                String t = dw_main.GetItemString(1, "loantype_code");
                str_itemchange strList = new str_itemchange();

                // ตรวจสอบว่า HdRowNumber มีค่าหรือไม่ ถ้าไม่มี ไม่ต้องทำอะไร
                if (HdRowNumber.Value.ToString() != "")
                {
                    int rowNumber = Convert.ToInt32(HdRowNumber.Value.ToString());

                    HdColumnName.Value = "";
                    if ((columnName == "checkmancollperiod") || (columnName == "setcolldetail"))
                    {
                        dw_coll.SetItemString(rowNumber, "ref_collno", HdRefcollNO.Value);
                        dwCollXML = dw_coll.Describe("DataWindow.Data.XML");
                    }
                    String refCollNo = "";
                    try
                    {
                        refCollNo = dw_coll.GetItemString(rowNumber, "ref_collno");
                        HdRefcollNO.Value = refCollNo;
                    }
                    catch
                    {
                        refCollNo = "";
                    }

                    strList.column_name = columnName;
                    strList.xml_main = dwMainXML;
                    strList.xml_guarantee = dwCollXML;
                    strList.xml_clear = dwClearXML;
                    strList.import_flag = true;
                    strList.format_type = "CAT";

                    strList.column_data = dw_coll.GetItemString(rowNumber, "loancolltype_code") + refCollNo;
                    // เรียก service สำหรับการเปลี่ยนแปลงค่า ของ DW_coll
                    int result = shrlonService.LoanRightItemChangeColl(state.SsWsPass, ref strList);
                    Session["strItemchange"] = strList;
                    //if ((strList.xml_message != null) && (strList.xml_message != ""))
                    //{
                    //    //dw_message.Reset();MO
                    //    //dw_message.ImportString(strList.xml_message, FileSaveAsType.Xml);
                    //    DwUtil.ImportData(strList.xml_message, dw_message, null, FileSaveAsType.Xml);
                    //    HdMsg.Value = dw_message.GetItemString(1, "msgtext");
                    //    HdMsgWarning.Value = dw_message.GetItemString(1, "msgicon");
                    //}
                    if (result == 8)
                    {
                        HdReturn.Value = result.ToString();
                        HdColumnName.Value = strList.column_name;
                    }
                    //try
                    //{
                    //    dw_otherclr.Reset();
                    //    dw_otherclr.ImportString(LtXmlOtherlr.Text, FileSaveAsType.Xml);
                    //}
                    //catch { dw_otherclr.Reset(); }
                    try
                    {
                        dw_coll.Reset();
                        dw_coll.ImportString(strList.xml_guarantee, FileSaveAsType.Xml);
                    }
                    catch { dw_coll.Reset(); }

                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }

        }

        /// <summary>
        /// sum ชำระหนี้อืนทั้งหมด
        /// </summary>       
        private void JsSumOthClr()
        {


            DateTime loanrcvfix_date = dw_main.GetItemDate(1, "loanrcvfix_date");
            String ls_memcoopid;
            Decimal ldc_contintrate = dw_main.GetItemDecimal(1, "int_contintrate");
            String ls_continttabcode = dw_main.GetItemString(1, "int_continttabcode");
            short year = Convert.ToInt16(loanrcvfix_date.Year + 543);
            short month = Convert.ToInt16(loanrcvfix_date.Month);
            int day_amount = 31;

            // DateTime postingdate_bf = wcf.Busscom.of_getpostingdate2(state.SsWsPass, Convert.ToInt16(loanrcvfix_date.Year + 543), Convert.ToInt16(loanrcvfix_date.Month));
            // DateTime postingdate = wcf.Busscom.of_getpostingdate2(state.SsWsPass, Convert.ToInt16(postingdate_bf.Year + 543), Convert.ToInt16(postingdate_bf.Month + 1));
            //DateTime postingdate_af = wcf.Busscom.of_getpostingdate2(state.SsWsPass, Convert.ToInt16(postingdate.Year + 543), Convert.ToInt16(postingdate.Month + 1));
            DateTime postingdate_bf = wcf.Busscom.of_getpostingdate2(state.SsWsPass, Convert.ToInt16(loanrcvfix_date.Year + 543), Convert.ToInt16(loanrcvfix_date.Month - 1));
            DateTime postingdate = wcf.Busscom.of_getpostingdate(state.SsWsPass, loanrcvfix_date);
            DateTime postingdate_af = wcf.Busscom.of_getpostingdate(state.SsWsPass, postingdate.AddMonths(1));


            //จ่ายเงินกู้หลังเรียกเก็บหรือไม่
            if (loanrcvfix_date > postingdate && loanrcvfix_date < postingdate_bf)
            {

                day_amount = loanrcvfix_date.DayOfYear - postingdate_bf.DayOfYear;

            }
            else if (loanrcvfix_date > postingdate_bf && loanrcvfix_date < postingdate)
            {

                // postingdate = wcf.Busscom.of_getpostingdate2(state.SsWsPass, Convert.ToInt16(postingdate_af.Year + 543), Convert.ToInt16(postingdate_af.Month));

                day_amount = loanrcvfix_date.DayOfYear - postingdate_bf.DayOfYear;

            }
            else
            {
                postingdate = wcf.Busscom.of_getpostingdate2(state.SsWsPass, Convert.ToInt16(loanrcvfix_date.Year + 543), Convert.ToInt16(loanrcvfix_date.Month - 1));

                day_amount = loanrcvfix_date.DayOfYear - postingdate.DayOfYear;
            }

            try { ls_memcoopid = dw_main.GetItemString(1, "memcoop_id"); }
            catch { ls_memcoopid = state.SsCoopControl; }
            Decimal clrother_amt = 0;
            Decimal principal_balance = 0, intestimate_amt = 0;
            Decimal sum_clear1 = 0;
            Decimal otherclr_amt = 0;

            Decimal clear_status;
            int i, j;
            int row_clr = dw_otherclr.RowCount;
            int row_clear = dw_clear.RowCount;
            if (row_clr > 0)
            {
                for (i = 0; i < row_clr; i++)
                {
                    try
                    {
                        clrother_amt = dw_otherclr.GetItemDecimal(i + 1, "clrother_amt");
                    }
                    catch { clrother_amt = 0; }
                    otherclr_amt = otherclr_amt + clrother_amt;
                    if (otherclr_amt > 0)
                    {
                        dw_main.SetItemDecimal(1, "otherclr_flag", 1);
                        dw_main.SetItemDecimal(1, "otherclr_amt", otherclr_amt);
                    }
                    else if (clrother_amt == 0)
                    {
                        dw_main.SetItemDecimal(1, "otherclr_flag", 0);
                        dw_main.SetItemDecimal(1, "otherclr_amt", 0);
                    }
                }
            }
            if (row_clear > 0)
            {
                Decimal ldc_balance = 0;
                for (j = 0; j < row_clear; j++)
                {
                    Decimal loanpayment_type = dw_clear.GetItemDecimal(j + 1, "loanpayment_type");
                    string ls_contno = dw_clear.GetItemString(j + 1, "loancontract_no");
                    DateTime ldtm_calintfrom = dw_clear.GetItemDateTime(j + 1, "lastcalint_date");
                    String ls_loantype = dw_clear.GetItemString(j + 1, "loantype_code");

                    try
                    {
                        clear_status = dw_clear.GetItemDecimal(j + 1, "clear_status");
                    }
                    catch { clear_status = 0; }
                    if (clear_status == 1)
                    {

                        if (Hdprincipal.Value == null || Hdprincipal.Value == "")
                        {

                            try
                            {
                                principal_balance = dw_clear.GetItemDecimal(j + 1, "principal_balance");
                            }
                            catch { principal_balance = 0; }

                            Decimal ldc_rkeep_principal = dw_clear.GetItemDecimal(j + 1, "rkeep_principal");
                            try
                            {
                                if (loanpayment_type == 1)
                                {
                                    intestimate_amt = dw_clear.GetItemDecimal(j + 1, "intestimate_amt");
                                }
                                else { intestimate_amt = 0; }
                                dw_clear.SetItemDecimal(j + 1, "intestimate_amt", intestimate_amt);
                                Decimal ldcpayment = Convert.ToDecimal(wcf.InterPreter.Getcutloanshr(ldc_rkeep_principal));
                                ldc_balance = principal_balance - ldcpayment;

                                dw_clear.SetItemDecimal(j + 1, "principal_balance", ldc_balance);
                            }
                            catch { intestimate_amt = 0; }
                        }
                        else
                        {

                            try
                            {
                                principal_balance = dw_clear.GetItemDecimal(j + 1, "principal_balance");
                            }
                            catch { principal_balance = 0; }

                            Decimal ldc_rkeep_principal = dw_clear.GetItemDecimal(j + 1, "rkeep_principal");
                            Decimal ldcpayment = Convert.ToDecimal(wcf.InterPreter.Getcutloanshr(ldc_rkeep_principal));
                            ldc_balance = principal_balance - ldcpayment;
                            Decimal ldc_inttotal = wcf.Shrlon.of_computeinterest2(state.SsWsPass, state.SsCoopControl, ls_continttabcode, ldtm_calintfrom, loanrcvfix_date, ldc_balance, ldc_balance);
                            dw_clear.SetItemDecimal(j + 1, "intestimate_amt", ldc_inttotal);
                            // dw_clear.SetItemDecimal(j + 1, "principal_balance", ldc_balance);
                        }


                        intestimate_amt = dw_clear.GetItemDecimal(j + 1, "intestimate_amt");

                        sum_clear1 = sum_clear1 + ldc_balance + intestimate_amt;

                    }


                    else if (clear_status == 0)
                    {

                        day_amount = postingdate.DayOfYear - postingdate_bf.DayOfYear;
                        principal_balance = dw_clear.GetItemDecimal(j + 1, "principal_balance");
                        intestimate_amt = principal_balance * (ldc_contintrate) * day_amount / 365;
                        intestimate_amt = wcf.Shrlon.of_roundmoney(state.SsWsPass, intestimate_amt);
                        // intestimate_amt = wcf.Shrlon.of_roundmoney(state.SsWsPass, (wcf.Shrlon.of_computeinterest(state.SsWsPass, ls_memcoopid, ls_contno, loanrcvfix_date)));

                        dw_clear.SetItemDecimal(j + 1, "intestimate_amt", intestimate_amt);

                    }
                }

            }
            Decimal total = sum_clear1;
            dw_main.SetItemDecimal(1, "sum_clear", total);
            if (total > 0)
            {
                dw_main.SetItemDecimal(1, "clearloan_flag", 1);
            }
            else { dw_main.SetItemDecimal(1, "clearloan_flag", 0); }
            // of_recalloanpermiss();
        }

        /// <summary>
        /// sum ชำระหนี้เก่าทั้งหมด
        /// </summary>  
        private void JsResumLoanClear()
        {
            //Session["xmlloandetail"];
            String xmlMain = "";
            String xmlColl = "";
            String xmlClear = "";
            String xmlLoanDetail = "";
            String xmlMessage = "";
            try
            {
                xmlMain = dw_main.Describe("DataWindow.Data.XML");

                xmlLoanDetail = Session["xmlloandetail"].ToString();
                if (dw_coll.RowCount == 0)
                { xmlColl = null; }
                else { xmlColl = dw_coll.Describe("DataWindow.Data.XML"); }

                if (dw_clear.RowCount == 0)
                { xmlClear = null; }
                else { xmlClear = dw_clear.Describe("DataWindow.Data.XML"); }

                int result = shrlonService.ResumLoanClear(state.SsWsPass, ref xmlMain, ref xmlClear, ref xmlColl, xmlLoanDetail, ref xmlMessage);

                if (result == 1)
                {
                    try
                    {
                        //นำเข้าข้อมูลหลัก
                        dw_main.Reset();
                        dw_main.ImportString(xmlMain, FileSaveAsType.Xml);
                        //DwUtil.ImportData(xmlMain, dw_main, tDwMain, FileSaveAsType.Xml);
                        if (dw_main.RowCount > 1) dw_main.DeleteRow(dw_main.RowCount);
                    }
                    catch { dw_main.Reset(); dw_main.InsertRow(0); }
                    try
                    {
                        dw_clear.Reset();
                        dw_clear.ImportString(xmlClear, FileSaveAsType.Xml);
                    }
                    catch { dw_clear.Reset(); }
                    try
                    {
                        dw_coll.Reset();
                        dw_coll.ImportString(xmlColl, FileSaveAsType.Xml);
                    }
                    catch { dw_coll.Reset(); }
                    LtXmlLoanDetail.Text = xmlLoanDetail;
                }
                if ((xmlMessage != null) && (xmlMessage != ""))
                {
                    dw_message.Reset();
                    dw_message.ImportString(xmlMessage, FileSaveAsType.Xml);
                    LtServerMessage.Text = WebUtil.ErrorMessage(dw_message.GetItemString(1, "msgtext"));
                }
            }
            catch (Exception ex)
            {
                //LtServerMessage.Text = WebUtil.ErrorMessage("JsResumLoanClear===>" + ex);
            }

        }


        /// <summary>
        /// หาสิทธิ์กู้สูงสุดจากอายุสมาชิก
        /// </summary>
        private void Jsmaxcreditperiod()
        {
            try
            {
                String ls_loantype = dw_main.GetItemString(1, "loantype_code");
                DateTime ldtm_member;
                try
                {
                    ldtm_member = dw_main.GetItemDateTime(1, "member_date");
                }
                catch { ldtm_member = state.SsWorkDate; }


                int ldc_share_lastperiod = Convert.ToInt16(dw_main.GetItemDecimal(1, "share_lastperiod"));
                Decimal ldc_shrstkvalue = dw_main.GetItemDecimal(1, "sharestk_value");
                Decimal ldc_salary = dw_main.GetItemDecimal(1, "salary_amt");
                String ls_memcoopid = dw_main.GetItemString(1, "memcoop_id");
                //  String ls_membtypedesc = dw_main.GetItemString(1, "membtype_desc");
                String member_no = dw_main.GetItemString(1, "member_no");

                ///<หาอายุงานของาสมาชิก>
                //int memtime = 0;
                //Decimal member_age = dw_main.GetItemDecimal(1, "member_age");
                //if (member_age > 1)
                //{
                //    memtime = Convert.ToInt32(BusscomService.of_cal_yearmonth(state.SsWsPass, ldtm_member, state.SsWorkDate) * 12);

                //}
                //else if (member_age < 1)
                //{
                //    int mem = Convert.ToInt32(member_age * 100);
                //    if (mem >= 3) { memtime = mem; } else { memtime = 0; }
                //}
                //else
                //{

                //    int mem = Convert.ToInt32(member_age * 12);
                //    if (mem >= 3) { memtime = mem; } else { memtime = 0; }
                //}


                ///<กำหนดวันที่จ่ายและเรียกเก็บแต่ปละเภทสัญญาวันจ่ายไม่เหมือนกัน>
                JsChangeStartkeep();

                Decimal[] max_creditperiod = new Decimal[4];

                Decimal loancredit_amt = 0;
                ls_loantype = dw_main.GetItemString(1, "loantype_code");



                String loanright_type;// = wcf.Busscom.of_getattribloantype(state.SsWsPass, ls_loantype, "loanrighttype_code");
                string sqllntype = @"select  loanrighttype_code
                                        from lnloantype  where loantype_code ='" + ls_loantype + @"' ";
                Sdt dtrigtht = WebUtil.QuerySdt(sqllntype);
                if (dtrigtht.Next())
                {
                    loanright_type = dtrigtht.GetString("loanrighttype_code");
                }


                ///< หาสิทธิ์กู้สูงสุด,งวดสูงสุด>
                max_creditperiod = shrlonService.Calloanpermiss(state.SsWsPass, ls_memcoopid, ls_loantype, ldc_salary, ldc_shrstkvalue, ldc_share_lastperiod, member_no);
                Decimal ldc_percenshare = max_creditperiod[2];
                Decimal ldc_percensaraly = max_creditperiod[3];

                if (ldc_percenshare > 0)
                {
                    loancredit_amt = ldc_shrstkvalue * ldc_percenshare;
                }
                else { loancredit_amt = max_creditperiod[0]; }


                dw_main.SetItemDecimal(1, "loancredit_amt", loancredit_amt);
                dw_main.SetItemDecimal(1, "maxsend_payamt", max_creditperiod[1]);

                JsCalperiodSend();//เช็คงวดเกษียณอายุ


                Genbaseloanclear();


            }
            catch (Exception ex)
            {
                // LtServerMessage.Text = WebUtil.ErrorMessage("Jsmaxcreditperiod===>" + ex);
            }

        }
        private void JsSetMonthpayCoop()
        {
            string ls_loantype;
            int li_index, li_count;
            int li_clrstatus, li_paytype, li_shrpaystatus, li_chkbalacestatus;

            Decimal ldc_shrperiod, ldc_payment, ldc_intestm;
            Decimal ldc_sumpay;

            // ดึงรายการหุ้น
            ldc_shrperiod = dw_main.GetItemDecimal(1, "periodshare_value");
            li_shrpaystatus = Convert.ToInt32(dw_main.GetItemDecimal(1, "sharepay_status"));

            // ถ้างดเก็บค่าุหุ้นให้หุ้นต่อเดือนเป็นศูนย์
            if (li_shrpaystatus == -1) { ldc_shrperiod = 0; }


            // wa  ดึงยอดเงินชำระต่อเดือนใบขอกู้
            //ls_loantype = dw_main.GetItemString(1, "loantype_code");
            //li_paytype = Convert.ToInt32(dw_main.GetItemDecimal(1, "loanpayment_type"));
            //ldc_intestm = dw_main.GetItemDecimal(1, "intestimate_amt");
            //ldc_payment = dw_main.GetItemDecimal(1, "period_payment");

            //Decimal ldc_payamt = dw_main.GetItemDecimal(1, "period_payamt");

            ldc_sumpay = 0; // ldc_payment;

            //if (li_paytype == 1)
            //{
            //    ldc_sumpay += ldc_intestm;
            //}

            // ดึงรายการหนี้
            li_count = dw_clear.RowCount;
            for (li_index = 1; li_index <= li_count; li_index++)
            {

                li_clrstatus = Convert.ToInt32(dw_clear.GetItemDecimal(li_index, "clear_status"));
                ls_loantype = dw_clear.GetItemString(li_index, "loantype_code");
                //li_chkbalacestatus = Convert.ToInt32(dw_clear.GetItemDecimal(li_index, "clear_status"));

                if (li_clrstatus == 0)
                {
                    li_paytype = Convert.ToInt32(dw_clear.GetItemDecimal(li_index, "loanpayment_type"));
                    ldc_payment = dw_clear.GetItemDecimal(li_index, "period_payment");
                    ldc_intestm = dw_clear.GetItemDecimal(li_index, "intestimate_amt");

                    if (li_paytype == 1)
                    {
                        ldc_sumpay += ldc_payment + ldc_intestm;
                    }
                    else
                    {
                        ldc_sumpay += ldc_payment;
                    }
                }
            }

            ldc_sumpay = ldc_sumpay + ldc_shrperiod;
            dw_main.SetItemDecimal(1, "paymonth_coop", ldc_sumpay);

        }

        private void of_recalloanpermiss()
        {

            JsSetMonthpayCoop();
            //สิทธิกู้ตามเงินเดือนคงเลหือ
            JsPermissSalary();
            JsContPeriod();
            JsGenBuyshare();
            //  JsSumOthClr();
        }
        /// <summary>
        /// ตรวจสอบสิทธิ์ตามเงินเดือนคงเหลือ
        /// </summary>
        protected void JsPermissSalary()
        {


            //คำนวณสิทธิกู้จากยอดเงินเดือนคงเหลือขั้นต่ำ
            JsSetMonthpayCoop();
            Decimal ldc_shrstkvalue = dw_main.GetItemDecimal(1, "sharestk_value");
            String loantype_code = dw_main.GetItemString(1, "loantype_code");
            decimal ldc_salary = dw_main.GetItemDecimal(1, "salary_amt");
            decimal ldc_mthcoop = dw_main.GetItemDecimal(1, "paymonth_coop");
            decimal ldc_minpercsal = dw_main.GetItemDecimal(1, "minsalary_perc");
            decimal ldc_minsalaamt = dw_main.GetItemDecimal(1, "minsalary_amt");
            decimal ldc_maxloan = dw_main.GetItemDecimal(1, "loancredit_amt");
            decimal ldc_periodsend = dw_main.GetItemDecimal(1, "period_payamt");
            decimal ldc_paymenttype = dw_main.GetItemDecimal(1, "loanpayment_type");
            decimal ldc_intrate = dw_main.GetItemDecimal(1, "int_contintrate");
            decimal retry_age = dw_main.GetItemDecimal(1, "retry_age");
            String ll_roundloan = wcf.Busscom.of_getattribloantype(state.SsWsPass, loantype_code, "reqround_factor");
            int roundloan = Convert.ToInt16(ll_roundloan);
            String ll_roundpay = wcf.Busscom.of_getattribloantype(state.SsWsPass, loantype_code, "payround_factor");
            int roundpay = Convert.ToInt16(ll_roundpay);

            if (ldc_periodsend > retry_age)
            {
                ldc_periodsend = retry_age;
            }
            //คำนวณเงินเดือนคงเหลือขั้นต่ำ
            if (ldc_minsalaamt < (ldc_salary * ldc_minpercsal))
            {
                ldc_minsalaamt = Math.Round(ldc_salary * ldc_minpercsal);
            }

            decimal salary_balance = ldc_salary - ldc_minsalaamt - ldc_mthcoop;
            decimal ldc_permamt;
            double li_maxperiod = Convert.ToDouble(ldc_periodsend);

            if (ldc_paymenttype == 1)
            {

                //คงต้น
                decimal ldc_temp = (ldc_periodsend * (ldc_intrate * (30 / 365)) + 1);
                ldc_permamt = (salary_balance * ldc_periodsend) / ldc_temp;

            }
            else
            { //คงยอด
                int li_fixcaltype = 1;//fixpaycal_type
                double ldc_permamttmp = 1, ldc_fr = 1, ldc_temp = 1;

                if (li_fixcaltype == 1)
                {
                    // ด/บ ทั้งปี / 12
                    // ldc_permamt = salary_balance * ((((1 + (ldc_intrate / 12)) ^ li_maxperiod) - 1) / ((ldc_intrate / 12) * ((1 + (ldc_intrate / 12)) ^ li_maxperiod)));
                    ldc_temp = Math.Log(1 + (Convert.ToDouble(ldc_intrate / 12)));
                    ldc_fr = Math.Exp(-li_maxperiod * ldc_temp);
                    ldc_permamttmp = (Convert.ToDouble(salary_balance) * (1 - ldc_fr)) / ((Convert.ToDouble(ldc_intrate) / 12));
                }
                else
                {
                    // ด/บ 30 วัน/เดือน
                    ldc_temp = Math.Log(1 + (Convert.ToDouble(ldc_intrate / (30 / 365))));
                    ldc_fr = Math.Exp(-li_maxperiod * ldc_temp);
                    ldc_permamttmp = (Convert.ToDouble(salary_balance) * (1 - ldc_fr)) / ((Convert.ToDouble(ldc_intrate) / (30 / 365)));

                }
                ldc_permamt = Convert.ToDecimal(ldc_permamttmp);
                decimal loan_credit = dw_main.GetItemDecimal(1, "loancredit_amt");
                if (ldc_permamt > loan_credit)
                {
                    ldc_permamt = loan_credit;
                }
            }

            //ตรวจสอบยอดขอกู้กลุ๋ม
            // JsGetloangrouppermissuesed();
            decimal loan_groupuse = 0;// dw_main.GetItemDecimal(1, "loangrpuse_amt");
            ldc_permamt = ldc_permamt - loan_groupuse;
            while (ldc_permamt > ldc_maxloan)
            {
                ldc_permamt = ldc_maxloan - 10;


            }
            if (ldc_permamt < 0) { ldc_permamt = 0; }
            ldc_permamt = Math.Round(ldc_permamt / roundloan) * roundloan;
            if (ldc_permamt > ldc_maxloan) { ldc_permamt = ldc_permamt - roundloan; }
            decimal period_payment = Math.Round((ldc_permamt / ldc_periodsend) / roundpay) * roundpay;



            dw_main.SetItemDecimal(1, "loanpermiss_amt", ldc_permamt);
            dw_main.SetItemDecimal(1, "loanrequest_amt", ldc_permamt);
            dw_main.SetItemDecimal(1, "period_payment", period_payment);
            dw_main.SetItemDecimal(1, "period_payamt", ldc_periodsend);

            Decimal intestimate_amt = of_calintestimatemain();
            dw_main.SetItemDecimal(1, "intestimate_amt", intestimate_amt);
            JsGenBuyshare();

            JsInsertRowcoll();

        }


        private void JsInsertRowcoll()
        {

            String loantype_code = dw_main.GetItemString(1, "loantype_code");
            Decimal ldc_shrstkvalue = dw_main.GetItemDecimal(1, "sharestk_value");
            Decimal ldc_permamt = dw_main.GetItemDecimal(1, "loanrequest_amt");
            Decimal ldc_useshare_flag = 0, ldc_useman_type = 0, ldc_useman_amt = 0;
            String sqlreqgrt = @"   SELECT COOP_ID,   
                                        LOANTYPE_CODE,   
                                        SEQ_NO,   
                                        MONEY_FROM,   
                                        MONEY_TO,   
                                        USESHARE_FLAG,   
                                        USEMAN_AMT,   
                                        USEMAN_TYPE,   
                                        USEMEMMAIN_AMT,   
                                        USEMEMCO_AMT,   
                                        USEMEM_OPERATION  
                                    FROM  LNLOANTYPEREQGRT    
                                    WHERE COOP_ID ='" + state.SsCoopControl + @"'    
                                  and LOANTYPE_CODE='" + loantype_code + @"'
                                and MONEY_FROM <=" + ldc_permamt + " and MONEY_TO >" + ldc_permamt + " ";
            Sdt dtreqgrt = WebUtil.QuerySdt(sqlreqgrt);
            if (dtreqgrt.Next())
            {
                ldc_useshare_flag = dtreqgrt.GetDecimal("USESHARE_FLAG");
                ldc_useman_type = dtreqgrt.GetDecimal("USEMAN_TYPE");
                ldc_useman_amt = dtreqgrt.GetDecimal("USEMAN_AMT");
            }
            if (ldc_useshare_flag == 1) // หุ้น
            {
                int row = dw_coll.RowCount + 1;
                dw_coll.InsertRow(row);
                //  if (ldc_percenshare == 0) { ldc_percenshare = 1; }
                dw_coll.Reset();
                dw_coll.InsertRow(1);
                dw_coll.SetItemString(row, "loancolltype_code", "02");
                dw_coll.SetItemString(row, "ref_collno", dw_main.GetItemString(1, "member_no"));
                dw_coll.SetItemString(row, "description", dw_main.GetItemString(1, "member_name"));
                dw_coll.SetItemDecimal(row, "coll_balance", ldc_shrstkvalue);
                dw_coll.SetItemDecimal(row, "use_amt", ldc_shrstkvalue);
                dw_coll.SetItemString(row, "coop_id", state.SsCoopControl);
            }
            else if (ldc_useman_type >= 1)
            {
                int row = dw_coll.RowCount;

                for (int i = 1; i <= ldc_useman_amt; i++)
                {
                    if (row < ldc_useman_amt)
                    {
                        dw_coll.InsertRow(1);
                    }
                    else { break; }
                }
            }
            else
            {
                for (int i = 1; i < dw_coll.RowCount; i++)
                {
                    DwUtil.DeleteLastRow(dw_coll);
                }
            }
            DwUtil.RetrieveDDDW(dw_coll, "coop_id", pbl, null);
        }

        /// <summary>
        /// ชื้อหุ้นเพิ่ม
        /// </summary>
        private void JsGenBuyshare()
        {
            decimal buyshare_amt = 0;
            string loantypereq_code = dw_main.GetItemString(1, "loantype_code");
            decimal loanrequest_amt = dw_main.GetItemDecimal(1, "loanrequest_amt");
            decimal sharestk_val = dw_main.GetItemDecimal(1, "sharestk_value");
            decimal shrstk_buytype = 0;
            decimal sharestk_percent = 0;

            string sqllntype = @"select  shrstk_buytype
                                        from lnloantype  where loantype_code ='" + loantypereq_code + @"' ";
            Sdt dtstk = WebUtil.QuerySdt(sqllntype);
            if (dtstk.Next())
            {
                shrstk_buytype = dtstk.GetDecimal("shrstk_buytype");
            }

            string sql = "select sharestk_percent  from lnloantypebuyshare where loantype_code ='" + loantypereq_code + "'";
            Sdt dttype = WebUtil.QuerySdt(sql);
            if (dttype.Next())
            {

                sharestk_percent = dttype.GetDecimal("sharestk_percent");

            }

            int li_count = dw_clear.RowCount;
            int li_index, li_clrstatus, li_paytype;
            Decimal ldc_principal_balance = 0, ldc_intestm, ldc_sumpay = 0;

            string ls_loantype = "";

            if (shrstk_buytype == 2)
            {
                for (li_index = 1; li_index <= li_count; li_index++)
                {

                    li_clrstatus = Convert.ToInt32(dw_clear.GetItemDecimal(li_index, "clear_status"));
                    ls_loantype = dw_clear.GetItemString(li_index, "loantype_code");

                    if (li_clrstatus == 0)
                    {
                        li_paytype = Convert.ToInt32(dw_clear.GetItemDecimal(li_index, "loanpayment_type"));
                        ldc_principal_balance = dw_clear.GetItemDecimal(li_index, "principal_balance");
                        ldc_principal_balance += ldc_principal_balance;

                    }
                }

                ldc_principal_balance += loanrequest_amt;
                decimal shareperc_val = (sharestk_val / ldc_principal_balance) * 100;
                if (shareperc_val <= sharestk_percent)
                {

                    shareperc_val = sharestk_percent - shareperc_val;
                    buyshare_amt = (ldc_principal_balance * shareperc_val) / 100;
                    decimal ldc_mod = buyshare_amt % 10;
                    if (ldc_mod > 0)
                    {

                        buyshare_amt = buyshare_amt + (10 - ldc_mod);
                    }

                    dw_otherclr.InsertRow(0);
                    dw_otherclr.SetItemString(1, "clrothertype_code", "SHR");
                    dw_otherclr.SetItemString(1, "clrother_desc", "ซื้อหุ้นเพิ่ม");
                    dw_otherclr.SetItemDecimal(1, "clear_status", 1);
                    dw_otherclr.SetItemDecimal(1, "clrother_amt", buyshare_amt);
                }
            }
            else
            {
                ldc_principal_balance += loanrequest_amt;
                decimal shareperc_val = (sharestk_val / ldc_principal_balance) * 100;
                if (shareperc_val <= sharestk_percent)
                {

                    shareperc_val = sharestk_percent - shareperc_val;
                    buyshare_amt = (ldc_principal_balance * shareperc_val) / 100;
                    decimal ldc_mod = buyshare_amt % 10;
                    if (ldc_mod > 0)
                    {
                        buyshare_amt = buyshare_amt + (10 - ldc_mod) - 10;

                    }

                    dw_otherclr.InsertRow(0);
                    dw_otherclr.SetItemString(1, "clrothertype_code", "SHR");
                    dw_otherclr.SetItemString(1, "clrother_desc", "ซื้อหุ้นเพิ่ม");
                    dw_otherclr.SetItemDecimal(1, "clear_status", 1);
                    dw_otherclr.SetItemDecimal(1, "clrother_amt", buyshare_amt);
                }
            }
        }

        private void JsCalperiodSend()
        {
            DateTime ldtm_retry = dw_main.GetItemDateTime(1, "retry_date");
            DateTime ldtm_lnreq = dw_main.GetItemDateTime(1, "loanrequest_date");
            string ls_loantype = dw_main.GetItemString(1, "loantype_code");
            Decimal ldc_maxperiod = dw_main.GetItemDecimal(1, "maxsend_payamt");
            Decimal ldc_maxperiodsend = 0;
            Decimal ldc_retryperiod = 0;
            String sqlStrperiod = @" SELECT retryloansend_time
                                    FROM LNLOANTYPE
                                    WHERE LOANTYPE_CODE='" + ls_loantype + "'";

            Sdt dt1 = WebUtil.QuerySdt(sqlStrperiod);
            if (dt1.Next())
            {
                try
                {
                    ldc_retryperiod = dt1.GetDecimal("retryloansend_time");
                }
                catch
                {
                    ldc_retryperiod = 0;
                }
            }
            if (ldc_retryperiod > 0)
            {
                Decimal retry_age = BusscomService.of_cal_yearmonth(state.SsWsPass, ldtm_lnreq, ldtm_retry) * 12;

                if (retry_age < ldc_maxperiod)
                {
                    ldc_maxperiodsend = retry_age + ldc_retryperiod;
                }
                //if (retry_age < ldc_maxperiod)
                //{
                //    ldc_maxperiod = retry_age;
                //}
            }
            if (ldc_maxperiodsend > ldc_maxperiod || ldc_maxperiodsend == 0) { ldc_maxperiodsend = ldc_maxperiod; }
            dw_main.SetItemDecimal(1, "maxsend_payamt", ldc_maxperiod);
            dw_main.SetItemDecimal(1, "period_payamt", ldc_maxperiodsend);
        }


        /// <summary>
        /// เช็คแลกกันค้ำ โดยใช้สิทธิ์กู้สูงสุดอกตาราง
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GenpermissCollLoop(object sender, EventArgs e)
        {
            //try
            //{
            //    string as_coopid = state.SsCoopId;
            //    String ls_memcoopid = dw_main.GetItemString(1, "memcoop_id");
            //    String ls_membtypedesc = dw_main.GetItemString(1, "membtype_desc");
            //    String member_no = dw_main.GetItemString(1, "member_no");
            //    DateTime ldtm_member;
            //    bool isChecked = Checkcollloop.Checked;
            //    if (isChecked)
            //    {
            //        try
            //        {
            //            ldtm_member = dw_main.GetItemDateTime(1, "member_date");
            //        }
            //        catch { ldtm_member = state.SsWorkDate; }
            //        ///<หาอายุงานของาสมาชิก>
            //        int memtime = 0;
            //        Decimal member_age = dw_main.GetItemDecimal(1, "member_age");
            //        if (member_age > 1)
            //        {
            //            memtime = Convert.ToInt32(BusscomService.of_cal_yearmonth(state.SsWsPass, ldtm_member, state.SsWorkDate) * 12);

            //        }
            //        else
            //        {

            //            memtime = Convert.ToInt32(member_age * 100);
            //        }
            //        String[] max_creditperiod = new String[5];
            //        Decimal per70 = new Decimal(0.7);
            //        Decimal per90 = new Decimal(0.9);
            //        Decimal loancredit_amt = 0;
            //        Decimal ldc_maxperiod = 0;

            //        string ls_loantype = dw_main.GetItemString(1, "loantype_code");

            //        ///< หาสิทธิ์กู้สูงสุด,งวดสูงสุด>                  
            //        max_creditperiod = shrlonService.CalloanpermissCollLoop(state.SsWsPass, ls_memcoopid, as_coopid, ls_loantype, member_no, memtime);

            //        loancredit_amt = Convert.ToDecimal(max_creditperiod[0]);
            //        ldc_maxperiod = Convert.ToDecimal(max_creditperiod[1]);

            //        dw_main.SetItemDecimal(1, "loancredit_amt", loancredit_amt);
            //        dw_main.SetItemDecimal(1, "maxsend_payamt", ldc_maxperiod);
            //        dw_main.SetItemDecimal(1, "period_payamt", ldc_maxperiod);

            //    }

            //    else { Jsmaxcreditperiod(); }
            //}
            //catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage("GenpermissCollLoop==>" + ex); }

        }

        /// <summary>
        /// สำหรับคำนวณดอกเบี้ยประมาณการสำหรับสัญญาใหม่
        /// </summary>
        /// <returns></returns>   
        public Decimal of_calintestimatemain()
        {
            string ls_continttabcode, ls_coopid;
            string ls_loantype;
            int li_continttype, li_intsteptype;
            Decimal ldc_apvamt, ldc_inttotal = 0, ldc_prncalint;
            Decimal ldc_contintrate, ldc_intincrease;
            DateTime ldtm_calintfrom, ldtm_calintto, ldtm_rcvfix, ldtm_estimate, adc_fixrate;
            try
            {
                // ข้อกำหนดเรื่องดอกเบี้ย
                li_continttype = Convert.ToInt32(dw_main.GetItemDecimal(1, "int_continttype"));
                ls_continttabcode = dw_main.GetItemString(1, "int_continttabcode");
                ldc_contintrate = dw_main.GetItemDecimal(1, "int_contintrate");
                ldc_intincrease = dw_main.GetItemDecimal(1, "int_contintincrease");
                li_intsteptype = Convert.ToInt32(dw_main.GetItemDecimal(1, "int_intsteptype"));
                ls_coopid = dw_main.GetItemString(1, "coop_id");
                ls_loantype = dw_main.GetItemString(1, "loantype_code");

                ldc_apvamt = dw_main.GetItemDecimal(1, "loanrequest_amt");
                ldc_prncalint = dw_main.GetItemDecimal(1, "loanrequest_amt");
                ldtm_rcvfix = dw_main.GetItemDateTime(1, "loanrcvfix_date");


                ldtm_estimate = ldtm_rcvfix.AddDays(31);


                // ถ้าไม่มียอดต้นที่จะคิด ด/บ เป็น 0
                if (ldc_prncalint <= 0) { return 0; }

                // ถ้าสถานะนี้เป็นไม่คิด ด/บ
                if (li_continttype == 0) { return 0; }


                // ถ้าวันที่คิดดอกเบี้ยถึงน้อยกว่าวันที่คิดดอกเบี้ยล่าสุด ด/บ 0
                if (ldtm_estimate <= ldtm_rcvfix) { return 0; }

                // ถ้าไม่ได้ set รูปแบบขั้นดอกเบี้ยไว้ set ให้เป็นจากยอดอนุมัติ
                if (li_intsteptype == null) { li_intsteptype = 1; }

                ldtm_calintfrom = ldtm_rcvfix;
                ldtm_calintto = ldtm_estimate;

                switch (li_continttype.ToString())
                {
                    case "1":	// ตาม rate ที่ระบุ

                        //// อัตราด/บเพิ่มลดพิเศษ
                        //inv_intsrv.of_setintincrease( 0 );

                        //ldc_inttotal = wcf.Shrlon.of_computeinterest(state.SsWsPass, state.SsCoopControl, ls_continttabcode, ldtm_calintto);
                        ldc_inttotal = Math.Round(wcf.Shrlon.of_computeintmonth(state.SsWsPass, state.SsCoopControl, ls_loantype, ldc_contintrate, ldtm_calintfrom, ldtm_calintto, ldc_prncalint));
                        break;
                    case "2":	// ตามตารางด/บที่ระบุ

                        // อัตราด/บเพิ่มลดพิเศษ
                        //inv_intsrv.of_setintincrease( ldc_intincrease );

                        // ตรวจว่าดูอัตราด/บจากยอดอนุมัติหรือคงเหลือ
                        //if (li_intsteptype == 1)
                        //{
                        ldc_inttotal = Math.Round(wcf.Shrlon.of_computeinterest2(state.SsWsPass, state.SsCoopControl, ls_continttabcode, ldtm_calintfrom, ldtm_calintto, ldc_prncalint, ldc_apvamt));
                        //}
                        //else
                        //{
                        //    ldc_inttotal = wcf.Shrlon.of_computeinterest3(state.SsWsPass, state.SsCoopControl, ls_continttabcode, ldtm_calintfrom, ldtm_calintto, ldc_prncalint);
                        //} 


                        //try
                        //{
                        //    DateTime startkeep_date = dw_main.GetItemDateTime(1, "startkeep_date");
                        //    DateTime loanrcvfix_date = dw_main.GetItemDateTime(1, "loanrcvfix_date");
                        //    short year = Convert.ToInt16(loanrcvfix_date.Year + 543);
                        //    short month = Convert.ToInt16(loanrcvfix_date.Month);
                        //    int day_amount = 31;

                        //    DateTime postingdate_bf = wcf.Busscom.of_getpostingdate2(state.SsWsPass, Convert.ToInt16(loanrcvfix_date.Year + 543), Convert.ToInt16(loanrcvfix_date.Month));
                        //    DateTime postingdate = wcf.Busscom.of_getpostingdate2(state.SsWsPass, Convert.ToInt16(postingdate_bf.Year + 543), Convert.ToInt16(postingdate_bf.Month + 1));
                        //    DateTime postingdate_af = wcf.Busscom.of_getpostingdate2(state.SsWsPass, Convert.ToInt16(postingdate.Year + 543), Convert.ToInt16(postingdate.Month + 1));
                        //    //  processdate = wcf.Busscom.of_getpostingdate(state.SsWsPass, ldtm_loanreceive_af);


                        //    ////จ่ายเงินกู้หลังเรียกเก็บหรือไม่
                        //    //if (loanrcvfix_date < postingdate && loanrcvfix_date < postingdate_bf)
                        //    //{

                        //    //    day_amount = postingdate.DayOfYear - loanrcvfix_date.DayOfYear;

                        //    //}
                        //    //else if (loanrcvfix_date > postingdate_bf)
                        //    //{

                        //    //    postingdate = wcf.Busscom.of_getpostingdate2(state.SsWsPass, Convert.ToInt16(postingdate_af.Year + 543), Convert.ToInt16(postingdate_af.Month));

                        //    //    day_amount = postingdate.DayOfYear - loanrcvfix_date.DayOfYear;

                        //    //}
                        //    //else
                        //    //{

                        //    //    day_amount = postingdate_af.DayOfYear - loanrcvfix_date.DayOfYear;
                        //    //}
                        //    day_amount = startkeep_date.DayOfYear - loanrcvfix_date.DayOfYear;
                        //    ldc_inttotal = ldc_apvamt * ldc_contintrate * day_amount / 365;
                        //    ldc_inttotal = wcf.Shrlon.of_roundmoney(state.SsWsPass, ldc_inttotal);
                        //}
                        //catch { }

                        break;

                }




            }
            catch (Exception ex)
            {
                //LtServerMessage.Text = WebUtil.ErrorMessage("of_calintestimatemain==+>" + ex); 
            }

            return ldc_inttotal;

        }


        //private void JsRunProcess()
        //{

        //    // --- Page Arguments
        //    try
        //    {
        //        app = Request["app"].ToString();
        //    }
        //    catch { }
        //    if (app == null || app == "")
        //    {
        //        app = state.SsApplication;
        //    }
        //    try
        //    {
        //        //gid = Request["gid"].ToString();
        //        gid = "LNNORM_DAILY";
        //    }
        //    catch { }
        //    try
        //    {
        //        //rid = Request["rid"].ToString();
        //        rid = "LNNORM_DAILY15";
        //    }
        //    catch { }

        //    String doc_no = dw_main.GetItemString(1, "loanrequest_docno");

        //    if (x == 2)
        //    {
        //        doc_no = reqdoc_no;
        //    }
        //    if (doc_no == null || doc_no == "")
        //    {
        //        return;
        //    }

        //    //String ls_xmlmain = dw_main.Describe("DataWindow.Data.XML");
        //    //String ls_format = "CAT";
        //    //short li_membtime = 0; Decimal ldc_right25 = 0; Decimal ldc_right33 = 0; Decimal ldc_right35 = 0; Decimal ldc_right26 = 0; Decimal ldc_right40 = 0;
        //    //int re = shrlonService.of_print_callpermiss(state.SsWsPass, ls_xmlmain, ls_format, ref li_membtime, ref ldc_right25, ref ldc_right33, ref ldc_right35, ref ldc_right26, ref ldc_right40);
        //    //Decimal li_membtime_ = li_membtime;
        //    //Decimal ldc_right25_ = ldc_right25;
        //    //Decimal ldc_right33_ = ldc_right33;
        //    //Decimal ldc_right35_ = ldc_right35;
        //    //Decimal ldc_right26_ = ldc_right26;
        //    //Decimal ldc_right40_ = ldc_right40;
        //    //string loan26 = dw_main.GetItemString(1, "loantype_code");
        //    //if (loan26 == "26")
        //    //{
        //    //    decimal right26 = dw_main.GetItemDecimal(1, "loancredit_amt");
        //    //    ldc_right26_ = right26;
        //    //    ldc_right33_ = 0;
        //    //    ldc_right35_ = 0;
        //    //    ldc_right25_ = 0;
        //    //}


        //    //แปลง Criteria ให้อยู่ในรูปแบบมาตรฐาน.
        //    ReportHelper lnv_helper = new ReportHelper();
        //    lnv_helper.AddArgument(doc_no, ArgumentType.String);
        //    //lnv_helper.AddArgument(li_membtime_.ToString(), ArgumentType.Number);
        //    //lnv_helper.AddArgument(ldc_right25_.ToString(), ArgumentType.Number);
        //    //lnv_helper.AddArgument(ldc_right33_.ToString(), ArgumentType.Number);
        //    //lnv_helper.AddArgument(ldc_right35_.ToString(), ArgumentType.Number);
        //    //lnv_helper.AddArgument(ldc_right26_.ToString(), ArgumentType.Number);
        //    //lnv_helper.AddArgument(ldc_right40_.ToString(), ArgumentType.Number);

        //    //ชื่อไฟล์ PDF = YYYYMMDDHHMMSS_<GID>_<RID>.PDF

        //    String pdfFileName = DateTime.Now.ToString("yyyyMMddHHmmss", WebUtil.EN);
        //    pdfFileName += "_" + gid + "_" + rid + ".pdf";
        //    pdfFileName = pdfFileName.Trim();

        //    //ส่งให้ ReportService สร้าง PDF ให้ {โดยปกติจะอยู่ใน C:\GCOOP\Saving\PDF\}.
        //    try
        //    {
        //        Saving.WsReport.Report lws_report = wcf.Report;
        //        String criteriaXML = lnv_helper.PopArgumentsXML();
        //        this.pdf = lws_report.GetPDFURL(state.SsWsPass) + pdfFileName;
        //        String li_return = lws_report.Run(state.SsWsPass, app, gid, rid, criteriaXML, pdfFileName);
        //        if (li_return == "true")
        //        {
        //            HdOpenIFrame.Value = "True";
        //            HdcheckPdf.Value = "True";

        //        }
        //        else if (li_return != "true")
        //        {
        //            HdcheckPdf.Value = "False";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LtServerMessage.Text = WebUtil.ErrorMessage(ex);
        //        return;
        //    }
        //    Session["pdf"] = pdf;
        //    //PopupReport();


        //}
        //private void JsRunProcessInvoice()
        //{

        //    // --- Page Arguments
        //    try
        //    {
        //        app = Request["app"].ToString();
        //    }
        //    catch { }
        //    if (app == null || app == "")
        //    {
        //        app = state.SsApplication;
        //    }
        //    try
        //    {
        //        //gid = Request["gid"].ToString();
        //        gid = "LNNORM_DAILY";
        //    }
        //    catch { }
        //    try
        //    {
        //        //rid = Request["rid"].ToString();
        //        rid = "LNNORM_DAILY13";
        //    }
        //    catch { }

        //    String doc_no = dw_main.GetItemString(1, "loanrequest_docno");

        //    if (x == 2)
        //    {
        //        doc_no = reqdoc_no;
        //    }

        //    if (doc_no == null || doc_no == "")
        //    {
        //        return;
        //    }




        //    //แปลง Criteria ให้อยู่ในรูปแบบมาตรฐาน.
        //    ReportHelper lnv_helper = new ReportHelper();
        //    lnv_helper.AddArgument(doc_no, ArgumentType.String);

        //    //ชื่อไฟล์ PDF = YYYYMMDDHHMMSS_<GID>_<RID>.PDF

        //    String pdfFileName = DateTime.Now.ToString("yyyyMMddHHmmss", WebUtil.EN);
        //    pdfFileName += "_" + gid + "_" + rid + ".pdf";
        //    pdfFileName = pdfFileName.Trim();

        //    //ส่งให้ ReportService สร้าง PDF ให้ {โดยปกติจะอยู่ใน C:\GCOOP\Saving\PDF\}.
        //    try
        //    {
        //        Saving.WsReport.Report lws_report = wcf.Report;
        //        String criteriaXML = lnv_helper.PopArgumentsXML();
        //        this.pdf = lws_report.GetPDFURL(state.SsWsPass) + pdfFileName;
        //        String li_return = lws_report.Run(state.SsWsPass, app, gid, rid, criteriaXML, pdfFileName);
        //        if (li_return == "true")
        //        {
        //            HdOpenIFrame.Value = "True";
        //            HdcheckPdf.Value = "True";

        //        }
        //        else if (li_return != "true")
        //        {
        //            HdcheckPdf.Value = "False";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LtServerMessage.Text = WebUtil.ErrorMessage(ex);
        //        return;
        //    }
        //    Session["pdfinvoice"] = pdf;
        //    //PopupReport();


        //}
        //public void JspPopupReport()
        //{
        //    //เด้ง Popup ออกรายงานเป็น PDF.
        //    //String pop = "Gcoop.OpenPopup('http://localhost/GCOOP/WebService/Report/PDF/20110223093839_shrlonchk_SHRLONCHK001.pdf')";

        //    String pop = "Gcoop.OpenPopup('" + Session["pdf"].ToString() + "')";
        //    ClientScript.RegisterClientScriptBlock(this.GetType(), "DsReport", pop, true);

        //}
        //public void JspPopupReportInvoice()
        //{
        //    //เด้ง Popup ออกรายงานเป็น PDF.
        //    //String pop = "Gcoop.OpenPopup('http://localhost/GCOOP/WebService/Report/PDF/20110223093839_shrlonchk_SHRLONCHK001.pdf')";

        //    String pop = "Gcoop.OpenPopup('" + Session["pdfinvoice"].ToString() + "')";
        //    ClientScript.RegisterClientScriptBlock(this.GetType(), "DsReport", pop, true);

        //}
        private void JsPostreturn()
        {
            String ls_loantype = dw_main.GetItemString(1, "loantype_code");

            Decimal useamt = Convert.ToDecimal(HUseamt.Value);
            Decimal loancredit_amt = dw_main.GetItemDecimal(1, "loancredit_amt");
            Decimal Maxloancredit_amt = 0;
            Decimal ldc_maxcredit = loancredit_amt + useamt;
            int row = Convert.ToInt16(HdRowNumber.Value);
            Decimal ldc_maxperiod = 0;
            Decimal ldc_max = 0;
            string loancolltype_code = dw_coll.GetItemString(row, "loancolltype_code");


            // Decimal ldc_maxcredit = loancredit_amt;
            String sqlStrperiod = @"  SELECT COOP_ID,   
                                        LOANTYPE_CODE,   
                                        SEQ_NO,   
                                        MONEY_FROM,
                                        MONEY_TO,
                                        MAX_PERIOD
                                    FROM LNLOANTY PEPERIOD
                                    WHERE  COOP_ID ='" + state.SsCoopControl + @"'  
                                    and LOANTYPE_CODE='" + ls_loantype + @"' 
                                    and MONEY_FROM <=" + ldc_maxcredit + " and MONEY_TO >" + ldc_maxcredit + " ";

            Sdt dtperiod = WebUtil.QuerySdt(sqlStrperiod);

            if (dtperiod.Next())
            {

                ldc_maxperiod = dtperiod.GetDecimal("MAX_PERIOD");
                ldc_max = dtperiod.GetDecimal("MONEY_TO");
            }

            if (loancolltype_code != "03")
            {
                if (ldc_maxcredit > ldc_max)
                {
                    Maxloancredit_amt = ldc_max;

                    dw_main.SetItemDecimal(1, "loancredit_amt", Maxloancredit_amt);
                }
                else
                {
                    dw_main.SetItemDecimal(1, "loancredit_amt", ldc_maxcredit);
                }
            }
            else { dw_main.SetItemDecimal(1, "loancredit_amt", ldc_maxcredit); }

            dw_main.SetItemDecimal(1, "maxsend_payamt", ldc_maxperiod);

            dw_main.SetItemDecimal(1, "period_payamt", ldc_maxperiod);
            JsLoanpaymenttype();
        }


        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            XmlConfigService xml = new XmlConfigService();
            if (xml.ShrlonPrintMode == 0)
            {
                JspopupDeptReport();
            }
            else
            {
                JspopupDeptReport(true, xml.ShrlonPrintMode);
            }
        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            XmlConfigService xml = new XmlConfigService();
            if (xml.ShrlonPrintMode == 0)
            {
                JspopupLoanReport();
            }
            else
            {
                JspopupLoanReport(true, xml.ShrlonPrintMode);
            }
        }

        protected void SetAcci_dept()
        {
            string dept_acc = "";
            try
            {
                string memberNo = dw_main.GetItemString(1, "member_no");
                String strSQL = "";
                strSQL = @"SELECT DPDEPTMASTER.DEPTACCOUNT_NO,   
         DPDEPTMASTER.MEMBER_NO,   
         DPDEPTMASTER.DEPTACCOUNT_NAME,   
         DPDEPTMASTER.DEPTTYPE_CODE,   
         MBMEMBMASTER.MEMB_NAME,   
         MBMEMBMASTER.MEMB_SURNAME,   
         MBMEMBMASTER.MEMBGROUP_CODE,   
         DPDEPTMASTER.DEPT_OBJECTIVE,   
         DPDEPTMASTER.DEPTCLOSE_STATUS,   
         DPDEPTTYPE.DEPTTYPE_GROUP,   
         DPDEPTMASTER.PRNCBAL  
    FROM DPDEPTMASTER,   
         MBMEMBMASTER,   
         DPDEPTTYPE  
   WHERE ( DPDEPTMASTER.MEMBER_NO = MBMEMBMASTER.MEMBER_NO ) and  
         ( DPDEPTMASTER.DEPTTYPE_CODE = DPDEPTTYPE.DEPTTYPE_CODE ) and  
         ( DPDEPTMASTER.MEMCOOP_ID = DPDEPTTYPE.COOP_ID )  and DPDEPTMASTER.DEPTCLOSE_STATUS=0 and DPDEPTMASTER.DEPTTYPE_CODE=10 and
         ( DPDEPTMASTER.MEMBER_NO = '" + memberNo + "')  ORDER BY DPDEPTMASTER.DEPTACCOUNT_NO ASC ";
                Sdt dtdept = WebUtil.QuerySdt(strSQL);

                if (dtdept.Next())
                {
                    int rowCount = dtdept.GetRowCount();


                    for (int x = 0; x < rowCount; x++)
                    {

                        dept_acc = dtdept.Rows[x]["DEPTACCOUNT_NO"].ToString();
                        if (x == 0)
                        {
                            dw_main.SetItemString(1, "expense_accid", dept_acc);
                        }
                    }
                }


            }
            catch { dept_acc = ""; }

        }

    }



}


/// 
/// ประเภทเงินกู้  lnloantype
/// ประเภทสมาชิก lnloanmbtype
/// กู้ฉุกเฉิน           10   salary X 3   max_period = 12 ใช้หุ้นค้ำ
/// กู้สามัญหุ้น/เงินฝากค้ำ  20   กู้หุ้นได้ 100%  ,เงินฝาก 100%    max_period  = 96  <800,000 ||  max_period  = 120  >  800,000 <2,000,000 
/// กู้สามัญบุคคลค้ำ      21   ตามตาราง สิทธิ์กู้  lnloantypecustum  ตามตาราง สิทธิํค้ำ lngrpmanprtpermtdet
///                       max_period  = 96  <800,000 ||  max_period  = 120  >  800,000 <2,000,000 
/// 
/// 
/// 
/// 
///