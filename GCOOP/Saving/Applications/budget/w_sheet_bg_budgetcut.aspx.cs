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
    public partial class w_sheet_bg_budgetcut : PageWebSheet, WebSheet
    {
        private DwThDate tDwMain;
        protected String DeleteRow;
        protected String initProgress;
        protected String ProcessCutPay;
        protected String FilterSortSeq;
        private String pbl = "budget.pbl";

        protected BudgetClient serBudget;
        #region WebSheet Members

        public void InitJsPostBack()
        {
            initProgress = WebUtil.JsPostBack(this, "initProgress");
            FilterSortSeq = WebUtil.JsPostBack(this, "FilterSortSeq");
            DeleteRow = WebUtil.JsPostBack(this, "DeleteRow");
            ProcessCutPay = WebUtil.JsPostBack(this, "ProcessCutPay");
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
                this.RestoreContextDw(DwDetail);
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
                try
                {
                    int row = int.Parse(HdRowDetail.Value);
                    //DateTime operateDate = DwMain.GetItemDateTime(1, "operate_date");
                    //Decimal seqNo = DwDetail.GetItemDecimal(row, "budget_seq_no");
                    //object[] argDDDW = new object[2] { operateDate.Year, seqNo };
                    //DwUtil.RetrieveDDDW(DwDetail, "sort_seq", "budget.pbl", argDDDW);
                }
                catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูล"); JsProcessCutPay(); }
            }
            else if (eventArg == "DeleteRow")
            {
            }
            else if (eventArg == "ProcessCutPay")
            {
                JsProcessCutPay();
            }
        }

        public void SaveWebSheet()
        {
            DateTime operateDate = DwMain.GetItemDateTime(1, "operate_date");
            //Decimal seqNo = 0;
            //Decimal sortSeq = 0;
            //            for (int i = 1; i <= DwDetail.RowCount; i++)
            //            {
            //                seqNo = DwDetail.GetItemDecimal(i, "budget_seq_no");
            //                sortSeq = DwDetail.GetItemDecimal(i, "sort_seq");
            //                String sql = @"select budgetgroup_code, budgettype_code from budgettype_year 
            //                               where budgetyear ='" + operateDate.Year + "' and " +
            //                               "seq_no ='" + seqNo + "' and " + 
            //                               "sort_seq = '" + sortSeq + "'";
            //                DataTable dt = WebUtil.Query(sql);
            //                String bgGrp = dt.Rows[0][0].ToString();
            //                String bgType = dt.Rows[0][1].ToString();
            //                DwDetail.SetItemString(i, "budgetgroup_code", bgGrp);
            //                DwDetail.SetItemString(i, "budgettype_code", bgType);
            //            }
            String xmlDetail = DwDetail.Describe("DataWindow.data.XML");
            string x = xmlDetail.Replace("<d_bg_budgetcut_detail_group1>", "");
            xmlDetail = x;
            x = xmlDetail.Replace("</d_bg_budgetcut_detail_group1>", "");
            xmlDetail = x;
            try
            {
                serBudget.SaveFromCutPay(state.SsWsPass, xmlDetail);
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเรียบร้อยแล้ว");
                DwDetail.Reset();
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
            DwDetail.SaveDataCache();
        }

        #endregion

        //private void JsInitSlip()
        //{

        //    DateTime operateDate = DwMain.GetItemDateTime(1, "operate_date");
        //    object[] arg = new object[1] { operateDate };
        //    DwDetail.Reset();
        //    try
        //    {
        //        //DwUtil.RetrieveDataWindow(DwDetail, "budget.pbl", null, arg);
        //        DwDetail.Retrieve(operateDate);
        //    }
        //    catch
        //    {
        //        LtServerMessage.Text = WebUtil.ErrorMessage("ไม่มีข้อมูลวันที่ " + DwMain.GetItemString(1, "operate_tdate"));
        //    }
        //    object[] argDDDW1 = new object[1] { operateDate.Year };
        //    DwUtil.RetrieveDDDW(DwDetail, "budget_seq_no", "budget.pbl", argDDDW1);

        //    int row = int.Parse(HdRowDetail.Value);
        //    Decimal seqNo = DwDetail.GetItemDecimal(row, "budget_seq_no");
        //    object[] argDDDW2 = new object[2] { operateDate.Year, seqNo };
        //    DwUtil.RetrieveDDDW(DwDetail, "sort_seq", "budget.pbl", argDDDW2);
        //}

        private void JsProcessCutPay()
        {
            //String xmlDetail = "";
            try
            {
                DateTime dt001 = DwMain.GetItemDateTime(1, "operate_date");
                DwUtil.RetrieveDataWindow(DwDetail, pbl, null, dt001);
                //DateTime date = DwMain.GetItemDateTime(1, "operate_date");
                //xmlDetail = serBudget.ProcessCutPay(state.SsWsPass, date, state.SsCoopId);
                //if (xmlDetail != "" && xmlDetail != null)
                //{
                //    String xml = xmlDetail.Replace("<d_bg_budgetcut_detail_group1>", "");
                //    xmlDetail = xml;
                //    xml = xmlDetail.Replace("</d_bg_budgetcut_detail_group1>", "");
                //    DwUtil.ImportData(xml, DwDetail, null, FileSaveAsType.Xml);
                //    //RetrieveDDDW
                //    int year = serBudget.GetYearBudget(state.SsWsPass, date);
                //    object[] arg1 = new object[1] { year };
                //    //DwUtil.RetrieveDDDW(DwDetail, "budget_seq_no", "budget.pbl", arg1);
                //    Decimal seqNo;
                //    try
                //    {
                //        seqNo = DwDetail.GetItemDecimal(1, "budget_seq_no");
                //    }
                //    catch { seqNo = 1; }
                //    //object[] arg2 = new object[2] { year, seqNo };
                //    //DwUtil.RetrieveDDDW(DwDetail, "sort_seq", "budget.pbl", arg2);
                //}
                //else
                //{
                //    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่มีข้อมูลวันที่" + date.ToString("dd/MM/yyyy"));
                //}
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }
    }
}
