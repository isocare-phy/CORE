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
using CoreSavingLibrary.WcfNCommon;
using Sybase.DataWindow.Web;
using Sybase.DataWindow;
//using CoreSavingLibrary.WcfPrint;
using CoreSavingLibrary.WcfNShrlon;
using DataLibrary;


namespace Saving.Applications.shrlon
{
    public partial class w_sheet_sl_payment_table : PageWebSheet, WebSheet
    {

        protected String postRetrieve;
        protected String downloadPDF;
        protected String postInstallmentChanged;
        protected String postPeriodPaymentChanged;
        protected String postShow;
        protected String postNewClicked;
        protected DwThDate dwThDate;


        //*********************report****************************
        protected String app;
        protected String gid;
        protected String rid;
        protected String pdf;
        protected String runProcess;
        protected String popupReport;

        #region WebSheet Members

        public void InitJsPostBack()
        {   //report
            HdOpenIFrame.Value = "False";
            runProcess = WebUtil.JsPostBack(this, "runProcess");
            popupReport = WebUtil.JsPostBack(this, "popupReport");


            postRetrieve = WebUtil.JsPostBack(this, "postRetrieve");
            downloadPDF = WebUtil.JsPostBack(this, "downloadPDF");
            postInstallmentChanged = WebUtil.JsPostBack(this, "postInstallmentChanged");
            postPeriodPaymentChanged = WebUtil.JsPostBack(this, "postPeriodPaymentChanged");
            postShow = WebUtil.JsPostBack(this, "postShow");
            postNewClicked = WebUtil.JsPostBack(this, "postNewClicked");
            //ThaiDateFields
            dwThDate = new DwThDate(dw_criteria, this);
            dwThDate.Add("startcont_date", "startcont_tdate");
        }

        public void WebSheetLoadBegin()
        {
            if (IsPostBack)
            {
                RestoreContextDw(dw_criteria);
                RestoreContextDw(dw_result);
            }
            else
            {
                //default values.
                Sta ta = new Sta(state.SsConnectionString);
                try
                {
                    ta.Exe("delete from paytable");
                    ta.Exe("delete from paytabledet");
                }
                catch
                { }
                ta.Close();
                newClicked();
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postRetrieve")
            {
                retrieve();
            }
            else if (eventArg == "downloadPDF")
            {
                createNPopupPDF();
            }
            else if (eventArg == "postInstallmentChanged")
            {
                installmentChanged();
            }
            else if (eventArg == "postPeriodPaymentChanged")
            {
                periodPaymentChanged();
            }
            else if (eventArg == "postShow")
            {
                showPaymentTable();
            }
            else if (eventArg == "postNewClicked")
            {
                newClicked();
            }
        }

        public void SaveWebSheet()
        {

        }

        public void WebSheetLoadEnd()
        {

            //end of functions
            dw_criteria.SaveDataCache();
            dw_result.SaveDataCache();
        }

        #endregion

        protected void retrieve()
        {
            //ดึงข้อมูลสัญญามาป้อนให้. ต้องป้อนเข้ามา 3 fields
            try
            {
                n_shrlonClient shrlonService = wcf.NShrlon;
                str_paytab lstr_paytab = new str_paytab();
                lstr_paytab.loancontract_no = dw_criteria.GetItemString(1, "loancontract_no");
                lstr_paytab.paytab_type = Convert.ToInt16(dw_criteria.GetItemDouble(1, "paytab_type"));
                lstr_paytab.prnbalfrom_type = Convert.ToInt16(dw_criteria.GetItemDouble(1, "prnbalfrom_type"));
                lstr_paytab.contcoop_id = state.SsCoopId;
                lstr_paytab.coop_id = state.SsCoopId;
                Int16 xmlCriteria = shrlonService.of_initlncontpaycriteria(state.SsWsPass, ref lstr_paytab);//InitLnContPayCriteria
                dw_criteria.Reset();
                DwUtil.ImportData(xmlCriteria.ToString(), dw_criteria, null, Sybase.DataWindow.FileSaveAsType.Xml);
                dwThDate.Eng2ThaiAllRow();
                //dw_criteria.ImportString(xmlCriteria,Sybase.DataWindow.FileSaveAsType.Xml);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.WarningMessage(ex);
            }
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


            PopupReport();


        }

        protected void installmentChanged()
        {
            //เมื่อเปลี่ยนจำนวนงวด. ให้คำนวณยอดชำระต่องวดใหม่. ยิงใส่หน้าจอเดิม.
            try
            {
                n_shrlonClient shrlonService = wcf.NShrlon;
                str_genperiodpaytab lstr_data = new str_genperiodpaytab();
                lstr_data.calfrom_type = "PERIOD";
                lstr_data.principal_balance = dw_criteria.GetItemDecimal(1, "principal_amt");
                lstr_data.payment_type = Convert.ToInt16(dw_criteria.GetItemDouble(1, "payment_type")); //(1=คงต้น,2=คงยอด)
                lstr_data.period = Convert.ToInt16(dw_criteria.GetItemDouble(1, "installment"));
                lstr_data.period_payment = dw_criteria.GetItemDecimal(1, "period_amt");
                lstr_data.lastperiod_payment = 0;
                lstr_data.interest_rate = dw_criteria.GetItemDecimal(1, "intrate_amt");
                lstr_data.xml_message = "อะไรก็ไม่รู้อันเนี้ย";
                shrlonService.of_genperiodpaytab(state.SsWsPass, ref lstr_data);
                dw_criteria.SetItemDouble(1, "installment", Convert.ToDouble(lstr_data.period));
                dw_criteria.SetItemDouble(1, "period_amt", Convert.ToDouble(lstr_data.period_payment));
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        protected void periodPaymentChanged()
        {
            //เมื่อเปลี่ยนยอดชำระต่องวด. ให้คำนวณจำนวนงวดใหม่. ยิงใส่หน้าจอเดิม.
            try
            {
                n_shrlonClient shrlonService = wcf.NShrlon;
                str_genperiodpaytab lstr_data = new str_genperiodpaytab();
                lstr_data.calfrom_type = "INSTALL";
                lstr_data.principal_balance = dw_criteria.GetItemDecimal(1, "principal_amt");
                lstr_data.payment_type = Convert.ToInt16(dw_criteria.GetItemDouble(1, "payment_type")); //(1=คงต้น,2=คงยอด)
                lstr_data.period = Convert.ToInt16(dw_criteria.GetItemDouble(1, "installment"));
                lstr_data.period_payment = dw_criteria.GetItemDecimal(1, "period_amt");
                lstr_data.lastperiod_payment = 0;
                lstr_data.interest_rate = dw_criteria.GetItemDecimal(1, "intrate_amt");
                lstr_data.xml_message = "อะไรก็ไม่รู้อันเนี้ย";
                shrlonService.of_genperiodpaytab(state.SsWsPass, ref lstr_data);
                dw_criteria.SetItemDouble(1, "installment", Convert.ToDouble(lstr_data.period));
                dw_criteria.SetItemDouble(1, "period_amt", Convert.ToDouble(lstr_data.period_payment));
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        protected void showPaymentTable()
        {
            //คำนวณตารางชำระ
            try
            {
               n_shrlonClient shrlonService = wcf.NShrlon;
                str_paytab lstr_paytab = new str_paytab();
                lstr_paytab.xml_paytab = "";
                lstr_paytab.xml_criteria = dw_criteria.Describe("Datawindow.data.XML");
                lstr_paytab.contcoop_id = state.SsCoopId;
                lstr_paytab.coop_id = state.SsCoopId;
                Int16 xmlResult = shrlonService.of_initlncontpaytable(state.SsWsPass, ref lstr_paytab);
               
                dw_result.Reset();
                DwUtil.ImportData(xmlResult.ToString(), dw_result, null, Sybase.DataWindow.FileSaveAsType.Xml);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.WarningMessage(ex);
            }

            inserttopaytable();
            RunProcess();
        }

        protected void newClicked()
        {
            //เมื่อคลิกที่ปุ่ม New ให้เคลียร์ข้อมูลเหมือนเริ่มต้นใหม่.
            String firstPeriod = DateTime.Today.ToString("MMyyyy", WebUtil.TH);    //string aa = dt.ToString("dd/MM/yyyy", new CultureInfo("th-TH"));
            dw_result.Reset();
            dw_criteria.Reset();
            dw_criteria.InsertRow(0);
            dw_criteria.SetItemDateTime(1, "startcont_date", DateTime.Today);
            dw_criteria.SetItemString(1, "firstpay_period", firstPeriod);
            dwThDate.Eng2ThaiAllRow();
        }

        private void inserttopaytable()
        {

            String loancontract_no = dw_criteria.GetItemString(1, "loancontract_no");
            String startcont_tdate = dw_criteria.GetItemString(1, "startcont_tdate");
            string date_start_cont = "to_date('', 'dd/mm/yyyy')";
            try
            {
                DateTime startcont_date;
                startcont_date = DateTime.ParseExact(startcont_tdate, "ddMMyyyy", WebUtil.TH);
                date_start_cont = "to_date('" + startcont_date.ToString("dd/MM/yyyy", WebUtil.EN) + "', 'dd/mm/yyyy')";
            }
            catch
            {
                date_start_cont = "null";
            }


            String firstpay_period = dw_criteria.GetItemString(1, "firstpay_period");

            string firstpay_date = "to_date('', 'dd/mm/yyyy')";
            try
            {
                DateTime firstpay;
                firstpay = DateTime.ParseExact(firstpay_period, "ddMMyyyy", WebUtil.TH);
                firstpay_date = "to_date('" + firstpay.ToString("dd/MM/yyyy", WebUtil.EN) + "', 'dd/mm/yyyy')";
            }
            catch
            {


                firstpay_date = "null";

            }

            Decimal intrate_amt = dw_criteria.GetItemDecimal(1, "intrate_amt") * 100;
            Decimal principal_amt = dw_criteria.GetItemDecimal(1, "principal_amt");
            Decimal period_amt = dw_criteria.GetItemDecimal(1, "period_amt");
            Decimal installment = dw_criteria.GetItemDecimal(1, "installment");





            Sta ta = new Sta(state.SsConnectionString);
            try
            { ta.Exe("insert into paytable ( loancontract_no,loanreqfixscv_date,firstpay_date,interest_amt,loanrequest_amt,loanperiod_amt,loanall_period) values ('" + loancontract_no + "'," + date_start_cont + "," + firstpay_date + "," + intrate_amt + "," + principal_amt + "," + period_amt + "," + installment + ")"); }
            catch
            { }
            ta.Close();

            //ดึงค่าจาก dw_result
            for (int i = 1; i <= dw_result.RowCount; i++)
            {
                Decimal seq_no = dw_result.GetItemDecimal(i, "seq_no");

                // String payment_tdate = dw_result.GetItemString(1, "payment_date");
                string payment_tdatee = "to_date('', 'dd/mm/yyyy')";
                try
                {
                    DateTime payment_date = dw_result.GetItemDateTime(i, "payment_date"); ;
                    //payment_date = DateTime.ParseExact(payment_tdate, "ddMMyyyy", WebUtil.TH);
                    payment_tdatee = "to_date('" + payment_date.ToString("dd/MM/yyyy", WebUtil.EN) + "', 'dd/mm/yyyy')";
                }
                catch
                {
                    payment_tdatee = "null";
                }



                Decimal prinpay_amt = dw_result.GetItemDecimal(i, "prinpay_amt");

                Decimal intpay_amt = dw_result.GetItemDecimal(i, "intpay_amt");

                Decimal balance_amt = dw_result.GetItemDecimal(i, "balance_amt");
                String sol = "55555";

                Sta ta2 = new Sta(state.SsConnectionString);
                try
                { ta2.Exe("insert into paytabledet ( loan_id,loancontract_no,loanperiod,loanfixscv_date,itempay_amt,interest_amt,principal_amt,sol) values (" + i + ",'" + loancontract_no + "'," + seq_no + "," + payment_tdatee + "," + prinpay_amt + "," + intpay_amt + "," + balance_amt + ",'" + sol + "')"); }
                catch
                { }
                ta2.Close();

            }

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
                gid = "LN_NORM";
            }
            catch { }
            try
            {
                //rid = Request["rid"].ToString();
                rid = "LN_NORM0003";
            }
            catch { }

            String doc_no = "";
           




            //แปลง Criteria ให้อยู่ในรูปแบบมาตรฐาน.
            ReportHelper lnv_helper = new ReportHelper();
            lnv_helper.AddArgument(doc_no, ArgumentType.String);


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
