using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;

namespace Saving.Applications.cmd.dlg
{
    public partial class w_dlg_cmd_contractdealer : PageWebDialog, WebDialog
    {
        private string dealer_no;
        protected string pbl = "dlg_search.pbl";

        public void InitJsPostBack()
        {

        }

        public void WebDialogLoadBegin()
        {
            try
            {
                dealer_no = Request["dealer_no"];
            }
            catch { }
            DwUtil.RetrieveDataWindow(DwDetail, pbl, null, dealer_no);
        }

        public void CheckJsPostBack(string eventArg)
        {

        }

        public void WebDialogLoadEnd()
        {

        }
    }
}