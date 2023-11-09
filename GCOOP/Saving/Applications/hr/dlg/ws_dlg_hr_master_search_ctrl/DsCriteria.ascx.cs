using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;

namespace Saving.Applications.hr.dlg.ws_dlg_hr_master_search_ctrl
{
    public partial class DsCriteria : DataSourceFormView
    {
        public DataSet1.Dt_CriteriaDataTable DATA { get; set; }

        public void InitDsCriteria(PageWeb pw)
        {
            css1.Visible = false;            
            DataSet1 ds = new DataSet1();
            this.DATA = ds.Dt_Criteria;
            this.InitDataSource(pw, FormView1, this.DATA, "dsCriteria");
            this.EventItemChanged = "OnDsCriteriaItemChanged";
            this.EventClicked = "OnDsCriteriaClicked";
            this.Register();
        }

        public void DdDeptgrp()
        {
            string sql = @"
            select hdg.deptgrp_code,
            hdg.deptgrp_code || ' - ' || hdg.deptgrp_desc || ' สายงาน: ' || hdl.deptline_desc as deptgrp_desc,
            1 as sorter
            from hrucfdeptgrp hdg, hrucfdeptline hdl
            where hdg.deptline_code = hdl.deptline_code
            union 
            select '','', 0 from dual
            order by sorter, deptgrp_code";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "deptgrp_code", "deptgrp_desc", "deptgrp_code");
        }

        public void DdPosition()
        {
            string sql = @"select pos_code,
            pos_desc,
            pos_money,
            1 as sorter
            from hrucfposition
            union 
            select '','', 0, 0 from dual
            order by sorter, pos_code";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "pos_code", "pos_desc", "pos_code");
        }
    }
}