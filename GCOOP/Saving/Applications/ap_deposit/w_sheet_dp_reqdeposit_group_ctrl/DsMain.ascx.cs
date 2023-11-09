using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;

namespace Saving.Applications.ap_deposit.w_sheet_dp_reqdeposit_group_ctrl
{
    public partial class DsMain : DataSourceFormView
    {
        public DataSet1.DataMainDataTable DATA { get; set; }
        public void InitDsMain(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.DataMain;
            this.EventItemChanged = "OnDsMainItemChanged";
            this.EventClicked = "OnDsMainClicked";
            this.InitDataSource(pw, FormView1, this.DATA, "dsMain");
            //this.Button.Add("b_process");
            this.Button.Add("b_save");
            this.Register();
        }
    }
}