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
    public partial class w_sheet_hr_reword : PageWebSheet, WebSheet
    {
        protected String getNameId;
        protected String getProcess;
        private int year;
        private int month;



        public void InitJsPostBack()
        {
            getNameId = WebUtil.JsPostBack(this, "getNameId");
            getProcess = WebUtil.JsPostBack(this, "getProcess");
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();
            dw_process.SetTransaction(sqlca);
            DwMain.SetTransaction(sqlca);
            DwDetail.SetTransaction(sqlca);

            if (!IsPostBack)
            {
                dw_process.InsertRow(0);

                dw_process.SetItemDecimal(1, "year_pay", DateTime.Now.Date.Year);
                DwMain.Visible = false;
                DwDetail.Visible = false;
                //DwMain.InsertRow(0);
                //DwDetail.InsertRow(0);

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

        }

        private void GetProcess()
        {
            Decimal year = dw_process.GetItemDecimal(1, "year_pay");
            int seq_no;
            String emplid, sqlStr, empl_name, begin_date;
            Double setpayroll_amt, m_bk, o_bk = 0;
            Sta ta = new Sta(sqlca.ConnectionString);
            try
            {
                sqlStr = @"  SELECT HRN_BKREWARD.EMPLID  
                                FROM HRN_BKREWARD  
                               WHERE ( HRN_BKREWARD.YEAR =" + year + @"  )";
                Sdt dt_row = ta.Query(sqlStr);
                if (dt_row.Next())
                {
                    LtServerMessage.Text = WebUtil.CompleteMessage("มีการประมวลไว้แล้ว");
                }
                else
                {

                   
                    sqlStr = @"  SELECT HRNPAYROLL_SET.EMPLID,   
                                    HRNPAYROLL_SET.SEQ_PAY,  HRNMLEMPLFILEMAS.EMPLBEGNDATE , 
                                    ( HRNMLEMPLFILEMAS.EMPLFIRSNAME||' '||HRNMLEMPLFILEMAS.EMPLLASTNAME)as emp_name, 
                                    HRNPAYROLL_SET.SETPAYROLL_AMT  
                              FROM  HRNMLEMPLFILEMAS,   
                                    HRNPAYROLL_SET  
                              WHERE ( HRNPAYROLL_SET.EMPLID = HRNMLEMPLFILEMAS.EMPLID )  AND   HRNPAYROLL_SET.SEQ_PAY='R01'
                              ORDER BY HRNPAYROLL_SET.EMPLID ASC";
                    Sdt dt = ta.Query(sqlStr);
                    int row = dt.GetRowCount();
                    for (int i = 0; i < row; i++)
                    {
                        seq_no = i + 1;
                        try { setpayroll_amt = Convert.ToDouble(dt.Rows[i]["SETPAYROLL_AMT"]); }
                        catch { setpayroll_amt = 0; }

                        try { begin_date = dt.Rows[i]["EMPLBEGNDATE"].ToString(); }
                        catch { begin_date = ""; }
                        emplid = dt.Rows[i]["EMPLID"].ToString();
                        empl_name = dt.Rows[i]["emp_name"].ToString();
                        //ขาด functino deffyear เพื่อ คำนวณ year * เงินเดือน *%ค่าบำเหน็ด
                        Double countdate = DateDiff(Convert.ToDateTime(begin_date), DateTime.Now.Date).TotalDays;
                        Int32 year_amount = Convert.ToInt32(countdate / 365);

                        //int month_amount = countdate % 365;
                        //Double month_a = month_amount / 30;
                        m_bk = Convert.ToDouble(year_amount) * setpayroll_amt * 0.2;
                        Decimal year_befor = year - 1;
                        sqlStr = @"  SELECT HRN_BKREWARD.O_BK,   
                                        HRN_BKREWARD.EMPLID  
                                 FROM HRN_BKREWARD  
                                 WHERE HRN_BKREWARD.YEAR = " + year_befor + " AND HRN_BKREWARD.EMPLID ='" + emplid + "'  ";
                        Sdt dt_befor = ta.Query(sqlStr);
                        dt_befor.Next();
                        try
                        {
                            o_bk = dt_befor.GetDouble("O_BK") + m_bk;
                        }
                        catch { o_bk = m_bk; }

                        sqlStr = @"    INSERT INTO HRN_BKREWARD  
                                                 ( YEAR,   
                                                   SEQ_NO,   
                                                   EMPLID,   
                                                   EMPNAME,   
                                                   EBEGIN,   
                                                   SALARY,   
                                                   M_BK,   
                                                   O_BK,   
                                                   ACCOUNT,   
                                                   EMPTIME,   
                                                   APP_STATUS,   
                                                   APP_DATE,   
                                                   EXPEND_CODE )  
                                          VALUES ( " + year + @",   
                                                   " + seq_no + @",   
                                                   '" + emplid + @"',   
                                                   '" + empl_name + @"',   
                                                   null,   
                                                   " + setpayroll_amt + @",
                                                   " + m_bk + @",   
                                                   " + o_bk + @",                                                       
                                                   null,   
                                                   " + year_amount + @",   
                                                   null,   
                                                   null,   
                                                   null )   ";
                        ta.Query(sqlStr);


                    }
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }

            DwMain.Visible = false;
            DwDetail.Visible = true;

            DwDetail.Retrieve(year);
        }
        public TimeSpan DateDiff(DateTime strStart, DateTime strFinish)
        {
            //::function name ; DateDiff
            //::functional ; คำนวณหาจำนวนวันที่ต่างกันจากวันที่เริ่มต้น ถึงวันที่สิ้นสุด

            //::เขียน/แก้ไขเมื่อวันที่ 28-04-2549
            //::เขียน/แก้ไขโดย nopmunintr

            //*** parameter list ***
            //::var1 --> strStart = วันที่เริ่มต้น
            //::var2 --> strFinish = วันที่สิ้นสุด
            //::remarks strStart,strFinish อยู่ในรูปแบบวันที่ที่เป็น string เท่านั้น เช่น 01/01/2549 หรือ 01/01/2006

            //*** Return Type ***
            //::returntype --> Result ตัวแปรประเภท TimeSpan
            //::remarks  ผลลัพธ์ที่ได้จะเป็นจำนวนวันเท่านั้น เช่น 365 วัน , 2450 วัน เป็นต้น 

            System.DateTime t1 = strStart;
            System.DateTime t2 = strFinish;
            //TimeSpan Result = Nothing;
            try
            {
                return t2.Subtract(t1);
            }
            catch (Exception ex)
            {
                return new System.TimeSpan(0, 0, 0, 0, 0);
            }
        }
      

        private void GetNameId()
        {
            try
            {
                Decimal year_pay = dw_process.GetItemDecimal(1, "year_pay");
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
            //try
            //{
            //    DwDetail.UpdateData();
            //    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลสำเร็จ");
            //}
            //catch (Exception ex)
            //{
            //    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            //}
        }

        public void WebSheetLoadEnd()
        {
        }


    }
}