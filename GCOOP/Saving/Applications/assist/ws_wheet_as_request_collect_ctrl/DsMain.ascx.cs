using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;

namespace Saving.Applications.assist.ws_wheet_as_request_collect_ctrl
{
    public partial class DsMain : DataSourceFormView
    {
        public DataSet1.DataDsMainDataTable DATA { get; set; }
        public void InitDsMain(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.DataDsMain;
            this.EventItemChanged = "OnDsMainItemChanged";
            this.EventClicked = "OnDsMainClicked";
            this.InitDataSource(pw, FormView1, this.DATA, "dsMain");
            this.Button.Add("b_search");
            this.Register();
        }

        public void RetrieveMain(String ls_member_no)
        {
            String sql = @" 
                         	   SELECT MBMEMBMASTER.MEMBER_NO,  MBMEMBMASTER.resign_status ,
                                 MBUCFPRENAME.PRENAME_DESC||MBMEMBMASTER.MEMB_NAME||'  '|| MBMEMBMASTER.MEMB_SURNAME AS FULLNAME,                                 
                                 TRIM(MBMEMBMASTER.MEMBGROUP_CODE)||' - '||MBUCFMEMBGROUP.MEMBGROUP_DESC AS FULLGROUP_DESC ,
                                 MBUCFMEMBTYPE.MEMBTYPE_DESC                                     
                             FROM MBMEMBMASTER,   
                                 MBUCFMEMBGROUP,   
                                 MBUCFMEMBTYPE,   
                                 MBUCFPRENAME  
                           WHERE ( mbmembmaster.membtype_code = mbucfmembtype.membtype_code (+)) and  
                                 ( MBMEMBMASTER.PRENAME_CODE = MBUCFPRENAME.PRENAME_CODE ) and  
                                 ( MBMEMBMASTER.MEMBGROUP_CODE = MBUCFMEMBGROUP.MEMBGROUP_CODE ) and  
                                 ( MBMEMBMASTER.COOP_ID = MBUCFMEMBGROUP.COOP_ID ) and  
                                 ( MBMEMBMASTER.COOP_ID = {0} ) AND  
                                 ( mbmembmaster.member_no ={1} )";

            sql = WebUtil.SQLFormat(sql, state.SsCoopId, ls_member_no);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
    }
}