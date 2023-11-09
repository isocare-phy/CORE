using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Saving.Applications.assist.ws_as_assistbegin_ctrl
{
    public partial class DsList : DataSourceRepeater
    {
        public DataSet1.ASSSUMLEDGERYEARDataTable DATA { get; set; }
        public void InitDsList(PageWeb pw)
        {
            css1.Visible = false;
            css2.Visible = false;

            DataSet1 ds = new DataSet1();
            this.DATA = ds.ASSSUMLEDGERYEAR;
            this.EventItemChanged = "OnDsListItemChanged";
            this.EventClicked = "OnDsListClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsList");
            this.Register();

        }
        public void RetrieveList(string coop_id, int assist_year)
        {
            string sql = @"select assucfassisttype.assisttype_code,
		                    assucfassisttype.assisttype_desc,
		                    (select  nvl( sum(asssumledgeryear.begin_amount),0) from  asssumledgeryear    where  asssumledgeryear.assist_year = {1}
                    and  asssumledgeryear.coop_id = assucfassisttype.coop_id and  asssumledgeryear.assisttype_code = assucfassisttype.assisttype_code ) as begin_amount 
                    from assucfassisttype 
                    where assucfassisttype.coop_id = {0}
                    order by  assucfassisttype.assisttype_code";
            sql = WebUtil.SQLFormat(sql, state.SsCoopId, assist_year);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);

        }

    }
}