using System;
using CoreSavingLibrary;
using System.Data;
using System.Linq;
using System.Collections;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using DataLibrary;
using CoreSavingLibrary.WcfBudget;
using Sybase.DataWindow;
using System.Web.Services.Protocols;

namespace Saving.Applications.budget
{
    public partial class w_sheet_bg_budgetadjust : PageWebSheet, WebSheet
    {
        private BudgetClient serBudget;
        protected DwThDate tDwMain;
        protected String FilterBgCode;
        protected String FilterSortSeq;
        protected String FilterToSortSeq;



        public void InitJsPostBack()
        {
            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("entry_date", "entry_tdate");
            //tDwMain.Add("entry_time", "entry_ttime");
            tDwMain.Add("operate_date", "operate_tdate");

            FilterBgCode = WebUtil.JsPostBack(this, "FilterBgCode");
            FilterSortSeq = WebUtil.JsPostBack(this, "FilterSortSeq");
            FilterToSortSeq = WebUtil.JsPostBack(this, "FilterToSortSeq");
        }

        public void WebSheetLoadBegin()
        {
            serBudget = wcf.Budget;
            if (!IsPostBack)
            {
                InitDwMain();
            }
            else
            {
                this.RestoreContextDw(DwMain);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "FilterBgCode")
            {
                Decimal year = DwMain.GetItemDecimal(1, "budgetyear");
                object[] arg = new object[1] { year };
                DataWindowChild DcSeq = DwMain.GetChild("seq_no");
                DcSeq.Reset();
                DataWindowChild DcToSeq = DwMain.GetChild("to_seq_no");
                DcToSeq.Reset();
                DataWindowChild DcSortSeq = DwMain.GetChild("sort_seq");
                DcSortSeq.Reset();
                DataWindowChild DcToSortSeq = DwMain.GetChild("to_sort_seq");
                DcToSortSeq.Reset();
                DwUtil.RetrieveDDDW(DwMain, "seq_no", "budget.pbl", arg);
                DwUtil.RetrieveDDDW(DwMain, "to_seq_no", "budget.pbl", arg);
            }
            else if (eventArg == "FilterSortSeq")
            {
                Decimal year = DwMain.GetItemDecimal(1, "budgetyear");
                Decimal seqNo = DwMain.GetItemDecimal(1, "seq_no");
                object[] arg = new object[2] { year, seqNo };
                DataWindowChild DcSortSeq = DwMain.GetChild("sort_seq");
                DcSortSeq.Reset();
                DwUtil.RetrieveDDDW(DwMain, "sort_seq", "budget.pbl", arg);
            }
            else if (eventArg == "FilterToSortSeq")
            {
                Decimal year = DwMain.GetItemDecimal(1, "budgetyear");
                Decimal seqNo = DwMain.GetItemDecimal(1, "to_seq_no");
                object[] arg = new object[2] { year, seqNo };
                DataWindowChild DcToSortSeq = DwMain.GetChild("to_sort_seq");
                DcToSortSeq.Reset();
                DwUtil.RetrieveDDDW(DwMain, "to_sort_seq", "budget.pbl", arg);
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                String mainXml = DwMain.Describe("DataWindow.Data.XML");
                //serBudget.SaveBudgetTransfer(state.SsWsPass, mainXml);
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเรียบร้อยแล้ว");
                DwMain.Reset();
                InitDwMain();
            }
            catch(Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
        }



        private void InitDwMain()
        {
            DwMain.InsertRow(0);

            DwMain.SetItemString(1, "entry_id", state.SsUsername);
            DwMain.SetItemDateTime(1, "entry_time", DateTime.Now);
            DwMain.SetItemDateTime(1, "entry_date", state.SsWorkDate);
            DwMain.SetItemDateTime(1, "operate_date", state.SsWorkDate);
            DwUtil.RetrieveDDDW(DwMain, "budgetyear", "budget.pbl", null);

            tDwMain.Eng2ThaiAllRow();
        }
    }
}
