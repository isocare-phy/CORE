using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;
namespace Saving.Applications.assist.ws_sheet_as_ucfassist_ctrl
{
    public partial class DsUcfasstype : DataSourceFormView
    {
        public DataSet1.DsUcfasstypeDataTable DATA { get; set; }
        public void InitDsUcfasstype(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.DsUcfasstype;
            this.EventItemChanged = "OnDsUcfasstypeItemChanged";
            this.EventClicked = "OnDsUcfasstypeClicked";
            this.InitDataSource(pw, FormView1, this.DATA, "dsUcfasstype");
            this.Button.Add("b_del");
            this.Register();
        }

        public void Retrieve(string ls_asscode)
        {
            string sql = "SELECT * FROM ASSUCFASSISTTYPE where COOP_ID = '" + state.SsCoopControl + "' and assisttype_code = '" + ls_asscode + "' ORDER BY ASSISTTYPE_CODE ";
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