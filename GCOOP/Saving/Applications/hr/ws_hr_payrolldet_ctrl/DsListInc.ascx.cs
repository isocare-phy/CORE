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
    public partial class DsListInc : DataSourceRepeater
    {
        public DataSet1.DT_ListIncDataTable DATA { get; set; }

        public void InitDsListInc(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.DT_ListInc;
            this.EventItemChanged = "OnDsListIncItemChanged";
            this.EventClicked = "OnDsListIncClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsListInc");
            this.Register();
        }

        public void Retrieve(string as_empno, string as_period)
        {
            string sql = @"select hprd.salitem_code || ' - ' || hus.salitem_desc as salitem_desc,
                hprd.item_amt
                from hrpayroll hpr,
                hrpayrolldet hprd,
                hrucfsalaryitem hus
                where hpr.coop_id = hprd.coop_id
                and hpr.payrollslip_no = hprd.payrollslip_no
                and hprd.salitem_code = hus.salitem_code
                and hpr.coop_id = {0}
                and hpr.emp_no = {1}
                and hpr.payroll_period = {2}
                and hus.sign_flag = 1";

            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, as_empno, as_period);
            Sdt dt = WebUtil.QuerySdt(sql);
            this.ImportData(dt);
        }
    }
}