using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary.WcfNPm;
using Sybase.DataWindow;
using DataLibrary;

namespace Saving.Applications.pm
{
    public partial class w_sheet_pm_investment_account : PageWebSheet, WebSheet
    {
        private String pbl = "pm_investment.pbl";
        private n_pmClient PmSevice;
        TimeSpan tp;
        protected String InterateInsert;
        protected String DelDetailRow;
        protected String DetailInsert;
        protected String DelInterateRow;
        protected String postNodueFlag;
        protected String postMainBank;
        protected String postDetailBank;
        protected String JsPostTime;
        protected String JsPostDueDate;
        protected String PostUnitCost;
        protected String PostUnitAmt;
        protected String JsSetInvestType;
        protected String JsSetInvSource;
        private DwThDate tDwMain;
        private DwThDate tDwInterate;
        protected String JsChangeFollow;
        protected String jsGetGroupMoneyType;
        protected String jsSetBankBranch;

        public void InitJsPostBack()
        {
            InterateInsert = WebUtil.JsPostBack(this, "InterateInsert");
            postNodueFlag = WebUtil.JsPostBack(this, "postNodueFlag");
            DetailInsert = WebUtil.JsPostBack(this, "DetailInsert");
            JsSetInvestType = WebUtil.JsPostBack(this, "JsSetInvestType");
            JsSetInvSource = WebUtil.JsPostBack(this, "JsSetInvSource");
            PostUnitCost = WebUtil.JsPostBack(this, "PostUnitCost");
            PostUnitAmt = WebUtil.JsPostBack(this, "PostUnitAmt");
            JsPostDueDate = WebUtil.JsPostBack(this, "JsPostDueDate");
            JsPostTime = WebUtil.JsPostBack(this, "JsPostTime");
            DelDetailRow = WebUtil.JsPostBack(this, "DelDetailRow");
            DelInterateRow = WebUtil.JsPostBack(this, "DelInterateRow");
            postMainBank = WebUtil.JsPostBack(this, "postMainBank");
            postDetailBank = WebUtil.JsPostBack(this, "postDetailBank");
            JsChangeFollow = WebUtil.JsPostBack(this, "JsChangeFollow");
            jsSetBankBranch = WebUtil.JsPostBack(this, "jsSetBankBranch");
            jsGetGroupMoneyType = WebUtil.JsPostBack(this, "jsGetGroupMoneyType");

            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("open_date", "open_tdate");
            tDwMain.Add("due_date", "due_tdate");
            tDwMain.Add("buy_date", "buy_tdate");
            tDwMain.Add("push_date", "push_tdate");
            tDwMain.Add("call_date", "call_tdate");

            tDwInterate = new DwThDate(DwInterate, this);
            tDwInterate.Add("int_start_date", "int_start_tdate");
            tDwInterate.Add("int_end_date", "int_end_tdate");
        }
        public void WebSheetLoadBegin()
        {
            PmSevice = wcf.NPm;
            if (!IsPostBack)
            {
                string coop_name = "สหกรณ์ออมทรัพย์มหาวิทยาลัยมหิดล จำกัด";
                HdFocus.Value = "NotPostBack";
                DwMain.Reset();
                DwDetail.Reset();
                DwInterate.Reset();

                DwMain.InsertRow(0);
                DwDetail.InsertRow(0);
                DwInterate.InsertRow(0);
                DwMain.SetItemDateTime(1, "buy_date", state.SsWorkDate);
                DwMain.SetItemDateTime(1, "open_date", state.SsWorkDate);
                DwMain.SetItemDecimal(1, "invest_period_unit", 2);
                MainPostBank();
                try
                {
                    string sqlcoop_name = "select coop_name from cmcoopconstant";
                    Sdt cn = WebUtil.QuerySdt(sqlcoop_name);
                    if (cn.Next())
                    {
                        coop_name = cn.GetString("coop_name");
                        DwMain.SetItemString(1, "account_name", coop_name);
                    }
                    else
                    {
                        DwMain.SetItemString(1, "account_name", coop_name);
                    }
                }
                catch { DwMain.SetItemString(1, "account_name", coop_name); }
            }
            else
            {
                this.RestoreContextDw(DwMain, tDwMain);
                this.RestoreContextDw(DwDetail);
                this.RestoreContextDw(DwInterate, tDwInterate);
            }
        }
        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "DelDetailRow":
                    DwDetail.DeleteRow(Convert.ToInt32(HdDetailRowDel.Value));
                    HdDetailRowDel.Value = "";
                    break;
                case "DelInterateRow":
                    DwInterate.DeleteRow(Convert.ToInt32(HdInterateRowDel.Value));
                    HdInterateRowDel.Value = "";
                    break;
                case "postMainBank":
                    JsDwMainPostBank();
                    break;
                case "postDetailBank":
                    JsDwDetailPostBank();
                    break;
                case "JsPostTime":
                    CalculateTime();
                    break;
                case "JsPostDueDate":
                    CalculateTime();
                    break;
                case "PostUnitAmt":
                    ChangeUnitAmt();
                    break;
                case "PostUnitCost":
                    ChangeUnitCost();
                    break;
                case "JsSetInvestType":
                    SetInvestType();
                    break;
                case "JsSetInvSource":
                    SetInvSource();
                    break;
                case "DetailInsert":
                    DetailInsertRow();
                    break;
                case "InterateInsert":
                    InterateInsertRow();
                    break;
                case "postNodueFlag":
                    JsNodueFlag();
                    break;
                /////////////////////
                case "JsChangeFollow":
                    JsChangeBuyDate();
                    break;
                /////////////////////
                case "jsSetBankBranch":
                    SetBankBranch();
                    break;
                case "jsGetGroupMoneyType":
                    GetGroupMoneyType();
                    break;
            }
        }
        public void JsNodueFlag()
        {
            DateTime duedate = state.SsWorkDate;
            decimal flag = DwMain.GetItemDecimal(1, "nodue_flag");

            if (flag == 1)
            {
                duedate = DateTime.ParseExact("31129456", "ddMMyyyy", null);
                DwMain.SetItemDateTime(1, "due_date", duedate);
            }
            else
            {
                DwMain.SetItemDateTime(1, "due_date", duedate);
            }


        }
        public void DetailInsertRow()
        {
            DwDetail.InsertRow(0);
            HdFocus.Value = "money_code";
        }
        public void InterateInsertRow()
        {
            DwInterate.InsertRow(0);
            HdFocus.Value = "int_start_tdate";
        }
        public void MainPostBank()
        {

            try
            {
                String bankcode = DwMain.GetItemString(1, "bank_code");
                DwUtil.RetrieveDDDW(DwMain, "bank_branch", pbl, null);
                DataWindowChild dc = DwMain.GetChild("bank_branch");
                dc.SetFilter("bank_code ='" + bankcode + "'");
                dc.Filter();
                DwMain.SetItemString(1, "bank_branch", "0827");
            }
            catch { }
        }
        public void JsDwMainPostBank()
        {
            
            try
            {
                String bankcode = DwMain.GetItemString(1,"bank_code");
                DwUtil.RetrieveDDDW(DwMain, "bank_branch", pbl, null);
                DataWindowChild dc = DwMain.GetChild("bank_branch");
                dc.SetFilter("bank_code ='" + bankcode + "'");
                dc.Filter();
                DwMain.SetItemString(1, "bank_branch", "");
            }
            catch { }
        }
        public void JsDwDetailPostBank()
        {
            try
            {
                String bankcode = HdBankCode.Value;
                DwUtil.RetrieveDDDW(DwDetail, "bank_branch", pbl, null);
                DataWindowChild dc = DwDetail.GetChild("bank_branch");
                dc.SetFilter("bank_code ='" + bankcode + "'");
                dc.Filter();
            }
            catch { }
            //try
            //{
            //    //string bank_code = DwMain.GetItemString(Convert.ToInt32(HdRow.Value), "bank_code");
            //    DwUtil.RetrieveDDDW(DwMain, "bank_branch", pbl, HdBankCode.Value);
            //}
            //catch { }
            DwDetail.SetItemString(Convert.ToInt32(HdRow.Value), "bank_branch", "");
        }
        public void SetInvestType()
        {
            string investtype_code = HdInvestType.Value;
            try
            {
                DwUtil.RetrieveDDDW(DwMain, "investtype_code", pbl, null);
                DwMain.SetItemString(1, "investtype_code", investtype_code);
            }
            catch { }

        }
        public void SetInvSource()
        {
            string invsource_code = HdInvSource.Value;
            try
            {
                DwUtil.RetrieveDDDW(DwMain, "invsource_code", pbl, null);
                DwMain.SetItemString(1, "invsource_code", invsource_code);
            }
            catch { }

        }
        public void ChangeUnitAmt()
        {
            decimal maturity_price = 0;
            decimal unit_amt = 0;
            decimal unit_cost = 0;
            try
            {
                unit_amt = DwMain.GetItemDecimal(1, "unit_amt");
            }
            catch { }
            try
            {
                maturity_price = DwMain.GetItemDecimal(1, "maturity_price");
            }
            catch { }
            if (maturity_price <= 0)
            {
                LtServerMessage.Text = WebUtil.WarningMessage("กรุณาตรวจสอบ ราคาหน้าตั๋ว");
            }
            else
            {
                if (unit_amt > maturity_price)
                {
                    LtServerMessage.Text = WebUtil.WarningMessage("จำนวนหน่วยลงทุน มากกว่า ราคาหน้าตั๋ว");
                }
                else
                {
                    unit_cost = maturity_price / unit_amt;
                    DwMain.SetItemDecimal(1, "unit_cost", unit_cost);
                }
            }
        }
        public void ChangeUnitCost()
        {
            decimal maturity_price = 0;
            decimal unit_amt = 0;
            decimal unit_cost = 0;
            try
            {
                unit_cost = DwMain.GetItemDecimal(1, "unit_cost");
            }
            catch { }
            try
            {
                maturity_price = DwMain.GetItemDecimal(1, "maturity_price");
            }
            catch { }
            if (maturity_price <= 0)
            {
                LtServerMessage.Text = WebUtil.WarningMessage("กรุณาตรวจสอบ ราคาหน้าตั๋ว");
            }
            else
            {
                if (unit_cost > maturity_price)
                {
                    LtServerMessage.Text = WebUtil.WarningMessage("ราคาต่อหน่วย มากกว่า ราคาหน้าตั๋ว");
                }
                else
                {
                    unit_amt = maturity_price / unit_cost;
                    if ((unit_amt % 10) > 0)
                    {
                        LtServerMessage.Text = WebUtil.WarningMessage("จำนวนหน่วยลงทุนต้องเป็นจำนวนเต็ม");
                    }
                    else
                    {
                        DwMain.SetItemDecimal(1, "unit_amt", unit_amt);
                    }
                }
            }
        }
        public void SaveWebSheet()
        {
            try
            {
                for (int i = 1; i <= DwInterate.RowCount; i++)
                {
                    try
                    {
                        string tint_start_date = DwMain.GetItemString(i, "int_start_tdate");
                        if (tint_start_date.Length == 8)
                        {
                            DateTime dt = DateTime.ParseExact(tint_start_date, "ddMMyyyy", WebUtil.TH);
                            DwMain.SetItemDateTime(i, "int_start_date", dt);
                        }
                    }
                    catch { }

                    try
                    {
                        string tint_end_date = DwMain.GetItemString(i, "int_end_tdate");
                        if (tint_end_date.Length == 8)
                        {
                            DateTime dt = DateTime.ParseExact(tint_end_date, "ddMMyyyy", WebUtil.TH);
                            DwMain.SetItemDateTime(i, "int_end_date", dt);
                        }
                    }
                    catch { }
                }
                DwMain.SetItemString(1, "coop_id", state.SsCoopId);
                String xml_main = DwMain.Describe("DataWindow.Data.XML");
                String xml_detail = DwDetail.Describe("DataWindow.Data.XML");
                String xml_interate = DwInterate.Describe("DataWindow.Data.XML");
                int resu = PmSevice.of_savepmreq(state.SsWsPass, xml_main, xml_detail, xml_interate);
                if (resu == 1)
                {
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                    DwMain.Reset();
                    DwDetail.Reset();
                    DwInterate.Reset();
                    DwMain.InsertRow(0);
                    DwDetail.InsertRow(0);
                    DwInterate.InsertRow(0);
                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด บันทึกไม่สำเร็จ");
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }
        public void WebSheetLoadEnd()
        {
            DwUtil.RetrieveDDDW(DwMain, "accid_prnc", pbl, null);
            DwUtil.RetrieveDDDW(DwMain, "investtype_code", pbl, null);
            DwUtil.RetrieveDDDW(DwMain, "invsource_code", pbl, null);
            DwUtil.RetrieveDDDW(DwMain, "bank_code", pbl, null);
            try
            {
                DwUtil.RetrieveDDDW(DwMain, "accid_int", pbl, null);
            }
            catch { }

            //DwMain.SetItemString(1, "bank_branch", "");
            //try
            //{
            //    string bank_code = DwMain.GetItemString(Convert.ToInt32(HdRow.Value), "bank_code");
            //    DwUtil.RetrieveDDDW(DwMain, "bank_branch", pbl, bank_code);
            //}
            //catch { }

            DwUtil.RetrieveDDDW(DwDetail, "money_code", pbl, null);
            DwUtil.RetrieveDDDW(DwDetail, "account_id", pbl, null);
            DwUtil.RetrieveDDDW(DwDetail, "bank_code", pbl, null);

            //DwDetail.SetItemString(1, "bank_branch", "");
            //try
            //{
            //    string bank_code = DwDetail.GetItemString(1, "bank_code");
            //    DwUtil.RetrieveDDDW(DwDetail, "bank_branch", pbl, null);
            //}
            //catch { }


            DwMain.SaveDataCache();
            DwDetail.SaveDataCache();
            DwInterate.SaveDataCache();
            tDwMain.Eng2ThaiAllRow();
            tDwInterate.Eng2ThaiAllRow();
        }
        public void JsChangeBuyDate()
        {
            DateTime buy_date = DwMain.GetItemDateTime(1, "buy_date");
            DwMain.SetItemDateTime(1, "open_date", buy_date);
            // Set textbox1 as textbox2
        }
        public void CalculateTime()
        {
            int status = 0;
            DateTime start_date = DateTime.Now;
            DateTime end_date = DateTime.Now;
            decimal invest_period_unit = 1;
            decimal invest_period_unit_again = 1;
            try
            {
                invest_period_unit = DwMain.GetItemDecimal(1, "invest_period_unit");
            }
            catch
            {
                DwMain.SetItemDecimal(1, "invest_period_unit", 1);
                invest_period_unit = 1;
            }
            try
            {
                start_date = DwMain.GetItemDateTime(1, "open_date");
                status = 1;
            }
            catch
            {
                status = 0;
                LtServerMessage.Text = WebUtil.WarningMessage("กรุณากรอกวันที่หน้าตั๋ว");
            }

            if (status == 1)
            {
                try
                {
                    end_date = DwMain.GetItemDateTime(1, "due_date");
                    status = 1;
                }
                catch
                {
                    status = 0;
                    LtServerMessage.Text = WebUtil.WarningMessage("กรุณากรอกวันที่จ่าย");
                }
            }
            //-------------------------------------------------------------------

            int day_start = Convert.ToInt32(start_date.ToString("dd"));
            int month_start = Convert.ToInt32(start_date.ToString("MM"));
            int year_start = Convert.ToInt32(start_date.ToString("yyyy"));
            int day_end = Convert.ToInt32(end_date.ToString("dd"));
            int month_end = Convert.ToInt32(end_date.ToString("MM"));
            int year_end = Convert.ToInt32(end_date.ToString("yyyy"));

            if (day_start != day_end)
            {
                tp = end_date - start_date;
                double countday = tp.TotalDays;
                DwMain.SetItemDecimal(1, "investment_period", Convert.ToDecimal(countday));
                DwMain.SetItemDecimal(1, "invest_period_unit", 1);
                invest_period_unit_again = 1;
            }
            else if (month_start != month_end && day_start == day_end)
            {
                int year = 0;
                int month = 0;
                year = year_end - year_start;
                month = month_end - month_start;


                double sum = (year * 12) + month;
                DwMain.SetItemDecimal(1, "investment_period", Convert.ToDecimal(sum));
                DwMain.SetItemDecimal(1, "invest_period_unit", 2);
                invest_period_unit_again = 2;
            }
            else
            {
                int year = 0;
                year = year_end - year_start;
                DwMain.SetItemDecimal(1, "investment_period", Convert.ToDecimal(year));
                DwMain.SetItemDecimal(1, "invest_period_unit", 3);
                invest_period_unit_again = 3;
            }


        }
        public void SetBankBranch()
        {
            try
            {
                int sheetRow = Convert.ToInt32(HdSheetRow.Value);
                DwDetail.SetItemString(sheetRow, "bank_code", HdBank_id.Value);
                DwDetail.SetItemString(sheetRow, "bank_branch", HdBranch_id.Value);

                DwDetail.SetItemString(sheetRow, "bank_desc", HdBank_desc.Value.Trim() + " " + HdBranch_desc.Value.Trim());
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                return;
            }
            HdSheetRow.Value = "";
            HdBank_id.Value = "";
            HdBank_desc.Value = "";
            HdBranch_id.Value = "";
            HdBranch_desc.Value = "";
        }
        public void GetGroupMoneyType()
        {
            try
            {
                string money_type = DwDetail.GetItemString(Convert.ToInt32(HdRow_Type.Value), "money_code");
                DataWindowChild dc = DwDetail.GetChild("money_code");
                int rGroup = dc.FindRow("moneytype_code='" + money_type + "'", 1, dc.RowCount);
                string moneytype_group = DwUtil.GetString(dc, rGroup, "moneytype_group", "");
                DwDetail.SetItemString(Convert.ToInt32(HdRow_Type.Value), "moneytype_group", moneytype_group);
                HdRow_Type.Value = "";
            }
            catch
            {
                HdRow_Type.Value = "";
            }
        }

    }
}