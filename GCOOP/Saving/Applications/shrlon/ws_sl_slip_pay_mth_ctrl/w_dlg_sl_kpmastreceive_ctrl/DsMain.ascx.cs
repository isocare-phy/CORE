using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Saving.Applications.shrlon.ws_sl_slip_pay_mth_ctrl.w_dlg_sl_kpmastreceive_ctrl
{
    public partial class DsMain : DataSourceFormView
    {
        public DataSet1.KPMASTRECEIVEDataTable DATA { get; set; }

        public void InitDs(PageWeb pw)
        {
            css1.Visible = false;

            DataSet1 ds = new DataSet1();
            this.DATA = ds.KPMASTRECEIVE;
            this.InitDataSource(pw, FormView1, this.DATA, "dsMain");
            this.EventItemChanged = "OnDsMainItemChanged";
            this.EventClicked = "OnDsMainClicked";
            this.Register();
        }
    }
}