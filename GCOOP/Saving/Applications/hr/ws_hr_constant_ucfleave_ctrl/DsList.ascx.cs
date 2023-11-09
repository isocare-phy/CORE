﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CoreSavingLibrary;

namespace Saving.Applications.hr.ws_hr_constant_ucfleave_ctrl
{
    public partial class DsList : DataSourceRepeater
    {
        public DataSet1.HRUCFLEAVECODEDataTable DATA { get; set; }

        public void InitDsList(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.HRUCFLEAVECODE;
            this.EventItemChanged = "OnDsListItemChanged";
            this.EventClicked = "OnDsListClicked";
            this.InitDataSource(pw,Repeater1,this.DATA,"dsList");
            this.Button.Add("b_delete");
            this.Register();
        }
        public void Retrieve()
        {
            string sql = @"
                select leave_code,leave_desc from hrucfleavecode";
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
    }
}