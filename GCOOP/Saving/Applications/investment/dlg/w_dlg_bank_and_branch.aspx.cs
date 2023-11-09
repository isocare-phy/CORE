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
using CoreSavingLibrary.WcfCommon;

namespace Saving.Applications.investment.dlg
{
    public partial class w_dlg_bank_and_branch : PageWebDialog, WebDialog
    {
        protected String postSelectBank;

        #region WebDialog Members

        public void InitJsPostBack()
        {
            postSelectBank = WebUtil.JsPostBack(this, "postSelectBank");
        }

        public void WebDialogLoadBegin()
        {
            if (!IsPostBack)
            {
                DwUtil.RetrieveDataWindow(DwBank, "sh_slippayout.pbl", null, null);
                try
                {
                    HdSheetRow.Value = Request["sheetRow"].ToString().Trim();
                }
                catch { HdSheetRow.Value = ""; }
            }
            else
            {
                this.RestoreContextDw(DwBank);
                this.RestoreContextDw(DwBranch);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postSelectBank")
            {
                JsPostSelectBank();
            }
        }

        public void WebDialogLoadEnd()
        {
            DwBank.SaveDataCache();
            DwBranch.SaveDataCache();
        }

        private void JsPostSelectBank()
        {
            int r = Convert.ToInt32(HdBankRow.Value);
            string bank_code = "";
            try
            {
                bank_code = DwBank.GetItemString(r, "bank_code");
                DwUtil.RetrieveDataWindow(DwBranch, "sh_slippayout.pbl", null, bank_code, "%%");
            }
            catch
            {
            }
        }
        #endregion
        protected void Button1_Click(object sender, EventArgs e)
        {
            String branch_name = "";
            string bank_code = "";
            try
            {
                int r = Convert.ToInt32(HdBankRow.Value);
                branch_name = TextBox1.Text;
                bank_code = DwBank.GetItemString(r, "bank_code");
                DwUtil.RetrieveDataWindow(DwBranch, "sh_slippayout.pbl", null, bank_code, "%" + branch_name + "%");
            }
            catch
            {
            }
        }
    }
}