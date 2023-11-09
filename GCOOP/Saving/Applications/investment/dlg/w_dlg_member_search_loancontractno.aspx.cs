using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sybase.DataWindow;
using DataLibrary;

namespace Saving.Applications.investment.dlg
{
    public partial class w_dlg_member_search_loancontractno : PageWebDialog,WebDialog
    {
       protected string postCriteria;
        string pbl = "loan.pbl";

        public void InitJsPostBack()
        {
            postCriteria = WebUtil.JsPostBack(this, "postCriteria");
        }

        public void WebDialogLoadBegin()
        {
            if (!IsPostBack)
            {
                DwCriteria.InsertRow(0);
                DwCriteria.SetItemString(1, "memb_name", "");
                DwCriteria.SetItemString(1, "prename_code", "ฮฮ");
                DwCriteria.SetItemString(1, "member_no", "");

            }
            else
            {
                this.RestoreContextDw(DwCriteria);
                this.RestoreContextDw(DwMain);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postCriteria")
            {
                Criteria();
            }
        }

        public void WebDialogLoadEnd()
        {
            try
            {
                DwUtil.RetrieveDDDW(DwCriteria, "prename_code", pbl, null);
            }
            catch { }
            DwCriteria.SaveDataCache();
            DwMain.SaveDataCache();
        }
        public void Criteria()
        {
            //string prename_code = "";
            //string memb_name = "";
            //try
            //{
            //    prename_code = DwCriteria.GetItemString(1, "prename_code");
            //    if (prename_code == "ฮฮ")
            //    {
            //        prename_code = "";
            //    }
            //    prename_code += "%";
            //    memb_name = DwCriteria.GetItemString(1, "memb_name") + "%";
            //    DwUtil.RetrieveDataWindow(DwMain, pbl, null, prename_code, memb_name);
            //}
            //catch { }
            string prename_code = "%";
            string memb_name = "%";
            string memb_no = "";

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
                memb_no = DwCriteria.GetItemString(1, "member_no");
            }
            catch { }
            try
            {
                DwUtil.RetrieveDataWindow(DwMain, pbl, null, prename_code, memb_name, memb_no);
            }
            catch { }
        }
    
    }
}