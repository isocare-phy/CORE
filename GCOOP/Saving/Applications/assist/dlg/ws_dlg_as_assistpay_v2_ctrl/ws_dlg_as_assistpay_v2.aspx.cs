using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary.WcfNShrlon;
using CoreSavingLibrary.WcfNCommon;
using DataLibrary;
using System.Globalization;
using System.Drawing;

namespace Saving.Applications.assist.dlg.ws_dlg_as_assistpay_v2_ctrl
{
    public partial class ws_dlg_as_assistpay_v2 : PageWebDialog, WebDialog
    {
        [JsPostBack]
        public string PostSave { get; set; }
        [JsPostBack]
        public string PostGetFee { get; set; }
        [JsPostBack]
        public string PostNewRowPayto { get; set; }
        [JsPostBack]
        public string PostNewRowLoan { get; set; }
        [JsPostBack]
        public string PostDelRowLoan { get; set; }
        [JsPostBack]
        public string PostDelRowPayto { get; set; }
        [JsPostBack]
        public string PostSetDataSave { get; set; }
        [JsPostBack]
        public string PostCheckDate { get; set; }
      
        
        
        String[] assists;
        String[] ls_arr_bank;
        String[] ls_arr_bankbranch;
        String[] la_arr_tofrom;
        String[] ls_arr_bankaccid;
        
        String ls_keep_bank;
        String ls_keep_bankbranch;
        String la_keep_tofrom;
        String ls_keep_bankaccid;

        int currentAssist = 0,ii_saveresult = 0;

        public void InitJsPostBack()
        {
            dsMain.InitDsMain(this);
            dsPayto.InitDsPayto(this);
            dsLoan.InitDsLoan(this);
            dsBank.InitDsBank(this);
        }

        public void WebDialogLoadBegin()
        {
            if (!IsPostBack)
            {
                ii_saveresult = 0;
                assists = Request["assists"].Split(',');
                lbCurrentAssist.Text = (currentAssist + 1) + "/" + assists.Length;
                
                //dsLoan.InsertLastRow();
                //dsLoan.DATA[0].payback_type = "LON"; //ต้องใส่ค่าให้มันก่อนไม่งั้นถ้าคนเพิ่มแถวของอีกตารางมันจะหาย
                //dsLoan.DATA[0].loan_detail = "หักชำระหนี้"; //เฉพาะรอบแรก
                of_initassistpay();
                for (int ii = 0; ii < dsPayto.RowCount; ii++)
                {
                    SetData_ToSave(ii);
                }
            }
            else
            {
                
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == PostSave)
            {
                SaveData();
                NextAssist();
            }
            else if (eventArg == PostNewRowPayto)
            {
                dsPayto.InsertLastRow();
                dsPayto.DdMoneyType();
                dsPayto.DATA[dsPayto.RowCount - 1].moneytype_code = "CSH"; //ต้องใส่ค่าให้มันก่อนไม่งั้นถ้าคนเพิ่มแถวของอีกตารางมันจะหาย
                dsPayto.DATA[dsPayto.RowCount - 1].payto_detail = "||||";
                SetData_ToSave(dsPayto.RowCount - 1);

            }
            else if (eventArg == PostNewRowLoan)
            {
                dsLoan.InsertLastRow();
                dsLoan.DATA[dsLoan.RowCount - 1].payback_type = "LON"; //ต้องใส่ค่าให้มันก่อนไม่งั้นถ้าคนเพิ่มแถวของอีกตารางมันจะหาย
                dsLoan.DATA[dsLoan.RowCount - 1].loan_detail = "หักชำระหนี้";
            }
            else if (eventArg == PostDelRowPayto)
            {
                int row = dsPayto.GetRowFocus();
                dsPayto.DeleteRow(row);
                dsPayto.DdMoneyType();
            }
            else if (eventArg == PostDelRowLoan)
            {
                int row = dsLoan.GetRowFocus();
                dsLoan.DeleteRow(row);
            }
            else if (eventArg == PostSetDataSave)
            {
                SetData_ToSave(0);
            }
            else if (eventArg == PostCheckDate) {
                if (state.SsWorkDate > dsMain.DATA[0].REQ_DATE)
                {
                    this.SetOnLoadedScript("alert('ไม่สามารถใส่วันจ่ายย้อนหลังได้!!')");
                    dsMain.DATA[0].REQ_DATE = state.SsWorkDate;
                }
                else {
                    this.SetOnLoadedScript("alert('การเปลี่ยนวันที่จ่าย ไม่ควรทำรายการเงินสด!!')");
                }
            }
        }

        private void SetData_ToSave(int row)
        {
            if (dsPayto.GetRowFocus() > 0) { row = dsPayto.GetRowFocus(); };
            string ls_show = "";
            
            string[] ls_arr_savetxt = dsPayto.DATA[row].payto_detail.Split('|');
            Hdbank.Value = Hdbank.Value + row + "r" + ls_arr_savetxt[0] + "|"; // รหัสธนาคาร
            Hdbankbranch.Value = Hdbankbranch.Value + row + "r" + ls_arr_savetxt[1] + "|"; // รหัสสาขา
            Hdbankaccid.Value = Hdbankaccid.Value + row + "r" + ls_arr_savetxt[2] + "|"; // เลขที่ธนาคาร
            string ls_payname = ls_arr_savetxt[3]; //จ่ายใคร
            Hdtofrom.Value = Hdtofrom.Value + row + "r" + ls_arr_savetxt[4] + "|"; // รหัสบัญชี

            if (ls_arr_savetxt[0].Trim() != "" && ls_arr_savetxt[0].Trim() != null) 
            {
                string sql = @"select trim(bank_desc)bank_desc from cmucfbank where bank_code = {0}";
                sql = WebUtil.SQLFormat(sql, ls_arr_savetxt[0]);
                Sdt dt = WebUtil.QuerySdt(sql);
                if (dt.Next())
                {
                    ls_show = ls_show + dt.GetString("bank_desc") + " ";
                }
            }
            if (ls_arr_savetxt[1].Trim() != "" && ls_arr_savetxt[1] != null)
            {
                string sql = @"select trim(branch_name)branch_name, serviceassist_amt from cmucfbankbranch where bank_code = {0} and branch_id = {1}";
                sql = WebUtil.SQLFormat(sql, ls_arr_savetxt[0], ls_arr_savetxt[1]);
                Sdt dt = WebUtil.QuerySdt(sql);
                if (dt.Next())
                {
                    ls_show = ls_show + dt.GetString("branch_name") + " ";
                    if (dt.GetInt32("serviceassist_amt") > 0)
                    {
                        //dsLoan.InsertLastRow();
                        //dsPayto.DATA[dsLoan.RowCount - 1].moneytype_code = "FEE";
                        //dsPayto.DATA[dsLoan.RowCount - 1].payto_detail = "ค่าทำเนียมธนาคาร";
                        //dsPayto.DATA[dsLoan.RowCount - 1].assist_amt = dt.GetInt32("serviceassist_amt");
                    }
                }
            }
            if (ls_arr_savetxt[2].Trim() != "" && ls_arr_savetxt[2] != null)
            {
                if (dsPayto.DATA[row].moneytype_code == "TRN")
                {
                    ls_show = ls_show + "บัญชี " + ls_arr_savetxt[2] + " ";
                }
                else
                {
                    ls_show = ls_show + "เลขที่ " + ls_arr_savetxt[2] + " ";
                }
            }
            ls_show = ls_show.Trim();
            if (ls_arr_savetxt[3].Trim() != "" && ls_arr_savetxt[3] != null)
            {
                if (dsPayto.DATA[row].moneytype_code == "CHQ")
                {
                    ls_show = ls_show + "จ่ายให้ " + ls_payname + " ";
                }
                else
                {
                    ls_show = ls_show + ls_arr_savetxt[3] + " "; //gain
                }
            }
            if (ls_arr_savetxt[4].Trim() != "" && ls_arr_savetxt[4] != null)
            {
                ls_show = ls_show + "รหัสบัญชี " + ls_arr_savetxt[4].Trim() + " ";
            }
            dsPayto.DATA[row].payto_detail = ls_show.Trim();
        }
        /// <summary>
        /// ดึงสัญญาถัดไป
        /// </summary> 
        private void NextAssist()
        {
            currentAssist = Convert.ToInt16(HdIndex.Value);
            assists = Request["assists"].Split(',');

            currentAssist += 1;
            if (currentAssist < assists.Length)
            {
                lbCurrentAssist.Text = (currentAssist + 1) + "/" + assists.Length;
                of_initassistpay();
            }
            HdIndex.Value = currentAssist + "";
            if ((currentAssist) >= assists.Length)
            {
                if (ii_saveresult == 1)
                {
                    this.SetOnLoadedScript("GetShowData();");
                }
                else
                {
                    this.SetOnLoadedScript("parent.GetShowData();");
                }

            }
        }
        /// <summary>
        /// Init ข้อมุลการจ่าย
        /// </summary>
        private void of_initassistpay()
        {
            assists = Request["assists"].Split(',');
            lbCurrentAssist.Text = (currentAssist + 1) + "/" + assists.Length;
            string assist = assists[currentAssist];
            dsMain.RetrieveDetail(assist);
            dsMain.DATA[0].REQ_DATE = state.SsWorkDate;
            dsPayto.RetrieveDetail(assist);
            dsPayto.DdMoneyType();
            dsBank.retrieve();
            string moneytype_code = dsMain.DATA[0].MONEYTYPE_CODE;
            HdIndex.Value = currentAssist + "";
          
        }


        private void SaveData()
        {
            //this.ConnectSQLCA();
            string sql;
            Sdt dt;
            string ls_flag = dsMain.DATA[0].stm_flag;
            int li_year = dsMain.DATA[0].assist_year;
            try
            {
                //ta.RollBack();                          
                string ls_memno = dsMain.DATA[0].MEMBER_NO;
                string ls_assisttypecode = dsMain.DATA[0].ASSISTTYPE_CODE;
                string ls_assisttypepay = dsMain.DATA[0].ASSISTPAY_CODE;
                string ls_reqdocno = dsMain.DATA[0].ASSIST_DOCNO;
                string ls_tofrom_accid = dsBank.DATA[0].accaccount;
                string ls_deptno = "", ls_bank = "", ls_branch = "", ls_bankaccid = "", ls_tofromaccid = "";
                DateTime slip_date = dsMain.DATA[0].REQ_DATE;
                decimal ld_approveamt = dsMain.DATA[0].APPROVE_AMT;
                decimal ld_assistamt = dsMain.DATA[0].ASSIST_AMT;
                decimal ld_lastperiod = 1;
                /////////////////////////
                for (int ii = 0; ii < dsPayto.RowCount; ii++)
                {
                    if (dsPayto.DATA[ii].assist_amt == 0)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('ตรวจสอบจำนวนเงินของรายการจ่าย');", true);
                        return;
                    }
                }
                /////////////////////////
                string ls_payoutslipno = wcf.NCommon.of_getnewdocno(state.SsWsPass, state.SsCoopId, "ASSISTSLIPNO");
                //string ls_payoutslipno = "test001";

                if (ls_flag == "1")
                {
                    decimal ld_lastseqno = 0, ld_paybalance = 0;
                    decimal witdraw = ld_approveamt - ld_assistamt;
                    string ls_asscontractno = "", ls_contractno = "";
                    int li_numpay = 1;

                    sql = @"select num_pay from assucfassisttypedet where coop_id = {0} and assisttype_code = {1} and assisttype_pay = {2}  and rownum = 1";
                    sql = WebUtil.SQLFormat(sql, state.SsCoopId, ls_assisttypecode, ls_assisttypepay);
                    dt = WebUtil.QuerySdt(sql);
                    if (dt.Next()) { li_numpay = dt.GetInt32("num_pay"); }

                    //////////////////// ASSCONTMASTER /////////////////////
                    sql = @"select asscontract_no, last_stm, last_periodpay, pay_balance  from asscontmaster where coop_id={0} and assisttype_code={1} and assistpaytype_code={2}  
                          and member_no={3}";
                    sql = WebUtil.SQLFormat(sql, state.SsCoopId, ls_assisttypecode, ls_assisttypepay, ls_memno);
                    dt = WebUtil.QuerySdt(sql);
                    if (dt.Next())
                    {
                        ls_contractno = dt.GetString("asscontract_no"); ;
                        ld_lastseqno = dt.GetInt32("last_stm") + 1;
                        ld_lastperiod = dt.GetInt32("last_periodpay") + 1;
                        ld_paybalance = dt.GetDecimal("pay_balance") + ld_assistamt;
                        witdraw = ld_approveamt - ld_paybalance;
                        try
                        {

                            string sqlStr_update = @" UPDATE ASSCONTMASTER SET WITHDRAWABLE_AMT = {2} ,PAY_BALANCE={3},LAST_STM={4},last_periodpay={5}
                                                    where coop_id={0} and ASSCONTRACT_NO = {1} 
                                                    ";
                            sqlStr_update = WebUtil.SQLFormat(sqlStr_update
                            , state.SsCoopId, ls_contractno
                            , witdraw, ld_paybalance, ld_lastseqno, ld_lastperiod);
                            WebUtil.ExeSQL(sqlStr_update);
                        }
                        catch { LtServerMessage.Text = WebUtil.ErrorMessage("Error  UPDATE ASSCONTMASTER "); return; }
                    }
                    else
                    {
                        ls_contractno = wcf.NCommon.of_getnewdocno(state.SsWsPass, state.SsCoopId, "ASSISTCONTNO");
                        ld_lastseqno = 1;
                        try
                        {
                            string sqlStr_cont = @"INSERT INTO ASSCONTMASTER   
                            ( COOP_ID,         COOP_CONTROL,         ASSCONTRACT_NO,   
                            ASSISTTYPE_CODE,   MEMBER_NO,            ASSISTPAYTYPE_CODE,   
                            ASSISTREQ_DOCNO,   APPROVE_DATE,         APPROVE_AMT,  
                            WITHDRAWABLE_AMT,  PAY_BALANCE,          PAYMENT_STATUS,
                            LAST_STM ,         last_periodpay,       APPROVE_ID,
                            max_periodpay
                            )  
                            VALUES ({0},      {0},          {1},   
                                    {2},      {3},          {4},   
                                    {5},      {6},          {7},
                                    {8},      {9},            1,
                                    {10},     {11},        {12},
                                    {13}
                            )  
                            ";
                            sqlStr_cont = WebUtil.SQLFormat(sqlStr_cont
                            , state.SsCoopId, ls_contractno
                            , ls_assisttypecode, ls_memno, ls_assisttypepay
                            , ls_payoutslipno, slip_date, ld_approveamt
                            , witdraw, ld_assistamt
                            , ld_lastseqno, ld_lastperiod, state.SsUsername
                            , li_numpay
                            );
                            WebUtil.ExeSQL(sqlStr_cont);
                        }
                        catch
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage("Error ASSCONTMASTER"); return;
                        }

                    }

                    //////////////////// ASSCONTSTATEMENT /////////////////////
                    try
                    {
                        string sqlStr_statement = @"  INSERT INTO ASSCONTSTATEMENT  
                                ( COOP_ID           , COOP_CONTROL      , ASSCONTRACT_NO   
                                , SEQ_NO            , SLIP_DATE         , OPERATE_DATE
                                , REF_SLIPNO        , PERIOD            , PAY_BALANCE   
                                , ITEM_STATUS       , ENTRY_ID
                                , ENTRY_DATE
                                )   
                        VALUES ( {0}                , {0}               , {1}
                                , {2}               , {3}               , {4}
                                , {5}               , {6}               , {7}   
                                , 1                 , {8}               , {9}
                                )
                                ";
                        sqlStr_statement = WebUtil.SQLFormat(sqlStr_statement
                        , state.SsCoopId, ls_contractno
                        , ld_lastseqno, slip_date, state.SsWorkDate
                        , ls_payoutslipno, ld_lastperiod, ld_assistamt
                        , state.SsUsername, state.SsWorkDate
                        );
                        WebUtil.ExeSQL(sqlStr_statement);
                    }
                    catch
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("Error ASSCONTSTATEMENT"); return;
                    }
                }// flag = 1

                //////////////////// ASSSLIPPAYOUT /////////////////////
                try
                {
                    string sqlStr_payout = @" INSERT INTO ASSSLIPPAYOUT  
                             ( COOP_ID,                COOP_CONTROL,           ASSISTSLIP_NO,   
                               CAPITAL_YEAR,           PAY_PERIOD,             ASSISTTYPE_CODE,   
                               SLIP_DATE,              OPERATE_DATE,           MEMBER_NO,   
                               SLIP_STATUS,            PAYOUT_AMT,             ENTRY_ID,   
                               ENTRY_DATE,             POST_TOFIN,             REF_REQDOCNO,
                               TOFROM_ACCID)  
                      VALUES ( {0},                     {0},                    {1},
                               {2},                     {3},                    {4},   
                               {5},                     {6},                    {7},
                                1,                      {8},                    {9},
                               {10},                       0,                   {11},
                               {12})  
                            ";
                    sqlStr_payout = WebUtil.SQLFormat(sqlStr_payout
                    , state.SsCoopId, ls_payoutslipno
                    , li_year, ld_lastperiod, ls_assisttypecode
                    , slip_date, state.SsWorkDate, ls_memno
                    , ld_assistamt, state.SsUsername
                    , state.SsWorkDate, ls_reqdocno
                    , ls_tofrom_accid);
                    WebUtil.ExeSQL(sqlStr_payout);
                }
                catch
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("Error ASSSLIPPAYOUT"); return;
                }
                //////////////////// ASSSLIPPAYOUTDET /////////////////////

                ls_arr_bank = Hdbank.Value.Split('|');
                ls_arr_bankbranch = Hdbankbranch.Value.Split('|');
                la_arr_tofrom = Hdtofrom.Value.Split('|');
                ls_arr_bankaccid = Hdbankaccid.Value.Split('|');

                int ld_seqno = 0;
                int li_arr = 0;
                for (int ii = 0; ii < dsPayto.RowCount; ii++) //จ่ายโดย
                {
                    if (Convert.ToInt32(ls_arr_bank[li_arr].Substring(0, 1)) == ii)//1 ตำแหน่งก่อน เด่วกลับมาแก้
                    {
                        try
                        {
                            ls_bank = ls_arr_bank[li_arr].Substring(ls_arr_bank[li_arr].IndexOf('r') + 1, ls_arr_bank[li_arr].Length - (ls_arr_bank[li_arr].IndexOf('r') + 1));
                        }
                        catch { ls_bank = ""; }
                    }
                    if (Convert.ToInt32(ls_arr_bankbranch[li_arr].Substring(0, 1)) == ii)
                    {
                        try
                        {
                            ls_branch = ls_arr_bankbranch[li_arr].Substring(ls_arr_bankbranch[li_arr].IndexOf('r') + 1, ls_arr_bankbranch[li_arr].Length - (ls_arr_bankbranch[li_arr].IndexOf('r') + 1));
                        }
                        catch { ls_branch = ""; }
                    }
                    if (Convert.ToInt32(la_arr_tofrom[li_arr].Substring(0, 1)) == ii)
                    {
                        try
                        {
                            ls_tofromaccid = la_arr_tofrom[li_arr].Substring(la_arr_tofrom[li_arr].IndexOf('r') + 1, la_arr_tofrom[li_arr].Length - (la_arr_tofrom[li_arr].IndexOf('r') + 1));
                        }
                        catch { ls_tofromaccid = ""; }
                    }
                    if (Convert.ToInt32(ls_arr_bankaccid[li_arr].Substring(0, 1)) == ii)
                    {
                        try
                        {
                            ls_bankaccid = ls_arr_bankaccid[li_arr].Substring(ls_arr_bankaccid[li_arr].IndexOf('r') + 1, ls_arr_bankaccid[li_arr].Length - (ls_arr_bankaccid[li_arr].IndexOf('r') + 1));
                            li_arr++;//เด่วกลับมาแก้
                        }
                        catch { ls_bankaccid = ""; }
                    }

                    if (dsPayto.DATA[ii].moneytype_code == "GAN")
                    {
                        ld_seqno = ld_seqno + 1;
                        SaveSlippayout(ls_payoutslipno, ld_seqno, "GAN", dsPayto.DATA[ii].payto_detail, dsPayto.DATA[ii].assist_amt, "", "", "", "", "", "");
                    }
                    else if (dsPayto.DATA[ii].moneytype_code == "TRN")
                    {
                        ld_seqno = ld_seqno + 1;
                        if (ls_bankaccid.Trim() != "")
                        {
                            ls_deptno = ls_bankaccid.Trim();
                            ls_bankaccid = "";
                        }
                        SaveSlippayout(ls_payoutslipno, ld_seqno, "TRN", dsPayto.DATA[ii].payto_detail, dsPayto.DATA[ii].assist_amt, dsPayto.DATA[ii].moneytype_code, ls_deptno, ls_bank.Trim(), ls_branch.Trim(), ls_bankaccid.Trim(), ls_tofromaccid.Trim());

                        //////////////////// DPDEPTTRAN /////////////////////
                        try
                        {
                            int ls_depttran_seqno = 1;
                            sql = @"select 
	                            max(SEQ_NO) SEQ_NO
	                        from DPDEPTTRAN where DEPTACCOUNT_NO = {1} and SYSTEM_CODE='ASS' and TRAN_YEAR={2} and TRAN_DATE={3} and COOP_ID={0}  ";
                            sql = WebUtil.SQLFormat(sql, state.SsCoopId, ls_deptno, li_year, slip_date);
                            dt = WebUtil.QuerySdt(sql);
                            if (dt.Next())
                            {
                                ls_depttran_seqno = dt.GetInt32("SEQ_NO");
                                ls_depttran_seqno = ls_depttran_seqno + 1;
                            }
                            string sqlStr_cont = @"
                                    INSERT INTO DPDEPTTRAN  
                                    ( COOP_ID,					DEPTACCOUNT_NO,				MEMCOOP_ID,				MEMBER_NO,   
                                    SYSTEM_CODE,				TRAN_YEAR,					TRAN_DATE,				SEQ_NO,   
                                    DEPTITEM_AMT,			TRAN_STATUS,				BRANCH_OPERATE,			SEQUEST_STATUS,	  
                                    REF_COOPID )  
                            VALUES ( {0},				        {1},	        			{0},                    {2},   
                                    'ASS',					{3},					    {4},                    {6},   
                                    {5},                     '0',						'001',					0,   
                                    {0}
		                            )";
                            sqlStr_cont = WebUtil.SQLFormat(sqlStr_cont
                            , state.SsCoopId, ls_deptno, ls_memno
                            , li_year, slip_date
                            , ld_assistamt, ls_depttran_seqno
                            );
                            WebUtil.ExeSQL(sqlStr_cont);
                        }
                        catch
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถส่งข้อมูลไประบบเงินฝากได้ " + ls_memno); return;
                        }
                    }
                    else
                    {
                        ld_seqno = ld_seqno + 1;
                        //ls_deptno, ls_bank, ls_branch, ls_bankaccid, ls_tofromaccid;
                        SaveSlippayout(ls_payoutslipno, ld_seqno, dsPayto.DATA[0].moneytype_code, dsPayto.DATA[ii].payto_detail, dsPayto.DATA[ii].assist_amt, dsPayto.DATA[ii].moneytype_code, ls_deptno, ls_bank.Trim(), ls_branch.Trim(), ls_bankaccid.Trim(), ls_tofromaccid.Trim());
                    }
                }
                for (int ii = 0; ii < dsLoan.RowCount; ii++)//รายการหักชำระ
                {
                    ld_seqno = ld_seqno + 1;
                    SaveSlippayout(ls_payoutslipno, ld_seqno, dsLoan.DATA[ii].payback_type, dsLoan.DATA[ii].loan_detail, dsLoan.DATA[ii].assist_amt, "", "", "", "", "", "");
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
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                ii_saveresult = 1;
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("Error ข้อมูลไม่ถูกต้อง"); return;
            }

        }
        public void SaveSlippayout(string payoutslipno, int seqno, string methpay, string paytowhom, decimal amt, string moneytype, string deptaccount, string bank, string bankbranch, string bank_accid, string tofrom)
        {
            try
            {
                string payoutslipno2 = payoutslipno.Substring(0, 3) + payoutslipno.Substring(5) + "_" + seqno;
                string sqlStr_payoutdet = @" INSERT INTO ASSSLIPPAYOUTDET  
                                ( COOP_ID           , ASSISTSLIP_NO     , SEQ_NO   
                                , METHPAYTYPE_CODE  , payto_whom        , ITEMPAY_AMT
                                , moneytype_code    , deptaccount_no    , bank_code
                                , bankbranch_id     , bank_accid        , tofrom_accid
                                , assistslip_control)  
                                VALUES
                                ( {0}               , {1}               , {2}
                                , {3}               , {4}               , {5}
                                , {6}               , {7}               , {8}
                                , {9}               , {10}              , {11}
                                , {12})";
                sqlStr_payoutdet = WebUtil.SQLFormat(sqlStr_payoutdet
                , state.SsCoopId, payoutslipno2, seqno
                , methpay, paytowhom, amt
                , moneytype, deptaccount, bank
                , bankbranch, bank_accid, tofrom
                , payoutslipno);
                WebUtil.ExeSQL(sqlStr_payoutdet);
            }
            catch { LtServerMessage.Text = WebUtil.ErrorMessage("Error ข้อมูล ASSSLIPPAYOUTDET ไม่ถูกต้อง"); return; }
        }
        public void WebDialogLoadEnd()
        {

        }
    }
}