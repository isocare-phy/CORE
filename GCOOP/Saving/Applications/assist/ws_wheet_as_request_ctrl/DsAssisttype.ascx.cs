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
    public partial class DsAssisttype : DataSourceFormView
    {
        public DataSet1.DataMainDataTable DATA { get; set; }
        public void InitDsAssisttype(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.DataMain;
            this.EventItemChanged = "OnDsAssisttypeItemChanged";
            this.EventClicked = "OnDsAssisttypeClicked";
            this.InitDataSource(pw, FormView1, this.DATA, "dsAssisttype");
            this.Register();
        }
        public void AssistType()
        {
            string sql = @"select * from
                        (
	                        select
		                        ASSISTTYPE_CODE, 
		                        ASSISTTYPE_CODE||' - '||ASSISTTYPE_DESC as display, 
		                        1 as sorter
	                        from ASSUCFASSISTTYPE 
	                        union
	                        select
		                        '00', 
		                        'กรุณาเลือกสวัสดิการ' as display, 
		                        1 as sorter
	                        from ASSUCFASSISTTYPE where rownum = 1
                        )
                        order by sorter, assisttype_code
                        ";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "assisttype_code", "display", "assisttype_code");

        }
        public void GetAssYear()
        {
            string sql = @"select ass_year + 543 ass_show, ass_year from assucfyear order by ass_year desc";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "ass_year", "ass_show", "ass_year");
        }
    }
}