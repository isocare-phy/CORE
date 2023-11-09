using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;

namespace Saving.Applications.assist.dlg.ws_dlg_as_assistpay_v2_ctrl
{
    public partial class DsLoan : DataSourceRepeater
    {
        public DataSet1.DsLoanDataTable DATA { get; set; }
        public void InitDsLoan(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.DsLoan;
            this.EventItemChanged = "OnDsLoanItemChanged";
            this.EventClicked = "OnDsLoanClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsLoan");
            this.Button.Add("b_delloan");          
            this.Register();
        }
//        public void RetrieveDetail(String ls_assistdocno)
//        {
//            string sql = @"select 
//                            mbucfprename.prename_desc || assreqgain.gain_name || '  ' || assreqgain.gain_surname gain_fullname ,
//                            ASSIST_AMT
//                        from assreqgain
//                        inner join mbucfprename on assreqgain.gainprename_code = mbucfprename.prename_code
//                        where assist_docno = {0} 
//                        order by seq_no";
//            sql = WebUtil.SQLFormat(sql, ls_assistdocno);
//            DataTable dt = WebUtil.Query(sql);
//            this.ImportData(dt);
//        }
    }
}