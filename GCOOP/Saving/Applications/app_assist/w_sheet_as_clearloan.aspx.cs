using System;
using CoreSavingLibrary;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using DataLibrary;
using Sybase.DataWindow;
using CoreSavingLibrary.WcfNShrlon;

namespace Saving.Applications.app_assist
{
    public partial class w_sheet_as_clearloan_benefit : PageWebSheet, WebSheet
    {

        Sta ta;
        [JsPostBack]
        public string JsPostAssDocno { get; set; }
        [JsPostBack]
        public string JsPostTranLoan { get; set; }


        private DwThDate tDw_Detail;

        public void InitJsPostBack()
        {
            tDw_Detail = new DwThDate(Dw_Detail, this);
            tDw_Detail.Add("pay_date", "pay_tdate");

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
                //DwMain.InsertRow(0);
                Dw_Detail.InsertRow(0);
                DwUtil.RetrieveDDDW(Dw_Detail, "expense_type", "as_payout.pbl", null);
                DwUtil.RetrieveDDDW(Dw_Detail, "expense_bank", "as_payout.pbl", null);
                DwUtil.RetrieveDDDW(Dw_Detail, "expense_branch", "as_payout.pbl", null);


                Dw_Detail.SetItemDate(1, "pay_date", state.SsWorkDate);
                tDw_Detail.Eng2ThaiAllRow();

                DwMain.Retrieve();
                FindLoan();
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(Dw_Detail);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == JsPostAssDocno)
            {
                try
                {
                    int row = Convert.ToInt16(HdSelectedRow.Value);
                    string ass_docno = DwMain.GetItemString(row, "assist_docno");

                    DwUtil.RetrieveDataWindow(Dw_Detail, "as_payout.pbl", null, ass_docno);
                    Dw_Detail.SetItemDate(1, "pay_date", state.SsWorkDate);
                    tDw_Detail.Eng2ThaiAllRow();
                }
                catch { }
            }
            else if (eventArg == JsPostTranLoan)
            {
                tDw_Detail.Thai2EngAllRow();
                TranLoan();
            }
        }

        public void SaveWebSheet()
        {
        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
            Dw_Detail.SaveDataCache();
        }

        public void FindLoan()
        {
            try
            {
                for (int row = 1; row <= DwMain.RowCount; row++)
                {
                    string member_no = "";
                    decimal temp_prnc = 0, temp_int = 0, sum_prnc = 0, sum_int = 0;
                    string temp_contno = "";
                    member_no = DwMain.GetItemString(row, "member_no");
                    string sql = "select * from lncontmaster where member_no={0} and coop_id={1} and principal_balance>0 and contract_status=1";
                    sql = WebUtil.SQLFormat(sql, member_no, state.SsCoopId);
                    Sdt dt = WebUtil.QuerySdt(sql);
                    while (dt.Next())
                    {
                        temp_contno = dt.GetString("loancontract_no");
                        temp_prnc = dt.GetDecimal("principal_balance");
                        temp_int = wcf.Shrlon.of_computeinterest(state.SsWsPass, state.SsCoopId, temp_contno, state.SsWorkDate);
                        sum_prnc += temp_prnc;
                        sum_int += temp_int;
                    }

                    DwMain.SetItemDecimal(row, "clearloan_amt", sum_prnc);
                    DwMain.SetItemDecimal(row, "clearloan_int", sum_int);
                }
            }
            catch /*(Exception ex)*/ { }
        }

        public void TranLoan()
        {
            try
            {
                string sqlStr = "", expense_type = "", expense_bank = "", expense_branch = "", expense_accid = "";
                decimal withdrawable_amt = 0;
                string assist_docno = "", member_no = "", remark = "";
                DateTime pay_date;

                try { expense_type = Dw_Detail.GetItemString(1, "expense_type"); }
                catch { expense_type = "TAL"; } //TAL = โอนชำระหนี้
                try { expense_bank = Dw_Detail.GetItemString(1, "expense_bank"); }
                catch { expense_bank = "34"; }
                try { expense_branch = Dw_Detail.GetItemString(1, "expense_branch"); }
                catch { expense_branch = ""; }
                try { expense_accid = Dw_Detail.GetItemString(1, "expense_accid"); }
                catch { expense_accid = ""; }
                try { withdrawable_amt = Dw_Detail.GetItemDecimal(1, "withdrawable_amt"); }
                catch { withdrawable_amt = 0; }
                try { assist_docno = Dw_Detail.GetItemString(1, "assist_docno"); }
                catch { assist_docno = ""; }
                try { member_no = Dw_Detail.GetItemString(1, "member_no"); }
                catch { member_no = ""; }
                try { pay_date = Dw_Detail.GetItemDateTime(1, "pay_date"); }
                catch { pay_date = state.SsWorkDate; }
                try { remark = Dw_Detail.GetItemString(1, "remark"); }
                catch { remark = ""; }

                #region อัพเดทข้อมูลตาราง asnslippayout, asnreqmaster, asnmemsalary, asnslippayout
                sqlStr = @" update  asnreqmaster 
                            set     pay_status = 1,
                                    pay_date = to_date('" + pay_date.ToString("dd/MM/yyyy") + @"','dd/MM/yyyy'),
                                    moneytype_code='" + expense_type + @"',
                                    withdrawable_amt = " + withdrawable_amt + @",
                                    expense_branch = '" + expense_branch + @"',
                                    expense_bank = '" + expense_bank + @"',
                                    remark ='" + remark + @"'
                            where   assist_docno = '" + assist_docno + @"' 
                            and     member_no = '" + member_no + @"'";
                ta.Exe(sqlStr);
                //update วันที่
                sqlStr = @" update  asnmemsalary 
                            set     entry_date = to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy") + @"','dd/MM/yyyy') 
                            where   assist_docno = '" + assist_docno + @"' 
                            and     member_no = '" + member_no + @"'";
                ta.Exe(sqlStr);
                sqlStr = @" update  asnslippayout 
                            set     slip_status = '1',
                                    moneytype_code ='" + expense_type + @"',
                                    operate_date = to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy") + @"','dd/MM/yyyy'),
                                    entry_date = to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy") + @"','dd/MM/yyyy') 
                            where   payoutslip_no = '" + assist_docno + @"' 
                            and     member_no = '" + member_no + @"'";
                ta.Exe(sqlStr);
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                #endregion
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("บันทึกไม่สำเร็จ" + ex.Message);
            }
        }
    }


}
