using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;

namespace Saving.Applications.hr.ws_hr_changework_ctrl
{
    public partial class DsList : DataSourceRepeater
    {
        public DataSet1.HRLOGCHANGEWORKDataTable DATA { get; set; }

        public void InitDsList(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.HRLOGCHANGEWORK;
            this.EventItemChanged = "OnDsListItemChanged";
            //this.EventClicked = "OnDsListClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsList");
            this.Register();
        }
        public void RetrieveHrEmployee(string emp_no)
        {
            /* string sql = @"select emp_no,emp_name,emp_surname,he.work_date,hp.pos_desc,hg.deptgrp_desc,hl.deptline_desc,he.salary_amt as old_salary_amt,
                             he.deptgrp_code as old_deptgrp_code,
                             he.pos_code as old_pos_code
                         from hremployee he,hrucfposition hp,hrucfdeptgrp hg,hrucfdeptline hl
                         where he.emp_no={0} and he.coop_id={1}
                         and he.pos_code = hp.pos_code
                         and he.deptgrp_code = hg.deptgrp_code
                         and hg.deptline_code = hl.deptline_code";
             sql = WebUtil.SQLFormat(sql, emp_no, state.SsCoopId);
             * */
            string sql = @"select  emp_no,seq_no,
                            hp.pos_desc,hg.deptgrp_desc,hl.deptline_desc,
                            logwork.deptgrp_code as old_deptgrp_code,
                            logwork.salary_amt as old_salary_amt,
                            logwork.pos_code as old_pos_code
                                from 
                                    hrlogchangework logwork,
                                    hrucfposition hp,
                                    hrucfdeptgrp hg,
                                    hrucfdeptline hl
                                where
                                    logwork.emp_no={0} and 
                                    logwork.deptgrp_code=hg.deptgrp_code and
                                    logwork.pos_code=hp.pos_code
                                    and hg.deptline_code = hl.deptline_code";
            sql = WebUtil.SQLFormat(sql, emp_no);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
    }
}