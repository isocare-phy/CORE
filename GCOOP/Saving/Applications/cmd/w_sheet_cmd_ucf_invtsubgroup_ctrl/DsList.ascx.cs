using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Saving.Applications.cmd.w_sheet_cmd_ucf_invtsubgroup_ctrl
{
    public partial class DsList : DataSourceRepeater
    {
        public DataSet1.dsmainDataTable DATA { get; set; }

        public void InitDsList(PageWeb pw)
        {
            css1.Visible = false;

            DataSet1 ds = new DataSet1();
            this.DATA = ds.dsmain;
            this.InitDataSource(pw, Repeater1, this.DATA, "dsList");
            this.EventClicked = "OnDsListClicked";
            this.EventItemChanged = "OnItemDsListChanged";
            this.Button.Add("b_edit");
            this.Button.Add("b_del");
            this.Register();
        }

        public void RetrieveList(string invtgrp_code)
        {
            String sql = @"   SELECT ptucfinvtsubgroupcode.invtgrp_code,   
         ptucfinvtsubgroupcode.invtsubgrp_desc,   
         ptucfinvtsubgroupcode.invtsubgrp_code  
    FROM ptucfinvtsubgroupcode  
   WHERE  ptucfinvtsubgroupcode.invtgrp_code ={0}
order by  ptucfinvtsubgroupcode.invtgrp_code,ptucfinvtsubgroupcode.invtsubgrp_code  asc ";
            sql = WebUtil.SQLFormat(sql, invtgrp_code);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }

    }
}