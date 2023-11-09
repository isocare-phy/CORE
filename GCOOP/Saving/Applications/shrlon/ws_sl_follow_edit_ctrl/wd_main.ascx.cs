using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Saving.Applications.shrlon.ws_sl_follow_edit_ctrl
{
    public partial class wd_main : DataSourceFormView
    {
        public DataSet1.DT_MAINDataTable DATA { get; set; }
        public void InitMain(PageWeb pw)
        {
            css1.Visible = false;
            css2.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.DT_MAIN;
            this.EventItemChanged = "OnMainItemChanged";
            this.EventClicked = "OnMainClicked";
            this.InitDataSource(pw, FormView1, this.DATA, "wd_main");
            this.Button.Add("b_search");
            this.Button.Add("b_process");
            this.Register();
        }
        public void RetrieveMain(string ls_member_no)
        {
            string sql = @"
                  SELECT MBMEMBMASTER.MEMBER_NO,   
                         MBUCFPRENAME.PRENAME_DESC,   
                         MBMEMBMASTER.MEMB_NAME,   
                         MBMEMBMASTER.MEMB_SURNAME,   
                         trim(MBMEMBMASTER.MEMBGROUP_CODE) as MEMBGROUP_CODE,   
                         MBUCFMEMBGROUP.MEMBGROUP_DESC,   
                         MBMEMBMASTER.MEMBTYPE_CODE,   
                         MBUCFMEMBTYPE.MEMBTYPE_DESC,   
                         MBMEMBMASTER.ACCUM_INTEREST,   
                         MBMEMBMASTER.MEMBER_STATUS,   
                         MBMEMBMASTER.MEMBER_DATE
                    FROM MBMEMBMASTER,   
                         MBUCFMEMBGROUP,   
                         MBUCFMEMBTYPE,   
                         MBUCFPRENAME
                   WHERE ( trim(MBUCFMEMBGROUP.MEMBGROUP_CODE) = trim(MBMEMBMASTER.MEMBGROUP_CODE) ) and  
                         ( MBUCFMEMBTYPE.MEMBTYPE_CODE = MBMEMBMASTER.MEMBTYPE_CODE ) and  
                         ( MBUCFPRENAME.PRENAME_CODE = MBMEMBMASTER.PRENAME_CODE ) and  
                         ( (mbmembmaster.coop_id= {0} )and
                           ( mbmembmaster.member_no = {1} ))   ";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, ls_member_no);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }

        public void DdLoancontract()
        {
            string sql = @"
                SELECT LOANCONTRACT_NO ,1 as sorter
                FROM LFCONTMASTER
                where coop_id = {0} and member_no = {1} 
                union 
                select '',0 from dual order by sorter,LOANCONTRACT_NO ASC";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, this.DATA[0].MEMBER_NO);
            this.DropDownDataBind(sql, "LOANCONTRACT_NO", "LOANCONTRACT_NO", "LOANCONTRACT_NO");
        }      
    }
}