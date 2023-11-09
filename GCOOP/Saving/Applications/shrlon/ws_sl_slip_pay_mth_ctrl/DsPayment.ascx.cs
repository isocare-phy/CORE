using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Saving.Applications.shrlon.ws_sl_slip_pay_mth_ctrl
{
    public partial class DsPayment : DataSourceFormView
    {
        public DataSet1.SUMPAYMENTDataTable DATA { get; set; }

        public void InitDsPayment(PageWeb pw)
        {
            css1.Visible = false;
            css2.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.SUMPAYMENT;
            this.InitDataSource(pw, FormView1, this.DATA, "dsPayment");
            this.EventItemChanged = "OnDsPaymentItemChanged";
            this.EventClicked = "OnDsPaymentClicked";
            this.Register();
        //    this.Button.Add("b_memsearch");

        //    this.Button.Add("b_ref");
        }

    }
}