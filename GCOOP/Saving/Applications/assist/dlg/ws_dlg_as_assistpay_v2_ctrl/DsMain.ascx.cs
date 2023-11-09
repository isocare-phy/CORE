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
    public partial class DsMain : DataSourceFormView
    {
        public DataSet1.ASSREQMASTERDataTable DATA { get; set; }
        public void InitDsMain(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.ASSREQMASTER;
            this.EventItemChanged = "OnDsMainItemChanged";
            this.EventClicked = "OnDsMainClicked";
            this.InitDataSource(pw, FormView2, this.DATA, "dsMain");
            //this.Button.Add("b_searchdeptno");
            //this.Button.Add("b_contsearch");
            this.Register();
        }

        public void RetrieveDetail(String ls_assistdocno)
        {
            string sql = @"
                         select 
	                        mbucfprename.prename_desc ||mbmembmaster.memb_name || '  ' || mbmembmaster.memb_surname as full_name,
	                        mbucfmembgroup.membgroup_desc,
	                        mbmembmaster.membgroup_code,
	                        assucfassisttype.assisttype_code,                            
	                        assucfassisttype.assisttype_code  || ':' || assucfassisttype.assisttype_desc assisttype_desc,	
	                        assucfassisttype.stm_flag,
	                        mbucfmembtype.membtype_code || ':' || mbucfmembtype.membtype_desc  as member_type,
	                        assreqmaster.assist_docno,   
	                        assreqmaster.member_no,
	                        assreqmaster.approve_amt,
	                        assreqmaster.assist_amt,
	                        assreqmaster.req_status,
	                        assreqmaster.assistpay_code,
	                        assreqmaster.assistpay_code || ':' || assucfassistpaytype.assistpay_desc assistpay_desc,
                            assreqmaster.assist_year - 543 assist_year
                        from mbmembmaster
                        inner join mbucfprename on mbmembmaster.prename_code = mbucfprename.prename_code
                        inner join mbucfmembgroup on mbmembmaster.membgroup_code = mbucfmembgroup.membgroup_code
                        inner join assreqmaster on assreqmaster.member_no = mbmembmaster.member_no
                        inner join assucfassisttype on assreqmaster.assisttype_code = assucfassisttype.assisttype_code
                        inner join assucfassistpaytype on assreqmaster.assistpay_code = assucfassistpaytype.assistpay_code
                        inner join mbucfmembtype  on mbucfmembtype.membtype_code = mbmembmaster.membtype_code
                        where assreqmaster.req_status = 1  and  
                        assreqmaster.coop_id = {0} and 
                        assreqmaster.assist_docno = {1}
                         ";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, ls_assistdocno);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
    }
}