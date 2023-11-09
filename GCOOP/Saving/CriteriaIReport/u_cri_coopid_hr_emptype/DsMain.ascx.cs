using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;

namespace Saving.CriteriaIReport.u_cri_coopid_hr_emptype
{
    public partial class DsMain : DataSourceFormView
    {
        public DataSet1.DT_MAINDataTable DATA { get; set; }
        public void InitDsMain(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.DT_MAIN;
            this.EventItemChanged = "OnDsMainItemChanged";
            this.EventClicked = "OnDsMainClicked";
            this.InitDataSource(pw, FormView1, this.DATA, "dsMain");
            this.Register();
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
    }
}