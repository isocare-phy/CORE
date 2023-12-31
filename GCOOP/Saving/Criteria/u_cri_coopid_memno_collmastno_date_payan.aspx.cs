﻿using System;
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
using DataLibrary;

namespace Saving.Criteria
{
    public partial class u_cri_coopid_memno_collmastno_date_payan : PageWebSheet, WebSheet
    {
        protected String app;
        protected String gid;
        protected String rid;
        protected String pdf;
        protected String jsPostMember;
        protected String runProcess;
        protected String popupReport;
        private DwThDate tdw_criteria;
        public String outputProcess = "";
        #region WebSheet Members

        public void InitJsPostBack()
        {
            HdOpenIFrame.Value = "False";
            runProcess = WebUtil.JsPostBack(this, "runProcess");
            jsPostMember = WebUtil.JsPostBack(this, "jsPostMember");
            tdw_criteria = new DwThDate(dw_criteria, this);
            tdw_criteria.Add("mob_date", "mob_tdate"); 
        }

        //protected void Page_Load(object sender, EventArgs e)
        public void WebSheetLoadBegin()
        {
            InitJsPostBack();

             //DwUtil.RetrieveDDDW(dw_criteria, "collmast_no", "criteria.pbl", state.SsCoopControl);

            if (IsPostBack)
            {
                dw_criteria.RestoreContext();
            }
            else
            {
                //default values.
                dw_criteria.InsertRow(0);
                string userid = state.SsUsername;
                string full_name = "";
                string sqlll = "select full_name from amsecusers where user_name  = '" + userid + "'";
                Sdt dt = WebUtil.QuerySdt(sqlll);
                if (dt.Next()) {
                     full_name = dt.GetString("full_name");

                }
                dw_criteria.SetItemString(1, "payan1", full_name);

                
            }

            //--- Page Arguments
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
                gid = Request["gid"].ToString();
            }
            catch { }
            try
            {
                rid = Request["rid"].ToString();
            }
            catch { }

            //Report Name.
            try
            {
                Sta ta = new Sta(state.SsConnectionString);
                String sql = "";
                sql = @"SELECT REPORT_NAME  
                    FROM WEBREPORTDETAIL  
                    WHERE ( GROUP_ID = '" + gid + @"' ) AND ( REPORT_ID = '" + rid + @"' )";
                Sdt dt = ta.Query(sql);
                ReportName.Text = dt.Rows[0]["REPORT_NAME"].ToString();
                ta.Close();
            }
            catch
            {
                ReportName.Text = "[" + rid + "]";
            }

            //Link back to the report menu.
            LinkBack.PostBackUrl = String.Format("~/ReportDefault.aspx?app={0}&gid={1}", app, gid);
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "runProcess")
            {
                RunProcess();
            }
            else if (eventArg == "popupReport")
            {
                PopupReport();
            }
            else if (eventArg == "post")
            {

            }
            else if (eventArg == "jsPostMember")
            {
                string memno = dw_criteria.GetItemString(1, "member_no");
                DwUtil.RetrieveDDDW(dw_criteria, "collmast_no", "criteria.pbl", memno);
            }
        }

        public void SaveWebSheet()
        {
        }
        public void Postmemno() { 
        
        
        }
        public void WebSheetLoadEnd()
        {
        }

        #endregion
        #region Report Process

        private void RunProcess()
        {
            //อ่านค่าจากหน้าจอใส่ตัวแปรรอไว้ก่อน.
            String payan1 = "", payan2 = " ";
            String collmast_no = dw_criteria.GetItemString(1, "collmast_no");
            String select_date = WebUtil.ConvertDateThaiToEng(dw_criteria, "mob_tdate", null);
            try
            {
                payan1 = dw_criteria.GetItemString(1, "payan1");
            }
            catch {
                payan1 = " ";
            }
            try
            {
                payan2 = dw_criteria.GetItemString(1, "payan2");
            }
            catch {
                payan2 = " ";
            }
            //แปลง Criteria ให้อยู่ในรูปแบบมาตรฐาน.
            ReportHelper lnv_helper = new ReportHelper();
            lnv_helper.AddArgument(collmast_no, ArgumentType.String);
            lnv_helper.AddArgument(select_date, ArgumentType.DateTime);
            lnv_helper.AddArgument(payan1, ArgumentType.String);
            lnv_helper.AddArgument(payan2, ArgumentType.String);

            //ชื่อไฟล์ PDF = YYYYMMDDHHMMSS_<GID>_<RID>.PDF
            String pdfFileName = DateTime.Now.ToString("yyyyMMddHHmmss", WebUtil.EN);
            pdfFileName += "_" + gid + "_" + rid + ".pdf";
            pdfFileName = pdfFileName.Trim();
            //ส่งให้ ReportService สร้าง PDF ให้ {โดยปกติจะอยู่ใน C:\GCOOP\Saving\PDF\}.
            try
            {
                //CoreSavingLibrary.WcfReport.ReportClient lws_report = wcf.Report;
                String criteriaXML = lnv_helper.PopArgumentsXML();
                //this.pdf = lws_report.GetPDFURL(state.SsWsPass) + pdfFileName;
                string printer = dw_criteria.GetItemString(1, "printer");
                outputProcess = WebUtil.runProcessingReport(state, app, gid, rid, criteriaXML, pdfFileName, printer);
                //String li_return = lws_report.Run(state.SsWsPass, app, gid, rid, criteriaXML, pdfFileName);
                //if (li_return == "true")
                //{
                //    HdOpenIFrame.Value = "True";
                //}
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
