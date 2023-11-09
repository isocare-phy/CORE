using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Saving.Applications.cmd.w_sheet_cmd_ucf_invtlitemcode_ctrl
{
    public partial class DsMain : DataSourceFormView
    {
        

        public DataSet1.ptucfinvtlitemcodeDataTable DATA { get; set; }

        public void InitDsMain(PageWeb pw)
        {
            css1.Visible = false;


            DataSet1 ds = new DataSet1();
            this.DATA = ds.ptucfinvtlitemcode;
            this.EventItemChanged = "OnDsMainItemChanged";
            this.EventClicked = "OnDsMainClicked";
            this.InitDataSource(pw, FormView1, this.DATA, "dsMain");
            this.Register();
        }

        public void retrieve(string item_code)
        {
            String sql = @" select item_code,
item_des,
sign_flag
from ptucfinvtlitemcode
where item_code={0}
order by item_code asc";
            sql = WebUtil.SQLFormat(sql, item_code);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
    }
}