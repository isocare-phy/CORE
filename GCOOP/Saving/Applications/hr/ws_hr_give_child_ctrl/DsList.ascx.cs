using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;

namespace Saving.Applications.hr.ws_hr_give_child_ctrl
{
    public partial class DsList : DataSourceRepeater
    {
        public DataSet1.DT_LISTDataTable DATA { get; set; }

        public void InitDsList(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.DT_LIST;
            this.EventItemChanged = "OnDsDsListItemChanged";
            this.EventClicked = "OnDsListClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsList");
            //this.Button.Add("b_delete");
            this.Register();
        }
        public void Retrieve(string emp_no)
        {
            string sql = @"select hf.f_name as child_name,hf.occupation as occupation,add_months(hf.birth_date,(18*12)) as date2,hf.birth_date as birth_date,
                            floor(floor(MONTHS_BETWEEN(SYSDATE,hf.birth_date )) /12) as  age
                            from hremployee hr,hremployeefamily hf,mbucfprename mp,hrucfposition hp
                            where hr.emp_no = hf.emp_no
                            and hr.prename_code = mp.prename_code
                            and hr.pos_code = hp.pos_code
                            and hf.f_relation = 'บุตร'
                            and hr.coop_id = {0}
                            and hr.emp_no = {1}
                            and floor(floor(MONTHS_BETWEEN(SYSDATE,hf.birth_date )) /12) < 19
";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, emp_no);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
    }
}