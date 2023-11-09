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
namespace Saving.Applications.cmd.dlg
{
    public partial class w_dlg_slip_searchimproduct : PageWebDialog, WebDialog
    {

        private WebState state;
        private DwTrans sqlca;
        private string is_sql;

        #region WebDialog Member

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
                }
                catch { }
                dw_detail.SetSqlSelect(HSqlTemp.Value);
                dw_detail.Retrieve();
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
            String ls_sql = "", ls_sqlext = "", ls_temp = "";
            String ls_slip_id = "", ls_entry_id = "";

            try { ls_slip_id = Dw_main.GetItemString(1, "slip_id"); }
            catch { ls_slip_id = ""; }
            try { ls_entry_id = Dw_main.GetItemString(1, "entry_id"); }
            catch { ls_entry_id = ""; }

            if (ls_slip_id.Length > 0) { ls_sqlext += "AND PTSLPSLIPMAS.SLIP_ID = '" + ls_slip_id + "'"; }
            if (ls_entry_id.Length > 0) { ls_sqlext += "AND PTSLPSLIPMAS.ENTRY_ID LIKE '%" + ls_entry_id + "%'"; }

            ls_sql = dw_detail.GetSqlSelect();
            ls_temp = ls_sql + ls_sqlext;
            HSqlTemp.Value = ls_temp;
            dw_detail.SetSqlSelect(HSqlTemp.Value);
            dw_detail.Retrieve();
        }

        #endregion
    }
}
