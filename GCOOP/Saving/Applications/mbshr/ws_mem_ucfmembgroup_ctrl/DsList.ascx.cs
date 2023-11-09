using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CoreSavingLibrary;

namespace Saving.Applications.mbshr.ws_mem_ucfmembgroup_ctrl
{
    public partial class DsList : DataSourceRepeater
    {
        public DataSet1.MBUCFMEMBGROUPDataTable DATA { get; set; }
        public void InitDsList(PageWeb pw)
        {
            css1.Visible = false;
            css2.Visible = false;

            DataSet1 ds = new DataSet1();
            this.DATA = ds.MBUCFMEMBGROUP;
            this.EventItemChanged = "OnDsListItemChanged";
            this.EventClicked = "OnDsListClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsList");
            this.Button.Add("b_delete");
            this.Button.Add("b_detail");
            this.Register();
        }
        public void RetrieveList()
        {
            string sql = @" ";
            sql = WebUtil.SQLFormat(sql, state.SsCoopId);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);

        }
    }
}