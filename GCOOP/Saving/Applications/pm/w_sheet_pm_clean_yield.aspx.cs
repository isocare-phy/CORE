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
//using CoreSavingLibrary.WcfPm;
using System.Web.Services.Protocols;
namespace Saving.Applications.pm
{
    public partial class w_sheet_pm_clean_yield : PageWebSheet, WebSheet
    {
        private String pbl = "pm_investment.pbl";
        public void InitJsPostBack()
        {
           
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                try
                {
                    DwUtil.RetrieveDataWindow(DwMain, pbl, null, null);
                    string test = DwMain.GetItemString(1, "symbol_code");
                }
                catch { DwMain.Reset(); }
            }
            else
            {
                this.RestoreContextDw(DwMain);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
           
        }

        public void SaveWebSheet()
        {
            try
            {
                DwUtil.UpdateDataWindow(DwMain, pbl, "pminvestmaster");
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");

            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("บันทึกไม่สำเร็จ");
            }
        }

        public void WebSheetLoadEnd()
        {
            try
            {
                DwUtil.RetrieveDDDW(DwMain, "investtype_code", pbl, null);
                DwUtil.RetrieveDDDW(DwMain, "invsource_code", pbl, null);
            }
            catch { }
            DwMain.SaveDataCache();
        }
    }
}