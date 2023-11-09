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
    public partial class w_sheet_ln_proc_noticepaytabdet : PageWebSheet, WebSheet
    {
        protected String jsPostLncontNo;
        protected String jsPostGen;
        protected String JsReport;
        private InvestmentClient InvestService;
        //Report
        public String app = "";
        public String gid = "";
        public String rid = "";
        protected String pdf = "";

        public void InitJsPostBack()
        {
            jsPostLncontNo = WebUtil.JsPostBack(this, "jsPostLncontNo");
            jsPostGen = WebUtil.JsPostBack(this, "jsPostGen");
            JsReport = WebUtil.JsPostBack(this, "JsReport");
        }

        public void WebSheetLoadBegin()
        {
            InvestService = wcf.Investment;
            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwList);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
             if (eventArg == "jsPostLncontNo")
            {
                Init();
            }
             else if (eventArg == "jsPostGen")
            {
                Generate();
            }
             else if (eventArg == "JsReport") 
             {
                 RunProcessDetail();
             }
             else if (eventArg == "popupReport")
             {
                 PopupReport();
             }

        }

        public void SaveWebSheet()
        {
            try
            {
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกทำรายการสำเร็จ");
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
            DwList.SaveDataCache();
        }

        #region Function
        private void Init()
        {
            string loancontract_no = DwMain.GetItemString(1, "loancontract_no");
            str_lcpaytab astr_lcpaytab = new str_lcpaytab();
            astr_lcpaytab.concoop_id = state.SsCoopId;
            astr_lcpaytab.loancontract_no = loancontract_no;

            try
            {
                short result = InvestService.of_initproc_paytab(state.SsWsPass, ref astr_lcpaytab);
                if (result == 1)
                {
                    DwMain.Reset();
                    DwList.Reset();
                    if (astr_lcpaytab.xml_contdetail != "" && astr_lcpaytab.xml_contdetail != null)
                    {
                        DwMain.ImportString(astr_lcpaytab.xml_contdetail, FileSaveAsType.Xml);
                    }
                    Panel6.Visible = false;
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
            HdContno.Value = loancontract_no;
        }

        private void Generate()
        {
            try
            {
                str_lcpaytab astr_lcpaytab = new str_lcpaytab();
                astr_lcpaytab.xml_contdetail = DwMain.Describe("Datawindow.Data.XML");
                astr_lcpaytab.paytab_date = state.SsWorkDate;

                short result = InvestService.of_saveproc_paytab(state.SsWsPass, ref astr_lcpaytab);
                if (result == 1)
                {
                    DwList.Reset();
                    if (astr_lcpaytab.xml_paytab != "" && astr_lcpaytab.xml_paytab != null)
                    {
                        DwList.ImportString(astr_lcpaytab.xml_paytab, FileSaveAsType.Xml);
                    }
                    Panel6.Visible = true;
                }
            }
            catch(Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }
        #endregion

        #region printreport
        private void RunProcessDetail()
        {
            String print_id = "INV_ETC001"; //"d_reqdept_fixed"
            app = state.SsApplication;
            gid = "INV_ETC";
            rid = print_id;
             
            //Decimal chgcontplace_type = DwMain.GetItemDecimal(1, "chgcontplace_type");
            ReportHelper lnv_helper = new ReportHelper();
            lnv_helper.AddArgument(HdContno.Value, ArgumentType.String);
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
                    HdOpenIFrame.Value = "True";
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