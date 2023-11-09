using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;

namespace Saving.Applications.hr.ws_hr_cure_family_ctrl
{
    public partial class DsList : DataSourceRepeater
    {
        public DataSet1.HREMPLOYEEASSISTDataTable DATA { get; set; }

        public void InitDsList(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.HREMPLOYEEASSIST;
            this.EventItemChanged = "OnDsListItemChanged";
            this.EventClicked = "OnDsListClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsList");
            this.Button.Add("b_detail");
            this.Register();
        }

        public void RetreiveList(string emp_no)
        {
            string sql = @"select coop_id,emp_no,seq_no,assist_date,
                assist_name,
                assist_detail,
                assist_sdate,
                assist_minamt
                from hremployeeassist
                where emp_no = {0} and assist_code = '01'";
            sql = WebUtil.SQLFormat(sql, emp_no);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }

    }
}