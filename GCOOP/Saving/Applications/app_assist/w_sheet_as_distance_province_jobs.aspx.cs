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

namespace Saving.Applications.app_assist
{
    public partial class w_sheet_as_distance_province_jobs : PageWebSheet, WebSheet
    {

        #region WebSheet Members

        public void InitJsPostBack()
        {

        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();

            if (!IsPostBack)
            {
                DwAdd.InsertRow(0);
                DwMain.SetTransaction(sqlca);
                DwMain.Retrieve("60000", "67000");
            }
            else
            {
                this.RestoreContextDw(DwMain);
            }

            DwUtil.RetrieveDDDW(DwAdd, "start_province_name", "as_seminar.pbl", null);
            DwUtil.RetrieveDDDW(DwAdd, "end_province_name", "as_seminar.pbl", null);
        }

        public void CheckJsPostBack(string eventArg)
        {

        }

        public void SaveWebSheet()
        {

        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
        }

        #endregion
    }
}
