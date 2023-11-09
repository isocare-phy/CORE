using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Saving.Applications.cmd.w_sheet_cmd_ucf_durtgrpsubcode_ctrl
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


        public void retrieve(string durtgrp_code, string durtgrpsub_code)
        {
            String sql = @"SELECT  trim(ptucfdurtgrpsubcode.durtgrp_code) as durtgrp_code,  
         ptucfdurtgrpsubcode.durtgrpsub_code ,     
         ptucfdurtgrpsubcode.durtgrpsub_desc,
ptucfdurtgrpsubcode.durtgrpsub_abb ,
         ptucfdurtgrpsubcode.devalue_percent 
    FROM ptucfdurtgrpsubcode
where ptucfdurtgrpsubcode.durtgrp_code ={0}
and ptucfdurtgrpsubcode.durtgrpsub_code={1}
order by      ptucfdurtgrpsubcode.durtgrp_code,  ptucfdurtgrpsubcode.durtgrpsub_code   asc";
            sql = WebUtil.SQLFormat(sql, durtgrp_code, durtgrpsub_code);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
            this.Ddgroupcode();

        }

        public void RetrieveMain(string durtgrp_code)
        {
            String sql = @" select trim(devalue_percent ) as devalue_percent,
trim(durtgrp_code ) as durtgrp_code
from ptucfdurtgrpcode 
where durtgrp_code = {0} ";
            sql = WebUtil.SQLFormat(sql, durtgrp_code);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
            this.Ddgroupcode();
        }


        public void Ddgroupcode()
        {
            String sql = @" select m.* from
 ( select durtgrp_code,
durtgrp_desc,1 as sorter
from ptucfdurtgrpcode
union 
select '' as durtgrp_code,'' as durtgrp_desc,0 as sorter from dual) m order by m.sorter,m.durtgrp_code ";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "durtgrp_code", "durtgrp_desc", "durtgrp_code");
        }




        
    }
}