using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;

namespace Saving.Applications.assist.ws_as_request_ctrl
{
    public partial class DsBonus_instead : DataSourceRepeater
    {
        public DataSet1.ASSREQMASTERDataTable DATA { get; set; }
        public void InitDsBonus_instead(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.ASSREQMASTER;
            this.EventItemChanged = "OnDsBonus_insteadItemChanged";
            this.EventClicked = "OnDsBonus_insteadClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsBonus_instead");
            this.Button.Add("b_search");
            this.Button.Add("b_del");
            this.Register();
        }
    }
}