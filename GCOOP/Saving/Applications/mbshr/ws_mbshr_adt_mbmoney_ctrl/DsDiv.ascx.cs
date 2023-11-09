using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;

namespace Saving.Applications.mbshr.ws_mbshr_adt_mbmoney_ctrl
{
    public partial class DsDiv : DataSourceFormView
    {
        public DataSet1.MBMEMBMONEYTR_DIVDataTable DATA { get; set; }

        public void InitDsDiv(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.MBMEMBMONEYTR_DIV;
            this.EventItemChanged = "OnDsDivItemChanged";
            this.EventClicked = "OnDsDivClicked";
            this.InitDataSource(pw, FormView4, this.DATA, "dsDiv");
            this.Button.Add("b_bank");
            this.Button.Add("b_branch");
            this.Register();
        }
        public void RetrieveDiv(String member_no)
        {
            String sql = @"select moneytype_code,
		                        trim( bank_code ) as bank_code,
		                        trim(bank_branch) as bank_branch,
		                        bank_accid
                         from mbmembmoneytr 
                        where coop_id={0}   and
                        member_no={1} and
                        trtype_code ='DVAV1'";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, member_no);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
        public void RetrieveDDWType()
        {
            String sql = @"select methpaytype_code as moneytype_code 
                ,methpaytype_code||' - '||methpaytype_desc as methpaytype_desc
                ,1 as sorter  from   yrucfmethpay  where showlist_flag = 1
                union
                select '','',0 from dual order by sorter,methpaytype_desc ASC";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "moneytype_code", "methpaytype_desc", "moneytype_code");
        }
        public void DdBank()
        {
            string sql = @"
              SELECT trim(BANK_CODE) as BANK_CODE ,   
                     BANK_DESC,   
                     EDIT_FORMAT ,1 as sorter 
                FROM CMUCFBANK 
            union
            select '','','',0 from dual order by sorter,BANK_DESC ASC";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "bank_name", "BANK_DESC", "BANK_CODE");
        }
        public void DdBranch(string bank_code)
        {
            string sql = @"
                  SELECT BANK_CODE,   
                         trim(BRANCH_ID) as BRANCH_ID ,   
                         BRANCH_NAME,   
                         1 as sorter
                    FROM CMUCFBANKBRANCH
                   where trim(bank_code) = {0}
                    union 
                    select '','','',0 from dual order by sorter,  BRANCH_NAME ASC";
            sql = WebUtil.SQLFormat(sql, bank_code.Trim());
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "branch_name", "BRANCH_NAME", "BRANCH_ID");
        }
    }
}