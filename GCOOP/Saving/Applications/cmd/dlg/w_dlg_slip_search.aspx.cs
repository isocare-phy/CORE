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

namespace Saving.Applications.cmd
{
    public partial class w_dlg_slip_search : System.Web.UI.Page
    {
        private WebState state;
        private DwTrans sqlca;
        private string is_sql;


        protected void Page_Load(object sender, EventArgs e)
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

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            try
            {
                sqlca.Disconnect();
            }
            catch { }
        }
        protected void b_search_Click(object sender, EventArgs e)
        {
            String ls_sql = "", ls_sqlext = "", ls_temp = "";
            ls_sql = dw_detail.GetSqlSelect();
            ls_temp = ls_sql + ls_sqlext;
            HSqlTemp.Value = ls_sql;
            dw_detail.SetSqlSelect(HSqlTemp.Value);
            dw_detail.Retrieve();
        }
    }
}
