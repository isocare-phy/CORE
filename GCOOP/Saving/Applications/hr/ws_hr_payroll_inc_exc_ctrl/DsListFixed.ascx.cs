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
    public partial class DsListFixed : DataSourceRepeater
    {
        public DataSet1.HRBASEPAYROLLFIXEDDataTable DATA { get; set; }

        public void InitDsListFixed(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.HRBASEPAYROLLFIXED;
            this.EventItemChanged = "OnDsListFixedItemChanged";
            this.EventClicked = "OnDsListFixedClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsListFixed");
            this.Button.Add("b_delete");
            this.Register();
        }

        public void Retrieve(string as_empno)
        {
            string sql = @"select hrbasepayrollfixed.coop_id,   
                hrbasepayrollfixed.emp_no,   
                hrbasepayrollfixed.seq_no,   
                hrbasepayrollfixed.salitem_code,
                (select sum(hrbasepayrollfixed.item_amt)  
                from hrbasepayrollfixed 
                where hrbasepayrollfixed.salitem_code like 'P%' 
                      and hrbasepayrollfixed.coop_id = {0}
            and hrbasepayrollfixed.emp_no = {1}) as pay,
                      (select sum(hrbasepayrollfixed.item_amt)  
                from hrbasepayrollfixed 
                where hrbasepayrollfixed.salitem_code like 'R%' 
                      and hrbasepayrollfixed.coop_id = {0}
            and hrbasepayrollfixed.emp_no = {1})as roll,
                hrbasepayrollfixed.item_amt  
            from hrbasepayrollfixed 
            where hrbasepayrollfixed.coop_id = {0}
            and hrbasepayrollfixed.emp_no = {1}";

            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, as_empno );
            Sdt dt = WebUtil.QuerySdt(sql);
            this.ImportData(dt);
        }

        public void DdSalaryItem()
        {
            string sql = @"
                select salitem_code,
                salitem_desc,1 as sorter 
                from hrucfsalaryitem where manual_flag = 1
            union
            select '','',0 from dual order by sorter, salitem_code";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "salitem_code", "salitem_desc", "salitem_code");
        }
    }
}