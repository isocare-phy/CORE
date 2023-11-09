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
    public partial class w_sheet_cmd_durtcaldevalue : PageWebSheet, WebSheet
    {
        protected String jsProcess;
        protected String jsAccYear;

        Sdt dt ;
        String pbl = "cmd_durtcaldevalue.pbl";

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
            String durt_id = "", insertsql = "", upsql = "";
            DateTime devaluestart_date = new DateTime();
            DateTime from_date = new DateTime();
            DateTime last_date = new DateTime();
            DateTime sale_date = new DateTime();
            DateTime buy_date = new DateTime();
            Decimal devalue_percent = 0, unit_price = 0, devaluebal_amt = 0, dayof_year = 0, durt_status = 0;
            Decimal devl_bfamt = 0, devl_amt = 0, devl_bal = 0, acc_year = 0, devl_month = 0, devl_day = 0, devl_type = 0;
            Decimal increase_amt = 0, decrease_amt = 0, sale_amt = 0, year_sale = 0;
            int row = DwDetail.RowCount;

            acc_year = DwMain.GetItemDecimal(1, "acc_year") - 543;
            devl_month = DwMain.GetItemDecimal(1, "devl_month");
            from_date = new DateTime(Convert.ToInt32(acc_year), Convert.ToInt32(devl_month), 1); ///วันแรกของเดือนที่คิดว่าเสื่อม

            try
            {
                for (int i = 1; i <= row; i++)
                {
                    durt_id = DwDetail.GetItemString(i, "durt_id");
                    devaluestart_date = DwDetail.GetItemDateTime(i, "devaluestart_date");
                    devalue_percent = DwDetail.GetItemDecimal(i, "devalue_percent");
                    unit_price = DwDetail.GetItemDecimal(i, "unit_price");
                    devaluebal_amt = DwDetail.GetItemDecimal(i, "devaluebal_amt");
                    durt_status = DwDetail.GetItemDecimal(i, "durt_status");
                    try { buy_date = DwDetail.GetItemDateTime(i, "buy_date"); }
                    catch { } 
                    try { sale_amt = DwDetail.GetItemDecimal(i, "sale_amt"); }
                    catch { sale_amt = 0; }
                    devl_bfamt = 0;
                    last_date = from_date.AddMonths(1).AddDays(-1); ///วันที่สิ้นเดือนที่คิดว่าเสื่อม
                                                                   
                    if (buy_date.Year == Convert.ToInt32(acc_year) && devaluebal_amt == unit_price)
                    {
                        //devl_bfamt = 0;
                        increase_amt = unit_price;
                    }
                    else
                    {
                        devl_bfamt = devaluebal_amt;
                    }
                    ///กรณีที่มีการตัดจำหน่ายภายในเดือนที่คิดค่าเสื่อม
                    if (durt_status != -9)
                    {
                        ///หาจำนวนวันกรณีที่คิดค่าเสื่อมไม่เต็มปี
                        devl_day = Convert.ToDecimal((last_date - devaluestart_date).TotalDays) + 1;
                        last_date = last_date.AddDays(1);
                    }
                    else
                    {
                        sale_date = DwDetail.GetItemDateTime(i, "sale_date");
                        devl_day = Convert.ToDecimal((last_date - sale_date).TotalDays) + 1;
                        decrease_amt = sale_amt;
                        year_sale = sale_date.Year;

                    }
                    ///จำนวนวันในปีที่รับจากหน้าจอ
                    dayof_year = Convert.ToDecimal(new DateTime(Convert.ToInt32(acc_year), 12, 31).DayOfYear);
                    if ((devaluebal_amt != 1 || devl_bfamt != 1) && (devaluebal_amt != 0 || devl_bfamt != 0 || increase_amt != 0))
                    {
                        devl_amt = Math.Round(((unit_price * (devalue_percent / 100)) / dayof_year) * devl_day,2) ;
                        ///เช็ครายการว่าตัดจำหน่ายหรือไม่
                        if (durt_status != -9)
                        {
                            if (devl_bfamt == 0) ///กรณีเป็นครุภัณฑ์เข้ามาใหม่
                            {
                                devl_bal = unit_price - devl_amt;
                            }
                            else
                            {
                                devl_bal = devl_bfamt - devl_amt;
                            }
                            if (devl_bal < 0)
                            {
                                devl_amt = devaluebal_amt - 1;
                                devl_bal = devaluebal_amt - devl_amt;
                            }
                        }
                        else if (durt_status == -9)
                        {
                            devl_amt = 0;
                            devl_bal = 0;
                        }
                        try
                        {                            
                            if (durt_status != -9)
                            {
                                insertsql = @"INSERT INTO PTDURTCALDEVALUE 
                                    ( ACC_YEAR, DEVL_MONTH, DEVL_BFAMT, DEVL_PERCENT,
                                      DEVL_AMT, DEVL_TYPE, FROM_DATE, DEVL_DAY, 
                                      DURT_ID, DEVL_BAL, INCREASE_AMT, DECREASE_AMT, POST_STATUS
                                    ) values
                                    (" + acc_year + "," + devl_month + "," + devl_bfamt + "," + devalue_percent + @",
                                     " + devl_amt + "," + devl_type + ",to_date('" + last_date.ToString("dd/MM/yyyy") + @"','dd/MM/yyyy')," + devl_day + @",
                                     '" + durt_id + "'," + devl_bal + ","+ increase_amt +","+ decrease_amt +",1)";
                                dt = WebUtil.QuerySdt(insertsql);
                                /// update คงเหลือ ใน ตาราง master 
                                upsql = @"update ptdurtmaster set devaluebal_amt = " + devl_bal + ", devaluelastcal_year = "+ acc_year +@", 
                                        devaluestart_date = to_date('" + last_date.ToString("dd/MM/yyyy") + @"','dd/MM/yyyy') where durt_id = '" + durt_id + "'";
                                dt = WebUtil.QuerySdt(upsql);
                            }
                            else if (durt_status == -9 ) ///กรณีครุภัณฑ์ตัดจำหน่ายไปแล้ว
                            {
                                if (acc_year == year_sale)
                                {
                                    insertsql = @"INSERT INTO PTDURTCALDEVALUE 
                                    ( ACC_YEAR, DEVL_MONTH, DEVL_BFAMT, DEVL_PERCENT,
                                      DEVL_AMT, DEVL_TYPE, FROM_DATE, DEVL_DAY, 
                                      DURT_ID, DEVL_BAL, INCREASE_AMT, DECREASE_AMT, POST_STATUS
                                    ) values
                                    (" + acc_year + "," + devl_month + "," + devl_bfamt + "," + devalue_percent + @",
                                     " + devl_amt + "," + devl_type + ",to_date('" + last_date.ToString("dd/MM/yyyy") + @"','dd/MM/yyyy')," + devl_day + @",
                                     '" + durt_id + "'," + devl_bal + "," + increase_amt + "," + decrease_amt + ",1)";
                                    dt = WebUtil.QuerySdt(insertsql);
                                    /// update คงเหลือ ใน ตาราง master 
                                    upsql = @"update ptdurtmaster set devaluebal_amt = " + devl_bal + ", devaluelastcal_year = " + acc_year + @"
                                        where durt_id = '" + durt_id + "'";
                                    dt = WebUtil.QuerySdt(upsql);
                                }
                            }
                        }
                        catch
                        { }
                    }
                    /// กรณีค่าเสื่อมคงเหลือเท่ากับ 1
                    else
                    {
                        ///เช็ครายการว่าตัดจำหน่ายหรือไม่
                        if (durt_status != -9)
                        {
                            devl_amt = 0;
                            devl_bal = devl_bfamt;
                        }
                        else if (durt_status == -9)
                        {
                            devl_amt = 0;
                            devl_bal = 0;
                        }
                        try
                        {                            
                            if (durt_status != -9)
                            {
                                insertsql = @"INSERT INTO PTDURTCALDEVALUE 
                                    ( ACC_YEAR, DEVL_MONTH, DEVL_BFAMT, DEVL_PERCENT,
                                      DEVL_AMT, DEVL_TYPE, FROM_DATE, DEVL_DAY, 
                                      DURT_ID, DEVL_BAL, INCREASE_AMT, DECREASE_AMT, POST_STATUS
                                    ) values
                                    (" + acc_year + "," + devl_month + "," + devl_bfamt + "," + devalue_percent + @",
                                     " + devl_amt + "," + devl_type + ",to_date('" + last_date.ToString("dd/MM/yyyy") + @"','dd/MM/yyyy')," + devl_day + @",
                                     '" + durt_id + "'," + devl_bal + "," + increase_amt + "," + decrease_amt + ",1)";
                                dt = WebUtil.QuerySdt(insertsql);
                                /// update คงเหลือ ใน ตาราง master 
                                upsql = @"update ptdurtmaster set devaluebal_amt = "+ devaluebal_amt +", devaluelastcal_year = "+ acc_year +@",
                                        devaluestart_date = to_date('" + last_date.ToString("dd/MM/yyyy") + @"','dd/MM/yyyy') where durt_id = '" + durt_id + "'";
                                dt = WebUtil.QuerySdt(upsql);
                            }
                            else if (durt_status == -9) ///กรณีครุภัณฑ์ตัดจำหน่ายไปแล้ว
                            {
                                if (acc_year == year_sale)
                                {
                                    insertsql = @"INSERT INTO PTDURTCALDEVALUE 
                                    ( ACC_YEAR, DEVL_MONTH, DEVL_BFAMT, DEVL_PERCENT,
                                      DEVL_AMT, DEVL_TYPE, FROM_DATE, DEVL_DAY, 
                                      DURT_ID, DEVL_BAL, INCREASE_AMT, DECREASE_AMT, POST_STATUS
                                    ) values
                                    (" + acc_year + "," + devl_month + "," + devl_bfamt + "," + devalue_percent + @",
                                     " + devl_amt + "," + devl_type + ",to_date('" + last_date.ToString("dd/MM/yyyy") + @"','dd/MM/yyyy')," + devl_day + @",
                                     '" + durt_id + "'," + devl_bal + "," + increase_amt + "," + decrease_amt + ",1)";
                                    dt = WebUtil.QuerySdt(insertsql);
                                    /// update คงเหลือ ใน ตาราง master 
                                    upsql = @"update ptdurtmaster set devaluebal_amt = " + devl_bal + ", devaluelastcal_year = " + acc_year + @"
                                        where durt_id = '" + durt_id + "'";
                                    dt = WebUtil.QuerySdt(upsql);
                                }
                            }
                            else
                            {
                                insertsql = @"INSERT INTO PTDURTCALDEVALUE 
                                    ( ACC_YEAR, DEVL_MONTH, DEVL_BFAMT, DEVL_PERCENT,
                                      DEVL_AMT, DEVL_TYPE, FROM_DATE, DEVL_DAY, 
                                      DURT_ID, DEVL_BAL, INCREASE_AMT, DECREASE_AMT, POST_STATUS
                                    ) values
                                    (" + acc_year + "," + devl_month + "," + devl_bfamt + "," + devalue_percent + @",
                                     " + devl_amt + "," + devl_type + ",to_date('" + last_date.ToString("dd/MM/yyyy") + @"','dd/MM/yyyy')," + devl_day + @",
                                     '" + durt_id + "'," + devl_bal + "," + increase_amt + "," + decrease_amt + ",1)";
                                dt = WebUtil.QuerySdt(insertsql);
                                /// update คงเหลือ ใน ตาราง master 
                                upsql = @"update ptdurtmaster set devaluebal_amt = " + devaluebal_amt + ", devaluelastcal_year = " + acc_year + @" 
                                        where durt_id = '" + durt_id + "'";
                                dt = WebUtil.QuerySdt(upsql);
                            }
                        }
                        catch
                        { }
                    }
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
            Decimal AccYear = 0, DevlMonth = 0;
            Int32 devStatus = 0;
            DateTime start_date = new DateTime();
            DateTime end_date = new DateTime();
            AccYear = DwMain.GetItemDecimal(1, "acc_year") - 543;
            DevlMonth = DwMain.GetItemDecimal(1, "devl_month");

            ///เช็คว่าในเดือนที่ทำการประมวลมีการประมวลไปแล้วหรือไม่
            devStatus = CheckDevalue( AccYear, DevlMonth);
            if (devStatus == 1)
            {
                UpReturnDevalueMonth(AccYear, DevlMonth);
                ClearDevalueMonth(AccYear, DevlMonth);
            }

            start_date = new DateTime(Convert.ToInt32(AccYear), Convert.ToInt32(DevlMonth), 1);
            end_date = start_date.AddMonths(1).AddDays(-1);
            DwDetail.Retrieve(end_date);
        }

        private void JsPostSetBegin()
        {
            String AccYear = "", DevlMonth = "";
            DateTime start_date = new DateTime();
            DateTime end_date = new DateTime();
            Int32 devStatus = 0;
            AccYear = GetAccYear(state.SsWorkDate);
            DevlMonth = state.SsWorkDate.ToString("MM");
            DwMain.SetItemDecimal(1, "acc_year", Convert.ToDecimal(AccYear) + 543);
            DwMain.SetItemDecimal(1, "devl_month", Convert.ToDecimal(DevlMonth));
            start_date = new DateTime(Convert.ToInt32(AccYear), Convert.ToInt32(DevlMonth), 1);
            end_date = start_date.AddMonths(1).AddDays(-1);

            ///เช็คว่าในเดือนที่ทำการประมวลมีการประมวลไปแล้วหรือไม่
            devStatus = CheckDevalue(Convert.ToDecimal(AccYear), Convert.ToDecimal(DevlMonth));
            if (devStatus == 1)
            {
                UpReturnDevalueMonth(Convert.ToDecimal(AccYear), Convert.ToDecimal(DevlMonth));
                ClearDevalueMonth(Convert.ToDecimal(AccYear), Convert.ToDecimal(DevlMonth));
            }

            DwDetail.Retrieve(end_date);
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

        Int32 CheckDevalue(Decimal accYear, Decimal devMonth)
        {
            Int32 status = 0;
            Decimal devl_month = 0, acc_year = 0;
            String se = "select * from ptdurtcaldevalue where acc_year = "+ accYear +" and devl_month = " + devMonth + "";
            Sdt ta = WebUtil.QuerySdt(se);
            if (ta.Next())
            {
                devl_month = ta.GetDecimal("devl_month");
                acc_year = ta.GetDecimal("acc_year");
            }
            if ( acc_year == accYear && devl_month == devMonth)
            {
                status = 1;
            }
            return status;
        }

        private void UpReturnDevalueMonth(Decimal accYear, Decimal devMonth)
        {
            DateTime start_date = new DateTime();
            String updevlbf = "update ptdurtmaster p set p.devaluebal_amt = " +
                              "( select pc.devl_bfamt from ptdurtcaldevalue pc " +
                              "where p.durt_id = pc.durt_id " +
                              "and pc.acc_year = " + accYear + " and pc.devl_month = " + devMonth + @")
                               where p.durt_id in ( select pc.durt_id from ptdurtcaldevalue pc " +
                              "where p.durt_id = pc.durt_id " +
                              "and pc.acc_year = " + accYear + " and pc.devl_month = " + devMonth + ")";
            Sdt upsql = WebUtil.QuerySdt(updevlbf);

            start_date = new DateTime(Convert.ToInt32(accYear), Convert.ToInt32(devMonth), 1);
            String updatedev = "update ptdurtmaster set devaluestart_date = to_date('" + start_date.ToString("dd/MM/yyyy") + @"','dd/MM/yyyy')
                                where durt_id in ( select durt_id from ptdurtcaldevalue where acc_year = " + accYear + " and devl_month = " + devMonth + ")";
            Sdt upsql2 = WebUtil.QuerySdt(updatedev);
        }

        private void ClearDevalueMonth(Decimal accYear, Decimal devMonth)
        {
            String deldevalCal = "delete ptdurtcaldevalue where acc_year = " + accYear + " and devl_month = " + devMonth + "";
            Sdt delsql = WebUtil.QuerySdt(deldevalCal);
        }
    }
}