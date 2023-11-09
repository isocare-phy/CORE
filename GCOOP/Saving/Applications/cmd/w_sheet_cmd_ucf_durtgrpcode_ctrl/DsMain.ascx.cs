using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Saving.Applications.cmd.w_sheet_cmd_ucf_durtgrpcode_ctrl
{
    public partial class DsMain : DataSourceFormView
    {
        

        public DataSet1.ptucfdurtgrpcodeDataTable DATA { get; set; }

        public void InitDsMain(PageWeb pw)
        {
            css1.Visible = false;


            DataSet1 ds = new DataSet1();
            this.DATA = ds.ptucfdurtgrpcode;
            this.EventItemChanged = "OnDsMainItemChanged";
            this.EventClicked = "OnDsMainClicked";
            this.InitDataSource(pw, FormView1, this.DATA, "dsMain");
            this.Register();
        }

        public void retrieve(string durtgrp_code)
        {
            String sql = @" select durtgrp_code,
durtgrp_desc,
devalue_percent,
durtgrp_abb
from ptucfdurtgrpcode
where durtgrp_code={0}
order by durtgrp_code asc ";
            sql = WebUtil.SQLFormat(sql, durtgrp_code);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
    }
}