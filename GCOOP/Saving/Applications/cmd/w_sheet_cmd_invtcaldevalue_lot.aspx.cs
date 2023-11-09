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
using DataLibrary;
using System.Globalization;
using System.Data.OracleClient;
using Sybase.DataWindow;
using CoreSavingLibrary.WcfNCommon;

namespace Saving.Applications.cmd
{
    public partial class w_sheet_cmd_invtcaldevalue_lot : PageWebSheet, WebSheet
    {
        protected String jsProcess;
        protected String jsAccYear;

        Sdt dt ;
        String pbl = "cmd_invtcaldevalue.pbl";

        public void InitJsPostBack()
        {
            jsProcess = WebUtil.JsPostBack(this, "jsProcess");
            jsAccYear = WebUtil.JsPostBack(this, "jsAccYear");
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();
            DwDetail.SetTransaction(sqlca);
            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                JsPostSetBegin();
            }
            else
            {
                this.RestoreContextDw(DwMain);
            }
           
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "jsProcess":
                    SaveWebSheet();
                    break;
                case "jsAccYear":
                    JsChangeAccYear();
                    break;
            }
        }

        public void SaveWebSheet()
        {
            String invt_id = "", invt_lotid = "", insertsql = "";
            DateTime from_date = new DateTime();
            DateTime last_date = new DateTime();
            Decimal bfinvtm = 0, sumrecv = 0, sumpay = 0, invt_bal = 0;
            Decimal acc_year = 0, invt_month = 0;
            int row = DwDetail.RowCount;

            acc_year = DwMain.GetItemDecimal(1, "acc_year") - 543;
            invt_month = DwMain.GetItemDecimal(1, "invt_month");
            from_date = new DateTime(Convert.ToInt32(acc_year), Convert.ToInt32(invt_month), 1); 
            last_date = from_date.AddMonths(1).AddDays(-1); ///วันที่สิ้นเดือนที่ประมวลคงเหลือวัสดุ

            try
            {
                for (int i = 1; i <= row; i++)
                {
                    invt_id = DwDetail.GetItemString(i, "invt_id");
                    invt_lotid = DwDetail.GetItemString(i, "invt_lotid");
                    bfinvtm = DwDetail.GetItemDecimal(i, "bfinvtm");
                    sumrecv = DwDetail.GetItemDecimal(i, "sumrecv");
                    sumpay = DwDetail.GetItemDecimal(i, "sumpay");
                    invt_bal = DwDetail.GetItemDecimal(i, "invt_bal");

                                                                   
                    try
                    {
                        insertsql = @"INSERT INTO PTINVTCALDEVALUE 
                                ( ACC_YEAR , INVT_MONTH , INVT_BFAMT, INVTINCREASE_AMT , 
                                    INVTDECREASE_AMT , INVT_BAL , INVT_TYPE , INVT_ID , 
                                    INVT_LOTID , ENTRY_DATE , POST_STATUS
                                ) values
                                (" + acc_year + "," + invt_month + "," + bfinvtm + "," + sumrecv + @",
                                    " + sumpay + "," + invt_bal + ",1 ,'" + invt_id + @"',
                                    '" + invt_lotid + "',to_date('" + last_date.ToString("dd/MM/yyyy") + @"','dd/MM/yyyy'), 1)";
                        dt = WebUtil.QuerySdt(insertsql);                           
                    }
                    catch
                    { }
                    
                }
                
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }

        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
        }

        private void JsChangeAccYear()
        {
            Decimal AccYear = 0, InvtMonth = 0;
            DateTime start_date = new DateTime();
            DateTime end_date = new DateTime();
            AccYear = DwMain.GetItemDecimal(1, "acc_year") - 543;
            InvtMonth = DwMain.GetItemDecimal(1, "invt_month");
            start_date = new DateTime(Convert.ToInt32(AccYear), Convert.ToInt32(InvtMonth), 1);
            end_date = start_date.AddMonths(1).AddDays(-1);
            DwDetail.Retrieve(end_date);
        }

        private void JsPostSetBegin()
        {
            String AccYear = "", InvtMonth = "";
            DateTime start_date = new DateTime();
            DateTime end_date = new DateTime();
            Int32 devStatus = 0;

            AccYear = GetAccYear(state.SsWorkDate);
            InvtMonth = state.SsWorkDate.ToString("MM");
            DwMain.SetItemDecimal(1, "acc_year", Convert.ToDecimal(AccYear) + 543);
            DwMain.SetItemDecimal(1, "invt_month", Convert.ToDecimal(InvtMonth));
            start_date = new DateTime(Convert.ToInt32(AccYear), Convert.ToInt32(InvtMonth), 1);
            end_date = start_date.AddMonths(1).AddDays(-1);

            ///เช็คว่าในเดือนที่ทำการประมวลมีการประมวลไปแล้วหรือไม่
            devStatus = CheckDevalue(Convert.ToDecimal(InvtMonth), Convert.ToDecimal(AccYear));
            if (devStatus == 1)
            {
                ClearInvalueMonth(Convert.ToDecimal(AccYear), Convert.ToDecimal(InvtMonth));
            }

            DwDetail.Retrieve(start_date, end_date);
        }

        String GetAccYear(DateTime entry_date)
        {
            String AccYear = "";

            try
            {
                String se = @"select accperiod.account_year as account_year 
                            from	accperiod  
                            where accperiod.period_end_date in 
                            (	select	min( accperiod.period_end_date)
                            from accperiod  
                            where accperiod.period_end_date >= to_date('" + entry_date.ToString("dd/MM/yyyy") + @"','dd/MM/yyyy') ) ";
                dt = WebUtil.QuerySdt(se);
                if (dt.Next())
                {
                    AccYear = dt.GetString("account_year").Trim();
                }
            }
            catch { }
            return AccYear;
        }

        Int32 CheckDevalue(Decimal InvtMonth, Decimal AccYear)
        {
            Int32 status = 0;
            Decimal invt_month = 0;
            String se = "select * from ptinvtcaldevalue where invt_month = " + InvtMonth + " and acc_year = "+ AccYear +"";
            Sdt ta = WebUtil.QuerySdt(se);
            if (ta.Next())
            {
                invt_month = ta.GetDecimal("invt_month");
            }

            if (invt_month == InvtMonth)
            {
                status = 1;
            }
            return status;
        }

        private void UpReturnInvalueMonth(Decimal accYear, Decimal InvtMonth)
        {

            String upinvtbf = "update ptinvtmast p set p.qty_bal = " +
                              "( select pi.invt_bfamt from ptinvtcaldevalue pi " +
                              "where p.invt_id = pi.invt_id " +
                              "and pi.invt_month = " + InvtMonth + ")";
            //Sdt upsql = WebUtil.QuerySdt(upinvtbf);
        }

        private void ClearInvalueMonth(Decimal accYear, Decimal InvtMonth)
        {
            String delintvalCal = "delete ptinvtcaldevalue where acc_year = " + accYear + " and invt_month = " + InvtMonth + "";
            Sdt delsql = WebUtil.QuerySdt(delintvalCal);
        }
    }
}