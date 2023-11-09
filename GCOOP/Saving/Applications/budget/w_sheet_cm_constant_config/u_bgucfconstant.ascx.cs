using System;
using CoreSavingLibrary;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace Saving.Applications.budget
{
    public partial class u_bgucfconstant : System.Web.UI.UserControl
    {
        private DwTrans sqlca;
        private WebState state;
        protected void Page_Load(object sender, EventArgs e)
        {
            state = new WebState(Session, Request);
            sqlca = new DwTrans();
            sqlca.Connect();
            DwMain.SetTransaction(sqlca);
            DwMain.Retrieve();
        }

        protected void Page_LoadComplete()
        {
            try
            {
                sqlca.Disconnect();
            }
            catch { }
        }
    }
}