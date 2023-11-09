using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataLibrary;
using Sybase.DataWindow;

namespace Saving.Applications.investment
{
    public partial class w_sheet_ln_ucf_loanstatement : PageWebSheet, WebSheet
    {
        public String pbl = "ln_ucf_all.pbl";
        public void InitJsPostBack()
        {
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                DwUtil.RetrieveDataWindow(DwMain, pbl, null, null);
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
                SaveInfo();
                DwUtil.RetrieveDataWindow(DwMain, pbl, null, null);
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage("บันทึกไม่สำเร็จเนื่องจาก : " + ex); }

        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
        }
        public void SaveInfo()
        {
            DwUtil.UpdateDataWindow(DwMain, pbl, "LCUCFLOANITEMTYPE");
        }
    }
}