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
    public partial class DsMain : DataSourceFormView
    {
        public DataSet1.HREMPLOYEEDataTable DATA { get; set; }
        public void InitDs(PageWeb pw)
        {
            //ปิด css1 ตอนรันไทม์ เปลี่ยนไปใช้ css ของเฟรมแทน
            css1.Visible = false;
            //ประกาศ dataset
            DataSet1 ds = new DataSet1();
            //ตั้งค่า property DATA เอา DataTable จาก DataSet มาใช้
            this.DATA = ds.HREMPLOYEE;
            //ตั้งชื่อ event ItemChanged ในรูปแบบ On<ชื่อ WebUserControl>ItemChanged
            this.EventItemChanged = "OnDsMainItemChanged";
            //ตั้งชื่อ event ItemChanged ในรูปแบบ On<ชื่อ WebUserControl>ItemChanged
            this.EventClicked = "OnDsMainClicked";

            //เรียกคำสั่งในคลาสแม่ให้การ init ก่อน
            //argument ที่ 2 ใส่ object ของ WebUserControl 
            //argument ที่ 4 ใส่ชื่อขึ้นต้นตัวเล็ก)
            this.InitDataSource(pw, FormView1, this.DATA, "dsMain");

            //กรณีมีปุ่มให้ใช้คำสั่งนี้ add เพื่อให้เกิด event clicked ที่ปุ่ม
            //this.Button.Add("b_del");//ถ้าหน้าจอไม่มีปุ่มให้ลบหรือคอมเมนท์บรรทัดนี้

            //เรียกให้คลาสแม่ทำงานอีกครั้ง (เป็นคำสั่งการรวม object ต่างๆ เข้าด้วยกัน)
            this.Register();
        }

        public void RetrieveEmp(string emp_no)
        {
            string sql = @"
                select he.emp_no,he.salary_id,hd.deptgrp_desc,mp.prename_desc,he.emp_name,he.emp_surname,hp.pos_desc 
                from hremployee he,mbucfprename mp,hrucfposition hp,hrucfdeptgrp hd
                where he.emp_no={0} and he.coop_id={1}
                and he.prename_code=mp.prename_code
                and he.deptgrp_code = hd.deptgrp_code
                and he.pos_code=hp.pos_code";
            sql = WebUtil.SQLFormat(sql, emp_no, state.SsCoopId);//กรณี มีการ join ข้อมูล ทำให้ข้อมูลเชื่อมต่อกัน
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);

        }
    }
}