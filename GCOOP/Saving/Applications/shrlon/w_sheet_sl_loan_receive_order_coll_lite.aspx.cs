using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Saving.Applications.shrlon
{
    public partial class w_sheet_sl_loan_receive_order_coll_lite : PageWebSheet, WebSheet
    {
        public void InitJsPostBack()
        {
            dsList.InitDsList(this);
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                try
                {
                    String xmlList = wcf.Shrlon.InitListLnRcv(state.SsWsPass, state.SsCoopId);
                    dsList.ImportData(xmlList);
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
        }

        public void SaveWebSheet()
        {
        }

        public void WebSheetLoadEnd()
        {
        }
    }
}