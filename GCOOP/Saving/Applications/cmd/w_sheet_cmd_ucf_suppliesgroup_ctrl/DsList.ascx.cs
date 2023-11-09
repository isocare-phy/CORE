using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Saving.Applications.cmd.w_sheet_cmd_ucf_suppliesgroup_ctrl
{
    public partial class DsList : DataSourceRepeater
    {
        public DataSet1.groupcodeDataTable DATA { get; set; }

        public void InitDsList(PageWeb pw)
        {
            css1.Visible = false;

            DataSet1 ds = new DataSet1();
            this.DATA = ds.groupcode;
            this.InitDataSource(pw, Repeater1, this.DATA, "dsList");
            this.EventClicked = "OnDsListClicked";
            this.EventItemChanged = "OnItemDsListChanged";
            this.Button.Add("b_del");
            this.Button.Add("b_edit");
            this.Register();
        }

        public void RetrieveList()
        {
            String sql = @"
select 
invtgrp_code,
invtgrp_desc
 from  ptucfinvtgroupcode
order by  invtgrp_code asc";
            sql = WebUtil.SQLFormat(sql);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }

    }
}