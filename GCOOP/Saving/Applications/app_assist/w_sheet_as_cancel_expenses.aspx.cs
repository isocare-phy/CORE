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
using DataLibrary;
using Sybase.DataWindow;
using System.Web.Services.Protocols;
using CoreSavingLibrary;

namespace Saving.Applications.app_assist
{
    public partial class w_sheet_as_cancel_expenses : PageWebSheet, WebSheet
    {
        protected DwThDate tDwMain;
        protected string postSearchList;
        protected string postApproveExpenses;
        protected string postSetBranch;
        protected string ls_sql;

        Sta ta;
        protected string pbl = "as_capital.pbl";

        protected string sqlStr;
        public void InitJsPostBack()
        {
            tDwMain = new DwThDate(DwMain, this);

            tDwMain.Add("approve_date", "approve_tdate");
            tDwMain.Add("entry_date", "entry_tdate");
            postSearchList = WebUtil.JsPostBack(this, "postSearchList");
            postApproveExpenses = WebUtil.JsPostBack(this, "postApproveExpenses");
            postSetBranch = WebUtil.JsPostBack(this, "postSetBranch");
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();
            ta = new Sta(sqlca.ConnectionString);
            sqlca = new DwTrans();
            sqlca.Connect();
            DwMain.SetTransaction(sqlca);
            ls_sql = DwMain.GetSqlSelect();

            if (!IsPostBack)
            {
                DwType.InsertRow(0);
                DwUtil.RetrieveDDDW(DwType, "astype", "as_capital.pbl", state.SsCoopId);
                DwUtil.RetrieveDDDW(DwType, "capital_year", "as_capital.pbl", null);
                DwUtil.RetrieveDDDW(DwType, "pay_type", "as_capital.pbl", null);
                DwType.SetItemDecimal(1, "capital_year", state.SsWorkDate.Year + 543);

                DwType.SetItemString(1, "pay_status_h", "1");
            }
            else
            {
                this.RestoreContextDw(DwType);
                this.RestoreContextDw(DwMain);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postSearchList")
            {
                SearchList();
            }
            else if (eventArg == "postApproveExpenses")
            {
                ApproveExpenses();
            }
            else if (eventArg == "postSetBranch")
            {
                SetBankbranch();
            }
        }

        public void SaveWebSheet()
        {
            string select_flag;

            for (int j = 1; j <= DwMain.RowCount; j++)
            {
                try { select_flag = DwMain.GetItemString(j, "cbxselect_flag"); }
                catch { select_flag = "-1"; }


                if (select_flag == "1")
                {
                    string assist_docno, moneytype_code, expense_branch, deptaccount_no;
                    string member_no = "";
                    decimal capital_year, assist_amt, pay_status;
                    DateTime expensese_date, operate_date;
                    DataTable dtSlippayout;
                    string expensese_tdate, operate_tdate;
                    try { member_no = DwMain.GetItemString(j, "member_no"); }
                    catch { member_no = ""; }

                    assist_docno = DwMain.GetItemString(j, "assist_docno");
                    capital_year = DwMain.GetItemDecimal(j, "capital_year");
                    //paytype_code = DwMain.GetItemString(j, "paytype_code");
                    try { moneytype_code = DwMain.GetItemString(j, "moneytype_code"); }
                    catch { moneytype_code = ""; }

                    try { expensese_date = DwMain.GetItemDate(j, "entry_date"); /* วันที่จ่าย*/}
                    catch { expensese_date = state.SsWorkDate; }
                    expensese_tdate = expensese_date.ToString("dd/MM/yyyy", WebUtil.EN);

                    try { deptaccount_no = DwMain.GetItemString(j, "deptaccount_no"); }
                    catch { deptaccount_no = ""; }
                    try { assist_amt = DwMain.GetItemDecimal(j, "assist_amt"); }
                    catch { assist_amt = 0; }

                    try { expense_branch = DwMain.GetItemString(j, "expense_branch"); }
                    catch { expense_branch = null; }
                    try { pay_status = DwMain.GetItemDecimal(j, "pay_status"); }
                    catch { pay_status = 0; }

                    try { operate_date = DwMain.GetItemDate(j, "operate_date"); /* วันที่ทำรายการ*/}
                    catch { operate_date = state.SsWorkDate; }
                    operate_tdate = operate_date.ToString("dd/MM/yyyy", WebUtil.EN);

                    // โอนเข้าบัญชีสหกรณ์
                    #region oldCode ยังไม่ใช้ 18/10/2556
                    //                    if (paytype_code == "04")
                    //                    {
                    //                        #region บันทึกข้อมูลลงตาราง dpdepttran และอัพเดทตาราง  asnreqmaster, asnmemsalary, asnslippayout
                    //                        sqlStr = @"select * 
                    //                                   from asnslippayout 
                    //                                   where payoutslip_no = '" + assist_docno + @"' 
                    //                                         and capital_year = '" + capital_year + @"' 
                    //                                         order by seq_no";
                    //                        dtSlippayout = WebUtil.QuerySdt(sqlStr);
                    //                        // seq_no
                    //                        int seq_no = 1;
                    //                        string sqlcheckseq = @"select count(deptaccount_no) as depcount 
                    //                                               from dpdepttran 
                    //                                               where tran_date = to_date('" + expensese_tdate + @"','dd/MM/yyyy') 
                    //                                               and deptaccount_no = '" + deptaccount_no + @"'";
                    //                        Sdt dtcheckseq = WebUtil.QuerySdt(sqlcheckseq);
                    //                        if (dtcheckseq.Next())
                    //                        {
                    //                            seq_no = seq_no + Convert.ToInt32(dtcheckseq.Rows[0]["depcount"].ToString());
                    //                        }
                    //                        for (int i = 0; i < dtSlippayout.Rows.Count; i++)
                    //                        {
                    //                            sqlStr = @"insert into dpdepttran (
                    //                                                    coop_id,                                               deptaccount_no,         
                    //                                                 memcoop_id,                                                    member_no,             
                    //                                                system_code,                                                    tran_year,          
                    //                                                  tran_date,                                                       seq_no,        
                    //                                                 old_balanc,                                                  old_accuint,
                    //                                               deptitem_amt,                                                      int_amt,        
                    //                                                new_accuint,                                                   new_balanc,          
                    //                                                tran_status,                                                  dividen_amt,        
                    //                                                average_amt,                                               branch_operate,     
                    //                                             sequest_status,                                                  sequest_amt
                    //                                    ) 
                    //                             values (
                    //                                  '" + state.SsCoopId + @"',  '" + dtSlippayout.Rows[i]["depaccount_no"].ToString() + @"',
                    //                                                   '001001',      '" + dtSlippayout.Rows[i]["member_no"].ToString() + @"',
                    //'" + dtSlippayout.Rows[i]["sliptype_code"].ToString() + @"',   '" + dtSlippayout.Rows[i]["capital_year"].ToString() + @"',
                    //   to_date('" + expensese_tdate + @"','dd/MM/yyyy'),                                                    '" + seq_no + @"',
                    //                                                          0,                                                            0,
                    //   '" + dtSlippayout.Rows[i]["payout_amt"].ToString() + @"',                                                            0,
                    //                                                          0,                                                            0,
                    //                                                          0,                                                            0,
                    //                                                          0,                                                        '001',
                    //                                                          0,                                                            0
                    //                                    )";
                    //                            ta.Exe(sqlStr);
                    //                            seq_no++;
                    //                        }
                    //                        // update pay_status
                    //                        sqlStr = @"update asnreqmaster set 
                    //                                                        pay_status = '1',
                    //                                                        pay_date = to_date('" + expensese_tdate + @"','dd/MM/yyyy') 
                    //                                    where assist_docno = '" + assist_docno + @"' 
                    //                                    and capital_year = '" + capital_year + @"'";
                    //                        ta.Exe(sqlStr);
                    //                        // update วันที่
                    //                        sqlStr = @"update asnmemsalary set 
                    //                                                        entry_date = to_date('" + expensese_tdate + @"','dd/MM/yyyy') 
                    //                                    where assist_docno = '" + assist_docno + @"' 
                    //                                    and capital_year = '" + capital_year + @"'";
                    //                        ta.Exe(sqlStr);
                    //                        sqlStr = @"update asnslippayout set 
                    //                                                        operate_date = to_date('" + expensese_tdate + @"','dd/MM/yyyy'),
                    //                                                        entry_date = to_date('" + expensese_tdate + @"','dd/MM/yyyy') 
                    //                                    where payoutslip_no = '" + assist_docno + @"' 
                    //                                    and capital_year = '" + capital_year + @"'";
                    //                        ta.Exe(sqlStr);
                    //                        #endregion
                    //                    }
                    //                    // จ่ายเงินสด
                    //                    else if (paytype_code == "01")
                    //                    {
                    //                        #region อัพเดทข้อมูลตาราง asnslippayout, asnreqmaster, asnmemsalary, asnslippayout
                    //                        sqlStr = @"select *
                    //                                   from asnslippayout 
                    //                                   where payoutslip_no = '" + assist_docno + @"' 
                    //                                   and capital_year = '" + capital_year + @"' 
                    //                                   order by seq_no";
                    //                        dtSlippayout = WebUtil.QuerySdt(sqlStr);
                    //                        for (int i = 0; i < dtSlippayout.Rows.Count; i++)
                    //                        {
                    //                            sqlStr = "insert into finslip values ";
                    //                        }
                    //                        // update pay_status
                    //                        sqlStr = @"update asnreqmaster set 
                    //                                                        pay_status = '1',
                    //                                                        pay_date = to_date('" + expensese_tdate + @"','dd/MM/yyyy') 
                    //                                    where assist_docno = '" + assist_docno + @"' 
                    //                                    and capital_year = '" + capital_year + @"'";
                    //                        ta.Exe(sqlStr);
                    //                        //update วันที่
                    //                        sqlStr = @"update asnmemsalary set 
                    //                                                        entry_date = to_date('" + expensese_tdate + @"','dd/MM/yyyy') 
                    //                                    where assist_docno = '" + assist_docno + @"' 
                    //                                    and capital_year = '" + capital_year + @"'";
                    //                        ta.Exe(sqlStr);
                    //                        sqlStr = @"update asnslippayout set 
                    //                                                        operate_date = to_date('" + expensese_tdate + @"','dd/MM/yyyy'),
                    //                                                        entry_date = to_date('" + expensese_tdate + @"','dd/MM/yyyy') 
                    //                                    where payoutslip_no = '" + assist_docno + @"' 
                    //                                    and capital_year = '" + capital_year + @"'";
                    //                        ta.Exe(sqlStr);
                    //                        #endregion
                    //                    }
                    #endregion
                    #region อัพเดทข้อมูลตาราง asnreqmaster, asnmemsalary, asnslippayout
                    //                    sqlStr = @" select *
                    //                                from asnslippayout 
                    //                                where payoutslip_no = '" + assist_docno + @"' 
                    //                                and capital_year = '" + capital_year + @"' 
                    //                                order by seq_no";
                    //                    dtSlippayout = WebUtil.QuerySdt(sqlStr);
                    //                    for (int i = 0; i < dtSlippayout.Rows.Count; i++)
                    //                    {
                    //                        sqlStr = "insert into finslip values ";
                    //                    } //comment on 10/11/2556
                    // update pay_status HC bankcode
                    //1.เช็คว่า ยกเลิกจ่ายย้อนหลังต้องลง วันที่ยกเลิก : cancel_date = วันที่ทำรายการ : operate_date
                    if (operate_date < state.SsWorkDate)
                    {
                        sqlStr = @" update  asnreqmaster 
                                    set     pay_status       = '" + pay_status + @"',
                                            clearloan_flag   = '0',
                                            withdrawable_amt = " + assist_amt + @",
                                            expense_branch   = '" + expense_branch + @"',
                                            expense_bank     = '034',
                                            cancel_id        = '" + state.SsUsername + @"',
                                            cancel_date      = to_date('" + operate_tdate + @"','dd/mm/yyyy') 
                                    where   assist_docno     = '" + assist_docno + @"' 
                                    and     capital_year     = '" + capital_year + @"'
                                    and     member_no        = '" + member_no + @"'";
                    }
                    //2.เช็คว่า ยกเลิกจ่ายล่วงหน้าต้องลง วันที่ยกเลิก : cancel_date = วันที่จ่าย : entry_date
                    else if (operate_date > state.SsWorkDate)
                    {
                        sqlStr = @" update  asnreqmaster 
                                    set     pay_status       = '" + pay_status + @"',
                                            clearloan_flag   = '0',
                                            withdrawable_amt = " + assist_amt + @",
                                            expense_branch   = '" + expense_branch + @"',
                                            expense_bank     = '034',
                                            cancel_id        = '" + state.SsUsername + @"',
                                            cancel_date      = to_date('" + expensese_tdate + @"','dd/mm/yyyy') 
                                    where   assist_docno     = '" + assist_docno + @"' 
                                    and     capital_year     = '" + capital_year + @"'
                                    and     member_no        = '" + member_no + @"'";
                    }
                    else
                    {
                        sqlStr = @" update  asnreqmaster 
                                    set     pay_status       = '" + pay_status + @"',
                                            clearloan_flag   = '0',
                                            withdrawable_amt = " + assist_amt + @",
                                            expense_branch   = '" + expense_branch + @"',
                                            expense_bank     = '034',
                                            cancel_id        = '" + state.SsUsername + @"',
                                            cancel_date      = to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + @"','dd/mm/yyyy') 
                                    where   assist_docno     = '" + assist_docno + @"' 
                                    and     capital_year     = '" + capital_year + @"'
                                    and     member_no        = '" + member_no + @"'";
                    }
                    try { ta.Exe(sqlStr); }
                    catch (Exception Err) { LtServerMessage.Text = WebUtil.ErrorMessage("ยกเลิกจ่ายทุนสวัสดิการไม่สำเร็จ: " + Err.Message); }
                    //update วันที่
                    sqlStr = @"     update  asnmemsalary 
                                    set     entry_date      = to_date('" + expensese_tdate + @"','dd/MM/yyyy') 
                                    where   assist_docno    = '" + assist_docno + @"' 
                                    and     capital_year    = '" + capital_year + @"'
                                    and     member_no       = '" + member_no + @"'";
                    try { ta.Exe(sqlStr); }
                    catch (Exception Err) { LtServerMessage.Text = WebUtil.ErrorMessage("ยกเลิกจ่ายทุนสวัสดิการไม่สำเร็จ: " + Err.Message); }
                    //1.เช็คว่า ยกเลิกจ่ายย้อนหลังต้องลง วันที่ยกเลิก : cancel_date = วันที่ทำรายการ : operate_date
                    if (operate_date < state.SsWorkDate)
                    {
                        sqlStr = @" update  asnslippayout 
                                    set     slip_status     = '-1',
                                            cancel_id       = '" + state.SsUsername + @"',
                                            cancel_date     = to_date('" + operate_tdate + @"','dd/mm/yyyy') 
                                    where   payoutslip_no   = '" + assist_docno + @"' 
                                    and     capital_year    = '" + capital_year + @"'
                                    and     member_no       = '" + member_no + @"'";
                    }
                    //2.เช็คว่า ยกเลิกจ่ายล่วงหน้าต้องลง วันที่ยกเลิก : cancel_date = วันที่จ่าย : entry_date
                    else if (operate_date > state.SsWorkDate)
                    {
                        sqlStr = @" update  asnslippayout 
                                    set     slip_status     = '-1',
                                            cancel_id       = '" + state.SsUsername + @"',
                                            cancel_date     = to_date('" + expensese_tdate + @"','dd/mm/yyyy') 
                                    where   payoutslip_no   = '" + assist_docno + @"' 
                                    and     capital_year    = '" + capital_year + @"'
                                    and     member_no       = '" + member_no + @"'";
                    }
                    else
                    {
                        sqlStr = @" update  asnslippayout 
                                    set     slip_status     = '-1',
                                            cancel_id       = '" + state.SsUsername + @"',
                                            cancel_date     = to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + @"','dd/mm/yyyy') 
                                    where   payoutslip_no   = '" + assist_docno + @"' 
                                    and     capital_year    = '" + capital_year + @"'
                                    and     member_no       = '" + member_no + @"'";
                    }
                    try { ta.Exe(sqlStr); }
                    catch (Exception Err) { LtServerMessage.Text = WebUtil.ErrorMessage("ยกเลิกจ่ายทุนสวัสดิการไม่สำเร็จ: " + Err.Message); }
                    #endregion
                }
            }
            LtServerMessage.Text = WebUtil.CompleteMessage("ยกเลิกจ่ายทุนสวัสดิการเรียบร้อยแล้ว");
            SearchList();

        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
            DwType.SaveDataCache();
            tDwMain.Eng2ThaiAllRow();
            this.DisConnectSQLCA();
            ta.Close();
        }
        private void SearchList()
        {
            decimal ai_year = 0;
            String as_type = "";
            string as_paytype = "";
            string member_no;
            string pay_status_h = "";

            try { ai_year = DwType.GetItemDecimal(1, "capital_year"); }
            catch { ai_year = 0; }
            try { as_type = DwType.GetItemString(1, "astype"); }
            catch { as_type = ""; }
            try { as_paytype = DwType.GetItemString(1, "pay_type"); }
            catch { as_paytype = ""; }
            try
            {
                member_no = WebUtil.MemberNoFormat(DwType.GetItemString(1, "member_no"));
                if (member_no == "00000000") { member_no = ""; }
                DwType.SetItemString(1, "member_no", member_no);
            }
            catch { member_no = ""; }
            try { pay_status_h = DwType.GetItemString(1, "pay_status_h"); }
            catch { pay_status_h = ""; }

            if (ai_year > 0) { ls_sql += " AND ASNREQMASTER.capital_year = " + ai_year; }
            if (as_type.Length > 0) { ls_sql += " AND ASNREQMASTER.assisttype_code = " + as_type.Trim(); }
            if (as_paytype.Length > 0) { ls_sql += " AND ASNREQMASTER.moneytype_code = '" + as_paytype.Trim() + "'"; }
            if (member_no.Length > 0) { ls_sql += " AND ASNREQMASTER.member_no = '" + member_no.Trim() + "'"; }
            if (pay_status_h.Length > 0) { ls_sql += " AND ASNREQMASTER.pay_status = '" + pay_status_h.Trim() + "'"; }
            ls_sql += " and asnreqmaster.approve_status=1 ";//AND ASNREQMASTER.PAY_STATUS=1";

            DwMain.SetSqlSelect(ls_sql);
            //LEK เช็คว่ากรณีเลือก ประเภททุน : 10 = สวัสดิการทุนการศึกษาบุตร ให้เรียงตาม ปี, เลขที่ใบคำขอ และวันที่ขอ
            if (as_type == "10")
            {
                DwMain.SetSort("capital_year asc, assist_docno asc, req_date asc");
                DwMain.Sort();
            }
            DwMain.Retrieve();
            //DwUtil.RetrieveDDDW(DwMain, "expense_branch", pbl, null);
            //SetBankbranch();
            return;
            for (int j = 1; j <= DwMain.RowCount; j++)
            {
                try
                {
                    //DwMain.GetItemString(j, "cbxselect_flag"); 
                    DwMain.SetItemDate(j, "entry_date", state.SsWorkDate);

                }
                catch { }
            }
            SetBankbranch();
        }
        private void ApproveExpenses()
        {
            string select_flag;

            for (int j = 1; j <= DwMain.RowCount; j++)
            {
                try
                {
                    select_flag = DwMain.GetItemString(j, "cbxselect_flag");
                }
                catch
                {
                    select_flag = "-1";
                }
                if (select_flag == "1")
                {
                    string assist_docno, moneytype_code, deptaccount_no;
                    decimal capital_year;
                    DateTime expensese_date;
                    DataTable dtSlippayout;
                    //
                    assist_docno = DwMain.GetItemString(j, "assist_docno");
                    capital_year = DwMain.GetItemDecimal(j, "capital_year");
                    moneytype_code = DwMain.GetItemString(j, "moneytype_code");
                    expensese_date = DwMain.GetItemDate(j, "entry_date"); // วันที่จ่าย
                    deptaccount_no = DwMain.GetItemString(j, "deptaccount_no");
                    // โอนเข้าบัญชีสหกรณ์
                    if (moneytype_code == "04")
                    {
                        sqlStr = "select * from asnslippayout where payoutslip_no = '" + assist_docno + "' and capital_year = '" + capital_year + "' order by seq_no";
                        dtSlippayout = WebUtil.QuerySdt(sqlStr);
                        // seq_no
                        int seq_no = 1;
                        string sqlcheckseq = "select count(deptaccount_no) as depcount from dpdepttran where tran_date = to_date('" + expensese_date.ToString("MM/dd/yyyy") + "','MM/dd/yyyy') and deptaccount_no = '" + deptaccount_no + "'";
                        Sdt dtcheckseq = WebUtil.QuerySdt(sqlcheckseq);
                        if (dtcheckseq.Next())
                        {
                            seq_no = seq_no + Convert.ToInt32(dtcheckseq.Rows[0]["depcount"].ToString());
                        }
                        for (int i = 0; i < dtSlippayout.Rows.Count; i++)
                        {
                            sqlStr = @"insert into dpdepttran (coop_id,deptaccount_no,memcoop_id,member_no,system_code,tran_year,tran_date,seq_no,old_balanc,old_accuint,deptitem_amt,int_amt,
                             new_accuint,new_balanc,tran_status,dividen_amt,average_amt,branch_operate,sequest_status,sequest_amt) 
                             values ('001001','" + dtSlippayout.Rows[i]["depaccount_no"].ToString() + "','001001','" + dtSlippayout.Rows[i]["member_no"].ToString() + "','" + dtSlippayout.Rows[i]["sliptype_code"].ToString() + "','"
                                     + dtSlippayout.Rows[i]["capital_year"].ToString() + "',to_date('" + expensese_date.ToString("MM/dd/yyyy") + "','MM/dd/yyyy'),'"
                                     + seq_no + "',0,0,'" + dtSlippayout.Rows[i]["payout_amt"].ToString() + "',0,0,0,0,0,0,'001',0,0)";
                            ta.Exe(sqlStr);
                            seq_no++;
                        }
                        // update pay_status
                        sqlStr = "update asnreqmaster set pay_status = '1',pay_date = to_date('" + expensese_date.ToString("MM/dd/yyyy") + "','MM/dd/yyyy') where assist_docno = '" + assist_docno + "' and capital_year = '" + capital_year + "'";
                        ta.Exe(sqlStr);
                        // update วันที่
                        sqlStr = "update asnmemsalary set entry_date = to_date('" + expensese_date.ToString("MM/dd/yyyy") + "','MM/dd/yyyy') where assist_docno = '" + assist_docno + "' and capital_year = '" + capital_year + "'";
                        ta.Exe(sqlStr);
                        sqlStr = "update asnslippayout set operate_date = to_date('" + expensese_date.ToString("MM/dd/yyyy") + "','MM/dd/yyyy'),entry_date = to_date('" + expensese_date.ToString("MM/dd/yyyy") + "','MM/dd/yyyy') where payoutslip_no = '" + assist_docno + "' and capital_year = '" + capital_year + "'";
                        ta.Exe(sqlStr);
                    }
                    // จ่ายเงินสด
                    else if (moneytype_code == "01")
                    {
                        sqlStr = "select * from asnslippayout where payoutslip_no = '" + assist_docno + "' and capital_year = '" + capital_year + "' order by seq_no";
                        dtSlippayout = WebUtil.QuerySdt(sqlStr);
                        for (int i = 0; i < dtSlippayout.Rows.Count; i++)
                        {
                            sqlStr = "insert into finslip values ";
                        }
                        // update pay_status
                        sqlStr = "update asnreqmaster set pay_status = '1',pay_date = to_date('" + expensese_date.ToString("MM/dd/yyyy") + "','MM/dd/yyyy') where assist_docno = '" + assist_docno + "' and capital_year = '" + capital_year + "'";
                        ta.Exe(sqlStr);
                        //update วันที่
                        sqlStr = "update asnmemsalary set entry_date = to_date('" + expensese_date.ToString("MM/dd/yyyy") + "','MM/dd/yyyy') where assist_docno = '" + assist_docno + "' and capital_year = '" + capital_year + "'";
                        ta.Exe(sqlStr);
                        sqlStr = "update asnslippayout set operate_date = to_date('" + expensese_date.ToString("MM/dd/yyyy") + "','MM/dd/yyyy'),entry_date = to_date('" + expensese_date.ToString("MM/dd/yyyy") + "','MM/dd/yyyy' where payoutslip_no = '" + assist_docno + "') and capital_year = '" + capital_year + "'";
                        ta.Exe(sqlStr);
                    }
                }
            }
            LtServerMessage.Text = WebUtil.CompleteMessage("จ่ายทุนสวัสดิการเรียบร้อยแล้ว");
            SearchList();
        }
        private bool CheckDeptClose(string deptaccountno)
        {
            bool status = false;
            Sdt dtCheck = null;
            sqlStr = "select * from dpdeptmaster where deptclose_status = '1' and deptaccount_no = '" + deptaccountno + "'";
            ta.Exe(sqlStr);
            if (dtCheck.Next())
            {
                status = true;
            }
            return status;
        }
        protected void CheckAllChanged(object sender, EventArgs e)
        {
            int rowcount = DwMain.RowCount;
            int i;
            if (ChkAll.Checked == true)
            {
                for (i = 1; i <= rowcount; i++)
                {
                    DwMain.SetItemString(i, "cbxselect_flag", "1");
                }
            }
            else
            {
                for (i = 1; i <= rowcount; i++)
                {
                    DwMain.SetItemString(i, "cbxselect_flag", "-1");
                }
            }

        }
        protected void SetBankbranch()
        {
            string select_flag, service_amt;
            decimal assist_amt;

            for (int j = 1; j <= DwMain.RowCount; j++)
            {
                string expense_branch;

                try
                {
                    expense_branch = DwMain.GetItemString(j, "expense_branch");
                    assist_amt = DwMain.GetItemDecimal(j, "assist_amt");

                    service_amt = GetVal_SQL("select * from cmucfbankbranch where bank_code = '034' and branch_id = '" + expense_branch + "'");
                    DwMain.SetItemDecimal(j, "sum_amt", assist_amt + Convert.ToDecimal(service_amt));
                }
                catch { }
            }
        }

        public string GetVal_SQL(string Select_Condition)
        {
            string max_value = "";
            string sqlf = Select_Condition;
            Sdt dt = WebUtil.QuerySdt(sqlf);

            if (dt.Next())
            {
                max_value = dt.GetString("service_amt");
            }
            return max_value;
        }
    }
}