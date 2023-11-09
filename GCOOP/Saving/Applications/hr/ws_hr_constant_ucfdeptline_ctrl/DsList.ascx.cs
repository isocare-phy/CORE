using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;

namespace Saving.Applications.hr.ws_hr_constant_ucfdeptline_ctrl
{
    public partial class DsList : DataSourceRepeater
    {
        public DataSet1.HRUCFDEPTLINEDataTable DATA { get; set; }

        public void InitDsList(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.HRUCFDEPTLINE;
            this.EventItemChanged = "OnDsListItemChanged";
            this.EventClicked = "OnDsListClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsList");
            this.Button.Add("b_delete");
            this.Register();
        }

        public void Retreive()
        {
            string sql = @"
            select deptline_code, deptline_desc from hrucfdeptline order by deptline_code";
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
    }
}