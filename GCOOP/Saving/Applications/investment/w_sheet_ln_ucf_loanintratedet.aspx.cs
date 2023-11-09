using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary.WcfNCommon;
using DataLibrary;
using Sybase.DataWindow;
using System.Data;
using System.Globalization;
namespace Saving.Applications.investment
{
    public partial class w_sheet_ln_ucf_loanintratedet : PageWebSheet, WebSheet
    {
        protected String postInsertRow;
        protected String postDeleteRow;
        protected String postLoanintrate;
        protected String postSetEffective;
        protected String postSetExpire;
        private DwThDate teffectiveDate;
        
        int InsertRow = 0; //จำนวนแถวที่เพิ่ม
        int DataRow = 0; //จำนวนข้อมูลที่มีอยู่เดิม
        private String pbl = "loan_ucf.pbl";

        #region WebSheet Members
        public void InitJsPostBack()
        {
            teffectiveDate = new DwThDate(DwMain, this);
            teffectiveDate.Add("effective_date", "effective_tdate");
            teffectiveDate.Add("expire_date", "expire_tdate");

            postInsertRow = WebUtil.JsPostBack(this, "postInsertRow");
            postDeleteRow = WebUtil.JsPostBack(this, "postDeleteRow");
            postLoanintrate = WebUtil.JsPostBack(this, "postLoanintrate");
            postSetEffective = WebUtil.JsPostBack(this, "postSetEffective");
            postSetExpire = WebUtil.JsPostBack(this, "postSetExpire");
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();
            DwMain.SetTransaction(sqlca);
            if (!IsPostBack)
            {
                DwHead.InsertRow(0);
            }
            else
            {
                this.RestoreContextDw(DwMain, teffectiveDate);
                this.RestoreContextDw(DwHead);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postInsertRow")
            {
                jspostInsertRow();
            }
            else if (eventArg == "postDeleteRow")
            {
                jspostDeleteRow();
            }
            else if (eventArg == "postLoanintrate")
            {
                jspostLoanintrate();
            }
            else if (eventArg == "postSetEffective")
            {
                try
                {
                    int rowcurrent = int.Parse(Hd_row.Value);
                    String effective_tdate = DwMain.GetItemString(rowcurrent, "effective_tdate");
                    DwMain.SetItemString(rowcurrent, "effective_tdate", effective_tdate);
                    DateTime effective_date = DwMain.GetItemDateTime(rowcurrent, "effective_date");
                    DwMain.SetItemDateTime(rowcurrent, "effective_date", effective_date);
                }
                catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message); }
               
               
            }
            else if (eventArg == "postSetExpire")
            {
                int rowcurrent = int.Parse(Hd_row.Value);
                String effective_tdate = DwMain.GetItemString(rowcurrent, "expire_tdate");
                DateTime effective_date = Convert.ToDateTime(effective_tdate);
                DwMain.SetItemDateTime(rowcurrent, "expire_date", effective_date);
            }
        }

        public void SaveWebSheet()
        {
            SaveInfo();
        }

        public void WebSheetLoadEnd()
        {
            try
            {
                DwUtil.RetrieveDDDW(DwHead, "loanintrate_code", pbl, null);
            }
            catch { }
            teffectiveDate.Eng2ThaiAllRow();

            DwMain.SaveDataCache();
            DwHead.SaveDataCache();
        }
        #endregion

        private void jspostInsertRow()
        {
            int seq = 1;
            DwMain.InsertRow(0);
            DwMain.ScrollLastPage();
            for (int i = 1; i < DwMain.RowCount; i++)
            {
                if (DwMain.GetItemDecimal(i, "seq_no") > seq)
                {
                    seq = (int)DwMain.GetItemDecimal(i, "seq_no");
                }
            }
            DwMain.SetItemDecimal(DwMain.RowCount, "seq_no", seq + 1);
        }

        private void jspostDeleteRow()
        {
            Int16 RowDetail = Convert.ToInt16(Hd_row.Value);
            try
            {
                string sqlselect = @"SELECT * FROM lccfloanintratedet WHERE loanintrate_code = '" + DwHead.GetItemString(1, "loanintrate_code") + "' and seq_no = '" +
                    DwMain.GetItemDecimal(RowDetail, "seq_no").ToString() + "'";
                Sdt select = WebUtil.QuerySdt(sqlselect);
                if (select.Next())
                {
                    string sqldelete = @"DELETE FROM lccfloanintratedet WHERE loanintrate_code = '" + DwHead.GetItemString(1, "loanintrate_code") + "' and seq_no = '" +
                        DwMain.GetItemDecimal(RowDetail, "seq_no").ToString() + "'";
                    Sdt delete = WebUtil.QuerySdt(sqldelete);
                    DwMain.DeleteRow(RowDetail);
                    LtServerMessage.Text = WebUtil.CompleteMessage("ลบข้อมูลสำเร็จ");
                }
                else
                {
                    DwMain.DeleteRow(RowDetail);
                }
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ลบข้อมูลไม่สำเร็จ");
            }
        }

        private void jspostLoanintrate()
        {
            string intrateCode = DwHead.GetItemString(1, "loanintrate_code");
            if (intrateCode != "")
            {
                DwMain.Retrieve(intrateCode);
            }
        }

        private void SaveInfo()
        {
            teffectiveDate.Eng2ThaiAllRow();

            bool flag = true;
            string erroe_code = "";
            string loanintrate_code = DwHead.GetItemString(1, "loanintrate_code");
            string seq_no = "";
            string lower_amt = "";
            string upper_amt = "";
            string interest_rate = "";
            DateTime effective_date = state.SsWorkDate;
            DateTime expire_date = state.SsWorkDate;

            InsertRow = DwMain.RowCount;
            string sqlcount = @"SELECT * FROM lccfloanintratedet WHERE loanintrate_code = '" + loanintrate_code + "'";
            Sdt count = WebUtil.QuerySdt(sqlcount);
            DataRow = count.GetRowCount();
            try
            {
                for (int j = 1; j <= DataRow; j++)
                {
                    seq_no = DwMain.GetItemDecimal(j, "seq_no").ToString();
                    lower_amt = DwMain.GetItemDecimal(j, "lower_amt").ToString();
                    upper_amt = DwMain.GetItemDecimal(j, "upper_amt").ToString();
                    interest_rate = DwMain.GetItemDecimal(j, "interest_rate").ToString();
                    effective_date = DwMain.GetItemDateTime(j, "effective_date");
                    expire_date = DwMain.GetItemDateTime(j, "expire_date");

                    string sqlupdate = @"UPDATE lccfloanintratedet SET effective_date = to_date('" + effective_date.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'), expire_date = to_date('" + expire_date.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),"+
                        " lower_amt = '" + lower_amt + "', upper_amt = '" + upper_amt + "', interest_rate = '" + interest_rate + "' WHERE loanintrate_code = '" + loanintrate_code + "' and seq_no = '" + 
                            seq_no + "'";
                    Sdt update = WebUtil.QuerySdt(sqlupdate);
                }

                for (int i = DataRow + 1; i <= InsertRow; i++)
                {
                    try
                    {
                        seq_no = DwMain.GetItemDecimal(i, "seq_no").ToString();
                        lower_amt = DwMain.GetItemDecimal(i, "lower_amt").ToString();
                        upper_amt = DwMain.GetItemDecimal(i, "upper_amt").ToString();
                        interest_rate = DwMain.GetItemDecimal(i, "interest_rate").ToString();
                        effective_date = DwMain.GetItemDateTime(i, "effective_date");
                        expire_date = DwMain.GetItemDateTime(i, "expire_date");

                        string sqlinsert = @"INSERT INTO lccfloanintratedet (coop_id,loanintrate_code,seq_no,effective_date,expire_date, lower_amt,upper_amt,interest_rate) VALUES('" + 
                            state.SsCoopId + "','" + loanintrate_code + "'," + seq_no + "', to_date('" + effective_date.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy') "+
                            "to_date('" + effective_date.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'), "+
                             lower_amt + "," + upper_amt + "," + interest_rate + ")";
                        Sdt insert = WebUtil.QuerySdt(sqlinsert);
                    }
                    catch
                    {
                        if (!flag)
                        {
                            erroe_code += ", ";
                        }
                        erroe_code += seq_no;
                        flag = false;
                    }
                }
                if (flag)
                {
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลสำเร็จ");
                }
                else
                {
                    string intrateCode = DwHead.GetItemString(1, "loanintrate_code");
                    if (intrateCode != "")
                    {
                        DwMain.Reset();
                        DwMain.Retrieve(intrateCode);
                    }
                    LtServerMessage.Text = WebUtil.ErrorMessage("ข้อมูลลำดับที่ " + erroe_code + " ไม่สามารถบันทึกได้");
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.ToString());
            }
        }
    }
}