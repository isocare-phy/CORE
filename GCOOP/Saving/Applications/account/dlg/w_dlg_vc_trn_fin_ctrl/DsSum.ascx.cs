using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Saving.Applications.account.dlg.w_dlg_vc_trn_fin_ctrl
{
    public partial class DsSum : DataSourceFormView
    {
        public DataSet1.FINSLIPDataTable DATA { get; set; }
        public void InitDsSum(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.FINSLIP;
            this.EventItemChanged = "OnDsSumItemChanged";
            this.EventClicked = "OnDsSumClicked";
            this.InitDataSource(pw, FormView1, this.DATA, "dsSum");
            this.Button.Add("btn_ok");
            this.Button.Add("btn_cancel");
            this.Register();
        }
    }
}