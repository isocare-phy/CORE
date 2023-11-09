using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using DataLibrary;

namespace Saving.Applications.hr.ws_hr_master_ctrl
{
    public partial class DsCurefamily : DataSourceRepeater
    {
        public DataSet1.HREMPLOYEECUREFAMILY1DataTable DATA { get; set; }

        public void InitDsCurefamily(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.HREMPLOYEECUREFAMILY1;
            this.EventItemChanged = "OnDsCurefamilyemChanged";
            this.EventClicked = "OnDsCurefamilyClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsCurefamily");
            this.Button.Add("b_del");
            this.Register();
        }

        public void Retrieve(string as_empno)
        {
            string sql = @"select 
                            assist_name,
                            assist_state,
                            assist_hosname,
                            assist_posit,
                            assist_amp,
                            assist_sdate,
                            assist_edate,
                            assist_amt,
                            assist_minamt,
                            assist_paper from hremployeeassist where coop_id={0} and emp_no = {1}";

            sql = WebUtil.SQLFormat(sql, state.SsCoopId, as_empno);
            Sdt dt = WebUtil.QuerySdt(sql);
            this.ImportData(dt);
        }
    }
}