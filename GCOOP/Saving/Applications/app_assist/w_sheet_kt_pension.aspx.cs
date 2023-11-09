using System;
using CoreSavingLibrary;
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
using DataLibrary;
using Sybase.DataWindow;
using System.Web.Services.Protocols;

namespace Saving.Applications.app_assist
{
    public partial class w_sheet_kt_pension : PageWebSheet, WebSheet
    {
        protected DwThDate tDwMain;
        protected DwThDate tDwDetail;

        protected String postRefresh;
        protected String postChangeHeight;
        protected String postChangeAmt;
        protected String postRetreiveDwMem;
        protected String postRetrieveDwMain;
        protected String postGetMemberDetail;
        protected String postRetrieveBankBranch;
        protected String postBPayclick;
        protected String postBDeadPayclick;

        private String sqlStr, envvalue;
        private String member_no, assist_docno, membgroup_code, member_dead_tdate;
        private String expense_bank, expense_branch, expense_accid, paytype_code, remark_cancel;
        private Decimal capital_year, salary_amt, assist_amt, req_status, seq_pay, rowcount;
        private DateTime pay_date, cancel_date = DateTime.Now, center_date, req_date, member_date;
        private Nullable<Decimal> cancel_id;


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
            postRetrieveBankBranch = WebUtil.JsPostBack(this, "postRetrieveBankBranch");
            postBPayclick = WebUtil.JsPostBack(this, "postBPayclick");
            postBDeadPayclick = WebUtil.JsPostBack(this, "postBDeadPayclick");

            tDwMain = new DwThDate(DwMain, this);
            tDwDetail = new DwThDate(DwDetail, this);

            tDwMain.Add("pay_date", "pay_tdate");
            tDwMain.Add("req_date", "req_tdate");
            tDwMain.Add("entry_date", "entry_tdate");
            tDwMain.Add("member_date", "member_tdate");
            tDwMain.Add("approve_date", "approve_tdate");
            tDwDetail.Add("member_dead_date", "member_dead_tdate");

        }

        public void WebSheetLoadBegin()
        {
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
                this.RestoreContextDw(DwMem);
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwDetail);
            }

            DwMain.SetItemString(1, "assisttype_code", "70");

            DwUtil.RetrieveDDDW(DwMain, "assisttype_code", "kt_pension.pbl", null);
            DwUtil.RetrieveDDDW(DwMain, "capital_year", "kt_pension.pbl", null);
            DwUtil.RetrieveDDDW(DwMain, "cancel_id", "kt_pension.pbl", null);
            DwUtil.RetrieveDDDW(DwMain, "req_status", "kt_pension.pbl", null);
            DwUtil.RetrieveDDDW(DwMain, "pay_status", "kt_pension.pbl", null);
            DwUtil.RetrieveDDDW(DwMain, "expense_bank", "kt_pension.pbl", null);
            DwUtil.RetrieveDDDW(DwMain, "paytype_code", "kt_pension.pbl", null);

            tDwMain.Eng2ThaiAllRow();
            tDwDetail.Eng2ThaiAllRow();
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
            else if (eventArg == "postBPayclick")
            {
                BPayclick();
            }
            else if (eventArg == "postBDeadPayclick")
            {
                BDeadPayclick();
            }
        }

        public void SaveWebSheet()
        {
//            Decimal member_age;
//            String member_receive, member_dead_case, remark;
//            DateTime member_dead_date, req_date;

//            GetItemDwMain();

//            assist_amt = DwDetail.GetItemDecimal(1, "assist_amt");
//            member_age = DwDetail.GetItemDecimal(1, "member_age");
//            member_receive = DwDetail.GetItemString(1, "member_receive");
//            member_dead_date = DwDetail.GetItemDateTime(1, "member_dead_date");
//            try
//            {
//                member_dead_case = DwDetail.GetItemString(1, "member_dead_case");
//            }
//            catch { member_dead_case = "ไม่ระบุ"; }
//            try
//            {
//                remark = DwDetail.GetItemString(1, "remark");
//            }
//            catch { remark = ""; }

//            try
//            {
//                assist_docno = DwMain.GetItemString(1, "assist_docno");
//            }
//            catch { assist_docno = ""; }

//            if (assist_docno == "" && req_status == 8)
//            {
//                assist_docno = GetLastDocNo(capital_year);

//                try
//                {
//                    DwMain.SetItemString(1, "member_no", member_no);
//                    DwMain.SetItemDecimal(1, "assist_amt", assist_amt);
//                    DwMain.SetItemString(1, "assist_docno", assist_docno);
//                    try
//                    {
//                        DwUtil.InsertDataWindow(DwMain, "kt_pension.pbl", "asnreqmaster");
//                    }
//                    catch { }

//                    seq_pay = GetSeqNo();

//                    DwDetail.SetItemDecimal(1, "seq_pay", seq_pay);
//                    DwDetail.SetItemString(1, "assist_docno", assist_docno);
//                    DwDetail.SetItemDecimal(1, "capital_year", capital_year);
//                    DwDetail.SetItemDateTime(1, "capital_date", DateTime.Now);
//                    try
//                    {
//                        DwUtil.InsertDataWindow(DwDetail, "kt_pension.pbl", "asnreqcapitaldet");
//                    }
//                    catch { }

//                    //                    sqlStr = @"INSERT INTO asnreqmaster(assist_docno  ,  capital_year  ,  member_no  ,  assisttype_code  ,
//                    //                                                assist_amt    ,  req_status ,  req_date  ,  pay_date  ,  paytype_code  ,  remark  ,
//                    //                                                membgroup_code  ,  expense_bank  ,  expense_branch  ,  expense_accid)
//                    //                                         VALUES('" + assist_docno + "','" + capital_year + "','" + member_no + @"','30',
//                    //                                                '" + assist_amt + "','" + req_status + "',to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + @"', 'dd/mm/yyyy'),to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + @"', 'dd/mm/yyyy'),
//                    //                                                '" + paytype_code + @"',
//                    //                                                '" + remark + "','" + membgroup_code + "','" + expense_bank + "','" + expense_branch + "','" + expense_accid + "')";
//                    //                    ta.Exe(sqlStr);

//                    //                    sqlStr = @"INSERT INTO asnreqcapitaldet(capital_year  ,  assist_docno  ,  capital_date  ,  damagetype_code  ,
//                    //                                                     subdamagetype_code  ,  member_dead_date  ,  member_age  ,  member_dead_case,  remark  ,  assist_amt  ,  member_receive)
//                    //                                   VALUES           ('" + capital_year + "','" + assist_docno + @"',
//                    //                                                     to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + @"', 'dd/mm/yyyy'),'03','03',
//                    //                                                     to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + @"', 'dd/mm/yyyy'),'" + member_age + "','" + member_dead_case + @"',
//                    //                                                     '" + remark + "','" + assist_amt + "','" + member_receive + "')";
//                    //                    ta.Exe(sqlStr);

//                    sqlStr = @"INSERT INTO asnmemsalary(capital_year  ,  member_no  ,  assist_docno  ,
//                                                    salary_amt    ,  entry_date  ,  membgroup_code)
//                                         VALUES('" + capital_year + "','" + member_no + "','" + assist_docno + @"',
//                                                '" + salary_amt + "',to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "', 'dd/mm/yyyy'),'" + membgroup_code + @"')";
//                    ta.Exe(sqlStr);

//                    sqlStr = @"UPDATE  asndoccontrol
//                               SET     last_docno   = '" + WebUtil.Right(assist_docno, 6) + @"'
//                               WHERE   doc_prefix   = 'AM' and
//                                       doc_year = '" + capital_year + "'";
//                    ta.Exe(sqlStr);

//                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกเรียบร้อย");
//                    NewClear();
//                }
//                catch (Exception ex)
//                {
//                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
//                }
//            }
//            else
//            {
//                try
//                {
                    //                    sqlStr = @"UPDATE asnreqmaster
                    //                               Set    assist_amt    = '" + assist_amt + @"',
                    //                                      req_status    = '" + req_status + @"',
                    //                                      paytype_code  = '" + paytype_code + @"',
                    //                                      remark        = '" + remark + @"',
                    //                                      expense_bank  = '" + expense_bank + @"',
                    //                                      expense_branch= '" + expense_branch + @"',
                    //                                      expense_accid = '" + expense_accid + @"'
                    //                               WHERE  assist_docno  = '" + assist_docno + "'";
                    //                    ta.Exe(sqlStr);

                    //                    if (req_status == -8 || req_status == -9)
                    //                    {
                    //                        sqlStr = @"UPDATE asnreqmaster
                    //                                   SET    cancel_date = to_date('" + cancel_date.ToString("dd/MM/yyyy", WebUtil.EN) + @"', 'dd/mm/yyyy'),
                    //                                          cancel_id    = '" + cancel_id + "'";
                    //                        ta.Exe(sqlStr);
                    //                    }

//                    sqlStr = @"UPDATE  asnmemsalary
//                               SET     salary_amt    = '" + salary_amt + @"'
//                               WHERE   assist_docno  = '" + assist_docno + "'";
//                    ta.Exe(sqlStr);
            if (HdTotalpay.Value != "1")
            {
                double assist_amt = DwDetail.GetItemDouble(1, "assist_amt");
                try
                {
                    string sqlStr2 = @"UPDATE  asnreqmaster
                               SET     sumpay    = '" + HdTotalpay.Value + @"','"+ assist_amt +@"'  
                               WHERE   member_no  = '" + DwMem.GetItemString(1, "member_no") + "' and assist_docno like 'PS%'";
                    ta.Exe(sqlStr2);
                    HdSumpay.Value = "";
                    Hdck.Value = "";
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                }
                catch
                {
                }
            }
            else { LtServerMessage.Text = WebUtil.ErrorMessage("รายการนี้ได้ทำการบันทึกไปแล้ว"); }

        }

        public void WebSheetLoadEnd()
        {
            //DwMain.SetItemDateTime(1, "pay_date", state.SsWorkDate);
            //DwMain.SetItemDateTime(1, "req_date", state.SsWorkDate);
            //DwMain.SetItemDateTime(1, "entry_date", state.SsWorkDate);
            //DwMain.SetItemDateTime(1, "approve_date", state.SsWorkDate);
            tDwMain.Eng2ThaiAllRow();
            tDwDetail.Eng2ThaiAllRow();

            DwMem.SaveDataCache();
            DwMain.SaveDataCache();
            DwDetail.SaveDataCache();
            dt.Clear();
            ta.Close();
        }

        private void Refresh()
        {

        }

        private void ChangeAmt()////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        {
            //String member_dead_tdate;
            DateTime member_dead_date;
            string temp = DwMem.GetItemString(1, "member_no");
            member_date = DwMain.GetItemDateTime(1, "member_date");

            //   member_dead_tdate = DwDetail.GetItemString(1, "member_dead_tdate");

            member_dead_date = DateTime.ParseExact(hdate1.Value, "ddMMyyyy", WebUtil.TH);
            DwDetail.SetItemDateTime(1, "member_dead_date", member_dead_date);

            //    DwDetail.SetItemString(1, "member_dead_tdate", member_dead_tdate);//testdate ลองเพิ่ม

            tp = member_dead_date - member_date;
            Double mem_year = (tp.TotalDays / 365);
            if (mem_year < 1)
            {
                LtServerMessage.Text = WebUtil.WarningMessage("สมาชิกร้ายนี้ยังเป็นสมาชิกไม่ถึง 1 ปี จนถึงวันเสียชีวิต");
                //return;
            }

            //หาว่าเกิน 120 วันหรือไม่
            req_date = DwMain.GetItemDateTime(1, "req_date");
            tp = req_date - member_dead_date;
            Double a = (tp.TotalDays);
            if (a < 120)
            {
                LtServerMessage.Text = WebUtil.WarningMessage("สมาชิกรายนี้ยื่นคำขอเกินกว่า 120 วัน");
                //return;
            }

            sqlStr = @"select envvalue
                           from asnsenvironmentvar
                           WHERE envcode = 'memdead_open_date'";
            dt = ta.Query(sqlStr);
            dt.Next();
            envvalue = dt.GetString("envvalue");
            center_date = Convert.ToDateTime(envvalue);

            tDwDetail.Eng2ThaiAllRow();
            string sumpay_amt = "0";
            try
            {
                sqlStr = @"select sumpay 
                           from asnreqmaster 
                           where member_no = '" + temp + "' and assist_docno like 'PS%'";
                dt = ta.Query(sqlStr);
                if (dt.Next())
                {
                    sumpay_amt = dt.GetString("sumpay");
                }

                if(sumpay_amt == "")
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
                string entry_date = DwDetail.GetItemDateTime(1, "member_dead_date").ToString("yyyy");
                string memb_date = DwMain.GetItemDateTime(1, "member_date").ToString("yyyy");
                int memb_age = (Convert.ToInt32(entry_date) + 543) - (Convert.ToInt32(memb_date) + 543);

                sqlStr = @"select sharestk_amt 
                           from shsharemaster 
                           where member_no = '" + temp + "'";
                dt = ta.Query(sqlStr);
                dt.Next();
                decimal deadpay_amt = dt.GetDecimal("sharestk_amt");

                double shareAmt = 0;
                double setDeadpay_amt = 0;
                shareAmt = Convert.ToDouble(deadpay_amt);
                if (memb_age >= 1 && memb_age < 5)
                {
                    setDeadpay_amt = 0.2 * shareAmt;
                    if (setDeadpay_amt > 10000)
                        setDeadpay_amt = 10000;
                }
                else if (memb_age >= 5 && memb_age < 10)
                {
                    setDeadpay_amt = 0.4 * shareAmt;
                    if (setDeadpay_amt > 30000)
                        setDeadpay_amt = 30000;
                }
                else if (memb_age >= 10 && memb_age < 15)
                {
                    setDeadpay_amt = 0.6 * shareAmt;
                    if (setDeadpay_amt > 50000)
                        setDeadpay_amt = 50000;
                }
                else if (memb_age >= 15 && memb_age < 20)
                {
                    setDeadpay_amt = 0.8 * shareAmt;
                    if (setDeadpay_amt > 70000)
                        setDeadpay_amt = 70000;
                }
                else if (memb_age >= 20)
                {
                    setDeadpay_amt = 1 * shareAmt;
                    if (setDeadpay_amt > 100000)
                        setDeadpay_amt = 100000;
                }
                double perpay = setDeadpay_amt * 0.05;
                setDeadpay_amt = setDeadpay_amt - Convert.ToDouble(sumpay_amt);
                if (setDeadpay_amt <= 0)
                    setDeadpay_amt = 0;
                DwDetail.SetItemDecimal(1, "assist_amt", Convert.ToDecimal(setDeadpay_amt));
                DwDetail.SetItemDecimal(1, "sperpay", Convert.ToDecimal(perpay));
                //fivePercent = setDeadpay_amt * 0.05;
                //DwDetail.SetItemString(1, "pay5per_amt", fivePercent.ToString());
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

            if (req_status == -9 || req_status == -8)
            {
                DwMain.Modify("datawindow.detail.Height=1004");
            }
            else
            {
                DwMain.Modify("datawindow.detail.Height=732");
            }
        }

        private void RetreiveDwMem()//**************************************************************************************************************
        {

            String member_no;
            Decimal deadpay_amt = 0;
            member_no = HfMemberNo.Value;
            try
            {

               // เพิ่มค่าในช่องหุ้น
                sqlStr = @"select sharestk_amt 
                           from shsharemaster 
                           where member_no = '" + member_no + "'";
                dt = ta.Query(sqlStr);
                if (dt.Next())
                {
                    deadpay_amt = dt.GetDecimal("sharestk_amt");
                    DwMain.SetItemDecimal(1, "sharestk_amt", deadpay_amt);
                }
                else { DwMain.SetItemDecimal (1, "sharestk_amt", 0); }
            }
            catch
            {

            }

            object[] args = new object[1];
            args[0] = member_no;

            DwMem.Reset();
            DwUtil.RetrieveDataWindow(DwMem, "kt_pension.pbl", null, args);
            if (DwMem.RowCount < 1)
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

            DwMem.Reset();
            DwUtil.RetrieveDataWindow(DwMem, "kt_pension.pbl", null, args1);

            object[] args2 = new object[3];
            args2[0] = assist_docno;
            args2[1] = capital_year;
            args2[2] = member_no;

            DwMain.Reset();
            DwUtil.RetrieveDataWindow(DwMain, "kt_pension.pbl", tDwMain, args2);

            object[] args3 = new object[2];
            args3[0] = assist_docno;
            args3[1] = capital_year;

            DwDetail.Reset();
            DwUtil.RetrieveDataWindow(DwDetail, "kt_pension.pbl", null, args3);

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
            Ds.LibraryList = "C:/GCOOP_ALL/CAT/GCOOP/Saving/DataWindow/app_assist/as_seminar.pbl";
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

            DwMem.SetItemString(1, "member_no", member_no);
            DwMem.SetItemString(1, "prename_desc", prename_desc);
            DwMem.SetItemString(1, "memb_name", memb_name);
            DwMem.SetItemString(1, "memb_surname", memb_surname);
            DwMem.SetItemString(1, "membgroup_desc", membgroup_desc);
            DwMem.SetItemString(1, "membgroup_code", membgroup_code);
            DwMem.SetItemString(1, "membtype_code", membtype_code);
            DwMem.SetItemString(1, "membtype_desc", membtype_desc);
            DwMain.SetItemDateTime(1, "member_date", member_date);

            tDwMain.Eng2ThaiAllRow();
        }

        private void RetrieveBankBranch()
        {
            String bank;

            try
            {
                bank = DwMain.GetItemString(1, "expense_bank");
            }
            catch { bank = ""; }

            DataWindowChild DcBankBranch = DwMain.GetChild("expense_branch");
            DcBankBranch.SetTransaction(sqlca);
            DcBankBranch.Retrieve();
            DcBankBranch.SetFilter("bank_code = '" + bank + "'");
            DcBankBranch.Filter();
        }

        private void NewClear()
        {
            tDwMain.Eng2ThaiAllRow();
            tDwDetail.Eng2ThaiAllRow();



            DwMem.Reset();
            DwMain.Reset();
            DwDetail.Reset();

            DwMem.InsertRow(0);
            DwMain.InsertRow(0);
            DwDetail.InsertRow(0);


            DwMain.SetItemDecimal(1, "req_status", 8);
            DwMain.SetItemDateTime(1, "pay_date", DateTime.Now);
            DwMain.SetItemDateTime(1, "req_date", DateTime.Now);
            DwMain.SetItemDateTime(1, "entry_date", DateTime.Now);
            //DwDetail.SetItemDateTime(1, "member_dead_date", state.SsWorkDate);
            //DwDetail.SetItemDateTime(1, "member_dead_tdate", state.SsWorkDate); 
            //DwMain.SetItemDateTime(1, "approve_date", state.SsWorkDate);
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
            member_no = DwMem.GetItemString(1, "member_no");
            membgroup_code = DwMem.GetItemString(1, "membgroup_code");
            req_status = DwMain.GetItemDecimal(1, "req_status");
            capital_year = DwMain.GetItemDecimal(1, "capital_year");
            try
            {
                salary_amt = DwMain.GetItemDecimal(1, "salary_amt");
            }
            catch { salary_amt = 0; }

            try
            {
                paytype_code = DwMain.GetItemString(1, "paytype_code");
            }
            catch { paytype_code = ""; }
            try
            {
                pay_date = DwMain.GetItemDateTime(1, "pay_date");
            }
            catch { }
            try
            {
                expense_bank = DwMain.GetItemString(1, "expense_bank");
            }
            catch { expense_bank = ""; }
            try
            {
                expense_branch = DwMain.GetItemString(1, "expense_branch");
            }
            catch { expense_branch = ""; }
            try
            {
                expense_accid = DwMain.GetItemString(1, "expense_accid");
            }
            catch { expense_accid = ""; }

            try
            {
                cancel_date = DwMain.GetItemDateTime(1, "cancel_date");
            }
            catch { }
            try
            {
                cancel_id = DwMain.GetItemDecimal(1, "cancel_id");
            }
            catch { cancel_id = null; }
            try
            {
                remark_cancel = DwMain.GetItemString(1, "remark");
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

        private void BDeadPayclick()
        {
            if (HdSumpay.Value != "")
            {
                HdSumpay.Value = HdSumpay.Value + (DwDetail.GetItemDecimal(1, "assist_amt") * 2).ToString();
                HdTotalpay.Value = HdSumpay.Value;
                DwDetail.SetItemDecimal(1, "assist_amt", 0);
            }
            else { LtServerMessage.Text = WebUtil.ErrorMessage("รายการนี้ได้ทำรายการไปแล้วจ่าย"); }
            
        }

        private void BPayclick()
        {
             
            if (HdSumpay.Value != "" && HdTotalpay.Value != "" && Hdck.Value != "1")
            {
                Hdck.Value = "1";
                double totalpay = 0;
                decimal perpayTemp = DwDetail.GetItemDecimal(1, "sperpay"); // ค่า 5%
                double sumpay_temp = Convert.ToDouble(HdSumpay.Value); // ค่า pay ที่เคยจ่ายไปแล้ว

                totalpay = Convert.ToDouble(perpayTemp) + sumpay_temp;
                HdTotalpay.Value = totalpay.ToString();
                double assist_value = DwDetail.GetItemDouble(1, "assist_amt");
                double calculate = assist_value - Convert.ToDouble(perpayTemp);
                try
                {
                    if (calculate <= 0)
                        calculate = 0.00;
                    DwDetail.SetItemDecimal(1, "assist_amt", Convert.ToDecimal(calculate));
                }
                catch
                {
                }

            }
            else { LtServerMessage.Text = WebUtil.ErrorMessage("รายการนี้ได้ทำการจ่ายแล้ว"); }
        }
    }
}