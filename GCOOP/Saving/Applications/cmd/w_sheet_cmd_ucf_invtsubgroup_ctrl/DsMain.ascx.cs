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
    public partial class DsMain : DataSourceFormView
    {
        

        public DataSet1.dsmainDataTable DATA { get; set; }

        public void InitDsMain(PageWeb pw)
        {
            css1.Visible = false;


            DataSet1 ds = new DataSet1();
            this.DATA = ds.dsmain;
            this.EventItemChanged = "OnDsMainItemChanged";
            this.EventClicked = "OnDsMainClicked";
            this.InitDataSource(pw, FormView1, this.DATA, "dsMain");
            this.Register();
        }


        public void retrieve(string invtgrp_code, string invtsubgrp_code)
        {
            String sql = @"   SELECT trim(ptucfinvtsubgroupcode.invtgrp_code) as invtgrp_code,   
         ptucfinvtsubgroupcode.invtsubgrp_desc,   
         ptucfinvtsubgroupcode.invtsubgrp_code  ,
ptucfinvtgroupcode.invtgrp_desc  
    FROM ptucfinvtsubgroupcode  
left  join  ptucfinvtgroupcode on ptucfinvtsubgroupcode.invtgrp_code=ptucfinvtgroupcode.invtgrp_code
   WHERE  ptucfinvtsubgroupcode.invtgrp_code ={0}
and ptucfinvtsubgroupcode.invtsubgrp_code  ={1}
order by  ptucfinvtsubgroupcode.invtgrp_code asc ";
            sql = WebUtil.SQLFormat(sql, invtgrp_code, invtsubgrp_code);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
            this.Ddgroupcode();
        }


        public void Ddgroupcode()
        {
            String sql = @" select m.* from
 ( select invtgrp_code,
invtgrp_desc,1 as sorter
from ptucfinvtgroupcode
union 
select '' as invtgrp_code,'' as invtgrp_desc,0 as sorter from dual) m order by m.sorter,m.invtgrp_code ";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "invtgrp_code", "invtgrp_desc", "invtgrp_code");
        }

        
    }
}