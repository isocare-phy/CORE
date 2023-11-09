using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Saving.Applications.cmd.w_sheet_cmd_ucf_invtadjtype_ctrl
{
    public partial class DsMain : DataSourceFormView
    {
        

        public DataSet1.ptucfinvtadjtypeDataTable DATA { get; set; }

        public void InitDsMain(PageWeb pw)
        {
            css1.Visible = false;
            

            DataSet1 ds = new DataSet1();
            this.DATA = ds.ptucfinvtadjtype;
            this.EventItemChanged = "OnDsMainItemChanged";
            this.EventClicked = "OnDsMainClicked";
            this.InitDataSource(pw, FormView1, this.DATA, "dsMain");
            this.Register();
        }

        public void retrieve(string adjtype_code)
        {
            String sql = @" select adjtype_code,
adjtype_desc,
trim(sign_flag) as  sign_flag
from ptucfinvtadjtype
where adjtype_code={0}
order by adjtype_code asc";
            sql = WebUtil.SQLFormat(sql, adjtype_code);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }

       
    }
}