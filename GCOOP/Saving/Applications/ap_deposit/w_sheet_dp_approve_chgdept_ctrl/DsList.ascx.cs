using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;

namespace Saving.Applications.ap_deposit.w_sheet_dp_approve_chgdept_ctrl
{
    public partial class DsList : DataSourceRepeater
    {
        public DataSet1.DPREQCHG_DEPTDataTable DATA { get; set; }
        public void InitDsList(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.DPREQCHG_DEPT;
            this.EventItemChanged = "OnDsListItemChanged";
            this.EventClicked = "OnDsListClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsList");
            this.Register();
        }
        public void RetrieveData(string coop_id, DateTime select_date, DateTime end_date)
        {
            string sql = @" SELECT
	            COOP_ID  ,
	            DPREQCHG_DOC,   
                    DEPTACCOUNT_NO,   
                    DEPTMONTCHG_DATE,   
                    DEPTMONTH_OLDAMT,   
                    DEPTMONTH_NEWAMT,           
	            ( CASE WHEN CHANGE_STATUS = 0 THEN 'งดฝากรายเดือน' ELSE 'ฝากรายเดือน' END )CHANGE_DESC,
                    CHANGE_STATUS
            FROM DPREQCHG_DEPT WHERE APPROVE_FLAG=0
            AND COOP_ID={0}
            AND REQCHG_DATE between {1} and {2}";
            sql = WebUtil.SQLFormat(sql, coop_id, select_date, end_date);
            DataTable dt = WebUtil.Query(sql);
            ImportData(dt);
        }
    }    
}