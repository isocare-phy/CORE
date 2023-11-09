using System;
using CoreSavingLibrary;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using CoreSavingLibrary.WcfBudget;

namespace Saving.Applications.budget
{
    public partial class w_sheet_bg_budgetclosemonth : PageWebSheet,WebSheet
    {
        protected String CloseMonthProcess;
        private BudgetClient serBudget;
        #region WebSheet Members

        public void InitJsPostBack()
        {
            CloseMonthProcess = WebUtil.JsPostBack(this, "CloseMonthProcess");
        }

        public void WebSheetLoadBegin()
        {
            serBudget = wcf.Budget;
            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
            }
            else
            {
                this.RestoreContextDw(DwMain);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "CloseMonthProcess")
            {
                try
                {
                    Decimal year = DwMain.GetItemDecimal(1, "year") - 543;
                    Decimal month = DwMain.GetItemDecimal(1, "month");
                    serBudget.CloseMonth(state.SsWsPass, short.Parse(year.ToString()), short.Parse(month.ToString()));
                    LtServerMessage.Text = WebUtil.CompleteMessage("ปิดงานสิ้นเดือนเรียบร้อยแล้ว");
                    DwMain.Reset();
                    DwMain.InsertRow(0);
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            }
        }

        public void SaveWebSheet()
        {
        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
        }

        #endregion
    }
}
