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
using CoreSavingLibrary.WcfNDeposit;

namespace Saving.Applications.app_assist.dlg
{
    public partial class w_dlg_kt_addapv_task : PageWebDialog, WebDialog
    {
        private n_depositClient depService;
        protected String postCheckApv;

        private void JsPostCheckApv()
        {
            String processId = Request["processId"];
            try
            {
                string result = depService.of_check_approved(state.SsWsPass, processId, state.SsCoopId);
                if (processId != "")
                {
                    HdValueCheckApv.Value = processId;
                    HdNameApv.Value = state.SsCoopId;
                    HdCheckApv.Value = "true";
                }
                else
                {
                    HdCheckApv.Value = "false";
                }
            }
            catch (Exception)
            {

            }
        }

        #region WebDialog Members

        public void InitJsPostBack()
        {
            postCheckApv = WebUtil.JsPostBack(this, "postCheckApv");
        }

        public void WebDialogLoadBegin()
        {
            HdCheckApv.Value = "";
            HdDlgClose.Value = "";
            try
            {
                depService = wcf.Deposit;
            }
            catch
            {

            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postCheckApv")
            {
                JsPostCheckApv();
            }
        }

        public void WebDialogLoadEnd()
        {
        }

        #endregion

        protected void BtnCancle_Click(object sender, EventArgs e)
        {
            String processId = Request["processId"];
            String avpCode = Request["avpCode"];
            String itemType = Request["itemType"];
            decimal avpAmt = Convert.ToDecimal(Request["avpAmt"]);
            //depService.ApvPermiss(state.SsWsPass, processId, "0", avpAmt, state.SsClientIp, state.SsUsername, state.SsWorkDate, avpCode, itemType, state.SsCoopId);
            HdDlgClose.Value = "true";
        }
    }
}
