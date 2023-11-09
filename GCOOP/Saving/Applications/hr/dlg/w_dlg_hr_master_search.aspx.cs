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
    public partial class w_dlg_hr_master_search : System.Web.UI.Page
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
                String sideid, deptid;

                try
                {
                    if (!IsPostBack)
                    {
                        dw_detail.InsertRow(0);
                        dw_data.InsertRow(0);
                        HSqlTemp.Value = is_sql;
                    }
                    if (Request["emplid"] != null && Request["emplid"].Trim() != "")
                    {
                        //เป็นการ  Retrieve ข้อมูลของ datawindow
                        dw_detail.Retrieve(Request["emplid"].Trim());
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
            string ls_posipid, ls_deptid, ls_sideid;
            string ls_emplid, ls_emplcode, ls_empltypeid;
            string ls_emplfisname, ls_empllastname, ls_brchid;
            string ls_sqlext, ls_temp, ls_branch;
            ls_sqlext = "";
            try
            {
                ls_posipid = dw_data.GetItemString(1, "postname");
            }
            catch
            {
                ls_posipid = "";
            }
            try
            {
                ls_deptid = dw_data.GetItemString(1, "deptname");
            }
            catch
            {
                ls_deptid = "";
            }
            try
            {
                ls_sideid = dw_data.GetItemString(1, "sidename");
            }
            catch
            {
                ls_sideid = "";
            }
            try
            {
                ls_emplfisname = dw_data.GetItemString(1, "emplfirsname");
            }
            catch
            {
                ls_emplfisname = "";
            }
            try
            {
                ls_empllastname = dw_data.GetItemString(1, "empllastname");
            }
            catch
            {
                ls_empllastname = "";
            }
            try
            {
                ls_brchid = dw_data.GetItemString(1, "brchid");
            }
            catch
            {
                ls_brchid = "";
            }
            try
            {
                ls_emplid = dw_data.GetItemString(1, "emplid");
            }
            catch
            {
                ls_emplid = "";
            }
            try
            {
                ls_emplcode = dw_data.GetItemString(1, "emplcode");
            }
            catch
            {
                ls_emplcode = "";
            }
            try
            {
                ls_empltypeid = dw_data.GetItemString(1, "empltypeid");
            }
            catch
            {
                ls_empltypeid = "";
            }
            //--
            if (ls_posipid.Length > 0)
            {
                ls_sqlext = " and ( hrnmlemplfilemas.postpid = '" + ls_posipid + "') ";
            }
            if (ls_deptid.Length > 0)
            {
                ls_sqlext += " and ( hrnmlemplfilemas.deptid = '" + ls_deptid + "') ";
            }
            if (ls_sideid.Length > 0)
            {
                ls_sqlext += " and ( hrnmlemplfilemas.sideid =  '" + ls_sideid + "') " + " and ( hrnmlemplfilemas.deptid = '" + ls_deptid + "') ";
            }
            if (ls_emplid.Length > 0)
            {
                ls_sqlext += " and ( hrnmlemplfilemas.emplid = '" + ls_emplid + "') ";
            }
            if (ls_emplcode.Length > 0)
            {
                ls_sqlext += " and ( hrnmlemplfilemas.emplcode = '" + ls_emplcode + "' ) ";
            }
            if (ls_empltypeid.Length > 0)
            {
                ls_sqlext += " and ( hrnmlemplfilemas.empltypeid = '" + ls_empltypeid + "') ";
            }
            if (ls_emplfisname.Length > 0)
            {
                ls_sqlext += " and ( hrnmlemplfilemas.emplfirsname like  '%" + ls_emplfisname + "%') ";
            }
            if (ls_empllastname.Length > 0)
            {
                ls_sqlext += " and ( hrnmlemplfilemas.empllastname like '%" + ls_empllastname + "%') ";
            }
            if (ls_brchid.Length > 0)
            {
                ls_sqlext += " and ( hrnmlemplfilemas.brchid = '" + ls_brchid + "') ";
            }
            if (ls_sqlext == null) ls_sqlext = "";
            ls_temp = is_sql + ls_sqlext;
            HSqlTemp.Value = ls_temp;
            dw_detail.SetSqlSelect(HSqlTemp.Value);
            dw_detail.Retrieve();
        }
    
    }
}
