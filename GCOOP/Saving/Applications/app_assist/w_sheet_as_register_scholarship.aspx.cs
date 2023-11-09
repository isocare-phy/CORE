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
using System.Timers;
using DataLibrary;
using Sybase.DataWindow;
using System.Web.Services.Protocols;

namespace Saving.Applications.app_assist
{
    public partial class w_sheet_as_register_scholarship : PageWebSheet, WebSheet
    {
        protected DwThDate tDwMain;
        protected DwThDate tDwDetail;

        protected String postNewClear;
        protected String postRefresh;
        protected String postGetMoney;
        protected String postChangeHeight;
        protected String postRetreiveDwMem;
        protected String postRetrieveDwMain;
        protected String postGetMemberDetail;
        protected String postFilterScholarship;
        protected String postCheckReQuest;
        protected String postRetrieveBankBranch;

        private DataStore Ds;

        private String sqlStr;
        private String member_no, assist_docno, membgroup_code, married_member;
        private String expense_bank, expense_branch, expense_accid, paytype_code, remark_cancel;
        private Decimal capital_year, salary_amt, assist_amt, req_status, seq_no, seq_no_same, level_School, level_School_same, sum_real_seq, now_year;
        private DateTime pay_date, cancel_date = DateTime.Now, req_date;
        private Nullable<Decimal> cancel_id;

        private Sta ta;
        private Sdt dt;

        TimeSpan tp;

        public void InitJsPostBack()
        {
            tDwMain = new DwThDate(DwMain, this);
            tDwDetail = new DwThDate(DwDetail, this);

            tDwMain.Add("pay_date", "pay_tdate");
            tDwMain.Add("req_date", "req_tdate");
            tDwMain.Add("entry_date", "entry_tdate");
            tDwMain.Add("member_date", "member_tdate");
            tDwMain.Add("approve_date", "approve_tdate");
            tDwMain.Add("cancel_date", "cancel_tdate");

            tDwDetail.Add("childbirth_date", "childbirth_tdate");
            //tDwDetail.Add("entry_date", "entry_tdate");

            postNewClear = WebUtil.JsPostBack(this, "postNewClear");
            postRefresh = WebUtil.JsPostBack(this, "postRefresh");
            postGetMoney = WebUtil.JsPostBack(this, "postGetMoney");
            postCheckReQuest = WebUtil.JsPostBack(this, "postCheckReQuest");
            postChangeHeight = WebUtil.JsPostBack(this, "postChangeHeight");
            postRetreiveDwMem = WebUtil.JsPostBack(this, "postRetreiveDwMem");
            postFilterScholarship = WebUtil.JsPostBack(this, "postFilterScholarship");
            postRetrieveDwMain = WebUtil.JsPostBack(this, "postRetrieveDwMain");
            postGetMemberDetail = WebUtil.JsPostBack(this, "postGetMemberDetail");
            postRetrieveBankBranch = WebUtil.JsPostBack(this, "postRetrieveBankBranch");
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();

            ta = new Sta(sqlca.ConnectionString);
            dt = new Sdt();

            tp = new TimeSpan();
            Ds = new DataStore();

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
                HdIsPostBack.Value = "true";
            }

            //DwDetail.SetItemDateTime(1, "entry_date", state.SsWorkDate);
            //DwDetail.SetItemString(1, "entry_id", state.SsUsername);
            tDwMain.Eng2ThaiAllRow();
            tDwDetail.Eng2ThaiAllRow();

            DwUtil.RetrieveDDDW(DwMain, "cancel_id", "as_capital.pbl", null);
            DwUtil.RetrieveDDDW(DwMain, "req_status", "as_capital.pbl", null);
            DwUtil.RetrieveDDDW(DwMain, "paytype_code", "as_capital.pbl", null);
            DwUtil.RetrieveDDDW(DwMain, "capital_year", "as_capital.pbl", null);
            DwUtil.RetrieveDDDW(DwMain, "expense_bank", "as_capital.pbl", null);
            DwUtil.RetrieveDDDW(DwMain, "assisttype_code", "as_capital.pbl", null);
            DwUtil.RetrieveDDDW(DwMain, "pay_status", "as_capital.pbl", null);
            DwUtil.RetrieveDDDW(DwDetail, "scholarship_type", "as_capital.pbl", null);
            DwUtil.RetrieveDDDW(DwDetail, "scholarship_level", "as_capital.pbl", null);
            //DwUtil.RetrieveDDDW(DwDetail, "level_school", "as_capital.pbl", null);
            DwUtil.RetrieveDDDW(DwDetail, "childprename_code", "as_capital.pbl", null);
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postRetreiveDwMem")
            {
                RetreiveDwMem();
            }
            else if (eventArg == "postNewClear")
            {
                NewClear();
            }
            else if (eventArg == "postRefresh")
            {
                Refresh();
            }
            else if (eventArg == "postFilterScholarship")
            {
                FilterScholarship();
            }
            else if (eventArg == "postRetrieveDwMain")
            {
                RetrieveDwMain();
            }
            else if (eventArg == "postRetrieveBankBranch")
            {
                RetrieveBankBranch();
            }
            else if (eventArg == "postCheckReQuest")
            {
                CheckReQuest();
            }
            else if (eventArg == "postChangeHeight")
            {
                ChangeHeight();
            }
            else if (eventArg == "postGetMoney")
            {
                GetMoney();
            }
            else if (eventArg == "postGetMemberDetail")
            {
                GetMemberDetail();
            }
        }

        public void SaveWebSheet()
        {
            String childprename_code, child_name, child_surname, child_sex, childschool_name, childbirth_tdate, remark;
            Decimal scholarship_type, scholarship_level, level_school, child_gpa, scholarship1_amt;
            DateTime childbirth_date;

            //Sta ta = new Sta(sqlca.ConnectionString);
            //Sdt dt = new Sdt();

            GetItemDwMain();

            //childprename_code = DwDetail.GetItemString(1, "childprename_code");
            child_name = DwDetail.GetItemString(1, "child_name").Trim();
            child_surname = DwDetail.GetItemString(1, "child_surname").Trim();
            //child_sex = DwDetail.GetItemString(1, "child_sex");
            //try
            //{
            //    childschool_name = DwDetail.GetItemString(1, "childschool_name");
            //}
            //catch { childschool_name = ""; }
            try
            {
                //childbirth_date = DwDetail.GetItemDateTime(1, "childbirth_date");

                childbirth_tdate = DwDetail.GetItemString(1, "childbirth_tdate");
                childbirth_date = DateTime.ParseExact(childbirth_tdate, "ddMMyyyy", WebUtil.TH);
                DwDetail.SetItemDateTime(1, "childbirth_date", childbirth_date);
            }
            catch { childbirth_date = DateTime.Now; }
            //scholarship_type = DwDetail.GetItemDecimal(1, "scholarship_type");
            //scholarship_level = DwDetail.GetItemDecimal(1, "scholarship_level");
            level_school = DwDetail.GetItemDecimal(1, "level_school");
            child_gpa = DwDetail.GetItemDecimal(1, "child_gpa");
            assist_amt = DwDetail.GetItemDecimal(1, "assist_amt");
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
                //เช็คชื่อลูกซ้ำ
                Int16 same = CheckNameSon(member_no, capital_year, child_name, child_surname);
                if (same == 0)
                {
                    String level_text = "";

                    if (level_School_same == 1)
                    {
                        level_text = "ประถม";
                    }
                    else if (level_School_same == 2)
                    {
                        level_text = "มัธยมต้น";
                    }
                    else if (level_School_same == 3)
                    {
                        level_text = "มัธยมปลายและปวช.";
                    }
                    else if (level_School_same == 5)
                    {
                        level_text = "ปวส.";
                    }
                    else if (level_School_same == 6)
                    {
                        level_text = "ปริญญาตรี";
                    }
                    else if (level_School_same == 7)
                    {
                        level_text = "ปริญญาโท";
                    }

                    LtServerMessage.Text = WebUtil.WarningMessage("สมาชิกรายนี้เคยขอทุนให้กับบุตรรายนี้ไปแล้วในระดับชั้น" + level_text + "ลำดับที่" + seq_no_same);
                    return;
                }

                //เช็คเกรดไม่ถึง 2.00
                if (child_gpa < 2)
                {
                    LtServerMessage.Text = WebUtil.WarningMessage("บุตรของสมาชิกมีเกรดเฉลี่ยไม่ถึง 2.00");
                    return;
                }

                //เช็คอายุ ตรี โท
                tp = DateTime.Now - childbirth_date;
                Double a = (tp.TotalDays / 365);
                if (level_school == 6)
                {
                    if (a > 27)
                    {
                        LtServerMessage.Text = WebUtil.WarningMessage("บุตรอยู่ในระดับปริญญาตรีมีอายุเกิน 27 ไม่สามารถขอทุนได้");
                        return;
                    }
                }
                else if (level_school == 7)
                {
                    if (a > 30)
                    {
                        LtServerMessage.Text = WebUtil.WarningMessage("บุตรอยู่ในระดับปริญญาโทมีอายุเกิน 30 ไม่สามารถขอทุนได้");
                        return;
                    }
                }

                assist_docno = GetLastDocNo(capital_year);

                try
                {
                    DwMain.SetItemString(1, "member_no", member_no);
                    DwMain.SetItemString(1, "membgroup_code", membgroup_code);
                    DwMain.SetItemDecimal(1, "assist_amt", assist_amt);
                    DwMain.SetItemString(1, "assist_docno", assist_docno);
                    DwMain.SetItemString(1, "married_member", married_member);

                    try
                    {
                        DwUtil.InsertDataWindow(DwMain, "as_capital.pbl", "asnreqmaster");
                    }
                    catch (Exception ex)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                        return;
                    }

                    try
                    {
                        seq_no = DwDetail.GetItemDecimal(1, "seq_no");
                    }
                    catch { seq_no = 0; }
                    if (seq_no == 0)
                    {
                        seq_no = GetSeqNo(level_school);
                    }

                    DwDetail.SetItemDecimal(1, "seq_no", seq_no);
                    DwDetail.SetItemString(1, "assist_docno", assist_docno);
                    DwDetail.SetItemDecimal(1, "capital_year", capital_year);
                    DwDetail.SetItemDateTime(1, "capital_date", state.SsWorkDate);
                    DwDetail.SetItemDateTime(1, "entry_date", state.SsWorkDate);
                    DwDetail.SetItemString(1, "entry_id", state.SsUsername);
                    try
                    {
                        DwUtil.InsertDataWindow(DwDetail, "as_capital.pbl", "asnreqschooldet");
                    }
                    catch { }

                    //                    sqlStr = @"INSERT INTO asnreqmaster(assist_docno  ,  capital_year  ,  member_no  ,  assisttype_code  ,
                    //                                                assist_amt    ,  req_status ,  req_date  ,  pay_date  ,  paytype_code  ,  remark  ,
                    //                                                membgroup_code  ,  expense_bank  ,  expense_branch  ,  expense_accid)
                    //                                         VALUES('" + assist_docno + "','" + capital_year + "','" + member_no + @"','10',
                    //                                                '" + assist_amt + "','" + req_status + "',to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + @"', 'dd/mm/yyyy'),to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + @"', 'dd/mm/yyyy'),
                    //                                                '" + paytype_code + @"',
                    //                                                '" + remark + "','" + membgroup_code + "','" + expense_bank + "','" + expense_branch + "','" + expense_accid + "')";
                    //                    ta.Exe(sqlStr);

                    //                    sqlStr = @"INSERT INTO asnreqschooldet(assist_docno  ,  capital_year  ,  capital_date  ,  childprename_code  ,  child_name  ,
                    //                                                   child_surname  ,  childschool_name  ,  child_gpa  ,  child_sex  ,
                    //                                                   assist_amt  ,  scholarship_level  ,  scholarship_type  ,  entry_date)
                    //                                            VALUES('" + assist_docno + "','" + capital_year + "',to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + "', 'dd/mm/yyyy'),'" + childprename_code + @"',
                    //                                                   '" + child_name + @"','" + child_surname + "','" + childschool_name + "','" + child_gpa + @"',
                    //                                                   '" + child_sex + "','" + assist_amt + "','" + scholarship_level + "','" + scholarship_type + @"',
                    //                                                   to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + "', 'dd/mm/yyyy'))";
                    //                    ta.Exe(sqlStr);

                    sqlStr = @"INSERT INTO asnmemsalary(capital_year  ,  member_no  ,  assist_docno  ,
                                                    salary_amt    ,  entry_date  ,  membgroup_code)
                                         VALUES('" + capital_year + "','" + member_no + "','" + assist_docno + @"',
                                                '" + salary_amt + "',to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + "', 'dd/mm/yyyy'),'" + membgroup_code + @"')";
                    ta.Exe(sqlStr);

                    sqlStr = @"UPDATE  asndoccontrol
                               SET     last_docno   = '" + WebUtil.Right(assist_docno, 6) + @"'
                               WHERE   doc_prefix   = 'AS' and
                                       doc_year = '" + capital_year + "'";
                    ta.Exe(sqlStr);

                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกเรียบร้อย");
                    //NewClear();
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

                    sqlStr = @"UPDATE  asnmemsalary
                               SET     salary_amt    = '" + salary_amt + @"'
                               WHERE   assist_docno  = '" + assist_docno + "'";
                    ta.Exe(sqlStr);

                    try
                    {
                        DwUtil.UpdateDataWindow(DwMain, "as_capital.pbl", "asnreqmaster");
                    }
                    catch { }

                    try
                    {
                        DwUtil.UpdateDataWindow(DwDetail, "as_capital.pbl", "asnreqschooldet");
                    }
                    catch { }

                    LtServerMessage.Text = WebUtil.CompleteMessage("แก้ไขเรียบร้อย");
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            }
            level_School = DwDetail.GetItemDecimal(1, "level_School");
            Session["level_School"] = level_School;
        }

        public void WebSheetLoadEnd()
        {
            tDwMain.Eng2ThaiAllRow();
            tDwDetail.Eng2ThaiAllRow();

            DwMem.SaveDataCache();
            DwMain.SaveDataCache();
            DwDetail.SaveDataCache();
            Ds.Reset();
            dt.Clear();
            ta.Close();
        }



        private void Refresh()
        {

        }

        private void GetMoney()
        {
            String sqlStr;
            Decimal scholarship_level, scholarship_type, scholarship1_amt;

            scholarship_level = DwDetail.GetItemDecimal(1, "scholarship_level");
            scholarship_type = DwDetail.GetItemDecimal(1, "scholarship_type");

            sqlStr = @"SELECT scholarship1_amt
                               FROM asnscholarship
                               WHERE school_level     = '" + scholarship_level + @"' and
                                     scholarship_type = '" + scholarship_type + "'";
            dt = ta.Query(sqlStr);
            dt.Next();
            try
            {
                scholarship1_amt = Convert.ToDecimal(dt.GetDouble("scholarship1_amt"));
            }
            catch
            {
                scholarship1_amt = 0;
            }
            DwMain.SetItemDecimal(1, "assist_amt", scholarship1_amt);
            DwDetail.SetItemDecimal(1, "assist_amt", scholarship1_amt);
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
            String member_no;

            member_no = HfMemberNo.Value;

            object[] args = new object[1];
            args[0] = member_no;

            DwMem.Reset();
            DwUtil.RetrieveDataWindow(DwMem, "as_capital.pbl", null, args);
            if (DwMem.RowCount < 1)
            {
                LtServerMessage.Text = WebUtil.WarningMessage("ไม่มีเลขที่สมาชิกนี้");
                //LtAlert.Text = "<script>Alert()</script>";
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
            DwUtil.RetrieveDataWindow(DwMem, "as_capital.pbl", null, args1);

            object[] args2 = new object[3];
            args2[0] = assist_docno;
            args2[1] = capital_year;
            args2[2] = member_no;

            DwMain.Reset();
            DwUtil.RetrieveDataWindow(DwMain, "as_capital.pbl", tDwMain, args2);

            DwDetail.Reset();
            DwUtil.RetrieveDataWindow(DwDetail, "as_capital.pbl", tDwDetail, args2);

            tDwDetail.Eng2ThaiAllRow();

            RetrieveBankBranch();
        }

        private void GetMemberDetail()
        {
            String member_no, prename_desc, memb_name, memb_surname, membgroup_code, membgroup_desc, membtype_code, membtype_desc;
            DateTime member_date, birth_date, lastkeeping_date;
            Decimal salary_amount;

            member_no = HfMemberNo.Value;

            //DataStore Ds = new DataStore();
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
            try
            {
                birth_date = Ds.GetItemDateTime(1, "birth_date");
            }
            catch { }
            salary_amount = Ds.GetItemDecimal(1, "salary_amount");
            try
            {
                lastkeeping_date = Ds.GetItemDateTime(1, "lastkeeping_date");
            }
            catch { }

            tp = DateTime.Now - member_date;
            Double a = (tp.TotalDays / 365);
            if (a < 1)
            {
                LtServerMessage.Text = WebUtil.WarningMessage("สมาชิกร้ายนี้ยังเป็นสมาชิกไม่ถึง 1 ปี");
                //return;
            }

            sum_real_seq = CheckReq(member_no);
            if (sum_real_seq > 0)
            {
                LtServerMessage.Text = WebUtil.WarningMessage("สมาชิกรายนี้ขอทุนในปีที่แล้วไปแล้ว");
                //return;
            }

            Int16 same_married = CheckMarried(member_no);
            if (same_married == 0)
            {
                LtServerMessage.Text = WebUtil.WarningMessage("สมาชิกรายนี้มีคู่สมรสขอทุนไปแล้ว");
                //return;
            }

            int index = memb_surname.IndexOf("(");
            if (index > 0)
            {
                memb_surname = WebUtil.Left(memb_surname, index);
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
            DwMain.SetItemDecimal(1, "salary_amt", salary_amount);

            DwDetail.SetItemString(1, "child_surname", memb_surname);

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
            DwMem.Reset();
            DwMain.Reset();
            DwDetail.Reset();

            DwMem.InsertRow(0);
            DwMain.InsertRow(0);
            DwDetail.InsertRow(0);

            level_School = Convert.ToDecimal(Session["level_School"]);
            if (level_School != 0)
            {
                DwDetail.SetItemDecimal(1, "level_School", level_School);

                DwUtil.RetrieveDDDW(DwDetail, "scholarship_level", "as_capital.pbl", null);
                DataWindowChild Dc = DwDetail.GetChild("scholarship_level");
                Dc.SetFilter("school_group =" + level_School + "");
                Dc.Filter();

                Session["level_School"] = 0;
            }

            now_year = DateTime.Now.Year;
            DwMain.SetItemDecimal(1, "capital_year", now_year + 543);

            DwMain.SetItemDecimal(1, "req_status", 8);
            DwMain.SetItemString(1, "assisttype_code", "10");
            DwMain.SetItemDateTime(1, "pay_date", state.SsWorkDate);
            DwMain.SetItemDateTime(1, "req_date", state.SsWorkDate);
            DwMain.SetItemDateTime(1, "entry_date", state.SsWorkDate);
            //DwMain.SetItemDateTime(1, "approve_date", state.SsWorkDate);
        }

        private void CheckReQuest()
        {
            String member_no;
            try
            {
                member_no = DwMem.GetItemString(1, "member_no");
            }
            catch { member_no = ""; }

            //last_year = CheckReq(member_no);
            //capital_year = DwMain.GetItemDecimal(1, "capital_year");
            //if (last_year == capital_year - 1)
            //{
            //    LtServerMessage.Text = WebUtil.WarningMessage("สมาชิกรายนี้ขอไปแล้ว");
            //    return;
            //}
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
            try
            {
                married_member = DwMain.GetItemString(1, "married_member");
            }
            catch { married_member = ""; }
        }

        private void FilterScholarship()
        {
            level_School = DwDetail.GetItemDecimal(1, "level_School");
            Session["level_School"] = level_School;

            DwUtil.RetrieveDDDW(DwDetail, "scholarship_level", "as_capital.pbl", null);
            DataWindowChild Dc = DwDetail.GetChild("scholarship_level");
            Dc.SetFilter("school_group =" + level_School + "");
            Dc.Filter();

            GetMoney();
        }

        private Decimal CheckReq(String member_no)
        {
            Decimal real_seq, sum_real_seq = 0;

            capital_year = DwMain.GetItemDecimal(1, "capital_year");

            Ds.LibraryList = "C:/GCOOP_ALL/CAT/GCOOP/Saving/DataWindow/app_assist/as_capital.pbl";
            Ds.DataWindowObject = "d_member_name_school";

            Ds.SetTransaction(sqlca);
            Ds.Retrieve(member_no, capital_year - 1);

            for (int i = 1; i <= Ds.RowCount; i++)
            {
                real_seq = Ds.GetItemDecimal(i, "real_seq");
                sum_real_seq = sum_real_seq + real_seq;
            }
            return sum_real_seq;
        }

        private Decimal GetSeqNo(Decimal level_school)
        {
            try
            {
                sqlStr = @"SELECT max(seq_no) as seq_no
                           FROM   asnreqschooldet
                           WHERE  capital_year= '" + capital_year + @"' and
                                  level_school= '" + level_school + "'";
                dt = ta.Query(sqlStr);
                dt.Next();

                try
                {
                    seq_no = dt.GetDecimal("seq_no");
                }
                catch { seq_no = 0; }

                seq_no = seq_no + 1;
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
            return seq_no;
        }

        private String GetLastDocNo(Decimal capital_year)
        {
            try
            {
                sqlStr = @"SELECT last_docno 
                               FROM   asndoccontrol
                               WHERE  doc_prefix = 'AS' and
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

                    assist_docno = "AS" + assist_docno;
                    assist_docno = assist_docno.Trim();
                }
                catch
                {
                    assist_docno = "AS000001";
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }

            return assist_docno;
        }

        private Int16 CheckMarried(String member_no)
        {
            Int32 row;
            Int16 same_married = 1;

            sqlStr = @"SELECT *
                       FROM   asnreqmaster
                       WHERE  married_member = '" + member_no + @"' and
                              assisttype_code = 10";
            dt = ta.Query(sqlStr);
            dt.Next();

            row = dt.GetRowCount();
            if (row > 0)
            {
                same_married = 0;
            }

            return same_married;
        }

        private Int16 CheckNameSon(String member_no, Decimal capital_year, String son_name, String son_surname)
        {
            String name, surname;
            Int16 same_flag = 1;

            Ds.LibraryList = "C:/GCOOP_ALL/CAT/GCOOP/Saving/DataWindow/app_assist/as_capital.pbl";
            Ds.DataWindowObject = "d_member_name_school";

            Ds.SetTransaction(sqlca);
            Ds.Retrieve(member_no, capital_year);

            for (int i = 1; i <= Ds.RowCount; i++)
            {
                name = Ds.GetItemString(i, "child_name").Trim();
                surname = Ds.GetItemString(i, "child_surname").Trim();

                if ((son_name == name) && (son_surname == surname))
                {
                    same_flag = 0;
                    level_School_same = Ds.GetItemDecimal(i, "level_School");
                    seq_no_same = Ds.GetItemDecimal(i, "seq_no");
                }
                else
                {
                    same_flag = 1;
                }
            }

            return same_flag;
        }


    }
}
