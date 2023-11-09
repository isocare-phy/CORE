using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;

namespace Saving.Applications.assist.ws_sheet_as_assistpaygroup_ctrl
{
    public partial class DsMain : DataSourceFormView
    {
        public DataSet1.DT_MAINDataTable DATA { get; set; }
        public void InitDsMain(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.DT_MAIN;
            this.EventItemChanged = "OnDsMainItemChanged";
            this.EventClicked = "OnDsMainClicked";
            this.InitDataSource(pw, FormView1, this.DATA, "dsMain");
            this.Button.Add("b_search");
            this.Button.Add("b_pay");
            this.Button.Add("b_searchmem");
            this.Register();
        }

        public void DDAssisttype()
        {
            string sql = @" select 
	                        assucfassisttype.assisttype_code,   
                            assucfassisttype.assisttype_code || ' : ' || assucfassisttype.assisttype_desc as fulltype_desc  ,
	                        1 as sorter
                        from assucfassisttype
                        where assucfassisttype.coop_id = {0}
                        union
                        select '','',0 from dual order by sorter,assisttype_code";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl);
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "assisttype_code", "fulltype_desc", "assisttype_code");
        }
        public void DdMoneyType()
        {
            string sql = @" 
                   SELECT MONEYTYPE_CODE,  
                          MONEYTYPE_GROUP, 
                          MONEYTYPE_DESC,   
                          SORT_ORDER  ,
                          MONEYTYPE_CODE || ' - '|| MONEYTYPE_DESC as MONEYTYPE_DISPLAY
                     FROM CMUCFMONEYTYPE WHERE  MONEYTYPE_GROUP IN('BNK','CHQ','CSH') order by sort_order
            ";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "moneytype_code", "MONEYTYPE_DISPLAY", "MONEYTYPE_CODE");
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
    }
}