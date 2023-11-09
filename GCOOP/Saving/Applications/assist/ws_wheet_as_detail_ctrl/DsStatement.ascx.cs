using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;

namespace Saving.Applications.assist.ws_wheet_as_detail_ctrl
{
    public partial class DsStatement : DataSourceRepeater
    {
        public DataSet1.DataDsStateDataTable DATA { get; private set; }
        public void InitDsStatement(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.DataDsState;
            this.EventItemChanged = "OnDsStateItemChanged";
            this.EventClicked = "OnDsStateClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsStatement");
            this.Register();
        }
        public void RetrieveData(string ls_contractno)
        {
            string sql = @"SELECT a.seq_no,a.slip_date,a.pay_balance,c.moneytype_desc,a.entry_id  from asscontstatement a inner join cmucfmoneytype c on a.moneytype_code=c.moneytype_code
                        where a.coop_id={0} and a.asscontract_no ={1} and a.item_status=1
                        order by a.seq_no ";
            sql = WebUtil.SQLFormat(sql, state.SsCoopId, ls_contractno);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
    }
}