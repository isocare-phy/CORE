using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;

namespace Saving.Applications.hr.ws_hr_payrolldet_ctrl
{
    public partial class ws_hr_payrolldet : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public String PostEmpNo { get; set; }
        [JsPostBack]
        public String PostRetrieve { get; set; }

        public void InitJsPostBack()
        {
            dsMain.InitDsMain(this);
            dsListExc.InitDsListExc(this);
            dsListInc.InitDsListInc(this);
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                NewClear();
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == PostEmpNo)
            {
                string ls_empno = dsMain.DATA[0].EMP_NO;
                dsMain.Retrieve(ls_empno);
            }
            else if (eventArg == PostRetrieve)
            {
                string ls_period = year.Text + Convert.ToDecimal(month.Text).ToString("00");
                string ls_empno = dsMain.DATA[0].EMP_NO;
                dsListInc.Retrieve(ls_empno, ls_period);
                dsListExc.Retrieve(ls_empno, ls_period);
            }
        }

        public void NewClear()
        {
            dsMain.ResetRow();

            year.Text = Convert.ToString(WebUtil.GetAccyear(state.SsCoopControl, state.SsWorkDate));
            month.Text = DateTime.Now.Month.ToString();
        }

        public void SaveWebSheet()
        {
            
        }

        public void WebSheetLoadEnd()
        {
            
        }
    }
}