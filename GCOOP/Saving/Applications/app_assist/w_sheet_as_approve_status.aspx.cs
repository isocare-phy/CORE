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
    public partial class w_sheet_as_approve_status : PageWebSheet, WebSheet
    {
        protected DwThDate tDwMain;
        protected string postSearchList;
        protected string postApprove;
        protected string postNotApprove;
        protected string ls_sql;

        Sta ta;

        protected string sqlStr;
        public void InitJsPostBack()
        {
            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("approve_date", "approve_tdate");
            postSearchList = WebUtil.JsPostBack(this, "postSearchList");
            postApprove = WebUtil.JsPostBack(this, "postApprove");
            postNotApprove = WebUtil.JsPostBack(this, "postNotApprove");
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
            else if (eventArg == "postApprove") { JsPostApprove(); }
            else if (eventArg == "postNotApprove") { JsPostNotApprove(); }
        }

        private void JsPostApprove()
        {
            int rowTotal = DwMain.RowCount;
            for (var i = 1; i <= rowTotal; i++)
            {
                Decimal req_status = DwMain.GetItemDecimal(i, "req_status");
                if (req_status == 1)
                {
                    DwMain.SetItemDecimal(i, "approve_status", 1);
                }
            }
        }

        private void JsPostNotApprove()
        {
            int rowTotal = DwMain.RowCount;
            for (var i = 1; i <= rowTotal; i++)
            {
                Decimal req_status = DwMain.GetItemDecimal(i, "req_status");
                if (req_status == 1)
                {
                    DwMain.SetItemDecimal(i, "approve_status", 0);
                }
            }
        }
        
        public void SaveWebSheet()
        {
            decimal req_status = 0, capital_year = 0, assist_amt = 0;
            decimal approve_status = 0, clearloan_flag = 0, withdrawable_amt = 0;
            string assist_docno, moneytype_code, astype;
            DateTime approve_date;
            // check dept close
            for (int i = 0; i < DwMain.RowCount; i++)
            {
                int currRowi = i + 1;
                req_status = DwMain.GetItemDecimal(currRowi, "req_status");
                assist_docno = DwMain.GetItemString(currRowi, "assist_docno");
                capital_year = DwMain.GetItemDecimal(currRowi, "capital_year");
                moneytype_code = DwMain.GetItemString(currRowi, "moneytype_code");
                #region ตรวจสอบสถานะปิดบัญชี (ยังไม่ใช้)

                //                if (paytype_code == "04" && req_status != 8)
                //                {
                //                    bool close_status;
                //                    string deptaccount_no = DwMain.GetItemString(currRowi, "deptaccount_no");
                //                    close_status = CheckDeptClose(deptaccount_no);
                //                    if (close_status)
                //                    {
                //                        LtServerMessage.Text = WebUtil.ErrorMessage("หมายเลขบัญชี '" + deptaccount_no + "' อยู่ในสถานะปิดบัญชีแล้ว");
                //                        try
                //                        {
                //                            sqlStr = @"update asnreqmaster 
                //                                    set req_status = '-1'
                //                                    where assist_docno = '" + assist_docno + @"' 
                //                                    and capital_year = '" + capital_year + @"'";
                //                            ta.Exe(sqlStr);
                //                        }
                //                        catch (Exception ex) { ex.ToString(); }
                //                        return;
                //                    }
                //                }

                #endregion

                #region วน update status นะจ๊ะ (ต้องแก้ไข)

                for (int j = 0; j < DwMain.RowCount; j++)
                {
                    int currRowj = j + 1;
                    clearloan_flag = 0;
                    withdrawable_amt = 0;

                    req_status = DwMain.GetItemDecimal(currRowj, "req_status");
                    assist_docno = DwMain.GetItemString(currRowj, "assist_docno");
                    capital_year = DwMain.GetItemDecimal(currRowj, "capital_year");
                    //try { paytype_code = DwType.GetItemString(1, "paytype_code"); }
                    //catch { paytype_code = ""; }
                    try { moneytype_code = DwType.GetItemString(1, "moneytype_code"); }
                    catch { moneytype_code = ""; }
                    assist_amt = DwMain.GetItemDecimal(currRowj, "assist_amt");
                    try
                    {
                        approve_status = DwMain.GetItemDecimal(currRowj, "approve_status");
                        req_status = DwMain.GetItemDecimal(currRowj, "approve_status");
                    }
                    catch { approve_status = 0; }

                    #region oldCode 18/10/2556
                    //                    if (paytype_code == "01" && req_status != 8)
                    //                    {
                    //                        // update asnreqmaster
                    //                        try { approve_date = DwMain.GetItemDateTime(currRowj, "approve_date"); }
                    //                        catch { approve_date = state.SsWorkDate; }
                    //                        try
                    //                        {
                    //                            sqlStr = @" update asnreqmaster 
                    //                                        set approve_date = to_date('" + approve_date.ToString("MM/dd/yyyy", WebUtil.EN) + @"','MM/dd/yyyy')
                    //                                        ,approve_status = 1
                    //                                        ,req_status = '" + req_status + @"'
                    //                                        ,pay_status = '8' 
                    //                                        where assist_docno = '" + assist_docno + @"' 
                    //                                        and capital_year = '" + capital_year + @"'";
                    //                            ta.Exe(sqlStr);
                    //                        }
                    //                        catch { }
                    //                        try
                    //                        {
                    //                            // update asnslippayout
                    //                            sqlStr = @" update asnslippayout 
                    //                                        set slip_status = 1 
                    //                                        where payoutslip_no = '" + assist_docno + @"' 
                    //                                        and capital_year = '" + capital_year + @"'";
                    //                            ta.Exe(sqlStr);
                    //                        }
                    //                        catch { }
                    //                    }
                    //                    else if (paytype_code == "04" && req_status != 8)
                    //                    {
                    //                        try { approve_date = DwMain.GetItemDateTime(currRowj, "approve_date"); }
                    //                        catch { approve_date = state.SsWorkDate; }
                    //                        // update asnreqmaster
                    //                        try
                    //                        {
                    //                            sqlStr = @" update asnreqmaster 
                    //                                        set approve_date = to_date('" + approve_date.ToString("MM/dd/yyyy", WebUtil.EN) + @"','MM/dd/yyyy')
                    //                                        ,approve_status = 1
                    //                                        ,req_status = '" + req_status + @"'
                    //                                        ,pay_status = '8'  
                    //                                        where assist_docno = '" + assist_docno + @"' 
                    //                                        and capital_year = '" + capital_year + @"'";
                    //                            ta.Exe(sqlStr);
                    //                        }
                    //                        catch { }
                    //                        try
                    //                        {
                    //                            // update asnslippayout
                    //                            sqlStr = @" update asnslippayout 
                    //                                        set slip_status = 1 
                    //                                        where payoutslip_no = '" + assist_docno + @"' 
                    //                                        and capital_year = '" + capital_year + "'";
                    //                            ta.Exe(sqlStr);
                    //                        }
                    //                        catch { }
                    //                    }
                    #endregion

                    try { approve_date = DwMain.GetItemDateTime(currRowj, "approve_date"); }
                    catch { approve_date = state.SsWorkDate; }
                    //try { astype = DwType.GetItemString(1, "astype"); }
                    //catch { astype = ""; }
                    try { astype = DwMain.GetItemString(currRowj, "assisttype_code").Trim(); }
                    catch { astype = ""; }

                    if (approve_status == -1 || approve_status == 0)
                    {
                        //req_status = 1;
                        withdrawable_amt = assist_amt;
                    }

                    if (astype == "30") //กรณีประเภท 30 = สมาชิกถึงแก่กรรม ให้ทำรายการโอนชำระหนี้
                    {
                        clearloan_flag = 1; //1 = โอนชำระหนี้
                        withdrawable_amt = assist_amt; //สวัสดิการคงเหลือ
                    }

                    if (approve_status == 0) { string mm = ",approve_date = to_date('" + approve_date.ToString("MM/dd/yyyy", WebUtil.EN) + @"','MM/dd/yyyy')"; }
                    // update asnreqmaster
                    try
                    {
                        sqlStr = @" update asnreqmaster
                                        set approve_date = to_date('" + approve_date.ToString("MM/dd/yyyy", WebUtil.EN) + @"','MM/dd/yyyy')
                                        ,cancel_date = to_date('" + approve_date.ToString("MM/dd/yyyy", WebUtil.EN) + @"','MM/dd/yyyy')
                                        ,approve_status = " + approve_status + @"
                                        ,req_status = '" + req_status + @"'
                                        ,approve_amt = " + assist_amt + @"
                                        ,withdrawable_amt = " + withdrawable_amt + @"
                                        ,pay_status = '8'  
                                        where assist_docno = '" + assist_docno + @"' 
                                        and capital_year = '" + capital_year + @"'";
                        ta.Exe(sqlStr);
                    }
                    catch (Exception ex) { ex.ToString(); }
                    try
                    {
                        sqlStr = @" update asnreqcapitaldet
                                        set clearloan_flag = " + clearloan_flag + @"
                                    where assist_docno = '" + assist_docno + @"'
                                    and capital_year = '" + capital_year + @"'
                                  ";
                        ta.Exe(sqlStr);
                    }
                    catch (Exception ex) { ex.ToString(); }
                    try
                    {
                        // update asnslippayout
                        sqlStr = @" update asnslippayout 
                                        set slip_status = '"+ approve_status +@"'
                                        where payoutslip_no = '" + assist_docno + @"' 
                                        and capital_year = '" + capital_year + "'";
                        ta.Exe(sqlStr);
                    }
                    catch (Exception ex) { ex.ToString(); }
                }
                #endregion

                LtServerMessage.Text = WebUtil.CompleteMessage("ทำรายการเรียบร้อยแล้ว");
                SearchList();
            }
        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
            DwType.SaveDataCache();
            tDwMain.Eng2ThaiAllRow();
            this.DisConnectSQLCA();
        }

        private void SearchList()
        {
            decimal ai_year = 0;
            String as_type = "";
            string as_paytype = "";
            string member_no;

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

            if (ai_year > 0) { ls_sql += " AND ASNREQMASTER.capital_year = " + ai_year; }
            if (as_type.Length > 0) { ls_sql += " AND ASNREQMASTER.assisttype_code = '" + as_type.Trim() + @"'"; }
            if (as_paytype.Length > 0) { ls_sql += " AND ASNREQMASTER.moneytype_code like '" + as_paytype.Trim() + @"'"; }
            if (member_no.Length > 0) { ls_sql += " AND ASNREQMASTER.member_no = '" + member_no.Trim() + "'"; }

            DwMain.SetSqlSelect(ls_sql);
            //LEK เช็คว่ากรณีเลือก ประเภททุน : 10 = สวัสดิการทุนการศึกษาบุตร ให้เรียงตาม ปี, เลขที่ใบคำขอ และวันที่ขอ
            if (as_type == "10")
            {
                DwMain.SetSort("capital_year asc, assist_docno asc, req_date asc");
                DwMain.Sort();
            }
            DwMain.Retrieve();
            int rowCount = DwMain.RowCount;
            for (int i = 1; i <= rowCount; i++)
            {
                DwMain.SetItemDateTime(i, "approve_date", state.SsWorkDate);
            }
            lblRowCount.Text = " จำนวนทั้งสิ้น  " + rowCount.ToString("#,##0") + "  ราย";
            return;

        }

        private bool CheckDeptClose(string deptaccountno)
        {
            bool status = false;
            Sdt dtCheck = null;
            sqlStr = "select * from dpdeptmaster where deptclose_status = '1' and deptaccount_no = '" + deptaccountno + "'";
            dtCheck = WebUtil.QuerySdt(sqlStr);
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
    }
}