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
    }
}