using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;

namespace Saving.Applications.hr.dlg.ws_dlg_hr_master_search_ctrl
{
    public partial class DsList : DataSourceRepeater
    {
        public DataSet1.Dt_ListDataTable DATA { get; set; }

        public void InitDsList(PageWeb pw)
        {
            css1.Visible = false;            
            DataSet1 ds = new DataSet1();
            this.DATA = ds.Dt_List;
            this.InitDataSource(pw, Repeater1, this.DATA, "dsList");
            this.EventClicked = "OnDsListClicked";
            this.EventItemChanged = "OnDsListItemChanged";
            this.Register();
        }
    }
}