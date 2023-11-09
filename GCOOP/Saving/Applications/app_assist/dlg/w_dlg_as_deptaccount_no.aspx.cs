using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Saving.Applications.app_assist.dlg
{
    public partial class w_dlg_as_deptaccount_no : PageWebDialog, WebDialog
    {
        private String member_no;
        private String pbl = "as_capital.pbl";

        public void InitJsPostBack()
        {
        }

        public void WebDialogLoadBegin()
        {
            member_no = Request["member_no"].ToString().Trim();
            DwUtil.RetrieveDataWindow(DwMain, pbl, null, member_no);
        }

        public void CheckJsPostBack(string eventArg)
        {
        }

        public void WebDialogLoadEnd()
        {
        }
    }
}