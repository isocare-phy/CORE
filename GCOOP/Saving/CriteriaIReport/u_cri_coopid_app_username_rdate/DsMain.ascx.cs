using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataLibrary;
using System.Data;

namespace Saving.CriteriaIReport.u_cri_coopid_app_username_rdate
{
    public partial class DsMain : DataSourceFormView
    {
        public DataSet1.AMSECUSERSDataTable DATA { get; set; }
        public void InitDsMain(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.AMSECUSERS;
            this.InitDataSource(pw, FormView1, this.DATA, "dsMain");
            this.EventItemChanged = "OnDsMainItemChanged";
            this.Register();
        }
        public void DdCoopId()
        {
            String sql = @"  SELECT COOP_ID,   
                                    COOP_NAME  
                               FROM CMCOOPMASTER ";
            sql = WebUtil.SQLFormat(sql);
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "coop_id", "coop_name", "coop_id");
        }
        /*public void DdCoopId()
        {
            String sql = @"  SELECT APPLICATION,   
                                    DESCRIPTION  
                               FROM AMAPPSTATUS ";
            sql = WebUtil.SQLFormat(sql);
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "application", "description", "application");
        }*/
    }
}