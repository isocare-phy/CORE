using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Saving.Applications.mis.dlg
{
    public partial class w_dlg_finance_ratio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                HiddenField1.Value = Request["cmd"].ToString();
            }
            catch { HiddenField1.Value = "1"; }
        }
    }
}
