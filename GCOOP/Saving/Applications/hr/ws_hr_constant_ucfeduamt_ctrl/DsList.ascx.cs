using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CoreSavingLibrary;

namespace Saving.Applications.hr.ws_hr_constant_ucfeduamt_ctrl
{
    public partial class DsList : DataSourceRepeater
    {
        public DataSet1.HRUCFEDUAMTDataTable DATA { get; set; }

        public void InitDsList(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.HRUCFEDUAMT;
            this.EventItemChanged = "OnDsListItemChanged";
            this.EventClicked = "OnDsListClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsList");
            this.Button.Add("b_delete");
            this.Register();
        }
        public void Retrieve()
        {
            string sql = @"
                select edu_code,education_code,
                    edu_amt
                    from hrucfeduamt order by edu_code";
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
        public void DdHrucfeducationcode()
        {
            string sql = @"
                select education_code,education_desc, 
                1 as sorter 
                from hrucfeducation
                union
                select '','',0 from dual order by sorter";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "education_code", "education_desc", "education_code");
        }
    }
}