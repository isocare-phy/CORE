using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Saving.Applications.hr
{
    public partial class u_cm_hr_constant_step : System.Web.UI.UserControl
    {
        private DwTrans SQLCA;

        protected void Page_Load(object sender, EventArgs e)
        {
            SQLCA = new DwTrans();
            SQLCA.Connect();
            DwMain.SetTransaction(SQLCA);
            DwMain.Retrieve();
        }

        protected void Page_LoadComplete()
        {
            SQLCA.Disconnect();
        }
    }
}