using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using DataLibrary;
using System.Data;

namespace Saving.Applications.hr.ws_hr_master_ctrl
{
    public partial class DsDisoffense : DataSourceRepeater
    {
        public DataSet1.HREMPLOYEEDISOFDataTable DATA { get; set; }

        public void InitDsDisoffense(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.HREMPLOYEEDISOF;
            this.EventItemChanged = "OnDsDisoffenseItemChanged";
            this.EventClicked = "OnDsDisoffenseClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsDisoffense");
            this.Button.Add("b_del");
            this.Register();
        }

        public void Retrieve(string as_empno)
        {
            string sql = @"  select hdis.coop_id,
                        hdis.emp_no,
                        hdis.seq_no,
                        hdis.disof_docno,
                        hdis.disof_date,
                        hdis.disof_inflic,
                        hdis.disof_remark,
                        hdis.disoffense_code
            from hremployeedisof hdis
            where hdis.coop_id = {0}
            and hdis.emp_no = {1}";

            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, as_empno);
            Sdt dt = WebUtil.QuerySdt(sql);
            this.ImportData(dt);
        }

        public void DdDisoffense()
        {
            string sql = @"select disoffense_code,
            disoffense_desc,
            1 as sorter
            from hrucfdisoffense
            union 
            select '','', 0 from dual
            order by sorter, disoffense_code";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "disoffense_code", "disoffense_desc", "disoffense_code");
        }
    }
}
