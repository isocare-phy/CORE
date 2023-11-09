using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Saving.Applications.shrlon.ws_sl_slip_pay_class_ctrl
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
            String sql = @"select
                        0 as operate_flag ,slslipadjust.adjslip_no , kpmastreceive.receipt_no , 
                        slslipadjust.adjslip_date , slslipadjust.ref_recvperiod , sum(slslipadjustclass.item_adjamt) as slip_amt ,
                        slslipadjust.entry_id,slslipadjust.ref_slipdate
                        from slslipadjustclass 
                        left join slslipadjust  on  slslipadjust.adjslip_no = slslipadjustclass.adjslip_no 
                        left join kpmastreceive on  slslipadjust.ref_slipno = kpmastreceive.kpslip_no 
                        where slslipadjustclass.item_adjamt >= 1
                        and slslipadjustclass.coop_id = {0}
                        and  slslipadjust.member_no = {1}
                        group by slslipadjust.adjslip_no , kpmastreceive.receipt_no , slslipadjust.adjslip_date , slslipadjust.ref_recvperiod ,slslipadjust.entry_id,slslipadjust.ref_slipdate
                        order by slslipadjust.ref_recvperiod ";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, member_no);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
    }
}