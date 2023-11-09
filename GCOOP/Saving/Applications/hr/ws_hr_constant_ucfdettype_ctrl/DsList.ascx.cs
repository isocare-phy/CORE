using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CoreSavingLibrary;

namespace Saving.Applications.hr.ws_hr_constant_ucfdettype_ctrl
{
    public partial class DsList : DataSourceRepeater
    {
        public DataSet1.HRUCFEMPTYPEDataTable DATA { get; set; }

        public void InitDsList(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.HRUCFEMPTYPE;
            this.EventItemChanged = "OnDsListItemChanged";
            this.EventClicked = "OnDsListClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsList");
            this.Button.Add("b_delete");
            this.Register();
        }
        public void Retreive()
        {
            string sql = @"
            select emptype_code,emptype_desc from hrucfemptype order by emptype_code";
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
    }
}