using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;

namespace Saving.Applications.hr.ws_hr_constant_ucfposition_ctrl
{
    public partial class DsList : DataSourceRepeater
    {
        public DataSet1.HRUCFPOSITIONDataTable DATA { get; set; }

        public void InitDsList(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.HRUCFPOSITION;
            this.EventItemChanged = "OnDsListItemChanged";
            this.EventClicked = "OnDsListClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsList");
            this.Button.Add("b_delete");
            this.Register();
        }

        public void Retreive()
        {
            string sql = @"
            select hrucfposition.pos_code,   
                hrucfposition.pos_desc,   
                hrucfposition.pos_money  
            from hrucfposition order by pos_code";
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
    }
}