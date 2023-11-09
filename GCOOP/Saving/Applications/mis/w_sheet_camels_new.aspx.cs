using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Saving.Applications.mis
{
    public partial class w_sheet_camels_new : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int li_year, li_month;
            li_year = 2553;
            li_month = 7;

            if (!IsPostBack)
            {
                //.Initial values.
                dw_cri.InsertRow(0);
                dw_cri.SetItemDecimal(1, "start_year", li_year - 1);
                dw_cri.SetItemDecimal(1, "end_year", li_year);
                dw_cri.SetItemDecimal(1, "start_month", 12);
                dw_cri.SetItemDecimal(1, "end_month", li_month);
            }
        }
    }
}
