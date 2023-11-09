using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary.WcfNPm;
using DataLibrary;
using System.Threading;
using Sybase.DataWindow;

namespace Saving.Applications.pm
{
    public partial class w_sheet_pm_account_detail : PageWebSheet, WebSheet
    {
        protected String app;
        protected String gid;
        protected String rid;
        protected String pdf;
        protected String runProcess;
        protected String popupReport;
        protected String GetMainDetail;
        n_pmClient SvPm;// = wcf.Pm;
        String pbl = "pm_investment.pbl";
        private DwThDate tDwIntRate;
        public String outputProcess = "";

        public void InitJsPostBack()
        {
            GetMainDetail = WebUtil.JsPostBack(this, "GetMainDetail");
            HdOpenIFrame.Value = "False";
            runProcess = WebUtil.JsPostBack(this, "runProcess");
            popupReport = WebUtil.JsPostBack(this, "popupReport");
            tDwIntRate = new DwThDate(DwIntRate, this);
            tDwIntRate.Add("int_start_date", "int_start_tdate");
            tDwIntRate.Add("int_end_date", "int_end_tdate");
        }

        public void WebSheetLoadBegin()
        {
            SvPm = wcf.NPm;
            if (!IsPostBack)
            {
                HiddenFieldTab.Value = "1";
                HdIspostback.Value = "false";
                DwMain.InsertRow(0);
            }
            else
            {
                HdIspostback.Value = "true";
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwIntHis);
                this.RestoreContextDw(DwMove);
                this.RestoreContextDw(DwIntRate);
                this.RestoreContextDw(DwDueDate);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "GetMainDetail")
            {
                GetAccDetail();
            }
            else if (eventArg == "popupReport")
            {
                PopupReport();
            }
        }

        public void SaveWebSheet()
        {
            int StatusSave = 1;
            string account_no = "";
            string coop_id = state.SsCoopId;
            int r = DwIntRate.RowCount;
            int[] row = new int[r];
            string sqlinsert = "";
            string sqldate = "";
            decimal int_rate = 0;
            decimal int_yield_rate = 0;
            string int_desc = "";
            DateTime start_date = DateTime.Now;
            DateTime end_date = DateTime.Now;
            DateTime open_date = DateTime.Now;
            DateTime due_date = DateTime.Now;
            //Check 1----------------------------------------------------------------------------------------
            try
            {
                account_no = DwMain.GetItemString(1, "account_no");
                sqldate = "select open_date,due_date from pminvestmaster where account_no='"+account_no+"'";
                Sdt da = WebUtil.QuerySdt(sqldate);
                if (da.Next())
                {
                    open_date = da.GetDate("open_date");
                    due_date = da.GetDate("due_date");
                    start_date = DwIntRate.GetItemDateTime(1, "int_start_date");
                    end_date = DwIntRate.GetItemDateTime(r, "int_end_date");
                    if (start_date > open_date || end_date < due_date)
                    {
                        StatusSave = 0;
                        LtServerMessage.Text = WebUtil.ErrorMessage("วันที่ไม่ถูกต้อง กรุณาตรวจสอบ");
                    }
                }
            }
            catch { StatusSave = 0; }
            //Check 2----------------------------------------------------------------------------------------
            if (StatusSave == 1 && end_date < start_date)
            {
                StatusSave = 0;
                LtServerMessage.Text = WebUtil.ErrorMessage("วันที่คิดดอกเบี้ยถึง น้อยกว่า วันที่เริ่มคำนวณดอกเบี้ย");
            }
            //Check 3----------------------------------------------------------------------------------------
            if (StatusSave == 1)
            {
                DateTime s1 = DateTime.Now;
                DateTime s2 = DateTime.Now;
                DateTime e1 = DateTime.Now;
                DateTime e2 = DateTime.Now;
                for (int j = 1; j < r; j++)
                {
                    try
                    {
                        s1 = DwIntRate.GetItemDateTime(j, "int_start_date");
                        s2 = DwIntRate.GetItemDateTime(j + 1, "int_start_date");
                        e1 = DwIntRate.GetItemDateTime(j, "int_end_date");
                        e2 = DwIntRate.GetItemDateTime(j + 1, "int_end_date");
                        if (s1 > s2 || s1 > e2 || e1 < s1 || e1 != s2 || e1 > e2)
                        {
                            StatusSave = 0;
                            LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาตรวจสอบสอบวันที่");
                        }
                        else if (s1 > e1 || s2 > e2)
                        {
                            StatusSave = 0;
                            LtServerMessage.Text = WebUtil.ErrorMessage("วันที่คิดดอกเบี้ยถึง น้อยกว่า วันที่เริ่มคำนวณดอกเบี้ย");
                        }
                    }
                    catch
                    {
                        StatusSave = 0;
                        LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกวันที่ให้ครบ");
                    }
                }
            }
            //Save   ----------------------------------------------------------------------------------------
            if (StatusSave == 1)
            {
                try
                {
                    account_no = DwMain.GetItemString(1, "account_no");
                    string deleteall = "delete from pminvestintrate where account_no='" + account_no + "' and coop_id='" + coop_id + "'";
                    Sdt delete = WebUtil.QuerySdt(deleteall);
                    for (int i = 1; i <= r; i++)
                    {
                        start_date = DwIntRate.GetItemDateTime(i, "int_start_date");
                        end_date = DwIntRate.GetItemDateTime(i, "int_end_date");
                        try
                        {
                            int_rate = DwIntRate.GetItemDecimal(i, "int_rate");
                        }
                        catch { int_rate = 0; }
                        try
                        {
                            int_yield_rate = DwIntRate.GetItemDecimal(i, "int_yield_rate");
                        }
                        catch { int_yield_rate = 0; }
                        try
                        {
                            int_desc = DwIntRate.GetItemString(i, "int_desc");
                        }
                        catch { int_desc = ""; }
                        //insert
                        sqlinsert = "insert into pminvestintrate (coop_id,account_no,seq_no,int_start_date,int_end_date,int_rate,int_yield_rate,int_desc) "
                            + "values ('" + coop_id + "','" + account_no + "'," + i + ",to_date('" + start_date.ToString("dd/MM/yyyy") + "','dd/mm/yyyy')," +
                            "to_date('" + end_date.ToString("dd/MM/yyyy") + "','dd/mm/yyyy')," + int_rate + "," + int_yield_rate + ",'" + int_desc + "')";
                        Sdt dt = WebUtil.QuerySdt(sqlinsert);
                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                    }//end for
                }
                catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
            }
            //-----------------------------------------------------------------------------------------------
        }

        public void WebSheetLoadEnd()
        {

            try
            {
                DwUtil.RetrieveDDDW(DwWarranty, "invsource_code", pbl, null);
            }
            catch { }
            try
            {
                DwUtil.RetrieveDDDW(DwMain, "invsource_code", pbl, null);
                DwUtil.RetrieveDDDW(DwMain, "investtype_code", pbl, null);
                DwUtil.RetrieveDDDW(DwMain, "accid_prnc", pbl, null);
            }
            catch { }
            tDwIntRate.Eng2ThaiAllRow();
            DwMain.SaveDataCache();
            DwMove.SaveDataCache();
            DwIntHis.SaveDataCache();
            DwIntRate.SaveDataCache();
            DwDueDate.SaveDataCache();
        }
        public void GetAccDetail()
        {
            String account_no = "";
            String coop_id = state.SsCoopId;
            try
            {
                account_no = DwMain.GetItemString(1, "account_no");
                DwUtil.RetrieveDataWindow(DwMain, pbl, null, account_no);
            }
            catch(Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
            //------------------------ดึงข้อมูล-----------------------------------
            try
            {
                DwMove.Reset();
                DwUtil.RetrieveDataWindow(DwMove, pbl, null, account_no);
                string ck = DwMove.GetItemString(1, "account_no");
            }
            catch
            {
                DwMove.Reset();
            }
            try
            {
                DwIntRate.Reset();
                DwUtil.RetrieveDataWindow(DwIntRate, pbl, null, account_no);
                string ck = DwIntRate.GetItemString(1, "account_no");
            }
            catch
            {
                DwIntRate.Reset();
            }
            try
            {
                DwDueDate.Reset();
                DwUtil.RetrieveDataWindow(DwDueDate, pbl, null, account_no);
                string ck = DwDueDate.GetItemString(1, "account_no");
            }
            catch
            {
                DwDueDate.Reset();
            }
            try
            {
                DwIntHis.Reset();
                DwUtil.RetrieveDataWindow(DwIntHis, pbl, null, account_no);
                string ck = DwIntHis.GetItemString(1, "account_no");
            }
            catch
            {
                DwIntHis.Reset();
            }
            try
            {
                DwWarranty.Reset();
                DwUtil.RetrieveDataWindow(DwWarranty, pbl, null, account_no);
                string ck = DwWarranty.GetItemString(1, "account_no");
            }
            catch
            {
                DwWarranty.Reset();
            }
            try
            {
                DwDuration.Reset();
                DwUtil.RetrieveDataWindow(DwDuration, pbl, null, coop_id,account_no);
                string ck = DwDuration.GetItemString(1, "account_no");
            }
            catch
            {
                DwDuration.Reset();
            }
        }

        #region Report Process
        public void PopupReport()
        {
            RunProcess();
            // Thread.Sleep(5000);
            Thread.Sleep(700);
            //เด้ง Popup ออกรายงานเป็น PDF.
            //String pop = "Gcoop.OpenPopup('http://localhost/GCOOP/WebService/Report/PDF/20110223093839_shrlonchk_SHRLONCHK001.pdf')";

            String pop = "Gcoop.OpenPopup('" + Session["pdf"].ToString() + "')";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "DsReport", pop, true);

        }
        private void RunProcess()
        {
            app = state.SsApplication;
            try
            {
                //gid = Request["gid"].ToString();
                gid = "investment_mth";
            }
            catch { }
            try
            {
                //rid = Request["rid"].ToString();
                rid = "investment_mth002";
            }
            catch { }
            
            String account_no = DwMain.GetItemString(1, "account_no");
            if (account_no == null || account_no == "")
            {
                return;
            }

            //แปลง Criteria ให้อยู่ในรูปแบบมาตรฐาน.
            ReportHelper lnv_helper = new ReportHelper();
            lnv_helper.AddArgument(account_no, ArgumentType.String);
            lnv_helper.AddArgument(account_no, ArgumentType.String);
            lnv_helper.AddArgument(state.SsCoopId, ArgumentType.String);

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
                //String li_return = lws_report.RunWithID(state.SsWsPass, app, gid, rid, state.SsUsername, criteriaXML, pdfFileName);
                //if (li_return == "true")
                //{
                //    HdOpenIFrame.Value = "True";
                //    HdcheckPdf.Value = "True";

                //}
                //else if (li_return != "true")
                //{
                //    HdcheckPdf.Value = "False";
                //}

                String criteriaXML = lnv_helper.PopArgumentsXML();
                //this.pdf = lws_report.GetPDFURL(state.SsWsPass) + pdfFileName;
                string printer = "PDF";
                outputProcess = WebUtil.runProcessingReport(state, app, gid, rid, criteriaXML, pdfFileName, printer);

            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                return;
            }



            Session["pdf"] = pdf;
            // PopupReport();
        }
        #endregion
    }
}