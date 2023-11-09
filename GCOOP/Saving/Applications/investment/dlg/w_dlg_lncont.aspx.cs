using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sybase.DataWindow;
using DataLibrary;
using CoreSavingLibrary.WcfInvestment;

namespace Saving.Applications.investment.dlg
{
    public partial class w_dlg_lncont : PageWebDialog, WebDialog
    {
        private InvestmentClient InvestmentService;

        #region Websheet Members
        public void InitJsPostBack()
        {

        }

        public void WebDialogLoadBegin()
        {
            InvestmentService = wcf.Investment;
            if (!IsPostBack)
            {
                str_lclnrcvlist lclnrcvlist = new str_lclnrcvlist();
                try
                {
                    lclnrcvlist.memcoop_id = state.SsCoopId;
                    lclnrcvlist.member_no = Request["memno"];
                }
                catch { }
                try
                {
                    short result = InvestmentService.of_getmemb_lnrcv(state.SsWsPass, ref lclnrcvlist);
                    if (result > 1)
                    {
                        DwMain.ImportString(lclnrcvlist.xml_list, Sybase.DataWindow.FileSaveAsType.Xml);
                    }
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
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