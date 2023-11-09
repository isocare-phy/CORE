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
    public partial class DsDecease : DataSourceFormView
    {
        public DataSet1.ASSREQMASTERDataTable DATA { get; set; }
        public void InitDsDecease(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.ASSREQMASTER;
            this.EventItemChanged = "OnDsDeceaseItemChanged";
            this.EventClicked = "OnDsDeceaseClicked";            
            this.InitDataSource(pw, FormView1, this.DATA, "dsDecease");
            this.Register();
        }

        public void Retrieve(String memno, String assisttype_code)
        {
            string sql = @"select * from assreqmaster where req_status = 8 and coop_control={0} and member_no={1} and assisttype_code={2}";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, memno, assisttype_code);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }

        public void Retrieve2(String memno, String assist_docno)
        {
            string sql = @"select * from assreqmaster where req_status = 8 and coop_control={0} and member_no={1} and assist_docno={2}";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, memno, assist_docno);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
        
    }
}