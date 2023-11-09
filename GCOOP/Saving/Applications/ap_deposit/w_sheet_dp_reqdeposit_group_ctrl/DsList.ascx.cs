using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;

namespace Saving.Applications.ap_deposit.w_sheet_dp_reqdeposit_group_ctrl
{
    public partial class DsList : DataSourceRepeater
    {
        public DataSet1.DPREQDEPOSITDataTable DATA { get; set; }
        public void InitDsList(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.DPREQDEPOSIT;
            this.EventItemChanged = "OnDsListItemChanged";
            this.EventClicked = "OnDsListClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsList");
            this.Register();
        }

        public void RetrieveData(string coop_id, DateTime workdate)
        {
            string sql = @"select deptrequest_docno,member_no,deptaccount_name,deptrequest_amt,depttype_code 
            from dpreqdeposit where coop_id={0} and approve_flag=0 and recppaytype_code='IIS'
            and deptopen_date={1} order by deptrequest_docno";
            sql = WebUtil.SQLFormat(sql, coop_id, workdate);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
    }
}