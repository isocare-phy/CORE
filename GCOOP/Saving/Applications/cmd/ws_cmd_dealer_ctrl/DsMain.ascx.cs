using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Saving.Applications.cmd.ws_cmd_dealer_ctrl
{
    public partial class DsMain : DataSourceFormView
    {
        //protected void Page_Load(object sender, EventArgs e)
        //{

        //}

        public DataSet1.PTUCFDEALERDataTable DATA { get; set; }

        public void InitDsMain(PageWeb pw)
        {
            css1.Visible = false;


            DataSet1 ds = new DataSet1();
            this.DATA = ds.PTUCFDEALER;
            this.EventItemChanged = "OnDsMainItemChanged";
            this.EventClicked = "OnDsMainClicked";
            this.InitDataSource(pw, FormView1, this.DATA, "dsMain");
            this.Register();
        }

        public void retrieve()
        {
            String re = @"SELECT dealer_no,
                            dealer_name,
                            dealer_addr,
                            dealer_taxid,
                            dealer_phone
                          FROM ptucfdealer ";

            DataTable dt = WebUtil.Query(re);
            this.ImportData(dt);
        }
    }
}