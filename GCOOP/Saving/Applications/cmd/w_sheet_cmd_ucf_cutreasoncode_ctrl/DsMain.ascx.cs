using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Saving.Applications.cmd.w_sheet_cmd_ucf_cutreasoncode_ctrl
{
    public partial class DsMain : DataSourceFormView
    {
        

        public DataSet1.ptucfcutreasoncodeDataTable DATA { get; set; }

        public void InitDsMain(PageWeb pw)
        {
            css1.Visible = false;


            DataSet1 ds = new DataSet1();
            this.DATA = ds.ptucfcutreasoncode;
            this.EventItemChanged = "OnDsMainItemChanged";
            this.EventClicked = "OnDsMainClicked";
            this.InitDataSource(pw, FormView1, this.DATA, "dsMain");
            this.Register();
        }

        public void retrieve(string cutreason_code)
        {
            String sql = @"select cutreason_code,
cutreason_desc
from ptucfcutreasoncode
where cutreason_code={0}
order by cutreason_code asc";
            sql = WebUtil.SQLFormat(sql, cutreason_code);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
    }
}