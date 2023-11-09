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
    public partial class w_sheet_sl_loan_requestment_coll : PageWebSheet, WebSheet
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
        protected String jsStartkeep;
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
            jsStartkeep = WebUtil.JsPostBack(this, "jsStartkeep");


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
            this.ConnectSQLCA();
            Ltjspopup.Text = "";
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
                Session["strItemchange"] = null;
                dw_main.InsertRow(0);

                dw_main.SetItemString(1, "memcoop_id", state.SsCoopControl);
                RetreiveDDDW();


                JsChangeStartkeep();
                tDwMain.Eng2ThaiAllRow();

                CbCheckcoop.Checked = false;
                Checkcollloop.Checked = false;
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
            else if (eventArg == "jsStartkeep")
            {
                JsStartkeep();
            }



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
                string re = wcf.Shrlon.of_printloancoll(state.SsWsPass, reqdoc_no, fromset, state.SsCoopControl, member_no, ref as_xml);
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage("ตรวจสอบเครื่องพิมพ์" + ex); }
        }

        private void JspopupCollReport(bool isJsPrint)
        {
            reqdoc_no = txt_reqNo.Text;
            member_no = txt_member_no.Text;
            try
            {
                //                String sql = @"
                //                    SELECT 
                //	                    MBUCFPRENAME.PRENAME_DESC,   
                //	                    MBMEMBMASTER.MEMB_NAME,   
                //	                    MBMEMBMASTER.MEMB_SURNAME,   
                //	                    LNREQLOAN.LOANRCVFIX_DATE,   
                //	                    LNLOANTYPE.LOANTYPE_DESC,   
                //	                    MBUCFMEMBGROUP_A.MEMBGROUP_CODE,   
                //	                    MBMEMBMASTER.MEMBER_NO,   
                //	                    MBUCFMEMBTYPE.MEMBTYPE_DESC,   
                //	                    MBMEMBMASTER.POSITION_DESC,   
                //	                    MBMEMBMASTER.LEVEL_CODE,   
                //	                    MBUCFMEMBGROUP_A.MEMBGROUP_DESC,   
                //	                    MBUCFMEMBGROUP_B.MEMBGROUP_DESC,   
                //	                    LNREQLOAN.LOANREQUEST_AMT,   
                //	                    LNCFLOANINTRATEDET.INTEREST_RATE,   
                //	                    LNREQLOAN.PERIOD_PAYAMT,   
                //	                    LNUCFLOANOBJECTIVE.LOANOBJECTIVE_DESC,   
                //	                    LNLOANTYPE.LOANTYPE_CODE,   
                //	                    LNREQLOAN.PERIOD_PAYMENT ,
                //	                    (select b.prename_desc||'  '||a.memb_name||'   '||a.memb_surname from mbmembmaster a,mbucfprename b where a.prename_code =b.prename_code and a.coop_id =lnreqloan.memcoop_id and a.member_no =lnreqloan.member_no)as loanreq_name,
                //	                    (select c.membtype_desc from mbmembmaster a,mbucfprename b,mbucfmembtype c where a.prename_code =b.prename_code and a.coop_id =lnreqloan.memcoop_id and a.member_no =lnreqloan.member_no and a.coop_id = c.coop_id and a.membtype_code =c.membtype_code)as loanreq_membtype,
                //	                    (select a.position_desc from mbmembmaster a,mbucfprename b where a.prename_code =b.prename_code and a.coop_id =lnreqloan.memcoop_id and a.member_no =lnreqloan.member_no)as loanreq_position,
                //	                    (select d.membgroup_desc from mbmembmaster a,mbucfprename b,mbucfmembgroup c,mbucfmembgroup d where a.prename_code =b.prename_code and a.coop_id =lnreqloan.memcoop_id and a.member_no =lnreqloan.member_no and a.coop_id =c.coop_id and a.membgroup_code =c.membgroup_code and c.coop_id =d.coop_id and c.membgroup_control =d.membgroup_code )as loanreq_membgroup_a,
                //	                    (select c.membgroup_desc from mbmembmaster a,mbucfprename b,mbucfmembgroup c,mbucfmembgroup d where a.prename_code =b.prename_code and a.coop_id =lnreqloan.memcoop_id and a.member_no =lnreqloan.member_no and a.coop_id =c.coop_id and a.membgroup_code =c.membgroup_code and c.coop_id =d.coop_id and c.membgroup_control =d.membgroup_code )as loanreq_membgroup_b,
                //	                    (select a.level_code from mbmembmaster a,mbucfprename b where a.prename_code =b.prename_code and a.coop_id =lnreqloan.memcoop_id and a.member_no =lnreqloan.member_no)as loanreq_level,
                //	                    lnreqloan.member_no as loanreq_memberno,
                //	                    lncontmaster.loancontract_no,
                //	                    lnreqloancoll.seq_no,'  ' as age ,'        'as mem_no
                //                    FROM 
                //	                    LNREQLOAN,   
                //	                    MBMEMBMASTER,   
                //	                    MBUCFPRENAME,   
                //	                    LNCONTMASTER,   
                //	                    LNLOANTYPE,   
                //	                    MBUCFMEMBGROUP MBUCFMEMBGROUP_A,   
                //	                    MBUCFMEMBGROUP MBUCFMEMBGROUP_B,   
                //	                    MBUCFMEMBTYPE,   
                //	                    LNCFLOANINTRATE,   
                //	                    LNCFLOANINTRATEDET,   
                //	                    LNUCFLOANOBJECTIVE,
                //	                    lnreqloancoll
                //                    WHERE 
                //	                    ( lnreqloan.coop_id = lncontmaster.coop_id (+)) and  
                //	                    ( lnreqloan.loanrequest_docno = lncontmaster.loanrequest_docno (+)) and  
                //	                    ( MBMEMBMASTER.PRENAME_CODE = MBUCFPRENAME.PRENAME_CODE ) and  
                //	                    ( LNREQLOANCOLL.COOP_ID = MBMEMBMASTER.COOP_ID ) and  
                //	                    ( LNREQLOANCOLL.REF_COLLNO = MBMEMBMASTER.MEMBER_NO ) and  
                //	                    ( LNREQLOAN.COOP_ID = LNLOANTYPE.COOP_ID ) and  
                //	                    ( LNREQLOAN.LOANTYPE_CODE = LNLOANTYPE.LOANTYPE_CODE ) and  
                //	                    ( MBMEMBMASTER.COOP_ID = MBUCFMEMBGROUP_A.COOP_ID ) and  
                //	                    ( MBMEMBMASTER.MEMBGROUP_CODE = MBUCFMEMBGROUP_A.MEMBGROUP_CODE ) and  
                //	                    ( MBUCFMEMBGROUP_A.COOP_ID = MBUCFMEMBGROUP_B.COOP_ID ) and  
                //	                    ( MBUCFMEMBGROUP_A.MEMBGROUP_CONTROL = MBUCFMEMBGROUP_B.MEMBGROUP_CODE ) and  
                //	                    ( MBMEMBMASTER.COOP_ID = MBUCFMEMBTYPE.COOP_ID ) and  
                //	                    ( MBMEMBMASTER.MEMBTYPE_CODE = MBUCFMEMBTYPE.MEMBTYPE_CODE ) and  
                //	                    ( LNCFLOANINTRATEDET.COOP_ID = LNCFLOANINTRATE.COOP_ID ) and  
                //	                    ( LNCFLOANINTRATEDET.LOANINTRATE_CODE = LNCFLOANINTRATE.LOANINTRATE_CODE ) and  
                //	                    ( LNREQLOAN.INT_CONTINTTABCODE = LNCFLOANINTRATE.LOANINTRATE_CODE ) and  
                //	                    ( LNREQLOAN.COOP_ID = LNCFLOANINTRATE.COOP_ID ) and  
                //	                    ( LNREQLOAN.COOP_ID = LNUCFLOANOBJECTIVE.COOP_ID ) and  
                //	                    ( LNREQLOAN.LOANOBJECTIVE_CODE = LNUCFLOANOBJECTIVE.LOANOBJECTIVE_CODE ) and  
                //	                    ( ( LNREQLOAN.COOP_ID = '" + state.SsCoopControl + @"' ) AND  
                //	                    ( LNREQLOAN.LOANREQUEST_DOCNO = '" + reqdoc_no + @"' ) )    and
                //	                    lnreqloan.coop_id = lnreqloancoll.coop_id and
                //	                    lnreqloan.loanrequest_docno = lnreqloancoll.loanrequest_docno and
                //	                    lnreqloancoll.loancolltype_code ='01'
                //                ";

                //                DataTable data = WebUtil.Query(sql);
                //                //Printing.Print(this, "Slip/shrlon/loanrequest_coll.aspx", data, 1);
                XmlConfigService x = new XmlConfigService();
                int printMode = x.ShrlonPrintMode;
                Printing.ShrlonPrintCollReport(this, state.SsCoopId, reqdoc_no, printMode);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ตรวจสอบเครื่องพิมพ์: " + ex.Message);
            }
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
                string re = wcf.Shrlon.of_printloancollagree(state.SsWsPass, reqdoc_no, fromset, state.SsCoopControl, member_no, ref as_xml);
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage("ตรวจสอบเครื่องพิมพ์" + ex); }
        }

        private void JspopupAgreeCollReport(bool isJsPrint, int printMode)
        {
            reqdoc_no = txt_reqNo.Text;
            member_no = txt_member_no.Text;
            try
            {
                Printing.ShrlonPrintAgreeCollReport(this, state.SsCoopId, reqdoc_no, printMode);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ตรวจสอบเครื่องพิมพ์: " + ex.Message);
            }
        }

        private void JspopupAgreeLoanReport()
        {
            //เด้ง Popup ออกรายงานเป็น PDF.
            //String pop = "Gcoop.OpenPopup('http://localhost/GCOOP/WebService/Report/PDF/20110223093839_shrlonchk_SHRLONCHK001.pdf')";

            //String pop = "Gcoop.OpenPopup('" + Session["AgreeLoan"].ToString() + "')";
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
                string re = wcf.Shrlon.of_printloanagree(state.SsWsPass, reqdoc_no, fromset, state.SsCoopControl, member_no, ref as_xml);
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage("ตรวจสอบเครื่องพิมพ์" + ex); }

        }

        private void JspopupAgreeLoanReport(bool isJsPrint, int printMode)
        {
            reqdoc_no = txt_reqNo.Text;
            try
            {
                Printing.ShrlonPrintAgreeLoanReport(this, state.SsCoopId, reqdoc_no, printMode);
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

        private void JspopupDeptReport()
        {
            //เด้ง Popup ออกรายงานเป็น PDF.
            //String pop = "Gcoop.OpenPopup('http://localhost/GCOOP/WebService/Report/PDF/20110223093839_shrlonchk_SHRLONCHK001.pdf')";

            //String pop = "Gcoop.OpenPopup('" + Session["dept"].ToString() + "')";
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "DsReport", pop, true);
            //reqdoc_no = DwMain.GetItemString(1, "loanrequest_docno");
            //member_no = DwMain.GetItemString(1, "member_no");
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
                string re = wcf.Shrlon.of_printloandept(state.SsWsPass, reqdoc_no, fromset, state.SsCoopControl, member_no, ref as_xml);
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage("ตรวจสอบเครื่องพิมพ์" + ex); }
        }

        private void JspopupReportslipfin()
        {
            //เด้ง Popup ออกรายงานเป็น PDF.
            //String pop = "Gcoop.OpenPopup('http://localhost/GCOOP/WebService/Report/PDF/20110223093839_shrlonchk_SHRLONCHK001.pdf')";

            String pop = "Gcoop.OpenPopup('" + Session["pdfslipfin"].ToString() + "')";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "DsReport", pop, true);
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
            catch { coop_id = state.SsCoopControl; }
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
            catch { coop_id = state.SsCoopControl; }
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
            catch { coop_id = state.SsCoopControl; }
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
            catch { coop_id = state.SsCoopControl; }
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

        private void JsrunProcessDeptReport()
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
            catch { coop_id = state.SsCoopControl; }
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
            catch { coop_id = state.SsCoopControl; }
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
            catch { coop_id = state.SsCoopControl; }
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

        private void JsPostreturn()
        {
            String ls_loantype = dw_main.GetItemString(1, "loantype_code");

            Decimal useamt = Convert.ToDecimal(HUseamt.Value);
            Decimal loancredit_amt = dw_main.GetItemDecimal(1, "loancredit_amt");
            Decimal Maxloancredit_amt = 0;
            Decimal ldc_maxcredit = loancredit_amt + useamt;
            String sqlStrperiod = @"  SELECT COOP_ID,   
                                        LOANTYPE_CODE,   
                                        SEQ_NO,   
                                        MONEY_FROM,
                                        MONEY_TO,
                                        MAX_PERIOD
                                    FROM LNLOANTYPEPERIOD
                                    WHERE  COOP_ID ='" + state.SsCoopControl + @"'  
                                    and LOANTYPE_CODE='" + ls_loantype + @"'  ";

            Sdt dtperiod = WebUtil.QuerySdt(sqlStrperiod);
            Decimal ldc_maxperiod = 0;
            Decimal ldc_max = 0;
            if (dtperiod.Next())
            {

                ldc_maxperiod = dtperiod.GetDecimal("MAX_PERIOD");
                ldc_max = dtperiod.GetDecimal("MONEY_TO");
            }
            if (ldc_maxcredit > ldc_max)
            {
                Maxloancredit_amt = ldc_max;

                dw_main.SetItemDecimal(1, "loancredit_amt", Maxloancredit_amt);
            }
            else
            {
                dw_main.SetItemDecimal(1, "loancredit_amt", ldc_maxcredit);
            }
            //   dw_main.SetItemDecimal(1, "maxsend_payamt", ldc_maxperiod);
            //   dw_main.SetItemDecimal(1, "period_payamt", ldc_maxperiod);

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
                dwloantype_code.SetFilter("LNLOANTYPE.LOANGROUP_CODE   ='02' and LNLOANMBTYPE.MEMBTYPE_CODE ='" + membtypecode + "' ");
                dwloantype_code.Filter();
                dw_main.SetItemString(1, "loantype_code", loantype);
            }


        }

        /// <summary>
        ///บันทึกใบคำขอ
        /// </summary>
        public void SaveWebSheet()
        {


            Decimal loanrequest_amt = dw_main.GetItemDecimal(1, "loanrequest_amt");
            Decimal sharestk_value = dw_main.GetItemDecimal(1, "sharestk_value");
            Decimal salary_amt = dw_main.GetItemDecimal(1, "salary_amt");
            loantype = dw_main.GetItemString(1, "loantype_code");
            Decimal loanreque_sharestk = 800000 + sharestk_value;
            int RowCount = dw_coll.RowCount;
            string mem_refcoll = "", loancolltype_code = "";
            int mem_collage1 = 0, mem_collage2 = 0;
            Decimal age = 0, use_amt = 0, total_use_amt = 0;
            DateTime loanrcvfix_date = dw_main.GetItemDateTime(1, "loanrcvfix_date");
            Sta ta = new Sta(sqlca.ConnectionString);

            string re = wcf.InterPreter.Checkcollpermiss(loanrequest_amt, sharestk_value, salary_amt, RowCount, loantype);
            if (re != "1")
            {

                LtServerMessage.Text = WebUtil.ErrorMessage(re);

            }
            else
            {

                for (int i = 0; i < RowCount; i++)
                {
                    use_amt = dw_coll.GetItemDecimal(i + 1, "use_amt");
                    total_use_amt += use_amt;


                }
                for (int i = 0; i < RowCount; i++)
                {

                    loancolltype_code = dw_coll.GetItemString(i + 1, "loancolltype_code");
                    if (loancolltype_code == "01")
                    {
                        mem_refcoll = dw_coll.GetItemString(i + 1, "ref_collno");

                        String[] mem_coll = shrlonService.GetMembercoll(state.SsWsPass, state.SsCoopControl, mem_refcoll, state.SsWorkDate);
                        age = Convert.ToDecimal(mem_coll[4]);
                        if (age >= 120)
                        {
                            mem_collage1 = 1;
                            break;
                        }
                        else
                        {
                            mem_collage1 = 2;
                        }
                    }

                }
                if (loanrequest_amt > loanreque_sharestk && mem_collage1 == 2)
                {

                    LtServerMessage.Text = WebUtil.ErrorMessage("วงเงินขอกู้มากกว่าหรือเท่ากับ  800,000 + หุ้น ขึ้นไปต้องใช้ผู้ค้ำประกันอายุงาน 10 ปี 1 คน");
                }
                else
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

                        try
                        {
                            for (int i = 1; i <= dw_coll.RowCount; i++)
                            {

                                dw_coll.SetItemString(i, "coop_id", state.SsCoopId);
                                dw_coll.SetItemString(i, "refcoop_id", state.SsCoopControl);
                            }

                            dwColl_XML = dw_coll.Describe("DataWindow.Data.XML");

                        }
                        catch { dwColl_XML = ""; }
                        try
                        {
                            for (int i = 1; i <= dw_clear.RowCount; i++)
                            {
                                dw_clear.SetItemString(i, "coop_id", state.SsCoopId);
                                dw_clear.SetItemString(i, "concoop_id", state.SsCoopControl);
                                String ls_contno = dw_clear.GetItemString(i, "loancontract_no");
                                Decimal clear_status = dw_clear.GetItemDecimal(i, "clear_status");
                                if (clear_status == 1)
                                {
                                    try
                                    {
                                        String sql = @" UPDATE LNCONTMASTER       
                                          SET    
                                            COMPOUND_STATUS = 1,   
                                            COMPOUND_DUEDATE = to_date('" + loanrcvfix_date.ToString("ddMMyyyy") + @"','ddmmyyyy'),   
                                            COMPOUND_PAYSTATUS = -1
                                          WHERE LOANCONTRACT_NO='" + ls_contno + "' and COOP_ID ='" + state.SsCoopControl + "'";
                                        ta.Exe(sql);
                                    }
                                    catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
                                }
                            }
                            ta.Close();

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

                        string expense_branch = "", expense_code = "", expense_accid = "";
                        try
                        {
                            expense_accid = dw_main.GetItemString(1, "expense_accid");
                        }
                        catch { expense_accid = ""; }

                        try
                        {
                            expense_branch = dw_main.GetItemString(1, "expense_branch");
                        }
                        catch { expense_branch = ""; }
                        try
                        {
                            expense_code = dw_main.GetItemString(1, "expense_code");
                        }
                        catch { expense_code = ""; }

                        String period_payamt = dw_main.GetItemDecimal(1, "period_payamt").ToString("0.00");
                        bool is_point1 = period_payamt.IndexOf(".00") < 0;

                        String period_payment = dw_main.GetItemDecimal(1, "period_payment").ToString("0.00");
                        bool is_point2 = period_payment.IndexOf(".00") < 0;


                        if (expense_code == "CHQ" && expense_branch == "" || total_use_amt < loanrequest_amt || is_point1 == true || is_point2 == true || expense_accid == "" || expense_accid == null)
                        {
                            if (expense_branch == "")
                            {
                                LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาเลือกสาขาธนาคาร");
                            }

                            else if (expense_accid == "" || expense_accid == null)
                            {
                                LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาเลือกบัญชีโอนเงินเข้าฝาก");
                            }

                            else if (is_point1 == true) { LtServerMessage.Text = WebUtil.ErrorMessage("จำนวนงวดเป็นทศนิยม =" + period_payamt); }
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

                            //JsrunProcessAgreeLoan();
                            //JsrunProcessCollReport();
                            //JsrunProcessAgreeColl();
                            //JsrunProcessLoan();
                            LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเรียบร้อยแล้ว");
                            String ls_loantype = dw_main.GetItemString(1, "loantype_code");
                            Session["loantypecoll"] = ls_loantype;
                            JsReNewPage();
                            LinkButton1.Visible = true;
                            LinkButton2.Visible = true;
                            LinkButton3.Visible = true;
                            LinkButton4.Visible = true;
                        }
                    }
                    catch (Exception ex)
                    { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
                }
            }

            //Ltcoll.Text = "";
            //LtServerMessage.Text = "";
            //Ltcollremark.Text = "";
            //Decimal loanrequest_amt = dw_main.GetItemDecimal(1, "loanrequest_amt");
            //Decimal sharestk_value = dw_main.GetItemDecimal(1, "sharestk_value");
            //Decimal salary_amt = dw_main.GetItemDecimal(1, "salary_amt") * 34;
            //Decimal loanreque_sharestk = 800000 + sharestk_value;
            //loantype = dw_main.GetItemString(1, "loantype_code");
            //if (loantype == "26") //สพ สามัญพ่วงพิเศษ
            //{

            //    if (loanrequest_amt >= loanreque_sharestk && dw_coll.RowCount < 2)
            //    {

            //        LtServerMessage.Text = WebUtil.ErrorMessage("วงเงินขอกู้มากกว่าหรือเท่ากับ  800,000 ต้องใช้ผู้ค้ำประกัน 2 คน ");

            //    }
            //    else
            //    {
            //        try
            //        {
            //            dw_main.SetItemString(1, "coop_id", state.SsCoopId);
            //            String memcoop_id = dw_main.GetItemString(1, "memcoop_id");
            //            member_no = dw_main.GetItemString(1, "member_no");
            //            String dwMain_XML = dw_main.Describe("DataWindow.Data.XML");
            //            String dwColl_XML = "";
            //            String dwClear_XML = "";
            //            String dwOtherClr_XML = "";

            //            try
            //            {
            //                for (int i = 1; i <= dw_coll.RowCount; i++)
            //                {

            //                    dw_coll.SetItemString(i, "coop_id", state.SsCoopId);
            //                    dw_coll.SetItemString(i, "refcoop_id", state.SsCoopControl);
            //                }

            //                dwColl_XML = dw_coll.Describe("DataWindow.Data.XML");

            //            }
            //            catch { dwColl_XML = ""; }
            //            try
            //            {
            //                for (int i = 1; i <= dw_clear.RowCount; i++)
            //                {
            //                    dw_clear.SetItemString(i, "coop_id", state.SsCoopId);
            //                    dw_clear.SetItemString(i, "concoop_id", state.SsCoopControl);
            //                }

            //                if (dw_clear.RowCount > 0)
            //                {
            //                    dwClear_XML = dw_clear.Describe("DataWindow.Data.XML");
            //                }

            //            }
            //            catch { dwClear_XML = ""; }

            //            try
            //            {
            //                for (int i = 1; i <= dw_otherclr.RowCount; i++)
            //                {
            //                    dw_otherclr.SetItemString(i, "coop_id", state.SsCoopId);

            //                }
            //                if (dw_otherclr.RowCount > 0)
            //                {
            //                    dwOtherClr_XML = dw_otherclr.Describe("DataWindow.Data.XML");
            //                }

            //            }
            //            catch { dwOtherClr_XML = ""; }
            //            str_itemchange strList = new str_itemchange();
            //            str_savereqloan strSave = new str_savereqloan();
            //            strList = WebUtil.str_itemchange_session(this);


            //            strSave.xml_main = dwMain_XML;
            //            strSave.xml_clear = dwClear_XML;
            //            strSave.xml_guarantee = dwColl_XML;
            //            strSave.xml_otherclr = dwOtherClr_XML;
            //            strSave.contcoopid = state.SsCoopControl;
            //            strSave.format_type = "CAT";
            //            strSave.entry_id = state.SsUsername;
            //            strSave.coop_id = state.SsCoopId;

            //            string expense_branch = "", expense_code = "";

            //            try
            //            {
            //                expense_branch = dw_main.GetItemString(1, "expense_branch");
            //            }
            //            catch { expense_branch = ""; }
            //            try
            //            {
            //                expense_code = dw_main.GetItemString(1, "expense_code");
            //            }
            //            catch { expense_code = ""; }
            //            if (expense_code == "CHQ" && expense_branch == "")
            //            {

            //                LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาเลือกสาขาธนาคาร");
            //            }
            //            else
            //            {
            //                String runningNo = shrlonService.LoanRightSaveReqloan(state.SsWsPass, ref strSave);

            //                reqdoc_no = strSave.request_no;
            //                txt_reqNo.Text = reqdoc_no;
            //                txt_member_no.Text = member_no;
            //                x = 2;

            //                //JsrunProcessAgreeLoan();
            //                //JsrunProcessCollReport();
            //                //JsrunProcessAgreeColl();
            //                //JsrunProcessLoan();
            //                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเรียบร้อยแล้ว");
            //                String ls_loantype = dw_main.GetItemString(1, "loantype_code");
            //                Session["loantypecoll"] = ls_loantype;
            //                JsReNewPage();
            //                LinkButton1.Visible = true;
            //                LinkButton2.Visible = true;
            //                LinkButton3.Visible = true;
            //                LinkButton4.Visible = true;
            //            }
            //        }
            //        catch (Exception ex)
            //        { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
            //    }
            //}
            //else
            //{
            //    if (loanrequest_amt >= loanreque_sharestk && dw_coll.RowCount < 3)
            //    {

            //        LtServerMessage.Text = WebUtil.ErrorMessage("วงเงินขอกู้มากกว่าหรือเท่ากับ  800,000 ต้องใช้ผู้ค้ำประกัน 2 คน ");

            //    }
            //    else
            //    {
            //        try
            //        {
            //            dw_main.SetItemString(1, "coop_id", state.SsCoopId);
            //            String memcoop_id = dw_main.GetItemString(1, "memcoop_id");
            //            member_no = dw_main.GetItemString(1, "member_no");
            //            String dwMain_XML = dw_main.Describe("DataWindow.Data.XML");
            //            String dwColl_XML = "";
            //            String dwClear_XML = "";
            //            String dwOtherClr_XML = "";

            //            try
            //            {
            //                for (int i = 1; i <= dw_coll.RowCount; i++)
            //                {

            //                    dw_coll.SetItemString(i, "coop_id", state.SsCoopId);
            //                    dw_coll.SetItemString(i, "refcoop_id", state.SsCoopControl);
            //                }

            //                dwColl_XML = dw_coll.Describe("DataWindow.Data.XML");

            //            }
            //            catch { dwColl_XML = ""; }
            //            try
            //            {
            //                for (int i = 1; i <= dw_clear.RowCount; i++)
            //                {
            //                    dw_clear.SetItemString(i, "coop_id", state.SsCoopId);
            //                    dw_clear.SetItemString(i, "concoop_id", state.SsCoopControl);
            //                }

            //                if (dw_clear.RowCount > 0)
            //                {
            //                    dwClear_XML = dw_clear.Describe("DataWindow.Data.XML");
            //                }

            //            }
            //            catch { dwClear_XML = ""; }

            //            try
            //            {
            //                for (int i = 1; i <= dw_otherclr.RowCount; i++)
            //                {
            //                    dw_otherclr.SetItemString(i, "coop_id", state.SsCoopId);

            //                }
            //                if (dw_otherclr.RowCount > 0)
            //                {
            //                    dwOtherClr_XML = dw_otherclr.Describe("DataWindow.Data.XML");
            //                }

            //            }
            //            catch { dwOtherClr_XML = ""; }
            //            str_itemchange strList = new str_itemchange();
            //            str_savereqloan strSave = new str_savereqloan();
            //            strList = WebUtil.str_itemchange_session(this);


            //            strSave.xml_main = dwMain_XML;
            //            strSave.xml_clear = dwClear_XML;
            //            strSave.xml_guarantee = dwColl_XML;
            //            strSave.xml_otherclr = dwOtherClr_XML;
            //            strSave.contcoopid = state.SsCoopControl;
            //            strSave.format_type = "CAT";
            //            strSave.entry_id = state.SsUsername;
            //            strSave.coop_id = state.SsCoopId;

            //            string expense_branch = "", expense_code = "";

            //            try
            //            {
            //                expense_branch = dw_main.GetItemString(1, "expense_branch");
            //            }
            //            catch { expense_branch = ""; }
            //            try
            //            {
            //                expense_code = dw_main.GetItemString(1, "expense_code");
            //            }
            //            catch { expense_code = ""; }
            //            if (expense_code == "CHQ" && expense_branch == "")
            //            {

            //                LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาเลือกสาขาธนาคาร");
            //            }
            //            else
            //            {
            //                String runningNo = shrlonService.LoanRightSaveReqloan(state.SsWsPass, ref strSave);

            //                reqdoc_no = strSave.request_no;
            //                txt_reqNo.Text = reqdoc_no;
            //                txt_member_no.Text = member_no;
            //                x = 2;

            //                //JsrunProcessAgreeLoan();
            //                //JsrunProcessCollReport();
            //                //JsrunProcessAgreeColl();
            //                //JsrunProcessLoan();
            //                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเรียบร้อยแล้ว");
            //                String ls_loantype = dw_main.GetItemString(1, "loantype_code");
            //                Session["loantypecoll"] = ls_loantype;
            //                JsReNewPage();
            //                LinkButton1.Visible = true;
            //                LinkButton2.Visible = true;
            //                LinkButton3.Visible = true;
            //                LinkButton4.Visible = true;
            //            }
            //        }
            //        catch (Exception ex)
            //        { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
            //    }
            //}
        }

        public void WebSheetLoadEnd()
        {

            try
            {
                DataWindowChild dwloantype_code = dw_main.GetChild("loantype_code_1");
                dwloantype_code.SetFilter("LNLOANTYPE.LOANTYPE_CODE in ('23','24','26') ");
                dwloantype_code.Filter();
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

            if ((expendCode == "CHQ"))
            {

                //ฝั่งธนาคาร
                dw_main.Modify("t_20.visible =1");
                dw_main.Modify("expense_bank.visible =1");
                dw_main.Modify("t_30.visible =1");
                dw_main.Modify("expense_bank_1.visible =1");

                dw_main.Modify("t_39.visible =1");
                dw_main.Modify("expense_branch.visible =1");
                dw_main.Modify("t_27.visible =1");
                dw_main.Modify("expense_branch_1.visible =1");

                dw_main.SetItemString(1, "expense_bank", "014");
                JsExpenseBank();

                try
                {

                    DwUtil.RetrieveDDDW(dw_main, "expense_bank_1", "sl_loan_requestment.pbl", null);

                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            }
            else if ((expendCode == "CSH"))
            {

                //ฝั่งธนาคาร
                dw_main.Modify("t_20.visible =0");
                dw_main.Modify("expense_bank.visible =0");
                dw_main.Modify("t_30.visible =0");
                dw_main.Modify("expense_bank_1.visible =0");

                dw_main.Modify("t_39.visible =0");
                dw_main.Modify("expense_branch.visible =0");
                dw_main.Modify("t_27.visible =0");
                dw_main.Modify("expense_branch_1.visible =0");


            }
            else if ((expendCode == "TRN"))
            {

                //ฝั่งธนาคาร
                dw_main.Modify("t_20.visible =0");
                dw_main.Modify("expense_bank.visible =0");
                dw_main.Modify("t_30.visible =0");
                dw_main.Modify("expense_bank_1.visible =0");

                dw_main.Modify("t_39.visible =0");
                dw_main.Modify("expense_branch.visible =0");
                dw_main.Modify("t_27.visible =0");
                dw_main.Modify("expense_branch_1.visible =0");


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
                try { bankCode = "014"; }
                catch { bankCode = ""; }
                DataWindowChild dwExpenseBranch = dw_main.GetChild("expense_branch_1");

                dwExpenseBranch.SetFilter("CMUCFBANKBRANCH.BANK_CODE='" + bankCode + "' and CMUCFBANKBRANCH.BRANCH_ID  in ('0016','0026','5233','5234')");
                dwExpenseBranch.Filter();

            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
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
            DataWindowChild dwloantype_code = dw_main.GetChild("loantype_code_1");
            dwloantype_code.SetFilter("LNLOANTYPE.LOANTYPE_CODE in ('23','24','26') ");
            dwloantype_code.Filter();
        }

        /// <summary>
        ///  retreive datawindows dropdown
        /// </summary>
        public void RetreiveDDDW()
        {

            try
            {

                loantype = Session["loantypecoll"].ToString();

                Session.Remove("loantype");

            }
            catch
            {
                loantype = "23";

                Session.Remove("loantype");

            }
            try
            {
                DwUtil.RetrieveDDDW(dw_main, "loantype_code_1", pbl, null);
                if (loantype == "")
                {
                    ///<กำหนดค่าเริ่มต้น เป็นสามัญ>

                    dw_main.SetItemString(1, "loantype_code", "23");

                }
                else
                {

                    DataWindowChild dwloantype_code = dw_main.GetChild("loantype_code_1");
                    dwloantype_code.SetFilter("LNLOANTYPE.LOANTYPE_CODE in ('23','24','26') ");
                    dwloantype_code.Filter();
                    dw_main.SetItemString(1, "loantype_code", loantype);

                }
                DwUtil.RetrieveDDDW(dw_main, "expense_code", pbl, null);
                dw_main.SetItemString(1, "expense_code", "CHQ");

                DwUtil.RetrieveDDDW(dw_main, "loanobjective_code_1", pbl, null);
                DwUtil.RetrieveDDDW(dw_main, "membtype_code", pbl, null);
                DwUtil.RetrieveDDDW(dw_main, "coop_id_1", pbl, null);
                DwUtil.RetrieveDDDW(dw_main, "memcoop_id", pbl, null);
                DwUtil.RetrieveDDDW(dw_main, "paytoorder_desc_1", pbl, null);
                DataWindowChild paytoorder = dw_main.GetChild("paytoorder_desc_1");
                if (state.SsUsername == "Loan01")
                {

                    dw_main.SetItemString(1, "paytoorder_desc", "001001");
                }
                else if (state.SsUsername == "Loan02")
                {

                    dw_main.SetItemString(1, "paytoorder_desc", "001002");
                }
                else if (state.SsUsername == "Loan03")
                {

                    dw_main.SetItemString(1, "paytoorder_desc", "001003");
                }
                else if (state.SsUsername == "Loan04")
                {

                    dw_main.SetItemString(1, "paytoorder_desc", "001004");
                }

                else { dw_main.SetItemString(1, "paytoorder_desc", state.SsCoopId); }

                DwUtil.RetrieveDDDW(dw_coll, "loancolltype_code", pbl, null);
                DwUtil.RetrieveDDDW(dw_coll, "coop_id", pbl, null);
                DwUtil.RetrieveDDDW(dw_otherclr, "clrothertype_code", pbl, null);
                JsExpenseCode();

            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
        }

        /// <summary>
        ///  init ข้อมูลสมาชิก
        /// </summary>
        private void JsGetMemberInfo()
        {
            try
            {
                JsReNewPage();
                Ltcoll.Text = "";
                LtServerMessage.Text = "";
                Ltcollremark.Text = "";
                LinkButton1.Visible = false;
                LinkButton2.Visible = false;
                CbCheckcoop.Checked = false;
                Checkcollloop.Checked = false;
                Decimal lndroploanall_flag = 0;
                DateTime loanrcvfix_date = dw_main.GetItemDate(1, "loanrcvfix_date");
                DateTime loanrequest_date = dw_main.GetItemDate(1, "loanrequest_date");
                DateTime postingdate_bf = wcf.Busscom.of_getpostingdate2(state.SsWsPass, Convert.ToInt16(loanrcvfix_date.Year + 543), Convert.ToInt16(loanrcvfix_date.Month));
                //เดือนปัจุจบัน
                DateTime postingdate = wcf.Busscom.of_getpostingdate(state.SsWsPass, loanrcvfix_date);
                //เดือนหลัง
                DateTime postingdate_af = wcf.Busscom.of_getpostingdate(state.SsWsPass, postingdate.AddMonths(1));

                short ai_increase = 0;
                DateTime ldtm_loanrequest = new DateTime();
                DateTime ldtm_postingdate = new DateTime();
                //ประเภคบุคคลค้ำ จ่ายถัดวันขอกู้อีก 3 วันทำการ
                //a hardcode

                ai_increase = 3;
                ldtm_loanrequest = wcf.Busscom.of_relativeworkdate(state.SsWsPass, loanrequest_date, ai_increase);
                ldtm_postingdate = wcf.Busscom.of_relativeworkdate(state.SsWsPass, postingdate_bf, 1);





                Decimal ldc_sharestk, ldc_periodshramt, ldc_periodshrvalue, ldc_shrvalue, ldc_loanrequeststatus = 0;
                Decimal ldc_salary = 0, ldc_incomemth = 0, ldc_paymonth;
                Decimal ldc_shrstkvalue = 0;
                int li_shrpaystatus, li_lastperiod = 0, li_membertype;

                DateTime ldtm_birth = new DateTime(), ldtm_member = new DateTime(), ldtm_work = new DateTime(), ldtm_retry = new DateTime();
                string ls_position, ls_remark, ls_membname, ls_membgroup, ls_groupname;
                string ls_membtypedesc = "", ls_controlname = "", ls_membcontrol = "", ls_appltype, ls_memno, ls_CoopControl;

                String member_no = WebUtil.MemberNoFormat(HdMemberNo.Value);
                dw_main.SetItemString(1, "member_no", member_no);

                ls_CoopControl = state.SsCoopControl; //สาขาที่ทำรายการ

                string as_xmlmessage = "";
                String bfls_loantype = "";
                String ls_loantype = "";
                String loanmbgroup_code = "";
                String membtype_desc = "";
                String loanoftype_code = "";
                string deptaccount_no = "";
                try
                {
                    bfls_loantype = dw_main.GetItemString(1, "loantype_code");
                    string sqltype = @"SELECT   DISTINCT   LNLOANMBTYPE.TYPEOFLOAN_CODE    
                                        FROM LNLOANMBTYPE  WHERE LNLOANMBTYPE.LOANTYPE_CODE ='" + bfls_loantype + @"' ";
                    Sdt dttype = WebUtil.QuerySdt(sqltype);
                    if (dttype.Next())
                    {

                        loanoftype_code = dttype.GetString("TYPEOFLOAN_CODE");
                    }


                    string sqlloan = @"SELECT LOANMBGROUP_CODE ,MEMBTYPE_DESC ,MEMBTYPE_CODE FROM MBUCFMEMBTYPE  
							   WHERE MEMBTYPE_CODE =( SELECT MEMBTYPE_CODE  FROM MBMEMBMASTER WHERE  MEMBER_NO= '" + member_no + @"')";
                    Sdt dtloan = WebUtil.QuerySdt(sqlloan);
                    if (dtloan.Next())
                    {
                        loanmbgroup_code = dtloan.GetString("LOANMBGROUP_CODE");
                        membtype_desc = dtloan.GetString("MEMBTYPE_DESC");
                        ls_membtype = dtloan.GetString("MEMBTYPE_CODE");
                        dw_main.SetItemString(1, "membtype_code", ls_membtype);
                        dw_main.SetItemString(1, "membtype_desc", membtype_desc);
                    }
                    if (loanmbgroup_code == "01")
                    {

                        bfls_loantype = loanoftype_code;

                    }

                    string loan_group = @"  SELECT

                                             LNLOANMBTYPE.LOANTYPE_CODE    
                                             FROM LNLOANMBTYPE  
                                            WHERE 
                                             LNLOANMBTYPE.LOANTYPE_CODE in (SELECT LNLOANTYPE.LOANTYPE_CODE
                                            FROM   LNLOANTYPE 
                                            WHERE LNLOANTYPE.LOANGROUP_CODE =(
                                            SELECT DISTINCT LNLOANTYPE.LOANGROUP_CODE  
                                                                                    FROM LNLOANMBTYPE,   
                                                                                         LNLOANTYPE  
                                                                                   WHERE ( LNLOANMBTYPE.COOP_ID = LNLOANTYPE.COOP_ID ) and  
                                                                                         ( LNLOANMBTYPE.LOANTYPE_CODE = LNLOANTYPE.LOANTYPE_CODE ) and  
                                                                                         ( ( LNLOANMBTYPE.LOANTYPE_CODE ='" + loanoftype_code + @"' ))) )
                                             and LNLOANMBTYPE.MEMBTYPE_CODE='" + loanmbgroup_code + @"'  and  LNLOANMBTYPE.TYPEOFLOAN_CODE='" + bfls_loantype + @"'
                                                                                        ";
                    Sdt dtloan_group = WebUtil.QuerySdt(loan_group);
                    if (dtloan_group.Next())
                    {

                        ls_loantype = dtloan_group.GetString("LOANTYPE_CODE");
                        dw_main.SetItemString(1, "loantype_code", ls_loantype);
                        dw_main.SetItemString(1, "loantype_code_1", ls_loantype);

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
                               WHERE ( a.MEMBGROUP_CODE = MBMEMBMASTER.MEMBGROUP_CODE ) and  a.membgroup_control = b.membgroup_code and
                                        a.coop_id = b.coop_id and 
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
                                        string sqldapt = @"select deptaccount_no from wcdeptmaster where member_no ='" + member_no + @"' and wftype_code = '01' ";  //เพิ่มเงื่อนไข คือ and wftype_code = '01' โดยดึงมาเฉพาะประเภทของสมาชิกเท่านั้น
                                        Sdt dtdapt = WebUtil.QuerySdt(sqldapt);
                                        if (dtdapt.Next())
                                        {
                                            deptaccount_no = dtdapt.GetString("deptaccount_no");
                                            li_cramationstatus = 1;
                                        }
                                        else { li_cramationstatus = 0; }
                                    }
                                    catch
                                    {

                                        li_cramationstatus = 0;

                                    }

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

                                    // ls_membtype = dt.GetString("membtype_code");
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

                                    dw_main.SetItemDateTime(1, "birth_date", ldtm_birth);
                                    dw_main.SetItemDateTime(1, "member_date", ldtm_member);
                                    //dw_main.SetItemString(1, "membtype_code", ls_membtype);
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
                                    dw_main.SetItemString(1, "member_remark", ls_remark + "==>>เลขสมาชิก สมม." + deptaccount_no);
                                    //ติดไว้ก่อน
                                    //if (ldtm_loanrequest == postingdate_bf || ldtm_loanrequest <= ldtm_postingdate)
                                    //{//&& loanrcvfix_date > postingdate_af
                                    //    dw_main.SetItemDecimal(1, "share_lastperiod", li_lastperiod + 1);
                                    //    dw_main.SetItemDecimal(1, "sharestk_value", ldc_shrstkvalue + ldc_periodshrvalue);
                                    //    // dw_main.SetItemDecimal(1, "periodshare_value", ldc_periodshrvalue);
                                    //}
                                    //else
                                    //{
                                    string result = wcf.InterPreter.GetAddshare(ldc_shrstkvalue);
                                    if (result == "1")
                                    {


                                        dw_main.SetItemDecimal(1, "share_lastperiod", li_lastperiod + 1);
                                        dw_main.SetItemDecimal(1, "sharestk_value", ldc_shrstkvalue + ldc_periodshrvalue);

                                    }
                                    else
                                    {

                                        dw_main.SetItemDecimal(1, "share_lastperiod", li_lastperiod);
                                        dw_main.SetItemDecimal(1, "sharestk_value", ldc_shrstkvalue);
                                    }
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
                                dw_main.SetItemDecimal(1, "loanpayment_type", 1);
                                //dw_main.SetItemDecimal(1, "minsalary_perc", Convert.ToDecimal(Setloantypeinfo[1]));
                                //dw_main.SetItemDecimal(1, "minsalary_amt", Convert.ToDecimal(Setloantypeinfo[2]));
                                //dw_main.SetItemDecimal(1, "minsalary_inc", Convert.ToDecimal(Setloantypeinfo[3]));
                                dw_main.SetItemDecimal(1, "int_intsteptype", 1);
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
                                dw_main.SetItemDecimal(1, "int_continttype", 2);
                                dw_main.SetItemDecimal(1, "int_contintrate", new Decimal(0.055));
                                dw_main.SetItemString(1, "int_continttabcode", "INT01");
                                dw_main.SetItemDecimal(1, "int_contintincrease", 0);
                                SetAcci_dept();//เซ็ดเลขบัญชี
                                //                                ///<สิทธิ์กู้สูดสุง>

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
                                    else { LtServerMessage.Text = WebUtil.ErrorMessage("งวดการส่ค่าหุ้นของสมาชิกท่านนี้ยังไม่ถึง 4 งวด"); }
                                    ///<ตรวจหาสัญญาที่จะหักกลบ ถ้ามีก็เอามาแสดง ใน dw_clear>
                                    Genbaseloanclear();
                                }


                            } //end เช็คสิทธิ์ ก่อนขอกู้ว่ามีสัญญาเก่าค้างหรือไม่

                            DwUtil.RetrieveDDDW(dw_main, "loantype_code_1", pbl, null);
                            DataWindowChild dwloantype_code = dw_main.GetChild("loantype_code_1");
                            dwloantype_code.SetFilter("MEMBTYPE_CODE ='" + loanmbgroup_code + "' and LNLOANTYPE.LOANGROUP_CODE   ='02'");
                            dwloantype_code.Filter();

                            dw_main.SetItemString(1, "loantype_code", ls_loantype);
                            dw_main.SetItemString(1, "loantype_code_1", ls_loantype);
                        }
                    }
                    else
                    {
                        LtServerMessage.Text = WebUtil.WarningMessage("ไม่มีสิทธิ์กู้เงินประเภทนี้ เนื่องจากเป็นสมาชิกประเภท " + membtype_desc);

                        DwUtil.RetrieveDDDW(dw_main, "loantype_code_1", pbl, null);
                        DataWindowChild dwloantype_code = dw_main.GetChild("loantype_code_1");
                        dwloantype_code.SetFilter("MEMBTYPE_CODE ='01'and LNLOANTYPE.LOANGROUP_CODE   ='02'");
                        dwloantype_code.Filter();

                        dw_main.SetItemString(1, "loantype_code", bfls_loantype);
                        dw_main.SetItemString(1, "loantype_code_1", bfls_loantype);

                    }


                }

                catch { LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาเลือกประเภทเงินกู้ที่จะทำรายการ"); }


                if (ls_loantype == "23" && deptaccount_no == "")
                {

                    LtServerMessage.Text = WebUtil.WarningMessage("สมาชิก" + member_no + " ไม่ได้สมัครเป็น สมาชิกฌาปนกิจสงเคาระห์ของสหกรณ์");

                }
                else if (ls_loantype == "26" && deptaccount_no == "")
                {

                    LtServerMessage.Text = WebUtil.WarningMessage("สมาชิก" + member_no + " ไม่ได้สมัครเป็น สมาชิกฌาปนกิจสงเคาระห์ของสหกรณ์");

                }


            }
            catch (Exception ex)
            {
                // LtServerMessage.Text = WebUtil.ErrorMessage("JsGetMemberInfo===>" + ex);

            }
            HdIsPostBack.Value = "false"; ;
        }

        private void Jsmaxcreditperiod()
        {
            try
            {
                String ls_loantype = dw_main.GetItemString(1, "loantype_code");
                DateTime ldtm_member;
                string as_xmlmessage = "";
                try
                {
                    ldtm_member = dw_main.GetItemDateTime(1, "member_date");
                }
                catch { ldtm_member = state.SsWorkDate; }

                Decimal ldc_shrstkvalue = dw_main.GetItemDecimal(1, "sharestk_value");
                Decimal ldc_salary = dw_main.GetItemDecimal(1, "salary_amt");
                String ls_memcoopid = dw_main.GetItemString(1, "memcoop_id");
                String ls_membtypedesc = dw_main.GetItemString(1, "membtype_desc");
                String member_no = dw_main.GetItemString(1, "member_no");

                ///<หาอายุงานของาสมาชิก>
                int memtime = 0;
                Decimal member_age = dw_main.GetItemDecimal(1, "member_age");
                if (member_age > 1)
                {
                    memtime = Convert.ToInt32(BusscomService.of_cal_yearmonth(state.SsWsPass, ldtm_member, state.SsWorkDate) * 12);

                }
                else if (member_age < 1)
                {
                    int mem = Convert.ToInt32(member_age * 100);
                    if (mem >= 3) { memtime = mem; } else { memtime = 0; }
                }
                else
                {

                    int mem = Convert.ToInt32(member_age * 12);
                    if (mem >= 3) { memtime = mem; } else { memtime = 0; }
                }

                //เช็คสิทธิ์ ก่อนขอกู้ว่ามีสัญญาเก่าค้างหรือไม่
                int checkoldloanpayment = wcf.Shrlon.of_checkoldloanpayment(state.SsWsPass, state.SsCoopControl, member_no, ls_loantype, ref as_xmlmessage);

                if (checkoldloanpayment != 1)   //มีสัญญาเก่าค้าง
                {
                    if ((as_xmlmessage != "") && (as_xmlmessage != null))
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage(as_xmlmessage);
                    }

                }
                else    //ไม่มีสัญญาเก่าค้าง
                {
                    ///<กำหนดวันที่จ่ายและเรียกเก็บแต่ปละเภทสัญญาวันจ่ายไม่เหมือนกัน>
                    JsChangeStartkeep();

                    Decimal[] max_creditperiod = new Decimal[4];
                    Decimal per70 = new Decimal(0.7);
                    Decimal per90 = new Decimal(0.9);
                    Decimal loancredit_amt = 0;
                    ls_loantype = dw_main.GetItemString(1, "loantype_code");
                    ///< หาสิทธิ์กู้สูงสุด,งวดสูงสุด>
                    max_creditperiod = shrlonService.CalloanpermissMHD(state.SsWsPass, ls_memcoopid, ls_loantype, ldc_salary, ldc_shrstkvalue, memtime);

                    loancredit_amt = max_creditperiod[0];

                    ///<05 ลูกจ้างชั่วคราว กู้ สามัญ.ได้ 90% ของค่าหุ้น>
                    if (ls_membtype == "05" && ls_loantype == "21") { loancredit_amt = Convert.ToDecimal(max_creditperiod[0]) * per90; }
                    else { loancredit_amt = max_creditperiod[0]; }

                    ///<13 เก็บเอง,รับบำเหน็จ กู้ สามัญ.ได้ 90% ของค่าหุ้น>
                    if (ls_membtype == "13" && ls_loantype == "21") { loancredit_amt = ldc_shrstkvalue * per90; }
                    ///<11 รอชำระคืน,รับบำเหน็จ กู้ สามัญ.ได้ 90% ของค่าหุ้น>
                    if (ls_membtype == "11" && ls_loantype == "20") { loancredit_amt = ldc_shrstkvalue * per90; }
                    ///<12 เกษียณรับบำนาญ,รับบำเหน็จ กู้ สามัญ.ได้ 100% ของค่าหุ้น>
                    if (ls_membtype == "12" && ls_loantype == "22") { loancredit_amt = ldc_shrstkvalue; }

                    if (ls_membtype == "05" && ls_loantype == "24") { loancredit_amt = ldc_shrstkvalue * per90; }
                    if (loancredit_amt > 2200000) { loancredit_amt = 2200000; }
                    dw_main.SetItemDecimal(1, "loancredit_amt", loancredit_amt);
                    dw_main.SetItemDecimal(1, "maxsend_payamt", max_creditperiod[1]);
                    dw_main.SetItemDecimal(1, "period_payamt", max_creditperiod[1]);


                    if (ls_loantype == "26" || ls_loantype == "24") /// default คนค้ำ.
                    {
                        dw_coll.Reset();
                        dw_coll.InsertRow(1);
                        dw_coll.SetItemString(1, "loancolltype_code", "01");



                    }
                    else if (ls_loantype == "20" || ls_loantype == "21" || ls_loantype == "22") /// default ค่าหุ้น.
                    {
                        dw_coll.Reset();
                        dw_coll.InsertRow(1);
                        dw_coll.SetItemString(1, "loancolltype_code", "02");
                        dw_coll.SetItemString(1, "ref_collno", dw_main.GetItemString(1, "member_no"));
                        dw_coll.SetItemString(1, "description", dw_main.GetItemString(1, "member_name"));
                        dw_coll.SetItemDecimal(1, "coll_balance", dw_main.GetItemDecimal(1, "sharestk_value"));
                        dw_coll.SetItemDecimal(1, "use_amt", dw_main.GetItemDecimal(1, "sharestk_value"));
                        dw_coll.SetItemString(1, "coop_id", state.SsCoopControl);
                    }

                }

            }
            catch (Exception ex)
            {
                // LtServerMessage.Text = WebUtil.ErrorMessage("Jsmaxcreditperiod===>" + ex);
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
            Decimal li_paytype, li_status = 0, li_clearflag, li_contstatus, li_intcontinttype, li_intsteptype;
            Decimal li_checkcontclr = 0, li_periodamt, li_contlaw, li_paystatus, li_clearinsure, li_countpayflag = 0;
            Decimal ldc_appvamt, ldc_balance = 0, ldc_withdrawable, ldc_rkeepprin, ldc_rkeepint, ldc_transbal;
            Decimal ldc_intarrear, ldc_payment;
            Decimal ldc_intestim = 0;
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
                    {
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


                        int day_amount = 0;

                        ls_loantype = dw_main.GetItemString(1, "loantype_code");
                        DateTime loanrequest_date = dw_main.GetItemDate(1, "loanrequest_date");
                        DateTime postingdate_bf = wcf.Busscom.of_getpostingdate2(state.SsWsPass, Convert.ToInt16(loanrcvfix_date.Year + 543), Convert.ToInt16(loanrcvfix_date.Month));
                        //เดือนปัจุจบัน
                        DateTime postingdate = wcf.Busscom.of_getpostingdate(state.SsWsPass, loanrcvfix_date);
                        //เดือนหลัง
                        DateTime postingdate_af = wcf.Busscom.of_getpostingdate(state.SsWsPass, postingdate.AddMonths(1));

                        short ai_increase = 0;
                        DateTime ldtm_loanrequest = new DateTime();
                        DateTime ldtm_postingdate = new DateTime();
                        //ประเภคบุคคลค้ำ จ่ายถัดวันขอกู้อีก 3 วันทำการ
                        //a hardcode

                        ai_increase = 3;
                        ldtm_loanrequest = wcf.Busscom.of_relativeworkdate(state.SsWsPass, loanrequest_date, ai_increase);
                        ldtm_postingdate = wcf.Busscom.of_relativeworkdate(state.SsWsPass, postingdate_bf, 1);

                        if (li_period == 0)
                        {
                            ldc_balance = ldc_balance - 0;
                            li_period = li_period + 0;
                        }
                        else
                        {
                            Decimal ldcpayment = Convert.ToDecimal(wcf.InterPreter.Getcutloanshr(ldc_payment));
                            ldc_balance = ldc_balance - ldcpayment; //เอก่อน
                            if (ldcpayment != 0)
                            {
                                li_period = li_period + 1;//เอก่อน
                            }
                            else { li_period = li_period + 0; }
                        }

                        if (ldtm_loanrequest == postingdate_bf || ldtm_loanrequest <= ldtm_postingdate)
                        {

                            //if (li_period == 0)
                            //{
                            //    ldc_balance = ldc_balance - 0;
                            //    li_period = li_period + 0;
                            //}
                            //else
                            //{
                            //    Decimal ldc_payment_ = Convert.ToDecimal(wcf.InterPreter.Getcutloanshr(ldc_payment));
                            //    ldc_balance = ldc_balance - ldc_payment; //เอก่อน
                            //    if (ldc_payment_ != 0)
                            //    {
                            //        li_period = li_period + 1;//เอก่อน
                            //    }
                            //    else { li_period = li_period + 0; }
                            //}

                        }
                        else
                        {

                            day_amount = postingdate_af.DayOfYear - postingdate.DayOfYear;
                            //ldc_intestim = ldc_balance * (ldc_intcontintrate / 100) * day_amount / 365;
                            //ldc_intestim = wcf.Shrlon.of_roundmoney(state.SsWsPass, ldc_intestim);
                        }
                        if (ls_conttype != ls_loantype)
                        {
                            //ldc_intestim = wcf.Shrlon.of_roundmoney(state.SsWsPass, ldc_balance * (ldc_intcontintrate) * day_amount / 365);
                            ldc_intestim = wcf.Shrlon.of_roundmoney(state.SsWsPass, (wcf.Shrlon.of_computeinterest(state.SsWsPass, ls_memcoopid, ls_contno, loanrcvfix_date)));
                        }
                        else
                        {
                            //ldc_intestim = wcf.Shrlon.of_roundmoney(state.SsWsPass, (wcf.Shrlon.of_computeinterest(state.SsWsPass, ls_memcoopid, ls_contno, loanrcvfix_date)));
                            ldc_intestim = wcf.Shrlon.of_roundmoney(state.SsWsPass, ldc_balance * (ldc_intcontintrate) * 7 / 365);

                        }
                    }
                    catch { ldc_intestim = 0; }
                    if (ls_conttype == dw_main.GetItemString(1, "loantype_code") || ls_conttype == "20" || ls_conttype == "21" || ls_conttype == "10" || ls_conttype == "11" || ls_conttype == "12")
                    {

                        li_status = 1;


                    }
                    else { li_status = 0; }

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
                        dw_clear.SetItemDecimal(i + 1, "clear_status", li_status);
                        dw_clear.SetItemDateTime(i + 1, "lastprocess_date", ldtm_lastproc);
                        dw_clear.SetItemDecimal(i + 1, "contractint_type", li_continttype);
                        dw_clear.SetItemDecimal(i + 1, "contract_interest", ldc_intrate);
                        dw_clear.SetItemDecimal(i + 1, "rkeep_principal", ldc_rkeepprin);
                        dw_clear.SetItemDecimal(i + 1, "rkeep_interest", ldc_rkeepint);
                        dw_clear.SetItemDecimal(i + 1, "interest_arrear", ldc_intarrear);

                        dw_clear.SetItemDateTime(i + 1, "loanapprove_date", ldtm_approve);
                        dw_clear.SetItemDateTime(i + 1, "startcont_date", ldtm_startcont);
                        dw_clear.SetItemDecimal(i + 1, "int_continttype", li_intcontinttype);
                        dw_clear.SetItemDecimal(i + 1, "int_contintrate", ldc_intcontintrate);
                        dw_clear.SetItemString(i + 1, "int_continttabcode", ls_intcontintcode);
                        dw_clear.SetItemDecimal(i + 1, "int_contintincrease", ldc_intincrease);
                        if (li_paytype == 1)
                        {
                            dw_clear.SetItemDecimal(i + 1, "intestimate_amt", Convert.ToDecimal(ldc_intestim));
                        }
                        else
                        {
                            dw_clear.SetItemDecimal(i + 1, "intestimate_amt", 0);
                        }
                        dw_clear.SetItemDecimal(i + 1, "int_intsteptype", li_intsteptype);
                        dw_clear.SetItemDecimal(i + 1, "period_payamt", li_periodamt);
                        dw_clear.SetItemDecimal(i + 1, "contlaw_status", li_contlaw);
                        dw_clear.SetItemDecimal(i + 1, "payment_status", li_paystatus);
                        dw_clear.SetItemDecimal(i + 1, "principal_transbal", ldc_transbal);
                        dw_clear.SetItemDecimal(i + 1, "insurecoll_flag", li_clearinsure);
                        dw_clear.SetItemDecimal(i + 1, "countpay_flag", li_countpayflag);
                        //  dw_clear.SetItemDecimal(i+1, "transfer_status", li_transfersts);
                        //  dw_clear.SetItemDecimal(i+1, "prnclear_amt", ldc_balance);

                        if (ls_conttype == dw_main.GetItemString(1, "loantype_code") || ls_conttype == "20" || ls_conttype == "21" || ls_conttype == "10" || ls_conttype == "11" || ls_conttype == "12")
                        {

                            //  li_status = 1;

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

        private void JsChangeStartkeep()
        {
            try
            {


                dw_main.SetItemDateTime(1, "loanrequest_date", state.SsWorkDate);
                DateTime ldtm_loanrequest = dw_main.GetItemDateTime(1, "loanrequest_date");
                String loantype = dw_main.GetItemString(1, "loantype_code");
                short ai_increase = 0;
                DateTime ldtm_loanreceive_af = new DateTime();
                //ประเภคบุคคลค้ำ จ่ายถัดวันขอกู้อีก 3 วันทำการ
                //a hardcode
                if (loantype == "23" || loantype == "26")
                {
                    ai_increase = 3;
                    ldtm_loanreceive_af = wcf.Busscom.of_relativeworkdate(state.SsWsPass, ldtm_loanrequest, ai_increase);

                }
                else { ldtm_loanreceive_af = ldtm_loanrequest; }

                DateTime postingdate_bf = wcf.Busscom.of_getpostingdate2(state.SsWsPass, Convert.ToInt16(ldtm_loanreceive_af.Year + 543), Convert.ToInt16(ldtm_loanreceive_af.Month));
                //  processdate = wcf.Busscom.of_getpostingdate(state.SsWsPass, ldtm_loanreceive_af);
                if (ldtm_loanreceive_af == postingdate_bf)
                {  //วันเดียวกับวันเงินเดือนออก
                    ai_increase = 1;
                    ldtm_loanreceive_af = wcf.Busscom.of_relativeworkdate(state.SsWsPass, ldtm_loanreceive_af, ai_increase);
                }
                DateTime postingdate = wcf.Busscom.of_getpostingdate2(state.SsWsPass, Convert.ToInt16(postingdate_bf.Year + 543), Convert.ToInt16(postingdate_bf.Month + 1));
                DateTime postingdate_af = wcf.Busscom.of_getpostingdate2(state.SsWsPass, Convert.ToInt16(postingdate.Year + 543), Convert.ToInt16(postingdate.Month + 1));


                ////จ่ายเงินกู้หลังเรียกเก็บหรือไม่
                //if (ldtm_loanrequest < postingdate && ldtm_loanrequest < postingdate_bf)
                //{
                //    if (ldtm_loanreceive_af > postingdate_bf)
                //    {
                //        postingdate = postingdate;// wcf.Busscom.of_getpostingdate2(state.SsWsPass, Convert.ToInt16(postingdate_af.Year + 543), Convert.ToInt16(postingdate_af.Month));
                //    }
                //    else
                //    {
                //        postingdate = postingdate;
                //    }

                //}
                //else if (ldtm_loanreceive_af > postingdate_bf)
                //{

                //    postingdate = wcf.Busscom.of_getpostingdate2(state.SsWsPass, Convert.ToInt16(postingdate_af.Year + 543), Convert.ToInt16(postingdate_af.Month));

                //}
                //  ldtm_loanreceive_af = wcf.InterPreter.Getpayloan(state.SsConnectionIndex, state.SsCoopControl, ldtm_loanreceive_af);
                dw_main.SetItemDate(1, "loanrcvfix_date", ldtm_loanreceive_af);
                postingdate = wcf.InterPreter.GetStartkeep(state.SsConnectionIndex, state.SsCoopControl, ldtm_loanreceive_af);
                dw_main.SetItemDate(1, "startkeep_date", postingdate);

                tDwMain.Eng2ThaiAllRow();


            }
            catch (Exception ex)
            {
                //LtServerMessage.Text = WebUtil.ErrorMessage("JsChangeStartkeep===>" + ex); 
            }
        }

        private void JsStartkeep()
        {
            try
            {
                DateTime postingdate = new DateTime();
                DateTime processdate = new DateTime();
                dw_main.SetItemDateTime(1, "loanrequest_date", state.SsWorkDate);
                DateTime ldtm_loanreceive = dw_main.GetItemDateTime(1, "loanrequest_date");
                String loantype = dw_main.GetItemString(1, "loantype_code");
                short ai_increase = 0;
                DateTime ldtm_loanreceive_af = new DateTime();
                //ประเภคบุคคลค้ำ จ่ายถัดวันขอกู้อีก 3 วันทำการ
                if (loantype == "23" || loantype == "26")
                {
                    ai_increase = 0;
                    ldtm_loanreceive_af = wcf.Busscom.of_relativeworkdate(state.SsWsPass, ldtm_loanreceive, ai_increase);
                }
                short year = Convert.ToInt16(ldtm_loanreceive_af.Year + 543);
                short month = Convert.ToInt16(ldtm_loanreceive_af.Month);
                String sqlpro = " SELECT MAX(LASTPROCESS_DATE)as LASTPROCESS_DATE FROM  LNCONTMASTER ";
                Sdt dtpro = WebUtil.QuerySdt(sqlpro);
                if (dtpro.Next())
                {
                    processdate = wcf.Busscom.of_getpostingdate(state.SsWsPass, ldtm_loanreceive_af);
                    // dtpro.GetDate("LASTPROCESS_DATE"); 
                }

                //จ่ายเงินกู้หลังเรียกเก็บหรือไม่
                if (ldtm_loanreceive_af < processdate)
                {

                    short month_old = Convert.ToInt16(ldtm_loanreceive_af.Month);
                    DateTime postingdate_old = wcf.Busscom.of_getpostingdate2(state.SsWsPass, year, month_old);
                    short month_new = Convert.ToInt16(postingdate_old.Month);
                    if (month_old == month_new) { month = Convert.ToInt16(ldtm_loanreceive_af.Month + 1); }
                    postingdate = wcf.Busscom.of_getpostingdate2(state.SsWsPass, year, month);


                }
                else if (ldtm_loanreceive_af == processdate)
                {
                    ai_increase = 1;
                    ldtm_loanreceive_af = wcf.Busscom.of_relativeworkdate(state.SsWsPass, ldtm_loanreceive_af, ai_increase);
                    month = Convert.ToInt16(ldtm_loanreceive_af.Month + 1);
                    postingdate = wcf.Busscom.of_getpostingdate2(state.SsWsPass, year, month);
                }
                else if (ldtm_loanreceive_af > processdate)
                {
                    //ai_increase = 1;
                    //ldtm_loanreceive = wcf.Busscom.of_relativeworkdate(state.SsWsPass, ldtm_loanreceive, ai_increase);
                    month = Convert.ToInt16(ldtm_loanreceive.Month + 2);
                    postingdate = wcf.Busscom.of_getpostingdate2(state.SsWsPass, year, month);
                }
                else
                {
                    month = Convert.ToInt16(ldtm_loanreceive_af.Month + 1);
                    postingdate = wcf.Busscom.of_getpostingdate2(state.SsWsPass, year, month);
                }

                dw_main.SetItemDate(1, "startkeep_date", postingdate);
                dw_main.SetItemDate(1, "loanrcvfix_date", ldtm_loanreceive_af);
                tDwMain.Eng2ThaiAllRow();

            }
            catch (Exception ex)
            {
                //LtServerMessage.Text = WebUtil.ErrorMessage("JsChangeStartkeep===>" + ex); 
            }
        }

        /// <summary>
        /// init ข้อมูลคนค้ำ
        /// </summary>
        private void JsGetMemberCollno()
        {
            try
            {
                Ltcoll.Text = "";
                LtServerMessage.Text = "";
                Ltcollremark.Text = "";
                int row = dw_coll.RowCount;
                String ls_memcoopid;
                String ref_collno = HdRefcoll.Value;
                string loancolltype_code = dw_coll.GetItemString(row, "loancolltype_code");
                string member_no_me = dw_main.GetItemString(1, "member_no");
                DateTime ldtm_retry = new DateTime();
                DateTime ldtm_birth = new DateTime();
                Decimal retryage = 0;
                Decimal Haveme_03 = 0;
                Decimal Haveref_03 = 0;
                Decimal coll_old = 0;
                String mb_collref = "f";
                Decimal member_age = dw_main.GetItemDecimal(1, "member_age");
                int mem_collage = 0;

                //ผู้กู้ค้ำประกันคนอื่นหรือไม่
                string sqlrefcoll = @"   SELECT LNCONTCOLL.LOANCONTRACT_NO,   
                                 LNCONTMASTER.MEMBER_NO, 
                                 LNCONTCOLL.COLL_PERCENT,    LNCONTCOLL.USE_AMT 
                            FROM LNCONTCOLL,   
                                 LNCONTMASTER
                           WHERE ( LNCONTCOLL.LOANCONTRACT_NO = LNCONTMASTER.LOANCONTRACT_NO ) and          
                                 ( LNCONTCOLL.COOP_ID = LNCONTMASTER.COOP_ID ) and  
                                 ( LNCONTCOLL.REF_COLLNO = '" + ref_collno + @"' ) AND  
                                 ( LNCONTCOLL.LOANCOLLTYPE_CODE = '01' ) AND  
                                 ( LNCONTCOLL.COLL_STATUS = 1 ) AND  
                                 ( LNCONTMASTER.CONTRACT_STATUS > 0 ) AND         
                                 LNCONTCOLL.COOP_ID = '" + state.SsCoopControl + @"' ";

                Sdt dtrefcoll = WebUtil.QuerySdt(sqlrefcoll);
                int dtrowcoll = dtrefcoll.GetRowCount();


                //ผู้ค้ำประกันค้ำแล้วกี่คน
                string sql1 = @"   SELECT LNCONTCOLL.LOANCONTRACT_NO,   
                                 LNCONTMASTER.MEMBER_NO, 
                                 LNCONTCOLL.COLL_PERCENT,    LNCONTCOLL.USE_AMT 
                            FROM LNCONTCOLL,   
                                 LNCONTMASTER
                           WHERE ( LNCONTCOLL.LOANCONTRACT_NO = LNCONTMASTER.LOANCONTRACT_NO ) and          
                                 ( LNCONTCOLL.COOP_ID = LNCONTMASTER.COOP_ID ) and  
                                 ( LNCONTCOLL.REF_COLLNO = '" + member_no_me + @"' ) AND  
                                 ( LNCONTCOLL.LOANCOLLTYPE_CODE = '01' ) AND  
                                 ( LNCONTCOLL.COLL_STATUS = 1 ) AND  
                                 ( LNCONTMASTER.CONTRACT_STATUS > 0 ) AND         
                                 LNCONTCOLL.COOP_ID = '" + state.SsCoopControl + @"' ";
                Sdt dt1 = WebUtil.QuerySdt(sql1);

                Decimal ageref = 0;

                int me_refcoll = dt1.GetRowCount();
                if (dt1.Next())
                {

                    int rowCount1 = dt1.GetRowCount();

                    //ค้ำคนอื่น 1 คน 
                    for (int x1 = 0; x1 < rowCount1; x1++)
                    {
                        string member_no3 = dt1.Rows[x1]["MEMBER_NO"].ToString();
                        //ค้ำเดิมกัน
                        if (ref_collno == member_no3)
                        {
                            //หาอายุคนค้ำ
                            String[] mem_coll = shrlonService.GetMembercoll(state.SsWsPass, state.SsCoopControl, member_no3, state.SsWorkDate);
                            ageref = Convert.ToDecimal(mem_coll[4]) / 12;
                            //คนกู้มากกว่าเท่ากับห้าปี คนค้ำน้อยกว่าห้าปีได้
                            if (member_age >= 5 && ageref <= 5)
                            { mem_collage = 1; }
                            //คนกู้น้อยกว่าเท่ากับห้าปี คนค้ำน้อยกว่าห้าปีได้  ไม่ได้
                            else if (member_age < 5 || ageref < 5)
                            { mem_collage = 2; }
                            else { mem_collage = 3; }

                        }
                        //ไม่ได้ค้ำเดิมกัน
                        else
                        {
                            //หาอายุคนค้ำ
                            String[] mem_coll = shrlonService.GetMembercoll(state.SsWsPass, state.SsCoopControl, ref_collno, state.SsWorkDate);
                            ageref = Convert.ToDecimal(mem_coll[4]) / 12;
                            //ถ้าคนค้ำติดค้ำคนอื่นอยู่
                            if (dtrowcoll > 1)
                            {
                                //คนกู้มากกว่าเท่ากับห้าปี คนค้ำน้อยกว่าห้าปีค้ำไม่ได้ต้องอายุสิบปีถึงจะค้ำได้สองคน
                                if (member_age >= 5 || ageref <= 5)
                                {
                                    mem_collage = 4;
                                }

                            }


                        }
                    }
                }
                else { mem_collage = 0; }



                //หาเกษียณอายุ
                String sqlstr = @"  SELECT  MBMEMBMASTER.BIRTH_DATE
                                    FROM MBMEMBMASTER
                                    WHERE  ( MBMEMBMASTER.MEMBER_NO = '" + ref_collno + @"' ) AND                                 
                                    MBMEMBMASTER.COOP_ID   ='" + state.SsCoopControl + @"'    ";
                Sdt dt = WebUtil.QuerySdt(sqlstr);
                while (dt.Next())
                {

                    try
                    {
                        ldtm_birth = dt.GetDate("BIRTH_DATE");
                    }
                    catch { }
                }
                try
                {
                    ///<หาวันที่เกษียณ>

                    ldtm_retry = wcf.InterPreter.CalReTryDate(state.SsConnectionIndex, state.SsCoopControl, ldtm_birth);

                }
                catch { }
                try
                {
                    ///<หาเกษียณอายุ>
                    Decimal retry_age = BusscomService.of_cal_yearmonth(state.SsWsPass, DateTime.Now, ldtm_retry);
                    String retry_agel = WebUtil.Left(retry_age.ToString("000.00"), 3);
                    String retry_ager = WebUtil.Right(retry_age.ToString(""), 2);
                    int retryagel = Convert.ToInt32(retry_agel) * 12;
                    Decimal retryager = (Math.Round(Convert.ToDecimal(retry_ager)) / 10) * 10;
                    retryage = retryagel + retryager;
                }
                catch { }

                //ประเภทใช้คนค้ำประกัน
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

                    ////เช็ค กรณีที่ผู้กู้และผู้ค้ำมีสัญญาเดิมเป็นกู้พิเศษต้องเพิ่มผู้ค้ำ อายุสมาชิก10 อีก 1 คน
                    //string sql03me = "  select loancontract_no from lncontmaster where member_no='" + member_no_me + "'  and loantype_code in ('30','31','32','33') and contract_status=1 ";
                    //Sdt dt03me = WebUtil.QuerySdt(sql03me);
                    //if (dt03me.Next())
                    //{
                    //    Haveme_03 = 1;
                    //}
                    //string sql03ref = "  select loancontract_no from lncontmaster where member_no='" + ref_collno + "'  and loantype_code in ('30','31','32','33') and contract_status=1";
                    //Sdt dt03ref = WebUtil.QuerySdt(sql03ref);
                    //if (dt03ref.Next())
                    //{
                    //    Haveref_03 = 1;
                    //}
                    //                    //เช็ค กรณีที่ผู้กู้และผู้ค้ำมีสัญญาเดิมเป็นกู้พิเศษต้องเพิ่มผู้ค้ำ อายุสมาชิก10 อีก 1 คน
                    //                    if (Haveme_03 == 1 && Haveref_03 == 1)
                    //                    {
                    //                        LtServerMessage.Text = WebUtil.WarningMessage("ผู้กู้และผู้ค้ำมีสัญญากู้พิเศษ ต้องใช้ผู้ค้ำประกัน  อายุ 10 ปี อีก 1   ท่าน");
                    //                        //   dw_coll.InsertRow(0);
                    //                        String[] mem_coll = shrlonService.GetMembercoll(state.SsWsPass, ls_memcoopid, ref_collno, state.SsWorkDate);
                    //                        String remark = "";
                    //                        if (mem_coll[5] == "1") { remark = "ท." + ref_collno + "  งดค้ำประกันเนื่อง   " + mem_coll[6]; } else { remark = ""; }
                    //                        if (mem_coll[0] != "" && mem_coll[5] == "0")
                    //                        {
                    //                            Decimal age = Convert.ToDecimal(mem_coll[4]);
                    //                            String agel = WebUtil.Left(age.ToString("000.00"), 3);
                    //                            String ager = WebUtil.Right(age.ToString(""), 2);
                    //                            int collagel = Convert.ToInt32(agel) / 12;
                    //                            Decimal collager = (Math.Round(Convert.ToDecimal(ager)) / 10) * 10;


                    //                            coll_old = Convert.ToDecimal(wcf.InterPreter.GetRefColl(state.SsConnectionIndex, member_no_me, state.SsCoopControl, ref_collno, loancolltype_code));
                    //                            String sqlcoll = @"      SELECT LNCONTCOLL.LOANCONTRACT_NO,   LNCONTCOLL.REF_COLLNO ,
                    //                                 LNCONTMASTER.MEMBER_NO,
                    //                                 LNCONTCOLL.COLL_PERCENT,    LNCONTCOLL.USE_AMT
                    //                            FROM LNCONTCOLL,  
                    //                                 LNCONTMASTER
                    //                           WHERE ( LNCONTCOLL.LOANCONTRACT_NO = LNCONTMASTER.LOANCONTRACT_NO ) and         
                    //                                 ( LNCONTCOLL.COOP_ID = LNCONTMASTER.COOP_ID ) and 
                    //                                 ( LNCONTMASTER.MEMBER_NO = '" + ref_collno + @"' ) AND
                    //                                 ( LNCONTCOLL.LOANCOLLTYPE_CODE = '" + loancolltype_code + @"') AND 
                    //                                 ( LNCONTCOLL.COLL_STATUS = 1 ) AND 
                    //                                 ( LNCONTMASTER.CONTRACT_STATUS > 0 ) AND
                    //                                    LNCONTCOLL.COOP_ID = '" + ls_memcoopid + "'";
                    //                            Sdt dtcoll = WebUtil.QuerySdt(sqlcoll);
                    //                            int rowCount = 0;
                    //                            rowCount = dtcoll.GetRowCount();

                    //                            if (dtcoll.Next())
                    //                            {

                    //                                string mb_coll = dtcoll.Rows[0]["REF_COLLNO"].ToString();
                    //                                String[] memb_coll = shrlonService.GetMembercoll(state.SsWsPass, ls_memcoopid, mb_coll, state.SsWorkDate);
                    //                                Decimal mb_age = Convert.ToDecimal(memb_coll[4]);
                    //                                if (mb_age > 60) { mb_collref = "t"; } else { mb_collref = "f"; }

                    //                            }
                    //                            //// อายุคนค้ำ < 5 ปี   และผู้กู้ไปค้ำให้สมาชิกอีกคนที่อายุไม่ถึง5 ปี                           
                    //                            if (age < 60 && mem_collage == 2 || mem_collage == 4 && coll_old != 1 && mb_collref == "f")
                    //                            {

                    //                                dw_coll.SetItemString(row, "ref_collno", "");
                    //                                dw_coll.SetItemString(row, "description", "");
                    //                                dw_coll.SetItemDecimal(row, "coll_balance", new Decimal(0));
                    //                                Ltcoll.Text = WebUtil.ErrorMessage("อายุคนค้ำ < 5 ปี และผู้กู้ไปค้ำให้สมาชิกอีกคนที่อายุไม่ถึง5 ปี ต้องใช้ผู้ค้ำอายุมมากว่า 5 ปีขึ้นไป ");
                    //                                mem_collage = 0;
                    //                            }

                    //                            else if (age < 60 && mem_collage == 1 && coll_old != 1 && mb_collref == "f")
                    //                            {

                    //                                string result = wcf.InterPreter.GetChangeColl(mem_collage);
                    //                                if (result == "1")
                    //                                {
                    //                                    dw_coll.SetItemString(row, "ref_collno", "");
                    //                                    dw_coll.SetItemString(row, "description", "");
                    //                                    dw_coll.SetItemDecimal(row, "coll_balance", new Decimal(0));

                    //                                    Ltcoll.Text = WebUtil.ErrorMessage("อายุคนค้ำ < 5 ปี แลกค้ำไม่ได้ ");
                    //                                }
                    //                                else
                    //                                {
                    //                                    dw_coll.SetItemString(row, "ref_collno", mem_coll[0]);
                    //                                    dw_coll.SetItemString(row, "description", mem_coll[1]);
                    //                                    dw_coll.SetItemDecimal(row, "coll_balance", Convert.ToDecimal(mem_coll[2]));
                    //                                    HUseamt.Value = mem_coll[2];
                    //                                    Ltcoll.Text = WebUtil.WarningMessage("สมาชิก ท. " + ref_collno + "  อายุสมาชิก " + collagel + "   ปี   " + (Math.Round(collager / 30) / 10) * 10 + "เดือน  งวดเกษียณเหลือ  =" + retryage + "  งวด");
                    //                                    Ltcollremark.Text = WebUtil.ErrorMessage(remark);
                    //                                }

                    //                            }
                    //                            //อายุคนค้ำ < 5 ปี ไม่ได้ค้ำเดิมกันอยู่ ค้ำได้แค่ 1
                    //                            else if (age <= 60 && rowCount >= 1 && coll_old != 1 && mb_collref == "f")
                    //                            {
                    //                                dw_coll.SetItemString(row, "ref_collno", "");
                    //                                dw_coll.SetItemString(row, "description", "");
                    //                                dw_coll.SetItemDecimal(row, "coll_balance", new Decimal(0));
                    //                                LtServerMessage.Text = WebUtil.WarningMessage("ไม่สามารถค้ำได้เนื่องจากสมาชิก ท. " + ref_collno + " ท่านนี้ค้ำไปแล้ว " + rowCount + " คน  และอายุ " + collagel + "   ปี   " + collager + "วัน");
                    //                                Ltcollremark.Text = WebUtil.ErrorMessage(remark);
                    //                            } //อายุคนค้ำ < 10 ปี ไม่ได้ค้ำเดิมกันอยู่ ค้ำได้แค่ 1
                    //                            else if (age > 60 && age < 120 && rowCount >= 1 && coll_old != 1 && me_refcoll >= 0 && mb_collref == "f")
                    //                            {

                    //                                dw_coll.SetItemString(row, "ref_collno", "");
                    //                                dw_coll.SetItemString(row, "description", "");
                    //                                dw_coll.SetItemDecimal(row, "coll_balance", new Decimal(0));
                    //                                LtServerMessage.Text = WebUtil.WarningMessage("ไม่สามารถค้ำได้เนื่องจากสมาชิก ท. " + ref_collno + " ท่านนี้ค้ำไปแล้ว " + rowCount + " คน  และอายุ " + collagel + "   ปี   " + (Math.Round(collager / 30) / 10) * 10 + "เดือน");
                    //                                Ltcollremark.Text = WebUtil.ErrorMessage(remark);
                    //                            }
                    //                            // อายุคนค้ำ > 10 ปีไม่ได้ค้ำเดิมกันอยู่ ค้ำได้แค่ 2
                    //                            else if (dtrowcoll == 2 && coll_old != 1 && mb_collref == "f")
                    //                            {
                    //                                dw_coll.SetItemString(row, "ref_collno", "");
                    //                                dw_coll.SetItemString(row, "description", "");
                    //                                dw_coll.SetItemDecimal(row, "coll_balance", new Decimal(0));
                    //                                LtServerMessage.Text = WebUtil.WarningMessage("ไม่สามารถค้ำได้เนื่องจากสมาชิก ท. " + ref_collno + " ท่านนี้ค้ำไปแล้ว " + rowCount + " คน ");
                    //                                Ltcollremark.Text = WebUtil.ErrorMessage(remark);
                    //                            }

                    //                            else
                    //                            {


                    //                                dw_coll.SetItemString(row, "ref_collno", mem_coll[0]);
                    //                                dw_coll.SetItemString(row, "description", mem_coll[1]);
                    //                                dw_coll.SetItemDecimal(row, "coll_balance", Convert.ToDecimal(mem_coll[2]));
                    //                                HUseamt.Value = mem_coll[2];
                    //                                Ltcoll.Text = WebUtil.WarningMessage("สมาชิก ท. " + ref_collno + "  อายุสมาชิก " + collagel + "   ปี   " + (Math.Round(collager / 30) / 10) * 10 + "เดือน  งวดเกษียณเหลือ  =" + retryage + "  งวด");
                    //                                Ltcollremark.Text = WebUtil.ErrorMessage(remark);

                    //                            }
                    //                        }
                    //                        else
                    //                        {
                    //                            LtServerMessage.Text = WebUtil.ErrorMessage(mem_coll[3]);
                    //                            Ltcollremark.Text = WebUtil.ErrorMessage(remark);
                    //                            dw_coll.SetItemString(row, "ref_collno", "");
                    //                        }

                    //                    }

                    //                    else
                    //                    {
                    //หาอายุคนค้ำ สิทธ์ค้ำ
                    String[] mem_coll = shrlonService.GetMembercoll(state.SsWsPass, ls_memcoopid, ref_collno, state.SsWorkDate);
                    String remark = "";
                    if (mem_coll[5] == "1") { remark = "ท." + ref_collno + "  งดค้ำประกันเนื่อง   " + mem_coll[6]; } else { remark = ""; }

                    if (mem_coll[0] != "" && mem_coll[5] == "0")
                    {

                        Decimal age = Convert.ToDecimal(mem_coll[4]);
                        String agel = WebUtil.Left(age.ToString("000.00"), 3);
                        String ager = WebUtil.Right(age.ToString(""), 2);
                        int collagel = Convert.ToInt32(agel) / 12;
                        Decimal collager = (Math.Round(Convert.ToDecimal(ager)) / 10) * 10;


                        //คนเดิมกันหรือไม่ coll_old=0 ไม่ได้ค้ำเดิมกัน,coll_old=1 ค้ำเดิมกัน
                        coll_old = Convert.ToDecimal(wcf.InterPreter.GetRefColl(state.SsConnectionIndex, member_no_me, state.SsCoopControl, ref_collno, loancolltype_code));
                       
                        String sqlcoll = @"      SELECT LNCONTCOLL.LOANCONTRACT_NO,   LNCONTCOLL.REF_COLLNO ,
                                 LNCONTMASTER.MEMBER_NO,
                                 LNCONTCOLL.COLL_PERCENT,    LNCONTCOLL.USE_AMT
                            FROM LNCONTCOLL,  
                                 LNCONTMASTER
                           WHERE ( LNCONTCOLL.LOANCONTRACT_NO = LNCONTMASTER.LOANCONTRACT_NO ) and         
                                 ( LNCONTCOLL.COOP_ID = LNCONTMASTER.COOP_ID ) and 
                                 (  LNCONTMASTER.MEMBER_NO  = '" + ref_collno + @"' ) AND
                                 ( LNCONTCOLL.LOANCOLLTYPE_CODE = '" + loancolltype_code + @"') AND 
                                 ( LNCONTCOLL.COLL_STATUS = 1 ) AND 
                                 ( LNCONTMASTER.CONTRACT_STATUS > 0 ) AND
                                    LNCONTCOLL.COOP_ID = '" + ls_memcoopid + "'";
                        Sdt dtcoll = WebUtil.QuerySdt(sqlcoll);
                        int rowCount = 0;
                        rowCount = dtcoll.GetRowCount();
                        if (dtcoll.Next())
                        {
                            string mem_collno = dtcoll.Rows[0]["MEMBER_NO"].ToString();
                            if (mem_collno == ref_collno) { rowCount = -1; }
                            string mb_coll = dtcoll.Rows[0]["REF_COLLNO"].ToString();
                            String[] memb_coll = shrlonService.GetMembercoll(state.SsWsPass, ls_memcoopid, mb_coll, state.SsWorkDate);
                            Decimal mb_age = Convert.ToDecimal(memb_coll[4]);
                            if (mb_age > 60) { mb_collref = "t"; } else { mb_collref = "f"; }

                        }
                        //// อายุคนค้ำ < 5 ปี   และผู้กู้ไปค้ำให้สมาชิกอีกคนที่อายุไม่ถึง5 ปี                           
                        if (age < 60 &&  mem_collage == 4 && coll_old != 1 && mb_collref == "f")
                        {

                            dw_coll.SetItemString(row, "ref_collno", "");
                            dw_coll.SetItemString(row, "description", "");
                            dw_coll.SetItemDecimal(row, "coll_balance", new Decimal(0));

                            Ltcoll.Text = WebUtil.ErrorMessage("อายุคนค้ำ < 5 ปี และผู้กู้ไปค้ำให้สมาชิกอีกคนที่อายุไม่ถึง5 ปี ต้องใช้ผู้ค้ำอายุมมากว่า 5 ปีขึ้นไป ");

                        }
                        else if (age < 60 && mem_collage == 1 && coll_old != 1 && mb_collref == "f")
                        {

                            string result = wcf.InterPreter.GetChangeColl(mem_collage);
                            if (result == "1")
                            {
                                dw_coll.SetItemString(row, "ref_collno", "");
                                dw_coll.SetItemString(row, "description", "");
                                dw_coll.SetItemDecimal(row, "coll_balance", new Decimal(0));

                                Ltcoll.Text = WebUtil.ErrorMessage("อายุคนค้ำ < 5 ปี แลกค้ำไม่ได้ ");
                            }
                            else
                            {
                                dw_coll.SetItemString(row, "ref_collno", mem_coll[0]);
                                dw_coll.SetItemString(row, "description", mem_coll[1]);
                                dw_coll.SetItemDecimal(row, "coll_balance", Convert.ToDecimal(mem_coll[2]));
                                HUseamt.Value = mem_coll[2];
                                Ltcoll.Text = WebUtil.WarningMessage("สมาชิก ท. " + ref_collno + "  อายุสมาชิก " + collagel + "   ปี   " + (Math.Round(collager / 30) / 10) * 10 + "เดือน  งวดเกษียณเหลือ  =" + retryage + "  งวด");
                                Ltcollremark.Text = WebUtil.ErrorMessage(remark);
                            }

                        }
                        //อายุคนค้ำ < 5 ปี ไม่ได้ค้ำเดิมกันอยู่ ค้ำได้แค่ 1
                        else if (age <= 60 && rowCount >= 1 && coll_old != 1 && mb_collref == "f")
                        {


                            dw_coll.SetItemString(row, "ref_collno", "");
                            dw_coll.SetItemString(row, "description", "");
                            dw_coll.SetItemDecimal(row, "coll_balance", new Decimal(0));
                            LtServerMessage.Text = WebUtil.WarningMessage("ไม่สามารถค้ำได้เนื่องจากสมาชิก ท. " + ref_collno + " ท่านนี้ค้ำไปแล้ว " + rowCount + " คน  และอายุ " + collagel + "   ปี   " + ager + "เดือน");
                            Ltcollremark.Text = WebUtil.ErrorMessage(remark);
                        }
                        //อายุคนค้ำ < 10 ปี ไม่ได้ค้ำเดิมกันอยู่ ค้ำได้แค่ 1
                        else if (age < 120 && rowCount >= 1 && coll_old != 1 && me_refcoll >= 0 && mb_collref == "f")
                        {

                            dw_coll.SetItemString(row, "ref_collno", "");
                            dw_coll.SetItemString(row, "description", "");
                            dw_coll.SetItemDecimal(row, "coll_balance", new Decimal(0));
                            LtServerMessage.Text = WebUtil.WarningMessage("ไม่สามารถค้ำได้เนื่องจากสมาชิก ท. " + ref_collno + " ท่านนี้ค้ำไปแล้ว " + rowCount + " คน  และอายุ " + collagel + "   ปี   " + ager + "เดือน");
                            Ltcollremark.Text = WebUtil.ErrorMessage(remark);
                        }
                        // อายุคนค้ำ > 10 ปีไม่ได้ค้ำเดิมกันอยู่ ค้ำได้แค่ 2
                        else if (age > 120 && dtrowcoll == 2 && coll_old != 1 && mb_collref == "f")
                        {
                            dw_coll.SetItemString(row, "ref_collno", "");
                            dw_coll.SetItemString(row, "description", "");
                            dw_coll.SetItemDecimal(row, "coll_balance", new Decimal(0));
                            LtServerMessage.Text = WebUtil.WarningMessage("ไม่สามารถค้ำได้เนื่องจากสมาชิก ท. " + ref_collno + " ท่านนี้ค้ำไปแล้ว " + dtrowcoll + " คน ");
                            Ltcollremark.Text = WebUtil.ErrorMessage(remark);
                        }
                        else if (age > 60 && age < 120 && dtrowcoll >= 1 && coll_old != 1 && mb_collref == "f")
                        {
                            dw_coll.SetItemString(row, "ref_collno", "");
                            dw_coll.SetItemString(row, "description", "");
                            dw_coll.SetItemDecimal(row, "coll_balance", new Decimal(0));
                            LtServerMessage.Text = WebUtil.WarningMessage("ไม่สามารถค้ำได้เนื่องจากสมาชิก ท. " + ref_collno + " ท่านนี้ค้ำไปแล้ว " + dtrowcoll + " คน ");
                            Ltcollremark.Text = WebUtil.ErrorMessage(remark);
                        }



                        else
                        {
                            dw_coll.SetItemString(row, "ref_collno", mem_coll[0]);
                            dw_coll.SetItemString(row, "description", mem_coll[1]);
                            dw_coll.SetItemDecimal(row, "coll_balance", Convert.ToDecimal(mem_coll[2]));
                            HUseamt.Value = mem_coll[2];
                            Ltcoll.Text = WebUtil.WarningMessage("สมาชิก ท. " + ref_collno + "  อายุสมาชิก " + collagel + "   ปี   " + (Math.Round(collager / 30) / 10) * 10 + "เดือน  งวดเกษียณเหลือ  =" + retryage + "  งวด");
                            Ltcollremark.Text = WebUtil.ErrorMessage(remark);
                        }
                    }
                    else
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage(mem_coll[3]);
                        Ltcollremark.Text = WebUtil.ErrorMessage(remark);
                        dw_coll.SetItemString(row, "ref_collno", "");
                    }
                    //  }
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
                mem_collage = 0;
            }
            catch (Exception ex)
            {
                //LtServerMessage.Text = WebUtil.ErrorMessage("33333");
                //dw_coll.SetItemString(row, "ref_collno", "");
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
                        period_payment = loanrequest_amt / period_payamt;
                        //Math.Round((loanrequest_amt / period_payamt) / roundpay) * roundpay;
                        // if ((loanrequest_amt / period_payamt) > period_payment) { period_payment = period_payment + roundpay; }
                    }
                    else
                    {
                        period_payment = 0;
                    }
                }
                dw_main.SetItemDecimal(1, "period_payment", period_payment);

                Decimal intestimate_amt = of_calintestimatemain();
                dw_main.SetItemDecimal(1, "intestimate_amt", intestimate_amt);

                if (ls_loantype == "23")
                {

                    dw_coll.Reset();
                    dw_coll.InsertRow(1);
                    dw_coll.SetItemString(1, "loancolltype_code", "02");
                    dw_coll.SetItemString(1, "ref_collno", dw_main.GetItemString(1, "member_no"));
                    dw_coll.SetItemString(1, "description", dw_main.GetItemString(1, "member_name"));
                    dw_coll.SetItemDecimal(1, "coll_balance", dw_main.GetItemDecimal(1, "sharestk_value"));
                    dw_coll.SetItemDecimal(1, "use_amt", dw_main.GetItemDecimal(1, "sharestk_value"));
                    dw_coll.SetItemString(1, "coop_id", state.SsCoopControl);
                    dw_coll.SetItemDecimal(1, "coll_percent", (dw_main.GetItemDecimal(1, "sharestk_value") / dw_main.GetItemDecimal(1, "loanrequest_amt")));
                    dw_coll.InsertRow(2);
                    dw_coll.SetItemString(2, "loancolltype_code", "01");
                }
            }
            catch (Exception ex)
            {
                //LtServerMessage.Text = WebUtil.ErrorMessage("JsSetpriod===>" + ex); 
            }

            //  Genbaseloanclear();
            HdIsPostBack.Value = "false";

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
            Decimal loancredit_amt = dw_main.GetItemDecimal(1, "loancredit_amt");
            // ปัดยอดชำระ
            String ll_roundpay = wcf.Busscom.of_getattribloantype(state.SsWsPass, ls_loantype, "payround_factor");
            int roundpay = Convert.ToInt16(ll_roundpay);


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
                    // period_payment = Math.Round(period_payment / roundpay) * roundpay;
                }
                dw_main.SetItemDecimal(1, "period_payment", period_payment);
                dw_main.SetItemDouble(1, "period_payamt", Convert.ToDouble(period_payamt));
                //   dw_main.SetItemDouble(1, "period_payamt", Convert.ToDouble( period_payamt.ToString("#00.00")));

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
            Decimal period_payment = 0;

            // ปัดยอดชำระ
            String ll_roundpay = wcf.Busscom.of_getattribloantype(state.SsWsPass, ls_loantype, "payround_factor");
            int roundpay = Convert.ToInt16(ll_roundpay);
            if (roundpay > 0)
            {
                if (period_payamt > 0)
                {
                    period_payment = loanrequest_amt / period_payamt;
                    //Math.Round((loanrequest_amt / period_payamt) / roundpay) * roundpay;
                    // if ((loanrequest_amt / period_payamt) > period_payment) { period_payment = period_payment + roundpay; }
                }
                else
                {
                    period_payment = 0;
                }

            }
            dw_main.SetItemDecimal(1, "period_payment", period_payment);
            HdIsPostBack.Value = "false";
        }

        /// <summary>
        /// ยกเลิก ใบคำขอกู้
        /// </summary>    
        private void JsCancelRequest()
        {
            String dwXmlMain = dw_main.Describe("DataWindow.Data.XML");
            String dwXmlMessage = "";
            String cancelID = state.SsUsername;
            String coop_id = state.SsCoopControl;
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
                // LtServerMessage.Text = WebUtil.ErrorMessage("JsCancelRequest===>" + ex);
            }

        }

        /// <summary>
        /// ค้ำประกัน
        /// </summary>
        private void JsCollCondition()
        {
            //String dwXmlMain = dw_main.Describe("DataWindow.Data.XML");
            //String dwXmlColl = dw_coll.Describe("DataWindow.Data.XML");
            //String dwXmlMessage = "";
            //try
            //{
            //    dwXmlColl = shrlonService.CollPercCondition(state.SsWsPass, dwXmlMain, dwXmlColl, ref dwXmlMessage);

            //    try
            //    {
            //        if ((dwXmlColl != "") && (dwXmlColl != null))
            //        {
            //            dw_coll.Reset();
            //            dw_coll.ImportString(dwXmlColl, FileSaveAsType.Xml);
            //        }

            //    }
            //    catch
            //    {
            //        dw_coll.Reset();
            //        //dw_coll.InsertRow(0); 
            //    }


            //    if ((dwXmlMessage != "") && (dwXmlMessage != null))
            //    {
            //        dw_message.Reset();
            //        dw_message.ImportString(dwXmlMessage, FileSaveAsType.Xml);
            //        string msgtext = dw_message.GetItemString(1, "msgtext");
            //        LtServerMessage.Text = WebUtil.WarningMessage(msgtext);
            //    }
            //}
            //catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage("JsCollCondition===>" + ex); }

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
            if (ls_loantype == "23")
            {
                Decimal totalprincipel_ref = 0;// = new Decimal[2];
                Decimal coll_balance = 0;
                string loancolltype_code, member_no = "", member_no3 = "", loan_coll = "";
                Decimal total = 0;
                DateTime member_date;
                Decimal balance_use = 0, principel_coll = 0;
                Decimal sharestk_ref = 0;
                Decimal sharestk_value = dw_main.GetItemDecimal(1, "sharestk_value");
                string member_no_me = dw_main.GetItemString(1, "member_no");
                Decimal loanrequest_amt, loanrequest;
                loanrequest = dw_main.GetItemDecimal(1, "loanrequest_amt");
                loanrequest_amt = loanrequest - sharestk_value;
                int row = dw_coll.RowCount;
                int i = 0;
                Decimal sum_balance = 0, coll_percen = 0, total_balance = 0, use_amt1 = 0, use_amt3 = 0, use_amt_bf3 = 0, use_amt2 = 0, use_amt_bf = 0;
                Decimal use1 = 0, u_1 = 0;
                for (i = 0; i < row; i++)
                {

                    loancolltype_code = dw_coll.GetItemString(i + 1, "loancolltype_code");

                    if (loancolltype_code == "01")
                    {
                        coll_balance = dw_coll.GetItemDecimal(i + 1, "coll_balance");

                    }

                    total = total + coll_balance;

                }

                while (row > 0)
                {
                    if (row != 1)
                    {

                        String ref_collno = dw_coll.GetItemString(row, "ref_collno");
                        string sql = @"  SELECT LNCONTCOLL.LOANCONTRACT_NO,   
                                 LNCONTMASTER.MEMBER_NO,                                    
                                 MBMEMBMASTER.MEMB_NAME,   
                                 MBMEMBMASTER.MEMB_SURNAME, MBMEMBMASTER.MEMBER_DATE, LNCONTMASTER.LOANTYPE_CODE, 
                                 lncontmaster.principal_balance + lncontmaster.withdrawable_amt as sum_balance,   
                                 LNCONTCOLL.COLL_PERCENT,    LNCONTCOLL.USE_AMT  ,(SHSHAREMASTER.SHARESTK_AMT*10) as SHARESTK_AMT,
                                 'CONT' as itemtype_code  
                            FROM LNCONTCOLL,     SHSHAREMASTER,  SHSHARETYPE  ,
                                 LNCONTMASTER,   
                                 LNLOANTYPE,   
                                 MBMEMBMASTER 
                           WHERE ( LNCONTCOLL.LOANCONTRACT_NO = LNCONTMASTER.LOANCONTRACT_NO ) and  
                                 ( LNCONTMASTER.LOANTYPE_CODE = LNLOANTYPE.LOANTYPE_CODE ) and  
                                 ( LNCONTMASTER.MEMBER_NO = MBMEMBMASTER.MEMBER_NO ) and  
                                 ( LNCONTCOLL.COOP_ID = LNCONTMASTER.COOP_ID ) and  
                                 ( LNCONTMASTER.COOP_ID = LNLOANTYPE.COOP_ID ) and  
                                 ( LNCONTMASTER.MEMCOOP_ID = MBMEMBMASTER.COOP_ID ) and  
                                 ( SHSHAREMASTER.MEMBER_NO = MBMEMBMASTER.MEMBER_NO )and
 								 ( SHSHAREMASTER.SHARETYPE_CODE = SHSHARETYPE.SHARETYPE_CODE )and
 								  ( SHSHAREMASTER.COOP_ID = SHSHARETYPE.COOP_ID ) and  
 								  ( shsharemaster.sharetype_code = '01' )  and 
                                 ( lncontcoll.ref_collno = '" + ref_collno + @"' ) AND  
                                 ( lncontcoll.loancolltype_code = '01' ) AND  
                                 ( lncontcoll.coll_status = 1 ) AND  
                                 ( lncontmaster.contract_status > 0 ) AND         
                                 LNCONTCOLL.COOP_ID = '" + state.SsCoopControl + @"'   ";

                        Sdt dt = WebUtil.QuerySdt(sql);

                        if (dt.Next())
                        {
                            int rowCount = dt.GetRowCount();


                            for (int x = 0; x < rowCount; x++)
                            {

                                member_no = dt.Rows[x]["MEMBER_NO"].ToString();
                                member_date = Convert.ToDateTime(dt.Rows[x]["MEMBER_DATE"]);
                                sum_balance = Convert.ToDecimal(dt.Rows[x]["sum_balance"]);
                                coll_percen = Convert.ToDecimal(dt.Rows[x]["COLL_PERCENT"]);
                                sharestk_ref = Convert.ToDecimal(dt.Rows[x]["SHARESTK_AMT"]);
                                loan_coll = dt.Rows[x]["LOANTYPE_CODE"].ToString();
                                use1 = Convert.ToDecimal(dt.Rows[x]["USE_AMT"]);
                                if (loan_coll != "22" || loan_coll != "26")
                                {
                                    use_amt_bf = sum_balance - sharestk_ref;
                                }
                                else { use_amt_bf = sum_balance; }


                                if (member_no_me != member_no)
                                {
                                    use_amt1 += use_amt_bf;
                                    member_no3 = member_no;
                                }
                                else
                                {
                                    use_amt2 += use_amt_bf;
                                    u_1 = 2;
                                }
                            }

                        }

                        Decimal collbalance = dw_coll.GetItemDecimal(row, "coll_balance");
                        //   principel_coll = collbalance - use_amt2;
                        Decimal use2 = 0;

                        if (u_1 == 2)
                        {
                            string sql1 = @"    SELECT LNCONTCOLL.LOANCONTRACT_NO,   
                                                         LNCONTCOLL.REF_COLLNO,   
                                                         LNCONTCOLL.DESCRIPTION,   
                                                         LNCONTMASTER.PRINCIPAL_BALANCE,   
                                                         LNCONTCOLL.COLL_PERCENT,   
                                                         LNCONTCOLL.LOANCOLLTYPE_CODE,   
                                                         LNCONTCOLL.USE_AMT,   
                                                         LNCONTCOLL.COLL_BALANCE,   
                                                         LNCONTCOLL.COLL_AMT  
                                                    FROM LNCONTCOLL,   
                                                         LNCONTMASTER  
                                                   WHERE ( LNCONTMASTER.LOANCONTRACT_NO = LNCONTCOLL.LOANCONTRACT_NO ) and  
                                                         ( LNCONTCOLL.COOP_ID = LNCONTMASTER.COOP_ID ) and  
                                                         ( ( lncontmaster.member_no = '" + member_no3 + @"' ) AND  
                                                         ( lncontmaster.contract_status > 0 ) AND  
                                                         ( lncontcoll.coll_status = 1 ) AND  
                                                         ( LNCONTMASTER.MEMCOOP_ID ='" + state.SsCoopControl + @"') AND  
                                                         ( LNCONTCOLL.LOANCOLLTYPE_CODE = '01' ) )    ";

                            Sdt dt1 = WebUtil.QuerySdt(sql1);

                            if (dt1.Next())
                            {

                                int rowCount1 = dt1.GetRowCount();


                                for (int x1 = 0; x1 < rowCount1; x1++)
                                {
                                    string member_no4 = dt1.Rows[x1]["REF_COLLNO"].ToString();
                                    sum_balance = Convert.ToDecimal(dt1.Rows[x1]["PRINCIPAL_BALANCE"]);
                                    coll_percen = Convert.ToDecimal(dt1.Rows[x1]["COLL_PERCENT"]);
                                    use_amt_bf3 = Convert.ToDecimal(dt1.Rows[x1]["USE_AMT"]);

                                    if (ref_collno != member_no4)
                                    {
                                        use_amt3 += use_amt_bf3;
                                    }

                                }

                            }
                            use_amt1 -= use_amt3;
                            principel_coll = collbalance - use_amt1;
                            //  LtServerMessage.Text = WebUtil.WarningMessage("สถานะแลกกันค้ำ");
                            balance_use = loanrequest_amt * (collbalance / total);

                            if (balance_use > principel_coll)
                            {//principel_coll
                                dw_coll.SetItemDecimal(row, "use_amt", wcf.Shrlon.of_roundmoney(state.SsWsPass, principel_coll));
                                dw_coll.SetItemDecimal(row, "coll_percent", (principel_coll / loanrequest));
                            }
                            else
                            {
                                dw_coll.SetItemDecimal(row, "use_amt", wcf.Shrlon.of_roundmoney(state.SsWsPass, balance_use));
                                dw_coll.SetItemDecimal(row, "coll_percent", (balance_use / loanrequest));
                            }


                        }
                        else
                        {

                            string sql1 = @"    SELECT LNCONTCOLL.LOANCONTRACT_NO,   
                                                         LNCONTCOLL.REF_COLLNO,   
                                                         LNCONTCOLL.DESCRIPTION,   
                                                         LNCONTMASTER.PRINCIPAL_BALANCE,   
                                                         LNCONTCOLL.COLL_PERCENT,   
                                                         LNCONTCOLL.LOANCOLLTYPE_CODE,   
                                                         LNCONTCOLL.USE_AMT,   
                                                         LNCONTCOLL.COLL_BALANCE,   
                                                         LNCONTCOLL.COLL_AMT  
                                                    FROM LNCONTCOLL,   
                                                         LNCONTMASTER  
                                                   WHERE ( LNCONTMASTER.LOANCONTRACT_NO = LNCONTCOLL.LOANCONTRACT_NO ) and  
                                                         ( LNCONTCOLL.COOP_ID = LNCONTMASTER.COOP_ID ) and  
                                                         ( ( lncontmaster.member_no = '" + member_no + @"' ) AND  
                                                         ( lncontmaster.contract_status > 0 ) AND  
                                                         ( lncontcoll.coll_status = 1 ) AND  
                                                         ( LNCONTMASTER.MEMCOOP_ID ='" + state.SsCoopControl + @"') AND  
                                                         ( LNCONTCOLL.LOANCOLLTYPE_CODE = '01' ) )    ";

                            Sdt dt1 = WebUtil.QuerySdt(sql1);

                            if (dt1.Next())
                            {

                                int rowCount1 = dt1.GetRowCount();


                                for (int x1 = 0; x1 < rowCount1; x1++)
                                {
                                    string member_no4 = dt1.Rows[x1]["REF_COLLNO"].ToString();
                                    sum_balance = Convert.ToDecimal(dt1.Rows[x1]["PRINCIPAL_BALANCE"]);
                                    coll_percen = Convert.ToDecimal(dt1.Rows[x1]["COLL_PERCENT"]);
                                    use_amt_bf3 = Convert.ToDecimal(dt1.Rows[x1]["USE_AMT"]);

                                    if (ref_collno != member_no4)
                                    {
                                        use_amt3 += use_amt_bf3;
                                    }

                                }

                            }

                            use_amt1 -= use_amt3;
                            principel_coll = collbalance - use_amt1;
                            if (principel_coll == total)
                            {
                                balance_use = loanrequest_amt;
                            }
                            else
                            {

                                balance_use = loanrequest_amt * (collbalance / total);

                            }
                            if (balance_use > principel_coll)
                            {//principel_coll
                                dw_coll.SetItemDecimal(row, "use_amt", wcf.Shrlon.of_roundmoney(state.SsWsPass, principel_coll));
                                dw_coll.SetItemDecimal(row, "coll_percent", (principel_coll / loanrequest));
                            }
                            else
                            {
                                dw_coll.SetItemDecimal(row, "use_amt", wcf.Shrlon.of_roundmoney(state.SsWsPass, balance_use));
                                dw_coll.SetItemDecimal(row, "coll_percent", (balance_use / loanrequest));
                            }

                        }

                    }
                    totalprincipel_ref = 0;
                    use_amt1 = 0;
                    use_amt2 = 0;
                    use_amt3 = 0;
                    use1 = 0;
                    row--;
                }

            }
            else if (ls_loantype == "26")
            {

                Decimal totalprincipel_ref = 0;// = new Decimal[2];
                Decimal coll_balance = 0;
                string loancolltype_code, member_no = "", member_no3 = "", loan_coll = "";
                Decimal total = 0;
                DateTime member_date;
                Decimal balance_use = 0, principel_coll = 0;
                Decimal sharestk_ref = 0;
                Decimal sharestk_value = dw_main.GetItemDecimal(1, "sharestk_value");
                string member_no_me = dw_main.GetItemString(1, "member_no");
                Decimal loanrequest_amt, loanrequest;
                loanrequest = dw_main.GetItemDecimal(1, "loanrequest_amt");
                loanrequest_amt = loanrequest;
                int row = dw_coll.RowCount;
                int i = 0;
                Decimal sum_balance = 0, coll_percen = 0, total_balance = 0, use_amt1 = 0, use_amt2 = 0, use_amt3 = 0, use_amt_bf3 = 0, use_amt_bf = 0;
                Decimal use1 = 0, u_1 = 0;
                for (i = 0; i < row; i++)
                {

                    loancolltype_code = dw_coll.GetItemString(i + 1, "loancolltype_code");

                    if (loancolltype_code == "01")
                    {
                        coll_balance = dw_coll.GetItemDecimal(i + 1, "coll_balance");

                    }

                    total = total + coll_balance;

                }

                while (row > 0)
                {


                    String ref_collno = dw_coll.GetItemString(row, "ref_collno");
                    string sql = @"  SELECT LNCONTCOLL.LOANCONTRACT_NO,   
                                 LNCONTMASTER.MEMBER_NO,                                    
                                 MBMEMBMASTER.MEMB_NAME,   LNCONTMASTER.LOANTYPE_CODE,
                                 MBMEMBMASTER.MEMB_SURNAME, MBMEMBMASTER.MEMBER_DATE,  
                                 lncontmaster.principal_balance + lncontmaster.withdrawable_amt as sum_balance,   
                                 LNCONTCOLL.COLL_PERCENT,    LNCONTCOLL.USE_AMT  ,(SHSHAREMASTER.SHARESTK_AMT*10) as SHARESTK_AMT,
                                 'CONT' as itemtype_code  
                            FROM LNCONTCOLL,     SHSHAREMASTER,  SHSHARETYPE  ,
                                 LNCONTMASTER,   
                                 LNLOANTYPE,   
                                 MBMEMBMASTER 
                           WHERE ( LNCONTCOLL.LOANCONTRACT_NO = LNCONTMASTER.LOANCONTRACT_NO ) and  
                                 ( LNCONTMASTER.LOANTYPE_CODE = LNLOANTYPE.LOANTYPE_CODE ) and  
                                 ( LNCONTMASTER.MEMBER_NO = MBMEMBMASTER.MEMBER_NO ) and  
                                 ( LNCONTCOLL.COOP_ID = LNCONTMASTER.COOP_ID ) and  
                                 ( LNCONTMASTER.COOP_ID = LNLOANTYPE.COOP_ID ) and  
                                 ( LNCONTMASTER.MEMCOOP_ID = MBMEMBMASTER.COOP_ID ) and  
                                 ( SHSHAREMASTER.MEMBER_NO = MBMEMBMASTER.MEMBER_NO )and
 								 ( SHSHAREMASTER.SHARETYPE_CODE = SHSHARETYPE.SHARETYPE_CODE )and
 								  ( SHSHAREMASTER.COOP_ID = SHSHARETYPE.COOP_ID ) and  
 								  ( shsharemaster.sharetype_code = '01' )  and 
                                 ( lncontcoll.ref_collno = '" + ref_collno + @"' ) AND  
                                 ( lncontcoll.loancolltype_code = '01' ) AND  
                                 ( lncontcoll.coll_status = 1 ) AND  
                                 ( lncontmaster.contract_status > 0 ) AND         
                                 LNCONTCOLL.COOP_ID = '" + state.SsCoopControl + @"'   ";

                    Sdt dt = WebUtil.QuerySdt(sql);

                    if (dt.Next())
                    {
                        int rowCount = dt.GetRowCount();


                        for (int x = 0; x < rowCount; x++)
                        {

                            member_no = dt.Rows[x]["MEMBER_NO"].ToString();
                            member_date = Convert.ToDateTime(dt.Rows[x]["MEMBER_DATE"]);
                            sum_balance = Convert.ToDecimal(dt.Rows[x]["sum_balance"]);
                            coll_percen = Convert.ToDecimal(dt.Rows[x]["COLL_PERCENT"]);
                            sharestk_ref = Convert.ToDecimal(dt.Rows[x]["SHARESTK_AMT"]);
                            loan_coll = dt.Rows[x]["LOANTYPE_CODE"].ToString();
                            use1 = Convert.ToDecimal(dt.Rows[x]["USE_AMT"]);
                            if (loan_coll == "21" || loan_coll == "23")
                            {
                                use_amt_bf = sum_balance - sharestk_ref;
                            }
                            else { use_amt_bf += sum_balance; }


                            if (member_no_me != member_no)
                            {
                                use_amt1 += use_amt_bf;
                                member_no3 = member_no;

                            }
                            else
                            {
                                use_amt2 += use_amt_bf;
                                u_1 = 2;
                            }
                        }

                    }

                    Decimal collbalance = dw_coll.GetItemDecimal(row, "coll_balance");
                    //   principel_coll = collbalance - use_amt2;
                    Decimal use2 = 0;

                    if (u_1 == 2)
                    {


                        string sql1 = @"    SELECT LNCONTCOLL.LOANCONTRACT_NO,   
                                                         LNCONTCOLL.REF_COLLNO,   
                                                         LNCONTCOLL.DESCRIPTION,   
                                                         LNCONTMASTER.PRINCIPAL_BALANCE,   
                                                         LNCONTCOLL.COLL_PERCENT,   
                                                         LNCONTCOLL.LOANCOLLTYPE_CODE,   
                                                         LNCONTCOLL.USE_AMT,   
                                                         LNCONTCOLL.COLL_BALANCE,   
                                                         LNCONTCOLL.COLL_AMT  
                                                    FROM LNCONTCOLL,   
                                                         LNCONTMASTER  
                                                   WHERE ( LNCONTMASTER.LOANCONTRACT_NO = LNCONTCOLL.LOANCONTRACT_NO ) and  
                                                         ( LNCONTCOLL.COOP_ID = LNCONTMASTER.COOP_ID ) and  
                                                         ( ( lncontmaster.member_no = '" + member_no3 + @"' ) AND  
                                                         ( lncontmaster.contract_status > 0 ) AND  
                                                         ( lncontcoll.coll_status = 1 ) AND  
                                                         ( LNCONTMASTER.MEMCOOP_ID ='" + state.SsCoopControl + @"') AND  
                                                         ( LNCONTCOLL.LOANCOLLTYPE_CODE = '01' ) )    ";

                        Sdt dt1 = WebUtil.QuerySdt(sql1);

                        if (dt1.Next())
                        {

                            int rowCount1 = dt1.GetRowCount();


                            for (int x1 = 0; x1 < rowCount1; x1++)
                            {
                                string member_no4 = dt1.Rows[x1]["REF_COLLNO"].ToString();
                                sum_balance = Convert.ToDecimal(dt1.Rows[x1]["PRINCIPAL_BALANCE"]);
                                coll_percen = Convert.ToDecimal(dt1.Rows[x1]["COLL_PERCENT"]);
                                use_amt_bf3 = Convert.ToDecimal(dt1.Rows[x1]["USE_AMT"]);

                                if (ref_collno != member_no4)
                                {
                                    use_amt3 += use_amt_bf3;
                                }

                            }

                        }
                        use_amt1 -= use_amt3;
                        principel_coll = collbalance - use_amt1;
                        //  LtServerMessage.Text = WebUtil.WarningMessage("สถานะแลกกันค้ำ");
                        balance_use = loanrequest_amt * (collbalance / total);
                        //principel_coll = collbalance - use_amt1;
                        //// LtServerMessage.Text = WebUtil.WarningMessage("สถานะแลกกันค้ำ");
                        //balance_use = loanrequest_amt * (collbalance / total);

                        if (balance_use > principel_coll)
                        {//principel_coll
                            dw_coll.SetItemDecimal(row, "use_amt", wcf.Shrlon.of_roundmoney(state.SsWsPass, balance_use));
                            dw_coll.SetItemDecimal(row, "coll_percent", (balance_use / loanrequest));
                        }
                        else
                        {
                            dw_coll.SetItemDecimal(row, "use_amt", wcf.Shrlon.of_roundmoney(state.SsWsPass, balance_use));
                            dw_coll.SetItemDecimal(row, "coll_percent", (balance_use / loanrequest));
                        }


                    }
                    else
                    {
                        string sql1 = @"    SELECT LNCONTCOLL.LOANCONTRACT_NO,   
                                                         LNCONTCOLL.REF_COLLNO,   
                                                         LNCONTCOLL.DESCRIPTION,   
                                                         LNCONTMASTER.PRINCIPAL_BALANCE,   
                                                         LNCONTCOLL.COLL_PERCENT,   
                                                         LNCONTCOLL.LOANCOLLTYPE_CODE,   
                                                         LNCONTCOLL.USE_AMT,   
                                                         LNCONTCOLL.COLL_BALANCE,   
                                                         LNCONTCOLL.COLL_AMT  
                                                    FROM LNCONTCOLL,   
                                                         LNCONTMASTER  
                                                   WHERE ( LNCONTMASTER.LOANCONTRACT_NO = LNCONTCOLL.LOANCONTRACT_NO ) and  
                                                         ( LNCONTCOLL.COOP_ID = LNCONTMASTER.COOP_ID ) and  
                                                         ( ( lncontmaster.member_no = '" + member_no + @"' ) AND  
                                                         ( lncontmaster.contract_status > 0 ) AND  
                                                         ( lncontcoll.coll_status = 1 ) AND  
                                                         ( LNCONTMASTER.MEMCOOP_ID ='" + state.SsCoopControl + @"') AND  
                                                         ( LNCONTCOLL.LOANCOLLTYPE_CODE = '01' ) )    ";

                        Sdt dt1 = WebUtil.QuerySdt(sql1);
                        int rowCount1 = 0;
                        if (dt1.Next())
                        {

                            rowCount1 = dt1.GetRowCount();


                            for (int x1 = 0; x1 < rowCount1; x1++)
                            {
                                string member_no4 = dt1.Rows[x1]["REF_COLLNO"].ToString();
                                sum_balance = Convert.ToDecimal(dt1.Rows[x1]["PRINCIPAL_BALANCE"]);
                                coll_percen = Convert.ToDecimal(dt1.Rows[x1]["COLL_PERCENT"]);
                                use_amt_bf3 = Convert.ToDecimal(dt1.Rows[x1]["USE_AMT"]);

                                if (ref_collno != member_no4)
                                {
                                    use_amt3 += use_amt_bf3;
                                }

                            }

                        }

                        use_amt1 -= use_amt3;
                        principel_coll = collbalance - use_amt1;

                        if (principel_coll == total)
                        {
                            balance_use = loanrequest_amt;
                        }
                        else
                        {

                            balance_use = loanrequest_amt * (collbalance / total);

                        }
                        if (balance_use > principel_coll)
                        {//principel_coll
                            dw_coll.SetItemDecimal(row, "use_amt", wcf.Shrlon.of_roundmoney(state.SsWsPass, principel_coll));
                            dw_coll.SetItemDecimal(row, "coll_percent", (principel_coll / loanrequest));
                        }
                        else
                        {
                            dw_coll.SetItemDecimal(row, "use_amt", wcf.Shrlon.of_roundmoney(state.SsWsPass, balance_use));
                            dw_coll.SetItemDecimal(row, "coll_percent", (balance_use / loanrequest));
                        }

                    }


                    totalprincipel_ref = 0;
                    use_amt1 = 0;
                    use_amt2 = 0;
                    use_amt3 = 0;
                    use1 = 0;
                    row--;
                }


            }
            else
            {
                Decimal coll_balance = 0;
                string loancolltype_code;
                Decimal total = 0;

                Decimal sharestk_value = dw_main.GetItemDecimal(1, "sharestk_value");
                Decimal loanrequest_amt = dw_main.GetItemDecimal(1, "loanrequest_amt");
                //loanrequest_amt = loanrequest_amt - sharestk_value;
                int row = dw_coll.RowCount;
                int i = 0;
                for (i = 0; i < row; i++)
                {

                    loancolltype_code = dw_coll.GetItemString(i + 1, "loancolltype_code");

                    if (loancolltype_code == "01")
                    {
                        coll_balance = dw_coll.GetItemDecimal(i + 1, "coll_balance");

                    }
                    total = total + coll_balance;
                }

                while (row > 0)
                {

                    Decimal collbalance = dw_coll.GetItemDecimal(row, "coll_balance");
                    Decimal balance = loanrequest_amt * (collbalance / total);

                    dw_coll.SetItemDecimal(row, "use_amt", balance);

                    row--;
                }
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
            strList.loanrequest_tdate = dw_main.GetItemDateTime(1, "loanrequest_date");
            Session["strItemchange"] = strList;
        }

        private void JsPostColl()
        {
            //try
            //{

            //    String columnName = "ref_collno";
            //    if (HdColumnName.Value == "" || HdColumnName.Value == "setcolldetail") { columnName = "setcolldetail"; }
            //    String dwMainXML = dw_main.Describe("DataWindow.Data.XML");
            //    String dwCollXML = dw_coll.Describe("DataWindow.Data.XML");
            //    String dwClearXML = dw_clear.Describe("DataWindow.Data.XML");
            //    String t = dw_main.GetItemString(1, "loantype_code");
            //    str_itemchange strList = new str_itemchange();

            //    // ตรวจสอบว่า HdRowNumber มีค่าหรือไม่ ถ้าไม่มี ไม่ต้องทำอะไร
            //    if (HdRowNumber.Value.ToString() != "")
            //    {
            //        int rowNumber = Convert.ToInt32(HdRowNumber.Value.ToString());

            //        HdColumnName.Value = "";
            //        if ((columnName == "checkmancollperiod") || (columnName == "setcolldetail"))
            //        {
            //            dw_coll.SetItemString(rowNumber, "ref_collno", HdRefcollNO.Value);
            //            dwCollXML = dw_coll.Describe("DataWindow.Data.XML");
            //        }
            //        String refCollNo = "";
            //        try
            //        {
            //            refCollNo = dw_coll.GetItemString(rowNumber, "ref_collno");
            //            HdRefcollNO.Value = refCollNo;
            //        }
            //        catch
            //        {
            //            refCollNo = "";
            //        }

            //        strList.column_name = columnName;
            //        strList.xml_main = dwMainXML;
            //        strList.xml_guarantee = dwCollXML;
            //        strList.xml_clear = dwClearXML;
            //        strList.import_flag = true;
            //        strList.format_type = "CAT";

            //        strList.column_data = dw_coll.GetItemString(rowNumber, "loancolltype_code") + refCollNo;
            //        // เรียก service สำหรับการเปลี่ยนแปลงค่า ของ DW_coll
            //        int result = shrlonService.LoanRightItemChangeColl(state.SsWsPass, ref strList);
            //        Session["strItemchange"] = strList;
            //        //if ((strList.xml_message != null) && (strList.xml_message != ""))
            //        //{
            //        //    //dw_message.Reset();MO
            //        //    //dw_message.ImportString(strList.xml_message, FileSaveAsType.Xml);
            //        //    DwUtil.ImportData(strList.xml_message, dw_message, null, FileSaveAsType.Xml);
            //        //    HdMsg.Value = dw_message.GetItemString(1, "msgtext");
            //        //    HdMsgWarning.Value = dw_message.GetItemString(1, "msgicon");
            //        //}
            //        if (result == 8)
            //        {
            //            HdReturn.Value = result.ToString();
            //            HdColumnName.Value = strList.column_name;
            //        }
            //        //try
            //        //{
            //        //    dw_otherclr.Reset();
            //        //    dw_otherclr.ImportString(LtXmlOtherlr.Text, FileSaveAsType.Xml);
            //        //}
            //        //catch { dw_otherclr.Reset(); }
            //        try
            //        {
            //            dw_coll.Reset();
            //            dw_coll.ImportString(strList.xml_guarantee, FileSaveAsType.Xml);
            //        }
            //        catch { dw_coll.Reset(); }

            //    }
            //}
            //catch (Exception ex)
            //{
            //    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            //}

        }

        /// <summary>
        /// sum ชำระหนี้อืนทั้งหมด
        /// </summary>       
        private void JsSumOthClr()
        {
            DateTime loanrcvfix_date = dw_main.GetItemDate(1, "loanrcvfix_date");
            DateTime loanrequest_date = dw_main.GetItemDate(1, "loanrequest_date");
            String ls_memcoopid;
            Decimal ldc_contintrate = dw_main.GetItemDecimal(1, "int_contintrate");
            int year = Convert.ToInt16(loanrcvfix_date.Year + 543);
            short month_bf = Convert.ToInt16(loanrcvfix_date.Month - 1);
            short month = Convert.ToInt16(loanrcvfix_date.Month);
            short month_af = Convert.ToInt16(loanrcvfix_date.Month + 1);

            DateTime ldtm_processdate = new DateTime();
            DateTime startkeep_date = dw_main.GetItemDate(1, "startkeep_date");
            //a hardcode
            String sqlpro = " SELECT MAX(LASTPROCESS_DATE)as LASTPROCESS_DATE FROM  LNCONTMASTER ";
            Sdt dtpro = WebUtil.QuerySdt(sqlpro);
            if (dtpro.Next())
            {
                try { ldtm_processdate = dtpro.GetDate("LASTPROCESS_DATE"); }
                catch { ldtm_processdate = new DateTime(); }
            }

            if (month_af > 12)
            {

                month_af = 1;
                year = year + 1;

            }
            if (month_bf > 12)
            {

                month_bf = 1;
                year = year - 1;

            }
            int day_amount = 0;
            //ex  เดือน 06 ====> ตัดยอด เย็น 2506  {เงินเดือนออก 2606  หุ้นเพิ่ม หนี้ลด เรียกเก็บเดือน 07  เย็น 2706} ไม่มีรายการคืน วันที่ 2806 มีรายการคืน ,
            //ex  เดือน 07 ====> ตัดยอด เย็น 2507  {เงินเดือนออก 2607  หุ้นเพิ่ม หนี้ลด เรียกเก็บเดือน 08  เย็น 2707} ไม่มีรายการคืน วันที่ 2807 มีรายการคืน ,  
            //เดือนก่อน 
            DateTime postingdate_bf = wcf.Busscom.of_getpostingdate2(state.SsWsPass, Convert.ToInt16(loanrcvfix_date.Year + 543), Convert.ToInt16(loanrcvfix_date.Month));
            //เดือนปัจุจบัน
            DateTime postingdate = wcf.Busscom.of_getpostingdate(state.SsWsPass, loanrcvfix_date);
            //เดือนหลัง
            DateTime postingdate_af = wcf.Busscom.of_getpostingdate(state.SsWsPass, postingdate.AddMonths(1));

            short ai_increase = 0;
            DateTime ldtm_loanrequest = new DateTime();
            DateTime ldtm_postingdate = new DateTime();
            //ประเภคบุคคลค้ำ จ่ายถัดวันขอกู้อีก 3 วันทำการ
            //a hardcode

            ai_increase = 3;
            ldtm_loanrequest = wcf.Busscom.of_relativeworkdate(state.SsWsPass, loanrequest_date, ai_increase);
            ldtm_postingdate = wcf.Busscom.of_relativeworkdate(state.SsWsPass, postingdate_bf, 1);

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
                for (j = 0; j < row_clear; j++)
                {
                    Decimal loanpayment_type = dw_clear.GetItemDecimal(j + 1, "loanpayment_type");
                    string ls_contno = dw_clear.GetItemString(j + 1, "loancontract_no");
                    try
                    {
                        clear_status = dw_clear.GetItemDecimal(j + 1, "clear_status");
                    }
                    catch { clear_status = 0; }
                    if (clear_status == 1)
                    {

                        try
                        {
                            principal_balance = dw_clear.GetItemDecimal(j + 1, "principal_balance");
                        }
                        catch { principal_balance = 0; }
                        try
                        {
                            if (loanpayment_type == 1)
                            {
                                if (ldtm_loanrequest >= postingdate_bf && ldtm_loanrequest <= startkeep_date)
                                {
                                    day_amount = loanrcvfix_date.DayOfYear - postingdate_bf.DayOfYear;
                                    principal_balance = dw_clear.GetItemDecimal(j + 1, "principal_balance");
                                    intestimate_amt = principal_balance * (ldc_contintrate) * day_amount / 365;
                                    intestimate_amt = wcf.Shrlon.of_roundmoney(state.SsWsPass, intestimate_amt);

                                }
                                else
                                {
                                    //if (ldtm_processdate == startkeep_date && loanrcvfix_date > postingdate_bf)
                                    //{
                                    //    day_amount = loanrcvfix_date.DayOfYear - postingdate_bf.DayOfYear;
                                    //    principal_balance = dw_clear.GetItemDecimal(j + 1, "principal_balance");
                                    //    intestimate_amt = principal_balance * (ldc_contintrate) * day_amount / 365;
                                    //    intestimate_amt = wcf.Shrlon.of_roundmoney(state.SsWsPass, intestimate_amt);

                                    //}
                                    //else
                                    //{
                                    intestimate_amt = wcf.Shrlon.of_roundmoney(state.SsWsPass, (wcf.Shrlon.of_computeinterest(state.SsWsPass, ls_memcoopid, ls_contno, loanrcvfix_date)));


                                    //  }

                                }
                                dw_clear.SetItemDecimal(j + 1, "intestimate_amt", intestimate_amt);
                            }
                            else { dw_clear.SetItemDecimal(j + 1, "intestimate_amt", 0); }

                        }
                        catch { intestimate_amt = 0; }


                        sum_clear1 = sum_clear1 + principal_balance + intestimate_amt;

                    }
                    else if (clear_status == 0)
                    {
                        if (loanpayment_type == 1)
                        {
                            day_amount = postingdate.DayOfYear - postingdate_bf.DayOfYear;
                            principal_balance = dw_clear.GetItemDecimal(j + 1, "principal_balance");
                            intestimate_amt = principal_balance * (ldc_contintrate) * day_amount / 365;
                            intestimate_amt = wcf.Shrlon.of_roundmoney(state.SsWsPass, intestimate_amt);
                            dw_clear.SetItemDecimal(j + 1, "intestimate_amt", intestimate_amt);
                        }
                        else { dw_clear.SetItemDecimal(j + 1, "intestimate_amt", 0); }
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
        /// ตรวจสอบสิทธิ์ตามเงินเดือน
        /// </summary>
        protected void JsPermissSalary()
        {

            string ls_memno, ls_loantype, ls_contno, ls_desc, ls_refreqloop;
            int li_index, li_count, li_exist, li_month;
            int li_clrstatus, li_paytype, li_shrpaystatus, li_signflag;
            int ll_row;
            Decimal ldc_shrperiod, ldc_payment, ldc_intestm, ldc_sumloan, ldc_reqpayment;
            Decimal ldc_sumpay, ldc_minsalaryamt, ldc_salary, ldc_reqloopclr, ldc_suminc;
            Decimal ldc_incomemthfix, ldc_intcomeoth, ldc_paymonthoth;
            Decimal ldc_minsalaryperc;

            try
            {

                ls_memno = dw_main.GetItemString(1, "member_no");
                ldc_salary = dw_main.GetItemDecimal(1, "salary_amt");

                try { ldc_minsalaryperc = dw_main.GetItemDecimal(1, "minsalary_perc"); }
                catch { ldc_minsalaryperc = 0; }
                try { ldc_minsalaryamt = dw_main.GetItemDecimal(1, "minsalary_amt"); }
                catch { ldc_minsalaryamt = 0; }
                try { ls_refreqloop = dw_main.GetItemString(1, "refreqloop_docno"); }
                catch { ls_refreqloop = ""; }
                try { ldc_incomemthfix = dw_main.GetItemDecimal(1, "incomemonth_fixed"); }
                catch { ldc_incomemthfix = 0; }
                try { ldc_intcomeoth = dw_main.GetItemDecimal(1, "incomemonth_other"); }
                catch { ldc_intcomeoth = 0; }
                try { ldc_paymonthoth = dw_main.GetItemDecimal(1, "paymonth_other"); }
                catch { ldc_paymonthoth = 0; }

                ldc_salary = ldc_salary + ldc_incomemthfix + ldc_intcomeoth;
                //75%
                Decimal percen_salary = ldc_salary - Convert.ToDecimal((Convert.ToDouble(ldc_salary) * 0.25));



                ldc_shrperiod = 0;
                ldc_sumloan = 0;
                ldc_reqpayment = 0;
                ldc_reqloopclr = 0;

                // ดึงรายการหุ้น
                ldc_shrperiod = dw_main.GetItemDecimal(1, "periodshare_value");
                li_shrpaystatus = Convert.ToInt32(dw_main.GetItemDecimal(1, "sharepay_status"));

                // ถ้างดเก็บค่าุหุ้นให้หุ้นต่อเดือนเป็นศูนย์
                if (li_shrpaystatus == -1) { ldc_shrperiod = 0; }


                // ดึงยอดเงินชำระต่อเดือนใบขอกู้
                ls_loantype = dw_main.GetItemString(1, "loantype_code");
                li_paytype = Convert.ToInt32(dw_main.GetItemDecimal(1, "loanpayment_type"));
                ldc_intestm = dw_main.GetItemDecimal(1, "intestimate_amt");
                ldc_payment = dw_main.GetItemDecimal(1, "period_payment");

                Decimal ldc_payamt = dw_main.GetItemDecimal(1, "period_payamt");

                ldc_sumpay = ldc_payment;

                if (li_paytype == 1)
                {
                    ldc_sumpay += ldc_intestm;
                }

                // ดึงรายการหนี้
                li_count = dw_clear.RowCount;
                for (li_index = 1; li_index <= li_count; li_index++)
                {

                    li_clrstatus = Convert.ToInt32(dw_clear.GetItemDecimal(li_index, "clear_status"));
                    if (li_clrstatus == 0)
                    {
                        li_paytype = Convert.ToInt32(dw_clear.GetItemDecimal(li_index, "loanpayment_type"));
                        ldc_payment = dw_clear.GetItemDecimal(li_index, "period_payment");
                        ldc_intestm = dw_clear.GetItemDecimal(li_index, "intestimate_amt");
                        ldc_sumpay += ldc_payment + ldc_intestm;
                    }
                }

                ldc_sumpay = ldc_sumpay + ldc_shrperiod;
                dw_main.SetItemDecimal(1, "paymonth_coop", ldc_sumpay);
                if (ldc_sumpay > percen_salary) { LtServerMessage.Text = WebUtil.WarningMessage("สิทธิ์ตามเงินเดือนคงเหลือไม่ผ่าน"); }
            }
            catch (Exception ex)
            {
                //LtServerMessage.Text = WebUtil.ErrorMessage("JsPermissSalary===>" + ex);
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

        protected void GenpermissCollLoop(object sender, EventArgs e)
        {
            CollLoop();
        }

        private void CollLoop()
        {
            try
            {
                string as_coopid = state.SsCoopControl;
                String ls_memcoopid = dw_main.GetItemString(1, "memcoop_id");
                String ls_membtypedesc = dw_main.GetItemString(1, "membtype_desc");
                String member_no = dw_main.GetItemString(1, "member_no");
                DateTime ldtm_member;
                bool isChecked = Checkcollloop.Checked;
                if (isChecked)
                {
                    try
                    {
                        ldtm_member = dw_main.GetItemDateTime(1, "member_date");
                    }
                    catch { ldtm_member = state.SsWorkDate; }
                    ///<หาอายุงานของาสมาชิก>
                    int memtime = 0;
                    Decimal member_age = dw_main.GetItemDecimal(1, "member_age");
                    if (member_age > 1)
                    {
                        memtime = Convert.ToInt32(BusscomService.of_cal_yearmonth(state.SsWsPass, ldtm_member, state.SsWorkDate) * 12);

                    }
                    else
                    {

                        memtime = Convert.ToInt32(member_age * 100);
                    }
                    String[] max_creditperiod = new String[5];
                    Decimal per70 = new Decimal(0.7);
                    Decimal per90 = new Decimal(0.9);
                    Decimal loancredit_amt = 0;
                    Decimal ldc_maxperiod = 0;

                    string ls_loantype = dw_main.GetItemString(1, "loantype_code");
                    ///< หาสิทธิ์กู้สูงสุด,งวดสูงสุด>
                    ///CalloanpermissCollLoop(string wsPass, String memcoop_id, String as_coopid, String ls_loantype, String ref_collno, DateTime dltm_workdate)
                    max_creditperiod = shrlonService.CalloanpermissCollLoop(state.SsWsPass, ls_memcoopid, as_coopid, ls_loantype, member_no, memtime);

                    loancredit_amt = Convert.ToDecimal(max_creditperiod[0]);
                    ldc_maxperiod = Convert.ToDecimal(max_creditperiod[1]);

                    dw_main.SetItemDecimal(1, "loancredit_amt", loancredit_amt);
                    dw_main.SetItemDecimal(1, "maxsend_payamt", ldc_maxperiod);
                    dw_main.SetItemDecimal(1, "period_payamt", ldc_maxperiod);

                }

                else { Jsmaxcreditperiod(); }
            }
            catch (Exception ex)
            {
                //LtServerMessage.Text = WebUtil.ErrorMessage("GenpermissCollLoop==>" + ex); 
            }



        }

        //   สำหรับคำนวณดอกเบี้ยประมาณการสำหรับสัญญาใหม่
        public Decimal of_calintestimatemain()
        {
            string ls_continttabcode, ls_coopid;

            int li_continttype, li_intsteptype;
            Decimal ldc_apvamt, ldc_inttotal = 0, ldc_prncalint;
            Decimal ldc_contintrate, ldc_intincrease;
            DateTime ldtm_calintfrom, ldtm_calintto, ldtm_request, ldtm_estimate;
            try
            {
                // ข้อกำหนดเรื่องดอกเบี้ย
                li_continttype = Convert.ToInt32(dw_main.GetItemDecimal(1, "int_continttype"));
                ls_continttabcode = dw_main.GetItemString(1, "int_continttabcode");
                ldc_contintrate = dw_main.GetItemDecimal(1, "int_contintrate");
                ldc_intincrease = dw_main.GetItemDecimal(1, "int_contintincrease");
                li_intsteptype = Convert.ToInt32(dw_main.GetItemDecimal(1, "int_intsteptype"));
                ls_coopid = dw_main.GetItemString(1, "coop_id");


                ldc_apvamt = dw_main.GetItemDecimal(1, "loanrequest_amt");
                ldc_prncalint = dw_main.GetItemDecimal(1, "loanrequest_amt");
                ldtm_request = dw_main.GetItemDateTime(1, "loanrequest_date");


                ldtm_estimate = ldtm_request.AddDays(31);


                // ถ้าไม่มียอดต้นที่จะคิด ด/บ เป็น 0
                if (ldc_prncalint <= 0) { return 0; }

                // ถ้าสถานะนี้เป็นไม่คิด ด/บ
                if (li_continttype == 0) { return 0; }


                // ถ้าวันที่คิดดอกเบี้ยถึงน้อยกว่าวันที่คิดดอกเบี้ยล่าสุด ด/บ 0
                if (ldtm_estimate <= ldtm_request) { return 0; }

                // ถ้าไม่ได้ set รูปแบบขั้นดอกเบี้ยไว้ set ให้เป็นจากยอดอนุมัติ
                if (li_intsteptype == null) { li_intsteptype = 1; }

                ldtm_calintfrom = ldtm_request;
                ldtm_calintto = ldtm_estimate;

                switch (li_continttype.ToString())
                {
                    case "1":	// ตาม rate ที่ระบุ

                        //// อัตราด/บเพิ่มลดพิเศษ
                        //inv_intsrv.of_setintincrease( 0 );

                        //ldc_inttotal	= inv_intsrv.of_computeinterest( ldc_contintrate, ldtm_calintfrom, ldtm_calintto, ldc_prncalint );
                        break;
                    case "2":	// ตามตารางด/บที่ระบุ

                        // อัตราด/บเพิ่มลดพิเศษ
                        //inv_intsrv.of_setintincrease( ldc_intincrease );

                        // ตรวจว่าดูอัตราด/บจากยอดอนุมัติหรือคงเหลือ
                        //if (li_intsteptype == 1)
                        //{
                        //    ldc_inttotal = wcf.Shrlon.of_computeinterest2(state.SsWsPass, ls_continttabcode, ls_coopid, ldtm_calintfrom, ldtm_calintto, ldc_prncalint, ldc_apvamt);
                        //}
                        //else
                        //{
                        //    ldc_inttotal = wcf.Shrlon.of_computeinterest3(state.SsWsPass, ls_continttabcode, ls_coopid, ldtm_calintfrom, ldtm_calintto, ldc_prncalint);
                        //}
                        break;

                }


                try
                {
                    DateTime processdate = new DateTime();
                    DateTime startkeep_date = dw_main.GetItemDateTime(1, "startkeep_date");
                    DateTime loanrcvfix_date = dw_main.GetItemDateTime(1, "loanrcvfix_date");
                    DateTime loanrequest_date = dw_main.GetItemDateTime(1, "loanrequest_date");
                    short year = Convert.ToInt16(loanrcvfix_date.Year + 543);
                    short month = Convert.ToInt16(loanrcvfix_date.Month);
                    int day_amount = 31;

                    //DateTime postingdate_bf = wcf.Busscom.of_getpostingdate2(state.SsWsPass, Convert.ToInt16(loanrcvfix_date.Year + 543), Convert.ToInt16(loanrcvfix_date.Month));
                    //DateTime postingdate = wcf.Busscom.of_getpostingdate2(state.SsWsPass, Convert.ToInt16(postingdate_bf.Year + 543), Convert.ToInt16(postingdate_bf.Month + 1));
                    //DateTime postingdate_af = wcf.Busscom.of_getpostingdate2(state.SsWsPass, Convert.ToInt16(postingdate.Year + 543), Convert.ToInt16(postingdate.Month + 1));
                    ////  processdate = wcf.Busscom.of_getpostingdate(state.SsWsPass, ldtm_loanreceive_af);
                    DateTime postingdate_bf = wcf.Busscom.of_getpostingdate2(state.SsWsPass, Convert.ToInt16(loanrcvfix_date.Year + 543), Convert.ToInt16(loanrcvfix_date.Month));
                    short year_bf = Convert.ToInt16(postingdate_bf.Year + 543);
                    short month_bf = Convert.ToInt16(postingdate_bf.Month + 1);
                    if (month_bf > 12)
                    {
                        month_bf = 1;
                        year_bf = Convert.ToInt16(postingdate_bf.Year + 544);
                    }

                    DateTime postingdate = wcf.Busscom.of_getpostingdate2(state.SsWsPass, year_bf, month_bf);
                    short year_pro = Convert.ToInt16(postingdate.Year + 543);
                    short month_pro = Convert.ToInt16(postingdate.Month + 1);
                    if (month_pro > 12)
                    {
                        month_pro = 1;
                        year_pro = Convert.ToInt16(postingdate.Year + 544);
                    }
                    DateTime postingdate_af = wcf.Busscom.of_getpostingdate2(state.SsWsPass, year_pro, month_pro);
                    short ai_increase = 0;
                    DateTime ldtm_loanrequest = new DateTime();
                    DateTime ldtm_postingdate = new DateTime();
                    //ประเภคบุคคลค้ำ จ่ายถัดวันขอกู้อีก 3 วันทำการ
                    //a hardcode

                    ai_increase = 3;
                    ldtm_loanrequest = wcf.Busscom.of_relativeworkdate(state.SsWsPass, loanrequest_date, ai_increase);
                    ldtm_postingdate = wcf.Busscom.of_relativeworkdate(state.SsWsPass, postingdate_bf, 1);



                    //จ่ายเงินกู้หลังเรียกเก็บหรือไม่
                    if (loanrcvfix_date < postingdate && loanrcvfix_date <= postingdate_bf)
                    {

                        day_amount = 366 + postingdate.DayOfYear - loanrcvfix_date.DayOfYear;

                    }
                    else if (ldtm_loanrequest == postingdate_bf || ldtm_loanrequest <= ldtm_postingdate)
                    {
                        //a 20-07 ก่อนเรียกเก็บ 
                        day_amount = 366 + postingdate.DayOfYear - loanrcvfix_date.DayOfYear;
                    }
                    else if (loanrcvfix_date > postingdate_bf)
                    {


                        postingdate = wcf.Busscom.of_getpostingdate2(state.SsWsPass, Convert.ToInt16(postingdate_af.Year + 543), Convert.ToInt16(postingdate_af.Month));

                        day_amount = 366 + startkeep_date.DayOfYear - loanrcvfix_date.DayOfYear;

                    }
                    else
                    {

                        day_amount = 366 + startkeep_date.DayOfYear - loanrcvfix_date.DayOfYear;
                        //postingdate_af.DayOfYear - loanrcvfix_date.DayOfYear;
                    }

                    // day_amount = loanrcvfix_date.DayOfYear

                    ldc_inttotal = ldc_apvamt * ldc_contintrate * day_amount / 365;
                    ldc_inttotal = wcf.Shrlon.of_roundmoney(state.SsWsPass, ldc_inttotal);
                }
                catch { }


            }
            catch (Exception ex)
            {
                //LtServerMessage.Text = WebUtil.ErrorMessage("of_calintestimatemain==+>" + ex);
            }

            return ldc_inttotal;

        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            XmlConfigService xml = new XmlConfigService();
            if (xml.ShrlonPrintMode == 0)
            {
                JspopupCollReport();
            }
            else
            {
                JspopupCollReport(true);
            }
        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            XmlConfigService xml = new XmlConfigService();
            if (xml.ShrlonPrintMode == 0)
            {
                JspopupAgreeCollReport();
            }
            else
            {
                JspopupAgreeCollReport(true, xml.ShrlonPrintMode);
            }
        }

        protected void LinkButton3_Click(object sender, EventArgs e)
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

        protected void LinkButton4_Click(object sender, EventArgs e)
        {
            XmlConfigService xml = new XmlConfigService();
            if (xml.ShrlonPrintMode == 0)
            {
                JspopupAgreeLoanReport();
            }
            else
            {
                JspopupAgreeLoanReport(true, xml.ShrlonPrintMode);
            }
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
        //        Saving.WcfReport.ReportClient lws_report = wcf.Report;
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
        //        Saving.WcfReport.ReportClient lws_report = wcf.Report;
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
                    if (rowCount == 1)
                    {

                        for (int x = 0; x < rowCount; x++)
                        {

                            dept_acc = dtdept.Rows[0]["DEPTACCOUNT_NO"].ToString();
                            if (x == 0)
                            {
                                dw_main.SetItemString(1, "expense_accid", dept_acc);
                            }
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