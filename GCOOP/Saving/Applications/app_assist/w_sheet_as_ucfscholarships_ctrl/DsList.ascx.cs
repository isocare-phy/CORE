using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Saving.Applications.app_assist.w_sheet_as_ucfscholarships_ctrl
{
    public partial class DsList : DataSourceRepeater
    {
        public DataSet1.ASNSENVIRONMENTVARDataTable DATA { get; set; }

        public void InitDsList(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.ASNSENVIRONMENTVAR;
            this.Button.Add("b_del");
            this.EventClicked = "OnDsListClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsList");
            this.Register();
        }

        public void retrieve()
        {
            string sql = @"    SELECT ASNSENVIRONMENTVAR.ENVVALUE,   
         ASNSENVIRONMENTVAR.ENVDESC,   
         ASNSENVIRONMENTVAR.ENVGROUP,   
         ASNSENVIRONMENTVAR.START_AGE,   
         ASNSENVIRONMENTVAR.END_AGE,   
         ASNSENVIRONMENTVAR.SYSTEM_CODE,   
         ASNSENVIRONMENTVAR.SEQ_NO,   
         ASNSENVIRONMENTVAR.ENVCODE,   
         ASNSENVIRONMENTVAR.TOFROM_ACCID,ASNSENVIRONMENTVAR.ENVCODE
    FROM ASNSENVIRONMENTVAR  
   WHERE ASNSENVIRONMENTVAR.ENVGROUP = 'school'  order by seq_no  
 ";
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
    }
}