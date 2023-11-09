using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataLibrary;
using Sybase.DataWindow;

namespace Saving.Applications.app_assist
{
    public partial class w_sheet_as_senior_member_assist_2 : PageWebSheet, WebSheet
    {
        protected String postToFromAccid;
        protected String postDepptacount;
        protected String postSearch;

        protected Sdt dt;
        protected Sta ta;
        private String sqlStr, member_no, moneytype_code, account_no, assist_docno, approve_tdate, req_tdate, membgroup_code,
                       system_code, tofrom_accid, member_age, deptaccount_no, expense_bank, expense_branch, expense_accid;
        private String moneytype_code2, tofrom_accid2, deptaccount_no2, expense_bank2, expense_branch2;
        private Decimal assist_amt, req_status, age_range, salary_amount;
        private DateTime approve_date, req_date, capital_date, capital_date_end;
        private Decimal capital_year, capital_month;

        protected DwThDate tDwHead;
        protected DwThDate tDwMain;

        private String pbl = "as_capital.pbl";

        #region WebSheet Member
        public void InitJsPostBack()
        {
            postSearch = WebUtil.JsPostBack(this, "postSearch");
            postDepptacount = WebUtil.JsPostBack(this, "postDepptacount");
            postToFromAccid = WebUtil.JsPostBack(this, "postToFromAccid");

            tDwHead = new DwThDate(DwHead, this);
            tDwMain = new DwThDate(DwMain, this);

            tDwHead.Add("capital_date", "capital_tdate");
            tDwHead.Add("capital_date_end", "capital_edate");
            tDwMain.Add("approve_date", "approve_tdate");
            tDwMain.Add("req_date", "req_tdate");
            tDwMain.Add("birth_date", "birth_tdate");
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();
            ta = new Sta(sqlca.ConnectionString);
            sqlca = new DwTrans();
            sqlca.Connect();
            DwMain.SetTransaction(sqlca);

            if (!IsPostBack)
            {
                NewClear();
            }
            else
            {
                this.RestoreContextDw(DwHead);
                this.RestoreContextDw(DwMain);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "postDepptacount":
                    PostDepptacount();
                    break;
                case "postSearch":
                    PostSearch();
                    break;
                case "postToFromAccid":
                    PostToFromAccid();
                    break;
            }
        }

        public void SaveWebSheet()
        {
            try { capital_date = DwHead.GetItemDateTime(1, "capital_date"); }
            catch { capital_date = state.SsWorkDate; }
            capital_year = capital_date.Year + 543;


            for (int i = 1; i <= DwMain.RowCount; i++)
            {
                try { member_no = DwMain.GetItemString(i, "member_no"); }
                catch { member_no = ""; }
                try { req_status = DwMain.GetItemDecimal(i, "req_status"); }
                catch { req_status = -1; }
                if (req_status == 1)
                {
                    sqlStr = @" SELECT *
                                FROM asnreqmaster
                                WHERE member_no ='" + member_no + @"'
                                AND capital_year ='" + capital_year + @"'
                                AND assisttype_code= '71'
                              ";
                    dt = ta.Query(sqlStr);

                    if (!dt.Next())
                    {
                        #region เตรียมข้อมูล เพื่อบันทึก
                        try { approve_date = DwMain.GetItemDateTime(i, "approve_date"); }
                        catch { approve_date = state.SsWorkDate; }
                        approve_tdate = approve_date.ToString("dd/MM/yyyy", WebUtil.EN);
                        try { req_date = DwMain.GetItemDateTime(i, "req_date"); }
                        catch { req_date = state.SsWorkDate; }
                        req_tdate = req_date.ToString("dd/MM/yyyy", WebUtil.EN);
                        try { moneytype_code = DwMain.GetItemString(i, "moneytype_code"); }
                        catch { moneytype_code = ""; }
                        try { account_no = DwMain.GetItemString(i, "deptaccount_no"); }
                        catch { account_no = ""; }
                        //try { req_status = DwMain.GetItemDecimal(i, "req_status"); }
                        try { req_status = 8; }
                        catch { req_status = -1; }
                        try { assist_docno = DwMain.GetItemString(i, "assist_docno"); }
                        catch { assist_docno = ""; }
                        try { assist_amt = DwMain.GetItemDecimal(i, "assist_amt"); }
                        catch { assist_amt = 0; }
                        try { member_age = DwMain.GetItemString(i, "age_range"); }
                        catch { member_age = "0"; }
                        age_range = Convert.ToDecimal(member_age) * 12; //แปลงปีเป็นเดือน
                        try { salary_amount = DwMain.GetItemDecimal(i, "salary_amount"); }
                        catch { salary_amount = 0; }
                        try { membgroup_code = DwMain.GetItemString(i, "membgroup_code"); }
                        catch { membgroup_code = ""; }
                        try { expense_accid = DwMain.GetItemString(i, "expense_accid").Trim(); }
                        catch { expense_accid = ""; }
                        try { tofrom_accid = DwMain.GetItemString(i, "tofrom_accid").Trim(); }
                        catch { tofrom_accid = ""; }
                        try { expense_bank = DwMain.GetItemString(i, "expense_bank"); }
                        catch { expense_bank = ""; }
                        try { expense_branch = DwMain.GetItemString(i, "expense_branch"); }
                        catch { expense_branch = ""; }
                        #endregion

                        #region บันทึกข้อมูล ตาราง asnreqmaster, asnreqcapitaldet, asnmemsalary, asndoccontrol, asnslippayout (สร้างใบคำขอ + อนุมัติ)
                        try
                        {
                            sqlStr = @"INSERT INTO asnreqmaster (
                                                assist_docno,                           capital_year,                       member_no,                      assist_amt,
                                                  req_status,                               req_date,                    approve_date,                        pay_date,
                                              moneytype_code,                         deptaccount_no,                   posttovc_flag,                      pay_status,
                                              approve_status,                        assisttype_code,                         coop_id,                    tofrom_accid,
                                                 approve_amt,                           expense_bank,                  expense_branch,              expense_accid
                                                    )
                                  VALUES(
                                     '" + assist_docno + @"',                '" + capital_year + @"',            '" + member_no + @"',             " + assist_amt + @",
                                       '" + req_status + @"',to_date('" + req_tdate + @"','dd/MM/yyyy'),to_date('" + approve_tdate + @"','dd/MM/yyyy'),to_date('" + approve_tdate + @"','dd/MM/yyyy'),
                                   '" + moneytype_code + @"',                  '" + account_no + @"',                            null,                             '8',
                                                         '8',                                   '71',       '" + state.SsCoopId + @"',         '" + tofrom_accid + @"',
                                         " + assist_amt + @",                '" + expense_bank + @"',       '" + expense_branch + @"',          '" + expense_accid + @"'
                                        )";
                            ta.Exe(sqlStr);

                            sqlStr = @"INSERT INTO asnreqcapitaldet (
                                                capital_year,                           assist_docno,                      assist_amt,                      member_age
                                                        )
                                  VALUES(
                                     '" + capital_year + @"',                '" + assist_docno + @"',             " + assist_amt + @",           '" + member_age + @"'
                                        )";
                            ta.Exe(sqlStr);

                            sqlStr = @"INSERT INTO asnmemsalary (
                                                capital_year,                             member_no,                     assist_docno,                     entry_date,
                                                  salary_amt,                        membgroup_code
                                                     )
                                  VALUES(
                                     '" + capital_year + @"',                  '" + member_no + @"',          '" + assist_docno + @"',to_date('" + approve_tdate + @"','dd/MM/yyyy'),
                                    '" + salary_amount + @"',             '" + membgroup_code + @"'
                                        )";
                            ta.Exe(sqlStr);

                            sqlStr = @"UPDATE  asndoccontrol
                                                       SET     last_docno   = '" + WebUtil.Right(assist_docno, 6) + @"'
                                                       WHERE   doc_prefix   = 'SM' and
                                                               doc_year = '" + capital_year + "'";
                            ta.Exe(sqlStr);

                            sqlStr = @"SELECT * 
                                   FROM asnucfpaytype 
                                   WHERE paytype_code = '" + moneytype_code + @"'";
                            dt = ta.Query(sqlStr);
                            if (dt.Next())
                            {
                                moneytype_code = dt.GetString("moneytype_code");
                            }

                            sqlStr = @"SELECT * 
                                   FROM asnsenvironmentvar 
                                   WHERE envgroup = 'senior_member_assist' 
                                   AND " + age_range + @" BETWEEN start_age AND end_age";
                            dt = ta.Query(sqlStr);
                            if (dt.Next())
                            {
                                system_code = dt.GetString("system_code");
                                tofrom_accid = dt.GetString("tofrom_accid");
                            }

                            sqlStr = @"INSERT INTO asnslippayout (
                                                payoutslip_no,                            member_no,                        slip_date,                   operate_date,
                                                   payout_amt,                          slip_status,                    depaccount_no,                assisttype_code,
                                               moneytype_code,                             entry_id,                       entry_date,                         seq_no,
                                                 capital_year,                        sliptype_code,                     tofrom_accid
                                                     ) 
                                  VALUES(
                                      '" + assist_docno + @"',                 '" + member_no + @"',to_date('" + approve_tdate + @"','dd/MM/yyyy'),to_date('" + approve_tdate + @"','dd/MM/yyyy'),
                                        '" + assist_amt + @"',                                  '8',           '" + expense_accid + @"',                            '71',
                                    '" + moneytype_code + @"',          '" + state.SsUsername + @"',to_date('" + approve_tdate + @"','dd/MM/yyyy'),        " + i + @",
                                      '" + capital_year + @"',               '" + system_code + @"',         '" + tofrom_accid + @"'
                                        )";
                            ta.Exe(sqlStr);

                            LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกเรียบร้อย");
                        }
                        catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage("บันทึกข้อมูลไม่สำเร็จเนื่องจาก" + ex.ToString()); }
                        #endregion
                    }
                    else
                    {
                        #region เตรียมข้อมูล เพื่อบันทึก
                        try { approve_date = DwMain.GetItemDateTime(i, "approve_date"); }
                        catch { approve_date = state.SsWorkDate; }
                        approve_tdate = approve_date.ToString("dd/MM/yyyy", WebUtil.EN);
                        try { req_date = DwMain.GetItemDateTime(i, "req_date"); }
                        catch { req_date = state.SsWorkDate; }
                        req_tdate = req_date.ToString("dd/MM/yyyy", WebUtil.EN);
                        try { moneytype_code = DwMain.GetItemString(i, "moneytype_code"); }
                        catch { moneytype_code = ""; }
                        try { account_no = DwMain.GetItemString(i, "deptaccount_no"); }
                        catch { account_no = ""; }
                        try { req_status = 8; }
                        catch { req_status = -1; }
                        try { assist_docno = dt.GetString("assist_docno").Trim(); }
                        catch { assist_docno = ""; }
                        try { assist_amt = DwMain.GetItemDecimal(i, "assist_amt"); }
                        catch { assist_amt = 0; }
                        try { member_age = DwMain.GetItemString(i, "age_range"); }
                        catch { member_age = "0"; }
                        age_range = Convert.ToDecimal(member_age) * 12; //แปลงปีเป็นเดือน
                        try { salary_amount = DwMain.GetItemDecimal(i, "salary_amount"); }
                        catch { salary_amount = 0; }
                        try { membgroup_code = DwMain.GetItemString(i, "membgroup_code"); }
                        catch { membgroup_code = ""; }
                        try { expense_accid = DwMain.GetItemString(i, "expense_accid").Trim(); }
                        catch { expense_accid = ""; }
                        try { tofrom_accid = DwMain.GetItemString(i, "tofrom_accid").Trim(); }
                        catch { tofrom_accid = ""; }
                        try { expense_bank = DwMain.GetItemString(i, "expense_bank"); }
                        catch { expense_bank = ""; }
                        try { expense_branch = DwMain.GetItemString(i, "expense_branch"); }
                        catch { expense_branch = ""; }
                        #endregion

                        #region บันทึกข้อมูล ตาราง asnreqmaster, asnreqcapitaldet, asnmemsalary, asndoccontrol, asnslippayout (สร้างใบคำขอ + อนุมัติ)
                        try
                        {
                            sqlStr = @"UPDATE  asnreqmaster
	                                SET     assist_amt  =  " + assist_amt + @",
			                                req_date = to_date('" + req_tdate + @"', 'dd/MM/yyyy'),
			                                approve_date = to_date('" + approve_tdate + @"','dd/MM/yyyy'),
			                                pay_date = to_date('" + approve_tdate + @"','dd/MM/yyyy'),
			                                moneytype_code = '" + moneytype_code + @"',
			                                tofrom_accid = '" + tofrom_accid + @"',
			                                approve_amt = " + assist_amt + @", 
			                                expense_bank = '" + expense_bank + @"', 
			                                expense_branch = '" + expense_branch + @"',
			                                expense_accid = '" + expense_accid + @"'
                                    WHERE   assist_docno   = '" + assist_docno + @"' 
		                                    and capital_year = '" + capital_year + @"' 
		                                    and member_no = '" + member_no + "' ";
                            ta.Exe(sqlStr);

                            sqlStr = @"UPDATE  asnreqcapitaldet
	                                    SET   assist_amt  =  " + assist_amt + @",
			                                member_age =  '" + member_age + @"'
                                        WHERE   assist_docno   = '" + assist_docno + @"' 
                                            and capital_year = '" + capital_year + @"' ";
                            ta.Exe(sqlStr);

                            sqlStr = @"UPDATE  asnmemsalary
                                    SET     entry_date  =  to_date('" + approve_tdate + @"','dd/MM/yyyy'),
	                                        salary_amt =   '" + salary_amount + @"', 
	                                        membgroup_code = '" + membgroup_code + @"'
                                    WHERE   assist_docno   = '" + assist_docno + @"' 
                                            and capital_year = '" + capital_year + @"' 
                                            and member_no = '" + member_no + "' ";
                            ta.Exe(sqlStr);

                            sqlStr = @"SELECT * 
                                            FROM asnucfpaytype 
                                            WHERE paytype_code = '" + moneytype_code + @"'";
                            dt = ta.Query(sqlStr);
                            if (dt.Next())
                            {
                                moneytype_code = dt.GetString("moneytype_code");
                            }
                            sqlStr = @"SELECT * 
                                            FROM asnsenvironmentvar 
                                            WHERE envgroup = 'senior_member_assist' 
                                            AND " + age_range + @" BETWEEN start_age AND end_age";
                            dt = ta.Query(sqlStr);
                            if (dt.Next())
                            {
                                system_code = dt.GetString("system_code");
                                tofrom_accid = dt.GetString("tofrom_accid");
                            }

                            sqlStr = @"UPDATE  asnslippayout
                                    SET   slip_date  =  to_date('" + approve_tdate + @"','dd/MM/yyyy'),
	                                    operate_date =   to_date('" + approve_tdate + @"','dd/MM/yyyy'),
	                                    payout_amt = '" + assist_amt + @"',
	                                    depaccount_no = '" + account_no + @"',    
	                                    moneytype_code =  '" + moneytype_code + @"', 
	                                    entry_id = '" + state.SsUsername + @"',
	                                    entry_date =  to_date('" + approve_tdate + @"','dd/MM/yyyy'),
	                                    sliptype_code =  '" + system_code + @"', 
	                                    tofrom_accid = '" + tofrom_accid + @"'
                                    WHERE   payoutslip_no   = '" + assist_docno + @"' 
                                    and capital_year = '" + capital_year + @"' 
                                    and member_no = '" + member_no + "' ";
                            ta.Exe(sqlStr);
                            LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกเรียบร้อย");
                        }
                        catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage("บันทึกข้อมูลไม่สำเร็จเนื่องจาก" + ex.ToString()); }
                        #endregion
                    }
                }
            }
        }

        public void WebSheetLoadEnd()
        {
            DwHead.SaveDataCache();
            DwMain.SaveDataCache();
            tDwHead.Eng2ThaiAllRow();
            tDwMain.Eng2ThaiAllRow();
            ta.Close();
        }
        #endregion

        #region Function
        /// <summary>
        /// เริ่มหน้าจอใหม่ 
        /// โดยจะค้นหารายชื่อสมาชิกอาวุโส ที่มีอายุจริง >= 61 ปี
        /// </summary>
        protected void NewClear()
        {
            DwHead.Reset();
            DwMain.Reset();
            DwHead.InsertRow(0);
            DwMain.InsertRow(0);
            capital_date = state.SsWorkDate;
            capital_date_end = state.SsWorkDate;
            capital_year = capital_date.Year + 543;
            DwHead.SetItemDecimal(1, "capital_year", capital_year);         //พ.ศ. :
            DwHead.SetItemDecimal(1, "capital_month", capital_date.Month);  //เดือน :
            //DwMain.Retrieve(capital_year);
            ChkAll.Checked = false;
            DwHead.SetItemDateTime(1, "capital_date", capital_date);
            DwHead.SetItemDateTime(1, "capital_date_end", capital_date_end);

            string mm_date_start = (capital_date.Day).ToString("00") + "/" + (capital_date.Month).ToString("00");
            string mm_date_end = (capital_date_end.Day).ToString("00") + "/" + (capital_date_end.Month).ToString("00");
            return;//Return ออกจากฟังก์ชัน  NewClear()
            DwUtil.RetrieveDDDW(DwHead, "capital_year", pbl, null);
            DwUtil.RetrieveDDDW(DwMain, "moneytype_code", pbl, null);
            DwUtil.RetrieveDataWindow(DwMain, pbl, null, (capital_date.Year - 61), capital_date.Month);
            GetMoney();
            GetLastDocNo(capital_year);
            int rowCount = DwMain.RowCount;
            lblRowCount.Text = " จำนวนทั้งสิ้น  " + rowCount.ToString("#,##0") + "  ราย";
            for (int i = 1; i <= rowCount; i++)
            {
                try { member_no = DwMain.GetItemString(i, "member_no"); }
                catch { member_no = ""; }
                DwMain.SetItemDateTime(i, "approve_date", state.SsWorkDate);
                DwMain.SetItemDateTime(i, "req_date", state.SsWorkDate);
                DwMain.SetItemDecimal(i, "assist_amt", assist_amt);
                DwMain.SetItemDecimal(i, "approve_amt", assist_amt);
                DwMain.SetItemString(i, "assist_docno", assist_docno);
                assist_docno = "SM" + (Convert.ToDecimal(WebUtil.Right(assist_docno, 6)) + 1).ToString("000000");

                SetFromYear(i);

                decimal max_capital_year = GetMax_Number("select max(CAPITAL_YEAR) as max_value from ASNREQMASTER where member_no = '" + member_no + "'");

                string sqlf_accid = @"SELECT 
                                        CAPITAL_YEAR,    
                                        DEPTACCOUNT_NO,
                                        MONEYTYPE_CODE,
                                        EXPENSE_BANK,
                                        EXPENSE_BRANCH,
                                        EXPENSE_ACCID
                                        FROM ASNREQMASTER  
                                        WHERE ( ASNREQMASTER.ASSISTTYPE_CODE (+) = '71' ) 
                                        AND ( ASNREQMASTER.CAPITAL_YEAR = {0} )  
                                        AND ( ASNREQMASTER.MEMBER_NO = {1} )";
                sqlf_accid = WebUtil.SQLFormat(sqlf_accid, max_capital_year, member_no);
                Sdt dtY = WebUtil.QuerySdt(sqlf_accid);

                string bank_accid = "", moneytype_code = "", expense_bank = "", expense_branch = "";

                if (dtY.Next())
                {
                    moneytype_code = dtY.GetString("MONEYTYPE_CODE");
                    expense_bank = dtY.GetString("EXPENSE_BANK");
                    expense_branch = dtY.GetString("EXPENSE_BRANCH");
                    bank_accid = dtY.GetString("EXPENSE_ACCID");

                    DwMain.SetItemString(i, "moneytype_code", moneytype_code);
                    DwMain.SetItemString(i, "expense_bank", expense_bank);
                    DwMain.SetItemString(i, "expense_branch", expense_branch);
                    DwMain.SetItemString(i, "tofrom_accid", "31035100");
                }

                try
                {
                    DwMain.GetItemString(i, "expense_accid");
                }
                catch
                {
                    DwMain.SetItemString(i, "moneytype_code", "TRN");

                    string sqlf_bank_accid = "select bank_accid from mbmembmoneytr where member_no = {0} and trtype_code = 'DVAV1'";
                    sqlf_bank_accid = WebUtil.SQLFormat(sqlf_bank_accid, member_no);
                    Sdt dtBN = WebUtil.QuerySdt(sqlf_bank_accid);

                    if (dtBN.Next())
                    {
                        bank_accid = dtBN.GetString("bank_accid");
                    }

                    DwMain.SetItemString(i, "expense_accid", bank_accid);
                }
            }
        }

        protected int[] clsCalAge(DateTime d1, DateTime d2)
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

        /// <summary>
        /// Set เลขบัญชีที่รับมาจาก Dialog
        /// </summary>
        protected void PostDepptacount()
        {
            int row;
            string deptaccount_no;

            row = Convert.ToInt32(HfRow.Value);
            deptaccount_no = Hfdeptaccount_no.Value;

            DwMain.SetItemString(row, "deptaccount_no", deptaccount_no);
            //DwMain.SetItemString(row, "paytype_code", "04");
        }

        /// <summary>
        /// กำหนด Event ที่เกิดจาก Check box กรณี เลือกทั้งหมด
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CheckAllChanged(object sender, EventArgs e)
        {
            int rowcount = DwMain.RowCount;
            int i;
            if (ChkAll.Checked == true)
            {
                for (i = 1; i <= rowcount; i++)
                {
                    DwMain.SetItemDecimal(i, "req_status", 1);
                }
            }
            else
            {
                for (i = 1; i <= rowcount; i++)
                {
                    DwMain.SetItemDecimal(i, "req_status", -1);
                }
            }
        }

        /// <summary>
        /// ดึงจำนวนเงินมาจากตารางค่าคงที่
        /// </summary>
        protected void GetMoney()
        {
            sqlStr = @" SELECT envvalue 
                        FROM asnsenvironmentvar
                        WHERE envcode = 'senior_member_assist_5'
                     ";
            dt = ta.Query(sqlStr);
            if (dt.Next())
            {
                assist_amt = Convert.ToDecimal(dt.GetString("envvalue"));
            }
        }

        protected void ChangeAccountNo(String moneytype_code)
        {//HC paytype_code
            #region GetDeptAccountNo(ไม่ใช้ 7/10/2556)
            //            switch (paytype_code)
            //            {
            //                case "05"://บัญชี ธ.ก.ส.
            //                    sqlStr = @"SELECT expense_accid 
            //                           FROM mbmembmaster 
            //                           WHERE member_no = '" + member_no + @"'
            //                          ";
            //                    dt = ta.Query(sqlStr);
            //                    if (dt.Next())
            //                    {
            //                        account_no = dt.GetString("expense_accid");
            //                        Hfpaytype_code.Value = "05";
            //                    }
            //                    break;
            //            }
            //            //บัญชี สหกรณ์
            //            if (account_no == "" || account_no == null)
            //            {
            //                //บัญชีออมทรัพย์
            //                sqlStr = @" SELECT dpdeptmaster.deptaccount_no
            //                            FROM dpdeptmaster
            //                                INNER JOIN dpdepttype
            //                                ON dpdeptmaster.coop_id = dpdepttype.coop_id and
            //                                   dpdeptmaster.depttype_code = dpdepttype.depttype_code
            //                            WHERE dpdeptmaster.member_no = '" + member_no + @"'
            //                            AND   dpdeptmaster.deptclose_status not in (1)
            //                            AND   dpdeptmaster.depttype_code in ('01')
            //                          ";
            //                dt = ta.Query(sqlStr);
            //                if (dt.Next())
            //                {
            //                    account_no = dt.GetString("deptaccount_no");
            //                    Hfpaytype_code.Value = "04";
            //                }
            //            }
            //            if (account_no == "" || account_no == null)
            //            {
            //                //เงินฝากออมทรัพย์พิเศษ
            //                sqlStr = @" SELECT dpdeptmaster.deptaccount_no
            //                            FROM dpdeptmaster
            //                                INNER JOIN dpdepttype
            //                                ON dpdeptmaster.coop_id = dpdepttype.coop_id and
            //                                   dpdeptmaster.depttype_code = dpdepttype.depttype_code
            //                            WHERE dpdeptmaster.member_no = '" + member_no + @"'
            //                            AND   dpdeptmaster.deptclose_status not in (1)
            //                            AND   dpdeptmaster.depttype_code in ('07')
            //                          ";
            //                dt = ta.Query(sqlStr);
            //                if (dt.Next())
            //                {
            //                    account_no = dt.GetString("deptaccount_no");
            //                    Hfpaytype_code.Value = "04";
            //                }
            //            }
            //            if (account_no == "" || account_no == null)
            //            {
            //                //เงินฝากออมทรัพย์เกษียณทวีสุข
            //                sqlStr = @" SELECT dpdeptmaster.deptaccount_no
            //                            FROM dpdeptmaster
            //                                INNER JOIN dpdepttype
            //                                ON dpdeptmaster.coop_id = dpdepttype.coop_id and
            //                                   dpdeptmaster.depttype_code = dpdepttype.depttype_code
            //                            WHERE dpdeptmaster.member_no = '" + member_no + @"'
            //                            AND   dpdeptmaster.deptclose_status not in (1)
            //                            AND   dpdeptmaster.depttype_code in ('29')
            //                          ";
            //                dt = ta.Query(sqlStr);
            //                if (dt.Next())
            //                {
            //                    account_no = dt.GetString("deptaccount_no");
            //                    Hfpaytype_code.Value = "04";
            //                }
            //            }
            //            if (account_no == "" || account_no == null)
            //            {
            //                //เงินฝากออมทรัพย์พิเศษสาขา
            //                sqlStr = @" SELECT dpdeptmaster.deptaccount_no
            //                            FROM dpdeptmaster
            //                                INNER JOIN dpdepttype
            //                                ON dpdeptmaster.coop_id = dpdepttype.coop_id and
            //                                   dpdeptmaster.depttype_code = dpdepttype.depttype_code
            //                            WHERE dpdeptmaster.member_no = '" + member_no + @"'
            //                            AND   dpdeptmaster.deptclose_status not in (1)
            //                            AND   dpdeptmaster.depttype_code in ('31')
            //                          ";
            //                dt = ta.Query(sqlStr);
            //                if (dt.Next())
            //                {
            //                    account_no = dt.GetString("deptaccount_no");
            //                    Hfpaytype_code.Value = "04";
            //                }
            //            }
            //            if (account_no == "" || account_no == null)
            //            {
            //                //เงินฝากออมทรัพย์วัยยิ้ม
            //                sqlStr = @" SELECT dpdeptmaster.deptaccount_no
            //                            FROM dpdeptmaster
            //                                INNER JOIN dpdepttype
            //                                ON dpdeptmaster.coop_id = dpdepttype.coop_id and
            //                                   dpdeptmaster.depttype_code = dpdepttype.depttype_code
            //                            WHERE dpdeptmaster.member_no = '" + member_no + @"'
            //                            AND   dpdeptmaster.deptclose_status not in (1)
            //                            AND   dpdeptmaster.depttype_code in ('63')
            //                          ";
            //                dt = ta.Query(sqlStr);
            //                if (dt.Next())
            //                {
            //                    account_no = dt.GetString("deptaccount_no");
            //                    Hfpaytype_code.Value = "04";
            //                }
            //            }
            #endregion
        }

        /// <summary>
        /// กรณีกดปุ่มค้นหา จะเกิด event ที่นี่
        /// </summary>
        protected void PostSearch()
        {
            //try
            //{
            //    ChkAll.Checked = false;
            //    String date = Hfcapital_tdate.Value;
            //    int day = Convert.ToInt16(date.Substring(0, 2));
            //    int month = Convert.ToInt16(date.Substring(2, 2));
            //    int year = Convert.ToInt16(date.Substring(4, 4)) - 543;
            //    capital_date = new DateTime(year, month, day);
            //}
            //catch { capital_date = state.SsWorkDate; }
            //try
            //{
            //    String edate = Hfcapital_edate.Value;
            //    int day = Convert.ToInt16(edate.Substring(0, 2));
            //    int month = Convert.ToInt16(edate.Substring(2, 2));
            //    int year = Convert.ToInt16(edate.Substring(4, 4)) - 543;
            //    capital_date_end = new DateTime(year, month, day);
            //}
            //catch { capital_date_end = state.SsWorkDate; }

            //string mm_date_start = (capital_date.Day).ToString("00") + "/" + (capital_date.Month).ToString("00");
            //string mm_date_end = (capital_date_end.Day).ToString("00") + "/" + (capital_date_end.Month).ToString("00");
            //ดึงค่าคงที่ของสมาชิกที่มีอายุตั้งแต่ 61 ปีขึ้นไป มาเซตค่าให้กับตัวแปร start_age
            Int32 start_age = 0;
            String sqlStartAge = @" SELECT 	(start_age/12) as start_age
                                    FROM 	asnsenvironmentvar 
                                    WHERE 	envgroup = 'senior_member_assist' ";
            Sdt dt = WebUtil.QuerySdt(sqlStartAge);
            if (dt.Next()) { start_age = Convert.ToInt32(dt.GetString("start_age")); }
            capital_year = DwHead.GetItemDecimal(1, "capital_year") - 543;  //พ.ศ. :
            capital_month = DwHead.GetItemDecimal(1, "capital_month");      //เดือน :
            //DateTime.ParseExact(hdate1.Value, "dd/MM/yyyy", WebUtil.EN);
            capital_date = DateTime.ParseExact(1.ToString("00") + "/" + capital_month.ToString("00") + "/" + capital_year, "dd/MM/yyyy", WebUtil.EN);
            capital_date_end = DateTime.ParseExact(((DateTime.DaysInMonth(Convert.ToInt32(capital_year), Convert.ToInt32(capital_month))).ToString("00") + "/" + capital_month.ToString("00") + "/" + capital_year), "dd/MM/yyyy", WebUtil.EN);
            ChkAll.Checked = false;
            DwHead.SetItemDateTime(1, "capital_date", capital_date);
            DwHead.SetItemDateTime(1, "capital_date_end", capital_date_end);
            DwUtil.RetrieveDDDW(DwHead, "capital_year", pbl, null);
            DwUtil.RetrieveDDDW(DwMain, "moneytype_code", pbl, null);
            DwUtil.RetrieveDDDW(DwMain, "expense_branch", pbl, null);
            DwUtil.RetrieveDataWindow(DwMain, pbl, tDwMain, Convert.ToInt32(capital_year - start_age), Convert.ToInt32(capital_month));
            capital_year += 543;
            GetMoney();
            //GetLastDocNo(capital_year);
            int rowCount = DwMain.RowCount;
            lblRowCount.Text = " จำนวนทั้งสิ้น  " + rowCount.ToString("#,##0") + "  ราย";
            for (int i = 1; i <= rowCount; i++)
            {
                try { member_no = DwMain.GetItemString(i, "member_no"); }
                catch { member_no = ""; }
                DwMain.SetItemDateTime(i, "approve_date", state.SsWorkDate);
                DwMain.SetItemDateTime(i, "req_date", state.SsWorkDate);
                DwMain.SetItemDecimal(i, "assist_amt", assist_amt);
                DwMain.SetItemDecimal(i, "approve_amt", assist_amt);

                //                decimal max_capital_year = GetMax_Number("select max(CAPITAL_YEAR) as max_value from ASNREQMASTER where member_no = '" + member_no + "'");

                //                string sqlf_accid = @"SELECT 
                //                                        CAPITAL_YEAR,    
                //                                        DEPTACCOUNT_NO,
                //                                        MONEYTYPE_CODE,
                //                                        EXPENSE_BANK,
                //                                        EXPENSE_BRANCH,
                //                                        EXPENSE_ACCID
                //                                        FROM ASNREQMASTER  
                //                                        WHERE ( ASNREQMASTER.ASSISTTYPE_CODE (+) = '71' ) 
                //                                        AND ( ASNREQMASTER.CAPITAL_YEAR = {0} )  
                //                                        AND ( ASNREQMASTER.MEMBER_NO = {1} )";
                //                sqlf_accid = WebUtil.SQLFormat(sqlf_accid, max_capital_year, member_no);
                //                Sdt dtY = WebUtil.QuerySdt(sqlf_accid);

                string bank_accid = "", moneytype_code = "", expense_bank = "", expense_branch = "", tofrom_accid = "";

                //                if (dtY.Next())
                //                {
                //                    moneytype_code = dtY.GetString("MONEYTYPE_CODE");
                //                    expense_bank = dtY.GetString("EXPENSE_BANK");
                //                    expense_branch = dtY.GetString("EXPENSE_BRANCH");
                //                    bank_accid = dtY.GetString("EXPENSE_ACCID");

                //                    DwMain.SetItemString(i, "moneytype_code", moneytype_code);
                //                    DwMain.SetItemString(i, "expense_bank", expense_bank);
                //                    DwMain.SetItemString(i, "expense_branch", expense_branch);
                //                    DwMain.SetItemString(i, "tofrom_accid", "31035100");
                //                }

                //                try
                //                {
                //                    DwMain.GetItemString(i, "expense_accid");
                //                }
                //                catch
                //                {
                //DwMain.SetItemString(i, "moneytype_code", "TRN");
                //ดึงเลขที่บัญชีมาจากรหัส ASST1: การจ่ายเงินปันผล กรณีไม่มีในปันผลให้ดึงมาจาก เลขที่บัญชีหลักสมาชิก
                string sqlf_bank_accid = @"select   moneytype_code,
		                                            bank_code,
		                                            bank_branch,
		                                            bank_accid 
                                            from 	mbmembmoneytr
                                            where   member_no   = {0}
                                            and     coop_id     = {1} 
                                            and     trtype_code = 'ASST1'
                                            union
                                            select  m.expense_code as moneytype_code,
		  	                                        m.expense_bank as bank_code,
		  	                                        m.expense_branch as bank_branch,
		  	                                        m.expense_accid as bank_accid 
                                            from 	mbmembmaster m
                                            where	m.member_no	= {0}
                                            and     m.coop_id   = {1}
                                            and		not exists (select 1 from mbmembmoneytr mtr where mtr.member_no = m.member_no and mtr.coop_id = m.coop_id and mtr.trtype_code = 'ASST1') ";
                sqlf_bank_accid = WebUtil.SQLFormat(sqlf_bank_accid, member_no, state.SsCoopId);
                Sdt dtBN = WebUtil.QuerySdt(sqlf_bank_accid);
                if (dtBN.Next())
                {
                    moneytype_code = dtBN.GetString("moneytype_code").Trim();
                    expense_bank = dtBN.GetString("bank_code").Trim();
                    expense_branch = dtBN.GetString("bank_branch").Trim();
                    bank_accid = dtBN.GetString("bank_accid").Trim();
                    //ดึงคู่บัญชี มาเซตค่าให้กับตัวแปร tofrom_accid
                    String sqlAccId = @"SELECT 	tofrom_accid 
                                        FROM 	asnsenvironmentvar 
                                        WHERE 	envgroup = 'senior_member_assist' ";
                    Sdt dtAcc = WebUtil.QuerySdt(sqlAccId);
                    if (dtAcc.Next()) { tofrom_accid = dtAcc.GetString("tofrom_accid").Trim(); }
                }
                DwMain.SetItemString(i, "moneytype_code", moneytype_code);
                //เช็คว่า ประเภทการชำระเป็น CBT: โอนธนาคาร ให้เซตค่าเริ่มต้น ธนาคาร: = 034 
                if (moneytype_code == "CBT") { DwMain.SetItemString(i, "expense_bank", "034"); }
                else { DwMain.SetItemString(i, "expense_bank", expense_bank); }
                DwMain.SetItemString(i, "expense_branch", expense_branch);
                DwMain.SetItemString(i, "expense_accid", bank_accid);
                DwMain.SetItemString(i, "tofrom_accid", tofrom_accid);
                //}
                assist_docno = GetLastDocNo(capital_year, i);
                DwMain.SetItemString(i, "assist_docno", assist_docno);
                //assist_docno = "SM" + (Convert.ToDecimal(WebUtil.Right(assist_docno, 6)) + 1).ToString("000000");
                
                SetFromYear(i);

                DwMain.SetItemDecimal(i, "req_status", 8);
            }
        }

        /// <summary>
        /// รับเลขเอกสารล่าสุด โดยการดึงเลขเอกสารที่มากที่สุดจากตาราง asndoccontrol ตามปีที่ได้รับ แล้วนำมา +1
        /// </summary>
        /// <param name="capital_year">ปีที่ต้องการดึงเลขเอกสาร</param>
        /// <returns></returns>
        /*        protected String GetLastDocNo(Decimal capital_year)
                {
                    try
                    {
                        sqlStr = @"SELECT last_docno 
                                       FROM   asndoccontrol
                                       WHERE  doc_prefix = 'SM' and
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

                            assist_docno = "SM" + assist_docno;
                            assist_docno = assist_docno.Trim();
                        }
                        catch
                        {
                            assist_docno = "SM000001";
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
                            WHERE  doc_prefix   = 'SM' 
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
                    assist_docno = "SM" + assist_docno;
                    assist_docno = assist_docno.Trim();
                }
                catch
                {
                    assist_docno = "SM" + doc_year + "000001";
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }

            return assist_docno;
        }
        private String GetLastDocNo(Decimal capital_year, int seq_no)
        {
            String doc_year = "";
            try
            {
                doc_year = WebUtil.Right(capital_year.ToString(), 2);
                sqlStr = @" SELECT substr( doc_year, 3, 2 ) || last_docno as last_docno 
                            FROM   asndoccontrol
                            WHERE  doc_prefix   = 'SM' 
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
                        assist_docno = "000000" + Convert.ToString(Convert.ToInt32(assist_docno) + seq_no);
                        assist_docno = WebUtil.Right(assist_docno, 8);
                    }
                    assist_docno = "SM" + assist_docno;
                    assist_docno = assist_docno.Trim();
                }
                catch
                {
                    assist_docno = "SM" + doc_year + "000001";
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }

            return assist_docno;
        }

        /// <summary>
        /// รับเลขคู่บัญชีมาจาก Dialog แล้ว Set ค่าลงไป ที่คู่บัญชี
        /// </summary>
        private void PostToFromAccid()
        {
            int row;

            row = Convert.ToInt32(HfRow.Value);
            tofrom_accid = Hfaccount_id.Value;
            DwMain.SetItemString(row, "tofrom_accid", tofrom_accid);
        }

        /// <summary>
        /// ตรวจสอบข้อมูลจากปีที่แล้ว 
        /// หากพบ จะ Set คู่บัญชี, วิธีการจ่าย ให้เหมือนกับปีที่แล้ว
        /// </summary>
        /// <param name="i">แถวที่พบการบันทึกในปีที่แล้ว</param>
        private void SetFromYear(int i)
        {
            decimal capital_year2 = capital_year - 1;
            moneytype_code2 = "";
            tofrom_accid2 = "";

            try { moneytype_code = DwMain.GetItemString(i, "moneytype_code"); }
            catch { moneytype_code = ""; }
            try { tofrom_accid = DwMain.GetItemString(i, "tofrom_accid"); }
            catch { tofrom_accid = ""; }
            try { deptaccount_no = DwMain.GetItemString(i, "deptaccount_no"); }
            catch { deptaccount_no = ""; }
            try { expense_bank = DwMain.GetItemString(i, "expense_bank"); }
            catch { expense_bank = ""; }
            try { expense_branch = DwMain.GetItemString(i, "expense_branch"); }
            catch { expense_branch = ""; }

            if (moneytype_code.Length < 0 || moneytype_code == "" || moneytype_code == null)
            {//ตรวจสอบว่ามีการอนุมัติในปีปัจจุบันหรือไม่?
                try
                {
                    sqlStr = @" SELECT moneytype_code,
        		                       tofrom_accid,
                                       deptaccount_no,
                                       expense_bank,
                                       expense_branch
                                        FROM asnreqmaster 
                                        WHERE assist_docno LIKE 'SM%'
                                        AND member_no = '" + member_no + @"'
                                        AND capital_year = '" + capital_year2 + @"'
                              ";
                    dt = ta.Query(sqlStr);

                    if (dt.Next())
                    {
                        moneytype_code2 = dt.GetString("moneytype_code");
                        tofrom_accid2 = dt.GetString("tofrom_accid");
                        deptaccount_no2 = dt.GetString("deptaccount_no");
                        expense_bank2 = dt.GetString("expense_bank");
                        expense_branch2 = dt.GetString("expense_branch");

                        if (moneytype_code.Length <= 0)
                        {
                            DwMain.SetItemString(i, "moneytype_code", moneytype_code2);
                        }
                        if (tofrom_accid.Length <= 0)
                        {
                            DwMain.SetItemString(i, "tofrom_accid", tofrom_accid2);
                        }
                        if (deptaccount_no.Length <= 0)
                        {
                            DwMain.SetItemString(i, "deptaccount_no", deptaccount_no2);
                        }
                        if (expense_bank.Length <= 0)
                        {
                            DwMain.SetItemString(i, "expense_bank", expense_bank2);
                        }
                        if (expense_branch.Length <= 0)
                        {
                            DwMain.SetItemString(i, "expense_branch", expense_branch2);
                        }
                    }
                }
                catch (Exception ex) { ex.ToString(); }
            }
        }

        // decimal max_seq_no = GetMax_Number(select max(seq_no) as max_value from xx where xx = " + xx + ")
        public decimal GetMax_Number(string Select_Condition)
        {
            decimal max_value = 0;
            string sqlf = Select_Condition;
            Sdt dt = WebUtil.QuerySdt(sqlf);

            if (dt.Next())
            {
                max_value = dt.GetDecimal("max_value");
            }
            return max_value;
        }

        //        protected void GetSqlStr(Decimal capital_year)
        //        {
        //            sqlSelect = DwMain.GetSqlSelect();

        //            //แสดงข้อมูลทั้งหมด
        //            sqlStr1 = @" SELECT MBMEMBMASTER.MEMBER_NO,   
        //                               MBUCFPRENAME.PRENAME_DESC,   
        //                               MBMEMBMASTER.MEMB_NAME,   
        //                               MBMEMBMASTER.MEMB_SURNAME,   
        //                               '        ' as approve_tdate,   
        //                               MBMEMBMASTER.BIRTH_DATE,   
        //                               ftcm_calagemth(MBMEMBMASTER.MEMBER_DATE,sysdate) as age_range,   
        //                               ASNREQMASTER.CAPITAL_YEAR,   
        //                               ASNREQMASTER.ASSISTTYPE_CODE,   
        //                               ASNREQMASTER.ASSIST_AMT,   
        //                               ASNREQMASTER.PAYTYPE_CODE,   
        //                               ASNREQMASTER.APPROVE_DATE,   
        //                               ASNREQMASTER.REQ_STATUS,   
        //                               ASNREQMASTER.ASSIST_DOCNO,   
        //                               MBMEMBMASTER.SALARY_AMOUNT,   
        //                               MBMEMBMASTER.MEMBGROUP_CODE,   
        //                               ASNREQMASTER.DEPTACCOUNT_NO ,
        //	                           ASNREQMASTER.CAPITAL_YEAR  
        //                          FROM MBMEMBMASTER,   
        //                               MBUCFPRENAME,   
        //                               ASNREQMASTER  
        //                         WHERE ( mbmembmaster.member_no = asnreqmaster.member_no (+)) and  
        //                               ( MBMEMBMASTER.PRENAME_CODE = MBUCFPRENAME.PRENAME_CODE ) and  
        //                               ( ( ftcm_calagemth(MBMEMBMASTER.MEMBER_DATE,sysdate) >= 42.03 ) AND  
        //                               ( MBMEMBMASTER.RESIGN_STATUS <> 1 ) AND  
        //                               ( ASNREQMASTER.REQ_STATUS (+)<> 1 ) AND  
        //                               ( ASNREQMASTER.ASSISTTYPE_CODE (+)= '71' ) AND
        //                               ( ASNREQMASTER.CAPITAL_YEAR is null ))";
        //            HfSqlFlag.Value = sqlStr1;
        //            DwMain.SetSqlSelect(HfSqlFlag.Value);
        //            DwMain.Retrieve();
        //            select1 = DwMain.RowCount;

        //            //แสดงข้อมูลที่ได้รับสวัสดิการตามปีแล้ว
        //            sqlStr2 = @" SELECT MBMEMBMASTER.MEMBER_NO,   
        //                        MBUCFPRENAME.PRENAME_DESC,   
        //                        MBMEMBMASTER.MEMB_NAME,   
        //                        MBMEMBMASTER.MEMB_SURNAME,   
        //                        '        ' as approve_tdate,   
        //                        MBMEMBMASTER.BIRTH_DATE,   
        //                        ftcm_calagemth(MBMEMBMASTER.MEMBER_DATE,sysdate) as age_range,   
        //                        ASNREQMASTER.CAPITAL_YEAR,   
        //                        ASNREQMASTER.ASSISTTYPE_CODE,   
        //                        ASNREQMASTER.ASSIST_AMT,  
        //                        ASNREQMASTER.PAYTYPE_CODE,   
        //                        ASNREQMASTER.APPROVE_DATE,   
        //                        ASNREQMASTER.REQ_STATUS,   
        //                        ASNREQMASTER.ASSIST_DOCNO,   
        //                        MBMEMBMASTER.SALARY_AMOUNT,   
        //                        MBMEMBMASTER.MEMBGROUP_CODE ,   
        //                        ASNREQMASTER.DEPTACCOUNT_NO  ,
        //	                    ASNREQMASTER.CAPITAL_YEAR
        //                   FROM MBMEMBMASTER,   
        //                        MBUCFPRENAME,   
        //                        ASNREQMASTER  
        //                  WHERE ( mbmembmaster.member_no = asnreqmaster.member_no (+)) and  
        //                        ( MBMEMBMASTER.PRENAME_CODE = MBUCFPRENAME.PRENAME_CODE ) and  
        //                        ( ( ftcm_calagemth(MBMEMBMASTER.MEMBER_DATE,sysdate) >= 42.03 ) AND  
        //                        ( ASNREQMASTER.ASSISTTYPE_CODE (+) = '71') AND  
        //                        ( ASNREQMASTER.CAPITAL_YEAR = " + capital_year + @" ) )
        //                      ";
        //            DwMain.SetSqlSelect(sqlStr2);
        //            DwMain.Retrieve();
        //            select2 = DwMain.RowCount;

        //            //แสดงข้อมูลที่ไม่ได้รับสวัสดิการ
        //            sqlStr3 = @" SELECT MBMEMBMASTER.MEMBER_NO,   
        //                               MBUCFPRENAME.PRENAME_DESC,   
        //                               MBMEMBMASTER.MEMB_NAME,   
        //                               MBMEMBMASTER.MEMB_SURNAME,   
        //                               '        ' as approve_tdate,   
        //                               MBMEMBMASTER.BIRTH_DATE,   
        //                               ftcm_calagemth(MBMEMBMASTER.MEMBER_DATE,sysdate) as age_range,   
        //                               ASNREQMASTER.CAPITAL_YEAR,   
        //                               ASNREQMASTER.ASSISTTYPE_CODE,   
        //                               ASNREQMASTER.ASSIST_AMT,   
        //                               ASNREQMASTER.PAYTYPE_CODE,   
        //                               ASNREQMASTER.APPROVE_DATE,   
        //                               ASNREQMASTER.REQ_STATUS,   
        //                               ASNREQMASTER.ASSIST_DOCNO,   
        //                               MBMEMBMASTER.SALARY_AMOUNT,   
        //                               MBMEMBMASTER.MEMBGROUP_CODE,   
        //                               ASNREQMASTER.DEPTACCOUNT_NO ,
        //	                           ASNREQMASTER.CAPITAL_YEAR  
        //                          FROM MBMEMBMASTER,   
        //                               MBUCFPRENAME,   
        //                               ASNREQMASTER  
        //                         WHERE ( mbmembmaster.member_no = asnreqmaster.member_no (+)) and  
        //                               ( MBMEMBMASTER.PRENAME_CODE = MBUCFPRENAME.PRENAME_CODE ) and  
        //                               ( ( ftcm_calagemth( MBMEMBMASTER.MEMBER_DATE,sysdate) >= 42.03 ) AND  
        //                               ( MBMEMBMASTER.RESIGN_STATUS <> 1 ) AND  
        //                               ( ASNREQMASTER.REQ_STATUS (+)= 1 ) AND  
        //                               ( ASNREQMASTER.ASSISTTYPE_CODE (+)= '71' ) AND
        //                               ( ASNREQMASTER.CAPITAL_YEAR is null ))
        //                      ";
        //            if (select2 <= 0)
        //            {
        //                sqlSelect = sqlStr1;
        //            }
        //            if (select2 > 0 && select2 < select1)
        //            {
        //                sqlSelect = sqlStr3 + " UNION " + sqlStr2;
        //            }
        //            else
        //            {
        //                sqlSelect = sqlStr2;
        //            }
        //        }
        #endregion
    }
}