using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Saving.Applications.shrlon
{
    public partial class w_sheet_sl_loan_requestment_lite:PageWebSheet, WebSheet
    {
        [JsPostBack]
        public string PostMemberNo { get; set; }

        public void InitJsPostBack()
        {
            dsMain.InitDsMain(this);
        }

        public void WebSheetLoadBegin()
        {
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == PostMemberNo)
            {
                string memb = dsMain.DATA[0].MEMBER_NO;
                //memb = WebUtil.MemberNoFormat(memb);

                dsMain.Retrieve(memb);
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