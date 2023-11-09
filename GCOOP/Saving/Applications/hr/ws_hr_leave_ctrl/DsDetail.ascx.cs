using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CoreSavingLibrary;

namespace Saving.Applications.hr.ws_hr_leave_ctrl
{
    public partial class DsDetail : DataSourceRepeater
    {
        public DataSet1.HRLOGLEAVEDataTable DATA { get; set; }

        public void InitDsDetail(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.HRLOGLEAVE;
            this.EventItemChanged = "OnDsDetailItemChanged";
            this.EventClicked = "OnDsDetailClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsDetail");
            this.Button.Add("b_detail");
            this.Button.Add("b_delete");
            this.Register();
        }

        public void RetrieveDetail(string emp_no)
        {
            if (state.SsCoopControl == "031001")
            {

                string sql = @"select  hl.emp_no,hl.seq_no,
		                           hlc.leave_desc,
		                           hl.leave_from,hl.leave_to,
		                           hl.totalday,hl.apv_date         
                            from 
                                  hrlogleave hl,
		                          hrucfleavecode hlc
                            where
                                  hl.emp_no={0} and
                                  hl.leave_code = hlc.leave_code and
                                  hl.leave_code != 'I'
                            order by
                                  hl.seq_no asc";
                sql = WebUtil.SQLFormat(sql, emp_no);
                DataTable dt = WebUtil.Query(sql);
                this.ImportData(dt);
            }
            else
            {
                string sql = @"select  hl.emp_no,hl.seq_no,
		                           hlc.leave_desc,
		                           hl.leave_from,hl.leave_to,
		                           hl.totalday,hl.apv_date         
                            from 
                                  hrlogleave hl,
		                          hrucfleavecode hlc
                            where
                                  hl.emp_no={0} and
                                  hl.leave_code = hlc.leave_code and
                                  hl.leave_code != '004'
                            order by
                                  hl.seq_no asc";
                sql = WebUtil.SQLFormat(sql, emp_no);
                DataTable dt = WebUtil.Query(sql);
                this.ImportData(dt);

            }
        }
    }
}