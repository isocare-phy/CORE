using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;

namespace Saving.Applications.assist.ws_as_assdetail_ctrl
{
    public partial class DsCont : DataSourceFormView
    {
        public DataSet1.ASSCONTMASTERDataTable DATA { get; set; }
        public void InitDsCont(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.ASSCONTMASTER;
            this.EventItemChanged = "OnDsContItemChanged";
            this.EventClicked = "OnDsContClicked";
            this.InitDataSource(pw, FormView1, this.DATA, "dsCont");
            this.Register();
        }

        public void RetrieveData(string as_asscontno)
        {
            string sql = @" select
		                        ast.assisttype_code|| ':' ||ast.assisttype_desc asstypedesc,	
		                        ass.assistpay_code || ':' || asp.assistpay_desc asspaydesc,
                                case when ass.payment_status = 1 then 'ปกติ' else 'งดจ่าย' end paystatdesc,
                                to_char( ass.cancel_date,'dd/mm/yyyy','NLS_CALENDAR=''THAI BUDDHA'' NLS_DATE_LANGUAGE=THAI') as cancel_tdate,
                                ass.*
                            from asscontmaster ass
		                        join assucfassisttype ast on ass.assisttype_code = ast.assisttype_code
		                        join assucfassisttypepay asp on ass.assisttype_code = asp.assisttype_code and ass.assistpay_code = asp.assistpay_code
                            where ass.coop_id={0} and ass.asscontract_no={1} ";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, as_asscontno);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
    }
}