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
    public partial class DsListOther : DataSourceRepeater
    {
        public DataSet1.HRBASEPAYROLLOTHERDataTable DATA { get; set; }

        public void InitDsListOther(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.HRBASEPAYROLLOTHER;
            this.EventItemChanged = "OnDsListOtherItemChanged";
            this.EventClicked = "OnDsListOtherClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsListOther");
            this.Button.Add("b_delete");
            this.Register();
        }

        public void Retrieve(string as_empno, string as_period)
        {
            string sql = @"  select hrbasepayrollother.coop_id,   
                hrbasepayrollother.emp_no,   
                hrbasepayrollother.payroll_period,   
                hrbasepayrollother.seq_no,   
                hrbasepayrollother.salitem_code,   
                hrbasepayrollother.payother_desc,   
                hrbasepayrollother.item_amt  
            from hrbasepayrollother
            where hrbasepayrollother.coop_id = {0}
            and hrbasepayrollother.emp_no = {1}";
            //and hrbasepayrollother.payroll_period = {2}

            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, as_empno);
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