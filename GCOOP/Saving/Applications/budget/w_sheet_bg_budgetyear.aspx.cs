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
using DataLibrary;
using Sybase.DataWindow;

namespace Saving.Applications.budget
{
    public partial class w_sheet_bg_budgetyear : PageWebSheet, WebSheet
    {
        protected String InsertRow;
        protected String DeleteRow;
        protected String SetStartEndDate;
        Sta ta;
        private DwThDate tDwMain;

        private BudgetClient serBudget;

        #region WebSheet Members

        public void InitJsPostBack()
        {
            InsertRow = WebUtil.JsPostBack(this, "InsertRow");
            DeleteRow = WebUtil.JsPostBack(this, "DeleteRow");
            SetStartEndDate = WebUtil.JsPostBack(this, "SetStartEndDate");
            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("beginning_of_budget", "beginning_of_tbudget");
            tDwMain.Add("ending_of_budget", "ending_of_tbudget");
        }

        public void WebSheetLoadBegin()
        {
            ta = new Sta(state.SsConnectionString);
            this.ConnectSQLCA();

            serBudget = wcf.Budget;
            if (!IsPostBack)
            {
                NewClear();
            }
            else
            {
                this.RestoreContextDw(DwMain);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "InsertRow")
            {
                DwMain.InsertRow(0);
            }
            else if (eventArg == "DeleteRow")
            {
                Delete_Row();
                //int row = int.Parse(HdMainRow.Value);
                //DwMain.DeleteRow(row);
                //DwMain.SetTransaction(sqlca);
                //DwMain.UpdateData();

            }
            else if (eventArg == "SetStartEndDate")
            {
                int row = int.Parse(HdMainRow.Value);
                Decimal year = DwMain.GetItemDecimal(row, "budgetyear") - 543;
                DateTime startDate = new DateTime(int.Parse(year.ToString()), 1, 1);
                DateTime endDate = new DateTime(int.Parse(year.ToString()), 12, 31);
                DwMain.SetItemDateTime(row, "beginning_of_budget", startDate);
                DwMain.SetItemDateTime(row, "ending_of_budget", endDate);
                tDwMain.Eng2ThaiAllRow();
                DwMain.SelectRow(row, true);
            }
        }

        public void SaveWebSheet()
        {
            if (HdcolumnName.Value == "budget_status")
            {

                int row = int.Parse(HdMainRow.Value);
                for (int i = 1; i <= DwMain.RowCount; i++)
                {
                    Decimal year = DwMain.GetItemDecimal(i, "budgetyear") - 543;
                    DwMain.SetItemDecimal(i, "budgetyear", year);
                    String sql = @"DELETE FROM budgetgroup_year WHERE budgetyear =" + year + @"";
                    ta.Exe(sql);
                }

                //DwMain.SetTransaction(sqlca);
                //DwMain.UpdateData();
                NewClear();
            }
            else
            {
                String xmlMain = "";
                for (int i = 1; i <= DwMain.RowCount; i++)
                {
                    Decimal year = DwMain.GetItemDecimal(i, "budgetyear") - 543;
                    DwMain.SetItemDecimal(i, "budgetyear", year);
                }
                try
                {
                    xmlMain = DwMain.Describe("DataWindow.Data.XML");
                    serBudget.SaveBudgetYear(state.SsWsPass, xmlMain);
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเรียบร้อยแล้ว...");
                    NewClear();
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            }
        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
        }

        #endregion

        private void Delete_Row()
        {
            //int row = int.Parse(HdMainRow.Value);
            int year = int.Parse(Hdyear.Value) - 543;
            try
            {
                String sql = @"DELETE FROM budgetgroup_year WHERE budgetyear =" + year + @"";
                ta.Exe(sql);
                sql = @"DELETE FROM budgetyear WHERE budgetyear =" + year + @"";
                ta.Exe(sql);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }
            NewClear();
        }

        private void NewClear()
        {
            DwMain.Reset();
            DwUtil.RetrieveDataWindow(DwMain, "budget.pbl", tDwMain, null);
            for (int i = 1; i <= DwMain.RowCount; i++)
            {
                Decimal year = DwMain.GetItemDecimal(i, "budgetyear") + 543;
                DwMain.SetItemDecimal(i, "budgetyear", year);
            }
        }
    }
}
