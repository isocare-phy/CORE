using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;

namespace Saving.Applications.hr.ws_hr_worktime_new_ctrl
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
            this.Register();
        }

        public void DdEmpno()
        {
            String sql = @"select hr.emp_no,
							hr.emp_no||' - '||mp.prename_desc||hr.emp_name||' '|| hr.emp_surname  as emp_name,
                            1 as sorter
                            from hremployee hr  left join mbucfprename mp on hr.prename_code  = mp.prename_code 
                            union 
                            select '','-กรุณาเลือกเลขพนักงาน-', 0 from dual
                            order by sorter";
            sql = WebUtil.SQLFormat(sql);
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "emp_no", "emp_name", "emp_no");
        }
        public void DdEmptype()
        {
            string sql = @"select emptype_code,emptype_desc from hrucfemptype";
            DataTable dt = WebUtil.Query(sql);
            //tomy ต่อ string โดยไมใช้ sql
            dt.Columns.Add("emptype_name", typeof(System.String));
            dt.Columns.Add("sort", typeof(System.Int32));
            foreach (DataRow row in dt.Rows)
            {
                string ls_grpcode = row["emptype_desc"].ToString().Trim();
                row["emptype_name"] = ls_grpcode;
                row["sort"] = 1;
            }
            dt.Rows.Add(new Object[] { "", "", "-กรุณาเลือกประเภทพนักงาน-", 0 });
            dt.DefaultView.Sort = "sort asc, emptype_code asc";
            dt = dt.DefaultView.ToTable();
            this.DropDownDataBind(dt, "emptype_code", "emptype_name", "emptype_code");
        }
        public void RetiveEmpno(string emptype_code)
        {
            String sql = @"select hr.emp_no,
							hr.emp_no||' - '||mp.prename_desc||hr.emp_name||' '|| hr.emp_surname  as emp_name,
                            1 as sorter
                            from hremployee hr  left join mbucfprename mp on hr.prename_code  = mp.prename_code 
                            where emptype_code ={0}
                            union 
                            select '','-กรุณาเลือกประเภทพนักงาน-', 0 from dual
                            order by sorter";
            sql = WebUtil.SQLFormat(sql, emptype_code);
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "emp_no", "emp_name", "emp_no");
        }
    }
}