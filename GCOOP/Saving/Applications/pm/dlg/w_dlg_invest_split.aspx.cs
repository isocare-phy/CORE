using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sybase.DataWindow;
using CoreSavingLibrary.WcfNPm;
using DataLibrary;
using CoreSavingLibrary;

namespace Saving.Applications.pm.dlg
{
    public partial class w_dlg_invest_split : PageWebDialog, WebDialog
    {

        protected String jsPostInsertRow;
        protected String jsPostDeleteRow;
        protected String jsPostSave;
        protected String jsPostBlank;
        string pbl = "pm_investment.pbl";

        #region WebDealog Members
        public void InitJsPostBack()
        {
            jsPostInsertRow = WebUtil.JsPostBack(this, "jsPostInsertRow");
            jsPostDeleteRow = WebUtil.JsPostBack(this, "jsPostDeleteRow");
            jsPostSave = WebUtil.JsPostBack(this, "jsPostSave");
            jsPostBlank = WebUtil.JsPostBack(this, "jsPostBlank");
        }

        public void WebDialogLoadBegin()
        {
            //n_pmClient svPm = wcf.NPm;

            if (!IsPostBack)
            {
                DwHead.InsertRow(0);
                DwHead.SetItemDecimal(1, "unit_amt", Convert.ToDecimal(Request["unit_amt"]));
                DwHead.SetItemDecimal(1, "money_amout", Convert.ToDecimal(Request["prncbal"]));
                InsertRow();
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwHead);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "jsPostInsertRow":
                    InsertRow();
                    break;
                case "jsPostDeleteRow":
                    DeleteRow();
                    break;
                case "jsPostSave":
                    SaveInvestSplit();
                    break;
            }
        }

        public void WebDialogLoadEnd()
        {
            DwMain.SaveDataCache();
            DwHead.SaveDataCache();
        }
        #endregion

        #region Function
        private void InsertRow()
        {
            String investment_DUE_DATE = Request["investment_due_date"];
            String registration_no = Request["registration_no"];
            int row = DwMain.InsertRow(0);
            DwMain.SetItemString(row, "investment_duedate", investment_DUE_DATE);
            DwMain.SetItemString(row, "regist_no", registration_no);
        }

        private void DeleteRow()
        {
            int row = Convert.ToInt32(Hdrow.Value);
            DwMain.DeleteRow(row);
        }

        private void SaveInvestSplit()
        {
            try
            {
                String as_xml = DwMain.Describe("Datawindow.Data.XML");
                String account_no = Request["account_no"];
                String branch_id = state.SsCoopId;
                String username = state.SsUsername;
                String WorkDate = state.SsWorkDate.ToString("dd/MM/yyyy");
                string[] tmpdate_start;
                string as_date_chk;

                string regist, duedate, duedate_get, sql_return, acc_no, slip_no , doc_no;
                decimal unit,money,unit_cost;
                double date_con;
                for (int i = 1; i < DwMain.RowCount +1; i++)
                {
                regist = DwMain.GetItemString(i, "regist_no");
                duedate_get = DwMain.GetItemString(i, "investment_duedate");
                date_con = Convert.ToDouble(duedate_get);
                duedate = date_con.ToString("##/##/####");
                tmpdate_start = duedate.Split('/');
                as_date_chk = tmpdate_start[0] + "/" + tmpdate_start[1] + "/" + (Convert.ToDecimal(tmpdate_start[2]) - 543);
                unit = DwMain.GetItemDecimal(i, "unit_amt");
                money = DwMain.GetItemDecimal(i, "money_amount");
                unit_cost = 0;
                acc_no = get_document_no(state.SsCoopId, "INVESTACCOUNT");//svPm.of_getdocumentno(state.SsWsPass, state.SsCoopId, "INVESTACCOUNT");
                doc_no = get_document_no(state.SsCoopId, "INVESTDOCNO"); //svPm.of_getdocumentno(state.SsWsPass, state.SsCoopId, "INVESTDOCNO"); 
                //slip_no = InvestmentService.of_getdocumentno(state.SsWsPass, state.SsCoopId, "INVESTSLIP");
                sql_return = ins_master(acc_no, money, unit, as_date_chk, regist, account_no, branch_id);
                sql_return = ins_intrate(acc_no, account_no, branch_id);
                //sql_return = ins_duedate(acc_no, account_no, branch_id, as_date_chk);
                sql_return = ins_reqinvest(acc_no, doc_no, as_date_chk, money, unit, unit_cost, branch_id, account_no);
                sql_return = ins_statement(branch_id, acc_no, WorkDate, money, username);
                
               }
                //sql_return = cancle_old(account_no, branch_id, username, WorkDate);
                
                //short result = InvestmentService.of_split_invest(state.SsWsPass, as_xml, account_no, branch_id, username, state.SsWorkDate);
                //if (result == 1)
                //{
                //    Hdfin.Value = "true";
                //}
                //else
                //{
                //    LtServerMessage.Text = WebUtil.ErrorMessage("บันทึกข้อมูลไม่สำเร็จ");
                //}

                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลสำเร็จ");
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }
        #endregion
        #region Function_ins
        public string ins_master(string acc_no, decimal prnc_bal, decimal unit,string invest_duedate,string regis_no,string acc_no_old,string coop_id)
        {
            string sql = @"INSERT INTO pminvestmaster  
	                    ( COOP_ID,
						ACCOUNT_NO,  
	                      INVSOURCE_CODE, 
	                      INVESTTYPE_CODE,   
	                      BRANCH_ID,   			
	                      OPEN_DATE,  
	                      DUE_DATE,   
	                      CLOSE_STATUS,   
	                      CLOSE_DATE,   
	                      ACCOUNT_NAME,   		
	                      COND_WITHDRAW,
	                      TAX_RATE,   
	                      BANK_CODE,   
	                      BANK_BRANCH,   		
	                      TRAN_BANKACC_NO,   
	                      TRANACC_NAME,   
	                      PRNCBAL,   
	                      LASTCALINT_DATE,   
	                      ACCUINT_AMT,   		 
	                      ACCUINTRCV_AMT,   
	                      ACCUTAXPAY_AMT,   
	                      WITHDRAW_COUNT,   
	                      LASTSTMSEQ_NO,   	
	                      LASTACCESS_DATE,   
	                      REMARK,   
	                      INT_RCV_ABLE,   
	                      INT_RCV_ABLE_SUM,   
	                      START_INTDATE,   		
	                      UNIT_AMT,   
	                      UNIT_COST,   
	                      ACCID_PRNC,   
	                      PURCHASE_PRICE,   
	                      SYMBOL_CODE,   		
	                      RATE_CODE,   
	                      PUSH_STATUS,   
	                      PUSH_DATE,   
	                      CALL_STATUS,   
	                      CALL_DATE,   			
	                      INVESTMENT_DOCNO,
						DAY_INYEAR,
	                      INVESTMENT_PERIOD,
	                      INVEST_PERIOD_UNIT,
						ACCID_INT,
	                      ORGAN_WARRANTY,
	                      INVEST_DETAIL,			
	                      DURATION_DUE,
	                      BUY_DATE,
	                      INT_TIMEDUE,
	                      DEFF_AMT,
	                      CLEANPRICE_AMT,
						SUBORDINATED,
						CLEANPRICE_PRESENT,
						INTYIELD_PRESENT,
	                      NODUE_FLAG
	                    )   
	                    ( 
                    SELECT
						COOP_ID,
	                     '" + acc_no + @"',  
	                      INVSOURCE_CODE, 
	                      INVESTTYPE_CODE,   
	                      BRANCH_ID,   			
	                      OPEN_DATE,  
	                      to_date('" + invest_duedate + @"','dd/mm/yyyy') as DUE_DATE,   
	                      0,   
	                      NULL,   
	                      ACCOUNT_NAME,   		
	                      COND_WITHDRAW,
	                      TAX_RATE,   
	                      BANK_CODE,   
	                      BANK_BRANCH,   		
	                      TRAN_BANKACC_NO,   
	                      TRANACC_NAME,   
	                      " + prnc_bal + @" as prnc_bal,  
	                      LASTCALINT_DATE,   
	                      0.00,   		 
	                      0.00,   
	                      0.00,   
	                      0,   
	                      1,   	
	                      LASTACCESS_DATE,   
	                      REMARK,   
	                      0.00,   
	                      0.00,   
	                      START_INTDATE,   		
	                      " + unit + @" as unit,   
	                      UNIT_COST,   
	                      ACCID_PRNC,   
	                      PURCHASE_PRICE,   
	                      SYMBOL_CODE,   		
	                      RATE_CODE,   
	                      PUSH_STATUS,   
	                      PUSH_DATE,   
	                      CALL_STATUS,   
	                      CALL_DATE,   			
	                      INVESTMENT_DOCNO,
						DAY_INYEAR,
	                      INVESTMENT_PERIOD,
	                      INVEST_PERIOD_UNIT,
	                      ACCID_INT,
	                      ORGAN_WARRANTY,
	                      INVEST_DETAIL,			
	                      0,
	                      BUY_DATE,
	                      INT_TIMEDUE,
	                      0,	                      		
	                      " + prnc_bal + @" as prnc_bal, 
						SUBORDINATED,	
						CLEANPRICE_PRESENT,
						INTYIELD_PRESENT,
	                      NODUE_FLAG	                    
	                      FROM  pminvestmaster
	                      WHERE ACCOUNT_NO = '" + acc_no_old + @"'
	                          AND COOP_ID = '" + coop_id + @"')
                            ";
           
            Sdt res = WebUtil.QuerySdt(sql);
            return sql;
        
        }
        public string ins_intrate(string acc_no, string acc_no_old, string coop_id) 
        {
            string sql = @"INSERT INTO PMINVESTINTRATE  
	(	COOP_ID,   
		ACCOUNT_NO,  
		SEQ_NO,   
		INT_START_DATE,   
		INT_END_DATE,   
		INT_RATE,   
		INT_YIELD_RATE,   
		INT_DESC )  
	(
SELECT COOP_ID,  
			'" + acc_no + @"',   
			SEQ_NO,   
			INT_START_DATE,   
			INT_END_DATE,   
			INT_RATE,   
			INT_YIELD_RATE,   
			INT_DESC
		FROM PMINVESTINTRATE
		WHERE ACCOUNT_NO = '" + acc_no_old + @"'
		AND COOP_ID = '" + coop_id + @"')
                            ";
            Sdt res = WebUtil.QuerySdt(sql);
            return sql;
        }
        public string ins_duedate(string acc_no, string acc_no_old, string coop_id, string invest_duedate)
        {
            string sql = @"INSERT INTO PMINVESTDUEDATE  
	(	INVESTDUEDATE_ID,   
		DUE_DATE,   
		ACCOUNT_NO,   
		MASTER_BRANCH_ID,   
		START_CALINT_DATE,   
		LAST_CAL_TO_DATE,   
		RECINT_FLAG,   
		C1_AMT,   
		PV_OF_C1,   
		PVC1_OF_V,   
		PVC1OFV_BY_T,   
		INT_RATE,   
		INTYIELD_RATE,   
		CUT_DIFFER,   
		OPERATE_DATE,   
		INT_AMT )  
	(	
SELECT PMINVESTDUEDATE_SEQ.NEXTVAL,   
			DUE_DATE,   
			'"+acc_no+@"',   
			MASTER_BRANCH_ID,   
			START_CALINT_DATE,   
			LAST_CAL_TO_DATE,   
			RECINT_FLAG,   
			0.00,   
			0.00,   
			0.00,   
			0.00,   
			INT_RATE,   
			INTYIELD_RATE,   
			0.00,   
			null,   
			0.00
		FROM PMINVESTDUEDATE
		WHERE ACCOUNT_NO = '"+acc_no_old+@"'
		AND MASTER_BRANCH_ID = '"+coop_id+ @"'
        AND LAST_CAL_TO_DATE <= to_date('" + invest_duedate + @"','dd/mm/yyyy')
        )
                            ";
            Sdt res = WebUtil.QuerySdt(sql);
            return sql;
        }
        public string ins_reqinvest(string acc_no, string doc_no, string invest_duedate, decimal money, decimal unit, decimal unit_cost, string coop_id, string acc_no_old)
        {
            string sql = 
            @"insert into pmreqinvestment
                            (COOP_ID,
						 REQINVESTMENT_NO,
						 ACCOUNT_NO, 
 						 OPEN_DATE,
                            DUE_DATE,
                            INVSOURCE_CODE,
                            INVESTTYPE_CODE,
                            ACCID_PRNC,
                            PURCHASE_PRICE,
                            SYMBOL_CODE,
                            RATE_CODE,
                            MATURITY_PRICE,
                            UNIT_AMT,
                            UNIT_COST,
                            INVESTMENT_PERIOD,
                            INVEST_PERIOD_UNIT,
                            INVESTMENT_DOCNO,
						 BRANCH_ID,
                            ACCOUNT_NAME,
                            TAX_RATE,
                            ENTRY_ID,
                            ENTRY_DATE,
                            DAY_INYEAR,
                            ORGAN_WARRANTY,
                            DURATION_DUE,
                            BUY_DATE,
                            INVEST_DETAIL,
                            INT_TIMEDUE,
                            DEFF_AMT,
                            BANK_CODE,
                            BANK_BRANCH,
                            TRAN_BANKACC_NO,
                            ACCID_INT,
                            PUSH_STATUS,
                            PUSH_DATE,
                            CALL_STATUS,
                            CALL_DATE,
                            REMARK,
                            SUBORDINATED,
                            CLEANPRICE_AMT,
                            NODUE_FLAG
                            ) 
						(
                            select COOP_ID,
						 '" + doc_no + @"' as REQINVESTMENT_NO,
						 '" + acc_no + @"' as ACCOUNT_NO,
						 OPEN_DATE,
                            to_date('" + invest_duedate + @"','dd/mm/yyyy') as DUE_DATE, 	
                            INVSOURCE_CODE,
						 INVESTTYPE_CODE,
                            ACCID_PRNC,
                            PURCHASE_PRICE,
                            SYMBOL_CODE,
                            RATE_CODE,
                            " + money + @" as MATURITY_PRICE,
                            UNIT_AMT as UNIT_AMT, 
                            UNIT_COST as UNIT_COST, 
                            INVESTMENT_PERIOD,
                            INVEST_PERIOD_UNIT,
                            INVESTMENT_DOCNO,
						 BRANCH_ID,
                            ACCOUNT_NAME,
                            TAX_RATE,
                            ENTRY_ID,
                            ENTRY_DATE,
                            DAY_INYEAR,
                            ORGAN_WARRANTY,
                            DURATION_DUE,
                            BUY_DATE,
                            INVEST_DETAIL,
                            INT_TIMEDUE,
                            DEFF_AMT,
                            BANK_CODE,
                            BANK_BRANCH,
                            TRAN_BANKACC_NO,
                            ACCID_INT,
                            PUSH_STATUS,
                            PUSH_DATE,
                            CALL_STATUS,
                            CALL_DATE,
                            REMARK,
                            SUBORDINATED,
                            CLEANPRICE_AMT,
                            NODUE_FLAG                            
                            from pmreqinvestment 
                            where COOP_ID = '" + coop_id + @"' AND
                            ACCOUNT_NO = '" + acc_no_old + @"' 
                            )";
            Sdt res = WebUtil.QuerySdt(sql);
            return sql;
        }
        public string ins_statement(string coop_id, string acc_no, string WorkDate, decimal money, string username) 
        {

            string sql = @"
                            insert into 
                                PMINVESTSTATEMENT 
                                (COOP_ID ,
                                 ACCOUNT_NO,
                                 SEQ_NO , 
                                MOVEMENT_DATE ,
                                 EFFECTIVE_DATE ,
                                 ITEM_CODE,
                                 ITEM_SIDE ,
                                 BALANCE_BEFORE ,
                                 OUTSTANDING_BALANCE ,
                                 ENTRY_ID ,
                                 ENTRY_DATE ,
                                 SLIPOPERATE_NO , 
                                ITEM_AMOUNT 
                                ) values
                                (
                                '"+coop_id+@"',
                                '"+acc_no+@"',
                                1,
                                  to_date('"+WorkDate+@"','dd/mm/yyyy'),
                                  to_date('"+WorkDate+@"','dd/mm/yyyy'),
                                'OAC',
                                1,
                                0,
                                "+money+@",
                                '" + username + @"',
                                to_date('" +WorkDate+@"','dd/mm/yyyy'),
                                null,
                                "+money+@"
                                )
                              ";
            Sdt res = WebUtil.QuerySdt(sql);
            return sql;
        }
        public string cancle_old(string acc_no_old, string coop_id,string user_name,string date)
        {
            string sql = @"UPDATE	pminvestmaster
      SET CLOSE_STATUS = -9 ,   
             CLOSE_DATE =  to_date('"+date+@"','dd/mm/yyyy'),
             CANCEL_REASON = 'ยกเลิกเนื่องจาก แยกบัญชีลงทุน',
             CANCEL_ID = '"+user_name+@"',
             CANCEL_DATE =  to_date('" + date + @"','dd/mm/yyyy')
  WHERE account_no = '"+acc_no_old+@"'
       AND branch_id = '"+coop_id+@"'
                            ";
            //Sdt res = WebUtil.QuerySdt(sql);
            return sql;
        }
        public string get_document_no(string CoopId, string document_code)
        {
            string sql_chk = "", DOCUMENT_CODE = "", DOCUMENT_YEAR = "", docno = "", DOCUMENT_PREFIX = "", DOCUMENT_FORMAT = "" , zero ="";
            decimal DOCUMENT_LENGTH, LAST_DOCUMENTNO = 0;
            
            sql_chk = @"select DOCUMENT_CODE,DOCUMENT_LENGTH,DOCUMENT_YEAR,LAST_DOCUMENTNO , DOCUMENT_PREFIX , DOCUMENT_FORMAT from cmdocumentcontrol where DOCUMENT_CODE = '" + document_code + "' AND COOP_ID = '" + state.SsCoopId + "' ";
            Sdt dt_chk = WebUtil.QuerySdt(sql_chk);
            if (dt_chk.Next())
            {
                DOCUMENT_CODE = dt_chk.GetString("DOCUMENT_CODE");
                DOCUMENT_LENGTH = dt_chk.GetDecimal("DOCUMENT_LENGTH");
                DOCUMENT_YEAR = dt_chk.GetString("DOCUMENT_YEAR");
                LAST_DOCUMENTNO = dt_chk.GetDecimal("LAST_DOCUMENTNO");
                DOCUMENT_PREFIX = dt_chk.GetString("DOCUMENT_PREFIX");
                DOCUMENT_FORMAT = dt_chk.GetString("DOCUMENT_FORMAT");
            }
            LAST_DOCUMENTNO = LAST_DOCUMENTNO + 1;
            sql_chk = @"update cmdocumentcontrol set LAST_DOCUMENTNO = {0} where DOCUMENT_CODE = '" + document_code + "' AND COOP_ID = {1} ";
            sql_chk = WebUtil.SQLFormat(sql_chk, LAST_DOCUMENTNO, state.SsCoopId);
            dt_chk = WebUtil.QuerySdt(sql_chk);
            if (document_code == "INVESTACCOUNT")
            {
                for (int i = Convert.ToString(LAST_DOCUMENTNO).Length; i < 10; i++)
                    zero = zero + "0";

               docno = zero + LAST_DOCUMENTNO;
            }
            else if (document_code == "INVESTDOCNO")
            {
                for (int i = Convert.ToString(LAST_DOCUMENTNO).Length; i < 6; i++)
                    zero = zero + "0";
                docno = DOCUMENT_PREFIX + DOCUMENT_YEAR.Substring(2, 2) + zero + LAST_DOCUMENTNO;
            }

                return docno;
        }
         #endregion

    }
}