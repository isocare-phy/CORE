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

namespace Saving.Applications.budget.dlg
{
    public partial class w_dlg_bg_budgettype : PageWebDialog,WebDialog
    {
        #region WebDialog Members

        public void InitJsPostBack()
        {
        }

        public void WebDialogLoadBegin()
        {
            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                DwUtil.RetrieveDDDW(DwMain, "budgetgroup_code", "budget_dlg.pbl", null);
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwList);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
        }

        public void WebDialogLoadEnd()
        {
            DwMain.SaveDataCache();
            DwList.SaveDataCache();
        }

        #endregion

        protected void B_Search_Click(object sender, EventArgs e)
        {
            String bgGrpCode = "";
            try
            {
                bgGrpCode = DwMain.GetItemString(1, "budgetgroup_code");
            }
            catch
            {
                bgGrpCode = "";
            }
            if (bgGrpCode != "" && bgGrpCode != null)
            {
                object[] arg = new object[1] { bgGrpCode };
                DwUtil.RetrieveDataWindow(DwList, "budget_dlg.pbl", null, arg);
                DwUtil.RetrieveDDDW(DwList, "account_id", "budget_dlg.pbl", null);
            }
        }
    }
}
