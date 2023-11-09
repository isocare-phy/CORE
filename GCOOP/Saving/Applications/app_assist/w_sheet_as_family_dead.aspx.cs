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
using System.Threading;

namespace Saving.Applications.app_assist
{
    public partial class w_sheet_as_family_dead : PageWebSheet, WebSheet
    {
        protected String app;
        protected String gid;
        protected String rid;
        protected String pdf;
        protected String postPopupReport;

        protected DwThDate tDwMain;
        protected DwThDate tDwDetail;

        protected String postToFromAccid;
        protected String postDepptacount;
        protected String postRefresh;
        protected String postChangeHeight;
        protected String postChangeType;
        protected String postChangeAmt;
        protected String postRetreiveDwMem;
        protected String postRetrieveDwMain;
        protected String postGetMemberDetail;
        protected String postRetrieveBankBranch;
        protected String postDelete;
        protected String postChangeAge;
        protected String jsformatIDCard;
        protected String jsformatDeptaccid;
        protected string postSetBranch;
        protected String postMateName;
        private String sqlStr, envvalue;
        private String member_no, assist_docno, membgroup_code, assisttype_code, deptaccount_no, card_person;
        private String expense_bank, expense_branch, expense_accid, moneytype_code, remark_cancel;
        private Decimal capital_year, salary_amt, assist_amt, req_status, pay_status, rowcount, family_type;
        private DateTime pay_date, cancel_date = DateTime.Now, center_date, req_date, birth_date, member_age, member_date;
        private Nullable<Decimal> cancel_id;
        private Double seq_pay, seq_pay_family;
        private String pbl = "as_capital.pbl";

        TimeSpan tp;

        protected Sta ta;
        protected Sdt dt;

        public void InitJsPostBack()
        {
            tDwMain = new DwThDate(DwMain, this);
            tDwDetail = new DwThDate(DwDetail, this);

            tDwMain.Add("pay_date", "pay_tdate");
            tDwMain.Add("req_date", "req_tdate");
            tDwMain.Add("entry_date", "entry_tdate");
            tDwMain.Add("member_date", "member_tdate");
            tDwMain.Add("approve_date", "approve_tdate");

            tDwDetail.Add("marriage_dead_date", "marriage_dead_tdate");
            tDwDetail.Add("family_dead_date", "family_dead_tdate");

            postRefresh = WebUtil.JsPostBack(this, "postRefresh");
            postChangeAmt = WebUtil.JsPostBack(this, "postChangeAmt");
            postChangeType = WebUtil.JsPostBack(this, "postChangeType");
            postChangeHeight = WebUtil.JsPostBack(this, "postChangeHeight");
            postRetreiveDwMem = WebUtil.JsPostBack(this, "postRetreiveDwMem");
            postRetrieveDwMain = WebUtil.JsPostBack(this, "postRetrieveDwMain");
            postGetMemberDetail = WebUtil.JsPostBack(this, "postGetMemberDetail");
            postRetrieveBankBranch = WebUtil.JsPostBack(this, "postRetrieveBankBranch");
            jsformatDeptaccid = WebUtil.JsPostBack(this, "jsformatDeptaccid");
            postDelete = WebUtil.JsPostBack(this, "postDelete");
            postChangeAge = WebUtil.JsPostBack(this, "postChangeAge");
            postPopupReport = WebUtil.JsPostBack(this, "postPopupReport");
            postDepptacount = WebUtil.JsPostBack(this, "postDepptacount");
            postToFromAccid = WebUtil.JsPostBack(this, "postToFromAccid");
            jsformatIDCard = WebUtil.JsPostBack(this, "jsformatIDCard");
            postSetBranch = WebUtil.JsPostBack(this, "postSetBranch");
            postMateName = WebUtil.JsPostBack(this, "postMateName");
        }

        public void WebSheetLoadBegin()
        {

            tDwMain.Eng2ThaiAllRow();
            tDwDetail.Eng2ThaiAllRow();

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


            tDwMain.Eng2ThaiAllRow();

            //    DwMain.SetItemString(1, "assisttype_code", "40");
            //    DwUtil.RetrieveDDDW(DwMain, "assisttype_code", "as_capital.pbl", null);
            //    DwUtil.RetrieveDDDW(DwMain, "capital_year", "as_capital.pbl", null);
            //    DwUtil.RetrieveDDDW(DwMain, "expense_bank", "as_capital.pbl", null);
            //    DwUtil.RetrieveDDDW(DwMain, "cancel_id", "as_capital.pbl", null);
            //    DwUtil.RetrieveDDDW(DwMain, "req_status", "as_capital.pbl", null);
            //    DwUtil.RetrieveDDDW(DwMain, "paytype_code", "as_capital.pbl", null);
            //    DwUtil.RetrieveDDDW(DwMain, "pay_status", "as_capital.pbl", null);
            //    DwUtil.RetrieveDDDW(DwMain, "birth_date", "as_capital.pbl", null);
            //    DwUtil.RetrieveDDDW(DwDetail, "member_age", "as_capital.pbl", null);

            DwMain.SetItemString(1, "assisttype_code", "40");
            DwUtil.RetrieveDDDW(DwMain, "assisttype_code", "as_capital.pbl", state.SsCoopId);
            DwUtil.RetrieveDDDW(DwMain, "capital_year", "as_capital.pbl", null);
            try { capital_year = DwMain.GetItemDecimal(1, "capital_year"); }
            catch { capital_year = state.SsWorkDate.Year + 543; }
            DwMain.SetItemDecimal(1, "capital_year", capital_year);
            //GetLastDocNo(capital_year);
            //DwMain.SetItemString(1, "assist_docno", assist_docno);

            DwUtil.RetrieveDDDW(DwMain, "expense_bank", "as_capital.pbl", null);
            DwUtil.RetrieveDDDW(DwMain, "cancel_id", "as_capital.pbl", null);
            DwUtil.RetrieveDDDW(DwMain, "req_status", "as_capital.pbl", null);
            //DwUtil.RetrieveDDDW(DwMain, "moneytype_code", "as_capital.pbl", null);
            DwUtil.RetrieveDDDW(DwDetail, "moneytype_code", "as_capital.pbl", null);
            DwUtil.RetrieveDDDW(DwMain, "pay_status", "as_capital.pbl", null);
            //DwUtil.RetrieveDDDW(DwMain, "birth_date", "as_capital.pbl", null);
            DwUtil.RetrieveDDDW(DwMain, "capital_year", "as_capital.pbl", null);
            //DwUtil.RetrieveDDDW(DwDetail, "member_age", "as_capital.pbl", null);
            //DwUtil.RetrieveDDDW(DwDetail, "marriage_name", "as_capital.pbl", null);
            //DwUtil.RetrieveDDDW(DwDetail, "family_age_1", "as_capital.pbl", null);
            //DwUtil.RetrieveDDDW(DwMain, "assist_docno", "as_capital", null);
            DwUtil.RetrieveDDDW(DwDetail, "expense_branch", "as_capital.pbl", null);
            DwMain.Describe("t_36.Visible");
            DwMain.Modify("t_36.Visible=false");
            DwMain.Describe("member_dead_tdate.Visible");
            DwMain.Modify("member_dead_tdate.Visible=false");
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postRetreiveDwMem")
            {
                RetreiveDwMem();
            }
            else if (eventArg == "postChangeAmt")
            {
                SetItemDwDetailMembDead();
                ChangeAmt();
                GetReqAge();
            }
            else if (eventArg == "postRefresh")
            {
                Refresh();
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
            else if (eventArg == "postGetMoney")
            {
                ChangeAmt();
                GetReqAge();
            }
            else if (eventArg == "postGetMemberDetail")
            {
                GetMemberDetail();
            }
            else if (eventArg == "postChangeType")
            {
                ChangeType();
            }
            else if (eventArg == "postDelete")
            {
                Delete();
            }
            else if (eventArg == "postChangeAge")
            {
                ChangeAge();
            }
            else if (eventArg == "postPopupReport")
            {
                PopupReport();
            }
            else if (eventArg == "postDepptacount")
            {
                PostDepptacount();
            }
            else if (eventArg == "postToFromAccid")
            {
                PostToFromAccid();
            }
            else if (eventArg == "jsformatDeptaccid")
            {
                string deptaccid = "";
                //deptaccid = DwMain.GetItemString(1, "deptaccount_no");
                deptaccid = DwDetail.GetItemString(1, "deptaccount_no");
                deptaccid = formatDeptaccid(deptaccid);
                //DwMain.SetItemString(1, "deptaccount_no", deptaccid);
                DwDetail.SetItemString(1, "deptaccount_no", deptaccid);
            }
            else if (eventArg == "jsformatIDCard")
            {
                card_person = DwDetail.GetItemString(1, "card_person");
                card_person = formatIDCard(card_person);
                DwDetail.SetItemString(1, "card_person", card_person);
                CheckIDCard();
            }
            else if (eventArg == "postSetBranch")
            {
                SetBankbranch();
            }
            else if (eventArg == "postMateName") { SetMateName(); }
        }

        protected void SetMateName()
        {
            //คู่สมรส : mate_name
            String mate_name = "", mate_cardperson = "", mate_salaryid = "";
            try { mate_name = DwMem.GetItemString(1, "mate_name"); }
            catch { }
            //เลขบัตรฯ คู่สมรส : mate_cardperson
            try { mate_cardperson = DwMem.GetItemString(1, "mate_cardperson").Trim(); }
            catch { }
            //เลขทะเบียนคู่สมรส: mate_salaryid
            try { mate_salaryid = DwMem.GetItemString(1, "mate_salaryid").Trim(); }
            catch { }
            DwMem.SetItemString(1, "mate_name", mate_name);
            mate_cardperson = formatIDCard(mate_cardperson);
            DwMem.SetItemString(1, "mate_cardperson", mate_cardperson);
            DwMem.SetItemString(1, "mate_salaryid", mate_salaryid);
        }

        protected void SetBankbranch()
        {
            string expense_branch = "";
            try { expense_branch = DwDetail.GetItemString(1, "expense_branch"); }
            catch { }
            DwMain.SetItemString(1, "expense_bank", "034");
            DwMain.SetItemString(1, "expense_branch", expense_branch);
        }

        public void Delete()
        {
            String sqlStr, assist_docno;
            Decimal capital_year;
            Sta ta2 = new Sta(sqlca.ConnectionString);

            assist_docno = DwMain.GetItemString(1, "assist_docno");
            capital_year = DwMain.GetItemDecimal(1, "capital_year");

            try
            {
                sqlStr = @"delete  from  asnmemsalary  
                       where assist_docno = '" + assist_docno + @"'  and
                             capital_year = '" + capital_year + "'";
                ta2.Exe(sqlStr);

                sqlStr = @"delete  from  ASNREQCAPITALDET 
                                       where assist_docno = '" + assist_docno + @"'  and
                                             capital_year = '" + capital_year + "'";
                ta2.Exe(sqlStr);

                sqlStr = @"delete  from  asnreqmaster
                       where assist_docno = '" + assist_docno + @"'  and
                             capital_year = '" + capital_year + "'";
                ta2.Exe(sqlStr);

                sqlStr = "delete from asnslippayout where payoutslip_no = '" + assist_docno + "' and capital_year = '" + capital_year + "'";
                ta2.Exe(sqlStr);

                LtServerMessage.Text = WebUtil.CompleteMessage("ลบข้อมูลเรียบร้อย");
                NewClear();
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถลบได้");
                //ta.RollBack();
            }
            ta2.Close();
        }

        public void SaveWebSheet()
        {
            Sta ta2 = new Sta(state.SsConnectionString);
            ta2.Transection();

            String marriage_name = "", family_name = "", remark, marriage_dead_tdate, family_dead_tdate;
            Decimal order_marriage = 0, family_age, family_type;
            DateTime family_dead_date, marriage_dead_date;
            DwMain.SetItemString(1, "coop_id", state.SsCoopId);
            Sta ta = new Sta(sqlca.ConnectionString);
            Sdt dt = new Sdt();

            setAccItemDwMain();

            unfomatAll();

            try
            {
                if (DwDetail.GetItemString(1, "card_person").Length != 13)
                {
                    //LtServerMessage.Text = WebUtil.WarningMessage("กรุณากรอกเลขบัตรประชาชนให้ครบ");
                    infomatAll();
                    Response.Write("<script language='javascript'>alert('กรุณากรอกเลขบัตรประชาชนให้ครบ'\n );</script>");
                    return;
                }
            }
            catch
            {
                //LtServerMessage.Text = WebUtil.WarningMessage("กรุณากรอกเลขบัตรประชาชนให้ครบ");
                infomatAll();
                Response.Write("<script language='javascript'>alert('กรุณาเลือก ความสัมพันธ์เกี่ยวข้องเป็น'\n );</script>");
                return;

            }

            GetItemDwMain();

            try
            {
                assist_amt = DwDetail.GetItemDecimal(1, "assist_amt");
            }
            catch
            {
                assist_amt = 0;
            }
            try
            {
                family_type = DwDetail.GetItemDecimal(1, "family_type");
            }
            catch
            {
                Response.Write("<script language='javascript'>alert('สมาชิกรายนี้ได้ทำรายการนี้แล้ว ไม่สามารถยื่นใบคำขอได้อีก'\n );</script>");
                return;
            }

            if (family_type == 0)
            {
                try
                {
                    order_marriage = DwDetail.GetItemDecimal(1, "order_marriage");
                }
                catch { order_marriage = 1; }
                marriage_name = DwDetail.GetItemString(1, "marriage_name");
                //family_age = DwDetail.GetItemDecimal(1, "family_age_1");
                //marriage_dead_date = DwDetail.GetItemDateTime(1, "marriage_dead_date");

                marriage_dead_tdate = DwDetail.GetItemString(1, "marriage_dead_tdate");
                marriage_dead_date = DateTime.ParseExact(marriage_dead_tdate, "ddMMyyyy", WebUtil.TH);
                DwDetail.SetItemDateTime(1, "marriage_dead_date", marriage_dead_date);
            }
            else
            {
                family_name = DwDetail.GetItemString(1, "family_name");
                //family_age = DwDetail.GetItemDecimal(1, "family_age");
                //family_dead_date = DwDetail.GetItemDateTime(1, "family_dead_date");

                family_dead_tdate = DwDetail.GetItemString(1, "family_dead_tdate");
                family_dead_date = DateTime.ParseExact(family_dead_tdate, "ddMMyyyy", WebUtil.TH);
                DwDetail.SetItemDateTime(1, "family_dead_date", family_dead_date);
            }

            try { expense_branch = DwDetail.GetItemString(1, "expense_branch").Trim(); }
            catch { }
            //เช็คว่าถ้าสาขาธนาคาร:  เป็นค่าว่าง ใช่หรือไม่ : ถ้าใช่ให้ Retrun  ออกจากฟังก์ชัน SaveWebSheet()
            if (expense_branch == null || expense_branch == "") { LtServerMessage.Text = WebUtil.WarningMessage("ป้อนข้อมูลไม่ครบถ้วน  กรุณาเลือกสาขาธนาคารด้วย"); return; }
            try { remark = DwDetail.GetItemString(1, "remark"); }
            catch { remark = ""; }
            try { assist_docno = DwMain.GetItemString(1, "assist_docno"); }
            catch { assist_docno = ""; }
            DateTime pay_date;
            if (assist_docno == "" || assist_docno == null)
            {
                decimal fam_type = DwDetail.GetItemDecimal(1, "family_type");
                Sdt dtcheck;
                string s_pay_date;
                if (fam_type == 3)
                {
                    string soncard_person = DwDetail.GetItemString(1, "card_person");
                    string checksondup = "select a.pay_date as pay_date from asnreqmaster a,asnreqcapitaldet b where a.assist_docno = b.assist_docno and a.capital_year = b.capital_year and a.assisttype_code = '40' and b.family_type in ('3') and b.card_person = '" + soncard_person + "'";
                    dtcheck = WebUtil.QuerySdt(checksondup);
                    if (dtcheck.Next())
                    {
                        pay_date = dtcheck.GetDate("pay_date");
                        s_pay_date = pay_date.ToString("dd/MM/") + (pay_date.Year + 543).ToString();
                        //LtServerMessage.Text = WebUtil.WarningMessage("หมายเลขบัตรประชาชน " + soncard_person + " ทำการขอสวัสดิการบุตรถึงแก่กรรมไปแล้ว");
                        infomatAll();
                        Response.Write("<script language='javascript'>alert('หมายเลขบัตรประชาชน " + soncard_person + " ทำการขอสวัสดิการถึงแก่กรรมไปแล้ว เมื่อวันที่ " + s_pay_date + "'\n );</script>");
                        return;
                    }
                }
                else if (fam_type == 1)
                {
                    string dadcard_person = DwDetail.GetItemString(1, "card_person");
                    string checkfatherdup = "select a.pay_date as pay_date from asnreqmaster a,asnreqcapitaldet b where a.assist_docno = b.assist_docno and a.capital_year = b.capital_year and a.assisttype_code = '40' and b.family_type in ('1') and b.card_person = '" + dadcard_person + "'";
                    dtcheck = WebUtil.QuerySdt(checkfatherdup);
                    if (dtcheck.Next())
                    {
                        pay_date = dtcheck.GetDate("pay_date");
                        s_pay_date = pay_date.ToString("dd/MM/") + (pay_date.Year + 543).ToString();
                        //LtServerMessage.Text = WebUtil.WarningMessage("หมายเลขบัตรประชาชน " + dadcard_person + " ได้ทำการขอทุนสวัสดิการบิดาถึงแก่กรรมไปแล้ว");
                        infomatAll();
                        Response.Write("<script language='javascript'>alert('หมายเลขบัตรประชาชน " + dadcard_person + " ทำการขอสวัสดิการถึงแก่กรรมไปแล้ว เมื่อวันที่ " + s_pay_date + "'\n );</script>");
                        return;
                    }
                }


                else if (fam_type == 2)
                {
                    string momcard_person = DwDetail.GetItemString(1, "card_person");
                    string checkfatherdup = "select a.pay_date as pay_date from asnreqmaster a,asnreqcapitaldet b where a.assist_docno = b.assist_docno and a.capital_year = b.capital_year and a.assisttype_code = '40' and b.family_type in ('2') and b.card_person = '" + momcard_person + "'";
                    dtcheck = WebUtil.QuerySdt(checkfatherdup);
                    if (dtcheck.Next())
                    {
                        pay_date = dtcheck.GetDate("pay_date");
                        s_pay_date = pay_date.ToString("dd/MM/") + (pay_date.Year + 543).ToString();
                        //LtServerMessage.Text = WebUtil.WarningMessage("หมายเลขบัตรประชาชน " + momcard_person + " ได้ทำการขอทุนสวัสดิการมารดาถึงแก่กรรมไปแล้ว");
                        infomatAll();
                        Response.Write("<script language='javascript'>alert('หมายเลขบัตรประชาชน " + momcard_person + " ทำการขอสวัสดิการถึงแก่กรรมไปแล้ว เมื่อวันที่ " + s_pay_date + "'\n );</script>");
                        return;
                    }
                }
            }
            //เพิ่มส่านการส่งค่า assist_docno
            //try
            //{
            //    assist_docno = DwMain.GetItemString(1, "assist_docno");
            //}
            //catch { assist_docno = ""; }


            if ((assist_docno == "" || assist_docno == null) && req_status == 8)
            {
                try { capital_year = DwMain.GetItemDecimal(1, "capital_year"); }
                catch { capital_year = 2555; }
                //capital_year = 2555;
                assist_docno = GetLastDocNo(capital_year);

                try { seq_pay = GetLastPayNo(capital_year); }
                catch { }

                try { seq_pay_family = GetLastMerPayNo(capital_year); }
                catch { }

                try
                {
                    DwMain.SetItemString(1, "member_no", member_no);
                    DwMain.SetItemDecimal(1, "assist_amt", assist_amt);
                    DwMain.SetItemString(1, "assist_docno", assist_docno);
                    DwMain.SetItemString(1, "remark", remark);

                    try
                    {
                        string strMemberAge = DwMain.GetItemString(1, "member_years_disp");
                        DwMain.SetItemString(1, "member_years_disp", "");
                        //DwUtil.InsertDataWindow(DwMain, "as_capital.pbl", "asnreqmaster");
                        InsertDwMain();
                        DwMain.SetItemString(1, "member_years_disp", strMemberAge);
                    }
                    catch { }

                    //capital_year = 2555;
                    DwDetail.SetItemString(1, "assist_docno", assist_docno);
                    DwDetail.SetItemDecimal(1, "capital_year", capital_year);
                    DwDetail.SetItemDateTime(1, "capital_date", state.SsWorkDate);

                    if (family_type == 1)
                    {
                        DwDetail.SetItemDouble(1, "seq_pay_family", seq_pay_family);
                    }
                    else
                    {
                        DwDetail.SetItemDouble(1, "seq_pay", seq_pay);
                    }

                    try
                    {
                        //DwUtil.InsertDataWindow(DwDetail, "as_capital.pbl", "asnreqcapitaldet");
                        InsertDwDetial();
                    }
                    catch { }


                    try
                    {
                        sqlStr = @"INSERT INTO asnmemsalary(capital_year  ,  member_no  ,  assist_docno  ,
                                                    salary_amt    ,  entry_date  ,  membgroup_code)
                                         VALUES('" + capital_year + "','" + member_no + "','" + assist_docno + @"',
                                                '" + salary_amt + "',to_date('" + DwMain.GetItemDateTime(1, "entry_date").ToString("dd/MM/yyyy", WebUtil.EN) + "', 'dd/mm/yyyy'),'" + membgroup_code + @"')";
                        ta.Exe(sqlStr);
                    }
                    catch (Exception ex) { ex.ToString(); }
                    try
                    {
                        //string last_docno = (Convert.ToDecimal(WebUtil.Right(assist_docno, 6))).ToString("000000");
                        sqlStr = @" UPDATE  asndoccontrol
                                    SET     last_docno   = '" + WebUtil.Right(assist_docno, 6) + @"'
                                    WHERE   doc_prefix   = 'AF' 
                                    AND     doc_year     = '" + capital_year + "'";
                        ta.Exe(sqlStr);
                    }
                    catch (Exception ex) { ex.ToString(); }

                    try
                    {
                        //asnslippayout
                        sqlStr = "select * from asnucfpaytype where moneytype_code = '" + moneytype_code + "'";
                        //string deptaccount_no;

                        try { deptaccount_no = DwMain.GetItemString(1, "deptaccount_no"); }
                        catch { deptaccount_no = ""; }

                        Sdt dtMoneytypecode = WebUtil.QuerySdt(sqlStr);
                        Sdt dtAmt = null;
                        string work_date = state.SsWorkDate.ToString("MM/dd/yyyy");
                        if (dtMoneytypecode.Next())
                        {
                            moneytype_code = dtMoneytypecode.Rows[0]["moneytype_code"].ToString().Trim();
                        }
                        //
                        decimal age_range = Decimal.Parse(DwMain.GetItemString(1, "age_range"));
                        decimal family_type_select = DwDetail.GetItemDecimal(1, "family_type");
                        if (family_type_select == 1 || family_type_select == 2 || family_type_select == 3 || family_type_select == 4)
                        {
                            sqlStr = "select * from asnsenvironmentvar where envgroup = 'management_bodies' and '" + age_range + "' between start_age and end_age and envcode like 'management_bodies_2%'";
                            dtAmt = WebUtil.QuerySdt(sqlStr);
                        }
                        //else if (family_type == 4)
                        //{
                        //    sqlStr = "select * from asnsenvironmentvar where envgroup = 'management_bodies' and '" + age_range + "' between start_age and end_age and envcode like 'management_bodies_1%'";
                        //    dtAmt = WebUtil.QuerySdt(sqlStr);
                        //}

                        // insert into asnslippayout
                        for (int i = 0; i < dtAmt.Rows.Count; i++)
                        {
                            int seq_no = i + 1;
                            sqlStr = @"INSERT INTO asnslippayout (payoutslip_no,member_no,slip_date,operate_date,
                                     payout_amt,slip_status,depaccount_no,assisttype_code,moneytype_code,entry_id,entry_date,seq_no,capital_year,sliptype_code,tofrom_accid)
                                     VALUES ('" + assist_docno + "','" + member_no + "',to_date('" + DwMain.GetItemDateTime(1, "req_date").ToString("dd/MM/yyyy", WebUtil.EN) +
                                    "', 'dd/mm/yyyy'),to_date('" + DwMain.GetItemDateTime(1, "entry_date").ToString("dd/MM/yyyy", WebUtil.EN) + "', 'dd/mm/yyyy'), '" +
                                    dtAmt.Rows[i]["envvalue"].ToString() + "','0','" + deptaccount_no + "','" + assisttype_code + "','" + moneytype_code + "','" + state.SsUsername +
                                    "',to_date('" + DwMain.GetItemDateTime(1, "entry_date").ToString("dd/MM/yyyy", WebUtil.EN) + "', 'dd/mm/yyyy')," + seq_no + ",'"
                                    + capital_year + "','" + dtAmt.Rows[i]["system_code"] + "','" + dtAmt.Rows[i]["tofrom_accid"] + "')";
                            ta2.Exe(sqlStr);
                        }
                        ta2.Commit();
                    }
                    catch
                    {
                        ta2.RollBack();
                    }

                    NewClear();
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกเรียบร้อย");
                }
                catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
            }
            else
            {
                DwMain.SetItemDecimal(1, "assist_amt", assist_amt);
                try
                {
                    sqlStr = @"UPDATE  asnmemsalary
                               SET     salary_amt    = '" + salary_amt + @"'
                               WHERE   assist_docno  = '" + assist_docno + "'";
                    ta.Exe(sqlStr);

                    try
                    {
                        string strMemberAge = DwMain.GetItemString(1, "member_years_disp");
                        DwMain.SetItemString(1, "member_years_disp", "");
                        //DwUtil.UpdateDataWindow(DwMain, "as_capital.pbl", "asnreqmaster");
                        UpdateDwMain();
                        DwMain.SetItemString(1, "member_years_disp", strMemberAge);
                    }
                    catch { }

                    seq_pay = GetLastPayNo(capital_year);
                    seq_pay_family = GetLastMerPayNo(capital_year);

                    if (pay_status == 1 && family_type == 1)
                    {

                        DwDetail.SetItemDouble(1, "seq_pay_family", seq_pay_family);
                    }
                    else
                    {
                        DwDetail.SetItemDouble(1, "seq_pay", seq_pay);
                    }

                    DwDetail.SetItemDecimal(1, "capital_year", capital_year);
                    DwDetail.SetItemString(1, "assist_docno", assist_docno);
                    try
                    {
                        //DwUtil.UpdateDataWindow(DwDetail, "as_capital.pbl", "asnreqcapitaldet");
                        UpdateDwDetial();
                    }
                    catch { }
                    try
                    {
                        sqlStr = @"UPDATE asnmemsalary 
                                    SET entry_date = to_date('" + DwMain.GetItemDateTime(1, "entry_date").ToString("dd/MM/yyyy", WebUtil.EN) + @"', 'dd/mm/yyyy') 
                                    where assist_docno = '" + assist_docno + @"' 
                                    and capital_year = '" + capital_year + @"'";
                        ta.Exe(sqlStr);
                    }
                    catch { }

                    try
                    {
                        sqlStr = "UPDATE asnmemsalary SET entry_date = to_date('" + DwMain.GetItemDateTime(1, "entry_date").ToString("dd/MM/yyyy", WebUtil.EN) + "', 'dd/mm/yyyy') where assist_docno = '" + assist_docno + "' and capital_year = '" + capital_year + "'";
                        ta.Exe(sqlStr);
                    }
                    catch { }

                    try
                    {
                        //asnslippayout
                        sqlStr = "select * from asnucfpaytype where moneytype_code = '" + moneytype_code + "'";
                        //string deptaccount_no;

                        try { deptaccount_no = DwMain.GetItemString(1, "deptaccount_no"); }
                        catch { deptaccount_no = ""; }

                        Sdt dtMoneytypecode = WebUtil.QuerySdt(sqlStr);
                        Sdt dtAmt = null;
                        string work_date = state.SsWorkDate.ToString("MM/dd/yyyy");
                        if (dtMoneytypecode.Next())
                        {
                            moneytype_code = dtMoneytypecode.Rows[0]["moneytype_code"].ToString().Trim();
                        }
                        //
                        decimal age_range = Decimal.Parse(DwMain.GetItemString(1, "age_range"));
                        decimal family_type_select = DwDetail.GetItemDecimal(1, "family_type");
                        if (family_type_select == 1 || family_type_select == 2 || family_type_select == 3 || family_type_select == 4)
                        {
                            sqlStr = "select * from asnsenvironmentvar where envgroup = 'management_bodies' and '" + age_range + "' between start_age and end_age and envcode like 'management_bodies_2%'";
                            dtAmt = WebUtil.QuerySdt(sqlStr);
                        }
                        //else if (family_type == 4)
                        //{
                        //    sqlStr = "select * from asnsenvironmentvar where envgroup = 'management_bodies' and '" + age_range + "' between start_age and end_age and envcode like 'management_bodies_1%'";
                        //    dtAmt = WebUtil.QuerySdt(sqlStr);
                        //}

                        // update asnslippayout
                        for (int i = 0; i < dtAmt.Rows.Count; i++)
                        {
                            int seq_no = i + 1;
                            sqlStr = @"UPDATE asnslippayout SET slip_date = to_date('" + DwMain.GetItemDateTime(1, "req_date").ToString("dd/MM/yyyy", WebUtil.EN) + "', 'dd/mm/yyyy'),operate_date = to_date('" + DwMain.GetItemDateTime(1, "entry_date").ToString("dd/MM/yyyy", WebUtil.EN) + "', 'dd/mm/yyyy'),payout_amt = '" + dtAmt.Rows[i]["envvalue"].ToString() + "',depaccount_no = '" + deptaccount_no + "',moneytype_code = '" + moneytype_code + "' ,entry_id = '" + state.SsUsername + "',entry_date = to_date('" + DwMain.GetItemDateTime(1, "entry_date").ToString("dd/MM/yyyy", WebUtil.EN) + "', 'dd/mm/yyyy') where payoutslip_no = '" + assist_docno + "' and capital_year = '" + capital_year + "' and seq_no = " + seq_no + "";
                            ta2.Exe(sqlStr);
                        }
                        ta2.Commit();
                    }
                    catch
                    {
                        ta2.RollBack();
                    }
                    NewClear();
                    LtServerMessage.Text = WebUtil.CompleteMessage("แก้ไขเรียบร้อย");
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            }
            ta2.Close();
        }

        public void WebSheetLoadEnd()
        {
            //try
            //{
            //    String membNo = DwUtil.GetString(DwMem, 1, "member_no", "");
            //    if (membNo != "")
            //    {
            //        DwUtil.RetrieveDDDW(DwMain, "deptaccount_no", "as_capital.pbl", membNo);
            //        DataWindowChild dc = DwMain.GetChild("deptaccount_no");
            //        String accNo = DwUtil.GetString(DwMain, 1, "deptaccount_no", "").Trim();
            //        if (dc.RowCount > 0 && accNo == "")
            //        {
            //            DwMain.SetItemString(1, "deptaccount_no", dc.GetItemString(1, "deptaccount_no"));
            //        }
            //    }
            //}
            //catch { }

            tDwMain.Eng2ThaiAllRow();
            tDwDetail.Eng2ThaiAllRow();


            dt.Clear();
            ta.Close();

            DwMem.SaveDataCache();
            DwMain.SaveDataCache();
            DwDetail.SaveDataCache();

        }



        private void Refresh()
        {

        }
        private void ChangeAmt()
        {
            String subdamagetype_code = "";
            Decimal age_range = 0, assist_amt = 0;

            try { age_range = Decimal.Parse(DwMain.GetItemString(1, "age_range")); }
            catch { }
            try { family_type = DwDetail.GetItemDecimal(1, "family_type"); }
            catch { }
            #region Old Code 20/09/2556
            //if (family_type == 1 || family_type == 2 || family_type == 3)
            //{
            //    Sta ta1 = new Sta(sqlca.ConnectionString);
            //    String sqlStr1 = "select * from asnsenvironmentvar where envgroup = 'management_bodies' and '" + age_range + "' between start_age and end_age and envcode like 'management_bodies_2%'";
            //    Sdt dt1 = ta1.Query(sqlStr1);
            //    if (dt1.Next())
            //    {
            //        String a = dt1.GetString("envvalue");
            //        assist_amt = Convert.ToDecimal(a);
            //        DwDetail.SetItemDecimal(1, "assist_amt", assist_amt);
            //    }
            //}
            //else if (family_type == 4)
            //{
            //    Sta ta1 = new Sta(sqlca.ConnectionString);
            //    String sqlStr1 = "select * from asnsenvironmentvar where envgroup = 'management_bodies' and '" + age_range + "' between start_age and end_age and envcode like 'management_bodies_1%'";
            //    Sdt dt1 = ta1.Query(sqlStr1);
            //    if (dt1.Next())
            //    {
            //        String a = dt1.GetString("envvalue");
            //        assist_amt = Convert.ToDecimal(a);
            //        DwDetail.SetItemDecimal(1, "assist_amt", assist_amt);
            //    }
            //}
            #endregion
            subdamagetype_code = "management_bodies_2_13";//HC
            Sta ta1 = new Sta(sqlca.ConnectionString);
            String sqlStr1 = @" select * 
                                from asnsenvironmentvar 
                                where envgroup = 'management_bodies' 
                                and '" + age_range + @"' between start_age and end_age 
                                and envcode = '" + subdamagetype_code + "'";
            Sdt dt1 = ta1.Query(sqlStr1);
            if (dt1.Next())
            {
                String a = dt1.GetString("envvalue");
                assist_amt = Convert.ToDecimal(a);
                DwDetail.SetItemDecimal(1, "assist_amt", assist_amt);
            }
            try
            {
                String family_dead_tdate;
                DateTime family_dead_date;

                family_dead_tdate = DwDetail.GetItemString(1, "family_dead_tdate");
                family_dead_date = DateTime.ParseExact(family_dead_tdate, "ddMMyyyy", WebUtil.TH);

                DwDetail.SetItemDateTime(1, "family_dead_date", family_dead_date);
            }
            catch (Exception ex) { ex.ToString(); }

        }

        private Decimal CheckReq(String member_no)
        {
            try
            {
                sqlStr = @"SELECT *
                           FROM   asnreqmaster
                           WHERE  member_no = '" + member_no + @"' and
                                  assist_docno like 'AF%'";

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
            DwUtil.RetrieveDataWindow(DwMem, "as_capital.pbl", tDwDetail, args);
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
            try
            {
                DwDetail.SetItemString(1, "assist_docno", assist_docno);
                DwDetail.SetItemDecimal(1, "capital_year", capital_year);
            }
            catch { }

            object[] args1 = new object[1];
            args1[0] = HfMemNo.Value;

            DwMem.Reset();
            DwUtil.RetrieveDataWindow(DwMem, "as_capital.pbl", null, args1);
            try { card_person = DwMem.GetItemString(1, "card_person"); }
            catch { card_person = ""; }
            card_person = formatIDCard(card_person);
            DwMem.SetItemString(1, "card_person", card_person);

            object[] args2 = new object[3];
            args2[0] = assist_docno;
            args2[1] = capital_year;
            args2[2] = member_no;

            DwMain.Reset();
            //DwMain.Retrieve(args2[0], args2[1], args2[2]);
            //DwUtil.RetrieveDataWindow(DwMain, "as_capital.pbl", tDwMain, args2);
            DwUtil.RetrieveDataWindow(DwMain, "as_capital.pbl", tDwMain, args2);
            try { deptaccount_no = DwMain.GetItemString(1, "deptaccount_no"); }
            catch { deptaccount_no = ""; }
            deptaccount_no = formatDeptaccid(deptaccount_no);
            DwMain.SetItemString(1, "deptaccount_no", deptaccount_no);

            object[] args3 = new object[2];
            args3[0] = assist_docno;
            args3[1] = capital_year;

            DwDetail.Reset();
            DwUtil.RetrieveDataWindow(DwDetail, "as_capital.pbl", tDwDetail, args3);
            try { card_person = DwDetail.GetItemString(1, "card_person"); }
            catch { card_person = ""; }
            card_person = formatIDCard(card_person);
            DwDetail.SetItemString(1, "card_person", card_person);

            SetItemDwMainMembDead();
            RetrieveBankBranch();
            setAccItemDwDetail();
            //เซตค่าให้กับ สาขา : expense_branch
            try { expense_branch = DwMain.GetItemString(1, "expense_branch"); }
            catch { }
            DwMain.SetItemString(1, "expense_branch", expense_branch);
            DwDetail.SetItemString(1, "expense_branch", expense_branch);

            decimal member_age;
            int[] member_year = new int[3];

            req_date = DwMain.GetItemDateTime(1, "req_date");
            member_date = DwMain.GetItemDateTime(1, "member_date");
            birth_date = DwMain.GetItemDateTime(1, "birth_date");
            member_year = clsCalAge(member_date, req_date);

            string member_age_range = member_year[2].ToString() + "." + member_year[1].ToString("00") + member_year[0].ToString("00");
            string s_member_year = member_year[2].ToString() + " ปี " + member_year[1].ToString() + " เดือน " + member_year[0].ToString() + " วัน";

            DwMain.SetItemString(1, "member_years_disp", s_member_year);
            member_age = wcf.Busscom.of_cal_yearmonth(state.SsWsPass, birth_date, req_date);
            string s_member_age = member_age.ToString() + " ปี";
            DwMem.SetItemString(1, "member_age", s_member_age);
            DwMain.SetItemString(1, "age_range", member_age_range);
        }

        private void GetMemberDetail()
        {
            String member_no, prename_desc, memb_name, memb_surname, membgroup_code, membgroup_desc, membtype_code, membtype_desc, age, card_person, mate_name, mate_salaryid;
            DateTime member_date, birth_date;
            Decimal salary_amount, member_age;
            member_no = HfMemberNo.Value;

            int[] member_year = new int[3];

            DataStore Ds = new DataStore();
            Ds.LibraryList = "C:/GCOOP_ALL/CORE/GCOOP/Saving/DataWindow/app_assist/as_capital.pbl";
            Ds.DataWindowObject = "d_member_detail";
            Ds.SetTransaction(sqlca);
            Ds.Retrieve(member_no);

            if (Ds.RowCount < 1)
            {
                LtAlert.Text = "<script>Alert()</script>";
                return;
            }
            //int check = CheckMemberDuration(member_no);
            //if (check == 0)
            //{
            //    LtServerMessage.Text = WebUtil.WarningMessage("สมาชิกรายนี้ยังเป็นสมาชิกไม่ถึง 1 ปี");
            //    //return;
            //}

            prename_desc = Ds.GetItemString(1, "prename_desc");
            memb_name = Ds.GetItemString(1, "memb_name");
            memb_surname = Ds.GetItemString(1, "memb_surname");
            membgroup_code = Ds.GetItemString(1, "membgroup_code");
            membgroup_desc = Ds.GetItemString(1, "membgroup_desc");
            member_date = Ds.GetItemDateTime(1, "member_date");
            membtype_code = Ds.GetItemString(1, "membtype_code");
            membtype_desc = Ds.GetItemString(1, "membtype_desc");
            birth_date = Ds.GetItemDateTime(1, "birth_date");
            salary_amount = Ds.GetItemDecimal(1, "salary_amount");
            card_person = Ds.GetItemString(1, "card_person");
            try { mate_name = Ds.GetItemString(1, "mate_name"); }
            catch { mate_name = ""; }
            try { mate_salaryid = Ds.GetItemString(1, "mate_salaryid"); }
            catch { mate_salaryid = ""; }

            member_year = clsCalAge(member_date, state.SsWorkDate);
            string member_age_range = member_year[2].ToString() + "." + member_year[1].ToString("00") + member_year[0].ToString("00");
            string s_member_year = member_year[2].ToString() + " ปี " + member_year[1].ToString() + " เดือน " + member_year[0].ToString() + " วัน";

            DwMain.SetItemString(1, "member_years_disp", s_member_year);
            member_age = wcf.Busscom.of_cal_yearmonth(state.SsWsPass, birth_date, state.SsWorkDate);
            string s_member_age = member_age.ToString() + " ปี";
            DwMem.SetItemString(1, "member_age", s_member_age);
            //
            DwMain.SetItemString(1, "age_range", member_age_range);

            DwMem.SetItemString(1, "member_no", member_no);
            DwMem.SetItemString(1, "prename_desc", prename_desc);
            DwMem.SetItemString(1, "memb_name", memb_name);
            DwMem.SetItemString(1, "memb_surname", memb_surname);
            DwMem.SetItemString(1, "membgroup_desc", membgroup_desc);
            DwMem.SetItemString(1, "membgroup_code", membgroup_code);
            DwMem.SetItemString(1, "membtype_code", membtype_code);
            DwMem.SetItemString(1, "membtype_desc", membtype_desc);
            card_person = formatIDCard(card_person);
            DwMem.SetItemString(1, "card_person", card_person);
            DwMem.SetItemString(1, "mate_name", mate_name);
            DwMem.SetItemString(1, "mate_salaryid", mate_salaryid);
            DwMain.SetItemDateTime(1, "member_date", member_date);
            DwMain.SetItemDateTime(1, "birth_date", birth_date);
            DwMain.SetItemDecimal(1, "salary_amt", salary_amount);
            //try { DwUtil.RetrieveDDDW(DwMain, "deptaccount_no", "as_capital.pbl", member_no); }
            //catch (Exception ex) { ex.ToString(); }
            tDwMain.Eng2ThaiAllRow();
            tDwMain.Eng2ThaiAllRow();
            //rowcount = CheckReq(member_no);
            //if (rowcount > 0)
            //{
            //    LtServerMessage.Text = WebUtil.WarningMessage("สมาชิกรายนี้ขอทุนไปแล้ว");
            //    return;//เดิม comment ไว้
            //}
            ChangeAmt();
            SetDefaultAccid();

            deptaccount_no = SetdefaultDeptacc(member_no, state.SsCoopId);
            deptaccount_no = formatDeptaccid(deptaccount_no);
            DwMain.SetItemString(1, "deptaccount_no", deptaccount_no);
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

        private void ChangeType()
        {
            family_type = DwDetail.GetItemDecimal(1, "family_type");

            if (family_type == 1)
            {

            }
            else
            {

            }
        }


        private void NewClear()
        {
            DwMem.Reset();
            DwMain.Reset();
            DwDetail.Reset();

            DwMem.InsertRow(0);
            DwMain.InsertRow(0);
            DwDetail.InsertRow(0);
            DwMain.SetItemDecimal(1, "req_status", 8);
            DwMain.SetItemDateTime(1, "pay_date", state.SsWorkDate);
            DwMain.SetItemDateTime(1, "req_date", state.SsWorkDate);
            DwMain.SetItemDateTime(1, "entry_date", state.SsWorkDate);
            DwMain.SetItemDateTime(1, "approve_date", state.SsWorkDate);
            DwDetail.SetItemDateTime(1, "family_dead_date", state.SsWorkDate);
            SetItemDwMainMembDead();
            SetDefaultAccid();
        }

        private void GetItemDwMain()
        {

            member_no = DwMem.GetItemString(1, "member_no");
            membgroup_code = DwMem.GetItemString(1, "membgroup_code");
            req_status = DwMain.GetItemDecimal(1, "req_status");
            capital_year = DwMain.GetItemDecimal(1, "capital_year");
            assisttype_code = DwMain.GetItemString(1, "assisttype_code");
            try
            {
                birth_date = DwMain.GetItemDateTime(1, "birth_date");
            }
            catch { }
            try
            {
                salary_amt = DwMain.GetItemDecimal(1, "salary_amt");
            }
            catch { salary_amt = 0; }

            try
            {
                moneytype_code = DwMain.GetItemString(1, "moneytype_code");
            }
            catch { moneytype_code = ""; }
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
                pay_status = DwMain.GetItemDecimal(1, "pay_status");
            }
            catch { pay_status = 0; }

        }

/*        private String GetLastDocNo(Decimal capital_year)
        {
            Sta ta = new Sta(sqlca.ConnectionString);
            Sdt dt = new Sdt();

            try
            {
                sqlStr = @"SELECT last_docno 
                               FROM   asndoccontrol
                               WHERE  doc_prefix = 'AF' and
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

                    assist_docno = "AF" + assist_docno;
                    assist_docno = assist_docno.Trim();
                }
                catch
                {
                    assist_docno = "AF000001";
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }

            return assist_docno;
        }
*/
        private String GetLastDocNo(Decimal capital_year)
        {
            String doc_year = "";
            try
            {
                doc_year = WebUtil.Right(capital_year.ToString(), 2);
                sqlStr = @" SELECT substr( doc_year, 3, 2 ) || last_docno as last_docno 
                            FROM   asndoccontrol
                            WHERE  doc_prefix   = 'AF' 
                            and    doc_year     = '" + capital_year + "'";
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
                        assist_docno = WebUtil.Right(assist_docno, 8);
                    }
                    assist_docno = "AF" + assist_docno;
                    assist_docno = assist_docno.Trim();
                }
                catch
                {
                    assist_docno = "AF" + doc_year + "000001";
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }

            return assist_docno;
        }

        private Double GetLastMerPayNo(Decimal capital_year)
        {
            Sta ta = new Sta(sqlca.ConnectionString);
            Sdt dt = new Sdt();

            try
            {
                sqlStr = @"SELECT max(seq_pay_family) as seq_pay_family 
                               FROM   asnreqcapitaldet
                               WHERE  capital_year = '" + capital_year + @"' and
                                      assist_docno like 'AF%'";
                dt = ta.Query(sqlStr);
                dt.Next();

                try
                {
                    try
                    {
                        seq_pay_family = dt.GetDouble("seq_pay_family");
                    }
                    catch { seq_pay_family = 0; }

                    //if (seq_pay_family == 0)
                    //{
                    //    seq_pay_family = 1;
                    //}

                    seq_pay_family = seq_pay_family + 1;
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }

            return seq_pay_family;
        }

        private Double GetLastPayNo(Decimal capital_year)
        {
            Sta ta = new Sta(sqlca.ConnectionString);
            Sdt dt = new Sdt();

            try
            {
                sqlStr = @"SELECT max(seq_pay) as seq_pay 
                               FROM   asnreqcapitaldet
                               WHERE  capital_year = '" + capital_year + @"' and
                                      assist_docno like 'AF%'";
                dt = ta.Query(sqlStr);
                dt.Next();

                try
                {
                    try
                    {
                        seq_pay = dt.GetDouble("seq_pay");
                    }
                    catch { seq_pay = 0; }

                    //if (seq_pay == 0)
                    //{
                    //    seq_pay = 1;
                    //}

                    seq_pay = seq_pay + 1;
                }
                catch (Exception ex)
                {
                    //seq_pay = 0;
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }

            return seq_pay;
        }

        private int CheckMemberDuration(string memberno)
        {
            int status = 1;
            string envvalue = "";
            DateTime center_date, member_date = DateTime.Today;
            string sql_notify = @"select envvalue
                           from asnsenvironmentvar
                           WHERE envcode = 'family_open_date'";
            Sdt dtMain = ta.Query(sql_notify);
            dtMain.Next();
            envvalue = dtMain.GetString("envvalue");
            center_date = DateTime.ParseExact(envvalue, "dd/MM/yyyy", WebUtil.TH);

            string member_no1 = DwMem.GetItemString(1, "member_no");
            string sqlmember_date = "select member_date from mbmembmaster where member_no = '" + memberno + "'";
            Sdt da = WebUtil.QuerySdt(sqlmember_date);
            if (da.Next())
            {
                member_date = Convert.ToDateTime(da.Rows[0]["member_date"].ToString());
            }
            TimeSpan diffTime = center_date.Subtract(member_date);
            int days = int.Parse(diffTime.Days.ToString());
            if (days < 365)
            {
                status = 0;
            }
            return status;

        }

        private void ChangeAge()
        {
            decimal member_age;
            int[] member_year = new int[3];
            DateTime Req_date;
            Req_date = DateTime.ParseExact(HdReqDate.Value, "ddMMyyyy", WebUtil.TH);
            DwMain.SetItemDateTime(1, "req_date", Req_date);

            birth_date = DwMain.GetItemDateTime(1, "birth_date");
            member_date = DwMain.GetItemDateTime(1, "member_date");

            member_year = clsCalAge(member_date, Req_date);
            string member_age_range = member_year[2].ToString() + "." + member_year[1].ToString("00") + member_year[0].ToString("00"); ;
            string s_member_year = member_year[2].ToString() + " ปี " + member_year[1].ToString() + " เดือน " + member_year[0].ToString() + " วัน";

            DwMain.SetItemString(1, "member_years_disp", s_member_year);
            member_age = wcf.Busscom.of_cal_yearmonth(state.SsWsPass, birth_date, Req_date);
            string s_member_age = member_age.ToString() + " ปี";
            DwMem.SetItemString(1, "member_age", s_member_age);

            DwMain.SetItemString(1, "age_range", member_age_range);
        }
        public int[] clsCalAge(DateTime d1, DateTime d2)
        {
            DateTime fromDate;
            DateTime toDate;

            int intyear;
            int intmonth;
            int intday;
            int intCheckedDay = 0;
            int[] CalAge = new int[3];

            if (d1 > d2)
            {
                fromDate = d2;
                toDate = d1;
            }
            else
            {
                fromDate = d1;
                toDate = d2;
            }
            intCheckedDay = 0;
            if (fromDate.Day > toDate.Day)
            {
                intCheckedDay = DateTime.DaysInMonth(fromDate.Year, fromDate.Month);
            }
            if (intCheckedDay != 0)
            {
                intday = (toDate.Day + intCheckedDay) - fromDate.Day;
                intCheckedDay = 1;
            }
            else
            {
                intday = toDate.Day - fromDate.Day;
            }
            if ((fromDate.Month + intCheckedDay) > toDate.Month)
            {
                intmonth = (toDate.Month + 12) - (fromDate.Month + intCheckedDay);
                intCheckedDay = 1;
            }
            else
            {
                intmonth = (toDate.Month) - (fromDate.Month + intCheckedDay);
                intCheckedDay = 0;
            }
            intyear = toDate.Year - (fromDate.Year + intCheckedDay);

            CalAge[0] = intday;
            CalAge[1] = intmonth;
            CalAge[2] = intyear;
            return CalAge;

        }
        private void RunProcess()
        {
            app = state.SsApplication;
            try
            {
                //gid = Request["gid"].ToString();
                gid = "assist_slip";
            }
            catch { }
            try
            {
                //rid = Request["rid"].ToString();
                rid = "ass_slip_master";
            }
            catch { }



            String assist_docno = DwMain.GetItemString(1, "assist_docno");
            decimal capital_year = DwMain.GetItemDecimal(1, "capital_year");
            //แปลง Criteria ให้อยู่ในรูปแบบมาตรฐาน.
            ReportHelper lnv_helper = new ReportHelper();
            lnv_helper.AddArgument(assist_docno, ArgumentType.String);
            lnv_helper.AddArgument(capital_year.ToString(), ArgumentType.Number);

            //ชื่อไฟล์ PDF = YYYYMMDDHHMMSS_<GID>_<RID>.PDF

            String pdfFileName = DateTime.Now.ToString("yyyyMMddHHmmss", WebUtil.EN);
            pdfFileName += "_" + gid + "_" + rid + "_" + state.SsClientIp + ".pdf";
            pdfFileName = pdfFileName.Trim();

            //ส่งให้ ReportService สร้าง PDF ให้ {โดยปกติจะอยู่ใน C:\GCOOP\Saving\PDF\}.
            try
            {
                CoreSavingLibrary.WcfReport.ReportClient lws_report = wcf.Report;
                String criteriaXML = lnv_helper.PopArgumentsXML();
                this.pdf = lws_report.GetPDFURL(state.SsWsPass) + pdfFileName;
                String li_return = lws_report.RunWithID(state.SsWsPass, app, gid, rid, state.SsUsername, criteriaXML, pdfFileName);
                if (li_return == "true")
                {
                    HdOpenIFrame.Value = "True";
                }
                //else if (li_return != "true")
                //{
                //    HdcheckPdf.Value = "False";
                //}
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                return;
            }
            Session["pdf"] = pdf;
            //PopupReport();
        }

        public void PopupReport()
        {
            RunProcess();
            // Thread.Sleep(5000);
            Thread.Sleep(4500);

            //เด้ง Popup ออกรายงานเป็น PDF.
            String pop = "Gcoop.OpenPopup('" + pdf + "')";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "DsReport", pop, true);
        }

        private void PostDepptacount()
        {
            deptaccount_no = Hfdeptaccount_no.Value;
            DwMain.SetItemString(1, "deptaccount_no", deptaccount_no);
            //            try { member_no = DwMem.GetItemString(1, "member_no"); }
            //            catch { member_no = ""; }
            //            sqlStr = @" SELECT expense_accid 
            //                        FROM mbmembmaster
            //                        WHERE member_no = '" + member_no + @"'";
            //            dt = ta.Query(sqlStr);

            //            if (dt.Next())
            //            {
            //                expense_accid = dt.GetString("expense_accid");
            //            }
            //            DwMain.SetItemString(1, "deptaccount_no", expense_accid);

        }

        private void PostToFromAccid()
        {
            //try { moneytype_code = DwMain.GetItemString(1, "moneytype_code"); }
            //catch { moneytype_code = ""; }
            //DwUtil.RetrieveDDDW(DwMain, "tofrom_accid", "as_capital.pbl", moneytype_code);
            try { moneytype_code = DwDetail.GetItemString(1, "moneytype_code"); }
            catch { moneytype_code = ""; }
            DwUtil.RetrieveDDDW(DwDetail, "tofrom_accid", "as_capital.pbl", moneytype_code);
            if (moneytype_code == "CSH")
            {
                DwDetail.SetItemString(1, "tofrom_accid", "11010100");
                DwDetail.Describe("t_19.Visible");
                DwDetail.Modify("t_19.Visible=false");
                DwDetail.Describe("tofrom_accid.Visible");
                DwDetail.Modify("tofrom_accid.Visible=false");
                DwDetail.Describe("t_20.Visible");
                DwDetail.Modify("t_20.Visible=false");
                DwDetail.Describe("deptaccount_no.Visible");
                DwDetail.Modify("deptaccount_no.Visible=false");
            }
            //LEK ดึงค่าเริ่มต้นของคู่บัญชี กำหนดเงื่อนไขจากประเภทการจ่ายเงิน: moneytype_code  นำมาเซตค่าให้กับคู่บัญชี: tofrom_accid
            String tofrom_accid = "";
            sqlStr = @" SELECT 	CMUCFTOFROMACCID.ACCOUNT_ID
                        FROM 	CMUCFMONEYTYPE,   
			                    CMUCFTOFROMACCID,   
			                    ACCMASTER  
                        WHERE 	( CMUCFMONEYTYPE.MONEYTYPE_CODE = CMUCFTOFROMACCID.MONEYTYPE_CODE ) and  
			                    ( CMUCFTOFROMACCID.ACCOUNT_ID = ACCMASTER.ACCOUNT_ID ) and  
			                    ( CMUCFMONEYTYPE.MONEYTYPE_CODE = '" + moneytype_code + @"' ) AND
            		            ( CMUCFTOFROMACCID.SLIPTYPE_CODE = 'LWD' ) AND
            		            ( CMUCFTOFROMACCID.DEFAULTPAYOUT_FLAG = 1 ) ";
            dt = ta.Query(sqlStr);
            if (dt.Next())
            {
                tofrom_accid = dt.GetString("ACCOUNT_ID");
            }
            DwDetail.SetItemString(1, "tofrom_accid", tofrom_accid);
        }
        private void SetDefaultAccid()
        {
            //DwMain.SetItemString(1, "moneytype_code", "CBT");
            DwDetail.SetItemString(1, "moneytype_code", "CBT");
            PostToFromAccid();
            //DwMain.SetItemString(1, "tofrom_accid", "11035200");
            //DwDetail.SetItemString(1, "tofrom_accid", "11035200"); //LEK Comment เปลี่ยนไปเรียกใช้ฟังก์ชัน PostToFromAccid() เพื่อเซตค่าให้กับคู่บัญชี: tofrom_accid
            //LEK ดึงค่าเริ่มต้นของเลขที่บัญชีมาเซตค่าให้กับ Field = เลขที่บัญชี : deptaccount_no
            try { member_no = DwMem.GetItemString(1, "member_no"); }
            catch { member_no = ""; }
            sqlStr = @" SELECT  expense_accid 
                        FROM    mbmembmaster
                        WHERE   member_no = '" + member_no + @"'";
            dt = ta.Query(sqlStr);

            if (dt.Next())
            {
                expense_accid = dt.GetString("expense_accid");
            }
            DwDetail.SetItemString(1, "deptaccount_no", expense_accid);
        }

        public void GetReqAge()
        {
            DateTime fam_dead = state.SsWorkDate;
            int[] fam_dead_sum = new int[3];
            try
            {
                fam_dead = DwDetail.GetItemDateTime(1, "family_dead_date");
            }
            catch { fam_dead = state.SsWorkDate; }
            DateTime Req_date = new DateTime();
            try { Req_date = DateTime.ParseExact(HdReqDate.Value, "dd/MM/yyyy", WebUtil.TH); }
            catch { Req_date = state.SsWorkDate; }
            //fam_dead_sum = clsCalAge(fam_dead, state.SsWorkDate);
            fam_dead_sum = clsCalAge(fam_dead, Req_date);
            Hfsumdate.Value = Convert.ToString(fam_dead_sum[0] + (fam_dead_sum[1] * 30) + (fam_dead_sum[2] * 365));
            //เช็คว่าวันที่รับใบคำขอ :  ต้องไม่เกิน 365 วัน            
            if (Convert.ToDouble(Hfsumdate.Value) > 365.00) { LtServerMessage.Text = WebUtil.WarningMessage("ยื่นคำขอรับทุน  เกินกำหนด  365 วัน"); return; }
        }

        private string formatIDCard(string idcard)
        {
            string[] id = new string[5];
            idcard = idcard.Replace("-", "");
            if (idcard.Length == 13)
            {
                id[0] = idcard.Substring(0, 1);
                id[1] = idcard.Substring(1, 4);
                id[2] = idcard.Substring(5, 5);
                id[3] = idcard.Substring(10, 2);
                id[4] = idcard.Substring(12, 1);

                idcard = id[0] + "-" + id[1] + "-" + id[2] + "-" + id[3] + "-" + id[4];
            }
            return idcard;
        }
        private string unfomatIDCard(string idcard)
        {
            idcard = idcard.Replace("-", "");
            return idcard;
        }

        private string formatDeptaccid(string deptaccid)
        {
            string[] deptid = new string[5];
            deptaccid = deptaccid.Replace("-", "");
            deptaccid = deptaccid.Trim();
            if (deptaccid.Length == 12)
            {
                deptid[0] = deptaccid.Substring(0, 2);
                deptid[1] = deptaccid.Substring(2, 3);
                deptid[2] = deptaccid.Substring(5, 1);
                deptid[3] = deptaccid.Substring(6, 5);
                deptid[4] = deptaccid.Substring(11, 1);

                deptaccid = deptid[0] + "-" + deptid[1] + "-" + deptid[2] + "-" + deptid[3] + "-" + deptid[4];
            }
            return deptaccid;
        }
        private string unformatDeptaccid(string deptaccid)
        {
            deptaccid = deptaccid.Replace("-", "");
            return deptaccid;
        }

        private string SetdefaultDeptacc(string member_no, string coop_id)
        {
            sqlStr = @"SELECT expense_accid FROM mbmembmaster WHERE coop_id = '" + coop_id + @"' AND member_no = '" + member_no + @"'";
            dt = ta.Query(sqlStr);
            if (dt.Next())
            {
                expense_accid = dt.GetString("expense_accid");
            }
            else
            {
                expense_accid = "";
            }

            return expense_accid;
        }

        private void unfomatAll()
        {
            card_person = DwMem.GetItemString(1, "card_person");//หมายเลขบัตรประชาชน: สมาชิก
            card_person = unfomatIDCard(card_person);
            DwMem.SetItemString(1, "card_person", card_person);

            deptaccount_no = DwMain.GetItemString(1, "deptaccount_no");
            deptaccount_no = unformatDeptaccid(deptaccount_no);
            DwMain.SetItemString(1, "deptaccount_no", deptaccount_no);

            try { card_person = DwDetail.GetItemString(1, "card_person"); } //หมายเลขบัตรประชาชน: ผู้ถึงแก่กรรม
            catch { LtServerMessage.Text = WebUtil.WarningMessage("คีย์ข้อมูลไม่ครบถ้วน!"); return; }
            card_person = unformatDeptaccid(card_person);
            DwDetail.SetItemString(1, "card_person", card_person);
        }

        private void infomatAll()
        {
            card_person = DwMem.GetItemString(1, "card_person");
            card_person = formatIDCard(card_person);
            DwMem.SetItemString(1, "card_person", card_person);

            deptaccount_no = DwMain.GetItemString(1, "deptaccount_no");
            deptaccount_no = formatDeptaccid(deptaccount_no);
            DwMain.SetItemString(1, "deptaccount_no", deptaccount_no);

            card_person = DwDetail.GetItemString(1, "card_person");
            card_person = formatIDCard(card_person);
            DwDetail.SetItemString(1, "card_person", card_person);
        }

        private void setAccItemDwMain()
        {
            string moneytype_code               //การจ่ายเงิน
                   , tofrom_accid               //เข้าบัญชี
                   , deptaccount_no             //เลขที่บัญชี
                   ;
            #region GetItem
            try { moneytype_code = DwDetail.GetItemString(1, "moneytype_code"); }
            catch { moneytype_code = ""; }
            try { tofrom_accid = DwDetail.GetItemString(1, "tofrom_accid"); }
            catch { tofrom_accid = ""; }
            try { deptaccount_no = DwDetail.GetItemString(1, "deptaccount_no"); }
            catch { deptaccount_no = ""; }
            #endregion

            deptaccount_no = unformatDeptaccid(deptaccount_no);

            DwMain.SetItemString(1, "moneytype_code", moneytype_code);
            DwMain.SetItemString(1, "tofrom_accid", tofrom_accid);
            DwMain.SetItemString(1, "deptaccount_no", deptaccount_no);
        }

        private void setAccItemDwDetail()
        {
            string moneytype_code               //การจ่ายเงิน
                   , tofrom_accid               //เข้าบัญชี
                   , deptaccount_no             //เลขที่บัญชี
                   ;
            #region GetItem
            try { moneytype_code = DwMain.GetItemString(1, "moneytype_code"); }
            catch { moneytype_code = ""; }
            try { tofrom_accid = DwMain.GetItemString(1, "tofrom_accid"); }
            catch { tofrom_accid = ""; }
            try { deptaccount_no = DwMain.GetItemString(1, "deptaccount_no"); }
            catch { deptaccount_no = ""; }
            #endregion

            deptaccount_no = formatDeptaccid(deptaccount_no);

            DwDetail.SetItemString(1, "moneytype_code", moneytype_code);
            DwDetail.SetItemString(1, "tofrom_accid", tofrom_accid);
            DwDetail.SetItemString(1, "deptaccount_no", deptaccount_no);
        }

        /// <summary>
        /// MiW::2014.04.10
        /// Set วันที่ผู้เสียชีวิต ลงในฟิวด์ DwDetail เพื่อรอ Save
        /// </summary>
        private void SetItemDwDetailMembDead()
        {
            String date;
            int day, month, year;
            DateTime dt;

            date = DwMain.GetItemString(1, "member_dead_tdate");
            date = date.Replace("/", "");
            day = Convert.ToInt16(date.Substring(0, 2));
            month = Convert.ToInt16(date.Substring(2, 2));
            year = Convert.ToInt16(date.Substring(4, 4)) - 543;
            dt = new DateTime(year, month, day);
            date = day.ToString("00") + "/" + month.ToString("00") + "/" + (year + 543).ToString("0000");
            DwDetail.SetItemDateTime(1, "family_dead_date", dt);
            DwMain.SetItemString(1, "member_dead_tdate", date);
        }

        /// <summary>
        /// MiW::2014.04.10
        /// Set วันที่ผู้เสียชีวิต ลงในฟิวด์ DwMain กรณีเปิดใบคำขอเดิม
        /// </summary>
        private void SetItemDwMainMembDead()
        {
            String date;
            int day, month, year;
            DateTime dt;

            try { date = (DwDetail.GetItemDate(1, "family_dead_date")).ToString("dd/MM/yyyy", WebUtil.TH); }
            catch { date = (state.SsWorkDate).ToString("dd/MM/yyyy", WebUtil.TH); }
            date = date.Replace("/", "");
            day = Convert.ToInt16(date.Substring(0, 2));
            month = Convert.ToInt16(date.Substring(2, 2));
            year = Convert.ToInt16(date.Substring(4, 4)) - 543;
            dt = new DateTime(year, month, day);
            date = day.ToString("00") + "/" + month.ToString("00") + "/" + (year + 543).ToString("0000");
            DwMain.SetItemString(1, "member_dead_tdate", date);
        }

        private void InsertDwMain()
        {
            string assist_docno         //เลขที่ใบคำขอ
                   , member_no          //เลขสมาชิก
                   , remark             //หมายเหตุ
                   , deptaccount_no     //เลขบัญชี
                   , cancle_id          //ผู้ยกเลิก
                   , membgroup_code     //สังกัด
                   , expense_bank       //รหัสธนาคาร
                   , voucher_no         //เลข voucher
                   , expense_branch     //รหัสสาขาธนาคาร
                   , married_member     //หมายเลขคู่สมรส
                   , moneytype_code     //วิธีการจ่าย
                   , tofrom_accid       //คู่บัญชี
                   , assisttype_code    //ประเภทสวัสดิการ
                   , req_tdate
                   , cancle_tdate
                   , approve_tdate
                   , pay_tdate
                ;
            decimal capital_year        //ปีทุนสวัสดิการ
                   , assist_amt         //จำนวนเงินสวัสดิการ
                   , req_status         //สถานะใบคำขอ
                   , posttovc_flag      //
                   , pay_status         //สถานะจ่าย
                   , seq_pay            //ลำดับการจ่าย
                ;
            DateTime req_date           //วันที่ขอ
                   , cancle_date        //วันที่ยอกเลิก
                   , approve_date       //วันที่อนุมัติ
                   , pay_date           //วันที่จ่าย
                ;

            #region GetItemBeforeSave
            try { assist_docno = DwMain.GetItemString(1, "assist_docno"); }
            catch { assist_docno = ""; }
            try { member_no = DwMem.GetItemString(1, "member_no"); }
            catch { member_no = ""; }
            try { remark = DwDetail.GetItemString(1, "remark"); }
            catch { remark = ""; }
            try { deptaccount_no = DwMain.GetItemString(1, "deptaccount_no"); }
            catch { deptaccount_no = ""; }
            try { cancle_id = DwMain.GetItemString(1, "cancle_id"); }
            catch { cancle_id = ""; }
            try { membgroup_code = DwMain.GetItemString(1, "membgroup_code"); }
            catch { membgroup_code = ""; }
            try { expense_bank = DwMain.GetItemString(1, "expense_bank"); }
            catch { expense_bank = ""; }
            try { seq_pay = DwDetail.GetItemDecimal(1, "seq_pay"); }
            catch { seq_pay = 0; }
            try { voucher_no = DwMain.GetItemString(1, "voucher_no"); }
            catch { voucher_no = ""; }
            try { expense_branch = DwMain.GetItemString(1, "expense_branch"); }
            catch { expense_branch = ""; }
            try { married_member = DwMain.GetItemString(1, "married_member"); }
            catch { married_member = ""; }
            try { moneytype_code = DwMain.GetItemString(1, "moneytype_code"); }
            catch { moneytype_code = ""; }
            try { tofrom_accid = DwMain.GetItemString(1, "tofrom_accid"); }
            catch { tofrom_accid = ""; }
            try { assisttype_code = DwMain.GetItemString(1, "assisttype_code"); }
            catch { assisttype_code = ""; }
            try { capital_year = DwMain.GetItemDecimal(1, "capital_year"); }
            catch { capital_year = state.SsWorkDate.Year; }
            try { assist_amt = DwDetail.GetItemDecimal(1, "assist_amt"); }
            catch { assist_amt = 0; }
            try { req_status = DwMain.GetItemDecimal(1, "req_status"); }
            catch { req_status = 0; }
            try { posttovc_flag = DwMain.GetItemDecimal(1, "posttovc_flag"); }
            catch { posttovc_flag = 0; }
            try { pay_status = DwMain.GetItemDecimal(1, "pay_status"); }
            catch { pay_status = 0; }
            try { req_date = DwMain.GetItemDateTime(1, "req_date"); }
            catch { req_date = state.SsWorkDate; }
            req_tdate = req_date.ToString("dd/MM/yyyy", WebUtil.EN);
            try { cancle_date = DwMain.GetItemDateTime(1, "cancle_date"); }
            catch { cancle_date = state.SsWorkDate; }
            cancle_tdate = cancle_date.ToString("dd/MM/yyyy", WebUtil.EN);
            try { approve_date = DwMain.GetItemDateTime(1, "approve_date"); }
            catch { approve_date = state.SsWorkDate; }
            approve_tdate = approve_date.ToString("dd/MM/yyyy", WebUtil.EN);
            try { pay_date = DwMain.GetItemDateTime(1, "pay_date"); }
            catch { pay_date = state.SsWorkDate; }
            pay_tdate = pay_date.ToString("dd/MM/yyyy", WebUtil.EN);
            #endregion

            sqlStr = @" INSERT INTO asnreqmaster(
                                    assist_docno                , member_no                 , remark                    , deptaccount_no                    , cancel_id
                                    , membgroup_code            , expense_bank              , voucher_no                , assisttype_code                   , expense_branch
                                    , married_member            , moneytype_code            , tofrom_accid              , capital_year                      , assist_amt
                                    , req_status                , posttovc_flag             , pay_status                , req_date                          , cancel_date
                                    , approve_date              , pay_date                  , coop_id                   , expense_accid
                                    )VALUES(
                                    '" + assist_docno + @"'     ,'" + member_no + @"'       ,'" + remark + @"'          ,'" + deptaccount_no + @"'          ,'" + cancle_id + @"'
                                    ,'" + membgroup_code + @"'  ,'" + expense_bank + @"'    ,'" + voucher_no + @"'      ,'" + assisttype_code + @"'         ,'" + expense_branch + @"'
                                    ,'" + married_member + @"'  ,'" + moneytype_code + @"'  ,'" + tofrom_accid + @"'    ,'" + capital_year + @"'            ,'" + assist_amt + @"'
                                    ,'" + req_status + @"'      ,'" + posttovc_flag + @"'   ,'" + pay_status + @"'      ,to_date('" + req_tdate + @"','dd/MM/yyyy') ,to_date('" + cancle_tdate + @"','dd/MM/yyyy')
                                    ,to_date('" + approve_tdate + @"','dd/MM/yyyy') ,to_date('" + pay_tdate + @"','dd/MM/yyyy') ,'" + state.SsCoopId + @"'  ,'" + deptaccount_no + @"'
                                    )
                      ";
            ta.Exe(sqlStr);
        }

        private void InsertDwDetial()
        {
            string assist_docno                  //เลขที่ใบคำขอ
                   , remark                      //หมายเหตุ
                   , family_name                 //ชื่อครอบครัว
                   , marriage_name               //ชื่อคู่สมรส
                   , damagetype_code             //
                   , subdamagetype_code          //
                   , card_person                 //หมายเลขบัตรประชาชนผู้เสียชีวิต
                   , capital_tdate               //
                   , family_dead_tdate           //วันที่ครอบครัวเสียชีวิต
                   , marriage_dead_tdate         //
                   , member_dead_case            //สาเหตุการตาย
                ;
            decimal capital_year                 //ปีทุนสวัสดิการ
                   , assist_amt                  //จำนวนเงินสวัสดิการ
                   , family_type                 //
                   , family_age                  //อายุครอบครัว
                   , order_marriage              //
                   , seq_pay                     //ลำดับการจ่าย
                   , seq_pay_family              //
                ;
            DateTime capital_date                //
                   , family_dead_date            //วันที่ครอบครัวเสียชีวิต
                   , marriage_dead_date          //
                ;

            #region GetItemBeforeSave
            try { assist_docno = DwMain.GetItemString(1, "assist_docno"); }
            catch { assist_docno = ""; }
            try { remark = DwDetail.GetItemString(1, "remark"); }
            catch { remark = ""; }
            try { family_name = DwDetail.GetItemString(1, "family_name"); }
            catch { family_name = ""; }
            try { marriage_name = DwMain.GetItemString(1, "marriage_name"); }
            catch { marriage_name = ""; }
            damagetype_code = "03";
            subdamagetype_code = "05";
            try { card_person = DwDetail.GetItemString(1, "card_person"); }
            catch { card_person = ""; }
            try { member_dead_case = DwDetail.GetItemString(1, "member_dead_case"); }
            catch { member_dead_case = ""; }
            try { capital_year = DwMain.GetItemDecimal(1, "capital_year"); }
            catch { capital_year = state.SsWorkDate.Year; }
            try { assist_amt = DwDetail.GetItemDecimal(1, "assist_amt"); }
            catch { assist_amt = 0; }
            try { family_type = DwDetail.GetItemDecimal(1, "family_type"); }
            catch { family_type = 0; }
            try { family_age = DwDetail.GetItemDecimal(1, "family_age"); }
            catch { family_age = 0; }
            try { order_marriage = DwMain.GetItemDecimal(1, "order_marriage"); }
            catch { order_marriage = 0; }
            try { seq_pay = DwDetail.GetItemDecimal(1, "seq_pay"); }
            catch { seq_pay = 0; }
            try { seq_pay_family = DwDetail.GetItemDecimal(1, "seq_pay_family"); }
            catch { seq_pay_family = 0; }
            try { capital_date = DwDetail.GetItemDateTime(1, "capital_date"); }
            catch { capital_date = state.SsWorkDate; }
            capital_tdate = capital_date.ToString("dd/MM/yyyy", WebUtil.EN);
            try { family_dead_date = DwDetail.GetItemDateTime(1, "family_dead_date"); }
            catch { family_dead_date = state.SsWorkDate; }
            family_dead_tdate = family_dead_date.ToString("dd/MM/yyyy", WebUtil.EN);
            try { marriage_dead_date = DwMain.GetItemDateTime(1, "marriage_dead_date"); }
            catch { marriage_dead_date = state.SsWorkDate; }
            marriage_dead_tdate = marriage_dead_date.ToString("dd/MM/yyyy", WebUtil.EN);
            #endregion

            sqlStr = @" INSERT INTO asnreqcapitaldet(
                            assist_docno                   , remark                         , family_name                   , marriage_name
                            , damagetype_code              , subdamagetype_code             , card_person                   , capital_year
                            , assist_amt                   , family_type                    , family_age                    , order_marriage
                            , seq_pay                      , seq_pay_family                 , capital_date                  , family_dead_date
                            , marriage_dead_date           , member_dead_case
                        ) VALUES(
                            '" + assist_docno + @"'        ,'" + remark + @"'               ,'" + family_name + @"'         ,'" + marriage_name + @"'
                            ,'" + damagetype_code + @"'    ,'" + subdamagetype_code + @"'   ,'" + card_person + @"'         ,'" + capital_year + @"'
                            ,'" + assist_amt + @"'         ,'" + family_type + @"'          ,'" + family_age + @"'          ,'" + order_marriage + @"'
                            ,'" + seq_pay + @"'            ,'" + seq_pay_family + @"'       ,to_date('" + capital_tdate + @"','dd/MM/yyyy'),to_date('" + family_dead_tdate + @"','dd/MM/yyyy')
                            ,to_date('" + marriage_dead_tdate + @"','dd/MM/yyyy')    ,'" + member_dead_case + @"'
                        )
                      ";
            ta.Exe(sqlStr);
        }

        private void UpdateDwMain()
        {
            string assist_docno         //เลขที่ใบคำขอ
                   , member_no          //เลขสมาชิก
                   , remark             //หมายเหตุ
                   , deptaccount_no     //เลขบัญชี
                   , cancle_id          //ผู้ยกเลิก
                   , membgroup_code     //สังกัด
                   , expense_bank       //รหัสธนาคาร
                   , voucher_no         //เลข voucher
                   , expense_branch     //รหัสสาขาธนาคาร
                   , married_member     //หมายเลขคู่สมรส
                   , moneytype_code     //วิธีการจ่าย
                   , tofrom_accid       //คู่บัญชี
                   , assisttype_code    //ประเภทสวัสดิการ
                   , req_tdate
                   , cancle_tdate
                   , approve_tdate
                   , pay_tdate
                ;
            decimal capital_year        //ปีทุนสวัสดิการ
                   , assist_amt         //จำนวนเงินสวัสดิการ
                   , req_status         //สถานะใบคำขอ
                   , posttovc_flag      //
                   , pay_status         //สถานะจ่าย
                   , seq_pay            //ลำดับการจ่าย
                ;
            DateTime req_date           //วันที่ขอ
                   , cancle_date        //วันที่ยอกเลิก
                   , approve_date       //วันที่อนุมัติ
                   , pay_date           //วันที่จ่าย
                ;

            #region GetItemBeforeSave
            try { assist_docno = DwMain.GetItemString(1, "assist_docno"); }
            catch { assist_docno = ""; }
            try { member_no = DwMem.GetItemString(1, "member_no"); }
            catch { member_no = ""; }
            try { remark = DwDetail.GetItemString(1, "remark"); }
            catch { remark = ""; }
            try { deptaccount_no = DwMain.GetItemString(1, "deptaccount_no"); }
            catch { deptaccount_no = ""; }
            try { cancle_id = DwMain.GetItemString(1, "cancle_id"); }
            catch { cancle_id = ""; }
            try { membgroup_code = DwMain.GetItemString(1, "membgroup_code"); }
            catch { membgroup_code = ""; }
            try { expense_bank = DwMain.GetItemString(1, "expense_bank"); }
            catch { expense_bank = ""; }
            try { seq_pay = DwDetail.GetItemDecimal(1, "seq_pay"); }
            catch { seq_pay = 0; }
            try { voucher_no = DwMain.GetItemString(1, "voucher_no"); }
            catch { voucher_no = ""; }
            try { expense_branch = DwMain.GetItemString(1, "expense_branch"); }
            catch { expense_branch = ""; }
            try { married_member = DwMain.GetItemString(1, "married_member"); }
            catch { married_member = ""; }
            try { moneytype_code = DwMain.GetItemString(1, "moneytype_code"); }
            catch { moneytype_code = ""; }
            try { tofrom_accid = DwMain.GetItemString(1, "tofrom_accid"); }
            catch { tofrom_accid = ""; }
            try { assisttype_code = DwMain.GetItemString(1, "assisttype_code"); }
            catch { assisttype_code = ""; }
            try { capital_year = DwMain.GetItemDecimal(1, "capital_year"); }
            catch { capital_year = state.SsWorkDate.Year; }
            try { assist_amt = DwDetail.GetItemDecimal(1, "assist_amt"); }
            catch { assist_amt = 0; }
            try { req_status = DwMain.GetItemDecimal(1, "req_status"); }
            catch { req_status = 0; }
            try { posttovc_flag = DwMain.GetItemDecimal(1, "posttovc_flag"); }
            catch { posttovc_flag = 0; }
            try { pay_status = DwMain.GetItemDecimal(1, "pay_status"); }
            catch { pay_status = 0; }
            try { req_date = DwMain.GetItemDateTime(1, "req_date"); }
            catch { req_date = state.SsWorkDate; }
            req_tdate = req_date.ToString("dd/MM/yyyy", WebUtil.EN);
            try { cancle_date = DwMain.GetItemDateTime(1, "cancle_date"); }
            catch { cancle_date = state.SsWorkDate; }
            cancle_tdate = cancle_date.ToString("dd/MM/yyyy", WebUtil.EN);
            try { approve_date = DwMain.GetItemDateTime(1, "approve_date"); }
            catch { approve_date = state.SsWorkDate; }
            approve_tdate = approve_date.ToString("dd/MM/yyyy", WebUtil.EN);
            try { pay_date = DwMain.GetItemDateTime(1, "pay_date"); }
            catch { pay_date = state.SsWorkDate; }
            pay_tdate = pay_date.ToString("dd/MM/yyyy", WebUtil.EN);
            #endregion

            sqlStr = @" UPDATE asnreqmaster SET
                          remark = '" + remark + @"'
                          , deptaccount_no = '" + deptaccount_no + @"'
                          , cancel_id = '" + cancle_id + @"'
                          , membgroup_code = '" + membgroup_code + @"'
                          , expense_bank = '" + expense_bank + @"'
                          , voucher_no = '" + voucher_no + @"'
                          , expense_branch = '" + expense_branch + @"'
                          , married_member = '" + married_member + @"'
                          , moneytype_code = '" + moneytype_code + @"'
                          , tofrom_accid = '" + tofrom_accid + @"'
                          , assist_amt = '" + assist_amt + @"'
                          , req_status = '" + req_status + @"'
                          , posttovc_flag = '" + posttovc_flag + @"'
                          , pay_status = '" + pay_status + @"'
                          , req_date = to_date('" + req_tdate + @"','dd/MM/yyyy')
                          , cancel_date = to_date('" + cancle_tdate + @"','dd/MM/yyyy')
                          , approve_date = to_date('" + approve_tdate + @"','dd/MM/yyyy')
                          , pay_date = to_date('" + pay_tdate + @"','dd/MM/yyyy')
                      
                       WHERE assist_docno = '" + assist_docno + @"'
                       AND member_no = '" + member_no + @"'
                       AND capital_year = '" + capital_year + @"'
                    ";
            ta.Exe(sqlStr);
        }

        private void UpdateDwDetial()
        {
            string assist_docno                  //เลขที่ใบคำขอ
                   , remark                      //หมายเหตุ
                   , family_name                 //ชื่อครอบครัว
                   , marriage_name               //ชื่อคู่สมรส
                   , damagetype_code             //
                   , subdamagetype_code          //
                   , card_person                 //หมายเลขบัตรประชาชนผู้เสียชีวิต
                   , member_dead_case            //สาเหตุการตาย
                   , capital_tdate               //
                   , family_dead_tdate           //วันที่ครอบครัวเสียชีวิต
                   , marriage_dead_tdate         //
                ;
            decimal capital_year                 //ปีทุนสวัสดิการ
                   , assist_amt                  //จำนวนเงินสวัสดิการ
                   , family_type                 //
                   , family_age                  //อายุครอบครัว
                   , order_marriage              //
                   , seq_pay                     //ลำดับการจ่าย
                   , seq_pay_family              //
                ;
            DateTime capital_date                //
                   , family_dead_date            //วันที่ครอบครัวเสียชีวิต
                   , marriage_dead_date          //
                ;

            #region GetItemBeforeSave
            try { assist_docno = DwMain.GetItemString(1, "assist_docno"); }
            catch { assist_docno = ""; }
            try { remark = DwDetail.GetItemString(1, "remark"); }
            catch { remark = ""; }
            try { family_name = DwDetail.GetItemString(1, "family_name"); }
            catch { family_name = ""; }
            try { marriage_name = DwMain.GetItemString(1, "marriage_name"); }
            catch { marriage_name = ""; }
            damagetype_code = "03";
            subdamagetype_code = "05";
            try { card_person = DwDetail.GetItemString(1, "card_person"); }
            catch { card_person = ""; }
            try { member_dead_case = DwDetail.GetItemString(1, "member_dead_case"); }
            catch { member_dead_case = ""; }
            try { capital_year = DwMain.GetItemDecimal(1, "capital_year"); }
            catch { capital_year = state.SsWorkDate.Year; }
            try { assist_amt = DwDetail.GetItemDecimal(1, "assist_amt"); }
            catch { assist_amt = 0; }
            try { family_type = DwDetail.GetItemDecimal(1, "family_type"); }
            catch { family_type = 0; }
            try { family_age = DwDetail.GetItemDecimal(1, "family_age"); }
            catch { family_age = 0; }
            try { order_marriage = DwMain.GetItemDecimal(1, "order_marriage"); }
            catch { order_marriage = 0; }
            try { seq_pay = DwDetail.GetItemDecimal(1, "seq_pay"); }
            catch { seq_pay = 0; }
            try { seq_pay_family = DwDetail.GetItemDecimal(1, "seq_pay_family"); }
            catch { seq_pay_family = 0; }
            try { capital_date = DwDetail.GetItemDateTime(1, "capital_date"); }
            catch { capital_date = state.SsWorkDate; }
            capital_tdate = capital_date.ToString("dd/MM/yyyy", WebUtil.EN);
            try { family_dead_date = DwDetail.GetItemDateTime(1, "family_dead_date"); }
            catch { family_dead_date = state.SsWorkDate; }
            family_dead_tdate = family_dead_date.ToString("dd/MM/yyyy", WebUtil.EN);
            try { marriage_dead_date = DwMain.GetItemDateTime(1, "marriage_dead_date"); }
            catch { marriage_dead_date = state.SsWorkDate; }
            marriage_dead_tdate = marriage_dead_date.ToString("dd/MM/yyyy", WebUtil.EN);
            #endregion

            sqlStr = @" UPDATE asnreqcapitaldet SET
                            remark ='" + remark + @"'
                            , family_name ='" + family_name + @"'
                            , marriage_name ='" + marriage_name + @"'
                            , card_person ='" + card_person + @"'
                            , member_dead_case ='" + member_dead_case + @"'
                            , assist_amt ='" + assist_amt + @"'
                            , family_type ='" + family_type + @"'
                            , family_age ='" + family_age + @"'
                            , order_marriage ='" + order_marriage + @"'
                            , seq_pay ='" + seq_pay + @"'
                            , seq_pay_family ='" + seq_pay_family + @"'
                            , capital_date =to_date('" + capital_tdate + @"','dd/MM/yyyy')
                            , family_dead_date =to_date('" + family_dead_tdate + @"','dd/MM/yyyy')
                            , marriage_dead_date =to_date('" + marriage_dead_tdate + @"','dd/MM/yyyy')
                        WHERE 
                            assist_docno ='" + assist_docno + @"'
                        AND capital_year ='" + capital_year + @"'
                      ";
            ta.Exe(sqlStr);
        }

        private void CheckIDCard()
        {
            try
            {
                string fam_type = DwDetail.GetItemDecimal(1, "family_type").ToString();
                Sdt dtcheck;
                string s_pay_date;
                switch (fam_type)
                {
                    case "3":


                        string soncard_person = DwDetail.GetItemString(1, "card_person").Replace("-", "");
                        string checksondup = "select a.pay_date as pay_date from asnreqmaster a,asnreqcapitaldet b where a.assist_docno = b.assist_docno and a.capital_year = b.capital_year and a.assisttype_code = '40' and b.family_type in ('3') and b.card_person = '" + soncard_person + "'";
                        dtcheck = WebUtil.QuerySdt(checksondup);
                        if (dtcheck.Next())
                        {
                            pay_date = dtcheck.GetDate("pay_date");
                            s_pay_date = pay_date.ToString("dd/MM/") + (pay_date.Year + 543).ToString();
                            infomatAll();
                            Response.Write("<script language='javascript'>alert('หมายเลขบัตรประชาชน " + soncard_person + " ทำการขอสวัสดิการถึงแก่กรรมไปแล้ว เมื่อวันที่ " + s_pay_date + "'\n );</script>");
                            return;
                        }
                        break;
                    case "1":
                        string dadcard_person = DwDetail.GetItemString(1, "card_person").Replace("-", "");
                        string checkfatherdup = "select a.pay_date as pay_date from asnreqmaster a,asnreqcapitaldet b where a.assist_docno = b.assist_docno and a.capital_year = b.capital_year and a.assisttype_code = '40' and b.family_type in ('1') and b.card_person = '" + dadcard_person + "'";
                        dtcheck = WebUtil.QuerySdt(checkfatherdup);
                        if (dtcheck.Next())
                        {
                            pay_date = dtcheck.GetDate("pay_date");
                            s_pay_date = pay_date.ToString("dd/MM/") + (pay_date.Year + 543).ToString();
                            infomatAll();
                            Response.Write("<script language='javascript'>alert('หมายเลขบัตรประชาชน " + dadcard_person + " ทำการขอสวัสดิการถึงแก่กรรมไปแล้ว เมื่อวันที่ " + s_pay_date + "'\n );</script>");
                            return;
                        }

                        break;

                    case "2":
                        string momcard_person = DwDetail.GetItemString(1, "card_person").Replace("-", "");
                        string checkmomdup = "select a.pay_date as pay_date from asnreqmaster a,asnreqcapitaldet b where a.assist_docno = b.assist_docno and a.capital_year = b.capital_year and a.assisttype_code = '40' and b.family_type in ('2') and b.card_person = '" + momcard_person + "'";
                        dtcheck = WebUtil.QuerySdt(checkmomdup);
                        if (dtcheck.Next())
                        {
                            pay_date = dtcheck.GetDate("pay_date");
                            s_pay_date = pay_date.ToString("dd/MM/") + (pay_date.Year + 543).ToString();
                            infomatAll();
                            Response.Write("<script language='javascript'>alert('หมายเลขบัตรประชาชน " + momcard_person + " ทำการขอสวัสดิการถึงแก่กรรมไปแล้ว เมื่อวันที่ " + s_pay_date + "'\n );</script>");
                            return;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }
    }
}
