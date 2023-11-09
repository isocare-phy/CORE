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
    public partial class w_sheet_bg_movement : PageWebSheet,WebSheet
    {
        protected String initBgTypeList;
        protected String initBalance;
        protected String FilterBgCode;

        private BudgetClient serBudget;

        #region WebSheet Members

        public void InitJsPostBack()
        {
            initBgTypeList = WebUtil.JsPostBack(this, "initBgTypeList");
            FilterBgCode = WebUtil.JsPostBack(this, "FilterBgCode");
            initBalance = WebUtil.JsPostBack(this, "initBalance");
        }

        public void WebSheetLoadBegin()
        {
            serBudget = wcf.Budget;
            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                DwBalance.InsertRow(0);
                DwUtil.RetrieveDDDW(DwMain, "year", "budget.pbl", null);
                DwUtil.RetrieveDDDW(DwMain, "budgetgroup", "budget.pbl", null);
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwBalance);
                this.RestoreContextDw(DwList);
                this.RestoreContextDw(DwDetail);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "initBgTypeList")
            {
                Decimal seqNo = DwMain.GetItemDecimal(1, "seq_no");
                Decimal bgYear = DwMain.GetItemDecimal(1, "year");
                object[] argDw = new object[2] { bgYear, seqNo };
                DwList.Reset();
                try
                {
                    DwUtil.RetrieveDataWindow(DwList, "budget.pbl", null, argDw);
                }
                catch
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่มีข้อมูลปีงบประมาณ " + (bgYear + 543) + " หมวด " + seqNo.ToString());
                }
            }
            else if (eventArg == "FilterBgCode")
            {
                Decimal bgYear = DwMain.GetItemDecimal(1, "year");
                object[] argDw = new object[1] { bgYear};
                DwUtil.RetrieveDDDW(DwMain, "seq_no", "budget.pbl", argDw);
            }
            else if (eventArg == "initBalance")
            {
                int row = int.Parse(HdListRow.Value);
                Decimal bgYear = DwMain.GetItemDecimal(1, "year");
                String sql = "select beginning_of_budget, ending_of_budget from budgetyear where budgetyear = '" + bgYear + "'";
                DataTable dt = WebUtil.Query(sql);
                DateTime begin = DateTime.Parse(dt.Rows[0][0].ToString());
                DateTime end = DateTime.Parse(dt.Rows[0][1].ToString());
                Decimal seqNo = DwMain.GetItemDecimal(1, "seq_no");
                Decimal sortSeq = DwList.GetItemDecimal(row, "sort_seq");

                object[] arg = new object[5] { bgYear, begin, end, seqNo, sortSeq };
                DwBalance.Reset();
                try
                {
                    DwUtil.RetrieveDataWindow(DwBalance, "budget.pbl", null, arg);
                }
                catch
                {
                    DwBalance.InsertRow(0);
                }

                String xmlDwDetail = "";
                try
                {
                    sql = @"select budgetgroup_code, budgettype_code from budgettype_year where budgetyear ='" + bgYear + "' " +
                            "and seq_no ='" + seqNo + "' and sort_seq ='" + sortSeq + "'";
                    DataTable dt2 = WebUtil.Query(sql);
                    String bgGrp = dt2.Rows[0][0].ToString();
                    String bgType = dt2.Rows[0][1].ToString();
                    short year = short.Parse(bgYear.ToString());
                    DwDetail.Reset();
                    xmlDwDetail = serBudget.GetBgMovmentYear(state.SsWsPass, year, bgGrp, bgType);
                    if (xmlDwDetail != null && xmlDwDetail != "")
                    {
                        //DwDetail.ImportString(xmlDwDetail, FileSaveAsType.Xml);
                        DwUtil.ImportData(xmlDwDetail, DwDetail, null, FileSaveAsType.Xml);
                        DwDetail.Sort();
                        DwDetail.CalculateGroups();
                    }
                    else
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("ไม่มีข้อมูล");
                    }
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
            DwBalance.SaveDataCache();
            DwList.SaveDataCache();
            DwDetail.SaveDataCache();
        }

        #endregion
    }
}
