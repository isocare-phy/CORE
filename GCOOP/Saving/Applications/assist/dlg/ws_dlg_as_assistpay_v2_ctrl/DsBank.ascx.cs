using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


namespace Saving.Applications.assist.dlg.ws_dlg_as_assistpay_v2_ctrl
{
    public partial class DsBank : DataSourceFormView
    {
        public DataSet1.DsBankDataTable DATA { get; set; }
        public void InitDsBank(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.DsBank;
            this.EventItemChanged = "OnDsBankItemChanged";
            this.EventClicked = "OnDsBankClicked";
            this.InitDataSource(pw, FormView2, this.DATA, "dsBank");
            //this.Button.Add("b_searchdeptno");
            //this.Button.Add("b_contsearch");
            this.Register();
        }

        public void retrieve()
        {
            string sql = @"
            select  account_id,trim(account_id)|| '-'||trim(account_name)  as bank_desc, 1 as sorter from accmaster where trim(account_type_id) ='3'and active_status='1'
            union 
            select '','---กรุณาเลือกธนาคาร---', 0 from dual order by sorter";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "accaccount", "bank_desc", "account_id");

        }
    }

}