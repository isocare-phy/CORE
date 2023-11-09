using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Saving.Applications.app_assist.dlg
{
    public partial class w_dlg_as_bankbranch : PageWebDialog, WebDialog
    {
        private String bank_code;
        private String pbl = "as_capital.pbl";

        public void InitJsPostBack()
        {
        }

        public void WebDialogLoadBegin()
        {
            bank_code = Request["bank_code"].ToString().Trim();
            DwUtil.RetrieveDataWindow(DwMain, pbl, null, bank_code);
        }

        public void CheckJsPostBack(string eventArg)
        {
        }

        public void WebDialogLoadEnd()
        {
        }
    }
}