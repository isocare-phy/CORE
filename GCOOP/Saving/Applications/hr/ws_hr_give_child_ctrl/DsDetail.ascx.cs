using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;

namespace Saving.Applications.hr.ws_hr_give_child_ctrl
{
    public partial class DsDetail : DataSourceFormView
    {
        public DataSet1.DT_MAINDataTable DATA { get; set; }
        public void InitDsDetail(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.DT_MAIN;
            this.EventItemChanged = "OnDsDetailChanged";
            this.EventClicked = "OnDsDetailClicked";
            this.InitDataSource(pw, FormView1, this.DATA, "dsDetail");
            this.Register();
        }
        public void Retrieve(string emp_no)
        {
            string sql = @"select hr.emp_no as emp_no,mp.prename_desc||' '||hr.emp_name||' '||hr.emp_surname as fullname,
                            hp.pos_desc as position_emp,hf.f_name as f_name,hf.occupation as occupation,hd.deptgrp_desc as deptgrp_desc
                            from hremployee hr,hremployeefamily hf,mbucfprename mp,hrucfposition hp,hrucfdeptgrp hd
                            where hr.emp_no = hf.emp_no
                            and hr.prename_code = mp.prename_code
                            and hr.deptgrp_code = hd.deptgrp_code
                            and hr.pos_code = hp.pos_code
                            and hf.f_relation = 'คู่สมรส'
                            and hr.coop_id = {0}
                            and hr.emp_no = {1}
";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, emp_no);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
    }
}