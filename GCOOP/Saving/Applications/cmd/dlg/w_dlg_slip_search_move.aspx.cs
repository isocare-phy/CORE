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
using CoreSavingLibrary.WcfNCommon;
namespace Saving.Applications.cmd.dlg
{
    public partial class w_dlg_slip_search_move : PageWebDialog, WebDialog
    {
        private WebState state;
        private DwTrans sqlca;
        private String is_sql;
        private String pbl = "pointofsale_move";

        //    protected void Page_Load(object sender, EventArgs e)
        //    {
        //        state = new WebState(Session, Request);
        //        if (state.IsLogin)
        //        {
        //            sqlca = new DwTrans();
        //            sqlca.Connect();
        //            Dw_main.SetTransaction(sqlca);
        //            dw_detail.SetTransaction(sqlca);
        //            is_sql = dw_detail.GetSqlSelect();

        //            try
        //            {
        //                if (!IsPostBack)
        //                {
        //                    Dw_main.InsertRow(0);
        //                }
        //                else { }
        //            }
        //            catch { }
        //        }
        //        else
        //        {

        //        }
        //    }

        //    protected void Page_LoadComplete(object sender, EventArgs e)
        //    {
        //        try
        //        {
        //            sqlca.Disconnect();
        //        }
        //        catch { }
        //    }
        //    protected void b_search_Click(object sender, EventArgs e)
        //    {
        //        String ls_slip_id, ls_sliptype_id, ls_start_tdate, ls_ref_no, ls_entry_id, ls_issue_state;
        //        String ls_sqlext = "", ls_temp = "";
        //        try
        //        {
        //            ls_slip_id = Dw_main.GetItemString(1, "slip_id");
        //        }
        //        catch { ls_slip_id = ""; }

        //        if (ls_slip_id.Length > 0)
        //        {
        //            ls_sqlext = "AND SLIP_ID = '" + ls_slip_id + "'";
        //        }
        //        else { ls_sqlext = ""; }

        //        ls_temp = is_sql + ls_sqlext;
        //        HSqlTemp.Value = ls_temp;
        //        dw_detail.SetSqlSelect(HSqlTemp.Value);
        //        dw_detail.Retrieve();

        //    }

        #region Pagedialog Member

        public void InitJsPostBack()
        {

        }

        public void WebDialogLoadBegin()
        {
            state = new WebState(Session, Request);
            if (state.IsLogin)
            {
                sqlca = new DwTrans();
                sqlca.Connect();
                Dw_main.SetTransaction(sqlca);
                dw_detail.SetTransaction(sqlca);
                is_sql = dw_detail.GetSqlSelect();

                try
                {
                    if (!IsPostBack)
                    {
                        Dw_main.InsertRow(0);
                        HSqlTemp.Value = is_sql;
                    }

                    dw_detail.SetSqlSelect(HSqlTemp.Value);
                    dw_detail.Retrieve();

                }
                catch { }


            }
            else
            {

            }
        }

        public void CheckJsPostBack(string eventArg)
        {

        }

        public void WebDialogLoadEnd()
        {
            try { sqlca.Disconnect(); }
            catch (Exception ex) { ex.ToString(); }
        }

        #endregion

        #region Function

        protected void b_search_Click(object sender, EventArgs e)
        {
            try
            {
                String ls_slip_id, ls_sliptype_id, ls_ref_no, ls_entry_id, ls_issue_state;
                String ls_sqlext = "", ls_temp = "";
                DateTime ls_start_date;
                try { ls_slip_id = Dw_main.GetItemString(1, "slip_id"); }
                catch { ls_slip_id = ""; }

                try { ls_sliptype_id = Hdsliptype_id.Value; /* Dw_main.GetItemString(1, "sliptype_id"); */}
                catch { ls_sliptype_id = "0"; }
                //if (ls_sliptype_id == "0") { ls_sliptype_id = ""; }

                try { ls_start_date = Dw_main.GetItemDateTime(1, "start_date"); }
                catch { ls_start_date = state.SsWorkDate; }

                try { ls_ref_no = Dw_main.GetItemString(1, "ref_no"); }
                catch { ls_ref_no = ""; }

                try { ls_entry_id = Dw_main.GetItemString(1, "entry_id"); }
                catch { ls_entry_id = ""; }

                try { ls_issue_state = Dw_main.GetItemString(1, "issue_state"); }
                catch { ls_issue_state = ""; }

                if (ls_sliptype_id.Length > 0)
                {
                    ls_sqlext = "AND PTSLPSLIPMAS.SLIPTYPE_ID LIKE '%" + ls_sliptype_id + "%'";
                }

                if (ls_slip_id.Length > 0)
                {
                    ls_sqlext += "AND PTSLPSLIPMAS.SLIP_ID = '" + ls_slip_id + "'";
                }

                if (ls_ref_no.Length > 0)
                {
                    ls_sqlext += "AND PTSLPSLIPMAS.REF_NO LIKE '%" + ls_ref_no + "%'";
                }

                if (ls_entry_id.Length > 0)
                {
                    ls_sqlext += "AND PTSLPSLIPMAS.ENTRY_ID LIKE '%" + ls_entry_id + "%'";
                }


                ls_temp = is_sql + ls_sqlext;
                HSqlTemp.Value = ls_temp;
                dw_detail.SetSqlSelect(HSqlTemp.Value);
                dw_detail.Retrieve();
            }
            catch (Exception ex) { ex.ToString(); }
        }

        #endregion
    }
}
