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
    public partial class DsUcfasspaytype : DataSourceRepeater
    {
        public DataSet1.DsUcfasspaytypeDataTable DATA { get; set; }
        public void InitDsUcfasspaytype(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.DsUcfasspaytype;
            this.EventItemChanged = "OnDsUcfasspaytypeChanged";
            this.EventClicked = "OnDsUcfasspaytypeClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsUcfasspaytype");
            this.Button.Add("b_del");
            this.Register();
        }

        public void Retrieve(string ls_asscode)
        {
            string sql = @"SELECT ASSUCFASSISTPAYTYPE.* 
                FROM ASSUCFASSISTPAYTYPE 
                inner join ASSUCFASSISTTYPE on ASSUCFASSISTPAYTYPE.assisttype_group = ASSUCFASSISTTYPE.assisttype_group
                where ASSUCFASSISTPAYTYPE.COOP_ID = {0} and ASSUCFASSISTTYPE.assisttype_code = {1}
                ORDER BY ASSUCFASSISTPAYTYPE.ASSISTPAY_CODE";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, ls_asscode);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
        public void RetriveGroup()
        {
            string sql = @"select assisttype_group,assisttype_group ||'-'|| assisttype_groupdesc as display from assucfassisttypegroup";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "ASSISTTYPE_GROUP", "display", "assisttype_group");
        }
        
    }
}