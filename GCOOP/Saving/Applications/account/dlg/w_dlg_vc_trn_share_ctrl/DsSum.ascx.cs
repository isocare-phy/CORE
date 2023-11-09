using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Saving.Applications.account.dlg.w_dlg_vc_trn_share_ctrl
{
    public partial class DsSum : DataSourceFormView
    {
        public DataSet1.DataTable1DataTable DATA { get; set; }
        public void InitDsSum(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.DataTable1;
            this.EventItemChanged = "OnDsSumItemChanged";
            this.EventClicked = "OnDsSumClicked";
            this.InitDataSource(pw, FormView1, this.DATA, "dsSum");
            this.Button.Add("btn_ok");
            this.Button.Add("btn_cancel");
            this.Register();
        }
    }
}