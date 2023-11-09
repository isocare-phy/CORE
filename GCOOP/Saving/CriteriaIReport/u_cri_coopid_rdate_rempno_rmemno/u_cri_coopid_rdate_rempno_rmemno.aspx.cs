using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using DataLibrary;
using System.Data;

namespace Saving.CriteriaIReport.u_cri_coopid_rdate_rempno_rmemno
{
    public partial class u_cri_coopid_rdate_rempno_rmemno : PageWebReport, WebReport
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
                dsMain.DATA[0].COOP_ID = state.SsCoopControl;
                dsMain.DATA[0].POST_DATE = state.SsWorkDate;
                dsMain.DATA[0].POST_DATE2 = state.SsWorkDate;
                //dsMain.DATA[0].UINF_ACCTION = "DTA";
                string[] minmaxmembno = ReportUtil.GetMinMaxMembno();
                dsMain.DATA[0].SMEMB_NO = minmaxmembno[0].Trim();
                dsMain.DATA[0].EMEMB_NO = minmaxmembno[1].Trim();

                string[] minmaxempno = GetMinMaxEmpno();
                dsMain.DATA[0].SEMP_NO = minmaxempno[0].Trim();
                dsMain.DATA[0].EEMP_NO = minmaxempno[1].Trim();
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            
        }

        public void RunReport()
        {
            string as_coopid = dsMain.DATA[0].COOP_ID;
            DateTime post_date = dsMain.DATA[0].POST_DATE;
            DateTime post_date2 = dsMain.DATA[0].POST_DATE2;
            //string as_unifacction = dsMain.DATA[0].UINF_ACCTION;
            string as_startmembno = dsMain.DATA[0].SMEMB_NO;
            string as_endmembno = dsMain.DATA[0].EMEMB_NO;
            string as_sempno = dsMain.DATA[0].SEMP_NO;
            string as_eempno = dsMain.DATA[0].EEMP_NO;


            try
            {
                iReportArgument arg = new iReportArgument();
                arg.Add("as_coopid", iReportArgumentType.String, as_coopid);
                arg.Add("post_date", iReportArgumentType.Date, post_date);
                arg.Add("post_date2", iReportArgumentType.Date, post_date2);
                //arg.Add("as_unifacction", iReportArgumentType.String, as_unifacction);
                arg.Add("as_startmembno", iReportArgumentType.String, as_startmembno);
                arg.Add("as_endmembno", iReportArgumentType.String, as_endmembno);
                arg.Add("as_sempno", iReportArgumentType.String, as_sempno);
                arg.Add("as_eempno", iReportArgumentType.String, as_eempno);
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

        public static string[] GetMinMaxEmpno()
        {
            DataTable dt = WebUtil.Query("select min(emp_no) as min, max(emp_no) as max from hremployee ");
            if (dt.Rows.Count > 0)
            {
                return new string[2] { Convert.ToString(dt.Rows[0][0]), Convert.ToString(dt.Rows[0][1]) };
            }
            else { return null; }
        }
    }
}