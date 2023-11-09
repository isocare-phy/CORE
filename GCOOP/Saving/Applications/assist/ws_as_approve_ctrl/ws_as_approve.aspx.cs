using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using CoreSavingLibrary.WcfNAccount;
using DataLibrary;
using System.Data;

namespace Saving.Applications.assist.ws_as_approve_ctrl
{
    public partial class ws_as_approve : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public string PostSearch { get; set; }
        [JsPostBack]
        public string PostChangeStatus { get; set; }
        [JsPostBack]
        public string PostSort { get; set; }
        [JsPostBack]
        public string InitAsssistpaytype { get; set; }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == PostSearch)
            {
                this.of_getdata();
            }
            else if (eventArg == PostChangeStatus)
            {
                for (int i = 0; i < dsList.RowCount; i++)
                {
                    if (dsList.DATA[i].choose_flag == "1")
                    {
                        dsList.DATA[i].REQ_STATUS = Convert.ToDecimal(dsMain.DATA[0].req_status);
                    }
                }
            }
            else if (eventArg == PostSort)
            {
                this.of_getdata();
            }
            else if (eventArg == InitAsssistpaytype)
            {
                string  ls_minpaytype = "",  ls_maxpaytype = "";
                if (dsMain.DATA[0].assisttype_code == "" )
                {
                    dsMain.ResetRow();
                    dsMain.DdAssistType();
                    dsMain.DATA[0].conclusion_no = "";
                    dsMain.DATA[0].conclusion_date = state.SsWorkDate;
                }
                else
                {
                    dsMain.AssistPayType(dsMain.DATA[0].assisttype_code, ref ls_minpaytype, ref ls_maxpaytype);
                    dsMain.DATA[0].assistpay_code1 = ls_minpaytype;
                    dsMain.DATA[0].assistpay_code2 = ls_maxpaytype;
                }
            }
        }

        public void InitJsPostBack()
        {
            dsMain.InitDsMain(this);
            dsList.InitDsList(this);
        }

        public void SaveWebSheet()
        {

            string ls_assisreq = "", ls_assiscont = "", ls_remark = "";
            Int32 li_apvstt;
            Int32 li_result = 0;

            // ตรวจสอบก่อนว่ามีปรับสถานะเป็นอนุมัติหรือยัง
            for (int i = 0; i < dsList.RowCount; i++)
            {
                if (dsList.DATA[i].choose_flag == "1")
                {
                    li_apvstt = Convert.ToInt32(dsList.DATA[i].REQ_STATUS);
                    if (li_apvstt == 8)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาระบุสถานะการอนุมัติให้ครบถ้วยด้วย");
                        return;
                    }
                }
            }

            for (int i = 0; i < dsList.RowCount; i++)
            {
                if (dsList.DATA[i].choose_flag == "1")
                {
                    ls_assisreq = dsList.DATA[i].ASSIST_DOCNO.Trim();
                    li_apvstt = Convert.ToInt32(dsList.DATA[i].REQ_STATUS);
                    ls_remark = dsMain.DATA[0].conclusion_no;

                    if (li_apvstt == 1)
                    {
                        ls_assiscont = wcf.NCommon.of_getnewdocno(state.SsWsPass, state.SsCoopId, "ASSISTCONTNO");
                    }

                    li_result = this.of_buildasscont(ls_assisreq, ls_assiscont, li_apvstt, ls_remark);

                    if (li_result != 1)
                    {
                        return;
                    }
                }
            }

            LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกเรียบร้อย");
            dsList.ResetRow();
        }

        private void of_getdata()
        {
            dsList.ResetRow();
            dsMain.DATA[0].select_check = "0";
            dsMain.DATA[0].req_status = "8";

            string sqlwhere = "", sqlorder = "";

            if (dsMain.DATA[0].member_no != "")
            {
                sqlwhere += " and req.member_no like '%" + dsMain.DATA[0].member_no + "%' ";
            }
            else { sqlwhere += ""; }

            if (dsMain.DATA[0].assisttype_code != "" )
            {
                sqlwhere += " and req.assisttype_code = '" + dsMain.DATA[0].assisttype_code + "' ";
            }
            else { sqlwhere += ""; }
            if (dsMain.DATA[0].assistpay_code1 != "")
            {
                sqlwhere += " and req.assistpay_code between '" + dsMain.DATA[0].assistpay_code1 + "'  and '" + dsMain.DATA[0].assistpay_code2 + "' ";
            }
            else { sqlwhere += ""; }

            if (dsMain.DATA[0].sort_type == "2")
            {
                sqlorder = "order by req.assisttype_code, req.member_no ";
            }
            else
            {
                sqlorder = "order by req.assisttype_code, req.assist_docno ";
            }
            dsList.RetrieveList(sqlwhere, sqlorder);

        }

        private Int32 of_buildasscont(string as_reqno, string as_contno, Int32 ai_status, string remark)
        {
            string SqlIns = "", SqlUpd = "";
            DateTime ldtm_apvdate;

            ldtm_apvdate = state.SsWorkDate;

            SqlIns = @"insert into  asscontmaster
		                        ( coop_id, asscontract_no, assisttype_code, member_no, assistpay_code, assistreq_docno, 
		                        assist_year, approve_date, approve_amt, withdrawable_amt, pay_balance, 
		                        last_periodpay, payment_status, last_stm, ass_rcvcardid, ass_prcardid, 
		                        moneytype_code, expense_bank, expense_branch, expense_accid, send_system, deptaccount_no, 
		                        ass_rcvname, approve_id, asscont_status, stm_flag, stmpay_num, stmpay_type , remark )
                        select	a.coop_id, {1}, a.assisttype_code, a.member_no, a.assistpay_code, a.assist_docno,
		                        a.assist_year, {2}, a.assistnet_amt, a.assistnet_amt, 0, 
                                0, 1, 0, a.ass_rcvcardid,  a.ass_prcardid, 
		                        a.moneytype_code, a.expense_bank, a.expense_branch, a.expense_accid, a.send_system, a.deptaccount_no, 
                                a.ass_rcvname, {3}, 1, a.stm_flag, a.stmpay_num, a.stmpay_type , {4}
                        from	assreqmaster a
		                        join mbmembmaster mb on a.member_no = mb.member_no
		                        join assucfassisttype b on a.assisttype_code = b.assisttype_code
		                        join assucfassisttypegroup c on b.assisttype_group = c.assisttype_group
                        where assist_docno = {0} ";

            SqlUpd = "update assreqmaster set req_status = {1}, approve_id = {2}, approve_date = {3} where assist_docno = {0} ";

            try
            {
                // ถ้าอนุมัติให้สร้างทะเบียนสวัสดิการขึ้นมา
                if (ai_status == 1)
                {
                    SqlIns = WebUtil.SQLFormat(SqlIns, as_reqno, as_contno, ldtm_apvdate, state.SsUsername, remark);
                    WebUtil.ExeSQL(SqlIns);
                }

                SqlUpd = WebUtil.SQLFormat(SqlUpd, as_reqno, ai_status, state.SsUsername, ldtm_apvdate);
                WebUtil.ExeSQL(SqlUpd);

            }
            catch (Exception ex)
            {
                WebUtil.ExeSQL("rollback");
                LtServerMessage.Text = WebUtil.ErrorMessage("Error Build ASSCONTMASTER "+ex);
                return -1;
            }

            return 1;
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();
            if (!IsPostBack)
            {
                dsMain.DdAssistType();
                dsMain.DATA[0].conclusion_no = "";
                dsMain.DATA[0].conclusion_date = state.SsWorkDate;
            }
        }

        public void WebSheetLoadEnd()
        {

        }
    }

}