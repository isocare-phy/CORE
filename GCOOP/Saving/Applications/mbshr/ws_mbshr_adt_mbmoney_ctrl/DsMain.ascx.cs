using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;

namespace Saving.Applications.mbshr.ws_mbshr_adt_mbmoney_ctrl
{
    public partial class DsMain : DataSourceFormView
    {
        public DataSet1.DataMainDataTable DATA { get; set; }

        public void InitDsMain(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.DataMain;
            this.EventItemChanged = "OnDsMainItemChanged";
            this.EventClicked = "OnDsMainClicked";
            this.InitDataSource(pw, FormView1, this.DATA, "dsMain");
            this.Button.Add("b_search");           
            this.Register();
        }
        public void RetrieveMain(String member_no)
        {
            String sql = @"SELECT   MBMEMBMASTER.MEMBER_NO,
                        FT_MEMNAME(MBMEMBMASTER.COOP_ID,MBMEMBMASTER.MEMBER_NO) as fullname
                        FROM MBMEMBMASTER
                        WHERE  
                        ( ( MBMEMBMASTER.COOP_ID = {0}) AND  
                        ( MBMEMBMASTER.MEMBER_NO ={1}) )   ";

            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, member_no);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
    }
}