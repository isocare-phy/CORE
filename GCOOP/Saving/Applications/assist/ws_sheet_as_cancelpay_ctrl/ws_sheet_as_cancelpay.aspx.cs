using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using DataLibrary;
namespace Saving.Applications.assist.ws_sheet_as_cancelpay_ctrl
{
    public partial class ws_sheet_as_cancelpay : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public string PostSearch { get; set; }
        public void InitJsPostBack()
        {
            dsMain.InitDsMain(this);
            dsList.InitDsList(this);
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                dsMain.DDloantype();
                dsMain.DATA[0].select_check = "0";
                dsMain.DATA[0].start_date = state.SsWorkDate;
                dsMain.DATA[0].end_date = state.SsWorkDate;
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == PostSearch)
            {
                dsList.ResetRow();
                dsMain.DATA[0].select_check = "0";
                DateTime start_date = dsMain.DATA[0].start_date;
                DateTime end_date = dsMain.DATA[0].end_date;
                string sqlwhere = "";
                //เพิ่มช่วงวันที่
                sqlwhere += " and assslippayout.slip_date between to_date('" + start_date.ToString("dd/MM/yyyy") + "', 'dd/mm/yyyy')  and to_date('" + end_date.ToString("dd/MM/yyyy") + "', 'dd/mm/yyyy')";
                if (dsMain.DATA[0].member_no != "")
                {
                    sqlwhere += " and assslippayout.member_no like '%" + dsMain.DATA[0].member_no + "%' ";
                }
                else { sqlwhere += ""; }

                if (dsMain.DATA[0].assisttype_code != "")
                {
                    sqlwhere += " and assslippayout.assisttype_code = '" + dsMain.DATA[0].assisttype_code + "' ";
                }
                else { sqlwhere += ""; }
                dsList.RetrieveList(sqlwhere);
            }
        }

        public void SaveWebSheet()
        {
            string ls_payoutslip;
            String flag = "", sqlStr = "", ls_reqdocno="";
            decimal ld_payamt = 0;
            try
            {
                for (int i = 0; i < dsList.RowCount; i++)
                {
                    if (dsList.DATA[i].choose_flag == "1")
                    {
                        ls_payoutslip = dsList.DATA[i].assistslip_no.Trim();
                        ls_reqdocno   = dsList.DATA[i].ref_reqdocno.Trim();
                        flag          = dsList.DATA[i].stm_flag;
                        try{
                            sqlStr = @"update assslippayout 
                                            set    
                                            slip_status = -9,cancel_id={2},cancel_date={3}
                                            where COOP_ID = {0} and assistslip_no = {1}";
                            sqlStr = WebUtil.SQLFormat(sqlStr, state.SsCoopControl, ls_payoutslip, state.SsUsername, state.SsWorkDate);
                            WebUtil.ExeSQL(sqlStr);

                            sqlStr = @" UPDATE ASSREQMASTER SET REF_SLIPNO = '' 
                                        where coop_id={0} and ASSIST_DOCNO = {1} 
                                        ";
                            sqlStr = WebUtil.SQLFormat(sqlStr, state.SsCoopId, ls_reqdocno);
                            WebUtil.ExeSQL(sqlStr);
                        }
                        catch
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถปรับปรุงข้อมูล assslippayout ได้");
                        }
                        if (flag == "1")
                        {
                            String ls_lastseqno = "";
                            string sql = @" 
                                        select last_stm from asscontmaster inner join asscontstatement on asscontmaster.asscontract_no=asscontmaster.asscontract_no
                                        where asscontstatement.ref_slipno={1} and asscontmaster.payment_status=1 and asscontmaster.coop_id={0}
                                        ";
                            sql = WebUtil.SQLFormat(sql, state.SsCoopId, ls_payoutslip);
                            Sdt dt = WebUtil.QuerySdt(sql);
                            if (dt.Next())
                            {
                                ls_lastseqno =dt.GetString("last_stm").Trim();                                                            
                            }
                            if (ls_lastseqno == "1")
                            {
                                sqlStr = @"update asscontmaster 
                                            set    
                                            payment_status = -9,cancel_id={2},cancel_date={3}
                                            where COOP_ID = {0} and assistreq_docno = {1}";
                                sqlStr = WebUtil.SQLFormat(sqlStr, state.SsCoopControl, ls_payoutslip, state.SsUsername, state.SsWorkDate);
                                WebUtil.ExeSQL(sqlStr);
                            }
                            else
                            {
                                ld_payamt = dsList.DATA[i].payout_amt;
                                sqlStr = @"update asscontmaster set last_periodpay=last_periodpay-1 ,pay_balance=pay_balance - " + ld_payamt + ",withdrawable_amt =approve_amt + " + ld_payamt + "-pay_balance   where coop_id={0} and  asscontract_no =  "+
                                        " (select asscontract_no from assslippayout inner join asscontstatement on assslippayout.assistslip_no=asscontstatement.ref_slipno "+
                                        " and assslippayout.assistslip_no={1}  )";
                                sqlStr = WebUtil.SQLFormat(sqlStr, state.SsCoopControl, ls_payoutslip);
                                WebUtil.ExeSQL(sqlStr);
                                //chk เพิ่ม
                            }
                            sqlStr = @"update asscontstatement 
                                    set item_status = -9,cancel_id={3},cancel_date={4}
                                    where COOP_ID = {0} and seq_no = {1} and ref_slipno={2} ";
                            sqlStr = WebUtil.SQLFormat(sqlStr, state.SsCoopControl, ls_lastseqno, ls_payoutslip, state.SsUsername, state.SsWorkDate);
                            WebUtil.ExeSQL(sqlStr);    
                        }
                    }
                }

                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกเรียบร้อย");
                dsList.ResetRow();
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("บันทึกไม่สำเร็จ : " + ex);
            }
        }

        public void WebSheetLoadEnd()
        {
            throw new NotImplementedException();
        }
    }
}