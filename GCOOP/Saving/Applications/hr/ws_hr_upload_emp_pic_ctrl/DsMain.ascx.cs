using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Saving.Applications.hr.ws_hr_upload_emp_pic_ctrl
{
    public partial class DsMain : DataSourceFormView
    {
        public DataSet1.HREMPLOYEEDataTable DATA { get; private set; }

        public void InitDsMain(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.HREMPLOYEE;
            this.InitDataSource(pw, FormView1, this.DATA, "dsMain");
            this.EventItemChanged = "OnDsMainItemsChanged";
            this.Register();
        }

        public void Retrieve(string emp_no)
        {
            string sql =
                @"select h.coop_id,h.emp_no,p.prename_desc,h.emp_name,h.emp_surname 
                from hremployee h,mbucfprename p 
                where h.prename_code = p.prename_code
                and h.coop_id = {1}
                and h.emp_no = {0}";

            //@"select emp_no,emp_name,emp_surname from hremployee order by emp_no";

            sql = WebUtil.SQLFormat(sql, emp_no, state.SsCoopControl);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);

        }
    }
}