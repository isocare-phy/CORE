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
    public partial class w_sheet_bg_cancelslip : PageWebSheet, WebSheet
    {
        protected String RetrieveList;
        protected String RetrieveDetail;
        private DwThDate tDwMain;
        private BudgetClient sevBudget;

        #region WebSheet Members

        public void InitJsPostBack()
        {
            RetrieveList = WebUtil.JsPostBack(this, "RetrieveList");
            RetrieveDetail = WebUtil.JsPostBack(this, "RetrieveDetail");
            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("operate_date", "operate_tdate");
        }

        public void WebSheetLoadBegin()
        {
            sevBudget = wcf.Budget;
            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                DwMain.SetItemDateTime(1, "operate_date", state.SsWorkDate);
                tDwMain.Eng2ThaiAllRow();
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwList);
                this.RestoreContextDw(DwDetail);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "RetrieveList")
            {
                DateTime operateDate = DwMain.GetItemDateTime(1, "operate_date");
                object[] arg = new object[1] { operateDate };
                DwList.Reset();
                DwDetail.Reset();
                DwUtil.RetrieveDataWindow(DwList, "budget.pbl", null, arg);
            }
            else if (eventArg == "RetrieveDetail")
            {
                object[] arg = new object[1] { HdSlipNo.Value };
                DwDetail.Reset();
                try
                {
                    DwUtil.RetrieveDataWindow(DwDetail, "budget.pbl", null, arg);
                }
                catch (Exception)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่มีข้อมูลเลขที่ใบสำคัญ : " + HdSlipNo.Value);
                }
            }
        }

        public void SaveWebSheet()
        {
            String xmlList = "";
            try
            {
                xmlList = DwList.Describe("DataWindow.Data.XML");
                int result = 0;// sevBudget.SaveCancelslip(state.SsWsPass, xmlList);
                if (result == 1)
                {
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเรียบร้อยแล้ว");
                }
                DwList.Reset();
                DwDetail.Reset();
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
            DwList.SaveDataCache();
            DwDetail.SaveDataCache();
        }

        #endregion
    }
}
