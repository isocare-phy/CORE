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
    public partial class w_sheet_as_member_dead_normal : PageWebSheet, WebSheet
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
        protected String postChangeAmt;
        protected String postRetreiveDwMem;
        protected String postRetrieveDwMain;
        protected String postGetMemberDetail;
        protected String postRetrieveBankBranch;
        protected String postDelete;
        protected String postChangeAge;
        protected String jsformatDeptaccid;
        protected string postSetBranch;
        protected String postSetMateName;
        private String sqlStr, envvalue;
        private String member_no,
                       assist_docno,
                       membgroup_code,
                       member_dead_tdate,
                       assisttype_code,
                       deptaccount_no,
                       tofrom_accid,
                       member_dead_case; //สาเหตุการถึงแก่กรรม :
        private String expense_bank,
                       expense_branch,
                       expense_accid,
                       moneytype_code,
                       remark_cancel;
        private Decimal capital_year,
                        salary_amt,
                        assist_amt,
                        req_status,
                        seq_pay,
                        rowcount;
        private DateTime pay_date,
                         cancel_date = DateTime.Now,
                         center_date,
                         req_date,
                         member_date,
                         birth_date,
                         member_age,
                         member_dead_date;
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
            postDelete = WebUtil.JsPostBack(this, "postDelete");
            postChangeAge = WebUtil.JsPostBack(this, "postChangeAge");
            postPopupReport = WebUtil.JsPostBack(this, "postPopupReport");
            postDepptacount = WebUtil.JsPostBack(this, "postDepptacount");
            postToFromAccid = WebUtil.JsPostBack(this, "postToFromAccid");
            jsformatDeptaccid = WebUtil.JsPostBack(this, "jsformatDeptaccid");
            postSetBranch = WebUtil.JsPostBack(this, "postSetBranch");
            postSetMateName = WebUtil.JsPostBack(this, "postSetMateName");

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


            DwMain.SetItemString(1, "assisttype_code", "30");



            DwUtil.RetrieveDDDW(DwMain, "assisttype_code", "as_capital.pbl", state.SsCoopId);
            DwUtil.RetrieveDDDW(DwMain, "capital_year", "as_capital.pbl", null);
            try { capital_year = DwMain.GetItemDecimal(1, "capital_year"); }
            catch { capital_year = state.SsWorkDate.Year + 543; }
            DwMain.SetItemDecimal(1, "capital_year", capital_year);
            //GetLastDocNo(capital_year);
            //DwMain.SetItemString(1, "assist_docno", assist_docno);
            DwUtil.RetrieveDDDW(DwMain, "cancel_id", "as_capital.pbl", null);
            DwUtil.RetrieveDDDW(DwMain, "req_status", "as_capital.pbl", null);
            DwUtil.RetrieveDDDW(DwMain, "pay_status", "as_capital.pbl", null);
            DwUtil.RetrieveDDDW(DwMain, "expense_bank", "as_capital.pbl", null);
            //DwUtil.RetrieveDDDW(DwMain, "moneytype_code", "as_capital.pbl", null);
            DwUtil.RetrieveDDDW(DwDetail, "moneytype_code", "as_capital.pbl", null);
            DwUtil.RetrieveDDDW(DwDetail, "expense_branch", "as_capital.pbl", null);
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
                SetItemDwDetailMembDead();
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
            else if (eventArg == "postSetBranch")
            {
                SetBankbranch();
            }
            else if (eventArg == "postSetMateName") { SetMateName(); }
        }

        private void SetMateName()
        {
            String member_recieve_type = "", mate_name = "";
            //ความสัมพันธ์ = 03: คู่สมรส
            try { member_recieve_type = DwDetail.GetItemString(1, "member_recieve_type").Trim(); }
            catch { }
            //คู่สมรส : mate_name
            try { mate_name = DwMem.GetItemString(1, "mate_name").Trim(); }
            catch { }
            //ชื่อผู้รับเงิน  - ถ้ามีคู่สมรส  ให้เซตค่า Default ไว้เลย ด้วยค่าของชื่อ คู่สมรส :
            if (member_recieve_type == "03") { DwDetail.SetItemString(1, "member_receive", mate_name); }
            else { DwDetail.SetItemString(1, "member_receive", ""); }
        }

        protected void SetBankbranch()
        {
            string expense_branch = "";
            try { expense_branch = DwDetail.GetItemString(1, "expense_branch"); }
            catch { }
            DwMain.SetItemString(1, "expense_bank", "034");
            DwMain.SetItemString(1, "expense_branch", expense_branch);
        }
        private void Delete()
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
                //ta.RollBack();
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถลบได้");
            }

            ta2.Close();
        }

        public void SaveWebSheet()
        {
            Sta ta2 = new Sta(state.SsConnectionString);
            ta2.Transection();

            Decimal member_age;
            String member_receive, member_dead_case, remark;
            DateTime member_dead_date, req_date;
            DwMain.SetItemString(1, "coop_id", state.SsCoopId);
            //DwMain.SetItemString(1, "coop_id", state.SsCoopId);
            try { deptaccount_no = DwDetail.GetItemString(1, "deptaccount_no"); }
            catch { deptaccount_no = ""; }
            try { moneytype_code = DwDetail.GetItemString(1, "moneytype_code"); }
            catch { moneytype_code = ""; }
            if ((moneytype_code == "CBT" || moneytype_code == "TBK") && moneytype_code != "")
            {
                if (deptaccount_no.Replace("-", "").Trim().Length != 12)
                {
                    Response.Write("<script language='javascript'>alert('กรุณากรอกเลขที่บัญชีให้ครบ 12 หลัก'\n );</script>");
                    return;
                }
            }
            setAccItemDwMain();

            unfomatAll();
            //try
            //{
            //    if (DwDetail.GetItemString(1, "card_person").Length != 13)
            //    {
            //        //LtServerMessage.Text = WebUtil.WarningMessage("กรุณากรอกเเลขบัตรประชาชนให้ครบ");
            //        Response.Write("<script language='javascript'>alert('กรุณากรอกเเลขบัตรประชาชนให้ครบ'\n );</script>");
            //        return;
            //    }
            //}
            //catch
            //{
            //    //LtServerMessage.Text = WebUtil.WarningMessage("กรุณากรอกเเลขบัตรประชาชนให้ครบ");
            //    Response.Write("<script language='javascript'>alert('กรุณากรอกเเลขบัตรประชาชนให้ครบ'\n );</script>");
            //    return;
            //}

            GetItemDwMain();

            //คู่สมรส : mate_name
            String mate_name = "", mate_cardperson = "";
            try { mate_name = DwMem.GetItemString(1, "mate_name").Trim(); }
            catch { }
            //เลขบัตรฯ คู่สมรส : mate_cardperson
            try { mate_cardperson = DwMem.GetItemString(1, "mate_cardperson").Trim(); }
            catch { }
            try
            {
                sqlStr = @" UPDATE  MBMEMBMASTER
                            SET     mate_name       = '" + mate_name + @"',
                                    mate_cardperson = '" + mate_cardperson.Replace("-", "") + @"'
                            WHERE   member_no       = '" + member_no + @"'";
                ta.Exe(sqlStr);
            }
            catch (Exception ex) { ex.ToString(); }

            assist_amt = DwDetail.GetItemDecimal(1, "assist_amt");
            string member_age2 = DwMain.GetItemString(1, "member_age");
            string[] text = member_age2.Split(' ');
            member_age = Convert.ToDecimal(text[0]);
            DwDetail.SetItemDecimal(1, "member_age", member_age);
            //try { member_age = DwDetail.GetItemDecimal(1, "member_age"); }
            //catch { }
            //กรณีเลือกการชำระเงิน = TRN: โอนภายในระบบ ไม่ต้องฟ้องเตือนว่า กรุณากรอก ชื่อผู้รับเงิน และ กรุณาเลือก ความสัมพันธ์เกี่ยวข้องเป็น ของผู้รับเงิน
            if (moneytype_code != "TRN" && moneytype_code != "")
            {
                try
                {
                    member_receive = DwDetail.GetItemString(1, "member_receive");
                }
                catch
                {
                    Response.Write("<script language='javascript'>alert('กรุณากรอก ชื่อผู้รับเงิน'\n );</script>");
                    return;
                }
                try
                {
                    member_dead_date = DwDetail.GetItemDateTime(1, "member_dead_date");
                }
                catch
                {
                    Response.Write("<script language='javascript'>alert('กรุณาเลือก ความสัมพันธ์เกี่ยวข้องเป็น ของผู้รับเงิน'\n );</script>");
                    return;
                }
            }
            try { member_dead_case = DwDetail.GetItemString(1, "member_dead_case"); }
            catch { member_dead_case = "ไม่ระบุ"; }

            try { remark = DwDetail.GetItemString(1, "remark"); }
            catch { remark = ""; }

            try { assist_docno = DwMain.GetItemString(1, "assist_docno"); }
            catch { assist_docno = ""; }

            if (assist_docno == "")
            {
                int flag = -1;
                //flag = CheckDeadBeforeSave();
                if (flag == -1)
                {
                    #region บันทึกใบคำขอใหม่
                    try { capital_year = DwMain.GetItemDecimal(1, "capital_year"); }
                    catch { capital_year = state.SsWorkDate.Year + 543; }

                    assist_docno = GetLastDocNo(capital_year);
                    seq_pay = GetSeqNo();

                    DwDetail.SetItemDecimal(1, "seq_pay", seq_pay);
                    DwDetail.SetItemString(1, "assist_docno", assist_docno);
                    DwDetail.SetItemDecimal(1, "capital_year", capital_year);
                    DwDetail.SetItemDateTime(1, "capital_date", state.SsWorkDate);
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
                        catch (Exception ex) { ex.ToString(); }

                        //try { DwUtil.InsertDataWindow(DwDetail, "as_capital.pbl", "asnreqcapitaldet"); }
                        //catch (Exception ex) { ex.ToString(); }
                        try { InsertDwDetial(); }
                        catch (Exception ex) { ex.ToString(); }


                        sqlStr = @" INSERT INTO asnmemsalary(capital_year,
                                                             member_no,
                                                             assist_docno,
                                                             salary_amt,
                                                             entry_id,
                                                             entry_date,
                                                             membgroup_code)
                                                      VALUES('" + capital_year + @"',
                                                             '" + member_no + @"',
                                                             '" + assist_docno + @"',
                                                             '" + salary_amt + @"',
                                                             '" + state.SsUsername + @"',
                                                             to_date('" + DwMain.GetItemDateTime(1, "entry_date").ToString("dd/MM/yyyy", WebUtil.EN) + @"', 'dd/mm/yyyy'),
                                                             '" + membgroup_code + @"')";
                        ta.Exe(sqlStr);

                        sqlStr = @"UPDATE  asndoccontrol
                               SET     last_docno   = '" + WebUtil.Right(assist_docno, 6) + @"'
                               WHERE   doc_prefix   = 'AM' and
                                       doc_year = '" + capital_year + "'";
                        ta.Exe(sqlStr);
                        try
                        {
                            //asnslippayout
                            sqlStr = "select * from asnucfpaytype where moneytype_code = '" + moneytype_code + "'";
                            try { deptaccount_no = DwMain.GetItemString(1, "deptaccount_no"); }
                            catch { deptaccount_no = ""; }
                            Sdt dtMoneytypecode = WebUtil.QuerySdt(sqlStr);
                            Sdt dtAmt = null;
                            string work_date = state.SsWorkDate.ToString("dd/MM/yyyy");
                            if (dtMoneytypecode.Next())
                            {
                                moneytype_code = dtMoneytypecode.Rows[0]["moneytype_code"].ToString().Trim();
                            }
                            //ดึงอายุการเป็นสมาชิก : มาเซตให้กับตัวแปร age_range
                            decimal age_range = Decimal.Parse(DwMain.GetItemString(1, "age_range"));
                            //age_range *= 12;//แปลงปีเป็นเดือน
                            sqlStr = "select * from asnsenvironmentvar where envgroup = 'member_dead_normal' and '" + age_range + "' between start_age and end_age";
                            dtAmt = WebUtil.QuerySdt(sqlStr);
                            // insert into asnslippayout
                            for (int i = 0; i < dtAmt.Rows.Count; i++)
                            {
                                int seq_no = i + 1;
                                sqlStr = @" INSERT INTO asnslippayout ( payoutslip_no,                    member_no,                      slip_date,                      operate_date,
                                                                           payout_amt,                  slip_status,                  depaccount_no,                   assisttype_code,
                                                                       moneytype_code,                     entry_id,                     entry_date,                            seq_no,
                                                                         capital_year,                sliptype_code,                   tofrom_accid
                                                                      )
                                            VALUES (          '" + assist_docno + @"',         '" + member_no + @"',to_date('" + DwMain.GetItemDateTime(1, "req_date").ToString("dd/MM/yyyy", WebUtil.EN) + @"', 'dd/mm/yyyy'),to_date('" + DwMain.GetItemDateTime(1, "entry_date").ToString("dd/MM/yyyy", WebUtil.EN) + @"', 'dd/mm/yyyy'),
                                                                '" + assist_amt + @"',                          '0',      '" + deptaccount_no + @"',                                 '" + assisttype_code + @"',
                                                            '" + moneytype_code + @"',  '" + state.SsUsername + @"',to_date('" + DwMain.GetItemDateTime(1, "entry_date").ToString("dd/MM/yyyy", WebUtil.EN) + @"', 'dd/mm/yyyy')," + seq_no + @",
                                                              '" + capital_year + @"','" + dtAmt.Rows[i]["system_code"] + @"','" + dtAmt.Rows[i]["tofrom_accid"] + @"')";
                                ta2.Exe(sqlStr);
                            }
                            ta2.Commit();
                        }
                        catch
                        {
                            ta2.RollBack();
                        }
                        UpdateDead();//ยกเลิกอนุมัติอาวุโส กรณีที่มีการร้องขอแต่ยังไม่ได้จ่ายเงิน
                        NewClear();
                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกเรียบร้อย");
                    }
                    catch (Exception ex)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                    }
                    #endregion
                }
                else
                {
                    //LtServerMessage.Text = WebUtil.WarningMessage("สมาชิกรายนี้ได้ทำรายการนี้แล้ว ไม่สามารถยื่นใบคำขอได้อีก");
                    infomatAll();
                    Response.Write("<script language='javascript'>alert('สมาชิกรายนี้ได้ทำรายการนี้แล้ว ไม่สามารถยื่นใบคำขอได้อีก'\n );</script>");
                    return;
                }
            }
            else
            {
                #region แก้ไขใบคำขอ
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

                    try
                    {
                        //DwUtil.UpdateDataWindow(DwDetail, "as_capital.pbl", "asnreqcapitaldet");
                        UpdateDwDetial();
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
                        sqlStr = "select * from asnucfpaytype where moneytype_code = '" + moneytype_code + "'";
                        try
                        {
                            deptaccount_no = DwMain.GetItemString(1, "deptaccount_no");
                        }
                        catch
                        {
                            deptaccount_no = "";
                        }
                        Sdt dtMoneytypecode = WebUtil.QuerySdt(sqlStr);
                        Sdt dtAmt = null;
                        string work_date = state.SsWorkDate.ToString("dd/MM/yyyy");
                        if (dtMoneytypecode.Next())
                        {
                            moneytype_code = dtMoneytypecode.Rows[0]["moneytype_code"].ToString().Trim();
                        }
                        //ดึงอายุการเป็นสมาชิก : มาเซตให้กับตัวแปร age_range
                        decimal age_range = Decimal.Parse(DwMain.GetItemString(1, "age_range"));
                        sqlStr = "select * from asnsenvironmentvar where envgroup = 'member_dead_normal' and '" + age_range + "' between start_age and end_age";
                        dtAmt = WebUtil.QuerySdt(sqlStr);
                        // update asnslippayout 
                        for (int i = 0; i < dtAmt.Rows.Count; i++)
                        {
                            int seq_no = i + 1;
                            sqlStr = @" UPDATE  asnslippayout 
                                        SET     slip_date       = to_date('" + DwMain.GetItemDateTime(1, "req_date").ToString("dd/MM/yyyy", WebUtil.EN) + @"', 'dd/mm/yyyy'),
                                                operate_date    = to_date('" + DwMain.GetItemDateTime(1, "entry_date").ToString("dd/MM/yyyy", WebUtil.EN) + @"', 'dd/mm/yyyy'),
                                                payout_amt      = '" + assist_amt + @"',
                                                depaccount_no   = '" + deptaccount_no + @"',
                                                moneytype_code  = '" + moneytype_code + @"' ,
                                                entry_id        = '" + state.SsUsername + @"',
                                                entry_date      = to_date('" + DwMain.GetItemDateTime(1, "entry_date").ToString("dd/MM/yyyy", WebUtil.EN) + @"', 'dd/mm/yyyy') 
                                        WHERE   payoutslip_no   = '" + assist_docno + @"' 
                                        AND     capital_year    = '" + capital_year + @"' 
                                        AND     seq_no          = " + seq_no + @"";
                            ta2.Exe(sqlStr);
                        }
                        ta2.Commit();
                    }
                    catch { ta2.RollBack(); }
                    NewClear();
                    LtServerMessage.Text = WebUtil.CompleteMessage("แก้ไขเรียบร้อย");
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
                #endregion
            }

            ta2.Close();
        }

        public void WebSheetLoadEnd()
        {
            //DwMain.SetItemDateTime(1, "pay_date", state.SsWorkDate);
            //DwMain.SetItemDateTime(1, "req_date", state.SsWorkDate);
            //DwMain.SetItemDateTime(1, "entry_date", state.SsWorkDate);
            //DwMain.SetItemDateTime(1, "approve_date", state.SsWorkDate);
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
            Decimal member_age2 = 0;
            Decimal start_age = 0;
            String member_dead_tdate;
            DateTime member_dead_date;
            DateTime birth_date;
            Decimal age_range = 0, assist_amt = 0, payout_amt = 0;
            decimal envvalue = 0, before_assist_amt = 0;
            int[] member_year = new int[3];

            member_no = HfMemberNo.Value;
            birth_date = DwMain.GetItemDateTime(1, "birth_date");

            #region CalAge
            try { member_dead_date = DwDetail.GetItemDateTime(1, "member_dead_date"); }
            catch { member_dead_date = state.SsWorkDate; }
            try { member_date = DwMain.GetItemDateTime(1, "member_date"); }
            catch { member_date = state.SsWorkDate; }
            member_year = clsCalAge(birth_date, member_dead_date);
            string member_age_range = member_year[2].ToString() + "." + member_year[1].ToString("00") + " ปี";
            member_year = clsCalAge(member_date, member_dead_date);
            string s_member_year = member_year[2].ToString() + " ปี " + member_year[1].ToString() + " เดือน " + member_year[0].ToString() + " วัน";
            DwMain.SetItemString(1, "member_years_disp", s_member_year);
            DwMain.SetItemString(1, "age_range", ((member_year[2] * 12) + member_year[1]).ToString());
            DwMain.SetItemString(1, "member_age", member_age_range);
            #endregion

            age_range = Decimal.Parse(DwMain.GetItemString(1, "age_range"));

            member_dead_tdate = DwDetail.GetItemString(1, "member_dead_tdate");
            member_dead_tdate = member_dead_tdate.Replace("/", "");
            member_dead_date = DateTime.ParseExact(member_dead_tdate, "ddMMyyyy", WebUtil.TH);

            DwDetail.SetItemDateTime(1, "member_dead_date", member_dead_date);

            tp = member_dead_date - member_date;

            //หาว่าเกิน 120 วันหรือไม่
            req_date = DwMain.GetItemDateTime(1, "req_date");
            tp = req_date - member_dead_date;
            Double a = (tp.TotalDays);
            //if (a > (120))//แก้ไขเพิ่มไป
            //{
            //Response.Write("<script language='javascript'>alert('สมาชิกรายนี้ยื่นคำขอเกินกว่า 120 วัน'\n );</script>");
            //return;
            //}

            try
            {
                birth_date = DwMain.GetItemDateTime(1, "birth_date");
                tp = member_dead_date - birth_date;
                Double member_age = (tp.TotalDays);
            }
            catch { }
            try
            {
                string[] text = (DwMain.GetItemString(1, "member_age").Trim()).Split(' ');
                member_age2 = Convert.ToDecimal(text[0]);
                member_age2 *= 12;
            }
            catch (Exception ex) { ex.ToString(); }

            Sta ta1 = new Sta(sqlca.ConnectionString);
            String sqlStr1 = "select * from asnsenvironmentvar where envgroup = 'senior_member_assist' ";
            Sdt dt1 = ta1.Query(sqlStr1);
            if (dt1.Next()) { start_age = Convert.ToDecimal(dt1.Rows[0]["start_age"].ToString()); }

            //check จากอายุการเป็นสมาชิก
            //age_range *= 12;//แปลงอายุการเป็นสมาชิกจากปี เป็น เดือน
            sqlStr = "select * from asnsenvironmentvar where envgroup = 'member_dead_normal' and '" + age_range + "' between start_age and end_age";
            dt = WebUtil.QuerySdt(sqlStr);
            if (dt.Next())
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    assist_amt = assist_amt + Decimal.Parse(dt.Rows[i]["envvalue"].ToString());
                    before_assist_amt = assist_amt;
                }

                DwDetail.SetItemDecimal(1, "assist_amt", assist_amt);

                DwDetail.SetItemDecimal(1, "before_assist_amt", before_assist_amt);
                DwDetail.SetItemDecimal(1, "envvalue", envvalue);
                DwDetail.SetItemDecimal(1, "total_amt", assist_amt);
            }
            //เช็คว่าจ่ายทุนไปแล้วเป็นยอดเงินเท่าไหร่
            sqlStr = @"  select  sum(payout_amt) as payout_amt
                         from    asnslippayout asd
                         where   exists
				                (select 1
				                from asnreqmaster
				                where assisttype_code = '30'
				                and	 assist_docno = asd.payoutslip_no
				                and	 capital_year = asd.capital_year
				                and	 capital_year = " + capital_year + @"
				                and	 member_no    = '" + member_no + @"'
				                )
                         and	slip_status = 1 ";
            dt = WebUtil.QuerySdt(sqlStr);
            if (dt.Next())
            {
                payout_amt = dt.GetDecimal("payout_amt");
                if (payout_amt > 0)
                {
                    DwDetail.SetItemDecimal(1, "assist_amt", (assist_amt - payout_amt)); //จำนวนเงินสวัสดิการ(ตามช่วงอายุการเป็นสมาชิก) - จำนวนเงินสวัสดิการ(จ่ายแล้ว)
                    LtServerMessage.Text = WebUtil.WarningMessage("สมาชิก เลขทะเบียน  " + member_no + "  ได้จ่ายทุนไปแล้วเป็นจำนวนเงิน:  " + payout_amt.ToString("#,##0.00") + "  บาท");
                }
            }
            #region หักกลบอาวุโส(ไม่ใช้)
            //            if (member_age2 >= start_age) //check อายุการเป็นสมาชิก ว่ามากกว่า 61 ปี หรือไม่?
            //            {
            //                ta1 = new Sta(sqlca.ConnectionString);
            //                sqlStr1 = @"SELECT * 
            //                            FROM asnsenvironmentvar 
            //                            WHERE envgroup = 'senior_member_assist' 
            //                            AND " + member_age2 + @" BETWEEN start_age AND end_age
            //                            ";
            //                dt1 = ta1.Query(sqlStr1);

            //                if (dt1.Next())
            //                {
            //                    sqlStr1 = @"SELECT assist_docno
            //                                       , (SELECT sum(assist_amt) FROM asnreqmaster
            //                                          WHERE member_no = '" + member_no + @"'
            //                                          AND assist_docno LIKE 'SM%'
            //                                          AND pay_status = '1') as assist_amt
            //                                FROM asnreqmaster
            //                                WHERE member_no = '" + member_no + @"'
            //                                AND assist_docno LIKE 'SM%'
            //                                AND pay_status = '1'
            //                                AND capital_year = '" + capital_year + @"'
            //                                ";
            //                    dt1 = ta1.Query(sqlStr1); //หาใบคำขอสมาชิกอาวุโสที่ได้รับการจ่ายเงินแล้ว***

            //                    if (dt1.Next())
            //                    {
            //                        String[] assist_Idocno = new String[dt1.GetRowCount()];
            //                        envvalue = dt1.GetRowCount();//นับปีที่ไดจ่ายไปแล้ว
            //                        assist_amt = assist_amt - dt1.GetDecimal("assist_amt");

            //                        for (int i = 0; i < dt1.GetRowCount(); i++)
            //                        {
            //                            assist_Idocno[i] = dt1.GetString("assist_docno");
            //                        }
            //                        HdOpenIFrame.Value = "true";

            //                    }
            //                    DwDetail.SetItemDecimal(1, "assist_amt", assist_amt);

            //                    DwDetail.SetItemDecimal(1, "before_assist_amt", before_assist_amt);
            //                    DwDetail.SetItemDecimal(1, "envvalue", envvalue);
            //                    DwDetail.SetItemDecimal(1, "total_amt", assist_amt);
            //                }
            //            }
            #endregion
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
                //LtAlert.Text = "<script>Alert()</script>";
                return;
            }

            GetMemberDetail();
        }

        private void RetrieveDwMain()
        {
            String assist_docno, member_no, card_person;
            Int32 capital_year;

            assist_docno = HfAssistDocNo.Value;
            capital_year = Convert.ToInt32(HfCapitalYear.Value);
            member_no = HfMemNo.Value;

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
            DwUtil.RetrieveDataWindow(DwMain, "as_capital.pbl", tDwMain, args2);
            try { deptaccount_no = DwMain.GetItemString(1, "deptaccount_no"); }
            catch { deptaccount_no = ""; }
            deptaccount_no = formatDeptaccid(deptaccount_no);
            DwMain.SetItemString(1, "deptaccount_no", deptaccount_no);

            object[] args3 = new object[2];
            args3[0] = assist_docno;
            args3[1] = capital_year;

            DwDetail.Reset();
            DwUtil.RetrieveDataWindow(DwDetail, "as_capital.pbl", null, args3);
            //เช็คว่า DwDetail มีจำนวนแถวมากกว่า  0 ใช่หรือไม่ :ถ้าใช่  ให้ Set การจ่ายเงิน คู่บัชี เลขบัญชี ลงในฟิวด์ DwDetail กรณีเปิดใบคำขอเดิม
            if (DwDetail.RowCount > 0) { setAccItemDwDetail(); }
            SetItemDwMainMembDead();

            RetrieveBankBranch();
            DateTime member_date, birth_date;
            int[] member_year = new int[3];
            decimal member_age = 0;
            //**
            //req_date = DwMain.GetItemDateTime(1, "req_date");
            try { member_dead_date = DwDetail.GetItemDateTime(1, "member_dead_date"); }
            catch { }
            member_date = DwMain.GetItemDateTime(1, "member_date");
            birth_date = DwMain.GetItemDateTime(1, "birth_date");

            member_year = clsCalAge(birth_date, member_dead_date);
            string member_age_range = member_year[2].ToString() + "." + member_year[1].ToString("00") + " ปี";

            member_year = clsCalAge(member_date, member_dead_date);
            string s_member_year = member_year[2].ToString() + " ปี " + member_year[1].ToString() + " เดือน " + member_year[0].ToString() + " วัน";

            DwMain.SetItemString(1, "member_years_disp", s_member_year);
            //member_age = wcf.Busscom.of_cal_yearmonth(state.SsWsPass, birth_date, req_date);

            //string s_member_age = member_year[2].ToString() + "." + member_year[1].ToString("00") + " ปี";
            DwMain.SetItemString(1, "member_age", member_age_range);
            DwMain.SetItemString(1, "age_range", ((member_year[2] * 12) + member_year[1]).ToString());
            //**
        }

        private void GetMemberDetail()
        {
            String member_no, prename_desc, memb_name, memb_surname, membgroup_code, membgroup_desc, membtype_code, membtype_desc, card_person, mate_name, mate_salaryid, mate_cardperson = "";
            DateTime member_date, birth_date;
            member_no = HfMemberNo.Value;
            Decimal salary_amount, member_age, resign_status;
            int[] member_year = new int[3];
            member_no = WebUtil.MemberNoFormat(member_no); //เซต Format เลขทะเบียน 8 หลัก #######X  EX. เลขทะเบียน: 0002513A

            DataStore Ds = new DataStore();
            Ds.LibraryList = "C:/GCOOP_ALL/CORE/GCOOP/Saving/DataWindow/app_assist/as_capital.pbl";
            Ds.DataWindowObject = "d_member_detail";
            Ds.SetTransaction(sqlca);
            Ds.Retrieve(member_no);

            if (Ds.RowCount < 1)
            {
                //LtServerMessage.Text = WebUtil.WarningMessage("ไม่มีเลขที่สมาชิกนี้");
                Response.Write("<script language='javascript'>alert('ไม่มีเลขที่สมาชิกนี้'\n );</script>");
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
            try { card_person = Ds.GetItemString(1, "card_person"); } //ดึงหมายเลขบัตรประชาชน: มาเซตค่าให้กับตัวแปร card_person กรณี ไม่มีให้เซตเป็นค่าว่าง
            catch { card_person = ""; }
            try { mate_name = Ds.GetItemString(1, "mate_name"); }
            catch { mate_name = ""; }
            try { mate_salaryid = Ds.GetItemString(1, "mate_salaryid"); }
            catch { mate_salaryid = ""; }
            //เลขบัตรฯ คู่สมรส : mate_cardperson
            try { mate_cardperson = Ds.GetItemString(1, "mate_cardperson").Trim(); }
            catch { }
            try { resign_status = Ds.GetItemDecimal(1, "resign_status"); }
            catch { resign_status = 0; }
            //if (resign_status == 1)
            //{
            //    Response.Write("<script language='javascript'>alert('สมาชิก เลขทะเบียน " + member_no + " ได้ลาออกไปแล้ว'\n );</script>");
            //    return;
            //}
            //else
            //{
            //เช็คว่า สมาชิกท่านนี้ได้ลาออกแล้วหรือยัง :ถ้าใช่ให้แจ้งเตือน เช่น สมาชิก เลขทะเบียน 00018963 ได้ลาออกไปแล้ว!
            if (resign_status == 1)
            {
                String sqlResign = @"select  mr.resigncause_desc as resigncause_desc
                                     from 	 mbmembmaster mb, mbucfresigncause mr
                                     where   mb.member_no        = '" + member_no + @"'
                                     and	 mb.coop_id	         = '" + state.SsCoopId + @"'
                                     and	 mb.coop_id		     = mr.coop_id
                                     and	 mb.resigncause_code = mr.resigncause_code
                                     ";
                Sdt dtResign = WebUtil.QuerySdt(sqlResign);
                String resigncause_desc = "";
                if (dtResign.Next()) { resigncause_desc = dtResign.GetString("resigncause_desc").Trim(); }
                LtServerMessage.Text = WebUtil.WarningMessage("สมาชิก เลขทะเบียน  " + member_no + "  " + resigncause_desc);
            }
            else { LtServerMessage.Text = WebUtil.ErrorMessage("สมาชิก เลขทะเบียน  " + member_no + "  ไม่สามารถขอทุนได้  เนื่องจากยังไม่พ้นสภาพการเป็นสมาชิก"); return; }
            member_year = clsCalAge(birth_date, state.SsWorkDate);
            string member_age_range = member_year[2].ToString() + "." + member_year[1].ToString("00") + " ปี";
            member_year = clsCalAge(member_date, state.SsWorkDate);
            string s_member_year = member_year[2].ToString() + " ปี " + member_year[1].ToString() + " เดือน " + member_year[0].ToString() + " วัน";

            DwMain.SetItemString(1, "member_years_disp", s_member_year);
            //member_age = wcf.Busscom.of_cal_yearmonth(state.SsWsPass, birth_date, state.SsWorkDate);
            //string s_member_age = member_age.ToString() + " ปี";
            DwMem.SetItemString(1, "member_age", member_age_range);
            DwMain.SetItemString(1, "age_range", ((member_year[2] * 12) + member_year[1]).ToString());

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
            DwDetail.SetItemString(1, "card_person", card_person);
            DwMem.SetItemString(1, "mate_name", mate_name);
            DwMem.SetItemString(1, "mate_salaryid", mate_salaryid);
            mate_cardperson = formatIDCard(mate_cardperson);
            DwMem.SetItemString(1, "mate_cardperson", mate_cardperson);
            DwMain.SetItemDateTime(1, "member_date", member_date);
            DwMain.SetItemDateTime(1, "birth_date", birth_date);
            DwMain.SetItemDecimal(1, "salary_amt", salary_amount);
            tDwMain.Eng2ThaiAllRow();

            String sqlChk = "select * from asnreqmaster where assist_docno like 'AM%' and member_no = '" + member_no + "' and (approve_status is null or approve_status = 1)";
            Sdt dtChk = WebUtil.QuerySdt(sqlChk);
            if (dtChk.Next())
            {
                Response.Write("<script language='javascript'>alert('สมาชิก เลขทะเบียน " + member_no + " ได้มีการบันทึกใบคำขอสวัสดิการแล้ว'\n );</script>");
            }

            //rowcount = CheckReq(member_no);
            //if (rowcount > 0)
            //{
            //    LtServerMessage.Text = WebUtil.WarningMessage("สมาชิกรายนี้ขอทุนไปแล้ว");
            //    return;//เดิม comment ไว้
            //}

            ChangeAmt();
            SetDefaultAccid();
            //เลขที่บัญชี – ให้เว้นว่างไว้
            //deptaccount_no = SetdefaultDeptacc(member_no, state.SsCoopId);
            //deptaccount_no = formatDeptaccid(deptaccount_no);
            //DwMain.SetItemString(1, "deptaccount_no", deptaccount_no);
            //DwDetail.SetItemString(1, "deptaccount_no", deptaccount_no);
            //if (mate_name != null && mate_name != "")
            //{
            //    //ชื่อผู้รับเงิน  - ถ้ามีคู่สมรส  ให้เซตค่า Default ไว้เลย ด้วยค่าของชื่อ คู่สมรส :
            //    DwDetail.SetItemString(1, "member_receive", mate_name);
            //    //ความสัมพันธ์ = 03: คู่สมรส
            //    DwDetail.SetItemString(1, "member_recieve_type", "03");
            //}
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


            DwMain.Reset();
            DwMem.Reset();
            DwDetail.Reset();

            DwMem.InsertRow(0);
            DwMain.InsertRow(0);
            DwDetail.InsertRow(0);

            DwMem.SaveDataCache();
            DwMain.SaveDataCache();
            DwDetail.SaveDataCache();

            DwMain.SetItemDecimal(1, "req_status", 8);

            DwMain.SetItemDateTime(1, "approve_date", state.SsWorkDate);
            DwMain.SetItemDateTime(1, "req_date", state.SsWorkDate);
            DwMain.SetItemDateTime(1, "entry_date", state.SsWorkDate);
            //DwDetail.SetItemDateTime(1, "member_dead_tdate", state.SsWorkDate); 
            DwDetail.SetItemDateTime(1, "member_dead_date", state.SsWorkDate);
            SetItemDwMainMembDead();
            // DwMain.SetItemDateTime(1, "pay_date", state.SsWorkDate);
            SetDefaultAccid();
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
            birth_date = DwMain.GetItemDateTime(1, "birth_date");
            assisttype_code = DwMain.GetItemString(1, "assisttype_code");
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
                birth_date = DwMain.GetItemDateTime(1, "birth_date");
            }
            catch { }
            try { member_dead_case = DwMain.GetItemString(1, "member_dead_case"); }
            catch { member_dead_case = ""; }
            DwDetail.SetItemString(1, "member_dead_case", member_dead_case); //เซตค่าให้กับ Field = member_dead_case สาเหตุการถึงแก่กรรม :
        }

        /*        private String GetLastDocNo(Decimal capital_year)
                {

                    //capital_year = 2555; //เพิ่มเพื่อทดสอบ
                    //try { capital_year = DwMain.GetItemDecimal(1, "capital_year"); }
                    //catch { capital_year = state.SsWorkDate.Year; }
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
        */
        private String GetLastDocNo(Decimal capital_year)
        {
            String doc_year = "";
            //capital_year = 2555; //เพิ่มเพื่อทดสอบ
            //try { capital_year = DwMain.GetItemDecimal(1, "capital_year"); }
            //catch { capital_year = state.SsWorkDate.Year; }
            try
            {
                doc_year = WebUtil.Right(capital_year.ToString(), 2);
                sqlStr = @" SELECT substr( doc_year, 3, 2 ) || last_docno as last_docno 
                            FROM   asndoccontrol
                            WHERE  doc_prefix   = 'AM' 
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
                    assist_docno = "AM" + assist_docno;
                    assist_docno = assist_docno.Trim();
                }
                catch
                {
                    assist_docno = "AM" + doc_year + "000001";
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
                           WHERE envcode = 'memdead_open_date'";
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
            member_dead_date = DwDetail.GetItemDateTime(1, "member_dead_date");

            member_year = clsCalAge(birth_date, member_dead_date);
            string member_age_range = member_year[2].ToString() + "." + member_year[1].ToString("00") + " ปี";
            member_year = clsCalAge(member_date, member_dead_date);
            string s_member_year = member_year[2].ToString() + " ปี " + member_year[1].ToString() + " เดือน " + member_year[0].ToString() + " วัน";

            DwMain.SetItemString(1, "member_years_disp", s_member_year);
            //member_age = wcf.Busscom.of_cal_yearmonth(state.SsWsPass, birth_date, Req_date);
            //string s_member_age = member_age.ToString() + " ปี";

            DwMem.SetItemString(1, "member_age", member_age_range);
            DwMain.SetItemString(1, "age_range", ((member_year[2] * 12) + member_year[1]).ToString());
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
                //CoreSavingLibrary.WcfReport.ReportClient lws_report = wcf.Report;
                //String criteriaXML = lnv_helper.PopArgumentsXML();
                //this.pdf = lws_report.GetPDFURL(state.SsWsPass) + pdfFileName;
                //String li_return = lws_report.RunWithID(state.SsWsPass, app, gid, rid, state.SsUsername, criteriaXML, pdfFileName);
                //if (li_return == "true")
                //{
                //    HdOpenIFrame.Value = "True";
                //}
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
            deptaccount_no = formatDeptaccid(deptaccount_no);
            DwMain.SetItemString(1, "deptaccount_no", deptaccount_no);
            //try { member_no = DwMem.GetItemString(1, "member_no"); }
            //catch { member_no = ""; }
            //sqlStr = @" SELECT expense_accid 
            //            FROM mbmembmaster
            //            WHERE member_no = '" + member_no + @"'";
            //dt = ta.Query(sqlStr);

            //if (dt.Next())
            //{
            //    expense_accid = dt.GetString("expense_accid");
            //}
            //DwMain.SetItemString(1, "deptaccount_no", expense_accid);
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
                DwDetail.Describe("t_24.Visible");
                DwDetail.Modify("t_24.Visible=false");
                DwDetail.Describe("tofrom_accid.Visible");
                DwDetail.Modify("tofrom_accid.Visible=false");
                DwDetail.Describe("t_25.Visible");
                DwDetail.Modify("t_25.Visible=false");
                DwDetail.Describe("deptaccount_no.Visible");
                DwDetail.Modify("deptaccount_no.Visible=false");
                DwDetail.Describe("t_26.Visible");
                DwDetail.Modify("t_26.Visible=false");
                DwDetail.Describe("expense_branch.Visible");
                DwDetail.Modify("expense_branch.Visible=false");                
            }
            else if (moneytype_code == "TAL") { DwDetail.SetItemString(1, "tofrom_accid", ""); }//TAL = โอนชำระหนี้
        }

        private void UpdateDead()
        {
            try
            {//หารายการอนุมัติของสมาชิก กรณียังไม่ได้จ่ายเงิน
                sqlStr = @" SELECT *
                            FROM asnreqmaster
                            WHERE member_no = '" + member_no + @"'
                            AND assist_docno LIKE 'SM%'
                            AND pay_status = '8'
                            AND capital_year = '" + capital_year + @"'
                          ";
                dt = ta.Query(sqlStr);
            }
            catch (Exception ex) { ex.ToString(); }

            if (dt.Next())
            {
                try
                {//ยกเลิกใบคำขออนุมัติในปีนั้นๆ กรณียังไม่ได้จ่ายเงิน                
                    sqlStr = @" UPDATE asnreqmaster 
                                SET req_status = '-1'
                                ,pay_status = '-1'
                                WHERE member_no = '" + member_no + @"'
                                AND assist_docno LIKE 'SM%'
                                AND pay_status = '8'
                                AND capital_year = '" + capital_year + @"'
                              ";
                    ta.Exe(sqlStr);
                }
                catch (Exception ex) { ex.ToString(); }
            }
        }

        private int CheckDeadBeforeSave()
        {
            try
            {//ตรวจสอบว่าเคยทำรายการนี้แล้วหรือยัง?
                sqlStr = @" SELECT * 
                            FROM asnreqmaster 
                            WHERE member_no = '" + member_no + @"'
                            AND assist_docno LIKE 'AM%'
                            AND approve_status = 1
                          ";
                dt = ta.Query(sqlStr);
            }
            catch (Exception ex) { ex.ToString(); }

            if (dt.Next()) { return 1; }
            else { return -1; }
        }
        private void SetDefaultAccid()
        {
            //DwMain.SetItemString(1, "moneytype_code", "CBT");
            DwDetail.SetItemString(1, "moneytype_code", "CBT");
            PostToFromAccid();
            //DwMain.SetItemString(1, "tofrom_accid", "11035200");
            DwDetail.SetItemString(1, "tofrom_accid", "11035200");
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
            string card_person;
            card_person = DwMem.GetItemString(1, "card_person");
            card_person = unfomatIDCard(card_person);
            DwMem.SetItemString(1, "card_person", card_person);

            deptaccount_no = DwMain.GetItemString(1, "deptaccount_no");
            deptaccount_no = unformatDeptaccid(deptaccount_no);
            DwMain.SetItemString(1, "deptaccount_no", deptaccount_no);

            //card_person = DwDetail.GetItemString(1, "card_person");
            //card_person = unformatDeptaccid(card_person);
            //DwDetail.SetItemString(1, "card_person", card_person);
        }

        private void infomatAll()
        {
            string card_person;
            card_person = DwMem.GetItemString(1, "card_person");
            card_person = formatIDCard(card_person);
            DwMem.SetItemString(1, "card_person", card_person);

            deptaccount_no = DwDetail.GetItemString(1, "deptaccount_no");
            deptaccount_no = formatDeptaccid(deptaccount_no);
            DwDetail.SetItemString(1, "deptaccount_no", deptaccount_no);

            //card_person = DwDetail.GetItemString(1, "card_person");
            //card_person = formatIDCard(card_person);
            //DwDetail.SetItemString(1, "card_person", card_person);
        }

        private void CheckSameAssist(string member_no, string capital_year)
        {
            sqlStr = @" SELECT * 
                        FROM asnreqmaster 
                        WHERE member_no = '" + member_no + @"' 
                        AND capital_year = '" + capital_year + @"' 
                        AND assist_docno like 'AM%'
                      ";
            dt = ta.Query(sqlStr);
            if (dt.Next())
            {
                Response.Write("<script language='javascript'>alert('');</script>");
                return;
            }
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
            catch { capital_year = state.SsWorkDate.Year + 543; }
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
            string remark = ""                       //หมายเหตุ
                   , capital_date = ""               //วันที่ยื่นใบคำขอ
                   , member_dead_date = ""           //วันที่เสียชีวิต
                   , member_dead_case = ""           //สาเหตุการเสียชีวิต
                   , member_receive = ""             //ชื่อผู้รับเงิน
                   , card_person = ""                //เลขประชาชนผู้รับเงิน
                   , member_recieve_type = ""        //ความสัมพันธ์
                   , assist_docno = ""               //เลขที่ใบคำขอ
                   ;
            decimal assist_amt = 0                   //รวมเงินสวัสดิการ
                   , member_age = 0                  //อายุสมาชิก
                   , clearloan_amt = 0               //
                   , seq_pay = 0                     //
                   , capital_year = 0                //ปีทุนสวัสดิการ
                   ;
            DateTime member_dead_tdate
                    , capital_tdate
                   ;

            #region GetItemBeforeSave
            try { remark = DwDetail.GetItemString(1, "remark"); }
            catch { remark = ""; }
            try { capital_tdate = DwDetail.GetItemDateTime(1, "capital_date"); }
            catch { capital_tdate = state.SsWorkDate; }
            capital_date = capital_tdate.ToString("dd/MM/yyyy", WebUtil.EN);
            try { member_dead_tdate = DwDetail.GetItemDateTime(1, "member_dead_date"); }
            catch { member_dead_tdate = state.SsWorkDate; }
            member_dead_date = member_dead_tdate.ToString("dd/MM/yyyy", WebUtil.EN);
            try { member_dead_case = DwDetail.GetItemString(1, "member_dead_case"); }
            catch { member_dead_case = ""; }
            try { member_receive = DwDetail.GetItemString(1, "member_receive"); }
            catch { member_receive = ""; }
            try { card_person = DwDetail.GetItemString(1, "card_person"); }
            catch { card_person = ""; }
            card_person = unfomatIDCard(card_person);
            try { member_recieve_type = DwDetail.GetItemString(1, "member_recieve_type"); }
            catch { member_recieve_type = ""; }
            try { assist_docno = DwDetail.GetItemString(1, "assist_docno"); }
            catch { assist_docno = ""; }
            try { assist_amt = DwDetail.GetItemDecimal(1, "assist_amt"); }
            catch { assist_amt = 0; }
            try { member_age = DwDetail.GetItemDecimal(1, "member_age"); }
            catch { member_age = 0; }
            try { clearloan_amt = DwDetail.GetItemDecimal(1, "clearloan_amt"); }
            catch { clearloan_amt = 0; }
            try { seq_pay = DwDetail.GetItemDecimal(1, "seq_pay"); }
            catch { seq_pay = 0; }
            try { capital_year = DwDetail.GetItemDecimal(1, "capital_year"); }
            catch { capital_year = 0; }
            #endregion

            sqlStr = @"INSERT INTO asnreqcapitaldet (
                                                    remark                  ,assist_amt                     ,capital_date                   ,member_dead_date
                                                    ,member_age             ,member_dead_case               ,member_receive                 ,clearloan_amt
                                                    ,seq_pay                ,card_person                    ,member_recieve_type            ,assist_docno
                                                    ,capital_year           ,damagetype_code                ,subdamagetype_code
                                                    ) 
                                              VALUES(
                                                    '" + remark + @"'       ,'" + assist_amt + @"'          ,to_date('" + capital_date + @"','dd/mm/yyyy')          ,to_date('" + member_dead_date + @"','dd/mm/yyyy')
                                                    ,'" + member_age + @"'  ,'" + member_dead_case + @"'    ,'" + member_receive + @"'      ,'" + clearloan_amt + @"'
                                                    ,'" + seq_pay + @"'     ,'" + card_person + @"'         ,'" + member_recieve_type + @"' ,'" + assist_docno + @"'
                                                    ,'" + capital_year + @"','02'                           ,'02'
                                                    )";
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
            catch { capital_year = state.SsWorkDate.Year + 543; }
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
            string remark = ""                       //หมายเหตุ
                   , capital_date = ""               //วันที่ยื่นใบคำขอ
                   , member_dead_date = ""           //วันที่เสียชีวิต
                   , member_dead_case = ""           //สาเหตุการเสียชีวิต
                   , member_receive = ""             //ชื่อผู้รับเงิน
                   , card_person = ""                //เลขประชาชนผู้รับเงิน
                   , member_recieve_type = ""        //ความสัมพันธ์
                   , assist_docno = ""               //เลขที่ใบคำขอ
                   ;
            decimal assist_amt = 0                   //รวมเงินสวัสดิการ
                   , member_age = 0                  //อายุสมาชิก
                   , clearloan_amt = 0               //
                   , seq_pay = 0                     //
                   , capital_year = 0                //ปีทุนสวัสดิการ
                   ;
            DateTime member_dead_tdate
                    , capital_tdate
                   ;

            #region GetItemBeforeSave
            try { remark = DwDetail.GetItemString(1, "remark"); }
            catch { remark = ""; }
            try { capital_tdate = DwDetail.GetItemDateTime(1, "capital_date"); }
            catch { capital_tdate = state.SsWorkDate; }
            capital_date = capital_tdate.ToString("dd/MM/yyyy", WebUtil.EN);
            try { member_dead_tdate = DwDetail.GetItemDateTime(1, "member_dead_date"); }
            catch { member_dead_tdate = state.SsWorkDate; }
            member_dead_date = member_dead_tdate.ToString("dd/MM/yyyy", WebUtil.EN);
            try { member_dead_case = DwDetail.GetItemString(1, "member_dead_case"); }
            catch { member_dead_case = ""; }
            try { member_receive = DwDetail.GetItemString(1, "member_receive"); }
            catch { member_receive = ""; }
            try { card_person = DwDetail.GetItemString(1, "card_person"); }
            catch { card_person = ""; }
            card_person = unfomatIDCard(card_person);
            try { member_recieve_type = DwDetail.GetItemString(1, "member_recieve_type"); }
            catch { member_recieve_type = ""; }
            try { assist_docno = DwDetail.GetItemString(1, "assist_docno"); }
            catch { assist_docno = ""; }
            try { assist_amt = DwDetail.GetItemDecimal(1, "assist_amt"); }
            catch { assist_amt = 0; }
            try { member_age = DwDetail.GetItemDecimal(1, "member_age"); }
            catch { member_age = 0; }
            try { clearloan_amt = DwDetail.GetItemDecimal(1, "clearloan_amt"); }
            catch { clearloan_amt = 0; }
            try { seq_pay = DwDetail.GetItemDecimal(1, "seq_pay"); }
            catch { seq_pay = 0; }
            try { capital_year = DwDetail.GetItemDecimal(1, "capital_year"); }
            catch { capital_year = 0; }
            #endregion

            sqlStr = @" UPDATE asnreqcapitaldet SET
                        remark = '" + remark + @"'
                        , member_dead_case = '" + member_dead_case + @"'
                        , member_receive = '" + member_receive + @"'
                        , card_person = '" + card_person + @"'
                        , member_recieve_type = '" + member_recieve_type + @"'
                        , assist_amt = '" + assist_amt + @"'
                        , member_age = '" + member_age + @"'
                        , clearloan_amt = '" + clearloan_amt + @"'
                        , capital_date = to_date('" + capital_date + @"','dd/MM/yyyy')
                        , member_dead_date = to_date('" + member_dead_date + @"','dd/MM/yyyy')
                        
                         WHERE assist_docno = '" + assist_docno + @"'
                         AND capital_year = '" + capital_year + @"'
                       ";
            ta.Exe(sqlStr);
        }

        /// <summary>
        /// MiW::10/04/2014
        /// Set การจ่ายเงิน คู่บัชี เลขบัญชี ลงในฟิวด์ DwMain เพื่อรอ Save
        /// </summary>
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

        /// <summary>
        /// MiW::10/04/2014
        /// Set การจ่ายเงิน คู่บัชี เลขบัญชี ลงในฟิวด์ DwDetail กรณีเปิดใบคำขอเดิม
        /// </summary>
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
            DwDetail.SetItemDateTime(1, "member_dead_date", dt);
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

            try { date = (DwDetail.GetItemDate(1, "member_dead_date")).ToString("dd/MM/yyyy", WebUtil.TH); }
            catch { date = (state.SsWorkDate).ToString("dd/MM/yyyy", WebUtil.TH); }
            date = date.Replace("/", "");
            day = Convert.ToInt16(date.Substring(0, 2));
            month = Convert.ToInt16(date.Substring(2, 2));
            year = Convert.ToInt16(date.Substring(4, 4)) - 543;
            dt = new DateTime(year, month, day);
            date = day.ToString("00") + "/" + month.ToString("00") + "/" + (year + 543).ToString("0000");
            DwMain.SetItemString(1, "member_dead_tdate", date);
        }

    }
}