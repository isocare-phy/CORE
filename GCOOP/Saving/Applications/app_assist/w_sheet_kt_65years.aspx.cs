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
    public partial class w_sheet_kt_65years : PageWebSheet, WebSheet
    {
        protected DwThDate tDwMain;
        protected DwThDate tDwMainP;
        protected DwThDate tDwDetail;
        protected DwThDate tDwDetailP;
        protected DwThDate tDwMain65;
        protected DwThDate tDwDetail65;

        protected String postRefresh;
        protected String postChangeHeight;
        protected String postChangeAmt;
        protected String postRetreiveDwMem;
        protected String postRetrieveDwMain;
        protected String postGetMemberDetail;
        protected String postGetMemberDetailpension;
        protected String postRetrieveBankBranch;
        protected String postChangeAssist;
        protected String postBPayclick;
        protected String postBDeadPayclick;
        protected String postMoneyType;
        protected String postFilterBranch;
        protected String postInsertRow;
        protected String postDeleteRow;

        private String sqlStr, envvalue;
        private String member_no, assist_docno, membgroup_code, member_dead_tdate;
        private String expense_bank, expense_branch, expense_accid, paytype_code, remark_cancel;
        private Decimal capital_year, salary_amt, assist_amt, req_status, seq_pay, rowcount;
        private DateTime pay_date, cancel_date = DateTime.Now, center_date, req_date, member_date;
        private Nullable<Decimal> cancel_id;
        private String assisttype_code="";
        TimeSpan tp;

        protected Sta ta;
        protected Sdt dt;

        public void InitJsPostBack()
        {
            postRefresh = WebUtil.JsPostBack(this, "postRefresh");
            postChangeAmt = WebUtil.JsPostBack(this, "postChangeAmt");
            postChangeHeight = WebUtil.JsPostBack(this, "postChangeHeight");
            postRetreiveDwMem = WebUtil.JsPostBack(this, "postRetreiveDwMem");
            postRetrieveDwMain = WebUtil.JsPostBack(this, "postRetrieveDwMain");
            postGetMemberDetail = WebUtil.JsPostBack(this, "postGetMemberDetail");
            postGetMemberDetailpension = WebUtil.JsPostBack(this, "postGetMemberDetailpension");
            postRetrieveBankBranch = WebUtil.JsPostBack(this, "postRetrieveBankBranch");
            postChangeAssist = WebUtil.JsPostBack(this, "postChangeAssist");
            postBPayclick = WebUtil.JsPostBack(this, "postBPayclick");
            postBDeadPayclick = WebUtil.JsPostBack(this, "postBDeadPayclick");
            postMoneyType = WebUtil.JsPostBack(this, "postMoneyType");
            postFilterBranch = WebUtil.JsPostBack(this, "postFilterBranch");
            postInsertRow = WebUtil.JsPostBack(this, "postInsertRow");
            postDeleteRow = WebUtil.JsPostBack(this, "postDeleteRow");

            tDwMain65 = new DwThDate(DwMain65, this);
            tDwDetail65 = new DwThDate(DwDetail65, this);
            tDwMain65.Add("pay_date", "pay_tdate");
            tDwMain65.Add("req_date", "req_tdate");
            tDwMain65.Add("entry_date", "entry_tdate");
            tDwMain65.Add("member_date", "member_tdate");
            tDwMain65.Add("approve_date", "approve_tdate");
            tDwDetail65.Add("member_dead_date", "member_dead_tdate");
        }

        public void WebSheetLoadBegin()
        {


            HdTy.Value = "90";
            this.ConnectSQLCA();

            ta = new Sta(sqlca.ConnectionString);
            dt = new Sdt();

            LtAlert.Text = "";


            if (!IsPostBack)
            {
                NewClear();
            }
            else
            {

                    this.RestoreContextDw(DwMem65);
                    this.RestoreContextDw(DwMain65);
                    this.RestoreContextDw(DwDetail65);
                    this.RestoreContextDw(DwReceive);

                    DwMain65.SetItemDecimal(1, "req_status", 8);
                    DwMain65.SetItemDateTime(1, "pay_date", DateTime.Now);
                    DwMain65.SetItemDateTime(1, "req_date", DateTime.Now);
                    DwMain65.SetItemDateTime(1, "entry_date", DateTime.Now);
                    DwMain65.SetItemDateTime(1, "approve_date", state.SsWorkDate);
            }

            tDwMain65.Eng2ThaiAllRow();
            tDwDetail65.Eng2ThaiAllRow();
            DwUtil.RetrieveDDDW(DwMain65, "assisttype_code", "kt_65years.pbl", null);
            DwUtil.RetrieveDDDW(DwMain65, "capital_year", "kt_65years.pbl", null);
            DwUtil.RetrieveDDDW(DwMain65, "cancel_id", "kt_65years.pbl", null);
            DwUtil.RetrieveDDDW(DwMain65, "req_status", "kt_65years.pbl", null);
            DwUtil.RetrieveDDDW(DwMain65, "pay_status", "kt_65years.pbl", null);
            DwUtil.RetrieveDDDW(DwMain65, "expense_bank", "kt_65years.pbl", null);
            DwUtil.RetrieveDDDW(DwMain65, "paytype_code", "kt_65years.pbl", null);
            DwUtil.RetrieveDDDW(DwMain65, "moneytype_code", "kt_65years.pbl", null);
            DwUtil.RetrieveDDDW(DwMain65, "asnslippayout_bank_code", "kt_65years.pbl", null);
            

        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postRetreiveDwMem")
            {
                RetreiveDwMem();
            }
            else if (eventArg == "postRefresh")
            {
                Refresh();
            }
            else if (eventArg == "postChangeAmt")
            {
                ChangeAmt();
            }
            else if (eventArg == "postRetrieveDwMain")
            {
                RetrieveDwMain();
            }
            else if (eventArg == "postRetrieveBankBranch")
            {
                RetrieveBankBranch();
            }
            else if (eventArg == "postChangeHeight")
            {
                ChangeHeight();
            }
            else if (eventArg == "postGetMemberDetail")
            {
                GetMemberDetail();
            }
            else if (eventArg == "postGetMemberDetailpension")
            {
                //GetMemberDetailpension();
            }
            else if (eventArg == "postChangeAssist")
            {
                GetAssistType();
            }
            else if (eventArg == "postBPayclick")
            {
                BPayclick();
            }
            else if (eventArg == "postBDeadPayclick")
            {
                BDeadPayclick();
            }
            else if (eventArg == "postMoneyType")
            {
                MoneyType();
            }
            else if (eventArg == "postFilterBranch")
            {
                JspostFilterBranch();
            }
            else if (eventArg == "postInsertRow")
            {
                JsPostInsertRow();
            }
            else if (eventArg == "postDeleteRow")
            {
                JsPostDeleteRow();
            }
            
        }

        public void SaveWebSheet()
        {

                    if (HdDateStatus.Value != "2")// save กรณีลงทะเบียนใหม่
                    {
                        SaveSF();//ลงทะเบียนรหัส
                        string member_no = HfMemberNo.Value;
                        string sqlassist = @"select assist_docno from asnreqmaster where member_no = '" + member_no + "' and assist_docno like 'SF%'";
                        dt = ta.Query(sqlassist);
                        dt.Next();
                        string assistdocno = dt.GetString("assist_docno").Trim();


                        DateTime dead_date = DateTime.ParseExact(hdate2.Value, "ddMMyyyy", WebUtil.TH);
                        string capitalstring = dead_date.ToString("yyyy");
                        Int32 capitalyyyy = Convert.ToInt32(capitalstring) + 543;
                        string remark, Dcase, member_receive;
                        decimal Age;
                        try
                        {
                            remark = DwDetail65.GetItemString(1, "remark");
                        }
                        catch { remark = ""; }
                        try
                        {
                            Dcase = DwDetail65.GetItemString(1, "member_dead_case");
                        }
                        catch { Dcase = ""; }
                        try
                        {
                            Age = DwDetail65.GetItemDecimal(1, "member_age");
                        }
                        catch { Age = 0; }
                        try
                        {
                            member_receive = DwReceive.GetItemString(1, "member_receive");
                        }
                        catch { member_receive = ""; }
                        string sqlinsert = @"insert into asnreqcapitaldet (capital_year,assist_docno,member_dead_date,member_dead_case,member_age,member_receive,pension_date,remark) values "
                            + "(" + capitalyyyy + ",'" + assistdocno + "',to_date('" + dead_date.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),'" + Dcase + "', " + Age + ",'" + member_receive + "',to_date('" + dead_date.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),'" + remark + "')";
                        ta.Exe(sqlinsert);
                        SaveReceive();
                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                    }
                    else if (HdDateStatus.Value == "2")//Save กรณี ที่เคยมีการลงทะเบียนกองทุน แล้ว
                    {
                        string member_no = HfMemberNo.Value;
                        string sqlassist = @"select assist_docno from asnreqmaster where member_no = '" + member_no + "' and assist_docno like 'SF%'";
                        dt = ta.Query(sqlassist);
                        dt.Next();
                        string assistdocno = dt.GetString("assist_docno").Trim();


                        DateTime dead_date = DateTime.ParseExact(HdDayDate.Value, "ddMMyyyy", WebUtil.TH);
                        string capitalstring = dead_date.ToString("yyyy");
                        Int32 capitalyyyy = Convert.ToInt32(capitalstring) + 543;

                        string remark, Dcase;
                        decimal Age;
                        try
                        {
                            remark = DwDetail65.GetItemString(1, "remark");
                        }
                        catch { remark = ""; }
                        try
                        {
                            Dcase = DwDetail65.GetItemString(1, "member_dead_case");
                        }
                        catch { Dcase = ""; }
                        try
                        {
                            Age = DwDetail65.GetItemDecimal(1, "member_age");
                        }
                        catch { Age = 0; }

                        SaveReceive();// ฟังก์ชั่น save ผู้รับผลประโยชน์
                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                    }
                    //-----------------------------------------
                    if (HdTotalpay.Value != "1")//save กรณีที่มีการ จ่ายเงิน
                    {
                    double assist_amt = DwDetail65.GetItemDouble(1, "assist_amt");
                    try
                    {
                        string sqlStr2 = @"UPDATE  asnreqmaster
                               SET     sumpay    = '" + HdTotalpay.Value + @"',assist_amt = '" + assist_amt + @"'  
                               WHERE   member_no  = '" + DwMem65.GetItemString(1, "member_no") + "' and assist_docno like 'SF%'";
                        ta.Exe(sqlStr2);
                        HdSumpay.Value = "";
                        Hdck.Value = "";
                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                    }
                    catch
                    {
                    }
                    SaveState();//บันทึกการจ่ายเงินแต่ละครั้ง
                    HdTotalpay.Value = "1";
                }
                else { }

           

        }

        public void WebSheetLoadEnd()
        {

            tDwMain65.Eng2ThaiAllRow();
            tDwDetail65.Eng2ThaiAllRow();

            DwMem65.SaveDataCache();
            DwMain65.SaveDataCache();
            DwDetail65.SaveDataCache();
            DwReceive.SaveDataCache();
           
            dt.Clear();
            ta.Close();
        }

        private void Refresh()
        {

        }

        private void ChangeAmt()
        {
            DateTime member_dead_date;
            string temp = DwMem65.GetItemString(1, "member_no");
            member_date = DwMain65.GetItemDateTime(1, "member_date");
              
            member_dead_date = DateTime.ParseExact(hdate2.Value, "ddMMyyyy", WebUtil.TH);
            DwDetail65.SetItemDateTime(1, "member_dead_date", member_dead_date);

            string sumpay_amt = "0";
            try
            {
                sqlStr = @"select sumpay 
                        from asnreqmaster 
                        where member_no = '" + temp + "' and assist_docno like 'SF%'";
                dt = ta.Query(sqlStr);
                if (dt.Next())
                {
                    sumpay_amt = dt.GetString("sumpay");
                }

                if (sumpay_amt == "")
                {
                    sumpay_amt = "0";
                }
                HdSumpay.Value = sumpay_amt;
                HdTotalpay.Value = "1";
            }
            catch
            {
            }

            try
            {

                string entry_date = DwDetail65.GetItemDateTime(1, "member_dead_date").ToString("yyyy");
                string memb_date = DwMain65.GetItemDateTime(1, "member_date").ToString("yyyy");
                string memNo = DwMem65.GetItemString(1, "member_no");
                // ดึงข้อมูลหุ้น แบบหักจากเงินเดือน
                sqlStr = @"select sum(share_amount) as sumshare , max(operate_date) ,min(operate_date)  
                        from shsharestatement
                        where shritemtype_code = 'SPM' and member_no = '" + memNo + "'";
                dt = ta.Query(sqlStr);
                decimal sumdate=0;
                decimal sumshare=0;
                DateTime maxdate = Convert.ToDateTime("01/01/2011");
                DateTime mindate = Convert.ToDateTime("01/01/2011");
                if (dt.Next())
                {
                    sumshare = dt.GetDecimal("sumshare");
                    sumdate = dt.GetDecimal("sumdate");
                    maxdate = dt.GetDate("max(operate_date)");
                    mindate = dt.GetDate("min(operate_date)");
                }
                TimeSpan sumday = maxdate - mindate;
                string sumdays = sumday.ToString();
                int leng = sumdays.IndexOf(".");
                string sumsumday = sumdays.Substring(0,leng);
                double memb_age = Convert.ToDouble(sumsumday);
                memb_age = memb_age / 365;

                double shareAmt = 0;
                double setDeadpay_amt = 0;
                shareAmt = Convert.ToDouble(sumshare);
                //-----------------------------------------------------------
                //ดึงข้อมูล ค่าเงินสูงสุด และ %การจ่ายแต่ละปี
                string sqlmaxandper = @"select max_pay,percent from asnucfpay65 where minmemb_year <= " +
                         memb_age + " and " + memb_age + " < maxmemb_year";
                dt = ta.Query(sqlmaxandper);
                dt.Next();
                int max_pay = Convert.ToInt32(dt.GetDecimal("max_pay"));
                double year_per = dt.GetDouble("percent");
                setDeadpay_amt = year_per * shareAmt;
                if (setDeadpay_amt > max_pay)
                {
                    setDeadpay_amt = max_pay;
                }
                string sqlyear_per = @"select percent,year,maxpay_peryear  from asnucfperpay 
                                        where assist_code =( select max(assist_code) 
                                        from asnucfperpay where assist_code like 'SF%' )";
                dt = ta.Query(sqlyear_per);
                dt.Next();
                double percent = dt.GetDouble("percent");
                int year_pay = Convert.ToInt32(dt.GetDecimal("year"));
                int maxpay_peryear = Convert.ToInt32(dt.GetDecimal("maxpay_peryear"));
                double perpay = setDeadpay_amt * percent;
                if (perpay >= maxpay_peryear)
                {
                    perpay = maxpay_peryear;
                }
                setDeadpay_amt = setDeadpay_amt - Convert.ToDouble(sumpay_amt);

                if (setDeadpay_amt <= 0)
                    setDeadpay_amt = 0;
                if (perpay % 10 != 0)
                {
                    perpay = perpay + (10 - (perpay % 10));
                }
                DwDetail65.SetItemDecimal(1, "assist_amt", Convert.ToDecimal(setDeadpay_amt));
                DwDetail65.SetItemDecimal(1, "sperpay", Convert.ToDecimal(perpay));
                string member_no = HfMemberNo.Value;
                string sqlcount = "select count(member_no) from asnslippayout where assisttype_code ='90' and member_no = '" + member_no + "'";
                dt = ta.Query(sqlcount);
                dt.Next();
                decimal count = dt.GetDecimal("count(member_no)");
                //นับงวดการจ่ายเงิน
                if (count >= Convert.ToInt32(year_pay))
                {
                    HdSumpay.Value = "";
                    LtServerMessage.Text = WebUtil.ErrorMessage("จ่ายครบ " + year_pay + "งวดแล้ว");
                }
                else { LtServerMessage.Text = WebUtil.CompleteMessage("จ่าย" + count + "งวดแล้ว"); }
                //-----------------------------------------------------------
                
            }
            catch (Exception er)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(er);
            }
            
        }

        private void ChangeHeight()
        {
            Decimal req_status;

            req_status = Convert.ToDecimal(HfReqSts.Value);

            //if (req_status == -9 || req_status == -8)
            //{
            //    DwMain.Modify("datawindow.detail.Height=1004");
            //}
            //else
            //{
            //    DwMain.Modify("datawindow.detail.Height=732");
            //}
        }

        private void RetreiveDwMem()
        {
            String member_no;
            decimal sumshare = 0;
            DwDetail65.Reset();
            DwDetail65.InsertRow(0);
            DwReceive.Reset();
            member_no = HfMemberNo.Value;

            try
            {
                sqlStr = @"select sum(share_amount) as sumshare
                        from shsharestatement
                        where shritemtype_code = 'SPM' and member_no = '" + member_no + "'";
                dt = ta.Query(sqlStr);
                if (dt.Next())
                {
                    sumshare = dt.GetDecimal("sumshare");
                    DwMain65.SetItemDecimal(1, "sharestk_amt", sumshare);
                }
                else { DwMain65.SetItemDecimal(1, "sharestk_amt", 0); }

            }
            catch
            {

            }
            object[] args = new object[1];
            args[0] = member_no;

            //DwMemP.Reset();
            DwUtil.RetrieveDataWindow(DwMem65, "kt_65years.pbl", null, args);
            if (DwMem65.RowCount < 1)
            {
                LtAlert.Text = "<script>Alert()</script>";
                return;
            }
            GetMemberDetail();
            
        }

        private void RetrieveDwMain()
        {
            String assist_docno, member_no;
            Int32 capital_year;

            assist_docno = HfAssistDocNo.Value;
            capital_year = Convert.ToInt32(HfCapitalYear.Value);
            member_no = HfMemNo.Value;

            object[] args1 = new object[1];
            args1[0] = HfMemNo.Value;

            DwMem65.Reset();
            DwUtil.RetrieveDataWindow(DwMem65, "kt_65years.pbl", null, args1);

            object[] args2 = new object[3];
            args2[0] = assist_docno;
            args2[1] = capital_year;
            args2[2] = member_no;

            DwMain65.Reset();
            DwUtil.RetrieveDataWindow(DwMain65, "kt_65years.pbl", tDwMain, args2);

            object[] args3 = new object[2];
            args3[0] = assist_docno;
            args3[1] = capital_year;

            DwDetail65.Reset();
            DwUtil.RetrieveDataWindow(DwDetail65, "kt_65years.pbl", null, args3);
            RetrieveBankBranch();
           
        }

        private void GetMemberDetail()
        {
            String member_no, prename_desc, memb_name, memb_surname, membgroup_code, membgroup_desc, membtype_code, membtype_desc;
            DateTime member_date, birth_date;
            //member_no = HfMemberNo.Value;
            member_no = HfMemberNo.Value;
            //member_no = int.Parse(member_no).ToString("00000000");

            DataStore Ds = new DataStore();
            Ds.LibraryList = "C:/GCOOP_ALL/EGAT/GCOOP/Saving/DataWindow/app_assist/as_seminar.pbl";
            Ds.DataWindowObject = "d_member_detail";

            Ds.SetTransaction(sqlca);
            Ds.Retrieve(member_no);

            if (Ds.RowCount < 1)
            {
                LtServerMessage.Text = WebUtil.WarningMessage("ไม่มีเลขที่สมาชิกนี้");
                return;
            }

            prename_desc = Ds.GetItemString(1, "prename_desc");
            memb_name = Ds.GetItemString(1, "memb_name");
            memb_surname = Ds.GetItemString(1, "memb_surname");
            membgroup_code = Ds.GetItemString(1, "membgroup_code");
            membgroup_desc = Ds.GetItemString(1, "membgroup_desc");
            member_date = Ds.GetItemDateTime(1, "member_date");
            membtype_code = Ds.GetItemString(1, "membtype_code");
            membtype_desc = Ds.GetItemString(1, "membtype_desc");
            birth_date = Ds.GetItemDateTime(1, "birth_date");

            rowcount = CheckReq(member_no);
            if (rowcount > 0)
            {
                LtServerMessage.Text = WebUtil.WarningMessage("สมาชิกรายนี้ขอทุนไปแล้ว");
                return;
            }

            DwMem65.SetItemString(1, "member_no", member_no);
            DwMem65.SetItemString(1, "prename_desc", prename_desc);
            DwMem65.SetItemString(1, "memb_name", memb_name);
            DwMem65.SetItemString(1, "memb_surname", memb_surname);
            DwMem65.SetItemString(1, "membgroup_desc", membgroup_desc);
            DwMem65.SetItemString(1, "membgroup_code", membgroup_code);
            DwMem65.SetItemString(1, "membtype_code", membtype_code);
            DwMem65.SetItemString(1, "membtype_desc", membtype_desc);
            DwMain65.SetItemDateTime(1, "member_date", member_date);
            DwMain65.SetItemString(1, "assisttype_code", "กองทุนสวัสดิการเงินสะสม 65 ปีมีสุข");

            tDwMain65.Eng2ThaiAllRow();
            //-----------------------------------------------------------
            //Decimal deadpay_amt = 0;
            //member_no = HfMemberNo.Value;
            try
            {
                DateTime pensiondate;
                string penddmmyyyy;
                Int32 yyyy;
                string pension;
                decimal age;
                string dead_case, member_receive;
                //ดึงวันที่ลงทะเบียนไว้
                string sqlDate = @"select asnreqcapitaldet.pension_date,asnreqcapitaldet.member_age,asnreqcapitaldet.member_dead_case,"+
                "asnreqcapitaldet.member_receive from asnreqcapitaldet join asnreqmaster on asnreqmaster.assist_docno=asnreqcapitaldet.assist_docno where asnreqmaster.member_no ='" + member_no + "' and asnreqmaster.assist_docno like 'SF%'";
                dt = ta.Query(sqlDate);
                if (dt.Next())
                {
                        age = dt.GetDecimal("member_age");
                        dead_case = dt.GetString("member_dead_case");
                        member_receive = dt.GetString("member_receive");
                        pensiondate = dt.GetDate("pension_date");
                        penddmmyyyy = pensiondate.ToString("ddMMyyyy" , WebUtil.TH);
                        pension = pensiondate.ToString("yyyy");
                        string a = "01/01/2011";
                        DateTime at = DateTime.ParseExact(a, "dd/mm/yyyy", WebUtil.TH);
                        //yyyy = Convert.ToInt32(pension);
                        //yyyy = yyyy + 543;
                        //penddmmyyyy = pensiondate.ToString("ddMM") + yyyy.ToString();
                        HdSumpay.Value = "0";

                        DwDetail65.SetItemDecimal(1, "member_age", age);
                        DwDetail65.SetItemString(1, "member_dead_case", dead_case);

                    if (pension != "1370" && pension != "")
                    {
                        HdDayDate.Value = penddmmyyyy;//เก็บ วว/ดด/ปปปป
                        HdDate.Value = pension;//เก็บ ปปปป
                        HdDateStatus.Value = "2";
                        HadDate();

                    }
                    else { HdDateStatus.Value = "0"; }
                }
            }
            catch
            {

                HdDateStatus.Value = "0";
            }

            
        }

        private void RetrieveBankBranch()
        {
            //String bank;

            //try
            //{
            //    //bank = DwMain.GetItemString(1, "expense_bank");
            //}
            //catch { bank = ""; }

            //DataWindowChild DcBankBranch = DwMain.GetChild("expense_branch");
            //DcBankBranch.SetTransaction(sqlca);
            //DcBankBranch.Retrieve();
            //DcBankBranch.SetFilter("bank_code = '" + bank + "'");
            //DcBankBranch.Filter();
        }

        private void NewClear()
        {   
            DwMem65.Reset();
            DwDetail65.Reset();
            DwMain65.Reset();
            //DwReceive.Reset();
            DwMain65.InsertRow(0);
            DwMem65.InsertRow(0);
            DwDetail65.InsertRow(0);

            tDwMain65.Eng2ThaiAllRow();
            tDwDetail65.Eng2ThaiAllRow();

            DwMain65.SetItemDecimal(1, "req_status", 8);
            DwMain65.SetItemDateTime(1, "pay_date", DateTime.Now);
            DwMain65.SetItemDateTime(1, "req_date", DateTime.Now);
            DwMain65.SetItemDateTime(1, "entry_date", DateTime.Now);
            DwMain65.SetItemDateTime(1, "approve_date", state.SsWorkDate);


        }

        private Decimal CheckReq(String member_no)
        {
            try
            {
                sqlStr = @"SELECT *
                           FROM   asnreqmaster
                           WHERE  member_no = '" + member_no + @"' and
                                  assist_docno like 'AM%'";

                dt = ta.Query(sqlStr);
                dt.Next();

                try
                {
                    rowcount = dt.GetRowCount();
                }
                catch { rowcount = 0; }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
            return rowcount;
        }

        private Decimal GetSeqNo()
        {
            try
            {
                sqlStr = @"SELECT max(seq_pay) as seq_pay
                           FROM   asnreqcapitaldet
                           WHERE  assist_docno like 'AM%' and
                                  capital_year = '" + capital_year + "'";
                dt = ta.Query(sqlStr);
                dt.Next();

                try
                {
                    seq_pay = dt.GetDecimal("seq_pay");
                }
                catch { seq_pay = 0; }

                seq_pay = seq_pay + 1;
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
            return seq_pay;
        }

        private void GetItemDwMain()
        {
            member_no = DwMem65.GetItemString(1, "member_no");
            membgroup_code = DwMem65.GetItemString(1, "membgroup_code");
            req_status = DwMain65.GetItemDecimal(1, "req_status");
            capital_year = DwMain65.GetItemDecimal(1, "capital_year");
            try
            {
                salary_amt = DwMain65.GetItemDecimal(1, "salary_amt");
            }
            catch { salary_amt = 0; }

            try
            {
                paytype_code = DwMain65.GetItemString(1, "paytype_code");
            }
            catch { paytype_code = ""; }
            try
            {
                pay_date = DwMain65.GetItemDateTime(1, "pay_date");
            }
            catch { }
            try
            {
                expense_bank = DwMain65.GetItemString(1, "expense_bank");
            }
            catch { expense_bank = ""; }
            try
            {
                expense_branch = DwMain65.GetItemString(1, "expense_branch");
            }
            catch { expense_branch = ""; }
            try
            {
                expense_accid = DwMain65.GetItemString(1, "expense_accid");
            }
            catch { expense_accid = ""; }

            try
            {
                cancel_date = DwMain65.GetItemDateTime(1, "cancel_date");
            }
            catch { }
            try
            {
                cancel_id = DwMain65.GetItemDecimal(1, "cancel_id");
            }
            catch { cancel_id = null; }
            try
            {
                remark_cancel = DwMain65.GetItemString(1, "remark");
            }
            catch { remark_cancel = ""; }
            
        }

        private String GetLastDocNo(Decimal capital_year)
        {
            try
            {
                sqlStr = @"SELECT last_docno 
                               FROM   asndoccontrol
                               WHERE  doc_prefix = 'AM' and
                                      doc_year = '" + capital_year + "'";
                dt = ta.Query(sqlStr);
                dt.Next();

                try
                {
                    assist_docno = dt.GetString("last_docno");
                    if (assist_docno == "")
                    {
                        assist_docno = "000001";
                    }
                    else if (assist_docno == "000000")
                    {
                        assist_docno = "000001";
                    }
                    else
                    {
                        assist_docno = "000000" + Convert.ToString(Convert.ToInt32(assist_docno) + 1);
                        assist_docno = WebUtil.Right(assist_docno, 6);
                    }

                    assist_docno = "AM" + assist_docno;
                    assist_docno = assist_docno.Trim();
                }
                catch
                {
                    assist_docno = "AM000001";
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }

            return assist_docno;
        }

        private void GetAssistType()
        {
            HdTy.Value = "90";
        }

        private void BDeadPayclick()
        {
            if (HdSumpay.Value != "")
            {
                decimal sump = Convert.ToDecimal(HdSumpay.Value);
                sump = sump + (DwDetail65.GetItemDecimal(1, "assist_amt") * 2);

                HdSumpay.Value = sump.ToString();

                HdTotalpay.Value = HdSumpay.Value.Trim();
                //DwDetail65.SetItemDecimal(1, "assist_amt", 0);
                CalPay();
            }
            else { LtServerMessage.Text = WebUtil.ErrorMessage("รายการนี้ได้ทำรายการไปแล้วจ่าย"); }

        }

        private void BPayclick()
        {
        if (HdSumpay.Value != "" && Hdck.Value != "1" )
            {
                Hdck.Value = "1";
                double totalpay = 0;
                decimal perpayTemp = DwDetail65.GetItemDecimal(1, "sperpay"); // ค่า 10%
                double sumpay_temp = Convert.ToDouble(HdSumpay.Value); // ค่า pay ที่เคยจ่ายไปแล้ว

                totalpay = Convert.ToDouble(perpayTemp) + sumpay_temp;
                HdTotalpay.Value = totalpay.ToString();
                double assist_value = DwDetail65.GetItemDouble(1, "assist_amt");
                double calculate = assist_value - Convert.ToDouble(perpayTemp);
                try
                {
                    if (calculate <= 0)
                        calculate = 0.00;
                    DwDetail65.SetItemDecimal(1, "assist_amt", Convert.ToDecimal(calculate));
                }
                catch
                {
                }

            }
            else { LtServerMessage.Text = WebUtil.ErrorMessage("รายการนี้ได้ทำการจ่ายแล้ว"); }
        }

        private void HadDate()
        {
                DateTime member_dead_date;
                string temp = DwMem65.GetItemString(1, "member_no");
                member_date = DwMain65.GetItemDateTime(1, "member_date");

                member_dead_date = DateTime.ParseExact(HdDayDate.Value, "ddMMyyyy", WebUtil.TH);
                DwDetail65.SetItemDateTime(1, "member_dead_date", member_dead_date);

                string sumpay_amt = "0";
                try
                {
                    sqlStr = @"select sumpay 
                           from asnreqmaster 
                           where member_no = '" + temp + "' and assist_docno like 'SF%'";
                    dt = ta.Query(sqlStr);
                    if (dt.Next())
                    {
                        sumpay_amt = dt.GetString("sumpay");
                    }

                    if (sumpay_amt == "")
                    {
                        sumpay_amt = "0";
                    }
                    HdSumpay.Value = sumpay_amt;
                    HdTotalpay.Value = "1";
                }
                catch
                {
                }
                string sqlQurymem = "select member_receive,percent_receive,pay_receive,receive_no,asnmembreceive.assist_docno "+
                    "from asnmembreceive,asnreqmaster where asnreqmaster.member_no ='" + HfMemberNo.Value + "' and "+
                    "asnmembreceive.assist_docno like 'SF%' and asnmembreceive.assist_docno = asnreqmaster.assist_docno ";
                dt = ta.Query(sqlQurymem);
                int w = 1;
                while (dt.Next())
                {
                    string receive = dt.GetString("member_receive");
                    string percentpay = dt.GetString("percent_receive");
                    string re_no = dt.GetString("receive_no");
                    double pay = dt.GetDouble("pay_receive");
                    string assist_no = dt.GetString("assist_docno");
                    DwReceive.InsertRow(0);
                    DwReceive.SetItemString(w, "member_receive", receive.Trim());
                    DwReceive.SetItemString(w, "percent_receive", percentpay.Trim());
                    DwReceive.SetItemDouble(w, "pay_receive", pay);
                    DwReceive.SetItemString(w, "receive_no", re_no);
                    DwReceive.SetItemString(w, "assist_docno", assist_no);
                    w++;
                }

                try
                {

                    string entry_date = DwDetail65.GetItemDateTime(1, "member_dead_date").ToString("yyyy");
                    string memb_date = DwMain65.GetItemDateTime(1, "member_date").ToString("yyyy");
                    string memNo = DwMem65.GetItemString(1, "member_no");
                    //ดึงหุ้น แบบหักจากเงินเดือน
                    sqlStr = @"select sum(share_amount) as sumshare , max(operate_date) ,min(operate_date)  
                           from shsharestatement
                           where shritemtype_code = 'SPM' and member_no = '" + memNo + "'";
                    dt = ta.Query(sqlStr);
                    decimal sumdate = 0;
                    decimal sumshare = 0;
                    DateTime maxdate = Convert.ToDateTime("01/01/2011");
                    DateTime mindate = Convert.ToDateTime("01/01/2011");
                    if (dt.Next())
                    {
                        sumshare = dt.GetDecimal("sumshare");
                        sumdate = dt.GetDecimal("sumdate");
                        maxdate = dt.GetDate("max(operate_date)");
                        mindate = dt.GetDate("min(operate_date)");
                    }
                    TimeSpan sumday = maxdate - mindate;
                    string sumdays = sumday.ToString();
                    int leng = sumdays.IndexOf(".");
                    string sumsumday = sumdays.Substring(0, leng);
                    double memb_age = Convert.ToDouble(sumsumday);
                    memb_age = memb_age / 365;

                    double shareAmt = 0;
                    double setDeadpay_amt = 0;
                    shareAmt = Convert.ToDouble(sumshare);
                    //--------------------------------------------
                    string sqlmaxandper = @"select max_pay,percent from asnucfpay65 where minmemb_year <= " +
                         memb_age + " and " + memb_age + " < maxmemb_year";
                    dt = ta.Query(sqlmaxandper);
                    dt.Next();
                    int max_pay = Convert.ToInt32(dt.GetDecimal("max_pay"));
                    double year_per = dt.GetDouble("percent");
                    setDeadpay_amt = year_per * shareAmt;
                    if (setDeadpay_amt > max_pay)
                    {
                        setDeadpay_amt = max_pay;
                    }
                    string sqlyear_per = @"select percent,year,maxpay_peryear  from asnucfperpay 
                                        where assist_code =( select max(assist_code) 
                                        from asnucfperpay where assist_code like 'SF%' )";
                    dt = ta.Query(sqlyear_per);
                    dt.Next();
                    double percent = dt.GetDouble("percent");
                    int year_pay = Convert.ToInt32(dt.GetDecimal("year"));
                    int maxpay_peryear = Convert.ToInt32(dt.GetDecimal("maxpay_peryear"));
                    double perpay = setDeadpay_amt * percent;
                    if (perpay >= maxpay_peryear)
                    {
                        perpay = maxpay_peryear;
                    }
                    setDeadpay_amt = setDeadpay_amt - Convert.ToDouble(sumpay_amt);
                    

                    if (setDeadpay_amt <= 0)
                        setDeadpay_amt = 0;
                    if (perpay % 10 != 0)
                    {
                        perpay = perpay + (10 - (perpay % 10));
                    }
                    DwDetail65.SetItemDecimal(1, "assist_amt", Convert.ToDecimal(setDeadpay_amt));
                    DwDetail65.SetItemDecimal(1, "sperpay", Convert.ToDecimal(perpay));
                    string member_no = HfMemberNo.Value;
                    string sqlcount = "select count(member_no) from asnslippayout where assisttype_code ='90' and member_no = '" + member_no + "'";
                    dt = ta.Query(sqlcount);
                    dt.Next();
                    decimal count = dt.GetDecimal("count(member_no)");
                    if (count >= Convert.ToInt32(year_pay))
                    {
                        HdSumpay.Value = "";
                        LtServerMessage.Text = WebUtil.ErrorMessage("จ่ายครบ " + year_pay + "งวดแล้ว");
                    }
                    else { LtServerMessage.Text = WebUtil.CompleteMessage("จ่าย" + count + "งวดแล้ว"); }

                    
                }
                catch (Exception er)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(er);
                }
            

        }

        private void SavePS()
        {
           

        }

        private void SaveSF()
        {
            string ass_docno = "";
            //string ck = "";
            string memNo = DwUtil.GetString(DwMem65, 1, "member_no");
            try
            {
                string qurryass = "select max(assist_docno) from asnreqmaster where assisttype_code = '90'and assist_docno like 'SF%'";
                DataTable qass = WebUtil.Query(qurryass);
                if (qass.Rows.Count > 0)
                {
                    ass_docno = qass.Rows[0][0].ToString();
                    if (ass_docno != "")
                    {
                        ass_docno = ass_docno.Substring(2, 6);
                    }
                    else { ass_docno = "000000"; }
                }
            }
            catch
            {
                ass_docno = "000000";
            }
            DateTime dead_date = DateTime.ParseExact(hdate2.Value, "ddMMyyyy", WebUtil.TH);
            string yyyynow = dead_date.ToString("yyyy");
            Int32 dDate = Convert.ToInt32(yyyynow) + 543;
            Decimal ass = Convert.ToDecimal(ass_docno);
            ass++;
            string AssAmt = "SF" + ass.ToString("000000");
            try
            {
                string savesql = "insert into asnreqmaster (assist_docno,capital_year,member_no,assisttype_code) values ('" + AssAmt + "'," + dDate + ",'" + memNo + "','90')";
                DataTable savekt = WebUtil.Query(savesql);

            }

            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถบันทึกข้อมูล ในกองทุนได้");
            }

        }

        private void SaveState()
        {
            string entry_id = state.SsUsername;
            
            string payout_no="";
            string yearnow = DateTime.Now.ToString("yyyy");
            yearnow = yearnow.Substring(2,2);
            Int32 sum543 = Convert.ToInt32(yearnow) + 543;
            yearnow = sum543.ToString();
            yearnow = yearnow.Substring(1,2);
  
                string moneyType = HdMoneyType.Value;//DwMain65.GetItemString(1, "moneytype_code");
                string member_no = HfMemberNo.Value;
                DateTime stamentdate = DwMain65.GetItemDateTime(1, "entry_date");
                decimal payout = DwDetail65.GetItemDecimal(1, "assist_amt");
                if (payout != 0)
                {
                    payout = DwDetail65.GetItemDecimal(1, "sperpay");
                }

                string sqlQu = "select max(payoutslip_no) from asnslippayout where payoutslip_no like '" + yearnow + HdTy.Value + "%'";
                DataTable slip_no = WebUtil.Query(sqlQu);
                if (slip_no.Rows.Count > 0)
                {
                    payout_no = slip_no.Rows[0][0].ToString();
                    if (payout_no != "")
                    {
                        payout_no = payout_no.Substring(4,6);
                    }
                    else { payout_no = "000000"; }
                }
                Decimal ass = Convert.ToDecimal(payout_no);
                ass++;
                string slippayout_no = yearnow + HdTy.Value + ass.ToString("000000");
                if (moneyType == "TRN")
                {
                    string sqlupdateasn = @"update asnreqmaster set moneytype_code = '" + moneyType + "' , to_system = 'DIV' where member_no = '" + member_no + "' and assisttype_code = '90'";
                    ta.Exe(sqlupdateasn);

                    string sqlsavestate = @"insert into asnslippayout (payoutslip_no,member_no,slip_date,operate_date,payout_amt,slip_status,assisttype_code,entry_id,entry_date,moneytype_code)" +
                        "values('" + slippayout_no + "','" + member_no + "',to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy')," +
                     +payout + ",1,'" + HdTy.Value + "','" + entry_id + "',to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),'" + moneyType + "')";
                    ta.Exe(sqlsavestate);
                }
                else if (moneyType == "CBT")
                {
                    string bankCode = DwMain65.GetItemString(1, "asnslippayout_bank_code");
                    string bankBranch = DwMain65.GetItemString(1, "asnslippayout_bankbranch_id");
                    string bankAcc = DwMain65.GetItemString(1, "asnslippayout_bank_accid");

                    string sqlupdateasn = @"update asnreqmaster set moneytype_code = '" + moneyType + "' where member_no = '" + member_no + "' and assisttype_code = '90'";
                    ta.Exe(sqlupdateasn);

                    string sqlsavestate = @"insert into asnslippayout (payoutslip_no,member_no,slip_date,operate_date,payout_amt,slip_status,assisttype_code,entry_id,entry_date,moneytype_code,bank_code,bankbranch_id,bank_accid)" +
                        "values('" + slippayout_no + "','" + member_no + "',to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy')," +
                     +payout + ",1,'" + HdTy.Value + "','" + entry_id + "',to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),'" + moneyType + "','" + bankCode + "','" + bankBranch + "','" + bankAcc + "')";
                    ta.Exe(sqlsavestate);
                }
                else
                {
                    string sqlupdateasn = @"update asnreqmaster set moneytype_code = '" + moneyType + "' where member_no = '" + member_no + "' and assisttype_code = '90'";
                    ta.Exe(sqlupdateasn);

                    string sqlsavestate = @"insert into asnslippayout (payoutslip_no,member_no,slip_date,operate_date,payout_amt,slip_status,assisttype_code,entry_id,entry_date,moneytype_code)" +
                        "values('" + slippayout_no + "','" + member_no + "',to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy')," +
                     +payout + ",1,'" + HdTy.Value + "','" + entry_id + "',to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),'" + moneyType + "')";
                    ta.Exe(sqlsavestate);
                }

        }

        private void MoneyType()
        {
            HdMoneyType.Value = DwMain65.GetItemString(1, "moneytype_code");
        }

        private void JspostFilterBranch()
        {
                //DwMain65.
            try
            {
                String bankcode = DwMain65.GetItemString(1, "asnslippayout_bank_code");
                DwUtil.RetrieveDDDW(DwMain65, "asnslippayout_bankbranch_id", "kt_65years.pbl", null);
                DataWindowChild dc = DwMain65.GetChild("asnslippayout_bankbranch_id");
                dc.SetFilter("bank_code ='" + bankcode + "'");
                dc.Filter();
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }

        }
        private void JsPostInsertRow()
        {
            DwReceive.InsertRow(0);
        }
        private void JsPostDeleteRow()
        {
            try
            {
                string assist_no = DwReceive.GetItemString(Convert.ToInt32(HdDwRow.Value), "assist_docno");
                string rece_no = DwReceive.GetItemString(Convert.ToInt32(HdDwRow.Value), "receive_no");
                string sql = "delete asnmembreceive where assist_docno = '" + assist_no + "' and receive_no ='" + rece_no + "'";
                ta.Exe(sql);
                DwReceive.DeleteRow(int.Parse(HdDwRow.Value));


            }
            catch
            {
                DwReceive.DeleteRow(int.Parse(HdDwRow.Value));
            }
        }
               private void SaveReceive()
        {
            if (HdRowReceive.Value == "เพิ่ม")
            {

                int rowww = DwReceive.RowCount + 1;
                string sqlcount = "select count(receive_no) from asnmembreceive where assist_docno = (select assist_docno from asnreqmaster  where member_no ='" + HfMemberNo.Value + "' and assist_docno like 'SF%') ";
                dt = ta.Query(sqlcount);
                dt.Next();
                int count = Convert.ToInt32(dt.GetDecimal("count(receive_no)"));
                int row2 = rowww - 1 - count;
                ////เพิ่ม 
                int rowcount = 1;
                string assist_docnoo = "";
                if (count != 0)
                {
                    string sqlrow = "select receive_no,assist_docno from asnmembreceive where receive_no =" +
                                    " (select max(receive_no) from asnmembreceive where " +
                                    " assist_docno = (select assist_docno from asnreqmaster  where member_no ='" + HfMemberNo.Value + "' and assist_docno like 'SF%')) and" +
                                    " assist_docno = (select assist_docno from asnreqmaster  where member_no ='" + HfMemberNo.Value + "' and assist_docno like 'SF%')";

                    dt = ta.Query(sqlrow);
                    dt.Next();
                    rowcount = Convert.ToInt32(dt.GetString("receive_no"));
                    rowcount++;
                    assist_docnoo = dt.GetString("assist_docno");
                }
                else if (count == 0)
                {
                    rowcount = 1;
                    string sqlassistno = "select assist_docno from asnreqmaster where member_no ='" + HfMemberNo.Value + "' and assist_docno like 'SF%'";
                    dt = ta.Query(sqlassistno);
                    dt.Next();
                    assist_docnoo = dt.GetString("assist_docno").Trim();
                }
                for (int k = count + 1; k < rowww; k++)
                {
                    string name = DwReceive.GetItemString(k, "member_receive");
                    string per = DwReceive.GetItemString(k, "percent_receive");
                    string pay = DwReceive.GetItemString(k, "pay_receive");
                    string rownext = rowcount.ToString("000");

                    string sqlinsertrow = "insert into asnmembreceive (assist_docno,receive_no,member_receive,percent_receive,pay_receive)" +
                        "values ('" + assist_docnoo + "','" + rownext + "','" + name + "','" + per + "',"+pay+")";
                    ta.Exe(sqlinsertrow);
                    //เพิ่ม
                    rowcount++;
                }
                DwUtil.UpdateDateWindow(DwReceive, "kt_65years.pbl", "asnmembreceive");
                HdRowReceive.Value = "";
            }
            else if (HdRowReceive.Value == "")
            {
                
                DwUtil.UpdateDateWindow(DwReceive, "kt_65years.pbl", "asnmembreceive");
            }
        }
        private void CalPay()
        {
            //int i = 0;
            //string n = "";
            //while (n != "0")//count row
            //{
            //    try
            //    {
            //        i++;
            //        n = DwReceive.GetItemString(i, "member_receive");
            //    }
            //    catch
            //    { n = "0"; }
            //}
            int rowww = DwReceive.RowCount + 1;
            int q = 1;
            while (q != rowww) //cal
            {
                double allpay = Convert.ToDouble(DwDetail65.GetItemDecimal(1, "assist_amt"));
                string per = DwReceive.GetItemString(q, "percent_receive");
                per = per.Replace("%", "").Trim();
                double cal = allpay * (Convert.ToDouble(per) / 100);
                DwReceive.SetItemDouble(q, "pay_receive", cal);

                q++;
            }
            DwDetail65.SetItemDecimal(1, "assist_amt", 0);
        }
    }
}
