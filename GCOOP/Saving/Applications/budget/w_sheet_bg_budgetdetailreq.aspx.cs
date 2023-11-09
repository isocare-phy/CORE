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
using Saving.CmPage;
using CoreSavingLibrary.WcfBudget;
namespace Saving.Applications.budget
{
    public partial class w_sheet_bg_budgetdetailreq : PageWebSheet,WebSheet
    {
        protected String InsertRow;
        protected String DeleteRow;
        protected String RetrieveList;
        protected String Refresh;
        protected String RetrieveBgType;
        protected String FilterBgGrp;
        protected String CheckBgAmt;

        private BudgetClient serBudget;

        #region WebSheet Members

        public void InitJsPostBack()
        {
            InsertRow = WebUtil.JsPostBack(this, "InsertRow");
            DeleteRow = WebUtil.JsPostBack(this, "DeleteRow");
            RetrieveList = WebUtil.JsPostBack(this, "RetrieveList");
            Refresh = WebUtil.JsPostBack(this, "Refresh");
            RetrieveBgType = WebUtil.JsPostBack(this, "RetrieveBgType");
            CheckBgAmt = WebUtil.JsPostBack(this, "CheckBgAmt");
            FilterBgGrp = WebUtil.JsPostBack(this, "FilterBgGrp");
        }

        public void WebSheetLoadBegin()
        {
            serBudget = wcf.Budget;
            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                DwAmt.InsertRow(0);
                DwUtil.RetrieveDDDW(DwMain, "budgetyear", "budget.pbl", null);
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwList);
                this.RestoreContextDw(DwAmt);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "InsertRow")
            {
                Decimal bgYear = DwMain.GetItemDecimal(1, "budgetyear"); ;
                Decimal seqNo = DwMain.GetItemDecimal(1, "budgetgroup_code");
                Decimal sortSeq = DwMain.GetItemDecimal(1, "budgettype_code");
                DwList.InsertRow(0);
                DwList.SetItemDecimal(DwList.RowCount, "budgetyear", bgYear);
                DwList.SetItemDecimal(DwList.RowCount, "seq_no", seqNo);
                DwList.SetItemDecimal(DwList.RowCount, "sort_seq", sortSeq);
                if (DwList.RowCount > 1)
                {
                    String bgControl = DwList.GetItemString(DwList.RowCount - 1, "budgetcontrol");
                    DwList.SetItemString(DwList.RowCount, "budgetcontrol", bgControl);
                }
            }
            else if (eventArg == "DeleteRow")
            {
                int row = int.Parse(HdListRow.Value);
                DwList.DeleteRow(row);
            }
            else if (eventArg == "RetrieveList")
            {
                Decimal bgYear = DwMain.GetItemDecimal(1, "budgetyear"); ;
                Decimal bgGrpCode = DwMain.GetItemDecimal(1, "budgetgroup_code");
                Decimal bgTypeCode = DwMain.GetItemDecimal(1, "budgettype_code");
                object[] argMain = new object[3] { bgYear, bgGrpCode, bgTypeCode };
                String errText = "";
                try
                {
                    DwAmt.Reset();
                    DwUtil.RetrieveDataWindow(DwAmt, "budget.pbl", null, argMain);
                }
                catch (Exception)
                {
                    errText += "ไม่มีงบประมาณขอตั้ง ";
                    DwAmt.InsertRow(0);
                }
                try
                {
                    DwList.Reset();
                    DwUtil.RetrieveDataWindow(DwList, "budget.pbl", null, argMain);
                }
                catch(Exception)
                {
                    errText += " ไม่มีข้อมูลปี : " + bgYear + " ประเภท : " + bgTypeCode;
                }
                if (errText != "")
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(errText);
                }
            }
            else if (eventArg == "RetrieveBgType")
            {
                Decimal year = DwMain.GetItemDecimal(1, "budgetyear"); 
                Decimal bgGrpCode = DwMain.GetItemDecimal(1, "budgetgroup_code");
                object[] argDDDW = new object[2] { year, bgGrpCode };
                DwUtil.RetrieveDDDW(DwMain, "budgettype_code", "budget.pbl", argDDDW);
            }
            else if (eventArg == "CheckBgAmt")
            {
                HdBgAmt.Value = "false";
                Decimal bgAmt = DwAmt.GetItemDecimal(1, "setbudget_amt");
                int row = DwList.RowCount;
                Decimal allAmt = 0;
                if (row > 0)
                {
                    Decimal amt = 0;
                    Decimal flag = 0;
                    for (int i = 1; i <= row; i++)
                    {
                        try
                        {
                            amt = DwList.GetItemDecimal(i, "budgetdetail_amt");
                        }
                        catch
                        {
                            amt = 0;
                        }
                        flag = DwList.GetItemDecimal(i, "budgetdetail_flag");
                        allAmt += amt * flag;
                    }
                }
                if (bgAmt >= allAmt)
                {
                    HdBgAmt.Value = "true";
                }
                else
                {
                    HdBgAmt.Value = "false";
                }
            }
            else if (eventArg == "FilterBgGrp")
            {
                Decimal year = DwMain.GetItemDecimal(1, "budgetyear");
                object[] arg = new object[1] { year };
                DwUtil.RetrieveDDDW(DwMain, "budgetgroup_code", "budget.pbl", arg);
            }
        }

        public void SaveWebSheet()
        {
            String xmlList = "";
            try
            {
                xmlList = DwList.Describe("DataWindow.Data.XML");
                serBudget.SaveBudgetDetail(state.SsWsPass, xmlList);
                DwList.Reset();
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเรียบร้อยแล้ว...");
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
            DwAmt.SaveDataCache();
        }

        #endregion
    }
}
