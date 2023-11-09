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
using CommonLibrary.WsCommon;

namespace Saving.Applications.app_assist
{
    public partial class w_sheet_kt_add65year : PageWebSheet, WebSheet
    {
        protected DwThDate tDwCri;
        protected string jsCalculate;
        protected string jsRefresh;
        private Common CmSrv;
        string daytime = DateTime.Now.ToString("dd/MM/yyyy");
        string caplital_year = (Convert.ToInt32(DateTime.Now.ToString("yyyy")) + 543).ToString();
        //protected Tittle.PercentageComplete Percentagecomplete5;


        //string assisttype_code = "90";
        TimeSpan tp;
        public void InitJsPostBack()
        {
            tDwCri = new DwThDate(DwCri, this);
            tDwCri.Add("calculate_date", "calculate_tdate");

            jsRefresh = WebUtil.JsPostBack(this, "jsRefresh");
            jsCalculate = WebUtil.JsPostBack(this, "jsCalculate");
        }

        public void WebSheetLoadBegin()
        {
            try
            {
                CmSrv = WsCalling.Common;
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ติดต่อ Web Service ไม่ได้");
                return;
            }
            this.ConnectSQLCA();
            if (!IsPostBack)
            {
                DwCri.InsertRow(0);
                DwCri.SetItemDate(1, "calculate_date", state.SsWorkDate);
                tDwCri.Eng2ThaiAllRow();
            }
            else
            {
                this.RestoreContextDw(DwCri);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "jsRefresh")
            {
                //Refresh Page
            }
            else if (eventArg == "jsCalculate")
            {
                AddToAssist();
            }
        }

        public void SaveWebSheet()
        {

        }

        public void WebSheetLoadEnd()
        {
            tDwCri.Eng2ThaiAllRow();
            DwCri.SaveDataCache();
        }
        ////ปุ่มดึงข้อมูล
        protected void Add_65_Click(object sender, EventArgs e)
        {
            //Label1.Text = "กรุณารอสักครู่...ขณะนี้โปรแกรมกำลังประมวลผล";
            AddToAssist();
            //string thirtyDate = "30/09/" + (state.SsWorkDate.Year); // 30/09/2012
            //string member_no;
            //DateTime member_date;
            //Sdt check;
            //string sql65 = "select distinct M.member_no as memNo,M.member_date as memDate from mbmembmaster M , shsharemaster S where (M.birth_date is not null) and M.emp_type<>'08' and " +
            //              "(trunc(months_between(to_date('" + thirtyDate + "','dd/mm/yyyy'),M.birth_date))/12)>=65 and S.member_no = M.member_no and S.sharestk_amt <>0 and M.resign_status <> 1 and M.member_no not in (select member_no from asnreqmaster where assisttype_code = '90' and dead_status <>1) "
            //              + " union select distinct M.member_no as memNo,M.member_date as memDate from mbmembmaster M , shsharemaster S where (M.birth_date is not null) and M.emp_type<>'08' and" +
            //              "(trunc(months_between(to_date('" + thirtyDate + "','dd/mm/yyyy'),M.birth_date))/12)>=65 and S.member_no = M.member_no and M.resign_status = 1 and M.resign_date > to_date('" + thirtyDate + "','dd/mm/yyyy') and M.member_no not in (select member_no from asnreqmaster where assisttype_code = '90')";
            //Sdt sixty = WebUtil.QuerySdt(sql65);
            //int row = sixty.Rows.Count;

            //HdCalculateDate.Value = "30/09/" + (state.SsWorkDate.Year + 543); // 30/09/25XX
            ////วนคำนวณทีละคน มี2เงื่อนไขคือ คนใหม่(65) และคนเก่า(65+)
            //int i = 1;

            //while (sixty.Next())
            //{
            //    member_no = sixty.GetString("memNo");
            //    member_date = sixty.GetDate("memDate");
            //    HdMemberNo.Value = member_no;
            //    string checkcase = "select * from asnreqmaster where member_no='" + member_no
            //        + "' and assist_docno like 'SF%' and assisttype_code='90'";
            //    check = WebUtil.QuerySdt(checkcase);

            //    if (check.Next())//เงื่อนไข 1 คนเก่า (65+)
            //    {
            //        HdMemberNo.Value = member_no;
            //        CalSumShare();
            //        cal_last();
            //        cut_reduce();
            //        CalculateOldmem(member_no, member_date);
            //    }
            //    else//เงื่อนไขที่ 2 คนใหม่ (65)
            //    {
            //        HdMemberNo.Value = member_no;
            //        CalSumShare();
            //        cal_last();
            //        cut_reduce();
            //        CalculateNewmem(member_no, member_date);
            //        UpdateBank(member_no);
            //    }
            //    i++;
            //    check.Reset();
            //}//end for
            ////อัพเดทสถานะรอจ่าย
            //try
            //{
            //    string sqlupdatepay = "update asnreqmaster set pay_status = 8,print_status=0 where assist_docno like 'SF%' and pay_status<>-9";
            //    WebUtil.Query(sqlupdatepay);
            //    //Hd_i.Value = row.ToString();
            //    //Timer1.Tick += new EventHandler<EventArgs>(Timer1_Tick);
            //    LtServerMessage.Text = WebUtil.CompleteMessage("โปรแกรมประมวลผลสำเร็จ");
            //}
            //catch (Exception ex)
            //{ LtServerMessage.Text = WebUtil.ErrorMessage(ex); }



        }

        public void UpdateBank(string member_no)
        {

            try
            {
                string upbank = "update asnreqmaster set remark = 'ไม่ได้แจ้ง' ,expense_type ='',expense_bank='',expense_branch='',expense_accid='' where member_no='" +
                    member_no + "' and assist_docno like 'SF%'";
                WebUtil.Query(upbank);
            }
            catch
            {
                string upbank = "update asnreqmaster set remark = 'ไม่ได้แจ้ง' ,expense_type ='',expense_bank='',expense_branch='',expense_accid='' where member_no='" +
                    member_no + "' and assist_docno like 'SF%'";
                WebUtil.Query(upbank);
            }
        }

        //เงื่อนไข 1 คนเก่า (65+)
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
                                       where member_no = '" + member_no + "' and assist_docno like 'SF%' and pay_status<> - 9";
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

            //------------------------------------
            Double MaxPay_amt = shareQuery(member_no);

            double perpay = MaxPay_amt * 0.1;

            //เช็คจ่ายสูงสุดต่อปี
            if (perpay >= 10000)
            {
                perpay = 10000;
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
                string sqlupdate = "update asnreqmaster set calculate_date = to_date('" + "30/09/" + (state.SsWorkDate.Year) + "','dd/mm/yyyy') " +
                    ",assist_amt=" + MaxPay_amt + ",balance=" + balance + ",perpay=" + perpay + ",sumpays=" + sumpays_amt + " where member_no='" + member_no + "' and assisttype_code = '90'";
                WebUtil.Query(sqlupdate);

            }
            catch
            {

            }
            //}//end if calculate_date
        }

        //เงื่อนไขที่ 2 คนใหม่ (65)
        public void CalculateNewmem(string member_no)
        {
            //ต้องดึงหุ้นทั้งหมดมาลงใน Asnsumshare และคำนวณเงินสูงสุดออกมา
            DateTime calculate_date = DwCri.GetItemDate(1, "calculate_date");
            Decimal pay_percent = 0, maxpay_peryear = 0;

            String SqlSelectPerpay = "select percent , maxpay_peryear from asnucfperpay where assist_code = 'SF001'";
            Sdt dtPerpay = WebUtil.QuerySdt(SqlSelectPerpay);
            if (dtPerpay.Next())
            {
                pay_percent = dtPerpay.GetDecimal("percent");
                maxpay_peryear = dtPerpay.GetDecimal("maxpay_peryear");
            }

            Double MaxPay_amt = shareQuery(member_no);
            double perpay = MaxPay_amt * Convert.ToDouble(pay_percent);//คำนวนจ่ายต่อปี
            //เช็คจ่ายสูงสุดต่อปี
            if (perpay >= Convert.ToDouble(maxpay_peryear))
            {
                perpay = Convert.ToDouble(maxpay_peryear);
            }
            if (perpay % 10 != 0)
            {
                perpay = perpay + (10 - (perpay % 10));//ปัดเศษ
            }
            perpay = Math.Floor(perpay);
            decimal assist_year = calculate_date.Year + 544; //คำนวนเพื่อจ่ายปีหน้า 
            string ass_docno90 = CmSrv.GetNewDocNo(state.SsWsPass, "ASSISTDOCNO90");
            //-------------------------------------------------------------------------------------------------
            //ลงทะเบียนข้อมูล
            //-------------------------------------------------------------------------------------------------
            try
            {
                string sqlintsert = "insert into asnreqmaster (assist_docno,capital_year,member_no,assisttype_code,pay_status,assist_amt,approve_amt,balance,calculate_date,perpay,sumpays,req_date,remark) " +
                    "values ('" + ass_docno90 + "'," + assist_year + ",'" + member_no + "','90',8," + MaxPay_amt + "," + MaxPay_amt + "," + MaxPay_amt +
                    ",to_date('" + calculate_date.ToString("dd/MM/yyyy") + "','dd/mm/yyyy')," + perpay + ",0,to_date('" + calculate_date.ToString("dd/MM/yyyy") + "','dd/mm/yyyy'),'ASS90')";
                WebUtil.Query(sqlintsert);
            }
            catch
            {

            }
        }

        public Double shareQuery(string member_no)
        {
            decimal max_pay = 0;
            decimal sumcal_amt = 0;
            string sqlcal65 = "";
            Sdt selectsum;
            try
            {
                string sqlGetMaxPay = "select max(max_pay) from asnucfpay65";
                selectsum = WebUtil.QuerySdt(sqlGetMaxPay);
                if (selectsum.Next())
                {
                    max_pay = selectsum.GetDecimal("max(max_pay)");
                    selectsum.Reset();
                }

                sqlcal65 = "select sum(shrcal_value) from asnsharecal where member_no = '"+member_no+"'";
                selectsum = WebUtil.QuerySdt(sqlcal65);
                if (selectsum.Next())
                {
                    sumcal_amt = selectsum.GetDecimal("sum(shrcal_value)");
                    if (sumcal_amt > max_pay)
                        sumcal_amt = max_pay;
                }
            }
            catch
            {
                sumcal_amt = 0;
            }
            sumcal_amt = Math.Floor(sumcal_amt);
            return Convert.ToDouble(sumcal_amt);

        }//end shareQuery

        //public void OldMemShareQuery(string member_no)
        //{
        //    //double calculate = 0;
        //    //DateTime operate_date = state.SsWorkDate;
        //    //DateTime member_date = state.SsWorkDate;
        //    //string startdate = "";
        //    //string enddate = "";

        //    //decimal sh_amt1 = 0;
        //    //decimal sh_amt2 = 0;
        //    //decimal sh_amt3 = 0;

        //    //string sql1 = "";


        //    //int yearnow = Convert.ToInt32(state.SsWorkDate.ToString("yyyy"));
        //    //int yearlast = Convert.ToInt32(state.SsWorkDate.Year) - 2;

        //    //int share_age = yearnow - yearlast;
        //    //sh_amt1 = 0;
        //    //sh_amt2 = 0;
        //    //sh_amt3 = 0;

        //    ////เดือน 1
        //    //try
        //    //{
        //    //    sql1 = "select share_amount from shsharestatement where (operate_date between to_date('01/01/" + yearlast +
        //    //           "','dd/mm/yyyy') and to_date('30/04/" + yearlast + "','dd/mm/yyyy')) and " +
        //    //           " shritemtype_code='SPM' and member_no='" + member_no + "'";
        //    //    Sdt dt1 = WebUtil.QuerySdt(sql1);
        //    //    while (dt1.Next())
        //    //    {
        //    //        sh_amt1 = sh_amt1 + dt1.GetDecimal("share_amount");
        //    //    }
        //    //}
        //    //catch { }
        //    ////เดือน 2
        //    //try
        //    //{
        //    //    sql1 = "select share_amount from shsharestatement where (operate_date between to_date('01/05/" + yearlast +
        //    //            "','dd/mm/yyyy') and to_date('31/08/" + yearlast + "','dd/mm/yyyy')) and " +
        //    //            " shritemtype_code='SPM' and member_no='" + member_no + "'";
        //    //    Sdt dt00 = WebUtil.QuerySdt(sql1);
        //    //    while (dt00.Next())
        //    //    {
        //    //        sh_amt2 = sh_amt2 + dt00.GetDecimal("share_amount");
        //    //    }
        //    //}
        //    //catch { }
        //    ////เดือน 3 ณ 30/09/####
        //    //try
        //    //{
        //    //    sql1 = "select share_amount from shsharestatement where (operate_date between to_date('01/09/" + yearlast +
        //    //        "','dd/mm/yyyy') and to_date('30/09/" + yearlast + "','dd/mm/yyyy')) and " +
        //    //        " shritemtype_code='SPM' and member_no='" + member_no + "'";
        //    //    Sdt dt01 = WebUtil.QuerySdt(sql1);
        //    //    while (dt01.Next())
        //    //    {
        //    //        sh_amt3 = sh_amt3 + dt01.GetDecimal("share_amount");
        //    //    }
        //    //}
        //    //catch { }

        //    //int rule_seq = 0;
        //    //if (share_age >= 2 && share_age < 5)
        //    //{
        //    //    rule_seq = 1;
        //    //    startdate = "30/09/" + yearlast.ToString();
        //    //    enddate = "01/09/" + (yearlast - 3).ToString();
        //    //    calculate = 0.1;
        //    //}

        //    //int thisyear = yearlast + 543;
        //    //decimal sh_sum = 0;
        //    //sh_sum = sh_amt1 + sh_amt2 + sh_amt3;

        //    //if (sh_amt1 != 0 || sh_amt2 != 0 || sh_amt3 != 0)
        //    //{
        //    //    try
        //    //    {
        //    //        sql1 = "insert into asnsumshare (member_no,rule_seq,year,month_1,month_2,month_3,sumyear,cal_amt,startdate,enddate) " +
        //    //            "values ('" + member_no + "'," + rule_seq + ",'" + thisyear + "'," + sh_amt1 + "," + sh_amt2 + "," + sh_amt3 + "," + sh_sum + "," +
        //    //            (Convert.ToDouble(sh_sum) * calculate) + ",to_date('" + startdate + "','dd/mm/yyyy'),to_date('" + enddate + "','dd/mm/yyyy') )";
        //    //        Sdt dt02 = WebUtil.QuerySdt(sql1);
        //    //    }
        //    //    catch { }
        //    //}//end if
        //    ////อัพเดทหุ้น ตุลาคม-ธันวาของ หุ้นอายุ 3ปี
        //    //decimal sh_amtup = 0;
        //    //string sqlup = "";
        //    //int yearupdate = yearlast - 1;
        //    //thisyear = thisyear - 1;
        //    //try
        //    //{
        //    //    sqlup = "select share_amount from shsharestatement where (operate_date between to_date('01/10/" + yearupdate +
        //    //     "','dd/mm/yyyy') and to_date('31/12/" + yearupdate + "','dd/mm/yyyy')) and " +
        //    //        " shritemtype_code='SPM' and member_no='" + member_no + "'";
        //    //    Sdt dtup = WebUtil.QuerySdt(sqlup);
        //    //    while (dtup.Next())
        //    //    {
        //    //        sh_amtup = sh_amtup + dtup.GetDecimal("share_amount");
        //    //    }
        //    //}
        //    //catch { }
        //    //if (sh_amtup != 0)
        //    //{
        //    //    decimal sumyear = 0;
        //    //    try
        //    //    {
        //    //        string selectup = "select sumyear from asnsumshare where member_no='" + member_no + "' and year='" + thisyear +
        //    //            "' and rule_seq=" + rule_seq + "";
        //    //        Sdt sumy = WebUtil.QuerySdt(selectup);
        //    //        if (sumy.Next())
        //    //        {
        //    //            sumyear = sumy.GetDecimal("sumyear");
        //    //            sumyear = sumyear + sh_amtup;
        //    //            //อัพเดท
        //    //            try
        //    //            {
        //    //                string update = "update asnsumshare set month_3=" + sh_amtup + ",sumyear=" + sumyear + " where member_no='" +
        //    //                    member_no + "' and rule_seq=" + rule_seq + " and year='" + thisyear + "'";
        //    //                Sdt update2 = WebUtil.QuerySdt(update);
        //    //            }
        //    //            catch { }
        //    //        }
        //    //    }
        //    //    catch { }

        //    //}

        //    //----------------------------------------------------

        //}//end OldMemShareQuery

        public void CalSumShare()
        {
            string member_no = HdMemberNo.Value;
            string yeartemp = "0000", monthtemp = "01", capital_year = "0000", month = "01", day = "01", rule_seq = "1", rule_seq_temp = "0";
            decimal count = 0, share_temp = 0;
            int i = 1, j = 1, flag = 0;
            double sumcal = 0;
            string startdate = "01/01/2000", enddate = "01/01/2000", deaddate;
            //string startdate1, startdate2, startdate3, startdate4, startdate5;
            //string enddate1, enddate2, enddate3, enddate4, enddate5;
            string[] seq_no = new string[6];
            double[] percent = new double[6];
            decimal thismonth = DateTime.Now.Month;
            decimal thisyear = DateTime.Now.Year + 543;
            decimal diff_year;
            decimal share1 = 0, share2 = 0, share3 = 0, share4 = 0, share5 = 0, share6 = 0, share7 = 0, share8 = 0, share9 = 0, share10 = 0, share11 = 0, share12 = 0;
            string check65 = "0";

            string SQLSumshare = "Select * from asnsumshare where member_no = '" + member_no + "'";
            Sdt dtsumshare = WebUtil.QuerySdt(SQLSumshare);

            string SQLShare = "select * from asnshare where member_no = '" + member_no + "' and capital_year <= '2546' order by capital_year DESC";
            Sdt dtshare = WebUtil.QuerySdt(SQLShare);

            string SQLStatement = "select to_char(operate_date , 'mm') as shrmonth,share_amount , operate_date from shsharestatement where member_no = '" + member_no + "' and shritemtype_code = 'SPM' and item_status <> -9 order by seq_no ASC";
            Sdt dtStatement = WebUtil.QuerySdt(SQLStatement);

            string SQLDeadDate = "select dead_date from asnreqmaster where member_no = '" + member_no + "'";
            Sdt dtDeadDate = WebUtil.QuerySdt(SQLDeadDate);

            deaddate = HdCalculateDate.Value; //******************************************

            thismonth = Convert.ToInt32(deaddate.Replace("/", "").Substring(2, 2));
            thisyear = Convert.ToInt32(deaddate.Replace("/", "").Substring(4, 4));

            //ดูว่าใน ASNSUMSHARE มีข้อมูลหรือไม่
            if (dtsumshare.Next())
            {
                //check65 = "0";
                string SqlDelete = "delete from asnsumshare where member_no = '" + member_no + "'";
                WebUtil.Query(SqlDelete);
            }

            if (check65 == "0")
            {
                HdCheckOldMem.Value = "0";
                while (dtStatement.Next())
                {
                    monthtemp = dtStatement.GetString("shrmonth");
                    share_temp = dtStatement.GetDecimal("share_amount");
                    share_temp = share_temp * 10;
                    capital_year = dtStatement.GetDateTh("operate_date");
                    capital_year = capital_year.Substring(capital_year.Length - 4);
                    diff_year = thisyear - Convert.ToInt32(capital_year);
                    if (diff_year >= 2 && count == 0)
                    {
                        flag = 1;
                        count = 1;
                    }
                    switch (monthtemp)
                    {
                        case "01": share1 = share_temp; break;
                        case "02": share2 = share_temp; break;
                        case "03": share3 = share_temp; break;
                        case "04": share4 = share_temp; break;
                        case "05": share5 = share_temp; break;
                        case "06": share6 = share_temp; break;
                        case "07": share7 = share_temp; break;
                        case "08": share8 = share_temp; break;
                        case "09": share9 = share_temp; break;
                        case "10": share10 = share_temp; break;
                        case "11": share11 = share_temp; break;
                        case "12": share12 = share_temp; break;
                    }
                    //j++;

                    if (monthtemp == "12")
                    {
                        InsertSumShare(share1, share2, share3, share4, share5, share6, share7, share8, share9, share10, share11, share12, capital_year, flag);
                        j = 1;
                        monthtemp = "00";
                        flag = 0;
                        share_temp = 0;
                        share1 = share2 = share3 = share4 = share5 = share6 = share7 = share8 = share9 = share10 = share11 = share12 = 0;
                    }
                }

                if (monthtemp != "00" && share_temp != 0) // กรณี มีหุ้นไม่ครบปี
                    InsertSumShare(share1, share2, share3, share4, share5, share6, share7, share8, share9, share10, share11, share12, capital_year, flag);

                while (dtshare.Next())
                {
                    share1 = dtshare.GetDecimal("share_amt_1");
                    share2 = dtshare.GetDecimal("share_amt_2");
                    share3 = dtshare.GetDecimal("share_amt_3");
                    share4 = dtshare.GetDecimal("share_amt_4");
                    share5 = dtshare.GetDecimal("share_amt_5");
                    share6 = dtshare.GetDecimal("share_amt_6");
                    share7 = dtshare.GetDecimal("share_amt_7");
                    share8 = dtshare.GetDecimal("share_amt_8");
                    share9 = dtshare.GetDecimal("share_amt_9");
                    share10 = dtshare.GetDecimal("share_amt_10");
                    share11 = dtshare.GetDecimal("share_amt_11");
                    share12 = dtshare.GetDecimal("share_amt_12");

                    capital_year = dtshare.GetString("capital_year");
                    InsertSumShare(share1, share2, share3, share4, share5, share6, share7, share8, share9, share10, share11, share12, capital_year, flag);

                }
            }
        } //ดึงหุ้นมาเรียง

        public void InsertSumShare(decimal share1, decimal share2, decimal share3, decimal share4, decimal share5, decimal share6, decimal share7, decimal share8, decimal share9, decimal share10, decimal share11, decimal share12, string capital_year, int flag)
        {
            string member_no = HdMemberNo.Value;
            string yeartemp = "0000", monthtemp = "01", month = "01", day = "01", rule_seq = "1", rule_seq_temp = "0";
            decimal count = 0, share_temp;
            int i = 1, j = 1;
            double sumcal = 0;
            string startdate = "01/01/2000", enddate = "01/01/2000";
            string[] seq_no = new string[6];
            double[] percent = new double[6];
            DateTime dead_date = DateTime.ParseExact(HdCalculateDate.Value, "dd/MM/yyyy", WebUtil.TH);
            decimal thisyear = dead_date.Year + 543;
            decimal thismonth = dead_date.Month;//DateTime.Now.Month;
            //decimal thisyear = DateTime.Now.Year + 543;
            decimal month1, month2, month3, sumyear, diff_year;
            //decimal share1, share2, share3, share4, share5, share6, share7, share8, share9, share10, share11, share12;
            string check65 = "0";
            if (flag == 1) // ดึงปีซื้อหุ้นปีแรก
            {
                HdMaxYear.Value = capital_year;
                monthtemp = thismonth.ToString("00");
                count = 1;
            }
            yeartemp = HdMaxYear.Value;
            monthtemp = thismonth.ToString("00");

            string SQLUcf = "select percent from asnucfpay65";
            Sdt dtUcf = WebUtil.QuerySdt(SQLUcf);
            while (dtUcf.Next())
            {
                percent[i] = dtUcf.GetDouble("percent");
                percent[i] = Convert.ToDouble(percent[i].ToString("0.00"));
                i++;
            }

            diff_year = thisyear - Convert.ToInt32(capital_year);

            if (diff_year == 2)
            {
                switch (Convert.ToInt32(thismonth))
                {
                    case 12: break;
                    case 11: share12 = 0; break;
                    case 10: share12 = share11 = 0; break;
                    case 9: share12 = share11 = share10 = 0; break;
                    case 8: share12 = share11 = share10 = share9 = 0; break;
                    case 7: share12 = share11 = share10 = share9 = share8 = 0; break;
                    case 6: share12 = share11 = share10 = share9 = share8 = share7 = 0; break;
                    case 5: share12 = share11 = share10 = share9 = share8 = share7 = share6 = 0; break;
                    case 4: share12 = share11 = share10 = share9 = share8 = share7 = share6 = share5 = 0; break;
                    case 3: share12 = share11 = share10 = share9 = share8 = share7 = share6 = share5 = share4 = 0; break;
                    case 2: share12 = share11 = share10 = share9 = share8 = share7 = share6 = share5 = share4 = share3 = 0; break;
                    case 1: share12 = share11 = share10 = share9 = share8 = share7 = share6 = share5 = share4 = share3 = share2 = 0; break;
                }
            }
            month1 = share1 + share2 + share3 + share4;
            month2 = share5 + share6 + share7 + share8;
            month3 = share9 + share10 + share11 + share12;
            sumyear = month1 + month2 + month3;

            if (share1 != 0)
                month = "31/01/";
            else if (share2 != 0)
                month = "28/02/";
            else if (share3 != 0)
                month = "31/03/";
            else if (share4 != 0)
                month = "30/04/";
            else if (share5 != 0)
                month = "31/05/";
            else if (share6 != 0)
                month = "30/06/";
            else if (share7 != 0)
                month = "31/07/";
            else if (share8 != 0)
                month = "31/08/";
            else if (share9 != 0)
                month = "30/09/";
            else if (share10 != 0)
                month = "31/10/";
            else if (share11 != 0)
                month = "30/11/";
            else if (share12 != 0)
                month = "31/12/";
            else
                month = "31/01/";

            if (diff_year >= 2 && diff_year <= 5)
            {
                rule_seq = "1";
                if (diff_year == 2)
                {
                    switch (Convert.ToInt32(thismonth))
                    {
                        case 12: sumcal = Convert.ToDouble(month1 + month2 + month3) * percent[1]; break;
                        case 11: sumcal = Convert.ToDouble(share9 + share10 + share11 + month1 + month2) * percent[1]; break;
                        case 10: sumcal = Convert.ToDouble(share9 + share10 + month1 + month2) * percent[1]; break;
                        case 9: sumcal = Convert.ToDouble(share9 + month1 + month2) * percent[1]; break;
                        case 8: sumcal = Convert.ToDouble(month1 + month2) * percent[1]; break;
                        case 7: sumcal = Convert.ToDouble(share5 + share6 + share7 + month1) * percent[1]; break;
                        case 6: sumcal = Convert.ToDouble(share5 + share6 + month1) * percent[1]; break;
                        case 5: sumcal = Convert.ToDouble(share5 + month1) * percent[1]; break;
                        case 4: sumcal = Convert.ToDouble(month1) * percent[1]; break;
                        case 3: sumcal = Convert.ToDouble(share1 + share2 + share3) * percent[1]; break;
                        case 2: sumcal = Convert.ToDouble(share1 + share2) * percent[1]; break;
                        case 1: sumcal = Convert.ToDouble(share1) * percent[1]; break;
                    }
                }
                else
                {
                    sumcal = Convert.ToDouble(sumyear) * percent[1];
                }
                i = 1;
            }
            else if (diff_year > 5 && diff_year <= 10)
            {
                rule_seq = "2";
                sumcal = Convert.ToDouble(sumyear) * percent[2];
                i = 2;
            }
            else if (diff_year > 10 && diff_year <= 15)
            {
                rule_seq = "3";
                sumcal = Convert.ToDouble(sumyear) * percent[3];
                i = 3;
            }
            else if (diff_year > 15 && diff_year <= 20)
            {
                rule_seq = "4";
                sumcal = Convert.ToDouble(sumyear) * percent[4];
                i = 4;
            }
            else if (diff_year > 20)
            {
                rule_seq = "5";
                sumcal = Convert.ToDouble(sumyear) * percent[5];
                i = 5;
            }
            //ดึง

            try
            {
                if (rule_seq == "1")
                {
                    startdate = month + (Convert.ToInt32(yeartemp) - 543);
                    enddate = "01" + month.Substring(2, 4) + (Convert.ToInt32(yeartemp) - 543 - 3);
                }
                else if (rule_seq == "2")
                {
                    startdate = month + (Convert.ToInt32(yeartemp) - 543 - 3);
                    enddate = "01" + month.Substring(2, 4) + (Convert.ToInt32(yeartemp) - 543 - 8);
                }
                else if (rule_seq == "3")
                {
                    startdate = month + (Convert.ToInt32(yeartemp) - 543 - 8);
                    enddate = "01" + month.Substring(2, 4) + (Convert.ToInt32(yeartemp) - 543 - 13);
                }
                else if (rule_seq == "4")
                {
                    startdate = month + (Convert.ToInt32(yeartemp) - 543 - 13);
                    enddate = "01" + month.Substring(2, 4) + (Convert.ToInt32(yeartemp) - 543 - 18);
                }
                else if (rule_seq == "5")
                {
                    startdate = month + (Convert.ToInt32(yeartemp) - 543 - 18);
                    enddate = "01" + month.Substring(2, 4) + (Convert.ToInt32(yeartemp) - 543 - 23);
                }
            }
            catch { }
            try
            {
                //sumcal = Math.Floor(sumcal);
                if (diff_year >= 2)
                {
                    string SQLInsSumShare = "insert into asnsumshare(member_no , rule_seq , year , month_1 , month_2 , month_3 , sumyear , cal_amt , startdate , enddate) values ('" + member_no + "','" + rule_seq + "','" + capital_year + "'," + month1 + "," + month2 + "," + month3 + "," + sumyear + "," + sumcal + ",to_date('" + startdate + "','dd/mm/yyyy'),to_date('" + enddate + "','dd/mm/yyyy'))";
                    WebUtil.Query(SQLInsSumShare);
                }
            }
            catch
            {
            }
        } //Query ลงตาราง Asnsumshare

        public void cal_last()
        {
            decimal month_1 = 0, month_2 = 0, month_3 = 0, rule_seq = 0, sumyear = 0, cal_amt = 0;
            decimal month_12 = 0, month_22 = 0, month_32 = 0, rule_seq2 = 0, sumyear2 = 0, cal_amt2 = 0;//แถวบน
            decimal m1 = 0, m2 = 0, m3 = 0, m4 = 0, m5 = 0, m6 = 0, m7 = 0, m8 = 0, m9 = 0, m10 = 0, m11 = 0, m12 = 0;
            string share_year = "";
            string shortmonth = "";
            DateTime dead_date = DateTime.ParseExact(HdCalculateDate.Value, "dd/MM/yyyy", WebUtil.TH);
            decimal thisday = dead_date.Day;
            decimal thisyear = dead_date.Year + 543;
            decimal thismonth = dead_date.Month;
            string start_tdate = "", end_tdate = "";
            Sdt dtAsnshare;
            string query = "select * from asnsumshare where member_no = '" + HdMemberNo.Value + "' and year in (" + (thisyear - 5) + " , " + (thisyear - 10) + " , " + (thisyear - 15) + " , " + (thisyear - 20) + ")";
            Sdt dt1 = WebUtil.QuerySdt(query);

            while (dt1.Next())
            {
                rule_seq2 = dt1.GetDecimal("rule_seq");
                share_year = dt1.GetString("year");

                month_1 = dt1.GetDecimal("month_1");
                month_2 = dt1.GetDecimal("month_2");
                month_3 = dt1.GetDecimal("month_3");
                start_tdate = thisday.ToString("00") + thismonth.ToString("00") + share_year;
                end_tdate = thisday.ToString("00") + thismonth.ToString("00") + (Convert.ToInt32(share_year) - 5).ToString();
                DateTime start_date = DateTime.ParseExact(start_tdate, "ddMMyyyy", WebUtil.TH);
                DateTime end_date = DateTime.ParseExact(end_tdate, "ddMMyyyy", WebUtil.TH);
                try
                {
                    if (Convert.ToInt32(share_year) <= 2546)
                    {
                        String SqlAsnshare = "select * from asnshare where member_no = '" + HdMemberNo.Value + "' and capital_year = '" + share_year + "'";
                        dtAsnshare = WebUtil.QuerySdt(SqlAsnshare);
                        if (dtAsnshare.Next())
                        {
                            m1 = dtAsnshare.GetDecimal("share_amt_1");
                            m2 = dtAsnshare.GetDecimal("share_amt_2");
                            m3 = dtAsnshare.GetDecimal("share_amt_3");
                            m4 = dtAsnshare.GetDecimal("share_amt_4");
                            m5 = dtAsnshare.GetDecimal("share_amt_5");
                            m6 = dtAsnshare.GetDecimal("share_amt_6");
                            m7 = dtAsnshare.GetDecimal("share_amt_7");
                            m8 = dtAsnshare.GetDecimal("share_amt_8");
                            m9 = dtAsnshare.GetDecimal("share_amt_9");
                            m10 = dtAsnshare.GetDecimal("share_amt_10");
                            m11 = dtAsnshare.GetDecimal("share_amt_11");
                            m12 = dtAsnshare.GetDecimal("share_amt_12");
                        }
                    }
                    else if (Convert.ToInt32(share_year) > 2546)
                    {
                        String SqlSelectShare = "select member_no , operate_date , share_amount*10 as sharemultiple , to_char(operate_date,'mm') as shortmonth from shsharestatement where to_char(operate_date,'yyyy')='" + (Convert.ToInt32(share_year) - 543) + "' and member_no = '" + HdMemberNo.Value + "' and shritemtype_code ='SPM' order by shortmonth";
                        Sdt dtStmShare = WebUtil.QuerySdt(SqlSelectShare);
                        while (dtStmShare.Next())
                        {
                            shortmonth = dtStmShare.GetString("shortmonth");
                            switch (shortmonth)
                            {
                                case "01": m1 = dtStmShare.GetDecimal("sharemultiple"); break;
                                case "02": m2 = dtStmShare.GetDecimal("sharemultiple"); break;
                                case "03": m3 = dtStmShare.GetDecimal("sharemultiple"); break;
                                case "04": m4 = dtStmShare.GetDecimal("sharemultiple"); break;
                                case "05": m5 = dtStmShare.GetDecimal("sharemultiple"); break;
                                case "06": m6 = dtStmShare.GetDecimal("sharemultiple"); break;
                                case "07": m7 = dtStmShare.GetDecimal("sharemultiple"); break;
                                case "08": m8 = dtStmShare.GetDecimal("sharemultiple"); break;
                                case "09": m9 = dtStmShare.GetDecimal("sharemultiple"); break;
                                case "10": m10 = dtStmShare.GetDecimal("sharemultiple"); break;
                                case "11": m11 = dtStmShare.GetDecimal("sharemultiple"); break;
                                case "12": m12 = dtStmShare.GetDecimal("sharemultiple"); break;
                            }
                        }
                    }
                }
                catch { }
                //m1 = m2 = m3 = m4 = (month_1 / 4);
                //m5 = m6 = m7 = m8 = (month_2 / 4);
                //m9 = m10 = m11 = m12 = (month_3 / 4);

                if (thismonth >= 1 && thismonth < 5)
                {
                    rule_seq = rule_seq2 + 1;
                    if (thismonth == 1)
                    {
                        month_1 = m1;
                        month_2 = 0;
                        month_3 = 0;
                        month_12 = m2 + m3 + m4;
                        month_22 = m5 + m6 + m7 + m8;
                        month_32 = m9 + m10 + m11 + m12;
                    }
                    else if (thismonth == 2)
                    {
                        month_1 = m1 + m2;
                        month_2 = 0;
                        month_3 = 0;
                        month_12 = m3 + m4;
                        month_22 = m5 + m6 + m7 + m8;
                        month_32 = m9 + m10 + m11 + m12;
                    }
                    else if (thismonth == 3)
                    {
                        month_1 = m1 + m2 + m3;
                        month_2 = 0;
                        month_3 = 0;
                        month_12 = m4;
                        month_22 = m5 + m6 + m7 + m8;
                        month_32 = m9 + m10 + m11 + m12;
                    }
                    else if (thismonth == 4)
                    {
                        month_1 = m1 + m2 + m3 + m4;
                        month_2 = 0;
                        month_3 = 0;
                        month_12 = 0;
                        month_22 = m5 + m6 + m7 + m8;
                        month_32 = m9 + m10 + m11 + m12;
                    }
                }
                else if (thismonth >= 5 && thismonth < 9)
                {
                    rule_seq = rule_seq2 + 1;
                    if (thismonth == 5)
                    {
                        month_1 = m1 + m2 + m3 + m4;
                        month_2 = m5;
                        month_3 = 0;
                        month_12 = 0;
                        month_22 = m6 + m7 + m8;
                        month_32 = m9 + m10 + m11 + m12;
                    }
                    else if (thismonth == 6)
                    {
                        month_1 = m1 + m2 + m3 + m4;
                        month_2 = m5 + m6;
                        month_3 = 0;
                        month_12 = 0;
                        month_22 = m7 + m8;
                        month_32 = m9 + m10 + m11 + m12;

                    }
                    else if (thismonth == 7)
                    {
                        month_1 = m1 + m2 + m3 + m4;
                        month_2 = m5 + m6 + m7;
                        month_3 = 0;
                        month_12 = 0;
                        month_22 = m8;
                        month_32 = m9 + m10 + m11 + m12;
                    }
                    else if (thismonth == 8)
                    {
                        month_1 = m1 + m2 + m3 + m4;
                        month_2 = m5 + m6 + m7 + m8;
                        month_3 = 0;
                        month_12 = 0;
                        month_22 = 0;
                        month_32 = m9 + m10 + m11 + m12;
                    }
                }
                else if (thismonth >= 9 && thismonth <= 12)
                {
                    rule_seq = rule_seq2 + 1;
                    if (thismonth == 9)
                    {
                        if (Convert.ToInt32(share_year) > 2546)
                        {
                            try
                            {
                                month_1 = m1 + m2 + m3 + m4;
                                month_2 = m5 + m6 + m7 + m8;
                                //m9 = m10 = m11 = m12 = 0;
                                //String tmpshare_year = (Convert.ToInt32(share_year) - 543).ToString();
                                //String SqlSelectShare = "select member_no , operate_date , share_amount*10 as sharemultiple , to_char(operate_date,'mm') as shortmonth from shsharestatement where to_char(operate_date,'yyyy')='" + tmpshare_year + "' and to_char(operate_date,'mm') in ('09','10','11','12') and member_no = '" + HdMemberNo.Value + "' and shritemtype_code ='SPM' order by shortmonth";
                                //Sdt dtShare = WebUtil.QuerySdt(SqlSelectShare);
                                //while (dtShare.Next())
                                //{
                                //    shortmonth = dtShare.GetString("shortmonth");
                                //    switch (shortmonth)
                                //    {
                                //        case "09": m9 = dtShare.GetDecimal("sharemultiple"); break;
                                //        case "10": m10 = dtShare.GetDecimal("sharemultiple"); break;
                                //        case "11": m11 = dtShare.GetDecimal("sharemultiple"); break;
                                //        case "12": m12 = dtShare.GetDecimal("sharemultiple"); break;
                                //    }
                                //}
                                month_3 = m9;
                                month_12 = 0;
                                month_22 = 0;
                                month_32 = m10 + m11 + m12;
                            }
                            catch { }
                            //ยังติดที่ว่ามันตัดส่วนหลัง 0 ทิ้ง
                        }
                        else
                        {
                            month_1 = m1 + m2 + m3 + m4;
                            month_2 = m5 + m6 + m7 + m8;
                            month_3 = m9;
                            month_12 = 0;
                            month_22 = 0;
                            month_32 = m10 + m11 + m12;
                        }
                    }
                    else if (thismonth == 10)
                    {
                        if (Convert.ToInt32(share_year) > 2546)
                        {
                            try
                            {
                                month_1 = m1 + m2 + m3 + m4;
                                month_2 = m5 + m6 + m7 + m8;
                                //m9 = m10 = m11 = m12 = 0;
                                //String tmpshare_year = (Convert.ToInt32(share_year) - 543).ToString();
                                //String SqlSelectShare = "select member_no , operate_date , share_amount*10 as sharemultiple , to_char(operate_date,'mm') as shortmonth from shsharestatement where to_char(operate_date,'yyyy')='" + tmpshare_year + "' and to_char(operate_date,'mm') in ('09','10','11','12') and member_no = '" + HdMemberNo.Value + "' and shritemtype_code ='SPM' order by shortmonth";
                                //Sdt dtShare = WebUtil.QuerySdt(SqlSelectShare);
                                //while (dtShare.Next())
                                //{
                                //    shortmonth = dtShare.GetString("shortmonth");
                                //    switch (shortmonth)
                                //    {
                                //        case "09": m9 = dtShare.GetDecimal("sharemultiple"); break;
                                //        case "10": m10 = dtShare.GetDecimal("sharemultiple"); break;
                                //        case "11": m11 = dtShare.GetDecimal("sharemultiple"); break;
                                //        case "12": m12 = dtShare.GetDecimal("sharemultiple"); break;
                                //    }
                                //}
                                month_3 = m9 + m10;
                                month_12 = 0;
                                month_22 = 0;
                                month_32 = m11 + m12;
                            }
                            catch { }
                            //ยังติดที่ว่ามันตัดส่วนหลัง 0 ทิ้ง
                        }
                        else
                        {
                            month_1 = m1 + m2 + m3 + m4;
                            month_2 = m5 + m6 + m7 + m8;
                            month_3 = m9 + m10;
                            month_12 = 0;
                            month_22 = 0;
                            month_32 = m11 + m12;
                        }
                    }
                    else if (thismonth == 11)
                    {
                        if (Convert.ToInt32(share_year) > 2546)
                        {
                            try
                            {
                                month_1 = m1 + m2 + m3 + m4;
                                month_2 = m5 + m6 + m7 + m8;
                                //m9 = m10 = m11 = m12 = 0;
                                //String tmpshare_year = (Convert.ToInt32(share_year) - 543).ToString();
                                //String SqlSelectShare = "select member_no , operate_date , share_amount*10 as sharemultiple , to_char(operate_date,'mm') as shortmonth from shsharestatement where to_char(operate_date,'yyyy')='" + tmpshare_year + "' and to_char(operate_date,'mm') in ('09','10','11','12') and member_no = '" + HdMemberNo.Value + "' and shritemtype_code ='SPM' order by shortmonth";
                                //Sdt dtShare = WebUtil.QuerySdt(SqlSelectShare);
                                //while (dtShare.Next())
                                //{
                                //    shortmonth = dtShare.GetString("shortmonth");
                                //    switch (shortmonth)
                                //    {
                                //        case "09": m9 = dtShare.GetDecimal("sharemultiple"); break;
                                //        case "10": m10 = dtShare.GetDecimal("sharemultiple"); break;
                                //        case "11": m11 = dtShare.GetDecimal("sharemultiple"); break;
                                //        case "12": m12 = dtShare.GetDecimal("sharemultiple"); break;
                                //    }
                                //}
                                month_3 = m9 + m10 + m11;
                                month_12 = 0;
                                month_22 = 0;
                                month_32 = m12;
                            }
                            catch { }
                            //ยังติดที่ว่ามันตัดส่วนหลัง 0 ทิ้ง
                        }
                        else
                        {
                            month_1 = m1 + m2 + m3 + m4;
                            month_2 = m5 + m6 + m7 + m8;
                            month_3 = m9 + m10 + m11;
                            month_12 = 0;
                            month_22 = 0;
                            month_32 = m12;
                        }
                    }
                    else if (thismonth == 12)
                    {
                        month_1 = m1 + m2 + m3 + m4;
                        month_2 = m5 + m6 + m7 + m8;
                        month_3 = m9 + m10 + m11 + m12;
                        month_12 = 0;
                        month_22 = 0;
                        month_32 = 0;
                    }
                }

                sumyear2 = month_12 + month_22 + month_32;
                sumyear = month_1 + month_2 + month_3;

                if (Convert.ToInt32(share_year) + 5 == thisyear)
                {
                    cal_amt2 = Convert.ToDecimal(Convert.ToDouble(sumyear2) * 0.1);
                    cal_amt = Convert.ToDecimal(Convert.ToDouble(sumyear) * 0.2);
                    cal_amt = Math.Floor(cal_amt);
                    cal_amt2 = Math.Floor(cal_amt2);

                }
                else if (Convert.ToInt32(share_year) + 10 == thisyear)
                {
                    cal_amt2 = Convert.ToDecimal(Convert.ToDouble(sumyear2) * 0.2);
                    cal_amt = Convert.ToDecimal(Convert.ToDouble(sumyear) * 0.3);
                    cal_amt = Math.Floor(cal_amt);
                    cal_amt2 = Math.Floor(cal_amt2);
                }
                else if (Convert.ToInt32(share_year) + 15 == thisyear)
                {
                    cal_amt2 = Convert.ToDecimal(Convert.ToDouble(sumyear2) * 0.3);
                    cal_amt = Convert.ToDecimal(Convert.ToDouble(sumyear) * 0.35);
                    cal_amt = Math.Floor(cal_amt);
                    cal_amt2 = Math.Floor(cal_amt2);
                }
                else if (Convert.ToInt32(share_year) + 20 == thisyear)
                {
                    cal_amt2 = Convert.ToDecimal(Convert.ToDouble(sumyear2) * 0.35);
                    cal_amt = Convert.ToDecimal(Convert.ToDouble(sumyear) * 0.4);
                    cal_amt = Math.Floor(cal_amt);
                    cal_amt2 = Math.Floor(cal_amt2);
                }
                string SQLUPDATE = "update asnsumshare set month_1=" + month_12 + ",month_2=" + month_22 + ",month_3=" + month_32 +
                        ",sumyear=" + sumyear2 + ",cal_amt=" + cal_amt2 + " where member_no='" + HdMemberNo.Value + "' and year='" + share_year + "' and rule_seq=" + rule_seq2 + "";
                WebUtil.QuerySdt(SQLUPDATE);

                String SQLINSERT = "insert into asnsumshare (member_no,rule_seq,year,month_1,month_2,month_3,sumyear,cal_amt,startdate , enddate) values ('" +
                        HdMemberNo.Value + "'," + rule_seq + ",'" + share_year + "'," + month_1 + "," + month_2 + "," + month_3 + "," + sumyear + "," + cal_amt + ",to_date('" + start_date.ToString("yyyy/MM/dd") + "','yyyy/mm/dd'),to_date('" + end_date.ToString("yyyy/MM/dd") + "','yyyy/mm/dd'))";
                WebUtil.QuerySdt(SQLINSERT);
                try
                {
                    String SqlUpdateDate = "update asnsumshare set startdate = to_date('" + start_date.ToString("yyyy/MM/dd") + "','yyyy/mm/dd') , enddate = to_date('" + end_date.ToString("yyyy/MM/dd") + "','yyyy/mm/dd') where member_no = '" + HdMemberNo.Value + "' and rule_seq = " + rule_seq;
                    WebUtil.Query(SqlUpdateDate);
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            }



        }    //จัดเรียงปีที่แบ่งการคำนวนเป็นสองช่วง

        public void cut_reduce()
        {
            try
            {
                string capital_year = (state.SsWorkDate.Year + 543).ToString();
                string SqlSelect = "select min(year) from asnsumshare where member_no = '" + HdMemberNo.Value + "' and sumyear = 0";
                Sdt dtyear = WebUtil.QuerySdt(SqlSelect);
                if (dtyear.Next())
                {
                    capital_year = dtyear.GetString("min(year)");
                    String SQLDeleteLast = "delete from asnsumshare where year > '" + capital_year + "' and member_no = '" + HdMemberNo.Value + "'";
                    WebUtil.Query(SQLDeleteLast);
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }//ตัดปีหลังจากที่มีการลดหุ้นออก

        //protected void Timer1_Tick(object sender, EventArgs e)
        //{
        //    Label3.Text = Hd_i.Value + " / " + HdRow.Value;
        //    UpdatePanel1.Update();
        //}  
        
        public void AddToAssist()
        {
            try
            {
                Decimal min_seq = 0, max_seq = 0;
                Decimal member_type = DwCri.GetItemDecimal(1, "member_type");
                Decimal countMembNo = 0, countMembNoDiv = 0, divNumber = 15;
                DateTime slipDate = state.SsWorkDate;
                DateTime calculate_date = state.SsWorkDate;

                try
                {
                    calculate_date = DwCri.GetItemDate(1, "calculate_date");
                }
                catch { }

                String SqlCountMembNo = "select count(member_no) from mbmembmaster ";
                Sdt dtCount = WebUtil.QuerySdt(SqlCountMembNo);
                if (dtCount.Next())
                {
                    countMembNo = dtCount.GetDecimal("count(member_no)");
                    countMembNoDiv = countMembNo / divNumber;
                }
                for (int i = 1; i <= divNumber; i++)
                {
                    max_seq = max_seq + countMembNoDiv;
                    min_seq = max_seq - countMembNoDiv;
                    
                    String SqlInsertToAsnPeriod = @"
                    insert into asnshareperiod
                    ( member_no , shr_year , shr_mth , shr_value )
                    select memno , shryear , shrmth , shr_amt 
            	        from (
            		        select member_No as memno , capital_year as shryear , '01' as shrmth , share_amt_1 as shr_amt 
            		        from asnshare 
            		        union
            		        select member_No as memno , capital_year as shryear , '02' as shrmth , share_amt_2 as shr_amt 
            		        from asnshare 
            		        union
            		        select member_No as memno , capital_year as shryear , '03' as shrmth , share_amt_3 as shr_amt 
            		        from asnshare 
            		        union
            		        select member_No as memno , capital_year as shryear , '04' as shrmth , share_amt_4 as shr_amt 
            		        from asnshare 
            		        union
            		        select member_No as memno , capital_year as shryear , '05' as shrmth , share_amt_5 as shr_amt 
            		        from asnshare 
            		        union
            		        select member_No as memno , capital_year as shryear , '06' as shrmth , share_amt_6 as shr_amt 
            		        from asnshare 
            		        union
            		        select member_No as memno , capital_year as shryear , '07' as shrmth , share_amt_7 as shr_amt 
            		        from asnshare 
            		        union
            		        select member_No as memno , capital_year as shryear , '08' as shrmth , share_amt_8 as shr_amt 
            		        from asnshare 
            		        union
            		        select member_No as memno , capital_year as shryear , '09' as shrmth , share_amt_9 as shr_amt 
            		        from asnshare 
            		        union
            		        select member_No as memno , capital_year as shryear , '10' as shrmth , share_amt_10 as shr_amt 
            		        from asnshare 
            		        union
            		        select member_No as memno , capital_year as shryear , '11' as shrmth , share_amt_11 as shr_amt 
            		        from asnshare 
            		        union
            		        select member_No as memno , capital_year as shryear , '12' as shrmth , share_amt_12 as shr_amt 
            		        from asnshare 
            	        ) ah where shryear || shrmth < '255001'
            	        and exists( select 1
                                       from
                                       (       select mm.member_no as member_no
                                               from mbmembmaster mm , shsharemaster sm
                                               where mm.birth_date is not null 
                                               and mm.member_no = sm.member_no
									           and sm.sharestk_amt > 0
                                               and resign_status = 0 
                                               and mm.emp_type <> '08'
                                               and months_between( to_date('" + calculate_date.ToString("dd/MM/yyyy") + @"' , 'dd/mm/yyyy') , mm.birth_date )/12 >= 65.00 
                                               and not exists( select 1 from asnreqmaster asn where asn.member_no = mm.member_no and assisttype_code = '90' )
                                               union
                                               select mm.member_no
                                               from mbmembmaster mm 
                                               where mm.birth_date is not null 
                                               and resign_status = 1
                                               and mm.emp_type <> '08'
                                               and resign_date >= to_date( '" + calculate_date.ToString("dd/MM/yyyy") + @"' , 'dd/mm/yyyy' ) 
                                               and months_between( to_date('" + calculate_date.ToString("dd/MM/yyyy") + @"' , 'dd/mm/yyyy') , mm.birth_date )/12 >= 65.00 
                                               and not exists( select 1 from asnreqmaster asn where asn.member_no = mm.member_no and assisttype_code = '90' )
                                       ) mem
                                       where mem.member_no = ah.memno 
                                       and exists ( 
                                                    select 1
                                                    from (
                                                            select member_no , rank() over ( partition by branch_id order by member_no ) as seq_no
                                                            from mbmembmaster
                                                    ) mas where seq_no between " + min_seq +" and " + max_seq +@"
                                                    and mem.member_no = mas.member_no
                                                  ) 
                                    )
                                    
            	        union

            	        select member_no , to_char ( to_number( to_char( slip_date , 'yyyy' ) ) + 543 ) as shryear , to_char( slip_date , 'mm' ) as shrmth , sum( share_amount * 10 ) as shr_amt
            	        from shsharestatement 
            	        where shritemtype_code = 'SPM'
            	        and slip_date >= to_date('01/01/2007' , 'dd/mm/yyyy') and slip_date <= to_date('" + calculate_date.ToString("dd/MM") + "/" + (calculate_date.Year - 2).ToString() + @"' , 'dd/mm/yyyy')

            	        and exists( select 1
                                       from
                                       (       select mm.member_no as member_no
                                               from mbmembmaster mm , shsharemaster sm
                                               where mm.birth_date is not null 
                                               and mm.member_no = sm.member_no
									           and sm.sharestk_amt > 0
                                               and resign_status = 0 
                                               and mm.emp_type <> '08'
                                               and months_between( to_date('" + calculate_date.ToString("dd/MM/yyyy") + @"' , 'dd/mm/yyyy') , mm.birth_date )/12 >= 65.00 
                                               and not exists( select 1 from asnreqmaster asn where asn.member_no = mm.member_no and assisttype_code = '90' )
                                               union
                                               select mm.member_no
                                               from mbmembmaster mm 
                                               where mm.birth_date is not null 
                                               and resign_status = 1
                                               and mm.emp_type <> '08'
                                               and resign_date >= to_date( '" + calculate_date.ToString("dd/MM/yyyy") + @"' , 'dd/mm/yyyy' ) 
                                               and months_between( to_date('" + calculate_date.ToString("dd/MM/yyyy") + @"' , 'dd/mm/yyyy') , mm.birth_date )/12 >= 65.00
                                               and not exists( select 1 from asnreqmaster asn where asn.member_no = mm.member_no and assisttype_code = '90' )
                                       ) mem
                                       where mem.member_no = shsharestatement.member_no
                                       and exists ( 
                                                    select 1
                                                    from (
                                                            select member_no , rank() over ( partition by branch_id order by member_no ) as seq_no
                                                            from mbmembmaster
                                                    ) mas where seq_no  between " + min_seq + " and " + max_seq + @"
                                                    and mem.member_no = mas.member_no
                                                  ) 
                                   )
            	        and item_status = 1
            	        group by member_no , to_char ( to_number( to_char( slip_date , 'yyyy' ) ) + 543 ) , to_char( slip_date , 'mm' ) ";
                    WebUtil.Query(SqlInsertToAsnPeriod);
                }

                //คำนวนกองทุนใหม่
                ReCalculate();

                //อัพเดทข้อมูลจาก asnsharecal ลง asnreqmaster
                UpdateOldMember();

                //insert ข้อมูลลง asnreqmaster
                PrepareToAsnreqmaster();
                LtServerMessage.Text = WebUtil.CompleteMessage("โปรแกรมประมวลผลสำเร็จ...");
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void ReCalculate()
        {
            DateTime calculate_date = state.SsWorkDate;
            try
            {
                calculate_date = DwCri.GetItemDate(1, "calculate_date");
            }
            catch { }
            String SqlDeleteAsnsharecal = "truncate table asnsharecal";
            WebUtil.Query(SqlDeleteAsnsharecal);
            String InsertToAsnsharecal = @"
                    insert into asnsharecal
                    (member_no , shr_year , seq_no , shrqt01_value , shrqt02_value , shrqt03_value , shr_value , shr_percent , shrcal_value )
                    select member_no , shr_year , seq_no , quarter01 , quarter02 , quarter03 , ( quarter01 + quarter02 + quarter03 ) , percent , ( quarter01 + quarter02 + quarter03 ) * percent
                    from
                    (
	                    select member_No , shr_year , 
	                    rank() over ( partition by member_no , shr_year order by percent ) as seq_no ,
	                    sum( case when shr_mth = '01' then shr_value when shr_mth = '02' then shr_value when shr_mth = '03' then shr_value when shr_mth = '04' then shr_value  else 0 end ) as quarter01 ,
	                    sum( case when shr_mth = '05' then shr_value when shr_mth = '06' then shr_value when shr_mth = '07' then shr_value when shr_mth = '08' then shr_value  else 0 end ) as quarter02 ,
	                    sum( case when shr_mth = '09' then shr_value when shr_mth = '10' then shr_value when shr_mth = '11' then shr_value when shr_mth = '12' then shr_value  else 0 end ) as quarter03 ,
	                    percent
	                    from(
		                        select member_no , shr_year , shr_mth , shr_value ,  
		                        ( select percent  
		                            from asnucfpay65 where 
		                            trim(asnshareperiod.shr_year || asnshareperiod.shr_mth ) > trim( to_char ( to_number( to_char( to_date('" + calculate_date.ToString("dd/MM/yyyy") + @"' , 'dd/mm/yyyy') , 'yyyy' ) ) + 542 - maxmemb_year ) || to_char( to_date('" + calculate_date.ToString("dd/MM/yyyy") + @"' , 'dd/mm/yyyy') , 'mm' ) ) and
		                            trim(asnshareperiod.shr_year || asnshareperiod.shr_mth ) <= trim( to_char ( to_number( to_char( to_date('" + calculate_date.ToString("dd/MM/yyyy") + @"' , 'dd/mm/yyyy') , 'yyyy' ) ) + 543 - minmemb_year ) || to_char( to_date('" + calculate_date.ToString("dd/MM/yyyy") + @"' , 'dd/mm/yyyy') , 'mm' ) ) 
		                        ) as percent
		                        from asnshareperiod 
                            
	                        )
	                group by member_no , shr_year , percent 
                     ) where (quarter01 + quarter02 + quarter03) > 0";
            WebUtil.Query(InsertToAsnsharecal);
        }

        public void PrepareToAsnreqmaster()
        {
            DateTime calculate_date = DwCri.GetItemDate(1, "calculate_date");
            String member_no = "";
            String SelectMemberToMaster = @"
                                       select member_no
                                       from
                                       (       
									        select mm.member_no as member_no
                                            from mbmembmaster mm 
                                            where mm.birth_date is not null 
                                            and resign_status = 0 
									        and mm.emp_type <> '08'
                                            and months_between( to_date('"+calculate_date.ToString("dd/MM/yyyy")+@"' , 'dd/mm/yyyy') , mm.birth_date )/12 >= 65.00 
									        and not exists ( select 1 from asnreqmaster where mm.member_no = asnreqmaster.member_no and asnreqmaster.assisttype_code = '90')
                                            union
                                            select mm.member_no 
                                            from mbmembmaster mm 
                                            where mm.birth_date is not null 
                                            and resign_status = 1
									        and mm.emp_type <> '08'
                                            and resign_date >= to_date( '" + calculate_date.ToString("dd/MM/yyyy") + @"' , 'dd/mm/yyyy' ) 
                                            and months_between( to_date('" + calculate_date.ToString("dd/MM/yyyy") + @"' , 'dd/mm/yyyy') , mm.birth_date )/12 >= 65.00 
									        and not exists ( select 1 from asnreqmaster where mm.member_no = asnreqmaster.member_no and asnreqmaster.assisttype_code = '90')                    
								        )";
            Sdt dtMember_no = WebUtil.QuerySdt(SelectMemberToMaster);
            while (dtMember_no.Next())
            {
                member_no = dtMember_no.GetString("member_no");
                CalculateNewmem(member_no);
            }
        }

        public void UpdateOldMember()
        {
            String member_no = "";
            Double assist_amt = 0, perpay = 0, balance = 0, sumpays = 0;
            Decimal pay_percent = 0, maxpay_peryear = 0;
            DateTime calculate_date = DwCri.GetItemDate(1, "calculate_date");
            String SqlMembNo = "select member_no , sumpays from asnreqmaster where assisttype_code = '90' and dead_status = 0";
            Sdt dtMembNo = WebUtil.QuerySdt(SqlMembNo);
            String SqlSelectPerpay = "select percent , maxpay_peryear from asnucfperpay where assist_code = 'SF001'";
            Sdt dtPerpay = WebUtil.QuerySdt(SqlSelectPerpay);
            if (dtPerpay.Next())
            {
                pay_percent = dtPerpay.GetDecimal("percent");
                maxpay_peryear = dtPerpay.GetDecimal("maxpay_peryear");
            }

            while(dtMembNo.Next())
            {
                member_no = dtMembNo.GetString("member_no");
                sumpays = dtMembNo.GetDouble("sumpays");
                assist_amt = shareQuery(member_no);
                perpay = Math.Floor(assist_amt * Convert.ToDouble(pay_percent));
                if (perpay % 10 != 0)
                {
                    perpay = perpay + (10 - (perpay % 10));//ปัดเศษ
                }
                if (perpay > Convert.ToDouble(maxpay_peryear)) perpay = Convert.ToDouble(maxpay_peryear);
                balance = assist_amt - sumpays;

                String SqlUpdateAsnreqmaster = @"update asnreqmaster 
                                             set assist_amt = "+assist_amt+" , pay_status = 8 , perpay = "+perpay+" , calculate_date = to_date('"+calculate_date.ToString("dd/MM/yyyy")+"','dd/mm/yyyy') , print_status = 0 , balance = "+balance+@"
                                             where member_no = '"+member_no+"' and assisttype_code = '90' and dead_status = 0 ";
                WebUtil.Query(SqlUpdateAsnreqmaster);
            }
        }
    }
}