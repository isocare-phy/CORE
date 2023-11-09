using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataLibrary;
using CoreSavingLibrary;

namespace Saving.CriteriaIReport.u_cri_coopid_rmonth_rgroup
{
    public partial class u_cri_coopid_rmonth_rgroup : PageWebReport, WebReport
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
                dsMain.DdMembgroup();
                
                
            }            
        }

        public void CheckJsPostBack(string eventArg)
        {

        }

        public void RunReport()
        {

            string as_startmonth = dsMain.DATA[0].as_startmonth;
            string as_endmonth = dsMain.DATA[0].as_endmonth;
            string as_startgroup = dsMain.DATA[0].as_startgroup;
            string as_endgroup = dsMain.DATA[0].as_endgroup;

            //if (as_startgroup.Length < 1)
            //{
            //    string sql = "select min(membgroup_code) as getminmemgroup from mbucfmembgroup";
            //    sql = WebUtil.SQLFormat(sql, state.SsCoopId);

            //    Sdt result = WebUtil.QuerySdt(sql);
            //    if (result.Next())
            //    {
            //        as_startgroup = result.GetString("getminmemgroup");
            //    }
            //}

            //if (as_endgroup.Length < 1)
            //{
            //    string sql = "select max(membgroup_code) as getmaxmemgroup from mbucfmembgroup";
            //    sql = WebUtil.SQLFormat(sql, state.SsCoopId);

            //    Sdt result = WebUtil.QuerySdt(sql);
            //    if (result.Next())
            //    {
            //        as_endgroup = result.GetString("getmaxmemgroup");
            //    }
            //}            

            try
            {
                iReportArgument arg = new iReportArgument();

                arg.Add("coop_id", iReportArgumentType.String, state.SsCoopControl);
                arg.Add("as_startmonth", iReportArgumentType.String, as_startmonth);
                arg.Add("as_endmonth", iReportArgumentType.String, as_endmonth);
                arg.Add("as_startgroup", iReportArgumentType.String, as_startgroup);
                arg.Add("as_endgroup", iReportArgumentType.String, as_endgroup);               

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