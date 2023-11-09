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
using DataLibrary;
using Sybase.DataWindow;
using System.Web.Services.Protocols;

namespace Saving.Applications.app_assist.dlg
{
    public partial class w_dlg_kt_memberdead_list : PageWebDialog, WebDialog
    {
        #region WebDialog Members

        public void InitJsPostBack()
        {

        }

        public void WebDialogLoadBegin()
        {
            this.ConnectSQLCA();

            if (!IsPostBack)
            {
                DwMain.SetTransaction(sqlca);
                //DwMain.Retrieve();
                DwUtil.RetrieveDataWindow(DwMain, "kt_50bath.pbl", null, "2553");
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
