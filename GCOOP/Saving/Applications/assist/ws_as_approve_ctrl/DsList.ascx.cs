using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;

namespace Saving.Applications.assist.ws_as_approve_ctrl
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
        public void RetrieveList(string sqlsearch, string sqlorder)
        {
            String sql = @"select 
	                        ft_getmbname( mb.coop_id, mb.member_no ) as mbname,
	                        mb.membgroup_code,
	                        req.assist_docno,   
	                        req.member_no,
	                        req.assisttype_code,
                            req.req_date,
	                        req.assistnet_amt,
	                        req.req_status,
	                        req.assisttype_code||':'||ast.assisttype_desc||' ('||uap.assistpay_desc||')' as assistpay_desc,
                            'ผู้รับทุน  :  '||req.ass_rcvname as ass_rcvname
                        from mbmembmaster mb
                            join assreqmaster req on req.member_no = mb.member_no
                            join assucfassisttype ast on req.assisttype_code = ast.assisttype_code
                            join assucfassisttypepay uap on req.assisttype_code = uap.assisttype_code and req.assistpay_code = uap.assistpay_code
                        where req.req_status = 8
                        and req.coop_id = {0}
                        " + sqlsearch + sqlorder;
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
            Int32 s_req = 0;
            decimal s_balance = 0;
            for (int i = 0; i < this.RowCount; i++)
            {
                s_req = this.RowCount;
                s_balance += this.DATA[i].ASSISTNET_AMT;
            }
            sum_req.InnerText = s_req.ToString("#,##0");
            sum_balance.InnerText = s_balance.ToString("N");
        }
    }
}