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
    public partial class w_sheet_kt_pension_dead : PageWebSheet, WebSheet
    {
        protected DwThDate tDwDetail;
        protected String postDeadDate;
        protected String postPayClick;
        protected String postInsertRow;
        protected String postDeleteRow;
        protected String postMoneyType;
        protected String postFilterBranch;
        protected String postRetreiveDwMem;
        protected String postRetrieveDwMain;
        protected String postGetMemberDetail;
        protected String postRetrieveBankBranch;
        protected String postBankAccid;
        protected Sta ta;
        protected Sdt dt;

        public void InitJsPostBack()
        {
            postRetreiveDwMem = WebUtil.JsPostBack(this, "postRetreiveDwMem");
            postRetrieveDwMain = WebUtil.JsPostBack(this, "postRetrieveDwMain");
            postGetMemberDetail = WebUtil.JsPostBack(this, "postGetMemberDetail");
            postRetrieveBankBranch = WebUtil.JsPostBack(this, "postRetrieveBankBranch");
            postMoneyType = WebUtil.JsPostBack(this, "postMoneyType");
            postFilterBranch = WebUtil.JsPostBack(this, "postFilterBranch");
            postDeadDate = WebUtil.JsPostBack(this, "postDeadDate");
            postInsertRow = WebUtil.JsPostBack(this, "postInsertRow");
            postDeleteRow = WebUtil.JsPostBack(this, "postDeleteRow");
            postPayClick = WebUtil.JsPostBack(this, "postPayClick");
            postBankAccid = WebUtil.JsPostBack(this, "postBankAccid");
            tDwDetail = new DwThDate(DwDetailP, this);
            tDwDetail.Add("member_dead_date", "member_dead_tdate");
        }

        public void WebSheetLoadBegin()
        {
            tDwDetail.Eng2ThaiAllRow();
            this.ConnectSQLCA();
            ta = new Sta(sqlca.ConnectionString);
            dt = new Sdt();
            if (!IsPostBack)
            {
                DwMemP.Reset();
                DwDetailP.Reset();
                DwMainP.Reset();
                DwMainP.InsertRow(0);
                DwMemP.InsertRow(0);
                DwDetailP.InsertRow(0);
            }
            else
            {
                this.RestoreContextDw(DwMemP);
                this.RestoreContextDw(DwMainP);
                this.RestoreContextDw(DwDetailP);
                this.RestoreContextDw(DwReceive);
            }
            DwUtil.RetrieveDDDW(DwMainP, "paytype_code", "kt_pension.pbl", null);
            DwUtil.RetrieveDDDW(DwMainP, "moneytype_code", "kt_pension.pbl", null);
            DwUtil.RetrieveDDDW(DwMainP, "asnslippayout_bank_code", "kt_pension.pbl", null);
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postRetreiveDwMem")
            {
                RetreiveDwMem();
            }
            else if (eventArg == "postDeadDate")
            {
                ChangDeadDate();
            }
            else if (eventArg == "postRetrieveDwMain")
            {
                //RetrieveDwMain();
            }
            else if (eventArg == "postRetrieveBankBranch")
            {
                //RetrieveBankBranch();
            }
            else if (eventArg == "postGetMemberDetail")
            {
                GetMemberMain();
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
            else if (eventArg == "postPayClick")
            {
                JspostPayClick();
            }
            else if (eventArg == "postBankAccid")
            {
                JspostBankAccid();
            }

        }

        public void SaveWebSheet()
        {
            string Assist_docno = "", remark = "", deadcase = "", member_dead_tdate = "";
            string moneytype_code, bank_code, bankbranch_id, bank_accid ,pay_status="";
            int capital_year = DateTime.Now.Year + 543;
            DateTime member_dead_date = DateTime.Now;
            decimal perpay = 0, assist_amt = 0, sumpays = 0, balance = 0, member_age = 0 , approve_amt = 0;
            
            string checkAssist = "select assist_docno from asnreqmaster where member_no = '" + HdMemberNo.Value + "' and assist_docno like 'PS%'";
            DataTable ck = WebUtil.Query(checkAssist);
            try
            {
                Assist_docno = ck.Rows[0][0].ToString().Trim();
            }
            catch
            {
                savePS();
                checkAssist = "select assist_docno from asnreqmaster where member_no = '" + HdMemberNo.Value + "' and assist_docno like 'PS%'";
                ck = WebUtil.Query(checkAssist);
                Assist_docno = ck.Rows[0][0].ToString().Trim();
            }
            SaveReceive();
            try
            {
                moneytype_code = DwMainP.GetItemString(1, "moneytype_code");
            }
            catch
            { moneytype_code = "CSH"; }
            if (moneytype_code == "CBT")
            {
                bank_code = DwMainP.GetItemString(1, "asnslippayout_bank_code");
                bankbranch_id = DwMainP.GetItemString(1, "asnslippayout_bankbranch_id");
                bank_accid = DwMainP.GetItemString(1, "asnslippayout_bank_accid");
            }
            try
            {
                perpay = DwDetailP.GetItemDecimal(1, "perpay");
            }
            catch { }
            try
            {
                assist_amt = DwDetailP.GetItemDecimal(1, "assist_amt");
            }
            catch { }
            try
            {
                sumpays = DwDetailP.GetItemDecimal(1, "sumpays");
            }
            catch { }
            try
            {
                approve_amt = DwDetailP.GetItemDecimal(1, "approve_amt");
            }
            catch { }
            try
            {
                balance = DwDetailP.GetItemDecimal(1, "balance");
                //balance = Convert.ToDecimal(HdPayout.Value);
               
            }
            catch { }
            try
            {
                member_age = DwDetailP.GetItemDecimal(1, "member_age");
            }
            catch { }
            try
            {
                member_dead_tdate = DwDetailP.GetItemString(1, "member_dead_tdate");
                member_dead_date = DateTime.ParseExact(member_dead_tdate, "ddMMyyyy", WebUtil.TH);
            }
            catch { }

            try
            {
                deadcase = DwDetailP.GetItemString(1, "member_dead_case");
            }
            catch { deadcase = ""; }
            try
            {
                remark = DwDetailP.GetItemString(1, "remark");
            }
            catch { remark = ""; }
            //if (deadcase != "" || DwDetailP.GetItemDecimal(1, "balance") == 0)
            //{
                pay_status = "-9";
            //}
            try
            {
                String SQLUpdateASN = "update asnreqmaster set perpay = " + perpay + ", sumpays =" + sumpays + " , balance =" + balance + " , assist_amt =" + assist_amt + ", approve_amt = "+approve_amt+
                    ",pay_date = to_date('" + DateTime.Now.ToString("ddMMyyyy", WebUtil.EN) + "','ddmmyyyy'),pay_status= "+
                    pay_status + ",dead_status ='1' ,dead_date=to_date('" + member_dead_date.ToString("ddMMyyyy", WebUtil.EN) + "','ddmmyyyy') , calculate_date = to_date('" + member_dead_date.ToString("ddMMyyyy", WebUtil.EN) + "','ddmmyyyy') where assist_docno = '" + Assist_docno.Trim() + "'";
                WebUtil.Query(SQLUpdateASN);
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("บันทึกไม่สำเร็จ");
            }
            //if (balance == 0)
            //{
            //    try
            //    {
            //        String SQLInsertReqCap = "insert into asnreqcapitaldet(capital_year,assist_docno,member_dead_date,member_age,member_dead_case,remark,assist_amt) values ('" + capital_year + "','" + Assist_docno + "',to_date('" + member_dead_date.ToString("ddMMyyyy", WebUtil.EN) + "','ddmmyyyy'),'" + member_age + "','" + deadcase + "','" + remark + "'," + assist_amt + ")";
            //        WebUtil.Query(SQLInsertReqCap);
            //        //String SQLInsertSlip = "insert into asnslippayout(payoutslip_no,member_no,slip_date,operate_date,payout_amt,slip_status,assisttype_code,moneytype_code,bank_code,bankbranch_id,bank_accid,entry_id,entry_date) "+
            //        //                                            "values ('','" + HdMemberNo.Value + "',to_date('" + DateTime.Now.ToString("ddMMyyyy", WebUtil.EN) + "','ddmmyyyy'),"+HdPayout.Value+",'1','90','','','','','','','')";
            //        //SaveState();
            //    }
            //    catch
            //    {
            //        LtServerMessage.Text = WebUtil.ErrorMessage("หมายเลขสมาชิกนี้มีการกดจ่าย กรณตีเสียชีวิตแล้ว");
            //    }
            //}
            //else
            //{
            //    LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากดจ่ายเงิน");
            //}
            
        }

        public void WebSheetLoadEnd()
        {
            DwMemP.SaveDataCache();
            DwMainP.SaveDataCache();
            DwDetailP.SaveDataCache();
            DwReceive.SaveDataCache();

            dt.Clear();
            ta.Close();
        }

        private void RetreiveDwMem()
        {
            DwDetailP.Reset();
            DwDetailP.InsertRow(0);
            DwReceive.Reset();
            String member_no;
            Decimal deadpay_amt = 0;
            member_no = HdMemberNo.Value;

                try
                {
                    // เพิ่มค่าในช่องหุ้น
                    string sqlStr = "select sharestk_amt from shsharemaster where member_no = '" + member_no + "'";
                    dt = ta.Query(sqlStr);
                    if (dt.Next())
                    {
                        deadpay_amt = dt.GetDecimal("sharestk_amt")*10;
                        DwMainP.SetItemDecimal(1, "sharestk_amt", deadpay_amt);
                    }
                    else { DwMainP.SetItemDecimal(1, "sharestk_amt", 0); }
                }
                catch
                {

                }

                //DwMemP.Reset();
                DwUtil.RetrieveDataWindow(DwMemP, "kt_pension.pbl", null, member_no);
                if (DwMemP.RowCount < 1)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่มีเลขที่สมาชิกนี้");
                    //LtAlert.Text = "<script>Alert()</script>";
                    //return;
                }
                GetMemberMain();


        }

        private void GetMemberMain()
        {
            string member_no = HdMemberNo.Value;
            string member_tdate, work_tdate, req_tdate, dead_tdate, dead_status;
            DateTime member_date, work_date;
            try // Set วันที่เข้าเป็นสมาชิก
            {
                string SQLMemberDate = "select member_date,resign_date , birth_date from mbmembmaster where member_no = '" + member_no + "'";
                Sdt dt = WebUtil.QuerySdt(SQLMemberDate);
                string SQLGetAsnreqmaster = "select req_date,dead_date , dead_status from asnreqmaster where member_no = '" + HdMemberNo.Value + "' and assisttype_code = '80'";
                Sdt dtDate = WebUtil.QuerySdt(SQLGetAsnreqmaster);
                if (dt.Next())
                {
                    try
                    {
                        dtDate.Next();
                        try
                        {
                            req_tdate = dtDate.GetDateTh("req_date");
                            HdReqDate.Value = req_tdate;
                            if (req_tdate == "01/01/1913")
                                req_tdate = DateTime.Now.Date.ToString("dd/MM/yyyy", WebUtil.TH).Substring(0, 10).Trim();
                            DwMainP.SetItemString(1, "req_tdate", req_tdate);
                        }
                        catch { }
                        dead_status = dtDate.GetString("dead_status");
                        dead_tdate = dtDate.GetDateTh("dead_date");
                        if (dead_status == "1")
                            LtServerMessage.Text = WebUtil.WarningMessage("หมายเลขสมาชิกนี้ ตายเมื่อวันที่ " + dead_tdate);
                        HdDeadDate.Value = dead_tdate.Replace("/", "");
                        DwDetailP.SetItemString(1, "member_dead_tdate", dead_tdate.Replace("/", ""));
                        ChangDeadDate();
                    }
                    catch 
                    { }
                    member_tdate = dt.GetDateTh("member_date");
                    HdBirthDate.Value = dt.GetDateTh("birth_date");
                    HdDeadDate.Value = dt.GetDateTh("resign_date");
                    DwMainP.SetItemString(1, "member_tdate", member_tdate);
                    DwMainP.SetItemString(1, "birth_tdate", HdBirthDate.Value);
                }
            }
            catch { }
            try // Set วันที่ทำการ
            {
                work_tdate = DateTime.Now.Date.ToString("dd/MM/yyyy",WebUtil.TH).Substring(0,10).Trim();
                
                DwMainP.SetItemString(1, "entry_tdate", work_tdate);
                //DwDetailP.SetItemString(1, "calculate_tdate", work_tdate.Replace("/",""));
            }
            catch { }

            // Set ประเภทกองทุน
            DwMainP.SetItemString(1, "assisttype_code", "กองทุนสวัสดิการบำนาญสมาชิก");
            GetMemberDetail();
        }

        private void GetMemberDetail()
        {
            decimal sumpays = 0, perpay, approve_amt, balance, assist_amt;
            try
            {
                string SQLGetAsnreqmaster = "select * from asnreqmaster where member_no = '" + HdMemberNo.Value + "' and assist_docno like 'PS%'";
                Sdt dtAsn = WebUtil.QuerySdt(SQLGetAsnreqmaster);
                try
                {
                    // ดึงยอดหนี้ GetMemberDetail
                    string SQLGetLnCont = "select principal_balance from lncontmaster where member_no ='" + HdMemberNo.Value + "'";
                    Sdt dtlncont = WebUtil.QuerySdt(SQLGetLnCont);
                    if (dtlncont.Next())
                    {
                        decimal ln = dtlncont.GetDecimal("principal_balance");
                        DwDetailP.SetItemDecimal(1, "principal_balance", ln);
                    }
                    else DwDetailP.SetItemDecimal(1, "principal_balance", 0);
                }
                catch { }  
                if (dtAsn.Next())
                {
                    sumpays = dtAsn.GetDecimal("sumpays");
                    perpay = dtAsn.GetDecimal("perpay");
                    approve_amt = dtAsn.GetDecimal("approve_amt");
                    balance = dtAsn.GetDecimal("balance");
                    assist_amt = dtAsn.GetDecimal("assist_amt");

                    DwDetailP.SetItemDecimal(1, "sumpays", sumpays);
                    DwDetailP.SetItemDecimal(1, "perpay", perpay);
                    DwDetailP.SetItemDecimal(1, "approve_amt", approve_amt);
                    DwDetailP.SetItemDecimal(1, "balance", balance);
                    DwDetailP.SetItemDecimal(1, "assist_amt", assist_amt);
                    HdPayout.Value = balance.ToString();
                }
                else 
                {
                    DwDetailP.SetItemDecimal(1, "sumpays", 0);
                    DwDetailP.SetItemDecimal(1, "perpay", 0);
                    DwDetailP.SetItemDecimal(1, "approve_amt", 0);
                    DwDetailP.SetItemDecimal(1, "balance", 0);
                    DwDetailP.SetItemDecimal(1, "assist_amt", 0);
                }
                //DwDetailP.SetItemString(1, "member_dead_tdate", HdDeadDate.Value.Replace("/", ""));
                //if (DwDetailP.GetItemString(1, "member_dead_tdate") == "01011913")
                //{
                //    DwDetailP.SetItemString(1, "member_dead_tdate", "00000000");
                //}
            }
            catch { }
            try
            {
                sumpays = DwDetailP.GetItemDecimal(1, "sumpays");
            }
            catch { }
                ChangDeadDate();
                

        }

        private void MoneyType()
        {
            HdMoneyType.Value = DwMainP.GetItemString(1, "moneytype_code");
        }

        private void JspostFilterBranch()
        {
            try
            {
                String bankcode = DwMainP.GetItemString(1, "asnslippayout_bank_code");
                DwUtil.RetrieveDDDW(DwMainP, "asnslippayout_bankbranch_id", "kt_pension.pbl", null);
                DataWindowChild dc = DwMainP.GetChild("asnslippayout_bankbranch_id");
                dc.SetFilter("bank_code ='" + bankcode + "'");
                dc.Filter();
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        private void ChangDeadDate()
        {
            try
            {
                DateTime dead_date, birth_date;
                string birth_tdate = HdBirthDate.Value;
                string temp;
                //string member_dead_tdate = DwDetailP.GetItemString(1, "member_dead_tdate");
                HdDeadDate.Value = DwDetailP.GetItemString(1, "member_dead_tdate");
                temp = HdDeadDate.Value;
                temp = temp.Replace("/", "");
                temp = temp.Insert(2, "/");
                temp = temp.Insert(5, "/");
                HdDeadDate.Value = temp;

                birth_date = DateTime.ParseExact(birth_tdate, "dd/MM/yyyy", WebUtil.TH);
                dead_date = DateTime.ParseExact(HdDeadDate.Value, "dd/MM/yyyy", WebUtil.TH);

                decimal Age = (dead_date.Year - birth_date.Year);

                DwDetailP.SetItemDecimal(1, "member_age", Age);



                DwMainP.SetItemString(1, "req_tdate", HdDeadDate.Value);
                DwDetailP.SetItemString(1, "calculate_tdate", HdDeadDate.Value.Replace("/",""));
                decimal sumpays = DwDetailP.GetItemDecimal(1, "sumpays");
                Calculate(sumpays);           
                //decimal sumpays = DwDetailP.GetItemDecimal(1, "sumpays");
                //Calculate(sumpays);
            }
            catch
            {
            }
            

        }

        private void Calculate(decimal sumpays)
        {
            try
            {
                //---------------------------------------------
                //ดึงข้อมูล ค่าเงินสูงสุด และ %การจ่ายแต่ละปี
                int memb_age = 0;
                double shareAmt = 0;
                double setDeadpay_amt = 0;

                double totalMem_days = 0.00;
                int total_year =0;
                try
                {
                    DateTime member_date = DateTime.ParseExact(DwMainP.GetItemString(1, "member_tdate"), "dd/MM/yyyy", WebUtil.TH);
                    DateTime dead_date = DateTime.ParseExact(HdDeadDate.Value, "dd/MM/yyyy", WebUtil.TH); //DateTime.ParseExact("30/09/" + DateTime.Now.Year + 543, "dd/MM/yyyy", WebUtil.TH);
                    totalMem_days = (dead_date - member_date).TotalDays;
                    total_year = Convert.ToInt32(totalMem_days / 365);
                }
                catch { }
                try
                {
                    double totalBirth_days = 0.00;
                    DateTime birth_date = DateTime.ParseExact(DwMainP.GetItemString(1, "birth_tdate"), "dd/MM/yyyy", WebUtil.TH);
                    DateTime dead_date = DateTime.ParseExact(HdDeadDate.Value, "dd/MM/yyyy", WebUtil.TH);
                    if (dead_date.Month != 9)
                    {
                        totalBirth_days = (dead_date - birth_date).TotalDays;
                    }
                }
                catch { }

                string Tage = DwMainP.GetItemString(1, "member_years_disp");
                memb_age = Convert.ToInt32(Tage.Replace("ปี", ""));
                shareAmt = Convert.ToDouble(DwMainP.GetItemDecimal(1, "sharestk_amt"));

                string sqlmaxandper = @"select max_pay,percent from asnucfpayps where minmemb_year <= " +
                      memb_age + " and maxmemb_year >=" + memb_age + "";
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
                                        from asnucfperpay where assist_code like 'PS%' )";
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
                //setDeadpay_amt = setDeadpay_amt - Convert.ToDouble(sumpays);

                if (setDeadpay_amt <= 0)
                    setDeadpay_amt = 0;
                //if (perpay % 10 > 1)
                //{
                //    perpay = perpay + (10 - (perpay % 10));
                //}
                DwDetailP.SetItemDecimal(1, "assist_amt", Convert.ToDecimal(setDeadpay_amt));
                DwDetailP.SetItemDecimal(1, "perpay", Convert.ToDecimal(perpay));

                if (DwDetailP.GetItemDecimal(1, "approve_amt") == 0)
                {
                    DwDetailP.SetItemDecimal(1, "approve_amt", Convert.ToDecimal(setDeadpay_amt));
                }
                decimal balance = Convert.ToDecimal(setDeadpay_amt) - sumpays;
                if (balance < 0)
                    balance = 0;
                DwDetailP.SetItemDecimal(1, "balance", balance);

            }catch{}

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

        private void JspostPayClick()
        {
            decimal assist_amt = DwDetailP.GetItemDecimal(1, "assist_amt");
            decimal sumpays = DwDetailP.GetItemDecimal(1, "sumpays");
            if (sumpays < assist_amt && sumpays >= 0)
            {
                decimal balance = DwDetailP.GetItemDecimal(1, "balance");
                decimal lncont = DwDetailP.GetItemDecimal(1, "principal_balance");
                decimal sum = balance - lncont;

                HdPayout.Value = balance.ToString();

                string balance_detail = balance.ToString("#,##0.00") + " - " + lncont.ToString("#,##0.00") + " = " + sum.ToString("#,##0.00");
                DwDetailP.SetItemString(1, "balance_detail", balance_detail);

                sumpays = sumpays + balance;
                DwDetailP.SetItemDecimal(1, "sumpays", sumpays);
                if (sum > 0)
                    sum = 0;
                //DwDetailP.SetItemDecimal(1, "balance", sum);
                DwDetailP.SetItemDecimal(1, "balance", 0);
            }
        }

        private void savePS()
        {
            string ass_docno = "";
            int cap_year = state.SsWorkDate.Year + 543;
            //string ck = "";
            string memNo = DwUtil.GetString(DwMemP, 1, "member_no");
            try
            {
                string qurryass = "select max(assist_docno) from asnreqmaster where assisttype_code = '80' and assist_docno like 'PS%' and capital_year = '"+cap_year+"'";
                DataTable qass = WebUtil.Query(qurryass);
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

            string dead_date = DwDetailP.GetItemString(1, "member_dead_tdate");
            string yyyynow = dead_date.Substring(4, 4);
            //Int32 dDate = Convert.ToInt32(yyyynow) + 543;
            Decimal ass = Convert.ToDecimal(ass_docno);
            Decimal sharestk_amt = 0;
            ass++;
            string AssAmt = "PS" + yyyynow.Substring(2, 2) + ass.ToString("00000");
            try
            {
                string savesql = "insert into asnreqmaster (assist_docno,capital_year,member_no,assisttype_code,req_date,approve_date) values ('" + AssAmt + "'," + yyyynow + ",'" + memNo + "','80',to_date('" + DateTime.Now.ToString("ddMMyyyy", WebUtil.EN) + "','ddmmyyyy'),to_date('" + DateTime.Now.ToString("ddMMyyyy", WebUtil.EN) + "','ddmmyyyy'))";
                WebUtil.Query(savesql);
                sharestk_amt = DwMainP.GetItemDecimal(1, "sharestk_amt");
                string QueryInsPensionperpay = "insert into asnpensionperpay (member_no, sharestk_amt) values ('"+memNo+"','"+sharestk_amt+"')";
                WebUtil.Query(QueryInsPensionperpay);

            }

            catch(Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถบันทึกข้อมูล ในกองทุนได้ : " + ex);
            }
            //try
            //{
            //    string upsql = "update asnreqmaster set req_status = '6' where member_no ='" + memNo + "' and assist_docno like 'KT%'";
            //    DataTable upup = WebUtil.Query(upsql);
            //}
            //catch { }
        }

        private void SaveState()
        {
            string entry_id = state.SsUsername;
            decimal payout = 0;
            string payout_no = "";
            int yearnow = DateTime.Now.Year + 543;

            string moneyType = HdMoneyType.Value;//DwMainP.GetItemString(1, "moneytype_code");
            string member_no = HdMemberNo.Value;
            //DateTime stamentdate = DwMainP.GetItemDateTime(1, "entry_date");
            try
            {
                payout = Convert.ToDecimal(HdPayout.Value);//DwDetailP.GetItemDecimal(1, "assist_amt");
            }
            catch
            {

            }

            string sqlQu = "select max(payoutslip_no) from asnslippayout where payoutslip_no like '" + yearnow + "80" + "%'";
            DataTable slip_no = WebUtil.Query(sqlQu);
            if (slip_no.Rows.Count > 0)
            {
                payout_no = slip_no.Rows[0][0].ToString();
                if (payout_no != "")
                {
                    payout_no = payout_no.Substring(4, 6);
                }
                else { payout_no = "00000"; }
            }
            Decimal ass = Convert.ToDecimal(payout_no);
            ass++;
            string slippayout_no = yearnow.ToString().Substring(2, 2) + "80" + ass.ToString("00000");

            if (moneyType == "TRN")
            {
                string bankAcc = DwMainP.GetItemString(1, "asnslippayout_bank_accid");
                string sqlupdateasn = @"update asnreqmaster set moneytype_code = '" + moneyType + "' , to_system = 'DIV' where member_no = '" + member_no + "' and assisttype_code = '80'";
                ta.Exe(sqlupdateasn);

                string sqlsavestate = @"insert into asnslippayout (payoutslip_no,member_no,slip_date,operate_date,payout_amt,slip_status,assisttype_code,entry_id,entry_date,moneytype_code,post_fin,bank_accid)" +
                    "values('" + slippayout_no + "','" + member_no + "',to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy')," +
                 +payout + ",1,'" + "80" + "','" + entry_id + "',to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),'" + moneyType + "','8','"+bankAcc+"')";
                ta.Exe(sqlsavestate);
            }
            else if (moneyType == "CBT")
            {
                string bankCode = DwMainP.GetItemString(1, "asnslippayout_bank_code");
                string bankBranch = DwMainP.GetItemString(1, "asnslippayout_bankbranch_id");
                string bankAcc = DwMainP.GetItemString(1, "asnslippayout_bank_accid");

                string sqlupdateasn = @"update asnreqmaster set moneytype_code = '" + moneyType + "' where member_no = '" + member_no + "' and assisttype_code = '80'";
                ta.Exe(sqlupdateasn);

                string sqlsavestate = @"insert into asnslippayout (payoutslip_no,member_no,slip_date,operate_date,payout_amt,slip_status,assisttype_code,entry_id,entry_date,moneytype_code,bank_code,bankbranch_id,bank_accid,post_fin)" +
                    "values('" + slippayout_no + "','" + member_no + "',to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy')," +
                 +payout + ",1,'" + "80" + "','" + entry_id + "',to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),'" + moneyType + "','" + bankCode + "','" + bankBranch + "','" + bankAcc + "','8')";
                ta.Exe(sqlsavestate);
            }
            else
            {
                string sqlupdateasn = @"update asnreqmaster set moneytype_code = '" + moneyType + "' where member_no = '" + member_no + "' and assisttype_code = '80'";
                ta.Exe(sqlupdateasn);

                string sqlsavestate = @"insert into asnslippayout (payoutslip_no,member_no,slip_date,operate_date,payout_amt,slip_status,assisttype_code,entry_id,entry_date,moneytype_code,post_fin)" +
                    "values('" + slippayout_no + "','" + member_no + "',to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy')," +
                 +payout + ",1,'" + "80" + "','" + entry_id + "',to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),'" + moneyType + "','8')";
                ta.Exe(sqlsavestate);
            }
            LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลสำเร็จ");

        }

        private void SaveReceive()
        {
            if (HdRowReceive.Value == "เพิ่ม")
            {

                int rowww = DwReceive.RowCount + 1;
                string sqlcount = "select count(receive_no) from asnmembreceive where assist_docno = (select assist_docno from asnreqmaster  where member_no ='" + HdMemberNo.Value + "' and assist_docno like 'PS%') ";
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
                                    " assist_docno = (select assist_docno from asnreqmaster  where member_no ='" + HdMemberNo.Value + "' and assist_docno like 'PS%')) and" +
                                    " assist_docno = (select assist_docno from asnreqmaster  where member_no ='" + HdMemberNo.Value + "' and assist_docno like 'PS%')";

                    dt = ta.Query(sqlrow);
                    dt.Next();
                    rowcount = Convert.ToInt32(dt.GetString("receive_no"));
                    rowcount++;
                    assist_docnoo = dt.GetString("assist_docno");
                }
                else if (count == 0)
                {
                    rowcount = 1;
                    string sqlassistno = "select assist_docno from asnreqmaster where member_no ='" + HdMemberNo.Value + "' and assist_docno like 'PS%'";
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
                        "values ('" + assist_docnoo + "','" + rownext + "','" + name + "','" + per + "'," + pay + ")";
                    ta.Exe(sqlinsertrow);
                    //เพิ่ม
                    rowcount++;
                }
                DwUtil.UpdateDateWindow(DwReceive, "kt_pension.pbl", "asnmembreceive");
                HdRowReceive.Value = "";
            }
            else if (HdRowReceive.Value == "")
            {

                DwUtil.UpdateDateWindow(DwReceive, "kt_pension.pbl", "asnmembreceive");
            }
        }

        private void JspostBankAccid()
        {
            string BankAccidStr = DwMainP.GetItemString(1, "asnslippayout_bank_accid");
            string branchId = BankAccidStr.Substring(0, 3);
            while (branchId.Count() != 4)
            {
                branchId = "0" + branchId;
            }

            DwMainP.SetItemString(1, "asnslippayout_bankbranch_id", branchId);
            //int BankAccidInt = Convert.ToInt32(BankAccidStr);
        }

    }
}