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
    public partial class DsLeaves : DataSourceRepeater
    {
        public DataSet1.HRLOGLEAVEDataTable DATA { get; set; }

        public void InitDsLeave(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.HRLOGLEAVE;
            this.EventItemChanged = "OnDsLeavesItemChanged";
            //this.EventClicked = "OnDsListClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsLeaves");
            this.Register();
        }

        public void RetrieveHrLeaves(string emp_no)
        {

            if (state.SsCoopControl == "031001")
            {

                string sql = @"select hl.emp_no,hl.seq_no,
		                           hlc.leave_desc,
		                           TO_CHAR(hl.leave_from, 'dd MON yyyy', 'NLS_CALENDAR=''THAI BUDDHA'' NLS_DATE_LANGUAGE=THAI') as leave_f ,
                                   TO_CHAR(hl.leave_to, 'dd MON yyyy', 'NLS_CALENDAR=''THAI BUDDHA'' NLS_DATE_LANGUAGE=THAI') as leave_t,
		                           hl.totalday,
                                   hl.leave_cause
                            from 
                                   hrlogleave hl,
		                           hrucfleavecode hlc,
                                   hrucfleaveapvby hlb
                            where
                                   hl.emp_no={0} and
                                   hl.leave_code = hlc.leave_code(+) and
                                   hl.apv_bycode = hlb.apv_bycode(+)and
                                   hl.leave_code != 'I'
                            order by
                                   hl.seq_no asc";

                sql = WebUtil.SQLFormat(sql, emp_no);
                Sdt dt = WebUtil.QuerySdt(sql);
                this.ImportData(dt);
            }
            else
            {

                string sql = @"select hl.emp_no,hl.seq_no,
		                           hlc.leave_desc,
		                           TO_CHAR(hl.leave_from, 'dd MON yyyy', 'NLS_CALENDAR=''THAI BUDDHA'' NLS_DATE_LANGUAGE=THAI') as leave_f ,
                                   TO_CHAR(hl.leave_to, 'dd MON yyyy', 'NLS_CALENDAR=''THAI BUDDHA'' NLS_DATE_LANGUAGE=THAI') as leave_t,
		                           hl.totalday,
                                   hl.leave_cause
                            from 
                                   hrlogleave hl,
		                           hrucfleavecode hlc,
                                   hrucfleaveapvby hlb
                            where
                                   hl.emp_no={0} and
                                   hl.leave_code = hlc.leave_code(+) and
                                   hl.apv_bycode = hlb.apv_bycode(+) and
                                   hl.leave_code != '004'
                            order by
                                   hl.seq_no asc";

                sql = WebUtil.SQLFormat(sql, emp_no);
                Sdt dt = WebUtil.QuerySdt(sql);
                this.ImportData(dt);
            }
           
        }
    }
}