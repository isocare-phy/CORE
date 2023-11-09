using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sybase.DataWindow;
using CoreSavingLibrary.WcfNAdmin;
using DataLibrary;

namespace Saving.Applications.pm.dlg
{
    public partial class w_dlg_account_list : PageWebDialog, WebDialog
    {
        protected string postCriteria;

        string pbl = "pm_investment.pbl";
        public void InitJsPostBack()
        {
            postCriteria = WebUtil.JsPostBack(this, "postCriteria");
        }

        public void WebDialogLoadBegin()
        {
            if (!IsPostBack)
            {
                DwCriteria.InsertRow(0);
                //DwUtil.RetrieveDataWindow(DwMain,pbl,null,null);
            }
            else
            {
                DwCriteria.Reset();
                //DwCriteria.InsertRow(0);
                DwCriteria.RestoreContext();
                DwMain.RestoreContext();
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
                DwUtil.RetrieveDDDW(DwMain, "invsource_code", pbl, null);
                DwUtil.RetrieveDDDW(DwMain, "investtype_code", pbl, null);
                DwUtil.RetrieveDDDW(DwCriteria , "invsource_code", pbl, null);
                DwUtil.RetrieveDDDW(DwCriteria, "investtype_code", pbl, null);
            }
            catch { }
            DwCriteria.SaveDataCache();
            DwMain.SaveDataCache();
        }

        public void Criteria()
        {
            string invsource_code="", investtype_code="", symbol_code="";
            try
            {
                try
                {
                    invsource_code = DwCriteria.GetItemString(1, "invsource_code");
                    invsource_code = "%" + invsource_code + "%";
                }
                catch { invsource_code = "%%"; }
                try
                {
                    investtype_code = DwCriteria.GetItemString(1, "investtype_code");
                    investtype_code = "%" + investtype_code + "%";
                }
                catch { investtype_code = "%%"; }
                try
                {
                    symbol_code = DwCriteria.GetItemString(1, "symbol_code");
                    symbol_code = "%" + symbol_code + "%";
                }
                catch { symbol_code = "%%"; }

                DwUtil.RetrieveDataWindow(DwMain, pbl, null, invsource_code, investtype_code, symbol_code);
            }
            catch 
            { 
            }
        }
    }
}