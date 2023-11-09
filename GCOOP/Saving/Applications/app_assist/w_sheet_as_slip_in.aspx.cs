using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sybase.DataWindow;
using DataLibrary;

namespace Saving.Applications.app_assist
{
    public partial class w_sheet_as_slip_in : PageWebSheet, WebSheet
    {
        protected String postRetreiveDwMem;
        protected String postRetrieveDwMain;
        protected String postRefresh;
        protected String postRetrieveBankBranch;
        private Nullable<Decimal> cancel_id;
        private DateTime pay_date, cancel_date = DateTime.Now;
        private Decimal capital_year;
        protected String postChangeHeight;
        protected String postGetMemberDetail;
        private DateTime req_date;

        #region Value_mb_dead_normal
        protected String app;
        protected String gid;
        protected String rid;
        protected String pdf;
        protected String postPopupReport;

        protected DwThDate tDwMain_mb_dead_normal;
        protected DwThDate tDwDetail_mb_dead_normal;

        protected String postChangeAmt;

        protected String postDelete;
        protected String postChangeAge;

        private String dwFlag;
        private String sqlStr, envvalue;
        private String member_no, assist_docno, membgroup_code, member_dead_tdate, assisttype_code;
        private String expense_bank, expense_branch, expense_accid, paytype_code, remark_cancel;
        private Decimal salary_amt, assist_amt, req_status, seq_pay, rowcount;
        private DateTime center_date, member_date, birth_date, member_age;

        TimeSpan tp;

        protected Sta ta;
        protected Sdt dt;
        #endregion

        #region Value_scholarship_mu
        private String pbl = "as_public_funds.pbl";
        private Decimal level_School, seq_no_same, level_School_same;
        private DataStore Ds;
        private Decimal now_year;
        protected DwThDate tDwMain_scholarship_mu;
        //protected DwThDate tDwDetail;
        protected DwThDate tDwDetail_scholarship_mu;

        protected String postChangeHost;
        protected String postCheckDate;
        protected String postCopyAddress;
        protected String postMemProvince;
        protected String postMemDistrict;
        protected String postMemTambol;
        protected String postMainProvince;
        protected String postMainDistrict;
        protected String postMainTambol;
        protected String saveBranchId;
        //------------------------ ทุนศึกษา -------------------------------------------
        protected String postNewClear;
        protected String postGetMoney;
        protected String postFilterScholarship;
        protected String postCheckReQuest;
        protected String postGetLevelSchool;
        #endregion

        #region WebSheetMember
        public void InitJsPostBack()
        {
            dwFlag = HdDwCode.Value.Trim();

            switch (dwFlag)
            {
                case "1":
                    break;
                case "2":
                    break;
                case "3":
                    Init_mb_dead_normal();
                    break;
                case "4":
                    break;
                case "5":
                    break;
            }
        }

        public void WebSheetLoadBegin()
        {
            switch (dwFlag)
            {
                case "1":
                    break;
                case "2":
                    break;
                case "3":
                    LoadBegin_mb_dead_normal();
                    break;
                case "4":
                    break;
                case "5":
                    break;
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (dwFlag)
            {
                case "1":
                    break;
                case "2":
                    break;
                case "3":
                    CheckPostBack_mb_dead_normal(eventArg);
                    break;
                case "4":
                    break;
                case "5":
                    break;
            }
        }

        public void SaveWebSheet()
        {
            switch (dwFlag)
            {
                case "1":
                    break;
                case "2":
                    break;
                case "3":
                    SaveWebSheet_mb_dead_normal();
                    break;
                case "4":
                    break;
                case "5":
                    break;
            }
        }

        public void WebSheetLoadEnd()
        {
            switch (dwFlag)
            {
                case "1":
                    break;
                case "2":
                    break;
                case "3":
                    LoadEnd_mb_dead_normal();
                    break;
                case "4":
                    break;
                case "5":
                    break;
            }
        }
        #endregion

        #region Function

        #region as_scholarship_mu
        public void Init_scholarship_mu()
        {
            postRefresh = WebUtil.JsPostBack(this, "postRefresh");
            postChangeHost = WebUtil.JsPostBack(this, "postChangeHost");
            postChangeHeight = WebUtil.JsPostBack(this, "postChangeHeight");
            postRetreiveDwMem = WebUtil.JsPostBack(this, "postRetreiveDwMem");
            postRetrieveDwMain = WebUtil.JsPostBack(this, "postRetrieveDwMain");
            postCheckDate = WebUtil.JsPostBack(this, "postCheckDate");
            postGetMemberDetail = WebUtil.JsPostBack(this, "postGetMemberDetail");
            postRetrieveBankBranch = WebUtil.JsPostBack(this, "postRetrieveBankBranch");
            postCopyAddress = WebUtil.JsPostBack(this, "postCopyAddress");
            postMemProvince = WebUtil.JsPostBack(this, "postMemProvince");
            postMemDistrict = WebUtil.JsPostBack(this, "postMemDistrict");
            postMemTambol = WebUtil.JsPostBack(this, "postMemTambol");
            postMainProvince = WebUtil.JsPostBack(this, "postMainProvince");
            postMainDistrict = WebUtil.JsPostBack(this, "postMainDistrict");

            postMainTambol = WebUtil.JsPostBack(this, "postMainTambol");
            saveBranchId = WebUtil.JsPostBack(this, "saveBranchId");
            postNewClear = WebUtil.JsPostBack(this, "postNewClear");
            postGetMoney = WebUtil.JsPostBack(this, "postGetMoney");
            postFilterScholarship = WebUtil.JsPostBack(this, "postFilterScholarship");
            postCheckReQuest = WebUtil.JsPostBack(this, "postCheckReQuest");
            postCheckReQuest = WebUtil.JsPostBack(this, "postGetLevelSchool");


            tDwMain_scholarship_mu = new DwThDate(DwMain_scholarship_mu, this);
            tDwMain_scholarship_mu.Add("req_date", "req_tdate");
            tDwMain_scholarship_mu.Add("approve_date", "approve_tdate");

            tDwDetail_scholarship_mu = new DwThDate(DwDetail_scholarship_mu, this);
            tDwDetail_scholarship_mu.Add("childbirth_date", "childbirth_tdate");

        }
        public void LoadBegin_scholarship_mu()
        {
            this.ConnectSQLCA();

            HdDuplicate.Value = "";
            HdNameDuplicate.Value = "";
            LtAlert.Text = "";
            //try {

            //    ss_dropdown_scholarship_branch = Session["ss_dropdown_scholarship_branch"].ToString();
            //    if (ss_dropdown_scholarship_branch == "") {
            //        ss_dropdown_scholarship_branch = state.SsCoopId;
            //   //   ss_dropdown_scholarship_branch = state.ssCoopid; ** ถ้ารวมโปรแกรมที่มหิดลเปลี่ยนเป็น coopid
            //    }
            //}
            //catch {
            //    ss_dropdown_scholarship_branch = "001001";//state.SsCoopId;
            //}
            if (!IsPostBack)
            {
                //NewClear();
                HdActionStatus.Value = "Insert";
                DwMem.InsertRow(0);
                DwMain_scholarship_mu.InsertRow(0);
                DwDetail_scholarship_mu.InsertRow(0);
            }
            else
            {
                this.RestoreContextDw(DwMem);
                this.RestoreContextDw(DwMain_scholarship_mu, tDwMain_scholarship_mu);
                this.RestoreContextDw(DwDetail_scholarship_mu, tDwDetail_scholarship_mu);
            }
            //DwMain.SetItemString(1, "assisttype_code", "10");
            DwUtil.RetrieveDDDW(DwMain_scholarship_mu, "assisttype_code", "as_public_funds.pbl", state.SsCoopId);
            DwUtil.RetrieveDDDW(DwMain_scholarship_mu, "capital_year", "as_capital.pbl", null);
            //aek
            //DwUtil.RetrieveDDDW(DwMain, "expense_bank", "as_capital.pbl", null);
            // DwUtil.RetrieveDDDW(DwMain, "cancel_id", "as_capital.pbl", null);
            DwUtil.RetrieveDDDW(DwMain_scholarship_mu, "req_status", "as_capital.pbl", null);
            //DwUtil.RetrieveDDDW(DwMain, "pay_status", "as_capital.pbl", null);
            // DwUtil.RetrieveDDDW(DwMain, "paytype_code", "as_capital.pbl", null);

            DwUtil.RetrieveDDDW(DwDetail_scholarship_mu, "scholarship_type", "as_capital.pbl", state.SsCoopId);
            //DwDetail.SetItemDecimal(1, "scholarship_type", 3);
            DwUtil.RetrieveDDDW(DwDetail_scholarship_mu, "scholarship_level", "as_capital.pbl", null);
            //DwUtil.RetrieveDDDW(DwDetail, "level_school", "as_capital.pbl", null);
            DwUtil.RetrieveDDDW(DwDetail_scholarship_mu, "childprename_code", "as_public_funds.pbl", null);
            //DwMain.SetItemDateTime(1, "req_date", state.SsWorkDate);//req_tdate
            //DwMain.SetItemDateTime(1, "approve_date", state.SsWorkDate);

            //tDwMain.Eng2ThaiAllRow();
            tDwDetail_scholarship_mu.Eng2ThaiAllRow();
        }
        public void CheckPostBack_scholarship_mu(string eventArg)
        {
            switch (eventArg)
            {
                case "postRetreiveDwMem":
                    RetreiveDwMem();
                    break;
                case "postRefresh":
                    break;
                case "postCheckDate":
                    break;
                case "postRetrieveDwMain":
                    RetrieveDwMain();
                    break;
                case "postChangeHost":
                    break;
                case "postRetrieveBankBranch":
                    RetrieveBankBranch();
                    break;
                case "postChangeHeight":
                    break;
                case "postGetMemberDetail":
                    GetMemberDetail();
                    break;
                case "postCopyAddress":
                    JsPostCopyAddress();
                    break;
                case "postMemProvince":
                    DwMem.SetItemNull(1, "postcode");
                    DwMem.SetItemNull(1, "district_code");
                    DwMem.SetItemNull(1, "tambol_code");
                    break;
                case "postMemDistrict":
                    DwMem.SetItemNull(1, "tambol_code");
                    try
                    {
                        String sqlPostCode = "select * from mbucfdistrict where district_code = '" + DwMem.GetItemString(1, "district_code") + "'";
                        Sdt dtPostCode = WebUtil.QuerySdt(sqlPostCode);
                        if (dtPostCode.Next())
                        {
                            DwMem.SetItemString(1, "postcode", dtPostCode.GetString("postcode"));
                        }
                    }
                    catch { }
                    break;
                case "postMemTambol":
                    break;
                case "postMainProvince":
                    DwMain_scholarship_mu.SetItemNull(1, "postcode");
                    DwMain_scholarship_mu.SetItemNull(1, "district_code");
                    DwMain_scholarship_mu.SetItemNull(1, "tambol_code");
                    break;
                case "postMainDistrict":
                    DwMain_scholarship_mu.SetItemNull(1, "tambol_code");
                    try
                    {
                        String sqlPostCode = "select * from mbucfdistrict where district_code = '" + DwMain_scholarship_mu.GetItemString(1, "district_code") + "'";
                        Sdt dtPostCode = WebUtil.QuerySdt(sqlPostCode);
                        if (dtPostCode.Next())
                        {
                            DwMain_scholarship_mu.SetItemString(1, "postcode", dtPostCode.GetString("postcode"));
                        }
                    }
                    catch { }
                    break;
                case "postMainTambol":
                    break;
                case "postGetMoney":
                    GetMoney();
                    break;
                case "postFilterScholarship":
                    FilterScholarship();
                    break;
                case "postCheckReQuest":
                    CheckReQuest();
                    break;
                case "postGetLevelSchool":
                    CheckLevelSchool();
                    break;

            }
        }
        public void SaveWebSheet_scholarship_mu()
        {
        }
        public void LoadEnd_scholarship_mu()
        {
        }
        #endregion

        #region as_member_dead_normal_@3
        //---Websheet Member---
        public void Init_mb_dead_normal()
        {
            postRefresh = WebUtil.JsPostBack(this, "postRefresh");
            postChangeAmt = WebUtil.JsPostBack(this, "postChangeAmt");
            postChangeHeight = WebUtil.JsPostBack(this, "postChangeHeight");
            postRetreiveDwMem = WebUtil.JsPostBack(this, "postRetreiveDwMem");
            postRetrieveDwMain = WebUtil.JsPostBack(this, "postRetrieveDwMain");
            postGetMemberDetail = WebUtil.JsPostBack(this, "postGetMemberDetail");
            postRetrieveBankBranch = WebUtil.JsPostBack(this, "postRetrieveBankBranch");
            postDelete = WebUtil.JsPostBack(this, "postDelete");
            postChangeAge = WebUtil.JsPostBack(this, "postChangeAge");
            postPopupReport = WebUtil.JsPostBack(this, "postPopupReport");

            tDwMain_mb_dead_normal = new DwThDate(DwMain_mb_dead_normal, this);
            tDwDetail_mb_dead_normal = new DwThDate(DwDetail_mb_dead_normal, this);

            tDwMain_mb_dead_normal.Add("pay_date", "pay_tdate");
            tDwMain_mb_dead_normal.Add("req_date", "req_tdate");
            tDwMain_mb_dead_normal.Add("entry_date", "entry_tdate");
            tDwMain_mb_dead_normal.Add("member_date", "member_tdate");
            tDwMain_mb_dead_normal.Add("approve_date", "approve_tdate");
            tDwDetail_mb_dead_normal.Add("member_dead_date", "member_dead_tdate");
        }
        public void LoadBegin_mb_dead_normal()
        {
            tDwMain_mb_dead_normal.Eng2ThaiAllRow();
            tDwDetail_mb_dead_normal.Eng2ThaiAllRow();

            this.ConnectSQLCA();

            ta = new Sta(sqlca.ConnectionString);
            dt = new Sdt();

            LtAlert.Text = "";
            if (!IsPostBack)
            {
                NewClear_mb_dead_normal();
            }
            else
            {
                this.RestoreContextDw(DwMem);
                this.RestoreContextDw(DwMain_mb_dead_normal);
                this.RestoreContextDw(DwDetail_mb_dead_normal);
            }

            DwMain_mb_dead_normal.SetItemString(1, "assisttype_code", "30");

            DwUtil.RetrieveDDDW(DwMain_mb_dead_normal, "assisttype_code", "as_capital.pbl", state.SsCoopId);
            DwUtil.RetrieveDDDW(DwMain_mb_dead_normal, "capital_year", "as_capital.pbl", null);
            DwUtil.RetrieveDDDW(DwMain_mb_dead_normal, "cancel_id", "as_capital.pbl", null);
            DwUtil.RetrieveDDDW(DwMain_mb_dead_normal, "req_status", "as_capital.pbl", null);
            DwUtil.RetrieveDDDW(DwMain_mb_dead_normal, "pay_status", "as_capital.pbl", null);
            DwUtil.RetrieveDDDW(DwMain_mb_dead_normal, "expense_bank", "as_capital.pbl", null);
            DwUtil.RetrieveDDDW(DwMain_mb_dead_normal, "paytype_code", "as_capital.pbl", null);
            //DwUtil.RetrieveDDDW(DwMain, "birth_date", "as_capital.pbl", null);
            //DwUtil.RetrieveDDDW(DwDetail, "member_age", "as_capital.pbl", null);
        }
        public void CheckPostBack_mb_dead_normal(string eventArg)
        {
            switch (eventArg)
            {
                case "postRetreiveDwMem":
                    RetreiveDwMem_mb_dead_normal();
                    break;
                case "postRefresh":
                    Refresh_mb_dead_normal();
                    break;
                case "postChangeAmt":
                    ChangeAmt_mb_dead_normal();
                    break;
                case "postRetrieveDwMain":
                    RetrieveDwMain_mb_dead_normal();
                    break;
                case "postRetrieveBankBranch":
                    RetrieveBankBranch_mb_dead_normal();
                    break;
                case "postChangeHeight":
                    ChangeHeight_mb_dead_normal();
                    break;
                case "postGetMemberDetail":
                    GetMemberDetail_mb_dead_normal();
                    break;
                case "postDelete":
                    Delete_mb_dead_normal();
                    break;
                case "postChangeAge":
                    ChangeAge_mb_dead_normal();
                    break;
                case "postPopupReport":
                    break;
            }

        }
        public void SaveWebSheet_mb_dead_normal()
        {
            Sta ta2 = new Sta(state.SsConnectionString);
            ta2.Transection();

            Decimal member_age;
            String member_receive, member_dead_case, remark;
            DateTime member_dead_date, req_date;
            DwMain_mb_dead_normal.SetItemString(1, "coop_id", state.SsCoopId);
            try
            {
                if (DwDetail_mb_dead_normal.GetItemString(1, "card_person").Length != 13)
                {
                    LtServerMessage.Text = WebUtil.WarningMessage("กรุณากรอกเเลขบัตรประชาชนให้ครบ");
                    return;
                }
            }
            catch
            {
                LtServerMessage.Text = WebUtil.WarningMessage("กรุณากรอกเเลขบัตรประชาชนให้ครบ");
                return;

            }


            GetItemDwMain_mb_dead_normal();

            assist_amt = DwDetail_mb_dead_normal.GetItemDecimal(1, "assist_amt");
            try
            {
                member_age = DwDetail_mb_dead_normal.GetItemDecimal(1, "member_age");
            }
            catch { }

            member_receive = DwDetail_mb_dead_normal.GetItemString(1, "member_receive");
            member_dead_date = DwDetail_mb_dead_normal.GetItemDateTime(1, "member_dead_date");

            try
            {
                member_dead_case = DwDetail_mb_dead_normal.GetItemString(1, "member_dead_case");
            }
            catch { member_dead_case = "ไม่ระบุ"; }
            try
            {
                remark = DwDetail_mb_dead_normal.GetItemString(1, "remark");
            }
            catch { remark = ""; }

            try
            {
                assist_docno = DwMain_mb_dead_normal.GetItemString(1, "assist_docno");
            }
            catch { assist_docno = ""; }

            if (assist_docno == "" && req_status == 8)
            {
                try
                {
                    capital_year = DwMain_mb_dead_normal.GetItemDecimal(1, "capital_year");
                }
                catch { capital_year = 2555; }
                assist_docno = GetLastDocNo_mb_dead_normal(capital_year);

                try
                {
                    DwMain_mb_dead_normal.SetItemString(1, "member_no", member_no);
                    DwMain_mb_dead_normal.SetItemDecimal(1, "assist_amt", assist_amt);
                    DwMain_mb_dead_normal.SetItemString(1, "assist_docno", assist_docno);
                    try
                    {
                        string strMemberAge = DwMain_mb_dead_normal.GetItemString(1, "member_years_disp");
                        DwMain_mb_dead_normal.SetItemString(1, "member_years_disp", "");
                        DwUtil.InsertDataWindow(DwMain_mb_dead_normal, "as_capital.pbl", "asnreqmaster");
                        DwMain_mb_dead_normal.SetItemString(1, "member_years_disp", strMemberAge);
                    }
                    catch { }

                    seq_pay = GetSeqNo_mb_dead_normal();

                    DwDetail_mb_dead_normal.SetItemDecimal(1, "seq_pay", seq_pay);
                    DwDetail_mb_dead_normal.SetItemString(1, "assist_docno", assist_docno);
                    DwDetail_mb_dead_normal.SetItemDecimal(1, "capital_year", capital_year);
                    DwDetail_mb_dead_normal.SetItemDateTime(1, "capital_date", state.SsWorkDate);
                    try
                    {
                        DwUtil.InsertDataWindow(DwDetail_mb_dead_normal, "as_capital.pbl", "asnreqcapitaldet");
                    }
                    catch { }


                    sqlStr = @"INSERT INTO asnmemsalary(capital_year  ,  member_no  ,  assist_docno  ,
                                                    salary_amt    ,  entry_date  ,  membgroup_code)
                                         VALUES('" + capital_year + "','" + member_no + "','" + assist_docno + @"',
                                                '" + salary_amt + "',to_date('" + DwMain_mb_dead_normal.GetItemDateTime(1, "entry_date").ToString("dd/MM/yyyy", WebUtil.EN) + "', 'dd/mm/yyyy'),'" + membgroup_code + @"')";
                    ta.Exe(sqlStr);

                    sqlStr = @"UPDATE  asndoccontrol
                               SET     last_docno   = '" + WebUtil.Right(assist_docno, 6) + @"'
                               WHERE   doc_prefix   = 'AM' and
                                       doc_year = '" + capital_year + "'";
                    ta.Exe(sqlStr);
                    try
                    {
                        //asnslippayout
                        sqlStr = "select * from asnucfpaytype where paytype_code = '" + paytype_code + "'";
                        string moneytype_code = "";
                        string deptaccount_no;
                        try
                        {
                            deptaccount_no = DwMain_mb_dead_normal.GetItemString(1, "deptaccount_no");
                        }
                        catch
                        {
                            deptaccount_no = "";
                        }
                        Sdt dtMoneytypecode = WebUtil.QuerySdt(sqlStr);
                        Sdt dtAmt = null;
                        string work_date = state.SsWorkDate.ToString("MM/dd/yyyy");
                        if (dtMoneytypecode.Next())
                        {
                            moneytype_code = dtMoneytypecode.Rows[0]["moneytype_code"].ToString().Trim();
                        }
                        //
                        decimal age_range = Decimal.Parse(DwMain_mb_dead_normal.GetItemString(1, "age_range"));
                        sqlStr = "select * from asnsenvironmentvar where envgroup = 'member_dead_normal' and '" + age_range + "' between start_age and end_age";
                        dtAmt = WebUtil.QuerySdt(sqlStr);
                        // insert into asnslippayout
                        for (int i = 0; i < dtAmt.Rows.Count; i++)
                        {
                            int seq_no = i + 1;
                            sqlStr = @"INSERT INTO asnslippayout (payoutslip_no,member_no,slip_date,operate_date,
                                     payout_amt,slip_status,depaccount_no,assisttype_code,moneytype_code,entry_id,entry_date,seq_no,capital_year,sliptype_code,tofrom_accid)
                                     VALUES ('" + assist_docno + "','" + member_no + "',to_date('" + DwMain_mb_dead_normal.GetItemDateTime(1, "req_date").ToString("dd/MM/yyyy", WebUtil.EN) +
                                     "', 'dd/mm/yyyy'),to_date('" + DwMain_mb_dead_normal.GetItemDateTime(1, "entry_date").ToString("dd/MM/yyyy", WebUtil.EN) + "', 'dd/mm/yyyy'), '" +
                                     dtAmt.Rows[i]["envvalue"].ToString() + "','0','" + deptaccount_no + "','" + assisttype_code + "','" + moneytype_code + "','" + state.SsUsername +
                                     "',to_date('" + DwMain_mb_dead_normal.GetItemDateTime(1, "entry_date").ToString("dd/MM/yyyy", WebUtil.EN) + "', 'dd/mm/yyyy')," + seq_no + ",'"
                                     + capital_year + "','" + dtAmt.Rows[i]["system_code"] + "','" + dtAmt.Rows[i]["tofrom_accid"] + "')";
                            ta2.Exe(sqlStr);
                        }
                        ta2.Commit();
                    }
                    catch
                    {
                        ta2.RollBack();
                    }
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกเรียบร้อย");
                    NewClear_mb_dead_normal();
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            }
            else
            {
                DwMain_mb_dead_normal.SetItemDecimal(1, "assist_amt", assist_amt);
                try
                {
                    sqlStr = @"UPDATE  asnmemsalary
                               SET     salary_amt    = '" + salary_amt + @"'
                               WHERE   assist_docno  = '" + assist_docno + "'";
                    ta.Exe(sqlStr);

                    try
                    {
                        string strMemberAge = DwMain_mb_dead_normal.GetItemString(1, "member_years_disp");
                        DwMain_mb_dead_normal.SetItemString(1, "member_years_disp", "");
                        DwUtil.UpdateDateWindow(DwMain_mb_dead_normal, "as_capital.pbl", "asnreqmaster");
                        DwMain_mb_dead_normal.SetItemString(1, "member_years_disp", strMemberAge);
                    }
                    catch { }

                    try
                    {
                        DwUtil.UpdateDateWindow(DwDetail_mb_dead_normal, "as_capital.pbl", "asnreqcapitaldet");
                    }
                    catch { }

                    try
                    {
                        sqlStr = "UPDATE asnmemsalary SET entry_date = to_date('" + DwMain_mb_dead_normal.GetItemDateTime(1, "entry_date").ToString("dd/MM/yyyy", WebUtil.EN) + "', 'dd/mm/yyyy') where assist_docno = '" + assist_docno + "' and capital_year = '" + capital_year + "'";
                        ta.Exe(sqlStr);
                    }
                    catch { }

                    try
                    {
                        sqlStr = "select * from asnucfpaytype where paytype_code = '" + paytype_code + "'";
                        string moneytype_code = "";
                        string deptaccount_no;
                        try
                        {
                            deptaccount_no = DwMain_mb_dead_normal.GetItemString(1, "deptaccount_no");
                        }
                        catch
                        {
                            deptaccount_no = "";
                        }
                        Sdt dtMoneytypecode = WebUtil.QuerySdt(sqlStr);
                        Sdt dtAmt = null;
                        string work_date = state.SsWorkDate.ToString("MM/dd/yyyy");
                        if (dtMoneytypecode.Next())
                        {
                            moneytype_code = dtMoneytypecode.Rows[0]["moneytype_code"].ToString().Trim();
                        }
                        //
                        decimal age_range = Decimal.Parse(DwMain_mb_dead_normal.GetItemString(1, "age_range"));
                        sqlStr = "select * from asnsenvironmentvar where envgroup = 'member_dead_normal' and '" + age_range + "' between start_age and end_age";
                        dtAmt = WebUtil.QuerySdt(sqlStr);
                        // update asnslippayout 
                        for (int i = 0; i < dtAmt.Rows.Count; i++)
                        {
                            int seq_no = i + 1;
                            sqlStr = @"UPDATE asnslippayout SET slip_date = to_date('" + DwMain_mb_dead_normal.GetItemDateTime(1, "req_date").ToString("dd/MM/yyyy", WebUtil.EN) + "', 'dd/mm/yyyy'),operate_date = to_date('" + DwMain_mb_dead_normal.GetItemDateTime(1, "entry_date").ToString("dd/MM/yyyy", WebUtil.EN) + "', 'dd/mm/yyyy'),payout_amt = '" + dtAmt.Rows[i]["envvalue"].ToString() + "',depaccount_no = '" + deptaccount_no + "',moneytype_code = '" + moneytype_code + "' ,entry_id = '" + state.SsUsername + "',entry_date = to_date('" + DwMain_mb_dead_normal.GetItemDateTime(1, "entry_date").ToString("dd/MM/yyyy", WebUtil.EN) + "', 'dd/mm/yyyy') where payoutslip_no = '" + assist_docno + "' and capital_year = '" + capital_year + "' and seq_no = " + seq_no + "";
                            ta2.Exe(sqlStr);
                        }
                        ta2.Commit();
                    }
                    catch { ta2.RollBack(); }

                    try { }
                    catch { }

                    LtServerMessage.Text = WebUtil.CompleteMessage("แก้ไขเรียบร้อย");
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            }
            ta2.Close();
        }
        public void LoadEnd_mb_dead_normal()
        {
            try
            {
                String membNo = DwUtil.GetString(DwMem, 1, "member_no", "");
                if (membNo != "")
                {
                    DwUtil.RetrieveDDDW(DwMain_mb_dead_normal, "deptaccount_no", "as_capital.pbl", membNo);
                    DataWindowChild dc = DwMain_mb_dead_normal.GetChild("deptaccount_no");
                    String accNo = DwUtil.GetString(DwMain_mb_dead_normal, 1, "deptaccount_no", "").Trim();
                    if (dc.RowCount > 0 && accNo == "")
                    {
                        DwMain_mb_dead_normal.SetItemString(1, "deptaccount_no", dc.GetItemString(1, "deptaccount_no"));
                    }
                }
            }
            catch { }

            tDwMain_mb_dead_normal.Eng2ThaiAllRow();
            tDwDetail_mb_dead_normal.Eng2ThaiAllRow();

            dt.Clear();
            ta.Close();

            DwMem.SaveDataCache();
            DwMain_mb_dead_normal.SaveDataCache();
            DwDetail_mb_dead_normal.SaveDataCache();
        }
        //------

        //---Function---
        private void NewClear_mb_dead_normal()
        {
            tDwMain_mb_dead_normal.Eng2ThaiAllRow();
            tDwDetail_mb_dead_normal.Eng2ThaiAllRow();


            DwMain_mb_dead_normal.Reset();
            DwMem.Reset();
            DwDetail_mb_dead_normal.Reset();

            DwMem.InsertRow(0);
            DwMain_mb_dead_normal.InsertRow(0);
            DwDetail_mb_dead_normal.InsertRow(0);

            DwMem.SaveDataCache();
            DwMain_mb_dead_normal.SaveDataCache();
            DwDetail_mb_dead_normal.SaveDataCache();

            DwMain_mb_dead_normal.SetItemDecimal(1, "req_status", 8);

            DwMain_mb_dead_normal.SetItemDateTime(1, "approve_date", state.SsWorkDate);
            DwMain_mb_dead_normal.SetItemDateTime(1, "req_date", state.SsWorkDate);
            DwMain_mb_dead_normal.SetItemDateTime(1, "entry_date", state.SsWorkDate);
        }
        private void RetreiveDwMem_mb_dead_normal()
        {
            String member_no;

            member_no = HfMemNo.Value;

            object[] args = new object[1];
            args[0] = member_no;

            DwMem.Reset();
            DwUtil.RetrieveDataWindow(DwMem, "as_capital.pbl", null, args);
            if (DwMem.RowCount < 1)
            {
                LtAlert.Text = "<script>Alert()</script>";
                return;
            }

            GetMemberDetail_mb_dead_normal();
        }
        private void GetMemberDetail_mb_dead_normal()
        {
            String member_no, prename_desc, memb_name, memb_surname, membgroup_code, membgroup_desc, membtype_code, membtype_desc, card_person;
            DateTime member_date, birth_date;
            member_no = HfMemNo.Value;
            Decimal salary_amount, member_age;
            int[] member_year = new int[3];

            DataStore Ds = new DataStore();
            Ds.LibraryList = "C:/GCOOP_ALL/CEN/GCOOP/Saving/DataWindow/app_assist/as_capital.pbl";
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
            salary_amount = Ds.GetItemDecimal(1, "salary_amount");
            card_person = Ds.GetItemString(1, "card_person");

            member_year = clsCalAge_mb_dead_normal(member_date, state.SsWorkDate);
            string member_age_range = member_year[2].ToString() + "." + member_year[1].ToString("00") + member_year[0].ToString("00");
            string s_member_year = member_year[2].ToString() + " ปี " + member_year[1].ToString() + " เดือน " + member_year[0].ToString() + " วัน";

            DwMain_mb_dead_normal.SetItemString(1, "member_years_disp", s_member_year);
            member_age = wcf.Busscom.of_cal_yearmonth(state.SsWsPass, birth_date, state.SsWorkDate);
            string s_member_age = member_age.ToString() + " ปี";
            DwMain_mb_dead_normal.SetItemString(1, "member_age", s_member_age);
            //
            DwMain_mb_dead_normal.SetItemString(1, "age_range", member_age_range);

            DwMem.SetItemString(1, "member_no", member_no);
            DwMem.SetItemString(1, "prename_desc", prename_desc);
            DwMem.SetItemString(1, "memb_name", memb_name);
            DwMem.SetItemString(1, "memb_surname", memb_surname);
            DwMem.SetItemString(1, "membgroup_desc", membgroup_desc);
            DwMem.SetItemString(1, "membgroup_code", membgroup_code);
            DwMem.SetItemString(1, "membtype_code", membtype_code);
            DwMem.SetItemString(1, "membtype_desc", membtype_desc);
            DwMem.SetItemString(1, "card_person", card_person);
            DwDetail_mb_dead_normal.SetItemString(1, "card_person", card_person);
            DwMain_mb_dead_normal.SetItemDateTime(1, "member_date", member_date);
            DwMain_mb_dead_normal.SetItemDateTime(1, "birth_date", birth_date);
            DwMain_mb_dead_normal.SetItemDecimal(1, "salary_amt", salary_amount);
            tDwMain_mb_dead_normal.Eng2ThaiAllRow();
        }
        public int[] clsCalAge_mb_dead_normal(DateTime d1, DateTime d2)
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
        private void Refresh_mb_dead_normal()
        {

        }
        private void ChangeAmt_mb_dead_normal()
        {
            String member_dead_tdate;
            DateTime member_dead_date;
            DateTime birth_date;
            Decimal age_range = 0, assist_amt = 0;
            member_no = HfMemNo.Value;
            age_range = Decimal.Parse(DwMain_mb_dead_normal.GetItemString(1, "age_range"));
            member_date = DwMain_mb_dead_normal.GetItemDateTime(1, "member_date");
            birth_date = DwMain_mb_dead_normal.GetItemDateTime(1, "birth_date");

            member_dead_tdate = DwDetail_mb_dead_normal.GetItemString(1, "member_dead_tdate");
            member_dead_date = DateTime.ParseExact(hdate1.Value, "ddMMyyyy", WebUtil.TH);

            DwDetail_mb_dead_normal.SetItemDateTime(1, "member_dead_date", member_dead_date);
            //DwDetail.SetItemDateTime(1, "member_age", member_age);
            //DwDetail.SetItemString(1, "member_dead_tdate", member_dead_tdate);//testdate ลองเพิ่ม

            tp = member_dead_date - member_date;
            Double mem_year = (tp.TotalDays / 365);
            if (mem_year < 1)
            {
                LtServerMessage.Text = WebUtil.WarningMessage("สมาชิกร้ายนี้ยังเป็นสมาชิกไม่ถึง 1 ปี จนถึงวันเสียชีวิต");
                //return;
            }

            //หาว่าเกิน 120 วันหรือไม่
            req_date = DwMain_mb_dead_normal.GetItemDateTime(1, "req_date");
            tp = req_date - member_dead_date;
            Double a = (tp.TotalDays);
            if (a > (120))//แก้ไขเพิ่มไป
            {
                LtServerMessage.Text = WebUtil.WarningMessage("สมาชิกรายนี้ยื่นคำขอเกินกว่า 120 วัน");
                //return;
            }

            try
            {
                birth_date = DwMain_mb_dead_normal.GetItemDateTime(1, "birth_date");
                tp = member_dead_date - birth_date;
                Double member_age = (tp.TotalDays);
            }
            catch { }

            sqlStr = "select * from asnsenvironmentvar where envgroup = 'member_dead_normal' and '" + age_range + "' between start_age and end_age";
            dt = WebUtil.QuerySdt(sqlStr);
            if (dt.Next())
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    assist_amt = assist_amt + Decimal.Parse(dt.Rows[i]["envvalue"].ToString());
                }
                DwDetail_mb_dead_normal.SetItemDecimal(1, "assist_amt", assist_amt);
            }
        }
        private void RetrieveDwMain_mb_dead_normal()
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

            DwMain_mb_dead_normal.Reset();
            DwUtil.RetrieveDataWindow(DwMain_mb_dead_normal, "as_capital.pbl", tDwMain_mb_dead_normal, args2);

            object[] args3 = new object[2];
            args3[0] = assist_docno;
            args3[1] = capital_year;

            DwDetail_mb_dead_normal.Reset();
            DwUtil.RetrieveDataWindow(DwDetail_mb_dead_normal, "as_capital.pbl", null, args3);

            RetrieveBankBranch_mb_dead_normal();
            DateTime member_date, birth_date;
            int[] member_year = new int[3];
            decimal member_age;
            //**
            req_date = DwMain_mb_dead_normal.GetItemDateTime(1, "req_date");
            member_date = DwMain_mb_dead_normal.GetItemDateTime(1, "member_date");
            birth_date = DwMain_mb_dead_normal.GetItemDateTime(1, "birth_date");

            member_year = clsCalAge_mb_dead_normal(member_date, req_date);

            string member_age_range = member_year[2].ToString() + "." + member_year[1].ToString("00") + member_year[0].ToString("00");
            string s_member_year = member_year[2].ToString() + " ปี " + member_year[1].ToString() + " เดือน " + member_year[0].ToString() + " วัน";

            DwMain_mb_dead_normal.SetItemString(1, "member_years_disp", s_member_year);
            member_age = wcf.Busscom.of_cal_yearmonth(state.SsWsPass, birth_date, req_date);
            string s_member_age = member_age.ToString() + " ปี";
            DwMain_mb_dead_normal.SetItemString(1, "member_age", s_member_age);
            DwMain_mb_dead_normal.SetItemString(1, "age_range", member_age_range);
        }
        private void RetrieveBankBranch_mb_dead_normal()
        {
            String bank;

            try
            {
                bank = DwMain_mb_dead_normal.GetItemString(1, "expense_bank");
            }
            catch { bank = ""; }

            DataWindowChild DcBankBranch = DwMain_mb_dead_normal.GetChild("expense_branch");
            DcBankBranch.SetTransaction(sqlca);
            DcBankBranch.Retrieve();
            DcBankBranch.SetFilter("bank_code = '" + bank + "'");
            DcBankBranch.Filter();
        }
        private void ChangeHeight_mb_dead_normal()
        {
            Decimal req_status;

            req_status = Convert.ToDecimal(HfReqSts.Value);

            if (req_status == -9 || req_status == -8)
            {
                DwMain_mb_dead_normal.Modify("datawindow.detail.Height=1004");
            }
            else
            {
                DwMain_mb_dead_normal.Modify("datawindow.detail.Height=732");
            }
        }
        private void Delete_mb_dead_normal()
        {
            String sqlStr, assist_docno;
            Decimal capital_year;
            Sta ta2 = new Sta(sqlca.ConnectionString);
            try
            {

                assist_docno = DwMain_mb_dead_normal.GetItemString(1, "assist_docno");
                capital_year = DwMain_mb_dead_normal.GetItemDecimal(1, "capital_year");

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
                NewClear_mb_dead_normal();
            }
            catch
            {
                //ta.RollBack();
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถลบได้");
            }

            ta2.Close();
        }
        private void ChangeAge_mb_dead_normal()
        {
            decimal member_age;
            int[] member_year = new int[3];
            DateTime Req_date;
            Req_date = DateTime.ParseExact(HdReqDate.Value, "ddMMyyyy", WebUtil.TH);
            DwMain_mb_dead_normal.SetItemDateTime(1, "req_date", Req_date);

            birth_date = DwMain_mb_dead_normal.GetItemDateTime(1, "birth_date");
            member_date = DwMain_mb_dead_normal.GetItemDateTime(1, "member_date");

            member_year = clsCalAge_mb_dead_normal(member_date, Req_date);
            string member_age_range = member_year[2].ToString() + "." + member_year[1].ToString("00") + member_year[0].ToString("00"); ;
            string s_member_year = member_year[2].ToString() + " ปี " + member_year[1].ToString() + " เดือน " + member_year[0].ToString() + " วัน";

            DwMain_mb_dead_normal.SetItemString(1, "member_years_disp", s_member_year);
            member_age = wcf.Busscom.of_cal_yearmonth(state.SsWsPass, birth_date, Req_date);
            string s_member_age = member_age.ToString() + " ปี";
            DwMain_mb_dead_normal.SetItemString(1, "member_age", s_member_age);

            DwMain_mb_dead_normal.SetItemString(1, "member_age", s_member_age);
            DwMain_mb_dead_normal.SetItemString(1, "age_range", member_age_range);
        }
        private Decimal GetSeqNo_mb_dead_normal()
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
        private void GetItemDwMain_mb_dead_normal()
        {
            member_no = DwMem.GetItemString(1, "member_no");
            membgroup_code = DwMem.GetItemString(1, "membgroup_code");
            req_status = DwMain_mb_dead_normal.GetItemDecimal(1, "req_status");
            capital_year = DwMain_mb_dead_normal.GetItemDecimal(1, "capital_year");
            birth_date = DwMain_mb_dead_normal.GetItemDateTime(1, "birth_date");
            assisttype_code = DwMain_mb_dead_normal.GetItemString(1, "assisttype_code");
            try
            {
                salary_amt = DwMain_mb_dead_normal.GetItemDecimal(1, "salary_amt");
            }
            catch { salary_amt = 0; }

            try
            {
                paytype_code = DwMain_mb_dead_normal.GetItemString(1, "paytype_code");
            }
            catch { paytype_code = ""; }
            try
            {
                pay_date = DwMain_mb_dead_normal.GetItemDateTime(1, "pay_date");
            }
            catch { }
            try
            {
                expense_bank = DwMain_mb_dead_normal.GetItemString(1, "expense_bank");
            }
            catch { expense_bank = ""; }
            try
            {
                expense_branch = DwMain_mb_dead_normal.GetItemString(1, "expense_branch");
            }
            catch { expense_branch = ""; }
            try
            {
                expense_accid = DwMain_mb_dead_normal.GetItemString(1, "expense_accid");
            }
            catch { expense_accid = ""; }

            try
            {
                cancel_date = DwMain_mb_dead_normal.GetItemDateTime(1, "cancel_date");
            }
            catch { }
            try
            {
                cancel_id = DwMain_mb_dead_normal.GetItemDecimal(1, "cancel_id");
            }
            catch { cancel_id = null; }
            try
            {
                remark_cancel = DwMain_mb_dead_normal.GetItemString(1, "remark");
            }
            catch { remark_cancel = ""; }
            try
            {
                birth_date = DwMain_mb_dead_normal.GetItemDateTime(1, "birth_date");
            }
            catch { }
        }
        private String GetLastDocNo_mb_dead_normal(Decimal capital_year)
        {

            capital_year = 2555; //เพิ่มเพื่อทดสอบ
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
        //------
        #endregion

        #endregion

    }
}