using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Saving.Applications.app_assist.dlg
{
    public partial class w_dlg_as_tofrom_accid : PageWebDialog, WebDialog
    {
        private String moneytype_code;
        private String pbl = "as_capital.pbl";

        public void InitJsPostBack()
        {
        }

        public void WebDialogLoadBegin()
        {
            moneytype_code = Request["moneytype_code"].ToString().Trim();
            DwUtil.RetrieveDataWindow(DwMain, pbl, null, moneytype_code);
        }

        public void CheckJsPostBack(string eventArg)
        {
        }

        public void WebDialogLoadEnd()
        {
        }
    }
}