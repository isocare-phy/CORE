using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CoreSavingLibrary;
using System.Data.OracleClient;

namespace Saving.Applications.hr.w_hr_apv_worktime_ctrl
{
    public partial class DsMain : DataSourceRepeater
    {
        public DataSet1.HRLOGWORKTIMEDataTable DATA { get; set; }

        public void InitDsDetail(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.HRLOGWORKTIME;
            this.EventItemChanged = "OnDsMainItemChanged";
            this.EventClicked = "OnDsMainClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsMain");
            this.Register();
        }

        public void RetrieveLeaveDetail(DateTime _getdate)
        {
            string sql = @"select hr.emp_no || ' -  ' || mup.prename_desc || hr.emp_name || ' ' || hr.emp_surname as full_name,
(select TO_CHAR(hw.work_date, 'dd MON yyyy', 'NLS_CALENDAR=''THAI BUDDHA'' NLS_DATE_LANGUAGE=THAI')from hrlogworktime hw where hw.emp_no = hr.emp_no and to_date(to_date(hw.work_date,'dd/mm/yyyy'),'dd/mm/yyyy') = to_date({0},'dd/mm/yyyy')) as work_date,
(select SUBSTR(to_char(hw.start_time,'dd/mm/yyyy HH:MI:SS'),11,9) from hrlogworktime hw where hw.emp_no = hr.emp_no and to_date(hw.work_date,'dd/mm/yyyy') = to_date(to_date({0}),'dd/mm/yyyy')) as start_time,
(select SUBSTR(to_char(hw.end_time,'dd/mm/yyyy HH:MI:SS'),11,9) from hrlogworktime hw where hw.emp_no = hr.emp_no and to_date(hw.work_date,'dd/mm/yyyy') = to_date(to_date({0}),'dd/mm/yyyy')) as end_time,
(select (case when hl.late_time is not null then 'สาย' else 'ไม่สาย' end) as late_time from hrloglate hl where hr.emp_no = hl.emp_no and to_date(hl.late_date,'dd/mm/yyyy') = to_date({0},'dd/mm/yyyy')) as late_time,
(select (case when to_date(hw.work_date,'dd/mm/yyyy') is not null then 'บันทึกเเล้ว' else 'ยังไม่ได้บันทึก' end) as apv_worktime from hrlogworktime hw where hw.emp_no = hr.emp_no and to_date(hw.work_date,'dd/mm/yyyy') = to_date({0},'dd/mm/yyyy')) as apv_worktime 
from hremployee hr , mbucfprename mup
where hr.emp_no between '0' and '9'
and hr.emp_status = '1'
and mup.prename_code = hr.prename_code 
and hr.work_date <= {0}
order by hr.emp_no";
            sql = WebUtil.SQLFormat(sql, _getdate);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);

        }
    }
}