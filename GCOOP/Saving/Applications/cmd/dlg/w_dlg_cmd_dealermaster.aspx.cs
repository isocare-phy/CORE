using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using Sybase.DataWindow;


namespace Saving.Applications.cmd.dlg
{
    public partial class w_dlg_cmd_dealermaster : PageWebDialog, WebDialog
    {
        string pbl = "dlg_search.pbl";
        protected string jsPostSearch;

        public void InitJsPostBack()
        {
            jsPostSearch = WebUtil.JsPostBack(this, "jsPostSearch");
        }

        public void WebDialogLoadBegin()
        {
            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                DwUtil.RetrieveDataWindow(DwDetail, pbl, null, "%", "%", "%");
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwDetail);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "jsPostSearch":
                    PostSearch();
                    break;
            }
        }

        public void WebDialogLoadEnd()
        {
            DwMain.SaveDataCache();
            DwDetail.SaveDataCache();
        }

        private void PostSearch()
        {
            string dealer_no = "", dealer_taxid = "", dealer_name = "";
            try
            {
                try { dealer_no = "%" + DwMain.GetItemString(1, "dealer_no") + "%"; }
                catch { dealer_no = "%"; }
                try { dealer_taxid = "%" + DwMain.GetItemString(1, "dealer_taxid") + "%"; }
                catch { dealer_taxid = "%"; }
                try { dealer_name = "%" + DwMain.GetItemString(1, "dealer_name") + "%"; }
                catch { dealer_name = "%"; }
                DwUtil.RetrieveDataWindow(DwDetail, pbl, null, dealer_no, dealer_taxid, dealer_name);
            }
            catch { }
            
        }
    }
}