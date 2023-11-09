using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;

namespace Saving.Applications.cmd.dlg
{
    public partial class w_dlg_cmd_ptmetlmastlot : PageWebDialog, WebDialog
    {
        private string invt_id;
        protected string pbl = "cmd_ptinvtmast.pbl";

        public void InitJsPostBack()
        {
        }

        public void WebDialogLoadBegin()
        {
            this.ConnectSQLCA();
            if (IsPostBack)
            {

                try
                {
                    Dw_detail.Reset();
                    Dw_detail.RestoreContext();

                }
                catch { }

            }
            else
            {
                try
                {
                    invt_id = Request["invt_id"];
                }
                catch { }
                Dw_detail.InsertRow(0);
                DwUtil.RetrieveDataWindow(Dw_detail, pbl, null, invt_id);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
        }

        public void WebDialogLoadEnd()
        {
            Dw_detail.SaveDataCache();
        }
    }
}