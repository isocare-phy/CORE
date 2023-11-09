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
    public partial class DsDetail : DataSourceFormView
    {
        public DataSet1.HRLOGCHANGEWORKDataTable DATA { get; set; }
        public void InitDs(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.HRLOGCHANGEWORK;
            this.EventItemChanged = "OnDsDetailItemChanged";
            this.EventClicked = "OnDsDetailClicked";

            this.InitDataSource(pw, FormView1, this.DATA, "dsDetail");

            this.Register();
        }
        public void DdHrucfdeptgrp()//กลุ่มงาน
        {
            string sql = @"
                select Deptgrp_code,Deptgrp_desc, 1 as sorter from hrucfdeptgrp 
                union
                select '','',0 from dual order by sorter";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "deptgrp_code", "deptgrp_desc", "DEPTGRP_CODE");
        }//คิวรี่ กลุ่มงานใหม่
        public void DdHrucfposition()//ตำแหน่งใหม่
        {
            string sql = @"
                select Pos_code,Pos_desc, 1 as sorter from hrucfposition 
                union
                select '','',0 from dual order by sorter";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "pos_code", "pos_desc", "POS_CODE");
        }//คิวรี่ ตำแหน่งใหม่
        public void RetrieveHrEmployee(string emp_no)
        {
            string sql = @"select emp_no,emp_name,emp_surname,he.work_date,hp.pos_desc,hg.deptgrp_desc,hl.deptline_desc,he.salary_amt as old_salary_amt,
                            he.deptgrp_code as old_deptgrp_code,
                            he.pos_code as old_pos_code
                        from hremployee he,hrucfposition hp,hrucfdeptgrp hg,hrucfdeptline hl
                        where he.emp_no={0} and he.coop_id={1}
                        and he.pos_code = hp.pos_code
                        and he.deptgrp_code = hg.deptgrp_code(+)
                        and hg.deptline_code = hl.deptline_code(+)";
            sql = WebUtil.SQLFormat(sql, emp_no,state.SsCoopId);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
    }
}