using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;

namespace Saving.Applications.hr.ws_hr_late_ctrl
{
    public partial class DsLate : DataSourceRepeater//System.Web.UI.UserControl
    {
        public DataSet1.HRLOGLATEDataTable DATA { get; set; }
        public void InitDs(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.HRLOGLATE;
            this.EventItemChanged = "OnDsLateItemChanged";
            this.EventClicked = "OnDsLateClicked";

            this.InitDataSource(pw, Repeater1, this.DATA, "dsLate");

            //กรณีมีปุ่มให้ใช้คำสั่งนี้ add เพื่อให้เกิด event clicked ที่ปุ่ม
            //this.Button.Add("b_del");//ถ้าหน้าจอไม่มีปุ่มให้ลบหรือคอมเมนท์บรรทัดนี้

            this.Register();
        }
        
        public void RetrieveHrLogLate(string emp_no)
        {
            string sql = @"
                    SELECT seq_no,late_date,late_time,late_cause 
                    from hrloglate where emp_no={0} 
                    order by seq_no ASC";
            sql = WebUtil.SQLFormat(sql ,emp_no);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
    }
}