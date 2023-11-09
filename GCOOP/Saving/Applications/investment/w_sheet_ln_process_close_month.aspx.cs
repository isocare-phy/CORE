using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary.WcfNInvestment;

namespace Saving.Applications.investment
{
    public partial class w_sheet_ln_process_close_month : PageWebSheet, WebSheet
    {
        public void InitJsPostBack()
        {
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
            }
            else
            {
                DwMain.RestoreContext();
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
        }

        public void SaveWebSheet()
        {
            try
            {

                DwMain.SetItemString(1, "coop_id", state.SsCoopControl);
                DwMain.SetItemString(1, "entry_id", state.SsUsername);
                DwMain.SetItemString(1, "entryby_coopid", state.SsCoopId);

                if (DwMain.RowCount > 1)
                {
                    DwMain.DeleteRow(2);
                }

                string xml = DwMain.Describe("Datawindow.Data.XML");
                str_lcproccls lcp = new str_lcproccls();
                lcp.xml_option = xml;
                wcf.NInvestment.of_lccls_mth_opt(state.SsWsPass, lcp);
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลสำเร็จ");
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
        }
    }
}