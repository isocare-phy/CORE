using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;
namespace Saving.Applications.assist.ws_sheet_as_cancelpay_ctrl
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
					      assslippayout.member_no,
	                      mbucfprename.prename_desc ||mbmembmaster.memb_name || '  ' || mbmembmaster.memb_surname full_name,
	                      mbmembmaster.membgroup_code,
						  assslippayout.assistslip_no,
						  assslippayout.slip_date,
	                      assslippayout.payout_amt,
						  assslippayout.ref_reqdocno,
                          assslippayout.pay_period,
						  assucfassisttype.assisttype_code,
	                      assucfassisttype.stm_flag					                      
                        from mbmembmaster
                        inner join mbucfprename on mbmembmaster.prename_code = mbucfprename.prename_code
                        inner join assslippayout on assslippayout.member_no = mbmembmaster.member_no
                        inner join assucfassisttype on assslippayout.assisttype_code = assucfassisttype.assisttype_code
                        where assslippayout.slip_status =  1 
                        and assslippayout.coop_id = {0}
                        " + sqlsearch + @"
                        order by assucfassisttype.assisttype_code, assslippayout.assistslip_no";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
    }
}