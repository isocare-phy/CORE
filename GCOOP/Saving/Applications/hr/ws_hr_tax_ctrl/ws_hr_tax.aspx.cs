using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CoreSavingLibrary;

namespace Saving.Applications.hr.ws_hr_tax_ctrl
{
    public partial class ws_hr_tax : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public String PostEmpNo { get; set; }
        [JsPostBack]
        public String PostProgress { get; set; }

        public void InitJsPostBack()
        {
            dsMain.InitDsMain(this);
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {

            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "PostEmpNo")
            {
                string ls_empno = dsMain.DATA[0].EMP_NO;
                dsMain.Retrieve(ls_empno);
            }
            else if (eventArg == "PostProgress")
            {
                
            }
        }

        public void SaveWebSheet()
        {

        }

        public void WebSheetLoadEnd()
        {

        }
    }
}