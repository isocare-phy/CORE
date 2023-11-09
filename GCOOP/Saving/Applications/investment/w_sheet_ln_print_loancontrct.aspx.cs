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
using CoreSavingLibrary.WcfReport;
namespace Saving.Applications.investment
{
    public partial class w_sheet_ln_print_loancontrct : PageWebSheet, WebSheet
    {
        protected String GetLoanReqData;
        protected String jspostCollChanged;
        protected String jspostMainChanged;
        protected String jsReport;
        protected String jsReport2;
        protected String jsReport3;

        private DwThDate tRcvDate;
        private DwThDate tCollDate;
        private InvestmentClient svInvest;

        public String pbl = "print_loancontrct.pbl";

        //Report
        public String app = "";
        public String gid = "";
        public String rid = "";
        protected String pdf = "";

        public void InitJsPostBack()
        {
            tRcvDate = new DwThDate(DwMain, this);
            tRcvDate.Add("contsign_date", "contsign_tdate");
            tRcvDate.Add("startpay_date", "startpay_tdate");

            tCollDate = new DwThDate(DwColl, this);
            tCollDate.Add("birth_date", "birth_tdate");

            GetLoanReqData = WebUtil.JsPostBack(this, "GetLoanReqData");
            jspostCollChanged = WebUtil.JsPostBack(this, "jspostCollChanged");
            jspostMainChanged = WebUtil.JsPostBack(this, "jspostMainChanged");
            jsReport = WebUtil.JsPostBack(this, "jsReport");
            jsReport2 = WebUtil.JsPostBack(this, "jsReport2");
            jsReport3 = WebUtil.JsPostBack(this, "jsReport3");
        }

        public void WebSheetLoadBegin()
        {
            
            svInvest = wcf.Investment;
            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwSignlist1);
                this.RestoreContextDw(DwSignlist2);
                this.RestoreContextDw(DwColl, tCollDate);
            }
        }
        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "GetLoanReqData":
                    GetLnReqDetail();
                    break;
                case "jspostCollChanged":
                    break;
                case "jspostMainChanged":
                    break;
                case "jsReport":
                    RunProcess();
                    break;
                case "jsReport2":
                    RunProcess2();
                    break;
                case "jsReport3":
                    RunProcess3();
                    break;
                case "popupReport":
                    PopupReport();
                    break;  
      
            }
        }

        public void SaveWebSheet()
        {
            for (int i = 1; i <= DwSignlist1.RowCount; i++)
            {
                DwSignlist1.SetItemDecimal(i, "contsign_type", 1);
            }

            for (int i = 1; i <= DwSignlist2.RowCount; i++)
            {
                DwSignlist2.SetItemDecimal(i, "contsign_type", 2);
            }

            tCollDate.Thai2EngAllRow();
            tRcvDate.Thai2EngAllRow();
    
            str_lcprintcont lcreqloan = new str_lcprintcont();
            lcreqloan.coop_id = state.SsCoopControl;
            lcreqloan.xml_contdetail = DwMain.Describe("Datawindow.Data.XML");
            lcreqloan.xml_signlist1 = DwSignlist1.Describe("Datawindow.Data.XML");
            lcreqloan.xml_signlist2 = DwSignlist2.Describe("Datawindow.Data.XML");
            lcreqloan.xml_contcoll = DwColl.Describe("Datawindow.Data.XML");
            try
            {
                short result = svInvest.of_save_printcontract(state.SsWsPass, ref lcreqloan);
                if (result == 1)
                {
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร้จ");
                }
            }catch(Exception ex){
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void WebSheetLoadEnd()
        {
            tRcvDate.Eng2ThaiAllRow();
            tCollDate.Eng2ThaiAllRow();
            try
            {
                DwUtil.RetrieveDDDW(DwColl, "bdrank_code", pbl, null);
            }
            catch { }

            DwMain.SaveDataCache();
            DwSignlist1.SaveDataCache();
            DwSignlist2.SaveDataCache();
            DwColl.SaveDataCache();
        }
        #region Function
        private void GetLnReqDetail()
        {
            str_lcprintcont lcreqloan = new str_lcprintcont();
            String LOANREQUEST_DOCNO = "";

            LOANREQUEST_DOCNO = Hfdoc_no.Value;
            try
            {
                lcreqloan.coop_id = state.SsCoopId;
                lcreqloan.loanrequest_docno = LOANREQUEST_DOCNO;

                DwMain.Reset();
                DwSignlist1.Reset();
                DwSignlist2.Reset();
                DwColl.Reset();
                try
                {
                    short result = svInvest.of_init_printcontract(state.SsWsPass, ref lcreqloan);
                    if (lcreqloan.xml_contdetail != null && lcreqloan.xml_contdetail != "")
                    {
                        DwMain.ImportString(lcreqloan.xml_contdetail, Sybase.DataWindow.FileSaveAsType.Xml);
                    }
                    if (lcreqloan.xml_signlist1 != null && lcreqloan.xml_signlist1 != "")
                    {
                        DwSignlist1.ImportString(lcreqloan.xml_signlist1, Sybase.DataWindow.FileSaveAsType.Xml);
                    }
                    if (lcreqloan.xml_signlist2 != null && lcreqloan.xml_signlist2 != "")
                    {
                        DwSignlist2.ImportString(lcreqloan.xml_signlist2, Sybase.DataWindow.FileSaveAsType.Xml);
                    }
                    if (lcreqloan.xml_contcoll != null && lcreqloan.xml_contcoll != "")
                    {
                        DwColl.ImportString(lcreqloan.xml_contcoll, Sybase.DataWindow.FileSaveAsType.Xml);
                    }
                }
                catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(" lcreqloan.xml_lcreqloan พบปัญหา :" + ex); }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(" Method GetMemberDetail พบปัญหา :" + ex);
            }
        }
        #endregion

         #region Report
         private void RunProcess()
         {

             String print_id = "LOAN_BOOK001"; //r_ln_booking_money.srd
             app = state.SsApplication;
             gid = "LOAN_BOOK";
             rid = print_id;
             //string branch_id = state.SsCoopId;

             string loanrequest_docno = DwMain.GetItemString(1, "loanrequest_docno");
             ReportHelper lnv_helper = new ReportHelper();
             lnv_helper.AddArgument(loanrequest_docno, ArgumentType.String);
             //lnv_helper.AddArgument(HdSlip_no.Value, ArgumentType.String);
             //****************************************************************
             //ชื่อไฟล์ PDF = YYYYMMDDHHMMSS_<GID>_<RID>.PDF
             String pdfFileName = DateTime.Now.ToString("yyyyMMddHHmmss", WebUtil.EN);
             pdfFileName += "_" + gid + "_" + rid + ".pdf";
             pdfFileName = pdfFileName.Trim();
             //ส่งให้ ReportService สร้าง PDF ให้ {โดยปกติจะอยู่ใน C:\GCOOP\Saving\PDF\}.
             try
             {
                 ReportClient lws_report = wcf.Report;
                 String criteriaXML = lnv_helper.PopArgumentsXML();
                 this.pdf = lws_report.GetPDFURL(state.SsWsPass) + pdfFileName;
                 // String li_return = lws_report.RunWithID(state.SsWsPass, app, gid, rid, state.SsUsername, criteriaXML, pdfFileName);
                 String li_return = lws_report.RunWithID(state.SsWsPass, app, gid, rid, state.SsUsername, criteriaXML, pdfFileName);
                 if (li_return == "true")
                 {
                     HdOpenReport.Value = "True";
                 }
             }
             catch (Exception ex)
             {
                 LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                 return;
             }
         }

         private void RunProcess2()
         {
             String print_id="";
             string loantypeCode = HdloanType.Value;
             if(loantypeCode =="11"){
                 print_id = "INV_ETC001"; //	[LOAN_BOOK002] - หนังสือสัญญากู้เงินระยะสั้น  
             }
             else if (loantypeCode == "21")
             {
                 print_id = "INV_ETC001"; 	//[LOAN_BOOK003] - หนังสือสัญญากู้เงินระยะปานกลาง  
             }
             else if (loantypeCode == "31")
             {
                 print_id = "INV_ETC001"; //	[LOAN_BOOK004] - หนังสือสัญญากู้เงินระยะยาว
             }
             else { LtServerMessage.Text = WebUtil.ErrorMessage("ออกรายงานไม่สำเร็จ"); }
             app = state.SsApplication;
             gid = "INV_ETC";
             rid = print_id;
             //string branch_id = state.SsCoopId;

             string loanrequest_docno = Hfdoc_no.Value;
             string coodId = state.SsCoopId;
             ReportHelper lnv_helper = new ReportHelper();
             lnv_helper.AddArgument(loanrequest_docno, ArgumentType.String);
             lnv_helper.AddArgument(coodId, ArgumentType.String);
             //lnv_helper.AddArgument(HdSlip_no.Value, ArgumentType.String);
             //****************************************************************
             //ชื่อไฟล์ PDF = YYYYMMDDHHMMSS_<GID>_<RID>.PDF
             String pdfFileName = DateTime.Now.ToString("yyyyMMddHHmmss", WebUtil.EN);
             pdfFileName += "_" + gid + "_" + rid + ".pdf";
             pdfFileName = pdfFileName.Trim();
             //ส่งให้ ReportService สร้าง PDF ให้ {โดยปกติจะอยู่ใน C:\GCOOP\Saving\PDF\}.
             try
             {
                 ReportClient lws_report = wcf.Report;
                 String criteriaXML = lnv_helper.PopArgumentsXML();
                 this.pdf = lws_report.GetPDFURL(state.SsWsPass) + pdfFileName;
                 // String li_return = lws_report.RunWithID(state.SsWsPass, app, gid, rid, state.SsUsername, criteriaXML, pdfFileName);
                 String li_return = lws_report.RunWithID(state.SsWsPass, app, gid, rid, state.SsUsername, criteriaXML, pdfFileName);
                 if (li_return == "true")
                 {
                     HdOpenReport.Value = "True";
                 }
             }
             catch (Exception ex)
             {
                 LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                 return;
             }
         }

         private void RunProcess3()
         {

             String print_id = "LOAN_BOOK013"; //r_ln_booking_money.srd
             app = state.SsApplication;
             gid = "LOAN_BOOK";
             rid = print_id;
             //string branch_id = state.SsCoopId;

             string loanrequest_docno = DwMain.GetItemString(1, "loanrequest_docno");
             ReportHelper lnv_helper = new ReportHelper();
             lnv_helper.AddArgument(loanrequest_docno, ArgumentType.String);
             //lnv_helper.AddArgument(HdSlip_no.Value, ArgumentType.String);
             //****************************************************************
             //ชื่อไฟล์ PDF = YYYYMMDDHHMMSS_<GID>_<RID>.PDF
             String pdfFileName = DateTime.Now.ToString("yyyyMMddHHmmss", WebUtil.EN);
             pdfFileName += "_" + gid + "_" + rid + ".pdf";
             pdfFileName = pdfFileName.Trim();
             //ส่งให้ ReportService สร้าง PDF ให้ {โดยปกติจะอยู่ใน C:\GCOOP\Saving\PDF\}.
             try
             {
                 ReportClient lws_report = wcf.Report;
                 String criteriaXML = lnv_helper.PopArgumentsXML();
                 this.pdf = lws_report.GetPDFURL(state.SsWsPass) + pdfFileName;
                 // String li_return = lws_report.RunWithID(state.SsWsPass, app, gid, rid, state.SsUsername, criteriaXML, pdfFileName);
                 String li_return = lws_report.RunWithID(state.SsWsPass, app, gid, rid, state.SsUsername, criteriaXML, pdfFileName);
                 if (li_return == "true")
                 {
                     HdOpenReport.Value = "True";
                 }
             }
             catch (Exception ex)
             {
                 LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                 return;
             }
         }
         public void PopupReport()
         {
             //เด้ง Popup ออกรายงานเป็น PDF.
             String pop = "Gcoop.OpenPopup('" + pdf + "')";
             ClientScript.RegisterClientScriptBlock(this.GetType(), "DsReport", pop, true);
         }
        #endregion
    }
}