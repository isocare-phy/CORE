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
//using Sybase.DataWindow;

namespace Saving.Applications.hr
{
    public partial class w_sheet_hr_tax : PageWebSheet, WebSheet
    {
        //POSTBACK
        protected String getCheckprocess;
        protected String getCheckprocess91;
        protected String getProcess;
        protected String getNameId;
        protected String checkFlag;
        protected String getProcess91;

        public void InitJsPostBack()
        {
            getCheckprocess = WebUtil.JsPostBack(this, "getCheckprocess");
            getCheckprocess91 = WebUtil.JsPostBack(this, "getCheckprocess91");
            getProcess = WebUtil.JsPostBack(this, "getProcess");
            getProcess91 = WebUtil.JsPostBack(this, "getProcess91");
            getNameId = WebUtil.JsPostBack(this, "getNameId");
            checkFlag = WebUtil.JsPostBack(this, "checkFlag");
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
            if (eventArg == "getProcess")
            {
                JsGetProcess();
            }
            else if (eventArg == "getProcess91")
            {
                JsGetProcess91();
            }
            else if (eventArg == "getNameId")
            {
                GetNameId();
            }
            else if (eventArg == "checkFlag")
            {
                CheckFlag();
            }
            else if (eventArg == "getCheckprocess")
            {   
                GetCheckProcess();
            }
            else if (eventArg == "getCheckprocess91")
            {
                GetCheckProcess91();
            }
        }

        private void CheckFlag()
        {
            
        }

        private void GetCheckProcess()
        {
            Decimal year_pay = dw_process.GetItemDecimal(1, "year_pay")-543;
            Decimal month_pay = dw_process.GetItemDecimal(1, "month_pay");
            String sqlStr;
            Sta ta = new Sta(sqlca.ConnectionString);

            sqlStr = @"   SELECT HRNPAYROLL_TAXSET.PAY_YEAR,   
                                    HRNPAYROLL_TAXSET.PAY_MONTH  
                            FROM HRNPAYROLL_TAXSET ";
            Sdt dt_haverow = ta.Query(sqlStr);
            if (dt_haverow.Next())
            {
                sqlStr = @"   SELECT HRNPAYROLL_TAXSET.PAY_YEAR,   
                                     HRNPAYROLL_TAXSET.PAY_MONTH  
                              FROM HRNPAYROLL_TAXSET  
                              WHERE ( HRNPAYROLL_TAXSET.PAY_YEAR ='" + year_pay + @"'  ) AND  
                                    ( HRNPAYROLL_TAXSET.PAY_MONTH = '" + month_pay + @"' ) ";
                Sdt dt_month = ta.Query(sqlStr);
                if (dt_month.Next())
                {
                    //เขียน javascript คอนเฟิร์ม
                    Literal1.Text = @"
                                <script>
                                if(confirm('มีการคำนวณภาษีประจำเดือนไว้แล้ว ต้องการคำนวณอีกหรือไม่') ){
                                    getProcess();
                                }
                                </script>
                                ";
                }
                else
                {
                    JsGetProcess();
                }
            }
            else
            {
                GetFProcess();
            }
        }

        private void JsGetProcess()
        {
            try
            {
                String seq_tax, taxlist, pre_seq_tax, emplid, sqlStr, sqlStr1, seq_pay, pre_seq_pay, card_id = "";
                Decimal setpayroll_amt = 0, payroll_tax = 0, payroll_C23 = 0, payroll_B07 = 0, payroll_S = 0;
                Decimal C_01 = 0, C_02 = 0, C_03 = 0, C_04 = 0, C_05 = 0, C_06 = 0, C_07 = 0
                      , C_08 = 0, C_09 = 0, C_10 = 0, C_11 = 0, C_12 = 0, C_13 = 0, C_14 = 0
                      , C_15 = 0, C_16 = 0, C_17 = 0, C_18 = 0, C_19 = 0, C_20 = 0, C_21 = 0
                      , C_22 = 0, C_23 = 0;
                Decimal B_01 = 0, B_02 = 0, B_03 = 0, B_04 = 0, B_05 = 0, B_06 = 0, B_07 = 0;
                Decimal A_01 = 0, A_02 = 0, A_03 = 0, A_05 = 0, A_06 = 0, A_07 = 0, A_08 = 0
                      , A_09 = 0, A_10 = 0, A_11 = 0, A_12 = 0, A_13 = 0, A_14 = 0, A_15 = 0, A_16 = 0
                      , A_17 = 0, A_18 = 0, A_19 = 0, A_20 = 0;
                Double A_04 = 0;
                Decimal pay_month, pay_year, salary_lastmonth = 1;
                Decimal year_pay = dw_process.GetItemDecimal(1, "year_pay") - 543;
                Decimal month_pay = dw_process.GetItemDecimal(1, "month_pay");
                Sta ta = new Sta(sqlca.ConnectionString);

                sqlStr = @"DELETE FROM HRNPAYROLL_TAXSET 
                           WHERE ( HRNPAYROLL_TAXSET.PAY_YEAR =" + year_pay + @"  ) AND  
                                 ( HRNPAYROLL_TAXSET.PAY_MONTH = " + month_pay + @" )";
                Sdt dt_delete = ta.Query(sqlStr);

                Int32 month;
                if (month_pay == 1)
                {
                    month = 12;
                    Int32 year = Convert.ToInt32(year_pay) - 1;
                    year_pay = year;
                }
                else
                {
                    month = Convert.ToInt32(month_pay) - 1;
                }

                sqlStr = @" SELECT *
                                    FROM HRNPAYROLL_TAXSET  
                                   WHERE ( HRNPAYROLL_TAXSET.PAY_YEAR ='" + year_pay + @"' ) AND  
                                         ( HRNPAYROLL_TAXSET.PAY_MONTH ='" + month + @"' ) ";
                Sdt dt_insert = ta.Query(sqlStr);
                int row_insert = dt_insert.GetRowCount();

                sqlStr = @"  SELECT EMPLID FROM HRNMLEMPLFILEMAS ORDER BY EMPLID ASC";
                Sdt dt_emp = ta.Query(sqlStr);
                int row_emp = dt_emp.GetRowCount();
                for (int j = 0; j < row_emp; j++)
                {
                    emplid = dt_emp.Rows[j]["EMPLID"].ToString();
                    // select พนักงาน
                    sqlStr = @"   SELECT HRNPAYROLL_SET.EMPLID,   
                                             HRNPAYROLL_SET.SEQ_PAY,   
                                             HRNPAYROLL_SET.SETPAYROLL_AMT  
                                        FROM HRNPAYROLL_SET WHERE HRNPAYROLL_SET.EMPLID ='" + emplid + "' AND HRNPAYROLL_SET.SEQ_PAY ='R01' ";

                    Sdt dt_payorll = ta.Query(sqlStr);
                    int row_payorll = dt_payorll.GetRowCount();
                    dt_payorll.Next();
                    try
                    {
                        setpayroll_amt = Convert.ToDecimal(dt_payorll.Rows[0]["SETPAYROLL_AMT"]);
                    }
                    catch { setpayroll_amt = 0; }


                    sqlStr1 = @" SELECT TAXLIST1_AMT   
                                        FROM HRNPAYROLL_TAXSET 
                                        WHERE EMPLID ='" + emplid + "' AND SEQ_TAX ='A01' AND PAY_YEAR = '" + year_pay + "'AND PAY_MONTH = '" + month + "' ";
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


                    if (setpayroll_amt == (salary_lastmonth / 12))
                    {
                        sqlStr1 = @" SELECT *
                                   FROM HRNPAYROLL_TAXSET  
                                   WHERE ( HRNPAYROLL_TAXSET.PAY_YEAR ='" + year_pay + @"' ) AND  
                                         ( HRNPAYROLL_TAXSET.PAY_MONTH ='" + month + @"' ) AND 
                                         ( HRNPAYROLL_TAXSET.EMPLID ='" + emplid + @"') 
                                   ORDER BY SEQ_TAX ASC";
                        Sdt dt_paylist = ta.Query(sqlStr1);
                        int row_paylist = dt_paylist.GetRowCount();
                        for (int r = 1; r <= row_paylist; r++)
                        {
                            seq_tax = dt_paylist.Rows[r]["SEQ_TAX"].ToString();
                            taxlist = dt_paylist.Rows[r]["TAXLIST"].ToString();
                            emplid = dt_paylist.Rows[r]["EMPLID"].ToString();
                            pay_year = Convert.ToDecimal(dt_paylist.Rows[r]["PAY_YEAR"]);
                            pay_month = Convert.ToDecimal(dt_paylist.Rows[r]["PAY_MONTH"]);
                            payroll_tax = Convert.ToDecimal(dt_paylist.Rows[r]["TAXLIST1_AMT"]);
                            sqlStr = @" INSERT INTO HRNPAYROLL_TAXSET  
                                                 ( PAY_YEAR,   
                                                   PAY_MONTH,   
                                                   SEQ_TAX,   
                                                   EMPLID,   
                                                   TAXLIST,   
                                                   PAY_DATE,   
                                                   TAXLIST1_AMT,   
                                                   TAXLIST2_AMT,   
                                                   TAXLIST3,   
                                                   TAXLIST_FLAG,   
                                                   ENTRY_ID,   
                                                   ENTRY_DATE )  
                                          VALUES (" + year_pay + @",   
                                                  " + month_pay + @",   
                                                   '" + seq_tax + @"',   
                                                   '" + emplid + @"',     
                                                   '" + taxlist + @"',   
                                                   null,   
                                                   " + payroll_tax + @",   
                                                   null,   
                                                   null,   
                                                   null,   
                                                   null,   
                                                   null)    ";
                            ta.Exe(sqlStr);
                        }
                    }
                    else
                    {
                        ////////////////////////////////////////////////////////////////////////////////
                        sqlStr = @"   SELECT HRNPAYROLL_SET.EMPLID,   
                                         HRNPAYROLL_SET.SEQ_PAY,   
                                         HRNPAYROLL_SET.SETPAYROLL_AMT  
                                    FROM HRNPAYROLL_SET WHERE HRNPAYROLL_SET.EMPLID ='" + emplid + "' AND HRNPAYROLL_SET.SEQ_PAY ='R01' ";

                        Sdt dt_payroll = ta.Query(sqlStr);
                        int row_payroll = dt_payroll.GetRowCount();
                        dt_payroll.Next();
                        try
                        {
                            setpayroll_amt = Convert.ToDecimal(dt_payroll.Rows[0]["SETPAYROLL_AMT"]);
                        }
                        catch { setpayroll_amt = 0; }


                        // select HRNPAYROLL_TAXCFG
                        sqlStr = @"  SELECT HRNPAYROLL_TAXCFG.SEQ_TAX,   
                                     HRNPAYROLL_TAXCFG.TAXLIST  
                                FROM HRNPAYROLL_TAXCFG  ORDER BY HRNPAYROLL_TAXCFG.SEQ_TAX ASC";
                        Sdt dt = ta.Query(sqlStr);
                        int row = dt.GetRowCount();
                        for (int i = 0; i < row; i++)
                        {
                            seq_tax = dt.Rows[i]["SEQ_TAX"].ToString();
                            taxlist = dt.Rows[i]["TAXLIST"].ToString();
                            pre_seq_tax = WebUtil.Left(seq_tax, 1);
                            ///////////////////  ข. รายการเงินที่ได้รับการยกเว้น  //////////////////////////////////////////////
                            if (pre_seq_tax == "B")
                            {
                                card_id = "";
                                switch (seq_tax)
                                {
                                    case "B01": B_01 = 0; payroll_tax = B_01;
                                        break;
                                    case "B02": B_02 = 0; payroll_tax = B_02;
                                        break;
                                    case "B03": B_03 = 0; payroll_tax = B_03;
                                        break;
                                    case "B04": B_04 = 0; payroll_tax = B_04;
                                        break;
                                    case "B05": B_05 = 0; payroll_tax = B_05;
                                        break;
                                    case "B06": B_06 = 0; payroll_tax = B_06;
                                        break;
                                    case "B07": B_07 = B_01 + B_02 + B_03 + B_04 + B_05 + B_06;
                                        payroll_tax = B_07;
                                        break;
                                }
                            }
                            //////////////////////  ค. รายการลดหย่อนและยกเว้นหลังจากหักค่าใช้จ่าย  /////////////////////////////////
                            else if (pre_seq_tax == "C")
                            {
                                switch (seq_tax)
                                {
                                    case "C01": C_01 = 40000; payroll_tax = C_01; card_id = "";
                                        break;
                                    case "C02": C_02 = 30000; payroll_tax = C_02; card_id = "";
                                        break;
                                    case "C03": C_03 = 0; payroll_tax = C_03; card_id = "";
                                        break;
                                    case "C04": C_04 = 0; payroll_tax = C_04; card_id = "";
                                        break;
                                    case "C05": C_05 = 0; payroll_tax = C_05; card_id = "";
                                        break;
                                    case "C06": C_06 = 0; payroll_tax = C_06; card_id = "000000000000";
                                        break;
                                    case "C07": C_07 = 0; payroll_tax = C_07; card_id = "000000000000";
                                        break;
                                    case "C08": C_08 = 0; payroll_tax = C_08; card_id = "000000000000";
                                        break;
                                    case "C09": C_09 = 0; payroll_tax = C_09; card_id = "000000000000";
                                        break;
                                    case "C10": C_10 = 0; payroll_tax = C_10; card_id = "";
                                        break;
                                    case "C11": C_11 = 0; payroll_tax = C_11; card_id = "000000000000";
                                        break;
                                    case "C12": C_12 = 0; payroll_tax = C_12; card_id = "000000000000";
                                        break;
                                    case "C13": C_13 = 0; payroll_tax = C_13; card_id = "000000000000";
                                        break;
                                    case "C14": C_14 = 0; payroll_tax = C_14; card_id = "000000000000";
                                        break;

                                    case "C15": C_15 = 0; payroll_tax = C_15; card_id = "";
                                        break;
                                    case "C16": C_16 = 0; payroll_tax = C_16; card_id = "";
                                        break;
                                    case "C17": C_17 = 0; payroll_tax = C_17; card_id = "";
                                        break;
                                    case "C18": C_18 = 0; payroll_tax = C_18; card_id = "";
                                        break;
                                    case "C19": C_19 = 0; payroll_tax = C_19; card_id = "";
                                        break;
                                    case "C20": C_20 = 0; payroll_tax = C_20; card_id = "";
                                        break;
                                    case "C21": C_21 = 0; payroll_tax = C_21; card_id = "";
                                        break;
                                    case "C22": C_22 = 9000; payroll_tax = C_22; card_id = "";
                                        break;
                                    case "C23": C_23 = C_01 + C_02 + C_03 + C_04 + C_05 + C_06 + C_07 + C_08 + C_09 + C_10 + C_11 + C_12 + C_13 + C_14 + C_15 + C_16 + C_17 + C_18 + C_19 + C_20 + C_21 + C_22;
                                        payroll_tax = C_23; card_id = "";
                                        break;
                                }

                            }
                            //////////////////////////// ส่วนคู่สมรส  /////////////////////////////////////////////////
                            else if (pre_seq_tax == "D")
                            {
                                card_id = "";
                                switch (seq_tax)
                                {
                                    case "D01": payroll_tax = 0;
                                        break;
                                    case "D02": payroll_tax = 0;
                                        break;
                                    case "D03": payroll_tax = 0;
                                        break;
                                    case "D04": payroll_tax = 0;
                                        break;
                                    case "D05": payroll_tax = 0;
                                        break;
                                    case "D06": payroll_tax = 0;
                                        break;
                                    case "D07": payroll_tax = 0;
                                        break;
                                }
                            }
                            sqlStr = @" INSERT INTO HRNPAYROLL_TAXSET  
                                         ( PAY_YEAR,   
                                           PAY_MONTH,   
                                           SEQ_TAX,   
                                           EMPLID,   
                                           TAXLIST,   
                                           PAY_DATE,   
                                           TAXLIST1_AMT,   
                                           TAXLIST2_AMT,   
                                           TAXLIST3,   
                                           TAXLIST_FLAG,   
                                           ENTRY_ID,   
                                           ENTRY_DATE )  
                                  VALUES (" + year_pay + @",   
                                          " + month_pay + @",   
                                           '" + seq_tax + @"',   
                                           '" + emplid + @"',     
                                           '" + taxlist + @"',   
                                           null,   
                                           " + payroll_tax + @",   
                                           null,   
                                           '" + card_id + @"',   
                                           null,   
                                           null,   
                                           null)    ";
                            ta.Query(sqlStr);



                        }
                        ///////////////////ตารางภาษี/////////////////////////////
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
                                    ORDER BY HRNUCFTAX_RATE.MIN_RATE ASC";
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
                        sqlStr = @"   SELECT HRNPAYROLL_TAXSET.SEQ_TAX,   
                                             HRNPAYROLL_TAXSET.TAXLIST1_AMT  
                                        FROM HRNPAYROLL_TAXSET  
                                       WHERE ( HRNPAYROLL_TAXSET.PAY_YEAR =" + year_pay + @" ) AND  
                                             ( HRNPAYROLL_TAXSET.PAY_MONTH = " + month_pay + @" ) AND  
                                             ( HRNPAYROLL_TAXSET.SEQ_TAX = 'B07' ) AND  
                                             ( HRNPAYROLL_TAXSET.EMPLID = '" + emplid + "' )   ";
                        Sdt dt_B07 = ta.Query(sqlStr);
                        dt_B07.Next();
                        sqlStr = @"   SELECT HRNPAYROLL_TAXSET.SEQ_TAX,   
                                             HRNPAYROLL_TAXSET.TAXLIST1_AMT  
                                        FROM HRNPAYROLL_TAXSET  
                                       WHERE ( HRNPAYROLL_TAXSET.PAY_YEAR = " + year_pay + @"  ) AND  
                                             ( HRNPAYROLL_TAXSET.PAY_MONTH =" + month_pay + @" ) AND  
                                             ( HRNPAYROLL_TAXSET.SEQ_TAX = 'C23' ) AND  
                                             ( HRNPAYROLL_TAXSET.EMPLID ='" + emplid + "')   ";
                        Sdt dt_C23 = ta.Query(sqlStr);
                        dt_C23.Next();

                        payroll_B07 = Convert.ToDecimal(dt_B07.Rows[0]["TAXLIST1_AMT"]);
                        payroll_C23 = Convert.ToDecimal(dt_C23.Rows[0]["TAXLIST1_AMT"]);

                        sqlStr = @" SELECT HRNPAYROLL_TAXSET.SEQ_TAX,   
                                             HRNPAYROLL_TAXSET.TAXLIST1_AMT  
                                        FROM HRNPAYROLL_TAXSET  
                                       WHERE ( HRNPAYROLL_TAXSET.SEQ_TAX LIKE'A%' ) AND  
                                             ( HRNPAYROLL_TAXSET.EMPLID = '" + emplid + "' ) ORDER BY HRNPAYROLL_TAXSET.SEQ_TAX ASC ";
                        Sdt dt_a = ta.Query(sqlStr);
                        int row_tax = dt_a.GetRowCount();
                        for (int tax = 0; tax < row_tax; tax++)
                        {
                            seq_tax = dt_a.Rows[tax]["SEQ_TAX"].ToString();

                            switch (seq_tax)
                            {
                                case "A01": A_01 = setpayroll_amt * 12;
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
                                case "A13": A_13 = payroll_tax;
                                    break;
                                case "A14": payroll_tax = 0;
                                    break;
                                case "A15": payroll_tax = 0;
                                    break;
                                case "A16": payroll_tax = 0;
                                    break;
                                case "A17": payroll_tax = 0;
                                    break;
                                case "A18": payroll_tax = 0;
                                    break;
                                case "A19": payroll_tax = 0;
                                    break;
                                case "A20": payroll_tax = 0;
                                    break;
                            }
                            sqlStr = @"  UPDATE HRNPAYROLL_TAXSET  
                                         SET TAXLIST1_AMT = " + payroll_tax + @"  
                                       WHERE ( HRNPAYROLL_TAXSET.PAY_YEAR =" + year_pay + @"   ) AND  
                                             ( HRNPAYROLL_TAXSET.PAY_MONTH = " + month_pay + @" ) AND  
                                             ( HRNPAYROLL_TAXSET.SEQ_TAX ='" + seq_tax + @"'  ) AND  
                                             ( HRNPAYROLL_TAXSET.EMPLID = '" + emplid + "' )";
                            ta.Exe(sqlStr);
                        }
                        ////////////////////////////////////////////////////////////////////////////////
                    }
                }
                LtServerMessage.Text = WebUtil.CompleteMessage("ประมวลผลเสร็จเรียบร้อยแล้ว");
            }

            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }

            DwMain.Reset();
            DwDetail.Reset();
            DwMain.InsertRow(1);
            DwDetail.InsertRow(1);
        }
               

        private void GetFProcess()
        {
            // Set ค่าใหม่ให้กับพนักงานทุกคน
            // select พนักงาน
            String sqlStr, emplid, seq_tax, taxlist, pre_seq_tax, card_id = "";
            Decimal setpayroll_amt = 0, payroll_tax = 0, payroll_C23 = 0, payroll_B07 = 0, payroll_S = 0;
            Decimal C_01 = 0, C_02 = 0, C_03 = 0, C_04 = 0, C_05 = 0, C_06 = 0, C_07 = 0
                    , C_08 = 0, C_09 = 0, C_10 = 0, C_11 = 0, C_12 = 0, C_13 = 0, C_14 = 0
                    , C_15 = 0, C_16 = 0, C_17 = 0, C_18 = 0, C_19 = 0, C_20 = 0, C_21 = 0
                    , C_22 = 0, C_23 = 0;
            Decimal B_01 = 0, B_02 = 0, B_03 = 0, B_04 = 0, B_05 = 0, B_06 = 0, B_07 = 0;
            Decimal A_01 = 0, A_02 = 0, A_03 = 0, A_05 = 0, A_06 = 0, A_07 = 0, A_08 = 0
                    , A_09 = 0, A_10 = 0, A_11 = 0, A_12 = 0, A_13 = 0, A_14 = 0, A_15 = 0, A_16 = 0
                    , A_17 = 0, A_18 = 0, A_19 = 0, A_20 = 0;
            Double A_04 = 0;
            Decimal year_pay = dw_process.GetItemDecimal(1, "year_pay")-543;
            Decimal month_pay = dw_process.GetItemDecimal(1, "month_pay");
            Sta ta = new Sta(sqlca.ConnectionString);

            try{
                    sqlStr = @"  SELECT EMPLID FROM HRNMLEMPLFILEMAS ORDER BY EMPLID ASC";
                    Sdt dt_emp = ta.Query(sqlStr);
                    int row_emp = dt_emp.GetRowCount();
                    for (int j = 0; j < row_emp; j++)
                    {
                        emplid = dt_emp.Rows[j]["EMPLID"].ToString();
                        // select พนักงาน
                        sqlStr = @"   SELECT HRNPAYROLL_SET.EMPLID,   
                                         HRNPAYROLL_SET.SEQ_PAY,   
                                         HRNPAYROLL_SET.SETPAYROLL_AMT  
                                    FROM HRNPAYROLL_SET WHERE HRNPAYROLL_SET.EMPLID ='" + emplid + "' AND HRNPAYROLL_SET.SEQ_PAY ='R01' ";

                        Sdt dt_payorll = ta.Query(sqlStr);
                        int row_payorll = dt_payorll.GetRowCount();
                        dt_payorll.Next();
                        try
                        {
                            setpayroll_amt = Convert.ToDecimal(dt_payorll.Rows[0]["SETPAYROLL_AMT"]);
                        }
                        catch { setpayroll_amt = 0; }


                        // select HRNPAYROLL_TAXCFG
                        sqlStr = @"  SELECT HRNPAYROLL_TAXCFG.SEQ_TAX,   
                                     HRNPAYROLL_TAXCFG.TAXLIST  
                                FROM HRNPAYROLL_TAXCFG  ORDER BY HRNPAYROLL_TAXCFG.SEQ_TAX ASC";
                        Sdt dt = ta.Query(sqlStr);
                        int row = dt.GetRowCount();
                        for (int i = 0; i < row; i++)
                        {
                            seq_tax = dt.Rows[i]["SEQ_TAX"].ToString();
                            taxlist = dt.Rows[i]["TAXLIST"].ToString();
                            pre_seq_tax = WebUtil.Left(seq_tax, 1);
                            ///////////////////  ข. รายการเงินที่ได้รับการยกเว้น  //////////////////////////////////////////////
                            if (pre_seq_tax == "B")
                            {
                                card_id = "";
                                switch (seq_tax)
                                {
                                    case "B01": B_01 = 0; payroll_tax = B_01;
                                        break;
                                    case "B02": B_02 = 0; payroll_tax = B_02;
                                        break;
                                    case "B03": B_03 = 0; payroll_tax = B_03;
                                        break;
                                    case "B04": B_04 = 0; payroll_tax = B_04;
                                        break;
                                    case "B05": B_05 = 0; payroll_tax = B_05;
                                        break;
                                    case "B06": B_06 = 0; payroll_tax = B_06;
                                        break;
                                    case "B07": B_07 = B_01 + B_02 + B_03 + B_04 + B_05 + B_06;
                                        payroll_tax = B_07;
                                        break;
                                }
                            }
                            //////////////////////  ค. รายการลดหย่อนและยกเว้นหลังจากหักค่าใช้จ่าย  /////////////////////////////////
                            else if (pre_seq_tax == "C")
                            {
                                switch (seq_tax)
                                {
                                    case "C01": C_01 = 40000; payroll_tax = C_01; card_id = "";
                                        break;
                                    case "C02": C_02 = 30000; payroll_tax = C_02; card_id = "";
                                        break;
                                    case "C03": C_03 = 0; payroll_tax = C_03; card_id = "";
                                        break;
                                    case "C04": C_04 = 0; payroll_tax = C_04; card_id = "";
                                        break;
                                    case "C05": C_05 = 0; payroll_tax = C_05; card_id = "";
                                        break;
                                    case "C06": C_06 = 0; payroll_tax = C_06; card_id = "000000000000";
                                        break;
                                    case "C07": C_07 = 0; payroll_tax = C_07; card_id = "000000000000";
                                        break;
                                    case "C08": C_08 = 0; payroll_tax = C_08; card_id = "000000000000";
                                        break;
                                    case "C09": C_09 = 0; payroll_tax = C_09; card_id = "000000000000";
                                        break;
                                    case "C10": C_10 = 0; payroll_tax = C_10; card_id = "";
                                        break;
                                    case "C11": C_11 = 0; payroll_tax = C_11; card_id = "000000000000";
                                        break;
                                    case "C12": C_12 = 0; payroll_tax = C_12; card_id = "000000000000";
                                        break;
                                    case "C13": C_13 = 0; payroll_tax = C_13; card_id = "000000000000";
                                        break;
                                    case "C14": C_14 = 0; payroll_tax = C_14; card_id = "000000000000";
                                        break;

                                    case "C15": C_15 = 0; payroll_tax = C_15; card_id = "";
                                        break;
                                    case "C16": C_16 = 0; payroll_tax = C_16; card_id = "";
                                        break;
                                    case "C17": C_17 = 0; payroll_tax = C_17; card_id = "";
                                        break;
                                    case "C18": C_18 = 0; payroll_tax = C_18; card_id = "";
                                        break;
                                    case "C19": C_19 = 0; payroll_tax = C_19; card_id = "";
                                        break;
                                    case "C20": C_20 = 0; payroll_tax = C_20; card_id = "";
                                        break;
                                    case "C21": C_21 = 0; payroll_tax = C_21; card_id = "";
                                        break;
                                    case "C22": C_22 = 9000; payroll_tax = C_22; card_id = "";
                                        break;
                                    case "C23": C_23 = C_01 + C_02 + C_03 + C_04 + C_05 + C_06 + C_07 + C_08 + C_09 + C_10 + C_11 + C_12 + C_13 + C_14 + C_15 + C_16 + C_17 + C_18 + C_19 + C_20 + C_21 + C_22;
                                        payroll_tax = C_23; card_id = "";
                                        break;
                                }

                            }
                            //////////////////////////// ส่วนคู่สมรส  /////////////////////////////////////////////////
                            else if (pre_seq_tax == "D")
                            {
                                card_id = "";
                                switch (seq_tax)
                                {
                                    case "D01": payroll_tax = 0;
                                        break;
                                    case "D02": payroll_tax = 0;
                                        break;
                                    case "D03": payroll_tax = 0;
                                        break;
                                    case "D04": payroll_tax = 0;
                                        break;
                                    case "D05": payroll_tax = 0;
                                        break;
                                    case "D06": payroll_tax = 0;
                                        break;
                                    case "D07": payroll_tax = 0;
                                        break;
                                }
                            }
                            sqlStr = @" INSERT INTO HRNPAYROLL_TAXSET  
                                         ( PAY_YEAR,   
                                           PAY_MONTH,   
                                           SEQ_TAX,   
                                           EMPLID,   
                                           TAXLIST,   
                                           PAY_DATE,   
                                           TAXLIST1_AMT,   
                                           TAXLIST2_AMT,   
                                           TAXLIST3,   
                                           TAXLIST_FLAG,   
                                           ENTRY_ID,   
                                           ENTRY_DATE )  
                                  VALUES (" + year_pay + @",   
                                          " + month_pay + @",   
                                           '" + seq_tax + @"',   
                                           '" + emplid + @"',     
                                           '" + taxlist + @"',   
                                           null,   
                                           " + payroll_tax + @",   
                                           null,   
                                           '" + card_id + @"',   
                                           null,   
                                           null,   
                                           null)    ";
                            ta.Query(sqlStr);



                        }
                        ///////////////////ตารางภาษี/////////////////////////////
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
                                    ORDER BY HRNUCFTAX_RATE.MIN_RATE ASC";
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
                        sqlStr = @"   SELECT HRNPAYROLL_TAXSET.SEQ_TAX,   
                                             HRNPAYROLL_TAXSET.TAXLIST1_AMT  
                                        FROM HRNPAYROLL_TAXSET  
                                       WHERE ( HRNPAYROLL_TAXSET.PAY_YEAR =" + year_pay + @" ) AND  
                                             ( HRNPAYROLL_TAXSET.PAY_MONTH = " + month_pay + @" ) AND  
                                             ( HRNPAYROLL_TAXSET.SEQ_TAX = 'B07' ) AND  
                                             ( HRNPAYROLL_TAXSET.EMPLID = '" + emplid + "' )   ";
                        Sdt dt_B07 = ta.Query(sqlStr);
                        dt_B07.Next();
                        sqlStr = @"   SELECT HRNPAYROLL_TAXSET.SEQ_TAX,   
                                             HRNPAYROLL_TAXSET.TAXLIST1_AMT  
                                        FROM HRNPAYROLL_TAXSET  
                                       WHERE ( HRNPAYROLL_TAXSET.PAY_YEAR = " + year_pay + @"  ) AND  
                                             ( HRNPAYROLL_TAXSET.PAY_MONTH =" + month_pay + @" ) AND  
                                             ( HRNPAYROLL_TAXSET.SEQ_TAX = 'C23' ) AND  
                                             ( HRNPAYROLL_TAXSET.EMPLID ='" + emplid + "')   ";
                        Sdt dt_C23 = ta.Query(sqlStr);
                        dt_C23.Next();

                        payroll_B07 = Convert.ToDecimal(dt_B07.Rows[0]["TAXLIST1_AMT"]);
                        payroll_C23 = Convert.ToDecimal(dt_C23.Rows[0]["TAXLIST1_AMT"]);

                        sqlStr = @" SELECT HRNPAYROLL_TAXSET.SEQ_TAX,   
                                             HRNPAYROLL_TAXSET.TAXLIST1_AMT  
                                        FROM HRNPAYROLL_TAXSET  
                                       WHERE ( HRNPAYROLL_TAXSET.SEQ_TAX LIKE'A%' ) AND  
                                             ( HRNPAYROLL_TAXSET.EMPLID = '" + emplid + "' ) ORDER BY HRNPAYROLL_TAXSET.SEQ_TAX ASC ";
                        Sdt dt_a = ta.Query(sqlStr);
                        int row_tax = dt_a.GetRowCount();
                        for (int tax = 0; tax < row_tax; tax++)
                        {
                            seq_tax = dt_a.Rows[tax]["SEQ_TAX"].ToString();

                            switch (seq_tax)
                            {
                                case "A01": A_01 = setpayroll_amt * 12;
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
                                case "A13": A_13 = payroll_tax;
                                    break;
                                case "A14": payroll_tax = 0;
                                    break;
                                case "A15": payroll_tax = 0;
                                    break;
                                case "A16": payroll_tax = 0;
                                    break;
                                case "A17": payroll_tax = 0;
                                    break;
                                case "A18": payroll_tax = 0;
                                    break;
                                case "A19": payroll_tax = 0;
                                    break;
                                case "A20": payroll_tax = 0;
                                    break;
                            }
                            sqlStr = @"  UPDATE HRNPAYROLL_TAXSET  
                                         SET TAXLIST1_AMT = " + payroll_tax + @"  
                                       WHERE ( HRNPAYROLL_TAXSET.PAY_YEAR =" + year_pay + @"   ) AND  
                                             ( HRNPAYROLL_TAXSET.PAY_MONTH = " + month_pay + @" ) AND  
                                             ( HRNPAYROLL_TAXSET.SEQ_TAX ='" + seq_tax + @"'  ) AND  
                                             ( HRNPAYROLL_TAXSET.EMPLID = '" + emplid + "' )";
                            ta.Exe(sqlStr);
                        } //end for tax
                    }//end for i
                    LtServerMessage.Text = WebUtil.CompleteMessage("ประมวลผลเสร็จเรียบร้อยแล้ว");
            }
            catch (Exception ex)
            {
               LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }

            DwMain.Reset();
            DwDetail.Reset();
            DwMain.InsertRow(1);
            DwDetail.InsertRow(1);
        }

        private void GetCheckProcess91()
        {
            String sqlStr;
            Decimal year_pay = dw_process.GetItemDecimal(1, "year_pay") - 543;
            Sta ta = new Sta(sqlca.ConnectionString);
            sqlStr = @"   SELECT HRNPAYROLL_TAXSET91.PAY_YEAR,   
                                     HRNPAYROLL_TAXSET91.PAY_MONTH  
                                FROM HRNPAYROLL_TAXSET91  
                               WHERE ( HRNPAYROLL_TAXSET91.PAY_YEAR ='" + year_pay + @"'  )";
            Sdt dt_year = ta.Query(sqlStr);
            if (dt_year.Next())
            {
                Literal1.Text = @"
                                <script>
                                if( confirm('มีการคำนวณภาษีประจำปีแล้ว ต้องการคำนวณอีกหรือไม่') ){
                                    getProcess91();
                                }
                                </script>
                                ";
            }
            else
            {
                JsGetProcess91();
            }
        }

        private void JsGetProcess91()
        {
            try
            {
                String seq_tax, taxlist, pre_seq_tax, emplid, sqlStr, sqlStr1, seq_pay, pre_seq_pay, card_id = "";
                Decimal setpayroll_amt = 0, payroll_tax = 0, payroll_C23 = 0, payroll_B07 = 0, payroll_S = 0;
                Decimal C_01 = 0, C_02 = 0, C_03 = 0, C_04 = 0, C_05 = 0, C_06 = 0, C_07 = 0
                      , C_08 = 0, C_09 = 0, C_10 = 0, C_11 = 0, C_12 = 0, C_13 = 0, C_14 = 0
                      , C_15 = 0, C_16 = 0, C_17 = 0, C_18 = 0, C_19 = 0, C_20 = 0, C_21 = 0
                      , C_22 = 0, C_23 = 0;
                Decimal B_01 = 0, B_02 = 0, B_03 = 0, B_04 = 0, B_05 = 0, B_06 = 0, B_07 = 0;
                Decimal A_01 = 0, A_02 = 0, A_03 = 0, A_05 = 0, A_06 = 0, A_07 = 0, A_08 = 0
                      , A_09 = 0, A_10 = 0, A_11 = 0, A_12 = 0, A_13 = 0, A_14 = 0, A_15 = 0, A_16 = 0
                      , A_17 = 0, A_18 = 0, A_19 = 0, A_20 = 0;
                Double A_04 = 0;
                Decimal year_pay = dw_process.GetItemDecimal(1, "year_pay") - 543;
                Sta ta = new Sta(sqlca.ConnectionString);

                sqlStr = @"DELETE FROM HRNPAYROLL_TAXSET91 
                           WHERE ( HRNPAYROLL_TAXSET91.PAY_YEAR =" + year_pay + @"  )";
                Sdt dt_delete = ta.Query(sqlStr);

                sqlStr = @"  SELECT EMPLID FROM HRNMLEMPLFILEMAS ORDER BY EMPLID ASC";
                Sdt dt_emp = ta.Query(sqlStr);
                int row_emp = dt_emp.GetRowCount();
                for (int j = 0; j < row_emp; j++)
                {
                    emplid = dt_emp.Rows[j]["EMPLID"].ToString();
                    sqlStr = @" select sum(taxlist1_amt/12) as salary
                            from hrnpayroll_taxset
                            where seq_tax = 'A01' and pay_month between 1 and 12 and emplid = '" + emplid + "'";

                    Sdt dt_payroll = ta.Query(sqlStr);
                    int row_payorll = dt_payroll.GetRowCount();
                    dt_payroll.Next();
                    try
                    {
                        setpayroll_amt = dt_payroll.GetDecimal("salary");
                    }
                    catch { setpayroll_amt = 0; }

                    // select HRNPAYROLL_TAXCFG
                    sqlStr = @"  SELECT HRNPAYROLL_TAXCFG.SEQ_TAX,   
                                HRNPAYROLL_TAXCFG.TAXLIST  
                        FROM HRNPAYROLL_TAXCFG ORDER BY HRNPAYROLL_TAXCFG.SEQ_TAX ASC";
                    Sdt dt = ta.Query(sqlStr);
                    int row = dt.GetRowCount();
                    for (int i = 0; i < row; i++)
                    {
                        seq_tax = dt.Rows[i]["SEQ_TAX"].ToString();
                        taxlist = dt.Rows[i]["TAXLIST"].ToString();
                        pre_seq_tax = WebUtil.Left(seq_tax, 1);
                        ///////////////////  ข. รายการเงินที่ได้รับการยกเว้น  //////////////////////////////////////////////
                        if (pre_seq_tax == "B")
                        {
                            card_id = "";
                            switch (seq_tax)
                            {
                                case "B01": B_01 = 0; payroll_tax = B_01;
                                    break;
                                case "B02": B_02 = 0; payroll_tax = B_02;
                                    break;
                                case "B03": B_03 = 0; payroll_tax = B_03;
                                    break;
                                case "B04": B_04 = 0; payroll_tax = B_04;
                                    break;
                                case "B05": B_05 = 0; payroll_tax = B_05;
                                    break;
                                case "B06": B_06 = 0; payroll_tax = B_06;
                                    break;
                                case "B07": B_07 = B_01 + B_02 + B_03 + B_04 + B_05 + B_06;
                                    payroll_tax = B_07;
                                    break;
                            }
                        }
                        //////////////////////  ค. รายการลดหย่อนและยกเว้นหลังจากหักค่าใช้จ่าย  /////////////////////////////////
                        else if (pre_seq_tax == "C")
                        {
                            switch (seq_tax)
                            {
                                case "C01": C_01 = 40000; payroll_tax = C_01; card_id = "";
                                    break;
                                case "C02": C_02 = 30000; payroll_tax = C_02; card_id = "";
                                    break;
                                case "C03": C_03 = 0; payroll_tax = C_03; card_id = "";
                                    break;
                                case "C04": C_04 = 0; payroll_tax = C_04; card_id = "";
                                    break;
                                case "C05": C_05 = 0; payroll_tax = C_05; card_id = "";
                                    break;
                                case "C06": C_06 = 0; payroll_tax = C_06; card_id = "000000000000";
                                    break;
                                case "C07": C_07 = 0; payroll_tax = C_07; card_id = "000000000000";
                                    break;
                                case "C08": C_08 = 0; payroll_tax = C_08; card_id = "000000000000";
                                    break;
                                case "C09": C_09 = 0; payroll_tax = C_09; card_id = "000000000000";
                                    break;
                                case "C10": C_10 = 0; payroll_tax = C_10; card_id = "";
                                    break;
                                case "C11": C_11 = 0; payroll_tax = C_11; card_id = "000000000000";
                                    break;
                                case "C12": C_12 = 0; payroll_tax = C_12; card_id = "000000000000";
                                    break;
                                case "C13": C_13 = 0; payroll_tax = C_13; card_id = "000000000000";
                                    break;
                                case "C14": C_14 = 0; payroll_tax = C_14; card_id = "000000000000";
                                    break;

                                case "C15": C_15 = 0; payroll_tax = C_15; card_id = "";
                                    break;
                                case "C16": C_16 = 0; payroll_tax = C_16; card_id = "";
                                    break;
                                case "C17": C_17 = 0; payroll_tax = C_17; card_id = "";
                                    break;
                                case "C18": C_18 = 0; payroll_tax = C_18; card_id = "";
                                    break;
                                case "C19": C_19 = 0; payroll_tax = C_19; card_id = "";
                                    break;
                                case "C20": C_20 = 0; payroll_tax = C_20; card_id = "";
                                    break;
                                case "C21": C_21 = 0; payroll_tax = C_21; card_id = "";
                                    break;
                                case "C22": C_22 = 9000; payroll_tax = C_22; card_id = "";
                                    break;
                                case "C23": C_23 = C_01 + C_02 + C_03 + C_04 + C_05 + C_06 + C_07 + C_08 + C_09 + C_10 + C_11 + C_12 + C_13 + C_14 + C_15 + C_16 + C_17 + C_18 + C_19 + C_20 + C_21 + C_22;
                                    payroll_tax = C_23; card_id = "";
                                    break;
                            }

                        }
                        //////////////////////////// ส่วนคู่สมรส  /////////////////////////////////////////////////
                        else if (pre_seq_tax == "D")
                        {
                            card_id = "";
                            switch (seq_tax)
                            {
                                case "D01": payroll_tax = 0;
                                    break;
                                case "D02": payroll_tax = 0;
                                    break;
                                case "D03": payroll_tax = 0;
                                    break;
                                case "D04": payroll_tax = 0;
                                    break;
                                case "D05": payroll_tax = 0;
                                    break;
                                case "D06": payroll_tax = 0;
                                    break;
                                case "D07": payroll_tax = 0;
                                    break;
                            }
                        }
                        sqlStr = @" INSERT INTO HRNPAYROLL_TAXSET91  
                                    ( PAY_YEAR, 
                                    PAY_MONTH,  
                                    SEQ_TAX,   
                                    EMPLID,   
                                    TAXLIST,   
                                    PAY_DATE,   
                                    TAXLIST1_AMT,   
                                    TAXLIST2_AMT,   
                                    TAXLIST3,   
                                    TAXLIST_FLAG,   
                                    ENTRY_ID,   
                                    ENTRY_DATE )  
                            VALUES (" + year_pay + @",  
                                    0, 
                                    '" + seq_tax + @"',   
                                    '" + emplid + @"',     
                                    '" + taxlist + @"',   
                                    null,   
                                    " + payroll_tax + @",   
                                    null,   
                                    '" + card_id + @"',   
                                    null,   
                                    null,   
                                    null)    ";
                        ta.Query(sqlStr);
                    }
                    ///////////////////ตารางภาษี/////////////////////////////
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
                            ORDER BY HRNUCFTAX_RATE.MIN_RATE ASC";
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
                    sqlStr = @"   SELECT HRNPAYROLL_TAXSET91.SEQ_TAX,   
                                        HRNPAYROLL_TAXSET91.TAXLIST1_AMT  
                                FROM HRNPAYROLL_TAXSET91  
                                WHERE ( HRNPAYROLL_TAXSET91.PAY_YEAR =" + year_pay + @" ) AND  
                                        ( HRNPAYROLL_TAXSET91.SEQ_TAX = 'B07' ) AND  
                                        ( HRNPAYROLL_TAXSET91.EMPLID = '" + emplid + "' )   ";
                    Sdt dt_B07 = ta.Query(sqlStr);
                    dt_B07.Next();
                    sqlStr = @"   SELECT HRNPAYROLL_TAXSET91.SEQ_TAX,   
                                        HRNPAYROLL_TAXSET91.TAXLIST1_AMT  
                                FROM HRNPAYROLL_TAXSET91  
                                WHERE ( HRNPAYROLL_TAXSET91.PAY_YEAR = " + year_pay + @"  ) AND  
                                        ( HRNPAYROLL_TAXSET91.SEQ_TAX = 'C23' ) AND  
                                        ( HRNPAYROLL_TAXSET91.EMPLID ='" + emplid + "')   ";
                    Sdt dt_C23 = ta.Query(sqlStr);
                    dt_C23.Next();

                    payroll_B07 = Convert.ToDecimal(dt_B07.Rows[0]["TAXLIST1_AMT"]);
                    payroll_C23 = Convert.ToDecimal(dt_C23.Rows[0]["TAXLIST1_AMT"]);

                    sqlStr = @" SELECT HRNPAYROLL_TAXSET91.SEQ_TAX,   
                                        HRNPAYROLL_TAXSET91.TAXLIST1_AMT  
                                FROM HRNPAYROLL_TAXSET91  
                                WHERE ( HRNPAYROLL_TAXSET91.SEQ_TAX LIKE'A%' ) AND  
                                        ( HRNPAYROLL_TAXSET91.EMPLID = '" + emplid + "' ) ORDER BY HRNPAYROLL_TAXSET91.SEQ_TAX ASC ";
                    Sdt dt_a = ta.Query(sqlStr);
                    int row_tax = dt_a.GetRowCount();
                    for (int tax = 0; tax < row_tax; tax++)
                    {
                        seq_tax = dt_a.Rows[tax]["SEQ_TAX"].ToString();

                        switch (seq_tax)
                        {
                            case "A01": A_01 = setpayroll_amt;
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
                                    payroll_tax = Convert.ToDecimal(t_amt2);
                                }
                                else if (rate_amt > max_rate3 || rate_amt < min_rate4)
                                {
                                    A_12 = 20; //ร้อยละ
                                    P_amt2 = max_rate2;
                                    max_level = max_rate3 - max_rate2;//จำนวนสูงสุดของขั้น
                                    P_amt3 = rate_amt - P_amt2;//เงินได้สุทธิแต่ละขั้น
                                    t_amt3 = P_amt3 * 0.2;//ภาษีเงินได้
                                    payroll_tax = Convert.ToDecimal(t_amt3);
                                }
                                else if (rate_amt > max_rate4 || rate_amt < min_rate5)
                                {
                                    A_12 = 30; //ร้อยละ
                                    P_amt3 = max_rate3;
                                    max_level = max_rate4 - max_rate3;//จำนวนสูงสุดของขั้น
                                    P_amt4 = rate_amt - P_amt3;//เงินได้สุทธิแต่ละขั้น
                                    t_amt4 = P_amt4 * 0.3;//ภาษีเงินได้
                                    payroll_tax = Convert.ToDecimal(t_amt4);
                                }
                                else if (rate_amt > max_rate5)
                                {
                                    A_12 = 37; //ร้อยละ
                                    P_amt4 = max_rate4;
                                    max_level = max_rate4 - max_rate5;//จำนวนสูงสุดของขั้น
                                    P_amt5 = rate_amt - P_amt4;//เงินได้สุทธิแต่ละขั้น
                                    t_amt5 = P_amt5 * 0.37;//ภาษีเงินได้
                                    payroll_tax = Convert.ToDecimal(t_amt5);
                                }
                                break;
                            case "A13": A_13 = payroll_tax;
                                break;
                            case "A14": payroll_tax = 0;
                                break;
                            case "A15": payroll_tax = 0;
                                break;
                            case "A16": payroll_tax = 0;
                                break;
                            case "A17": payroll_tax = 0;
                                break;
                            case "A18": payroll_tax = 0;
                                break;
                            case "A19": payroll_tax = 0;
                                break;
                            case "A20": payroll_tax = 0;
                                break;
                        }
                        sqlStr = @"  UPDATE HRNPAYROLL_TAXSET91  
                                    SET TAXLIST1_AMT = " + payroll_tax + @"  
                                WHERE ( HRNPAYROLL_TAXSET91.PAY_YEAR =" + year_pay + @"   ) AND  
                                        ( HRNPAYROLL_TAXSET91.SEQ_TAX ='" + seq_tax + @"'  ) AND  
                                        ( HRNPAYROLL_TAXSET91.EMPLID = '" + emplid + "' )";
                        ta.Exe(sqlStr);
                    } //end for tax
                }//end for i

                LtServerMessage.Text = WebUtil.CompleteMessage("ประมวลผลเสร็จเรียบร้อยแล้ว");
            }//end if dt_month
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }

            DwMain.Reset();
            DwDetail.Reset();
            DwMain.InsertRow(1);
            DwDetail.InsertRow(1);
        
        }

        private void GetNameId()
        {
            try
            {
                Decimal year_pay = dw_process.GetItemDecimal(1, "year_pay")-543;
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
            catch { }

        }


        public void SaveWebSheet()
        {
            try
            {
                DwDetail.UpdateData();
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



    }
}
