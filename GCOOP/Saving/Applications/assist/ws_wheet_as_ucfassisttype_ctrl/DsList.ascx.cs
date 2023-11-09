using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;

namespace Saving.Applications.assist.ws_wheet_as_ucfassisttype_ctrl
{
    public partial class DsList : DataSourceRepeater
    {
        public DataSet1.ASSUCFASSISTTYPEDataTable DATA { get; private set; }
        public void InitDs(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.ASSUCFASSISTTYPE;
            this.EventItemChanged = "OnDsListItemChanged";
            this.EventClicked = "OnDsListClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsList");
            this.Button.Add("b_del");
            this.Register();
        }

        public void Retrieve()
        {
            string sql = "SELECT * FROM ASSUCFASSISTTYPE where COOP_ID = '" + state.SsCoopControl + "' ORDER BY ASSISTTYPE_CODE ";
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
        public void RetriveGroup()
        {
            string sql = @"select assisttype_group,assisttype_group ||'-'|| assisttype_groupdesc as display from assucfassisttypegroup order by assisttype_group";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "ASSISTTYPE_GROUP", "display", "assisttype_group");
        }
    }    
}