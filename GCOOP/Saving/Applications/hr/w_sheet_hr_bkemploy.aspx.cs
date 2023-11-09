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
    public partial class w_sheet_hr_bkemploy : PageWebSheet , WebSheet 
    {
        private DwThDate tDwMain;
        protected String postCalculate;
        protected String postNewClear;

        //=======================
        private void JspostCalculate()
        {
            if (Dw_main.RowCount > 0)
            {
                String prob_tdate, current_tdate = null;
                int p_day, p_month, p_year, c_day, c_month, c_year;
                

                for (int i = 1; i <= Dw_main.RowCount; i++)
                {
                    Decimal all_month = 0, all_year = 0;
                    Decimal all_money = 0, salary_employ = 0, all_tax = 0, commu = 0;

                    prob_tdate = Dw_main.GetItemString(i, "prob_tdate");
                    current_tdate = Dw_main.GetItemString(i, "current_tdate");

                    p_day = int.Parse(WebUtil.Left(prob_tdate, 2));
                    p_month = int.Parse(WebUtil.Mid(prob_tdate, 2, 2));
                    p_year = int.Parse(WebUtil.Mid(prob_tdate, 4, 4));

                    c_day = int.Parse(WebUtil.Left(current_tdate, 2));
                    c_month = int.Parse(WebUtil.Mid(current_tdate, 2, 2));
                    c_year = int.Parse(WebUtil.Mid(current_tdate, 4, 4));

                    if (c_day >= 15)
                    {
                        

                        all_month = 1;

                        if (c_month >= p_month)
                        {
                            all_month = all_month + (c_month - p_month);
                            all_year = c_year - p_year;
                            all_year = all_year * 12;
                            all_month = all_month + all_year; //จำนวนเดือนสุทธิ
                        }
                        else
                        {
                            all_month = (12 - p_month) + c_month;
                            all_year = (c_year - p_year) - 1;
                            all_year = all_year * 12;
                            all_month = all_month + all_year; //จำนวนเดือนสุทธิ
                        }
                    }
                    else
                    {
                        if (c_month >= p_month)
                        {
                            all_month = all_month + (c_month - p_month);
                            all_year = c_year - p_year;
                            all_year = all_year * 12;
                            all_month = all_month + all_year; //จำนวนเดือนสุทธิ
                        }
                        else
                        {
                            all_month = (12 - p_month) + c_month;
                            all_year = (c_year - p_year) - 1;
                            all_year = all_year * 12;
                            all_month = all_month + all_year; //จำนวนเดือนสุทธิ
                        }
                    }

                    Dw_main.SetItemDecimal(i, "workage_employ", all_month);
                    salary_employ = Dw_main.GetItemDecimal(i, "salary_employ");
                    all_money = (all_month * salary_employ) / 12;
                    Dw_main.SetItemDecimal(i, "employ_amt", all_money);
                    commu = salary_employ * 10;
                    Dw_main.SetItemDecimal(i, "commu_amt", commu);
                    //all_tax = (all_money * 7) / 100;
                    all_tax = CallTaxEmploy(all_money, commu, all_month, salary_employ);
                    Dw_main.SetItemDecimal(i, "tax_employ", all_tax);
                }
            }
        }

        private void JspostNewClear()
        {
            int GetYear = DateTime.Now.Year;
            GetYear = GetYear + 543;
            txt_year.Text = Convert.ToString(GetYear);
            Dw_main.Reset();
        }

        //=======================
        #region WebSheet Members

        public void InitJsPostBack()
        {
            postCalculate = WebUtil.JsPostBack(this, "postCalculate");
            postNewClear = WebUtil.JsPostBack(this, "postNewClear");
            //===============
            //====================================
            tDwMain = new DwThDate(Dw_main, this);
            tDwMain.Add("currentdate", "current_tdate");
            tDwMain.Add("probdate", "prob_tdate");
            tDwMain.Eng2ThaiAllRow();
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();
            Dw_main.SetTransaction(sqlca);

            if (!IsPostBack)
            {
                JspostNewClear();
                tDwMain.Eng2ThaiAllRow();
            }
            else
            {
                this.RestoreContextDw(Dw_main);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postCalculate")
            {
                JspostCalculate();
            }
            else if (eventArg == "postNewClear")
            {
                JspostNewClear();
            }
        }

        public void SaveWebSheet()
        {
                //Update Record
            if (Dw_main.RowCount > 0)
            {
                String Getyear = txt_year.Text;
                
                for (int i = 1; i <= Dw_main.RowCount; i++)
                {
                    String emplid = Dw_main.GetItemString(i, "emplid").Trim();
                    
                    Decimal workage_employ = Dw_main.GetItemDecimal(i, "workage_employ");
                    Decimal employ_amt = Dw_main.GetItemDecimal(i, "employ_amt");
                    Decimal tax_employ = Dw_main.GetItemDecimal(i, "tax_employ");
                    Decimal commu_amt = Dw_main.GetItemDecimal(i, "commu_amt");
                    try
                    {
                        Sta ta = new Sta(sqlca.ConnectionString);
                        String sql = @"Update hrnml_bkemploy set workage_employ = " + workage_employ + ", employ_amt = " + employ_amt + ",tax_employ = " + tax_employ + " ,commu_amt= " + commu_amt + " where emplid = '" + emplid + "'";
                        ta.Exe(sql);
                        ta.Close();
                    }
                    catch (Exception ex)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถบันทึกข้อมูลได้" + ex.Message);
                    }
                }
                Dw_main.Retrieve(Getyear);
                tDwMain.Eng2ThaiAllRow();
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเสร็จเรียบร้อยแล้ว");
            }
        }

        public void WebSheetLoadEnd()
        {
            Dw_main.SaveDataCache();
        }

        #endregion

        protected void B_ok_Click(object sender, EventArgs e)
        {
            String GetYear = null;
            String entry_dd, entry_mm, entry_yyyy = null;
            GetYear = txt_year.Text;
            DateTime ENTRY_DATE = new DateTime();
            //SetDate
            ENTRY_DATE = DateTime.Now;
            entry_dd = ENTRY_DATE.Day.ToString();
            entry_mm = ENTRY_DATE.Month.ToString();
            entry_yyyy = ENTRY_DATE.Year.ToString();
            //======
            if (GetYear == null || GetYear == "")
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกปีที่เกษียณ");
            }
            else
            {
                //ตรวจสอบว่า ปีเกษียณ นี้มีในตาราง hrnml_bkemploy มีอยู่หรือไม่
                Sta ta3 = new Sta(sqlca.ConnectionString);
                String sql3 = @"select year_employ from hrnml_bkemploy where year_employ = '" + GetYear + "'";
                Sdt dt3 = ta3.Query(sql3);
                if (dt3.Next())
                {
                    // มีข้อมูลปีเกษียณนั้น ๆ อยู่แล้ว
                    Dw_main.Retrieve(GetYear);
                    tDwMain.Eng2ThaiAllRow();
                }
                else
                {
                    Sta ta1 = new Sta(sqlca.ConnectionString);
                    try
                    {
                        // ตรวจสอบคนที่อายุตั้งแต่ 65 ปี ขึ้นไป
                        String sql1 = @"select emplid, emplprobdate, salary from hrnmlemplfilemas where months_between(sysdate,emplbirtdate) >= 780";
                        Sdt dt1 = ta1.Query(sql1);
                        if (dt1.Next())
                        {
                                 //new record
                            try
                            {
                                Sta ta2 = new Sta(sqlca.ConnectionString);
                                String sql2 = @"INSERT INTO hrnml_bkemploy (emplid, year_employ, salary_employ, workage_employ, employ_amt, tax_employ, entry_id, entry_date, currentdate, probdate)";
                                sql2 += "select emplid , '" + GetYear + "' , salary ,0 ,0 ,0 , '" + state.SsUsername + "' , to_date('" + entry_dd + "/" + entry_mm + "/" + entry_yyyy + "' ,'dd/mm/yyyy'),to_date('" + entry_dd + "/" + entry_mm + "/" + entry_yyyy + "' ,'dd/mm/yyyy'),emplprobdate  from hrnmlemplfilemas where months_between ( sysdate , emplbirtdate ) >= 714.5";
                                ta2.Exe(sql2);
                                ta2.Close();
                                Dw_main.Retrieve(txt_year.Text);
                                tDwMain.Eng2ThaiAllRow();

                            }
                            catch (Exception ex)
                            {
                                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถบันทึกข้อมูลได้" + ex.Message);
                            }  
                        }
                        else
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูลเจ้าหน้าที่ใกล้เกษียณประจำปี :" + txt_year.Text);
                            JspostNewClear();
                        }
                    }
                    catch(Exception ex)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
                    }
                        ta1.Close();
                       ta3.Close();
                }
            }
        }

        protected Decimal CallTaxEmploy(decimal pension, decimal commu, decimal all_month, decimal salary)
        {
            decimal all_tax = 0;
            //คำนวณเงินได้
            decimal sum_a = pension + commu;

            //คำนวณเงินเดือนรับจากนายจ้างเดือนสุดท้าย
            decimal sum_b1 = salary * (all_month / 12);
            //คำนวณจำนวนเงินได้ที่ถือเป็นฐานเพื่อคำนวณค่าใช้จ่าย
            decimal sum_b2 =  sum_a;
            //คำนวณค่าใช้จ่ายส่วนแรก
            decimal sum_b3 = 7000 * (all_month / 12);
            //คงเหลือ 2-3
            decimal sum_b4 = sum_b2 - sum_b3;
            //คำนวณค่าใช้จ่ายส่วนที่2 หัก50% ของ sum_b4
            decimal sum_b5 = sum_b4 * Convert.ToDecimal(0.5);
            //รวมค่าใช้จ่ายที่หักได้ทั้งหมด
            decimal sum_b = sum_b3 + sum_b5;

            //หักค่าใช้จ่ายยกมา 
            decimal sum_c3 = sum_a - sum_b;

            all_tax = CallRateTaxEmploy(sum_c3);
            return all_tax;
        }

        protected Decimal CallRateTaxEmploy(decimal total_amt)
        {
            decimal a = 0;
            String sqlStr;
            Double min_rate1 = 0, max_rate1 = 0, taxper1 = 0, goss_rate1 = 0;
            Double min_rate2 = 0, max_rate2 = 0, taxper2 = 0, goss_rate2 = 0;
            Double min_rate3 = 0, max_rate3 = 0, taxper3 = 0, goss_rate3 = 0;
            Double min_rate4 = 0, max_rate4 = 0, taxper4 = 0, goss_rate4 = 0;
            Double min_rate5 = 0, max_rate5 = 0, taxper5 = 0, goss_rate5 = 0;
            Double total_amt_d = Convert.ToDouble(total_amt);
            Sta ta = new Sta(sqlca.ConnectionString);
            sqlStr = @" SELECT HRNUCFTAX_RATE.MIN_RATE,   
                                             HRNUCFTAX_RATE.MAX_RATE,   
                                             HRNUCFTAX_RATE.TAXPER,   
                                             HRNUCFTAX_RATE.GOSS_RATE  
                                        FROM HRNUCFTAX_RATE  
                                    ORDER BY HRNUCFTAX_RATE.MIN_RATE ASC";
            Sdt dt_rate = ta.Query(sqlStr);
            int row_rate = dt_rate.GetRowCount();
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
            Double max_level ,balance = 0;
            //Double P_amt1 = 0, P_amt2 = 0, P_amt3 = 0, P_amt4 = 0, P_amt5 = 0;
            Double t_amt1 = 0, t_amt2 = 0, t_amt3 = 0, t_amt4 = 0, t_amt5 = 0;

            if ((total_amt_d > 0) && (total_amt_d < (max_rate1-(min_rate1-1))))
            {
                t_amt1 = 0;//ภาษีเงินได้
                goto Finish;
            }
            else 
            {
                balance = total_amt_d - (max_rate1-(min_rate1-1));
                t_amt1 = 0;
            }

            if (balance > (max_rate2 - (min_rate2-1)))
            {
                balance = balance - (max_rate2 - (min_rate2-1));
                t_amt2 = 35000;
            }
            else
            {
                t_amt2 = balance * 0.1;
                goto Finish;
            }


            if (balance > (max_rate3 - (min_rate3-1)))
            {
                balance = balance - (max_rate3 - (min_rate3-1));
                t_amt3 = 100000;
            }
            else
            {
                t_amt3= balance * 0.2;
                goto Finish;
            }

            if (balance >= (max_rate4 - (min_rate4-1)))
            {
                balance = balance - (max_rate4 - (min_rate4-1));
                t_amt4 = 900000;
            }
            else
            {
                t_amt4 = balance * 0.3;
                goto Finish;
            }

            if(!(balance >= (max_rate5 - (min_rate5-1))))
            {  
                t_amt5 = balance * 0.37;
                goto Finish;
            }
             
            Finish: 
                a = Convert.ToDecimal(t_amt1 + t_amt2 + t_amt3 + t_amt4 + t_amt5);
            return a;
        }
    }
}
