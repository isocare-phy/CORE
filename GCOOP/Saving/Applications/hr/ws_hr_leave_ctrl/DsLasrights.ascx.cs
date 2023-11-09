using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataLibrary;
using CoreSavingLibrary;
using System.Data;

namespace Saving.Applications.hr.ws_hr_leave_ctrl
{
    public partial class DsLasrights : DataSourceFormView//System.Web.UI.UserControl
    {
        public DataSet1.HRLOGLEAVEDataTable DATA { get; set; }
        public void InitDs(PageWeb pw)
        {

            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.HRLOGLEAVE;
            this.EventItemChanged = "OnDsLasrightsItemChanged";
            this.EventClicked = "OnDsLasrightsClicked";
            this.InitDataSource(pw, FormView1, this.DATA, "dsLasrights");
            this.Register();
        }

        public void Retrieveleavemax(string emp_no)
        {
            string sql = @"select hc.leave_desc,
                            hl.leave_from,
                            hl.leave_to,
                            hl.totalday
                    from hrlogleave hl,hrucfleavecode hc
                    where hl.leave_code = hc.leave_code
                            and hl.coop_id = {0}  
                            and hl.emp_no = {1}
                            and hl.seq_no = (select max(seq_no)
                                            from hrlogleave 
                                            where coop_id = {0} 
                                            and emp_no = {1})";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, emp_no);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
    }
}