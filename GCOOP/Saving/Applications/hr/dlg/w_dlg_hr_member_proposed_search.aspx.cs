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
using Sybase.DataWindow;

namespace Saving.Applications.hr.dlg
{
    public partial class w_dlg_hr_member_proposed_search : System.Web.UI.Page
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
                dw_detail.SetTransaction(sqlca);
                //dw_data.SetTransaction(sqlca);
                is_sql = dw_detail.GetSqlSelect();
                String seq_empid; 

                try
                {
                    if (!IsPostBack)
                    {
                        dw_detail.InsertRow(0);
                        dw_data.InsertRow(0); 
                        HSqlTemp.Value = is_sql;
                    }
                    if (Request["seq_empid"] != null && Request["seq_empid"].Trim() != "")
                    {
                        //เป็นการ  Retrieve ข้อมูลของ datawindow
                        dw_detail.Retrieve(Request["seq_empid"].Trim());
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

        protected void Page_LoadComplete()
        {
            try
            {
                sqlca.Disconnect();
            }
            catch { }
        }

        protected void BSearch_Click(object sender, EventArgs e)
        {
            string member_reqdate, member_mbnumber, member_empcard;
            string member_reqeat, member_name;
           
            string ls_sqlext, ls_temp, ls_branch; 
            ls_sqlext = "";
            try
            {
                member_reqdate = dw_data.GetItemString(1, "member_reqdate");
            }
            catch
            {
                member_reqdate = "";
            }
            try
            {
                member_mbnumber = dw_data.GetItemString(1, "member_mbnumber");
            }
            catch
            {
                member_mbnumber = "";
            }
            try
            {
                member_empcard = dw_data.GetItemString(1, "member_empcard");
            }
            catch
            {
                member_empcard = "";
            }
            try
            {
                member_reqeat = dw_data.GetItemString(1, "member_reqeat");
            }
            catch
            {
                member_reqeat = "";
            }
            try
            {
                member_name = dw_data.GetItemString(1, "member_name");
            }
            catch
            {
                member_name = "";
            }
           
            //--
            if (member_reqdate.Length > 0)
            {
                ls_sqlext = " and ( MBREQAPPMEMBER_BF.member_reqdate = '" + member_reqdate + "') ";
            }
            if (member_mbnumber.Length > 0)
            {
                ls_sqlext += " and ( MBREQAPPMEMBER_BF.member_mbnumber = '" + member_mbnumber + "') ";
            }

            if (member_empcard.Length > 0)
            {
                ls_sqlext += " and ( MBREQAPPMEMBER_BF.member_empcard = '" + member_empcard + "') ";
            }
            if (member_reqeat.Length > 0)
            {
                ls_sqlext += " and ( MBREQAPPMEMBER_BF.member_reqeat = '" + member_reqeat + "' ) ";
            }
            if (member_name.Length > 0)
            {
                ls_sqlext += " and ( MBREQAPPMEMBER_BF.member_name = '" + member_name + "') "; 
            }
           
            if (ls_sqlext == null) ls_sqlext = "";
            ls_temp = is_sql + ls_sqlext;
            HSqlTemp.Value = ls_temp;
            dw_detail.SetSqlSelect(HSqlTemp.Value); 
            dw_detail.Retrieve();
        }

    }
}