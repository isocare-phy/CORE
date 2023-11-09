using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Saving.Applications.app_assist.dlg
{
    public partial class w_dlg_as_senior_assist_paylist : PageWebDialog, WebDialog
    {
        private String member_no;
        private String pbl = "as_capital.pbl";

        protected DwThDate tDwMain;

        public void InitJsPostBack()
        {
            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("pay_date", "pay_tdate");
        }

        public void WebDialogLoadBegin()
        {
            member_no = Request["member_no"].ToString().Trim();
            DwUtil.RetrieveDataWindow(DwMain, pbl, null, member_no);

            tDwMain.Eng2ThaiAllRow();
        }

        public void CheckJsPostBack(string eventArg)
        {
        }

        public void WebDialogLoadEnd()
        {
            DwMain.SaveDataCache();
            tDwMain.Eng2ThaiAllRow();
        }
    }
}