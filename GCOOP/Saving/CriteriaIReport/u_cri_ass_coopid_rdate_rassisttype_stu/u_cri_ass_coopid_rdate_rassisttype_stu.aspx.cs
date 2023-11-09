using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using DataLibrary;

namespace Saving.CriteriaIReport.u_cri_ass_coopid_rdate_rassisttype_stu
{
    public partial class u_cri_ass_coopid_rdate_rassisttype_stu : PageWebReport, WebReport
    {
        protected String app;
        protected String gid;
        protected String rid;

        public void InitJsPostBack()
        {
            dsMain.InitDsMain(this);
        }

        public void WebSheetLoadBegin()
        {
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

            if (!IsPostBack)
            {
                dsMain.DdCoopId();
                dsMain.DATA[0].coop_id = state.SsCoopId;
                dsMain.DdStartassisttype();
                dsMain.DdEndassisttype();
                dsMain.DefaultStartassisttype();
                dsMain.DefaultEndassisttype();
                dsMain.DATA[0].start_date = state.SsWorkDate;
                dsMain.DATA[0].end_date = state.SsWorkDate;
                dsMain.DATA[0].type_sort = "0";
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            
        }

        public void RunReport()
        {
            //int li_period = 0;
            string as_coopid = dsMain.DATA[0].coop_id;
            DateTime date1 = dsMain.DATA[0].start_date;
            DateTime date2 = dsMain.DATA[0].end_date;
            string assisttype_start = dsMain.DATA[0].assisttype_start;
            string assisttype_end = dsMain.DATA[0].assisttype_end;
            string type_sort = dsMain.DATA[0].type_sort;
            String szDescTitle = "";
            if (type_sort == "1")
            {
                szDescTitle = "r_ass_req_stu";
            }
            else
            {
                szDescTitle = "r_ass_req_stu_sort_gpa";

            }
            try
            {
                iReportArgument arg = new iReportArgument();
                //as_coopid ตัวแปรใน iReport
                arg.Add("as_coopid", iReportArgumentType.String, state.SsCoopId);
                arg.Add("date1", iReportArgumentType.Date, date1);
                arg.Add("date2", iReportArgumentType.Date, date2);
                arg.Add("assisttype_start", iReportArgumentType.String, assisttype_start);
                arg.Add("assisttype_end", iReportArgumentType.String, assisttype_end);
                iReportBuider report = new iReportBuider(this, arg, szDescTitle);
                report.Retrieve();
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void WebSheetLoadEnd()
        {
            
        }
    }
}