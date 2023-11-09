using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CommonLibrary;
using DBAccess;
using Sybase.DataWindow;
using System.Web.Services.Protocols;


namespace Saving.Applications.app_assist
{
    public partial class w_sheet_kt_addpension : PageWebSheet, WebSheet
    {
        protected DwThDate tDwMain;
        protected String postDate;
        protected String postDate2;
        protected string jsRefresh;
        TimeSpan tp;

        //DateTime DayTimeNow = DateTime.Now;
        string daytime= DateTime.Now.ToString("dd/mm/yyyy");
        string caplital_year = (Convert.ToInt32(DateTime.Now.ToString("yyyy"))+543).ToString();
        string assisttype_code = "80";

        public void InitJsPostBack()
        {
            postDate = WebUtil.JsPostBack(this, "postDate");
            postDate2 = WebUtil.JsPostBack(this, "postDate2");
            jsRefresh = WebUtil.JsPostBack(this, "jsRefresh");
        }

        public void WebSheetLoadBegin()
        {

        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "jsRefresh")
            {
                //Refresh Page
            }
        }

        public void SaveWebSheet()
        {
           
        }

        public void WebSheetLoadEnd()
        {

        }
        ////ปุ่มดึงข้อมูล
        protected void Add_PS_Click(object sender, EventArgs e)
        {
            Label1.Text = "กรุณารอสักครู่...ขณะนี้โปรแกรมกำลังประมวลผล";

            string member_no;
            DateTime member_date;
            DateTime birth_date;
            //string sql60 = "select member_no,member_date,birth_date from mbmembmaster where resign_status<>1  and (birth_date is not null) and " +
            //              "(trunc(months_between(to_date('" +"30/09/"+ state.SsWorkDate.Year + "','dd/mm/yyyy'),birth_date))/12)>=60";
            string sql60 = "select member_no,member_date,birth_date from mbmembmaster where resign_status<>1  and (birth_date is not null) and " +
                          "(trunc(months_between(to_date('" + "30/09/" + state.SsWorkDate.Year + "','dd/mm/yyyy'),birth_date))/12)>=60 and emp_type<>'08' order by member_no";
            Sdt sixty = WebUtil.QuerySdt(sql60);
            int row = sixty.Rows.Count;
            //วนคำนวณทีละคน มี2เงื่อนไขคือ คนใหม่(60) และคนเก่า(60+)
            int i = 1;

            while (sixty.Next())
            {
                member_no = sixty.GetString("member_no");
                member_date = sixty.GetDate("member_date");
                birth_date = sixty.GetDate("birth_date");
                string checkcase = "select * from asnreqmaster where member_no='" + member_no
                    + "' and assist_docno like 'PS%' and assisttype_code='80'";
                Sdt check = WebUtil.QuerySdt(checkcase);

                if (check.Next())//เงื่อนไข 1 คนเก่า (60+)
                {
                    CalculateOldmem(member_no, member_date);
                    //UpdateBank(member_no);
                }
                else//เงื่อนไขที่ 2 คนใหม่ (60)
                {
                    CalculateNewmem(member_no, member_date, birth_date);
                    UpdateBank(member_no);
                }
                

                i++;

            }//end for
            //อัพเดทสถานะรอจ่าย
            try
            {
                string sqlupdatepay = "update asnreqmaster set pay_status = 8,print_status=0 where assist_docno like 'PS%' and pay_status<>-9";
                Sdt uppay_status = WebUtil.QuerySdt(sqlupdatepay);
                LtServerMessage.Text = WebUtil.CompleteMessage("โปรแกรมประมวลผลสำเร็จ");
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }


        }

        public void UpdateBank(string member_no)
        {
            try
            {
                string upbank = "update asnreqmaster set remark = 'ไม่ได้แจ้ง' ,expense_type ='',expense_bank='',expense_branch='',expense_accid='' where member_no='" +
                    member_no + "' and assist_docno like 'PS%'";
                WebUtil.QuerySdt(upbank);
            }
            catch
            {
                //string upbank = "update asnreqmaster set remark = 'ไม่ได้แจ้ง' ,expense_type ='',expense_bank='',expense_branch='',expense_accid='' where member_no='" +
                //    member_no + "' and assist_docno like 'PS%'";
                //WebUtil.QuerySdt(upbank);
            }
        }

        //เงื่อนไข 1 คนเก่า (60+)
        public void CalculateOldmem(string member_no, DateTime member_date)
        {

            Sdt dt;
            //ดึงค่าของเงินที่ได้รับไปแล้ว
            double sumpays_amt = 0;
            double approve_amt = 0; //เงินที่ได้รับครั้งแรกเต็มจำนวน
            DateTime req_date = state.SsWorkDate;
            DateTime calculate_date = state.SsWorkDate;
            try
            {
               string sqlsumpays = @"select sumpays, approve_amt,req_date,calculate_date
                           from asnreqmaster 
                           where member_no = '" + member_no + "' and assist_docno like 'PS%'";
                dt = WebUtil.QuerySdt(sqlsumpays);
                if (dt.Next())
                {
                    sumpays_amt = Convert.ToDouble(dt.GetDecimal("sumpays"));
                    approve_amt = Convert.ToDouble(dt.GetDecimal("approve_amt"));
                    req_date = dt.GetDate("req_date");
                    calculate_date = dt.GetDate("calculate_date");
                }
            }
            catch { sumpays_amt = 0; }

            //เช็คว่าคำนวณไปแล้วรึยังในปีนั้นๆ
            if (calculate_date.ToString("yyyy") != state.SsWorkDate.ToString("yyyy"))
            {
                            // ดึงจำนวนหุ้นที่มีทั้งหมด
                            decimal shareamt = 0;
                            string sqlshare = @"select sharestk_amt 
                                           from shsharemaster 
                                           where member_no = '" + member_no + "'";
                            dt = WebUtil.QuerySdt(sqlshare);
                            if (dt.Next())
                            {
                                shareamt = dt.GetDecimal("sharestk_amt");
                            }
                            double shareAmt = 0;
                            double MaxPay_amt = 0;
                            shareAmt = Convert.ToDouble(shareamt);//หุ้นทั้งหมด
                            //------------------------------------------------------------------------------------------

                            //คำนวณอายุสมาชิก
                            string entry_date = req_date.ToString("yyyy");
                            string mem_date = member_date.ToString("yyyy");
                            int memb_age = (Convert.ToInt32(entry_date) + 543) - (Convert.ToInt32(mem_date) + 543);
                            //ดึงข้อมูล ค่าเงินสูงสุด 
                            string sqlmaxandper = @"select max_pay,percent from asnucfpayps where minmemb_year <= " +
                                  memb_age + " and maxmemb_year > " + memb_age;
                            dt = WebUtil.QuerySdt(sqlmaxandper);
                            double yearmem_per = 0;
                            int max_pay = 0;
                            if (dt.Next())
                            {
                                max_pay = Convert.ToInt32(dt.GetDecimal("max_pay"));
                                yearmem_per = dt.GetDouble("percent");
                            }
                            //คำนวณตามเงื่อนไข
                            MaxPay_amt = yearmem_per * shareAmt;
                            if (MaxPay_amt > max_pay)
                            {
                                MaxPay_amt = max_pay;
                            }
                            // เช็คกรณีหุ้นเพิ่ม
                            if (MaxPay_amt > approve_amt)
                            {
                                MaxPay_amt = approve_amt;
                            }
                            //คำนวนจ่ายแต่ละปี
                            //------------------------------------------------------------------------------------------------------
                            string sqlyear_per = @"select percent,year,maxpay_peryear  from asnucfperpay 
                                                        where assist_code =( select max(assist_code) 
                                                        from asnucfperpay where assist_code like 'PS%' )";
                            dt = WebUtil.QuerySdt(sqlyear_per);
                            double percent = 0;
                            int year_pay = 0;
                            int maxpay_per_year = 0;
                            if (dt.Next())
                            {
                                percent = dt.GetDouble("percent");//5%
                                year_pay = Convert.ToInt32(dt.GetDecimal("year"));//10ปี
                                maxpay_per_year = Convert.ToInt32(dt.GetDecimal("maxpay_peryear"));//5,000
                            }

                            //คำนวนจ่ายต่อปี
                            double perpay = MaxPay_amt * percent;

                            //เช็คจ่ายสูงสุดต่อปี
                            if (perpay >= maxpay_per_year)
                            {
                                perpay = maxpay_per_year;
                            }
                            if (perpay % 10 != 0)
                            {
                                perpay = perpay + (10 - (perpay % 10));//ปัดเศษ
                            }
                            //-------------------------------------------------------------------------------------------------------------
                            //อัพเดทข้อมูลใหม่
                            double balance = MaxPay_amt - sumpays_amt;

                            try
                            {
                                string sqlupdate = "update asnreqmaster set calculate_date = to_date('"+ "30/09/" + state.SsWorkDate.Year + "','dd/mm/yyyy') " +
                                    ",assist_amt=" + MaxPay_amt + ",balance=" + balance + ",perpay=" + perpay + ",sumpays=" + sumpays_amt + " where member_no='" + member_no + "' and assist_docno like 'PS%'";
                                WebUtil.Query(sqlupdate);
                                
                            }
                            catch
                            {
                              
                            }
            }//end if calculate_date
           // อัพเดทตาราง10ปี
        }

        //เงื่อนไขที่ 2 คนใหม่ (60)
        public void CalculateNewmem(string member_no, DateTime member_date, DateTime birth_date)
        {

            //คำนวณอายุสมาชิก
            DateTime date_cal = DateTime.ParseExact("3009" + state.SsWorkDate.ToString("yyyy"), "ddMMyyyy", WebUtil.EN);
            tp = date_cal - member_date;
            Double age_year = (tp.TotalDays / 365);
            int memb_age = Convert.ToInt32(age_year); // อายุการเป็นสมาชิก
            //------------------------------------
            tp = date_cal - birth_date;
            age_year = (tp.TotalDays / 365);
            //age = age_year.ToString();
            //age = age.Substring(0, 2);
            int member_age = Convert.ToInt32(Math.Floor(age_year)); //อายุของสมาชิก
            //------------------------------------

            decimal shareamt = 0;
            //Query หุ้นจาก Statement ล่าสุดก่อนวันที่ 31/09/ปีนั้นๆ
            string sqlshare = "select *  from shsharestatement where member_no='" + member_no + "' and seq_no =(select max(seq_no) from shsharestatement where member_no = '" + member_no + "' and operate_date<=to_date('" + date_cal.ToString("ddMMyyyy") + "','ddmmyyyy') and item_status =1) ";
            Sdt dt = WebUtil.QuerySdt(sqlshare);
            if (dt.Next())
            {
                shareamt = dt.GetDecimal("sharestk_amt");
            }
            double shareAmt = 0;
            double MaxPay_amt = 0;
            shareAmt = Convert.ToDouble(shareamt)*10;//หุ้นทั้งหมด
            //------------------------------------------------------------------------------------------
            //ดึงข้อมูล ค่าเงินสูงสุด 
            string sqlmaxandper = @"select max_pay,percent from asnucfpayps where minmemb_year <= " +
                      memb_age + " and maxmemb_year > " + memb_age ;
            dt = WebUtil.QuerySdt(sqlmaxandper);
            double year_per = 0;
            int max_pay = 0;
            if (dt.Next())
            {
                max_pay = Convert.ToInt32(dt.GetDecimal("max_pay"));
                year_per = dt.GetDouble("percent");
                year_per = double.Parse(year_per.ToString("0.00"));
            }
            MaxPay_amt = year_per * shareAmt; //คำนวณตามเงื่อนไข
            if (MaxPay_amt > max_pay)
            {
                MaxPay_amt = max_pay;
            }
            //------------------------------------------------------------------------------------------------------
            string sqlyear_per = @"select percent,year,maxpay_peryear  from asnucfperpay 
                                        where assist_code =( select max(assist_code) 
                                        from asnucfperpay where assist_code like 'PS%' )";
            dt = WebUtil.QuerySdt(sqlyear_per);
            double percent = 0;
            int year_pay = 0;
            int maxpay_per_year = 0;
            if (dt.Next())
            {
                percent = dt.GetDouble("percent");//5%
                percent = double.Parse(percent.ToString("0.00"));
                year_pay = Convert.ToInt32(dt.GetDecimal("year"));//10ปี
                maxpay_per_year = Convert.ToInt32(dt.GetDecimal("maxpay_peryear"));//5,000
            }
            double perpay = MaxPay_amt * percent;//คำนวนจ่ายต่อปี
            perpay = Math.Floor(perpay); // ปัดเศษลง

            //เช็คจ่ายสูงสุดต่อปี
            if (perpay >= maxpay_per_year)
            {
                perpay = maxpay_per_year;
            }

            string ass_docno = "";
            try
            {
                string queryass = "select max(assist_docno) from asnreqmaster where assisttype_code = '80'and assist_docno like 'PS%' and capital_year = '" + (state.SsWorkDate.Year + 543 + 1) + "'"; //จ่ายปีหน้า
                DataTable qass = WebUtil.Query(queryass);
                if (qass.Rows.Count > 0)
                {
                    ass_docno = qass.Rows[0][0].ToString();
                    if (ass_docno != "")
                    {
                        ass_docno = ass_docno.Substring(4, 5);
                    }
                    else { ass_docno = "00000"; }
                }
            }
            catch
            {
                ass_docno = "00000";
            }

            decimal ass_cal = Convert.ToDecimal(ass_docno);
            ass_cal++;
            caplital_year = (state.SsWorkDate.Year + 543 + 1).ToString();
            string year_docno = caplital_year.Substring(2,2);
            string Ass_docno = "PS" + year_docno + ass_cal.ToString("00000");//รันรหัสใหม่
            //-------------------------------------------------------------------------------------------------
            //ลงทะเบียนข้อมูล

            try
            {
                string sqlintsert = "insert into asnreqmaster (assist_docno,capital_year,member_no,assisttype_code,pay_status,assist_amt,approve_amt,balance,calculate_date,perpay,sumpays,req_date) " +
                    "values ('" + Ass_docno + "'," + caplital_year + ",'" + member_no + "','80',8," + MaxPay_amt + "," + MaxPay_amt + "," + MaxPay_amt +
                    ",to_date('"+"30/09/" + state.SsWorkDate.Year + "','dd/mm/yyyy')," + perpay + ",0,to_date('"+"30/09/" + state.SsWorkDate.Year + "','dd/mm/yyyy'))";
                WebUtil.Query(sqlintsert);
                string sqlinsertten_years = @"insert into asnpensionperpay (
                    member_no,year_1,year_2,year_3,year_4,year_5,year_6,year_7,year_8,year_9,year_10,
                    pension_amt1,pension_amt2,pension_amt3,pension_amt4,pension_amt5,pension_amt6,pension_amt7,
                    pension_amt8,pension_amt9,pension_amt10,withdraw_count,sharestk_amt
                    )
                    values
                    ('" + member_no + "','" + member_age + "','" + (member_age + 1) + "','" + (member_age + 2) +
                        "','" + (member_age + 3) + "','" + (member_age + 4) + "','" + (member_age + 5) + "','" + (member_age + 6) +
                        "','" + (member_age + 7) + "','" + (member_age + 8) + "','" + (member_age + 9) + "'," +
                    " " + perpay + "," + perpay + "," + perpay + "," + perpay + "," + perpay +
                    "," + perpay + "," + perpay + "," + perpay + "," + perpay + "," + perpay + ",0,"+shareAmt+")";
                WebUtil.Query(sqlinsertten_years);
            }
            catch
            {
            }
            //อัพเดทจำนวน cmdocumentcontrol ASNREQDOCNO80
            int last_doc = Convert.ToInt32(ass_cal);
            string sqlupdatedoc = "update cmdocumentcontrol set last_documentno = "+last_doc+"  where document_code='ASNREQDOCNO80'";
            WebUtil.Query(sqlupdatedoc);
        }

    }
}