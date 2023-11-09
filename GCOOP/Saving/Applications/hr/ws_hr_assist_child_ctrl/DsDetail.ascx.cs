using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;

namespace Saving.Applications.hr.ws_hr_assist_child_ctrl
{
    public partial class DsDetail : DataSourceFormView
    {
        public DataSet1.HREMPLOYEEASSISTDataTable DATA { get; set; }
        public void InitDsDetail(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.HREMPLOYEEASSIST;
            this.EventItemChanged = "OnDsDetailChanged";
            this.EventClicked = "OnDsDetailClicked";
            this.InitDataSource(pw, FormView1, this.DATA, "dsDetail");
            this.Register();
        }


        public void RetrieveDetail(string seq_no, string emp_no)
        {
            string sql = @"select ha.coop_id,ha.emp_no,ha.seq_no,ha.assist_date,
                ha.assist_son,
                ha.assist_detail,
                ha.assist_posit,
                ha.assist_month,
                edu.education_code,
                ha.assist_amt,
                ha.assist_paper
                from hremployeeassist ha,
                HRUCFEDUCATION edu
                where ha.EDUCATION_CODE = edu.EDUCATION_CODE and ha.coop_id={0} and ha.seq_no ={1} and ha.emp_no = {2}";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, seq_no, emp_no);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }


        public void DdEducation()
        {
            string sql = @"select education_code,
            education_desc,
            1 as sorter
            from hrucfeducation
            union 
            select '','', 0 from dual
            order by sorter, education_code";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "education_code", "education_desc", "education_code");
        }

    }
}