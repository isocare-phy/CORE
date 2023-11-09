using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OracleClient;
using CoreSavingLibrary;
using DataLibrary;

namespace Saving.Applications.hr.ws_hr_workout_ctrl
{
    public partial class DsLog : DataSourceRepeater
    {
        public DataSet1.HRW_OTDataTable DATA { get; set; }

        public void InitDsList(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.HRW_OT;
            this.EventItemChanged = "OnDsLogItemChanged";
            this.EventClicked = "OnDsLogClicked";
            this.Button.Add("b_detail");
            this.InitDataSource(pw, Repeater1, this.DATA, "DsLog");
            this.Register();
        }

        public void RetrieveHrLog(string EmpNo)
        {
            string sql = @"select hrw_ot.seq_no,hrucfottype.ottype,
                            hrw_ot.date_work,
                            hrw_ot.work_in,
                            hrw_ot.work_out,
                            hrw_ot.ot_p,
                            (case when hrw_ot.apv_ot_status = '8' then 'รออนุมัติ'
                            when hrw_ot.apv_ot_status = '1' then 'อนุมัติ' else 'ยกเลิกโอที' end) as apv_ot_status
                            from hrw_ot , hrucfottype
                            where hrw_ot.emp_no = {0} 
                            and hrw_ot.apv_ot_status = '8' 
                            and trim(hrw_ot.remark) = trim(hrucfottype.seq_no) 
                            order by hrw_ot.seq_no";
            sql = WebUtil.SQLFormat(sql, EmpNo);
            Sdt dt = WebUtil.QuerySdt(sql);
            this.ImportData(dt);
        }
    }
}