﻿using System;
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

namespace Saving.Applications.app_assist.w_sheet_cm_constant_config
{
    public partial class u_as_asnufpaytype : System.Web.UI.UserControl
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