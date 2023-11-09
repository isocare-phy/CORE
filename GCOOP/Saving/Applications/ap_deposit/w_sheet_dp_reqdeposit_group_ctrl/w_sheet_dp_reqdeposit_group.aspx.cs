using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using DataLibrary;

namespace Saving.Applications.ap_deposit.w_sheet_dp_reqdeposit_group_ctrl
{
    public partial class w_sheet_dp_reqdeposit_group : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public string JsPosSave { get; set; }

        public void InitJsPostBack()
        {
            dsMain.InitDsMain(this);
            dsList.InitDsList(this);
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)//show data first  
            {
                dsMain.DATA[0].all_check = "0";
                dsMain.DATA[0].operate_date = state.SsWorkDate;
                dsList.RetrieveData(state.SsCoopId,state.SsWorkDate);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == JsPosSave)
            {
                SaveData();
            }
        }
        private void SaveData() {
            decimal ld_reqamt = 0;
            string ls_memno = "", ls_deptname = "", ls_accno = "", ls_deptnameCon = "", ls_passbook = "", ls_defaultaccid = "", ls_depttype = "", ls_requestdocno="";
            try
            {
                
                for (int ii = 0; ii < dsList.RowCount; ii++)
                {
                    if (dsList.DATA[ii].choose_flag == 1)
                    {
                        Sta ta = new Sta(state.SsConnectionString);
                        ta.Transection();
                        try
                        {                            
                            ls_depttype = dsList.DATA[ii].DEPTTYPE_CODE;
                            ls_accno = wcf.NCommon.of_getnewdocno(state.SsWsPass, state.SsCoopId, "DPACCDOCNO" + ls_depttype);
                            ls_memno = dsList.DATA[ii].MEMBER_NO;
                            ld_reqamt = dsList.DATA[ii].DEPTREQUEST_AMT;
                            ls_deptname = dsList.DATA[ii].DEPTACCOUNT_NAME;                            
                            ls_deptnameCon = ls_deptname + " เพียงผู้เดียว";

                            //select passbook 
                            string sql_passbook = @"SELECT min(DPDEPTBOOKHIS.BOOK_NO) as passbook FROM DPDEPTBOOKHIS,DPDEPTTYPE WHERE ( DPDEPTBOOKHIS.BOOK_GRP = DPDEPTTYPE.BOOK_GROUP ) and ( DPDEPTBOOKHIS.BOOK_STATUS = 8 ) AND DPDEPTTYPE.DEPTTYPE_CODE ='" + ls_depttype + "' and DPDEPTBOOKHIS.coop_id='" + state.SsCoopId + "' ";
                            Sdt dt_passbook = ta.Query(sql_passbook);
                            ls_passbook = dt_passbook.Rows[0]["passbook"].ToString();

                            //insert dpdeptmaster
                            String sql_master = "insert into dpdeptmaster               " +
                            "   (deptaccount_no,                      depttype_code,	                     member_no, " +
                            "     deptopen_date,			       deptclose_status,			        deptclose_date, " +
                            "  deptaccount_name,	        	     dept_objective,				   deptpassbook_no, " +
                            "   condforwithdraw,			             f_tax_rate,				  deptmonth_status, " +
                            "     deptmonth_amt,			      deptmonth_amt_old,			                remark, " +
                            "   monthint_status,			       monthintpay_meth,			       tran_deptacc_no, " +
                            "   tran_bankacc_no,			     spcint_rate_status,			           spcint_rate, " +
                            "          beginbal,						    prncbal,					 checkpend_amt, " +
                            "    sequest_status,				     sequest_amount,				  withdrawable_amt, " +
                            "   lastcalint_date,				        accuint_amt,					 intarrear_amt, " +
                            "    accuintpay_amt,			         accutaxpay_amt,				    withdraw_count, " +
                            "  reset_count_date,			          laststmseq_no,					 lastrec_no_pb, " +
                            "    lastpage_no_pb,			         lastline_no_pb,				 checkbook_code_pb, " +
                            "   lastrec_no_card,			       lastpage_no_card,				  lastline_no_card, " +
                            "     closemonth_no,			           doperate_bal,				   lastaccess_date, " +
                            "           prnc_no,					membgroup_code ,			           bank_branch, " +
                            " dept_tranacc_name,		                  bank_code,					  acccont_type, " +
                            "           COOP_ID,					    check_digit,				 lastmovement_date, " +
                            "    prnbook_status,			             memcoop_id,		    	 taxspcrate_status, " +
                            "    deptint_remain,			           book_balance,		 		 deptaccount_ename, " +
                            "     booknorm_flag,				bookconfirm_status	)  " +
                            " VALUES  " +
                            "(	'" + ls_accno + "',                 '" + ls_depttype + "',                  '" + ls_memno + "', " +
                            "to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy") + "', 'dd/mm/yyyy'),'0',        null, " +
                            "'" + ls_deptname + "',	                           null,	            '" + ls_passbook + "', " +
                            "'" + ls_deptnameCon + "',                   '0',                                '0', " +
                            "               '0',                               '0',                               null, " +
                            "               '0',							   '0',			                      null, " +
                            "              null,	                           '0',								   '0', " +
                            "               '0',			'" + ld_reqamt + "',							       '0', " +
                            "               '0',							   '0',				'" + ld_reqamt + "', " +
                            "to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy") + "', 'dd/mm/yyyy'),		'0',   '0', " +
                            "               '0',						       '0',							       '0', " +
                            "              null,							   '1',							       '0', " +
                            "               '0',							   '0',							   	  null, " +
                            "               '0',							   '0',							       '0', " +
                            "               '0',							   '0',to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy") + "', 'dd/mm/yyyy'),   " +
                            "               '0',							  null,								  null, " +
                            "              null,		                      null,			                      '01', " +
                            "'" + state.SsCoopId + "',	                       '0',to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy") + "', 'dd/mm/yyyy'),    " +
                            "               '1',          '" + state.SsCoopId + "',				                   '0', " +
                            "               '0',		                       '0',		                          null, " +
                            "               '1',                               '1' )";
                            ta.Exe(sql_master);

                            //select document ฟังชั่นนี้เป็นการอัพเดท  docno แล้ว
                            string deptslipno = wcf.NCommon.of_getnewdocno(state.SsWsPass, state.SsCoopId, "DPSLIPNO");

                            //insert dpdeptstatement
                            String sql_stament = "insert into dpdeptstatement                 " +
                            "(     coop_id,             deptaccount_no,              seq_no,     deptitemtype_code,     operate_date,        deptitem_amt,  " +
                            "balance_forward,                prncbal,               prnc_no,              chrg_amt,          tax_amt,             int_amt,  " +
                            "  accuint_amt,                 retint_amt,         item_status,        prntopb_status,         entry_id,          entry_date,  " +
                            "  calint_from,                  calint_to,        authorize_id,          no_book_flag,   cheque_pending,       forprnbk_flag,  " +
                            "   transec_no,                calint_flag,         deptitem_adj,           deptcoop_id,     deptslip_no,        operate_time)    " +
                            "VALUES " +
                            "('" + state.SsCoopId + "','" + ls_accno + "',                 '1',                'OTR',to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy") + "', 'dd/mm/yyyy'),'" + ld_reqamt + "', " +
                            "          '0',     '" + ld_reqamt + "',                 '0',                  '0',              '0',                    '0', " +
                            "          '0',                        '0',                 '1',                  '0','" + state.SsUsername + "',to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy") + "', 'dd/mm/yyyy'), " +
                            "to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy") + "', 'dd/mm/yyyy'),to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy") + "', 'dd/mm/yyyy'), '" + state.SsUsername + "','0','0','1', " +
                            "          '0',                        '1',                 '0','" + state.SsCoopId + "',   '" + deptslipno + "' ,to_date('" + DateTime.Now.ToString("dd/MM/yyyy") + "', 'dd/mm/yyyy')) ";
                            ta.Exe(sql_stament);


                            //select passbook 
                            string sql_ucfrecppaytype = @"SELECT default_accid FROM dpucfrecppaytype  WHERE ( recppaytype_code = 'OTR' ) and ( COOP_ID = '" + state.SsCoopControl + "') ";
                            Sdt dt_ucfrecppaytype = ta.Query(sql_ucfrecppaytype);
                            ls_defaultaccid = dt_ucfrecppaytype.Rows[0]["default_accid"].ToString();

                            //insert deptslip                     
                            String sql_depslip = "insert into dpdeptslip   " +
                                    "(coop_id, deptslip_no, deptcoop_id,deptgroup_code, deptaccount_no, depttype_code, deptslip_date, recppaytype_code, deptslip_amt,deptslip_netamt, cash_type, prncbal, withdrawable_amt, checkpend_amt, loangarantee_amt, accuint_amt, fee_amt, preprncbal, preaccuint_amt, entry_id, entry_date, intarrear_amt, dpstm_no, deptitemtype_code, calint_from, calint_to, int_amt, int_return, item_status, closeday_status, other_amt, count_wtd, machine_id, nobook_flag, int_curyear, tofrom_accid, posttovc_flag, tax_amt, int_bfyear, accid_flag, showfor_dept)    " +
                                    "values     " +
                                    "('" + state.SsCoopId + "','" + deptslipno + "','" + state.SsCoopId + "','00', '" + ls_accno + "','" + ls_depttype + "',to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy") + "','dd/mm/yyyy'), 'OTR','" + ld_reqamt + "','" + ld_reqamt + "', 'TRN', '" + ld_reqamt + "', '" + ld_reqamt + "', 0, 0, 0, 0, 0, 0,'" + state.SsUsername + "', to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy") + "','dd/mm/yyyy'), 0, '1', 'OTR', to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy") + "','dd/mm/yyyy'), to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy") + "','dd/mm/yyyy'), 0, 0, 1, 0, 0, 0,'" + state.SsUsername + "', 0, 0,'" + ls_defaultaccid + "', 0, 0, 0, 0, 1) ";
                            ta.Exe(sql_depslip);
                            
                            //passbook
                            String update_sql = "update DPDEPTBOOKHIS set BOOK_STATUS =1 where BOOK_NO ='" + ls_passbook + "' ";
                            Sdt sqlupdate = WebUtil.QuerySdt(update_sql);
                            //dpreqdeposit
                            ls_requestdocno = dsList.DATA[ii].DEPTREQUEST_DOCNO;
                            update_sql = "update dpreqdeposit set deptaccount_no='" + ls_accno + "',approve_flag = 1 ,entry_id='" + state.SsUsername + "',acccont_type='01',condforwithdraw='" + ls_deptnameCon + "' where deptrequest_docno ='" + ls_requestdocno + "' and approve_flag = 0 and coop_id='" + state.SsCoopId + "'";
                            sqlupdate = WebUtil.QuerySdt(update_sql);

                            ta.Commit();
                            ta.Close();
                        }
                        catch (Exception ex)
                        {
                            ta.RollBack();
                            ta.Close();
                            String updateDMDeptNo = "update cmdocumentcontrol set last_documentno = last_documentno-1 where document_code = 'DPACCDOCNO" + ls_depttype + "' and coop_id = '" + state.SsCoopControl + "' ";
                            Sdt sqlCmDeptNo = WebUtil.QuerySdt(updateDMDeptNo);
                            LtServerMessage.Text = WebUtil.ErrorMessage("บันทึกไม่สำเร็จ " + ex);
                        }
                    }
                }                
                
                dsList.ResetRow();
                dsList.RetrieveData(state.SsCoopId, state.SsWorkDate);
                dsMain.DATA[0].all_check = "0";
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("บันทึกไม่สำเร็จ");
            }
        }
        public void SaveWebSheet()
        {
            
        }

        public void WebSheetLoadEnd()
        {
            throw new NotImplementedException();
        }
    }
}