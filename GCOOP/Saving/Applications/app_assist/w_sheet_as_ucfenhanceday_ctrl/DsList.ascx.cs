using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Saving.Applications.app_assist.w_sheet_as_ucfenhanceday_ctrl
{
    public partial class DsList : DataSourceRepeater
    {
        public DataSet1.ASNENVGROUPDataTable DATA { get; set; }

        public void InitDsList(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.ASNENVGROUP;
            this.Button.Add("b_del");
            this.EventClicked = "OnDsListClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsList");
            this.Register();
        }

        public void retrieve()
        {
            string sql = @" select * from asnenvgroup ";
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }


    }
}