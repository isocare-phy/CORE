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
    public partial class w_sheet_as_membercard : PageWebSheet, WebSheet
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
                DwMain.InsertRow(0);
                DwCard.SetTransaction(sqlca);
                DwCard.Retrieve(1);
            }
            else
            {
                this.RestoreContextDw(DwCard);
                this.RestoreContextDw(DwMain);
            }
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
            DwCard.SaveDataCache();
        }

        #endregion
    }
}
