using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary.WcfNCommon;
using CoreSavingLibrary.WcfNShrlon;
using Sybase.DataWindow;
using System.Web.Services.Protocols;
using CoreSavingLibrary.WcfNMis;
using DataLibrary;

namespace Saving.Applications.mis
{
    public partial class w_sheet_benchmark_mis : PageWebSheet, WebSheet
    {
        private DwTrans SQLCA;
        private n_misClient MisService;
        private n_commonClient commonService;

        //*********************report****************************
        protected String app;
        protected String gid;
        protected String rid;
        protected String pdf;
        protected String runProcess;
        protected String popupReport;
        protected String downloadPDF;

        protected String jsClickSubmit;

        public void InitJsPostBack()
        {
            jsClickSubmit = WebUtil.JsPostBack(this, "jsClickSubmit");

            HdOpenIFrame.Value = "False";
            runProcess = WebUtil.JsPostBack(this, "runProcess");
            popupReport = WebUtil.JsPostBack(this, "popupReport");
            downloadPDF = WebUtil.JsPostBack(this, "downloadPDF");
        }

        public void WebSheetLoadBegin()
        {
            ////ลบทุกครั้งก่อน Insert
            //Sta ta = new Sta(state.SsConnectionString);
            //try
            //{ ta.Exe("delete from misratioreport"); }
            //catch
            //{ }
            //ta.Close();

            try
            {
                MisService = wcf.NMis;
                commonService = wcf.NCommon;
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ติดต่อ Web Service ไม่ได้");
                return;
            }
            if (IsPostBack)
            {
                this.RestoreContextDw(dw_main);
                this.RestoreContextDw(dw_cri);
                //DeleteMisratioreport();
            }
            else
            {
                dw_main.InsertRow(0);
                dw_cri.InsertRow(0);

                string toDayNow = DateTime.Now.ToString("M/yyyy");
                string[] DateNow = toDayNow.Split('/');

                try
                {
                    Decimal endMonth = Convert.ToDecimal(DateNow[0]);
                    Decimal endYear = Convert.ToDecimal(DateNow[1]) + 543;
                    Decimal startMonth = endMonth;
                    Decimal startYear = endYear;//Decimal startYear = endYear - 1;
                    endYear -= 1;

                    dw_cri.SetItemDecimal(1, "start_month", startMonth);
                    dw_cri.SetItemDecimal(1, "end_month", endMonth);
                    dw_cri.SetItemDecimal(1, "start_year", startYear);
                    dw_cri.SetItemDecimal(1, "end_year", endYear);
                    //DeleteMisratioreport();
                }
                catch { }
                //DeleteMisratioreport();
            }
            DeleteMisratioreport();
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "jsClickSubmit")
            {
                ClickSubmit();
            }
            else if (eventArg == "popupReport")
            {
                //RunProcess();
                PopupReport();
            }
            else if (eventArg == "downloadPDF")
            {
                createNPopupPDF();
            }
            else if (eventArg == "runProcess")
            {
                RunProcess();
            }
        }

        public void SaveWebSheet()
        {

        }

        public void WebSheetLoadEnd()
        {
            dw_cri.SaveDataCache();
            dw_main.SaveDataCache();
        }

        protected void createNPopupPDF()
        {

            ////เอา XML จากหน้าจอที่แสดงอยู่ มาใส่ใน Report เพื่อเปลี่ยนรูปแบบหน้าตา ก่อนสั่งพิมพ์.
            //String xml = dw_result.Describe("Datawindow.data.XML");

            ////WebDataWindowControl dw_report = new WebDataWindowControl();
            ////DwUtil.ImportData(xml, dw_report, null, FileSaveAsType.Xml);

            ////FileName
            //PrintClient svPrint = wcf.Print;
            //String pdfFile = DateTime.Now.ToString("yyyyMMddHHmmss", WebUtil.EN);
            //pdfFile += "_payment_table.pdf";
            //pdfFile = pdfFile.Trim();

            ////Print
            //Int32 li_rv = svPrint.PrintPDF(state.SsWsPass, xml, pdfFile);
            //if (li_rv < 0)
            //{
            //    LtServerMessage.Text = WebUtil.ErrorMessage("PrintPDF failed : (" + Convert.ToString(li_rv) + ") returned.");
            //    return;
            //}

            ////Popup
            //String pdfURL = svPrint.GetPDFURL(state.SsWsPass) + pdfFile;
            //String pop = "Gcoop.OpenPopup('" + pdfURL + "')";
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "PaymentTable", pop, true);

            RunProcess();
            PopupReport();


        }

        public void ClickSubmit()
        {

            Int16 startMonth = 0;
            String startYear = "";
            Int16 endMouth = 0;
            String endYear = "";

            try
            {
                try
                { startMonth = Convert.ToInt16(dw_cri.GetItemDecimal(1, "start_month")); }
                catch { startMonth = 0; }
                try { startYear = Convert.ToString(dw_cri.GetItemString(1, "start_year")); }
                catch { startYear = ""; }
                try { endMouth = Convert.ToInt16(dw_cri.GetItemDecimal(1, "end_month")); }
                catch { endMouth = 0; }
                try { endYear = Convert.ToString(dw_cri.GetItemString(1, "end_year")); }
                catch { endYear = ""; }

                if ((startMonth == 0) || (startYear == "") || (endMouth == 0) || (endYear == ""))
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาเลือกข้อมูลให้ครบ");
                }
                else
                {
                    //TODO: เปลี่ยนฟังก์ชัน  PBService
                    //WsMis.Mis Ms = wcf.NMis;
                    String xmlMain = MisService.RatioGetRatiosValues(state.SsWsPass, startMonth, startYear, endMouth, endYear);
                    dw_main.Reset();
                    dw_main.ImportString(xmlMain, FileSaveAsType.Xml);
                    InsertMisratioreport();
                    RunProcess();
                }
                //RunProcess();
                //InsertMisratioreport();
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
        }

        private void InsertMisratioreport()
        {

            for (int i = 1; i <= dw_main.RowCount; i++)
            {
                String ratio_name = dw_main.GetItemString(i, "ratio_name");
                Decimal ratio_value = dw_main.GetItemDecimal(i, "ratio_value");
                String ratio_function = dw_main.GetItemString(i, "ratio_function");

                Sta ta = new Sta(state.SsConnectionString);
                try
                {
                    ta.Exe("insert into misratioreport(mis_id, ratio_name, ratio_value, ratio_function) values(" + i + ", '" + ratio_name + "', " + ratio_value + " , '" + ratio_function + "')");
                    ta.Commit();
                }
                catch
                { }
                ta.Close();

            }
        }

        private void DeleteMisratioreport()
        {
            //ลบทุกครั้งก่อน Insert
            Sta ta = new Sta(state.SsConnectionString);
            try
            {
                ta.Exe("delete from misratioreport");
                ta.Commit();
            }
            catch
            { }
            ta.Close();
        }

        private void RunProcess()
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
                gid = "Mis_spec";
            }
            catch { }
            try
            {
                //rid = Request["rid"].ToString();
                rid = "MISSPEC005";
            }
            catch { }

            //String doc_no = dw_main.GetItemString(1, "loanrequest_docno");
            //if (doc_no == null || doc_no == "")
            //{
            //    return;
            //}

            //String ls_xmlmain = dw_main.Describe("DataWindow.Data.XML");
            //String ls_format = "CAT";
            //short li_membtime = 0; Decimal ldc_right25 = 0; Decimal ldc_right33 = 0; Decimal ldc_right35 = 0;
            //int re = shrlonService.Ofprintcallpermiss(state.SsWsPass, ls_xmlmain, ls_format, ref li_membtime, ref ldc_right25, ref ldc_right33, ref ldc_right35);
            //Decimal li_membtime_ = li_membtime;
            //Decimal ldc_right25_ = ldc_right25;
            //Decimal ldc_right33_ = ldc_right33;
            //Decimal ldc_right35_ = ldc_right35;

            Int16 startMonth = 0;
            String startYear = "";
            Int16 endMouth = 0;
            String endYear = "";

            try
            { startMonth = Convert.ToInt16(dw_cri.GetItemDecimal(1, "start_month")); }
            catch { startMonth = 0; }
            try { startYear = Convert.ToString(dw_cri.GetItemString(1, "start_year")); }
            catch { startYear = ""; }
            try { endMouth = Convert.ToInt16(dw_cri.GetItemDecimal(1, "end_month")); }
            catch { endMouth = 0; }
            try { endYear = Convert.ToString(dw_cri.GetItemString(1, "end_year")); }
            catch { endYear = ""; }


            //แปลง Criteria ให้อยู่ในรูปแบบมาตรฐาน.
            ReportHelper lnv_helper = new ReportHelper();
            //lnv_helper.AddArgument("55555", ArgumentType.String);
            lnv_helper.AddArgument(startMonth.ToString(), ArgumentType.Number);
            lnv_helper.AddArgument(startYear.ToString(), ArgumentType.Number);
            lnv_helper.AddArgument(endMouth.ToString(), ArgumentType.Number);
            lnv_helper.AddArgument(endYear.ToString(), ArgumentType.Number);

            //ชื่อไฟล์ PDF = YYYYMMDDHHMMSS_<GID>_<RID>.PDF

            String pdfFileName = DateTime.Now.ToString("yyyyMMddHHmmss", WebUtil.EN);
            pdfFileName += "_" + gid + "_" + rid + ".pdf";
            pdfFileName = pdfFileName.Trim();

            //ส่งให้ ReportService สร้าง PDF ให้ {โดยปกติจะอยู่ใน C:\GCOOP\Saving\PDF\}.
            try
            {
                //CoreSavingLibrary.WcfReport.ReportClient lws_report = wcf.Report;
                //String criteriaXML = lnv_helper.PopArgumentsXML();
                //this.pdf = lws_report.GetPDFURL(state.SsWsPass) + pdfFileName;
                //String li_return = lws_report.Run(state.SsWsPass, app, gid, rid, criteriaXML, pdfFileName);
                //if (li_return == "true")
                //{
                //    HdOpenIFrame.Value = "True";
                //    HdcheckPdf.Value = "True";

                //}
                //else if (li_return != "true")
                //{
                //    HdcheckPdf.Value = "False";
                //}
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                return;
            }
            Session["pdf"] = pdf;
            //PopupReport();


        }

        public void PopupReport()
        {
            //เด้ง Popup ออกรายงานเป็น PDF.
            //String pop = "Gcoop.OpenPopup('http://localhost/GCOOP/WebService/Report/PDF/20110223093839_shrlonchk_SHRLONCHK001.pdf')";

            String pop = "Gcoop.OpenPopup('" + Session["pdf"].ToString() + "')";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "DsReport", pop, true);

        }
    }
}