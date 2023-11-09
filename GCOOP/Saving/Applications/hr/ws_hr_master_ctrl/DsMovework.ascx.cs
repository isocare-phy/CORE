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
    public partial class DsMovework : DataSourceRepeater
    {
        public DataSet1.HRLOGCHANGEWORKDataTable DATA { get; set; }

        public void InitDsMovework(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.HRLOGCHANGEWORK;
            this.EventItemChanged = "OnDsMoveworkItemChanged";
            this.EventClicked = "OnDsListClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsMovework");
            this.Register();
        }

        public void Retrieve(string emp_no)
        {
            string sql = @"select
      hc.emp_no,
      hc.order_docno,
      hc.order_date,
      hp.pos_desc||' '||hd.deptgrp_desc as deptgrp_desc,
      hc.salary_amt,
      hp2.pos_desc||' '||hd2.deptgrp_desc as pos_desc,
      hc.old_salary_amt
from
      hrlogchangework hc,
      hrucfposition hp,
      hrucfdeptgrp hd,
      hrucfposition hp2,
      hrucfdeptgrp hd2
where
      hc.emp_no = {0} and
      hc.deptgrp_code = hd.deptgrp_code and
      hc.pos_code = hp.pos_code and
      hc.old_deptgrp_code = hd2.deptgrp_code and
      hc.old_pos_code = hp2.pos_code 
order by hc.seq_no";
            sql = WebUtil.SQLFormat(sql, emp_no);
            Sdt dt = WebUtil.QuerySdt(sql);
            this.ImportData(dt);
        }
    }
}