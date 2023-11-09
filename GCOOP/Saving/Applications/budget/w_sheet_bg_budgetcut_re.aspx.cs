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
using EncryptDecryptEngine;

namespace Saving.Applications.budget
{
    public partial class w_sheet_bg_budgetcut_re : PageWebSheet, WebSheet
    {
        private DwThDate tDwMain;
        protected String DeleteRow;
        protected String initProgress;
        protected String ProcessCutPay;
        protected String FilterSortSeq;
        protected String getSortseq;
        protected BudgetClient serBudget;
        

        public void InitJsPostBack()
        {
            initProgress = WebUtil.JsPostBack(this, "initProgress");
            FilterSortSeq = WebUtil.JsPostBack(this, "FilterSortSeq");
            DeleteRow = WebUtil.JsPostBack(this, "DeleteRow");
            ProcessCutPay = WebUtil.JsPostBack(this, "ProcessCutPay");
            getSortseq = WebUtil.JsPostBack(this, "getSortseq");
            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("operate_date", "operate_tdate");
        }

        public void WebSheetLoadBegin()
        {
            serBudget = wcf.Budget;
            //this.ConnectSQLCA();
            //DwDetail.SetTransaction(sqlca);
            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                DwMain.SetItemDateTime(1, "operate_date", state.SsWorkDate);
                tDwMain.Eng2ThaiAllRow();
                
                try
                {
                    String xml = serBudget.GetBgTypeNonAccId(state.SsWsPass);
                    if (xml != "" && xml != null)
                    {
                        HdCheckAccId.Value = "true";
                        String x = xml.Replace("<d_bg_budgettype_noaccid_group1>", "");
                        xml = x;
                        x = xml.Replace("</d_bg_budgettype_noaccid_group1>", "");
                        xml = x;

                        Encryption ec = new Encryption();
                        string xmlEc = ec.EncryptStrBase64(xml);
                        x = xmlEc.Replace("+", " ");
                        HdXmlFromDlg.Value = x;
                    }
                    else
                    {
                        HdCheckAccId.Value = "false";
                    }
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            }
            else
            {
                this.RestoreContextDw(DwMain);
               
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "initProgress")
            {
                JsProcessCutPay();
            }
            else if (eventArg == "FilterSortSeq")
            {
                
            }
            else if (eventArg == "DeleteRow")
            {
            }
            else if (eventArg == "ProcessCutPay")
            {
                JsProcessCutPay();
            }
            else if (eventArg == "getSortseq")
            {

                GetSortseq();
            }
        }

        private void GetSortseq()
        {
            //   DwDetail.
        }

        public void SaveWebSheet()
        {

        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
           
        }


       
        private void JsProcessCutPay()
        {
            String xmlDetail = "";
            try
            {
                DateTime date = DwMain.GetItemDateTime(1, "operate_date");

                xmlDetail = ""; //serBudget.Processmovepay(state.SsWsPass, date);
                if (xmlDetail != "" && xmlDetail != null)
                {
               
                    //int year = serBudget.GetYearBudget(state.SsWsPass, date);
                    //object[] arg1 = new object[1] { year };
                    //DwUtil.RetrieveDDDW(DwDetail, "budget_seq_no", "budget.pbl", arg1);
                    //Decimal seqNo = DwDetail.GetItemDecimal(1, "budget_seq_no");
                    //object[] arg2 = new object[2] { year, seqNo };
                    //DwUtil.RetrieveDDDW(DwDetail, "sort_seq", "budget.pbl", arg2);
                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่มีข้อมูลวันที่" + date.ToString("dd/MM/yyyy"));
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }
    }
}
