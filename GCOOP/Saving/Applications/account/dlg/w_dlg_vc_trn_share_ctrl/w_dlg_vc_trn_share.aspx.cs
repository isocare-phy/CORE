using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

namespace Saving.Applications.account.dlg.w_dlg_vc_trn_share_ctrl
{
    public partial class w_dlg_vc_trn_share : PageWebDialog, WebDialog
    {
        protected String cashType;

        public void InitJsPostBack()
        {
            dsList.InitDsList(this);
            dsSum.InitDsSum(this);
        }

        public void WebDialogLoadBegin()
        {
            if (!IsPostBack)
            {
                JspostSlip();
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
        }

        public void WebDialogLoadEnd()
        {
        }

        private void JspostSlip()
        {
            String queryStrVcDate = "";
            try
            {
                queryStrVcDate = Request["vcDate"].Trim();
                cashType = Request["cashType"].Trim();
            }
            catch { }
            DateTime vcDate = DateTime.ParseExact(queryStrVcDate, "ddMMyyyy", new CultureInfo("th-TH"));
            if (cashType == "b_shrcv")
            {
                dsList.Retrieve(vcDate);
            }
            else if (cashType == "b_shrfilter")
            {
                dsList.RetrieveCash(vcDate);
            }

        }
    }
}