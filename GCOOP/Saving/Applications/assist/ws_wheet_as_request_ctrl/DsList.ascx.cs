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
    public partial class DsList : DataSourceRepeater
    {
        public DataSet1.ASSREQMASTERDataTable DATA { get; set; }
        public void InitDsList(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.ASSREQMASTER;
            this.EventItemChanged = "OnDsListItemChanged";
            this.EventClicked = "OnDsListClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsList");
            this.Register();
        }

        public void RetrieveReq(String memno)
        {
            string sql = @"select
	                        mas.assist_docno,
	                        mas.req_date,
	                        mas.assisttype_code || ' ' || ucftype.assisttype_desc || ' : ' || paytype.assistpay_desc assisttype_code,
	                        mas.approve_amt,
	                        case when mas.ref_slipno is null then case mas.req_status when 1 then 'อนุมัติ' when 8 then 'รออนุมัติ' end else 'จ่าย' end req_statusdesc
                        from assreqmaster mas
                        inner join assucfassisttype ucftype on mas.assisttype_code = ucftype.assisttype_code
                        inner join assucfassistpaytype paytype on mas.assistpay_code = paytype.assistpay_code
                        where mas.req_status > 0 and mas.coop_control={0} and mas.member_no={1}
                        order by mas.req_date desc, mas.assisttype_code,  mas.assist_docno";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, memno);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }


        public void RetrieveReqAssist(String memno,String assisttype_code)
        {
            string sql = @"select
	                        mas.assist_docno,
	                        mas.req_date,
	                        mas.assisttype_code || ' ' || ucftype.assisttype_desc || ' : ' || paytype.assistpay_desc assisttype_code,
	                        mas.approve_amt,
	                        case when mas.ref_slipno is null then case mas.req_status when 1 then 'อนุมัติ' when 8 then 'รออนุมัติ' end else 'จ่าย' end req_statusdesc
                        from assreqmaster mas
                        inner join assucfassisttype ucftype on mas.assisttype_code = ucftype.assisttype_code
                        inner join assucfassistpaytype paytype on mas.assistpay_code = paytype.assistpay_code
                        where mas.req_status > 0 and mas.coop_control={0} and mas.member_no={1} and mas.assisttype_code={2}
                        order by mas.req_date desc, mas.assisttype_code,  mas.assist_docno";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, memno, assisttype_code);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
    }
}