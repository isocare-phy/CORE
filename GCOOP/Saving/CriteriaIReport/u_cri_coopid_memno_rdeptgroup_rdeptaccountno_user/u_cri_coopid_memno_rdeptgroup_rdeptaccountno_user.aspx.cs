using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using DataLibrary;

namespace Saving.CriteriaIReport.u_cri_coopid_memno_rdeptgroup_rdeptaccountno_user
{
    public partial class u_cri_coopid_memno_rdeptgroup_rdeptaccountno_user : PageWebReport, WebReport
    {
        protected String app;
        protected String gid;
        protected String rid;
        [JsPostBack]
        public String PostMemberNo;
        [JsPostBack]
        public String PostName { get; set; }

        public void InitJsPostBack()
        {
            dsMain.InitDsMain(this);
            PostMemberNo = WebUtil.JsPostBack(this, "PostMemberNo");
            //PostName = WebUtil.JsPostBack(this, "PostName");
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
                
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            //if (eventArg == PostMemberNo)
            //{
            //    string membno = dsMain.DATA[0].memno;
            //    membno = WebUtil.MemberNoFormat(membno);
            //    dsMain.DATA[0].memno = membno;
            //}
                if (eventArg == "PostMemberNo")
                {
                    string membno = WebUtil.MemberNoFormat(dsMain.DATA[0].memno);
                    dsMain.DATA[0].memno = membno;
                    of_GetMemName();
                    //dsMain.RetrieveMain(membno);
                }
                if (eventArg == "PostName")
                {
                    of_GetMemName();
                }
        }

        public void RunReport()
        {
            string ls_memno2="%",ls_memno3="%";
            string ls_memno1 = dsMain.DATA[0].memno;
            string ls_deptaccountno = dsMain.DATA[0].deptaccount_no;
            if (ls_deptaccountno != "")
            {
                ls_memno3 = dsMain.DATA[0].memno;
                ls_deptaccountno = ls_deptaccountno.Substring(1, ls_deptaccountno.Length-2);
            }
            string ls_share = dsMain.DATA[0].c_share;
            if(ls_share=="1"){ ls_memno2=dsMain.DATA[0].memno;}
            
            string ls_guarantee = dsMain.DATA[0].guarantee;
            string ls_language = dsMain.DATA[0].language;
            string ls_manager = dsMain.DATA[0].as_manager;
            string ls_seconder = dsMain.DATA[0].seconder;
            string ls_username = dsMain.DATA[0].user_name;
            string ls_country = dsMain.DATA[0].country;
            decimal ls_us = dsMain.DATA[0].us;
            string ls_deptaccountname = dsMain.DATA[0].deptaccount_name;
            string sql = @"create or replace view report_certificate as select distinct mb.member_no,
            'MAS' as system,
            '' as  prncbal1,
            '' as  sharestk_amt1,
            '' as  prncbal_us,
            '' as  sharestk_amt_us,
            '' as  sharestk_shr,
            '' as  deptaccount_no,
            'Name of Account:' as eng_desc,
            '' as type_desc,
            '' as type_shere,
            '' as type_tbh,
            '' as type_us,
            (pn.PRENAME_desc||mb.MEMB_NAME||' '||mb.MEMB_SURNAME) as fullname_thai,
            '' as deptaccount_name,
            0 as prncbal,
            0 as sharestk_amt,
            ''as bth_thai
            from  mbmembmaster mb
            inner join MBUCFPRENAME pn on  pn.PRENAME_CODE = mb.PRENAME_CODE
            where
             mb.member_no ={1}
            and mb.coop_id ={0}

            union

            select distinct mb.member_no,
            'DEP' as system ,
            TRIM(TO_CHAR(dp.prncbal, '999,999,999,999,999.00')) as prncbal1,
            '' as sharestk_amt1,
            TRIM(TO_CHAR((dp.prncbal/{5}), '999,999,999,999,999.00')) as prncbal_us,
            '' as sharestk_amt_us,
            '' as sharestk_shr,
            substr(dp.deptaccount_no,0,3)||'-'||substr(dp.deptaccount_no,4,2)||'-'||substr(dp.deptaccount_no,6,5) as deptaccount_no,
            'Type of Accounts : The deposit of saving account(s) number' as eng_desc,
            '' as type_desc,
            'credit' as type_shere,
            'balance THB' as type_tbh,
            'Approx US $' as type_us,
            (pn.PRENAME_desc||mb.MEMB_NAME||' '||mb.MEMB_SURNAME) as fullname_thai,
            dp.deptaccount_name,
            dp.prncbal,
            0 as sharestk_amt,
            ft_readtbaht(dp.prncbal) as bth_thai
            from  mbmembmaster mb
            inner join MBUCFPRENAME pn on  pn.PRENAME_CODE = mb.PRENAME_CODE
            inner join dpdeptmaster dp on mb.member_no = dp.member_no
            where
             mb.member_no ={2}
            and dp.deptaccount_no in({4})
            and mb.coop_id = {0}
            and dp.deptclose_status=0


            union

            select distinct mb.member_no,
            'SHR' as system ,
            '' as  prncbal1,
            TRIM(TO_CHAR(s.sharestk_amt, '999,999,999,999,999')) as sharestk_amt1,
            '' as  prncbal_us,
            TRIM(TO_CHAR((s.sharestk_amt*10)/{5}, '999,999,999,999,999.00')) as sharestk_amt_us,
            TRIM(TO_CHAR((s.sharestk_amt*10), '999,999,999,999,999.00')) as sharestk_shr,
            '' as  deptaccount_no,
            'The member of Tak Saving and Credit Co-operative for Officials in Ministry of Education Limited member' as eng_desc,
            'holding' as type_desc,
            'share' as type_shere,
            'equal THB' as type_tbh,
            'Approx US $' as type_us,
            (pn.PRENAME_desc||mb.MEMB_NAME||' '||mb.MEMB_SURNAME) as  fullname_thai,
            '' as deptaccount_name,
            0 as prncbal,
            s.sharestk_amt,
            ft_readtbaht( s.sharestk_amt*10) as bth_thai
            from  mbmembmaster mb
            inner join shsharemaster s on  s.member_no = mb.member_no
            inner join MBUCFPRENAME pn on  pn.PRENAME_CODE = mb.PRENAME_CODE
            where
            mb.member_no ={3}
            and mb.coop_id ={0}
            and s.sharemaster_status = 1
            ";

                //sql = sql.Replace("$P{as_coopid}", format2);
                sql = WebUtil.SQLFormat(sql, state.SsCoopControl,ls_memno1, ls_memno2, ls_memno3,ls_deptaccountno,ls_us);
                Sdt dtIns = WebUtil.QuerySdt(sql);
            try
            {
                string report_name = "";
                if (ls_language == "thai") { report_name = "rpt_certificate_thai"; }
                else if (ls_language == "eng") { report_name = "rpt_certificate"; }
                string report_label = report_name;
                iReportArgument arg = new iReportArgument();
                arg.Add("as_manager", iReportArgumentType.String, ls_manager);
                arg.Add("as_name", iReportArgumentType.String, ls_seconder);
                arg.Add("as_type", iReportArgumentType.Integer, ls_guarantee);

                iReportBuider report = new iReportBuider(this, arg);
                //iReportBuider report = new iReportBuider(this, "");
                //report.AddCriteria("r_ln_print_loan_req_doc_rbt", "สัญญาเงินกู้", ReportType.pdf, args);
                report.AddCriteria(report_name, report_label, ReportType.pdf, arg);
                report.AutoOpenPDF = true;
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

        public void of_GetMemName()
        {
            if (dsMain.DATA[0].language == "thai")
            {
                string sql = @"select  (pn.PRENAME_desc||mb.MEMB_NAME||' '||mb.MEMB_SURNAME) as  mem_name  from  mbmembmaster mb           
            inner join MBUCFPRENAME pn on pn.PRENAME_CODE = mb.PRENAME_CODE
            WHERE mb.coop_id={0} and  mb.member_no={1} ";
                sql = WebUtil.SQLFormat(sql, state.SsCoopControl, dsMain.DATA[0].memno);
                Sdt dt = WebUtil.QuerySdt(sql);
                if (dt.Next())
                {
                    dsMain.DATA[0].seconder = dt.GetString("mem_name");
                }
                Page.ClientScript.RegisterStartupScript(this.GetType(), "init_memname", "init_memname(1)", true);
            }
            else
            {
                dsMain.DATA[0].seconder = "";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "init_memname", "init_memname(2)", true);
            }
        }
    }
}