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
    public partial class DsExperience : DataSourceRepeater
    {
        public DataSet1.HREMPLOYEEEXPERIENCEDataTable DATA { get; set; }

        public void InitDsExperience(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.HREMPLOYEEEXPERIENCE;
            this.EventItemChanged = "OnDsExperienceItemChanged";
            this.EventClicked = "OnDsExperienceClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsExperience");
            this.Button.Add("b_del");
            this.Register();
        }

        public void Retrieve(string as_empno)
        {
            string sql = @"  select hremployeeexperience.coop_id,   
                 hremployeeexperience.emp_no,   
                 hremployeeexperience.seq_no,   
                 hremployeeexperience.corp_name,   
                 hremployeeexperience.pos_name,   
                 hremployeeexperience.yearstart,   
                 hremployeeexperience.yearend,   
                 hremployeeexperience.last_salary,   
                 hremployeeexperience.job_type,   
                 hremployeeexperience.job_desc,   
                 hremployeeexperience.awaycause,   
                 hremployeeexperience.remark  
            from hremployeeexperience
            where hremployeeexperience.coop_id = {0}
            and hremployeeexperience.emp_no = {1}";

            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, as_empno);
            Sdt dt = WebUtil.QuerySdt(sql);
            this.ImportData(dt);
        }
    }
}
