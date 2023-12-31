﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CoreSavingLibrary;

namespace Saving.Applications.mbshr.ws_mbshr_transfer_new_ctrl
{
    public partial class DsMain : DataSourceFormView
    {
        public DataSet1.DataTable2DataTable DATA { get; set; }

        public void InitDsMain(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.DataTable2;
            this.EventItemChanged = "OnDsMainItemChanged";
            this.EventClicked = "OnDsMainClicked";
            this.InitDataSource(pw, FormView1, this.DATA, "dsMain");
            this.Button.Add("b_search");
            this.Register();
        }
        public void RetrieveMain(String member_no)
        {
            String sql = @"    select mbmembmaster.coop_id,   
                             mbmembmaster.member_no,   
                             mbmembmaster.prename_code,        
                             mbmembmaster.member_type,     
                          	(trim(mbmembmaster.membgroup_code)||''||'-'||trim(mbucfmembgroup.membgroup_desc)) as membgroup_desc,
                    (trim(mbucfprename.prename_desc)||trim(mbmembmaster.memb_name)   
                             ||' '||trim(mbmembmaster.memb_surname) ) as MEMB_NAME
                        from mbmembmaster
        left join mbucfmembgroup on mbmembmaster.coop_id = mbucfmembgroup.coop_id and mbmembmaster.membgroup_code = mbucfmembgroup.membgroup_code
        left join mbucfprename on mbmembmaster.prename_code = mbucfprename.prename_code                    
                       where (mbmembmaster.member_no = {1}) and ( mbmembmaster.coop_id = {0} )";
            sql = WebUtil.SQLFormat(sql,state.SsCoopId,member_no);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
    }
}