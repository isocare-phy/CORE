using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Saving.Applications.shrlon.ws_sl_slip_pay_mth_ctrl.w_dlg_sl_kpmastreceive_ctrl
{
    public partial class DsList : DataSourceRepeater
    {
        public DataSet1.KPMASTRECEIVEDataTable DATA { get; set; }

        public void InitDs(PageWeb pw)
        {
            css1.Visible = false;
            
            DataSet1 ds = new DataSet1();
            this.DATA = ds.KPMASTRECEIVE;
            this.InitDataSource(pw, Repeater1, this.DATA, "dsList");
            this.EventClicked = "OnDsListClicked";
            this.EventItemChanged = "OnDsListItemChanged";
            this.Register();
        }
        public void RetrieveList(string member_no) {

            string sql = @"select * from (
                    select kptempreceive.recv_period,
		                    kptempreceive.receipt_no,
		                    kptempreceive.member_no,
		                    kptempreceive.operate_date,
		                    kptempreceive.receive_amt
                    from kptempreceive, kptempreceivedet
                    where kptempreceive.kpslip_no = kptempreceivedet.kpslip_no
                    and kptempreceive.coop_id = kptempreceivedet.coop_id
                    and (kptempreceive.keeping_status <> 1 or  kptempreceivedet.keepitem_status <> 1)
                    and kptempreceive.recv_period = (select max(kptempreceive.recv_period) from kptempreceive )
                    union
                    select kpmastreceive.recv_period,
		                    kpmastreceive.receipt_no,
		                    kpmastreceive.member_no,
		                    kpmastreceive.operate_date,
		                    kpmastreceive.receive_amt
                    from kpmastreceive, kpmastreceivedet
                    where kpmastreceive.kpslip_no = kpmastreceivedet.kpslip_no
                    and kpmastreceive.coop_id = kpmastreceivedet.coop_id
                    and (kpmastreceive.keeping_status <> 1 or  kpmastreceivedet.keepitem_status <> 1)
                    ) kpreceive
                    where kpreceive.member_no = {0} 
                    group by 
		                    kpreceive.recv_period,
		                    kpreceive.receipt_no,
		                    kpreceive.member_no,
		                    kpreceive.operate_date,
		                    kpreceive.receive_amt
                    order by kpreceive.receipt_no,kpreceive.member_no";
                sql = WebUtil.SQLFormat(sql, member_no);            
                DataTable dt = WebUtil.Query(sql);
                this.ImportData(dt);
        }
    }
}