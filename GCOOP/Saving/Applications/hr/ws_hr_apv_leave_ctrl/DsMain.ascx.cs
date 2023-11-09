using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CoreSavingLibrary;
using System.Data.OracleClient;

namespace Saving.Applications.hr.ws_hr_apv_leave_ctrl
{
    public partial class DsMain : DataSourceRepeater
    {
        public DataSet1.HRLOGLEAVEDataTable DATA { get; set; }

        public void InitDsDetail(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.HRLOGLEAVE;
            this.EventItemChanged = "OnDsMainItemChanged";
            this.EventClicked = "OnDsMainClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsMain");
            this.Button.Add("b_apv");
            this.Button.Add("b_apv_cancel");
            this.Button.Add("b_detail");
            this.Register();
        }

        public void RetrieveLeaveDetail(DateTime _getdate)
        {
            string sql = @"select  hl.emp_no,hl.seq_no,
                                   hl.leave_date,
		                           hlc.leave_desc,
		                           hl.leave_from,hl.leave_to,
		                           hl.totalday,hl.apv_date,
                                   he.EMP_NAME||' '||he.EMP_SURNAME as fullname         
                            from 
                                  hrlogleave hl,
		                          hrucfleavecode hlc, HREMPLOYEE he
                            where
                                  hl.coop_id={0} and
                                  hl.leave_date= {1} and
                                  hl.leave_code = hlc.leave_code and
                                  hl.EMP_NO=he.EMP_NO and
                                  hl.apv_status=8
                            order by
                                  hl.seq_no asc";
            sql = WebUtil.SQLFormat(sql, state.SsCoopId,_getdate);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
    }
}