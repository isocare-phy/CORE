using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;

namespace Saving.Applications.cmd.dlg
{
    public partial class w_dlg_cmd_ptdurtregno : PageWebDialog, WebDialog
    {
        private string slipin_no;
        protected string pbl = "dlg_search.pbl";

        public void InitJsPostBack()
        {
            
        }

        public void WebDialogLoadBegin()
        {
            try
            {
                slipin_no = Request["slipin_no"];
            }
            catch { }
            DwUtil.RetrieveDataWindow(DwDetail, pbl, null, slipin_no);
        }

        public void CheckJsPostBack(string eventArg)
        {
            
        }

        public void WebDialogLoadEnd()
        {
            
        }
    }
}