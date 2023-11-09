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
using DataLibrary;

namespace Saving.Criteria
{
    public partial class u_cri_coopid_rdate_rmembgroup_a : PageWebSheet, WebSheet
    {
        protected String app;
        protected String gid;
        protected String rid;
        protected String pdf;
        protected String runProcess;
        protected String popupReport;
        private DwThDate tdw_criteria;
        public String outputProcess = "";
        #region WebSheet Members

        public void InitJsPostBack()
        {
            HdOpenIFrame.Value = "False";
            runProcess = WebUtil.JsPostBack(this, "runProcess");

            tdw_criteria = new DwThDate(dw_criteria, this);
            tdw_criteria.Add("adtm_startdate", "start_tdate");
            tdw_criteria.Add("adtm_enddate", "end_tdate");
        }

        //protected void Page_Load(object sender, EventArgs e)
        public void WebSheetLoadBegin()
        {
            InitJsPostBack();

            //dw_criteria.RestoreContext();
            DwUtil.RetrieveDDDW(dw_criteria, "as_start_memgroup", "criteria.pbl", state.SsCoopControl);
            DwUtil.RetrieveDDDW(dw_criteria, "as_end_memgroup", "criteria.pbl", state.SsCoopControl);

            if (IsPostBack)
            {
                dw_criteria.RestoreContext();
            }
            else
            {
                //default values.
                dw_criteria.InsertRow(0);
                DwUtil.RetrieveDDDW(dw_criteria, "select_coop", "criteria.pbl", state.SsCoopControl);
               // DwUtil.RetrieveDDDW(dw_criteria, "select_coop2", "criteria.pbl", state.SsCoopControl);
                dw_criteria.SetItemString(1, "select_coop", state.SsCoopId);
                //dw_criteria.SetItemString(1, "select_coop2", state.SsCoopId);
                dw_criteria.SetItemDateTime(1, "adtm_startdate", state.SsWorkDate);
                dw_criteria.SetItemDateTime(1, "adtm_enddate", state.SsWorkDate);
                dw_criteria.SetItemString(1, "start_tdate", "");
                dw_criteria.SetItemString(1, "end_tdate", "");
                string[] minmax = ReportUtil.GetMinMaxMembgroup();
                dw_criteria.SetItemString(1, "as_start_memgroup", minmax[0]);
                dw_criteria.SetItemString(1, "as_end_memgroup", minmax[1]);
                tdw_criteria.Eng2ThaiAllRow();
                DwUtil.RetrieveDDDW(dw_criteria, "printer", "criteria.pbl", null);
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
                DwUtil.RetrieveDDDW(dw_criteria, "as_start_memgroup", "criteria.pbl", null);
                DwUtil.RetrieveDDDW(dw_criteria, "as_end_memgroup", "criteria.pbl", null);
            }
        }

        public void SaveWebSheet()
        {
        }

        public void WebSheetLoadEnd()
        {
        }

        #endregion
        #region Report Process

        private void RunProcess()
        {
            //อ่านค่าจากหน้าจอใส่ตัวแปรรอไว้ก่อน.

            //DateTime start_date = dw_criteria.GetItemDateTime(1, "start_date");
            //DateTime end_date = dw_criteria.GetItemDateTime(1, "end_date");
            String as_coopid = dw_criteria.GetItemString(1, "select_coop");
            //String coop_id2 = dw_criteria.GetItemString(1, "select_coop2");
            String adtm_startdate = WebUtil.ConvertDateThaiToEng(dw_criteria, "start_tdate", null);
            String adtm_enddate = WebUtil.ConvertDateThaiToEng(dw_criteria, "end_tdate", null);
            String as_start_memgroup = dw_criteria.GetItemString(1, "as_start_memgroup");
            String as_end_memgroup = dw_criteria.GetItemString(1, "as_end_memgroup");

            //แปลง Criteria ให้อยู่ในรูปแบบมาตรฐาน.
            ReportHelper lnv_helper = new ReportHelper();

            lnv_helper.AddArgument(as_coopid, ArgumentType.String);
           // lnv_helper.AddArgument(coop_id2, ArgumentType.String);
            lnv_helper.AddArgument(adtm_startdate, ArgumentType.DateTime);
            lnv_helper.AddArgument(adtm_enddate, ArgumentType.DateTime);
            lnv_helper.AddArgument(as_start_memgroup, ArgumentType.String);
            lnv_helper.AddArgument(as_end_memgroup, ArgumentType.String);

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
                //}
                string printer = dw_criteria.GetItemString(1, "printer");
                String criteriaXML = lnv_helper.PopArgumentsXML();
                outputProcess = WebUtil.runProcessingReport(state, app, gid, rid, criteriaXML, pdfFileName, printer);

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
