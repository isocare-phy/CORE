using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using DataLibrary;
using Sybase.DataWindow;
using CoreSavingLibrary.WcfNPm;
using System.Web.Services.Protocols;
using CoreSavingLibrary;

namespace Saving.Applications.pm
{
    public partial class w_sheet_pm_edit_close_investment_master : PageWebSheet, WebSheet
    {
        private String pbl = "pm_investment.pbl";
        private n_pmClient PmSevice;
        protected String postAccountNo;
        protected String postNodueFlag;
        protected String postBankCode;
        protected String postBankAccountNo;
        protected String postNull;

        private DwThDate tDwMain;

        public void InitJsPostBack()
        {

            postAccountNo = WebUtil.JsPostBack(this, "postAccountNo");
            postNodueFlag = WebUtil.JsPostBack(this, "postNodueFlag");
            postBankCode = WebUtil.JsPostBack(this, "postBankCode");
            postNull = WebUtil.JsPostBack(this, "postNull");
            postBankAccountNo = WebUtil.JsPostBack(this, "postBankAccountNo");


            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("open_date", "open_tdate");
            tDwMain.Add("close_date", "close_tdate");
            tDwMain.Add("due_date", "due_tdate");
            tDwMain.Add("start_intdate", "start_inttdate");
            tDwMain.Add("push_date", "push_tdate");
            tDwMain.Add("call_date", "call_tdate");
            tDwMain.Add("buy_date", "buy_tdate");
        }

        public void WebSheetLoadBegin()
        {
            try
            {
                PmSevice = wcf.NPm;
            }
            catch
            {
                LtServerMessage.Text = WebUtil.WarningMessage("ติดต่อกับ Web Service ไม่ได้");
            }

            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
            }
            else
            {
                this.RestoreContextDw(DwMain);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postAccountNo")
            {
                JsPostAccountNo();
            }
            else if (eventArg == "postBankCode")
            {
                JsPostBankCode();
            }
            else if (eventArg == "postBankAccountNo")
            {
                JsBankAccountNo();
            }
            else if (eventArg == "postNull")
            {
                JsPostNull();
            }
            else if (eventArg == "postNodueFlag")
            {
                JsNodueFlag();
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
                duedate = DateTime.ParseExact(HdDueDate.Value, "ddMMyyyy", null);
                DwMain.SetItemDateTime(1, "due_date", duedate);
            }


        }

        public void SaveWebSheet()
        {
            //try
            //{
            //    DwMain.SetItemString(1, "coop_id", "001001");
            //    String xml_dwmain = DwMain.Describe("DataWindow.Data.XML");
            //    int result = PmService.of_updateinvestmaster(state.SsWsPass, xml_dwmain);
            //    if (result == 1)
            //    {
            //        JsPostAccountNo();
            //        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
            //    }
            //    else
            //    {
            //        LtServerMessage.Text = WebUtil.ErrorMessage("บันทึกไม่สำเร็จ");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LtServerMessage.Text = WebUtil.ErrorMessage(ex);

            //}


            try
            {
                DwUtil.UpdateDateWindow(DwMain, pbl, "pminvestmaster");
                JsPostAccountNo();
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");

            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("บันทึกไม่สำเร็จ");
            }
        }

        public void WebSheetLoadEnd()
        {
            //tDwMain.Eng2ThaiAllRow();
            try
            {
                DwUtil.RetrieveDDDW(DwMain, "invsource_code", pbl, null);
            }
            catch { }
            try
            {
                DwUtil.RetrieveDDDW(DwMain, "investtype_code", pbl, null);
            }
            catch { }
            try
            {
                DwUtil.RetrieveDDDW(DwMain, "bank_code", pbl, null);
            }
            catch { }
            try
            {
                DwUtil.RetrieveDDDW(DwMain, "accid_int", pbl, null);
            }
            catch { }
            try
            {
                DwUtil.RetrieveDDDW(DwMain, "accid_prnc", pbl, null);
            }
            catch { }


            tDwMain.Eng2ThaiAllRow();
            DwMain.SaveDataCache();
        }

        public void JsPostAccountNo()
        {
            DateTime Q_date = DateTime.Now;
            String coop_id = state.SsCoopId;
            try
            {
                String account_no = DwMain.GetItemString(1, "account_no");
                try
                {
                    string sql = "select due_date from pminvestmaster where account_no='" + account_no + "' ";
                    Sdt dt = WebUtil.QuerySdt(sql);
                    if (dt.Next())
                    {
                        Q_date = dt.GetDate("due_date");
                        if (Q_date > DateTime.Now)
                        {
                            Q_date = DateTime.Now;
                        }
                    }

                }
                catch
                {
                    Q_date = DateTime.Now;
                }

                DwUtil.RetrieveDataWindow(DwMain, pbl, null, account_no, Q_date, coop_id);
                HdDueDate.Value = DwMain.GetItemDate(1,"due_date").ToString("ddMMyyyy");
                JsPostBankCode();
            }
            catch { }
        }

        public void JsPostBankCode()
        {
            //try
            //{
            //    String bank_code = DwMain.GetItemString(1, "bank_code");
            //    DwUtil.RetrieveDDDW(DwMain, "bank_branch", pbl, null);
            //}
            //catch { }
            try
            {
                String bankcode = DwMain.GetItemString(1, "bank_code");
                DwUtil.RetrieveDDDW(DwMain, "bank_branch", pbl, null);
                DataWindowChild dc = DwMain.GetChild("bank_branch");
                dc.SetFilter("bank_code ='" + bankcode + "'");
                dc.Filter();
                //DwMain.SetItemString(1, "bank_branch", "");
            }
            catch { }

        }

        public void JsBankAccountNo()
        {
            try
            {
                String bank_code = DwMain.GetItemString(1, "bank_code");
                String branch_name = "";
                String tran_bankacc_no = DwMain.GetItemString(1, "tran_bankacc_no");
                tran_bankacc_no = tran_bankacc_no.Substring(0, 3);
                tran_bankacc_no = int.Parse(tran_bankacc_no).ToString("0000");

                string selectSQL = "select branch_id from cmucfbankbranch where bank_code = '" + bank_code + "' and branch_id ='" + tran_bankacc_no + "'";
                Sdt dt = WebUtil.QuerySdt(selectSQL);
                if (dt.Next())
                {
                    branch_name = dt.GetString("branch_id");
                    DwMain.SetItemString(1, "bank_branch", tran_bankacc_no);
                }
            }
            catch { }
        }

        public void JsPostNull()
        {
        }
    }
}