using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Saving.Applications.cmd.w_sheet_cmd_ucf_durtgrpcode_ctrl
{
    public partial class DsList : DataSourceRepeater
    {
        public DataSet1.ptucfdurtgrpcodeDataTable DATA { get; set; }

        public void InitDsList(PageWeb pw)
        {
            css1.Visible = false;

            DataSet1 ds = new DataSet1();
            this.DATA = ds.ptucfdurtgrpcode;
            this.InitDataSource(pw, Repeater1, this.DATA, "dsList");
            this.EventClicked = "OnDsListClicked";
            this.EventItemChanged = "OnItemDsListChanged";
            this.Button.Add("b_edit");
            this.Button.Add("b_del");
            this.Register();
        }

        public void RetrieveList()
        {
            String sql = @"
  select durtgrp_code,
durtgrp_desc,
devalue_percent,
durtgrp_abb
from ptucfdurtgrpcode
order by durtgrp_code asc ";
            sql = WebUtil.SQLFormat(sql);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }

    }
}