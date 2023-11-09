using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;

namespace Saving.Applications.hr.ws_hr_cure_newfamily_ctrl
{
    public partial class DsCure : DataSourceFormView
    {
        public DataSet1.HREMPLOYEEASSISTDataTable DATA { get; set; }
        public void InitDs(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.HREMPLOYEEASSIST;
            this.EventItemChanged = "OnDsLeaveItemChanged";
            this.EventClicked = "OnDsLeaveClicked";
            this.InitDataSource(pw, FormView1, this.DATA, "dsLeave");
            this.Register();
        }

        public void Retrieveleave(string seq_no, string emp_no)
        {
            string sql = @"select
                            emp_no,
                            seq_no,
                            assist_name,
                            assist_state,
                            assist_hosname,
                            assist_posit,
                            assist_amp,
                            assist_sdate,
                            assist_amt,
                            assist_minamt
                            from hremployeeassist where coop_id={0} and seq_no = {1} and emp_no = {2}";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, seq_no, emp_no);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }

    }
}