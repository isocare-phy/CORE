using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Saving.Applications.deposit_const.w_sheet_dp_const_sum_booktyp_ctrl
{
    public partial class DsMain : DataSourceRepeater
    {
        public DataSet1.DPUCFBOOKCONSTDataTable DATA { get; set; }

        public void InitDsMain(PageWeb pw)
        {
            css1.Visible = false;
          

            DataSet1 ds = new DataSet1();
            this.DATA = ds.DPUCFBOOKCONST;
            this.EventItemChanged = "OnDsMainItemChanged";
            this.EventClicked = "OnDsMainClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsMain");
            this.Button.Add("B_DEL");
            this.Register();
        }

        public void retrieve() {
            String re = @"SELECT 
                            dpb.BOOK_TYPE,
                            dpb.BOOK_GRP,
                            dpb.BOOK_DESC,
                            count(dph.BOOK_no) as  book

                            FROM DPUCFBOOKCONST dpb 
                            inner join dpdeptbookhis dph on dph.coop_id = dpb.coop_id and dph.book_grp = dpb.book_grp
                            where dph.book_status = 8 
                            group by dpb.BOOK_TYPE,
                            dpb.BOOK_GRP,
                            dpb.BOOK_DESC,
                            dpb.COOP_ID
                            ORDER BY dpb.book_grp";
            DataTable dt = WebUtil.Query(re);
            this.ImportData(dt);
        }
    }
}