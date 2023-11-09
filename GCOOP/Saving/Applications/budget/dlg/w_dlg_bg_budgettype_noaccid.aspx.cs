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
using Sybase.DataWindow;
using EncryptDecryptEngine;

namespace Saving.Applications.budget.dlg
{
    public partial class w_dlg_bg_budgettype_noaccid : PageWebDialog, WebDialog
    {

        #region WebDialog Members

        public void InitJsPostBack()
        {
            
        }

        public void WebDialogLoadBegin()
        {
            this.ConnectSQLCA();
            DwMain.SetTransaction(sqlca);
            if (!IsPostBack)
            {
                //DwUtil.RetrieveDataWindow(DwMain, "budget_dlg.pbl", null, null);
                //DwMain.InsertRow(0);
                try
                {
                    string xml = Request["xmlAccId"];
                    Decryption dc = new Decryption();
                    string x = xml.Replace(" ", "+");
                    string xmlDc = dc.DecryptStrBase64(x);
                    DwMain.Retrieve();
                    //DwUtil.RetrieveDataWindow(DwMain, "budget_dlg.pbl", null, null);
                }
                catch
                {
                    DwMain.Retrieve();
                }
            }
            else
            {
                this.RestoreContextDw(DwMain);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
        }

        public void WebDialogLoadEnd()
        {
            DwMain.SaveDataCache();
        }

        #endregion
    }
}
