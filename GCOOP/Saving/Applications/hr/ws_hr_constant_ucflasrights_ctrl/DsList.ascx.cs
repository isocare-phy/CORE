using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CoreSavingLibrary;

namespace Saving.Applications.hr.ws_hr_constant_ucflasrights_ctrl
{
    public partial class DsList : DataSourceRepeater
    {
        public DataSet1.HRUCFLASRIGHTSDataTable DATA { get; set; }

        public void InitDsList(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.HRUCFLASRIGHTS;
            this.EventItemChanged = "OnDsListItemChanged";
            this.EventClicked = "OnDsListClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsList");
            this.Button.Add("b_delete");
            this.Register();
        }
        public void Retrieve()
        {
            string sql = @"
                select las_code,leave_code,
                    las_smwork,las_emwork,
                    las_dateleave  
                    from hrucflasrights order by las_code";
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
        public void DdHrucfleavecode()
        {
            string sql = @"
                select leave_code,leave_desc, 
                1 as sorter 
                from hrucfleavecode
                union
                select '','',0 from dual order by sorter";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "leave_code", "leave_desc", "LEAVE_CODE");
        }
    }
}