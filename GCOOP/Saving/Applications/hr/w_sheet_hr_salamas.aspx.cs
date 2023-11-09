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

namespace Saving.Applications.hr
{
    public partial class w_sheet_hr_salamas : PageWebSheet, WebSheet
    {
        protected String getNameId;
        protected String getProcess;
        protected String getTranfer;
        protected String getCheckprocess;

        public void InitJsPostBack()
        {
            getNameId = WebUtil.JsPostBack(this, "getNameId");
            getProcess = WebUtil.JsPostBack(this, "getProcess");
            getTranfer = WebUtil.JsPostBack(this, "getTranfer");
            getCheckprocess = WebUtil.JsPostBack(this, "getCheckprocess");
        }

        public void WebSheetLoadBegin()
        {
            Literal1.Text = "";
            this.ConnectSQLCA();
            dw_process.SetTransaction(sqlca);
            DwMain.SetTransaction(sqlca);
            DwDetail.SetTransaction(sqlca);

            if (!IsPostBack)
            {
                dw_process.InsertRow(0);
                DwMain.InsertRow(0);
                DwDetail.InsertRow(0);

            }
            else
            {
                try
                {
                    dw_process.RestoreContext();
                    DwMain.RestoreContext();
                    DwDetail.RestoreContext();
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "getNameId")
            {
                GetNameId();
            }
            else if (eventArg == "getProcess")
            {
                GetProcess();
            }
            else if (eventArg == "getTranfer")
            {
                GetTranfer();
            }
            else if (eventArg == "getCheckprocess")
            {
                GetCheckprocess();
            }
        }

        private void GetTranfer()
        {
            try
            {
               // Sta ta = new Sta(sqlca.ConnectionString);
                Sdt dt = new Sdt();
                
                String sqlStr = @"   SELECT  HRNPAYROLL.EMPLID,HRNPAYROLL.SEQ_PAY,   
                                             HRNPAYROLL.PAYROLL_AMT,   
                                             HRNPAYROLL.TRAN_ACC,
									HRNPAYROLL.TRAN_DATE,
                                            HRNPAYROLL.PAY_YEAR
                                        FROM HRNPAYROLL 
                                    WHERE  HRNPAYROLL.SEQ_PAY='N01'";
                   dt = WebUtil.QuerySdt(sqlStr);
                        if (dt.Next())
                        {
                             String sqlCheck = "select * from dpdepttran where coop_id = '"+state.SsCoopId+"' and memcoop_id = '"+state.SsCoopId+"' and system_code = 'HRP' and tran_year = '"+dt.Rows[0]["PAY_YEAR"]+"' and tran_date  = '"+dt.GetDateEn("TRAN_DATE")+"' ";
                             Sdt dtt = WebUtil.QuerySdt(sqlCheck);
                             if (dtt.Next())
                             {
                                 LtServerMessage.Text = WebUtil.ErrorMessage("เดือนนี้มีการโอนเข้าบัญชีเงินฝากเรียบร้อยแล้ว");
                                 dt.Clear();
                             }
                             else
                             {
                                 dt = WebUtil.QuerySdt(sqlStr);
                                 while (dt.Next())
                                 {
                                     Decimal PAYROLL_AMT = dt.GetDecimal("PAYROLL_AMT");
                                     String EMPLID = dt.GetString("EMPLID");;
                                     String sqlMem_no = "select * from ";

                                 }
                             }
                            
                        }
                     
            }
            catch { }
            LtServerMessage.Text = WebUtil.CompleteMessage("โอนเข้าเงินฝากเรียบร้อยแล้ว");
        }

        private void GetCheckprocess()
        {
            Decimal year_pay = dw_process.GetItemDecimal(1, "year_pay") - 543;
            Decimal month_pay = dw_process.GetItemDecimal(1, "month_pay");
            String sqlStr;
            Sta ta = new Sta(sqlca.ConnectionString);
            Sdt dt = new Sdt();
            sqlStr = @"  SELECT HRNPAYROLL.EMPLID  
                             FROM HRNPAYROLL  
                             WHERE ( HRNPAYROLL.PAY_YEAR =" + year_pay + @"  ) AND  
                                   ( HRNPAYROLL.PAY_MONTH = " + month_pay + @" )";
            dt = ta.Query(sqlStr);
            if (dt.Next())
            {
                //เขียน javascript คอนเฟิร์ม
                Literal1.Text = @"
                                <script>
                                if( confirm('มีการประมวลผลเงินเดือนไว้แล้ว ต้องการประมวลผลอีกหรือไม่') ){
                                    getProcess();
                                }
                                </script>
                                ";
            }
            else
            {
                GetProcess();
            }
        }

        private void GetProcess()
        {
            try
            {
                Double sum_payroll = 0;
                Double p_amt = 0, payroll_S01 = 0, payroll_S02 = 0, payroll_S03 = 0, payroll_N01 = 0, payroll_P13 = 0;
                String seq_pay, pre_seq_pay, emplid, sqlStr, emplid_, sqlStr1;
                Decimal setpayroll_amt, salary_lastmonth = 0, payroll_R = 0, payroll_p = 0, payroll_tax = 0;
                Decimal year_pay = dw_process.GetItemDecimal(1, "year_pay") - 543;
                Decimal month_pay = dw_process.GetItemDecimal(1, "month_pay");
                DateTime entry_date = state.SsWorkDate;
                Decimal month_befor = month_pay - 1;
                if (month_befor < 1) { month_befor = 1; }
                Sta ta = new Sta(sqlca.ConnectionString);
                Sdt dt = new Sdt();

                sqlStr = @"DELETE FROM HRNPAYROLL 
                           WHERE ( HRNPAYROLL.PAY_YEAR =" + year_pay + @"  ) AND  
                                 ( HRNPAYROLL.PAY_MONTH = " + month_pay + @" )";
                Sdt dt_delete = ta.Query(sqlStr);

                sqlStr = @" SELECT HRNPAYROLL_SALARY.PAYLIST,   
                            HRNPAYROLL_SET.SET_NO,   
                            HRNPAYROLL_SALARY.SEQ_PAY,   
                            HRNPAYROLL_SET.EMPLID,   
                            HRNPAYROLL_SET.SETPAYROLL_AMT,   
                            HRNPAYROLL_SALARY.SEQ_SORT,   
                            HRNPAYROLL_SALARY.FLAG_SHOW_SET     
                            FROM HRNPAYROLL_SALARY,   
                            HRNPAYROLL_SET  
                            WHERE (HRNPAYROLL_SET.SEQ_PAY =  HRNPAYROLL_SALARY.SEQ_PAY)
                            ORDER BY HRNPAYROLL_SET.EMPLID ASC,   
                            HRNPAYROLL_SALARY.SEQ_SORT ASC";
                Sdt dt_1 = ta.Query(sqlStr);
                int row = dt_1.GetRowCount();
                for (int i = 0; i < row; i++)
                {
                    try { setpayroll_amt = Convert.ToDecimal(dt_1.Rows[i]["SETPAYROLL_AMT"]); }
                    catch { setpayroll_amt = 0; }
                    emplid = dt_1.Rows[i]["EMPLID"].ToString();
                    seq_pay = dt_1.Rows[i]["SEQ_PAY"].ToString();
                    pre_seq_pay = WebUtil.Left(seq_pay, 1);

                    if (pre_seq_pay == "R") //******รายการรับ***********
                    {
                        switch (seq_pay)
                        {
                            case "R01": //::เงินเดือน
                                payroll_R = setpayroll_amt;
                                break;
                        }
                    }
                    else if (pre_seq_pay == "P") //*****รายการจ่าย***********
                    {
                        switch (seq_pay)
                        {
                            case "P01"://:: ภาษี เรียก function คำนวณภาษี ส่ง  tax_amt,emplid
                                sqlStr1 = @" SELECT TAXLIST1_AMT   
                                                FROM HRNPAYROLL_TAXSET 
                                                WHERE EMPLID ='" + emplid + "' AND SEQ_TAX ='A16' AND PAY_YEAR = '" + year_pay + "'AND PAY_MONTH = '" + month_pay + "' ";
                                Sdt dt_search_salary = ta.Query(sqlStr1);
                                dt_search_salary.Next();
                                try
                                {
                                    salary_lastmonth = dt_search_salary.GetDecimal("TAXLIST1_AMT");
                                }
                                catch (Exception ex)
                                {
                                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                                }
                                payroll_tax = salary_lastmonth;
                                setpayroll_amt = payroll_tax;

                                break;
                            case "P12"://:: เบี้ยประกันสังคม
                                if (payroll_R >= 15000) { setpayroll_amt = 750; payroll_p = 750; }
                                else
                                {
                                    payroll_p = (payroll_R * 5) / 100;
                                    setpayroll_amt = (payroll_R * 5) / 100;
                                }
                                break;
                        }

                    }
                    if (seq_pay == "S01")//:: เงินเดือนสะสม
                    {
                        sqlStr = @"  SELECT SUM(  HRNPAYROLL.PAYROLL_AMT  ) AS  PAYROLL_AMT
                                                                                                        FROM HRNPAYROLL  
                                                                                                    WHERE ( HRNPAYROLL.PAY_YEAR =" + year_pay + @"  ) AND                                                                                           
                                                                                                            ( HRNPAYROLL.SEQ_PAY ='R01'  ) AND  
                                                                                                            ( HRNPAYROLL.EMPLID ='" + emplid + "'  ) ";
                        dt = ta.Query(sqlStr);
                        dt.Next();
                        try
                        {
                            payroll_S01 = dt.GetDouble("PAYROLL_AMT");
                        }
                        catch { payroll_S01 = 0; }
                        setpayroll_amt = Convert.ToDecimal(payroll_S01);
                    }
                    else if (seq_pay == "S02")//:: ภาษีสะสม
                    {
                        sqlStr = @"  SELECT SUM(  HRNPAYROLL.PAYROLL_AMT  ) AS  PAYROLL_AMT
                                                                                                        FROM HRNPAYROLL  
                                                                                                    WHERE ( HRNPAYROLL.PAY_YEAR =" + year_pay + @"  ) AND                                                                                           
                                                                                                            ( HRNPAYROLL.SEQ_PAY ='P01'  ) AND  
                                                                                                            ( HRNPAYROLL.EMPLID ='" + emplid + "'  ) ";
                        dt = ta.Query(sqlStr);
                        dt.Next();
                        try
                        {
                            payroll_S02 = dt.GetDouble("PAYROLL_AMT");
                        }
                        catch { payroll_S02 = 0; }
                        setpayroll_amt = Convert.ToDecimal(payroll_S02);
                    }
                    else if (seq_pay == "S03")//:: เบี้ยประกันสังคมสะสม
                    {
                        sqlStr = @"  SELECT SUM(  HRNPAYROLL.PAYROLL_AMT  ) AS  PAYROLL_AMT
                                                                                                        FROM HRNPAYROLL  
                                                                                                    WHERE ( HRNPAYROLL.PAY_YEAR =" + year_pay + @"  ) AND                                                                                           
                                                                                                            ( HRNPAYROLL.SEQ_PAY ='P12'  ) AND  
                                                                                                            ( HRNPAYROLL.EMPLID ='" + emplid + "'  ) ";
                        dt = ta.Query(sqlStr);
                        dt.Next();
                        try
                        {
                            payroll_S03 = dt.GetDouble("PAYROLL_AMT");
                        }
                        catch { payroll_S03 = 0; }
                        setpayroll_amt = Convert.ToDecimal(payroll_S03);
                    }

                    else if (seq_pay == "S04")//:: กองทุนสำรองเลี้ยงชีพสะสม
                    {
                        sqlStr = @"  SELECT SUM(  HRNPAYROLL.PAYROLL_AMT  ) AS  PAYROLL_AMT
                                                                                                        FROM HRNPAYROLL  
                                                                                                    WHERE ( HRNPAYROLL.PAY_YEAR =" + year_pay + @"  ) AND                                                                                           
                                                                                                            ( HRNPAYROLL.SEQ_PAY ='P15'  ) AND  
                                                                                                            ( HRNPAYROLL.EMPLID ='" + emplid + "'  ) ";
                        dt = ta.Query(sqlStr);
                        dt.Next();
                        try
                        {
                            payroll_S03 = dt.GetDouble("PAYROLL_AMT");
                        }
                        catch { payroll_S03 = 0; }
                        setpayroll_amt = Convert.ToDecimal(payroll_S03);
                    }

                    sqlStr = @"  INSERT INTO HRNPAYROLL  
                                                                ( PAY_YEAR,   
                                                                PAY_MONTH,   
                                                                SEQ_PAY,   
                                                                EMPLID,   
                                                                INCOMETYPE_ID,   
                                                                PAYROLL_AMT,   
                                                                ENTRY_ID,   
                                                                ENTRY_DATE,TRAN_DATE,TRAN_ACC )  
                                                        VALUES ( " + year_pay + @",   
                                                                " + month_pay + @",   
                                                                '" + seq_pay + @"',   
                                                                '" + emplid + @"',   
                                                                null,   
                                                                " + setpayroll_amt + @",   
                                                                null,   
                                                                to_date('" + entry_date.ToString("dd/MM/yyyy", WebUtil.EN) + @"', 'dd/mm/yyyy') ,null,null  )";
                    ta.Exe(sqlStr);


                }//end for 

                sqlStr = @"   SELECT    DISTINCT  HRNPAYROLL_SET.EMPLID  
                            FROM HRNPAYROLL_SET";
                Sdt dt_2 = ta.Query(sqlStr);
                int w = dt_2.GetRowCount();
                for (int j = 0; j < w; j++)
                {
                    emplid_ = dt_2.Rows[j]["EMPLID"].ToString();
                    //*********sum รายรับทั้งหมด ไว้ที่ R09****************
                    sqlStr = @"  SELECT SUM(  HRNPAYROLL.PAYROLL_AMT  )AS PAYROLL_AMT
                                                                                                        FROM HRNPAYROLL  
                                                                                                    WHERE ( HRNPAYROLL.PAY_YEAR =" + year_pay + @"  ) AND  
                                                                                                            ( HRNPAYROLL.PAY_MONTH = " + month_pay + @" ) AND  
                                                                                                            ( HRNPAYROLL.SEQ_PAY LIKE 'R%'  ) AND  
                                                                                                            ( HRNPAYROLL.EMPLID ='" + emplid_ + "'  ) ";
                    dt = ta.Query(sqlStr);
                    dt.Next();
                    try
                    {
                        sum_payroll = dt.GetDouble("PAYROLL_AMT");
                    }
                    catch { sum_payroll = 0; }
                    sqlStr = @"  UPDATE HRNPAYROLL  
                                                            SET PAYROLL_AMT = " + sum_payroll + @" 
                                                        WHERE ( HRNPAYROLL.PAY_YEAR = " + year_pay + @" ) AND  
                                                                ( HRNPAYROLL.PAY_MONTH =  " + month_pay + @"  ) AND  
                                                                ( HRNPAYROLL.SEQ_PAY ='R09'  ) AND  
                                                                ( HRNPAYROLL.EMPLID ='" + emplid_ + "'  )";
                    ta.Exe(sqlStr);
                    //*********หาภาษีโบนัส ****************
                    sqlStr = @"  SELECT SUM(  HRNPAYROLL.PAYROLL_AMT  )AS PAYROLL_AMT
                                                                                                        FROM HRNPAYROLL  
                                                                                                    WHERE ( HRNPAYROLL.PAY_YEAR =" + year_pay + @"  ) AND  
                                                                                                            ( HRNPAYROLL.PAY_MONTH = " + month_pay + @" ) AND  
                                                                                                            ( HRNPAYROLL.SEQ_PAY = 'R99'  ) AND  
                                                                                                            ( HRNPAYROLL.EMPLID ='" + emplid_ + "'  ) ";
                    dt = ta.Query(sqlStr);
                    dt.Next();
                    try
                    {
                        payroll_N01 = dt.GetDouble("PAYROLL_AMT");
                    }
                    catch { payroll_N01 = 0; }

                    if (payroll_N01 != 0)
                    {
                        //decimal Bonus_amt = (payroll_R * 12) + Convert.ToDecimal(payroll_N01);
                    //    decimal tax_bonus_amt = GetCalTax(emplid_, payroll_R, Convert.ToDecimal(payroll_N01));
                        //String aa = emplid_;
                        decimal tax_bonus_amt = GetCallTaxMonth(emplid_);
                        sqlStr = @"  UPDATE HRNPAYROLL  
                                                            SET PAYROLL_AMT = " + tax_bonus_amt + @" 
                                                        WHERE ( HRNPAYROLL.PAY_YEAR = " + year_pay + @" ) AND  
                                                                ( HRNPAYROLL.PAY_MONTH =  " + month_pay + @"  ) AND  
                                                                ( HRNPAYROLL.SEQ_PAY ='P99'  ) AND  
                                                                ( HRNPAYROLL.EMPLID ='" + emplid_ + "'  )";
                        ta.Exe(sqlStr);
                    }
                    //*********sum รายจ่ายทั้งหมด ไว้ที่ P16****************
                    sqlStr = @"  SELECT SUM(  HRNPAYROLL.PAYROLL_AMT  )AS PAYROLL_AMT
                                                                                                        FROM HRNPAYROLL  
                                                                                                    WHERE ( HRNPAYROLL.PAY_YEAR =" + year_pay + @"  ) AND  
                                                                                                            ( HRNPAYROLL.PAY_MONTH = " + month_pay + @" ) AND  
                                                                                                            ( HRNPAYROLL.SEQ_PAY LIKE 'P%'  ) AND  
                                                                                                            ( HRNPAYROLL.EMPLID ='" + emplid_ + "'  ) ";
                    dt = ta.Query(sqlStr);
                    dt.Next();
                    try
                    {
                        payroll_P13 = dt.GetDouble("PAYROLL_AMT");
                    }
                    catch { payroll_P13 = 0; }

                    sqlStr = @"  UPDATE HRNPAYROLL  
                                                            SET PAYROLL_AMT = " + payroll_P13 + @" 
                                                        WHERE ( HRNPAYROLL.PAY_YEAR = " + year_pay + @" ) AND  
                                                                ( HRNPAYROLL.PAY_MONTH =  " + month_pay + @"  ) AND  
                                                                ( HRNPAYROLL.SEQ_PAY ='P16'  ) AND  
                                                                ( HRNPAYROLL.EMPLID ='" + emplid_ + "'  )";
                    ta.Exe(sqlStr);
                    //*********sum รายรับทั้งหมด-รายจ่ายทั้งหมด ไว้ที่  N01 รายได้สุทธิ ****************
                    p_amt = sum_payroll - payroll_P13;
                    sqlStr = @"  UPDATE HRNPAYROLL  
                                                            SET PAYROLL_AMT = " + p_amt + @"
                                                        WHERE ( HRNPAYROLL.PAY_YEAR = " + year_pay + @" ) AND  
                                                                ( HRNPAYROLL.PAY_MONTH =  " + month_pay + @"  ) AND  
                                                                ( HRNPAYROLL.SEQ_PAY ='N01'  ) AND  
                                                                ( HRNPAYROLL.EMPLID ='" + emplid_ + "'  )";
                    ta.Exe(sqlStr);
                    //*********วันที่โอนเข้าบัญชี ****************
                    sqlStr = @"  UPDATE HRNPAYROLL  
                                                            SET 
                                                                TRAN_DATE=to_date('" + entry_date.ToString("dd/MM/yyyy", WebUtil.EN) + @"', 'dd/mm/yyyy')                                                             
                                                        WHERE ( HRNPAYROLL.PAY_YEAR = " + year_pay + @" ) AND  
                                                                ( HRNPAYROLL.PAY_MONTH =  " + month_pay + @"  ) AND  
                                                                ( HRNPAYROLL.SEQ_PAY ='N03'  ) AND  
                                                                ( HRNPAYROLL.EMPLID ='" + emplid_ + "'  )";
                    ta.Exe(sqlStr);
                    //*********เลขที่บัญชี ****************
                    sqlStr = @"  UPDATE HRNPAYROLL  
                                                            SET TRAN_ACC='xxxxxxxx'
                                                        WHERE ( HRNPAYROLL.PAY_YEAR = " + year_pay + @" ) AND  
                                                                ( HRNPAYROLL.PAY_MONTH =  " + month_pay + @"  ) AND  
                                                                ( HRNPAYROLL.SEQ_PAY ='N02'  ) AND  
                                                                ( HRNPAYROLL.EMPLID ='" + emplid_ + "'  )";
                    ta.Exe(sqlStr);
                }
                LtServerMessage.Text = WebUtil.CompleteMessage("ประมวลผลเสร็จเรียบร้อยแล้ว");
            }//end else have row

            //}
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }


            DwMain.Reset();
            DwDetail.Reset();
            DwMain.InsertRow(1);
            DwDetail.InsertRow(1);
        }

        private Decimal GetCallTaxMonth(String emplid)
        {
            String sqlStr;
            Sta ta = new Sta(sqlca.ConnectionString);
            Sdt dt = new Sdt();
            Decimal year_pay = (dw_process.GetItemDecimal(1, "year_pay")-543);
            Decimal month_pay = dw_process.GetItemDecimal(1, "month_pay");
            Decimal taxmonth;
            sqlStr = @"   SELECT HRNPAYROLL_TAXSET.SEQ_TAX,   
                                             HRNPAYROLL_TAXSET.TAXLIST1_AMT  
                                        FROM HRNPAYROLL_TAXSET  
                                       WHERE ( HRNPAYROLL_TAXSET.PAY_YEAR = " + year_pay + @"  ) AND  
                                             ( HRNPAYROLL_TAXSET.PAY_MONTH =" + month_pay + @" ) AND  
                                             ( HRNPAYROLL_TAXSET.SEQ_TAX = 'A16' ) AND  
                                             ( HRNPAYROLL_TAXSET.EMPLID ='" + emplid + "')   ";
            dt = ta.Query(sqlStr);
            dt.Next();
            taxmonth = Convert.ToDecimal(dt.Rows[0]["TAXLIST1_AMT"]);
            ta.Close();
            return taxmonth;
        }
        private Decimal GetCalTax(String emplid, Decimal payroll_salary, Decimal bonus)
        {

            ///////////////////ตารางภาษี/////////////////////////////
            String sqlStr;
            Sta ta = new Sta(sqlca.ConnectionString);
            Double min_rate1 = 0, max_rate1 = 0, taxper1 = 0, goss_rate1 = 0;
            Double min_rate2 = 0, max_rate2 = 0, taxper2 = 0, goss_rate2 = 0;
            Double min_rate3 = 0, max_rate3 = 0, taxper3 = 0, goss_rate3 = 0;
            Double min_rate4 = 0, max_rate4 = 0, taxper4 = 0, goss_rate4 = 0;
            Double min_rate5 = 0, max_rate5 = 0, taxper5 = 0, goss_rate5 = 0;
            sqlStr = @" SELECT HRNUCFTAX_RATE.MIN_RATE,   
                                             HRNUCFTAX_RATE.MAX_RATE,   
                                             HRNUCFTAX_RATE.TAXPER,   
                                             HRNUCFTAX_RATE.GOSS_RATE  
                                        FROM HRNUCFTAX_RATE  
                                    ORDER BY HRNUCFTAX_RATE.TAXPER ASC";
            Sdt dt_rate = ta.Query(sqlStr);
            int row_rate = dt_rate.GetRowCount();
            // ArrayList tax_rate = new ArrayList();
            for (int t = 0; t < row_rate; t++)
            {
                try
                {
                    if (t == 0)
                    {
                        min_rate1 = Convert.ToDouble(dt_rate.Rows[t]["MIN_RATE"]);
                        max_rate1 = Convert.ToDouble(dt_rate.Rows[t]["MAX_RATE"]);
                        taxper1 = Convert.ToDouble(dt_rate.Rows[t]["TAXPER"]);
                        goss_rate1 = Convert.ToDouble(dt_rate.Rows[t]["GOSS_RATE"]);
                    }
                    else if (t == 1)
                    {
                        min_rate2 = Convert.ToDouble(dt_rate.Rows[t]["MIN_RATE"]);
                        max_rate2 = Convert.ToDouble(dt_rate.Rows[t]["MAX_RATE"]);
                        taxper2 = Convert.ToDouble(dt_rate.Rows[t]["TAXPER"]);
                        goss_rate2 = Convert.ToDouble(dt_rate.Rows[t]["GOSS_RATE"]);
                    }
                    else if (t == 2)
                    {
                        min_rate3 = Convert.ToDouble(dt_rate.Rows[t]["MIN_RATE"]);
                        max_rate3 = Convert.ToDouble(dt_rate.Rows[t]["MAX_RATE"]);
                        taxper3 = Convert.ToDouble(dt_rate.Rows[t]["TAXPER"]);
                        goss_rate3 = Convert.ToDouble(dt_rate.Rows[t]["GOSS_RATE"]);
                    }
                    else if (t == 3)
                    {
                        min_rate4 = Convert.ToDouble(dt_rate.Rows[t]["MIN_RATE"]);
                        max_rate4 = Convert.ToDouble(dt_rate.Rows[t]["MAX_RATE"]);
                        taxper4 = Convert.ToDouble(dt_rate.Rows[t]["TAXPER"]);
                        goss_rate4 = Convert.ToDouble(dt_rate.Rows[t]["GOSS_RATE"]);
                    }
                    else if (t == 4)
                    {
                        min_rate5 = Convert.ToDouble(dt_rate.Rows[t]["MIN_RATE"]);
                        max_rate5 = Convert.ToDouble(dt_rate.Rows[t]["MAX_RATE"]);
                        taxper5 = Convert.ToDouble(dt_rate.Rows[t]["TAXPER"]);
                        goss_rate5 = Convert.ToDouble(dt_rate.Rows[t]["GOSS_RATE"]);
                    }

                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }

            }
            /////////////////////////    ก. การคำนวณภาษี     /////////////////////////////////////////////////////        
            Decimal payroll_tax = 0, payroll_C23 = 0, payroll_B07 = 0;
            Decimal A_01 = 0, A_02 = 0, A_03 = 0, A_05 = 0, A_06 = 0, A_07 = 0, A_08 = 0
                  , A_09 = 0, A_10 = 0, A_11 = 0, A_12 = 0;
            Double A_04 = 0;

            Decimal payroll_amt = payroll_salary;
            Decimal year_pay = (dw_process.GetItemDecimal(1, "year_pay") - 543);
            Decimal month_pay = dw_process.GetItemDecimal(1, "month_pay");

            Sdt dt = new Sdt();
            try
            {

                //                sqlStr = @" SELECT   
                //                                                  HRNPAYROLL_SET.EMPLID,   
                //                                                  HRNPAYROLL_SET.SETPAYROLL_AMT   
                //                                    FROM          HRNPAYROLL_SET 
                //                                    WHERE (HRNPAYROLL_SET.SEQ_PAY =  'R01') AND HRNPAYROLL_SET.EMPLID='" + emplid + "'";
                //                dt = ta.Query(sqlStr);
                //                dt.Next();
                //                try { payroll_amt = Convert.ToDecimal(dt.GetDouble("SETPAYROLL_AMT")); }
                //                catch { payroll_amt = 0; }

                //                sqlStr = @"   SELECT HRNPAYROLL_TAXSET.SEQ_TAX,   
                //                                             HRNPAYROLL_TAXSET.TAXLIST1_AMT  
                //                                        FROM HRNPAYROLL_TAXSET  
                //                                       WHERE ( HRNPAYROLL_TAXSET.PAY_YEAR =" + year_pay + @" ) AND  
                //                                             ( HRNPAYROLL_TAXSET.PAY_MONTH = " + month_pay + @" ) AND  
                //                                             ( HRNPAYROLL_TAXSET.SEQ_TAX = 'B07' ) AND  
                //                                             ( HRNPAYROLL_TAXSET.EMPLID = '" + emplid + "' )   ";
                //                dt = ta.Query(sqlStr);
                //                dt.Next();
                //                payroll_B07 = Convert.ToDecimal(dt.Rows[0]["TAXLIST1_AMT"]);

                sqlStr = @"   SELECT HRNPAYROLL_TAXSET.SEQ_TAX,   
                                             HRNPAYROLL_TAXSET.TAXLIST1_AMT  
                                        FROM HRNPAYROLL_TAXSET  
                                       WHERE ( HRNPAYROLL_TAXSET.PAY_YEAR = " + year_pay + @"  ) AND  
                                             ( HRNPAYROLL_TAXSET.PAY_MONTH =" + month_pay + @" ) AND  
                                             ( HRNPAYROLL_TAXSET.SEQ_TAX = 'C23' ) AND  
                                             ( HRNPAYROLL_TAXSET.EMPLID ='" + emplid + "')   ";
                dt = ta.Query(sqlStr);
                dt.Next();
                payroll_C23 = Convert.ToDecimal(dt.Rows[0]["TAXLIST1_AMT"]);

                sqlStr = @" SELECT HRNPAYROLL_TAXSET.SEQ_TAX,   
                                             HRNPAYROLL_TAXSET.TAXLIST1_AMT  
                                        FROM HRNPAYROLL_TAXSET  
                                       WHERE ( HRNPAYROLL_TAXSET.SEQ_TAX LIKE'A%' ) AND  
                                             ( HRNPAYROLL_TAXSET.EMPLID = '" + emplid + "' )AND ( HRNPAYROLL_TAXSET.PAY_YEAR = " + year_pay + @"  ) AND  
                                             ( HRNPAYROLL_TAXSET.PAY_MONTH =" + month_pay + @" )   ORDER BY HRNPAYROLL_TAXSET.SEQ_TAX ASC ";
                dt = ta.Query(sqlStr);
                int row_tax = dt.GetRowCount();
                for (int x = 0; x < row_tax; x++)
                {
                    String seq_tax = dt.Rows[x]["SEQ_TAX"].ToString();

                    switch (seq_tax)
                    {
                        case "A01": A_01 = (payroll_amt * 12) + bonus;
                            payroll_tax = A_01;
                            break;
                        case "A02": A_02 = payroll_B07;
                            payroll_tax = A_02;
                            break;
                        case "A03": A_03 = A_01 - A_02;
                            payroll_tax = A_03;
                            break;
                        case "A04": A_04 = Convert.ToDouble(A_03) * 0.4;
                            payroll_tax = Convert.ToDecimal(A_04);
                            break;
                        case "A05": A_05 = A_03 - Convert.ToDecimal(A_04);
                            payroll_tax = A_05;
                            break;
                        case "A06": A_06 = payroll_C23;
                            payroll_tax = A_06;
                            break;
                        case "A07": A_07 = A_05 - A_06;
                            if (A_07 < 0) { A_07 = 0; }
                            payroll_tax = A_07;
                            break;
                        case "A08": A_08 = 0;
                            payroll_tax = A_08;
                            break;
                        case "A09": A_09 = A_07 - A_08;
                            payroll_tax = A_09;
                            break;
                        case "A10": A_10 = 0;
                            payroll_tax = A_10;
                            break;
                        case "A11": A_11 = A_09 - A_10;
                            payroll_tax = A_11;
                            break;
                        case "A12":

                            //////////////////Check Tax_Rate แบบ Hard Code//////////////////
                            Double rate_amt = Convert.ToDouble(A_11);
                            Double max_level = 0;
                            Double P_amt1 = 0, P_amt2 = 0, P_amt3 = 0, P_amt4 = 0, P_amt5 = 0;
                            Double t_amt1 = 0, t_amt2 = 0, t_amt3 = 0, t_amt4 = 0, t_amt5 = 0;
                            if (rate_amt < min_rate2)
                            {
                                A_12 = 1; //ร้อยละ
                                max_level = max_rate1;//จำนวนสูงสุดของขั้น                                        
                                P_amt1 = max_level;//เงินได้สุทธิแต่ละขั้น
                                t_amt1 = 0;//ภาษีเงินได้
                                payroll_tax = Convert.ToDecimal(t_amt1);
                            }
                            else if (rate_amt > max_rate2 || rate_amt < min_rate3)
                            {
                                A_12 = 10; //ร้อยละ
                                P_amt1 = max_rate1;
                                max_level = max_rate2 - max_rate1;//จำนวนสูงสุดของขั้น
                                P_amt2 = rate_amt - P_amt1;//เงินได้สุทธิแต่ละขั้น
                                t_amt2 = P_amt2 * 0.1;//ภาษีเงินได้
                                payroll_tax = Convert.ToDecimal(t_amt2 / 12);
                            }
                            else if (rate_amt > max_rate3 || rate_amt < min_rate4)
                            {
                                A_12 = 20; //ร้อยละ
                                P_amt2 = max_rate2;
                                max_level = max_rate3 - max_rate2;//จำนวนสูงสุดของขั้น
                                P_amt3 = rate_amt - P_amt2;//เงินได้สุทธิแต่ละขั้น
                                t_amt3 = P_amt3 * 0.2;//ภาษีเงินได้
                                payroll_tax = Convert.ToDecimal(t_amt3 / 12);
                            }
                            else if (rate_amt > max_rate4 || rate_amt < min_rate5)
                            {
                                A_12 = 30; //ร้อยละ
                                P_amt3 = max_rate3;
                                max_level = max_rate4 - max_rate3;//จำนวนสูงสุดของขั้น
                                P_amt4 = rate_amt - P_amt3;//เงินได้สุทธิแต่ละขั้น
                                t_amt4 = P_amt4 * 0.3;//ภาษีเงินได้
                                payroll_tax = Convert.ToDecimal(t_amt4 / 12);
                            }
                            else if (rate_amt > max_rate5)
                            {
                                A_12 = 37; //ร้อยละ
                                P_amt4 = max_rate4;
                                max_level = max_rate4 - max_rate5;//จำนวนสูงสุดของขั้น
                                P_amt5 = rate_amt - P_amt4;//เงินได้สุทธิแต่ละขั้น
                                t_amt5 = P_amt5 * 0.37;//ภาษีเงินได้
                                payroll_tax = Convert.ToDecimal(t_amt5 / 12);
                            }
                            break;
                    }

                } //end for tax

            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
            return payroll_tax;
        }


        private void GetNameId()
        {
            try
            {
                Decimal year_pay = dw_process.GetItemDecimal(1, "year_pay") - 543;
                Decimal month_pay = dw_process.GetItemDecimal(1, "month_pay");
                String empid = DwMain.GetItemString(1, "empid").Trim();
                Sta ta = new Sta(sqlca.ConnectionString);
                String sqlStr = @"  SELECT HRNMLEMPLFILEMAS.EMPLID,   
                                         HRNMLEMPLFILEMAS.EMPLFIRSNAME,   
                                         HRNMLEMPLFILEMAS.EMPLLASTNAME  
                                    FROM HRNMLEMPLFILEMAS  WHERE HRNMLEMPLFILEMAS.EMPLID='" + empid + "'";
                Sdt dt = ta.Query(sqlStr);
                dt.Next();
                DwMain.SetItemString(1, "emp_name", dt.GetString("EMPLFIRSNAME"));
                DwMain.SetItemString(1, "emp_lastname", dt.GetString("EMPLLASTNAME"));
                DwDetail.Retrieve(empid, year_pay, month_pay);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                //DwDetail.UpdateData();
                DwUtil.UpdateDataWindow(DwDetail, "hr_payroll.pbl", "HRNPAYROLL");
              
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลสำเร็จ");
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void WebSheetLoadEnd()
        {
        }

        public double sum_payroll { get; set; }
    }
}
