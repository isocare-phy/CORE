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
    public partial class w_sheet_bg_slip : PageWebSheet, WebSheet
    {
        protected String InsertRow;
        protected String DeleteRow;
        protected String RetrieveBgType;
        protected String SetAmt;
        protected String RetrieveAccId;
        protected String MemberName;

        private DwThDate tDwMain;
        private BudgetClient serBudget;


        public void InitJsPostBack()
        {
            InsertRow = WebUtil.JsPostBack(this, "InsertRow");
            DeleteRow = WebUtil.JsPostBack(this, "DeleteRow");
            RetrieveBgType = WebUtil.JsPostBack(this, "RetrieveBgType");
            SetAmt = WebUtil.JsPostBack(this, "SetAmt");
            RetrieveAccId = WebUtil.JsPostBack(this, "RetrieveAccId");
            MemberName = WebUtil.JsPostBack(this, "MemberName");

            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("entry_date", "entry_tdate");
            tDwMain.Add("operate_date", "operate_tdate");
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();
            serBudget = wcf.Budget;
            if (!IsPostBack)
            {
                NewClear();
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



                string budgetgroup_code = DwList.GetItemString(DwList.RowCount, "budgetgroup_code");
                string budgettype_code = DwList.GetItemString(DwList.RowCount, "budgettype_code");
                string account_id = DwList.GetItemString(DwList.RowCount, "account_id");


                DwList.InsertRow(0);
                DwUtil.RetrieveDDDW(DwList, "budgetgroup_code", "budget.pbl", null);
                DwUtil.RetrieveDDDW(DwList, "account_id", "budget.pbl", null);
                DwList.SetItemDecimal(DwList.RowCount, "slipitem_status", 8);
                DwList.SetItemString(DwList.RowCount, "budgetgroup_code", budgetgroup_code);
                DwList.SetItemString(DwList.RowCount, "budgettype_code", budgettype_code);
                DwList.SetItemString(DwList.RowCount, "account_id", account_id);

            }
            else if (eventArg == "DeleteRow")
            {
                int row = int.Parse(HdListRow.Value);
                DwList.DeleteRow(row);
                SetAllAmt();
            }
            else if (eventArg == "RetrieveBgType")
            {
                int row = int.Parse(HdListRow.Value);


                DwList.SetItemString(row, "budgettype_code", "");
                DwList.SetItemString(row, "account_id", "");
                try
                {
                    String bgGrpCode = DwList.GetItemString(row, "budgetgroup_code");
                    object[] argDDDw = new object[1] { bgGrpCode };
                    DwUtil.RetrieveDDDW(DwList, "budgettype_code", "budget.pbl", argDDDw);
                    //DataWindowChild budgettype = DwList.GetChild("budgettype_code");
                    //budgettype.SetTransaction(sqlca);
                    //budgettype.Retrieve();
                    //String group_code = DwList.GetItemString(row, "budgetgroup_code");
                    //budgettype.SetFilter("budgetgroup_code='" + group_code + "'");
                    //budgettype.Filter();
                }
                catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
            }
            else if (eventArg == "SetAmt")
            {
                SetAllAmt();
            }
            else if (eventArg == "RetrieveAccId")
            {
                int row = int.Parse(HdListRow.Value);
                String bgTypeCode = DwList.GetItemString(row, "budgettype_code");
                String sqlText = @"select account_id from budgettype 
                                   where budgettype_code = '" + bgTypeCode + "'";
                DataTable dt = WebUtil.Query(sqlText);
                if (DwList.RowCount > 0)
                {
                    String accId = dt.Rows[0][0].ToString().Trim();
                    DwList.SetItemString(row, "account_id", accId);
                }
                else
                {
                    throw new Exception("ประเภท : " + bgTypeCode + "ไม่พบรหัสคู่บัญชี ");
                }
            }
            else if (eventArg == "MemberName")
            {
                String membNo = DwMain.GetItemString(1, "member_no");
                String membName = "";
                DwMain.SetItemString(1, "pay_towhom", "");
                try
                {
                    membName = serBudget.GetMemberName(state.SsWsPass, membNo);
                    DwMain.SetItemString(1, "pay_towhom", membName);
                }
                catch
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบ ชื่อ-นามสกุล ของรหัสสมาชิก  : " + membNo);
                }
            }
        }

        public void SaveWebSheet()
        {
            String xmlMain = "";
            String xmlList = "";
            try
            {
                xmlMain = DwMain.Describe("DataWindow.Data.XML");
                xmlList = DwList.Describe("DataWindow.Data.XML");
                serBudget.SaveSlip(state.SsWsPass, xmlMain, xmlList);
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเรียบร้อยแล้ว...");
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }

            NewClear();
        }

        public void WebSheetLoadEnd()
        {

            try
            {
                int row = int.Parse(HdListRow.Value);
                String bgGrpCode = DwList.GetItemString(row, "budgetgroup_code");
                object[] argDDDw = new object[1] { bgGrpCode };
                DwUtil.RetrieveDDDW(DwList, "budgettype_code", "budget.pbl", argDDDw);
                DwUtil.RetrieveDDDW(DwList, "budgetgroup_code", "budget.pbl", null);
                DwUtil.RetrieveDDDW(DwList, "account_id", "budget.pbl", null);
            }
            catch
            {
                DwUtil.RetrieveDDDW(DwList, "budgetgroup_code", "budget.pbl", null);
                //DwUtil.RetrieveDDDW(DwList, "budgettype_code", "budget.pbl", null);
                DwUtil.RetrieveDDDW(DwList, "account_id", "budget.pbl", null);
            }
            DwMain.SaveDataCache();
            DwList.SaveDataCache();
        }



        private void NewClear()
        {
            DwMain.Reset();
            DwMain.InsertRow(0);
            DwUtil.RetrieveDDDW(DwMain, "account_id_1", "budget.pbl", null);
            DwUtil.RetrieveDDDW(DwMain, "cash_type", "budget.pbl", null);
            DwMain.SetItemDateTime(1, "entry_date", state.SsWorkDate);
            DwMain.SetItemDateTime(1, "operate_date", state.SsWorkDate);
            DwMain.SetItemString(1, "entry_id", state.SsUsername);
            DwMain.SetItemString(1, "coopbranch_id", state.SsCoopId);
            DwList.Reset();
            DwList.InsertRow(0);
            tDwMain.Eng2ThaiAllRow();
        }

        private void SetAllAmt()
        {
            Decimal totalAmt = 0;
            for (int i = 1; i <= DwList.RowCount; i++)
            {
                totalAmt += DwList.GetItemDecimal(i, "itempay_amt");
            }
            DwMain.SetItemDecimal(1, "itempay_amt", totalAmt);
        }
    }
}
