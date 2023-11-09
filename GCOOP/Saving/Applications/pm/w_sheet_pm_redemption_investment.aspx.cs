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
    public partial class w_sheet_pm_redemption_investment : PageWebSheet, WebSheet
    {
        private String pbl = "pm_investment.pbl";
        private n_pmClient PmService;
        protected String postAccountNo;
        protected String postTdate;
        protected String DelSlipRow;
        protected String postBank;
        protected String jsGetGroupMoneyType;
        protected String jsSetBankBranch;
        private DwThDate tDwMain;

        public void InitJsPostBack()
        {
            postAccountNo = WebUtil.JsPostBack(this, "postAccountNo");
            DelSlipRow = WebUtil.JsPostBack(this, "DelSlipRow");
            postBank = WebUtil.JsPostBack(this, "postBank");
            jsGetGroupMoneyType = WebUtil.JsPostBack(this, "jsGetGroupMoneyType");
            jsSetBankBranch = WebUtil.JsPostBack(this, "jsSetBankBranch");
             postTdate = WebUtil.JsPostBack(this, "postTdate");
            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("operate_date", "operate_tdate");
        }

        public void WebSheetLoadBegin()
        {
            PmService = wcf.NPm;
            if (!IsPostBack)
            {
                DwMain.Reset();
                DwSlipdet.Reset();

                DwMain.InsertRow(0);
                DwSlipdet.InsertRow(0);
                //postTdate = DateTime.ParseExact(postTdate, "ddMMyyyy", null);
                //DwMain.SetItemDateTime(1, "operate_date", postTdate);
            }
            else
            {
                this.RestoreContextDw(DwMain,tDwMain);
                this.RestoreContextDw(DwSlipdet);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "postAccountNo":
                    InitData();
                    break;
                case "DelSlipRow":
                    DwSlipdet.DeleteRow(Convert.ToInt32(HdSlipRowDel.Value));
                    HdSlipRowDel.Value = "";
                    break;
                case "postBank":
                    JsBankCHange();
                    break;
                case "jsGetGroupMoneyType":
                    GetGroupMoneyType();
                    break;
                case "jsSetBankBranch":
                    SetBankBranch();
                    break;
                case "postTdate":
                     tdateChange();
                     break;
            }
        }

        public void SaveWebSheet()
        {
            try
            {
               
                    String xml_main = DwMain.Describe("DataWindow.Data.XML");
                    String xml_slip = DwSlipdet.Describe("DataWindow.Data.XML");
                    int result = PmService.of_savewithdraw(state.SsWsPass, xml_main, xml_slip);
                    if (result == 1)
                    {
                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                        DwMain.Reset();
                        DwSlipdet.Reset();
                        DwMain.InsertRow(0);
                        DwSlipdet.InsertRow(0);
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
            tDwMain.Eng2ThaiAllRow();

            DwUtil.RetrieveDDDW(DwMain, "accid_prnc", pbl, null);
            DwUtil.RetrieveDDDW(DwMain, "investtype_code", pbl, null);
            DwUtil.RetrieveDDDW(DwMain, "invsource_code", pbl, null);
            DwUtil.RetrieveDDDW(DwSlipdet, "money_code", pbl, null);
            DwUtil.RetrieveDDDW(DwSlipdet, "account_id", pbl, null);
            DwUtil.RetrieveDDDW(DwSlipdet, "bank_code", pbl, null);

            //DwSlipdet.SetItemString(1, "bank_branch", "");
            //try
            //{
            //    string bank_code = DwSlipdet.GetItemString(1, "bank_code");
            //    DwUtil.RetrieveDDDW(DwSlipdet, "bank_branch", pbl, bank_code);
            //}
            //catch { }
            //DwUtil.RetrieveDDDW(DwSlipdet, "bank_branch", pbl, null);

            DwMain.SaveDataCache();
            DwSlipdet.SaveDataCache();
        }

        private void InitData()
        {
            try
            {
                //  int li_chk;
                // try
                // {
                //   DwMain.SetItemDateTime(1, "operate_date", postTdate);
                //    li_chk = 1;
                //}
                // catch
                // {
                //     li_chk = 0;
                // }
                //DateTime operate_date = DateTime.Today;
                //DwMain.GetItemDateTime(1, "operate_date");
                string account_no = DwMain.GetItemString(1, "account_no");
                account_no = int.Parse(account_no).ToString("0000000000");
                //DateTime postTdates = postTdate;
                String xml_main = DwMain.Describe("DataWindow.Data.XML");
                String result = PmService.of_setslipmain_withdraw(state.SsWsPass, xml_main, account_no = int.Parse(account_no).ToString("0000000000"));
                if (result != "")
                {
                    DwMain.Reset();
                    DwMain.ImportString(result, FileSaveAsType.Xml);
                }
                string  req_tdate, due_tdate;
                string sql_pmmaster = @"SELECT distinct	m.COOP_ID						,		m.INVEST_PERIOD_UNIT		,		m.INVSOURCE_CODE			,		m.INVESTTYPE_CODE		,
			                                m.BRANCH_ID					,		m.OPEN_DATE						,		m.DUE_DATE					,		m.ACCOUNT_NAME			,
			                                m.INT_PRESENT_RATE		, 		m.TAX_RATE						,		m.PRNCBAL						,		m.LASTCALINT_DATE 		,		
			                                m.ACCUINT_AMT				,		m.INTARREAR_AMT				,		m.ACCUINTRCV_AMT		,		m.ACCUTAXPAY_AMT 		, 		
			                                m.START_INTDATE			,		m.UNIT_AMT						,		m.UNIT_COST					,		m.ACCID_PRNC				,
			                                m.PURCHASE_PRICE			,		m.SYMBOL_CODE					,		m.RATE_CODE					,		m.INVESTMENT_DOCNO		,		
			                                m.DAY_INYEAR				,		m.INVESTMENT_PERIOD         ,       d.account_balance
                                    FROM	pminvestmaster m, pminvestduedate d
                                  WHERE	m.account_no = '"+account_no+@"'
                                       AND	m.coop_id = '"+state.SsCoopId+@"'
                                       AND m.account_no = d.account_no 
                                       AND d.account_balance = (select min(account_balance) from pminvestduedate where account_no =  '"+account_no+@"')";
                Sta ta = new Sta(new DwTrans().ConnectionString);
                Sdt dt = ta.Query(sql_pmmaster);
                if (dt.Next())
                {
                    DwMain.SetItemString(1, "COOP_ID", dt.GetString("COOP_ID"));
                    DwMain.SetItemString(1, "account_name", dt.GetString("ACCOUNT_NAME"));
                    DwMain.SetItemString(1, "investment_docno", dt.GetString("INVESTMENT_DOCNO"));
                    req_tdate = dt.GetDate("OPEN_DATE").ToString("ddMM") + Convert.ToString(Convert.ToDecimal(dt.GetDate("OPEN_DATE").ToString("yyyy")) + 543);
                    due_tdate = dt.GetDate("DUE_DATE").ToString("ddMM") +Convert.ToString(Convert.ToDecimal(dt.GetDate("DUE_DATE").ToString("yyyy")) + 543);
                    DwMain.SetItemString(1, "req_tdate", req_tdate);
                    DwMain.SetItemString(1, "due_tdate", due_tdate);
                    DwMain.SetItemDate(1, "operate_date", state.SsWorkDate);
                    DwMain.SetItemString(1, "accid_prnc", dt.GetString("ACCID_PRNC"));
                    DwMain.SetItemString(1, "symbol_code", dt.GetString("SYMBOL_CODE"));
                    DwMain.SetItemString(1, "investtype_code", dt.GetString("INVESTTYPE_CODE"));
                    DwMain.SetItemString(1, "invsource_code", dt.GetString("INVSOURCE_CODE"));
                    DwMain.SetItemString(1, "rate_code", dt.GetString("RATE_CODE"));
                    DwMain.SetItemDecimal(1, "purchase_price", dt.GetDecimal("PURCHASE_PRICE"));
                    DwMain.SetItemDecimal(1, "maturity_price", dt.GetDecimal("PRNCBAL"));
                    DwMain.SetItemDecimal(1, "investment_period", dt.GetDecimal("INVESTMENT_PERIOD"));
                    DwMain.SetItemDecimal(1, "invest_period_unit", dt.GetDecimal("INVEST_PERIOD_UNIT"));
                    DwMain.SetItemDecimal(1, "unit_amt", dt.GetDecimal("UNIT_AMT"));
                    DwMain.SetItemDecimal(1, "unit_cost", dt.GetDecimal("UNIT_COST"));
                    DwMain.SetItemString(1, "item_code", "w");



                }
                ta.Close();

            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด เรียกข้อมูลไม่สำเร็จ ตรวจสอบ วันที่ขาย และ เลขบัญชี");
                //LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }

        }
        private void tdateChange()
        {
            
            DwMain.GetItemDateTime(1, "operate_date");
            string account_no = DwMain.GetItemString(1, "account_no");
            account_no = int.Parse(account_no).ToString("0000000000");
            //DateTime postTdates = postTdate;
            String xml_main = DwMain.Describe("DataWindow.Data.XML");
            String result = PmService.of_setslipmain_withdraw(state.SsWsPass, xml_main, account_no = int.Parse(account_no).ToString("0000000000"));
            if (result != "")
            {
                DwMain.Reset();
                DwMain.ImportString(result, FileSaveAsType.Xml);
            }

        }
        public void JsBankCHange()
        {
            try
            {
                String bankcode = HdBankCode.Value;
                DwUtil.RetrieveDDDW(DwSlipdet, "bank_branch", pbl, null);
                DataWindowChild dc = DwSlipdet.GetChild("bank_branch");
                dc.SetFilter("bank_code ='" + bankcode + "'");
                dc.Filter();
            }
            catch { }
            DwSlipdet.SetItemString(Convert.ToInt32(HdRow.Value), "bank_branch", "");

        }
        public void GetGroupMoneyType()
        {
            try
            {
                string money_type = DwSlipdet.GetItemString(Convert.ToInt32(HdRow_Type.Value), "money_code");
                DataWindowChild dc = DwSlipdet.GetChild("money_code");
                int rGroup = dc.FindRow("moneytype_code='" + money_type + "'", 1, dc.RowCount);
                string moneytype_group = DwUtil.GetString(dc, rGroup, "moneytype_group", "");
                DwSlipdet.SetItemString(Convert.ToInt32(HdRow_Type.Value), "moneytype_group", moneytype_group);
                HdRow_Type.Value = "";
            }
            catch
            {
                HdRow_Type.Value = "";
            }
        }
        public void SetBankBranch()
        {
            try
            {
                int sheetRow = Convert.ToInt32(HdSheetRow.Value);
                DwSlipdet.SetItemString(sheetRow, "bank_code", HdBank_id.Value);
                DwSlipdet.SetItemString(sheetRow, "bank_branch", HdBranch_id.Value);

                DwSlipdet.SetItemString(sheetRow, "bank_desc", HdBank_desc.Value.Trim() + " " + HdBranch_desc.Value.Trim());
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
    }
}