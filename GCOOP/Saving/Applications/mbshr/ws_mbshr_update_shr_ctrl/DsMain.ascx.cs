using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;

namespace Saving.Applications.mbshr.ws_mbshr_update_shr_ctrl
{
    public partial class DsMain : DataSourceFormView
    {
        public DataSet1.DTMAINDataTable DATA { get; set; }

        public void InitDsMain(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.DTMAIN;
            this.EventItemChanged = "OnDsMainItemChanged";
            this.EventClicked = "OnDsMainClicked";
            this.InitDataSource(pw, FormView1, this.DATA, "dsMain");
            this.Register();
        }

        public void DdSMembgroupCode()
        {
            string sql = @"
            select membgroup_code,
                membgroup_desc
                , 1 as sorter
            from mbucfmembgroup
            where coop_id = {0}
            union
            select '','',0 from dual order by sorter , membgroup_code";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl);
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "smembgroup_code", "membgroup_desc", "membgroup_code");
        }

        public void DdEMembgroupCode()
        {
            string sql = @"
            select membgroup_code,
                membgroup_desc
                , 1 as sorter
            from mbucfmembgroup
            where coop_id = {0}
            union
            select '','',0 from dual order by sorter , membgroup_code";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl);
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "emembgroup_code", "membgroup_desc", "membgroup_code");
        }
    }
}