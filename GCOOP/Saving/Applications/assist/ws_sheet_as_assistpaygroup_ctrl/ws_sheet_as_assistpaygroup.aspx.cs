using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using DataLibrary;


namespace Saving.Applications.assist.ws_sheet_as_assistpaygroup_ctrl
{
    public partial class ws_sheet_as_assistpaygroup : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public string PostSearch { get; set; }
        [JsPostBack]
        public string PostBank { get; set; }
        [JsPostBack]
        public string PostBranch { get; set; }
        [JsPostBack]
        public string PostDefaulass { get; set; }
        [JsPostBack]
        public string PostSave { get; set; }
       
        public void InitJsPostBack()
        {
            dsMain.InitDsMain(this);
            dsList.InitDsList(this);
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                dsMain.DDAssisttype();
                dsMain.DATA[0].select_check = "0";
                dsMain.DdMoneyType();
                dsMain.DdFromAccId();
                InitTofromaccid();
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == PostSearch)
            {
                SearchData();                               
            }
            else if (eventArg == PostBank) {
                dsMain.DdBankDesc();
                InitTofromaccid();
            }
            else if (eventArg == PostBranch)
            {
                String bank = dsMain.DATA[0].expense_bank;
                dsMain.DdBranch(bank);
            }
            else if (eventArg == PostDefaulass)
            {
                InitTofromaccid();
            }
            else if (eventArg == PostSave)
            {
                SaveData();
            }           
        }
        private void SearchData() 
        {
            dsList.ResetRow();
            dsMain.DATA[0].select_check = "0";
            string sqlwhere = "";
            if (dsMain.DATA[0].moneytype_code != "")
            {
                String ls_money = dsMain.DATA[0].moneytype_code.Trim();
                sqlwhere += " and assreqmaster.moneytype_code like '%" + ls_money + "%' ";
            }
            else if (dsMain.DATA[0].member_no != "")
            {
                String ls_memno = WebUtil.MemberNoFormat(dsMain.DATA[0].member_no);
                sqlwhere += " and assreqmaster.member_no like '%" + ls_memno + "%' ";
            }
            else { sqlwhere += ""; }

            if (dsMain.DATA[0].assisttype_code != "")
            {
                sqlwhere += " and assreqmaster.assisttype_code = '" + dsMain.DATA[0].assisttype_code + "' ";
            }
            else { sqlwhere += ""; }
            if (dsMain.DATA[0].expense_bank != "")
            {
                sqlwhere += " and assreqmaster.expense_bank = '" + dsMain.DATA[0].expense_bank + "' ";
            }
            else { sqlwhere += ""; }
            if (dsMain.DATA[0].expense_branch != "")
            {
                sqlwhere += " and assreqmaster.expense_branch = '" + dsMain.DATA[0].expense_branch + "' ";
            }
            else { sqlwhere += ""; }
            dsList.RetrieveList(sqlwhere);
            if (dsMain.DATA[0].moneytype_code != "CSH")
            {
                InitData();
            }         
        }
        private void InitTofromaccid()
        {
            String ls_defaultassid = "";
            String ls_money = dsMain.DATA[0].moneytype_code;
            string sql = @" 
                       SELECT MONEYTYPE_CODE,  
                              MONEYTYPE_GROUP, 
                              MONEYTYPE_DESC,   
                              SORT_ORDER  ,
                              MONEYTYPE_CODE || ' - '|| MONEYTYPE_DESC as MONEYTYPE_DISPLAY,
                              DEFAULTPAY_ACCID
                         FROM CMUCFMONEYTYPE WHERE  MONEYTYPE_GROUP IN('BNK','CHQ','CSH')  AND MONEYTYPE_CODE={0}  order by sort_order
                ";
            sql = WebUtil.SQLFormat(sql, ls_money);
            Sdt dt = WebUtil.QuerySdt(sql);
            if (dt.Next())
            {
                ls_defaultassid = dt.GetString("DEFAULTPAY_ACCID").Trim();
                dsMain.DATA[0].tofrom_accid = ls_defaultassid;
            }
            if (ls_money == "CSH")
            {
                this.SetOnLoadedScript("dsMain.SetItem(0,'expense_bank', '');dsMain.GetElement(0,'expense_bank').readOnly = true;dsMain.GetElement(0,'expense_bank').style.background = '#CCCCCC';dsMain.SetItem(0,'expense_branch', '');dsMain.GetElement(0,'expense_branch').readOnly = true;dsMain.GetElement(0,'expense_branch').style.background = '#CCCCCC'");
            }
            else {
                String bank = dsMain.DATA[0].expense_bank;
                dsMain.DdBranch(bank);
            }
        }
        private void InitData() {
            for (int i = 0; i < dsList.RowCount;i++ )
            {
                string expense_accid = "", mexpense_accid="";
                String ls_reqdocno = dsList.DATA[i].assist_docno;
                string sql = @" 
                select r.expense_accid, m.expense_accid  AS mexpense_accid from assreqmaster r
                left join mbmembmaster m on
                m.member_no = r.member_no
                where  r.req_status = 1  and trim(r.ref_slipno) is null
                and r.assist_docno ={0}
                    ";
                sql = WebUtil.SQLFormat(sql, ls_reqdocno);
                Sdt dt = WebUtil.QuerySdt(sql);
                if (dt.Next())
                {
                    expense_accid = dt.GetString("expense_accid").Trim();
                    mexpense_accid = dt.GetString("mexpense_accid").Trim();
                    if (expense_accid == "" )
                    {
                        dsList.DATA[i].expense_accid = mexpense_accid;
                    }
                    else { dsList.DATA[i].expense_accid = expense_accid; 
                    }
                }                
            }
            
        }
        private void SaveData()
        {
            try
            {
                String ls_tofromaccid = dsMain.DATA[0].tofrom_accid;
                        
                for (int i = 0; i < dsList.RowCount; i++)
                {
                    if (dsList.DATA[i].choose_flag == "1")
                    {
                        String flag = dsList.DATA[i].stm_flag;
                        String ls_memno = dsList.DATA[i].member_no;
                        String ls_assisttypecode = dsList.DATA[i].assisttype_code;
                        String ls_assisttypepay = dsList.DATA[i].assistpay_code;
                        String ls_reqdocno = dsList.DATA[i].assist_docno;
                        String ls_money = dsList.DATA[i].moneytype_code;
                        String ls_bank = dsList.DATA[i].expense_bank;
                        String ls_branch = dsList.DATA[i].expense_branch;
                        String ls_accid = dsList.DATA[i].expense_accid;
                        decimal ld_approveamt = dsList.DATA[i].approve_amt;
                        DateTime slip_date = dsList.DATA[i].req_date;
                        int li_year = state.SsWorkDate.Year;
                        String sql = @"select 
	                            document_year
	                        from cmdocumentcontrol where document_code = 'ASSISTDOCNO'";
                        sql = WebUtil.SQLFormat(sql);
                        Sdt dt = WebUtil.QuerySdt(sql);
                        if (dt.Next())
                        {
                            li_year = dt.GetInt32("document_year");
                        }
                        string ls_contractno   = wcf.NCommon.of_getnewdocno(state.SsWsPass, state.SsCoopId, "ASSISTCONTNO");
                        string ls_payoutslipno = wcf.NCommon.of_getnewdocno(state.SsWsPass, state.SsCoopId, "ASSISTSLIPNO");

                        try
                        {

                            string sqlStr_payout = @" INSERT INTO ASSSLIPPAYOUT  
                                                     ( COOP_ID,                COOP_CONTROL,           ASSISTSLIP_NO,   
                                                       CAPITAL_YEAR,           PAY_PERIOD,             ASSISTTYPE_CODE,   
                                                       SLIP_DATE,              OPERATE_DATE,           MEMBER_NO,   
                                                       SLIP_STATUS,            PAYOUT_AMT,             MONEYTYPE_CODE,   
                                                       BANK_CODE,              BANKBRANCH_ID,          BANK_ACCID,  
                                                       TOFROM_ACCID,           ENTRY_ID,               ENTRY_DATE,        
                                                       POST_TOFIN,             REF_REQDOCNO )  
                                              VALUES ( {0},                     {0},                    {1},
                                                       {2},                     {3},                    {4},   
                                                       {5},                     {6},                    {7},
                                                        1,                      {8},                    {9},
                                                       {10},                    {11},                   {12},
                                                       {13},                    {14},                   {15},
                                                        0,                      {16})  
                                                    ";
                            sqlStr_payout = WebUtil.SQLFormat(sqlStr_payout
                            , state.SsCoopId, ls_payoutslipno
                            , li_year, 1, ls_assisttypecode
                            , slip_date, state.SsWorkDate, ls_memno
                            , ld_approveamt, ls_money
                            , ls_bank, ls_branch, ls_accid
                            , ls_tofromaccid, state.SsUsername, state.SsWorkDate
                            , ls_reqdocno);
                            WebUtil.ExeSQL(sqlStr_payout);
                        }
                        catch
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage("Error ASSSLIPPAYOUT"); return;
                        }
                        if (flag == "1")
                        {
                            try
                            {
                                string sqlStr_statement = @"  INSERT INTO ASSCONTSTATEMENT  
                                                     ( COOP_ID,             COOP_CONTROL,        ASSCONTRACT_NO,   
                                                       SEQ_NO,              SLIP_DATE,           OPERATE_DATE,   
                                                       REF_SLIPNO,          PERIOD,              PAY_BALANCE,   
                                                       MONEYTYPE_CODE,      ITEM_STATUS,         ENTRY_ID,   
                                                       ENTRY_DATE
                                                     )   
                                              VALUES ( {0},                  {0},               {1},   
                                                         1,                  {2},               {3},
                                                       {4},                  {5},               {6},   
                                                       {7},                  1,                 {8},   
                                                       {9}
                                                      )
                                                       ";
                                sqlStr_statement = WebUtil.SQLFormat(sqlStr_statement
                                , state.SsCoopId, ls_contractno
                                , slip_date, state.SsWorkDate
                                , ls_payoutslipno, 1, ld_approveamt
                                , ls_money, state.SsUsername, state.SsWorkDate
                                );
                                WebUtil.ExeSQL(sqlStr_statement);
                            }
                            catch
                            {
                                LtServerMessage.Text = WebUtil.ErrorMessage("Error ASSCONTSTATEMENT"); return;
                            }
                            try
                            {
                                //ขาด          MAX_PERIODPAY   LAST_PERIODPAY  
                                string sqlStr_cont = @"INSERT INTO ASSCONTMASTER   
                                                    ( COOP_ID,         COOP_CONTROL,         ASSCONTRACT_NO,   
                                                    ASSISTTYPE_CODE,   MEMBER_NO,            ASSISTPAYTYPE_CODE,   
                                                    ASSISTREQ_DOCNO,   APPROVE_DATE,         APPROVE_AMT,  
                                                    WITHDRAWABLE_AMT,  PAY_BALANCE,          PAYMENT_STATUS,
                                                    LAST_STM ,         APPROVE_ID
                                                    )  
                                                    VALUES ( {0},      {0},          {1},   
                                                             {2},      {3},          {4},   
                                                             {5},      {6},          {7},
                                                             {7},      {7},            1,
                                                               1,      {8}     
                                                    )  
                                                    ";
                                sqlStr_cont = WebUtil.SQLFormat(sqlStr_cont
                                , state.SsCoopId, ls_contractno
                                , ls_assisttypecode, ls_memno, ls_assisttypepay
                                , ls_payoutslipno, state.SsWorkDate, ld_approveamt
                                , state.SsUsername
                                );
                                WebUtil.ExeSQL(sqlStr_cont);
                            }
                            catch
                            {
                                LtServerMessage.Text = WebUtil.ErrorMessage("Error ASSCONTMASTER"); return;
                            }
                        }
                        try
                        {

                            string sqlStr_update = @" UPDATE ASSREQMASTER SET REF_SLIPNO = {2} ,pay_date={3}
                                                    where coop_id={0} and ASSIST_DOCNO = {1} 
                                                    ";
                            sqlStr_update = WebUtil.SQLFormat(sqlStr_update
                            , state.SsCoopId, ls_reqdocno, ls_payoutslipno, state.SsWorkDate);
                            WebUtil.ExeSQL(sqlStr_update);
                        }
                        catch { LtServerMessage.Text = WebUtil.ErrorMessage("Error  UPDATE ASSREQMASTER "); return; }
                    }
                }
                SearchData();
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
            }
            catch { LtServerMessage.Text = WebUtil.ErrorMessage("Error บันทึกการจ่ายไม่สำเร็จ "); return; }            
        }
        public void SaveWebSheet()
        {
            throw new NotImplementedException();
        }
        public void WebSheetLoadEnd()
        {
            throw new NotImplementedException();
        }
    }
}