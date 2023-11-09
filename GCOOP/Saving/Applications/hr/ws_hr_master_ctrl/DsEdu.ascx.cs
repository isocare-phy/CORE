using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using DataLibrary;
using System.Data;

namespace Saving.Applications.hr.ws_hr_master_ctrl
{
    public partial class DsEdu : DataSourceRepeater
    {
        public DataSet1.HREMPLOYEEEDUDataTable DATA { get; set; }

        public void InitDsEdu(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.HREMPLOYEEEDU;
            this.EventItemChanged = "OnDsEduItemChanged";
            this.EventClicked = "OnDsEduClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsEdu");
            this.Button.Add("b_del");
            this.Register();
        }

        public void Retrieve(string as_empno)
        {
            string sql = @"select hremployeeedu.coop_id,   
                 hremployeeedu.emp_no,   
                 hremployeeedu.seq_no,   
                 hremployeeedu.education_code,   
                 hremployeeedu.edu_inst,   
                 hremployeeedu.edu_degree,   
                 hremployeeedu.edu_major,   
                 hremployeeedu.edu_gpa,   
                 hremployeeedu.edu_succyear  
            from hremployeeedu
            where hremployeeedu.coop_id = {0}
            and hremployeeedu.emp_no = {1}";

            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, as_empno);
            Sdt dt = WebUtil.QuerySdt(sql);
            this.ImportData(dt);
        }

        public void DdEducation()
        {
            string sql = @"select education_code,
            education_desc,
            1 as sorter
            from hrucfeducation
            union 
            select '','', 0 from dual
            order by sorter, education_code";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "education_code", "education_desc", "education_code");
        }
    }
}