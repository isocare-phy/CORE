using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;

namespace Saving.Applications.assist.ws_as_assistpaygroup_ctrl
{
    public partial class ws_as_assistpaygroup : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public string PostSearch { get; set; }
        [JsPostBack]
        public string InitAsssistpaytype { get; set; }

        public void InitJsPostBack()
        {
            dsMain.InitDsMain(this);
            dsList.InitDsList(this);
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                dsMain.DdMoneyType();
                dsMain.DDAssisttype();
                dsMain.DATA[0].select_check = "0";
                dsMain.DATA[0].moneytype_code = "TRN";
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == PostSearch)
            {
                dsList.ResetRow();
                dsMain.DATA[0].select_check = "0";
                string sqlwhere = "";

                if (dsMain.DATA[0].moneytype_code != "")
                {
                    sqlwhere += " and ass.moneytype_code = '" + dsMain.DATA[0].moneytype_code + "' ";
                }
                else { sqlwhere += ""; }

                if (dsMain.DATA[0].assisttype_code != "")
                {
                    sqlwhere += " and ass.assisttype_code = '" + dsMain.DATA[0].assisttype_code + "' ";
                }
                else { sqlwhere += ""; }
                dsList.RetrieveList(sqlwhere);
            }
            else if (eventArg == InitAsssistpaytype)
            {
                string ls_minpaytype = "", ls_maxpaytype = "";
                if (dsMain.DATA[0].assisttype_code == "")
                {
                    dsMain.ResetRow();
                    dsMain.DDAssisttype();
                }
                else
                {
                    dsMain.AssistPayType(dsMain.DATA[0].assisttype_code, ref ls_minpaytype, ref ls_maxpaytype);
                    dsMain.DATA[0].assistpay_code1 = ls_minpaytype;
                    dsMain.DATA[0].assistpay_code2 = ls_maxpaytype;
                }
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