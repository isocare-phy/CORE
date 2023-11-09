using System;
using CoreSavingLibrary;
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
using DataLibrary;
using Sybase.DataWindow;
using System.Web.Services.Protocols;
namespace Saving.Applications.app_assist.dlg
{
    public partial class w_dlg_as_memberdead_acc_list : PageWebDialog, WebDialog
    {
        #region WebDialog Members

        protected String postFuncSearch;
        protected String postCapitalYear;
        protected String postMemberNo;
        protected String postChildCardPerson;


        public void InitJsPostBack()
        {
            postFuncSearch = WebUtil.JsPostBack(this, "postFuncSearch");
            postMemberNo = WebUtil.JsPostBack(this, "postMemberNo");
        }


        public void WebDialogLoadBegin()
        {
            state = new WebState(Session, Request);
            sqlca = new DwTrans();
            sqlca.Connect();
            dw_detail.SetTransaction(sqlca);


            if (!IsPostBack)
            {
                dw_main.InsertRow(0);
            }
            else
            {
                this.RestoreContextDw(dw_main);
                this.RestoreContextDw(dw_detail);
            }
            if (!HfSearchValue.Value.Equals(""))
            {
                dw_detail.SetSqlSelect(HfSearchValue.Value);
                dw_detail.Retrieve();
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postMemberNo")
            {

                BtnSearch();
            }
            else if (eventArg == "postFuncSearch")
            {
                BtnSearch();
            }
        }


        public void WebDialogLoadEnd()
        {
            dw_main.SaveDataCache();
            dw_detail.SaveDataCache();
        }
        protected void Page_LoadComplete()
        {
            try
            {
                sqlca.Disconnect();
            }
            catch { }
        }

        private void BtnSearch()
        {
            String ls_capital_year;
            String ls_member_no;
            String ls_sql = "", ls_sqlext = "", ls_temp = "";
            ls_sql = dw_detail.GetSqlSelect();
            try
            {
                ls_member_no = WebUtil.MemberNoFormat(dw_main.GetItemString(1, "member_no")).Trim();
            }
            catch
            {
                ls_member_no = "";
            }
            try
            {
                ls_capital_year = dw_main.GetItemString(1, "capital_year");
            }
            catch { ls_capital_year = ""; }
            if (ls_capital_year.Length > 0)
            {
                ls_sqlext = "and (asnreqmaster.capital_year = '" + ls_capital_year + "')";
            }
            if (ls_member_no.Length > 0)
            {
                ls_sqlext += "and (asnreqmaster.member_no like '" + ls_member_no + "%')";
            }
            ls_temp = ls_sql + ls_sqlext;
            HfSearchValue.Value = ls_temp;
            dw_detail.SetSqlSelect(HfSearchValue.Value);
            dw_detail.Retrieve();
        }
        #endregion
    }
}

