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
    public partial class w_sheet_hr_saladea : PageWebSheet, WebSheet
    {
        protected String getNameId, callCountAge, checkAgeChild;
        private int year;
        private int month;

        public void InitJsPostBack()
        {
            getNameId = WebUtil.JsPostBack(this, "getNameId");
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();
            DwMain.SetTransaction(sqlca);
            DwDetail.SetTransaction(sqlca);
            DwTranacc.SetTransaction(sqlca);

            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                DwDetail.InsertRow(0);
                DwTranacc.InsertRow(0);
            }
            else
            {
                try
                {
                    DwMain.RestoreContext();
                    DwDetail.RestoreContext();
                    DwTranacc.RestoreContext();
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
        }

        private decimal CheckAgeChild(int age)
        {
            //ฟังก์ชั่นคำนวณ rate ค่าสวัสดิการบุตร
            Sta ta = new Sta(sqlca.ConnectionString);
            String sqlStr;
            decimal a = 0;
            sqlStr = @"SELECT hrnmlemplchild.over_age, hrnmlemplchild.over_age_amt
                        FROM hrnmlemplchild
                        ORDER BY hrnmlemplchild.seq_no ASC";
            Sdt dt_rate = ta.Query(sqlStr);
            int row_rate = dt_rate.GetRowCount();
            for (int t = 0; t < row_rate; t++)
            {
                if (age <= Convert.ToInt32(dt_rate.Rows[t]["over_age"]))
                {
                    a = Convert.ToDecimal(dt_rate.Rows[t]["over_age_amt"]);
                    break;
                }
            }
            return a;
        }

        private decimal CallCountAge()
        {
            //ดึงวันเกิดบุตรมาคำนวณอายุ
            Sta ta = new Sta(sqlca.ConnectionString);
            String empid = DwMain.GetItemString(1, "empid").Trim();
            //String famirela= DwDetail.GetItemString(1, "famirela");
            String sqlStr;
            DateTime birthdate = new DateTime();
            DateTime todate = new DateTime();
            todate = DateTime.Now;
            Decimal child_amt;
            int age;
            sqlStr = @"select birthdate
                     from hrnmlemplfamidea
                     where emplid = '" + empid + "' and famirela = '003'";
            Sdt dt_child_age = new Sdt();
            dt_child_age = ta.Query(sqlStr);
            dt_child_age.Next();
            birthdate = dt_child_age.GetDate("birthdate");

            int birthdate_dd = Convert.ToInt32(birthdate.Day.ToString());
            int birthdate_mm = Convert.ToInt32(birthdate.Month.ToString());
            int birthdate_yyyy = Convert.ToInt32(birthdate.Year.ToString());

            int todate_dd = Convert.ToInt32(todate.Day.ToString());
            int todate_mm = Convert.ToInt32(todate.Month.ToString());
            int todate_yyyy = Convert.ToInt32(todate.Year.ToString());

            //คำนวณอายุ
            age = todate_yyyy - birthdate_yyyy;
            if (todate_mm < (birthdate_mm - 1))
            {
                age--;
            }
            if (((birthdate_mm - 1) == todate_mm) && (todate_dd < birthdate_dd))
            {
                age--;
            }

            //child_amt = CheckAgeChild(age);
            //  return child_amt;
            return 0;
        }

        private void GetNameId()
        {
            try
            {
                Double provident, socisepercent;
                Decimal salary, a1, s1, posi_amt, child_amt;
                String empid = DwMain.GetItemString(1, "empid").Trim();
                String SEQ_PAY;
                Sta ta = new Sta(sqlca.ConnectionString);
                String sqlStr;
                sqlStr = @"select salary
                            from hrnmlemplfilemas
                            where emplid = '" + empid + "' ";
                Sdt dt_salary_old = new Sdt();
                dt_salary_old = ta.Query(sqlStr);
                dt_salary_old.Next();
                salary = dt_salary_old.GetDecimal("salary");
                String sqlStrP15 = @"select emplprovident from HRNMLEMPLINFODEA where emplid = '" + empid + "' ";
                Sdt dt_provident = new Sdt();
                dt_provident = ta.Query(sqlStrP15);
                dt_provident.Next();
                try
                {

                    provident = dt_provident.GetDouble("emplprovident");
                }
                catch { provident = 0; }

                s1 = (Convert.ToDecimal(salary) * Convert.ToDecimal(provident));

                String sqlStrP12 = @"select socisepercent from HRNMLEMPLINFODEA where emplid = '" + empid + "' ";
                Sdt dt_socisepercent = new Sdt();
                dt_socisepercent = ta.Query(sqlStrP12);
                dt_socisepercent.Next();
                try
                {

                    socisepercent = dt_socisepercent.GetDouble("socisepercent");
                }
                catch { socisepercent = 0; }

                if (Convert.ToDecimal(salary) > 15000)
                {
                    a1 = 15000 * Convert.ToDecimal(socisepercent);
                }
                else
                    a1 = (Convert.ToDecimal(salary) * Convert.ToDecimal(socisepercent));

                sqlStr = @" SELECT * 
                            FROM HRNPAYROLL_SET  
                           WHERE HRNPAYROLL_SET.EMPLID = '" + empid + "' ";
                Sdt dt_row = ta.Query(sqlStr);
                if (dt_row.GetRowCount() > 0)
                {
                    /*Insert hrpayroll_salary ที่ไม่มีใน hrpayroll_set*/
                    sqlStr = @" INSERT INTO HRNPAYROLL_SET  
                                     ( SET_NO,   
                                       SEQ_PAY,   
                                       EMPLID,   
                                       SETPAYROLL_AMT,   
                                       ENTRY_ID,   
                                       ENTRY_DATE )  
                                select 1 , hsr.seq_pay , '" + empid + @"', 0 , null , null
                                from hrnpayroll_salary hsr
                                where not exists (  select 1 from hrnpayroll_set hs 
                                                    where hs.seq_pay = hsr.seq_pay 
                                                    and hs.emplid = '" + empid + @"' )
                                ";

                    ta.Exe(sqlStr);
                }
                else
                {
                    sqlStr = @" SELECT HRNPAYROLL_SALARY.SEQ_PAY
                            FROM HRNPAYROLL_SALARY";
                    Sdt dt_salary = ta.Query(sqlStr);
                    int r = dt_salary.GetRowCount();
                    for (int i = 0; i < r; i++)
                    {
                        SEQ_PAY = dt_salary.Rows[i]["SEQ_PAY"].ToString();

                        sqlStr = @" INSERT INTO HRNPAYROLL_SET  
                                     ( SET_NO,   
                                       SEQ_PAY,   
                                       EMPLID,   
                                       SETPAYROLL_AMT,   
                                       ENTRY_ID,   
                                       ENTRY_DATE )  
                              VALUES ( 1,   
                                       '" + SEQ_PAY + @"',   
                                       '" + empid + @"',   
                                       null,  
                                       null,   
                                       null) ";

                        ta.Exe(sqlStr);
                    }
//                    sqlStr = @"  UPDATE HRNPAYROLL_SET
//                             SET SETPAYROLL_AMT = '" + salary + "' WHERE SEQ_PAY = 'R01' and emplid ='" + empid + "' ";
//                    Sdt dt_update_salary = ta.Query(sqlStr);

                }
                sqlStr = @"  SELECT HRNMLEMPLFILEMAS.EMPLID,   
                                         HRNMLEMPLFILEMAS.EMPLFIRSNAME,   
                                         HRNMLEMPLFILEMAS.EMPLLASTNAME  
                                    FROM HRNMLEMPLFILEMAS WHERE HRNMLEMPLFILEMAS.EMPLID='" + empid + "'";
                Sdt dt = ta.Query(sqlStr);
                dt.Next();
                DwMain.SetItemString(1, "emp_name", dt.GetString("EMPLFIRSNAME"));
                DwMain.SetItemString(1, "emp_lastname", dt.GetString("EMPLLASTNAME"));

                sqlStr = @"  select posiwexpen
                             from hrucfposition_w a , hrucfposition_p b, hrnmlemplfilemas c
                             where c.postid = b.posipid and b.posipid = a.posiwid and c.emplid='" + empid + "' ";
                Sdt dt_update_posi = ta.Query(sqlStr);
                dt_update_posi.Next();
                posi_amt = dt_update_posi.GetDecimal("posiwexpen");

//                sqlStr = @"  UPDATE HRNPAYROLL_SET
//                             SET SETPAYROLL_AMT = '" + salary + "' WHERE SEQ_PAY = 'R01' and emplid ='" + empid + "' ";
               //Sdt dt_update_salary1 = ta.Query(sqlStr);
                //                sqlStr = @"  UPDATE HRNPAYROLL_SET
                //                             SET SETPAYROLL_AMT = '" + s1 + "' WHERE SEQ_PAY = 'P15' and emplid ='" + empid + "' ";
                //                Sdt dt_update_s1 = ta.Query(sqlStr);

                //                sqlStr = @"  UPDATE HRNPAYROLL_SET
                //                             SET SETPAYROLL_AMT = '" + a1 + "' WHERE SEQ_PAY = 'P12' and emplid ='" + empid + "' ";
                //                Sdt dt_update_a1 = ta.Query(sqlStr);

                sqlStr = @"  UPDATE HRNPAYROLL_SET
                             SET SETPAYROLL_AMT = '" + posi_amt + "' WHERE SEQ_PAY = 'R05' and emplid ='" + empid + "' ";
                Sdt dt_update_posi1 = ta.Query(sqlStr);

                child_amt = CallCountAge();
                //                sqlStr = @"  UPDATE HRNPAYROLL_SET
                //                             SET SETPAYROLL_AMT = '" + child_amt + "' WHERE SEQ_PAY = 'R04' and emplid ='" + empid + "' ";
                //                Sdt dt_update_child = ta.Query(sqlStr);
                DwDetail.Retrieve(empid);
                DwTranacc.Retrieve(empid);
            }
            catch { }
        }

        private void SalaryRetrieveList()
        {
            //ตารางหลักอยุ่ที่ salamas ที่ทีข้อมูลตอนแรกตั้งแต่เพิ่มข้อมูลพนักงาน
            //ตารางเงินเดือนอยุ่ที่ saladea  ดึงรายชื่อมาจาก salamas

            //filter from dw
            //compare with master sala
            //nsert
            try
            {
                year = Convert.ToInt32(DwMain.GetItemString(1, "year_pay"));
                month = Convert.ToInt32(DwMain.GetItemString(1, "month_pay"));
                DwDetail.Reset();
                DwDetail.Retrieve(year, month, "01"); // year, month, period(optional)
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }

            //all row
            ArrayList a = new ArrayList();
            Sta ta = new Sta(sqlca.ConnectionString);
            try
            {
                //ดึงข้อมูลหลักมาคิดเงินเดือน
                String sql = @"select  hrnmlemplfilemas.emplcode, hrnmlemplfilemas.empltitl, hrnmlemplfilemas.emplfirsname, hrnmlemplfilemas.empllastname, hrnmlemplinfodea.emplsala ,hrnmlemplinfodea.emplprovident, hrnmlemplinfodea.socisepercent, hrnmlemplfilemas.emplid
                                from hrnmlemplfilemas, hrnmlemplinfodea 
                                where hrnmlemplfilemas.emplid = hrnmlemplinfodea.emplid
                                order by emplid";
                Sdt dt = ta.Query(sql);
                for (int i = 0; i < dt.GetRowCount(); i++)
                {
                    String tCode = dt.Rows[i]["emplcode"].ToString();
                    String tTitle = dt.Rows[i]["empltitl"].ToString();
                    String tFirstname = dt.Rows[i]["emplfirsname"].ToString();
                    String tLastname = dt.Rows[i]["empllastname"].ToString();
                    String tSala = dt.Rows[i]["emplsala"].ToString();
                    String tEmplprovident = dt.Rows[i]["emplprovident"].ToString();
                    String tSocisepercent = dt.Rows[i]["socisepercent"].ToString();
                    String tEmplid = dt.Rows[i]["emplid"].ToString();
                    String[] arr = new String[] { tCode, tTitle, tFirstname, tLastname, tSala, tEmplprovident, tSocisepercent, tEmplid };

                    //เช็ค ว่า คนที่ยังไม่ได้เข้าเงินเดือนเดือนนี้
                    if (DwDetail.RowCount > 0)
                    {
                        for (int x = 0; x < DwDetail.RowCount; x++)
                        {
                            if (DwDetail.IsItemNull(x, "employe_no"))
                            {
                                a.Add(arr);
                                // a.Add(emplcode);
                            }
                        }
                    }
                    else
                    {
                        a.Add(arr);
                        //a.Add(emplcode);
                    }
                }

                // แสดงรายชื่อ ที่ยังไม่มีรายการเงินเดือนใหม่
                for (int i = 0; i < a.Count; i++)
                {
                    DwDetail.InsertRow(0);
                    int lastRow = DwDetail.RowCount;
                    String[] arr_set_tmp = (String[])a[i];
                    String ecode = arr_set_tmp.GetValue(0).ToString();
                    String etitle = arr_set_tmp.GetValue(1).ToString();
                    String efirstname = arr_set_tmp.GetValue(2).ToString();
                    String elastname = arr_set_tmp.GetValue(3).ToString();
                    String esala = arr_set_tmp.GetValue(4).ToString();
                    String eEmplprovident = arr_set_tmp.GetValue(5).ToString();
                    String eSocisepercent = arr_set_tmp.GetValue(6).ToString();
                    String eEmplid = arr_set_tmp.GetValue(7).ToString();
                    DwDetail.SetItemString(lastRow, "employe_no", eEmplid);
                    DwDetail.SetItemString(lastRow, "empltitl", etitle);
                    DwDetail.SetItemString(lastRow, "emplfirsname", efirstname);
                    DwDetail.SetItemString(lastRow, "empllastname", elastname);
                    DwDetail.SetItemDecimal(lastRow, "in1", Convert.ToDecimal(esala));
                    DwDetail.SetItemDecimal(lastRow, "emplprovident", Convert.ToDecimal(eEmplprovident));
                    DwDetail.SetItemDecimal(lastRow, "socisepercent", Convert.ToDecimal(eSocisepercent));
                    DwDetail.SetItemDecimal(lastRow, "year_pay", year);
                    DwDetail.SetItemDecimal(lastRow, "month_pay", month);
                    DwDetail.SetItemString(lastRow, "emplid", eEmplid);
                    try
                    {
                        decimal c8 = DwDetail.GetItemDecimal(lastRow, "c_cut8");
                        decimal c9 = DwDetail.GetItemDecimal(lastRow, "c_cut9");
                        DwDetail.SetItemDecimal(lastRow, "cut8", c8);
                        DwDetail.SetItemDecimal(lastRow, "cut9", c9);
                    }
                    catch { }

                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
            ta.Close();

        }

        public void SaveWebSheet()
        {
            try
            {
                DwDetail.UpdateData();
                DwTranacc.UpdateData();
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
