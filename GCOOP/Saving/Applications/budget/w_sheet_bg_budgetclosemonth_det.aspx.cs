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
    public partial class w_sheet_bg_budgetclosemonth_det : PageWebSheet,WebSheet
    {
        protected String FilterBgGrp;
        protected String InitBgMonthDet;
        protected String InitBgDetailAndBal;
        protected String InsertRow;
        protected String DeleteRow;
        protected String Refresh;

        private BudgetClient serBudget;

        #region WebSheet Members

        public void InitJsPostBack()
        {
            FilterBgGrp = WebUtil.JsPostBack(this, "FilterBgGrp");
            InitBgMonthDet = WebUtil.JsPostBack(this, "InitBgMonthDet");
            InitBgDetailAndBal = WebUtil.JsPostBack(this, "InitBgDetailAndBal");
            InsertRow = WebUtil.JsPostBack(this, "InsertRow");
            DeleteRow = WebUtil.JsPostBack(this, "DeleteRow");
            Refresh = WebUtil.JsPostBack(this, "Refresh");
        }

        public void WebSheetLoadBegin()
        {
            serBudget = wcf.Budget;
            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                DwBalance.InsertRow(0);
                DwUtil.RetrieveDDDW(DwMain, "budgetyear", "budget.pbl", null);
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwList);
                this.RestoreContextDw(DwBalance);
                this.RestoreContextDw(DwDetail);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {

            if (eventArg == "FilterBgGrp")
            {
                Decimal year = DwMain.GetItemDecimal(1, "budgetyear");
                object[] arg = new object[1] { year };
                DwUtil.RetrieveDDDW(DwMain, "seq_no", "budget.pbl", arg);
            }
            else if (eventArg == "InitBgMonthDet")
            {
                Decimal year = DwMain.GetItemDecimal(1, "budgetyear");
                Decimal month = DwMain.GetItemDecimal(1, "budgetmonth");
                Decimal seqNo = DwMain.GetItemDecimal(1, "seq_no");
                object[] arg = new object[3] { year, month, seqNo };
                DwList.Reset();
                try
                {
                    DwUtil.RetrieveDataWindow(DwList, "budget.pbl", null, arg);
                }
                catch
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่มีข้อมูล");
                }
            }
            else if (eventArg == "InsertRow")
            {
                DwDetail.InsertRow(0);
                int row = DwDetail.RowCount;
                Decimal year = DwMain.GetItemDecimal(1, "budgetyear");
                Decimal month = DwMain.GetItemDecimal(1, "budgetmonth");
                Decimal seqNo = DwMain.GetItemDecimal(1, "seq_no");
                Decimal sortSeq = DwList.GetItemDecimal(1, "sort_seq");
                DwDetail.SetItemDecimal(row, "budgetyear", year);
                DwDetail.SetItemDecimal(row, "budgetmonth", month);
                DwDetail.SetItemDecimal(row, "seq_no", seqNo);
                DwDetail.SetItemDecimal(row, "sort_seq", sortSeq);
            }
            else if (eventArg == "DeleteRow")
            {
                int row = int.Parse(HdDetailRow.Value);
                DwDetail.DeleteRow(row);
            }
            else if (eventArg == "InitBgDetailAndBal")
            {
                int row = int.Parse(HdListRow.Value);
                Decimal year = DwMain.GetItemDecimal(1, "budgetyear");
                Decimal month = DwMain.GetItemDecimal(1, "budgetmonth");
                Decimal seqNo = DwMain.GetItemDecimal(1, "seq_no");
                Decimal sortSeq = DwList.GetItemDecimal(row, "sort_seq");
                object[] arg = new object[4] { year, month, seqNo, sortSeq };
                DwBalance.Reset();
                DwDetail.Reset();
                try
                {
                    DwUtil.RetrieveDataWindow(DwBalance, "budget.pbl", null, arg);
                }
                catch
                {
                    DwBalance.InsertRow(0);
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่มีข้อมูล");
                }
                try
                {
                    DwUtil.RetrieveDataWindow(DwDetail, "budget.pbl", null, arg);
                }
                catch
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่มีข้อมูล");
                }

            }
            
        }

        public void SaveWebSheet()
        {
            String xmlDetail = "";
            Int16 sortSeq=0;
            try
            {
                int row = int.Parse(HdListRow.Value);
                sortSeq = Convert.ToInt16(DwList.GetItemDecimal(row, "sort_seq"));
                int row_d = DwDetail.RowCount;
                for (int r = 1; r <= row_d; r++)
                {
                    DwDetail.SetItemDecimal(r, "sort_seq", Convert.ToDecimal(sortSeq));
                }
                xmlDetail = DwDetail.Describe("DataWindow.Data.XML");
                //serBudget.SaveCloseMonthDetail(state.SsWsPass, xmlDetail, sortSeq);
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเรียบร้อยแล้ว");
                DwDetail.Reset();
                DwBalance.Reset();
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
            DwBalance.SaveDataCache();
            DwDetail.SaveDataCache();
        }

        #endregion
    }
}
