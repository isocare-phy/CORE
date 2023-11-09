using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CoreSavingLibrary;
using DataLibrary;

namespace Saving.Applications.hr.ws_hr_payroll_inc_exc_ctrl
{
    public partial class DsMain : DataSourceFormView
    {
        public DataSet1.DT_MainDataTable DATA { get; set; }

        public void InitDsMain(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.DT_Main;
            this.EventItemChanged = "OnDsMainItemChanged";
            this.EventClicked = "OnDsMainClicked";
            this.InitDataSource(pw, FormView1, this.DATA, "dsMain");
            this.TableName = "HREMPLOYEE";
            this.Register();
        }

        public void Retrieve(string as_empno)
        {
            string sql = @"select hremployee.coop_id,   
                hremployee.emp_no,   
                mbucfprename.prename_desc,
                hremployee.emp_name,   
                hremployee.emp_surname,         
                hremployee.sex,
                hremployee.salary_id,
                hremployee.pos_code || ' - ' || hrucfposition.pos_desc as pos_desc,
                hremployee.emptype_code || ' - ' || hrucfemptype.emptype_desc as emptype_desc,
                hremployee.deptgrp_code || ' - ' || hrucfdeptgrp.deptgrp_desc || ' สายงาน: ' || hrucfdeptline.deptline_desc as deptgrp_desc
            from hremployee,
            mbucfprename,
            hrucfposition,
            hrucfemptype,
            hrucfdeptgrp,
            hrucfdeptline			
            where hremployee.prename_code = mbucfprename.prename_code
            and hremployee.pos_code = hrucfposition.pos_code
            and hremployee.emptype_code = hrucfemptype.emptype_code
            and hremployee.deptgrp_code = hrucfdeptgrp.deptgrp_code(+)
            and hrucfdeptgrp.deptline_code = hrucfdeptline.deptline_code(+)
            and hremployee.coop_id = {0}
            and hremployee.emp_no = {1}";

            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, as_empno);
            Sdt dt = WebUtil.QuerySdt(sql);
            this.ImportData(dt);
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