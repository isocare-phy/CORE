using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CoreSavingLibrary;
using DataLibrary;

namespace Saving.Applications.hr.ws_hr_leave_n_ctrl
{
    public partial class DsLeave : DataSourceFormView
    {
        public DataSet1.HRLOGLEAVEDataTable DATA { get; set; }

        public void InitDsLeave(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.HRLOGLEAVE;
            this.EventItemChanged = "OnDsLeaveItemChanged";
            this.EventClicked = "OnDsLeaveClicked";
            this.InitDataSource(pw, FormView1, this.DATA, "dsLeave");
            this.Register();
        }

        public void DdHrucfleavecode()
        {
            string sql = @"
                select leave_code,leave_desc, 1 as sorter from hrucfleavecode where leave_code != '004'
                union
                select '','',0 from dual order by sorter";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "leave_code", "leave_desc", "LEAVE_CODE");
        }
    }
}