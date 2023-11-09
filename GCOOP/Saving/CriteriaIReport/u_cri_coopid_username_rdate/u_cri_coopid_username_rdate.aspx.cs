using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataLibrary;

namespace Saving.CriteriaIReport.u_cri_coopid_username_rdate
{
    public partial class u_cri_coopid_username_rdate : PageWebReport, WebReport
    {
        protected String app;
        protected String gid;
        protected String rid;
        public void InitJsPostBack()
        {
            dsMain.InitDsMain(this);
            dsList.InitDsList(this);
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

            if (rid == "R_AU_001")
            {
                WebUtil.generateTriggerAuditLogAll();
            }
            
            if (!IsPostBack)
            {
                dsMain.DdCoopId();
                dsList.retrieve(state.SsCoopId);
                dsMain.DATA[0].START_DATE = state.SsWorkDate;
                dsMain.DATA[0].END_DATE = state.SsWorkDate;
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
        }

        public void RunReport()
        {
            String username = "";
            DateTime start_date = dsMain.DATA[0].START_DATE;
            DateTime end_date = dsMain.DATA[0].END_DATE;
            try
            {
                username = dsMain.DATA[0].USER_NAME;
            }
            catch { }


            if (!(username.Length > 0))
            {
                username = "%";
            }


            try
            {
                iReportArgument arg = new iReportArgument();

                arg.Add("user_name", iReportArgumentType.String, username+"%");
                arg.Add("coop_control", iReportArgumentType.String, state.SsCoopControl);
                arg.Add("start_date", iReportArgumentType.Date, start_date);
                arg.Add("end_date", iReportArgumentType.Date, end_date);
                iReportBuider report = new iReportBuider(this, arg);
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