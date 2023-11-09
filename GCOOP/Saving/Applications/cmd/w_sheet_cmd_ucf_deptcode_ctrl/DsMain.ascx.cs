using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Saving.Applications.cmd.w_sheet_cmd_ucf_deptcode_ctrl
{
    public partial class DsMain : DataSourceFormView
    {
        

        public DataSet1.deptcodeDataTable DATA { get; set; }

        public void InitDsMain(PageWeb pw)
        {
            css1.Visible = false;


            DataSet1 ds = new DataSet1();
            this.DATA = ds.deptcode;
            this.EventItemChanged = "OnDsMainItemChanged";
            this.EventClicked = "OnDsMainClicked";
            this.InitDataSource(pw, FormView1, this.DATA, "dsMain");
            this.Register();
        }

        public void retrieve(string dept_code)
        {
            String sql = @" SELECT ptucfdeptcode.dept_code,   
         ptucfdeptcode.dept_desc,   
         ptucfdeptcode.dept_abb  
    FROM ptucfdeptcode  
where ptucfdeptcode.dept_code={0}
ORDER BY ptucfdeptcode.dept_code ASC   ";
            sql = WebUtil.SQLFormat(sql, dept_code);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
    }
}