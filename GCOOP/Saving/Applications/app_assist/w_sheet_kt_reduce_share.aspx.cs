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
using System.Threading;

namespace Saving.Applications.app_assist
{
    public partial class w_sheet_kt_reduce_share : PageWebSheet, WebSheet
    {
        public String app = "";
        public String gid = "";
        public String rid = "";
        protected String pdf = "";
        protected String postPayClick;
        protected String postRetreiveDwMem;
        protected String jsprintColl;
        protected Sta ta;
        protected Sdt dt;
        TimeSpan tp;
        public void InitJsPostBack()
        {
            postRetreiveDwMem = WebUtil.JsPostBack(this, "postRetreiveDwMem");
            postPayClick = WebUtil.JsPostBack(this, "postPayClick");
            jsprintColl = WebUtil.JsPostBack(this, "jsprintColl");
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();
            ta = new Sta(sqlca.ConnectionString);
            dt = new Sdt();
            if (!IsPostBack)
            {
                DwMemP.Reset();
                DwDetailP.Reset();
                DwMainP.Reset();
                DwLog.Reset();
                DwMainP.InsertRow(0);
                DwMemP.InsertRow(0);
                DwDetailP.InsertRow(0);
                DwLog.InsertRow(0);
            }
            else
            {
                this.RestoreContextDw(DwLog);
                this.RestoreContextDw(DwMemP);
                this.RestoreContextDw(DwMainP);
                this.RestoreContextDw(DwDetailP);
              
            }
    
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postRetreiveDwMem")
            {
                RetreiveDwMem();
            }
            else if (eventArg == "postPayClick")
            {
                Calculate();
            }
            else if (eventArg == "jsprintColl")
            {
                JsprintColl();
            }
        }

        public void SaveWebSheet()
        {   
            //เช็คจำนวนการจ่าย
            string member_no = HdMemberNo.Value;
            string updateperpay_year = "";
            decimal withdraw_count = 0;
            try
            {
                decimal perpay = DwDetailP.GetItemDecimal(1,"perpay");
                decimal balance = DwDetailP.GetItemDecimal(1, "balance");
                decimal assist_amt = DwDetailP.GetItemDecimal(1, "assist_amt");

                string withdraw = "select withdraw_count from asnpensionperpay where member_no='"+member_no+"'";
                dt = WebUtil.QuerySdt(withdraw);
                if(dt.Next())
                {
                    withdraw_count = dt.GetDecimal("withdraw_count");
                }
                switch (withdraw_count.ToString())
                {
                    case "0":
                        updateperpay_year = " pension_amt1 = " + perpay + ",pension_amt2 = " + perpay + ",pension_amt3 = " + perpay + 
                            ",pension_amt4 = " + perpay + ",pension_amt5 = " + perpay + ",pension_amt6 = " + perpay + ",pension_amt7 = " +
                            perpay + ",pension_amt8 = " + perpay + ",pension_amt9 = " + perpay + ",pension_amt10 = " + perpay + " ";
                        break;
                    case "1":
                        updateperpay_year = " pension_amt2 = " + perpay + ",pension_amt3 = " + perpay +
                         ",pension_amt4 = " + perpay + ",pension_amt5 = " + perpay + ",pension_amt6 = " + perpay + ",pension_amt7 = " +
                        perpay + ",pension_amt8 = " + perpay + ",pension_amt9 = " + perpay + ",pension_amt10 = " + perpay + " ";
                        break;
                    case "2":
                        updateperpay_year = " pension_amt3 = " + perpay +
                         ",pension_amt4 = " + perpay + ",pension_amt5 = " + perpay + ",pension_amt6 = " + perpay + ",pension_amt7 = " +
                        perpay + ",pension_amt8 = " + perpay + ",pension_amt9 = " + perpay + ",pension_amt10 = " + perpay + " ";
                        break;
                    case "3":
                        updateperpay_year = " pension_amt4 = " + perpay + ",pension_amt5 = " + perpay + ",pension_amt6 = " + perpay + ",pension_amt7 = " +
                        perpay + ",pension_amt8 = " + perpay + ",pension_amt9 = " + perpay + ",pension_amt10 = " + perpay + " ";
                        break;
                    case "4":
                        updateperpay_year = " pension_amt5 = " + perpay + ",pension_amt6 = " + perpay + ",pension_amt7 = " +
                        perpay + ",pension_amt8 = " + perpay + ",pension_amt9 = " + perpay + ",pension_amt10 = " + perpay + " ";
                        break;
                    case "5":
                        updateperpay_year = " pension_amt6 = " + perpay + ",pension_amt7 = " +
                        perpay + ",pension_amt8 = " + perpay + ",pension_amt9 = " + perpay + ",pension_amt10 = " + perpay + " ";
                        break;
                    case "6":
                        updateperpay_year = " pension_amt7 = " +
                        perpay + ",pension_amt8 = " + perpay + ",pension_amt9 = " + perpay + ",pension_amt10 = " + perpay + " ";
                        break;
                    case "7":
                        updateperpay_year = " pension_amt8 = " + perpay + ",pension_amt9 = " + perpay + ",pension_amt10 = " + perpay + " ";
                        break;
                    case "8":
                        updateperpay_year = " pension_amt9 = " + perpay + ",pension_amt10 = " + perpay + " ";
                        break;
                    case "9":
                        updateperpay_year = " pension_amt10 = " + perpay + " ";
                        break;
                }
                //อัพเดทข้อมูล
                if (withdraw_count < 10)
                {
                    string sqlup = "update asnpensionperpay set " + updateperpay_year +
                        " where member_no ='"+member_no+"'";
                    dt = WebUtil.QuerySdt(sqlup);
                    
                }
                //อัพเดท asnreqmaster
                try
                {
                    string upmaster = "update asnreqmaster set perpay =" + perpay + ",balance=" + balance + ",assist_amt = '" + assist_amt + "',calculate_date=to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy')" +
                        " where assisttype_code ='80' and member_no ='"+member_no+"'";
                    dt = WebUtil.QuerySdt(upmaster);
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                }
                catch
                {

                }



            }
            catch
            {

            }
      
        }

        public void WebSheetLoadEnd()
        {
            DwLog.SaveDataCache();
            DwMemP.SaveDataCache();
            DwMainP.SaveDataCache();
            DwDetailP.SaveDataCache();

            dt.Clear();
            ta.Close();
        }

        private void RetreiveDwMem()
        {
            DwDetailP.Reset();
            DwDetailP.InsertRow(0);
            
            String member_no;
            Decimal Share_amt = 0;
            member_no = HdMemberNo.Value;

            try
            {
                // เพิ่มค่าในช่องหุ้น
                string sqlStr = "select sharestk_amt from shsharemaster where member_no = '" + member_no + "'";
                dt = ta.Query(sqlStr);
                if (dt.Next())
                {
                    Share_amt = dt.GetDecimal("sharestk_amt")*10;
                    DwMainP.SetItemDecimal(1, "sharestk_amt", Share_amt);
                }
                else { DwMainP.SetItemDecimal(1, "sharestk_amt", 0); }
            }
            catch
            {

            }

            //DwMemP.Reset();
            DwUtil.RetrieveDataWindow(DwMemP, "kt_pension.pbl", null, member_no);
            try
            {
               string test =  DwMemP.GetItemString(1, "member_no");
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่มีเลขที่สมาชิกนี้");
                //LtAlert.Text = "<script>Alert()</script>";
                //return;
            }

            GetMemberMain();
        }//end RetreiveDwMem()

        private void GetMemberMain()
        {
            string member_no = HdMemberNo.Value;
            string member_tdate, work_tdate;
            DateTime member_date, work_date;
            try // Set วันที่เข้าเป็นสมาชิก
            {
                string SQLMemberDate = "select member_date, birth_date from mbmembmaster where member_no = '" + member_no + "'";
                Sdt dt = WebUtil.QuerySdt(SQLMemberDate);
                if (dt.Next())
                {
                    member_tdate = dt.GetDateTh("member_date");
                    HdBirthDate.Value = dt.GetDateTh("birth_date");

                    DwMainP.SetItemString(1, "member_tdate", member_tdate);
                    DwMainP.SetItemString(1, "birth_tdate", HdBirthDate.Value);
                }
            }
            catch { }
            try // Set วันที่ทำการ
            {
                work_tdate = state.SsWorkDate.Date.ToString("dd/MM/yyyy", WebUtil.TH).Substring(0, 10).Trim();

                DwMainP.SetItemString(1, "entry_tdate", work_tdate);
                DwDetailP.SetItemString(1, "calculate_tdate", work_tdate.Replace("/", ""));
            }
            catch { }

            // Set ประเภทกองทุน
            DwMainP.SetItemString(1, "assisttype_code", "กองทุนสวัสดิการบำนาญสมาชิก");
            GetMemberDetail();
        }//end GetMemberMain()

        private void GetMemberDetail()
        {
            decimal sumpays = 0, perpay, approve_amt, balance, assist_amt;
            DateTime req_date = state.SsWorkDate;
            try
            {
                string SQLGetAsnreqmaster = "select * from asnreqmaster where member_no = '" + HdMemberNo.Value + "' and assist_docno like 'PS%'";
                Sdt dtAsn = WebUtil.QuerySdt(SQLGetAsnreqmaster);
                if (dtAsn.Next())
                {
                    sumpays = dtAsn.GetDecimal("sumpays");
                    perpay = dtAsn.GetDecimal("perpay");
                    approve_amt = dtAsn.GetDecimal("approve_amt");
                    balance = dtAsn.GetDecimal("balance");
                    assist_amt = dtAsn.GetDecimal("assist_amt");
                    req_date = dtAsn.GetDate("req_date");

                    DwDetailP.SetItemDecimal(1, "sumpays", sumpays);
                    DwDetailP.SetItemDecimal(1, "perpay", perpay);
                    DwDetailP.SetItemDecimal(1, "approve_amt", approve_amt);
                    DwDetailP.SetItemDecimal(1, "balance", balance);
                    DwDetailP.SetItemDecimal(1, "assist_amt", assist_amt);
                    string PS_date = req_date.ToString("dd/MM/yyyy", WebUtil.TH).Substring(0, 10).Trim();
                    DwMainP.SetItemString(1, "req_tdate", PS_date);
                }
                else
                {
                    DwDetailP.SetItemDecimal(1, "sumpays", 0);
                    DwDetailP.SetItemDecimal(1, "perpay", 0);
                    DwDetailP.SetItemDecimal(1, "approve_amt", 0);
                    DwDetailP.SetItemDecimal(1, "balance", 0);
                    DwDetailP.SetItemDecimal(1, "assist_amt", 0);
                }
            }
            catch { }

            GetLog();
        }//end GetMemberDetail()

        private void GetLog()
        {
            string member_no = HdMemberNo.Value;
            string operate_date;
            decimal payout_amt;
            int i = 1;
            try
            {
                DwUtil.RetrieveDataWindow(DwLog, "kt_pension.pbl", null, member_no);
            }catch{}
            string SQLSelectSlip = "select operate_date , payout_amt from asnslippayout where member_no ='"+member_no+"' and assisttype_code = '80' order by operate_date";
            Sdt dtSlip = WebUtil.QuerySdt(SQLSelectSlip);
            while (dtSlip.Next())
            {
                try
                {
                    operate_date = dtSlip.GetDateTh("operate_date");
                }
                catch { operate_date = ""; }
                try
                {
                    payout_amt = dtSlip.GetDecimal("payout_amt");
                }
                catch { payout_amt = 0; }

                try
                {
                    DwLog.SetItemString(1, "pay_date" + i, operate_date);
                }
                catch { }
                try
                {
                    DwLog.SetItemDecimal(1, "pay" + i, payout_amt);
                }
                catch { }
                i++;
            }

        }


        private void Calculate()
        {
            try
            {

                //ดึงค่าของเงินที่ได้รับไปแล้ว
                double sumpays_amt = 0;
                double approve_amt = 0; //เงินที่ได้รับครั้งแรกเต็มจำนวน
                DateTime req_date = state.SsWorkDate;
                DateTime calculate_date = state.SsWorkDate;
                string member_no = HdMemberNo.Value;
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
                //------------------------------------------------------------------------------------------
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
                shareAmt = Convert.ToDouble(shareamt)*10;//หุ้นทั้งหมด
                //------------------------------------------------------------------------------------------
                //คำนวณอายุสมาชิก
                DateTime member_date = state.SsWorkDate;
                try
                {
                    string SQLMemberDate = "select member_date from mbmembmaster where member_no = '" + member_no + "'";
                    dt = WebUtil.QuerySdt(SQLMemberDate);
                    if (dt.Next())
                    {
                        member_date = dt.GetDate("member_date");
                    }
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }

                tp = req_date - member_date;
                Double age_year = (tp.TotalDays / 365);
                //int memb_age = Convert.ToInt32(age_year);
                //ไม่ปัดขึ้น
                string age = age_year.ToString();
                age = age.Substring(0, 2);
                age = age.Replace(".", "");
                int memb_age = Convert.ToInt32(age);

                //------------------------------------------------------------------------------------------
                //ดึงข้อมูล ค่าเงินสูงสุด 
                string sqlmaxandper = @"select max_pay,percent from asnucfpayps where minmemb_year <= " +
                                        memb_age + " and " + memb_age + " < maxmemb_year";
                dt = WebUtil.QuerySdt(sqlmaxandper);
                double yearmem_per = 0;
                int max_pay = 0;
                if (dt.Next())
                {
                    max_pay = Convert.ToInt32(dt.GetDecimal("max_pay"));
                    yearmem_per = dt.GetDouble("percent");
                }
                //------------------------------------------------------------------------------------------
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
                //------------------------------------------------------------------------------------------
                //คำนวนจ่ายแต่ละปี

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
                if (perpay % 10 > 1)
                {
                    perpay = perpay + (10 - (perpay % 10));//ปัดเศษ
                }
                //------------------------------------------------------------------------------------------
                //Set ค่าแสดง
                Double balance = MaxPay_amt - sumpays_amt;
                if (balance < 0)
                    perpay = 0;
                DwDetailP.SetItemDecimal(1, "assist_amt",Convert.ToDecimal(MaxPay_amt));
                DwDetailP.SetItemDecimal(1, "perpay", Convert.ToDecimal(perpay));
                DwDetailP.SetItemDecimal(1, "balance", Convert.ToDecimal(balance));
            }
            catch { }
            }//end Calculate()


        private void RunProcess()
        {
            String print_id = "PS_ASS04";
            String as_member_no = HdMemberNo.Value;
            DateTime date = new DateTime(state.SsWorkDate.Year - 1, 9, 30);
            string tdate = date.ToString("MM/dd/yyyy");
            tdate = tdate.Replace("/", "-");
            

            app = state.SsApplication;
            try
            {
                gid = "Pension";
            }
            catch { }
            try
            {
                rid = print_id;
            }
            catch { }


            if (as_member_no == null || as_member_no == "")
            {
                return;
            }
            else
            {
                ReportHelper lnv_helper = new ReportHelper();
                lnv_helper.AddArgument(as_member_no, ArgumentType.String);
                lnv_helper.AddArgument(tdate, ArgumentType.DateTime);

                //****************************************************************

                //ชื่อไฟล์ PDF = YYYYMMDDHHMMSS_<GID>_<RID>.PDF
                String pdfFileName = state.SsWorkDate.ToString("yyyyMMddHHmmss", WebUtil.EN);
                pdfFileName += "_" + gid + "_" + rid + ".pdf";
                pdfFileName = pdfFileName.Trim();

                //ส่งให้ ReportService สร้าง PDF ให้ {โดยปกติจะอยู่ใน C:\GCOOP\Saving\PDF\}.
                try
                {
                    CommonLibrary.WsReport.Report lws_report = WsCalling.Report;
                    String criteriaXML = lnv_helper.PopArgumentsXML();
                    this.pdf = lws_report.GetPDFURL(state.SsWsPass) + pdfFileName;
                    String li_return = lws_report.RunWithID(state.SsWsPass, app, gid, rid, state.SsUsername, criteriaXML, pdfFileName);
                    if (li_return == "true")
                    {
                        HdOpenIFrame.Value = "True";
                    }
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                    return;
                }
            }
        }

        private void JsprintColl()
        {
            //Mai
            RunProcess();
            // Thread.Sleep(5000);
            Thread.Sleep(4500);

            //เด้ง Popup ออกรายงานเป็น PDF.
            String pop = "Gcoop.OpenPopup('" + pdf + "')";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "DsReport", pop, true);
          
        }
        
    }
}