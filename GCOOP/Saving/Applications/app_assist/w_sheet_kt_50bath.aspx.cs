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
    public partial class w_sheet_kt_50bath : PageWebSheet, WebSheet
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

            tDwMain = new DwThDate(DwMain, this);
            tDwDetail = new DwThDate(DwDetail, this);
            tDwMain.Add("pay_date", "pay_tdate");
            tDwMain.Add("req_date", "req_tdate");
            tDwMain.Add("entry_date", "entry_tdate");
            tDwMain.Add("member_date", "member_tdate");
            tDwMain.Add("approve_date", "approve_tdate");
            tDwDetail.Add("member_dead_date", "member_dead_tdate");

            tDwMainP = new DwThDate(DwMainP, this);
            tDwDetailP = new DwThDate(DwDetailP, this);
            tDwMainP.Add("pay_date", "pay_tdate");
            tDwMainP.Add("req_date", "req_tdate");
            tDwMainP.Add("entry_date", "entry_tdate");
            tDwMainP.Add("member_date", "member_tdate");
            tDwMainP.Add("approve_date", "approve_tdate");
            tDwDetailP.Add("member_dead_date", "member_dead_tdate");

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
                    this.RestoreContextDw(DwSelect);
                    this.RestoreContextDw(DwMemP);
                    this.RestoreContextDw(DwMainP);
                    this.RestoreContextDw(DwDetailP);
                    this.RestoreContextDw(DwMem65);
                    this.RestoreContextDw(DwMain65);
                    this.RestoreContextDw(DwDetail65);

                    DwMain.SetItemDecimal(1, "req_status", 8);
                    DwMain.SetItemDateTime(1, "pay_date", DateTime.Now);
                    DwMain.SetItemDateTime(1, "req_date", DateTime.Now);
                    DwMain.SetItemDateTime(1, "entry_date", DateTime.Now);
                    DwDetail.SetItemDateTime(1, "member_dead_date", state.SsWorkDate);
                    DwMain.SetItemDateTime(1, "approve_date", state.SsWorkDate);

                    DwMainP.SetItemDecimal(1, "req_status", 8);
                    DwMainP.SetItemDateTime(1, "pay_date", DateTime.Now);
                    DwMainP.SetItemDateTime(1, "req_date", DateTime.Now);
                    DwMainP.SetItemDateTime(1, "entry_date", DateTime.Now);
                    DwMainP.SetItemDateTime(1, "approve_date", state.SsWorkDate);

                    DwMain65.SetItemDecimal(1, "req_status", 8);
                    DwMain65.SetItemDateTime(1, "pay_date", DateTime.Now);
                    DwMain65.SetItemDateTime(1, "req_date", DateTime.Now);
                    DwMain65.SetItemDateTime(1, "entry_date", DateTime.Now);
                    DwMain65.SetItemDateTime(1, "approve_date", state.SsWorkDate);
            }


            tDwMain.Eng2ThaiAllRow();
            tDwDetail.Eng2ThaiAllRow();

            DwUtil.RetrieveDDDW(DwMain, "assisttype_code", "kt_50bath.pbl", null);
            DwUtil.RetrieveDDDW(DwMain, "capital_year", "kt_50bath.pbl", null);
            DwUtil.RetrieveDDDW(DwMain, "cancel_id", "kt_50bath.pbl", null);
            DwUtil.RetrieveDDDW(DwMain, "req_status", "kt_50bath.pbl", null);
            DwUtil.RetrieveDDDW(DwMain, "pay_status", "kt_50bath.pbl", null);
            DwUtil.RetrieveDDDW(DwMain, "expense_bank", "kt_50bath.pbl", null);
            DwUtil.RetrieveDDDW(DwMain, "paytype_code", "kt_50bath.pbl", null);
            DwUtil.RetrieveDDDW(DwMain, "moneytype_code", "kt_50bath.pbl", null);
            DwUtil.RetrieveDDDW(DwMain, "asnslippayout_bank_code", "kt_50bath.pbl", null);

            tDwMainP.Eng2ThaiAllRow();
            tDwDetailP.Eng2ThaiAllRow();
            DwUtil.RetrieveDDDW(DwMainP, "assisttype_code", "kt_pension.pbl", null);
            DwUtil.RetrieveDDDW(DwMainP, "capital_year", "kt_pension.pbl", null);
            DwUtil.RetrieveDDDW(DwMainP, "cancel_id", "kt_pension.pbl", null);
            DwUtil.RetrieveDDDW(DwMainP, "req_status", "kt_pension.pbl", null);
            DwUtil.RetrieveDDDW(DwMainP, "pay_status", "kt_pension.pbl", null);
            DwUtil.RetrieveDDDW(DwMainP, "expense_bank", "kt_pension.pbl", null);
            DwUtil.RetrieveDDDW(DwMainP, "paytype_code", "kt_pension.pbl", null);
            DwUtil.RetrieveDDDW(DwMainP, "moneytype_code", "kt_pension.pbl", null);
            DwUtil.RetrieveDDDW(DwMainP, "asnslippayout_bank_code", "kt_pension.pbl", null);

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
            
        }

        public void SaveWebSheet()
        {
            if (HdTy.Value == "80")
            {

                if (HdTotalpay.Value != "1")
                {
                    if (HdDateStatus.Value != "1" && HdDateStatus.Value != "2")
                    {
                        SavePS();
                        string member_no = HfMemberNo.Value;
                        string sqlassist = @"select assist_docno from asnreqmaster where member_no = '" + member_no + "' and assist_docno like 'PS%'";
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
                            remark = DwDetailP.GetItemString(1, "remark");
                        }
                        catch { remark = ""; }
                        try
                        {
                            Dcase = DwDetailP.GetItemString(1, "member_dead_case");
                        }
                        catch { Dcase = ""; }
                        try
                        {
                            Age = DwDetailP.GetItemDecimal(1, "member_age");
                        }
                        catch { Age = 0; }
                        try
                        {
                            member_receive = DwDetailP.GetItemString(1, "member_receive");
                        }
                        catch { member_receive = ""; }
                        string sqlinsert = @"insert into asnreqcapitaldet (capital_year,assist_docno,member_dead_date,member_dead_case,member_age,member_receive,pension_date,remark) values "
                            + "(" + capitalyyyy + ",'" + assistdocno + "',to_date('" + dead_date.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),'" + Dcase + "', " + Age + ",'" + member_receive + "',to_date('" + dead_date.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),'" + remark + "')";
                        ta.Exe(sqlinsert);
                        
                        HdDateStatus.Value = "0";
                    }
                    else if (HdDateStatus.Value == "2")
                    {
                        string member_no = HfMemberNo.Value;
                        string sqlassist = @"select assist_docno from asnreqmaster where member_no = '" + member_no + "' and assist_docno like 'PS%'";
                        dt = ta.Query(sqlassist);
                        dt.Next();
                        string assistdocno = dt.GetString("assist_docno").Trim();


                        DateTime dead_date = DateTime.ParseExact(hdate2.Value, "ddMMyyyy", WebUtil.TH);
                        string capitalstring = dead_date.ToString("yyyy");
                        Int32 capitalyyyy = Convert.ToInt32(capitalstring) + 543;
                        string sqlUpdate = @"update asnreqcapitaldet set pension_date = to_date('" + dead_date.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy') where assist_docno = '" + assistdocno + "'";
               
                        ta.Exe(sqlUpdate);

                    }
                    
                    double assist_amt = DwDetailP.GetItemDouble(1, "assist_amt");
                    try
                    {
                        string sqlStr2 = @"UPDATE  asnreqmaster
                               SET     sumpay    = '" + HdTotalpay.Value + @"',assist_amt = '" + assist_amt + @"'  
                               WHERE   member_no  = '" + DwMemP.GetItemString(1, "member_no") + "' and assist_docno like 'PS%'";
                        ta.Exe(sqlStr2);
                        HdSumpay.Value = "";
                        Hdck.Value = "";
                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                    }
                    catch
                    {
                    }
                    SaveState();
                 
                }
                else { LtServerMessage.Text = WebUtil.ErrorMessage("รายการนี้ได้ทำการบันทึกไปแล้ว"); }

            }
            else if (HdTy.Value == "90")
            {

                if (HdTotalpay.Value != "1")
                {
                    if (HdDateStatus.Value != "1")
                    {
                        SaveSF();
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
                            member_receive = DwDetail65.GetItemString(1, "member_receive");
                        }
                        catch { member_receive = ""; }
                        string sqlinsert = @"insert into asnreqcapitaldet (capital_year,assist_docno,member_dead_date,member_dead_case,member_age,member_receive,pension_date,remark) values "
                            + "(" + capitalyyyy + ",'" + assistdocno + "',to_date('" + dead_date.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),'" + Dcase + "', " + Age + ",'" + member_receive + "',to_date('" + dead_date.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),'" + remark + "')";
                        ta.Exe(sqlinsert);
                    }
                    //-----------------------------------------
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
                    SaveState();
                }
                else { LtServerMessage.Text = WebUtil.ErrorMessage("รายการนี้ได้ทำการบันทึกไปแล้ว"); }

            }//end if

            else
            {
                Decimal member_age;
                String member_receive, member_dead_case, remark;
                DateTime member_dead_date, req_date;

                GetItemDwMain();

                assist_amt = DwDetail.GetItemDecimal(1, "assist_amt");
                member_age = DwDetail.GetItemDecimal(1, "member_age");
                member_receive = DwDetail.GetItemString(1, "member_receive");
                member_dead_date = DwDetail.GetItemDateTime(1, "member_dead_date");
                try
                {
                    member_dead_case = DwDetail.GetItemString(1, "member_dead_case");
                }
                catch { member_dead_case = "ไม่ระบุ"; }
                try
                {
                    remark = DwDetail.GetItemString(1, "remark");
                }
                catch { remark = ""; }

                try
                {
                    assist_docno = DwMain.GetItemString(1, "assist_docno");
                }
                catch { assist_docno = ""; }

                if (assist_docno == "" && req_status == 8)
                {
                    assist_docno = GetLastDocNo(capital_year);

                    try
                    {
                        DwMain.SetItemString(1, "member_no", member_no);
                        DwMain.SetItemDecimal(1, "assist_amt", assist_amt);
                        DwMain.SetItemString(1, "assist_docno", assist_docno);
                        try
                        {
                            DwUtil.InsertDataWindow(DwMain, "kt_50bath.pbl", "asnreqmaster");
                        }
                        catch { }

                        seq_pay = GetSeqNo();

                        DwDetail.SetItemDecimal(1, "seq_pay", seq_pay);
                        DwDetail.SetItemString(1, "assist_docno", assist_docno);
                        DwDetail.SetItemDecimal(1, "capital_year", capital_year);
                        DwDetail.SetItemDateTime(1, "capital_date", DateTime.Now);
                        try
                        {
                            DwUtil.InsertDataWindow(DwDetail, "kt_50bath.pbl", "asnreqcapitaldet");
                        }
                        catch { }

                        sqlStr = @"INSERT INTO asnmemsalary(capital_year  ,  member_no  ,  assist_docno  ,
                                                    salary_amt    ,  entry_date  ,  membgroup_code)
                                         VALUES('" + capital_year + "','" + member_no + "','" + assist_docno + @"',
                                                '" + salary_amt + "',to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "', 'dd/mm/yyyy'),'" + membgroup_code + @"')";
                        ta.Exe(sqlStr);

                        sqlStr = @"UPDATE  asndoccontrol
                               SET     last_docno   = '" + WebUtil.Right(assist_docno, 6) + @"'
                               WHERE   doc_prefix   = 'AM' and
                                       doc_year = '" + capital_year + "'";
                        ta.Exe(sqlStr);

                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกเรียบร้อย");
                        NewClear();
                    }
                    catch (Exception ex)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                    }
                }
                else
                {
                    try
                    {

                        sqlStr = @"UPDATE  asnmemsalary
                               SET     salary_amt    = '" + salary_amt + @"'
                               WHERE   assist_docno  = '" + assist_docno + "'";
                        ta.Exe(sqlStr);

                        try
                        {
                            DwUtil.UpdateDataWindow(DwMain, "kt_50bath.pbl", "asnreqmaster");
                        }
                        catch { }

                        try
                        {
                            DwUtil.UpdateDataWindow(DwDetail, "kt_50bath.pbl", "asnreqcapitaldet");
                        }
                        catch { }

                        LtServerMessage.Text = WebUtil.CompleteMessage("แก้ไขเรียบร้อย");
                    }
                    catch (Exception ex)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                    }
                }
            }
        }

        public void WebSheetLoadEnd()
        {


            try { DwUtil.RetrieveDDDW(DwSelect, "assisttype_code", "kt_50bath.pbl", null); }
            catch { }


            tDwMain.Eng2ThaiAllRow();
            tDwDetail.Eng2ThaiAllRow();
            tDwMainP.Eng2ThaiAllRow();
            tDwDetailP.Eng2ThaiAllRow();
            tDwMain65.Eng2ThaiAllRow();
            tDwDetail65.Eng2ThaiAllRow();

            DwMem.SaveDataCache();
            DwMain.SaveDataCache();
            DwDetail.SaveDataCache();
            DwSelect.SaveDataCache();

            DwMemP.SaveDataCache();
            DwMainP.SaveDataCache();
            DwDetailP.SaveDataCache();

            DwMem65.SaveDataCache();
            DwMain65.SaveDataCache();
            DwDetail65.SaveDataCache();

            dt.Clear();
            ta.Close();
        }

        private void Refresh()
        {

        }

        private void ChangeAmt()
        {
            if (HdTy.Value == "70")
            {
                //String member_dead_tdate;
                DateTime member_dead_date;

                member_date = DwMain.GetItemDateTime(1, "member_date");

                //   member_dead_tdate = DwDetail.GetItemString(1, "member_dead_tdate");

                member_dead_date = DateTime.ParseExact(hdate1.Value, "ddMMyyyy", WebUtil.TH);
                DwDetail.SetItemDateTime(1, "member_dead_date", member_dead_date);

                //    DwDetail.SetItemString(1, "member_dead_tdate", member_dead_tdate);//testdate ลองเพิ่ม

                tp = member_dead_date - member_date;
                Double mem_year = (tp.TotalDays / 365);
                if (mem_year < 1)
                {
                    //LtServerMessage.Text = WebUtil.WarningMessage("สมาชิกร้ายนี้ยังเป็นสมาชิกไม่ถึง 1 ปี จนถึงวันเสียชีวิต");
                    //return;
                }

                //หาว่าเกิน 120 วันหรือไม่
                req_date = DwMain.GetItemDateTime(1, "req_date");
                tp = req_date - member_dead_date;
                Double a = (tp.TotalDays);
                if (a < 120)
                {
                    //LtServerMessage.Text = WebUtil.WarningMessage("สมาชิกรายนี้ยื่นคำขอเกินกว่า 120 วัน");
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

                string entry_date = DwMain.GetItemDateTime(1, "entry_date").ToString("yyyy");
                string memb_date = DwMain.GetItemDateTime(1, "member_date").ToString("yyyy");
                int memb_age = (Convert.ToInt32(entry_date) + 543) - (Convert.ToInt32(memb_date) + 543);
                sqlStr = @"select deadpay_amt
                           from asnucfpayktdead
                           WHERE minmemb_year <= " + memb_age + " AND maxmemb_year > " + memb_age;
                dt = ta.Query(sqlStr);
                dt.Next();
                //envvalue = dt.GetString("deadpay_amt");
                decimal deadpay_amt = dt.GetDecimal("deadpay_amt");
                DwDetail.SetItemDecimal(1, "assist_amt", deadpay_amt);
            }
            else if (HdTy.Value == "80")
            {
                //String member_dead_tdate;
                DateTime member_dead_date;
                string temp = DwMemP.GetItemString(1, "member_no");
                member_date = DwMainP.GetItemDateTime(1, "member_date");

                //   member_dead_tdate = DwDetail.GetItemString(1, "member_dead_tdate");34

                    member_dead_date = DateTime.ParseExact(hdate2.Value, "ddMMyyyy", WebUtil.TH);
                    DwDetailP.SetItemDateTime(1, "member_dead_date", member_dead_date);
                
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

                    if (sumpay_amt == "")
                    {
                        sumpay_amt = "0";
                    }
                    HdSumpay.Value = sumpay_amt;
                    //HdTotalpay.Value = "1";
                }
                catch
                {
                }

                try
                {
                    string  entry_date = DwDetailP.GetItemDateTime(1, "member_dead_date").ToString("yyyy");
                    string memb_date = DwMainP.GetItemDateTime(1, "member_date").ToString("yyyy");
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
                    if (perpay % 10 != 0)
                    {
                        perpay = perpay + (10 - (perpay % 10));
                    }
                    setDeadpay_amt = setDeadpay_amt - Convert.ToDouble(sumpay_amt);

                    if (setDeadpay_amt <= 0)
                        setDeadpay_amt = 0;
                    DwDetailP.SetItemDecimal(1, "assist_amt", Convert.ToDecimal(setDeadpay_amt));
                    DwDetailP.SetItemDecimal(1, "sperpay", Convert.ToDecimal(perpay));
                    if (Convert.ToDouble(HdSumpay.Value) >= (perpay * 10))
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("จ่ายเงินครบ 10 งวดแล้ว");
                    }
                }
                catch (Exception er)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(er);
                }
            }
            else if (HdTy.Value == "90")
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
                    //HdTotalpay.Value = "1";
                }
                catch
                {
                }

                try
                {

                    string entry_date = DwDetail65.GetItemDateTime(1, "member_dead_date").ToString("yyyy");

                    string memb_date = DwMain65.GetItemDateTime(1, "member_date").ToString("yyyy");
                    //int memb_age = (Convert.ToInt32(entry_date) + 543) - (Convert.ToInt32(memb_date) + 543);
                    string memNo = DwMem65.GetItemString(1, "member_no");
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


                    double shareAmt = 0;
                    double setDeadpay_amt = 0;
                    shareAmt = Convert.ToDouble(sumshare);
                    if (memb_age >= 2 && memb_age < 5)
                    {
                        setDeadpay_amt = 0.1 * shareAmt;
                    }
                    else if (memb_age >= 5 && memb_age < 10)
                    {
                        setDeadpay_amt = 0.2 * shareAmt;

                    }
                    else if (memb_age >= 10 && memb_age < 15)
                    {
                        setDeadpay_amt = 0.3 * shareAmt;
                    }
                    else if (memb_age >= 15 && memb_age < 20)
                    {
                        setDeadpay_amt = 0.35 * shareAmt;

                    }
                    else if (memb_age >= 20)
                    {
                        setDeadpay_amt = 0.4 * shareAmt;
                    }
                    if (setDeadpay_amt > 500000)
                    {
                        setDeadpay_amt = 500000;
                    }
                    double perpay = setDeadpay_amt * 0.01;
                    if (perpay % 10 != 0)
                    {
                        perpay = perpay + (10 - (perpay % 10));
                    }
                    if (perpay > 10000)
                    {
                        perpay = 10000;
                    }
                    setDeadpay_amt = setDeadpay_amt - Convert.ToDouble(sumpay_amt);
                    if (setDeadpay_amt <= 0)
                    {
                        setDeadpay_amt = 0;
                    }
                        DwDetail65.SetItemDecimal(1, "assist_amt", Convert.ToDecimal(setDeadpay_amt));
                        DwDetail65.SetItemDecimal(1, "sperpay", Convert.ToDecimal(perpay));

                }
                catch (Exception er)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(er);
                }
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

        private void RetreiveDwMem()
        {
            if (HdTy.Value == "70")
            {
                String member_no;

                member_no = HfMemberNo.Value;

                object[] args = new object[1];
                args[0] = int.Parse(member_no).ToString("00000000");

                DwMem.Reset();
                DwUtil.RetrieveDataWindow(DwMem, "kt_50bath.pbl", null, args);
                if (DwMem.RowCount < 1)
                {
                    LtAlert.Text = "<script>Alert()</script>";
                    return;
                }
               
                GetMemberDetail();
            }
            else if (HdTy.Value == "80")
            {
                DwDetailP.Reset();
                DwDetailP.InsertRow(0);

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
                        DwMainP.SetItemDecimal(1, "sharestk_amt", deadpay_amt);
                    }
                    else { DwMainP.SetItemDecimal(1, "sharestk_amt", 0); }
                }
                catch
                {

                }
                object[] args = new object[1];
                args[0] = member_no;

                //DwMemP.Reset();
                DwUtil.RetrieveDataWindow(DwMemP, "kt_pension.pbl", null, args);
                if (DwMemP.RowCount < 1)
                {
                    LtAlert.Text = "<script>Alert()</script>";
                    return;
                }
                GetMemberDetail();
            }
            else if (HdTy.Value == "90")
            {
                DwDetail65.Reset();
                DwDetail65.InsertRow(0);

                String member_no;
                decimal sumshare = 0;
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
        }

        private void RetrieveDwMain()
        {
            if (HdTy.Value == "70")
            {
                String assist_docno, member_no;
                Int32 capital_year;

                assist_docno = HfAssistDocNo.Value;
                capital_year = Convert.ToInt32(HfCapitalYear.Value);
                member_no = HfMemNo.Value;
                member_no = int.Parse(member_no).ToString("00000000");

                object[] args1 = new object[1];
                args1[0] = member_no;

                DwMem.Reset();
                DwUtil.RetrieveDataWindow(DwMem, "kt_50bath.pbl", null, args1);

                object[] args2 = new object[3];
                args2[0] = assist_docno;
                args2[1] = capital_year;
                args2[2] = member_no;

                DwMain.Reset();
                DwUtil.RetrieveDataWindow(DwMain, "kt_50bath.pbl", tDwMain, args2);

                object[] args3 = new object[2];
                args3[0] = assist_docno;
                args3[1] = capital_year;

                DwDetail.Reset();
                DwUtil.RetrieveDataWindow(DwDetail, "kt_50bath.pbl", null, args3);
                
                RetrieveBankBranch();
            }
            else if (HdTy.Value== "80")
            {
                String assist_docno, member_no;
                Int32 capital_year;

                assist_docno = HfAssistDocNo.Value;
                capital_year = Convert.ToInt32(HfCapitalYear.Value);
                member_no = HfMemNo.Value;

                object[] args1 = new object[1];
                args1[0] = HfMemNo.Value;

                DwMemP.Reset();
                DwUtil.RetrieveDataWindow(DwMemP, "kt_pension.pbl", null, args1);

                object[] args2 = new object[3];
                args2[0] = assist_docno;
                args2[1] = capital_year;
                args2[2] = member_no;

                DwMainP.Reset();
                DwUtil.RetrieveDataWindow(DwMainP, "kt_pension.pbl", tDwMain, args2);

                object[] args3 = new object[2];
                args3[0] = assist_docno;
                args3[1] = capital_year;

                DwDetailP.Reset();
                DwUtil.RetrieveDataWindow(DwDetailP, "kt_pension.pbl", null, args3);
                RetrieveBankBranch();
            }
            else if (HdTy.Value == "90")
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

                DwMainP.Reset();
                DwUtil.RetrieveDataWindow(DwMain65, "kt_65years.pbl", tDwMain, args2);

                object[] args3 = new object[2];
                args3[0] = assist_docno;
                args3[1] = capital_year;

                DwDetail65.Reset();
                DwUtil.RetrieveDataWindow(DwDetail65, "kt_65years.pbl", null, args3);
                RetrieveBankBranch();
            }
        }

        private void GetMemberDetail()
        {
            if (HdTy.Value == "70")
            {
                String member_no, prename_desc, memb_name, memb_surname, membgroup_code, membgroup_desc, membtype_code, membtype_desc;
                DateTime member_date, birth_date;
                member_no = HfMemberNo.Value;
                member_no = int.Parse(member_no).ToString("00000000");

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
                DwMain.SetItemString(1, "assisttype_code", "กองทุนสวัสดิการสมาชิก");

                tDwMain.Eng2ThaiAllRow();
            }
            else if (HdTy.Value == "80")
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

                DwMemP.SetItemString(1, "member_no", member_no);
                DwMemP.SetItemString(1, "prename_desc", prename_desc);
                DwMemP.SetItemString(1, "memb_name", memb_name);
                DwMemP.SetItemString(1, "memb_surname", memb_surname);
                DwMemP.SetItemString(1, "membgroup_desc", membgroup_desc);
                DwMemP.SetItemString(1, "membgroup_code", membgroup_code);
                DwMemP.SetItemString(1, "membtype_code", membtype_code);
                DwMemP.SetItemString(1, "membtype_desc", membtype_desc);
                DwMainP.SetItemDateTime(1, "member_date", member_date);
                DwMainP.SetItemString(1, "assisttype_code", "กองทุนสวัสดิบำนาญสมาชิก");

                tDwMainP.Eng2ThaiAllRow();
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
                    string sqlDate = @"select asnreqcapitaldet.pension_date,asnreqcapitaldet.member_age,asnreqcapitaldet.member_dead_case," +
                    "asnreqcapitaldet.member_receive from asnreqcapitaldet join asnreqmaster on asnreqmaster.assist_docno=asnreqcapitaldet.assist_docno where asnreqmaster.member_no ='" + member_no + "' and asnreqmaster.assist_docno like 'PS%'";
                    dt = ta.Query(sqlDate);
                    if (dt.Next())
                    {
                        age = dt.GetDecimal("member_age");
                        dead_case = dt.GetString("member_dead_case");
                        member_receive = dt.GetString("member_receive");
                        pensiondate = dt.GetDate("pension_date");
                        pension = pensiondate.ToString("yyyy");
                        yyyy = Convert.ToInt32(pension);
                        yyyy = yyyy + 543;
                        penddmmyyyy = pensiondate.ToString("ddMM") + yyyy.ToString();
                        HdSumpay.Value = "0";

                        DwDetailP.SetItemDecimal(1, "member_age", age);
                        DwDetailP.SetItemString(1, "member_dead_case", dead_case);
                        DwDetailP.SetItemString(1, "member_receive", member_receive);

                        if (pension != "1370" && pension != "")
                        {
                            HdDayDate.Value = penddmmyyyy;
                            HdDate.Value = pension;
                            HdDateStatus.Value = "1";
                            HadDate();

                        }
                        else { HdDateStatus.Value = "0"; }
                  }
                }
                catch {

                    HdDateStatus.Value = "0"; 
                }

            }
            else if (HdTy.Value == "90")
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
                    string sqlDate = @"select asnreqcapitaldet.pension_date,asnreqcapitaldet.member_age,asnreqcapitaldet.member_dead_case,"+
                    "asnreqcapitaldet.member_receive from asnreqcapitaldet join asnreqmaster on asnreqmaster.assist_docno=asnreqcapitaldet.assist_docno where asnreqmaster.member_no ='" + member_no + "' and asnreqmaster.assist_docno like 'SF%'";
                    dt = ta.Query(sqlDate);
                    if (dt.Next())
                    {
                        age = dt.GetDecimal("member_age");
                        dead_case = dt.GetString("member_dead_case");
                        member_receive = dt.GetString("member_receive");
                        pensiondate = dt.GetDate("pension_date");
                        pension = pensiondate.ToString("yyyy");
                        yyyy = Convert.ToInt32(pension);
                        yyyy = yyyy + 543;
                        penddmmyyyy = pensiondate.ToString("ddMM") + yyyy.ToString();
                        HdSumpay.Value = "0";

                        DwDetail65.SetItemDecimal(1,"member_age",age);
                        DwDetail65.SetItemString(1,"member_dead_case",dead_case);
                        DwDetail65.SetItemString(1,"member_receive",member_receive);

                        if (pension != "1370" && pension != "")
                        {
                            HdDayDate.Value = penddmmyyyy;
                            HdDate.Value = pension;
                            HdDateStatus.Value = "1";
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
            DwSelect.Reset();
            DwSelect.InsertRow(0);


            if (HdTy.Value == "70" || HdTy.Value =="")
            {
                tDwMain.Eng2ThaiAllRow();
                tDwDetail.Eng2ThaiAllRow();
                tDwMainP.Eng2ThaiAllRow();
                tDwDetailP.Eng2ThaiAllRow();
                tDwMain65.Eng2ThaiAllRow();
                tDwDetail65.Eng2ThaiAllRow();
                //DwSelect.Reset();
                DwMem.Reset();
                DwMain.Reset();
                DwDetail.Reset();
                DwMem.InsertRow(0);
                DwMain.InsertRow(0);
                DwDetail.InsertRow(0);

                DwMemP.Reset();
                DwDetailP.Reset();
                DwMainP.Reset();
                DwMainP.InsertRow(0);
                DwMemP.InsertRow(0);
                DwDetailP.InsertRow(0);

                DwMem65.Reset();
                DwDetail65.Reset();
                DwMain65.Reset();
                DwMain65.InsertRow(0);
                DwMem65.InsertRow(0);
                DwDetail65.InsertRow(0);
                
            }
            else if (HdTy.Value == "80")
            {
                tDwMainP.Eng2ThaiAllRow();
                tDwDetailP.Eng2ThaiAllRow();

                DwMainP.SetItemDecimal(1, "req_status", 8);
                DwMainP.SetItemDateTime(1, "pay_date", DateTime.Now);
                DwMainP.SetItemDateTime(1, "req_date", DateTime.Now);
                DwMainP.SetItemDateTime(1, "entry_date", DateTime.Now);
                DwMainP.SetItemDateTime(1, "approve_date", state.SsWorkDate);

            }
            else if (HdTy.Value == "90")
            {
                tDwMain65.Eng2ThaiAllRow();
                tDwDetail65.Eng2ThaiAllRow();

                DwMain65.SetItemDecimal(1, "req_status", 8);
                DwMain65.SetItemDateTime(1, "pay_date", DateTime.Now);
                DwMain65.SetItemDateTime(1, "req_date", DateTime.Now);
                DwMain65.SetItemDateTime(1, "entry_date", DateTime.Now);
                DwMain65.SetItemDateTime(1, "approve_date", state.SsWorkDate);

            }
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
            if (HdTy.Value == "70")
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
            else if (HdTy.Value == "80")
            {
                member_no = DwMemP.GetItemString(1, "member_no");
                membgroup_code = DwMemP.GetItemString(1, "membgroup_code");
                req_status = DwMainP.GetItemDecimal(1, "req_status");
                capital_year = DwMainP.GetItemDecimal(1, "capital_year");
                try
                {
                    salary_amt = DwMainP.GetItemDecimal(1, "salary_amt");
                }
                catch { salary_amt = 0; }

                try
                {
                    paytype_code = DwMainP.GetItemString(1, "paytype_code");
                }
                catch { paytype_code = ""; }
                try
                {
                    pay_date = DwMainP.GetItemDateTime(1, "pay_date");
                }
                catch { }
                try
                {
                    expense_bank = DwMainP.GetItemString(1, "expense_bank");
                }
                catch { expense_bank = ""; }
                try
                {
                    expense_branch = DwMainP.GetItemString(1, "expense_branch");
                }
                catch { expense_branch = ""; }
                try
                {
                    expense_accid = DwMainP.GetItemString(1, "expense_accid");
                }
                catch { expense_accid = ""; }

                try
                {
                    cancel_date = DwMainP.GetItemDateTime(1, "cancel_date");
                }
                catch { }
                try
                {
                    cancel_id = DwMainP.GetItemDecimal(1, "cancel_id");
                }
                catch { cancel_id = null; }
                try
                {
                    remark_cancel = DwMainP.GetItemString(1, "remark");
                }
                catch { remark_cancel = ""; }
            }
            else if (HdTy.Value == "90")
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
            String as_typcode = DwSelect.GetItemString(1, "assisttype_code");
            assisttype_code = as_typcode;
            if (as_typcode == "70")
            {
                HdTy.Value = "70";
                DwMain.Visible = true;
                DwMem.Visible = true;
                DwDetail.Visible = true;

                DwMainP.Visible = false;
                DwMemP.Visible = false;
                DwDetailP.Visible = false;

                DwMain65.Visible = false;
                DwMem65.Visible = false;
                DwDetail65.Visible = false;
            }
            else if (as_typcode == "80")
            {
                HdTy.Value = "80";
                DwMain.Visible = false;
                DwMem.Visible = false;
                DwDetail.Visible = false;

                DwMainP.Visible = true;
                DwMemP.Visible = true;
                DwDetailP.Visible = true;

                DwMain65.Visible = false;
                DwMem65.Visible = false;
                DwDetail65.Visible = false;

            }
            else if (as_typcode == "90")
            {
                HdTy.Value = "90";
                DwMain.Visible = false;
                DwMem.Visible = false;
                DwDetail.Visible = false;

                DwMain65.Visible = true;
                DwMem65.Visible = true;
                DwDetail65.Visible = true;

                DwMainP.Visible = false;
                DwMemP.Visible = false;
                DwDetailP.Visible = false;

            }

        }

        private void BDeadPayclick()
        {
            if (HdSumpay.Value != "" && HdTy.Value == "80")
            {
                decimal sump = Convert.ToDecimal(HdSumpay.Value);
                sump = sump + (DwDetailP.GetItemDecimal(1, "assist_amt") * 2);
                    
                HdSumpay.Value = sump.ToString();

                HdTotalpay.Value = HdSumpay.Value.Trim();
                DwDetailP.SetItemDecimal(1, "assist_amt", 0);
            }
            else if (HdSumpay.Value != "" && HdTy.Value == "90")
            {
                decimal sump = Convert.ToDecimal(HdSumpay.Value);
                sump = sump + (DwDetail65.GetItemDecimal(1, "assist_amt") * 2);

                HdSumpay.Value = sump.ToString();

                HdTotalpay.Value = HdSumpay.Value.Trim();
                DwDetail65.SetItemDecimal(1, "assist_amt", 0);
            }
            else { LtServerMessage.Text = WebUtil.ErrorMessage("รายการนี้ได้ทำรายการไปแล้วจ่าย"); }

        }

        private void BPayclick()
        {

            if (HdSumpay.Value != "" && Hdck.Value != "1" && HdTy.Value == "80")
            {
                Hdck.Value = "1";
                double totalpay = 0;
                decimal perpayTemp = DwDetailP.GetItemDecimal(1, "sperpay"); // ค่า 5%
                double sumpay_temp = Convert.ToDouble(HdSumpay.Value); // ค่า pay ที่เคยจ่ายไปแล้ว

                totalpay = Convert.ToDouble(perpayTemp) + sumpay_temp;
                HdTotalpay.Value = totalpay.ToString();
                double assist_value = DwDetailP.GetItemDouble(1, "assist_amt");
                double calculate = assist_value - Convert.ToDouble(perpayTemp);
                try
                {
                    if (calculate <= 0)
                        calculate = 0.00;
                    DwDetailP.SetItemDecimal(1, "assist_amt", Convert.ToDecimal(calculate));
                }
                catch
                {
                }

            }
            else if (HdSumpay.Value != "" && Hdck.Value != "1" && HdTy.Value == "90")
            {
                Hdck.Value = "1";
                double totalpay = 0;
                decimal perpayTemp = DwDetail65.GetItemDecimal(1, "sperpay"); // ค่า 5%
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
            if (HdTy.Value == "80")
            {
                DateTime member_dead_date;
                string temp = DwMemP.GetItemString(1, "member_no");
                member_date = DwMainP.GetItemDateTime(1, "member_date");

                //   member_dead_tdate = DwDetail.GetItemString(1, "member_dead_tdate");34

                member_dead_date = DateTime.ParseExact(HdDayDate.Value, "ddMMyyyy", WebUtil.TH);
                DwDetailP.SetItemDateTime(1, "member_dead_date", member_dead_date);


                //    DwDetail.SetItemString(1, "member_dead_tdate", member_dead_tdate);//testdate ลองเพิ่ม

                tp = member_dead_date - member_date;
                Double mem_year = (tp.TotalDays / 365);
                if (mem_year < 1)
                {
                    LtServerMessage.Text = WebUtil.WarningMessage("สมาชิกรายนี้ยังเป็นสมาชิกไม่ถึง 1 ปี จนถึงวันเสียชีวิต");
                    //return;
                }

                //หาว่าเกิน 120 วันหรือไม่
                req_date = DwMainP.GetItemDateTime(1, "req_date");
                tp = req_date - member_dead_date;
                Double a = (tp.TotalDays);
                if (a < 120)
                {
                    //LtServerMessage.Text = WebUtil.WarningMessage("สมาชิกรายนี้ยื่นคำขอเกินกว่า 120 วัน");
                    //return;
                }

                sqlStr = @"select envvalue
                           from asnsenvironmentvar
                           WHERE envcode = 'memdead_open_date'";
                dt = ta.Query(sqlStr);
                dt.Next();
                envvalue = dt.GetString("envvalue");
                center_date = Convert.ToDateTime(envvalue);

                tDwDetailP.Eng2ThaiAllRow();
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

                    if (sumpay_amt == "")
                    {
                        sumpay_amt = "0";
                    }
                    HdSumpay.Value = sumpay_amt;
                    //HdTotalpay.Value = "1";
                }
                catch
                {
                }

                try
                {
                    string entry_date = "";
                    if (HdDateStatus.Value == "1")
                    {
                        entry_date = HdDate.Value;
                    }
                    else
                    {
                        entry_date = DwDetailP.GetItemDateTime(1, "member_dead_date").ToString("yyyy");
                    }
                    string memb_date = DwMainP.GetItemDateTime(1, "member_date").ToString("yyyy");
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
                    DwDetailP.SetItemDecimal(1, "assist_amt", Convert.ToDecimal(setDeadpay_amt));
                    DwDetailP.SetItemDecimal(1, "sperpay", Convert.ToDecimal(perpay));
                    if (Convert.ToDouble(HdSumpay.Value) >= (perpay * 10))
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("จ่ายเงินครบ 10 งวดแล้ว");
                    }

                }
                catch (Exception er)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(er);
                }

            }
            else if (HdTy.Value == "90")
            {
                DateTime member_dead_date;
                string temp = DwMem65.GetItemString(1, "member_no");
                member_date = DwMain65.GetItemDateTime(1, "member_date");

                member_dead_date = DateTime.ParseExact(HdDayDate.Value, "ddMMyyyy", WebUtil.TH);
                DwDetail65.SetItemDateTime(1, "member_dead_date", member_dead_date);

                //                tp = member_dead_date - member_date;
                //                Double mem_year = (tp.TotalDays / 365);
                //                if (mem_year < 1)
                //                {
                //                    LtServerMessage.Text = WebUtil.WarningMessage("สมาชิกรายนี้ยังเป็นสมาชิกไม่ถึง 1 ปี จนถึงวันเสียชีวิต");
                //                    //return;
                //                }

                //                //หาว่าเกิน 120 วันหรือไม่
                //                req_date = DwMain65.GetItemDateTime(1, "req_date");
                //                tp = req_date - member_dead_date;
                //                Double a = (tp.TotalDays);
                //                if (a < 120)
                //                {
                //                    LtServerMessage.Text = WebUtil.WarningMessage("สมาชิกรายนี้ยื่นคำขอเกินกว่า 120 วัน");
                //                    //return;
                //                }

                //                sqlStr = @"select envvalue
                //                           from asnsenvironmentvar
                //                           WHERE envcode = 'memdead_open_date'";
                //                dt = ta.Query(sqlStr);
                //                dt.Next();
                //                envvalue = dt.GetString("envvalue");
                //                center_date = Convert.ToDateTime(envvalue);

                //                tDwDetail65.Eng2ThaiAllRow();
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
                    //HdTotalpay.Value = "1";
                }
                catch
                {
                }

                try
                {

                    string entry_date = DwDetail65.GetItemDateTime(1, "member_dead_date").ToString("yyyy");

                    string memb_date = DwMain65.GetItemDateTime(1, "member_date").ToString("yyyy");
                    //int memb_age = (Convert.ToInt32(entry_date) + 543) - (Convert.ToInt32(memb_date) + 543);


                    string memNo = DwMem65.GetItemString(1, "member_no");
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


                    double shareAmt = 0;
                    double setDeadpay_amt = 0;
                    shareAmt = Convert.ToDouble(sumshare);
                    if (memb_age >= 2 && memb_age < 5)
                    {
                        setDeadpay_amt = 0.1 * shareAmt;
                    }
                    else if (memb_age >= 5 && memb_age < 10)
                    {
                        setDeadpay_amt = 0.2 * shareAmt;

                    }
                    else if (memb_age >= 10 && memb_age < 15)
                    {
                        setDeadpay_amt = 0.3 * shareAmt;
                    }
                    else if (memb_age >= 15 && memb_age < 20)
                    {
                        setDeadpay_amt = 0.35 * shareAmt;

                    }
                    else if (memb_age >= 20)
                    {
                        setDeadpay_amt = 0.4 * shareAmt;
                    }
                    if (setDeadpay_amt > 500000)
                    {
                        setDeadpay_amt = 500000;
                    }
                    double perpay = setDeadpay_amt * 0.01;
                    if (perpay > 10000)
                    {
                        perpay = 10000;
                    }
                    setDeadpay_amt = setDeadpay_amt - Convert.ToDouble(sumpay_amt);
                    if (setDeadpay_amt <= 0)
                    {
                        setDeadpay_amt = 0;
                    }
                    DwDetail65.SetItemDecimal(1, "assist_amt", Convert.ToDecimal(setDeadpay_amt));
                    DwDetail65.SetItemDecimal(1, "sperpay", Convert.ToDecimal(perpay));

                }
                catch (Exception er)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(er);
                }
            }

        }
        private void SavePS()
        {
            string ass_docno = "";
            string ck = "";
            string memNo = DwUtil.GetString(DwMemP, 1, "member_no");
            try
            {
                string qurryass = "select max(assist_docno) from asnreqmaster where assisttype_code = '80'and assist_docno like 'PS%'";
                DataTable qass = WebUtil.Query(qurryass);
                if (qass.Rows.Count > 0)
                {
                    ass_docno = qass.Rows[0][0].ToString();
                    if (ass_docno != "")
                    {
                        ass_docno = ass_docno.Substring(2,6);
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
            string AssAmt = "PS" + ass.ToString("000000");
            try
            {
                string savesql = "insert into asnreqmaster (assist_docno,capital_year,member_no,assisttype_code) values ('" + AssAmt + "'," + dDate + ",'" + memNo + "','80')";
                DataTable savekt = WebUtil.Query(savesql);

            }

            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถบันทึกข้อมูล ในกองทุนได้");
            }


        }
        private void SaveSF()
        {
            string ass_docno = "";
            string ck = "";
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
  
            if (HdTy.Value == "80")
            {
                string moneyType = HdMoneyType.Value;//DwMainP.GetItemString(1, "moneytype_code");
                string member_no = HfMemberNo.Value;
                DateTime stamentdate = DwMainP.GetItemDateTime(1, "entry_date");
                decimal payout = DwDetailP.GetItemDecimal(1, "assist_amt");
                if (payout != 0)
                {
                    payout = DwDetailP.GetItemDecimal(1, "sperpay");
                }

                string sqlQu = "select max(payoutslip_no) from asnslippayout where payoutslip_no like '" + yearnow + HdTy.Value + "%'";
                DataTable slip_no = WebUtil.Query(sqlQu);
                if (slip_no.Rows.Count > 0)
                {
                    payout_no = slip_no.Rows[0][0].ToString();
                    if (payout_no != "")
                    {
                        payout_no = payout_no.Substring(4, 6);
                    }
                    else { payout_no = "000000"; }
                }
                Decimal ass = Convert.ToDecimal(payout_no);
                ass++;
                string slippayout_no = yearnow + HdTy.Value + ass.ToString("000000");

                if (moneyType == "TRN")
                {
                    string sqlupdateasn = @"update asnreqmaster set moneytype_code = '" + moneyType + "' , to_system = 'DIV' where member_no = '" + member_no + "' and assisttype_code = '80'";
                    ta.Exe(sqlupdateasn);

                    string sqlsavestate = @"insert into asnslippayout (payoutslip_no,member_no,slip_date,operate_date,payout_amt,slip_status,assisttype_code,entry_id,entry_date,moneytype_code)" +
                        "values('" + slippayout_no + "','" + member_no + "',to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy')," +
                     +payout + ",1,'" + HdTy.Value + "','" + entry_id + "',to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),'" + moneyType + "')";
                    ta.Exe(sqlsavestate);
                }
                else if (moneyType == "CBT")
                {
                    string bankCode = DwMainP.GetItemString(1, "asnslippayout_bank_code");
                    string bankBranch = DwMainP.GetItemString(1, "asnslippayout_bankbranch_id");
                    string bankAcc = DwMainP.GetItemString(1, "asnslippayout_bank_accid");
                    
                    string sqlupdateasn = @"update asnreqmaster set moneytype_code = '" + moneyType + "' where member_no = '" + member_no + "' and assisttype_code = '80'";
                    ta.Exe(sqlupdateasn);

                    string sqlsavestate = @"insert into asnslippayout (payoutslip_no,member_no,slip_date,operate_date,payout_amt,slip_status,assisttype_code,entry_id,entry_date,moneytype_code,bank_code,bankbranch_id,bank_accid)" +
                        "values('" + slippayout_no + "','" + member_no + "',to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy')," +
                     +payout + ",1,'" + HdTy.Value + "','" + entry_id + "',to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),'" + moneyType + "','"+bankCode+"','"+bankBranch +"','"+bankAcc+"')";
                    ta.Exe(sqlsavestate);
                }
                else
                {
                    string sqlupdateasn = @"update asnreqmaster set moneytype_code = '" + moneyType + "' where member_no = '" + member_no + "' and assisttype_code = '80'";
                    ta.Exe(sqlupdateasn);

                    string sqlsavestate = @"insert into asnslippayout (payoutslip_no,member_no,slip_date,operate_date,payout_amt,slip_status,assisttype_code,entry_id,entry_date,moneytype_code)" +
                        "values('" + slippayout_no + "','" + member_no + "',to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy')," +
                     +payout + ",1,'" + HdTy.Value + "','" + entry_id + "',to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),'" + moneyType + "')";
                    ta.Exe(sqlsavestate);
                }


            }
            else if (HdTy.Value == "90")
            {
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
            else if (HdTy.Value == "70")
            {
                string member_no = HfMemberNo.Value;
                string moneyType = HdMoneyType.Value;
                DateTime stamentdate = DwMain.GetItemDateTime(1, "entry_date");
                decimal payout = DwDetail.GetItemDecimal(1, "assist_amt");
                if (payout != 0)
                {
                    payout = DwDetail.GetItemDecimal(1, "sperpay");
                }

                string sqlQu = "select max(payoutslip_no) from asnslippayout where payoutslip_no like '" + yearnow + HdTy.Value + "%'";
                DataTable slip_no = WebUtil.Query(sqlQu);
                if (slip_no.Rows.Count > 0)
                {
                    payout_no = slip_no.Rows[0][0].ToString();
                    if (payout_no != "")
                    {
                        payout_no = payout_no.Substring(4, 6);
                    }
                    else { payout_no = "000000"; }
                }
                Decimal ass = Convert.ToDecimal(payout_no);
                ass++;
                string slippayout_no = yearnow + HdTy.Value + ass.ToString("000000");

                if (moneyType == "TRN")
                {
                    string sqlupdateasn = @"update asnreqmaster set moneytype_code = '" + moneyType + "' , to_system = 'DIV' where member_no = '" + member_no + "' and assisttype_code = '70'";
                    ta.Exe(sqlupdateasn);

                    string sqlsavestate = @"insert into asnslippayout (payoutslip_no,member_no,slip_date,operate_date,payout_amt,slip_status,assisttype_code,entry_id,entry_date,moneytype_code)" +
                        "values('" + slippayout_no + "','" + member_no + "',to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy')," +
                     +payout + ",1,'" + HdTy.Value + "','" + entry_id + "',to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),'" + moneyType + "')";
                    ta.Exe(sqlsavestate);
                }
                else if (moneyType == "CBT")
                {
                    string bankCode = DwMain.GetItemString(1, "asnslippayout_bank_code");
                    string bankBranch = DwMain.GetItemString(1, "asnslippayout_bankbranch_id");
                    string bankAcc = DwMain.GetItemString(1, "asnslippayout_bank_accid");

                    string sqlupdateasn = @"update asnreqmaster set moneytype_code = '" + moneyType + "' where member_no = '" + member_no + "' and assisttype_code = '70'";
                    ta.Exe(sqlupdateasn);

                    string sqlsavestate = @"insert into asnslippayout (payoutslip_no,member_no,slip_date,operate_date,payout_amt,slip_status,assisttype_code,entry_id,entry_date,moneytype_code,bank_code,bankbranch_id,bank_accid)" +
                        "values('" + slippayout_no + "','" + member_no + "',to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy')," +
                     +payout + ",1,'" + HdTy.Value + "','" + entry_id + "',to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),'" + moneyType + "','" + bankCode + "','" + bankBranch + "','" + bankAcc + "')";
                    ta.Exe(sqlsavestate);
                }
                else
                {
                    string sqlupdateasn = @"update asnreqmaster set moneytype_code = '" + moneyType + "' where member_no = '" + member_no + "' and assisttype_code = '70'";
                    ta.Exe(sqlupdateasn);

                    string sqlsavestate = @"insert into asnslippayout (payoutslip_no,member_no,slip_date,operate_date,payout_amt,slip_status,assisttype_code,entry_id,entry_date,moneytype_code)" +
                        "values('" + slippayout_no + "','" + member_no + "',to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy')," +
                     +payout + ",1,'" + HdTy.Value + "','" + entry_id + "',to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),'" + moneyType + "')";
                    ta.Exe(sqlsavestate);
                }


                //string sqlsavestate = @"insert into asnslippayout (payoutslip_no,member_no,slip_date,operate_date,payout_amt,slip_status,assisttype_code,entry_id,entry_date)" +
                //    "values('" + slippayout_no + "','" + member_no + "',to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy')," +
                // +payout + ",1,'" + HdTy.Value + "','" + entry_id + "',to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'))";
                //ta.Exe(sqlsavestate);
            }

        }

        private void MoneyType()
        {
            if (HdTy.Value == "70")
            {
                HdMoneyType.Value = DwMain.GetItemString(1, "moneytype_code");
            }
            else if (HdTy.Value == "80")
            {
                HdMoneyType.Value = DwMainP.GetItemString(1, "moneytype_code");
            }
            else if (HdTy.Value == "90")
            {
                HdMoneyType.Value = DwMain65.GetItemString(1, "moneytype_code");
            }
        }

        private void JspostFilterBranch()
        {
            if (HdTy.Value == "70")
            {
                try
                {
                    String bankcode = DwMain.GetItemString(1, "asnslippayout_bank_code");
                    DwUtil.RetrieveDDDW(DwMain, "asnslippayout_bankbranch_id", "kt_50bath.pbl", null);
                    DataWindowChild dc = DwMain.GetChild("asnslippayout_bankbranch_id");
                    dc.SetFilter("bank_code ='" + bankcode + "'");
                    dc.Filter();
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            }
            else if (HdTy.Value == "80")
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
            else if (HdTy.Value == "90")
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

        }

    }
}
