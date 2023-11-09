using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using DataLibrary;

namespace Saving.Applications.hr.ws_hr_payrolldet_ctrl
{
    public partial class DsListEmp : DataSourceRepeater
    {
        public DataSet1.DT_ListEmpDataTable DATA { get; set; }

        public void InitDsListEmp(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.DT_ListEmp;
            this.EventItemChanged = "OnDsListEmpItemChanged";
            this.EventClicked = "OnDsListEmpClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsListEmp");
            this.Register();
        }

        public void Retrieve()
        {
            string sql = @"select hremployee.coop_id,   
                hremployee.emp_no,   
                mbucfprename.prename_desc,
                hremployee.emp_name,   
                hremployee.emp_surname,
                hremployee.salary_id
                from hremployee,
                mbucfprename	
                where hremployee.prename_code = mbucfprename.prename_code
                and hremployee.coop_id = {0}
                and hremployee.emp_status = 1 order by emp_no";

            sql = WebUtil.SQLFormat(sql, state.SsCoopControl);
            Sdt dt = WebUtil.QuerySdt(sql);
            this.ImportData(dt);
        }
    }
}