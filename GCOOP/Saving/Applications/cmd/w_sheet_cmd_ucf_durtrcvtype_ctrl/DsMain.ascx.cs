using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Saving.Applications.cmd.w_sheet_cmd_ucf_durtrcvtype_ctrl
{
    public partial class DsMain : DataSourceFormView
    {
        

        public DataSet1.ptucfdurtrcvtypeDataTable DATA { get; set; }

        public void InitDsMain(PageWeb pw)
        {
            css1.Visible = false;


            DataSet1 ds = new DataSet1();
            this.DATA = ds.ptucfdurtrcvtype;
            this.EventItemChanged = "OnDsMainItemChanged";
            this.EventClicked = "OnDsMainClicked";
            this.InitDataSource(pw, FormView1, this.DATA, "dsMain");
            this.Register();
        }

        public void retrieve(string durtrcv_code)
        {
            String sql = @"select durtrcv_code,
durtrcv_desc
from ptucfdurtrcvtype
where durtrcv_code={0}
order by durtrcv_code asc ";
            sql = WebUtil.SQLFormat(sql, durtrcv_code);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
    }
}