using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OracleClient;
using CoreSavingLibrary;
using DataLibrary;

namespace Saving.Applications.hr.ws_hr_edit_leave_ctrl
{
    public partial class DsList : DataSourceRepeater
    {
        public DataSet1.HRLOGLEAVEDataTable DATA { get; set; }

        public void InitDsList(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.HRLOGLEAVE;
            this.EventItemChanged = "OnDsListItemChanged";
            this.EventClicked = "OnDsListClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsList");
            this.Button.Add("b_detail");
            this.Register();
        }

        public void RetrieveList(string emp_no)
        {
            string sql = @"select 
                            hrlogleave.seq_no,
                            hrlogleave.emp_no,
                            hrlogleave.leave_date,
                            hrucfleavecode.leave_desc,
                            hrlogleave.leave_from,
                            hrlogleave.leave_to,
                            hrlogleave.totalday,
                            hrlogleave.apv_status,
                            case 
                            when hrlogleave.apv_status=8  then 'รอการอนุมัติ'
                            when hrlogleave.apv_status=-9 then 'ไม่อนุมัติ'
                            when hrlogleave.apv_status=1 then 'อนุมัติ'
                            end as apvstatus
                            from 
                            hrlogleave ,
                            hrucfleavecode 
                            where 
                            hrlogleave.emp_no = {0} and 
                            hrlogleave.leave_code=hrucfleavecode.leave_code and
                            hrlogleave.apv_status = 8
                            order by hrlogleave.seq_no";
            sql = WebUtil.SQLFormat(sql, emp_no);
            Sdt dt = WebUtil.QuerySdt(sql);
            this.ImportData(dt);
        }
    }
}