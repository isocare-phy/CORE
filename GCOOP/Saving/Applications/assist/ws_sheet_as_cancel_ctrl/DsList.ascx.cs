using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;

namespace Saving.Applications.assist.ws_sheet_as_cancel_ctrl
{
    public partial class DsList : DataSourceRepeater
    {
        public DataSet1.DT_LISTDataTable DATA { get; set; }

        public void InitDsList(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.DT_LIST;
            this.EventItemChanged = "OnDsDsListItemChanged";
            this.EventClicked = "OnDsListClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsList");

            this.Register();
        }
        public void RetrieveList(string sqlsearch)
        {
            String sql = @"select 
	                        mbucfprename.prename_desc ||mbmembmaster.memb_name || '  ' || mbmembmaster.memb_surname full_name,
	                        mbmembmaster.membgroup_code,
	                        assreqmaster.assist_docno,   
	                        assreqmaster.member_no,
	                        assreqmaster.assisttype_code,
	                        assreqmaster.approve_amt,
	                        assreqmaster.req_status,
	                        assreqmaster.assistpay_code || ':' || assucfassistpaytype.assistpay_desc assistpay_code
                        from mbmembmaster
                        inner join mbucfprename on mbmembmaster.prename_code = mbucfprename.prename_code
                        inner join assreqmaster on assreqmaster.member_no = mbmembmaster.member_no
                        inner join assucfassisttype on assreqmaster.assisttype_code = assucfassisttype.assisttype_code
                        inner join assucfassistpaytype on assreqmaster.assistpay_code = assucfassistpaytype.assistpay_code
                        where assreqmaster.req_status = 1  and  
                        assreqmaster.coop_id = {0}
                        " + sqlsearch + @"
                        order by assreqmaster.assisttype_code, assreqmaster.assist_docno";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
    }
}