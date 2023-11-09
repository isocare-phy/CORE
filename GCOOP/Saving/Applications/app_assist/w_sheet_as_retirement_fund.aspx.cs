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
    public partial class w_sheet_as_retirement_fund : PageWebSheet, WebSheet
    {
        protected String app;
        protected String gid;
        protected String rid;
        protected String pdf;
        protected String postPopupReport;

        protected DwThDate tDwMain;
        protected DwThDate tDwDetail;

        protected String postRefresh;
        protected String postChangeHeight;
        protected String postRetreiveDwMem;
        protected String postRetrieveDwMain;
        protected String postGetMemberDetail;
        protected String postRetrieveBankBranch;
        protected String postHoardCase;
        protected String postCheckYear;
        protected String postDelete;
        protected String postChangeAge;
        protected String postChangeAmt;
        protected Sta ta;

        private String sqlStr;
        private String member_no, assist_docno, membgroup_code,assisttype_code;
        private String expense_bank, expense_branch, expense_accid, paytype_code, remark_cancel;
        private Decimal capital_year, salary_amt, assist_amt, req_status, rowcount;
        private DateTime pay_date, cancel_date = DateTime.Now,member_date, birth_date,req_date;
        private Nullable<Decimal> cancel_id;

        public void InitJsPostBack()
        {
            tDwMain = new DwThDate(DwMain, this);
            tDwDetail = new DwThDate(DwDetail, this);

            tDwMain.Add("pay_date", "pay_tdate");
            tDwMain.Add("req_date", "req_tdate");
            tDwMain.Add("entry_date", "entry_tdate");
            tDwMain.Add("member_date", "member_tdate");
            tDwMain.Add("approve_date", "approve_tdate");

            tDwDetail.Add("retire_date", "retire_tdate");
          

            postRefresh = WebUtil.JsPostBack(this, "postRefresh");
            postChangeHeight = WebUtil.JsPostBack(this, "postChangeHeight");
            postRetreiveDwMem = WebUtil.JsPostBack(this, "postRetreiveDwMem");
            postRetrieveDwMain = WebUtil.JsPostBack(this, "postRetrieveDwMain");
            postGetMemberDetail = WebUtil.JsPostBack(this, "postGetMemberDetail");
            postRetrieveBankBranch = WebUtil.JsPostBack(this, "postRetrieveBankBranch");
            postHoardCase = WebUtil.JsPostBack(this, "postHoardCase");
            postCheckYear = WebUtil.JsPostBack(this, "postCheckYear");
            postDelete = WebUtil.JsPostBack(this, "postDelete");
            postChangeAge = WebUtil.JsPostBack(this, "postChangeAge");
            postChangeAmt = WebUtil.JsPostBack(this, "postChangeAmt");
            postPopupReport = WebUtil.JsPostBack(this, "postPopupReport");
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();
            ta = new Sta(sqlca.ConnectionString);
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
            tDwDetail.Eng2ThaiAllRow();
            DwMain.SetItemString(1, "assisttype_code", "82");
            DwUtil.RetrieveDDDW(DwMain, "assisttype_code", "as_capital.pbl", state.SsCoopId);
            DwUtil.RetrieveDDDW(DwMain, "capital_year", "as_capital.pbl", null);
            DwUtil.RetrieveDDDW(DwMain, "expense_bank", "as_capital.pbl", null);
            DwUtil.RetrieveDDDW(DwMain, "cancel_id", "as_capital.pbl", null);
            DwUtil.RetrieveDDDW(DwMain, "pay_status", "as_capital.pbl", null);
            DwUtil.RetrieveDDDW(DwMain, "req_status", "as_capital.pbl", null);
            DwUtil.RetrieveDDDW(DwMain, "paytype_code", "as_capital.pbl", null);
            DwUtil.RetrieveDDDW(DwDetail, "subdamagetype_code", "as_capital.pbl", null);
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
            else if (eventArg == "postHoardCase")
            {
                ChangeAmt();
            }
            else if (eventArg == "postCheckYear")
            {
                GetCheckYear();
            }
            else if (eventArg == "postDelete")
            {
                DeleteRow();
            }
            else if (eventArg == "postChangeAge")
            {
                ChangeAge();
            }
            else if (eventArg == "postPopupReport")
            {
                PopupReport();
            }
            else if (eventArg == "postChangeAmt")
            {
                ChangeAmt();
            }
        }

        private void DeleteRow()
        {
            String sqlStr, assist_docno;
            Decimal capital_year;
            Sta ta2 = new Sta(sqlca.ConnectionString);
            try
            {

                assist_docno = DwMain.GetItemString(1, "assist_docno");
                capital_year = DwMain.GetItemDecimal(1, "capital_year");

                sqlStr = @"delete  from  asnmemsalary  
                       where assist_docno = '" + assist_docno + @"'  and
                             capital_year = " + capital_year + "";
                ta2.Exe(sqlStr);


                sqlStr = @"delete  from  ASNREQCAPITALDET    
                        where assist_docno = '" + assist_docno + @"'  and
                              capital_year = " + capital_year + "";
                ta2.Exe(sqlStr);

                sqlStr = @"delete  from  asnreqmaster
                       where assist_docno = '" + assist_docno + @"'  and
                             capital_year = " + capital_year + "";
                ta2.Exe(sqlStr);

                sqlStr = "delete from asnslippayout where payoutslip_no = '" + assist_docno + "' and capital_year = '" + capital_year + "'";
                ta2.Exe(sqlStr);

                LtServerMessage.Text = WebUtil.CompleteMessage("ลบข้อมูลเรียบร้อย");
                NewClear();
            }
            catch
            {
                //ta.RollBack();
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถลบได้");
            }

            ta2.Close();
        }

        public void GetCheckYear()
        {
            DateTime req_date, hoard_date;
            int req_date_dd = 0, req_date_mm = 0, req_date_yyyy = 0;
            int hoard_date_dd = 0, hoard_date_mm = 0, hoard_date_yyyy = 0;
       
            try
            {
                req_date = DateTime.ParseExact(DwMain.GetItemString(1, "req_tdate"), "ddMMyyyy", WebUtil.TH);
                req_date_dd = Convert.ToInt32(req_date.Day.ToString());
                req_date_mm = Convert.ToInt32(req_date.Month.ToString());
                req_date_yyyy = Convert.ToInt32(req_date.Year.ToString());
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
            try
            {
                String b = HdfHoardTdate.Value.Trim();
                hoard_date = DateTime.ParseExact(b, "ddMMyyyy", WebUtil.TH);
                hoard_date_dd = Convert.ToInt32(hoard_date.Day.ToString());
                hoard_date_mm = Convert.ToInt32(hoard_date.Month.ToString());
                hoard_date_yyyy = Convert.ToInt32(hoard_date.Year.ToString());

                DwDetail.SetItemDate(1, "hoard_date", hoard_date);
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }

            DateTime req_date_d1 = new DateTime(req_date_yyyy, req_date_mm, req_date_dd);
            DateTime cripple_date_d2 = new DateTime(hoard_date_yyyy, hoard_date_mm, hoard_date_dd);

            TimeSpan diffTime = req_date_d1 - cripple_date_d2;
            int diftime = diffTime.Days;
            if (diftime > 120)
            {
                LtServerMessage.Text = WebUtil.WarningMessage("เกิน 120 วันนับจากวันที่ทุพพลภาพ");
            }
            //DwDetail.SetItemString(1, "hoard_tdate", Convert.ToString(hoard_date_dd + "/" + hoard_date_mm + "/" + hoard_date_yyyy));

            tDwMain.Eng2ThaiAllRow();
            tDwDetail.Eng2ThaiAllRow();
        }

        public void ChangeAmt()
        {
            String retire_tdate;
            DateTime retire_date;

            retire_tdate = DwDetail.GetItemString(1, "retire_tdate");
            retire_date = DateTime.ParseExact(HdRetireDate.Value, "ddMMyyyy", WebUtil.TH);

            DwDetail.SetItemDateTime(1, "retire_date", retire_date);
            //GetCheckYear(); ตรวจวัน
            String SqlStr = "";
            Decimal age_range = 0 ,assist_amt = 0 ;
            age_range = Decimal.Parse(DwMain.GetItemString(1, "age_range"));
            //แก้คิดจากการส่งงวดหุ้น
            string last_period = DwMain.GetItemString(1, "last_period");

            string retire_case = DwDetail.GetItemString(1, "retire_case");
            if (retire_case == "retire_1")
            {
                sqlStr = "select * from asnsenvironmentvar where envgroup = 'retirement_fund' and envcode like 'retirement_fund_1%' and '"+last_period+"' between start_age and end_age";
            }
            else if (retire_case == "retire_2") {
                sqlStr = "select * from asnsenvironmentvar where envgroup = 'retirement_fund' and envcode like 'retirement_fund_2%' and '" + last_period + "' between start_age and end_age";
            }
            else if (retire_case == "retire_3")
            {
                sqlStr = "select * from asnsenvironmentvar where envgroup = 'retirement_fund' and envcode like 'retirement_fund_3%' and '" + last_period + "' between start_age and end_age";
            }
                Sta ta1 = new Sta(sqlca.ConnectionString);
                Sdt dt1 = ta1.Query(sqlStr);
                if (dt1.Next())
                {
                    assist_amt = assist_amt + Convert.ToDecimal(dt1.Rows[0]["envvalue"].ToString());
                    DwDetail.SetItemDecimal(1, "assist_amt", assist_amt);
                }
        }

        public void SaveWebSheet()
        {
            Sta ta2 = new Sta(state.SsConnectionString);
            ta2.Transection();

            String sqlStr;
            String remark;
            Decimal member_age;
            
            Sta ta = new Sta(sqlca.ConnectionString);
            Sdt dt = new Sdt();

            GetItemDwMain();
            DwMain.SetItemString(1, "coop_id", state.SsCoopId);
            assist_amt = DwDetail.GetItemDecimal(1, "assist_amt");
            member_age = DwDetail.GetItemDecimal(1, "member_age");

           
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
                try
                {
                    capital_year = DwMain.GetItemDecimal(1, "capital_year");
                }
                catch { capital_year = 2555; }
                assist_docno = GetLastDocNo(capital_year);

                try
                {
                    DwMain.SetItemString(1, "member_no", member_no);
                    DwMain.SetItemDecimal(1, "assist_amt", assist_amt);
                    DwMain.SetItemString(1, "assist_docno", assist_docno);
                    try
                    {
                        string strMemberAge = DwMain.GetItemString(1, "member_years_disp");
                        DwMain.SetItemString(1, "member_years_disp", "");
                        DwUtil.InsertDataWindow(DwMain, "as_capital.pbl", "asnreqmaster");
                        DwMain.SetItemString(1, "member_years_disp", strMemberAge);
                    }
                    catch { }

                    DwDetail.SetItemString(1, "assist_docno", assist_docno);
                    DwDetail.SetItemDecimal(1, "capital_year", capital_year);
                    DwDetail.SetItemDateTime(1, "capital_date", state.SsWorkDate);
                    try
                    {
                        DwUtil.InsertDataWindow(DwDetail, "as_capital.pbl", "asnreqcapitaldet");
                    }
                    catch { }

                    //                    sqlStr = @"INSERT INTO asnreqmaster(assist_docno  ,  capital_year  ,  member_no  ,  assisttype_code  ,
                    //                                                assist_amt    ,  req_status ,  req_date  ,  pay_date  ,  paytype_code  ,  remark  ,
                    //                                                membgroup_code  ,  expense_bank  ,  expense_branch  ,  expense_accid)
                    //                                         VALUES('" + assist_docno + "','" + capital_year + "','" + member_no + @"','50',
                    //                                                '" + assist_amt + "','" + req_status + "',to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + @"', 'dd/mm/yyyy'),to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + @"', 'dd/mm/yyyy'),
                    //                                                '" + paytype_code + @"',
                    //                                                '" + remark + "','" + membgroup_code + "','" + expense_bank + "','" + expense_branch + "','" + expense_accid + "')";
                    //                    ta.Exe(sqlStr);

                    //                    sqlStr = @"INSERT INTO asnreqcapitaldet(capital_year  ,  assist_docno  ,  capital_date  ,  damagetype_code  ,  subdamagetype_code,  member_age  ,  hoard_case  ,
                    //                                                            remark  ,  assist_amt  )
                    //                                   VALUES           ('" + capital_year + "','" + assist_docno + "',to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + @"', 'dd/mm/yyyy'),'05','05',
                    //                                                     '" + member_age + "','" + hoard_case + "','" + remark + "','" + assist_amt + "')";

                    //                    ta.Exe(sqlStr);

                    sqlStr = @"INSERT INTO asnmemsalary(capital_year  ,  member_no  ,  assist_docno  ,
                                                    salary_amt    ,  entry_date  ,  membgroup_code)
                                         VALUES('" + capital_year + "','" + member_no + "','" + assist_docno + @"',
                                                '" + salary_amt + "',to_date('" + DwMain.GetItemDateTime(1, "entry_date").ToString("dd/MM/yyyy", WebUtil.EN) + "', 'dd/mm/yyyy'),'" + membgroup_code + @"')";
                    ta.Exe(sqlStr);

                    sqlStr = @"UPDATE  asndoccontrol
                               SET     last_docno   = '" + WebUtil.Right(assist_docno, 6) + @"'
                               WHERE   doc_prefix   = 'RF' and
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
                             deptaccount_no = DwMain.GetItemString(1, "deptaccount_no");
                        }
                        catch {
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
                        decimal age_range = Decimal.Parse(DwMain.GetItemString(1, "age_range"));
                        //แก้คิดจากการส่งงวดหุ้น
                        string last_period = DwMain.GetItemString(1, "last_period");
                        string retire_case = DwDetail.GetItemString(1, "retire_case");
                        if (retire_case == "retire_1")
                        {
                            sqlStr = "select * from asnsenvironmentvar where envgroup = 'retirement_fund' and envcode like 'retirement_fund_1%' and '" + last_period + "' between start_age and end_age";
                        }
                        else if (retire_case == "retire_2")
                        {
                            sqlStr = "select * from asnsenvironmentvar where envgroup = 'retirement_fund' and envcode like 'retirement_fund_2%' and '" + last_period + "' between start_age and end_age";
                        }
                        else if (retire_case == "retire_3")
                        {
                            sqlStr = "select * from asnsenvironmentvar where envgroup = 'retirement_fund' and envcode like 'retirement_fund_3%' and '" + last_period + "' between start_age and end_age";
                        }
                        dtAmt = WebUtil.QuerySdt(sqlStr);
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
                DwMain.SetItemDecimal(1, "assist_amt", assist_amt);
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
                        string strMemberAge = DwMain.GetItemString(1, "member_years_disp");
                        DwMain.SetItemString(1, "member_years_disp", "");
                        DwUtil.UpdateDataWindow(DwMain, "as_capital.pbl", "asnreqmaster");
                        DwMain.SetItemString(1, "member_years_disp", strMemberAge);
                    }
                    catch { }

                    try
                    {
                        DwUtil.UpdateDataWindow(DwDetail, "as_capital.pbl", "asnreqcapitaldet");
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
                        sqlStr = "select * from asnucfpaytype where paytype_code = '" + paytype_code + "'";
                        string moneytype_code = "";
                        string deptaccount_no;
                        try
                        {
                            deptaccount_no = DwMain.GetItemString(1, "deptaccount_no");
                        }
                        catch {
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
                        decimal age_range = Decimal.Parse(DwMain.GetItemString(1, "age_range"));
                        //แก้คิดจากการส่งงวดหุ้น
                        string last_period = DwMain.GetItemString(1, "last_period");
                        string retire_case = DwDetail.GetItemString(1, "retire_case");
                        if (retire_case == "retire_1")
                        {
                            sqlStr = "select * from asnsenvironmentvar where envgroup = 'retirement_fund' and envcode like 'retirement_fund_1%' and '" + last_period + "' between start_age and end_age";
                        }
                        else if (retire_case == "retire_2")
                        {
                            sqlStr = "select * from asnsenvironmentvar where envgroup = 'retirement_fund' and envcode like 'retirement_fund_2%' and '" + last_period + "' between start_age and end_age";
                        }
                        else if (retire_case == "retire_3")
                        {
                            sqlStr = "select * from asnsenvironmentvar where envgroup = 'retirement_fund' and envcode like 'retirement_fund_3%' and '" + last_period + "' between start_age and end_age";
                        }
                        dtAmt = WebUtil.QuerySdt(sqlStr);
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
            //DwMain.SetItemDateTime(1, "pay_date", state.SsWorkDate);
            //DwMain.SetItemDateTime(1, "req_date", state.SsWorkDate);
            //DwMain.SetItemDateTime(1, "entry_date", state.SsWorkDate);
            //DwMain.SetItemDateTime(1, "approve_date", state.SsWorkDate);

            try
            {
                String membNo = DwUtil.GetString(DwMem, 1, "member_no", "");
                if (membNo != "")
                {
                    DwUtil.RetrieveDDDW(DwMain, "deptaccount_no", "as_capital.pbl", membNo);
                    DataWindowChild dc = DwMain.GetChild("deptaccount_no");
                    String accNo = DwUtil.GetString(DwMain, 1, "deptaccount_no", "").Trim();
                    if (dc.RowCount > 0 && accNo == "")
                    {
                        DwMain.SetItemString(1, "deptaccount_no", dc.GetItemString(1, "deptaccount_no"));
                    }
                }
            }
            catch { }

            tDwMain.Eng2ThaiAllRow();
            tDwDetail.Eng2ThaiAllRow();

            DwMem.SaveDataCache();
            DwMain.SaveDataCache();
            DwDetail.SaveDataCache();
        }



        private void Refresh()
        {

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
            DwUtil.RetrieveDataWindow(DwMem, "as_capital.pbl", null, args1);

            object[] args2 = new object[3];
            args2[0] = assist_docno;
            args2[1] = capital_year;
            args2[2] = member_no;

            DwMain.Reset();
            DwUtil.RetrieveDataWindow(DwMain, "as_capital.pbl", tDwMain, args2);

            object[] args3 = new object[2];
            args3[0] = assist_docno;
            args3[1] = capital_year;

            DwDetail.Reset();
            DwUtil.RetrieveDataWindow(DwDetail, "as_capital.pbl", tDwDetail, args3);

            RetrieveBankBranch();

            decimal member_age;
            int[] member_year = new int[3];

            //**
            req_date = DwMain.GetItemDateTime(1, "req_date");
            member_date = DwMain.GetItemDateTime(1, "member_date");
            birth_date = DwMain.GetItemDateTime(1, "birth_date");
            
            member_year = clsCalAge(member_date, req_date);

            string member_age_range = member_year[2].ToString() + "." + member_year[1].ToString("00") + member_year[0].ToString("00");
            string s_member_year = member_year[2].ToString() + " ปี " + member_year[1].ToString() + " เดือน " + member_year[0].ToString() + " วัน";

            DwMain.SetItemString(1, "member_years_disp", s_member_year);
            member_age = wcf.Busscom.of_cal_yearmonth(state.SsWsPass, birth_date, req_date);
            string s_member_age = member_age.ToString() + " ปี";
            DwMain.SetItemString(1, "member_age", s_member_age);
            DwMain.SetItemString(1, "age_range", member_age_range);

            string last_peroid = GetLastPeriod(member_no).ToString();
            DwMain.SetItemString(1, "last_period", last_peroid);
        }

        private void GetMemberDetail()
        {
            String member_no, prename_desc, memb_name, memb_surname, membgroup_code, membgroup_desc, membtype_code, membtype_desc,card_person;
            DateTime member_date, birth_date;
            Decimal salary_amount, member_age;
            member_no = HfMemberNo.Value;

            int[] member_year = new int[3];

            DataStore Ds = new DataStore();
            Ds.LibraryList = "C:/GCOOP_ALL/CEN/GCOOP/Saving/DataWindow/app_assist/as_capital.pbl";
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

            member_year = clsCalAge(member_date, state.SsWorkDate);
            string member_age_range = member_year[2].ToString() + "." + member_year[1].ToString("00") + member_year[0].ToString("00");
            string s_member_year = member_year[2].ToString() + " ปี " + member_year[1].ToString() + " เดือน " + member_year[0].ToString() + " วัน";

            DwMain.SetItemString(1, "member_years_disp", s_member_year);
            member_age = wcf.Busscom.of_cal_yearmonth(state.SsWsPass, birth_date, state.SsWorkDate);
            string s_member_age = member_age.ToString() + " ปี";
            DwMain.SetItemString(1, "member_age", s_member_age);
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
            DwMem.SetItemString(1, "card_person", card_person);
            DwMain.SetItemDateTime(1, "member_date", member_date);
            DwMain.SetItemDateTime(1, "birth_date", birth_date);
            DwMain.SetItemDecimal(1, "salary_amt", salary_amount);

            string last_period = GetLastPeriod(member_no).ToString();
            DwMain.SetItemString(1, "last_period", last_period);
            tDwMain.Eng2ThaiAllRow();
            tDwDetail.Eng2ThaiAllRow();
            
            
            //rowcount = CheckReq(member_no);
            //if (rowcount > 0)
            //{
            //    string detail = "สมาชิกรายนี้ได้ทำการขอทุนไปแล้ว";
            //    string sqldocno = "select a.*,b.* from asnreqmaster a , asnreqcapitaldet b where a.assist_docno like 'AH%' and a.assist_docno = b.assist_docno and a.capital_year = b.capital_year and a.member_no = '"+member_no+"' order by a.capital_year";
            //    Sdt dt = WebUtil.QuerySdt(sqldocno);
            //    string hoard_case = "";
            //        for (int i = 0; i < dt.Rows.Count; i++) {
            //              if(dt.Rows[i]["hoard_case"].ToString() == "cripple_some"){
            //                hoard_case = "ทุพพลภาพบางส่วน";
            //             }
            //              else if(dt.Rows[i]["hoard_case"].ToString() == "cripple_all"){
            //              hoard_case = "ทุพพลภาพถาวร";
            //              }
            //              detail = detail + " ลำดับที่ " + (i + 1).ToString() + ". ปี " + dt.Rows[i]["capital_year"].ToString() + " " + hoard_case;
            //        }
            //    LtServerMessage.Text = WebUtil.WarningMessage(detail);
            //    return;//เดิม comment ไว้
            //}

            ChangeAmt();
        }
        
        private Decimal CheckReq(String member_no)
        {
            Sta ta2 = new Sta(sqlca.ConnectionString);
            try
            {
                String sqlStr2 = @"SELECT *
                           FROM   asnreqmaster
                           WHERE  member_no = '" + member_no + @"' and
                                  assist_docno like 'RF%'";
                Sdt dt2 = ta2.Query(sqlStr2);
                if (dt2.Next())
                {
                    try
                    {
                        rowcount = dt2.GetRowCount();
                    }
                    catch { rowcount = 0; }
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
            return rowcount;
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
            DwMain.SetItemDecimal(1, "req_status", 8);
            DwMain.SetItemDateTime(1, "pay_date", state.SsWorkDate);
            DwMain.SetItemDateTime(1, "req_date", state.SsWorkDate);
            DwMain.SetItemDateTime(1, "entry_date", state.SsWorkDate);
            //DwMain.SetItemDateTime(1, "approve_date", state.SsWorkDate);
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
            Sta ta = new Sta(sqlca.ConnectionString);
            Sdt dt = new Sdt();

            try
            {
                sqlStr = @"SELECT last_docno 
                               FROM   asndoccontrol
                               WHERE  doc_prefix = 'RF' and
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

                    assist_docno = "RF" + assist_docno;
                    assist_docno = assist_docno.Trim();
                }
                catch
                {
                    assist_docno = "RF000001";
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }

            return assist_docno;
        }

        private int CheckMemberDuration(string memberno)
        {
            int status = 1;
            string envvalue = "";
            DateTime center_date, member_date = DateTime.Today;
            string sql_notify = @"select envvalue
                           from asnsenvironmentvar
                           WHERE envcode = 'disabled_open_date'";
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
            //tDwMain.Eng2ThaiAllRow();
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
            DwMain.SetItemString(1, "member_age", s_member_age);

            DwMain.SetItemString(1, "member_age", s_member_age);
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
        public int GetLastPeriod(string member_no)
        {
            int last_peroid = 0;
            DataTable dt;
            sqlStr = "select last_period-1 as Last_period from shsharemaster where member_no = '" + member_no + "'";
            dt = WebUtil.QuerySdt(sqlStr);
            if (dt.Rows.Count > 0)
            {
                last_peroid = Convert.ToInt32(dt.Rows[0]["Last_period"].ToString());
            }
            return last_peroid;
        }
    }
}
