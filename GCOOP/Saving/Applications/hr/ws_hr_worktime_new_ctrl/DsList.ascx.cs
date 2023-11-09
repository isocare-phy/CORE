using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;
using DataLibrary;
using System.Globalization;
using System.Drawing;
using System.Data;

namespace Saving.Applications.hr.ws_hr_worktime_new_ctrl
{
    public partial class DsList : DataSourceRepeater
    {
        public DataSet1.HRLOGWORKTIMEDataTable DATA { get; private set; }
        public void InitDs(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.HRLOGWORKTIME;
            this.EventItemChanged = "OnDsListItemChanged";
            this.EventClicked = "OnDsListClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsList");
            this.Register();
        }

        public void RetrieveLogWorkDate(string work_date, string emp_no)
        {
            string sql = @"
                       select hw.emp_no,mp.prename_desc,he.emp_name,he.emp_surname,hw.start_time,hw.end_time,hwc.worktime_code
                        from hremployee he,mbucfprename mp,hrlogworktime hw,hrucfworktimecode hwc
                        where to_char(hw.work_date,'yyyymm') = {0}
                        and hw.coop_id = {1}
                        and he.emp_no = hw.emp_no
                        and he.prename_code=mp.prename_code
                        and hw.worktime_code = hwc.worktime_code
                        and to_char(he.work_date,'yyyymm') <= {0}
                        and he.emp_no = {2}
                        order by emp_no";
            sql = WebUtil.SQLFormat(sql, work_date, state.SsCoopId, emp_no);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }

        public void RetrieveShowEmp(string year, string month, string emp_no)
        {
            string sql = @"  
                select   DISTINCT ht.work_date,
                ht.emp_no,
                mp.prename_desc,
                ht.emptype_code,
                he.emp_name,
                he.emp_surname,
                ht.worktime_code as worktime,
			    hl.leave_code ,
                nvl(hw.work_out,0) as hwwork_out,
                hl.leave_code ,
                ht.back_time,
                hw.work_in as max_late,
                TO_CHAR(ht.end_time , 'hh24') ||'.'|| TO_CHAR(ht.end_time , 'mi')  as work_out,
			    TO_CHAR(ht.start_time , 'hh24') ||'.'|| TO_CHAR(ht.start_time , 'mi') as work_in,
                (case when hl.emp_no is not null   then  hc.leave_desc			
                   when ha.late_cause = '2' then 'สาย'
					when  TO_CHAR(ht.end_time , 'hh24') ||'.'|| TO_CHAR(ht.end_time , 'mi') = '00.00' and  TO_CHAR(ht.start_time , 'hh24') ||'.'|| TO_CHAR(ht.start_time , 'mi') = '00.00'   then 'ขาด' 
                    when  TO_CHAR(ht.end_time , 'hh24') ||'.'|| TO_CHAR(ht.end_time , 'mi') = '00.00' or TO_CHAR(ht.start_time , 'hh24') ||'.'|| TO_CHAR(ht.start_time , 'mi') = '00.00'   then 'ลงเวลางานไม่ครบ' 
					when  ht.back_time != '' or  ht.back_time  is not null    then 'กลับก่อนเวลา'
                    when  TO_CHAR(ht.end_time , 'hh24') ||'.'|| TO_CHAR(ht.end_time , 'mi') != '00.00' and  TO_CHAR(ht.start_time , 'hh24') ||'.'|| TO_CHAR(ht.start_time , 'mi') != '00.00'  and trim(ht.worktime_code) = 'HO' then 'ทำงานในวันหยุด'      
                    else  'เข้างานปกติ' end) as worktime_code
                from hrlogworktime ht inner join hremployee he on ht.emp_no = he.emp_no
                left join hrlogleave hl on ht.emp_no = hl.emp_no  and hl.apv_status =1 and   ht.work_date between trunc(hl.leave_from)  and  trunc(hl.leave_to)
                left join  hrucfleavecode hc on hl.leave_code = hc.leave_code
			    left join hrloglate ha on ht.emp_no = ha.emp_no  and ht.work_date = ha.late_date,
                mbucfprename mp,
                hrucfworktimecode hw
                where he.emp_status = 1
                and he.prename_code = mp.prename_code
                and he.worktime_code = hw.worktime_code
                and ht.coop_id ={0}
                and to_char(ht.work_date,'mm/yyyy') ={1}
               and ht.emp_no ={2}
                order by ht.emp_no, ht.work_date";
            sql = WebUtil.SQLFormat(sql, state.SsCoopId, month + "/" + year, emp_no);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
    }
}