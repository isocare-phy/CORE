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
    public partial class DsPayout : DataSourceRepeater
    {
        public DataSet1.DataDsPayoutDataTable DATA { get; private set; }
        public void InitDsPayout(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.DataDsPayout;
            this.InitDataSource(pw, Repeater1, this.DATA, "dsPayout");
            this.Register();
        }
        public void RetrieveData(string ls_memno,string ls_assistcode)
        {
            string sql = @"select a.slip_date,a.payout_amt,a.ref_reqdocno,a.entry_id from assslippayout a
                        where a.coop_id={0} and a.member_no={1} and a.assisttype_code={2} and a.slip_status=1
                        ";
            sql = WebUtil.SQLFormat(sql, state.SsCoopId, ls_memno, ls_assistcode);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
    }
}