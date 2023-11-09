using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Saving.Applications.shrlon.ws_sl_slip_pay_ctrl
{
    public partial class DsDetailAdjust : DataSourceRepeater
    {
        public DataSet1.DT_List_AdjustDataTable DATA { get; set; }
        public void InitDsDetailAdjust(PageWeb pw)
        {
            css1.Visible = false;
            css2.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.DT_List_Adjust;
            this.EventItemChanged = "OnDsDetailAdjustItemChanged";
            this.EventClicked = "OnDsDetailAdjustClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsDetailAdjust");
            this.Register();
        }
        public void RetrieveDetailAdjust(string member_no)
        {
            String sql = @"select 0 as operate_flag , sa.coop_id , sa.adjslip_no , kmr.receipt_no , 
                            sa.adjslip_date , sa.ref_recvperiod , sa.slip_amt , sa.cancel_id , 
                            sa.slip_status , sa.ref_slipdate , sa.entry_id
                            from slslipadjust sa , kpmastreceive kmr 
                            where sa.coop_id = kmr.coop_id 
                            and sa.ref_slipno = kmr.kpslip_no 
                            and sa.pmx_status = 0
                            and sa.slip_status = 1
                            and sa.slip_amt = kmr.receive_amt
                            and sa.coop_id = {0}
                            and sa.member_no = {1}";

            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, member_no);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
    }
}