using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Saving.Applications.assist.dlg.ws_dlg_as_assistpay_ctrl
{
    public partial class DsMain : DataSourceFormView
    {
        public DataSet1.ASSREQMASTERDataTable DATA { get; set; }
        public void InitDsMain(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.ASSREQMASTER;
            this.EventItemChanged = "OnDsMainItemChanged";
            this.EventClicked = "OnDsMainClicked";
            this.InitDataSource(pw, FormView2, this.DATA, "dsMain");
            this.Button.Add("b_searchdeptno");
            //this.Button.Add("b_contsearch");
            this.Register();
        }

        public void RetrieveDetail(String ls_assistdocno)
        {
            string sql = @"
                         select 
	                        mbucfprename.prename_desc ||mbmembmaster.memb_name || '  ' || mbmembmaster.memb_surname as full_name,
	                        mbucfmembgroup.membgroup_desc,
	                        mbmembmaster.membgroup_code,
	                        assucfassisttype.assisttype_code,                            
	                        assucfassisttype.assisttype_code  || ':' || assucfassisttype.assisttype_desc assisttype_desc,	
	                        assucfassisttype.stm_flag,
	                        mbucfmembtype.membtype_code || ':' || mbucfmembtype.membtype_desc  as member_type,
	                        assreqmaster.assist_docno,   
	                        assreqmaster.member_no,
	                        assreqmaster.approve_amt,
	                        assreqmaster.assist_amt,
	                        assreqmaster.req_status,
	                        assreqmaster.assistpay_code,
	                        assreqmaster.assistpay_code || ':' || assucfassistpaytype.assistpay_desc assistpay_desc,
	                        assreqmaster.moneytype_code,
	                        assreqmaster.expense_accid,
	                        assreqmaster.expense_bank,
	                        assreqmaster.DEPTACCOUNT_NO,
	                        assreqmaster.DEPTACCOUNT_NO as r_deptno,
	                        assreqmaster.send_system,
	                        assreqmaster.expense_branch,                            					 
	                        MBMEMBMASTER.EXPENSE_CODE as MEXPENSE_CODE,   
	                        MBMEMBMASTER.EXPENSE_BANK AS MEXPENSE_BANK,   
	                        MBMEMBMASTER.EXPENSE_BRANCH AS MEXPENSE_BRANCH,   
	                        MBMEMBMASTER.EXPENSE_ACCID  AS MEXPENSE_ACCID
                        from mbmembmaster
                        inner join mbucfprename on mbmembmaster.prename_code = mbucfprename.prename_code
                        inner join mbucfmembgroup on mbmembmaster.membgroup_code = mbucfmembgroup.membgroup_code
                        inner join assreqmaster on assreqmaster.member_no = mbmembmaster.member_no
                        inner join assucfassisttype on assreqmaster.assisttype_code = assucfassisttype.assisttype_code
                        inner join assucfassistpaytype on assreqmaster.assistpay_code = assucfassistpaytype.assistpay_code
                        inner join mbucfmembtype  on mbucfmembtype.membtype_code = mbmembmaster.membtype_code
                        where assreqmaster.req_status = 1  and  
                        assreqmaster.coop_id = {0} and 
                        assreqmaster.assist_docno = {1}
                         ";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, ls_assistdocno);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
        public void DdMoneyType()
        {
            string sql = @" 
                   SELECT MONEYTYPE_CODE,  
                          MONEYTYPE_GROUP, 
                          MONEYTYPE_DESC,   
                          SORT_ORDER  ,
                          MONEYTYPE_CODE || ' - '|| MONEYTYPE_DESC as MONEYTYPE_DISPLAY
                     FROM CMUCFMONEYTYPE WHERE   MONEYTYPE_CODE in ('CSH','CHQ','CBT','TRN')  order by sort_order
            ";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "MONEYTYPE_CODE", "MONEYTYPE_DISPLAY", "MONEYTYPE_CODE");
        }       
                    
        public void DdBankDesc()
        {
            string sql = @"
            select  bank_code,bank_code|| ' '||bank_desc  as bank_desc, 1 as sorter from cmucfbank 
            union 
            select '','', 0 from dual order by sorter, bank_desc
            ";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "expense_bank", "bank_desc", "bank_code");
        }
        public void DdBranch(string bank_code)
        {
            string sql = @"
            select bank_code,branch_id, branch_id||'-'|| branch_name branch_name, 1 as sorter from cmucfbankbranch where bank_code = {0} 
            union
            select '', '','', 0 from dual order by sorter, branch_id
            ";
            sql = WebUtil.SQLFormat(sql, bank_code);
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "expense_branch", "branch_name", "branch_id");
        }

        public void DdFromAccId()
        {
            string sql = @"SELECT  
                            ACCMASTER.ACCOUNT_ID,    
                            ACCMASTER.ACCOUNT_ID ||'-'||ACCMASTER.ACCOUNT_NAME AS fromacc_display,
                            ACCMASTER.ACCOUNT_NAME  ,  
                            1 as sorter
                        FROM ACCMASTER  WHERE ACCOUNT_LEVEL=4
                        union
                        select '', '-----กรุณาเลือก---','', 0 from dual order by sorter, ACCOUNT_ID
                ";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "tofrom_accid", "fromacc_display", "ACCOUNT_ID");
        }

    }
}