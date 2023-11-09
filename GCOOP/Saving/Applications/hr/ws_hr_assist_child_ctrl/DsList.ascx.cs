using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;

namespace Saving.Applications.hr.ws_hr_assist_child_ctrl
{
    public partial class DsList : DataSourceRepeater
    {
        public DataSet1.HREMPLOYEEASSISTDataTable DATA { get; set; }

        public void InitDsList(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.HREMPLOYEEASSIST;
            this.EventItemChanged = "OnDsDsListItemChanged";
            this.EventClicked = "OnDsListClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsList");
            this.Button.Add("b_detail");
            this.Button.Add("b_delete");
            this.Register();
        }
        public void Retrieve(string emp_no)
        {
            string sql = @"select ha.coop_id,ha.emp_no,ha.seq_no,ha.assist_date,
                ha.assist_detail,
                ha.assist_posit||' ' ||'ภาคเรียนที่'||' '|| ha.assist_month as assist_posit,
                ha.assist_month,
                edu.education_desc,
                ha.assist_amt
                from hremployeeassist ha,
                HRUCFEDUCATION edu
                where ha.EDUCATION_CODE = edu.EDUCATION_CODE and
                      ha.assist_code = '02' and ha.emp_no = {0}";
            sql = WebUtil.SQLFormat(sql, emp_no);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
    }
}