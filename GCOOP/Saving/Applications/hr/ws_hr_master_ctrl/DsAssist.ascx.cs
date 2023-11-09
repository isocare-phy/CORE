using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using DataLibrary;
using System.Data;

namespace Saving.Applications.hr.ws_hr_master_ctrl
{
    public partial class DsAssist : DataSourceRepeater
    {
        public DataSet1.HREMPLOYEEASSISTDataTable DATA { get; set; }

        public void InitDsAssist(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.HREMPLOYEEASSIST;
            this.EventItemChanged = "OnDsAssistItemChanged";
            this.EventClicked = "OnDsAssistClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsAssist");
            this.Button.Add("b_del");
            this.Register();
        }

        public void Retrieve(string as_empno)
        {
            if (state.SsCoopControl == "031001")
            {

            string sql = @"  select hremployeeassist.coop_id,   
                 hremployeeassist.emp_no,   
                 hremployeeassist.seq_no,      
                 hremployeeassist.assist_code,   
                 hremployeeassist.assist_sdate as assist_date,   
                 hremployeeassist.assist_detail,   
                 hremployeeassist.assist_amt,   
                 hremployeeassist.assist_remark  
            from hremployeeassist
            where hremployeeassist.coop_id = {0}
            and hremployeeassist.emp_no = {1}";

            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, as_empno);
            Sdt dt = WebUtil.QuerySdt(sql);
            this.ImportData(dt);

            }else{

                string sql = @"  select hremployeeassist.coop_id,   
                 hremployeeassist.emp_no,   
                 hremployeeassist.seq_no,      
                 hremployeeassist.assist_code,   
                 hremployeeassist.assist_date,  
                 hremployeeassist.assist_detail,   
                 hremployeeassist.assist_amt,   
                 hremployeeassist.assist_remark  
            from hremployeeassist
            where hremployeeassist.coop_id = {0}
            and hremployeeassist.emp_no = {1}";

            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, as_empno);
            Sdt dt = WebUtil.QuerySdt(sql);
            this.ImportData(dt);
            }
        }

        public void DdAssist()
        {
            string sql = @"select assist_code,
            assist_desc,
            1 as sorter
            from hrucfassist
            union 
            select '','', 0 from dual
            order by sorter, assist_code";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "assist_code", "assist_desc", "assist_code");
        }
    }
}