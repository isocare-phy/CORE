using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sybase.DataWindow;
using DataLibrary;

namespace Saving.Applications.investment.dlg
{
    public partial class w_dlg_open_loanreq : PageWebDialog, WebDialog
    {
        protected String jsPostSearch;
        protected String jsPostMemberno;
        protected String jsPostBlank;
        String pbl = "ln_req_loan.pbl";

        #region Websheet Members
        public void InitJsPostBack()
        {
            jsPostSearch = WebUtil.JsPostBack(this, "jsPostSearch");
            jsPostMemberno = WebUtil.JsPostBack(this, "jsPostMemberno");
            jsPostBlank = WebUtil.JsPostBack(this, "jsPostBlank");
        }

        public void WebDialogLoadBegin()
        {
            if (!IsPostBack)
            {
                DwFilter.InsertRow(0);
                DwUtil.RetrieveDataWindow(DwMain, pbl, null, "%", "%", "%");
            }
            else
            {
                this.RestoreContextDw(DwFilter);
                this.RestoreContextDw(DwMain);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "jsPostSearch":
                    Search();
                    break;
                case "jsPostMemberno":
                    GetMemberName();
                    break;
            }
        }

        public void WebDialogLoadEnd()
        {
            try
            {
                DwUtil.RetrieveDDDW(DwFilter, "loantype_code_1", pbl, null);
            }
            catch { }
            DwFilter.SaveDataCache();
            DwMain.SaveDataCache();
        }
        #endregion

        #region Function
        private void Search()
        {
            String loanrequest_docno = "%";
            String member_no = "%";
            String loantype_code = "%";
            try
            {
                loanrequest_docno = DwFilter.GetItemString(1, "loanrequest_docno") + "%";
            }
            catch
            {
                loanrequest_docno = "%";
            }
            try
            {
                member_no = DwFilter.GetItemString(1, "member_no") + "%";
            }
            catch
            {
                member_no = "%";
            }
            try
            {
                loantype_code = DwFilter.GetItemString(1, "loantype_code_1") + "%";
            }
            catch
            {
                loantype_code = "%";
            }

            DwUtil.RetrieveDataWindow(DwMain, pbl, null, loanrequest_docno, member_no, loantype_code);
        }

        private void GetMemberName()
        {
            String member_no = DwFilter.GetItemString(1, "member_no");
            String sqlselectmember = "select pre.prename_desc, mb.memb_name, pre.suffname_desc from lcmembmaster mb, lcucfprename pre " +
                "where mb.prename_code = pre.prename_code and member_no = '" + member_no + "'";
            Sdt member = WebUtil.QuerySdt(sqlselectmember);
            if (member.Next())
            {
                String member_name = member.GetString(0) + " " + member.GetString(1) + " " + member.GetString(2);
                DwFilter.SetItemString(1, "member_name", member_name);
            }
            else
            {
                DwFilter.SetItemNull(1, "member_no");
                DwFilter.SetItemNull(1, "member_name");
            }
        }
        #endregion
    }
}