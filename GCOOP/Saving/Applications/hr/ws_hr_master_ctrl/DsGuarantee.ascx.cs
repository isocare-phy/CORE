using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CoreSavingLibrary;

namespace Saving.Applications.hr.ws_hr_master_ctrl
{
    public partial class DsGuarantee : DataSourceRepeater
    {
        public DataSet1.HRGARANTEEDataTable DATA { get; set; }

        public void InitDsGuarantee(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.HRGARANTEE;
            this.EventItemChanged = "OnDsGuaranteeChanged";
            this.EventClicked = "OnDsGuaranteeClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsGuarantee");
            this.Button.Add("b_del");
            this.Register();
        }

        public void RetrieveHrGuarantee(string emp_no)
        {
            string sql = @"select  hg.emp_no,hg.seq_no,
                            hg.garan_name,hg.garan_birth,
                            extract (year from (sysdate - hg.garan_birth) year to month) as garan_age,
                                hg.garan_work,hg.garan_tel
                                from 
                                    hrgarantee hg
                                where
                                    hg.emp_no={0}";
            sql = WebUtil.SQLFormat(sql, emp_no);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
    }
}