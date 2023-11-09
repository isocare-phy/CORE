using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CoreSavingLibrary;
using DataLibrary;
using System.Data.OracleClient;

namespace Saving.Applications.hr.w_hr_apv_worktime_ctrl
{
    public partial class w_hr_apv_worktime : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public string PostWORKTIME { get; set; }
        public void InitJsPostBack()
        {
            dsSearch.InitMain(this);
            dsMain.InitDsDetail(this);
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                //dsMain.Visible = false;
            }
            else
            {
                //dsMain.Visible = false;
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            DateTime date = new DateTime();
            //DateTime date_end = new DateTime();

            date = dsSearch.DATA[0].WORK_DATES;

            if (eventArg == "PostWORKTIME")
            {
                dsMain.RetrieveLeaveDetail(date);
               // dsMain.DATA[0].APV_STATUS = "0";

                

               /* if (dsMain.DATA[0].APV_WORKTIME == "")
                {

                    dsMain.DATA[0].APV_WORKTIME = "ไม่ได้บันทึก";

                }
                else
                {
                    dsMain.DATA[0].APV_WORKTIME = dsMain.DATA[0].APV_WORKTIME;

                }*/
               
            }
        }

        public void SaveWebSheet()
        {

        }

        public void WebSheetLoadEnd()
        {
            dsSearch.ResetRow();
        }
    }
}