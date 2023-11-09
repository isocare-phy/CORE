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
    public partial class DsTraining : DataSourceRepeater
    {
        public DataSet1.HREMPLOYEETRAININGDataTable DATA { get; set; }

        public void InitTraining(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.HREMPLOYEETRAINING;
            this.EventItemChanged = "OnDsTrainingItemChanged";
            this.EventClicked = "OnDsTrainingClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsTraining");
            this.Button.Add("b_del");
            this.Register();
        }

        public void Retrieve(string as_empno)
        {
            string sql = @"select hremployeetraining.coop_id,   
                 hremployeetraining.emp_no,   
                 hremployeetraining.seq_no,   
                 hremployeetraining.tr_date,   
                 hremployeetraining.tr_code,   
                 hremployeetraining.tr_subject,   
                 hremployeetraining.tr_cause,   
                 hremployeetraining.tr_location,   
                 hremployeetraining.tr_institution,   
                 hremployeetraining.tr_fromdate,   
                 hremployeetraining.tr_todate,   
                 hremployeetraining.tr_day,   
                 hremployeetraining.tr_expamt,   
                 hremployeetraining.tr_certdesc,   
                 hremployeetraining.remark  
            from hremployeetraining
            where hremployeetraining.coop_id = {0}
            and hremployeetraining.emp_no = {1}";

            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, as_empno);
            Sdt dt = WebUtil.QuerySdt(sql);
            this.ImportData(dt);
        }
    }
}