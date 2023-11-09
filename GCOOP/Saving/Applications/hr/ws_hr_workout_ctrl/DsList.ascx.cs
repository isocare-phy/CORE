using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CoreSavingLibrary;
using DataLibrary;

namespace Saving.Applications.hr.ws_hr_workout_ctrl
{
    public partial class DsList : DataSourceFormView
    {
        public DataSet1.HRW_OTDataTable DATA { get; set; }

        public void InitDsList(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.HRW_OT;
            this.EventItemChanged = "OnDsListItemChanged";
            this.EventClicked = "OnDsListClicked";
            this.InitDataSource(pw, FormView1, this.DATA, "dsList");
            this.Register();
        }

        public void Retrieveot(string seq_no, string emp_no)
        {
            string sql = @"select coop_id,emp_no,seq_no,date_work,
                                  trim(remark) as remark,work_in,
                                  work_out,ot_p,description
                            from hrw_ot
                            where coop_id={0} and seq_no ={1} and emp_no = {2}";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, seq_no, emp_no);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }

        public void RetrieveHr_Ot()
        {
            string sql = @"
                select seq_no,ottype, 1 as sorter from hrucfottype
                union
                select '','',0 from dual order by sorter";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "remark", "ottype", "seq_no");
        }

    }
}
    