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
    public partial class wd_statement : DataSourceRepeater
    {
        public DataSet1.LFCONTSTATEMENTDataTable DATA { get; set; }
        public void InitStatement(PageWeb pw)
        {
            css1.Visible = false;
            css2.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.LFCONTSTATEMENT;
            this.EventItemChanged = "OnStatementItemChanged";
            this.EventClicked = "OnStatementClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "wd_statement");
            this.Button.Add("b_delete");

            this.Register();
        }
        public void RetrieveStatement(string ls_acc_no)
        {
            string sql = @"select  
                        LFCONTSTATEMENT.coop_control,
                        LFCONTSTATEMENT.coop_id,
                        LFCONTSTATEMENT.loancontract_no,
                        LFCONTSTATEMENT.seq_no,
                        LFCONTSTATEMENT.recv_period,
                        LFCONTSTATEMENT.sliptype_code,
                        LFUCFITEMTYPE.sliptype_desc,
                        LFCONTSTATEMENT.principal_payment,
                        LFCONTSTATEMENT.interest_payment,
                        LFCONTSTATEMENT.item_payment,
                        LFCONTSTATEMENT.calint_from,
                        LFCONTSTATEMENT.calint_to,
                        LFCONTSTATEMENT.interest_period,
                        LFCONTSTATEMENT.principal_arrear,
                        LFCONTSTATEMENT.interest_arrear,
                        LFCONTSTATEMENT.item_status
                        from LFCONTSTATEMENT , LFUCFITEMTYPE
                        where LFCONTSTATEMENT.sliptype_code = LFUCFITEMTYPE.sliptype_code
                        and LFCONTSTATEMENT.loancontract_no = {0}
                        order by seq_no asc";
            sql = WebUtil.SQLFormat(sql, ls_acc_no);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
          
        }
        public void DdSlipitemtype()
        {
            string sql = @"
                SELECT SLIPTYPE_CODE,   
                     SLIPTYPE_DESC,   
                     SLIPTYPE_FLAG  ,1 as sorter
                FROM LFUCFITEMTYPE
                union 
                select '','',0,0 from dual order by sorter,SLIPTYPE_CODE ASC";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "SLIPTYPE_CODE", "SLIPTYPE_CODE", "SLIPTYPE_CODE");
        }
    }
}