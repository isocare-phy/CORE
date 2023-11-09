using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;

namespace Saving.Applications.hr.ws_hr_payroll_process_ctrl
{
    public partial class DsMain : DataSourceFormView
    {
        public DataSet1.DT_mainDataTable DATA { get; set; }

        public void InitDsMain(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.DT_main;
            this.EventItemChanged = "OnDsMainItemChanged";
            this.EventClicked = "OnDsMainClicked";
            this.InitDataSource(pw, FormView1, this.DATA, "dsMain");
            this.Register();
        }

        public void DdEmptype()
        {
            string sql = @"select emptype_code,
            emptype_desc,
            1 as sorter
            from hrucfemptype
            union 
            select '','', 0 from dual
            order by sorter, emptype_code";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "emptype_code", "emptype_desc", "emptype_code");
        }
    }
}