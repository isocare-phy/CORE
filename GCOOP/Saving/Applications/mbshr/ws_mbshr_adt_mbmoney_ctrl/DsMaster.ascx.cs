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
    public partial class DsMaster :  DataSourceFormView
    {
        public DataSet1.MBMEMBMASTERDataTable DATA { get; set; }

        public void InitDsMaster(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.MBMEMBMASTER;
            this.EventItemChanged = "OnDsMasterItemChanged";
            this.EventClicked = "OnDsMasterClicked";
            this.InitDataSource(pw, FormView2, this.DATA, "dsMaster");
            this.Button.Add("b_bank");
            this.Button.Add("b_branch");
            this.Register();
        }
        public void RetrieveMaster(String member_no)
        {
            String sql = @"SELECT   MBMEMBMASTER.MEMBER_NO,
                        MBMEMBMASTER.EXPENSE_CODE,   
                        trim(MBMEMBMASTER.EXPENSE_BANK) as expense_bank,   
                        trim(MBMEMBMASTER.EXPENSE_BRANCH) as EXPENSE_BRANCH,   
                        MBMEMBMASTER.EXPENSE_ACCID as EXPENSE_ACCID
                        FROM MBMEMBMASTER
                        WHERE  
                        ( ( MBMEMBMASTER.COOP_ID = {0}) AND  
                        ( MBMEMBMASTER.MEMBER_NO ={1}) )   ";

            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, member_no);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);            
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