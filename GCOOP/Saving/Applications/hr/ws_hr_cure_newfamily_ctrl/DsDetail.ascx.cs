using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CoreSavingLibrary;

namespace Saving.Applications.hr.ws_hr_cure_newfamily_ctrl
{
    public partial class DsDetail : DataSourceRepeater
    {
        public DataSet1.HREMPLOYEEASSISTDataTable DATA { get; set; }

        public void InitDsDetail(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.HREMPLOYEEASSIST;
            this.EventItemChanged = "OnDsDetailItemChanged";
            this.EventClicked = "OnDsDetailClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsDetail");
            this.Button.Add("b_detail");
            this.Button.Add("b_delete");
            this.Register();
        }

        public void RetrieveDetail(string emp_no)
        {
            string sql = @"select 
emp_no,
                            seq_no,
                            assist_name,
                            assist_sdate,
                            assist_amt,
                            assist_minamt
from hremployeeassist
 					where  coop_id={0} and emp_no = {1} and assist_code = '01' order by seq_no";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, emp_no);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
    }
}