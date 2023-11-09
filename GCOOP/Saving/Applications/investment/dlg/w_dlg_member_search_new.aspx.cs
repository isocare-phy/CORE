using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sybase.DataWindow;
//using Saving.WsAdmin;
using DataLibrary;

namespace Saving.Applications.investment.dlg
{
    public partial class w_dlg_member_search_new : PageWebDialog, WebDialog
    {
        protected string jsPostSearch;
        string pbl = "loan.pbl";

        public void InitJsPostBack()
        {
            jsPostSearch = WebUtil.JsPostBack(this, "jsPostSearch");
        }

        public void WebDialogLoadBegin()
        {
            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
            }
            else
            {

                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwDetail);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "jsPostSearch")
            {
                Search();
            }
        }

        public void WebDialogLoadEnd()
        {
            DwMain.SaveDataCache();
            DwDetail.SaveDataCache();
        }

        private void Search()
        {
            string prename_code = "";
            string memb_name = "%";

            try
            {
                prename_code = DwMain.GetItemString(1, "prename_code");
                //prename_code += "%";
                if (prename_code == "Z0" || prename_code == "ฮฮ")
                {
                    prename_code = "%";
                }
            }
            catch
            {
                prename_code += "%";
            }
            try
            {
                memb_name += DwMain.GetItemString(1, "memb_name");
                memb_name += "%";
            }
            catch
            {

            }
            try
            {
                DwUtil.RetrieveDataWindow(DwDetail, pbl, null, prename_code, memb_name);
            }
            catch { }
        }
    }
}