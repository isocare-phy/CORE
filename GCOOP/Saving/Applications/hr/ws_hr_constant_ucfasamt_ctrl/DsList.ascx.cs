using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CoreSavingLibrary;

namespace Saving.Applications.hr.ws_hr_constant_ucfasamt_ctrl
{
    public partial class DsList : DataSourceRepeater
    {
        public DataSet1.HRUCFASAMTDataTable DATA { get; set; }

        public void InitDsList(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.HRUCFASAMT;
            this.EventItemChanged = "OnDsListItemChanged";
            this.EventClicked = "OnDsListClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsList");
            this.Button.Add("b_delete");
            this.Register();
        }
        public void Retrieve()
        {
            string sql = @"
                select as_code,assist_code,
                    as_amt
                    from hrucfasamt order by as_code";
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
        public void DdHrucfassistcode()
        {
            string sql = @"
                select assist_code,assist_desc, 
                1 as sorter 
                from hrucfassist
                union
                select '','',0 from dual order by sorter";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "assist_code", "assist_desc", "assist_code");
        }
    }
}