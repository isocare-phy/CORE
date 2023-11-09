using System;
using CoreSavingLibrary;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using CoreSavingLibrary.WcfNCommon;
using DataLibrary;
using System.Globalization;

namespace Saving.Applications.mis.dlg
{
    public partial class w_dlg_camels_var : System.Web.UI.Page
    {
        DwTrans SQLCA;

        protected void Page_Load(object sender, EventArgs e)
        {
            SQLCA = new DwTrans();
            SQLCA.Connect();

            dw_sheethead.SetTransaction(SQLCA);
            dw_sheethead.Retrieve();
            dw_sheetdet.SetTransaction(SQLCA);
            


            //String req = "";
            int req;
            try
            {
                req = Convert.ToInt32(Request["cmd"]);
                //reqs = Convert.ToInt32(req);
                if (req != 0 && req != null)
                {
                    dw_sheetdet.Retrieve(req);
                }
                else
                {
                    dw_sheetdet.Retrieve(2);
                }
            }
            catch
            {

            }
        }

        protected void Page_LoadComplete()
        {
            try
            {
                SQLCA.Disconnect();
            }
            catch { }
        }
    }
}
