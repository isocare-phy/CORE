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
    public partial class wd_detail : DataSourceFormView
    {
        public DataSet1.LFCONTMASTERDataTable DATA { get; set; }
        public void InitDetail(PageWeb pw)
        {
            css1.Visible = false;
            css2.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.LFCONTMASTER;
            this.EventItemChanged = "OnDetailItemChanged";
            this.EventClicked = "OnDetailClicked";
            this.InitDataSource(pw, FormView1, this.DATA, "wd_detail");
 
            this.Register();
        }

        public void RetrieveDetail(string ls_acc_no)
        {
            string sql = @"select 
                        coop_control,
                        coop_id,
                        loancontract_no,
                        member_no,
                        loantype_code,
                        loanapprove_date,
                        principal_balance,
                        period_payment,
                        period_payamt,
                        lastcalint_date,
                        principal_arrear,
                        interest_arrear,
                        last_stm_no,
                        contract_status 
                        from LFCONTMASTER
                        where loancontract_no = {0}";
            sql = WebUtil.SQLFormat(sql, ls_acc_no);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
    }
}