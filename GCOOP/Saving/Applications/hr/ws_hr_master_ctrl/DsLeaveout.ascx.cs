using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using DataLibrary;


namespace Saving.Applications.hr.ws_hr_master_ctrl
{
    public partial class DsLeaveout : DataSourceRepeater
    {
        public DataSet1.HRLOGLEAVEDataTable DATA { get; set; }

        public void InitDsLeaveout(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.HRLOGLEAVE;
            this.EventItemChanged = "OnDsLeavesItemChanged";
            //this.EventClicked = "OnDsListClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsLeaves");
            this.Register();
        }
        // FTCNVTDATE(hl.apv_date,2) as apv_d,
        public void RetrieveHrLeaveout(string emp_no)
        {

            if (state.SsCoopControl == "031001")
            {

            string sql = @"select  hl.emp_no,hl.seq_no,
		                           hlc.leave_desc,
		                           hl.START_TIME,hl.LAST_TIME,
		                           hl.TOTALTIME,
                                  
                                   CASE 
            WHEN hl.apv_status = '0' THEN 'อนุมัติการลา' 
            WHEN hl.apv_status = '1' THEN 'ไม่อนุมัติการลา'
            WHEN hl.apv_status = '2' THEN 'ยกเลิกการลา'
               ELSE ' ' 
       END as apv_s,hl.leave_cause
                            from 
                                   hrlogleave hl,
		                           hrucfleavecode hlc,
                                   hrucfleaveapvby hlb
                            where
                                   hl.emp_no={0} and
                                   hl.leave_code = hlc.leave_code and
                                   hl.apv_bycode = hlb.apv_bycode and
                                   hl.leave_code = 'I'
                            order by
                                   hl.seq_no asc";
            sql = WebUtil.SQLFormat(sql, emp_no);
            Sdt dt = WebUtil.QuerySdt(sql);
            this.ImportData(dt);
            }else{

                string sql = @"select  hl.emp_no,hl.seq_no,
		                           hlc.leave_desc,
		                           hl.START_TIME,hl.LAST_TIME,
		                           hl.TOTALTIME,
                                  
                                   CASE 
            WHEN hl.apv_status = '0' THEN 'อนุมัติการลา' 
            WHEN hl.apv_status = '1' THEN 'ไม่อนุมัติการลา'
            WHEN hl.apv_status = '2' THEN 'ยกเลิกการลา'
               ELSE ' ' 
       END as apv_s,hl.leave_cause
                            from 
                                   hrlogleave hl,
		                           hrucfleavecode hlc,
                                   hrucfleaveapvby hlb
                            where
                                   hl.emp_no={0} and
                                   hl.leave_code = hlc.leave_code and
                                   hl.apv_bycode = hlb.apv_bycode and
                                   hl.leave_code = '004'
                            order by
                                   hl.seq_no asc";
            sql = WebUtil.SQLFormat(sql, emp_no);
            Sdt dt = WebUtil.QuerySdt(sql);
            this.ImportData(dt);

                }
        }
    }
}