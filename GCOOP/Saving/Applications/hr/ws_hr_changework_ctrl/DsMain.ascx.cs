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
    public partial class DsMain : DataSourceFormView
    {
        public DataSet1.HREMPLOYEEDataTable DATA { get; set; }
        public void InitDs(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.HREMPLOYEE;
            this.EventItemChanged = "OnDsMainItemChanged";
            this.EventClicked = "OnDsMainClicked";

            this.InitDataSource(pw, FormView1, this.DATA, "dsMain");

            this.Register();
        }
        public void RetrieveEmp(string emp_no)
        {
            string sql = @"
                select he.emp_no,he.salary_id,hd.deptgrp_desc,mp.prename_desc,he.emp_name,he.emp_surname,hp.pos_desc 
                from hremployee he,mbucfprename mp,hrucfposition hp,hrucfdeptgrp hd
                where he.emp_no={0} and he.coop_id={1}
                and he.prename_code=mp.prename_code
                and he.deptgrp_code = hd.deptgrp_code(+)
                and he.pos_code=hp.pos_code";
            sql = WebUtil.SQLFormat(sql, emp_no, state.SsCoopId);//กรณี มีการ join ข้อมูล ทำให้ข้อมูลเชื่อมต่อกัน
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
    }
}