using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;

namespace Saving.Applications.assist.ws_sheet_as_assistpay_ctrl
{
    public partial class ws_sheet_as_assistpay : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public string PostSearch { get; set; }
        public void InitJsPostBack()
        {
            dsMain.InitDsMain(this);
            dsList.InitDsList(this);
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                dsMain.DDAssisttype();
                dsMain.DATA[0].select_check = "0";
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == PostSearch)
            {
                dsList.ResetRow();
                dsMain.DATA[0].select_check = "0";
                string sqlwhere = "";

                if (dsMain.DATA[0].member_no != "")
                {
                    String ls_memno = WebUtil.MemberNoFormat(dsMain.DATA[0].member_no);
                    sqlwhere += " and assreqmaster.member_no like '%" + ls_memno + "%' ";
                }
                else { sqlwhere += ""; }

                if (dsMain.DATA[0].assisttype_code != "")
                {
                    sqlwhere += " and assreqmaster.assisttype_code = '" + dsMain.DATA[0].assisttype_code + "' ";
                }
                else { sqlwhere += ""; }
                dsList.RetrieveList(sqlwhere);
            }
        }

        public void SaveWebSheet()
        {
            throw new NotImplementedException();
        }

        public void WebSheetLoadEnd()
        {
            throw new NotImplementedException();
        }
    }
}