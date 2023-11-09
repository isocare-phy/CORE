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

namespace Saving.Applications.investment.dlg
{
    public partial class w_dlg_organize_search_loancont : System.Web.UI.Page
    {
        WebState state;
        DwTrans SQLCA;
        protected void Page_Load(object sender, EventArgs e)
        {
            state = new WebState(Session, Request);
            SQLCA = new DwTrans();
            SQLCA.Connect();
            dw_data.SetTransaction(SQLCA);
            dw_detail.SetTransaction(SQLCA);


            if (!IsPostBack)
            {
                dw_data.InsertRow(0);
            }
            if (!hidden_search.Value.Equals(""))
            {
                dw_detail.SetSqlSelect(hidden_search.Value);
                dw_detail.Retrieve();
            }

        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            SQLCA.Disconnect();
        }

        protected void cb_find_Click(object sender, EventArgs e)
        {

            String ls_memno = "", ls_loancontract = "", ls_type = "";
            String ls_sql = "", ls_sqlext = "", ls_temp = "";

            ls_sql = dw_detail.GetSqlSelect();
            
            try
            {
                ls_memno = dw_data.GetItemString(1, "org_memno").Trim();

            }
            catch { ls_memno = ""; }
            try
            {
                ls_loancontract = dw_data.GetItemString(1, "loancontract_no").Trim();

            }
            catch { ls_loancontract = ""; }
            try
            {
                ls_type = dw_data.GetItemString(1, "org_type").Trim();

            }
            catch { ls_type = ""; }

           

            if (ls_memno.Length > 0)
            {
                ls_sqlext = " and (  olorgmaster.org_memno like '%" + ls_memno + "%') ";
            }
            if (ls_loancontract.Length > 0)
            {
                ls_sqlext += " and ( olcontmaster.loancontract_no like '" + ls_loancontract + "%') ";
            }
            if (ls_type.Length > 0)
            {
                ls_sqlext += " and ( olcontmaster.orgtype_code like '" + ls_type + "%') ";
            }
           

            ls_temp = ls_sql + ls_sqlext;
            hidden_search.Value = ls_temp;
            dw_detail.SetSqlSelect(hidden_search.Value);
            dw_detail.Retrieve();
        }
    }
}
