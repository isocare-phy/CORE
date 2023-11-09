using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;

namespace Saving.Applications.assist.ws_wheet_as_request_ctrl
{
    public partial class DsEducation : DataSourceFormView
    {
        public DataSet1.ASSREQMASTERDataTable DATA { get; set; }
        public void InitDsEducation(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.ASSREQMASTER;
            this.EventItemChanged = "OnDsEducationItemChanged";
            this.EventClicked = "OnDsEducationClicked";
            this.InitDataSource(pw, FormView1, this.DATA, "dsEducation");
            this.Register();
        }

        public void Retrieve(String memno, String assisttype_code)
        {
            string sql = @"select assistpay_code child_level, assreqmaster.* from assreqmaster where req_status = 8 and coop_control={0} and member_no={1} and assisttype_code={2}";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, memno, assisttype_code);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }

        public void Retrieve2(String memno, String assist_docno)
        {
            string sql = @"select assistpay_code child_level, trim(child_prename_code)child_prename_code, assreqmaster.* from assreqmaster where req_status = 8 and coop_control={0} and member_no={1} and assist_docno={2}";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, memno, assist_docno);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }

        public void DdEducatLevel(String assisttype_code)
        {
            string sql = @"select * from
                        (
	                        select
		                        assucfassistpaytype.assistpay_code,
		                        assucfassistpaytype.assistpay_desc
	                        from assucfassisttypedet
	                        inner join assucfassistpaytype on assucfassisttypedet.assisttype_pay = assucfassistpaytype.assistpay_code
	                        where assucfassisttypedet.coop_id = {0} and assucfassisttypedet.assisttype_code = {1}
	                        union
	                        select
		                        '00',
		                        case assucfassistpaytype.assisttype_group when '01' then 'เลือกระดับชั้น' when '03' then 'เลือกผู้เกียวข้อง' when '04' then 'เลือกภัยพิบัติ' else '' end 
	                        from assucfassisttypedet
	                        inner join assucfassistpaytype on assucfassisttypedet.assisttype_pay = assucfassistpaytype.assistpay_code
	                        where assucfassisttypedet.coop_id = {0} and assucfassisttypedet.assisttype_code = {1} and rownum = 1
                        )order by assistpay_code";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, assisttype_code);
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "child_level", "assistpay_desc", "assistpay_code");
        }

        public void RetrievePrename()
        {
            string sql = @"select prename_code child_prename_code, prename_desc from mbucfprename order by prename_code";
            sql = WebUtil.SQLFormat(sql);
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "child_prename_code", "prename_desc", "child_prename_code");
        }
    }
}