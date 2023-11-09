using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataLibrary;
using CoreSavingLibrary;
using System.Data;

namespace Saving.Applications.hr.ws_hr_leave_ctrl
{
    public partial class DsCallas : DataSourceFormView//System.Web.UI.UserControl
    {
        public DataSet1.HRLOGLEAVEDataTable DATA { get; set; }
        public void InitDs(PageWeb pw)
        {

            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.HRLOGLEAVE;
            this.EventItemChanged = "OnDsLasrightsItemChanged";
            this.EventClicked = "OnDsLasrightsClicked";
            this.InitDataSource(pw, FormView1, this.DATA, "dsLasrights");
            this.Register();
        }

        public void RetrieveWorkage(string EmpNo)
        {
            string year = Convert.ToString(WebUtil.GetAccyear(state.SsCoopControl, state.SsWorkDate));
            string year_eng = Convert.ToString(Convert.ToInt32(year) - 543);
            string year_eng2 = Convert.ToString(Convert.ToInt32(year_eng) - 1);
            string sql = @"select LAS_DATELEAVE as TO_YEAR
                           from HRUCFLASRIGHTS 
                           where LEAVE_CODE = '006'
                                 and LAS_SMWORK <= (select sum(trunc(Ft_Calage(work_date,sysdate,4))*12) as aa
                                                    from hremployee where emp_no = {1})
                                 and LAS_EMWORK >= (select sum(trunc(Ft_Calage(work_date,sysdate,4))*12) as aa
                                                    from hremployee where emp_no = {1})";
            sql = WebUtil.SQLFormat(sql, year_eng2, EmpNo);
            Sdt dt = WebUtil.QuerySdt(sql);
            this.ImportData(dt);
        }
    }
}