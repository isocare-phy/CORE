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
    public partial class w_sheet_bg_budgettype_year : PageWebSheet,WebSheet
    {

        protected String InsertRow;
        protected String DeleteRow;
        protected String InitDwList;
        protected String RetriveDDDW;
        protected String Refresh;
        private BudgetClient serBudget;

        #region WebSheet Members

        public void InitJsPostBack()
        {
            InsertRow = WebUtil.JsPostBack(this, "InsertRow");
            DeleteRow = WebUtil.JsPostBack(this, "DeleteRow");
            RetriveDDDW = WebUtil.JsPostBack(this, "RetriveDDDW");
            InitDwList = WebUtil.JsPostBack(this, "InitDwList");
            Refresh = WebUtil.JsPostBack(this, "Refresh");
        }

        public void WebSheetLoadBegin()
        {
            serBudget = wcf.Budget;
            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                DwUtil.RetrieveDDDW(DwMain, "year", "budget.pbl", null);
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwList);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "InsertRow")
            {
                DwList.InsertRow(0);
                Decimal year = DwMain.GetItemDecimal(1, "year");
                Decimal seqNo = DwMain.GetItemDecimal(1, "seq_no");
                DwList.SetItemDecimal(DwList.RowCount, "budgetyear", year);
                DwList.SetItemDecimal(DwList.RowCount, "seq_no", seqNo);
                //DwUtil.RetrieveDDDW(DwList, "budgettype_code_1", "budget.pbl", null);
            }
            else if (eventArg == "DeleteRow")
            {
                try
                {
                    int row = int.Parse(HdListRow.Value);
                    short year = Convert.ToInt16(DwMain.GetItemDecimal(1, "year"));
                    short seq_no = Convert.ToInt16(DwList.GetItemDecimal(row, "seq_no"));
                    short sort_order = Convert.ToInt16(DwList.GetItemDecimal(row, "sort_seq"));
                    short result = 0;// serBudget.of_delete_budgettype_year(state.SsWsPass, year, seq_no, sort_order);
                    if (result == 1) { LtServerMessage.Text = "ok"; }
                }
                catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
            }
            else if (eventArg == "RetriveDDDW")
            {
                Decimal year = DwMain.GetItemDecimal(1, "year");
                try
                {
                    object[] arg = new object[1] { year };
                    DwUtil.RetrieveDDDW(DwMain, "seq_no", "budget.pbl", arg);
                }
                catch
                {
                    DwMain.Reset();
                    DwMain.InsertRow(0);
                    DwMain.ClearDataCache();
                    DwMain.SetItemDecimal(1, "year", year);
                }
            }
            else if (eventArg == "InitDwList")
            {
                Decimal year = DwMain.GetItemDecimal(1, "year");
                Decimal seqNo = DwMain.GetItemDecimal(1, "seq_no");
                object[] arg = new object[2] { year, seqNo };
                try
                {
                    DwList.Reset();
                    DwUtil.RetrieveDataWindow(DwList, "budget.pbl", null, arg);
                    //DwUtil.RetrieveDDDW(DwList, "budgettype_code_1", "budget.pbl", null);
                }
                catch
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่มีข้อมูลปีงบประมาณ : " + (year + 543).ToString());
                }
            }
            else if (eventArg == "Refresh")
            {
            }
        }

        public void SaveWebSheet()
        {
            String xmlList = "";
            try
            {
                xmlList = DwList.Describe("DataWindow.Data.XML");
                serBudget.SaveBudgetTypeYear(state.SsWsPass, xmlList);
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเรียบร้อยแล้ว...");
                DwList.Reset();
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
        }

        #endregion
    }
}
