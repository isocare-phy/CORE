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
using Sybase.DataWindow;

namespace Saving.Applications.budget
{
    public partial class w_sheet_bg_budgetpaydetial : PageWebSheet, WebSheet
    {
        protected String initPayDetail;
        private DwThDate tDwMain;

        private BudgetClient serBudget;

        #region WebSheet Members

        public void InitJsPostBack()
        {
            initPayDetail = WebUtil.JsPostBack(this, "initPayDetail");
            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("operate_date", "operate_tdate");
        }

        public void WebSheetLoadBegin()
        {
            serBudget = wcf.Budget;
            this.ConnectSQLCA();
            DwDetail.SetTransaction(sqlca);

            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                DwMain.SetItemDateTime(1, "operate_date", state.SsWorkDate);
                tDwMain.Eng2ThaiAllRow();
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwDetail);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "initPayDetail")
            {
                DateTime date = DwMain.GetItemDateTime(1, "operate_date");
                int year = serBudget.GetYearBudget(state.SsWsPass, date);
                DwDetail.Retrieve(date, year);
                //object[] arg = new object[2] { date, year };
                //DwUtil.RetrieveDataWindow(DwDetail, "budget.pbl", null, arg);
            }
        }

        public void SaveWebSheet()
        {
            String xmlDetail = "";
            try
            {
                xmlDetail = DwDetail.Describe("DataWindow.Data.XML");
                serBudget.SaveFromEditPay(state.SsWsPass, xmlDetail);
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเรียบร้อยแล้ว");
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
            DwDetail.SaveDataCache();
        }

        #endregion
    }
}
