using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;

namespace Saving.Applications.hr.ws_hr_constant_ucfworkintime_ctrl
{
    public partial class DsList : DataSourceRepeater
    {
        public DataSet1.HRUCFINTIMEDataTable DATA { get; set; }

        public void InitDsList(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.HRUCFINTIME;
            this.EventItemChanged = "OnDsListItemChanged";
            this.EventClicked = "OnDsListClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsList");
            this.Button.Add("b_del");
            this.Register();
        }

        public void Retrieve()
        {
            string sql = @"select worktime_code,
                intime
                from hrucfworkintime 
                order by worktime_code";
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
    }
}