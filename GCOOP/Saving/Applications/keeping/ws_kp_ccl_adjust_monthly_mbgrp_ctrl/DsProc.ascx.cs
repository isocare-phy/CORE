using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;

namespace Saving.Applications.keeping.ws_kp_ccl_adjust_monthly_mbgrp_ctrl
{
    public partial class DsProc : DataSourceFormView
    {
        public DataSet1.DT_PROCDataTable DATA { get; private set; }

        public void InitDsProc(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.DT_PROC;
            this.EventItemChanged = "OnDsProcItemChanged";
            this.EventClicked = "OnDsProcClicked";
            this.InitDataSource(pw, FormView1, this.DATA, "dsProc");
            this.Button.Add("b_process");
            this.Register();
        }

        public void DdMembgroup()
        {
            string sql = @"
                select membgroup_code,membgroup_code||' '||membgroup_desc as display,1 as sorter from mbucfmembgroup 
                union
                select '','',0 from dual
                order by sorter,membgroup_code";
            sql = WebUtil.SQLFormat(sql);
            this.DropDownDataBind(sql, "membgroup_desc", "display", "membgroup_code");
        }

        public void DdSlipcause()
        {
            string sql = @"SELECT coop_id, slipretcause_code, slipretcause_desc
                            FROM slucfslipreturncause  
                            WHERE coop_id = {0} 
                            order by slipretcause_code"; 
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl);
            sql = WebUtil.SQLFormat(sql);
            this.DropDownDataBind(sql, "slipretcause_desc", "slipretcause_desc", "slipretcause_code");
        }
    }
}