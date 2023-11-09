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
    public partial class DsUcfasstypedet : DataSourceRepeater
    {
        public DataSet1.DsUcfasstypedetDataTable DATA { get; set; }
        public void InitDsUcfasstypedet(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.DsUcfasstypedet;
            this.EventItemChanged = "OnDsUcfasstypedetItemChanged";
            this.EventClicked = "OnDsUcfasstypedetClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsUcfasstypedet");
            this.Button.Add("b_del");
            this.Register();
        }

        public void AssistPayType(string ls_asscode)
        {
            string sql = @"select 
	                        assucfassistpaytype.assistpay_code assisttype_pay, 
	                        assistpay_code || ' - ' || assistpay_desc display, 
	                        1 as sorter from assucfassisttype 
                        inner join assucfassistpaytype on assucfassisttype.assisttype_group = assucfassistpaytype.assisttype_group
                        where assucfassisttype.coop_id= {0} and assucfassisttype.assisttype_code = {1}
                        union
                        select '','',0 from dual order by sorter, assisttype_pay";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, ls_asscode);
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "ASSISTTYPE_PAY", "display", "assisttype_pay");
        }
        public void Membertype()
        {
            string sql = @"select membtype_code,membtype_desc as display,1 as sorter from mbucfmembtype
                        union
                        select '','-กรุณาเลือก-',0 from dual order by sorter, membtype_code";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "membtype_code", "display", "membtype_code");
        }
        public void RetrieveData(string ls_asstypecode)
        {
            string sql = "select * from ASSUCFASSISTTYPEDET where assisttype_code='" + ls_asstypecode + "' and coop_id ='" + state.SsCoopControl + "' order by assisttype_pay,membtype_code,min_check,seq_no";
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
            AssistPayType(ls_asstypecode);
        }
    }
}