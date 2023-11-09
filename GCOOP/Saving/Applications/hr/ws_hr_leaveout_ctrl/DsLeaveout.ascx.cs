using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;

namespace Saving.Applications.hr.ws_hr_leaveout_ctrl
{
    public partial class DsLeaveout : DataSourceFormView
    {
        public DataSet1.HRLOGLEAVEDataTable DATA { get; set; }
        public void InitDs(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.HRLOGLEAVE;
            this.EventItemChanged = "OnDsLeaveItemChanged";
            this.EventClicked = "OnDsLeaveClicked";
            this.InitDataSource(pw, FormView1, this.DATA, "dsLeave");
            this.Register();
        }

        public void Retrieveleave(string seq_no, string emp_no)
        {
            string sql = @"select coop_id,emp_no,seq_no,operate_date,
                                  leave_code,leave_cause,
                                  start_time,last_time,totaltime,
                                  apv_status,apv_date,apv_bycode,
                                  Leave_Place,Leave_Tel
                            from hrlogleave
                            where coop_id={0} and seq_no ={1} and emp_no = {2}";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, seq_no, emp_no);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }

        public void DdHrucfleavecode()
        {
            if (state.SsCoopControl == "031001")
            {

                string sql = @"
                select leave_code,leave_desc, 1 as sorter from hrucfleavecode where leave_code = 'I'
                union
                select '','',0 from dual order by sorter";
                DataTable dt = WebUtil.Query(sql);
                this.DropDownDataBind(dt, "leave_code", "leave_desc", "LEAVE_CODE");
            }
            else
            {
                string sql = @"
                select leave_code,leave_desc, 1 as sorter from hrucfleavecode where leave_desc = 'ลาชั่วโมง'
                union
                select '','',0 from dual order by sorter";
                DataTable dt = WebUtil.Query(sql);
                this.DropDownDataBind(dt, "leave_code", "leave_desc", "LEAVE_CODE");

            }
        }//คิวรี่ ลักษณะการลา

        public void DdHrucfleaveapvby()
        {
            string sql = @"
                select apv_bycode,apv_posdesc, 1 as sorter from hrucfleaveapvby 
                union
                select '','',0 from dual order by sorter";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "apv_bycode", "apv_posdesc", "APV_BYCODE");
        }//คิวรี่ ผู้อนุมัติ


    }
}