using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;

namespace Saving.Applications.hr.ws_hr_worktime_ctrl
{
    public partial class DsMain : DataSourceFormView
    {
        public DataSet1.HRLOGWORKTIMEDataTable DATA { get; set; }
        public void InitDs(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.HRLOGWORKTIME;
            this.EventItemChanged = "OnDsMainItemChanged";
            this.EventClicked = "OnDsMainClicked";

            this.InitDataSource(pw, FormView1, this.DATA, "dsMain");

            //กรณีมีปุ่มให้ใช้คำสั่งนี้ add เพื่อให้เกิด event clicked ที่ปุ่ม
            //this.Button.Add("b_del");//ถ้าหน้าจอไม่มีปุ่มให้ลบหรือคอมเมนท์บรรทัดนี้

            this.Register();
        }
    }
}