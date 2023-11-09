using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Saving.Applications.mis.w_sheet_pm_estsumday_ctrl
{
    public partial class DsMain : DataSourceFormView
    {
        public DataSet1.DataTable1DataTable DATA { get; set; }

        public void InitDsMain(PageWeb pw)
        {
            css1.Visible = false;
            css2.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.DataTable1;
            this.EventItemChanged = "OnDsMainItemChanged";
            this.EventClicked = "OnDsMainClicked";
            this.InitDataSource(pw, FormView1, this.DATA, "dsMain");
            //this.Button.Add("b_search");
            //this.Button.Add("b_show");    
            this.Register();
        }

        public void retrieveddp()
        {
            string sql = @"select investtype_code , investtype_desc , 1 as sorter 
                        from pmucfinvest_type  
                        where coop_id = '" + state.SsCoopId + @"' 
                        union
                        select '','ทั้งหมด', 0 from dual order by sorter, investtype_code";

            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "dropdown", "investtype_desc", "investtype_code");
        }
    }
}