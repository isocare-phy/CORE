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

namespace Saving.Applications.assist.dlg.ws_dlg_as_assistpay_ctrl
{
    public partial class ws_dlg_as_assistpay : PageWebDialog, WebDialog
    {
        [JsPostBack]
        public string PostSave { get; set; }
        [JsPostBack]
        public string PostBank { get; set; }
        [JsPostBack]
        public string PostMoneygroup { get; set; }
        [JsPostBack]
        public string PostChktrn { get; set; }
        [JsPostBack]
        public string PostNewRow { get; set; }
        [JsPostBack]
        public string PostChkfee { get; set; }
        [JsPostBack]
        public string PostDelRow { get; set; }
        
        
        String []assists;

        int currentAssist = 0,ii_saveresult = 0;

        public void InitJsPostBack()
        {
            dsMain.InitDsMain(this);
            dsGain.InitDsGain(this);
        }

        public void WebDialogLoadBegin()
        {
            if (!IsPostBack)
            {
                ii_saveresult = 0;
                assists = Request["assists"].Split(',');
                lbCurrentAssist.Text = (currentAssist + 1) + "/" + assists.Length;
                of_initassistpay();   
            }
            else
            {
                
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == PostBank)
            {
                string bank_code = dsMain.DATA[0].EXPENSE_BANK;
                dsMain.DdBranch(bank_code);
                this.SetOnLoadedScript("dsMain.SetItem(0,'send_system', '');dsMain.GetElement(0,'send_system').disabled = true;dsMain.GetElement(0,'send_system').style.background = '#CCCCCC'");
                //dsMain.DATA[0].SEND_SYSTEM = ""; dsMain.FindDropDownList(0, "send_system").Enabled = false; dsMain.FindDropDownList(0, "send_system").BackColor = Color.Gray;                                               
            }
            else if (eventArg == PostSave)
            {
                SaveData();
                NextAssist();
            }
            else if (eventArg == PostMoneygroup)
            {
                string moneytype_code = dsMain.DATA[0].MONEYTYPE_CODE;
                Chk_Moneytype(moneytype_code);
            }
            else if (eventArg == PostChktrn)
            {
                TRN_SendSystem();
            }
            else if (eventArg == PostNewRow)
            {
                dsGain.InsertLastRow();
                //dsGain.SetItem(0, dsList.DATA.COOP_IDColumn, state.SsCoopControl);//set value to primary key
            }
            else if (eventArg == PostChkfee)
            {
                if (dsMain.DATA[0].MONEYTYPE_CODE == "CBT") { 
                    if(dsMain.DATA[0].EXPENSE_BANK != "" && dsMain.DATA[0].EXPENSE_BRANCH != "" ){
                        decimal ld_fee = 0;
                        string sql = @" 
                                select serviceassist_amt from cmucfbankbranch where bank_code ={0} and branch_id={1}";
                        sql = WebUtil.SQLFormat(sql, dsMain.DATA[0].EXPENSE_BANK, dsMain.DATA[0].EXPENSE_BRANCH);
                        Sdt dt = WebUtil.QuerySdt(sql);
                        if (dt.Next())
                        {
                            ld_fee = dt.GetDecimal("serviceassist_amt");
                            dsMain.DATA[0].fee = ld_fee;
                        }
                        dsMain.DATA[0].ASSIST_AMT = dsMain.DATA[0].ASSIST_AMT - (ld_fee + dsMain.DATA[0].lon_amt);
                    }
                }
            }
            else if (eventArg == PostDelRow)
            {
                int row = dsGain.GetRowFocus();
                dsGain.DeleteRow(row);
            }
        }
        /// <summary>
        /// ดึงสัญญาถัดไป
        /// </summary> 
        private void NextAssist()
        {
            currentAssist  = Convert.ToInt16(HdIndex.Value);
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
            dsMain.DdBranch(dsMain.DATA[0].EXPENSE_BANK);
            string moneytype_code = dsMain.DATA[0].MONEYTYPE_CODE;
            dsMain.DdMoneyType();
            Chk_Moneytype(moneytype_code);             
            dsMain.DdFromAccId();
            HdIndex.Value = currentAssist + "";
            try { dsGain.RetrieveDetail(assist); }
            catch { }
            string ls_assistcode = (dsMain.DATA[0].assisttype_desc).Substring(0, 2);
            if (ls_assistcode == "20")//สมาชิกถึงแก่กรรม
            {
                if (!IsPostBack)
                {
                    dsMain.FindTextBox(0, "lon_amt").Enabled = false;
                    dsMain.DATA[0].chkgain = 1;
                }
            }
            else
            {
                if (!IsPostBack)
                {
                    dsMain.FindTextBox(0, "lon_amt").Enabled = false;
                    dsMain.FindCheckBox(0, "chkgain").Enabled = false;
                }
                this.SetOnLoadedScript("document.getElementById('insertRow').style.display = 'none';document.getElementById('show_box').style.display = 'none';");
                dsGain.Visible = false;
            }
            string ls_membtype = (dsMain.DATA[0].member_type).Substring(0, 2);


//            decimal dec_calapprove = 0;

//            sql = @"select 
//	                typedet.seq_no,
//	                typedet.min_check,
//	                typedet.max_check,
//	                typedet.first_payamt,
//	                typedet.first_typepay,
//	                nvl(typedet.max_firstpayamt, 0) max_firstpayamt
//                from assucfassisttypedet typedet
//                inner join assucfassisttype ucftype on typedet.assisttype_code = ucftype.assisttype_code
//                where typedet.assisttype_code = {0} and typedet.assisttype_pay = {1} and typedet.membtype_code = {2}
//                order by typedet.seq_no";
//            sql = WebUtil.SQLFormat(sql, ls_assistcode, ls_assistpay, ls_membtype);
//            Sdt dt = WebUtil.QuerySdt(sql);
//            if (dt.GetRowCount() > 1) // ถ้ามีมากกว่า 1 row แสดงว่ามีการตรวจสอบ max min check
//            {
//                String sql_age = @"select case assisttype_group when '01' then child_gpa else member_age end chk_condition 
//                                from
//                                (
//	                                select 
//		                                asstype.assisttype_group ,
//		                                ass.child_gpa ,
//		                                (substr(FT_CALAGE(mb.member_date,sysdate,4), 1, instr(FT_CALAGE(mb.member_date,sysdate,4), '.') - 1) * 12) + substr(FT_CALAGE(mb.member_date,sysdate,4), instr(FT_CALAGE(mb.member_date,sysdate,4), '.') + 1, 2) member_age
//	                                from mbmembmaster mb
//	                                inner join assreqmaster ass on mb.member_no = ass.member_no
//	                                inner join assucfassisttype asstype on ass.assisttype_code = asstype.assisttype_code
//	                                where ass.assist_docno = {0})";
//                sql_age = WebUtil.SQLFormat(sql_age, assist);
//                Sdt dt_age = WebUtil.QuerySdt(sql_age);
//                dt_age.Next();

//                for (int ii = 0; ii < dt.GetRowCount(); ii++)
//                {
//                    dt.Next();
//                    if (dt.GetInt32("min_check") <= dt_age.GetInt32("chk_condition") && dt_age.GetInt32("chk_condition") <= dt.GetInt32("max_check"))
//                    {
//                        if (dt.GetInt32("first_typepay") == 1 && dt.GetInt32("max_firstpayamt") == 0) // บาท 
//                        {
//                            dec_calapprove = dsMain.DATA[0].APPROVE_AMT;
//                        }
//                        else if (dt.GetInt32("first_typepay") == 1 && dt.GetInt32("max_firstpayamt") > 0) // บาท แต่มีค่าจ่ายครั้งแรก
//                        {
//                            if (dsMain.DATA[0].APPROVE_AMT > dt.GetInt32("max_firstpayamt"))
//                            {
//                                dec_calapprove = dt.GetInt32("max_firstpayamt");
//                            }
//                            else
//                            {
//                                dec_calapprove = dsMain.DATA[0].APPROVE_AMT;
//                            }
//                        }
//                        else if (dt.GetInt32("first_typepay") == 2 && dt.GetInt32("max_firstpayamt") == 0) // เปอร์เซ็น
//                        {
//                            dec_calapprove = (dsMain.DATA[0].APPROVE_AMT * dt.GetInt32("first_payamt")) / 100;
//                        }
//                        else if (dt.GetInt32("first_typepay") == 2 && dt.GetInt32("max_firstpayamt") > 0)// เปอร์เซ็น แต่มีค่าจ่ายครั้งแรก
//                        {
//                            dec_calapprove = (dsMain.DATA[0].APPROVE_AMT * dt.GetInt32("first_payamt")) / 100;
//                            if (dec_calapprove > dt.GetInt32("max_firstpayamt"))
//                            {
//                                dec_calapprove = dt.GetInt32("max_firstpayamt");
//                            }
//                        }
//                        goto next_step;
//                    }
//                }
//            next_step:
//                dsMain.DATA[0].ASSIST_AMT = dec_calapprove;
//            }
//            else // มี row เดียว
//            {
//                dt.Next();
//                if (dt.GetInt32("first_typepay") == 1 && dt.GetInt32("max_firstpayamt") == 0) // บาท 
//                {
//                    dsMain.DATA[0].ASSIST_AMT = dsMain.DATA[0].APPROVE_AMT;
//                }
//                else if (dt.GetInt32("first_typepay") == 1 && dt.GetInt32("max_firstpayamt") > 0) // บาท แต่มีค่าจ่ายครั้งแรก
//                {
//                    if (dsMain.DATA[0].APPROVE_AMT > dt.GetInt32("max_firstpayamt"))
//                    {
//                        dsMain.DATA[0].ASSIST_AMT = dt.GetInt32("max_firstpayamt");
//                    }
//                    else
//                    {
//                        dsMain.DATA[0].ASSIST_AMT = dsMain.DATA[0].APPROVE_AMT;
//                    }
//                }
//                else if (dt.GetInt32("first_typepay") == 2 && dt.GetInt32("max_firstpayamt") == 0) // เปอร์เซ็น
//                {
//                    dsMain.DATA[0].ASSIST_AMT = (dsMain.DATA[0].APPROVE_AMT * dt.GetInt32("first_payamt")) / 100;
//                }
//                else if (dt.GetInt32("first_typepay") == 2 && dt.GetInt32("max_firstpayamt") > 0)// เปอร์เซ็น แต่มีค่าจ่ายครั้งแรก
//                {
//                    dec_calapprove = (dsMain.DATA[0].APPROVE_AMT * dt.GetInt32("first_payamt")) / 100;
//                    if (dec_calapprove > dt.GetInt32("max_firstpayamt"))
//                    {
//                        dsMain.DATA[0].ASSIST_AMT = dt.GetInt32("max_firstpayamt");
//                    }
//                    else
//                    {
//                        dsMain.DATA[0].ASSIST_AMT = dec_calapprove;
//                    }
//                }
//            }
            

            ////////////////////////////////////

//            String flag = dsMain.DATA[0].stm_flag;
//            //เช็คจ่ายต่อเนื่อง
//            if(flag=="1"){
//                String ls_reqdocno = dsMain.DATA[0].ASSIST_DOCNO;
//                decimal payamt = 0;
//                sql = @"
//                select * from
//                (
//	                select	distinct
//		                 (case assucfassisttype.stm_flag when 0  then   assreqmaster.approve_amt  else  
//			                  case assucfassisttypedet.first_typepay when 1 then assreqmaster.approve_amt else assreqmaster.approve_amt*assucfassisttypedet.first_payamt/100 end end) approve_nextamt,
//		                 assucfassisttype.assisttype_code,                            
//		                 assucfassisttype.stm_flag,
//		                 assreqmaster.assist_docno,   
//		                 assreqmaster.member_no,
//		                 assreqmaster.approve_amt,
//		                 assreqmaster.assist_amt,
//		                 assreqmaster.assistpay_code
//		                 from	
//		                 assreqmaster inner join assucfassisttype on assreqmaster.assisttype_code = assucfassisttype.assisttype_code
//		                 inner join assucfassistpaytype on assreqmaster.assistpay_code = assucfassistpaytype.assistpay_code
//		                 inner join assucfassisttypedet  on assucfassisttypedet.assisttype_pay = assucfassistpaytype.assistpay_code
//		                 and  assucfassisttype.assisttype_code = assucfassisttypedet.assisttype_code and  assreqmaster.assisttype_code =assucfassisttypedet.assisttype_code  
//		                 where assreqmaster.req_status = 1  and  assreqmaster.coop_id ={0}  and  assreqmaster.assist_docno ={1}
//	                order by 
//	                (case assucfassisttype.stm_flag when 0  then   assreqmaster.approve_amt  else  
//		                 case assucfassisttypedet.first_typepay when 1 then assreqmaster.approve_amt else assreqmaster.approve_amt*assucfassisttypedet.first_payamt/100 end end)
//                )where  rownum=1
//                "; //order by approve_nextamt
//                sql = WebUtil.SQLFormat(sql, state.SsCoopId, ls_reqdocno);
//                dt = WebUtil.QuerySdt(sql);
//                if (dt.Next())
//                {
//                    payamt = dt.GetInt32("approve_nextamt");
//                    dsMain.DATA[0].ASSIST_AMT = payamt;
//                }
//            }           
        }

        //chk  MONEYTYPE_GROUP IN('BNK','CHQ','CSH','TRN') ไม่ต้องกรอก ธนาคาร สาขา เลขบัญชี
        private void Chk_Moneytype(string moneytype_code)
        {
            string ls_chkmoneygroup = "";
            string ls_defaultassid = "";
            string sql = @" 
                   SELECT MONEYTYPE_CODE,  
                          MONEYTYPE_GROUP, 
                          MONEYTYPE_DESC,   
                          SORT_ORDER  ,
                          MONEYTYPE_CODE || ' - '|| MONEYTYPE_DESC as MONEYTYPE_DISPLAY,
                          DEFAULTPAY_ACCID
                     FROM CMUCFMONEYTYPE WHERE  MONEYTYPE_GROUP IN('BNK','CHQ','CSH','TRN')  AND MONEYTYPE_CODE={0}  order by sort_order
            ";
            sql = WebUtil.SQLFormat(sql, moneytype_code);
            Sdt dt = WebUtil.QuerySdt(sql);
            if (dt.Next())
            {
                ls_chkmoneygroup = dt.GetString("MONEYTYPE_GROUP").Trim();
                ls_defaultassid = dt.GetString("DEFAULTPAY_ACCID").Trim();
                dsMain.DATA[0].tofrom_accid = ls_defaultassid;
            }
           
            if (ls_chkmoneygroup == "CSH")
            {
                this.SetOnLoadedScript("dsMain.SetItem(0,'expense_bank', '');dsMain.SetItem(0,'expense_branch', '');dsMain.SetItem(0,'deptaccount_no', '');dsMain.SetItem(0,'send_system', '');dsMain.GetElement(0,'expense_bank').disabled = true;dsMain.GetElement(0,'expense_bank').style.background = '#CCCCCC';dsMain.GetElement(0, 'expense_branch').disabled = true;dsMain.GetElement(0, 'expense_branch').style.background = '#CCCCCC';dsMain.GetElement(0, 'deptaccount_no').disabled = true;dsMain.GetElement(0, 'deptaccount_no').style.background = '#CCCCCC';dsMain.GetElement(0, 'send_system').disabled = true;dsMain.GetElement(0, 'send_system').style.background = '#CCCCCC';dsMain.GetElement(0,'fee').disabled = true;");       
                //dsMain.DATA[0].EXPENSE_BANK = ""; dsMain.FindDropDownList(0, "expense_bank").Enabled = false; dsMain.FindDropDownList(0, "expense_bank").BackColor = Color.Gray;
                //dsMain.DATA[0].EXPENSE_BRANCH = ""; dsMain.FindDropDownList(0, "expense_branch").Enabled = false; dsMain.FindDropDownList(0, "expense_branch").BackColor = Color.Gray;
                //dsMain.DATA[0].SEND_SYSTEM = ""; dsMain.FindDropDownList(0, "send_system").Enabled = false; dsMain.FindDropDownList(0, "send_system").BackColor = Color.Gray;
                //dsMain.DATA[0].DEPTACCOUNT_NO = ""; dsMain.FindTextBox(0, "deptaccount_no").BackColor = Color.Gray; dsMain.FindTextBox(0, "deptaccount_no").Enabled = false; 
                
            }
            else if (ls_chkmoneygroup == "TRN")
            {
                TRN_SendSystem();
                //bee dsMain.GetElement(0,'fee').disabled = true;
            }
            else
            {
                dsMain.DdBankDesc();
                //ดึงธนาคารจาก mbmembmaster
                if(dsMain.DATA[0].EXPENSE_BANK == ""){
                    dsMain.DATA[0].EXPENSE_BANK = dsMain.DATA[0].MEXPENSE_BANK;
                    dsMain.DdBranch(dsMain.DATA[0].EXPENSE_BANK.Trim());                    
                    dsMain.DATA[0].EXPENSE_BRANCH = dsMain.DATA[0].MEXPENSE_BRANCH;                                    
                    dsMain.DATA[0].DEPTACCOUNT_NO = dsMain.DATA[0].MEXPENSE_ACCID;
                }
                else if (dsMain.DATA[0].DEPTACCOUNT_NO=="")
                {
                    dsMain.DATA[0].DEPTACCOUNT_NO = dsMain.DATA[0].EXPENSE_ACCID;
                }
                else if (dsMain.DATA[0].DEPTACCOUNT_NO=="")
                {
                    dsMain.DATA[0].DEPTACCOUNT_NO = dsMain.DATA[0].MEXPENSE_ACCID;
                }
                this.SetOnLoadedScript("dsMain.GetElement(0,'expense_bank').disabled = false;dsMain.GetElement(0,'expense_bank').style.background = '#FFFFFF';dsMain.GetElement(0, 'expense_branch').disabled = false;dsMain.GetElement(0, 'expense_branch').style.background = '#FFFFFF';dsMain.GetElement(0, 'deptaccount_no').disabled = false;dsMain.GetElement(0, 'deptaccount_no').style.background = '#FFFFFF';dsMain.GetElement(0, 'deptaccount_no').style.background = '#FFFFFF';dsMain.SetItem(0,'send_system', '');dsMain.GetElement(0, 'send_system').disabled = true;dsMain.GetElement(0, 'send_system').style.background = '#CCCCCC';");
                //dsMain.FindDropDownList(0, "expense_bank").Enabled = true; dsMain.FindDropDownList(0, "expense_bank").BackColor = Color.White;
                //dsMain.FindDropDownList(0, "expense_branch").Enabled = true; dsMain.FindDropDownList(0, "expense_branch").BackColor = Color.White;
                //dsMain.FindTextBox(0, "deptaccount_no").Enabled = true; dsMain.FindTextBox(0, "deptaccount_no").BackColor = Color.White;
                //dsMain.DATA[0].SEND_SYSTEM = ""; dsMain.FindDropDownList(0, "send_system").Enabled = false; dsMain.FindDropDownList(0, "send_system").BackColor = Color.Gray;                                               
               
            }
        }
        private void TRN_SendSystem() {
            if (dsMain.DATA[0].SEND_SYSTEM == "LON")
            {
                this.SetOnLoadedScript("dsMain.SetItem(0,'expense_bank', '');dsMain.SetItem(0,'expense_branch', '');dsMain.SetItem(0,'deptaccount_no', '');dsMain.GetElement(0,'expense_bank').disabled = true;dsMain.GetElement(0,'expense_bank').style.background = '#CCCCCC';dsMain.GetElement(0, 'expense_branch').disabled = true;dsMain.GetElement(0, 'expense_branch').style.background = '#CCCCCC';dsMain.SetItem(0,'deptaccount_no', '');dsMain.GetElement(0, 'deptaccount_no').disabled = true;dsMain.GetElement(0, 'deptaccount_no').style.background = '#CCCCCC';");     
                //dsMain.DATA[0].EXPENSE_BANK = ""; dsMain.FindDropDownList(0, "expense_bank").Enabled = false; dsMain.FindDropDownList(0, "expense_bank").BackColor = Color.Gray;
                //dsMain.DATA[0].EXPENSE_BRANCH = ""; dsMain.FindDropDownList(0, "expense_branch").Enabled = false; dsMain.FindDropDownList(0, "expense_branch").BackColor = Color.Gray;
                //dsMain.DATA[0].DEPTACCOUNT_NO = ""; dsMain.FindTextBox(0, "deptaccount_no").Enabled = false; dsMain.FindTextBox(0, "deptaccount_no").BackColor = Color.Gray;                
            }
            else
            {
                if (dsMain.DATA[0].SEND_SYSTEM == "") {
                    dsMain.DATA[0].SEND_SYSTEM = "DEP";
                }
                if (dsMain.DATA[0].r_deptno == "")
                {
                    String sql = @" 
                            select deptaccount_no from dpdeptmaster where member_no={1}
                            and deptclose_status=0 and rownum=1  and coop_id={0}";
                    sql = WebUtil.SQLFormat(sql, state.SsCoopId, dsMain.DATA[0].MEMBER_NO.Trim());
                    Sdt dt = WebUtil.QuerySdt(sql);
                    if (dt.Next())
                    {
                        String deptno = dt.GetString("deptaccount_no").Trim();
                        dsMain.DATA[0].DEPTACCOUNT_NO = deptno;
                    }
                }
                else {
                    dsMain.DATA[0].DEPTACCOUNT_NO = dsMain.DATA[0].r_deptno;
                }
                this.SetOnLoadedScript("dsMain.SetItem(0,'expense_bank', '');dsMain.SetItem(0,'expense_branch', '');dsMain.GetElement(0,'expense_bank').disabled = true;dsMain.GetElement(0,'expense_bank').style.background = '#CCCCCC';dsMain.GetElement(0, 'expense_branch').disabled = true;dsMain.GetElement(0, 'expense_branch').style.background = '#CCCCCC';dsMain.GetElement(0, 'deptaccount_no').disabled = false;dsMain.GetElement(0, 'deptaccount_no').style.background = '#FFFFFF';dsMain.GetElement(0,'fee').disabled = true;");
                //dsMain.DATA[0].EXPENSE_BANK = ""; dsMain.FindDropDownList(0, "expense_bank").Enabled = false; dsMain.FindDropDownList(0, "expense_bank").BackColor = Color.Gray;
                //dsMain.DATA[0].EXPENSE_BRANCH = ""; dsMain.FindDropDownList(0, "expense_branch").Enabled = false; dsMain.FindDropDownList(0, "expense_branch").BackColor = Color.Gray;
                //dsMain.FindTextBox(0, "deptaccount_no").Enabled = true; dsMain.FindTextBox(0, "deptaccount_no").BackColor = Color.White;                
            }
        }      
        private void SaveData()
        {
            //this.ConnectSQLCA();
            String flag = dsMain.DATA[0].stm_flag;
            try
            {                
                //ta.RollBack();                          
                String ls_memno = dsMain.DATA[0].MEMBER_NO;
                String ls_assisttypecode = dsMain.DATA[0].ASSISTTYPE_CODE;
                String ls_assisttypepay = dsMain.DATA[0].ASSISTPAY_CODE;
                String ls_reqdocno = dsMain.DATA[0].ASSIST_DOCNO;
                String ls_money = dsMain.DATA[0].MONEYTYPE_CODE;
                String ls_deptno = dsMain.DATA[0].DEPTACCOUNT_NO;
                String ls_bank = dsMain.DATA[0].EXPENSE_BANK;
                String ls_branch = dsMain.DATA[0].EXPENSE_BRANCH;
                String ls_accid = dsMain.DATA[0].EXPENSE_ACCID;
                String ls_tofromaccid = dsMain.DATA[0].tofrom_accid;                
                DateTime slip_date    = dsMain.DATA[0].REQ_DATE;
                decimal ld_approveamt = dsMain.DATA[0].APPROVE_AMT;
                decimal ld_assistamt  = dsMain.DATA[0].ASSIST_AMT;
                decimal ld_lastperiod = 1;    

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
                /////////////////////////
                if (dsMain.DATA[0].chkgain == 1)
                {
                    for (int ii = 0; ii < dsGain.RowCount; ii++)
                    {
                        if (dsGain.DATA[ii].ASSIST_AMT == 0 || dsGain.DATA[ii].gain_fullname == "")
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('ตรวจสอบชื่อและ จำนวนเงินของผู้รับผลประโยชน์');", true);
                            return;
                        }
                    }
                }
                /////////////////////////
                string ls_payoutslipno = wcf.NCommon.of_getnewdocno(state.SsWsPass, state.SsCoopId, "ASSISTSLIPNO");                               
                
                if (flag == "1")
                {
                    decimal ld_lastseqno = 0, ld_paybalance=0;
                    decimal witdraw = ld_approveamt - ld_assistamt;
                    string ls_asscontractno = "", ls_contractno = "";
                    int li_numpay = 1;

                    sql = @"select num_pay from assucfassisttypedet where coop_id = {0} and assisttype_code = {1} and assisttype_pay = {2}  and rownum = 1";
                    sql = WebUtil.SQLFormat(sql, state.SsCoopId, ls_assisttypecode, ls_assisttypepay);
                    dt = WebUtil.QuerySdt(sql);
                    if(dt.Next()){ li_numpay = dt.GetInt32("num_pay"); }

                    sql = @"select asscontract_no,last_stm,last_periodpay,pay_balance  from asscontmaster where coop_id={0} and assisttype_code={1} and assistpaytype_code={2}  
                          and member_no={3}";
                    sql = WebUtil.SQLFormat(sql, state.SsCoopId, ls_assisttypecode, ls_assisttypepay, ls_memno);
                    dt = WebUtil.QuerySdt(sql);
                    if (dt.Next())
                    {
                        ls_asscontractno = dt.GetString("asscontract_no");
                        ld_lastseqno = dt.GetInt32("last_stm");
                        ld_lastperiod = dt.GetInt32("last_periodpay");
                        ld_paybalance = dt.GetDecimal("pay_balance");
                    }
                    //////////////////// ASSCONTMASTER /////////////////////
                    if (ls_asscontractno == "")
                    {
                        ls_contractno = wcf.NCommon.of_getnewdocno(state.SsWsPass, state.SsCoopId, "ASSISTCONTNO");
                        ld_lastseqno = 1;
                        try
                        {
                            //ขาด          MAX_PERIODPAY   LAST_PERIODPAY  
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
                    else 
                    { 
                        ls_contractno = ls_asscontractno;
                        ld_lastseqno = ld_lastseqno + 1;
                        ld_lastperiod = ld_lastperiod + 1;
                        ld_paybalance = ld_paybalance + ld_assistamt ;
                        witdraw = ld_approveamt - ld_paybalance;
                        try
                        {

                            string sqlStr_update = @" UPDATE ASSCONTMASTER SET WITHDRAWABLE_AMT = {2} ,PAY_BALANCE={3},LAST_STM={4},last_periodpay={5}
                                                    where coop_id={0} and ASSCONTRACT_NO = {1} 
                                                    ";
                            sqlStr_update = WebUtil.SQLFormat(sqlStr_update
                            , state.SsCoopId, ls_contractno
                            , witdraw , ld_paybalance, ld_lastseqno, ld_lastperiod);
                            WebUtil.ExeSQL(sqlStr_update);
                        }
                        catch { LtServerMessage.Text = WebUtil.ErrorMessage("Error  UPDATE ASSCONTMASTER "); return; }  
                    }
                    //////////////////// ASSCONTSTATEMENT /////////////////////
                    try
                    {
                        string sqlStr_statement = @"  INSERT INTO ASSCONTSTATEMENT  
                                ( COOP_ID           , COOP_CONTROL      , ASSCONTRACT_NO   
                                , SEQ_NO            , SLIP_DATE         , OPERATE_DATE
                                , REF_SLIPNO        , PERIOD            , PAY_BALANCE   
                                , MONEYTYPE_CODE    , ITEM_STATUS       , ENTRY_ID
                                , ENTRY_DATE
                                )   
                        VALUES ( {0}                , {0}               , {1}
                                , {2}               , {3}               , {4}
                                , {5}               , {6}               , {7}   
                                , {8}               , 1                 , {9}   
                                , {10}
                                )
                                ";
                        sqlStr_statement = WebUtil.SQLFormat(sqlStr_statement
                        , state.SsCoopId, ls_contractno
                        , ld_lastseqno, slip_date, state.SsWorkDate
                        , ls_payoutslipno, ld_lastperiod, ld_assistamt
                        , ls_money, state.SsUsername
                        , state.SsWorkDate
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
                    if (ls_money != "TRN")
                    {
                        ls_deptno = "";
                        ls_accid = dsMain.DATA[0].DEPTACCOUNT_NO;
                    }
                    string sqlStr_payout = @" INSERT INTO ASSSLIPPAYOUT  
                             ( COOP_ID,                COOP_CONTROL,           ASSISTSLIP_NO,   
                               CAPITAL_YEAR,           PAY_PERIOD,             ASSISTTYPE_CODE,   
                               SLIP_DATE,              OPERATE_DATE,           MEMBER_NO,   
                               SLIP_STATUS,            PAYOUT_AMT,             MONEYTYPE_CODE,   
                               DEPACCOUNT_NO,          BANK_CODE,              BANKBRANCH_ID,   
                               BANK_ACCID,             TOFROM_ACCID,           ENTRY_ID,   
                               ENTRY_DATE,             POST_TOFIN,             REF_REQDOCNO )  
                      VALUES ( {0},                     {0},                    {1},
                               {2},                     {3},                    {4},   
                               {5},                     {6},                    {7},
                                1,                      {8},                    {9},
                               {10},                    {11},                   {12},
                               {13},                    {14},                   {15},
                               {16},                       0,                   {17})  
                            ";
                    sqlStr_payout = WebUtil.SQLFormat(sqlStr_payout
                    , state.SsCoopId, ls_payoutslipno
                    , li_year, ld_lastperiod, ls_assisttypecode
                    , slip_date, state.SsWorkDate, ls_memno
                    , ld_assistamt, ls_money, ls_deptno
                    , ls_bank, ls_branch, ls_accid
                    , ls_tofromaccid, state.SsUsername, state.SsWorkDate
                    , ls_reqdocno);
                    WebUtil.ExeSQL(sqlStr_payout);
                }
                catch
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("Error ASSSLIPPAYOUT"); return;
                }
                //////////////////// ASSSLIPPAYOUTDET /////////////////////
                int ld_seqno = 0;
                if (dsMain.DATA[0].lon_amt > 0)
                {
                    ld_seqno = ld_seqno + 1;
                    SaveSlippayout(ls_payoutslipno, ld_seqno, "LON", "ชำระหนี้", dsMain.DATA[0].lon_amt);
                }
                if (dsMain.DATA[0].chkgain == 1)
                {
                    for (int ii = 0; ii < dsGain.RowCount; ii++)
                    {
                        ld_seqno = ld_seqno + 1;
                        SaveSlippayout(ls_payoutslipno, ld_seqno, "GAN", dsGain.DATA[ii].gain_fullname, dsGain.DATA[ii].ASSIST_AMT);
                    }
                }
                else
                {
                    if (ls_money == "CBT")
                    {
                        if (dsMain.DATA[0].fee > 0)
                        {
                            ld_seqno = ld_seqno + 1;
                            SaveSlippayout(ls_payoutslipno, ld_seqno, "FEE", "ค่าธรรมเนียมธนาคาร", dsMain.DATA[0].fee);
                        }
                        ld_seqno = ld_seqno + 1;
                        SaveSlippayout(ls_payoutslipno, ld_seqno, "CBT", "ค่าธรรมเนียมธนาคาร", dsMain.DATA[0].ASSIST_AMT);
                    }
                    else
                    {
                        ld_seqno = ld_seqno + 1;
                        SaveSlippayout(ls_payoutslipno, ld_seqno, dsMain.DATA[0].MONEYTYPE_CODE, "", dsMain.DATA[0].ASSIST_AMT);
                    }
                }





                //////////////////// DPDEPTTRAN /////////////////////
                if(dsMain.DATA[0].SEND_SYSTEM=="DEP"){
                    try {
                        int ls_depttran_seqno = 1;
                        sql = @"select 
	                        max(SEQ_NO) SEQ_NO
	                    from DPDEPTTRAN where DEPTACCOUNT_NO = {1} and SYSTEM_CODE='ASS' and TRAN_YEAR={2} and TRAN_DATE={3} and COOP_ID={0}  ";
                        sql = WebUtil.SQLFormat(sql, state.SsCoopId, dsMain.DATA[0].DEPTACCOUNT_NO.Trim(), li_year, slip_date);
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
                        , state.SsCoopId, dsMain.DATA[0].DEPTACCOUNT_NO.Trim(), ls_memno
                        , li_year, slip_date
                        , ld_assistamt, ls_depttran_seqno
                        );
                        WebUtil.ExeSQL(sqlStr_cont);                
                    }
                    catch {
                        LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถส่งข้อมูลไประบบเงินฝากได้ " + ls_memno); return; 
                    }
                }
//                else if (dsMain.DATA[0].SEND_SYSTEM == "LON")
//                {
//                    try
//                    {
//                        string sqlStr_cont = @"
//                         INSERT INTO SLTRANSPAYIN  
//                         ( COOP_ID,					MEMCOOP_ID,				    MEMBER_NO,              TRANS_DATE,		
//                           SEQ_NO,                  SYSTEM_CODE,				CONCOOP_ID,			    TRANS_AMT,		    	
//                           TRNSOURCE_STATUS,        MONEYTYPE_CODE,             TRANSPAY_TYPE)  
//                  VALUES ( {0},				        {0},	        			{1},                    {2},   
//                           '1',				    	'ASS',					    {0},                    {3},   
//                           '8',                     {4},						'2'
//		                  )";
//                        sqlStr_cont = WebUtil.SQLFormat(sqlStr_cont
//                        , state.SsCoopId, ls_memno ,slip_date
//                        , ld_approveamt
//                        , ls_money );
//                        WebUtil.ExeSQL(sqlStr_cont);
//                    }
//                    catch
//                    {
//                        LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถส่งข้อมูลไประบบสินเชื่อได้ได้ " + ls_memno); return;
//                    }
                //}
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
        public void SaveSlippayout(string payoutslipno, int seqno, string methpay,string paytowhom,decimal amt)
        {
            try
            {
                string sqlStr_payoutdet = @" INSERT INTO ASSSLIPPAYOUTDET  
                             ( COOP_ID,                ASSISTSLIP_NO,           SEQ_NO,   
                               METHPAYTYPE_CODE,       payto_whom,        ITEMPAY_AMT)  
                      VALUES ( {0},                     {1},                    {2},
                               {3},                     {4},                    {5}
                              )";
                sqlStr_payoutdet = WebUtil.SQLFormat(sqlStr_payoutdet
                , state.SsCoopId, payoutslipno, seqno, methpay, paytowhom , amt);
                WebUtil.ExeSQL(sqlStr_payoutdet);
            }
            catch { LtServerMessage.Text = WebUtil.ErrorMessage("Error ข้อมูล ASSSLIPPAYOUTDET ไม่ถูกต้อง"); return; }
        }
        public void WebDialogLoadEnd()
        {
            string moneytype_code = dsMain.DATA[0].MONEYTYPE_CODE;
            Chk_Moneytype(moneytype_code);
            string ls_assistcode = (dsMain.DATA[0].assisttype_desc).Substring(0, 2);
            if (ls_assistcode != "20")//ไม่ใช่สมาชิกถึงแก่กรรม
            {               
                this.SetOnLoadedScript("document.getElementById('insertRow').style.display = 'none';document.getElementById('show_box').style.display = 'none';");
                dsGain.Visible = false;
            }
        }
    }
}