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
using DataLibrary;
using System.Data.OracleClient;

namespace Saving.Applications.cmd.dlg
{

    public partial class w_dlg_cmd_ptdurtslipin : PageWebDialog, WebDialog
    {
        private string is_sql;

        #region WebDialog Members

        public void InitJsPostBack()
        {
            
        }

        public void WebDialogLoadBegin()
        {
            this.ConnectSQLCA();
            DwDetail.SetTransaction(sqlca);
            is_sql = DwDetail.GetSqlSelect();

            if (!IsPostBack)
            {
                DwDetail.Retrieve();
            }
            else
            {
                DwDetail.RestoreContext();
            }

        }

        public void CheckJsPostBack(string eventArg)
        {

        }

        public void WebDialogLoadEnd()
        {
            DwDetail.SaveDataCache();
            //this.DisConnectSQLCA();
        }

        #endregion
    }
}
