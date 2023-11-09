using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
//using CoreSavingLibrary.WcfNCommon;
//using CoreSavingLibrary.WcfNDeposit;
using CoreSavingLibrary.WcfNCommon;
using CoreSavingLibrary.WcfNDeposit;
using DataLibrary;
using CoreSavingLibrary;

namespace Saving.Applications.ap_deposit.dlg
{
    public partial class w_dlg_dp_printbook_int : PageWebDialog, WebDialog
    {
        #region WebDialog Members


        public void InitJsPostBack()
        {
           
        }

        public void WebDialogLoadBegin()
        {

            if (!IsPostBack)
            {

            }
            else { }
        }

        public void CheckJsPostBack(string eventArg)
        {

        }

        public void WebDialogLoadEnd()
        {
        }

        #endregion

        protected void btnCommit_Click(object sender, EventArgs e)
        {
            string dp_slip = Request["dp_slip"];
            string sql = @"select FTCNVTDATE(b.deptslip_date,4) as slip_date, a.deptclose_status as status,
                         a.deptaccount_no as deptaccount_no,b.int_amt as deptslip_amt,'-- '|| FT_READTBAHT(b.int_amt)||' --' as amt_th, 
                         'เลขที่บัญชี '||a.deptaccount_no || '  ชื่อบัญชี'||a.deptaccount_name as name_account,'ใบสำคัญจ่ายเงินดอกเบี้ยเงินฝาก '||c.depttype_desc as depttype,
                         b.int_amt as sum , 'ดอกเบี้ยจ่าย ' as description
                         from dpdeptmaster a,dpdeptslip b,dpdepttype c where  
                         a.deptaccount_no = b.deptaccount_no and a.depttype_code = c.depttype_code 
                         and a.coop_id = '" + state.SsCoopId + @"'  and b.int_amt > 0
                         and b.deptslip_no = '" + dp_slip + "'";
            DataTable dt = WebUtil.Query(sql);
            if (dt.Rows.Count > 0)
            {
                Printing.PrintApplet(this, "dept_printinterest", dt);
            }
                
        }
    }
}
