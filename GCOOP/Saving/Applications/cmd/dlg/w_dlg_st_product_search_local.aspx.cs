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

    public partial class w_dlg_st_product_search_local : PageWebDialog, WebDialog
    {
        #region WebDialog Members

        public void InitJsPostBack()
        {

        }

        public void WebDialogLoadBegin()
        {
            this.ConnectSQLCA();

            if (!IsPostBack)
            {
                Dw_main.InsertRow(0);
                //dw_detail.InsertRow(0);
                dw_detail.SetTransaction(sqlca);
                dw_detail.Retrieve();
            }
            else
            {
                this.RestoreContextDw(Dw_main);
                this.RestoreContextDw(dw_detail);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {

        }

        public void WebDialogLoadEnd()
        {

        }

        #endregion

        #region Function

        protected void b_search_Click(object sender, EventArgs e)
        {
            String ls_sql = "", ls_sqlext = "", ls_temp = "";

            ls_sql = dw_detail.GetSqlSelect();
            ls_temp = ls_sql + ls_sqlext;
            HSqlTemp.Value = ls_sql;
            //dw_detail.SetSqlSelect(HSqlTemp.Value);
            dw_detail.SetTransaction(sqlca);
            dw_detail.Retrieve();
        }

        #endregion
    }
}
