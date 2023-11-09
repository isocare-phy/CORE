using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;

namespace Saving.Applications.hr.ws_hr_cure_family_ctrl
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

        public void RetrieveList(string seq_no, string emp_no)
        {
            string sql = @"select coop_id,
        emp_no,
        seq_no,
        assist_code,
        assist_date,
                assist_name,
                assist_state,
                assist_detail,
                assist_hosname,
                assist_sdate,
                assist_edate,
                assist_amt,
                assist_minamt,
                assist_posit,
                assist_amp,
                assist_prov,
                assist_paper
                from hremployeeassist
                where coop_id={0} and seq_no ={1} and emp_no = {2}";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, seq_no, emp_no);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
    }
}