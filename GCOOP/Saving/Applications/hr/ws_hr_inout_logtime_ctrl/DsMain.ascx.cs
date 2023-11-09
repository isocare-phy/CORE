using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CoreSavingLibrary;
using DataLibrary;

namespace Saving.Applications.hr.ws_hr_inout_logtime_ctrl
{
    public partial class DsMain : DataSourceFormView
    {
        public DataSet1.HRLOGWORKTIMEDataTable DATA { get; set; }

        public void InitMain(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.HRLOGWORKTIME;
            this.EventItemChanged = "OnDsMainItemChanged";
            this.EventClicked = "OnDsMainClicked";
            this.InitDataSource(pw, FormView1, this.DATA, "dsMain");
            this.Button.Add("process");
            this.Register();
        }
    }
}