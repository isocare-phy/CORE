using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;
using DataLibrary;

namespace Saving.Applications.hr.ws_hr_payroll_inc_exc_ctrl
{
    public partial class DsList : DataSourceFormView
    {
        public DataSet1.HRBASEPAYROLLFIXEDDataTable DATA { get; set; }

        public void InitDsList(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.HRBASEPAYROLLFIXED;
            this.EventItemChanged = "OnDsListItemChanged";
            this.EventClicked = "OnDsListClicked";
            this.InitDataSource(pw, FormView1, this.DATA, "dsList");
            this.Register();
        }

        public void Retrieve(string as_empno)
        {
            string sql = @"select 
                (select sum(hrbasepayrollfixed.item_amt)  
                from hrbasepayrollfixed 
                where hrbasepayrollfixed.salitem_code like 'P%' 
                      and hrbasepayrollfixed.coop_id = {0}
            and hrbasepayrollfixed.emp_no = {1}) as pay,
                      (select sum(hrbasepayrollfixed.item_amt)  
                from hrbasepayrollfixed 
                where hrbasepayrollfixed.salitem_code like 'R%' 
                      and hrbasepayrollfixed.coop_id = {0}
            and hrbasepayrollfixed.emp_no = {1})as roll
            from hrbasepayrollfixed 
            where hrbasepayrollfixed.coop_id = {0}
            and hrbasepayrollfixed.emp_no = {1}";

            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, as_empno);
            Sdt dt = WebUtil.QuerySdt(sql);
            this.ImportData(dt);
        }
    }
}