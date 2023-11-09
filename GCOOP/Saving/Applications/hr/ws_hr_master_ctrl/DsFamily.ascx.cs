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
    public partial class DsFamily : DataSourceRepeater
    {
        public DataSet1.HREMPLOYEEFAMILYDataTable DATA { get; set; }

        public void InitDsFamily(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.HREMPLOYEEFAMILY;
            this.EventItemChanged = "OnDsFamilyemChanged";
            this.EventClicked = "OnDsFamilyClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsFamily");
            this.Button.Add("b_del");
            this.Register();
        }

        public void Retrieve(string as_empno)
        {
            string sql = @"select hremployeefamily.coop_id,   
                 hremployeefamily.emp_no,   
                 hremployeefamily.seq_no,   
                 hremployeefamily.f_name,   
                 hremployeefamily.f_relation,   
                 hremployeefamily.birth_date,   
                 hremployeefamily.occupation  
            from hremployeefamily
            where hremployeefamily.coop_id = {0}
            and hremployeefamily.emp_no = {1}";

            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, as_empno);
            Sdt dt = WebUtil.QuerySdt(sql);
            this.ImportData(dt);
        }
    }
}