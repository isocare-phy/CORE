using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CoreSavingLibrary;
using DataLibrary;

namespace Saving.Applications.hr.ws_hr_edit_leave_ctrl
{
    public partial class DsLeave : DataSourceFormView
    {
        public DataSet1.HRLOGLEAVEDataTable DATA { get; set; }

        public void InitDsLeave(PageWeb pw)
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
            string sql = @"select coop_id,emp_no,seq_no,LEAVE_DATE,
                                  leave_code,leave_cause,
                                  leave_from,leave_to,totalday,
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
            string sql = @"
                select leave_code,leave_desc, 1 as sorter from hrucfleavecode where leave_code != '004'
                union
                select '','',0 from dual order by sorter";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "leave_code", "leave_desc", "LEAVE_CODE");
        }
    }
}