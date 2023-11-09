using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;

namespace Saving.Applications.hr.ws_hr_constant_ucfdeptgrp_ctrl
{
    public partial class DsList : DataSourceRepeater
    {
        public DataSet1.HRUCFDEPTGRPDataTable DATA { get; set; }

        public void InitDsList(PageWeb pw)
        {
            css1.Visible = false;            
            DataSet1 ds = new DataSet1();
            this.DATA = ds.HRUCFDEPTGRP;
            this.EventItemChanged = "OnDsListItemChanged";
            this.EventClicked = "OnDsListClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsList");
            this.Button.Add("b_delete");
            this.Register();
        }

        public void Retreive()
        {
            string sql = @"
            select hrucfdeptgrp.deptgrp_code,   
                hrucfdeptgrp.deptgrp_desc,   
                hrucfdeptgrp.deptline_code  
            from hrucfdeptgrp order by deptgrp_code";
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }

        public void DdDeptline()
        {
            string sql = @"
            select hrucfdeptline.deptline_code,   
            hrucfdeptline.deptline_desc,
            1 as sorter  
            from hrucfdeptline
             union 
            select '','', 0 from dual
            order by sorter, deptline_code";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "deptline_code", "deptline_desc", "deptline_code");
        }
    }
}